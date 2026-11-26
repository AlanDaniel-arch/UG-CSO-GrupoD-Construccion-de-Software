using System;
using System.Linq;
using System.Text.RegularExpressions;
using SistemaGestionISP.Exceptions;
using SistemaGestionISP.Models;

namespace SistemaGestionISP.Services
{
    /// <summary>
    /// Validación de la entrada (programación defensiva). Es detección preventiva de fallas:
    /// las restricciones se comprueban antes de cambiar la base de datos, así un dato
    /// incorrecto nunca llega a MySQL. El dígito verificador de la cédula detecta errores
    /// por redundancia.
    /// </summary>
    public static class ValidadorCliente
    {
        public static readonly string[] EstadosPermitidos = { "Activo", "Suspendido", "Cancelado" };

        /// <summary>Valida todos los campos de un cliente y lanza ValidacionException si alguno falla.</summary>
        public static void Validar(Cliente cliente)
        {
            ValidarCedula(cliente.Cedula);
            ValidarTexto("Nombres", cliente.Nombres, 100);
            ValidarTexto("Apellidos", cliente.Apellidos, 100);

            if (!string.IsNullOrWhiteSpace(cliente.Telefono) && !Regex.IsMatch(cliente.Telefono, @"^\d{7,15}$"))
                throw new ValidacionException("Teléfono", "Debe contener entre 7 y 15 dígitos numéricos.");

            if (cliente.Direccion?.Length > 200)
                throw new ValidacionException("Dirección", "No puede superar los 200 caracteres.");
        }

        /// <summary>
        /// Valida una cédula ecuatoriana: 10 dígitos, provincia de 01 a 24 (o 30), tercer dígito
        /// de 0 a 5 y dígito verificador por módulo 10. Sigue el patrón Try de .NET: devuelve si es
        /// válida o no y, aparte, el motivo técnico en <paramref name="detalle"/> (vacío si es válida).
        /// El motivo no se muestra al usuario, pero sirve para registros o para un inicio de sesión.
        /// </summary>
        public static bool TryValidarCedula(string? cedula, out string detalle)
        {
            detalle = string.Empty;

            if (string.IsNullOrWhiteSpace(cedula))
            {
                detalle = "La cédula está vacía.";
                return false;
            }

            if (!Regex.IsMatch(cedula, @"^\d{10}$"))
            {
                detalle = "No tiene exactamente 10 dígitos numéricos.";
                return false;
            }

            int provincia = int.Parse(cedula.Substring(0, 2));
            if ((provincia < 1 || provincia > 24) && provincia != 30)
            {
                detalle = "El código de provincia (dos primeros dígitos) no existe.";
                return false;
            }

            if (cedula[2] - '0' > 5)
            {
                detalle = "El tercer dígito no corresponde a una persona natural.";
                return false;
            }

            int suma = 0;
            for (int i = 0; i < 9; i++)
            {
                int valor = (cedula[i] - '0') * (i % 2 == 0 ? 2 : 1);
                suma += valor > 9 ? valor - 9 : valor;
            }
            int verificador = (10 - suma % 10) % 10;

            if (verificador != cedula[9] - '0')
            {
                detalle = "El dígito verificador no coincide.";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Igual que <see cref="TryValidarCedula"/>, pero lanza ValidacionException si la cédula no es
        /// válida. Al usuario solo le dice que no es válida, sin el motivo técnico.
        /// </summary>
        public static void ValidarCedula(string? cedula)
        {
            if (!TryValidarCedula(cedula, out _))
                throw new ValidacionException("Cédula", "La cédula no es válida.");
        }

        /// <summary>Comprueba que el estado de servicio sea uno de los permitidos y lo devuelve bien escrito.</summary>
        public static string ValidarEstado(string? estado)
        {
            string? normalizado = EstadosPermitidos.FirstOrDefault(
                e => string.Equals(e, estado?.Trim(), StringComparison.OrdinalIgnoreCase));

            if (normalizado is null)
                throw new ValidacionException("Estado", $"Estado no permitido. Use: {string.Join(", ", EstadosPermitidos)}.");

            return normalizado;
        }

        private static void ValidarTexto(string campo, string? valor, int max)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ValidacionException(campo, "Es un campo obligatorio.");
            if (valor.Length > max)
                throw new ValidacionException(campo, $"No puede superar los {max} caracteres.");
        }
    }
}
