Imports System.Reflection

Public Class DisplayNameConverter
    Implements IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        If Not IsNothing(value) Then
            Dim objPropInfo As PropertyInfo = value.[GetType]().GetProperty(parameter.ToString())
            If Not IsNothing(objPropInfo) Then
                Dim objAttrib = objPropInfo.CustomAttributes()

                If Not IsNothing(objAttrib) Then
                    If objAttrib.Where(Function(i) i.AttributeType.Name = "DisplayAttribute").Count Then
                        Dim objAttributo As CustomAttributeData = objAttrib.Where(Function(i) i.AttributeType.Name = "DisplayAttribute").First
                        If objAttributo.NamedArguments.Where(Function(i) i.MemberName = "Name").Count > 0 Then
                            Dim strDisplayName As String = objAttributo.NamedArguments.Where(Function(i) i.MemberName = "Name").First.TypedValue.ToString
                            strDisplayName = strDisplayName.Replace("""", "")
                            Return strDisplayName
                        End If
                    End If
                End If
            End If
        End If

        Return String.Empty
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class
