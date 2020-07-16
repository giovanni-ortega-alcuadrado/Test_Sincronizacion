Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports System.Threading.Tasks
Imports System.Text.RegularExpressions

Module General

    Friend Const GSTR_TIPO_LINEA_INCONSISTENCIA As String = "INCONSISTENCIA"

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

    Friend Function inicializarProxyUtilidades() As UtilidadesCFDomainContext
        Dim objProxy As UtilidadesCFDomainContext = Nothing
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            objProxy = New UtilidadesCFDomainContext()
        Else
            objProxy = New UtilidadesCFDomainContext(New System.Uri(Program.RutaServicioUtilidadesCF()))
        End If

        DirectCast(objProxy.DomainClient, WebDomainClient(Of UtilidadesCFDomainContext.IUtilidadesCFDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, A2ComunesControl.FuncionesCompartidas.TimeUpProxy, 0)

        Return objProxy
    End Function

    Friend Function inicializarProxyUtilidadesOYD() As UtilidadesDomainContext
        Dim objProxy As UtilidadesDomainContext = Nothing
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            objProxy = (New UtilidadesDomainContext())
        Else
            objProxy = (New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD())))
        End If
        DirectCast(objProxy.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, A2ComunesControl.FuncionesCompartidas.TimeUpProxy, 0)

        Return objProxy
    End Function

    Public Function IsNumeric(ByVal str As String) As Boolean
        Dim r As Regex = New Regex("\d+")
        Dim m As Match = r.Match(str)
        If (m.Success) Then
            Return True
        End If
        Return False
    End Function

End Module

