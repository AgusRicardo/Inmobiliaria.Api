using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models
{
    public class Contrato
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_contrato { get; set; }
        public int id_propietario { get; set; }
        [ForeignKey("id_propietario")]
        public virtual Propietario Propietario { get; set; }
        public int id_propiedad { get; set; }
        [ForeignKey("id_propiedad")]
        public virtual Propiedad Propiedad { get; set; }
        public int id_inquilino { get; set; }
        [ForeignKey("id_inquilino")]
        public virtual Inquilino Inquilino { get; set; }
        public int id_garante { get; set; }
        [ForeignKey("id_garante")]
        public virtual Garante Garante { get; set; }
        public DateOnly fecha_inicio { get; set; }
        public DateOnly fecha_fin { get; set; }
        public decimal monto { get; set; }
        public int id_estado { get; set; }
        [ForeignKey("id_estado")]
        public virtual Estados Estado { get; set; }
        public DateOnly fecha_alta { get; set; }
    }
}
