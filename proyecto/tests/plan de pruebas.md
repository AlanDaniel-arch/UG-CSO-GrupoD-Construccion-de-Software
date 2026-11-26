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

### Entorno y procedimiento común para los cuatro módulos

La ejecución del 25 de noviembre de 2026 usó Chrome 158.0.8127.37 en modo sin interfaz, con un perfil limpio y `../src/index.html.html` abierto como archivo local. Se usaron los datos de demostración y se inició sesión con el rol indicado en cada apartado.

En cada alta o edición, se completó el formulario, se vació el campo obligatorio indicado y se intentó guardar. Chrome bloqueó el envío mediante su validación nativa y el almacenamiento quedó igual. Al pulsar **Cancelar**, se cerró el formulario sin guardar. Luego se abrió nuevamente, se completó y se guardó. Para comprobar persistencia, se recargó la misma pestaña y se consultaron los registros creados.

**Rol.** Secretario.

| ID | Pasos | Resultado esperado | Resultado observado | Estado |
| --- | --- | --- | --- | --- |
| CLI-01 | Crear el cliente `0999999911` con nombre Cliente QA, dirección Centro, teléfono `0990000000` y correo `qa@example.com`. Probar el nombre vacío y la cancelación. | El nombre vacío impide guardar. Cancelar conserva los datos. Un envío válido añade al cliente. | Se bloqueó el envío incompleto, la cancelación conservó los datos y el cliente apareció en el listado. | Aprobado |
| CLI-02 | Abrir el mismo cliente y cambiar su nombre a Cliente QA Editado. Probar la dirección vacía y la cancelación. | Se bloquea el envío incompleto. Cancelar descarta el cambio. Guardar actualiza el cliente. | Pasaron la validación y la cancelación. El listado mostró el nombre actualizado. | Aprobado |
| CLI-03 | Recargar la pestaña y abrir Clientes. | El listado conserva el nombre actualizado. | Apareció Cliente QA Editado. | Aprobado |

## Pruebas de Planes y Equipos

**Rol.** Técnico.

| ID | Pasos | Resultado esperado | Resultado observado | Estado |
| --- | --- | --- | --- | --- |
| PLA-01 | Crear el plan `PLN-QA` con nombre Fibra QA, velocidad 100 Mbps, precio 29,50 y descripción Plan de prueba. Probar el precio vacío y la cancelación. | El precio vacío impide guardar. Cancelar conserva los datos. Guardar añade el plan. | Pasaron la validación y la cancelación. Fibra QA apareció en la vista de planes. | Aprobado |
| PLA-02 | Editar `PLN-QA` y cambiar el nombre a Fibra QA Editada. Probar la velocidad vacía y la cancelación. | Se bloquea el envío incompleto. Cancelar descarta el cambio. Guardar actualiza el plan. | Pasaron la validación y la cancelación. Se mostró Fibra QA Editada. | Aprobado |
| PLA-03 | Cambiar el estado de `PLN-QA`. | El plan pasa a Inactivo y el cambio se guarda. | La vista mostró Inactivo y se guardó ese estado para el plan de prueba. | Aprobado |
| PLA-04 | Recargar la pestaña y volver a Planes. | Se conservan el nombre editado y el estado Inactivo. | El nombre editado apareció en la vista y el estado siguió guardado. | Aprobado |
| EQU-01 | Crear el equipo `EQ-QA` con marca o modelo Router QA y serie `SER-QA`. Probar la serie vacía y la cancelación. | La serie vacía impide guardar. Cancelar conserva los datos. Guardar añade el equipo. | Pasaron la validación y la cancelación. Router QA apareció en el listado. | Aprobado |
| EQU-02 | Editar `EQ-QA` con el nombre Router QA Editado y el estado En mantenimiento. Probar el tipo vacío y la cancelación. | Se bloquea el envío incompleto. Cancelar descarta el cambio. Guardar actualiza el equipo. | Pasaron la validación y la cancelación. El listado mostró En mantenimiento en la fila del equipo. | Aprobado |
| EQU-03 | Recargar la pestaña y abrir Equipos Asociados. | Se conservan el nombre editado y el estado En mantenimiento. | La vista mostró Router QA Editado y el estado siguió guardado. | Aprobado |

## Pruebas de Facturación

**Rol.** Cajero.

| ID | Pasos | Resultado esperado | Resultado observado | Estado |
| --- | --- | --- | --- | --- |
| FAC-01 | Emitir `FAC-QA` para el cliente de demostración `0918743310` por 41,25, con emisión el 25 de noviembre y vencimiento el 5 de diciembre de 2026. Probar el valor vacío y la cancelación. | El valor vacío impide guardar. Cancelar conserva los datos. La factura válida aparece como Pendiente. | Pasaron la validación y la cancelación. `FAC-QA` apareció como Pendiente. | Aprobado |
| FAC-02 | Registrar el pago `PAG-QA` de `FAC-QA` en efectivo el 25 de noviembre de 2026. Probar la fecha vacía y la cancelación. | Se bloquea el envío incompleto. Cancelar conserva la factura pendiente. Guardar marca la factura como Pagada y añade el pago al historial. | Pasaron la validación y la cancelación. La factura apareció como Pagada, dejó de ofrecer la acción de pago y `PAG-QA` apareció en el historial. | Aprobado |
| FAC-03 | Recargar la pestaña y consultar Facturas e Historial de Pagos. | La factura conserva el estado Pagada y el pago figura en el historial. | Se encontraron `FAC-QA` como Pagada y `PAG-QA` en el historial. | Aprobado |

## Pruebas de Soporte

**Rol.** Técnico.

| ID | Pasos | Resultado esperado | Resultado observado | Estado |
| --- | --- | --- | --- | --- |
| SOP-01 | Crear el ticket `SOP-QA` para el cliente `0918743310`, con fecha 25 de noviembre de 2026 y descripción Incidencia QA. Probar la descripción vacía y la cancelación. | La descripción vacía impide guardar. Cancelar conserva los datos. El ticket válido aparece y se guarda como Abierto. | Pasaron la validación y la cancelación. Incidencia QA apareció en el listado y el ticket se guardó como Abierto. | Aprobado |
| SOP-02 | Avanzar dos veces el estado de `SOP-QA`. | Pasa de Abierto a En proceso y luego a Resuelto. Al resolverse, desaparece la acción de avanzar. | Se guardaron ambas transiciones en ese orden. El ticket resuelto dejó de ofrecer la acción. | Aprobado |
| SOP-03 | Cambiar a Suspendido el servicio del cliente `0918743310`. | El selector refleja el cambio y los estados del servicio y del contrato se actualizan. | El selector mostró Suspendido y ambos estados se guardaron como Suspendido. | Aprobado |
| SOP-04 | Recargar la pestaña y consultar Solicitudes de Soporte y Estado de Servicios. | Se conservan el ticket resuelto y el servicio suspendido. | Incidencia QA siguió en el listado con estado Resuelto guardado y el selector del servicio mostró Suspendido. | Aprobado |

**Balance de los cuatro módulos.** 17 casos ejecutados y aprobados.
