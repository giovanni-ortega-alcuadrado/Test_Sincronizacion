Imports System.Windows.Data
Imports A2.OyD.OYDServer.RIA.WEB
Public Class clsPersonaConverter
	Implements IValueConverter

	Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
		Dim Persona = CType(value, CPX_BuscadorPersonas)
		If Not IsNothing(Persona) Then
			Return Persona.strNroDocumento '& " - " & Persona.strNombre
		Else
			Return ""
		End If
	End Function

	Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
		Return ""
	End Function
End Class
