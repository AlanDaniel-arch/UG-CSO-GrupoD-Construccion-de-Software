using MySql.Data.MySqlClient;

namespace SistemaGestionISP.Data
{
    /// <summary>
    /// Contrato para obtener conexiones a la base de datos. Los repositorios dependen de esta
    /// interfaz y no de la clase concreta (principio D, V2 §7.2), lo que permite sustituirla
    /// en las pruebas.
    /// </summary>
    public interface IConexionBD
    {
        /// <summary>Crea una conexión nueva, aún sin abrir. Quien la use debe liberarla (using).</summary>
        MySqlConnection ObtenerConexion();
    }
}
