IF NOT EXISTS ( SELECT *
                    FROM sys.sysobjects
                    WHERE id=OBJECT_ID('PREORDENES.usp_Procesos_Utilitarios_BuscadorGenerico','P') ) 
BEGIN
        EXECUTE('Create Procedure PREORDENES.usp_Procesos_Utilitarios_BuscadorGenerico AS return(0)')
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*------------------------------------------------------------------------------------------------------------------

Procedimiento [PREORDENES].[usp_Procesos_Utilitarios_BuscadorGenerico]
 
Descripción: Procedimiento para realizar las busquedas especificas del modulo PREORDENES. Este procedimiento
			es llamado del buscador generico principal creado para WPF.

Desarrollado por: Natalia Andrea Otalvaro
Fecha: Enero 24/2019

Ejemplo:	
exec [PREORDENES].[usp_Procesos_Utilitarios_BuscadorGenerico] 
@pstrCondicionFiltro='Col',
@pstrTipoItem='MAESTROS_GEN_PAIS',
@pstrEstadoItem='A'
@pstrAgrupacion='',
@pstrUsuario='natalia.otalvaro'
*/

ALTER PROCEDURE [PREORDENES].[usp_Procesos_Utilitarios_BuscadorGenerico]
@pstrCondicionFiltro varchar(50),	-- Condición por la cual se busca
@pstrTipoItem varchar(25),			-- Indica sobre que tipo de registros o tabla se debe hacer la búsqueda
@pstrEstadoItem varchar(25),		-- Estado de los registros sobre los que se busca.Si el estado no se recibe se consulta sobre todos los registros
@pstrAgrupacion varchar(1000),		-- Parámetro adicional que permitirá incluir busquedas por criterios específicos
@pstrCondicion1 varchar(100) = NULL,-- Condición 1 adicional para efectuar el filtro - SV20160203
@pstrCondicion2 varchar(100) = NULL,-- Condición 2 adicional para efectuar el filtro - SV20160203
@pstrUsuario varchar(60),
@pstrInfosesion	varchar(1000) = Null, -- Texto XML que enviará la aplicación para extraer la información de auditoria
@pintErrorPersonalizado	tinyint = 0 -- Cuando hay un error personalizado en el estado del error se envía este número, si es cero se envía el predefinido
AS 

SET NOCOUNT ON

-- Declaraciones generales requeridas por el manejo de error y auditoria
DECLARE @strParametros VARCHAR(2000), @intNroRegistros INT, @strProcedimiento VARCHAR(100), @intIdAuditoria INT, @strOpcion VARCHAR(255), @strProceso VARCHAR(100), @strObjeto VARCHAR(100), @strMsg VARCHAR(2000)

-----------------------------------------------------------------------------------------------------------------
-- Declaraciones propias del procedimiento
-----------------------------------------------------------------------------------------------------------------

BEGIN TRY
	-- Concatenar todos los parámetros para guardarlos en el log de uso
		SET @strParametros ='@pstrCondicionFiltro=' + isnull('''' + @pstrCondicionFiltro + '''', 'null') + 
						',@pstrTipoItem=' + isnull('''' + @pstrTipoItem + '''', 'null') + 
						',@pstrEstadoItem=' + isnull('''' + @pstrEstadoItem + '''', 'null') + 
						',@pstrAgrupacion=' + isnull('''' + @pstrAgrupacion + '''', 'null') + 
						',@pstrCondicion1=' + isnull('''' + @pstrCondicion1 + '''', 'null') + 
						',@pstrCondicion2=' + isnull('''' + @pstrCondicion2 + '''', 'null') + 
						',@pstrUsuario=' + isnull('''' + @pstrUsuario + '''', 'null') + 
						',@pstrInfosesion=' + isnull('''' + @pstrInfosesion + '''', 'null')

	-----------------------------------------------------------------------------------------------------------------
	-- Inicializar auditoria de uso
	SELECT @strOpcion = 'PREORDENES', @strProceso= 'BuscadorGenerico', @strProcedimiento = OBJECT_NAME(@@PROCID), @strObjeto = '', @pstrUsuario=ISNULL(@pstrUsuario,'') 	-- Asignar valor a los parámetros para identificar la opción ejecutada
	EXEC @intIdAuditoria = PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pstrOpcion= @strOpcion, @pstrProceso = @strProceso, @pstrObjeto = @strObjeto, @pstrProcedimiento = @strProcedimiento, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrInfosesion = @pstrInfosesion

	--------------------------------------------------------------------------------------------------------------------------------------------------------------------
	-----------------------------------------------------------------------------------------------------------------
	IF @pstrTipoItem='PREORDENES_ENTIDADES'
	BEGIN
		SELECT TOP 100 CONVERT(VARCHAR(20), intIDEntidad) AS strIdItem,
			strNombre AS strNombre,
			strNroDocumento AS strCodItem,
			strNroDocumento + ' - ' + strNombre AS strDescripcion,
			'ID: ' AS strEtiquetaIdItem,
			'Nro documento: ' AS strEtiquetaCodItem,
			strNroDocumento AS strClasificacion,
			'1' AS strCodEstado,
			'Activo' AS strEstado,
			'' AS strCodigoAuxiliar,
			CONVERT(VARCHAR(20),intDigitoVerificacion) AS strInfoAdicional01,
			'' AS strInfoAdicional02,
			'' AS strInfoAdicional03,
			'' AS strInfoAdicional04,
			'' AS strInfoAdicional05,
			'' AS strInfoAdicional06,
			'' AS strInfoAdicional07,
			'' AS strInfoAdicional08,
			'' AS strInfoAdicional09,
			'' AS strInfoAdicional10,
			'' AS strInfoAdicional11,
			'' AS strInfoAdicional12,
			'' AS strInfoAdicional13,
			'' AS strInfoAdicional14,
			'' AS strInfoAdicional15,
			'' AS strInfoAdicional16,
			'' AS strInfoAdicional17,
			'' AS strInfoAdicional18,
			'' AS strInfoAdicional19,
			'' AS strInfoAdicional20
		FROM	CF.tblEntidades
		WHERE	logActivo=1
			AND (strNroDocumento like @pstrCondicionFiltro
				OR strNombre like @pstrCondicionFiltro)
	END
	
	-----------------------------------------------------------------------------------------------------------------
	
	SET @intNroRegistros = @@ROWCOUNT

	-- Cerrar registro de auditoria
	EXEC PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pintLogAuditoriaUso = @intIdAuditoria, @pintNroRegistros = @intNroRegistros
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	SET @strMsg = PLATAFORMA.ufnA2_Mensajes_obtenerMsgError('PLATAFORMA_ERROR_CONSULTAR', 'Combos', '', '')
	EXEC PLATAFORMA.uspA2_Util_CrearLogErroresSQL @pstrProceso = @@ProcId, @pstrMsgPersonalizado = @strMsg, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrMaquina = NULL, @pstrInfosesion = @pstrInfosesion, @plogLanzarError = 1
END CATCH
