using System;

namespace SistemaGestionISP.Models
{
    /// <summary>
    /// Acuerdo de servicio entre el ISP y un cliente (tabla <c>contratos</c>, Módulo 1).
    /// Cada contrato pertenece a un cliente y a un plan (RN-02).
    /// </summary>
    public class Contrato
    {
        public int IdContrato { get; set; }

        /// <summary>Código legible (CTR-AAAA-NNN); lo genera la base a partir del identificador.</summary>
        public string CodigoContrato { get; set; } = string.Empty;

        public int IdCliente { get; set; }
        public int IdPlan { get; set; }
        public DateTime FechaInicio { get; set; } = DateTime.Today;
        public EstadoContrato EstadoContrato { get; set; } = EstadoContrato.Activo;
    }
}
