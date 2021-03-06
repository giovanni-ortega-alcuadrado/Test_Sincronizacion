﻿IF not exists (Select * From sys.sysobjects Where Id = object_id('[PREORDENES].[usp_PreOrdenes_Consultar]', 'P'))
BEGIN
	EXECUTE('Create Procedure [PREORDENES].[usp_PreOrdenes_Consultar] As return(0)')
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*------------------------------------------------------------------------------------------------------------------

Procedimiento [PREORDENES].[usp_PreOrdenes_Consultar]
 
Descripción: Consulta los registros que cumplan con todos los criterios indicados en los parámetros de búsqueda.
			Aplica para la tabla de PREORDENES.tblPreOrdenes

Desarrollado por: Natalia Andrea Otalvaro
Fecha: Enero 24/2019

Ejemplo:	
exec [PREORDENES].[usp_PreOrdenes_Consultar]
@pintID=0,
@pdtmFechaInversion=NULL,
@pdtmFechaVigencia=NULL,
@pintIDCodigoPersona=0,
@pstrTipoPreOrden='',
@pstrTipoInversion='',
@pintIDEntidad=0,
@pstrIntencion='',
@plogActivo=NULL,
@pstrUsuario='natalia.otalvaro'

*/

ALTER PROCEDURE [PREORDENES].[usp_PreOrdenes_Consultar]
@pintID					INT,
@pdtmFechaInversion		DATETIME,
@pdtmFechaVigencia		DATETIME,
@pintIDPersona			INT,
@pstrIDComitente		VARCHAR(17),
@pstrTipoPreOrden		VARCHAR(2),
@pstrTipoInversion		VARCHAR(2),
@pintIDEntidad			INT,
@pstrIntencion			VARCHAR(15),
@plogActivo				BIT,
@pstrUsuario			VARCHAR(60), -- Usuario que ejecuta la acción
@pstrInfosesion			VARCHAR(1000) = Null, -- XML con inofrmación que se utiliza para auditoria (usuario, ip máquina usuario, nombre máquina usuario, servidor web, etc.)
@plogConsultarCruzada	        BIT=0,
@pstrInfosesionAdicionall	VARCHAR(1000) = Null, -- XML con inofrmación que se utiliza para auditoria (usuario, ip máquina usuario, nombre máquina usuario, servidor web, etc.)
--WITH ENCRYPTION
AS 

SET NOCOUNT ON

-- Declaraciones generales requeridas por el manejo de error y auditoria
DECLARE @strParametros VARCHAR(2000), @intNroRegistros INT, @strProcedimiento VARCHAR(100), @intIdAuditoria INT, @strOpcion VARCHAR(255), @strProceso VARCHAR(100), @strObjeto VARCHAR(100), @strMsg VARCHAR(2000)

-----------------------------------------------------------------------------------------------------------------
-- Declaraciones propias del procedimiento
-----------------------------------------------------------------------------------------------------------------
DECLARE @intIDCodigoPersona		INT


