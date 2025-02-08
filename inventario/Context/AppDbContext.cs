using inventario.Models;
using Microsoft.EntityFrameworkCore;

namespace inventario.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<inventario.Models.Piezas> Piezas { get; set; } = default!;
        public DbSet<PiezaResultado> PiezasResultado { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PiezaResultado>().HasNoKey(); // Evita error si no hay PK
        }

    }
}