Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OYD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OYD.OYDServer.RIA.Web.OYDUtilidades
Imports System.Web
Imports System.Collections.ObjectModel
Imports A2.OYD.OYDServer.RIA.Web.OyDTesoreria
Imports A2ComunesImportaciones

Public Class CarterasCuentasActivasConfianzaViewModel
    Inherits A2ControlMenu.A2ViewModel

    Implements INotifyPropertyChanged

    ''' <summary>
    ''' ViewModel para la pantalla CarterasCuentasActivasConfianzaViewModel.
    ''' </summary>
    ''' <history>
    ''' Creado por       : Daniel Esteban Marin (IoSoft S.A.)
    ''' Descripción      : Creacion. 
    ''' Fecha            : 7 de Julio/2016
    '''  
    ''' </history>

#Region "Constantes"

#End Region

#Region "Variables - REQUERIDO"

    Public ViewGenerarRecibosCaja_Intermedia As GenerarRecibosCaja_IntermediaView = Nothing

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As TesoreriaDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private dcProxyImportaciones As ImportacionesDomainContext
    Private mdcProxyUtilidad As UtilidadesDomainContext

#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                mdcProxy = New TesoreriaDomainContext()
                dcProxyImportaciones = New ImportacionesDomainContext
                mdcProxyUtilidad = New UtilidadesDomainContext()
            Else
                mdcProxy = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                dcProxyImportaciones = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
                mdcProxyUtilidad = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            End If

            DirectCast(mdcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 15, 0)
            DirectCast(dcProxyImportaciones.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 15, 0)
            DirectCast(mdcProxyUtilidad.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 15, 0)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function inicializar() As Boolean
        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
                mdcProxy.CarterasColectivasConfianza_RutaArchivo(Program.Usuario, Program.Maquina, Program.HashConexion, AddressOf TerminoConsultarRutaArchivoDefecto, Nothing)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)

    End Function

    Private Sub TerminoConsultarRutaArchivoDefecto(ByVal lo As InvokeOperation(Of String))
        Try
            If Not lo.HasError Then
                If Not String.IsNullOrEmpty(lo.Value) Then
                    NombreArchivo = lo.Value
                    CambioRutaDefecto = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de la ruta por defecto.", _
                     Me.ToString(), "TerminoConsultarRutaArchivoDefecto", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de la ruta por defecto.", _
                     Me.ToString(), "TerminoConsultarRutaArchivoDefecto", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    ''' <summary>
    ''' Lista de Indicadores que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private WithEvents _ListaEncabezado As List(Of GenerarAjustesAutomaticos)
    Public Property ListaEncabezado() As List(Of GenerarAjustesAutomaticos)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As List(Of GenerarAjustesAutomaticos))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de Indicadores para navegar sobre el grid con paginación
    ''' </summary>
    Public ReadOnly Property ListaEncabezadoPaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEncabezado) Then
                If IsNothing(_ListaEncabezadoPaginada) Then
                    Dim view = New PagedCollectionView(_ListaEncabezado)
                    _ListaEncabezadoPaginada = view
                    Return view
                Else
                    Return (_ListaEncabezadoPaginada)
                End If
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _DiccionarioCarga As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
    Public Property DiccionarioCarga() As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
        Get
            Return _DiccionarioCarga
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))
            _DiccionarioCarga = value
            MyBase.CambioItem("DiccionarioCarga")
        End Set
    End Property

    Private _ConsecutivoSeleccionado As String
    Public Property ConsecutivoSeleccionado() As String
        Get
            Return _ConsecutivoSeleccionado
        End Get
        Set(ByVal value As String)
            _ConsecutivoSeleccionado = value
            MyBase.CambioItem("ConsecutivoSeleccionado")
        End Set
    End Property

    Private _TipoAjusteSeleccionado As String
    Public Property TipoAjusteSeleccionado() As String
        Get
            Return _TipoAjusteSeleccionado
        End Get
        Set(ByVal value As String)
            _TipoAjusteSeleccionado = value
            MyBase.CambioItem("TipoAjusteSeleccionado")
        End Set
    End Property

    Private _NombreArchivo As String
    Public Property NombreArchivo() As String
        Get
            Return _NombreArchivo
        End Get
        Set(ByVal value As String)
            _NombreArchivo = value
            MyBase.CambioItem("NombreArchivo")
        End Set
    End Property

    Private _CambioRutaDefecto As Boolean = False
    Public Property CambioRutaDefecto() As Boolean
        Get
            Return _CambioRutaDefecto
        End Get
        Set(ByVal value As Boolean)
            _CambioRutaDefecto = value
            MyBase.CambioItem("CambioRutaDefecto")
        End Set
    End Property


