using System.Collections.Generic;
using SistemaGestionISP.Models;

namespace SistemaGestionISP.Repositories
{
    /// <summary>Persistencia del estado del servicio (Módulo 4 · RF-19, RF-20).</summary>
    public interface IServicioRepositorio
    {
        /// <summary>RF-19: lista el estado del servicio de cada contrato.</summary>
        List<ServicioResumen> Listar();

        /// <summary>
        /// RF-20 y RN-07: cambia el estado del servicio y actualiza el estado de su contrato en
        /// la misma transacción. Devuelve false si el servicio no existe.
        /// </summary>
        bool CambiarEstado(int idServicio, EstadoServicio nuevoEstado);
    }
}
