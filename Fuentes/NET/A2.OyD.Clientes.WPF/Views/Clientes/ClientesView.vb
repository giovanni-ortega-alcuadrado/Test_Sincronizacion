Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: ClientesView.xaml.vb
'Generado el : 07/08/2011 09:34:53
'Propiedad de Alcuadrado S.A. 2010
Imports A2.OyD.OYDServer.RIA.Web
Imports System.ComponentModel
Imports System.Windows.Printing
Imports OpenRiaServices.DomainServices.Client


Partial Public Class ClientesView
    Inherits UserControl
    Const ENTER = 13
    Dim logcargainicial As Boolean = True
    Dim logPintaTabControl As Boolean = True
    Dim logPintaTabControlnuevo As Boolean = True
    Dim logPintaTabControlbuscar As Boolean = True
    Public Sub New()
        Try
            Me.DataContext = New ClientesViewModel
InitializeComponent()
            'Messenger.Default.Register(Of String)(Me,Program.Usuario, Program.HashConexion, AddressOf capturarEventosRegistradosStr)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ClientesView.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub Clientes_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If logcargainicial Then
                logcargainicial = False
                cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
                'cm.DF = df
                CType(Me.DataContext, ClientesViewModel).NombreView = Me.ToString
                CType(Me.DataContext, ClientesViewModel).viewClientes = Me
                inicializa()
                'AddHandler df.ValidationSummary.FocusingInvalidControl,Program.Usuario, Program.HashConexion, AddressOf ValidationSummary_FocusingInvalidControl
                'Else
                '    If Not CType(Me.DataContext, ClientesViewModel).Editando And CType(Me.DataContext, ClientesViewModel).visNavegando = "Visible" Then
                '        CType(Me.DataContext, ClientesViewModel).Autorefresh()
                '    End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "Clientes_Loaded", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Sub inicializa()
        Try
            CType(Me.DataContext, ClientesViewModel).inicializar()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "inicializa", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    'Private Sub ValidationSummary_FocusingInvalidControl(ByVal sender As System.Object, ByVal e As System.Windows.Controls.FocusingInvalidControlEventArgs)
    '    Try
    '        CType(Me.DataContext, ClientesViewModel).seleccionarCampoTab(e.Target.PropertyName)
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al localizar el control.", _
    '                             Me.ToString(), "ValidationSummary_FocusingInvalidControl", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub
    Private Sub dgCtasBancarias_BindingValidationError(ByVal sender As Object, ByVal e As System.Windows.Controls.ValidationErrorEventArgs)
        Try
            e.Handled = True
            'df.ValidateItem()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al localizar el control.", _
                                 Me.ToString(), "dgCtasBancarias_BindingValidationError", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub cm_EventoRefrescarCombos(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoRefrescarCombos
        Try
            If Me.Resources.Contains("A2VM") Then
                CType(Me.DataContext, ClientesViewModel).IsBusy = True
                CType(Me.Resources("A2VM"), A2UtilsViewModel).EjecutaActualizacionCombos(Me.ToString)
                CType(Me.DataContext, ClientesViewModel).IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar los combos.",
                                 Me.ToString(), "cm_EventoRefrescarCombos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub cm_Eventonuevo(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoNuevoRegistro
        Try
            If logPintaTabControlnuevo Then
                logPintaTabControlnuevo = False
                logPintaTabControlbuscar = False
                bdbotones.Visibility = Visibility.Visible
                bdBorder.Visibility = Visibility.Visible
                taClientes.Visibility = Visibility.Visible
                tbcontrol.Visibility = Visibility.Visible
                tbiGeneral.Visibility = Visibility.Visible
                grdGeneral.Visibility = Visibility.Visible
                tbFinanciero.Visibility = Visibility.Visible
                stReceptores.Visibility = Visibility.Visible
                stOrdenantes.Visibility = Visibility.Visible
                stBeneficiarios.Visibility = Visibility.Visible
                grFicha.Visibility = Visibility.Visible
                TabControl1.Visibility = Visibility.Visible
                stUbicacion.Visibility = Visibility.Visible
                grInfAdicional.Visibility = Visibility.Visible
                stDocumentosR.Visibility = Visibility.Visible
                grReplicacion.Visibility = Visibility.Visible
                taClientes.SelectedIndex = 0
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al localizar al iniciar el evento nuevo.", _
                                 Me.ToString(), "cm_Eventonuevo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub cm_Eventoconfirmabusqueda(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoConfirmarBuscar
        Try
            If logPintaTabControlbuscar Then
                logPintaTabControlbuscar = False
                logPintaTabControlnuevo = False
                bdbotones.Visibility = Visibility.Visible
                bdBorder.Visibility = Visibility.Visible
                taClientes.Visibility = Visibility.Visible
                tbcontrol.Visibility = Visibility.Visible
                tbiGeneral.Visibility = Visibility.Visible
                grdGeneral.Visibility = Visibility.Visible
                tbFinanciero.Visibility = Visibility.Visible
                stReceptores.Visibility = Visibility.Visible
                stOrdenantes.Visibility = Visibility.Visible
                stBeneficiarios.Visibility = Visibility.Visible
                grFicha.Visibility = Visibility.Visible
                TabControl1.Visibility = Visibility.Visible
                stUbicacion.Visibility = Visibility.Visible
                grInfAdicional.Visibility = Visibility.Visible
                stDocumentosR.Visibility = Visibility.Visible
                grReplicacion.Visibility = Visibility.Visible
                taClientes.SelectedIndex = 0
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema confirmar la busqueda.", _
                                 Me.ToString(), "cm_Eventoconfirmabusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                Select Case pstrClaseControl
                    Case "IDReceptor"
                        CType(Me.DataContext, ClientesViewModel).ClientesReceptoreSelected.IDReceptor = pobjItem.IdItem
                        CType(Me.DataContext, ClientesViewModel).ClientesReceptoreSelected.Nombre = pobjItem.Nombre
                        CType(Me.DataContext, ClientesViewModel).validarreceptor()
                    Case "IDBanco"
                        CType(Me.DataContext, ClientesViewModel).CuentasClientesSelected.IDBanco = pobjItem.IdItem
                        CType(Me.DataContext, ClientesViewModel).CuentasClientesSelected.NombreBanco = pobjItem.Nombre
                        CType(Me.DataContext, ClientesViewModel).CuentasClientesSelected.CodigoACH = pobjItem.CodItem
                    Case "IDCiudadDoc"
                        CType(Me.DataContext, ClientesViewModel).ClientesBeneficiarioSelected.IDCiudadDoc = CType(pobjItem.IdItem, Integer)
                        CType(Me.DataContext, ClientesViewModel).ClientesBeneficiarioSelected.NombreCiudadDoc = pobjItem.Descripcion
                    Case "IdCiudadDomicilio"
                        CType(Me.DataContext, ClientesViewModel).ClientesBeneficiarioSelected.IdCiudadDomicilio = CType(pobjItem.IdItem, Integer)
                        CType(Me.DataContext, ClientesViewModel).ClientesBeneficiarioSelected.NombreCiudadDomicilio = pobjItem.Descripcion
                    Case "IDCiudadDocu"
                        CType(Me.DataContext, ClientesViewModel).ClientesDireccionesSelected.Ciudad = CType(pobjItem.IdItem, Integer)
                        CType(Me.DataContext, ClientesViewModel).ClientesDireccionesSelected.IdDepartamento = CType(pobjItem.InfoAdicional01, Integer)
                        CType(Me.DataContext, ClientesViewModel).ClientesDireccionesSelected.IdPais = CType(pobjItem.EtiquetaCodItem, Integer)
                        CType(Me.DataContext, ClientesViewModel).ClientesDireccionesSelected.Nombre = pobjItem.Descripcion
                        CType(Me.DataContext, ClientesViewModel).CiudadesClientes.strPoblacion = pobjItem.Nombre
                        CType(Me.DataContext, ClientesViewModel).CiudadesClientes.strdepartamento = pobjItem.CodigoAuxiliar
                        CType(Me.DataContext, ClientesViewModel).CiudadesClientes.strPais = pobjItem.InfoAdicional02
                    Case "IdPoblaciondoc"
                        CType(Me.DataContext, ClientesViewModel).ClienteSelected.IDPoblacionDoc = CType(pobjItem.IdItem, Integer)
                        CType(Me.DataContext, ClientesViewModel).ClienteSelected.IDDepartamentoDoc = CType(pobjItem.InfoAdicional01, Integer)
                        CType(Me.DataContext, ClientesViewModel).ClienteSelected.IDPaisDoc = CType(pobjItem.EtiquetaCodItem, Integer)
                        CType(Me.DataContext, ClientesViewModel).CiudadesClientes.strPoblaciondoc = pobjItem.Nombre
                        CType(Me.DataContext, ClientesViewModel).CiudadesClientes.strdepartamentoDoc = pobjItem.CodigoAuxiliar
                        CType(Me.DataContext, ClientesViewModel).CiudadesClientes.strPaisDoc = pobjItem.InfoAdicional02
                    Case "codigoCiiu"
                        'CType(Me.DataContext, ClientesViewModel).ClienteSelected.codigoCiiu = pobjItem.IdItem
                        'CType(Me.DataContext, ClientesViewModel).CamposSeGenerales.strcodigociu = pobjItem.Descripcion
                        CType(Me.DataContext, ClientesViewModel).asignarCodigoCIIU(pobjItem)
                    Case "IdCiudadRprLegal"
                        CType(Me.DataContext, ClientesViewModel).ClienteSelected.IDCiudadReprLegal = CType(pobjItem.IdItem, Integer)
                        CType(Me.DataContext, ClientesViewModel).CiudadesClientes.strciudad = pobjItem.Nombre
                    Case "IdCiudadNacimiento"
                        CType(Me.DataContext, ClientesViewModel).ClienteSelected.IdCiudadNacimiento = CType(pobjItem.IdItem, Integer)
                        CType(Me.DataContext, ClientesViewModel).CiudadesClientes.strciudadNacimiento = pobjItem.Nombre
                    Case "IdProfesion"
                        CType(Me.DataContext, ClientesViewModel).ClienteSelected.IdProfesion = CType(pobjItem.IdItem, Integer)
                        CType(Me.DataContext, ClientesViewModel).CamposSeGenerales.strProfesion = pobjItem.Descripcion
                    Case "IDCiudad"
                        CType(Me.DataContext, ClientesViewModel).ClientesAccionistasSelected.IDCiudad = CType(pobjItem.IdItem, Integer)
                        CType(Me.DataContext, ClientesViewModel).ClientesAccionistasSelected.NombreCiudad = pobjItem.Descripcion
                    Case "ClienteAccionista"
                        CType(Me.DataContext, ClientesViewModel).ClientesAccionistasSelected.ClienteAccionista = pobjItem.IdItem
                        CType(Me.DataContext, ClientesViewModel).ClientesAccionistasSelected.TipoVinculacionAccionista = pobjItem.Descripcion
                    Case "IDPoblacion"
                        CType(Me.DataContext, ClientesViewModel).ClientesFichaSelected.IDPoblacion = pobjItem.IdItem
                        CType(Me.DataContext, ClientesViewModel).ClientesFichaSelected.NombreCiudad = pobjItem.Descripcion
                    Case "IDCiudadDocup"
                        CType(Me.DataContext, ClientesViewModel).ClientesPersonaselected.idPoblacion = CType(pobjItem.IdItem, Integer)
                        CType(Me.DataContext, ClientesViewModel).ClientesPersonaselected.Ciudad = pobjItem.Descripcion
                    Case "IDCiudadDepEcono"
                        CType(Me.DataContext, ClientesViewModel).ClientesDepEconomicaselected.idPoblacion = CType(pobjItem.IdItem, Integer)
                        CType(Me.DataContext, ClientesViewModel).ClientesDepEconomicaselected.Ciudad = pobjItem.Descripcion
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la respuesta del Buscador generico", _
                                 Me.ToString(), "BuscadorGenerico_finalizoBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub BuscadorGenerico_finalizoBusquedaClientes(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjItem) Then
                Select Case pstrClaseControl
                    Case "IDOrdenante"
                        CType(Me.DataContext, ClientesViewModel).ClientesOrdenantesSelected.IDOrdenante = pobjItem.IdComitente
                        CType(Me.DataContext, ClientesViewModel).ClientesOrdenantesSelected.Nombre = pobjItem.Nombre
                        CType(Me.DataContext, ClientesViewModel).validaordenantes()
                    Case "IDComitentePerDepEco"
                        CType(Me.DataContext, ClientesViewModel).ClientesDepEconomicaselected.IDComitentePerDepEco = pobjItem.IdComitente
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la respuesta del Buscador clientes", _
                                 Me.ToString(), "BuscadorGenerico_finalizoBusquedaClientes", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub RecuperaFocoBuscador(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(DirectCast(sender, A2ComunesControl.BuscadorGenericoListaButon)) Then
                If Not IsNothing(CType(Me.DataContext, ClientesViewModel).ClienteSelected) Then
                    DirectCast(sender, A2ComunesControl.BuscadorGenericoListaButon).Agrupamiento = CType(Me.DataContext, ClientesViewModel).ClienteSelected.strNroDocumento
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al localizar el control.", _
                                 Me.ToString(), "RecuperaFocoBuscador", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub LostFocusCalculos(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            CType(Me.DataContext, ClientesViewModel).Calculadatos()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al localizar el control.", _
                                Me.ToString(), "LostFocusCalculos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

   
    Private Sub LostFocusActivos(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            CType(Me.DataContext, ClientesViewModel).CalcularFormula("Activos")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al localizar el control.", _
                                Me.ToString(), "LostFocusCalculos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub LostFocusPasivos(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            CType(Me.DataContext, ClientesViewModel).CalcularFormula("Pasivos")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al localizar el control.", _
                                Me.ToString(), "LostFocusCalculos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub LostFocusPatrimonio(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            CType(Me.DataContext, ClientesViewModel).CalcularFormula("Patrimonio")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al localizar el control.", _
                                Me.ToString(), "LostFocusCalculos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub LostFocusCalculossmmlv(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            CType(Me.DataContext, ClientesViewModel).calculosmmlv()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al localizar el control.", _
                                Me.ToString(), "LostFocusCalculossmmlv", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub LostFocusNroDocumentoCliente(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            CType(Me.DataContext, ClientesViewModel).ValidarDigitoVerificacion(txtNroDocumentoCliente.Text)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al localizar el control.", _
                                Me.ToString(), "LostFocusNroDocumentoCliente", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub txtCuenta_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        Try
            If Not (e.Key > 47 And e.Key < 58) And Not e.Key = 9 And Not (e.Key > 95 And e.Key < 106) Then
                e.Handled = True
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el digito.", _
                                Me.ToString(), "txtCuenta_KeyDown", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub LayoutRoot1_KeyDown(sender As Object, e As KeyEventArgs) Handles bdBorder.KeyDown
        Try
            If e.Key = ENTER And CType(Me.DataContext, ClientesViewModel).Editando Then
                CType(Me.DataContext, ClientesViewModel).ActualizarRegistro()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el digito.",
                                Me.ToString(), "LayoutRoot1_KeyDown", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub dfbuscar_KeyDown(sender As Object, e As KeyEventArgs) Handles dfBuscar.KeyDown
        Try
            If e.Key = ENTER Then
                Dim filtro As Byte
                If dfBuscar.FindName("chkFiltro").IsChecked Then
                    filtro = 1
                Else
                    filtro = 0
                End If
                CType(Me.DataContext, ClientesViewModel).buscaconenter(dfBuscar.FindName("txtComitente").Text, dfBuscar.FindName("txtNombre").Text, filtro, dfBuscar.FindName("txtstrNroDocumento").Text, dfBuscar.FindName("cmbTipoIden").SelectedValue, dfBuscar.FindName("cmbClasifi").SelectedValue)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el digito.", _
                                Me.ToString(), "dfbuscar_KeyDown", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub dg_LoadingRow(sender As Object, e As GridView.RowLoadedEventArgs) Handles dg.RowLoaded
        Try
            If logPintaTabControl Then
                If Not IsNothing((CType(Me.DataContext, ClientesViewModel).ListaClientes)) Then
                    If (CType(Me.DataContext, ClientesViewModel).ListaClientes).Count > 0 Then
                        logPintaTabControl = False
                        logPintaTabControlnuevo = False
                        logPintaTabControlbuscar = False
                        CType(Me.DataContext, ClientesViewModel).CambiarAForma()
                        bdbotones.Visibility = Visibility.Visible
                        bdBorder.Visibility = Visibility.Visible
                        taClientes.Visibility = Visibility.Visible
                        tbcontrol.Visibility = Visibility.Visible
                        tbiGeneral.Visibility = Visibility.Visible
                        grdGeneral.Visibility = Visibility.Visible
                        tbFinanciero.Visibility = Visibility.Visible
                        stReceptores.Visibility = Visibility.Visible
                        stOrdenantes.Visibility = Visibility.Visible
                        stBeneficiarios.Visibility = Visibility.Visible
                        grFicha.Visibility = Visibility.Visible
                        TabControl1.Visibility = Visibility.Visible
                        stUbicacion.Visibility = Visibility.Visible
                        grInfAdicional.Visibility = Visibility.Visible
                        stDocumentosR.Visibility = Visibility.Visible
                        grReplicacion.Visibility = Visibility.Visible
                        taClientes.SelectedIndex = 0
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga de la fila.", _
                                Me.ToString(), "dg_LoadingRow", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub cmbPerfil_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles cmbPerfil.SelectionChanged
        Try
            If Not IsNothing(CType(sender, ComboBox).SelectedItem) Then
                CType(Me.DataContext, ClientesViewModel).recuperadescripcionperfil(CType(sender, ComboBox).SelectedItem.Descripcion)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema la cambiar el valor del control.", _
                                Me.ToString(), "cmbPerfil_SelectionChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub SeleccionaFatca(sender As Object, e As RoutedEventArgs)
        Try
            If CType(sender, CheckBox).IsChecked Then
                If Not CType(Me.DataContext, ClientesViewModel).DescripcionValida(Program.FatcaDescripcionPais, "|", CType(sender, CheckBox).DataContext.IDNacionalidad) Then
                    CType(sender, CheckBox).IsChecked = False
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema la cambiar el valor del control.", _
                                Me.ToString(), "SeleccionaFatca", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
        'Private Sub SeleccionaTipoCiudadanoFatca(sender As Object, e As C1.Silverlight.PropertyChangedEventArgs(Of Boolean))
    '    Try
    '        If CType(sender, C1.Silverlight.C1ComboBox).IsDropDownOpen Then
    '            If Not CType(Me.DataContext, ClientesViewModel).DescripcionValida(Program.FatcaDescripcionPais, "|", CType(sender, C1.Silverlight.C1ComboBox).DataContext.IDNacionalidad) Then
    '                CType(sender, C1.Silverlight.C1ComboBox).IsDropDownOpen = False
    '            End If
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema la cambiar el valor del control.", _
    '                            Me.ToString(), "SeleccionaTipoCiudadanoFatca", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub
    Private Sub NroDocumentoCliente_TextChanged(sender As Object, e As TextChangedEventArgs)
        Try
            If Not IsNothing(sender) Then
                Dim objTextBox As TextBox = CType(sender, TextBox)
                If Not IsNothing(objTextBox) Then
                    If Not String.IsNullOrEmpty(objTextBox.Text) Then
                        Dim objValidacion = clsExpresiones.ValidarCaracteresEnCadena(objTextBox.Text, clsExpresiones.TipoExpresion.LetrasNumerosUnicamente)
                        If Not IsNothing(objValidacion) Then
                            If objValidacion.TextoValido = False Then
                                objTextBox.Text = objValidacion.CadenaNueva
                                objTextBox.Select(objValidacion.PosicionPrimerInvalido, 0)
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el texto.", _
                                 Me.ToString(), "NroDocumentoCliente_TextChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub LostFocusValidarDireccion(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            CType(Me.DataContext, ClientesViewModel).validardireccionSafyr("Direccion")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al localizar el control.", _
                                Me.ToString(), "LostFocusValidarDireccion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub LostFocusValidarTelefono(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            CType(Me.DataContext, ClientesViewModel).validardireccionSafyr("Telefono")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al localizar el control.", _
                                Me.ToString(), "LostFocusValidarTelefono", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub LostFocusValidarFax(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            CType(Me.DataContext, ClientesViewModel).validardireccionSafyr("Fax")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al localizar el control.", _
                                Me.ToString(), "LostFocusValidarFax", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub LostFocusValidarEntregarA(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            CType(Me.DataContext, ClientesViewModel).validardireccionSafyr("Entregar A")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al localizar el control.", _
                                Me.ToString(), "LostFocusValidarEntregarA", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub LostFocusValidarExtension(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Try
            CType(Me.DataContext, ClientesViewModel).validardireccionSafyr("Extension")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al localizar el control.", _
                                Me.ToString(), "LostFocusValidarExtension", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub txtComitente_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        Try
            'Jorge Andres Bedoya 20160329
            If (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D9) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D8) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D7) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D6) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D5) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D4) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D3) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D2) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D1) Or _
                   (Keyboard.Modifiers = ModifierKeys.Shift And e.Key = Key.D0) Then
                e.Handled = True
            Else
                If (((e.Key >= Key.D0 And e.Key <= Key.D9) Or (e.Key >= Key.NumPad0 And e.Key <= Key.NumPad9) Or e.Key = Key.Back Or e.Key = Key.Tab)) Then
                    e.Handled = False
                Else
                    e.Handled = True
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el codigo ingresado.", _
                                Me.ToString(), "txtComitente_KeyDown", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Private Sub btnEnviarBus_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, ClientesViewModel).EnviarBus()
    End Sub

