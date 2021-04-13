using System;

namespace prayogsala_services.Middlewares
{
    public interface IAuthenticatedUserService
    {
        int UserId { get; set; }
        string Role { get; set; }
    }

    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public int UserId { get; set; }
        public string Role { get; set; }
    }
}