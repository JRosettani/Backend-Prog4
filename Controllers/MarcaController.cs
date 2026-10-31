using System.Net;
using Backend_Prog4.Utils.Repuestas;
using Microsoft.AspNetCore.Mvc;
using Backend_Prog4.DTOs.Talle;
using Backend_Prog4.Services;
using Microsoft.AspNetCore.Authorization;
using Backend_Prog4.Enums;

namespace Backend_Prog4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class MarcaController : ControllerBase
    {
        private readonly MarcaServices _service;
        public MarcaController(MarcaServices service)
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
                RespuestaExito exito = new RespuestaExito("marcas obtenidas!", HttpStatusCode.OK, todos);
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
        public async Task<ActionResult<Respuesta>> ObtenerUnaPorId(int id)
        {
            try
            {
                var marca = await _service.ObtenerUnoPorId(id);
                RespuestaExito exito = new RespuestaExito("marca encontrado!", HttpStatusCode.OK, marca);
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
        public async Task<ActionResult<Respuesta>> CrearUna([FromBody] CrearActualizarMarcaDTO crearDto)
        {
            try
            {
                var marca = await _service.CrearUno(crearDto);
                RespuestaExito exito = new RespuestaExito("Se creo correctamente!", HttpStatusCode.Created, marca);
                return StatusCode((int)exito.CodigoEstado, exito.Res);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta(error.Message));
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = ROL.ADMIN)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        public async Task<ActionResult<Respuesta>> ActualizarUna(int id, [FromBody] CrearActualizarMarcaDTO dto)
        {
            try
            {
                var marca = await _service.ActualizarUnoPorId(id, dto);
                RespuestaExito exito = new RespuestaExito("Se actualizo correctamente!", HttpStatusCode.OK, marca);
                return StatusCode((int)exito.CodigoEstado, exito.Res);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta(error.Message));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = ROL.ADMIN)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        public async Task<ActionResult<Respuesta>> EliminarUno(int id)
        {
            try
            {
                await _service.EliminarUnoPorId(id);
                RespuestaExito exito = new RespuestaExito("Se elimino correctamente!", HttpStatusCode.OK);
                return StatusCode((int)exito.CodigoEstado, exito.Res);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta(error.Message));
            }
        }
    }
}