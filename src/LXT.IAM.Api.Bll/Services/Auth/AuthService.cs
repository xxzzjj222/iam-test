using LXT.IAM.Api.Bll.Services.Auth.Dtos;
using LXT.IAM.Api.Bll.Services.SocialAuth;
using LXT.IAM.Api.Bll.Services.SocialAuth.Dtos;
using LXT.IAM.Api.Common.Consts;
using LXT.IAM.Api.Common.Exceptions;
using LXT.IAM.Api.Common.Helper;
using LXT.IAM.Api.Common.Intefaces;
using LXT.IAM.Api.Storage.Context;
using LXT.IAM.Api.Storage.Entity;
using Microsoft.EntityFrameworkCore;

namespace LXT.IAM.Api.Bll.Services.Auth;

/// <summary>
/// 认证业务服务
/// </summary>
public class AuthService : IAuthService
{
    private readonly IAMDbContext _db;
    private readonly JwtTokenHelper _jwtTokenHelper;
    private readonly IHttpContextUtility _httpContextUtility;
    private readonly IWeChatMiniAppAuthService _weChatMiniAppAuthService;
    private readonly IDouyinMiniAppAuthService _douyinMiniAppAuthService;

    /// <summary>
    /// 构造
    /// </summary>
    public AuthService(
        IAMDbContext db,
        JwtTokenHelper jwtTokenHelper,
        IHttpContextUtility httpContextUtility,
        IWeChatMiniAppAuthService weChatMiniAppAuthService,
        IDouyinMiniAppAuthService douyinMiniAppAuthService)
    {
        _db = db;
        _jwtTokenHelper = jwtTokenHelper;
        _httpContextUtility = httpContextUtility;
        _weChatMiniAppAuthService = weChatMiniAppAuthService;
        _douyinMiniAppAuthService = douyinMiniAppAuthService;
    }

    /// <summary>
    /// 账号密码登录
    /// </summary>
    public async Task<LoginOutput> LoginByPasswordAsync(LoginByPasswordInput input)
    {
        var identifier = await FindIdentifierAsync(input.AccountType, input.Account, input.CountryCode);
        if (identifier == null)
        {
            throw new NotFoundException("账号不存在");
        }

        var user = await _db.CommonUser.FirstAsync(x => x.CommonUserId == identifier.CommonUserId);
        if (user.IsFrozen)
        {
            throw new InvalidParameterException("账号已冻结");
        }

        var credential = await _db.UserCredential.FirstOrDefaultAsync(x =>
            x.CommonUserId == identifier.CommonUserId &&
            x.CredentialType == AuthConst.CredentialTypePassword);
        if (credential == null || !PasswordHelper.VerifyPassword(input.Password, credential.PasswordHash))
        {
            throw new UnauthorizedException("账号或密码错误");
        }

        return await CreateLoginOutputAsync(user, input.AppCode, input.ClientType, AuthConst.LoginTypePassword);
    }

    /// <summary>
    /// 验证码登录
    /// </summary>
    public async Task<LoginOutput> LoginByCodeAsync(LoginByCodeInput input)
    {
        var identifier = await FindIdentifierAsync(input.AccountType, input.Account, input.CountryCode);
        if (identifier == null)
        {
            throw new NotFoundException("账号不存在");
        }

        var verifyCode = await GetAvailableVerifyCodeAsync(input.Account, input.AccountType, AuthConst.VerifySceneLogin, input.VerifyCode);
        if (verifyCode == null)
        {
            throw new InvalidParameterException("验证码无效");
        }

        await UseVerifyCodeAsync(verifyCode);
        var user = await _db.CommonUser.FirstAsync(x => x.CommonUserId == identifier.CommonUserId);
        if (user.IsFrozen)
        {
            throw new InvalidParameterException("账号已冻结");
        }

        return await CreateLoginOutputAsync(user, input.AppCode, input.ClientType, AuthConst.LoginTypeVerifyCode);
    }

