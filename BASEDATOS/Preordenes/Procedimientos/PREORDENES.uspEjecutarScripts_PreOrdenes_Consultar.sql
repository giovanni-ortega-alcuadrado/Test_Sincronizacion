DECLARE @strSQL nvarchar(1000)
IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'[PREORDENES].[uspEjecutarScripts_PreOrdenes_Consultar]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	BEGIN
		SET @strSQL = 'CREATE PROCEDURE [PREORDENES].[uspEjecutarScripts_PreOrdenes_Consultar] AS Return'
		EXEC sp_executesql @strSQL
	END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--------------------------------------------------------------------------------------------------------------------------------
--
-- Procedimiento		: [PREORDENES].[uspEjecutarScripts_PreOrdenes_Consultar]
-- Desarrollado por		: Natalia Andrea Otalvaro.
-- Descripción			: Procedimiento para consultar las preordenes. 
-- Fecha				: Mayo 2 de 2019
-- Ejemplo Llamado		: 
/*
exec [PREORDENES].[uspEjecutarScripts_PreOrdenes_Consultar] @pdtmFechaInicial='2019-05-03', @pdtmFechaFinal='2019-05-03', @pstrUsuario=''
*/
--						   								
-- Pruebas CB			: Natalia Andrea Otalvaro - Mayo 2 de 2019 - Resultado Ok
--------------------------------------------------------------------------------------------------------------------------------

 ALTER PROCEDURE [PREORDENES].[uspEjecutarScripts_PreOrdenes_Consultar]
	@pdtmFechaInicial		DATETIME,				--- Fecha inicial en la cual se esta realizando los movimientos
	@pdtmFechaFinal			DATETIME,				--- Fecha final en la cual se esta realizando los movimientos
	@pstrIDComitente		VARCHAR(17)	=NULL,		-- Codigo OYD
	@pstrUsuario			VARCHAR(60),			-- Usuario que realizo el cambio a la tabla, para efectos de auditoria.
	@pstrInfoSesion			VARCHAR(1000)	=NULL,	-- XML que enviará la aplicación para extraer valores de parámetros de auditoria de conexión
	@pintErrorPersonalizado TINYINT = 0				-- Cuando hay un error personalizado en el estado del error se envía este número, si es cero se envía el predefinido
AS

SET NOCOUNT ON;
	
DECLARE @strProcedimiento		VARCHAR(100),
		@strOpcion				VARCHAR(255),
		@strProceso				VARCHAR(100),
		@strParametros			VARCHAR(2000),
		@lngIdEncabezadoOrden		INT=0


BEGIN TRY
	
	SELECT  @strOpcion = 'Ejecutar Scripts',
			@strProceso = 'Reportes PREORDENES',
			@strProcedimiento = 'uspEjecutarScripts_PreOrdenes_Consultar'

	SET @strParametros = + ' ,@pdtmFechaInicial ='
		 + ISNULL(CONVERT(VARCHAR, @pdtmFechaInicial), 'NULL')  
		 + ' ,@pdtmFechaFinal ='
		 + ISNULL(CONVERT(VARCHAR, @pdtmFechaFinal), 'NULL')
		 + ' ,@pstrIDComitente ='
		 + ISNULL(CONVERT(VARCHAR, @pstrIDComitente), 'NULL')		  	  
		 + ' ,@pstrUsuario ='
		 + ISNULL(CONVERT(VARCHAR, @pstrUsuario), 'NULL')   
		 + ' ,@pstrInfoSesion ='
		 + ISNULL(CONVERT(VARCHAR, @pstrInfoSesion), 'NULL')   
		 + ' ,@pintErrorPersonalizado ='
		 + ISNULL(CONVERT(VARCHAR, @pintErrorPersonalizado), 'NULL') 


END TRY
BEGIN CATCH
	SET @strParametros = 'No se pudo inicializar la lista de parámetros.'    	
END CATCH

