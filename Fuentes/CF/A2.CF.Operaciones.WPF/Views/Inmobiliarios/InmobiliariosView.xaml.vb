Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class InmobiliariosView
    Inherits UserControl
    Dim objVMInmuebles As InmobiliariosViewModel
    Private mlogInicializado As Boolean = False
    Private mlogErrorInicializando As Boolean = False

    Public Sub New()
        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            Me.DataContext = New InmobiliariosViewModel
InitializeComponent()
            AddHandler Me.Unloaded, AddressOf View_Unloaded
            'AddHandler Me.SizeChanged, AddressOf CambioDePantalla

            'Me.stackBotones.Width = Application.Current.MainWindow.ActualWidth * 0.98
            'Me.ScrollForma.Width = Application.Current.MainWindow.ActualWidth * 0.98
            'Me.ScrollForma.Height = Application.Current.MainWindow.ActualHeight - 185
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar los componentes del formulario", Me.ToString(), "OperacionesOtrosNegociosView", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    Private Async Sub OtrosNegocios_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializado = False Then
                cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1

                objVMInmuebles = CType(Me.DataContext, InmobiliariosViewModel)

                objVMInmuebles.HabilitarDuplicar = True

                objVMInmuebles.ViewInmuebles = Me
                objVMInmuebles.visNavegando = "Collapse"
                'AddHandler Application.Current.Host.Content.Resized, AddressOf CambioDePantalla

                Await objVMInmuebles.inicializar()

                mlogInicializado = True
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de otros negocios", Me.Name, "", "OtrosNegocios_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
        'Me.stackBotones.Width = Application.Current.MainWindow.ActualWidth * 0.98
        'Me.ScrollForma.Width = Application.Current.MainWindow.ActualWidth * 0.98
        'Me.ScrollForma.Height = Application.Current.MainWindow.ActualHeight - 185
    End Sub

    Private Sub View_Unloaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Me.Unloaded
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMInmuebles) Then
                Me.objVMInmuebles.pararTemporizador()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar cerrar la pantalla de operaciones.", Me.Name, "View_Unloaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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

    Private Sub btnDuplicar_Click(sender As Object, e As RoutedEventArgs)
        Me.objVMInmuebles.DuplicarRegistro()
    End Sub

Private Sub btnRefrescarPantalla_Click(sender As Object, e As RoutedEventArgs)
        Me.objVMInmuebles.RecargarPantalla()
    End Sub

Private Sub BuscadorClienteListaButon_finalizoBusqueda(pstrClaseControl As String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMInmuebles) Then
                Me.objVMInmuebles.BusquedaInmueble.IDComitente = pobjComitente.IdComitente
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el cliente.", Me.Name, "BuscadorClienteListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

End Class
