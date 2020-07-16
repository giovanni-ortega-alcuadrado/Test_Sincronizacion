Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu
Imports Microsoft.VisualBasic.CompilerServices
Imports A2.OYD.OYDServer.RIA.Web
Imports System.Globalization
Imports A2ComunesControl
Imports A2Utilidades.Mensajes
Imports A2OYDPLUSUtilidades
Imports OpenRiaServices.DomainServices.Client

Public Class ContenidoOpcionPLUSViewModel
    Implements INotifyPropertyChanged

    Dim objContenidoCargar As Object = Nothing
    Private dcProxy As OYDPLUSOrdenesDomainContext

#Region "Inicialización"

    Public Sub New()
        Try
            IsBusyContenido = True

            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OYDPLUSOrdenesDomainContext()
            Else
                dcProxy = New OYDPLUSOrdenesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ContenidoOpcionViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Propiedades"

    Public Enum CONTENIDOMODULOS
        ORDENES
        ORDENESOF
        DERIVADOS
        MERCAMSOFT
    End Enum

    Public viewContenido As ContenidoOpcionPLUSView

    Private _ModuloSeleccionado As Utilidades_ModulosUsuario
    Public Property ModuloSeleccionado() As Utilidades_ModulosUsuario
        Get
            Return _ModuloSeleccionado
        End Get
        Set(ByVal value As Utilidades_ModulosUsuario)
            _ModuloSeleccionado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ModuloSeleccionado"))
        End Set
    End Property

    Private _IsBusyContenido As Boolean = False
    Public Property IsBusyContenido() As Boolean
        Get
            Return _IsBusyContenido
        End Get
        Set(ByVal value As Boolean)
            _IsBusyContenido = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsBusyContenido"))
        End Set
    End Property


#End Region

#Region "Metodos"

    Public Sub CrearControlModulo()
        Try
            If Not IsNothing(_ModuloSeleccionado) Then
                Select Case _ModuloSeleccionado.Modulo
                    Case CONTENIDOMODULOS.ORDENES.ToString
                        objContenidoCargar = New A2OYDPLUSOrdenesBolsa.OrdenesPLUSView(_ModuloSeleccionado)
                    Case CONTENIDOMODULOS.ORDENESOF.ToString
                        objContenidoCargar = New A2OYDPLUSOrdenesOF.OrdenesPLUSOFView(_ModuloSeleccionado)
                    Case CONTENIDOMODULOS.MERCAMSOFT.ToString
                        objContenidoCargar = New A2OYDPLUSOrdenesDivisas.OrdenesPLUSDivisasView(_ModuloSeleccionado)
                    Case CONTENIDOMODULOS.DERIVADOS.ToString
                        objContenidoCargar = New A2OYDPLUSOrdenesDerivados.OrdenesPLUSDerivadosView(_ModuloSeleccionado)
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ContenidoOpcionViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub InicializarControles()
        Try
            dcProxy.EsperarCargaControl(Program.Usuario, Program.HashConexion, AddressOf TerminoEsperarCargaControl, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicializar los controles.", _
                                 Me.ToString(), "InicializarControles", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoEsperarCargaControl(ByVal lo As InvokeOperation(Of Boolean))
        Try
            If Not IsNothing(_ModuloSeleccionado) Then
                viewContenido.GridContenidoOpcion.Children.Clear()

                If Not IsNothing(objContenidoCargar) Then
                    viewContenido.GridContenidoOpcion.Children.Add(objContenidoCargar)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al esperar la carga del control.", _
                                 Me.ToString(), "TerminoEsperarCargaControl", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        IsBusyContenido = False
    End Sub

#End Region

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class