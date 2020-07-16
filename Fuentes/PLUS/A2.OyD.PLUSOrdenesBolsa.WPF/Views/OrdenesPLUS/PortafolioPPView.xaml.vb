Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class PortafolioPPView
    Inherits UserControl
    Dim objVMPortafolioPP As PortafolioPPViewModel

    Public Sub New()
        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            Me.DataContext = New PortafolioPPViewModel
InitializeComponent()

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar los componentes del formulario", Me.ToString(), "PortafolioPPView.New", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    Private Sub PortafolioPPView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            objVMPortafolioPP = CType(Me.DataContext, PortafolioPPViewModel)

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario", Me.Name, "", "PortafolioPPView_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If Not IsNothing(objVMPortafolioPP) Then
                    Select Case pstrClaseControl.ToLower
                        Case "IdReceptor".ToLower
                            objVMPortafolioPP.strReceptor = pobjItem.IdItem
                            objVMPortafolioPP.strNombreReceptor = pobjItem.Nombre
                    End Select
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el registro seleccionado", Me.Name, "BuscadorGenerico_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctrlCliente_comitenteAsignado(pstrIdComitente As System.String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(objVMPortafolioPP) Then
                Me.objVMPortafolioPP.strCliente = pobjComitente.IdComitente
                Me.objVMPortafolioPP.strNombreCliente = pobjComitente.Nombre
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctrlCliente_LostFocus(objBuscadorCliente As A2ComunesControl.BuscadorCliente, e As System.Windows.RoutedEventArgs) Handles ctrlCliente.LostFocus
        Try
            If IsNothing(objBuscadorCliente.ComitenteActivo) Then
                Me.objVMPortafolioPP.LimpiarCliente()
            End If

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al limpiar el cliente", Me.Name, "btnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCliente_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(objVMPortafolioPP) Then
                Me.objVMPortafolioPP.BorrarCliente = True
                Me.objVMPortafolioPP.BorrarCliente = False
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al limpiar el cliente", Me.Name, "btnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub VerDetallesOperacionDiaActual(sender As Object, e As RoutedEventArgs)
        Try
            Dim objRegistro = CType(sender, Button)
            Dim dtmFechaEmision As Nullable(Of DateTime)
            Dim dtmFechaVencimiento As Nullable(Of DateTime)
            Dim strModalidad As String = String.Empty
            Dim strIndicador As String = String.Empty
            Dim dblTasaOpuntos As Nullable(Of Double) = Nothing
            Dim Especie As String = String.Empty
            If Not IsNothing(objVMPortafolioPP) Then
                If Not IsNothing(objVMPortafolioPP.ListaPortafolioDiaActual) Then

                    dtmFechaEmision = objVMPortafolioPP.ListaPortafolioDiaActual.Where(Function(i) i.Id = objRegistro.Tag).FirstOrDefault.Emision
                    dtmFechaVencimiento = objVMPortafolioPP.ListaPortafolioDiaActual.Where(Function(i) i.Id = objRegistro.Tag).FirstOrDefault.Vencimiento
                    strModalidad = objVMPortafolioPP.ListaPortafolioDiaActual.Where(Function(i) i.Id = objRegistro.Tag).FirstOrDefault.strValorModalidad
                    strIndicador = objVMPortafolioPP.ListaPortafolioDiaActual.Where(Function(i) i.Id = objRegistro.Tag).FirstOrDefault.strValorIndicador
                    dblTasaOpuntos = objVMPortafolioPP.ListaPortafolioDiaActual.Where(Function(i) i.Id = objRegistro.Tag).FirstOrDefault.TasaNominal
                    Especie = objVMPortafolioPP.ListaPortafolioDiaActual.Where(Function(i) i.Id = objRegistro.Tag).FirstOrDefault.Nemo
                End If
            End If
            Dim objOperacionesView As New PortafolioPPOperacionesView(Me.objVMPortafolioPP.strReceptor,
                                                                      Me.objVMPortafolioPP.strTipoProducto,
                                                                      Me.objVMPortafolioPP.logTodosLosClientes,
                                                                      Me.objVMPortafolioPP.strCliente,
                                                                      True,
                                                                      Me.objVMPortafolioPP.dtmFecha,
                                                                      Especie,
                                                                      dtmFechaEmision,
                                                                      dtmFechaVencimiento,
                                                                      strModalidad,
                                                                      strIndicador,
                                                                      dblTasaOpuntos)
            Program.Modal_OwnerMainWindowsPrincipal(objOperacionesView)
            objOperacionesView.ShowDialog()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al ver los detalles de la operación", Me.Name, "VerDetallesOperacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub VerDetallesOperacionNoDiaActual(sender As Object, e As RoutedEventArgs)
        Try
            Dim objRegistro = CType(sender, Button)
            Dim dtmFechaEmision As Nullable(Of DateTime)
            Dim dtmFechaVencimiento As Nullable(Of DateTime)
            Dim strModalidad As String = String.Empty
            Dim strIndicador As String = String.Empty
            Dim dblTasaOpuntos As Nullable(Of Double) = Nothing
            Dim Especie As String = String.Empty

            If Not IsNothing(objVMPortafolioPP) Then
                If Not IsNothing(objVMPortafolioPP.ListaPortafolioOtrosDias) Then

                    dtmFechaEmision = objVMPortafolioPP.ListaPortafolioOtrosDias.Where(Function(i) i.Id = objRegistro.Tag).FirstOrDefault.Emision
                    dtmFechaVencimiento = objVMPortafolioPP.ListaPortafolioOtrosDias.Where(Function(i) i.Id = objRegistro.Tag).FirstOrDefault.Vencimiento
                    strModalidad = objVMPortafolioPP.ListaPortafolioOtrosDias.Where(Function(i) i.Id = objRegistro.Tag).FirstOrDefault.strValorModalidad
                    strIndicador = objVMPortafolioPP.ListaPortafolioOtrosDias.Where(Function(i) i.Id = objRegistro.Tag).FirstOrDefault.strValorIndicador
                    dblTasaOpuntos = objVMPortafolioPP.ListaPortafolioOtrosDias.Where(Function(i) i.Id = objRegistro.Tag).FirstOrDefault.TasaNominal
                    Especie = objVMPortafolioPP.ListaPortafolioOtrosDias.Where(Function(i) i.Id = objRegistro.Tag).FirstOrDefault.Nemo
                End If
            End If
            Dim objOperacionesView As New PortafolioPPOperacionesView(Me.objVMPortafolioPP.strReceptor,
                                                                    Me.objVMPortafolioPP.strTipoProducto,
                                                                    Me.objVMPortafolioPP.logTodosLosClientes,
                                                                    Me.objVMPortafolioPP.strCliente,
                                                                    False,
                                                                    Me.objVMPortafolioPP.dtmFecha,
                                                                    Especie,
                                                                    dtmFechaEmision,
                                                                    dtmFechaVencimiento,
                                                                    strModalidad,
                                                                    strIndicador,
                                                                    dblTasaOpuntos)

            Program.Modal_OwnerMainWindowsPrincipal(objOperacionesView)
            objOperacionesView.ShowDialog()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al ver los detalles de la operación", Me.Name, "VerDetallesOperacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnExportarExcel_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.objVMPortafolioPP.ExportarInformacion()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al ver los detalles de la operación", Me.Name, "btnExportarExcel_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
Private Sub btnConsultar_Click(sender As Object, e As RoutedEventArgs)
        objVMPortafolioPP.ConsultarPortafolio()
    End Sub

    Private Sub btnLimpiar_Click(sender As Object, e As RoutedEventArgs)
        objVMPortafolioPP.LimpiarForma()
    End Sub
End Class


