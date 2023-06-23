using Microsoft.AspNetCore.Identity;

namespace Users.Models
{
    public class IdentityNewUser: IdentityUser
    {
        public string Location { get; set; }
    }
}
