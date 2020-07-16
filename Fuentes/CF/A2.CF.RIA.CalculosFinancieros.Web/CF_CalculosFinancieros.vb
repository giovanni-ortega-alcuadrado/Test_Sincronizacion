Imports System.Data.SqlClient
Imports System.Data.Linq
Imports System.Reflection
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports A2.OyD.Infraestructura

Partial Public Class CalculosFinancierosDBML
    Public Sub New()
        MyBase.New(ObtenerCadenaConexion(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString), mappingSource)
        OnCreated()
    End Sub
    Public Overrides Sub SubmitChanges(failureMode As Data.Linq.ConflictMode)
        Try
            MyBase.SubmitChanges(failureMode)
        Catch ex As Exception
            ManejarError(ex, "CalculosFinancierosDBML", "SubmitChanges")
        End Try
    End Sub

    'JAEZ 20161001
    Public Sub ActualizarModulo(ByVal pstrModulo As String)

        'MyBase.New(A2Utilidades.Cifrar.descifrar(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString), mappingSource)

        Dim strConnection As String
        strConnection = ajustarConexion(A2Utilidades.Cifrar.descifrar(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString), pstrModulo)
        MyBase.Connection.ConnectionString = strConnection

        OnCreated()
    End Sub



#Region "PreciosEspecies"

    Private Sub InsertPreciosEspecies(ByVal obj As PreciosEspecies)
        Dim p1 As Integer = obj.lngID
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_PreciosEspecies_Actualizar(
            p1,
            obj.strIDEspecie,
            obj.strISIN,
            CType(obj.intNroEmision, System.Nullable(Of Integer)),
            obj.dtmFechaArchivo,
            obj.dtmEmision,
            obj.dtmVencimiento,
            obj.strModalidad,
            obj.strMoneda,
            CType(obj.dblSpread, System.Nullable(Of Double)),
            obj.strTasaRef,
            CType(obj.dblPrecio, System.Nullable(Of Double)),
            obj.strTipoInversion,
            obj.strUsuario,
            obj.InfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2,
            CType(obj.dblPrecioLimpio, System.Nullable(Of Double)),
            obj.strProveedor)
        obj.lngID = p1
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub UpdatePreciosEspecies(ByVal obj As PreciosEspecies)
        Dim p1 As System.Nullable(Of Integer) = obj.lngID
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_PreciosEspecies_Actualizar(
            p1,
            obj.strIDEspecie,
            obj.strISIN,
            CType(obj.intNroEmision, System.Nullable(Of Integer)),
            obj.dtmFechaArchivo,
            obj.dtmEmision,
            obj.dtmVencimiento,
            obj.strModalidad,
            obj.strMoneda,
            CType(obj.dblSpread, System.Nullable(Of Double)),
            obj.strTasaRef,
            CType(obj.dblPrecio, System.Nullable(Of Double)),
            obj.strTipoInversion,
            obj.strUsuario,
            obj.InfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2,
            CType(obj.dblPrecioLimpio, System.Nullable(Of Double)),
            obj.strProveedor)
        obj.lngID = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub DeletePreciosEspecies(ByVal obj As PreciosEspecies)
        Dim p1 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_PreciosEspecies_Eliminar(
            CType(obj.lngID, System.Nullable(Of Integer)),
            obj.strUsuario,
            obj.InfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p1)
        obj.strMsgValidacion = p1
        If Not String.IsNullOrEmpty(p1) Then
            Throw New Exception(p1, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
        End If
    End Sub

#End Region

#Region "Indicadores"
    Private Sub InsertIndicadores(ByVal obj As Indicadores)
        Dim p1 As Integer = obj.lngID
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_Indicadores_Actualizar(
            p1,
            obj.strTipoIndicador,
            obj.strTasaMoneda,
            obj.dtmFechaArchivo,
            CType(obj.dblValor, System.Nullable(Of Double)),
            obj.strUsuario,
            obj.InfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.lngID = p1
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub UpdateIndicadores(ByVal obj As Indicadores)
        Dim p1 As System.Nullable(Of Integer) = obj.lngID
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_Indicadores_Actualizar(
            p1,
            obj.strTipoIndicador,
            obj.strTasaMoneda,
            obj.dtmFechaArchivo,
            CType(obj.dblValor, System.Nullable(Of Double)),
            obj.strUsuario,
            obj.InfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.lngID = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub DeleteIndicadores(ByVal obj As Indicadores)
        Dim p1 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_Indicadores_Eliminar(
            CType(obj.lngID, System.Nullable(Of Integer)),
            CType(obj.strTipoIndicador, String),
            obj.strUsuario,
            obj.InfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p1)
        obj.strMsgValidacion = p1
        If Not String.IsNullOrEmpty(p1) Then
            Throw New Exception(p1, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
        End If
    End Sub

#End Region

#Region "MovimientosParticipacionesFondos"
    Private Sub DeleteMovimientosParticipacionesFondos(ByVal obj As CFCalculosFinancieros.MovimientosParticipacionesFondos)
        Dim p1 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_MovimientosParticipacionesFondo_Eliminar(
                obj.intCuenta.ToString(),
                obj.lngIDPortafolio,
                obj.strFondo,
                obj.strUsuario,
                obj.strInfoSesion,
                CType(Nothing, System.Nullable(Of Byte)),
                p1)
        obj.strMsgValidacion = p1
        If Not String.IsNullOrEmpty(p1) Then
            Throw New Exception(p1, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
        End If
    End Sub
#End Region

#Region "Companias"

    Private Sub DeleteCompanias(ByVal obj As Companias)
        Dim p1 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_Companias_Eliminar(CType(obj.intIDCompania, System.Nullable(Of Integer)),
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p1)
        obj.strMsgValidacion = p1
        If Not String.IsNullOrEmpty(p1) Then
            Throw New Exception(p1, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
        End If
    End Sub

#End Region

#Region "MvtoCustodiasAplicados"

    Private Sub DeleteMvtoCustodiasAplicados(ByVal obj As CFCalculosFinancieros.MvtoCustodiasAplicados)
        Dim p1 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_AdministracionMovimientosTitulos_Eliminar(
            CType(obj.intID, System.Nullable(Of Integer)),
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p1)
        obj.strMsgValidacion = p1
        If Not String.IsNullOrEmpty(p1) Then
            Throw New Exception(p1, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
        End If
    End Sub

#End Region

#Region "ConfiguracionArbitraje"

    Private Sub InsertConfiguracionArbitraje(ByVal obj As CFCalculosFinancieros.ConfiguracionArbitraje)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDConfiguracionArbitraje
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_ConfiguracionArbitraje_Actualizar(
            p1,
            obj.strIDEspecie,
            obj.strTipo,
            obj.dtmFechaRegistro,
            obj.dtmFechaVigencia,
            obj.dblUnidades,
            obj.intIDEstadosConceptoTitulos,
            obj.intIDEstadosConceptoTitulosD,
            CType(Nothing, String),
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIDConfiguracionArbitraje = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub UpdateConfiguracionArbitraje(ByVal obj As CFCalculosFinancieros.ConfiguracionArbitraje)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDConfiguracionArbitraje
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_ConfiguracionArbitraje_Actualizar(
            p1,
            obj.strIDEspecie,
            obj.strTipo,
            obj.dtmFechaRegistro,
            obj.dtmFechaVigencia,
            obj.dblUnidades,
            obj.intIDEstadosConceptoTitulos,
            obj.intIDEstadosConceptoTitulosD,
            CType(Nothing, String),
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIDConfiguracionArbitraje = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub DeleteConfiguracionArbitraje(ByVal obj As CFCalculosFinancieros.ConfiguracionArbitraje)
        Dim p1 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_ConfiguracionArbitraje_Eliminar(
            CType(obj.intIDConfiguracionArbitraje, System.Nullable(Of Integer)),
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p1)
        obj.strMsgValidacion = p1
        If Not String.IsNullOrEmpty(p1) Then
            Throw New Exception(p1, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
        End If
    End Sub

#End Region

#Region "Arbitrajes"

    Private Sub InsertArbitrajes(ByVal obj As CFCalculosFinancieros.Arbitrajes)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDArbitrajes
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_Arbitrajes_Actualizar(
            p1,
            obj.strIDEspecie,
            CType(obj.logConstruir, System.Nullable(Of Boolean)),
            obj.dtmFechaProceso,
            CType(Nothing, String),
            CType(obj.intIDEstadosConceptoTitulos, System.Nullable(Of Integer)),
            CType(obj.logMGC, System.Nullable(Of Boolean)),
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIDArbitrajes = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub UpdateArbitrajes(ByVal obj As CFCalculosFinancieros.Arbitrajes)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDArbitrajes
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_Arbitrajes_Actualizar(
            p1,
            obj.strIDEspecie,
            CType(obj.logConstruir, System.Nullable(Of Boolean)),
            obj.dtmFechaProceso,
            CType(Nothing, String),
            CType(obj.intIDEstadosConceptoTitulos, System.Nullable(Of Integer)),
            CType(obj.logMGC, System.Nullable(Of Boolean)),
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIDArbitrajes = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub DeleteArbitrajes(ByVal obj As CFCalculosFinancieros.Arbitrajes)
        Dim p1 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_Arbitrajes_Eliminar(
            CType(obj.intIDArbitrajes, System.Nullable(Of Integer)),
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p1)
        obj.strMsgValidacion = p1
        If Not String.IsNullOrEmpty(p1) Then
            Throw New Exception(p1, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
        End If
    End Sub

#End Region

#Region "ControlLiquidezOperacion"

    Private Sub InsertControlLiquidezOperaciones(ByVal obj As CFCalculosFinancieros.ControlLiquidezOperaciones)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDControlLiquidezOperaciones
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_ControlLiquidezOperaciones_Actualizar(
            p1,
            CType(obj.intIDCompania, System.Nullable(Of Integer)),
            obj.lngIDComitente,
            obj.dtmFechaProceso,
            obj.strTipoFechaProceso,
            obj.strModulo,
            obj.strSubmodulo,
            CType(obj.lngIDMoneda, System.Nullable(Of Integer)),
            CType(obj.dblSaldo, System.Nullable(Of Double)),
            CType(obj.dblCompras, System.Nullable(Of Double)),
            CType(obj.dblVentas, System.Nullable(Of Double)),
            CType(obj.dblTotal, System.Nullable(Of Double)),
            CType(Nothing, String),
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIDControlLiquidezOperaciones = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub UpdateControlLiquidezOperaciones(ByVal obj As CFCalculosFinancieros.ControlLiquidezOperaciones)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDControlLiquidezOperaciones
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_ControlLiquidezOperaciones_Actualizar(
            p1,
            CType(obj.intIDCompania, System.Nullable(Of Integer)),
            obj.lngIDComitente,
            obj.dtmFechaProceso,
            obj.strTipoFechaProceso,
            obj.strModulo,
            obj.strSubmodulo,
            CType(obj.lngIDMoneda, System.Nullable(Of Integer)),
            CType(obj.dblSaldo, System.Nullable(Of Double)),
            CType(obj.dblCompras, System.Nullable(Of Double)),
            CType(obj.dblVentas, System.Nullable(Of Double)),
            CType(obj.dblTotal, System.Nullable(Of Double)),
            CType(Nothing, String),
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIDControlLiquidezOperaciones = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub DeleteControlLiquidezOperaciones(ByVal obj As CFCalculosFinancieros.ControlLiquidezOperaciones)
        Dim p1 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_ControlLiquidezOperaciones_Eliminar(
            CType(obj.intIDControlLiquidezOperaciones, System.Nullable(Of Integer)),
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p1)
        obj.strMsgValidacion = p1
        If Not String.IsNullOrEmpty(p1) Then
            Throw New Exception(p1, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
        End If
    End Sub

#End Region

#Region "ConceptosRetencion"

    Private Sub InserttblConceptoRetencion(ByVal obj As tblConceptoRetencion)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDConceptoRetencion
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_ConceptosRetencion_Actualizar(
            p1,
            obj.strCodigo,
            obj.strDescripcion,
            obj.dblPorcentajeRetencion,
            obj.dblGravado,
            obj.dblNoGravado,
            obj.dtmActualizacion,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            obj.strMsgValidacion)
        obj.intIDConceptoRetencion = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub UpdatetblConceptoRetencion(ByVal obj As tblConceptoRetencion)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDConceptoRetencion
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_ConceptosRetencion_Actualizar(
            p1,
            obj.strCodigo,
            obj.strDescripcion,
            obj.dblPorcentajeRetencion,
            obj.dblGravado,
            obj.dblNoGravado,
            obj.dtmActualizacion,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            obj.strMsgValidacion)
        obj.intIDConceptoRetencion = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub DeletetblConceptoRetencion(ByVal obj As tblConceptoRetencion)
        Dim p1 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_ConceptosRetencion_Eliminar(
            CType(obj.intIDConceptoRetencion, System.Nullable(Of Integer)),
            obj.strCodigo,
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p1)
        obj.strMsgValidacion = p1
        If Not String.IsNullOrEmpty(p1) Then
            Throw New Exception(p1, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
        End If
    End Sub

#End Region

#Region "ParesDeDivisas"

    Private Sub InsertParesDeDivisas(ByVal obj As CFCalculosFinancieros.ParesDeDivisas)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDParesDeDivisas
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_ParesDeDivisas_Actualizar(
            p1,
            CType(obj.intIDMonedaOrigen, System.Nullable(Of Integer)),
            CType(obj.intIDMonedaDestino, System.Nullable(Of Integer)),
            obj.logCambioCruzado,
            CType(obj.intIDMonedaCambioCruzado, System.Nullable(Of Integer)),
            obj.logComisionMonedaOrigen,
            CType(obj.numComisionMonedaOrigen, System.Nullable(Of Decimal)),
            obj.logComisionMonedaDestino,
            CType(obj.numComisionMonedaDestino, System.Nullable(Of Decimal)),
            obj.strCurvaMonedaOrigenNovado,
            obj.strCurvaMonedaOrigenNoNovado,
            obj.strCurvaMonedaDestinoNovado,
            obj.strCurvaMonedaDestinoNoNovado,
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIDParesDeDivisas = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub UpdateParesDeDivisas(ByVal obj As CFCalculosFinancieros.ParesDeDivisas)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDParesDeDivisas
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_ParesDeDivisas_Actualizar(
            p1,
            CType(obj.intIDMonedaOrigen, System.Nullable(Of Integer)),
            CType(obj.intIDMonedaDestino, System.Nullable(Of Integer)),
            obj.logCambioCruzado,
            CType(obj.intIDMonedaCambioCruzado, System.Nullable(Of Integer)),
            obj.logComisionMonedaOrigen,
            CType(obj.numComisionMonedaOrigen, System.Nullable(Of Decimal)),
            obj.logComisionMonedaDestino,
            CType(obj.numComisionMonedaDestino, System.Nullable(Of Decimal)),
            obj.strCurvaMonedaOrigenNovado,
            obj.strCurvaMonedaOrigenNoNovado,
            obj.strCurvaMonedaDestinoNovado,
            obj.strCurvaMonedaDestinoNoNovado,
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIDParesDeDivisas = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub DeleteParesDeDivisas(ByVal obj As CFCalculosFinancieros.ParesDeDivisas)
        Dim p1 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_ParesDeDivisas_Eliminar(
            CType(obj.intIDParesDeDivisas, System.Nullable(Of Integer)),
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p1)
        obj.strMsgValidacion = p1
        If Not String.IsNullOrEmpty(p1) Then
            Throw New Exception(p1, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
        End If
    End Sub

#End Region

End Class
