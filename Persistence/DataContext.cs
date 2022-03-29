using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<AnimalType> AnimalTypes { get; set; }
        public DbSet<Bird> Birds { get; set; }
        public DbSet<Terrestrial> Terrestrials { get; set; }
        public DbSet<TerrestrialType> TerrestrialTypes { get; set; }
    }
}