Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDTesoreria
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2Utilidades.Mensajes 'CORREC_CITI_SV_2014


Public Class PlanoComEgresosViewModel
    Inherits A2ControlMenu.A2ViewModel

    Dim dcProxy As TesoreriaDomainContext
    Dim dcArchivosProxy As ImportacionesDomainContext
    Dim cambiandoSeleccionarTodos As Boolean = False

    Private strArchivoBorrar As String 'CORREC_CITI_SV_2014

    Public Const CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO = "ComprobantesEgresoTesoreria"

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New TesoreriaDomainContext()
                dcArchivosProxy = New ImportacionesDomainContext
            Else
                dcProxy = New TesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcArchivosProxy = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
            End If
            'CORREC_CITI_SV_2014
            DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)


            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.GetListaComprobantesEgresoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerComprobantesEgreso, Nothing)
                dcArchivosProxy.Load(dcArchivosProxy.TraerArchivosDirectorioQuery(CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivos, Nothing)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "PlanoComEgresosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


#Region "Propiedades"
    Public _ListaArchivosGuardados As EntitySet(Of OyDImportaciones.Archivo)
    Public Property ListaArchivosGuardados As EntitySet(Of OyDImportaciones.Archivo)
        Get
            Return _ListaArchivosGuardados
        End Get
        Set(ByVal value As EntitySet(Of OyDImportaciones.Archivo))
            _ListaArchivosGuardados = value
            CambioItem("ListaArchivosGuardados")
            CambioItem("ListaArchivosGuardadosPaged")
        End Set
    End Property

    Public ReadOnly Property ListaArchivosGuardadosPaged() As PagedCollectionView
        Get
            If Not IsNothing(ListaArchivosGuardados) Then
                Dim view = New PagedCollectionView(ListaArchivosGuardados)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Public _ListaComEgresosSeleccionar As EntitySet(Of ComEgresosSeleccionar)
    Public Property ListaComEgresosSeleccionar As EntitySet(Of ComEgresosSeleccionar)
        Get
            Return _ListaComEgresosSeleccionar
        End Get
        Set(ByVal value As EntitySet(Of ComEgresosSeleccionar))
            _ListaComEgresosSeleccionar = value
            CambioItem("ListaComEgresosSeleccionar")
        End Set
    End Property

    Private _SeleccionarTodos As Boolean = False
    Public Property SeleccionarTodos As Boolean
        Get
            Return _SeleccionarTodos
        End Get
        Set(ByVal value As Boolean)
            _SeleccionarTodos = value
            If Not cambiandoSeleccionarTodos Then
                For Each c As ComEgresosSeleccionar In ListaComEgresosSeleccionar
                    c.Imprimir = value
                Next
            End If
            CambioItem("SeleccionarTodos")
        End Set
    End Property

    Private _RutaArchivoPlano As String
    Public Property RutaArchivoPlano As String
        Get
            Return _RutaArchivoPlano
        End Get
        Set(ByVal value As String)
            _RutaArchivoPlano = value
            CambioItem("RutaArchivoPlano")
        End Set
    End Property

    Private _Resultado As String
    Public Property Resultado As String
        Get
            Return _Resultado
        End Get
        Set(ByVal value As String)
            _Resultado = value
            CambioItem("Resultado")
        End Set
    End Property

#End Region

#Region "Métodos"
    Public Sub CambioItemSeleccionado()
        If Not cambiandoSeleccionarTodos Then
            cambiandoSeleccionarTodos = True
            CambioSeleccionarTodos()
            cambiandoSeleccionarTodos = False
        End If
    End Sub
    Private Sub CambioSeleccionarTodos()
        SeleccionarTodos = Not ListaComEgresosSeleccionar.Any(Function(r) Not r.Imprimir)
    End Sub

    Public Sub Generar()
        Try
            If Not ListaComEgresosSeleccionar Is Nothing Then
                If ListaComEgresosSeleccionar.Any Then
                    If ListaComEgresosSeleccionar.Any(Function(r) r.Imprimir) Then

                        If Not String.IsNullOrEmpty(RutaArchivoPlano) Then
                            'If Not RutaArchivoPlano.EndsWith(".txt") Then
                            RutaArchivoPlano = RutaArchivoPlano & ".csv"
                            'End If
                            Dim separador As String = ","
                            Dim listaSeleccionados = ListaComEgresosSeleccionar.Where(Function(r) r.Imprimir).OrderBy(Function(r) r.Comprobante)
                            Dim comprobantes As String = String.Join(separador, listaSeleccionados.Select(Function(r) r.Comprobante).ToArray)
                            Dim consecutivos As String = String.Join(separador, listaSeleccionados.Select(Function(r) r.Consecutivo).ToArray)

                            IsBusy = True
                            dcProxy.GenerarComprobantesDeEgreso(comprobantes, consecutivos, separador, RutaArchivoPlano, CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO, Program.Usuario, Program.HashConexion, AddressOf TerminoGenerarComprobantes, Nothing)
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("Debe definir el archivo plano que contendrá los comprobantes.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar los comprobantes a que se van a imprimir.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("No existen comprobantes para generar el archivo plano.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("No existen comprobantes para generar el archivo plano.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema cuando se trataba de Generar el Comprobante.", _
                                                             Me.ToString(), "Generar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    'Public Sub NavegarArchivo(ByVal archivo As OyDImportaciones.Archivo)
    '    'CORREC_CITI_SV_2014 - Se añade el manejo del error
    '    Try
    '        If Not IsNothing(archivo) Then
    '            If (Application.Current.IsRunningOutOfBrowser) Then
    '                'para que cargue el visor de reportes para cuando la consola se ejecuta por fuera del browser
    '                Dim button As New MyHyperlinkButton
    '                button.NavigateUri = New Uri(archivo.RutaWeb)
    '                button.TargetName = "_blank"
    '                button.ClickMe()
    '            Else
    '                HtmlPage.Window.Navigate(New Uri(archivo.RutaWeb), "_blank")
    '            End If
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar la ventana", _
    '                                                         Me.ToString(), "NavegarArchivo" & archivo.RutaWeb, Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub

    'CORREC_CITI_SV_2014
    'Se corrige el modo de hacer la pregunta ya que siempre se estava yendo por el si y borraba el archivo
    Public Sub BorrarArchivo(ByVal archivo As OyDImportaciones.Archivo)
        If Not IsNothing(archivo) Then
            strArchivoBorrar = archivo.Nombre
            mostrarMensajePregunta("¿Está seguro de borrar este archivo?", _
                                          Program.TituloSistema, _
                                          "BORRAR", _
                                          AddressOf TerminoPreguntaBorrar, False)
        End If
    End Sub

    'CORREC_CITI_SV_2014
    Private Sub TerminoPreguntaBorrar(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            dcArchivosProxy.BorrarArchivo(strArchivoBorrar, CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO, Program.Usuario, Program.HashConexion, AddressOf TerminoBorrarArchivo, Nothing)
        End If
    End Sub
#End Region

#Region "Eventos"

    Private Sub TerminoTraerComprobantesEgreso(ByVal lo As LoadOperation(Of ComEgresosSeleccionar))
        Try
            If Not lo.HasError Then
                ListaComEgresosSeleccionar = dcProxy.ComEgresosSeleccionars
                For Each c As ComEgresosSeleccionar In ListaComEgresosSeleccionar
                    c.Imprimir = False
                Next
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Comprobantes", _
                                                 Me.ToString(), "TerminoTraerComprobantesEgreso", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Comprobantes", _
                                                             Me.ToString(), "TerminoTraerComprobantesEgreso", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

    Private archivo As IO.Stream
    Private Sub TerminoGenerarComprobantes(ByVal lo As InvokeOperation(Of Boolean))
        Try
            If Not lo.HasError Then

                A2Utilidades.Mensajes.mostrarMensaje("El archivo se ha generado correctamente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)

                IsBusy = True
                dcProxy.ComEgresosSeleccionars.Clear()
                dcProxy.Load(dcProxy.GetListaComprobantesEgresoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerComprobantesEgreso, Nothing)

                dcArchivosProxy.Archivos.Clear()
                dcArchivosProxy.Load(dcArchivosProxy.TraerArchivosDirectorioQuery(CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivos, Nothing)

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la Generación de los Comprobantes", _
                                                 Me.ToString(), "TerminoGenerarComprobantes", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la Generación de los Comprobantes", _
                                                             Me.ToString(), "TerminoGenerarComprobantes", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

    Private Sub TerminoTraerArchivos(ByVal lo As LoadOperation(Of OyDImportaciones.Archivo))
        Try
            If Not lo.HasError Then
                ListaArchivosGuardados = dcArchivosProxy.Archivos
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Archivos", _
                                                 Me.ToString(), "TerminoTraerArchivos", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Archivos", _
                                                             Me.ToString(), "TerminoTraerArchivos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

    Private Sub TerminoBorrarArchivo(ByVal e As InvokeOperation)
        If Not IsNothing(e.Error) Then
            MessageBox.Show(e.Error.Message)
            e.MarkErrorAsHandled()
        End If
        dcArchivosProxy.Archivos.Clear()
        dcArchivosProxy.Load(dcArchivosProxy.TraerArchivosDirectorioQuery(CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivos, Nothing)
    End Sub
#End Region

    'Public Class MyHyperlinkButton
    '    Inherits HyperlinkButton
    '    Public Sub ClickMe()
    '        MyBase.OnClick()
    '    End Sub
    'End Class

End Class
