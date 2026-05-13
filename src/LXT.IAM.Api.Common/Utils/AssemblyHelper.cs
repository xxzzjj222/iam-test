using System.Reflection;

namespace LXT.IAM.Api.Common.Utils;

public static class AssemblyHelper
{
    public static IEnumerable<Assembly> GetAssemblies(string searchPattern)
    {
        return Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, searchPattern)
            .Select(Assembly.LoadFrom)
            .ToList();
    }
}
