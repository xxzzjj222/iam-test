using LXT.IAM.Api.Common.Consts;
using LXT.IAM.Api.Common.Helper;
using LXT.IAM.Api.Storage.Context;
using LXT.IAM.Api.Storage.Entity;
using LXT.IAM.Api.Bll.Services.VerifyCode.Dtos;
using Microsoft.EntityFrameworkCore;

namespace LXT.IAM.Api.Bll.Services.VerifyCode;

public class VerifyCodeService : IVerifyCodeService
{
    private readonly IAMDbContext _db;

    public VerifyCodeService(IAMDbContext db)
    {
        _db = db;
    }

    public async Task<SendVerifyCodeOutput> SendAsync(SendVerifyCodeInput input)
    {
        var code = Random.Shared.Next(100000, 999999).ToString();
        var receiver = input.ReceiverType == "email" ? input.Receiver.Trim().ToLowerInvariant() : input.Receiver.Trim();
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

        await _db.SaveChangesAsync();

        return new SendVerifyCodeOutput
        {
            Receiver = receiver,
            SceneCode = input.SceneCode,
            ExpireTime = expireTime,
            DebugCode = code
        };
    }
}
