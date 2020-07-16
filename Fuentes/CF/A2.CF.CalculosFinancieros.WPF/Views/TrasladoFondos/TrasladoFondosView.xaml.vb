Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class TrasladoFondosView
    Inherits UserControl
    Dim objVMTraslados As TrasladoFondosViewModel
    Private mlogInicializado As Boolean = False
    Private mlogErrorInicializando As Boolean = False

    Public Sub New()
        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

            Me.DataContext = New TrasladoFondosViewModel
InitializeComponent()
            AddHandler Me.Unloaded, AddressOf View_Unloaded
            AddHandler Me.SizeChanged, AddressOf CambioDePantalla
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar los componentes del formulario", Me.ToString(), "OperacionesOtrosNegociosView", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Sub

    Private Sub OtrosNegocios_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializado = False Then
                objVMTraslados = CType(Me.DataContext, TrasladoFondosViewModel)
                objVMTraslados.viewTrasladoFondos = Me

                'AddHandler Application.Current.Host.Content.Resized, AddressOf CambioDePantalla

                mlogInicializado = True
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario de otros negocios", Me.Name, "", "OtrosNegocios_Loaded", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    Private Sub CambioDePantalla(ByVal sender As Object, ByVal e As EventArgs)
       
    End Sub

    Private Sub View_Unloaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Me.Unloaded
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMTraslados) Then
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

    
    Private Async Sub BuscadorGenericoListaButon_finalizoBusqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMTraslados) Then
                If Not IsNothing(pobjItem) Then
                    If pstrClaseControl = "Compania" Then
                        objVMTraslados.IDCompania = CInt(pobjItem.IdItem)
                        objVMTraslados.NombreCompania = pobjItem.Nombre
                        Await objVMTraslados.ConsultarFechaDefectoCompania()
                    End If
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar la información de la compañia.", Me.Name, "txtCompania_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnLimpiarCompania_Click(sender As Object, e As RoutedEventArgs)
        objVMTraslados.LimpiarCompania()
    End Sub

Private Sub btnConsultar_Click(sender As Object, e As RoutedEventArgs)
        objVMTraslados.ConsultarTraslados()
    End Sub

Private Sub btnGenerar_Click(sender As Object, e As RoutedEventArgs)
        objVMTraslados.GenerarTraslados()
    End Sub

Private Sub txtCompania_LostFocus_1(sender As Object, e As RoutedEventArgs)
        Try
            'Para el timer de ordenes
            If Not IsNothing(objVMTraslados) Then
                If CType(sender, A2Utilidades.A2NumericBox).Value > 0 Then
                    objVMTraslados.ConsultarDatosCompania(CType(sender, A2Utilidades.A2NumericBox).Value.ToString)
                Else
                    objVMTraslados.LimpiarCompania()
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar la información de la compañia.", Me.Name, "txtCompania_LostFocus", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
End Class
