Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: BancosNacionalesView.xaml.vb
'Generado el : 03/07/2011 12:15:57
'Propiedad de Alcuadrado S.A. 2010
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports Microsoft.VisualBasic.CompilerServices
Imports Telerik.Windows.Controls.GridView

Partial Public Class ActualizarLEOView
    Inherits UserControl
    Dim vm As ActualizarLEOViewModel
    Dim logInicializa As Boolean = True

    Public Sub New()
        Try
            Me.DataContext = New ActualizarLEOViewModel
            InitializeComponent()

            AddHandler Me.SizeChanged, AddressOf CambioDePantalla
            Me.Width = Application.Current.MainWindow.ActualWidth * 0.96
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la pantalla.", Me.ToString(), "New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        Me.Width = Application.Current.MainWindow.ActualWidth * 0.96
    End Sub

    Private Sub ActualizarLEOView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If logInicializa Then
            vm = CType(Me.DataContext, ActualizarLEOViewModel)
            CType(Me.DataContext, ActualizarLEOViewModel).NombreView = Me.ToString
            CType(Me.DataContext, ActualizarLEOViewModel).ViewActualizarLeo = Me
            vm.ReiniciaTimer()
            logInicializa = False
        Else
            vm.ReiniciaTimer()
            vm.ReiniciaTimer2()
        End If
    End Sub

    Private Sub ActualizarLEOView_Unloaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Unloaded
        vm.pararTemporizador()
    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl
                Case "receptores"
                    CType(Me.DataContext, ActualizarLEOViewModel).IDReceptor = pobjItem.CodItem
            End Select
        End If
    End Sub

    Private Sub CheckBox_Checked(sender As Object, e As RoutedEventArgs)
        Try
            Dim intIDSeleccionado As Integer = CInt(CType(sender, CheckBox).Tag)

            If Not IsNothing(CType(Me.DataContext, ActualizarLEOViewModel).ListaEstadosLEO) Then
                For Each li In CType(Me.DataContext, ActualizarLEOViewModel).ListaEstadosLEO
                    If li.intID <> intIDSeleccionado Then
                        li.ObjMarcar = False
                    End If
                Next
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar el archivo.", Me.ToString(), "CheckBox_Checked", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub btnLimpiar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, ActualizarLEOViewModel).IDReceptor = Nothing
    End Sub

    Private Sub btnConsultar_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, ActualizarLEOViewModel).ConsultarLeo()
    End Sub

    Private Sub btnCambiarEstado_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, ActualizarLEOViewModel).CambiarEstadoLeo()
    End Sub

    'Private Sub BuscadorGenericoListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
    '    If Not IsNothing(pobjItem) Then
    '        CType(Me.DataContext, BloquearTituloViewModel)._mlogBuscarEspecie = False
    '        CType(Me.DataContext, BloquearTituloViewModel).CamposBusquedaSelected.Nemotecnico = pobjItem.IdItem
    '        CType(Me.DataContext, BloquearTituloViewModel).CamposBusquedaSelected.Especie = pobjItem.Nombre
    '        CType(Me.DataContext, BloquearTituloViewModel)._mlogBuscarEspecie = True
    '    End If
    'End Sub

    'Private Sub txtClienteBusqueda_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
    '    If Not (e.Key > 47 And e.Key < 58) And Not e.Key = 9 And Not (e.Key > 95 And e.Key < 106) Then
    '        e.Handled = True
    '    End If
    'End Sub

    'Private Sub BuscadorGenerico_finalizoBusquedaISINFungible(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
    '    Try
    '        If Not IsNothing(pobjItem) Then
    '            Select Case pstrClaseControl
    '                Case "ISINFUNGIBLE"
    '                    vm._mlogBuscarISINFungible = False
    '                    vm.ISINFungibleSeleccionada(pobjItem)
    '                    vm._mlogBuscarISINFungible = True
    '            End Select
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al el Isin Fungible",
    '                             Me.ToString(), "BuscadorGenerico_finalizoBusquedaISINFungible", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub

    'Private Sub CheckBox_Checked(sender As Object, e As RoutedEventArgs)
    '    Try
    '        If Not IsNothing(vm.ListaCustodiaSeleccionada) Then
    '            vm.AsignarMotivoBloqueo()
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar valores para el motivo de bloqueo",
    '                            Me.ToString(), "CheckBox_Checked", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub

    Private Sub RadGridView_RowLoaded(ByVal sender As Object, ByVal e As RowLoadedEventArgs)
        Try
            Dim objItemSeleccionado As ListadoActualizarLEO = Nothing

            Try
                objItemSeleccionado = CType(e.Row.Item, ListadoActualizarLEO)
            Catch ex1 As Exception

            End Try

            If Not IsNothing(objItemSeleccionado) Then
                If Not IsNothing(CType(Me.DataContext, ActualizarLEOViewModel).diccionariocoloractual) Then
                    If CType(Me.DataContext, ActualizarLEOViewModel).diccionariocoloractual.Where(Function(i) i.Key = objItemSeleccionado.lngIDOrden).Count > 0 Then
                        Dim strColor As String = CType(Me.DataContext, ActualizarLEOViewModel).diccionariocoloractual.Where(Function(i) i.Key = objItemSeleccionado.lngIDOrden).First.Color
                        If strColor = "Rojo" Then
                            e.Row.Background = New BrushConverter().ConvertFromString("#FF0000") ' datarow.BackColor = Color.Transparentel
                        ElseIf strColor = "Yellow" Then
                            e.Row.Background = New BrushConverter().ConvertFromString("#FFFF00")
                        ElseIf strColor = "Orange" Then
                            e.Row.Background = New BrushConverter().ConvertFromString("#FF8000")
                        ElseIf strColor = "Transparente" Then
                            e.Row.Background = New BrushConverter().ConvertFromString("#FFFFFF")
                        End If
                    End If
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

End Class