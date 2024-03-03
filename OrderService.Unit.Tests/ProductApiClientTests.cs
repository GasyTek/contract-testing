using System.Net;
using OrderService.Contracts.Consumed;
using OrderService.Infrastructure;
using PactNet;
using PactNet.Output.Xunit;
using Shouldly;
using Xunit.Abstractions;

namespace OrderService.Unit.Tests;

public class ProductApiClientTests
{
    private readonly HttpClient _httpClient;
    private readonly IPactBuilderV3 _pactBuilder;

    public ProductApiClientTests(ITestOutputHelper output)
    {
        var pactConfig = new PactConfig
        {
            PactDir = Path.Join("..", "..", "..", "..", "Pacts"),
            Outputters = new[] { new XunitOutput(output) }
        };

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:7777")
        };

        _pactBuilder = Pact.V3("OrderService", "ProductService", pactConfig)
            .WithHttpInteractions(7777);
    }

    [Fact]
    public async Task GetProductById_Success()
    {
        // Arrange
        var sut = new ProductApiClient(_httpClient);
        var expectedProduct = new ProductData
        {
            Id = 1,
            Type = "Book",
            Reference = "How to implement contract testing"
        };
        _pactBuilder
            .UponReceiving("A request for a product")
            .Given("A product with the given id exists")
            .WithRequest(HttpMethod.Get, "/api/products/1")
            .WillRespond()
            .WithStatus(HttpStatusCode.OK)
            .WithJsonBody(expectedProduct);

        // Act
        await _pactBuilder.VerifyAsync(async ctx =>
        {
            var response = await sut.GetProductAsync(1);

            // Assert
            response.ShouldNotBeNull();
            response.ShouldBeEquivalentTo(expectedProduct);
        });
    }
}