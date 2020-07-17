IF not exists (SELECT * FROM sys.sysobjects Where Id = object_id('[PREORDENES].[usp_PreOrdenes_AnulacionAutomatica]', 'P'))
BEGIN
	execute('Create Procedure [PREORDENES].[usp_PreOrdenes_AnulacionAutomatica] As return(0)')
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*------------------------------------------------------------------------------------------------------------------

Procedimiento [PREORDENES].[usp_PreOrdenes_AnulacionAutomatica]
 
Descripci�n: Anula los registros que corresponda con la clave primaria que se reciba.
			Aplica para la tabla de PREORDENES.tblPreOrdenes

Desarrollado por: Natalia Andrea Otalvaro
Fecha: Mayo 1/2019

Ejemplo:	exec [PREORDENES].[usp_PreOrdenes_AnulacionAutomatica]
*/

ALTER PROCEDURE [PREORDENES].[usp_PreOrdenes_AnulacionAutomatica]
--WITH ENCRYPTION
@pstrFiltro		VARCHAR(100)=''
AS 

SET NOCOUNT ON

-- Declaraciones generales requeridas por el manejo de error y auditoria
DECLARE @strParametros VARCHAR(2000), @intNroRegistros INT, @strProcedimiento VARCHAR(100), @intIdAuditoria INT, @strOpcion VARCHAR(255), @strProceso VARCHAR(100), @strObjeto VARCHAR(100), @strMsg VARCHAR(2000), @intInconsistencias smallint, @strSQL nvarchar(4000), @strCodigoMsg varchar(100) = ''

-----------------------------------------------------------------------------------------------------------------
-- Declaraciones propias del procedimiento

-----------------------------------------------------------------------------------------------------------------

BEGIN TRY
	-- Concatenar todos los par�metros para guardarlos en el log de uso
	SET @strParametros = ''

	-----------------------------------------------------------------------------------------------------------------
	-- Inicializar auditoria de uso
	SELECT @intInconsistencias = 1, @strOpcion = 'PreOrdenes', @strProceso= 'Anulacion automatica', @strProcedimiento = OBJECT_NAME(@@PROCID), @strObjeto = ''-- Asignar valor a los par�metros para identIFicar la opci�n ejecutada
	EXEC @intIdAuditoria = PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pstrOpcion= @strOpcion, @pstrProceso = @strProceso, @pstrObjeto = @strObjeto, @pstrProcedimiento = @strProcedimiento, @pstrParametros = @strParametros, @pstrUsuario = 'JOB ANULACION AUTOMATICA', @pstrInfosesion = NULL

	-----------------------------------------------------------------------------------------------------------------
	-- Definir tabla para manejo de inconsistencias

	-----------------------------------------------------------------------------------------------------------------
	-- Funcionalidad propia del procedimiento
	-----------------------------------------------------------------------------------------------------------------

	--ANULA LOS REGISTROS QUE SE ENCUENTRAN VENCIDOS
	UPDATE [PREORDENES].tblPreOrdenes 
	SET logActivo=0, strOrigenAnulacion='VENCIMIENTO', strObservaciones='Inactivaci�n por vencimiento.'
	WHERE CONVERT(VARCHAR(10), dtmFechaVigencia, 121)<CONVERT(VARCHAR(10), GETDATE(), 121)
	
	--TRASLADA LA INFORMACI�N DE LAS PREORDENES QUE SE ENCUENTRAN INACTIVAS A LA TABLA DE HISTORICO
	INSERT INTO PREORDENES.tblPreOrdenes_Anuladas(
		intIDPreOrden
		,strOrigenAnulacion
		,dtmFechaInversion
		,dtmFechaVigencia
		,intIDCodigoPersona
		,strTipoPreOrden
		,strTipoInversion
		,intIDEntidad
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
	)
	SELECT intID
		,ISNULL(strOrigenAnulacion,'USUARIO')
		,dtmFechaInversion
		,dtmFechaVigencia
		,intIDCodigoPersona
		,strTipoPreOrden
		,strTipoInversion
		,intIDEntidad
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
	FROM PREORDENES.tblPreOrdenes
	WHERE logActivo=0

	DELETE FROM PREORDENES.tblPreOrdenes
	WHERE logActivo=0

	-- Cerrar registro de auditoria
	EXEC PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pintLogAuditoriaUso = @intIdAuditoria, @pintNroRegistros = @intNroRegistros
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	SET @strMsg = PLATAFORMA.ufnA2_Mensajes_obtenerMsgError('PLATAFORMA_ERROR_ELIMINAR', 'PreOrdenes', '', '')
	EXEC PLATAFORMA.uspA2_Util_CrearLogErroresSQL @pstrProceso = @@ProcId, @pstrMsgPersonalizado = @strMsg, @pstrParametros = @strParametros, @pstrUsuario = 'JOB ANULACION AUTOMATICA', @pstrMaquina = NULL, @pstrInfosesion = NULL, @plogLanzarError = 1
END CATCH
