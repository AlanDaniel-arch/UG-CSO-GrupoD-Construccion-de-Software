# Code Smells y Refactorización — Avance 2 (U1 · S4)

**Proyecto:** Sistema de Gestión de Clientes, Planes y Facturación para ISP · **Grupo D** · Construcción de Software (611)
**Fecha de revisión:** 25/11/2026 · **Responsable:** Gutiérrez Muñoz Alan Daniel (`@AlanDaniel-arch`)
**Documento base:** [Diseño Detallado v2 (documento maestro)](Diseno_Detallado_GrupoD_v2.pdf), §7.3, Tabla 20. Este archivo amplía esa tabla con evidencia y con lo aplicado en el código.
**Referencias:** Fowler, *Refactoring* (2.ª ed.) · Martin, *Clean Code* · Principios SOLID.

> **Para el equipo.** La Tabla 20 del V2 lista 8 olores del prototipo. Aquí se conservan con la misma redacción (códigos **V2-1 … V2-8**), se agregan los hallazgos propios de la revisión (**E**: código C#, **J**: prototipo, **D**: base de datos) y se indica qué se corrigió ya en el repositorio.

## 1. Alcance de la revisión

| Componente | Ubicación | Autor |
|---|---|---|
| Prototipo web (Avance 1) | `proyecto/src/prototipo-web/` (`index.html`, `js/app.js`, `js/forms.js`, `js/auth.js`) | Grupo; separación en tres scripts: Monge Dorado Esther (PR #2) |
| Capa de datos C# (inicio de la implementación objetivo) | `proyecto/src/SistemaGestionISP/` | Castillo Torres Aaron (base); realineada al V2 por Gutiérrez Muñoz Alan el 25/11/2026 |
| Base de datos única | `proyecto/database/base_datos_isp.sql` | Equipo de diseño (V2 Anexo B) |

## 2. Catálogo de olores de la Tabla 20 del V2 y su estado real

| Código | Code smell | Evidencia | Técnica | Estado V2 | Estado en el repositorio |
|:-:|---|---|---|:-:|---|
| V2-1 | Clase o archivo grande (*Large Class*) | Toda la lógica estaba en `index.html`; `app.js` genera el HTML de cinco vistas (567 líneas) | *Extract Class*, *Move Function*; una vista y un controlador por módulo en MVC | Parcial | **Parcial.** Scripts separados (PR #2). En C# se evitó el problema desde el inicio: un repositorio por tabla |
| V2-2 | Datos globales (*Global Data*) | `state` y `currentUser` globales, modificados por tres scripts | *Encapsulate Variable*; repositorios y sesión del servidor | Pendiente | **Pendiente** (se resuelve en ASP.NET) |
| V2-3 | Obsesión por primitivos | Estados `'Activo'`, `'Pendiente'`, `'Pagada'`… escritos 37 veces como cadenas | *Replace Primitive with Object*: enumeraciones | Pendiente | **Aplicado en C#:** `EstadoPlan`, `EstadoContrato`, `EstadoServicio` (con `AEstadoContrato()`, regla RN-07). Falta `EstadoFactura` y `EstadoTicket`. Prototipo: pendiente |
| V2-4 | Código duplicado | Los modales de cliente, plan, equipo, factura y ticket repiten el marcado de campos | *Extract Function* / vistas parciales Razor | Pendiente | **Pendiente** |
| V2-5 | Excepciones silenciadas | Cinco bloques `catch (e) {}` vacíos en `auth.js` y `app.js` | Registrar el error y avisar; excepciones propias (§9) | Pendiente | **Pendiente, entrega S6.** En C# no hay catch vacíos: `Program.cs` distingue error 1062 (RN-01) de los demás |
| V2-6 | Datos sensibles en el cliente | Usuarios y contraseñas de demostración en texto plano en `auth.js` | *Move Field* a `usuarios.clave_hash` | Parcial | **Parcial** (la tabla ya modela `clave_hash`; la validación sigue en el cliente) |
| V2-7 | Lógica frágil de códigos | Códigos calculados con `length + 1` | *Substitute Algorithm*: AUTO_INCREMENT | Pendiente | **Aplicado en C#:** `RegistrarConContrato` genera el código desde el `AUTO_INCREMENT` dentro de una transacción. Prototipo: pendiente |
| V2-8 | Dato redundante | `pagos.id_cliente` deducible de `facturas.id_cliente` | Reestructuración de datos | Pendiente | **Pendiente** (decisión en Unidad 2) |

## 3. Hallazgos adicionales de la revisión

| Código | Code smell | Dónde | Técnica | Estado |
|:-:|---|---|---|:-:|
| E1 | Dependencia de clase concreta (DIP) | El servicio original creaba `new ConexionBD()` | *Extract Interface* `IConexionBD`; repositorios reciben la conexión por constructor | ✅ Aplicado |
| E2 | Hard-coded secret | `Pwd=root` en el código | Variable de entorno `ISP_DB_CONNECTION` | ✅ Aplicado |
| E3 | Large Class | `GestionISPService` mezclaba clientes, planes y estados | *Extract Class*: `ClienteRepositorioMySql`, `PlanRepositorioMySql`, `ServicioRepositorioMySql` | ✅ Aplicado |
| E4 | Long Method / Magic Number | `Main` con menú, lectura y SQL; opción de salida `5` | *Extract Method*; menú de 6 opciones por métodos | ✅ Aplicado |
| E5 | Duplicated Code | Creación de comandos y lectura de columnas repetida | *Extract Method*: `AyudanteSql.Crear` y `LeerTexto` | ✅ Aplicado |
| E6 | Varias clases en un archivo | `Entidades.cs` | *Move Class*: un archivo por clase | ✅ Aplicado |
| E7 | Esquema distinto al diseño | La app C# usaba `isp_gestion` (5 tablas, `cedula`, tickets sin tabla de equipos) | Se adoptó `isp_gestion_db` del V2 y se reescribieron modelos y repositorios | ✅ Aplicado |
| J1 | Cross-Site Scripting | `innerHTML` con datos del usuario sin escapar | `escapeHtml()` / `textContent` | ⏳ **Prioridad alta** |
| J2 | Long Method | `exportPDF` (~80 líneas), `renderDashboard`, `renderSoporte` | *Extract Method* | ⏳ Pendiente |
| J3 | Datos personales en el código | Teléfono y correo de un integrante en `CONTACTO` (`app.js`) y en el seed SQL, en repositorio público | Datos genéricos; decisión del grupo | ⏳ Pendiente |
| J4 | Nombres de archivo erróneos | `index.html.html`, `base_datos_isp..sql` | *Rename* | ✅ Aplicado |
| D1 | Dos esquemas para el mismo alcance | `isp_gestion` vs `isp_gestion_db` | Un único script oficial en `proyecto/database/` | ✅ Aplicado |
| D2 | Contraseñas con `SHA2` sin sal | `usuarios.clave_hash` en el seed | bcrypt/Argon2 en la implementación | ⏳ Pendiente |
| D3 | Bloqueo de cuentas no modelado | Sin contador de intentos | Cambio **CC-01** en `proyecto/database/cambios/` | ⏳ Se aplica en S6 |

**Resumen:** de los 8 olores del V2, 2 se corrigieron en la capa C# (V2-3 parcial, V2-7), 2 están parciales (V2-1, V2-6) y 4 siguen pendientes por diseño. Además se resolvieron 9 hallazgos propios.

## 4. Refactorizaciones aplicadas (código)

**Capa de datos (SRP + DIP).** Cada tabla tiene su repositorio detrás de una interfaz; todos reciben `IConexionBD`:

```csharp
public class ServicioRepositorioMySql : IServicioRepositorio
{
    private readonly IConexionBD _conexion;
    public ServicioRepositorioMySql(IConexionBD conexion) => _conexion = conexion;
    public List<ServicioResumen> Listar() { /* ... */ }
    public void CambiarEstado(int idServicio, EstadoServicio nuevo) { /* transacción */ }
}
```

*Por qué:* con una clase por tabla, cada módulo crece sin editar a los demás (SRP/OCP), y las pruebas de la S5 pueden sustituir la conexión por un doble (DIP, V2 Tabla 19).

**Enumeración con regla de negocio (V2-3).** `EstadoServicio.AEstadoContrato()` centraliza RN-07 (servicio Activo → contrato Activo; cualquier otro → Suspendido). `CambiarEstado` actualiza servicio y contrato en **una transacción**, de modo que no quedan estados inconsistentes.

**Códigos sin `length + 1` (V2-7).** `RegistrarConContrato` crea cliente, contrato y servicio en una transacción y genera los códigos con el `AUTO_INCREMENT` (`CONCAT` + `LPAD`), por lo que dos altas simultáneas no repiten código.

**Secretos fuera del código (E2).** `ConexionBD` lee `ISP_DB_CONNECTION`; el valor por defecto es solo para desarrollo local.

## 5. SOLID tras la revisión

| Principio | Capa C# | Prototipo web |
|---|---|---|
| **S** | ✅ un repositorio por tabla; `Program` solo interactúa | ⚠️ `app.js` mezcla estado, vistas y PDF |
| **O** | ✅ enums de estado; módulos nuevos = clases nuevas | ⚠️ `badge()` y `can()` se editan por cada estado/rol |
| **L** | n/a por ahora (sin herencia) | n/a |
| **I** | ✅ interfaces pequeñas por repositorio | n/a |
| **D** | ✅ se depende de `IConexionBD` y de interfaces de repositorio | ⚠️ acceso directo a `state` global |

## 6. Verificación

- [x] `node --test proyecto/tests/prototipo.test.cjs` → 16/16 aprobadas.
- [x] Sintaxis de los archivos `.cs` revisada con un analizador (tree-sitter) y del SQL con `sqlglot` (dialecto MySQL).
- [ ] **Compilar y ejecutar `SistemaGestionISP` con `dotnet build`/`dotnet run`.** El entorno donde se escribió no tenía el SDK de .NET; el código **no ha sido compilado**. Debe hacerlo un integrante antes de la entrega.

## 7. Siguiente paso

1. Compilar y probar la capa C# contra MySQL (Alan, 26/11).
2. S6: excepciones propias y CC-01; corregir los catch vacíos (V2-5) y el XSS (J1).
3. Implementación ASP.NET Core MVC por módulos M1 → M4 con un PR por refactorización pendiente.
