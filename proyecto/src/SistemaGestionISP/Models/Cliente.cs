using System;

namespace SistemaGestionISP.Models
{
    /// <summary>Abonado del ISP (tabla <c>clientes</c>, Módulo 1). Diccionario de datos: V2, Anexo B.</summary>
    public class Cliente
    {
        public int IdCliente { get; set; }

        /// <summary>Cédula o RUC; es único (RN-01).</summary>
        public string Identificacion { get; set; } = string.Empty;

        public string NombresApellidos { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; } = DateTime.Today;
    }
}
