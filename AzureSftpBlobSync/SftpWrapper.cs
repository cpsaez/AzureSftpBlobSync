﻿using AzureSftpBlobSync.JobConfigs;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AzureSftpBlobSync
{
    public class SftpWrapper : IDisposable
    {
        private SftpClient client;

        public SftpWrapper(SftpAccountConfig config)
        {
            this.client = BuildSftpClient(config);
        }

        public IEnumerable<string> Dir(string sftpFolder, bool sftpFolderRecursiveEnabled)
        {
            this.Connect();
            if (!this.client.Exists(sftpFolder))
            {
                return Enumerable.Empty<string>();
            }

            var listDirectory=this.client.ListDirectory(sftpFolder);
            if (listDirectory == null) return Array.Empty<string>();
            List<string> result = new List<string>();
            foreach (var entry in listDirectory) { 
                if (entry.IsDirectory && sftpFolderRecursiveEnabled)
                {
                    result.AddRange(Dir(entry.FullName, true));
                }   
                else if (entry.IsRegularFile)
                {
                    result.Add(entry.FullName);
                }
            }

            return result;
        }

        public Stream OpenReadStream(string file)
        {
            this.Connect();
            var stream=this.client.OpenRead(file);
            return stream;
        }

        public void WriteStream(Stream reader, string destination)
        {
            Connect();
            this.client.UploadFile(reader, destination);
        }

        public void WriteBytes(byte[] bytes, string destination)
        {
            Connect();
            this.client.WriteAllBytes(destination, bytes);
        }

        public void DeleteFile(string file)
        {
            this.Connect();
            this.client.Delete(file);
        }

        public void Connect()
        {
            if (!this.client.IsConnected) this.client.Connect();
        }

        public void Dispose()
        {
            try
            {
                if (this.client!=null && this.client.IsConnected)
                {
                    this.client.Disconnect();
                    this.client.Dispose();
                }
            }
            catch { }
        }

        private SftpClient BuildSftpClient(SftpAccountConfig sftpConfig)
        {
            if (sftpConfig == null) { throw new ArgumentNullException(nameof(sftpConfig)); }

            List<AuthenticationMethod> authMethods = new List<AuthenticationMethod>();
            if (!string.IsNullOrWhiteSpace(sftpConfig.Password))
            {
                authMethods.Add(new PasswordAuthenticationMethod(sftpConfig.UserName, sftpConfig.Password));
            }

            if (!string.IsNullOrWhiteSpace(sftpConfig.PrivateKey))
            {
                var keystrm = new MemoryStream(Encoding.ASCII.GetBytes(sftpConfig.PrivateKey));
                var privateKey = new PrivateKeyFile(keystrm, sftpConfig.PrivateKeyPassPhrase);
                authMethods.Add(new PrivateKeyAuthenticationMethod(sftpConfig.UserName, new[] { privateKey }));
            }

            ConnectionInfo connectionInfo = new ConnectionInfo(sftpConfig.Url, sftpConfig.Port, sftpConfig.UserName, authMethods.ToArray());
            SftpClient client = new SftpClient(connectionInfo);
            return client;
        }


    }
}
