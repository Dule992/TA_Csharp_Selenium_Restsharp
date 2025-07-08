using API_Automation.Client;
using API_Automation.Helpers;
using API_Automation.Models.Request;
using API_Automation.Models.Response;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using System.Net;

namespace API_Automation.StepDefinitions
{
    [Binding]
    public class ReplaceABookStepDefinitions : TestBase
    {
        private readonly RestApiClient _apiClient;
        private RestResponse _response;
        private UserRequest User;
        private string _token;
        private string _basicAuth;
        public ReplaceABookStepDefinitions(ScenarioContext scenarioContext) : base(scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _apiClient = new RestApiClient(Configuration["BASE_URL"]);
            User = FakeUserFactory.GetUserFaker();
        }

        [Given(@"User is created and authorized.")]
        public async Task GivenUserIsCreatedAndAuthorized()
        {
            Logger.Log($"Creating user with username: {User.userName} and password: {User.password}.");
            _response = await _apiClient.CreateUser(User.userName, User.password);
            Logger.Log($"User creation response: {_response.Content}");

            _token = await _apiClient.ReturnGeneratedToken(User.userName, User.password);
            Logger.Log($"Generated token: {_token}");

            _basicAuth = BasicAuth.BasicAuthValue(User.userName, User.password);

            var responseBody = JsonConvert.DeserializeObject<UserPostResponse>(_response.Content);
            Logger.Log($"User ID: {responseBody.userID}, Username: {responseBody.username}");

            _scenarioContext.Set(responseBody.userID, "userId");

            bool isUserAuthorized = await _apiClient.IsUserAuthorized(User.userName, User.password, _basicAuth, _token);
            Assert.That(isUserAuthorized, Is.True, "User is not authorized");
            Logger.Log("User is created and authorized successfully.");
        }

        [When(@"User send request to get all books sending GET request to {string}.")]
        public async Task WhenUserSendRequestToGetAllBooksSendingGETRequestTo(string getBooksEndpoint)
        {
            _response = await _apiClient.GetAsync(getBooksEndpoint);
            Logger.Log($"GET request sent to {getBooksEndpoint}. Response status code: {_response.StatusCode}");
        }

        [Then("User should receive a {int} OK and the list of books in the response")]
        public void ThenUserShouldReceiveAOKAndTheListOfBooksInTheResponse(int statusCode)
        {
            var responseBody = JsonConvert.DeserializeObject<BooksGetResponse>(_response.Content);
            Logger.Log($"Response body: {JsonConvert.SerializeObject(responseBody)}");
            Assert.That(_response.StatusCode, Is.EqualTo((HttpStatusCode)statusCode), "Unexpected status code received");
            Assert.That(_response.Content, Is.Not.Null.Or.Empty, "Response content is null or empty");
            Assert.That(responseBody.books.Count, Is.GreaterThan(0), "Response body is null");
            _scenarioContext.Set(responseBody, "GetBooksResponse");
            _scenarioContext.Set(responseBody.books[0].isbn, "FirstIsbnBook");
            _scenarioContext.Set(responseBody.books[1].isbn, "SecondIsbnBook");
            _scenarioContext.Set(responseBody.books[0].title, "FirstTitleBook");
            _scenarioContext.Set(responseBody.books[1].title, "SecondTitleBook");
        }

        [When("User add the first book from the book list from the previous response to the user's book list sending POST request to {string}.")]
        public async Task WhenUserAddTheFirstBookFromTheBookListFromThePreviousResponseToTheUsersBookListSendingPOSTBookStoreVBooks(string postBooksEndpoint)
        {
            var responseBody = _scenarioContext.Get<BooksGetResponse>("GetBooksResponse");
            if (responseBody.books.Count == 0)
            {
                throw new Exception("No books found in the response body.");
            }
            var firstBook = new PostBookRequest()
            {
                userId = _scenarioContext.Get<string>("userId"),
                collectionOfIsbns = new List<PostBookRequest.Isbns>()
                {
                    new PostBookRequest.Isbns { isbn = _scenarioContext.Get<string>("FirstIsbnBook") }
                }
            };
            string firstBookJsonPayload = JsonConvert.SerializeObject(firstBook, Formatting.Indented);
            Logger.Log($"Adding the first book: {firstBookJsonPayload}");
            _response = await _apiClient.PostAsync(postBooksEndpoint, _basicAuth, _token, firstBookJsonPayload);
            Logger.Log($"POST request sent to {postBooksEndpoint}.");
        }

