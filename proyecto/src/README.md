# Código fuente

Código único del proyecto, que crece semana a semana. Todo va **comentado** (requisito de cada entrega). El estado del código en cada entrega se guarda con un *tag* de Git.

El proyecto tiene dos componentes **separados**, cada uno con su propia base de datos:

```
src/
├── SistemaGestionISP/            Aplicación de consola C# (.NET 8) + MySQL
│   ├── SistemaGestionISP.csproj  Proyecto (paquete NuGet MySql.Data)
│   ├── Program.cs                Menú de consola (solo interacción con el usuario)
│   ├── Models/                   Entidades: Cliente, PlanInternet, Factura, TicketSoporte, EstadoServicio
│   ├── Data/                     IConexionBD y ConexionBD (cadena de conexión)
│   ├── Repositories/             Acceso a datos: ClienteRepositorio, PlanRepositorio (+ interfaces)
│   └── database/isp_gestion.sql  Script de la base `isp_gestion` (5 tablas)
└── prototipo-web/                Prototipo de interfaz web (datos de demostración en el navegador)
    ├── index.html                Pantallas de los 4 módulos, login, modal y aviso
    ├── js/app.js                 Estado, renderizado de vistas, exportación JSON/PDF
    ├── js/forms.js               Ventana modal y formularios de alta/edición
    ├── js/auth.js                Login y permisos por rol
    └── database/base_datos_isp.sql  Script de la base `isp_gestion_db` (10 tablas)
```

| Módulo del alcance | App de consola (C#) | Prototipo web |
|---|:---:|:---:|
| 1. Gestión de Clientes y Contratos | registro y listado | alta, edición y búsqueda |
| 2. Gestión de Planes y Equipos | consulta de planes | planes y equipos (alta, edición) |
| 3. Facturación y Registro de Pagos | solo modelo/tabla | emisión de facturas y pagos |
| 4. Estado de Servicio y Soporte Básico | cambio de estado | estado de servicio y tickets |

Cómo abrir y ejecutar cada componente: [README principal](../../README.md#cómo-abrir-y-ejecutar-el-proyecto).
Revisión de las dos bases de datos y de los *code smells*: [`Code_Smells_y_Refactorizacion.md`](../../unidad-1/semana-04_T1_avance-2/Code_Smells_y_Refactorizacion.md).

> **Pendiente de decisión del grupo:** las dos bases de datos modelan el mismo alcance con esquemas distintos. Se unificarán a partir del Diseño Detallado v2.
