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
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseRate> CourseRates { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}