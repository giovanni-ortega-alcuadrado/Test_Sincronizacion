﻿Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class OrdenesPLUSOFView
    Inherits UserControl
    Dim objVMA2Utils As A2UtilsViewModel
    Dim objVMOrdenes As OrdenesOYDPLUSOFViewModel
    Private mlogInicializado As Boolean = False
    Private mlogErrorInicializando As Boolean = False
    Private pobjModulo As A2OYDPLUSUtilidades.Utilidades_ModulosUsuario = Nothing
    Dim strClaseCombos As String = "Ord_OyDPLUS"
    Private mstrDicCombosEspecificos As String = String.Empty

    Public Sub New()
        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            mstrDicCombosEspecificos = String.Format("Combos_{0}", Me.ToString)
            objVMA2Utils = New A2UtilsViewModel(strClaseCombos, mstrDicCombosEspecificos)
            Me.Resources.Add("A2VM", objVMA2Utils)

            objVMA2Utils.IsBusy = True
            Me.DataContext = New OrdenesOYDPLUSOFViewModel
InitializeComponent()
            AddHandler Me.Unloaded, AddressOf View_Unloaded
            AddHandler Me.SizeChanged, AddressOf CambioDePantalla

            Me.stackBotones.Width = Application.Current.MainWindow.ActualWidth * 0.96
            Me.stackTicket.Width = Application.Current.MainWindow.ActualWidth * 0.96
            Me.ScrollForma.Width = Application.Current.MainWindow.ActualWidth * 0.96
            Me.ScrollForma.Height = Application.Current.MainWindow.ActualHeight - 295
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar los componentes del formulario", Me.ToString(), "OrdenesPLUSView", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    Public Sub New(ByVal pobjModuloSeleccionado As A2OYDPLUSUtilidades.Utilidades_ModulosUsuario)
        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            Dim strClaseCombos As String = "Ord_OyDPLUS"

            mstrDicCombosEspecificos = String.Format("Combos_{0}", Me.ToString)
            objVMA2Utils = New A2UtilsViewModel(strClaseCombos, mstrDicCombosEspecificos)
            Me.Resources.Add("A2VM", objVMA2Utils)

            objVMA2Utils.IsBusy = True
            Me.DataContext = New OrdenesOYDPLUSOFViewModel
InitializeComponent()
            AddHandler Me.Unloaded, AddressOf View_Unloaded
            AddHandler Me.SizeChanged, AddressOf CambioDePantalla

            Me.stackBotones.Width = Application.Current.MainWindow.ActualWidth * 0.96
            Me.stackTicket.Width = Application.Current.MainWindow.ActualWidth * 0.96
            Me.ScrollForma.Width = Application.Current.MainWindow.ActualWidth * 0.96
            Me.ScrollForma.Height = Application.Current.MainWindow.ActualHeight - 295

            pobjModulo = pobjModuloSeleccionado
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar los componentes del formulario", Me.ToString(), "OrdenesPLUSView", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    Private Sub Ordenes_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializado = False Then
                If Not IsNothing(objVMA2Utils) Then
                    cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1

                    objVMOrdenes = CType(Me.DataContext, OrdenesOYDPLUSOFViewModel)

                    If Not IsNothing(pobjModulo) Then
                        txtTituloOrdenes.Text = pobjModulo.TituloVistaModulo
                        objVMOrdenes.DiccionarioBotonesOrdenes = pobjModulo.CamposControlMenu
                        objVMOrdenes.Modulo = pobjModulo.Modulo

                        If objVMOrdenes.DiccionarioBotonesOrdenes.ContainsKey("AbrirPlantillas") Then
                            objVMOrdenes.HabilitarAbrirPlantillas = True
                        Else
                            objVMOrdenes.HabilitarAbrirPlantillas = False
                        End If
                        If objVMOrdenes.DiccionarioBotonesOrdenes.ContainsKey("GenerarPlantilla") Then
                            objVMOrdenes.HabilitarGenerarPlantillas = True
                        Else
                            objVMOrdenes.HabilitarGenerarPlantillas = False
                        End If
                        If objVMOrdenes.DiccionarioBotonesOrdenes.ContainsKey("Duplicar") Then
                            objVMOrdenes.HabilitarDuplicar = True
                        Else
                            objVMOrdenes.HabilitarDuplicar = False
                        End If
                    Else
                        objVMOrdenes.DiccionarioBotonesOrdenes = objVMOrdenes.DicBotonesMenuVM
                        objVMOrdenes.HabilitarAbrirPlantillas = True
                        objVMOrdenes.HabilitarGenerarPlantillas = True
                        objVMOrdenes.HabilitarDuplicar = True
                    End If

                    objVMOrdenes.ListaCombosEsp = strClaseCombos
                    objVMOrdenes.ViewOrdenesOYDPLUS = Me
                    objVMOrdenes.visNavegando = "Collapse"
                    'AddHandler Application.Current.Host.Content.Resized, AddressOf CambioDePantalla

                    'Inicia el timer de ordenes
                    If Not IsNothing(objVMOrdenes) Then
                        objVMOrdenes.ReiniciaTimer()
                    End If
                    'scrollEdicion.MaxWidth = Application.Current.MainWindow.ActualWidth - 100
                End If

                mlogInicializado = True
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de órdenes", Me.Name, "", "Ordenes_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        Me.stackBotones.Width = Application.Current.MainWindow.ActualWidth * 0.96
        Me.stackTicket.Width = Application.Current.MainWindow.ActualWidth * 0.96
        Me.ScrollForma.Width = Application.Current.MainWindow.ActualWidth * 0.96
        Me.ScrollForma.Height = Application.Current.MainWindow.ActualHeight - 295
    End Sub

    Private Sub btnRefrescarPreciosBolsa_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(objVMOrdenes) Then
                Me.objVMOrdenes.CargarMensajeDinamicoOYDPLUS("PRECIOSMERCADO", String.Empty, String.Empty, String.Empty)
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al refrescar los precios de bolsa", Me.Name, "btnRefrescarPreciosBolsa_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub View_Unloaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Me.Unloaded
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMOrdenes) Then
                Me.objVMOrdenes.pararTemporizador()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar cerrar la pantalla de ordenes.", Me.Name, "View_Unloaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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

Private Sub btnConsultarPlantillas_Click(sender As Object, e As RoutedEventArgs)
        objVMOrdenes.PreguntarAbrirConsultaPlantillasOrden()
    End Sub

    Private Sub btnGenerarPlantilla_Click(sender As Object, e As RoutedEventArgs)
        objVMOrdenes.PreguntarGenerarPlantillaOrden()
    End Sub

    Private Sub btnDuplicar_Click(sender As Object, e As RoutedEventArgs)
        objVMOrdenes.PreguntarDuplicarOrden()
    End Sub

    Private Sub cm_EventoRefrescarCombos(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoRefrescarCombos
        Try
            If Me.Resources.Contains("A2VM") Then
                objVMOrdenes.IsBusy = True
                CType(Me.Resources("A2VM"), A2UtilsViewModel).EjecutaActualizacionCombos(Me.ToString)
                objVMOrdenes.IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar los combos.",
                                 Me.ToString(), "cm_EventoRefrescarCombos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub btnRefrescarPantalla_Click(sender As Object, e As RoutedEventArgs)
        objVMOrdenes.RecargarPantallaOrdenes()
    End Sub
End Class
