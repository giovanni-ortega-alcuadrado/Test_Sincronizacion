Public Class EjecutarScriptGridPantalla_DisenoView
    Inherits Window

    Private mobjVM As EjecutarScriptGridPantallaViewModel

    Public Sub New(ByVal pobjViewModel As EjecutarScriptGridPantallaViewModel)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.DataContext = pobjViewModel
        mobjVM = pobjViewModel
    End Sub

    Private Sub btnGuardarNuevoDiseno_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.Nuevo_Actualizacion_Diseno(True)
    End Sub

    Private Sub btnActualizarDiseno_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.Nuevo_Actualizacion_Diseno(False)
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As RoutedEventArgs)
        Me.DialogResult = False
    End Sub

    Private Sub seleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            ' Seleccionar el texto del control en el cual el usuario se ubicó
            MyBase.OnGotFocus(e)

            If TypeOf sender Is TextBox Then
                CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
            ElseIf TypeOf sender Is Telerik.Windows.Controls.RadNumericUpDown Then
                CType(sender, Telerik.Windows.Controls.RadNumericUpDown).Select(0, CType(sender, Telerik.Windows.Controls.RadNumericUpDown).Value.ToString.Length + 10)
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
