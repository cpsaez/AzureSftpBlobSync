using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSftpBlobSync.JobConfigs
{
    public class SftpAccountConfig
    {
        public SftpAccountConfig() { 
            this.Url = string.Empty;
            this.Port = 0;
            this.UserName = string.Empty;
            this.Password = string.Empty;
            this.PrivateKey = string.Empty;
            this.PrivateKeyPassPhrase = string.Empty;
        }
        public int Id { get; set; }
        public string Url { get; set; }

        public int Port { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }

        public string PrivateKey { get; set; }

        public string PrivateKeyPassPhrase { get; set; }

        public override string ToString()
        {
            return $"{Id} : {Url} : {Port} ";
        }
    }
}
