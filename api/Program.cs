using devCrowd.BuildYourOwnTeamsClient.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker;

IHost? host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((hostContext, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddOptions<Authentication>().Configure<IConfiguration>((settings, configuration) =>
        {
            configuration.GetSection(nameof(Authentication)).Bind(settings);
        });
        services.AddOptions<Graph>().Configure<IConfiguration>((settings, configuration) =>
        {
            configuration.GetSection(nameof(Graph)).Bind(settings);
        });
        services.AddOptions<AzureCommunicationServices>().Configure<IConfiguration>((settings, configuration) =>
        {
            configuration.GetSection(nameof(AzureCommunicationServices)).Bind(settings);
        });
        
    })
    .Build();

host.Run();