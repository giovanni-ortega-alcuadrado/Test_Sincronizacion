Imports System.Threading.Tasks
Imports A2.OyD.OYDServer.RIA.Web.OyDOperaciones
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura

''' <summary>
''' DomainServices para los maestros Calificadoras, EspeciesClaseInversion, CalificacionesCalificadora, PreciosEspecies,
''' Indicadores y ProcesarPortafolio, pertenecientes al proyecto Cálculos Financieros.
''' </summary>
''' Creado por       : Jorge Peña (Alcuadrado S.A.)
''' Descripción      : Creacion.
''' Fecha            : Febrero 21/2014
''' Pruebas CB       : Jorge Peña - Febrero 21/2014 - Resultado Ok
''' <remarks></remarks>

<EnableClientAccess()>
Partial Public Class OperacionesDomainService
    Inherits LinqToSqlDomainService(Of OyDOperacionesDatacontext)

    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

#Region "Operaciones"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertOperaciones(ByVal newOperaciones As Operaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,newOperaciones.pstrUsuarioConexion, newOperaciones.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            newOperaciones.strInfoSesion = DemeInfoSesion(newOperaciones.pstrUsuarioConexion, "InsertOperaciones")
            Me.DataContext.Operaciones.InsertOnSubmit(newOperaciones)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertOperaciones")
        End Try
    End Sub

    Public Sub UpdateOperaciones(ByVal currentOperaciones As Operaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentOperaciones.pstrUsuarioConexion, currentOperaciones.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentOperaciones.strInfoSesion = DemeInfoSesion(currentOperaciones.pstrUsuarioConexion, "UpdateOperaciones")
            Me.DataContext.Operaciones.Attach(currentOperaciones, Me.ChangeSet.GetOriginal(currentOperaciones))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateOperaciones")
        End Try
    End Sub

    Public Sub DeleteOperaciones(ByVal deleteOperaciones As Operaciones)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,deleteOperaciones.pstrUsuarioConexion, deleteOperaciones.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            deleteOperaciones.strInfoSesion = DemeInfoSesion(deleteOperaciones.pstrUsuarioConexion, "DeleteOperaciones")
            Me.DataContext.Operaciones.Attach(deleteOperaciones)
            Me.DataContext.Operaciones.DeleteOnSubmit(deleteOperaciones)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteOperaciones")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"

    Public Function FiltrarOperaciones(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Operaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Operaciones_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarOperaciones"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarOperaciones")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarOperaciones(ByVal plngID As System.Nullable(Of Integer), ByVal plngIDComitente As String, ByVal pstrTipo As String, ByVal pstrClaseOrden As String, ByVal pdtmLiquidacion As System.Nullable(Of Date), ByVal pdtmCumplimiento As System.Nullable(Of Date), ByVal plngIDOrden As System.Nullable(Of Integer), ByVal plngIDano As System.Nullable(Of Integer), ByVal plngParcial As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Operaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Operaciones_Consultar(String.Empty, plngID, plngIDComitente, pstrTipo, pstrClaseOrden, pdtmLiquidacion, pdtmCumplimiento, plngIDOrden, plngIDano, plngParcial, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarOperaciones"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarOperaciones")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarOperacionesPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Operaciones
        Dim objOperaciones As Operaciones = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Operaciones_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, 0, String.Empty, String.Empty, String.Empty, Nothing, Nothing, 0, 0, 0, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarOperacionesPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objOperaciones = ret.FirstOrDefault
            End If
            Return objOperaciones
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarOperacionesPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Function CumplimientoOrden_liq(ByVal pstrTipo As String, ByVal pstrClase As String, ByVal plngIDOrden As Integer,
                                  ByVal pstrIDEspecie As String, ByVal pdblCantidadLiq? As Double,
                                  ByVal pdblCantidadOrden? As Double, ByVal pdblCantidadImportacion? As Double, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.sp_CumplimientoOrden_OyDNet(pstrTipo, pstrClase, plngIDOrden, pstrIDEspecie, pdblCantidadLiq, pdblCantidadOrden, pdblCantidadImportacion)
            Dim variable As String
            variable = CStr(pdblCantidadLiq) + "," + CStr(pdblCantidadOrden)
            Return variable
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CumplimientoOrden_liq")
            Return Nothing
        End Try
    End Function

    Public Function ActualizaOrdenEstadoCumplida(ByVal pstrTipo As String, ByVal pstrClase As String, ByVal plngID As Integer, ByVal plngVersion As Integer, ByVal pstrEstado As String, ByVal pdtmEstado As System.Nullable(Of Date), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spUpdOrdenes_OyDNet(pstrTipo, pstrClase, plngID, plngVersion, pstrEstado, pdtmEstado, pstrUsuario)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizaOrdenEstadoCumplida")
            Return Nothing
        End Try
    End Function

    Public Function ActualizaOrdenEstado(ByVal pstrTipo As String, ByVal pstrClase As String, ByVal plngID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim Estado As String = String.Empty
            Dim ret = Me.DataContext.spLiquidacionesOrdeCum_OyDNet(pstrTipo, pstrClase, plngID, Estado)
            Return Estado
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizaOrdenEstado")
            Return Nothing
        End Try
    End Function

    Public Function Aplazamiento(ByVal pstrTipoAplazamiento As String, ByVal pstrAplazamiento As String, ByVal pstrClaseOrden As String, ByVal pstrTipoOrden As String _
                                 , ByVal plngIDLiquidacion As Integer, ByVal plngParcial As Integer, ByVal pdtmLiquidacion As DateTime, ByVal pdtmCumplimiento As DateTime,
                                 ByVal pstrUsuario As String, ByVal pstreRRor As String, ByVal intNroAplazamientos As System.Nullable(Of Integer), ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Liquidaciones_Aplazamiento(pstrTipoAplazamiento, pstrAplazamiento, pstrClaseOrden, pstrTipoOrden, plngIDLiquidacion, plngParcial, pdtmLiquidacion _
              , pdtmCumplimiento, pstrUsuario, pstreRRor, intNroAplazamientos, DemeInfoSesion(pstrUsuario, "Aplazamiento"), 0).ToString

            If pstreRRor = String.Empty Then
                Return intNroAplazamientos.ToString
            Else
                Return pstreRRor
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Aplazamiento")
            Return Nothing
        End Try
    End Function

    Public Function Verilifavalor(ByVal pstrId As String, ByVal pdtmFecha As System.Nullable(Of Date), ByVal Curvalor As System.Nullable(Of Double), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Double)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spValorEspecie_OyDNet(pstrId, pdtmFecha, Curvalor)
            Return Curvalor
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Buscaverilifavalor")
            Return Nothing
        End Try
    End Function

    Public Function VerificaNombreTarifa(ByVal pstrId As String, ByVal pstrNombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spDenominacionEspecie(pstrId, pstrNombre)
            Return pstrNombre
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Buscaverificanombretarifa")
            Return Nothing
        End Try
    End Function

    Public Function VerificadblIvaComision(ByVal pivacomision As System.Nullable(Of Double), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Double)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spdblIvacomision(pivacomision)
            Return pivacomision
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscaverificadblIvacomision")
            Return Nothing
        End Try
    End Function

    Public Function ValidarLiquidacion(ByVal plngID As Integer, ByVal plngParcial As System.Nullable(Of Integer), ByVal pstrTipo As String, ByVal pstrClase As String, ByVal pdtmLiquidacion As System.Nullable(Of Date), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacionesVeriLiq_OyDNet(plngID, plngParcial, pstrTipo, pstrClase, pdtmLiquidacion)
            Return plngID
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarLiquidacion")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function FiltrarOperacionesSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Operaciones)
        Dim objTask As Task(Of List(Of Operaciones)) = Me.FiltrarOperacionesAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function FiltrarOperacionesAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Operaciones))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Operaciones)) = New TaskCompletionSource(Of List(Of Operaciones))()
        objTaskComplete.TrySetResult(FiltrarOperaciones(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarOperacionesSync(ByVal plngID As System.Nullable(Of Integer), ByVal plngIDComitente As String, ByVal pstrTipo As String, ByVal pstrClaseOrden As String, ByVal pdtmLiquidacion As System.Nullable(Of Date), ByVal pdtmCumplimiento As System.Nullable(Of Date), ByVal plngIDOrden As System.Nullable(Of Integer), ByVal plngIDano As System.Nullable(Of Integer), ByVal plngParcial As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Operaciones)
        Dim objTask As Task(Of List(Of Operaciones)) = Me.ConsultarOperacionesAsync(plngID, plngIDComitente, pstrTipo, pstrClaseOrden, pdtmLiquidacion, pdtmCumplimiento, plngIDOrden, plngIDano, plngParcial, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarOperacionesAsync(ByVal plngID As System.Nullable(Of Integer), ByVal plngIDComitente As String, ByVal pstrTipo As String, ByVal pstrClaseOrden As String, ByVal pdtmLiquidacion As System.Nullable(Of Date), ByVal pdtmCumplimiento As System.Nullable(Of Date), ByVal plngIDOrden As System.Nullable(Of Integer), ByVal plngIDano As System.Nullable(Of Integer), ByVal plngParcial As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Operaciones))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Operaciones)) = New TaskCompletionSource(Of List(Of Operaciones))()
        objTaskComplete.TrySetResult(ConsultarOperaciones(plngID, plngIDComitente, pstrTipo, pstrClaseOrden, pdtmLiquidacion, pdtmCumplimiento, plngIDOrden, plngIDano, plngParcial, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarOperacionesPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Operaciones
        Dim objTask As Task(Of Operaciones) = Me.ConsultarOperacionesPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarOperacionesPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Operaciones)
        Dim objTaskComplete As TaskCompletionSource(Of Operaciones) = New TaskCompletionSource(Of Operaciones)()
        objTaskComplete.TrySetResult(ConsultarOperacionesPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "OperacionesReceptores"

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertOperacionesReceptores(ByVal newOperacionesReceptores As OperacionesReceptores)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,newOperacionesReceptores.pstrUsuarioConexion, newOperacionesReceptores.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            newOperacionesReceptores.strInfoSesion = DemeInfoSesion(newOperacionesReceptores.pstrUsuarioConexion, "InsertOperacionesReceptores")
            Me.DataContext.OperacionesReceptores.InsertOnSubmit(newOperacionesReceptores)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertOperacionesReceptores")
        End Try
    End Sub

    Public Sub UpdateOperacionesReceptores(ByVal currentOperacionesReceptores As OperacionesReceptores)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentOperacionesReceptores.pstrUsuarioConexion, currentOperacionesReceptores.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentOperacionesReceptores.strInfoSesion = DemeInfoSesion(currentOperacionesReceptores.pstrUsuarioConexion, "UpdateOperacionesReceptores")
            Me.DataContext.OperacionesReceptores.Attach(currentOperacionesReceptores, Me.ChangeSet.GetOriginal(currentOperacionesReceptores))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateOperacionesReceptores")
        End Try
    End Sub

    Public Sub DeleteOperacionesReceptores(ByVal deleteOperacionesReceptores As OperacionesReceptores)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,deleteOperacionesReceptores.pstrUsuarioConexion, deleteOperacionesReceptores.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            deleteOperacionesReceptores.strInfoSesion = DemeInfoSesion(deleteOperacionesReceptores.pstrUsuarioConexion, "DeleteOperacionesReceptores")
            Me.DataContext.OperacionesReceptores.Attach(deleteOperacionesReceptores)
            Me.DataContext.OperacionesReceptores.DeleteOnSubmit(deleteOperacionesReceptores)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteOperacionesReceptores")
        End Try
    End Sub

#End Region

#Region "Métodos asincrónicos"
    Public Function Traer_ReceptoresOrdenes_Liquidaciones(ByVal pstrTipo As String, ByVal pstrClase As String, ByVal plngID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OperacionesReceptores)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Bolsa_ReceptoresOrdenes_Filtrar(pstrTipo, pstrClase, plngID, DemeInfoSesion(pstrUsuario, "FiltrarOperacionesReceptores"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarOperacionesReceptores")
            Return Nothing
        End Try
    End Function

    Public Function ReceptoresOrdenesliq(ByVal pstrTipo As String, ByVal pstrClase As String, ByVal plngID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OperacionesReceptores)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacionesReceOrd_OyDNet(pstrTipo, pstrClase, plngID).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarOperacionesReceptores")
            Return Nothing
        End Try
    End Function
#End Region


#End Region

#Region "OperacionesBeneficiarios"
#Region "Métodos asincrónicos"
    Public Function BeneficiariosOrdenesliq(ByVal pstrTipo As String, ByVal pstrClase As String, ByVal plngID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OperacionesBeneficiarios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacionesBeneOrd_OyDNet(pstrTipo, pstrClase, plngID).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarBeneficiariosOrdenesliq")
            Return Nothing
        End Try
    End Function

    Public Function Traer_BeneficiariosOrdenes_Liquidaciones(ByVal pId As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OperacionesBeneficiarios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pId) Then
                Dim ret = Me.DataContext.uspOyDNet_Bolsa_BeneficiariosOrdenes_Filtrar(pId, DemeInfoSesion(pstrUsuario, "Traer_BeneficiariosOrdenes_Liquidaciones"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_BeneficiariosOrdenes_Liquidaciones")
            Return Nothing
        End Try
    End Function
#End Region
#End Region

#Region "OperacionesEspecies"
#Region "Métodos asincrónicos"
    Public Function EspeciesOrdenesLiq(ByVal pstrTipo As String, ByVal pstrClase As String, ByVal plngID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OperacionesEspecies)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacionesEspecies_OyDNet(pstrTipo, pstrClase, plngID).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarEspeciesOrdenesliq")
            Return Nothing
        End Try
    End Function

    Public Function Traer_EspeciesLiquidaciones(ByVal pId As Integer, ByVal plngParcial As Integer, ByVal pdtmFechaLiquidacion As System.Nullable(Of Date), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OperacionesEspecies)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pId) Then
                Dim ret = Me.DataContext.uspOyDNet_Bolsa_EspeciesLiquidaciones_Filtrar(pId, plngParcial, pdtmFechaLiquidacion, DemeInfoSesion(pstrUsuario, "Traer_EspeciesLiquidaciones"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_EspeciesLiquidaciones")
            Return Nothing
        End Try
    End Function
#End Region
#End Region

#Region "OperacionesAplazamientos"
#Region "Métodos asincrónicos"
    Public Function Traer_AplazamientosLiquidaciones(ByVal pId As Integer, ByVal dtmliquidacion As System.Nullable(Of Date), ByVal pParcial As System.Nullable(Of Integer), ByVal ptipo As String, ByVal pclase As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OperacionesAplazamientos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pId) Then
                Dim ret = Me.DataContext.uspOyDNet_Bolsa_AplazamientosLiquidaciones_Filtrar(pId, dtmliquidacion, pParcial, ptipo, pclase, DemeInfoSesion(pstrUsuario, "Traer_AplazamientosLiquidaciones"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_AplazamientosLiquidaciones")
            Return Nothing
        End Try
    End Function
#End Region
#End Region

#Region "OperacionesCustodias"
#Region "Métodos asincrónicos"
    Public Function Traer_CustodiasLiquidaciones(ByVal plngIDComisionista As Integer, ByVal PlngIDSucComisionista As Integer, ByVal plngIDComitente As String,
                                                     ByVal pstrIDEspecie As String, ByVal pstrTipo As String, ByVal pstrClaseOrden As String, ByVal plngID As Integer,
                                                     ByVal plngParcial As Integer, ByVal pdtmLiquidacion As System.Nullable(Of Date), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OperacionesCustodias)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(plngID) Then
                Dim ret = Me.DataContext.uspOyDNet_Bolsa_CustodiasLiquidaciones_Filtrar(plngIDComisionista, PlngIDSucComisionista, plngIDComitente, pstrIDEspecie, pstrTipo,
                                                                                        pstrClaseOrden, plngID, plngParcial, pdtmLiquidacion,
                                                                                        DemeInfoSesion(pstrUsuario, "Traer_CustodiasLiquidaciones"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_CustodiasLiquidaciones")
            Return Nothing
        End Try
    End Function

    Public Function OperacionesModificacionValidar(ByVal plngID As Integer, ByVal plngParcial As Integer, ByVal pstrTipo As String, ByVal pstrClaseOrden As String, ByVal pdtmLiquidacion As DateTime, ByVal pstrtipoOferta As String, ByVal plngIDComitente As String, ByVal pstrIDEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ModificacionValidar)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_Operaciones_ModificacionValidar(plngID, plngParcial, pstrTipo, pstrClaseOrden, pdtmLiquidacion, pstrtipoOferta, plngIDComitente, pstrIDEspecie).ToList()
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OperacionesModificacionValidar")
            Return Nothing
        End Try
    End Function

    Public Function OperacionesRetardosActualizar(ByVal plngID As Integer, ByVal plngParcial As Integer, ByVal pstrTipo As String, ByVal pstrClaseOrden As String, ByVal pdtmLiquidacion As DateTime, ByVal plogReconstruirRetardo As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RetardoOperacionesActualizar)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_Operaciones_Retardo_Actualizar(plngID, plngParcial, pstrTipo, pstrClaseOrden, pdtmLiquidacion, plogReconstruirRetardo, pstrUsuario).ToList()
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OperacionesRetardosActualizar")
            Return Nothing
        End Try
    End Function
#End Region
#End Region

#Region "LiquidacionesOrdenes"

#Region "Métodos asincrónicos"

    Public Function OrdenesLiq(ByVal pstrTipo As String, ByVal pstrClase As String, ByVal plngID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of LiquidacionesOrdenes)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacionesOrdenes_OyDNet(pstrTipo, pstrClase, plngID).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarOperaciones")
            Return Nothing
        End Try
    End Function

#End Region

#End Region

#Region "LiquidacionesConsultar"
    Public Function LiquidacionesConsultarValidar(ByVal pstrTipo As String, ByVal pstrClase As String, ByVal plngID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of LiquidacionesConsultar)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spLiquidacionesVeriOrd_OyDNet(pstrTipo, pstrClase, plngID).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "LiquidacionesConsultarValidar")
            Return Nothing
        End Try
    End Function
#End Region

#Region "ConsultarCantidad"
    Public Function LiquidacionesConsultarCantidad(ByVal pstrTipo As String, ByVal pstrClase As String, ByVal plngID As Integer, ByVal pstrIDEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConsultarCantidad)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.sp_TraerCumplimiento_OyDNet(pstrTipo, pstrClase, plngID, pstrIDEspecie).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "LiquidacionesConsultarCantidad")
            Return Nothing
        End Try
    End Function
#End Region

#Region "RetardoOperaciones"

    Public Function ConsultarRetardoOperaciones(ByVal plogFiltroSencillo As Boolean,
                                                ByVal plogFiltroAvanzado As Boolean,
                                                ByVal pstrFiltro As String,
                                                ByVal pdtmFecha As System.Nullable(Of Date),
                                                ByVal pintCuenta As Integer,
                                                ByVal pstrIDEspecie As String,
                                                ByVal plogProcesado As System.Nullable(Of Boolean),
                                                ByVal pstrUsuario As String,
                                                ByVal pstrInfoConexion As String) As List(Of RetardoOperaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_RetardoOperaciones_Consultar(plogFiltroSencillo,
                                                                            plogFiltroAvanzado,
                                                                            RetornarValorDescodificado(pstrFiltro),
                                                                            pdtmFecha,
                                                                            pintCuenta,
                                                                            pstrIDEspecie,
                                                                            plogProcesado,
                                                                            pstrUsuario,
                                                                            DemeInfoSesion(pstrUsuario, "ConsultarRetardoOperaciones"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarRetardoOperacionesPorDefecto")
            Return Nothing
        End Try
    End Function

    Private Function ConsultarRetardoOperacionesAsync(ByVal plogFiltroSencillo As Boolean,
                                                      ByVal plogFiltroAvanzado As Boolean,
                                                      ByVal pstrFiltro As String,
                                                      ByVal pdtmFecha As System.Nullable(Of Date),
                                                      ByVal pintCuenta As Integer,
                                                      ByVal pstrIDEspecie As String,
                                                      ByVal plogProcesado As System.Nullable(Of Boolean),
                                                      ByVal pstrUsuario As String,
                                                      ByVal pstrInfoConexion As String) As Task(Of List(Of RetardoOperaciones))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of RetardoOperaciones)) = New TaskCompletionSource(Of List(Of RetardoOperaciones))()
        objTaskComplete.TrySetResult(ConsultarRetardoOperaciones(plogFiltroSencillo, plogFiltroAvanzado, pstrFiltro, pdtmFecha, pintCuenta, pstrIDEspecie, plogProcesado, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarRetardoOperacionesSync(ByVal plogFiltroSencillo As Boolean,
                                                      ByVal plogFiltroAvanzado As Boolean,
                                                      ByVal pstrFiltro As String,
                                                      ByVal pdtmFecha As System.Nullable(Of Date),
                                                      ByVal pintCuenta As Integer,
                                                      ByVal pstrIDEspecie As String,
                                                      ByVal plogProcesado As System.Nullable(Of Boolean),
                                                      ByVal pstrUsuario As String,
                                                      ByVal pstrInfoConexion As String) As List(Of RetardoOperaciones)
        Dim objTask As Task(Of List(Of RetardoOperaciones)) = Me.ConsultarRetardoOperacionesAsync(plogFiltroSencillo, plogFiltroAvanzado, pstrFiltro, pdtmFecha, pintCuenta, pstrIDEspecie, plogProcesado, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Public Function ConsultarRetardoOperacionesDetalle(ByVal pintIDRetardoOperaciones As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RetardoOperacionesDetalle)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_RetardoOperaciones_ConsultarDetalle(pintIDRetardoOperaciones, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarRetardoOperacionesDetalle"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarRetardoOperacionesDetalle")
            Return Nothing
        End Try
    End Function

    Private Function ConsultarRetardoOperacionesDetalleAsync(ByVal pintIDRetardoOperaciones As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of RetardoOperacionesDetalle))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of RetardoOperacionesDetalle)) = New TaskCompletionSource(Of List(Of RetardoOperacionesDetalle))()
        objTaskComplete.TrySetResult(ConsultarRetardoOperacionesDetalle(pintIDRetardoOperaciones, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarRetardoOperacionesDetalleSync(ByVal pintIDRetardoOperaciones As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RetardoOperacionesDetalle)
        Dim objTask As Task(Of List(Of RetardoOperacionesDetalle)) = Me.ConsultarRetardoOperacionesDetalleAsync(pintIDRetardoOperaciones, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Public Sub UpdateRetardoOperaciones(ByVal objUpdateRetardoOperaciones As RetardoOperaciones)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objUpdateRetardoOperaciones.pstrUsuarioConexion, objUpdateRetardoOperaciones.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objUpdateRetardoOperaciones.pstrInfoConexion = DemeInfoSesion(objUpdateRetardoOperaciones.pstrUsuarioConexion, "UpdateRetardoOperaciones")
            Me.DataContext.RetardoOperaciones.Attach(objUpdateRetardoOperaciones, Me.ChangeSet.GetOriginal(objUpdateRetardoOperaciones))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateRetardoOperaciones")
        End Try
    End Sub

    Public Sub UpdateRetardoOperacionesDetalle(ByVal objUpdateRetardoOperacionesDetalle As RetardoOperacionesDetalle)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, objUpdateRetardoOperacionesDetalle.pstrUsuarioConexion, objUpdateRetardoOperacionesDetalle.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objUpdateRetardoOperacionesDetalle.pstrInfoConexion = DemeInfoSesion(objUpdateRetardoOperacionesDetalle.pstrUsuarioConexion, "UpdateRetardoOperacionesDetalle")
            Me.DataContext.RetardoOperacionesDetalle.Attach(objUpdateRetardoOperacionesDetalle, Me.ChangeSet.GetOriginal(objUpdateRetardoOperacionesDetalle))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateRetardoOperacionesDetalle")
        End Try
    End Sub

    Public Function ActualizarRetardoOperacionesDetalle(ByVal pintIDRetardo As System.Nullable(Of Integer), ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim plogRetardoxPantalla As Boolean = True
            Dim pstrMsgValidacion As String = ""
            Dim ret = Me.DataContext.uspOyDNet_RetardoOperaciones_Controlador(Nothing, Nothing, pstrUsuario, DemeInfoSesion(pstrUsuario, "ActualizarRetardoOperacionesDetalle"), 0, plogRetardoxPantalla, pintIDRetardo, pxmlDetalleGrid, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizarRetardoOperacionesDetalle")
            Return Nothing
        End Try
    End Function

    Private Function ActualizarRetardoOperacionesDetalleAsync(ByVal pintIDRetardo As System.Nullable(Of Integer), ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(ActualizarRetardoOperacionesDetalle(pintIDRetardo, pxmlDetalleGrid, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ActualizarRetardoOperacionesDetalleSync(ByVal pintIDRetardo As System.Nullable(Of Integer), ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.ActualizarRetardoOperacionesDetalleAsync(pintIDRetardo, pxmlDetalleGrid, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

#End Region

End Class
