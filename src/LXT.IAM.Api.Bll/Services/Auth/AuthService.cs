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
/// 认证业务服务。
/// </summary>
public class AuthService : IAuthService
{
    private readonly IAMDbContext _db;
    private readonly JwtTokenHelper _jwtTokenHelper;
    private readonly IHttpContextUtility _httpContextUtility;
    private readonly IWeChatMiniAppAuthService _weChatMiniAppAuthService;
    private readonly IDouyinMiniAppAuthService _douyinMiniAppAuthService;

    /// <summary>
    /// 构造函数。
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
    /// 账号密码登录。
    /// </summary>
    public async Task<LoginOutput> LoginByPasswordAsync(LoginByPasswordInput input)
    {
        var identifier = await FindIdentifierAsync(input.AccountType, input.Account, input.CountryCode);
        if (identifier == null)
        {
            throw new NotFoundException("账号不存在");
        }

        var user = await _db.User.FirstAsync(x => x.UserId == identifier.UserId);
        if (user.IsFrozen)
        {
            throw new InvalidParameterException("账号已冻结");
        }

        var credential = await _db.UserCredential.FirstOrDefaultAsync(x =>
            x.UserId == identifier.UserId &&
            x.CredentialType == AuthConst.CredentialTypePassword);
        if (credential == null || !PasswordHelper.VerifyPassword(input.Password, credential.PasswordHash))
        {
            throw new UnauthorizedException("账号或密码错误");
        }

        return await CreateLoginOutputAsync(user, input.AppCode, input.ClientType, AuthConst.LoginTypePassword);
    }

    /// <summary>
    /// 验证码登录。
    /// </summary>
    public async Task<LoginOutput> LoginByCodeAsync(LoginByCodeInput input)
    {
        var identifier = await FindIdentifierAsync(input.AccountType, input.Account, input.CountryCode);
        if (identifier == null)
        {
            throw new NotFoundException("账号不存在");
        }

        var sceneCode = GetVerifySceneCode(input.AccountType, true);
        var verifyCode = await GetAvailableVerifyCodeAsync(input.Account, input.AccountType, input.CountryCode, sceneCode, input.VerifyCode);
        if (verifyCode == null)
        {
            throw new InvalidParameterException("验证码无效");
        }

        MarkVerifyCodeAsUsed(verifyCode);

        var user = await _db.User.FirstAsync(x => x.UserId == identifier.UserId);
        if (user.IsFrozen)
        {
            throw new InvalidParameterException("账号已冻结");
        }

        return await CreateLoginOutputAsync(user, input.AppCode, input.ClientType, AuthConst.LoginTypeVerifyCode);
    }

    /// <summary>
    /// 微信小程序登录。
    /// </summary>
    public async Task<LoginOutput> LoginByWeChatMiniAppAsync(WeChatMiniAppLoginInput input)
    {
        var session = await _weChatMiniAppAuthService.Code2SessionAsync(input.WeChatCode);
        if (string.IsNullOrWhiteSpace(session.OpenId))
        {
            throw new UnauthorizedException("微信授权失败");
        }

        var phoneInfo = await _weChatMiniAppAuthService.DecodePhoneAsync(new DecodePhoneInput
        {
            SessionKey = session.SessionKey,
            IV = input.IV,
            EncryptedData = input.EncryptedData
        });
        if (string.IsNullOrWhiteSpace(phoneInfo.PhoneNumber))
        {
            throw new UnauthorizedException("微信手机号解密失败");
        }

        return await LoginOrRegisterByPhoneAsync(
            phoneInfo.PhoneNumber,
            phoneInfo.CountryCode,
            input.AppCode,
            input.ClientType,
            "wechat_miniapp",
            "wechat_miniapp",
            session.OpenId,
            input.InviteCode);
    }

