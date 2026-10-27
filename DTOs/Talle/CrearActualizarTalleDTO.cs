using System.ComponentModel.DataAnnotations;

namespace Backend_Prog4.DTOs.Talle
{
    public class CrearActualizarTalleDTO
    {
        [Required(ErrorMessage = "Debe tener obligatoriamente un nombre")]
        [MinLength(1, ErrorMessage = "Minimo 1 letra")]
        public string Nombre { get; set; } = string.Empty;
    }
}
