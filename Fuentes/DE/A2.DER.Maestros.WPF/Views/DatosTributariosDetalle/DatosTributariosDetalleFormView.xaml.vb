﻿Imports A2.OyD.OYDServer.RIA.WEB
Imports Telerik.Windows.Controls

Partial Public Class DatosTributariosDetalleFormView
    Inherits Window

    ''' <summary>
    ''' Eventos creados para la comunicación con las clases DatosTributariosDetalleView y DatosTributariosDetalleViewModel
    ''' Pantalla Configuración de Parámetros (Maestros)
    ''' </summary>
    ''' <remarks>(Alcuadrado S.A.) - 30 de Octubre 2017</remarks>
#Region "Variables"

    Private mobjVM As DatosTributariosDetalleViewModel
    Private mlogInicializar As Boolean = True
    Private mobjPaginador As RadDataPager = Nothing

#End Region

#Region "Inicializacion"
    ''' <history>
    ''' </history>
    Public Sub New(ByVal pobjViewModel As DatosTributariosDetalleViewModel)
        InitializeComponent()
        mobjVM = pobjViewModel
        Me.DataContext = mobjVM
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then
                mlogInicializar = False
                ' Asociar el grid de edición y el data forma a la barra de herramientas que controla la edición
                cm.GridViewRegistros = mobjVM.viewListaPrincipal.datapager1
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Eventos de controles"

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

    Private Async Sub btnGuardar_Click(sender As Object, e As RoutedEventArgs)
        If Await mobjVM.ActualizarRegistroDetalle() Then
            mobjVM.Editando = False
            Me.DialogResult = True
        End If
    End Sub

    Private Async Sub btnGuardarContinuar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.logCambiarEncabezadoSeleccionado = False
        If Await mobjVM.ActualizarRegistroDetalle() Then
            mobjVM.CrearNuevoDetalle(True)
        End If
        mobjVM.logCambiarEncabezadoSeleccionado = True
    End Sub

    Private Async Sub btnGuardarContinuarNuevo_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.logCambiarEncabezadoSeleccionado = False
        If Await mobjVM.ActualizarRegistroDetalle() Then
            mobjVM.CrearNuevoDetalle()
        End If
        mobjVM.logCambiarEncabezadoSeleccionado = True
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.Editando = False
        mobjVM.CancelarRegistroDetalle()
    End Sub

    Private Sub btnNuevoRegistro_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.NuevoRegistroDetalle()
    End Sub

    Private Sub btnEditarRegistro_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.EditarRegistroDetalle()
    End Sub

    Private Sub btnBorrarRegistro_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.BorrarRegistroDetalle()
    End Sub


#End Region

#Region "Manejadores error"

    Private Sub dgGrid_BindingValidationError(ByVal sender As Object, ByVal e As System.Windows.Controls.ValidationErrorEventArgs)
        Try
            ' Control de error del bindding del grid
            If Not e.Error.Exception Is Nothing Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "dgGrid_BindingValidationError", Program.TituloSistema, Program.Maquina, e.Error.Exception)
            End If
            e.Handled = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, mobjVM.DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.Name, "dgGrid_BindingValidationError", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    Private Sub ControlNotificacionInconsistencias_EventoVisualizacionErrores(sender As Object, e As RoutedEventArgs)
        mobjVM.ActualizarListaInconsistencias(mobjVM.ListaInconsistenciasActualizacion, Program.TituloSistema, True)
    End Sub

#End Region

End Class
