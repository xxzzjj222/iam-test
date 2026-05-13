using LXT.IAM.Api.Bll.Migration;

namespace LXT.IAM.Api.Service.Filters;

public class MigrateStartupFilter : IStartupFilter
{
    private readonly IServiceProvider _serviceProvider;

    public MigrateStartupFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

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
