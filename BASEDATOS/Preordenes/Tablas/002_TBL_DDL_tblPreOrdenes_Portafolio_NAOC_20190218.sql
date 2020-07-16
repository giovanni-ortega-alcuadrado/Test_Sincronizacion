------------------------------------------------------------------------------------------------------------------
-- Tabla:				[PREORDENES][tblPreOrdenes_Portafolio]
-- Desarrollado por:	Natalia Andrea Otalvaro Castrillon.
-- Descripción:			Script para crear la tabla tblPreOrdenes_Portafolio.
-- Fecha:				Febrero/2019.
------------------------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[PREORDENES].[tblPreOrdenes_Portafolio]') AND TYPE in (N'U'))
BEGIN
	CREATE TABLE [PREORDENES].[tblPreOrdenes_Portafolio](
		[intID] [INT] IDENTITY(1,1),
		[intIDPreOrden][INT] NOT NULL,
		[lngIDRecibo][INT] NOT NULL,
		[lngSecuencia][INT] NOT NULL,
		[strNroTitulo][VARCHAR](30) NOT NULL,
		[strInstrumento][VARCHAR](15) NOT NULL,
		[dblTasaReferencia][FLOAT] NULL,
		[intIDEntidad][INT]NOT NULL,
		[dblValorNominal][FLOAT]NOT NULL,
		[dblValorCompra][FLOAT]NOT NULL,
		[dblVPNMercado][FLOAT]NOT NULL,
		[dtmFechaCompra][DATETIME]NOT NULL,
		[strUsuario] [varchar] (60)  NOT NULL,
		[dtmActualizacion] [datetime] NOT NULL,
		[strUsuarioInsercion] [varchar] (60)  NOT NULL
	CONSTRAINT [PK_tblPreOrdenes_Portafolio] PRIMARY KEY CLUSTERED 
	(
		[intID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = 'tblPreOrdenes_Portafolio' AND CONSTRAINT_NAME = 'FK_tblPreOrdenes_Portafolio_tblPreOrdenes' AND TABLE_SCHEMA = 'PREORDENES')
BEGIN
	ALTER TABLE [PREORDENES].tblPreOrdenes_Portafolio  DROP CONSTRAINT [FK_tblPreOrdenes_Portafolio_tblPreOrdenes]
END	
GO	
