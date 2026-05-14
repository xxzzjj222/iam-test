using LXT.IAM.Api.Bll.Services.Email;
using LXT.IAM.Api.Bll.Services.Sms;
using LXT.IAM.Api.Bll.Services.VerifyCode.Dtos;
using LXT.IAM.Api.Common.Consts;
using LXT.IAM.Api.Common.Exceptions;
using LXT.IAM.Api.Common.Helper;
using LXT.IAM.Api.Model.Options;
using LXT.IAM.Api.Storage.Context;
using LXT.IAM.Api.Storage.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace LXT.IAM.Api.Bll.Services.VerifyCode;

/// <summary>
/// 验证码服务。
/// </summary>
public class VerifyCodeService : IVerifyCodeService
{
    private readonly IAMDbContext _db;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly ISmsSender _smsSender;
    private readonly IEmailSender _emailSender;
    private readonly SmsOptions _smsOptions;
    private readonly EmailOptions _emailOptions;

    /// <summary>
    /// 构造函数。
    /// </summary>
    public VerifyCodeService(
        IAMDbContext db,
        IHostEnvironment hostEnvironment,
        ISmsSender smsSender,
        IEmailSender emailSender,
        IOptions<SmsOptions> smsOptions,
        IOptions<EmailOptions> emailOptions)
    {
        _db = db;
        _hostEnvironment = hostEnvironment;
        _smsSender = smsSender;
        _emailSender = emailSender;
        _smsOptions = smsOptions.Value;
        _emailOptions = emailOptions.Value;
    }

    /// <summary>
    /// 发送验证码。
    /// </summary>
    public async Task<SendVerifyCodeOutput> SendAsync(SendVerifyCodeInput input)
    {
        if (string.IsNullOrWhiteSpace(input.Receiver))
        {
            throw new BadRequestException("接收方不能为空");
        }

        var rawReceiver = input.Receiver.Trim();
        var normalizedReceiver = NormalizeReceiver(input.ReceiverType, rawReceiver, input.CountryCode);
        var code = Random.Shared.Next(100000, 999999).ToString();
        var expireTime = DateTime.UtcNow.AddMinutes(10);

        await CheckSendFrequencyAsync(normalizedReceiver, rawReceiver, input.ReceiverType, input.SceneCode);
        await CheckSendLimitAsync(normalizedReceiver, rawReceiver, input.ReceiverType, input.SceneCode);

        var oldCodes = await QueryReceiverCodes(normalizedReceiver, rawReceiver, input.ReceiverType, input.SceneCode)
            .Where(x => x.Status == CommonStatusConst.VerifyCodeUnused)
            .ToListAsync();
        foreach (var item in oldCodes)
        {
            item.Status = CommonStatusConst.VerifyCodeUsed;
            item.UsedTime = DateTime.UtcNow;
        }

        _db.VerificationCode.Add(new VerificationCodeEntity
        {
            Id = Yitter.IdGenerator.YitIdHelper.NextId(),
            Receiver = normalizedReceiver,
            ReceiverType = input.ReceiverType,
            SceneCode = input.SceneCode,
            CodeHash = SecurityHelper.Sha256(code),
            ExpireTime = expireTime,
            Status = CommonStatusConst.VerifyCodeUnused,
            SendChannel = input.ReceiverType
        });

        if (input.ReceiverType == AuthConst.AccountTypePhone)
        {
            await _smsSender.SendVerifyCodeAsync(rawReceiver, code);
        }
        else if (input.ReceiverType == AuthConst.AccountTypeEmail)
        {
            await _emailSender.SendVerifyCodeAsync(normalizedReceiver, code);
        }
        else
        {
            throw new InvalidParameterException("不支持的接收方类型");
        }

        await _db.SaveChangesAsync();

        return new SendVerifyCodeOutput
        {
            Receiver = rawReceiver,
            SceneCode = input.SceneCode,
            ExpireTime = expireTime,
            DebugCode = _hostEnvironment.IsDevelopment() ? code : null
        };
    }

    private async Task CheckSendFrequencyAsync(string normalizedReceiver, string rawReceiver, string receiverType, string sceneCode)
    {
        var intervalSeconds = receiverType == AuthConst.AccountTypePhone
            ? _smsOptions.SendIntervalSeconds
            : _emailOptions.SendIntervalSeconds;
        var threshold = DateTime.UtcNow.AddSeconds(-intervalSeconds);

        var hasRecentRecord = await QueryReceiverCodes(normalizedReceiver, rawReceiver, receiverType, sceneCode)
            .AnyAsync(x => x.CreateTime >= threshold);
        if (hasRecentRecord)
        {
            throw new InvalidParameterException("验证码发送过于频繁");
        }
    }

    private async Task CheckSendLimitAsync(string normalizedReceiver, string rawReceiver, string receiverType, string sceneCode)
    {
        var limitMinutes = receiverType == AuthConst.AccountTypePhone
            ? _smsOptions.LimitTimeMinutes
            : _emailOptions.ELimitTime;
        var limitCount = receiverType == AuthConst.AccountTypePhone
            ? _smsOptions.LimitCount
            : _emailOptions.ElimitCount;
        var threshold = DateTime.UtcNow.AddMinutes(-limitMinutes);

        var sendCount = await QueryReceiverCodes(normalizedReceiver, rawReceiver, receiverType, sceneCode)
            .CountAsync(x => x.CreateTime >= threshold);
        if (sendCount >= limitCount)
        {
            throw new InvalidParameterException("验证码发送次数已达上限");
        }
    }

    private IQueryable<VerificationCodeEntity> QueryReceiverCodes(string normalizedReceiver, string rawReceiver, string receiverType, string sceneCode)
    {
        var query = _db.VerificationCode.Where(x =>
            x.ReceiverType == receiverType &&
            x.SceneCode == sceneCode);

        if (receiverType == AuthConst.AccountTypePhone)
        {
            return query.Where(x => x.Receiver == normalizedReceiver || x.Receiver == rawReceiver);
        }

        return query.Where(x => x.Receiver == normalizedReceiver);
    }

    private static string NormalizeReceiver(string receiverType, string receiver, string? countryCode)
    {
        if (receiverType == AuthConst.AccountTypeEmail)
        {
            return receiver.Trim().ToLowerInvariant();
        }

        if (receiverType == AuthConst.AccountTypePhone)
        {
            var normalizedCountryCode = string.IsNullOrWhiteSpace(countryCode)
                ? string.Empty
                : countryCode.Trim().TrimStart('+');

            return string.IsNullOrWhiteSpace(normalizedCountryCode)
                ? receiver
                : $"+{normalizedCountryCode}:{receiver}";
        }

        return receiver;
    }
}
