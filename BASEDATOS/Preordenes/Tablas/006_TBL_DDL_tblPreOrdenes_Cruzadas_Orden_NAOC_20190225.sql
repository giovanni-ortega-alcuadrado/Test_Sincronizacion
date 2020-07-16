------------------------------------------------------------------------------------------------------------------
-- Tabla:				[PREORDENES][tblPreOrdenes_Cruzadas_Orden]
-- Desarrollado por:	Natalia Andrea Otalvaro Castrillon.
-- Descripción:			Script para crear la tabla tblPreOrdenes_Cruzadas_Orden.
-- Fecha:				Febrero/2019.
------------------------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[PREORDENES].[tblPreOrdenes_Cruzadas_Orden]') AND TYPE in (N'U'))
BEGIN
	CREATE TABLE [PREORDENES].[tblPreOrdenes_Cruzadas_Orden](
		[intID] [INT] IDENTITY(1,1),
		[intIDPreOrden_Cruzada][INT] NOT NULL,
		[strOrigenOrden][VARCHAR](20) NOT NULL,
		[intNroRegistro][INT] NOT NULL,
		[strClaseRegistro][VARCHAR](2) NOT NULL,
		[strTipoOperacionRegistro][VARCHAR](2) NOT NULL,
		[strTipoOrigenRegistro][VARCHAR](2) NOT NULL,
		[strUsuario] [varchar] (60)  NOT NULL,
		[dtmFechaCreacion] [datetime] NOT NULL,
		[dtmActualizacion] [datetime] NOT NULL,
		[strUsuarioInsercion] [varchar] (60)  NOT NULL
	CONSTRAINT [PK_tblPreOrdenes_Cruzadas_Orden] PRIMARY KEY CLUSTERED 
	(
		[intID] ASC
	)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE TABLE_NAME = 'tblPreOrdenes_Cruzadas_Orden' AND CONSTRAINT_NAME = 'FK_tblPreOrdenes_Cruzadas_Orden_tblPreOrdenes_Cruzadas' AND TABLE_SCHEMA = 'PREORDENES')
	BEGIN
		ALTER TABLE [PREORDENES].[tblPreOrdenes_Cruzadas_Orden]  WITH CHECK ADD  CONSTRAINT [FK_tblPreOrdenes_Cruzadas_Orden_tblPreOrdenes_Cruzadas] FOREIGN KEY([intIDPreOrden_Cruzada])
		REFERENCES [PREORDENES].[tblPreOrdenes_Cruzadas] ([intID])
	END	
GO
