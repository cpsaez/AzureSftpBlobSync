using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSftpBlobSync.JobConfigs
{
    public enum JobType
    {
        None = 0,
        CopyFromSftpToBlobStorage = 10, 
        MoveFromSftpToBlobStorage = 20, 
        CopyFromBlobStorageToSftp =30, 
        MoveFromBlobStorageToSftp = 40,
        Sync = 50 // Put the missing files in blob storage from sftp and viceversa. Don't delete anything.
    }
}
