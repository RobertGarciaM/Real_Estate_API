using AutoMapper;
using DataModels;
using DTOModels;
using RealEstate.Mediator.Utilities;

namespace RealEstate.Mediator.MapperProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            _ = CreateMap<Owner, OwnerDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdOwner));
            _ = CreateMap<CreateOwnerDto, Owner>()
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => Util.ConvertFormFileToByteArray(src.Photo)));
            _ = CreateMap<UpdateOwnerDto, Owner>()
               .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => Util.ConvertFormFileToByteArray(src.Photo)));
            _ = CreateMap<Property, PropertyDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdProperty));
            _ = CreateMap<UpdatePropertyDto, Property>()
                .ForMember(dest => dest.IdProperty, opt => opt.MapFrom(src => src.Id));
            _ = CreateMap<CreatePropertyDto, Property>();

            _ = CreateMap<CreatePropertyImageDto, PropertyImage>()
              .ForMember(dest => dest.File, opt => opt.MapFrom(src => Util.ConvertFormFileToByteArray(src.File)));
            _ = CreateMap<PropertyImage, PropertyImageDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdPropertyImage));
            _ = CreateMap<UpdatePropertyImagesDto, PropertyImage>()
             .ForMember(dest => dest.File, opt => opt.MapFrom(src => Util.ConvertFormFileToByteArray(src.File)));
            _ = CreateMap<CreatePropertyTraceDto, PropertyTrace>();
            _ = CreateMap<UpdatePropertyTraceDto, PropertyTrace>();
            _ = CreateMap<PropertyTrace, PropertyTraceDto>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdPropertyTrace));

        }
    }
}