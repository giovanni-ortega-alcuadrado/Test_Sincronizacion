Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSTesoreria
Imports A2ComunesControl
Imports A2Utilidades.Mensajes
Imports System.Text.RegularExpressions
Imports System.Globalization
Imports System.Threading
Imports Microsoft.VisualBasic
Imports OpenRiaServices.DomainServices.Client

Public Class SubirArchivoFondosViewModel_OYDPLUS
    Inherits A2ControlMenu.A2ViewModel


#Region "Declaraciones"
    Dim dcProxyUtilidades As UtilidadesDomainContext
    Dim dcProxyUtilidadesPLUS As OYDPLUSUtilidadesDomainContext
    Dim dcProxyImportaciones As ImportacionesDomainContext

#End Region

#Region "Constantes"

    Private Const STRPROCESO As String = "ImpFondos"


#End Region

#Region "Inicializacion"
    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxyUtilidades = New UtilidadesDomainContext()
                dcProxyUtilidadesPLUS = New OYDPLUSUtilidadesDomainContext()
                dcProxyImportaciones = New ImportacionesDomainContext
            Else
                dcProxyUtilidades = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
                dcProxyUtilidadesPLUS = New OYDPLUSUtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYDPLUS)))
                dcProxyImportaciones = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
            End If

            DirectCast(dcProxyUtilidades.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 900)
            DirectCast(dcProxyUtilidadesPLUS.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.OYDPLUSUtilidadesDomainContext.IOYDPLUSUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 900)
            DirectCast(dcProxyImportaciones.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 900)

            CargarCombosOYDPLUS(String.Empty, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "TesoreriaViewModelOyDPlus.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

#Region "PROPIEDADES"

    Private _DiccionarioCombosOYDPlus As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
    Public Property DiccionarioCombosOYDPlus() As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
        Get
            Return _DiccionarioCombosOYDPlus
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor)))
            _DiccionarioCombosOYDPlus = value
            MyBase.CambioItem("DiccionarioCombosOYDPlus")
        End Set
    End Property

    Private _TipoImportacion As String = String.Empty
    Public Property TipoImportacion() As String
        Get
            Return _TipoImportacion
        End Get
        Set(ByVal value As String)
            _TipoImportacion = value
            MyBase.CambioItem("TipoImportacion")
        End Set
    End Property

    Private _EliminarRegistros As Boolean = False
    Public Property EliminarRegistros() As Boolean
        Get
            Return _EliminarRegistros
        End Get
        Set(ByVal value As Boolean)
            _EliminarRegistros = value
            MyBase.CambioItem("EliminarRegistros")
        End Set
    End Property

    Private _NombreArchivo As String = String.Empty
    Public Property NombreArchivo() As String
        Get
            Return _NombreArchivo
        End Get
        Set(ByVal value As String)
            _NombreArchivo = value
            MyBase.CambioItem("NombreArchivo")
        End Set
    End Property


#End Region

