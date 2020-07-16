Imports Telerik.Windows.Controls
'Codigo Desarrollado por: Sebastian Londoño Benitez
'Archivo: Public Class Public Class GenerarArchivoOperacionesViewModel.vb
'Propiedad de Alcuadrado S.A. 2013

Imports A2.OyD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.Web
Imports System.Text.RegularExpressions
Imports A2Utilidades.Mensajes

Public Class GenerarArchivoOperacionesViewModel
    Inherits A2ControlMenu.A2ViewModel
    Dim dcProxy As TesoreriaDomainContext
    Dim dcProxy1 As TesoreriaDomainContext
    Dim objProxy As UtilidadesDomainContext
    Dim dcArchivosProxy As ImportacionesDomainContext
    Dim _mlogValidoNombrePlano As Boolean = True
    Public Const CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO = "ArchivoPlanoOperaciones"

#Region "Inicializaciones"

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New TesoreriaDomainContext()
            dcProxy1 = New TesoreriaDomainContext()
            objProxy = New UtilidadesDomainContext()
            dcArchivosProxy = New ImportacionesDomainContext()
        Else
            dcProxy = New TesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy1 = New TesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))
            objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
            dcArchivosProxy = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
        End If
        DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 1800)
        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcArchivosProxy.Load(dcArchivosProxy.TraerArchivosDirectorioQuery(CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivos, Nothing)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "LiquidacionesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Metodos"

    ''' <summary>
    ''' Método generaración del Plano Operaciones.
    ''' </summary>
    ''' <remarks>SLB20130607</remarks>
    Public Sub GuardarPlanoOperaciones()
        Try
            If Validaciones() Then
                'C1.Silverlight.C1MessageBox.Show("Está seguro de generar el archivo plano", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, _
                '                                 C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminoPregunta)
                mostrarMensajePregunta("Está seguro de generar el archivo plano", _
                                       Program.TituloSistema, _
                                       "GUARDARPLANO", _
                                       AddressOf TerminoPregunta, True, "¿Generar?")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Si la respuesta es Si procede a Generar el Archivo plano de Operaciones.
    ''' </summary>
    ''' <remarks>SLB20130530</remarks>
    Private Sub TerminoPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                RutaArchivoPlano = RutaArchivoPlano & ".txt"
                IsBusy = True
                dcProxy.GenerarArchivoPlanoOperaciones(Desde, Hasta, RutaArchivoPlano, CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO, _
                                               Program.Usuario, Program.HashConexion, AddressOf TerminoGenerarArchivoPlano, "GenerarPlano")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la generación del plano de operaciones", _
                                 Me.ToString(), "TerminoPregunta", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método de las validaciones para generar los Comprobantes de Egreso.
    ''' </summary>
    ''' <remarks>SLB20130419</remarks>
    Public Function Validaciones() As Boolean
        Validaciones = True

        If Desde > Hasta Then
            A2Utilidades.Mensajes.mostrarMensaje("La fecha del rango inicial es mayor a la fecha final.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Validaciones = False
            Return Validaciones
        End If

        If String.IsNullOrEmpty(RutaArchivoPlano) Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe definir el nombre del archivo plano.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Validaciones = False
            Return Validaciones
        End If

        If Not _mlogValidoNombrePlano Then
            A2Utilidades.Mensajes.mostrarMensaje("El nombre del archivo plano posee caractares no válidos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Validaciones = False
            Return Validaciones
        End If

    End Function

    Public Sub NavegarArchivo(ByVal archivo As OyDImportaciones.Archivo)
        Try
            If Not IsNothing(archivo) Then
                'If (Application.Current.IsRunningOutOfBrowser) Then
                '    'para que cargue el visor de reportes para cuando la consola se ejecuta por fuera del browser
                '    Dim button As New MyHyperlinkButton
                '    button.NavigateUri = New Uri(archivo.RutaWeb)
                '    button.TargetName = "_blank"
                '    button.ClickMe()
                'Else
                '    HtmlPage.Window.Navigate(New Uri(archivo.RutaWeb), "_blank")
                'End If

                Dim objDescargarArchivo As New A2ComunesControl.ucDescargarArchivo()
                objDescargarArchivo.ColocarNombreArchivoDefecto = True
                objDescargarArchivo.DescargarTXTPorNavegador = False
                objDescargarArchivo.DescargarArchivo(archivo.RutaWeb, archivo.Nombre, archivo.Extension)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la navegación del archivo", _
                                 Me.ToString(), "NavegarArchivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub BorrarArchivo(ByVal archivo As OyDImportaciones.Archivo)
        Try
            If Not IsNothing(archivo) Then
                dcArchivosProxy.BorrarArchivo(archivo.Nombre, CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO, Program.Usuario, Program.HashConexion, AddressOf TerminoBorrarArchivo, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar el archivo", _
                                 Me.ToString(), "BorrarArchivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Metodos Asincronicos"

    ''' <summary>
    ''' Metodo que recibe la respuesta de la generación del archivo Plano de Operaciones.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130607</remarks>
    Private Sub TerminoGenerarArchivoPlano(ByVal lo As InvokeOperation(Of Boolean))
        Try
            IsBusy = False
            If Not lo.HasError Then
                A2Utilidades.Mensajes.mostrarMensaje("Archivo generado exitosamente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)

                RutaArchivoPlano = String.Empty
                dcArchivosProxy.Archivos.Clear()
                dcArchivosProxy.Load(dcArchivosProxy.TraerArchivosDirectorioQuery(CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivos, Nothing)
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la generación del archivo plano.", _
                                                 Me.ToString(), "TerminoGenerarArchivoPlano", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la generación del archivo plano.", _
                                                             Me.ToString(), "TerminoGenerarArchivoPlano", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoBorrarArchivo(ByVal e As InvokeOperation)
        Try
            If Not IsNothing(e.Error) Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar el archivo plano.", _
                                                 Me.ToString(), "TerminoBorrarArchivo", Application.Current.ToString(), Program.Maquina, e.Error)
                e.MarkErrorAsHandled()
            End If
            dcArchivosProxy.Archivos.Clear()
            dcArchivosProxy.Load(dcArchivosProxy.TraerArchivosDirectorioQuery(CSTR_NOMBREPROCESO_COMPROBANTES_EGRESO, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArchivos, Nothing)
        Catch ex As Exception

        End Try

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

#End Region

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

    Private _RutaArchivoPlano As String
    Public Property RutaArchivoPlano As String
        Get
            Return _RutaArchivoPlano
        End Get
        Set(ByVal value As String)
            _RutaArchivoPlano = value
            If Not String.IsNullOrEmpty(_RutaArchivoPlano) Then
                If Not Regex.IsMatch(_RutaArchivoPlano, "^[a-z A-ZÑ 0-9 á-ú ._-]*$") Then
                    A2Utilidades.Mensajes.mostrarMensaje("El nombre del archivo plano posee caractares no válidos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    _mlogValidoNombrePlano = False
                Else
                    _mlogValidoNombrePlano = True
                End If
            End If
            CambioItem("RutaArchivoPlano")
        End Set
    End Property

    Private _Desde As Date = Now.Date
    Public Property Desde As Date
        Get
            Return _Desde
        End Get
        Set(ByVal value As Date)
            _Desde = value
            CambioItem("Desde")
        End Set
    End Property

    Private _Hasta As Date = Now.Date
    Public Property Hasta As Date
        Get
            Return _Hasta
        End Get
        Set(ByVal value As Date)
            _Hasta = value
            CambioItem("Hasta")
        End Set
    End Property

#End Region


End Class

