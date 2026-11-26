namespace SistemaGestionISP.Models
{
    /// <summary>
    /// Estado operativo del servicio de un contrato (tabla <c>servicios</c>, columna
    /// <c>estado_servicio</c>). Los nombres coinciden con el ENUM de la base de datos.
    /// </summary>
    public enum EstadoServicio
    {
        Activo,
        Suspendido,
        Pendiente
    }

    /// <summary>Reglas de negocio que dependen del estado del servicio.</summary>
    public static class EstadoServicioExtensiones
    {
        /// <summary>
        /// RN-07: al cambiar el estado del servicio, el contrato se actualiza en consecuencia.
        /// Servicio Activo → contrato Activo; cualquier otro estado → contrato Suspendido
        /// (mismo comportamiento que el prototipo web, <c>forms.js</c>).
        /// </summary>
        public static EstadoContrato AEstadoContrato(this EstadoServicio estado)
        {
            return estado == EstadoServicio.Activo ? EstadoContrato.Activo : EstadoContrato.Suspendido;
        }
    }
}
