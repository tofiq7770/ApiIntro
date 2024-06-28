
using ApiAspIntro.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiAspIntro.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Category> Categories { get; set; }



    }
}
