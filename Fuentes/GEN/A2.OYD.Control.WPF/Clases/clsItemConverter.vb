Imports System.Windows.Data
Imports A2.OyD.OYDServer.RIA.Web

Public Class clsItemConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim Item = CType(value, OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(Item) Then
            Return String.Format("{0} - {1}", Item.IdItem, Item.Nombre)
        Else
            Return ""
        End If
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return ""
    End Function
End Class