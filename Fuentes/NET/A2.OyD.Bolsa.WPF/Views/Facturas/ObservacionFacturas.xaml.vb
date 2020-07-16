Imports Telerik.Windows.Controls
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDBolsa
Imports Microsoft.VisualBasic.CompilerServices

Partial Public Class ObservacionFacturas
    Inherits Window
    Implements INotifyPropertyChanged
    
    Public strObservaciones As String = String.Empty
    Dim dcProxy As BolsaDomainContext

    'Executes when the user navigates to this page.
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        Me.LayoutRoot.DataContext = Me
        Me.DataContext = Me
    End Sub

#Region "Propiedades"
   
    Private _Observacion As String
    '<Display(Name:="Detalle")> _
    Public Property Observacion As String
        Get
            Return _Observacion
        End Get
        Set(ByVal value As String)
            'If Not IsNothing(value) Then
            _Observacion = value
            'End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Observacion"))
        End Set
    End Property
   
#End Region
#Region "Metodos"
    Private Sub OKButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles OKButton.Click
        Try
            strObservaciones = Observacion
            DialogResult = True
            Me.Close()

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de saldos", _
                                                 Me.ToString(), "TerminoConsultar", Application.Current.ToString(), Program.Maquina, ex.InnerException)

            'IsBusy = False
        End Try

    End Sub
    
    Private Sub CancelButton_Click(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles CancelButton.Click
        Me.Close()
    End Sub
#End Region


        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged


End Class
