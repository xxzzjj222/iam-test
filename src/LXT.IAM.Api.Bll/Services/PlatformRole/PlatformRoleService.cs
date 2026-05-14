using LXT.IAM.Api.Bll.Services.PlatformRole.Dtos;
using LXT.IAM.Api.Common.Models;
using LXT.IAM.Api.Storage.Context;
using LXT.IAM.Api.Storage.Entity;
using Microsoft.EntityFrameworkCore;

namespace LXT.IAM.Api.Bll.Services.PlatformRole;

public class PlatformRoleService : IPlatformRoleService
{
    private readonly IAMDbContext _db;

    public PlatformRoleService(IAMDbContext db)
    {
        _db = db;
    }

    public async Task<PagedList<PlatformRoleOutput>> GetPagedListAsync(GetPlatformRolePagedListInput input)
    {
        var query = _db.PlatformRole.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(input.Keyword))
        {
            query = query.Where(x => x.Name.Contains(input.Keyword) || x.Code.Contains(input.Keyword));
        }

        var total = await query.CountAsync();
        var items = await query.OrderBy(x => x.Id)
            .Skip((input.PageIndex - 1) * input.PageSize)
            .Take(input.PageSize)
            .Select(x => new PlatformRoleOutput
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                Description = x.Description
            })
            .ToListAsync();

        return new PagedList<PlatformRoleOutput>
        {
            PageIndex = input.PageIndex,
            PageSize = input.PageSize,
            Total = total,
            Items = items
        };
    }

    public async Task<PlatformRoleOutput> GetAsync(long id)
    {
        return await _db.PlatformRole.AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new PlatformRoleOutput
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                Description = x.Description
            })
            .FirstAsync();
    }

    public async Task<List<LongStringKV>> GetOptionsAsync()
    {
        return await _db.PlatformRole.AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => new LongStringKV
            {
                Key = x.Id,
                Value = x.Name
            }).ToListAsync();
    }

    public async Task<long> AddAsync(AddPlatformRoleInput input)
    {
        var entity = new PlatformRoleEntity
        {
            Id = Yitter.IdGenerator.YitIdHelper.NextId(),
            Name = input.Name,
            Code = input.Code,
            Description = input.Description,
            Remark = input.Remark
        };
        _db.PlatformRole.Add(entity);
        await _db.SaveChangesAsync();
        return entity.Id;
    }

    public async Task PutAsync(long id, PutPlatformRoleInput input)
    {
        var entity = await _db.PlatformRole.FirstAsync(x => x.Id == id);
        entity.Name = input.Name;
        entity.Code = input.Code;
        entity.Description = input.Description;
        entity.Remark = input.Remark;
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var entity = await _db.PlatformRole.FirstAsync(x => x.Id == id);
        _db.PlatformRole.Remove(entity);
        var userRoles = await _db.UserPlatformRole.Where(x => x.RoleId == id).ToListAsync();
        _db.UserPlatformRole.RemoveRange(userRoles);
        await _db.SaveChangesAsync();
    }

    public async Task AssignUserRolesAsync(AssignUserPlatformRolesInput input)
    {
        var oldRoles = await _db.UserPlatformRole.Where(x => x.UserId == input.UserId).ToListAsync();
        _db.UserPlatformRole.RemoveRange(oldRoles);
        foreach (var roleId in input.RoleIds.Distinct())
        {
            _db.UserPlatformRole.Add(new UserPlatformRoleEntity
            {
                Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                UserId = input.UserId,
                RoleId = roleId
            });
        }
        await _db.SaveChangesAsync();
    }
}

