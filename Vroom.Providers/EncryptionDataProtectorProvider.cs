using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vroom.Providers.Contracts;
using Vroom.Providers.EncodingHelpers;

namespace Vroom.Providers
{
    public class EncryptionDataProtectorProvider : IEncryptionProvider
    {
        private readonly IDataProtector dataProtector;

        public EncryptionDataProtectorProvider(IDataProtectionProvider dataProtectorProvider, UniqueCode uniqueCode)
        {
            this.dataProtector = dataProtectorProvider.CreateProtector(uniqueCode.Key);
        }
        public int Decrypt(string cypherText)
        {
            int result = 0;
            if (!int.TryParse(this.dataProtector.Unprotect(cypherText), out result))
            {
                return 0;
            }
            return result;
        }

        public string Encrypt(int text)
        {
            return this.dataProtector.Protect(text.ToString());
        }

        public string GetPublicKey()
        {
            throw new NotImplementedException();
        }
    }
}
