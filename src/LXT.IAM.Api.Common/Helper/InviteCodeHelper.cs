namespace LXT.IAM.Api.Common.Helper;

/// <summary>
/// 邀请码工具类
/// </summary>
public static class InviteCodeHelper
{
    /// <summary>
    /// 生成统一邀请码
    /// </summary>
    public static string GenerateUniversalCode(long sourceId)
    {
        return "U" + Base58Converter.ToBase58(sourceId);
    }

    /// <summary>
    /// 生成兼容旧系统的邀请码
    /// </summary>
    public static string GenerateLegacyAliasCode(string bizRoleCode, long sourceId)
    {
        var prefix = bizRoleCode.ToUpperInvariant() switch
        {
            "CUSTOMER" => "C",
            "PROMOTE" => "P",
            "BUSINESS" => "B",
            "FINANCE" => "F",
            "ADMIN" => "A",
            "OPS_ADMIN" => "H",
            _ => "D"
        };

        return prefix + Base58Converter.ToBase58(sourceId);
    }

    /// <summary>
    /// 尝试解析邀请码
    /// </summary>
    public static (bool success, int sourceId, string prefix) TryParse(string code)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length < 2)
        {
            return (false, 0, string.Empty);
        }

        var prefix = code[..1];
        var value = code[1..];
        var (success, sourceId) = Base58Converter.TryParseFromBase58(value);
        return (success, sourceId, prefix);
    }
}
