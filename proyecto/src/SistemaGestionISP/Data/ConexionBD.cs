using MySql.Data.MySqlClient;

namespace SistemaGestionISP.Data
{
    /// <summary>
    /// Administra la conexión hacia el servidor de base de datos MySQL.
    /// </summary>
    public class ConexionBD
    {
        // Cadena de conexión local (actualizar contraseña según el entorno)
        private readonly string cadenaConexion = "Server=localhost;Database=isp_gestion;Uid=root;Pwd=root;";

        /// <summary>
        /// Crea y retorna una nueva instancia de conexión MySQL.
        /// </summary>
        public MySqlConnection ObtenerConexion()
        {
            return new MySqlConnection(cadenaConexion);
        }
    }
}