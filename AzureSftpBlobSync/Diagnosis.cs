using System.Net;
using AzureSftpBlobSync.JobConfigs;
using AzureSftpBlobSync.Providers.AzureBlobProvider;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AzureSftpBlobSync
{
    public class Diagnosis
    {
        private readonly ILogger _logger;
        private readonly IConfigBuilder config;

        public Diagnosis(ILoggerFactory loggerFactory, IConfigBuilder config)
        {
            _logger = loggerFactory.CreateLogger<Diagnosis>();
            this.config = config;
        }



        [Function("Diagnosis")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            try
            {
                var jsonConfig = await config.BuildConfig();
                response.WriteString("Sftp config:" + Environment.NewLine);
                response.WriteString("------------" + Environment.NewLine);
                foreach (var sftpConfig in jsonConfig.SftpAccounts)
                {
                    response.WriteString(sftpConfig.Id.ToString() + Environment.NewLine);
                    try
                    {
                        SftpWrapper wrapper = new SftpWrapper(sftpConfig);
                        wrapper.Connect();
                        response.WriteString("ok" + Environment.NewLine);

                    }
                    catch (Exception ex)
                    {
                        response.WriteString($"Failed to connect {ex.Message}" + Environment.NewLine);
                    }
                }

                response.WriteString("Pgp keys config:" + Environment.NewLine);
                response.WriteString("------------" + Environment.NewLine);
                
                foreach (var pgpKey in jsonConfig.PgpKeys)
                {
                    PgpWrapper pgp = new PgpWrapper(pgpKey.KeyValue);
                    response.WriteString(pgpKey.Id.ToString() + Environment.NewLine);
                    try
                    {
                        MemoryStream stream = new MemoryStream(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 });
                        MemoryStream output = new MemoryStream();
                        pgp.Encrypt(stream, output);
                    }
                    catch (Exception ex)
                    {
                        response.WriteString($"Failed to use pgp key: {ex.Message}" + Environment.NewLine);
                    }
                }

                response.WriteString("Blob config:" + Environment.NewLine);
                response.WriteString("------------" + Environment.NewLine);
                foreach (var blobConfig in jsonConfig.BlobAccounts)
                {
                    response.WriteString(blobConfig.Id.ToString() + Environment.NewLine);
                    try
                    {
                        AzureBlobWrapper wrapper = new AzureBlobWrapper(blobConfig);
                        await wrapper.Dir("/", false);
                        response.WriteString("ok" + Environment.NewLine);
                    }
                    catch (Exception ex)
                    {
                        response.WriteString($"Failed to connect {ex.Message}" + Environment.NewLine);
                    }
                }

                response.WriteString("Jobs config:" + Environment.NewLine);
                response.WriteString("------------" + Environment.NewLine);
                response.WriteString($"{jsonConfig.JobDefinitions.Count()} jobs detected");

            }
            catch (Exception ex)
            {
                response.WriteString(ex.Message);
            }

            response.WriteString("End");
            return response;
        }
    }
}
