using System.Net;
using System.Security.Claims;
using Backend_Prog4.Utils.Repuestas;
using Microsoft.AspNetCore.Mvc;
using Backend_Prog4.Services;
using Microsoft.AspNetCore.Authorization;
using Backend_Prog4.Enums;
using Backend_Prog4.Dtos.Usuario;

namespace Backend_Prog4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<ActionResult<Respuesta>> ObtenerTodos()
        {
            try
            {
                var todos = await _service.ObtenerTodos();
                RespuestaExito exito = new RespuestaExito("usuarios obtenidos!", HttpStatusCode.OK, todos);
                return StatusCode((int)exito.CodigoEstado, exito.Res);
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Respuesta(error.Message));
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = ROL.ADMIN)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Respuesta>> ObtenerUnoPorId(int id)
        {
            try
            {
                var u = await _service.ObtenerUnoPorId(id);
                RespuestaExito exito = new RespuestaExito("Usuario encontrado!", HttpStatusCode.OK, u);
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
        [Authorize(Roles = $"{ROL.ADMIN},{ROL.USER}")]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Respuesta>> ActualizarUno(int id, [FromBody] ActualizarUsuarioDTO dto)
        {
            try
            {
                // Si no es Admin, solo puede modificarse a sí mismo
                bool esAdmin = User.IsInRole(ROL.ADMIN);
                if (!esAdmin)
                {
                    var idDelToken = int.Parse(User.FindFirstValue("Id")!);
                    if (idDelToken != id)
                        throw new RespuestaError("No podés modificar otro usuario", HttpStatusCode.Forbidden);
                }

                var usu = await _service.ActualizarUnoPorId(id, dto);
                RespuestaExito exito = new RespuestaExito("Se actualizo correctamente!", HttpStatusCode.OK, usu);
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

        [HttpGet("perfil")]///api/usuarios/perfil
        [Authorize(Roles = $"{ROL.ADMIN},{ROL.USER}")]
        [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
        public async Task<ActionResult<Respuesta>> ObtenerPerfil()
        {
            try
            {

                //aca saca el id del token para que el usuario solo pueda acceder solo a su perfil , donde este id sera obtenido de la claim
                var idDelToken = int.Parse(User.FindFirstValue("Id")!);
                var u = await _service.ObtenerUnoPorId(idDelToken);
                RespuestaExito exito = new RespuestaExito("Perfil obtenido!", HttpStatusCode.OK, u);
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