Imports Telerik.Windows.Controls
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes


Partial Public Class ConsecutivosVsUsuariosView
    Inherits UserControl

    ''' <summary>
    ''' Eventos creados para la comunicación con las clases ConsecutivosVsUsuariosView y ConsecutivosVsUsuariosViewModel
    ''' Pantalla Consecutivos vs Usuario
    ''' </summary>
    ''' <remarks>Jose Walter Sierra - Mayo 05/2016(Alcuadrado S.A.) - Mayo 05/2016</remarks>
#Region "Variables"

    Dim mobjVM As ConsecutivosVsUsuariosViewModel
    Private mlogInicializar As Boolean = True

#End Region

#Region "Inicializacion"

    ''' <summary>
    ''' Inicializa los estilos metro para la pantalla 
    ''' </summary>
    ''' <history>
    ''' Creado por   : Jose Walter Sierra
    ''' Fecha        : Mayo 05/2016
    ''' Pruebas CB   : Jose Walter Sierra - Mayo 05/2016 - Resultado OK
    ''' </history>
    Public Sub New()
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.EncuentaContabilidad)
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try
        'A2ControlMenu.ControlMenuA2.
        Try
            Me.Resources.Add("A2VM", (New A2UtilsViewModel()))
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        Me.DataContext = New ConsecutivosVsUsuariosViewModel
InitializeComponent()
    End Sub

    ''' <summary>
    ''' Método que inicializa el view
    ''' </summary>
    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then
                mlogInicializar = False

                If Not Me.DataContext Is Nothing Then
                    mobjVM = CType(Me.DataContext, ConsecutivosVsUsuariosViewModel)
                    mobjVM.NombreView = Me.ToString

                    mobjVM.inicializar()

                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del control", Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Métodos para control de eventos"

    ''' <summary>
    ''' Controlador de eventos para seleccionar todos los check en la columna Seleccionar todo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub chkSeleccionarTodo_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim chk As CheckBox = CType(sender, CheckBox)
            Dim check As Boolean = chk.IsChecked.Value
            mobjVM.SeleccionarTodo(check)
        Catch ex As Exception
            mostrarMensaje("Error chkSeleccionarTodo_Click.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
        End Try
    End Sub

    Private Sub ctlConsecutivosVsUsuarios_itemAsignado(pstrIdItem As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If pstrIdItem = "LOGIN_USUARIOS" Then
                    'mobjVM.strUsuarioProceso = pobjItem.CodItem
                    mobjVM.strCondicionTipo = pobjItem.CodItem
                    If Not String.IsNullOrEmpty(pobjItem.Nombre) Then
                        mobjVM.strNombreUsuarioProceso = pobjItem.Nombre
                    Else
                        mobjVM.strNombreUsuarioProceso = pobjItem.CodItem
                    End If
                End If
            End If

        Catch ex As Exception
            mostrarMensaje("Error ctlConsecutivosVsUsuarios_itemAsignado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
        End Try
    End Sub

    Private Sub ctlCompanias_itemAsignado(pstrIdItem As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If pstrIdItem = "compania" Then
                    mobjVM.intCompanias = pobjItem.IdItem
                    mobjVM.strNombreCompañia = pobjItem.InfoAdicional02
                End If
            End If

        Catch ex As Exception
            mostrarMensaje("Error ctlCompanias_itemAsignado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
        End Try
    End Sub

    Private Sub ctlConsecutivos_itemAsignado(pstrIdItem As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If pstrIdItem = "ConsecutivosxCompania" Then
                    'mobjVM.strNombreConsecutivo = pobjItem.CodItem
                    mobjVM.strCondicionTipo = pobjItem.CodItem
                End If
            End If

        Catch ex As Exception
            mostrarMensaje("Error ctlConsecutivos_itemAsignado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
        End Try
    End Sub

    Private Sub btnLimpiarCia_Dataform_Edicion_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                mobjVM.strNombreCompañia = Nothing
                mobjVM.intCompanias = Nothing
            End If

        Catch ex As Exception
            mostrarMensaje("Error btnLimpiarCia_Dataform_Edicion_Click.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
        End Try
    End Sub



    Private Sub btnConsultar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.FiltrarDatos()
    End Sub

Private Sub TextBox_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            ctlConsecutivosVsUsuarios.AbrirBuscador()
        Catch ex As Exception
            mostrarMensaje("Error TextBox_MouseLeftButtonDown.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
        End Try
    End Sub
#End Region



    'Private Sub BuscadorGenerico_finalizoBusqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)

    'End Sub
End Class
