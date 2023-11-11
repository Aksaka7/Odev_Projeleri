using Microsoft.EntityFrameworkCore;
using Odev_Projesi.Context.Domain;

namespace Odev_Projesi.Context
{
    public class OdevKurulumDbContext : DbContext
    {
        public OdevKurulumDbContext(DbContextOptions<OdevKurulumDbContext> options) : base(options) 
        {
        
        }

        public DbSet<UserInfo> UserOdevs { get; set; }
    }
}
