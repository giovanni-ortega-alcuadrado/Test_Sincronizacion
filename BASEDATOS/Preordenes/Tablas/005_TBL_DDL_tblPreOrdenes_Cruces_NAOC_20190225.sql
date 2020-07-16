------------------------------------------------------------------------------------------------------------------
-- Tabla:				[PREORDENES][tblPreOrdenes_Cruces]
-- Desarrollado por:	Natalia Andrea Otalvaro Castrillon.
-- Descripción:			Script para crear la tabla tblPreOrdenes_Cruces.
-- Fecha:				Febrero/2019.
------------------------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[PREORDENES].[tblPreOrdenes_Cruces]') AND TYPE in (N'U'))
BEGIN
	CREATE TABLE [PREORDENES].[tblPreOrdenes_Cruces](
		[intID] [INT] IDENTITY(1,1),
		[guidCruce][UNIQUEIDENTIFIER]NOT NULL,
		[intIDPreOrdenCompra][INT]NOT NULL,
		[intIDPreOrdenVenta][INT]NOT NULL,
		[strTipoCrucePrincipal][VARCHAR](2) NOT NULL,	--INDICA EL TIPO CRUCE PRINCIPAL A:AMBAS, C:COMPRA, V:VENTA
		[dtmFechaCruce][DATETIME] NOT NULL,
		[dblValorCruzado][FLOAT] NOT NULL,
		[strUsuario] [varchar] (60)  NOT NULL,
		[dtmActualizacion] [datetime] NOT NULL
	CONSTRAINT [PK_tblPreOrdenes_Cruces] PRIMARY KEY CLUSTERED 
	(
		[intID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO
