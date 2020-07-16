Imports System.Windows.Data
Imports A2.OYD.OYDServer.RIA.Web

Public Class clsEspecieConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim Especie = CType(value, OYDUtilidades.BuscadorEspecies)
        If Not IsNothing(Especie) Then
            Return Especie.ISIN
        Else
            Return ""
        End If
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return ""
    End Function
End Class

Public Class clsNemotecnicoConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim Especie = CType(value, EspeciesAgrupadas)
        If Not IsNothing(Especie) Then
            Return Especie.Nemotecnico '& " (" & Especie.Especie & ")"
        Else
            Return ""
        End If
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return ""
    End Function
End Class