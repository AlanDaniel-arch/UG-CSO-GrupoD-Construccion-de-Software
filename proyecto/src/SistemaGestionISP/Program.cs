using System;
using SistemaGestionISP.Models;
using SistemaGestionISP.Services;

namespace SistemaGestionISP
{
    /// <summary>
    /// Punto de entrada principal de la aplicación de gestión ISP (Avance 1).
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
                        if (int.TryParse(Console.ReadLine(), out int idCliente))
                        {
                            Console.Write("Nuevo estado (Activo / Suspendido / Cancelado): ");
                            string estado = Console.ReadLine() ?? "Activo";

                            if (servicio.ActualizarEstadoServicio(idCliente, estado))
                                Console.WriteLine("\n[OK] Estado de servicio actualizado.");
                        }
                        break;
                }

                if (opcion != 5)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }
    }
}