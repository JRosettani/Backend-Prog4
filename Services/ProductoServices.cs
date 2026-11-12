using Backend_Prog4.DTOs.Producto;
using Backend_Prog4.Models;
using Backend_Prog4.Repositories;
using Backend_Prog4.Utils.Repuestas;
using System.Linq.Expressions;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Backend_Prog4.Config;

namespace Backend_Prog4.Services
{
    public class ProductoServices
    {
        private readonly IRepository<Producto> _repo;
        private readonly AppDbContext _db;

        public ProductoServices(IRepository<Producto> repo, AppDbContext db)
        {
            _repo = repo;
            _db = db;
        }

        private async Task<Producto> ObtenerEntidadPorId(int id)
        {
            var prod = await _db.Set<Producto>()
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .Include(p => p.Talles)
                    .ThenInclude(t => t.Talle)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prod == null)
                throw new RespuestaError($"El producto con id = {id} no se encontro", HttpStatusCode.NotFound);

            return prod;
        }

        public async Task<List<ProductoDTO>> ObtenerTodos()
        {
            var lista = await _db.Set<Producto>()
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .Include(p => p.Talles)
                    .ThenInclude(t => t.Talle)
                .ToListAsync();

            return lista.Select(p => new ProductoDTO
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                ImagenUrl = p.ImagenUrl,
                MarcaId = p.MarcaId,
                MarcaNombre = p.Marca.Nombre,
                CategoriaId = p.CategoriaId,
                CategoriaNombre = p.Categoria.Nombre,
                Talles = p.Talles.Select(t => new ProductoTalleDTO
                {
                    TalleId = t.TalleId,
                    TalleNombre = t.Talle.Nombre,
                    Stock = t.Stock
                }).ToList()
            }).ToList();
        }

        public async Task<ProductoDTO> ObtenerUnoPorId(int id)
        {
            var prod = await ObtenerEntidadPorId(id);

            return new ProductoDTO
            {
                Id = prod.Id,
                Nombre = prod.Nombre,
                Descripcion = prod.Descripcion,
                Precio = prod.Precio,
                ImagenUrl = prod.ImagenUrl,
                MarcaId = prod.MarcaId,
                MarcaNombre = prod.Marca.Nombre,
                CategoriaId = prod.CategoriaId,
                CategoriaNombre = prod.Categoria.Nombre,
                Talles = prod.Talles.Select(t => new ProductoTalleDTO
                {
                    TalleId = t.TalleId,
                    TalleNombre = t.Talle.Nombre,
                    Stock = t.Stock
                }).ToList()
            };
        }

        public async Task<ProductoDTO> CrearUno(CrearProductoDTO dto)
        {
            var prod = new Producto()
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                ImagenUrl = dto.ImagenUrl,
                CategoriaId = dto.CategoriaId,
                MarcaId = dto.MarcaId,
                Talles = dto.Talles.Select(t => new ProductoTalle
                {
                    TalleId = t.TalleId,
                    Stock = t.Stock
                }).ToList()
            };

            await _repo.CrearUno(prod);

            return await ObtenerUnoPorId(prod.Id);
        }

        public async Task<ProductoDTO> ActualizarUnoPorId(int id, ActualizarProductoDTO dto)
        {
            Producto prod = await ObtenerEntidadPorId(id);

            if (dto.Nombre != null) prod.Nombre = dto.Nombre;
            if (dto.Descripcion != null) prod.Descripcion = dto.Descripcion;
            if (dto.Precio != 0) prod.Precio = dto.Precio;
            if (dto.ImagenUrl != null) prod.ImagenUrl = dto.ImagenUrl;

            if (dto.Talles.Count > 0)
            {
                prod.Talles = dto.Talles.Select(t => new ProductoTalle
                {
                    ProductoId = id,
                    TalleId = t.TalleId,
                    Stock = t.Stock
                }).ToList();
            }

            await _repo.ActualizarUno(prod);

            return await ObtenerUnoPorId(prod.Id);
        }

        public async Task EliminarUnoPorId(int id)
        {
            Producto prod = await ObtenerEntidadPorId(id);
            await _repo.EliminarUno(prod);
        }

        public async Task<List<Producto>> ObtenerPorIds(List<int> Ids)
        {
            if (Ids == null || Ids.Count == 0)
                throw new RespuestaError("La lista esta vacia", HttpStatusCode.BadRequest);

            return await _repo.ObtenerTodos(prod => Ids.Contains(prod.Id));
        }

        public async Task<List<ProductoDTO>> Buscar(string? nombre, int? categoriaId, int? marcaId, decimal? precioMin, decimal? precioMax)
        {
            var lista = await _repo.ObtenerTodos(
                // Filtro dinámico: cada condición solo se aplica si el parámetro fue enviado.
                // Si es null o vacío, la condición devuelve true y no filtra nada.
                predicate: p =>
                    (string.IsNullOrEmpty(nombre) || p.Nombre.Contains(nombre)) &&   // si no hay nombre, pasa todos; si hay, filtra por nombre
                    (categoriaId == null || p.CategoriaId == categoriaId) &&          // si no hay categoriaId, pasa todos; si hay, filtra por categoría
                    (marcaId == null || p.MarcaId == marcaId) &&                      // si no hay marcaId, pasa todos; si hay, filtra por marca
                    (precioMin == null || p.Precio >= precioMin) &&                   // si no hay precioMin, pasa todos; si hay, filtra precio mayor o igual
                    (precioMax == null || p.Precio <= precioMax),                     // si no hay precioMax, pasa todos; si hay, filtra precio menor o igual

                // Carga las relaciones necesarias para armar el DTO
                thenIncludes: q => q
                    .Include(p => p.Categoria)       // para acceder a Categoria.Nombre
                    .Include(p => p.Marca)           // para acceder a Marca.Nombre
                    .Include(p => p.Talles)          // para acceder a la lista de talles
                        .ThenInclude(t => t.Talle)   // para acceder a Talle.Nombre dentro de cada ProductoTalle
            );

            // Mapea cada entidad Producto a un ProductoDTO para no devolver la entidad cruda
            return lista.Select(p => new ProductoDTO
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                ImagenUrl = p.ImagenUrl,
                MarcaId = p.MarcaId,
                MarcaNombre = p.Marca.Nombre,
                CategoriaId = p.CategoriaId,
                CategoriaNombre = p.Categoria.Nombre,
                Talles = p.Talles.Select(t => new ProductoTalleDTO
                {
                    TalleId = t.TalleId,
                    TalleNombre = t.Talle.Nombre,
                    Stock = t.Stock
                }).ToList()
            }).ToList();
        }

    }
}