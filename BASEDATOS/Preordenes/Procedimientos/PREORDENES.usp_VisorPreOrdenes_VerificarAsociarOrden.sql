IF not exists (Select * From sys.sysobjects Where Id = object_id('[PREORDENES].[usp_VisorPreOrdenes_VerificarAsociarOrden]', 'P'))
BEGIN
	EXECUTE('Create Procedure [PREORDENES].[usp_VisorPreOrdenes_VerificarAsociarOrden] As return(0)')
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*-----------------------------------------------------------------------------------------------------------------

Procedimiento [PREORDENES].[usp_VisorPreOrdenes_VerificarAsociarOrden]
 
Descripción: Verifica sí el registro no contiene Orden asociada en el Visor de PreOrdenes. 
			Aplica para la tabla de PREORDENES.tblPreOrdenes

Desarrollado por: Natalia Andrea Otalvaro
Fecha: Marzo 25/2019

Ejemplo: 
exec [PREORDENES].[usp_VisorPreOrdenes_VerificarAsociarOrden] @pintIDRegistro=10,@pdblValorRegistro=1000000,@pstrRegistrosAsociados='13*500000',@pstrUsuario='juan.correa',@pstrInfosesion='<InfoSesion ip="10.10.40.164" usuario="VisorPreOrdenes_Generar" maquina=" " browser="Unknown - default 0.0" servidor="PC-JCORREA" modulo="juan.correa"></InfoSesion>'
*/

ALTER PROCEDURE [PREORDENES].[usp_VisorPreOrdenes_VerificarAsociarOrden]
@pintID						INT,
@plogRegistroValido			BIT OUTPUT,
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
						',@plogRegistroValido=' + ISNULL('''' + CONVERT(VARCHAR(20),@plogRegistroValido) + '''', 'null') + 
						',@pstrUsuario=' + ISNULL('''' + @pstrUsuario + '''', 'null') + 
						',@pstrInfosesion=' + ISNULL('''' + @pstrInfosesion + '''', 'null')
	-----------------------------------------------------------------------------------------------------------------
	-- Inicializar auditoria de uso
	SELECT @strOpcion = 'VisorPreOrdenes', @strProceso= 'AsociarOrden', @strProcedimiento = OBJECT_NAME(@@PROCID), @strObjeto = '', @pstrUsuario=ISNULL(@pstrUsuario,'') 	-- Asignar valor a los parámetros para identificar la opción ejecutada
	EXEC @intIdAuditoria = PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pstrOpcion= @strOpcion, @pstrProceso = @strProceso, @pstrObjeto = @strObjeto, @pstrProcedimiento = @strProcedimiento, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrInfosesion = @pstrInfosesion
	
	-----------------------------------------------------------------------------------------------------------------
	-- Funcionalidad propia del procedimiento
	IF EXISTS(SELECT 1 FROM [PREORDENES].[tblPreOrdenes_Cruzadas_Orden] WHERE intIDPreOrden_Cruzada=@pintID)
	BEGIN
		SET @plogRegistroValido=0
	END
	ELSE
	BEGIN
		SET @plogRegistroValido=1
	END
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
