using AngularprojectApis.Models;
using Microsoft.EntityFrameworkCore;

namespace AngularprojectApis.Data
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {

        }

        public DbSet<Users> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().Property(p => p.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
        }

    }
}
