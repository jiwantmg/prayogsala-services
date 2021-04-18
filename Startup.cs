using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PragyoSala.Services.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using prayogsala_services.Middlewares;

namespace prayogsala_services
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddNewtonsoftJson(options => {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;                
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,                    
                    ValidateIssuerSigningKey = true,
                    ValidAudience = Configuration["JwtSettings:ValidAudience"],
                    ValidIssuer = Configuration["JwtSettings:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JwtSettings:Secret"]))                     
                };
            });
            services.AddDbContext<AppDbContext>(options => {
                options.UseMySql(
                    Configuration["ConnectionString"],
                    new MySqlServerVersion(new Version(8, 0, 23))
                );
            });
            services.AddCors(options =>
            {
                options.AddPolicy("CorsAllow", builder =>
                {
                    builder.SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });
            services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "prayogsala_services", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "prayogsala_services v1"));
            }

            //app.UseHttpsRedirection();
            app.UseCors("CorsAllow");
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();            
            app.UseAuthorization();
            app.UseUserAuthMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}