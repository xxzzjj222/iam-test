using LXT.IAM.Api.Bll.Services.SocialAuth.Dtos;
using LXT.IAM.Api.Common.Exceptions;
using LXT.IAM.Api.Model.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SKIT.FlurlHttpClient.ByteDance.MicroApp;
using SKIT.FlurlHttpClient.ByteDance.MicroApp.Models;

namespace LXT.IAM.Api.Bll.Services.SocialAuth;

/// <summary>
/// 抖音小程序认证服务实现
/// </summary>
public class DouyinMiniAppAuthService : IDouyinMiniAppAuthService
{
    private readonly DouyinAppOptions _options;
    private readonly ILogger<DouyinMiniAppAuthService> _logger;

    public DouyinMiniAppAuthService(IOptions<DouyinAppOptions> options, ILogger<DouyinMiniAppAuthService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task<DouyinCode2SessionOutput> Code2SessionAsync(string code)
    {
        var client = DouyinMicroAppClientBuilder.Create(new DouyinMicroAppClientOptions
        {
            AppId = _options.AppId,
            AppSecret = _options.AppSecret
        }).Build();

        var request = new AppsJsCode2SessionV2Request
        {
            Code = code
        };

        var response = await client.ExecuteAppsJsCode2SessionV2Async(request);
        if (!response.IsSuccessful() || response.Data == null)
        {
            _logger.LogError("抖音 code2session 失败: {Message}", response.ErrorMessage);
            throw new UnauthorizedException("未授权");
        }

        return new DouyinCode2SessionOutput
        {
            OpenId = response.Data.OpenId ?? string.Empty,
            SessionKey = response.Data.SessionKey ?? string.Empty
        };
    }
}
