using Backend_Prog4.DTOs.ProductoTalle;
using Backend_Prog4.Models;
using Backend_Prog4.Repositories;
using Backend_Prog4.Utils.Repuestas;
using System.Linq.Expressions;
using System.Net;

namespace Backend_Prog4.Services
{
    public class ProductoTalleServices
    {
        private readonly IRepository<ProductoTalle> _repo;

        public ProductoTalleServices(IRepository<ProductoTalle> repo)
        {
            _repo = repo;
        }

        private async Task<ProductoTalle> ObtenerEntidadPorId(int productoId, int talleId)
        {
            var prodTalle = await _repo.ObtenerUno(
                pt => pt.ProductoId == productoId && pt.TalleId == talleId,
                includes: new Expression<Func<ProductoTalle, object>>[]
                {
                    pt => pt.Producto,
                    pt => pt.Talle
                }
            );

            if (prodTalle == null)
                throw new RespuestaError(
                    $"No se encontró el talle {talleId} para el producto {productoId}",
                    HttpStatusCode.NotFound
                );

            return prodTalle;
        }

        public async Task<List<ProductoTalleDTO>> ObtenerTodos()
        {
            var lista = await _repo.ObtenerTodos(
                includes: new Expression<Func<ProductoTalle, object>>[]
                {
                    pt => pt.Producto,
                    pt => pt.Talle
                }
            );

            return lista.Select(pt => new ProductoTalleDTO
            {
                ProductoId = pt.ProductoId,
                ProductoNombre = pt.Producto.Nombre,
                TalleId = pt.TalleId,
                TalleNombre = pt.Talle.Nombre,
                Stock = pt.Stock
            }).ToList();
        }

        public async Task<ProductoTalleDTO> ObtenerUnoPorId(int productoId, int talleId)
        {
            var prodTalle = await ObtenerEntidadPorId(productoId, talleId);

            return new ProductoTalleDTO
            {
                ProductoId = prodTalle.ProductoId,
                ProductoNombre = prodTalle.Producto.Nombre,
                TalleId = prodTalle.TalleId,
                TalleNombre = prodTalle.Talle.Nombre,
                Stock = prodTalle.Stock
            };
        }

        public async Task<ProductoTalleDTO> CrearUno(CrearActualizarProductoTalleDTO dto)
        {
            var prodTalle = new ProductoTalle()
            {
                ProductoId = dto.ProductoId,
                TalleId = dto.TalleId,
                Stock = dto.Stock
            };

            await _repo.CrearUno(prodTalle);

            return await ObtenerUnoPorId(prodTalle.ProductoId, prodTalle.TalleId);
        }

        public async Task<ProductoTalleDTO> ActualizarUnoPorId(int productoId, int talleId, CrearActualizarProductoTalleDTO dto)
        {
            ProductoTalle prodTalle = await ObtenerEntidadPorId(productoId, talleId);

            if (dto.Stock != 0)
                prodTalle.Stock = dto.Stock;

            await _repo.ActualizarUno(prodTalle);

            return await ObtenerUnoPorId(productoId, talleId);
        }

        public async Task EliminarUnoPorId(int productoId, int talleId)
        {
            ProductoTalle prodTalle = await ObtenerEntidadPorId(productoId, talleId);
            await _repo.EliminarUno(prodTalle);
        }

        public async Task<List<ProductoTalle>> ObtenerPorProductoIds(List<int> productoIds)
        {
            if (productoIds == null || productoIds.Count == 0)
                throw new RespuestaError("La lista esta vacia", HttpStatusCode.BadRequest);

            return await _repo.ObtenerTodos(pt => productoIds.Contains(pt.ProductoId));
        }
    }
}