Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class PortafolioPPUtilidadTraderView
    Inherits UserControl
    Dim objVMPortafolioPP As PortafolioPPUtilidadTraderViewModel

    Public Sub New()
        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            Me.DataContext = New PortafolioPPUtilidadTraderViewModel
InitializeComponent()

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar los componentes del formulario", Me.ToString(), "PortafolioPPView.New", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    Private Sub PortafolioPPUtilidadTraderView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            objVMPortafolioPP = CType(Me.DataContext, PortafolioPPUtilidadTraderViewModel)

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario", Me.Name, "", "PortafolioPPUtilidadTraderView_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If Not IsNothing(objVMPortafolioPP) Then
                    Select Case pstrClaseControl.ToLower
                        Case "IdEmisor".ToLower
                            objVMPortafolioPP.intEmisor = CType(pobjItem.IdItem, Integer)
                            objVMPortafolioPP.strEmisor = pobjItem.Nombre
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

    Private Sub BuscadorEspecieListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(pobjEspecie) Then
                If Not IsNothing(objVMPortafolioPP) Then
                    If pstrClaseControl.ToLower = "Nemotecnico".ToLower Then
                        objVMPortafolioPP.strNemotecnico = pobjEspecie.Nemotecnico
                        objVMPortafolioPP.strEspecie = pobjEspecie.Especie
                    End If
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el nemotécnico seleccionado", Me.Name, "BuscadorEspecieListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub VerDetallesOperacion(sender As Object, e As RoutedEventArgs)
        Try
            Dim objRegistro = CType(sender, Button)
            Dim dtmFechaEmision As Nullable(Of DateTime)
            Dim dtmFechaVencimiento As Nullable(Of DateTime)
            Dim strModalidad As String = String.Empty
            Dim strIndicador As String = String.Empty
            Dim dblTasaOpuntos As Nullable(Of Double)
            Dim Especie As String = String.Empty
            If Not IsNothing(objVMPortafolioPP) Then
                If Not IsNothing(objVMPortafolioPP.ListaPortafolio) Then
                    dtmFechaEmision = objVMPortafolioPP.ListaPortafolio.Where(Function(i) i.Id = objRegistro.Tag).FirstOrDefault.Emision
                    dtmFechaVencimiento = objVMPortafolioPP.ListaPortafolio.Where(Function(i) i.Id = objRegistro.Tag).FirstOrDefault.Vencimiento
                    strModalidad = objVMPortafolioPP.ListaPortafolio.Where(Function(i) i.Id = objRegistro.Tag).FirstOrDefault.strValorModalidad
                    strIndicador = objVMPortafolioPP.ListaPortafolio.Where(Function(i) i.Id = objRegistro.Tag).FirstOrDefault.strValorIndicador
                    dblTasaOpuntos = objVMPortafolioPP.ListaPortafolio.Where(Function(i) i.Id = objRegistro.Tag).FirstOrDefault.TasaNominal
                    Especie = objVMPortafolioPP.ListaPortafolio.Where(Function(i) i.Id = objRegistro.Tag).FirstOrDefault.Nemo
                End If
            End If
            Dim objOperacionesView As New PortafolioPPOperacionesView(Me.objVMPortafolioPP.strReceptor,
                                                                      Me.objVMPortafolioPP.intEmisor,
                                                                      Me.objVMPortafolioPP.intMesa,
                                                                      Especie,
                                                                      Me.objVMPortafolioPP.logIncluirFechas,
                                                                      Me.objVMPortafolioPP.dtmFechaInicial,
                                                                      Me.objVMPortafolioPP.dtmFechaFinal,
                                                                      Me.objVMPortafolioPP.strTipoProducto,
                                                                      dtmFechaEmision, dtmFechaVencimiento, strModalidad, strIndicador, dblTasaOpuntos)
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
        objVMPortafolioPP.ConsultarRentabilidadPorTrader()
    End Sub

    Private Sub btnLimpiar_Click(sender As Object, e As RoutedEventArgs)
        objVMPortafolioPP.LimpiarForma()
    End Sub
End Class


