using Microsoft.EntityFrameworkCore;
using Tsea.Domain.Models;

namespace Tsea.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Equipment> Equipments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Equipment>().HasKey(e => e.Id);
            modelBuilder.Entity<Equipment>().Property(e => e.Name).IsRequired().HasMaxLength(150);
            modelBuilder.Entity<Equipment>().Property(e => e.SerialNumber).IsRequired().HasMaxLength(50);
        }
    }
}
