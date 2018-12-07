USE MASTER
GO

/* ============================== ESTRUCTURA DE LA BASE DE DATOS ================================= */
IF DB_ID('BD_ExamenOnline') IS NOT NULL
	DROP DATABASE BD_ExamenOnline
GO
CREATE DATABASE BD_ExamenOnline
GO

USE BD_ExamenOnline
GO

SET DATEFORMAT YMD
GO

-- Tabla TipoUsuario: Esta tabla permite identificar el tipo de Usuario luego de registrarse o comprar una suscripcion: Básico(1), Premium(2), Gold(3)
IF OBJECT_ID('TipoUsuario') IS NOT NULL
	DROP TABLE TipoUsuario
GO
CREATE TABLE TipoUsuario
(
	IdTipoUsuario INT NOT NULL PRIMARY KEY, -- NO ES IDENTITY
	Descripcion VARCHAR(100), -- FREE, PREMIUM, GOLD
	Estado			BIT NOT NULL,
	Crea			VARCHAR(100) NOT NULL,
	Modifica		VARCHAR(100) NOT NULL,
	Elimina			VARCHAR(100) NOT NULL
)
GO

-- Tabla Suscripcion
IF OBJECT_ID('Suscripcion') IS NOT NULL
	DROP TABLE Suscripcion
GO
CREATE TABLE Suscripcion
(
	IdSuscripcion	INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Descripcion		VARCHAR(50) NOT NULL, -- PREMIUM, ..
	Precio			DECIMAL(18, 2) NOT NULL, -- PRECIO POR MES EN SOLES
	IdTipoUsuario	INT REFERENCES TipoUsuario(IdTipoUsuario),
	Estado			BIT NOT NULL,
	Crea			VARCHAR(100) NOT NULL,
	Modifica		VARCHAR(100) NOT NULL,
	Elimina			VARCHAR(100) NOT NULL
)
GO

-- Detalle: fECHA de compra, Fecha de expiración de la suscripción ()

-- Tabla Usuario
IF OBJECT_ID('Usuario') IS NOT NULL
	DROP TABLE Usuario
GO
CREATE TABLE Usuario
(
	IdUsuario		INT			NOT NULL PRIMARY KEY IDENTITY(1,1),
	Nombres			VARCHAR(50)	NOT NULL,
	ApellidoPaterno	VARCHAR(50)	NOT NULL,
	ApellidoMaterno	VARCHAR(50)	NOT NULL,
	Correo			VARCHAR(50) NOT NULL,
	Dni				VARCHAR(50)	NOT NULL,
	Usuario			VARCHAR(50)	NOT NULL UNIQUE,
	Contraseña		VARCHAR(50)	NOT NULL,
	imagen text,
	FechaRegistro	DATETIME NOT NULL,
	Intentos		INT NOT NULL,
	Bloqueado		BIT NOT NULL,
	IdTipoUsuario	INT REFERENCES TipoUsuario(IdTipoUsuario) NOT NULL, -- Por defecto es FREE(1)
	Estado			BIT NOT NULL,
	Crea			VARCHAR(100) NOT NULL,
	Modifica		VARCHAR(100) NOT NULL,
	Elimina			VARCHAR(100) NOT NULL
)
GO

-- Tabla Categoria
IF OBJECT_ID('Categoria') IS NOT NULL
	DROP TABLE Categoria
GO
CREATE TABLE Categoria
(
	IdCategoria		INT			NOT NULL PRIMARY KEY IDENTITY(1, 1),
	NombreCategoria	VARCHAR(50) NOT NULL,
	IconoCategoria	VARCHAR(50),
	UrlCategoria	VARCHAR(50),
	Estado			BIT NOT NULL,
	Crea			VARCHAR(100) NOT NULL,
	Modifica		VARCHAR(100) NOT NULL,
	Elimina			VARCHAR(100) NOT NULL
)
GO

-- Tabla Enlace
IF OBJECT_ID('Enlace') IS NOT NULL
	DROP TABLE Enlace
GO
CREATE TABLE Enlace
(
	IdEnlace		INT				NOT NULL PRIMARY KEY IDENTITY(1,1),
	NombreEnlace	VARCHAR(50)		NOT NULL,
	UrlEnlace		VARCHAR(100)	NOT NULL,
	EnlaceIcono		VARCHAR(100),
	Estado			BIT NOT NULL,
	Crea			VARCHAR(100) NOT NULL,
	Modifica		VARCHAR(100) NOT NULL,
	Elimina			VARCHAR(100) NOT NULL
)
GO

-- Tabla Rol
IF OBJECT_ID('Rol') IS NOT NULL
	DROP TABLE Rol
GO
CREATE TABLE Rol(
	IdRol			INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	NombreRol		VARCHAR(50)		NOT NULL,
	Estado			BIT NOT NULL,
	Crea			VARCHAR(100) NOT NULL,
	Modifica		VARCHAR(100) NOT NULL,
	Elimina			VARCHAR(100) NOT NULL
)
GO

-- Tabla UsuarioRol
IF OBJECT_ID('UsuarioRol') IS NOT NULL
	DROP TABLE UsuarioRol
GO
CREATE TABLE UsuarioRol(
	IdUsuario	INT NOT NULL REFERENCES Usuario(IdUsuario),
	IdRol		INT	NOT NULL REFERENCES Rol(IdRol),
	PRIMARY KEY(IdUsuario, IdRol)
)
GO

-- Tabla RolCategoria
IF OBJECT_ID('RolCategoria') IS NOT NULL
	DROP TABLE RolCategoria
GO
CREATE TABLE RolCategoria(
	IdRol		INT NOT NULL REFERENCES Rol(IdRol),
	IdCategoria	INT	NOT NULL REFERENCES Categoria(IdCategoria),
	PRIMARY KEY(IdRol, IdCategoria)
)
GO

-- Tabla EnlaceCategoria
IF OBJECT_ID('EnlaceCategoria') IS NOT NULL
	DROP TABLE EnlaceCategoria
GO
CREATE TABLE EnlaceCategoria(
	IdEnlace	INT NOT NULL REFERENCES Enlace(IdEnlace),
	IdCategoria	INT	NOT NULL REFERENCES Categoria(IdCategoria),
	PRIMARY KEY(IdEnlace, IdCategoria)
)
GO

IF OBJECT_ID('Asignatura') IS NOT NULL
	DROP TABLE Asignatura
