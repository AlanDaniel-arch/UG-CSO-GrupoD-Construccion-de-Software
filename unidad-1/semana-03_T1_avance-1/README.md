# U1 · Semana 3 · Tarea 1 (GRUPAL)
## Desarrollo de proyecto: entrega del avance 1 del proyecto grupal

| Detalle | Valor |
| :--- | :--- |
| Cierre en el aula virtual | jueves 17 de diciembre de 2026, 05:09 |
| Tiempo | 7 horas de integración |
| Estado | ✅ Completado (en revisión PR #1) |
| Tag de la entrega | se crea al cerrar la entrega |

### Qué pide la actividad (lista de control)

- [x] Avance del Artículo de investigación -> `Articulo_Avance1_GrupoD.docx` + `.pdf`
- [x] Resumen de lo implementado en este avance y lo pendiente por hacer
- [x] Código fuente comentado, en `/proyecto/src`
- [x] Instrucciones para abrir y ejecutar el proyecto en el IDE (sección «Cómo abrir y ejecutar el proyecto» del `README.md` principal)
- [x] Bitácora de actividades con los módulos/componentes implementados y su porcentaje de avance, en `/bitacoras`

### Responsables

| Parte | Integrante(s) | Estado |
| :--- | :--- | :--- |
| Código fuente inicial, BD, Artículo y Bitácora | Castillo Torres Aaron Andrés (`@aa1218-a`) | Completado |

## Resumen de lo implementado en este avance (U1 - S3)
- Base de datos relacional (`database/isp_gestion.sql`): Creación de las 5 tablas del sistema ISP (`clientes`, `planes_internet`, `contratos`, `facturas`, `tickets_soporte`) junto con datos semilla iniciales de planes comerciales.
- Estructura modular en C# (`/proyecto/src/SistemaGestionISP`): Implementación de los componentes `Models`, `Data`, `Services` (capa de servicios con acceso a datos) y `Program.cs` debidamente comentados.
- Funcionalidades operativas por módulo:
  - Módulo 1 – Gestión de Clientes y Contratos (30 %): Tabla `clientes`, entidad `Cliente` y CRUD de registro y listado operativos (faltan entidad/CRUD de Contrato y **actualización de clientes).
  - Módulo 2 – Gestión de Planes y Equipos (25 %): Tablas `planes_internet` y `contratos`, datos semilla y consulta de catálogo operativos (falta gestión de Equipos).
  - Módulo 3 – Facturación y Registro de Pagos (15 %): Tabla `facturas` y modelo `Factura` definidos.
  - Módulo 4 – Control de Estado de Servicio y Soporte Básico (40 %): Tabla `tickets_soporte` creada y actualización de estado de servicio (`Activo`, `Suspendido`, `Cancelado`) operativa.
- Documentación: Primer avance del artículo técnico (`Articulo_Avance1_GrupoD.docx` y `.pdf`), bitácora de reunión (`BIT_02_GRUPO-D_25112026.docx` y `.pdf`) y guía de ejecución en el `README.md` principal.

## Pendiente para el siguiente avance (U1 - S4)
- Completar las entidades y operaciones CRUD pendientes de **Contrato**, **Equipo** y **actualización de clientes**.
- Implementar el flujo transaccional de emisión de facturas y registro de pagos (Módulo 3) y la gestión de tickets de soporte (Módulo 4).
- Ejecutar análisis de *Code Smells*, refactorización aplicando principios SOLID y manejo robusto de excepciones.

### Archivos de esta entrega

_Se listan aquí con enlace cuando se suban._
