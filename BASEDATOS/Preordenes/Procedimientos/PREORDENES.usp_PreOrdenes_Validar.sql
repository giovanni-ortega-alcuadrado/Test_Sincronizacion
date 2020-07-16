IF not exists (SELECT * FROM sys.sysobjects Where Id = object_id('[PREORDENES].[usp_PreOrdenes_Validar]', 'P'))
BEGIN
	execute('Create Procedure [PREORDENES].[usp_PreOrdenes_Validar] As return(0)')
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*------------------------------------------------------------------------------------------------------------------

Procedimiento [PREORDENES].[usp_PreOrdenes_Validar]
 
Descripción: Valida los datos recibidos para ingresar o actualizar un registro, para garantizar que cumplan las condiciones
             de tipos de dato, integridad de datos y reglas de negocio.
			 Aplica para la tabla de PREORDENES.tblPreOrdenes

Desarrollado por: Natalia Andrea Otalvaro
Fecha: Enero 24/2019

Ejemplo:	
exec [PREORDENES].[usp_PreOrdenes_Validar] 
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
@plogSoloValidar=1,
@pstrUsuario='natalia.otalvaro'
*/

ALTER PROCEDURE [PREORDENES].[usp_PreOrdenes_Validar]
@pintID					INT,
@pdtmFechaInversion		DATETIME,
@pdtmFechaVigencia		DATETIME,
@pintIDPersona			INT,
@pstrIDComitente		VARCHAR(17),
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
@plogSoloValidar		bit, -- Solamente ejecuta las validaciones pero no realiza la actualización de datos
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

DECLARE @xmlDetalle				XML,
		@intIDCodigoPersona		INT

