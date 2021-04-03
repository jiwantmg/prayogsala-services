using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PragyoSala.Services.Data;
using PragyoSala.Services.Dtos;
using PragyoSala.Services.Models;

namespace PragyoSala.Services.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private IConfiguration Configuration { get; }
        public UsersController(
            AppDbContext context,
            IConfiguration configuration
            )
        {
            _context = context;
            Configuration = configuration;
        }

        [HttpPost]
        public ActionResult Register([FromBody]UserDto userDto)
        {
            try{
                var user = new User
                {
                    Address = userDto.Address,
                    Email = userDto.Email,
                    Password = ToSHA512(userDto.Password),
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

            if (user.Password.Equals(ToSHA512(login.Password)))
            {
                return Ok(new AuthenticateResponse(user, generateJwtToken(user)));
            }

            return BadRequest(new
            {
                Message = "Invalid username or password"
            });
        }

        private string ToSHA512(string password)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString();
            }
        }
        
        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration["JwtSettings:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.UserId.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token); 
        }
    }
}