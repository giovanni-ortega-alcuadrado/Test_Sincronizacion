IF not exists (SELECT * FROM sys.sysobjects Where Id = object_id('[PREORDENES].[usp_PreOrdenes_Eliminar]', 'P'))
BEGIN
	execute('Create Procedure [PREORDENES].[usp_PreOrdenes_Eliminar] As return(0)')
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*------------------------------------------------------------------------------------------------------------------

Procedimiento [PREORDENES].[usp_PreOrdenes_Eliminar]
 
Descripción: Borra el registro que corresponda con la clave primaria que se reciba.
			Aplica para la tabla de PREORDENES.tblPreOrdenes

Desarrollado por: Natalia Andrea Otalvaro
Fecha: Enero 24/2019

Ejemplo:	exec [PREORDENES].[usp_PreOrdenes_Eliminar] @pintID=1,@pstrObservaciones='PRUEBA ELIMINACIÓN 1',@pstrUsuario='natalia.otalvaro',@pstrInfosesion=''
*/

ALTER PROCEDURE [PREORDENES].[usp_PreOrdenes_Eliminar]
@pintID					int,
@pstrObservaciones		VARCHAR(500),
@pstrUsuario			varchar(60), -- Usuario que ejecuta la acción
@pstrMaquina			varchar(60), -- Maquina que ejecuta la acción
@pstrInfosesion			varchar(2000) = Null -- XML con inofrmación que se utiliza para auditoria (usuario, ip máquina usuario, nombre máquina usuario, servidor web, etc.)
--WITH ENCRYPTION
AS 

SET NOCOUNT ON

-- Declaraciones generales requeridas por el manejo de error y auditoria
DECLARE @strParametros VARCHAR(2000), @intNroRegistros INT, @strProcedimiento VARCHAR(100), @intIdAuditoria INT, @strOpcion VARCHAR(255), @strProceso VARCHAR(100), @strObjeto VARCHAR(100), @strMsg VARCHAR(2000), @intInconsistencias smallint, @strSQL nvarchar(4000), @strCodigoMsg varchar(100) = ''

-----------------------------------------------------------------------------------------------------------------
-- Declaraciones propias del procedimiento

-----------------------------------------------------------------------------------------------------------------

