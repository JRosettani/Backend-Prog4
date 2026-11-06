using System.Net;
using Backend_Prog4.DTOs.PedidoItem;
using Backend_Prog4.Models;
using Backend_Prog4.Repositories;
using Backend_Prog4.Utils.Repuestas;


namespace Backend_Prog4.Services
{
    public class PedidoItemServices
    {
        private readonly IRepository<PedidoItem> _repo;

        public PedidoItemServices(IRepository<PedidoItem> repo)
        {
            _repo = repo;
        }

        public async Task<List<PedidoItem>> ObtenerTodos()
        {
            var lista = await _repo.ObtenerTodos(
                includes: new System.Linq.Expressions.Expression<Func<PedidoItem, object>>[]
                {
                p => p.pedido,
                p => p.producto,
                p => p.talle
                }
            );

            return lista;
        }

        public async Task<PedidoItem> ObtenerUnoPorId(int id)
        {
            var item = await _repo.ObtenerUno(
                p => p.Id == id,
                new System.Linq.Expressions.Expression<Func<PedidoItem, object>>[]
                {
                p => p.pedido,
                p => p.producto,
                p => p.talle
                }
            );

            if (item == null)
            {
                throw new RespuestaError(
                    $"El PedidoItem con id = {id} no se encontro",
                    HttpStatusCode.NotFound
                );
            }

            return item;
        }

        public async Task<PedidoItem> CrearUno(CrearPedidoItem dto)
        {
            var item = new PedidoItem()
            {
                Cantidad = dto.Cantidad,
                PrecioUnitario = dto.PrecioUnitario,
                PedidoId = dto.PedidoId,
                ProductoId = dto.ProductoId,
                TalleId = dto.TalleId
            };

            await _repo.CrearUno(item);

            return item;
        }

        public async Task<PedidoItem> ActualizarUnoPorId(
            int id,
            ActualizarPedidoItem dto)
        {
            PedidoItem item = await ObtenerUnoPorId(id);

            if (dto.Cantidad.HasValue)
            {
                item.Cantidad = dto.Cantidad.Value;
            }

            if (dto.PrecioUnitario.HasValue)
            {
                item.PrecioUnitario = dto.PrecioUnitario.Value;
            }

            await _repo.ActualizarUno(item);

            return item;
        }

        public async Task EliminarUnoPorId(int id)
        {
            PedidoItem item = await ObtenerUnoPorId(id);

            await _repo.EliminarUno(item);
        }
    }
}
