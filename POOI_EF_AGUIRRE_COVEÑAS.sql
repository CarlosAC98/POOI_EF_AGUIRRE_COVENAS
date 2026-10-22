
CREATE DATABASE ExamenFinal;
GO
USE ExamenFinal;
GO

CREATE TABLE tb_paises(
    idpais int PRIMARY KEY,
    nompais varchar(50) NOT NULL
);

CREATE TABLE tb_consumidor(
    dnicont char(8) PRIMARY KEY,
    nomcont varchar(40) NOT NULL,
    apecont varchar(40) NOT NULL,
    emailcont varchar(250) UNIQUE,
    idpais int REFERENCES tb_paises
);
GO

INSERT INTO tb_paises VALUES (1,'Perú'), (2,'Chile'), (3,'Colombia');
GO

-- CREANDO PROCEDURE AGREGAR
CREATE OR ALTER PROC usp_agrega_consumidor
(@dnicont CHAR(8), @nomcont VARCHAR(40), @apecont VARCHAR(40),
 @emailcont VARCHAR(250), @idpais CHAR(3), @mensaje VARCHAR(100) OUTPUT)
AS
BEGIN
    IF LEN(@dnicont) <> 8 OR @dnicont LIKE '%[^0-9]%'
        SET @mensaje = 'El DNI debe ser numérico de 8 dígitos'
    ELSE IF @nomcont = '' OR @apecont = ''
        SET @mensaje = 'El nombre y el apellido no pueden estar vacíos'
    ELSE
    BEGIN
        INSERT tb_consumidor VALUES (@dnicont, @nomcont, @apecont, @emailcont, @idpais)
        SET @mensaje = 'Consumidor registrado correctamente'
    END
END
GO


-- CREANDO PROCEDURE ELIMINAR
CREATE OR ALTER PROC usp_eliminar_consumidor
(@dnicont CHAR(8), @mensaje VARCHAR(100) OUTPUT)
AS
BEGIN
    IF EXISTS (SELECT * FROM tb_consumidor WHERE dnicont = @dnicont)
    BEGIN
        DELETE tb_consumidor WHERE dnicont = @dnicont
        SET @mensaje = 'Consumidor eliminado correctamente'
    END
    ELSE
        SET @mensaje = 'El consumidor no existe'
END
GO

-- CREANDO PROCEDURE LISTAR
CREATE OR ALTER PROC usp_listar_consumidor
AS
BEGIN
    SELECT C.dnicont, C.nomcont, C.apecont, C.emailcont, C.idpais, P.nompais
    FROM tb_consumidor C JOIN tb_paises P ON C.idpais = P.idpais
END
GO

-- CREANDO PROCEDURE ACTUALIZAR
CREATE OR ALTER PROC usp_actualizar_consumidor
(@dnicont CHAR(8), @nomcont VARCHAR(40), @apecont VARCHAR(40),
 @emailcont VARCHAR(250), @idpais CHAR(3), @mensaje VARCHAR(100) OUTPUT)
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM tb_consumidor WHERE dnicont = @dnicont)
        SET @mensaje = 'El consumidor no existe'
    ELSE IF @nomcont = '' OR @apecont = ''
        SET @mensaje = 'El nombre y el apellido no pueden estar vacíos'
    ELSE
    BEGIN
        UPDATE tb_consumidor
        SET nomcont = @nomcont, apecont = @apecont,
            emailcont = @emailcont, idpais = @idpais
        WHERE dnicont = @dnicont
        SET @mensaje = 'Consumidor actualizado correctamente'
    END
END
GO

-- CREANDO PROCEDURE BUSCAR
CREATE OR ALTER PROC usp_buscar_consumidor
(@dnicont CHAR(8))
AS
BEGIN
    SELECT C.dnicont, C.nomcont, C.apecont, C.emailcont, C.idpais, P.nompais
    FROM tb_consumidor C JOIN tb_paises P ON C.idpais = P.idpais
    WHERE C.dnicont = @dnicont
END
GO
