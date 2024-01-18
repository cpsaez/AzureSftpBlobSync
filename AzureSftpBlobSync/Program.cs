using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AzureSftpBlobSync.Engine;
using AzureSftpBlobSync;
using AzureSftpBlobSync.Providers.SshProvider;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration((hostContext, config) =>
        {
            if (hostContext.HostingEnvironment.IsDevelopment())
            {
                config.AddJsonFile("local.settings.json");
            }
        })
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddSingleton<IJobsExecutor, JobsExecutor>();
        services.AddSingleton<IConfigReader, ConfigReader>();
        services.AddSingleton<IKeysService, KeysService>();
    })
    .Build();


host.Run();
