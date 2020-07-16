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

    Friend Function inicializarProxy() As DER_MaestrosDomainServices
        Dim objProxy = New DER_MaestrosDomainServices(New System.Uri(Program.RutaServicioNegocio(Program.TipoServicio.URLServicio_DERMaestros.ToString())))
        DirectCast(objProxy.DomainClient, WebDomainClient(Of DER_MaestrosDomainServices.IDER_MaestrosDomainServicesContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 500)

        Return objProxy
    End Function

End Module