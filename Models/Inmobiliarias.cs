using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inmobiliaria.Models
{
    public class Inmobiliarias
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int inmobiliaria_id { get; set; }
        public string nombre { get; set; }
        public string? direccion { get; set; }
        public string? email { get; set; }
    }
}
