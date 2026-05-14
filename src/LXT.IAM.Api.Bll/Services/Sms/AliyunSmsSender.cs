using LXT.IAM.Api.Common.Exceptions;
using LXT.IAM.Api.Model.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tea;

namespace LXT.IAM.Api.Bll.Services.Sms;

/// <summary>
/// 阿里云短信发送实现
/// </summary>
public class AliyunSmsSender : ISmsSender
{
    private readonly SmsOptions _options;
    private readonly ILogger<AliyunSmsSender> _logger;

    public AliyunSmsSender(IOptions<SmsOptions> options, ILogger<AliyunSmsSender> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task SendVerifyCodeAsync(string phone, string code)
    {
        if (!_options.Enabled)
        {
            return;
        }

        var client = CreateClient();
        var sendRequest = new AlibabaCloud.SDK.Dysmsapi20170525.Models.SendSmsRequest
        {
            PhoneNumbers = phone,
            SignName = _options.ALIBABA_CLOUD_ACCESS_SIGNNAME,
            TemplateCode = _options.ALIBABA_CLOUD_ACCESS_TEMPLATECODE,
            TemplateParam = $"{{\"code\":\"{code}\",\"time\":\"10\"}}"
        };

        var runtime = new AlibabaCloud.TeaUtil.Models.RuntimeOptions();

        try
        {
            var response = await client.SendSmsWithOptionsAsync(sendRequest, runtime);
            if (response.Body.Code != "OK")
            {
                _logger.LogError("短信发送失败: {Message}", response.Body.Message);
                throw new InternalServerException("内部服务错误");
            }
        }
        catch (TeaException ex)
        {
            _logger.LogError(ex, "阿里云短信发送异常");
            throw new InternalServerException("内部服务错误");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "短信发送系统异常");
            throw new InternalServerException("内部服务错误");
        }
    }

    private AlibabaCloud.SDK.Dysmsapi20170525.Client CreateClient()
    {
        var config = new AlibabaCloud.OpenApiClient.Models.Config
        {
            AccessKeyId = _options.ALIBABA_CLOUD_ACCESS_KEY_ID,
            AccessKeySecret = _options.ALIBABA_CLOUD_ACCESS_KEY_SECRET,
            Endpoint = "dysmsapi.aliyuncs.com"
        };

        return new AlibabaCloud.SDK.Dysmsapi20170525.Client(config);
    }
}
