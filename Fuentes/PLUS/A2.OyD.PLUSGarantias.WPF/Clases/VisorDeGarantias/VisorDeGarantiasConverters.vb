Imports A2.OyD.OYDServer.RIA.Web

''' <summary>
''' Convertidor para el tamano de la barra del estado de la liquidación para el cumplimiento interno
''' </summary>
''' <remarks></remarks>
Public Class TamanoInternoLiquidacionConverter
    Implements Data.IValueConverter

    ''' <summary>
    ''' Obtener el valor
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="targetType"></param>
    ''' <param name="parameter"></param>
    ''' <param name="culture"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Return VisorDeGarantiasViewModel.ObtenerTamanoEstadoInterno(value)
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return value
    End Function
End Class



''' <summary>
''' Convertidor para el tamano de la barra del estado de la liquidación para el cumplimiento legal
''' </summary>
''' <remarks></remarks>
Public Class TamanoLegalLiquidacionConverter
    Implements Data.IValueConverter

    ''' <summary>
    ''' Obtener el valor
    ''' </summary>
    ''' <param name="value"></param>
    ''' <param name="targetType"></param>
    ''' <param name="parameter"></param>
    ''' <param name="culture"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Return VisorDeGarantiasViewModel.ObtenerTamanoEstadoLegal(value)
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return value
    End Function
End Class


''' <summary>
''' Convertidor para obtener el color del estado interno
''' </summary>
''' <remarks></remarks>
Public Class ColorEstadoInternoConverter
    Implements Data.IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Return New SolidColorBrush(VisorDeGarantiasViewModel.ObtenerColorEstado(VisorDeGarantiasViewModel.ObtenerEstadoInterno(value)))
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return value
    End Function

End Class

''' <summary>
''' Convertidor para obtener el color del estado Legal
''' </summary>
''' <remarks></remarks>
Public Class ColorEstadoLegalConverter
    Implements Data.IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Return New SolidColorBrush(VisorDeGarantiasViewModel.ObtenerColorEstado(VisorDeGarantiasViewModel.ObtenerEstadoLegal(value)))
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return value
    End Function

End Class


''' <summary>
''' Convertidor para obtener el valor del estado interno
''' </summary>
''' <remarks></remarks>
Public Class ValorEstadoInternoConverter
    Implements Data.IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Return VisorDeGarantiasViewModel.ObtenerSaldoInternoLiquidacion(value)
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return value
    End Function
End Class

''' <summary>
''' Convertidor para obtener el valor del estado legal
''' </summary>
''' <remarks></remarks>
Public Class ValorEstadoLegalConverter
    Implements Data.IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Return VisorDeGarantiasViewModel.ObtenerSaldoLegalLiquidacion(value)
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return value
    End Function
End Class


' ''' <summary>
' ''' Convertidor para obtener el valor del estado legal
' ''' </summary>
' ''' <remarks></remarks>
'Public Class ValorCoberturaTotalConverter
'    Implements Data.IValueConverter

'    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
'        Return VisorDeGarantiasViewModel.ObtenerCoberturaTotalLiquidacion(value)
'    End Function

'    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
'        Return value
'    End Function
'End Class

''' <summary>
''' convertidor para ocultar el texto en las filas grupo, para que no se sobreponga con las otras columnas
''' </summary>
''' <remarks></remarks>
Public Class TextoVacioConverter
    Implements Data.IValueConverter

    Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
        Return String.Empty
    End Function

    Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
        Return value
    End Function
End Class