#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"


    ''' <summary>
    ''' Ejecuta el proceso de generar documentos si el usuario presiona el botón Generar RC.
    ''' </summary>
    Public Sub Aceptar()
        Try
            If String.IsNullOrEmpty(NombreArchivo) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe de seleccionar el archivo.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            A2Utilidades.Mensajes.mostrarMensajePregunta("¿Está seguro de procesar el archivo?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf ConfirmarGenerar, False)

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al presionar el botón Generar", Me.ToString(), "Generar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub Ayuda()
        Try
            A2Utilidades.Mensajes.mostrarMensaje("Formato de archivo (ID o NIT cliente,Tipo ID cliente,Código de la cartera,Código de Oficina de la cuenta de inversión,Número de la cuenta de inversión,Fecha generación,Nombre del titular,Nombre de la cartera colectiva)", Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al presionar el botón Generar", Me.ToString(), "Generar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para confirmar si continua el proceso si hay fechas sin valoración inferiores a la fecha de valoración.
    ''' </summary>
    Public Sub ConfirmarGenerar(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If Not CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                Exit Sub
            Else
                A2Utilidades.Mensajes.mostrarMensajePregunta("¿Está totalmente seguro de procesar el archivo?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf ConfirmarTotalmenteGenerar, False)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al preguntar si está seguro de generar los Recibos de Caja", _
                                                             Me.ToString(), "ConfirmarGenerar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConfirmarTotalmenteGenerar(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If Not CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                Exit Sub
            Else
                IsBusy = True
                If Not IsNothing(dcProxyImportaciones.RespuestaArchivoImportacions) Then
                    dcProxyImportaciones.RespuestaArchivoImportacions.Clear()
                End If

                dcProxyImportaciones.Load(dcProxyImportaciones.CuentasActivasConfianza_ProcesarQuery(CambioRutaDefecto, NombreArchivo, "ImpCarterasCuentasConfianza", Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoGenerarDocumentos, String.Empty)
            End If
            
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar los documentos de tesorería.", _
                                                             Me.ToString(), "ConfirmarTotalmenteGenerar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub TerminoGenerarDocumentos(ByVal lo As LoadOperation(Of OyDImportaciones.RespuestaArchivoImportacion))
        Try
            If Not lo.HasError Then
                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacion)
                Dim objListaMensajes As New List(Of String)
                Dim objViewImportarArchivo As New A2ComunesControl.ResultadoGenericoImportaciones()

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    If objListaRespuesta.Where(Function(i) i.Exitoso = False).Count > 0 Then

                        objListaMensajes.Add("El sistema generó algunas inconsistencias al intentar generar los documentos:")

                        For Each li In objListaRespuesta.Where(Function(i) i.Exitoso = False)
                            objListaMensajes.Add(li.Mensaje)
                        Next

                        objViewImportarArchivo.ListaMensajes = objListaMensajes
                    Else
                        objListaMensajes.Add("Se generaron los registros exitosamente:")

                        For Each li In objListaRespuesta.Where(Function(i) i.Exitoso)
                            objListaMensajes.Add(li.Mensaje)
                        Next

                        objViewImportarArchivo.ListaMensajes = objListaMensajes
                    End If
                Else
                    objViewImportarArchivo.ListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")
                End If

                objViewImportarArchivo.Title = "Generar carteras colectivas confianza"
                Program.Modal_OwnerMainWindowsPrincipal(objViewImportarArchivo)
                objViewImportarArchivo.ShowDialog()

                ListaEncabezado = Nothing
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar las carteras colectivas confianza.", Me.ToString(), "TerminoGenerarDocumentos", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar las carteras colectivas confianza.", Me.ToString(), "TerminoGenerarDocumentos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

#End Region

End Class

