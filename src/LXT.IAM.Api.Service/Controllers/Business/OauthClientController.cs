using LXT.IAM.Api.Bll.Services.OauthClient;
using LXT.IAM.Api.Bll.Services.OauthClient.Dtos;
using LXT.IAM.Api.Common.Models;
using LXT.IAM.Api.Service.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers.Business;

/// <summary>
/// 开放客户端管理控制器
/// </summary>
[Route("api/oauth-client")]
public class OauthClientController : BusinessController
{
    private readonly IOauthClientService _oauthClientService;

    /// <summary>
    /// 构造
    /// </summary>
    public OauthClientController(IOauthClientService oauthClientService)
    {
        _oauthClientService = oauthClientService;
    }

    /// <summary>
    /// 分页查询开放客户端
    /// </summary>
    [HttpPost("page")]
    public async Task<PagedList<OauthClientOutput>> GetPagedListAsync([FromBody] GetOauthClientPagedListInput input)
    {
        return await _oauthClientService.GetPagedListAsync(input);
    }

    /// <summary>
    /// 获取开放客户端详情
    /// </summary>
    [HttpGet("{id}")]
    public async Task<OauthClientOutput> GetAsync(long id)
    {
        return await _oauthClientService.GetAsync(id);
    }

    /// <summary>
    /// 新增开放客户端
    /// </summary>
    [HttpPost]
    public async Task<long> AddAsync([FromBody] AddOauthClientInput input)
    {
        return await _oauthClientService.AddAsync(input);
    }

    /// <summary>
    /// 修改开放客户端
    /// </summary>
    [HttpPut("{id}")]
    public async Task PutAsync(long id, [FromBody] PutOauthClientInput input)
    {
        await _oauthClientService.PutAsync(id, input);
    }

    /// <summary>
    /// 删除开放客户端
    /// </summary>
    [HttpDelete("{id}")]
    public async Task DeleteAsync(long id)
    {
        await _oauthClientService.DeleteAsync(id);
    }
}
