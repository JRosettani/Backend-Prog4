namespace Backend_Prog4.DTOs.Paginado
{
    public class PaginadoDTO<T>
    {
        public List<T> Items { get; set; } = new();
        public int Total { get; set; }
        public int Pagina { get; set; }
        public int TamanioPagina { get; set; }
        public int TotalPaginas => TamanioPagina == 0 ? 0 : (int)Math.Ceiling(Total / (double)TamanioPagina);
    }
}
