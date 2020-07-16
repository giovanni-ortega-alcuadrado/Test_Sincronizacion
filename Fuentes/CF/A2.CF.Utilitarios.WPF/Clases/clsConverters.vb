Imports Telerik.Windows.Controls
Imports System.Windows.Data

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

Public Class BooleanConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim objResultado As Boolean = True

        If IsNothing(value) Then
            objResultado = False
        Else
            If CType(value, Boolean) Then
                objResultado = True
            Else
                objResultado = False
            End If
        End If

        Return objResultado
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Dim objResultado As Boolean = True

        If IsNothing(value) Then
            objResultado = False
        Else
            objResultado = CType(value, Boolean)
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
        If value IsNot Nothing Then
            If Not String.IsNullOrEmpty(value.ToString) Then
                returnValue = CInt(value)
            End If
        End If
        Return returnValue
    End Function
End Class