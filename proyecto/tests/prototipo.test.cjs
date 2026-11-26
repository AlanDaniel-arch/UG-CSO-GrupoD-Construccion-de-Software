const assert = require('node:assert/strict');
const { readFileSync } = require('node:fs');
const path = require('node:path');
const { test } = require('node:test');
const vm = require('node:vm');

function memoryStorage(entries = []) {
  const values = new Map(entries);
  return {
    getItem: key => values.get(key) ?? null,
    setItem: (key, value) => values.set(key, String(value)),
    removeItem: key => values.delete(key)
  };
}

function createElement() {
  const listeners = new Map();
  const classes = new Set();
  return {
    value: '',
    textContent: '',
    classList: {
      add: name => classes.add(name),
      remove: name => classes.delete(name),
      contains: name => classes.has(name)
    },
    addEventListener: (type, listener) => listeners.set(type, listener),
    reset() {},
    emit: (type, event = {}) => listeners.get(type)({ preventDefault() {}, ...event })
  };
}

function loadScript(file, dependencies = {}) {
  const elements = new Map();
  function element(id) {
    if (!elements.has(id)) elements.set(id, createElement());
    return elements.get(id);
  }
  const context = vm.createContext({
    document: { getElementById: element, querySelectorAll: () => [], addEventListener() {} },
    window: {},
    ...dependencies
  });
  const filename = path.join(__dirname, '..', 'src', 'prototipo-web', 'js', file);
  vm.runInContext(readFileSync(filename, 'utf8'), context, { filename });
  return { context, element, evaluate: code => vm.runInContext(code, context) };
}

const roles = [
  ['admin', 'Administrador', ['clientes', 'planes', 'facturacion', 'soporte'], ['clientes', 'planes', 'facturacion', 'soporte']],
  ['gerente', 'Gerente', ['clientes', 'planes', 'facturacion', 'soporte'], []],
  ['secretario', 'Secretario', ['clientes', 'soporte'], ['clientes', 'soporte']],
  ['cajero', 'Cajero', ['facturacion'], ['facturacion']],
  ['tecnico', 'Técnico', ['planes', 'soporte'], ['planes', 'soporte']]
];

const sessionKey = 'ug_isp_grupoD_session_v1';

function loadAuth(sessionStorage = memoryStorage()) {
  const app = loadScript('auth.js', {
    sessionStorage,
    setupContacto() {},
    renderAll() {},
    switchView() {},
    showToast() {}
  });
  function login(user, password) {
    app.element('login-user').value = user;
    app.element('login-pass').value = password;
    app.element('login-form').emit('submit');
  }
  return { ...app, login, sessionStorage };
}

for (const [user, role, visible, editable] of roles) {
  test('auth: inicio de sesión y permisos de ' + user, () => {
    const { context, element, login, sessionStorage } = loadAuth();
    login(user, user + '123');
    assert.equal(sessionStorage.getItem(sessionKey), user);
    assert.equal(element('user-role').textContent, role);
    assert.equal(element('login-screen').classList.contains('hidden'), true);
    assert.equal(context.canView('dashboard'), true);
    for (const module of ['clientes', 'planes', 'facturacion', 'soporte']) {
      assert.equal(context.canView(module), visible.includes(module), 'vista ' + module);
      assert.equal(context.can(module), editable.includes(module), 'edición ' + module);
    }
  });
}

test('auth: rechaza contraseñas incorrectas y usuarios inexistentes', () => {
  for (const [user, password] of [['admin', 'incorrecta'], ['desconocido', 'admin123']]) {
    const { context, element, login, sessionStorage } = loadAuth();
    login(user, password);
    assert.equal(element('login-error').classList.contains('hidden'), false);
    assert.equal(element('login-screen').classList.contains('hidden'), false);
    assert.equal(sessionStorage.getItem(sessionKey), null);
    assert.equal(context.can('clientes'), false);
  }
});

test('auth: restaura la sesión y elimina sus permisos al salir', () => {
  const storage = memoryStorage([[sessionKey, 'cajero']]);
  const { context, element } = loadAuth(storage);
  assert.equal(element('user-role').textContent, 'Cajero');
  assert.equal(context.can('facturacion'), true);
  element('btn-logout').emit('click');
  assert.equal(storage.getItem(sessionKey), null);
  assert.equal(context.can('facturacion'), false);
  assert.equal(element('login-screen').classList.contains('hidden'), false);
});

const storageKey = 'ug_isp_grupoD_state_v1';

test('app: recupera datos persistidos y guarda los cambios', () => {
  const localStorage = memoryStorage([[storageKey, '{"clientes":[{"id":"C-1","nombre":"Ana"}]}']]);
  const { context, evaluate } = loadScript('app.js', { localStorage });
  assert.equal(context.getClientName('C-1'), 'Ana');
  assert.equal(context.getClientName('C-2'), 'C-2');
  evaluate("state.clientes[0].nombre = 'Ana Pérez'; saveState();");
  assert.equal(JSON.parse(localStorage.getItem(storageKey)).clientes[0].nombre, 'Ana Pérez');
});

test('app: usa una copia de los datos iniciales si el almacenamiento está dañado', () => {
  const localStorage = memoryStorage([[storageKey, '{']]);
  const { context, evaluate } = loadScript('app.js', { localStorage });
  assert.match(context.getPlanName('PLN-01'), /Fibra Hogar 100/);
  evaluate('state.planes.length = 0');
  assert.ok(context.loadState().planes.length > 0);
});

