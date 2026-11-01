using System.ComponentModel.DataAnnotations;

namespace Backend_Prog4.DTOs.Producto
{
namespace Backend_Prog4.DTOs.Producto
    {
        public class ActualizarProductoDTO
        {
            [Required]
            [MinLength(2)]
            [MaxLength(100)]
            public string Nombre { get; set; } = string.Empty;

            [Required]
            [MaxLength(500)]
            public string Descripcion { get; set; } = string.Empty;

            [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
            public decimal Precio { get; set; }

            [Url]
            public string ImagenUrl { get; set; } = string.Empty;
        }
    }
}

