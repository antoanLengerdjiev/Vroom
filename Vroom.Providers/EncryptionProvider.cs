using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Vroom.Providers.Contracts;

namespace Vroom.Providers
{
    public class EncryptionProvider : IEncryptionProvider
    {
        private static RSACryptoServiceProvider csp = new RSACryptoServiceProvider(2048);
        private RSAParameters _privateKey;
        private RSAParameters _publicKey;

        public EncryptionProvider()
        {
            this._privateKey = csp.ExportParameters(true);
            this._publicKey = csp.ExportParameters(false);
        }
        public int Decrypt(string cypherText)
        {
            var dataBytes = Convert.FromBase64String(cypherText);
            csp.ImportParameters(this._privateKey);
            var text = csp.Decrypt(dataBytes, false);
            int result;
            if(!int.TryParse(Encoding.Unicode.GetString(text), out result))
            {
                return 0;
            }

            return result;
        }

        public string Encrypt(int text)
        {
            csp = new RSACryptoServiceProvider();
            csp.ImportParameters(this._publicKey);
            var data = Encoding.Unicode.GetBytes(text.ToString());
            var cypher = csp.Encrypt(data, false);
            return Convert.ToBase64String(cypher);
        }

        public string GetPublicKey()
        {
            var sw = new StringWriter();
            var xs = new XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, this._publicKey);
            return sw.ToString();
        }
    }
}
