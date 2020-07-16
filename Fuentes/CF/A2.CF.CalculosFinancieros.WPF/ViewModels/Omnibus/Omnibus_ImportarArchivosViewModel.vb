Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OYD.OYDServer.RIA.Web
Imports A2.OYD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OYD.OYDServer.RIA.Web.OYDUtilidades
Imports System.Web
Imports System.Collections.ObjectModel
Imports SpreadsheetGear
Imports System.IO
Imports A2MCCoreWPF
Imports GalaSoft.MvvmLight.Threading
Imports A2.Notificaciones.Cliente
Imports A2.OYD.OYDServer.RIA
Imports A2.OYD.OYDServer.RIA.Web.CFUtilidades
Imports A2CFUtilitarios
Imports A2ComunesImportaciones


Public Class Omnibus_ImportarArchivosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Implements INotifyPropertyChanged

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Declaracion de Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As CalculosFinancierosDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private mdcProxyUtilidades As UtilidadesDomainContext
#End Region

    Public Async Function inicializar() As Task(Of Boolean)
        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                If IsNothing(mdcProxy) Then
                    mdcProxy = inicializarProxyCalculosFinancieros()
                End If

                Await ConsultarConfiguracion()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

#Region "Métodos privados del encabezado - REQUERIDOS"

    Private Async Function ConsultarConfiguracion() As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFCalculosFinancieros.Omnibus_Configuracion_Importacion)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            mdcProxy.Omnibus_Combos.Clear()

            objRet = Await mdcProxy.Load(mdcProxy.Omnibus_ConsultarConfiguracion_ImportacionSyncQuery(Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al consultar los registros.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los registros.", Me.ToString(), "ConsultarCombosGestor", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    ListaComboModulos = objRet.Entities.ToList
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los registros.", Me.ToString(), "ConsultarCombosGestor", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    Private Sub ReasignarVariablesModulo()
        Try
            HabilitarSeleccionArchivo = False
            If Not String.IsNullOrEmpty(ImportacionSeleccionada) Then
                HabilitarSeleccionArchivo = True
                If Not IsNothing(ListaComboModulos) Then
                    If ListaComboModulos.Where(Function(i) i.strModulo = ImportacionSeleccionada).Count > 0 Then
                        ExtensionesPermitidasArchivo = ListaComboModulos.Where(Function(i) i.strModulo = ImportacionSeleccionada).First.strExtensionesPermitidas
                        TipoGeneracionArchivo = ListaComboModulos.Where(Function(i) i.strModulo = ImportacionSeleccionada).First.strTipoArchivo
                        FilasADescartar = CInt(ListaComboModulos.Where(Function(i) i.strModulo = ImportacionSeleccionada).First.intFilasADescartar)
                        ColumnasArchivo = CInt(ListaComboModulos.Where(Function(i) i.strModulo = ImportacionSeleccionada).First.intColumnasArchivo)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar los archivos.", Me.ToString(), "ReasignarVariablesModulo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Sub CargarArchivo(pstrModulo As String, NombreArchivo As String)
        Try
            IsBusy = True

            If Not IsNothing(mdcProxy.RetornoInformacionArchivos) Then
                mdcProxy.RetornoInformacionArchivos.Clear()
            End If

            mdcProxy.Load(mdcProxy.Omnibus_ImportarArchivoQuery(pstrModulo, NombreArchivo, pstrModulo, TipoGeneracionArchivo, FilasADescartar, ColumnasArchivo, Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoImportarArchivo, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar el archivo recibo.", _
                               Me.ToString(), "CargarArchivoRecibos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoImportarArchivo(ByVal lo As LoadOperation(Of RetornoInformacionArchivo))
        Try
            IsBusy = False
            If lo.HasError = False Then
                Dim objListaRespuesta As List(Of RetornoInformacionArchivo)
                Dim objListaMensajes As New List(Of String)

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    If objListaRespuesta.Where(Function(i) CBool(i.Exitoso) = False).Count > 0 Then

                        objListaMensajes.Add("El archivo genero algunas inconsistencias al intentar subirlo:")

                        For Each li In objListaRespuesta.Where(Function(i) CBool(i.Exitoso) = False).OrderBy(Function(o) o.Tipo)
                            If li.Tipo = "C" Then
                                objListaMensajes.Add(li.strInformacionGenerar)
                            Else
                                objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: {2}", li.Fila, li.Columna, li.strInformacionGenerar))
                            End If
                        Next

                        objListaMensajes.Add("No se importaron registros debido a que se presentaron inconsistencias")

                        ViewImportarArchivo.ListaMensajes = objListaMensajes
                        ViewImportarArchivo.IsBusy = False
                    Else
                        For Each li In objListaRespuesta.Where(Function(i) CBool(i.Exitoso))
                            objListaMensajes.Add(li.strInformacionGenerar)
                        Next
                        ViewImportarArchivo.ListaMensajes = objListaMensajes
                        ViewImportarArchivo.IsBusy = False
                    End If
                Else
                    ViewImportarArchivo.ListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")
                    ViewImportarArchivo.IsBusy = False
                End If

                NombreArchivoSeleccionado = String.Empty
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

#End Region

#Region "Métodos publicos del encabezado - REQUERIDOS"

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    Private _ListaComboModulos As List(Of Omnibus_Configuracion_Importacion)
    Public Property ListaComboModulos() As List(Of Omnibus_Configuracion_Importacion)
        Get
            Return _ListaComboModulos
        End Get
        Set(ByVal value As List(Of Omnibus_Configuracion_Importacion))
            _ListaComboModulos = value
            MyBase.CambioItem("ListaComboModulos")
        End Set
    End Property

    Private _HabilitarSeleccionArchivo As Boolean = False
    Public Property HabilitarSeleccionArchivo() As Boolean
        Get
            Return _HabilitarSeleccionArchivo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarSeleccionArchivo = value
            MyBase.CambioItem("HabilitarSeleccionArchivo")
        End Set
    End Property

    Private _ImportacionSeleccionada As String
    Public Property ImportacionSeleccionada() As String
        Get
            Return _ImportacionSeleccionada
        End Get
        Set(ByVal value As String)
            _ImportacionSeleccionada = value
            ReasignarVariablesModulo()
            MyBase.CambioItem("ImportacionSeleccionada")
        End Set
    End Property

    Private _ExtensionesPermitidasArchivo As String
    Public Property ExtensionesPermitidasArchivo() As String
        Get
            Return _ExtensionesPermitidasArchivo
        End Get
        Set(ByVal value As String)
            _ExtensionesPermitidasArchivo = value
            MyBase.CambioItem("ExtensionesPermitidasArchivo")
        End Set
    End Property

    Private _TipoGeneracionArchivo As String
    Public Property TipoGeneracionArchivo() As String
        Get
            Return _TipoGeneracionArchivo
        End Get
        Set(ByVal value As String)
            _TipoGeneracionArchivo = value
            MyBase.CambioItem("TipoGeneracionArchivo")
        End Set
    End Property

    Private _FilasADescartar As Integer
    Public Property FilasADescartar() As Integer
        Get
            Return _FilasADescartar
        End Get
        Set(ByVal value As Integer)
            _FilasADescartar = value
            MyBase.CambioItem("FilasADescartar")
        End Set
    End Property

    Private _ColumnasArchivo As Integer
    Public Property ColumnasArchivo() As Integer
        Get
            Return _ColumnasArchivo
        End Get
        Set(ByVal value As Integer)
            _ColumnasArchivo = value
            MyBase.CambioItem("ColumnasArchivo")
        End Set
    End Property

    Private _NombreArchivoSeleccionado As String
    Public Property NombreArchivoSeleccionado() As String
        Get
            Return _NombreArchivoSeleccionado
        End Get
        Set(ByVal value As String)
            _NombreArchivoSeleccionado = value
            MyBase.CambioItem("NombreArchivoSeleccionado")
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

#Region "Resultados Asincrónicos del encabezado - REQUERIDO"


#End Region

End Class