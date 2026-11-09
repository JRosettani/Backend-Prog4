
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

        // GET: Obtener todos los pedidos
        public async Task<List<Pedido>> ObtenerTodos()
        {
            return await _pedidoRepo.ObtenerTodos(
                includes: new Expression<Func<Pedido, object>>[]
                {
                    p => p.Usuario,
                    p => p.EstadoPedido,
                    p => p.PedidoItems
                }
            );
        }

        // GET: Obtener un pedido por ID
        public async Task<Pedido> ObtenerUnoPorId(int id)
        {
            var pedido = await _pedidoRepo.ObtenerUno(
                p => p.Id == id,
                includes: new Expression<Func<Pedido, object>>[]
                {
                    p => p.Usuario,
                    p => p.EstadoPedido,
                    p => p.PedidoItems
                }
            );

            if (pedido == null)
                throw new RespuestaError(
                    "Pedido no encontrado",
                    HttpStatusCode.NotFound
                );

            return pedido;
        }

        // POST: Crear un pedido
        public async Task<Pedido> CrearPedido(CrearPedidoDTO dto)
        {
            if (dto.Items == null || dto.Items.Count == 0)
                throw new RespuestaError(
                    "El pedido debe contener al menos un producto",
                    HttpStatusCode.BadRequest
                );

            if (dto.Items.Any(i => i.Cantidad <= 0))
                throw new RespuestaError(
                    "Las cantidades deben ser mayores a cero",
                    HttpStatusCode.BadRequest
                );

            var usuario = await _usuarioRepo.ObtenerUno(
                u => u.Id == dto.UsuarioId
            );

            if (usuario == null)
                throw new RespuestaError(
                    "Usuario no encontrado",
                    HttpStatusCode.NotFound
                );

            var estadoPendiente = await _estadoRepo.ObtenerUno(
                e => e.Nombre == "Pendiente"
            );

            if (estadoPendiente == null)
                throw new RespuestaError(
                    "Estado 'Pendiente' no configurado",
                    HttpStatusCode.InternalServerError
                );

            decimal total = 0;

            var itemsParaCrear = new List<PedidoItem>();

            // Agrupar productos repetidos
            var grupos = dto.Items
                .GroupBy(i => new { i.ProductoId, i.TalleId })
                .Select(g => new
                {
                    g.Key.ProductoId,
                    g.Key.TalleId,
                    Cantidad = g.Sum(i => i.Cantidad)
                })
                .ToList();

            var variantes = new List<(ProductoTalle Variante, int Cantidad)>();

            // Validar stock y productos
            foreach (var item in grupos)
            {
                var productoTalle = await _productoTalleRepo.ObtenerUno(
                    pt => pt.ProductoId == item.ProductoId &&
                          pt.TalleId == item.TalleId,
                    includes: new Expression<Func<ProductoTalle, object>>[]
                    {
                        pt => pt.Producto
                    }
                );

                if (productoTalle == null)
                    throw new RespuestaError(
                        "El producto con ese talle no existe",
                        HttpStatusCode.NotFound
                    );

                if (productoTalle.Stock < item.Cantidad)
                    throw new RespuestaError(
                        $"Stock insuficiente para {productoTalle.Producto.Nombre}",
                        HttpStatusCode.BadRequest
                    );

                variantes.Add((productoTalle, item.Cantidad));

                itemsParaCrear.Add(new PedidoItem
                {
                    ProductoId = item.ProductoId,
                    TalleId = item.TalleId,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = productoTalle.Producto.Precio
                });

                total += productoTalle.Producto.Precio * item.Cantidad;
            }

            // Descontar stock
            foreach (var variante in variantes)
            {
                variante.Variante.Stock -= variante.Cantidad;

                await _productoTalleRepo.ActualizarUno(
                    variante.Variante
                );
            }

            // Crear pedido
            var pedido = new Pedido
            {
                Fecha = DateTime.Now,
                UsuarioId = dto.UsuarioId,
                IdEstado = estadoPendiente.Id,
                EstadoPedido = estadoPendiente,
                Total = total,
                PedidoItems = itemsParaCrear
            };

            await _pedidoRepo.CrearUno(pedido);

            return pedido;
        }

        // PUT: Actualizar solamente el estado
        public async Task<Pedido> ActualizarEstado(
            int id,
            ActualizarPedidoDTO dto)
        {
            var pedido = await _pedidoRepo.ObtenerUno(
                p => p.Id == id,
                includes: new Expression<Func<Pedido, object>>[]
                {
                    p => p.EstadoPedido
                }
            );

            if (pedido == null)
                throw new RespuestaError(
                    "Pedido no encontrado",
                    HttpStatusCode.NotFound
                );

            if (pedido.EstadoPedido.Nombre == "Cancelado")
                throw new RespuestaError(
                    "No se puede modificar un pedido cancelado",
                    HttpStatusCode.BadRequest
                );

            var nuevoEstado = await _estadoRepo.ObtenerUno(
                e => e.Id == dto.IdEstado
            );

            if (nuevoEstado == null)
                throw new RespuestaError(
                    "Estado de pedido no encontrado",
                    HttpStatusCode.NotFound
                );

            if (nuevoEstado.Nombre == "Cancelado")
                throw new RespuestaError(
                    "Para cancelar un pedido utilizá DELETE",
                    HttpStatusCode.BadRequest
                );

            pedido.IdEstado = nuevoEstado.Id;
            pedido.EstadoPedido = nuevoEstado;

            await _pedidoRepo.ActualizarUno(pedido);

            return pedido;
        }

        // DELETE: Cancelar pedido y devolver stock
        public async Task<Pedido> CancelarPedido(int id)
        {
            var pedido = await _pedidoRepo.ObtenerUno(
                p => p.Id == id,
                includes: new Expression<Func<Pedido, object>>[]
                {
                    p => p.EstadoPedido,
                    p => p.PedidoItems
                }
            );

            if (pedido == null)
                throw new RespuestaError(
                    "Pedido no encontrado",
                    HttpStatusCode.NotFound
                );

            if (pedido.EstadoPedido.Nombre == "Cancelado")
                throw new RespuestaError(
                    "El pedido ya está cancelado",
                    HttpStatusCode.BadRequest
                );

            var estadoCancelado = await _estadoRepo.ObtenerUno(
                e => e.Nombre == "Cancelado"
            );

            if (estadoCancelado == null)
                throw new RespuestaError(
                    "Estado 'Cancelado' no configurado",
                    HttpStatusCode.InternalServerError
                );

            var variantes = new List<(ProductoTalle Variante, int Cantidad)>();

            // Buscar todas las variantes antes de devolver stock
            foreach (var item in pedido.PedidoItems)
            {
                var productoTalle = await _productoTalleRepo.ObtenerUno(
                    pt => pt.ProductoId == item.ProductoId &&
                          pt.TalleId == item.TalleId
                );

                if (productoTalle == null)
                    throw new RespuestaError(
                        "No se encontró una variante de producto del pedido",
                        HttpStatusCode.NotFound
                    );

                variantes.Add((productoTalle, item.Cantidad));
            }

            // Devolver stock
            foreach (var variante in variantes)
            {
                variante.Variante.Stock += variante.Cantidad;

                await _productoTalleRepo.ActualizarUno(
                    variante.Variante
                );
            }

            // Cambiar estado a Cancelado
            pedido.IdEstado = estadoCancelado.Id;
            pedido.EstadoPedido = estadoCancelado;

            await _pedidoRepo.ActualizarUno(pedido);

            return pedido;
        }
    }
}
