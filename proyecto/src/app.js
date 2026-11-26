    const STORAGE_KEY = 'ug_isp_grupoD_state_v1';

    const DEFAULT_STATE = {
      planes: [
        { id: 'PLN-01', nombre: 'Fibra Hogar 100', velocidad: '100 Mbps', precio: 25.00, descripcion: 'Ideal para navegación, teletrabajo y streaming HD.', estado: 'Activo' },
        { id: 'PLN-02', nombre: 'Fibra Plus 250', velocidad: '250 Mbps', precio: 35.00, descripcion: 'Alta velocidad simétrica para hogares conectados.', estado: 'Activo' },
        { id: 'PLN-03', nombre: 'Fibra Pro PyME 500', velocidad: '500 Mbps', precio: 58.00, descripcion: 'Plan empresarial con soporte prioritario y IP fija.', estado: 'Activo' }
      ],
      clientes: [
        {
          id: '0924815201', nombre: 'Carlos Mendoza Vera', direccion: 'Cdla. Alborada 7ma Etapa Mz 714',
          telefono: '0994128831', correo: 'cmendoza@correo.ec', fechaRegistro: '2026-09-12',
          contrato: 'CTR-2026-001', fechaInicio: '2026-09-15', planId: 'PLN-02', estadoContrato: 'Activo',
          servicioId: 'SRV-001', estadoServicio: 'Activo', fechaActualizacion: '2026-11-20'
        },
        {
          id: '0918743310', nombre: 'María Fernanda Paredes', direccion: 'Av. Francisco de Orellana Km 3',
          telefono: '0987654120', correo: 'mparedes@correo.ec', fechaRegistro: '2026-10-02',
          contrato: 'CTR-2026-002', fechaInicio: '2026-10-05', planId: 'PLN-03', estadoContrato: 'Activo',
          servicioId: 'SRV-002', estadoServicio: 'Activo', fechaActualizacion: '2026-11-22'
        },
        {
          id: '0951209844', nombre: 'Luis Eduardo Zambrano', direccion: 'Sauces 4 Mz 281 Villa 14',
          telefono: '0991204458', correo: 'lzambrano@correo.ec', fechaRegistro: '2026-08-19',
          contrato: 'CTR-2026-003', fechaInicio: '2026-08-20', planId: 'PLN-01', estadoContrato: 'Suspendido',
          servicioId: 'SRV-003', estadoServicio: 'Suspendido', fechaActualizacion: '2026-11-18'
        }
      ],
      equipos: [
        { id: 'EQ-001', tipo: 'ONT Fibra', marcaModelo: 'Huawei HG8145V5', serie: 'HW-9948120A', contrato: 'CTR-2026-001', estado: 'Asignado' },
        { id: 'EQ-002', tipo: 'Router Dual Band', marcaModelo: 'TP-Link Archer AX23', serie: 'TP-5582910C', contrato: 'CTR-2026-002', estado: 'Asignado' },
        { id: 'EQ-003', tipo: 'ONT Fibra', marcaModelo: 'ZTE F670L', serie: 'ZT-3310844B', contrato: 'CTR-2026-003', estado: 'Asignado' },
        { id: 'EQ-004', tipo: 'Router Wi-Fi 6', marcaModelo: 'MikroTik hAP ax2', serie: 'MK-7721004D', contrato: 'Sin asignar', estado: 'Disponible' }
      ],
      facturas: [
        { id: 'FAC-2026-101', clienteId: '0924815201', contrato: 'CTR-2026-001', emision: '2026-11-01', vencimiento: '2026-11-15', valor: 35.00, estado: 'Pagada' },
        { id: 'FAC-2026-102', clienteId: '0918743310', contrato: 'CTR-2026-002', emision: '2026-11-05', vencimiento: '2026-11-28', valor: 58.00, estado: 'Pendiente' },
        { id: 'FAC-2026-103', clienteId: '0951209844', contrato: 'CTR-2026-003', emision: '2026-10-20', vencimiento: '2026-11-05', valor: 25.00, estado: 'Vencida' }
      ],
      pagos: [
        { id: 'PAG-2026-051', facturaId: 'FAC-2026-101', clienteId: '0924815201', fecha: '2026-11-10', valor: 35.00, metodo: 'Transferencia' }
      ],
      tickets: [
        { id: 'SOP-001', clienteId: '0918743310', descripcion: 'Intermitencia en la señal Wi-Fi durante horas pico.', fecha: '2026-11-23', estado: 'En proceso' },
        { id: 'SOP-002', clienteId: '0951209844', descripcion: 'Solicitud de reconexión tras consulta de saldo pendiente.', fecha: '2026-11-24', estado: 'Abierto' }
      ]
    };

    let state = loadState();

    function loadState() {
      try {
        const raw = localStorage.getItem(STORAGE_KEY);
        if (raw) return JSON.parse(raw);
      } catch (e) {}
      return JSON.parse(JSON.stringify(DEFAULT_STATE));
    }

    function saveState() {
      try { localStorage.setItem(STORAGE_KEY, JSON.stringify(state)); } catch (e) {}
    }

    function showToast(msg) {
      const toast = document.getElementById('toast');
      document.getElementById('toast-msg').textContent = msg;
      toast.classList.remove('hidden');
      toast.classList.add('flex');
      clearTimeout(window._toastTimer);
      window._toastTimer = setTimeout(() => {
        toast.classList.add('hidden');
        toast.classList.remove('flex');
      }, 2800);
    }

    function badge(status) {
      const map = {
        'Activo': 'bg-emerald-50 text-emerald-700 border-emerald-200',
        'Pagada': 'bg-emerald-50 text-emerald-700 border-emerald-200',
        'Resuelto': 'bg-emerald-50 text-emerald-700 border-emerald-200',
        'Disponible': 'bg-sky-50 text-sky-700 border-sky-200',
        'Asignado': 'bg-indigo-50 text-indigo-700 border-indigo-200',
        'Pendiente': 'bg-amber-50 text-amber-700 border-amber-200',
        'En proceso': 'bg-amber-50 text-amber-700 border-amber-200',
        'En mantenimiento': 'bg-amber-50 text-amber-700 border-amber-200',
        'Suspendido': 'bg-rose-50 text-rose-700 border-rose-200',
        'Vencida': 'bg-rose-50 text-rose-700 border-rose-200',
        'Abierto': 'bg-rose-50 text-rose-700 border-rose-200',
        'Cancelado': 'bg-slate-100 text-slate-600 border-slate-200',
        'Inactivo': 'bg-slate-100 text-slate-600 border-slate-200'
      };
      const cls = map[status] || 'bg-slate-100 text-slate-700 border-slate-200';
      return `<span class="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium border ${cls}">${status}</span>`;
    }

    function getPlanName(planId) {
      const p = state.planes.find(x => x.id === planId);
      return p ? `${p.nombre} (${p.velocidad})` : planId;
    }

    function getClientName(clienteId) {
      const c = state.clientes.find(x => x.id === clienteId);
      return c ? c.nombre : clienteId;
    }

    function todayISO() {
      return new Date().toISOString().slice(0, 10);
    }

    // --- NAVIGATION ---
    function switchView(viewName) {
      if (!canView(viewName)) viewName = 'dashboard';
      document.querySelectorAll('.view-section').forEach(s => s.classList.add('hidden'));
      const target = document.getElementById('view-' + viewName);
      if (target) target.classList.remove('hidden');

      document.querySelectorAll('.nav-btn').forEach(btn => {
        if (btn.dataset.view === viewName) {
          btn.classList.add('tab-active');
          btn.classList.remove('hover:bg-slate-100');
        } else {
          btn.classList.remove('tab-active');
          btn.classList.add('hover:bg-slate-100');
        }
      });
      renderAll();
    }

    function bindSubtabs(btnClass, panelPrefix) {
      document.querySelectorAll('.' + btnClass).forEach(btn => {
        btn.addEventListener('click', () => {
          document.querySelectorAll('.' + btnClass).forEach(b => {
            b.classList.remove('subtab-active');
            b.classList.add('text-slate-500');
          });
          btn.classList.add('subtab-active');
          btn.classList.remove('text-slate-500');
          const sub = btn.dataset.subtab;
          document.querySelectorAll(`[id^="${panelPrefix}"]`).forEach(p => p.classList.add('hidden'));
          const activePanel = document.getElementById('subpanel-' + sub);
          if (activePanel) activePanel.classList.remove('hidden');
        });
      });
    }

    // --- RENDER FUNCTIONS ---
    function renderDashboard() {
      const activos = state.clientes.filter(c => c.estadoContrato === 'Activo').length;
      const recaudado = state.pagos.reduce((s, p) => s + Number(p.valor || 0), 0);
      const pendientes = state.facturas.filter(f => f.estado !== 'Pagada');
      const montoPendiente = pendientes.reduce((s, f) => s + Number(f.valor || 0), 0);
      const ticketsAbiertos = state.tickets.filter(t => t.estado !== 'Resuelto').length;

      document.getElementById('kpi-grid').innerHTML = `
        <div class="bg-white p-5 rounded-xl border border-slate-200 shadow-sm">
          <div class="flex items-center justify-between text-xs font-medium text-slate-500">
            <span>Clientes Activos</span>
            <i data-lucide="users" class="w-4 h-4 text-sky-600"></i>
          </div>
          <p class="text-2xl font-bold text-slate-900 mt-2">${activos} <span class="text-xs font-normal text-slate-400">/ ${state.clientes.length} total</span></p>
        </div>
        <div class="bg-white p-5 rounded-xl border border-slate-200 shadow-sm">
          <div class="flex items-center justify-between text-xs font-medium text-slate-500">
            <span>Recaudación Registrada</span>
            <i data-lucide="wallet" class="w-4 h-4 text-emerald-600"></i>
          </div>
          <p class="text-2xl font-bold text-emerald-700 mt-2">$${recaudado.toFixed(2)}</p>
        </div>
        <div class="bg-white p-5 rounded-xl border border-slate-200 shadow-sm">
          <div class="flex items-center justify-between text-xs font-medium text-slate-500">
            <span>Por Cobrar (${pendientes.length})</span>
            <i data-lucide="alert-circle" class="w-4 h-4 text-amber-600"></i>
          </div>
          <p class="text-2xl font-bold text-amber-700 mt-2">$${montoPendiente.toFixed(2)}</p>
        </div>
        <div class="bg-white p-5 rounded-xl border border-slate-200 shadow-sm">
          <div class="flex items-center justify-between text-xs font-medium text-slate-500">
            <span>Soporte Abierto</span>
            <i data-lucide="headphones" class="w-4 h-4 text-rose-600"></i>
          </div>
          <p class="text-2xl font-bold text-slate-900 mt-2">${ticketsAbiertos} <span class="text-xs font-normal text-slate-400">solicitudes</span></p>
        </div>
      `;

      const pendEl = document.getElementById('dash-pending-invoices');
      if (!pendientes.length) {
        pendEl.innerHTML = `<p class="p-5 text-slate-400 text-center">No hay facturas pendientes.</p>`;
      } else {
        pendEl.innerHTML = pendientes.slice(0, 5).map(f => `
          <div class="px-5 py-3 flex items-center justify-between gap-2">
            <div>
              <p class="font-medium text-slate-800">${getClientName(f.clienteId)}</p>
              <p class="text-xs text-slate-500">${f.id} · Vence: ${f.vencimiento}</p>
            </div>
            <div class="flex items-center gap-2">
              <span class="font-semibold text-slate-900">$${Number(f.valor).toFixed(2)}</span>
              ${badge(f.estado)}
            </div>
          </div>
        `).join('');
      }

      const openTickets = state.tickets.filter(t => t.estado !== 'Resuelto');
      const tickEl = document.getElementById('dash-open-tickets');
      if (!openTickets.length) {
        tickEl.innerHTML = `<p class="p-5 text-slate-400 text-center">Todas las solicitudes están resueltas.</p>`;
      } else {
        tickEl.innerHTML = openTickets.slice(0, 5).map(t => `
          <div class="px-5 py-3 flex items-center justify-between gap-3">
            <div class="min-w-0">
              <p class="font-medium text-slate-800 truncate">${getClientName(t.clienteId)} <span class="text-xs text-slate-400">(${t.id})</span></p>
              <p class="text-xs text-slate-500 truncate">${t.descripcion}</p>
            </div>
            ${badge(t.estado)}
          </div>
        `).join('');
      }
    }

    function renderClientes() {
      const q = (document.getElementById('search-clients').value || '').toLowerCase().trim();
      const status = document.getElementById('filter-contract-status').value;
      const list = state.clientes.filter(c => {
        const matchQ = !q || c.id.toLowerCase().includes(q) || c.nombre.toLowerCase().includes(q) || c.contrato.toLowerCase().includes(q);
        const matchS = status === 'ALL' || c.estadoContrato === status;
        return matchQ && matchS;
      });
      const tbody = document.getElementById('clients-tbody');
      if (!list.length) {
        tbody.innerHTML = `<tr><td colspan="6" class="px-4 py-8 text-center text-slate-400">No se encontraron clientes con ese criterio.</td></tr>`;
        return;
      }
      tbody.innerHTML = list.map(c => `
        <tr class="hover:bg-slate-50">
          <td class="px-4 py-3 font-mono text-xs text-slate-700">${c.id}</td>
          <td class="px-4 py-3">
            <p class="font-medium text-slate-900">${c.nombre}</p>
            <p class="text-xs text-slate-500">${c.telefono} · ${c.correo}</p>
            <p class="text-xs text-slate-400">${c.direccion}</p>
          </td>
          <td class="px-4 py-3">
            <p class="font-medium text-slate-800">${c.contrato}</p>
            <p class="text-xs text-slate-500">Inicio: ${c.fechaInicio}</p>
          </td>
          <td class="px-4 py-3 text-slate-700">${getPlanName(c.planId)}</td>
          <td class="px-4 py-3">${badge(c.estadoContrato)}</td>
          <td class="px-4 py-3 text-right">
            <button type="button" data-edit-client="${c.id}" class="text-xs font-medium text-sky-700 hover:text-sky-900 px-2.5 py-1 rounded-md hover:bg-sky-50 transition">Editar</button>
          </td>
        </tr>
      `).join('');
    }

    function renderPlanesYEquipos() {
      const planesEl = document.getElementById('subpanel-planes-list');
      planesEl.innerHTML = state.planes.map(p => `
        <div class="bg-white rounded-xl border border-slate-200 shadow-sm p-5 flex flex-col justify-between gap-4">
          <div>
            <div class="flex items-start justify-between gap-2">
              <div>
                <span class="text-xs font-mono text-slate-400">${p.id}</span>
                <h3 class="text-base font-bold text-slate-900">${p.nombre}</h3>
              </div>
              ${badge(p.estado)}
            </div>
            <p class="text-2xl font-bold text-[#1e3a5f] mt-3">$${Number(p.precio).toFixed(2)} <span class="text-xs font-normal text-slate-500">/ mes · ${p.velocidad}</span></p>
            <p class="text-xs text-slate-600 mt-2">${p.descripcion}</p>
          </div>
          <div class="pt-3 border-t border-slate-100 flex justify-end gap-2">
            <button type="button" data-toggle-plan="${p.id}" class="text-xs font-medium text-slate-600 hover:text-slate-900 px-2.5 py-1 rounded hover:bg-slate-100">
              ${p.estado === 'Activo' ? 'Desactivar' : 'Activar'}
            </button>
            <button type="button" data-edit-plan="${p.id}" class="text-xs font-medium text-sky-700 hover:text-sky-900 px-2.5 py-1 rounded hover:bg-sky-50">Editar</button>
          </div>
        </div>
      `).join('');

      const eqBody = document.getElementById('equipos-tbody');
      eqBody.innerHTML = state.equipos.map(e => `
        <tr class="hover:bg-slate-50">
          <td class="px-4 py-3 font-mono text-xs">${e.id}</td>
          <td class="px-4 py-3">
            <p class="font-medium text-slate-900">${e.tipo}</p>
            <p class="text-xs text-slate-500">${e.marcaModelo}</p>
          </td>
          <td class="px-4 py-3 font-mono text-xs text-slate-600">${e.serie}</td>
          <td class="px-4 py-3 text-slate-700">${e.contrato}</td>
          <td class="px-4 py-3">${badge(e.estado)}</td>
          <td class="px-4 py-3 text-right">
            <button type="button" data-edit-equip="${e.id}" class="text-xs font-medium text-sky-700 hover:text-sky-900 px-2.5 py-1 rounded hover:bg-sky-50">Editar</button>
          </td>
        </tr>
      `).join('');
    }

    function renderFacturacion() {
      const fStatus = document.getElementById('filter-invoice-status').value;
      const list = state.facturas.filter(f => fStatus === 'ALL' || f.estado === fStatus);
      const tbody = document.getElementById('facturas-tbody');
      if (!list.length) {
        tbody.innerHTML = `<tr><td colspan="6" class="px-4 py-8 text-center text-slate-400">No hay facturas en esta categoría.</td></tr>`;
      } else {
        tbody.innerHTML = list.map(f => `
          <tr class="hover:bg-slate-50">
            <td class="px-4 py-3 font-mono text-xs font-medium">${f.id}</td>
            <td class="px-4 py-3">
              <p class="font-medium text-slate-900">${getClientName(f.clienteId)}</p>
              <p class="text-xs text-slate-500">${f.contrato}</p>
            </td>
            <td class="px-4 py-3 text-xs text-slate-600">
              <p>Emisión: ${f.emision}</p>
              <p>Vence: ${f.vencimiento}</p>
            </td>
            <td class="px-4 py-3 font-semibold text-slate-900">$${Number(f.valor).toFixed(2)}</td>
            <td class="px-4 py-3">${badge(f.estado)}</td>
            <td class="px-4 py-3 text-right">
              ${f.estado !== 'Pagada'
                ? `<button type="button" data-pay-invoice="${f.id}" class="text-xs font-medium bg-emerald-600 hover:bg-emerald-700 text-white px-3 py-1.5 rounded-lg transition">Registrar Pago</button>`
                : `<span class="text-xs text-slate-400">Liquidada</span>`}
            </td>
          </tr>
        `).join('');
      }

      const pBody = document.getElementById('pagos-tbody');
      if (!state.pagos.length) {
        pBody.innerHTML = `<tr><td colspan="6" class="px-4 py-8 text-center text-slate-400">Aún no se registran pagos.</td></tr>`;
      } else {
        pBody.innerHTML = state.pagos.map(p => `
          <tr class="hover:bg-slate-50">
            <td class="px-4 py-3 font-mono text-xs">${p.id}</td>
            <td class="px-4 py-3 font-mono text-xs text-slate-600">${p.facturaId}</td>
            <td class="px-4 py-3 font-medium text-slate-800">${getClientName(p.clienteId)}</td>
            <td class="px-4 py-3 text-xs text-slate-600">${p.fecha}</td>
            <td class="px-4 py-3 text-slate-700">${p.metodo}</td>
            <td class="px-4 py-3 text-right font-semibold text-emerald-700">$${Number(p.valor).toFixed(2)}</td>
          </tr>
        `).join('');
      }
    }

    function renderSoporte() {
      const sBody = document.getElementById('servicios-tbody');
      sBody.innerHTML = state.clientes.map(c => `
        <tr class="hover:bg-slate-50">
          <td class="px-4 py-3 font-mono text-xs">${c.servicioId}</td>
          <td class="px-4 py-3">
            <p class="font-medium text-slate-900">${c.nombre}</p>
            <p class="text-xs text-slate-500">${c.contrato}</p>
          </td>
          <td class="px-4 py-3 text-slate-700">${getPlanName(c.planId)}</td>
          <td class="px-4 py-3 text-xs text-slate-500">${c.fechaActualizacion}</td>
          <td class="px-4 py-3">${badge(c.estadoServicio)}</td>
          <td class="px-4 py-3 text-right">
            <select data-change-service="${c.id}" class="text-xs border border-slate-300 rounded-md px-2 py-1 bg-white">
              <option value="Activo" ${c.estadoServicio === 'Activo' ? 'selected' : ''}>Activo</option>
              <option value="Suspendido" ${c.estadoServicio === 'Suspendido' ? 'selected' : ''}>Suspendido</option>
              <option value="Pendiente" ${c.estadoServicio === 'Pendiente' ? 'selected' : ''}>Pendiente</option>
            </select>
          </td>
        </tr>
      `).join('');

      const tBody = document.getElementById('tickets-tbody');
      if (!state.tickets.length) {
        tBody.innerHTML = `<tr><td colspan="6" class="px-4 py-8 text-center text-slate-400">No hay solicitudes de soporte registradas.</td></tr>`;
      } else {
        tBody.innerHTML = state.tickets.map(t => {
          const next = t.estado === 'Abierto' ? 'En proceso' : (t.estado === 'En proceso' ? 'Resuelto' : null);
          return `
            <tr class="hover:bg-slate-50">
              <td class="px-4 py-3 font-mono text-xs">${t.id}</td>
              <td class="px-4 py-3 font-medium text-slate-900">${getClientName(t.clienteId)}</td>
              <td class="px-4 py-3 text-slate-700 max-w-xs">${t.descripcion}</td>
              <td class="px-4 py-3 text-xs text-slate-500">${t.fecha}</td>
              <td class="px-4 py-3">${badge(t.estado)}</td>
              <td class="px-4 py-3 text-right">
                ${next
                  ? `<button type="button" data-advance-ticket="${t.id}" class="text-xs font-medium text-sky-700 hover:bg-sky-50 px-2.5 py-1 rounded-md border border-sky-200 transition">Pasar a ${next}</button>`
                  : `<span class="text-xs text-slate-400">Cerrado</span>`}
              </td>
            </tr>
          `;
        }).join('');
      }
    }

    function renderAll() {
      renderDashboard();
      renderClientes();
      renderPlanesYEquipos();
      renderFacturacion();
      renderSoporte();
      if (window.lucide) lucide.createIcons();
      applyPermissions();
    }

    // --- EVENT LISTENERS ---
    document.querySelectorAll('.nav-btn').forEach(btn => {
      btn.addEventListener('click', () => switchView(btn.dataset.view));
    });
    document.querySelectorAll('.jump-btn').forEach(btn => {
      btn.addEventListener('click', () => switchView(btn.dataset.jump));
    });

    bindSubtabs('planes-subtab', 'subpanel-planes-list');
    // Also handle planes/equipos toggling cleanly
    document.querySelectorAll('.planes-subtab').forEach(btn => {
      btn.addEventListener('click', () => {
        document.getElementById('subpanel-planes-list').classList.toggle('hidden', btn.dataset.subtab !== 'planes-list');
        document.getElementById('subpanel-equipos-list').classList.toggle('hidden', btn.dataset.subtab !== 'equipos-list');
      });
    });
    document.querySelectorAll('.fact-subtab').forEach(btn => {
      btn.addEventListener('click', () => {
        document.querySelectorAll('.fact-subtab').forEach(b => { b.classList.remove('subtab-active'); b.classList.add('text-slate-500'); });
        btn.classList.add('subtab-active'); btn.classList.remove('text-slate-500');
        document.getElementById('subpanel-facturas-list').classList.toggle('hidden', btn.dataset.subtab !== 'facturas-list');
        document.getElementById('subpanel-pagos-list').classList.toggle('hidden', btn.dataset.subtab !== 'pagos-list');
      });
    });
    document.querySelectorAll('.soporte-subtab').forEach(btn => {
      btn.addEventListener('click', () => {
        document.querySelectorAll('.soporte-subtab').forEach(b => { b.classList.remove('subtab-active'); b.classList.add('text-slate-500'); });
        btn.classList.add('subtab-active'); btn.classList.remove('text-slate-500');
        document.getElementById('subpanel-servicios-list').classList.toggle('hidden', btn.dataset.subtab !== 'servicios-list');
        document.getElementById('subpanel-tickets-list').classList.toggle('hidden', btn.dataset.subtab !== 'tickets-list');
      });
    });

    document.getElementById('search-clients').addEventListener('input', renderClientes);
    document.getElementById('filter-contract-status').addEventListener('change', renderClientes);
    document.getElementById('filter-invoice-status').addEventListener('change', renderFacturacion);

    document.getElementById('btn-reset-demo').addEventListener('click', () => {
      state = JSON.parse(JSON.stringify(DEFAULT_STATE));
      saveState();
      renderAll();
      showToast('Datos de demostración restaurados.');
    });

    document.getElementById('btn-export-json').addEventListener('click', () => {
      const blob = new Blob([JSON.stringify(state, null, 2)], { type: 'application/json' });
      const url = URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = 'sistema_isp_datos.json';
      a.click();
      URL.revokeObjectURL(url);
      showToast('Datos exportados en formato JSON.');
    });

    // =====================================================
    //  CONFIGURACIÓN DE CONTACTO (cámbialo por los datos reales)
    // =====================================================
    const CONTACTO = {
      telefonoVisible: '0978834917 (+593 97 883 4917)',
      whatsapp: '593978834917',            // código de país + número, sin + ni espacios
      correo: 'belinda.tenegusnaytib@ug.edu.ec',
      horario: 'Lun - Sáb · 08:00 a 20:00',
      mensaje: 'Hola, necesito soporte técnico con mi servicio de internet.'
    };

    // =====================================================
    //  CONTACTO + QR A WHATSAPP
    // =====================================================
    function setupContacto() {
      const url = `https://wa.me/${CONTACTO.whatsapp}?text=${encodeURIComponent(CONTACTO.mensaje)}`;
      document.getElementById('contact-phone').textContent = CONTACTO.telefonoVisible;
      document.getElementById('contact-mail').textContent = CONTACTO.correo;
      document.getElementById('contact-hours').textContent = CONTACTO.horario;
      document.getElementById('btn-whatsapp').href = url;
      const box = document.getElementById('qr-whatsapp');
      box.innerHTML = '';
      if (window.QRCode) {
        new QRCode(box, { text: url, width: 132, height: 132, colorDark: '#0b2447', colorLight: '#ffffff', correctLevel: QRCode.CorrectLevel.M });
      } else {
        box.textContent = 'No se pudo cargar el generador de QR (revisa tu conexión).';
      }
    }

    // =====================================================
    //  EXPORTAR PDF
    // =====================================================
    function exportPDF() {
      if (!(window.jspdf && window.jspdf.jsPDF)) { showToast('No se pudo cargar la librería de PDF. Revisa tu conexión.'); return; }
      const doc = new window.jspdf.jsPDF({ unit: 'pt', format: 'a4' });
      if (typeof doc.autoTable !== 'function') { showToast('No se pudo cargar el plugin de tablas PDF.'); return; }

      const W = doc.internal.pageSize.getWidth();
      const fecha = new Date().toLocaleString('es-EC');
      const money = v => '$' + Number(v).toFixed(2);

      // Portada / encabezado
      doc.setFillColor(30, 58, 95); doc.rect(0, 0, W, 78, 'F');
      doc.setTextColor(255); doc.setFont('helvetica', 'bold'); doc.setFontSize(17);
      doc.text('ISP Manager', 40, 36);
      doc.setFont('helvetica', 'normal'); doc.setFontSize(10);
      doc.text('Sistema de Gestión de Clientes, Planes y Facturación', 40, 54);
      doc.text(`Generado: ${fecha}` + (currentUser ? `  ·  Usuario: ${currentUser.nombre} (${currentUser.rol})` : ''), 40, 68);

      // Resumen
      const activos = state.clientes.filter(c => c.estadoServicio === 'Activo').length;
      const recaudado = state.pagos.reduce((s, p) => s + Number(p.valor), 0);
      const porCobrar = state.facturas.filter(f => f.estado !== 'Pagada').reduce((s, f) => s + Number(f.valor), 0);
      const ticketsAbiertos = state.tickets.filter(t => t.estado !== 'Resuelto').length;
      let y = 100;
      doc.setTextColor(30, 58, 95); doc.setFont('helvetica', 'bold'); doc.setFontSize(12);
      doc.text('Resumen general', 40, y);
      doc.autoTable({
        startY: y + 8, theme: 'grid',
        head: [['Clientes', 'Servicios activos', 'Recaudado', 'Por cobrar', 'Soporte abierto']],
        body: [[state.clientes.length, activos, money(recaudado), money(porCobrar), ticketsAbiertos]],
        headStyles: { fillColor: [30, 58, 95] }, styles: { fontSize: 9, halign: 'center' }, margin: { left: 40, right: 40 }
      });

      const seccion = (titulo, head, body) => {
        let sy = doc.lastAutoTable.finalY + 24;
        if (sy > doc.internal.pageSize.getHeight() - 120) { doc.addPage(); sy = 50; }
        doc.setTextColor(30, 58, 95); doc.setFont('helvetica', 'bold'); doc.setFontSize(12);
        doc.text(titulo, 40, sy);
        doc.autoTable({
          startY: sy + 8, head: [head],
          body: body.length ? body : [[{ content: 'Sin registros', colSpan: head.length, styles: { halign: 'center', textColor: 150 } }]],
          headStyles: { fillColor: [37, 99, 168] }, alternateRowStyles: { fillColor: [241, 247, 255] },
          styles: { fontSize: 8, cellPadding: 4 }, margin: { left: 40, right: 40 }
        });
      };

      seccion('1. Clientes y contratos',
        ['Identificación', 'Nombre', 'Teléfono', 'Contrato', 'Plan', 'Estado'],
        state.clientes.map(c => [c.id, c.nombre, c.telefono, c.contrato, getPlanName(c.planId), c.estadoContrato]));
      seccion('2. Planes de internet',
        ['Código', 'Nombre', 'Velocidad', 'Precio mensual', 'Estado'],
        state.planes.map(p => [p.id, p.nombre, p.velocidad, money(p.precio), p.estado]));
      seccion('3. Equipos',
        ['Código', 'Tipo', 'Marca / modelo', 'Serie', 'Contrato', 'Estado'],
        state.equipos.map(e => [e.id, e.tipo, e.marcaModelo, e.serie, e.contrato, e.estado]));
      seccion('4. Facturas',
        ['Código', 'Cliente', 'Contrato', 'Emisión', 'Vencimiento', 'Valor', 'Estado'],
        state.facturas.map(f => [f.id, getClientName(f.clienteId), f.contrato, f.emision, f.vencimiento, money(f.valor), f.estado]));
      seccion('5. Pagos',
        ['Código', 'Factura', 'Cliente', 'Fecha', 'Valor', 'Método'],
        state.pagos.map(p => [p.id, p.facturaId, getClientName(p.clienteId), p.fecha, money(p.valor), p.metodo]));
      seccion('6. Estado de servicios',
        ['Servicio', 'Cliente', 'Contrato', 'Estado', 'Actualización'],
        state.clientes.map(c => [c.servicioId, c.nombre, c.contrato, c.estadoServicio, c.fechaActualizacion]));
      seccion('7. Solicitudes de soporte',
        ['Código', 'Cliente', 'Descripción', 'Fecha', 'Estado'],
        state.tickets.map(t => [t.id, getClientName(t.clienteId), t.descripcion, t.fecha, t.estado]));

      // Pie con contacto y numeración
      const pages = doc.internal.getNumberOfPages();
      for (let i = 1; i <= pages; i++) {
        doc.setPage(i);
        const H = doc.internal.pageSize.getHeight();
        doc.setFontSize(8); doc.setTextColor(120); doc.setFont('helvetica', 'normal');
        doc.text(`Servicio al cliente: ${CONTACTO.telefonoVisible} · ${CONTACTO.correo}`, 40, H - 24);
        doc.text(`Página ${i} de ${pages}`, W - 40, H - 24, { align: 'right' });
      }
      const pdfFilename = 'sistema_isp_reporte.pdf';
      doc.save(pdfFilename);
      showToast('Reporte exportado en formato PDF.');
      openSendEmailModal(pdfFilename);
    }

    document.getElementById('btn-export-pdf').addEventListener('click', exportPDF);

