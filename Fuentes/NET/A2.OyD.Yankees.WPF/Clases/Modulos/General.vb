﻿Imports Telerik.Windows.Controls
Imports System.Web
Imports A2Utilidades.Mensajes

Module General
    Public Enum EnumVersionAplicacionCliente
        G 'Genérico
        C 'City
    End Enum

    Public Sub MostrarReporte(ByVal pstrParametros As String, ByVal pstrProceso As String, ByVal pstrReporte As String)
        Dim strRefrescar As String = String.Empty
        Dim strNroVentana As String = String.Empty 'Variable para mostrar el reporte en una ventana diferente
        Dim strFormatoRuta As String = String.Empty
        Dim strServidorRS As String = String.Empty   'Servidor de RS

        Try
            If Application.Current.Resources.Contains("A2VServicioRS") = False Then
                A2Utilidades.Mensajes.mostrarMensaje("No se puede lanzar el reporte porque no se tiene configurado el nombre del servidor de Reportes", Program.TituloSistema)
                Exit Sub
            End If
            strServidorRS = Application.Current.Resources("A2VServicioRS").ToString.Trim()
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
