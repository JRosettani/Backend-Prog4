using Backend_Prog4.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_Prog4.Config
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Talle> Talles { get; set; }

        public DbSet<Marca> Marcas { get; set; }

        //public DbSet<Producto> Productos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>()
                .HasIndex(c => c.Nombre)
                .IsUnique();

            modelBuilder.Entity<Talle>()
                .HasIndex(t => t.Nombre)
                .IsUnique();

            //modelBuilder.Entity<Producto>().

        }
    }
}