function loadForms(initialState = {}) {
  const state = { clientes: [], planes: [], equipos: [], facturas: [], pagos: [], tickets: [], ...initialState };
  const calls = { save: 0, render: 0 };
  const { context, element } = loadScript('forms.js', {
    state,
    todayISO: () => '2026-11-25',
    getClientName: id => id,
    saveState: () => { calls.save++; },
    renderAll: () => { calls.render++; },
    showToast() {},
    FormData: class {
      constructor(fields) { this.fields = fields; }
      get(name) { return this.fields[name] ?? null; }
    }
  });
  return {
    state, context, calls, element,
    submit: fields => element('modal-form').emit('submit', { target: fields })
  };
}

test('forms: crea y actualiza un cliente sin duplicar su servicio', () => {
  const { state, context, submit, calls } = loadForms();
  const fields = { id: ' C-1 ', nombre: ' Ana ', direccion: 'Centro', telefono: '0990000000',
    correo: 'ana@example.com', contrato: 'CTR-1', planId: 'PLN-01', estadoContrato: 'Activo' };
  context.openClientModal();
  submit(fields);
  assert.equal(state.clientes[0].nombre, 'Ana');
  assert.equal(state.clientes[0].estadoServicio, 'Activo');
  const service = state.clientes[0].servicioId;
  context.openClientModal('C-1');
  submit({ ...fields, nombre: 'Ana Pérez', estadoContrato: 'Suspendido' });
  assert.equal(state.clientes.length, 1);
  assert.equal(state.clientes[0].nombre, 'Ana Pérez');
  assert.equal(state.clientes[0].servicioId, service);
  assert.equal(state.clientes[0].estadoServicio, 'Suspendido');
  assert.deepEqual(calls, { save: 2, render: 2 });
});

test('forms: registra un plan con precio numérico y actualiza el mismo registro', () => {
  const { state, context, submit } = loadForms();
  const fields = { id: 'PLN-1', nombre: 'Fibra', velocidad: '100 Mbps', precio: '24.50', descripcion: '' };
  context.openPlanModal();
  submit(fields);
  assert.equal(state.planes[0].precio, 24.5);
  assert.equal(state.planes[0].estado, 'Activo');
  context.openPlanModal('PLN-1');
  submit({ ...fields, velocidad: '200 Mbps' });
  assert.equal(state.planes.length, 1);
  assert.equal(state.planes[0].velocidad, '200 Mbps');
});

test('forms: registra y edita un equipo asociado a un contrato', () => {
  const { state, context, submit } = loadForms({ clientes: [{ id: 'C-1', contrato: 'CTR-1' }] });
  const fields = { id: 'EQ-1', tipo: 'Router', marcaModelo: 'Modelo 1', serie: 'SER-1',
    contrato: 'CTR-1', estado: 'Asignado' };
  context.openEquipModal();
  submit(fields);
  assert.equal(state.equipos[0].contrato, 'CTR-1');
  context.openEquipModal('EQ-1');
  submit({ ...fields, estado: 'En mantenimiento' });
  assert.equal(state.equipos.length, 1);
  assert.equal(state.equipos[0].estado, 'En mantenimiento');
});

test('forms: emite una factura pendiente vinculada al contrato del cliente', () => {
  const { state, context, submit } = loadForms({ clientes: [{ id: 'C-1', contrato: 'CTR-1' }] });
  context.openInvoiceModal();
  submit({ id: ' F-1 ', clienteId: 'C-1', emision: '2026-11-25', vencimiento: '2026-12-05', valor: '35.50' });
  const invoice = state.facturas[0];
  assert.equal(invoice.id, 'F-1');
  assert.equal(invoice.contrato, 'CTR-1');
  assert.equal(invoice.valor, 35.5);
  assert.equal(invoice.estado, 'Pendiente');
});

test('forms: registra el pago por el valor de la factura y la marca pagada', () => {
  const invoice = { id: 'F-1', clienteId: 'C-1', valor: 35.5, estado: 'Pendiente' };
  const { state, context, submit, calls } = loadForms({ facturas: [invoice] });
  context.openPayModal('F-1');
  submit({ id: 'P-1', fecha: '2026-11-25', metodo: 'Efectivo' });
  assert.equal(invoice.estado, 'Pagada');
  const payment = state.pagos[0];
  assert.equal(payment.facturaId, invoice.id);
  assert.equal(payment.clienteId, invoice.clienteId);
  assert.equal(payment.valor, invoice.valor);
  assert.equal(payment.metodo, 'Efectivo');
  assert.deepEqual(calls, { save: 1, render: 1 });
});

test('forms: crea un ticket abierto y recorta su descripción', () => {
  const { state, context, submit } = loadForms({ clientes: [{ id: 'C-1', nombre: 'Ana' }] });
  context.openTicketModal();
  submit({ id: 'SOP-001', clienteId: 'C-1', descripcion: ' Sin conexión ', fecha: '2026-11-25' });
  assert.equal(state.tickets[0].clienteId, 'C-1');
  assert.equal(state.tickets[0].descripcion, 'Sin conexión');
  assert.equal(state.tickets[0].estado, 'Abierto');
});

test('forms: al cancelar se elimina el manejador del formulario', () => {
  const { state, context, submit, calls, element } = loadForms();
  context.openPlanModal();
  element('btn-cancel-modal').emit('click');
  submit({});
  assert.equal(state.planes.length, 0);
  assert.deepEqual(calls, { save: 0, render: 0 });
});

