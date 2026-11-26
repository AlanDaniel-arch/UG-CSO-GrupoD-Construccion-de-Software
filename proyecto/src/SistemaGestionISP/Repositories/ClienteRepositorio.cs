using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SistemaGestionISP.Data;
using SistemaGestionISP.Models;

namespace SistemaGestionISP.Repositories
{
    /// <summary>
    /// Acceso a datos de la tabla <c>clientes</c> (Módulo 1: registro y listado;
    /// Módulo 4: estado del servicio). Todas las consultas son parametrizadas
    /// para evitar inyección SQL.
    /// </summary>
    public class ClienteRepositorio : IClienteRepositorio
    {
        private readonly IConexionBD db;

        /// <param name="db">Proveedor de conexiones a la base de datos.</param>
        public ClienteRepositorio(IConexionBD db)
        {
            this.db = db;
        }

        /// <summary>Módulo 1: registra un nuevo cliente en la base de datos.</summary>
        public bool Registrar(Cliente cliente)
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

        /// <summary>Módulo 1: obtiene la lista completa de clientes registrados.</summary>
        public List<Cliente> Listar()
        {
            var lista = new List<Cliente>();
            using var conn = db.ObtenerConexion();
            conn.Open();
            string sql = "SELECT id_cliente, cedula, nombres, apellidos, telefono, direccion, estado_servicio FROM clientes";

            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(MapearCliente(reader));
            }
            return lista;
        }

        /// <summary>Módulo 4: actualiza el estado operativo del servicio de un cliente.</summary>
        public bool ActualizarEstadoServicio(int idCliente, EstadoServicio nuevoEstado)
        {
            using var conn = db.ObtenerConexion();
            conn.Open();
            string sql = "UPDATE clientes SET estado_servicio = @estado WHERE id_cliente = @id";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@estado", nuevoEstado.ToString());
            cmd.Parameters.AddWithValue("@id", idCliente);

            return cmd.ExecuteNonQuery() > 0;
        }

        /// <summary>
        /// Convierte la fila actual del lector en un <see cref="Cliente"/>.
        /// Las columnas se leen por nombre (no por posición) y los valores NULL de
        /// teléfono y dirección se convierten en texto vacío.
        /// </summary>
        private static Cliente MapearCliente(MySqlDataReader reader)
        {
            int colTelefono = reader.GetOrdinal("telefono");
            int colDireccion = reader.GetOrdinal("direccion");

            return new Cliente
            {
                IdCliente = reader.GetInt32("id_cliente"),
                Cedula = reader.GetString("cedula"),
                Nombres = reader.GetString("nombres"),
                Apellidos = reader.GetString("apellidos"),
                Telefono = reader.IsDBNull(colTelefono) ? string.Empty : reader.GetString(colTelefono),
                Direccion = reader.IsDBNull(colDireccion) ? string.Empty : reader.GetString(colDireccion),
                EstadoServicio = Enum.Parse<EstadoServicio>(reader.GetString("estado_servicio"))
            };
        }
    }
}
