using System.ComponentModel.DataAnnotations;

namespace Backend_Prog4.DTOs.Producto
{
    public class CrearProductoTalleDTO
    {

        [Range(1, int.MaxValue)]
        public int TalleId { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
    }
}
