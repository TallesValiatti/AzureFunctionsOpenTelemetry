using Functions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

string appName = "MySuperApp.Functions";

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication()
    .AddLogging(appName);;

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights()
    .AddConfigurations(builder.Configuration, appName);;

builder.Build().Run();
