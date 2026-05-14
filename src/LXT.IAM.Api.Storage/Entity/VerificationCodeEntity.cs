using LXT.IAM.Api.Storage.Entity.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace LXT.IAM.Api.Storage.Entity;

[Table("verification_code")]
public class VerificationCodeEntity : AuditEntity
{
    [Column("user_id")]
    public Guid? UserId { get; set; }

    [Column("receiver")]
    public string Receiver { get; set; } = string.Empty;

    [Column("receiver_type")]
    public string ReceiverType { get; set; } = string.Empty;

    [Column("scene_code")]
    public string SceneCode { get; set; } = string.Empty;

    [Column("code_hash")]
    public string CodeHash { get; set; } = string.Empty;

    [Column("expire_time")]
    public DateTime ExpireTime { get; set; }

    [Column("status")]
    public int Status { get; set; }

    [Column("send_channel")]
    public string SendChannel { get; set; } = string.Empty;

    [Column("send_ip")]
    public string? SendIp { get; set; }

    [Column("used_time")]
    public DateTime? UsedTime { get; set; }
}

