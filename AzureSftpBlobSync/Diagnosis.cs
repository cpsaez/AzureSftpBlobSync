using System.Net;
using AzureSftpBlobSync.JobConfigs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AzureSftpBlobSync
{
    public class Diagnosis
    {
        private readonly ILogger _logger;
        private AzureSftpBlobSyncConfig config;

        public Diagnosis(ILoggerFactory loggerFactory, IOptions<AzureSftpBlobSyncConfig> config)
        {
            _logger = loggerFactory.CreateLogger<Diagnosis>();
            this.config = config.Value;
        }

        

        [Function("Diagnosis")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            try
            {
                response.WriteString("Sftp config:" + Environment.NewLine);
                response.WriteString("------------" + Environment.NewLine);
                foreach (var sftpConfig in config.BuildSftpAccounts())
                {
                    response.WriteString(sftpConfig.ToString() + Environment.NewLine);
                }
                response.WriteString("Blob config:" + Environment.NewLine);
                response.WriteString("------------" + Environment.NewLine);
                foreach (var blobConfig in config.BuildBlobAccounts())
                {
                    response.WriteString(blobConfig.ToString() + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                response.WriteString(ex.Message);
            }

            response.WriteString("Welcome to Azure Functions!");
            return response;
        }
    }
}
