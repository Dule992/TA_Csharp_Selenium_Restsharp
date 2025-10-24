using static API_Automation.Models.Response.BooksGetResponse;

namespace API_Automation.Models.Response
{
    public class UserPostResponse
    {
        public string userID { get; set; }
        public string username { get; set; }
        public List<Book> books { get; set; }
    }
}
