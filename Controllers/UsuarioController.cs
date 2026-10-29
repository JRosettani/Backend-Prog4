using System.Net;
using Backend_Prog4.Utils.Repuestas;
using Microsoft.AspNetCore.Mvc;
using Backend_Prog4.DTOs.Categoria;
using Backend_Prog4.Services;
using Microsoft.AspNetCore.Authorization;
using Backend_Prog4.Enums;
using Backend_Prog4.Dtos.Usuario;

namespace Backend_Prog4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuariosService _service;
        public UsuariosController(UsuariosService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = ROL.ADMIN)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        public ActionResult<Respuesta> ObtenerTodos()
        {
            try
            {
                var todos = _service.ObtenerTodos();
                RespuestaExito exito = new RespuestaExito(
                    "usuarios obtenidos!",
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
        [Authorize(Roles = ROL.ADMIN)]
        public ActionResult<Respuesta> ObtenerUnaPorId(int id)
        {
            try
            {
                var u = _service.ObtenerUnoPorId(id);
                RespuestaExito exito = new RespuestaExito(
                    "Categoria encontrada!",
                    HttpStatusCode.OK,
                    u
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

       

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        public ActionResult<Respuesta> ActualizarUna(int id, [FromBody] ActualizarUsuarioDTO dto)
        {
            try
            {
                var usu = _service.ActualizarUnoPorId(id, dto);
                RespuestaExito exito = new RespuestaExito(
                    "Se actualizo correctamente!",
                    HttpStatusCode.OK,
                    usu
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

        //[HttpDelete("{id}")]
        //[ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(Respuesta), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(Respuesta), StatusCodes.Status404NotFound)]
        //public ActionResult<Respuesta> EliminarUno(int id)
        //{
        //    try
        //    {
        //        _service.EliminarUnoPorId(id);
        //        RespuestaExito exito = new RespuestaExito(
        //            "Se elimino correctamente!",
        //            HttpStatusCode.OK
        //        );
        //        return StatusCode((int)exito.CodigoEstado, exito.Res);
        //    }
        //    catch (Exception error)
        //    {
        //        return StatusCode(
        //            StatusCodes.Status500InternalServerError,
        //            new Respuesta(error.Message)
        //        );
        //    }

        //}
    }
}