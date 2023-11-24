using AzureSftpBlobSync.JobConfigs;
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
        private JobDefinition jobDefinition;
        private BlobConfig blobConfig;
        private SftpAccountConfig sftpConfig;

        public JobExecutor(JobDefinition jobDefinition, IEnumerable<BlobConfig> blobConfigs, IEnumerable<SftpAccountConfig> sftpconfigs)
        {
            this.jobDefinition = jobDefinition;
            var blobConfig = blobConfigs.Where(x => x.Id == jobDefinition.BlobAccountId).FirstOrDefault();
            if (blobConfig == null) throw new ArgumentException($"JobDefinition.BlobAccountId is {jobDefinition.BlobAccountId} but that Id dosen't exist in BlobConfigs");
            this.blobConfig = blobConfig;

            var sftpConfig = sftpconfigs.Where(x => x.Id == jobDefinition.SftpAccountId).FirstOrDefault();
            if (sftpConfig == null) throw new ArgumentException($"JobDefinition.SftpAccountId is {jobDefinition.SftpAccountId} but that Id dosen't exist in SftpAccounts");
            this.sftpConfig = sftpConfig;
        }

        public async Task Execute()
        {
            var sftp = this.BuildSftpClient(this.sftpConfig);
            var blob = this.BuildBlobClient(this.blobConfig);
            var jobType = this.jobDefinition.JobType;

            if (jobType == JobType.MoveFromSftpToBlobStorage || jobType == JobType.CopyFromSftpToBlobStorage)
            {
                FromSftpToBlobStorage(sftp, blob);
            }
            else if (jobType == JobType.MoveFromBlobStorageToSftp || jobType == JobType.CopyFromBlobStorageToSftp)
            {

            }
        }

        private void FromSftpToBlobStorage(SftpWrapper sftp, AzureBlobWrapper blob)
        {
            var filesToCopy = sftp.Dir(this.jobDefinition.SftpFolder, this.jobDefinition.SftpFolderRecursiveEnabled);
            foreach (var file in filesToCopy)
            {

                using (var reader = sftp.OpenReadStream(file))
                {
                    

                }
            }
        }

        private AzureBlobWrapper BuildBlobClient(BlobConfig blobConfig)
        {
            var result = new AzureBlobWrapper(blobConfig);
            return result;
        }

        private SftpWrapper BuildSftpClient(SftpAccountConfig config)
        {
            SftpWrapper result = new SftpWrapper(config);
            return result;
        }
    }
}
