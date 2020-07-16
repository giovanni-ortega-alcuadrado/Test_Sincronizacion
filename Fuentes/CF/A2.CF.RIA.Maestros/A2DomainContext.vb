Imports System.ComponentModel
Imports OpenRiaServices.DomainServices.Client

'Desarrollado por Jose Walter Sierra
'Fecha Marzo del 2015
'Clases extendidas del DomainContext para establecer timeouts de los bindings de los servicios


Namespace Web

    Public NotInheritable Class A2Timeouts
        'Estos son valores por defecto en minutos
        'Estos valores no sobreescriben los valores establecidos en los ViewModels
        Public Shared intSendTimeout As New TimeSpan(0, 20, 0, 0)
        Public Shared intOpenTimeout As New TimeSpan(0, 10, 0, 0)
        Public Shared intCloseTimeout As New TimeSpan(0, 10, 0, 0)
        Public Shared intReceiveTimeout As New TimeSpan(0, 20, 0, 0)
        Public Sub New()
            'los valores se deberian tomar de web.config o el archivo xmlconfiguracion
        End Sub
    End Class

    Partial Public NotInheritable Class MaestrosCFDomainContext
        Inherits DomainContext

        Private Sub OnCreated()
            If Not DesignerProperties.IsInDesignTool Then
                DirectCast(Me.DomainClient, WebDomainClient(Of MaestrosCFDomainContext.IMaestrosCFDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = A2Timeouts.intSendTimeout
                DirectCast(Me.DomainClient, WebDomainClient(Of MaestrosCFDomainContext.IMaestrosCFDomainServiceContract)).ChannelFactory.Endpoint.Binding.OpenTimeout = A2Timeouts.intOpenTimeout
                DirectCast(Me.DomainClient, WebDomainClient(Of MaestrosCFDomainContext.IMaestrosCFDomainServiceContract)).ChannelFactory.Endpoint.Binding.CloseTimeout = A2Timeouts.intCloseTimeout
                DirectCast(Me.DomainClient, WebDomainClient(Of MaestrosCFDomainContext.IMaestrosCFDomainServiceContract)).ChannelFactory.Endpoint.Binding.ReceiveTimeout = A2Timeouts.intReceiveTimeout
            End If
        End Sub

    End Class

End Namespace

