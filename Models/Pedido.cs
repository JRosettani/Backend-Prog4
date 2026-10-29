using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_Prog4.Models
{
    public class Pedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Fecha { get; set; }

        public int IdEstado { get; set; }
        public EstadoPedido EstadoPedido { get; set; } = null!;
        public decimal Total { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

          //public List<PedidoItem> PedidoItems { get; set; } = new();
    }
}
