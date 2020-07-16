Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class ProcesarArchivos
    Inherits UserControl
    Dim objVM As ViewModelProcesarArchivosDeceval


    Public Sub New()
        'Carga los Estilos de la aplicación de OYDPLUS
        'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

        Me.DataContext = New ViewModelProcesarArchivosDeceval
InitializeComponent()
        objVM = Me.LayoutRoot.DataContext
    End Sub

    Private Sub ProcesarArchivos_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            objVM = CType(Me.DataContext, ViewModelProcesarArchivosDeceval)
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario Procesar Archivos ", Me.Name, "", " ProcesarArchivos_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctrlCliente_comitenteAsignado(pstrIdComitente As System.String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(objVM) Then
                Me.objVM.ComitenteSeleccionado = pobjComitente
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctrlCliente_comitenteAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctlrEspecies_nemotecnicoAsignado(pstrNemotecnico As System.String, pstrNombreNemotecnico As System.String)
        Try
            If Not IsNothing(objVM) Then
                Me.objVM.Especie = pstrNemotecnico
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctlrEspecies_nemotecnicoAsignado", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ctlrEspecies_especieAsignada(pstrNemotecnico As System.String, pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(objVM) Then
                Me.objVM.NemotecnicoSeleccionado = pobjEspecie
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "ctlrEspecies_especieAsignada", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub SeleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        MyBase.OnGotFocus(e)
        If TypeOf sender Is TextBox Then
            CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
            'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
        End If
    End Sub

    Private Sub Procesar_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVM) Then
                objVM.ProcesarArchivoDeceval()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "Procesar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub


End Class
