Imports Telerik.Windows.Controls
Imports A2.OYD.OYDServer.RIA.Web
Imports A2.OYD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client

Partial Public Class cwMovimientosParticipacionesFondosView
    Inherits Window
    Implements INotifyPropertyChanged

#Region "Variables"

    Private mobjVM As MovimientosParticipacionesFondosViewModel
    Private mobjDetallePorDefecto As MovimientosParticipacionesFondosDetalle
    Private Const PARAM_STR_MONEDA_LOCAL = "MONEDALOCAL"
    Private strMonedaLocalParametro As String = String.Empty
    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Dim dblUnidadesFinalesAnteriores As Double

#End Region

#Region "Propiedades"

    Private WithEvents _strTipo As String
    Public Property strTipo() As String
        Get
            Return _strTipo
        End Get
        Set(ByVal value As String)
            _strTipo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipo"))
        End Set
    End Property

    Private _dtmMovimiento As System.Nullable(Of System.DateTime)
    Public Property dtmMovimiento() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmMovimiento
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmMovimiento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmMovimiento"))
        End Set
    End Property

    Private _dtmFechaRegistro As System.Nullable(Of System.DateTime)
    Public Property dtmFechaRegistro() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaRegistro
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaRegistro = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmFechaRegistro"))
        End Set
    End Property

    Private _dblValor As System.Nullable(Of Double)
    Public Property dblValor() As System.Nullable(Of Double)
        Get
            Return _dblValor
        End Get
        Set(ByVal value As System.Nullable(Of Double))
            _dblValor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblValor"))
        End Set
    End Property

    Private _dblUnidades As System.Nullable(Of Double)
    Public Property dblUnidades() As System.Nullable(Of Double)
        Get
            Return _dblUnidades
        End Get
        Set(ByVal value As System.Nullable(Of Double))
            _dblUnidades = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblUnidades"))
        End Set
    End Property

    Private _dblVlrUnidad As System.Nullable(Of Double)
    Public Property dblVlrUnidad() As System.Nullable(Of Double)
        Get
            Return _dblVlrUnidad
        End Get
        Set(ByVal value As System.Nullable(Of Double))
            _dblVlrUnidad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblVlrUnidad"))
        End Set
    End Property

    Private _logAplicado As Boolean
    Public Property logAplicado() As Boolean
        Get
            Return _logAplicado
        End Get
        Set(ByVal value As Boolean)
            _logAplicado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logAplicado"))
        End Set
    End Property

    Private _HabilitarEncabezado As Boolean = False
    Public Property HabilitarEncabezado() As Boolean
        Get
            Return _HabilitarEncabezado
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEncabezado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarEncabezado"))
        End Set
    End Property

    Private _HabilitarFechaRegistro As Boolean = False
    Public Property HabilitarFechaRegistro() As Boolean
        Get
            Return _HabilitarFechaRegistro
        End Get
        Set(ByVal value As Boolean)
            _HabilitarFechaRegistro = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarFechaRegistro"))
        End Set
    End Property
    Private _HabilitarConversionMoneda As Boolean = False
    Public Property HabilitarConversionMoneda() As Boolean
        Get
            Return _HabilitarConversionMoneda
        End Get
        Set(ByVal value As Boolean)
            _HabilitarConversionMoneda = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarConversionMoneda"))
        End Set
    End Property
    ''' <summary>
    ''' Indica cuál de los detalles está seleccionado
    ''' </summary>
    Private WithEvents _DetalleSeleccionado As MovimientosParticipacionesFondosDetalle
    Public Property DetalleSeleccionado() As MovimientosParticipacionesFondosDetalle
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As MovimientosParticipacionesFondosDetalle)
            _DetalleSeleccionado = value
        End Set
    End Property

    Private _logExisteApertura As Boolean = False
    Public Property logExisteApertura As Boolean
        Get
            Return _logExisteApertura
        End Get
        Set(ByVal value As Boolean)
            _logExisteApertura = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logExisteApertura"))
        End Set
    End Property

    Private _intIDExisteApertura As Integer
    Public Property intIDExisteApertura As Integer
        Get
            Return _intIDExisteApertura
        End Get
        Set(ByVal value As Integer)
            _intIDExisteApertura = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intIDExisteApertura"))
        End Set
    End Property

    Private _logExisteCancelacion As Boolean = False
    Public Property logExisteCancelacion As Boolean
        Get
            Return _logExisteCancelacion
        End Get
        Set(ByVal value As Boolean)
            _logExisteCancelacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logExisteCancelacion"))
        End Set
    End Property

    Private _intIDExisteCancelacion As Integer
    Public Property intIDExisteCancelacion As Integer
        Get
            Return _intIDExisteCancelacion
        End Get
        Set(ByVal value As Integer)
            _intIDExisteCancelacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intIDExisteCancelacion"))
        End Set
    End Property

    Private _dblSaldo As System.Nullable(Of Double)
    Public Property dblSaldo As System.Nullable(Of Double)
        Get
            Return _dblSaldo
        End Get
        Set(ByVal value As System.Nullable(Of Double))
            _dblSaldo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblSaldo"))
        End Set
    End Property

    Private _dblEntradas As System.Nullable(Of Double)
    Public Property dblEntradas As System.Nullable(Of Double)
        Get
            Return _dblEntradas
        End Get
        Set(ByVal value As System.Nullable(Of Double))
            _dblEntradas = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblEntradas"))
        End Set
    End Property

    Private _dblSalidas As System.Nullable(Of Double)
    Public Property dblSalidas As System.Nullable(Of Double)
        Get
            Return _dblSalidas
        End Get
        Set(ByVal value As System.Nullable(Of Double))
            _dblSalidas = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblSalidas"))
        End Set
    End Property

    Private _dtmFechaCierrePortafolio As System.Nullable(Of System.DateTime)
    Public Property dtmFechaCierrePortafolio As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaCierrePortafolio
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaCierrePortafolio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmFechaCierrePortafolio"))
        End Set
    End Property

    Private _dblValorMonedaOrigen As System.Nullable(Of Double)
    Public Property dblValorMonedaOrigen() As System.Nullable(Of Double)
        Get
            Return _dblValorMonedaOrigen
        End Get
        Set(ByVal value As System.Nullable(Of Double))
            _dblValorMonedaOrigen = value
            If Not IsNothing(strMoneda) Then
                If (dblTasaConvMoneda <> 1 And dblTasaConvMoneda <> 0) Or dblTasaConvMoneda Is Nothing Then
                    dblValor = dblValorMonedaOrigen * dblTasaConvMoneda
                    'dblValor = _DetalleSeleccionado.dblValor
                Else
                    dblValor = dblValorMonedaOrigen
                    'dblValor = _DetalleSeleccionado.dblValor
                End If
            End If
            '    If strMoneda.ToLower() = "cop" Then
            '        dblValor = dblValorMonedaOrigen
            '    End If
            'End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblValorMonedaOrigen"))
        End Set
    End Property

    Private _strMoneda As String
    Public Property strMoneda() As String
        Get
            Return _strMoneda
        End Get
        Set(ByVal value As String)
            _strMoneda = value
        End Set
    End Property

    Private _intIDBanco As System.Nullable(Of Integer)
    Public Property intIDBanco As System.Nullable(Of Integer)
        Get
            Return _intIDBanco
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _intIDBanco = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intIDBanco"))
        End Set
    End Property

    Private _dblTasaConvMoneda As System.Nullable(Of Double)
    Public Property dblTasaConvMoneda() As System.Nullable(Of Double)
        Get
            Return _dblTasaConvMoneda
        End Get
        Set(ByVal value As System.Nullable(Of Double))
            _dblTasaConvMoneda = value
            If Not IsNothing(strMoneda) Then
                If (dblTasaConvMoneda <> 1 And dblTasaConvMoneda <> 0) Or dblTasaConvMoneda Is Nothing Then
                    dblValor = dblValorMonedaOrigen * dblTasaConvMoneda
                    'dblValor = _DetalleSeleccionado.dblValor
                Else
                    dblValor = dblValorMonedaOrigen
                    'dblValor = _DetalleSeleccionado.dblValor
                End If
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblTasaConvMoneda"))
        End Set
    End Property

    Private WithEvents _strDescripcionBanco As String
    Public Property strDescripcionBanco() As String
        Get
            Return _strDescripcionBanco
        End Get
        Set(ByVal value As String)
            _strDescripcionBanco = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDescripcionBanco"))
        End Set
    End Property

    Private _HabilitarMonedaOrigen As Boolean = True
    Public Property HabilitarMonedaOrigen() As Boolean
        Get
            Return _HabilitarMonedaOrigen
        End Get
        Set(ByVal value As Boolean)
            _HabilitarMonedaOrigen = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarMonedaOrigen"))
        End Set
    End Property

#End Region

#Region "Inicializacion"

    ''' <summary>
    ''' Inicializa la ventana para cobro de utilidades (estilos y consulta de los registros)
    ''' </summary>
    Public Sub New(ByVal pmobjVM As MovimientosParticipacionesFondosViewModel, ByVal DetalleSeleccionadoVM As MovimientosParticipacionesFondosDetalle, mobjDetallePorDefectoVM As MovimientosParticipacionesFondosDetalle, ByVal HabilitarEdicionDetalleVM As Boolean, ByVal logExisteAperturaVM As Boolean, ByVal intIDExisteAperturaVM As Integer, ByVal logExisteCancelacionVM As Boolean, ByVal intIDExisteCancelacionVM As Integer, ByVal EncabezadoSeleccionadoVM As MovimientosParticipacionesFondos)
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

        Try

            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                mdcProxyUtilidad01 = New UtilidadesDomainContext()
            Else
                mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            End If

            inicializar()

            'Me.DataContext = mobjVM
            InitializeComponent()
            Me.LayoutRoot.DataContext = Me

            DetalleSeleccionado = DetalleSeleccionadoVM
            mobjDetallePorDefecto = mobjDetallePorDefectoVM
            logExisteApertura = logExisteAperturaVM
            intIDExisteApertura = intIDExisteAperturaVM
            logExisteCancelacion = logExisteCancelacionVM
            intIDExisteCancelacion = intIDExisteCancelacionVM
            dblSaldo = EncabezadoSeleccionadoVM.dblSaldo
            dblEntradas = EncabezadoSeleccionadoVM.dblEntradas
            dblSalidas = EncabezadoSeleccionadoVM.dblSalidas
            dtmFechaCierrePortafolio = EncabezadoSeleccionadoVM.dtmFechaCierrePortafolio
            strMoneda = EncabezadoSeleccionadoVM.strMoneda



            If Not IsNothing(DetalleSeleccionado) Then
                strTipo = DetalleSeleccionado.strDescripcionTipo
                dtmMovimiento = DetalleSeleccionado.dtmMovimiento
                dblValor = DetalleSeleccionado.dblValor
                dblUnidades = DetalleSeleccionado.dblUnidades
                dblVlrUnidad = DetalleSeleccionado.dblVlrUnidad
                logAplicado = DetalleSeleccionado.logAplicado
                dblValorMonedaOrigen = DetalleSeleccionado.dblValorMonedaOrigen
                intIDBanco = DetalleSeleccionado.intIDBanco
                strDescripcionBanco = DetalleSeleccionado.strDescripcionBanco
                dblTasaConvMoneda = DetalleSeleccionado.dblTasaConvMoneda
                dtmFechaRegistro = DetalleSeleccionado.dtmFechaRegistro
                dblUnidadesFinalesAnteriores = CDbl(DetalleSeleccionado.dblValorMonedaOrigen)



                If Not HabilitarEdicionDetalleVM Then
                    HabilitarEncabezado = False
                Else
                    HabilitarEncabezado = CBool(IIf(DetalleSeleccionado.logAplicado = False, True, False))
                End If
            Else
                strTipo = String.Empty
                dtmMovimiento = Date.Now
                dtmFechaRegistro = Date.Now
                dblValor = 0
                dblUnidades = 0
                dblVlrUnidad = 0
                logAplicado = False
                HabilitarEncabezado = True
                dblValorMonedaOrigen = 0
                strDescripcionBanco = String.Empty
            End If

            mdcProxyUtilidad01.Verificaparametro(PARAM_STR_MONEDA_LOCAL, Program.Usuario, Program.HashConexion, AddressOf TerminotraerparametroCF, PARAM_STR_MONEDA_LOCAL)

            mobjVM = pmobjVM
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible inicializar el control." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

    End Sub
    Private Sub TerminotraerparametroCF(ByVal obj As InvokeOperation(Of String))
        Try
            If obj.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la validacion", Me.ToString(), "TerminotraerparametroCF", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
            Else
                If Not IsNothing(obj.UserState) Then
                    If obj.UserState.ToString = PARAM_STR_MONEDA_LOCAL Then
                        strMonedaLocalParametro = obj.Value

                        If strMonedaLocalParametro = strMoneda Then
                            If Not IsNothing(_DetalleSeleccionado) Then
                                _DetalleSeleccionado.dblTasaConvMoneda = 1
                                dblTasaConvMoneda = 1
                            Else
                                dblTasaConvMoneda = 1
                            End If

                            HabilitarConversionMoneda = False
                        Else
                            HabilitarConversionMoneda = True
                        End If
                    End If

                    If Not IsNothing(strMoneda) Then
                        If (dblTasaConvMoneda <> 1 And dblTasaConvMoneda <> 0) Or dblTasaConvMoneda Is Nothing Then
                            dblValor = dblValorMonedaOrigen * dblTasaConvMoneda
                            'dblValor = _DetalleSeleccionado.dblValor
                        Else
                            dblValor = dblValorMonedaOrigen
                            'dblValor = _DetalleSeleccionado.dblValor
                        End If
                    End If
                End If
            End If


        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención del parámetro", _
                                    Me.ToString(), "TerminotraerparametroCF", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Async Sub inicializar()

        Await CType(Me.Resources("A2VM"), A2UtilsViewModel).inicializarCombos(String.Empty, String.Empty, True)

    End Sub

#End Region

    Private Function ValidarDetalle() As Boolean
        Dim logResultado As Boolean = True
        Dim strMsg As String = String.Empty

        Try
            '------------------------------------------------------------------------------------------------------------------------------------------------
            '-- Valida que por lo menos exista un detalle para poder crear todo un registro
            '------------------------------------------------------------------------------------------------------------------------------------------------
            If dtmFechaRegistro.Value.Date > dtmMovimiento.Value.Date Then
                strMsg = String.Format("{0}{1} + La fecha de registro no puede ser mayor a la fecha de cumplimiento.", strMsg, vbCrLf)
            End If

            If String.IsNullOrEmpty(strTipo) Then
                strMsg = String.Format("{0}{1} + El tipo es un campo requerido.", strMsg, vbCrLf)
            End If

            If IsNothing(dtmMovimiento) Then
                strMsg = String.Format("{0}{1} + La fecha es un campo requerido.", strMsg, vbCrLf)
            End If

            'If IsNothing(DetalleSeleccionado) Then
            If dtmMovimiento <= dtmFechaCierrePortafolio Then
                strMsg = String.Format("{0}{1} + La fecha de movimiento no puede ser menor o igual a la fecha de cierre de portafolio.", strMsg, vbCrLf)
            End If
            'End If

            If dblValorMonedaOrigen = 0 Then
                strMsg = String.Format("{0}{1} + El valor es un campo requerido.", strMsg, vbCrLf)
            End If

            If Not IsNothing(strTipo) Then
                If strTipo.ToLower = "cancelación" Then
                    If dblSaldo = 0 Then
                        If dblValorMonedaOrigen <> (dblEntradas - dblSalidas) Then
                            strMsg = String.Format("{0}{1} + Si el tipo es una cancelación, el valor debe de ser igual a la diferencia entre el valor de la entrada y el valor de la salida del movimiento.", strMsg, vbCrLf)
                        End If
                    Else
                        If Math.Round(CDbl(dblValorMonedaOrigen), 4) <> Math.Round(CDbl(dblSaldo), 4) Then
                            strMsg = String.Format("{0}{1} + Si el tipo es una cancelación, el valor debe de ser igual al saldo del movimiento.", strMsg, vbCrLf)
                        End If
                    End If
                End If
            End If

            If Not IsNothing(strTipo) Then
                If strTipo.ToLower = "retiro" Then
                    If dblSaldo = 0 Then
                        If dblValorMonedaOrigen <> (dblEntradas - dblSalidas) Then
                            strMsg = String.Format("{0}{1} + Si el tipo es un retiro, el valor debe de ser igual a la diferencia entre el valor de la entrada y el valor de la salida del movimiento.", strMsg, vbCrLf)
                        End If
                    Else
                        If dblValorMonedaOrigen >= dblSaldo Then
                            strMsg = String.Format("{0}{1} + Si el tipo es un retiro, el valor debe de ser inferior al saldo del movimiento.", strMsg, vbCrLf)
                        End If
                    End If
                End If
            End If

            If Not IsNothing(strTipo) Then
                If IsNothing(DetalleSeleccionado) And Not logExisteApertura Then
                    If strTipo.ToLower <> "apertura" Then
                        strMsg = String.Format("{0}{1} + Inicialmente debe de existir una apertura.", strMsg, vbCrLf)
                    End If
                End If
            End If

            If Not strMsg.Equals(String.Empty) Then
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias en el detalle: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            logResultado = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados en el detalle.", Me.ToString(), "ValidarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return (logResultado)
    End Function

