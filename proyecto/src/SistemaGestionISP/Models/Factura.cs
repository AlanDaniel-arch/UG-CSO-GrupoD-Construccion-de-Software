using System;

namespace SistemaGestionISP.Models
{
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
}
