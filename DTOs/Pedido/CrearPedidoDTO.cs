using System.ComponentModel.DataAnnotations;

namespace Backend_Prog4.DTOs.Pedido
{
    public class CrearPedidoDTO
    {
        [Required(ErrorMessage = "Los items son requeridos")]
        [MinLength(1, ErrorMessage = "El pedido debe tener al menos un item")]
        public List<CrearPedidoItemDTO> Items { get; set; } = new();
    }
}
