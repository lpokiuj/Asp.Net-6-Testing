using Microsoft.EntityFrameworkCore;
using testt.Models;

namespace testt.Data
{
    public class ApiDbContext : DbContext
    {

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        /*protected readonly IConfiguration Configuration;
        public void DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to postgres with connection string from app settings
            options.UseNpgsql(Configuration.GetConnectionString("SampleDbConnection"));
        }*/

        public DbSet<Promo> Promos { get; set; }
    }
}
