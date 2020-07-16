'Codigo Creado Por: Edgar Orlando Muñoz Carvajal
'Archivo: ImportacionArchivosGenericoViewModel.vb
'Generado el : 07/30/2013
'Propiedad de Alcuadrado S.A. 2011

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
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades

Public Class ImportacionArchivosGenericoViewModel
    Inherits A2ControlMenu.A2ViewModel
    Private _strNombreArchivoImportado As String = String.Empty
    Private dcProxy As ImportacionesDomainContext
    Private mdcProxyUtilidad As UtilidadesDomainContext
    Public paccion As String = ""
    Public lngidconcepto As Integer = 0


    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New ImportacionesDomainContext
            mdcProxyUtilidad = New UtilidadesDomainContext
        Else
            dcProxy = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            mdcProxyUtilidad = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If

        DirectCast(dcProxy.DomainClient, WebDomainClient(Of ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, A2ComunesControl.FuncionesCompartidas.TimeUpProxy, 0)

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                mdcProxyUtilidad.Load(mdcProxyUtilidad.cargarCombosSistemasCargaArchivosQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerSistemas, Nothing)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ImportacionArchivosGenericoViewModel.New", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub LimpiarResultados()
        If Not Resultados Is Nothing Then Resultados.Clear()
    End Sub

#Region "Asincronos"

    Private Sub TerminoTraerSistemas(lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Dim ListaCombos As List(Of OYDUtilidades.ItemCombo)

        Try
            If Not lo.HasError Then
                ListaCombos = lo.Entities.ToList
                ListaComboSistema = ListaCombos.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los datos del combo de sistemas de carga de archivos",
                                                 Me.ToString(), "TerminoTraerSistemas", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
            IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los datos del combo de sistemas de carga de archivos",
                                                Me.ToString(), "TerminoTraerSistemas", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminaPregunta(lo As MessageBoxResult)
        If lo = MessageBoxResult.Yes Then
            IsBusy = True
            'dcProxy.Load(dcProxy.NroDeRegistrosDCVQuery(True, Program.Usuario, Program.HashConexion), AddressOf TerminaConsultaNroRegistros, Nothing)
        Else
            _strResultados.Append(vbCrLf)
            _strResultados.Append(Now.ToString() & " - " & "Carga de archivo cancelada.")
            MyBase.CambioItem("Resultados")
        End If
    End Sub

    Private Sub TerminoTraerCargasArchivo(ByVal lo As LoadOperation(Of CargasArchivo))
        Try
            If IsNothing(lo.Error) Then
                ListaCargasArchivos = dcProxy.CargasArchivos.ToList
                CargasArchivoSeleccionado = _listaCargasArchivos.FirstOrDefault()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción",
                                     Me.ToString(), "TerminoTraerCargasArchivo", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción",
                                 Me.ToString(), "TerminoTraerCargasArchivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Sub CargarArchivo(pstrModulo As String, NombreArchivo As String)
        Try
            IsBusy = True

            If CargasArchivoSeleccionado.strModulo <> "BloqueoDesbloqueoClientes" Then
                ViewImportarArchivo.IsBusy = True
            End If
            If Not IsNothing(dcProxy.RespuestaArchivoImportacions) Then
                dcProxy.RespuestaArchivoImportacions.Clear()
            End If

            dcProxy.Load(dcProxy.CargarArchivoImportacionesQuery(pstrModulo, NombreArchivo, _cargasArchivoSeleccionado.strArchivoFormato, Program.Usuario, _cargasArchivoSeleccionado.intFilasADescartar, paccion, lngidconcepto, Program.HashConexion), AddressOf TerminoCargarArchivo, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar el archivo recibo.",
                               Me.ToString(), "CargarArchivoRecibos", Application.Current.ToString(), Program.Maquina, ex)
        End Try


    End Sub

    Private Sub TerminoGenerarArchivo(ByVal e As LoadOperation(Of GenerarArchivosPlanos))
        Try
            IsBusy = False
            If Not e.HasError Then
                If e.Entities.Count > 0 Then
                    If e.Entities.First.Exitoso Then
                        TerminoCrearArchivo()
                    Else
                        Dim ex1 As New Exception(e.Entities.First.Mensaje)
                        A2Utilidades.Mensajes.mostrarMensaje("Se presentó un error en el momento de generar el archivo plano." & ex1.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Se presentó un error en el momento de generar el archivo plano." & e.Error.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error en el momento de generar el archivo plano.",
                                 Me.ToString(), "TerminoGenerarArchivoPlano", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub TerminoCrearArchivo()
        Try
            Dim cwCar As New ListarArchivosDirectorioView(CargasArchivoSeleccionado.strModulo + "xls") 'CSTR_NOMBREPROCESO_TITULOSACTIVOS)
            Program.Modal_OwnerMainWindowsPrincipal(cwCar)
            cwCar.ShowDialog()
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al levantar la ventana de visualización de los archivos",
                                 Me.ToString(), "TerminoCrearArchivo", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

#End Region


#Region "Propiedades"

    Private _strResultados As New StringBuilder
    Public Property Resultados() As StringBuilder
        Get
            Return _strResultados
        End Get
        Set(ByVal value As StringBuilder)
            _strResultados = value
            MyBase.CambioItem("Resultados")
        End Set
    End Property

    Private _listaCargasArchivos As List(Of CargasArchivo)
    Public Property ListaCargasArchivos() As List(Of CargasArchivo)
        Get
            Return _listaCargasArchivos
        End Get
        Set(ByVal value As List(Of CargasArchivo))
            _listaCargasArchivos = value
            MyBase.CambioItem("ListaCargasArchivos")
            CargasArchivoSeleccionado = _listaCargasArchivos.FirstOrDefault
        End Set
    End Property

    Private _cargasArchivoSeleccionado As CargasArchivo
    Public Property CargasArchivoSeleccionado() As CargasArchivo
        Get
            Return _cargasArchivoSeleccionado
        End Get
        Set(ByVal value As CargasArchivo)
            If Not IsNothing(value) Then
                _cargasArchivoSeleccionado = value
                MyBase.CambioItem("CargasArchivoSeleccionado")
            End If
        End Set
    End Property

    Private _listaComboSistema As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaComboSistema As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _listaComboSistema
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _listaComboSistema = value
            MyBase.CambioItem("ListaComboSistema")
        End Set
    End Property

    Private _sistemaSelected As OYDUtilidades.ItemCombo
    Public Property SistemaSelected() As OYDUtilidades.ItemCombo
        Get
            Return _sistemaSelected
        End Get
        Set(ByVal value As OYDUtilidades.ItemCombo)
            _sistemaSelected = value
            If Not IsNothing(value) Then
                IsBusy = True
                dcProxy.CargasArchivos.Clear()
                dcProxy.Load(dcProxy.archivosCargaGenericaConsultarQuery(value.ID, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCargasArchivo, Nothing)
            End If
            MyBase.CambioItem("SistemaSelected")
        End Set
    End Property

    Private _ViewImportarArchivo As New cwCargaArchivos
    Public Property ViewImportarArchivo() As cwCargaArchivos
        Get
            Return _ViewImportarArchivo
        End Get
        Set(ByVal value As cwCargaArchivos)
            _ViewImportarArchivo = value
        End Set
    End Property

#End Region

    Private Sub TerminoCargarArchivo(ByVal lo As LoadOperation(Of OyDImportaciones.RespuestaArchivoImportacion))
        Try
            IsBusy = False
            If lo.HasError = False Then
                If CargasArchivoSeleccionado.strModulo = "BloqueoDesbloqueoClientes" Then
                    IsBusy = True
                    mdcProxyUtilidad.Load(mdcProxyUtilidad.GenerarArchivoPlanoQuery(CargasArchivoSeleccionado.strModulo + "xls", "BloqueoDesbloqueo", "", String.Format("tmpIconsistenciasDesbloqueo_{0:yyyyMMddHHmmss}", Now), "TAB", "EXCEL", Program.Maquina, Program.Usuario, Program.HashConexion), AddressOf TerminoGenerarArchivo, Nothing)
                    Exit Sub
                End If
                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacion)
                Dim objListaMensajes As New List(Of String)

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    If objListaRespuesta.Where(Function(i) i.Exitoso = False).Count > 0 Then

                        objListaMensajes.Add("El archivo genero algunas inconsistencias al intentar subirlo:")

                        For Each li In objListaRespuesta.Where(Function(i) i.Exitoso = False).OrderBy(Function(o) o.Tipo)
                            If li.Tipo = "C" Then
                                objListaMensajes.Add(li.Mensaje)
                            Else
                                objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: {2}", li.Fila, li.Columna, li.Mensaje))
                            End If
                        Next

                        objListaMensajes.Add("No se importaron registros debido a que se presentaron inconsistencias")

                        ViewImportarArchivo.ListaMensajes = objListaMensajes
                        ViewImportarArchivo.IsBusy = False
                    Else
                        For Each li In objListaRespuesta.Where(Function(i) i.Exitoso)
                            objListaMensajes.Add(li.Mensaje)
                        Next
                        ViewImportarArchivo.ListaMensajes = objListaMensajes
                        ViewImportarArchivo.IsBusy = False
                    End If
                Else
                    ViewImportarArchivo.ListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")
                    ViewImportarArchivo.IsBusy = False
                End If
            Else

                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el archivo.", Me.ToString(), "TerminoCargarArchivo", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                lo.MarkErrorAsHandled()
                ViewImportarArchivo.IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el archivo.", Me.ToString(), "TerminoCargarArchivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            ViewImportarArchivo.IsBusy = False
        End Try
    End Sub

End Class
