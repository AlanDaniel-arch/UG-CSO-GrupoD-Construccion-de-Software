-- ============================================================================
-- Base de Datos: Sistema de Gestión de Clientes, Planes y Facturación para ISP
-- Motor: MySQL 8.0+ (Compatible con MySQL Workbench)
-- Sincronizado con los módulos, roles y datos de contacto de la interfaz web
-- ============================================================================

CREATE DATABASE IF NOT EXISTS isp_gestion_db
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE isp_gestion_db;

-- 1. Tabla de Usuarios y Roles (5 perfiles del sistema)
CREATE TABLE IF NOT EXISTS usuarios (
  id_usuario INT AUTO_INCREMENT PRIMARY KEY,
  usuario VARCHAR(50) NOT NULL UNIQUE,
  nombre_completo VARCHAR(120) NOT NULL,
  correo VARCHAR(120) NOT NULL UNIQUE,
  clave_hash VARCHAR(255) NOT NULL,
  rol ENUM('Administrador', 'Gerente', 'Secretario', 'Cajero', 'Técnico') NOT NULL,
  estado ENUM('Activo', 'Inactivo') NOT NULL DEFAULT 'Activo',
  fecha_creacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-- 2. Tabla de Clientes
CREATE TABLE IF NOT EXISTS clientes (
  id_cliente INT AUTO_INCREMENT PRIMARY KEY,
  identificacion VARCHAR(20) NOT NULL UNIQUE,
  nombres_apellidos VARCHAR(150) NOT NULL,
  direccion VARCHAR(255) NOT NULL,
  telefono VARCHAR(25) NOT NULL,
  correo VARCHAR(120) NOT NULL,
  fecha_registro DATE NOT NULL
);

-- 3. Tabla de Planes de Internet
CREATE TABLE IF NOT EXISTS planes (
  id_plan INT AUTO_INCREMENT PRIMARY KEY,
  codigo_plan VARCHAR(20) NOT NULL UNIQUE,
  nombre_plan VARCHAR(100) NOT NULL,
  velocidad VARCHAR(40) NOT NULL,
  precio_mensual DECIMAL(10, 2) NOT NULL,
  descripcion TEXT,
  estado ENUM('Activo', 'Inactivo') NOT NULL DEFAULT 'Activo'
);

-- 4. Tabla de Contratos
CREATE TABLE IF NOT EXISTS contratos (
  id_contrato INT AUTO_INCREMENT PRIMARY KEY,
  codigo_contrato VARCHAR(30) NOT NULL UNIQUE,
  id_cliente INT NOT NULL,
  id_plan INT NOT NULL,
  fecha_inicio DATE NOT NULL,
  estado_contrato ENUM('Activo', 'Suspendido', 'Cancelado') NOT NULL DEFAULT 'Activo',
  CONSTRAINT fk_contrato_cliente FOREIGN KEY (id_cliente) REFERENCES clientes(id_cliente)
    ON UPDATE CASCADE ON DELETE RESTRICT,
  CONSTRAINT fk_contrato_plan FOREIGN KEY (id_plan) REFERENCES planes(id_plan)
    ON UPDATE CASCADE ON DELETE RESTRICT
);

-- 5. Tabla de Equipos (CPE / ONT / Routers)
CREATE TABLE IF NOT EXISTS equipos (
  id_equipo INT AUTO_INCREMENT PRIMARY KEY,
  codigo_equipo VARCHAR(25) NOT NULL UNIQUE,
  tipo_equipo VARCHAR(60) NOT NULL,
  marca_modelo VARCHAR(100) NOT NULL,
  numero_serie VARCHAR(80) NOT NULL UNIQUE,
  id_contrato INT NULL,
  estado_equipo ENUM('Asignado', 'Disponible', 'En mantenimiento') NOT NULL DEFAULT 'Disponible',
  CONSTRAINT fk_equipo_contrato FOREIGN KEY (id_contrato) REFERENCES contratos(id_contrato)
    ON UPDATE CASCADE ON DELETE SET NULL
);

-- 6. Tabla de Facturas
CREATE TABLE IF NOT EXISTS facturas (
  id_factura INT AUTO_INCREMENT PRIMARY KEY,
  codigo_factura VARCHAR(30) NOT NULL UNIQUE,
  id_cliente INT NOT NULL,
  id_contrato INT NOT NULL,
  fecha_emision DATE NOT NULL,
  fecha_vencimiento DATE NOT NULL,
  valor_total DECIMAL(10, 2) NOT NULL,
  estado_factura ENUM('Pendiente', 'Pagada', 'Vencida') NOT NULL DEFAULT 'Pendiente',
  CONSTRAINT fk_factura_cliente FOREIGN KEY (id_cliente) REFERENCES clientes(id_cliente)
    ON UPDATE CASCADE ON DELETE RESTRICT,
  CONSTRAINT fk_factura_contrato FOREIGN KEY (id_contrato) REFERENCES contratos(id_contrato)
    ON UPDATE CASCADE ON DELETE RESTRICT
);

-- 7. Tabla de Pagos
CREATE TABLE IF NOT EXISTS pagos (
  id_pago INT AUTO_INCREMENT PRIMARY KEY,
  codigo_pago VARCHAR(30) NOT NULL UNIQUE,
  id_factura INT NOT NULL,
  id_cliente INT NOT NULL,
  fecha_pago DATE NOT NULL,
  valor_pagado DECIMAL(10, 2) NOT NULL,
  metodo_pago ENUM('Efectivo', 'Transferencia', 'Tarjeta', 'Depósito') NOT NULL,
  CONSTRAINT fk_pago_factura FOREIGN KEY (id_factura) REFERENCES facturas(id_factura)
    ON UPDATE CASCADE ON DELETE RESTRICT,
  CONSTRAINT fk_pago_cliente FOREIGN KEY (id_cliente) REFERENCES clientes(id_cliente)
    ON UPDATE CASCADE ON DELETE RESTRICT
);

-- 8. Tabla de Estado de Servicios
CREATE TABLE IF NOT EXISTS servicios (
  id_servicio INT AUTO_INCREMENT PRIMARY KEY,
  codigo_servicio VARCHAR(25) NOT NULL UNIQUE,
  id_cliente INT NOT NULL,
  id_contrato INT NOT NULL UNIQUE,
  estado_servicio ENUM('Activo', 'Suspendido', 'Pendiente') NOT NULL DEFAULT 'Activo',
  fecha_actualizacion DATE NOT NULL,
  CONSTRAINT fk_servicio_cliente FOREIGN KEY (id_cliente) REFERENCES clientes(id_cliente)
    ON UPDATE CASCADE ON DELETE RESTRICT,
  CONSTRAINT fk_servicio_contrato FOREIGN KEY (id_contrato) REFERENCES contratos(id_contrato)
    ON UPDATE CASCADE ON DELETE CASCADE
);

-- 9. Tabla de Solicitudes de Soporte Técnico
CREATE TABLE IF NOT EXISTS soporte_tecnico (
  id_soporte INT AUTO_INCREMENT PRIMARY KEY,
  codigo_solicitud VARCHAR(25) NOT NULL UNIQUE,
  id_cliente INT NOT NULL,
  descripcion_inconveniente TEXT NOT NULL,
  fecha_registro DATE NOT NULL,
  estado_solicitud ENUM('Abierto', 'En proceso', 'Resuelto') NOT NULL DEFAULT 'Abierto',
  id_tecnico_asignado INT NULL,
  CONSTRAINT fk_soporte_cliente FOREIGN KEY (id_cliente) REFERENCES clientes(id_cliente)
    ON UPDATE CASCADE ON DELETE RESTRICT,
  CONSTRAINT fk_soporte_tecnico FOREIGN KEY (id_tecnico_asignado) REFERENCES usuarios(id_usuario)
    ON UPDATE CASCADE ON DELETE SET NULL
);

-- 10. Tabla de Configuración de Contacto (Atención al Cliente y WhatsApp)
CREATE TABLE IF NOT EXISTS configuracion_contacto (
  id_config INT AUTO_INCREMENT PRIMARY KEY,
  telefono_visible VARCHAR(40) NOT NULL,
  whatsapp_numero VARCHAR(20) NOT NULL,
  correo_soporte VARCHAR(120) NOT NULL,
  horario_atencion VARCHAR(100) NOT NULL,
  mensaje_whatsapp VARCHAR(255) NOT NULL
);

-- ============================================================================
-- Datos Iniciales (Sincronizados con la aplicación web)
-- ============================================================================

INSERT INTO usuarios (usuario, nombre_completo, correo, clave_hash, rol) VALUES
('admin',      'Administrador del Sistema', 'admin@ispmanager.ec',             SHA2('admin123', 256),      'Administrador'),
('gerente',    'Gerente General',           'gerente@ispmanager.ec',           SHA2('gerente123', 256),    'Gerente'),
('secretario', 'Secretaría',                'secretario@ispmanager.ec',        SHA2('secretario123', 256), 'Secretario'),
('cajero',     'Caja y Cobranza',           'cajero@ispmanager.ec',            SHA2('cajero123', 256),     'Cajero'),
('tecnico',    'Soporte Técnico',           'belinda.tenegusnaytib@ug.edu.ec', SHA2('tecnico123', 256),    'Técnico');

INSERT INTO configuracion_contacto (telefono_visible, whatsapp_numero, correo_soporte, horario_atencion, mensaje_whatsapp) VALUES
('0978834917 (+593 97 883 4917)', '593978834917', 'belinda.tenegusnaytib@ug.edu.ec', 'Lun - Sáb · 08:00 a 20:00', 'Hola, necesito soporte técnico con mi servicio de internet.');

INSERT INTO planes (codigo_plan, nombre_plan, velocidad, precio_mensual, descripcion, estado) VALUES
('PLN-01', 'Fibra Hogar 100', '100 Mbps', 25.00, 'Ideal para navegación, teletrabajo y streaming HD.', 'Activo'),
('PLN-02', 'Fibra Plus 250', '250 Mbps', 35.00, 'Alta velocidad simétrica para hogares conectados.', 'Activo'),
('PLN-03', 'Fibra Pro PyME 500', '500 Mbps', 58.00, 'Plan empresarial con soporte prioritario e IP fija.', 'Activo');

INSERT INTO clientes (identificacion, nombres_apellidos, direccion, telefono, correo, fecha_registro) VALUES
('0924815201', 'Carlos Mendoza Vera', 'Cdla. Alborada 7ma Etapa Mz 714', '0994128831', 'cmendoza@correo.ec', '2026-09-12'),
('0918743310', 'María Fernanda Paredes', 'Av. Francisco de Orellana Km 3', '0987654120', 'mparedes@correo.ec', '2026-10-02'),
('0951209844', 'Luis Eduardo Zambrano', 'Sauces 4 Mz 281 Villa 14', '0991204458', 'lzambrano@correo.ec', '2026-08-19');

INSERT INTO contratos (codigo_contrato, id_cliente, id_plan, fecha_inicio, estado_contrato) VALUES
('CTR-2026-001', 1, 2, '2026-09-15', 'Activo'),
('CTR-2026-002', 2, 3, '2026-10-05', 'Activo'),
('CTR-2026-003', 3, 1, '2026-08-20', 'Suspendido');

INSERT INTO equipos (codigo_equipo, tipo_equipo, marca_modelo, numero_serie, id_contrato, estado_equipo) VALUES
('EQ-001', 'ONT Fibra', 'Huawei HG8145V5', 'HW-9948120A', 1, 'Asignado'),
('EQ-002', 'Router Dual Band', 'TP-Link Archer AX23', 'TP-5582910C', 2, 'Asignado'),
('EQ-003', 'ONT Fibra', 'ZTE F670L', 'ZT-3310844B', 3, 'Asignado'),
('EQ-004', 'Router Wi-Fi 6', 'MikroTik hAP ax2', 'MK-7721004D', NULL, 'Disponible');

INSERT INTO facturas (codigo_factura, id_cliente, id_contrato, fecha_emision, fecha_vencimiento, valor_total, estado_factura) VALUES
('FAC-2026-101', 1, 1, '2026-11-01', '2026-11-15', 35.00, 'Pagada'),
('FAC-2026-102', 2, 2, '2026-11-05', '2026-11-28', 58.00, 'Pendiente'),
('FAC-2026-103', 3, 3, '2026-10-20', '2026-11-05', 25.00, 'Vencida');

INSERT INTO pagos (codigo_pago, id_factura, id_cliente, fecha_pago, valor_pagado, metodo_pago) VALUES
('PAG-2026-051', 1, 1, '2026-11-10', 35.00, 'Transferencia');

INSERT INTO servicios (codigo_servicio, id_cliente, id_contrato, estado_servicio, fecha_actualizacion) VALUES
('SRV-001', 1, 1, 'Activo', '2026-11-20'),
('SRV-002', 2, 2, 'Activo', '2026-11-22'),
('SRV-003', 3, 3, 'Suspendido', '2026-11-18');

INSERT INTO soporte_tecnico (codigo_solicitud, id_cliente, descripcion_inconveniente, fecha_registro, estado_solicitud, id_tecnico_asignado) VALUES
('SOP-001', 2, 'Intermitencia en la señal Wi-Fi durante horas pico.', '2026-11-23', 'En proceso', 5),
('SOP-002', 3, 'Solicitud de reconexión tras consulta de saldo pendiente.', '2026-11-24', 'Abierto', 5);