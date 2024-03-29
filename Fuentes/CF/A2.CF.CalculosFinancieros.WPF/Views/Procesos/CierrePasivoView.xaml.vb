﻿Imports Telerik.Windows.Controls

Partial Public Class CierrePasivoView
    Inherits UserControl

    ''' <summary>
    ''' Eventos creados para la comunicación con las clases CierrePasivoViewModel
    ''' Pantalla Cierre Pasivo (Calculos Financieros)
    ''' </summary>


#Region "Variables"

    Private mobjVM As CierrePasivoViewModel
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
            'Me.Resources.Add("A2VM", (New A2UtilsViewModel()))
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Me.DataContext = New CierrePasivoViewModel
InitializeComponent()
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then
                mlogInicializar = False

                inicializar()

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del control", Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            mobjVM = CType(Me.DataContext, CierrePasivoViewModel)
            mobjVM.NombreView = Me.ToString

            Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty)

            mobjVM.inicializar()

        End If
    End Sub

#End Region

#Region "Eventos de controles"

#End Region

#Region "Manejadores error"

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

    Private Sub btnProcesar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ProcesarCierrePasivo()
    End Sub

Private Sub btnInformeValoracion_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ExportarInformeValoracion()
    End Sub

Private Sub btnRefrescarConsultarInformeProcesamiento_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ConsultarInformeProcesamiento()
    End Sub

Private Sub btnExportarExcel_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ExportarAvanceProcesamiento()
    End Sub

Private Sub CierrePasivoView_Unloaded(sender As Object, e As RoutedEventArgs) Handles Me.Unloaded
        If Me.DataContext IsNot Nothing Then
            CType(Me.DataContext, CierrePasivoViewModel).CerrarTemporizador()
        End If
    End Sub
End Class
