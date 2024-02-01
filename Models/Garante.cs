using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models
{
    public class Garante
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_garante { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string dni { get; set; }
        public string garantia { get; set; }
        public DateOnly fecha_alta { get; set; }

        public virtual ICollection<Contrato> Contrato { get; set; }
    }
}
