using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PactNet;

namespace ProductService.Unit.Tests.Middleware;

public class ProviderStateMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IDictionary<string, Action> _providerStates;

    public ProviderStateMiddleware(RequestDelegate next)
    {
        _next = next;
        _providerStates = new Dictionary<string, Action>
        {
            { "A product with the given id exists", ProductExist },
        };
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/provider-states"))
        {
            await this.HandleProviderStatesRequest(context);
            await context.Response.WriteAsync(string.Empty);
        }
        else
        {
            await this._next(context);
        }
    }

    private void ProductExist()
    {
        // Set the prerequisite state of the ProductService here
        // For instance, insert a product with the given id in the database  
    }

    private async Task HandleProviderStatesRequest(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.OK;

        string jsonRequestBody;
        if (string.Equals(context.Request.Method, HttpMethod.Post.ToString(), StringComparison.OrdinalIgnoreCase))
        {
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
            {
                jsonRequestBody = await reader.ReadToEndAsync();
            }

            var providerState = JsonConvert.DeserializeObject<ProviderState>(jsonRequestBody);

            //A null or empty provider state key must be handled
            if (providerState != null && !string.IsNullOrWhiteSpace(providerState.State))
            {
                _providerStates[providerState.State].Invoke();
            }
        }
    }
}