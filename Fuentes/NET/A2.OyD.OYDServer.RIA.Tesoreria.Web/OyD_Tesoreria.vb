Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Data.Linq
Imports System.Data.Linq.Mapping
Imports System.Linq
Imports System.Linq.Expressions
Imports System.Reflection
Imports A2.OyD.Infraestructura

Partial Public Class OyDTesoreriaDatacontext
    Dim idDocumento As Integer
    Dim idDetalleDocumento As Integer
    Dim idClaveporaprobar As Integer
    Dim intIDTesoreria As Integer


    Public Sub New()
        MyBase.New(ObtenerCadenaConexion(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString), mappingSource)
        OnCreated()
    End Sub
    Public Overrides Sub SubmitChanges(failureMode As Data.Linq.ConflictMode)
        Try
            MyBase.SubmitChanges(failureMode)
        Catch ex As Exception
            ManejarError(ex, "OyDTesoreriaDatacontext", "SubmitChanges")
        End Try
    End Sub

    '********************************** INICIA OYD PLUS*****************************************************************************

    Private Sub InsertTesoreriaOrdenesEncabezado(ByVal obj As OyDTesoreria.TesoreriaOrdenesEncabezado)
        'Dim p1 As Integer = Nothing
        'Me.uspOyDNet_TesoreriaOrdenesEncabezado_Actualizar( _
        '     obj.ValorTipo,
        '     obj.strCodigoReceptor,
        '     obj.strNombreConsecutivo,
        '     obj.ValorTipoProducto,
        '     obj.strIDComitente,
        '    p1,
        '     obj.ValorTipoIdentificacion,
        '     obj.lngNroDocumento,
        '     obj.strNroDocumento,
        '     obj.strNombre,
        '     obj.lngIDBanco,
        '     obj.curValor,
        '     obj.strUsuario,
        '     DemeInfoSesion(pstrUsuario, "InsertTesoreriaOrdenesEncabezado"),
        '     Nothing, "ING", obj.lngID, obj.ValorEstado,
        '     obj.logClienteRecoge,
        '     obj.logClientePresente,
        '     obj.logllevarDireccion,
        '     obj.logRecogeTercero,
        '     obj.logConsignarCta,
        '     obj.ValorTipoIdentificacion_Instrucciones,
        '     obj.strNroDocumento_Instrucciones,
        '     obj.strNombre_Instrucciones,
        '     obj.logDireccionInscrita_Instrucciones,
        '     obj.strCuenta_Instrucciones,
        '     obj.strDireccion_Instrucciones,
        '     obj.strCiudad_Instrucciones,
        '     obj.strSector_Instrucciones,
        '     obj.logEsTercero_Instrucciones,
        '     obj.logEsCtaInscrita_Instrucciones,
        '     obj.ValorTipoCta_Instrucciones)
        'intIdTesoreriaOrdenesEncabezado = p1
    End Sub

    Private Sub UpdateTesoreriaOrdenesEncabezado(ByVal obj As OyDTesoreria.TesoreriaOrdenesEncabezado)
        'Try
        '    Dim p1 As Integer = Nothing
        '    Me.uspOyDNet_TesoreriaOrdenesEncabezado_Actualizar( _
        '     obj.ValorTipo,
        '     obj.strCodigoReceptor,
        '     obj.strNombreConsecutivo,
        '     obj.ValorTipoProducto,
        '     obj.strIDComitente,
        '    Nothing,
        '     obj.ValorTipoIdentificacion,
        '     obj.lngNroDocumento,
        '     obj.strNroDocumento,
        '     obj.strNombre,
        '     obj.lngIDBanco,
        '     obj.curValor,
        '     obj.strUsuario,
        '     DemeInfoSesion(pstrUsuario, "InsertTesoreriaOrdenesEncabezado"),
        '     Nothing, "MOD", obj.lngID, obj.ValorEstado,
        '     obj.logClienteRecoge,
        '     obj.logClientePresente,
        '     obj.logllevarDireccion,
        '     obj.logRecogeTercero,
        '     obj.logConsignarCta,
        '     obj.ValorTipoIdentificacion_Instrucciones,
        '     obj.strNroDocumento_Instrucciones,
        '     obj.strNombre_Instrucciones,
        '     obj.logDireccionInscrita_Instrucciones,
        '     obj.strCuenta_Instrucciones,
        '     obj.strDireccion_Instrucciones,
        '     obj.strCiudad_Instrucciones,
        '     obj.strSector_Instrucciones,
        '     obj.logEsTercero_Instrucciones,
        '     obj.logEsCtaInscrita_Instrucciones,
        '     obj.ValorTipoCta_Instrucciones)
        '    intIdTesoreriaOrdenesEncabezado = p1
        'Catch ex As Exception
        '    ManejarError(ex, "ActualizarTesoreriaOrdenes", "UpdateTesoreriaOrdenesEncabezado")
        'End Try

    End Sub

