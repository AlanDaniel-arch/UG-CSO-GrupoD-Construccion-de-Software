using System;
using SistemaGestionISP.Data;
using SistemaGestionISP.Models;
using SistemaGestionISP.Repositories;

namespace SistemaGestionISP
{
    /// <summary>
    /// Punto de entrada y menú de consola del sistema de gestión ISP (Avance 2).
    /// Esta clase solo se ocupa de la interacción con el usuario; el acceso a la
    /// base de datos está en los repositorios (carpeta Repositories).
    /// </summary>
    internal static class Program
    {
        /// <summary>Opciones del menú principal. Los valores coinciden con los números que se muestran.</summary>
        private enum OpcionMenu
        {
            Invalida = 0,
            RegistrarCliente = 1,
            ListarClientes = 2,
            ConsultarPlanes = 3,
            CambiarEstadoServicio = 4,
            Salir = 5
        }

        private static void Main()
        {
            // Raíz de composición: aquí se crean las dependencias y se entregan a quien las usa.
            IConexionBD conexion = new ConexionBD();
            IClienteRepositorio clientes = new ClienteRepositorio(conexion);
            IPlanRepositorio planes = new PlanRepositorio(conexion);

            OpcionMenu opcion;
            do
            {
                MostrarMenu();
                opcion = LeerOpcion();
                Ejecutar(opcion, clientes, planes);

                if (opcion != OpcionMenu.Salir)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            } while (opcion != OpcionMenu.Salir);
        }

        /// <summary>Limpia la pantalla y dibuja el menú principal.</summary>
        private static void MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("================================================");
            Console.WriteLine("   SISTEMA DE GESTIÓN ISP - GRUPO D (AVANCE 2)  ");
            Console.WriteLine("================================================");
            Console.WriteLine($"{(int)OpcionMenu.RegistrarCliente}. [Módulo 1] Registrar Cliente");
            Console.WriteLine($"{(int)OpcionMenu.ListarClientes}. [Módulo 1] Listar Clientes");
            Console.WriteLine($"{(int)OpcionMenu.ConsultarPlanes}. [Módulo 2] Consultar Planes de Internet");
            Console.WriteLine($"{(int)OpcionMenu.CambiarEstadoServicio}. [Módulo 4] Cambiar Estado de Servicio");
            Console.WriteLine($"{(int)OpcionMenu.Salir}. Salir");
            Console.Write("\nSeleccione una opción: ");
        }

        /// <summary>Lee la opción escrita; si no es un número del menú devuelve <see cref="OpcionMenu.Invalida"/>.</summary>
        private static OpcionMenu LeerOpcion()
        {
            bool esNumero = int.TryParse(Console.ReadLine(), out int valor);
            return esNumero && Enum.IsDefined(typeof(OpcionMenu), valor)
                ? (OpcionMenu)valor
                : OpcionMenu.Invalida;
        }

        /// <summary>Despacha la opción elegida hacia el método que la atiende.</summary>
        private static void Ejecutar(OpcionMenu opcion, IClienteRepositorio clientes, IPlanRepositorio planes)
        {
            switch (opcion)
            {
                case OpcionMenu.RegistrarCliente:
                    RegistrarCliente(clientes);
                    break;
                case OpcionMenu.ListarClientes:
                    ListarClientes(clientes);
                    break;
                case OpcionMenu.ConsultarPlanes:
                    ConsultarPlanes(planes);
                    break;
                case OpcionMenu.CambiarEstadoServicio:
                    CambiarEstadoServicio(clientes);
                    break;
                case OpcionMenu.Salir:
                    break;
                default:
                    Console.WriteLine("\n[AVISO] Opción no válida. Elija un número del menú.");
                    break;
            }
        }

        /// <summary>Módulo 1: pide los datos por consola y registra al cliente.</summary>
        private static void RegistrarCliente(IClienteRepositorio clientes)
        {
            var cliente = new Cliente
            {
                Cedula = LeerTexto("Cédula: "),
                Nombres = LeerTexto("Nombres: "),
                Apellidos = LeerTexto("Apellidos: "),
                Telefono = LeerTexto("Teléfono: "),
                Direccion = LeerTexto("Dirección: ")
            };

            Console.WriteLine(clientes.Registrar(cliente)
                ? "\n[OK] Cliente registrado exitosamente."
                : "\n[ERROR] No se pudo registrar al cliente.");
        }

        /// <summary>Módulo 1: muestra todos los clientes registrados.</summary>
        private static void ListarClientes(IClienteRepositorio clientes)
        {
            Console.WriteLine("\n--- CLIENTES REGISTRADOS ---");
            foreach (var c in clientes.Listar())
            {
                Console.WriteLine($"ID: {c.IdCliente} | Cédula: {c.Cedula} | {c.Nombres} {c.Apellidos} | Estado: {c.EstadoServicio}");
            }
        }

        /// <summary>Módulo 2: muestra el catálogo de planes de Internet.</summary>
        private static void ConsultarPlanes(IPlanRepositorio planes)
        {
            Console.WriteLine("\n--- PLANES DISPONIBLES ---");
            foreach (var p in planes.Listar())
            {
                Console.WriteLine($"ID: {p.IdPlan} | {p.NombrePlan} ({p.VelocidadMbps} Mbps) - ${p.PrecioMensual}");
            }
        }

        /// <summary>
        /// Módulo 4: cambia el estado del servicio de un cliente. El estado escrito se
        /// valida contra <see cref="EstadoServicio"/> antes de llegar a la base de datos.
        /// </summary>
        private static void CambiarEstadoServicio(IClienteRepositorio clientes)
        {
            if (!int.TryParse(LeerTexto("ID del Cliente: "), out int idCliente))
            {
                Console.WriteLine("\n[ERROR] El ID del cliente debe ser un número entero.");
                return;
            }

            string entrada = LeerTexto("Nuevo estado (Activo / Suspendido / Cancelado): ");
            if (!Enum.TryParse(entrada, true, out EstadoServicio estado) || !Enum.IsDefined(estado))
            {
                Console.WriteLine("\n[ERROR] Estado no válido. Use Activo, Suspendido o Cancelado.");
                return;
            }

            Console.WriteLine(clientes.ActualizarEstadoServicio(idCliente, estado)
                ? "\n[OK] Estado de servicio actualizado."
                : "\n[ERROR] No existe un cliente con ese ID.");
        }

        /// <summary>Muestra un mensaje y devuelve lo que el usuario escribe (nunca nulo).</summary>
        private static string LeerTexto(string mensaje)
        {
            Console.Write(mensaje);
            return Console.ReadLine() ?? string.Empty;
        }
    }
}
