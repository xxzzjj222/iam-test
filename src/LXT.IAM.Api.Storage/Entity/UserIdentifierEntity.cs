using LXT.IAM.Api.Storage.Entity.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace LXT.IAM.Api.Storage.Entity;

[Table("user_identifier")]
public class UserIdentifierEntity : AuditEntity
{
    [Column("common_user_id")]
    public Guid CommonUserId { get; set; }

    [Column("identifier_type")]
    public string IdentifierType { get; set; } = string.Empty;

    [Column("identifier")]
    public string Identifier { get; set; } = string.Empty;

    [Column("country_code")]
    public string? CountryCode { get; set; }

    [Column("is_primary")]
    public bool IsPrimary { get; set; }

    [Column("is_verified")]
    public bool IsVerified { get; set; }

    [Column("verified_time")]
    public DateTime? VerifiedTime { get; set; }
}
