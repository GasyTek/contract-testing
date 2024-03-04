using Microsoft.AspNetCore.Mvc;
using ProductService.Contracts;

namespace ProductService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ILogger<ProductsController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{id}")]
    public ProductData GetProduct(int id)
    {
        if (id == 1)
        {
            return new ProductData
            {
                Id = 1,
                Type = "Book",
                Reference = "How to implement contract testing"
            };
        }

        return new ProductData
        {
            Id = 10,
            Type = "Phone",
            Reference = "Oneplus 8"
        };
    }
}