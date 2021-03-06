IF not exists (Select * From sys.sysobjects Where Id = object_id('[PREORDENES].[usp_PreOrdenes_Cliente_ConsultarPortafolio]', 'P'))
BEGIN
	EXECUTE('Create Procedure [PREORDENES].[usp_PreOrdenes_Cliente_ConsultarPortafolio] As return(0)')
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*------------------------------------------------------------------------------------------------------------------

Procedimiento [PREORDENES].[usp_PreOrdenes_Cliente_ConsultarPortafolio]
 
Descripción: Consulta los registros que cumplan con todos los criterios indicados en los parámetros de búsqueda.

Desarrollado por: Natalia Andrea Otalvaro
Fecha: Enero 24/2019

Ejemplo:	exec [PREORDENES].[usp_PreOrdenes_Cliente_ConsultarPortafolio] @pstrIDComitente=123,@pstrInstrumento='ECOPETROL', @pstrUsuario = 'alcudrado'

*/

ALTER PROCEDURE [PREORDENES].[usp_PreOrdenes_Cliente_ConsultarPortafolio]
@pstrIDComitente		VARCHAR(17),
@pstrTipoInversion	VARCHAR(2),
@pstrInstrumento		VARCHAR(15),
@pstrUsuario			varchar(60), -- Usuario que ejecuta la acción
@pstrInfosesion			varchar(1000) = Null, -- XML con inofrmación que se utiliza para auditoria (usuario, ip máquina usuario, nombre máquina usuario, servidor web, etc.)
@pstrInfosesionAdicional varchar(1000) = Null
--WITH ENCRYPTION
AS 

SET NOCOUNT ON

-- Declaraciones generales requeridas por el manejo de error y auditoria
DECLARE @strParametros VARCHAR(2000), @intNroRegistros INT, @strProcedimiento VARCHAR(100), @intIdAuditoria INT, @strOpcion VARCHAR(255), @strProceso VARCHAR(100), @strObjeto VARCHAR(100), @strMsg VARCHAR(2000)

-----------------------------------------------------------------------------------------------------------------
-- Declaraciones propias del procedimiento
-----------------------------------------------------------------------------------------------------------------
DECLARE @dtmFechaValoracionCliente	DATETIME

DECLARE @tmpPreOrdenes_Portafolio	TABLE(
		[intID] [INT] IDENTITY(1,1),
		[lngIDRecibo][INT] NULL,
		[lngSecuencia][INT] NULL,
		[strNroTitulo][VARCHAR](30) NULL,
		[strInstrumento][VARCHAR](15) NULL,
		[strDescripcionInstrumento][VARCHAR](200) NULL,
		[dblTasaReferencia][FLOAT] NULL,
		[intIDEntidad][INT] NULL,
		[strNroDocumentoEntidad][VARCHAR](15) NULL,
		[strNombreEntidad][VARCHAR](200) NULL,
		[dblValorNominal][FLOAT] NULL,
		[dblValorCompra][FLOAT] NULL,
		[dblVPNMercado][FLOAT] NULL,
		[dtmFechaCompra][DATETIME] NULL,
		[strTipoEspecie][VARCHAR](20)NULL
)