GO
CREATE TABLE Asignatura
(
	IdAsignatura		INT			NOT NULL PRIMARY KEY IDENTITY(1,1),
	Nombre				VARCHAR(100) NOT NULL, -- Privado, Público
	Estado				BIT NOT NULL
);


IF OBJECT_ID('TipoExamen') IS NOT NULL
	DROP TABLE TipoExamen
GO
CREATE TABLE TipoExamen
(
	IdTipoExamen		INT			NOT NULL PRIMARY KEY IDENTITY(1,1),
	Descripcion			VARCHAR(100) NOT NULL, -- Privado, Público
	MaxPreguntas		INT NOT NULL-- Número máximo de preguntas
);


IF OBJECT_ID('Compra') IS NOT NULL
	DROP TABLE Compra
GO
CREATE TABLE Compra
(
	IdCompra	INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	IdUsuario	INT NOT NULL REFERENCES Usuario(IdUsuario),
	FechaCompra	DATETIME NOT NULL,
	Total decimal(18,2)
)
GO

IF OBJECT_ID('DetalleCompra') IS NOT NULL
	DROP TABLE DetalleCompra
GO
CREATE TABLE DetalleCompra
(
	IdDetalleCompra	INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdCompra	INT NOT NULL REFERENCES Compra(IdCompra),
	IdSuscripcion	INT NOT NULL REFERENCES Suscripcion(IdSuscripcion),
	CantidadMeses	INT NOT NULL,
	PrecioPorMes DECIMAL(18,2),
	FechaExpiracion	DATETIME NOT NULL, 
	Estado		BIT NOT NULL -- 0: Ya expiró | 1: No expira
)
GO


IF OBJECT_ID('Examen') IS NOT NULL
	DROP TABLE Examen
GO
CREATE TABLE Examen
(
	IdExamen			INT			NOT NULL PRIMARY KEY IDENTITY(1,1),
	Clave				VARCHAR(50) DEFAULT(''), -- Clave para acceder al Examen
	FechaRegistro		DATETIME	NOT NULL,
	FechaExpiracion		DATETIME	NOT NULL, -- Fecha para cuando el examen ya no puede resolverse (Lo determina el que crea el examen)
	IdUsuario			INT REFERENCES Usuario(IdUsuario),
	IdTipoExamen		INT REFERENCES TipoExamen(IdTipoExamen),
	IdAsignatura		INT REFERENCES Asignatura(IdAsignatura),
	Titulo				VARCHAR(50) NOT NULL,
	Descripcion			VARCHAR(MAX) NOT NULL,
	TiempoMaximo		INT			NOT NULL, -- Tiempo de duración del Examen en minutos
	EscalaCalificacion	INT			NOT NULL -- Es la escala de calificación que se aplicará (puntaje máximo): 20, 100, eso lo determina el creador del examen
);


IF OBJECT_ID('Pregunta') IS NOT NULL
	DROP TABLE Pregunta
GO
CREATE TABLE Pregunta
(
	IdPregunta	INT				NOT NULL PRIMARY KEY IDENTITY(1, 1),
	Pregunta	NVARCHAR(MAX)	NOT NULL, -- Descripción de la pregunta HTML
	Numero		INT NOT NULL,
	IdExamen	INT NOT NULL REFERENCES Examen(IdExamen)
);


IF OBJECT_ID('Imagen') IS NOT NULL
	DROP TABLE Imagen
GO
CREATE TABLE Imagen
(
	IdImagen	INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	Imagen		IMAGE,
	IdPregunta	INT NOT NULL REFERENCES Pregunta(IdPregunta)
);

IF OBJECT_ID('Video') IS NOT NULL
	DROP TABLE Video
GO
CREATE TABLE Video
(
	IdVideo		INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	Video		VARBINARY(MAX),
	IdPregunta	INT NOT NULL REFERENCES Pregunta(IdPregunta)
);

IF OBJECT_ID('Alternativa') IS NOT NULL
	DROP TABLE Alternativa
GO
CREATE TABLE Alternativa
(
	IdAlternativa	INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	IdPregunta		INT NOT NULL REFERENCES Pregunta(IdPregunta),
	Numero			INT NOT NULL,
	Descripcion		VARCHAR(100),
	OpcionCorrecta	BIT
);

IF OBJECT_ID('ExamenRealizado') IS NOT NULL
	DROP TABLE ExamenRealizado
GO
CREATE TABLE ExamenRealizado
(
	IdExamenRealizado			INT	NOT NULL PRIMARY KEY IDENTITY(1, 1),
	IdUsuario					INT	REFERENCES Usuario(IdUsuario),
	IdExamen					INT	REFERENCES Examen(IdExamen),
	FechaRealizacion			DATETIME NOT NULL,
	FechaTermino				DATETIME,
	TotalPreguntas				INT,
	NumeroPreguntasCorrectas	INT,
	Estado						BIT NOT NULL, -- 1: Examen Abierto: Puede seguir resolviendo | Examen Cerrado: Ya no puede continuar
);

IF OBJECT_ID('ExamenRealizadoDetalle') IS NOT NULL
	DROP TABLE ExamenRealizadoDetalle
GO
CREATE TABLE ExamenRealizadoDetalle
(
	IdExamenRealizadoDetalle	INT	NOT NULL PRIMARY KEY IDENTITY(1, 1),
	IdExamenRealizado			INT NOT NULL REFERENCES ExamenRealizado(IdExamenRealizado),
	IdPregunta					INT NOT NULL REFERENCES Pregunta(IdPregunta),
	IdAlternativa				INT NOT NULL REFERENCES Alternativa(IdAlternativa)
);

/* =========================================================================================== */


/* ============================== PROCEDIMIENTOS ALMACENADOS ================================= */
IF OBJECT_ID('Usp_ObtenerUsuarios') IS NOT NULL
	DROP PROCEDURE Usp_ObtenerUsuarios
GO
CREATE PROCEDURE Usp_ObtenerUsuarios
AS
BEGIN
	SELECT U.*,
			R.IdRol,
			R.NombreRol
	FROM Usuario U
	INNER JOIN UsuarioRol UR
		ON UR.IdUsuario = U.IdUsuario
	INNER JOIN Rol R
		ON R.IdRol = UR.IdRol
END
GO
-- EXEC Usp_ObtenerUsuarios


