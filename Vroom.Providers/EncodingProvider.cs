using Bytes2you.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vroom.Common;
using Vroom.Providers.Contracts;

namespace Vroom.Providers
{
    public class EncodingProvider : IEncodingProvider
    {
        public byte[] UTF8GetBytes(string code)
        {

            Guard.WhenArgument<string>(code, GlobalConstants.GetMemberName(() => code)).IsNullOrEmpty().Throw();

            return Encoding.UTF8.GetBytes(code);
        }

        public string UTF8GetString(byte[] bytes)
        {
            Guard.WhenArgument<byte[]>(bytes, GlobalConstants.GetMemberName(() => bytes)).IsNull().Throw();

            return Encoding.UTF8.GetString(bytes);
        }
    }
}
