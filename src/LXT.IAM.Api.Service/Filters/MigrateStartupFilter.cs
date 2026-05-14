using LXT.IAM.Api.Bll.Migration;

namespace LXT.IAM.Api.Service.Filters;

/// <summary>
/// 数据库迁移启动过滤器
/// </summary>
public class MigrateStartupFilter : IStartupFilter
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// 构造
    /// </summary>
    public MigrateStartupFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// 配置启动管道
    /// </summary>
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return builder =>
        {
            Execute().Wait();
            next(builder);
        };
    }

    private async Task Execute()
    {
        using var scope = _serviceProvider.CreateScope();
        var migrateService = scope.ServiceProvider.GetRequiredService<IMigrateService>();
        var currentDbVersion = await migrateService.PretreatVersionAsync();
        await migrateService.UpgradeVersionAsync(currentDbVersion);
    }
}
