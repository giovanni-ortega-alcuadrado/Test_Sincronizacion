' Desarrollo de órdenes y maestros de módulos genericos
' Santiago Alexander Vergara Orrego
' SV20180711_ORDENES


Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.ComponentModel
Imports A2.OyD.OYDServer.RIA.Web
Imports Newtonsoft.Json
Imports System.Collections.ObjectModel


''' <summary>
''' Métodos creados para la comunicación con el OPENRIA (EjemploPracticoDomainService.vb y dbEjemploPractico.edmx)
''' Pantalla Clientes Beneficiarios (Maestros)
''' </summary>
''' <remarks>Juan David Correa (Alcuadrado S.A.) - 30 de Octubre 2017</remarks>
''' <history>
'''
'''</history>
Public Class OrdenesDivisasViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As tblOrdenesDivisas
    Private mobjEncabezadoPorDefectoMultimoneda As CPX_tblOrdenesDivisasMultimoneda
    Private mdcProxy As OrdenesDivisasDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As tblOrdenesDivisas
    Private mobjEncabezadoGuardadoAnterior As tblOrdenesDivisas
    Private mobjEncabezadoAnteriorMultimoneda As CPX_OrdenesDivisasMultimonedaNotify
    Public viewPrincipal As OrdenesDivisasView
    Private logVentanaDetalleActiva As Boolean = False
    Private strNombreProducto As String = String.Empty
    Public intDiasCumplimiento As Integer 'JAPC20200408_CC20200306-04_Dias cumlplimiento forward variable necesaria 

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
                'inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                If Not IsNothing(_RegistroEncabezadoPadre) Then
                    Await OrdenesDivisasCombosEspecificos("DIVISAS", Nothing, Nothing, Nothing, Nothing)
                    If _RegistroEncabezadoPadre.intID > 0 Then
                        Await ConsultarEncabezadoEdicion(_RegistroEncabezadoPadre.intID)
                        'RABP20190802
                        Await ConsultarEncabezadoMultimonedaEdicion(_RegistroEncabezadoPadre.intID)
                    Else
                        Await consultarEncabezadoPorDefecto()
                        'RABP20190802
                        Await consultarEncabezadoPorDefectoMultimoneda()

                    End If
                End If
            End If

            Editando = True

            If IsNothing(EncabezadoEdicionSeleccionado) Then
                EncabezadoEdicionSeleccionado = New tblOrdenesDivisas()
            End If

            'RABP20190802
            If IsNothing(EncabezadoEdicionSeleccionadoMultimoneda) Then
                EncabezadoEdicionSeleccionadoMultimoneda = New CPX_OrdenesDivisasMultimonedaNotify
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return logResultado
    End Function

#End Region



