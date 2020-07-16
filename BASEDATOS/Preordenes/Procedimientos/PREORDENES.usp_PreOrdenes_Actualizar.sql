IF not exists (Select * From sys.sysobjects Where Id = object_id('[PREORDENES].[usp_PreOrdenes_Actualizar]', 'P'))
BEGIN
	execute('Create Procedure [PREORDENES].[usp_PreOrdenes_Actualizar] As return(0)')
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*------------------------------------------------------------------------------------------------------------------

Procedimiento [MAESTROS_GEN].[usp_PreOrdenes_Actualizar]
 
Descripción: Actualiza los registros en la tabla de PREORDENES.tblPreOrdenes.

Desarrollado por: Natalia Andrea Otalvaro
Fecha: Enero 24/2019

Ejemplo:	

exec [MAESTROS_GEN].[usp_PreOrdenes_Actualizar] 
@pintID=0,
@pdtmFechaInversion='2019-02-19',
@pdtmFechaVigencia='2019-02-20',
@pintIDCodigoPersona=10,
@pstrTipoPreOrden='C',
@pstrTipoInversion='C',
@pintIDEntidad=NULL,
@pstrInstrumento='VE',
@pstrIntencion='',
@pdblValor=23155202,
@pdblPrecio=0,
@pdblRentabilidadMinima=1,
@pdblRentabilidadMaxima=2,
@pstrInstrucciones='Prueba 1',
@plogActivo=1,
@pstrDetallePortafolio='<Detalles/>',
@pstrTablaValidaciones='#tmpValidaciones',
@pstrUsuario='natalia.otalvaro'
*/

ALTER PROCEDURE [PREORDENES].[usp_PreOrdenes_Actualizar]
@pintID					INT,
@pdtmFechaInversion		DATETIME,
@pdtmFechaVigencia		DATETIME,
@pintIDCodigoPersona	INT,
@pstrTipoPreOrden		VARCHAR(2),
@pstrTipoInversion		VARCHAR(2),
@pintIDEntidad			INT,
@pstrInstrumento		VARCHAR(15),
@pstrIntencion			VARCHAR(2),
@pdblValor				FLOAT,
@pdblPrecio				FLOAT,
@pdblRentabilidadMinima	FLOAT,
@pdblRentabilidadMaxima	FLOAT,
@pstrInstrucciones		VARCHAR(500),
@plogActivo				BIT,
@pstrDetallePortafolio	VARCHAR(MAX),
@pstrTablaValidaciones	VARCHAR(100),
@pstrUsuario			VARCHAR(60), -- Usuario que ejecuta la acción
@pstrMaquina			VARCHAR(60), -- Maquina que ejecuta la acción
@pstrInfosesion			VARCHAR(2000) = Null -- XML con inofrmación que se utiliza para auditoria (usuario, ip máquina usuario, nombre máquina usuario, servidor web, etc.)
--WITH ENCRYPTION
AS 

SET NOCOUNT ON

-- Declaraciones generales requeridas por el manejo de error y auditoria
DECLARE @strParametros VARCHAR(2000), @intNroRegistros INT, @strProcedimiento VARCHAR(100), @intIdAuditoria INT, @strOpcion VARCHAR(255), @strProceso VARCHAR(100), 
        @strObjeto VARCHAR(100), @strMsg VARCHAR(2000), @intInconsistencias INT, @strSQLMsg nvarchar(4000), @strSQL nvarchar(4000), @intCodigoMsgErr varchar(100) = 111

-----------------------------------------------------------------------------------------------------------------
-- Declaraciones propias del procedimiento
-----------------------------------------------------------------------------------------------------------------
DECLARE @tmpPreOrdenes_Portafolio	TABLE(
		[intID] [INT] IDENTITY(1,1),
		[intIDRegistro][INT] NULL,
		[lngIDRecibo][INT] NULL,
		[lngSecuencia][INT] NULL,
		[strNroTitulo][VARCHAR](30) NULL,
		[strInstrumento][VARCHAR](15) NULL,
		[dblTasaReferencia][FLOAT] NULL,
		[intIDEntidad][INT] NULL,
		[dblValorNominal][FLOAT] NULL,
		[dblValorCompra][FLOAT] NULL,
		[dblVPNMercado][FLOAT] NULL,
		[dtmFechaCompra][DATETIME] NULL
)