#Region "Métodos para control de eventos"

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        Try
            If ValidarDetalle() Then

                If IsNothing(DetalleSeleccionado) Then

                    Dim objNvoDetalle As New MovimientosParticipacionesFondosDetalle
                    Dim objNuevaLista As New List(Of MovimientosParticipacionesFondosDetalle)

                    Program.CopiarObjeto(Of MovimientosParticipacionesFondosDetalle)(mobjDetallePorDefecto, objNvoDetalle)

                    objNvoDetalle.intIDMovimientosParticipacionesFondosDetalle = -New Random().Next(0, 1000000)

                    If IsNothing(mobjVM.ListaDetalle) Then
                        mobjVM.ListaDetalle = New List(Of MovimientosParticipacionesFondosDetalle)
                    End If

                    objNuevaLista = mobjVM.ListaDetalle

                    objNvoDetalle.strDescripcionTipo = strTipo
                    objNvoDetalle.dtmMovimiento = dtmMovimiento
                    objNvoDetalle.dblValor = dblValor
                    objNvoDetalle.dblUnidades = dblUnidades
                    objNvoDetalle.dblVlrUnidad = dblVlrUnidad
                    objNvoDetalle.dblValorMonedaOrigen = dblValorMonedaOrigen
                    objNvoDetalle.dblTasaConvMoneda = dblTasaConvMoneda
                    objNvoDetalle.dtmFechaRegistro = dtmFechaRegistro

                    If (Application.Current.Resources.Contains(Program.NombreListaCombos)) Then
                        If Not IsNothing(CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))) Then
                            If CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey("CuentaBancaria") Then
                                If CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))("CuentaBancaria").Where(Function(i) i.ID = intIDBanco.ToString).Count > 0 Then
                                    objNvoDetalle.strDescripcionBanco = CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))("CuentaBancaria").Where(Function(i) i.ID = intIDBanco.ToString).First.Descripcion
                                End If
                            End If
                        End If
                    End If

                    objNvoDetalle.intIDBanco = intIDBanco

                    objNuevaLista.Add(objNvoDetalle)
                    mobjVM.ListaDetalle = objNuevaLista
                    DetalleSeleccionado = mobjVM.ListaDetalle.First

                    mobjVM.DetalleSeleccionado = DetalleSeleccionado

                Else
                    DetalleSeleccionado.strDescripcionTipo = strTipo
                    DetalleSeleccionado.dtmMovimiento = dtmMovimiento
                    DetalleSeleccionado.dblValor = dblValor
                    DetalleSeleccionado.dblUnidades = dblUnidades
                    DetalleSeleccionado.dblVlrUnidad = dblVlrUnidad
                    DetalleSeleccionado.dblValorMonedaOrigen = dblValorMonedaOrigen
                    DetalleSeleccionado.dblTasaConvMoneda = dblTasaConvMoneda
                    DetalleSeleccionado.dtmFechaRegistro = dtmFechaRegistro

                    If (Application.Current.Resources.Contains(Program.NombreListaCombos)) Then
                        If Not IsNothing(CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))) Then
                            If CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey("CuentaBancaria") Then
                                If CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))("CuentaBancaria").Where(Function(i) i.ID = intIDBanco.ToString).Count > 0 Then
                                    DetalleSeleccionado.strDescripcionBanco = CType(Application.Current.Resources(Program.NombreListaCombos), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))("CuentaBancaria").Where(Function(i) i.ID = intIDBanco.ToString).First.Descripcion
                                End If
                            End If
                        End If
                    End If

                    DetalleSeleccionado.intIDBanco = intIDBanco
                End If

                Me.DialogResult = True
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón aceptar.", Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As RoutedEventArgs)
        Try
            'Me.Close()
            Me.DialogResult = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón cerrar.", Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    Private Sub cmbTipo_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)
        If cmbTipo.SelectedItem IsNot Nothing Then
            HabilitarMonedaOrigen = HabilitarEncabezado
            dblValorMonedaOrigen = dblUnidadesFinalesAnteriores
            If strTipo.ToLower = "apertura" Then
                If Not IsNothing(DetalleSeleccionado) Then
                    If DetalleSeleccionado.intIDMovimientosParticipacionesFondosDetalle <> intIDExisteApertura And logExisteApertura Then
                        A2Utilidades.Mensajes.mostrarMensaje("No es posible realizar una nueva apertura, solamente puede existir una por movimiento.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        strTipo = String.Empty
                        Exit Sub
                    End If
                Else
                    If logExisteApertura Then
                        A2Utilidades.Mensajes.mostrarMensaje("No es posible realizar una nueva apertura, solamente puede existir una por movimiento.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        strTipo = String.Empty
                        Exit Sub
                    End If
                End If
            ElseIf strTipo.ToLower = "cancelación" Then
                dblValor = dblSaldo

                If Not IsNothing(DetalleSeleccionado) Then
                    If DetalleSeleccionado.intIDMovimientosParticipacionesFondosDetalle <> intIDExisteCancelacion And logExisteCancelacion Then
                        A2Utilidades.Mensajes.mostrarMensaje("No es posible realizar una nueva cancelación, solamente puede existir una por movimiento.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        strTipo = String.Empty
                        dblValor = 0
                        Exit Sub
                    End If
                Else
                    If logExisteCancelacion Then
                        A2Utilidades.Mensajes.mostrarMensaje("No es posible realizar una nueva cancelación, solamente puede existir una por movimiento.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        strTipo = String.Empty
                        dblValor = 0
                        Exit Sub
                    End If
                End If
                dblValorMonedaOrigen = dblSaldo
                HabilitarMonedaOrigen = False
            End If
        End If
    End Sub
End Class