#Region "Metodos"

    Public Sub CargarCombosOYDPLUS(ByVal pstrIDReceptor As String, ByVal pstrUserState As String)

        Try
            IsBusy = True
            If Not IsNothing(dcProxyUtilidadesPLUS.CombosReceptors) Then
                dcProxyUtilidadesPLUS.CombosReceptors.Clear()
            End If
            dcProxyUtilidadesPLUS.Load(dcProxyUtilidadesPLUS.OYDPLUS_ConsultarCombosReceptorQuery(pstrIDReceptor, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombosOYD, pstrUserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar los combos de la pantalla.", _
                                 Me.ToString(), "CargarCombosOYDPLUS", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub ImportarArchivo()
        Try
            If String.IsNullOrEmpty(_TipoImportacion) Then
                mostrarMensaje("El tipo de importación es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If String.IsNullOrEmpty(_NombreArchivo) Then
                mostrarMensaje("El archivo es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If EliminarRegistros Then
                mostrarMensajePregunta("¿Esta Seguro que desea eliminar los registros actuales?", _
                                           Program.TituloSistema, _
                                           "ELIMINARREGISTROS", _
                                           AddressOf TerminoMensajePregunta, False)
            Else
                IsBusy = True
                If Not IsNothing(dcProxyImportaciones.RespuestaArchivoImportacions) Then
                    dcProxyImportaciones.RespuestaArchivoImportacions.Clear()
                End If

                dcProxyImportaciones.Load(dcProxyImportaciones.CargarArchivoImportacionFondosQuery(NombreArchivo, STRPROCESO, TipoImportacion, EliminarRegistros, Program.Usuario, Program.HashConexion), AddressOf TerminoCargarArchivoRecibos, String.Empty)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al importar el archivo.", _
                                 Me.ToString(), "ImportarArchivo", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Public Sub TerminoMensajePregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            IsBusy = True
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

            If objResultado.DialogResult Then
                IsBusy = True
                If Not IsNothing(dcProxyImportaciones.RespuestaArchivoImportacions) Then
                    dcProxyImportaciones.RespuestaArchivoImportacions.Clear()
                End If

                dcProxyImportaciones.Load(dcProxyImportaciones.CargarArchivoImportacionFondosQuery(NombreArchivo, STRPROCESO, TipoImportacion, EliminarRegistros, Program.Usuario, Program.HashConexion), AddressOf TerminoCargarArchivoRecibos, String.Empty)
            Else
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la respuesta del mensaje.", _
                                                             Me.ToString(), "TerminoMensajePregunta", Application.Current.ToString(), Program.Maquina, ex)

        End Try

    End Sub
    
#End Region

#Region "Resultados Asincrónicos"

    Private Sub TerminoConsultarCombosOYD(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.CombosReceptor))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    Dim strNombreCategoria As String = String.Empty
                    Dim objListaNodosCategoria As List(Of OYDPLUSUtilidades.CombosReceptor) = Nothing
                    Dim objDiccionario As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))
                    Dim objDiccionarioCompleto As New Dictionary(Of String, List(Of OYDPLUSUtilidades.CombosReceptor))

                    Dim listaCategorias = From lc In lo.Entities Select lc.Categoria Distinct

                    For Each li In listaCategorias
                        strNombreCategoria = li
                        objListaNodosCategoria = (From ln In lo.Entities Where ln.Categoria = strNombreCategoria).ToList
                        objDiccionario.Add(strNombreCategoria, objListaNodosCategoria)
                        objDiccionarioCompleto.Add(strNombreCategoria, objListaNodosCategoria)
                    Next

                    If Not IsNothing(DiccionarioCombosOYDPlus) Then
                        DiccionarioCombosOYDPlus.Clear()
                    End If

                    DiccionarioCombosOYDPlus = Nothing

                    DiccionarioCombosOYDPlus = objDiccionario
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener la lista de Combos de la aplicación.", Me.ToString(), "TerminoConsultarCombosOYD", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al obtener la lista de Combos de la aplicación.", Me.ToString(), "TerminoConsultarCombosOYD", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try

        IsBusy = False
    End Sub

    Private Sub TerminoCargarArchivoRecibos(ByVal lo As LoadOperation(Of OyDImportaciones.RespuestaArchivoImportacion))
        Try
            If lo.HasError = False Then
                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacion)
                Dim objListaMensajes As New List(Of String)
                Dim ViewImportarArchivo As New ImportarArchivoRecibos()
                Dim objTipoImportacion As String = String.Empty

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    If objListaRespuesta.Where(Function(i) i.Exitoso = False And i.Fila = 0).Count > 0 Then
                        objListaMensajes.Add(objListaRespuesta.Where(Function(i) i.Exitoso = False And i.Fila = 0).First.Mensaje)
                    End If

                    If objListaRespuesta.Where(Function(i) i.Exitoso = True And i.Fila = 0).Count > 0 Then
                        objListaMensajes.Add(objListaRespuesta.Where(Function(i) i.Exitoso = True And i.Fila = 0).First.Mensaje)
                        If String.IsNullOrEmpty(objTipoImportacion) Then
                            objTipoImportacion = objListaRespuesta.Where(Function(i) i.Exitoso = True And i.Fila = 0).First.Tipo
                        End If
                    End If

                    If objListaRespuesta.Where(Function(i) i.Exitoso = False).Count > 0 Then
                        Dim objTipo As String = String.Empty
                        For Each li In objListaRespuesta.Where(Function(i) i.Exitoso = False).OrderBy(Function(o) o.Tipo)
                            If li.Fila > 0 Then
                                If objTipo <> li.Tipo Then
                                    If String.IsNullOrEmpty(objTipoImportacion) Then
                                        objTipoImportacion = li.Tipo
                                    End If
                                    objTipo = li.Tipo
                                    objListaMensajes.Add(String.Format("Hoja {0}", li.Tipo))
                                End If

                                objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: {2}", li.Fila, li.Columna, li.Mensaje))
                            End If
                        Next

                        ViewImportarArchivo.ListaMensajes = objListaMensajes
                        ViewImportarArchivo.ListaRespuestaImportacion = objListaRespuesta
                        ViewImportarArchivo.ExportarListaRespuestaImportacion = True
                        ViewImportarArchivo.IsBusy = False

                        ViewImportarArchivo.NombreArchivo = objTipoImportacion
                        ViewImportarArchivo.Title = "Subir archivo fondos"
                    Else
                        If objListaMensajes.Count = 0 Then
                            ViewImportarArchivo.ListaMensajes.Add("Se importo el archivo exitosamente.")
                        End If

                        ViewImportarArchivo.ListaMensajes = objListaMensajes
                        ViewImportarArchivo.IsBusy = False
                        ViewImportarArchivo.NombreArchivo = objTipoImportacion
                        ViewImportarArchivo.Title = "Subir archivo fondos"
                    End If
                Else
                    ViewImportarArchivo.ListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")
                    ViewImportarArchivo.IsBusy = False
                    ViewImportarArchivo.NombreArchivo = objTipoImportacion
                    ViewImportarArchivo.Title = "Subir archivo fondos"
                End If

                Program.Modal_OwnerMainWindowsPrincipal(ViewImportarArchivo)
                ViewImportarArchivo.ShowDialog()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el archivo de carga de fondos.", Me.ToString(), "TerminoCargarArchivoRecibos", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el archivo de carga de fondos.", Me.ToString(), "TerminoCargarArchivoRecibos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
        IsBusy = False
    End Sub

#End Region

End Class