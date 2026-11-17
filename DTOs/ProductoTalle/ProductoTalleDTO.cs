namespace Backend_Prog4.DTOs.ProductoTalle
{
    public class ProductoTalleDTO
    {
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public int TalleId { get; set; }
        public string TalleNombre { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}