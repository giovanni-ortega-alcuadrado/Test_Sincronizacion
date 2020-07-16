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


Public Class clsInVisibilityConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim Visibilidad As Boolean = CType(value, Boolean)
        Return IIf(Visibilidad, Visibility.Collapsed, Visibility.Visible)
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Dim Visibilidad As Visibility = CType(value, Visibility)
        Return (Visibilidad = Visibility.Visible)
    End Function
End Class

Public Class clsInBooleanConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim logValor As Boolean = CType(value, Boolean)
        Return IIf(logValor, False, True)
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Dim logValor As Boolean = CType(value, Boolean)
        Return (logValor = True)
    End Function
End Class

Public Class TextoVacioVisibilityConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert

        Dim objResultado As Visibility = Visibility.Collapsed

        If Not targetType.Equals(objResultado.GetType()) Then
            Throw New ArgumentOutOfRangeException("targetType", "BooleanInvertidoConverter can only convert to Boolean")
        End If

        If IsNothing(value) Then
            objResultado = Visibility.Collapsed
        Else
            If String.IsNullOrEmpty(value) Then
                objResultado = Visibility.Collapsed
            Else
                objResultado = Visibility.Visible
            End If
        End If

        Return objResultado

    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Dim objResultado As Visibility = Visibility.Collapsed
        If IsNothing(value) Then
            objResultado = Visibility.Collapsed
        Else
            If String.IsNullOrEmpty(value) Then
                objResultado = Visibility.Collapsed
            Else
                objResultado = Visibility.Visible
            End If
        End If

        Return objResultado
    End Function
End Class
