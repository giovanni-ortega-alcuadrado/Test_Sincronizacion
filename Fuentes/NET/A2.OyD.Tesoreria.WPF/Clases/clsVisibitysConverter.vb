Imports Telerik.Windows.Controls
Imports System.Windows.Data

Public Class clsVisibilityConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim Visibilidad As Boolean = CType(value, Boolean)
        Return IIf(Visibilidad, Visibility.Visible, Visibility.Collapsed)
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Dim Visibilidad As Visibility = CType(value, Visibility)
        Return (Visibilidad = Visibility.Visible)
    End Function
End Class
