using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_Prog4.Models
{
    public class Pedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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
    }
}