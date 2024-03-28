using System.Net;
using devCrowd.BuildYourOwnTeamsClient.Options;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace devCrowd.BuildYourOwnTeamsClient;

public class GetConfiguration(
    ILogger<GetConfiguration> logger,
    IOptions<Authentication> authOptions,
    IOptions<Graph> graphOptions)
{
    [Function(nameof(GetConfiguration))]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, nameof(HttpMethod.Get), Route = "app-config")] 
        HttpRequestData req,
        FunctionContext executionContext)
    {
        HttpResponseData response;
        
        if (authOptions.Value is not null 
            && graphOptions.Value is not null)
        {
            response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new
            {
                Auth = authOptions.Value,
                Graph = graphOptions.Value
            });
        }
        else
        {
            response = req.CreateResponse(HttpStatusCode.InternalServerError);
            await response.WriteStringAsync("Wrong or missing Configuration Data");
        }
        
        return response;
    }
}