namespace SistemaGestionISP.Models
{
    /// <summary>
    /// Estados operativos posibles del servicio de un cliente (Módulo 4).
    /// Los nombres coinciden con los valores del ENUM de la columna
    /// <c>clientes.estado_servicio</c> en <c>database/isp_gestion.sql</c>.
    /// </summary>
    public enum EstadoServicio
    {
        Activo,
        Suspendido,
        Cancelado
    }
}
