
Imports Microsoft.Win32
Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: BancosView.xaml.vb
'Generado el : 08/08/2011 12:11:50
'Propiedad de Alcuadrado S.A. 2010
Imports A2.OyD.OYDServer.RIA.Web
Imports System.IO
Imports System.Text.RegularExpressions




Partial Public Class BancosView
    Inherits UserControl
    Private WithEvents mobjBuscadorLst As A2ComunesControl.BuscadorGenericoLista

    Public Sub New()
        Me.DataContext = New BancosViewModel
InitializeComponent()

    End Sub

    Private Sub Bancos_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        'cm.DF = df
        CType(Me.DataContext, BancosViewModel).NombreView = Me.ToString
    End Sub

    'Private Sub CancelarEditarRegistro_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
    '    df.CancelEdit()
    ''    If Not IsNothing(df.ValidationSummary) Then
    '        df.ValidationSummary.DataContext = Nothing
    '    End If
    'End Sub

    'Private Sub cm_EventoConfirmarGrabacion(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoConfirmarGrabacion
    '    df.ValidateItem()
    '    If df.ValidationSummary.HasErrors Then
    '        df.CancelEdit()
    '    Else
    '        df.CommitEdit()
    '    End If
    'End Sub


    Private Sub cm_EventoRefrescarCombos(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoRefrescarCombos
        Try
            If Me.Resources.Contains("A2VM") Then
                CType(Me.DataContext, BancosViewModel).IsBusy = True
                CType(Me.Resources("A2VM"), A2UtilsViewModel).EjecutaActualizacionCombos(Me.ToString)
                CType(Me.DataContext, BancosViewModel).IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar los combos.",
                                 Me.ToString(), "cm_EventoRefrescarCombos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl.ToLower
                Case "ciudades"
                    CType(Me.DataContext, BancosViewModel).BancoSelected.IDPoblacion = CType(pobjItem.IdItem, Integer)
                    CType(Me.DataContext, BancosViewModel).CiudadesClaseSelected.Ciudad = pobjItem.Nombre

                    CType(Me.DataContext, BancosViewModel).BancoSelected.IDDepartamento = CType(pobjItem.InfoAdicional01, Integer)
                    CType(Me.DataContext, BancosViewModel).CiudadesClaseSelected.Departamento = pobjItem.CodigoAuxiliar

                    CType(Me.DataContext, BancosViewModel).BancoSelected.IDPais = CType(pobjItem.EtiquetaCodItem, Integer)
                    CType(Me.DataContext, BancosViewModel).CiudadesClaseSelected.Pais = pobjItem.InfoAdicional02
                Case "compania"
                    CType(Me.DataContext, BancosViewModel).BancoSelected.Compania = CType(pobjItem.IdItem, Integer)
                    CType(Me.DataContext, BancosViewModel).BancoSelected.NombreCompania = pobjItem.CodigoAuxiliar
                    CType(Me.DataContext, BancosViewModel).CompaniaBanco = pobjItem.IdItem
                    CType(Me.DataContext, BancosViewModel).BancoSelected.IdMoneda = CType(pobjItem.InfoAdicional03, Integer)
                    CType(Me.DataContext, BancosViewModel).IdMonedaCompanias = CType(pobjItem.InfoAdicional03, Integer)
                Case "bcompania"
                    CType(Me.DataContext, BancosViewModel).cb.CompaniaBanco = CType(pobjItem.IdItem, Integer)
                    CType(Me.DataContext, BancosViewModel).cb.NombreCompania = pobjItem.CodigoAuxiliar

                 'DEMC20170831
                Case "idcuentacontable"
                    CType(Me.DataContext, BancosViewModel).BancoSelected.IdCuentaCtble = CType(pobjItem.IdItem, String) 'DEMC20180510 Se convierte la variable IdItem a string ya que estaba llegando un valor de mayor longitud y por sacaba error.
                    'DEMC20170831

            End Select
        End If
    End Sub
    Private Sub BuscadorC_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl.ToLower
                Case "compania"
                    CType(Me.DataContext, BancosViewModel).cb.CompaniaBanco = CType(pobjItem.IdItem, Integer)
                    CType(Me.DataContext, BancosViewModel).cb.NombreCompania = pobjItem.CodigoAuxiliar
            End Select
        End If
    End Sub

    'Private Sub Button_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
    '    Dim p As New OpenFileDialog
    '    p.Filter = "Reportes (*.rpt)|*.rpt"

    '    If p.ShowDialog Then
    '        'Dim nombre As FileInfo
    '        'nombre = p.File
    '        CType(Me.DataContext, BancosViewModel).BancoSelected.Reporte = p.File.Name 'p.FileName
    '    End If
    'End Sub

    Private Sub C1NumericBox_LostFocus(sender As System.Object, e As System.Windows.RoutedEventArgs)
        If CType(Me.DataContext, BancosViewModel)._mlogNuevo Then
            'Valida si el codigo ya existe.
            For Each Lista In CType(Me.DataContext, BancosViewModel).ListaBancos
                If Lista.ID = CType(Me.DataContext, BancosViewModel).BancoSelected.ID Then
                    A2Utilidades.Mensajes.mostrarMensaje("El código elegido ya ha sido asignado.", "OyD Server", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    CType(Me.DataContext, BancosViewModel).BancoSelected.ID = Nothing
                    Exit Sub
                End If
            Next
        End If
    End Sub

    Private Sub TextBox_TextChanged_1(sender As Object, e As TextChangedEventArgs)
        Try
            If Not IsNothing(sender) Then
                Dim objTextBox As TextBox = CType(sender, TextBox)
                If Not String.IsNullOrEmpty(objTextBox.Text) Then
                    Dim objValidacion = clsExpresiones.ValidarCaracteresEnCadena(objTextBox.Text, clsExpresiones.TipoExpresion.Numeros)
                    If Not IsNothing(objValidacion) Then
                        If objValidacion.TextoValido = False Then
                            objTextBox.Text = objValidacion.CadenaNueva
                            objTextBox.Select(objValidacion.PosicionPrimerInvalido, 0)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el texto.",
                                 Me.ToString(), "TextBox_TextChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TextBox_TextChanged(sender As Object, e As TextChangedEventArgs)
        Try
            If Not IsNothing(sender) Then
                Dim objTextBox As TextBox = CType(sender, TextBox)
                If Not String.IsNullOrEmpty(objTextBox.Text) Then
                    Dim objValidacion = clsExpresiones.ValidarCaracteresEnCadena(objTextBox.Text, clsExpresiones.TipoExpresion.CuentaBancaria)
                    If Not IsNothing(objValidacion) Then
                        If objValidacion.TextoValido = False Then
                            objTextBox.Text = objValidacion.CadenaNueva
                            objTextBox.Select(objValidacion.PosicionPrimerInvalido, 0)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el texto.",
                                 Me.ToString(), "TextBox_TextChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Private Sub BtnNuevoRegistro_Click(sender As Object, e As RoutedEventArgs)
        'Dim cwTasasRendimientos As New cwConfiguracionRendimientos(CType(Me.DataContext, BancosViewModel))
        'Dim as  As New cwConfiguracionRendimientos(CType(Me.DataContext, BancosViewModel), 1)
        Dim cwTasasRendimientos As New cwConfiguracionRendimientos(CType(Me.DataContext, BancosViewModel), 0)
        Program.Modal_OwnerMainWindowsPrincipal(cwTasasRendimientos)
        cwTasasRendimientos.ShowDialog()
    End Sub

    Private Sub BtnEliminarRegistro_Click(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(CType(Me.DataContext, BancosViewModel).ListaBancoTasasRendimientos) Then
            If Not IsNothing(CType(Me.DataContext, BancosViewModel).ListaBancoTasasRendimientosSeleccionado) Then
                CType(Me.DataContext, BancosViewModel).ValidarBorrarConfirmacionTasasRendimientos()
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe Seleccionar un registro.", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        End If
    End Sub

    Private Sub BtnEditarRegistro_Click(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(CType(Me.DataContext, BancosViewModel).ListaBancoTasasRendimientos) Then
            If Not IsNothing(CType(Me.DataContext, BancosViewModel).ListaBancoTasasRendimientosSeleccionado) Then
                Dim cwTasasRendimientos As New cwConfiguracionRendimientos(CType(Me.DataContext, BancosViewModel), 1)
                Program.Modal_OwnerMainWindowsPrincipal(cwTasasRendimientos)
                cwTasasRendimientos.ShowDialog()
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe Seleccionar un registro.", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        End If
    End Sub

    Private Sub chkManejaRangos_Checked(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub chkManejaRangos_Unchecked(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub seleccionarFocoControl(sender As Object, e As RoutedEventArgs)
        Try
            ' Seleccionar el texto del control en el cual el usuario se ubicó
            MyBase.OnGotFocus(e)

            If TypeOf sender Is TextBox Then
                CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
            ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
                'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub IDComitente_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            If Not String.IsNullOrEmpty(IDComitente.Text) Then
                'Await CType(Me.DataContext, BancosViewModel).ConsultarDatosPortafolio(IDComitente.Text)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la asignación del comitente", Me.Name, "IDComitente_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorClienteListaButon_finalizoBusqueda(pstrClaseControl As String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(CType(Me.DataContext, BancosViewModel)) And Not IsNothing(pobjComitente) Then
                CType(Me.DataContext, BancosViewModel).BancoSelected.lngIDComitente = pobjComitente.CodigoOYD
                CType(Me.DataContext, BancosViewModel).BancoSelected.strNombreComitente = pobjComitente.NombreCodigoOYD
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la asignación del comitente", Me.Name, "BuscadorClienteListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCliente_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(CType(Me.DataContext, BancosViewModel).BancoSelected) Then
                If Not IsNothing(CType(Me.DataContext, BancosViewModel)) Then
                    CType(Me.DataContext, BancosViewModel).BancoSelected.lngIDComitente = Nothing
                    CType(Me.DataContext, BancosViewModel).BancoSelected.strNombreComitente = Nothing
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub cmbTipoCuentaRecaudadora_SelectedItemChanged(sender As Object, e As SelectionChangedEventArgs)
        Try
            If Not IsNothing(CType(Me.DataContext, BancosViewModel).BancoSelected) Then
                If Not IsNothing(CType(Me.DataContext, BancosViewModel).BancoSelected.lngTipoCuentaRecaudadora) And CType(Me.DataContext, BancosViewModel).Editando Then
                    If CType(Me.DataContext, BancosViewModel).BancoSelected.lngTipoCuentaRecaudadora = "0" Then
                        CType(Me.DataContext, BancosViewModel).BancoSelected.lngIDComitente = Nothing
                        CType(Me.DataContext, BancosViewModel).BancoSelected.strNombreComitente = Nothing
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "cmbTipoCuentaRecaudadora_SelectedItemChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


End Class