    /// <summary>
    /// 微信小程序登录
    /// </summary>
    public async Task<LoginOutput> LoginByWeChatMiniAppAsync(WeChatMiniAppLoginInput input)
    {
        var session = await _weChatMiniAppAuthService.Code2SessionAsync(input.WeChatCode);
        var phoneInfo = await _weChatMiniAppAuthService.DecodePhoneAsync(new DecodePhoneInput
        {
            SessionKey = session.SessionKey,
            IV = input.IV,
            EncryptedData = input.EncryptedData
        });

        return await LoginOrRegisterByPhoneAsync(
            phoneInfo.PhoneNumber,
            phoneInfo.CountryCode,
            input.AppCode,
            input.ClientType,
            AuthConst.LoginTypeVerifyCode,
            "wechat_miniapp",
            session.OpenId,
            input.InviteCode);
    }

    /// <summary>
    /// 抖音小程序登录
    /// </summary>
    public async Task<LoginOutput> LoginByDouyinMiniAppAsync(DouyinMiniAppLoginInput input)
    {
        var session = await _douyinMiniAppAuthService.Code2SessionAsync(input.Code);
        var phoneInfo = await _weChatMiniAppAuthService.DecodePhoneAsync(new DecodePhoneInput
        {
            SessionKey = session.SessionKey,
            IV = input.IV,
            EncryptedData = input.EncryptedData
        });

        return await LoginOrRegisterByPhoneAsync(
            phoneInfo.PhoneNumber,
            phoneInfo.CountryCode,
            input.AppCode,
            input.ClientType,
            AuthConst.LoginTypeVerifyCode,
            "douyin_miniapp",
            session.OpenId,
            input.InviteCode);
    }

    /// <summary>
    /// 注册
    /// </summary>
    public async Task<LoginOutput> RegisterAsync(RegisterInput input)
    {
        var existingIdentifier = await FindIdentifierAsync(input.AccountType, input.Account, input.CountryCode);
        if (existingIdentifier != null)
        {
            throw new InvalidParameterException("账号已存在");
        }

        var verifyCode = await GetAvailableVerifyCodeAsync(input.Account, input.AccountType, AuthConst.VerifySceneRegister, input.VerifyCode);
        if (verifyCode == null)
        {
            throw new InvalidParameterException("验证码无效");
        }

        var commonUserId = Guid.NewGuid();
        var now = DateTime.UtcNow;
        var user = new CommonUserEntity
        {
            Id = Yitter.IdGenerator.YitIdHelper.NextId(),
            CommonUserId = commonUserId,
            Name = BuildDefaultName(input.AccountType, input.Account),
            Status = CommonStatusConst.Enabled,
            IsFrozen = false,
            RegisterAppCode = input.AppCode,
            LastLoginTime = now,
            LastActiveTime = now,
            LanguageCode = input.LanguageCode,
            CountryCode = input.CountryCode,
            Phone = input.AccountType == AuthConst.AccountTypePhone ? input.Account : null,
            Email = input.AccountType == AuthConst.AccountTypeEmail ? input.Account.Trim().ToLowerInvariant() : null
        };

        var identifier = new UserIdentifierEntity
        {
            Id = Yitter.IdGenerator.YitIdHelper.NextId(),
            CommonUserId = commonUserId,
            IdentifierType = input.AccountType,
            Identifier = NormalizeIdentifier(input.AccountType, input.Account),
            CountryCode = input.CountryCode,
            IsPrimary = true,
            IsVerified = true,
            VerifiedTime = now
        };

        _db.CommonUser.Add(user);
        _db.UserIdentifier.Add(identifier);

        if (!string.IsNullOrWhiteSpace(input.Password))
        {
            _db.UserCredential.Add(new UserCredentialEntity
            {
                Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                CommonUserId = commonUserId,
                CredentialType = AuthConst.CredentialTypePassword,
                PasswordHash = PasswordHelper.HashPassword(input.Password),
                PasswordVersion = "bcrypt",
                NeedResetPassword = false,
                LastPasswordChangeTime = now
            });
        }

        var grantApps = await _db.App.Where(x => x.Status == CommonStatusConst.Enabled && x.AutoGrantForNormalUser).ToListAsync();
        foreach (var app in grantApps)
        {
            _db.UserApp.Add(new UserAppEntity
            {
                Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                CommonUserId = commonUserId,
                AppId = app.Id,
                GrantType = AuthConst.UserAppGrantTypeAuto,
                Status = CommonStatusConst.Enabled
            });
        }

        _db.InviteCode.Add(new InviteCodeEntity
        {
            Id = Yitter.IdGenerator.YitIdHelper.NextId(),
            CommonUserId = commonUserId,
            Code = InviteCodeHelper.GenerateUniversalCode(user.Id),
            CodeType = AuthConst.CodeTypeUniversal,
            IsDefault = true,
            Status = CommonStatusConst.Enabled
        });

        if (!string.IsNullOrWhiteSpace(input.InviteCode))
        {
            var inviterCode = await _db.InviteCode.FirstOrDefaultAsync(x => x.Code == input.InviteCode && x.Status == CommonStatusConst.Enabled);
            if (inviterCode != null)
            {
                _db.InviteRelation.Add(new InviteRelationEntity
                {
                    Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                    InviterUserId = inviterCode.CommonUserId,
                    InviteeUserId = commonUserId,
                    InviteCodeId = inviterCode.Id,
                    SourceAppCode = inviterCode.AppCode ?? input.AppCode,
                    RegisterAppCode = input.AppCode,
                    ResolvedBizRoleCode = inviterCode.BizRoleCode,
                    ResolvedExternalRefId = inviterCode.ExternalRefId,
                    BindTime = now
                });
            }
        }

        await UseVerifyCodeAsync(verifyCode);
        await _db.SaveChangesAsync();

        return await CreateLoginOutputAsync(user, input.AppCode, AuthConst.LoginTypeRegister, AuthConst.LoginTypeRegister);
    }

