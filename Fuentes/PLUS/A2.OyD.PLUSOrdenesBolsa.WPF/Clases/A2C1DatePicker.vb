Imports Telerik.Windows.Controls
Imports System.Windows.Controls.Primitives
Imports System.Windows.Data

Public Class A2TelerikDatePicker
    Inherits Telerik.Windows.Controls.RadTimePicker

    Protected Overrides Sub OnMouseWheel(e As MouseWheelEventArgs)
        MyBase.OnMouseWheel(e)
        e.Handled = False
    End Sub

End Class