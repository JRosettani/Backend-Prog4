using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend_Prog4.DTOs.Pedido;
using Backend_Prog4.Services;
using Backend_Prog4.Utils.Repuestas;
using Backend_Prog4.Enums;

namespace Backend_Prog4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoService _service;

        public PedidoController(PedidoService service)
        {
            _service = service;
        }

        // GET: Obtener todos los pedidos
        [HttpGet]
        [Authorize(Roles = ROL.ADMIN)]
        public async Task<IActionResult> ObtenerTodos()
        {
            try
            {
                var pedidos = await _service.ObtenerTodos();
                RespuestaExito exito = new RespuestaExito(
                    "Pedidos obtenidos correctamente",
                    HttpStatusCode.OK,
                    pedidos
                );
                return StatusCode((int)exito.CodigoEstado, exito.Res);
            }
            catch (Exception error)
            {
                return StatusCode(500, new Respuesta(error.Message));
            }
        }

        // GET: Obtener pedido por ID
        [HttpGet("{id}")]
        [Authorize(Roles = ROL.ADMIN)]
        public async Task<IActionResult> ObtenerUnoPorId(int id)
        {
            try
            {
                var pedido = await _service.ObtenerUnoPorId(id);
                RespuestaExito exito = new RespuestaExito(
                    "Pedido obtenido correctamente",
                    HttpStatusCode.OK,
                    pedido
                );
                return StatusCode((int)exito.CodigoEstado, exito.Res);
            }
            catch (RespuestaError error)
            {
                return StatusCode((int)error.CodigoEstado, error.Res);
            }
            catch (Exception error)
            {
                return StatusCode(500, new Respuesta(error.Message));
            }
        }

        // GET: Buscar pedidos pendientes por email del cliente
        [HttpGet("cliente/{email}")]
        [Authorize(Roles = ROL.ADMIN)]
        public async Task<IActionResult> ObtenerPorEmail(string email)
        {
            try
            {
                var pedidos = await _service.ObtenerPorEmail(email);
                RespuestaExito exito = new RespuestaExito(
                    "Pedidos del cliente obtenidos correctamente",
                    HttpStatusCode.OK,
                    pedidos
                );
                return StatusCode((int)exito.CodigoEstado, exito.Res);
            }
            catch (RespuestaError error)
            {
                return StatusCode((int)error.CodigoEstado, error.Res);
            }
            catch (Exception error)
            {
                return StatusCode(500, new Respuesta(error.Message));
            }
        }

        [HttpGet("mis-pedidos")]
        [Authorize(Roles = $"{ROL.ADMIN},{ROL.USER}")]
        public async Task<IActionResult> ObtenerMisPedidos()
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue("Id")!);
                var pedidos = await _service.ObtenerMisPedidos(usuarioId);
                RespuestaExito exito = new RespuestaExito(
                    "Pedidos obtenidos correctamente",
                    HttpStatusCode.OK,
                    pedidos
                );
                return StatusCode((int)exito.CodigoEstado, exito.Res);
            }
            catch (RespuestaError error)
            {
                return StatusCode((int)error.CodigoEstado, error.Res);
            }
            catch (Exception error)
            {
                return StatusCode(500, new Respuesta(error.Message));
            }
        }

        // POST: Crear pedido
        [HttpPost]
        [Authorize(Roles = $"{ROL.ADMIN},{ROL.USER}")]
        public async Task<IActionResult> CrearPedido([FromBody] CrearPedidoDTO dto)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue("Id")!);
                var pedido = await _service.CrearPedido(usuarioId, dto);
                RespuestaExito exito = new RespuestaExito(
                    "Pedido creado correctamente",
                    HttpStatusCode.Created,
                    pedido
                );
                return StatusCode((int)exito.CodigoEstado, exito.Res);
            }
            catch (RespuestaError error)
            {
                return StatusCode((int)error.CodigoEstado, error.Res);
            }
            catch (Exception error)
            {
                return StatusCode(500, new Respuesta(error.Message));
            }
        }

        // PUT: Actualizar estado del pedido
        [HttpPut("{id}")]
        [Authorize(Roles = ROL.ADMIN)]
        public async Task<IActionResult> ActualizarEstado(int id, [FromBody] ActualizarPedidoDTO dto)
        {
            try
            {
                var pedido = await _service.ActualizarEstado(id, dto);
                RespuestaExito exito = new RespuestaExito(
                    "Estado del pedido actualizado correctamente",
                    HttpStatusCode.OK,
                    pedido
                );
                return StatusCode((int)exito.CodigoEstado, exito.Res);
            }
            catch (RespuestaError error)
            {
                return StatusCode((int)error.CodigoEstado, error.Res);
            }
            catch (Exception error)
            {
                return StatusCode(500, new Respuesta(error.Message));
            }
        }

        [HttpDelete("mis-pedidos/{id}")]
        [Authorize(Roles = $"{ROL.ADMIN},{ROL.USER}")]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CancelarMiPedido(int id)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue("Id")!);
                await _service.CancelarMiPedido(id, usuarioId);
                RespuestaExito exito = new RespuestaExito(
                    "Pedido cancelado correctamente. Stock devuelto.",
                    HttpStatusCode.OK
                );
                return StatusCode((int)exito.CodigoEstado, exito.Res);
            }
            catch (RespuestaError error)
            {
                return StatusCode((int)error.CodigoEstado, error.Res);
            }
            catch (Exception error)
            {
                return StatusCode(500, new Respuesta(error.Message));
            }
        }

        [HttpGet("mis-pedidos/{id}")]
        [Authorize(Roles = $"{ROL.ADMIN},{ROL.USER}")]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ObtenerMiPedidoPorId(int id)
        {
            try
            {
                var usuarioId = int.Parse(User.FindFirstValue("Id")!);
                var pedido = await _service.ObtenerMiPedidoPorId(id, usuarioId);
                RespuestaExito exito = new RespuestaExito(
                    "Pedido obtenido correctamente",
                    HttpStatusCode.OK,
                    pedido
                );
                return StatusCode((int)exito.CodigoEstado, exito.Res);
            }
            catch (RespuestaError error)
            {
                return StatusCode((int)error.CodigoEstado, error.Res);
            }
            catch (Exception error)
            {
                return StatusCode(500, new Respuesta(error.Message));
            }
        }


    }
}