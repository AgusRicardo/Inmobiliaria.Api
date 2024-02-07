using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inmobiliaria.Models
{
    public partial class Usuario
    {
        [Key]
        public int User_id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string id_rol { get; set; }
        [ForeignKey("id_rol")]
        public virtual Roles Rol { get; set; }
        public int inmobiliaria_id { get; set; }
        [ForeignKey("inmobiliaria_id")]
        public virtual Inmobiliarias Inmobiliaria { get; set; }
    }
}
