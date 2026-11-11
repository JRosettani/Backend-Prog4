namespace Backend_Prog4.DTOs.Pedido
{
    public class PedidoRespuestaDTO
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string EstadoNombre { get; set; } = string.Empty;
        public List<PedidoItemRespuestaDTO> Items { get; set; } = new();
    }

}