#Region "Ordenes Recibo"
#End Region

    '********************************** TERMINA OYD PLUS*****************************************************************************
    Private Sub InsertTesoreri(ByVal obj As OyDTesoreria.Tesoreri)
        Dim p1 As Integer = obj.IDDocumento
        Dim p2 As Integer = obj.IDTesoreria
        Me.uspOyDNet_Tesoreria_Actualizar(obj.Aprobacion, obj.Tipo, obj.NombreConsecutivo, p1, obj.TipoIdentificacion, obj.NroDocumento, obj.Nombre, obj.IDBanco, CType(obj.NumCheque, System.Nullable(Of Double)), CType(obj.Valor, System.Nullable(Of Decimal)), obj.Detalle, CType(obj.Documento, System.Nullable(Of Date)), obj.Estado, CType(obj.FecEstado, System.Nullable(Of Date)), obj.Impresiones, obj.FormaPagoCE, obj.NroLote, CType(obj.Contabilidad, System.Nullable(Of Boolean)), CType(obj.Actualizacion, System.Nullable(Of Date)), obj.Usuario, obj.ParametroContable, CType(obj.ImpresionFisica, System.Nullable(Of Boolean)), CType(obj.MultiCliente, System.Nullable(Of Boolean)), obj.CuentaCliente, obj.CodComitente, obj.ArchivoTransferencia, obj.IdNumInst, obj.DVP, obj.Instruccion, CType(obj.IdNroOrden, System.Nullable(Of Decimal)), obj.DetalleInstruccion, obj.EstadoNovedadContabilidad, obj.eroComprobante_Contabilidad, obj.hadecontabilizacion_Contabilidad, CType(obj.haProceso_Contabilidad, System.Nullable(Of Date)), obj.EstadoNovedadContabilidad_Anulada, obj.EstadoAutomatico, obj.eroLote_Contabilidad, obj.MontoEscrito, obj.TipoIntermedia, obj.Concepto, CType(obj.RecaudoDirecto, System.Nullable(Of Boolean)), CType(obj.ContabilidadEncuenta, System.Nullable(Of Date)), CType(obj.Sobregiro, System.Nullable(Of Boolean)), obj.IdentificacionAutorizadoCheque, obj.NombreAutorizadoCheque, p2, CType(obj.ContabilidadENC, System.Nullable(Of Date)), obj.NroLoteAntENC, CType(obj.ContabilidadAntENC, System.Nullable(Of Date)), obj.IdSucursalBancaria, CType(obj.Creacion, System.Nullable(Of Date)), obj.CobroGMF, obj.TrasladoEntreBancos, obj.lngBancoConsignacion, obj.ClienteGMF, obj.BancoGMF, obj.Tipocheque, obj.TipoCuenta, obj.XMLDetalle, obj.XMLDetalleCheques, obj.IDCompania, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.IDDocumento = p1
        obj.IDTesoreria = p2
        idDocumento = p1
        intIDTesoreria = p2
    End Sub

    Public Sub UpdateTesoreri(ByVal obj As OyDTesoreria.Tesoreri)
        Dim p1 As Integer = obj.IDDocumento
        Dim p2 As Integer = obj.IDTesoreria
        obj.InfoSesion = DemeInfoSesion(obj.pstrUsuarioConexion, "UpdateTesoreri")

        If obj.Estado = "A" And IsNothing(obj.Por_Aprobar) Then
            Me.uspOyDNet_Tesoreria_Eliminar(obj.Tipo, obj.IDTesoreria, obj.IDDocumento, obj.NombreConsecutivo, obj.TipoIdentificacion, obj.NroDocumento, obj.Nombre, obj.Documento, obj.Usuario, obj.Estado, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        Else
            Me.uspOyDNet_Tesoreria_Actualizar(obj.Aprobacion, obj.Tipo, obj.NombreConsecutivo, p1, obj.TipoIdentificacion, obj.NroDocumento, obj.Nombre, obj.IDBanco, CType(obj.NumCheque, System.Nullable(Of Double)), CType(obj.Valor, System.Nullable(Of Decimal)), obj.Detalle, CType(obj.Documento, System.Nullable(Of Date)), obj.Estado, CType(obj.FecEstado, System.Nullable(Of Date)), obj.Impresiones, obj.FormaPagoCE, obj.NroLote, CType(obj.Contabilidad, System.Nullable(Of Boolean)), CType(obj.Actualizacion, System.Nullable(Of Date)), obj.Usuario, obj.ParametroContable, CType(obj.ImpresionFisica, System.Nullable(Of Boolean)), CType(obj.MultiCliente, System.Nullable(Of Boolean)), obj.CuentaCliente, obj.CodComitente, obj.ArchivoTransferencia, obj.IdNumInst, obj.DVP, obj.Instruccion, CType(obj.IdNroOrden, System.Nullable(Of Decimal)), obj.DetalleInstruccion, obj.EstadoNovedadContabilidad, obj.eroComprobante_Contabilidad, obj.hadecontabilizacion_Contabilidad, CType(obj.haProceso_Contabilidad, System.Nullable(Of Date)), obj.EstadoNovedadContabilidad_Anulada, obj.EstadoAutomatico, obj.eroLote_Contabilidad, obj.MontoEscrito, obj.TipoIntermedia, obj.Concepto, CType(obj.RecaudoDirecto, System.Nullable(Of Boolean)), CType(obj.ContabilidadEncuenta, System.Nullable(Of Date)), CType(obj.Sobregiro, System.Nullable(Of Boolean)), obj.IdentificacionAutorizadoCheque, obj.NombreAutorizadoCheque, p2, CType(obj.ContabilidadENC, System.Nullable(Of Date)), obj.NroLoteAntENC, CType(obj.ContabilidadAntENC, System.Nullable(Of Date)), obj.IdSucursalBancaria, CType(obj.Creacion, System.Nullable(Of Date)), obj.CobroGMF, obj.TrasladoEntreBancos, obj.lngBancoConsignacion, obj.ClienteGMF, obj.BancoGMF, obj.Tipocheque, obj.TipoCuenta, obj.XMLDetalle, obj.XMLDetalleCheques, obj.IDCompania, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        End If

        obj.IDDocumento = p1
        obj.IDTesoreria = p2
        idDocumento = p1
    End Sub

    Private Sub InsertDetalleTesoreri(ByVal obj As OyDTesoreria.DetalleTesoreri)
        Dim p1 As Integer = obj.IDDetalleTesoreria
        If idDocumento = 0 Then
            idDetalleDocumento = obj.IDDocumento
            idClaveporaprobar = obj.intClave_PorAprobar
        Else
            idDetalleDocumento = idDocumento
            idClaveporaprobar = -1
        End If
        Me.uspOyDNet_DetalleTesoreria_Actualizar(obj.Aprobacion, obj.Tipo, obj.NombreConsecutivo, idDetalleDocumento, obj.Secuencia, obj.IDComitente, CType(obj.Valor, System.Nullable(Of Decimal)), obj.IDCuentaContable, obj.Detalle, obj.IDBanco, obj.NIT, obj.CentroCosto, CType(obj.Actualizacion, System.Nullable(Of Date)), obj.Usuario, obj.EstadoTransferencia, obj.BancoDestino, obj.CuentaDestino, obj.TipoCuenta, obj.IdentificacionTitular, obj.Titular, obj.TipoIdTitular, obj.FormaEntrega, obj.Beneficiario, obj.TipoIdentBeneficiario, obj.IdentificacionBenficiciario, obj.NombrePersonaRecoge, obj.IdentificacionPerRecoge, obj.OficinaEntrega, p1, obj.NombreConsecutivoNotaGMF, obj.IDDocumentoNotaGMF, obj.IDConcepto, obj.Exportados, idClaveporaprobar, intIDTesoreria, obj.OficinaCuentaInversion, obj.NombreCarteraColectiva, obj.NombreAsesor, obj.CodigoCartera, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.IDDetalleTesoreria = p1
    End Sub

    Public Sub UpdateDetalleTesoreri(ByVal obj As OyDTesoreria.DetalleTesoreri)
        Dim p1 As Integer = obj.IDDetalleTesoreria
        If idDocumento = 0 Then
            If IsNothing(obj.Por_Aprobar) Or obj.Por_Aprobar = String.Empty Then
                idDetalleDocumento = obj.IDDocumento
                idClaveporaprobar = -1
                'Me.uspOyDNet_DetalleTesoreria_Actualizar(obj.Aprobacion, obj.Tipo, obj.NombreConsecutivo, idDetalleDocumento, obj.Secuencia, obj.IDComitente, CType(obj.Valor, System.Nullable(Of Decimal)), obj.IDCuentaContable, obj.Detalle, obj.IDBanco, obj.NIT, obj.CentroCosto, CType(obj.Actualizacion, System.Nullable(Of Date)), obj.Usuario, obj.EstadoTransferencia, obj.BancoDestino, obj.CuentaDestino, obj.TipoCuenta, obj.IdentificacionTitular, obj.Titular, obj.TipoIdTitular, obj.FormaEntrega, obj.Beneficiario, obj.TipoIdentBeneficiario, obj.IdentificacionBenficiciario, obj.NombrePersonaRecoge, obj.IdentificacionPerRecoge, obj.OficinaEntrega, p1, obj.NombreConsecutivoNotaGMF, obj.IDDocumentoNotaGMF, obj.IDConcepto, obj.Exportados, idClaveporaprobar, intIDTesoreria, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
                Me.uspOyDNet_DetalleTesoreria_Actualizar(obj.Aprobacion, obj.Tipo, obj.NombreConsecutivo, idDetalleDocumento, obj.Secuencia, obj.IDComitente, CType(obj.Valor, System.Nullable(Of Decimal)), obj.IDCuentaContable, obj.Detalle, obj.IDBanco, obj.NIT, obj.CentroCosto, CType(obj.Actualizacion, System.Nullable(Of Date)), obj.Usuario, obj.EstadoTransferencia, obj.BancoDestino, obj.CuentaDestino, obj.TipoCuenta, obj.IdentificacionTitular, obj.Titular, obj.TipoIdTitular, obj.FormaEntrega, obj.Beneficiario, obj.TipoIdentBeneficiario, obj.IdentificacionBenficiciario, obj.NombrePersonaRecoge, obj.IdentificacionPerRecoge, obj.OficinaEntrega, p1, obj.NombreConsecutivoNotaGMF, obj.IDDocumentoNotaGMF, obj.IDConcepto, obj.Exportados, idClaveporaprobar, obj.intIDTesoreria, obj.OficinaCuentaInversion, obj.NombreCarteraColectiva, obj.NombreAsesor, obj.CodigoCartera, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
                obj.IDDetalleTesoreria = p1
                Exit Sub
            End If
            idDetalleDocumento = obj.IDDocumento
            idClaveporaprobar = obj.intClave_PorAprobar
        Else
            idDetalleDocumento = idDocumento
            idClaveporaprobar = -1
        End If
        Me.uspOyDNet_DetalleTesoreria_Actualizar(obj.Aprobacion, obj.Tipo, obj.NombreConsecutivo, idDetalleDocumento, obj.Secuencia, obj.IDComitente, CType(obj.Valor, System.Nullable(Of Decimal)), obj.IDCuentaContable, obj.Detalle, obj.IDBanco, obj.NIT, obj.CentroCosto, CType(obj.Actualizacion, System.Nullable(Of Date)), obj.Usuario, obj.EstadoTransferencia, obj.BancoDestino, obj.CuentaDestino, obj.TipoCuenta, obj.IdentificacionTitular, obj.Titular, obj.TipoIdTitular, obj.FormaEntrega, obj.Beneficiario, obj.TipoIdentBeneficiario, obj.IdentificacionBenficiciario, obj.NombrePersonaRecoge, obj.IdentificacionPerRecoge, obj.OficinaEntrega, p1, obj.NombreConsecutivoNotaGMF, obj.IDDocumentoNotaGMF, obj.IDConcepto, obj.Exportados, idClaveporaprobar, obj.intIDTesoreria, obj.OficinaCuentaInversion, obj.NombreCarteraColectiva, obj.NombreAsesor, obj.CodigoCartera, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.IDDetalleTesoreria = p1
    End Sub

    Private Sub InsertCheque(ByVal obj As OyDTesoreria.Cheque)
        Dim p1 As Integer = obj.IDCheques
        If idDocumento = 0 Then
            idDetalleDocumento = obj.IDDocumento
        Else
            idDetalleDocumento = idDocumento
        End If
        Me.uspOyDNet_ChequesTesoreria_Actualizar(obj.Aprobacion, obj.NombreConsecutivo, idDetalleDocumento, obj.Secuencia, CType(obj.Efectivo, System.Nullable(Of Boolean)), obj.BancoGirador, CType(obj.NumCheque, System.Nullable(Of Double)), CType(obj.Valor, System.Nullable(Of Decimal)), obj.BancoConsignacion, CType(obj.Consignacion, System.Nullable(Of Date)), obj.FormaPagoRC, CType(obj.Actualizacion, System.Nullable(Of Date)), obj.Usuario, obj.Comentario, obj.IdProducto, obj.SucursalesBancolombia, p1, CType(obj.ChequeHizoCanje, System.Nullable(Of Boolean)), intIDTesoreria, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.IDCheques = p1
    End Sub

    Public Sub UpdateCheque(ByVal obj As OyDTesoreria.Cheque)
        Dim p1 As Integer = obj.IDCheques
        If idDocumento = 0 Then
            idDetalleDocumento = obj.IDDocumento
        Else
            idDetalleDocumento = idDocumento
        End If
        'Me.uspOyDNet_ChequesTesoreria_Actualizar(obj.Aprobacion, obj.NombreConsecutivo, idDetalleDocumento, obj.Secuencia, CType(obj.Efectivo, System.Nullable(Of Boolean)), obj.BancoGirador, CType(obj.NumCheque, System.Nullable(Of Double)), CType(obj.Valor, System.Nullable(Of Decimal)), obj.BancoConsignacion, CType(obj.Consignacion, System.Nullable(Of Date)), obj.FormaPagoRC, CType(obj.Actualizacion, System.Nullable(Of Date)), obj.Usuario, obj.Comentario, obj.IdProducto, obj.SucursalesBancolombia, p1, CType(obj.ChequeHizoCanje, System.Nullable(Of Boolean)), intIDTesoreria, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        Me.uspOyDNet_ChequesTesoreria_Actualizar(obj.Aprobacion, obj.NombreConsecutivo, idDetalleDocumento, obj.Secuencia, CType(obj.Efectivo, System.Nullable(Of Boolean)), obj.BancoGirador, CType(obj.NumCheque, System.Nullable(Of Double)), CType(obj.Valor, System.Nullable(Of Decimal)), obj.BancoConsignacion, CType(obj.Consignacion, System.Nullable(Of Date)), obj.FormaPagoRC, CType(obj.Actualizacion, System.Nullable(Of Date)), obj.Usuario, obj.Comentario, obj.IdProducto, obj.SucursalesBancolombia, p1, CType(obj.ChequeHizoCanje, System.Nullable(Of Boolean)), obj.intIDTesoreria, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.IDCheques = p1
    End Sub

    Private Sub InsertTesoreriaAdicionale(ByVal obj As OyDTesoreria.TesoreriaAdicionale)
        Dim p1 As Integer = obj.IDTesoreriaAdicionales
        If idDocumento = 0 Then
            idDetalleDocumento = obj.IDDocumento
        Else
            idDetalleDocumento = idDocumento
        End If
        Me.uspOyDNet_ObservacionesTesoreria_Actualizar(obj.Tipo, obj.NombreConsecutivo, idDetalleDocumento, obj.Observacion, CType(obj.Actualizacion, System.Nullable(Of Date)), obj.Usuario, p1, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.IDTesoreriaAdicionales = p1
    End Sub

    Public Sub UpdateTesoreriaAdicionale(ByVal obj As OyDTesoreria.TesoreriaAdicionale)
        Dim p1 As Integer = obj.IDTesoreriaAdicionales
        If idDocumento = 0 Then
            idDetalleDocumento = obj.IDDocumento
        Else
            idDetalleDocumento = idDocumento
        End If
        Me.uspOyDNet_ObservacionesTesoreria_Actualizar(obj.Tipo, obj.NombreConsecutivo, idDetalleDocumento, obj.Observacion, CType(obj.Actualizacion, System.Nullable(Of Date)), obj.Usuario, p1, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.IDTesoreriaAdicionales = p1
    End Sub

    <Global.System.Data.Linq.Mapping.FunctionAttribute(Name:="dbo.uspOyDNet_Tesoreria_ConsutarCuentaTransferencia")> _
    Public Function uspOyDNet_Tesoreria_ConsutarCuentaTransferencia(<Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Char(17)")> ByVal plngIDComitente As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(1000)")> ByVal infosesion As String, <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="TinyInt")> ByVal pintErrorPersonalizado As System.Nullable(Of Byte), <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Bit")> ByRef pbolTransferenciaDisponible As Boolean) As Integer
        Dim result As IExecuteResult = Me.ExecuteMethodCall(Me, CType(MethodInfo.GetCurrentMethod, MethodInfo), plngIDComitente, infosesion, pintErrorPersonalizado, pbolTransferenciaDisponible)
        pbolTransferenciaDisponible = CType(result.GetParameterValue(3), System.Nullable(Of Boolean))
        Return CType(result.ReturnValue, Integer)
    End Function

End Class