Private Sub btnPreclientes_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, ClientesViewModel).Preclientes()
    End Sub

Private Sub btnConsultaDescripcionD_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, ClientesViewModel).ConsultaDescripcionD()
    End Sub

Private Sub btnConsultarCustodia_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, ClientesViewModel).ConsultarCustodia()
    End Sub

Private Sub btnConsultarVencimientos_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, ClientesViewModel).ConsultarVencimientos()
    End Sub

Private Sub btnConsultarLiqxCumplir_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, ClientesViewModel).ConsultarLiqxCumplir()
    End Sub

Private Sub btnConsultarRepos_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, ClientesViewModel).ConsultarRepos()
    End Sub

Private Sub btnConsultarFondos_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        CType(Me.DataContext, ClientesViewModel).ConsultarFondos()
    End Sub

Private Sub NumeroID_TextChanged(sender As Object, e As TextChangedEventArgs)
        Try
            If Not IsNothing(sender) Then
                Dim objTextBox As TextBox = CType(sender, TextBox)
                If Not String.IsNullOrEmpty(objTextBox.Text) Then
                    If Not IsNothing(CType(Me.DataContext, ClientesViewModel).ClienteSelected) And Not IsNothing(CType(Me.DataContext, ClientesViewModel).CuentasClientesSelected) Then
                        If CType(Me.DataContext, ClientesViewModel).ClienteSelected.NroDocumento.ToString = objTextBox.Text _
                            And CType(Me.DataContext, ClientesViewModel).ClienteSelected.TipoIdentificacion = CType(Me.DataContext, ClientesViewModel).CuentasClientesSelected.TipoID Then
                            CType(Me.DataContext, ClientesViewModel).CuentasClientesSelected.HabilitarUnicoTitular = True
                        Else
                            CType(Me.DataContext, ClientesViewModel).CuentasClientesSelected.HabilitarUnicoTitular = False
                            CType(Me.DataContext, ClientesViewModel).CuentasClientesSelected.UnicoTitular = False

                        End If
                    End If
                Else
                    If Not IsNothing(CType(Me.DataContext, ClientesViewModel).CuentasClientesSelected) Then

                        CType(Me.DataContext, ClientesViewModel).CuentasClientesSelected.HabilitarUnicoTitular = False
                        CType(Me.DataContext, ClientesViewModel).CuentasClientesSelected.UnicoTitular = False
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el texto.", _
                                 Me.ToString(), "NumeroID_TextChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
End Class