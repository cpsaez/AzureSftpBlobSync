using System.Net;
using AzureSftpBlobSync.JobConfigs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AzureSftpBlobSync
{
    public class ConfigExamples
    {
        private readonly ILogger _logger;

        public ConfigExamples(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ConfigExamples>();
        }

        [Function("ConfigExamples")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            options.WriteIndented = true;

            AzureSftpBlobSyncConfig config = new AzureSftpBlobSyncConfig();
            config.SftpAccounts = new[] {   new SftpAccountConfig()
            {
                Name="SftpAcccount example",
                Id = 1,
                Url = "ip",
                Port = 8888,
                UserName = "username",
                Password = "password",
                PrivateKeyPassPhrase = "passphrase",
                PrivateKeyFileName="/config/privatekey.txt"
                }
            };

            config.PgpKeys = new[] { new PgpKey() { Name="KeyExample", Id = 1, KeyPath = $"/config/pgppublickey.txt" } };

            config.BlobAccounts = new[]
            {
                new BlobConfig() { ConnectionString = "yourconnectionstring", Id = 3, ContainerName = "containername" }
            };

            config.JobDefinitions = new[]
            {
                 new JobDefinition()
                    {
                        Name = "your job definition name",
                        BlobAccountId = 1,
                        SftpAccountId = 1,
                        BlobFolder = "/yourblobFolder/here",
                        SftpFolder = "/yoursftpFolder/here",
                        JobType = JobType.CopyFromSftpToBlobStorage,
                        BlobFolderRecursiveEnabled = true,
                        SftpFolderRecursiveEnabled = true,
                        SaveInJournalBeforeDeleteFromBlobStoragePath = "/YourJournalPath/here/{yyyy}/{MM}/{dd}/{hh}/{mm}/{ss}"
                    }
             };


            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            string output = JsonSerializer.Serialize(config, options);
            response.WriteString(output);

            return response;
        }
    }
}
