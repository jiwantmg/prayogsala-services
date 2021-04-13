using System.ComponentModel.DataAnnotations.Schema;

namespace PragyoSala.Services.Models
{
    [Table("Users")]
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }        
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
    }
}