        [Then("User should receive a {int} Created and the book should be added to the user's book list.")]
        public void ThenUserShouldReceiveACreatedAndTheBookShouldBeAddedToTheUsersBookList(int statusCode)
        {
            var responseBody = JsonConvert.DeserializeObject<BooksPostResponse>(_response.Content);
            Logger.Log($"Response status code: {_response.StatusCode}. Response body: {responseBody}");
            Assert.That(_response.StatusCode, Is.EqualTo((HttpStatusCode)statusCode), "Unexpected status code received");
            Assert.That(responseBody.books.First().isbn, Is.Not.Null.Or.Empty, "ISBN is null or empty");
        }

        [When("User send request to get user by its id sending GET request to {string}.")]
        public async Task WhenUserSendRequestToGetUserByItsId(string getUserEndpoint)
        {
            string userId = _scenarioContext.Get<string>("userId");
            string getUserIdEndpoint = getUserEndpoint.Replace("{userId}", userId);
            Logger.Log($"Sending GET request to {getUserIdEndpoint} with user ID: {userId}");
            _response = await _apiClient.GetAsync(getUserIdEndpoint, _basicAuth, _token);
            var respnoseBody = JsonConvert.DeserializeObject<UserGetResponse>(_response.Content);
            Logger.Log($"GET request sent to {getUserIdEndpoint}. Response status code: {_response.StatusCode}, User ID: {respnoseBody.userId}, Username: {respnoseBody.username}");
        }

        [Then("User should receive a {int} OK and the user's book list contain only {int} book.")]
        public void ThenUserShouldReceiveAOKAndTheUsersBookListContainOnlyBook(int statusCode, int numberOfBooks)
        {
            Assert.That(_response.StatusCode, Is.EqualTo((HttpStatusCode)statusCode), "Unexpected status code received");
            var responseBody = JsonConvert.DeserializeObject<UserPostResponse>(_response.Content);
            Logger.Log($"Response body: {JsonConvert.SerializeObject(responseBody)}");
            Assert.That(responseBody.books, Is.Not.Null, "Books list is null");
            Assert.That(responseBody.books.Count, Is.EqualTo(numberOfBooks), $"Expected {numberOfBooks} book(s) in the user's book list, but found {responseBody.books.Count}.");
        }

        [Then("The book in the user's book list matches the {string} book title added in the previous step.")]
        public void ThenTheBookInTheUsersBookListMatchesTheBookAddedInThePreviousStep(string orderBook)
        {
            string addedBook;
            if (orderBook.Equals("first", StringComparison.OrdinalIgnoreCase))
            {
                addedBook = _scenarioContext.Get<string>("FirstTitleBook");
            }
            else
            {
                addedBook = _scenarioContext.Get<string>("SecondTitleBook");
            }
            var responseBody = JsonConvert.DeserializeObject<UserPostResponse>(_response.Content);
            Logger.Log($"Added book title: {addedBook}");
            Logger.Log($"User's book list first book title: {responseBody.books.First().title}");
            Assert.That(responseBody.books.First().title, Is.EqualTo(addedBook), "The book in the user's book list does not match the book added in the previous step.");
        }

        [When("User replace the book from the user's book list with the second book from the book list from the previous response sending PUT request to {string}.")]
        public async Task WhenUserReplaceTheBookFromTheUsersBookListWithTheSecondBookFromTheBookListFromThePreviousResponse(string putBookEndpoint)
        {
            var responseBody = _scenarioContext.Get<BooksGetResponse>("GetBooksResponse");
            if (responseBody.books.Count < 2)
            {
                throw new Exception("Not enough books in the response body to replace.");
            }
            var secondBook = new PutBookRequest()
            {
                userId = _scenarioContext.Get<string>("userId"),
                isbn = _scenarioContext.Get<string>("SecondIsbnBook")
            };
            Logger.Log($"Replacing the book with: {JsonConvert.SerializeObject(secondBook)}");
            _response = await _apiClient.PutAsync($"{putBookEndpoint}/{_scenarioContext.Get<string>("FirstIsbnBook")}", _basicAuth, _token, secondBook);
            Logger.Log($"PUT request sent to {putBookEndpoint}. Response status code: {_response.StatusCode}");
        }

        [Then("User should receive a {int} OK.")]
        public void ThenUserShouldReceiveAOK(int statusCode)
        {
            var responseBody = JsonConvert.DeserializeObject<BooksPutResponse>(_response.Content);
            Logger.Log($"Response status code: {_response.StatusCode}. Response body: {JsonConvert.SerializeObject(responseBody)}");
            Assert.That(_response.StatusCode, Is.EqualTo((HttpStatusCode)statusCode), "Unexpected status code received");
        }

    }
}
