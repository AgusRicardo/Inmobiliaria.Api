using System;
using System.Collections.Generic;

namespace Inmobiliaria.Models;

public partial class Gasto
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public string? Estado { get; set; }

    public DateTime? Fecha { get; set; }
}
