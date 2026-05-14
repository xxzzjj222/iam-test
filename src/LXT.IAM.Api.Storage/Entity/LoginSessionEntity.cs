using LXT.IAM.Api.Storage.Entity.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace LXT.IAM.Api.Storage.Entity;

[Table("login_session")]
public class LoginSessionEntity : AuditEntity
{
    [Column("session_id")]
    public Guid SessionId { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("app_code")]
    public string AppCode { get; set; } = string.Empty;

    [Column("client_type")]
    public string ClientType { get; set; } = string.Empty;

    [Column("login_type")]
    public string LoginType { get; set; } = string.Empty;

    [Column("refresh_token_hash")]
    public string RefreshTokenHash { get; set; } = string.Empty;

    [Column("access_token_expire_time")]
    public DateTime AccessTokenExpireTime { get; set; }

    [Column("refresh_token_expire_time")]
    public DateTime RefreshTokenExpireTime { get; set; }

    [Column("device_id")]
    public string? DeviceId { get; set; }

    [Column("ip")]
    public string? Ip { get; set; }

    [Column("logout_time")]
    public DateTime? LogoutTime { get; set; }

    [Column("status")]
    public int Status { get; set; }
}

