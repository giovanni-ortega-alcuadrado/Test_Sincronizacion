Imports Telerik.Windows.Controls
' Descripción:    Child Window creado para mostrar el combo de la Causa de anulación de un documento de Tesorería.
' Creado por:     Sebastian Londoño Benitez
' Fecha:          Marzo 5/2013


Imports System.ComponentModel

Partial Public Class cwMotivoAnulacion
    Inherits Window
    Implements INotifyPropertyChanged

    Public Sub New()
        InitializeComponent()
        'cmbCausal.SelectedIndex = 0
        Me.LayoutRoot.DataContext = Me
    End Sub

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        Me.DialogResult = True
    End Sub

    Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles CancelButton.Click
        Me.DialogResult = False
    End Sub

    Private _CausalInactividad As String 
    Public Property CausalInactividad As String
        Get
            Return _CausalInactividad
        End Get
        Set(ByVal value As String)
            _CausalInactividad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CausalInactividad"))
        End Set
    End Property

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
