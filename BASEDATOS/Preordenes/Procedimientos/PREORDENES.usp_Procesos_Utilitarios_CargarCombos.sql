IF NOT EXISTS ( SELECT *
                    FROM sys.sysobjects
                    WHERE id=OBJECT_ID('PREORDENES.usp_Procesos_Utilitarios_CargarCombos','P') ) 
BEGIN
        EXECUTE('Create Procedure PREORDENES.usp_Procesos_Utilitarios_CargarCombos AS return(0)')
END

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*------------------------------------------------------------------------------------------------------------------

Procedimiento [PREORDENES].[usp_Procesos_Utilitarios_CargarCombos]
 
Descripción: Procedimiento para realizar la carga de combos especifico para el modulo PREORDENES. Este procedimiento
			es llamado desde el sp principal de cargar combos creado para WPF.

Desarrollado por: Natalia Andrea Otalvaro
Fecha: Febrero 24/2019

Ejemplo:	
exec [PREORDENES].[usp_Procesos_Utilitarios_CargarCombos] 
pstrProducto='',
@pstrCondicionTexto1='',
@pstrCondicionTexto2='',
@pstrCondicionEntero1='',
@pstrCondicionEntero2=''
*/

ALTER PROCEDURE [PREORDENES].[usp_Procesos_Utilitarios_CargarCombos]
@pstrProducto			VARCHAR(50) = '',
@pstrCondicionTexto1	VARCHAR(500),
@pstrCondicionTexto2	VARCHAR(500),
@pstrCondicionEntero1	INT,
@pstrCondicionEntero2	INT,
@plogCrearTablaTemporal Bit = 1, --Identifica si crea la tabla temporal RABP20181023
@pstrUsuario			VARCHAR(60), -- Usuario que ejecuta la acción
@pstrInfosesion			VARCHAR(2000) = Null -- XML con inofrmación que se utiliza para auditoria (usuario, ip máquina usuario, nombre máquina usuario, servidor web, etc.)
AS 

SET NOCOUNT ON

-- Declaraciones generales requeridas por el manejo de error y auditoria
DECLARE @strParametros VARCHAR(2000), @intNroRegistros INT, @strProcedimiento VARCHAR(100), @intIdAuditoria INT, @strOpcion VARCHAR(255), @strProceso VARCHAR(100), @strObjeto VARCHAR(100), @strMsg VARCHAR(2000)

-----------------------------------------------------------------------------------------------------------------
-- Declaraciones propias del procedimiento
-----------------------------------------------------------------------------------------------------------------
IF @plogCrearTablaTemporal = 1 --RABP2018
BEGIN
	CREATE TABLE #tmpCombosGenericoAplicacion(
		intID			INT,
		strTopico		VARCHAR(100),
		strOrigen       VARCHAR(100),
		strRetorno		VARCHAR(MAX),
		strDescripcion	VARCHAR(MAX),
		intIDDependencia1	INT,
		intIDDependencia2	INT,
		strDependencia1		VARCHAR(MAX),
		strDependencia2		VARCHAR(MAX)
	)
