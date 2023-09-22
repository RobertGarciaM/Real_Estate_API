using AutoMapper;
using DataModels;
using DTOModels;
using RealEstate.Mediator.Utilities;

namespace RealEstate.Mediator.AutoMapperProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Owner, OwnerDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdOwner));
            CreateMap<CreateOwnerDto, Owner>()
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => Util.ConvertFormFileToByteArray(src.Photo)));
            CreateMap<UpdateOwnerDto, Owner>()
               .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => Util.ConvertFormFileToByteArray(src.Photo)));
        }
    }
}
