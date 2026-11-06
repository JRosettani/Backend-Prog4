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
        public DbSet<Producto> Productos { get; set; }
        public DbSet<ProductoTalle> ProductosTalles { get; set; }
        public DbSet<PedidoItem> PedidoItems { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Índices únicos
            modelBuilder.Entity<Categoria>()
                .HasIndex(c => c.Nombre).IsUnique();

            modelBuilder.Entity<Talle>()
                .HasIndex(t => t.Nombre).IsUnique();

            modelBuilder.Entity<Marca>()
                .HasIndex(m => m.Nombre).IsUnique();

            modelBuilder.Entity<EstadoPedido>()
                .HasIndex(ep => ep.Nombre).IsUnique();

            // Seed de roles
            modelBuilder.Entity<Rol>().HasData(
                new Rol() { Id = 1, Nombre = "Admin" },
                new Rol() { Id = 2, Nombre = "User" }
            );

            // Usuario ↔ Rol (many-to-many con entidad intermedia)
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Roles).WithMany()
                .UsingEntity<UsuarioRol>(
                    l => l.HasOne<Rol>().WithMany().HasForeignKey(e => e.RolId),
                    r => r.HasOne<Usuario>().WithMany().HasForeignKey(e => e.UsuarioId)
                );

            // Producto → Categoria
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)
                .WithMany()
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Producto → Marca
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Marca)
                .WithMany()
                .HasForeignKey(p => p.MarcaId)
                .OnDelete(DeleteBehavior.Restrict);

            // ProductoTalle (PK compuesta)
            modelBuilder.Entity<ProductoTalle>()
                .HasKey(pt => new { pt.ProductoId, pt.TalleId });

            modelBuilder.Entity<ProductoTalle>()
                .HasOne(pt => pt.Producto)
                .WithMany(p => p.Talles)
                .HasForeignKey(pt => pt.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductoTalle>()
                .HasOne(pt => pt.Talle)
                .WithMany(t => t.ProductoTalles)
                .HasForeignKey(pt => pt.TalleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Pedido → EstadoPedido
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.EstadoPedido)
                .WithMany()
                .HasForeignKey(p => p.IdEstado)
                .OnDelete(DeleteBehavior.Restrict);

            // Pedido → Usuario
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // PedidoItem → Pedido
            modelBuilder.Entity<PedidoItem>()
                .HasOne(pi => pi.Pedido)
                .WithMany(p => p.PedidoItems)
                .HasForeignKey(pi => pi.PedidoId)
                .OnDelete(DeleteBehavior.Restrict);

            // PedidoItem → Producto
            modelBuilder.Entity<PedidoItem>()
                .HasOne(pi => pi.Producto)
                .WithMany()
                .HasForeignKey(pi => pi.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            // PedidoItem → Talle
            modelBuilder.Entity<PedidoItem>()
                .HasOne(pi => pi.Talle)
                .WithMany()
                .HasForeignKey(pi => pi.TalleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}