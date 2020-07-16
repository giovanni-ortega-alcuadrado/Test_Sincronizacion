Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web
Imports Microsoft.VisualBasic.CompilerServices
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Globalization

'
Partial Public Class DetalleInmobiliarioView
    Inherits Window
    Dim objVM As InmobiliariosViewModel

    Public Sub New(ByVal pobjVM As InmobiliariosViewModel)
        Try
            'Carga los Estilos de la aplicación de OYDPLUS
            'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)
            objVM = pobjVM
            Me.Resources.Add("VMInmuebles", objVM)
            
InitializeComponent()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model DetalleInmobiliarioView", Me.Name, "New(Sobrecargado)", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ChildWindow_Loaded(sender As System.Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            Me.DataContext = objVM
            Me.dtmFechaMovimiento.Focus()
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model DetalleInmobiliarioView", Me.Name, "ChildWindow_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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

    Private Sub txtConcepto_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            ctlConcepto.AbrirBuscador()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtConcepto_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(objVM) Then
                If Not IsNothing(pobjItem) Then
                    If pstrClaseControl.ToUpper = "DETALLECONCEPTO" Then
                        objVM.DetalleSeleccionado.IDConcepto = pobjItem.IdItem
                        objVM.DetalleSeleccionado.Concepto = pobjItem.Nombre
                        objVM.DetalleSeleccionado.TipoMovimiento = pobjItem.Descripcion.ToUpper
                        txtDetalle.Focus()
                    End If
                Else
                    If pstrClaseControl.ToUpper = "DETALLECONCEPTO" Then
                        ctlConcepto.Focus()
                    End If
                End If
            End If

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "BuscadorGenericoListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnGrabarYSalir_Click(sender As Object, e As RoutedEventArgs)
        Try
            objVM.Detalle_ActualizarRegistro()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar guardar el registro.", Me.ToString, "btnGrabarYSalir_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnGrabarYContinuar_Click(sender As Object, e As RoutedEventArgs)
        Try
            objVM.Detalle_ActualizarRegistro()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar abrir el buscador.", Me.ToString, "txtConcepto_MouseLeftButtonDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub

    Private Sub btnAnterior_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVM.DetalleSeleccionado) Then
                If objVM.ListaDetalle.Count > 1 Then
                    Dim intOrdenActual As Integer = objVM.DetalleSeleccionado.Orden
                    If objVM.ListaDetalle.Where(Function(o) o.Orden < intOrdenActual).Count > 0 Then
                        Dim intOrden As Integer = objVM.ListaDetalle.Where(Function(o) o.Orden < intOrdenActual).Max(Function(i) i.Orden).Value
                        objVM.DetalleSeleccionado = objVM.ListaDetalle.Where(Function(o) o.Orden = intOrden).First
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar buscar los registros.", Me.ToString, "btnAnterior_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnSiguiente_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(objVM.DetalleSeleccionado) Then
                If objVM.ListaDetalle.Count > 1 Then
                    Dim intOrdenActual As Integer = objVM.DetalleSeleccionado.Orden
                    If objVM.ListaDetalle.Where(Function(o) o.Orden > intOrdenActual).Count > 0 Then
                        Dim intOrden As Integer = objVM.ListaDetalle.Where(Function(o) o.Orden > intOrdenActual).Min(Function(i) i.Orden).Value
                        objVM.DetalleSeleccionado = objVM.ListaDetalle.Where(Function(o) o.Orden = intOrden).First
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar buscar los registros.", Me.ToString, "btnSiguiente_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub btnBorrar_Click(sender As Object, e As RoutedEventArgs)
        Try
            objVM.Detalle_BorrarRegistro()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar borrar los registros.", Me.ToString, "btnBorrar_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
End Class


