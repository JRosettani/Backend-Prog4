using Backend_Prog4.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Talle
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public List<ProductoTalle> ProductoTalles { get; set; } = new();
}