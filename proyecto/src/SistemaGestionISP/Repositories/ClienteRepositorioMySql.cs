using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SistemaGestionISP.Data;
using SistemaGestionISP.Models;

namespace SistemaGestionISP.Repositories
{
    /// <summary>Implementación MySQL de <see cref="IClienteRepositorio"/> (tablas clientes, contratos y servicios).</summary>
    public class ClienteRepositorioMySql : IClienteRepositorio
    {
        private readonly IConexionBD db;

        /// <param name="db">Proveedor de conexiones a la base de datos.</param>
        public ClienteRepositorioMySql(IConexionBD db)
        {
            this.db = db;
        }

        /// <inheritdoc />
        public string RegistrarConContrato(Cliente cliente, int idPlan)
        {
            using var conn = db.ObtenerConexion();
            conn.Open();
            // Si ocurre una excepción antes de Commit, al liberar la transacción se hace rollback.
            using var tx = conn.BeginTransaction();

            // 1. Cliente.
            long idCliente;
            using (var cmd = AyudanteSql.Crear(conn, tx,
                @"INSERT INTO clientes (identificacion, nombres_apellidos, direccion, telefono, correo, fecha_registro)
                  VALUES (@identificacion, @nombres, @direccion, @telefono, @correo, @fecha)",
                ("@identificacion", cliente.Identificacion),
                ("@nombres", cliente.NombresApellidos),
                ("@direccion", cliente.Direccion),
                ("@telefono", cliente.Telefono),
                ("@correo", cliente.Correo),
                ("@fecha", cliente.FechaRegistro.Date)))
            {
                cmd.ExecuteNonQuery();
                idCliente = cmd.LastInsertedId;
            }

            // 2. Contrato. El código definitivo (CTR-AAAA-NNN) se deriva del identificador
            //    autoincremental, para que nunca se repita (a diferencia de «length + 1», V2 §7.3).
            //    Mientras tanto se usa un código temporal único.
            long idContrato;
            using (var cmd = AyudanteSql.Crear(conn, tx,
                @"INSERT INTO contratos (codigo_contrato, id_cliente, id_plan, fecha_inicio, estado_contrato)
                  VALUES (@codigo, @cliente, @plan, @inicio, 'Activo')",
                ("@codigo", "TMP-" + Guid.NewGuid().ToString("N").Substring(0, 20)),
                ("@cliente", idCliente),
                ("@plan", idPlan),
                ("@inicio", DateTime.Today)))
            {
                cmd.ExecuteNonQuery();
                idContrato = cmd.LastInsertedId;
            }

            using (var cmd = AyudanteSql.Crear(conn, tx,
                @"UPDATE contratos
                  SET codigo_contrato = CONCAT('CTR-', YEAR(fecha_inicio), '-', LPAD(id_contrato, 3, '0'))
                  WHERE id_contrato = @id",
                ("@id", idContrato)))
            {
                cmd.ExecuteNonQuery();
            }

            // 3. Servicio (relación 1:1 con el contrato, RN-02), inicialmente Activo.
            long idServicio;
            using (var cmd = AyudanteSql.Crear(conn, tx,
                @"INSERT INTO servicios (codigo_servicio, id_cliente, id_contrato, estado_servicio, fecha_actualizacion)
                  VALUES (@codigo, @cliente, @contrato, 'Activo', @fecha)",
                ("@codigo", "TMP-" + Guid.NewGuid().ToString("N").Substring(0, 16)),
                ("@cliente", idCliente),
                ("@contrato", idContrato),
                ("@fecha", DateTime.Today)))
            {
                cmd.ExecuteNonQuery();
                idServicio = cmd.LastInsertedId;
            }

            using (var cmd = AyudanteSql.Crear(conn, tx,
                "UPDATE servicios SET codigo_servicio = CONCAT('SRV-', LPAD(id_servicio, 3, '0')) WHERE id_servicio = @id",
                ("@id", idServicio)))
            {
                cmd.ExecuteNonQuery();
            }

            // 4. Código del contrato recién creado, para informarlo al usuario.
            string codigoContrato;
            using (var cmd = AyudanteSql.Crear(conn, tx,
                "SELECT codigo_contrato FROM contratos WHERE id_contrato = @id",
                ("@id", idContrato)))
            {
                codigoContrato = Convert.ToString(cmd.ExecuteScalar()) ?? string.Empty;
            }

            tx.Commit();
            return codigoContrato;
        }

        /// <inheritdoc />
        public List<ClienteResumen> Listar()
        {
            var lista = new List<ClienteResumen>();
            using var conn = db.ObtenerConexion();
            conn.Open();

            // LEFT JOIN: un cliente sin contrato también aparece en el listado.
            using var cmd = AyudanteSql.Crear(conn, null,
                @"SELECT c.id_cliente, c.identificacion, c.nombres_apellidos, c.telefono, c.correo,
                         ct.codigo_contrato, p.nombre_plan, ct.estado_contrato
                  FROM clientes c
                  LEFT JOIN contratos ct ON ct.id_cliente = c.id_cliente
                  LEFT JOIN planes p ON p.id_plan = ct.id_plan
                  ORDER BY c.id_cliente, ct.id_contrato");
            using var lector = cmd.ExecuteReader();

            while (lector.Read())
            {
                lista.Add(new ClienteResumen
                {
                    IdCliente = lector.GetInt32("id_cliente"),
                    Identificacion = lector.LeerTexto("identificacion"),
                    NombresApellidos = lector.LeerTexto("nombres_apellidos"),
                    Telefono = lector.LeerTexto("telefono"),
                    Correo = lector.LeerTexto("correo"),
                    CodigoContrato = lector.LeerTexto("codigo_contrato", "Sin contrato"),
                    NombrePlan = lector.LeerTexto("nombre_plan", "—"),
                    EstadoContrato = lector.LeerTexto("estado_contrato", "—")
                });
            }
            return lista;
        }
    }
}
