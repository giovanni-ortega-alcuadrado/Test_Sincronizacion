Imports Telerik.Windows.Controls
Imports System.Web
Imports A2Utilidades.Mensajes
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

    Friend Function inicializarProxyOperacionesOtrosNegocios() As OperacionesCFDomainContext
        Dim objProxy As OperacionesCFDomainContext = Nothing
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            objProxy = New OperacionesCFDomainContext()
        Else
            objProxy = New OperacionesCFDomainContext(New System.Uri(Program.RutaServicioOperacionesCF()))
        End If

        DirectCast(objProxy.DomainClient, WebDomainClient(Of OperacionesCFDomainContext.IOperacionesCFDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, A2ComunesControl.FuncionesCompartidas.TimeUpProxy)

        Return objProxy
    End Function

    Friend Function inicializarProxyUtilidades() As UtilidadesCFDomainContext
        Dim objProxy As UtilidadesCFDomainContext = Nothing
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            objProxy = New UtilidadesCFDomainContext()
        Else
            objProxy = New UtilidadesCFDomainContext(New System.Uri(Program.RutaServicioUtilidadesCF()))
        End If

        DirectCast(objProxy.DomainClient, WebDomainClient(Of UtilidadesCFDomainContext.IUtilidadesCFDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, A2ComunesControl.FuncionesCompartidas.TimeUpProxy)

        Return objProxy
    End Function

    Friend Function inicializarProxyUtilidadesOYD() As UtilidadesDomainContext
        Dim objProxy As UtilidadesDomainContext = Nothing
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            objProxy = (New UtilidadesDomainContext())
        Else
            objProxy = (New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD())))
        End If
        DirectCast(objProxy.DomainClient, WebDomainClient(Of UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, A2ComunesControl.FuncionesCompartidas.TimeUpProxy)

        Return objProxy
    End Function

    Public Sub MostrarReporte(ByVal pstrParametros As String, ByVal pstrProceso As String, ByVal pstrReporte As String)
        Dim strRefrescar As String = String.Empty
        Dim strNroVentana As String = String.Empty 'Variable para mostrar el reporte en una ventana diferente
        Dim strFormatoRuta As String = String.Empty
        Dim strServidorRS As String = String.Empty   'Servidor de RS

        Try
            If Application.Current.Resources.Contains(Program.SERVICIO_REPORTE) = False Then
                A2Utilidades.Mensajes.mostrarMensaje("No se puede lanzar el reporte porque no se tiene configurado el nombre del servidor de Reportes", Program.TituloSistema)
                Exit Sub
            End If
            strServidorRS = Application.Current.Resources(Program.SERVICIO_REPORTE).ToString.Trim()
            strServidorRS = strServidorRS.Substring(0, strServidorRS.LastIndexOf("/"))

            If strServidorRS.Equals(String.Empty) Then
                A2Utilidades.Mensajes.mostrarMensaje("No se puede lanzar el reporte porque nel nombre del servidor de Reportes está en blanco", Program.TituloSistema)
                Exit Sub
            End If

            If pstrReporte.Trim().Equals(String.Empty) Then
                A2Utilidades.Mensajes.mostrarMensaje("No se puede lanzar el reporte porque el nombre del reporte está en blanco", Program.TituloSistema)
                Exit Sub
            End If

            strRefrescar = Date.Now.Hour.ToString & Date.Now.Minute.ToString & Date.Now.Second.ToString & Date.Now.Millisecond.ToString 'Variable para que el reporte se refresque y evite utilizar el caché.
            strFormatoRuta = "{0}/Pages/ReportViewer.aspx?{1}{2}" & "&pstrRefrescar=" & strRefrescar & "&pstrProceso=" & pstrProceso.ToString() & "&pstrMaquina=" & Program.Maquina & "&pstrLogUsuario=" & Program.Usuario & "&rs:Command=Render&rc:Parameters=false" '&rs:ParameterLanguage=" & objCulturaServidor.Name
            pstrReporte = String.Format(strFormatoRuta, strServidorRS, HttpUtility.UrlEncode(pstrReporte), pstrParametros)  'Se debe codificar por las tildes!

            'Se utiliza para que el reporte se muestre en una ventana diferente
            strNroVentana = Date.Now.Hour.ToString & Date.Now.Minute.ToString & Date.Now.Second.ToString & Date.Now.Millisecond.ToString

            'Visualiza el reporte en pantalla.
            Program.VisorArchivosWeb_CargarURL(pstrReporte)
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la visualización del reporte", "General", "MostrarReporte", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

End Module

''' <summary>
''' Clase definida para generar los reportes en RS cuando se está Out of Browser
''' </summary>
''' <remarks></remarks>
'Public Class MyHyperlinkButton
'    Inherits HyperlinkButton
'    Public Sub ClickMe()
'        MyBase.OnClick()
'    End Sub
'End Class
