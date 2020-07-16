Imports System.Windows.Data
Imports A2.OyD.OYDServer.RIA.WEB

Public Class clsClienteConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        'Dim Cliente = CType(value, OYDUtilidades.BuscadorClientes)
        'If Not IsNothing(Cliente) Then
        '    Return Cliente.IdComitente '& " - " & Cliente.strNombre
        'Else
        '    Return ""
        'End If
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return ""
    End Function
End Class
