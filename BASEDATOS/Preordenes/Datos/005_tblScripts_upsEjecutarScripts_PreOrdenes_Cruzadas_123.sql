-----------------------------------------------------------------------------------------------------------------
-- Descripción:			Script de insercion en las tablas de CF.tblScripts y CF.tblScriptsParametros (Log multiples cancelaciones) 
-- Fecha:				Abril 12 de 2019
--
-- Desarrollado por:	Juan Camilo Munera - JCM20190412
-----------------------------------------------------------------------------------------------------------------

DECLARE @lngIdScript INT

SELECT @lngIdScript = intIdScript
FROM CF.tblScripts 
WHERE strNombre = 'PreOrdenes cruzadas Consultar'
	AND strGrupo = 'REPORTES_PREORDENES'

IF NOT EXISTS(SELECT * FROM CF.tblScripts
				WHERE strNombre = 'PreOrdenes cruzadas Consultar'
					AND strGrupo = 'REPORTES_PREORDENES')

BEGIN
	INSERT INTO CF.tblScripts(intIdCompania,--1
	strGrupo,--2
	strNombre,--3
	strProcedimientoAlmacenado,--4
	strDescripcion,--5
	strTipoProceso,--6
	strTipoResultado,--7
	strSeparador,--8
	strCamposOrdenamiento,--9
	strNombreArchivo,--10
	strRutaArchivo,--10
	logCopiarArchivo,--12
	strParametroMulticompania,--13
	logUsarSufijoFechaHora,--14
	strUsuario,--15
	strMaquina,--16
	dtmActualizacion)--17
	SELECT 
	null,--1
	'REPORTES_PREORDENES',--2
	'PreOrdenes cruzadas Consultar',--3
	'[PREORDENES].[PREORDENES.uspEjecutarScripts_PreOrdenes_Cruzadas]',--4
	'Script para consultar la información de las PreOrdenes registradas en el sistema. ',--5
	'SINCRONICO',--6
	'EXCEL',--7
	'',--8
	'',--9
	'',--10
	'',--11
	0,--12
	'',--13
	0,--14
	'Alcuadrado',--15
	'Alcuadrado',--16
	getdate()--17

	SET @lngIdScript = SCOPE_IDENTITY()
END


DELETE FROM CF.tblScriptsParametros WHERE intIdScript = @lngIdScript


INSERT INTO cf.tblScriptsParametros(intIdScript,strParametro,strEtiqueta,strDescripcion,strTipoDato,intLongitudMaxima,strTipoFuenteDatos,strFuenteDatos,strSeparadorFuenteDatos,strTipoValorPorDefecto,strValorPorDefecto,logRequerido,logOculto,strUsuario,dtmActualizacion)
SELECT @lngIdScript,'@pdtmFechaInicial','Fecha inicial','Fecha inicial del movimiento','TIPO_DATO_FECHA',0.000000,'TIPO_FUENTE_DATOS_FECHA','','','TIPO_VALOR_DEFAULT_FUNCION','',1,0,'Alcuadrado',GETDATE()
UNION ALL
SELECT @lngIdScript,'@pdtmFechaFinal','Fecha Final','Fecha Final del movimiento','TIPO_DATO_FECHA',0.000000,'TIPO_FUENTE_DATOS_FECHA','','','TIPO_VALOR_DEFAULT_FUNCION','',1,0,'Alcuadrado',GETDATE()
UNION ALL
SELECT @lngIdScript,'@pstrIDComitente','Comitente','Comitente','TIPO_DATO_TEXTO',17.000000,'TIPO_FUENTE_DATOS_VALOR_FIJO','','','TIPO_VALOR_DEFAULT_FIJO','',0,0,'Alcuadrado',GETDATE()
UNION ALL
SELECT @lngIdScript,'@pstrUsuario', 'Usuario','Usuario que ejecuta la consulta','TIPO_DATO_TEXTO',60.000000,'TIPO_FUENTE_DATOS_VALOR_FIJO','','','TIPO_VALOR_DEFAULT_PARAMETRO','@pstrUsuario',0,1,'Alcuadrado',GETDATE()		