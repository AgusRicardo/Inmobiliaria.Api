using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models
{
    public partial class Usuario
    {
        [Key]
        public int User_id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Rol { get; set; }
    }
}