BEGIN TRY	
	CREATE TABLE #tmpDatosRetorno(
		strEstadoActual				VARCHAR(100)
		,intIDPreOrden				int
		,dtmFechaInversion			datetime
		,dtmFechaVigencia			datetime
		,intIDCodigoPersona			int
		,strIDComitente				varchar(17)
		,strNombre					varchar(200)
		,strTipoPreOrden			VARCHAR(2)
		,strTipoInversion			VARCHAR(2)
		,intIDEntidad				int
		,strNombreEntidad			VARCHAR(200)
		,strInstrumento				VARCHAR(25)
		,strIntencion				VARCHAR(2)
		,dblValor					float
		,dblPrecio					float
		,dblRentabilidadMinima		float
		,dblRentabilidadMaxima		float
		,strInstrucciones			VARCHAR(500)
		,logActivo					bit
		,strObservaciones			VARCHAR(500)
		,strUsuario					VARCHAR(60)
		,dtmFechaCreacion			datetime
		,dtmActualizacion			datetime
		,strUsuarioInsercion		VARCHAR(60)
		,logEsParcial				bit
		,intIDPreOrdenOrigen		int
		,dblValorPendiente			float
		,intNroParcial				int
		,strOrigenAnulacion			VARCHAR(50)
	)

	SET @pdtmFechaInicial = CONVERT(DATE,@pdtmFechaInicial)
	SET @pdtmFechaFinal = DATEADD(MINUTE, -1, CONVERT(DATETIME, DATEADD(DAY, 1, CONVERT(DATE,@pdtmFechaFinal))))
	SET @pstrIDComitente=ISNULL(@pstrIDComitente,'')

	IF @pstrIDComitente<>''
		SET @pstrIDComitente=STR(LTRIM(RTRIM(@pstrIDComitente)),17)
	
	--SELECT @pdtmFechaInicial, @pdtmFechaFinal, @pstrIDComitente

	--CONSULTA LAS PREORDENES QUE TODAVIA ESTAN ACTIVAS
	INSERT INTO #tmpDatosRetorno(
		strEstadoActual
		,intIDPreOrden
		,dtmFechaInversion
		,dtmFechaVigencia
		,intIDCodigoPersona
		,strIDComitente
		,strNombre
		,strTipoPreOrden
		,strTipoInversion
		,intIDEntidad
		,strNombreEntidad
		,strInstrumento
		,strIntencion
		,dblValor
		,dblPrecio
		,dblRentabilidadMinima
		,dblRentabilidadMaxima
		,strInstrucciones
		,logActivo
		,strObservaciones
		,strUsuario
		,dtmFechaCreacion
		,dtmActualizacion
		,strUsuarioInsercion
		,logEsParcial
		,intIDPreOrdenOrigen
		,dblValorPendiente
		,intNroParcial
		,strOrigenAnulacion
	)
	SELECT 'PENDIENTECRUZAR' AS strEstadoActual
		,PRE.intID AS intIDPreOrden
		,PRE.dtmFechaInversion
		,PRE.dtmFechaVigencia
		,PRE.intIDCodigoPersona
		,LTRIM(RTRIM(CODIGO.intIDComitente))
		,PERSONA.strNombre
		,PRE.strTipoPreOrden
		,PRE.strTipoInversion
		,PRE.intIDEntidad
		,ENTIDAD.strNombre
		,PRE.strInstrumento
		,PRE.strIntencion
		,PRE.dblValor
		,PRE.dblPrecio
		,PRE.dblRentabilidadMinima
		,PRE.dblRentabilidadMaxima
		,PRE.strInstrucciones
		,PRE.logActivo
		,PRE.strObservaciones
		,PRE.strUsuario
		,PRE.dtmFechaCreacion
		,PRE.dtmActualizacion
		,PRE.strUsuarioInsercion
		,PRE.logEsParcial
		,PRE.intIDPreOrdenOrigen
		,PRE.dblValorPendiente
		,PRE.intNroParcial
		,PRE.strOrigenAnulacion
	FROM PREORDENES.tblPreOrdenes PRE
	INNER JOIN Personas.tblCodigos CODIGO ON CODIGO.intID=PRE.intIDCodigoPersona
	INNER JOIN Personas.tblPersonas PERSONA ON PERSONA.intID=CODIGO.intIDPersona
	LEFT JOIN CF.tblEntidades ENTIDAD ON ENTIDAD.intIDEntidad=PRE.intIDEntidad
	WHERE PRE.dtmFechaInversion BETWEEN @pdtmFechaInicial AND @pdtmFechaFinal
		AND (CODIGO.intIDComitente=@pstrIDComitente OR @pstrIDComitente='')

	--CONSULTA LAS PREORDENES CRUZADAS
	INSERT INTO #tmpDatosRetorno(
		strEstadoActual
		,intIDPreOrden
		,dtmFechaInversion
		,dtmFechaVigencia
		,intIDCodigoPersona
		,strIDComitente
		,strNombre
		,strTipoPreOrden
		,strTipoInversion
		,intIDEntidad
		,strNombreEntidad
		,strInstrumento
		,strIntencion
		,dblValor
		,dblPrecio
		,dblRentabilidadMinima
		,dblRentabilidadMaxima
		,strInstrucciones
		,logActivo
		,strObservaciones
		,strUsuario
		,dtmFechaCreacion
		,dtmActualizacion
		,strUsuarioInsercion
		,logEsParcial
		,intIDPreOrdenOrigen
		,dblValorPendiente
		,intNroParcial
		,strOrigenAnulacion
	)
	SELECT 'CRUZADA' AS strEstadoActual
		,PRE.intIDPreOrden AS intIDPreOrden
		,PRE.dtmFechaInversion
		,PRE.dtmFechaVigencia
		,PRE.intIDCodigoPersona
		,LTRIM(RTRIM(CODIGO.intIDComitente))
		,PERSONA.strNombre
		,PRE.strTipoPreOrden
		,PRE.strTipoInversion
		,PRE.intIDEntidad
		,ENTIDAD.strNombre
		,PRE.strInstrumento
		,PRE.strIntencion
		,PRE.dblValor
		,PRE.dblPrecio
		,PRE.dblRentabilidadMinima
		,PRE.dblRentabilidadMaxima
		,PRE.strInstrucciones
		,PRE.logActivo
		,PRE.strObservaciones
		,PRE.strUsuario
		,PRE.dtmFechaCreacion
		,PRE.dtmActualizacion
		,PRE.strUsuarioInsercion
		,PRE.logEsParcial
		,PRE.intIDPreOrden
		,0 AS dblValorPendiente
		,PRE.intNroParcial
		,'' AS strOrigenAnulacion
	FROM PREORDENES.tblPreOrdenes_Cruzadas PRE
	INNER JOIN Personas.tblCodigos CODIGO ON CODIGO.intID=PRE.intIDCodigoPersona
	INNER JOIN Personas.tblPersonas PERSONA ON PERSONA.intID=CODIGO.intIDPersona
	LEFT JOIN CF.tblEntidades ENTIDAD ON ENTIDAD.intIDEntidad=PRE.intIDEntidad
	WHERE PRE.dtmFechaInversion BETWEEN @pdtmFechaInicial AND @pdtmFechaFinal
		AND (CODIGO.intIDComitente=@pstrIDComitente OR @pstrIDComitente='')

	--CONSULTA LAS PREORDENES ANULADAS
	INSERT INTO #tmpDatosRetorno(
		strEstadoActual
		,intIDPreOrden
		,dtmFechaInversion
		,dtmFechaVigencia
		,intIDCodigoPersona
		,strIDComitente
		,strNombre
		,strTipoPreOrden
		,strTipoInversion
		,intIDEntidad
		,strNombreEntidad
		,strInstrumento
		,strIntencion
		,dblValor
		,dblPrecio
		,dblRentabilidadMinima
		,dblRentabilidadMaxima
		,strInstrucciones
		,logActivo
		,strObservaciones
		,strUsuario
		,dtmFechaCreacion
		,dtmActualizacion
		,strUsuarioInsercion
		,logEsParcial
		,intIDPreOrdenOrigen
		,dblValorPendiente
		,intNroParcial
		,strOrigenAnulacion
	)
	SELECT 'ANULADA' AS strEstadoActual
		,PRE.intID AS intIDPreOrden
		,PRE.dtmFechaInversion
		,PRE.dtmFechaVigencia
		,PRE.intIDCodigoPersona
		,LTRIM(RTRIM(CODIGO.intIDComitente))
		,PERSONA.strNombre
		,PRE.strTipoPreOrden
		,PRE.strTipoInversion
		,PRE.intIDEntidad
		,ENTIDAD.strNombre
		,PRE.strInstrumento
		,PRE.strIntencion
		,PRE.dblValor
		,PRE.dblPrecio
		,PRE.dblRentabilidadMinima
		,PRE.dblRentabilidadMaxima
		,PRE.strInstrucciones
		,PRE.logActivo
		,PRE.strObservaciones
		,PRE.strUsuario
		,PRE.dtmFechaCreacion
		,PRE.dtmActualizacion
		,PRE.strUsuarioInsercion
		,PRE.logEsParcial
		,PRE.intIDPreOrdenOrigen
		,PRE.dblValorPendiente
		,PRE.intNroParcial
		,PRE.strOrigenAnulacion
	FROM PREORDENES.tblPreOrdenes_Anuladas PRE
	INNER JOIN Personas.tblCodigos CODIGO ON CODIGO.intID=PRE.intIDCodigoPersona
	INNER JOIN Personas.tblPersonas PERSONA ON PERSONA.intID=CODIGO.intIDPersona
	LEFT JOIN CF.tblEntidades ENTIDAD ON ENTIDAD.intIDEntidad=PRE.intIDEntidad
	WHERE PRE.dtmFechaInversion BETWEEN @pdtmFechaInicial AND @pdtmFechaFinal
		AND (CODIGO.intIDComitente=@pstrIDComitente OR @pstrIDComitente='')
	
	SELECT intIDPreOrden AS [ID PreOrdén]
		,strEstadoActual AS [ACCION]
		,dtmFechaInversion AS [Fecha inversión]
		,dtmFechaVigencia AS [Fecha vigencia]
		,strIDComitente AS [Código Cliente]
		,strNombre AS [Nombre Cliente]
		,CASE WHEN strTipoPreOrden='C' THEN 'C-Compra' ELSE 'V-Venta' END AS [Tipo PreOrdén]
		,CASE WHEN strTipoInversion='A' THEN 'A-Acciones' ELSE 'C-Renta fija' END AS [Tipo Inversión]
		,CASE WHEN intIDEntidad IS NULL THEN 'Información no registrada en la pre-ordene' ELSE CONVERT(VARCHAR(20), intIDEntidad) + '-' + strNombreEntidad END AS [Entidad]
		,CASE WHEN strInstrumento IS NULL THEN 'Información no registrada en la pre-ordene' ELSE strInstrumento END AS [Instrumento]
		,CASE WHEN strIntencion='E' THEN 'E-Valor economico' ELSE 'N-Valor nominal' END AS [Intención]
		,ISNULL(dblValor,0) AS [Valor]
		,ISNULL(dblValorPendiente,0) AS [Saldo]
		,ISNULL(dblPrecio,0) AS [Precio]
		,ISNULL(dblRentabilidadMinima,0) AS [Rentabilidad mínima]
		,ISNULL(dblRentabilidadMaxima,0) AS [Rentabilidad máxima]
		,CASE WHEN logEsParcial=1 THEN 'SI' ELSE 'NO' END AS [Es parcial]
		,CASE WHEN logEsParcial=1 THEN intNroParcial ELSE 0 END AS [Nro parcial]
		,CASE WHEN strInstrucciones IS NULL THEN 'Información no registrada en la pre-ordene' ELSE strInstrucciones END AS [Instrucciones]
		,logActivo AS [Activo]
		,CASE WHEN logActivo=1 THEN strOrigenAnulacion ELSE '' END AS [Origen anulación]
		,CASE WHEN strObservaciones IS NULL THEN 'Información no registrada en la pre-ordene' ELSE strObservaciones END AS [Observaciones]
		,strUsuarioInsercion AS [Usuario creación]
		,CONVERT(VARCHAR(25), dtmFechaCreacion, 121) AS [Fecha creación]
		,strUsuario AS [Usuario ultima actualización]
		,CONVERT(VARCHAR(25), dtmActualizacion, 121) AS [Fecha ultima actualización]
	FROM #tmpDatosRetorno
	ORDER BY intIDPreOrden
	
END TRY
BEGIN CATCH
    IF @@trancount>0 
        ROLLBACK TRANSACTION
        	
    EXEC dbo.uspA2_Util_CrearLogErroresSQL @@ProcId,'Se presentó un problema al ejecutar Reporte', @strParametros, @pstrUsuario, NULL, @pstrInfosesion, @pintErrorPersonalizado 
END CATCH
