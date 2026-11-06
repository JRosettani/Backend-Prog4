using System.Net;
using Backend_Prog4.DTOs.ProductoTalle;
using Backend_Prog4.Enums;
using Backend_Prog4.Services;
using Backend_Prog4.Utils.Repuestas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Prog4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ProductoTalleController : ControllerBase
    {
        private readonly ProductoTalleServices _service;

        public ProductoTalleController(ProductoTalleServices service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        public async Task<ActionResult<Respuesta>> ObtenerTodos()
        {
            try
            {
                var todos = await _service.ObtenerTodos();
                RespuestaExito exito = new RespuestaExito("talles de productos obtenidos!", HttpStatusCode.OK, todos);
                return StatusCode((int)exito.CodigoEstado, exito.Res);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta(error.Message));
            }
        }

        [HttpGet("{productoId}/{talleId}")]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Respuesta>> ObtenerUnoPorId(int productoId, int talleId)
        {
            try
            {
                var productoTalle = await _service.ObtenerUnoPorId(productoId, talleId);
                RespuestaExito exito = new RespuestaExito("talle del producto encontrado!", HttpStatusCode.OK, productoTalle);
                return StatusCode((int)exito.CodigoEstado, exito.Res);
            }
            catch (RespuestaError error)
            {
                return StatusCode((int)error.CodigoEstado, error.Res);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta(error.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = ROL.ADMIN)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Respuesta>> CrearUno([FromBody] CrearActualizarProductoTalleDTO crearDto)
        {
            try
            {
                var productoTalle = await _service.CrearUno(crearDto);
                RespuestaExito exito = new RespuestaExito("Se creo correctamente!", HttpStatusCode.Created, productoTalle);
                return StatusCode((int)exito.CodigoEstado, exito.Res);
            }
            catch (RespuestaError error)
            {
                return StatusCode((int)error.CodigoEstado, error.Res);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta(error.Message));
            }
        }

        [HttpPut]
        [Authorize(Roles = ROL.ADMIN)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Respuesta>> ActualizarUno([FromBody] CrearActualizarProductoTalleDTO dto)
        {
            try
            {
                var productoTalle = await _service.ActualizarUnoPorId(dto.ProductoId, dto.TalleId, dto);
                RespuestaExito exito = new RespuestaExito("Se actualizo correctamente!", HttpStatusCode.OK, productoTalle);
                return StatusCode((int)exito.CodigoEstado, exito.Res);
            }
            catch (RespuestaError error)
            {
                return StatusCode((int)error.CodigoEstado, error.Res);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta(error.Message));
            }
        }

        [HttpDelete("{productoId}/{talleId}")]
        [Authorize(Roles = ROL.ADMIN)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Respuesta>> EliminarUno(int productoId, int talleId)
        {
            try
            {
                await _service.EliminarUnoPorId(productoId, talleId);
                RespuestaExito exito = new RespuestaExito("Se elimino correctamente!", HttpStatusCode.OK);
                return StatusCode((int)exito.CodigoEstado, exito.Res);
            }
            catch (RespuestaError error)
            {
                return StatusCode((int)error.CodigoEstado, error.Res);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta(error.Message));
            }
        }
    }
}