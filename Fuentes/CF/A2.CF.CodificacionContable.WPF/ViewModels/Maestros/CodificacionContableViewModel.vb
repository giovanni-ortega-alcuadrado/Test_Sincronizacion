Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades
Imports System.Web
Imports A2.OyD.OYDServer.RIA.Web.CFCodificacionContable
Imports GalaSoft.MvvmLight.Command

Public Class CodificacionContableViewModel
    Inherits A2ControlMenu.A2ViewModel

    Public Event CambioItemLista(ByVal value As CFCodificacionContable.ConfiguracionContableXModulo, ByVal Nombre As String)

    ''' <summary>
    ''' ViewModel para la pantalla Codificación contable.
    ''' </summary>
    ''' <history>
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Marzo 31/2014
    ''' Pruebas CB       : Jorge Peña - Febrero 31/2014 - Resultado Ok 
    ''' </history>

#Region "Constantes"

    Private Const STR_MODULO As String = "Modulos"
    Private Const STR_CENTRO_COSTOS_FIJO As String = "03"
    Private Const STR_DURACION_DIAS As String = "DuracionDias"
    Private Const STR_TIPO_FECHA As String = "TipoFecha"
    Private Const STR_RETORNO_DURACION_DIAS As String = "T+0"

    Private Const STR_NEGOCIO As String = "Negocio"
    Private Const STR_RETORNO_NEGOCIO As String = "perdida por valoracion"
    Private Const INT_MODULO_CALC_FINANCIEROS As Integer = 7

    Public Shadows Enum CombosCodificacionContable
        TIPOSMODULOS
        TODOS
        MODULO
        MODULOBUSQUEDA
    End Enum

    Private Enum Encabezado
        intModulo
        intDuracion
        intNegocio
    End Enum

    Public Enum Detalles
        cmCodificacion
        strCentroCostos
    End Enum

#End Region

#Region "Variables - REQUERIDO"

    Public ViewCodificacionContable As CodificacionContableView = Nothing
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As CFCodificacionContable.CodificacionContable
    Private mobjDetallePorDefecto As CFCodificacionContable.CodificacionContableDetalle
    Private mdcProxy As CodificacionContableDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Dim cwCodificacionContableValor As cwCodificacionContableValor
    Dim cwCodificacionContableCCostos As cwCodificacionContableCCostos
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As CFCodificacionContable.CodificacionContable

#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        Try
            IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la construcción del objeto de negocio.", Me.ToString(), "New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Jorge Peña
    ''' Descripción      : Creacion.
    ''' Fecha            : Febrero 21/2014
    ''' Pruebas CB       : Jorge Peña - Febrero 21/2014 - Resultado Ok 
    ''' </history>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                ConsultarEncabezadoPorDefectoSync()
                ConsultarDetallePorDefectoSync(Detalles.cmCodificacion)

                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarEncabezado(True, String.Empty)

                ' Consultar los registros de la tabla tblConfiguracionContableXValor incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarListaCodificacionContable(Nothing, Nothing)

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function
#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaCodificacionContable
    Public Property cb() As CamposBusquedaCodificacionContable
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaCodificacionContable)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

    ''' <summary>
    ''' Indica la pestaña del tab de detalle seleccionado. Por defecto se selecciona el primero
    ''' </summary>
    Public Property TabSeleccionado As Short = 1

    ''' <summary>
    ''' Lista los registros que se encuentran en la tabla tblConfiguracionContableXModulo para llenar los combos del encabezado
    ''' </summary>
    Private _ListaCodificacionContableCompleta As List(Of CFCodificacionContable.ConfiguracionContableXModulo)
    Public Property ListaCodificacionContableCompleta() As List(Of CFCodificacionContable.ConfiguracionContableXModulo)
        Get
            Return _ListaCodificacionContableCompleta
        End Get
        Set(ByVal value As List(Of CFCodificacionContable.ConfiguracionContableXModulo))
            _ListaCodificacionContableCompleta = value

            MyBase.CambioItem("ListaCodificacionContableCompleta")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para filtar los registros que se encuentran en la tabla tblConfiguracionContableXModulo para llenar el combo "Módulo"
    ''' </summary>
    Private _ListaModulo As List(Of CFCodificacionContable.ConfiguracionContableXModulo)
    Public Property ListaModulo() As List(Of CFCodificacionContable.ConfiguracionContableXModulo)
        Get
            Return _ListaModulo
        End Get
        Set(ByVal value As List(Of CFCodificacionContable.ConfiguracionContableXModulo))
            _ListaModulo = value
            MyBase.CambioItem("ListaModulo")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad de tipo Dictionary utilizada para manejar los registros que se encuentran en la tabla tblConfiguracionContableXModulo para distribuirlos en todos los combos del encabezado
    ''' </summary>
    Private _ListaInformacionModulo As Dictionary(Of String, List(Of CFCodificacionContable.ConfiguracionContableXModulo))
    Public Property ListaInformacionModulo() As Dictionary(Of String, List(Of CFCodificacionContable.ConfiguracionContableXModulo))
        Get
            Return _ListaInformacionModulo
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of CFCodificacionContable.ConfiguracionContableXModulo)))
            _ListaInformacionModulo = value
            MyBase.CambioItem("ListaInformacionModulo")
        End Set
    End Property

    Private _ListaInformacionModuloBusqueda As Dictionary(Of String, List(Of CFCodificacionContable.ConfiguracionContableXModulo))
    Public Property ListaInformacionModuloBusqueda() As Dictionary(Of String, List(Of CFCodificacionContable.ConfiguracionContableXModulo))
        Get
            Return _ListaInformacionModuloBusqueda
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of CFCodificacionContable.ConfiguracionContableXModulo)))
            _ListaInformacionModuloBusqueda = value
            MyBase.CambioItem("ListaInformacionModuloBusqueda")
        End Set
    End Property

    ''' <summary>
    ''' Lista de detalles de la entidad en este caso detalle de codificacion de activos
    ''' </summary>
    Private _ListaDetalle As List(Of CFCodificacionContable.CodificacionContableDetalle)
    Public Property ListaDetalle() As List(Of CFCodificacionContable.CodificacionContableDetalle)
        Get
            Return _ListaDetalle
        End Get
        Set(ByVal value As List(Of CFCodificacionContable.CodificacionContableDetalle))
            _ListaDetalle = value
            MyBase.CambioItem("ListaDetalle")
            MyBase.CambioItem("ListaDetallePaginada")
        End Set
    End Property

    ''' <summary>
    ''' Pagina la lista detalles. Se presenta en el grid del detalle 
    ''' </summary>
    Public ReadOnly Property ListaDetallePaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDetalle) Then
                Dim view = New PagedCollectionView(_ListaDetalle)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Indica cuál de los detalles está seleccionado
    ''' </summary>
    Private WithEvents _DetalleSeleccionado As CFCodificacionContable.CodificacionContableDetalle
    Public Property DetalleSeleccionado() As CFCodificacionContable.CodificacionContableDetalle
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As CFCodificacionContable.CodificacionContableDetalle)
            _DetalleSeleccionado = value
            MyBase.CambioItem("DetalleSeleccionado")
        End Set
    End Property


    ''' <summary>
    ''' Lista de encabezado que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As EntitySet(Of CFCodificacionContable.CodificacionContable)
    Public Property ListaEncabezado() As EntitySet(Of CFCodificacionContable.CodificacionContable)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of CFCodificacionContable.CodificacionContable))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property


    ''' <summary>
    ''' Colección que pagina la lista de CodificacionContableDetalle para navegar sobre el grid con paginación
    ''' </summary>
    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
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
    ''' Si esta propiedad contiene valor se podrá ingresar un nuevo detalle en el ingreso de datos del formulario
    ''' </summary>
    Private WithEvents _strModuloSeleccionado As CFCodificacionContable.ConfiguracionContableXModulo
    Public Property strModuloSeleccionado() As CFCodificacionContable.ConfiguracionContableXModulo
        Get
            Return _strModuloSeleccionado
        End Get
        Set(ByVal value As CFCodificacionContable.ConfiguracionContableXModulo)
            _strModuloSeleccionado = value
            RaiseEvent CambioItemLista(value, "ListaBusqueda")
            MyBase.CambioItem("strModuloSeleccionado")  'CodificacionContable
        End Set
    End Property


    ''' <summary>
    ''' Propiedad de tipo lista para asignar los registros que se encuentran en la tabla tblConfiguracionContableXValor
    ''' </summary>
    Private _ListaComboValor As List(Of CFCodificacionContable.ConfiguracionContableXValor)
    Public Property ListaComboValor() As List(Of CFCodificacionContable.ConfiguracionContableXValor)
        Get
            Return _ListaComboValor
        End Get
        Set(ByVal value As List(Of CFCodificacionContable.ConfiguracionContableXValor))
            _ListaComboValor = value
            MyBase.CambioItem("ListaComboValor")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad que contiene los registros seleccionados en el child window que se despliega en algunas opciones del combo valor en el detalle
    ''' </summary>
    Private _strCWChequeados As EntitySet(Of CFCodificacionContable.ConfiguracionContableXValor)
    Public Property strCWChequeados As EntitySet(Of CFCodificacionContable.ConfiguracionContableXValor)
        Get
            Return _strCWChequeados
        End Get
        Set(value As EntitySet(Of CFCodificacionContable.ConfiguracionContableXValor))
            _strCWChequeados = value
            MyBase.CambioItem("strCWChequeados")
        End Set
    End Property

    Private _strCentroCostoFijo As String
    Public Property strCentroCostoFijo As String
        Get
            Return _strCentroCostoFijo
        End Get
        Set(value As String)
            _strCentroCostoFijo = value
            MyBase.CambioItem("strCentroCostoFijo")
        End Set
    End Property

    ''' <summary>
    ''' Elemento de la lista de CodificacionContable que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoSeleccionado As CFCodificacionContable.CodificacionContable
    Public Property EncabezadoSeleccionado() As CFCodificacionContable.CodificacionContable
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As CFCodificacionContable.CodificacionContable)

            Dim logIncializarDet As Boolean = False
            _EncabezadoSeleccionado = value
            If _EncabezadoSeleccionado Is Nothing Then
                logIncializarDet = True
            Else
                If _EncabezadoSeleccionado.intID > 0 Then
                    ConsultarDetalles(_EncabezadoSeleccionado.intID, Detalles.cmCodificacion)
                Else
                    logIncializarDet = True
                End If
            End If
            If logIncializarDet Then
                ListaDetalle = Nothing
            End If
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    Private Sub _EncabezadoSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EncabezadoSeleccionado.PropertyChanged
        If e.PropertyName = Encabezado.intModulo.ToString Then
            If Not IsNothing(EncabezadoSeleccionado.intModulo) Then
                RecargarCombo(CombosCodificacionContable.MODULO, CInt(_EncabezadoSeleccionado.intModulo))
            End If

        ElseIf e.PropertyName = Encabezado.intDuracion.ToString And _EncabezadoSeleccionado.intModulo <> INT_MODULO_CALC_FINANCIEROS Then
            Dim objLista = New List(Of CFCodificacionContable.ConfiguracionContableXModulo)
            Dim logRecargarComboTipoFecha As Boolean = False

            If Not IsNothing(_EncabezadoSeleccionado.intDuracion) Then
                If Not IsNothing(ListaInformacionModulo) Then

                    If ListaInformacionModulo.ContainsKey(STR_DURACION_DIAS) Then
                        For Each li In ListaInformacionModulo(STR_DURACION_DIAS)
                            If li.IntId = _EncabezadoSeleccionado.intDuracion And li.strRetorno = STR_RETORNO_DURACION_DIAS Then
                                If ListaInformacionModulo.ContainsKey(STR_TIPO_FECHA) Then
                                    For Each lis In ListaInformacionModulo(STR_TIPO_FECHA)
                                        If lis.strRetorno = "de Liquidacion" Then
                                            objLista.Add(lis)
                                            GoTo Linea_1
                                        End If
                                    Next
                                End If
                            Else
                                For Each liDato In ListaCodificacionContableCompleta.Where(Function(i) i.strTopico = STR_TIPO_FECHA)
                                    If liDato.intPadre = EncabezadoSeleccionado.intModulo Then
                                        objLista.Add(liDato)
                                    End If
                                Next
                                Dim objDiccionarioNuevo As New Dictionary(Of String, List(Of CFCodificacionContable.ConfiguracionContableXModulo))
                                objDiccionarioNuevo(STR_TIPO_FECHA) = objLista
                                GoTo Linea_1
                            End If
                        Next

