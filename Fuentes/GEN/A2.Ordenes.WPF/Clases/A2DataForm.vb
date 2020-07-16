Imports Telerik.Windows.Controls
Imports System.Windows

Public Class A2DataForm
    Inherits RadDataForm

    Private Const DATAFORM_stateDisabled As String = "Disabled"
    Private Const DATAFORM_stateNormal As String = "Normal"

    Public Overrides Sub OnApplyTemplate()

        AddHandler IsEnabledChanged, AddressOf A2DataForm_IsEnabledChanged
        MyBase.OnApplyTemplate()
    End Sub

    Private Sub A2DataForm_IsEnabledChanged(ByVal sender As Object, ByVal e As DependencyPropertyChangedEventArgs)
        If Not IsEnabled Then
            VisualStateManager.GoToState(Me, DATAFORM_stateDisabled, True)
        Else
            VisualStateManager.GoToState(Me, DATAFORM_stateNormal, True)
        End If
    End Sub
End Class