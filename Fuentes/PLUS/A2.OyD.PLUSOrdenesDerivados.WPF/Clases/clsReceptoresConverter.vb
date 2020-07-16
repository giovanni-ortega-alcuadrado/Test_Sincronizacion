Imports Telerik.Windows.Controls
Imports System.Windows.Data
Imports A2.OyD.OYDServer.RIA.Web

Public Class clsReceptoresConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim Item = CType(value, OyDPLUSOrdenesDerivados.ReceptoresBusqueda)
        Return String.Format("{0} - {1}", Item.CodigoComercial, Item.Nombre)
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return ""
    End Function
End Class