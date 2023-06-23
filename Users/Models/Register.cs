using System.ComponentModel.DataAnnotations;

namespace Users.Models
{
    public class Register
    {
        [Required]
        public String UserName { get; set; } // change to username

        [Required]
        public String Email { get; set; }

        [Required]
        public String Password { get; set; }

        [Required]
        public String PhoneNumber { get; set; }

        [Required]
        public String Location { get; set; }
   
    }
}