Linea_1:
                        If ListaInformacionModulo.ContainsKey(STR_TIPO_FECHA) Then
                            ListaInformacionModulo.Remove(STR_TIPO_FECHA)
                        End If

                        ListaInformacionModulo.Add(STR_TIPO_FECHA, objLista)
                        MyBase.CambioItem("ListaInformacionModulo")
                    End If

                End If
            End If

        ElseIf e.PropertyName = Encabezado.intNegocio.ToString Then
            Dim objLista = New List(Of CFCodificacionContable.ConfiguracionContableXModulo)
            Dim logRecargarComboTipoFecha As Boolean = False


            If Not IsNothing(_EncabezadoSeleccionado.intNegocio) Then
                If Not IsNothing(ListaInformacionModulo) Then

                    If ListaInformacionModulo.ContainsKey(STR_NEGOCIO) Then
                        For Each li In ListaInformacionModulo(STR_NEGOCIO)
                            If li.IntId = _EncabezadoSeleccionado.intNegocio And li.strRetorno.Contains(STR_RETORNO_NEGOCIO) Then
                                If ListaInformacionModulo.ContainsKey(STR_TIPO_FECHA) Then
                                    For Each lis In ListaInformacionModulo(STR_TIPO_FECHA)
                                        If lis.strRetorno = "Valoración" Then
                                            objLista.Add(lis)
                                            logRecargarComboTipoFecha = True
                                            GoTo Linea_2
                                        End If
                                    Next
                                End If
                            End If
                        Next

                        If Not logRecargarComboTipoFecha Then
                            For Each liDato In ListaCodificacionContableCompleta.Where(Function(i) i.strTopico = STR_TIPO_FECHA)
                                If liDato.intPadre = EncabezadoSeleccionado.intModulo Then
                                    objLista.Add(liDato)
                                End If
                            Next
                            Dim objDiccionarioNuevo As New Dictionary(Of String, List(Of CFCodificacionContable.ConfiguracionContableXModulo))
                            objDiccionarioNuevo(STR_TIPO_FECHA) = objLista
                        End If

