using Backend_Prog4.Models;
using Microsoft.EntityFrameworkCore;
using panaderia.Models;

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

        public DbSet<Rol> Roles { get; set; }
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

            modelBuilder.Entity<Marca>()
                .HasIndex(m => m.Nombre)
                .IsUnique();


            modelBuilder.Entity<Rol>().HasData(
                new Rol() { Id = 1, Nombre = "Admin" },
                new Rol() { Id = 2, Nombre = "User" }
            );

            modelBuilder.Entity<Usuario>()
           .HasMany(p => p.Roles).WithMany()
           .UsingEntity<UsuarioRol>(
               l => l.HasOne<Rol>().WithMany().HasForeignKey(e => e.RolId),
               r => r.HasOne<Usuario>().WithMany().HasForeignKey(e => e.UsuarioId)
           );

            //modelBuilder.Entity<Producto>().

        }
    }
}
