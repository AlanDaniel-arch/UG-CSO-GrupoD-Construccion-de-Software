using System;
using MySql.Data.MySqlClient;
using SistemaGestionISP.Data;
using SistemaGestionISP.Models;
using SistemaGestionISP.Repositories;

namespace SistemaGestionISP
{
    /// <summary>
    /// Punto de entrada y menú de consola. Es una herramienta provisional para ejercitar la capa
    /// de datos (Models, Data, Repositories) mientras se construye la aplicación ASP.NET Core MVC
    /// del documento maestro V2 (§6.2). Aquí solo hay interacción con el usuario; el SQL vive en
    /// los repositorios.
    /// </summary>
    internal static class Program
    {
        /// <summary>Opciones del menú. Los valores coinciden con los números que se muestran.</summary>
        private enum OpcionMenu
        {
            Invalida = 0,
            RegistrarCliente = 1,      // RF-04
            ListarClientes = 2,        // RF-05
            ConsultarPlanes = 3,       // RF-09 (consulta)
            VerServicios = 4,          // RF-19
            CambiarEstadoServicio = 5, // RF-20
            Salir = 6
        }

        private static void Main()
        {
            // Raíz de composición: aquí se crean las dependencias y se entregan a quien las usa.
            IConexionBD conexion = new ConexionBD();
            IClienteRepositorio clientes = new ClienteRepositorioMySql(conexion);
            IPlanRepositorio planes = new PlanRepositorioMySql(conexion);
            IServicioRepositorio servicios = new ServicioRepositorioMySql(conexion);

            OpcionMenu opcion;
            do
            {
                MostrarMenu();
                opcion = LeerOpcion();
                Ejecutar(opcion, clientes, planes, servicios);

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
            Console.WriteLine("   ISP MANAGER - GRUPO D (consola de verificación)");
            Console.WriteLine("================================================");
            Console.WriteLine($"{(int)OpcionMenu.RegistrarCliente}. [M1] Registrar cliente con contrato");
            Console.WriteLine($"{(int)OpcionMenu.ListarClientes}. [M1] Listar clientes");
            Console.WriteLine($"{(int)OpcionMenu.ConsultarPlanes}. [M2] Consultar planes de Internet");
            Console.WriteLine($"{(int)OpcionMenu.VerServicios}. [M4] Ver estado de los servicios");
            Console.WriteLine($"{(int)OpcionMenu.CambiarEstadoServicio}. [M4] Cambiar estado de un servicio");
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

        /// <summary>
        /// Despacha la opción elegida. Los errores de MySQL se informan al usuario en lugar de
        /// cerrar el programa. Es un manejo provisional: la jerarquía de excepciones propias
        /// (V2 §9.3) se incorpora en la entrega parcial (semana 6).
        /// </summary>
        private static void Ejecutar(OpcionMenu opcion, IClienteRepositorio clientes,
            IPlanRepositorio planes, IServicioRepositorio servicios)
        {
            try
            {
                switch (opcion)
                {
                    case OpcionMenu.RegistrarCliente:
                        RegistrarCliente(clientes, planes);
                        break;
                    case OpcionMenu.ListarClientes:
                        ListarClientes(clientes);
                        break;
                    case OpcionMenu.ConsultarPlanes:
                        MostrarPlanes(planes);
                        break;
                    case OpcionMenu.VerServicios:
                        MostrarServicios(servicios);
                        break;
                    case OpcionMenu.CambiarEstadoServicio:
                        CambiarEstadoServicio(servicios);
                        break;
                    case OpcionMenu.Salir:
                        break;
                    default:
                        Console.WriteLine("\n[AVISO] Opción no válida. Elija un número del menú.");
                        break;
                }
            }
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                Console.WriteLine("\n[ERROR] Ya existe un cliente con esa identificación (RN-01).");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\n[ERROR] No se pudo completar la operación en la base de datos (código {ex.Number}): {ex.Message}");
            }
        }

        /// <summary>RF-04: pide los datos del cliente y el plan, y registra el cliente con su contrato.</summary>
        private static void RegistrarCliente(IClienteRepositorio clientes, IPlanRepositorio planes)
        {
            MostrarPlanes(planes);

            var cliente = new Cliente
            {
                Identificacion = LeerTexto("\nCédula o RUC: "),
                NombresApellidos = LeerTexto("Nombres y apellidos: "),
                Direccion = LeerTexto("Dirección de instalación: "),
                Telefono = LeerTexto("Teléfono: "),
                Correo = LeerTexto("Correo electrónico: ")
            };
            string entradaPlan = LeerTexto("ID del plan contratado: ");

            // RNF-02: los campos obligatorios se validan antes de tocar la base de datos.
            if (string.IsNullOrWhiteSpace(cliente.Identificacion) || string.IsNullOrWhiteSpace(cliente.NombresApellidos)
                || string.IsNullOrWhiteSpace(cliente.Direccion) || string.IsNullOrWhiteSpace(cliente.Telefono)
                || string.IsNullOrWhiteSpace(cliente.Correo))
            {
                Console.WriteLine("\n[ERROR] Todos los campos del cliente son obligatorios.");
                return;
            }
            if (!int.TryParse(entradaPlan, out int idPlan))
            {
                Console.WriteLine("\n[ERROR] El ID del plan debe ser un número entero.");
                return;
            }

            string codigoContrato = clientes.RegistrarConContrato(cliente, idPlan);
            Console.WriteLine($"\n[OK] Cliente registrado con el contrato {codigoContrato}.");
        }

        /// <summary>RF-05: muestra los clientes con su plan y el estado de su contrato.</summary>
        private static void ListarClientes(IClienteRepositorio clientes)
        {
            Console.WriteLine("\n--- CLIENTES ---");
            foreach (var c in clientes.Listar())
            {
                Console.WriteLine($"{c.Identificacion} | {c.NombresApellidos} | {c.CodigoContrato} | {c.NombrePlan} | {c.EstadoContrato}");
            }
        }

        /// <summary>Módulo 2: muestra el catálogo de planes de Internet.</summary>
        private static void MostrarPlanes(IPlanRepositorio planes)
        {
            Console.WriteLine("\n--- PLANES DE INTERNET ---");
            foreach (var p in planes.Listar())
            {
                Console.WriteLine($"ID: {p.IdPlan} | {p.CodigoPlan} | {p.NombrePlan} ({p.Velocidad}) - ${p.PrecioMensual} | {p.Estado}");
            }
        }

        /// <summary>RF-19: muestra el estado del servicio de cada contrato.</summary>
        private static void MostrarServicios(IServicioRepositorio servicios)
        {
            Console.WriteLine("\n--- ESTADO DE LOS SERVICIOS ---");
            foreach (var s in servicios.Listar())
            {
                Console.WriteLine($"ID: {s.IdServicio} | {s.CodigoServicio} | {s.NombreCliente} | {s.CodigoContrato} | {s.EstadoServicio} | {s.FechaActualizacion:yyyy-MM-dd}");
            }
        }

        /// <summary>
        /// RF-20: cambia el estado de un servicio. El estado escrito se valida contra
        /// <see cref="EstadoServicio"/> antes de llegar a MySQL y el contrato se sincroniza (RN-07).
        /// </summary>
        private static void CambiarEstadoServicio(IServicioRepositorio servicios)
        {
            MostrarServicios(servicios);

            if (!int.TryParse(LeerTexto("\nID del servicio: "), out int idServicio))
            {
                Console.WriteLine("\n[ERROR] El ID del servicio debe ser un número entero.");
                return;
            }

            string entrada = LeerTexto("Nuevo estado (Activo / Suspendido / Pendiente): ");
            if (!Enum.TryParse(entrada, true, out EstadoServicio estado) || !Enum.IsDefined(estado))
            {
                Console.WriteLine("\n[ERROR] Estado no válido. Use Activo, Suspendido o Pendiente.");
                return;
            }

            Console.WriteLine(servicios.CambiarEstado(idServicio, estado)
                ? "\n[OK] Estado del servicio actualizado y contrato sincronizado."
                : "\n[ERROR] No existe un servicio con ese ID.");
        }

        /// <summary>Muestra un mensaje y devuelve lo que el usuario escribe (nunca nulo).</summary>
        private static string LeerTexto(string mensaje)
        {
            Console.Write(mensaje);
            return Console.ReadLine() ?? string.Empty;
        }
    }
}
