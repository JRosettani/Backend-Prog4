namespace Backend_Prog4.DTOs.Pedido
{
    public class PedidoItemRespuestaDTO
    {
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public string TalleNombre { get; set; } = string.Empty;
    }
}
