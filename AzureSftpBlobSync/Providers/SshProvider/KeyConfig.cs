using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AzureSftpBlobSync.Providers.SshProvider
{
    public class KeyConfig
    {
        public KeyConfig()
        {
            this.PrivateKeyFilePath = string.Empty;
            this.PrivateKey = string.Empty;
            this.PrivateKeyPassPhrase = string.Empty;
            this.Name = string.Empty;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public string PrivateKeyPassPhrase { get; set; }
        public string PrivateKeyFilePath { get; set; }

        [JsonIgnore]
        public string PrivateKey { get; set; }
    }
}
