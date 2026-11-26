using System.Collections.Generic;
using SistemaGestionISP.Models;

namespace SistemaGestionISP.Repositories
{
    /// <summary>Persistencia de clientes y contratos (Módulo 1 · RF-04, RF-05).</summary>
    public interface IClienteRepositorio
    {
        /// <summary>
        /// RF-04: registra un cliente junto con su contrato y el servicio asociado, todo en una
        /// sola transacción (si algo falla no queda nada a medias). Devuelve el código del contrato creado.
        /// </summary>
        /// <exception cref="MySql.Data.MySqlClient.MySqlException">
        /// Identificación repetida (RN-01, error 1062) o plan inexistente (clave foránea).
        /// </exception>
        string RegistrarConContrato(Cliente cliente, int idPlan);

        /// <summary>RF-05: lista los clientes con su plan y el estado de su contrato.</summary>
        List<ClienteResumen> Listar();
    }
}
