using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Request
{
    public class CrearPersonaRequest
    {
        [StringLength(50)]
        public string? Nombre { get; set; }

        [StringLength(50)]
        public string? Apellido { get; set; }

        [StringLength(20)]
        public string? Dni { get; set; }
    }
}
