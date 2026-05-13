using LXT.IAM.Api.Bll.Services.PlatformFunction.Dtos;
using LXT.IAM.Api.Common.Intefaces;
using LXT.IAM.Api.Storage.Context;
using LXT.IAM.Api.Storage.Entity;
using Microsoft.EntityFrameworkCore;

namespace LXT.IAM.Api.Bll.Services.PlatformFunction;

public class PlatformFunctionService : IPlatformFunctionService
{
    private readonly IAMDbContext _db;
    private readonly IHttpContextUtility _httpContextUtility;

    public PlatformFunctionService(IAMDbContext db, IHttpContextUtility httpContextUtility)
    {
        _db = db;
        _httpContextUtility = httpContextUtility;
    }

    public async Task<List<PlatformFunctionOutput>> GetTreeAsync()
    {
        var functions = await _db.PlatformFunction.AsNoTracking().OrderBy(x => x.Sort).ToListAsync();
        return BuildTree(functions);
    }

    public async Task<List<PlatformFunctionOutput>> GetCurrentUserFunctionsAsync()
    {
        var userId = _httpContextUtility.GetUserId();
        var roleIds = await _db.UserPlatformRole.Where(x => x.CommonUserId == userId).Select(x => x.RoleId).ToListAsync();
        var functionIds = await _db.PlatformRoleFunction.Where(x => roleIds.Contains(x.RoleId)).Select(x => x.FunctionId).Distinct().ToListAsync();
        var functions = await _db.PlatformFunction.AsNoTracking().Where(x => functionIds.Contains(x.Id)).OrderBy(x => x.Sort).ToListAsync();
        return BuildTree(functions);
    }

    public async Task<PlatformFunctionOutput> GetAsync(long id)
    {
        return await _db.PlatformFunction.AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new PlatformFunctionOutput
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                ParentId = x.ParentId,
                Icon = x.Icon,
                Type = x.Type,
                RouteUrl = x.RouteUrl,
                ComponentUrl = x.ComponentUrl,
                IsHidden = x.IsHidden,
                Sort = x.Sort
            }).FirstAsync();
    }

    public async Task<long> AddAsync(AddPlatformFunctionInput input)
    {
        var entity = new PlatformFunctionEntity
        {
            Id = Yitter.IdGenerator.YitIdHelper.NextId(),
            Name = input.Name,
            Code = input.Code,
            ParentId = input.ParentId,
            Icon = input.Icon,
            Type = input.Type,
            RouteUrl = input.RouteUrl,
            ComponentUrl = input.ComponentUrl,
            IsHidden = input.IsHidden,
            Sort = input.Sort,
            Remark = input.Remark
        };
        _db.PlatformFunction.Add(entity);
        await _db.SaveChangesAsync();
        return entity.Id;
    }

    public async Task PutAsync(long id, PutPlatformFunctionInput input)
    {
        var entity = await _db.PlatformFunction.FirstAsync(x => x.Id == id);
        entity.Name = input.Name;
        entity.Code = input.Code;
        entity.ParentId = input.ParentId;
        entity.Icon = input.Icon;
        entity.Type = input.Type;
        entity.RouteUrl = input.RouteUrl;
        entity.ComponentUrl = input.ComponentUrl;
        entity.IsHidden = input.IsHidden;
        entity.Sort = input.Sort;
        entity.Remark = input.Remark;
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var entity = await _db.PlatformFunction.FirstAsync(x => x.Id == id);
        _db.PlatformFunction.Remove(entity);
        var refs = await _db.PlatformRoleFunction.Where(x => x.FunctionId == id).ToListAsync();
        _db.PlatformRoleFunction.RemoveRange(refs);
        await _db.SaveChangesAsync();
    }

    public async Task AssignRoleFunctionsAsync(AssignPlatformRoleFunctionsInput input)
    {
        var oldRefs = await _db.PlatformRoleFunction.Where(x => x.RoleId == input.RoleId).ToListAsync();
        _db.PlatformRoleFunction.RemoveRange(oldRefs);
        foreach (var functionId in input.FunctionIds.Distinct())
        {
            _db.PlatformRoleFunction.Add(new PlatformRoleFunctionEntity
            {
                Id = Yitter.IdGenerator.YitIdHelper.NextId(),
                RoleId = input.RoleId,
                FunctionId = functionId
            });
        }
        await _db.SaveChangesAsync();
    }

    private static List<PlatformFunctionOutput> BuildTree(List<PlatformFunctionEntity> functions)
    {
        var dict = functions.ToDictionary(
            x => x.Id,
            x => new PlatformFunctionOutput
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                ParentId = x.ParentId,
                Icon = x.Icon,
                Type = x.Type,
                RouteUrl = x.RouteUrl,
                ComponentUrl = x.ComponentUrl,
                IsHidden = x.IsHidden,
                Sort = x.Sort
            });

        var roots = new List<PlatformFunctionOutput>();
        foreach (var item in dict.Values.OrderBy(x => x.Sort))
        {
            if (item.ParentId.HasValue && dict.TryGetValue(item.ParentId.Value, out var parent))
            {
                parent.Children.Add(item);
            }
            else
            {
                roots.Add(item);
            }
        }

        return roots;
    }
}
