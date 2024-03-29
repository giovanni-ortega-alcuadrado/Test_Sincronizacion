﻿Imports Telerik.Windows.Controls
Imports System.Windows.Data
Imports A2.OYD.OYDServer.RIA.Web
Imports System.Globalization

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

Public Class LongConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim objResultado As Long

        If IsNothing(value) Then
            objResultado = 0
        ElseIf value.trim().Equals(String.Empty) Then
            objResultado = 0
        Else
            objResultado = CType(value, Long)
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
        If value IsNot Nothing AndAlso Not value.ToString().Trim().Equals(String.Empty) Then
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

Public Class GridRowColorConverter
    Implements IValueConverter
    Public Function Convert(value As Object, targetType As System.Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Try
            If TypeOf (value) Is System.String Then
                'Return New SolidColorBrush(colorFromHex("#FFD1DCE8"))
                Return Nothing
            Else
                Dim item = CType(value, OyDPLUSOrdenesBolsa.OrdenSeteador)
                'Select Case item.strEstado
                '    Case "A"
                '        Return New SolidColorBrush(Color.FromArgb(100, 149, 255, 146)) 'verde
                '    Case "C"
                '        Return New SolidColorBrush(Colors.LightGray)
                '    Case "M"
                '        Return New SolidColorBrush(Colors.Yellow)
                '    Case "P"
                '        Return New SolidColorBrush(Colors.Red)
                '    Case Else
                '        Return parameter
                'End Select
                Select Case item.lngID Mod 4
                    Case 0
                        Return New SolidColorBrush(Color.FromArgb(100, 149, 255, 146)) 'verde
                    Case 1
                        Return New SolidColorBrush(Colors.LightGray)
                    Case 2
                        Return New SolidColorBrush(Colors.Yellow)
                    Case 3
                        Return New SolidColorBrush(Colors.Red)
                    Case Else
                        Return parameter
                End Select
            End If
        Catch
            Return New SolidColorBrush(colorFromHex("#FFD1DCE8"))
        End Try
    End Function

    Public Function ConvertBack(value As Object, targetType As System.Type, parameter As Object, culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return Nothing
    End Function
    Private Function colorFromHex(pstrHex As String) As Color
        Try
            pstrHex = pstrHex.Replace("#", String.Empty)
            Dim r, g, b As Byte
            'r = Byte.Parse(UInt32.Parse(pstrHex.Substring(0, 2), 16))
            'g = Byte.Parse(UInt32.Parse(pstrHex.Substring(2, 2), 16))
            'b = Byte.Parse(UInt32.Parse(pstrHex.Substring(4, 2), 16))

            r = Byte.Parse(pstrHex.Substring(0, 2), NumberStyles.HexNumber)
            g = Byte.Parse(pstrHex.Substring(2, 2), NumberStyles.HexNumber)
            b = Byte.Parse(pstrHex.Substring(4, 2), NumberStyles.HexNumber)
            Return Color.FromArgb(255, r, g, b)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al tratar de asignar color desde hexadecimal", Program.TituloSistema, "colorFromHex", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
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

Public Class DateTimeHoraFinDiaConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Dim objResultado As DateTime

        If IsNothing(value) Then
            objResultado = Now
        Else
            objResultado = CType(value, DateTime)
        End If

        Return objResultado
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Dim objResultado As DateTime

        If IsNothing(value) Then
            objResultado = Now
        Else
            If CType(value, DateTime).Hour = 0 Then
                objResultado = DateAdd(DateInterval.Minute, -1, DateAdd(DateInterval.Day, 1, CType(value, DateTime)))
            Else
                objResultado = CType(value, DateTime)
            End If
        End If

        Return objResultado
    End Function

End Class

Public Class NumericBoxConverter
    Implements IValueConverter
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If value Is Nothing Then
            Return Double.NaN
        End If
        Return value
    End Function
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        If Double.IsNaN(value) Then
            Return Nothing
        Else
            Return value
        End If
    End Function
End Class

Public Class BooleanInvertidoConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert

        Dim objResultado As Boolean = True

        If Not targetType.Equals(objResultado.GetType()) Then
            Throw New ArgumentOutOfRangeException("targetType", "BooleanInvertidoConverter can only convert to Boolean")
        End If

        If IsNothing(value) Then
            objResultado = True
        Else
            objResultado = Not CType(value, Boolean)
        End If

        Return objResultado

    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Dim objResultado As Boolean = True
        If IsNothing(value) Then
            objResultado = True
        Else
            objResultado = Not CType(value, Boolean)
        End If

        Return objResultado
    End Function
End Class

Public Class TextoVacioVisibilityConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert

        Dim objResultado As Visibility = Visibility.Collapsed

        If Not targetType.Equals(objResultado.GetType()) Then
            Throw New ArgumentOutOfRangeException("targetType", "TextoVacioVisibilityConverter can only convert to Boolean")
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
