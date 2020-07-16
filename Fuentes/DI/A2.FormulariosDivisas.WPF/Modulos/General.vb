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

	Friend Function inicializarProxy() As FormulariosDivisasDomainServices
        Dim objProxy = New FormulariosDivisasDomainServices(New System.Uri(Program.RutaServicioNegocio(Program.TipoServicio.URLServicioFormularios.ToString())))
        DirectCast(objProxy.DomainClient, WebDomainClient(Of FormulariosDivisasDomainServices.IFormulariosDivisasDomainServicesContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 500)

		Return objProxy
	End Function

End Module