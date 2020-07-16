Imports Telerik.Windows.Controls
Imports A2Utilidades
Imports OyDCtl = A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web
Imports GalaSoft.MvvmLight.Messaging
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades

Partial Public Class GeneradorConsultaView
    Inherits UserControl

#Region "Variables"

    Private WithEvents mobjVM As EjecutarScriptsViewModel
    Private mlogInicializar As Boolean = True
    Private NombreScriptFiltro As String = String.Empty
    Private EsDesdePantallaPrincipal As Boolean = False

#End Region

#Region "Inicializacion"

    Public Sub New()
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.EncuentaContabilidad)
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Try
            Me.Resources.Add("A2VM", (New A2UtilsViewModel()))
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Try
            Me.DataContext = New EjecutarScriptsViewModel()

            InitializeComponent()

            If Application.Current.Resources.Contains("QueryString") Then
                For Each item In Application.Current.Resources("QueryString").ToString().Split(CChar("&"))
                    If item.ToUpper.StartsWith("NOMBRE") Then
                        Me.NombreScriptFiltro = item.Split(CChar("=")).Last
                    End If
                Next
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del control", Me.Name, "New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Public Sub New(ByVal pobjViewModel As EjecutarScriptsViewModel)
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.EncuentaContabilidad)
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Try
            EsDesdePantallaPrincipal = True

            Me.DataContext = pobjViewModel

            InitializeComponent()
            inicializarFormulario()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del control", Me.Name, "New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        inicializarFormulario()
    End Sub

    Private  Sub inicializarFormulario()
        Try
            If mlogInicializar Then
                mlogInicializar = False
                mobjVM = CType(Me.DataContext, EjecutarScriptsViewModel)

                If EsDesdePantallaPrincipal Then
                    mobjVM.logEsSoloUnScript = False
                Else
                    mobjVM.logEsSoloUnScript = True
                    If Not String.IsNullOrEmpty(Me.NombreScriptFiltro) Then
                        mobjVM.NombreScriptFiltro = Me.NombreScriptFiltro
                        mobjVM.logEsFiltroGrupo = False
                        mobjVM.GrupoScriptFiltro = String.Empty
                    End If

                    inicializar()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del control", Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            mobjVM.NombreView = Me.ToString

            Await mobjVM.inicializar()
        End If
    End Sub

#End Region

