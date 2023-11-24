using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AzureSftpBlobSync.Engine;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration((hostContext, config) =>
        {
            if (hostContext.HostingEnvironment.IsDevelopment())
            {
                config.AddJsonFile("local.settings.json");
            }
        })
    .ConfigureServices(s => SetupConfigurationHelper.Configure(s))
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddSingleton<IJobsExecutor, JobsExecutor>();
    })
    .Build();


host.Run();
