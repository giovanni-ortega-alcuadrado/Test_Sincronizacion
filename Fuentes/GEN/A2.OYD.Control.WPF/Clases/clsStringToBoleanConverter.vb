Imports System.Windows.Data

'JWSJ
Public Class clsStringToBoleanConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim strResultado As Boolean
        If Not IsNothing(value) Then
            strResultado = System.Convert.ToBoolean(value)
        End If
        Return strResultado
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Dim returnValue As String = Boolean.FalseString
        If value IsNot Nothing Then
            returnValue = value.ToString
        End If
        Return returnValue
    End Function
End Class
