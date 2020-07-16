Imports System.Windows.Interactivity
Imports System.Windows.Controls

Namespace TriggersAndBehavior

    Public Class A2SetFocusTrigger
        Inherits TargetedTriggerAction(Of Control)

        Protected Overrides Sub Invoke(parameter As Object)
            If Target IsNot Nothing Then
                System.Windows.Browser.HtmlPage.Plugin.Focus()
                Target.Focus()
                'TODO: Add types
                If TypeOf Target Is TextBox Then
                    DirectCast(Target, TextBox).SelectAll()
                End If
                Target.UpdateLayout()
            End If
        End Sub


    End Class

End Namespace