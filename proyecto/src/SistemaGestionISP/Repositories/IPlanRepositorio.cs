using System.Collections.Generic;
using SistemaGestionISP.Models;

namespace SistemaGestionISP.Repositories
{
    /// <summary>Operaciones de persistencia del catálogo de planes (Módulo 2).</summary>
    public interface IPlanRepositorio
    {
        /// <summary>Devuelve los planes de Internet disponibles.</summary>
        List<PlanInternet> Listar();
    }
}
