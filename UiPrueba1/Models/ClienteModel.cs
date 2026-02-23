namespace UiPrueba1.Models
{
    public class ClienteModel
    {
        public string ClienteId { get; set; } = string.Empty;
        public string TipoId { get; set; } = "Fisico";
        public string Nombre { get; set; } = string.Empty;
        public bool EsReceptor { get; set; }
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Provincia { get; set; }
        public string? Canton { get; set; }
        public string? Distrito { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string CodActividad { get; set; } = string.Empty;
        public string DescActividad { get; set; } = string.Empty;
    }
}
