using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSftpBlobSync.Providers
{
    public abstract class StorageAccountConfigBase
    {
        public StorageAccountConfigBase()
        {
            Name = string.Empty;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public abstract string StorageAccountType { get; }
    }
}
}
