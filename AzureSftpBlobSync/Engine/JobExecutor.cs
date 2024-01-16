using AzureSftpBlobSync.JobConfigs;
using AzureSftpBlobSync.JobConfigs.StorageAccounts;
using AzureSftpBlobSync.Providers.AzureBlobProvider;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.Extensions.Logging;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AzureSftpBlobSync.Engine
{
    public class JobExecutor
    {
        private ILogger<JobsExecutor> logger;
        private JobDefinition jobDefinition;
        private Config blobConfig;
        private SftpAccountConfig sftpConfig;
        private PgpKey? pgpConfig;

        public JobExecutor(JobDefinition jobDefinition, IEnumerable<Config> blobConfigs, IEnumerable<SftpAccountConfig> sftpconfigs, IEnumerable<PgpKey> pgpKeys, ILogger<JobsExecutor> logger)
        {
            this.logger = logger;
            this.jobDefinition = jobDefinition;
            var blobConfig = blobConfigs.Where(x => x.Id == jobDefinition.BlobAccountId).FirstOrDefault();
            if (blobConfig == null) throw new ArgumentException($"JobDefinition.BlobAccountId is {jobDefinition.BlobAccountId} but that Id dosen't exist in BlobConfigs");
            this.blobConfig = blobConfig;

            var sftpConfig = sftpconfigs.Where(x => x.Id == jobDefinition.SftpAccountId).FirstOrDefault();
            if (sftpConfig == null) throw new ArgumentException($"JobDefinition.SftpAccountId is {jobDefinition.SftpAccountId} but that Id dosen't exist in SftpAccounts");
            this.sftpConfig = sftpConfig;

            this.pgpConfig = null;
            if (jobDefinition.PGPEncryptWithKey > 0)
            {
                var pgpConfig = pgpKeys.Where(x => x.Id == jobDefinition.PGPEncryptWithKey).FirstOrDefault();
                if (pgpConfig == null) throw new ArgumentException($"JobDefinition.PGPEncryptWithKey is {jobDefinition.PGPEncryptWithKey} but that Id dosen't exist in PgpKeys");
                this.pgpConfig = pgpConfig;
            }
        }

        public async Task Execute()
        {
            var sftp = this.BuildSftpClient(this.sftpConfig);
            var blob = this.BuildBlobClient(this.blobConfig);
            PgpWrapper? pgpClient = null;
            if (this.pgpConfig != null)
            {
                pgpClient = this.BuildPgpWrapper(this.pgpConfig);
            }

            var jobType = this.jobDefinition.JobType;

            if (jobType == JobType.MoveFromSftpToBlobStorage || jobType == JobType.CopyFromSftpToBlobStorage)
            {
                await FromSftpToBlobStorage(sftp, blob, pgpClient);
            }
            else if (jobType == JobType.MoveFromBlobStorageToSftp || jobType == JobType.CopyFromBlobStorageToSftp)
            {
                await FromBlobStorageToSftp(sftp, blob, pgpClient);
            }
        }

        private async Task FromSftpToBlobStorage(SftpWrapper sftp, AzureBlobWrapper blob, PgpWrapper? pgpClient)
        {
            var filesToCopy = sftp.Dir(this.jobDefinition.SftpFolder, this.jobDefinition.SftpFolderRecursiveEnabled);
            foreach (var file in filesToCopy)
            {
                logger.LogInformation($"Trying file from sftp {file}");
                try
                {
                    var destination = PathHelper.CalculateDestiny(this.jobDefinition.SftpFolder, this.jobDefinition.BlobFolder, file);
                    using (var reader = sftp.OpenReadStream(file))
                    {
                        if (pgpClient != null)
                        {
                            MemoryStream pgpStream = new MemoryStream();
                            pgpClient.Encrypt(reader, pgpStream);
                            pgpStream.Seek(0, SeekOrigin.Begin);
                            await blob.WriteBlob(destination, pgpStream);
                        }
                        else
                        {
                            await blob.WriteBlob(destination, reader);
                        }
                    }

                    logger.LogInformation($"Copied file from sftp {file}");

                    if (this.jobDefinition.JobType == JobType.MoveFromSftpToBlobStorage)
                    {
                        sftp.DeleteFile(file);
                        logger.LogInformation($"Delete file from sftp {file}");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.ToString(), ex);
                    throw new Exception($"error handling the file {file}", ex);
                }
            }
        }

        private async Task FromBlobStorageToSftp(SftpWrapper sftp, AzureBlobWrapper blob, PgpWrapper? pgpClient)
        {
            var filesToCopy = await blob.Dir(this.jobDefinition.BlobFolder, this.jobDefinition.BlobFolderRecursiveEnabled);
            foreach (var file in filesToCopy)
            {
                logger.LogInformation($"Trying file from blob {file}");
                try
                {
                    var destination = PathHelper.CalculateDestiny(this.jobDefinition.BlobFolder, this.jobDefinition.SftpFolder, file);
                    using (var reader = await blob.GetStreamReader(file))
                    {
                        if (pgpClient != null)
                        {
                            MemoryStream pgpStream = new MemoryStream();
                            pgpClient.Encrypt(reader, pgpStream);
                            pgpStream.Seek(0, SeekOrigin.Begin);
                            sftp.WriteStream(pgpStream, destination);
                        }
                        else
                        {
                            sftp.WriteStream(reader, destination);
                        }

                        logger.LogInformation($"Copied file from blob {file}");
                    }

                    if (this.jobDefinition.JobType == JobType.MoveFromBlobStorageToSftp)
                    {
                        await blob.DeleteBlob(file);
                        logger.LogInformation($"deleted file from blob {file}");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.ToString(), ex);
                    throw new Exception($"error handling the file {file}", ex);
                }
            }
        }

        private AzureBlobWrapper BuildBlobClient(Config blobConfig)
        {
            var result = new AzureBlobWrapper(blobConfig);
            return result;
        }

        private SftpWrapper BuildSftpClient(SftpAccountConfig config)
        {
            SftpWrapper result = new SftpWrapper(config);
            return result;
        }

        private PgpWrapper BuildPgpWrapper(PgpKey key)
        {
            var result = new PgpWrapper(key.KeyValue);
            return result;
        }
    }
}
