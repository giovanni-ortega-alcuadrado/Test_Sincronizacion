------------------------------------------------------------------------------------------------------------------
-- Tabla:				[PREORDENES][tblPreOrdenes_Anuladas]
-- Desarrollado por:	Natalia Andrea Otalvaro Castrillon.
-- Descripción:			Script para crear la tabla tblPreOrdenes_Anuladas.
-- Fecha:				Febrero/2019.
------------------------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[PREORDENES].[tblPreOrdenes_Anuladas]') AND TYPE in (N'U'))
BEGIN
	CREATE TABLE [PREORDENES].[tblPreOrdenes_Anuladas](
		[intID] [INT] IDENTITY(1,1),
		[intIDPreOrden][INT] NOT NULL,
		[strOrigenAnulacion][VARCHAR](20) NOT NULL,
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
		[strUsuarioInsercion] [varchar] (60)  NOT NULL,
		[logEsParcial][INT] NULL,
		[intIDPreOrdenOrigen][INT] NULL,
		[dblValorPendiente][FLOAT] NULL,
		[intNroParcial][INT] NULL
	CONSTRAINT [PK_tblPreOrdenes_Anuladas] PRIMARY KEY CLUSTERED 
	(
		[intID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO