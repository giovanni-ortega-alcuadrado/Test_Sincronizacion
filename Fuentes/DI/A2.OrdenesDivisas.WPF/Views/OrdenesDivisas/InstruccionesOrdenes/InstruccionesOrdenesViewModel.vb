' Desarrollo de órdenes y maestros de módulos genericos
' Santiago Alexander Vergara Orrego
' SV20180711_ORDENES

Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web
Imports System.Collections.ObjectModel
Imports System.ComponentModel

Public Class InstruccionesOrdenesViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As CPX_tblOrdenesInstrucciones
    Private mdcProxy As OrdenesDivisasDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As CPX_tblOrdenesInstrucciones
    Private mobjEncabezadoGuardadoAnterior As CPX_tblOrdenesInstrucciones
    Public viewListaPrincipal As InstruccionesOrdenesView
    Private logVentanaDetalleActiva As Boolean = False
    Private strNombreProducto As String = String.Empty

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
    ''' Creado por       : Juan David Correa (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Octubre 30/2017
    ''' Pruebas CB       : Juan David Correa (Alcuadrado S.A.) - Octubre 30/2017 - Resultado Ok 
    ''' </history>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            mdcProxy = inicializarProxy()

            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                Editando = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return logResultado
    End Function

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


#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

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

    ''' <summary>
    ''' SV20181023_AJUSTESORDENES
    ''' Lista de combo de instrucciones dependiendo del tipo de operación
    ''' </summary>
    Private _lstInstrucciones As List(Of CPX_ComboOrdenes)
    Public Property lstInstrucciones() As List(Of CPX_ComboOrdenes)
        Get
            Return _lstInstrucciones
        End Get
        Set(ByVal value As List(Of CPX_ComboOrdenes))
            _lstInstrucciones = value
            CambioItem("lstInstrucciones")
        End Set
    End Property

    ''' <summary>
    ''' RABP20181024_INSTRUCCIONES DE PAGO TRANSFERENCIA BANCARIAS
    ''' Lista de combo de instrucciones dependiendo del tipo de operación
    ''' </summary>
    Private _lstCuentas As List(Of CPX_ComboOrdenes)
    Public Property lstCuentas() As List(Of CPX_ComboOrdenes)
        Get
            Return _lstCuentas
        End Get
        Set(ByVal value As List(Of CPX_ComboOrdenes))
            _lstCuentas = value
            CambioItem("lstCuentas")
        End Set
    End Property

    ''' <summary>
    ''' RABP20181024_Datos de cobro GMF
    ''' </summary>
    Private _NaturalezaCobroGMF As CPX_InstruccionesGMF
    Public Property NaturalezaCobroGMF() As CPX_InstruccionesGMF
        Get
            Return _NaturalezaCobroGMF
        End Get
        Set(ByVal value As CPX_InstruccionesGMF)
            _NaturalezaCobroGMF = value
            CambioItem("NaturalezaCobroGMF")
        End Set
    End Property

    ''' <summary>
    ''' RABP20181101_Marcar Cobro  GMF
    ''' </summary>
    Private _LogGMF As Boolean = False
    Public Property LogGMF() As Boolean
        Get
            Return _LogGMF
        End Get
        Set(ByVal value As Boolean)
            _LogGMF = value
            CambioItem("LogGMF")
        End Set
    End Property
    ''' <summary>
    ''' Lista de Parametro que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As List(Of CPX_tblOrdenesInstrucciones)
    Public Property ListaEncabezado() As List(Of CPX_tblOrdenesInstrucciones)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As List(Of CPX_tblOrdenesInstrucciones))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value
            viewListaPrincipal.ListaDetalle = _ListaEncabezado

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de Parametro para navegar sobre el grid con paginación
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
    ''' Elemento de la lista de Parametro que se encuentra seleccionado
    ''' </summary>
    Private _EncabezadoSeleccionado As CPX_tblOrdenesInstrucciones
    Public Property EncabezadoSeleccionado() As CPX_tblOrdenesInstrucciones
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As CPX_tblOrdenesInstrucciones)
            _EncabezadoSeleccionado = value
            'Llamado de metodo para realizar la consulta del encabezado editable
            ConsultarEncabezadoEdicion()
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    Dim logDesdeViewModel As Boolean = False

    ''' <summary>
    ''' Elemento de la lista de Registros que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoEdicionSeleccionado As CPX_tblOrdenesInstrucciones
    Public Property EncabezadoEdicionSeleccionado() As CPX_tblOrdenesInstrucciones
        Get
            Return _EncabezadoEdicionSeleccionado
        End Get
        Set(ByVal value As CPX_tblOrdenesInstrucciones)
            _EncabezadoEdicionSeleccionado = value
            logDesdeViewModel = True
            If Not IsNothing(_EncabezadoEdicionSeleccionado) Then
                intInstruccionGMF = _EncabezadoEdicionSeleccionado.intIdInstruccion

            Else
                intInstruccionGMF = Nothing
            End If
            logDesdeViewModel = False
            MyBase.CambioItem("EncabezadoEdicionSeleccionado")
        End Set
    End Property

    Private WithEvents _RegistroEncabezado As tblOrdenes
    Public Property RegistroEncabezado() As tblOrdenes
        Get
            Return _RegistroEncabezado
        End Get
        Set(ByVal value As tblOrdenes)
            _RegistroEncabezado = value
            mobjEncabezadoAnterior = Nothing
            mobjEncabezadoGuardadoAnterior = Nothing
            ListaEncabezado = Nothing
            ConsultarInformacionEncabezado()
            consultarEncabezadoPorDefecto()
            MyBase.CambioItem("RegistroEncabezado")
        End Set
    End Property

    Private _DetalleGuardaIndependiente As Boolean = False
    Public Property DetalleGuardaIndependiente() As Boolean
        Get
            Return _DetalleGuardaIndependiente
        End Get
        Set(ByVal value As Boolean)
            _DetalleGuardaIndependiente = value
            If _DetalleGuardaIndependiente Then
                MostrarCamposGuardaIndependiente = Visibility.Visible
            Else
                MostrarCamposGuardaIndependiente = Visibility.Collapsed
            End If
            MyBase.CambioItem("DetalleGuardaIndependiente")
        End Set
    End Property

    Private _MostrarCamposGuardaIndependiente As Visibility = Visibility.Collapsed
    Public Property MostrarCamposGuardaIndependiente() As Visibility
        Get
            Return _MostrarCamposGuardaIndependiente
        End Get
        Set(ByVal value As Visibility)
            _MostrarCamposGuardaIndependiente = value
            MyBase.CambioItem("MostrarCamposGuardaIndependiente")
        End Set
    End Property

    Private _HabilitarBotonesAcciones As Boolean = True
    Public Property HabilitarBotonesAcciones() As Boolean
        Get
            Return _HabilitarBotonesAcciones
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBotonesAcciones = value
            'If _HabilitarBotonesAcciones = False Then
            '    ConsultarInformacionEncabezado()
            'End If
            MyBase.CambioItem("HabilitarBotonesAcciones")
        End Set
    End Property


    Private _Habilitar_GuardarYCopiarAnterior As Boolean = True
    Public Property Habilitar_GuardarYCopiarAnterior() As Boolean
        Get
            Return _Habilitar_GuardarYCopiarAnterior
        End Get
        Set(ByVal value As Boolean)
            _Habilitar_GuardarYCopiarAnterior = value
            MyBase.CambioItem("Habilitar_GuardarYCopiarAnterior")
        End Set
    End Property

    Private _HabilitarBoton_GuardarYCopiarAnterior As Boolean = False
    Public Property HabilitarBoton_GuardarYCopiarAnterior() As Boolean
        Get
            Return _HabilitarBoton_GuardarYCopiarAnterior
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBoton_GuardarYCopiarAnterior = value
            MyBase.CambioItem("HabilitarBoton_GuardarYCopiarAnterior")
        End Set
    End Property

    Private _Habilitar_GuardarYCrearNuevo As Boolean = True
    Public Property Habilitar_GuardarYCrearNuevo() As Boolean
        Get
            Return _Habilitar_GuardarYCrearNuevo
        End Get
        Set(ByVal value As Boolean)
            _Habilitar_GuardarYCrearNuevo = value
            MyBase.CambioItem("Habilitar_GuardarYCrearNuevo")
        End Set
    End Property

    Private _HabilitarBoton_GuardarYCrearNuevo As Boolean = False
    Public Property HabilitarBoton_GuardarYCrearNuevo() As Boolean
        Get
            Return _HabilitarBoton_GuardarYCrearNuevo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBoton_GuardarYCrearNuevo = value
            MyBase.CambioItem("HabilitarBoton_GuardarYCrearNuevo")
        End Set
    End Property

    ''' <summary>
    ''' RABP20181025_variable para poder evaluar si hay cobro GMF
    ''' Valida si se cobra GMF
    ''' </summary>
    Private _intInstruccionGMF As Integer
    Public Property intInstruccionGMF() As Integer
        Get
            Return _intInstruccionGMF
        End Get

        Set(ByVal value As Integer)
            _intInstruccionGMF = value
            If logDesdeViewModel = False Then
                _EncabezadoEdicionSeleccionado.intIdInstruccion = _intInstruccionGMF
                MyBase.CambioItem("EncabezadoEdicionSeleccionado")
                OrdenesInstruccionesGMF(_EncabezadoEdicionSeleccionado.intIdInstruccion, _RegistroEncabezado.strTipo, _RegistroEncabezado.intIDComitente, Nothing, Nothing)
            End If
            MyBase.CambioItem("intInstruccionGMF")
        End Set
    End Property

    'JAPC20200402: Clase con eventos para manejar cambios en datos adicionales de la orden
    Private WithEvents _DatosAdicionalesOrden As clsDatosAdicionalesOrden
    Public Property DatosAdicionalesOrden() As clsDatosAdicionalesOrden
        Get
            Return _DatosAdicionalesOrden
        End Get
        Set(ByVal value As clsDatosAdicionalesOrden)
            _DatosAdicionalesOrden = value

            MyBase.CambioItem("DatosAdicionalesOrden")
        End Set
    End Property


#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"
    '    ''' <summary>
    '    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    '    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    '    ''' </summary>
    '    ''' <history>
    '    ''' </history>
    Public Overrides Sub NuevoRegistroDetalle()
        Try
            CrearNuevoDetalle()
            MyBase.NuevoRegistroDetalle()
            AbrirDetalle()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Sub ConsultaDatosCombo()

        If Not IsNothing(_RegistroEncabezado) AndAlso Not IsNothing(_RegistroEncabezado.intID) Then
            Await OrdenesCombosEspecificos(_RegistroEncabezado.strProducto, "INSTRUCCIONES", _RegistroEncabezado.strTipo, Nothing, Nothing)

        End If

    End Sub


    Public Sub CrearNuevoDetalle(Optional ByVal plogCopiarValorRegistroAnterior As Boolean = False)
        Try
            ConsultaDatosCombo()

            Dim objNvoEncabezado As New CPX_tblOrdenesInstrucciones
            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If plogCopiarValorRegistroAnterior Then
                'Obtiene el registro por defecto
                ObtenerRegistroAnterior(mobjEncabezadoGuardadoAnterior, objNvoEncabezado)
            Else
                'Obtiene el registro por defecto
                ObtenerRegistroAnterior(mobjEncabezadoPorDefecto, objNvoEncabezado)
            End If

            objNvoEncabezado.intIDOrden = RegistroEncabezado.intID

            'SV20190503
            If Not IsNothing(ListaEncabezado) AndAlso ListaEncabezado.Count > 0 Then
                'objNvoEncabezado.dblValor = DatosAdicionalesOrden.dblValorNeto - (From c In ListaEncabezado Select c.dblValor).Sum()
                A2Utilidades.Mensajes.mostrarMensaje("Por el momento no es posible crear mas instrucciones de giro", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)

                Exit Sub
            Else
                objNvoEncabezado.dblValor = DatosAdicionalesOrden.dblValorNeto
            End If

            'Salva el registro anterior
            ObtenerRegistroAnterior(_EncabezadoEdicionSeleccionado, mobjEncabezadoAnterior)

            If Not IsNothing(_ListaEncabezado) Then
                objNvoEncabezado.intID = -(_ListaEncabezado.Count + 1)
            End If

            Editando = True
            If _Habilitar_GuardarYCopiarAnterior Then
                HabilitarBoton_GuardarYCopiarAnterior = True
            End If
            If _Habilitar_GuardarYCrearNuevo Then
                HabilitarBoton_GuardarYCrearNuevo = True
            End If
            MyBase.CambioItem("Editando")

            EncabezadoEdicionSeleccionado = objNvoEncabezado
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "CrearNuevoDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    ''' <history>
    ''' </history>
    Public Overrides Sub EditarRegistroDetalle()
        Try
            If Not IsNothing(_EncabezadoEdicionSeleccionado) Then

                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                IsBusy = True

                ObtenerRegistroAnterior(_EncabezadoEdicionSeleccionado, mobjEncabezadoAnterior)

                Editando = True
                If _Habilitar_GuardarYCopiarAnterior Then
                    HabilitarBoton_GuardarYCopiarAnterior = True
                End If
                If _Habilitar_GuardarYCrearNuevo Then
                    HabilitarBoton_GuardarYCrearNuevo = True
                End If
                MyBase.CambioItem("Editando")

                IsBusy = False

                MyBase.EditarRegistro()
                AbrirDetalle()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón guardar de la barra de herramientas. 
    ''' Ejecuta el proceso que ingresa o actualiza la base de datos con los cambios realizados 
    ''' </summary>
    ''' <history>
    ''' </history>
    Public Function ActualizarRegistroDetalle() As Boolean
        Dim plogRetorno As Boolean = False
        Dim strAccion As String = ValoresUserState.Actualizar.ToString()

        Try
            ErrorForma = String.Empty
            IsBusy = True

            mobjEncabezadoGuardadoAnterior = _EncabezadoEdicionSeleccionado


            Dim objListaEncabezado As New List(Of CPX_tblOrdenesInstrucciones)
            If Not IsNothing(_ListaEncabezado) Then
                For Each li In _ListaEncabezado
                    objListaEncabezado.Add(li)
                Next
            End If

            If objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).Count > 0 Then
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.intIDOrden = _EncabezadoEdicionSeleccionado.intIDOrden
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.intIdInstruccion = _EncabezadoEdicionSeleccionado.intIdInstruccion
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.dblValor = _EncabezadoEdicionSeleccionado.dblValor
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.strDetalle = _EncabezadoEdicionSeleccionado.strDetalle
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.strObservaciones = _EncabezadoEdicionSeleccionado.strObservaciones
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.strNaturalezaOP = _EncabezadoEdicionSeleccionado.strNaturalezaOP
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.logGMF = _EncabezadoEdicionSeleccionado.logGMF
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.strUsuario = _EncabezadoEdicionSeleccionado.strUsuario
                objListaEncabezado.Where(Function(i) i.intID = _EncabezadoEdicionSeleccionado.intID).First.dtmActualizacion = _EncabezadoEdicionSeleccionado.dtmActualizacion
            Else
                Dim objNvoEncabezado As New CPX_tblOrdenesInstrucciones

                objNvoEncabezado.intID = _EncabezadoEdicionSeleccionado.intID
                objNvoEncabezado.intIdInstruccion = _EncabezadoEdicionSeleccionado.intIdInstruccion
                objNvoEncabezado.intIDOrden = _EncabezadoEdicionSeleccionado.intIDOrden
                objNvoEncabezado.dblValor = _EncabezadoEdicionSeleccionado.dblValor
                objNvoEncabezado.strDetalle = _EncabezadoEdicionSeleccionado.strDetalle
                objNvoEncabezado.strObservaciones = _EncabezadoEdicionSeleccionado.strObservaciones
                objNvoEncabezado.logGMF = _EncabezadoEdicionSeleccionado.logGMF
                objNvoEncabezado.strNaturalezaOP = _EncabezadoEdicionSeleccionado.strNaturalezaOP
                objNvoEncabezado.strUsuario = _EncabezadoEdicionSeleccionado.strUsuario
                objNvoEncabezado.dtmActualizacion = _EncabezadoEdicionSeleccionado.dtmActualizacion


                EncabezadoSeleccionado = objNvoEncabezado

                objListaEncabezado.Add(EncabezadoSeleccionado)
            End If

            ListaEncabezado = objListaEncabezado
            plogRetorno = True

            IsBusy = False


        Catch ex As Exception
            IsBusy = False
            plogRetorno = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ActualizarRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return plogRetorno
    End Function

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Editar de la barra de herramientas.
    ''' Activa la edición del encabezado y del detalle (si aplica) del encabezado activo
    ''' </summary>
    Public Sub VisualizarRegistroDetalle()
        Try
            MyBase.ActualizarListaInconsistencias(Nothing, Program.TituloSistema, False)
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If HabilitarBotonesAcciones Then
                    Editando = True
                    If _Habilitar_GuardarYCopiarAnterior Then
                        HabilitarBoton_GuardarYCopiarAnterior = True
                    End If
                    If _Habilitar_GuardarYCrearNuevo Then
                        HabilitarBoton_GuardarYCrearNuevo = True
                    End If
                Else
                    Editando = False
                    HabilitarBoton_GuardarYCopiarAnterior = False
                    HabilitarBoton_GuardarYCrearNuevo = False
                End If
                MyBase.CambioItem("Editando")
                AbrirDetalle()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "EditarRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Borrar de la barra de herramientas e incia el proceso de eliminación del encabezado activo
    ''' </summary>
    ''' <history>
    ''' </history>
    Public Overrides Sub BorrarRegistroDetalle()
        Try
            MyBase.BorrarRegistroDetalle()

            If Not IsNothing(_EncabezadoSeleccionado) Then
                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                A2Utilidades.Mensajes.mostrarMensajePregunta(DiccionarioMensajesPantalla("GENERICO_ELIMINARREGISTRO"), Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "BorrarRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub BorrarRegistroConfirmado(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim strAccion As String = ValoresUserState.Actualizar.ToString()

            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                If Not IsNothing(_EncabezadoSeleccionado) Then
                    Dim objListaDetalle As New List(Of CPX_tblOrdenesInstrucciones)

                    For Each li In ListaEncabezado
                        objListaDetalle.Add(li)
                    Next

                    'If IsNothing(_ListaEncabezadoEliminar) Then
                    '    _ListaEncabezadoEliminar = New List(Of CPX_tblOrdenesInstrucciones)
                    'End If

                    'If _ListaEncabezadoEliminar.Where(Function(i) i.intID = _EncabezadoSeleccionado.intID).Count = 0 Then
                    '    _ListaEncabezadoEliminar.Add(_EncabezadoSeleccionado)
                    'End If

                    If objListaDetalle.Where(Function(i) i.intID = _EncabezadoSeleccionado.intID).Count > 0 Then
                        objListaDetalle.Remove(objListaDetalle.Where(Function(i) i.intID = _EncabezadoSeleccionado.intID).First)
                    End If

                    ListaEncabezado = objListaDetalle
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "BorrarRegistroConfirmado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub AbrirDetalle()
        Try
            If logVentanaDetalleActiva = False Then
                Dim objViewDetalle As New InstruccionesOrdenesModalView(Me)
                objViewDetalle.Owner = Window.GetWindow(Me.viewListaPrincipal)
                logVentanaDetalleActiva = True
                Program.Modal_OwnerMainWindowsPrincipal(objViewDetalle)
                objViewDetalle.ShowDialog()
                ConsultarEncabezadoEdicion()
                logVentanaDetalleActiva = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "AbrirDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    ''' <history>
    ''' </history>
    Public Sub CancelarRegistroDetalle()
        Try
            Editando = False
            HabilitarBoton_GuardarYCopiarAnterior = False
            HabilitarBoton_GuardarYCrearNuevo = False
            MyBase.CambioItem("Editando")
            EncabezadoEdicionSeleccionado = mobjEncabezadoAnterior
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"

    Private Sub ObtenerRegistroAnterior(ByVal pobjRegistro As CPX_tblOrdenesInstrucciones, ByRef pobjRegistroSalvar As CPX_tblOrdenesInstrucciones)
        Dim objEncabezado As CPX_tblOrdenesInstrucciones = New CPX_tblOrdenesInstrucciones

        Try
            If Not IsNothing(pobjRegistro) Then
                objEncabezado.intID = pobjRegistro.intID
                objEncabezado.intIdInstruccion = pobjRegistro.intIdInstruccion
                objEncabezado.intIDOrden = pobjRegistro.intIDOrden
                objEncabezado.dblValor = pobjRegistro.dblValor
                objEncabezado.strDetalle = pobjRegistro.strDetalle
                objEncabezado.strObservaciones = pobjRegistro.strObservaciones
                objEncabezado.logGMF = pobjRegistro.logGMF
                objEncabezado.strNaturalezaOP = pobjRegistro.strNaturalezaOP
                objEncabezado.strUsuario = pobjRegistro.strUsuario
                objEncabezado.dtmActualizacion = pobjRegistro.dtmActualizacion

                pobjRegistroSalvar = objEncabezado
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"
    '    ''' <summary>
    '    ''' Consulta los valores por defecto para un nuevo encabezado
    '    ''' </summary>
    Private Async Sub consultarEncabezadoPorDefecto()
        Try
            Dim objRespuesta As InvokeResult(Of CPX_tblOrdenesInstrucciones)
            If RegistroEncabezado IsNot Nothing Then

                objRespuesta = Await mdcProxy.OrdenesInstrucciones_DefectoAsync(Program.Usuario)

                If Not IsNothing(objRespuesta) Then
                    If Not IsNothing(objRespuesta.Value) Then
                        mobjEncabezadoPorDefecto = objRespuesta.Value
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
    End Sub

    Private Async Sub ConsultarInformacionEncabezado()
        Try
            If Not IsNothing(_RegistroEncabezado) AndAlso Not IsNothing(_RegistroEncabezado.intID) AndAlso _RegistroEncabezado.intID > 0 Then
                Await OrdenesCombos()
                'SV20181023_AJUSTESORDENES Se hace el el llamado para carga del combo de instrucciones según el tipo de operación de la órden
                Await OrdenesCombosEspecificos(_RegistroEncabezado.strProducto, "INSTRUCCIONES", _RegistroEncabezado.strTipo, Nothing, Nothing)
                Await ConsultarEncabezado("ID", _RegistroEncabezado.intID)
                'RABP20181024_Consulta de cuentas para las instrucciones de pago de transferencia de pagos
                Await OrdenesCombosEspecificosCuentasInstrucciones(_RegistroEncabezado.intIDComitente, "CUENTASBANCARIAS", _RegistroEncabezado.strTipo, Nothing, Nothing)
            Else
                Await OrdenesCombos()

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarInformacionEncabezado_Proc", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' SV20181023_AJUSTESORDENES
    ''' Carga de combos específicos de ordenes
    ''' </summary>
    ''' <param name="pstrProducto"></param>
    ''' <param name="pstrCondicionTexto1"></param>
    ''' <param name="pstrCondicionTexto2"></param>
    ''' <param name="pstrCondicionEntero1"></param>
    ''' <param name="pstrCondicionEntero2"></param>
    ''' <returns></returns>
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

                    If pstrCondicionTexto1 = "INSTRUCCIONES" Then
                        lstInstrucciones = objRespuesta.Value
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
    ''' RABP20181024_SE ACIONA NUEVO COMBO DE CUENTAS para las instrucciones de pago transferencias de pago
    ''' Carga de combos específicos de ordenes
    ''' </summary>
    ''' <param name="pstrComitente"></param>
    ''' <param name="pstrCondicionTexto1"></param>
    ''' <param name="pstrCondicionTexto2"></param>
    ''' <param name="pstrCondicionEntero1"></param>
    ''' <param name="pstrCondicionEntero2"></param>
    ''' <returns></returns>
    Public Async Function OrdenesCombosEspecificosCuentasInstrucciones(ByVal pstrComitente As String,
                                        ByVal pstrCondicionTexto1 As String,
                                        ByVal pstrCondicionTexto2 As String,
                                        ByVal pstrCondicionEntero1 As Integer,
                                        ByVal pstrCondicionEntero2 As Integer) As Task

        Try
            Dim objRespuesta As InvokeResult(Of List(Of CPX_ComboOrdenes))

            If RegistroEncabezado.strTipo = "V" Then
                objRespuesta = Await mdcProxy.OrdenesCombosEspecificosAsync(pstrComitente, pstrCondicionTexto1, pstrCondicionTexto2, pstrCondicionEntero1, pstrCondicionEntero2, Program.Usuario)

                If Not IsNothing(objRespuesta) Then
                    If Not IsNothing(objRespuesta.Value) Then

                        If pstrCondicionTexto1 = "CUENTASBANCARIAS" Then
                            lstCuentas = objRespuesta.Value
                        End If

                    Else
                        A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "OrdenesCombosEspecificosCuentasInstrucciones", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function

    ''' <summary>
    ''' RABP20181024_SE ACIONA NUEVO COMBO DE CUENTAS para las instrucciones de pago transferencias de pago
    ''' Carga de naturaleza del GMF
    ''' </summary>
    ''' <param name="pintIdInstruccion"></param>
    ''' <param name="pstrTipo"></param>
    ''' <param name="pintComitente"></param>
    ''' <param name="pstrCuenta"></param>
    ''' <param name="pstrTipoReferencia"></param>

    Private Async Sub OrdenesInstruccionesGMF(ByVal pintIdInstruccion As Integer, ByVal pstrTipo As String, ByVal pintComitente As String, ByVal pstrCuenta As String, ByVal pstrTipoReferencia As String)
        Try
            IsBusy = True
            Dim objRespuesta = Nothing
            Dim user = Program.Usuario


            objRespuesta = Await mdcProxy.InstruccionesGMF_ValidarAsync(pintIdInstruccion, pstrTipo, pintComitente, pstrCuenta, pstrTipoReferencia, user)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    NaturalezaCobroGMF = objRespuesta.Value
                    _EncabezadoEdicionSeleccionado.strNaturalezaOP = NaturalezaCobroGMF.strNaturalezaOP
                    MyBase.CambioItem("EncabezadoEdicionSeleccionado")

                    Await OrdenesCombosEspecificosCuentasInstrucciones(_RegistroEncabezado.intIDComitente, "CUENTASBANCARIAS", _RegistroEncabezado.strTipo, pintIdInstruccion, Nothing)

                    If NaturalezaCobroGMF.strNaturalezaOP = "Sujeta GMF" Then
                        LogGMF = True
                        _EncabezadoEdicionSeleccionado.logGMF = LogGMF
                    End If

                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("DIVISAS_CUENTACOMITENTE"), Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "OrdenesInstruccionesGMF", Program.TituloSistema, Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try
    End Sub
    Private Async Function ConsultarEncabezado(Optional ByVal pstrOpcion As String = "", Optional ByVal pintIDRegistro As Integer = 0) As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            IsBusy = True
            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblOrdenesInstrucciones)) = Nothing

            ErrorForma = String.Empty
            If RegistroEncabezado IsNot Nothing Then

                If pstrOpcion = "ID" Then
                    objRespuesta = Await mdcProxy.OrdenesInstrucciones_ConsultarAsync(pintIDRegistro, Program.Usuario)
                Else
                    objRespuesta = Await mdcProxy.OrdenesInstrucciones_ConsultarAsync(0, Program.Usuario)
                End If

                If Not IsNothing(objRespuesta) Then
                    If Not IsNothing(objRespuesta.Value) Then
                        ListaEncabezado = objRespuesta.Value
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If

                MyBase.CambioItem("ListaEncabezado")
                MyBase.CambioItem("ListaEncabezadoPaginada")

                logResultado = True
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    Public Sub ConsultarEncabezadoEdicion()
        Try
            EncabezadoEdicionSeleccionado = Nothing
            If Not IsNothing(_EncabezadoSeleccionado) Then
                Dim objEncabezadoEdicion As New CPX_tblOrdenesInstrucciones

                objEncabezadoEdicion.intID = _EncabezadoSeleccionado.intID
                objEncabezadoEdicion.intIdInstruccion = _EncabezadoSeleccionado.intIdInstruccion
                objEncabezadoEdicion.dblValor = _EncabezadoSeleccionado.dblValor
                objEncabezadoEdicion.strDetalle = _EncabezadoSeleccionado.strDetalle
                objEncabezadoEdicion.intIDOrden = _EncabezadoSeleccionado.intIDOrden
                objEncabezadoEdicion.strObservaciones = _EncabezadoSeleccionado.strObservaciones
                objEncabezadoEdicion.LogGMF = _EncabezadoSeleccionado.logGMF
                objEncabezadoEdicion.strNaturalezaOP = _EncabezadoSeleccionado.strNaturalezaOP
                objEncabezadoEdicion.strUsuario = _EncabezadoSeleccionado.strUsuario
                objEncabezadoEdicion.dtmActualizacion = _EncabezadoSeleccionado.dtmActualizacion

                EncabezadoEdicionSeleccionado = objEncabezadoEdicion
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
            Dim objRespuesta As InvokeResult(Of CPX_tblOrdenesInstrucciones) = Nothing

            ErrorForma = String.Empty

            objRespuesta = Await mdcProxy.OrdenesInstrucciones_ConsultarIDAsync(pintID, Program.Usuario)

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


    ''' <summary>
    ''' JAPC20200402: Metodo manejador de eventos cuando ocurra cambios en la clase con datos adicionales de la orden
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub _DatosAdicionalesOrden_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _DatosAdicionalesOrden.PropertyChanged
        Select Case e.PropertyName

            Case "dblValorNeto"
                If Not IsNothing(_DatosAdicionalesOrden) Then
                    If Not IsNothing(ListaEncabezado) Then
                        ListaEncabezado.FirstOrDefault.dblValor = DatosAdicionalesOrden.dblValorNeto
                    End If

                End If
        End Select
    End Sub

End Class
