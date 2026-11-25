# U1 · Semana 3 · Tarea 1 (GRUPAL)
## Desarrollo de proyecto: entrega del avance 1 del proyecto grupal

| | |
|---|---|
| Cierre en el aula virtual | jueves 17 de diciembre de 2026, 05:09 |
| Tiempo estimado | 7 horas por integrante |
| Estado | ⏳ Pendiente |
| Tag de la entrega | _se crea al cerrar la entrega_ |

### Qué pide la actividad (lista de control)

- [x] Avance del **Artículo** de investigación -> `Articulo_Avance1_GrupoD.docx` + `.pdf`
- [x] Resumen de lo implementado en este avance y lo pendiente por hacer
- [x] Código fuente **comentado**, en `/proyecto/src`
- [x] Instrucciones para abrir y ejecutar el proyecto en el IDE (sección «Cómo abrir y ejecutar el proyecto» del `README.md` principal)
- [x] Bitácora de actividades con los módulos/componentes implementados y su porcentaje de avance, en `/bitacoras`

### Responsables

| Parte | Integrante(s) | Estado |
| :--- | :--- | :--- |
| Código fuente inicial, BD, Artículo y Bitácora | Castillo Torres Aaron Andrés (`@aa1218-a`) | Completado |

## Resumen de lo implementado en este avance (U1 - S3)
- **Base de datos relacional (`isp_gestion.sql`):** Creación de las 5 tablas del sistema ISP (`clientes`, `planes_internet`, `contratos`, `facturas`, `tickets_soporte`) junto con datos semilla iniciales de planes comerciales.
- **Arquitectura base en C# (`/proyecto/src/SistemaGestionISP`):** Implementación de la estructura en capas (`Models`, `Data`, `Services` y `Program.cs`) debidamente comentada con documentación XML e inline.
- **Funcionalidades operativas:** Conexión a MySQL mediante `MySql.Data`, registro y listado de clientes (Módulo 1 – 45 % avance), consulta de planes disponibles (Módulo 2 – 35 % avance), estructura base de facturación (Módulo 3 – 15 % avance) y actualización de estado de servicio Activo/Suspendido/Cancelado (Módulo 4 – 40 % avance).
- **Documentación:** Primer avance del artículo técnico (`Articulo_Avance1_GrupoD.docx` y `.pdf`), bitácora de reunión (`BIT_02_GRUPO-D_25112026.docx` y `.pdf`) y guía de ejecución en el `README.md` principal.

## Pendiente para el siguiente avance (U1 - S4)
- Implementar el flujo completo de creación de contratos, generación de facturas y registro de pagos (Módulo 3).
- Implementar la apertura, asignación y cierre de tickets de soporte técnico (Módulo 4).
- Ejecutar análisis de *Code Smells*, refactorización aplicando principios SOLID y manejo avanzado de excepciones (`try-catch` personalizados).

### Archivos de esta entrega

_Se listan aquí con enlace cuando se suban._
