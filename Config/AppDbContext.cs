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

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<EstadoPedido> EstadoPedidos { get; set; }
        //public DbSet<Producto> Productos { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>()
                .HasIndex(c => c.Nombre)
                .IsUnique();

            modelBuilder.Entity<Talle>()
                .HasIndex(t => t.Nombre)
                .IsUnique();

            modelBuilder.Entity<EstadoPedido>()
                .HasIndex(ep => ep.Nombre)
                .IsUnique();

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.EstadoPedido)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Usuario)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);


            //modelBuilder.Entity<Producto>().

        }
    }
}
