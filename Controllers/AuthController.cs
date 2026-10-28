using System.Net;
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

    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("login")]
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
        catch (Exception error)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new Respuesta(error.Message)
            );
        }
    }
}