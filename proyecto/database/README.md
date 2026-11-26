# Base de datos del proyecto

**Una sola base de datos para todo el proyecto:** `isp_gestion_db` (MySQL 8.0, `utf8mb4`), con **10 tablas**. Está definida en el documento maestro V2 (§6.4 y Anexo B) y la usan tanto el prototipo como la implementación en C#.

| Archivo | Para qué sirve |
|---|---|
| [`base_datos_isp.sql`](base_datos_isp.sql) | Crea la base `isp_gestion_db`, sus 10 tablas y los datos de demostración. **Es el único script oficial.** |
| [`cambios/CC-01_bloqueo_cuentas.sql`](cambios/CC-01_bloqueo_cuentas.sql) | Cambio CC-01 (bloqueo de cuentas, S6). **Pendiente**, todavía no forma parte del esquema. |

## Cómo crearla

- *MySQL Workbench:* `File → Open SQL Script…` → `proyecto/database/base_datos_isp.sql` → botón del rayo.
- *Terminal:* `mysql -u root -p < proyecto/database/base_datos_isp.sql`

Debe quedar el esquema `isp_gestion_db` con 10 tablas: `usuarios`, `clientes`, `planes`, `contratos`, `equipos`, `facturas`, `pagos`, `servicios`, `soporte_tecnico`, `configuracion_contacto`.

> El script **no es repetible**: vuelve a insertar los datos y falla por las restricciones `UNIQUE`. Para reiniciar, ejecutar antes `DROP DATABASE isp_gestion_db;`.

## Historia (por qué hay un solo script)

Hasta el 25/11/2026 existían dos esquemas: `isp_gestion` (5 tablas, de la primera versión en consola) e `isp_gestion_db` (10 tablas, del prototipo). El documento maestro V2 adoptó el segundo, por lo que el primero se retiró y el código C# se adaptó. Detalle en [`NOTAS_PARA_EL_EQUIPO.md`](../../NOTAS_PARA_EL_EQUIPO.md).
