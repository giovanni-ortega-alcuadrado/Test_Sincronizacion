'Codigo generado
'Archivo: ImportacionDerechosPatrimonialesViewModel
'Generado el : 09/06/2016 
'Propiedad de Alcuadrado S.A. 2016


Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data

Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones
Imports System.Text
Imports A2Utilidades.Mensajes

Public Class ImportacionDerechosPatrimonialesViewModel
    Inherits A2ControlMenu.A2ViewModel

    Private _strNombreArchivoImportado As String = String.Empty
    Private Const _STR_NOMBRE_PROCESO As String = "DatosDeceval"
    Private dcProxy As ImportacionesDomainContext


    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New ImportacionesDomainContext
        Else
            dcProxy = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioNegocio))
        End If

        DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 600)

        Try
            If Not Program.IsDesignMode() Then
                'IsBusy = True
                'dcProxy.Load(dcProxy.TraerArchivosDirectorioQuery(_STR_NOMBRE_PROCESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivosAdjuntos, Nothing)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ImportacionDecevalViewModel.New", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    'Public Sub MostrarCargadorArchivos()
    '    Try
    '        Dim cwCar As New CargarArchivosView(_STR_NOMBRE_PROCESO)
    '        AddHandler cwCar.Closed, AddressOf VentanaCargaArchivoCerro
    '        cwCar.Show()
    '    Catch ex As Exception
    '        IsBusy = False
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción", _
    '                             Me.ToString(), "ImportacionDecevalViewModel.MostrarCargadorArchivos", Program.TituloSistema, Program.Maquina, ex)
    '    End Try
    'End Sub

    Public Sub LimpiarResultados()
        Try
            Dim objListaResultado As New List(Of OyDImportaciones.RespuestaArchivoImportacionDER245)
            ListaResultado = objListaResultado
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción",
                                 Me.ToString(), "ImportarLiquidacionesView.LimpiarResultados", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub CargarArchivo()
        Try
            If ArchivoSeleccionado Is Nothing Then
                A2Utilidades.Mensajes.mostrarMensaje("No ha seleccionado el archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If String.IsNullOrEmpty(ArchivoSeleccionado.Nombre) Then
                A2Utilidades.Mensajes.mostrarMensaje("No ha seleccionado el archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True

            If Not IsNothing(dcProxy.LineaComentarios) Then
                dcProxy.LineaComentarios.Clear()
            End If

            dcProxy.Load(dcProxy.NroDeRegistrosDecevalQuery(False, Program.Usuario, Program.HashConexion), AddressOf TerminaConsultaNroRegistros, Nothing)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción",
                                 Me.ToString(), "ImportarLiquidacionesView.btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarResultados(ByVal pstrTipoConsulta As String)
        Try
            IsBusy = True
            dcProxy.RespuestaArchivoImportacions.Clear()
            dcProxy.Load(dcProxy.ConsultarResultadoDER245Query(pstrTipoConsulta, _STR_NOMBRE_PROCESO, Program.Usuario, Program.HashConexion), AddressOf TerminaConsultarResultados, Nothing)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción",
                                 Me.ToString(), "ConsultarResultados.", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub GenerarResultadoImportacion()
        Try
            If Not String.IsNullOrEmpty(URLArchivo) Then
                Program.VisorArchivosWeb_DescargarURL(URLArchivo)
                'Dim strNroVentana As String = Date.Now.Hour.ToString & Date.Now.Minute.ToString & Date.Now.Second.ToString & Date.Now.Millisecond.ToString
                'If (Application.Current.IsRunningOutOfBrowser) Then
                '    'para que cargue el visor de reportes para cuando la consola se ejecuta por fuera del browser
                '    Dim button As New MyHyperlinkButton
                '    button.NavigateUri = New Uri(URLArchivo)
                '    button.TargetName = "vtnNva" & "00" & strNroVentana
                '    button.ClickMe()
                'Else
                '    HtmlPage.Window.Navigate(New Uri(URLArchivo), "vtnNva" & "00" & strNroVentana, "height=550,width=750,top=25,left=25,toolbar=1,resizable=1")
                'End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción",
                                 Me.ToString(), "GenerarResultadoImportacion.", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Asincronos"

    Private Sub VentanaCargaArchivoCerro(sender As System.Object, e As EventArgs)
        Try
            If Not IsNothing(ListaArchivos) Then
                ListaArchivos.Clear()
            End If

            dcProxy.Archivos.Clear()
            dcProxy.Load(dcProxy.TraerArchivosDirectorioQuery(_STR_NOMBRE_PROCESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivosAdjuntos, Nothing)
            'If CType(sender, CargarArchivosView).DialogResult = True And Not IsNothing(CType(sender, CargarArchivosView).ArchivoSeleccionado) Then
            '    _strNombreArchivoImportado = CType(sender, CargarArchivosView).ArchivoSeleccionado.Nombre
            '    ArchivoSeleccionado = CType(sender, CargarArchivosView).ArchivoSeleccionado
            'End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ImportacionDecevalViewModel.VentanaCargaArchivoCerro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub VentanaCargaArchivoControl(ByVal strNombrearchivo As String)
        Try
            'SLB20131008 Manejo con el text box
            ArchivoSeleccionado.Nombre = strNombrearchivo
            Exit Sub

            If Not IsNothing(ListaArchivos) Then
                ListaArchivos.Clear()
            End If
            dcProxy.Archivos.Clear()
            dcProxy.Load(dcProxy.TraerArchivosDirectorioQuery(_STR_NOMBRE_PROCESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivosAdjuntos, Nothing)
            _strNombreArchivoImportado = strNombrearchivo
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ImportacionDecevalViewModel.VentanaCargaArchivoCerro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerArchivosAdjuntos(ByVal lo As LoadOperation(Of Archivo))
        If IsNothing(lo.Error) Then
            ListaArchivos = dcProxy.Archivos.ToList
            ArchivoSeleccionado = _ListaArchivos.FirstOrDefault()
        Else
            MessageBox.Show(lo.Error.Message)
        End If
        IsBusy = False
    End Sub

    Private Sub TerminaConsultaNroRegistros(lo As LoadOperation(Of LineaComentario))
        Try
            Dim sb As New StringBuilder

            If lo.HasError = False Then
                If Not String.IsNullOrEmpty(lo.Entities(0).Texto) Then
                    If lo.Entities(0).Texto.Equals("BORRADO") Or lo.Entities(0).Texto.Equals("OK") Then
                        IsBusy = True
                        If Not IsNothing(dcProxy.RespuestaArchivoImportacionDER245s) Then
                            dcProxy.RespuestaArchivoImportacionDER245s.Clear()
                        End If
                        dcProxy.Load(dcProxy.CargarArchivoDecevalDER245Query(ArchivoSeleccionado.Nombre, _STR_NOMBRE_PROCESO, Program.Usuario, Program.HashConexion), AddressOf TerminaCargaDatosDeceval, Nothing)
                        'dcProxy.Load(dcProxy.CargarArchivoDecevalQuery(ArchivoSeleccionado.Ruta, _STR_NOMBRE_PROCESO, Program.Usuario, Program.HashConexion), AddressOf TerminaCargaDatosDeceval, Nothing)
                    Else
                        IsBusy = False
                        mostrarMensajePregunta(lo.Entities(0).Texto,
                                               Program.TituloSistema,
                                               "CONSULTARNROREGISTROS",
                                               AddressOf TerminaPregunta, False)
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción",
                                 Me.ToString(), "ImportacionDecevalViewModel.TerminoCargarArchivoLiquidaciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminaCargaDatosDeceval(lo As LoadOperation(Of OyDImportaciones.RespuestaArchivoImportacionDER245))
        Try
            Dim sb As New StringBuilder

            If lo.HasError = False Then
                ListaResultado = lo.Entities.ToList

                Dim logprimero As Boolean = True

                For Each Lista In ListaResultado
                    If logprimero Then
                        _URLArchivo = Lista.URLArchivo
                        logprimero = False
                    End If
                Next
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción",
                                 Me.ToString(), "ImportacionDecevalViewModel.TerminaCargaDatosDeceval", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminaConsultarResultados(lo As LoadOperation(Of RespuestaArchivoImportacion))
        Try
            Dim sb As New StringBuilder

            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    Dim strArchivo As String = lo.Entities.First.URLArchivo
                    Program.VisorArchivosWeb_DescargarURL(strArchivo)
                    'Dim strNroVentana As String = Date.Now.Hour.ToString & Date.Now.Minute.ToString & Date.Now.Second.ToString & Date.Now.Millisecond.ToString
                    'If (Application.Current.IsRunningOutOfBrowser) Then
                    '    'para que cargue el visor de reportes para cuando la consola se ejecuta por fuera del browser
                    '    Dim button As New MyHyperlinkButton
                    '    button.NavigateUri = New Uri(strArchivo)
                    '    button.TargetName = "vtnNva" & "00" & strNroVentana
                    '    button.ClickMe()
                    'Else
                    '    HtmlPage.Window.Navigate(New Uri(strArchivo), "vtnNva" & "00" & strNroVentana, "height=550,width=750,top=25,left=25,toolbar=1,resizable=1")
                    'End If
                End If
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción",
                                 Me.ToString(), "ImportacionDecevalViewModel.TerminaCargaDatosDeceval", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminaPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                IsBusy = True
                If Not IsNothing(dcProxy.LineaComentarios) Then
                    dcProxy.LineaComentarios.Clear()
                End If
                dcProxy.Load(dcProxy.NroDeRegistrosDecevalQuery(True, Program.Usuario, Program.HashConexion), AddressOf TerminaConsultaNroRegistros, Nothing)
            Else
                LimpiarResultados()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción",
                                 Me.ToString(), "ImportacionDecevalViewModel.TerminaPregunta", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

#End Region

#Region "Propiedades"

    Private _ListaResultado As List(Of OyDImportaciones.RespuestaArchivoImportacionDER245)
    Public Property ListaResultado() As List(Of OyDImportaciones.RespuestaArchivoImportacionDER245)
        Get
            Return _ListaResultado
        End Get
        Set(ByVal value As List(Of OyDImportaciones.RespuestaArchivoImportacionDER245))
            _ListaResultado = value
            MyBase.CambioItem("ListaResultado")
            MyBase.CambioItem("ListaResultadoPaged")
        End Set
    End Property

    Public ReadOnly Property ListaResultadoPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaResultado) Then
                Dim view = New PagedCollectionView(_ListaResultado)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ListaArchivos As List(Of Archivo)
    Public Property ListaArchivos() As List(Of Archivo)
        Get
            Return _ListaArchivos
        End Get
        Set(ByVal value As List(Of Archivo))
            _ListaArchivos = value
            MyBase.CambioItem("ListaArchivos")
            ArchivoSeleccionado = _ListaArchivos.FirstOrDefault
        End Set
    End Property

    Private _ArchivoSeleccionado As New Archivo
    Public Property ArchivoSeleccionado() As Archivo
        Get
            Return _ArchivoSeleccionado
        End Get
        Set(ByVal value As Archivo)
            If Not IsNothing(value) Then
                _ArchivoSeleccionado = value
                MyBase.CambioItem("ArchivoSeleccionado")
            End If
        End Set
    End Property

    Private _URLArchivo As String
    Public Property URLArchivo() As String
        Get
            Return _URLArchivo
        End Get
        Set(ByVal value As String)
            _URLArchivo = value
        End Set
    End Property

#End Region

End Class
