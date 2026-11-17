using Backend_Prog4.DTOs.Usuario;

namespace Backend_Prog4.DTOs.Auth;

public class LoginRespuestaDTO
{
    public string Token { get; set; } = string.Empty;
    public UsuarioDTO Usuario { get; set; } = null!;
}