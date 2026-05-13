using System.Security.Cryptography;
using System.Text;

namespace LXT.IAM.Api.Common.Helper;

public static class SecurityHelper
{
    public static string Sha256(string content)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(content));
        return Convert.ToHexString(bytes);
    }

    public static string GenerateTokenString()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
    }
}
