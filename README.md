# Construcción de Software — Grupo D

**Universidad de Guayaquil · Facultad de Ciencias Matemáticas y Físicas · Carrera de Software**
Asignatura: Construcción de Software (611) · Paralelo SOF-S-NO-6-7 (Virtual) · Ciclo II 2026-2027
Docente: Ph.D. Franklin Ricardo Parrales Bravo

> **Proyecto:** _[NOMBRE DEL PROYECTO — pendiente de definir]_
> _[Descripción en 2-3 líneas: qué problema resuelve y para quién.]_

Este es el repositorio único del Grupo D. Su enlace es el que se entrega en **todas** las tareas grupales del aula virtual. Cada actividad tiene su propia carpeta, ordenada por **unidad → semana → actividad**, con la lista de lo que pide el docente y los archivos entregados.

---

## Entregas por semana

### Unidad 1 — Principios básicos y tolerancia a fallos (cierre: 17/12/2026)

| Semana | Actividad | Carpeta | Estado |
|:---:|---|---|:---:|
| 2 | Tarea 2 · Alcance del proyecto grupal (Diseño Detallado corregido, alcance, requerimientos, tecnologías) | [abrir](unidad-1/semana-02_T2_alcance-del-proyecto) | ⏳ |
| 3 | Tarea 1 · Avance 1 (código comentado, IDE, avance del artículo) | [abrir](unidad-1/semana-03_T1_avance-1) | ⏳ |
| 4 | Tarea 1 · Avance 2 (+ diseño detallado, avance del artículo) | [abrir](unidad-1/semana-04_T1_avance-2) | ⏳ |
| 5 | Tarea 1 · Avance 3 con PLAN DE PRUEBAS | [abrir](unidad-1/semana-05_T1_avance-3-plan-de-pruebas) | ⏳ |
| 5 | Foro · Casos de prueba del proyecto (mínimo 5) | [abrir](unidad-1/semana-05_foro_casos-de-prueba) | ⏳ |
| 6 | Tarea 1 · ENTREGA PARCIAL con PLAN DE PRUEBAS y TOLERANCIA A FALLOS (+ artículo) | [abrir](unidad-1/semana-06_T1_entrega-parcial) | ⏳ |
| 6 | Foro · Excepciones propias en el login | [abrir](unidad-1/semana-06_foro_tolerancia-a-fallos) | ⏳ |

### Unidad 2 — Mantenimiento de software (cierre: 24/02/2027)

| Semana | Actividad | Carpeta | Estado |
|:---:|---|---|:---:|
| 9 | Tarea 1 · Avance 5 con PLAN DE MANTENIMIENTO | [abrir](unidad-2/semana-09_T1_avance-5-plan-de-mantenimiento) | ⏳ |
| 10 | Tarea 1 · Avance 6 con PLAN DE MANTENIMIENTO | [abrir](unidad-2/semana-10_T1_avance-6-plan-de-mantenimiento) | ⏳ |

### Unidad 3 — Implementación y documentación

Avance 7, entrega final, artículo y exposición → [abrir](unidad-3)

Leyenda: ⏳ pendiente · 🔄 en progreso · ✅ entregado

---

## Integrantes

| # | Apellidos y nombres | GitHub | Responsabilidad principal |
|:---:|---|---|---|
| 1 | Alvarado Vargas Jostin Paul | | |
| 2 | Castillo Torres Aaron Andrés | | |
| 3 | Gutiérrez Muñoz Alan Daniel | [@AlanDaniel-arch](https://github.com/AlanDaniel-arch) | Organización del repositorio e integración |
| 4 | Martínez Tomalá Marcos Williams | | |
| 5 | Mina Guacho Andrea Rosario | | |
| 6 | Monge Dorado Esther Betzabeth | | |
| 7 | Olvera Suarez Jefferson Lenin | | |
| 8 | Ordoñez Zeas Luis Alberto | | |
| 9 | Requelme Quinto Jostin Jair | | |
| 10 | Tenegusñay Tiban Belinda Elizabeth | | |

El aporte de cada integrante se comprueba en el historial de commits, así que cada uno sube su parte desde su propia cuenta.

---

## Tecnologías

| Componente | Tecnología |
| :--- | :--- |
| Lenguaje | C# (.NET 8.0) |
| IDE | Visual Studio 2022 / Visual Studio Code |
| Framework / Librerías | .NET 8.0, MySql.Data |
| Base de datos | MySQL 8.0 |
| Pruebas unitarias | xUnit / MSTest (previsto en U1-S5) |
| Control de versiones | Git + GitHub |

## Cómo abrir y ejecutar el proyecto

_Requisito de todas las entregas. Se completa con la primera versión del código._

1. Requisitos previos: Tener instalado .NET 8.0 SDK (o superior), MySQL Server 8.0 y Visual Studio 2022.
2. Clonar el repositorio:
   ```bash
   git clone https://github.com/AlanDaniel-arch/UG-CSO-GrupoD-Construccion-de-Software.git
---

## Estructura

```
├── unidad-1/
│   ├── semana-02_T2_alcance-del-proyecto/
│   ├── semana-03_T1_avance-1/
│   ├── semana-04_T1_avance-2/
│   ├── semana-05_T1_avance-3-plan-de-pruebas/
│   ├── semana-05_foro_casos-de-prueba/
│   ├── semana-06_T1_entrega-parcial/
│   └── semana-06_foro_tolerancia-a-fallos/
├── unidad-2/
│   ├── semana-09_T1_avance-5-plan-de-mantenimiento/
│   └── semana-10_T1_avance-6-plan-de-mantenimiento/
├── unidad-3/
├── proyecto/
│   ├── src/        Código fuente comentado (crece semana a semana)
│   └── tests/      Pruebas unitarias
├── bitacoras/      Minutas BIT_NN_GRUPO-D_DDMMAAAA
└── plantillas/     Plantillas del docente
```

Los documentos de cada entrega van en la carpeta de su semana. El código es uno solo y vive en `proyecto/`; su estado en cada entrega queda guardado con un **tag** (ej. `U1-S03-avance-1`).

## Reglas de trabajo

1. **Nadie sube directamente a `main`.** Se trabaja en una rama `nombre/tarea` (ej. `alan/login-excepciones`) y se abre un Pull Request.
2. Cada Pull Request lo revisa **otro integrante** antes de unirlo.
3. Mensajes de commit: `tipo: descripción` → `feat:`, `fix:`, `refactor:`, `test:`, `docs:`.
4. Los documentos de Word se suben en `.docx` **y** en `.pdf` (el PDF se puede leer directamente en GitHub).
5. Cada reunión genera una bitácora (ver [`/bitacoras`](bitacoras)).
6. Fecha interna de cierre: **48 horas antes** de la fecha del aula.
