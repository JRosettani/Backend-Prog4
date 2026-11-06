using System.ComponentModel.DataAnnotations;

namespace Backend_Prog4.DTOs.Producto
{

        public class ActualizarProductoDTO
        {
            [MinLength(2)]
            [MaxLength(100)]
            public string? Nombre { get; set; } 

            [MaxLength(500)]
            public string? Descripcion { get; set; }

            [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
            public decimal? Precio { get; set; }

            [Url]
            public string? ImagenUrl { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría válida.")]
            public int? CategoriaId { get; set; } // Opcional por si también quieren cambiar la categoría

            [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una marca válida.")]
            public int? MarcaId { get; set; } // Opcional por si también quieren cambiar la marca

    }
}

