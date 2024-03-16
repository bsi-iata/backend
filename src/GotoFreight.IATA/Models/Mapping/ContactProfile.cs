using AutoMapper;
using GotoFreight.IATA.Models.Dto;
using GotoFreight.IATA.Models.Entity;

namespace GotoFreight.IATA.Models.Mapping;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        CreateMap<ContactDto, Contact>()
            .ForMember(dest => dest.CreateTime, opt => opt.MapFrom(_ => DateTime.Now))
            .ForMember(dest => dest.UpdateTime, opt => opt.MapFrom(_ => DateTime.Now))
            .ReverseMap();
    }
}