#Region "Propiedades del Encabezado - REQUERIDO"


    ''' <summary>
    ''' Elemento de la lista de Registros que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoEdicionSeleccionado As tblOrdenesDivisas
    Public Property EncabezadoEdicionSeleccionado() As tblOrdenesDivisas
        Get
            Return _EncabezadoEdicionSeleccionado
        End Get
        Set(ByVal value As tblOrdenesDivisas)
            _EncabezadoEdicionSeleccionado = value
            If Not IsNothing(_EncabezadoEdicionSeleccionado) Then
                'SV20190503
                viewPrincipal.NotificarValorPropiedad("ValorNeto")
            End If
            MyBase.CambioItem("EncabezadoEdicionSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Elemento de la lista de Registros que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoEdicionSeleccionadoMultimoneda As CPX_OrdenesDivisasMultimonedaNotify
    Public Property EncabezadoEdicionSeleccionadoMultimoneda() As CPX_OrdenesDivisasMultimonedaNotify
        Get
            Return _EncabezadoEdicionSeleccionadoMultimoneda
        End Get
        Set(ByVal value As CPX_OrdenesDivisasMultimonedaNotify)
            _EncabezadoEdicionSeleccionadoMultimoneda = value
            If Not IsNothing(_EncabezadoEdicionSeleccionadoMultimoneda) Then
                viewPrincipal.NotificarValorPropiedad("ValorNetoM")
            End If
            MyBase.CambioItem("EncabezadoEdicionSeleccionadoMultimoneda")
        End Set
    End Property


    Private WithEvents _RegistroEncabezadoPadre As tblOrdenes
    Public Property RegistroEncabezadoPadre() As tblOrdenes
        Get
            Return _RegistroEncabezadoPadre
        End Get
        Set(ByVal value As tblOrdenes)
            _RegistroEncabezadoPadre = value

            If _RegistroEncabezadoPadre.intID <= 0 Then
                If IsNothing(mdcProxy) Then
                    mdcProxy = inicializarProxy()
                End If
                consultarEncabezadoPorDefecto()
                'RABP20190802
                consultarEncabezadoPorDefectoMultimoneda()
            End If

            mobjEncabezadoAnterior = Nothing
            mobjEncabezadoGuardadoAnterior = Nothing

            MyBase.CambioItem("RegistroEncabezadoPadre")
        End Set
    End Property


    Private _JsonDetalle As String
    Public Property JsonDetalle() As String
        Get
            Return _JsonDetalle
        End Get
        Set(ByVal value As String)
            _JsonDetalle = value
            CambioItem("JsonDetalle")
        End Set
    End Property


    Private _dblPrecioParcial As Double
    Public Property dblPrecioParcial() As Double
        Get
            Return _dblPrecioParcial
        End Get
        Set(ByVal value As Double)
            _dblPrecioParcial = value
            CalcularPrecioTotal()
            CambioItem("dblPrecioParcial")
        End Set
    End Property

    'RABP20190802
    Private _dblPrecioM As Double
    Public Property dblPrecioM() As Double
        Get
            Return _dblPrecioM
        End Get
        Set(ByVal value As Double)
            _dblPrecioM = value
            CambioItem("dblPrecioM")
        End Set
    End Property

    'RABP20190802
    Private _dblPrecioIntermedioM As Double
    Public Property dblPrecioIntermedioM() As Double
        Get
            Return _dblPrecioIntermedioM
        End Get
        Set(ByVal value As Double)
            _dblPrecioIntermedioM = value
            CalcularValorNetoMultimonedaUSD()
            CambioItem("dblPrecioIntermedioM")
        End Set
    End Property


    Private _dicCombosEspecificos As Dictionary(Of String, ObservableCollection(Of ItemLista))
    Public Property dicCombosEspecificos() As Dictionary(Of String, ObservableCollection(Of ItemLista))
        Get
            Return _dicCombosEspecificos
        End Get
        Set(ByVal value As Dictionary(Of String, ObservableCollection(Of ItemLista)))
            _dicCombosEspecificos = value
            CambioItem("dicCombosEspecificos")
        End Set
    End Property


    Private _lstCiudades As List(Of CPX_ComboOrdenesDivisas)
    Public Property lstCiudades() As List(Of CPX_ComboOrdenesDivisas)
        Get
            Return _lstCiudades
        End Get
        Set(ByVal value As List(Of CPX_ComboOrdenesDivisas))
            _lstCiudades = value
            CambioItem("lstCiudades")
        End Set
    End Property

    Private _lstFormularios As List(Of CPX_ComboOrdenesDivisas)
    Public Property lstFormularios() As List(Of CPX_ComboOrdenesDivisas)
        Get
            Return _lstFormularios
        End Get
        Set(ByVal value As List(Of CPX_ComboOrdenesDivisas))
            _lstFormularios = value
            CambioItem("lstFormularios")
        End Set
    End Property

    Private _lstNumerales As List(Of CPX_ComboOrdenesDivisas)
    Public Property lstNumerales() As List(Of CPX_ComboOrdenesDivisas)
        Get
            Return _lstNumerales
        End Get
        Set(ByVal value As List(Of CPX_ComboOrdenesDivisas))
            _lstNumerales = value
            CambioItem("lstNumerales")
        End Set
    End Property

    'Parámero para indicar si se requiere permisos para mostrar el botón de cumplimiento, Este requiere implementación ya que aun no se sabe como se van a traer los permisos desde las utilidades'
    Private _DIVISAS_REQUIERE_PERMISOS_CUMPLIMIENTO As Boolean
    Public Property DIVISAS_REQUIERE_PERMISOS_CUMPLIMIENTO() As Boolean
        Get
            Return _DIVISAS_REQUIERE_PERMISOS_CUMPLIMIENTO
        End Get
        Set(ByVal value As Boolean)
            _DIVISAS_REQUIERE_PERMISOS_CUMPLIMIENTO = value
            CambioItem("lstNumerales")
        End Set
    End Property

    'RABP20190723_Activar el grid de multimoneda, cuando aplique
    Private _logActivarMultimoneda As Boolean
    Public Property logActivarMultimoneda() As Boolean
        Get
            Return _logActivarMultimoneda
        End Get
        Set(ByVal value As Boolean)
            _logActivarMultimoneda = value
            CambioItem("logActivarMultimoneda")
        End Set
    End Property
    'RABP20200211_Activar el combo de codSwift cuando solo sean compras
    Private _logActivarCodSwift As Boolean
    Public Property logActivarCodSwift() As Boolean
        Get
            Return _logActivarCodSwift
        End Get
        Set(ByVal value As Boolean)
            _logActivarCodSwift = value
            CambioItem("logActivarCodSwift")
        End Set
    End Property


    ''' <summary>
    ''' Propiedad si el tab de multimoneda esta seleccionado
    ''' </summary>
    ''' <remarks>RABP20190808</remarks>
    Private _logTabMultimonedaSeleccionado As Boolean
    Public Property logTabMultimonedaSeleccionado() As Boolean
        Get
            Return _logTabMultimonedaSeleccionado
        End Get
        Set(ByVal value As Boolean)
            _logTabMultimonedaSeleccionado = value
            CambioItem("logTabMultimonedaSeleccionado")
        End Set
    End Property


    ''' <summary>
    ''' JosePineda_JAPC20200318: Propiedad para controlar cuando activar campos forward en pantalla
    ''' CC:C-20200306
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
    ''' JosePineda_JAPC20200520: Propiedad para controlar cuando activar precio parcial divisas
    ''' CC:C-20200366
    ''' </summary>
    Private _HabilitarPrecio As Boolean = True
    Public Property HabilitarPrecio() As Boolean
        Get
            Return _HabilitarPrecio
        End Get
        Set(ByVal value As Boolean)
            _HabilitarPrecio = value
            CambioItem("HabilitarPrecio")
        End Set
    End Property


#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Carga de los combos específicos de la pantalla de divisas
    ''' </summary>
    ''' <param name="pstrProducto"></param>
    ''' <param name="pstrCondicionTexto1"></param>
    ''' <param name="pstrCondicionTexto2"></param>
    ''' <param name="pstrCondicionEntero1"></param>
    ''' <param name="pstrCondicionEntero2"></param>
    ''' <returns></returns>
    Public Async Function OrdenesDivisasCombosEspecificos(ByVal pstrProducto As String,
                                        ByVal pstrCondicionTexto1 As String,
                                        ByVal pstrCondicionTexto2 As String,
                                        ByVal pstrCondicionEntero1 As Integer,
                                        ByVal pstrCondicionEntero2 As Integer) As Task

        Try
            Dim objRespuesta As InvokeResult(Of List(Of CPX_ComboOrdenesDivisas))

            objRespuesta = Await mdcProxy.OrdenesDivisasCombosEspecificosAsync(pstrProducto, pstrCondicionTexto1, pstrCondicionTexto2, pstrCondicionEntero1, pstrCondicionEntero2, Program.Usuario)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then

                    If String.IsNullOrEmpty(pstrCondicionTexto1) Then
                        dicCombosEspecificos = clsGenerales.CargarListasDivisas(objRespuesta.Value)
                    End If

                    If pstrCondicionTexto1 = "CIUDADES" Then
                        lstCiudades = objRespuesta.Value
                    End If

                    If pstrCondicionTexto1 = "FORMULARIOS" Then
                        lstFormularios = objRespuesta.Value
                    End If

                    If pstrCondicionTexto1 = "NUMERALES" Then
                        lstNumerales = objRespuesta.Value
                    End If

                    If pstrCondicionTexto1 = "PARAMETROS" Then
                        If (From I In objRespuesta.Value Where I.strDescripcion = "DIVISAS_REQUIERE_PERMISOS_CUMPLIMIENTO" Select I).FirstOrDefault.strRetorno = "SI" Then
                            DIVISAS_REQUIERE_PERMISOS_CUMPLIMIENTO = True
                        Else
                            DIVISAS_REQUIERE_PERMISOS_CUMPLIMIENTO = False
                        End If

                    End If

                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "OrdenesDivisasCombosEspecificos", Program.TituloSistema, Program.Maquina, ex)
        End Try


    End Function


    Private _EdtandoDetalleDivisas As Boolean
    Public Property EdtandoDetalleDivisas() As Boolean
        Get
            Return _EdtandoDetalleDivisas
        End Get
        Set(ByVal value As Boolean)
            _EdtandoDetalleDivisas = value
            MyBase.CambioItem("EdtandoDetalleDivisas")

            'SV20181106_MOTIVOINVERSIONORDENES: Se añade el campo para seleccionar el destino de la inversión
            If Not IsNothing(EncabezadoEdicionSeleccionado) AndAlso Not IsNothing(EncabezadoEdicionSeleccionado.strTipoReferencia) Then
                If EncabezadoEdicionSeleccionado.strTipoReferencia = "IE" Then
                    logDestinoInversion = True
                Else
                    logDestinoInversion = False
                    EncabezadoEdicionSeleccionado.intDestinoInversion = Nothing
                End If

                If EncabezadoEdicionSeleccionado.strTipoReferencia = "IN" Then
                    logCompensacion = True
                Else
                    logCompensacion = False
                    EncabezadoEdicionSeleccionado.strCompensacion = Nothing
                End If
            Else
                logDestinoInversion = False
                logCompensacion = False
            End If

            If RegistroEncabezadoPadre.strTipo = "C" Then
                logActivarCodSwift = True
            Else
                logActivarCodSwift = False
            End If

        End Set
    End Property

    ''' <summary>
    '''  'SV20181106_MOTIVOINVERSIONORDENES: Se añade el campo para seleccionar el destino de la inversión
    ''' </summary>
    Private _logDestinoInversion As Boolean
    Public Property logDestinoInversion() As Boolean
        Get
            Return _logDestinoInversion
        End Get
        Set(ByVal value As Boolean)
            _logDestinoInversion = value
            MyBase.CambioItem("logDestinoInversion")
        End Set
    End Property


    Private _logCompensacion As Boolean
    Public Property logCompensacion() As Boolean
        Get
            Return _logCompensacion
        End Get
        Set(ByVal value As Boolean)
            _logCompensacion = value
            MyBase.CambioItem("logCompensacion")
        End Set
    End Property


    Public Sub IniciarEditarRegistro()
        Try
            EdtandoDetalleDivisas = True
            mobjEncabezadoAnterior = EncabezadoEdicionSeleccionado
            mobjEncabezadoAnteriorMultimoneda = EncabezadoEdicionSeleccionadoMultimoneda
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "IniciarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    ''' <history>
    ''' </history>
    Public Sub CancelarEditarRegistroDivisas()
        Try
            EdtandoDetalleDivisas = False
            EncabezadoEdicionSeleccionado = mobjEncabezadoAnterior
            mobjEncabezadoAnteriorMultimoneda = EncabezadoEdicionSeleccionadoMultimoneda
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "CancelarEditarRegistroDivisas", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para entregar el json del detalle para grabar
    ''' </summary>
    Public Sub EntregarDetalleJson()
        JsonDetalle = String.Empty
        'limpia el json a entregar
        If Not IsNothing(EncabezadoEdicionSeleccionado) Then
            Dim strJsonEntregar As String = JsonConvert.SerializeObject(EncabezadoEdicionSeleccionado)
            'RABP20190729: Para llevar la inforación a la grabación en ordenes
            Dim strJsonEntregarMultimoneda As String = JsonConvert.SerializeObject(EncabezadoEdicionSeleccionadoMultimoneda)

            JsonDetalle = strJsonEntregar.Substring(0, strJsonEntregar.Length - 1) + "," + strJsonEntregarMultimoneda.Substring(1, strJsonEntregarMultimoneda.Length - 1)

            viewPrincipal.EjecutarEventoDetalle()
        End If

        viewPrincipal.EntregarJsonGuardado = False
    End Sub


    ''' <summary>
    ''' JAPC20200408_CC20200306-04
    ''' Metodo que  entrega dias de cumplimiento forward de la orden
    ''' </summary>
    ''' <param name="intDiasCumplimientoFW"></param>
    Public Sub EntregarDiasCumplimientoForward(ByVal intDiasCumplimientoFW As Integer)
        Try
            intDiasCumplimiento = intDiasCumplimientoFW

            If RegistroEncabezadoPadre.strTipoDerivado = "FWA" And Editando = True Then
                CalcularDevaluacionForward()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "EntregarDiasCumplimientoForward", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos privados del encabezado"


    ''' <summary>
    ''' Calcular el precio total de acuerdo al tipo de operación y el spread
    ''' </summary>
    Private Sub CalcularPrecioTotal()
        If Not IsNothing(_EncabezadoEdicionSeleccionado) Then
            'C-20200366:calculos forward
            If RegistroEncabezadoPadre.strTipoDerivado = "FWA" Then
                If RegistroEncabezadoPadre.strTipo = "C" Then
                    _EncabezadoEdicionSeleccionado.dblPrecio = _EncabezadoEdicionSeleccionado.dblValorTasaForward - _EncabezadoEdicionSeleccionado.dblSpreadComision
                End If
                If RegistroEncabezadoPadre.strTipo = "V" Then
                    _EncabezadoEdicionSeleccionado.dblPrecio = _EncabezadoEdicionSeleccionado.dblValorTasaForward + _EncabezadoEdicionSeleccionado.dblSpreadComision
                End If
            Else
                If RegistroEncabezadoPadre.strTipo = "C" Then
                    _EncabezadoEdicionSeleccionado.dblPrecio = _EncabezadoEdicionSeleccionado.dblPrecioParcial - _EncabezadoEdicionSeleccionado.dblSpreadComision
                End If
                If RegistroEncabezadoPadre.strTipo = "V" Then
                    _EncabezadoEdicionSeleccionado.dblPrecio = _EncabezadoEdicionSeleccionado.dblPrecioParcial + _EncabezadoEdicionSeleccionado.dblSpreadComision
                End If
            End If
            CalcularValorBruto()
        End If
    End Sub

    ''' <summary>
    ''' Calcular el valor bruto, cantidad por precio
    ''' </summary>
    Private Sub CalcularValorBruto()
        If Not IsNothing(_EncabezadoEdicionSeleccionado) Then
            _EncabezadoEdicionSeleccionado.dblValorBruto = _EncabezadoEdicionSeleccionado.dblPrecio * _EncabezadoEdicionSeleccionado.dblCantidad
            CalcularValorNeto()

        End If
    End Sub

    ''' <summary>
    ''' Cálculo del valor neto de la Orden
    ''' </summary>
    Private Sub CalcularValorNeto()
        If Not IsNothing(_EncabezadoEdicionSeleccionado) Then
            If RegistroEncabezadoPadre.strTipo = "V" Then
                _EncabezadoEdicionSeleccionado.dblValorNeto = _EncabezadoEdicionSeleccionado.dblValorBruto - _EncabezadoEdicionSeleccionado.dblValorGMF - _EncabezadoEdicionSeleccionado.dblValorRteFuente
            End If
            If RegistroEncabezadoPadre.strTipo = "C" Then
                _EncabezadoEdicionSeleccionado.dblValorNeto = _EncabezadoEdicionSeleccionado.dblValorBruto
            End If
        End If
    End Sub

    ''' <summary>
    ''' Cálculo del valor neto de la Orden
    ''' </summary>
    Private Sub CalcularPrecioParcial()
        If Not IsNothing(_EncabezadoEdicionSeleccionado) Then
            If RegistroEncabezadoPadre.strTipo = "V" Then
                dblPrecioParcial = _EncabezadoEdicionSeleccionado.dblPrecio - _EncabezadoEdicionSeleccionado.dblSpreadComision
            End If
            If RegistroEncabezadoPadre.strTipo = "C" Then
                dblPrecioParcial = _EncabezadoEdicionSeleccionado.dblPrecio + _EncabezadoEdicionSeleccionado.dblSpreadComision
            End If
        End If
    End Sub
#End Region

#Region "Métodos privados del encabezado para multimoneda"

    ''' <summary>
    ''' Calcular el valor bruto, cantidad por precio
    ''' </summary>
    Private Sub CalcularValorBrutoMultimoneda()
        If Not IsNothing(_EncabezadoEdicionSeleccionadoMultimoneda) Then
            _EncabezadoEdicionSeleccionadoMultimoneda.dblValorBrutoMultimoneda = _EncabezadoEdicionSeleccionadoMultimoneda.dblPrecioIntermedioM * _EncabezadoEdicionSeleccionadoMultimoneda.dblCantidadMultimoneda
            CalcularValorNetoMultimonedaUSD()
        End If
    End Sub


    ''' <summary>
    ''' Cálculo del valor neto de la Orden
    ''' </summary>
    Private Sub CalcularValorNetoMultimonedaUSD()
        If Not IsNothing(_EncabezadoEdicionSeleccionadoMultimoneda) Then
            If RegistroEncabezadoPadre.strTipo = "V" Then
                _EncabezadoEdicionSeleccionadoMultimoneda.dblPrecioMonedaNegociadaM = _EncabezadoEdicionSeleccionadoMultimoneda.dblValorBrutoMultimoneda - _EncabezadoEdicionSeleccionadoMultimoneda.dblValorGMFM - _EncabezadoEdicionSeleccionadoMultimoneda.dblValorRteFuenteM
            End If
            If RegistroEncabezadoPadre.strTipo = "C" Then
                _EncabezadoEdicionSeleccionadoMultimoneda.dblPrecioMonedaNegociadaM = _EncabezadoEdicionSeleccionadoMultimoneda.dblValorBrutoMultimoneda + _EncabezadoEdicionSeleccionadoMultimoneda.dblComisionUSD
            End If
            CalcularCantidadCOP()
        End If
    End Sub

    ''' <summary>
    ''' Calcular la cantidad de los valores USD para la cantidad de COP
    ''' </summary>
    Private Sub CalcularCantidadCOP()
        If Not IsNothing(_EncabezadoEdicionSeleccionadoMultimoneda) Then
            _EncabezadoEdicionSeleccionadoMultimoneda.dblCantidadM = _EncabezadoEdicionSeleccionadoMultimoneda.dblPrecioMonedaNegociadaM
            CalcularValorBrutoultimoneda()
        End If
    End Sub


    ''' <summary>
    ''' Cálculo del valor neto de la Orden
    ''' </summary>
    Private Sub CalcularValorNetoMultimonedaCOP()
        If Not IsNothing(_EncabezadoEdicionSeleccionadoMultimoneda) Then
            If RegistroEncabezadoPadre.strTipo = "V" Then
                _EncabezadoEdicionSeleccionadoMultimoneda.dblValorNetoM = _EncabezadoEdicionSeleccionadoMultimoneda.dblValorBrutoM - _EncabezadoEdicionSeleccionadoMultimoneda.dblValorGMFM - _EncabezadoEdicionSeleccionadoMultimoneda.dblValorRteFuenteM
            End If
            If RegistroEncabezadoPadre.strTipo = "C" Then
                _EncabezadoEdicionSeleccionadoMultimoneda.dblValorNetoM = _EncabezadoEdicionSeleccionadoMultimoneda.dblValorBrutoM + _EncabezadoEdicionSeleccionadoMultimoneda.dblComisionCOP
            End If
            LlevarValoresaDetalle()
        End If
    End Sub

    ''' <summary>
    ''' Llevar los valores al detalle de la orden en COP
    ''' </summary>
    Private Sub LlevarValoresaDetalle()
        _EncabezadoEdicionSeleccionado.dblCantidad = _EncabezadoEdicionSeleccionadoMultimoneda.dblPrecioMonedaNegociadaM
        _EncabezadoEdicionSeleccionado.dblValorBruto = _EncabezadoEdicionSeleccionadoMultimoneda.dblValorBrutoM
        dblPrecioParcial = _EncabezadoEdicionSeleccionadoMultimoneda.dblPrecioM
    End Sub


    ''' <summary>
    ''' Cálculo del valor neto de la Orden
    ''' </summary>
    Private Sub CalcularValorBrutoultimoneda()
        If Not IsNothing(_EncabezadoEdicionSeleccionadoMultimoneda) Then
            _EncabezadoEdicionSeleccionadoMultimoneda.dblValorBrutoM = _EncabezadoEdicionSeleccionadoMultimoneda.dblCantidadM * _EncabezadoEdicionSeleccionadoMultimoneda.dblPrecioM
            CalcularValorNetoMultimonedaCOP()
        End If
    End Sub
#End Region


#Region "Métodos sincrónicos del encabezado - REQUERIDO"

    '    ''' <summary>
    '    ''' Consulta los valores por defecto para un nuevo encabezado
    '    ''' </summary>
    Private Async Function consultarEncabezadoPorDefecto() As Task
        Try
            Dim objRespuesta As InvokeResult(Of List(Of tblOrdenesDivisas))
            If RegistroEncabezadoPadre IsNot Nothing Then

                objRespuesta = Await mdcProxy.OrdenesDivisas_DefectoAsync(Program.Usuario)

                If Not IsNothing(objRespuesta) Then
                    If Not IsNothing(objRespuesta.Value) Then
                        mobjEncabezadoPorDefecto = objRespuesta.Value.FirstOrDefault
                        EncabezadoEdicionSeleccionado = mobjEncabezadoPorDefecto
                        CalcularPrecioParcial()
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "consultarEncabezadoPorDefecto", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function

    '    ''' <summary>
    '    ''' RABP20190729: Consulta los valores por defecto para un nuevo encabezado  (Consulta de la tabla de multimoneda)
    '    ''' </summary>
    Private Async Function consultarEncabezadoPorDefectoMultimoneda() As Task
        Try
            'RABP20190729: Consulta de la tabla de multimoeda
            Dim objRespuestaMultimoneda As InvokeResult(Of CPX_tblOrdenesDivisasMultimoneda)


            If RegistroEncabezadoPadre IsNot Nothing Then

                objRespuestaMultimoneda = Await mdcProxy.OrdenesDivisasMultimoneda_DefectoAsync(Program.Usuario)

                If Not IsNothing(objRespuestaMultimoneda) Then
                    If Not IsNothing(objRespuestaMultimoneda.Value) Then
                        mobjEncabezadoPorDefectoMultimoneda = objRespuestaMultimoneda.Value
                        EncabezadoEdicionSeleccionadoMultimoneda = ConvertirACPX_OrdenesDivisasMultimonedaINotify(mobjEncabezadoPorDefectoMultimoneda)
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "consultarEncabezadoPorDefecto", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function

    ''' <summary>
    ''' Consulta los datos del registro a editar
    ''' </summary>
    Public Sub ConsultarEncabezadoEdicion()
        Try
            EncabezadoEdicionSeleccionado = Nothing
            If Not IsNothing(_EncabezadoEdicionSeleccionado) Then
                Dim objEncabezadoEdicion As New tblOrdenesDivisas
                EncabezadoEdicionSeleccionado = objEncabezadoEdicion

            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' RABP20190729 Consulta los datos del registro a editar
    ''' </summary>
    Public Sub ConsultarEncabezadoMultimonedaEdicion()
        Try
            EncabezadoEdicionSeleccionadoMultimoneda = Nothing

            If Not IsNothing(_EncabezadoEdicionSeleccionadoMultimoneda) Then
                Dim objEncabezadoEdicionMultimoneda As New CPX_OrdenesDivisasMultimonedaNotify
                EncabezadoEdicionSeleccionadoMultimoneda = objEncabezadoEdicionMultimoneda
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consultar de forma sincrónica los datos del Registro a editar
    ''' </summary>
    ''' <param name="pintID">Indica el ID de la entidad a consultar</param>
    Private Async Function ConsultarEncabezadoEdicion(ByVal pintID As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            IsBusy = True
            Dim objRespuesta As InvokeResult(Of List(Of tblOrdenesDivisas)) = Nothing

            ErrorForma = String.Empty

            objRespuesta = Await mdcProxy.OrdenesDivisas_ConsultarAsync(pintID, Program.Usuario)

            If objRespuesta.Value.Count > 0 Then

                'RABP20200210: Se coloca esta validación ya que cuando no se guarda los formularios y el país no genere error de NUll
                If IsNothing(objRespuesta.Value.FirstOrDefault.intIDFormulario) Then
                    Await OrdenesDivisasCombosEspecificos("DIVISAS", "PARAMETROS", Nothing, Nothing, Nothing)
                Else
                    Await OrdenesDivisasCombosEspecificos("DIVISAS", "FORMULARIOS", IIf(Not IsNothing(objRespuesta.Value.FirstOrDefault.strTipoReferencia), objRespuesta.Value.FirstOrDefault.strTipoReferencia, Nothing), Nothing, Nothing)
                    Await OrdenesDivisasCombosEspecificos("DIVISAS", "NUMERALES", Nothing, IIf(Not IsNothing(objRespuesta.Value.FirstOrDefault.intIDFormulario), objRespuesta.Value.FirstOrDefault.intIDFormulario, Nothing), Nothing)
                    Await OrdenesDivisasCombosEspecificos("DIVISAS", "PARAMETROS", Nothing, Nothing, Nothing)
                    Await OrdenesDivisasCombosEspecificos("DIVISAS", "CIUDADES", Nothing, IIf(Not IsNothing(objRespuesta.Value.FirstOrDefault.intIdPais), objRespuesta.Value.FirstOrDefault.intIdPais, Nothing), Nothing)
                End If

                EncabezadoEdicionSeleccionado = objRespuesta.Value.FirstOrDefault

            Else
                consultarEncabezadoPorDefecto()
                CalcularPrecioParcial()
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarEncabezadoEdicion", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' RABP20190729: Consultar de forma sincrónica los datos del Registro a editar
    ''' </summary>
    ''' <param name="pintID">Indica el ID de la entidad a consultar</param>
    Private Async Function ConsultarEncabezadoMultimonedaEdicion(ByVal pintID As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            IsBusy = True
            Dim objRespuestaMultimoneda As InvokeResult(Of List(Of CPX_tblOrdenesDivisasMultimoneda)) = Nothing

            ErrorForma = String.Empty

            objRespuestaMultimoneda = Await mdcProxy.OrdenesDivisasMultimonedas_ConsultarAsync(pintID, Program.Usuario)


            If objRespuestaMultimoneda.Value.Count > 0 Then
                EncabezadoEdicionSeleccionadoMultimoneda = ConvertirACPX_OrdenesDivisasMultimonedaINotify(objRespuestaMultimoneda.Value.FirstOrDefault)

                If _EncabezadoEdicionSeleccionado.intIDOrden = objRespuestaMultimoneda.Value.FirstOrDefault.intIDOrden Then
                    logActivarMultimoneda = True
                End If


            Else
                consultarEncabezadoPorDefectoMultimoneda()
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarEncabezadoMultimonedaEdicion", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Consultar las monedas que son cambio cruzado
    '''  Ricardo Barrientos Pérez
    '''  20190723
    ''' </summary>
    ''' <param name="pintIDMoneda">Indica el ID de la entidad a consultar</param>
    Public Async Sub ConsultarMultimonedaCambioCruzado(ByVal pintIDMoneda As Integer, ByVal pstrUsuario As String)

        Try
            Dim objRespuesta As InvokeResult(Of List(Of CPX_MultimonedaCambioCruzado))
            Dim CambioCruzado As Integer

            If RegistroEncabezadoPadre IsNot Nothing Then

                objRespuesta = Await mdcProxy.MultimonedasCambioCruzado_ConsultarAsync(pintIDMoneda, Program.Usuario)

                If Not IsNothing(objRespuesta) Then
                    If Not IsNothing(objRespuesta.Value) Then
                        CambioCruzado = objRespuesta.Value.FirstOrDefault.logCambioCruzado

                        If CambioCruzado = True Then
                            MessageBox.Show("Selecciono par de moneda diferente al dolar, por favor llenar los datos en el tab de multimoneda")
                            logTabMultimonedaSeleccionado = True
                            logActivarMultimoneda = CambioCruzado
                            EncabezadoEdicionSeleccionadoMultimoneda.dblComisionUSD = objRespuesta.Value.FirstOrDefault.numComisionMonedaOrigen
                            EncabezadoEdicionSeleccionadoMultimoneda.dblComisionCOP = objRespuesta.Value.FirstOrDefault.numComisionMonedaDestino
                            EncabezadoEdicionSeleccionadoMultimoneda.dblPrecioIntermedioM = objRespuesta.Value.FirstOrDefault.dblValor
                            EncabezadoEdicionSeleccionadoMultimoneda.dblPrecioM = objRespuesta.Value.FirstOrDefault.dblValorUSD

                        Else
                            logActivarMultimoneda = False
                        End If

                    Else
                        A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
                viewPrincipal.NotificarValorPropiedad("Multimoneda")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarMultimonedaCambioCruzado", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub



    ''' <summary>
    '''   JAPC20200408_CC20200306-04
    '''   Metodo para calcular la devaluacion forward de la orden segun curvas
    ''' </summary>
    Public Async Sub CalcularDevaluacionForward()
        Try
            IsBusy = True
            Dim objRespuestaDevaluacion As InvokeResult(Of CPX_tblOrdenesDivisasDevaluacion) = Nothing


            If Not IsNothing(_EncabezadoEdicionSeleccionado.intIDMoneda) AndAlso intDiasCumplimiento > 0 Then
                objRespuestaDevaluacion = Await mdcProxy.OrdenesDivisasDevaluacion_ConsultarAsync(_EncabezadoEdicionSeleccionado.intIDMoneda, intDiasCumplimiento, DateAdd(DateInterval.Day, -1, Date.Now), "T", 0, 0, Program.Usuario)
            End If


            If Not IsNothing(objRespuestaDevaluacion) Then
                If Not IsNothing(objRespuestaDevaluacion.Value.dblDevaluacionForward) Then
                    _EncabezadoEdicionSeleccionado.dblDevaluacion = 0
                    _EncabezadoEdicionSeleccionado.dblDevaluacion = objRespuestaDevaluacion.Value.dblDevaluacionForward
                Else
                    _EncabezadoEdicionSeleccionado.dblDevaluacion = 0
                End If

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "CalcularDevaluacionForward", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub


    ''' <summary>
    ''' JAPC20200408_CC20200306-04
    ''' Metodo para calcular valor tasa forward  teniendo ya la devaluacion segun curvas y TRM
    ''' </summary>
    Public Sub CalcularTasaForward()
        Try
            IsBusy = True
            'C-20200366:calculos forward
            If Not IsNothing(_EncabezadoEdicionSeleccionado.dblDevaluacion) And Not IsNothing(_EncabezadoEdicionSeleccionado.dblSpot) Then
                _EncabezadoEdicionSeleccionado.dblValorTasaForward = _EncabezadoEdicionSeleccionado.dblSpot * (1 + _EncabezadoEdicionSeleccionado.dblDevaluacion) ^ (intDiasCumplimiento / 360)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "CalcularTasaForward", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub



    ''' <summary>
    ''' Cambio de una de las propiedades del encabezado cuando está en edición
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Async Sub _EncabezadoEdicionSeleccionado_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _EncabezadoEdicionSeleccionado.PropertyChanged
        Select Case e.PropertyName
            Case "dblCantidad"
                CalcularPrecioTotal()

            Case "dblSpreadComision"
                CalcularPrecioTotal()

            Case "dblValorRteFuente"
                CalcularValorNeto()

            Case "dblValorIVA"
                CalcularValorNeto()

            Case "dblValorGMF"
                CalcularValorNeto()

            Case "intIdPais"
                _EncabezadoEdicionSeleccionado.intIdciudad = Nothing
                lstCiudades = Nothing
                If Not IsNothing(_EncabezadoEdicionSeleccionado.intIdPais) Then
                    Await OrdenesDivisasCombosEspecificos("DIVISAS", "CIUDADES", Nothing, _EncabezadoEdicionSeleccionado.intIdPais, Nothing)
                End If

            Case "strTipoReferencia"
                EncabezadoEdicionSeleccionado.intIDFormulario = Nothing
                EncabezadoEdicionSeleccionado.intIDNumeral = Nothing
                lstFormularios = Nothing
                lstNumerales = Nothing
                If Not IsNothing(_EncabezadoEdicionSeleccionado.strTipoReferencia) Then
                    Await OrdenesDivisasCombosEspecificos("DIVISAS", "FORMULARIOS", _EncabezadoEdicionSeleccionado.strTipoReferencia, Nothing, Nothing)
                    'SV20181106_MOTIVOINVERSIONORDENES: Se añade el campo para seleccionar el destino de la inversión
                    If EncabezadoEdicionSeleccionado.strTipoReferencia = "IE" Then
                        logDestinoInversion = True
                    Else
                        logDestinoInversion = False
                        EncabezadoEdicionSeleccionado.intDestinoInversion = Nothing
                    End If

                    If EncabezadoEdicionSeleccionado.strTipoReferencia = "IN" Then
                        logCompensacion = True
                    Else
                        logCompensacion = False
                        EncabezadoEdicionSeleccionado.strCompensacion = Nothing
                    End If

                Else
                    logDestinoInversion = False
                    logCompensacion = False
                End If
                viewPrincipal.NotificarValorPropiedad("strReferencia")

            Case "intIDFormulario"
                EncabezadoEdicionSeleccionado.intIDNumeral = Nothing
                lstNumerales = Nothing
                If Not IsNothing(_EncabezadoEdicionSeleccionado.intIDFormulario) Then
                    Await OrdenesDivisasCombosEspecificos("DIVISAS", "NUMERALES", Nothing, _EncabezadoEdicionSeleccionado.intIDFormulario, Nothing)
                End If

            Case "intIDMoneda"
                'SV20181023_AJUSTESORDENES llamado para notificar el cambio de valor de la propiedad
                viewPrincipal.NotificarValorPropiedad("Moneda")
                ConsultarMultimonedaCambioCruzado(EncabezadoEdicionSeleccionado.intIDMoneda, Program.Usuario)
            'SV20190503
            Case "dblValorNeto"
                viewPrincipal.NotificarValorPropiedad("ValorNeto")
                'RABP20190614

            Case "dblPrecioParcial"
                CalcularPrecioTotal()

            ''JAPC20200408_CC20200306-04_Evento sobre calculo devaluacion para calcular tasa forward
            Case "dblDevaluacion"
                CalcularTasaForward()
            'JAPC20200408_CC20200306-03_Ajuste divisas forward tipo de cumplimiento DELIVERY, el campo de FIXING debe estar fijo en T + 0, si el tipo de cumplimiento es NONDELIVERY si se debe activar el campo de FIXING
            Case "strTipoCumplimiento"
                viewPrincipal.NotificarValorPropiedad("TipoCumplimiento")

            'C-20200366:calculos forward
            Case "dblValorTasaForward"
                CalcularPrecioTotal()
            Case "dblSpot"
                If RegistroEncabezadoPadre.strTipoDerivado = "FWA" And Editando = True Then
                    CalcularDevaluacionForward()
                End If
        End Select
    End Sub

#Region "Metodos para convertir tipocomplejo a propertychange"
    Public Function ConvertirACPX_OrdenesDivisasMultimonedaINotify(ByRef pobjRegistro As CPX_tblOrdenesDivisasMultimoneda) As CPX_OrdenesDivisasMultimonedaNotify
        Dim objEncabezadoNuevo = New CPX_OrdenesDivisasMultimonedaNotify With {
            .intIDMonedaIntermediaM = pobjRegistro.intIDMonedaIntermediaM,
            .dblCantidadMultimoneda = pobjRegistro.dblCantidadMultimoneda,
            .dblPrecioIntermedioM = pobjRegistro.dblPrecioIntermedioM,
            .dblSpreadComisionM = pobjRegistro.dblSpreadComisionM,
            .dblValorBrutoMultimoneda = pobjRegistro.dblValorBrutoMultimoneda,
            .dblPrecioMonedaNegociadaM = pobjRegistro.dblPrecioMonedaNegociadaM,
            .dblComisionUSD = pobjRegistro.dblComisionUSD,
            .dblCantidadM = pobjRegistro.dblCantidadM,
            .dblPrecioM = pobjRegistro.dblPrecioM,
            .dblValorBrutoM = pobjRegistro.dblValorBrutoM,
            .dblValorNetoM = pobjRegistro.dblValorNetoM,
            .dblComisionCOP = pobjRegistro.dblComisionCOP,
            .dblValorRteFuenteM = pobjRegistro.dblValorRteFuenteM,
            .dblValorGMFM = pobjRegistro.dblValorGMFM,
            .dblBaseIVAM = pobjRegistro.dblBaseIVAM,
            .dblValorIVAM = pobjRegistro.dblValorIVAM
        }

        Return objEncabezadoNuevo
    End Function

    ''' <summary>
    ''' RABP20200211: Cambio de una de las propiedades del encabezado cuando está en edición y solo se debe activivar el combo de Swift cuando son compras
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub _RegistroEncabezadoPadre_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _RegistroEncabezadoPadre.PropertyChanged
        Select Case e.PropertyName

            Case "strTipo"
                If RegistroEncabezadoPadre.strTipo = "C" Then
                    logActivarCodSwift = True
                Else
                    logActivarCodSwift = False
                End If
            Case "strTipoDerivado" 'JosePineda_JAPC20200318 CCC-20200306 Logica con eventos sobre el enzabezado de la orden para habilitar tipo cumplmiento solo para forward
                If RegistroEncabezadoPadre.strTipoDerivado = "FWA" Then
                    HabilitarForward = True
                    'HabilitarPrecio = False
                Else
                    HabilitarForward = False
                    'HabilitarPrecio = True
                End If

        End Select
    End Sub

    ''' <summary>
    ''' Cambio de una de las propiedades del encabezado cuando está en edición
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub _ConvertirACPX_OrdenesDivisasMultimonedaINotify_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _EncabezadoEdicionSeleccionadoMultimoneda.PropertyChanged
        Select Case e.PropertyName
            Case "dblCantidadMultimoneda"
                CalcularValorBrutoMultimoneda()

            Case "dblPrecioIntermedioM"
                CalcularValorBrutoMultimoneda()

            Case "dblPrecioM"

                CalcularValorBrutoMultimoneda()
                LlevarValoresaDetalle()

            Case "dblSpreadComisionM"
                CalcularValorBrutoMultimoneda()

            Case "dblComisionUSD"
                CalcularValorBrutoMultimoneda()

            Case "dblValorRteFuenteM"
                CalcularValorNetoMultimonedaUSD()

            Case "dblValorIVAM"
                CalcularValorNetoMultimonedaUSD()

            Case "dblValorGMFM"
                CalcularValorNetoMultimonedaUSD()

            Case "dblValorNetoM"
                viewPrincipal.NotificarValorPropiedad("ValorNetoM")

        End Select
    End Sub
#End Region

#Region "Comandos"
    Private WithEvents _CumplirCmd As RelayCommand
    Public ReadOnly Property CumplirCmd() As RelayCommand
        Get
            If _CumplirCmd Is Nothing Then
                _CumplirCmd = New RelayCommand(AddressOf Cumplir)
            End If
            Return _CumplirCmd
        End Get

    End Property
#End Region

    Public Sub Cumplir()
        Try
            If Not IsNothing(EncabezadoEdicionSeleccionado) And Not IsNothing(RegistroEncabezadoPadre) Then
                If RegistroEncabezadoPadre.strEstado <> "CPL" Then
                    If RegistroEncabezadoPadre.intID > 1 Then
                        Dim objViewDetalle As New CumplirOrdenModalView(Me)
                        objViewDetalle.Owner = Window.GetWindow(Me.viewPrincipal)
                        Program.Modal_OwnerMainWindowsPrincipal(objViewDetalle)
                        objViewDetalle.ShowDialog()
                    Else
                        MessageBox.Show("Se debe grabar la órden antes de generar su cumplimiento")
                    End If
                Else
                    MessageBox.Show("La orden ya está cumplida")
                End If
            Else
                MessageBox.Show("No hay una orden seleccionada")
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "AbrirDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

#End Region

End Class
