namespace Backend_Prog4.DTOs.Pedido
{
    public class ActualizarEstadoPedidoRespuestaDTO
    {

        public int Id { get; set; }
        public string EstadoAnterior { get; set; } = string.Empty;
        public string EstadoNuevo { get; set; } = string.Empty;
    }
}
