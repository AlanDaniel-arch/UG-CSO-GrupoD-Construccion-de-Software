using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SistemaGestionISP.Data;
using SistemaGestionISP.Models;

namespace SistemaGestionISP.Repositories
{
    /// <summary>Implementación MySQL de <see cref="IPlanRepositorio"/> (tabla planes).</summary>
    public class PlanRepositorioMySql : IPlanRepositorio
    {
        private readonly IConexionBD db;

        /// <param name="db">Proveedor de conexiones a la base de datos.</param>
        public PlanRepositorioMySql(IConexionBD db)
        {
            this.db = db;
        }

        /// <inheritdoc />
        public List<PlanInternet> Listar()
        {
            var planes = new List<PlanInternet>();
            using var conn = db.ObtenerConexion();
            conn.Open();

            using var cmd = AyudanteSql.Crear(conn, null,
                @"SELECT id_plan, codigo_plan, nombre_plan, velocidad, precio_mensual, descripcion, estado
                  FROM planes ORDER BY id_plan");
            using var lector = cmd.ExecuteReader();

            while (lector.Read())
            {
                planes.Add(new PlanInternet
                {
                    IdPlan = lector.GetInt32("id_plan"),
                    CodigoPlan = lector.LeerTexto("codigo_plan"),
                    NombrePlan = lector.LeerTexto("nombre_plan"),
                    Velocidad = lector.LeerTexto("velocidad"),
                    PrecioMensual = lector.GetDecimal("precio_mensual"),
                    Descripcion = lector.LeerTexto("descripcion"),
                    Estado = Enum.Parse<EstadoPlan>(lector.GetString("estado"))
                });
            }
            return planes;
        }
    }
}
