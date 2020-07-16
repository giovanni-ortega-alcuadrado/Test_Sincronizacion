------------------------------------------------------------------------------------------------------------------
-- Tabla:				[dbo][tblA2_MensajesSistema]
-- Desarrollado por:	Natalia Andrea Otalvaro
-- Descripción:			Inserción de configuración de combos para los maestros genericos
-- Fecha:				Enero 2019
------------------------------------------------------------------------------------------------------------------
DECLARE @lngIDComisionista		INT,
		@lngIDSucComisionista	INT,
		@intIDTipoSistema		INT

SELECT @lngIDComisionista=lngIDComisionista,
	@lngIDSucComisionista=lngIDSucComisionista
FROM tblInstalacion

select @intIDTipoSistema=intIdTipoMensajeSistema from PLATAFORMA.tblA2_MensajesSistemaTipos where strTipoMensaje='INCONSISTENCIA'

IF NOT EXISTS(SELECT 1 FROM PLATAFORMA.tblA2_MensajesSistema WHERE strCodMensaje='PREORDENES_PREORDENES_REQUERIDO_FECHAINVERSION')
BEGIN
	INSERT INTO PLATAFORMA.tblA2_MensajesSistema(
		intIdTipoMensajeSistema,
		strCodMensaje,
		strMensajePorDefecto,
		intIdProducto,
		strGrupo,
		strUsuario,
		dtmActualizacion
	)
	VALUES(@intIDTipoSistema,
		'PREORDENES_PREORDENES_REQUERIDO_FECHAINVERSION',
		'La Fecha inversión es un campo requerido.',
		NULL,
		'',
		'Alcuadrado',
		GETDATE())
END

IF NOT EXISTS(SELECT 1 FROM PLATAFORMA.tblA2_MensajesSistema WHERE strCodMensaje='PREORDENES_PREORDENES_REQUERIDO_FECHAVIGENCIA')
BEGIN
	INSERT INTO PLATAFORMA.tblA2_MensajesSistema(
		intIdTipoMensajeSistema,
		strCodMensaje,
		strMensajePorDefecto,
		intIdProducto,
		strGrupo,
		strUsuario,
		dtmActualizacion
	)
	VALUES(@intIDTipoSistema,
		'PREORDENES_PREORDENES_REQUERIDO_FECHAVIGENCIA',
		'La Fecha vigencia es un campo requerido.',
		NULL,
		'',
		'Alcuadrado',
		GETDATE())
END

IF NOT EXISTS(SELECT 1 FROM PLATAFORMA.tblA2_MensajesSistema WHERE strCodMensaje='PREORDENES_PREORDENES_FECHASINVALIDAS')
BEGIN
	INSERT INTO PLATAFORMA.tblA2_MensajesSistema(
		intIdTipoMensajeSistema,
		strCodMensaje,
		strMensajePorDefecto,
		intIdProducto,
		strGrupo,
		strUsuario,
		dtmActualizacion
	)
	VALUES(@intIDTipoSistema,
		'PREORDENES_PREORDENES_FECHASINVALIDAS',
		'La Fecha vigencia no puede ser menor a la Fecha inversión.',
		NULL,
		'',
		'Alcuadrado',
		GETDATE())
END

IF NOT EXISTS(SELECT 1 FROM PLATAFORMA.tblA2_MensajesSistema WHERE strCodMensaje='PREORDENES_PREORDENES_REQUERIDO_CUENTA')
BEGIN
	INSERT INTO PLATAFORMA.tblA2_MensajesSistema(
		intIdTipoMensajeSistema,
		strCodMensaje,
		strMensajePorDefecto,
		intIdProducto,
		strGrupo,
		strUsuario,
		dtmActualizacion
	)
	VALUES(@intIDTipoSistema,
		'PREORDENES_PREORDENES_REQUERIDO_CUENTA',
		'La Cuenta es un campo requerido.',
		NULL,
		'',
		'Alcuadrado',
		GETDATE())
END

