using GotoFreight.IATA.Models.Dto;
using GotoFreight.IATA.Services;
using Microsoft.AspNetCore.Mvc;
using SJZY.Expand.ABP.Core.Common.Dto;

namespace GotoFreight.IATA.Controllers;

public class OrderController : BaseController
{
    private readonly OrderService _orderService;

    // ReSharper disable once ConvertToPrimaryConstructor
    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<UnifyResultDto> Submit([FromBody] OrderSubmitDto request)
    {
        var code = await _orderService.Submit(GetUserId(), request);
        return new UnifyResultDto(true, code);
    }

    [HttpPost("{code}")]
    public async Task<UnifyResultDto> Remove(string code)
    {
        await _orderService.Remove(code);
        return new UnifyResultDto(true);
    }

    [HttpGet("{code}")]
    public async Task<OrderDetailDto> Find(string code)
    {
        return await _orderService.Find(code);
    }

    [HttpGet]
    public async Task<OrderListResultDto> List([FromQuery] OrderListRequestDto request)
    {
        if (request.UserId <= 0)
        {
            request.UserId = GetUserId();
        }

        return await _orderService.List(request);
    }
}