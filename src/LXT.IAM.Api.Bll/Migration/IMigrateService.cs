using LXT.IAM.Api.Common.Intefaces.Base;

namespace LXT.IAM.Api.Bll.Migration;

public interface IMigrateService : IScopedDependency
{
    Task<string> PretreatVersionAsync();

    ValueTask UpgradeVersionAsync(string currentDbVersion);
}
