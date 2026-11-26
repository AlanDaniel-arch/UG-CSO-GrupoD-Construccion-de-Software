    /**
     * forms.js — Ventana modal y formularios de alta/edición de los cuatro módulos.
     *
     * Patrón común: cada `openXxxModal()` arma los campos HTML y entrega a
     * `openModal()` una función `onSubmit(FormData)` que modifica `state`, guarda
     * con `saveState()` y vuelve a dibujar con `renderAll()` (definidos en app.js).
     * Con `existingId` el formulario edita un registro; sin él, crea uno nuevo.
     */

    // Función que se ejecuta al enviar el formulario de la ventana modal abierta.
    let currentModalHandler = null;

    // --- MODAL HELPERS ---
    /**
     * Abre la ventana modal genérica.
     * @param {string} title título de la ventana
     * @param {string} fieldsHtml campos del formulario (HTML)
     * @param {function(FormData):void} onSubmit acción al guardar
     */
    function openModal(title, fieldsHtml, onSubmit) {
      document.getElementById('modal-title').textContent = title;
      document.getElementById('modal-fields').innerHTML = fieldsHtml;
      currentModalHandler = onSubmit;
      const bg = document.getElementById('modal-backdrop');
      bg.classList.remove('hidden');
      bg.classList.add('flex');
      if (window.lucide) lucide.createIcons();
    }

    /** Cierra la ventana modal y descarta el manejador pendiente. */
    function closeModal() {
      const bg = document.getElementById('modal-backdrop');
      bg.classList.add('hidden');
      bg.classList.remove('flex');
      currentModalHandler = null;
    }

    /** Módulo 1: alta/edición de cliente junto con su contrato. La cédula no se puede editar. */
    function openClientModal(existingId = null) {
      const c = existingId ? state.clientes.find(x => x.id === existingId) : null;
      const planOptions = state.planes.map(p =>
        `<option value="${p.id}" ${c && c.planId === p.id ? 'selected' : ''}>${p.nombre} — $${Number(p.precio).toFixed(2)}</option>`
      ).join('');
      const nextNum = String(state.clientes.length + 1).padStart(3, '0');
      openModal(c ? 'Editar Cliente y Contrato' : 'Registrar Cliente y Contrato', `
        <div class="grid grid-cols-1 sm:grid-cols-2 gap-3">
          <div>
            <label class="block text-xs font-medium text-slate-600 mb-1">Cédula / RUC</label>
            <input name="id" required value="${c ? c.id : ''}" ${c ? 'readonly class="w-full px-3 py-2 text-sm border border-slate-200 bg-slate-100 rounded-lg"' : 'class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg"'} placeholder="09XXXXXXXX" />
          </div>
          <div>
            <label class="block text-xs font-medium text-slate-600 mb-1">Código de Contrato</label>
            <input name="contrato" required value="${c ? c.contrato : 'CTR-2026-' + nextNum}" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg" />
          </div>
        </div>
        <div>
          <label class="block text-xs font-medium text-slate-600 mb-1">Nombres y Apellidos</label>
          <input name="nombre" required value="${c ? c.nombre : ''}" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg" />
        </div>
        <div>
          <label class="block text-xs font-medium text-slate-600 mb-1">Dirección de Instalación</label>
          <input name="direccion" required value="${c ? c.direccion : ''}" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg" />
        </div>
        <div class="grid grid-cols-1 sm:grid-cols-2 gap-3">
          <div>
            <label class="block text-xs font-medium text-slate-600 mb-1">Teléfono</label>
            <input name="telefono" required value="${c ? c.telefono : ''}" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg" />
          </div>
          <div>
            <label class="block text-xs font-medium text-slate-600 mb-1">Correo Electrónico</label>
            <input name="correo" type="email" required value="${c ? c.correo : ''}" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg" />
          </div>
        </div>
        <div class="grid grid-cols-1 sm:grid-cols-2 gap-3">
          <div>
            <label class="block text-xs font-medium text-slate-600 mb-1">Plan Contratado</label>
            <select name="planId" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg bg-white">${planOptions}</select>
          </div>
          <div>
            <label class="block text-xs font-medium text-slate-600 mb-1">Estado del Contrato</label>
            <select name="estadoContrato" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg bg-white">
              <option value="Activo" ${!c || c.estadoContrato === 'Activo' ? 'selected' : ''}>Activo</option>
              <option value="Suspendido" ${c && c.estadoContrato === 'Suspendido' ? 'selected' : ''}>Suspendido</option>
              <option value="Cancelado" ${c && c.estadoContrato === 'Cancelado' ? 'selected' : ''}>Cancelado</option>
            </select>
          </div>
        </div>
      `, (fd) => {
        const record = {
          id: fd.get('id').trim(),
          nombre: fd.get('nombre').trim(),
          direccion: fd.get('direccion').trim(),
          telefono: fd.get('telefono').trim(),
          correo: fd.get('correo').trim(),
          fechaRegistro: c ? c.fechaRegistro : todayISO(),
          contrato: fd.get('contrato').trim(),
          fechaInicio: c ? c.fechaInicio : todayISO(),
          planId: fd.get('planId'),
          estadoContrato: fd.get('estadoContrato'),
          servicioId: c ? c.servicioId : 'SRV-' + nextNum,
          estadoServicio: fd.get('estadoContrato') === 'Activo' ? 'Activo' : 'Suspendido',
          fechaActualizacion: todayISO()
        };
        if (c) {
          const idx = state.clientes.findIndex(x => x.id === existingId);
          state.clientes[idx] = record;
          showToast('Cliente y contrato actualizados.');
        } else {
          state.clientes.unshift(record);
          showToast('Cliente y contrato registrados.');
        }
        saveState();
        renderAll();
      });
    }

    /** Módulo 2: alta/edición de un plan de internet. El código no se puede editar. */
    function openPlanModal(existingId = null) {
      const p = existingId ? state.planes.find(x => x.id === existingId) : null;
      const nextId = 'PLN-' + String(state.planes.length + 1).padStart(2, '0');
      openModal(p ? 'Editar Plan de Internet' : 'Nuevo Plan de Internet', `
        <div class="grid grid-cols-2 gap-3">
          <div>
            <label class="block text-xs font-medium text-slate-600 mb-1">Código</label>
            <input name="id" required value="${p ? p.id : nextId}" ${p ? 'readonly class="w-full px-3 py-2 text-sm border border-slate-200 bg-slate-100 rounded-lg"' : 'class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg"'} />
          </div>
          <div>
            <label class="block text-xs font-medium text-slate-600 mb-1">Velocidad</label>
            <input name="velocidad" required value="${p ? p.velocidad : ''}" placeholder="Ej: 150 Mbps" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg" />
          </div>
        </div>
        <div class="grid grid-cols-2 gap-3">
          <div>
            <label class="block text-xs font-medium text-slate-600 mb-1">Nombre del Plan</label>
            <input name="nombre" required value="${p ? p.nombre : ''}" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg" />
          </div>
          <div>
            <label class="block text-xs font-medium text-slate-600 mb-1">Precio Mensual ($)</label>
            <input name="precio" type="number" step="0.01" min="1" required value="${p ? p.precio : ''}" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg" />
          </div>
        </div>
        <div>
          <label class="block text-xs font-medium text-slate-600 mb-1">Descripción</label>
          <textarea name="descripcion" rows="2" required class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg">${p ? p.descripcion : ''}</textarea>
        </div>
      `, (fd) => {
        const rec = {
          id: fd.get('id').trim(),
          nombre: fd.get('nombre').trim(),
          velocidad: fd.get('velocidad').trim(),
          precio: parseFloat(fd.get('precio')),
          descripcion: fd.get('descripcion').trim(),
          estado: p ? p.estado : 'Activo'
        };
        if (p) {
          const i = state.planes.findIndex(x => x.id === existingId);
          state.planes[i] = rec;
          showToast('Plan actualizado.');
        } else {
          state.planes.push(rec);
          showToast('Plan creado correctamente.');
        }
        saveState();
        renderAll();
      });
    }

    /** Módulo 2: alta/edición de un equipo (CPE/ONT/router) y su contrato asociado. */
    function openEquipModal(existingId = null) {
      const e = existingId ? state.equipos.find(x => x.id === existingId) : null;
      const nextId = 'EQ-' + String(state.equipos.length + 1).padStart(3, '0');
      const ctrOptions = `<option value="Sin asignar">Sin asignar</option>` + state.clientes.map(c =>
        `<option value="${c.contrato}" ${e && e.contrato === c.contrato ? 'selected' : ''}>${c.contrato} (${c.nombre})</option>`
      ).join('');
      openModal(e ? 'Editar Equipo' : 'Registrar Equipo CPE/ONT', `
        <div class="grid grid-cols-2 gap-3">
          <div>
            <label class="block text-xs font-medium text-slate-600 mb-1">Código Equipo</label>
            <input name="id" required value="${e ? e.id : nextId}" ${e ? 'readonly class="w-full px-3 py-2 text-sm border border-slate-200 bg-slate-100 rounded-lg"' : 'class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg"'} />
          </div>
          <div>
            <label class="block text-xs font-medium text-slate-600 mb-1">Tipo</label>
            <input name="tipo" required value="${e ? e.tipo : 'ONT Fibra'}" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg" />
          </div>
        </div>
        <div class="grid grid-cols-2 gap-3">
          <div>
            <label class="block text-xs font-medium text-slate-600 mb-1">Marca y Modelo</label>
            <input name="marcaModelo" required value="${e ? e.marcaModelo : ''}" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg" />
          </div>
          <div>
            <label class="block text-xs font-medium text-slate-600 mb-1">Número de Serie</label>
            <input name="serie" required value="${e ? e.serie : ''}" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg" />
          </div>
        </div>
        <div class="grid grid-cols-2 gap-3">
          <div>
            <label class="block text-xs font-medium text-slate-600 mb-1">Contrato Asociado</label>
            <select name="contrato" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg bg-white">${ctrOptions}</select>
          </div>
          <div>
            <label class="block text-xs font-medium text-slate-600 mb-1">Estado</label>
            <select name="estado" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg bg-white">
              <option value="Asignado" ${e && e.estado === 'Asignado' ? 'selected' : ''}>Asignado</option>
              <option value="Disponible" ${!e || e.estado === 'Disponible' ? 'selected' : ''}>Disponible</option>
              <option value="En mantenimiento" ${e && e.estado === 'En mantenimiento' ? 'selected' : ''}>En mantenimiento</option>
            </select>
          </div>
        </div>
      `, (fd) => {
        const rec = {
          id: fd.get('id').trim(),
          tipo: fd.get('tipo').trim(),
          marcaModelo: fd.get('marcaModelo').trim(),
          serie: fd.get('serie').trim(),
          contrato: fd.get('contrato'),
          estado: fd.get('estado')
        };
        if (e) {
          const i = state.equipos.findIndex(x => x.id === existingId);
          state.equipos[i] = rec;
          showToast('Equipo actualizado.');
        } else {
          state.equipos.push(rec);
          showToast('Equipo registrado.');
        }
        saveState();
        renderAll();
      });
    }

    /** Módulo 3: emisión de una factura nueva en estado «Pendiente». */
    function openInvoiceModal() {
      const nextId = 'FAC-2026-' + (101 + state.facturas.length);
      const clientOpts = state.clientes.map(c => {
        const p = state.planes.find(x => x.id === c.planId);
        return `<option value="${c.id}" data-val="${p ? p.precio : 25}">${c.nombre} (${c.contrato}) — $${p ? p.precio.toFixed(2) : '25.00'}</option>`;
      }).join('');
      openModal('Emitir Nueva Factura', `
        <div>
          <label class="block text-xs font-medium text-slate-600 mb-1">Código de Factura</label>
          <input name="id" required value="${nextId}" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg" />
        </div>
        <div>
          <label class="block text-xs font-medium text-slate-600 mb-1">Cliente y Contrato</label>
          <select name="clienteId" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg bg-white">${clientOpts}</select>
        </div>
        <div class="grid grid-cols-2 gap-3">
          <div>
            <label class="block text-xs font-medium text-slate-600 mb-1">Fecha de Emisión</label>
            <input name="emision" type="date" required value="${todayISO()}" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg" />
          </div>
          <div>
            <label class="block text-xs font-medium text-slate-600 mb-1">Fecha de Vencimiento</label>
            <input name="vencimiento" type="date" required value="${todayISO()}" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg" />
          </div>
        </div>
        <div>
          <label class="block text-xs font-medium text-slate-600 mb-1">Valor Total ($)</label>
          <input name="valor" type="number" step="0.01" min="1" required value="35.00" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg" />
        </div>
      `, (fd) => {
        const cid = fd.get('clienteId');
        const c = state.clientes.find(x => x.id === cid);
        state.facturas.unshift({
          id: fd.get('id').trim(),
          clienteId: cid,
          contrato: c ? c.contrato : 'CTR-2026',
          emision: fd.get('emision'),
          vencimiento: fd.get('vencimiento'),
          valor: parseFloat(fd.get('valor')),
          estado: 'Pendiente'
        });
        saveState();
        renderAll();
        showToast('Factura emitida correctamente.');
      });
    }

    /**
     * Módulo 3: registra el pago de una factura. Crea el pago y marca la factura
     * como «Pagada». El valor pagado es siempre el total de la factura.
     */
    function openPayModal(facturaId) {
      const f = state.facturas.find(x => x.id === facturaId);
      if (!f) return;
      const nextPay = 'PAG-2026-' + String(51 + state.pagos.length).padStart(3, '0');
      openModal(`Registrar Pago — ${f.id}`, `
        <div class="p-3 bg-slate-50 rounded-lg border border-slate-200 text-xs space-y-1">
          <p><span class="font-semibold">Cliente:</span> ${getClientName(f.clienteId)}</p>
          <p><span class="font-semibold">Contrato:</span> ${f.contrato}</p>
          <p><span class="font-semibold">Monto a pagar:</span> $${Number(f.valor).toFixed(2)}</p>
        </div>
        <div class="grid grid-cols-2 gap-3">
          <div>
            <label class="block text-xs font-medium text-slate-600 mb-1">Código de Pago</label>
            <input name="id" required value="${nextPay}" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg" />
          </div>
          <div>
            <label class="block text-xs font-medium text-slate-600 mb-1">Fecha de Pago</label>
            <input name="fecha" type="date" required value="${todayISO()}" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg" />
          </div>
        </div>
        <div>
          <label class="block text-xs font-medium text-slate-600 mb-1">Método de Pago</label>
          <select name="metodo" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg bg-white">
            <option value="Transferencia">Transferencia bancaria</option>
            <option value="Efectivo">Efectivo</option>
            <option value="Tarjeta">Tarjeta de crédito/débito</option>
            <option value="Depósito">Depósito bancario</option>
          </select>
        </div>
      `, (fd) => {
        f.estado = 'Pagada';
        state.pagos.unshift({
          id: fd.get('id').trim(),
          facturaId: f.id,
          clienteId: f.clienteId,
          fecha: fd.get('fecha'),
          valor: f.valor,
          metodo: fd.get('metodo')
        });
        saveState();
        renderAll();
        showToast(`Pago ${fd.get('id')} registrado y factura liquidada.`);
      });
    }

    /** Módulo 4: registra una solicitud de soporte técnico en estado «Abierto». */
    function openTicketModal() {
      const nextId = 'SOP-' + String(state.tickets.length + 1).padStart(3, '0');
      const clientOpts = state.clientes.map(c => `<option value="${c.id}">${c.nombre} (${c.contrato})</option>`).join('');
      openModal('Nueva Solicitud de Soporte Técnico', `
        <div class="grid grid-cols-2 gap-3">
          <div>
            <label class="block text-xs font-medium text-slate-600 mb-1">Código</label>
            <input name="id" required value="${nextId}" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg" />
          </div>
          <div>
            <label class="block text-xs font-medium text-slate-600 mb-1">Fecha</label>
            <input name="fecha" type="date" required value="${todayISO()}" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg" />
          </div>
        </div>
        <div>
          <label class="block text-xs font-medium text-slate-600 mb-1">Cliente</label>
          <select name="clienteId" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg bg-white">${clientOpts}</select>
        </div>
        <div>
          <label class="block text-xs font-medium text-slate-600 mb-1">Descripción del inconveniente</label>
          <textarea name="descripcion" rows="3" required placeholder="Describe el problema reportado por el cliente..." class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg"></textarea>
        </div>
      `, (fd) => {
        state.tickets.unshift({
          id: fd.get('id').trim(),
          clienteId: fd.get('clienteId'),
          descripcion: fd.get('descripcion').trim(),
          fecha: fd.get('fecha'),
          estado: 'Abierto'
        });
        saveState();
        renderAll();
        showToast('Solicitud de soporte registrada.');
      });
    }

    // Eventos de los formularios.

    // Botones que abren cada formulario (accesos rápidos del panel y de cada módulo).
    document.getElementById('quick-add-client').addEventListener('click', () => openClientModal());
    document.getElementById('btn-open-client-modal').addEventListener('click', () => openClientModal());
    document.getElementById('btn-open-plan-modal').addEventListener('click', () => openPlanModal());
    document.getElementById('btn-open-equip-modal').addEventListener('click', () => openEquipModal());
    document.getElementById('quick-add-invoice').addEventListener('click', () => openInvoiceModal());
    document.getElementById('btn-open-invoice-modal').addEventListener('click', () => openInvoiceModal());
    document.getElementById('btn-open-ticket-modal').addEventListener('click', () => openTicketModal());
    document.getElementById('btn-close-modal').addEventListener('click', closeModal);
    document.getElementById('btn-cancel-modal').addEventListener('click', closeModal);

    // Envío del formulario modal: ejecuta el manejador actual y cierra la ventana.
    document.getElementById('modal-form').addEventListener('submit', (e) => {
      e.preventDefault();
      if (currentModalHandler) {
        currentModalHandler(new FormData(e.target));
        closeModal();
      }
    });

    // Event delegation for dynamic table/card actions
    // Delegación de eventos: los botones de tablas y tarjetas se crean al dibujar,
    // por eso se escucha el clic en `document` y se identifica el botón por su atributo data-*.
    document.addEventListener('click', (e) => {
      const editClient = e.target.closest('[data-edit-client]');
      if (editClient) return openClientModal(editClient.dataset.editClient);

      const editPlan = e.target.closest('[data-edit-plan]');
      if (editPlan) return openPlanModal(editPlan.dataset.editPlan);

      const togglePlan = e.target.closest('[data-toggle-plan]');
      if (togglePlan) {
        const p = state.planes.find(x => x.id === togglePlan.dataset.togglePlan);
        if (p) {
          p.estado = p.estado === 'Activo' ? 'Inactivo' : 'Activo';
          saveState(); renderAll(); showToast(`Plan ${p.nombre} marcado como ${p.estado}.`);
        }
        return;
      }

      const editEquip = e.target.closest('[data-edit-equip]');
      if (editEquip) return openEquipModal(editEquip.dataset.editEquip);

      const payInv = e.target.closest('[data-pay-invoice]');
      if (payInv) return openPayModal(payInv.dataset.payInvoice);

      const advTicket = e.target.closest('[data-advance-ticket]');
      if (advTicket) {
        const t = state.tickets.find(x => x.id === advTicket.dataset.advanceTicket);
        if (t) {
          t.estado = t.estado === 'Abierto' ? 'En proceso' : 'Resuelto';
          saveState(); renderAll(); showToast(`Ticket ${t.id} actualizado a ${t.estado}.`);
        }
      }
    });

    // Cambio de estado del servicio desde la lista desplegable del módulo Soporte.
    document.addEventListener('change', (e) => {
      const srv = e.target.closest('[data-change-service]');
      if (srv) {
        const c = state.clientes.find(x => x.id === srv.dataset.changeService);
        if (c) {
          c.estadoServicio = srv.value;
          c.estadoContrato = srv.value === 'Activo' ? 'Activo' : 'Suspendido';
          c.fechaActualizacion = todayISO();
          saveState(); renderAll(); showToast(`Estado de servicio de ${c.nombre} actualizado a ${srv.value}.`);
        }
      }
    });

    /**
     * Tras exportar el PDF, ofrece abrir el cliente de correo (mailto:) con asunto y
     * mensaje listos. El navegador no puede adjuntar el archivo automáticamente.
     * @param {string} pdfFilename nombre del PDF recién descargado
     */
    function openSendEmailModal(pdfFilename) {
      const asuntoDefault = `Reporte ISP Manager (${todayISO()})`;
      const cuerpoDefault = `Hola,\n\nTe comparto el reporte PDF recién descargado (${pdfFilename}) del Sistema de Gestión de Clientes, Planes y Facturación para ISP.\n\nPor favor revisa el archivo adjunto.\n\nSaludos,\n${currentUser ? currentUser.nombre : 'ISP Manager'}`;
      openModal('Enviar reporte PDF por correo', `
        <div class="p-3 bg-sky-50 border border-sky-200 rounded-lg text-xs text-sky-900">
          El archivo <strong>${pdfFilename}</strong> ya se descargó en tu equipo. Indica el destinatario para abrir tu correo listo para adjuntarlo y enviarlo.
        </div>
        <div>
          <label class="block text-xs font-medium text-slate-600 mb-1">Correo del destinatario</label>
          <input name="to" type="email" required value="${CONTACTO.correo}" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg" placeholder="destinatario@correo.com" />
        </div>
        <div>
          <label class="block text-xs font-medium text-slate-600 mb-1">Asunto</label>
          <input name="subject" required value="${asuntoDefault}" class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg" />
        </div>
        <div>
          <label class="block text-xs font-medium text-slate-600 mb-1">Mensaje</label>
          <textarea name="body" rows="4" required class="w-full px-3 py-2 text-sm border border-slate-300 rounded-lg">${cuerpoDefault}</textarea>
        </div>
      `, (fd) => {
        const to = encodeURIComponent(fd.get('to').trim());
        const subject = encodeURIComponent(fd.get('subject').trim());
        const body = encodeURIComponent(fd.get('body').trim());
        const mailtoUrl = `mailto:${to}?subject=${subject}&body=${body}`;
        const a = document.createElement('a');
        a.href = mailtoUrl;
        a.click();
        showToast(`Abriendo correo para ${fd.get('to').trim()}...`);
      });
    }
