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

    Friend Function inicializarProxyProcesarPortafolios() As ProcesarPortafoliosDomainContext
        Dim objProxy As ProcesarPortafoliosDomainContext = Nothing
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            objProxy = New ProcesarPortafoliosDomainContext()
        Else
            objProxy = New ProcesarPortafoliosDomainContext(New System.Uri(Program.RutaServicioNegocio(Program.TipoServicio.URLServicioCFProcesarPortafolios.ToString())))
        End If

        DirectCast(objProxy.DomainClient, WebDomainClient(Of ProcesarPortafoliosDomainContext.IProcesarPortafoliosDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, A2ComunesControl.FuncionesCompartidas.TimeUpProxy, 0)
        DirectCast(objProxy.DomainClient, WebDomainClient(Of ProcesarPortafoliosDomainContext.IProcesarPortafoliosDomainServiceContract)).ChannelFactory.Endpoint.Binding.ReceiveTimeout = New TimeSpan(0, A2ComunesControl.FuncionesCompartidas.TimeUpProxy, 0)

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

    Friend Function inicializarProxyUtilidades() As UtilidadesCFDomainContext
        Dim objProxy As UtilidadesCFDomainContext = Nothing
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            objProxy = New UtilidadesCFDomainContext()
        Else
            objProxy = New UtilidadesCFDomainContext(New System.Uri(Program.RutaServicioUtilidadesCF()))
        End If

        'JEPM20160422 Cambio en el timeout de segundos a minutos. Apoyo Juan D. Correa
        DirectCast(objProxy.DomainClient, WebDomainClient(Of UtilidadesCFDomainContext.IUtilidadesCFDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, A2ComunesControl.FuncionesCompartidas.TimeUpProxy, 0)
        DirectCast(objProxy.DomainClient, WebDomainClient(Of UtilidadesCFDomainContext.IUtilidadesCFDomainServiceContract)).ChannelFactory.Endpoint.Binding.ReceiveTimeout = New TimeSpan(0, A2ComunesControl.FuncionesCompartidas.TimeUpProxy, 0) 'JEPM20170123 Se añade linea ReceiveTimeout

        Return objProxy
    End Function
End Module

'Public Class MyHyperlinkButton
'    Inherits HyperlinkButton
'    Public Sub ClickMe()
'        MyBase.OnClick()
'    End Sub
'End Class