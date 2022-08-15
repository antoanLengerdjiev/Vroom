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
    public class MakeIdEncodeResolver : IValueResolver<IIntMakeIdable, IStringMakeIdable, string>
    {
        private readonly IEncryptionProvider encryptionProvider;
        public MakeIdEncodeResolver(IEncryptionProvider encryptionProvider)
        {
            Guard.WhenArgument<IEncryptionProvider>(encryptionProvider, GlobalConstants.GetMemberName(() => encryptionProvider)).IsNull().Throw();
            this.encryptionProvider = encryptionProvider;
        }
        public string Resolve(IIntMakeIdable source, IStringMakeIdable destination, string destMember, ResolutionContext context)
        {
            return this.encryptionProvider.Encrypt(source.MakeId);
        }
    }
}
