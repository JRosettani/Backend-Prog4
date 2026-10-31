using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_Prog4.Models
{
   public class Producto
    {
       [Key]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public int Id { get; set; }
       public string Nombre { get; set; } = string.Empty;
       public string Descripcion { get; set; } = string.Empty;
       public decimal Precio { get; set; }
       public string ImagenUrl { get; set; } = string.Empty;

       // Clave foránea hacia Categoria
       //[ForeignKey(nameof(Categoria))]
       public int CategoriaId { get; set; }

       public Categoria Categoria { get; set; } = null!;
       // clave foranea a marca

        //[ForeignKey(nameof(Marca))]
        public int MarcaId { get; set; }

        public Marca Marca { get; set; } = null!;

       //Lista de ítems de pedido donde aparece este producto
      // public List<PedidoItem> Items { get; set; } = new();

      // public List<ProductoTalle> Talles { get; set; } = new();

    }
}