END
BEGIN TRY
	-- Concatenar todos los parámetros para guardarlos en el log de uso
		SET @strParametros ='@pstrProducto=' + isnull('''' + @pstrProducto + '''', 'null') + 
						',@pstrCondicionTexto1=' + isnull('''' + @pstrCondicionTexto1 + '''', 'null') + 
						',@pstrCondicionTexto2=' + isnull('''' + @pstrCondicionTexto2 + '''', 'null') + 
						',@pstrCondicionEntero1=' + ISNULL(CONVERT(VARCHAR, @pstrCondicionEntero1), 'null') +
						',@pstrCondicionEntero1=' + ISNULL(CONVERT(VARCHAR, @pstrCondicionEntero2), 'null') +
						',@pstrUsuario=' + isnull('''' + @pstrUsuario + '''', 'null') + 
						',@pstrInfosesion=' + isnull('''' + @pstrInfosesion + '''', 'null')

	-----------------------------------------------------------------------------------------------------------------
	-- Inicializar auditoria de uso
	SELECT @strOpcion = 'PREORDENES', @strProceso= 'Combos', @strProcedimiento = OBJECT_NAME(@@PROCID), @strObjeto = '', @pstrUsuario=ISNULL(@pstrUsuario,'') 	-- Asignar valor a los parámetros para identificar la opción ejecutada
	EXEC @intIdAuditoria = PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pstrOpcion= @strOpcion, @pstrProceso = @strProceso, @pstrObjeto = @strObjeto, @pstrProcedimiento = @strProcedimiento, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrInfosesion = @pstrInfosesion

	--------------------------------------------------------------------------------------------------------------------------------------------------------------------
	-----------------------------------------------------------------------------------------------------------------
	-- Funcionalidad propia del procedimiento
	INSERT INTO #tmpCombosGenericoAplicacion(intID,strTopico,strOrigen,strRetorno,strDescripcion,
											intIDDependencia1,intIDDependencia2,strDependencia1,strDependencia2)
	SELECT 1, 'PREORDENES_ESTADOS', 'A2PLATPreordenes.PreordenesView','NINGUNO','Ninguno',
			  NULL, NULL, '', ''
	UNION ALL
	SELECT 1, 'PREORDENES_ESTADOS', 'A2PLATPreordenes.PreordenesView','ACTIVO','Activo',
			  NULL, NULL, '', ''
	UNION ALL
	SELECT 1, 'PREORDENES_ESTADOS', 'A2PLATPreordenes.PreordenesView','INACTIVO','Inactivo',
			  NULL, NULL, '', ''
	UNION ALL
	SELECT 1, 'PREORDENES_TIPO', 'A2PLATPreordenes.PreordenesView','C','Compra',
			  NULL, NULL, '', ''
	UNION ALL
	SELECT 1, 'PREORDENES_TIPO', 'A2PLATPreordenes.PreordenesView','V','Venta',
			  NULL, NULL, '', ''
	UNION ALL
	SELECT 1, 'PREORDENES_TIPOINVERSION', 'A2PLATPreordenes.PreordenesView','C','Renta fija',
			  NULL, NULL, '', ''
	UNION ALL
	SELECT 1, 'PREORDENES_TIPOINVERSION', 'A2PLATPreordenes.PreordenesView','A','Renta variable',
			  NULL, NULL, '', ''
	UNION ALL
	SELECT 1, 'PREORDENES_INTENCION', 'A2PLATPreordenes.PreordenesView','E','Valor economico',
			  NULL, NULL, '', ''
	UNION ALL
	SELECT 1, 'PREORDENES_INTENCION', 'A2PLATPreordenes.PreordenesView','N','Valor nominal',
			  NULL, NULL, '', ''
	UNION ALL
	SELECT 1, 'VISORPREORDENES_TIPOORDEN', 'A2PLATPreordenes.VisorPreordenesView','OYD','Orden bolsa',
			  NULL, NULL, '', ''
	UNION ALL
	SELECT 1, 'VISORPREORDENES_TIPOORDEN', 'A2PLATPreordenes.VisorPreordenesView','ON','Otros Negocios',
			  NULL, NULL, '', ''
	UNION ALL
	SELECT 1, 'VISORPREORDENES_TIPOORDEN', 'A2PLATPreordenes.VisorPreordenesView','OYDPLUS','Ordenes OYDPLUS',
			  NULL, NULL, '', ''
	
	


	IF @plogCrearTablaTemporal = 1 --RABP2018
	BEGIN
		SELECT intID,
			strTopico,
			strOrigen,
			strRetorno,
			strDescripcion,
			intIDDependencia1,
			intIDDependencia2,
			strDependencia1,
			strDependencia2
		FROM #tmpCombosGenericoAplicacion ORDER BY strTopico ASC , intID ASC
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
