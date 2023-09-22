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
            CreateMap<Property, PropertyDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdProperty));
            CreateMap<UpdatePropertyDto, Property>()
                .ForMember(dest => dest.IdProperty, opt => opt.MapFrom(src => src.Id));
            CreateMap<CreatePropertyDto, Property>();

            CreateMap<CreatePropertyImageDto, PropertyImage>()
              .ForMember(dest => dest.File, opt => opt.MapFrom(src => Util.ConvertFormFileToByteArray(src.File)));
            CreateMap<PropertyImage, PropertyImageDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdPropertyImage));
            CreateMap<UpdatePropertyImagesDto, PropertyImage>()
             .ForMember(dest => dest.File, opt => opt.MapFrom(src => Util.ConvertFormFileToByteArray(src.File)));
            CreateMap<CreatePropertyTraceDto, PropertyTrace>();
            CreateMap<UpdatePropertyTraceDto, PropertyTrace>();
            CreateMap<PropertyTrace, PropertyTraceDto>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdPropertyTrace));

        }
    }
}