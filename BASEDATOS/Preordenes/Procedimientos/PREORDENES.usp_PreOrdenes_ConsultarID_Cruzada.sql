IF not exists (Select * From sys.sysobjects Where Id = object_id('[PREORDENES].[usp_PreOrdenes_ConsultarID_Cruzada]', 'P'))
BEGIN
	EXECUTE('Create Procedure [PREORDENES].[usp_PreOrdenes_ConsultarID_Cruzada] As return(0)')
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*-----------------------------------------------------------------------------------------------------------------

Procedimiento [PREORDENES].[usp_PreOrdenes_ConsultarID_Cruzada]
 
Descripción: Consulta el registro que coincida en su clave principal con el Id enviado como parámetro. 
			Aplica para la tabla de PREORDENES.tblPreOrdenes_Cruzadas

Desarrollado por: Natalia Andrea Otalvaro
Fecha: Enero 24/2019

Ejemplo: 
[PREORDENES].[usp_PreOrdenes_ConsultarID_Cruzada] @pintID=1, @pstrTipoOperacion='C', @pstrUsuario = 'natalia.otalvaro', @pstrInfosesion = ''
[PREORDENES].[usp_PreOrdenes_ConsultarID_Cruzada] @pintID=1, @pstrTipoOperacion='V', @pstrUsuario = 'natalia.otalvaro', @pstrInfosesion = ''
*/

ALTER PROCEDURE [PREORDENES].[usp_PreOrdenes_ConsultarID_Cruzada]
@pintID				INT, -- Clave primaria de la tabla. De acuerdo con el estándar toda tabla debe tener un Identity que es la clave primaria.
@pstrTipoOperacion	VARCHAR(60), -- Usuario que ejecuta la acción
@pstrUsuario		VARCHAR(60), -- Usuario que ejecuta la acción
@pstrInfosesion		VARCHAR(1000) = Null -- XML con inofrmación que se utiliza para auditoria (usuario, ip máquina usuario, nombre máquina usuario, servidor web, etc.)
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
						',@pstrTipoOperacion=' + ISNULL('''' + @pstrTipoOperacion + '''', 'null') + 
						',@pstrUsuario=' + ISNULL('''' + @pstrUsuario + '''', 'null') + 
						',@pstrInfosesion=' + ISNULL('''' + @pstrInfosesion + '''', 'null')
	-----------------------------------------------------------------------------------------------------------------
	-- Inicializar auditoria de uso
	SELECT @strOpcion = 'PreOrdenes', @strProceso= 'ConsultarID', @strProcedimiento = OBJECT_NAME(@@PROCID), @strObjeto = '', @pstrUsuario=ISNULL(@pstrUsuario,'') 	-- Asignar valor a los parámetros para identificar la opción ejecutada
	EXEC @intIdAuditoria = PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pstrOpcion= @strOpcion, @pstrProceso = @strProceso, @pstrObjeto = @strObjeto, @pstrProcedimiento = @strProcedimiento, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrInfosesion = @pstrInfosesion
	
	-----------------------------------------------------------------------------------------------------------------
	-- Funcionalidad propia del procedimiento
	SELECT PRE.intID
		,PRE.guidCruce
		,PRE.intIDPreOrden
		,PRE.logEsParcial
		,PRE.intNroParcial
		,PRE.dblValorInicial
		,PRE.dblValorAnterior
		,PRE.dblValor
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
		,PERSONA.intID AS intIDPersona
		,COMITENTE.intIDComitente
		,PERSONA.strNroDocumento
		,PERSONA.strNombre
		,CCLIENTE.strTipoProducto
		,CCLIENTERECEPTOR.strIDReceptor
	FROM [PREORDENES].[tblPreOrdenes_Cruzadas] PRE
	INNER JOIN [PREORDENES].[tblPreOrdenes_Cruces] CRUCE ON CRUCE.guidCruce=PRE.guidCruce
															AND PRE.intIDPreOrden=CASE WHEN @pstrTipoOperacion='C' THEN CRUCE.intIDPreOrdenCompra ELSE CRUCE.intIDPreOrdenVenta END
	INNER JOIN [Personas].tblCodigos COMITENTE ON COMITENTE.intID=PRE.intIDCodigoPersona
	INNER JOIN [Personas].tblPersonas PERSONA ON PERSONA.intID=COMITENTE.intIDPersona
	INNER JOIN [dbo].[tblClientes] CCLIENTE ON CCLIENTE.lngID=COMITENTE.intIDComitente
	INNER JOIN [dbo].[tblClientesReceptores] CCLIENTERECEPTOR ON CCLIENTERECEPTOR.lngIDComitente=CCLIENTE.lngID
																AND CCLIENTERECEPTOR.logLider=1
	WHERE CRUCE.intID=@pintID
		AND PRE.strTipoPreOrden=@pstrTipoOperacion

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
