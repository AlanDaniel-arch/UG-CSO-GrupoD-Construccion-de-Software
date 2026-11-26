using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SistemaGestionISP.Data;
using SistemaGestionISP.Exceptions;
using SistemaGestionISP.Models;

namespace SistemaGestionISP.Services
{
    /// <summary>
    /// Servicio principal que implementa las operaciones CRUD del Avance 1.
    /// Valida la entrada antes de acceder a MySQL, abre la conexión con reintentos y
    /// traduce los errores de MySQL a excepciones propias con mensajes claros (ver Exceptions/).
    /// </summary>
    public class GestionISPService
    {
        // Código de MySQL para entrada duplicada (viola un UNIQUE).
        private const int ErrorEntradaDuplicada = 1062;

        private readonly ConexionBD db = new ConexionBD();

        /// <summary>
        /// Módulo 1: Registra un nuevo cliente en la base de datos.
        /// </summary>
        /// <exception cref="ValidacionException">Si algún dato es inválido.</exception>
        /// <exception cref="ClienteDuplicadoException">Si la cédula ya existe.</exception>
        /// <exception cref="BaseDatosNoDisponibleException">Si MySQL no responde tras los reintentos.</exception>
        public bool RegistrarCliente(Cliente cliente)
        {
            ValidadorCliente.Validar(cliente);   // se valida antes de tocar la base

            using var conn = db.AbrirConexion();
            string sql = @"INSERT INTO clientes (cedula, nombres, apellidos, telefono, direccion) 
                           VALUES (@cedula, @nombres, @apellidos, @telefono, @direccion)";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@cedula", cliente.Cedula);
            cmd.Parameters.AddWithValue("@nombres", cliente.Nombres);
            cmd.Parameters.AddWithValue("@apellidos", cliente.Apellidos);
            cmd.Parameters.AddWithValue("@telefono", cliente.Telefono);
            cmd.Parameters.AddWithValue("@direccion", cliente.Direccion);

            try
            {
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (MySqlException ex) when (ex.Number == ErrorEntradaDuplicada)
            {
                // El error de MySQL se convierte en una excepción propia.
                throw new ClienteDuplicadoException(cliente.Cedula, ex);
            }
        }

        /// <summary>
        /// Módulo 1: Obtiene la lista completa de clientes registrados.
        /// </summary>
        public List<Cliente> ListarClientes()
        {
            var lista = new List<Cliente>();
            using var conn = db.AbrirConexion();
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
            using var conn = db.AbrirConexion();
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
        /// <exception cref="ValidacionException">Si el estado no es uno de los permitidos.</exception>
        /// <exception cref="RegistroNoEncontradoException">Si el cliente no existe.</exception>
        public bool ActualizarEstadoServicio(int idCliente, string nuevoEstado)
        {
            string estado = ValidadorCliente.ValidarEstado(nuevoEstado);

            using var conn = db.AbrirConexion();
            string sql = "UPDATE clientes SET estado_servicio = @estado WHERE id_cliente = @id";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@estado", estado);
            cmd.Parameters.AddWithValue("@id", idCliente);

            // MySQL devuelve 0 filas tanto si el ID no existe como si el estado ya era ese,
            // así que se consulta si el cliente existe para dar el aviso correcto.
            if (cmd.ExecuteNonQuery() > 0) return true;

            using var existe = new MySqlCommand("SELECT COUNT(*) FROM clientes WHERE id_cliente = @id", conn);
            existe.Parameters.AddWithValue("@id", idCliente);
            if (Convert.ToInt32(existe.ExecuteScalar()) == 0)
                throw new RegistroNoEncontradoException("un cliente", idCliente);

            return true; // existe y ya tenía ese estado
        }
    }
}