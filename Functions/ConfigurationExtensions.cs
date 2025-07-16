using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Functions;

public static class ConfigurationExtensions
{
    public static void AddLogging(this FunctionsApplicationBuilder builder, string appName)
    {
        builder.Logging.AddOpenTelemetry(options =>
        {
            options.AddOtlpExporter();
            options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(appName)); 
            options.IncludeFormattedMessage = true;
            options.ParseStateValues = true;
            options.IncludeScopes = true; 
        });
    }
    
    public static void AddConfigurations(
        this IServiceCollection services,
        IConfiguration configuration,
        string appName)
    {
        
    
        var tp = Sdk.CreateTracerProviderBuilder()
            .AddSource("Microsoft.Azure.Functions.Worker")
            .SetSampler(new AlwaysOnSampler())
            .ConfigureResource(configure => configure
                .AddService(serviceName: appName))
            .AddOtlpExporter()
            .Build();
        
        services.AddSingleton(tp);
    }
}