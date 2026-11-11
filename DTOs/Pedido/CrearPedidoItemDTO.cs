using System.ComponentModel.DataAnnotations;

namespace Backend_Prog4.DTOs.Pedido
{
    public class CrearPedidoItemDTO
    {
        [Required(ErrorMessage = "El producto es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El ProductoId debe ser un ID válido")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "El talle es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El TalleId debe ser un ID válido")]
        public int TalleId { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero")]
        public int Cantidad { get; set; }

    }
}
