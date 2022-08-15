using AutoMapper;
using Bytes2you.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vroom.Common;
using Vroom.Models.Contracts;
using Vroom.Providers.Contracts;
using Vroom.Service.Models.Contracts;

namespace Vroom.MappingProfiles.Resolvers
{
    public class MakeIdDecodeResolver : IValueResolver<IStringMakeIdable, IIntMakeIdable, int>
    {
        private readonly IEncryptionProvider encryptionProvider;
        public MakeIdDecodeResolver(IEncryptionProvider encryptionProvider)
        {
            Guard.WhenArgument<IEncryptionProvider>(encryptionProvider, GlobalConstants.GetMemberName(() => encryptionProvider)).IsNull().Throw();
            this.encryptionProvider = encryptionProvider;
        }
        public int Resolve(IStringMakeIdable source, IIntMakeIdable destination, int destMember, ResolutionContext context)
        {
            return this.encryptionProvider.Decrypt(source.MakeId);
        }
    }
}
