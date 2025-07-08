using static API_Automation.Models.Response.BooksGetResponse;

namespace API_Automation.Models.Response
{
    public class UserGetResponse
    {
        public string userId { get; set; }
        public string username { get; set; }
        public List<Book> books { get; set; }
    }
}
