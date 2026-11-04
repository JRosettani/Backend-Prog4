using System.ComponentModel.DataAnnotations;

namespace Backend_Prog4.DTOs.Pedido;


public class CrearPedidoDTO
{
    [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID del usuario debe ser válido.")]
    public int UsuarioId { get; set; }

    [Required(ErrorMessage = "El ID del estado inicial es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID del estado debe ser válido.")]
    public int IdEstado { get; set; }

    [Required(ErrorMessage = "El pedido debe contener al menos un ítem.")]
    [MinLength(1, ErrorMessage = "Debe incluir al menos un producto en el pedido.")]
    public List<CrearPedidoItemParaPedidoDTO> Items { get; set; } = new();
}