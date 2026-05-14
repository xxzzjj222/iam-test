using LXT.IAM.Api.Bll.Services.SystemInit.Dtos;
using LXT.IAM.Api.Common.Consts;
using LXT.IAM.Api.Common.Helper;
using LXT.IAM.Api.Storage.Context;
using LXT.IAM.Api.Storage.Entity;
using Microsoft.EntityFrameworkCore;

namespace LXT.IAM.Api.Bll.Services.SystemInit;

/// <summary>
/// 系统初始化服务。
/// </summary>
public class SystemInitService : ISystemInitService
{
    private readonly IAMDbContext _db;

    /// <summary>
    /// 构造函数。
    /// </summary>
    public SystemInitService(IAMDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// 初始化系统超级管理员。
    /// </summary>
    public async Task<InitializeSystemOutput> InitializeAsync(InitializeSystemInput input)
    {
        var normalizedAccount = input.AdminAccountType == AuthConst.AccountTypeEmail
            ? input.AdminAccount.Trim().ToLowerInvariant()
            : input.AdminAccount.Trim();
        var normalizedCountryCode = input.AdminAccountType == AuthConst.AccountTypePhone
            ? input.CountryCode?.Trim().TrimStart('+') ?? string.Empty
            : string.Empty;
        var normalizedIdentifier = input.AdminAccountType == AuthConst.AccountTypePhone
            ? string.IsNullOrWhiteSpace(normalizedCountryCode)
                ? normalizedAccount
                : $"+{normalizedCountryCode}:{normalizedAccount}"
            : normalizedAccount;

        var identifier = await _db.UserIdentifier.FirstOrDefaultAsync(x =>
            x.IdentifierType == input.AdminAccountType &&
            (x.Identifier == normalizedIdentifier ||
             (input.AdminAccountType == AuthConst.AccountTypePhone && x.Identifier == normalizedAccount)) &&
            (input.AdminAccountType != AuthConst.AccountTypePhone || x.CountryCode == normalizedCountryCode || x.CountryCode == input.CountryCode || (normalizedCountryCode == string.Empty && (x.CountryCode == null || x.CountryCode == string.Empty))));

        if (identifier != null)
        {
            await EnsureSuperAdminRoleAsync(identifier.UserId);
            await EnsureAllAppAccessAsync(identifier.UserId);
            await EnsureSuperAdminRoleFunctionsAsync();
            await _db.SaveChangesAsync();

            return new InitializeSystemOutput
            {
                UserId = identifier.UserId,
                AdminAccount = input.AdminAccount,
                PlatformRoleCode = PlatformConst.PlatformRoleSuperAdmin,
                Created = false
            };
        }

        var userId = Guid.NewGuid();
        var now = DateTime.UtcNow;
        var user = new UserEntity
        {
            Id = Yitter.IdGenerator.YitIdHelper.NextId(),
            UserId = userId,
            Name = input.AdminName,
            Status = CommonStatusConst.Enabled,
            IsFrozen = false,
            RegisterAppCode = "IAM_INIT",
            LastLoginTime = now,
            LastActiveTime = now,
            CountryCode = input.AdminAccountType == AuthConst.AccountTypePhone ? normalizedCountryCode : null,
            Phone = input.AdminAccountType == AuthConst.AccountTypePhone ? normalizedAccount : null,
            Email = input.AdminAccountType == AuthConst.AccountTypeEmail ? normalizedAccount : null
        };

        _db.User.Add(user);
        _db.UserIdentifier.Add(new UserIdentifierEntity
        {
            Id = Yitter.IdGenerator.YitIdHelper.NextId(),
            UserId = userId,
            IdentifierType = input.AdminAccountType,
            Identifier = normalizedIdentifier,
            CountryCode = input.AdminAccountType == AuthConst.AccountTypePhone ? normalizedCountryCode : string.Empty,
            IsPrimary = true,
            IsVerified = true,
            VerifiedTime = now
        });
        _db.UserCredential.Add(new UserCredentialEntity
        {
            Id = Yitter.IdGenerator.YitIdHelper.NextId(),
            UserId = userId,
            CredentialType = AuthConst.CredentialTypePassword,
            PasswordHash = PasswordHelper.HashPassword(input.AdminPassword),
            PasswordVersion = "bcrypt",
            NeedResetPassword = false,
            LastPasswordChangeTime = now
        });
        _db.InviteCode.Add(new InviteCodeEntity
        {
            Id = Yitter.IdGenerator.YitIdHelper.NextId(),
            UserId = userId,
            Code = InviteCodeHelper.GenerateUniversalCode(user.Id),
            CodeType = AuthConst.CodeTypeUniversal,
            IsDefault = true,
            Status = CommonStatusConst.Enabled
        });

        await EnsureSuperAdminRoleAsync(userId);
        await EnsureAllAppAccessAsync(userId);
        await EnsureSuperAdminRoleFunctionsAsync();
        await _db.SaveChangesAsync();

        return new InitializeSystemOutput
        {
            UserId = userId,
            AdminAccount = input.AdminAccount,
            PlatformRoleCode = PlatformConst.PlatformRoleSuperAdmin,
            Created = true
        };
    }

    /// <summary>
    /// 获取系统初始化状态。
    /// </summary>
    public async Task<SystemInitStatusOutput> GetStatusAsync()
    {
        var hasSuperAdminRole = await _db.PlatformRole.AnyAsync(x => x.Code == PlatformConst.PlatformRoleSuperAdmin);
        var roleId = await _db.PlatformRole
            .Where(x => x.Code == PlatformConst.PlatformRoleSuperAdmin)
            .Select(x => (long?)x.Id)
            .FirstOrDefaultAsync();
        var hasSuperAdminUser = roleId.HasValue && await _db.UserPlatformRole.AnyAsync(x => x.RoleId == roleId.Value);
        var appCount = await _db.App.CountAsync();
        var functionCount = await _db.PlatformFunction.CountAsync();

        return new SystemInitStatusOutput
        {
            Initialized = hasSuperAdminRole && hasSuperAdminUser && appCount > 0,
            HasSuperAdminRole = hasSuperAdminRole,
            HasSuperAdminUser = hasSuperAdminUser,
            AppCount = appCount,
            PlatformFunctionCount = functionCount
        };
    }

    /// <summary>
    /// 修复初始化后的权限与应用分配。
    /// </summary>
    public async Task<SystemRepairOutput> RepairAsync()
    {
        var output = new SystemRepairOutput();
        var superAdminRole = await _db.PlatformRole.FirstOrDefaultAsync(x => x.Code == PlatformConst.PlatformRoleSuperAdmin);
        if (superAdminRole == null)
        {
            return output;
        }

        var functionIds = await _db.PlatformFunction.Select(x => x.Id).ToListAsync();
        var existingRoleFunctionIds = await _db.PlatformRoleFunction
            .Where(x => x.RoleId == superAdminRole.Id)
            .Select(x => x.FunctionId)
            .ToListAsync();
        foreach (var functionId in functionIds.Except(existingRoleFunctionIds))
        {
            _db.PlatformRoleFunction.Add(new PlatformRoleFunctionEntity
            {
                Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                RoleId = superAdminRole.Id,
                FunctionId = functionId
            });
            output.AddedRoleFunctions++;
        }

        var superAdminUserIds = await _db.UserPlatformRole
            .Where(x => x.RoleId == superAdminRole.Id)
            .Select(x => x.UserId)
            .Distinct()
            .ToListAsync();
        var appIds = await _db.App
            .Where(x => x.Status == CommonStatusConst.Enabled)
            .Select(x => x.Id)
            .ToListAsync();
        foreach (var userId in superAdminUserIds)
        {
            var existingUserAppIds = await _db.UserApp
                .Where(x => x.UserId == userId)
                .Select(x => x.AppId)
                .ToListAsync();
            foreach (var appId in appIds.Except(existingUserAppIds))
            {
                _db.UserApp.Add(new UserAppEntity
                {
                    Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                    UserId = userId,
                    AppId = appId,
                    GrantType = AuthConst.UserAppGrantTypeManual,
                    Status = CommonStatusConst.Enabled
                });
                output.AddedUserApps++;
            }
        }

        await _db.SaveChangesAsync();
        return output;
    }

    private async Task EnsureSuperAdminRoleAsync(Guid userId)
    {
        var superAdminRole = await _db.PlatformRole.FirstAsync(x => x.Code == PlatformConst.PlatformRoleSuperAdmin);
        var exists = await _db.UserPlatformRole.AnyAsync(x => x.UserId == userId && x.RoleId == superAdminRole.Id);
        if (!exists)
        {
            _db.UserPlatformRole.Add(new UserPlatformRoleEntity
            {
                Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                UserId = userId,
                RoleId = superAdminRole.Id
            });
        }
    }

    private async Task EnsureAllAppAccessAsync(Guid userId)
    {
        var appIds = await _db.App.Where(x => x.Status == CommonStatusConst.Enabled).Select(x => x.Id).ToListAsync();
        var existingAppIds = await _db.UserApp.Where(x => x.UserId == userId).Select(x => x.AppId).ToListAsync();
        foreach (var appId in appIds.Except(existingAppIds))
        {
            _db.UserApp.Add(new UserAppEntity
            {
                Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                UserId = userId,
                AppId = appId,
                GrantType = AuthConst.UserAppGrantTypeManual,
                Status = CommonStatusConst.Enabled
            });
        }
    }

    private async Task EnsureSuperAdminRoleFunctionsAsync()
    {
        var superAdminRole = await _db.PlatformRole.FirstAsync(x => x.Code == PlatformConst.PlatformRoleSuperAdmin);
        var functionIds = await _db.PlatformFunction.Select(x => x.Id).ToListAsync();
        var existingFunctionIds = await _db.PlatformRoleFunction.Where(x => x.RoleId == superAdminRole.Id).Select(x => x.FunctionId).ToListAsync();
        foreach (var functionId in functionIds.Except(existingFunctionIds))
        {
            _db.PlatformRoleFunction.Add(new PlatformRoleFunctionEntity
            {
                Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                RoleId = superAdminRole.Id,
                FunctionId = functionId
            });
        }
    }
}
