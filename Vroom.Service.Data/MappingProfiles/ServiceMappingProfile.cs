using AutoMapper;
using Vroom.Data.Models;
using Vroom.Models;
using Vroom.Service.Models;

namespace Vroom.Service.Data.MappingProfiles
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            CreateMap<Make, MakeServiceModel>();
            CreateMap<MakeServiceModel, Make>();
            CreateMap<Model, ModelServiceModel>();
            CreateMap<ModelServiceModel, Model>();
            CreateMap<Bike, BikeServiceModel>().ForMember(dest => dest.MakeId, opt => opt.MapFrom(src => src.Model.MakeId));
            CreateMap<BikeServiceModel, Bike>();
            CreateMap<ApplicationUser, ApplicationUserServiceModel>();
            CreateMap<ApplicationUserServiceModel, ApplicationUser>();
        }
    }
}
