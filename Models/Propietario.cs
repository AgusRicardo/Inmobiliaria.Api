﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inmobiliaria.Models
{
    public class Propietario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_propietario { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string dni { get; set; }
        public DateOnly fecha_alta { get; set; }
        public int inmobiliaria_id { get; set; }
        [ForeignKey("inmobiliaria_id")]
        public virtual Inmobiliarias Inmobiliaria { get; set; }

        public virtual ICollection<Propiedad> Propiedades { get; set; }

    }
}
