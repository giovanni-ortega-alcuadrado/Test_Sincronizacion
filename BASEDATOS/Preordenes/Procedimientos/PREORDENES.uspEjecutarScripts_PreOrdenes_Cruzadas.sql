DECLARE @strSQL nvarchar(1000)
IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'[PREORDENES].[PREORDENES.uspEjecutarScripts_PreOrdenes_Cruzadas]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	BEGIN
		SET @strSQL = 'CREATE PROCEDURE [PREORDENES].[PREORDENES.uspEjecutarScripts_PreOrdenes_Cruzadas] AS Return'
		EXEC sp_executesql @strSQL
	END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--------------------------------------------------------------------------------------------------------------------------------
--
-- Procedimiento		: [PREORDENES].[PREORDENES.uspEjecutarScripts_PreOrdenes_Cruzadas]
-- Desarrollado por		: Natalia Andrea Otalvaro.
-- Descripción			: Procedimiento para consultar los cruces de las preordenes. 
-- Fecha				: Mayo 2 de 2019
-- Ejemplo Llamado		: 
/*
exec [PREORDENES].[PREORDENES.uspEjecutarScripts_PreOrdenes_Cruzadas] @pdtmFechaInicial='2019-05-03', @pdtmFechaFinal='2019-05-03', @pstrUsuario=''
*/
--						   								
-- Pruebas CB			: Natalia Andrea Otalvaro - Mayo 2 de 2019 - Resultado Ok
--------------------------------------------------------------------------------------------------------------------------------

 ALTER PROCEDURE [PREORDENES].[PREORDENES.uspEjecutarScripts_PreOrdenes_Cruzadas]
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
			@strProceso = 'Reportes PREORDENES Cruzadas',
			@strProcedimiento = 'PREORDENES.uspEjecutarScripts_PreOrdenes_Cruzadas'

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
	SET @pdtmFechaInicial = CONVERT(DATE,@pdtmFechaInicial)
	SET @pdtmFechaFinal = DATEADD(MINUTE, -1, CONVERT(DATETIME, DATEADD(DAY, 1, CONVERT(DATE,@pdtmFechaFinal))))
	SET @pstrIDComitente=ISNULL(@pstrIDComitente,'')

	IF @pstrIDComitente<>''
		SET @pstrIDComitente=STR(LTRIM(RTRIM(@pstrIDComitente)),17)
	
	SELECT CRUCE.intID AS [ID],
		COMPRA.intIDPreOrden AS [ID PreOrden Compra],
		VENTA.intIDPreOrden AS [ID PreOrden Venta],
		CRUCE.strTipoCrucePrincipal AS [Tipo Cruce Principal],
		CONVERT(VARCHAR(25), CRUCE.dtmFechaCruce, 121) AS [Tipo Cruce Principal],
		CRUCE.dblValorCruzado AS [Valor cruce],
		CASE WHEN ORDENCOMPRA.intID IS NULL 
			THEN 'Información no registrada en la pre-ordene' 
			ELSE CASE WHEN ORDENCOMPRA.strOrigenOrden='OYD' THEN 'Orden bolsa'
					  WHEN ORDENCOMPRA.strOrigenOrden='ON' THEN 'Otros Negocios'
					  WHEN ORDENCOMPRA.strOrigenOrden='OYDPLUS' THEN 'Ordenes OYDPLUS'
					  ELSE '' END
				+ ':' + CONVERT(VARCHAR(20), ORDENCOMPRA.intNroRegistro)
				+ '-' + CASE WHEN ORDENCOMPRA.strClaseRegistro='A' THEN 'Acciones' ELSE 'Renta fija' END
				+ '-' + CASE WHEN ORDENCOMPRA.strTipoOperacionRegistro='C' THEN 'Compra' ELSE 'Venta' END
		END AS [Orden compra],
		CASE WHEN ORDENVENTA.intID IS NULL 
			THEN 'Información no registrada en la pre-ordene' 
			ELSE CASE WHEN ORDENVENTA.strOrigenOrden='OYD' THEN 'Orden bolsa'
					  WHEN ORDENVENTA.strOrigenOrden='ON' THEN 'Otros Negocios'
					  WHEN ORDENVENTA.strOrigenOrden='OYDPLUS' THEN 'Ordenes OYDPLUS'
					  ELSE '' END
				+ ':' + CONVERT(VARCHAR(20), ORDENVENTA.intNroRegistro)
				+ '-' + CASE WHEN ORDENVENTA.strClaseRegistro='A' THEN 'Acciones' ELSE 'Renta fija' END
				+ '-' + CASE WHEN ORDENVENTA.strTipoOperacionRegistro='C' THEN 'Compra' ELSE 'Venta' END
		END AS [Orden venta],
		CRUCE.strUsuario AS [Usuario],
		CRUCE.dtmActualizacion AS [Fecha actualización]
	FROM PREORDENES.tblPreOrdenes_Cruces CRUCE
	INNER JOIN PREORDENES.tblPreOrdenes_Cruzadas COMPRA ON COMPRA.intIDPreOrden=CRUCE.intIDPreOrdenCompra
	INNER JOIN Personas.tblCodigos COMPRACODIGO ON COMPRACODIGO.intID=COMPRA.intIDCodigoPersona
	INNER JOIN PREORDENES.tblPreOrdenes_Cruzadas VENTA ON VENTA.intIDPreOrden=CRUCE.intIDPreOrdenVenta
	INNER JOIN Personas.tblCodigos VENTACODIGO ON VENTACODIGO.intID=VENTA.intIDCodigoPersona
	LEFT JOIN PREORDENES.tblPreOrdenes_Cruzadas_Orden ORDENCOMPRA ON ORDENCOMPRA.intID=COMPRA.intID
	LEFT JOIN PREORDENES.tblPreOrdenes_Cruzadas_Orden ORDENVENTA ON ORDENVENTA.intID=COMPRA.intID
	WHERE CRUCE.dtmFechaCruce BETWEEN @pdtmFechaInicial AND @pdtmFechaFinal
		AND (COMPRACODIGO.intIDComitente=@pstrIDComitente
			OR VENTACODIGO.intIDComitente=@pstrIDComitente
			OR @pstrIDComitente='')
	
END TRY
BEGIN CATCH
    IF @@trancount>0 
        ROLLBACK TRANSACTION
        	
    EXEC dbo.uspA2_Util_CrearLogErroresSQL @@ProcId,'Se presentó un problema al ejecutar Reporte', @strParametros, @pstrUsuario, NULL, @pstrInfosesion, @pintErrorPersonalizado 
END CATCH
