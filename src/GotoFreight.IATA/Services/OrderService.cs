using AutoMapper;
using GotoFreight.IATA.Models.Dto;
using GotoFreight.IATA.Models.Entity;
using GotoFreight.IATA.Models.Enum;
using GotoFreight.IATA.Repository;
using GotoFreight.IATA.X;

namespace GotoFreight.IATA.Services;

public class OrderService
{
    private readonly IMapper _mapper;
    private readonly OrderRepository _orderRepository;
    private readonly ContactRepository _contactRepository;
    private readonly ContactService _contactService;

    // ReSharper disable once ConvertToPrimaryConstructor
    public OrderService(IMapper mapper,
        OrderRepository orderRepository,
        ContactRepository contactRepository,
        ContactService contactService)
    {
        _mapper = mapper;
        _orderRepository = orderRepository;
        _contactRepository = contactRepository;
        _contactService = contactService;
    }

    private string GenOrderCode()
    {
        var random = new Random();
        return "BSI-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + random.Next(1000, 10000);
    }

    private void FixOrderSubmitDto(OrderSubmitDto dto)
    {
        if (dto.Contact == null)
        {
            dto.Contact = _contactService.GetDefault();
        }

        ReflectX.FixNullValue(dto.Package, typeof(string), "");
        if (string.IsNullOrWhiteSpace(dto.Package.DepartureAddress))
        {
            dto.Package.DepartureAddress = "HongKong";
        }

        if (dto.Package.Departure.Year == 1)
        {
            dto.Package.Departure = DateTime.Now;
        }

        if (string.IsNullOrWhiteSpace(dto.Package.ArrivalAddress))
        {
            dto.Package.ArrivalAddress = "Los Angeles";
        }

        if (dto.Package.Arrival.Year == 1)
        {
            dto.Package.Arrival = DateTime.Now.AddDays(7);
        }

        if (string.IsNullOrWhiteSpace(dto.Package.Remark))
        {
            dto.Package.Remark = "important";
        }

        if (string.IsNullOrWhiteSpace(dto.Package.GoodsDesc))
        {
            dto.Package.GoodsDesc = $"Goods count is: {dto.Goods.Count()}";
        }

        foreach (var good in dto.Goods)
        {
            ReflectX.FixNullValue(good, typeof(string), "");
        }
    }

    public async Task<string> Submit(long userId, OrderSubmitDto request)
    {
        var validateResult = ValidationX.RecurveValidate(request);
        if (validateResult.Any(q => !string.IsNullOrWhiteSpace(q.ErrorMessage)))
        {
            throw new Exception(string.Join("\n", validateResult.Select(q => q.ErrorMessage)));
        }

        FixOrderSubmitDto(request);

        if (!string.IsNullOrWhiteSpace(request.Code))
        {
            await Remove(request.Code);
        }

        var contactId = await _contactRepository.Upsert(_mapper.Map<Contact>(request.Contact));
        var order = new Order
        {
            Code = GenOrderCode(),
            UserId = userId,
            Airline = request.Package.Airline,
            Flight = request.Package.Flight,
            Reference = "",
            Remark = "",
            Status = OrderStaus.Awaiting.ToInt(),
            DepartureAddress = request.Package.DepartureAddress,
            ArrivalAddress = request.Package.ArrivalAddress,
            Departure = request.Package.Departure,
            Arrival = request.Package.Arrival,
            CreateTime = DateTime.Now,
            UpdateTime = DateTime.Now,

            //extensions
            Packages = new List<Package>
            {
                _mapper.Map<Package>(request.Package)
            }
        };
        foreach (var package in order.Packages)
        {
            package.OrderCode = order.Code;
            package.ContactId = contactId;

            package.Goods = _mapper.Map<IEnumerable<Good>>(request.Goods);
        }

        await _orderRepository.Insert(order);

        return order.Code;
    }

    public async Task Remove(string code)
    {
        await _orderRepository.Remove(code);
    }

    public async Task<OrderDetailDto> Find(string code)
    {
        var order = await _orderRepository.Find(code);
        return _mapper.Map<OrderDetailDto>(order);
    }

    public async Task<OrderListResultDto> List(OrderListRequestDto request)
    {
        if (request.PageIndex < 1)
        {
            request.PageIndex = 1;
        }

        if (request.PageSize > 100)
        {
            request.PageSize = 100;
        }

        var (total, orders) = await _orderRepository.List(request);
        return new OrderListResultDto
        {
            Total = total,
            Items = _mapper.Map<IEnumerable<OrderInfo>>(orders)
        };
    }
}