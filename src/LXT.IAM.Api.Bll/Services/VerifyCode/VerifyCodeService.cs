using LXT.IAM.Api.Bll.Services.VerifyCode.Dtos;
using LXT.IAM.Api.Common.Consts;
using LXT.IAM.Api.Common.Exceptions;
using LXT.IAM.Api.Common.Helper;
using LXT.IAM.Api.Bll.Services.Sms;
using LXT.IAM.Api.Storage.Context;
using LXT.IAM.Api.Storage.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace LXT.IAM.Api.Bll.Services.VerifyCode;

public class VerifyCodeService : IVerifyCodeService
{
    private readonly IAMDbContext _db;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly ISmsSender _smsSender;

    public VerifyCodeService(IAMDbContext db, IHostEnvironment hostEnvironment, ISmsSender smsSender)
    {
        _db = db;
        _hostEnvironment = hostEnvironment;
        _smsSender = smsSender;
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

        await _db.SaveChangesAsync();

        return new SendVerifyCodeOutput
        {
            Receiver = receiver,
            SceneCode = input.SceneCode,
            ExpireTime = expireTime,
            DebugCode = _hostEnvironment.IsDevelopment() ? code : null
        };
    }
}