    /// <summary>
    /// 抖音小程序登录。
    /// </summary>
    public async Task<LoginOutput> LoginByDouyinMiniAppAsync(DouyinMiniAppLoginInput input)
    {
        var session = await _douyinMiniAppAuthService.Code2SessionAsync(input.Code);
        if (string.IsNullOrWhiteSpace(session.OpenId))
        {
            throw new UnauthorizedException("抖音授权失败");
        }

        var phoneInfo = await _weChatMiniAppAuthService.DecodePhoneAsync(new DecodePhoneInput
        {
            SessionKey = session.SessionKey,
            IV = input.IV,
            EncryptedData = input.EncryptedData
        });
        if (string.IsNullOrWhiteSpace(phoneInfo.PhoneNumber))
        {
            throw new UnauthorizedException("抖音手机号解密失败");
        }

        return await LoginOrRegisterByPhoneAsync(
            phoneInfo.PhoneNumber,
            phoneInfo.CountryCode,
            input.AppCode,
            input.ClientType,
            "douyin_miniapp",
            "douyin_miniapp",
            session.OpenId,
            input.InviteCode);
    }

    /// <summary>
    /// 注册。
    /// </summary>
    public async Task<LoginOutput> RegisterAsync(RegisterInput input)
    {
        var existingIdentifier = await FindIdentifierAsync(input.AccountType, input.Account, input.CountryCode);
        if (existingIdentifier != null)
        {
            throw new InvalidParameterException("账号已存在");
        }

        var sceneCode = GetVerifySceneCode(input.AccountType, false);
        var verifyCode = await GetAvailableVerifyCodeAsync(input.Account, input.AccountType, input.CountryCode, sceneCode, input.VerifyCode);
        if (verifyCode == null)
        {
            throw new InvalidParameterException("验证码无效");
        }

        var userId = Guid.NewGuid();
        var now = DateTime.UtcNow;
        var user = new UserEntity
        {
            Id = Yitter.IdGenerator.YitIdHelper.NextId(),
            UserId = userId,
            Name = BuildDefaultName(input.AccountType, input.Account),
            Status = CommonStatusConst.Enabled,
            IsFrozen = false,
            RegisterAppCode = input.AppCode,
            LastLoginTime = now,
            LastActiveTime = now,
            LanguageCode = input.LanguageCode,
            CountryCode = input.CountryCode,
            Phone = input.AccountType == AuthConst.AccountTypePhone ? input.Account.Trim() : null,
            Email = input.AccountType == AuthConst.AccountTypeEmail ? input.Account.Trim().ToLowerInvariant() : null
        };

        _db.User.Add(user);
        _db.UserIdentifier.Add(new UserIdentifierEntity
        {
            Id = Yitter.IdGenerator.YitIdHelper.NextId(),
            UserId = userId,
            IdentifierType = input.AccountType,
            Identifier = NormalizeIdentifier(input.AccountType, input.Account, input.CountryCode),
            CountryCode = NormalizeCountryCode(input.AccountType, input.CountryCode),
            IsPrimary = true,
            IsVerified = true,
            VerifiedTime = now
        });

        if (!string.IsNullOrWhiteSpace(input.Password))
        {
            _db.UserCredential.Add(new UserCredentialEntity
            {
                Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                UserId = userId,
                CredentialType = AuthConst.CredentialTypePassword,
                PasswordHash = PasswordHelper.HashPassword(input.Password),
                PasswordVersion = "bcrypt",
                NeedResetPassword = false,
                LastPasswordChangeTime = now
            });
        }

        await GrantDefaultAppsAsync(userId);
        _db.InviteCode.Add(CreateDefaultInviteCode(userId, user.Id));
        await TryBindInviteRelationAsync(input.InviteCode, userId, input.AppCode, now);

        MarkVerifyCodeAsUsed(verifyCode);

        return await CreateLoginOutputAsync(user, input.AppCode, input.ClientType, AuthConst.LoginTypeRegister);
    }

