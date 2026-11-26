using System;
using MySql.Data.MySqlClient;

namespace SistemaGestionISP.Data
{
    /// <summary>
    /// Utilidades comunes de los repositorios. Evita repetir en cada consulta la creación del
    /// comando con parámetros y la lectura de columnas que admiten NULL (código duplicado).
    /// </summary>
    internal static class AyudanteSql
    {
        /// <summary>
        /// Crea un comando con parámetros. Todos los valores viajan como parámetros (nunca
        /// concatenados al texto SQL), lo que previene la inyección SQL.
        /// </summary>
        /// <param name="conexion">Conexión ya abierta.</param>
        /// <param name="transaccion">Transacción en curso o null.</param>
        /// <param name="sql">Sentencia con parámetros @nombre.</param>
        /// <param name="parametros">Pares (nombre, valor); un valor null se envía como NULL.</param>
        public static MySqlCommand Crear(MySqlConnection conexion, MySqlTransaction? transaccion, string sql,
            params (string Nombre, object? Valor)[] parametros)
        {
            var comando = new MySqlCommand(sql, conexion, transaccion);
            foreach (var (nombre, valor) in parametros)
            {
                comando.Parameters.AddWithValue(nombre, valor ?? DBNull.Value);
            }
            return comando;
        }

        /// <summary>Lee una columna de texto por nombre; si es NULL devuelve el valor por defecto.</summary>
        public static string LeerTexto(this MySqlDataReader lector, string columna, string porDefecto = "")
        {
            int posicion = lector.GetOrdinal(columna);
            return lector.IsDBNull(posicion) ? porDefecto : lector.GetString(posicion);
        }
    }
}
