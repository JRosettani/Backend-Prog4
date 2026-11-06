using System.Linq.Expressions;
using System.Net;
using Backend_Prog4.DTOs.Pedido;
using Backend_Prog4.Models;
using Backend_Prog4.Repositories;
using Backend_Prog4.Utils.Repuestas;

namespace Backend_Prog4.Services
{
    public class PedidoService
    {
        private readonly IRepository<Pedido> _pedidoRepo;
        private readonly IRepository<Usuario> _usuarioRepo;
        private readonly IRepository<EstadoPedido> _estadoRepo;
        private readonly IRepository<ProductoTalle> _productoTalleRepo;

        public PedidoService(
            IRepository<Pedido> pedidoRepo,
            IRepository<Usuario> usuarioRepo,
            IRepository<EstadoPedido> estadoRepo,
            IRepository<ProductoTalle> productoTalleRepo)
        {
            _pedidoRepo = pedidoRepo;
            _usuarioRepo = usuarioRepo;
            _estadoRepo = estadoRepo;
            _productoTalleRepo = productoTalleRepo;
        }

        public async Task<Pedido> CrearPedido(CrearPedidoDTO dto)
        {
            // 1. Verificar que el usuario existe
            var usuario = await _usuarioRepo.ObtenerUno(u => u.Id == dto.UsuarioId);
            if (usuario == null)
                throw new RespuestaError("Usuario no encontrado", HttpStatusCode.NotFound);

            // 2. Obtener el estado inicial "Pendiente"
            var estadoPendiente = await _estadoRepo.ObtenerUno(e => e.Nombre == "Pendiente");
            if (estadoPendiente == null)
                throw new RespuestaError("Estado 'Pendiente' no configurado", HttpStatusCode.InternalServerError);

            // 3. Validar items y calcular total
            decimal total = 0;
            var itemsParaCrear = new List<PedidoItem>();

            foreach (var itemDto in dto.Items)
            {
                var productoTalle = await _productoTalleRepo.ObtenerUno(
                    pt => pt.ProductoId == itemDto.ProductoId && pt.TalleId == itemDto.TalleId,
                    includes: new Expression<Func<ProductoTalle, object>>[] { pt => pt.Producto }
                );

                if (productoTalle == null)
                    throw new RespuestaError("El producto con ese talle no existe", HttpStatusCode.NotFound);

                if (productoTalle.Stock < itemDto.Cantidad)
                    throw new RespuestaError(
                        $"Stock insuficiente para {productoTalle.Producto.Nombre}",
                        HttpStatusCode.BadRequest
                    );

                productoTalle.Stock -= itemDto.Cantidad;
                await _productoTalleRepo.ActualizarUno(productoTalle);

                itemsParaCrear.Add(new PedidoItem
                {
                    ProductoId = itemDto.ProductoId,
                    TalleId = itemDto.TalleId,
                    Cantidad = itemDto.Cantidad,
                    PrecioUnitario = productoTalle.Producto.Precio
                });

                total += productoTalle.Producto.Precio * itemDto.Cantidad;
            }

            // 4. Crear el pedido
            var pedido = new Pedido
            {
                Fecha = DateTime.Now,
                UsuarioId = dto.UsuarioId,
                IdEstado = estadoPendiente.Id,
                Total = total,
                PedidoItems = itemsParaCrear
            };

            await _pedidoRepo.CrearUno(pedido);

            return pedido;
        }
    }
}