BEGIN TRY
	-- Concatenar todos los parámetros para guardarlos en el log de uso
	set @strParametros = 
						'@pintID=' + isnull('''' + CONVERT(VARCHAR,@pintID) + '''', 'null') + 
						',@pdtmFechaInversion=' + isnull('''' + CONVERT(VARCHAR,@pdtmFechaInversion) + '''', 'null') + 
						',@pdtmFechaVigencia=' + isnull('''' + CONVERT(VARCHAR,@pdtmFechaVigencia) + '''', 'null') + 
						',@pintIDPersona=' + isnull('''' + CONVERT(VARCHAR,@pintIDPersona) + '''', 'null') + 
						',@pstrIDComitente=' + isnull('''' + CONVERT(VARCHAR,@pstrIDComitente) + '''', 'null') + 
						',@pstrTipoPreOrden=' + isnull('''' + @pstrTipoPreOrden + '''', 'null') + 
						',@pstrTipoInversion=' + isnull('''' + @pstrTipoInversion + '''', 'null') + 
						',@plogActivo=' + isnull('''' + CONVERT(VARCHAR,@plogActivo) + '''', 'null') + 
						',@pintIDEntidad=' + isnull('''' + CONVERT(VARCHAR,@pintIDEntidad) + '''', 'null') + 
						',@pstrIntencion=' + isnull('''' + @pstrIntencion + '''', 'null') + 
						',@pstrUsuario=' + isnull('''' + @pstrUsuario + '''', 'null') + 
						',@pstrInfosesion=' + isnull('''' + @pstrInfosesion + '''', 'null') +
						',@plogConsultarCruzada=' + isnull('''' + CONVERT(VARCHAR,@plogConsultarCruzada) + '''', 'null')

	-----------------------------------------------------------------------------------------------------------------
	-- Inicializar auditoria de uso
	SELECT @strOpcion = 'PreOrdenes', @strProceso= 'Consultar', @strProcedimiento = OBJECT_NAME(@@PROCID), @strObjeto = '', @pstrUsuario=ISNULL(@pstrUsuario,'') 	-- Asignar valor a los parámetros para identificar la opción ejecutada
	EXEC @intIdAuditoria = PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pstrOpcion= @strOpcion, @pstrProceso = @strProceso, @pstrObjeto = @strObjeto, @pstrProcedimiento = @strProcedimiento, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrInfosesion = @pstrInfosesion

	--------------------------------------------------------------------------------------------------------------------------------------------------------------------
	-- Validar uso de textos no válidos en los parámetros tipo texto (varchar, nvarchar, ...), si se utilizan instrucciones dinámicas (sp_executesql, execute)
	IF (PLATAFORMA.ufnA2_ValidacionCaracteresEspeciales((@pstrTipoPreOrden))=0) OR
		(PLATAFORMA.ufnA2_ValidacionCaracteresEspeciales((@pstrTipoInversion))=0)OR
		(PLATAFORMA.ufnA2_ValidacionCaracteresEspeciales((@pstrIntencion))=0)
	BEGIN
		SET @strMsg = PLATAFORMA.ufnA2_Mensajes_obtenerMsgError('PLATAFORMA_ERROR_VALIDAR_INYECCION_SQL', 'PreOrdenes', '', '') -- CCM201711
		RAISERROR (@strMsg, 16, 1 )
	END 

	-----------------------------------------------------------------------------------------------------------------
	-- Funcionalidad propia del procedimiento
	IF EXISTS(SELECT 1 FROM Personas.tblCodigos WHERE intIDPersona=@pintIDPersona AND intIDComitente=@pstrIDComitente)
	BEGIN
		SELECT TOP 1 @intIDCodigoPersona=intID
		FROM Personas.tblCodigos 
		WHERE intIDPersona=@pintIDPersona 
			AND intIDComitente=@pstrIDComitente
	END

	SET @pintID= ISNULL (@pintID,0)
	SET @intIDCodigoPersona= ISNULL (@intIDCodigoPersona,0)
	SET @pstrTipoPreOrden= ISNULL (@pstrTipoPreOrden,'')
	SET @pstrTipoInversion= ISNULL (@pstrTipoInversion,'')
	SET @pintIDEntidad= ISNULL (@pintIDEntidad,0)
	SET @pstrIntencion= ISNULL (@pstrIntencion,'')
	
	IF @pintID<0
		SET @pintID=0
	
	IF @plogActivo=0
	BEGIN
		SELECT  TOP 100 
			PRE.intIDPreOrden AS intID
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
			,CASE WHEN PRE.strIntencion='E' THEN 'Valor económico' ELSE 'Valor nominal' END AS strDescripcionIntencion
			,PRE.dblValor
			,PRE.strUsuario
			,PRE.dtmFechaCreacion
			,PRE.dtmActualizacion
			,PRE.strUsuarioInsercion
			,PRE.logActivo
			,ISNULL(PRE.dblValorPendiente,0) AS dblValorPendiente
		FROM [PREORDENES].[tblPreOrdenes_Anuladas] PRE
		INNER JOIN [Personas].tblCodigos COMITENTE ON COMITENTE.intID=PRE.intIDCodigoPersona
		INNER JOIN [Personas].tblPersonas PERSONA ON PERSONA.intID=COMITENTE.intIDPersona
		LEFT JOIN dbo.tblEspecies ESPECIE ON ESPECIE.strID=PRE.strInstrumento
		LEFT JOIN CF.tblEntidades ENTIDAD ON ENTIDAD.intIDEntidad=PRE.intIDEntidad
		WHERE (PRE.intID = @pintID OR @pintID=0) 
			AND   (PRE.intIDCodigoPersona = @intIDCodigoPersona OR @intIDCodigoPersona='')
			AND   (PRE.strTipoPreOrden = @pstrTipoPreOrden OR @pstrTipoPreOrden='')
			AND   (PRE.strTipoInversion = @pstrTipoInversion OR @pstrTipoInversion='')
			AND   (PRE.intIDEntidad = @pintIDEntidad OR @pintIDEntidad=0)
			AND   (PRE.strIntencion = @pstrIntencion OR @pstrIntencion='')
		ORDER BY dtmFechaCreacion DESC
	END
	ELSE IF @plogConsultarCruzada=1
	BEGIN
		SELECT  TOP 100 
			PRE.intID
			,PRE.intIDPreOrden AS intIDPreOrdenOrigen
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
			,CASE WHEN PRE.strIntencion='E' THEN 'Valor económico' ELSE 'Valor nominal' END AS strDescripcionIntencion
			,PRE.dblValor
			,PRE.strUsuario
			,PRE.dtmFechaCreacion
			,PRE.dtmActualizacion
			,PRE.strUsuarioInsercion
			,PRE.logActivo
			,CONVERT(FLOAT,0) AS dblValorPendiente
		FROM [PREORDENES].[tblPreOrdenes_Cruzadas] PRE
		INNER JOIN [Personas].tblCodigos COMITENTE ON COMITENTE.intID=PRE.intIDCodigoPersona
		INNER JOIN [Personas].tblPersonas PERSONA ON PERSONA.intID=COMITENTE.intIDPersona
		LEFT JOIN dbo.tblEspecies ESPECIE ON ESPECIE.strID=PRE.strInstrumento
		LEFT JOIN CF.tblEntidades ENTIDAD ON ENTIDAD.intIDEntidad=PRE.intIDEntidad
		WHERE PRE.intID = @pintID
		ORDER BY dtmFechaCreacion DESC
	END
	ELSE
	BEGIN
		SELECT  TOP 100 
			PRE.intID
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
			,CASE WHEN PRE.strIntencion='E' THEN 'Valor económico' ELSE 'Valor nominal' END AS strDescripcionIntencion
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
		WHERE (PRE.intID = @pintID OR @pintID=0) 
			AND   (PRE.intIDCodigoPersona = @intIDCodigoPersona OR @intIDCodigoPersona='')
			AND   (PRE.strTipoPreOrden = @pstrTipoPreOrden OR @pstrTipoPreOrden='')
			AND   (PRE.strTipoInversion = @pstrTipoInversion OR @pstrTipoInversion='')
			AND   (PRE.intIDEntidad = @pintIDEntidad OR @pintIDEntidad=0)
			AND   (PRE.strIntencion = @pstrIntencion OR @pstrIntencion='')
		ORDER BY dtmFechaCreacion DESC
	END
		
	-----------------------------------------------------------------------------------------------------------------
	
	SET @intNroRegistros = @@ROWCOUNT

	-- Cerrar registro de auditoria
	EXEC PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pintLogAuditoriaUso = @intIdAuditoria, @pintNroRegistros = @intNroRegistros
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	SET @strMsg = PLATAFORMA.ufnA2_Mensajes_obtenerMsgError('PLATAFORMA_ERROR_CONSULTAR', 'PreOrdenes', '', '') -- CCM201711
	EXEC PLATAFORMA.uspA2_Util_CrearLogErroresSQL @pstrProceso = @@ProcId, @pstrMsgPersonalizado = @strMsg, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrMaquina = NULL, @pstrInfosesion = @pstrInfosesion, @plogLanzarError = 1
END CATCH
