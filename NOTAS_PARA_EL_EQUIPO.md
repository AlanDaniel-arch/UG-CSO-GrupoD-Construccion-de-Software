# Notas para el equipo — Grupo D

**Léelo antes de tocar el repositorio.** Última actualización: 25/11/2026 por Alan Daniel Gutiérrez (`@AlanDaniel-arch`), administrador del repositorio.

## 1. Qué cambió el 25/11/2026

El **documento maestro V2** (`unidad-1/semana-04_T1_avance-2/Diseno_Detallado_GrupoD_v2.pdf` / `.docx`) pasó a ser la referencia única. Todo el repositorio se alineó con él:

| Antes | Ahora |
|---|---|
| Dos bases de datos (`isp_gestion` de 5 tablas y `isp_gestion_db` de 10) | **Una sola:** `proyecto/database/base_datos_isp.sql` (`isp_gestion_db`, 10 tablas) |
| App C# con modelos propios (cédula, tickets…) | Capa C# reescrita sobre el esquema V2 (`proyecto/src/SistemaGestionISP`) |
| Avance 1 descrito como «app de consola» | Avance 1 = **prototipo web** (V2 §12.2) |
| Porcentajes inventados en BIT_02/03 | Criterio de la Tabla 32 del V2 (M1–M4 = 70 %, Acceso ≈ 65 %) |
| READMEs de semana sin estado | Listas de control, responsables (V2 §12.1) y enlaces al V2 |

## 2. Dónde está cada cosa

- `README.md` — cómo abrir y ejecutar (prototipo, base de datos, pruebas, app C#) y estructura.
- `proyecto/src/README.md` — mapa del código y qué RF cubre cada archivo.
- `proyecto/database/` — script único, README y cambios pendientes (`cambios/CC-01_bloqueo_cuentas.sql`).
- `proyecto/tests/` — 16 pruebas unitarias + plan de 27 casos funcionales.
- `unidad-1/semana-0N_*/README.md` — qué pide cada actividad, quién la hace y su estado.
- `unidad-1/semana-04_T1_avance-2/Code_Smells_y_Refactorizacion.md` — olores del V2 con su estado real.
- `bitacoras/` — BIT_02 (histórica) y BIT_03 (criterio V2).

**Rutas del V2 que quedaron desactualizadas:** el V2 menciona `proyecto/src/index.html.html` y `proyecto/src/base_datos_isp..sql`. Hoy son `proyecto/src/prototipo-web/index.html` y `proyecto/database/base_datos_isp.sql`. No se editó el V2 para no tocar su documento; si lo reeditan, actualicen esas rutas.

## 3. Pendientes y responsables (según V2 §12.1)

| Pendiente | Responsable | Cuándo |
|---|---|---|
| **Compilar** `SistemaGestionISP` y probarlo con MySQL | Alan | 26/11 |
| Completar hora/asistentes de BIT_03; crear BIT_01 | Jostin Requelme | antes de cerrar S4 |
| Plan de pruebas (plantilla del docente) + pruebas C# | Marcos Martínez · Luis Ordoñez | S5 |
| Excepciones propias, CC-01 y catch vacíos | Jefferson Olvera · Andrea Mina | S6 |
| Migración ASP.NET Core MVC (M1 → M4) | por asignar | U1 en adelante |
| Crear tags de entrega (`U1-S03-avance-1`, `U1-S04-avance-2`…) | quien cierre cada entrega | al entregar |

## 4. Advertencias — léanlas

1. **El código C# NO está compilado.** El entorno donde se escribió no tenía el SDK de .NET. Solo se revisó la sintaxis. Puede haber errores de compilación; corríjanlos en un PR pequeño.
2. **La rama de Jefferson (`jefferson/u1-s6-tolerancia-fallos`) va a tener conflictos:** toca los mismos archivos C# que se reorganizaron. Rehacerla sobre `Repositories/` y el esquema nuevo, no hacer merge a ciegas.
3. **Esta alineación se subió directo a `main`** (sin PR) por pedido del administrador, para tener un punto de partida ordenado. A partir de ahora, volver al flujo del V2 §10: rama `nombre/tarea` → PR → revisión.
4. **`main` no está protegida** aunque el V2 dice que sí. Activar la protección en *Settings → Branches*.
5. **Datos personales públicos:** el teléfono y correo de un integrante están en `app.js` (`CONTACTO`) y en el seed SQL de un repositorio público. Decidan si se reemplazan por datos genéricos.
6. **Riesgo XSS en el prototipo** (`innerHTML` sin escapar): es demostración, pero hay que corregirlo antes de migrar.
7. Los porcentajes de avance siguen el criterio del V2; la hora y los asistentes de las bitácoras deben completarlos quienes asistieron.

## 5. Cómo trabajar a partir de ahora

1. `git pull origin main` antes de empezar.
2. Rama: `tunombre/tarea` (ej. `marcos/plan-pruebas`).
3. Comenta tu código (propósito de cada archivo y función pública, V2 §7.1) y no dejes `catch` vacíos.
4. Corre `node --test proyecto/tests/prototipo.test.cjs` antes de abrir el PR.
5. Actualiza el README de la semana (checklist y estado) en el mismo PR.
6. Al cerrar una entrega: tag y nueva bitácora con la tabla de avance.
