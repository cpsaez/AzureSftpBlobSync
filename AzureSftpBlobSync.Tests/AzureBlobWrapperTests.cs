using AzureSftpBlobSync.Providers.AzureBlobProvider;
using Microsoft.Extensions.Azure;

namespace AzureSftpBlobSync.Tests
{
    public class AzureBlobWrapperTest
    {
        private AzureBlobWrapper wrapper;

        [SetUp]
        public void Setup()
        {
            this.wrapper = new AzureBlobWrapper(new JobConfigs.BlobConfig()
            {
                ConnectionString = "UseDevelopmentStorage=true",
                ContainerName = "azuresftpblobsync"
            });
        }

        [Test]
        public async Task DirTestREcursive()
        {
            try
            {
                await wrapper.WriteBlob("/azuresftpblobsynctest/subfoder1/hola.txt", new byte[] { 1, 2, 3, });
                await wrapper.WriteBlob("/azuresftpblobsynctest/subfoder2/hola2.txt", new byte[] { 1, 2, 3, });
                await wrapper.WriteBlob("/azuresftpblobsynctest/subfoder2/hola3.txt", new byte[] { 1, 2, 3, });
                await wrapper.WriteBlob("/azuresftpblobsynctestanotherone/subfoder2/hola3.txt", new byte[] { 1, 2, 3, });

                var result = await wrapper.Dir("/azuresftpblobsynctest", true);
                Assert.That(result.Count(), Is.EqualTo(3));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Test]
        public async Task DirMultipleFiles()
        {
            try
            {
                for (int i = 0; i < 150; i++)
                {
                    await wrapper.WriteBlob($"/azuresftpblobsynctest2/subfoder1/{i}.txt", new byte[] { 1, 2, 3, });
                }

                var result = await wrapper.Dir("/azuresftpblobsynctest2", true);
                Assert.That(result.Count(), Is.EqualTo(150));

                await wrapper.DeleteBlob("/azuresftpblobsynctest2/subfoder1/1.txt");

                result = await wrapper.Dir("/azuresftpblobsynctest2", true);
                Assert.That(result.Count(), Is.EqualTo(149));
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}