Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls

Partial Public Class ProcesarPortafolioView
    Inherits UserControl


    ''' <summary>
    ''' Eventos creados para la comunicación con las clases ProcesarPortafolioView y ProcesarPortafolioViewModel
    ''' Pantalla Procesar Portafolio (Calculos Financieros)
    ''' </summary>
    ''' <remarks>Jorge Peña (Alcuadrado S.A.) - 21 de Febrero 2014</remarks>
#Region "Variables"

    Private mobjVM As ProcesarPortafolioViewModel
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

        Me.DataContext = New ProcesarPortafolioViewModel
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

    Private Async Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            mobjVM = CType(Me.DataContext, ProcesarPortafolioViewModel)
            mobjVM.NombreView = Me.ToString
            mobjVM.ViewProcesarPortafolio = Me

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

#Region "Eventos para asignar cliente y especie"

    ' ''' <summary>
    ' ''' Se ejecuta cuando se dispara el evento comitenteAsignado en el buscador de clientes (control buscador clientes lista)
    ' ''' </summary>
    ' ''' <param name="pstrClaseControl">Permite identificar el llamado</param>
    ' ''' <param name="pobjComitente">Datos del comitente seleccionado en el buscador</param>
    ' ''' <remarks></remarks>
    Private Sub ctrlCliente_comitenteAsignado(pstrIdComitente As System.String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(mobjVM) And Not IsNothing(pobjComitente) Then
                Me.mobjVM.lngIDComitente = pobjComitente.CodigoOYD
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la asignación del comitente", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    'Private Sub ctrlEspecie_nemotecnicoAsignado(pstrNemotecnico As System.String, pstrNombreNemotecnico As System.String)
    '    Try
    '        If Not IsNothing(mobjVM) Then
    '            Me.mobjVM.strIdEspecie = pstrNemotecnico
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la asignación del nemotécnico", Me.Name, "ctrlEspecie_nemotecnicoAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
    '    End Try
    'End Sub

    Private Sub ctrlEspecie_nemotecnicoAsignado(pstrClaseControl As String, pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(pobjEspecie) Then
                Select Case pstrClaseControl.ToLower()
                    Case "nemotecnico"
                        Me.mobjVM.strIdEspecie = pobjEspecie.Nemotecnico
                    Case "nemotecnicobuscar"
                        'Me.mobjVM.cb.NemotecnicoSeleccionado = pobjEspecie
                        'Me.mobjVM.cb.Nemotecnico = pobjEspecie.Nemotecnico
                        Me.mobjVM.CambioItem("cb")
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la asignación del nemotécnico", Me.Name, "ctrlEspecie_nemotecnicoAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCliente_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
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

    Private Sub btnLimpiarEspecie_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                Me.mobjVM.BorrarEspecie = True
                Me.mobjVM.strIdEspecie = Nothing
                Me.mobjVM.BorrarEspecie = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarEspecie_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub IDComitente_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            If Not String.IsNullOrEmpty(IDComitente.Text) Then
                Await Me.mobjVM.ConsultarDatosPortafolio(IDComitente.Text)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la asignación del comitente", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Manejadores error"

    Private Sub btnProcesar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ProcesarValoracion()
    End Sub

Private Sub btnInformeValoracion_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ExportarInformeValoracion()
    End Sub

Private Sub btnEliminar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.EliminarCalculos()
    End Sub

Private Sub btnRefrescarConsultarInformeProcesamiento_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ConsultarInformeProcesamiento()
    End Sub

Private Sub btnExportarExcel_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.ExportarAvanceProcesamiento()
    End Sub

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

