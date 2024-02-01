using Inmobiliaria.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inmobiliaria.Request
{
    public class CrearPropiedadesRequest
    {
        public int id_propietario { get; set; }
        [StringLength(50)]
        public string tipo { get; set; }
        [StringLength(50)]
        public string direccion { get; set; }
        public DateOnly fecha_alta { get; set; }
    }
}
