namespace Backend_Prog4.DTOs.Producto
{
    public class ProductoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string ImagenUrl { get; set; } = string.Empty;
        public int MarcaId { get; set; }
        public int CategoriaId { get; set; }

        // Capaz que nos sirve como DTO de salida, sino lo sacamos.
    }
}
