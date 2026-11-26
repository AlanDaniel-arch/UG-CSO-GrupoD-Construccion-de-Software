using System;
using SistemaGestionISP.Exceptions;
using SistemaGestionISP.Models;
using SistemaGestionISP.Services;

namespace SistemaGestionISP
{
    /// <summary>
    /// Punto de entrada principal de la aplicación de gestión ISP (Avance 1).
    /// Capa de presentación en consola: pide los datos, llama al servicio y muestra el
    /// resultado. Captura las excepciones propias, de modo que un error del usuario o de
    /// la base de datos no cierra la aplicación. Tipo de tolerancia: manejo de excepciones
    /// con degradación aceptable (fail-soft).
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            GestionISPService servicio = new GestionISPService();
            int opcion = 0;

            while (opcion != 5)
            {
                Console.Clear();
                Console.WriteLine("================================================");
                Console.WriteLine("   SISTEMA DE GESTIÓN ISP - GRUPO D (AVANCE 1)  ");
                Console.WriteLine("================================================");
                Console.WriteLine("1. [Módulo 1] Registrar Cliente");
                Console.WriteLine("2. [Módulo 1] Listar Clientes");
                Console.WriteLine("3. [Módulo 2] Consultar Planes de Internet");
                Console.WriteLine("4. [Módulo 4] Cambiar Estado de Servicio");
                Console.WriteLine("5. Salir");
                Console.Write("\nSeleccione una opción: ");

                if (!int.TryParse(Console.ReadLine(), out opcion)) continue;

                try
                {
                    EjecutarOpcion(opcion, servicio);
                }
                catch (ValidacionException ex)
                {
                    // Error del usuario: se le dice qué campo corregir.
                    Console.WriteLine($"\n[DATO INVÁLIDO] {ex.Message}");
                }
                catch (ClienteDuplicadoException ex)
                {
                    Console.WriteLine($"\n[DUPLICADO] {ex.Message}");
                }
                catch (RegistroNoEncontradoException ex)
                {
                    Console.WriteLine($"\n[NO ENCONTRADO] {ex.Message}");
                }
                catch (BaseDatosNoDisponibleException ex)
                {
                    // Sin base de datos: la aplicación sigue abierta y el usuario puede volver a intentar.
                    Console.WriteLine($"\n[SIN CONEXIÓN] {ex.Message}");
                }
                catch (ISPException ex)
                {
                    Console.WriteLine($"\n[ERROR] {ex.Message}");
                }
                catch (Exception ex)
                {
                    // Cualquier otro error que no se esperaba.
                    Console.WriteLine($"\n[ERROR INESPERADO] {ex.GetType().Name}: {ex.Message}");
                }

                if (opcion != 5)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        /// <summary>
        /// Ejecuta la opción elegida del menú. Las excepciones suben hasta Main, que las captura.
        /// </summary>
        private static void EjecutarOpcion(int opcion, GestionISPService servicio)
        {
            switch (opcion)
            {
                case 1:
                    Console.Write("Cédula: ");
                    string cedula = Console.ReadLine() ?? string.Empty;
                    Console.Write("Nombres: ");
                    string nombres = Console.ReadLine() ?? string.Empty;
                    Console.Write("Apellidos: ");
                    string apellidos = Console.ReadLine() ?? string.Empty;
                    Console.Write("Teléfono: ");
                    string telefono = Console.ReadLine() ?? string.Empty;
                    Console.Write("Dirección: ");
                    string direccion = Console.ReadLine() ?? string.Empty;

                    var cliente = new Cliente
                    {
                        Cedula = cedula,
                        Nombres = nombres,
                        Apellidos = apellidos,
                        Telefono = telefono,
                        Direccion = direccion
                    };

                    if (servicio.RegistrarCliente(cliente))
                        Console.WriteLine("\n[OK] Cliente registrado exitosamente.");
                    break;

                case 2:
                    var clientes = servicio.ListarClientes();
                    Console.WriteLine("\n--- CLIENTES REGISTRADOS ---");
                    foreach (var c in clientes)
                    {
                        Console.WriteLine($"ID: {c.IdCliente} | Cédula: {c.Cedula} | {c.Nombres} {c.Apellidos} | Estado: {c.EstadoServicio}");
                    }
                    break;

                case 3:
                    var planes = servicio.ListarPlanes();
                    Console.WriteLine("\n--- PLANES DISPONIBLES ---");
                    foreach (var p in planes)
                    {
                        Console.WriteLine($"ID: {p.IdPlan} | {p.NombrePlan} ({p.VelocidadMbps} Mbps) - ${p.PrecioMensual}");
                    }
                    break;

                case 4:
                    Console.Write("ID del Cliente: ");
                    if (!int.TryParse(Console.ReadLine(), out int idCliente))
                        throw new ValidacionException("ID", "Debe ingresar un número entero.");

                    Console.Write("Nuevo estado (Activo / Suspendido / Cancelado): ");
                    string estado = Console.ReadLine() ?? "Activo";

                    if (servicio.ActualizarEstadoServicio(idCliente, estado))
                        Console.WriteLine("\n[OK] Estado de servicio actualizado.");
                    break;
            }
        }
    }
}