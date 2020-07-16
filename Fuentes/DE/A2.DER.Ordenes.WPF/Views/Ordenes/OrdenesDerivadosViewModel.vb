
Imports System.Threading.Tasks
Imports A2Utilidades
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OyD.OYDServer.RIA.WEB
Imports Telerik.Windows.Controls
Imports Newtonsoft.Json



''' <summary>
''' Métodos creados para la comunicación con el OPENRIA (DER_MaestrosDomainServices.vb y dbDER_Maestros.edmx)
''' Pantalla Instrumentos (Maestros)
''' </summary>
''' <remarks>Jhon Alexis Echavarria (Alcuadrado S.A.) - 01 de Marzo 2018</remarks>
''' <history>
'''
'''</history>
Public Class OrdenesDerivadosViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As tblOrdenesDV
    Private mdcProxy As DER_OrdenesDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As tblOrdenesDV
    Private mobjEncabezadoGuardadoAnterior As tblOrdenesDV
    Public viewListaPrincipal As OrdenesDerivadosView
    Private logVentanaDetalleActiva As Boolean = False
    Public logCambiarEncabezadoSeleccionado As Boolean = True
    Private strNombreProducto As String = String.Empty
    Public intIdOrden As Integer
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
    ''' Fecha            : Marzo 01/2018
    ''' Pruebas CB       :  Jhon Alexis Echavarria (Alcuadrado S.A.) -Marzo 01/2018 - Resultado Ok 
    ''' </history>
    Public Function inicializar() As Boolean

        Dim logResultado As Boolean = False

        Try
            mdcProxy = inicializarProxy()

            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                consultarEncabezadoPorDefecto()
                Editando = False
            End If

            If IsNothing(EncabezadoEdicionSeleccionado) Then
                EncabezadoEdicionSeleccionado = New tblOrdenesDV()
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
    ''' Lista de Parametro que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As List(Of CPX_tblOrdenesDV)
    Public Property ListaEncabezado() As List(Of CPX_tblOrdenesDV)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As List(Of CPX_tblOrdenesDV))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value
            ' viewListaPrincipal.ListaDetalle = _ListaEncabezado

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
    Private _EncabezadoSeleccionado As CPX_tblOrdenesDV
    Public Property EncabezadoSeleccionado() As CPX_tblOrdenesDV
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As CPX_tblOrdenesDV)
            _EncabezadoSeleccionado = value
            'Llamado de metodo para realizar la consulta del encabezado editable
            ConsultarEncabezadoEdicion()
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Elemento de la lista de Registros que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoEdicionSeleccionado As tblOrdenesDV
    Public Property EncabezadoEdicionSeleccionado() As tblOrdenesDV
        Get
            Return _EncabezadoEdicionSeleccionado
        End Get
        Set(ByVal value As tblOrdenesDV)
            _EncabezadoEdicionSeleccionado = value
            MyBase.CambioItem("EncabezadoEdicionSeleccionado")
        End Set
    End Property

    Private _IDRegistroEncabezado As Integer
    Public Property IDRegistroEncabezado() As Integer
        Get
            Return _IDRegistroEncabezado
        End Get
        Set(ByVal value As Integer)
            _IDRegistroEncabezado = value
            mobjEncabezadoAnterior = Nothing
            mobjEncabezadoGuardadoAnterior = Nothing
            ConsultarRegistrosEncabezado()
            MyBase.CambioItem("IDRegistroEncabezado")
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

    Private _HabilitarBotonesAcciones As Boolean = False
    Public Property HabilitarBotonesAcciones() As Boolean
        Get
            Return _HabilitarBotonesAcciones
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBotonesAcciones = value
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

    'Public Sub EntregarDetalleJson()
    '    JsonDetalle = String.Empty
    '    'limpia el json a entregar
    '    If Not IsNothing(EncabezadoEdicionSeleccionado) Then
    '        Dim strJsonEntregar As String = JsonConvert.SerializeObject(EncabezadoEdicionSeleccionado)
    '        JsonDetalle = strJsonEntregar

    '        viewPrincipal.EjecutarEventoDetalle()
    '    End If

    '    viewPrincipal.EntregarJsonGuardado = False
    'End Sub



    '------------------------------------------------------------------------------------------------

    Private _MostrarCamposPrecio As Visibility = Visibility.Visible
    Public Property MostrarCamposPrecio() As Visibility
        Get
            Return _MostrarCamposPrecio
        End Get
        Set(ByVal value As Visibility)
            _MostrarCamposPrecio = value
            MyBase.CambioItem("MostrarCamposPrecio")
        End Set
    End Property

    Private _MostrarCamposSpread As Visibility = Visibility.Collapsed
    Public Property MostrarCamposSpread() As Visibility
        Get
            Return _MostrarCamposSpread
        End Get
        Set(ByVal value As Visibility)
            _MostrarCamposSpread = value
            MyBase.CambioItem("MostrarCamposSpread")
        End Set
    End Property

    'Para guardar el Id del tipo Instrumento
    Private _intIdTipoProducto As Integer = -1
    Public Property intIdTipoProducto() As Integer
        Get
            Return _intIdTipoProducto
        End Get
        Set(ByVal value As Integer)
            _intIdTipoProducto = value
            If _intIdTipoProducto > 0 Then

                ListaTipoOpcionProducto = DiccionarioCombosPantalla("TIPOOPCIONPRODUCTO").Where(Function(i) i.IDDependencia1 = _intIdTipoProducto).ToList
                ListaProducto = DiccionarioCombosPantalla("PRODUCTOS").Where(Function(i) i.IDDependencia1 = _intIdTipoProducto).ToList
            End If

            MyBase.CambioItem("intIdTipoProducto")
        End Set
    End Property

    'Para guardar el Id del tipo Instrumento
    Private _intIdProducto As Integer = -1
    Public Property intIdProducto() As Integer
        Get
            Return _intIdProducto
        End Get
        Set(ByVal value As Integer)
            _intIdProducto = value
            If _intIdProducto > 0 Then

                ListaProductoVencimientos = DiccionarioCombosPantalla("PRODUCTOSVENCIMIENTOS").Where(Function(i) i.IDDependencia1 = _intIdProducto).ToList

                consultarNominalInstrumento(_intIdProducto)

            End If
            MyBase.CambioItem("intIdProducto")
        End Set
    End Property

    'Para guardar el Id del tipo Instrumento
    Private _dblNominal As Decimal
    Public Property dblNominal() As Decimal
        Get
            Return _dblNominal
        End Get
        Set(ByVal value As Decimal)
            _dblNominal = value
            MyBase.CambioItem("dblNominal")
        End Set
    End Property

    'Valor del tipo registro por defecto
    Private _intIdTipoRegistroDefecto As Integer = -1
    Public Property intIdTipoRegistroDefecto() As Integer
        Get
            Return _intIdTipoRegistroDefecto
        End Get
        Set(ByVal value As Integer)
            _intIdTipoRegistroDefecto = value
            MyBase.CambioItem("intIdTipoRegistroDefecto")
        End Set
    End Property

    'Lista para SubCuenta CRCC
    Private _ListaSubCuentaCRCC As List(Of ProductoCombos)
    Public Property ListaSubCuentaCRCC() As List(Of ProductoCombos)
        Get
            Return _ListaSubCuentaCRCC
        End Get
        Set(ByVal value As List(Of ProductoCombos))
            _ListaSubCuentaCRCC = value
            MyBase.CambioItem("ListaSubCuentaCRCC")
        End Set
    End Property

    'Lista para Tipo Opcion Producto
    Private _ListaTipoOpcionProducto As List(Of ProductoCombos)
    Public Property ListaTipoOpcionProducto() As List(Of ProductoCombos)
        Get
            Return _ListaTipoOpcionProducto
        End Get
        Set(ByVal value As List(Of ProductoCombos))
            _ListaTipoOpcionProducto = value
            MyBase.CambioItem("ListaTipoOpcionProducto")
        End Set
    End Property

    'Lista para Instrumentos
    Private _ListaProducto As List(Of ProductoCombos)
    Public Property ListaProducto() As List(Of ProductoCombos)
        Get
            Return _ListaProducto
        End Get
        Set(ByVal value As List(Of ProductoCombos))
            _ListaProducto = value
            MyBase.CambioItem("ListaProducto")
        End Set
    End Property

    'Lista para Productos Vencimientos
    Private _ListaProductoVencimientos As List(Of ProductoCombos)
    Public Property ListaProductoVencimientos() As List(Of ProductoCombos)
        Get
            Return _ListaProductoVencimientos
        End Get
        Set(ByVal value As List(Of ProductoCombos))
            _ListaProductoVencimientos = value
            MyBase.CambioItem("ListaProductoVencimientos")
        End Set
    End Property

    Private _bitPrecioStop As Boolean = False
    Public Property bitPrecioStop() As Boolean
        Get
            Return _bitPrecioStop
        End Get
        Set(ByVal value As Boolean)
            _bitPrecioStop = value

            If _bitPrecioStop Then
                Habilitar_PrecioStop = True
            Else
                Habilitar_PrecioStop = False
                EncabezadoEdicionSeleccionado.numPrecioStop = 0
            End If

            MyBase.CambioItem("bitPrecioStop")
        End Set
    End Property

    Private _bitCantidadVisible As Boolean = False
    Public Property bitCantidadVisible() As Boolean
        Get
            Return _bitCantidadVisible
        End Get
        Set(ByVal value As Boolean)
            _bitCantidadVisible = value

            If _bitCantidadVisible Then
                Habilitar_CantidadVisible = True
            Else
                Habilitar_CantidadVisible = False
                EncabezadoEdicionSeleccionado.numCantidadVisible = 0
            End If

            MyBase.CambioItem("bitCantidadVisible")
        End Set
    End Property

    Private _bitSeguirInstrumento As Boolean = False
    Public Property bitSeguirInstrumento() As Boolean
        Get
            Return _bitSeguirInstrumento
        End Get
        Set(ByVal value As Boolean)
            _bitSeguirInstrumento = value

            If _bitSeguirInstrumento Then
                Habilitar_SeguirInstrumento = True
            Else
                Habilitar_SeguirInstrumento = False
                EncabezadoEdicionSeleccionado.intIdInstrumentoSeguimiento = -1
                EncabezadoEdicionSeleccionado.intIdComponentePrecio = -1
            End If

            MyBase.CambioItem("bitSeguirInstrumento")
        End Set
    End Property

    Private _Habilitar_AplicarComision As Boolean = False
    Public Property Habilitar_AplicarComision() As Boolean
        Get
            Return _Habilitar_AplicarComision
        End Get
        Set(ByVal value As Boolean)
            _Habilitar_AplicarComision = value
            MyBase.CambioItem("Habilitar_AplicarComision")
        End Set
    End Property

    Private _Habilitar_TabCondicionesOrden As Boolean = False
    Public Property Habilitar_TabCondicionesOrden() As Boolean
        Get
            Return _Habilitar_TabCondicionesOrden
        End Get
        Set(ByVal value As Boolean)
            _Habilitar_TabCondicionesOrden = value
            MyBase.CambioItem("Habilitar_TabCondicionesOrden")
        End Set
    End Property

    Private _Habilitar_TabGiveOut As Boolean = False
    Public Property Habilitar_TabGiveOut() As Boolean
        Get
            Return _Habilitar_TabGiveOut
        End Get
        Set(ByVal value As Boolean)
            _Habilitar_TabGiveOut = value
            MyBase.CambioItem("Habilitar_TabGiveOut")
        End Set
    End Property

    Private _Habilitar_TipoPrima As Boolean = False
    Public Property Habilitar_TipoPrima() As Boolean
        Get
            Return _Habilitar_TipoPrima
        End Get
        Set(ByVal value As Boolean)
            _Habilitar_TipoPrima = value
            MyBase.CambioItem("Habilitar_TipoPrima")
        End Set
    End Property

    Private _Habilitar_Prima As Boolean = False
    Public Property Habilitar_Prima() As Boolean
        Get
            Return _Habilitar_Prima
        End Get
        Set(ByVal value As Boolean)
            _Habilitar_Prima = value
            MyBase.CambioItem("Habilitar_Prima")
        End Set
    End Property

    Private _Habilitar_PrecioPrima As Boolean = False
    Public Property Habilitar_PrecioPrima() As Boolean
        Get
            Return _Habilitar_PrecioPrima
        End Get
        Set(ByVal value As Boolean)
            _Habilitar_PrecioPrima = value
            MyBase.CambioItem("Habilitar_PrecioPrima")
        End Set
    End Property

    Private _Habilitar_Contraparte As Boolean = False
    Public Property Habilitar_Contraparte() As Boolean
        Get
            Return _Habilitar_Contraparte
        End Get
        Set(ByVal value As Boolean)
            _Habilitar_Contraparte = value
            MyBase.CambioItem("Habilitar_Contraparte")
        End Set
    End Property

    Private _Habilitar_PrecioStop As Boolean = False
    Public Property Habilitar_PrecioStop() As Boolean
        Get
            Return _Habilitar_PrecioStop
        End Get
        Set(ByVal value As Boolean)
            _Habilitar_PrecioStop = value
            MyBase.CambioItem("Habilitar_PrecioStop")
        End Set
    End Property

    Private _Habilitar_CantidadVisible As Boolean = False
    Public Property Habilitar_CantidadVisible() As Boolean
        Get
            Return _Habilitar_CantidadVisible
        End Get
        Set(ByVal value As Boolean)
            _Habilitar_CantidadVisible = value
            MyBase.CambioItem("Habilitar_CantidadVisible")
        End Set
    End Property

    Private _Habilitar_SeguirInstrumento As Boolean = False
    Public Property Habilitar_SeguirInstrumento() As Boolean
        Get
            Return _Habilitar_SeguirInstrumento
        End Get
        Set(ByVal value As Boolean)
            _Habilitar_SeguirInstrumento = value
            MyBase.CambioItem("Habilitar_SeguirInstrumento")
        End Set
    End Property

    Private _Habilitar_VencimientoFinal As Boolean = False
    Public Property Habilitar_VencimientoFinal() As Boolean
        Get
            Return _Habilitar_VencimientoFinal
        End Get
        Set(ByVal value As Boolean)
            _Habilitar_VencimientoFinal = value
            MyBase.CambioItem("Habilitar_VencimientoFinal")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    ''' <history>
    ''' </history>
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

    Public Sub CrearNuevoDetalle(Optional ByVal plogCopiarValorRegistroAnterior As Boolean = False)
        Try
            Dim objNvoEncabezado As New tblOrdenesDV
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
            'Salva el registro anterior
            ObtenerRegistroAnterior(_EncabezadoEdicionSeleccionado, mobjEncabezadoAnterior)

            objNvoEncabezado.intIdordenEnc = _IDRegistroEncabezado

            Editando = True

            If _Habilitar_GuardarYCopiarAnterior Then
                HabilitarBoton_GuardarYCopiarAnterior = True
            End If
            If _Habilitar_GuardarYCrearNuevo Then
                HabilitarBoton_GuardarYCrearNuevo = True
            End If


            MyBase.CambioItem("Editando")

            EncabezadoEdicionSeleccionado = objNvoEncabezado


            Habilitar_Inhabilitar_AplicarComision()
            Habilitar_Inhabilitar_TABCONDICIONESORDEN()
            Habilitar_Inhabilitar_TABGIVEOUT()
            Habilitar_Inhabilitar_PrecioPrima_Prima()

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

                Habilitar_Inhabilitar_AplicarComision()
                Habilitar_Inhabilitar_TABCONDICIONESORDEN()
                Habilitar_Inhabilitar_TABGIVEOUT()
                Habilitar_Inhabilitar_PrecioPrima_Prima()

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
    ''' 

    Public Async Function ActualizarRegistroDetalle() As Task(Of Boolean)
        Dim plogRetorno As Boolean = False
        Dim strAccion As String = ValoresUserState.Actualizar.ToString()

        Try
            MyBase.ActualizarRegistro()

            ErrorForma = String.Empty
            IsBusy = True

            mobjEncabezadoGuardadoAnterior = _EncabezadoEdicionSeleccionado

            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesGenerales)) = Await mdcProxy.OrdenesDV_ActualizarAsync(_EncabezadoEdicionSeleccionado.intIdOrden,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intNroOrden,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdordenEnc,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdTipoOperacion,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdCuentaCRCC,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdSubcuentaCRCC,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdProductoVencimiento,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdProductoEspecial,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdTipoOpcionProducto,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdComercial,
                                                                                                                                  _EncabezadoEdicionSeleccionado.bitRegistradaEnBolsa,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdTipoRegistro,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdContraparte,
                                                                                                                                  _EncabezadoEdicionSeleccionado.numCantidad,
                                                                                                                                  _EncabezadoEdicionSeleccionado.numPrecio,
                                                                                                                                  _EncabezadoEdicionSeleccionado.numPrima,
                                                                                                                                  _EncabezadoEdicionSeleccionado.numComision,
                                                                                                                                  _EncabezadoEdicionSeleccionado.bitComisionPorPorcentaje,
                                                                                                                                  _EncabezadoEdicionSeleccionado.numPorcentajeComision,
                                                                                                                                  _EncabezadoEdicionSeleccionado.bitFacturaComisionInicio,
                                                                                                                                  _EncabezadoEdicionSeleccionado.bitEsGiveOut,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdFirmaGiveOut,
                                                                                                                                  _EncabezadoEdicionSeleccionado.strReferenciaGiveOut,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdTipoOrdenDuracion,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdTipoOrdenEjecucion,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdTipoOrdenNaturaleza,
                                                                                                                                  _EncabezadoEdicionSeleccionado.numCantidadMinima,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdSesion,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intDiasAvisoCumplimiento,
                                                                                                                                  bitPrecioStop,
                                                                                                                                  _EncabezadoEdicionSeleccionado.numPrecioStop,
                                                                                                                                  bitCantidadVisible,
                                                                                                                                  _EncabezadoEdicionSeleccionado.numCantidadVisible,
                                                                                                                                 bitSeguirInstrumento,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdInstrumentoSeguimiento,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdComponentePrecio,
                                                                                                                                  _EncabezadoEdicionSeleccionado.strObservaciones,
                                                                                                                                  Nothing,
                                                                                                                                  _EncabezadoEdicionSeleccionado.strInstruccionesPago,
                                                                                                                                  Nothing,
                                                                                                                                  Nothing,
                                                                                                                                  Nothing,
                                                                                                                                 Nothing,
                                                                                                                                  _EncabezadoEdicionSeleccionado.numPrecioSpot,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdTipoPrima,
                                                                                                                                  _EncabezadoEdicionSeleccionado.numPrecioPrima,
                                                                                                                                    False,
                                                                                                                                    Program.Usuario, Program.HashConexion)


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
                        intIDRegistroActualizado = objListaResultado.First.intIDRegistro
                        Await ConsultarEncabezado("BUSQUEDA", intIDRegistroActualizado)
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
                Editando = False
                HabilitarBoton_GuardarYCopiarAnterior = False
                HabilitarBoton_GuardarYCrearNuevo = False
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

    Private Async Sub BorrarRegistroConfirmado(ByVal sender As Object, ByVal e As EventArgs)
        Try
            '        Dim strAccion As String = ValoresUserState.Actualizar.ToString()

            '        If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
            '            If Not IsNothing(_EncabezadoSeleccionado) Then
            '                IsBusy = True

            '                Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesGenerales)) = Await mdcProxy.DetalleDatosTributarios_EliminarAsync(_EncabezadoSeleccionado.intIdOrden, Program.Usuario)

            '                If Not IsNothing(objRespuesta) Then
            '                    If Not IsNothing(objRespuesta.Value) Then
            '                        Dim objListaResultado As List(Of CPX_tblValidacionesGenerales) = CType(objRespuesta.Value, List(Of CPX_tblValidacionesGenerales))
            '                        Dim objListaMensajes As New List(Of ProductoValidaciones)

            '                        For Each li In objListaResultado
            '                            objListaMensajes.Add(New ProductoValidaciones With {
            '                                         .Campo = li.strCampo,
            '                                         .TituloCampo = MyBase.ObtenerTituloCampo(li.strCampo),
            '                                         .Mensaje = li.strMensaje
            '                                         })
            '                        Next

            '                        If (objListaResultado.Where(Function(i) i.logInconsitencia = True).Count) = 0 Then
            '                            A2Utilidades.Mensajes.mostrarMensaje(objListaResultado.First.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Exito)

            '                            EncabezadoSeleccionado = Nothing

            '                            Await ConsultarEncabezado(String.Empty, IDRegistroEncabezado)
            '                        Else
            '                            MyBase.ActualizarListaInconsistencias(objListaMensajes, Program.TituloSistema)
            '                            IsBusy = False
            '                        End If
            '                    Else
            '                        A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            '                        IsBusy = False
            '                    End If
            '                Else
            '                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            '                    IsBusy = False
            '                End If
            '            End If
            '        End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "BorrarRegistroConfirmado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Sub AbrirDetalle()
        Try
            If logVentanaDetalleActiva = False Then
                Dim objViewDetalle As New OrdenesDerivadosFormView(Me)
                logVentanaDetalleActiva = True
                inicializarValorCampos()
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

            Habilitar_Inhabilitar_AplicarComision()
            Habilitar_Inhabilitar_TABCONDICIONESORDEN()
            Habilitar_Inhabilitar_TABGIVEOUT()
            Habilitar_Inhabilitar_PrecioPrima_Prima()



            HabilitarBoton_GuardarYCopiarAnterior = False
            HabilitarBoton_GuardarYCrearNuevo = False
            inicializarValorCampos()


            MyBase.CambioItem("Editando")
            EncabezadoEdicionSeleccionado = mobjEncabezadoAnterior
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"
    ''' <summary>
    ''' Retorna una copia del encabezado activo. 
    ''' Se hace un "clon" del encabezado activo para poder devolver los cambios y dejar el encabezado activo en su estado original si es necesario.
    ''' </summary>
    ''' <history>
    ''' </history>
    Private Sub ObtenerRegistroAnterior(ByVal pobjRegistro As tblOrdenesDV, ByRef pobjRegistroSalvar As tblOrdenesDV)
        Dim objEncabezado As tblOrdenesDV = New tblOrdenesDV

        Try
            If Not IsNothing(pobjRegistro) Then
                objEncabezado.intIdOrden = pobjRegistro.intIdOrden
                objEncabezado.intIdordenEnc = pobjRegistro.intIdordenEnc
                objEncabezado.intNroOrden = pobjRegistro.intNroOrden
                objEncabezado.intIdTipoOperacion = pobjRegistro.intIdTipoOperacion
                objEncabezado.bitCompra = pobjRegistro.bitCompra
                objEncabezado.intIdCuentaCRCC = pobjRegistro.intIdCuentaCRCC
                objEncabezado.intIdSubcuentaCRCC = pobjRegistro.intIdSubcuentaCRCC
                objEncabezado.intIdProductoVencimiento = pobjRegistro.intIdProductoVencimiento
                objEncabezado.intIdProductoEspecial = pobjRegistro.intIdProductoEspecial
                objEncabezado.intIdTipoOpcionProducto = pobjRegistro.intIdTipoOpcionProducto
                objEncabezado.intIdComercial = pobjRegistro.intIdComercial
                objEncabezado.bitRegistradaEnBolsa = pobjRegistro.bitRegistradaEnBolsa
                objEncabezado.intIdTipoRegistro = pobjRegistro.intIdTipoRegistro
                objEncabezado.intIdContraparte = pobjRegistro.intIdContraparte
                objEncabezado.numCantidad = pobjRegistro.numCantidad
                objEncabezado.numPrecio = pobjRegistro.numPrecio
                objEncabezado.numPrima = pobjRegistro.numPrima
                objEncabezado.numComision = pobjRegistro.numComision
                objEncabezado.bitComisionPorPorcentaje = pobjRegistro.bitComisionPorPorcentaje
                objEncabezado.numPorcentajeComision = pobjRegistro.numPorcentajeComision
                objEncabezado.bitFacturaComisionInicio = pobjRegistro.bitFacturaComisionInicio
                objEncabezado.numCantidadPendiente = pobjRegistro.numCantidadPendiente
                objEncabezado.numSaldoPendiente = pobjRegistro.numSaldoPendiente
                objEncabezado.bitEsGiveOut = pobjRegistro.bitEsGiveOut
                objEncabezado.intIdFirmaGiveOut = pobjRegistro.intIdFirmaGiveOut
                objEncabezado.strReferenciaGiveOut = pobjRegistro.strReferenciaGiveOut
                objEncabezado.intDiasAvisoCumplimiento = pobjRegistro.intDiasAvisoCumplimiento
                objEncabezado.intIdTipoOrdenDuracion = pobjRegistro.intIdTipoOrdenDuracion
                objEncabezado.intIdTipoOrdenEjecucion = pobjRegistro.intIdTipoOrdenEjecucion
                objEncabezado.intIdTipoOrdenNaturaleza = pobjRegistro.intIdTipoOrdenNaturaleza
                objEncabezado.numPrecioStop = pobjRegistro.numPrecioStop
                objEncabezado.numCantidadVisible = pobjRegistro.numCantidadVisible
                objEncabezado.numCantidadMinima = pobjRegistro.numCantidadMinima
                objEncabezado.intIdInstrumentoSeguimiento = pobjRegistro.intIdInstrumentoSeguimiento
                objEncabezado.intIdComponentePrecio = pobjRegistro.intIdComponentePrecio
                objEncabezado.intIdSesion = pobjRegistro.intIdSesion
                objEncabezado.strObservaciones = pobjRegistro.strObservaciones
                objEncabezado.strUsuarioCreacion = pobjRegistro.strUsuarioCreacion
                objEncabezado.dtmActualizacion = pobjRegistro.dtmActualizacion
                objEncabezado.strUsuario = pobjRegistro.strUsuario
                objEncabezado.strInstruccionesPago = pobjRegistro.strInstruccionesPago
                objEncabezado.bitPermiteConsultar = pobjRegistro.bitPermiteConsultar
                objEncabezado.numCantidadCancelada = pobjRegistro.numCantidadCancelada
                objEncabezado.intIdComercialToma = pobjRegistro.intIdComercialToma
                objEncabezado.dtmFechaHoraToma = pobjRegistro.dtmFechaHoraToma
                objEncabezado.intIdCanal = pobjRegistro.intIdCanal
                objEncabezado.intIdMedioVerificable = pobjRegistro.intIdMedioVerificable
                objEncabezado.strDetalleMV = pobjRegistro.strDetalleMV
                objEncabezado.numPrecioSpot = pobjRegistro.numPrecioSpot
                objEncabezado.intIdTipoPrima = pobjRegistro.intIdTipoPrima
                objEncabezado.numPrecioPrima = pobjRegistro.numPrecioPrima
                objEncabezado.strReceptorCliente = pobjRegistro.strReceptorCliente
                objEncabezado.intAplicarComision = pobjRegistro.intAplicarComision
                objEncabezado.strPrimaPor = pobjRegistro.strPrimaPor


                pobjRegistroSalvar = objEncabezado
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    '''' <summary>
    '''' Modificación de las propeiedades del encabezado.
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    Private Sub _EncabezadoEdicionSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EncabezadoEdicionSeleccionado.PropertyChanged
        Try
            If Editando Then
                If e.PropertyName = "intIdCuentaCRCC" Then

                    ListaSubCuentaCRCC = DiccionarioCombosPantalla("CLIENTESSUBCUENTASCRCC").Where(Function(i) i.IDDependencia1 = EncabezadoEdicionSeleccionado.intIdCuentaCRCC).ToList

                ElseIf e.PropertyName = "bitComisionPorPorcentaje" Then

                    Habilitar_Inhabilitar_AplicarComision()

                ElseIf e.PropertyName = "intIdTipoRegistro" Then

                    Habilitar_Inhabilitar_TABCONDICIONESORDEN()

                ElseIf e.PropertyName = "bitEsGiveOut" Then

                    Habilitar_Inhabilitar_TABGIVEOUT()

                ElseIf e.PropertyName = "intIdFirmaGiveOut" Then

                    consultarReferenciaFirmaGiveOut(EncabezadoEdicionSeleccionado.intIdFirmaGiveOut)

                ElseIf e.PropertyName = "intIdTipoPrima" Then

                    Habilitar_Inhabilitar_PrecioPrima_Prima()

                End If


            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "_EncabezadoEdicionSeleccionado_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    'Método para habilitar o inhabilitar el combos de  Aplicar Comisión
    Public Sub Habilitar_Inhabilitar_AplicarComision()
        Try

            If Not IsNothing(EncabezadoEdicionSeleccionado) Then
                If EncabezadoEdicionSeleccionado.bitComisionPorPorcentaje Then
                    If Editando Then
                        Habilitar_AplicarComision = True
                    Else
                        Habilitar_AplicarComision = False
                        EncabezadoEdicionSeleccionado.intAplicarComision = -1
                    End If
                Else
                    Habilitar_AplicarComision = False
                    EncabezadoEdicionSeleccionado.intAplicarComision = -1
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'Método para habilitar o inhabilitar  el tab de condiciones de Orden
    Public Sub Habilitar_Inhabilitar_TABCONDICIONESORDEN()
        Try

            If Not IsNothing(EncabezadoEdicionSeleccionado) Then
                If EncabezadoEdicionSeleccionado.intIdTipoRegistro = intIdTipoRegistroDefecto Then
                    If Editando Then

                        Habilitar_TabCondicionesOrden = True
                        Habilitar_Contraparte = False
                        EncabezadoEdicionSeleccionado.intIdContraparte = -1

                        If bitPrecioStop Then
                            Habilitar_PrecioStop = True
                        Else
                            Habilitar_PrecioStop = False
                            EncabezadoEdicionSeleccionado.numPrecioStop = 0
                        End If

                        If bitCantidadVisible Then
                            Habilitar_CantidadVisible = True
                        Else
                            Habilitar_CantidadVisible = False
                            EncabezadoEdicionSeleccionado.numCantidadVisible = 0
                        End If

                        If bitSeguirInstrumento Then
                            Habilitar_SeguirInstrumento = True
                        Else
                            Habilitar_SeguirInstrumento = False
                            EncabezadoEdicionSeleccionado.intIdInstrumentoSeguimiento = -1
                            EncabezadoEdicionSeleccionado.intIdComponentePrecio = -1
                        End If

                    Else
                        Habilitar_TabCondicionesOrden = False
                        Habilitar_PrecioStop = False
                        Habilitar_CantidadVisible = False
                        Habilitar_SeguirInstrumento = False
                        Habilitar_Contraparte = False
                    End If
                Else
                    Habilitar_TabCondicionesOrden = False
                    Habilitar_PrecioStop = False
                    Habilitar_CantidadVisible = False
                    Habilitar_SeguirInstrumento = False
                    Habilitar_Contraparte = True
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'Método para habilitar o inhabilitar  el tab de GiveOut 
    Public Sub Habilitar_Inhabilitar_TABGIVEOUT()
        Try

            If Not IsNothing(EncabezadoEdicionSeleccionado) Then
                If EncabezadoEdicionSeleccionado.bitEsGiveOut Then
                    If Editando Then
                        Habilitar_TabGiveOut = True
                    Else
                        Habilitar_TabGiveOut = False
                        EncabezadoEdicionSeleccionado.intIdFirmaGiveOut = -1
                        EncabezadoEdicionSeleccionado.strReferenciaGiveOut = ""
                    End If
                Else
                    Habilitar_TabGiveOut = False
                    EncabezadoEdicionSeleccionado.intIdFirmaGiveOut = -1
                    EncabezadoEdicionSeleccionado.strReferenciaGiveOut = ""
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'Método para habilitar o inhabilitar los campos Prima
    Public Sub Habilitar_Inhabilitar_PrecioPrima_Prima()
        Try

            If Not IsNothing(EncabezadoEdicionSeleccionado) Then
                If EncabezadoEdicionSeleccionado.intIdTipoPrima = DiccionarioCombosPantalla("TIPOPRIMA").Where(Function(i) i.Descripcion.ToUpper = "PRECIO").First.ID Then
                    If Editando Then
                        Habilitar_PrecioPrima = True
                        Habilitar_Prima = False
                        EncabezadoEdicionSeleccionado.numPrima = 0
                    Else
                        Habilitar_PrecioPrima = False
                        Habilitar_Prima = False
                    End If
                ElseIf EncabezadoEdicionSeleccionado.intIdTipoPrima = DiccionarioCombosPantalla("TIPOPRIMA").Where(Function(i) i.Descripcion.ToUpper = "VALOR").First.ID Then
                    If Editando Then
                        Habilitar_PrecioPrima = False
                        Habilitar_Prima = True
                        EncabezadoEdicionSeleccionado.numPrecioPrima = 0
                    Else
                        Habilitar_PrecioPrima = False
                        Habilitar_Prima = False
                    End If
                Else
                    Habilitar_PrecioPrima = False
                    Habilitar_Prima = False
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'Metodo Para consultar la Referencia de la firma Give OUT  Seleccionada
    Private Async Sub consultarReferenciaFirmaGiveOut(ByVal intIdFirmaGiveOut As Integer)
        Try
            Dim objRespuesta As InvokeResult(Of tblFirmasGiveUp)

            objRespuesta = Await mdcProxy.FirmasGiveUp_IDAsync(intIdFirmaGiveOut, Program.Usuario, Program.HashConexion)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then

                    EncabezadoEdicionSeleccionado.strReferenciaGiveOut = objRespuesta.Value.strReferenciaGiveOut

                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "consultarReferenciaFirmaGiveOut", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    'Metodo Para consultar el valor del Nominal del instrumento seleccionado
    Private Async Sub consultarNominalInstrumento(ByVal intIdProducto As Integer)
        Try
            Dim objRespuesta As InvokeResult(Of tblProductos)

            objRespuesta = Await mdcProxy.Productos_IDAsync(intIdProducto, Program.Usuario, Program.HashConexion)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then

                    dblNominal = objRespuesta.Value.numValorNominalContrato

                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "consultarNominalInstrumento", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    'Metodo Para consultar el valor del Nominal del instrumento seleccionado
    Private Async Sub consultarCuentasCRCC()
        Try
            Dim objRespuesta As InvokeResult(Of tblProductos)

            objRespuesta = Await mdcProxy.Productos_IDAsync(intIdProducto, Program.Usuario, Program.HashConexion)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then

                    dblNominal = objRespuesta.Value.numValorNominalContrato

                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "consultarCuentasCRCC", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub



    'Método para habilitar o inhabilitar los campos Prima
    Public Sub inicializarValorCampos()
        Try

            intIdTipoProducto = -1
            intIdProducto = -1
            dblNominal = Nothing
            intIdTipoRegistroDefecto = -1

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Consulta los valores por defecto para un nuevo encabezado
    ''' </summary>
    Private Async Sub consultarEncabezadoPorDefecto()
        Try
            Dim objRespuesta As InvokeResult(Of tblOrdenesDV)

            objRespuesta = Await mdcProxy.OrdenesDV_DefectoAsync(Program.Usuario, Program.HashConexion)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    mobjEncabezadoPorDefecto = objRespuesta.Value

                    'Obtener el valor del tipo registro por defecto
                    intIdTipoRegistroDefecto = mobjEncabezadoPorDefecto.intIdTipoRegistro

                Else
                    A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje(DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "consultarEncabezadoPorDefecto", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Async Sub ConsultarRegistrosEncabezado()
        Try
            Await ConsultarEncabezado(String.Empty, IDRegistroEncabezado)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consultar de forma sincrónica los datos de Parametro
    ''' </summary>
    Private Async Function ConsultarEncabezado(ByVal pstrOpcion As String,
                                               ByVal pintIdOrdenEnc As Integer,
                                               Optional ByVal pintIdOrden As Integer = 0,
                                               Optional ByVal pstrFiltro As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            IsBusy = True
            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblOrdenesDV)) = Nothing

            ErrorForma = String.Empty

            If pstrOpcion = "FILTRAR" Then

                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación

                objRespuesta = Await mdcProxy.OrdenesDV_FiltrarAsync(pstrFiltro, Program.Usuario, Program.HashConexion)

            Else
                objRespuesta = Await mdcProxy.OrdenesDV_ConsultarAsync(pintIdOrdenEnc, Program.Usuario, Program.HashConexion)
            End If

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    ListaEncabezado = objRespuesta.Value

                    If pintIdOrden > 0 Then
                        If ListaEncabezado.Where(Function(i) i.intIdOrden = pintIdOrden).Count > 0 Then
                            EncabezadoSeleccionado = ListaEncabezado.Where(Function(i) i.intIdOrden = pintIdOrden).First
                        End If
                    Else
                        If ListaEncabezado.Count = 0 Then
                            EncabezadoSeleccionado = Nothing
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

    Public Async Sub ConsultarEncabezadoEdicion()
        Try
            EncabezadoEdicionSeleccionado = Nothing
            If logCambiarEncabezadoSeleccionado Then
                If Not IsNothing(_EncabezadoSeleccionado) Then
                    If _EncabezadoSeleccionado.intIdOrden > 0 Then
                        Await ConsultarEncabezadoEdicion(_EncabezadoSeleccionado.intIdOrden)
                    End If
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
    ''' <param name="pintID">Indica el ID de la entidad a consultar</param>
    Private Async Function ConsultarEncabezadoEdicion(ByVal pintID As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            IsBusy = True
            Dim objRespuesta As InvokeResult(Of tblOrdenesDV) = Nothing

            ErrorForma = String.Empty

            objRespuesta = Await mdcProxy.OrdenesDV_IDAsync(pintID, Program.Usuario, Program.HashConexion)

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

End Class