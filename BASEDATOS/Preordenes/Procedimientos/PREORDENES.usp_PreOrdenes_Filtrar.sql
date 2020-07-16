IF not exists (Select * From sys.sysobjects Where Id = object_id('[PREORDENES].[usp_PreOrdenes_Filtrar]', 'P'))
BEGIN
	EXECUTE('Create Procedure [PREORDENES].[usp_PreOrdenes_Filtrar] As return(0)')
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*-----------------------------------------------------------------------------------------------------------------

Procedimiento [PREORDENES].[usp_PreOrdenes_Filtrar]
 
Descripci�n: Consulta los registros que cumplan con el criterio de b�squeda aplicado en los campos seleccionados para busqueda r�pia.
			 Aplica para la tabla de PREORDENES.tblPreOrdenes

Desarrollado por: Natalia Andrea Otalvaro
Fecha: Enero 24/2019

Ejemplo: 
exec [PREORDENES].[usp_PreOrdenes_Filtrar] @pstrFiltro = 'COL', @pstrUsuario = 'natalia.otalvaro', @pstrInfosesion = ''
*/

ALTER PROCEDURE [PREORDENES].[usp_PreOrdenes_Filtrar]
@pstrFiltro		VARCHAR(30),
@pstrUsuario	VARCHAR(60), -- Usuario que ejecuta la acci�n
@pstrInfosesion	VARCHAR(1000) = Null -- XML con inofrmaci�n que se utiliza para auditoria (usuario, ip m�quina usuario, nombre m�quina usuario, servidor web, etc.)
WITH ENCRYPTION
AS 

-- Validar longitud de infosesion y si debe ser nvarchar

SET NOCOUNT ON

-- Declaraciones generales requeridas por el manejo de error y auditoria
DECLARE @strParametros VARCHAR(2000),@intNroRegistros INT,@strProcedimiento VARCHAR(100),@intIdAuditoria INT,@strOpcion VARCHAR(255),@strProceso VARCHAR(100),@strObjeto VARCHAR(100), @strMsg VARCHAR(2000)

-----------------------------------------------------------------------------------------------------------------
-- Declaraciones propias del procedimiento
-----------------------------------------------------------------------------------------------------------------

BEGIN TRY
	-- Concatenar todos los par�metros para guardarlos en el log de uso
	SET @strParametros = '@pstrFiltro=' + ISNULL('''' + @pstrFiltro + '''', 'null') +
						',@pstrUsuario=' + ISNULL('''' + @pstrUsuario + '''', 'null') + 
						',@pstrInfosesion=' + ISNULL('''' + @pstrInfosesion + '''', 'null')
	-----------------------------------------------------------------------------------------------------------------
	-- Inicializar auditoria de uso
	SELECT @strOpcion = 'PreOrdenes', @strProceso= 'Filtrar', @strProcedimiento = OBJECT_NAME(@@PROCID), @strObjeto = '', @pstrUsuario=ISNULL(@pstrUsuario,'') 	-- Asignar valor a los par�metros para identificar la opci�n ejecutada
	EXEC @intIdAuditoria = PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pstrOpcion= @strOpcion, @pstrProceso = @strProceso, @pstrObjeto = @strObjeto, @pstrProcedimiento = @strProcedimiento, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrInfosesion = @pstrInfosesion

	--------------------------------------------------------------------------------------------------------------------------------------------------------------------
	-- Validar uso de textos no v�lidos en los par�metros tipo texto (varchar, nvarchar, ...), si se utilizan instrucciones din�micas (sp_executesql, execute)
	IF (PLATAFORMA.ufnA2_ValidacionCaracteresEspeciales(@pstrfiltro)=0)
	BEGIN
		SET @strMsg = PLATAFORMA.ufnA2_Mensajes_obtenerMsgError('PLATAFORMA_ERROR_VALIDAR_INYECCION_SQL', 'PreOrdenes', '', '') -- CCM201711
		RAISERROR (@strMsg, 16, 1 )
	END
	
	-----------------------------------------------------------------------------------------------------------------
	-- Funcionalidad propia del procedimiento
	-----------------------------------------------------------------------------------------------------------------
	SELECT  PRE.intID
		,PRE.intIDPreOrdenOrigen
		,PRE.dtmFechaInversion
		,PRE.dtmFechaVigencia
		,PERSONA.intID AS intIDPersona
		,COMITENTE.intIDComitente
		,PERSONA.strNroDocumento
		,PERSONA.strNombre
		,ESPECIE.strID AS strInstrumento
		,ESPECIE.strNombre AS strDescripcionInstrumento
		,ENTIDAD.strNroDocumento AS strNroDocumentoEntidad
		,ENTIDAD.strNombre AS strNombreEntidad
		,CASE WHEN PRE.strTipoPreOrden='C' THEN 'Compra' ELSE 'Venta' END AS strDescripcionTipoPreOrden
		,CASE WHEN PRE.strTipoInversion='A' THEN 'Acciones'
				  WHEN PRE.strTipoInversion='C' THEN 'Renta fija' 
				  ELSE '' END AS strDescripcionTipoInversion
		,CASE WHEN PRE.strIntencion='E' THEN 'Valor econ�mico' ELSE 'Valor nominal' END AS strDescripcionIntencion
		,PRE.dblValor
		,PRE.strUsuario
		,PRE.dtmFechaCreacion
		,PRE.dtmActualizacion
		,PRE.strUsuarioInsercion
		,PRE.logActivo
		,ISNULL(PRE.dblValorPendiente,0) AS dblValorPendiente
	FROM [PREORDENES].[tblPreOrdenes] PRE
	INNER JOIN [Personas].tblCodigos COMITENTE ON COMITENTE.intID=PRE.intIDCodigoPersona
	INNER JOIN [Personas].tblPersonas PERSONA ON PERSONA.intID=COMITENTE.intIDPersona
	LEFT JOIN dbo.tblEspecies ESPECIE ON ESPECIE.strID=PRE.strInstrumento
	LEFT JOIN CF.tblEntidades ENTIDAD ON ENTIDAD.intIDEntidad=PRE.intIDEntidad
	ORDER BY dtmFechaCreacion DESC
	
	SET @intNroRegistros = @@ROWCOUNT

	-- Cerrar registro de auditoria
	EXEC PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pintLogAuditoriaUso = @intIdAuditoria, @pintNroRegistros = @intNroRegistros
END TRY
BEGIN CATCH
	SET @strMsg = PLATAFORMA.ufnA2_Mensajes_obtenerMsgError('PLATAFORMA_ERROR_FILTRAR', 'PreOrdenes', '', '') -- CCM201711
	EXEC PLATAFORMA.uspA2_Util_CrearLogErroresSQL @pstrProceso = @@ProcId, @pstrMsgPersonalizado = @strMsg, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrMaquina = NULL, @pstrInfosesion = @pstrInfosesion, @plogLanzarError = 1
END CATCH
