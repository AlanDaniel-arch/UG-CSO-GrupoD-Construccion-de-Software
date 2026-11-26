    // =====================================================
    //  USUARIOS Y ROLES (demo en el navegador)
    // =====================================================
    const USUARIOS = [
      { usuario: 'admin',      clave: 'admin123',      nombre: 'Administrador del Sistema', rol: 'Administrador', icono: 'shield-check',
        vistas: ['dashboard','clientes','planes','facturacion','soporte'], edita: ['clientes','planes','facturacion','soporte'] },
      { usuario: 'gerente',    clave: 'gerente123',    nombre: 'Gerente General',           rol: 'Gerente',       icono: 'briefcase',
        vistas: ['dashboard','clientes','planes','facturacion','soporte'], edita: [] },
      { usuario: 'secretario', clave: 'secretario123', nombre: 'Secretaría',                rol: 'Secretario',    icono: 'clipboard-list',
        vistas: ['dashboard','clientes','soporte'], edita: ['clientes','soporte'] },
      { usuario: 'cajero',     clave: 'cajero123',     nombre: 'Caja y Cobranza',           rol: 'Cajero',        icono: 'wallet',
        vistas: ['dashboard','facturacion'], edita: ['facturacion'] },
      { usuario: 'tecnico',    clave: 'tecnico123',    nombre: 'Soporte Técnico',           rol: 'Técnico',       icono: 'wrench',
        vistas: ['dashboard','planes','soporte'], edita: ['planes','soporte'] }
    ];
    const SESSION_KEY = 'ug_isp_grupoD_session_v1';
    let currentUser = null;

    const EDIT_TARGETS = {
      clientes:    '#btn-open-client-modal, #quick-add-client, [data-edit-client]',
      planes:      '#btn-open-plan-modal, #btn-open-equip-modal, [data-edit-plan], [data-toggle-plan], [data-edit-equip]',
      facturacion: '#btn-open-invoice-modal, #quick-add-invoice, [data-pay-invoice]',
      soporte:     '#btn-open-ticket-modal, [data-advance-ticket], [data-change-service]'
    };

    function can(modulo) { return !!currentUser && currentUser.edita.includes(modulo); }

    function applyPermissions() {
      if (!currentUser) return;
      document.querySelectorAll('.nav-btn').forEach(b => {
        b.classList.toggle('hidden', !currentUser.vistas.includes(b.dataset.view));
      });
      Object.entries(EDIT_TARGETS).forEach(([mod, sel]) => {
        const allowed = can(mod);
        document.querySelectorAll(sel).forEach(el => {
          if (el.tagName === 'SELECT') { el.disabled = !allowed; el.classList.toggle('opacity-60', !allowed); }
          else el.classList.toggle('hidden', !allowed);
        });
      });
      document.getElementById('btn-reset-demo').classList.toggle('hidden', currentUser.rol !== 'Administrador');
      document.querySelectorAll('.jump-btn').forEach(b => {
        b.classList.toggle('hidden', !currentUser.vistas.includes(b.dataset.jump));
      });
    }

    function startSession(u) {
      currentUser = u;
      try { sessionStorage.setItem(SESSION_KEY, u.usuario); } catch (e) {}
      document.getElementById('login-screen').classList.add('hidden');
      document.getElementById('login-screen').classList.remove('flex');
      document.getElementById('user-name').textContent = u.nombre;
      document.getElementById('user-role').textContent = u.rol;
      switchView('dashboard');
      showToast(`Bienvenido/a, ${u.nombre} (${u.rol}).`);
    }

    function showLogin() {
      currentUser = null;
      try { sessionStorage.removeItem(SESSION_KEY); } catch (e) {}
      const s = document.getElementById('login-screen');
      s.classList.remove('hidden'); s.classList.add('flex');
      document.getElementById('login-form').reset();
      document.getElementById('login-error').classList.add('hidden');
      if (window.lucide) lucide.createIcons();
    }

    function setupLogin() {
      document.getElementById('role-cards').innerHTML = USUARIOS.map(u => `
        <button type="button" data-demo-user="${u.usuario}" class="role-card flex items-center gap-2.5 text-left border border-slate-200 rounded-xl px-3 py-2">
          <span class="w-8 h-8 rounded-lg bg-sky-100 text-sky-700 flex items-center justify-center shrink-0"><i data-lucide="${u.icono}" class="w-4 h-4"></i></span>
          <span class="min-w-0">
            <span class="block text-sm font-semibold text-slate-800">${u.rol}</span>
            <span class="block text-[11px] text-slate-400 truncate">${u.usuario} / ${u.clave}</span>
          </span>
        </button>`).join('');

      document.getElementById('role-cards').addEventListener('click', (e) => {
        const b = e.target.closest('[data-demo-user]');
        if (!b) return;
        const u = USUARIOS.find(x => x.usuario === b.dataset.demoUser);
        document.getElementById('login-user').value = u.usuario;
        document.getElementById('login-pass').value = u.clave;
      });

      document.getElementById('login-form').addEventListener('submit', (e) => {
        e.preventDefault();
        const user = document.getElementById('login-user').value.trim().toLowerCase();
        const pass = document.getElementById('login-pass').value;
        const u = USUARIOS.find(x => x.usuario === user && x.clave === pass);
        if (!u) { document.getElementById('login-error').classList.remove('hidden'); return; }
        startSession(u);
      });

      document.getElementById('btn-logout').addEventListener('click', showLogin);

      let saved = null;
      try { saved = sessionStorage.getItem(SESSION_KEY); } catch (e) {}
      const u = USUARIOS.find(x => x.usuario === saved);
      if (u) startSession(u); else showLogin();
    }

    function canView(viewName) {
      return !currentUser || currentUser.vistas.includes(viewName);
    }

    // Inicia el prototipo cuando los tres scripts ya están disponibles.
    setupContacto();
    setupLogin();
    renderAll();
