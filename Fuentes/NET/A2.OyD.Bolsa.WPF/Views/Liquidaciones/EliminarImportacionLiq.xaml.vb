Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data

Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades
Imports Microsoft.VisualBasic.CompilerServices
Partial Public Class EliminarImportacionLiqView
    Inherits Window
    Implements INotifyPropertyChanged

#Region "propiedades"

    Public Sub New()
        InitializeComponent()
        Me.DataContext = Me
        '    Eliminar.datacontex()
    End Sub

    Private _Respuesta As String
    Public Property Respuesta As String
        Get
            Return _Respuesta
        End Get
        Set(value As String)
            _Respuesta = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Respuesta"))
        End Set
    End Property

    Private _logtodos As Boolean
    Public Property logtodos As Boolean
        Get
            Return _logtodos
        End Get
        Set(value As Boolean)
            Try
                _logtodos = value
                If _logtodos Then
                    Respuesta = "T"
                End If
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Respuesta"))
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creacion del detalle", _
                         Me.ToString(), "logtodo", Application.Current.ToString(), Program.Maquina, ex.InnerException)
            End Try
        End Set
    End Property

    Private _logOperaciones As Boolean
    Public Property logOperaciones As Boolean
        Get
            Return _logOperaciones
        End Get
        Set(value As Boolean)
            Try
                _logOperaciones = value
                If _logOperaciones Then

                    Respuesta = "0"

                End If
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logOperaciones"))
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creacion del detalle", _
                         Me.ToString(), "logOperaciones", Application.Current.ToString(), Program.Maquina, ex.InnerException)
            End Try
        End Set
    End Property

    Private _logAcciones As Boolean
    Public Property logAcciones As Boolean
        Get
            Return _logAcciones
        End Get
        Set(value As Boolean)
            Try
                _logAcciones = value
                If _logAcciones Then

                    Respuesta = "A"

                End If
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logAcciones"))
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creacion del detalle", _
                         Me.ToString(), "logAcciones", Application.Current.ToString(), Program.Maquina, ex.InnerException)
            End Try
        End Set
    End Property
    Private _logRentaFija As Boolean
    Public Property logRentaFija As Boolean
        Get
            Return _logRentaFija
        End Get
        Set(value As Boolean)
            Try
                _logRentaFija = value
                If _logRentaFija Then

                    Respuesta = "C"

                End If
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logRentaFija"))
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creacion del detalle", _
                         Me.ToString(), "logRentaFija", Application.Current.ToString(), Program.Maquina, ex.InnerException)
            End Try
        End Set
    End Property
#End Region
    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        Me.DialogResult = True
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        Me.Close()
    End Sub

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged


End Class
