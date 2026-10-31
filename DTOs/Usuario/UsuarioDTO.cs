using Backend_Prog4.Models;

namespace Backend_Prog4.DTOs.Usuario;

public class UsuarioDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;

    public string Apellido { get; set; } = string.Empty;   
    public string Email { get; set; } = string.Empty;

    public List<Rol> Roles { get; set; } = null!;

}