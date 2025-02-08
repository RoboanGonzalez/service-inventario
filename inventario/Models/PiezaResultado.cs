namespace inventario.Models
{
    public class PiezaResultado
    {
        public int ID { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int AnioAuto { get; set; }
        public string Motor { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public DateTime FechaIngreso { get; set; }
        public int PROVEEDORID { get; set; }

    }
}
