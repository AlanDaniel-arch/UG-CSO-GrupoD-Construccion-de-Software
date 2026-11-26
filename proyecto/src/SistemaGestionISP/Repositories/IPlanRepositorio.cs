using System.Collections.Generic;
using SistemaGestionISP.Models;

namespace SistemaGestionISP.Repositories
{
    /// <summary>Persistencia del catálogo de planes (Módulo 2 · RF-09, consulta).</summary>
    public interface IPlanRepositorio
    {
        /// <summary>Devuelve todos los planes de Internet, activos e inactivos.</summary>
        List<PlanInternet> Listar();
    }
}
