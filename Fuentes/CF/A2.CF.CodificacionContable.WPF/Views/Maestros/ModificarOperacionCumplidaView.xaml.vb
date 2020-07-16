Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes


Partial Public Class ModificarOperacionCumplidaView
    Inherits UserControl

    ''' <summary>
    ''' Eventos creados para la comunicación con la clase ModificarOperacionCumplidaViewModel
    ''' Pantalla Modificar Operaciones Cumplidas (Calculos Financieros)
    ''' </summary>
    ''' <history>
    ''' Creado por  : Germán Arbey González Osorio - Alcuadrado S.A.
    ''' Fecha       : Abril 22/2014
    ''' </history>

#Region "Variables"

    ' Permite que se inicialice el control solo en la carga inicial
    Private mlogInicializar As Boolean = True
    ' Variable para instanciar el ViewModel requerido
    Private mobjVM As ModificarOperacionCumplidaViewModel

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

        Me.DataContext = New ModificarOperacionCumplidaViewModel
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
    ''' Inicializa el ViewModel y carga las extensiones
    ''' </summary>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 22/2014
    ''' Descripción  : Inicializa el ViewModel ModificarOperacionCumplidaViewModel para la comunicación con la capa de datos y carga el combo de extensiones
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 25/2014 - Resultado OK
    ''' </history>
    Private Async Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            mobjVM = CType(Me.DataContext, ModificarOperacionCumplidaViewModel)
            mobjVM.NombreView = Me.ToString
            mobjVM.ViewModificarOperacionCumplida = Me

            Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty)

            Await mobjVM.inicializar()

        End If
    End Sub

#End Region

#Region "Métodos para control de eventos"

    ''' <summary>
    ''' Coloca o retira la selección de todas las operaciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <history>
    ''' Creado por   : Germán Arbey González Osorio
    ''' Fecha        : Abril 22/2014
    ''' Descripción  : Coloca o retira la selección de todas las operaciones
    ''' Pruebas CB   : Germán Arbey González Osorio - Abril 25/2014 - Resultado OK
    ''' </history>
    Private Sub btnExportar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ExportarOperaciones()
    End Sub

Private Sub btnActualizar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ActualizarOperaciones()
    End Sub

Private Sub chkSeleccionarTodos_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim chk As CheckBox = CType(sender, CheckBox)
            Dim check As Boolean = chk.IsChecked.Value

            mobjVM.SeleccionarTodasOperaciones(check)

        Catch ex As Exception
            mostrarMensaje("Error en el evento chkSeleccionarTodos_Click.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
        End Try
    End Sub

#End Region

End Class

