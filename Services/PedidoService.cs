
using System.Linq.Expressions;
using System.Net;
using Backend_Prog4.DTOs.Pedido;
using Backend_Prog4.Models;
using Backend_Prog4.Repositories;
using Backend_Prog4.Utils.Repuestas;
using Microsoft.EntityFrameworkCore;

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
        public async Task<List<PedidoRespuestaDTO>> ObtenerTodos()
        {
            var pedidos = await _pedidoRepo.ObtenerTodos(
                thenIncludes: q => q
                    .Include(p => p.Usuario)
                    .Include(p => p.EstadoPedido)
                    .Include(p => p.PedidoItems)
                        .ThenInclude(i => i.Producto)
                    .Include(p => p.PedidoItems)
                        .ThenInclude(i => i.Talle)
            );

            return pedidos.Select(p => new PedidoRespuestaDTO
            {
                Id = p.Id,
                Fecha = p.Fecha,
                Total = p.Total,
                EstadoNombre = p.EstadoPedido.Nombre,
                UsuarioEmail = p.Usuario.Email,
                Items = p.PedidoItems.Select(i => new PedidoItemRespuestaDTO
                {
                    Cantidad = i.Cantidad,
                    PrecioUnitario = i.PrecioUnitario,
                    ProductoNombre = i.Producto.Nombre,
                    TalleNombre = i.Talle.Nombre
                }).ToList()
            }).ToList();
        }

        // GET: Obtener un pedido por ID
        public async Task<PedidoRespuestaDTO> ObtenerUnoPorId(int id)
        {
            var pedido = await _pedidoRepo.ObtenerUno(
                p => p.Id == id,
                thenIncludes: q => q
                    .Include(p => p.Usuario)
                    .Include(p => p.EstadoPedido)
                    .Include(p => p.PedidoItems)
                        .ThenInclude(i => i.Producto)
                    .Include(p => p.PedidoItems)
                        .ThenInclude(i => i.Talle)
            ) ?? throw new RespuestaError("Pedido no encontrado", HttpStatusCode.NotFound);

            return new PedidoRespuestaDTO
            {
                Id = pedido.Id,
                Fecha = pedido.Fecha,
                Total = pedido.Total,
                EstadoNombre = pedido.EstadoPedido.Nombre,
                UsuarioEmail = pedido.Usuario.Email,
                Items = pedido.PedidoItems.Select(i => new PedidoItemRespuestaDTO
                {
                    Cantidad = i.Cantidad,
                    PrecioUnitario = i.PrecioUnitario,
                    ProductoNombre = i.Producto.Nombre,
                    TalleNombre = i.Talle.Nombre
                }).ToList()
            };
        }

        // GET: Buscar pedidos pendientes por email del cliente
        public async Task<List<PedidoRespuestaDTO>> ObtenerPorEmail(string email)
        {
            var usuario = await _usuarioRepo.ObtenerUno(u => u.Email == email)
                ?? throw new RespuestaError("Usuario no encontrado", HttpStatusCode.NotFound);

            var pedidos = await _pedidoRepo.ObtenerTodos(
                predicate: p => p.UsuarioId == usuario.Id && p.EstadoPedido.Nombre == "Pendiente",
                thenIncludes: q => q
                    .Include(p => p.Usuario)
                    .Include(p => p.EstadoPedido)
                    .Include(p => p.PedidoItems)
                        .ThenInclude(i => i.Producto)
                    .Include(p => p.PedidoItems)
                        .ThenInclude(i => i.Talle)
            );

            return pedidos.Select(p => new PedidoRespuestaDTO
            {
                Id = p.Id,
                Fecha = p.Fecha,
                Total = p.Total,
                EstadoNombre = p.EstadoPedido.Nombre,
                UsuarioEmail = p.Usuario.Email,
                Items = p.PedidoItems.Select(i => new PedidoItemRespuestaDTO
                {
                    Cantidad = i.Cantidad,
                    PrecioUnitario = i.PrecioUnitario,
                    ProductoNombre = i.Producto.Nombre,
                    TalleNombre = i.Talle.Nombre
                }).ToList()
            }).ToList();
        }

        // POST: Crear un pedido
        public async Task<PedidoRespuestaDTO> CrearPedido(int usuarioId, CrearPedidoDTO dto)
        {
            // Validar que el pedido tenga al menos un item
            if (dto.Items == null || dto.Items.Count == 0)
                throw new RespuestaError(
                    "El pedido debe contener al menos un producto",
                    HttpStatusCode.BadRequest
                );

            // Validar que todas las cantidades sean mayores a cero
            if (dto.Items.Any(i => i.Cantidad <= 0))
                throw new RespuestaError(
                    "Las cantidades deben ser mayores a cero",
                    HttpStatusCode.BadRequest
                );

            // Verificar que el usuario existe en la DB
            var usuario = await _usuarioRepo.ObtenerUno(u => u.Id == usuarioId)
                ?? throw new RespuestaError("Usuario no encontrado", HttpStatusCode.NotFound);

            // Obtener el estado "Pendiente" (debe estar cargado en la DB)
            var estadoPendiente = await _estadoRepo.ObtenerUno(e => e.Nombre == "Pendiente")
                ?? throw new RespuestaError("Estado 'Pendiente' no configurado", HttpStatusCode.InternalServerError);

            decimal total = 0;
            var itemsParaCrear = new List<PedidoItem>();

            // Lista para guardar las variantes validadas y luego descontar stock
            // Se separa en dos pasos para no descontar stock si algún item falla la validación
            var variantes = new List<(ProductoTalle Variante, int Cantidad)>();

            // Agrupar items por ProductoId + TalleId para evitar procesar duplicados
            // Ej: si mandan 2 veces el mismo producto/talle, se suman las cantidades
            var grupos = dto.Items
                .GroupBy(i => new { i.ProductoId, i.TalleId })
                .Select(g => new
                {
                    g.Key.ProductoId,
                    g.Key.TalleId,
                    Cantidad = g.Sum(i => i.Cantidad)
                })
                .ToList();

            // Validar existencia y stock de cada variante (producto + talle)
            foreach (var item in grupos)
            {
                // Buscar el ProductoTalle con sus navegaciones para acceder a Nombre y Precio
                var productoTalle = await _productoTalleRepo.ObtenerUno(
                    pt => pt.ProductoId == item.ProductoId && pt.TalleId == item.TalleId,
                    includes: new Expression<Func<ProductoTalle, object>>[]
                    {
                pt => pt.Producto,  // para acceder a Nombre y Precio
                pt => pt.Talle      // para acceder a Nombre del talle
                    }
                ) ?? throw new RespuestaError(
                    "El producto con ese talle no existe",
                    HttpStatusCode.NotFound
                );

                // Verificar que hay suficiente stock disponible
                if (productoTalle.Stock < item.Cantidad)
                    throw new RespuestaError(
                        $"Stock insuficiente para {productoTalle.Producto.Nombre}",
                        HttpStatusCode.BadRequest
                    );

                // Guardar la variante validada para descontar stock después
                variantes.Add((productoTalle, item.Cantidad));

                // Construir el PedidoItem con el precio actual del producto
                // PrecioUnitario se guarda para mantener el precio histórico
                // aunque el producto cambie de precio en el futuro
                itemsParaCrear.Add(new PedidoItem
                {
                    ProductoId = item.ProductoId,
                    TalleId = item.TalleId,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = productoTalle.Producto.Precio
                });

                // Acumular el total del pedido
                total += productoTalle.Producto.Precio * item.Cantidad;
            }

            // Descontar stock solo si todas las validaciones pasaron
            foreach (var variante in variantes)
            {
                variante.Variante.Stock -= variante.Cantidad;
                await _productoTalleRepo.ActualizarUno(variante.Variante);
            }

            // Construir y persistir el pedido con todos sus items
            var pedido = new Pedido
            {
                Fecha = DateTime.Now,
                UsuarioId = usuarioId,
                IdEstado = estadoPendiente.Id,
                Total = total,
                PedidoItems = itemsParaCrear  // EF Core guarda los items automáticamente
            };

            var pedidoCreado = await _pedidoRepo.CrearUno(pedido);

            // Mapear a DTO de respuesta para evitar la referencia circular
            // Pedido → PedidoItems → PedidoItem.Pedido → PedidoItems → ...
            return new PedidoRespuestaDTO
            {
                Id = pedidoCreado.Id,
                Fecha = pedidoCreado.Fecha,
                Total = pedidoCreado.Total,
                EstadoNombre = estadoPendiente.Nombre,
                Items = variantes.Select(v => new PedidoItemRespuestaDTO
                {
                    Cantidad = v.Cantidad,
                    PrecioUnitario = v.Variante.Producto.Precio,
                    ProductoNombre = v.Variante.Producto.Nombre,
                    TalleNombre = v.Variante.Talle.Nombre
                }).ToList()
            };
        }

        // PUT: Actualizar solamente el estado
        public async Task<ActualizarEstadoPedidoRespuestaDTO> ActualizarEstado(int id, ActualizarPedidoDTO dto)
        {
            // Buscar el pedido con su estado actual
            var pedido = await _pedidoRepo.ObtenerUno(
                p => p.Id == id,
                includes: new Expression<Func<Pedido, object>>[]
                {
            p => p.EstadoPedido
                }
            ) ?? throw new RespuestaError("Pedido no encontrado", HttpStatusCode.NotFound);

            // No se puede modificar un pedido cancelado
            if (pedido.EstadoPedido.Nombre == "Cancelado")
                throw new RespuestaError(
                    "No se puede modificar un pedido cancelado",
                    HttpStatusCode.BadRequest
                );

            // Verificar que el nuevo estado existe
            var nuevoEstado = await _estadoRepo.ObtenerUno(e => e.Id == dto.IdEstado)
                ?? throw new RespuestaError("Estado de pedido no encontrado", HttpStatusCode.NotFound);

            // Cancelar debe hacerse por DELETE, no por PUT
            if (nuevoEstado.Nombre == "Cancelado")
                throw new RespuestaError(
                    "Para cancelar un pedido utilizá DELETE",
                    HttpStatusCode.BadRequest
                );

            // Guardar el nombre del estado anterior para la respuesta
            var estadoAnterior = pedido.EstadoPedido.Nombre;

            // Actualizar el estado
            pedido.IdEstado = nuevoEstado.Id;
            pedido.EstadoPedido = nuevoEstado;
            await _pedidoRepo.ActualizarUno(pedido);

            return new ActualizarEstadoPedidoRespuestaDTO
            {
                Id = pedido.Id,
                EstadoAnterior = estadoAnterior,
                EstadoNuevo = nuevoEstado.Nombre
            };
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

        public async Task<List<PedidoRespuestaDTO>> ObtenerMisPedidos(int usuarioId)
        {
            var pedidos = await _pedidoRepo.ObtenerTodos(
                predicate: p => p.UsuarioId == usuarioId,
                thenIncludes: q => q
                    .Include(p => p.Usuario)
                    .Include(p => p.EstadoPedido)
                    .Include(p => p.PedidoItems)
                        .ThenInclude(i => i.Producto)
                    .Include(p => p.PedidoItems)
                        .ThenInclude(i => i.Talle)
            );

            return pedidos.Select(p => new PedidoRespuestaDTO
            {
                Id = p.Id,
                Fecha = p.Fecha,
                Total = p.Total,
                EstadoNombre = p.EstadoPedido.Nombre,
                UsuarioEmail = p.Usuario.Email,
                Items = p.PedidoItems.Select(i => new PedidoItemRespuestaDTO
                {
                    Cantidad = i.Cantidad,
                    PrecioUnitario = i.PrecioUnitario,
                    ProductoNombre = i.Producto.Nombre,
                    TalleNombre = i.Talle.Nombre
                }).ToList()
            }).ToList();
        }

        // DELETE: Cancelar pedido propio (usuario logueado)
        public async Task CancelarMiPedido(int id, int usuarioId)
        {
            var pedido = await _pedidoRepo.ObtenerUno(
                p => p.Id == id,
                includes: new Expression<Func<Pedido, object>>[]
                {
            p => p.EstadoPedido,
            p => p.PedidoItems
                }
            ) ?? throw new RespuestaError("Pedido no encontrado", HttpStatusCode.NotFound);

            // Validar que el pedido pertenece al usuario logueado
            if (pedido.UsuarioId != usuarioId)
                throw new RespuestaError(
                    "No tenés permisos para cancelar este pedido",
                    HttpStatusCode.Forbidden
                );

            if (pedido.EstadoPedido.Nombre == "Cancelado")
                throw new RespuestaError("El pedido ya está cancelado", HttpStatusCode.BadRequest);

            // Solo se puede cancelar si está Pendiente
            if (pedido.EstadoPedido.Nombre != "Pendiente")
                throw new RespuestaError(
                    "Solo podés cancelar pedidos en estado Pendiente",
                    HttpStatusCode.BadRequest
                );

            var estadoCancelado = await _estadoRepo.ObtenerUno(e => e.Nombre == "Cancelado")
                ?? throw new RespuestaError("Estado 'Cancelado' no configurado", HttpStatusCode.InternalServerError);

            // Devolver stock
            foreach (var item in pedido.PedidoItems)
            {
                var productoTalle = await _productoTalleRepo.ObtenerUno(
                    pt => pt.ProductoId == item.ProductoId && pt.TalleId == item.TalleId
                ) ?? throw new RespuestaError(
                    "No se encontró una variante de producto del pedido",
                    HttpStatusCode.NotFound
                );

                productoTalle.Stock += item.Cantidad;
                await _productoTalleRepo.ActualizarUno(productoTalle);
            }

            pedido.IdEstado = estadoCancelado.Id;
            pedido.EstadoPedido = estadoCancelado;
            await _pedidoRepo.ActualizarUno(pedido);
        }


        public async Task<PedidoRespuestaDTO> ObtenerMiPedidoPorId(int id, int usuarioId)
        {
            var pedido = await _pedidoRepo.ObtenerUno(
                p => p.Id == id,
                thenIncludes: q => q
                    .Include(p => p.Usuario)
                    .Include(p => p.EstadoPedido)
                    .Include(p => p.PedidoItems)
                        .ThenInclude(i => i.Producto)
                    .Include(p => p.PedidoItems)
                        .ThenInclude(i => i.Talle)
            ) ?? throw new RespuestaError("Pedido no encontrado", HttpStatusCode.NotFound);

            if (pedido.UsuarioId != usuarioId)
                throw new RespuestaError("No tenés permisos para ver este pedido", HttpStatusCode.Forbidden);

            return new PedidoRespuestaDTO
            {
                Id = pedido.Id,
                Fecha = pedido.Fecha,
                Total = pedido.Total,
                EstadoNombre = pedido.EstadoPedido.Nombre,
                UsuarioEmail = pedido.Usuario.Email,
                Items = pedido.PedidoItems.Select(i => new PedidoItemRespuestaDTO
                {
                    Cantidad = i.Cantidad,
                    PrecioUnitario = i.PrecioUnitario,
                    ProductoNombre = i.Producto.Nombre,
                    TalleNombre = i.Talle.Nombre
                }).ToList()
            };
        }


    }
}
