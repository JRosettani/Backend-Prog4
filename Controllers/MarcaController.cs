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
    public class MarcaController : ControllerBase
    {
        private readonly MarcaServices _service;
        public MarcaController(MarcaServices service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = ROL.USER)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        public ActionResult<Respuesta> ObtenerTodos()
        {
            try
            {
                var todos = _service.ObtenerTodos();
                RespuestaExito exito = new RespuestaExito(
                    "marcas obtenidas!",
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
                    "marca encontrado!",
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
        public ActionResult<Respuesta> CrearUna([FromBody] CrearActualizarMarcaDTO crearDto)
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
        public ActionResult<Respuesta> ActualizarUna(int id, [FromBody] CrearActualizarMarcaDTO dto)
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