﻿Imports Telerik.Windows.Controls


Partial Public Class ExportarFormatosView
    Inherits UserControl

#Region "Variables"

    Private mobjVM As ExportarFormatosViewModel
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

        Me.DataContext = New ExportarFormatosViewModel
InitializeComponent()
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then
                mlogInicializar = False
                '' Asociar el grid de edición y el data forma a la barra de herramientas que controla la edición
                

                inicializar()

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del control", Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnExportar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ExportarArchivo()
    End Sub

Private Sub btnLimpiar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.LlenarValoresPorDefecto()
    End Sub

Private Async Sub inicializar()
        Try
            If Not Me.DataContext Is Nothing Then
                mobjVM = CType(Me.DataContext, ExportarFormatosViewModel)
                mobjVM.NombreView = Me.ToString

                Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty, True)

                mobjVM.inicializar()


            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la inicialización", Me.Name, "inicializar", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try


    End Sub
#End Region



End Class

