using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Users.Models;

namespace Users.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public UserController(UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(Register request, string role = "User")
        {
            if (ModelState.IsValid)
            {
                var useremail_exist = await _userManager.FindByEmailAsync(request.Email);
                //user exist no registration possible
                if (useremail_exist != null)
                return BadRequest("Email Already taken!");

                //creating new user 
                var user = new IdentityNewUser()
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Location = request.Location,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                var role_exists = await _roleManager.RoleExistsAsync(role);
                if (role_exists)
                {
                    var is_created = await _userManager.CreateAsync(user,request.Password);
                    if(is_created.Succeeded)
                    {
                    //Assign role
                    await _userManager.AddToRoleAsync(user, role);
                    return Ok("User Created Successfully");
                    }
                    else
                    {
                        var errors = string.Join(", ", is_created.Errors.Select(e => e.Description));
                        return BadRequest($"Error while creating User: {errors}");
                    }
                }
                else
                {
                    return BadRequest("Bad Request! Role does not exists.");
                }
            }
            else
            return BadRequest("Bad Request");
        }

        [HttpGet]
        [Route("getusers")]
        public async Task<IActionResult> Getuser()
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }

        [HttpGet]
        [Authorize]
        [Route("getuser/{id:guid}")]
        public IActionResult getuser(Guid id)
        {
            var tokenId = User.FindFirstValue("Id");   // get token id
            if(tokenId != null)                         // check if there is token id
            {
                if(id.Equals(tokenId))                   // compare both the id
                {
                    var user_exists = _userManager.FindByIdAsync(tokenId);  // find user if both id are similar

                    if (user_exists != null)                //if user exists
                    {
                        return Ok(user_exists);
                    }
                    else
                    {
                        return BadRequest("Bad Request User not found!");
                    }
                }
                else { return BadRequest("Bad Request! id and token id don't match.");  }
            }
            else { return BadRequest("Invalide token!"); }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login (Login request)
        {
            if (ModelState.IsValid)
            {
                var existing_user = await _userManager.FindByEmailAsync(request.Email);
                //Check User
                if (existing_user == null)
                {
                    return BadRequest("No User Found!");
                }   
                //Check Password
                var isCorrect = await _userManager.CheckPasswordAsync(existing_user, request.Passsword);
                if (!isCorrect)
                {
                    return BadRequest("Wrong Password");
                }
                //Generate token with role
                var roles = await _userManager.GetRolesAsync(existing_user);
                var role = "";
                foreach(var r in roles)
                {
                    role = r.ToString();
                }
                var token = GenerateJwtToken(existing_user,role);
                return Ok(new  {
                 id = existing_user.Id,
                 username = existing_user.UserName,
                 email = existing_user.Email,
                 token = token
                });
            }
            else
                return BadRequest("Bad request");
        }


        [NonAction]
        private String GenerateJwtToken(IdentityUser user, string role)
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
                                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                                new Claim(ClaimTypes.Role, role)
                            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = jwttokenhandler.CreateToken(tokendescriptor);
            return (jwttokenhandler.WriteToken(token));
        }
    }
}
