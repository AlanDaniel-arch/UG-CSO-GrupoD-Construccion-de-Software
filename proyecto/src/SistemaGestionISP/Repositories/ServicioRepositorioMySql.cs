using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SistemaGestionISP.Data;
using SistemaGestionISP.Models;

namespace SistemaGestionISP.Repositories
{
    /// <summary>Implementación MySQL de <see cref="IServicioRepositorio"/> (tablas servicios y contratos).</summary>
    public class ServicioRepositorioMySql : IServicioRepositorio
    {
        private readonly IConexionBD db;

        /// <param name="db">Proveedor de conexiones a la base de datos.</param>
        public ServicioRepositorioMySql(IConexionBD db)
        {
            this.db = db;
        }

        /// <inheritdoc />
        public List<ServicioResumen> Listar()
        {
            var lista = new List<ServicioResumen>();
            using var conn = db.ObtenerConexion();
            conn.Open();

            using var cmd = AyudanteSql.Crear(conn, null,
                @"SELECT s.id_servicio, s.codigo_servicio, c.nombres_apellidos, ct.codigo_contrato,
                         s.estado_servicio, s.fecha_actualizacion
                  FROM servicios s
                  JOIN clientes c ON c.id_cliente = s.id_cliente
                  JOIN contratos ct ON ct.id_contrato = s.id_contrato
                  ORDER BY s.id_servicio");
            using var lector = cmd.ExecuteReader();

            while (lector.Read())
            {
                lista.Add(new ServicioResumen
                {
                    IdServicio = lector.GetInt32("id_servicio"),
                    CodigoServicio = lector.LeerTexto("codigo_servicio"),
                    NombreCliente = lector.LeerTexto("nombres_apellidos"),
                    CodigoContrato = lector.LeerTexto("codigo_contrato"),
                    EstadoServicio = Enum.Parse<EstadoServicio>(lector.GetString("estado_servicio")),
                    FechaActualizacion = lector.GetDateTime("fecha_actualizacion")
                });
            }
            return lista;
        }

        /// <inheritdoc />
        public bool CambiarEstado(int idServicio, EstadoServicio nuevoEstado)
        {
            using var conn = db.ObtenerConexion();
            conn.Open();
            using var tx = conn.BeginTransaction();

            int filas;
            using (var cmd = AyudanteSql.Crear(conn, tx,
                "UPDATE servicios SET estado_servicio = @estado, fecha_actualizacion = CURDATE() WHERE id_servicio = @id",
                ("@estado", nuevoEstado.ToString()),
                ("@id", idServicio)))
            {
                filas = cmd.ExecuteNonQuery();
            }
            if (filas == 0)
            {
                return false; // no existe el servicio: se descarta la transacción sin cambios
            }

            // RN-07: el contrato de este servicio queda en el estado que corresponde.
            using (var cmd = AyudanteSql.Crear(conn, tx,
                @"UPDATE contratos SET estado_contrato = @estadoContrato
                  WHERE id_contrato = (SELECT id_contrato FROM servicios WHERE id_servicio = @id)",
                ("@estadoContrato", nuevoEstado.AEstadoContrato().ToString()),
                ("@id", idServicio)))
            {
                cmd.ExecuteNonQuery();
            }

            tx.Commit();
            return true;
        }
    }
}
