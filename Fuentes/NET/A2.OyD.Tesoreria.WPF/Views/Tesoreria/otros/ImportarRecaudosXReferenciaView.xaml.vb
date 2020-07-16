
Imports System.IO

Imports Microsoft.Win32
Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports Microsoft.VisualBasic.CompilerServices


Partial Public Class ImportarRecaudosXReferenciaView
    Inherits UserControl


#Region "Variables"

    Private mobjVM As ImportarRecaudosXReferenciaViewModel
    Private mlogInicializar As Boolean = True

#End Region

#Region "Inicializacion"
    Public Sub New()
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Try
            'objA2VM = New A2UtilsViewModel()
            'Me.Resources.Add("A2VM", objA2VM)

            Me.Resources.Add("A2VM", (New A2UtilsViewModel()))
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Me.DataContext = New ImportarRecaudosXReferenciaViewModel
InitializeComponent()
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then
                mlogInicializar = False
                ' Asociar el grid de edición y el data forma a la barra de herramientas que controla la edición
                

                inicializar()

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del control", Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            mobjVM = CType(Me.DataContext, ImportarRecaudosXReferenciaViewModel)
            mobjVM.NombreView = Me.ToString
            dtmFecha.Text = Now.Date
            'mobjVM.ViewGenerarRecibosCaja_Intermedia = Me

            'Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty)

            mobjVM.inicializar()
        End If
    End Sub

#End Region

#Region "Eventos de controles"

    Private Sub GenerarRC_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        mobjVM.GenerarRC()
    End Sub

    Private Sub btnSubirArchivoRecaudo_CargarArchivo(sender As OpenFileDialog, e As IO.Stream)
        Try
            mobjVM = CType(Me.DataContext, ImportarRecaudosXReferenciaViewModel)
            Dim objDialog = CType(sender, OpenFileDialog)
            If Not IsNothing(objDialog) Then
                If Not IsNothing(objDialog.FileName) Then
                    If Path.GetExtension(objDialog.FileName).Equals(".txt") Or Path.GetExtension(objDialog.FileName).Equals(".xls") Or Path.GetExtension(objDialog.FileName).Equals(".xlsx") Then
                        mobjVM.NombreArchivo = Path.GetFileName(objDialog.FileName)
                        mobjVM.ImportarArchivo()
                    Else
                        mostrarMensaje("El archivo no tiene el formato correcto, por favor vuelva a seleccionar el archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó el problema al intentar subir el archivo.", Me.ToString(), "btnSubirArchivoRecaudo_CargarArchivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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

End Class


