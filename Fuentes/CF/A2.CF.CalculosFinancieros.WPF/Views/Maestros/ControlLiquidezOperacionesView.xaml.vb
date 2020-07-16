Imports Telerik.Windows.Controls
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web



Partial Public Class ControlLiquidezOperacionesView
    Inherits UserControl

    ''' <summary>
    ''' Eventos creados para la comunicación con las clases ControlLiquidezOperacionesView y ControlLiquidezOperacionesViewModel
    ''' Pantalla ControlLiquidezOperaciones (Calculos Financieros)
    ''' </summary>
    ''' <remarks>Jorge Peña (Alcuadrado S.A.) - 2 de Septiembre 2016</remarks>
#Region "Variables"

    Private mobjVM As ControlLiquidezOperacionesViewModel
    Private mlogInicializar As Boolean = True ' CCM20130827 - Cristian Ciceri Muñetón - Incluir controlar en el evento load para que se ejecute solo la primera vez que el control se muestra (esto para cuando el control se carga en controles tipo Tab)

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

        Me.DataContext = New ControlLiquidezOperacionesViewModel
InitializeComponent()
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then
                mlogInicializar = False
                ' Asociar el grid de edición y el data forma a la barra de herramientas que controla la edición
                cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1

                inicializar()

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del control", Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub inicializar()
        Try
            If Not Me.DataContext Is Nothing Then
                mobjVM = CType(Me.DataContext, ControlLiquidezOperacionesViewModel)
                mobjVM.NombreView = Me.ToString

                Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty, True)

                Await mobjVM.inicializar()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el método inicializar.", Me.Name, "inicializar", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Eventos de controles"

    Private Sub cm_EventoRefrescarCombos(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoRefrescarCombos
        Try
            If Me.Resources.Contains("A2VM") Then
                mobjVM.IsBusy = True
                CType(Me.Resources("A2VM"), A2UtilsViewModel).EjecutaActualizacionCombos(Me.ToString)
                mobjVM.IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar los combos.",
                                 Me.ToString(), "cm_EventoRefrescarCombos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

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

    Private Sub BuscadorGenerico_finalizo_Dataform_Edicion(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                Select Case pstrClaseControl
                    Case "CompaniaEdicion"
                        Me.mobjVM.EncabezadoSeleccionado.intIDCompania = CType(pobjItem.IdItem, Integer?)
                        Me.mobjVM.EncabezadoSeleccionado.strDescripcionCompania = pobjItem.Nombre
                        mobjVM.strAgrupamiento = "CONTROL_LIQUIDEZ" + CStr(pobjItem.IdItem)
                    Case "CompaniaBusqueda"
                        Me.mobjVM.cb.intIDCompania = CInt(pobjItem.IdItem)
                        Me.mobjVM.cb.strDescripcionCompania = pobjItem.Nombre
                        mobjVM.cb.strAgrupamiento = "CONTROL_LIQUIDEZ" + CStr(pobjItem.IdItem)
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la asignación de la compañía.", Me.Name, "BuscadorGenerico_finalizo_Dataform_Edicion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorClienteListaButon_finalizoBusqueda(pstrClaseControl As String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(mobjVM) And Not IsNothing(pobjComitente) Then
                Select Case pstrClaseControl
                    Case "IDComitenteEdicion"
                        Me.mobjVM.EncabezadoSeleccionado.lngIDComitente = LTrim(RTrim(pobjComitente.CodigoOYD))
                        Me.mobjVM.EncabezadoSeleccionado.strDescripcionComitente = pobjComitente.Nombre
                    Case "IDComitenteBusqueda"
                        Me.mobjVM.cb.lngIDComitente = pobjComitente.CodigoOYD
                        Me.mobjVM.cb.strDescripcionComitente = pobjComitente.Nombre
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la asignación del comitente.", Me.Name, "BuscadorClienteListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub IDComitente_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            If Not String.IsNullOrEmpty(txtPortafolio.Text) Then
                Await Me.mobjVM.ConsultarDatosPortafolio(txtPortafolio.Text)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la la consulta de datos del comitente.", Me.Name, "IDComitente_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Notifica que la propiedad cambió sin tener que perder el foco.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtPortafolio_TextChanged(sender As Object, e As TextChangedEventArgs)
        Try
            CType(sender, TextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource()

            If Not IsNothing(mobjVM.EncabezadoSeleccionado) And mobjVM.Editando Then
                mobjVM.EncabezadoSeleccionado.dblSaldo = 0
                mobjVM.EncabezadoSeleccionado.dblCompras = 0
                mobjVM.EncabezadoSeleccionado.dblVentas = 0
                mobjVM.EncabezadoSeleccionado.dblTotal = 0
                mobjVM.dblTotalAuxiliar = 0
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante el evento del control txtPortafolio.", Me.Name, "txtPortafolio_TextChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Notifica que la propiedad cambió sin tener que perder el foco.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub IDCompania_TextChanged(sender As Object, e As TextChangedEventArgs)
        Try
            CType(sender, TextBox).GetBindingExpression(TextBox.TextProperty).UpdateSource()

            If Not IsNothing(mobjVM.EncabezadoSeleccionado) And mobjVM.Editando Then
                mobjVM.EncabezadoSeleccionado.dblSaldo = 0
                mobjVM.EncabezadoSeleccionado.dblCompras = 0
                mobjVM.EncabezadoSeleccionado.dblVentas = 0
                mobjVM.EncabezadoSeleccionado.dblTotal = 0
                mobjVM.dblTotalAuxiliar = 0
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante el evento del control IDCompania_TextChanged.", Me.Name, "IDCompania_TextChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiar_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                Select Case CType(sender, Button).Name
                    Case "btnLimpiarCompaniaEdicion"
                        Me.mobjVM.EncabezadoSeleccionado.intIDCompania = 0
                        Me.mobjVM.EncabezadoSeleccionado.strDescripcionCompania = String.Empty
                        Me.mobjVM.EncabezadoSeleccionado.lngIDComitente = String.Empty
                        Me.mobjVM.EncabezadoSeleccionado.strDescripcionComitente = String.Empty
                        Me.mobjVM.strAgrupamiento = String.Empty
                    Case "btnLimpiarClienteEdicion"
                        Me.mobjVM.EncabezadoSeleccionado.lngIDComitente = String.Empty
                        Me.mobjVM.EncabezadoSeleccionado.strDescripcionComitente = String.Empty
                    Case "btnLimpiarCompaniaBusqueda"
                        Me.mobjVM.cb.intIDCompania = 0
                        Me.mobjVM.cb.strDescripcionCompania = String.Empty
                        Me.mobjVM.cb.lngIDComitente = String.Empty
                        Me.mobjVM.cb.strDescripcionComitente = String.Empty
                        Me.mobjVM.cb.strAgrupamiento = String.Empty
                    Case "btnLimpiarClienteBusqueda"
                        Me.mobjVM.cb.lngIDComitente = String.Empty
                        Me.mobjVM.cb.strDescripcionComitente = String.Empty
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante el evento limpiar.", Me.Name, "btnLimpiar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorClienteListaButon_GotFocus(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                Select Case CType(sender, A2ComunesControl.BuscadorClienteListaButon).CampoBusqueda
                    Case "IDComitenteEdicion"
                        CType(sender, A2ComunesControl.BuscadorClienteListaButon).Agrupamiento = mobjVM.strAgrupamiento
                    Case "IDComitenteBusqueda"
                        CType(sender, A2ComunesControl.BuscadorClienteListaButon).Agrupamiento = mobjVM.cb.strAgrupamiento
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante el evento del buscador de clientes.", Me.Name, "BuscadorClienteListaButon_GotFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub chkModulos_Checked(sender As Object, e As RoutedEventArgs)
        Try
            If mobjVM.Editando Then
                ListaModulos.GetBindingExpression(ListBox.SelectedItemProperty).UpdateSource()
                CType(sender, CheckBox).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource()

                If Not IsNothing(ListaModulos) Then

                    If mobjVM.ListaModulos.Where(Function(i) CType(i.logSeleccionado, Boolean) = True).Count > 0 Then

                        Dim objListaSubmodulos = From SUBMODULOS In mobjVM.ListaModulosSubmodulos
                                                 Join MODULOS In mobjVM.ListaModulos.Where(Function(i) CType(i.logSeleccionado, Boolean) = True) On SUBMODULOS.strIDOwner Equals MODULOS.strRetorno
                                                 Select SUBMODULOS
                                                 Where SUBMODULOS.strTopico = "SUBMODULOS"


                        If Not IsNothing(objListaSubmodulos) Then

                            Dim objListaSubmodulosFinal As New List(Of CFCalculosFinancieros.ModulosSubmodulos)

                            For Each li In objListaSubmodulos
                                If objListaSubmodulosFinal.Where(Function(i) i.strDescripcion = li.strDescripcion).Count = 0 Then
                                    objListaSubmodulosFinal.Add(li)
                                End If
                            Next

                            mobjVM.ListaSubmodulos = objListaSubmodulosFinal
                        End If

                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar el módulo.", _
                                                Me.ToString(), "chkModulos_Checked", Application.Current.ToString(), Program.Maquina, ex.InnerException)
        End Try
    End Sub

    Private Sub chkModulos_Unchecked(sender As Object, e As RoutedEventArgs)
        Try
            If mobjVM.Editando Then
                ListaModulos.GetBindingExpression(ListBox.SelectedItemProperty).UpdateSource()
                CType(sender, CheckBox).GetBindingExpression(CheckBox.IsCheckedProperty).UpdateSource()

                If Not IsNothing(ListaModulos) Then

                    'If mobjVM.ListaModulos.Where(Function(i) CType(i.logSeleccionado, Boolean) = True).Count > 0 Then

                    Dim objListaSubmodulos = From SUBMODULOS In mobjVM.ListaModulosSubmodulos
                                             Join MODULOS In mobjVM.ListaModulos.Where(Function(i) CType(i.logSeleccionado, Boolean) = True) On SUBMODULOS.strIDOwner Equals MODULOS.strRetorno
                                             Select SUBMODULOS
                                             Where SUBMODULOS.strTopico = "SUBMODULOS"

                    Dim objListaSubmodulosFinal As New List(Of CFCalculosFinancieros.ModulosSubmodulos)

                    For Each li In objListaSubmodulos
                        If objListaSubmodulosFinal.Where(Function(i) i.strDescripcion = li.strDescripcion).Count = 0 Then
                            objListaSubmodulosFinal.Add(li)
                        End If
                    Next

                    mobjVM.ListaSubmodulos = objListaSubmodulosFinal

                    'End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al retirar la selección del módulo.", _
                                                Me.ToString(), "chkModulos_Unchecked", Application.Current.ToString(), Program.Maquina, ex.InnerException)
        End Try
    End Sub

#End Region

#Region "Manejadores error"

    Private Sub btnBuscar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.BuscarOperaciones()
    End Sub

Private Sub btnEditar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ActualizarDetalle()
    End Sub

Private Sub btnNuevo_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.IngresarDetalle()
    End Sub

Private Sub btnBorrar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.BorrarDetalle()
    End Sub

Private Sub btnExportar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ExportarInformacion()
    End Sub

Private Sub dgGrid_BindingValidationError(ByVal sender As Object, ByVal e As System.Windows.Controls.ValidationErrorEventArgs)
        Try
            ' Control de error del bindding del grid
            If Not e.Error.Exception Is Nothing Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema para presentar los datos del detalle.", Me.Name, "dgGrid_BindingValidationError", Program.TituloSistema, Program.Maquina, e.Error.Exception)
            End If
            e.Handled = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga de los datos", Me.Name, "dgGrid_BindingValidationError", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

End Class


