using System.ComponentModel.DataAnnotations;

namespace Backend_Prog4.DTOs.Auth;

public class LoginDTO
{
    [Required(ErrorMessage = "Debe ser obligatorio el correo electronico")]
    [EmailAddress(ErrorMessage = "El correo electronico no tiene un formato valido")]

    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debe ser obligatoria la contraseña")]

    public string Password { get; set; } = string.Empty;
}