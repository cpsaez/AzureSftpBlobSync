using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AzureSftpBlobSync.JobConfigs
{
    public class PgpKey
    {
        public PgpKey()
        {
            this.Name= string.Empty;
            this.KeyValue=Array.Empty<byte>();
            this.KeyPath= string.Empty;
        }
        public string Name { get; set; }
        public int Id { get; set; }
        public string KeyPath { get; set; }

        [JsonIgnore]
        public byte[] KeyValue { get; set; }
    }
}
