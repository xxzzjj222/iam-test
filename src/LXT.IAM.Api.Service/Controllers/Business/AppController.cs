using LXT.IAM.Api.Bll.Services.App;
using LXT.IAM.Api.Bll.Services.App.Dtos;
using LXT.IAM.Api.Common.Models;
using LXT.IAM.Api.Service.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace LXT.IAM.Api.Service.Controllers.Business;

/// <summary>
/// 应用管理控制器
/// </summary>
[Route("api/app")]
public class AppController : BusinessController
{
    private readonly IAppService _appService;

    /// <summary>
    /// 构造
    /// </summary>
    public AppController(IAppService appService)
    {
        _appService = appService;
    }

    /// <summary>
    /// 分页查询应用
    /// </summary>
    [HttpPost("page")]
    public async Task<PagedList<AppOutput>> GetPagedListAsync([FromBody] GetAppPagedListInput input)
    {
        return await _appService.GetPagedListAsync(input);
    }

    /// <summary>
    /// 获取应用下拉选项
    /// </summary>
    [HttpGet("options")]
    public async Task<List<LongStringKV>> GetOptionsAsync()
    {
        return await _appService.GetOptionsAsync();
    }
}
