using System.Net;
using Backend_Prog4.Dtos.Usuario;
using Backend_Prog4.DTOs.Auth;
using Backend_Prog4.Services;
using Backend_Prog4.Utils.Repuestas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace panaderia.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    private readonly UsuariosService _usuariosServices;

    public AuthController(IAuthService auth,UsuariosService userService)
    {
        _auth = auth;
        _usuariosServices = userService;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(Respuesta), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Respuesta), StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public ActionResult<Respuesta> Login([FromBody] LoginDTO dto)
    {
        try
        {
            var response = _auth.Login(dto);
            RespuestaExito exito = new RespuestaExito(
            "Bienvenido!",
            HttpStatusCode.OK,
            response
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

    [HttpPost("register")]
    [ProducesResponseType(typeof(Respuesta), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Respuesta), StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public ActionResult<Respuesta> Register([FromBody] CrearUsuarioDTO dto)
    {
        try
        {
            var usuario =  _usuariosServices.CrearUno(dto);
            RespuestaExito exito = new RespuestaExito(
                "Usuario Creado!",
                HttpStatusCode.Created,
                usuario
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
}