BEGIN TRY
	-- Concatenar todos los parámetros para guardarlos en el log de uso
	SET @strParametros = '@pintID=' + isnull(convert(varchar, (@pintID)), 'null') +
						',@pstrUsuario=' + isnull('''' + @pstrUsuario + '''', 'null') + 
						',@pstrMaquina=' + isnull('''' + @pstrMaquina + '''', 'null') + 
						',@pstrInfosesion=' + isnull('''' + @pstrInfosesion + '''', 'null')

	-----------------------------------------------------------------------------------------------------------------
	-- Inicializar auditoria de uso
	SELECT @intInconsistencias = 1, @strOpcion = 'PreOrdenes', @strProceso= 'Eliminar', @strProcedimiento = OBJECT_NAME(@@PROCID), @strObjeto = '', @pstrUsuario=ISNULL(@pstrUsuario,'') 	-- Asignar valor a los parámetros para identIFicar la opción ejecutada
	EXEC @intIdAuditoria = PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pstrOpcion= @strOpcion, @pstrProceso = @strProceso, @pstrObjeto = @strObjeto, @pstrProcedimiento = @strProcedimiento, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrInfosesion = @pstrInfosesion

	-----------------------------------------------------------------------------------------------------------------
	-- Definir tabla para manejo de inconsistencias

	Create table #tmpA2ValInconsElim_PreOrdenes (
		intIdValidacion int not null identity(1,1),
		strTipoMensaje varchar(100) not null,
		strCodMensaje varchar(100) not null,
		strMensaje varchar(2000) not null,
		strCampo varchar(100) not null CONSTRAINT DF_#tmpA2ValIncons_tmpA2ValInconsAct_PreOrdenes_Campo Default(''),
		intOrden int not null default(0),
		logInconsitencia bit not null default(0),
		intIDRegistro int null -- CCM201711
	)

	DECLARE @strURLNotificaciones	VARCHAR(200),
			@strTipoMensaje			VARCHAR(200),
			@strMensajeConsola		VARCHAR(500),
			@strXLM					NVARCHAR(MAX),
			@strInfoMensaje			NVARCHAR(MAX),
			@strTopico				VARCHAR(200)

	-----------------------------------------------------------------------------------------------------------------
	-- Funcionalidad propia del procedimiento
	-----------------------------------------------------------------------------------------------------------------

	-- Validar si existen inconsistencias pendientes por resolver
	SELECT @intInconsistencias = COUNT(*) 
		FROM #tmpA2ValInconsElim_PreOrdenes Where strTipoMensaje in (SELECT strTipoMensaje FROM PLATAFORMA.tblA2_MensajesSistemaTipos Where logDetieneProceso = 1)
	
	SET @pstrObservaciones='Inactivada por el usuario (' + @pstrUsuario + '): ' + ISNULL(@pstrObservaciones,'')

	IF @intInconsistencias = 0
	BEGIN
		-- Ejecutar borrado de datos
		UPDATE [PREORDENES].tblPreOrdenes 
		SET logActivo=0, 
			strOrigenAnulacion='USUARIO',
			strObservaciones=@pstrObservaciones 
		WHERE intID = @pintID

		--LLAMA PROCEDIMIENTO PARA TRASLADAR LA INFORMACIÓN DE LAS ANULACIONES A LA TABLA NUEVA
		EXEC [PREORDENES].[usp_PreOrdenes_AnulacionAutomatica]

		/*NOTIFICACIONES*/
		--*********************************************************************************************************************
		SET @strURLNotificaciones = (SELECT strValor FROM dbo.tblParametros WHERE strParametro='URL_SERVICIO_NOTIFICACIONES')
		SET @strTipoMensaje = 'VISORPREORDENES_NOTIFICACION'
		SET @strTopico=''
		SET @strMensajeConsola = ''
		SET @strXLM='<PreOrden>'
					+'<ID>' + CONVERT(VARCHAR(20), @pintID) + '</ID>'
					+'<Accion>Anulación de PreOrden</Accion>'
					+'<Estado>Anulado</Estado>'
					+'</PreOrden>'

		SET @strInfoMensaje = (SELECT dbo.ufnNotificaciones_JSONSerializarTabla (@strXLM))

		--NOTIFICA A LA PANTALLA DEL VISOR
		EXEC dbo.uspNotificaciones_Notificar	@pstrURL = @strURLNotificaciones,
												@pstrTipoMensaje = @strTipoMensaje,
												@pstrTopicos = @strTopico,
												@pstrUsuariosNotificacion = '',
												@pstrInfoMensaje = @strInfoMensaje,
												@pstrMensajeConsola = @strMensajeConsola,
												@pstrUsuario = @pstrUsuario,
												@pstrMaquina = @pstrMaquina
		--*********************************************************************************************************************

		INSERT INTO #tmpA2ValInconsElim_PreOrdenes (strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia)
			SELECT strCodMensaje, strTipoMensaje, strMensaje, logInconsistencia 
				FROM PLATAFORMA.ufnA2_Mensajes_obtenerInfoMsgProceso('PLATAFORMA_ACTUALIZACION_EXITOSA','','','')
	END 

	SELECT intIdValidacion, strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia, intOrden, intIDRegistro, strCampo
		FROM #tmpA2ValInconsElim_PreOrdenes
		ORDER BY logInconsitencia desc, intOrden
	
	-- Cerrar registro de auditoria
	EXEC PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pintLogAuditoriaUso = @intIdAuditoria, @pintNroRegistros = @intNroRegistros
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	SET @strMsg = PLATAFORMA.ufnA2_Mensajes_obtenerMsgError('PLATAFORMA_ERROR_ELIMINAR', 'PreOrdenes', '', '')
	EXEC PLATAFORMA.uspA2_Util_CrearLogErroresSQL @pstrProceso = @@ProcId, @pstrMsgPersonalizado = @strMsg, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrMaquina = NULL, @pstrInfosesion = @pstrInfosesion, @plogLanzarError = 1
END CATCH
