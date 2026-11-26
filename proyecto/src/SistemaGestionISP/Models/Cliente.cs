namespace SistemaGestionISP.Models
{
    /// <summary>
    /// Representa a un cliente abonado al servicio de Internet (Módulo 1).
    /// </summary>
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Cedula { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;

        /// <summary>Estado operativo del servicio; un cliente nuevo inicia como Activo.</summary>
        public EstadoServicio EstadoServicio { get; set; } = EstadoServicio.Activo;
    }
}
