namespace SistemaGestionISP.Models
{
    /// <summary>Plan comercial de Internet (tabla <c>planes</c>, Módulo 2).</summary>
    public class PlanInternet
    {
        public int IdPlan { get; set; }
        public string CodigoPlan { get; set; } = string.Empty;
        public string NombrePlan { get; set; } = string.Empty;

        /// <summary>Velocidad como texto, tal como la guarda la base (por ejemplo «100 Mbps»).</summary>
        public string Velocidad { get; set; } = string.Empty;

        public decimal PrecioMensual { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public EstadoPlan Estado { get; set; } = EstadoPlan.Activo;
    }
}
