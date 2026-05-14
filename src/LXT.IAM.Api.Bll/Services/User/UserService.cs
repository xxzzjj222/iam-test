using LXT.IAM.Api.Common.Consts;
using LXT.IAM.Api.Common.Helper;
using LXT.IAM.Api.Common.Models;
using LXT.IAM.Api.Bll.Services.User.Dtos;
using LXT.IAM.Api.Storage.Context;
using Microsoft.EntityFrameworkCore;

namespace LXT.IAM.Api.Bll.Services.User;

public class UserService : IUserService
{
    private readonly IAMDbContext _db;

    public UserService(IAMDbContext db)
    {
        _db = db;
    }

    public async Task<PagedList<UserOutput>> GetPagedListAsync(GetUserPagedListInput input)
    {
        var query = _db.User.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(input.Keyword))
        {
            query = query.Where(x =>
                x.Name.Contains(input.Keyword) ||
                (x.Phone != null && x.Phone.Contains(input.Keyword)) ||
                (x.Email != null && x.Email.Contains(input.Keyword)));
        }

        if (input.Status.HasValue)
        {
            query = query.Where(x => x.Status == input.Status.Value);
        }

        if (!string.IsNullOrWhiteSpace(input.AppCode))
        {
            query = from u in query
                    join ua in _db.UserApp on u.UserId equals ua.UserId
                    join app in _db.App on ua.AppId equals app.Id
                    where app.Code == input.AppCode
                    select u;
        }

        if (!string.IsNullOrWhiteSpace(input.RegisterAppCode))
        {
            query = query.Where(x => x.RegisterAppCode == input.RegisterAppCode);
        }

        if (input.StartTime.HasValue)
        {
            query = query.Where(x => x.CreateTime >= input.StartTime.Value);
        }

        if (input.EndTime.HasValue)
        {
            query = query.Where(x => x.CreateTime < input.EndTime.Value);
        }

        var total = await query.CountAsync();
        var items = await query.OrderByDescending(x => x.CreateTime)
            .Skip((input.PageIndex - 1) * input.PageSize)
            .Take(input.PageSize)
            .Select(x => new UserOutput
            {
                UserId = x.UserId,
                Name = x.Name,
                Avatar = x.Avatar,
                Phone = x.Phone,
                Email = x.Email,
                Status = x.Status,
                IsFrozen = x.IsFrozen,
                RegisterAppCode = x.RegisterAppCode,
                CreateTime = x.CreateTime
            })
            .ToListAsync();

        return new PagedList<UserOutput>
        {
            PageIndex = input.PageIndex,
            PageSize = input.PageSize,
            Total = total,
            Items = items
        };
    }

    public async Task<UserOutput> GetAsync(Guid UserId)
    {
        return await _db.User.AsNoTracking()
            .Where(x => x.UserId == UserId)
            .Select(x => new UserOutput
            {
                UserId = x.UserId,
                Name = x.Name,
                Avatar = x.Avatar,
                Phone = x.Phone,
                Email = x.Email,
                Status = x.Status,
                IsFrozen = x.IsFrozen,
                RegisterAppCode = x.RegisterAppCode,
                CreateTime = x.CreateTime
            })
            .FirstAsync();
    }

    public async Task FreezeAsync(Guid UserId)
    {
        var user = await _db.User.FirstAsync(x => x.UserId == UserId);
        user.IsFrozen = true;
        await _db.SaveChangesAsync();
    }

    public async Task UnfreezeAsync(Guid UserId)
    {
        var user = await _db.User.FirstAsync(x => x.UserId == UserId);
        user.IsFrozen = false;
        await _db.SaveChangesAsync();
    }

    public async Task AssignAppsAsync(Guid UserId, AssignUserAppsInput input)
    {
        var existing = await _db.UserApp.Where(x => x.UserId == UserId).ToListAsync();
        _db.UserApp.RemoveRange(existing);
        foreach (var appId in input.AppIds.Distinct())
        {
            _db.UserApp.Add(new Storage.Entity.UserAppEntity
            {
                Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                UserId = UserId,
                AppId = appId,
                GrantType = "manual",
                Status = CommonStatusConst.Enabled
            });
        }

        await _db.SaveChangesAsync();
    }

    public async Task ResetPasswordAsync(Guid UserId, ResetPasswordInput input)
    {
        var credential = await _db.UserCredential.FirstOrDefaultAsync(x =>
            x.UserId == UserId &&
            x.CredentialType == AuthConst.CredentialTypePassword);

        if (credential == null)
        {
            credential = new Storage.Entity.UserCredentialEntity
            {
                Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                UserId = UserId,
                CredentialType = AuthConst.CredentialTypePassword,
                PasswordHash = PasswordHelper.HashPassword(input.NewPassword),
                PasswordVersion = "bcrypt",
                NeedResetPassword = input.NeedResetPassword,
                LastPasswordChangeTime = DateTime.UtcNow
            };
            _db.UserCredential.Add(credential);
        }
        else
        {
            credential.PasswordHash = PasswordHelper.HashPassword(input.NewPassword);
            credential.PasswordVersion = "bcrypt";
            credential.NeedResetPassword = input.NeedResetPassword;
            credential.LastPasswordChangeTime = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();
    }
}

