using System.ComponentModel.DataAnnotations;
    
    namespace Backend_Prog4.DTOs.Producto
    {
        public class CrearProductoDTO
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

        [Range(1, int.MaxValue)]
        public int CategoriaId { get; set; }

        [Range(1, int.MaxValue)]
        public int MarcaId { get; set; }

        public List<CrearProductoTalleDTO> Talles { get; set; } = new();
    }
    }

