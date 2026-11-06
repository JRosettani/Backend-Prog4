using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_Prog4.Models
{
    public class Pedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
<<<<<<< HEAD

        public DateTime Fecha { get; set; }

        [ForeignKey(nameof(EstadoPedido))]
        public int IdEstado { get; set; }
        public EstadoPedido EstadoPedido { get; set; } = null!;

        public decimal Total { get; set; }

        [ForeignKey(nameof(Usuario))]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

         public List<PedidoItem> PedidoItems { get; set; } = new();
=======

        public DateTime Fecha { get; set; } = DateTime.Now; 

        public decimal Total { get; set; }

        // Clave foránea explícita para EstadoPedido
        public int IdEstado { get; set; }

        [ForeignKey("IdEstado")] // Vinculamos explícitamente la FK
        public EstadoPedido EstadoPedido { get; set; } = null!;

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        // Descomentado: Relación de uno a muchos con los ítems del pedido
        public List<PedidoItem> PedidoItems { get; set; } = new();
>>>>>>> 2f598e8ece983067f9efbe45b82bb3d8eb318360
    }
}