DECLARE @xmlDetalle			XML,
		@lngIDRecibo		INT,
		@lngSecuencia		INT,
		@strNroTitulo		VARCHAR(30),
		@strInstrumento		VARCHAR(15),
		@dblTasaReferencia	FLOAT,
		@intIDEntidad		INT,
		@dblValorNominal		FLOAT,
		@dblValorCompra		FLOAT,
		@dblVPNMercado		FLOAT,
		@dtmFechaCompra		DATETIME,
		@strURLNotificaciones	VARCHAR(200),
		@strTipoMensaje			VARCHAR(200),
		@strMensajeConsola		VARCHAR(500),
		@strXLM					NVARCHAR(MAX),
		@strInfoMensaje			NVARCHAR(MAX),
		@strTopico				VARCHAR(200)

BEGIN TRY
	-- Concatenar todos los parámetros para guardarlos en el log de uso
	SET @strParametros = '@pintID=' + isnull('''' + CONVERT(VARCHAR,@pintID) + '''', 'null') + 
						',@pdtmFechaInversion=' + isnull('''' + CONVERT(VARCHAR,@pdtmFechaInversion) + '''', 'null') + 
						',@pdtmFechaVigencia=' + isnull('''' + CONVERT(VARCHAR,@pdtmFechaVigencia) + '''', 'null') + 
						',@pintIDCodigoPersona=' + isnull('''' + CONVERT(VARCHAR,@pintIDCodigoPersona) + '''', 'null') + 
						',@pstrTipoPreOrden=' + isnull('''' + @pstrTipoPreOrden + '''', 'null') + 
						',@pstrTipoInversion=' + isnull('''' + @pstrTipoInversion + '''', 'null') + 
						',@pintIDEntidad=' + isnull('''' + CONVERT(VARCHAR, @pintIDEntidad) + '''', 'null') + 
						',@pstrInstrumento=' + isnull('''' + @pstrInstrumento + '''', 'null') + 
						',@pstrIntencion=' + isnull('''' + @pstrIntencion + '''', 'null') + 
						',@pdblValor=' + isnull('''' + CONVERT(VARCHAR, @pdblValor) + '''', 'null') + 
						',@pdblPrecio=' + isnull('''' + CONVERT(VARCHAR, @pdblPrecio) + '''', 'null') + 
						',@pdblRentabilidadMinima=' + isnull('''' + CONVERT(VARCHAR, @pdblRentabilidadMinima) + '''', 'null') + 
						',@pdblRentabilidadMaxima=' + isnull('''' + CONVERT(VARCHAR, @pdblRentabilidadMaxima) + '''', 'null') + 
						',@pstrInstrucciones=' + isnull('''' + @pstrInstrucciones + '''', 'null') + 
						',@plogActivo=' + isnull('''' + CONVERT(VARCHAR, @plogActivo) + '''', 'null') + 
						',@pstrInstrucciones=' + isnull('''' + @pstrInstrucciones + '''', 'null') + 
						',@pstrDetallePortafolio=' + isnull('''' + @pstrDetallePortafolio + '''', 'null') + 
						',@pstrUsuario=' + isnull('''' + @pstrUsuario + '''', 'null') + 
						',@pstrInfosesion=' + isnull('''' + @pstrInfosesion + '''', 'null')

	-----------------------------------------------------------------------------------------------------------------
	-- Inicializar auditoria de uso
	SELECT @intInconsistencias = 1, @strOpcion = 'PreOrdenes', @strProceso= 'Actualizar', @strProcedimiento = OBJECT_NAME(@@PROCID), @strObjeto = '', @pstrUsuario=ISNULL(@pstrUsuario,'') 	-- Asignar valor a los parámetros para identIFicar la opción ejecutada
	EXEC @intIdAuditoria = PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pstrOpcion= @strOpcion, @pstrProceso = @strProceso, @pstrObjeto = @strObjeto, @pstrProcedimiento = @strProcedimiento, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrInfosesion = @pstrInfosesion

	IF EXISTS (SELECT * FROM tempdb.sys.sysobjects WHERE id = object_id('TempDB..' + @pstrTablaValidaciones, 'U'))
	BEGIN	
		-- Instrucción para consultar el mensaje que se debe retornar
		set @strSQLMsg = 'Insert Into  ' + @pstrTablaValidaciones + ' (strCodMensaje, strTipoMensaje, strMensaje, logInconsitencia, intIDRegistro) ' +
			             '    SELECT strCodMensaje, strTipoMensaje, strMensaje, logInconsistencia, @pintIDRegistro ' +
					     '    FROM PLATAFORMA.ufnA2_Mensajes_obtenerInfoMsgProceso(@pstrCodigoMsg, @pstrParametrosMsg, @pstrProducto, @pstrGrupo)' --CCM201710

		-- Validar si existen inconsistencias pendientes por resolver
		SET @strSQL = 'Select @intInconsistencias= count(*) From ' + @pstrTablaValidaciones + ' Where strTipoMensaje in (Select strTipoMensaje From PLATAFORMA.tblA2_MensajesSistemaTipos Where logDetieneProceso = 1)'
		exec sp_executesql @strSQL, N'@intInconsistencias bit output', @intInconsistencias = @intInconsistencias output
	
		IF @intInconsistencias = 0
		BEGIN
		
			-----------------------------------------------------------------------------------------------------------------
			-- Funcionalidad propia del procedimiento
			--Lleva a una tabla temporal los detalles que se van a actualizar.
			SELECT @xmlDetalle = @pstrDetallePortafolio
			INSERT INTO @tmpPreOrdenes_Portafolio(intIDRegistro,lngIDRecibo,lngSecuencia,strNroTitulo,strInstrumento,
											dblTasaReferencia,intIDEntidad,dblValorNominal,dblValorCompra,dblVPNMercado,dtmFechaCompra)
			SELECT	Tab.col.value('@intIDRegistro','INTEGER') AS intIDRegistro,
				Tab.col.value('@lngIDRecibo','INTEGER') AS lngIDRecibo,
				Tab.col.value('@lngSecuencia','INTEGER') AS lngSecuencia,
				Tab.col.value('@strNroTitulo','VARCHAR(30)') AS strNroTitulo,
				Tab.col.value('@strInstrumento','VARCHAR(15)') AS strInstrumento,
				Tab.col.value('@dblTasaReferencia','FLOAT') AS dblTasaReferencia,
				Tab.col.value('@intIDEntidad','INTEGER') AS intIDEntidad,
				Tab.col.value('@dblValorNominal','FLOAT') AS dblValorNominal,
				Tab.col.value('@dblValorCompra','FLOAT') AS dblValorCompra,
				Tab.col.value('@dblVPNMercado','FLOAT') AS dblVPNMercado,
				Tab.col.value('@dtmFechaCompra','DATETIME') AS dtmFechaCompra
			FROM @xmlDetalle.nodes('/Detalles/Detalle') AS Tab(col)

			IF NOT EXISTS(SELECT 1 FROM PREORDENES.tblPreOrdenes WHERE intID=@pintID)
			BEGIN
				-- Insertamos el registro
				INSERT INTO [PREORDENES].[tblPreOrdenes]
				(
					dtmFechaInversion,dtmFechaVigencia,intIDCodigoPersona,strTipoPreOrden,strTipoInversion,intIDEntidad,strInstrumento,
					strIntencion,dblValor,dblPrecio,dblRentabilidadMinima,dblRentabilidadMaxima,strInstrucciones,logActivo,
					strUsuario,dtmActualizacion,strUsuarioInsercion,dtmFechaCreacion,
					logEsParcial,intIDPreOrdenOrigen,dblValorPendiente,intNroParcial
				)
				VALUES(
					@pdtmFechaInversion,@pdtmFechaVigencia,@pintIDCodigoPersona,@pstrTipoPreOrden,@pstrTipoInversion,@pintIDEntidad,@pstrInstrumento,
					@pstrIntencion,@pdblValor,@pdblPrecio,@pdblRentabilidadMinima,@pdblRentabilidadMaxima,@pstrInstrucciones,1,
					@pstrUsuario,GETDATE(),@pstrUsuario,GETDATE(),
					0,NULL,@pdblValor,0
				)

				SET @intNroRegistros = @@ROWCOUNT
				SET @pintID = SCOPE_IDENTITY()

				UPDATE [PREORDENES].[tblPreOrdenes]
				SET intIDPreOrdenOrigen=@pintID
				WHERE intID=@pintID

				IF @pstrTipoPreOrden='V'
				BEGIN
					INSERT INTO [PREORDENES].[tblPreOrdenes_Portafolio](
						intIDPreOrden,lngIDRecibo,lngSecuencia,strNroTitulo,strInstrumento,dblTasaReferencia,intIDEntidad,dblValorNominal,dblValorCompra,
						dblVPNMercado,dtmFechaCompra,strUsuario,dtmActualizacion,strUsuarioInsercion
					)
					SELECT @pintID,lngIDRecibo,lngSecuencia,strNroTitulo,strInstrumento,dblTasaReferencia,intIDEntidad,dblValorNominal,dblValorCompra,
						dblVPNMercado,dtmFechaCompra,@pstrUsuario,GETDATE(),@pstrUsuario
					FROM @tmpPreOrdenes_Portafolio
				END
			END
			ELSE -- IF (@pintID) is null
			BEGIN
				-- Actualizamos el registro
				UPDATE [PREORDENES].[tblPreOrdenes]
				SET dtmFechaInversion=@pdtmFechaInversion,
					dtmFechaVigencia=@pdtmFechaVigencia,
					intIDCodigoPersona=@pintIDCodigoPersona,
					strTipoPreOrden=@pstrTipoPreOrden,
					strTipoInversion=@pstrTipoInversion,
					intIDEntidad=@pintIDEntidad,
					strInstrumento=@pstrInstrumento,
					strIntencion=@pstrIntencion,
					dblValor=@pdblValor,
					dblPrecio=@pdblPrecio,
					dblRentabilidadMinima=@pdblRentabilidadMinima,
					dblRentabilidadMaxima=@pdblRentabilidadMaxima,
					strInstrucciones=@pstrInstrucciones,
					logActivo=@plogActivo,
					strUsuario=@pstrUsuario,
					dtmActualizacion=GETDATE(),
					dblValorPendiente=@pdblValor,
					intIDPreOrdenOrigen=CASE WHEN intIDPreOrdenOrigen IS NULL THEN @pintID ELSE intIDPreOrdenOrigen END,
					intNroParcial=CASE WHEN intNroParcial IS NULL THEN 0 ELSE intNroParcial END
				WHERE intID=@pintID

				SET @intNroRegistros = @@ROWCOUNT

				IF @pstrTipoPreOrden='V'
				BEGIN
					IF EXISTS(SELECT 1 
							FROM [PREORDENES].[tblPreOrdenes_Portafolio] P
							WHERE P.intIDPreOrden=@pintID)
					BEGIN
						SELECT TOP 1 @lngIDRecibo=lngIDRecibo,
							@lngSecuencia=lngSecuencia,
							@strNroTitulo=strNroTitulo,
							@strInstrumento=strInstrumento,
							@dblTasaReferencia=dblTasaReferencia,
							@intIDEntidad=intIDEntidad,
							@dblValorNominal=dblValorNominal,
							@dblValorCompra=dblValorCompra,
							@dblVPNMercado=dblVPNMercado,
							@dtmFechaCompra=dtmFechaCompra
						FROM @tmpPreOrdenes_Portafolio
						
						UPDATE P
						SET lngIDRecibo=@lngIDRecibo,
							lngSecuencia=@lngSecuencia,
							strNroTitulo=@strNroTitulo,
							strInstrumento=@strInstrumento,
							dblTasaReferencia=@dblTasaReferencia,
							intIDEntidad=@intIDEntidad,
							dblValorNominal=@dblValorNominal,
							dblValorCompra=@dblValorCompra,
							dblVPNMercado=@dblVPNMercado,
							dtmFechaCompra=@dtmFechaCompra,
							strUsuario=@pstrUsuario,
							dtmActualizacion=GETDATE()
						FROM [PREORDENES].[tblPreOrdenes_Portafolio] P
						WHERE P.intIDPreOrden=@pintID
					END
					ELSE
					BEGIN
						INSERT INTO [PREORDENES].[tblPreOrdenes_Portafolio](
							intIDPreOrden,lngIDRecibo,lngSecuencia,strNroTitulo,strInstrumento,dblTasaReferencia,intIDEntidad,dblValorNominal,dblValorCompra,
							dblVPNMercado,dtmFechaCompra,strUsuario,dtmActualizacion,strUsuarioInsercion
						)
						SELECT @pintID,lngIDRecibo,lngSecuencia,strNroTitulo,strInstrumento,dblTasaReferencia,intIDEntidad,dblValorNominal,dblValorCompra,
							dblVPNMercado,dtmFechaCompra,@pstrUsuario,GETDATE(),@pstrUsuario
						FROM @tmpPreOrdenes_Portafolio
					END
				END
				ELSE
				BEGIN
					DELETE P
					FROM [PREORDENES].[tblPreOrdenes_Portafolio] P
					WHERE P.intIDPreOrden=@pintID
				END
			END -- IF (@pintID) is null
			
			/*NOTIFICACIONES*/
			--*********************************************************************************************************************
			SET @strURLNotificaciones = (SELECT strValor FROM dbo.tblParametros WHERE strParametro='URL_SERVICIO_NOTIFICACIONES')
			SET @strTipoMensaje = 'VISORPREORDENES_NOTIFICACION'
			SET @strTopico=''
			SET @strMensajeConsola = ''
			SET @strXLM='<PreOrden>'
						+'<ID>' + CONVERT(VARCHAR(20), @pintID) + '</ID>'
						+'<Accion>Ingreso/Modificación de PreOrden</Accion>'
						+'<Estado>Pendiente</Estado>'
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
			-----------------------------------------------------------------------------------------------------------------

			-- Notificar que la actualización finalizó exitosamente
			exec sp_executesql @strSQLMsg, N'@pstrCodigoMsg varchar(100), @pstrParametrosMsg varchar(1000), @pstrProducto varchar(25), @pstrModulo varchar(25), @pstrGrupo varchar(50), @pintIDRegistro INT', 'PLATAFORMA_ACTUALIZACION_EXITOSA', '', '', '', '', @pintID --CCM201710
		END 
		ELSE -- IF @intInconsistencias = 0
		BEGIN
			-- Notificar que la actualización NO finalizó exitosamente
			exec sp_executesql @strSQLMsg, N'@pstrCodigoMsg varchar(100), @pstrParametrosMsg varchar(1000), @pstrProducto varchar(25), @pstrModulo varchar(25), @pstrGrupo varchar(50), @pintIDRegistro INT', 'PLATAFORMA_ACTUALIZACION_INCONSISTENTE', '', '', '', '', @pintID --CCM201710
		END -- IF @intInconsistencias = 0
	END
	ELSE
	BEGIN
		-- Notificar que la actualización NO se realizó
		SET @strMsg = PLATAFORMA.ufnA2_Mensajes_obtenerMsgProceso('PLATAFORMA_ACTUALIZACION_NO_REALIZADA', '', '', '') --CCM201710
		raiserror(@strMsg, 16, @intCodigoMsgErr)
	END
	
	-- Cerrar registro de auditoria
	EXEC PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pintLogAuditoriaUso = @intIdAuditoria, @pintNroRegistros = @intNroRegistros
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	IF ERROR_STATE() <> @intCodigoMsgErr SET @strMsg = PLATAFORMA.ufnA2_Mensajes_obtenerMsgError('PLATAFORMA_ERROR_ACTUALIZAR', 'PreOrdenes', '', '') --CCM201710
	EXEC PLATAFORMA.uspA2_Util_CrearLogErroresSQL @pstrProceso = @@ProcId, @pstrMsgPersonalizado = @strMsg, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrMaquina = NULL, @pstrInfosesion = @pstrInfosesion, @plogLanzarError = 1
END CATCH
