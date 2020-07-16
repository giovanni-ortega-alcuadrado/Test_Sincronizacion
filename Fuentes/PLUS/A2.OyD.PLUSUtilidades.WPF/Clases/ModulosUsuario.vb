Imports Telerik.Windows.Controls
Imports System.ComponentModel

Public Class Utilidades_ModulosUsuario
    Implements INotifyPropertyChanged


    Private _Modulo As String = String.Empty
    Public Property Modulo() As String
        Get
            Return _Modulo
        End Get
        Set(ByVal value As String)
            _Modulo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Modulo"))
        End Set
    End Property

    Private _TituloModulo As String
    Public Property TituloModulo() As String
        Get
            Return _TituloModulo
        End Get
        Set(ByVal value As String)
            _TituloModulo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TituloModulo"))
        End Set
    End Property

    Private _TituloVistaModulo As String
    Public Property TituloVistaModulo() As String
        Get
            Return _TituloVistaModulo
        End Get
        Set(ByVal value As String)
            _TituloVistaModulo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TituloVistaModulo"))
        End Set
    End Property

    Private _Orden As Integer
    Public Property Orden() As Integer
        Get
            Return _Orden
        End Get
        Set(ByVal value As Integer)
            _Orden = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Orden"))
        End Set
    End Property

    Private _TieneContenido As Boolean = False
    Public Property TieneContenido() As Boolean
        Get
            Return _TieneContenido
        End Get
        Set(ByVal value As Boolean)
            _TieneContenido = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TieneContenido"))
        End Set
    End Property

    Private _CamposControlMenu As Dictionary(Of String, A2ControlMenu.BotonMenu)
    Public Property CamposControlMenu() As Dictionary(Of String, A2ControlMenu.BotonMenu)
        Get
            Return _CamposControlMenu
        End Get
        Set(ByVal value As Dictionary(Of String, A2ControlMenu.BotonMenu))
            _CamposControlMenu = value
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class