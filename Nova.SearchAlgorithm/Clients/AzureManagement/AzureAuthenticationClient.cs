using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Nova.SearchAlgorithm.Clients.AzureManagement.AzureApiModels;
using Nova.SearchAlgorithm.Exceptions;
using Nova.SearchAlgorithm.Settings;

namespace Nova.SearchAlgorithm.Clients.AzureManagement
{
    public interface IAzureAuthenticationClient
    {
        Task<string> GetAuthToken(string scope);
    }

    public class AzureAuthenticationClient : IAzureAuthenticationClient
    {
        private const string OAuthBaseUrl = "https://login.microsoftonline.com/ukmarrow.org/oauth2/v2.0/token";

        private readonly HttpClient httpClient;
        private readonly AzureAuthenticationSettings settings;

        public AzureAuthenticationClient(IOptions<AzureAuthenticationSettings> azureFunctionOptions)
        {
            settings = azureFunctionOptions.Value;

            httpClient = new HttpClient {BaseAddress = new Uri(OAuthBaseUrl)};
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
        }

        public async Task<string> GetAuthToken(string scope)
        {
            var values = new Dictionary<string, string>
            {
                {"client_id", settings.ClientId},
                {"client_secret", settings.ClientSecret},
                {"grant_type", "client_credentials"},
                {"scope", scope},
            };
            var content = new FormUrlEncodedContent(values);

            var response = await httpClient.PostAsync("", content);
            if (!response.IsSuccessStatusCode)
            {
                throw new AzureAuthorisationException();
            }

            var responseBody = JsonConvert.DeserializeObject<OAuthResponse>(await response.Content.ReadAsStringAsync());
            return responseBody.access_token;
        }
    }
}