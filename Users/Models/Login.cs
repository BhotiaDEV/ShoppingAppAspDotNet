using System.ComponentModel.DataAnnotations;

namespace Users.Models
{
    public class Login
    {
        [Required]
        public String Email { get; set; }

        [Required]
        public String Passsword { get; set; }
    }
}