using AutoMapper;
using GotoFreight.IATA.Models.Dto;
using GotoFreight.IATA.Models.Entity;

namespace GotoFreight.IATA.Models.Mapping;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderInfo>()
            .ForMember(dest => dest.NO, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.Destination,
                opt => opt.MapFrom(
                    src =>
                        $"{src.Packages.FirstOrDefault().Contact.Contry}\n{src.Packages.FirstOrDefault().Contact.State}\n{src.Packages.FirstOrDefault().Contact.City}"))
            .ForMember(dest => dest.Package, opt => opt.MapFrom(
                src =>
                    $"{src.Packages.FirstOrDefault().Weight}\n{src.Packages.FirstOrDefault().Volumn}\n{src.Packages.FirstOrDefault().Quantity}"))
            .ForMember(dest => dest.Goods, opt => opt.MapFrom(
                src => src.Packages.FirstOrDefault().GoodsDesc));

        CreateMap<Order, OrderDetailDto>();
    }
}