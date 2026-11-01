using Backend_Prog4.DTOs.ProductoTalle;
using Backend_Prog4.Models;
using Backend_Prog4.Repositories;
using Backend_Prog4.Utils.Repuestas;
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

            public async Task<List<ProductoTalle>> ObtenerTodos()
            {
                var lista = await _repo.ObtenerTodos();
                return lista;
            }

        public async Task<ProductoTalle> ObtenerUnoPorId(int productoId, int talleId) // Ahora busca con los dos Ids
        {
            var prodTalle = await _repo.ObtenerUno(
                pt => pt.ProductoId == productoId && pt.TalleId == talleId
            );

            if (prodTalle == null)
            {
                throw new RespuestaError(
                    $"No se encontró el talle {talleId} para el producto {productoId}",
                    HttpStatusCode.NotFound
                );
            }

            return prodTalle;
        }

        public async Task<ProductoTalle> CrearUno(CrearActualizarProductoTalleDTO dto)
            {
                var prodTalle = new ProductoTalle()
                {
                    ProductoId = dto.ProductoId,
                    TalleId = dto.TalleId,
                    Stock = dto.Stock
                };

                await _repo.CrearUno(prodTalle);

                return prodTalle;
            }
                   
            public async Task<ProductoTalle> ActualizarUnoPorId(int productoId, int talleId, CrearActualizarProductoTalleDTO dto)
            {
                ProductoTalle prodTalle = await ObtenerUnoPorId(productoId, talleId);

                if (dto.Stock != 0)
                {
                    prodTalle.Stock = dto.Stock;
                }

                await _repo.ActualizarUno(prodTalle);

                return prodTalle;
            }

            public async Task EliminarUnoPorId(int productoId, int talleId)
            {
                ProductoTalle prodTalle = await ObtenerUnoPorId(productoId, talleId);

                await _repo.EliminarUno(prodTalle);
            }

        public async Task<List<ProductoTalle>> ObtenerPorProductoIds(List<int> productoIds) // Trae todos los talles de los productos de la lista de ids.
        {
            if (productoIds == null || productoIds.Count == 0)
            {
                throw new RespuestaError(
                    "La lista esta vacia",
                    HttpStatusCode.BadRequest
                );
            }

            var prodTalles = await _repo.ObtenerTodos(pt => productoIds.Contains(pt.ProductoId));

            return prodTalles;
        }
    }

    }