BEGIN TRY
	-- Concatenar todos los parámetros para guardarlos en el log de uso
	SET @strParametros = '@pintID=' + isnull('''' + CONVERT(VARCHAR,@pintID) + '''', 'null') + 
						',@pdtmFechaInversion=' + isnull('''' + CONVERT(VARCHAR,@pdtmFechaInversion) + '''', 'null') + 
						',@pdtmFechaVigencia=' + isnull('''' + CONVERT(VARCHAR,@pdtmFechaVigencia) + '''', 'null') + 
						',@pintIDPersona=' + isnull('''' + CONVERT(VARCHAR,@pintIDPersona) + '''', 'null') + 
						',@pstrIDComitente=' + isnull('''' + CONVERT(VARCHAR,@pstrIDComitente) + '''', 'null') + 
						',@pstrTipoPreOrden=' + isnull('''' + @pstrTipoPreOrden + '''', 'null') + 
						',@pstrTipoInversion=' + isnull('''' + @pstrTipoInversion + '''', 'null') + 
						',@pintIDEntidad=' + isnull('''' + CONVERT(VARCHAR, @pintIDEntidad) + '''', 'null') + 
						',@pstrInstrumento=' + isnull('''' + @pstrInstrumento + '''', 'null') + 
						',@pstrIntencion=' + isnull('''' + @pstrIntencion + '''', 'null') + 
						',@pdblValor=' + isnull('''' + CONVERT(VARCHAR, @pdblPrecio) + '''', 'null') + 
						',@pdblPrecio=' + isnull('''' + CONVERT(VARCHAR, @pdblValor) + '''', 'null') + 
						',@pdblRentabilidadMinima=' + isnull('''' + CONVERT(VARCHAR, @pdblRentabilidadMinima) + '''', 'null') + 
						',@pdblRentabilidadMaxima=' + isnull('''' + CONVERT(VARCHAR, @pdblRentabilidadMaxima) + '''', 'null') + 
						',@pstrInstrucciones=' + isnull('''' + @pstrInstrucciones + '''', 'null') + 
						',@plogActivo=' + isnull('''' + CONVERT(VARCHAR, @plogActivo) + '''', 'null') + 
						',@pstrDetallePortafolio=' + isnull('''' + @pstrDetallePortafolio + '''', 'null') + 
						',@plogSoloValidar=' + isnull(convert(varchar, @plogSoloValidar), 'null') +
						',@pstrUsuario=' + isnull('''' + @pstrUsuario + '''', 'null') + 
						',@pstrMaquina=' + isnull('''' + @pstrMaquina + '''', 'null') + 
						',@pstrInfosesion=' + isnull('''' + @pstrInfosesion + '''', 'null')

	-----------------------------------------------------------------------------------------------------------------
	-- Inicializar auditoria de uso
	SELECT @intInconsistencias = 1, @strOpcion = 'PreOrdenes', @strProceso= 'Validar', @strProcedimiento = OBJECT_NAME(@@PROCID), @strObjeto = '', @pstrUsuario=ISNULL(@pstrUsuario,'') 	-- Asignar valor a los parámetros para identIFicar la opción ejecutada
	EXEC @intIdAuditoria = PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pstrOpcion= @strOpcion, @pstrProceso = @strProceso, @pstrObjeto = @strObjeto, @pstrProcedimiento = @strProcedimiento, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrInfosesion = @pstrInfosesion

	
	-----------------------------------------------------------------------------------------------------------------
	-- Definir tabla para manejo de inconsistencias

	Create table #tmpA2ValInconsAct_PreOrdenes (
		intIdValidacion int not null identity(1,1),
		strTipoMensaje varchar(100) not null,
		strCodMensaje varchar(100) not null,
		strMensaje varchar(2000) not null,
		strCampo varchar(100) not null CONSTRAINT DF_#tmpA2ValIncons_tmpA2ValInconsAct_PreOrdenes_Campo Default(''),
		intOrden int not null default(0),
		logInconsitencia bit not null default(0),
		intIDRegistro int null -- CCM201711
	)


	-----------------------------------------------------------------------------------------------------------------
	-- Funcionalidad propia del procedimiento
	-----------------------------------------------------------------------------------------------------------------
	IF @pintIDEntidad=-1
	BEGIN
		SET @pintIDEntidad=NULL
	END
	IF @pstrInstrumento=''
	BEGIN
		SET @pstrInstrumento=NULL
	END
	
	IF EXISTS(SELECT 1 FROM Personas.tblCodigos WHERE intIDPersona=@pintIDPersona AND intIDComitente=@pstrIDComitente)
	BEGIN
		SELECT TOP 1 @intIDCodigoPersona=intID
		FROM Personas.tblCodigos 
		WHERE intIDPersona=@pintIDPersona 
			AND intIDComitente=@pstrIDComitente
	END

	IF @pstrTipoPreOrden NOT IN('C','V')
	BEGIN
		INSERT INTO #tmpA2ValInconsAct_PreOrdenes (strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia, strCampo)
			SELECT strTipoMensaje, strCodMensaje, strMensaje, logInconsistencia, 'TIPOPREORDEN'
				FROM PLATAFORMA.ufnA2_Mensajes_obtenerInfoMsgProceso('PREORDENES_PREORDENES_REQUERIDO_TIPOPREORDEN','','PREORDENES','PREORDENES')
	END

	IF @pdtmFechaInversion IS NULL
	BEGIN
		INSERT INTO #tmpA2ValInconsAct_PreOrdenes (strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia, strCampo)
			SELECT strTipoMensaje, strCodMensaje, strMensaje, logInconsistencia, 'FECHAINVERSION'
				FROM PLATAFORMA.ufnA2_Mensajes_obtenerInfoMsgProceso('PREORDENES_PREORDENES_REQUERIDO_FECHAINVERSION','','PREORDENES','PREORDENES')
	END

	IF @pdtmFechaVigencia IS NULL
	BEGIN
		INSERT INTO #tmpA2ValInconsAct_PreOrdenes (strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia, strCampo)
			SELECT strTipoMensaje, strCodMensaje, strMensaje, logInconsistencia, 'FECHAVIGENCIA'
				FROM PLATAFORMA.ufnA2_Mensajes_obtenerInfoMsgProceso('PREORDENES_PREORDENES_REQUERIDO_FECHAVIGENCIA','','PREORDENES','PREORDENES')
	END

	IF @pdtmFechaInversion IS NOT NULL AND @pdtmFechaVigencia IS NOT NULL
	BEGIN
		IF @pdtmFechaInversion>@pdtmFechaVigencia
		BEGIN
			INSERT INTO #tmpA2ValInconsAct_PreOrdenes (strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia, strCampo)
			SELECT strTipoMensaje, strCodMensaje, strMensaje, logInconsistencia, 'FECHAVIGENCIA'
				FROM PLATAFORMA.ufnA2_Mensajes_obtenerInfoMsgProceso('PREORDENES_PREORDENES_FECHASINVALIDAS','','PREORDENES','PREORDENES')
		END
	END

	IF ISNULL(@intIDCodigoPersona,0)=0
	BEGIN
		INSERT INTO #tmpA2ValInconsAct_PreOrdenes (strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia, strCampo)
		SELECT strTipoMensaje, strCodMensaje, strMensaje, logInconsistencia, 'CUENTA'
			FROM PLATAFORMA.ufnA2_Mensajes_obtenerInfoMsgProceso('PREORDENES_PREORDENES_REQUERIDO_CUENTA','','PREORDENES','PREORDENES')
	END
	ELSE
	BEGIN
		IF NOT EXISTS(SELECT 1 
					FROM Personas.tblCodigos C
					WHERE C.intID=@intIDCodigoPersona
						AND C.strEstado='A')
		BEGIN
			INSERT INTO #tmpA2ValInconsAct_PreOrdenes (strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia, strCampo)
			SELECT strTipoMensaje, strCodMensaje, strMensaje, logInconsistencia, 'CUENTA'
				FROM PLATAFORMA.ufnA2_Mensajes_obtenerInfoMsgProceso('PREORDENES_PREORDENES_NOEXISTE_CUENTA','','PREORDENES','PREORDENES')
		END
	END

	IF ISNULL(@pstrTipoInversion,'')=''
	BEGIN
		INSERT INTO #tmpA2ValInconsAct_PreOrdenes (strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia, strCampo)
		SELECT strTipoMensaje, strCodMensaje, strMensaje, logInconsistencia, 'TIPOINVERSION'
			FROM PLATAFORMA.ufnA2_Mensajes_obtenerInfoMsgProceso('PREORDENES_PREORDENES_REQUERIDO_TIPOINVERSION','','PREORDENES','PREORDENES')
	END

	IF ISNULL(@pstrInstrumento,'')<>''
	BEGIN
		IF NOT EXISTS(SELECT 1 FROM dbo.tblEspecies WHERE strId=@pstrInstrumento AND logActivo=1)
		BEGIN
			INSERT INTO #tmpA2ValInconsAct_PreOrdenes (strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia, strCampo)
				SELECT strTipoMensaje, strCodMensaje, strMensaje, logInconsistencia, 'INSTRUMENTO'
					FROM PLATAFORMA.ufnA2_Mensajes_obtenerInfoMsgProceso('PREORDENES_PREORDENES_NOEXISTE_INSTRUMENTO','','PREORDENES','PREORDENES')
		END
	END

	IF @pintIDEntidad>0
	BEGIN
		IF NOT EXISTS(SELECT 1 FROM CF.tblEntidades WHERE logActivo=1 AND intIDEntidad=@pintIDEntidad)
		BEGIN
			INSERT INTO #tmpA2ValInconsAct_PreOrdenes (strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia, strCampo)
				SELECT strTipoMensaje, strCodMensaje, strMensaje, logInconsistencia, 'ENTIDAD'
					FROM PLATAFORMA.ufnA2_Mensajes_obtenerInfoMsgProceso('PREORDENES_PREORDENES_NOEXISTE_ENTIDAD','','PREORDENES','PREORDENES')
		END
	END

	IF @pstrIntencion NOT IN('E','N')
	BEGIN
		INSERT INTO #tmpA2ValInconsAct_PreOrdenes (strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia, strCampo)
			SELECT strTipoMensaje, strCodMensaje, strMensaje, logInconsistencia, 'INTENCION'
				FROM PLATAFORMA.ufnA2_Mensajes_obtenerInfoMsgProceso('PREORDENES_PREORDENES_REQUERIDO_INTENCION','','PREORDENES','PREORDENES')
	END

	IF ISNULL(@pdblValor,0)=0
	BEGIN
		IF @pstrIntencion='E'
		BEGIN
			INSERT INTO #tmpA2ValInconsAct_PreOrdenes (strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia, strCampo)
				SELECT strTipoMensaje, strCodMensaje, strMensaje, logInconsistencia, 'VALOR'
					FROM PLATAFORMA.ufnA2_Mensajes_obtenerInfoMsgProceso('PREORDENES_PREORDENES_REQUERIDO_VALORECONOMICO','','PREORDENES','PREORDENES')
		END
		ELSE IF @pstrIntencion='N'
		BEGIN
			INSERT INTO #tmpA2ValInconsAct_PreOrdenes (strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia, strCampo)
				SELECT strTipoMensaje, strCodMensaje, strMensaje, logInconsistencia, 'VALOR'
					FROM PLATAFORMA.ufnA2_Mensajes_obtenerInfoMsgProceso('PREORDENES_PREORDENES_REQUERIDO_VALORNOMINAL','','PREORDENES','PREORDENES')
		END
		ELSE
		BEGIN
			INSERT INTO #tmpA2ValInconsAct_PreOrdenes (strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia, strCampo)
				SELECT strTipoMensaje, strCodMensaje, strMensaje, logInconsistencia, 'VALOR'
					FROM PLATAFORMA.ufnA2_Mensajes_obtenerInfoMsgProceso('PREORDENES_PREORDENES_REQUERIDO_VALOR','','PREORDENES','PREORDENES')
		END
	END

	IF @pstrTipoInversion='A'
	BEGIN
		IF ISNULL(@pdblPrecio,0)=0
		BEGIN
			INSERT INTO #tmpA2ValInconsAct_PreOrdenes (strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia, strCampo)
				SELECT strTipoMensaje, strCodMensaje, strMensaje, logInconsistencia, 'PRECIO'
					FROM PLATAFORMA.ufnA2_Mensajes_obtenerInfoMsgProceso('PREORDENES_PREORDENES_REQUERIDO_PRECIO','','PREORDENES','PREORDENES')
		END
	END
	ELSE IF @pstrTipoInversion='C'
	BEGIN
		IF ISNULL(@pdblRentabilidadMinima,0)=0
		BEGIN
			INSERT INTO #tmpA2ValInconsAct_PreOrdenes (strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia, strCampo)
				SELECT strTipoMensaje, strCodMensaje, strMensaje, logInconsistencia, 'RENTABILIDADMINIMA'
					FROM PLATAFORMA.ufnA2_Mensajes_obtenerInfoMsgProceso('PREORDENES_PREORDENES_REQUERIDO_RENTABILIDADMINIMA','','PREORDENES','PREORDENES')
		END
		IF ISNULL(@pdblRentabilidadMaxima,0)=0
		BEGIN
			INSERT INTO #tmpA2ValInconsAct_PreOrdenes (strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia, strCampo)
				SELECT strTipoMensaje, strCodMensaje, strMensaje, logInconsistencia, 'RENTABILIDADMAXIMA'
					FROM PLATAFORMA.ufnA2_Mensajes_obtenerInfoMsgProceso('PREORDENES_PREORDENES_REQUERIDO_RENTABILIDADMAXIMA','','PREORDENES','PREORDENES')
		END
		IF @pdblRentabilidadMinima>0 AND @pdblRentabilidadMaxima>0
		BEGIN
			IF @pdblRentabilidadMaxima<@pdblRentabilidadMinima
			BEGIN
				INSERT INTO #tmpA2ValInconsAct_PreOrdenes (strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia, strCampo)
					SELECT strTipoMensaje, strCodMensaje, strMensaje, logInconsistencia, 'RENTABILIDADMAXIMA'
						FROM PLATAFORMA.ufnA2_Mensajes_obtenerInfoMsgProceso('PREORDENES_PREORDENES_RENTABILIDADINVALIDA','','PREORDENES','PREORDENES')
			END
		END
	END

	IF @pstrTipoPreOrden='V'
	BEGIN
		SET @pstrDetallePortafolio=REPLACE(@pstrDetallePortafolio,'.','')
		SET @pstrDetallePortafolio=REPLACE(@pstrDetallePortafolio,',','.')
		SET @xmlDetalle=@pstrDetallePortafolio
		
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

		IF NOT EXISTS(SELECT 1 FROM @tmpPreOrdenes_Portafolio)
		BEGIN
			INSERT INTO #tmpA2ValInconsAct_PreOrdenes (strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia, strCampo)
				SELECT strTipoMensaje, strCodMensaje, strMensaje, logInconsistencia, 'PORTAFOLIO'
					FROM PLATAFORMA.ufnA2_Mensajes_obtenerInfoMsgProceso('PREORDENES_PREORDENES_REQUERIDO_PORTAFOLIO','','PREORDENES','PREORDENES')
		END
	END

	--verifica que la preorden no se halla cumplido desde el visor de prerodenes
	IF @pintID>0
	BEGIN
		IF NOT EXISTS(SELECT 1 FROM PREORDENES.tblPreOrdenes WHERE intID=@pintID)
		BEGIN
			INSERT INTO #tmpA2ValInconsAct_PreOrdenes (strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia, strCampo)
			SELECT strTipoMensaje, strCodMensaje, strMensaje, logInconsistencia, 'NODISPONIBLE'
				FROM PLATAFORMA.ufnA2_Mensajes_obtenerInfoMsgProceso('PREORDENES_PREORDENES_ORDENNODISPONIBLE','','PREORDENES','PREORDENES')
		END
		ELSE IF EXISTS(SELECT 1 FROM PREORDENES.tblPreOrdenes WHERE intID=@pintID AND logActivo=0)
		BEGIN
			INSERT INTO #tmpA2ValInconsAct_PreOrdenes (strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia, strCampo)
			SELECT strTipoMensaje, strCodMensaje, strMensaje, logInconsistencia, 'NODISPONIBLE'
				FROM PLATAFORMA.ufnA2_Mensajes_obtenerInfoMsgProceso('PREORDENES_PREORDENES_ORDENINACTIVA','','PREORDENES','PREORDENES')
		END
		ELSE IF EXISTS(SELECT 1 FROM PREORDENES.tblPreOrdenes WHERE intID=@pintID AND logEsParcial=1)
		BEGIN
			INSERT INTO #tmpA2ValInconsAct_PreOrdenes (strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia, strCampo)
			SELECT strTipoMensaje, strCodMensaje, strMensaje, logInconsistencia, 'NODISPONIBLE'
				FROM PLATAFORMA.ufnA2_Mensajes_obtenerInfoMsgProceso('PREORDENES_PREORDENES_ORDENNODISPONIBLE','','PREORDENES','PREORDENES')
		END
	END
	

	-- Validar si existen inconsistencias pendientes por resolver
	SELECT @intInconsistencias = COUNT(*) 
		FROM #tmpA2ValInconsAct_PreOrdenes Where strCodMensaje in (SELECT strCodMensaje FROM PLATAFORMA.tblA2_MensajesSistemaTipos Where logDetieneProceso = 1)
		
	IF @intInconsistencias = 0 And @plogSoloValidar = 0
	BEGIN
		print 'llama [PREORDENES].[usp_PreOrdenes_Actualizar]'
		-- Ejecutar actualización de datos
		exec [PREORDENES].[usp_PreOrdenes_Actualizar] 
				@pintID=@pintID,
				@pdtmFechaInversion=@pdtmFechaInversion,
				@pdtmFechaVigencia=@pdtmFechaVigencia,
				@pintIDCodigoPersona=@intIDCodigoPersona,
				@pstrTipoPreOrden=@pstrTipoPreOrden,
				@pstrTipoInversion=@pstrTipoInversion,
				@pintIDEntidad=@pintIDEntidad,
				@pstrInstrumento=@pstrInstrumento,
				@pstrIntencion=@pstrIntencion,
				@pdblValor=@pdblValor,
				@pdblPrecio=@pdblPrecio,
				@pdblRentabilidadMinima=@pdblRentabilidadMinima,
				@pdblRentabilidadMaxima=@pdblRentabilidadMaxima,
				@pstrInstrucciones=@pstrInstrucciones,
				@plogActivo=@plogActivo,
				@pstrDetallePortafolio=@pstrDetallePortafolio,
				@pstrTablaValidaciones='#tmpA2ValInconsAct_PreOrdenes', 
				@pstrUsuario=@pstrUsuario, 
				@pstrMaquina=@pstrMaquina, 
				@pstrInfosesion=@pstrInfosesion
		print 'termina [PREORDENES].[usp_PreOrdenes_Actualizar]'

	END 

	SELECT intIdValidacion, strTipoMensaje, strCodMensaje, strMensaje, logInconsitencia, intOrden, intIDRegistro, strCampo
		FROM #tmpA2ValInconsAct_PreOrdenes
		ORDER BY logInconsitencia desc, intOrden
	
	-- Cerrar registro de auditoria
	EXEC PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pintLogAuditoriaUso = @intIdAuditoria, @pintNroRegistros = @intNroRegistros
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	SET @strMsg = PLATAFORMA.ufnA2_Mensajes_obtenerMsgError('PLATAFORMA_ERROR_VALIDAR', 'PreOrdenes', '', '')
	EXEC PLATAFORMA.uspA2_Util_CrearLogErroresSQL @pstrProceso = @@ProcId, @pstrMsgPersonalizado = @strMsg, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrMaquina = NULL, @pstrInfosesion = @pstrInfosesion, @plogLanzarError = 1
END CATCH
