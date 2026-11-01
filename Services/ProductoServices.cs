using Backend_Prog4.DTOs.Producto;
using Backend_Prog4.DTOs.Producto.Backend_Prog4.DTOs.Producto;
using Backend_Prog4.Models;
using Backend_Prog4.Repositories;
using Backend_Prog4.Utils.Repuestas;
using System.Net;

namespace Backend_Prog4.Services
{
    public class ProductoServices
    {
        
        private readonly IRepository<Producto> _repo;

        public ProductoServices(IRepository<Producto> repo)
        {
            _repo = repo;
        }

        public async Task<List<Producto>> ObtenerTodos()
        {
            var lista = await _repo.ObtenerTodos();
            return lista;
        }

        public async Task<Producto> ObtenerUnoPorId(int id)
        {
            var prod = await _repo.ObtenerUno(p => p.Id == id);

            if (prod == null)
            {
                throw new RespuestaError(
                    $"El producto con id = {id} no se encontro",
                    HttpStatusCode.NotFound
                );
            }

            return prod;
        }

        public async Task<Producto> CrearUno(CrearProductoDTO dto)
        {
            var prod = new Producto()
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                ImagenUrl = dto.ImagenUrl,
                CategoriaId = dto.CategoriaId,
                MarcaId = dto.MarcaId
            };

            await _repo.CrearUno(prod);

            return prod;
        }

        public async Task<Producto> ActualizarUnoPorId(int id, ActualizarProductoDTO dto)
        {
            Producto prod = await ObtenerUnoPorId(id);

            if (dto.Nombre != null)
            {
                prod.Nombre = dto.Nombre;
            }

            if (dto.Descripcion != null)
            {
                prod.Descripcion = dto.Descripcion;
            }

            if (dto.Precio != 0)
            {
                prod.Precio = dto.Precio;
            }

            if (dto.ImagenUrl != null)
            {
                prod.ImagenUrl = dto.ImagenUrl;
            }

            await _repo.ActualizarUno(prod);

            return prod;
        }

        public async Task EliminarUnoPorId(int id)
        {
            Producto prod = await ObtenerUnoPorId(id);

            await _repo.EliminarUno(prod);
        }

        public async Task<List<Producto>> ObtenerPorIds(List<int> Ids)
        {
            if (Ids == null || Ids.Count == 0)
            {
                throw new RespuestaError(
                    "La lista esta vacia",
                    HttpStatusCode.BadRequest
                );
            }

            var prods = await _repo.ObtenerTodos(prod => Ids.Contains(prod.Id));

            return prods;
        }
    }

}

