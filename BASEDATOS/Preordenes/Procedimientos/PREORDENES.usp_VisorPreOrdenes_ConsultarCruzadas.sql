IF not exists (Select * From sys.sysobjects Where Id = object_id('[PREORDENES].[usp_VisorPreOrdenes_ConsultarCruzadas]', 'P'))
BEGIN
	EXECUTE('Create Procedure [PREORDENES].[usp_VisorPreOrdenes_ConsultarCruzadas] As return(0)')
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*-----------------------------------------------------------------------------------------------------------------

Procedimiento [PREORDENES].[usp_VisorPreOrdenes_ConsultarCruzadas]
 
Descripción: Consulta los registros cruzados realizados por el usuario para ser utilizados en el Visor de PreOrdenes. 
			Aplica para la tabla de PREORDENES.tblPreOrdenes

Desarrollado por: Natalia Andrea Otalvaro
Fecha: Marzo 25/2019

Ejemplo: 
exec [PREORDENES].[usp_VisorPreOrdenes_ConsultarCruzadas] @pdtmFechaInicial='2019-04-04 00:00:00',@pdtmFechaFinal='2019-04-04 00:00:00',@pintIDPreOrden=14,@plogSoloUsuario=1,@pstrUsuario='juan.correa',@pstrInfosesion='<InfoSesion ip="10.10.40.164" usuario="VisorPreOrdenes_ConsultarCruzadas" maquina=" " browser="Unknown - default 0.0" servidor="PC-JCORREA" modulo="juan.correa"></InfoSesion>'
*/

ALTER PROCEDURE [PREORDENES].[usp_VisorPreOrdenes_ConsultarCruzadas]
@pdtmFechaInicial		DATETIME,
@pdtmFechaFinal			DATETIME,
@pintIDPreOrden			INT,
@plogSoloUsuario		BIT,
@pstrUsuario			VARCHAR(60), -- Usuario que ejecuta la acción
@pstrInfosesion			VARCHAR(1000) = Null -- XML con inofrmación que se utiliza para auditoria (usuario, ip máquina usuario, nombre máquina usuario, servidor web, etc.)
--WITH ENCRYPTION
AS 

SET NOCOUNT ON

-- Declaraciones generales requeridas por el manejo de error y auditoria
DECLARE @strParametros VARCHAR(2000),@intNroRegistros INT,@strProcedimiento VARCHAR(100),@intIdAuditoria INT,@strOpcion VARCHAR(255),@strProceso VARCHAR(100),@strObjeto VARCHAR(100), @strMsg VARCHAR(2000)

-----------------------------------------------------------------------------------------------------------------
-- Declaraciones propias del procedimiento

-----------------------------------------------------------------------------------------------------------------

