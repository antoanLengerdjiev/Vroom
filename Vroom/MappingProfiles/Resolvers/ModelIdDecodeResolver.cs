using AutoMapper;
using Bytes2you.Validation;
using Vroom.Common;
using Vroom.Models.Contracts;
using Vroom.Providers.Contracts;
using Vroom.Service.Models.Contracts;

namespace Vroom.MappingProfiles.Resolvers
{
    public class ModelIdDecodeResolver : IValueResolver<IStringModelIdable, IIntModelIdable, int>
    {
        private readonly IEncryptionProvider encryptionProvider;
        public ModelIdDecodeResolver(IEncryptionProvider encryptionProvider)
        {
            Guard.WhenArgument<IEncryptionProvider>(encryptionProvider, GlobalConstants.GetMemberName(() => encryptionProvider)).IsNull().Throw();
            this.encryptionProvider = encryptionProvider;
        }
        public int Resolve(IStringModelIdable source, IIntModelIdable destination, int destMember, ResolutionContext context)
        {
            return this.encryptionProvider.Decrypt(source.ModelId);
        }
    }
}