    /// <summary>
    /// 刷新令牌
    /// </summary>
    public async Task<LoginOutput> RefreshTokenAsync(RefreshTokenInput input)
    {
        var refreshTokenHash = SecurityHelper.Sha256(input.RefreshToken);
        var session = await _db.LoginSession.FirstOrDefaultAsync(x =>
            x.RefreshTokenHash == refreshTokenHash &&
            x.Status == CommonStatusConst.SessionActive &&
            x.RefreshTokenExpireTime > DateTime.UtcNow);
        if (session == null)
        {
            throw new UnauthorizedException("refreshToken无效");
        }

        var user = await _db.CommonUser.FirstAsync(x => x.CommonUserId == session.CommonUserId);
        var platformRoles = await GetPlatformRolesAsync(user.CommonUserId);
        var accessTokenExpireTime = DateTime.UtcNow.AddHours(2);
        var accessToken = _jwtTokenHelper.GenerateAccessToken(user.CommonUserId, user.Name, user.Phone, user.Email, session.SessionId, session.AppCode, platformRoles, 2);

        session.AccessTokenExpireTime = accessTokenExpireTime;
        user.LastLoginTime = DateTime.UtcNow;
        user.LastActiveTime = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return new LoginOutput
        {
            CommonUserId = user.CommonUserId,
            Name = user.Name,
            AccessToken = accessToken,
            RefreshToken = input.RefreshToken,
            AccessTokenExpireTime = accessTokenExpireTime,
            RefreshTokenExpireTime = session.RefreshTokenExpireTime
        };
    }

    /// <summary>
    /// 获取当前用户信息
    /// </summary>
    public async Task<CurrentUserOutput> GetCurrentUserAsync()
    {
        var commonUserId = _httpContextUtility.GetUserId();
        var user = await _db.CommonUser.FirstAsync(x => x.CommonUserId == commonUserId);
        var inviteCodes = await _db.InviteCode.Where(x => x.CommonUserId == commonUserId && x.Status == CommonStatusConst.Enabled).Select(x => x.Code).ToListAsync();
        return new CurrentUserOutput
        {
            CommonUserId = commonUserId,
            Name = user.Name,
            Phone = user.Phone,
            Email = user.Email,
            InviteCodes = inviteCodes
        };
    }