BEGIN TRY
	-- Concatenar todos los parámetros para guardarlos en el log de uso
	SET @strParametros ='@pdtmFechaInicial=' + ISNULL('''' + CONVERT(VARCHAR(20), @pdtmFechaInicial) + '''', 'null') + 
						',@pdtmFechaFinal=' + ISNULL('''' + CONVERT(VARCHAR(20), @pdtmFechaFinal) + '''', 'null') + 
						',@pintIDPreOrden=' + ISNULL('''' + CONVERT(VARCHAR(20), @pintIDPreOrden) + '''', 'null') + 
						',@pstrUsuario=' + ISNULL('''' + @pstrUsuario + '''', 'null') + 
						',@pstrInfosesion=' + ISNULL('''' + @pstrInfosesion + '''', 'null')
	-----------------------------------------------------------------------------------------------------------------
	-- Inicializar auditoria de uso
	SELECT @strOpcion = 'VisorPreOrdenes', @strProceso= 'Consultar', @strProcedimiento = OBJECT_NAME(@@PROCID), @strObjeto = '', @pstrUsuario=ISNULL(@pstrUsuario,'') 	-- Asignar valor a los parámetros para identificar la opción ejecutada
	EXEC @intIdAuditoria = PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pstrOpcion= @strOpcion, @pstrProceso = @strProceso, @pstrObjeto = @strObjeto, @pstrProcedimiento = @strProcedimiento, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrInfosesion = @pstrInfosesion
	
	-----------------------------------------------------------------------------------------------------------------
	-- Funcionalidad propia del procedimiento
	SET @pdtmFechaInicial=CONVERT(DATE, @pdtmFechaInicial)
	SET @pdtmFechaFinal=DATEADD(MINUTE, -1, CONVERT(DATETIME, DATEADD(DAY, 1, CONVERT(DATE, @pdtmFechaFinal))))
	SET @pintIDPreOrden=ISNULL(@pintIDPreOrden,0)

	SELECT CRUCES.intID,
		PRECOMPRA.intIDPreOrden AS intIDPreOrdenCompra,
		PRECOMPRA.strTipoInversion AS strTipoInversionCompra,
		CASE WHEN PRECOMPRA.strTipoInversion='C' THEN 'Renta fija' ELSE 'Acciones' END AS strDescripcionTipoInversionCompra,
		PRECOMPRA.strInstrumento AS strInstrumentoCompra,
		CONVERT(BIT, CASE WHEN ORDENCOMPRA.intID IS NULL THEN 0 ELSE 1 END) AS logTieneAsociacionCompra,
		CASE WHEN ORDENCOMPRA.strOrigenOrden='OYD' THEN 'Orden bolsa'
			 WHEN ORDENCOMPRA.strOrigenOrden='ON' THEN 'Otros Negocios'
			 WHEN ORDENCOMPRA.strOrigenOrden='OYDPLUS' THEN 'Ordenes OYDPLUS'
			 ELSE '' END AS strTipoOrigenOrdenCompra,
		ORDENCOMPRA.intNroRegistro AS intNroRegistroOrdenCompra,
		PREVENTA.intIDPreOrden AS intIDPreOrdenVenta,
		PREVENTA.strTipoInversion AS strTipoInversionVenta,
		CASE WHEN PREVENTA.strTipoInversion='C' THEN 'Renta fija' ELSE 'Acciones' END AS strDescripcionTipoInversionVenta,
		PREVENTA.strInstrumento AS strInstrumentoVenta,
		CONVERT(BIT, CASE WHEN ORDENVENTA.intID IS NULL THEN 0 ELSE 1 END) AS logTieneAsociacionVenta,
		CASE WHEN ORDENVENTA.strOrigenOrden='OYD' THEN 'Orden bolsa'
			 WHEN ORDENVENTA.strOrigenOrden='ON' THEN 'Otros Negocios'
			 WHEN ORDENVENTA.strOrigenOrden='OYDPLUS' THEN 'Ordenes OYDPLUS'
			 ELSE '' END AS strTipoOrigenOrdenVenta,
		ORDENVENTA.intNroRegistro AS intNroRegistroOrdenVenta,
		CRUCES.dtmFechaCruce,
		CRUCES.dblValorCruzado,
		CRUCES.strUsuario
	FROM PREORDENES.tblPreOrdenes_Cruces AS CRUCES
	INNER JOIN PREORDENES.tblPreOrdenes_Cruzadas AS PRECOMPRA ON PRECOMPRA.guidCruce=CRUCES.guidCruce AND PRECOMPRA.intIDPreOrden=CRUCES.intIDPreOrdenCompra
	INNER JOIN PREORDENES.tblPreOrdenes_Cruzadas AS PREVENTA ON PREVENTA.guidCruce=CRUCES.guidCruce AND PREVENTA.intIDPreOrden=CRUCES.intIDPreOrdenVenta
	LEFT JOIN PREORDENES.tblPreOrdenes_Cruzadas_Orden AS ORDENCOMPRA ON ORDENCOMPRA.intIDPreOrden_Cruzada=PRECOMPRA.intID
	LEFT JOIN PREORDENES.tblPreOrdenes_Cruzadas_Orden AS ORDENVENTA ON ORDENVENTA.intIDPreOrden_Cruzada=PREVENTA.intID
	WHERE CRUCES.strUsuario=CASE WHEN @plogSoloUsuario=1 THEN @pstrUsuario ELSE CRUCES.strUsuario END
		AND CRUCES.dtmFechaCruce BETWEEN @pdtmFechaInicial AND @pdtmFechaFinal
		AND (CRUCES.intIDPreOrdenCompra=@pintIDPreOrden OR CRUCES.intIDPreOrdenVenta=@pintIDPreOrden OR @pintIDPreOrden=0)
	ORDER BY CRUCES.dtmFechaCruce DESC

	-----------------------------------------------------------------------------------------------------------------
	
	SET @intNroRegistros = @@ROWCOUNT

	-- Cerrar registro de auditoria
	EXEC PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pintLogAuditoriaUso = @intIdAuditoria, @pintNroRegistros = @intNroRegistros
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	SET @strMsg = PLATAFORMA.ufnA2_Mensajes_obtenerMsgError('PLATAFORMA_ERROR_CONSULTARID', 'PreOrdenes', '', '') -- CCM201711
	EXEC PLATAFORMA.uspA2_Util_CrearLogErroresSQL @pstrProceso = @@ProcId, @pstrMsgPersonalizado = @strMsg, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrMaquina = NULL, @pstrInfosesion = @pstrInfosesion, @plogLanzarError = 1
END CATCH
