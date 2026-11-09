
using System.ComponentModel.DataAnnotations;

namespace Backend_Prog4.DTOs.Pedido
{
    public class ActualizarPedidoDTO
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int IdEstado { get; set; }
    }
}
