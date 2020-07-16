IF not exists (Select * From sys.sysobjects Where Id = object_id('[PREORDENES].[usp_PreOrdenes_ConsultarID]', 'P'))
BEGIN
	EXECUTE('Create Procedure [PREORDENES].[usp_PreOrdenes_ConsultarID] As return(0)')
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

Ejemplo: [PREORDENES].[usp_PreOrdenes_ConsultarID] @pintID=1, @pstrUsuario = 'natalia.otalvaro', @pstrInfosesion = ''
*/

ALTER PROCEDURE [PREORDENES].[usp_PreOrdenes_ConsultarID]
@pintID			INT, -- Clave primaria de la tabla. De acuerdo con el estándar toda tabla debe tener un Identity que es la clave primaria.
@pstrUsuario	VARCHAR(60), -- Usuario que ejecuta la acción
@pstrInfosesion	VARCHAR(1000) = Null, -- XML con inofrmación que se utiliza para auditoria (usuario, ip máquina usuario, nombre máquina usuario, servidor web, etc.)
@plogConsultarCruzada	BIT=0
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
	SET @strParametros ='@pintID=' + ISNULL('''' + isnull(convert(varchar(50), @pintID), 'null') + '''', 'null') + 
						',@pstrUsuario=' + ISNULL('''' + @pstrUsuario + '''', 'null') + 
						',@pstrInfosesion=' + ISNULL('''' + @pstrInfosesion + '''', 'null') +
						',@plogConsultarCruzada=' + ISNULL('''' + isnull(convert(varchar(50), @plogConsultarCruzada), 'null') + '''', 'null')
	-----------------------------------------------------------------------------------------------------------------
	-- Inicializar auditoria de uso
	SELECT @strOpcion = 'PreOrdenes', @strProceso= 'ConsultarID', @strProcedimiento = OBJECT_NAME(@@PROCID), @strObjeto = '', @pstrUsuario=ISNULL(@pstrUsuario,'') 	-- Asignar valor a los parámetros para identificar la opción ejecutada
	EXEC @intIdAuditoria = PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pstrOpcion= @strOpcion, @pstrProceso = @strProceso, @pstrObjeto = @strObjeto, @pstrProcedimiento = @strProcedimiento, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrInfosesion = @pstrInfosesion
	
	-----------------------------------------------------------------------------------------------------------------
	-- Funcionalidad propia del procedimiento
	IF @pintID=-1
	BEGIN
		SELECT -1 AS intID
			,GETDATE() AS dtmFechaInversion
			,GETDATE() AS dtmFechaVigencia
			,-1 AS intIDCodigoPersona
			,'' AS strTipoPreOrden
			,'' AS strTipoInversion
			,-1 AS intIDEntidad
			,'' AS strInstrumento
			,'E' AS strIntencion
			,CONVERT(FLOAT, 0) AS dblValor
			,CONVERT(FLOAT, 0) AS dblPrecio
			,CONVERT(FLOAT, 0) AS dblRentabilidadMinima
			,CONVERT(FLOAT, 0) AS dblRentabilidadMaxima
			,'' AS strInstrucciones
			,CONVERT(BIT, 1) AS logActivo
			,'' AS strObservaciones
			,@pstrUsuario AS strUsuario
			,GETDATE() AS dtmFechaCreacion
			,GETDATE() AS dtmActualizacion
			,@pstrUsuario AS strUsuarioInsercion
			,CONVERT(BIT, 0) AS logEsParcial
			,NULL AS intIDPreOrdenOrigen
			,CONVERT(FLOAT, 0) AS dblValorPendiente
			,NULL AS intNroParcial
			,NULL AS strOrigenAnulacion
	END
	ELSE IF @plogConsultarCruzada=1
	BEGIN
		SELECT intID
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
			,ISNULL(logEsParcial,0) AS logEsParcial
			,intIDPreOrden AS intIDPreOrdenOrigen
			,ISNULL(dblValorAnterior,0) AS dblValorPendiente
			,intNroParcial
			,'' AS strOrigenAnulacion
		FROM [PREORDENES].[tblPreOrdenes_Cruzadas]
		WHERE intID=@pintID
	END
	ELSE
	BEGIN
		SELECT intID
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
			,ISNULL(logEsParcial,0) AS logEsParcial
			,intIDPreOrdenOrigen
			,dblValorPendiente
			,intNroParcial
			,strOrigenAnulacion
		FROM [PREORDENES].[tblPreOrdenes]
		WHERE intID=@pintID
	END

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
