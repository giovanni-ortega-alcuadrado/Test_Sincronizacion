Imports Telerik.Windows.Controls
Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web

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
    Friend Function inicializarProxyPLUSMaestros() AS OyDPLUSMaestrosDomainContext

        Dim objProxy AS OyDPLUSMaestrosDomainContext = Nothing
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            objProxy = New A2.OyD.OYDServer.RIA.Web.OyDPLUSMaestrosDomainContext()
        Else
            objProxy = New A2.OyD.OYDServer.RIA.Web.OyDPLUSMaestrosDomainContext(New System.Uri(Program.RutaServicioPLUSMaestros(Program.TipoServicio.URLServicioPLUSMaestros.ToString())))
        End If

        DirectCast(objProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.OyDPLUSMaestrosDomainContext.IOyDPLUSMaestrosDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, A2ComunesControl.FuncionesCompartidas.TimeUpProxy, 0)
        DirectCast(objProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.OyDPLUSMaestrosDomainContext.IOyDPLUSMaestrosDomainServiceContract)).ChannelFactory.Endpoint.Binding.ReceiveTimeout = New TimeSpan(0, A2ComunesControl.FuncionesCompartidas.TimeUpProxy, 0)

        Return objProxy

    End Function
End Module
