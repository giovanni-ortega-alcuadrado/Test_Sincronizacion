
Imports System.Threading.Tasks
Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OyD.OYDServer.RIA.WEB

''' <summary>
''' Métodos creados para la comunicación con el OPENRIA (DER_MaestrosDomainServices.vb y dbDER_Maestros.edmx)
''' Pantalla Instrumentos (Maestros)
''' </summary>
''' <remarks>Jhon Alexis Echavarria (Alcuadrado S.A.) - 20 de Marzo 2018</remarks>
''' <history>
'''
'''</history>
Public Class CruceOperacionesViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As DER_OperacionesDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Dim strTipoFiltroBusqueda As String = String.Empty

#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' ID caso de prueba: Id_1
    ''' Creado por       : Jhon Alexis Echavarria (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Marzo 20/2018
    ''' Pruebas CB       :  Jhon Alexis Echavarria (Alcuadrado S.A.) -Marzo 20/2018 - Resultado Ok 
    ''' </history>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            mdcProxy = inicializarProxy()

            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico

                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarEncabezado("FILTRAR", String.Empty)

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

#End Region

#Region "Enumeradores"

    Public Enum Enum_TipoNegocio
        Mercado = 1
        TimeSpread = 2
        Ninguno = -1
    End Enum
#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    ''' <summary>
    ''' Lista de Registros que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As List(Of CPX_tblCRCC_CTRADES)
    Public Property ListaEncabezado() As List(Of CPX_tblCRCC_CTRADES)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As List(Of CPX_tblCRCC_CTRADES))
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

    Private _ListaDatosOperacion As List(Of CPX_tblCRCC_CTRADES)
    Public Property ListaDatosOperacion() As List(Of CPX_tblCRCC_CTRADES)
        Get
            Return _ListaDatosOperacion
        End Get
        Set(ByVal value As List(Of CPX_tblCRCC_CTRADES))
            _ListaDatosOperacion = value
            MyBase.CambioItem("ListaDatosOperacion")
        End Set
    End Property

    Private _DatosOperacionSeleccionado As CPX_tblCRCC_CTRADES
    Public Property DatosOperacionSeleccionado() As CPX_tblCRCC_CTRADES
        Get
            Return _DatosOperacionSeleccionado
        End Get
        Set(ByVal value As CPX_tblCRCC_CTRADES)
            _DatosOperacionSeleccionado = value
            MyBase.CambioItem("DatosOperacionSeleccionado")
        End Set
    End Property

    Private _ListaDatosOperacionTimeSpread As List(Of CPX_tblCRCC_CTRADES_TimeSpread)
    Public Property ListaDatosOperacionTimeSpread() As List(Of CPX_tblCRCC_CTRADES_TimeSpread)
        Get
            Return _ListaDatosOperacionTimeSpread
        End Get
        Set(ByVal value As List(Of CPX_tblCRCC_CTRADES_TimeSpread))
            _ListaDatosOperacionTimeSpread = value
            MyBase.CambioItem("ListaDatosOperacionTimeSpread")
        End Set
    End Property

    Private _DatosOperacionTimeSpreadSeleccionado As CPX_tblCRCC_CTRADES_TimeSpread
    Public Property DatosOperacionTimeSpreadSeleccionado() As CPX_tblCRCC_CTRADES_TimeSpread
        Get
            Return _DatosOperacionTimeSpreadSeleccionado
        End Get
        Set(ByVal value As CPX_tblCRCC_CTRADES_TimeSpread)
            _DatosOperacionTimeSpreadSeleccionado = value
            MyBase.CambioItem("DatosOperacionTimeSpreadSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Elemento de la lista de Registros que se encuentra seleccionado
    ''' </summary>
    Private _EncabezadoSeleccionado As CPX_tblCRCC_CTRADES
    Public Property EncabezadoSeleccionado() As CPX_tblCRCC_CTRADES
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As CPX_tblCRCC_CTRADES)
            _EncabezadoSeleccionado = value

            If Not IsNothing(_EncabezadoSeleccionado) Then
                DatosInicialesCruceManual()
            End If

            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaCruceOperaciones
    Public Property cb() As CamposBusquedaCruceOperaciones
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaCruceOperaciones)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

    Private _HabilitarEdicionDetalle As Boolean = False
    Public Property HabilitarEdicionDetalle() As Boolean
        Get
            Return _HabilitarEdicionDetalle
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEdicionDetalle = value
            MyBase.CambioItem("HabilitarEdicionDetalle")
        End Set
    End Property

    ' propiedad para seleccionar todo
    Private _bitSeleccionarTodo As Boolean = False
    Public Property bitSeleccionarTodo() As Boolean
        Get
            Return _bitSeleccionarTodo
        End Get
        Set(ByVal value As Boolean)
            _bitSeleccionarTodo = value
            MarcarDesmarcarTodo()
            MyBase.CambioItem("bitSeleccionarTodo")
        End Set
    End Property

    ' propiedad las cruzadas
    Private _bitCruzada As Boolean = False
    Public Property bitCruzada() As Boolean
        Get
            Return _bitCruzada
        End Get
        Set(ByVal value As Boolean)
            _bitCruzada = value
            MyBase.CambioItem("bitCruzada")
        End Set
    End Property

    Private _HabilitarBotonDescalzar As Boolean = False
    Public Property HabilitarBotonDescalzar() As Boolean
        Get
            Return _HabilitarBotonDescalzar
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBotonDescalzar = value
            MyBase.CambioItem("HabilitarBotonDescalzar")
        End Set
    End Property

    Private _HabilitarBotonEliminar As Boolean = False
    Public Property HabilitarBotonEliminar() As Boolean
        Get
            Return _HabilitarBotonEliminar
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBotonEliminar = value
            MyBase.CambioItem("HabilitarBotonEliminar")
        End Set
    End Property

    Private _MostrarBotonesEncabezado As Visibility = Visibility.Visible
    Public Property MostrarBotonesEncabezado() As Visibility
        Get
            Return _MostrarBotonesEncabezado
        End Get
        Set(ByVal value As Visibility)
            _MostrarBotonesEncabezado = value
            MyBase.CambioItem("MostrarBotonesEncabezado")
        End Set
    End Property

    Private _MostrarBotonesBusqueda As Visibility = Visibility.Collapsed
    Public Property MostrarBotonesBusqueda() As Visibility
        Get
            Return _MostrarBotonesBusqueda
        End Get
        Set(ByVal value As Visibility)
            _MostrarBotonesBusqueda = value
            MyBase.CambioItem("MostrarBotonesBusqueda")
        End Set
    End Property

    Private _MostrarBotonesCruceManual As Visibility = Visibility.Collapsed
    Public Property MostrarBotonesCruceManual() As Visibility
        Get
            Return _MostrarBotonesCruceManual
        End Get
        Set(ByVal value As Visibility)
            _MostrarBotonesCruceManual = value
            MyBase.CambioItem("MostrarBotonesCruceManual")
        End Set
    End Property

    Private _HabilitarBotonesCruceAutomatico As Boolean = False
    Public Property HabilitarBotonesCruceAutomatico() As Boolean
        Get
            Return _HabilitarBotonesCruceAutomatico
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBotonesCruceAutomatico = value
            MyBase.CambioItem("HabilitarBotonesCruceAutomatico")
        End Set
    End Property

    Private _MostrarGridOperaciones As Visibility = Visibility.Collapsed
    Public Property MostrarGridOperaciones() As Visibility
        Get
            Return _MostrarGridOperaciones
        End Get
        Set(ByVal value As Visibility)
            _MostrarGridOperaciones = value
            MyBase.CambioItem("MostrarGridOperaciones")
        End Set
    End Property

    Private _MostrarGridTimeSpread As Visibility = Visibility.Collapsed
    Public Property MostrarGridTimeSpread() As Visibility
        Get
            Return _MostrarGridTimeSpread
        End Get
        Set(ByVal value As Visibility)
            _MostrarGridTimeSpread = value
            MyBase.CambioItem("MostrarGridTimeSpread")
        End Set
    End Property

    Private _EnumTipoNegocio As Enum_TipoNegocio
    Public Property EnumTipoNegocio() As Enum_TipoNegocio
        Get
            Return _EnumTipoNegocio
        End Get
        Set(ByVal value As Enum_TipoNegocio)
            _EnumTipoNegocio = value
            MyBase.CambioItem("EnumTipoNegocio")
        End Set
    End Property

    Private _intIdValorLiq As Integer = -1
    Public Property intIdValorLiq() As Integer
        Get
            Return _intIdValorLiq
        End Get
        Set(ByVal value As Integer)
            _intIdValorLiq = value
            MyBase.CambioItem("intIdValorLiq")
        End Set
    End Property

    Private _intIdValorLiqTimeSpread As Integer = -1
    Public Property intIdValorLiqTimeSpread() As Integer
        Get
            Return _intIdValorLiqTimeSpread
        End Get
        Set(ByVal value As Integer)
            _intIdValorLiqTimeSpread = value
            MyBase.CambioItem("intIdValorLiqTimeSpread")
        End Set
    End Property

    Private _intValorTimeSpreadInicialGuardado As Integer = -1
    Public Property intValorTimeSpreadInicialGuardado() As Integer
        Get
            Return _intValorTimeSpreadInicialGuardado
        End Get
        Set(ByVal value As Integer)
            _intValorTimeSpreadInicialGuardado = value
            MyBase.CambioItem("intValorTimeSpreadInicialGuardado")
        End Set
    End Property


