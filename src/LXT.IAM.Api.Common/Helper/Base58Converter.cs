namespace LXT.IAM.Api.Common.Helper;

public static class Base58Converter
{
    private const string Characters5 = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
    private const string Characters6 = "123456789ABCDEFGHJKLMNPQRSTUVWXYZ";

    public static string ToBase58(long decimalNumber, int length = 6)
    {
        if (decimalNumber < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(decimalNumber));
        }

        var result = new char[length];
        for (var i = length - 1; i >= 0; i--)
        {
            var remainder = (int)(decimalNumber % 33);
            result[i] = Characters6[remainder];
            decimalNumber /= 33;
        }

        return new string(result);
    }

    public static (bool, int) TryParseFromBase58(string base58Number, int length = 6)
    {
        var result = 0;
        try
        {
            if (string.IsNullOrWhiteSpace(base58Number) || (base58Number.Length != length && base58Number.Length != length - 1) || base58Number.Contains("I") || base58Number.Contains("O"))
            {
                throw new ArgumentException("invalid base58");
            }

            var chars = base58Number.Length == 5 ? Characters5 : Characters6;
            foreach (var c in base58Number)
            {
                result = result * chars.Length + chars.IndexOf(c);
            }

            return (true, result);
        }
        catch
        {
            return (false, 0);
        }
    }
}
