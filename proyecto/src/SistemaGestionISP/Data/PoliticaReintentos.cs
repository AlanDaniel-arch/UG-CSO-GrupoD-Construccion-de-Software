using System;
using System.Threading;
using SistemaGestionISP.Exceptions;

namespace SistemaGestionISP.Data
{
    /// <summary>
    /// Técnica de tolerancia a fallos del proyecto: reintentos con espera exponencial (retry con backoff).
    ///
    /// Controla fallos transitorios, como MySQL arrancando, un corte breve de red o demasiadas
    /// conexiones. Es recuperación hacia adelante por reintento: se repite la operación.
    /// El grado de tolerancia es degradación aceptable (fail-soft): si el fallo no desaparece,
    /// la aplicación sigue abierta y avisa con BaseDatosNoDisponibleException.
    ///
    /// La operación se ejecuta hasta <see cref="MaxIntentos"/> veces y entre intentos espera
    /// 1x, 2x, 4x... <see cref="EsperaBaseMs"/>. Los fallos que no son transitorios (SQL incorrecto,
    /// dato duplicado) se propagan al instante, sin reintentar.
    /// </summary>
    public class PoliticaReintentos
    {
        public int MaxIntentos { get; }
        public int EsperaBaseMs { get; }

        private readonly Func<Exception, bool> esTransitoria;

        public PoliticaReintentos(int maxIntentos, int esperaBaseMs, Func<Exception, bool> esTransitoria)
        {
            MaxIntentos = maxIntentos;
            EsperaBaseMs = esperaBaseMs;
            this.esTransitoria = esTransitoria;
        }

        /// <summary>
        /// Ejecuta la acción con la política. Devuelve su resultado si algún intento funciona.
        /// </summary>
        public T Ejecutar<T>(Func<T> accion)
        {
            Exception? ultimo = null;

            for (int intento = 1; intento <= MaxIntentos; intento++)
            {
                try
                {
                    return accion();
                }
                catch (Exception ex) when (esTransitoria(ex))
                {
                    ultimo = ex;
                    if (intento < MaxIntentos)
                    {
                        // La espera se duplica en cada intento: base * 2^(intento-1)
                        Thread.Sleep(EsperaBaseMs * (1 << (intento - 1)));
                    }
                }
            }

            throw new BaseDatosNoDisponibleException(MaxIntentos, ultimo!);
        }
    }
}