IF OBJECT_ID('Usp_ObtenerUsuarioPorCuenta') IS NOT NULL
	DROP PROCEDURE Usp_ObtenerUsuarioPorCuenta
GO
CREATE PROCEDURE Usp_ObtenerUsuarioPorCuenta --'axel'
@Usuario VARCHAR(50)
AS
BEGIN
	SELECT U.*,
			R.IdRol,
			R.NombreRol
	FROM Usuario U
	INNER JOIN UsuarioRol UR
		ON UR.IdUsuario = U.IdUsuario
	INNER JOIN Rol R
		ON R.IdRol = UR.IdRol
	WHERE U.Usuario = @Usuario
END
GO
-- EXEC Usp_ObtenerUsuarioPorCuenta 'axel'


IF OBJECT_ID('Usp_Login') IS NOT NULL
	DROP PROCEDURE Usp_Login
GO
CREATE PROCEDURE Usp_Login --'axel','123'
@Usuario VARCHAR(50),
@Contraseña VARCHAR(50)
AS
BEGIN
	IF EXISTS(SELECT * FROM Usuario U WHERE U.Usuario = @Usuario)
	BEGIN
		-- Contraseña Correcta
		IF EXISTS(SELECT * FROM Usuario U WHERE LOWER(U.Usuario) = LOWER(@Usuario) AND U.Contraseña = @Contraseña AND U.Bloqueado = 0)
		BEGIN
			SELECT 
				U.IdUsuario,
				U.Nombres,
				U.ApellidoPaterno,
				U.ApellidoMaterno,
				U.Correo,
				U.Dni,
				U.Usuario,
				U.Contraseña,
				U.imagen,
				U.FechaRegistro,
				U.Intentos,
				U.Bloqueado,
				TU.IdTipoUsuario,
				TU.Descripcion,
				R.IdRol,
				R.NombreRol
			FROM Usuario U
			INNER JOIN UsuarioRol UR
				ON UR.IdUsuario = U.IdUsuario
			INNER JOIN Rol R
				ON R.IdRol = UR.IdRol
			INNER JOIN TipoUsuario TU
				ON TU.IdTipoUsuario = U.IdTipoUsuario
			WHERE U.Usuario = @Usuario AND
				U.Contraseña = @Contraseña
		END
		ELSE
		BEGIN
			-- Contraseña incorrecta
			DECLARE @Intentos INT
			SELECT @Intentos = Intentos FROM Usuario U WHERE U.Usuario = @Usuario
			IF @Intentos > 0
			BEGIN
				UPDATE Usuario SET Intentos = Intentos - 1 WHERE Usuario = @Usuario
				SELECT @Intentos = Intentos FROM Usuario U WHERE U.Usuario = @Usuario
				IF @Intentos = 0
				BEGIN
					-- Bloquea la cuenta de Usuario
					UPDATE Usuario SET Bloqueado = 1 WHERE Usuario = @Usuario
				END
			END
		END
	END
END
GO

-- Usp_Login 'luis', '123'


IF OBJECT_ID('Usp_DesbloquearUsuario') IS NOT NULL
	DROP PROCEDURE Usp_DesbloquearUsuario
GO
CREATE PROCEDURE Usp_DesbloquearUsuario
@Usuario VARCHAR(50)
AS
BEGIN
	UPDATE Usuario
	SET Bloqueado = 0,
		Intentos = 3
	WHERE Usuario = @Usuario
END
GO
-- EXEC Usp_ObtenerUsuarioPorCuenta 'axel'
-- EXEC Usp_ObtenerUsuarioPorCuenta 'axel'
-- EXEC Usp_Login 'axel', '123'
-- EXEC Usp_Login 'axel', '1234'
-- EXEC Usp_Login 'axels', '1234'
-- EXEC Usp_DesbloquearUsuario 'axel'



IF OBJECT_ID('Usp_ListarEnlacesPorCategoria') IS NOT NULL
	DROP PROCEDURE Usp_ListarEnlacesPorCategoria
GO
CREATE PROCEDURE Usp_ListarEnlacesPorCategoria
@IdCategoria INT
AS
BEGIN
	SELECT E.* 
	FROM EnlaceCategoria EC 
	INNER JOIN Enlace E ON EC.IdEnlace = E.IdEnlace
	WHERE IdCategoria = @IdCategoria
END
GO
-- EXEC Usp_ListarEnlacesPorCategoria 1


IF OBJECT_ID('Usp_ListarCategoriasPorRol') IS NOT NULL
	DROP PROCEDURE Usp_ListarCategoriasPorRol
GO
CREATE PROCEDURE Usp_ListarCategoriasPorRol
@IdRol INT
AS
BEGIN
	SELECT C.IdCategoria,
			C.NombreCategoria,
			C.IconoCategoria,
			C.UrlCategoria,
			R.IdRol,
			R.NombreRol
	FROM Categoria C 
	INNER JOIN RolCategoria RC ON RC.IdCategoria = C.IdCategoria
	INNER JOIN Rol R ON R.IdRol = RC.IdRol
	WHERE R.IdRol = @IdRol
END
GO
-- EXEC Usp_ListarCategoriasPorRol 1


IF OBJECT_ID('Usp_InsertarExamen') IS NOT NULL
	DROP PROCEDURE Usp_InsertarExamen
GO
CREATE PROCEDURE Usp_InsertarExamen
@FechaRegistro DATETIME,
@FechaExpiracion DATETIME,
@IdUsuario INT,
@Titulo VARCHAR(50),
@Descripcion VARCHAR(MAX),
@TiempoMaximo INT,
@IdAsignatura INT,
@Clave VARCHAR(50)
AS
BEGIN
	INSERT INTO Examen(FechaRegistro, FechaExpiracion, IdUsuario, Titulo, Descripcion, TiempoMaximo, IdAsignatura, IdTipoExamen, EscalaCalificacion, Clave)
	VALUES(@FechaRegistro, @FechaExpiracion, @IdUsuario, @Titulo, @Descripcion, @TiempoMaximo, @IdAsignatura, 1, 20, @Clave)
	SELECT SCOPE_IDENTITY() -- Obtiene el id generado
END
GO


IF OBJECT_ID('Usp_InsertarPregunta') IS NOT NULL
	DROP PROCEDURE Usp_InsertarPregunta
