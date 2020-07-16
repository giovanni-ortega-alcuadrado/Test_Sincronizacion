------------------------------------------------------------------------------------------------------------------
-- Tabla:				[dbo][tblLista]
-- Desarrollado por:	Natalia Andrea Otalvaro
-- Descripción:			Inserción de configuración de combos para los maestros genericos
-- Fecha:				Enero 2019
------------------------------------------------------------------------------------------------------------------
DECLARE @lngIDComisionista		INT,
		@lngIDSucComisionista	INT

SELECT @lngIDComisionista=lngIDComisionista,
	@lngIDSucComisionista=lngIDSucComisionista
FROM tblInstalacion
--INSERTA LA CONFIGURACIÓN DE LISTAS
IF NOT EXISTS(SELECT 1 FROM PLATAFORMA.tblA2_ConfiguracionModulos WHERE strNombreModulo='PREORDENES')
BEGIN
	INSERT INTO PLATAFORMA.tblA2_ConfiguracionModulos(
		strNombreModulo,
		strProcedimiento,
		strUsuario,
		dtmActualizacion
	)
	VALUES('PREORDENES',
		'[PREORDENES].[usp_Procesos_Utilitarios_CargarCombos]',
		'A2',
		GETDATE())
END