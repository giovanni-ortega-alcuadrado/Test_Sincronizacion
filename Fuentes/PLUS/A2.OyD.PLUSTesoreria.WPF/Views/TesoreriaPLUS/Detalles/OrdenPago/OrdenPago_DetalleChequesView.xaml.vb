Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web
Imports Microsoft.VisualBasic.CompilerServices
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Globalization

'
Partial Public Class OrdenPago_DetalleChequesView
    Inherits Window
    Dim vm As OrdenPago_DetalleChequesViewModel

    Public Event OrdenPago_FinalizoGuardarRegistro(ByVal pintIDDetalle As Integer,
                                                   ByVal pintIDEncabezado As Integer,
                                                   ByVal plogGuardarYSalir As Boolean,
                                                   ByVal pTipoCliente As String,
                                                   ByVal pEsTercero As Boolean,
                                                   ByVal pDescripcionTipoCliente As String,
                                                   ByVal pNombreBeneficiario As String,
                                                   ByVal pTipoIdentificacion As String,
                                                   ByVal pDescripcionTipoIdentificacion As String,
                                                   ByVal pNroDocumento As String,
                                                   ByVal pTipoCheque As String,
                                                   ByVal pDescripcionTipoCheque As String,
                                                   ByVal pTipoCruce As String,
                                                   ByVal pDescripcionTipoCruce As String,
                                                   ByVal pIDConcepto As Nullable(Of Integer),
                                                   ByVal pDescripcionConcepto As String,
                                                   ByVal pDetalleConcepto As String,
                                                   ByVal pTipoGMF As String,
                                                   ByVal pDescripcionTipoGMF As String,
                                                   ByVal pValorGenerar As Double,
                                                   ByVal pValorGMF As Double,
                                                   ByVal pValorNeto As Double,
                                                   ByVal pValorSaldoConsultado As Double,
                                                   ByVal pLiquidacionesSeleccionadas As String)
    Public Event OrdenPago_CancelarGuardarRegistro(ByVal pintIDDetalle As Integer,
                                                   ByVal pintIDEncabezado As Integer)
    Public Event OrdenPago_FinalizoGuardarRegistro_BaseDatos(ByVal pintIDDetalle As Integer,
                                                   ByVal pintIDEncabezado As Integer)

    ''' <summary>
    ''' Sobrecarga creada para consultar directamente del tesorero
    ''' </summary>
    ''' <param name="pintIDDetalle"></param>
    ''' <param name="pintIDEncabezado"></param>
    Public Sub New(ByVal pintIDDetalle As Integer,
                   ByVal pintIDEncabezado As Integer)
        Try
            vm = New OrdenPago_DetalleChequesViewModel
            InitializeComponent()

            Me.DataContext = vm

            vm.logCambiarPropiedades = False

            vm.ConsultarValoresBaseDatos = True
            vm.IDEncabezado = pintIDEncabezado
            vm.IDDetalle = pintIDDetalle
            vm.HabilitarGuardarContinuar = False
            vm.HabilitarValor = False

            vm.logCambiarPropiedades = True

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model wppFrmCheque", Me.Name, "New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub
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
            vm = New OrdenPago_DetalleChequesViewModel
            InitializeComponent()

            vm.EsNuevoRegistro = True
            Me.DataContext = vm

            vm.logCambiarPropiedades = False

            vm.ConsultarValoresBaseDatos = False
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
    ''' <param name="pTipoCliente"></param>
    ''' <param name="pNombreBeneficiario"></param>
    ''' <param name="pTipoIdentificacion"></param>
    ''' <param name="pNroDocumento"></param>
    ''' <param name="pTipoCheque"></param>
    ''' <param name="pTipoCruce"></param>
    ''' <param name="pIDConcepto"></param>
    ''' <param name="pDetalleConcepto"></param>
    ''' <param name="pTipoGMF"></param>
    ''' <param name="pValorGenerar"></param>
    ''' <param name="pValorGMF"></param>
    ''' <param name="pValorNeto"></param>
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
                   ByVal pTipoCliente As String,
                   ByVal pNombreBeneficiario As String,
                   ByVal pTipoIdentificacion As String,
                   ByVal pNroDocumento As String,
                   ByVal pTipoCheque As String,
                   ByVal pTipoCruce As String,
                   ByVal pIDConcepto As Nullable(Of Integer),
                   ByVal pDetalleConcepto As String,
                   ByVal pTipoGMF As String,
                   ByVal pValorGenerar As Double,
                   ByVal pValorGMF As Double,
                   ByVal pValorNeto As Double,
                   ByVal pHabilitarValor As Boolean,
                   ByVal pTieneOrdenEnCero As Boolean,
                   ByVal pHabilitarGMF As Boolean)
        Try
            vm = New OrdenPago_DetalleChequesViewModel
            InitializeComponent()

            vm.EsEdicionRegistro = True
            Me.DataContext = vm

            vm.logCambiarPropiedades = False

            vm.ConsultarValoresBaseDatos = False
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
            vm.TipoCliente = pTipoCliente
            vm.NombreBeneficiario = pNombreBeneficiario
            vm.TipoIdentificacion = pTipoIdentificacion
            vm.NroDocumento = pNroDocumento
            vm.TipoCheque = pTipoCheque
            vm.TipoCruce = pTipoCruce
            vm.IDConcepto = pIDConcepto

            If IsNothing(vm.IDConcepto) Or vm.IDConcepto = 0 Then
                If pDetalleConcepto.Contains("(") Then
                    vm.DetalleConcepto = vm.RetornarValorDetalle(pDetalleConcepto, "(", ")")
                Else
                    vm.DetalleConcepto = pDetalleConcepto
                End If
            Else
                vm.DetalleConcepto = vm.RetornarValorDetalle(pDetalleConcepto, "(", ")")
            End If

            vm.LiquidacionesSeleccionadas = vm.RetornarValorDetalle(pDetalleConcepto, "[", "]")

            If pTipoGMF = "N" Then
                vm.TipoGMF = String.Empty
            Else
                vm.TipoGMF = pTipoGMF
            End If

            If vm.TipoGMF = GSTR_GMF_ENCIMA Then
                vm.ValorGenerar = pValorNeto
                vm.ValorGMF = pValorGMF
                vm.ValorNeto = pValorGenerar
            ElseIf vm.TipoGMF = GSTR_GMF_DEBAJO Then
                vm.ValorGenerar = pValorGenerar
                vm.ValorGMF = pValorGMF
                vm.ValorNeto = pValorNeto
            Else
                vm.ValorGenerar = pValorGenerar
                vm.ValorGMF = 0
                vm.ValorNeto = pValorGenerar
            End If

            vm.HabilitarValor_Inicio = pHabilitarValor
            vm.TieneOrdenEnCero = pTieneOrdenEnCero

            vm.logCambiarPropiedades = True

            vm.HabilitarTipoGMFTesorero = pHabilitarGMF 'JAPC20200424-CC20200364: se asigna parametro para controlar si se edita o no desde el tesorero

            vm.TipoRetiroFondos = pTipoRetiroFondos     'JAPC20200504-S-45740 : se asigna tipo retiro fondos en la edicion para que realice validaciones correctamente

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model wppFrmCheque", Me.Name, "New", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Async Sub OrdenPago_DetalleChequesView_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If vm.ConsultarValoresBaseDatos Then
                Await vm.ConsultarDatosRegistro()
            End If
            vm.CargarCombosOYDPLUS("INICIO")
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el formulario", Me.Name, "OrdenPago_DetalleChequesView_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub OrdenPago_DetalleChequesView_Unloaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Unloaded
        Try
            If Not IsNothing(vm) Then
                vm.CancelarEdicionRegistro()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al cerrar el formulario", Me.Name, "OrdenPago_DetalleChequesView_Unloaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para obtener la respuesta del control de liquidaciones.
    ''' Desarrollado por Juan David Correa.
    ''' Fecha 12 de marzo del 20132
    ''' </summary>
    ''' <param name="pstrCliente">Cliente con el cual se realizo la consulta</param>
    ''' <param name="pobjValores">Objeto con los calores: Cliente, ValorTotalLiquidaciones, ValorLiquidacionesSeleccionado,String Liquidaciones Seleccioandas, Objeto información liquidaciones seleccionadas.</param>
    ''' <remarks></remarks>
    Private Sub ctrlConsultaLiquidaciones_finalizoBusquedaLiquidacion(pstrCliente As System.String, pobjValores As A2OYDPLUSUtilidades.RetornoValoresLiquidacion)
        Try
            vm.ObtenerLiquidacionesSeleccionadas(pobjValores)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener las liquidaciones seleccionas.",
                                 Me.ToString(), "ctrlConsultaLiquidaciones_finalizoBusquedaLiquidacion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(pstrClaseControl As System.String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If pstrClaseControl = "conceptos" Then
                    Me.vm.IDConcepto = CInt(pobjItem.IdItem)
                    Me.vm.DescripcionConcepto = pobjItem.Descripcion
                    Me.vm.VerificarCobro_GMF()
                End If
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al obtener el resultado del buscador.", Me.Name, "New", "BuscadorGenerico_finalizoBusqueda", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub txtNombreBeneficiario_TextChanged_1(sender As Object, e As TextChangedEventArgs)
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
                                 Me.ToString(), "txtNombreBeneficiario_TextChanged_1", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub txtNumeroDctoBeneficiario_TextChanged_1(sender As Object, e As TextChangedEventArgs)
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
                                 Me.ToString(), "txtNumeroDctoBeneficiario_TextChanged_1", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub txtConcepto_TextChanged_1(sender As Object, e As TextChangedEventArgs)
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

    Private Sub SeleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        MyBase.OnGotFocus(e)
        If TypeOf sender Is TextBox Then
            CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
            'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
        End If
    End Sub

    Private Sub BuscadorGenericoListaButon_GotFocus_1(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(vm.TipoCheque) Then
                If vm.TipoCheque = GSTR_CHEQUE Then
                    BuscadorGenericoConceptos.TipoItem = "conceptosegresos"
                ElseIf vm.TipoCheque = GSTR_CHEQUE_GERENCIA Then
                    BuscadorGenericoConceptos.TipoItem = "conceptosnotas"
                Else
                    BuscadorGenericoConceptos.TipoItem = ""
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar el texto.",
                                 Me.ToString(), "BuscadorGenericoListaButon_GotFocus_1", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TextBlockConcepto_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs)
        Try
            If Not IsNothing(vm.TipoCheque) Then
                If vm.TipoCheque = GSTR_CHEQUE Then
                    BuscadorGenericoConceptos.TipoItem = "conceptosegresos"
                ElseIf vm.TipoCheque = GSTR_CHEQUE_GERENCIA Then
                    BuscadorGenericoConceptos.TipoItem = "conceptosnotas"
                Else
                    BuscadorGenericoConceptos.TipoItem = ""
                End If

                BuscadorGenericoConceptos.AbrirBuscador()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar el texto.",
                                 Me.ToString(), "TextBlockConcepto_MouseLeftButtonDown", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub btnLimpiarConcepto_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.vm.IDConcepto = Nothing
            Me.vm.DescripcionConcepto = String.Empty
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar el texto.",
                                 Me.ToString(), "btnLimpiarConcepto_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub BtnConsultarSaldo_Click(sender As Object, e As RoutedEventArgs)
        Try
            Me.vm.ConsultarSaldos(String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el saldo.",
                                 Me.ToString(), "BtnConsultarSaldo_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Async Sub BtnGuardarSalir_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim logGuardoExitoso As Boolean = Await vm.GuardarRegistro()
            If logGuardoExitoso Then
                Me.Close()
                If vm.ConsultarValoresBaseDatos Then
                    RaiseEvent OrdenPago_FinalizoGuardarRegistro_BaseDatos(vm.IDDetalle, vm.IDEncabezado)
                Else
                    NotificarGuardadoRegistro(True)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al guardar el registro.",
                                 Me.ToString(), "BtnGuardarSalir_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Async Sub BtnGuardarContinuar_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim logGuardoExitoso As Boolean = Await vm.GuardarRegistro()
            If logGuardoExitoso Then
                Me.Close()
                If vm.ConsultarValoresBaseDatos Then
                    RaiseEvent OrdenPago_FinalizoGuardarRegistro_BaseDatos(vm.IDDetalle, vm.IDEncabezado)
                Else
                    NotificarGuardadoRegistro(False)
                End If
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
                                                     vm.TipoCliente,
                                                     vm.EsTercero,
                                                     vm.DescripcionTipoCliente,
                                                     vm.NombreBeneficiario,
                                                     vm.TipoIdentificacion,
                                                     vm.DescripcionTipoIdentificacion,
                                                     vm.NroDocumento,
                                                     vm.TipoCheque,
                                                     vm.DescripcionTipoCheque,
                                                     vm.TipoCruce,
                                                     vm.DescripcionTipoCruce,
                                                     vm.IDConcepto,
                                                     vm.DescripcionConcepto,
                                                     vm.DetalleConcepto,
                                                     vm.TipoGMF,
                                                     vm.DescripcionTipoGMF,
                                                     vm.ValorGenerar,
                                                     vm.ValorGMF,
                                                     vm.ValorNeto,
                                                     vm.SaldoConsultado,
                                                     vm.LiquidacionesSeleccionadas)
    End Sub
End Class


