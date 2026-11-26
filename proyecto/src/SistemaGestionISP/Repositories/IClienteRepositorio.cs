using System.Collections.Generic;
using SistemaGestionISP.Models;

namespace SistemaGestionISP.Repositories
{
    /// <summary>Operaciones de persistencia de clientes y de su estado de servicio (Módulos 1 y 4).</summary>
    public interface IClienteRepositorio
    {
        /// <summary>Inserta un cliente nuevo. Devuelve true si se guardó.</summary>
        bool Registrar(Cliente cliente);

        /// <summary>Devuelve todos los clientes registrados.</summary>
        List<Cliente> Listar();

        /// <summary>Cambia el estado del servicio de un cliente. Devuelve false si el cliente no existe.</summary>
        bool ActualizarEstadoServicio(int idCliente, EstadoServicio nuevoEstado);
    }
}
