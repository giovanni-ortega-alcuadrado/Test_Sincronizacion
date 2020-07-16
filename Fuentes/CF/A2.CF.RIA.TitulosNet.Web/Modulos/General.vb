Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client

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

    Friend Function inicializarProxyTitulosNet() As TitulosNetDomainContext
        Dim objProxy As TitulosNetDomainContext = Nothing
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            objProxy = New TitulosNetDomainContext()
        Else
            objProxy = New TitulosNetDomainContext(New System.Uri(Program.RutaServicioNegocio(Program.TipoServicio.URLServicioCFTitulosNet.ToString())))
        End If

        DirectCast(objProxy.DomainClient, WebDomainClient(Of TitulosNetDomainContext.ITitulosNetDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, A2ComunesControl.FuncionesCompartidas.TimeUpProxy, 0)
        DirectCast(objProxy.DomainClient, WebDomainClient(Of TitulosNetDomainContext.ITitulosNetDomainServiceContract)).ChannelFactory.Endpoint.Binding.ReceiveTimeout = New TimeSpan(0, A2ComunesControl.FuncionesCompartidas.TimeUpProxy, 0)
        
        Return objProxy
    End Function

    Friend Function inicializarProxyUtilidadesOYD() As UtilidadesDomainContext
        Dim objProxy As UtilidadesDomainContext = Nothing
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            objProxy = (New UtilidadesDomainContext())
        Else
            objProxy = (New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidades())))
        End If
        DirectCast(objProxy.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, A2ComunesControl.FuncionesCompartidas.TimeUpProxy, 0)
        DirectCast(objProxy.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.ReceiveTimeout = New TimeSpan(0, A2ComunesControl.FuncionesCompartidas.TimeUpProxy, 0)

        Return objProxy
    End Function

End Module

'Public Class MyHyperlinkButton
'    Inherits HyperlinkButton
'    Public Sub ClickMe()
'        MyBase.OnClick()
'    End Sub
'End Class