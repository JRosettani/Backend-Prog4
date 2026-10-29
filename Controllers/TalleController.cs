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
    [Authorize(Roles = ROL.ADMIN)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class TalleController : ControllerBase
    {
        private readonly TallesService _service;
        public TalleController(TallesService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        [Authorize(Roles = ROL.USER)]
        public ActionResult<Respuesta> ObtenerTodos()
        {
            try
            {
                var todos = _service.ObtenerTodos();
                RespuestaExito exito = new RespuestaExito(
                    "Talles obtenidos!",
                    HttpStatusCode.OK,
                    todos
                );

                return StatusCode((int)exito.CodigoEstado, exito.Res);
            }
            catch (Exception error)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new Respuesta(error.Message)
                );
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status404NotFound)]
        [Authorize(Roles = ROL.USER)]
        public ActionResult<Respuesta> ObtenerUnaPorId(int id)
        {
            try
            {
                var talle = _service.ObtenerUnoPorId(id);
                RespuestaExito exito = new RespuestaExito(
                    "Talle encontrado!",
                    HttpStatusCode.OK,
                    talle
                );

                return StatusCode((int)exito.CodigoEstado, exito.Res);
            }
            catch (RespuestaError error)
            {
                return StatusCode(
                    (int)error.CodigoEstado,
                    error.Res
                );
            }
            catch (Exception error)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new Respuesta(error.Message)
                );
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status201Created)]
        public ActionResult<Respuesta> CrearUna([FromBody] CrearActualizarTalleDTO crearDto)
        {
            try
            {
                var talle = _service.CrearUno(crearDto);
                RespuestaExito exito = new RespuestaExito(
                    "Se creo correctamente!",
                    HttpStatusCode.Created,
                    talle
                );

                return StatusCode((int)exito.CodigoEstado, exito.Res);
            }
            catch (Exception error)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new Respuesta(error.Message)
                );
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        public ActionResult<Respuesta> ActualizarUna(int id, [FromBody] CrearActualizarTalleDTO dto)
        {
            try
            {
                var t = _service.ActualizarUnoPorId(id, dto);
                RespuestaExito exito = new RespuestaExito(
                    "Se actualizo correctamente!",
                    HttpStatusCode.OK,
                    t
                );
                return StatusCode((int)exito.CodigoEstado, exito.Res);
            }
            catch (Exception error)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new Respuesta(error.Message)
                );
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        public ActionResult<Respuesta> EliminarUno(int id)
        {
            try
            {
                _service.EliminarUnoPorId(id);
                RespuestaExito exito = new RespuestaExito(
                    "Se elimino correctamente!",
                    HttpStatusCode.OK
                );
                return StatusCode((int)exito.CodigoEstado, exito.Res);
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