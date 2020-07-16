Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: ConfigLEOView.xaml.vb
'Generado el : 11/21/2011 16:36:48
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class  ConfigLEOView
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New ConfigLEOViewModel
InitializeComponent()
    End Sub

    Private Sub ConfigLEO_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        
        'cm.DF = df
		CType(Me.DataContext,ConfigLEOViewModel).NombreView = Me.ToString
    End Sub

    Private Sub CancelarEditarRegistro_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        'df.CancelEdit()
                ''If Not IsNothing(df.ValidationSummary) Then
        '    df.ValidationSummary.DataContext = Nothing
        'End If
    End Sub

    'Private Sub cm_EventoConfirmarGrabacion(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoConfirmarGrabacion
    '    'df.ValidateItem()
    '    'If df.ValidationSummary.HasErrors Then
    '    '    df.CancelEdit()
    '    'Else
    '    '    df.CommitEdit()
    '    'End If
    'End Sub

    'Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    '    df.ValidateItem()
    'End Sub

    Private Sub Aceptar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, ConfigLEOViewModel).Actualizar()
    End Sub

    Private Sub CheckBox_GotFocus(sender As Object, e As RoutedEventArgs)
        Dim objRegistroSeleccionado As ConfigLeoLista = CType(CType(sender, CheckBox).Tag, ConfigLeoLista)
        If Not IsNothing(objRegistroSeleccionado) Then
            If IsNothing(CType(Me.DataContext, ConfigLEOViewModel).disponibles) Then
                CType(Me.DataContext, ConfigLEOViewModel).disponibles = objRegistroSeleccionado
            Else
                If CType(Me.DataContext, ConfigLEOViewModel).disponibles.Descripcion <> objRegistroSeleccionado.Descripcion Then
                    CType(Me.DataContext, ConfigLEOViewModel).disponibles = objRegistroSeleccionado
                End If
            End If
        End If
    End Sub
End Class


