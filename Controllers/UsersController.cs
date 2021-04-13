using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PragyoSala.Services.Data;
using PragyoSala.Services.Dtos;
using PragyoSala.Services.Models;
using prayogsala_services.Middlewares;
using prayogsala_services.Util;

namespace PragyoSala.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private IConfiguration Configuration { get; }
        private IAuthenticatedUserService _authenticatedService;
        public UsersController(
            AppDbContext context,
            IConfiguration configuration,
            IAuthenticatedUserService authenticatedService
            )
        {
            _context = context;
            Configuration = configuration;
            _authenticatedService = authenticatedService;
        }

        [HttpPost]
        public ActionResult Register([FromBody]UserDto userDto)
        {
            try{
                var user = new User
                {
                    Address = userDto.Address,
                    Email = userDto.Email,
                    Password = UserUtil.ToSHA512(userDto.Password),
                    FirstName = userDto.Fname,
                    LastName = userDto.Lname,
                    PhoneNo = userDto.Phone                    
                };
                
                _context.Users.Add(user);
                _context.SaveChanges();
                return Ok(new
                {
                    Message = "New user created successfully"
                });
            }catch(Exception ex)
            {
                return BadRequest("Invalid request");
            }
        }      

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto login)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == login.Email);            
            if (user == null)
                return BadRequest(new
                {
                    Message = "Invalid username or password"
                });

            var newHash =UserUtil.ToSHA512(login.Password);
            Console.WriteLine($"{user.Password} {newHash}");
            if (string.Equals(user.Password, newHash))
            {
                return Ok(new AuthenticateResponse(user, generateJwtToken(user)));
            }

            return BadRequest(new
            {
                Message = "Invalid username or password"
            });
        }

        [HttpGet("role")]
        [Authorize]
        public ActionResult Me()
        {
            return Ok(
               new {
                    role = _authenticatedService.Role
                }
            );
        }
        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JwtSettings:Secret"]));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[] { 
                        new Claim("id", user.UserId.ToString()),
                        new Claim(ClaimTypes.Role, user.Role)
                    }
                ),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
                Issuer = Configuration["JwtSettings:ValidIssuer"],
                Audience = Configuration["JwtSettings:ValidAudience"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token); 
        }
    }
}