# Construcción de Software — Grupo D

**Universidad de Guayaquil · Facultad de Ciencias Matemáticas y Físicas · Carrera de Software**
Asignatura: Construcción de Software (611) · Paralelo SOF-S-NO-6-7 (Virtual) · Ciclo II 2026-2027
Docente: Ph.D. Franklin Ricardo Parrales Bravo

> **Proyecto:** Sistema de Gestión de Clientes, Planes y Facturación para un ISP (proveedor de Internet)
> Centraliza el registro de clientes y contratos, el catálogo de planes y equipos, la facturación con registro de pagos y el control del estado del servicio con soporte básico, para el personal administrativo y técnico de un proveedor de Internet.

Este es el repositorio único del Grupo D. Su enlace es el que se entrega en **todas** las tareas grupales del aula virtual. Cada actividad tiene su propia carpeta, ordenada por **unidad → semana → actividad**, con la lista de lo que pide el docente y los archivos entregados.

---

## Entregas por semana

### Unidad 1 — Principios básicos y tolerancia a fallos (cierre: 17/12/2026)

| Semana | Actividad | Carpeta | Estado |
|:---:|---|---|:---:|
| 2 | Tarea 2 · Alcance del proyecto grupal (Diseño Detallado corregido, alcance, requerimientos, tecnologías) | [abrir](unidad-1/semana-02_T2_alcance-del-proyecto) | ⏳ |
| 3 | Tarea 1 · Avance 1 (código comentado, IDE, avance del artículo) | [abrir](unidad-1/semana-03_T1_avance-1) | ⏳ |
| 4 | Tarea 1 · Avance 2 (+ diseño detallado, avance del artículo) | [abrir](unidad-1/semana-04_T1_avance-2) | 🔄 |
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

El repositorio contiene **dos componentes** dentro de [`proyecto/src`](proyecto/src):

| Carpeta | Qué es | Tecnología |
| :--- | :--- | :--- |
| [`SistemaGestionISP/`](proyecto/src/SistemaGestionISP) | Aplicación de consola con acceso a MySQL (línea principal del proyecto) | C# · .NET 8 · MySQL |
| [`prototipo-web/`](proyecto/src/prototipo-web) | Prototipo de interfaz web de los cuatro módulos (datos de demostración en el navegador) | HTML · JavaScript · Tailwind |

Cada componente trae su propio script de base de datos en su carpeta `database/`. Ver [`proyecto/src/README.md`](proyecto/src/README.md).

## Cómo abrir y ejecutar el proyecto

### A. Aplicación de consola C# (`proyecto/src/SistemaGestionISP`)

**Requisitos previos:** [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) o superior, MySQL Server 8.0 (con MySQL Workbench o el cliente `mysql`) y Visual Studio 2022 (o Visual Studio Code con la extensión *C# Dev Kit*).

1. **Clonar** el repositorio:
   ```bash
   git clone https://github.com/AlanDaniel-arch/UG-CSO-GrupoD-Construccion-de-Software.git
   cd UG-CSO-GrupoD-Construccion-de-Software
   ```
2. **Crear la base de datos.** Con MySQL en ejecución, correr el script (crea la base `isp_gestion`, sus 5 tablas y 3 planes de ejemplo):
   - *MySQL Workbench:* `File → Open SQL Script…` → `proyecto/src/SistemaGestionISP/database/isp_gestion.sql` → ejecutar todo (icono del rayo).
   - *Línea de comandos:* `mysql -u root -p < proyecto/src/SistemaGestionISP/database/isp_gestion.sql`
3. **Configurar la conexión.** Por defecto el programa usa `Server=localhost;Database=isp_gestion;Uid=root;Pwd=root;`. Si tu usuario o contraseña son otros, define la variable de entorno `ISP_DB_CONNECTION` **antes** de abrir el IDE (así la contraseña no queda en el código):
   - *PowerShell:* `$env:ISP_DB_CONNECTION = "Server=localhost;Database=isp_gestion;Uid=root;Pwd=TU_CLAVE;"`
   - *Visual Studio:* `Proyecto → Propiedades de SistemaGestionISP → Depurar → Abrir la interfaz de usuario de perfiles de inicio → Variables de entorno`.
4. **Abrir en el IDE.**
   - *Visual Studio 2022:* `Archivo → Abrir → Proyecto o solución…` y elegir `proyecto/src/SistemaGestionISP/SistemaGestionISP.csproj` (no hay archivo `.sln`). Visual Studio restaura el paquete NuGet `MySql.Data` automáticamente.
   - *Visual Studio Code:* `File → Open Folder…` → `proyecto/src/SistemaGestionISP`.
5. **Ejecutar:** `F5` o `Ctrl+F5` en Visual Studio, o desde una terminal:
   ```bash
   cd proyecto/src/SistemaGestionISP
   dotnet restore
   dotnet run
   ```
   Se muestra un menú de consola con las opciones: registrar cliente, listar clientes, consultar planes y cambiar el estado del servicio.

| Problema frecuente | Causa y solución |
| :--- | :--- |
| `Access denied for user 'root'` | La contraseña no coincide: definir `ISP_DB_CONNECTION` (paso 3). |
| `Unknown database 'isp_gestion'` | No se ejecutó el script del paso 2. |
| `Unable to connect to any of the specified MySQL hosts` | MySQL no está iniciado o no escucha en `localhost:3306`. |

### B. Prototipo web (`proyecto/src/prototipo-web`)

1. Abrir `proyecto/src/prototipo-web/index.html` con doble clic (Chrome, Edge o Firefox). Necesita **conexión a Internet** para cargar Tailwind, íconos, generador de PDF y código QR desde sus CDN.
2. Iniciar sesión con una cuenta de demostración (las tarjetas de la pantalla de ingreso rellenan usuario y clave): `admin`, `gerente`, `secretario`, `cajero` o `tecnico`.
3. *(Opcional)* La base MySQL equivalente a los datos de demostración está en `proyecto/src/prototipo-web/database/base_datos_isp.sql` (crea `isp_gestion_db`); el prototipo no se conecta a ella, guarda sus datos en el navegador.

### C. Pruebas unitarias del prototipo

Con **Node.js 22 o superior**, desde la raíz del repositorio:

```bash
node --test proyecto/tests/prototipo.test.cjs
```

Más detalle en [`proyecto/tests/README.md`](proyecto/tests/README.md). Las pruebas de la aplicación C# (xUnit/MSTest) están previstas para la semana 5.

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
│   ├── src/
│   │   ├── SistemaGestionISP/   App de consola C# (Models, Data, Repositories, database/)
│   │   └── prototipo-web/       Prototipo web (index.html, js/, database/)
│   └── tests/      Pruebas unitarias y plan de pruebas
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
