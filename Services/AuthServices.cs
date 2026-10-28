using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Backend_Prog4.Utils.Repuestas;
using Microsoft.IdentityModel.Tokens;
using Backend_Prog4.DTOs.Auth;
using Backend_Prog4.DTOs.Usuario;
using Backend_Prog4.Models;

namespace Backend_Prog4.Services;

public class AuthService : IAuthService
{
    private readonly string _secret;

    private readonly UsuariosServices _usuarioService;
    public AuthService(IConfiguration config, UsuariosServices usuarioService)
    {
        string secret = config.GetSection("JWT:Secret").Value ?? string.Empty;
        _secret = secret;
        _usuarioService = usuarioService;
    }

    public LoginRespuestaDTO Login(LoginDTO login)
    {
        Usuario usuario = _usuarioService.ObtenerUnoPorNombreUsuario(login.Usuario);
        if (usuario.Clave != login.Clave)
        {
            throw new RespuestaError("Credenciales invalidas", HttpStatusCode.BadRequest);
        }

        string token = GenerarJwtToken(usuario);
        UsuarioDTO dto = new UsuarioDTO()
        {
            Id = usuario.Id,
            NombreUsuario = usuario.NombreUsuario,
            FechaAlta = usuario.FechaAlta
        };

        return new LoginRespuestaDTO()
        {
            Token = token,
            Usuario = dto
        };
    }

    private string GenerarJwtToken(Usuario usuario)
    {
        var claims = new ClaimsIdentity();

        claims.AddClaim(new Claim("Id", usuario.Id.ToString()));
        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.NombreUsuario));

        byte[] bytes = Encoding.UTF8.GetBytes(_secret);
        SymmetricSecurityKey key = new SymmetricSecurityKey(bytes);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
        string token = tokenHandler.WriteToken(tokenConfig);
        return token;
    }
}