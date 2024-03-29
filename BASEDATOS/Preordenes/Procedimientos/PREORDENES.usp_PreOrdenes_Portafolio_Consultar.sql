IF not exists (Select * From sys.sysobjects Where Id = object_id('[PREORDENES].[usp_PreOrdenes_Portafolio_Consultar]', 'P'))
BEGIN
	EXECUTE('Create Procedure [PREORDENES].[usp_PreOrdenes_Portafolio_Consultar] As return(0)')
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*-----------------------------------------------------------------------------------------------------------------

Procedimiento [PREORDENES].[usp_PreOrdenes_ConsultarID]
 
Descripción: Consulta el registro que coincida en su clave principal con el Id enviado como parámetro. 
			Aplica para la tabla de PREORDENES.tblPreOrdenes

Desarrollado por: Natalia Andrea Otalvaro
Fecha: Enero 24/2019

Ejemplo: [PREORDENES].[usp_PreOrdenes_ConsultarID] @pintIDPreOrden=1, @pstrUsuario = 'natalia.otalvaro', @pstrInfosesion = ''
*/

ALTER PROCEDURE [PREORDENES].[usp_PreOrdenes_Portafolio_Consultar]
@pintIDPreOrden		INT,
@pstrUsuario		varchar(60), -- Usuario que ejecuta la acción
@pstrInfosesion		varchar(1000) = Null -- XML con inofrmación que se utiliza para auditoria (usuario, ip máquina usuario, nombre máquina usuario, servidor web, etc.)
--WITH ENCRYPTION
AS 

SET NOCOUNT ON

-- Declaraciones generales requeridas por el manejo de error y auditoria
DECLARE @strParametros VARCHAR(2000), @intNroRegistros INT, @strProcedimiento VARCHAR(100), @intIdAuditoria INT, @strOpcion VARCHAR(255), @strProceso VARCHAR(100), @strObjeto VARCHAR(100), @strMsg VARCHAR(2000)

-----------------------------------------------------------------------------------------------------------------
-- Declaraciones propias del procedimiento

-----------------------------------------------------------------------------------------------------------------

BEGIN TRY
	-- Concatenar todos los parámetros para guardarlos en el log de uso
	set @strParametros = 
						'@pintIDPreOrden=' + isnull('''' + CONVERT(VARCHAR,@pintIDPreOrden) + '''', 'null') + 
						',@pstrUsuario=' + isnull('''' + @pstrUsuario + '''', 'null') + 
						',@pstrInfosesion=' + isnull('''' + @pstrInfosesion + '''', 'null')
	-----------------------------------------------------------------------------------------------------------------
	-- Inicializar auditoria de uso
	SELECT @strOpcion = 'PreOrdenes_Portafolio', @strProceso= 'Consultar', @strProcedimiento = OBJECT_NAME(@@PROCID), @strObjeto = '', @pstrUsuario=ISNULL(@pstrUsuario,'') 	-- Asignar valor a los parámetros para identificar la opción ejecutada
	EXEC @intIdAuditoria = PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pstrOpcion= @strOpcion, @pstrProceso = @strProceso, @pstrObjeto = @strObjeto, @pstrProcedimiento = @strProcedimiento, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrInfosesion = @pstrInfosesion

	--------------------------------------------------------------------------------------------------------------------------------------------------------------------

	-----------------------------------------------------------------------------------------------------------------
	-- Funcionalidad propia del procedimiento
	SELECT P.intID
		,CONVERT(BIT, 1) AS logSeleccionado
		,P.lngIDRecibo
		,P.lngSecuencia
		,P.strNroTitulo
		,P.strInstrumento
		,E.strNombre AS strDescripcionInstrumento
		,P.dblTasaReferencia
		,P.intIDEntidad
		,E.strNroDocumento AS strNroDocumentoEntidad
		,E.strNombre AS strNombreEntidad
		,P.dblValorNominal
		,P.dblValorCompra
		,P.dblVPNMercado
		,P.dtmFechaCompra
	FROM PREORDENES.tblPreOrdenes_Portafolio P
	LEFT JOIN CF.tblEntidades E ON E.intIDEntidad=P.intIDEntidad
	LEFT JOIN dbo.tblEspecies ESP ON ESP.strId=P.strInstrumento
	WHERE intIDPreOrden=@pintIDPreOrden	
	-----------------------------------------------------------------------------------------------------------------
	
	SET @intNroRegistros = @@ROWCOUNT

	-- Cerrar registro de auditoria
	EXEC PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pintLogAuditoriaUso = @intIdAuditoria, @pintNroRegistros = @intNroRegistros
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	SET @strMsg = PLATAFORMA.ufnA2_Mensajes_obtenerMsgError('PLATAFORMA_ERROR_CONSULTAR', 'PreOrdenes_Portafolio', '', '') -- CCM201711
	EXEC PLATAFORMA.uspA2_Util_CrearLogErroresSQL @pstrProceso = @@ProcId, @pstrMsgPersonalizado = @strMsg, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrMaquina = NULL, @pstrInfosesion = @pstrInfosesion, @plogLanzarError = 1
END CATCH
