using GotoFreight.IATA.Services;
using Microsoft.AspNetCore.Mvc;

namespace GotoFreight.IATA.Controllers;

public class HomeController : BaseController
{
    private readonly ILogger<HomeController> _logger;
    private readonly OrderService _orderService;

    // ReSharper disable once ConvertToPrimaryConstructor
    public HomeController(ILogger<HomeController> logger, OrderService orderService)
    {
        _logger = logger;
        _orderService = orderService;
    }

    [HttpGet("{msg}")]
    public Task<string> Get(string msg)
    {
        return Task.FromResult($"test: {msg}");
    }
}