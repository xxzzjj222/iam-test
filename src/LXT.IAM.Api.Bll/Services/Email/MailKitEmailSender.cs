using LXT.IAM.Api.Common.Exceptions;
using LXT.IAM.Api.Model.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace LXT.IAM.Api.Bll.Services.Email;

/// <summary>
/// MailKit 邮件发送实现
/// </summary>
public class MailKitEmailSender : IEmailSender
{
    private readonly EmailOptions _options;
    private readonly ILogger<MailKitEmailSender> _logger;

    public MailKitEmailSender(IOptions<EmailOptions> options, ILogger<MailKitEmailSender> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task SendVerifyCodeAsync(string email, string code)
    {
        if (!_options.Enabled)
        {
            return;
        }

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_options.CurrName, _options.CurrEmailAddr));
        message.To.Add(new MailboxAddress(email, email));
        message.Subject = _options.SubjectTemplate;
        message.Body = new TextPart("html")
        {
            Text = _options.BodyTemplate.Replace("{code}", code)
        };

        using var smtp = new SmtpClient();
        try
        {
#pragma warning disable S4830
            smtp.ServerCertificateValidationCallback = (_, _, _, _) => true;
#pragma warning restore S4830
            await smtp.ConnectAsync(_options.PostfixServerIp, _options.PostfixServerPort, SecureSocketOptions.Auto);
            if (!string.IsNullOrWhiteSpace(_options.MailPwd))
            {
                await smtp.AuthenticateAsync(_options.CurrEmailAddr, _options.MailPwd);
            }

            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "邮件发送失败");
            throw new InternalServerException("内部服务错误");
        }
    }
}
