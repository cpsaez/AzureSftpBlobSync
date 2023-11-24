using System.Net;
using AzureSftpBlobSync.Engine;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureSftpBlobSync
{
    public class LaunchManual
    {
        private readonly ILogger logger;
        private readonly IJobsExecutor executor;

        public LaunchManual(ILoggerFactory loggerFactory, IJobsExecutor executor)
        {
            this.logger = loggerFactory.CreateLogger<LaunchManual>();
            this.executor = executor;
        }

        [Function("LaunchManual")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
        {
            try
            {
                await executor.Execute();
                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                response.WriteString("ok");
                return response;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                var response = req.CreateResponse(HttpStatusCode.InternalServerError);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                response.WriteString(ex.ToString());
                return response;
            }
        }
    }
}