GO
CREATE PROCEDURE Usp_InsertarPregunta
@Pregunta NVARCHAR(MAX),
@Numero INT,
@IdExamen INT
AS
BEGIN
	INSERT INTO Pregunta(Pregunta, Numero, IdExamen)
	VALUES(@Pregunta, @Numero, @IdExamen)
	SELECT SCOPE_IDENTITY() -- Obtiene el id generado
END
GO


IF OBJECT_ID('Usp_InsertarAlternativa') IS NOT NULL
	DROP PROCEDURE Usp_InsertarAlternativa
GO
CREATE PROCEDURE Usp_InsertarAlternativa
@IdPregunta INT,
@Numero INT,
@Descripcion VARCHAR(100),
@OpcionCorrecta BIT
AS
BEGIN
	INSERT INTO Alternativa(IdPregunta, Numero, Descripcion, OpcionCorrecta)
	VALUES(@IdPregunta, @Numero, @Descripcion, @OpcionCorrecta)
END
GO


IF OBJECT_ID('Usp_InsertarImagen') IS NOT NULL
	DROP PROCEDURE Usp_InsertarImagen
GO
CREATE PROCEDURE Usp_InsertarImagen
@Imagen IMAGE,
@IdPregunta INT
AS
BEGIN
	INSERT INTO Imagen 
	VALUES(@Imagen, @IdPregunta)
END
GO


IF OBJECT_ID('Usp_InsertarVideo') IS NOT NULL
	DROP PROCEDURE Usp_InsertarVideo
GO
CREATE PROCEDURE Usp_InsertarVideo
@Video VARBINARY(max),
@IdPregunta INT
AS
BEGIN
	insert into Video(Video, IdPregunta)
	values(@Video, @IdPregunta)
END
Go


/* =========================================================================================== */


IF OBJECT_ID('FUN_VALIDA_EXAMEN') IS NOT NULL
	DROP FUNCTION FUN_VALIDA_EXAMEN
GO
CREATE FUNCTION FUN_VALIDA_EXAMEN(@IdUsuario INT, @IdExamen INT) returns int
AS
	begin
		declare @Valida int

		select top 1 @Valida = IdExamenRealizado from ExamenRealizado 
		where IdExamen = @IdExamen and IdUsuario =  @IdUsuario and Estado = 1
		order by IdExamenRealizado desc

		if(@Valida is null or @Valida = '')
		begin
			set @Valida = 0
		end
		else
		begin
			set @Valida = 1
		end

		return @Valida
	end
GO

IF OBJECT_ID('FUN_GET_MINUTOS_EXAMEN') IS NOT NULL
	DROP FUNCTION FUN_GET_MINUTOS_EXAMEN
GO
CREATE FUNCTION FUN_GET_MINUTOS_EXAMEN(@IdUsuario INT,@IdExamen INT) returns int
AS
	begin
		declare @CalHora int
		declare @TiemExm int
		declare @resultado int

		select top 1 @CalHora = DATEDIFF(MINUTE,FechaRealizacion, GETDATE()) from ExamenRealizado
		where IdUsuario = @IdUsuario and IdExamen = @IdExamen 
		order by IdExamenRealizado desc

		select @TiemExm = TiempoMaximo from Examen
		where IdExamen = @IdExamen

		set @resultado = @TiemExm - @CalHora

		if(@resultado <= 0)
		begin
			set @resultado = 0
		end

		return @resultado
	end
GO

-- Retorna el tiempo restante en segundos que queda para resolver el examen
IF OBJECT_ID('Fun_Obtener_Segundos_Examen') IS NOT NULL
	DROP FUNCTION Fun_Obtener_Segundos_Examen
GO
CREATE FUNCTION Fun_Obtener_Segundos_Examen(@IdExamen INT, @IdExamenRealizado INT) RETURNS INT
AS
BEGIN
	DECLARE @duracionExamen INT -- Duración del examen en segundos
	DECLARE @segundosTranscurridosHastaHoy INT -- Tiempo en segundos que ha transcurrido desde que se inició el examen
	DECLARE @tiempoRestante INT -- Tiempo restante para resolver el examen

	-- Duración del Examen en segundos
	SELECT @duracionExamen = (E.TiempoMaximo * 60)
	FROM Examen E
	WHERE E.IdExamen = @IdExamen

	-- Tiempo transcurrido desde que se empezó a resolver el examen hasta hoy en segundos
	SELECT @segundosTranscurridosHastaHoy = DATEDIFF(SECOND, FechaRealizacion, GETDATE())
		FROM ExamenRealizado ER
		WHERE ER.IdExamenRealizado = @IdExamenRealizado
	
	-- Tiempo que queda para resolver el examen
	SET @tiempoRestante = @duracionExamen - @segundosTranscurridosHastaHoy
	IF (@tiempoRestante < 0)
	BEGIN
		SET @tiempoRestante = 0
	END

	RETURN @tiempoRestante
END
GO

IF OBJECT_ID('Fun_Obtener_FechaExp_Examen') IS NOT NULL
	DROP FUNCTION Fun_Obtener_FechaExp_Examen
GO
CREATE FUNCTION Fun_Obtener_FechaExp_Examen(@IdExamen INT) RETURNS INT
AS
BEGIN
	DECLARE @data INT, @FechaTermino datetime

	-- Duración del Examen en segundos
	SELECT @FechaTermino = e.FechaExpiracion FROM Examen E WHERE E.IdExamen = @IdExamen

	-- Tiempo transcurrido desde que se empezó a resolver el examen hasta hoy en segundos
	if(@FechaTermino > GETDATE())
	begin
		set @data = 1
	end
	else
	begin
		set @data = 0
	end
	
	RETURN @data
END
GO


-- Retorna el tiempo restante en segundos que queda para un ExamenRealizado
IF OBJECT_ID('Fun_ObtenerSegundosExamenRealizado') IS NOT NULL
	DROP FUNCTION Fun_ObtenerSegundosExamenRealizado
