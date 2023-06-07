using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Users.Config;
using Users.Data;
using Users.Models;

namespace Users.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly IConfiguration _configuration;
        public UserController(UserManager<IdentityUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(Register request)
        {
            if (ModelState.IsValid)
            {
                var useremail_exist = await _userManager.FindByEmailAsync(request.Email);

                if (useremail_exist != null)
                {
                    return BadRequest("Email Already taken!");
                }
                else
                {
                    var username_exist = await _userManager.FindByNameAsync(request.UserName);
                    if (useremail_exist != null)
                    {
                        return BadRequest("Username is Already taken!");
                    }
                    
                    var user = new IdentityUser()
                    {
                        UserName = request.UserName, // check for existing user name
                        Email = request.Email,
                        PasswordHash = request.Password,
                    };

                    var is_created = _userManager.CreateAsync(user);

                    if (is_created.IsCompletedSuccessfully)
                    {
                        var jwttoken = GenerateJwtToken(user);

                        return Ok(jwttoken);
                    }
                    // owasp vunerability
                    else
                    {
                        return BadRequest("Server Error");
                    }
                }
            }

            else
            return BadRequest("Bad Request");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login (Login request)
        {
            if (ModelState.IsValid)
            {
                var existing_user = await _userManager.FindByEmailAsync(request.Email);

                if(existing_user != null)
                {
                    return BadRequest("No User Found!");
                }

                var isCorrect = await _userManager.CheckPasswordAsync(existing_user, request.Passsword);

                if (!isCorrect)
                {
                    return BadRequest("Wrong Password");
                }

              //  if(request.Email == "abc@gmail.com")
                //{
               //    var token = GenerateJwtToken(existing_user);
                 //   return Ok(token, "admin");
               // }

                var token = GenerateJwtToken(existing_user);
                return Ok(token);
            }
            else
                return BadRequest("Bad request");
        }


        [NonAction]
        public String GenerateJwtToken(IdentityUser user)
        {

            var jwttokenhandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_configuration["JwtConfig:Secret"]);

            var tokendescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]
                {
                                new Claim("Id",value:user.Id),
                                new Claim(JwtRegisteredClaimNames.Sub, value:user.Email),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString())
                            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = jwttokenhandler.CreateToken(tokendescriptor);
            return (jwttokenhandler.WriteToken(token));
        }
    }
}
