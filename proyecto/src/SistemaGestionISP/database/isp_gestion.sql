-- ============================================================================
-- Script de creación de Base de Datos - Sistema de Gestión ISP (Grupo D)
-- Asignatura: Construcción de Software | Avance 1 (U1 - S3)
-- ============================================================================

CREATE DATABASE IF NOT EXISTS isp_gestion;
USE isp_gestion;

-- Módulo 1: Gestión de Clientes y Contratos
CREATE TABLE IF NOT EXISTS clientes (
    id_cliente INT AUTO_INCREMENT PRIMARY KEY,
    cedula VARCHAR(13) NOT NULL UNIQUE,
    nombres VARCHAR(100) NOT NULL,
    apellidos VARCHAR(100) NOT NULL,
    telefono VARCHAR(15),
    direccion VARCHAR(200),
    estado_servicio ENUM('Activo', 'Suspendido', 'Cancelado') DEFAULT 'Activo',
    fecha_registro DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Módulo 2: Gestión de Planes y Equipos
CREATE TABLE IF NOT EXISTS planes_internet (
    id_plan INT AUTO_INCREMENT PRIMARY KEY,
    nombre_plan VARCHAR(80) NOT NULL,
    velocidad_mbps INT NOT NULL,
    precio_mensual DECIMAL(10, 2) NOT NULL
);

CREATE TABLE IF NOT EXISTS contratos (
    id_contrato INT AUTO_INCREMENT PRIMARY KEY,
    id_cliente INT NOT NULL,
    id_plan INT NOT NULL,
    equipo_router VARCHAR(100),
    ip_asignada VARCHAR(45),
    fecha_inicio DATE NOT NULL,
    FOREIGN KEY (id_cliente) REFERENCES clientes(id_cliente),
    FOREIGN KEY (id_plan) REFERENCES planes_internet(id_plan)
);

-- Módulo 3: Facturación y Registro de Pagos
CREATE TABLE IF NOT EXISTS facturas (
    id_factura INT AUTO_INCREMENT PRIMARY KEY,
    id_contrato INT NOT NULL,
    monto DECIMAL(10, 2) NOT NULL,
    fecha_emision DATE NOT NULL,
    pagada BOOLEAN DEFAULT FALSE,
    FOREIGN KEY (id_contrato) REFERENCES contratos(id_contrato)
);

-- Módulo 4: Control de Estado de Servicio y Soporte Básico
CREATE TABLE IF NOT EXISTS tickets_soporte (
    id_ticket INT AUTO_INCREMENT PRIMARY KEY,
    id_cliente INT NOT NULL,
    descripcion VARCHAR(255) NOT NULL,
    estado ENUM('Abierto', 'En Proceso', 'Cerrado') DEFAULT 'Abierto',
    fecha_creacion DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (id_cliente) REFERENCES clientes(id_cliente)
);

-- Datos semilla iniciales para pruebas del Avance 1
INSERT INTO planes_internet (nombre_plan, velocidad_mbps, precio_mensual) VALUES
('Plan Hogar Básico 50 Mbps', 50, 22.50),
('Plan Hogar Plus 100 Mbps', 100, 30.00),
('Plan Fibra Pro 300 Mbps', 300, 45.00);