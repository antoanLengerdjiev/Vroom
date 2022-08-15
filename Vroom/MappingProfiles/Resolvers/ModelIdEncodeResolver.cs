using AutoMapper;
using Bytes2you.Validation;
using Vroom.Common;
using Vroom.Models.Contracts;
using Vroom.Providers.Contracts;
using Vroom.Service.Models.Contracts;

namespace Vroom.MappingProfiles.Resolvers
{
    public class ModelIdEncodeResolver : IValueResolver<IIntModelIdable, IStringModelIdable, string>
    {
        private readonly IEncryptionProvider encryptionProvider;
        public ModelIdEncodeResolver(IEncryptionProvider encryptionProvider)
        {
            Guard.WhenArgument<IEncryptionProvider>(encryptionProvider, GlobalConstants.GetMemberName(() => encryptionProvider)).IsNull().Throw();
            this.encryptionProvider = encryptionProvider;
        }
        public string Resolve(IIntModelIdable source, IStringModelIdable destination, string destMember, ResolutionContext context)
        {
            return this.encryptionProvider.Encrypt(source.ModelId);
        }
    }
}
