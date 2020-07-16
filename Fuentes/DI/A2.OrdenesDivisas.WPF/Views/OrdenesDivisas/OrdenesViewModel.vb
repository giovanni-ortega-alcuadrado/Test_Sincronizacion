' Desarrollo de órdenes y maestros de módulos genericos
' Santiago Alexander Vergara Orrego
' SV20180711_ORDENES

Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.ComponentModel
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Collections.ObjectModel
Imports Newtonsoft.Json

Public Class OrdenesViewModel
    Inherits A2ControlMenu.A2ViewModel


#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As tblOrdenes
    Private mdcProxy As OrdenesDivisasDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Public mobjEncabezadoAnterior As tblOrdenes
    Dim strTipoFiltroBusqueda As String = String.Empty

    Public objViewPrincipal As OrdenesView

    Dim OrdenesDivisasView As OrdenesDivisasView

    Public Enum TipoOperacion
        Inicial = 1
        Devolucion = 2
        Cambio = 3
        Modificacion = 4
    End Enum

    Private strNombreReporte As String = "Ordenes"

    Dim strProcesoLlamado As String


    Private objListaValidaciones As List(Of CPX_tblValidacionesOrdenes)
    Private intTotalLlamadosConfirmacion As Integer = 0
    Private intCantidadLlamadosConfirmacion As Integer = 0
    Private strConfirmaciones As String = String.Empty
    Private logCalculoFixing As Boolean = False


