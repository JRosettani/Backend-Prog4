using System.ComponentModel.DataAnnotations;

namespace Backend_Prog4.Dtos.Usuario;

public class ActualizarUsuarioDTO
{
    [MinLength(2)]
    public string? Nombre { get; set; }

    [MinLength(2)]
    public string? Apellido { get; set; }

    [MinLength(4)]
    public string? Clave { get; set; }

    [EmailAddress(ErrorMessage = "El correo electronico no tiene un formato valido")]
    public string? Email { get; set; }

    //public List<int>? RolesIds { get; set; } no queremos que cambie su rol
}