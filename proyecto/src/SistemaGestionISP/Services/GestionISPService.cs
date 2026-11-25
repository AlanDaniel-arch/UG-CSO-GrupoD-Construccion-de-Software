using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SistemaGestionISP.Data;
using SistemaGestionISP.Models;

namespace SistemaGestionISP.Services
{
    /// <summary>
    /// Servicio principal que implementa las operaciones CRUD del Avance 1.
    /// </summary>
    public class GestionISPService
    {
        private readonly ConexionBD db = new ConexionBD();

        /// <summary>
        /// Módulo 1: Registra un nuevo cliente en la base de datos.
        /// </summary>
        public bool RegistrarCliente(Cliente cliente)
        {
            using var conn = db.ObtenerConexion();
            conn.Open();
            string sql = @"INSERT INTO clientes (cedula, nombres, apellidos, telefono, direccion) 
                           VALUES (@cedula, @nombres, @apellidos, @telefono, @direccion)";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@cedula", cliente.Cedula);
            cmd.Parameters.AddWithValue("@nombres", cliente.Nombres);
            cmd.Parameters.AddWithValue("@apellidos", cliente.Apellidos);
            cmd.Parameters.AddWithValue("@telefono", cliente.Telefono);
            cmd.Parameters.AddWithValue("@direccion", cliente.Direccion);

            return cmd.ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// Módulo 1: Obtiene la lista completa de clientes registrados.
        /// </summary>
        public List<Cliente> ListarClientes()
        {
            var lista = new List<Cliente>();
            using var conn = db.ObtenerConexion();
            conn.Open();
            string sql = "SELECT id_cliente, cedula, nombres, apellidos, telefono, direccion, estado_servicio FROM clientes";

            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Cliente
                {
                    IdCliente = reader.GetInt32("id_cliente"),
                    Cedula = reader.GetString("cedula"),
                    Nombres = reader.GetString("nombres"),
                    Apellidos = reader.GetString("apellidos"),
                    Telefono = reader.IsDBNull(4) ? string.Empty : reader.GetString("telefono"),
                    Direccion = reader.IsDBNull(5) ? string.Empty : reader.GetString("direccion"),
                    EstadoServicio = reader.GetString("estado_servicio")
                });
            }
            return lista;
        }

        /// <summary>
        /// Módulo 2: Consulta el catálogo de planes de Internet disponibles.
        /// </summary>
        public List<PlanInternet> ListarPlanes()
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

        /// <summary>
        /// Módulo 4: Actualiza el estado operativo del servicio de un cliente (Activo/Suspendido/Cancelado).
        /// </summary>
        public bool ActualizarEstadoServicio(int idCliente, string nuevoEstado)
        {
            using var conn = db.ObtenerConexion();
            conn.Open();
            string sql = "UPDATE clientes SET estado_servicio = @estado WHERE id_cliente = @id";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@estado", nuevoEstado);
            cmd.Parameters.AddWithValue("@id", idCliente);

            return cmd.ExecuteNonQuery() > 0;
        }
    }
}