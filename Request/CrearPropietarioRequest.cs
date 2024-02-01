using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Request
{
    public class CrearPropietarioRequest
    {
        [StringLength(50)]
        public string nombre { get; set; }

        [StringLength(50)]
        public string apellido { get; set; }

        [StringLength(20)]
        public string dni { get; set; }

        public DateOnly fecha_alta { get; set; }
    }
}
