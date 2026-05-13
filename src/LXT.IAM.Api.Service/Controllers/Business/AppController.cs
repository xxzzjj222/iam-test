using LXT.IAM.Api.Bll.Services.App;
using LXT.IAM.Api.Bll.Services.App.Dtos;
using LXT.IAM.Api.Common.Models;
using LXT.IAM.Api.Service.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers.Business;

[Route("api/app")]
public class AppController : BusinessController
{
    private readonly IAppService _appService;

    public AppController(IAppService appService)
    {
        _appService = appService;
    }

    [HttpPost("page")]
    public async Task<PagedList<AppOutput>> GetPagedListAsync([FromBody] GetAppPagedListInput input)
    {
        return await _appService.GetPagedListAsync(input);
    }

    [HttpGet("options")]
    public async Task<List<LongStringKV>> GetOptionsAsync()
    {
        return await _appService.GetOptionsAsync();
    }
}
