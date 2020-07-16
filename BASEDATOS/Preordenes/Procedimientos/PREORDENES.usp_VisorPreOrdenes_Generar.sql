IF not exists (Select * From sys.sysobjects Where Id = object_id('[PREORDENES].[usp_VisorPreOrdenes_Generar]', 'P'))
BEGIN
	EXECUTE('Create Procedure [PREORDENES].[usp_VisorPreOrdenes_Generar] As return(0)')
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*-----------------------------------------------------------------------------------------------------------------

Procedimiento [PREORDENES].[usp_VisorPreOrdenes_Generar]
 
Descripción: Realizar el cruce de los registros disponibles en el Visor de PreOrdenes. 
			Aplica para la tabla de PREORDENES.tblPreOrdenes

Desarrollado por: Natalia Andrea Otalvaro
Fecha: Marzo 25/2019

Ejemplo: 
exec [PREORDENES].[usp_VisorPreOrdenes_Generar] @pintIDRegistro=10,@pdblValorRegistro=1000000,@pstrRegistrosAsociados='13*500000',@pstrUsuario='juan.correa',@pstrInfosesion='<InfoSesion ip="10.10.40.164" usuario="VisorPreOrdenes_Generar" maquina=" " browser="Unknown - default 0.0" servidor="PC-JCORREA" modulo="juan.correa"></InfoSesion>'
*/