#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"


    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción "Búsqueda avanzada" de la barra de herramientas.
    ''' Presenta la forma de búsqueda para que el usuario seleccione los valores por los cuales desea buscar dentro de los campos definidos en la forma de búsqueda
    ''' </summary>
    Public Overrides Sub Buscar()
        PrepararNuevaBusqueda()
        MyBase.Buscar()
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Buscar de la forma de búsqueda
    ''' Ejecuta una búsqueda por los campos contenidos en la forma de búsqueda y cuyos valores correspondan con los seleccionados por el usuario
    ''' </summary>
    ''' <history>
    ''' </history>
    Public Overrides Async Sub ConfirmarBuscar()
        Try
            If Not String.IsNullOrEmpty(cb.dtmFechaImportacion) Or Not String.IsNullOrEmpty(cb.strNemotecnico) Or cb.intFolioCamara > 0 Or cb.intFolioBVC > 0 _
                Or Not String.IsNullOrEmpty(cb.strGestionOperaciones) Or (cb.bitCruzada = False Or cb.bitCruzada = True) Then
                Await ConsultarEncabezado("BUSQUEDA", String.Empty, cb.strNemotecnico, cb.intFolioBVC, cb.intFolioCamara, cb.bitCruzada, cb.dtmFechaImportacion, cb.strGestionOperaciones)
                strTipoFiltroBusqueda = "BUSQUEDA"
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Async Function CruceAutomatico() As Task(Of Boolean)
        Dim plogRetorno As Boolean = False
        Dim strAccion As String = ValoresUserState.Actualizar.ToString()

        Try
            MyBase.ActualizarRegistro()

            ErrorForma = String.Empty
            IsBusy = True


            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesGenerales)) = Await mdcProxy.CruceOperaciones_CruceAutomaticoAsync(Program.Usuario, Program.HashConexion)


            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    Dim objListaResultado As List(Of CPX_tblValidacionesGenerales) = CType(objRespuesta.Value, List(Of CPX_tblValidacionesGenerales))
                    Dim objListaMensajes As New List(Of ProductoValidaciones)
                    Dim intIDRegistroActualizado As Integer = -1

                    For Each li In objListaResultado
                        objListaMensajes.Add(New ProductoValidaciones With {
                                             .Campo = li.strCampo,
                                             .TituloCampo = MyBase.ObtenerTituloCampo(li.strCampo),
                                             .Mensaje = li.strMensaje
                                             })
                    Next

                    If (objListaResultado.Where(Function(i) i.logInconsitencia = True).Count) = 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje(objListaResultado.First.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
                        Await ConsultarEncabezado("FILTRAR", String.Empty)
                        plogRetorno = True
                    Else
                        MyBase.ActualizarListaInconsistencias(objListaMensajes, Program.TituloSistema)
                        IsBusy = False
                    End If

                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                    plogRetorno = False
                    IsBusy = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                plogRetorno = False
                IsBusy = False
            End If

        Catch ex As Exception
            IsBusy = False
            plogRetorno = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "CruceAutomatico", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return plogRetorno
    End Function


    Public Async Function Descalzar() As Task(Of Boolean)
        Dim plogRetorno As Boolean = False

        Try

            ErrorForma = String.Empty
            IsBusy = True

            Dim objListaResultado As List(Of CPX_tblValidacionesGenerales) = Nothing
            Dim objListaMensajes As New List(Of ProductoValidaciones)


            For Each listas In ListaEncabezado
                If listas.bitSel Then

                    Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesGenerales)) = Nothing

                    If EnumTipoNegocio = Enum_TipoNegocio.Mercado Then

                        objRespuesta = Await mdcProxy.CruceOperaciones_DescalzarLiquidacionesAsync(listas.intID, True, Program.Usuario, Program.HashConexion)

                    Else

                        If listas.strTradeReference <> String.Empty Then
                            Dim TimeQuery = From q In ListaEncabezado
                                            Where q.strTradeReference = listas.strTradeReference
                                            Select q

                            If TimeQuery.Count > 0 Then

                                Dim IntCont As Integer = 0

                                For Each number In TimeQuery
                                    If TimeQuery.ToList.Count = 1 Then
                                        objRespuesta = Await mdcProxy.CruceOperaciones_DescalzarLiquidacionesAsync(number.intID, False, Program.Usuario, Program.HashConexion)
                                    Else
                                        objRespuesta = Await mdcProxy.CruceOperaciones_DescalzarLiquidacionesAsync(listas.intID, True, Program.Usuario, Program.HashConexion)
                                    End If
                                Next
                            End If
                        End If

                    End If

                    If Not IsNothing(objRespuesta) Then
                        If Not IsNothing(objRespuesta.Value) Then
                            objListaResultado = CType(objRespuesta.Value, List(Of CPX_tblValidacionesGenerales))
                            Dim intIDRegistroActualizado As Integer = -1

                            For Each li In objListaResultado
                                objListaMensajes.Add(New ProductoValidaciones With {
                                                     .Campo = li.strCampo,
                                                     .TituloCampo = MyBase.ObtenerTituloCampo(li.strCampo),
                                                     .Mensaje = li.strMensaje
                                                     })
                            Next


                        Else
                            A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                            plogRetorno = False
                            IsBusy = False
                            Exit For
                        End If
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                        plogRetorno = False
                        IsBusy = False
                        Exit For
                    End If

                End If
            Next

            If (objListaResultado.Where(Function(i) i.logInconsitencia = True).Count) = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje(objListaResultado.First.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Exito)

                EncabezadoSeleccionado = Nothing
                Await ConsultarEncabezado(True, String.Empty)

                plogRetorno = True

            Else
                MyBase.ActualizarListaInconsistencias(objListaMensajes, Program.TituloSistema)
                plogRetorno = False
                IsBusy = False
            End If

        Catch ex As Exception
            IsBusy = False
            plogRetorno = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "Descalzar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return plogRetorno
    End Function


    Public Sub MarcarDesmarcarTodo()

        Dim plogRetorno As Boolean = False
        bitCruzada = False

        Try
            For Each listas In ListaEncabezado
                listas.bitSel = bitSeleccionarTodo
                bitCruzada = listas.bitCazada
            Next

            If bitSeleccionarTodo And bitCruzada Then
                HabilitarBotonDescalzar = True
            Else
                HabilitarBotonDescalzar = False
            End If

            If bitSeleccionarTodo And bitCruzada = False Then
                HabilitarBotonEliminar = True
            Else
                HabilitarBotonEliminar = False
            End If


        Catch ex As Exception
            IsBusy = False
            plogRetorno = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "MarcarDesmarcarTodo", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Sub MarcarDesmarcarUno()

        Dim plogRetorno As Boolean = False

        Try

            Dim chkSel As Boolean = False
            bitCruzada = False

            For Each listas In ListaEncabezado

                If listas.bitSel Then
                    chkSel = True
                    bitCruzada = listas.bitCazada
                    Exit For
                End If
            Next

            If chkSel And bitCruzada Then
                HabilitarBotonDescalzar = True
            Else
                HabilitarBotonDescalzar = False
            End If

            If chkSel = True And bitCruzada = False Then
                HabilitarBotonEliminar = True
            Else
                HabilitarBotonEliminar = False
            End If

        Catch ex As Exception
            IsBusy = False
            plogRetorno = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "MarcarDesmarcarUno", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private Async Function EvaluarTipoContrato(ByVal strTipoContrato As String) As Task

        Try


            Dim objRespuesta As InvokeResult(Of String) = (Await mdcProxy.CruceOperaciones_TipoOperacion_TimeSpreadAsync(Program.Usuario, Program.HashConexion))


            If strTipoContrato = objRespuesta.Value Then
                EnumTipoNegocio = Enum_TipoNegocio.TimeSpread
                MostrarGridTimeSpread = Visibility.Visible
                MostrarGridOperaciones = Visibility.Collapsed
            Else
                EnumTipoNegocio = Enum_TipoNegocio.Mercado
                MostrarGridTimeSpread = Visibility.Collapsed
                MostrarGridOperaciones = Visibility.Visible
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "EvaluarTipoContrato", Application.Current.ToString(), Program.Maquina, ex)
        End Try


    End Function

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Editar de la barra de herramientas.
    ''' Activa la edición del encabezado y del detalle (si aplica) del encabezado activo
    ''' </summary>
    ''' 
    Public Sub CruceOperacionesManual()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then

                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                IsBusy = True

                Editando = True
                MyBase.CambioItem("Editando")
                MyBase.EditarRegistro()

                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Sub GuardarCruceOperaconesManuales()

        Dim intEstadoGuardado As Integer = 0
        Dim logDetalleSpread As Boolean = False

        Try
            IsBusy = True

            If EnumTipoNegocio = Enum_TipoNegocio.Mercado Then

                Await ValidarLiquidacion(EncabezadoSeleccionado.intID, EncabezadoSeleccionado.strTradeReference)

            Else
                logDetalleSpread = ValidarDetalleTimeSpread()

                If logDetalleSpread = True Then
                    If intValorTimeSpreadInicialGuardado <> 0 Then
                        intEstadoGuardado = Await ValidarTimeSpreadInicial(EncabezadoSeleccionado.intID, EncabezadoSeleccionado.strTradeReference)
                        If intEstadoGuardado <> -1 Then
                            Await ValidarLiquidacion(EncabezadoSeleccionado.intID, EncabezadoSeleccionado.strTradeReference, intIdValorLiqTimeSpread)
                        End If
                    Else
                        Await ValidarLiquidacion(EncabezadoSeleccionado.intID, EncabezadoSeleccionado.strTradeReference, intIdValorLiqTimeSpread)
                    End If
                End If

            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ActualizarRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try


    End Sub

    Private Async Function ValidarLiquidacion(ByVal intIdLiq As Integer, ByVal intIdOrden As String, Optional ByVal intIdTimeSpread As Integer = 0) As Task(Of Boolean)

        Dim plogRetorno As Boolean = False
        Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesGenerales)) = Nothing

        Try
            If EnumTipoNegocio = Enum_TipoNegocio.Mercado Then

                objRespuesta = Await mdcProxy.CruceOperaciones_CruceManualAsync(intIdLiq, Nothing, intIdOrden, Program.Usuario, Program.HashConexion)
            Else

                objRespuesta = Await mdcProxy.CruceOperaciones_CruceManualAsync(intIdLiq, intIdTimeSpread, intIdOrden, Program.Usuario, Program.HashConexion)
            End If

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    Dim objListaResultado As List(Of CPX_tblValidacionesGenerales) = CType(objRespuesta.Value, List(Of CPX_tblValidacionesGenerales))
                    Dim objListaMensajes As New List(Of ProductoValidaciones)
                    Dim intIDRegistroActualizado As Integer = -1

                    For Each li In objListaResultado
                        objListaMensajes.Add(New ProductoValidaciones With {
                                         .Campo = li.strCampo,
                                         .TituloCampo = MyBase.ObtenerTituloCampo(li.strCampo),
                                         .Mensaje = li.strMensaje
                                         })
                    Next

                    If (objListaResultado.Where(Function(i) i.logInconsitencia = True).Count) = 0 Then
                        Await ConsultarEncabezado("BUSQUEDA", "", cb.strNemotecnico, cb.intFolioBVC, cb.intFolioCamara, cb.bitCruzada, cb.dtmFechaImportacion, cb.strGestionOperaciones)
                        plogRetorno = True
                    Else
                        MyBase.ActualizarListaInconsistencias(objListaMensajes, Program.TituloSistema)
                        plogRetorno = False
                        IsBusy = False
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                    plogRetorno = False
                    IsBusy = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                plogRetorno = False
                IsBusy = False
            End If

        Catch ex As Exception
            IsBusy = False
            plogRetorno = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ValidarLiquidacion", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return plogRetorno
    End Function

    Private Function ValidarDetalleTimeSpread() As Boolean

        Dim logHabilitar As Boolean = False
        Dim intNroLiq As Integer
        Try
            If Not IsNothing(ListaDatosOperacionTimeSpread) Then
                For Each listas In ListaDatosOperacionTimeSpread
                    If (listas.bitSel) Then
                        logHabilitar = True
                        If listas.IDLiq <> String.Empty Then
                            intNroLiq = listas.IDLiq
                            intIdValorLiqTimeSpread = intNroLiq
                        End If
                        Exit For
                    End If
                Next
            End If
            If logHabilitar = False Then
                A2Utilidades.Mensajes.mostrarMensaje("No hay ninguna operación seleccionada para compra o venta, verifique.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            IsBusy = False
            logHabilitar = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ValidarDetalleTimeSpread", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return logHabilitar
    End Function

    Private Async Function ValidarTimeSpreadInicial(ByVal intIdTimeSpread As Integer, ByVal strNroOrden As String) As Task(Of Integer)
        Dim intValorRetorno As Integer = 0
        Dim plogRetorno As Boolean = False

        Try

            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesGenerales)) = Await mdcProxy.ValidarOrdenes_Liquidacion_InicialTimeSpreadAsync(intIdTimeSpread,
                                                                                                                                                          strNroOrden,
                                                                                                                                                          Program.Usuario,
                                                                                                                                                          Program.HashConexion)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    Dim objListaResultado As List(Of CPX_tblValidacionesGenerales) = CType(objRespuesta.Value, List(Of CPX_tblValidacionesGenerales))
                    Dim objListaMensajes As New List(Of ProductoValidaciones)
                    Dim intIDRegistroActualizado As Integer = -1

                    For Each li In objListaResultado
                        objListaMensajes.Add(New ProductoValidaciones With {
                                             .Campo = li.strCampo,
                                             .TituloCampo = MyBase.ObtenerTituloCampo(li.strCampo),
                                             .Mensaje = li.strMensaje
                                             })
                    Next

                    If (objListaResultado.Where(Function(i) i.logInconsitencia = True).Count) = 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje(objListaResultado.First.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
                        plogRetorno = True
                        intValorRetorno = -1
                    Else
                        MyBase.ActualizarListaInconsistencias(objListaMensajes, Program.TituloSistema)
                        plogRetorno = False
                        IsBusy = False
                        intValorRetorno = -1
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                    plogRetorno = False
                    IsBusy = False
                    intValorRetorno = -1
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                plogRetorno = False
                IsBusy = False
                intValorRetorno = -1
            End If

        Catch ex As Exception
            IsBusy = False
            plogRetorno = False
            intValorRetorno = -1
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ValidarTimeSpreadInicial", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        intValorTimeSpreadInicialGuardado = intValorRetorno
        Return intValorRetorno
    End Function


    Public Async Sub ConsultarLiquidacionesTimeSpread(ByVal intIdLiq As Integer, ByVal intIdOrden As String)

        Dim intEstadoGuardado As Integer = 0

        Try
            IsBusy = True

            If intIdOrden <> String.Empty Then
                intEstadoGuardado = Await ValidarTimeSpreadInicial(intIdLiq, intIdOrden)
                If intEstadoGuardado <> -1 Then

                    Dim objRespuesta As InvokeResult(Of List(Of CPX_tblCRCC_CTRADES_TimeSpread)) = Await mdcProxy.CruceOperaciones_TimeSpread_ConsultarAsync(intIdOrden,
                                                                                                                                                             Program.Usuario,
                                                                                                                                                             Program.HashConexion)

                    If Not IsNothing(objRespuesta) Then
                        If Not IsNothing(objRespuesta.Value) Then
                            ListaDatosOperacionTimeSpread = objRespuesta.Value
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("No se encontraron operaciones de compra o venta asociadas a la operación seleccionada", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                        End If
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("No se encontraron operaciones de compra o venta asociadas a la operación seleccionada", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                    End If

                    MyBase.CambioItem("ListaDatosOperacionTimeSpread")

                End If

            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarLiquidacionesTimeSpread", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub DatosInicialesCruceManual()

        Try
            IsBusy = True

            Dim strTradeReference As String = ""
            Dim intTradeID As Integer = 0
            Dim intIdOperacion As Integer = 0


            strTradeReference = EncabezadoSeleccionado.strTradeReference
            intTradeID = EncabezadoSeleccionado.intTradeID
            intIdOperacion = EncabezadoSeleccionado.intID
            ListaDatosOperacion = Nothing
            ListaDatosOperacionTimeSpread = Nothing



            If strTradeReference <> String.Empty Then
                ListaDatosOperacion = ListaEncabezado.Where(Function(i) i.intID = _EncabezadoSeleccionado.intID).ToList

                If ListaDatosOperacion.Count > 0 Then

                    MostrarGridOperaciones = Visibility.Visible
                    MostrarGridTimeSpread = Visibility.Collapsed

                Else
                    MostrarGridOperaciones = Visibility.Collapsed
                    MostrarGridTimeSpread = Visibility.Visible
                End If

            Else
                MostrarGridOperaciones = Visibility.Collapsed
            End If

            intIdValorLiq = intIdOperacion

            'Debo verificar si se muestra o no los controles para tipo Mercado o Time Spread
            Dim query = (From q In ListaEncabezado
                         Where intIdOperacion.Equals(q.intID) Select q)

            If query.Count > 0 Then
                EvaluarTipoContrato(query.First.strTradeType.ToString)
            End If



            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "DatosInicialesCruceManual", Application.Current.ToString(), Program.Maquina, ex)
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
            MyBase.CambioItem("Editando")
            MyBase.CancelarEditarRegistro()

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Borrar de la barra de herramientas e incia el proceso de eliminación del encabezado activo
    ''' </summary>
    ''' <history>
    ''' </history>
    Public Overrides Sub BorrarRegistro()
        Try
            MyBase.BorrarRegistro()

            Dim logEliminar As Boolean = False

            For Each listas In ListaEncabezado
                If listas.bitSel Then
                    logEliminar = True
                    Exit For
                End If
            Next

            If logEliminar Then
                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                A2Utilidades.Mensajes.mostrarMensajePregunta(DiccionarioMensajesPantalla("GENERICO_ELIMINARREGISTRO"), Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Async Sub BorrarRegistroConfirmado(ByVal sender As Object, ByVal e As EventArgs)
        Try

            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then

                Dim objListaResultado As List(Of CPX_tblValidacionesGenerales) = Nothing
                Dim objListaMensajes As New List(Of ProductoValidaciones)

                For Each listas In ListaEncabezado
                    If listas.bitSel Then

                        IsBusy = True

                        Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesGenerales)) = Await mdcProxy.CruceOperaciones_EliminarAsync(listas.intID, Program.Usuario, Program.HashConexion)

                        If Not IsNothing(objRespuesta) Then
                            If Not IsNothing(objRespuesta.Value) Then
                                objListaResultado = CType(objRespuesta.Value, List(Of CPX_tblValidacionesGenerales))


                                For Each li In objListaResultado
                                    objListaMensajes.Add(New ProductoValidaciones With {
                                                     .Campo = li.strCampo,
                                                     .TituloCampo = MyBase.ObtenerTituloCampo(li.strCampo),
                                                     .Mensaje = li.strMensaje
                                                     })

                                Next


                            Else
                                A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al mostrar la lista de mensajes.", Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                                IsBusy = False
                                Exit For
                            End If
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al mostrar la lista de mensajes.", Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                            IsBusy = False
                            Exit For
                        End If
                    End If
                Next

                If (objListaResultado.Where(Function(i) i.logInconsitencia = True).Count) = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje(objListaResultado.First.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Exito)

                    EncabezadoSeleccionado = Nothing

                    Await ConsultarEncabezado(True, String.Empty)
                Else
                    MyBase.ActualizarListaInconsistencias(objListaMensajes, Program.TituloSistema)
                    IsBusy = False
                End If

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "BorrarRegistroConfirmado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"
    ''' <summary>
    ''' Inicializa el objeto en el cual se capturan los filtros seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaCruceOperaciones
            objCB.dtmFechaImportacion = ""
            objCB.strNemotecnico = ""
            objCB.intFolioCamara = Nothing
            objCB.intFolioBVC = Nothing
            objCB.strGestionOperaciones = ""
            objCB.bitCruzada = False
            cb = objCB
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Consultar de forma sincrónica los datos de Registro
    ''' </summary>
    ''' <param name="pstrOpcion">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    Public Async Function ConsultarEncabezado(ByVal pstrOpcion As String,
                                               Optional ByVal pstrFiltro As String = "",
                                               Optional ByVal pstrNemotecnico As String = "",
                                               Optional ByVal pintFolioBVC As Integer = 0,
                                               Optional ByVal pintFolioCamara As Integer = 0,
                                               Optional ByVal pbitCruzada As Boolean = 0,
                                               Optional ByVal pdtmFechaImportacion As String = "29000101",
                                               Optional ByVal pstrGestionOperaciones As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            IsBusy = True
            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblCRCC_CTRADES)) = Nothing

            ErrorForma = String.Empty

            If pstrOpcion = "FILTRAR" Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRespuesta = Await mdcProxy.CruceOperaciones_ConsultarAsync("", 0, 0, 0, "29000101", "", Program.Usuario, Program.HashConexion)

            Else
                pstrNemotecnico = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrNemotecnico, String.Empty))
                pdtmFechaImportacion = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pdtmFechaImportacion, String.Empty))
                pstrGestionOperaciones = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrGestionOperaciones, String.Empty))

                objRespuesta = Await mdcProxy.CruceOperaciones_ConsultarAsync(pstrNemotecnico, pintFolioBVC, pintFolioCamara, pbitCruzada, pdtmFechaImportacion, pstrGestionOperaciones, Program.Usuario, Program.HashConexion)
            End If

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    ListaEncabezado = objRespuesta.Value

                    If ListaEncabezado.Count > 0 Then
                        If pstrOpcion = "BUSQUEDA" Then
                            MyBase.ConfirmarBuscar()
                        End If

                        If ListaEncabezado.First.bitCazada Then
                            HabilitarBotonesCruceAutomatico = False
                        Else
                            HabilitarBotonesCruceAutomatico = True
                        End If

                    Else
                        If (pstrOpcion = "FILTRAR" And Not String.IsNullOrEmpty(pstrFiltro)) Or (pstrOpcion = "BUSQUEDA") Then
                            ' Solamente se presenta el mensaje cuando se ejecuta la opción de filtrar con un filtro específico o la opción de buscar (consultar) 
                            A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_BUSQUEDANOEXITOSA"), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            CambiarALista()
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
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function


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
Public Class CamposBusquedaCruceOperaciones
    Implements INotifyPropertyChanged

    Private _bitCruzada As Boolean
    Public Property bitCruzada() As Boolean
        Get
            Return _bitCruzada
        End Get
        Set(ByVal value As Boolean)
            _bitCruzada = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("bitCruzada"))
        End Set
    End Property

    Private _dtmFechaImportacion As String
    Public Property dtmFechaImportacion() As String
        Get
            Return _dtmFechaImportacion
        End Get
        Set(ByVal value As String)
            _dtmFechaImportacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmFechaImportacion"))
        End Set
    End Property

    Private _strNemotecnico As String
    Public Property strNemotecnico() As String
        Get
            Return _strNemotecnico
        End Get
        Set(ByVal value As String)
            _strNemotecnico = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strNemotecnico"))
        End Set
    End Property

    Private _intFolioCamara As Integer
    Public Property intFolioCamara() As Integer
        Get
            Return _intFolioCamara
        End Get
        Set(ByVal value As Integer)
            _intFolioCamara = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intFolioCamara"))
        End Set
    End Property

    Private _intFolioBVC As Integer
    Public Property intFolioBVC() As Integer
        Get
            Return _intFolioBVC
        End Get
        Set(ByVal value As Integer)
            _intFolioBVC = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intFolioBVC"))
        End Set
    End Property

    Private _strGestionOperaciones As String
    Public Property strGestionOperaciones() As String
        Get
            Return _strGestionOperaciones
        End Get
        Set(ByVal value As String)
            _strGestionOperaciones = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strGestionOperaciones"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class