Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades


Partial Public Class ReportesOrdenesPLUSView
    Inherits UserControl
    Dim objVM As ReporteOrdenesPLUSViewModel

#Region "Variables"


    
#End Region

#Region "Selección de Compañía"

   

#End Region

#Region "Inicializacion"

    Public Sub New()
        Try
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try
        '---------------------------------------------------------------------------------------------------------------------------------------------------
        ' Validar si ya existe una compañía eleccionada o si se debe cargar la pantalla que pide al usuario la compañía seleccionada
        '---------------------------------------------------------------------------------------------------------------------------------------------------



        Me.DataContext = New ReporteOrdenesPLUSViewModel
InitializeComponent()
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If objVM Is Nothing Then
                objVM = CType(Me.DataContext, ReporteOrdenesPLUSViewModel)

                inicializar()
            End If


        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del control", Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            objVM = CType(Me.DataContext, ReporteOrdenesPLUSViewModel)
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

   
    Private Sub ctrlCliente_comitenteAsignado(ByVal pstrIdComitente As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjComitente) Then
                If Not IsNothing(pobjComitente) Then
                    Me.objVM.Comitente = pobjComitente.CodigoOYD
                End If
            End If

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "BuscadorGenericoListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctlrEspecies_especieAsignada(pstrNemotecnico As System.String, pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(pstrNemotecnico) And Not IsNothing(pobjEspecie) Then
                objVM.Especie = pobjEspecie.Nemotecnico
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctlrEspecies_especieAsignada", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

     
    Private Sub ctlrEspecies_especieAsignada2(pstrNemotecnico As String, pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(pstrNemotecnico) And Not IsNothing(pobjEspecie) Then
                objVM.Especie2 = pobjEspecie.Nemotecnico
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctlrEspecies_especieAsignada2", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            If Not IsNothing(objVM) Then 
                objVM.CodigoReceptor = pobjItem.CodItem
                objVM.NombreReceptor = pobjItem.Nombre 
            End If
        End If


    End Sub

    Private Sub btnLimpiarReceptor_Click_1(sender As Object, e As RoutedEventArgs)
        If Not IsNothing(objVM) Then
            objVM.CodigoReceptor = GSTR_ID_TODOS
            objVM.NombreReceptor = GSTR_TODOS
        End If

    End Sub
Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        objVM.VerReporte()
    End Sub

    Private Sub Button_Click_1(sender As Object, e As RoutedEventArgs)
        objVM.GenerarExcel()
    End Sub
End Class

