Imports System.Configuration
Imports A2.OyD.Infraestructura
''' <summary>
''' Esta clase fue creada para poder extender los metodos new() de la clase oyddatacontext y poder descifrar la cadena de conexión.
''' Cuando se realizan cambios en el diseñador de consultas relacional (.dbml) los metodos new() vuelven a ser automáticamente generados y se presenta un error de que 
''' existen múltiples definiciones con firmas idénticas (‘Public Sub New()’ has multiple definitions with identical signatures).
''' LA SOLUCION es eliminar de la clase oyddatacontext todos los métodos New() y dejar los que se encuentran en esta clase.
''' </summary>
''' <remarks></remarks>


Partial Public Class OyDBolsaDatacontext
    Dim idOrden As Integer
    Dim idDetalleDocumento As Integer

    Public Sub New()
        MyBase.New(ObtenerCadenaConexion(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString), mappingSource)
        OnCreated()
    End Sub
    Public Overrides Sub SubmitChanges(failureMode As Data.Linq.ConflictMode)
        Try
            MyBase.SubmitChanges(failureMode)
        Catch ex As Exception
            ManejarError(ex, "OyDBolsaDatacontext", "SubmitChanges")
        End Try
    End Sub

    Private Sub InsertLiquidacione(ByVal obj As OyDBolsa.Liquidacione)
        Dim p1 As Integer = obj.IDLiquidaciones
        Dim p2 As Integer = obj.IDOrden
        Me.uspOyDNet_Liquidaciones_Actualizar(obj.ID, obj.Parcial, obj.Tipo, obj.ClaseOrden, obj.IDEspecie, obj.IDOrden, obj.Prefijo, obj.IDFactura, obj.Facturada, obj.IDComitente, obj.IDOrdenante, obj.IDBolsa, CType(obj.ValBolsa, System.Nullable(Of Double)), CType(obj.IDRueda, System.Nullable(Of Short)), CType(obj.TasaDescuento, System.Nullable(Of Double)), CType(obj.TasaCompraVende, System.Nullable(Of Double)), obj.Modalidad, obj.IndicadorEconomico, CType(obj.PuntosIndicador, System.Nullable(Of Double)), obj.Plazo, CType(obj.Liquidacion, System.Nullable(Of Date)), CType(obj.Cumplimiento, System.Nullable(Of Date)), CType(obj.Emision, System.Nullable(Of Date)), CType(obj.Vencimiento, System.Nullable(Of Date)), CType(obj.OtraPlaza, System.Nullable(Of Boolean)), obj.Plaza, obj.IDComisionistaLocal, obj.IDComisionistaOtraPlaza, obj.IDCiudadOtraPlaza, CType(obj.TasaEfectiva, System.Nullable(Of Double)), CType(obj.Cantidad, System.Nullable(Of Double)), CType(obj.Precio, System.Nullable(Of Double)), CType(obj.Transaccion_cur, System.Nullable(Of Double)), CType(obj.SubTotalLiq, System.Nullable(Of Double)), CType(obj.TotalLiq, System.Nullable(Of Double)), CType(obj.Comision, System.Nullable(Of Double)), CType(obj.Retencion, System.Nullable(Of Double)), CType(obj.Intereses, System.Nullable(Of Double)), CType(obj.ValorIva, System.Nullable(Of Double)), obj.DiasIntereses, CType(obj.FactorComisionPactada, System.Nullable(Of Double)), obj.Mercado, obj.NroTitulo, obj.IDCiudadExpTitulo, obj.PlazoOriginal, CType(obj.Aplazamiento, System.Nullable(Of Boolean)), CType(obj.VersionPapeleta, System.Nullable(Of Short)), CType(obj.EmisionOriginal, System.Nullable(Of Date)), CType(obj.VencimientoOriginal, System.Nullable(Of Date)), obj.Impresiones, obj.FormaPago, obj.CtrlImpPapeleta, obj.DiasVencimiento, obj.PosicionPropia, obj.Transaccion, obj.TipoOperacion, obj.DiasContado, CType(obj.Ordinaria, System.Nullable(Of Boolean)), obj.ObjetoOrdenExtraordinaria, obj.NumPadre, obj.ParcialPadre, CType(obj.OperacionPadre, System.Nullable(Of Date)), obj.DiasTramo, CType(obj.Vendido, System.Nullable(Of Boolean)), CType(obj.Vendido_fecha, System.Nullable(Of Date)), CType(obj.Manual, System.Nullable(Of Boolean)), CType(obj.ValorTraslado, System.Nullable(Of Double)), CType(obj.ValorBrutoCompraVencida, System.Nullable(Of Double)), obj.AutoRetenedor, obj.Sujeto, CType(obj.PcRenEfecCompraRet, System.Nullable(Of Double)), CType(obj.PcRenEfecVendeRet, System.Nullable(Of Double)), obj.Reinversion, obj.Swap, obj.NroSwap, obj.Certificacion, CType(obj.DescuentoAcumula, System.Nullable(Of Double)), CType(obj.PctRendimiento, System.Nullable(Of Double)), CType(obj.FechaCompraVencido, System.Nullable(Of Date)), CType(obj.PrecioCompraVencido, System.Nullable(Of Double)), obj.ConstanciaEnajenacion, obj.RepoTitulo, CType(obj.ServBolsaVble, System.Nullable(Of Double)), CType(obj.ServBolsaFijo, System.Nullable(Of Double)), obj.Traslado, obj.UBICACIONTITULO, obj.HoraGrabacion, obj.OrigenOperacion, obj.CodigoOperadorCompra, obj.CodigoOperadorVende, obj.IdentificacionRemate, obj.ModalidaOperacion, obj.IndicadorPrecio, obj.PeriodoExdividendo, obj.PlazoOperacionRepo, CType(obj.ValorCaptacionRepo, System.Nullable(Of Double)), CType(obj.VolumenCompraRepo, System.Nullable(Of Double)), CType(obj.PrecioNetoFraccion, System.Nullable(Of Double)), CType(obj.VolumenNetoFraccion, System.Nullable(Of Double)), obj.CodigoContactoComercial, obj.NroFraccionOperacion, obj.IdentificacionPatrimonio1, obj.TipoidentificacionCliente2, obj.NitCliente2, obj.IdentificacionPatrimonio2, obj.TipoIdentificacionCliente3, obj.NitCliente3, obj.IdentificacionPatrimonio3, obj.IndicadorOperacion, CType(obj.BaseRetencion, System.Nullable(Of Double)), CType(obj.PorcRetencion, System.Nullable(Of Double)), CType(obj.BaseRetencionTranslado, System.Nullable(Of Double)), CType(obj.PorcRetencionTranslado, System.Nullable(Of Double)), CType(obj.PorcIvaComision, System.Nullable(Of Double)), obj.IndicadorAcciones, obj.OperacionNegociada, CType(obj.FechaConstancia, System.Nullable(Of Date)), CType(obj.ValorConstancia, System.Nullable(Of Double)), obj.GeneraConstancia, CType(obj.Cargado, System.Nullable(Of Boolean)), obj.Usuario, CType(obj.CumplimientoTitulo, System.Nullable(Of Date)), obj.NroLote, CType(obj.ValorEntregaContraPago, System.Nullable(Of Double)), obj.AquienSeEnviaRetencion, obj.IDBaseDias, obj.TipoDeOferta, obj.NroLoteENC, CType(obj.ContabilidadENC, System.Nullable(Of Date)), p1, obj.CodigoIntermediario, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)), obj.ValorExtemporaneo, obj.PosicionExtemporaneo)
        obj.IDLiquidaciones = p1
        obj.IDOrden = p2
        idOrden = p2
    End Sub

    Private Sub InsertReceptoresOrdene(ByVal obj As OyDBolsa.ReceptoresOrdene)
        Dim p1 As Integer = obj.IDReceptoresOrdenes
        If idOrden = 0 Then
            idDetalleDocumento = obj.IDOrden
        Else
            idDetalleDocumento = idOrden
        End If       
        Me.uspOyDNet_ReceptoresOrdenes_Actualizar(obj.TipoOrden, obj.ClaseOrden, idDetalleDocumento, obj.Version, obj.IDReceptor,
                                                  CType(obj.Lider, System.Nullable(Of Boolean)),
                                                  CType(obj.Porcentaje, System.Nullable(Of Double)),
                                                  obj.Usuario, p1, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.IDReceptoresOrdenes = p1
    End Sub

    Private Sub UpdateReceptoresOrdene(ByVal obj As OyDBolsa.ReceptoresOrdene)
        Dim p1 As Integer = obj.IDReceptoresOrdenes
        Me.uspOyDNet_ReceptoresOrdenes_Actualizar(obj.TipoOrden, obj.ClaseOrden, obj.IDOrden, obj.Version, obj.IDReceptor, CType(obj.Lider, System.Nullable(Of Boolean)), CType(obj.Porcentaje, System.Nullable(Of Double)), obj.Usuario, p1, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.IDReceptoresOrdenes = p1
    End Sub

End Class











