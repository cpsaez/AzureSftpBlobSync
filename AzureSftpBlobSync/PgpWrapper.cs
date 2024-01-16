using Azure.Core.Cryptography;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Zlib;
using PgpCore;
using PgpCore.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AzureSftpBlobSync
{
    public class PgpWrapper
    {
        private EncryptionKeys pgpKey;

        public PgpWrapper(byte[] pgpKey)
        {
            this.pgpKey = ReadPublicKey(pgpKey);
        }
        public void Encrypt(Stream dataToEncrypt,Stream outputStream)
        {
            PGP pgp = new PGP(this.pgpKey);
            pgp.Encrypt(dataToEncrypt, outputStream, true, true);
        }

        internal static EncryptionKeys ReadPublicKey(byte[] key)
        {
            using (MemoryStream stream = new MemoryStream(key))
            {
                return new EncryptionKeys(stream); 
            }
        }
    }
}