GO
CREATE FUNCTION Fun_ObtenerSegundosExamenRealizado(@IdExamenRealizado INT) RETURNS INT
AS
BEGIN
	DECLARE @IdExamen INT
	DECLARE @duracionExamen INT -- Duración del examen en segundos
	DECLARE @segundosTranscurridosHastaHoy INT -- Tiempo en segundos que ha transcurrido desde que se inició el examen
	DECLARE @tiempoRestante INT -- Tiempo restante para resolver el examen

	-- Obtiene el IdExamen del Examen perteneciente al ExamenRealizado
	SELECT @IdExamen = IdExamen
	FROM ExamenRealizado ER
	WHERE ER.IdExamenRealizado = @IdExamenRealizado

	-- Duración del Examen en segundos
	SELECT @duracionExamen = (E.TiempoMaximo * 60)
	FROM Examen E
	WHERE E.IdExamen = @IdExamen

	-- Tiempo transcurrido desde que se empezó a resolver el examen hasta hoy en segundos
	SELECT @segundosTranscurridosHastaHoy = DATEDIFF(SECOND, FechaRealizacion, GETDATE())
		FROM ExamenRealizado ER
		WHERE ER.IdExamenRealizado = @IdExamenRealizado
	
	-- Tiempo que queda para resolver el examen
	SET @tiempoRestante = @duracionExamen - @segundosTranscurridosHastaHoy
	IF (@tiempoRestante < 0)
	BEGIN
		SET @tiempoRestante = 0
	END

	RETURN @tiempoRestante
END
GO

-- Obtiene el estado del Examen que se está resolviendo o intentando resolver
IF OBJECT_ID('Fun_VerificarEstadoExamen') IS NOT NULL
	DROP FUNCTION Fun_VerificarEstadoExamen
GO
CREATE FUNCTION Fun_VerificarEstadoExamen(@IdExamenRealizado INT) RETURNS INT
AS
BEGIN
	DECLARE @v_EstadoExamen INT

	SELECT @v_EstadoExamen = ER.Estado 
	FROM ExamenRealizado ER
	WHERE ER.IdExamenRealizado = @IdExamenRealizado

	RETURN @v_EstadoExamen
END
GO

-- Obtiene el total de preguntas del Examen
IF OBJECT_ID('Fun_TotalPreguntasPorExamen') IS NOT NULL
	DROP FUNCTION Fun_TotalPreguntasPorExamen
GO
CREATE FUNCTION Fun_TotalPreguntasPorExamen(@IdExamen INT) RETURNS INT
AS
BEGIN
	DECLARE @v_numeroPreguntas INT

	SELECT @v_numeroPreguntas = COUNT(*)
	FROM Pregunta P
	WHERE P.IdExamen = @IdExamen

	RETURN @v_numeroPreguntas
END
GO
-- SELECT dbo.Fun_TotalPreguntasPorExamen(6)


-- Obtiene el total de preguntas del ExamenRealizado
IF OBJECT_ID('Fun_TotalPreguntasPorExamenRealizado') IS NOT NULL
	DROP FUNCTION Fun_TotalPreguntasPorExamenRealizado
GO
CREATE FUNCTION Fun_TotalPreguntasPorExamenRealizado(@IdExamenRealizado INT) RETURNS INT
AS
BEGIN
	DECLARE @IdExamen INT
	DECLARE @v_numeroPreguntas INT

	-- Obtiene el IdExamen del Examen perteneciente al ExamenRealizado
	SELECT @IdExamen = IdExamen
	FROM ExamenRealizado ER
	WHERE ER.IdExamenRealizado = @IdExamenRealizado
	
	SET @v_numeroPreguntas = dbo.Fun_TotalPreguntasPorExamen(@IdExamen)

	RETURN @v_numeroPreguntas
END
GO
-- SELECT dbo.Fun_TotalPreguntasPorExamenRealizado(10)

/* =========================================================================================== */
/*IF OBJECT_ID('Usp_Obtener_Examen_Usuario') IS NOT NULL
	DROP PROCEDURE Usp_Obtener_Examen_Usuario
GO
CREATE PROC Usp_Obtener_Examen_Usuario --1,1
@IdUsuario INT,
@IdExamen INT
AS
	SELECT e.IdExamen,
		   e.FechaRegistro,
		   e.FechaExpiracion,
		   e.Titulo,
		   e.Descripcion,
		   e.TiempoMaximo, -- Duración del examen en minutos
		   dbo.FUN_GET_MINUTOS_EXAMEN(@IdUsuario, @IdExamen) as TiempoMaximoActual,
		   -- dbo.Fun_Obtener_Segundos_Examen(@IdUsuario, @IdExamen) as TiempoMaximoActual,
		   dbo.FUN_VALIDA_EXAMEN(@IdUsuario, @IdExamen) as ValidaExamenResolver,
		   u.IdUsuario,
		   u.Nombres + ' ' + ApellidoPaterno + ' ' + ApellidoMaterno as NombreCompleto,
		   u.Correo
	FROM Examen e inner join Usuario u on e.IdUsuario = u.IdUsuario
	WHERE e.IdExamen = @IdExamen
GO
-- Usp_Obtener_Examen_Usuario 3,1*/

-- Obtiene la información del Examen que va a resolver (valida que le pertenezca la usuario)
IF OBJECT_ID('Usp_ObtenerExamenResolver') IS NOT NULL
	DROP PROCEDURE Usp_ObtenerExamenResolver
GO
CREATE PROC Usp_ObtenerExamenResolver
@IdExamen INT,
@IdExamenRealizado INT
AS
	SELECT E.IdExamen,
		   E.FechaRegistro,
		   E.FechaExpiracion,
		   E.Titulo,
		   E.Descripcion,
		   E.TiempoMaximo, -- Duración del examen en minutos
		   dbo.Fun_Obtener_Segundos_Examen(@IdExamen, @IdExamenRealizado) as TiempoRestante, -- Segundos restantes para terminar el examen
		   dbo.Fun_VerificarEstadoExamen(@IdExamenRealizado) as EstadoExamen, -- Obtiene el estado del examen: 0-Cerrado | 1-Abierto
		   dbo.Fun_Obtener_FechaExp_Examen(@IdExamen) as ValidaFechaExpiracion,
		   U.IdUsuario,
		   U.Nombres + ' ' + U.ApellidoPaterno + ' ' + U.ApellidoMaterno as NombreCompleto,
		   U.Correo,
		   U.imagen
	FROM Examen E INNER JOIN Usuario U on E.IdUsuario = U.IdUsuario
	WHERE E.IdExamen = @IdExamen
GO


select * from ExamenRealizado

IF OBJECT_ID('Usp_ListarPreguntasPorExamen') IS NOT NULL
	DROP PROCEDURE Usp_ListarPreguntasPorExamen
