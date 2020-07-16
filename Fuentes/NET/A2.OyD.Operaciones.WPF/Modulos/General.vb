Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports A2ComunesControl

Module General
    Friend Enum ValoresUserState
        Combos
        CombosEspecificos
        Listar
        Consultar
        Filtrar
        Buscar
        Ingresar
        Actualizar
        Borrar
        PorDefecto
    End Enum

    Friend Function inicializarProxyOperaciones() As OperacionesDomainContext
        Dim objProxy As OperacionesDomainContext = Nothing
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            objProxy = (New OperacionesDomainContext())
        Else
            objProxy = (New OperacionesDomainContext(New System.Uri(Program.RutaServicioNegocio(Program.TipoServicio.URLServicioOperaciones.ToString()))))
        End If
        DirectCast(objProxy.DomainClient, WebDomainClient(Of OperacionesDomainContext.IOperacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, A2ComunesControl.FuncionesCompartidas.TimeUpProxy)
        Return objProxy
    End Function



    Friend Function inicializarProxyUtilidades() As UtilidadesDomainContext
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            Return (New UtilidadesDomainContext())
        Else
            Return (New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidades())))
        End If
    End Function

    Friend Function inicializarProxyUtilidadesConTiempoEspera() As UtilidadesDomainContext
        Dim objProxy As UtilidadesDomainContext = Nothing
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            objProxy = (New UtilidadesDomainContext())
        Else
            objProxy = (New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidades())))
        End If
        DirectCast(objProxy.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, A2ComunesControl.FuncionesCompartidas.TimeUpProxy)

        Return objProxy
    End Function

End Module

'Public Class MyHyperlinkButton
'    Inherits HyperlinkButton
'    Public Sub ClickMe()
'        MyBase.OnClick()
'    End Sub
'End Class