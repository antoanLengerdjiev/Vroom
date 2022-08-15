using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vroom.Providers.Contracts
{
    public interface IEncryptionProvider
    {
        string GetPublicKey();

        string Encrypt(int text);

        int Decrypt(string cypherText);
    }
}
