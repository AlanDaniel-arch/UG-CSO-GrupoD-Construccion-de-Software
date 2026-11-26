# Pruebas unitarias

Desde la raíz del repositorio, ejecutar con Node.js 22 o posterior.

```sh
node --test proyecto/tests/prototipo.test.cjs
```

Las pruebas cargan los scripts del prototipo con un DOM y almacenamiento simulados. Cubren el inicio de sesión y los permisos por rol, la persistencia del estado y la lógica de los formularios de los cuatro módulos. La validación nativa del navegador y la visibilidad real de los controles se verifican en las pruebas funcionales del [plan de pruebas](plan%20de%20pruebas.md).
