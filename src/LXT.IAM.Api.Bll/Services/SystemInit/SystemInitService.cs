using LXT.IAM.Api.Bll.Services.SystemInit.Dtos;
using LXT.IAM.Api.Common.Consts;
using LXT.IAM.Api.Common.Helper;
using LXT.IAM.Api.Storage.Context;
using LXT.IAM.Api.Storage.Entity;
using Microsoft.EntityFrameworkCore;

namespace LXT.IAM.Api.Bll.Services.SystemInit;

public class SystemInitService : ISystemInitService
{
    private readonly IAMDbContext _db;

    public SystemInitService(IAMDbContext db)
    {
        _db = db;
    }

    public async Task<InitializeSystemOutput> InitializeAsync(InitializeSystemInput input)
    {
        var normalizedAccount = input.AdminAccountType == AuthConst.AccountTypeEmail
            ? input.AdminAccount.Trim().ToLowerInvariant()
            : input.AdminAccount.Trim();

        var identifier = await _db.UserIdentifier.FirstOrDefaultAsync(x =>
            x.IdentifierType == input.AdminAccountType &&
            x.Identifier == normalizedAccount &&
            (input.AdminAccountType != AuthConst.AccountTypePhone || x.CountryCode == input.CountryCode));

        if (identifier != null)
        {
            await EnsureSuperAdminRoleAsync(identifier.CommonUserId);
            await _db.SaveChangesAsync();
            return new InitializeSystemOutput
            {
                CommonUserId = identifier.CommonUserId,
                AdminAccount = input.AdminAccount,
                PlatformRoleCode = PlatformConst.PlatformRoleSuperAdmin,
                Created = false
            };
        }

        var commonUserId = Guid.NewGuid();
        var now = DateTime.UtcNow;
        var user = new CommonUserEntity
        {
            Id = Yitter.IdGenerator.YitIdHelper.NextId(),
            CommonUserId = commonUserId,
            Name = input.AdminName,
            Status = CommonStatusConst.Enabled,
            IsFrozen = false,
            RegisterAppCode = "IAM_INIT",
            LastLoginTime = now,
            LastActiveTime = now,
            CountryCode = input.CountryCode,
            Phone = input.AdminAccountType == AuthConst.AccountTypePhone ? input.AdminAccount : null,
            Email = input.AdminAccountType == AuthConst.AccountTypeEmail ? normalizedAccount : null
        };

        _db.CommonUser.Add(user);
        _db.UserIdentifier.Add(new UserIdentifierEntity
        {
            Id = Yitter.IdGenerator.YitIdHelper.NextId(),
            CommonUserId = commonUserId,
            IdentifierType = input.AdminAccountType,
            Identifier = normalizedAccount,
            CountryCode = input.CountryCode,
            IsPrimary = true,
            IsVerified = true,
            VerifiedTime = now
        });
        _db.UserCredential.Add(new UserCredentialEntity
        {
            Id = Yitter.IdGenerator.YitIdHelper.NextId(),
            CommonUserId = commonUserId,
            CredentialType = AuthConst.CredentialTypePassword,
            PasswordHash = PasswordHelper.HashPassword(input.AdminPassword),
            PasswordVersion = "bcrypt",
            NeedResetPassword = false,
            LastPasswordChangeTime = now
        });
        _db.InviteCode.Add(new InviteCodeEntity
        {
            Id = Yitter.IdGenerator.YitIdHelper.NextId(),
            CommonUserId = commonUserId,
            Code = InviteCodeHelper.GenerateUniversalCode(user.Id),
            CodeType = AuthConst.CodeTypeUniversal,
            IsDefault = true,
            Status = CommonStatusConst.Enabled
        });

        await EnsureSuperAdminRoleAsync(commonUserId);
        await _db.SaveChangesAsync();

        return new InitializeSystemOutput
        {
            CommonUserId = commonUserId,
            AdminAccount = input.AdminAccount,
            PlatformRoleCode = PlatformConst.PlatformRoleSuperAdmin,
            Created = true
        };
    }

    private async Task EnsureSuperAdminRoleAsync(Guid commonUserId)
    {
        var superAdminRole = await _db.PlatformRole.FirstAsync(x => x.Code == PlatformConst.PlatformRoleSuperAdmin);
        var exists = await _db.UserPlatformRole.AnyAsync(x => x.CommonUserId == commonUserId && x.RoleId == superAdminRole.Id);
        if (!exists)
        {
            _db.UserPlatformRole.Add(new UserPlatformRoleEntity
            {
                Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                CommonUserId = commonUserId,
                RoleId = superAdminRole.Id
            });
        }
    }
}
