# Code Smells y Refactorización — Avance 2 (U1 · S4)

**Proyecto:** Sistema de Gestión de Clientes, Planes y Facturación para ISP · **Grupo D** · Construcción de Software (611)
**Fecha de revisión:** 25/11/2026 · **Responsable:** Gutiérrez Muñoz Alan Daniel (`@AlanDaniel-arch`)
**Referencias:** Fowler, *Refactoring* (2.ª ed.) — nombres de smells y técnicas · Martin, *Clean Code* — nombres, funciones, clases y manejo de errores · Principios SOLID.

## 1. Alcance de la revisión

| Componente | Archivos revisados | Autor del código |
|---|---|---|
| App de consola C# | `Program.cs`, `Data/ConexionBD.cs`, `Models/Entidades.cs`, `Services/GestionISPService.cs` (Avance 1) | Castillo Torres Aaron |
| Prototipo web | `index.html`, `app.js`, `forms.js`, `auth.js` | Subido por el grupo; separación en archivos: Monge Dorado Esther (PR #2) |
| Bases de datos | `isp_gestion.sql` (C#) y `base_datos_isp.sql` (prototipo) | — |

Las refactorizaciones **aplicadas** en esta entrega están en la app C# (donde se concentra el trabajo del Avance 2) y en la organización de archivos. Del prototipo web se aplicaron solo cambios que no alteran su comportamiento; el resto queda registrado como pendiente con su técnica.

## 2. Resumen

| # | Code smell (Fowler / Clean Code) | Dónde | Principio | Técnica de refactorización | Estado |
|:-:|---|---|:-:|---|:-:|
| C1 | **Large Class** / Divergent Change | `GestionISPService` (clientes, planes y estado de servicio en una clase) | SRP | *Extract Class* → `ClienteRepositorio` y `PlanRepositorio` | ✅ Aplicada |
| C2 | **Dependencia directa de una clase concreta** | `new ConexionBD()` dentro del servicio | DIP | *Extract Interface* (`IConexionBD`, `IClienteRepositorio`, `IPlanRepositorio`) + inyección por constructor | ✅ Aplicada |
| C3 | **Hard-coded secret / Magic String** | Cadena de conexión con `Pwd=root` en `ConexionBD` | — | *Extract Constant* + *Introduce Parameter*; la variable de entorno `ISP_DB_CONNECTION` reemplaza el valor por defecto | ✅ Aplicada |
| C4 | **Long Method** | `Program.Main` (≈80 líneas: menú, lectura, validación y salida en un `switch`) | SRP | *Extract Method*: `MostrarMenu`, `LeerOpcion`, `Ejecutar`, `RegistrarCliente`, `ListarClientes`, `ConsultarPlanes`, `CambiarEstadoServicio` | ✅ Aplicada |
| C5 | **Magic Number** | `opcion != 5`; `reader.IsDBNull(4)` / `(5)` (posición de columna) | — | *Replace Magic Number with Symbolic Constant* (`enum OpcionMenu`); lectura de columnas con `GetOrdinal("nombre")` | ✅ Aplicada |
| C6 | **Primitive Obsession** | Estado del servicio como `string` libre (`"Activo"`…) | OCP | *Replace Type Code with Enum* (`EstadoServicio`) | ✅ Aplicada |
| C7 | **Duplicated Code** | `Console.Write(...)` + `ReadLine() ?? string.Empty` repetido 7 veces | DRY | *Extract Method* `LeerTexto(mensaje)` | ✅ Aplicada |
| C8 | **Varias clases en un archivo** | `Entidades.cs` (4 clases) | — | *Move Class*: un archivo por clase | ✅ Aplicada |
| C9 | **Fallo silencioso** (Clean Code, cap. 7) | `Main` ignoraba el `false` de `RegistrarCliente` y de `ActualizarEstadoServicio`; un estado nulo se reemplazaba por `"Activo"` | — | Mensajes de éxito/error explícitos; el estado escrito se valida con `Enum.TryParse` antes de llegar a MySQL | ✅ Aplicada |
| C10 | **Manejo de excepciones ausente** | Si MySQL no responde, la consola termina con una excepción sin controlar | — | *Introduce Exception Handling* + excepciones propias + reintentos | ⏳ Rama `jefferson/u1-s6-tolerancia-fallos` (S6) |
| C11 | **Speculative Generality / Dead Code** | `Factura` y `TicketSoporte` existen sin repositorio (Módulos 3 y 4); `TicketSoporte.Estado` sigue como `string` | — | Mantener (están en el alcance); aplicar C6 a tickets cuando se implemente el repositorio | ⏳ Pendiente |
| J1 | **Datos de prueba/credenciales en el cliente** | `auth.js`: claves en texto plano y visibles en las tarjetas de ingreso | — | *Move to server*: autenticación con hash con sal (la tabla `usuarios` ya tiene `clave_hash`) | ⏳ Pendiente (es demostración) |
| J2 | **Cross-Site Scripting (XSS)** | `app.js` / `forms.js`: valores del usuario se insertan con `innerHTML` sin escapar | — | *Extract Method* `escapeHtml()` y uso de `textContent` | ⏳ **Prioridad alta** |
| J3 | **Global Data** (estado mutable compartido) | `state`, `currentUser`, `currentModalHandler` globales entre tres scripts | SRP | *Encapsulate Variable* / módulos ES (`import`/`export`) | ⏳ Pendiente |
| J4 | **Duplicated Code** | Marcado de campos repetido en los formularios; tres bloques casi idénticos de subpestañas en `app.js` | DRY | *Extract Method* (`campoTexto()`, reutilizar `bindSubtabs`) | ⏳ Pendiente |
| J5 | **Long Method** | `exportPDF` (≈80 líneas), `renderDashboard`, `renderSoporte` | SRP | *Extract Method* por sección | ⏳ Pendiente |
| J6 | **Magic Strings / Numbers** | `'Activo'`, `'Pagada'`… repetidos; IDs con `101 + length`, `51 + length`; valor por defecto `35.00` | — | *Replace Magic Number with Symbolic Constant*; generar IDs con un contador, no con `length` | ⏳ Pendiente |
| J7 | **Datos personales en el código** | Teléfono y correo institucional de un integrante en `CONTACTO` (`app.js`) y en el seed SQL, en un repositorio público | — | *Extract Configuration*; usar datos de contacto genéricos | ⏳ Pendiente (decidir con el grupo) |
| J8 | **Nombres de archivo incorrectos** | `index.html.html`, `base_datos_isp..sql` | — | *Rename* → `index.html`, `base_datos_isp.sql` | ✅ Aplicada |

## 3. Refactorizaciones aplicadas (código)

### C1 + C2 — Extraer repositorios e interfaces (SRP, DIP)

**Antes** (`Services/GestionISPService.cs`): una sola clase con las consultas de los Módulos 1, 2 y 4, que creaba su propia conexión:

```csharp
public class GestionISPService
{
    private readonly ConexionBD db = new ConexionBD();   // dependencia concreta
    public bool RegistrarCliente(Cliente cliente) { ... }
    public List<Cliente> ListarClientes() { ... }
    public List<PlanInternet> ListarPlanes() { ... }
    public bool ActualizarEstadoServicio(int idCliente, string nuevoEstado) { ... }
}
```

**Después** (`Repositories/`): cada clase trabaja con una tabla y recibe la conexión por constructor:

```csharp
public class ClienteRepositorio : IClienteRepositorio
{
    private readonly IConexionBD db;
    public ClienteRepositorio(IConexionBD db) { this.db = db; }
    public bool Registrar(Cliente cliente) { ... }
    public List<Cliente> Listar() { ... }
    public bool ActualizarEstadoServicio(int idCliente, EstadoServicio nuevoEstado) { ... }
}
```

**Por qué:** una clase con motivos de cambio distintos (clientes, planes, estados) crece sin control al agregar Contrato, Equipo, Factura y Ticket en los siguientes avances. Con una clase por tabla, cada módulo se amplía sin tocar a los demás (SRP, OCP). Depender de `IConexionBD` permite sustituir la conexión por un doble de prueba en las pruebas unitarias de la semana 5 (DIP).

### C3 — Credenciales fuera del código

**Antes:** `private readonly string cadenaConexion = "Server=localhost;Database=isp_gestion;Uid=root;Pwd=root;";`

**Después:** la clase lee la variable de entorno `ISP_DB_CONNECTION`; si no existe, usa la cadena de desarrollo. Cada integrante configura su contraseña sin subirla al repositorio (ver README principal, paso 3).

### C4 + C5 + C7 + C9 — `Program.cs`

**Antes:** `Main` mezclaba dibujo del menú, lectura de datos, llamadas a la base y mensajes. Cualquier cambio de un módulo obligaba a editar el mismo método, y la condición de salida dependía del número `5` escrito a mano.

**Después:**

```csharp
OpcionMenu opcion;
do
{
    MostrarMenu();
    opcion = LeerOpcion();
    Ejecutar(opcion, clientes, planes);
    if (opcion != OpcionMenu.Salir) { /* pausa */ }
} while (opcion != OpcionMenu.Salir);
```

Cada opción tiene su método y todos los mensajes distinguen éxito y error. Una opción inválida ahora avisa al usuario en lugar de redibujar el menú sin explicación.

### C6 — `EstadoServicio` como enumeración

**Antes:** `cliente.EstadoServicio` era un `string`; escribir `"suspendido "` o `"Pausado"` llegaba a MySQL y fallaba en la base. **Después:** `enum EstadoServicio { Activo, Suspendido, Cancelado }`, con los mismos nombres que el `ENUM` de la columna; el valor escrito por el usuario se valida en `CambiarEstadoServicio` (`Enum.TryParse` + `Enum.IsDefined`).

### C8 — Un archivo por clase

`Models/Entidades.cs` se dividió en `Cliente.cs`, `PlanInternet.cs`, `Factura.cs`, `TicketSoporte.cs` y `EstadoServicio.cs`.

### J8 y organización del código

`proyecto/src` ahora separa los dos componentes (`SistemaGestionISP/` y `prototipo-web/`). Los scripts del prototipo pasaron a `prototipo-web/js/`, la base de datos a `prototipo-web/database/`, y se actualizaron las referencias en `index.html`, en las pruebas (`prototipo.test.cjs`) y en el plan de pruebas.

## 4. Principios SOLID — estado tras la revisión

| Principio | App C# antes | App C# después | Prototipo web |
|---|---|---|---|
| **S** — responsabilidad única | ❌ servicio con 3 módulos; `Main` hace todo | ✅ un repositorio por tabla; `Program` solo interactúa | ⚠️ `app.js` mezcla estado, vistas, PDF y contacto |
| **O** — abierto/cerrado | ⚠️ estados como texto | ✅ enum de estados; nuevos módulos = nuevas clases | ⚠️ `badge()` y `can()` se editan por cada estado/rol nuevo |
| **L** — sustitución de Liskov | n/a (sin herencia) | n/a | n/a |
| **I** — segregación de interfaces | n/a | ✅ interfaces pequeñas por módulo | n/a |
| **D** — inversión de dependencias | ❌ `new ConexionBD()` | ✅ se depende de `IConexionBD` | ⚠️ funciones acceden directamente a `state` global |

## 5. Revisión de las bases de datos

Se revisaron ambos scripts; los dos se validaron con un analizador de sintaxis MySQL sin errores (`isp_gestion.sql`: 8 sentencias; `base_datos_isp.sql`: 22).

| # | Hallazgo | Detalle | Acción |
|:-:|---|---|---|
| D1 | **Dos esquemas distintos para el mismo alcance** | `isp_gestion` (5 tablas) vs `isp_gestion_db` (10 tablas): `cedula` / `identificacion`; `nombres` + `apellidos` / `nombres_apellidos`; velocidad `INT` / `VARCHAR`; estado del servicio en `clientes` / tabla `servicios` | ⏳ Unificar con el Diseño Detallado v2 |
| D2 | **Valores de estado inconsistentes** | Tickets: `En Proceso`/`Cerrado` (C#) vs `En proceso`/`Resuelto` (prototipo). Servicio: `Cancelado` vs `Pendiente` | ⏳ Definir un catálogo único |
| D3 | **`isp_gestion` no cubre el alcance** | No existen tablas de equipos, pagos ni usuarios (el Módulo 2 pide equipos; el Módulo 3, registro de pagos; el foro S6, login). `facturas.pagada` no guarda fecha ni método de pago | ⏳ Se resolverá con D1 |
| D4 | **Redundancia (3FN)** | En `base_datos_isp.sql`, `facturas`, `pagos` y `servicios` guardan `id_cliente` aunque ya se obtiene desde `id_contrato`; puede quedar inconsistente | ⏳ Quitar o justificar al unificar |
| D5 | **Script no repetible** | `isp_gestion.sql` duplicaba los 3 planes al ejecutarse dos veces | ✅ `UNIQUE(nombre_plan)` + `INSERT IGNORE` |
| D6 | **Contraseñas con `SHA2` sin sal** | `usuarios.clave_hash` en el seed del prototipo | ⏳ Usar bcrypt/Argon2 en la implementación real |
| D7 | **Datos personales reales en el seed** | Teléfono y correo institucional de un integrante (ver J7) | ⏳ Reemplazar por datos genéricos |
| D8 | Sin `CHARACTER SET` explícito en `isp_gestion.sql` | MySQL 8 usa `utf8mb4` por defecto; el prototipo sí lo declara | Sin cambio; igualar al unificar |

Puntos correctos: ambos esquemas tienen claves primarias y foráneas, `UNIQUE` en los identificadores de negocio (cédula, códigos) y consultas del código C# **parametrizadas** (sin riesgo de inyección SQL).

## 6. Verificación

- [x] Pruebas del prototipo después de reorganizar y comentar: `node --test proyecto/tests/prototipo.test.cjs` → **16 de 16 aprobadas**.
- [x] Sintaxis de los 12 archivos `.cs` y de los dos scripts SQL revisada con analizador.
- [ ] **Compilar y ejecutar la app C# en Visual Studio** (`dotnet build` / `F5`) antes de cerrar la entrega: la refactorización se escribió y revisó, pero el entorno de trabajo no tenía el SDK de .NET.

## 7. Siguiente paso

1. Unificar los esquemas de base de datos (D1–D4) cuando el Diseño Detallado v2 esté en el repositorio.
2. Integrar el manejo de excepciones de S6 sobre esta estructura de repositorios (C10). La rama de S6 modifica los mismos archivos (`ConexionBD`, `Program`, servicio), por lo que hay que resolver conflictos al unir.
3. Escapar HTML en el prototipo (J2) y completar los repositorios de Contrato, Equipo, Factura y Ticket aplicando C1–C6 desde el inicio.
4. Confirmar la tecnología con el grupo: el documento de alcance de la semana 2 describe una aplicación web con ASP.NET Core MVC, mientras que el código actual es una aplicación de consola (C#) y un prototipo web en JavaScript.
