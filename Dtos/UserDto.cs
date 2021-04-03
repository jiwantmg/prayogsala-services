using System.Security.Cryptography;

namespace PragyoSala.Services.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }       
    }
}