IF not exists (Select * From sys.sysobjects Where Id = object_id('[PREORDENES].[usp_VisorPreOrdenes_Consultar]', 'P'))
BEGIN
	EXECUTE('Create Procedure [PREORDENES].[usp_VisorPreOrdenes_Consultar] As return(0)')
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*-----------------------------------------------------------------------------------------------------------------

Procedimiento [PREORDENES].[usp_VisorPreOrdenes_Consultar]
 
Descripción: Consulta los registros disponibles para ser utilizados en el Visor de PreOrdenes. 
			Aplica para la tabla de PREORDENES.tblPreOrdenes

Desarrollado por: Natalia Andrea Otalvaro
Fecha: Marzo 25/2019

Ejemplo: 
EXEC [PREORDENES].[usp_VisorPreOrdenes_Consultar] @pstrUsuario = 'natalia.otalvaro', @pstrInfosesion = ''
*/

ALTER PROCEDURE [PREORDENES].[usp_VisorPreOrdenes_Consultar]
@pstrUsuario	VARCHAR(60), -- Usuario que ejecuta la acción
@pstrInfosesion	VARCHAR(1000) = Null -- XML con inofrmación que se utiliza para auditoria (usuario, ip máquina usuario, nombre máquina usuario, servidor web, etc.)
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
	SET @strParametros ='@pstrUsuario=' + ISNULL('''' + @pstrUsuario + '''', 'null') + 
						',@pstrInfosesion=' + ISNULL('''' + @pstrInfosesion + '''', 'null')
	-----------------------------------------------------------------------------------------------------------------
	-- Inicializar auditoria de uso
	SELECT @strOpcion = 'VisorPreOrdenes', @strProceso= 'Consultar', @strProcedimiento = OBJECT_NAME(@@PROCID), @strObjeto = '', @pstrUsuario=ISNULL(@pstrUsuario,'') 	-- Asignar valor a los parámetros para identificar la opción ejecutada
	EXEC @intIdAuditoria = PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pstrOpcion= @strOpcion, @pstrProceso = @strProceso, @pstrObjeto = @strObjeto, @pstrProcedimiento = @strProcedimiento, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrInfosesion = @pstrInfosesion
	
	-----------------------------------------------------------------------------------------------------------------
	-- Funcionalidad propia del procedimiento
	CREATE TABLE #tmpRetornoInformacion(
		logSeleccionado				BIT DEFAULT(0),
		intID						INT,
		intIDPreOrdenVisualizar		INT,
		strTipoPreOrden				VARCHAR(2),
		strDescripcionTipoPreOrden	VARCHAR(100),
		strTipoInversion			VARCHAR(2),
		strDescripcionTipoInversion	VARCHAR(100),
		strInstrumento				VARCHAR(25),
		dblValor					FLOAT,
		dblValorPendiente			FLOAT
	)
	
	INSERT INTO #tmpRetornoInformacion(
		intID,
		intIDPreOrdenVisualizar,
		strTipoPreOrden,
		strDescripcionTipoPreOrden,
		strTipoInversion,
		strDescripcionTipoInversion,
		strInstrumento,
		dblValor,
		dblValorPendiente
	)
	SELECT PRE.intID,
		CASE WHEN PRE.logEsParcial=1 THEN PRE.intIDPreOrdenOrigen ELSE PRE.intID END AS intIDPreOrdenVisualizar,
		PRE.strTipoPreOrden AS strTipoPreOrden,
		CASE WHEN PRE.strTipoPreOrden='C' THEN 'Compra' ELSE 'Venta' END AS strDescripcionTipoPreOrden,
		PRE.strTipoInversion AS strTipoInversion,
		CASE WHEN PRE.strTipoInversion='C' THEN 'Renta fija' ELSE 'Acciones' END AS strDescripcionTipoInversion,
		PRE.strInstrumento AS strInstrumento,
		PRE.dblValor AS dblValor,
		ISNULL(PRE.dblValorPendiente,PRE.dblValor) AS dblValorPendiente
	FROM PREORDENES.tblPreOrdenes AS PRE
	WHERE logActivo=1
		AND PRE.logEsParcial=1
	ORDER BY PRE.intID ASC

	INSERT INTO #tmpRetornoInformacion(
		intID,
		intIDPreOrdenVisualizar,
		strTipoPreOrden,
		strDescripcionTipoPreOrden,
		strTipoInversion,
		strDescripcionTipoInversion,
		strInstrumento,
		dblValor,
		dblValorPendiente
	)
	SELECT PRE.intID,
		CASE WHEN PRE.logEsParcial=1 THEN PRE.intIDPreOrdenOrigen ELSE PRE.intID END AS intIDPreOrdenVisualizar,
		PRE.strTipoPreOrden AS strTipoPreOrden,
		CASE WHEN PRE.strTipoPreOrden='C' THEN 'Compra' ELSE 'Venta' END AS strDescripcionTipoPreOrden,
		PRE.strTipoInversion AS strTipoInversion,
		CASE WHEN PRE.strTipoInversion='C' THEN 'Renta fija' ELSE 'Acciones' END AS strDescripcionTipoInversion,
		PRE.strInstrumento AS strInstrumento,
		PRE.dblValor AS dblValor,
		ISNULL(PRE.dblValorPendiente,PRE.dblValor) AS dblValorPendiente
	FROM PREORDENES.tblPreOrdenes AS PRE
	WHERE logActivo=1
		AND ISNULL(PRE.logEsParcial,0)=0
	ORDER BY PRE.intID ASC

	SELECT logSeleccionado,
		intID,
		intIDPreOrdenVisualizar,
		strTipoPreOrden,
		strDescripcionTipoPreOrden,
		strTipoInversion,
		strDescripcionTipoInversion,
		strInstrumento,
		dblValor,
		dblValorPendiente
	FROM #tmpRetornoInformacion

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
