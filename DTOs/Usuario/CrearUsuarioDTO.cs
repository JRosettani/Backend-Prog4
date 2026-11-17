using Backend_Prog4.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Backend_Prog4.Dtos.Usuario;

public class CrearUsuarioDTO
{
    [Required]
    [MinLength(2)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MinLength(2)]
    public string Apellido { get; set; } = string.Empty;

    [Required]
    [MinLength(4)]
    public string Clave { get; set; } = string.Empty;


    [Required]
    [MinLength(4)]
    public string Email { get; set; } = string.Empty;
    public List<Rol> Roles { get; set; } = null!;

}