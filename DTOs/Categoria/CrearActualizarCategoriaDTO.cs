using System.ComponentModel.DataAnnotations;

namespace Backend_Prog4.DTOs.Categoria
{
    public class CrearActualizarCategoriaDTO
    {
        [Required(ErrorMessage = "Debe tener obligatoriamente un nombre")]
        [MinLength(2, ErrorMessage = "Minimo 2 letras")]
        public string Nombre { get; set; } = string.Empty;

    }
}
