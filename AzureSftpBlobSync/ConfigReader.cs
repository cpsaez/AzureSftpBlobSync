using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using AzureSftpBlobSync.JobConfigs;
using static System.Net.WebRequestMethods;
using AzureSftpBlobSync.Providers.StorageProviders.AzureBlobProvider;
using AzureSftpBlobSync.Providers.StorageProviders;
using Grpc.Net.Client.Configuration;

namespace AzureSftpBlobSync
{
    public interface IConfigReader
    {
        //Task<AzureSftpBlobSyncConfig> BuildConfig();
        IProviderStreamer GetReader();

        string GetConfigFolder();
    }

    public class ConfigReader : IConfigReader
    {
        public string GetConfigFolder()
        {
            var result= Environment.GetEnvironmentVariable("SftpConfigPath");
            if (result == null) return string.Empty;
            return result;
        }

        public IProviderStreamer GetReader()
        {
            var connString = Environment.GetEnvironmentVariable("SftpConfigStorageConnectionString");
            var container = Environment.GetEnvironmentVariable("SftpConfigStorageContainer");
            var path = Environment.GetEnvironmentVariable("SftpConfigPath");

            if (string.IsNullOrWhiteSpace(connString))
            {
                throw new ArgumentException("Connection string is null, be sure 'SftpConfigStorageConnectionString' is setup in app config");
            }

            if (string.IsNullOrWhiteSpace(container))
            {
                throw new ArgumentException("container is null, be sure 'SftpConfigStorageContainer' is setup in app config");
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("path is null, be sure 'SftpConfigPath' is setup in app config");
            }

            AzureBlobWrapper wrapper = new AzureBlobWrapper(new Config()
            {
                ConnectionString = connString,
                ContainerName = container
            });

            return wrapper;
        }

        //public async Task<AzureSftpBlobSyncConfig> BuildConfig()
        //{
        //    var connString = Environment.GetEnvironmentVariable("SftpConfigStorageConnectionString");
        //    var container = Environment.GetEnvironmentVariable("SftpConfigStorageContainer");
        //    var path = Environment.GetEnvironmentVariable("SftpConfigPath");

        //    if (string.IsNullOrWhiteSpace(connString))
        //    {
        //        throw new ArgumentException("Connection string is null, be sure 'SftpConfigStorageConnectionString' is setup in app config");
        //    }

        //    if (string.IsNullOrWhiteSpace(container))
        //    {
        //        throw new ArgumentException("container is null, be sure 'SftpConfigStorageContainer' is setup in app config");
        //    }

        //    if (string.IsNullOrWhiteSpace(path))
        //    {
        //        throw new ArgumentException("path is null, be sure 'SftpConfigPath' is setup in app config");
        //    }

        //    AzureBlobWrapper wrapper = new AzureBlobWrapper(new BlobConfig()
        //    {
        //        ConnectionString = connString,
        //        ContainerName = container
        //    });

        //    try
        //    {
        //        var configJson = await wrapper.ReadBlob(path);
        //        if (configJson == null) { throw new Exception($"The blob file {path} is empty"); }
        //        var jsonString = Encoding.UTF8.GetString(configJson);

        //        var options = new JsonSerializerOptions();
        //        options.Converters.Add(new JsonStringEnumConverter());
        //        var result = JsonSerializer.Deserialize<AzureSftpBlobSyncConfig>(jsonString, options);
        //        if (result == null)
        //        {
        //            throw new Exception($"The blob file {path} cannot be parsed");
        //        }

        //        // we need to fill the content of the PrivateKeys
        //        foreach (var stp in result.SftpAccounts)
        //        {
        //            if (!string.IsNullOrWhiteSpace(stp.PrivateKeyFileName))
        //            {
        //                var keyWrapper = await wrapper.ReadBlob(stp.PrivateKeyFileName);
        //                if (keyWrapper == null) { throw new Exception($"The private key {stp.PrivateKeyFileName} is null"); }
        //                stp.PrivateKey = Encoding.UTF8.GetString(keyWrapper);
        //            }
        //        }

        //        if (result.PgpKeys != null)
        //        {
        //            foreach (var pgpKey in result.PgpKeys)
        //            {
        //                var keyWrapper = await wrapper.ReadBlob(pgpKey.KeyPath);
        //                if (keyWrapper == null)
        //                {
        //                    throw new Exception($"The pgp key {pgpKey.KeyPath} is null");
        //                }

        //                pgpKey.KeyValue = keyWrapper;
        //            }
        //        }

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error reading configuration in path: {path}, {ex}", ex);
        //    }
        //}


    }
}
