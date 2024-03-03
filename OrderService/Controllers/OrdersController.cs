using Microsoft.AspNetCore.Mvc;
using OrderService.Contracts;

namespace OrderService.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(ILogger<OrdersController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{orderId}")]
    public OrderData GetOrder(int orderId)
    {
        return new OrderData();
    }
}