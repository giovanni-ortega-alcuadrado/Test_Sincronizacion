------------------------------------------------------------------------------------------------------------------
-- Tabla:				[PREORDENES][tblPreOrdenes]
-- Desarrollado por:	Natalia Andrea Otalvaro Castrillon.
-- Descripción:			Script para crear la tabla tblPreOrdenes.
-- Fecha:				Febrero/2019.
------------------------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[PREORDENES].[tblPreOrdenes]') AND TYPE in (N'U'))
BEGIN
	CREATE TABLE [PREORDENES].[tblPreOrdenes](
		[intID] [INT] IDENTITY(1,1),
		[dtmFechaInversion][DATETIME] NOT NULL,
		[dtmFechaVigencia][DATETIME] NOT NULL,
		[intIDCodigoPersona][INT] NOT NULL,
		[strTipoPreOrden][VARCHAR](2) NOT NULL,
		[strTipoInversion][VARCHAR](2) NULL,
		[intIDEntidad][INT] NULL,
		[strInstrumento][VARCHAR](15) NULL,
		[strIntencion][VARCHAR](2) NOT NULL,
		[dblValor][FLOAT]NOT NULL,
		[dblPrecio][FLOAT]NOT NULL,
		[dblRentabilidadMinima][FLOAT]NOT NULL,
		[dblRentabilidadMaxima][FLOAT]NOT NULL,
		[strInstrucciones] [varchar] (500)  NULL,
		[logActivo][BIT] NOT NULL,
		[strObservaciones] [varchar] (500)  NULL,
		[strUsuario] [varchar] (60)  NOT NULL,
		[dtmFechaCreacion] [datetime] NOT NULL,
		[dtmActualizacion] [datetime] NOT NULL,
		[strUsuarioInsercion] [varchar] (60)  NOT NULL
	CONSTRAINT [PK_tblPreOrdenes] PRIMARY KEY CLUSTERED 
	(
		[intID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = 'tblPreOrdenes' AND CONSTRAINT_NAME = 'FK_tblPreOrdenes_tblCodigos' AND TABLE_SCHEMA = 'PREORDENES')
	BEGIN
		ALTER TABLE [PREORDENES].[tblPreOrdenes]  WITH CHECK ADD  CONSTRAINT [FK_tblPreOrdenes_tblCodigos] FOREIGN KEY([intIDCodigoPersona])
		REFERENCES [Personas].[tblCodigos] ([intID])
	END	
GO	

ALTER TABLE [PREORDENES].[tblPreOrdenes] CHECK CONSTRAINT [FK_tblPreOrdenes_tblCodigos]
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = 'tblPreOrdenes' AND CONSTRAINT_NAME = 'FK_tblPreOrdenes_tblEntidades' AND TABLE_SCHEMA = 'PREORDENES')
	BEGIN
		ALTER TABLE [PREORDENES].[tblPreOrdenes]  WITH CHECK ADD  CONSTRAINT [FK_tblPreOrdenes_tblEntidades] FOREIGN KEY([intIDEntidad])
		REFERENCES [CF].[tblEntidades] ([intIDEntidad])
	END	
GO	

ALTER TABLE [PREORDENES].[tblPreOrdenes] CHECK CONSTRAINT [FK_tblPreOrdenes_tblEntidades]
GO

--Crear la columna logEsParcial para la tabla tblPreOrdenes
IF NOT EXISTS (SELECT 1 
				FROM INFORMATION_SCHEMA.COLUMNS
				WHERE TABLE_NAME = 'tblPreOrdenes' AND TABLE_SCHEMA = 'PREORDENES' 
						AND COLUMN_NAME = 'logEsParcial')
BEGIN
	ALTER TABLE [PREORDENES].[tblPreOrdenes] ADD logEsParcial BIT NULL
END
GO

--Crear la columna intIDPreOrdenOrigen para la tabla tblPreOrdenes
IF NOT EXISTS (SELECT 1 
				FROM INFORMATION_SCHEMA.COLUMNS
				WHERE TABLE_NAME = 'tblPreOrdenes' AND TABLE_SCHEMA = 'PREORDENES' 
						AND COLUMN_NAME = 'intIDPreOrdenOrigen')
BEGIN
	ALTER TABLE [PREORDENES].[tblPreOrdenes] ADD intIDPreOrdenOrigen INT NULL
END
GO

--Crear la columna dblValorPendiente para la tabla tblPreOrdenes
IF NOT EXISTS (SELECT 1 
				FROM INFORMATION_SCHEMA.COLUMNS
				WHERE TABLE_NAME = 'tblPreOrdenes' AND TABLE_SCHEMA = 'PREORDENES' 
						AND COLUMN_NAME = 'dblValorPendiente')
BEGIN
	ALTER TABLE [PREORDENES].[tblPreOrdenes] ADD dblValorPendiente FLOAT NULL
END
GO

--Crear la columna intNroParcial para la tabla tblPreOrdenes
IF NOT EXISTS (SELECT 1 
				FROM INFORMATION_SCHEMA.COLUMNS
				WHERE TABLE_NAME = 'tblPreOrdenes' AND TABLE_SCHEMA = 'PREORDENES' 
						AND COLUMN_NAME = 'intNroParcial')
BEGIN
	ALTER TABLE [PREORDENES].[tblPreOrdenes] ADD intNroParcial INT NULL
END
GO

--Crear la columna intNroParcial para la tabla tblPreOrdenes
IF NOT EXISTS (SELECT 1 
				FROM INFORMATION_SCHEMA.COLUMNS
				WHERE TABLE_NAME = 'tblPreOrdenes' AND TABLE_SCHEMA = 'PREORDENES' 
						AND COLUMN_NAME = 'strOrigenAnulacion')
BEGIN
	ALTER TABLE [PREORDENES].[tblPreOrdenes] ADD strOrigenAnulacion VARCHAR(20) NULL
END
GO
