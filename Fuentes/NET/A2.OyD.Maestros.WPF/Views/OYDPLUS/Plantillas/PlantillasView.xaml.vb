Imports Telerik.Windows.Controls
Imports C1.Silverlight
Imports C1.Silverlight.RichTextBox
Imports C1.WPF.RichTextBox

Partial Public Class PlantillasView
    Inherits UserControl

    Public Sub New()
        'Carga los Estilos de la aplicación de OYDPLUS
        'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

        Me.DataContext = New PlantillasViewModel
        InitializeComponent()
    End Sub

    Private Sub Plantillas_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
        cm.GridViewRegistros = datapager1
    End Sub

    ''' <summary>
    ''' Habilita el menú contextual
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lstMetaPalabras_MouseRightButtonDown(sender As Object, e As MouseButtonEventArgs)
        e.Handled = True
    End Sub

    Private Sub lstMetaPalabras_MouseRightButtonUp(sender As Object, e As MouseButtonEventArgs)
        Try
            'obtiene la palabra clave
            If Not IsNothing(Me.FindName("lstMetaPalabras")) Then
                If Not IsNothing(CType(CType(Me.FindName("lstMetaPalabras"), ListBox).SelectedItem, A2.OyD.OYDServer.RIA.Web.tblMetapalabras)) Then
                    Dim strMetapalabra As String = "[[" + CType(CType(Me.FindName("lstMetaPalabras"), ListBox).SelectedItem, A2.OyD.OYDServer.RIA.Web.tblMetapalabras).strPalabraClave + "]]"

                    'ubica el tab seleccionado
                    Dim intTabSelected As Integer = CType(Me.FindName("tabVistaPlantilla"), System.Windows.Controls.TabControl).SelectedIndex

                    'asigna la metapalabra al texto seleccionado
                    If intTabSelected = 0 Then
                        CType(Me.FindName("richTB"), C1RichTextBox).SelectedText = strMetapalabra
                    Else
                        CType(Me.FindName("htmlRich"), C1RichTextBox).SelectedText = strMetapalabra
                    End If
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar la metapalabra.", "PlantillasView", "lstMetaPalabras_MouseRightButtonUp", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
End Class
