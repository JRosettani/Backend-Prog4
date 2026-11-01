using Backend_Prog4.DTOs.Producto;
using Backend_Prog4.DTOs.Producto.Backend_Prog4.DTOs.Producto;
using Backend_Prog4.Enums;
using Backend_Prog4.Services;
using Backend_Prog4.Utils.Repuestas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Backend_Prog4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoServices _service;

        public ProductoController(ProductoServices service)
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
                RespuestaExito exito = new RespuestaExito("productos obtenidos!", HttpStatusCode.OK, todos);
                return StatusCode((int)exito.CodigoEstado, exito.Res);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta(error.Message));
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Respuesta>> ObtenerUnoPorId(int id)
        {
            try
            {
                var producto = await _service.ObtenerUnoPorId(id);
                RespuestaExito exito = new RespuestaExito("producto encontrado!", HttpStatusCode.OK, producto);
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
        public async Task<ActionResult<Respuesta>> CrearUno([FromBody] CrearProductoDTO crearDto)
        {
            try
            {
                var producto = await _service.CrearUno(crearDto);
                RespuestaExito exito = new RespuestaExito("Se creo correctamente!", HttpStatusCode.Created, producto);
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

        [HttpPut("{id}")]
        [Authorize(Roles = ROL.ADMIN)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Respuesta>> ActualizarUno(int id, [FromBody] ActualizarProductoDTO dto)
        {
            try
            {
                var producto = await _service.ActualizarUnoPorId(id, dto);
                RespuestaExito exito = new RespuestaExito("Se actualizo correctamente!", HttpStatusCode.OK, producto);
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

        [HttpDelete("{id}")]
        [Authorize(Roles = ROL.ADMIN)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Respuesta>> EliminarUno(int id)
        {
            try
            {
                await _service.EliminarUnoPorId(id);
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