    /// <summary>
    /// 刷新令牌。
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
            throw new UnauthorizedException("RefreshToken 无效");
        }

        var user = await _db.User.FirstAsync(x => x.UserId == session.UserId);
        if (user.IsFrozen)
        {
            throw new InvalidParameterException("账号已冻结");
        }

        var platformRoles = await GetPlatformRolesAsync(user.UserId);
        var accessTokenExpireTime = DateTime.UtcNow.AddHours(2);
        var accessToken = _jwtTokenHelper.GenerateAccessToken(user.UserId, user.Name, user.Phone, user.Email, session.SessionId, session.AppCode, platformRoles, 2);

        session.AccessTokenExpireTime = accessTokenExpireTime;
        user.LastLoginTime = DateTime.UtcNow;
        user.LastActiveTime = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return new LoginOutput
        {
            UserId = user.UserId,
            Name = user.Name,
            AccessToken = accessToken,
            RefreshToken = input.RefreshToken,
            AccessTokenExpireTime = accessTokenExpireTime,
            RefreshTokenExpireTime = session.RefreshTokenExpireTime
        };
    }

    /// <summary>
    /// 获取当前登录用户信息。
    /// </summary>
    public async Task<CurrentUserOutput> GetCurrentUserAsync()
    {
        var userId = _httpContextUtility.GetUserId();
        var user = await _db.User.FirstAsync(x => x.UserId == userId);
        var inviteCodes = await _db.InviteCode
            .Where(x => x.UserId == userId && x.Status == CommonStatusConst.Enabled)
            .Select(x => x.Code)
            .ToListAsync();

        return new CurrentUserOutput
        {
            UserId = userId,
            Name = user.Name,
            Phone = user.Phone,
            Email = user.Email,
            InviteCodes = inviteCodes
        };
    }

    /// <summary>
    /// 修改当前登录用户密码。
    /// </summary>
    public async Task ChangePasswordAsync(ChangePasswordInput input)
    {
        var userId = _httpContextUtility.GetUserId();
        var credential = await _db.UserCredential.FirstOrDefaultAsync(x =>
            x.UserId == userId &&
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
        var normalizedIdentifier = NormalizeIdentifier(accountType, account, countryCode);
        var normalizedCountryCode = NormalizeCountryCode(accountType, countryCode);

        if (accountType == AuthConst.AccountTypePhone)
        {
            var legacyPhone = account.Trim();
            return await _db.UserIdentifier.FirstOrDefaultAsync(x =>
                x.IdentifierType == accountType &&
                (
                    x.Identifier == normalizedIdentifier ||
                    (x.Identifier == legacyPhone &&
                     (x.CountryCode == normalizedCountryCode ||
                      x.CountryCode == countryCode ||
                      (normalizedCountryCode == string.Empty && (x.CountryCode == null || x.CountryCode == string.Empty))))
                ));
        }

        return await _db.UserIdentifier.FirstOrDefaultAsync(x =>
            x.IdentifierType == accountType &&
            x.Identifier == normalizedIdentifier);
    }

    private async Task<VerificationCodeEntity?> GetAvailableVerifyCodeAsync(
        string account,
        string accountType,
        string? countryCode,
        string sceneCode,
        string plainCode)
    {
        var codeHash = SecurityHelper.Sha256(plainCode);
        var normalizedReceiver = NormalizeReceiver(accountType, account, countryCode);
        var query = _db.VerificationCode.Where(x =>
            x.ReceiverType == accountType &&
            x.SceneCode == sceneCode &&
            x.CodeHash == codeHash &&
            x.Status == CommonStatusConst.VerifyCodeUnused &&
            x.ExpireTime > DateTime.UtcNow);

        if (accountType == AuthConst.AccountTypePhone)
        {
            var legacyPhone = account.Trim();
            query = query.Where(x => x.Receiver == normalizedReceiver || x.Receiver == legacyPhone);
        }
        else
        {
            query = query.Where(x => x.Receiver == normalizedReceiver);
        }

        return await query
            .OrderByDescending(x => x.CreateTime)
            .FirstOrDefaultAsync();
    }

    private static void MarkVerifyCodeAsUsed(VerificationCodeEntity verifyCode)
    {
        verifyCode.Status = CommonStatusConst.VerifyCodeUsed;
        verifyCode.UsedTime = DateTime.UtcNow;
    }

    private async Task<LoginOutput> CreateLoginOutputAsync(UserEntity user, string appCode, string? clientType, string loginType)
    {
        var sessionId = Guid.NewGuid();
        var refreshToken = SecurityHelper.GenerateTokenString();
        var refreshTokenHash = SecurityHelper.Sha256(refreshToken);
        var accessTokenExpireTime = DateTime.UtcNow.AddHours(2);
        var refreshTokenExpireTime = DateTime.UtcNow.AddDays(7);
        var platformRoles = await GetPlatformRolesAsync(user.UserId);
        var accessToken = _jwtTokenHelper.GenerateAccessToken(user.UserId, user.Name, user.Phone, user.Email, sessionId, appCode, platformRoles, 2);

        _db.LoginSession.Add(new LoginSessionEntity
        {
            Id = Yitter.IdGenerator.YitIdHelper.NextId(),
            SessionId = sessionId,
            UserId = user.UserId,
            AppCode = appCode,
            ClientType = clientType ?? string.Empty,
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
            UserId = user.UserId,
            Name = user.Name,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpireTime = accessTokenExpireTime,
            RefreshTokenExpireTime = refreshTokenExpireTime
        };
    }

    private async Task<List<string>> GetPlatformRolesAsync(Guid userId)
    {
        return await (from userPlatformRole in _db.UserPlatformRole
                      join platformRole in _db.PlatformRole on userPlatformRole.RoleId equals platformRole.Id
                      where userPlatformRole.UserId == userId
                      select platformRole.Code)
            .ToListAsync();
    }

    private async Task GrantDefaultAppsAsync(Guid userId)
    {
        var grantApps = await _db.App
            .Where(x => x.Status == CommonStatusConst.Enabled && x.AutoGrantForNormalUser)
            .ToListAsync();

        foreach (var app in grantApps)
        {
            _db.UserApp.Add(new UserAppEntity
            {
                Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                UserId = userId,
                AppId = app.Id,
                GrantType = AuthConst.UserAppGrantTypeAuto,
                Status = CommonStatusConst.Enabled
            });
        }
    }

    private InviteCodeEntity CreateDefaultInviteCode(Guid userId, long sourceId)
    {
        return new InviteCodeEntity
        {
            Id = Yitter.IdGenerator.YitIdHelper.NextId(),
            UserId = userId,
            Code = InviteCodeHelper.GenerateUniversalCode(sourceId),
            CodeType = AuthConst.CodeTypeUniversal,
            IsDefault = true,
            Status = CommonStatusConst.Enabled
        };
    }

    private async Task TryBindInviteRelationAsync(string? inviteCode, Guid userId, string appCode, DateTime now)
    {
        if (string.IsNullOrWhiteSpace(inviteCode))
        {
            return;
        }

        var inviterCode = await _db.InviteCode.FirstOrDefaultAsync(x =>
            x.Code == inviteCode &&
            x.Status == CommonStatusConst.Enabled);
        if (inviterCode == null)
        {
            return;
        }

        _db.InviteRelation.Add(new InviteRelationEntity
        {
            Id = Yitter.IdGenerator.YitIdHelper.NextId(),
            InviterUserId = inviterCode.UserId,
            InviteeUserId = userId,
            InviteCodeId = inviterCode.Id,
            SourceAppCode = inviterCode.AppCode ?? appCode,
            RegisterAppCode = appCode,
            ResolvedBizRoleCode = inviterCode.BizRoleCode,
            ResolvedExternalRefId = inviterCode.ExternalRefId,
            BindTime = now
        });
    }

    private static string GetVerifySceneCode(string accountType, bool isLogin)
    {
        if (accountType == AuthConst.AccountTypeEmail)
        {
            return isLogin ? AuthConst.VerifySceneEmailLogin : AuthConst.VerifySceneEmailRegister;
        }

        return isLogin ? AuthConst.VerifySceneLogin : AuthConst.VerifySceneRegister;
    }

    private static string NormalizeIdentifier(string accountType, string account, string? countryCode)
    {
        if (accountType == AuthConst.AccountTypeEmail)
        {
            return account.Trim().ToLowerInvariant();
        }

        if (accountType == AuthConst.AccountTypePhone)
        {
            return BuildPhoneReceiver(countryCode, account);
        }

        return account.Trim();
    }

    private static string NormalizeReceiver(string accountType, string receiver, string? countryCode)
    {
        return NormalizeIdentifier(accountType, receiver, countryCode);
    }

    private static string NormalizeCountryCode(string accountType, string? countryCode)
    {
        if (accountType != AuthConst.AccountTypePhone)
        {
            return string.Empty;
        }

        return string.IsNullOrWhiteSpace(countryCode)
            ? string.Empty
            : countryCode.Trim().TrimStart('+');
    }

    private static string BuildPhoneReceiver(string? countryCode, string phone)
    {
        var normalizedPhone = phone.Trim();
        var normalizedCountryCode = string.IsNullOrWhiteSpace(countryCode)
            ? string.Empty
            : countryCode.Trim().TrimStart('+');

        return string.IsNullOrWhiteSpace(normalizedCountryCode)
            ? normalizedPhone
            : $"+{normalizedCountryCode}:{normalizedPhone}";
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
        UserEntity user;
        var now = DateTime.UtcNow;

        if (identifier == null)
        {
            var userId = Guid.NewGuid();
            user = new UserEntity
            {
                Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                UserId = userId,
                Name = BuildDefaultName(AuthConst.AccountTypePhone, phone),
                Status = CommonStatusConst.Enabled,
                IsFrozen = false,
                RegisterAppCode = appCode,
                LastLoginTime = now,
                LastActiveTime = now,
                CountryCode = countryCode,
                Phone = phone.Trim()
            };

            _db.User.Add(user);
            _db.UserIdentifier.Add(new UserIdentifierEntity
            {
                Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                UserId = userId,
                IdentifierType = AuthConst.AccountTypePhone,
                Identifier = NormalizeIdentifier(AuthConst.AccountTypePhone, phone, countryCode),
                CountryCode = NormalizeCountryCode(AuthConst.AccountTypePhone, countryCode),
                IsPrimary = true,
                IsVerified = true,
                VerifiedTime = now
            });

            await GrantDefaultAppsAsync(userId);
            _db.InviteCode.Add(CreateDefaultInviteCode(userId, user.Id));
            await TryBindInviteRelationAsync(inviteCode, userId, appCode, now);
        }
        else
        {
            user = await _db.User.FirstAsync(x => x.UserId == identifier.UserId);
        }

        if (user.IsFrozen)
        {
            throw new InvalidParameterException("账号已冻结");
        }

        await UpsertSocialAccountAsync(user.UserId, platformType, openId);

        return await CreateLoginOutputAsync(user, appCode, clientType, loginType);
    }

    private async Task UpsertSocialAccountAsync(Guid userId, string platformType, string openId)
    {
        var entity = await _db.UserSocialAccount.FirstOrDefaultAsync(x =>
            x.PlatformType == platformType &&
            x.AppId == string.Empty &&
            x.OpenId == openId);

        if (entity == null)
        {
            _db.UserSocialAccount.Add(new UserSocialAccountEntity
            {
                Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                UserId = userId,
                PlatformType = platformType,
                AppId = string.Empty,
                OpenId = openId,
                Status = CommonStatusConst.Enabled,
                BindTime = DateTime.UtcNow
            });
            return;
        }

        entity.UserId = userId;
        entity.Status = CommonStatusConst.Enabled;
        entity.BindTime = DateTime.UtcNow;
        entity.UnbindTime = null;
    }
}
