<<<<<<< HEAD
﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
=======
﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
>>>>>>> 2f598e8ece983067f9efbe45b82bb3d8eb318360

namespace Backend_Prog4.Models
{
    public class PedidoItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
<<<<<<< HEAD
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
=======

        public int Id { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int PedidoId { get; set; }
        public int ProductoId { get; set; }
        public int TalleId { get; set; }
        public Pedido pedido { get; set; }
        public Producto producto { get; set; }
        public Talle talle { get; set; }


>>>>>>> 2f598e8ece983067f9efbe45b82bb3d8eb318360
    }
}
