namespace inventario.Models
{
    public class Piezas
    {
        public string? operacion { get; set; }
        public int? Id { get; set; }
        public string? Codigo { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? Marca { get; set; }
        public string? Modelo { get; set; }
        public int? Anio { get; set; }
        public string? Motor { get; set; }
        public decimal? Precio { get; set; }
        public int? Stock { get; set; }
        public int? ProveedorId { get; set; }


    }
}
