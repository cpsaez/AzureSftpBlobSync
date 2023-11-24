using AzureSftpBlobSync.JobConfigs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

internal class SetupConfigurationHelper
{
    public static void Configure(IServiceCollection services)
    {
        services.AddOptions<AzureSftpBlobSyncConfig>().Configure<IConfiguration>((settings, configuration) =>
        {
            configuration.Bind("AzureSftpBlobSync", settings);
        });
    }
}