using LXT.IAM.Api.Bll.Services.SocialAuth.Dtos;
using LXT.IAM.Api.Common.Exceptions;
using LXT.IAM.Api.Model.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SKIT.FlurlHttpClient.Wechat.Api;
using SKIT.FlurlHttpClient.Wechat.Api.Models;
using System.Security.Cryptography;
using System.Text;

namespace LXT.IAM.Api.Bll.Services.SocialAuth;

/// <summary>
/// 微信小程序认证服务实现
/// </summary>
public class WeChatMiniAppAuthService : IWeChatMiniAppAuthService
{
    private readonly AppOptions _options;
    private readonly ILogger<WeChatMiniAppAuthService> _logger;

    public WeChatMiniAppAuthService(IOptions<AppOptions> options, ILogger<WeChatMiniAppAuthService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task<WeChatCode2SessionOutput> Code2SessionAsync(string code)
    {
        var client = WechatApiClientBuilder.Create(new WechatApiClientOptions
        {
            AppId = _options.AppId,
            AppSecret = _options.AppSecret
        }).Build();

        var request = new SnsJsCode2SessionRequest
        {
            JsCode = code
        };

        var response = await client.ExecuteSnsJsCode2SessionAsync(request);
        if (!response.IsSuccessful())
        {
            _logger.LogError("微信 code2session 失败: {Message}", response.ErrorMessage);
            throw new UnauthorizedException("未授权");
        }

        return new WeChatCode2SessionOutput
        {
            OpenId = response.OpenId,
            SessionKey = response.SessionKey
        };
    }

    public Task<SocialPhoneInfoOutput> DecodePhoneAsync(DecodePhoneInput input)
    {
        using var aes = Aes.Create();
        aes.Mode = CipherMode.CBC;
        aes.BlockSize = 128;
        aes.Padding = PaddingMode.PKCS7;
        aes.IV = Convert.FromBase64String(input.IV);
        aes.Key = Convert.FromBase64String(input.SessionKey);

        var encryptedData = Convert.FromBase64String(input.EncryptedData);
        var transform = aes.CreateDecryptor();
        var final = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        var result = Encoding.UTF8.GetString(final);

        dynamic? phoneInfo = JsonConvert.DeserializeObject(result);
        if (phoneInfo == null)
        {
            throw new InvalidParameterException("参数无效");
        }

        return Task.FromResult(new SocialPhoneInfoOutput
        {
            PhoneNumber = phoneInfo.phoneNumber,
            CountryCode = phoneInfo.countryCode,
            PurePhoneNumber = phoneInfo.purePhoneNumber
        });
    }
}
