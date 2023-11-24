using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using AzureSftpBlobSync.JobConfigs;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSftpBlobSync
{
    public class AzureBlobWrapper 
    {
        private BlobServiceClient blobServiceClient;
        private BlobContainerClient blobContainerClient;

        public AzureBlobWrapper(BlobConfig config)
        {
            this.blobServiceClient = new BlobServiceClient(config.ConnectionString);
            this.blobContainerClient = blobServiceClient.GetBlobContainerClient(config.ContainerName);
        }

        /// <summary>
        /// returns the name of the url only if the blob exits, null in other case
        /// </summary>
        /// <param name="blobName"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string?> GetBlobUrl(string blobName)
        {
            var blockBlobClient = blobContainerClient.GetBlockBlobClient(blobName);
            var result = await blockBlobClient.ExistsAsync();
            if (result.Value)
            {
                return blockBlobClient.Uri.ToString();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the full url of the blobname, without evaluating if the blob exits or not.
        /// </summary>
        /// <param name="blobName">Name of the BLOB.</param>
        /// <returns></returns>
        public string GetBlobUrlName(string blobName)
        {
            var blockBlobClient = blobContainerClient.GetBlockBlobClient(blobName);
            return blockBlobClient.Uri.ToString();
        }

        public async Task<byte[]?> ReadBlob(string blobName)
        {
            var blockBlobClient = blobContainerClient.GetBlockBlobClient(blobName);
            if (!await blockBlobClient.ExistsAsync())
            {
                return null;
            }

            var response = await blockBlobClient.DownloadContentAsync();
            var result = response.Value.Content.ToArray();
            return result;
        }

        public async Task WriteBlob(string blobName, byte[] value)
        {
            var blockBlobClient = blobContainerClient.GetBlockBlobClient(blobName);
            using (var ms = new MemoryStream(value))
            {
                await blockBlobClient.UploadAsync(ms);
            }
        }

        public async Task WriteBlob(string blobName, Stream stream)
        {
            await blobContainerClient.UploadBlobAsync(blobName, stream);
        }

        public async Task MoveBlob(string blobNameFrom, string blobNameTo)
        {
            var blockBlobClientTo = blobContainerClient.GetBlockBlobClient(blobNameTo);
            var blockBlobClientoFrom = blobContainerClient.GetBlockBlobClient(blobNameFrom);
            var operation = blockBlobClientTo.StartCopyFromUri(blockBlobClientoFrom.Uri);
            await operation.WaitForCompletionAsync();
            await blockBlobClientoFrom.DeleteAsync();
        }

        public async Task<IEnumerable<string>> Dir(string folder, bool recursive)
        {
            if (folder.StartsWith(@"/"))
            {
                folder = folder.Substring(1, folder.Length - 1);
            }
            if (!folder.EndsWith(@"/"))
            {
                folder = folder + "/";
            }

            List<string> result = new List<string>();
            //return result;
            // Call the listing operation and return pages of the specified size.
            var resultSegment = blobContainerClient.GetBlobsByHierarchyAsync(prefix: folder, delimiter: "/")
                .AsPages(default, 50);

            // Enumerate the blobs returned for each page.
            await foreach (Page<BlobHierarchyItem> blobPage in resultSegment)
            {
                // A hierarchical listing may return both virtual directories and blobs.
                foreach (BlobHierarchyItem blobhierarchyItem in blobPage.Values)
                {
                    if (blobhierarchyItem.IsPrefix && recursive)
                    {
                        // Write out the prefix of the virtual directory.
                        Console.WriteLine("Virtual directory prefix: {0}", blobhierarchyItem.Prefix);

                        // Call recursively with the prefix to traverse the virtual directory.
                        var resultRecursive=await Dir(blobhierarchyItem.Prefix, true);
                        result.AddRange(resultRecursive); ;
                    }
                    else
                    {
                        // Write out the name of the blob.
                        result.Add(blobhierarchyItem.Blob.Name);
                    }
                }
            }

            return result;
        }

        public async Task DeleteBlob(string blobName)
        {
            var blockBlobClient = blobContainerClient.GetBlockBlobClient(blobName);
            await blockBlobClient.DeleteIfExistsAsync();
        }

        public bool Exists(string file)
        {
            var blob = blobContainerClient.GetBlobClient(file);
            return blob.Exists();
        }

        public DateTimeOffset GetLastModificationDate(string file)
        {
            var blob = blobContainerClient.GetBlobClient(file);
            return blob.GetProperties().Value.LastModified;
        }

        public async Task<Stream> GetStreamReader(string file)
        {
            if (!this.Exists(file))
            {
                throw new ArgumentNullException($"File {file} doesnt exist");
            }

            var blob = blobContainerClient.GetBlobClient(file);
            var stream = await blob.OpenReadAsync();
            return stream;
        }

        public async Task<Stream> GetStreamWriter(string blobName)
        {
            var blockBlobClient = blobContainerClient.GetBlockBlobClient(blobName);
            var stream = await blockBlobClient.OpenWriteAsync(true);
            return stream;
        }
    }
}
