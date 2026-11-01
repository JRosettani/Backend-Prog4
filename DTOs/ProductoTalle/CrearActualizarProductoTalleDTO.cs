namespace Backend_Prog4.DTOs.ProductoTalle
{
    using Backend_Prog4.Models;
    using System.ComponentModel.DataAnnotations;

    public class CrearActualizarProductoTalleDTO
    {
        [Range(1, int.MaxValue)]
        public int ProductoId { get; set; }

        // en los ids se deben mostrar desde el frontend mas que nada para que quede mucho mas limpio y no se tenga que poner cualquier id

        [Range(1, int.MaxValue)]
        public int TalleId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }
    }
}