    /// <summary>
    /// 修改当前用户密码
    /// </summary>
    public async Task ChangePasswordAsync(ChangePasswordInput input)
    {
        var commonUserId = _httpContextUtility.GetUserId();
        var credential = await _db.UserCredential.FirstOrDefaultAsync(x =>
            x.CommonUserId == commonUserId &&
            x.CredentialType == AuthConst.CredentialTypePassword);
        if (credential == null)
        {
            throw new InvalidParameterException("当前用户未设置密码");
        }

        if (!PasswordHelper.VerifyPassword(input.OldPassword, credential.PasswordHash))
        {
            throw new InvalidParameterException("原密码错误");
        }

        credential.PasswordHash = PasswordHelper.HashPassword(input.NewPassword);
        credential.PasswordVersion = "bcrypt";
        credential.NeedResetPassword = false;
        credential.LastPasswordChangeTime = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    private async Task<UserIdentifierEntity?> FindIdentifierAsync(string accountType, string account, string? countryCode)
    {
        var normalizedIdentifier = NormalizeIdentifier(accountType, account);
        return await _db.UserIdentifier.FirstOrDefaultAsync(x =>
            x.IdentifierType == accountType &&
            x.Identifier == normalizedIdentifier &&
            (accountType != AuthConst.AccountTypePhone || x.CountryCode == countryCode));
    }

    private async Task<VerificationCodeEntity?> GetAvailableVerifyCodeAsync(string account, string accountType, string sceneCode, string plainCode)
    {
        var codeHash = SecurityHelper.Sha256(plainCode);
        return await _db.VerificationCode
            .Where(x => x.Receiver == NormalizeIdentifier(accountType, account)
                        && x.ReceiverType == accountType
                        && x.SceneCode == sceneCode
                        && x.CodeHash == codeHash
                        && x.Status == CommonStatusConst.VerifyCodeUnused
                        && x.ExpireTime > DateTime.UtcNow)
            .OrderByDescending(x => x.CreateTime)
            .FirstOrDefaultAsync();
    }

    private async Task UseVerifyCodeAsync(VerificationCodeEntity verifyCode)
    {
        verifyCode.Status = CommonStatusConst.VerifyCodeUsed;
        verifyCode.UsedTime = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    private async Task<LoginOutput> CreateLoginOutputAsync(CommonUserEntity user, string appCode, string clientType, string loginType)
    {
        var sessionId = Guid.NewGuid();
        var refreshToken = SecurityHelper.GenerateTokenString();
        var refreshTokenHash = SecurityHelper.Sha256(refreshToken);
        var accessTokenExpireTime = DateTime.UtcNow.AddHours(2);
        var refreshTokenExpireTime = DateTime.UtcNow.AddDays(7);
        var platformRoles = await GetPlatformRolesAsync(user.CommonUserId);
        var accessToken = _jwtTokenHelper.GenerateAccessToken(user.CommonUserId, user.Name, user.Phone, user.Email, sessionId, appCode, platformRoles, 2);

        _db.LoginSession.Add(new LoginSessionEntity
        {
            Id = Yitter.IdGenerator.YitIdHelper.NextId(),
            SessionId = sessionId,
            CommonUserId = user.CommonUserId,
            AppCode = appCode,
            ClientType = clientType,
            LoginType = loginType,
            RefreshTokenHash = refreshTokenHash,
            AccessTokenExpireTime = accessTokenExpireTime,
            RefreshTokenExpireTime = refreshTokenExpireTime,
            Status = CommonStatusConst.SessionActive
        });

        user.LastLoginTime = DateTime.UtcNow;
        user.LastActiveTime = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return new LoginOutput
        {
            CommonUserId = user.CommonUserId,
            Name = user.Name,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpireTime = accessTokenExpireTime,
            RefreshTokenExpireTime = refreshTokenExpireTime
        };
    }

    private async Task<List<string>> GetPlatformRolesAsync(Guid commonUserId)
    {
        return await (from upr in _db.UserPlatformRole
                      join pr in _db.PlatformRole on upr.RoleId equals pr.Id
                      where upr.CommonUserId == commonUserId
                      select pr.Code).ToListAsync();
    }

    private static string NormalizeIdentifier(string accountType, string account)
    {
        return accountType == AuthConst.AccountTypeEmail ? account.Trim().ToLowerInvariant() : account.Trim();
    }

    private static string BuildDefaultName(string accountType, string account)
    {
        if (accountType == AuthConst.AccountTypePhone && account.Length >= 4)
        {
            return $"用户{account[^4..]}";
        }

        var name = account.Split('@').FirstOrDefault();
        return string.IsNullOrWhiteSpace(name) ? "新用户" : name;
    }

    private async Task<LoginOutput> LoginOrRegisterByPhoneAsync(
        string phone,
        string? countryCode,
        string appCode,
        string clientType,
        string loginType,
        string platformType,
        string openId,
        string? inviteCode)
    {
        var identifier = await FindIdentifierAsync(AuthConst.AccountTypePhone, phone, countryCode);
        CommonUserEntity user;
        var now = DateTime.UtcNow;

        if (identifier == null)
        {
            var commonUserId = Guid.NewGuid();
            user = new CommonUserEntity
            {
                Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                CommonUserId = commonUserId,
                Name = BuildDefaultName(AuthConst.AccountTypePhone, phone),
                Status = CommonStatusConst.Enabled,
                IsFrozen = false,
                RegisterAppCode = appCode,
                LastLoginTime = now,
                LastActiveTime = now,
                CountryCode = countryCode,
                Phone = phone
            };
            _db.CommonUser.Add(user);
            _db.UserIdentifier.Add(new UserIdentifierEntity
            {
                Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                CommonUserId = commonUserId,
                IdentifierType = AuthConst.AccountTypePhone,
                Identifier = phone.Trim(),
                CountryCode = countryCode,
                IsPrimary = true,
                IsVerified = true,
                VerifiedTime = now
            });

            var grantApps = await _db.App.Where(x => x.Status == CommonStatusConst.Enabled && x.AutoGrantForNormalUser).ToListAsync();
            foreach (var app in grantApps)
            {
                _db.UserApp.Add(new UserAppEntity
                {
                    Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                    CommonUserId = commonUserId,
                    AppId = app.Id,
                    GrantType = AuthConst.UserAppGrantTypeAuto,
                    Status = CommonStatusConst.Enabled
                });
            }

            _db.InviteCode.Add(new InviteCodeEntity
            {
                Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                CommonUserId = commonUserId,
                Code = InviteCodeHelper.GenerateUniversalCode(user.Id),
                CodeType = AuthConst.CodeTypeUniversal,
                IsDefault = true,
                Status = CommonStatusConst.Enabled
            });

            if (!string.IsNullOrWhiteSpace(inviteCode))
            {
                var inviterCode = await _db.InviteCode.FirstOrDefaultAsync(x => x.Code == inviteCode && x.Status == CommonStatusConst.Enabled);
                if (inviterCode != null)
                {
                    _db.InviteRelation.Add(new InviteRelationEntity
                    {
                        Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                        InviterUserId = inviterCode.CommonUserId,
                        InviteeUserId = commonUserId,
                        InviteCodeId = inviterCode.Id,
                        SourceAppCode = inviterCode.AppCode ?? appCode,
                        RegisterAppCode = appCode,
                        ResolvedBizRoleCode = inviterCode.BizRoleCode,
                        ResolvedExternalRefId = inviterCode.ExternalRefId,
                        BindTime = now
                    });
                }
            }
        }
        else
        {
            user = await _db.CommonUser.FirstAsync(x => x.CommonUserId == identifier.CommonUserId);
        }

        if (user.IsFrozen)
        {
            throw new InvalidParameterException("账号已冻结");
        }

        await UpsertSocialAccountAsync(user.CommonUserId, platformType, openId);
        await _db.SaveChangesAsync();

        return await CreateLoginOutputAsync(user, appCode, clientType, loginType);
    }

    private async Task UpsertSocialAccountAsync(Guid commonUserId, string platformType, string openId)
    {
        var entity = await _db.UserSocialAccount.FirstOrDefaultAsync(x =>
            x.CommonUserId == commonUserId &&
            x.PlatformType == platformType &&
            x.OpenId == openId);

        if (entity == null)
        {
            _db.UserSocialAccount.Add(new UserSocialAccountEntity
            {
                Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                CommonUserId = commonUserId,
                PlatformType = platformType,
                AppId = string.Empty,
                OpenId = openId,
                Status = CommonStatusConst.Enabled,
                BindTime = DateTime.UtcNow
            });
        }
        else
        {
            entity.Status = CommonStatusConst.Enabled;
            entity.BindTime = DateTime.UtcNow;
        }
    }
}
