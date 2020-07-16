Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web
Imports Microsoft.VisualBasic.CompilerServices
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Globalization

'
Partial Public Class OrdenPago_DetalleOperacionesEspecialesView
    Inherits Window
    Dim vm As OrdenPago_DetalleOperacionesEspecialesViewModel

    Public Event OrdenPago_FinalizoGuardarRegistro(ByVal pintIDDetalle As Integer,
                                                   ByVal pintIDEncabezado As Integer,
                                                   ByVal plogGuardarYSalir As Boolean,
                                                   ByVal pTipoOperacionEspecial As String,
                                                   ByVal pDescripcionTipoOperacionEspecial As String,
                                                   ByVal pOperacionEspecial As String,
                                                   ByVal pDescripcionOperacionEspecial As String,
                                                   ByVal pCodigoOYDComprador As String,
                                                   ByVal pNombreComprador As String,
                                                   ByVal pTipoDocumentoComprador As String,
                                                   ByVal pDescripcionTipoDocumentoComprador As String,
                                                   ByVal pNroDocumentoComprador As String,
                                                   ByVal pCodigoOYDVendedor As String,
                                                   ByVal pNombreVendedor As String,
                                                   ByVal pTipoDocumentoVendedor As String,
                                                   ByVal pDescripcionTipoDocumentoVendedor As String,
                                                   ByVal pNroDocumentoVendedor As String,
                                                   ByVal pProvieneDinero As String,
                                                   ByVal pObservaciones As String,
                                                   ByVal pValor As Double)
    Public Event OrdenPago_CancelarGuardarRegistro(ByVal pintIDDetalle As Integer,
                                                   ByVal pintIDEncabezado As Integer)

    ''' <summary>
    ''' Sobrecarga creada para la creación de un nuevo registro desde la pantalla de ordenes de pago
    ''' </summary>
    ''' <param name="pintIDDetalle"></param>
    ''' <param name="pintIDEncabezado"></param>
    ''' <param name="pCarteraColectivaFondos"></param>
    ''' <param name="pNroEncargoFondos"></param>
    ''' <param name="pReceptor"></param>
    ''' <param name="pIDComitente"></param>
    ''' <param name="pNroDocumentoComitente"></param>
    ''' <param name="pTipoDocumentoComitente"></param>
    ''' <param name="pNombreComitente"></param>
    ''' <param name="pTipoProducto"></param>
    ''' <param name="pTipoRetiroFondos"></param>
    ''' <param name="pValorNetoOrden"></param>
    ''' <param name="pValorEdicionDetalle"></param>
    ''' <param name="pHabilitarValor"></param>
    ''' <param name="pTieneOrdenEnCero"></param>
    Public Sub New(ByVal pintIDDetalle As Integer,
                   ByVal pintIDEncabezado As Integer,
                   ByVal pCarteraColectivaFondos As String,
                   ByVal pNroEncargoFondos As String,
                   ByVal pReceptor As String,
                   ByVal pIDComitente As String,
                   ByVal pNroDocumentoComitente As String,
                   ByVal pTipoDocumentoComitente As String,
                   ByVal pNombreComitente As String,
                   ByVal pTipoProducto As String,
                   ByVal pTipoRetiroFondos As String,
                   ByVal pValorNetoOrden As Double,
                   ByVal pValorEdicionDetalle As Double,
                   ByVal pHabilitarValor As Boolean,
                   ByVal pTieneOrdenEnCero As Boolean)
        Try
            vm = New OrdenPago_DetalleOperacionesEspecialesViewModel
            InitializeComponent()

            vm.EsNuevoRegistro = True
            Me.DataContext = vm

            vm.logCambiarPropiedades = False

            vm.IDEncabezado = pintIDEncabezado
            vm.IDDetalle = pintIDDetalle
            vm.CarteraColectivaFondos = pCarteraColectivaFondos
            vm.NroEncargoFondos = pNroEncargoFondos
            vm.Receptor = pReceptor
            vm.IDComitente = pIDComitente
            vm.NroDocumentoComitente = pNroDocumentoComitente
            vm.TipoDocumentoComitente = pTipoDocumentoComitente
            vm.NombreComitente = pNombreComitente
            vm.TipoProducto = pTipoProducto
            vm.TipoRetiroFondos = pTipoRetiroFondos
            vm.ValorNetoOrden = pValorNetoOrden
            vm.ValorEdicionDetalle = pValorEdicionDetalle
            vm.HabilitarValor_Inicio = pHabilitarValor
            vm.TieneOrdenEnCero = pTieneOrdenEnCero

            vm.logCambiarPropiedades = True

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model wppFrmCheque", Me.Name, "New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
    ''' <summary>
    ''' Sobrecarga para realizar la edición de un registro
    ''' </summary>
    ''' <param name="pintIDDetalle"></param>
    ''' <param name="pintIDEncabezado"></param>
    ''' <param name="pCarteraColectivaFondos"></param>
    ''' <param name="pNroEncargoFondos"></param>
    ''' <param name="pReceptor"></param>
    ''' <param name="pIDComitente"></param>
    ''' <param name="pNroDocumentoComitente"></param>
    ''' <param name="pTipoDocumentoComitente"></param>
    ''' <param name="pNombreComitente"></param>
    ''' <param name="pTipoProducto"></param>
    ''' <param name="pTipoRetiroFondos"></param>
    ''' <param name="pValorNetoOrden"></param>
    ''' <param name="pValorEdicionDetalle"></param>
    ''' <param name="pValorOriginalDetalle"></param>
    ''' <param name="pTipoOperacionEspecial"></param>
    ''' <param name="pOperacionEspecial"></param>
    ''' <param name="pCodigoOYDComprador"></param>
    ''' <param name="pNombreComprador"></param>
    ''' <param name="pTipoDocumentoComprador"></param>
    ''' <param name="pNroDocumentoComprador"></param>
    ''' <param name="pCodigoOYDVendedor"></param>
    ''' <param name="pNombreVendedor"></param>
    ''' <param name="pTipoDocumentoVendedor"></param>
    ''' <param name="pNroDocumentoVendedor"></param>
    ''' <param name="pProvieneDinero"></param>
    ''' <param name="pObservaciones"></param>
    ''' <param name="pValor"></param>
    ''' <param name="pHabilitarValor"></param>
    ''' <param name="pTieneOrdenEnCero"></param>
    Public Sub New(ByVal pintIDDetalle As Integer,
                   ByVal pintIDEncabezado As Integer,
                   ByVal pCarteraColectivaFondos As String,
                   ByVal pNroEncargoFondos As String,
                   ByVal pReceptor As String,
                   ByVal pIDComitente As String,
                   ByVal pNroDocumentoComitente As String,
                   ByVal pTipoDocumentoComitente As String,
                   ByVal pNombreComitente As String,
                   ByVal pTipoProducto As String,
                   ByVal pTipoRetiroFondos As String,
                   ByVal pValorNetoOrden As Double,
                   ByVal pValorEdicionDetalle As Double,
                   ByVal pValorOriginalDetalle As Double,
                   ByVal pTipoOperacionEspecial As String,
                   ByVal pOperacionEspecial As String,
                   ByVal pCodigoOYDComprador As String,
                   ByVal pNombreComprador As String,
                   ByVal pTipoDocumentoComprador As String,
                   ByVal pNroDocumentoComprador As String,
                   ByVal pCodigoOYDVendedor As String,
                   ByVal pNombreVendedor As String,
                   ByVal pTipoDocumentoVendedor As String,
                   ByVal pNroDocumentoVendedor As String,
                   ByVal pProvieneDinero As String,
                   ByVal pObservaciones As String,
                   ByVal pValor As Double,
                   ByVal pHabilitarValor As Boolean,
                   ByVal pTieneOrdenEnCero As Boolean)
        Try
            vm = New OrdenPago_DetalleOperacionesEspecialesViewModel
            InitializeComponent()

            vm.EsEdicionRegistro = True
            Me.DataContext = vm

            vm.logCambiarPropiedades = False

            vm.IDEncabezado = pintIDEncabezado
            vm.IDDetalle = pintIDDetalle
            vm.CarteraColectivaFondos = pCarteraColectivaFondos
            vm.NroEncargoFondos = pNroEncargoFondos
            vm.Receptor = pReceptor
            vm.IDComitente = pIDComitente
            vm.NroDocumentoComitente = pNroDocumentoComitente
            vm.TipoDocumentoComitente = pTipoDocumentoComitente
            vm.NombreComitente = pNombreComitente
            vm.TipoProducto = pTipoProducto
            vm.ValorNetoOrden = pValorNetoOrden
            vm.ValorOriginalDetalle = pValorOriginalDetalle
            vm.ValorEdicionDetalle = pValorEdicionDetalle
            vm.Tipo = pTipoOperacionEspecial
            vm.OperacionEspecial = pOperacionEspecial
            vm.CodigoOYDComprador = pCodigoOYDComprador
            vm.NombreComprador = pNombreComprador
            vm.TipoDocumentoComprador = pTipoDocumentoComprador
            vm.NroDocumentoComprador = pNroDocumentoComprador
            vm.CodigoOYDVendedor = pCodigoOYDVendedor
            vm.NombreVendedor = pNombreVendedor
            vm.TipoDocumentoVendedor = pTipoDocumentoVendedor
            vm.NroDocumentoVendedor = pNroDocumentoVendedor
            vm.ProvieneDinero = pProvieneDinero
            vm.Observaciones = pObservaciones
            vm.ValorGenerar = pValor

            vm.HabilitarValor_Inicio = pHabilitarValor
            vm.TieneOrdenEnCero = pTieneOrdenEnCero

            vm.logCambiarPropiedades = True

            vm.TipoRetiroFondos = pTipoRetiroFondos

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model wppFrmCheque", Me.Name, "New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub OrdenPago_DetalleOperacionesEspecialesView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            vm.CargarCombosOYDPLUS("INICIO")
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario", Me.Name, "OrdenPago_DetalleOperacionesEspecialesView_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub OrdenPago_DetalleOperacionesEspecialesView_Unloaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Unloaded
        Try
            If Not IsNothing(vm) Then
                vm.CancelarEdicionRegistro()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cerrar el formulario", Me.Name, "OrdenPago_DetalleOperacionesEspecialesView_Unloaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtCampoTexto_TextChanged_1(sender As Object, e As TextChangedEventArgs)
        Try
            If Not IsNothing(sender) Then
                Dim objTextBox As TextBox = CType(sender, TextBox)
                If Not String.IsNullOrEmpty(objTextBox.Text) Then
                    Dim objValidacion = clsExpresiones.ValidarCaracteresEnCadena(objTextBox.Text, clsExpresiones.TipoExpresion.Caracteres)
                    If Not IsNothing(objValidacion) Then
                        If objValidacion.TextoValido = False Then
                            objTextBox.Text = objValidacion.CadenaNueva
                            objTextBox.Select(objValidacion.PosicionPrimerInvalido, 0)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el texto.",
                                 Me.ToString(), "txtConcepto_TextChanged_1", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub Buscador_finalizoBusquedaClientes(pstrClaseControl As System.String, pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjComitente) Then
                If pstrClaseControl = "IDComitenteComprador" Then
                    vm.CodigoOYDComprador = pobjComitente.CodigoOYD
                    vm.NroDocumentoComprador = pobjComitente.NroDocumento
                    vm.TipoDocumentoComprador = pobjComitente.CodTipoIdentificacion
                    vm.NombreComprador = pobjComitente.NombreCodigoOYD
                Else
                    vm.CodigoOYDVendedor = pobjComitente.CodigoOYD
                    vm.NroDocumentoVendedor = pobjComitente.NroDocumento
                    vm.TipoDocumentoVendedor = pobjComitente.CodTipoIdentificacion
                    vm.NombreVendedor = pobjComitente.NombreCodigoOYD
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al finalizar la busqueda de clientes",
                                 Me.ToString(), "Buscador_finalizoBusquedaClientes", Application.Current.ToString(), Program.Maquina, ex)
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

    Private Sub TextBlockComprador_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        ctlBuscadorComprador.AbrirBuscador()
    End Sub

    Private Sub btnLimpiarCodigoComprador_Click(sender As Object, e As RoutedEventArgs)
        Try
            vm.CodigoOYDComprador = String.Empty
            vm.NroDocumentoComprador = String.Empty
            vm.TipoDocumentoComprador = String.Empty
            vm.NombreComprador = String.Empty
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar el texto.",
                                 Me.ToString(), "btnLimpiarConcepto_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TextBlockVendedor_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        ctlBuscadorVendedor.AbrirBuscador()
    End Sub

    Private Sub btnLimpiarCodigoVendedor_Click(sender As Object, e As RoutedEventArgs)
        Try
            vm.CodigoOYDVendedor = String.Empty
            vm.NroDocumentoVendedor = String.Empty
            vm.TipoDocumentoVendedor = String.Empty
            vm.NombreVendedor = String.Empty
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar el texto.",
                                 Me.ToString(), "btnLimpiarConcepto_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub BtnGuardarSalir_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim logGuardoExitoso As Boolean = vm.GuardarRegistro()
            If logGuardoExitoso Then
                Me.Close()
                NotificarGuardadoRegistro(True)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al guardar el registro.",
                                 Me.ToString(), "BtnGuardarSalir_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub BtnGuardarContinuar_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim logGuardoExitoso As Boolean = vm.GuardarRegistro()
            If logGuardoExitoso Then
                Me.Close()
                NotificarGuardadoRegistro(False)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al guardar el registro.",
                                 Me.ToString(), "BtnGuardarContinuar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub BtnCancelar_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.Close()
            RaiseEvent OrdenPago_CancelarGuardarRegistro(vm.IDDetalle, vm.IDEncabezado)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al guardar el registro.",
                                 Me.ToString(), "BtnCancelar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub NotificarGuardadoRegistro(ByVal plogGuardarYSalir As Boolean)
        RaiseEvent OrdenPago_FinalizoGuardarRegistro(vm.IDDetalle,
                                                     vm.IDEncabezado,
                                                     plogGuardarYSalir,
                                                     vm.Tipo,
                                                     vm.DescripcionTipo,
                                                     vm.OperacionEspecial,
                                                     vm.DescripcionOperacionEspecial,
                                                     vm.CodigoOYDComprador,
                                                     vm.NombreComprador,
                                                     vm.TipoDocumentoComprador,
                                                     vm.DescripcionTipoDocumentoComprador,
                                                     vm.NroDocumentoComprador,
                                                     vm.CodigoOYDVendedor,
                                                     vm.NombreVendedor,
                                                     vm.TipoDocumentoVendedor,
                                                     vm.DescripcionTipoDocumentoVendedor,
                                                     vm.NroDocumentoVendedor,
                                                     vm.ProvieneDinero,
                                                     vm.Observaciones,
                                                     vm.ValorGenerar)
    End Sub
End Class


