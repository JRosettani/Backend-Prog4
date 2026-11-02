using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_Prog4.Models
{
    public class PedidoItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int PedidoId { get; set; }
        public int ProductoId { get; set; }
        public int TalleId { get; set; }
        public Pedido pedido { get; set; }
        public Producto producto { get; set; }
        public Talle talle { get; set; }


    }
}
