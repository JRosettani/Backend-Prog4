using System.Net;
using Backend_Prog4.Utils.Repuestas;
using Microsoft.AspNetCore.Mvc;
using Backend_Prog4.DTOs.Categoria;
using Backend_Prog4.Services;
using Microsoft.AspNetCore.Authorization;
using Backend_Prog4.Enums;

namespace Backend_Prog4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class CategoriasController : ControllerBase
    {
        private readonly CategoriasService _service;
        public CategoriasController(CategoriasService service)
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
                RespuestaExito exito = new RespuestaExito("Categorias obtenidas!", HttpStatusCode.OK, todos);
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
                var cat = await _service.ObtenerUnoPorId(id);
                RespuestaExito exito = new RespuestaExito("Categoria encontrada!", HttpStatusCode.OK, cat);
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
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status201Created)]
        [Authorize(Roles = ROL.ADMIN)]
        public async Task<ActionResult<Respuesta>> CrearUna([FromBody] CrearActualizarCategoriaDTO crearDto)
        {
            try
            {
                var cat = await _service.CrearUno(crearDto);
                RespuestaExito exito = new RespuestaExito("Se creo correctamente!", HttpStatusCode.Created, cat);
                return StatusCode((int)exito.CodigoEstado, exito.Res);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta(error.Message));
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        [Authorize(Roles = ROL.ADMIN)]

        public async Task<ActionResult<Respuesta>> ActualizarUna(int id, [FromBody] CrearActualizarCategoriaDTO dto)
        {
            try
            {
                var cat = await _service.ActualizarUnoPorId(id, dto);
                RespuestaExito exito = new RespuestaExito("Se actualizo correctamente!", HttpStatusCode.OK, cat);
                return StatusCode((int)exito.CodigoEstado, exito.Res);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta(error.Message));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status404NotFound)]
        [Authorize(Roles = ROL.ADMIN)]

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