#Region "Eventos de controles"

    Private Sub seleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
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

    Private Sub mobjVM_actualizarControles() Handles mobjVM.actualizarControles
        Dim objRetorno As clsObjetosAgregados
        Dim objGrid As Grid

        Try
            If Not mobjVM Is Nothing Then
                If Not IsNothing(Me.splControles) Then
                    If Not IsNothing(Me.splControles.Children) Then
                        If Me.splControles.Children.Count > 0 Then
                            Dim objGridRegistrado As Grid = Me.splControles.Children(0)
                            If Not IsNothing(objGridRegistrado) Then
                                For Each li In objGridRegistrado.Children
                                    If Not String.IsNullOrEmpty(li.Name) Then
                                        objGridRegistrado.UnregisterName(li.Name)
                                    End If
                                Next
                            End If
                            Me.splControles.Children.Clear()
                        End If
                        objRetorno = mobjVM.crearControlesParametros()
                        objGrid = objRetorno.GridRetorno
                        Me.splControles.Children.Add(objGrid)

                        If Not IsNothing(objRetorno.ListaControles) Then
                            For Each li In objRetorno.ListaControles
                                objGrid.RegisterName(li.Nombre, li.Elemento)
                            Next
                        End If

                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la generación de los parámetros", Me.Name, "mobjVM_actualizarControles", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub mobjVM_actualizarControlesDependientes(ByVal pstrParametroCambio As String, ByVal pstrValorParametro As String) Handles mobjVM.actualizarControlesDependientes
        Try
            If Not mobjVM Is Nothing Then
                If Not String.IsNullOrEmpty(pstrParametroCambio) Then
                    If mobjVM.ListaDependenciaParametros.Where(Function(i) i.NombreParametro = pstrParametroCambio).Count > 0 Then
                        Dim strParametroDependiente As String = mobjVM.ListaDependenciaParametros.Where(Function(i) i.NombreParametro = pstrParametroCambio).First.NombreDependencia
                        strParametroDependiente = strParametroDependiente.Replace("@", "")
                        Dim objControl = Me.FindName(strParametroDependiente)
                        If Not IsNothing(objControl) Then
                            If TypeOf objControl Is TextBox Then
                                CType(objControl, TextBox).Text = String.Empty
                            ElseIf TypeOf objControl Is A2DatePicker Then
                                CType(objControl, A2DatePicker).SelectedDate = Nothing
                            ElseIf TypeOf objControl Is ComboBox Then
                                Dim strFuenteDatos As String = CType(objControl, ComboBox).Tag
                                If Not String.IsNullOrEmpty(strFuenteDatos) Then
                                    CType(objControl, ComboBox).SelectedValue = String.Empty
                                    Dim strValoresComboRefrescar As String = String.Format("{0},{1},{2}",
                                                                                          strParametroDependiente,
                                                                                          strFuenteDatos,
                                                                                          pstrValorParametro)
                                    mobjVM.ConsultarInformacionComboDependiente(strValoresComboRefrescar)
                                End If
                            ElseIf TypeOf objControl Is A2ComunesControl.BuscadorGenerico Then
                                CType(objControl, A2ComunesControl.BuscadorGenerico).Agrupamiento = pstrValorParametro
                                CType(objControl, A2ComunesControl.BuscadorGenerico).Limpiar()
                            ElseIf TypeOf objControl Is A2ComunesControl.BuscadorEspecie Then
                                CType(objControl, A2ComunesControl.BuscadorEspecie).Agrupamiento = pstrValorParametro
                                CType(objControl, A2ComunesControl.BuscadorEspecie).Limpiar()
                            ElseIf TypeOf objControl Is A2ComunesControl.BuscadorCliente Then
                                CType(objControl, A2ComunesControl.BuscadorCliente).Agrupamiento = pstrValorParametro
                                CType(objControl, A2ComunesControl.BuscadorCliente).Limpiar()
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la generación de los parámetros", Me.Name, "mobjVM_actualizarControlesDependientes", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub mobjVM_actualizarListaComboDependiente(ByVal pstrParametroCambio As String, ByVal pobjListaNueva As List(Of OYDUtilidades.ItemCombo)) Handles mobjVM.actualizarListaComboDependiente
        Try
            If Not mobjVM Is Nothing Then
                Dim objControl = Me.FindName(pstrParametroCambio)
                If Not IsNothing(objControl) Then
                    CType(objControl, ComboBox).SelectedValue = String.Empty
                    CType(objControl, ComboBox).ItemsSource = pobjListaNueva
                    If pobjListaNueva.Count = 1 Then
                        CType(objControl, ComboBox).SelectedValue = pobjListaNueva.First.ID
                    End If
                End If
                mobjVM.IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la generación de los parámetros", Me.Name, "mobjVM_actualizarListaComboDependiente", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            mobjVM.IsBusy = False
        End Try
    End Sub


    Private Async Sub cmdEjecutar_Click(sender As Object, e As RoutedEventArgs)

        Dim objControl As FrameworkElement
        Dim objControlesParam As Dictionary(Of String, FrameworkElement)

        Try
            If Not mobjVM Is Nothing Then
                objControlesParam = New Dictionary(Of String, FrameworkElement)

                If mobjVM.ListaParametros Is Nothing Then
                    A2Utilidades.Mensajes.mostrarMensaje("Por favor seleccionar el script que se debe ejecutar.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)

                    Exit Sub
                End If

                For Each objParametro In mobjVM.ListaParametros
                    objControl = CType(FindName(objParametro.Parametro.Replace("@", "")), FrameworkElement)
                    If Not objControl Is Nothing Then
                        objControlesParam.Add(objParametro.Parametro, objControl)
                    End If
                Next

                Await mobjVM.ejecutarScript(objControlesParam)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para verificar los valores de los parámetros", Me.Name, "cmdEjecutar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    Private Async Sub ControlRefrescarCache_EventoRefrescarCombos(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                mobjVM.IsBusy = True
                Await mobjVM.objA2ViewModel.consultarCombos(String.Empty, String.Empty, Program.Usuario, True)
                Await mobjVM.objA2ViewModel.consultarCombos(EjecutarScriptsViewModel.MSTR_COMBOS_ESPECIFICOS, EjecutarScriptsViewModel.MSTR_COMBOS_ESPECIFICOS, Program.Usuario, True)
                If Not mobjVM Is Nothing Then
                    If Not IsNothing(mobjVM.EncabezadoSeleccionado) Then
                        If Not IsNothing(Me.splControles) Then
                            Dim objRetorno As clsObjetosAgregados
                            Dim objGrid As Controls.Grid

                            If Not IsNothing(Me.splControles.Children) Then
                                If Me.splControles.Children.Count > 0 Then
                                    Dim objGridRegistrado As Grid = Me.splControles.Children(0)
                                    If Not IsNothing(objGridRegistrado) Then
                                        For Each li In objGridRegistrado.Children
                                            If Not String.IsNullOrEmpty(li.Name) Then
                                                objGridRegistrado.UnregisterName(li.Name)
                                            End If
                                        Next
                                    End If
                                    Me.splControles.Children.Clear()
                                End If
                                objRetorno = mobjVM.crearControlesParametros(True)
                                objGrid = objRetorno.GridRetorno
                                Me.splControles.Children.Add(objGrid)

                                If Not IsNothing(objRetorno.ListaControles) Then
                                    For Each li In objRetorno.ListaControles
                                        objGrid.RegisterName(li.Nombre, li.Elemento)
                                    Next
                                End If
                            End If
                        End If
                    End If
                End If

                Await mobjVM.consultarDisenosScript()

                mobjVM.IsBusy = False
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para refrescar los combos", Me.Name, "ControlRefrescarCache_EventoRefrescarCombos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub







#End Region

#Region "Manejadores error"


#End Region


End Class
