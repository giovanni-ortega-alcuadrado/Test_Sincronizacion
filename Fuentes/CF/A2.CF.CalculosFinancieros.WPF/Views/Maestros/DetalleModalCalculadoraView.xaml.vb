Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes

Partial Public Class DetalleModalCalculadoraView
    Inherits Window

#Region "Variables"

    Private mobjVM As DetalleModalCalculadoraViewModal

#End Region

#Region "Inicializacion"

    ''' <summary>
    ''' Inicializa la ventana para la DetalleModalCalculadoraView (aplica los estilos a la pantalla), 
    ''' esta muestra el resultado de la calculadora financiera básica
    ''' </summary>
    ''' <param name="pmobjVM">Requiere el ViewModel para asociarlo a la ventana y consultar el resultado</param>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Junio 17/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Junio 17/2014 - Resultado OK
    ''' </history>
    Public Sub New(ByVal pmobjVM As DetalleModalCalculadoraViewModal)
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.EncuentaContabilidad)
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        mobjVM = pmobjVM
        Me.DataContext = mobjVM

        Inicializar()

        InitializeComponent()

    End Sub

    ''' <summary>
    ''' Inicializa el viewmodel para realizar la consulta de los resultados
    ''' </summary>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Junio 17/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Junio 17/2014 - Resultado OK
    ''' </history>
    Private Async Sub Inicializar()
        Await mobjVM.inicializar()
    End Sub

#End Region

#Region "Métodos para control de eventos"

    ''' <summary>
    ''' Controlador de eventos para el botón Aceptar, para cerrar la ventana modal
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Junio 17/2014
    ''' Pruebas CB   : Germán Arbey González Osorio - Junio 17/2014 - Resultado OK
    ''' </history>
    Private Sub Aceptar_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.Close()
        Catch ex As Exception
            mostrarMensaje("Error en el método Aceptar_Click.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
        End Try
    End Sub

#End Region

End Class
