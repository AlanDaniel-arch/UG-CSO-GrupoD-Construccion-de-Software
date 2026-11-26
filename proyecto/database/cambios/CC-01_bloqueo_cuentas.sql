-- ============================================================================
-- CC-01 · Soporte para el bloqueo de cuentas tras 3 intentos fallidos
-- Documento maestro V2 · §6.4 «Cambio propuesto para la entrega parcial», RNF-05 y §9.3
-- ESTADO: PENDIENTE de aplicar (se aplica en la entrega parcial, semana 6).
-- Ejecutar DESPUÉS de base_datos_isp.sql y solo una vez (el ALTER falla si las
-- columnas ya existen). Cuando se apruebe, incorporar estas columnas a la
-- definición de `usuarios` en base_datos_isp.sql y borrar este archivo.
-- ============================================================================
USE isp_gestion_db;

ALTER TABLE usuarios
  ADD COLUMN intentos_fallidos TINYINT NOT NULL DEFAULT 0,
  ADD COLUMN bloqueada BOOLEAN NOT NULL DEFAULT FALSE;
