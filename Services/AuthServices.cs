using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Backend_Prog4.DTOs.Auth;
using Backend_Prog4.DTOs.Usuario;
using Backend_Prog4.Models;
using Backend_Prog4.Services;
using Backend_Prog4.Utils.Repuestas;
using Microsoft.IdentityModel.Tokens;

namespace Backend_Prog4.Services;

public class AuthServices : IAuthService
{
    private readonly string _secret;
    private readonly UsuariosService _usuarioService;
    private readonly IEncoderServices _encoder;

    public AuthServices(IConfiguration config, UsuariosService usuarioService, IEncoderServices encoder)
    {
        _secret = config.GetSection("JWT:Secret").Value ?? string.Empty;
        _usuarioService = usuarioService;
        _encoder = encoder;
    }

    public async Task<LoginRespuestaDTO> Login(LoginDTO login)
    {
        Usuario usuario;
        try
        {
            usuario = await _usuarioService.ObtenerUnoPorEmail(login.Email);
        }
        catch
        {
            throw new RespuestaError("Credenciales invalidas", HttpStatusCode.BadRequest);
        }

        bool isVerified = _encoder.Verify(login.Password, usuario.PasswordHash);
        if (!isVerified)
            throw new RespuestaError("Credenciales invalidas", HttpStatusCode.BadRequest);

        string token = GenerarJwtToken(usuario);

        UsuarioDTO dto = new UsuarioDTO()
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Roles = usuario.Roles
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
        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, usuario.Email));

        foreach (var rol in usuario.Roles)
            claims.AddClaim(new Claim(ClaimTypes.Role, rol.Nombre));

        byte[] bytes = Encoding.UTF8.GetBytes(_secret);
        var key = new SymmetricSecurityKey(bytes);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(tokenConfig);
    }
}