Linea_2:
                        If ListaInformacionModulo.ContainsKey(STR_TIPO_FECHA) Then
                            ListaInformacionModulo.Remove(STR_TIPO_FECHA)
                        End If

                        ListaInformacionModulo.Add(STR_TIPO_FECHA, objLista)
                        MyBase.CambioItem("ListaInformacionModulo")
                    End If

                End If
            End If
        End If


    End Sub

    ''' <summary>
    ''' Propiedad para manejar el IsEnabled en el View
    ''' </summary>
    Private _HabilitarEncabezado As Boolean = False
    Public Property HabilitarEncabezado() As Boolean
        Get
            Return _HabilitarEncabezado
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEncabezado = value
            MyBase.CambioItem("HabilitarEncabezado")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad para manejar el IsEnabled en el View
    ''' </summary>
    Private _HabilitarEncabezadoNormaContable As Boolean = False
    Public Property HabilitarEncabezadoNormaContable() As Boolean
        Get
            Return _HabilitarEncabezadoNormaContable
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEncabezadoNormaContable = value
            MyBase.CambioItem("HabilitarEncabezadoNormaContable")
        End Set
    End Property

    ''' <summary>
    ''' Permite indicar el estado actual de la entidad: Verdadero si está en modo de edición o False de lo contrario
    ''' </summary>
    Private _EditandoDetalle As Boolean = True
    Public Property EditandoDetalle() As Boolean
        Get
            Return _EditandoDetalle
        End Get
        Set(ByVal value As Boolean)
            _EditandoDetalle = value
            MyBase.CambioItem("EditandoDetalle")
        End Set
    End Property

    Private _VisibilidadBtnValorTotalizado As Visibility = Visibility.Visible
    Public Property VisibilidadBtnValorTotalizado() As Visibility
        Get
            Return _VisibilidadBtnValorTotalizado
        End Get
        Set(ByVal value As Visibility)
            _VisibilidadBtnValorTotalizado = value
            MyBase.CambioItem("VisibilidadBtnValorTotalizado")
        End Set
    End Property

    Private _VisibilidadActividad As Visibility = Visibility.Visible
    Public Property VisibilidadActividad() As Visibility
        Get
            Return _VisibilidadActividad
        End Get
        Set(ByVal value As Visibility)
            _VisibilidadActividad = value
            MyBase.CambioItem("VisibilidadActividad")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    ''' <history>
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 1/2014
    ''' Id caso de prueba: Id_2
    ''' Pruebas CB       : Jorge Peña - Abril 1/2014 - Resultado Ok    
    ''' </history>
    Public Overrides Sub NuevoRegistro()

        Dim objNvoEncabezado As New CFCodificacionContable.CodificacionContable

        Try
            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not mobjEncabezadoPorDefecto Is Nothing Then
                Program.CopiarObjeto(Of CFCodificacionContable.CodificacionContable)(mobjEncabezadoPorDefecto, objNvoEncabezado)
            Else
                objNvoEncabezado.intID = -1
                objNvoEncabezado.strDescripcionNormaContable = String.Empty
                objNvoEncabezado.strNormaContable = String.Empty
                objNvoEncabezado.strModulo = String.Empty
                objNvoEncabezado.intModulo = 0
                objNvoEncabezado.strOperacion = String.Empty
                objNvoEncabezado.intOperacion = 0
                objNvoEncabezado.strDuracion = String.Empty
                objNvoEncabezado.intDuracion = 0
                objNvoEncabezado.strTipoFecha = String.Empty
                objNvoEncabezado.intTipoFecha = 0
                objNvoEncabezado.intTipoProducto = 0
                objNvoEncabezado.strTipoInversion = String.Empty
                objNvoEncabezado.strIdMoneda = String.Empty
                objNvoEncabezado.lngIdMoneda = 0
                objNvoEncabezado.logActivo = -1
                objNvoEncabezado.srtNegocio = String.Empty
                objNvoEncabezado.dtmFechaInicio = Date.Now()
                objNvoEncabezado.intDuracion = 0
            End If

            objNvoEncabezado.strUsuario = Program.Usuario

            mobjEncabezadoAnterior = ObtenerRegistroAnterior()

            VisibilidadActividad = Visibility.Collapsed
            Editando = True
            MyBase.CambioItem("Editando")

            EncabezadoSeleccionado = objNvoEncabezado

            RecargarCombo(CombosCodificacionContable.TIPOSMODULOS)

            HabilitarEncabezado = True
            HabilitarEncabezadoNormaContable = True

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción Buscar de la barra de herramientas.
    ''' Ejecuta una búsqueda sobre los datos que contengan en los campos definidos internamente en el procedimiento de búsqueda (filtrado) el texto 
    ''' ingresado en el campo Filtro de la barra de herramientas
    ''' </summary>
    ''' <history>
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 1/2014
    ''' Id caso de prueba: Id_3
    ''' Pruebas CB       : Jorge Peña - Abril 1/2014 - Resultado Ok  
    ''' </history>
    Public Overrides Async Sub Filtrar()
        Try
            Await ConsultarEncabezado(True, FiltroVM)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicializar la ejecución del filtro", Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción "Búsqueda avanzada" de la barra de herramientas.
    ''' Presenta la forma de búsqueda para que el usuario seleccione los valores por los cuales desea buscar dentro de los campos definidos en la forma de búsqueda
    ''' </summary>
    ''' <history>
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 1/2014
    ''' Id caso de prueba: Id_4
    ''' Pruebas CB       : Jorge Peña - Abril 1/2014 - Resultado Ok 
    ''' </history>
    Public Overrides Sub Buscar()
        Try
            PrepararNuevaBusqueda()

            'Se declara una variable de tipo ConfiguracionContableXModulo para asigarsela a la variable strModuloSeleccionadoy limpiar el 
            'valor porque estaba seleccionado el primer registro en el combo "Módulo" en el momento de realizar la búsqueda avanzada.
            Dim objConfiguracionContableXModulo As New CFCodificacionContable.ConfiguracionContableXModulo
            strModuloSeleccionado = objConfiguracionContableXModulo
            RecargarCombo(CombosCodificacionContable.TIPOSMODULOS)
            MyBase.Buscar()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el método buscar", Me.ToString(), "Buscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Buscar de la forma de búsqueda
    ''' Ejecuta una búsqueda por los campos contenidos en la forma de búsqueda y cuyos valores correspondan con los seleccionados por el usuario
    ''' </summary>
    ''' <history>
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 1/2014
    ''' Id caso de prueba: Id_4
    ''' Pruebas CB       : Jorge Peña - Abril 1/2014 - Resultado Ok 
    ''' </history>
    ''' <history>
    ''' Creado por       : Javier Pardo (Alcuadrado S.A.)
    ''' Descripción      : Se elimina código correspondiente al contenido del método PrepararNuevaBusqueda() ya que evita filtrar correctamente la primera vez.
    ''' Fecha            : Enero 06/2016
    ''' Pruebas CB       : Javier Pardo - Enero 06/2016 - Resultado Ok 
    ''' ID del cambio    : JEPM20160106
    ''' </history> 
    Public Overrides Async Sub ConfirmarBuscar()
        Try

            If IsNothing(cb.intModulo) Then cb.intModulo = 0
            If IsNothing(cb.intNegocio) Then cb.intNegocio = 0
            If IsNothing(cb.intOperacion) Then cb.intOperacion = 0
            If IsNothing(cb.intDuracion) Then cb.intDuracion = 0
            If IsNothing(cb.intTipoFecha) Then cb.intTipoFecha = 0
            If IsNothing(cb.intTipoProducto) Then cb.intTipoProducto = 0

            If Not IsNothing(cb.intModulo) Or Not IsNothing(cb.intNegocio) Or Not IsNothing(cb.intOperacion) Or
               Not IsNothing(cb.intDuracion) Or Not IsNothing(cb.intTipoFecha) Or Not IsNothing(cb.intTipoProducto) Or Not IsNothing(cb.strTipoInversion) Then
                'Validar que ingresó algo en los campos de búsqueda
                Await ConsultarEncabezado(False, String.Empty, -1, CInt(cb.intModulo), CInt(cb.intNegocio), CInt(cb.intOperacion), CInt(cb.intDuracion), CInt(cb.intTipoFecha), CInt(cb.intTipoProducto), cb.strTipoInversion)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón guardar de la barra de herramientas. 
    ''' Ejecuta el proceso que ingresa o actualiza la base de datos con los cambios realizados 
    ''' </summary>
    ''' <history>
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 1/2014
    ''' Id caso de prueba: Id_6
    ''' Pruebas CB       : Jorge Peña - Abril 1/2014 - Resultado Ok 
    ''' 
    ''' Modificado por   : Germán González (Alcuadrado S.A.)
    ''' Descripción      : Se modifica para controlar mensajes de validación en el sp de forma sincrona.
    ''' Fecha            : Abril 8/2014
    ''' Pruebas CB       : Germán González - Abril 8/2014 - Pendientes
    ''' </history>
    Public Overrides Async Sub ActualizarRegistro()

        Dim strAccion As String = ValoresUserState.Actualizar.ToString()

        Try
            Dim xmlCompleto As String
            Dim xmlDetalle As String
            ErrorForma = String.Empty
            IsBusy = True

            If ValidarRegistro() Then

                xmlCompleto = "<CodificacionContable>"

                For Each objeto In (From c In ListaDetalle)

                    xmlDetalle = "<Detalle intContableConcepto=""" & objeto.intContableConcepto & """ strValor=""" & objeto.strValor & """ strNIT=""" & objeto.strNIT &
                        """ strCtaContable=""" & objeto.strCtaContable & """ strCentroCostos=""" & objeto.strCentroCostos & """ strCentroCostosFijo=""" & objeto.strCentroCostosFijo &
                        """ strComprobante=""" & objeto.strComprobante & """ strNaturaleza=""" & objeto.strNaturaleza & """ strDetalle=""" & objeto.strDetalle &
                        """ strNroDocumento=""" & String.Empty & """ strDocReferencia=""" & objeto.strDocReferencia & """ strNitExterno=""" & objeto.strNitExterno &
                        """ strCamposTotalizados=""" & objeto.strCamposTotalizados & """></Detalle>"


                    xmlCompleto = xmlCompleto & xmlDetalle
                Next

                xmlCompleto = xmlCompleto & "</CodificacionContable>"

                'IsBusy = True

                Dim strMsg As String = ""
                Dim objRet As InvokeOperation(Of String)

                objRet = Await mdcProxy.ActualizarCodificacionContableSync(EncabezadoSeleccionado.intID, EncabezadoSeleccionado.strNormaContable, CInt(EncabezadoSeleccionado.intModulo), _
                                                        CInt(EncabezadoSeleccionado.intNegocio), CInt(EncabezadoSeleccionado.intOperacion), CInt(EncabezadoSeleccionado.intDuracion), _
                                                        CInt(EncabezadoSeleccionado.intTipoFecha), CInt(EncabezadoSeleccionado.intTipoProducto), EncabezadoSeleccionado.strTipoInversion, _
                                                        CInt(EncabezadoSeleccionado.lngIdMoneda), CDate(EncabezadoSeleccionado.dtmFechaInicio), CInt(EncabezadoSeleccionado.logActivo), _
                                                        xmlCompleto, Program.Usuario, Program.HashConexion).AsTask()

                If Not String.IsNullOrEmpty(objRet.Value.ToString()) Then
                    strMsg = String.Format("{0}{1} + {2}", strMsg, vbCrLf, objRet.Value.ToString())

                    If Not String.IsNullOrEmpty(strMsg) Then
                        A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias: " & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                Else
                    Editando = False
                    EditandoDetalle = False
                    HabilitarEncabezado = False
                    HabilitarEncabezadoNormaContable = False
                    VisibilidadBtnValorTotalizado = Visibility.Visible
                    ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                    RecargarCombo(CombosCodificacionContable.TODOS)
                    Await ConsultarEncabezado(True, String.Empty)
                End If

            Else
                HabilitarEncabezado = True
            End If

            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicar el proceso de actualización.", Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Editar de la barra de herramientas.
    ''' Activa la edición del encabezado y del detalle (si aplica) del encabezado activo
    ''' </summary>
    ''' <history>
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 1/2014
    ''' Id caso de prueba: Id_6
    ''' Pruebas CB       : Jorge Peña - Abril 1/2014 - Resultado Ok 
    ''' </history>
    Public Overrides Sub EditarRegistro()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then

                If mdcProxy.IsLoading Then
                    MyBase.RetornarValorEdicionNavegacion()
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                IsBusy = True

                _EncabezadoSeleccionado.strUsuario = Program.Usuario

                mobjEncabezadoAnterior = ObtenerRegistroAnterior()
                Dim objSeleccionado = ObtenerRegistroAnterior()

                EncabezadoSeleccionado = Nothing
                RecargarCombo(CombosCodificacionContable.MODULO, CInt(objSeleccionado.intModulo))
                EncabezadoSeleccionado = objSeleccionado

                Editando = True
                MyBase.CambioItem("Editando")

                HabilitarEncabezado = True
                HabilitarEncabezadoNormaContable = False
                VisibilidadBtnValorTotalizado = Visibility.Collapsed
                VisibilidadActividad = Visibility.Visible

                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicar la edición del registro seleccionado.", Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Cancelar de la barra de herramientas durante el ingreso o modificación del encabezado activo
    ''' </summary>
    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = String.Empty

            If Not _EncabezadoSeleccionado Is Nothing Then
                mdcProxy.RejectChanges()

                Editando = False
                MyBase.CambioItem("Editando")
                RecargarCombo(CombosCodificacionContable.TODOS)
                EncabezadoSeleccionado = mobjEncabezadoAnterior
                HabilitarEncabezadoNormaContable = False
                HabilitarEncabezado = False
                VisibilidadBtnValorTotalizado = Visibility.Visible
                VisibilidadActividad = Visibility.Visible
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        RecargarCombo(CombosCodificacionContable.TODOS)
        MyBase.CancelarBuscar()
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Borrar de la barra de herramientas e incia el proceso de eliminación del encabezado activo
    ''' </summary>
    ''' <history>
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 1/2014
    ''' Id caso de prueba: Id_7
    ''' Pruebas CB       : Jorge Peña - Abril 1/2014 - Resultado Ok 
    ''' </history>
    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción borra la codificación contable seleccionada. ¿Confirma el borrado de este codificación contable?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejcutá cuando el usuario confirma la eliminación del encabezado activo.
    ''' Ejecuta el proceso de eliminación en la base de datos
    ''' </summary>
    ''' <history>
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 1/2014
    ''' Id caso de prueba: Id_7
    ''' Pruebas CB       : Jorge Peña - Abril 1/2014 - Resultado Ok 
    ''' </history>
    Private Sub BorrarRegistroConfirmado(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                If Not IsNothing(_EncabezadoSeleccionado) Then
                    IsBusy = True

                    mobjEncabezadoAnterior = ObtenerRegistroAnterior()

                    If mdcProxy.CodificacionContables.Where(Function(i) i.intID = EncabezadoSeleccionado.intID).Count > 0 Then
                        mdcProxy.CodificacionContables.Remove(mdcProxy.CodificacionContables.Where(Function(i) i.intID = EncabezadoSeleccionado.intID).First)
                    End If

                    If _ListaEncabezado.Count > 0 Then
                        EncabezadoSeleccionado = _ListaEncabezado.LastOrDefault
                    Else
                        EncabezadoSeleccionado = Nothing
                    End If
                    Program.VerificarCambiosProxyServidor(mdcProxy)
                    mdcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, ValoresUserState.Borrar.ToString)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", Me.ToString(), "BorrarRegistroConfirmado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos públicos del detalle - REQUERIDO (si hay detalle)"

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Nuevo del detalle 
    ''' Solamente se ingresa un nuevo elemento en la lista del detalle para que el usuario ingrese el nuevo detalle
    ''' </summary>
    ''' <history>
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 1/2014
    ''' Id caso de prueba: Id_8
    ''' Pruebas CB       : Jorge Peña - Abril 1/2014 - Resultado Ok 
    ''' </history>
    Public Overrides Async Sub NuevoRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle.ToLower()
                Case Detalles.cmCodificacion.ToString.ToLower
                    If Not IsNothing(_strModuloSeleccionado) Then
                        Dim objNvoDetalle As New CFCodificacionContable.CodificacionContableDetalle
                        Dim objNuevaLista As New List(Of CFCodificacionContable.CodificacionContableDetalle)

                        Program.CopiarObjeto(Of CFCodificacionContable.CodificacionContableDetalle)(mobjDetallePorDefecto, objNvoDetalle)

                        objNvoDetalle.intID = -New Random().Next(0, 1000000)

                        If IsNothing(ListaDetalle) Then
                            If Not Await LlenarComboValor() Then
                                Exit Sub
                            End If
                            ListaDetalle = New List(Of CFCodificacionContable.CodificacionContableDetalle)
                        End If

                        objNuevaLista = ListaDetalle
                        objNuevaLista.Add(objNvoDetalle)
                        ListaDetalle = objNuevaLista
                        DetalleSeleccionado = _ListaDetalle.First

                        HabilitarEncabezado = True
                        VisibilidadBtnValorTotalizado = Visibility.Collapsed

                        MyBase.CambioItem("ListaDetalle")
                        MyBase.CambioItem("ListaDetallesPaginada")
                        MyBase.CambioItem("DetalleSeleccionado")
                    Else
                        If (EncabezadoSeleccionado.intModulo) = 0 Then
                            A2Utilidades.Mensajes.mostrarMensaje("El módulo es un campo requerido para poder agregar un detalle. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
            End Select

            EditandoDetalle = True

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo detalle", Me.ToString(), "NuevoRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Borrar del detalle 
    ''' Solamente se elimina el registro de la lista del detalle pero no se afecta base de datos. Esto se hace al guardar el encabezado.
    ''' </summary>
    ''' <history>
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 1/2014
    ''' Id caso de prueba: Id_9
    ''' Pruebas CB       : Jorge Peña - Abril 1/2014 - Resultado Ok 
    ''' </history>
    Public Overrides Sub BorrarRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle.ToLower()
                Case Detalles.cmCodificacion.ToString.ToLower
                    If Not IsNothing(DetalleSeleccionado) Then
                        Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(DetalleSeleccionado, ListaDetalle)

                        If _ListaDetalle.Where(Function(i) i.intID = _DetalleSeleccionado.intID).Count > 0 Then
                            _ListaDetalle.Remove(_ListaDetalle.Where(Function(i) i.intID = _DetalleSeleccionado.intID).First)
                        End If

                        Dim objNuevaListaDetalle As New List(Of CFCodificacionContable.CodificacionContableDetalle)

                        For Each li In _ListaDetalle
                            objNuevaListaDetalle.Add(li)
                        Next

                        If objNuevaListaDetalle.Where(Function(i) i.intID = _DetalleSeleccionado.intID).Count > 0 Then
                            objNuevaListaDetalle.Remove(objNuevaListaDetalle.Where(Function(i) i.intID = _DetalleSeleccionado.intID).First)
                        End If

                        ListaDetalle = objNuevaListaDetalle

                        If Not IsNothing(_ListaDetalle) Then
                            If _ListaDetalle.Count > 0 Then
                                Program.PosicionarItemLista(DetalleSeleccionado, ListaDetalle, intRegistroPosicionar)
                            End If
                        End If
                    End If
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para borrar un detalle", Me.ToString(), "BorrarRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para capturar el objeto desde el evento ComboBox_DropDownClosedValor en el code behind del combo valor que existe en el detalle.
    ''' </summary>
    Public Sub CerroDropDownValor(sender As Object)
        Try
            Dim objComboBox As ComboBox = CType(sender, ComboBox)
            If Not IsNothing(objComboBox.SelectedItem) Then
                Dim objNuevo As CFCodificacionContable.ConfiguracionContableXValor = CType(objComboBox.SelectedItem, CFCodificacionContable.ConfiguracionContableXValor)
                If Not IsNothing(objNuevo.logTotalizado) And objNuevo.logTotalizado And Not IsNothing(objNuevo) Then
                    cwCodificacionContableValor = New cwCodificacionContableValor(DetalleSeleccionado.strCamposTotalizados, True) 'strCWChequeados)
                    MyBase.CambioItem("ListaComboValor")
                    AddHandler cwCodificacionContableValor.Closed, AddressOf CerroVentanaCodificacionContableValor
                    Program.Modal_OwnerMainWindowsPrincipal(cwCodificacionContableValor)
                    cwCodificacionContableValor.ShowDialog()
                Else
                    DetalleSeleccionado.strCamposTotalizados = String.Empty
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema cerrando el control de valor en el detalle", Me.ToString(), "CerroDropDownValor", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para capturar el objeto desde el evento ComboBox_DropDownClosedCentroCostos en el code behind del combo centro de costos que existe en el detalle.
    ''' </summary>
    Public Sub CerroDropDownCentroCostos(sender As Object)
        Try
            Dim objComboBox As ComboBox = CType(sender, ComboBox)
            If Not IsNothing(objComboBox.SelectedItem) Then
                Dim objNuevo As OYDUtilidades.ItemCombo = CType(objComboBox.SelectedItem, OYDUtilidades.ItemCombo)
                If Not IsNothing(objNuevo.ID) And (objNuevo.ID = STR_CENTRO_COSTOS_FIJO) And Not IsNothing(objNuevo) Then
                    cwCodificacionContableCCostos = New cwCodificacionContableCCostos(DetalleSeleccionado.strCentroCostosFijo)
                    AddHandler cwCodificacionContableCCostos.Closed, AddressOf CerroVentanaCodificacionContableCCostos
                    cwCodificacionContableCCostos.ShowDialog()
                Else
                    DetalleSeleccionado.strCentroCostosFijo = String.Empty
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema cerrando el control de valor en el detalle", Me.ToString(), "CerroDropDownValor", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub VerTotalizadosSeleccionados()
        Try
            If Not IsNothing(EncabezadoSeleccionado) And Not IsNothing(DetalleSeleccionado) Then
                If Not String.IsNullOrEmpty(DetalleSeleccionado.strCamposTotalizados) Then
                    cwCodificacionContableValor = New cwCodificacionContableValor(DetalleSeleccionado.strCamposTotalizados, False)
                    MyBase.CambioItem("ListaComboValor")
                    AddHandler cwCodificacionContableValor.Closed, AddressOf CerroVentanaCodificacionContableValor
                    Program.Modal_OwnerMainWindowsPrincipal(cwCodificacionContableValor)
                    cwCodificacionContableValor.ShowDialog()
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema cerrando el control de valor en el detalle", Me.ToString(), "CerroDropDownValor", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos privados del detalle - REQUERIDO (si hay detalle)"

    ''' <summary>
    ''' Método para capturar los registros seleccionados o chequeados desde la venta emergente que se activa al 
    ''' presionar la opción "Totalizado" desde el combo valor en el detalle
    ''' </summary>
    Private Sub CerroVentanaCodificacionContableValor(sender As Object, e As EventArgs)
        Try
            If cwCodificacionContableValor.DialogResult.Value Then
                If Not IsNothing(cwCodificacionContableValor.strCWChequeados) Then
                    DetalleSeleccionado.strCamposTotalizados = String.Empty
                    For Each li In cwCodificacionContableValor.ListaTotalizados
                        If li.logTotalizado Then
                            If String.IsNullOrEmpty(_DetalleSeleccionado.strCamposTotalizados) Then
                                DetalleSeleccionado.strCamposTotalizados = li.strRetorno
                            Else
                                DetalleSeleccionado.strCamposTotalizados = DetalleSeleccionado.strCamposTotalizados & "," & li.strRetorno
                            End If
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cerrar la ventana para totalizar el valor", _
                     Me.ToString(), "CerroVentanaCodificacionContableValor", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para capturar el valor ingresado desde la venta emergente que se activa al 
    ''' presionar la opción "Fijo" desde el combo centro de costos en el detalle
    ''' </summary>
    Private Sub CerroVentanaCodificacionContableCCostos(sender As Object, e As EventArgs)
        Try
            If cwCodificacionContableCCostos.DialogResult.Value Then
                If Not IsNothing(cwCodificacionContableCCostos.strCentroCostoFijo) Then
                    DetalleSeleccionado.strCentroCostosFijo = cwCodificacionContableCCostos.strCentroCostoFijo
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cerrar la ventana para totalizar el valor", _
                     Me.ToString(), "CerroVentanaCodificacionContableCCostos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Procedimiento que se ejecuta cuando se va guardar un nuevo encabezado o actualizar el activo. 
    ''' Se debe llamar desde el procedimiento ValidarRegistro
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 1/2014
    ''' Id caso de prueba: Id_5
    ''' Pruebas CB       : Jorge Peña - Abril 1/2014 - Resultado Ok 
    ''' </summary>
    ''' <param name="pintTipoDetalle">Indica el detalle que se debe validar</param>
    Private Function ValidarDetalle(ByVal pintTipoDetalle As Integer) As Boolean
        Dim logResultado As Boolean = True
        Dim strMsg As String = String.Empty

        Try
            Select Case pintTipoDetalle
                Case Detalles.cmCodificacion
                    '------------------------------------------------------------------------------------------------------------------------------------------------
                    '-- Valida que por lo menos exista un detalle para poder crear todo un registro
                    '------------------------------------------------------------------------------------------------------------------------------------------------
                    If IsNothing(_ListaDetalle) Then
                        strMsg = String.Format("{0}{1} + Debe ingresar por lo menos un detalle.", strMsg, vbCrLf)
                    ElseIf _ListaDetalle.Count = 0 Then
                        strMsg = String.Format("{0}{1} + Debe ingresar por lo menos un detalle.", strMsg, vbCrLf)
                    Else
                        Dim strTipoCuenta As String = String.Empty
                        Dim logSalirCiclo As Boolean = False

                        For Each objDet As CFCodificacionContable.CodificacionContableDetalle In ListaDetalle

                            'Valida el valor en el detalle
                            If String.IsNullOrEmpty(objDet.strValor) Then
                                strMsg = String.Format("{0}{1} + Hay detalles que no tienen definido el valor.", strMsg, vbCrLf)
                                logSalirCiclo = True
                            End If

                            'Valida el nit en el detalle
                            If String.IsNullOrEmpty(objDet.strNIT) Then
                                strMsg = String.Format("{0}{1} + Hay detalles que no tienen definido el nit.", strMsg, vbCrLf)
                                logSalirCiclo = True
                            End If

                            'Valida el concepto de la cta. contable en el detalle
                            If String.IsNullOrEmpty(objDet.strCtaContable) Then
                                strMsg = String.Format("{0}{1} + Hay detalles que no tienen definido el concepto de la cta. contable.", strMsg, vbCrLf)
                                logSalirCiclo = True
                            End If

                            'Valida el centro de costos en el detalle
                            If String.IsNullOrEmpty(objDet.strCentroCostos) Then
                                strMsg = String.Format("{0}{1} + Hay detalles que no tienen definido el centro de costos.", strMsg, vbCrLf)
                                logSalirCiclo = True
                            End If

                            'Valida la naturaleza en el detalle
                            If String.IsNullOrEmpty(objDet.strNaturaleza) Then
                                strMsg = String.Format("{0}{1} + Hay detalles que no tienen definido la naturaleza.", strMsg, vbCrLf)
                                logSalirCiclo = True
                            End If

                            'Valida el nit externo en el detalle
                            If String.IsNullOrEmpty(objDet.strNitExterno) Then
                                strMsg = String.Format("{0}{1} + Hay detalles que no tienen definido el nit externo.", strMsg, vbCrLf)
                                logSalirCiclo = True
                            End If

                            If logSalirCiclo Then
                                Exit For
                            End If
                        Next
                    End If

                    If Not strMsg.Equals(String.Empty) Then
                        logResultado = False
                        A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias en el detalle: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
            End Select
        Catch ex As Exception
            logResultado = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados en el detalle.", Me.ToString(), "validarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return (logResultado)
    End Function

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"

    ''' <summary>
    ''' Inicializa el objeto en el cual se capturan los filtros seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaCodificacionContable(Me)
            objCB.intModulo = 0
            objCB.intOperacion = 0
            objCB.intTipoFecha = 0
            objCB.strTipoInversion = String.Empty
            objCB.intNegocio = 0
            objCB.intDuracion = 0
            objCB.intTipoProducto = 0
            cb = objCB
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar los datos de la forma de búsqueda", Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Retorna una copia del encabezado activo. 
    ''' Se hace un "clon" del encabezado activo para poder devolver los cambios y dejar el encabezado activo en su estado original si es necesario.
    ''' </summary>
    Private Function ObtenerRegistroAnterior() As CFCodificacionContable.CodificacionContable
        Dim objEncabezado As CFCodificacionContable.CodificacionContable = New CFCodificacionContable.CodificacionContable

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                Program.CopiarObjeto(Of CFCodificacionContable.CodificacionContable)(_EncabezadoSeleccionado, objEncabezado)
                objEncabezado.intID = _EncabezadoSeleccionado.intID
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para guardar los datos del registro activo antes de su modificación.", Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (objEncabezado)
    End Function

    ''' <summary>
    ''' Función para validar todos los campos obligatorios de la pantalla, si la pantalla tiene detalle 
    ''' esta función realizará el llamado a otra función para validar el detalle.
    ''' </summary>
    ''' <history>
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 1/2014
    ''' Id caso de prueba: Id_5
    ''' Pruebas CB       : Jorge Peña - Abril 1/2014 - Resultado Ok 
    ''' </history>
    ''' 
    ''' Se agrego la validacion del isNothig para los campos que son combos en el encabezado -- SM20151119

    Private Function ValidarRegistro() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(_EncabezadoSeleccionado) Then
                'Valida la norma contable
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strNormaContable) Or IsNothing(_EncabezadoSeleccionado.strNormaContable) Then
                    strMsg = String.Format("{0}{1} + La norma contable es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el módulo
                If (_EncabezadoSeleccionado.intModulo) = 0 Or IsNothing(_EncabezadoSeleccionado.intModulo) Then
                    strMsg = String.Format("{0}{1} + El módulo es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el negocio
                If (_EncabezadoSeleccionado.intNegocio) = 0 Or IsNothing(_EncabezadoSeleccionado.intNegocio) Then
                    strMsg = String.Format("{0}{1} + El negocio es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida la operación
                If (_EncabezadoSeleccionado.intOperacion) = 0 Or IsNothing(_EncabezadoSeleccionado.intOperacion) Then
                    strMsg = String.Format("{0}{1} + La operación es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida la duración
                If (_EncabezadoSeleccionado.intDuracion) = 0 Or IsNothing(_EncabezadoSeleccionado.intDuracion) Then
                    strMsg = String.Format("{0}{1} + La duración es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el tipo de fecha
                If (_EncabezadoSeleccionado.intTipoFecha) = 0 Or IsNothing(_EncabezadoSeleccionado.intTipoFecha) Then
                    strMsg = String.Format("{0}{1} + El tipo de fecha es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el tipo de producto
                If (_EncabezadoSeleccionado.intTipoProducto) = 0 Or IsNothing(_EncabezadoSeleccionado.intTipoProducto) Then
                    strMsg = String.Format("{0}{1} + El tipo de producto es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el tipo de inversión
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strTipoInversion) Or IsNothing(_EncabezadoSeleccionado.strTipoInversion) Then
                    strMsg = String.Format("{0}{1} + El tipo de inversión es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida la moneda
                If (_EncabezadoSeleccionado.lngIdMoneda) = 0 Or IsNothing(_EncabezadoSeleccionado.lngIdMoneda) Then
                    strMsg = String.Format("{0}{1} + La moneda es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida la fecha de inicio
                If IsNothing(_EncabezadoSeleccionado.dtmFechaInicio) Or IsNothing(_EncabezadoSeleccionado.dtmFechaInicio) Then
                    strMsg = String.Format("{0}{1} + La fecha de inicio es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el estado
                If VisibilidadActividad = Visibility.Visible Then
                    If (_EncabezadoSeleccionado.logActivo) = -1 Then
                        strMsg = String.Format("{0}{1} + El estado es un campo requerido.", strMsg, vbCrLf)
                    End If
                End If
            Else
                strMsg = String.Format("{0}{1} Debe de seleccionar un registro", strMsg, vbCrLf)
            End If

            If strMsg.Equals(String.Empty) Then
                '------------------------------------------------------------------------------------------------------------------------
                '-- VALIDAR DATOS DEL DETALLE
                '-------------------------------------------------------------------------------------------------------------------------
                logResultado = ValidarDetalle(Detalles.cmCodificacion)
            Else
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias antes de guardar: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "ValidarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Procedimiento para cargar y recargar los combos del encabezado.
    ''' </summary>
    ''' <history>
    ''' Creado por       : Javier Pardo (Alcuadrado S.A.)
    ''' Descripción      : Se cambia propiedad privada por pública ListaInformacionModuloBusqueda.
    '''                    Se asignan los mismos valores a ListaInformacionModuloBusqueda y ListaInformacionModulo
    ''' Fecha            : Enero 06/2016
    ''' Pruebas CB       : Javier Pardo - Enero 06/2016 - Resultado Ok 
    ''' ID del cambio    : JEPM20160106
    ''' </history> 
    Public Sub RecargarCombo(ByVal pobjOpcion As CombosCodificacionContable, Optional ByVal pintIDModulo As Integer = Nothing)
        Try
            If Not IsNothing(pobjOpcion) And Not IsNothing(_ListaCodificacionContableCompleta) Then
                Dim objLista As New List(Of CFCodificacionContable.ConfiguracionContableXModulo)

                If pobjOpcion = CombosCodificacionContable.TIPOSMODULOS Then
                    For Each li In ListaCodificacionContableCompleta
                        If li.strTopico = STR_MODULO Then
                            objLista.Add(li)
                        End If
                    Next

                    ListaModulo = objLista
                    ListaInformacionModulo = Nothing
                    ListaInformacionModuloBusqueda = Nothing
                ElseIf pobjOpcion = CombosCodificacionContable.TODOS Then
                    Dim objDiccionarioNuevo As New Dictionary(Of String, List(Of CFCodificacionContable.ConfiguracionContableXModulo))
                    Dim strTopico As String = String.Empty

                    For Each li In ListaCodificacionContableCompleta.OrderBy(Function(i) i.strTopico)
                        If li.strTopico <> strTopico And li.strTopico <> STR_MODULO Then
                            strTopico = li.strTopico
                            objLista = New List(Of CFCodificacionContable.ConfiguracionContableXModulo)

                            For Each liDato In ListaCodificacionContableCompleta.Where(Function(i) i.strTopico = strTopico)
                                If objLista.Where(Function(i) i.IntId = liDato.IntId).Count = 0 Then
                                    objLista.Add(liDato)
                                End If
                            Next

                            objDiccionarioNuevo.Add(strTopico, objLista)
                        End If
                    Next

                    ListaInformacionModulo = objDiccionarioNuevo
                    ListaInformacionModuloBusqueda = objDiccionarioNuevo
                ElseIf pobjOpcion = CombosCodificacionContable.MODULO Or pobjOpcion = CombosCodificacionContable.MODULOBUSQUEDA Then
                    Dim objDiccionarioNuevo As New Dictionary(Of String, List(Of CFCodificacionContable.ConfiguracionContableXModulo))
                    Dim strTopico As String = String.Empty

                    For Each li In ListaCodificacionContableCompleta.OrderBy(Function(i) i.strTopico)
                        If li.strTopico <> strTopico And li.strTopico <> STR_MODULO Then
                            strTopico = li.strTopico
                            objLista = New List(Of CFCodificacionContable.ConfiguracionContableXModulo)

                            For Each liDato In ListaCodificacionContableCompleta.Where(Function(i) i.strTopico = strTopico)
                                If liDato.intPadre = pintIDModulo Then
                                    objLista.Add(liDato)
                                End If
                            Next

                            objDiccionarioNuevo.Add(strTopico, objLista)
                        End If
                    Next

                    'If pobjOpcion = CombosCodificacionContable.MODULO Then
                    '    ListaInformacionModulo = objDiccionarioNuevo
                    'Else
                    '    ListaInformacionModuloBusqueda = objDiccionarioNuevo
                    'End If
                    ListaInformacionModulo = objDiccionarioNuevo
                    ListaInformacionModuloBusqueda = objDiccionarioNuevo
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al recargar los combos del encabezado.", Me.ToString(), "RecargarCombo", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Resultados Asincrónicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Procedimiento que se ejecuta cuando finaliza la ejecución de una actualización a la base de datos
    ''' </summary>
    Public Overrides Async Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Dim strMsg As String = String.Empty

        Try
            IsBusy = False
            If So.HasError Then
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                Else
                    If Not So.Error Is Nothing Then
                        strMsg = So.Error.Message
                    End If
                End If

                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.obtenerMensajeValidacion(strMsg, So.UserState.ToString, True), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

                ' Marcar los cambios como rechazados
                mdcProxy.RejectChanges()

                If So.UserState.Equals(ValoresUserState.Borrar.ToString) Then
                    ' Cuando se elimina un registro, el método RejectChanges lo vuelve a adicionar a la lista pero al final no en la posición original
                    _ListaEncabezadoPaginada.MoveToLastPage()
                    _ListaEncabezadoPaginada.MoveCurrentToLast()
                    MyBase.CambioItem("ListaEncabezadoPaginada")
                End If

                So.MarkErrorAsHandled()
            Else
                HabilitarEncabezadoNormaContable = False
                HabilitarEncabezado = False

                MyBase.TerminoSubmitChanges(So)
                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                RecargarCombo(CombosCodificacionContable.TODOS)
                Await ConsultarEncabezado(True, String.Empty)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del resultado de la actualización de los datos", Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
            If Not So Is Nothing Then
                If So.HasError Then
                    So.MarkErrorAsHandled()
                End If
            End If
        End Try
    End Sub

#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"
    ''' <summary>
    ''' Consulta los valores por defecto para un nuevo encabezado
    ''' </summary>
    Private Async Sub ConsultarEncabezadoPorDefectoSync()
        Dim objRet As LoadOperation(Of CFCodificacionContable.CodificacionContable)
        Dim dcProxy As CodificacionContableDomainContext

        Try
            dcProxy = inicializarProxyCodificacionContable()

            objRet = Await dcProxy.Load(dcProxy.ConsultarCodificacionContablePorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de los valores por defecto.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto.", Me.ToString(), "consultarEncabezadoPorDefectoSync", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    mobjEncabezadoPorDefecto = objRet.Entities.FirstOrDefault
                End If
            Else
                mobjEncabezadoPorDefecto = Nothing
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto.", Me.ToString(), "consultarEncabezadoPorDefectoSync", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consultar de forma sincrónica los datos de CodificacionContable
    ''' </summary>
    ''' <param name="plogFiltrar">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    ''' <param name="pstrFiltro">Texto que se utiliza para filtrar los datos solicitados</param>    
    ''' <history>
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 1/2014
    ''' Id caso de prueba: Id_1 
    ''' Pruebas CB       : Jorge Peña - Abril 1/2014 - Resultado Ok 
    ''' </history>
    Private Async Function ConsultarEncabezado(ByVal plogFiltrar As Boolean,
                                               ByVal pstrFiltro As String,
                                               Optional ByVal plngID As Integer = -1,
                                               Optional ByVal plngModulo As Integer = 0,
                                               Optional ByVal plngNegocio As Integer = 0,
                                               Optional ByVal plngOperacion As Integer = 0,
                                               Optional ByVal plngDuracion As Integer = 0,
                                               Optional ByVal plngTipoFecha As Integer = 0,
                                               Optional ByVal pintTipoProducto As Integer = 0,
                                               Optional ByVal pstrTipoInversion As String = "") As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFCodificacionContable.CodificacionContable)

        Try

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCodificacionContable()
            End If

            mdcProxy.CodificacionContables.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await mdcProxy.Load(mdcProxy.FiltrarCodificacionContableSyncQuery(pstrFiltro:=pstrFiltro, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            Else
                objRet = Await mdcProxy.Load(mdcProxy.ConsultarCodificacionContableSyncQuery(plngID:=plngID, plngModulo:=plngModulo,
                                                                                             plngNegocio:=plngNegocio, plngOperacion:=plngOperacion,
                                                                                             plngDuracion:=plngDuracion, plngTipoFecha:=plngTipoFecha,
                                                                                             pintTipoProducto:=pintTipoProducto, pstrTipoInversion:=pstrTipoInversion,
                                                                                             pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            End If

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "ConsultarEncabezado", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    ListaEncabezado = mdcProxy.CodificacionContables

                    If objRet.Entities.Count > 0 Then
                        If Not plogFiltrar Then
                            MyBase.ConfirmarBuscar()
                        End If
                    Else
                        If Not plogFiltrar Or (plogFiltrar And Not pstrFiltro.Equals(String.Empty)) Then
                            ' Solamente se presenta el mensaje cuando se ejecuta la opción de filtrar con un filtro específico o la opción de buscar (consultar) 
                            A2Utilidades.Mensajes.mostrarMensaje("No se encontraron datos que concuerden con los criterios de búsqueda.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                End If
            Else
                ListaEncabezado.Clear()
            End If

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")

            logResultado = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de CodificacionContableDetalle ", Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Consultar de forma sincrónica los datos de la tabla tblConfiguracionContableXModulo
    ''' </summary>
    ''' <param name="pintPadre">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    ''' <param name="pstrTopico">Texto que se utiliza para filtrar los datos solicitados</param>    
    ''' <history>
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 1/2014
    ''' Pruebas CB       : Jorge Peña - Abril 1/2014 - Resultado Ok 
    ''' </history>
    Private Async Function ConsultarListaCodificacionContable(Optional ByVal pintPadre As String = Nothing,
                                               Optional ByVal pstrTopico As String = Nothing) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of CFCodificacionContable.ConfiguracionContableXModulo)

        Try

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCodificacionContable()
            End If

            mdcProxy.ConfiguracionContableXModulos.Clear()

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarConfiguracionContableXModuloSyncQuery(pintPadre:=CInt(pintPadre),
                                                                                  pstrTopico:=pstrTopico,
                                                                                  pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()


            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "ConsultarListaCodificacionContable", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        ListaCodificacionContableCompleta = mdcProxy.ConfiguracionContableXModulos.ToList

                        RecargarCombo(CombosCodificacionContable.TIPOSMODULOS)
                        RecargarCombo(CombosCodificacionContable.TODOS)
                    Else
                        ' Solamente se presenta el mensaje cuando se ejecuta la opción de filtrar con un filtro específico o la opción de buscar (consultar) 
                        A2Utilidades.Mensajes.mostrarMensaje("No se ha configurado ninguna codificación contable. Por favor comuníquese con el administrador del sistema. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            Else
                ListaCodificacionContableCompleta.Clear()
            End If

            MyBase.CambioItem("ListaCodificacionContableCompleta")

            logResultado = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de CodificacionContableDetalle ", Me.ToString(), "ConsultarListaCodificacionContable", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

#End Region

#Region "Métodos sincrónicos del detalle - REQUERIDO (si hay detalle)"

    ''' <summary>
    ''' Consulta los detalles asociados al encabezado activo
    ''' </summary>
    ''' <param name="pintIdEncabezado">Identificador único del encabezado activo requerido para consultar los datos del detalle</param>
    ''' <param name="pintTipoDetalle">Tipo de detalles que se consultan</param>
    ''' <history>
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 1/2014
    ''' Pruebas CB       : Jorge Peña - Abril 1/2014 - Resultado Ok 
    ''' </history>
    Private Async Sub ConsultarDetalles(ByVal pintIdEncabezado As Integer, ByVal pintTipoDetalle As Detalles)
        Dim logResultado As Boolean

        Try
            If Not Await LlenarComboValor() Then
                Exit Sub
            End If
            Select Case pintTipoDetalle
                Case Detalles.cmCodificacion
                    logResultado = Await ConsultarDetalle(pintIdEncabezado)
            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los detalle del registro seleccionado. ", Me.ToString(), "ConsultarDetalles", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <summary>
    ''' Ejecutar de forma sincrónica una consulta de datos del detalle relacionado con el encabezado seleccionado
    ''' </summary>
    ''' <param name="pintIdEncabezado">Id del encabezado</param>
    ''' <remarks>NOTA: En este método no se puede utilizar la propiedad IsBusy porque hace que sean necesarios dos clic para seleccionar un detalle</remarks>
    ''' <history>
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 1/2014
    ''' Pruebas CB       : Jorge Peña - Abril 1/2014 - Resultado Ok 
    ''' </history>
    Public Async Function ConsultarDetalle(ByVal pintIdEncabezado As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim mdcProxy As CodificacionContableDomainContext = inicializarProxyCodificacionContable()
        Dim objRet As LoadOperation(Of CFCodificacionContable.CodificacionContableDetalle)

        Try
            ErrorForma = String.Empty

            If Not mdcProxy.CodificacionContableDetalles Is Nothing Then
                mdcProxy.CodificacionContableDetalles.Clear()
            End If

            objRet = Await mdcProxy.Load(mdcProxy.ConsultarCodificacionContableDetalleSyncQuery(pintIdEncabezado, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el detalle de la codificación activa pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle de la codificación activa.", Me.ToString(), "consultarDetalle", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    ListaDetalle = objRet.Entities.ToList
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del detalle de la codificación seleccionada.", Me.ToString(), "ConsultarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    ''' <summary>
    ''' Consulta los datos por defecto para un nuevo detalle
    ''' </summary>
    ''' <param name="pintTipoDetalle">Tipo de detalle que se consulta</param>
    Private Async Sub ConsultarDetallePorDefectoSync(ByVal pintTipoDetalle As Detalles)

        Dim dcProxy As CodificacionContableDomainContext

        Try
            dcProxy = inicializarProxyCodificacionContable()

            Select Case pintTipoDetalle
                Case Detalles.cmCodificacion
                    Dim objRet As LoadOperation(Of CFCodificacionContable.CodificacionContableDetalle)

                    objRet = Await dcProxy.Load(dcProxy.ConsultarCodificacionContableDetallePorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

                    If Not objRet Is Nothing Then
                        If objRet.HasError Then
                            If objRet.Error Is Nothing Then
                                A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de los valores por defecto del detalle.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                            Else
                                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto del detalle.", Me.ToString(), "ConsultarDetallePorDefectoSync", Program.TituloSistema, Program.Maquina, objRet.Error)
                            End If

                            objRet.MarkErrorAsHandled()
                        Else
                            mobjDetallePorDefecto = objRet.Entities.FirstOrDefault
                        End If
                    Else
                        mobjDetallePorDefecto = Nothing
                    End If
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto del detalle.", Me.ToString(), "consultarDetallePorDefectoSync", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Función para llenar el combo valor en el detalle.
    ''' </summary>
    ''' <history>
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion.
    ''' Fecha            : Abril 1/2014
    ''' Pruebas CB       : Jorge Peña - Abril 1/2014 - Resultado Ok 
    ''' </history>
    Private Async Function LlenarComboValor() As Task(Of Boolean)
        Dim logResultado As Boolean = False
        Try
            Dim objRet As LoadOperation(Of CFCodificacionContable.ConfiguracionContableXValor)
            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCodificacionContable()
            End If
            objRet = Await mdcProxy.Load(mdcProxy.ConsultarConfiguracionContableXValorSyncQuery(False, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If Not objRet.Entities.Count > 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontraron datos que concuerden con los criterios de búsqueda.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    ListaComboValor = mdcProxy.ConfiguracionContableXValors.ToList
                End If

            End If
            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de CodificacionContableDetalle ", Me.ToString(), "LlenarComboValor", Application.Current.ToString(), Program.Maquina, ex)
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
Public Class CamposBusquedaCodificacionContable
    Implements INotifyPropertyChanged

    Dim objViewModel As CodificacionContableViewModel

    Public Sub New(ByVal pobjViewModel As CodificacionContableViewModel)
        objViewModel = pobjViewModel
        AddHandler objViewModel.CambioItemLista, AddressOf NotificoCambioLista
    End Sub

    Private Sub NotificoCambioLista(ByVal ListaCambio As CFCodificacionContable.ConfiguracionContableXModulo, ByVal Nombre As String)
        If Not IsNothing(ListaCambio) Then

            If Not IsNothing(ListaCambio) Then
                intModulo = ListaCambio.IntId
                objViewModel.RecargarCombo(CodificacionContableViewModel.CombosCodificacionContable.MODULOBUSQUEDA, CInt(intModulo))
            End If
        End If

    End Sub


    Private _intModulo As System.Nullable(Of Integer)
    Public Property intModulo() As System.Nullable(Of Integer)
        Get
            Return _intModulo
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _intModulo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intModulo"))
        End Set
    End Property

    Private _intOperacion As System.Nullable(Of Integer)
    Public Property intOperacion() As System.Nullable(Of Integer)
        Get
            Return _intOperacion
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _intOperacion = value
            'RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intOperacion"))
        End Set
    End Property

    Private _intTipoFecha As System.Nullable(Of Integer)
    Public Property intTipoFecha() As System.Nullable(Of Integer)
        Get
            Return _intTipoFecha
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _intTipoFecha = value
            'RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intTipoFecha"))
        End Set
    End Property

    Private _strTipoInversion As String
    Public Property strTipoInversion() As String
        Get
            Return _strTipoInversion
        End Get
        Set(ByVal value As String)
            _strTipoInversion = value
            'RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoInversion"))
        End Set
    End Property

    Private _intNegocio As System.Nullable(Of Integer)
    Public Property intNegocio() As System.Nullable(Of Integer)
        Get
            Return _intNegocio
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _intNegocio = value
            'RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intNegocio"))
        End Set
    End Property

    Private _intDuracion As System.Nullable(Of Integer)
    Public Property intDuracion() As System.Nullable(Of Integer)
        Get
            Return _intDuracion
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _intDuracion = value
            'RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intDuracion"))
        End Set
    End Property

    Private _intTipoProducto As System.Nullable(Of Integer)
    Public Property intTipoProducto() As System.Nullable(Of Integer)
        Get
            Return _intTipoProducto
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _intTipoProducto = value
            'RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intTipoProducto"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class

