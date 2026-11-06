using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Backend_Prog4.Models
{
    public class PedidoItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        [ForeignKey(nameof(Pedido))]
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; } = null!;

        [ForeignKey(nameof(Producto))]
        public int ProductoId { get; set; }
        public Producto Producto { get; set; } = null!;

        [ForeignKey(nameof(Talle))]
        public int TalleId { get; set; }
        public Talle Talle { get; set; } = null!;
    }
}
