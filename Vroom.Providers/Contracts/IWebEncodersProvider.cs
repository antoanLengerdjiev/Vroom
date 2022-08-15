using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vroom.Providers.Contracts
{
    public interface IWebEncodersProvider
    {
        byte[] Base64UrlDecode(string code);

        string Base64UrlEncode(byte[] bytes);
    }
}
