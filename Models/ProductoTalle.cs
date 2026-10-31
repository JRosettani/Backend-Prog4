namespace Backend_Prog4.Models
{
    public class ProductoTalle
    {
        public int ProductoId { get; set; }

        public int TalleId { get; set; }

        public int Stock { get; set; }

        public Producto Producto { get; set; } = null!;
        public Talle Talle { get; set; } = null!;

    }
}
