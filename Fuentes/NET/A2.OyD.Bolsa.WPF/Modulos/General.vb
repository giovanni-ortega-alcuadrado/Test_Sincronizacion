Imports Telerik.Windows.Controls
Imports System.Web
Imports A2ComunesControl

Module General

    Public Sub MostrarReporte(ByVal pstrParametros As String, ByVal pstrProceso As String, ByVal pstrReporte As String)
        Dim strRefrescar As String = String.Empty
        Dim strNroVentana As String = String.Empty 'Variable para mostrar el reporte en una ventana diferente
        Dim strFormatoRuta As String = String.Empty
        Dim strServidorRS As String = String.Empty   'Servidor de RS
        Dim strNombreReporteCompleto As String = String.Empty
        Dim strNombreCarpeta As String = String.Empty
        Dim strUrlRutaCompleta As String = String.Empty

        Try
            If Application.Current.Resources.Contains(Program.SERVICIO_REPORTE) = False Then
                A2Utilidades.Mensajes.mostrarMensaje("No se puede lanzar el reporte porque no se tiene configurado el nombre del servidor de Reportes", Program.TituloSistema)
                Exit Sub
            End If
            strServidorRS = Program.RutaServicioReportes
            strServidorRS = strServidorRS.Substring(0, strServidorRS.LastIndexOf("/"))


            If strServidorRS.Equals(String.Empty) Then
                A2Utilidades.Mensajes.mostrarMensaje("No se puede lanzar el reporte porque nel nombre del servidor de Reportes está en blanco", Program.TituloSistema)
                Exit Sub
            End If

            If pstrReporte.Trim().Equals(String.Empty) Then
                A2Utilidades.Mensajes.mostrarMensaje("No se puede lanzar el reporte porque el nombre del reporte está en blanco", Program.TituloSistema)
                Exit Sub
            End If

            strNombreCarpeta = Program.CarpetaReportes

            If strNombreCarpeta.Trim().Equals(String.Empty) Then
                A2Utilidades.Mensajes.mostrarMensaje("No se puede lanzar el reporte porque la carpeta del reporte está en blanco", Program.TituloSistema)
                Exit Sub
            End If

            If Left(strNombreCarpeta, 1) <> "/" Then
                strNombreReporteCompleto = "/" & strNombreCarpeta
            Else
                strNombreReporteCompleto = strNombreCarpeta
            End If

            If Right(strNombreReporteCompleto, 1) <> "/" Then
                strNombreReporteCompleto += "/"
            End If

            strNombreReporteCompleto = strNombreReporteCompleto + Replace(pstrReporte, "/", "")

            strRefrescar = Date.Now.Hour.ToString & Date.Now.Minute.ToString & Date.Now.Second.ToString & Date.Now.Millisecond.ToString 'Variable para que el reporte se refresque y evite utilizar el caché.
            'strFormatoRuta = "{0}/Pages/ReportViewer.aspx?{1}{2}" & "&pstrRefrescar=" & strRefrescar & "&pstrProceso=" & pstrProceso.ToString() & "&pstrMaquina=" & Program.Maquina & "&pstrLogUsuario=" & Program.Usuario & "&rs:Command=Render&rc:Parameters=false" '&rs:ParameterLanguage=" & objCulturaServidor.Name
            strFormatoRuta = "{0}/Pages/ReportViewer.aspx?{1}{2}" & "&rs:Command=Render&rc:Parameters=false" '&rs:ParameterLanguage=" & objCulturaServidor.Name
            strUrlRutaCompleta = String.Format(strFormatoRuta, strServidorRS, HttpUtility.UrlEncode(strNombreReporteCompleto), pstrParametros)  'Se debe codificar por las tildes!

            'Se utiliza para que el reporte se muestre en una ventana diferente
            strNroVentana = Date.Now.Hour.ToString & Date.Now.Minute.ToString & Date.Now.Second.ToString & Date.Now.Millisecond.ToString

            'Visualiza el reporte en pantalla.
            Program.VisorArchivosWeb_CargarURL(strUrlRutaCompleta)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la visualización del reporte", "General", "MostrarReporte", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub



End Module