GO
CREATE PROC Usp_ListarPreguntasPorExamen
@IdExamen INT
AS
	SELECT * FROM Pregunta
	WHERE IdExamen = @IdExamen
	ORDER BY Numero
GO

IF OBJECT_ID('Usp_ListarAlternativasPorPregunta') IS NOT NULL
	DROP PROCEDURE Usp_ListarAlternativasPorPregunta
GO
CREATE PROC Usp_ListarAlternativasPorPregunta --2
@IdPregunta INT
AS

	DECLARE @cuenta INT
	SELECT @cuenta = COUNT(*) FROM Alternativa WHERE IdPregunta = @IdPregunta AND OpcionCorrecta = 1
	
	SELECT IdAlternativa, IdPregunta, Numero, Descripcion, OpcionCorrecta, @cuenta as CantAltCorrectas FROM Alternativa
	WHERE IdPregunta = @IdPregunta
	ORDER BY Numero 

GO
-- EXEC Usp_ListarAlternativasPorPregunta 10

IF OBJECT_ID('Usp_ObtenerExamen') IS NOT NULL
	DROP PROCEDURE Usp_ObtenerExamen
GO
CREATE PROC Usp_ObtenerExamen
@IdExamen INT
AS
	SELECT e.IdExamen,
		   e.FechaRegistro,
		   e.FechaExpiracion,
		   e.Titulo,e.Descripcion,
		   e.TiempoMaximo,
		   e.EscalaCalificacion,
		   e.Clave,
		   u.IdUsuario,
		   u.Nombres + ' ' + ApellidoPaterno + ' ' + ApellidoMaterno as NombreCompleto,
		   u.Correo,
		   A.IdAsignatura,
		   A.Nombre
	FROM Examen e 
	INNER JOIN Usuario u on e.IdUsuario = u.IdUsuario
	INNER JOIN Asignatura A on A.IdAsignatura = e.IdAsignatura
				
	WHERE e.IdExamen = @IdExamen
GO

-- Muestra los exámenes registrados
IF OBJECT_ID('Usp_ObtenerExamenes') IS NOT NULL
	DROP PROCEDURE Usp_ObtenerExamenes
GO

CREATE PROC Usp_ObtenerExamenes
AS
	SELECT E.IdExamen,
		   E.FechaRegistro,
		   E.FechaExpiracion,
		   E.Titulo,
		   E.Descripcion,
		   E.TiempoMaximo,
		   U.IdUsuario,
		   U.Nombres,
		   U.ApellidoPaterno,
		   U.ApellidoMaterno,
		   U.imagen,
		   (SELECT COUNT(*) FROM Pregunta P WHERE P.IdExamen = E.IdExamen) AS NroPreguntas
	FROM Examen E inner join Usuario U on E.IdUsuario = U.IdUsuario
	where FechaExpiracion > GETDATE()
GO
-- EXEC Usp_ObtenerExamenes

-- Obtiene aquellos exámenes sin clave
IF OBJECT_ID('Usp_ObtenerExamenesPublicos') IS NOT NULL
	DROP PROCEDURE Usp_ObtenerExamenesPublicos 
GO
CREATE PROC Usp_ObtenerExamenesPublicos
AS
	SELECT E.IdExamen,
		   E.FechaRegistro,
		   E.FechaExpiracion,
		   E.Titulo,
		   E.Descripcion,
		   E.TiempoMaximo,
		   U.IdUsuario,
		   U.Nombres,
		   U.ApellidoPaterno,
		   U.ApellidoMaterno,
		   U.imagen,
		   (SELECT COUNT(*) FROM Pregunta P WHERE P.IdExamen = E.IdExamen) AS NroPreguntas
	FROM Examen E inner join Usuario U on E.IdUsuario = U.IdUsuario
	where FechaExpiracion > GETDATE() AND E.Clave = ''
GO
-- EXEC Usp_ObtenerExamenesPublicos




IF OBJECT_ID('Usp_ListarPreguntasCorrectas') IS NOT NULL
	DROP PROCEDURE Usp_ListarPreguntasCorrectas 
GO
CREATE PROC Usp_ListarPreguntasCorrectas --1
@IdExamen INT
AS
	select distinct e.IdExamen,
	   p.Numero,
       p.Pregunta, 
	   STUFF((SELECT CAST(',' as nvarchar(max)) + Descripcion FROM Alternativa where IdPregunta = p.IdPregunta and OpcionCorrecta = 1 for xml path('')),1,1,'') as Descripcion 
	from Examen e inner join Pregunta p on e.IdExamen = p.IdExamen
	inner join Alternativa a on p.IdPregunta = a.IdPregunta
	where e.IdExamen = @IdExamen and a.OpcionCorrecta = 1
GO	

IF OBJECT_ID('Usp_RegistrarExamenRealizado') IS NOT NULL
	DROP PROCEDURE Usp_RegistrarExamenRealizado
GO
CREATE PROC Usp_RegistrarExamenRealizado
@IdUsuario INT,
@IdExamen INT
AS
	INSERT INTO ExamenRealizado(IdUsuario, IdExamen, FechaRealizacion, Estado) 
	VALUES(@IdUsuario, @IdExamen, GETDATE(), 1)

	SELECT SCOPE_IDENTITY()
GO

IF OBJECT_ID('Usp_ActualizarExamenRealizado') IS NOT NULL
	DROP PROCEDURE Usp_ActualizarExamenRealizado
GO
CREATE PROC Usp_ActualizarExamenRealizado
@IdUsuario INT,
@IdExamen INT,
@TotalPreguntas INT,
@NumeroPreguntasCorrectas INT
AS
	
	DECLARE @ID_EXM_RLZ int
	select top 1 @ID_EXM_RLZ = IdExamenRealizado from ExamenRealizado
	where IdUsuario = @IdUsuario and IdExamen = @IdExamen 
	order by IdExamenRealizado desc

	UPDATE ExamenRealizado
	set FechaTermino = GETDATE(),
		TotalPreguntas = @TotalPreguntas,
		NumeroPreguntasCorrectas = @NumeroPreguntasCorrectas,
		Estado = 0
	where IdExamenRealizado = @ID_EXM_RLZ
	SELECT @ID_EXM_RLZ
GO

IF OBJECT_ID('Usp_InsertarExamenRealizadoDetalle') IS NOT NULL
	DROP PROCEDURE Usp_InsertarExamenRealizadoDetalle
