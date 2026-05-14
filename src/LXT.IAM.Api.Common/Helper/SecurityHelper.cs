using System.Security.Cryptography;
using System.Text;

namespace LXT.IAM.Api.Common.Helper;

/// <summary>
/// 安全工具类
/// </summary>
public static class SecurityHelper
{
    /// <summary>
    /// 计算 SHA256
    /// </summary>
    public static string Sha256(string content)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(content));
        return Convert.ToHexString(bytes);
    }

    /// <summary>
    /// 生成随机令牌字符串
    /// </summary>
    public static string GenerateTokenString()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
    }
}
