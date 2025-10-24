using Allure.NUnit.Attributes;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using UI_Automation.Pages;
using UI_Automation.Support;

namespace UI_Automation.StepDefinitions
{
    [Binding]
    public class SearchAndNavigateStepDefinitions
    {
        private readonly IWebDriver _driver;
        private readonly IWait<IWebDriver> _wait;
        private Config _config;
        private readonly StorePage _storePage;
        private readonly AboutPage _aboutPage;

        public SearchAndNavigateStepDefinitions(IWebDriver driver, StorePage storePage, AboutPage aboutPage, ScenarioContext scenarioContext)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            _config = Config.Load();
            _storePage = storePage;
            _aboutPage = aboutPage;
        }

        [Given("I open Store page")]
        [AllureStep]
        public void GivenIOpenStorePage()
        {
            _driver.Navigate().GoToUrl(_config.BaseUrl); // Navigate to the Steam Store page
            Logger.Log("Navigated to Steam Store page: " + _config.BaseUrl);
        }

        [When("I search for {string} game")]
        [AllureStep]
        public void WhenISearchForGame(string gameName)
        {
            _storePage.SearchForGame(gameName); // Perform the search action
            Logger.Log($"Searched for game: {gameName}");
        }

        [Then("I should see the first search result {string}")]
        [AllureStep]
        public void ThenIShouldSeeTheFirstSearchResult(string searchedFirstGame)
        {
            Assert.That(_storePage.GetFirstSearchResultText(), Does.Contain(searchedFirstGame), "Expected the first search result to not be null or empty");
            Logger.Log($"Verified the first search result contains: {searchedFirstGame}");
        }

        [Then("I should see the second search result {string}")]
        [AllureStep]
        public void ThenIShouldSeeTheSecondSearchResult(string searchedSecondGame)
        {
            Assert.That(_storePage.GetSecondSearchResultText(), Does.Contain(searchedSecondGame), "Expected the second search result to not be null or empty");
            Logger.Log($"Verified the second search result contains: {searchedSecondGame}");
        }

        [When("I click on the first search result in the search results")]
        [AllureStep]
        public void WhenIClickOnTheFirstSearchResultInTheSearchResults()
        {
            _storePage.ClickFirstSearchResultWithJs(); // Click on the first search result
            Logger.Log("Clicked on the first search result in the search results");
        }

        [Then("I should be redirected to the {string} page")]
        [AllureStep]
        public void ThenIShouldBeRedirectedToThePage(string pageUrl)
        {
            
            _wait.Until(driver => driver.Url.Contains(pageUrl));
            Assert.That(_storePage.GetPageUrl(), Does.Contain(pageUrl), $"Expected to be redirected to the page containing '{pageUrl}'");
            Logger.Log($"Verified redirection to the page: {pageUrl}");
        }

        [Then("I should see the game name {string} from the 1st search result")]
        [AllureStep]
        public void ThenIShouldSeeTheGameNameFromTheStSearchResult(string gameName)
        {
            Assert.That(_storePage.GetGameNameHeadingText(), Does.Contain(gameName), "Expected the game name heading text to not be null or empty");
            Logger.Log($"Verified the game name in the heading: {gameName}");
        }

        [When("I click on Download button")]
        [AllureStep]
        public void WhenIClickOnDownloadButton()
        {
            _storePage.ClickDownloadButton(); // Click on the Download button
            Logger.Log("Clicked on the Download button");
        }

        [When("I click on No, I need Steam button")]
        [AllureStep]
        public void WhenIClickOnNoINeedSteamButton()
        {
            _storePage.ClickNoINeedSteamButton(); // Click on the "No, I need Steam" button
            Logger.Log("Clicked on the 'No, I need Steam' button");
        }

        [Then("I should see the Install Steam button is clickable")]
        [AllureStep]
        public void ThenIShouldSeeTheInstallSteamButtonIsClickable()
        {
            Assert.That(_aboutPage.IsInstallSteamButtonClickable(), Is.True, "Expected the Install Steam button to be clickable");
            Logger.Log("Verified that the Install Steam button is clickable");
        }

        [Then("I should see that Playing Now gamers status are less than Online gamers status")]
        [AllureStep]
        public void ThenIShouldSeeThatPlayingNowGamersStatusAreLessThanOnlineGamersStatus()
        {
            Assert.That(_aboutPage.CompareIfPlayingNowStatusIsLessThanOnlineStatus(), Is.True, "Expected to find at least one Online status element");
            Logger.Log("Verified that Playing Now gamers status is less than Online gamers status");
        }
    }
}
