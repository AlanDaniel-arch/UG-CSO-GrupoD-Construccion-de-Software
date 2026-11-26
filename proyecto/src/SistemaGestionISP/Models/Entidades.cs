using System;

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
        public string EstadoServicio { get; set; } = "Activo";
    }

    /// <summary>
    /// Representa un plan comercial de velocidad y tarifa mensual (Módulo 2).
    /// </summary>
    public class PlanInternet
    {
        public int IdPlan { get; set; }
        public string NombrePlan { get; set; } = string.Empty;
        public int VelocidadMbps { get; set; }
        public decimal PrecioMensual { get; set; }
    }

    /// <summary>
    /// Representa un comprobante de cobro asociado a un contrato (Módulo 3).
    /// </summary>
    public class Factura
    {
        public int IdFactura { get; set; }
        public int IdContrato { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaEmision { get; set; }
        public bool Pagada { get; set; }
    }

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