Imports System.ComponentModel
Imports GalaSoft.MvvmLight.Command
Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.WEB
Imports OpenRiaServices.DomainServices.Client
Imports System.Collections.ObjectModel
Public Class CorreosViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables"
    Dim window As RadWindow = New RadWindow()
    Dim strEstadoAnterior As String
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Proxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As OrdenesDivisasDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------

#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
        inicializar()
    End Sub


    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            mdcProxy = inicializarProxy()

            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                IsBusy = True
                traerCorreos()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        Finally
            IsBusy = False
        End Try

    End Function
#End Region




#Region "Propiedades"


    Private _ListaCorreos As List(Of clx_CorreosPersonas)
    Public Property ListaCorreos() As List(Of clx_CorreosPersonas)
        Get
            Return _ListaCorreos
        End Get
        Set(ByVal value As List(Of clx_CorreosPersonas))
            _ListaCorreos = value
            'CambioPropiedad("ListaCorreos")
        End Set
    End Property


    Private _ListaCorreosSelected As ObservableCollection(Of clx_CorreosPersonas) = New ObservableCollection(Of clx_CorreosPersonas)
    Public Property ListaCorreosSelected() As ObservableCollection(Of clx_CorreosPersonas)
        Get
            Return _ListaCorreosSelected
        End Get
        Set(ByVal value As ObservableCollection(Of clx_CorreosPersonas))
            If Not IsNothing(value) Then
                _ListaCorreosSelected = value
                'CambioPropiedad("ListaCorreosSelected")
            End If
        End Set
    End Property


#End Region


#Region "Zona Encabezado"

    'metodo para cargar los correos de las personas y terceros
    Private Async Sub traerCorreos()
        Try
            Dim z = Await mdcProxy.GetCorreosReceptores_ConsultarAsync()
            ListaCorreos = z.Value

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Error. traer Correos:",
                                                     Me.ToString(), "traerCorreos", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub


#End Region




End Class
