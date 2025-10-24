using API_Automation.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace API_Automation
{
    [Binding]
    public class TestBase
    {
        protected IConfiguration Configuration;
        protected ScenarioContext _scenarioContext;

        public TestBase(ScenarioContext scenarioContext)
        {
            Configuration = new ConfigurationBuilder()
            .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
            .Build();

            _scenarioContext = scenarioContext;
        }

        [AfterScenario]
        public async Task DeleteUser()
        {
            var apiClient = new RestApiClient(Configuration["BASE_URL"]);
            var userId = _scenarioContext.Get<string>("userId");
            var response = await apiClient.DeleteAsync(Configuration["DELETE_ENDPOINT"].Replace("{userId}", userId));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Logger.Log($"User with ID {userId} deleted successfully.");
            }
            else
            {
                Logger.Log($"Failed to delete user with ID {userId}. Response: {response.Content}");
            }
        }
    }
}
