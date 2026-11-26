# U1 · Semana 6 · Tarea 1 (GRUPAL)
## ENTREGA PROYECTO PARCIAL con PLAN DE PRUEBAS y TOLERANCIA A FALLOS

| | |
|---|---|
| Cierre en el aula virtual | jueves 17 de diciembre de 2026, 05:09 |
| Tiempo estimado | 7 horas por integrante |
| Estado | ⏳ Pendiente |
| Tag de la entrega | _se crea al cerrar la entrega_ |

### Qué pide la actividad (lista de control)

- [ ] **PLAN DE PRUEBAS** del proyecto (versión final del parcial) → `Plan_de_Pruebas_GrupoD_Final.docx` + `.pdf`
- [ ] **DOCUMENTO DE DISEÑO DETALLADO (ACTUALIZADO)** → `Diseno_Detallado_GrupoD_Actualizado.docx` + `.pdf`
- [ ] Alguna **técnica de TOLERANCIA A FALLOS** implementada en el proyecto, documentada en `Tolerancia_a_Fallos.md` (técnica, qué falla controla, clase/método, prueba que la verifica)
- [ ] **Artículo de investigación** → `Articulo_Investigacion_GrupoD.docx` + `.pdf`
- [ ] En la bitácora: porcentaje de avance **parcial** del proyecto
- [ ] Código fuente **comentado**, en [`/proyecto/src`](../../proyecto/src)
- [ ] Instrucciones detalladas para abrir el proyecto en el IDE elegido (sección «Cómo abrir el proyecto» del [README principal](../../README.md#cómo-abrir-y-ejecutar-el-proyecto))
- [ ] Bitácora de actividades del grupo indicando los módulos/componentes implementados y su **porcentaje de avance**, en [`/bitacoras`](../../bitacoras)

### Ya disponible en el repositorio (punto de partida)

- Diseño de la tolerancia a fallos: documento maestro V2 §9 ([PDF](../semana-04_T1_avance-2/Diseno_Detallado_GrupoD_v2.pdf)): mecanismos del prototipo, jerarquía de excepciones propias (usuario inexistente, contraseña incorrecta, cuenta bloqueada) y matriz de fallos.
- **Cambio CC-01 pendiente** (bloqueo tras 3 intentos): [`proyecto/database/cambios/CC-01_bloqueo_cuentas.sql`](../../proyecto/database/cambios/CC-01_bloqueo_cuentas.sql).
- Hay una rama con trabajo previo de excepciones y reintentos: `jefferson/u1-s6-tolerancia-fallos`. **Está basada en la estructura antigua** (consola con base `isp_gestion`): hay que rehacerla sobre `Repositories/` y el esquema `isp_gestion_db` antes de unirla. Ver [`NOTAS_PARA_EL_EQUIPO.md`](../../NOTAS_PARA_EL_EQUIPO.md).
- Los catch vacíos del prototipo (V2 §7.3) deben corregirse como parte de esta entrega.

### Responsables

| Parte | Integrante(s) | Estado |
|---|---|---|
| Plan de pruebas final, tolerancia a fallos y artículo | Mina Guacho Andrea Rosario · Olvera Suarez Jefferson Lenin | ⏳ En preparación (rama de Jefferson por adaptar) |

### Archivos de esta entrega

_Se listan aquí con enlace cuando se suban._
