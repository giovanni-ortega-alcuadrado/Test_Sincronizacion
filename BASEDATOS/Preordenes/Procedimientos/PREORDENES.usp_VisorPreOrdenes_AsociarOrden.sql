IF not exists (Select * From sys.sysobjects Where Id = object_id('[PREORDENES].[usp_VisorPreOrdenes_AsociarOrden]', 'P'))
BEGIN
	EXECUTE('Create Procedure [PREORDENES].[usp_VisorPreOrdenes_AsociarOrden] As return(0)')
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*-----------------------------------------------------------------------------------------------------------------

Procedimiento [PREORDENES].[usp_VisorPreOrdenes_AsociarOrden]
 
Descripción: Realizar el cruce de los registros disponibles en el Visor de PreOrdenes. 
			Aplica para la tabla de PREORDENES.tblPreOrdenes

Desarrollado por: Natalia Andrea Otalvaro
Fecha: Marzo 25/2019

Ejemplo: 
exec [PREORDENES].[usp_VisorPreOrdenes_AsociarOrden] @pintIDRegistro=10,@pdblValorRegistro=1000000,@pstrRegistrosAsociados='13*500000',@pstrUsuario='juan.correa',@pstrInfosesion='<InfoSesion ip="10.10.40.164" usuario="VisorPreOrdenes_Generar" maquina=" " browser="Unknown - default 0.0" servidor="PC-JCORREA" modulo="juan.correa"></InfoSesion>'
*/

ALTER PROCEDURE [PREORDENES].[usp_VisorPreOrdenes_AsociarOrden]
@pintID						INT,
@pstrOrigenOrden			VARCHAR(20),
@pintNroRegistro			INT,
@pstrClaseRegistro			VARCHAR(2),
@pstrTipoOperacionRegistro	VARCHAR(2),
@pstrTipoOrigenRegistro		VARCHAR(4),
@pstrUsuario				VARCHAR(60), -- Usuario que ejecuta la acción
@pstrInfosesion				VARCHAR(1000) = Null -- XML con inofrmación que se utiliza para auditoria (usuario, ip máquina usuario, nombre máquina usuario, servidor web, etc.)
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
	SET @strParametros ='@pintID=' + ISNULL('''' + CONVERT(VARCHAR(20),@pintID) + '''', 'null') + 
						',@pintNroRegistro=' + ISNULL('''' + CONVERT(VARCHAR(20),@pintNroRegistro) + '''', 'null') + 
						',@pstrClaseRegistro=' + ISNULL('''' + @pstrClaseRegistro + '''', 'null') + 
						',@pstrTipoOperacionRegistro=' + ISNULL('''' + @pstrTipoOperacionRegistro + '''', 'null') + 
						',@pstrTipoOrigenRegistro=' + ISNULL('''' + @pstrTipoOrigenRegistro + '''', 'null') + 
						',@pstrUsuario=' + ISNULL('''' + @pstrUsuario + '''', 'null') + 
						',@pstrInfosesion=' + ISNULL('''' + @pstrInfosesion + '''', 'null')
	-----------------------------------------------------------------------------------------------------------------
	-- Inicializar auditoria de uso
	SELECT @strOpcion = 'VisorPreOrdenes', @strProceso= 'AsociarOrden', @strProcedimiento = OBJECT_NAME(@@PROCID), @strObjeto = '', @pstrUsuario=ISNULL(@pstrUsuario,'') 	-- Asignar valor a los parámetros para identificar la opción ejecutada
	EXEC @intIdAuditoria = PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pstrOpcion= @strOpcion, @pstrProceso = @strProceso, @pstrObjeto = @strObjeto, @pstrProcedimiento = @strProcedimiento, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrInfosesion = @pstrInfosesion
	
	-----------------------------------------------------------------------------------------------------------------
	-- Funcionalidad propia del procedimiento
	--DECLARACIÓN DE VARIABLES
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

	BEGIN TRANSACTION
		IF EXISTS(SELECT 1 FROM [PREORDENES].[tblPreOrdenes_Cruzadas_Orden] WHERE intIDPreOrden_Cruzada=@pintID)
		BEGIN
			UPDATE PREORDENES.tblPreOrdenes_Cruzadas_Orden
			SET intNroRegistro=@pintNroRegistro,
				strOrigenOrden=@pstrOrigenOrden,
				strClaseRegistro=@pstrClaseRegistro,
				strTipoOperacionRegistro=@pstrTipoOperacionRegistro,
				strTipoOrigenRegistro=@pstrTipoOrigenRegistro,
				dtmActualizacion=GETDATE(),
				strUsuario=@pstrUsuario
			WHERE intIDPreOrden_Cruzada=@pintID
		END
		ELSE
		BEGIN
			INSERT INTO PREORDENES.tblPreOrdenes_Cruzadas_Orden(
				intIDPreOrden_Cruzada
				,strOrigenOrden
				,intNroRegistro
				,strClaseRegistro
				,strTipoOperacionRegistro
				,strTipoOrigenRegistro
				,strUsuario
				,dtmFechaCreacion
				,dtmActualizacion
				,strUsuarioInsercion
			)
			VALUES(
				@pintID
				,@pstrOrigenOrden
				,@pintNroRegistro
				,@pstrClaseRegistro
				,@pstrTipoOperacionRegistro
				,@pstrTipoOrigenRegistro
				,@pstrUsuario
				,GETDATE()
				,GETDATE()
				,@pstrUsuario
			)
		END
	COMMIT TRANSACTION

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
	SET @strMsg = PLATAFORMA.ufnA2_Mensajes_obtenerMsgError('PLATAFORMA_ERROR_CONSULTARID', 'VisorPreOrdenes', '', '') -- CCM201711
	EXEC PLATAFORMA.uspA2_Util_CrearLogErroresSQL @pstrProceso = @@ProcId, @pstrMsgPersonalizado = @strMsg, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrMaquina = NULL, @pstrInfosesion = @pstrInfosesion, @plogLanzarError = 1
END CATCH
