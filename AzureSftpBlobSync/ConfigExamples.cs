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

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            SftpAccountConfig config = new SftpAccountConfig()
            {
                Id = 1,
                Url = "ip",
                Port = 8888,
                UserName = "username",
                Password = "password",
                PrivateKey = privateKeyRAw,
                PrivateKeyPassPhrase = "passphrase"
            };

            IEnumerable<SftpAccountConfig> sftpAccountConfigs = new SftpAccountConfig[] { config };
            string output = JsonSerializer.Serialize(sftpAccountConfigs, options);

            response.WriteString("SftpAccounts" + Environment.NewLine);
            response.WriteString(output + Environment.NewLine);

            BlobConfig blobConfig = new BlobConfig() { ConnectionString = "yourconnectionstring", Id = 3, ContainerName="containername" };
            IEnumerable<BlobConfig> blobConfigs = new BlobConfig[] { blobConfig };
            output = JsonSerializer.Serialize(blobConfigs, options);
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
            output = JsonSerializer.Serialize(jobDefinitions, options);
            response.WriteString("Job Definitions" + Environment.NewLine);
            response.WriteString(output + Environment.NewLine);

            return response;
        }

        private string privateKeyRAw =
@"-----BEGIN OPENSSH PRIVATE KEY-----
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
hfv8xuETAQldz77VZvNbMBL1mms=
-----END OPENSSH PRIVATE KEY-----";
    }
}
