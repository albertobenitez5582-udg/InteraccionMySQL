-- ============================================================
-- Base de Datos: Sistema de Gestión de Inventario
-- ============================================================

CREATE DATABASE IF NOT EXISTS `inventario_db`
  DEFAULT CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE `inventario_db`;

-- ------------------------------------------------------------
-- Tabla 1: CatPersonal
-- Descripción: Catálogo del personal de la empresa
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS `CatPersonal` (
    `ID` INT AUTO_INCREMENT PRIMARY KEY,
    `Nombre` VARCHAR(80) NOT NULL,
    `Cargo` VARCHAR(80) NOT NULL
) ENGINE=InnoDB COMMENT='Catálogo del personal de la empresa';

-- ------------------------------------------------------------
-- Tabla 2: CatProducto
-- Descripción: Catálogo de productos
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS `CatProducto` (
    `ID` INT AUTO_INCREMENT PRIMARY KEY,
    `Descripcion` VARCHAR(40) NOT NULL
) ENGINE=InnoDB COMMENT='Catálogo de productos';

-- ------------------------------------------------------------
-- Tabla 3: tblInventario
-- Descripción: Tabla de trabajo dónde se gestionará el inventario
-- ------------------------------------------------------------
CREATE TABLE IF NOT EXISTS `tblInventario` (
    `ID` INT AUTO_INCREMENT PRIMARY KEY,
    `FechaAlta` TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    `IDProducto` INT NOT NULL,
    `Cantidad` INT NOT NULL DEFAULT 1,
    CONSTRAINT `fk_inventario_producto`
        FOREIGN KEY (`IDProducto`)
        REFERENCES `CatProducto` (`ID`)
        ON DELETE RESTRICT
        ON UPDATE CASCADE
) ENGINE=InnoDB COMMENT='Tabla de trabajo dónde se gestionará el inventario';