IF NOT EXISTS(SELECT 1 FROM PLATAFORMA.tblA2_MensajesSistema WHERE strCodMensaje='PREORDENES_PREORDENES_NOEXISTE_CUENTA')
BEGIN
	INSERT INTO PLATAFORMA.tblA2_MensajesSistema(
		intIdTipoMensajeSistema,
		strCodMensaje,
		strMensajePorDefecto,
		intIdProducto,
		strGrupo,
		strUsuario,
		dtmActualizacion
	)
	VALUES(@intIDTipoSistema,
		'PREORDENES_PREORDENES_NOEXISTE_CUENTA',
		'La Cuenta debe estar Activo.',
		NULL,
		'',
		'Alcuadrado',
		GETDATE())
END

IF NOT EXISTS(SELECT 1 FROM PLATAFORMA.tblA2_MensajesSistema WHERE strCodMensaje='PREORDENES_PREORDENES_REQUERIDO_TIPOPREORDEN')
BEGIN
	INSERT INTO PLATAFORMA.tblA2_MensajesSistema(
		intIdTipoMensajeSistema,
		strCodMensaje,
		strMensajePorDefecto,
		intIdProducto,
		strGrupo,
		strUsuario,
		dtmActualizacion
	)
	VALUES(@intIDTipoSistema,
		'PREORDENES_PREORDENES_REQUERIDO_TIPOPREORDEN',
		'El Tipo es un campo requerido.',
		NULL,
		'',
		'Alcuadrado',
		GETDATE())
END

IF NOT EXISTS(SELECT 1 FROM PLATAFORMA.tblA2_MensajesSistema WHERE strCodMensaje='PREORDENES_PREORDENES_NOEXISTE_INSTRUMENTO')
BEGIN
	INSERT INTO PLATAFORMA.tblA2_MensajesSistema(
		intIdTipoMensajeSistema,
		strCodMensaje,
		strMensajePorDefecto,
		intIdProducto,
		strGrupo,
		strUsuario,
		dtmActualizacion
	)
	VALUES(@intIDTipoSistema,
		'PREORDENES_PREORDENES_NOEXISTE_INSTRUMENTO',
		'El Instrumento debe estar Activo.',
		NULL,
		'',
		'Alcuadrado',
		GETDATE())
END

IF NOT EXISTS(SELECT 1 FROM PLATAFORMA.tblA2_MensajesSistema WHERE strCodMensaje='PREORDENES_PREORDENES_NOEXISTE_ENTIDAD')
BEGIN
	INSERT INTO PLATAFORMA.tblA2_MensajesSistema(
		intIdTipoMensajeSistema,
		strCodMensaje,
		strMensajePorDefecto,
		intIdProducto,
		strGrupo,
		strUsuario,
		dtmActualizacion
	)
	VALUES(@intIDTipoSistema,
		'PREORDENES_PREORDENES_NOEXISTE_ENTIDAD',
		'La Entidad debe estar Activo.',
		NULL,
		'',
		'Alcuadrado',
		GETDATE())
END

IF NOT EXISTS(SELECT 1 FROM PLATAFORMA.tblA2_MensajesSistema WHERE strCodMensaje='PREORDENES_PREORDENES_REQUERIDO_INTENCION')
BEGIN
	INSERT INTO PLATAFORMA.tblA2_MensajesSistema(
		intIdTipoMensajeSistema,
		strCodMensaje,
		strMensajePorDefecto,
		intIdProducto,
		strGrupo,
		strUsuario,
		dtmActualizacion
	)
	VALUES(@intIDTipoSistema,
		'PREORDENES_PREORDENES_REQUERIDO_INTENCION',
		'La Intención es un campo requerido.',
		NULL,
		'',
		'Alcuadrado',
		GETDATE())
END

IF NOT EXISTS(SELECT 1 FROM PLATAFORMA.tblA2_MensajesSistema WHERE strCodMensaje='PREORDENES_PREORDENES_REQUERIDO_VALORECONOMICO')
BEGIN
	INSERT INTO PLATAFORMA.tblA2_MensajesSistema(
		intIdTipoMensajeSistema,
		strCodMensaje,
		strMensajePorDefecto,
		intIdProducto,
		strGrupo,
		strUsuario,
		dtmActualizacion
	)
	VALUES(@intIDTipoSistema,
		'PREORDENES_PREORDENES_REQUERIDO_VALORECONOMICO',
		'El Valor económico es un campo requerido.',
		NULL,
		'',
		'Alcuadrado',
		GETDATE())