#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
    End Sub


    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            mdcProxy = inicializarProxy()

            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico

                Await consultarEncabezadoPorDefecto()

                Await OrdenesCombos()
                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarEncabezado("FILTRAR", String.Empty)

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    ''' <summary>
    ''' Lista de Registros que se encuentran cargadas en el grid del formulario
    ''' 
    ''' 
    ''' </summary>
    ''' 
    Private _ListaTipoOperacion As List(Of ProductoCombos)
    Public Property ListaTipoOperacion() As List(Of ProductoCombos)
        Get
            Return _ListaTipoOperacion
        End Get
        Set(ByVal value As List(Of ProductoCombos))
            _ListaTipoOperacion = value
            MyBase.CambioItem("ListaTipoOperacion")
        End Set
    End Property

    Private _ListaEncabezado As List(Of CPX_tblOrdenes)
    Public Property ListaEncabezado() As List(Of CPX_tblOrdenes)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As List(Of CPX_tblOrdenes))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value
            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de Registros para navegar sobre el grid con paginación
    ''' </summary>
    Public ReadOnly Property ListaEncabezadoPaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEncabezado) Then
                If IsNothing(_ListaEncabezadoPaginada) Then
                    Dim view = New PagedCollectionView(_ListaEncabezado)
                    _ListaEncabezadoPaginada = view
                    Return view
                Else
                    Return (_ListaEncabezadoPaginada)
                End If
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Elemento de la lista de Registros que se encuentra seleccionado
    ''' </summary>
    Private _EncabezadoSeleccionado As CPX_tblOrdenes
    Public Property EncabezadoSeleccionado() As CPX_tblOrdenes
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As CPX_tblOrdenes)
            _EncabezadoSeleccionado = value
            'Llamado de metodo para realizar la consulta del encabezado editable
            ConsultarEncabezadoEdicion()
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Elemento de la lista de Registros que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoEdicionSeleccionado As tblOrdenes
    Public Property EncabezadoEdicionSeleccionado() As tblOrdenes
        Get
            Return _EncabezadoEdicionSeleccionado
        End Get
        Set(ByVal value As tblOrdenes)
            _EncabezadoEdicionSeleccionado = value
            If Not IsNothing(_EncabezadoEdicionSeleccionado) Then
                objDatosAdicionalesOrden = New clsDatosAdicionalesOrden 'SV20190503
                CargarControlDetalle()
                MyBase.CambioItem("Editando")
            End If
            MyBase.CambioItem("EncabezadoEdicionSeleccionado")
        End Set
    End Property

    Private _ListaDetalleReceptores As List(Of CPX_tblOrdenesReceptores)
    Public Property ListaDetalleReceptores() As List(Of CPX_tblOrdenesReceptores)
        Get
            Return _ListaDetalleReceptores
        End Get
        Set(ByVal value As List(Of CPX_tblOrdenesReceptores))
            _ListaDetalleReceptores = value
            MyBase.CambioItem("ListaDetalleReceptores")
        End Set
    End Property


    Private _ListaDetalleReceptoresEliminar As List(Of CPX_tblOrdenesReceptores)
    Public Property ListaDetalleReceptoresEliminar() As List(Of CPX_tblOrdenesReceptores)
        Get
            Return _ListaDetalleReceptoresEliminar
        End Get
        Set(ByVal value As List(Of CPX_tblOrdenesReceptores))
            _ListaDetalleReceptores = value
            MyBase.CambioItem("ListaDetalleReceptoresEliminar")
        End Set
    End Property


    Private _ListaDetalleInstrucciones As List(Of CPX_tblOrdenesInstrucciones)
    Public Property ListaDetalleInstrucciones() As List(Of CPX_tblOrdenesInstrucciones)
        Get
            Return _ListaDetalleInstrucciones
        End Get
        Set(ByVal value As List(Of CPX_tblOrdenesInstrucciones))
            _ListaDetalleInstrucciones = value
            MyBase.CambioItem("ListaDetalleInstrucciones")
        End Set
    End Property


    Private _ListaDetalleInstruccionesEliminar As List(Of CPX_tblOrdenesInstrucciones)
    Public Property ListaDetalleInstruccionesEliminar() As List(Of CPX_tblOrdenesInstrucciones)
        Get
            Return _ListaDetalleInstruccionesEliminar
        End Get
        Set(ByVal value As List(Of CPX_tblOrdenesInstrucciones))
            _ListaDetalleInstrucciones = value
            MyBase.CambioItem("ListaDetalleInstruccionesEliminar")
        End Set
    End Property


    Private _ListaDetalleDatosGiros As List(Of CPX_DatosGiros)
    Public Property ListaDetalleDatosGiros() As List(Of CPX_DatosGiros)
        Get
            Return _ListaDetalleDatosGiros
        End Get
        Set(ByVal value As List(Of CPX_DatosGiros))
            _ListaDetalleDatosGiros = value
            MyBase.CambioItem("ListaDetalleDatosGiros")
        End Set
    End Property

    Private _strDetalleNegocio As String
    Public Property strDetalleNegocio() As String
        Get
            Return _strDetalleNegocio
        End Get
        Set(ByVal value As String)
            _strDetalleNegocio = value
            CambioItem("strDetalleNegocio")
        End Set
    End Property

    ''' <summary>
    ''' JAPC20180926: propiedad que muestra el nombre de la persona seleccionada en la orden
    ''' </summary>
    Private _strNombreCompleto As String
    Public Property strNombreCompleto() As String
        Get
            Return _strNombreCompleto
        End Get
        Set(ByVal value As String)
            _strNombreCompleto = value
            CambioItem("strNombreCompleto")
        End Set
    End Property

    ''' <summary>
    ''' JAPC20180926: propiedad para guardar el nombre anterior de la persoan por si se cancela la edicion o el nuevo registro esto para estados formulario
    ''' </summary>
    Private _strNombreCompletoAnterior As String
    Public Property strNombreCompletoAnterior() As String
        Get
            Return _strNombreCompletoAnterior
        End Get
        Set(ByVal value As String)
            _strNombreCompletoAnterior = value
            CambioItem("strNombreCompletoAnterior")
        End Set
    End Property


    ''' <summary>
    ''' JAPC20200318: Propiedad para controlar cuando activar campos forward en pantalla
    ''' </summary>
    Private _HabilitarForward As Boolean = False
    Public Property HabilitarForward() As Boolean
        Get
            Return _HabilitarForward
        End Get
        Set(ByVal value As Boolean)
            _HabilitarForward = value
            CambioItem("HabilitarForward")
        End Set
    End Property



    ''' <summary>
    ''' JAPC20200408_CC20200306-03: Propiedad para controlar cuando activar fixing forward dependiendo tipo de cumplimiento
    ''' </summary>
    Private _HabilitarFixing As Boolean = False
    Public Property HabilitarFixing() As Boolean
        Get
            Return _HabilitarFixing
        End Get
        Set(ByVal value As Boolean)
            _HabilitarFixing = value
            CambioItem("HabilitarFixing")
        End Set
    End Property


    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaOrdenes
    Public Property cb() As CamposBusquedaOrdenes
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaOrdenes)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

    'Persona buscador
    Private _PersonaSelected As CPX_BuscadorPersonas
    Public Property PersonaSelected() As CPX_BuscadorPersonas
        Get
            Return _PersonaSelected
        End Get
        Set(ByVal value As CPX_BuscadorPersonas)
            _PersonaSelected = value
        End Set
    End Property

    Private _dicCombosGeneral As Dictionary(Of String, ObservableCollection(Of ItemLista))
    Public Property dicCombosGeneral() As Dictionary(Of String, ObservableCollection(Of ItemLista))
        Get
            Return _dicCombosGeneral
        End Get
        Set(ByVal value As Dictionary(Of String, ObservableCollection(Of ItemLista)))
            _dicCombosGeneral = value
            CambioItem("dicCombosGeneral")
        End Set
    End Property


    Private _dicCombosProducto As Dictionary(Of String, ObservableCollection(Of ItemLista))
    Public Property dicCombosProducto() As Dictionary(Of String, ObservableCollection(Of ItemLista))
        Get
            Return _dicCombosProducto
        End Get
        Set(ByVal value As Dictionary(Of String, ObservableCollection(Of ItemLista)))
            _dicCombosProducto = value
            CambioItem("dicCombosProducto")
        End Set
    End Property


    Private _intDiasCumplimiento As Long
    Public Property intDiasCumplimiento() As Long
        Get
            Return _intDiasCumplimiento
        End Get
        Set(ByVal value As Long)
            _intDiasCumplimiento = value
            CambioItem("intDiasCumplimiento")
        End Set
    End Property


    Private _lstEstados As List(Of CPX_ComboOrdenes)
    Public Property lstEstados() As List(Of CPX_ComboOrdenes)
        Get
            Return _lstEstados
        End Get
        Set(ByVal value As List(Of CPX_ComboOrdenes))
            _lstEstados = value
            CambioItem("lstEstados")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad solo para saber cuando se está editando un registro no creando
    ''' </summary>
    Private _logSoloEditar As Boolean = False
    Public Property logSoloEditar() As Boolean
        Get
            Return _logSoloEditar
        End Get
        Set(ByVal value As Boolean)
            _logSoloEditar = value
            CambioItem("logSoloEditar")
        End Set
    End Property

    'SV20190503: Campos adicionales en la órden
    Private WithEvents _objDatosAdicionalesOrden As New clsDatosAdicionalesOrden
    Public Property objDatosAdicionalesOrden() As clsDatosAdicionalesOrden
        Get
            Return _objDatosAdicionalesOrden
        End Get
        Set(ByVal value As clsDatosAdicionalesOrden)
            _objDatosAdicionalesOrden = value

            CambioItem("objDatosAdicionalesOrden")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"




    ''' <summary>
    '''  Carga de combos de la pantlla de órdenes
    ''' </summary>
    ''' <returns>SV20180711_ORDENES</returns>
    Public Async Function OrdenesCombos() As Task

        Try
            Dim objRespuesta As InvokeResult(Of List(Of CPX_ComboOrdenes))

            objRespuesta = Await mdcProxy.OrdenesCombosAsync(Program.Usuario)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    dicCombosGeneral = clsGenerales.CargarListas(objRespuesta.Value)
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "OrdenesCombos", Program.TituloSistema, Program.Maquina, ex)
        End Try

    End Function

    ''' <summary>
    ''' Carga de combos específicos para el encabezado
    ''' </summary>
    ''' <param name="pstrProducto"></param>
    ''' <param name="pstrCondicionTexto1"></param>
    ''' <param name="pstrCondicionTexto2"></param>
    ''' <param name="pstrCondicionEntero1"></param>
    ''' <param name="pstrCondicionEntero2"></param>
    ''' <returns>SV20180711_ORDENES</returns>
    Public Async Function OrdenesCombosEspecificos(ByVal pstrProducto As String,
                                        ByVal pstrCondicionTexto1 As String,
                                        ByVal pstrCondicionTexto2 As String,
                                        ByVal pstrCondicionEntero1 As Integer,
                                        ByVal pstrCondicionEntero2 As Integer) As Task

        Try
            Dim objRespuesta As InvokeResult(Of List(Of CPX_ComboOrdenes))

            objRespuesta = Await mdcProxy.OrdenesCombosEspecificosAsync(pstrProducto, pstrCondicionTexto1, pstrCondicionTexto2, pstrCondicionEntero1, pstrCondicionEntero2, Program.Usuario)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then

                    If pstrCondicionTexto1 = "ESPECIFICOS" Then
                        dicCombosProducto = clsGenerales.CargarListas(objRespuesta.Value)
                    End If
                    If pstrCondicionTexto1 = "ESTADOSORDEN" Then
                        lstEstados = objRespuesta.Value
                    End If

                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "OrdenesCombosEspecificos", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function


    ''' <summary>
    ''' SV20181023_AJUSTESORDENES
    ''' Proceso del cálculo de dias de cumplimineto de la órden
    ''' Se añade la moneda
    ''' </summary>
    ''' <param name="pstrTipoCalculo"></param>
    ''' <param name="pdtmFechaInicial"></param>
    ''' <param name="pdtmFechaFinal"></param>
    ''' <param name="pintNroDias"></param>
    ''' <param name="pintMoneda"></param>
    ''' <returns></returns>
    Public Async Function Procesos_CalcularDiasOrden(ByVal pstrTipoCalculo As String,
                                             ByVal pdtmFechaInicial As DateTime?,
                                             ByVal pdtmFechaFinal As DateTime?,
                                             ByVal pintNroDias As Integer?,
                                             ByVal pintMoneda As Integer?) As Task

        Try
            Dim objRespuesta As InvokeResult(Of List(Of CPX_CalculoDiasOrden))
            objRespuesta = Await mdcProxy.Procesos_CalcularDiasOrdenAsync(pstrTipoCalculo, pdtmFechaInicial, pdtmFechaFinal, pintNroDias, pintMoneda, Program.Usuario)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    If pstrTipoCalculo = "CalcularDiasVigencia" Then
                        intDiasCumplimiento = objRespuesta.Value.FirstOrDefault.intNroDias
                        'JAPC20200408_CC20200306-04 logica para enviar dias de cumplimiento forward al detalle para correcto calculo devaluacion forward
                        If EncabezadoEdicionSeleccionado.strProducto = "DERIVADOS" And Editando = True Then
                            OrdenesDivisasView.DiasCumplimientoForward = 0
                            OrdenesDivisasView.DiasCumplimientoForward = CInt(intDiasCumplimiento)
                        End If
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "Procesos_CalcularDiasOrden", Program.TituloSistema, Program.Maquina, ex)
        End Try


    End Function

    ''' <summary>
    ''' SV20181023_AJUSTESORDENES
    ''' Respuesta del evento de cambio de una propiedad del detalle
    ''' </summary>
    ''' </summary>
    ''' <param name="strPropiedad"></param>
    ''' <param name="strValor"></param>
    Public Async Sub RespuestaValorPropiedadDetalle(ByVal strPropiedad As String, ByVal strValor As String, dblValor As Double?)
        Try
            If strPropiedad = "Moneda" Then
                OrdenesDivisasView.strEntregarValorPropiedad = ""
                If Not String.IsNullOrEmpty(strValor) Then
                    If IsNumeric(strValor) Then
                        Await CalcularDiasCumplimiento(CInt(strValor))
                    End If
                Else
                    Await CalcularDiasCumplimiento(Nothing)
                End If
                MyBase.CambioItem("objDatosAdicionalesOrden")
            End If

            'SV20190503
            If strPropiedad = "ValorNeto" Then

                OrdenesDivisasView.strEntregarValorPropiedad = ""
                If Not IsNothing(dblValor) Then
                    objDatosAdicionalesOrden.dblValorNeto = dblValor

                Else
                    objDatosAdicionalesOrden.dblValorNeto = 0
                End If
                MyBase.CambioItem("objDatosAdicionalesOrden")
            End If

            'RABP201906
            If strPropiedad = "strReferencia" Then
                OrdenesDivisasView.strEntregarValorPropiedad = ""
                If Not String.IsNullOrEmpty(strValor) Then
                    objDatosAdicionalesOrden.strReferencia = strValor
                Else
                    objDatosAdicionalesOrden.strReferencia = ""
                End If
                MyBase.CambioItem("objDatosAdicionalesOrden")
            End If


            'JAPC20200408_CC20200306-03_Ajuste divisas forward tipo de cumplimiento DELIVERY, el campo de FIXING debe estar fijo en T + 0, si el tipo de cumplimiento es NONDELIVERY si se debe activar el campo de FIXING
            If strPropiedad = "TipoCumplimiento" Then

                OrdenesDivisasView.strEntregarValorPropiedad = ""

                If strValor = "NOND" Then
                    HabilitarFixing = True
                ElseIf strValor = "DELI" Then
                    EncabezadoEdicionSeleccionado.strFixing = "T0"
                    HabilitarFixing = False
                End If
                CambioItem("HabilitarFixing")
            End If


        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "RespuestaValorPropiedadDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub



    ''' <summary>
    ''' SV20181023_AJUSTESORDENES
    ''' Inicio del Cálculo de los dias de cumplimineto de la orden
    ''' </summary>
    Public Sub IniciarCalcularDiasCumplimiento()
        Try
            If Not IsNothing(EncabezadoEdicionSeleccionado) AndAlso Not IsNothing(EncabezadoEdicionSeleccionado.dtmOrden) AndAlso Not IsNothing(EncabezadoEdicionSeleccionado.dtmVigenciaHasta) Then
                OrdenesDivisasView.strEntregarValorPropiedad = "Moneda"
            Else
                intDiasCumplimiento = 0
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "IniciarCalcularDiasCumplimiento", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Llamado al servidor para el cálculo de los dias de cumplimiento de la orden
    ''' JosePineda_JAPC20200318: Ajuste para incluir en logica fixing de derivados
    ''' CC:C-20200306
    ''' </summary>
    ''' <returns></returns>
    Public Async Function CalcularDiasCumplimiento(intIdMoneda As Integer?, Optional intFixing As Integer = 0) As Task
        Try
            logCalculoFixing = False
            If Not IsNothing(EncabezadoEdicionSeleccionado) AndAlso Not IsNothing(EncabezadoEdicionSeleccionado.dtmOrden) AndAlso Not IsNothing(EncabezadoEdicionSeleccionado.dtmVigenciaHasta) Then
                'If intFixing > 0 Then
                '    ReCalcularDiasNegocio(intFixing)
                'Else
                '    If Not IsNothing(EncabezadoEdicionSeleccionado.strFixing) AndAlso EncabezadoEdicionSeleccionado.strFixing <> "" AndAlso Not IsNothing(EncabezadoEdicionSeleccionado.dtmOrden) AndAlso Not IsNothing(EncabezadoEdicionSeleccionado.dtmVigenciaHasta) Then
                '        intFixing = EncabezadoEdicionSeleccionado.strFixing.Substring(1)
                '        ReCalcularDiasNegocio(intFixing)
                '    End If
                'End If

                'JAPC20200408_CC20200306-03 : ajuste fixing dias cumplimiento reales forward
                If EncabezadoEdicionSeleccionado.strProducto = "DERIVADOS" And EncabezadoEdicionSeleccionado.strTipoDerivado = "FWA" Then
                    If intFixing = 0 And Not IsNothing(EncabezadoEdicionSeleccionado.strFixing) AndAlso EncabezadoEdicionSeleccionado.strFixing <> "" AndAlso Not IsNothing(EncabezadoEdicionSeleccionado.dtmOrden) AndAlso Not IsNothing(EncabezadoEdicionSeleccionado.dtmVigenciaHasta) And Editando = True Then
                        intFixing = EncabezadoEdicionSeleccionado.strFixing.Substring(1)
                    End If
                    intDiasCumplimiento = DateDiff(DateInterval.Day, EncabezadoEdicionSeleccionado.dtmOrden, IIf(intFixing > 0, EncabezadoEdicionSeleccionado.dtmVigenciaHasta.AddDays(intFixing), EncabezadoEdicionSeleccionado.dtmVigenciaHasta))

                    If Editando = True Then
                        'JAPC20200408_CC20200306-04 logica para enviar dias de cumplimiento forward al detalle para correcto calculo devaluacion forward
                        OrdenesDivisasView.DiasCumplimientoForward = 0
                        OrdenesDivisasView.DiasCumplimientoForward = CInt(intDiasCumplimiento)
                    End If
                Else
                    Await Procesos_CalcularDiasOrden("CalcularDiasVigencia", EncabezadoEdicionSeleccionado.dtmOrden, EncabezadoEdicionSeleccionado.dtmVigenciaHasta, Nothing, intIdMoneda)
                End If



                If intFixing > 0 Then
                    logCalculoFixing = True
                    EncabezadoEdicionSeleccionado.dtmVigenciaHasta = EncabezadoEdicionSeleccionado.dtmVigenciaHasta.AddDays(intFixing)
                Else
                    logCalculoFixing = False
                End If


            Else
                intDiasCumplimiento = 0
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "CalcularDiasCumplimiento", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Function


    '' <summary>
    '' JosePineda_JAPC20200318: Metodo para recalcular teniendo encuenta los dias habiles para el fixing derivados
    '' CC:C-20200306
    '' </summary>
    '' <param name="Fixing"></param>
    'Public Sub ReCalcularDiasNegocio(ByRef Fixing As Integer)

    '    If EncabezadoEdicionSeleccionado.dtmVigenciaHasta.DayOfWeek = DayOfWeek.Thursday And Fixing > 1 Then
    '        Fixing = Fixing + 2
    '    ElseIf EncabezadoEdicionSeleccionado.dtmVigenciaHasta.DayOfWeek = DayOfWeek.Friday And Fixing > 0 Then
    '        Fixing = Fixing + 2
    '    End If
    'End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    ''' <history>
    ''' </history>
    Public Overrides Async Sub NuevoRegistro()
        Try

            Dim objNvoEncabezado As New tblOrdenes

            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            ''JAPC20181123_VALIDACIONESORDENES
            'If Await VerificarFechaHoraOrden() Then
            '    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("ORDENES_VERIFICARFECHAORDEN"), Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
            '    CambiarALista()
            '    Exit Sub
            'End If

            'JAPC20181123_VALIDACIONESORDENES
            If Await VerificarFechaCierreAnterior() Then
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("ORDENES_VERIFICARFECHACIERREANT"), Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
                CambiarALista()
                Exit Sub
            End If


            'Obtiene el registro por defecto
            ObtenerRegistroAnterior(mobjEncabezadoPorDefecto, objNvoEncabezado)
            'Salva el registro anterior
            ObtenerRegistroAnterior(_EncabezadoEdicionSeleccionado, mobjEncabezadoAnterior)

            'JAPC20180926: salva el nombre anterior
            strNombreCompleto = String.Empty
            If Not IsNothing(EncabezadoSeleccionado) Then
                strNombreCompletoAnterior = EncabezadoSeleccionado.strNombre
            End If



            Editando = True
            MyBase.CambioItem("Editando")

            EncabezadoEdicionSeleccionado = objNvoEncabezado

            MyBase.NuevoRegistro()

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción Buscar de la barra de herramientas.
    ''' Ejecuta una búsqueda sobre los datos que contengan en los campos definidos internamente en el procedimiento de búsqueda (filtrado) el texto ingresado en el campo Filtro de la barra de herramientas
    ''' </summary>
    ''' <history>
    ''' </history>
    Public Overrides Async Sub Filtrar()
        Try
            Await ConsultarEncabezado("FILTRAR", FiltroVM)
            strTipoFiltroBusqueda = "FILTRAR"
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Quitar el estado de filtro de la pantalla y cargar los registros sin condicional
    ''' </summary>
    Public Overrides Sub QuitarFiltro()
        Try
            MyBase.QuitarFiltro()
            strTipoFiltroBusqueda = String.Empty
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "QuitarFiltro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción "Búsqueda avanzada" de la barra de herramientas.
    ''' Presenta la forma de búsqueda para que el usuario seleccione los valores por los cuales desea buscar dentro de los campos definidos en la forma de búsqueda
    ''' </summary>
    Public Overrides Sub Buscar()
        Try
            PrepararNuevaBusqueda()
            MyBase.Buscar()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "Buscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Buscar de la forma de búsqueda
    ''' Ejecuta una búsqueda por los campos contenidos en la forma de búsqueda y cuyos valores correspondan con los seleccionados por el usuario
    ''' </summary>
    ''' <history>
    ''' </history>
    Public Overrides Async Sub ConfirmarBuscar()
        Try
            Await ConsultarEncabezado("BUSQUEDA", String.Empty, cb.intID, cb.intConsecutivo, cb.intVersion, cb.strProducto, cb.intIDComitente, cb.strTipo, cb.dtmOrden, cb.strEstado)
            strTipoFiltroBusqueda = "BUSQUEDA"
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón guardar de la barra de herramientas. 
    ''' Ejecuta el proceso que ingresa o actualiza la base de datos con los cambios realizados 
    ''' </summary>
    ''' <history>
    ''' </history>
    Public Overrides Sub ActualizarRegistro()
        Dim strAccion As String = ValoresUserState.Actualizar.ToString()
        Try
            strProcesoLlamado = "ACTUALIZAR"
            If EncabezadoEdicionSeleccionado.strProducto = "DIVISAS" Then
                OrdenesDivisasView.EntregarJsonGuardado = False
                OrdenesDivisasView.EntregarJsonGuardado = True
            ElseIf EncabezadoEdicionSeleccionado.strProducto = "DERIVADOS" Then     'JosePineda_JAPC20200318: Ajuste para cargar detalle negocio derivados forward de divisas con ayuda de activacion eventos json CC:C-20200306
                OrdenesDivisasView.EntregarJsonGuardado = False
                OrdenesDivisasView.EntregarJsonGuardado = True
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Callback de llegada del evento de grabado llamado desde el detalle específico de la orden
    ''' </summary>
    ''' <param name="pstrDetalleJson"></param>
    ''' <param name="pstrLlamado"></param>
    Public Sub RespuestaJsonConcatenadoDetalle(ByVal pstrDetalleJson As String, ByVal pstrLlamado As String)
        Try
            strDetalleNegocio = pstrDetalleJson
            ContinuarActualizarRegistro()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "RespuestaJsonConcatenadoDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    ''' <summary>
    ''' Proceso para el ejecutar el llamado al procedimiento de validación la grabación del registro 
    ''' </summary>
    Public Async Sub ContinuarActualizarRegistro()
        Try
            MyBase.ActualizarRegistro()

            If String.IsNullOrEmpty(strProcesoLlamado) Then
                strProcesoLlamado = "CUMPLIR"
            End If

            ErrorForma = String.Empty
            IsBusy = True

            Dim objEncabezadoComplex As New CPX_tblOrdenes

            objEncabezadoComplex = LlevarObjectAComplex(_EncabezadoEdicionSeleccionado)


            If Not IsNothing(_ListaDetalleReceptores) AndAlso _ListaDetalleReceptores.Count > 0 Then
                objEncabezadoComplex.strDetalleReceptores = JsonConvert.SerializeObject(_ListaDetalleReceptores)
            End If

            If Not IsNothing(_ListaDetalleInstrucciones) AndAlso _ListaDetalleInstrucciones.Count > 0 Then
                objEncabezadoComplex.strDetalleInstrucciones = JsonConvert.SerializeObject(_ListaDetalleInstrucciones)
            End If

            If Not IsNothing(_ListaDetalleDatosGiros) AndAlso _ListaDetalleDatosGiros.Count > 0 Then
                objEncabezadoComplex.strDetalleDatosGiros = JsonConvert.SerializeObject(_ListaDetalleDatosGiros)
            End If


            objEncabezadoComplex.strDetalleNegocio = strDetalleNegocio

            Dim strDatosNegocio As String = String.Empty

            strDatosNegocio = JsonConvert.SerializeObject(objEncabezadoComplex)


            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesOrdenes)) = Await mdcProxy.Ordenes_ActualizarAsync(
                                                                            _EncabezadoEdicionSeleccionado.intID,
                                                                             _EncabezadoEdicionSeleccionado.strProducto,
                                                                            strDatosNegocio,
                                                                            strConfirmaciones,
                                                                            False,
                                                                            Program.Usuario,
                                                                            _EncabezadoEdicionSeleccionado.dtmActualizacion,
                                                                            strProcesoLlamado)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    Dim objListaResultado As List(Of CPX_tblValidacionesOrdenes) = CType(objRespuesta.Value, List(Of CPX_tblValidacionesOrdenes))
                    Dim objListaMensajes As New List(Of ProductoValidaciones)
                    Dim intIDRegistroActualizado As Integer = -1
                    Dim dtmRegistroActualizado As Date = Now


                    If (objListaResultado.All(Function(i) i.logInconsitencia = False)) Then
                        A2Utilidades.Mensajes.mostrarMensaje(objListaResultado.First.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
                        intIDRegistroActualizado = objListaResultado.First.intIDRegistro
                        dtmRegistroActualizado = objListaResultado.First.dtmRegistro
                            MyBase.FinalizoGuardadoRegistros()
                            logSoloEditar = False
                            strProcesoLlamado = String.Empty
                            strConfirmaciones = String.Empty

                            ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                            If strTipoFiltroBusqueda = "FILTRAR" Then
                                Await ConsultarEncabezado("FILTRAR", FiltroVM, intIDRegistroActualizado)
                            ElseIf strTipoFiltroBusqueda = "BUSQUEDA" And Not IsNothing(cb) Then
                                Await ConsultarEncabezado("BUSQUEDA", String.Empty, cb.intID, cb.intConsecutivo, cb.intVersion, cb.strProducto, cb.intIDComitente, cb.strTipo, cb.dtmOrden)
                            Else
                                Await ConsultarEncabezado("FILTRAR", String.Empty, intIDRegistroActualizado)
                            End If
                        Else
                            If (From c In objListaResultado Where c.strTipoMensaje = "INCONSISTENCIA" Select c).Count > 0 Then

                            For Each li In (From c In objListaResultado Where c.strTipoMensaje = "INCONSISTENCIA")
                                objListaMensajes.Add(New ProductoValidaciones With {
                                                 .Campo = li.strCampo,
                                                 .TituloCampo = MyBase.ObtenerTituloCampo(li.strCampo),
                                                 .Mensaje = li.strMensaje
                                                 })
                            Next

                            Dim strMensaje As String = ""
                            For Each m In objListaMensajes
                                strMensaje += IIf(String.IsNullOrWhiteSpace(m.TituloCampo), "", m.TituloCampo + ":") + m.Mensaje + vbCrLf
                            Next
                            A2Utilidades.Mensajes.mostrarMensaje(strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia,)
                            MyBase.ActualizarListaInconsistencias(objListaMensajes, Program.TituloSistema, False)
                            IsBusy = False

                        Else
                            If (objListaResultado.All(Function(i) i.strTipoMensaje = "CONFIRMACION")) Then

                                objListaValidaciones = (From c In objListaResultado Where c.strTipoMensaje = "CONFIRMACION").ToList
                                intTotalLlamadosConfirmacion = objListaValidaciones.Count
                                intCantidadLlamadosConfirmacion = 0
                                strConfirmaciones = String.Empty

                                LanzarMensajesPregunta("VALIDARORDEN")

                            End If
                        End If
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al mostrar la lista de mensajes.", Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                    IsBusy = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al mostrar la lista de mensajes.", Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al recibir el resultado del grabado en el servidor.",
                                 Me.ToString(), "GRABAR", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
            HabilitarForward = False
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Editar de la barra de herramientas.
    ''' Activa la edición del encabezado y del detalle (si aplica) del encabezado activo
    ''' </summary>
    Public Overrides Sub EditarRegistro()
        Try
            If Not IsNothing(_EncabezadoEdicionSeleccionado) Then

                If _EncabezadoEdicionSeleccionado.intVersion > 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEditarVersion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Editando = False
                    Exit Sub
                End If

                If _EncabezadoEdicionSeleccionado.strEstado = "CNC" Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEditarEstadoCancelado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Editando = False
                    Exit Sub
                End If

                If _EncabezadoEdicionSeleccionado.strEstado = "PND" And Format(_EncabezadoEdicionSeleccionado.dtmVigenciaHasta, "yyyyMMdd") < Format(Now(), "yyyyMMdd") Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEditarEstadoPendiente & " - " & _EncabezadoEdicionSeleccionado.dtmVigenciaHasta, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Editando = False
                    Exit Sub
                End If

                If _EncabezadoEdicionSeleccionado.strEstado = "CPL" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se puede editar la orden, se encuentra cumplida ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Editando = False
                    Exit Sub
                End If

                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Editando = False
                    Exit Sub
                End If
                IsBusy = True


                Validar_Procesos("EDICION")

                'JAPC20200408_CC20200306-03-Correccion para editar preorden forward
                If _EncabezadoEdicionSeleccionado.strProducto = "DERIVADOS" And _EncabezadoEdicionSeleccionado.strTipoDerivado = "FWA" Then

                    HabilitarForward = True
                    _EncabezadoEdicionSeleccionado.strTipoDerivado = Nothing
                    _EncabezadoEdicionSeleccionado.strTipoDerivado = "FWA"

                    OrdenesDivisasView.strEntregarValorPropiedad = "TipoCumplimiento"
                    OrdenesDivisasView.DiasCumplimientoForward = 0
                    OrdenesDivisasView.DiasCumplimientoForward = CInt(intDiasCumplimiento)
                End If


                IsBusy = False
                End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Inicia la edición del registro cuando se ha confirmado que es permistido para editar
    ''' </summary>
    Public Sub IniciaEditar()
        Try
            ObtenerRegistroAnterior(_EncabezadoEdicionSeleccionado, mobjEncabezadoAnterior)
            'JAPC20180926: se salva el nombre anterior
            strNombreCompletoAnterior = EncabezadoSeleccionado.strNombre
            Editando = True
            logSoloEditar = True
            MyBase.CambioItem("Editando")
            MyBase.CambioItem("HabilitarEdicionDetalle")
            If EncabezadoEdicionSeleccionado.strProducto = "DIVISAS" Then
                OrdenesDivisasView.EditarRegistroDetalle = True
                'JAPC20200408_CC20200306-03-Correccion para editar preorden forward
            ElseIf EncabezadoEdicionSeleccionado.strProducto = "DERIVADOS" Then
                OrdenesDivisasView.EditarRegistroDetalle = True
            End If
            MyBase.EditarRegistro()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "IniciaEditar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Termina la pregunta de confirmación de edición del registro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Public Sub TerminoPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If objResultado.DialogResult Then
                IniciaEditar()
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Cancelar de la barra de herramientas durante el ingreso o modificación del encabezado activo
    ''' </summary>
    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = String.Empty
            mdcProxy.RejectChanges()
            Editando = False
            logSoloEditar = False
            strProcesoLlamado = String.Empty
            strConfirmaciones = String.Empty
            MyBase.CambioItem("Editando")
            MyBase.CambioItem("HabilitarEdicionDetalle")
            If EncabezadoEdicionSeleccionado.strProducto = "DIVISAS" Then
                OrdenesDivisasView.EditarRegistroDetalle = False
            ElseIf EncabezadoEdicionSeleccionado.strProducto = "DERIVADOS" Then
                OrdenesDivisasView.EditarRegistroDetalle = False
            End If
            MyBase.CancelarEditarRegistro()

            EncabezadoEdicionSeleccionado = mobjEncabezadoAnterior
            'JAPC20180926 : se devuelve el nombre anterior por cancelacion de usuario y recalcula dias cumplimiento
            strNombreCompleto = strNombreCompletoAnterior
            'JAPC20200408_CC20200306-03: Ajuste cancelacion edicion forward
            HabilitarForward = False
            HabilitarFixing = False
            MyBase.CambioItem("HabilitarForward")
            MyBase.CambioItem("HabilitarFixing")
            IniciarCalcularDiasCumplimiento()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta al hacer clic en el el botón eliminar de la barra de tareas, para el caso de la órden se hace una anulación no una eliminación
    ''' </summary>
    Public Overrides Sub BorrarRegistro()
        Try
            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True

            ObtenerRegistroAnterior(_EncabezadoEdicionSeleccionado, mobjEncabezadoAnterior)

            strProcesoLlamado = "ANULAR"


            If EncabezadoEdicionSeleccionado.strProducto = "DIVISAS" Then
                OrdenesDivisasView.EntregarJsonGuardado = False
                OrdenesDivisasView.EntregarJsonGuardado = True
            End If

            'RABP20190911_Se hace necesario realizar el tru false para cuando se elimina un registro no se sobremonte la lsita con la forma
            Me.Editando = True
                Me.Editando = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"),
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo para para validar la hora que se ingresa la orden para 
    ''' verificar que no se pase de la hora de control divisas 
    ''' </summary>
    ''' <returns>JAPC20181123_VALIDACIONESORDENES</returns>
    Private Async Function VerificarFechaHoraOrden() As Task(Of Boolean)
        Try
            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Function
            End If

            Dim objRespuesta = Await mdcProxy.Procesos_VerificarFechaHoraOrdenAsync(Program.Usuario)

            Return objRespuesta.Value
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"),
             Me.ToString(), "VerificarFechaHoraOrden", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function


    ''' <summary>
    ''' Metodo para  verificar que ya exista un cierre de operaciones del ultimo dia 
    ''' anterior al actual para poder realizar las operaciones del dia actual
    ''' </summary>
    ''' <returns>JAPC20181123_VALIDACIONESORDENES</returns>
    Private Async Function VerificarFechaCierreAnterior() As Task(Of Boolean)
        Try
            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Function
            End If

            Dim objRespuesta = Await mdcProxy.Procesos_VerificarFechaCierreAnteriorAsync("USD", Program.Usuario)

            Return objRespuesta.Value
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"),
             Me.ToString(), "VerificarFechaCierreAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    Public Sub RefrescarOrden()
        Try
            ConsultarEncabezado("FILTRAR", String.Empty)
            'ConfirmarBuscar()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar la consulta de actualización de la orden", Me.ToString(), "refrescarOrden", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub
#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Consulta los valores por defecto para un nuevo encabezado
    ''' </summary>
    Private Async Function consultarEncabezadoPorDefecto() As Task
        Try
            Dim objRespuesta As InvokeResult(Of tblOrdenes)

            objRespuesta = Await mdcProxy.Ordenes_DefectoAsync(Program.Usuario)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    mobjEncabezadoPorDefecto = objRespuesta.Value
                    mobjEncabezadoPorDefecto.strUsuario = Program.Usuario
                    mobjEncabezadoPorDefecto.strUsuarioCreacion = Program.Usuario
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "consultarEncabezadoPorDefecto", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function

    ''' <summary>
    ''' Consultar de forma sincrónica los datos de Registro
    ''' </summary>
    Private Async Function ConsultarEncabezado(ByVal pstrOpcion As String _
                                             , Optional ByVal pstrFiltro As String = "" _
                                             , Optional ByVal pintID As Integer? = Nothing _
                                            , Optional ByVal pintConsecutivo As Long? = Nothing _
                                              , Optional ByVal pintVersion As Integer? = Nothing _
                                            , Optional ByVal pstrProducto As String = Nothing _
                                            , Optional ByVal pintIDComitente As String = Nothing _
                                            , Optional ByVal pstrTipo As String = Nothing _
                                            , Optional ByVal pdtmOrden As Date? = Nothing _
                                            , Optional ByVal pstrEstado As String = Nothing) As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            IsBusy = True
            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblOrdenes)) = Nothing

            Dim objRespuestaEntidad As InvokeResult(Of tblOrdenes) = Nothing

            ErrorForma = String.Empty

            If pstrOpcion = "FILTRAR" Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRespuesta = Await mdcProxy.Ordenes_FiltrarAsync(pstrFiltro, Program.Usuario)
            ElseIf pstrOpcion = "ID" Then
                objRespuestaEntidad = Await mdcProxy.Ordenes_IDAsync(pintID, Program.Usuario)
            Else
                objRespuesta = Await mdcProxy.Ordenes_ConsultarAsync(pintID, pintConsecutivo, pintVersion, pstrProducto, pintIDComitente, pstrTipo, pdtmOrden, pstrEstado, Program.Usuario)
            End If

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    ListaEncabezado = objRespuesta.Value

                    If pintID > 0 Then
                        If ListaEncabezado.Where(Function(i) i.intID = pintID).Count > 0 Then
                            EncabezadoSeleccionado = ListaEncabezado.Where(Function(i) i.intID = pintID).First
                        End If
                    End If

                    If ListaEncabezado.Count > 0 Then
                        If pstrOpcion = "BUSQUEDA" Then
                            MyBase.ConfirmarBuscar()
                        End If
                    Else
                        If (pstrOpcion = "FILTRAR" And Not String.IsNullOrEmpty(pstrFiltro)) Or (pstrOpcion = "BUSQUEDA") Then
                            ' Solamente se presenta el mensaje cuando se ejecuta la opción de filtrar con un filtro específico o la opción de buscar (consultar) 
                            A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_BUSQUEDANOEXITOSA"), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Consulta los datos del registro a editar 
    ''' </summary>
    Public Async Sub ConsultarEncabezadoEdicion()
        Try
            EncabezadoEdicionSeleccionado = Nothing

            If Not IsNothing(_EncabezadoSeleccionado) Then
                If _EncabezadoSeleccionado.intID > 0 Then
                    Await OrdenesCombosEspecificos(_EncabezadoSeleccionado.strProducto, "ESPECIFICOS", Nothing, Nothing, Nothing)
                    Await OrdenesCombosEspecificos(_EncabezadoSeleccionado.strProducto, "ESTADOSORDEN", _EncabezadoSeleccionado.strEstado, Nothing, Nothing)
                    Await ConsultarEncabezadoEdicion(_EncabezadoSeleccionado.intID)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consultar de forma sincrónica los datos del Registro a editar
    ''' </summary>
    ''' <param name="pintId">Indica el ID de la entidad a consultar</param>
    Private Async Function ConsultarEncabezadoEdicion(ByVal pintId As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            IsBusy = True
            Dim objRespuesta As InvokeResult(Of tblOrdenes) = Nothing

            ErrorForma = String.Empty

            objRespuesta = Await mdcProxy.Ordenes_IDAsync(pintId, Program.Usuario)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    EncabezadoEdicionSeleccionado = objRespuesta.Value
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarEncabezadoEdicion", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"

    ''' <summary>
    ''' Limpia los datos de la entidad de busqueda
    ''' </summary>
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaOrdenes
            objCB.intID = Nothing
            objCB.intConsecutivo = Nothing
            objCB.intVersion = Nothing
            objCB.strProducto = Nothing
            objCB.intIDComitente = Nothing
            objCB.strNombre = Nothing
            objCB.strTipo = Nothing
            objCB.dtmOrden = Nothing
            cb = objCB

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Retorna una copia del encabezado activo. 
    ''' Se hace un "clon" del encabezado activo para poder devolver los cambios y dejar el encabezado activo en su estado original si es necesario.
    ''' </summary>
    ''' <history>
    ''' </history>
    Private Sub ObtenerRegistroAnterior(ByVal pobjRegistro As tblOrdenes, ByRef pobjRegistroSalvar As tblOrdenes)
        Dim objEncabezado As tblOrdenes = New tblOrdenes

        Try
            If Not IsNothing(pobjRegistro) Or IsDBNull(pobjRegistro) Then
                objEncabezado.intID = pobjRegistro.intID
                objEncabezado.intConsecutivo = pobjRegistro.intConsecutivo
                objEncabezado.intVersion = pobjRegistro.intVersion
                objEncabezado.intIDComitente = pobjRegistro.intIDComitente
                objEncabezado.strTipo = pobjRegistro.strTipo
                objEncabezado.dtmOrden = pobjRegistro.dtmOrden
                objEncabezado.dtmVigenciaHasta = pobjRegistro.dtmVigenciaHasta
                objEncabezado.strEstado = pobjRegistro.strEstado
                objEncabezado.dtmEstado = pobjRegistro.dtmEstado
                objEncabezado.strProducto = pobjRegistro.strProducto
                objEncabezado.strClasificacionNegocio = pobjRegistro.strClasificacionNegocio
                objEncabezado.strUsuarioCreacion = pobjRegistro.strUsuarioCreacion
                objEncabezado.dtmCreacion = pobjRegistro.dtmCreacion
                objEncabezado.strUsuario = pobjRegistro.strUsuario
                objEncabezado.dtmActualizacion = pobjRegistro.dtmActualizacion
                objEncabezado.strTipoDerivado = pobjRegistro.strTipoDerivado            'JAPC20200318: C-20200306 Forward divisas nuevos campos
                objEncabezado.strFixing = pobjRegistro.strFixing
                objEncabezado.strTipoOperacion = pobjRegistro.strTipoOperacion
                objEncabezado.strObjetivo = pobjRegistro.strObjetivo
                objEncabezado.strTpoContraparte = pobjRegistro.strTpoContraparte

                pobjRegistroSalvar = objEncabezado

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Modificación de las propeiedades del encabezado.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Async Sub _EncabezadoEdicionSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EncabezadoEdicionSeleccionado.PropertyChanged
        Try
            If Editando Then
                Select Case e.PropertyName

                    Case "strProducto"
                        If Not IsNothing(EncabezadoEdicionSeleccionado.strProducto) Then
                            Await OrdenesCombosEspecificos(EncabezadoEdicionSeleccionado.strProducto, "ESPECIFICOS", Nothing, Nothing, Nothing)
                            If logSoloEditar Then
                                Await OrdenesCombosEspecificos(EncabezadoEdicionSeleccionado.strProducto, "ESTADOSORDEN", EncabezadoEdicionSeleccionado.strEstado, EncabezadoEdicionSeleccionado.intID, Nothing)
                            End If

                            'RABP20200602: Se hace esta validación ya que solo aplica el cierre de operaciones para Divisas
                            If EncabezadoEdicionSeleccionado.strProducto = "DIVISAS" Then
                                'JAPC20181123_VALIDACIONESORDENES
                                If Await VerificarFechaHoraOrden() Then
                                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("ORDENES_VERIFICARFECHAORDEN"), Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
                                    CambiarALista()
                                    CancelarEditarRegistro()
                                    Exit Sub
                                End If
                            End If

                            'JAPC20200318: C-20200306 Logica para activar controles forward de la orden y valores por defecto
                            If EncabezadoEdicionSeleccionado.strProducto = "DERIVADOS" And Editando = True Then
                                HabilitarForward = True
                                EncabezadoEdicionSeleccionado.strClasificacionNegocio = "FWD"
                                EncabezadoEdicionSeleccionado.strTipoOperacion = "MON"
                                EncabezadoEdicionSeleccionado.strObjetivo = "ESP"
                                EncabezadoEdicionSeleccionado.strFixing = "T0"
                            Else
                                HabilitarForward = False
                            End If
                            CargarControlDetalle()
                        End If
                    Case "dtmOrden"
                        'SV20181023_AJUSTESORDENES
                        IniciarCalcularDiasCumplimiento()
                    Case "dtmVigenciaHasta"
                        'SV20181023_AJUSTESORDENES
                        'JAPC20200318: C-20200306 Logica para manejo fixing
                        If logCalculoFixing = False Then
                            IniciarCalcularDiasCumplimiento()
                        End If
                        logCalculoFixing = False

                    Case "strFixing"
                        'JAPC20200318: C-20200306 Logica para campos fixing forward divisas 

                        If Not IsNothing(EncabezadoEdicionSeleccionado.strFixing) AndAlso EncabezadoEdicionSeleccionado.strFixing <> "" AndAlso Not IsNothing(EncabezadoEdicionSeleccionado.dtmOrden) AndAlso Not IsNothing(EncabezadoEdicionSeleccionado.dtmVigenciaHasta) Then
                            Dim intFixing As Integer
                            intFixing = EncabezadoEdicionSeleccionado.strFixing.Substring(1)

                            Await CalcularDiasCumplimiento(0, intFixing)
                        End If

                        logCalculoFixing = False
                End Select
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "_EncabezadoEdicionSeleccionado_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para la carga del control específico del detalle de la órden dependiendo del producto, por ejemplo el control de los datos de divisas
    ''' </summary>
    ''' <param name="plogNuevoRegistro"></param>
    Private Sub CargarControlDetalle(Optional ByVal plogNuevoRegistro As Boolean = False)
        Try

            objViewPrincipal.grdDetalle.Children.Clear()

            If plogNuevoRegistro = False Then

                If Not IsNothing(EncabezadoEdicionSeleccionado.strProducto) Then

                    'Se evalúa el mercado para mostrar el formulario correspondiente
                    If EncabezadoEdicionSeleccionado.strProducto = "DIVISAS" Then
                        OrdenesDivisasView = New OrdenesDivisasView(EncabezadoEdicionSeleccionado)

                        AddHandler OrdenesDivisasView.JsonConcatenadoDetalle, AddressOf RespuestaJsonConcatenadoDetalle

                        'SV20181023_AJUSTESORDENES se crea el método de respuesta para el cambio de una propiedad en el detalle
                        AddHandler OrdenesDivisasView.NotificarPropiedad, AddressOf RespuestaValorPropiedadDetalle

                        If Not IsNothing(OrdenesDivisasView) Then
                            objViewPrincipal.grdDetalle.Children.Add(OrdenesDivisasView)
                        End If

                        If Editando Then
                            OrdenesDivisasView.EditarRegistroDetalle = True
                        End If
                        'SV20181023_AJUSTESORDENES
                        IniciarCalcularDiasCumplimiento()


                        'JAPC20200318: C-20200306 Logica para manejo Derivados forward y cargas campos del negocio divisas
                    ElseIf EncabezadoEdicionSeleccionado.strProducto = "DERIVADOS" Then
                        OrdenesDivisasView = New OrdenesDivisasView(EncabezadoEdicionSeleccionado)

                        AddHandler OrdenesDivisasView.JsonConcatenadoDetalle, AddressOf RespuestaJsonConcatenadoDetalle


                        AddHandler OrdenesDivisasView.NotificarPropiedad, AddressOf RespuestaValorPropiedadDetalle

                        If Not IsNothing(OrdenesDivisasView) Then
                            objViewPrincipal.grdDetalle.Children.Add(OrdenesDivisasView)
                        End If

                        If Editando Then
                            OrdenesDivisasView.EditarRegistroDetalle = True
                        End If

                        IniciarCalcularDiasCumplimiento()
                    End If

                End If

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al esperar la carga del control.",
                                 Me.ToString(), "CargarControlDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Recibe un objeto de tipo entidad y se pasan los valores de las propiedades una a una para devolver un complex type
    ''' </summary>
    ''' <param name="objEncabezado"></param>
    ''' <returns></returns>
    Public Function LlevarObjectAComplex(ByVal objEncabezado As tblOrdenes) As CPX_tblOrdenes
        Try
            Dim objCPXEncabezado As New CPX_tblOrdenes

            With objCPXEncabezado
                .intID = objEncabezado.intID
                .intConsecutivo = objEncabezado.intConsecutivo
                .intVersion = objEncabezado.intVersion
                .intIDComitente = objEncabezado.intIDComitente
                .strTipo = objEncabezado.strTipo
                .dtmOrden = objEncabezado.dtmOrden
                .dtmVigenciaHasta = objEncabezado.dtmVigenciaHasta
                .strEstado = objEncabezado.strEstado
                .dtmEstado = objEncabezado.dtmEstado
                .strProducto = objEncabezado.strProducto
                .strClasificacionNegocio = objEncabezado.strClasificacionNegocio
                .strUsuarioCreacion = objEncabezado.strUsuarioCreacion
                .dtmCreacion = objEncabezado.dtmCreacion
                .strUsuario = objEncabezado.strUsuario
                .dtmActualizacion = objEncabezado.dtmActualizacion
                .strTipoDerivado = objEncabezado.strTipoDerivado   'JAPC20200318: C-20200306 Forward divisas nuevos campos
                .strFixing = objEncabezado.strFixing
                .strTipoOperacion = objEncabezado.strTipoOperacion
                .strObjetivo = objEncabezado.strObjetivo
                .strTpoContraparte = objEncabezado.strTpoContraparte

            End With

            Return objCPXEncabezado
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "LlevarObjectAComplex", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    ''' <summary>
    ''' Método adicional para hacer un llamado con un tópico a la base de datos para validar algún tipo de proceso, por ejemplo validad si la órden permite anulación
    ''' </summary>
    ''' <param name="strProceso"></param>
    Public Async Sub Validar_Procesos(ByVal strProceso As String)
        Try
            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesOrdenes)) = Await mdcProxy.Ordenes_Validar_ProcesosAsync(
                                                                      _EncabezadoEdicionSeleccionado.intID,
                                                                       _EncabezadoEdicionSeleccionado.strProducto,
                                                                      strProceso,
                                                                      strConfirmaciones,
                                                                      Program.Usuario)

            Dim objListaResultado As List(Of CPX_tblValidacionesOrdenes) = CType(objRespuesta.Value, List(Of CPX_tblValidacionesOrdenes))
            Dim objListaMensajes As New List(Of ProductoValidaciones)

            For Each li In objListaResultado
                objListaMensajes.Add(New ProductoValidaciones With {
                                                 .Campo = li.strCampo,
                                                 .TituloCampo = MyBase.ObtenerTituloCampo(li.strCampo),
                                                 .Mensaje = li.strMensaje
                                                 })
            Next

            If (objListaResultado.All(Function(i) i.logInconsitencia = False)) Then
                If strProceso = "EDICION" Then
                    IniciaEditar()
                End If

            Else
                Editando = False
                If (objListaResultado.All(Function(i) i.strTipoMensaje = "INCONSISTENCIA")) Then

                    Dim strMensaje As String = ""
                    For Each m In objListaMensajes
                        strMensaje += IIf(String.IsNullOrWhiteSpace(m.TituloCampo), "", m.TituloCampo + ":") + m.Mensaje + vbCrLf
                    Next
                    A2Utilidades.Mensajes.mostrarMensaje(strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                    MyBase.ActualizarListaInconsistencias(objListaMensajes, Program.TituloSistema, False)
                    IsBusy = False
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al hacer la validación del proceso.",
                                 Me.ToString(), "Validar_Procesos", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Proceso para mostrar en pantalla mensajes de confirmación por el usuario
    ''' </summary>
    ''' <param name="strCodigoLlamado"></param>
    Private Sub LanzarMensajesPregunta(ByVal strCodigoLlamado As String)
        Try
            Dim objMensajeSeleccionado As CPX_tblValidacionesOrdenes

            objMensajeSeleccionado = objListaValidaciones.FirstOrDefault

            intCantidadLlamadosConfirmacion = intCantidadLlamadosConfirmacion + 1

            A2Utilidades.Mensajes.mostrarMensajePregunta(objMensajeSeleccionado.strMensaje,
                                     Program.TituloSistema,
                                     strCodigoLlamado,
                                     AddressOf TerminoMensajePregunta,
                                                         False,
                                                         "",
                                                         False,
                                                         False,
                                                         True,
                                                         True,
                                                         objMensajeSeleccionado.strCodMensaje)

            objListaValidaciones.Remove(objMensajeSeleccionado)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "strCodigoLlamado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' recibe la respuesta dada por el usuario al mensaje de confirmación
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TerminoMensajePregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

            If Not IsNothing(objResultado) Then
                If Not IsNothing(objResultado.CodigoLlamado) Then
                    Select Case objResultado.CodigoLlamado.ToUpper
                        Case "VALIDARORDEN"
                            If objResultado.DialogResult Then
                                strConfirmaciones = strConfirmaciones & "," & objResultado.CodConfirmacion
                                If intCantidadLlamadosConfirmacion <> intTotalLlamadosConfirmacion Then
                                    LanzarMensajesPregunta(objResultado.CodigoLlamado)
                                Else
                                    ContinuarActualizarRegistro()
                                End If
                            Else
                                strConfirmaciones = String.Empty
                                IsBusy = False
                            End If
                    End Select
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la respuesta del mensaje pregunta.", Me.ToString(), "TerminoMensajePregunta", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub


#End Region


End Class


''' <summary>
''' REQUERIDO
''' 
''' Clase base para forma de búsquedas 
''' Esta clase se instancia para crear un objeto que guarda los valores seleccionados por el usuario en la forma de búsqueda
''' Sus atributos dependen de los datos del encabezado relevantes en una búsqueda
''' </summary>
''' <history>
''' 
''' </history>
Public Class CamposBusquedaOrdenes
    Implements INotifyPropertyChanged

    Private _intID As Integer?
    Public Property intID() As Integer?
        Get
            Return _intID
        End Get
        Set(ByVal value As Integer?)
            _intID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intID"))
        End Set
    End Property

    Private _intConsecutivo As Long?
    Public Property intConsecutivo() As Long?
        Get
            Return _intConsecutivo
        End Get
        Set(ByVal value As Long?)
            _intConsecutivo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intConsecutivo"))
        End Set
    End Property

    Private _intVersion As Integer?
    Public Property intVersion() As Integer?
        Get
            Return _intVersion
        End Get
        Set(ByVal value As Integer?)
            _intVersion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intVersion"))
        End Set
    End Property

    Private _strProducto As String
    Public Property strProducto() As String
        Get
            Return _strProducto
        End Get
        Set(ByVal value As String)
            _strProducto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strProducto"))
        End Set
    End Property

    Private _intIDComitente As String
    Public Property intIDComitente() As String
        Get
            Return _intIDComitente
        End Get
        Set(ByVal value As String)
            _intIDComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intIDComitente"))
        End Set
    End Property

    Private _strNombre As String
    Public Property strNombre() As String
        Get
            Return _strNombre
        End Get
        Set(ByVal value As String)
            _strNombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strNombre"))
        End Set
    End Property

    Private _strTipo As String
    Public Property strTipo() As String
        Get
            Return _strTipo
        End Get
        Set(ByVal value As String)
            _strTipo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipo"))
        End Set
    End Property

    Private _dtmOrden As Nullable(Of Date)
    Public Property dtmOrden() As Nullable(Of Date)
        Get
            Return _dtmOrden
        End Get
        Set(ByVal value As Nullable(Of Date))
            _dtmOrden = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmOrden"))
        End Set
    End Property

    Private _strEstado As String
    Public Property strEstado() As String
        Get
            Return _strEstado
        End Get
        Set(ByVal value As String)
            _strEstado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strEstado"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class


''' <summary>
''' SV20190503: Clase para manejo de datos adicionales de la órden
''' </summary>
Public Class clsDatosAdicionalesOrden
    Implements INotifyPropertyChanged

    Private _dblValorNeto As Double
    Public Property dblValorNeto() As Double
        Get
            Return _dblValorNeto
        End Get
        Set(ByVal value As Double)
            _dblValorNeto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblValorNeto"))
        End Set
    End Property

    Private _strReferencia As String
    Public Property strReferencia() As String
        Get
            Return _strReferencia
        End Get
        Set(ByVal value As String)
            _strReferencia = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strReferencia"))
        End Set
    End Property



    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class