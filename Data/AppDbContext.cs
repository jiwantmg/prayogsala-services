using Microsoft.EntityFrameworkCore;
using PragyoSala.Services.Models;

namespace PragyoSala.Services.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> context) : base(context)
        {

        }
        public DbSet<User> Users { get; set; }
    }
}