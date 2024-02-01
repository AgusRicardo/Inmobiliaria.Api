namespace Inmobiliaria.Request
{
    public class EditarContratoRequest
    {
        public int id_propietario { get; set; }
        public int id_propiedad { get; set; }
        public int id_inquilino { get; set; }
        public int id_garante { get; set; }
        public DateOnly fecha_inicio { get; set; }
        public DateOnly fecha_fin { get; set; }
        public decimal monto { get; set; }
    }
}
