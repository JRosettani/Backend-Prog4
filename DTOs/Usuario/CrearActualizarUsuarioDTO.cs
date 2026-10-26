using System.ComponentModel.DataAnnotations;

namespace Backend_Prog4.DTOs.Usuario
{
    public class CrearActualizarUsuarioDTO
    {
        [Required(ErrorMessage = "Debe tener obligatoriamente un nombre")]
        [MinLength(2, ErrorMessage = "Minimo 2 letras")]
        public string Nombre { get; set; } = string.Empty;
    }
}
