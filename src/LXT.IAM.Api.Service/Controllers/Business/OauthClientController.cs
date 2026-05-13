using LXT.IAM.Api.Bll.Services.OauthClient;
using LXT.IAM.Api.Bll.Services.OauthClient.Dtos;
using LXT.IAM.Api.Common.Models;
using LXT.IAM.Api.Service.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers.Business;

[Route("api/oauth-client")]
public class OauthClientController : BusinessController
{
    private readonly IOauthClientService _oauthClientService;

    public OauthClientController(IOauthClientService oauthClientService)
    {
        _oauthClientService = oauthClientService;
    }

    [HttpPost("page")]
    public async Task<PagedList<OauthClientOutput>> GetPagedListAsync([FromBody] GetOauthClientPagedListInput input)
    {
        return await _oauthClientService.GetPagedListAsync(input);
    }

    [HttpGet("{id}")]
    public async Task<OauthClientOutput> GetAsync(long id)
    {
        return await _oauthClientService.GetAsync(id);
    }

    [HttpPost]
    public async Task<long> AddAsync([FromBody] AddOauthClientInput input)
    {
        return await _oauthClientService.AddAsync(input);
    }

    [HttpPut("{id}")]
    public async Task PutAsync(long id, [FromBody] PutOauthClientInput input)
    {
        await _oauthClientService.PutAsync(id, input);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(long id)
    {
        await _oauthClientService.DeleteAsync(id);
    }
}