GO
CREATE PROC Usp_InsertarExamenRealizadoDetalle
@IdExamenRealizado INT,
@IdPregunta INT,
@IdAlternativa INT
AS
	insert into ExamenRealizadoDetalle(IdExamenRealizado, IdPregunta, IdAlternativa) 
								values(@IdExamenRealizado,@IdPregunta,@IdAlternativa)
GO



IF OBJECT_ID('Usp_Lista_Alternativas_Correctas_Por_Pregunta') IS NOT NULL
	DROP PROCEDURE Usp_Lista_Alternativas_Correctas_Por_Pregunta
GO
CREATE PROCEDURE Usp_Lista_Alternativas_Correctas_Por_Pregunta
@IdPregunta INT
AS	
	SELECT IdAlternativa, IdPregunta, Numero, Descripcion, OpcionCorrecta 
	FROM Alternativa A
	WHERE A.IdPregunta = @IdPregunta
	And A.OpcionCorrecta = 1
	ORDER BY 1

GO
--EXEC Usp_Lista_Alternativas_Correctas_Por_Pregunta 6


-- Obtiene el listado de Exámenes pendientes (que faltan terminar)
IF OBJECT_ID('Usp_ObtenerExamenesPendientesPorUsuario') IS NOT NULL
	DROP PROCEDURE Usp_ObtenerExamenesPendientesPorUsuario
GO
CREATE PROCEDURE Usp_ObtenerExamenesPendientesPorUsuario
@IdUsuario INT
AS
BEGIN
	-- SELECT * FROM ExamenRealizado WHERE IdUsuario = @IdUsuario AND Estado = 1
	SELECT	ER.IdExamenRealizado,
			E.IdExamen,
			E.FechaRegistro,
			E.FechaExpiracion,
			E.Titulo,
			E.Descripcion,
			E.TiempoMaximo, -- Duración del examen en minutos
			dbo.Fun_Obtener_Segundos_Examen(E.IdExamen, ER.IdExamenRealizado) as TiempoRestante, -- Segundos restantes para terminar el examen
			dbo.Fun_VerificarEstadoExamen(ER.IdExamenRealizado) as EstadoExamen, -- Obtiene el estado del examen: 0-Cerrado | 1-Abierto
			ER.FechaRealizacion,
			U.IdUsuario,
			U.Nombres + ' ' + U.ApellidoPaterno + ' ' + U.ApellidoMaterno as NombreCompleto,
			U.Correo
	FROM ExamenRealizado ER
		INNER JOIN Examen E ON ER.IdExamen = E.IdExamen
		INNER JOIN Usuario U on E.IdUsuario = U.IdUsuario
	WHERE ER.IdUsuario = @IdUsuario AND ER.Estado = 1
END
GO
--EXEC Usp_ObtenerExamenesPendientesPorUsuario 1

-- Muestra los exámenes creados por usuario
IF OBJECT_ID('Usp_ObtenerExamenesCreadosPorUsuario') IS NOT NULL
	DROP PROCEDURE Usp_ObtenerExamenesCreadosPorUsuario
GO
CREATE PROC Usp_ObtenerExamenesCreadosPorUsuario
@IdUsuario INT
AS
	SELECT E.IdExamen,
		   E.FechaRegistro,
		   E.FechaExpiracion,
		   E.Titulo,
		   E.Descripcion,
		   E.TiempoMaximo,
		   E.Clave,
		   E.EscalaCalificacion,
		   U.IdUsuario,
		   U.Nombres,
		   U.ApellidoPaterno,
		   U.ApellidoMaterno,
		   U.imagen,
		   (SELECT COUNT(*) FROM Pregunta P WHERE P.IdExamen = E.IdExamen) AS NroPreguntas
	FROM Examen E inner join Usuario U on E.IdUsuario = U.IdUsuario
	where E.IdUsuario = @IdUsuario
GO
-- EXEC Usp_ObtenerExamenesCreadosPorUsuario 3

IF OBJECT_ID('Usp_ObtenerExamenesRealizadosPorUsuario') IS NOT NULL
	DROP PROCEDURE Usp_ObtenerExamenesRealizadosPorUsuario
GO
CREATE PROCEDURE Usp_ObtenerExamenesRealizadosPorUsuario
@IdUsuario INT
AS
BEGIN
	SELECT * FROM ExamenRealizado WHERE IdUsuario = @IdUsuario AND Estado = 0
END
GO
--EXEC Usp_ObtenerExamenesRealizadosPorUsuario 3


IF OBJECT_ID('Usp_ObtenerExamenRealizadoDetallePorExamen') IS NOT NULL
	DROP PROCEDURE Usp_ObtenerExamenRealizadoDetallePorExamen
GO
CREATE PROCEDURE Usp_ObtenerExamenRealizadoDetallePorExamen
@IdExamenRealizado INT
AS
BEGIN
	SELECT * FROM ExamenRealizadoDetalle ERD WHERE ERD.IdExamenRealizado = @IdExamenRealizado
END
GO
-- EXEC Usp_ObtenerExamenRealizadoDetallePorExamen 1


IF OBJECT_ID('Usp_ObtenerExamenRealizado') IS NOT NULL
	DROP PROCEDURE Usp_ObtenerExamenRealizado
GO
CREATE PROC Usp_ObtenerExamenRealizado 
@IdExamenRealizado INT
AS
	SELECT	ER.IdExamenRealizado,
			ER.IdExamen,
			ER.IdUsuario,
			ER.IdExamen,
			ER.FechaRealizacion,
			ER.FechaTermino,
			ER.TotalPreguntas,
			ER.NumeroPreguntasCorrectas,
			ER.Estado
	FROM ExamenRealizado ER
	WHERE ER.IdExamenRealizado = @IdExamenRealizado
GO
-- EXEC Usp_ObtenerExamenRealizado 1


-- Procedimiento almacenado que permite cerrar todos los Examenes Expirados que no fueron cerrados apropiadamente
IF OBJECT_ID('Usp_CerrarExamenesExpiradosPorUsuario') IS NOT NULL
	DROP PROCEDURE Usp_CerrarExamenesExpiradosPorUsuario
