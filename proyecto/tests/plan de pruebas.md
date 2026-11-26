# Plan de pruebas

Este documento reúne las pruebas funcionales del prototipo web ISP Manager. Cada apartado describe el alcance de sus pruebas, los resultados esperados y el estado de ejecución.

## Pruebas de autenticación y permisos

Verificar el inicio de sesión con cada cuenta de demostración y los permisos de navegación y edición que aplica `auth.js`. Este apartado cubre el menú, los accesos rápidos, los controles de edición y la opción de restaurar los datos de demostración.

### Entorno y precondiciones

- Ejecución del 25 de noviembre de 2026 en Windows, con Google Chrome 158.0.8127.37 en modo sin interfaz.
- Abrir `../src/index.html.html` como archivo local, con un perfil limpio y los datos de demostración. Los datos iniciales permiten comprobar acciones asociadas a filas y tarjetas.
- Comenzar cada caso desde la pantalla de ingreso. Usar las credenciales que muestra la tarjeta de demostración del rol correspondiente.

### Permisos esperados

El panel principal está disponible para los cinco roles. La edición de Soporte incluye el selector de estado del servicio y las acciones de tickets.

| Rol | Módulos visibles | Módulos editables | Accesos rápidos a módulos | Restaurar datos |
| --- | --- | --- | --- | --- |
| Administrador | Clientes, Planes y Equipos, Facturación, Soporte | Todos | Facturación y Soporte | Visible |
| Gerente | Clientes, Planes y Equipos, Facturación, Soporte | Ninguno | Facturación y Soporte | Oculto |
| Secretario | Clientes y Soporte | Clientes y Soporte | Soporte | Oculto |
| Cajero | Facturación | Facturación | Facturación | Oculto |
| Técnico | Planes y Equipos, y Soporte | Planes y Equipos, y Soporte | Soporte | Oculto |

### Procedimiento

1. Introducir las credenciales de la cuenta y enviar el formulario. Verificar que se oculte la pantalla de ingreso, se abra el panel y el encabezado muestre el nombre y el rol esperados.
2. Comparar el menú, los accesos rápidos a módulos y el botón de restauración con la tabla de permisos. Revisar también los botones rápidos de alta de cliente y de factura según sus respectivos permisos de edición.
3. Abrir cada módulo permitido y comprobar los controles de la tabla siguiente. Las acciones autorizadas deben ser visibles. Para consulta, los botones de edición deben estar ocultos y los selectores de estado deben estar deshabilitados.
4. Comprobar que los módulos restringidos estén ocultos en el menú. En la ejecución automatizada, se activó mediante el DOM el botón oculto de cada módulo restringido y se verificó que la navegación volviera al panel.
5. Cerrar sesión y verificar que vuelva a aparecer la pantalla de ingreso antes de probar la siguiente cuenta.

| Módulo | Controles que se revisan |
| --- | --- |
| Clientes | Alta y edición de clientes |
| Planes y Equipos | Alta, edición y cambio de estado de planes. Alta y edición de equipos |
| Facturación | Creación de facturas y registro de pagos |
| Soporte | Cambio de estado del servicio, creación de tickets y avance de su estado |

### Resultados de la ejecución

| ID | Cuenta | Resultado esperado | Resultado observado | Estado |
| --- | --- | --- | --- | --- |
| AUTH-01 | `admin` | Acceso al panel como Administrador del Sistema, rol Administrador | Panel abierto y encabezado correcto | Aprobado |
| AUTH-02 | `admin` | Consulta y edición en los cuatro módulos | Menú y accesos rápidos según la matriz. Controles de edición visibles y opción de restauración visible | Aprobado |
| AUTH-03 | `gerente` | Acceso al panel como Gerente General, rol Gerente | Panel abierto y encabezado correcto | Aprobado |
| AUTH-04 | `gerente` | Consulta en los cuatro módulos | Menú y accesos rápidos según la matriz. Botones de edición ocultos, selectores de servicio deshabilitados y opción de restauración oculta | Aprobado |
| AUTH-05 | `secretario` | Acceso al panel como Secretaría, rol Secretario | Panel abierto y encabezado correcto | Aprobado |
| AUTH-06 | `secretario` | Consulta y edición en Clientes y Soporte | Controles autorizados visibles y selectores de servicio habilitados. Planes y Equipos, y Facturación ocultos. Acceso rápido solo a Soporte y opción de restauración oculta | Aprobado |
| AUTH-07 | `cajero` | Acceso al panel como Caja y Cobranza, rol Cajero | Panel abierto y encabezado correcto | Aprobado |
| AUTH-08 | `cajero` | Consulta y edición en Facturación | Facturación fue el único módulo visible además del panel. Controles de factura y pago visibles, acceso rápido a Facturación y opción de restauración oculta | Aprobado |
| AUTH-09 | `tecnico` | Acceso al panel como Soporte Técnico, rol Técnico | Panel abierto y encabezado correcto | Aprobado |
| AUTH-10 | `tecnico` | Consulta y edición en Planes y Equipos, y Soporte | Controles autorizados visibles y selectores de servicio habilitados. Clientes y Facturación ocultos. Acceso rápido solo a Soporte y opción de restauración oculta | Aprobado |

**Balance.** 10 casos ejecutados y aprobados. Los módulos restringidos redirigieron al panel en los tres roles con restricciones de navegación. El cierre de sesión devolvió a la pantalla de ingreso después de cada rol.

## Pruebas de Clientes

**Estado.** Pendiente de ejecución.

Verificar el alta y la edición de clientes, los campos obligatorios, la cancelación y la persistencia de los cambios al recargar.

## Pruebas de Planes y Equipos

**Estado.** Pendiente de ejecución.

Verificar el alta y la edición de planes y equipos, los campos obligatorios, la cancelación y la persistencia de los cambios al recargar.

## Pruebas de Facturación

**Estado.** Pendiente de ejecución.

Verificar la creación de facturas, el registro de pagos, los campos obligatorios y la actualización de los registros de facturas y pagos.

## Pruebas de Soporte

**Estado.** Pendiente de ejecución.

Verificar la creación de tickets, los campos obligatorios y la actualización del estado del ticket y del servicio.

