namespace Inmobiliaria.Request
{
    public class CrearGaranteRequest { 
    
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string dni { get; set; }
        public string garantia { get; set; }
        public DateOnly fecha_alta { get; set; }
    }
}
