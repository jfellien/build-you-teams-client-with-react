using System.Collections.Generic;
using System.Net;
using Azure;
using devCrowd.BuildYourOwnTeamsClient.Options;
using Azure.Communication.Identity;
using Azure.Core;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace devCrowd.BuildYourOwnTeamsClient;

public class GetAcsToken(
    ILogger<GetAcsToken> logger,
    IOptions<Authentication> authOptions,
    IOptions<AzureCommunicationServices> acsOptions)
{
    
    [Function(nameof(GetAcsToken))]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, 
            nameof(HttpMethod.Get), 
            Route = "acs-token/{userObjectId}/{teamsUserAadToken}")] 
        HttpRequestData req,
        string userObjectId,
        string teamsUserAadToken,
        FunctionContext executionContext)
    {
        HttpResponseData response;

        if (authOptions.Value is not null
            && acsOptions.Value is not null)
        {
            try
            {
                string acsAccessToken = await GetAcsAccessToken(
                    userObjectId, 
                    teamsUserAadToken, 
                    authOptions.Value.ClientId, 
                    acsOptions.Value.ConnectionString);
            
                response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteStringAsync(acsAccessToken);
            }
            catch (Exception ex)
            {
                response = req.CreateResponse(HttpStatusCode.BadRequest);
                await response.WriteStringAsync(ex.Message);
            }
            
        }
        else {
            response = req.CreateResponse(HttpStatusCode.InternalServerError);
            await response.WriteStringAsync("Wrong or missing Configuration Data");
        }
        
        return response;
    }

    private static async Task<string> GetAcsAccessToken(
        string userObjectId, 
        string teamsUserAadToken, 
        string appClientId, 
        string acsConnectionString)
    {
        CommunicationIdentityClient client = new (acsConnectionString);
        GetTokenForTeamsUserOptions options = new (teamsUserAadToken, appClientId, userObjectId);
        Response<AccessToken>? accessToken = await client.GetTokenForTeamsUserAsync(options);

        return accessToken.Value.Token;
    }
}