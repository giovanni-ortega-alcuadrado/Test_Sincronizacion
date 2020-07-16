Imports Telerik.Windows.Controls
Imports System.Windows.Data
Imports A2.OyD.OYDServer.RIA.Web


Public Class VisibilityConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert

        Dim objResultado As Visibility = Visibility.Visible

        If Not targetType.Equals(objResultado.GetType()) Then
            Throw New ArgumentOutOfRangeException("targetType", "VisibilityConverter can only convert to Visibility")
        End If

        If IsNothing(value) Then
            objResultado = Visibility.Collapsed
        Else
            If CType(value, Boolean) Then
                objResultado = Visibility.Visible
            Else
                objResultado = Visibility.Collapsed
            End If
        End If

        Return objResultado

    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Dim objResultado As Boolean = True
        If IsNothing(value) Then
            objResultado = False
        Else
            If CType(value, Visibility) = Visibility.Visible Then
                objResultado = True
            Else
                objResultado = False
            End If
        End If

        Return objResultado
    End Function
End Class

Public Class InVisibilityConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert

        Dim objResultado As Visibility = Visibility.Visible

        If Not targetType.Equals(objResultado.GetType()) Then
            Throw New ArgumentOutOfRangeException("targetType", "VisibilityConverter can only convert to Visibility")
        End If

        If IsNothing(value) Then
            objResultado = Visibility.Visible
        Else
            If CType(value, Boolean) Then
                objResultado = Visibility.Collapsed
            Else
                objResultado = Visibility.Visible
            End If
        End If

        Return objResultado

    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Dim objResultado As Boolean = True
        If IsNothing(value) Then
            objResultado = False
        Else
            If CType(value, Visibility) = Visibility.Visible Then
                objResultado = False
            Else
                objResultado = True
            End If
        End If

        Return objResultado
    End Function
End Class

Public Class IntegerConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim objResultado As Integer

        If IsNothing(value) Then
            objResultado = 0
        Else
            objResultado = CType(value, Integer)
        End If

        Return objResultado
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return value.ToString()
    End Function
End Class

Public Class StringConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim strResultado As String = String.Empty
        If Not IsNothing(value) Then
            strResultado = value.ToString()
        End If
        Return strResultado
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Dim returnValue As Integer = 0
        If value IsNot Nothing And value <> String.Empty Then
            returnValue = value
        End If
        Return returnValue
    End Function
End Class

Public Class NumericConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim objResultado As Double
        If Not IsNothing(value) Then
            objResultado = 0
        Else
            objResultado = CType(value, Double)
        End If
        Return objResultado
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Dim returnValue As Integer = 0
        If value <> String.Empty Then
            returnValue = CType(value, Double)
        Else

            returnValue = 0

        End If
        Return returnValue

    End Function

End Class

Public Class BooleanVisibilityConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert

        Dim objResultado As Boolean = True

        If Not targetType.Equals(objResultado.GetType()) Then
            Throw New ArgumentOutOfRangeException("targetType", "BooleanVisibilityConverter can only convert to Boolean")
        End If

        If IsNothing(value) Then
            objResultado = False
        Else
            If CType(value, Visibility) = Visibility.Visible Then
                objResultado = True
            Else
                objResultado = False
            End If
        End If

        Return objResultado

    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Dim objResultado As Visibility = Visibility.Visible
        If IsNothing(value) Then
            objResultado = Visibility.Collapsed
        Else
            If CType(value, Boolean) Then
                objResultado = Visibility.Visible
            Else
                objResultado = Visibility.Collapsed
            End If
        End If

        Return objResultado
    End Function
End Class

Public Class BooleanInVisibilityConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert

        Dim objResultado As Boolean = True

        If Not targetType.Equals(objResultado.GetType()) Then
            Throw New ArgumentOutOfRangeException("targetType", "BooleanVisibilityConverter can only convert to Boolean")
        End If

        If IsNothing(value) Then
            objResultado = False
        Else
            If CType(value, Visibility) = Visibility.Visible Then
                objResultado = False
            Else
                objResultado = True
            End If
        End If

        Return objResultado

    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Dim objResultado As Visibility = Visibility.Visible
        If IsNothing(value) Then
            objResultado = Visibility.Collapsed
        Else
            If CType(value, Boolean) Then
                objResultado = Visibility.Collapsed
            Else
                objResultado = Visibility.Visible
            End If
        End If

        Return objResultado
    End Function
End Class

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

Public Class ColorFilaConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim strColor As String = CStr(value)
        Dim objColor As New Color

        If strColor = "ROJO" Then
            objColor = Program.colorFromHex("#DE1A1A")
        Else
            objColor = Program.colorFromHex("#000000")
        End If

        Return New SolidColorBrush(objColor)
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Throw New NotImplementedException()
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

Public Class InBooleanConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert

        Dim objResultado As Boolean = True

        If Not targetType.Equals(objResultado.GetType()) Then
            Throw New ArgumentOutOfRangeException("targetType", "InBooleanConverter can only convert to Boolean")
        End If

        If IsNothing(value) Then
            objResultado = False
        Else
            If CType(value, Boolean) Then
                objResultado = False
            Else
                objResultado = True
            End If
        End If

        Return objResultado

    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Dim objResultado As Boolean = True
        If IsNothing(value) Then
            objResultado = False
        Else
            If CType(value, Boolean) Then
                objResultado = False
            Else
                objResultado = True
            End If
        End If

        Return objResultado
    End Function
End Class