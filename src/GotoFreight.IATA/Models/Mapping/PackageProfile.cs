using AutoMapper;
using GotoFreight.IATA.Models.Dto;
using GotoFreight.IATA.Models.Entity;

namespace GotoFreight.IATA.Models.Mapping;

public class PackageProfile : Profile
{
    public PackageProfile()
    {
        CreateMap<PackageDto, Package>()
            .ForMember(dest => dest.CreateTime, opt => opt.MapFrom(_ => DateTime.Now))
            .ForMember(dest => dest.UpdateTime, opt => opt.MapFrom(_ => DateTime.Now));
    }
}