using System.Net;
using Backend_Prog4.DTOs.Pedido;
using Backend_Prog4.Services;
using Backend_Prog4.Utils.Repuestas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        [Authorize(Roles = $"{ROL.ADMIN},{ROL.USER}")]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Respuesta>> CrearPedido([FromBody] CrearPedidoDTO dto)
        {
            try
            {
                var pedido = await _service.CrearPedido(dto);
                RespuestaExito exito = new RespuestaExito(
                    "Pedido creado correctamente!",
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
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new Respuesta(error.Message)
                );
            }
        }
    }
}