# U1 · Semana 3 · Tarea 1 (GRUPAL)
## Desarrollo de proyecto: entrega del avance 1 del proyecto grupal

| | |
|---|---|
| Cierre en el aula virtual | jueves 17 de diciembre de 2026, 05:09 |
| Tiempo estimado | 7 horas por integrante |
| Estado | ✅ Completado |
| Tag de la entrega | _se crea al cerrar la entrega_ (propuesto: `U1-S03-avance-1`) |

**Qué es el Avance 1 según el documento maestro V2 (§12.2):** el **prototipo funcional** del sistema: los cuatro módulos y el control de acceso por roles, ya probado (16 pruebas unitarias y 27 funcionales). Está en [`/proyecto/src/prototipo-web`](../../proyecto/src/prototipo-web).

### Qué pide la actividad (lista de control)

- [x] Avance del **artículo** de investigación → [`Articulo_Avance1_GrupoD.docx`](Articulo_Avance1_GrupoD.docx) + [`.pdf`](Articulo_Avance1_GrupoD.pdf)
- [x] Código fuente **comentado**, en [`/proyecto/src`](../../proyecto/src) (prototipo web)
- [x] Instrucciones detalladas para abrir el proyecto en el IDE → [README principal](../../README.md#cómo-abrir-y-ejecutar-el-proyecto)
- [x] Bitácora con los módulos implementados y su **porcentaje de avance** → [`BIT_02_GRUPO-D_25112026`](../../bitacoras/BIT_02_GRUPO-D_25112026.pdf) *(ver la nota de corrección más abajo)*

### Responsables

| Parte | Integrante(s) | Estado |
|---|---|---|
| Avance 1 e integración del repositorio | Castillo Torres Aaron Andrés (`@aa1218-a`) | Completado |
| Prototipo web: separación en `app.js`, `auth.js` y `forms.js`, pruebas unitarias y funcionales | Monge Dorado Esther Betzabeth (`@esthermonge1515-beep`) — PR #2, #3 y #4 | Completado |

### Historia y nota de corrección (25/11/2026)

La primera versión de esta entrega describía una **aplicación de consola en C# con una base `isp_gestion` de 5 tablas** (porcentajes 30/25/15/40 %). Al publicarse el documento maestro V2, el grupo adoptó una **única base de datos de 10 tablas** y definió el prototipo web como Avance 1. Por eso:

- El artículo del Avance 1 se **reescribió** para alinearse con el V2 (el original sigue en el historial de Git).
- El código C# de Aaron **no se perdió**: se adaptó al esquema del V2 y es hoy la base de la implementación objetivo ([`/proyecto/src/SistemaGestionISP`](../../proyecto/src/SistemaGestionISP)).
- La bitácora `BIT_02` **no se modificó** (es el acta de lo hablado ese día). Sus porcentajes quedan reemplazados por los del V2 (Tabla 32), recogidos en [`BIT_03`](../../bitacoras/BIT_03_GRUPO-D_25112026.pdf).

### Archivos de esta entrega

- [`Articulo_Avance1_GrupoD.docx`](Articulo_Avance1_GrupoD.docx) · [`.pdf`](Articulo_Avance1_GrupoD.pdf)
- Código: [`/proyecto/src/prototipo-web`](../../proyecto/src/prototipo-web) · Pruebas: [`/proyecto/tests`](../../proyecto/tests)
