using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureSftpBlobSync.Tests
{
    public class PgpWrapperTest
    {
        private byte[] key;
        private PgpWrapper wrapper;

        [SetUp]
        public void Setup()
        {
            this.key = ResourceForTest.FOINAC1I;
            this.wrapper = new PgpWrapper(this.key);
        }

        [Test]
        public void PgpEncryptTest()
        {
            MemoryStream output=new MemoryStream();
            MemoryStream intput = new MemoryStream(ResourceForTest.exampleFile);
            this.wrapper.Encrypt(intput, output);
            var data=output.ToArray();
            var result = Encoding.UTF8.GetString(data);
            Assert.NotNull(result);
        }
    }
}
