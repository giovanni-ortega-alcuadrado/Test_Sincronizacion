Imports System.Windows.Data

Public Class clsLiderConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value Is Nothing Then
            Return ""
        ElseIf CBool(value) Then
            Return "Líder"
        Else
            Return ""
        End If
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        If String.IsNullOrEmpty(value.ToString) Then
            Return False
        Else
            Return True
        End If
    End Function
End Class