GO
CREATE PROC Usp_CerrarExamenesExpiradosPorUsuario
@IdUsuario INT
AS
	UPDATE ExamenRealizado
	SET Estado = 0,
		FechaTermino = GETDATE(),
		NumeroPreguntasCorrectas = 0,
		TotalPreguntas = dbo.Fun_TotalPreguntasPorExamenRealizado(IdExamenRealizado)
	WHERE IdUsuario = @IdUsuario AND
		dbo.Fun_ObtenerSegundosExamenRealizado(IdExamenRealizado) = 0
GO

-- EXEC Usp_CerrarExamenesExpiradosPorUsuario 1

IF OBJECT_ID('USP_MANTENIMIENTO_USUARIO') IS NOT NULL
	DROP PROCEDURE USP_MANTENIMIENTO_USUARIO
GO
CREATE PROCEDURE USP_MANTENIMIENTO_USUARIO
@IdUsuario int,
@Nombres varchar(100),
@ApellidoPaterno varchar(100),
@ApellidoMaterno varchar(100),
@Correo varchar(100),
@Dni varchar(50),
@Usuario varchar(50),
@Contrasena varchar(50),
@imagen text,
@Intentos int,
@Bloqueado bit,
@IdTipoUsuario INT,
@Estado bit,
@Crea varchar(100),
@Modifica varchar(100),
@Elimina varchar(100),
@Evento int
AS

IF(@Evento =1)
  BEGIN
  INSERT INTO Usuario(Nombres,
                      ApellidoPaterno,
					  ApellidoMaterno,
					  Correo,
					  Dni,
					  Usuario,
					  Contraseña,
					  imagen,
					  FechaRegistro,
					  Intentos,
					  Bloqueado,
					  IdTipoUsuario,
					  Estado,
					  Crea,
					  Modifica,
					  Elimina)
        VALUES (UPPER(@Nombres),
                UPPER(@ApellidoPaterno),
		        UPPER(@ApellidoMaterno),
		              @Correo,
		              @Dni,
		              @Usuario,
		              @Contrasena,
					  @imagen,
					  GETDATE(), -- LA fecha de registro del usuario
		              @Intentos,
		              @Bloqueado,
					  @IdTipoUsuario,
		              @Estado,
		              @Crea,
		              @Modifica,
		              @Elimina)

					  declare @idUsu int

					  SELECT @idUsu = SCOPE_IDENTITY()

					  INSERT INTO UsuarioRol VALUES(@idUsu,3)


END

IF(@Evento =2)
  BEGIN
       UPDATE Usuario
       SET 
       Nombres=UPPER(@Nombres),
       ApellidoPaterno=UPPER(@ApellidoPaterno),
       ApellidoMaterno =UPPER(@ApellidoMaterno),
       Correo=@Correo,
       Dni=@Dni,
       Usuario=@Usuario,
       Contraseña=@Contrasena,
       Intentos=@Intentos,
       Bloqueado=@Bloqueado,
       Estado=@Estado,
       Modifica=@Modifica

      WHERE IdUsuario=@IdUsuario
END

--IF(@Evento = 3)--Eliminar
--  BEGIN
--		UPDATE Usuario SET Estado = 0
--		 WHERE IdUsuario = @IdUsuario
--	END

IF(@Evento = 4)--Consultar
  BEGIN
	SELECT    IdUsuario,
		      Nombres,
		      ApellidoPaterno,
			  ApellidoMaterno,
			  Correo,
			  Dni,
			  Usuario,
			  Contraseña ,
			  FechaRegistro,
			  Intentos,
			  Bloqueado,
			  IdTipoUsuario,
			  Estado 
			  FROM Usuario
	END

IF(@Evento = 5)--Consultar por id
  BEGIN
		SELECT IdUsuario,
		       Nombres,
			   ApellidoPaterno,
			   ApellidoMaterno,
			   Correo,
			   Dni,
			   Usuario,
			   Contraseña ,
			   FechaRegistro
			   Intentos,
			   Bloqueado,
			   IdTipoUsuario,
			   Estado 
			  FROM Usuario
		WHERE IdUsuario = @IdUsuario
	END
GO

IF OBJECT_ID('USP_LISTA_ASIGNATURAS') IS NOT NULL
	DROP PROCEDURE USP_LISTA_ASIGNATURAS
GO
CREATE PROC USP_LISTA_ASIGNATURAS
AS
	SELECT * FROM Asignatura
GO

IF OBJECT_ID('USP_REGISTRA_COMPRA') IS NOT NULL
	DROP PROCEDURE USP_REGISTRA_COMPRA
GO
CREATE PROC USP_REGISTRA_COMPRA
@idUsuario int,
@FechaCompra Datetime,
@Total decimal(18,2)
AS
	insert into Compra values(@idUsuario,@FechaCompra,@Total)
	SELECT SCOPE_IDENTITY() -- Obtiene el id generado
GO

IF OBJECT_ID('USP_REGISTRA_DET_COMPRA') IS NOT NULL
	DROP PROCEDURE USP_REGISTRA_DET_COMPRA
GO
CREATE PROC USP_REGISTRA_DET_COMPRA
@idCompra int,
@IdSuscripcion int,
@CantidadMeses int,
@PrecioPorMes decimal(18,2),
--@FechaExpiracion datetime,
@Estado bit
AS
	declare @FechaExpiracion datetime

	select @FechaExpiracion = DATEADD(MONTH,@CantidadMeses,GETDATE())

	insert into DetalleCompra values(@idCompra,@IdSuscripcion,@CantidadMeses,@PrecioPorMes,@FechaExpiracion,@Estado)
GO

IF OBJECT_ID('USP_ACTUALIZA_TIPO_USUARIO') IS NOT NULL
	DROP PROCEDURE USP_ACTUALIZA_TIPO_USUARIO
GO
CREATE PROC USP_ACTUALIZA_TIPO_USUARIO
@idUsuario int,
@IdSuscripcion int
AS
	declare @IdTipoUsuario int

	select @IdTipoUsuario = IdTipoUsuario from Suscripcion where IdSuscripcion = @IdSuscripcion

	update Usuario set IdTipoUsuario = @IdTipoUsuario
	where IdUsuario = @idUsuario

GO

IF OBJECT_ID('USP_LISTA_SUSCRIPCIONES') IS NOT NULL
	DROP PROCEDURE USP_LISTA_SUSCRIPCIONES
GO
CREATE PROC USP_LISTA_SUSCRIPCIONES
AS
	select * from Suscripcion

GO