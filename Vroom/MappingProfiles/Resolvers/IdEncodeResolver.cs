using AutoMapper;
using Bytes2you.Validation;
using Vroom.Common;
using Vroom.Models;
using Vroom.Models.Contracts;
using Vroom.Providers.Contracts;

using Vroom.Service.Models.Contracts;

namespace Vroom.MappingProfiles.Resolvers
{
    public class IdEncodeResolver : IValueResolver<IIntIdalbe, IStringIdable, string>
    {
        private readonly IEncryptionProvider encryptionProvider;
        public IdEncodeResolver(IEncryptionProvider encryptionProvider)
        {
            Guard.WhenArgument<IEncryptionProvider>(encryptionProvider, GlobalConstants.GetMemberName(() => encryptionProvider)).IsNull().Throw();
            this.encryptionProvider = encryptionProvider;
        }
        public string Resolve(IIntIdalbe source, IStringIdable destination, string destMember, ResolutionContext context)
        {
            return this.encryptionProvider.Encrypt(source.Id);
        }
    }
}
