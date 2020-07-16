------------------------------------------------------------------------------------------------------------------
-- Tabla:				[PREORDENES][tblPreOrdenes_Cruzadas]
-- Desarrollado por:	Natalia Andrea Otalvaro Castrillon.
-- Descripción:			Script para crear la tabla tblPreOrdenes_Cruzadas.
-- Fecha:				Febrero/2019.
------------------------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[PREORDENES].[tblPreOrdenes_Cruzadas]') AND TYPE in (N'U'))
BEGIN
	CREATE TABLE [PREORDENES].[tblPreOrdenes_Cruzadas](
		[intID] [INT] IDENTITY(1,1),
		[guidCruce][UNIQUEIDENTIFIER]NOT NULL,
		[intIDPreOrden][INT] NOT NULL,
		[logEsParcial][BIT] NOT NULL,
		[intNroParcial][INT] NOT NULL,
		[dblValorInicial][FLOAT]NOT NULL,
		[dblValorAnterior][FLOAT]NOT NULL,
		[dblValor][FLOAT]NOT NULL,
		[dtmFechaInversion][DATETIME] NOT NULL,
		[dtmFechaVigencia][DATETIME] NOT NULL,
		[intIDCodigoPersona][INT] NOT NULL,
		[strTipoPreOrden][VARCHAR](2) NOT NULL,
		[strTipoInversion][VARCHAR](2) NULL,
		[intIDEntidad][INT] NULL,
		[strInstrumento][VARCHAR](15) NULL,
		[strIntencion][VARCHAR](2) NOT NULL,
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
	CONSTRAINT [PK_tblPreOrdenes_Cruzadas] PRIMARY KEY CLUSTERED 
	(
		[intID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = 'tblPreOrdenes_Cruzadas' AND CONSTRAINT_NAME = 'FK_tblPreOrdenes_Cruzadas_tblCodigos' AND TABLE_SCHEMA = 'PREORDENES')
	BEGIN
		ALTER TABLE [PREORDENES].[tblPreOrdenes_Cruzadas]  WITH CHECK ADD  CONSTRAINT [FK_tblPreOrdenes_Cruzadas_tblCodigos] FOREIGN KEY([intIDCodigoPersona])
		REFERENCES [Personas].[tblCodigos] ([intID])
	END	
GO	

ALTER TABLE [PREORDENES].[tblPreOrdenes_Cruzadas] CHECK CONSTRAINT [FK_tblPreOrdenes_Cruzadas_tblCodigos]
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = 'tblPreOrdenes_Cruzadas' AND CONSTRAINT_NAME = 'FK_tblPreOrdenes_Cruzadas_tblEntidades' AND TABLE_SCHEMA = 'PREORDENES')
	BEGIN
		ALTER TABLE [PREORDENES].[tblPreOrdenes_Cruzadas]  WITH CHECK ADD  CONSTRAINT [FK_tblPreOrdenes_Cruzadas_tblEntidades] FOREIGN KEY([intIDEntidad])
		REFERENCES [CF].[tblEntidades] ([intIDEntidad])
	END	
GO	

ALTER TABLE [PREORDENES].[tblPreOrdenes_Cruzadas] CHECK CONSTRAINT [FK_tblPreOrdenes_Cruzadas_tblEntidades]
GO
