Imports Telerik.Windows.Controls
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes


Partial Public Class PermisosExportacionFormatosView
    Inherits UserControl

    ''' <summary>
    ''' Eventos creados para la comunicación con las clases PermisosExportacionFormatosView y PermisosExportacionFormatosViewModel
    ''' Pantalla Permisos a exportación de formatos
    ''' </summary>
    ''' <remarks>Jhonatan Arley Acevedo - Abril 09/2015(Alcuadrado S.A.) - Abril 09/2015</remarks>
#Region "Variables"

    Dim mobjVM As PermisosExportacionFormatosViewModel
    Private mlogInicializar As Boolean = True

#End Region

#Region "Inicializacion"

    ''' <summary>
    ''' Inicializa los estilos metro para la pantalla permisos a exportación de formatos
    ''' </summary>
    ''' <history>
    ''' Creado por   : Jhonatan Arley Acevedo Martínez
    ''' Fecha        : Abril 09/2015
    ''' Pruebas CB   : Jhonatan Arley Acevedo Martínez Osorio - Abril 09/2015 - Resultado OK
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

        Me.DataContext = New PermisosExportacionFormatosViewModel
        Me.Resources.Add("ViewModelPrincipal", Me.DataContext)
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
                    mobjVM = CType(Me.DataContext, PermisosExportacionFormatosViewModel)
                    mobjVM.NombreView = Me.ToString
                    mobjVM.viewAsignarPemrisos = Me

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

    Private Sub cltBuscadorUsuarios_itemAsignado(pstrIdItem As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If pstrIdItem = "LOGIN_USUARIOS" Then
                    mobjVM.strLoginUsuario = pobjItem.CodItem
                End If
            End If

        Catch ex As Exception
            mostrarMensaje("Error cltBuscadorUsuarios_itemAsignado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
        End Try
    End Sub

    Private Sub cltBuscadorTipo_itemAsignado(pstrIdItem As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If pstrIdItem = "ASIGNARPERMISOS_OBJETOS" Then
                    mobjVM.strBuscadorTipo = pobjItem.CodItem
                End If
            End If

        Catch ex As Exception
            mostrarMensaje("Error cltBuscadorTipo_itemAsignado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
        End Try
    End Sub

    Private Sub TextBoxLogin_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            ctlLoginUsuario.AbrirBuscador()
        Catch ex As Exception
            mostrarMensaje("Error TextBoxLogin_MouseLeftButtonDown.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
        End Try
    End Sub

    Private Sub TextBoxTipo_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            ctlTipo.AbrirBuscador()
        Catch ex As Exception
            mostrarMensaje("Error TextBoxTipo_MouseLeftButtonDown.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
        End Try
    End Sub
#End Region

    Private Sub txtFiltro_KeyUp(sender As System.Object, e As System.Windows.Input.KeyEventArgs)
        Try
            If e.Key = Key.Enter Then
                mobjVM.FiltrarInformacion(Me.txtFiltro.Text)
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al filtrar la información.", Me.Name, "acbEspecies_KeyUp", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtFiltro_KeyDown(sender As System.Object, e As System.Windows.Input.KeyEventArgs)
        Try
            If e.Key = Key.Tab Then
                mobjVM.FiltrarInformacion(Me.txtFiltro.Text)
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al filtrar la información.", Me.Name, "txtFiltro_KeyDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnFiltro_Click(sender As Object, e As RoutedEventArgs)
        Try
            mobjVM.FiltrarInformacion(Me.txtFiltro.Text)
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al filtrar la información.", Me.Name, "btnFiltro_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnGrabar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.GrabarPermisosAsignados()
    End Sub

Private Sub btnLimpiarFiltro_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.txtFiltro.Text = String.Empty
            mobjVM.FiltrarInformacion(Me.txtFiltro.Text)
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al filtrar la información.", Me.Name, "btnLimpiarFiltro_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

End Class
