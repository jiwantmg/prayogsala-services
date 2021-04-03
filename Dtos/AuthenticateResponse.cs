using PragyoSala.Services.Models;

namespace PragyoSala.Services.Dtos
{
    public class AuthenticateResponse
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(User user, string token)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Token = token;
        }
    }
}