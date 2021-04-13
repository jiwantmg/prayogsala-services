using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace prayogsala_services.Middlewares
{
    public class UserContextHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        public UserContextHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IAuthenticatedUserService userService)
        {
            var identity = context?.User?.Identity as ClaimsIdentity;            
            if (identity != null)
            {
                Claim userId = identity.Claims.FirstOrDefault(x => x.Type == "id");
                if(userId != null)
                   userService.UserId = Int32.Parse(userId.Value);


                Claim role = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
                if(role != null)
                   userService.Role = role.Value;
            }
            await _next(context);
        }
    }

    public static class UserAuthExtension
    {
        public static void UseUserAuthMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<UserContextHandlerMiddleware>();
        }
    }
}