using System.Reflection;

namespace LXT.IAM.Api.Common.Utils;

/// <summary>
/// 程序集工具类
/// </summary>
public static class AssemblyHelper
{
    /// <summary>
    /// 按模式加载程序集
    /// </summary>
    public static IEnumerable<Assembly> GetAssemblies(string searchPattern)
    {
        return Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, searchPattern)
            .Select(Assembly.LoadFrom)
            .ToList();
    }
}
