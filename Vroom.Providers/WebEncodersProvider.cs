using Bytes2you.Validation;
using Microsoft.AspNetCore.WebUtilities;
using Vroom.Common;
using Vroom.Providers.Contracts;

namespace Vroom.Providers
{
    public class WebEncodersProvider : IWebEncodersProvider
    {
        public byte[] Base64UrlDecode(string code)
        {
            Guard.WhenArgument<string>(code, GlobalConstants.GetMemberName(() => code)).IsNullOrEmpty().Throw();

            return WebEncoders.Base64UrlDecode(code);
        }

        public string Base64UrlEncode(byte[] bytes)
        {
            Guard.WhenArgument<byte[]>(bytes, GlobalConstants.GetMemberName(() => bytes)).IsNull().Throw();

            return WebEncoders.Base64UrlEncode(bytes);
        }
    }
}
