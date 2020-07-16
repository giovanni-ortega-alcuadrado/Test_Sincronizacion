Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web

Partial Public Class Libranzas_CargaMasivaCamposView
    Inherits UserControl
    Dim objVMLibranzas As CargaMasivaLibranzaCFViewModel
    Dim logDigitoValor As Boolean = False
    Dim logCambioValor As Boolean = False
    Private _cargaMasivaLibranzaCFViewModel As CargaMasivaLibranzaCFViewModel


    Sub New(pobjVMLibranzas As CargaMasivaLibranzaCFViewModel)
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            objVMLibranzas = pobjVMLibranzas

            If Me.Resources.Contains("VMLibranzas") Then
                Me.Resources.Remove("VMLibranzas")
            End If

            Me.Resources.Add("VMLibranzas", pobjVMLibranzas)
            InitializeComponent()

            'Me.stackDistribucionComisiones.MaxWidth = Application.Current.MainWindow.ActualWidth * 0.95
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el control.", Me.Name, "FormaOperacionesOtrosNegociosView", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        'Me.stackDistribucionComisiones.MaxWidth = Application.Current.MainWindow.ActualWidth * 0.95
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(objVMLibranzas) Then
                If Not IsNothing(pobjItem) Then
                    If pstrClaseControl.ToUpper = "EMISOR" Then
                        objVMLibranzas.Libranzas_ImportacionImportadasSelected.intIDEmisor = CType(pobjItem.IdItem, Integer?)
                        objVMLibranzas.Libranzas_ImportacionImportadasSelected.strCodTipoIdentificacionEmisor = pobjItem.InfoAdicional01
                        objVMLibranzas.Libranzas_ImportacionImportadasSelected.strNumeroDocumentoEmisor = pobjItem.CodItem
                        objVMLibranzas.Libranzas_ImportacionImportadasSelected.strNombreEmisor = pobjItem.Nombre
                    ElseIf pstrClaseControl.ToUpper = "PAGADOR" Then
                        objVMLibranzas.Libranzas_ImportacionImportadasSelected.intIDPagador = CType(pobjItem.IdItem, Integer?)
                        objVMLibranzas.Libranzas_ImportacionImportadasSelected.strCodTipoIdentificacionPagador = pobjItem.InfoAdicional01
                        objVMLibranzas.Libranzas_ImportacionImportadasSelected.strNumeroDocumentoPagador = pobjItem.CodItem
                        objVMLibranzas.Libranzas_ImportacionImportadasSelected.strNombrePagador = pobjItem.Nombre
                    ElseIf pstrClaseControl.ToUpper = "CUSTODIO" Then
                        objVMLibranzas.Libranzas_ImportacionImportadasSelected.intIDCustodio = CType(pobjItem.IdItem, Integer?)
                        objVMLibranzas.Libranzas_ImportacionImportadasSelected.strCodTipoIdentificacionCustodio = pobjItem.InfoAdicional01
                        objVMLibranzas.Libranzas_ImportacionImportadasSelected.strNumeroDocumentoCustodio = pobjItem.CodItem
                        objVMLibranzas.Libranzas_ImportacionImportadasSelected.strNombreCustodio = pobjItem.Nombre
                    End If
                Else
                    If pstrClaseControl.ToUpper = "EMISOR" Then
                        ctlBuscadorEmisor.Focus()
                    ElseIf pstrClaseControl.ToUpper = "PAGADOR" Then
                        ctlBuscadorPagador.Focus()
                    ElseIf pstrClaseControl.ToUpper = "CUSTODIO" Then
                        ctlBuscadorCustodio.Focus()
                    End If
                End If
            End If

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "BuscadorGenericoListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorClienteListaButon_finalizoBusqueda(pstrClaseControl As String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(objVMLibranzas) Then
                If Not IsNothing(pobjComitente) Then
                    objVMLibranzas.ComitenteSeleccionado = pobjComitente
                Else
                    ctlBuscadorClientes.Focus()
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCliente_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(objVMLibranzas) Then
                Me.objVMLibranzas.ComitenteSeleccionado = Nothing
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarEmisor_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMLibranzas) Then
                objVMLibranzas.Libranzas_ImportacionImportadasSelected.intIDEmisor = Nothing
                objVMLibranzas.Libranzas_ImportacionImportadasSelected.strCodTipoIdentificacionEmisor = String.Empty
                objVMLibranzas.Libranzas_ImportacionImportadasSelected.strNumeroDocumentoEmisor = String.Empty
                objVMLibranzas.Libranzas_ImportacionImportadasSelected.strNombreEmisor = String.Empty
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarEmisor_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCustodio_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMLibranzas) Then
                objVMLibranzas.Libranzas_ImportacionImportadasSelected.intIDCustodio = Nothing
                objVMLibranzas.Libranzas_ImportacionImportadasSelected.strCodTipoIdentificacionCustodio = String.Empty
                objVMLibranzas.Libranzas_ImportacionImportadasSelected.strNumeroDocumentoCustodio = String.Empty
                objVMLibranzas.Libranzas_ImportacionImportadasSelected.strNombreCustodio = String.Empty
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarCustodio_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarPagador_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVMLibranzas) Then
                objVMLibranzas.Libranzas_ImportacionImportadasSelected.intIDPagador = Nothing
                objVMLibranzas.Libranzas_ImportacionImportadasSelected.strCodTipoIdentificacionPagador = String.Empty
                objVMLibranzas.Libranzas_ImportacionImportadasSelected.strNumeroDocumentoPagador = String.Empty
                objVMLibranzas.Libranzas_ImportacionImportadasSelected.strNombrePagador = String.Empty
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarPagador_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub SeleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        MyBase.OnGotFocus(e)
        If TypeOf sender Is TextBox Then
            CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
            'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
        End If
    End Sub

    Public Sub BuscarControlValidacion(ByVal pstrOpcion As String)
        Try
            If Not IsNothing(Me) Then
                If TypeOf Me.FindName(pstrOpcion) Is TabItem Then
                    CType(Me.FindName(pstrOpcion), TabItem).IsSelected = True
                ElseIf TypeOf Me.FindName(pstrOpcion) Is TextBox Then
                    CType(Me.FindName(pstrOpcion), TextBox).Focus()
                ElseIf TypeOf Me.FindName(pstrOpcion) Is ComboBox Then
                    CType(Me.FindName(pstrOpcion), ComboBox).Focus()
                
                    
                ElseIf TypeOf Me.FindName(pstrOpcion) Is A2Utilidades.A2NumericBox Then
                    CType(Me.FindName(pstrOpcion), A2Utilidades.A2NumericBox).Focus()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al buscar el control dentro del registro.", Me.ToString, "BuscarControlValidacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtComitente_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMLibranzas.DiccionarioEdicionCampos("IDComitente") Then
                ctlBuscadorClientes.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtComitente_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtPagador_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMLibranzas.DiccionarioEdicionCampos("IDPagador") Then
                ctlBuscadorPagador.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtPagador_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtEmisor_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMLibranzas.DiccionarioEdicionCampos("IDEmisor") Then
                ctlBuscadorEmisor.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtEmisor_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtCustodio_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If objVMLibranzas.DiccionarioEdicionCampos("IDCustodio") Then
                ctlBuscadorCustodio.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtCustodio_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


End Class