END

IF NOT EXISTS(SELECT 1 FROM PLATAFORMA.tblA2_MensajesSistema WHERE strCodMensaje='PREORDENES_PREORDENES_REQUERIDO_VALORNOMINAL')
BEGIN
	INSERT INTO PLATAFORMA.tblA2_MensajesSistema(
		intIdTipoMensajeSistema,
		strCodMensaje,
		strMensajePorDefecto,
		intIdProducto,
		strGrupo,
		strUsuario,
		dtmActualizacion
	)
	VALUES(@intIDTipoSistema,
		'PREORDENES_PREORDENES_REQUERIDO_VALORNOMINAL',
		'El Valor nominal es un campo requerido.',
		NULL,
		'',
		'Alcuadrado',
		GETDATE())
END

IF NOT EXISTS(SELECT 1 FROM PLATAFORMA.tblA2_MensajesSistema WHERE strCodMensaje='PREORDENES_PREORDENES_REQUERIDO_VALOR')
BEGIN
	INSERT INTO PLATAFORMA.tblA2_MensajesSistema(
		intIdTipoMensajeSistema,
		strCodMensaje,
		strMensajePorDefecto,
		intIdProducto,
		strGrupo,
		strUsuario,
		dtmActualizacion
	)
	VALUES(@intIDTipoSistema,
		'PREORDENES_PREORDENES_REQUERIDO_VALOR',
		'El Valor es un campo requerido.',
		NULL,
		'',
		'Alcuadrado',
		GETDATE())
END

IF NOT EXISTS(SELECT 1 FROM PLATAFORMA.tblA2_MensajesSistema WHERE strCodMensaje='PREORDENES_PREORDENES_REQUERIDO_PRECIO')
BEGIN
	INSERT INTO PLATAFORMA.tblA2_MensajesSistema(
		intIdTipoMensajeSistema,
		strCodMensaje,
		strMensajePorDefecto,
		intIdProducto,
		strGrupo,
		strUsuario,
		dtmActualizacion
	)
	VALUES(@intIDTipoSistema,
		'PREORDENES_PREORDENES_REQUERIDO_PRECIO',
		'El Precio es un campo requerido.',
		NULL,
		'',
		'Alcuadrado',
		GETDATE())
END

IF NOT EXISTS(SELECT 1 FROM PLATAFORMA.tblA2_MensajesSistema WHERE strCodMensaje='PREORDENES_PREORDENES_REQUERIDO_RENTABILIDADMINIMA')
BEGIN
	INSERT INTO PLATAFORMA.tblA2_MensajesSistema(
		intIdTipoMensajeSistema,
		strCodMensaje,
		strMensajePorDefecto,
		intIdProducto,
		strGrupo,
		strUsuario,
		dtmActualizacion
	)
	VALUES(@intIDTipoSistema,
		'PREORDENES_PREORDENES_REQUERIDO_RENTABILIDADMINIMA',
		'La Rentabilidad mínima es un campo requerido.',
		NULL,
		'',
		'Alcuadrado',
		GETDATE())
END

IF NOT EXISTS(SELECT 1 FROM PLATAFORMA.tblA2_MensajesSistema WHERE strCodMensaje='PREORDENES_PREORDENES_REQUERIDO_RENTABILIDADMAXIMA')
BEGIN
	INSERT INTO PLATAFORMA.tblA2_MensajesSistema(
		intIdTipoMensajeSistema,
		strCodMensaje,
		strMensajePorDefecto,
		intIdProducto,
		strGrupo,
		strUsuario,
		dtmActualizacion
	)
	VALUES(@intIDTipoSistema,
		'PREORDENES_PREORDENES_REQUERIDO_RENTABILIDADMAXIMA',
		'La Rentabilidad máxima es un campo requerido.',
		NULL,
		'',
		'Alcuadrado',
		GETDATE())
