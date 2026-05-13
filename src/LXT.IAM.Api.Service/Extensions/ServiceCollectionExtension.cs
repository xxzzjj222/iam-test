using LXT.IAM.Api.Common.Intefaces.Base;
using LXT.IAM.Api.Common.Utils;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Grafana.Loki;
using System.Reflection;
using Yitter.IdGenerator;

namespace LXT.IAM.Api.Service.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddBussinessObjectInjection(this IServiceCollection services)
    {
        var assemblies = AssemblyHelper.GetAssemblies("LXT.IAM.Api.*.dll");
        services.InjectLifetimeService(assemblies, typeof(ITransientDependency), ServiceLifetime.Transient);
        services.InjectLifetimeService(assemblies, typeof(IScopedDependency), ServiceLifetime.Scoped);
        services.InjectLifetimeService(assemblies, typeof(ISingletonDependency), ServiceLifetime.Singleton);
        return services;
    }

    private static IServiceCollection InjectLifetimeService(this IServiceCollection services, IEnumerable<Assembly> assemblies, Type type, ServiceLifetime serviceLifetime)
    {
        var injectTypes = assemblies.SelectMany(a => a.GetTypes().Where(t => t.IsClass && t.GetInterfaces().Contains(type))).ToList();
        injectTypes.ForEach(implementType =>
        {
            implementType.GetInterfaces().ToList().ForEach(serviceType =>
            {
                if (serviceType != type && !implementType.IsGenericType)
                {
                    services.Add(new ServiceDescriptor(serviceType, implementType, serviceLifetime));
                }
            });
        });
        return services;
    }

    public static Serilog.ILogger GetLogConfig(string name, bool enableLokiLogging, string? lokiUrl)
    {
        var loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.Console()
            .Enrich.FromLogContext();

        if (enableLokiLogging && !string.IsNullOrWhiteSpace(lokiUrl))
        {
            loggerConfiguration = loggerConfiguration.WriteTo.GrafanaLoki(lokiUrl, [new LokiLabel { Key = "App", Value = name }]);
        }

        return loggerConfiguration.CreateLogger();
    }

    public static IServiceCollection AddSnowflakeId(this IServiceCollection services, ushort workId)
    {
        var options = new IdGeneratorOptions
        {
            WorkerId = workId,
        };
        YitIdHelper.SetIdGenerator(options);
        return services;
    }
}
