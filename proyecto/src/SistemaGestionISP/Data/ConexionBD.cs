using System;
using MySql.Data.MySqlClient;

namespace SistemaGestionISP.Data
{
    /// <summary>
    /// Administra la conexión hacia el servidor de base de datos MySQL.
    /// Aquí se aplica la tolerancia a fallos: abrir la conexión se reintenta con
    /// <see cref="PoliticaReintentos"/> cuando el servidor no responde.
    /// </summary>
    public class ConexionBD
    {
        // Ajustes de la tolerancia a fallos. Se cambian aquí si el servidor está lejos o es más lento.
        // Peor caso de espera = intentos x tiempo límite + esperas entre intentos (hoy, unos 10 s).
        private const int MaxIntentos = 3;
        private const int EsperaBaseMs = 500;
        private const int TiempoLimiteSegundos = 3;

        // Cadena de conexión local (actualizar contraseña según el entorno)
        private readonly string cadenaConexion =
            $"Server=localhost;Database=isp_gestion;Uid=root;Pwd=root;Connection Timeout={TiempoLimiteSegundos};";

        // Solo se reintentan los errores de conexión (servidor caído u ocupado).
        private readonly PoliticaReintentos politica =
            new PoliticaReintentos(maxIntentos: MaxIntentos, esperaBaseMs: EsperaBaseMs, esTransitoria: EsErrorDeConexion);

        /// <summary>
        /// Crea y retorna una nueva instancia de conexión MySQL.
        /// </summary>
        public MySqlConnection ObtenerConexion()
        {
            return new MySqlConnection(cadenaConexion);
        }

        /// <summary>
        /// Abre una conexión usando la política de reintentos.
        /// Lanza BaseDatosNoDisponibleException si no logra conectar.
        /// </summary>
        public MySqlConnection AbrirConexion()
        {
            return politica.Ejecutar(() =>
            {
                var conexion = ObtenerConexion();
                try
                {
                    conexion.Open();
                    return conexion;
                }
                catch
                {
                    conexion.Dispose();
                    throw;
                }
            });
        }

        /// <summary>
        /// Indica si la excepción es un fallo de conexión (transitorio). Números de MySQL:
        /// 0 no se pudo conectar, 1040 demasiadas conexiones, 1042 host inalcanzable,
        /// 1043 handshake inválido.
        /// </summary>
        public static bool EsErrorDeConexion(Exception ex)
        {
            return (ex is MySqlException m && (m.Number == 0 || m.Number == 1040 || m.Number == 1042 || m.Number == 1043))
                   || ex is TimeoutException
                   || ex is System.Net.Sockets.SocketException;
        }
    }
}