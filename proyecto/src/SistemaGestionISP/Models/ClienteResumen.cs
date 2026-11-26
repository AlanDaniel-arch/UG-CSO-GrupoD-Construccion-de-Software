namespace SistemaGestionISP.Models
{
    /// <summary>
    /// Fila del listado de clientes (RF-05): datos del cliente con el plan y el estado de su
    /// contrato. Es una vista de lectura, no una tabla de la base de datos.
    /// </summary>
    public class ClienteResumen
    {
        public int IdCliente { get; set; }
        public string Identificacion { get; set; } = string.Empty;
        public string NombresApellidos { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string CodigoContrato { get; set; } = "Sin contrato";
        public string NombrePlan { get; set; } = "—";
        public string EstadoContrato { get; set; } = "—";
    }
}
