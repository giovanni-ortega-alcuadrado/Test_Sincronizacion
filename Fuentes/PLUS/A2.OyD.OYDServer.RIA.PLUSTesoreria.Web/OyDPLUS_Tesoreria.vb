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

Partial Public Class OyDPLUSTesoreriaDatacontext
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
            ManejarError(ex, "OyDPLUSTesoreriaDatacontext", "SubmitChanges")
        End Try
    End Sub

    '********************************** INICIA OYD PLUS*****************************************************************************

    Private Sub InsertTesoreriaOrdenesEncabezado(ByVal obj As OyDPLUSTesoreria.TesoreriaOrdenesEncabezado)
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

    Private Sub UpdateTesoreriaOrdenesEncabezado(ByVal obj As OyDPLUSTesoreria.TesoreriaOrdenesEncabezado)
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

    'Private Sub UpdateTesorero(ByVal obj As OyDPLUSTesoreria.Tesorero)

    'End Sub

    Private Sub InsertTesoreriaOyDPlusCheques(ByVal obj As OyDPLUSTesoreria.TesoreriaOyDPlusCheques)
        Dim p1 As Integer = 0
        If IsNothing(obj.lngIDTesoreriaEncabezado) Or (obj.lngIDTesoreriaEncabezado = 0) Then
            If intIdTesoreriaOrdenesEncabezado <> 0 Then
                p1 = intIdTesoreriaOrdenesEncabezado
            End If
        Else
            p1 = obj.lngIDTesoreriaEncabezado
        End If

        Me.uspOyDNet_DetalleTesoreriaOrdenes_Actualizar(
            CType(obj.lngIDDetalle, System.Nullable(Of Integer)),
            CType(obj.lngID, System.Nullable(Of Integer)),
            CType(p1, System.Nullable(Of Integer)),
            obj.strTipo, obj.strFormaPago,
            CType(obj.lngIDConcepto, System.Nullable(Of Integer)),
            obj.strDetalleConcepto, obj.NombreConsecutivo,
            obj.strNroDocumento, obj.strTipoDocumento,
            obj.strNombre, CType(obj.curValor, System.Nullable(Of Decimal)),
            CType(obj.logEsTercero, System.Nullable(Of Boolean)),
            obj.ValorTipoGMF,
            obj.ValorTipoCheque,
            CType(obj.ValorTipoCruce, String),
            CType(obj.lngNroCheque, System.Nullable(Of Double)),
            CType(obj.logEsCuentaRegistrada, System.Nullable(Of Boolean)),
            obj.strCuenta,
            CType(obj.lngIdBanco, System.Nullable(Of Integer)),
            obj.strNombreTitular,
            obj.strTipoDocumentoTitular,
            obj.strNroDocumentoTitular,
            CType(obj.curValorGMF, System.Nullable(Of Decimal)),
            obj.strTipoCuenta, CType(obj.dtmFechaDocumento, System.Nullable(Of Date)),
            obj.lngNroEncargo,
            obj.strNombreCarteraColectiva, obj.strCodigoOyD, obj.strUsuario,
            obj.infosesion, CType(Nothing, System.Nullable(Of Byte)),
                obj.strIDTipoCliente,
                obj.curValorNeto, Nothing, obj.ValorTotalNota,
                obj.curValorSaldoConsultado,
                obj.curValorBloqueado,
                obj.strMotivoBloqueado,
                obj.strNaturaleza,
                obj.strDetalleBloqueo,
                String.Empty, 0, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty,
                obj.UsuarioWindows,
                Nothing, Nothing)

    End Sub

    Private Sub UpdateTesoreriaOyDPlusCheques(ByVal obj As OyDPLUSTesoreria.TesoreriaOyDPlusCheques)

        Dim p1 As Integer = 0
        If IsNothing(obj.lngIDTesoreriaEncabezado) Or (obj.lngIDTesoreriaEncabezado = 0) Then
            If intIdTesoreriaOrdenesEncabezado <> 0 Then
                p1 = intIdTesoreriaOrdenesEncabezado
            End If
        Else
            p1 = obj.lngIDTesoreriaEncabezado
        End If

        Me.uspOyDNet_DetalleTesoreriaOrdenes_Actualizar(
            CType(obj.lngIDDetalle, System.Nullable(Of Integer)),
            CType(obj.lngID, System.Nullable(Of Integer)),
            CType(p1, System.Nullable(Of Integer)),
            obj.strTipo, obj.strFormaPago,
            CType(obj.lngIDConcepto, System.Nullable(Of Integer)),
            obj.strDetalleConcepto, obj.NombreConsecutivo,
            obj.strNroDocumento, obj.strTipoDocumento,
            obj.strNombre, CType(obj.curValor, System.Nullable(Of Decimal)),
            CType(obj.logEsTercero, System.Nullable(Of Boolean)),
            obj.ValorTipoGMF,
            obj.ValorTipoCheque,
            CType(obj.ValorTipoCruce, String),
            CType(obj.lngNroCheque, System.Nullable(Of Double)),
            CType(obj.logEsCuentaRegistrada, System.Nullable(Of Boolean)),
            obj.strCuenta,
            CType(obj.lngIdBanco, System.Nullable(Of Integer)),
            obj.strNombreTitular,
            obj.strTipoDocumentoTitular,
            obj.strNroDocumentoTitular,
            CType(obj.curValorGMF, System.Nullable(Of Decimal)),
            obj.strTipoCuenta, CType(obj.dtmFechaDocumento, System.Nullable(Of Date)),
            obj.lngNroEncargo,
            obj.strNombreCarteraColectiva, obj.strCodigoOyD, obj.strUsuario,
            obj.infosesion, CType(Nothing, System.Nullable(Of Byte)),
                obj.strIDTipoCliente,
                obj.curValorNeto, Nothing, obj.ValorTotalNota,
                obj.curValorSaldoConsultado,
                 obj.curValorBloqueado,
                obj.strMotivoBloqueado,
                obj.strNaturaleza,
                obj.strDetalleBloqueo,
                String.Empty, 0, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty,
                obj.UsuarioWindows,
                Nothing, Nothing)
    End Sub

    Private Sub InsertTesoreriaOyDPlusTransferencia(ByVal obj As OyDPLUSTesoreria.TesoreriaOyDPlusTransferencia)

        Dim p1 As Integer = 0
        If IsNothing(obj.lngIDTesoreriaEncabezado) Or (obj.lngIDTesoreriaEncabezado = 0) Then
            If intIdTesoreriaOrdenesEncabezado <> 0 Then
                p1 = intIdTesoreriaOrdenesEncabezado
            End If
        Else
            p1 = obj.lngIDTesoreriaEncabezado
        End If

        Me.uspOyDNet_DetalleTesoreriaOrdenes_Actualizar(
            CType(obj.lngIDDetalle, System.Nullable(Of Integer)),
            CType(obj.lngID, System.Nullable(Of Integer)),
            CType(p1, System.Nullable(Of Integer)),
            obj.strTipo, obj.strFormaPago,
            CType(obj.lngIDConcepto, System.Nullable(Of Integer)),
            obj.strDetalleConcepto, obj.NombreConsecutivo,
            obj.strNroDocumento, obj.strTipoDocumento,
            obj.strNombre, CType(obj.curValor, System.Nullable(Of Decimal)),
            CType(obj.logEsTercero, System.Nullable(Of Boolean)),
            obj.ValorTipoGMF,
            obj.ValorTipoCheque,
            CType(obj.ValorTipoCruce, String),
            CType(obj.lngNroCheque, System.Nullable(Of Double)),
            CType(obj.logEsCuentaRegistrada, System.Nullable(Of Boolean)),
            obj.strCuenta,
            CType(obj.lngIdBanco, System.Nullable(Of Integer)),
            obj.strNombreTitular,
            obj.strValorTipoDocumentoTitular,
            obj.strNroDocumentoTitular,
            CType(obj.curValorGMF, System.Nullable(Of Decimal)),
            obj.strValorTipoCuenta, CType(obj.dtmFechaDocumento, System.Nullable(Of Date)),
            obj.lngNroEncargo,
            obj.strNombreCarteraColectiva, obj.strCodigoOyD, obj.strUsuario,
            obj.infosesion, CType(Nothing, System.Nullable(Of Byte)),
                obj.strIDTipoCliente,
                obj.curValorNeto, obj.strEsCtaRegistrada, obj.ValorTotalNota,
                obj.curValorSaldoConsultado,
                 obj.curValorBloqueado,
                obj.strMotivoBloqueado,
                obj.strNaturaleza,
                obj.strDetalleBloqueo,
                String.Empty, 0, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty,
                obj.UsuarioWindows,
                Nothing, Nothing)
    End Sub

    Private Sub UpdateTesoreriaOyDPlusTransferencia(ByVal obj As OyDPLUSTesoreria.TesoreriaOyDPlusTransferencia)

        Dim p1 As Integer = 0
        If IsNothing(obj.lngIDTesoreriaEncabezado) Or (obj.lngIDTesoreriaEncabezado = 0) Then
            If intIdTesoreriaOrdenesEncabezado <> 0 Then
                p1 = intIdTesoreriaOrdenesEncabezado
            End If
        Else
            p1 = obj.lngIDTesoreriaEncabezado
        End If

        Me.uspOyDNet_DetalleTesoreriaOrdenes_Actualizar(
             CType(obj.lngIDDetalle, System.Nullable(Of Integer)),
             CType(obj.lngID, System.Nullable(Of Integer)),
             CType(p1, System.Nullable(Of Integer)),
             obj.strTipo, obj.strFormaPago,
             CType(obj.lngIDConcepto, System.Nullable(Of Integer)),
             obj.strDetalleConcepto, obj.NombreConsecutivo,
             obj.strNroDocumento, obj.strTipoDocumento,
             obj.strNombre, CType(obj.curValor, System.Nullable(Of Decimal)),
             CType(obj.logEsTercero, System.Nullable(Of Boolean)),
             obj.ValorTipoGMF,
             obj.ValorTipoCheque,
             CType(obj.ValorTipoCruce, String),
             CType(obj.lngNroCheque, System.Nullable(Of Double)),
             CType(obj.logEsCuentaRegistrada, System.Nullable(Of Boolean)),
             obj.strCuenta,
             CType(obj.lngIdBanco, System.Nullable(Of Integer)),
             obj.strNombreTitular,
             obj.strValorTipoDocumentoTitular,
             obj.strNroDocumentoTitular,
             CType(obj.curValorGMF, System.Nullable(Of Decimal)),
             obj.strValorTipoCuenta, CType(obj.dtmFechaDocumento, System.Nullable(Of Date)),
             obj.lngNroEncargo,
             obj.strNombreCarteraColectiva, obj.strCodigoOyD, obj.strUsuario,
             obj.infosesion, CType(Nothing, System.Nullable(Of Byte)),
                 obj.strIDTipoCliente,
                 obj.curValorNeto, obj.strEsCtaRegistrada, obj.ValorTotalNota,
                obj.curValorSaldoConsultado,
                 obj.curValorBloqueado,
                obj.strMotivoBloqueado,
                obj.strNaturaleza,
                obj.strDetalleBloqueo,
                String.Empty, 0, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty,
                obj.UsuarioWindows,
                Nothing, Nothing)
    End Sub

    Private Sub InsertTesoreriaOyDPlusCarterasColectivas(ByVal obj As OyDPLUSTesoreria.TesoreriaOyDPlusCarterasColectivas)
        Dim p1 As Integer = 0
        If IsNothing(obj.lngIDTesoreriaEncabezado) Or (obj.lngIDTesoreriaEncabezado = 0) Then
            If intIdTesoreriaOrdenesEncabezado <> 0 Then
                p1 = intIdTesoreriaOrdenesEncabezado
            End If
        Else
            p1 = obj.lngIDTesoreriaEncabezado
        End If

        Me.uspOyDNet_DetalleTesoreriaOrdenes_Actualizar(
            CType(obj.lngIDDetalle, System.Nullable(Of Integer)),
            CType(obj.lngID, System.Nullable(Of Integer)),
            CType(p1, System.Nullable(Of Integer)),
            obj.strTipo, obj.strFormaPago,
            CType(obj.lngIDConcepto, System.Nullable(Of Integer)),
            obj.strDetalleConcepto, obj.NombreConsecutivo,
            obj.strNroDocumento, obj.strTipoDocumento,
            obj.strNombre, CType(obj.curValor, System.Nullable(Of Decimal)),
            CType(obj.logEsTercero, System.Nullable(Of Boolean)),
            obj.ValorTipoGMF,
            obj.ValorTipoCheque,
            CType(obj.ValorTipoCruce, String),
            CType(obj.lngNroCheque, System.Nullable(Of Double)),
            CType(obj.logEsCuentaRegistrada, System.Nullable(Of Boolean)),
            obj.strCuenta,
            CType(obj.lngIdBanco, System.Nullable(Of Integer)),
            obj.strNombreTitular,
            obj.strTipoDocumentoTitular,
            obj.strNroDocumentoTitular,
            CType(obj.curValorGMF, System.Nullable(Of Decimal)),
            obj.strTipoCuenta, CType(obj.dtmFechaDocumento, System.Nullable(Of Date)),
            obj.lngNroEncargo,
            obj.ValorCarteraColectiva, obj.strCodigoOyD, obj.strUsuario,
            obj.infosesion, CType(Nothing, System.Nullable(Of Byte)),
                obj.strIDTipoCliente,
                obj.curValorNeto, Nothing, obj.ValorTotalNota,
                obj.curValorSaldoConsultado,
                 obj.curValorBloqueado,
                obj.strMotivoBloqueado,
                obj.strNaturaleza,
                obj.strDetalleBloqueo,
                String.Empty, 0, String.Empty, String.Empty, String.Empty, String.Empty, obj.strTipoAccionFondos,
                obj.UsuarioWindows,
                obj.dtmFechaAplicacion,
                obj.DescripcionEncargoFondos)
    End Sub

    Private Sub UpdateTesoreriaOyDPlusCarterasColectivas(ByVal obj As OyDPLUSTesoreria.TesoreriaOyDPlusCarterasColectivas)

        Dim p1 As Integer = 0
        If IsNothing(obj.lngIDTesoreriaEncabezado) Or (obj.lngIDTesoreriaEncabezado = 0) Then
            If intIdTesoreriaOrdenesEncabezado <> 0 Then
                p1 = intIdTesoreriaOrdenesEncabezado
            End If
        Else
            p1 = obj.lngIDTesoreriaEncabezado
        End If

        Me.uspOyDNet_DetalleTesoreriaOrdenes_Actualizar(
            CType(obj.lngIDDetalle, System.Nullable(Of Integer)),
            CType(obj.lngID, System.Nullable(Of Integer)),
            CType(p1, System.Nullable(Of Integer)),
            obj.strTipo, obj.strFormaPago,
            CType(obj.lngIDConcepto, System.Nullable(Of Integer)),
            obj.strDetalleConcepto, obj.NombreConsecutivo,
            obj.strNroDocumento, obj.strTipoDocumento,
            obj.strNombre, CType(obj.curValor, System.Nullable(Of Decimal)),
            CType(obj.logEsTercero, System.Nullable(Of Boolean)),
            obj.ValorTipoGMF,
            obj.ValorTipoCheque,
            CType(obj.ValorTipoCruce, String),
            CType(obj.lngNroCheque, System.Nullable(Of Double)),
            CType(obj.logEsCuentaRegistrada, System.Nullable(Of Boolean)),
            obj.strCuenta,
            CType(obj.lngIdBanco, System.Nullable(Of Integer)),
            obj.strNombreTitular,
            obj.strTipoDocumentoTitular,
            obj.strNroDocumentoTitular,
            CType(obj.curValorGMF, System.Nullable(Of Decimal)),
            obj.strTipoCuenta, CType(obj.dtmFechaDocumento, System.Nullable(Of Date)),
            obj.lngNroEncargo,
            obj.ValorCarteraColectiva, obj.strCodigoOyD, obj.strUsuario,
            obj.infosesion, CType(Nothing, System.Nullable(Of Byte)),
                obj.strIDTipoCliente,
                obj.curValorNeto, Nothing, obj.ValorTotalNota,
                obj.curValorSaldoConsultado,
                 obj.curValorBloqueado,
                obj.strMotivoBloqueado,
                obj.strNaturaleza,
                obj.strDetalleBloqueo,
                String.Empty, 0, String.Empty, String.Empty, String.Empty, String.Empty, obj.strTipoAccionFondos,
                obj.UsuarioWindows,
                obj.dtmFechaAplicacion,
                obj.DescripcionEncargoFondos)
    End Sub

    Private Sub InsertTesoreriaOyDPlusOYD(ByVal obj As OyDPLUSTesoreria.TesoreriaOyDPlusOYD)
        Dim p1 As Integer = 0
        If IsNothing(obj.lngIDTesoreriaEncabezado) Or (obj.lngIDTesoreriaEncabezado = 0) Then
            If intIdTesoreriaOrdenesEncabezado <> 0 Then
                p1 = intIdTesoreriaOrdenesEncabezado
            End If
        Else
            p1 = obj.lngIDTesoreriaEncabezado
        End If

        Me.uspOyDNet_DetalleTesoreriaOrdenes_Actualizar(
            CType(obj.lngIDDetalle, System.Nullable(Of Integer)),
            CType(obj.lngID, System.Nullable(Of Integer)),
            CType(p1, System.Nullable(Of Integer)),
            obj.strTipo, obj.strFormaPago,
            CType(obj.lngIDConcepto, System.Nullable(Of Integer)),
            obj.strDetalleConcepto, obj.NombreConsecutivo,
            obj.strNroDocumento, obj.strTipoDocumento,
            obj.strNombre, CType(obj.curValor, System.Nullable(Of Decimal)),
            CType(obj.logEsTercero, System.Nullable(Of Boolean)),
            obj.ValorTipoGMF,
            obj.ValorTipoCheque,
            CType(obj.ValorTipoCruce, String),
            CType(obj.lngNroCheque, System.Nullable(Of Double)),
            CType(obj.logEsCuentaRegistrada, System.Nullable(Of Boolean)),
            obj.strCuenta,
            CType(obj.lngIdBanco, System.Nullable(Of Integer)),
            obj.strNombreTitular,
            obj.strTipoDocumentoTitular,
            obj.strNroDocumentoTitular,
            CType(obj.curValorGMF, System.Nullable(Of Decimal)),
            obj.strTipoCuenta, CType(obj.dtmFechaDocumento, System.Nullable(Of Date)),
            obj.lngNroEncargo,
            obj.ValorCarteraColectiva, obj.strCodigoOyD, obj.strUsuario,
            obj.infosesion, CType(Nothing, System.Nullable(Of Byte)),
                obj.strIDTipoCliente,
                obj.curValorNeto, Nothing, obj.ValorTotalNota,
                obj.curValorSaldoConsultado,
                 obj.curValorBloqueado,
                obj.strMotivoBloqueado,
                obj.strNaturaleza,
                obj.strDetalleBloqueo,
                String.Empty, 0, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty,
                obj.UsuarioWindows,
                Nothing, Nothing)
    End Sub

    Private Sub UpdateTesoreriaOyDPlusOYD(ByVal obj As OyDPLUSTesoreria.TesoreriaOyDPlusOYD)

        Dim p1 As Integer = 0
        If IsNothing(obj.lngIDTesoreriaEncabezado) Or (obj.lngIDTesoreriaEncabezado = 0) Then
            If intIdTesoreriaOrdenesEncabezado <> 0 Then
                p1 = intIdTesoreriaOrdenesEncabezado
            End If
        Else
            p1 = obj.lngIDTesoreriaEncabezado
        End If

        Me.uspOyDNet_DetalleTesoreriaOrdenes_Actualizar(
            CType(obj.lngIDDetalle, System.Nullable(Of Integer)),
            CType(obj.lngID, System.Nullable(Of Integer)),
            CType(p1, System.Nullable(Of Integer)),
            obj.strTipo, obj.strFormaPago,
            CType(obj.lngIDConcepto, System.Nullable(Of Integer)),
            obj.strDetalleConcepto, obj.NombreConsecutivo,
            obj.strNroDocumento, obj.strTipoDocumento,
            obj.strNombre, CType(obj.curValor, System.Nullable(Of Decimal)),
            CType(obj.logEsTercero, System.Nullable(Of Boolean)),
            obj.ValorTipoGMF,
            obj.ValorTipoCheque,
            CType(obj.ValorTipoCruce, String),
            CType(obj.lngNroCheque, System.Nullable(Of Double)),
            CType(obj.logEsCuentaRegistrada, System.Nullable(Of Boolean)),
            obj.strCuenta,
            CType(obj.lngIdBanco, System.Nullable(Of Integer)),
            obj.strNombreTitular,
            obj.strTipoDocumentoTitular,
            obj.strNroDocumentoTitular,
            CType(obj.curValorGMF, System.Nullable(Of Decimal)),
            obj.strTipoCuenta, CType(obj.dtmFechaDocumento, System.Nullable(Of Date)),
            obj.lngNroEncargo,
            obj.ValorCarteraColectiva, obj.strCodigoOyD, obj.strUsuario,
            obj.infosesion, CType(Nothing, System.Nullable(Of Byte)),
                obj.strIDTipoCliente,
                obj.curValorNeto, Nothing, obj.ValorTotalNota,
                obj.curValorSaldoConsultado,
                 obj.curValorBloqueado,
                obj.strMotivoBloqueado,
                obj.strNaturaleza,
                obj.strDetalleBloqueo,
                String.Empty, 0, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty,
                obj.UsuarioWindows,
                Nothing, Nothing)
    End Sub

    Private Sub InsertTesoreriaOyDPlusInternos(ByVal obj As OyDPLUSTesoreria.TesoreriaOyDPlusInternos)

        Dim p1 As Integer = 0
        If IsNothing(obj.lngIDTesoreriaEncabezado) Or (obj.lngIDTesoreriaEncabezado = 0) Then
            If intIdTesoreriaOrdenesEncabezado <> 0 Then
                p1 = intIdTesoreriaOrdenesEncabezado
            End If
        Else
            p1 = obj.lngIDTesoreriaEncabezado
        End If

        Me.uspOyDNet_DetalleTesoreriaOrdenes_Actualizar(
            CType(obj.lngIDDetalle, System.Nullable(Of Integer)),
            CType(obj.lngID, System.Nullable(Of Integer)),
            CType(p1, System.Nullable(Of Integer)),
            obj.strTipo, obj.strFormaPago,
            CType(obj.lngIDConcepto, System.Nullable(Of Integer)),
            obj.strDetalleConcepto, obj.NombreConsecutivo,
            obj.strNroDocumento, obj.strTipoDocumento,
            obj.strNombre, CType(obj.curValor, System.Nullable(Of Decimal)),
            CType(obj.logEsTercero, System.Nullable(Of Boolean)),
            obj.ValorTipoGMF,
            obj.ValorTipoCheque,
            CType(obj.ValorTipoCruce, String),
            CType(obj.lngNroCheque, System.Nullable(Of Double)),
            CType(obj.logEsCuentaRegistrada, System.Nullable(Of Boolean)),
            obj.strCuenta,
            CType(obj.lngIdBanco, System.Nullable(Of Integer)),
            obj.strNombreTitular,
            obj.strTipoDocumentoTitular,
            obj.strNroDocumentoTitular,
            CType(obj.curValorGMF, System.Nullable(Of Decimal)),
            obj.strTipoCuenta, CType(obj.dtmFechaDocumento, System.Nullable(Of Date)),
            obj.lngNroEncargo,
            obj.strNombreCarteraColectiva, obj.strCodigoOyD, obj.strUsuario,
            obj.infosesion, CType(Nothing, System.Nullable(Of Byte)),
                obj.strIDTipoCliente,
                obj.curValorNeto, Nothing, obj.ValorTotalNota,
                obj.curValorSaldoConsultado,
                 obj.curValorBloqueado,
                obj.strMotivoBloqueado,
                obj.strNaturaleza,
                obj.strDetalleBloqueo,
                String.Empty, 0, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty,
                obj.UsuarioWindows,
                Nothing, Nothing)
    End Sub

    Private Sub UpdateTesoreriaOyDPlusInternos(ByVal obj As OyDPLUSTesoreria.TesoreriaOyDPlusInternos)
        Dim p1 As Integer = 0
        If IsNothing(obj.lngIDTesoreriaEncabezado) Or (obj.lngIDTesoreriaEncabezado = 0) Then
            If intIdTesoreriaOrdenesEncabezado <> 0 Then
                p1 = intIdTesoreriaOrdenesEncabezado
            End If
        Else
            p1 = obj.lngIDTesoreriaEncabezado
        End If

        Me.uspOyDNet_DetalleTesoreriaOrdenes_Actualizar(
           CType(obj.lngIDDetalle, System.Nullable(Of Integer)),
           CType(obj.lngID, System.Nullable(Of Integer)),
           CType(p1, System.Nullable(Of Integer)),
           obj.strTipo, obj.strFormaPago,
           CType(obj.lngIDConcepto, System.Nullable(Of Integer)),
           obj.strDetalleConcepto, obj.NombreConsecutivo,
           obj.strNroDocumento, obj.strTipoDocumento,
           obj.strNombre, CType(obj.curValor, System.Nullable(Of Decimal)),
           CType(obj.logEsTercero, System.Nullable(Of Boolean)),
           obj.ValorTipoGMF,
           obj.ValorTipoCheque,
           CType(obj.ValorTipoCruce, String),
           CType(obj.lngNroCheque, System.Nullable(Of Double)),
           CType(obj.logEsCuentaRegistrada, System.Nullable(Of Boolean)),
           obj.strCuenta,
           CType(obj.lngIdBanco, System.Nullable(Of Integer)),
           obj.strNombreTitular,
           Nothing,
           obj.strNroDocumentoTitular,
           CType(obj.curValorGMF, System.Nullable(Of Decimal)),
           Nothing, CType(obj.dtmFechaDocumento, System.Nullable(Of Date)),
           obj.lngNroEncargo,
           obj.strNombreCarteraColectiva, obj.strCodigoOyD, obj.strUsuario,
           obj.infosesion, CType(Nothing, System.Nullable(Of Byte)),
           obj.strIDTipoCliente,
           obj.curValorNeto, Nothing, obj.ValorTotalNota,
                obj.curValorSaldoConsultado,
                 obj.curValorBloqueado,
                obj.strMotivoBloqueado,
                obj.strNaturaleza,
                obj.strDetalleBloqueo,
                String.Empty, 0, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty,
                obj.UsuarioWindows,
                Nothing, Nothing)
    End Sub

    Private Sub InsertTesoreriaOyDPlusBloqueos(ByVal obj As OyDPLUSTesoreria.TesoreriaOyDPlusBloqueos)

        Dim p1 As Integer = 0
        If IsNothing(obj.lngIDTesoreriaEncabezado) Or (obj.lngIDTesoreriaEncabezado = 0) Then
            If intIdTesoreriaOrdenesEncabezado <> 0 Then
                p1 = intIdTesoreriaOrdenesEncabezado
            End If
        Else
            p1 = obj.lngIDTesoreriaEncabezado
        End If

        Me.uspOyDNet_DetalleTesoreriaOrdenes_Actualizar(
            CType(obj.lngIDDetalle, System.Nullable(Of Integer)),
            CType(obj.lngID, System.Nullable(Of Integer)),
            CType(p1, System.Nullable(Of Integer)),
            obj.strTipo, obj.strFormaPago,
            CType(obj.lngIDConcepto, System.Nullable(Of Integer)),
            obj.strDetalleConcepto, obj.NombreConsecutivo,
            obj.strNroDocumento, obj.strTipoDocumento,
            obj.strNombre, CType(obj.curValor, System.Nullable(Of Decimal)),
            CType(obj.logEsTercero, System.Nullable(Of Boolean)),
            obj.ValorTipoGMF,
            obj.ValorTipoCheque,
            CType(obj.ValorTipoCruce, String),
            CType(obj.lngNroCheque, System.Nullable(Of Double)),
            CType(obj.logEsCuentaRegistrada, System.Nullable(Of Boolean)),
            obj.strCuenta,
            CType(obj.lngIdBanco, System.Nullable(Of Integer)),
            obj.strNombreTitular,
            obj.strTipoDocumentoTitular,
            obj.strNroDocumentoTitular,
            CType(obj.curValorGMF, System.Nullable(Of Decimal)),
            obj.strTipoCuenta, CType(obj.dtmFechaDocumento, System.Nullable(Of Date)),
            obj.lngNroEncargo,
            obj.strNombreCarteraColectiva, obj.strCodigoOyD, obj.strUsuario,
            obj.infosesion, CType(Nothing, System.Nullable(Of Byte)),
                obj.strIDTipoCliente,
                obj.curValorNeto, Nothing, obj.ValorTotalNota,
                obj.curValorSaldoConsultado,
                 obj.curValorBloqueado,
                obj.strMotivoBloqueo,
                obj.strNaturaleza,
                obj.strDetalleBloqueo,
                obj.strCarteraColectivaFondos,
                obj.intNroEncargoFondos, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty,
                obj.UsuarioWindows,
                Nothing, Nothing)
    End Sub

    Private Sub UpdateTesoreriaOyDPlusBloqueos(ByVal obj As OyDPLUSTesoreria.TesoreriaOyDPlusBloqueos)
        Dim p1 As Integer = 0
        If IsNothing(obj.lngIDTesoreriaEncabezado) Or (obj.lngIDTesoreriaEncabezado = 0) Then
            If intIdTesoreriaOrdenesEncabezado <> 0 Then
                p1 = intIdTesoreriaOrdenesEncabezado
            End If
        Else
            p1 = obj.lngIDTesoreriaEncabezado
        End If

        Me.uspOyDNet_DetalleTesoreriaOrdenes_Actualizar(
           CType(obj.lngIDDetalle, System.Nullable(Of Integer)),
           CType(obj.lngID, System.Nullable(Of Integer)),
           CType(p1, System.Nullable(Of Integer)),
           obj.strTipo, obj.strFormaPago,
           CType(obj.lngIDConcepto, System.Nullable(Of Integer)),
           obj.strDetalleConcepto, obj.NombreConsecutivo,
           obj.strNroDocumento, obj.strTipoDocumento,
           obj.strNombre, CType(obj.curValor, System.Nullable(Of Decimal)),
           CType(obj.logEsTercero, System.Nullable(Of Boolean)),
           obj.ValorTipoGMF,
           obj.ValorTipoCheque,
           CType(obj.ValorTipoCruce, String),
           CType(obj.lngNroCheque, System.Nullable(Of Double)),
           CType(obj.logEsCuentaRegistrada, System.Nullable(Of Boolean)),
           obj.strCuenta,
           CType(obj.lngIdBanco, System.Nullable(Of Integer)),
           obj.strNombreTitular,
           Nothing,
           obj.strNroDocumentoTitular,
           CType(obj.curValorGMF, System.Nullable(Of Decimal)),
           Nothing, CType(obj.dtmFechaDocumento, System.Nullable(Of Date)),
           obj.lngNroEncargo,
           obj.strNombreCarteraColectiva, obj.strCodigoOyD, obj.strUsuario,
           obj.infosesion, CType(Nothing, System.Nullable(Of Byte)),
           obj.strIDTipoCliente,
           obj.curValorNeto, Nothing, obj.ValorTotalNota,
                obj.curValorSaldoConsultado,
                 obj.curValorBloqueado,
                obj.strMotivoBloqueo,
                obj.strNaturaleza,
                obj.strDetalleBloqueo,
                obj.strCarteraColectivaFondos,
                obj.intNroEncargoFondos, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty,
                obj.UsuarioWindows,
                Nothing, Nothing)
    End Sub

    Private Sub InsertTesoreriaOyDPlusOperacionesEspeciales(ByVal obj As OyDPLUSTesoreria.TesoreriaOyDPlusOperacionesEspeciales)

        Dim p1 As Integer = 0
        If IsNothing(obj.lngIDTesoreriaEncabezado) Or (obj.lngIDTesoreriaEncabezado = 0) Then
            If intIdTesoreriaOrdenesEncabezado <> 0 Then
                p1 = intIdTesoreriaOrdenesEncabezado
            End If
        Else
            p1 = obj.lngIDTesoreriaEncabezado
        End If

        Me.uspOyDNet_DetalleTesoreriaOrdenes_Actualizar(
            CType(obj.lngIDDetalle, System.Nullable(Of Integer)),
            CType(obj.lngID, System.Nullable(Of Integer)),
            CType(p1, System.Nullable(Of Integer)),
            obj.strTipo, obj.strFormaPago,
            CType(obj.lngIDConcepto, System.Nullable(Of Integer)),
            obj.strDetalleConcepto, obj.NombreConsecutivo,
            obj.strNroDocumento, obj.strTipoDocumento,
            obj.strNombre, CType(obj.curValor, System.Nullable(Of Decimal)),
            False,
            Nothing,
            Nothing,
            Nothing,
            Nothing,
            Nothing,
            String.Empty,
            Nothing,
            obj.strNombreTitular,
            obj.strTipoDocumentoTitular,
            obj.strNroDocumentoTitular,
            0,
            String.Empty, CType(obj.dtmFechaDocumento, System.Nullable(Of Date)),
            Nothing,
            String.Empty, obj.strCodigoOyD, obj.strUsuario,
            obj.infosesion, CType(Nothing, System.Nullable(Of Byte)),
            String.Empty,
            0, Nothing, obj.ValorTotalNota,
            obj.curValorSaldoConsultado,
            0,
            String.Empty,
            String.Empty,
            String.Empty,
            String.Empty,
            0,
            obj.ValorTipoOperacionEspecial,
            obj.ValorOperacionEspecial,
            obj.strCodigoOyD,
            obj.strProvieneDinero, String.Empty,
                obj.UsuarioWindows,
                Nothing, Nothing)
    End Sub

    Private Sub UpdateTesoreriaOyDPlusOperacionesEspeciales(ByVal obj As OyDPLUSTesoreria.TesoreriaOyDPlusOperacionesEspeciales)
        Dim p1 As Integer = 0
        If IsNothing(obj.lngIDTesoreriaEncabezado) Or (obj.lngIDTesoreriaEncabezado = 0) Then
            If intIdTesoreriaOrdenesEncabezado <> 0 Then
                p1 = intIdTesoreriaOrdenesEncabezado
            End If
        Else
            p1 = obj.lngIDTesoreriaEncabezado
        End If

        Me.uspOyDNet_DetalleTesoreriaOrdenes_Actualizar(
            CType(obj.lngIDDetalle, System.Nullable(Of Integer)),
            CType(obj.lngID, System.Nullable(Of Integer)),
            CType(p1, System.Nullable(Of Integer)),
            obj.strTipo, obj.strFormaPago,
            CType(obj.lngIDConcepto, System.Nullable(Of Integer)),
            obj.strDetalleConcepto, obj.NombreConsecutivo,
            obj.strNroDocumento, obj.strTipoDocumento,
            obj.strNombre, CType(obj.curValor, System.Nullable(Of Decimal)),
            False,
            Nothing,
            Nothing,
            Nothing,
            Nothing,
            Nothing,
            String.Empty,
            Nothing,
            obj.strNombreTitular,
            obj.strTipoDocumentoTitular,
            obj.strNroDocumentoTitular,
            0,
            String.Empty, CType(obj.dtmFechaDocumento, System.Nullable(Of Date)),
            Nothing,
            String.Empty, obj.strCodigoOyD, obj.strUsuario,
            obj.infosesion, CType(Nothing, System.Nullable(Of Byte)),
            String.Empty,
            0, Nothing, obj.ValorTotalNota,
            obj.curValorSaldoConsultado,
            0,
            String.Empty,
            String.Empty,
            String.Empty,
            String.Empty,
            0,
            obj.ValorTipoOperacionEspecial,
            obj.ValorOperacionEspecial,
            obj.strCodigoOyD,
            obj.strProvieneDinero, String.Empty,
                obj.UsuarioWindows,
                Nothing, Nothing)
    End Sub

