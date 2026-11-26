namespace SistemaGestionISP.Models
{
    /// <summary>
    /// Representa una incidencia técnica reportada por un cliente (Módulo 4).
    /// </summary>
    public class TicketSoporte
    {
        public int IdTicket { get; set; }
        public int IdCliente { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string Estado { get; set; } = "Abierto";
    }
}
