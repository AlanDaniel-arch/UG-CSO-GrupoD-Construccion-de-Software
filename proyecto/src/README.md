# Código fuente

Código único del proyecto, que crece semana a semana. Todo va **comentado** (requisito de cada entrega). El estado del código en cada entrega se guarda con un *tag* de Git. La base de datos está en [`../database`](../database) (un solo script para todos los componentes).

```
src/
├── prototipo-web/                  PROTOTIPO FUNCIONAL (Avance 1) — ya terminado y probado
│   ├── index.html                  Pantallas de los 4 módulos, ingreso, ventana modal y aviso
│   └── js/
│       ├── app.js                  Estado, datos de demostración, vistas, exportación JSON/PDF
│       ├── forms.js                Ventana modal y formularios de alta/edición
│       └── auth.js                 Ingreso y permisos por rol
└── SistemaGestionISP/              IMPLEMENTACIÓN OBJETIVO en C# — en construcción
    ├── SistemaGestionISP.csproj    .NET 8 + paquete NuGet MySql.Data
    ├── Program.cs                  Consola provisional para probar la capa de datos
    ├── Models/                     Entidades y estados (enums) de isp_gestion_db
    ├── Data/                       IConexionBD, ConexionBD, AyudanteSql (utilidades de consulta)
    └── Repositories/               IClienteRepositorio, IPlanRepositorio, IServicioRepositorio y sus
                                    implementaciones *MySql
```

## Cómo encaja con el documento maestro (V2)

| Componente | Papel según el V2 | Estado |
|---|---|---|
| `prototipo-web/` | «Prototipo funcional» que valida flujos, permisos y reglas de negocio; sirve de especificación ejecutable (§6.1, §6.3). | ✅ 16 pruebas unitarias + 27 funcionales |
| `SistemaGestionISP/` | Primera pieza de la «implementación objetivo»: capa **Modelo y acceso a datos** de la arquitectura MVC en capas (§6.2). | 🔄 Modelo y repositorios de M1, M2 (consulta) y M4 (estado); falta M3, equipos, soporte, autenticación y la capa web |
| Controladores, servicios de negocio y vistas Razor | Resto de la arquitectura ASP.NET Core MVC (§6.2). | ⏳ Por iniciar |

### Qué hace hoy `SistemaGestionISP`

| Requerimiento | Operación | Archivo |
|---|---|---|
| RF-04 | Registrar cliente + contrato + servicio en una transacción | `Repositories/ClienteRepositorioMySql.cs` |
| RF-05 | Listar clientes con plan y estado del contrato | `Repositories/ClienteRepositorioMySql.cs` |
| RF-09 (consulta) | Listar planes | `Repositories/PlanRepositorioMySql.cs` |
| RF-19 | Listar el estado del servicio de cada contrato | `Repositories/ServicioRepositorioMySql.cs` |
| RF-20 · RN-07 | Cambiar el estado del servicio y sincronizar el contrato | `Repositories/ServicioRepositorioMySql.cs` |

Convenciones (V2 §7.1): nombres en español del dominio, una clase por archivo, comentario de propósito en cada archivo y método público, consultas siempre parametrizadas, sin `catch` vacíos.

Cómo ejecutar cada componente: [README principal](../../README.md#cómo-abrir-y-ejecutar-el-proyecto). Hallazgos de calidad: [`Code_Smells_y_Refactorizacion.md`](../../unidad-1/semana-04_T1_avance-2/Code_Smells_y_Refactorizacion.md).