BEGIN TRY
	-- Concatenar todos los parámetros para guardarlos en el log de uso
	set @strParametros = 
						'@pstrIDComitente=' + isnull('''' + CONVERT(VARCHAR,@pstrIDComitente) + '''', 'null') + 
						',@pstrTipoInversion=' + isnull('''' + @pstrTipoInversion + '''', 'null') + 
						',@pstrInstrumento=' + isnull('''' + @pstrInstrumento + '''', 'null') + 
						',@pstrUsuario=' + isnull('''' + @pstrUsuario + '''', 'null') + 
						',@pstrInfosesion=' + isnull('''' + @pstrInfosesion + '''', 'null')
	-----------------------------------------------------------------------------------------------------------------
	-- Inicializar auditoria de uso
	SELECT @strOpcion = 'PreOrdenes', @strProceso= 'Consultar', @strProcedimiento = OBJECT_NAME(@@PROCID), @strObjeto = '', @pstrUsuario=ISNULL(@pstrUsuario,'') 	-- Asignar valor a los parámetros para identificar la opción ejecutada
	EXEC @intIdAuditoria = PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pstrOpcion= @strOpcion, @pstrProceso = @strProceso, @pstrObjeto = @strObjeto, @pstrProcedimiento = @strProcedimiento, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrInfosesion = @pstrInfosesion

	--------------------------------------------------------------------------------------------------------------------------------------------------------------------

	-----------------------------------------------------------------------------------------------------------------
	-- Funcionalidad propia del procedimiento
	
	
	--OBTIENE LA FECHA DE VALORACIÓN
	SELECT TOP 1 @dtmFechaValoracionCliente=dtmFechaCierrePortafolio 
	FROM tblClientes
	WHERE lngID=@pstrIDComitente

	SET @pstrTipoInversion=ISNULL(@pstrTipoInversion,'')
	SET @pstrInstrumento=ISNULL(@pstrInstrumento,'')

	INSERT INTO @tmpPreOrdenes_Portafolio(
		lngIDRecibo,lngSecuencia,strNroTitulo,strInstrumento,dblTasaReferencia,strNroDocumentoEntidad,strNombreEntidad,
		dblValorNominal,dblValorCompra,dblVPNMercado,dtmFechaCompra,strTipoEspecie
	)
	SELECT NroRecibo,Secuencia,strNroTitulo,Nemotecnico,dblTasaRetencion,NitEntidad,Entidad,Nominal,ValorCompra,VPN,FechaCompra,TipoEspecie
	FROM CF.tblInformePortafolioTemp
	WHERE Fecha=@dtmFechaValoracionCliente
		AND Portafolio=@pstrIDComitente
		AND TipoInversion='Negociable'
		AND (TipoEspecie=CASE WHEN @pstrTipoInversion='C' THEN 'Renta Fija' ELSE 'Renta variable' END OR @pstrTipoInversion='')
		AND (Nemotecnico=@pstrInstrumento OR @pstrInstrumento='')
	
	UPDATE	P
	SET P.intIDEntidad=E.intIDEntidad
	FROM @tmpPreOrdenes_Portafolio P
	INNER JOIN CF.tblEntidades E ON E.strNroDocumento=P.strNroDocumentoEntidad

	UPDATE	P
	SET P.strDescripcionInstrumento=E.strNombre
	FROM @tmpPreOrdenes_Portafolio P
	INNER JOIN dbo.tblEspecies E ON E.strId=P.strInstrumento

	SELECT intID,
		CONVERT(BIT, 0) AS logSeleccionado,
		lngIDRecibo,
		lngSecuencia,
		strNroTitulo,
		strTipoEspecie,
		strInstrumento,
		strDescripcionInstrumento,
		dblTasaReferencia,
		intIDEntidad,
		strNroDocumentoEntidad,
		strNombreEntidad,
		dblValorNominal,
		dblValorCompra,
		dblVPNMercado,
		dtmFechaCompra
	FROM @tmpPreOrdenes_Portafolio
	-----------------------------------------------------------------------------------------------------------------
	
	SET @intNroRegistros = @@ROWCOUNT

	-- Cerrar registro de auditoria
	EXEC PLATAFORMA.uspA2_Util_CrearLogAuditoriaUso @pintLogAuditoriaUso = @intIdAuditoria, @pintNroRegistros = @intNroRegistros
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
	SET @strMsg = PLATAFORMA.ufnA2_Mensajes_obtenerMsgError('PLATAFORMA_ERROR_CONSULTAR', 'PreOrdenes', '', '') -- CCM201711
	EXEC PLATAFORMA.uspA2_Util_CrearLogErroresSQL @pstrProceso = @@ProcId, @pstrMsgPersonalizado = @strMsg, @pstrParametros = @strParametros, @pstrUsuario = @pstrUsuario, @pstrMaquina = NULL, @pstrInfosesion = @pstrInfosesion, @plogLanzarError = 1
END CATCH
