using System.Net;
using AzureSftpBlobSync.JobConfigs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

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
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            SftpAccountConfig config = new SftpAccountConfig()
            {
                Id = 1,
                Url = "ip",
                Port = 8888,
                UserName = "username",
                Password = "password",
                PrivateKey = "privatekye",
                PrivateKeyPassPhrase = "passphrase"
            };

            IEnumerable<SftpAccountConfig> sftpAccountConfigs = new SftpAccountConfig[] { config };
            string output = JsonSerializer.Serialize(sftpAccountConfigs);

            response.WriteString("SftpAccounts" + Environment.NewLine);
            response.WriteString(output + Environment.NewLine);

            BlobConfig blobConfig = new BlobConfig() { ConnectionString = "yourconnectionstring", Id = 3, ContainerName="containername" };
            IEnumerable<BlobConfig> blobConfigs = new BlobConfig[] { blobConfig };
            output = JsonSerializer.Serialize(blobConfigs);
            response.WriteString("BlobAccounts" + Environment.NewLine);
            response.WriteString(output + Environment.NewLine);

            JobDefinition jobDefinition = new JobDefinition()
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
            };

            IEnumerable<JobDefinition> jobDefinitions = new JobDefinition[] { jobDefinition };
            output = JsonSerializer.Serialize(jobDefinitions);
            response.WriteString("Job Definitions" + Environment.NewLine);
            response.WriteString(output + Environment.NewLine);

            return response;
        }
    }
}
