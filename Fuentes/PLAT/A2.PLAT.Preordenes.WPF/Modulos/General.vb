Imports A2.OyD.OYDServer.RIA.WEB
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

    Friend Function inicializarProxy() As PLAT_PreordenesDomainServices
        Dim objProxy = New PLAT_PreordenesDomainServices(New System.Uri(Program.RutaServicioNegocio(Program.TipoServicio.URLServicio_PLATPreordenes.ToString())))
        DirectCast(objProxy.DomainClient, WebDomainClient(Of PLAT_PreordenesDomainServices.IPLAT_PreordenesDomainServicesContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 500)

        Return objProxy
    End Function

End Module