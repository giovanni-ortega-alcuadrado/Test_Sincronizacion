Imports System.Windows.Interactivity
Imports System.Windows.Input
Imports System.Windows.Controls

Namespace TriggersAndBehavior

    Public Class A2EnterKeyDownEventTrigger
        Inherits EventTrigger

        Public Sub New()
            MyBase.New("KeyDown")
        End Sub

        Protected Overrides Sub OnEvent(eventArgs As EventArgs)
            Dim e = TryCast(eventArgs, KeyEventArgs)
            If e IsNot Nothing AndAlso e.Key = Key.Enter Then
                'Update ViewModel Property
                Select Case Me.Source.GetType().Name
                    Case GetType(TextBox).Name
                        If DirectCast(Me.Source, TextBox).GetBindingExpression(TextBox.TextProperty) IsNot Nothing Then
                            DirectCast(Me.Source, TextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource()
                        End If
                    Case GetType(PasswordBox).Name
                        If DirectCast(Me.Source, PasswordBox).GetBindingExpression(PasswordBox.PasswordProperty) IsNot Nothing Then
                            DirectCast(Me.Source, PasswordBox).GetBindingExpression(PasswordBox.PasswordProperty).UpdateSource()
                        End If
                End Select
                Me.InvokeActions(eventArgs)
            End If
        End Sub

    End Class

End Namespace