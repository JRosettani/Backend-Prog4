namespace Backend_Prog4.DTOs.Pedido
{
    public class CrearPedidoDTO
    {
        public int UsuarioId { get; set; }
        public List<CrearPedidoItemDTO> Items { get; set; } = new();
    }
}
