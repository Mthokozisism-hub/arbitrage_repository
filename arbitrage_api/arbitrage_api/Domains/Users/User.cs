using Microsoft.AspNetCore.Identity;

namespace arbitrage_api.Domains.Users
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public bool EnableNotifications { get; set; }
    }
}