#Region "Ordenes Recibo"
    Private Sub InsertTesoreriaOyDPlusChequesRecibo(ByVal obj As OyDPLUSTesoreria.TesoreriaOyDPlusChequesRecibo)
        Dim p1 As Integer = 0
        Dim p2 As System.Nullable(Of Integer) = obj.lngID
        If IsNothing(obj.lngIDTesoreriaEncabezado) Or (obj.lngIDTesoreriaEncabezado = 0) Then
            If intIdTesoreriaOrdenesEncabezado <> 0 Then
                p1 = intIdTesoreriaOrdenesEncabezado
            End If
        Else
            p1 = obj.lngIDTesoreriaEncabezado
        End If
        Me.uspOyDNet_DetalleTesoreriaOrdenesRecibo_Actualizar(
            CType(obj.lngIDDetalle, System.Nullable(Of Integer)),
            p2,
            CType(p1, System.Nullable(Of Integer)),
            obj.strTipo, Nothing, Nothing, obj.strFormaPago,
            CType(obj.curValor, System.Nullable(Of Decimal)),
            CType(obj.lngNroCheque, System.Nullable(Of Double)),
            CType(obj.lngIDBanco, System.Nullable(Of Integer)),
            CType(obj.logClienteTrae, System.Nullable(Of Boolean)),
            CType(obj.logDireccionCliente, System.Nullable(Of Boolean)),
            CType(obj.logOtraDireccion, System.Nullable(Of Boolean)),
            obj.strDireccionCheque,
            obj.strTelefono,
            obj.strCiudad,
            obj.strSector,
            obj.strEscaneo,
            CType(Nothing, String), Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing,
                obj.UsuarioWindows, Nothing)
        obj.lngID = p2.GetValueOrDefault
    End Sub

    Private Sub UpdateTesoreriaOyDPlusChequesRecibo(ByVal obj As OyDPLUSTesoreria.TesoreriaOyDPlusChequesRecibo)
        Dim p1 As Integer = 0
        Dim p2 As System.Nullable(Of Integer) = obj.lngID
        If IsNothing(obj.lngIDTesoreriaEncabezado) Or (obj.lngIDTesoreriaEncabezado = 0) Then
            If intIdTesoreriaOrdenesEncabezado <> 0 Then
                p1 = intIdTesoreriaOrdenesEncabezado
            End If
        Else
            p1 = obj.lngIDTesoreriaEncabezado
        End If
        Me.uspOyDNet_DetalleTesoreriaOrdenesRecibo_Actualizar(
            CType(obj.lngIDDetalle, System.Nullable(Of Integer)),
            p2,
            CType(p1, System.Nullable(Of Integer)),
            obj.strTipo, Nothing, Nothing, obj.strFormaPago,
            CType(obj.curValor, System.Nullable(Of Decimal)),
            CType(obj.lngNroCheque, System.Nullable(Of Double)),
            CType(obj.lngIDBanco, System.Nullable(Of Integer)),
            CType(obj.logClienteTrae, System.Nullable(Of Boolean)),
            CType(obj.logDireccionCliente, System.Nullable(Of Boolean)),
            CType(obj.logOtraDireccion, System.Nullable(Of Boolean)),
            obj.strDireccionCheque,
            obj.strTelefono,
            obj.strCiudad,
            obj.strSector,
            obj.strEscaneo,
             CType(Nothing, String), Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing,
                obj.UsuarioWindows, Nothing)
        obj.lngID = p2.GetValueOrDefault
    End Sub

    Private Sub InsertTesoreriaOyDPlusTransferenciasRecibo(ByVal obj As OyDPLUSTesoreria.TesoreriaOyDPlusTransferenciasRecibo)
        Dim p1 As Integer = 0
        Dim p2 As System.Nullable(Of Integer) = obj.lngID
        If IsNothing(obj.lngIDTesoreriaEncabezado) Or (obj.lngIDTesoreriaEncabezado = 0) Then
            If intIdTesoreriaOrdenesEncabezado <> 0 Then
                p1 = intIdTesoreriaOrdenesEncabezado
            End If
        Else
            p1 = obj.lngIDTesoreriaEncabezado
        End If
        Me.uspOyDNet_DetalleTesoreriaOrdenesRecibo_Actualizar(CType(obj.lngIDDetalle, System.Nullable(Of Integer)), p2, p1, obj.strTipo, CType(Nothing, String), CType(Nothing, String), obj.strFormaPago, CType(obj.curValor, System.Nullable(Of Decimal)), Nothing, CType(obj.lngIDBanco, System.Nullable(Of Integer)), Nothing, Nothing, Nothing, obj.strDireccionCheque, obj.strTelefono, obj.strCiudad, obj.strSector, obj.strEscaneo, CType(Nothing, String), obj.strCuentaOrigen, obj.strCuentadestino, obj.lngIdBancoDestino, obj.ValorTipoCuentaOrigen, obj.ValorTipoCuentaDestino, Nothing, Nothing, obj.logEsCuentaRegistrada, Nothing, Nothing, Nothing, Nothing, Nothing,
                obj.UsuarioWindows, Nothing)
        obj.lngID = p2.GetValueOrDefault
    End Sub

    Private Sub UpdateTesoreriaOyDPlusTransferenciasRecibo(ByVal obj As OyDPLUSTesoreria.TesoreriaOyDPlusTransferenciasRecibo)
        Dim p1 As Integer = 0
        Dim p2 As System.Nullable(Of Integer) = obj.lngID
        If IsNothing(obj.lngIDTesoreriaEncabezado) Or (obj.lngIDTesoreriaEncabezado = 0) Then
            If intIdTesoreriaOrdenesEncabezado <> 0 Then
                p1 = intIdTesoreriaOrdenesEncabezado
            End If
        Else
            p1 = obj.lngIDTesoreriaEncabezado
        End If
        Me.uspOyDNet_DetalleTesoreriaOrdenesRecibo_Actualizar(CType(obj.lngIDDetalle, System.Nullable(Of Integer)), p2, p1, obj.strTipo, CType(Nothing, String), CType(Nothing, String), obj.strFormaPago, CType(obj.curValor, System.Nullable(Of Decimal)), Nothing, CType(obj.lngIDBanco, System.Nullable(Of Integer)), Nothing, Nothing, Nothing, obj.strDireccionCheque, obj.strTelefono, obj.strCiudad, obj.strSector, obj.strEscaneo, CType(Nothing, String), obj.strCuentaOrigen, obj.strCuentadestino, obj.lngIdBancoDestino, obj.ValorTipoCuentaOrigen, obj.ValorTipoCuentaDestino, Nothing, Nothing, obj.logEsCuentaRegistrada, Nothing, Nothing, Nothing, Nothing, Nothing,
                obj.UsuarioWindows, Nothing)
        obj.lngID = p2.GetValueOrDefault
    End Sub

    Private Sub InsertTesoreriaOyDPlusConsignacionesRecibo(ByVal obj As OyDPLUSTesoreria.TesoreriaOyDPlusConsignacionesRecibo)
        Dim p1 As Integer = 0
        Dim p2 As System.Nullable(Of Integer) = obj.lngID
        If IsNothing(obj.lngIDTesoreriaEncabezado) Or (obj.lngIDTesoreriaEncabezado = 0) Then
            If intIdTesoreriaOrdenesEncabezado <> 0 Then
                p1 = intIdTesoreriaOrdenesEncabezado
            End If
        Else
            p1 = obj.lngIDTesoreriaEncabezado
        End If
        Me.uspOyDNet_DetalleTesoreriaOrdenesRecibo_Actualizar(CType(obj.lngIDDetalle, System.Nullable(Of Integer)), p2, p1, obj.strTipo, CType(Nothing, String), CType(Nothing, String), obj.strFormaPago, CType(obj.curValor, System.Nullable(Of Decimal)), CType(obj.lngNroCheque, System.Nullable(Of Double)), CType(obj.lngIDBanco, System.Nullable(Of Integer)), CType(Nothing, System.Nullable(Of Boolean)), CType(Nothing, System.Nullable(Of Boolean)), CType(Nothing, System.Nullable(Of Boolean)), CType(Nothing, String), CType(Nothing, String), CType(Nothing, String), CType(Nothing, String), obj.strEscaneo, CType(Nothing, String), obj.strCuentaOrigen, obj.strCuentadestino, CType(obj.lngIdBancoDestino, System.Nullable(Of Integer)), obj.strTipoCuentaOrigen, CType(Nothing, String), CType(obj.lngNroReferencia, System.Nullable(Of Double)), obj.ValorFormaConsignacion, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing,
                obj.UsuarioWindows, Nothing)
        obj.lngID = p2.GetValueOrDefault
    End Sub

    Private Sub UpdateTesoreriaOyDPlusConsignacionesRecibo(ByVal obj As OyDPLUSTesoreria.TesoreriaOyDPlusConsignacionesRecibo)
        Dim p1 As Integer = 0
        Dim p2 As System.Nullable(Of Integer) = obj.lngID
        If IsNothing(obj.lngIDTesoreriaEncabezado) Or (obj.lngIDTesoreriaEncabezado = 0) Then
            If intIdTesoreriaOrdenesEncabezado <> 0 Then
                p1 = intIdTesoreriaOrdenesEncabezado
            End If
        Else
            p1 = obj.lngIDTesoreriaEncabezado
        End If
        Me.uspOyDNet_DetalleTesoreriaOrdenesRecibo_Actualizar(CType(obj.lngIDDetalle, System.Nullable(Of Integer)), p2, p1, obj.strTipo, CType(Nothing, String), CType(Nothing, String), obj.strFormaPago, CType(obj.curValor, System.Nullable(Of Decimal)), CType(obj.lngNroCheque, System.Nullable(Of Double)), CType(obj.lngIDBanco, System.Nullable(Of Integer)), CType(Nothing, System.Nullable(Of Boolean)), CType(Nothing, System.Nullable(Of Boolean)), CType(Nothing, System.Nullable(Of Boolean)), CType(Nothing, String), CType(Nothing, String), CType(Nothing, String), CType(Nothing, String), obj.strEscaneo, CType(Nothing, String), obj.strCuentaOrigen, obj.strCuentadestino, CType(obj.lngIdBancoDestino, System.Nullable(Of Integer)), obj.strTipoCuentaOrigen, CType(Nothing, String), CType(obj.lngNroReferencia, System.Nullable(Of Double)), obj.ValorFormaConsignacion, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing,
                obj.UsuarioWindows, Nothing)
        obj.lngID = p2.GetValueOrDefault
    End Sub

    Private Sub InsertTesoreriaOyDPlusCargosPagosARecibo(ByVal obj As OyDPLUSTesoreria.TesoreriaOyDPlusCargosPagosARecibo)
        Dim p1 As Integer = 0
        Dim p2 As System.Nullable(Of Integer) = obj.lngID
        If IsNothing(obj.lngIDTesoreriaEncabezado) Or (obj.lngIDTesoreriaEncabezado = 0) Then
            If intIdTesoreriaOrdenesEncabezado <> 0 Then
                p1 = intIdTesoreriaOrdenesEncabezado
            End If
        Else
            p1 = obj.lngIDTesoreriaEncabezado
        End If
        Me.uspOyDNet_DetalleTesoreriaOrdenesRecibo_Actualizar(CType(obj.lngIDDetalle, System.Nullable(Of Integer)), p2, p1, obj.strTipo, obj.strCodigoOyD, obj.ValorTipoCliente, obj.strFormaPago, CType(obj.curValor, System.Nullable(Of Decimal)), CType(Nothing, System.Nullable(Of Integer)), CType(Nothing, System.Nullable(Of Integer)), CType(Nothing, System.Nullable(Of Boolean)), CType(Nothing, System.Nullable(Of Boolean)), CType(Nothing, System.Nullable(Of Boolean)), CType(Nothing, String), CType(Nothing, String), CType(Nothing, String), CType(Nothing, String), CType(Nothing, String), CType(Nothing, String), Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, obj.strDetalleConcepto, obj.lngIDConcepto, Nothing, Nothing, Nothing,
                obj.UsuarioWindows, Nothing)
        obj.lngID = p2.GetValueOrDefault
    End Sub

    Private Sub UpdateTesoreriaOyDPlusCargosPagosARecibo(ByVal obj As OyDPLUSTesoreria.TesoreriaOyDPlusCargosPagosARecibo)
        Dim p1 As Integer = 0
        Dim p2 As System.Nullable(Of Integer) = obj.lngID
        If IsNothing(obj.lngIDTesoreriaEncabezado) Or (obj.lngIDTesoreriaEncabezado = 0) Then
            If intIdTesoreriaOrdenesEncabezado <> 0 Then
                p1 = intIdTesoreriaOrdenesEncabezado
            End If
        Else
            p1 = obj.lngIDTesoreriaEncabezado
        End If
        Me.uspOyDNet_DetalleTesoreriaOrdenesRecibo_Actualizar(CType(obj.lngIDDetalle, System.Nullable(Of Integer)), p2, p1, obj.strTipo, obj.strCodigoOyD, obj.ValorTipoCliente, obj.strFormaPago, CType(obj.curValor, System.Nullable(Of Decimal)), CType(Nothing, System.Nullable(Of Integer)), CType(Nothing, System.Nullable(Of Integer)), CType(Nothing, System.Nullable(Of Boolean)), CType(Nothing, System.Nullable(Of Boolean)), CType(Nothing, System.Nullable(Of Boolean)), CType(Nothing, String), CType(Nothing, String), CType(Nothing, String), CType(Nothing, String), CType(Nothing, String), CType(Nothing, String), Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, obj.strDetalleConcepto, obj.lngIDConcepto, Nothing, Nothing, Nothing,
                obj.UsuarioWindows, Nothing)
        obj.lngID = p2.GetValueOrDefault
    End Sub

    Private Sub InsertTesoreriaOyDPlusCargosPagosAFondosRecibo(ByVal obj As OyDPLUSTesoreria.TesoreriaOyDPlusCargosPagosAFondosRecibo)
        Dim p1 As Integer = 0
        Dim p2 As System.Nullable(Of Integer) = obj.lngID
        If IsNothing(obj.lngIDTesoreriaEncabezado) Or (obj.lngIDTesoreriaEncabezado = 0) Then
            If intIdTesoreriaOrdenesEncabezado <> 0 Then
                p1 = intIdTesoreriaOrdenesEncabezado
            End If
        Else
            p1 = obj.lngIDTesoreriaEncabezado
        End If
        Me.uspOyDNet_DetalleTesoreriaOrdenesRecibo_Actualizar(CType(obj.lngIDDetalle, System.Nullable(Of Integer)), p2, p1, obj.strTipo, obj.strCodigoOyD, obj.ValorTipoCliente, obj.strFormaPago, CType(obj.curValor, System.Nullable(Of Decimal)), CType(Nothing, System.Nullable(Of Integer)), CType(Nothing, System.Nullable(Of Integer)), CType(Nothing, System.Nullable(Of Boolean)), CType(Nothing, System.Nullable(Of Boolean)), CType(Nothing, System.Nullable(Of Boolean)), CType(Nothing, String), CType(Nothing, String), CType(Nothing, String), CType(Nothing, String), CType(Nothing, String), CType(Nothing, String), Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, obj.strDetalleConcepto, obj.lngIDConcepto, obj.strTipoAccionFondos, obj.strCarteraColectivaFondos, obj.intNroEncargoFondos,
                obj.UsuarioWindows, obj.DescripcionEncargoFondos)
        obj.lngID = p2.GetValueOrDefault
    End Sub

    Private Sub UpdateTesoreriaOyDPlusCargosPagosAFondosRecibo(ByVal obj As OyDPLUSTesoreria.TesoreriaOyDPlusCargosPagosAFondosRecibo)
        Dim p1 As Integer = 0
        Dim p2 As System.Nullable(Of Integer) = obj.lngID
        If IsNothing(obj.lngIDTesoreriaEncabezado) Or (obj.lngIDTesoreriaEncabezado = 0) Then
            If intIdTesoreriaOrdenesEncabezado <> 0 Then
                p1 = intIdTesoreriaOrdenesEncabezado
            End If
        Else
            p1 = obj.lngIDTesoreriaEncabezado
        End If
        Me.uspOyDNet_DetalleTesoreriaOrdenesRecibo_Actualizar(CType(obj.lngIDDetalle, System.Nullable(Of Integer)), p2, p1, obj.strTipo, obj.strCodigoOyD, obj.ValorTipoCliente, obj.strFormaPago, CType(obj.curValor, System.Nullable(Of Decimal)), CType(Nothing, System.Nullable(Of Integer)), CType(Nothing, System.Nullable(Of Integer)), CType(Nothing, System.Nullable(Of Boolean)), CType(Nothing, System.Nullable(Of Boolean)), CType(Nothing, System.Nullable(Of Boolean)), CType(Nothing, String), CType(Nothing, String), CType(Nothing, String), CType(Nothing, String), CType(Nothing, String), CType(Nothing, String), Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, obj.strDetalleConcepto, obj.lngIDConcepto, obj.strTipoAccionFondos, obj.strCarteraColectivaFondos, obj.intNroEncargoFondos,
                obj.UsuarioWindows, obj.DescripcionEncargoFondos)
        obj.lngID = p2.GetValueOrDefault
    End Sub
#End Region

End Class
