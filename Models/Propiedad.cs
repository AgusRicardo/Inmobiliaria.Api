using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models
{
    public class Propiedad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_propiedad { get; set; }
        public int id_propietario { get; set; }
        [ForeignKey("id_propietario")]
        public virtual Propietario Propietario { get; set; }
        public string tipo { get; set; }
        public string direccion { get; set; }
        public DateOnly fecha_alta { get; set; }

    }
}
