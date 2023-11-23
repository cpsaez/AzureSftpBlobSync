﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSftpBlobSync.JobConfigs
{
    public class AccountConfig
    {
        public AccountConfig() { 
            this.Url = string.Empty;
            this.Port = 0;
            this.UserName = string.Empty;
            this.Password = string.Empty;
            this.PrivateKey = string.Empty;
            this.PrivateKeyPassPhrase = string.Empty;
        }

        public string Url { get; set; }

        public int Port { get; set; }
        public string UserName { get; set; }

        public string Password { get; set; }

        public string PrivateKey { get; set; }

        public string PrivateKeyPassPhrase { get; set; }
    }
}