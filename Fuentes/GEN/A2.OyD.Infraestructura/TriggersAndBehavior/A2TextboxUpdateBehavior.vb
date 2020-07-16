Imports System.Windows.Interactivity
Imports System.Windows.Controls
Imports System.Windows

Namespace TriggersAndBehavior

    Public Class A2TextboxUpdateBehavior
        Inherits Behavior(Of TextBox)

        Protected Overrides Sub OnAttached()
            MyBase.OnAttached()
            AddHandler AssociatedObject.TextChanged, AddressOf OnValueChanged
        End Sub

        Protected Overrides Sub OnDetaching()
            MyBase.OnDetaching()
            RemoveHandler AssociatedObject.TextChanged, AddressOf OnValueChanged
        End Sub

        Private Sub OnValueChanged(sender As Object, e As RoutedEventArgs)
            AssociatedObject.GetBindingExpression(TextBox.TextProperty).UpdateSource()
        End Sub

    End Class

End Namespace