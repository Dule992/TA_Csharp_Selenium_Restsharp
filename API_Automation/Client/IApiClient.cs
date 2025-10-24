using RestSharp;
namespace API_Automation.Client
{
    public interface IApiClient
    {
        Task<RestResponse> GetAsync(string endpoint);
        Task<RestResponse> PostAsync<T>(string endpoint, string basicAuth, string token, T body);
        Task<RestResponse> PutAsync<T>(string endpoint, string basicAuth, string token, T body);
        Task<RestResponse> DeleteAsync(string endpoint);
    }
}
