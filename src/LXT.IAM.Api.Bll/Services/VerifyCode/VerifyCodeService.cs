using LXT.IAM.Api.Bll.Services.VerifyCode.Dtos;
using LXT.IAM.Api.Common.Consts;
using LXT.IAM.Api.Common.Exceptions;
using LXT.IAM.Api.Bll.Services.Email;
using LXT.IAM.Api.Common.Helper;
using LXT.IAM.Api.Bll.Services.Sms;
using LXT.IAM.Api.Model.Options;
using LXT.IAM.Api.Storage.Context;
using LXT.IAM.Api.Storage.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace LXT.IAM.Api.Bll.Services.VerifyCode;

public class VerifyCodeService : IVerifyCodeService
{
    private readonly IAMDbContext _db;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly ISmsSender _smsSender;
    private readonly IEmailSender _emailSender;
    private readonly SmsOptions _smsOptions;
    private readonly EmailOptions _emailOptions;

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

    public async Task<SendVerifyCodeOutput> SendAsync(SendVerifyCodeInput input)
    {
        if (string.IsNullOrWhiteSpace(input.Receiver))
        {
            throw new BadRequestException("请求错误");
        }

        var code = Random.Shared.Next(100000, 999999).ToString();
        var receiver = input.ReceiverType == AuthConst.AccountTypeEmail ? input.Receiver.Trim().ToLowerInvariant() : input.Receiver.Trim();
        var expireTime = DateTime.UtcNow.AddMinutes(10);

        await CheckSendFrequencyAsync(receiver, input.ReceiverType, input.SceneCode);
        await CheckSendLimitAsync(receiver, input.ReceiverType, input.SceneCode);

        var oldCodes = await _db.VerificationCode
            .Where(x => x.Receiver == receiver && x.ReceiverType == input.ReceiverType && x.SceneCode == input.SceneCode && x.Status == CommonStatusConst.VerifyCodeUnused)
            .ToListAsync();
        foreach (var item in oldCodes)
        {
            item.Status = CommonStatusConst.VerifyCodeUsed;
            item.UsedTime = DateTime.UtcNow;
        }

        _db.VerificationCode.Add(new VerificationCodeEntity
        {
            Id = Yitter.IdGenerator.YitIdHelper.NextId(),
            Receiver = receiver,
            ReceiverType = input.ReceiverType,
            SceneCode = input.SceneCode,
            CodeHash = SecurityHelper.Sha256(code),
            ExpireTime = expireTime,
            Status = CommonStatusConst.VerifyCodeUnused,
            SendChannel = input.ReceiverType
        });

        if (input.ReceiverType == AuthConst.AccountTypePhone)
        {
            await _smsSender.SendVerifyCodeAsync(receiver, code);
        }
        else if (input.ReceiverType == AuthConst.AccountTypeEmail)
        {
            await _emailSender.SendVerifyCodeAsync(receiver, code);
        }

        await _db.SaveChangesAsync();

        return new SendVerifyCodeOutput
        {
            Receiver = receiver,
            SceneCode = input.SceneCode,
            ExpireTime = expireTime,
            DebugCode = _hostEnvironment.IsDevelopment() ? code : null
        };
    }

    private async Task CheckSendFrequencyAsync(string receiver, string receiverType, string sceneCode)
    {
        var intervalSeconds = receiverType == AuthConst.AccountTypePhone ? _smsOptions.SendIntervalSeconds : _emailOptions.SendIntervalSeconds;
        var threshold = DateTime.UtcNow.AddSeconds(-intervalSeconds);
        var hasRecentRecord = await _db.VerificationCode.AnyAsync(x =>
            x.Receiver == receiver &&
            x.ReceiverType == receiverType &&
            x.SceneCode == sceneCode &&
            x.CreateTime >= threshold);
        if (hasRecentRecord)
        {
            throw new InvalidParameterException("请求错误");
        }
    }

    private async Task CheckSendLimitAsync(string receiver, string receiverType, string sceneCode)
    {
        var limitMinutes = receiverType == AuthConst.AccountTypePhone ? _smsOptions.LimitTimeMinutes : _emailOptions.ELimitTime;
        var limitCount = receiverType == AuthConst.AccountTypePhone ? _smsOptions.LimitCount : _emailOptions.ElimitCount;
        var threshold = DateTime.UtcNow.AddMinutes(-limitMinutes);
        var sendCount = await _db.VerificationCode.CountAsync(x =>
            x.Receiver == receiver &&
            x.ReceiverType == receiverType &&
            x.SceneCode == sceneCode &&
            x.CreateTime >= threshold);
        if (sendCount >= limitCount)
        {
            throw new InvalidParameterException("请求错误");
        }
    }
}
