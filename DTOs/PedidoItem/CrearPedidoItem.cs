namespace Backend_Prog4.DTOs.PedidoItem
{
    public class CrearPedidoItem
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "999999999", ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal PrecioUnitario { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int PedidoId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int ProductoId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int TalleId { get; set; }
    }
}
