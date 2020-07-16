Imports Telerik.Windows.Controls
' Desarrollo       : Clase ActualizarTitulos, Clase xxx
' Creado por	    :	Juan David Osorio
' Fecha			:	Julio 2013


#Region "Librerias"
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel
Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFPortafolio
Imports A2.OyD.OYDServer.RIA.Web.CFEspecies
Imports System.Windows.Data
Imports A2ComunesControl
Imports A2Utilidades.Mensajes
Imports System.Threading.Tasks

#End Region

#Region "Clases"

''' <summary>
''' View model
''' </summary>
''' <remarks>
''' Nombre	        :	ActualizarTitulos
''' Creado por	    :	Juan David Osorio
''' Fecha			:	Julio 2013
''' </remarks>
Public Class ActualizarTitulosViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Declaraciones"

    Dim dcProxy As PortafolioDomainContext
    Dim dcProxyEspecies As EspeciesCFDomainContext
    Dim dcProxyMaestros As MaestrosCFDomainContext
    Dim intTitulos As Integer
    Dim _mlogSeRealizoPregunta As Boolean = False
    Public _mlogBuscarClienteInicial As Boolean = True
    Public _mlogBuscarClienteFinal As Boolean = True
    Private objProxyUtilidades As UtilidadesDomainContext
    Public _mlogBuscarCliente As Boolean = True
    Public _mlogBuscarCliente2 As Boolean = True
    Public _mlogBuscarEspecie As Boolean = True
    Public _mlogBuscarEspecie2 As Boolean = True
    Public strCodigoOyD1 As String = ""
    Public strCodigoOyD2 As String = ""
    Public strEspecie1 As String = ""
    Public strEspecie2 As String = ""
    Public _mlogBuscarISINFungible As Boolean = True
    Public strISINFungible As String = ""
    Public mlogRedondear As Boolean = True

    Private mdtmFechaCierre_Del_Codigo As DateTime? = Nothing
    Private mdtmFechaCierre_Al_Codigo As DateTime? = Nothing
    Private dtmFechaProcesoSeleccionada As Date
#End Region

