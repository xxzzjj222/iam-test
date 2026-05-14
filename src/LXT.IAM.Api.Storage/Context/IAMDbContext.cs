using LXT.IAM.Api.Common.Intefaces;
using LXT.IAM.Api.Storage.Entity;
using LXT.IAM.Api.Storage.Entity.Base;
using Microsoft.EntityFrameworkCore;

namespace LXT.IAM.Api.Storage.Context;

public class IAMDbContext : DbContext
{
    private readonly IHttpContextUtility _httpContextUtility;

    public IAMDbContext(DbContextOptions<IAMDbContext> options, IHttpContextUtility httpContextUtility) : base(options)
    {
        _httpContextUtility = httpContextUtility;
    }

    public DbSet<UserEntity> User { get; set; }
    public DbSet<UserIdentifierEntity> UserIdentifier { get; set; }
    public DbSet<UserCredentialEntity> UserCredential { get; set; }
    public DbSet<UserSocialAccountEntity> UserSocialAccount { get; set; }
    public DbSet<VerificationCodeEntity> VerificationCode { get; set; }
    public DbSet<LoginSessionEntity> LoginSession { get; set; }
    public DbSet<AppEntity> App { get; set; }
    public DbSet<UserAppEntity> UserApp { get; set; }
    public DbSet<PlatformRoleEntity> PlatformRole { get; set; }
    public DbSet<UserPlatformRoleEntity> UserPlatformRole { get; set; }
    public DbSet<PlatformFunctionEntity> PlatformFunction { get; set; }
    public DbSet<PlatformRoleFunctionEntity> PlatformRoleFunction { get; set; }
    public DbSet<InviteCodeEntity> InviteCode { get; set; }
    public DbSet<InviteRelationEntity> InviteRelation { get; set; }
    public DbSet<UserAppRoleSnapshotEntity> UserAppRoleSnapshot { get; set; }
    public DbSet<OauthClientEntity> OauthClient { get; set; }
    public DbSet<UserAppActivityDailyEntity> UserAppActivityDaily { get; set; }
    public DbSet<DailyUserStatEntity> DailyUserStat { get; set; }
    public DbSet<DailyBusinessStatEntity> DailyBusinessStat { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var currentUserId = _httpContextUtility.GetUserId();
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is not AuditEntity entity)
            {
                continue;
            }

            if (entry.State == EntityState.Added)
            {
                entity.CreateBy = currentUserId;
                entity.CreateTime = now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entity.UpdateBy = currentUserId;
                entity.UpdateTime = now;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
