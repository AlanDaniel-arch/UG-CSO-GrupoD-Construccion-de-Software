using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SistemaGestionISP.Data;
using SistemaGestionISP.Models;

namespace SistemaGestionISP.Repositories
{
    /// <summary>Acceso a datos de la tabla <c>planes_internet</c> (Módulo 2).</summary>
    public class PlanRepositorio : IPlanRepositorio
    {
        private readonly IConexionBD db;

        /// <param name="db">Proveedor de conexiones a la base de datos.</param>
        public PlanRepositorio(IConexionBD db)
        {
            this.db = db;
        }

        /// <summary>Módulo 2: consulta el catálogo de planes de Internet disponibles.</summary>
        public List<PlanInternet> Listar()
        {
            var planes = new List<PlanInternet>();
            using var conn = db.ObtenerConexion();
            conn.Open();
            string sql = "SELECT id_plan, nombre_plan, velocidad_mbps, precio_mensual FROM planes_internet";

            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                planes.Add(new PlanInternet
                {
                    IdPlan = reader.GetInt32("id_plan"),
                    NombrePlan = reader.GetString("nombre_plan"),
                    VelocidadMbps = reader.GetInt32("velocidad_mbps"),
                    PrecioMensual = reader.GetDecimal("precio_mensual")
                });
            }
            return planes;
        }
    }
}
