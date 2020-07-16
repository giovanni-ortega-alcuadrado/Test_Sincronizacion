Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls


Partial Public Class CobroUtilidadesView
    Inherits UserControl

    ''' <summary>
    ''' Eventos creados para la comunicación con las clases CobroUtilidadesView y CobroUtilidadesViewModel
    ''' Pantalla Procesar Portafolio (Calculos Financieros)
    ''' </summary>
    ''' <remarks>Jorge Peña (Alcuadrado S.A.) - 21 de Febrero 2014</remarks>
#Region "Variables"

    Private mobjVM As CobroUtilidadesViewModel
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

            Me.DataContext = New CobroUtilidadesViewModel
            Me.Resources.Add("ViewModelPrincipal", Me.DataContext)
            InitializeComponent()
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try
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

    Private Async Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            mobjVM = CType(Me.DataContext, CobroUtilidadesViewModel)
            mobjVM.NombreView = Me.ToString

            Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty)

            mobjVM.inicializar()
        End If
    End Sub

#End Region

#Region "Eventos de controles"

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

#Region "Eventos"

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim CerrarVentana As Boolean = True

            mobjVM.RealizarProceso()

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante el proceso.", Me.Name, "btnAceptar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiar_Click(sender As Object, e As RoutedEventArgs)
        Try
            mobjVM.LimpiarGrid()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar el grid.", Me.Name, "btnLimpiar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCliente_Dataform_Edicion_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                Me.mobjVM.BorrarCliente = True
                Me.mobjVM.lngIDComitente = Nothing
                Me.mobjVM.strNombreComitente = Nothing
                Me.mobjVM.BorrarCliente = False

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctrlCliente_comitente_Dataform_Edicion(pstrClaseControl As String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(mobjVM) And Not IsNothing(pobjComitente) Then
                Me.mobjVM.lngIDComitente = pobjComitente.CodigoOYD
                Me.mobjVM.strNombreComitente = pobjComitente.Nombre
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la asignación del comitente", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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

    Private Async Sub btnBuscar_Click(sender As Object, e As RoutedEventArgs) Handles btnBuscar.Click
        Try
            If Not IsNothing(mobjVM) Then

                Await Me.mobjVM.UtilidadesCustodiasConsultar()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnBuscar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub ControlRefrescarCache_EventoRefrescarCombos(sender As Object, e As RoutedEventArgs)

        mobjVM = CType(Me.DataContext, CobroUtilidadesViewModel)
        mobjVM.NombreView = Me.ToString
        Await mobjVM.RefresacarCache()

    End Sub

End Class
