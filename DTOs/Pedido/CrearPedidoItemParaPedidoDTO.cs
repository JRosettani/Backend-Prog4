using System.ComponentModel.DataAnnotations;

namespace Backend_Prog4.DTOs.Pedido;


public class CrearPedidoItemParaPedidoDTO
{
    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
    public int Cantidad { get; set; }

    [Required(ErrorMessage = "El precio unitario es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor a 0.")]
    public decimal PrecioUnitario { get; set; }

    [Required(ErrorMessage = "El ID del producto es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID del producto debe ser válido.")]
    public int ProductoId { get; set; }

    [Required(ErrorMessage = "El ID del talle es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID del talle debe ser válido.")]
    public int TalleId { get; set; }
}