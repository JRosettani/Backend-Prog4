
using System.ComponentModel.DataAnnotations;

namespace Backend_Prog4.DTOs.Pedido
{
    public class ActualizarPedidoDTO
    {
        [Required(ErrorMessage = "El estado es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El estado debe ser un ID válido")]
        public int IdEstado { get; set; }
    }
}
