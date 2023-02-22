using Microsoft.EntityFrameworkCore;
using testt.Models;

namespace testt.Data
{
    public class ApiDbContext : DbContext
    {

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        public DbSet<Promo> Promos { get; set; }
    }
}
