
Imports System.IO

Imports Microsoft.Win32
Imports Telerik.Windows.Controls

Partial Public Class UnificarFormatosView
    Inherits UserControl

#Region "Variables"

    Private mobjVM As UnificarFormatosViewModel
    ' Incluir controlar en el evento load para que se ejecute solo la primera vez que el control se muestra (esto para cuando el control se carga en controles tipo Tab)
    Private mlogInicializar As Boolean = True

#End Region

#Region "Inicializacion"

    ''' <summary>
    ''' Inicializa la ventana para unificar formatos (aplica los estilos a la pantalla)
    ''' </summary>
    ''' <history>
    ''' Creado por   : Jeison Ramírez Pino
    ''' Fecha        : Octubre 08/2016
    ''' Pruebas CB   : Jeison Ramírez Pino - Octubre 08/2016 - Resultado OK
    ''' </history>
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

        Me.DataContext = New UnificarFormatosViewModel
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

    ''' <summary>
    ''' Realiza un llamado sincrono al proceso inicializar del ViewModel.
    ''' </summary>
    ''' <history>
    ''' Creado por   : Jeison Ramírez Pino.
    ''' Fecha        : Octubre 09/2016
    ''' Pruebas CB   : Jeison Ramírez Pino - Octubre 09/2016 - Resultado OK
    ''' </history>
    Private Async Sub inicializar()
        Try
            If Not Me.DataContext Is Nothing Then
                mobjVM = CType(Me.DataContext, UnificarFormatosViewModel)
                mobjVM.NombreView = Me.ToString
                
                mobjVM.inicializar()
                Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la inicialización", Me.Name, "inicializar", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Eventos de controles"

    ''' <summary>
    ''' Selecciona todo el texto que se encuentre en la caja de texto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <history>
    ''' Creado por   : Jeison Ramírez Pino. IOsoft S.A.S.
    ''' Fecha        : Octubre 09/2016
    ''' Pruebas CB   : Jeison Ramírez Pino - Octubre 09/2016 - Resultado OK
    ''' </history>
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la selección del Texto.", Me.Name, "seleccionarFocoControl", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnSubirArchivo_CargarArchivoPrincipal(sender As OpenFileDialog, e As IO.Stream)
        Try
            Dim objDialog = CType(sender, OpenFileDialog)
            If Not IsNothing(objDialog) Then
                If Not IsNothing(objDialog.FileName) Then
                    'If Path.GetExtension(objDialog.FileName).Equals(".csv") Then
                    mobjVM.UnificarFormatosSelected.strFormatoPrincipal = Path.GetFileName(objDialog.FileName)
                    'Else
                    'A2Utilidades.Mensajes.mostrarMensaje("El archivo no tiene formato correcto por favor vuelva a seleccionar el archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó el problema al intentar subir el archivo.", Me.ToString(), "btnSubirArchivoTesoreria_CargarArchivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnUnificar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.Unificar()
    End Sub

    Private Sub btnSubirArchivo_CargarArchivoProducto(sender As OpenFileDialog, e As IO.Stream)
        Try
            Dim objDialog = CType(sender, OpenFileDialog)
            If Not IsNothing(objDialog) Then
                If Not IsNothing(objDialog.FileName) Then
                    'If Path.GetExtension(objDialog.FileName).Equals(".csv") Then
                    mobjVM.UnificarFormatosSelected.strFormatoProductos = Path.GetFileName(objDialog.FileName)
                    'Else
                    'A2Utilidades.Mensajes.mostrarMensaje("El archivo no tiene formato correcto por favor vuelva a seleccionar el archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó el problema al intentar subir el archivo.", Me.ToString(), "btnSubirArchivoTesoreria_CargarArchivo", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

End Class
