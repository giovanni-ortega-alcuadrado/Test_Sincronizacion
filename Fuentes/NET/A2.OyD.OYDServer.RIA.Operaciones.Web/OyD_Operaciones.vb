Imports System.Data.SqlClient
Imports System.Data.Linq
Imports System.Reflection
Imports A2.OyD.Infraestructura

Partial Public Class OyDOperacionesDatacontext
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
            ManejarError(ex, "OyDOperacionesDatacontext", "SubmitChanges")
        End Try
    End Sub

    Private Sub InsertOperaciones(ByVal obj As OyDOperaciones.Operaciones)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDLiquidaciones
        Dim p2 As String = obj.strMsgValidacion
        Me.uspOyDNet_Operaciones_Actualizar(p1, CType(obj.lngID, System.Nullable(Of Integer)), CType(obj.lngParcial, System.Nullable(Of Integer)), obj.strTipo, obj.strClaseOrden, obj.strIDEspecie, CType(obj.lngIDOrden, System.Nullable(Of Integer)), obj.strPrefijo, obj.lngIDFactura, obj.strFacturada, obj.lngIDComitente, obj.lngIDOrdenante, CType(obj.lngIDBolsa, System.Nullable(Of Integer)), CType(obj.dblValBolsa, System.Nullable(Of Double)), CType(obj.bytIDRueda, System.Nullable(Of Short)), CType(obj.dblTasaDescuento, System.Nullable(Of Double)), CType(obj.dblTasaCompraVende, System.Nullable(Of Double)), obj.strModalidad, obj.strIndicadorEconomico, CType(obj.dblPuntosIndicador, System.Nullable(Of Double)), CType(obj.lngPlazo, System.Nullable(Of Integer)), CType(obj.dtmLiquidacion, System.Nullable(Of Date)), CType(obj.dtmCumplimiento, System.Nullable(Of Date)), CType(obj.dtmEmision, System.Nullable(Of Date)), CType(obj.dtmVencimiento, System.Nullable(Of Date)), obj.logOtraPlaza, obj.strPlaza, obj.lngIDComisionistaLocal, obj.lngIDComisionistaOtraPlaza, CType(obj.lngIDCiudadOtraPlaza, System.Nullable(Of Integer)), CType(obj.dblTasaEfectiva, System.Nullable(Of Double)), CType(obj.dblCantidad, System.Nullable(Of Double)), CType(obj.curPrecio, System.Nullable(Of Double)), CType(obj.curTransaccion, System.Nullable(Of Double)), CType(obj.curSubTotalLiq, System.Nullable(Of Double)), CType(obj.curTotalLiq, System.Nullable(Of Double)), CType(obj.curComision, System.Nullable(Of Double)), CType(obj.curRetencion, System.Nullable(Of Double)), CType(obj.curIntereses, System.Nullable(Of Double)), CType(obj.curValorIva, System.Nullable(Of Double)), CType(obj.lngDiasIntereses, System.Nullable(Of Integer)), CType(obj.dblFactorComisionPactada, System.Nullable(Of Double)), obj.strMercado, obj.strNroTitulo, CType(obj.lngIDCiudadExpTitulo, System.Nullable(Of Integer)), CType(obj.lngPlazoOriginal, System.Nullable(Of Integer)), obj.logAplazamiento, CType(obj.bytVersionPapeleta, System.Nullable(Of Short)), CType(obj.dtmEmisionOriginal, System.Nullable(Of Date)), CType(obj.dtmVencimientoOriginal, System.Nullable(Of Date)), CType(obj.lngImpresiones, System.Nullable(Of Integer)), obj.strFormaPago, CType(obj.lngCtrlImpPapeleta, System.Nullable(Of Integer)), CType(obj.lngDiasVencimiento, System.Nullable(Of Integer)), obj.strPosicionPropia, obj.strTransaccion, obj.strTipoOperacion, CType(obj.lngDiasContado, System.Nullable(Of Integer)), obj.logOrdinaria, obj.strObjetoOrdenExtraordinaria, CType(obj.lngNumPadre, System.Nullable(Of Integer)), obj.lngParcialPadre, CType(obj.dtmOperacionPadre, System.Nullable(Of Date)), CType(obj.lngDiasTramo, System.Nullable(Of Integer)), obj.logVendido, CType(obj.dtmVendido, System.Nullable(Of Date)), obj.logManual, CType(obj.dblValorTraslado, System.Nullable(Of Double)), CType(obj.dblValorBrutoCompraVencida, System.Nullable(Of Double)), obj.strAutoRetenedor, obj.strSujeto, CType(obj.dblPcRenEfecCompraRet, System.Nullable(Of Double)), CType(obj.dblPcRenEfecVendeRet, System.Nullable(Of Double)), obj.strReinversion, obj.strSwap, CType(obj.lngNroSwap, System.Nullable(Of Integer)), obj.strCertificacion, CType(obj.dblDescuentoAcumula, System.Nullable(Of Double)), CType(obj.dblPctRendimiento, System.Nullable(Of Double)), CType(obj.dtmFechaCompraVencido, System.Nullable(Of Date)), CType(obj.dblPrecioCompraVencido, System.Nullable(Of Double)), obj.strConstanciaEnajenacion, obj.strRepoTitulo, CType(obj.dblServBolsaVble, System.Nullable(Of Double)), CType(obj.dblServBolsaFijo, System.Nullable(Of Double)), CType(Nothing, String), obj.strUBICACIONTITULO, obj.strHoraGrabacion, obj.strOrigenOperacion, CType(obj.lngCodigoOperadorCompra, System.Nullable(Of Integer)), CType(obj.lngCodigoOperadorVende, System.Nullable(Of Integer)), obj.strIdentificacionRemate, obj.strModalidaOperacion, obj.strIndicadorPrecio, obj.strPeriodoExdividendo, CType(obj.lngPlazoOperacionRepo, System.Nullable(Of Integer)), CType(obj.dblValorCaptacionRepo, System.Nullable(Of Double)), CType(obj.lngVolumenCompraRepo, System.Nullable(Of Double)), CType(obj.dblPrecioNetoFraccion, System.Nullable(Of Double)), CType(obj.lngVolumenNetoFraccion, System.Nullable(Of Double)), obj.lngCodigoContactoComercial, CType(obj.lngNroFraccionOperacion, System.Nullable(Of Integer)), obj.strIdentificacionPatrimonio1, obj.strTipoidentificacionCliente2, obj.strNitCliente2, obj.strIdentificacionPatrimonio2, obj.strTipoIdentificacionCliente3, obj.strNitCliente3, obj.strIdentificacionPatrimonio3, obj.strIndicadorOperacion, CType(obj.dblBaseRetencion, System.Nullable(Of Double)), CType(obj.dblPorcRetencion, System.Nullable(Of Double)), CType(obj.dblBaseRetencionTranslado, System.Nullable(Of Double)), CType(obj.dblPorcRetencionTranslado, System.Nullable(Of Double)), CType(obj.dblPorcIvaComision, System.Nullable(Of Double)), obj.strIndicadorAcciones, obj.strOperacionNegociada, CType(obj.dtmFechaConstancia, System.Nullable(Of Date)), CType(obj.dblValorConstancia, System.Nullable(Of Double)), obj.strGeneraConstancia, obj.logCargado, CType(obj.dtmCumplimientoTitulo, System.Nullable(Of Date)), CType(obj.intNroLote, System.Nullable(Of Integer)), CType(obj.dblValorEntregaContraPago, System.Nullable(Of Double)), obj.strAquienSeEnviaRetencion, obj.strIDBaseDias, obj.strTipoDeOferta, CType(obj.intNroLoteENC, System.Nullable(Of Integer)), CType(obj.dtmContabilidadENC, System.Nullable(Of Date)), obj.strCodigoIntermediario, CType(obj.curValorExtemporaneo, System.Nullable(Of Double)), obj.strPosicionExtemporaneo, obj.strUsuario, obj.strInfoSesion, CType(Nothing, System.Nullable(Of Byte)), p2)
        obj.intIDLiquidaciones = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub UpdateOperaciones(ByVal obj As OyDOperaciones.Operaciones)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDLiquidaciones
        Dim p2 As String = obj.strMsgValidacion
        Me.uspOyDNet_Operaciones_Actualizar(p1, CType(obj.lngID, System.Nullable(Of Integer)), CType(obj.lngParcial, System.Nullable(Of Integer)), obj.strTipo, obj.strClaseOrden, obj.strIDEspecie, CType(obj.lngIDOrden, System.Nullable(Of Integer)), obj.strPrefijo, obj.lngIDFactura, obj.strFacturada, obj.lngIDComitente, obj.lngIDOrdenante, CType(obj.lngIDBolsa, System.Nullable(Of Integer)), CType(obj.dblValBolsa, System.Nullable(Of Double)), CType(obj.bytIDRueda, System.Nullable(Of Short)), CType(obj.dblTasaDescuento, System.Nullable(Of Double)), CType(obj.dblTasaCompraVende, System.Nullable(Of Double)), obj.strModalidad, obj.strIndicadorEconomico, CType(obj.dblPuntosIndicador, System.Nullable(Of Double)), CType(obj.lngPlazo, System.Nullable(Of Integer)), CType(obj.dtmLiquidacion, System.Nullable(Of Date)), CType(obj.dtmCumplimiento, System.Nullable(Of Date)), CType(obj.dtmEmision, System.Nullable(Of Date)), CType(obj.dtmVencimiento, System.Nullable(Of Date)), obj.logOtraPlaza, obj.strPlaza, obj.lngIDComisionistaLocal, obj.lngIDComisionistaOtraPlaza, CType(obj.lngIDCiudadOtraPlaza, System.Nullable(Of Integer)), CType(obj.dblTasaEfectiva, System.Nullable(Of Double)), CType(obj.dblCantidad, System.Nullable(Of Double)), CType(obj.curPrecio, System.Nullable(Of Double)), CType(obj.curTransaccion, System.Nullable(Of Double)), CType(obj.curSubTotalLiq, System.Nullable(Of Double)), CType(obj.curTotalLiq, System.Nullable(Of Double)), CType(obj.curComision, System.Nullable(Of Double)), CType(obj.curRetencion, System.Nullable(Of Double)), CType(obj.curIntereses, System.Nullable(Of Double)), CType(obj.curValorIva, System.Nullable(Of Double)), CType(obj.lngDiasIntereses, System.Nullable(Of Integer)), CType(obj.dblFactorComisionPactada, System.Nullable(Of Double)), obj.strMercado, obj.strNroTitulo, CType(obj.lngIDCiudadExpTitulo, System.Nullable(Of Integer)), CType(obj.lngPlazoOriginal, System.Nullable(Of Integer)), obj.logAplazamiento, CType(obj.bytVersionPapeleta, System.Nullable(Of Short)), CType(obj.dtmEmisionOriginal, System.Nullable(Of Date)), CType(obj.dtmVencimientoOriginal, System.Nullable(Of Date)), CType(obj.lngImpresiones, System.Nullable(Of Integer)), obj.strFormaPago, CType(obj.lngCtrlImpPapeleta, System.Nullable(Of Integer)), CType(obj.lngDiasVencimiento, System.Nullable(Of Integer)), obj.strPosicionPropia, obj.strTransaccion, obj.strTipoOperacion, CType(obj.lngDiasContado, System.Nullable(Of Integer)), obj.logOrdinaria, obj.strObjetoOrdenExtraordinaria, CType(obj.lngNumPadre, System.Nullable(Of Integer)), obj.lngParcialPadre, CType(obj.dtmOperacionPadre, System.Nullable(Of Date)), CType(obj.lngDiasTramo, System.Nullable(Of Integer)), obj.logVendido, CType(obj.dtmVendido, System.Nullable(Of Date)), obj.logManual, CType(obj.dblValorTraslado, System.Nullable(Of Double)), CType(obj.dblValorBrutoCompraVencida, System.Nullable(Of Double)), obj.strAutoRetenedor, obj.strSujeto, CType(obj.dblPcRenEfecCompraRet, System.Nullable(Of Double)), CType(obj.dblPcRenEfecVendeRet, System.Nullable(Of Double)), obj.strReinversion, obj.strSwap, CType(obj.lngNroSwap, System.Nullable(Of Integer)), obj.strCertificacion, CType(obj.dblDescuentoAcumula, System.Nullable(Of Double)), CType(obj.dblPctRendimiento, System.Nullable(Of Double)), CType(obj.dtmFechaCompraVencido, System.Nullable(Of Date)), CType(obj.dblPrecioCompraVencido, System.Nullable(Of Double)), obj.strConstanciaEnajenacion, obj.strRepoTitulo, CType(obj.dblServBolsaVble, System.Nullable(Of Double)), CType(obj.dblServBolsaFijo, System.Nullable(Of Double)), CType(Nothing, String), obj.strUBICACIONTITULO, obj.strHoraGrabacion, obj.strOrigenOperacion, CType(obj.lngCodigoOperadorCompra, System.Nullable(Of Integer)), CType(obj.lngCodigoOperadorVende, System.Nullable(Of Integer)), obj.strIdentificacionRemate, obj.strModalidaOperacion, obj.strIndicadorPrecio, obj.strPeriodoExdividendo, CType(obj.lngPlazoOperacionRepo, System.Nullable(Of Integer)), CType(obj.dblValorCaptacionRepo, System.Nullable(Of Double)), CType(obj.lngVolumenCompraRepo, System.Nullable(Of Double)), CType(obj.dblPrecioNetoFraccion, System.Nullable(Of Double)), CType(obj.lngVolumenNetoFraccion, System.Nullable(Of Double)), obj.lngCodigoContactoComercial, CType(obj.lngNroFraccionOperacion, System.Nullable(Of Integer)), obj.strIdentificacionPatrimonio1, obj.strTipoidentificacionCliente2, obj.strNitCliente2, obj.strIdentificacionPatrimonio2, obj.strTipoIdentificacionCliente3, obj.strNitCliente3, obj.strIdentificacionPatrimonio3, obj.strIndicadorOperacion, CType(obj.dblBaseRetencion, System.Nullable(Of Double)), CType(obj.dblPorcRetencion, System.Nullable(Of Double)), CType(obj.dblBaseRetencionTranslado, System.Nullable(Of Double)), CType(obj.dblPorcRetencionTranslado, System.Nullable(Of Double)), CType(obj.dblPorcIvaComision, System.Nullable(Of Double)), obj.strIndicadorAcciones, obj.strOperacionNegociada, CType(obj.dtmFechaConstancia, System.Nullable(Of Date)), CType(obj.dblValorConstancia, System.Nullable(Of Double)), obj.strGeneraConstancia, obj.logCargado, CType(obj.dtmCumplimientoTitulo, System.Nullable(Of Date)), CType(obj.intNroLote, System.Nullable(Of Integer)), CType(obj.dblValorEntregaContraPago, System.Nullable(Of Double)), obj.strAquienSeEnviaRetencion, obj.strIDBaseDias, obj.strTipoDeOferta, CType(obj.intNroLoteENC, System.Nullable(Of Integer)), CType(obj.dtmContabilidadENC, System.Nullable(Of Date)), obj.strCodigoIntermediario, CType(obj.curValorExtemporaneo, System.Nullable(Of Double)), obj.strPosicionExtemporaneo, obj.strUsuario, obj.strInfoSesion, CType(Nothing, System.Nullable(Of Byte)), p2)
        obj.intIDLiquidaciones = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
    End Sub

    Private Sub DeleteOperaciones(ByVal obj As OyDOperaciones.Operaciones)
        Dim p1 As String = obj.strMsgValidacion
        Me.uspOyDNet_Operaciones_Eliminar(CType(obj.intIDLiquidaciones, System.Nullable(Of Integer)), obj.strUsuario, obj.strInfoSesion, CType(Nothing, System.Nullable(Of Byte)), p1)
        obj.strMsgValidacion = p1
    End Sub

    Private Sub InsertOperacionesReceptores(ByVal obj As OyDOperaciones.OperacionesReceptores)
        Dim p1 As Integer = obj.intIDReceptoresOrdenes
        If idOrden = 0 Then
            idDetalleDocumento = obj.lngIDOrden
        Else
            idDetalleDocumento = idOrden
        End If
        Me.uspOyDNet_ReceptoresOrdenes_Actualizar(obj.strTipoOrden, obj.strClaseOrden, obj.lngIDOrden, obj.lngVersion, obj.strIDReceptor, CType(obj.logLider, System.Nullable(Of Boolean)), CType(obj.dblPorcentaje, System.Nullable(Of Double)), obj.strUsuario, p1, CType(Nothing, String), CType(Nothing, System.Nullable(Of Byte)))
        obj.intIDReceptoresOrdenes = p1
    End Sub

    Private Sub UpdateOperacionesReceptores(ByVal obj As OyDOperaciones.OperacionesReceptores)
        Dim p1 As Integer = obj.intIDReceptoresOrdenes
        Me.uspOyDNet_ReceptoresOrdenes_Actualizar(obj.strTipoOrden, obj.strClaseOrden, obj.lngIDOrden, obj.lngVersion, obj.strIDReceptor, CType(obj.logLider, System.Nullable(Of Boolean)), CType(obj.dblPorcentaje, System.Nullable(Of Double)), obj.strUsuario, p1, CType(Nothing, String), CType(Nothing, System.Nullable(Of Byte)))
        obj.intIDReceptoresOrdenes = p1
    End Sub

    Private Sub DeleteOperacionesReceptores(ByVal obj As OyDOperaciones.OperacionesReceptores)
        Me.uspOyDNet_ReceptoresOrdenes_Eliminar(obj.intIDReceptoresOrdenes, CType(Nothing, String), CType(Nothing, System.Nullable(Of Byte)))
    End Sub
    Private Sub UpdateRetardoOperaciones(ByVal obj As OyDOperaciones.RetardoOperaciones)
        Me.uspOyDNet_RetardoOperaciones_Editar()
    End Sub

    Private Sub UpdateRetardoOperacionesDetalle(ByVal obj As OyDOperaciones.RetardoOperacionesDetalle)
        Me.uspOyDNet_RetardoOperacionesDetalle_Editar()
    End Sub

End Class
