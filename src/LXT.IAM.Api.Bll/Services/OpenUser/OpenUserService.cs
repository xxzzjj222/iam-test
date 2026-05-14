using LXT.IAM.Api.Bll.Services.OpenUser.Dtos;
using LXT.IAM.Api.Common.Consts;
using LXT.IAM.Api.Storage.Context;
using Microsoft.EntityFrameworkCore;

namespace LXT.IAM.Api.Bll.Services.OpenUser;

public class OpenUserService : IOpenUserService
{
    private readonly IAMDbContext _db;

    public OpenUserService(IAMDbContext db)
    {
        _db = db;
    }

    public async Task<List<OpenUserOutput>> GetUsersByAppAsync(string appCode)
    {
        return await (from user in _db.User.AsNoTracking()
                      join userApp in _db.UserApp.AsNoTracking() on user.UserId equals userApp.UserId
                      join app in _db.App.AsNoTracking() on userApp.AppId equals app.Id
                      where app.Code == appCode && userApp.Status == CommonStatusConst.Enabled
                      select new OpenUserOutput
                      {
                          UserId = user.UserId,
                          Name = user.Name,
                          Avatar = user.Avatar,
                          Phone = user.Phone,
                          Email = user.Email,
                          IsFrozen = user.IsFrozen,
                          RegisterAppCode = user.RegisterAppCode
                      }).ToListAsync();
    }

    public async Task<OpenUserOutput> GetByUserIdAsync(Guid UserId)
    {
        return await _db.User.AsNoTracking()
            .Where(x => x.UserId == UserId)
            .Select(x => new OpenUserOutput
            {
                UserId = x.UserId,
                Name = x.Name,
                Avatar = x.Avatar,
                Phone = x.Phone,
                Email = x.Email,
                IsFrozen = x.IsFrozen,
                RegisterAppCode = x.RegisterAppCode
            })
            .FirstAsync();
    }

    public async Task<List<OpenUserOutput>> BatchGetAsync(BatchOpenUserInput input)
    {
        return await _db.User.AsNoTracking()
            .Where(x => input.UserIds.Contains(x.UserId))
            .Select(x => new OpenUserOutput
            {
                UserId = x.UserId,
                Name = x.Name,
                Avatar = x.Avatar,
                Phone = x.Phone,
                Email = x.Email,
                IsFrozen = x.IsFrozen,
                RegisterAppCode = x.RegisterAppCode
            })
            .ToListAsync();
    }
}

