Imports Telerik.Windows.Controls

Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web



Partial Public Class OperacionesView
    Inherits UserControl

    ''' <summary>
    ''' Eventos creados para la comunicación con las clases OperacionesView y OperacionesViewModel
    ''' </summary>
    ''' <remarks>Jorge Peña (Alcuadrado S.A.) - Abril 22/2015</remarks>
#Region "Variables"

    Private mobjVM As OperacionesViewModel
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

        Me.DataContext = New OperacionesViewModel
InitializeComponent()
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then
                mlogInicializar = False
                ' Asociar el grid de edición y el data forma a la barra de herramientas que controla la edición
                cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1

                inicializar()

                CType(Me.DataContext, OperacionesViewModel).VmOperaciones = Me
            Else
                If Not CType(Me.DataContext, OperacionesViewModel).Editando And CType(Me.DataContext, OperacionesViewModel).visNavegando = "Visible" Then
                    CType(Me.DataContext, OperacionesViewModel).RefrescarForma()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del control", Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


    Private Async Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            mobjVM = CType(Me.DataContext, OperacionesViewModel)
            mobjVM.NombreView = Me.ToString

            Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty)

            Await mobjVM.inicializar()
        End If
    End Sub

#End Region

#Region "Eventos de controles"

    Private Sub cm_EventoRefrescarCombos(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoRefrescarCombos
        Try
            If Me.Resources.Contains("A2VM") Then
                mobjVM.IsBusy = True
                CType(Me.Resources("A2VM"), A2UtilsViewModel).EjecutaActualizacionCombos(Me.ToString)
                mobjVM.IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar los combos.",
                                 Me.ToString(), "cm_EventoRefrescarCombos", Application.Current.ToString(), Program.Maquina, ex)
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

    Private Sub BuscadorGenerico_finalizoBusqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, OperacionesViewModel).ReceptoresOrdenesSelected.strIDReceptor = pobjItem.IdItem
            CType(Me.DataContext, OperacionesViewModel).ReceptoresOrdenesSelected.strNombre = pobjItem.Nombre
        End If
    End Sub

    Private Sub ValidarLiquidacion(sender As Object, e As RoutedEventArgs)
        If CType(Me.DataContext, OperacionesViewModel).logEstadoRegistro = False Then
            Exit Sub
        End If

        CType(Me.DataContext, OperacionesViewModel).ValidaLiq()
    End Sub

    Private Sub CalcularValores_LostFocus(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, OperacionesViewModel).CalcularValoresVM()
    End Sub

    Private Sub dgReceptoresOrdenes_BindingValidationError(ByVal sender As Object, ByVal e As System.Windows.Controls.ValidationErrorEventArgs)
        e.Handled = True
    End Sub

    Private Sub btnLimpiarCliente_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        Try
            If Not IsNothing(mobjVM) Then
                Me.mobjVM.BorrarCliente = True
                Me.mobjVM.cb.lngIDComitente = Nothing
                Me.mobjVM.cb.ComitenteSeleccionado = Nothing
                Me.mobjVM.BorrarCliente = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "btnLimpiarCliente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorGenerico_finalizoBusquedaPLAZA(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, OperacionesViewModel).EncabezadoSeleccionado.lngIDComisionistaOtraPlaza = CType(pobjItem.IdItem, Integer?)
        End If
    End Sub
    Private Sub btnAplazar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.Aplazar()
    End Sub

Private Sub btnDuplicar_Click(sender As Object, e As RoutedEventArgs)
        mobjVM.Duplicar()
    End Sub

Private Sub BuscadorGenerico_finalizoBusquedaPLAZALocal(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            CType(Me.DataContext, OperacionesViewModel).EncabezadoSeleccionado.lngIDComisionistaLocal = CType(pobjItem.IdItem, Integer?)
        End If
    End Sub

    ' ''' <summary>
    ' ''' Se ejecuta cuando se dispara el evento comitenteAsignado en el buscador de clientes (control buscador clientes lista)
    ' ''' </summary>
    ' ''' <param name="pstrClaseControl">Permite identificar el llamado</param>
    ' ''' <param name="pobjComitente">Datos del comitente seleccionado en el buscador</param>
    ' ''' <remarks></remarks>
    Private Sub BuscadorClienteListaButon_comitenteAsignado(pstrClaseControl As String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(mobjVM) And Not IsNothing(pobjComitente) Then
                Me.mobjVM.cb.ComitenteSeleccionado = pobjComitente
                Me.mobjVM.cb.lngIDComitente = pobjComitente.CodigoOYD
                CType(Me.DataContext, OperacionesViewModel).CambioItem("cb")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la asignación del comitente", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
End Class

