using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Text;

namespace API_Automation.Client
{
    public class RestApiClient : IApiClient
    {
        private TimeSpan _timeout = TimeSpan.FromSeconds(1000);
        public readonly RestClient _client;

        public RestApiClient(string baseUrl)
        {
            var options = new RestClientOptions(baseUrl)
            {
                BaseUrl = new Uri(baseUrl), // Set the base URL for the API
                ThrowOnAnyError = false, // Throw exceptions for any HTTP errors
                Timeout = _timeout // Set a timeout of 10 seconds
            };
            _client = new RestClient(options);
        }
        public async Task<RestResponse> GetAsync(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.Get);
            return await _client.ExecuteAsync(request);
        }

        public async Task<RestResponse> GetAsync(string endpoint, string basicAuth, string token)
        {
            var request = new RestRequest(endpoint, Method.Get);

            request.AddHeader("Authorization", $"Basic {basicAuth}");
            request.AddHeader("Authorization", $"{token}"); // Add the authorization token to the request header
            return await _client.ExecuteAsync(request);
        }

        public async Task<RestResponse> PostAsync<T>(string endpoint, T body)
        {
            var request = new RestRequest(endpoint, Method.Post);
            request.AddBody(body);
            return await _client.ExecuteAsync(request);
        }

        public async Task<RestResponse> PostAsync<T>(string endpoint, string basicAuth, string token, T body)
        {
            var request = new RestRequest(endpoint, Method.Post);

            request.AddHeader("Authorization", $"Basic {basicAuth}");
            request.AddHeader("Authorization", $"{token}"); // Add the authorization token to the request header
            request.AddBody(body);
            return await _client.ExecuteAsync(request);
        }

        public async Task<RestResponse> PutAsync<T>(string endpoint, string basicAuth, string token, T body)
        {
            var request = new RestRequest(endpoint, Method.Put);

            request.AddHeader("Authorization", $"Basic {basicAuth}");
            request.AddHeader("Authorization", $"{token}"); // Add the authorization token to the request header
            request.AddBody(body);
            return await _client.ExecuteAsync(request);
        }

        public async Task<RestResponse> DeleteAsync(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.Delete);
            return await _client.ExecuteAsync(request);
        }

        public async Task<RestResponse> CreateUser(string username, string password)
        {
            // This method should implement the logic to create a user and authorize them.
            // For example, it could send a POST request to a user creation endpoint and store the authorization token.
            // This is a placeholder implementation.
            var user = new { userName = username, password = password };
            var response = await PostAsync("/Account/v1/User", user);
            if (!response.IsSuccessful)
            {
                throw new Exception("Failed to create user: " + response.Content);
            }
            return response;
        }
        // Assuming the response contains an authorization token
        // Store the token for future requests (not implemented here);
        public async Task<bool> IsUserAuthorized(string username, string password, string basicAuth, string token)
        {
            // This method should implement the logic to check if the user is authorized.
            // For example, it could check if a valid authorization token is stored.
            // This is a placeholder implementation.
            var user = new { userName = username, password = password };
            var response = await PostAsync("/Account/v1/Authorized", basicAuth, token, user);
            return response.IsSuccessful && response.Content.Contains("true");
        }

        public async Task<string> ReturnGeneratedToken(string username, string password)
        {
            // This method should implement the logic to generate a token for the user.
            // For example, it could send a POST request to a token generation endpoint.
            // This is a placeholder implementation.
            var user = new { userName = username, password = password };
            var response = await PostAsync("/Account/v1/GenerateToken", user);
            if (!response.IsSuccessful)
            {
                throw new Exception("Failed to generate token: " + response.Content);
            }

            // Assuming the response contains a token in JSON format
            var tokenResponse = JsonConvert.DeserializeObject<dynamic>(response.Content);
            if (tokenResponse != null && tokenResponse.token != null)
            {
                return tokenResponse.token.ToString();
            }
            throw new Exception("Token not found in response: " + response.Content);
        }
    }
}