#Region "Inicialización"

    ''' <summary>
    ''' Se realizan acciones iniciales del proceso y se inicializa el DomainContext dependiendo de si se esta en ambiente de desarrollo o no.
    ''' </summary>
    ''' <remarks>
    ''' Nombre	        :	Sub New
    ''' Creado por	    :	Juan Carlos Soto Cruz.
    ''' Fecha			:	Agosto 26/2011
    ''' Pruebas CB		:	Juan Carlos Soto Cruz - Agosto 26/2011 - Resultado Ok
    ''' </remarks>
    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New PortafolioDomainContext
                objProxyUtilidades = New UtilidadesDomainContext()
                dcProxyEspecies = New EspeciesCFDomainContext
                dcProxyMaestros = New MaestrosCFDomainContext
            Else
                dcProxy = New PortafolioDomainContext(New System.Uri(Program.RutaServicioPortafolio))
                objProxyUtilidades = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
                dcProxyEspecies = New EspeciesCFDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_ESPECIES).ToString()))
                dcProxyMaestros = New MaestrosCFDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            End If

            DirectCast(dcProxy.DomainClient, WebDomainClient(Of PortafolioDomainContext.IPortafolioDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 40, 0)

            ConsultarConceptoTitulo()

            strTipoRedondeo = 0

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ActualizarEstadoViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Propiedades"
    Private _ListaDetalleCustodia As List(Of DetalleCustodia)
    Public Property ListaDetalleCustodia() As List(Of DetalleCustodia)
        Get
            Return _ListaDetalleCustodia
        End Get
        Set(ByVal value As List(Of DetalleCustodia))
            _ListaDetalleCustodia = value
            If Not IsNothing(_ListaDetalleCustodia) Then
                SelectedDetalleCustodia = _ListaDetalleCustodia.LastOrDefault
                ContarDecimales()
            End If
            MyBase.CambioItem("ListaDetalleCustodia")
            MyBase.CambioItem("ListaDetalleCustodiaPaged")
        End Set
    End Property
    Private _SelectedDetalleCustodia As DetalleCustodia
    Public Property SelectedDetalleCustodia() As DetalleCustodia
        Get
            Return _SelectedDetalleCustodia
        End Get
        Set(ByVal value As DetalleCustodia)
            _SelectedDetalleCustodia = value
            MyBase.CambioItem("SelectedDetalleCustodia")
        End Set
    End Property
    Public ReadOnly Property ListaDetalleCustodiaPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDetalleCustodia) Then
                Dim view = New PagedCollectionView(_ListaDetalleCustodia)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property


    Private WithEvents _CamposBusquedaSelected As New CamposBusquedaActualizarTitulos
    Public Property CamposBusquedaSelected() As CamposBusquedaActualizarTitulos
        Get
            Return _CamposBusquedaSelected
        End Get
        Set(ByVal value As CamposBusquedaActualizarTitulos)
            _CamposBusquedaSelected = value
            MyBase.CambioItem("CamposBusquedaSelected")
        End Set
    End Property
    Private WithEvents _CamposBusquedaSelected2 As New CamposBusquedaActualizarTitulos
    Public Property CamposBusquedaSelected2() As CamposBusquedaActualizarTitulos
        Get
            Return _CamposBusquedaSelected2
        End Get
        Set(ByVal value As CamposBusquedaActualizarTitulos)
            _CamposBusquedaSelected2 = value
            MyBase.CambioItem("CamposBusquedaSelected2")
        End Set
    End Property

    Private _IdComitente As String
    Public Property IdComitente() As String
        Get
            Return _IdComitente
        End Get
        Set(ByVal value As String)
            _IdComitente = value
            MyBase.CambioItem("IdComitente")
        End Set
    End Property


    Private _RealizarSplits As Boolean = False
    Public Property RealizarSplits() As Boolean
        Get
            Return _RealizarSplits
        End Get
        Set(ByVal value As Boolean)
            _RealizarSplits = value
            If _RealizarSplits = False Then
                VerSplit = Visibility.Collapsed
                VerRecalculo = Visibility.Collapsed
            End If
            MyBase.CambioItem("RealizarSplits")
        End Set
    End Property

    Private Sub terminoMensaje()
        RealizarSplits = False
    End Sub

    Private _VerRecalculo As Visibility = Visibility.Visible
    Public Property VerRecalculo() As Visibility
        Get
            Return _VerRecalculo
        End Get
        Set(ByVal value As Visibility)
            _VerRecalculo = value
            MyBase.CambioItem("VerRecalculo")
        End Set
    End Property
    Private _VerSplit As Visibility = Visibility.Collapsed
    Public Property VerSplit() As Visibility
        Get
            Return _VerSplit
        End Get
        Set(ByVal value As Visibility)
            _VerSplit = value
            MyBase.CambioItem("VerSplit")
        End Set
    End Property
    Private _SeleccionarTodos As Boolean = False
    Public Property SeleccionarTodos() As Boolean
        Get
            Return _SeleccionarTodos
        End Get
        Set(ByVal value As Boolean)
            _SeleccionarTodos = value
            RealizarSplits = value
            If Not IsNothing(ListaDetalleCustodia) Then
                If ListaDetalleCustodia.Count > 0 Then
                    If _SeleccionarTodos Then

                        For Each li In ListaDetalleCustodia
                            li.logGenerar = True
                        Next
                    Else
                        For Each li In ListaDetalleCustodia
                            li.logGenerar = False
                        Next
                    End If
                End If
            End If

            MyBase.CambioItem("SeleccionarTodos")
        End Set
    End Property
    Private _HabilitarSeleccionarTodos As Boolean = False
    Public Property HabilitarSeleccionarTodos() As Boolean
        Get
            Return _HabilitarSeleccionarTodos
        End Get
        Set(ByVal value As Boolean)
            _HabilitarSeleccionarTodos = value
            MyBase.CambioItem("HabilitarSeleccionarTodos")
        End Set
    End Property
    Private _HabilitarConsultar As Boolean = False
    Public Property HabilitarConsultar() As Boolean
        Get
            Return _HabilitarConsultar
        End Get
        Set(ByVal value As Boolean)
            _HabilitarConsultar = value
            MyBase.CambioItem("HabilitarConsultar")
        End Set
    End Property
    Private _ListaIsin As List(Of EspeciesISINFungible)
    Public Property ListaIsin() As List(Of EspeciesISINFungible)
        Get
            Return _ListaIsin
        End Get
        Set(ByVal value As List(Of EspeciesISINFungible))
            _ListaIsin = value

            MyBase.CambioItem("ListaIsin")
        End Set
    End Property
    Private _SelectedISIN As String
    Public Property SelectedISIN() As String
        Get
            Return _SelectedISIN
        End Get
        Set(ByVal value As String)
            _SelectedISIN = value
            MyBase.CambioItem("SelectedISIN")
        End Set
    End Property

    Private _dblFactor As Double = 1
    Public Property dblFactor() As Double
        Get
            Return _dblFactor
        End Get
        Set(ByVal value As Double)
            _dblFactor = value
            MyBase.CambioItem("dblFactor")
        End Set
    End Property
    Private _dblPrecio As Double = 0
    Public Property dblPrecio() As Double
        Get
            Return _dblPrecio
        End Get
        Set(ByVal value As Double)
            _dblPrecio = value
            MyBase.CambioItem("dblPrecio")
        End Set
    End Property

    'JAEZ 20161026
    Private _logConvertirMoneda As Boolean = False
    Public Property logConvertirMoneda As Boolean
        Get
            Return _logConvertirMoneda
        End Get
        Set(ByVal value As Boolean)
            _logConvertirMoneda = value
            If Not _logConvertirMoneda Then
                strMoneda = Nothing
                dblTasaConversion = 0
            End If
            MyBase.CambioItem("logConvertirMoneda")
        End Set
    End Property

    'JAEZ 20161026
    Private _strMoneda As String
    Public Property strMoneda As String
        Get
            Return _strMoneda
        End Get
        Set(ByVal value As String)
            _strMoneda = value
            If IsNothing(_strMoneda) Then
                dblTasaConversion = 0
            End If
            MyBase.CambioItem("strMoneda")
        End Set
    End Property

    'JAEZ 20161026
    Private _dblTasaConversion As Double
    Public Property dblTasaConversion As Double
        Get
            Return _dblTasaConversion
        End Get
        Set(ByVal value As Double)
            _dblTasaConversion = value
            MyBase.CambioItem("dblTasaConversion")
        End Set
    End Property

    'JAEZ 20161027
    Private _listaCuentas As List(Of CFPortafolio.ListadoCuenta)
    Public Property listaCuentas As List(Of CFPortafolio.ListadoCuenta)
        Get
            Return _listaCuentas
        End Get
        Set(ByVal value As List(Of CFPortafolio.ListadoCuenta))
            _listaCuentas = value
            MyBase.CambioItem("listaCuentas")
        End Set
    End Property


    'JAEZ 20161026
    Private _strCuentaDestino As Nullable(Of Integer)
    Public Property strCuentaDestino As Nullable(Of Integer)
        Get
            Return _strCuentaDestino
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _strCuentaDestino = value
            MyBase.CambioItem("strCuentaDestino")
        End Set
    End Property

    Private _strFondo As String
    Public Property strFondo As String
        Get
            Return _strFondo
        End Get
        Set(ByVal value As String)
            _strFondo = value
            MyBase.CambioItem("strFondo")
        End Set
    End Property


    Private _ListaEstadoConcepto As List(Of CFMaestros.EstadosConceptoTitulos)
    Public Property ListaEstadoConcepto() As List(Of CFMaestros.EstadosConceptoTitulos)
        Get
            Return _ListaEstadoConcepto
        End Get
        Set(ByVal value As List(Of CFMaestros.EstadosConceptoTitulos))
            _ListaEstadoConcepto = value

            MyBase.CambioItem("ListaEstadoConcepto")
        End Set
    End Property

    Private _SelectedEstadoConcepto As CFMaestros.EstadosConceptoTitulos
    Public Property SelectedEstadoConcepto() As CFMaestros.EstadosConceptoTitulos
        Get
            Return _SelectedEstadoConcepto
        End Get
        Set(ByVal value As CFMaestros.EstadosConceptoTitulos)
            _SelectedEstadoConcepto = value
            MyBase.CambioItem("SelectedEstadoConcepto")
        End Set
    End Property

    Private _strTipoRedondeo As String = String.Empty
    Public Property strTipoRedondeo() As String
        Get
            Return _strTipoRedondeo
        End Get
        Set(ByVal value As String)
            _strTipoRedondeo = value
            MyBase.CambioItem("strTipoRedondeo")
        End Set
    End Property

    Private _dtmFechaProceso As System.Nullable(Of System.DateTime) = Now.Date
    Public Property dtmFechaProceso() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaProceso
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaProceso = value
            MyBase.CambioItem("dtmFechaProceso")
        End Set
    End Property

    ''' <summary>
    ''' Descripción:  Al consultar el portafolio por una fecha de proceso y al cambiar la fecha de proceso por otra, se debe limpiar el grid.
    ''' Responsable:  Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:        30 de Diciembre\2015
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub _dtmFechaProceso_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles Me.PropertyChanged
        Try
            Select Case e.PropertyName
                Case "dtmFechaProceso"
                    If Not IsNothing(dtmFechaProcesoSeleccionada) Then
                        If dtmFechaProcesoSeleccionada <> dtmFechaProceso Then
                            ListaDetalleCustodia = Nothing
                        End If
                    End If
            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar la fecha de proceso", _
                     Me.ToString(), "_dtmFechaProceso_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Metodos"

    ''' <history>
    ''' Descripción:  Se valida la fecha del proceso. Cambio autorizado por Jorge Arango.
    ''' Responsable:  Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:        16 de Octubre/2015
    ''' </history>
    Public Function Validaciones() As Boolean
        Try
            Dim logValidacion As Boolean = True
            Dim strMensajeValidacion As String = String.Empty

            If Not IsNothing(ListaDetalleCustodia) Then

                If IsNothing(dtmFechaProceso) Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1} La fecha del proceso es un campo requerido.", strMensajeValidacion, vbCrLf)
                End If

                If dtmFechaProceso > Now.Date Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1} La fecha del proceso no puede ser mayor a la fecha actual.", strMensajeValidacion, vbCrLf)
                End If

                If Not IsNothing(mdtmFechaCierre_Del_Codigo) Then
                    If (mdtmFechaCierre_Del_Codigo >= dtmFechaProceso) Then
                        logValidacion = False
                        strMensajeValidacion = String.Format("{0}{1}El portafolio del cliente " & CamposBusquedaSelected.CodigoCliente.Trim & "-" & CamposBusquedaSelected.NombreCliente & " está cerrado para la fecha de proceso (Fecha de cierre " & Year(mdtmFechaCierre_Del_Codigo) & "/" & Month(mdtmFechaCierre_Del_Codigo) & "/" & Day(mdtmFechaCierre_Del_Codigo) & ", Fecha de proceso " & Year(dtmFechaProceso) & "/" & Month(dtmFechaProceso) & "/" & Day(dtmFechaProceso) & "). ", strMensajeValidacion, vbCrLf)
                    End If
                End If

                If Not IsNothing(mdtmFechaCierre_Al_Codigo) Then
                    If (mdtmFechaCierre_Al_Codigo >= dtmFechaProceso) Then
                        logValidacion = False
                        strMensajeValidacion = String.Format("{0}{1}El portafolio del cliente " & CamposBusquedaSelected2.CodigoCliente.Trim & "-" & CamposBusquedaSelected2.NombreCliente & " está cerrado para la fecha de proceso (Fecha de cierre " & Year(mdtmFechaCierre_Al_Codigo) & "/" & Month(mdtmFechaCierre_Al_Codigo) & "/" & Day(mdtmFechaCierre_Al_Codigo) & ", Fecha de proceso " & Year(dtmFechaProceso) & "/" & Month(dtmFechaProceso) & "/" & Day(dtmFechaProceso) & "). ", strMensajeValidacion, vbCrLf)
                    End If
                End If

                If ListaDetalleCustodia.Where(Function(i) i.logGenerar = True).Count <= 0 Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1} Elegir: Debe elegir minimo 1 registro para generar.", strMensajeValidacion, vbCrLf)
                Else
                    'Descripción:   Se valida que la cantidad a trasladar no pueda ser superior a la cantidad
                    'Responsable:   Jorge Peña (Alcuadrado S.A.)
                    'Fecha:         5 de Otubre/2015
                    'Pruebas CB:    Jorge Peña (Alcuadrado S.A.) - 5 de Otubre/2015 - Resultados OK
                    For Each li In ListaDetalleCustodia
                        If li.dblCantidadTrasladar > li.Cantidad Then
                            logValidacion = False
                            strMensajeValidacion = String.Format("{0}{1}La cantidad a trasladar en la custodia nro. " + CType(li.IdRecibo, String) + " no puede ser superior a la cantidad.", strMensajeValidacion, vbCrLf)
                        End If
                    Next
                End If
            End If
            If Not IsNothing(CamposBusquedaSelected) And Not IsNothing(CamposBusquedaSelected2) Then
                If CamposBusquedaSelected.EsAccion <> CamposBusquedaSelected2.EsAccion Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1} No se puede trasladar las custodias, si las especies son de Tipo diferente.", strMensajeValidacion, vbCrLf)

                End If
            End If


            If RealizarSplits = False Then
                If (String.IsNullOrEmpty(strCodigoOyD1) And String.IsNullOrEmpty(strCodigoOyD2)) And ((strEspecie1 = CamposBusquedaSelected2.Nemotecnico) And (strCodigoOyD1 = strCodigoOyD2)) Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1} El traslado de las custodias no se puede afectar sobre la misma Especie. Debe marcar como Split.", strMensajeValidacion, vbCrLf)
                End If


                If (strCodigoOyD1 = strCodigoOyD2) And (Not String.IsNullOrEmpty(strCodigoOyD1) And Not String.IsNullOrEmpty(strCodigoOyD2)) And (strEspecie1 = strEspecie2) Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1} El traslado de las custodias no se puede afectar sobre el mismo cliente.", strMensajeValidacion, vbCrLf)
                End If

                'JAEZ 20161110 se comenta validación por que no aplica
                'If (strCodigoOyD1 <> strCodigoOyD2) And (Not String.IsNullOrEmpty(strCodigoOyD1) And Not String.IsNullOrEmpty(strCodigoOyD2)) And (strEspecie1 <> strEspecie2) Then
                '    logValidacion = False
                '    strMensajeValidacion = String.Format("{0}{1} El traslado de las custodias no se puede afectar sobre un cliente diferente y una especie diferente.", strMensajeValidacion, vbCrLf)
                'End If

                ' Se quita esta validacion por peticion de Asesores en Valores, caso 6786, Santiago Mazo 06 de Agosto 2013
                'If String.IsNullOrEmpty(strEspecie1) And String.IsNullOrEmpty(strEspecie2) Then
                '    If String.IsNullOrEmpty(strCodigoOyD1) Or String.IsNullOrEmpty(strCodigoOyD2) Then
                '        logValidacion = False
                '        strMensajeValidacion = String.Format("{0}{1} No se puede trasladar las custodias, si no se selecciona el cliente al cual se le van a cargar.", strMensajeValidacion, vbCrLf)
                '    End If
                'End If
                'If String.IsNullOrEmpty(strCodigoOyD1) Or String.IsNullOrEmpty(strCodigoOyD2) And (strEspecie1 <> strEspecie2) Then
                '    logValidacion = False
                '    strMensajeValidacion = String.Format("{0}{1} No se puede trasladar las custodias, si no selecciona cliente y las especies son diferentes. Marcar como Split.", strMensajeValidacion, vbCrLf)
                'End If
                'If String.IsNullOrEmpty(strCodigoOyD1) Then
                '    logValidacion = False
                '    strMensajeValidacion = String.Format("{0}{1} Cliente: Debe seleccionar cliente inicial y cliente al cual se le cargaran las custodias.", strMensajeValidacion, vbCrLf)
                'Else
                '    If String.IsNullOrEmpty(strCodigoOyD2) Then
                '        logValidacion = False
                '        strMensajeValidacion = String.Format("{0}{1} Cliente Final: Es necesario seleccionar el cliente al cual se le cargaran las custodias.", strMensajeValidacion, vbCrLf)
                '    End If
                '    If strCodigoOyD2 = strCodigoOyD1 Then
                '        logValidacion = False
                '        strMensajeValidacion = String.Format("{0}{1} Clientes Iguales: El traslado de las custodias no se puede afectuar sobre el mismo cliente.", strMensajeValidacion, vbCrLf)
                '    End If
                'End If
            Else
                'If String.IsNullOrEmpty(SelectedISIN) And (strEspecie1 = "" And strEspecie2 = "") Then
                '    logValidacion = False
                '    strMensajeValidacion = String.Format("{0}{1} Si selecciona el campo <<A la Especie>>  debe también ingresar datos en la lista despegable ISIN. ", strMensajeValidacion, vbCrLf)
                'End If
                'If (strEspecie1 = strEspecie2) Then
                '    Return True
                'End If

                'If String.IsNullOrEmpty(SelectedISIN) Then
                '    logValidacion = False
                '    strMensajeValidacion = String.Format("{0}{1} Debe ingresar datos en la lista despegable ISIN.", strMensajeValidacion, vbCrLf)
                'End If


                If (String.IsNullOrEmpty(strEspecie2) And String.IsNullOrEmpty(strEspecie2)) And (String.IsNullOrEmpty(strCodigoOyD1) And String.IsNullOrEmpty(strCodigoOyD1)) Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1} Los campos De la Especie y A la especie No puede estar en blanco.", strMensajeValidacion, vbCrLf)
                End If


                If IsNothing(dblFactor) Or dblFactor = 0 Then
                    logValidacion = False
                    strMensajeValidacion = String.Format("{0}{1} El factor de transferencia debe ser mayor que 0.", strMensajeValidacion, vbCrLf)
                End If

            End If
            If RealizarSplits Then

            End If



            If IsNothing(SelectedEstadoConcepto) Then
                logValidacion = False
                strMensajeValidacion = String.Format("{0}{1} El Concepto es requerido.", strMensajeValidacion, vbCrLf)

            End If

            If logValidacion = False Then
                IsBusy = False
                mostrarMensaje("Validaciones:" & vbCrLf & strMensajeValidacion, "", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
            Else
                strMensajeValidacion = String.Empty
                Return True
            End If


        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en las Validaciones.", _
                                Me.ToString(), "Validaciones", Application.Current.ToString(), Program.Maquina, ex)
            Return False

        End Try
    End Function
    Public Sub Recalcular()
        Try
            If Not IsNothing(ListaDetalleCustodia) Then
                If RealizarSplits Then
                    'JAEZ 20161202  Se recalcula solo para los seleccionados
                    For Each li In ListaDetalleCustodia.Where(Function(i) i.logGenerar = True)
                        li.dblCantidaRecalculo = li.Cantidad * dblFactor
                        li.Precio = dblPrecio
                    Next

                    VerRecalculo = Visibility.Visible

                Else
                    VerRecalculo = Visibility.Collapsed

                End If

            Else
                VerRecalculo = Visibility.Collapsed
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema Recalculando.", _
                                Me.ToString(), "Recalcular", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub


    Public Sub ConsultarISIN()
        Try


            If Not IsNothing(CamposBusquedaSelected) And Not IsNothing(CamposBusquedaSelected2) Then

                If Not String.IsNullOrEmpty(strEspecie1) And String.IsNullOrEmpty(strEspecie2) Then
                    IsBusy = True
                    SelectedISIN = String.Empty
                    dcProxyEspecies.EspeciesISINFungibles.Clear()
                    ' eomc -- 09/26/2013 -- Se agrega string vacío como parámetro para el isin
                    'dcProxyMaestros.Load(dcProxyMaestros.EspeciesISINFungibleConsultarQuery(strEspecie2, Nothing), AddressOf TerminoTraerEspeciesISINFungible, Nothing)
                    dcProxyEspecies.Load(dcProxyEspecies.EspeciesISINFungibleConsultarQuery(strEspecie1, String.Empty, Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesISINFungible, Nothing)
                    ' eomc -- 09/26/2013 -- Fin                    

                ElseIf Not String.IsNullOrEmpty(strEspecie2) Then
                    IsBusy = True
                    SelectedISIN = String.Empty
                    dcProxyEspecies.EspeciesISINFungibles.Clear()
                    ' eomc -- 09/26/2013 -- Se agrega string vacío como parámetro para el isin
                    'dcProxyMaestros.Load(dcProxyMaestros.EspeciesISINFungibleConsultarQuery(strEspecie2, Nothing), AddressOf TerminoTraerEspeciesISINFungible, Nothing)
                    dcProxyEspecies.Load(dcProxyEspecies.EspeciesISINFungibleConsultarQuery(strEspecie2, String.Empty, Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesISINFungible, Nothing)
                    ' eomc -- 09/26/2013 -- Fin                    

                End If

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al tratar de Consultar los ISINES", _
                                 Me.ToString(), "ConsultarIsin", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'JAEZ 20161027 Metodo para consultar la tasa de conversión
    Public Sub ConsultarTasaConversion(ByVal pdtmFechaProceso As DateTime, ByVal pstrMoneda As String)
        Try

            If Not IsNothing(pdtmFechaProceso) And Not IsNothing(pdtmFechaProceso) Then

                IsBusy = True

                dcProxy.ConsultarTasaConversion(pdtmFechaProceso, pstrMoneda, Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarTasaConversion, Nothing)

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al tratar de Consultar TasaConversion", _
                                 Me.ToString(), "ConsultarTasaConversion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'JAEZ 20161027 cacturar la tasa de conversion, la cual fue consultada
    Private Sub TerminoConsultarTasaConversion(ByVal lo As InvokeOperation(Of Double))
        Try
            If Not lo.HasError Then
                dblTasaConversion = lo.Value
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en TerminoConsultarTasaConversion", _
                                  Me.ToString(), "TerminoConsultarTasaConversion", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en TerminoConsultarTasaConversion", _
                     Me.ToString(), "TerminoConsultarTasaConversion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub


    Public Sub ConsultarCuentasFondosDestino(ByVal pstrFondo As String)
        Try
            If Not IsNothing(pstrFondo) Then
                IsBusy = True

                dcProxy.ListadoCuentas.Clear()
                dcProxy.Load(dcProxy.TraerCuentasConsultarQuery(0, CamposBusquedaSelected2.CodigoCliente, strFondo, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentas, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al tratar de Consultar TasaConversion", _
                                 Me.ToString(), "ConsultarTasaConversion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Private Sub TerminoTraerCuentas(ByVal lo As LoadOperation(Of CFPortafolio.ListadoCuenta))
        Try
            If Not lo.HasError Then

                strCuentaDestino = Nothing
                listaCuentas = Nothing
                listaCuentas = dcProxy.ListadoCuentas.ToList
                If (listaCuentas.Count > 0) Then
                    Dim primeraCuenta = listaCuentas.FirstOrDefault
                    strCuentaDestino = primeraCuenta.IdCuentaDeceval
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de las cuentas", _
                                                             Me.ToString(), "TerminoTraerCuentas", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de las cuentas", _
                                             Me.ToString(), "TerminoTraerCuentas", Application.Current.ToString(), Program.Maquina, lo.Error)
        End Try

        IsBusy = False
    End Sub


    Public Sub Generar()
        Try
            If Validaciones() Then
                mostrarMensajePregunta("¿Esta Seguro de generar los nuevos traslados?", _
                                       Program.TituloSistema, _
                                       "Generación", _
                                       AddressOf TerminoConfirmarGeneracion, False)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al tratar de Generar", _
                                 Me.ToString(), "Generar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <history>
    ''' Descripción:  Se valida la fecha del proceso. Cambio autorizado por Jorge Arango.
    ''' Responsable:  Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:        16 de Octubre/2015
    ''' </history>
    ''' <remarks></remarks>
    Public Sub ConsultarPortafolio()
        Try
            If IsNothing(dtmFechaProceso) Then
                mostrarMensaje("La fecha del proceso es un campo requerido.", "", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If dtmFechaProceso > Now.Date Then
                mostrarMensaje("La fecha del proceso no puede ser mayor a la fecha actual.", "", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If String.IsNullOrEmpty(strEspecie1) And String.IsNullOrEmpty(strCodigoOyD1) Then
                mostrarMensaje("No es posible realizar la consulta debe seleccionar un cliente <<Del código>> ó cliente <<Del código>> y especie <<De la Especie>>", "", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                If RealizarSplits = False Then
                    VerRecalculo = Visibility.Collapsed
                End If
                IsBusy = True
                dtmFechaProcesoSeleccionada = dtmFechaProceso
                dcProxy.DetalleCustodias.Clear()
                dcProxy.Load(dcProxy.ConsultarPortafolioQuery(strEspecie1, strCodigoOyD1, Program.Usuario, strISINFungible, dtmFechaProceso, Program.HashConexion), AddressOf TerminoTraerPortafolio, "Consultar")
            End If

        Catch ex As Exception

            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Consultar los detalles de custodias", _
                                 Me.ToString(), "ConsultarPortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub EspecieSeleccionadaM(ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(pobjEspecie) Then
                If (_mlogBuscarEspecie) Then
                    strEspecie1 = pobjEspecie.Nemotecnico
                    CamposBusquedaSelected.Especie = pobjEspecie.Especie
                    CamposBusquedaSelected.EsAccion = pobjEspecie.EsAccion
                    HabilitarConsultar = True
                    IsBusy = False

                    If _mlogBuscarEspecie = True Then
                        ConsultarISIN()
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Consultar el Especie", _
                                 Me.ToString(), "EspecieSeleccionadaM", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Sub EspecieSeleccionadaM2(ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(pobjEspecie) Then
                If (_mlogBuscarEspecie2) Then
                    strEspecie2 = pobjEspecie.Nemotecnico
                    CamposBusquedaSelected2.Especie = pobjEspecie.Especie
                    CamposBusquedaSelected2.EsAccion = pobjEspecie.EsAccion
                    If RealizarSplits Then
                        ConsultarISIN()
                    End If
                    IsBusy = False
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Consultar el Especie", _
                                 Me.ToString(), "EspecieSeleccionadaM2", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub EspecieSeleccionada2M(ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)
        Try
            If Not IsNothing(pobjEspecie) Then
                If (_mlogBuscarEspecie2) Then
                    strEspecie1 = pobjEspecie.Nemotecnico
                    CamposBusquedaSelected2.Especie = pobjEspecie.Especie
                    CamposBusquedaSelected2.EsAccion = pobjEspecie.EsAccion
                    IsBusy = False
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Consultar el Especie", _
                                 Me.ToString(), "EspecieSeleccionada2M", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    ''' <summary>Autocompletar información del cliente</summary>
    ''' <param name="pobjComitente"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' Agregado por     : Jhon bayron torres .
    ''' Descripción      : Según el código del cliente digitado por el usuario, se autocompleta el nombre.
    ''' Fecha            : Mayo 17/2013
    ''' Pruebas CB       : Jhon bayron torres  - Mayo 17/2013 - Resultado Ok 
    ''' </history>
    Sub ComitenteSeleccionadoM(ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        Try
            If Not IsNothing(pobjComitente) Then
                If (_mlogBuscarCliente) Then
                    strCodigoOyD1 = pobjComitente.CodigoOYD
                    _CamposBusquedaSelected.NombreCliente = pobjComitente.NombreCodigoOYD
                    HabilitarConsultar = True
                    IsBusy = False
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Consultar el cliente", _
                                 Me.ToString(), "ComitenteSeleccionadoM", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Sub ComitenteSeleccionadoM2(ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            If (_mlogBuscarCliente2) Then
                strCodigoOyD2 = pobjComitente.CodigoOYD
                _CamposBusquedaSelected2.NombreCliente = pobjComitente.NombreCodigoOYD
                IsBusy = False
            End If
        End If
    End Sub
    Public Sub ISINFungibleSeleccionada(ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                strISINFungible = pobjItem.IdItem
                CamposBusquedaSelected.IsinFungible = pobjItem.IdItem
                CamposBusquedaSelected.DescripcionIsinFungible = pobjItem.Descripcion

                If CamposBusquedaSelected.Nemotecnico <> pobjItem.InfoAdicional01 Then
                    strEspecie1 = pobjItem.InfoAdicional01
                    CamposBusquedaSelected.Nemotecnico = pobjItem.InfoAdicional01
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al el Isin Fungible", _
                                 Me.ToString(), "ISINFungibleSeleccionada", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <history>
    ''' Agregado por     : Jhon bayron torres .
    ''' Descripción      : LimpiarCamposBusqueda()
    ''' Fecha            : Mayo 17/2013
    ''' Pruebas CB       : Jhon bayron torres  - Mayo 17/2013 - Resultado Ok 
    ''' </history>
    ''' 

    Sub LimpiarCamposBusqueda()
        CamposBusquedaSelected.CodigoCliente = String.Empty
        CamposBusquedaSelected.NombreCliente = String.Empty
    End Sub

    Sub LimpiarCamposBusqueda2()
        CamposBusquedaSelected2.CodigoCliente = String.Empty
        CamposBusquedaSelected2.NombreCliente = String.Empty
    End Sub
    Sub LimpiarCamposBusquedaEspecie()
        CamposBusquedaSelected.Nemotecnico = String.Empty
    End Sub
    Sub LimpiarCamposBusquedaEspecie2()
        CamposBusquedaSelected2.Nemotecnico = String.Empty
    End Sub
    Friend Sub buscarEspecie2(Optional ByVal pstrNemotecnico As String = "", Optional ByVal pstrBusqueda As String = "")
        Dim strNemotecnico As String = String.Empty
        Try
            If (_mlogBuscarEspecie2) Then
                If Not Me.CamposBusquedaSelected2 Is Nothing Then
                    If Not strNemotecnico.Equals(Me.CamposBusquedaSelected2.Nemotecnico) Then
                        If pstrNemotecnico.Trim.Equals(String.Empty) Then
                            strNemotecnico = Me.CamposBusquedaSelected2.Nemotecnico
                        Else
                            strNemotecnico = pstrNemotecnico
                        End If
                    End If
                End If
            End If

            If Not strNemotecnico Is Nothing AndAlso Not strNemotecnico.Trim.Equals(String.Empty) Then
                objProxyUtilidades.BuscadorEspecies.Clear()
                objProxyUtilidades.Load(objProxyUtilidades.buscarEspeciesQuery(strNemotecnico, "T", "A", "nemotecnico", Program.Usuario, Program.HashConexion), AddressOf buscarEspecieCompleted2, pstrBusqueda)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la especie", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Friend Sub buscarEspecie(Optional ByVal pstrNemotecnico As String = "", Optional ByVal pstrBusqueda As String = "")
        Dim strNemotecnico As String = String.Empty
        Try
            If (_mlogBuscarEspecie) Then
                If Not Me.CamposBusquedaSelected Is Nothing Then
                    If Not strNemotecnico.Equals(Me.CamposBusquedaSelected.Nemotecnico) Then
                        If pstrNemotecnico.Trim.Equals(String.Empty) Then
                            strNemotecnico = Me.CamposBusquedaSelected.Nemotecnico
                        Else
                            strNemotecnico = pstrNemotecnico
                        End If
                    End If
                End If
            End If

            If Not strNemotecnico Is Nothing AndAlso Not strNemotecnico.Trim.Equals(String.Empty) Then
                objProxyUtilidades.BuscadorEspecies.Clear()
                objProxyUtilidades.Load(objProxyUtilidades.buscarEspeciesQuery(strNemotecnico, "T", "A", "nemotecnico", Program.Usuario, Program.HashConexion), AddressOf buscarEspecieCompleted, pstrBusqueda)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la especie", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Friend Sub buscarGenerico(Optional ByVal pstrCondicionBusqueda As String = "", Optional ByVal pstrBusqueda As String = "", Optional pstrAgrupacion As String = "")
        Try
            If Not String.IsNullOrEmpty(pstrCondicionBusqueda) Then
                objProxyUtilidades.BuscadorGenericos.Clear()
                objProxyUtilidades.Load(objProxyUtilidades.buscarItemsQuery(pstrCondicionBusqueda, pstrBusqueda, "A", pstrAgrupacion, "", "", Program.Usuario, Program.HashConexion), AddressOf buscarItemCompleted, pstrBusqueda)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar", Me.ToString(), "buscar", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Buscar los datos del comitente
    ''' </summary>
    ''' <param name="pstrIdComitente">Comitente que se debe buscar</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' Agregado por     : Juan David Osorio .
    ''' Descripción      : 
    ''' Fecha            : julio 2013
    ''' Pruebas CB       :  Juan David Osorio .
    ''' </history>
    Friend Sub buscarComitente(Optional ByVal pstrIdComitente As String = "", Optional ByVal pstrBusqueda As String = "")
        Dim strIdComitente As String = String.Empty
        Try
            If (_mlogBuscarCliente) Then
                If Not Me.CamposBusquedaSelected Is Nothing Then
                    If Not strIdComitente.Equals(Me.CamposBusquedaSelected.CodigoCliente) Then
                        If pstrIdComitente.Trim.Equals(String.Empty) Then
                            strIdComitente = Me.CamposBusquedaSelected.CodigoCliente
                        Else
                            strIdComitente = pstrIdComitente
                        End If
                    End If
                End If
            End If

            If Not strIdComitente Is Nothing AndAlso Not strIdComitente.Trim.Equals(String.Empty) Then
                objProxyUtilidades.BuscadorClientes.Clear()
                objProxyUtilidades.Load(objProxyUtilidades.buscarClienteEspecificoQuery(strIdComitente, Program.Usuario, "IdComitente", Program.HashConexion), AddressOf buscarComitenteCompleted, pstrBusqueda)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Friend Sub buscarComitente2(Optional ByVal pstrIdComitente As String = "", Optional ByVal pstrBusqueda As String = "")
        Dim strIdComitente As String = String.Empty
        Try
            If (_mlogBuscarCliente2) Then
                If Not Me.CamposBusquedaSelected2 Is Nothing Then
                    If Not strIdComitente.Equals(Me.CamposBusquedaSelected2.CodigoCliente) Then
                        If pstrIdComitente.Trim.Equals(String.Empty) Then
                            strIdComitente = Me.CamposBusquedaSelected2.CodigoCliente
                        Else
                            strIdComitente = pstrIdComitente
                        End If
                    End If
                End If
            End If

            If Not strIdComitente Is Nothing AndAlso Not strIdComitente.Trim.Equals(String.Empty) Then
                objProxyUtilidades.BuscadorClientes.Clear()
                objProxyUtilidades.Load(objProxyUtilidades.buscarClienteEspecificoQuery(strIdComitente, Program.Usuario, "IdComitente", Program.HashConexion), AddressOf buscarComitenteCompleted2, pstrBusqueda)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Async Function ConsultarConceptoTitulo() As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFMaestros.EstadosConceptoTitulos)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            dcProxyMaestros.EstadosConceptoTitulos.Clear()
            objRet = Await dcProxyMaestros.Load(dcProxyMaestros.FiltrarEstadosConceptoTitulosSyncQuery(pstrFiltro:=String.Empty, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()


            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "ConsultarEncabezado", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    ListaEstadoConcepto = dcProxyMaestros.EstadosConceptoTitulos.ToList

                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de EstadosConceptoTitulos ", Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' JP20151210: Consultar fecha de cierre del portafolio para validar la generación del proceso.
    ''' </summary>
    Public Async Function ObtenerFechaCierrePortafolio(ByVal pstrIdComitente As String) As Task(Of DateTime?)
        Dim dtmFechaCierre As DateTime? = Nothing

        Try
            If String.IsNullOrEmpty(pstrIdComitente) Then
                Return (Nothing)
            End If

            Dim objRet As InvokeOperation(Of DateTime?)
            Dim objProxyUtil As UtilidadesCFDomainContext

            objProxyUtil = inicializarProxyUtilidades()

            objRet = Await objProxyUtil.ConsultarFechaCierrePortafolioSync(pstrIdComitente, Program.Usuario, Program.HashConexion).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar la fecha de cierre del portafolio del cliente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If Not IsNothing(objRet.Value) Then
                        dtmFechaCierre = objRet.Value
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la fecha de cierre del portafolio. ", Me.ToString(), "ObtenerFechaCierrePortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return dtmFechaCierre
    End Function


#End Region

#Region "Resultados Asincronicos"

    Public Sub TerminoTraerEspeciesISINFungible(ByVal lo As LoadOperation(Of EspeciesISINFungible))
        Try
            If Not lo.HasError Then
                ListaIsin = dcProxyEspecies.EspeciesISINFungibles.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer los ISINs", Me.ToString(), "TerminoTraerEspeciesISINFungible", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                lo.MarkErrorAsHandled()
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer los ISINs", _
                                 Me.ToString(), "TerminoTraerEspeciesISINFungible", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub




    ''' <history>
    '''Descripción:   Se envía el parámetro dblCantidadTrasladar
    '''Responsable:   Jorge Peña (Alcuadrad S.A.)
    '''Fecha:         5 de Otubre/2015
    '''Pruebas CB:    Jorge Peña (Alcuadrad S.A.) - 5 de Otubre/2015 - Resultados OK
    ''' </history>
    ''' <history>
    ''' Descripción:  Se valida la fecha del proceso. Cambio autorizado por Jorge Arango.
    ''' Responsable:  Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:        16 de Octubre/2015
    ''' </history>
    ''' <remarks></remarks>
    Public Sub TerminoConfirmarGeneracion(ByVal sender As Object, ByVal e As EventArgs)
        Try

            'JAEZ Se documenta las lineas ya que  afeacta el formato de toda la consola
            'System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
            'System.Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo("en-US")

            Dim dtmEmision As String = ""
            Dim dtmVencimiento As String = ""
            Dim dtmRetencion As String = ""
            Dim dtmSellado As String = ""
            Dim dtmLiquidacion As String = ""
            Dim dtmCumplimientoTitulo As String = ""
            Dim logpermitirThen As Boolean = True
            Dim strRegistrosDetalle = ""
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                If Not IsNothing(ListaDetalleCustodia) Then
                    If ListaDetalleCustodia.Count > 0 Then
                        For Each li In ListaDetalleCustodia.Where(Function(i) i.logGenerar = True)
                            'CCM20151119: Eliminar la asinación del ISIN, Nemo y Cantidad con los valores destino
                            'If RealizarSplits Then
                            '    li.Cantidad = li.dblCantidaRecalculo
                            '    li.IdEspecie = strEspecie2
                            '    'SLB20131206 
                            '    li.ISIN = SelectedISIN
                            'End If
                            'JAEZ 20161219 se ajusta formato de fechas por dd/MM/yyyy
                            If logpermitirThen Then
                                dtmEmision = Format(li.Emision, "dd/MM/yyyy")
                                dtmVencimiento = Format(li.Vencimiento, "dd/MM/yyyy")
                                dtmRetencion = Format(li.Retencion, "dd/MM/yyyy")
                                dtmSellado = Format(li.Sellado, "dd/MM/yyyy")
                                dtmLiquidacion = Format(li.Liquidacion, "dd/MM/yyyy")
                                dtmCumplimientoTitulo = Format(li.CumplimientoTitulo, "dd/MM/yyyy")


                                strRegistrosDetalle = String.Format("%{0}%**%{1}%**%{2}%**%{3}%**%{4}%**%{5}%**%{6}%**%{7}%**%{8}%**%{9}%**%{10}%**%{11}%**%{12}%**%{13}%**%{14}%**%{15}%**%{16}%**%{17}%**%{18}%**%{19}%**%{20}%**%{21}%**%{22}%**%{23}%**%{24}%**%{25}%**%{26}%**%{27}%**%{28}%**%{29}%**%{30}%**%{31}%**%{32}%**%{33}%**%{34}%**%{35}%**%{36}%**%{37}%**%{38}%**%{39}%**%{40}%**%{41}%**%{42}%**%{43}%**%{44}%**%{45}%**%{46}%**%{47}%**%{48}%**%{49}%**%{50}%**%{51}%**%{52}%**%{53}%**%{54}%", _
                                                                   li.Comitente, li.IdRecibo, li.Secuencia, li.IdEspecie, li.NroTitulo, _
                                                                    li.RentaVariable, li.IndicadorEconomico, li.PuntosIndicador, li.DiasVencimiento, _
                                                                    li.Modalidad, dtmEmision, dtmVencimiento, li.Cantidad, li.Fondo, li.TasaInteres, li.NroRefFondo, _
                                                                    dtmRetencion, li.TasaRetencion, li.ValorRetencion, li.PorcRendimiento, li.IdAgenteRetenedor, li.ObjVenta, _
                                                                    li.ObjCobroIntDiv, li.ObjRenovReinv, li.ObjSuscripcion, li.ObjCancelacion, dtmSellado, li.IdCuentaDeceval, li.ISIN, _
                                                                    li.Fungible, li.TipoValor, li.FechasPagoRendimientos, li.IDDepositoExtranjero, li.IDCustodio, li.TitularCustodio, li.Reinversion, _
                                                                    li.IDLiquidacion, li.Parcial, li.ClaseLiquidacion, li.TipoLiquidacion, dtmLiquidacion, li.TotalLiq, li.TasaCompraVende, dtmCumplimientoTitulo, _
                                                                    li.TasaDescuento, li.Precio, li.dblCantidadTrasladar, strEspecie2, SelectedISIN, dblFactor, logConvertirMoneda, strMoneda, dblTasaConversion, strFondo, strCuentaDestino)
                                logpermitirThen = False
                            Else
                                strRegistrosDetalle = String.Format("{0}|%{1}%**%{2}%**%{3}%**%{4}%**%{5}%**%{6}%**%{7}%**%{8}%**%{9}%**%{10}%**%{11}%**%{12}%**%{13}%**%{14}%**%{15}%**%{16}%**%{17}%**%{18}%**%{19}%**%{20}%**%{21}%**%{22}%**%{23}%**%{24}%**%{25}%**%{26}%**%{27}%**%{28}%**%{29}%**%{30}%**%{31}%**%{32}%**%{33}%**%{34}%**%{35}%**%{36}%**%{37}%**%{38}%**%{39}%**%{40}%**%{41}%**%{42}%**%{43}%**%{44}%**%{45}%**%{46}%**%{47}%**%{48}%**%{49}%**%{50}%**%{51}%**%{52}%**%{53}%**%{54}%**%{55}%", _
                                                                    strRegistrosDetalle, _
                                                                   li.Comitente, li.IdRecibo, li.Secuencia, li.IdEspecie, li.NroTitulo, _
                                                                    li.RentaVariable, li.IndicadorEconomico, li.PuntosIndicador, li.DiasVencimiento, _
                                                                    li.Modalidad, dtmEmision, dtmVencimiento, li.Cantidad, li.Fondo, li.TasaInteres, li.NroRefFondo, _
                                                                    dtmRetencion, li.TasaRetencion, li.ValorRetencion, li.PorcRendimiento, li.IdAgenteRetenedor, li.ObjVenta, _
                                                                    li.ObjCobroIntDiv, li.ObjRenovReinv, li.ObjSuscripcion, li.ObjCancelacion, dtmSellado, li.IdCuentaDeceval, li.ISIN, _
                                                                    li.Fungible, li.TipoValor, li.FechasPagoRendimientos, li.IDDepositoExtranjero, li.IDCustodio, li.TitularCustodio, li.Reinversion, _
                                                                    li.IDLiquidacion, li.Parcial, li.ClaseLiquidacion, li.TipoLiquidacion, dtmLiquidacion, li.TotalLiq, li.TasaCompraVende, dtmCumplimientoTitulo, _
                                                                    li.TasaDescuento, li.Precio, li.dblCantidadTrasladar, strEspecie2, SelectedISIN, dblFactor, logConvertirMoneda, strMoneda, dblTasaConversion, strFondo, strCuentaDestino)
                            End If
                        Next
                        logpermitirThen = False
                        IsBusy = True

                        dcProxy.ActualizarTitulos(strRegistrosDetalle, strCodigoOyD2, RealizarSplits, Program.Usuario, SelectedEstadoConcepto.intIDEstadosConceptoTitulos, strTipoRedondeo, dtmFechaProceso, Program.HashConexion, AddressOf TerminoActualizarTitulos, "")

                    Else
                        mostrarMensaje("Para generar traslados de custodias es necesario que existan registros", "", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                Else
                    mostrarMensaje("Para generar traslados de custodias es necesario que existan registros", "", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If


            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al confirmar generación", _
                                 Me.ToString(), "TerminoConfirmarGeneracion", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub
    Private Sub TerminoActualizarTitulos(lo As InvokeOperation(Of Integer))
        Try
            If Not lo.HasError Then
                IsBusy = False
                dcProxy.DetalleCustodias.Clear()
                HabilitarConsultar = False
                CamposBusquedaSelected.CodigoCliente = String.Empty
                CamposBusquedaSelected2.CodigoCliente = String.Empty
                CamposBusquedaSelected.Nemotecnico = String.Empty
                CamposBusquedaSelected2.Nemotecnico = String.Empty
                dblFactor = 1
                dblPrecio = 0
                SelectedISIN = String.Empty
                VerRecalculo = Visibility.Collapsed
                RealizarSplits = False
                ListaDetalleCustodia = Nothing
                SeleccionarTodos = False
                CamposBusquedaSelected.IsinFungible = String.Empty
                CamposBusquedaSelected.strConceptoTitulo = String.Empty
                CamposBusquedaSelected.strEstadoEntrada = String.Empty
                CamposBusquedaSelected.strEstadoSalida = String.Empty
                CamposBusquedaSelected.Especie = String.Empty
                CamposBusquedaSelected2.Especie = String.Empty

                'JAEZ 20161031-- Se limpian los campos 
                strFondo = String.Empty
                strCuentaDestino = Nothing
                logConvertirMoneda = False
                strMoneda = String.Empty
                dblTasaConversion = 0
                SelectedEstadoConcepto = Nothing




                A2Utilidades.Mensajes.mostrarMensaje("Se ejecuto correctamente el proceso de actualización de Títulos", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualizacion de titulos", _
                                                Me.ToString(), "TerminoActualizarTitulos", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualizacion de titulos", _
                                                Me.ToString(), "TerminoActualizarTitulos", Application.Current.ToString(), Program.Maquina, lo.Error)
        End Try
    End Sub

    Public Sub TerminoTraerPortafolio(ByVal lo As LoadOperation(Of DetalleCustodia))
        Try
            If Not lo.HasError Then
                ListaDetalleCustodia = dcProxy.DetalleCustodias.ToList
                For Each li In ListaDetalleCustodia
                    li.logGenerar = False
                Next
                Recalcular()
                If ListaDetalleCustodia.Count > 0 Then
                    HabilitarSeleccionarTodos = True

                Else
                    HabilitarSeleccionarTodos = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se consultaron las custodias.", Me.ToString(), "CustodiasConsultarCompleted", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                lo.MarkErrorAsHandled()
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer los detalles de custodias", _
                                 Me.ToString(), "TerminoTraerPortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try


    End Sub

    Private Sub buscarEspecieCompleted2(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorEspecies))
        Try
            If (_mlogBuscarEspecie2) Then
                If lo.Entities.ToList.Count > 0 Then
                    If lo.Entities.ToList.Item(0).Activo = 0 Then
                        IsBusy = False
                        A2Utilidades.Mensajes.mostrarMensaje("la especie que esta ingresado en el encabezado se encuentra inactiva", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        LimpiarCamposBusquedaEspecie2()

                    Else
                        Me.EspecieSeleccionadaM2(lo.Entities.ToList.Item(0))
                    End If
                Else
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarMensaje("la especie ingresada en el encabezado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    LimpiarCamposBusquedaEspecie2()
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la especie", Me.ToString(), "buscarEspecieCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Private Sub buscarEspecieCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorEspecies))
        Try
            If (_mlogBuscarEspecie) Then
                If lo.Entities.ToList.Count > 0 Then
                    If lo.Entities.ToList.Item(0).Activo = 0 Then
                        IsBusy = False
                        A2Utilidades.Mensajes.mostrarMensaje("la especie que esta ingresado en el encabezado se encuentra inactiva", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        LimpiarCamposBusquedaEspecie()

                    Else
                        Me.EspecieSeleccionadaM(lo.Entities.ToList.Item(0))
                    End If
                Else
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarMensaje("la especie ingresada en el encabezado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    LimpiarCamposBusquedaEspecie()
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la especie", Me.ToString(), "buscarEspecieCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    ''' <summary>
    ''' Agregado por     : Jhon bayron torres .
    ''' Descripción      : Se obtiene el resultado de buscar el cliente cuando se digita desde el control
    ''' Fecha            : Mayo 17/2013
    ''' Pruebas CB       : Jhon bayron torres  - Abril 17/2013 - Resultado Ok 
    ''' </summary>
    ''' <param name="lo"></param>
    Private Sub buscarComitenteCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            If (_mlogBuscarCliente) Then
                If lo.Entities.ToList.Count > 0 Then
                    If lo.Entities.ToList.Item(0).Estado.ToLower = "inactivo" Or lo.Entities.ToList.Item(0).Estado.ToLower = "bloqueado" Then
                        IsBusy = False
                        A2Utilidades.Mensajes.mostrarMensaje("El cliente ingresado en el encabezado se encuentra " & lo.Entities.ToList.Item(0).Estado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        LimpiarCamposBusqueda()

                    Else
                        Me.ComitenteSeleccionadoM(lo.Entities.ToList.Item(0))
                    End If
                Else
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarMensaje("El cliente ingresado en el encabezado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    LimpiarCamposBusqueda()
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del comitente de la orden", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Private Sub buscarComitenteCompleted2(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            If (_mlogBuscarCliente2) Then
                If lo.Entities.ToList.Count > 0 Then
                    If lo.Entities.ToList.Item(0).Estado.ToLower = "inactivo" Or lo.Entities.ToList.Item(0).Estado.ToLower = "bloqueado" Then
                        A2Utilidades.Mensajes.mostrarMensaje("El cliente ingresado en el encabezado se encuentra " & lo.Entities.ToList.Item(0).Estado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        LimpiarCamposBusqueda2()
                    Else
                        Me.ComitenteSeleccionadoM2(lo.Entities.ToList.Item(0))
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("El cliente ingresado en el encabezado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    LimpiarCamposBusqueda2()
                End If
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del comitente de la orden", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub buscarItemCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Try
            If lo.UserState = "ISINFUNGIBLE" Then
                If lo.Entities.ToList.Count > 0 Then
                    _mlogBuscarISINFungible = False
                    ISINFungibleSeleccionada(lo.Entities.First)
                    _mlogBuscarISINFungible = True
                Else
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarMensaje("la isin ingresado en el encabezado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    LimpiarCamposBusquedaEspecie()
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la especie", Me.ToString(), "buscarEspecieCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ContarDecimales()

        Dim strCantidad As String
        Dim strPos As String

        For Each li In ListaDetalleCustodia

            strCantidad = li.Cantidad.ToString

            If InStr(1, strCantidad, ".") <> 0 Then

                strPos = InStr(1, strCantidad, ".")
                strCantidad = strCantidad.Substring(strPos)

                If Not IsNothing(strCantidad) And Len(strCantidad) <> 0 Then
                    If Len(strCantidad) > 3 And Len(strCantidad) < 9 Then
                        li.NroDecimal = "" & Len(strCantidad)
                    ElseIf Len(strCantidad) >= 9 Then
                        li.NroDecimal = "9"
                    Else
                        li.NroDecimal = "4"
                    End If
                Else
                    li.NroDecimal = "4"
                End If
            Else
                li.NroDecimal = "4"
            End If
        Next

    End Sub
    Public Sub ContarDecimalesSelected()

        Dim strCantidad As String
        Dim strPos As String

        strCantidad = SelectedDetalleCustodia.dblCantidadTrasladar.ToString

        If InStr(1, strCantidad, ".") <> 0 Then

            strPos = InStr(1, strCantidad, ".")
            strCantidad = strCantidad.Substring(strPos)

            If Not IsNothing(strCantidad) And Len(strCantidad) <> 0 Then
                If Len(strCantidad) > 3 And Len(strCantidad) < 9 Then
                    SelectedDetalleCustodia.NroDecimal = "" & Len(strCantidad)
                ElseIf Len(strCantidad) >= 9 Then
                    SelectedDetalleCustodia.NroDecimal = "9"
                Else
                    SelectedDetalleCustodia.NroDecimal = "4"
                End If
            Else
                SelectedDetalleCustodia.NroDecimal = "4"
            End If

        Else
            SelectedDetalleCustodia.NroDecimal = "4"
        End If
    End Sub

#End Region


    Private Async Sub _CamposBusquedaSelected_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _CamposBusquedaSelected.PropertyChanged
        Try
            Select Case e.PropertyName

                Case "CodigoCliente"
                    If Not String.IsNullOrEmpty(LTrim(RTrim(_CamposBusquedaSelected.CodigoCliente))) And _mlogBuscarCliente Then
                        IsBusy = True
                        ListaDetalleCustodia = Nothing
                        HabilitarSeleccionarTodos = False
                        HabilitarConsultar = False
                        buscarComitente(_CamposBusquedaSelected.CodigoCliente)
                        mdtmFechaCierre_Del_Codigo = Await ObtenerFechaCierrePortafolio(_CamposBusquedaSelected.CodigoCliente)
                    ElseIf String.IsNullOrEmpty(LTrim(RTrim(_CamposBusquedaSelected.CodigoCliente))) Then
                        ListaDetalleCustodia = Nothing
                        strCodigoOyD1 = String.Empty
                        _CamposBusquedaSelected.NombreCliente = String.Empty

                    End If
                Case "Nemotecnico"
                    If Not String.IsNullOrEmpty(_CamposBusquedaSelected.Nemotecnico) And _mlogBuscarEspecie Then
                        IsBusy = True
                        ListaDetalleCustodia = Nothing
                        HabilitarSeleccionarTodos = False
                        HabilitarConsultar = False
                        buscarEspecie(_CamposBusquedaSelected.Nemotecnico)
                    ElseIf String.IsNullOrEmpty(LTrim(RTrim(_CamposBusquedaSelected.Nemotecnico))) Then
                        ListaDetalleCustodia = Nothing
                        strEspecie1 = String.Empty
                    End If
                Case "IsinFungible"
                    If Not String.IsNullOrEmpty(_CamposBusquedaSelected.IsinFungible) And _mlogBuscarISINFungible Then
                        IsBusy = True
                        ListaDetalleCustodia = Nothing
                        HabilitarSeleccionarTodos = False
                        HabilitarConsultar = False
                        buscarGenerico(_CamposBusquedaSelected.IsinFungible, "ISINFUNGIBLE")
                    ElseIf String.IsNullOrEmpty(LTrim(RTrim(_CamposBusquedaSelected.IsinFungible))) Then
                        ListaDetalleCustodia = Nothing
                        strISINFungible = String.Empty
                    End If

            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro", _
                                 Me.ToString(), "_CamposBusquedaSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Async Sub _CamposBusquedaSelected2_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _CamposBusquedaSelected2.PropertyChanged
        Try
            Select Case e.PropertyName

                Case "CodigoCliente"
                    If Not String.IsNullOrEmpty(_CamposBusquedaSelected2.CodigoCliente) And _mlogBuscarCliente2 Then
                        IsBusy = True
                        HabilitarSeleccionarTodos = False
                        buscarComitente2(_CamposBusquedaSelected2.CodigoCliente)
                        mdtmFechaCierre_Al_Codigo = Await ObtenerFechaCierrePortafolio(_CamposBusquedaSelected2.CodigoCliente)
                    ElseIf String.IsNullOrEmpty(LTrim(RTrim(_CamposBusquedaSelected2.CodigoCliente))) Then
                        strCodigoOyD2 = String.Empty
                        _CamposBusquedaSelected2.NombreCliente = String.Empty
                    End If

                Case "Nemotecnico"
                    If Not String.IsNullOrEmpty(LTrim(RTrim(_CamposBusquedaSelected2.Nemotecnico))) And _mlogBuscarEspecie2 Then
                        IsBusy = True
                        HabilitarSeleccionarTodos = False
                        buscarEspecie2(_CamposBusquedaSelected2.Nemotecnico)
                    ElseIf String.IsNullOrEmpty(LTrim(RTrim(_CamposBusquedaSelected2.Nemotecnico))) Then
                        ListaIsin = Nothing
                        SelectedISIN = String.Empty
                        strEspecie2 = String.Empty
                    End If

                    If RealizarSplits Then
                        VerSplit = Visibility.Visible
                    Else
                        VerSplit = Visibility.Collapsed
                    End If
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar en la edición del detalle registro", _
                                 Me.ToString(), "_CamposBusquedaSelected2_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
End Class

#Region "Propiedades Campos Búsqueda"
Public Class CamposBusquedaActualizarTitulos
    Implements INotifyPropertyChanged

    Private _CodigoCliente As String
    Public Property CodigoCliente() As String
        Get
            Return _CodigoCliente
        End Get
        Set(ByVal value As String)
            _CodigoCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CodigoCliente"))
        End Set
    End Property

    Private _NombreCliente As String
    Public Property NombreCliente() As String
        Get
            Return _NombreCliente
        End Get
        Set(ByVal value As String)
            _NombreCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreCliente"))
        End Set
    End Property

    Private _Nemotecnico As String
    Public Property Nemotecnico() As String
        Get
            Return _Nemotecnico
        End Get
        Set(ByVal value As String)
            _Nemotecnico = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nemotecnico"))
        End Set
    End Property

    Private _Especie As String
    Public Property Especie() As String
        Get
            Return _Especie
        End Get
        Set(ByVal value As String)
            _Especie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Especie"))
        End Set
    End Property

    Private _EsAccion As Boolean
    Public Property EsAccion() As Boolean
        Get
            Return _EsAccion
        End Get
        Set(ByVal value As Boolean)
            _EsAccion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("EsAccion"))
        End Set
    End Property

    Private _strConceptoTitulo As String
    Public Property strConceptoTitulo() As String
        Get
            Return _strConceptoTitulo
        End Get
        Set(ByVal value As String)
            _strConceptoTitulo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strConceptoTitulo"))
        End Set
    End Property

    Private _strEstadoEntrada As String
    Public Property strEstadoEntrada() As String
        Get
            Return _strEstadoEntrada
        End Get
        Set(ByVal value As String)
            _strEstadoEntrada = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strEstadoEntrada"))
        End Set
    End Property

    Private _strEstadoSalida As String
    Public Property strEstadoSalida() As String
        Get
            Return _strEstadoSalida
        End Get
        Set(ByVal value As String)
            _strEstadoSalida = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strEstadoSalida"))
        End Set
    End Property

    Private _IsinFungible As String
    Public Property IsinFungible() As String
        Get
            Return _IsinFungible
        End Get
        Set(ByVal value As String)
            _IsinFungible = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsinFungible"))
        End Set
    End Property

    Private _DescripcionIsinFungible As String
    Public Property DescripcionIsinFungible() As String
        Get
            Return _DescripcionIsinFungible
        End Get
        Set(ByVal value As String)
            _DescripcionIsinFungible = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DescripcionIsinFungible"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class

#End Region

#End Region