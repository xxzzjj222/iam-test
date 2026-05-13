using LXT.IAM.Api.Bll.Services.OauthClient.Dtos;
using LXT.IAM.Api.Common.Helper;
using LXT.IAM.Api.Common.Models;
using LXT.IAM.Api.Storage.Context;
using LXT.IAM.Api.Storage.Entity;
using Microsoft.EntityFrameworkCore;

namespace LXT.IAM.Api.Bll.Services.OauthClient;

public class OauthClientService : IOauthClientService
{
    private readonly IAMDbContext _db;

    public OauthClientService(IAMDbContext db)
    {
        _db = db;
    }

    public async Task<PagedList<OauthClientOutput>> GetPagedListAsync(GetOauthClientPagedListInput input)
    {
        var query = _db.OauthClient.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(input.Keyword))
        {
            query = query.Where(x => x.ClientCode.Contains(input.Keyword) || x.ClientName.Contains(input.Keyword));
        }

        var total = await query.CountAsync();
        var items = await query.OrderBy(x => x.Id)
            .Skip((input.PageIndex - 1) * input.PageSize)
            .Take(input.PageSize)
            .Select(x => new OauthClientOutput
            {
                Id = x.Id,
                ClientCode = x.ClientCode,
                ClientName = x.ClientName,
                GrantType = x.GrantType,
                Scopes = x.Scopes,
                Status = x.Status,
                AccessTokenExpireSeconds = x.AccessTokenExpireSeconds
            }).ToListAsync();

        return new PagedList<OauthClientOutput>
        {
            PageIndex = input.PageIndex,
            PageSize = input.PageSize,
            Total = total,
            Items = items
        };
    }

    public async Task<OauthClientOutput> GetAsync(long id)
    {
        return await _db.OauthClient.AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new OauthClientOutput
            {
                Id = x.Id,
                ClientCode = x.ClientCode,
                ClientName = x.ClientName,
                GrantType = x.GrantType,
                Scopes = x.Scopes,
                Status = x.Status,
                AccessTokenExpireSeconds = x.AccessTokenExpireSeconds
            }).FirstAsync();
    }

    public async Task<long> AddAsync(AddOauthClientInput input)
    {
        var entity = new OauthClientEntity
        {
            Id = Yitter.IdGenerator.YitIdHelper.NextId(),
            ClientCode = input.ClientCode,
            ClientName = input.ClientName,
            ClientSecretHash = SecurityHelper.Sha256(input.ClientSecret),
            GrantType = input.GrantType,
            Scopes = input.Scopes,
            Status = input.Status,
            AccessTokenExpireSeconds = input.AccessTokenExpireSeconds,
            Remark = input.Remark
        };
        _db.OauthClient.Add(entity);
        await _db.SaveChangesAsync();
        return entity.Id;
    }

    public async Task PutAsync(long id, PutOauthClientInput input)
    {
        var entity = await _db.OauthClient.FirstAsync(x => x.Id == id);
        entity.ClientCode = input.ClientCode;
        entity.ClientName = input.ClientName;
        entity.ClientSecretHash = SecurityHelper.Sha256(input.ClientSecret);
        entity.GrantType = input.GrantType;
        entity.Scopes = input.Scopes;
        entity.Status = input.Status;
        entity.AccessTokenExpireSeconds = input.AccessTokenExpireSeconds;
        entity.Remark = input.Remark;
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var entity = await _db.OauthClient.FirstAsync(x => x.Id == id);
        _db.OauthClient.Remove(entity);
        await _db.SaveChangesAsync();
    }
}
