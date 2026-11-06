using System.ComponentModel.DataAnnotations;

namespace Backend_Prog4.DTOs.PedidoItem
{
    public class ActualizarPedidoItem
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int? Cantidad { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "999999999", ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal? PrecioUnitario { get; set; }

    }
}
