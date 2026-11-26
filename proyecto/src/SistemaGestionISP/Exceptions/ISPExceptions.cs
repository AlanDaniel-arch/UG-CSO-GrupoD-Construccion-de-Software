using System;

namespace SistemaGestionISP.Exceptions
{
    /// <summary>
    /// Excepción base de la aplicación. Las demás excepciones del sistema heredan de ella,
    /// así Program.cs puede capturarlas todas con un solo catch y mostrar un mensaje claro.
    /// </summary>
    public class ISPException : Exception
    {
        public ISPException(string mensaje) : base(mensaje) { }
        public ISPException(string mensaje, Exception? interna) : base(mensaje, interna) { }
    }

    /// <summary>
    /// Se lanza cuando un dato de entrada no cumple las reglas (cédula inválida,
    /// campo obligatorio vacío, estado no permitido, etc.).
    /// </summary>
    public class ValidacionException : ISPException
    {

        public ValidacionException(string campo, string mensaje)
            : base($"[{campo}] {mensaje}") { }
    }

    /// <summary>
    /// Se lanza al registrar un cliente cuya cédula ya existe (restricción UNIQUE
    /// de la tabla clientes).
    /// </summary>
    public class ClienteDuplicadoException : ISPException
    {

        public ClienteDuplicadoException(string cedula, Exception? interna = null)
            : base($"Ya existe un cliente registrado con la cédula {cedula}.", interna) { }
    }

    /// <summary>
    /// Se lanza al modificar un registro que no existe en la base de datos.
    /// </summary>
    public class RegistroNoEncontradoException : ISPException
    {
        public RegistroNoEncontradoException(string entidad, int id)
            : base($"No se encontró {entidad} con ID {id}.") { }
    }

    /// <summary>
    /// Se lanza cuando MySQL no responde después de agotar todos los reintentos.
    /// </summary>
    public class BaseDatosNoDisponibleException : ISPException
    {

        public BaseDatosNoDisponibleException(int intentos, Exception interna)
            : base($"La base de datos no está disponible (se realizaron {intentos} intentos). Verifique que MySQL esté en ejecución.", interna) { }
    }
}
