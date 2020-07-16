Imports Telerik.Windows.Controls
' Desarrollo       : Clase TrasladarSaldoViewModel, Clase trasladarSaldo
' Creado por       : Juan Carlos Soto Cruz.
' Fecha            : Agosto 17/2011 4:00 p.m   

#Region "Librerias"

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel
Imports A2.OYD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports Microsoft.VisualBasic.CompilerServices

#End Region

#Region "Clases"

''' <summary>
''' View model
''' </summary>
''' <remarks>
''' Nombre	        :	TrasladarSaldoViewModel
''' Creado por	    :	Juan Carlos Soto Cruz.
''' Fecha			:	Agosto 20/2011
''' Pruebas CB		:	Juan Carlos Soto Cruz - Agosto 23/2011 - Resultado Ok
''' </remarks>
Public Class TrasladarSaldoViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Declaraciones"

    'Dim dcProxy As ClientesDomainContext
    Dim dcProxy As CitiBankDomainContext
    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Dim strDestino As String
    Dim DicCamposTab As New Dictionary(Of String, Integer)

    Private MINT_LONG_MAX_CODIGO_OYD As Byte = 17
    Public cambiocomitente As Boolean

#End Region

#Region "Inicialización"

    ''' <summary>
    ''' Se realizan acciones iniciales del proceso y se inicializa el DomainContext dependiendo de si se esta en ambiente de desarrollo o no.
    ''' </summary>
    ''' <remarks>
    ''' Nombre	        :	Sub New
    ''' Creado por	    :	Juan Carlos Soto Cruz.
    ''' Fecha			:	Agosto 20/2011
    ''' Pruebas CB		:	Juan Carlos Soto Cruz - Agosto 23/2011 - Resultado Ok
    ''' </remarks>
    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            'dcProxy = New ClientesDomainContext()
            dcProxy = New CitiBankDomainContext()
            mdcProxyUtilidad01 = New UtilidadesDomainContext()
        Else
            dcProxy = New CitiBankDomainContext(New System.Uri(Program.RutaServicioNegocio))
            mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If

        Try
            If Not Program.IsDesignMode() Then
                trasladarSaldo.Fecha = Now
                trasladarSaldo.rdbSaldoDisponibleChequeado = True
                trasladarSaldo.consecutivoCtaRemunerada = "NOTA_ING_REMUN"
                trasladarSaldo.DescripcionconsecutivoCtaRemunerada = "NOTA INGRESO CUENTA REMUNERADA"
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "TrasladarSaldoViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Propiedades"

    ''' <summary>
    ''' Propiedad del tipo trasladarSaldo para el llenado del CurrentItem del dataform de traslado.
    ''' </summary>
    ''' <remarks>
    ''' Nombre	        :	trasladarSaldo
    ''' Creado por	    :	Juan Carlos Soto Cruz.
    ''' Fecha			:	Agosto 20/2011
    ''' Pruebas CB		:	Juan Carlos Soto Cruz - Agosto 23/2011 - Resultado Ok
    ''' </remarks>
    Private WithEvents _trasladarSaldo As trasladarSaldo = New trasladarSaldo
    Public Property trasladarSaldo() As trasladarSaldo
        Get
            Return _trasladarSaldo
        End Get
        Set(ByVal value As trasladarSaldo)
            _trasladarSaldo = value
            MyBase.CambioItem("trasladarSaldo")
        End Set
    End Property

    Private _TabSeleccionadaFinanciero As Integer = 0
    Public Property TabSeleccionadaFinanciero
        Get
            Return _TabSeleccionadaFinanciero
        End Get
        Set(ByVal value)
            _TabSeleccionadaFinanciero = value
            MyBase.CambioItem("TabSeleccionadaFinanciero")
        End Set
    End Property

#End Region

#Region "Metodos"

    ''' <summary>
    ''' Metodo encargado de consultar el saldo de un comitente para lo cual lanza la operacion SaldoConsultar.
    ''' </summary>
    ''' <remarks>
    ''' Nombre	        :	Consultar()
    ''' Creado por	    :	Juan Carlos Soto Cruz.
    ''' Fecha			:	Agosto 20/2011
    ''' Pruebas CB		:	Juan Carlos Soto Cruz - Agosto 23/2011 - Resultado Ok
    ''' </remarks>
    Sub Consultar()
        Try
            If IsNothing(trasladarSaldo.IdComitente) Or trasladarSaldo.IdComitente = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el código del cliente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            Else
                IsBusy = True
                dcProxy.SaldoConsultar(trasladarSaldo.rdbSaldoDisponibleChequeado, trasladarSaldo.IdComitente, trasladarSaldo.Fecha, Program.Usuario, Program.HashConexion, AddressOf SaldoConsultarCompleted, "Consultar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la Consulta de ", Me.ToString(), "Consultar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        'IsBusy = False
    End Sub

    ''' <summary>
    ''' Metodo encargado de:
    ''' El lanzamiento de la funcion que realiza las validaciones previas al traslado del saldo de un comitente.
    ''' El lanzamiento de la operacion que valida si un comitente posee saldo pendiente.
    ''' </summary>
    ''' <remarks>
    ''' Nombre	        :	trasladaSaldo()
    ''' Creado por	    :	Juan Carlos Soto Cruz.
    ''' Fecha			:	Agosto 20/2011
    ''' Pruebas CB		:	Juan Carlos Soto Cruz - Agosto 23/2011 - Resultado Ok
    ''' </remarks>
    Sub trasladaSaldo()
        Try
            If DatosValidosParaTrasladar() Then
                IsBusy = True
                dcProxy.PendientePorAprobarConsultar(trasladarSaldo.IdComitente, Program.Usuario, Program.HashConexion, AddressOf PendientePorAprobarCompleted, "Consultar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la Consulta de ", Me.ToString(), "Consultar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub

    Private Sub _trasladarSaldo_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _trasladarSaldo.PropertyChanged
        If cambiocomitente Then
            cambiocomitente = False
            Exit Sub
        End If
        If e.PropertyName.Equals("IdComitente") Then
            If Not Versioned.IsNumeric(_trasladarSaldo.IdComitente) Then
                A2Utilidades.Mensajes.mostrarMensaje("El código del comitente debe ser un valor numérico", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                'If _ComitenteSeleccionado Is Nothing Then
                cambiocomitente = True
                trasladarSaldo.IdComitente = Nothing


                'Else
                '    _mstrIdComitente = _ComitenteSeleccionado.IdComitente
                'End If
                'ElseIf _trasladarSaldo.IdComitente.ToString.Length() > MINT_LONG_MAX_CODIGO_OYD Then
                '    A2Utilidades.Mensajes.mostrarMensaje("La longitud máxima del código del comitente es de 17 caracteres", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                '    If _ComitenteSeleccionado Is Nothing Then
                '        _mstrIdComitente = String.Empty
                '    Else
                '        _mstrIdComitente = _ComitenteSeleccionado.IdComitente
                '    End If
            Else
                '_mstrIdComitente = value

                ' If Not _OrdenSelected Is Nothing AndAlso (_ComitenteSeleccionado Is Nothing OrElse Not value.Equals(_ComitenteSeleccionado.IdComitente)) Then
                buscarComitente(Right(Space(17) & trasladarSaldo.IdComitente, MINT_LONG_MAX_CODIGO_OYD))
                'End If
            End If
        End If

    End Sub

    Friend Sub buscarComitente(Optional ByVal pstrIdComitente As String = "")
        Dim strIdComitente As String = String.Empty

        Try
            If Not Me._trasladarSaldo Is Nothing Then
                If Not pstrIdComitente Is Nothing Then
                    strIdComitente = pstrIdComitente
                    'End If

                    'If Not strIdComitente.Equals(Me.OrdenSelected.IDComitente) Then
                    '    If pstrIdComitente.Trim.Equals(String.Empty) Then
                    '        strIdComitente = Me.OrdenSelected.IDComitente
                    '    Else
                    '        strIdComitente = pstrIdComitente
                    '    End If

                    'If Not strIdComitente Is Nothing AndAlso Not strIdComitente.Trim.Equals(String.Empty) Then
                    'IsBusy = True
                    mdcProxyUtilidad01.BuscadorClientes.Clear()
                    mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarClienteEspecificoQuery(strIdComitente, Program.Usuario, "IdComitente", Program.HashConexion), AddressOf buscarComitenteCompleted, "")
                End If
            End If
            'End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub buscarComitenteCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            If lo.Entities.ToList.Count > 0 Then
                cambiocomitente = True
                trasladarSaldo.IdComitente = lo.Entities.ToList.Item(0).CodigoOYD
                trasladarSaldo.Nombre = lo.Entities.ToList.Item(0).Nombre
                trasladarSaldo.valorATrasladar = String.Empty
                trasladarSaldo.valorSaldoCredito = String.Empty
                trasladarSaldo.valorSaldoDebito = String.Empty

            Else
                'Me.ComitenteSeleccionado = Nothing
                'If mstrAccionOrden = MSTR_MC_ACCION_ACTUALIZAR Or mstrAccionOrden = MSTR_MC_ACCION_INGRESAR Then
                A2Utilidades.Mensajes.mostrarMensaje("El comitente ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                'End If
                cambiocomitente = True
                trasladarSaldo.IdComitente = Nothing
                trasladarSaldo.Nombre = String.Empty
                trasladarSaldo.valorATrasladar = String.Empty
                trasladarSaldo.valorSaldoCredito = String.Empty
                trasladarSaldo.valorSaldoDebito = String.Empty

                ' IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del comitente de la orden", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
        'IsBusy = False
    End Sub

#End Region

#Region "Funciones"

    ''' <summary>
    ''' Funcion que realiza las validaciones previas al traslado del saldo de un comitente.
    ''' </summary>
    ''' <returns>Retorna True o False</returns>
    ''' <remarks>
    ''' Nombre	        :	DatosValidosParaTrasladar()
    ''' Creado por	    :	Juan Carlos Soto Cruz.
    ''' Fecha			:	Agosto 20/2011
    ''' Pruebas CB		:	Juan Carlos Soto Cruz - Agosto 23/2011 - Resultado Ok
    ''' </remarks>
    Private Function DatosValidosParaTrasladar() As Boolean

        DatosValidosParaTrasladar = False

        If Program.MostrarMensajeLog = "1" Then
            A2Utilidades.Mensajes.mostrarMensaje("trasladarSaldo.rdbSaldoDisponibleChequeado esta en: " & trasladarSaldo.rdbSaldoDisponibleChequeado & " trasladarSaldo.valorATrasladar esta en: " & trasladarSaldo.valorATrasladar & " trasladarSaldo.valorSaldoDebito esta en:" & trasladarSaldo.valorSaldoDebito,
                                                 Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        End If

        Select Case trasladarSaldo.rdbSaldoDisponibleChequeado
            Case True
                strDestino = " a "
                If trasladarSaldo.valorSaldoDebito <> String.Empty Then
                    A2Utilidades.Mensajes.mostrarMensaje("El traslado que requiere no opera con Saldos A Cargo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Function
                End If
                If trasladarSaldo.valorSaldoCredito = String.Empty Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe consultar el saldo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Function
                End If
                If trasladarSaldo.valorSaldoCredito.Equals(0) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Un saldo en cero no permite hacer traslados.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Function
                End If
                If CDec(trasladarSaldo.valorATrasladar) > CDec(trasladarSaldo.valorSaldoCredito) Then
                    A2Utilidades.Mensajes.mostrarMensaje("El valor de traslado debe ser menor o igual al saldo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Function
                End If
            Case Else
                strDestino = " de "
                If trasladarSaldo.valorSaldoCredito <> String.Empty Then
                    A2Utilidades.Mensajes.mostrarMensaje("El traslado que requiere no opera con Saldos A Favor.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Function
                End If
                If trasladarSaldo.valorSaldoDebito = String.Empty Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe consultar el saldo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Function
                End If
                If trasladarSaldo.valorSaldoDebito = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Un saldo en cero no permite hacer traslados.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Function
                End If
                If CDec(trasladarSaldo.valorATrasladar) > CDec(trasladarSaldo.valorSaldoDebito) Then
                    A2Utilidades.Mensajes.mostrarMensaje("El valor de traslado debe ser menor o igual al saldo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Function
                End If
        End Select


        If trasladarSaldo.valorATrasladar = String.Empty Then
            A2Utilidades.Mensajes.mostrarMensaje("No existe el valor a trasladar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Function
        End If

        If IsNothing(trasladarSaldo.IDCuentaContable) Or trasladarSaldo.IDCuentaContable = 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la cuenta contable.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Function
        End If

        DatosValidosParaTrasladar = True

    End Function

#End Region

#Region "Resultados Asincronicos"

    ''' <summary>
    ''' Metodo Asincrono lanzado al finalizar la ejecucion de la operacion SaldoConsultar.
    ''' </summary>
    ''' <param name="obj">
    ''' ByVal obj As InvokeOperation(Of Decimal)
    ''' </param>
    ''' <remarks>
    ''' Nombre	        :	SaldoConsultarCompleted
    ''' Creado por	    :	Juan Carlos Soto Cruz.
    ''' Fecha			:	Agosto 20/2011
    ''' Pruebas CB		:	Juan Carlos Soto Cruz - Agosto 23/2011 - Resultado Ok
    ''' </remarks>
    Private Sub SaldoConsultarCompleted(ByVal obj As InvokeOperation(Of Decimal))
        IsBusy = False
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó el saldo", Me.ToString(), "SaldoConsultarCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            If obj.Value < 0 Then
                trasladarSaldo.valorSaldoCredito = Format(Math.Abs(obj.Value), "#,#0.00")
                trasladarSaldo.valorSaldoDebito = String.Empty
                trasladarSaldo.valorATrasladar = trasladarSaldo.valorSaldoCredito
            Else
                trasladarSaldo.valorSaldoDebito = Format(Math.Abs(obj.Value), "#,#0.00")
                trasladarSaldo.valorSaldoCredito = String.Empty
                trasladarSaldo.valorATrasladar = trasladarSaldo.valorSaldoDebito
            End If
        End If
        'IsBusy = False
    End Sub

    ''' <summary>
    ''' Metodo Asincrono lanzado al finalizar la ejecucion de la operacion PendientePorAprobarConsultar se encarga tambien de:
    ''' Realizar el lanzamiento de la operacion GrabarNota, la cual realiza el proceso de trasladar el saldo de un comitente.
    ''' </summary>
    ''' <param name="obj">
    ''' ByVal obj As InvokeOperation(Of System.Nullable(Of Decimal))
    ''' </param>
    ''' <remarks>
    ''' Nombre	        :	PendientePorAprobarCompleted
    ''' Creado por	    :	Juan Carlos Soto Cruz.
    ''' Fecha			:	Agosto 20/2011
    ''' Pruebas CB		:	Juan Carlos Soto Cruz - Agosto 23/2011 - Resultado Ok
    ''' </remarks>
    Private Sub PendientePorAprobarCompleted(ByVal obj As InvokeOperation(Of System.Nullable(Of Decimal)))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó el valor pendiente por aprobar.", Me.ToString(), "PendientePorAprobarCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            If obj.Value > 0 Then
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensaje("Este cliente tiene notas de cuenta remunerada pendientes por aprobar, por un valor de " & Format(obj.Value, "#,#0.000"), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            Else
                If MessageBox.Show("Se realizara un traslado de dinero " & trasladarSaldo.valorATrasladar & " del cliente " & trasladarSaldo.Nombre & ". " & strDestino & " su cuenta remunerada. " & vbCrLf & "¿Está de acuerdo?", Program.TituloSistema, MessageBoxButton.OKCancel) = MessageBoxResult.OK Then
                    Select Case trasladarSaldo.rdbSaldoDisponibleChequeado
                        Case True
                            IsBusy = True
                            dcProxy.GrabarNota("ND",
                                               trasladarSaldo.consecutivoCtaRemunerada, trasladarSaldo.IdComitente, trasladarSaldo.valorATrasladar,
                                               trasladarSaldo.IDCuentaContable, trasladarSaldo.Fecha, 0, trasladarSaldo.Nombre, Program.Usuario, Program.HashConexion, AddressOf GrabarNotaCompleted, "GrabarNotaDebito")
                        Case Else
                            IsBusy = True
                            dcProxy.GrabarNota("NC",
                                               trasladarSaldo.consecutivoCtaRemunerada, trasladarSaldo.IdComitente, trasladarSaldo.valorATrasladar,
                                               trasladarSaldo.IDCuentaContable, trasladarSaldo.Fecha, 1, trasladarSaldo.Nombre, Program.Usuario, Program.HashConexion, AddressOf GrabarNotaCompleted, "GrabarNotaCredito")
                    End Select
                End If
            End If
        End If
        IsBusy = False
    End Sub

    ''' <summary>
    ''' Metodo Asincrono lanzado al finalizar la ejecucion de la operacion GrabarNota se encarga tambien de:
    ''' Realizar el lanzamiento del metodo encargado de consultar el saldo de un comitente.
    ''' </summary>
    ''' <param name="obj">
    ''' ByVal obj As InvokeOperation(Of System.Nullable(Of Integer))
    ''' </param>
    ''' <remarks>
    ''' Nombre	        :	GrabarNotaCompleted
    ''' Creado por	    :	Juan Carlos Soto Cruz.
    ''' Fecha			:	Agosto 20/2011
    ''' Pruebas CB		:	Juan Carlos Soto Cruz - Agosto 23/2011 - Resultado Ok
    ''' </remarks>
    Private Sub GrabarNotaCompleted(ByVal obj As InvokeOperation(Of System.Nullable(Of Integer)))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarMensaje("Se presentaron errores en " & obj.UserState, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        Else
            Consultar()
        End If
    End Sub
#End Region

End Class

''' <summary>
''' Clase y Propiedades para el uso en el dataform dfTrasladarSaldo.
''' </summary>
''' <remarks>
''' Nombre	        :	trasladarSaldo
''' Creado por	    :	Juan Carlos Soto Cruz.
''' Fecha			:	Agosto 20/2011
''' Pruebas CB		:	Juan Carlos Soto Cruz - Agosto 23/2011 - Resultado Ok
''' </remarks>
Public Class trasladarSaldo
    Implements INotifyPropertyChanged

    Private _CuentaRetira As System.Nullable(Of Integer)
    <Required(ErrorMessage:="Este campo es requerido. (CuentaRetira)")> _
    <Display(Name:="CuentaRetira")> _
    Public Property CuentaRetira As System.Nullable(Of Integer)
        Get
            Return _CuentaRetira
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _CuentaRetira = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CuentaRetira"))
        End Set
    End Property

    Private _IdComitente As String
    <Display(Name:="Comitente")> _
    Public Property IdComitente As String
        Get
            Return _IdComitente
        End Get
        Set(ByVal value As String)
            _IdComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IdComitente"))
        End Set
    End Property

    Private _IDCuentaContable As String
    <Display(Name:="Cuenta Contable")> _
    Public Property IDCuentaContable As String
        Get
            Return _IDCuentaContable
        End Get
        Set(ByVal value As String)
            _IDCuentaContable = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDCuentaContable"))
        End Set
    End Property

    Private _Nombre As String
    <Display(Name:="")> _
    Public Property Nombre As String
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            _Nombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nombre"))
        End Set
    End Property

    Private _Fecha As Date
    <Display(Name:="A la fecha")> _
    Public Property Fecha As Date
        Get
            Return _Fecha
        End Get
        Set(ByVal value As Date)
            _Fecha = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Fecha"))
        End Set
    End Property

    Private _rdbSaldoDisponibleChequeado As Boolean
    <Display(Name:=" ")> _
    Public Property rdbSaldoDisponibleChequeado As Boolean
        Get
            Return _rdbSaldoDisponibleChequeado
        End Get
        Set(ByVal value As Boolean)
            _rdbSaldoDisponibleChequeado = value
            consecutivoCtaRemunerada = "NOTA_ING_REMUN"
            DescripcionconsecutivoCtaRemunerada = "NOTA INGRESO CUENTA REMUNERADA"
            valorSaldoCredito = String.Empty
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("rdbSaldoDisponibleChequeado"))
        End Set
    End Property

    Private _rdbCtaRemuneradaChequeado As Boolean
    <Display(Name:=" ")> _
    Public Property rdbCtaRemuneradaChequeado As Boolean
        Get
            Return _rdbCtaRemuneradaChequeado
        End Get
        Set(ByVal value As Boolean)
            _rdbCtaRemuneradaChequeado = value
            consecutivoCtaRemunerada = "NOTA_RET_REMUN"
            DescripcionconsecutivoCtaRemunerada = "NOTA RETIRO CUENTA REMUNERADA"
            valorSaldoDebito = String.Empty
            valorATrasladar = String.Empty
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("rdbCtaRemuneradaChequeado"))
        End Set
    End Property

    Private _valorSaldoCredito As String
    <Display(Name:="A su Favor")> _
    Public Property valorSaldoCredito As String
        Get
            Return _valorSaldoCredito
        End Get
        Set(ByVal value As String)
            _valorSaldoCredito = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("valorSaldoCredito"))
        End Set
    End Property

    Private _valorSaldoDebito As String
    <Display(Name:="A su Cargo")> _
    Public Property valorSaldoDebito As String
        Get
            Return _valorSaldoDebito
        End Get
        Set(ByVal value As String)
            _valorSaldoDebito = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("valorSaldoDebito"))
        End Set
    End Property

    Private _valorATrasladar As String
    <Display(Name:="Valor a Trasladar")> _
    Public Property valorATrasladar As String
        Get
            Return _valorATrasladar
        End Get
        Set(ByVal value As String)
            _valorATrasladar = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("valorATrasladar"))
        End Set
    End Property

    Private _consecutivoCtaRemunerada As String
    <Display(Name:="Consecutivo")> _
    Public Property consecutivoCtaRemunerada As String
        Get
            Return _consecutivoCtaRemunerada
        End Get
        Set(ByVal value As String)
            _consecutivoCtaRemunerada = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("consecutivoCtaRemunerada"))
        End Set
    End Property

    Private _DescripcionconsecutivoCtaRemunerada As String
    <Display(Name:=" ")> _
    Public Property DescripcionconsecutivoCtaRemunerada As String
        Get
            Return _DescripcionconsecutivoCtaRemunerada
        End Get
        Set(ByVal value As String)
            _DescripcionconsecutivoCtaRemunerada = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DescripcionconsecutivoCtaRemunerada"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

#End Region