using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using PactNet.Output.Xunit;
using PactNet.Verifier;
using Xunit.Abstractions;

namespace ProductService.Unit.Tests;

public class ConsumersContractTests
{
    private const string BaseUrl = "http://localhost:8888";
    private readonly ITestOutputHelper _output;
    private readonly WebApplicationFactory<Program> _webApplicationFactory;

    public ConsumersContractTests(ITestOutputHelper output)
    {
        _output = output;
        _webApplicationFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(b => b.UseUrls(BaseUrl));
    }

    [Fact]
    public void EnsurePactWithConsumersAreHonoured()
    {
        // Arrange
        var pactConfig = new PactVerifierConfig
        {
            Outputters = new[] { new XunitOutput(_output) }
        };

        //Act / Assert
        var pactVerifier = new PactVerifier(pactConfig);
        pactVerifier
            .ServiceProvider("ProductService", new Uri(BaseUrl))
            .WithDirectorySource(new DirectoryInfo(Path.Join("..", "..", "..", "..", "Pacts")))
            .WithProviderStateUrl(new Uri($"{BaseUrl}/provider-states"))
            .Verify();
    }
}