END

IF NOT EXISTS(SELECT 1 FROM PLATAFORMA.tblA2_MensajesSistema WHERE strCodMensaje='PREORDENES_PREORDENES_REQUERIDO_PORTAFOLIO')
BEGIN
	INSERT INTO PLATAFORMA.tblA2_MensajesSistema(
		intIdTipoMensajeSistema,
		strCodMensaje,
		strMensajePorDefecto,
		intIdProducto,
		strGrupo,
		strUsuario,
		dtmActualizacion
	)
	VALUES(@intIDTipoSistema,
		'PREORDENES_PREORDENES_REQUERIDO_PORTAFOLIO',
		'Debe seleccionar un item del portafolio.',
		NULL,
		'',
		'Alcuadrado',
		GETDATE())
END

IF NOT EXISTS(SELECT 1 FROM PLATAFORMA.tblA2_MensajesSistema WHERE strCodMensaje='PREORDENES_PREORDENES_RENTABILIDADINVALIDA')
BEGIN
	INSERT INTO PLATAFORMA.tblA2_MensajesSistema(
		intIdTipoMensajeSistema,
		strCodMensaje,
		strMensajePorDefecto,
		intIdProducto,
		strGrupo,
		strUsuario,
		dtmActualizacion
	)
	VALUES(@intIDTipoSistema,
		'PREORDENES_PREORDENES_RENTABILIDADINVALIDA',
		'La Rentabiliad máxima no puede ser menor a la Rentabilidad mínima.',
		NULL,
		'',
		'Alcuadrado',
		GETDATE())
END

--VISOR DE PREORDENES
IF NOT EXISTS(SELECT 1 FROM PLATAFORMA.tblA2_MensajesSistema WHERE strCodMensaje='PREORDENES_VISOR_CRUCEINVALIDO')
BEGIN
	INSERT INTO PLATAFORMA.tblA2_MensajesSistema(
		intIdTipoMensajeSistema,
		strCodMensaje,
		strMensajePorDefecto,
		intIdProducto,
		strGrupo,
		strUsuario,
		dtmActualizacion
	)
	VALUES(@intIDTipoSistema,
		'PREORDENES_VISOR_CRUCEINVALIDO',
		'Uno de los registros ya no se encuentra activo o cambio el valor para realizar el cruce, se recargará la pantalla.',
		NULL,
		'',
		'Alcuadrado',
		GETDATE())
END

--PREORDENES
IF NOT EXISTS(SELECT 1 FROM PLATAFORMA.tblA2_MensajesSistema WHERE strCodMensaje='PREORDENES_PREORDENES_ORDENINACTIVA')
BEGIN
	INSERT INTO PLATAFORMA.tblA2_MensajesSistema(
		intIdTipoMensajeSistema,
		strCodMensaje,
		strMensajePorDefecto,
		intIdProducto,
		strGrupo,
		strUsuario,
		dtmActualizacion
	)
	VALUES(@intIDTipoSistema,
		'PREORDENES_PREORDENES_ORDENINACTIVA',
		'El registro ya no se encuentra activo, fue modificado por otro usuario.',
		NULL,
		'',
		'Alcuadrado',
		GETDATE())
END

--PREORDENES
IF NOT EXISTS(SELECT 1 FROM PLATAFORMA.tblA2_MensajesSistema WHERE strCodMensaje='PREORDENES_PREORDENES_ORDENNODISPONIBLE')
BEGIN
	INSERT INTO PLATAFORMA.tblA2_MensajesSistema(
		intIdTipoMensajeSistema,
		strCodMensaje,
		strMensajePorDefecto,
		intIdProducto,
		strGrupo,
		strUsuario,
		dtmActualizacion
	)
	VALUES(@intIDTipoSistema,
		'PREORDENES_PREORDENES_ORDENNODISPONIBLE',
		'El registro ya no se encuentra disponible para modificación, fue modificado por otro usuario.',
		NULL,
		'',
		'Alcuadrado',
		GETDATE())
END