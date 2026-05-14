namespace LXT.IAM.Api.Common.Helper;

/// <summary>
/// 密码工具类
/// </summary>
public static class PasswordHelper
{
    /// <summary>
    /// 哈希密码
    /// </summary>
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// 校验密码
    /// </summary>
    public static bool VerifyPassword(string password, string hash)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash))
        {
            return false;
        }

        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
