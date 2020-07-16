Imports Telerik.Windows.Controls
Imports System.Windows.Data

'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: PrefijosFacturasView.xaml.vb
'Generado el : 03/04/2011 15:46:43
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class PrefijosFacturasView
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New PrefijosFacturasViewModel
InitializeComponent()
    End Sub

    Private Sub PrefijosFacturas_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
    End Sub

    Private Sub CancelarEditarRegistro_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        df.CancelEdit()
        If Not IsNothing(df.ValidationSummary) Then
            df.ValidationSummary.DataContext = Nothing
        End If
    End Sub

    Private Sub cm_EventoConfirmarGrabacion(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoConfirmarGrabacion
        df.ValidateItem()
        If Not IsNothing(df.ValidationSummary) Then
            If df.ValidationSummary.HasErrors Then
                df.CancelEdit()
            Else
                df.CommitEdit()
            End If
        End If
    End Sub
    

    'Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    '    df.ValidateItem()
    'End Sub


    Public Class prueba
        Implements IValueConverter

        ' This converts the DateTime object to the string to display. 
        Public Function Convert(ByVal value As Object, ByVal targetType As Type, _
    ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object _
            Implements IValueConverter.Convert

            'Dim format As String = If((parameter Is Nothing), "###", parameter.ToString())
            'Return If((value Is Nothing), [String].Empty, Decimal.Parse(value.ToString()).ToString(format))

            Dim format As String = If((parameter Is Nothing), "###", parameter.ToString())
            Return If((value Is Nothing), [String].Empty, Integer.Parse(value.ToString()).ToString(format))
        End Function

        'Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object
        '    Dim format As String = If((parameter Is Nothing), "#.##", parameter.ToString())
        '    Return If((value Is Nothing), [String].Empty, Decimal.Parse(value.ToString()).ToString(format))
        'End Function



        ' No need to implement converting back on a one-way binding.
        Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, _
            ByVal parameter As Object, _
            ByVal culture As System.Globalization.CultureInfo) As Object _
            Implements IValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function
    End Class






End Class


