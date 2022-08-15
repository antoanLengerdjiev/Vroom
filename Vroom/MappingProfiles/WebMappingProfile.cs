using AutoMapper;
using Vroom.Areas.Administration.Models;
using Vroom.Data.Models;
using Vroom.MappingProfiles.Resolvers;
using Vroom.Models;
using Vroom.Service.Models;

namespace Vroom.MappingProfiles
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            CreateMap<MakeServiceModel, MakeViewModel>().ForMember(dest => dest.Id, opt => opt.MapFrom<IdEncodeResolver>());
            CreateMap<MakeViewModel, MakeServiceModel>().ForMember(dest => dest.Id, opt => { opt.PreCondition(src => src.Id != null); opt.MapFrom<IdDecodeResolver>(); });

            CreateMap<ModelServiceModel, ModelViewModel>().ForMember(dest => dest.Id, opt => opt.MapFrom<IdEncodeResolver>())
                .ForMember(dest => dest.MakeId, opt => opt.MapFrom<MakeIdEncodeResolver>());

            CreateMap<CreateViewModel, ModelServiceModel>().ForMember(dest => dest.Id, opt => { opt.PreCondition(src => src.Id != null); opt.MapFrom<IdDecodeResolver>(); })
                .ForMember(dest => dest.MakeId, opt => { opt.MapFrom<MakeIdDecodeResolver>(); });

            CreateMap<Bike, BikeViewModel>().ForMember(dest => dest.MakeId, opt => opt.MapFrom(s =>s.Model.MakeId));
            CreateMap<BikeViewModel, Bike>();

            CreateMap<BikeServiceModel, BikeViewModel>().ForMember(dest => dest.Id, opt => opt.MapFrom<IdEncodeResolver>())
                .ForMember(dest => dest.ModelId, opt => opt.MapFrom<ModelIdEncodeResolver>())
                .ForMember(dest => dest.MakeId, opt => opt.MapFrom<MakeIdEncodeResolver>());
                //.ForMember(dest => dest.MakeId, opt => opt.MapFrom(src => src.Model.MakeId));
            CreateMap<BikeViewModel, BikeServiceModel>().ForMember(dest => dest.Id, opt => { opt.PreCondition(src => src.Id != null); opt.MapFrom<IdDecodeResolver>(); })
                .ForMember(dest => dest.MakeId, opt => {  opt.PreCondition(src => src.MakeId != null); opt.MapFrom<MakeIdDecodeResolver>(); })
                .ForMember(dest => dest.ModelId, opt => { opt.PreCondition(src => src.ModelId != null); opt.MapFrom<ModelIdDecodeResolver>(); });



            CreateMap<ApplicationUser, ApplicationUserViewModel>();
            CreateMap<ApplicationUser, AppUserViewModel>();

            CreateMap<ApplicationUserServiceModel, ApplicationUserViewModel>();
            CreateMap<ApplicationUserServiceModel, AppUserViewModel>();

        }
    }
}
