using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSftpBlobSync.JobConfigs
{
    public class JobDefinition
    {
        public JobDefinition() { 
            this.Name = string.Empty;
            this.SftpFolder = string.Empty;
            this.BlobFolder = string.Empty;
            this.JobType = JobType.None;
            this.SaveInJournalBeforeDeleteFromBlobStoragePath = string.Empty;
            this.BlobFolderRecursiveEnabled = false; 
            this.BlobSftpFolderRecursiveEnabled = false;

        } 

        public string Name { get; set; }
        public int SftpAccountId { get; set; }
        public int BlobAccountId { get; set; }
        public string SftpFolder { get; set; }
        public string BlobFolder { get; set; }
        public JobType JobType { get; set; }
        public string SaveInJournalBeforeDeleteFromBlobStoragePath { get; set; }

        public bool BlobFolderRecursiveEnabled { get; set; }

        public bool BlobSftpFolderRecursiveEnabled { get; set; }
    }
}
