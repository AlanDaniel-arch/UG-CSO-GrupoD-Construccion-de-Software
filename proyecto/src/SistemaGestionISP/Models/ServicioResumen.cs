using System;

namespace SistemaGestionISP.Models
{
    /// <summary>
    /// Fila del listado de servicios (RF-19): estado del servicio de cada contrato con el
    /// nombre del cliente. Es una vista de lectura de las tablas <c>servicios</c>,
    /// <c>contratos</c> y <c>clientes</c>.
    /// </summary>
    public class ServicioResumen
    {
        public int IdServicio { get; set; }
        public string CodigoServicio { get; set; } = string.Empty;
        public string NombreCliente { get; set; } = string.Empty;
        public string CodigoContrato { get; set; } = string.Empty;
        public EstadoServicio EstadoServicio { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
