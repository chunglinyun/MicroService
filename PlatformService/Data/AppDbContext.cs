using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<DbContext> opt) : base(opt)
        {
            
        }

        public DbSet<Platfrom> Platfroms { get; set; }
    }
}