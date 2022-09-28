using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class UserService : IUserService
    {
        List<User> users = new List<User>()
        {
            new User(){Email = "franco@google.com", Password = "12345" },
            new User(){Email = "franco2@google.com", Password = "12345" }
        };

        public bool isUser(string email, string password)=>
            users.Where(d=>d.Email == email && d.Password== password).Count() > 0;
    }
}