ALTER PROCEDURE [PREORDENES].[usp_VisorPreOrdenes_Generar]
@pintIDRegistro			INT,
@pdblValorRegistro		FLOAT,
@pstrRegistrosAsociados	VARCHAR(MAX),
@pstrUsuario			VARCHAR(60), -- Usuario que ejecuta la acción
@pstrMaquina			VARCHAR(60), -- Maquina que ejecuta la acción
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
	SET @strParametros ='@pintIDRegistro=' + ISNULL('''' + CONVERT(VARCHAR(20),@pintIDRegistro) + '''', 'null') + 
						',@pstrRegistrosAsociados=' + ISNULL('''' + @pstrRegistrosAsociados + '''', 'null') + 
						',@pstrUsuario=' + ISNULL('''' + @pstrUsuario + '''', 'null') + 
						',@pstrInfosesion=' + ISNULL('''' + @pstrInfosesion + '''', 'null')
	-----------------------------------------------------------------------------------------------------------------
	-- Inicializar auditoria de uso
	SELECT @strOpcion = 'VisorPreOrdenes', @strProceso= 'Generar', @strProcedimiento = OBJECT_NAME(@@PROCID), @strObjeto = '', @pstrUsuario=ISNULL(@pstrUsuario,'') 	-- Asignar valor a los parámetros para identificar la opción ejecutada
	EXEC @intIdAuditoria = PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pstrOpcion= @strOpcion, @pstrProceso = @strProceso, @pstrObjeto = @strObjeto, @pstrProcedimiento = @strProcedimiento, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrInfosesion = @pstrInfosesion
	
	-----------------------------------------------------------------------------------------------------------------
	-- Funcionalidad propia del procedimiento
	--DECLARACIÓN DE VARIABLES
	DECLARE @strTipoPreOrdenPrincipal		VARCHAR(2),
			@intCantidadRegistrosCruce		INT,
			@dblValorFaltanteRegistro		FLOAT,
			@guidCruce						UNIQUEIDENTIFIER,
			@strURLNotificaciones			VARCHAR(200),
			@strTipoMensaje					VARCHAR(200),
			@strMensajeConsola				VARCHAR(500),
			@strXLM							NVARCHAR(MAX),
			@strInfoMensaje					NVARCHAR(MAX),
			@strTopico						VARCHAR(200)
	
	CREATE TABLE #tmpPreOrdenesCruzar(
		intIDPreOrden		INT,
		dblValor			FLOAT
	)

	CREATE TABLE #tmpPreOrdenesActualizar(
		intIDRegistro		INT,
		dblValor			FLOAT,
		logPrincipal		BIT
	)

	CREATE TABLE #tmpA2ValInconsAct_PreOrdenes (
		intIdValidacion int not null identity(1,1),
		strTipoMensaje varchar(100) not null,
		strCodMensaje varchar(100) not null,
		strMensaje varchar(2000) not null,
		strCampo varchar(100) not null CONSTRAINT DF_#tmpA2ValIncons_tmpA2ValInconsAct_PreOrdenes_Campo Default(''),
		intOrden int not null default(0),
		logInconsitencia bit not null default(0),
		intIDRegistro int null
	)

	CREATE TABLE #tmpRegistros_Notificacion(
		ID			INT,
		Accion		VARCHAR(200),
		Estado		VARCHAR(200)
	)

	--REPARA EL DATO EN LA VARIABLE PARA EVITAR PROBLEMAS CON LAS ,
	SET @pstrRegistrosAsociados=REPLACE(@pstrRegistrosAsociados,',','.')

	--INSERTA EL CODIGO DE LAS PREORDENES A CRUZAR CON UNA PREORDEN BASE
	EXEC dbo.uspOyDNet_SepararRegistros 
					@pstrRegistros=@pstrRegistrosAsociados
					,@pstrTblTemporal='#tmpPreOrdenesCruzar'
					,@pstrLstCamposTbl='intIDPreOrden,dblValor'
					,@pstrSeparador='|'
					,@pstrSeparadorRegistro='*'
					,@plogIncluirComillas=0
	
	--VALIDA QUE LAS PREORDENES SE ENCUENTREN TODAVIA PENDIENTES POR PROCESAR
	IF NOT EXISTS(SELECT 1 FROM PREORDENES.tblPreOrdenes WHERE intIDPreOrdenOrigen=@pintIDRegistro AND dblValorPendiente=@pdblValorRegistro AND logActivo=1)
	BEGIN
		INSERT INTO #tmpA2ValInconsAct_PreOrdenes (strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia, strCampo)
		SELECT strTipoMensaje, strCodMensaje, strMensaje, logInconsistencia, 'CRUCEINVALIDO'
			FROM PLATAFORMA.ufnA2_Mensajes_obtenerInfoMsgProceso('PREORDENES_VISOR_CRUCEINVALIDO','','PREORDENES','PREORDENES')
	END

	IF NOT EXISTS(SELECT 1 
			FROM PREORDENES.tblPreOrdenes AS PRE
			INNER JOIN #tmpPreOrdenesCruzar AS CRUZAR ON CRUZAR.intIDPreOrden=PRE.intIDPreOrdenOrigen
			WHERE PRE.dblValorPendiente=CRUZAR.dblValor
				AND PRE.logActivo=1)
	BEGIN
		INSERT INTO #tmpA2ValInconsAct_PreOrdenes (strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia, strCampo)
		SELECT strTipoMensaje, strCodMensaje, strMensaje, logInconsistencia, 'CRUCEINVALIDO'
			FROM PLATAFORMA.ufnA2_Mensajes_obtenerInfoMsgProceso('PREORDENES_VISOR_CRUCEINVALIDO','','PREORDENES','PREORDENES')
	END

	SET @strURLNotificaciones = (SELECT strValor FROM dbo.tblParametros WHERE strParametro='URL_SERVICIO_NOTIFICACIONES')
	SET @strTopico=''
	SET @strMensajeConsola = ''

	IF NOT EXISTS(SELECT 1 FROM #tmpA2ValInconsAct_PreOrdenes)
	BEGIN
		--OBTIENE EL TIPO DE CRUCE PRINCIPAL
		SELECT @intCantidadRegistrosCruce=COUNT(*)
		FROM #tmpPreOrdenesCruzar

		SELECT @strTipoPreOrdenPrincipal=strTipoPreOrden
		FROM PREORDENES.tblPreOrdenes
		WHERE intIDPreOrdenOrigen=@pintIDRegistro

		--OBTIENE EL VALOR FALTANTE PARA CRUZAR EL TOTAL DE LA PREORDEN PRINCIPAL
		SELECT @dblValorFaltanteRegistro=@pdblValorRegistro-SUM(dblValor)
		FROM #tmpPreOrdenesCruzar

		--INSERTA EN LA TABLA TEMPORAL LOS REGISTROS PARA REALIZAR LA ACTUALIZACIÓN
		INSERT INTO #tmpPreOrdenesActualizar(intIDRegistro,dblValor,logPrincipal)
		SELECT PRE.intID,@pdblValorRegistro-@dblValorFaltanteRegistro,1
		FROM PREORDENES.tblPreOrdenes AS PRE
		WHERE PRE.intIDPreOrdenOrigen=@pintIDRegistro
		UNION ALL
		SELECT PRE.intID,CRUZAR.dblValor,0
		FROM PREORDENES.tblPreOrdenes AS PRE
		INNER JOIN #tmpPreOrdenesCruzar AS CRUZAR ON CRUZAR.intIDPreOrden=PRE.intIDPreOrdenOrigen

		SET @guidCruce=NEWID()

		--COMIENZA LA TRANSACCIÓN PARA REALIZAR EL PROCESO
		BEGIN TRANSACTION
			--INSERTA LOS CRUCES
			INSERT INTO PREORDENES.tblPreOrdenes_Cruces(
				guidCruce
				,intIDPreOrdenCompra
				,intIDPreOrdenVenta
				,strTipoCrucePrincipal
				,dtmFechaCruce
				,dblValorCruzado
				,strUsuario
				,dtmActualizacion
			)
			SELECT @guidCruce
				,CASE WHEN @strTipoPreOrdenPrincipal='C' THEN @pintIDRegistro ELSE PRE.intIDPreOrden END AS intIDPreOrdenCompra
				,CASE WHEN @strTipoPreOrdenPrincipal='V' THEN @pintIDRegistro ELSE PRE.intIDPreOrden END AS intIDPreOrdenVenta
				,CASE WHEN @intCantidadRegistrosCruce=1 THEN 'A' ELSE @strTipoPreOrdenPrincipal END AS strTipoCrucePrincipal
				,GETDATE() AS dtmFechaCruce
				,PRE.dblValor AS dblValorCruzado
				,@pstrUsuario AS strUsuario
				,GETDATE() AS dtmActualizacion
			FROM #tmpPreOrdenesCruzar AS PRE
			
			--TRASLADA LA INFORMACIÓN DE LAS ORDENES CRUZADA EN LA TABLA DE PREORDENES CRUZADAS
			INSERT INTO PREORDENES.tblPreOrdenes_Cruzadas(
				guidCruce
				,intIDPreOrden
				,logEsParcial
				,intNroParcial
				,dblValorInicial
				,dblValorAnterior
				,dblValor
				,dtmFechaInversion
				,dtmFechaVigencia
				,intIDCodigoPersona
				,strTipoPreOrden
				,strTipoInversion
				,intIDEntidad
				,strInstrumento
				,strIntencion
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
			)
			SELECT @guidCruce
				,PRE.intIDPreOrdenOrigen AS intIDPreOrden
				,PRE.logEsParcial AS logEsParcial
				,ISNULL(PRE.intNroParcial,0) AS intNroParcial
				,PRE.dblValor AS dblValorInicial
				,PRE.dblValorPendiente AS dblValorAnterior
				,ACT.dblValor AS dblValor
				,PRE.dtmFechaInversion
				,PRE.dtmFechaVigencia
				,PRE.intIDCodigoPersona
				,PRE.strTipoPreOrden
				,PRE.strTipoInversion
				,PRE.intIDEntidad
				,PRE.strInstrumento
				,PRE.strIntencion
				,PRE.dblPrecio
				,PRE.dblRentabilidadMinima
				,PRE.dblRentabilidadMaxima
				,PRE.strInstrucciones
				,PRE.logActivo
				,PRE.strObservaciones
				,PRE.strUsuario
				,PRE.dtmFechaCreacion
				,PRE.dtmActualizacion
				,PRE.strUsuarioInsercion
			FROM PREORDENES.tblPreOrdenes AS PRE
			INNER JOIN #tmpPreOrdenesActualizar AS ACT ON ACT.intIDRegistro=PRE.intID

			--CREA LAS ORDENES PARCIALES DE LO QUE NO SE CRUZO AL 100%
			IF @dblValorFaltanteRegistro>0
			BEGIN
				INSERT INTO PREORDENES.tblPreOrdenes(
					dtmFechaInversion
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
				SELECT PRE.dtmFechaInversion
					,PRE.dtmFechaVigencia
					,PRE.intIDCodigoPersona
					,PRE.strTipoPreOrden
					,PRE.strTipoInversion
					,PRE.intIDEntidad
					,PRE.strInstrumento
					,PRE.strIntencion
					,PRE.dblValor
					,PRE.dblPrecio
					,PRE.dblRentabilidadMinima
					,PRE.dblRentabilidadMaxima
					,PRE.strInstrucciones
					,PRE.logActivo
					,PRE.strObservaciones
					,PRE.strUsuario
					,PRE.dtmFechaCreacion
					,PRE.dtmActualizacion
					,PRE.strUsuarioInsercion
					,1 AS logEsParcial
					,PRE.intIDPreOrdenOrigen AS intIDPreOrdenOrigen
					,@dblValorFaltanteRegistro AS dblValorPendiente
					,ISNULL(PRE.intNroParcial,0) + 1 AS intNroParcial
				FROM PREORDENES.tblPreOrdenes AS PRE
				WHERE PRE.intIDPreOrdenOrigen=@pintIDRegistro
			END

			--ELIMINA LOS REGISTROS DE LA TABLA DE PREORDENES QUE FUERON CRUZADOS
			DELETE PRE
			FROM PREORDENES.tblPreOrdenes AS PRE
			INNER JOIN #tmpPreOrdenesActualizar AS ACT ON ACT.intIDRegistro=PRE.intID

		COMMIT TRANSACTION

		/*NOTIFICACIONES*/
		--*********************************************************************************************************************
		SET @strTipoMensaje = 'PREORDENES_NOTIFICACION'
		SET @strXLM='<PreOrden>'
						+'<ID>' + CONVERT(VARCHAR(20), @pintIDRegistro) + '</ID>'
						+'<Accion>Cruce de PreOrden</Accion>'
						+'<Estado>Cruzar</Estado>'
						+'</PreOrden>'

		SET @strInfoMensaje = (SELECT dbo.ufnNotificaciones_JSONSerializarTabla (@strXLM))

		--NOTIFICA A LA PANTALLA DE PREORDENES
		EXEC dbo.uspNotificaciones_Notificar	@pstrURL = @strURLNotificaciones,
												@pstrTipoMensaje = @strTipoMensaje,
												@pstrTopicos = @strTopico,
												@pstrUsuariosNotificacion = '',
												@pstrInfoMensaje = @strInfoMensaje,
												@pstrMensajeConsola = @strMensajeConsola,
												@pstrUsuario = @pstrUsuario,
												@pstrMaquina = @pstrMaquina
		--*********************************************************************************************************************

		INSERT INTO #tmpRegistros_Notificacion(ID,Accion,Estado)
		VALUES (@pintIDRegistro, 'Cruce de PreOrden', 'CRUCE')

		INSERT INTO #tmpRegistros_Notificacion(ID,Accion,Estado)
		SELECT intIDPreOrden, 'Cruce de PreOrden', 'CRUCE'
		FROM #tmpPreOrdenesCruzar
	END

	/*NOTIFICACIONES*/
	--*********************************************************************************************************************
	SET @strTipoMensaje = 'VISORPREORDENES_NOTIFICACION'
	SET @strXLM=(SELECT	ID, Accion, Estado
				FROM #tmpRegistros_Notificacion
				FOR XML RAW ('PreOrden'), ROOT ('PreOrdenes'), ELEMENTS)

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

	--NOTIFICA A LA PANTALLA DE PREORDENES
	SET @strTipoMensaje = 'PREORDENES_CRUZADAS_NOTIFICACION'
	EXEC dbo.uspNotificaciones_Notificar	@pstrURL = @strURLNotificaciones,
												@pstrTipoMensaje = @strTipoMensaje,
												@pstrTopicos = @strTopico,
												@pstrUsuariosNotificacion = '',
												@pstrInfoMensaje = @strInfoMensaje,
												@pstrMensajeConsola = @strMensajeConsola,
												@pstrUsuario = @pstrUsuario,
												@pstrMaquina = @pstrMaquina
	--*********************************************************************************************************************

	SELECT intIdValidacion,
		strTipoMensaje,
		strCodMensaje,
		strMensaje,
		strCampo,
		intOrden,
		logInconsitencia,
		intIDRegistro
	FROM #tmpA2ValInconsAct_PreOrdenes

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
