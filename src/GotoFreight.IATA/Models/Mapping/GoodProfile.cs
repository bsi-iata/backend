using AutoMapper;
using GotoFreight.IATA.Models.Dto;
using GotoFreight.IATA.Models.Entity;

namespace GotoFreight.IATA.Models.Mapping;

public class GoodProfile : Profile
{
    public GoodProfile()
    {
        CreateMap<GoodDto, Good>()
            .ForMember(dest => dest.CreateTime, opt => opt.MapFrom(_ => DateTime.Now))
            .ForMember(dest => dest.UpdateTime, opt => opt.MapFrom(_ => DateTime.Now));
    }
}