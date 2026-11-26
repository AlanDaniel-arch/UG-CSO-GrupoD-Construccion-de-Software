using System;
using MySql.Data.MySqlClient;

namespace SistemaGestionISP.Data
{
    /// <summary>Administra la conexión hacia la base de datos MySQL <c>isp_gestion_db</c>.</summary>
    public class ConexionBD : IConexionBD
    {
        /// <summary>
        /// Variable de entorno que, si existe, reemplaza la cadena de conexión por defecto.
        /// Así la contraseña real no queda escrita en el código.
        /// </summary>
        public const string VariableEntorno = "ISP_DB_CONNECTION";

        // Valor SOLO para desarrollo local; en otros entornos definir ISP_DB_CONNECTION.
        private const string CadenaPorDefecto = "Server=localhost;Database=isp_gestion_db;Uid=root;Pwd=root;";

        private readonly string cadenaConexion;

        /// <summary>Usa la variable de entorno ISP_DB_CONNECTION o, si no existe, la cadena por defecto.</summary>
        public ConexionBD() : this(Environment.GetEnvironmentVariable(VariableEntorno))
        {
        }

        /// <summary>Permite indicar la cadena de conexión directamente (útil en pruebas).</summary>
        /// <param name="cadenaConexion">Cadena de conexión; si es nula o vacía se usa la de desarrollo.</param>
        public ConexionBD(string? cadenaConexion)
        {
            this.cadenaConexion = string.IsNullOrWhiteSpace(cadenaConexion) ? CadenaPorDefecto : cadenaConexion;
        }

        /// <summary>Crea y retorna una nueva instancia de conexión MySQL (sin abrir).</summary>
        public MySqlConnection ObtenerConexion()
        {
            return new MySqlConnection(cadenaConexion);
        }
    }
}
