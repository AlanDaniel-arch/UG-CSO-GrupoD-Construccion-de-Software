namespace SistemaGestionISP.Models
{
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
}
