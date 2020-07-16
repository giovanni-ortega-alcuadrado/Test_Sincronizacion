Imports Telerik.Windows.Controls

Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFCalculosFinancieros
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Text.RegularExpressions
Imports System.Object
Imports System.Globalization.CultureInfo
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades
Imports System.Web

Public Class ControlLiquidezOperacionesViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As ControlLiquidezOperaciones
    Private mdcProxy As CalculosFinancierosDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private mobjVM As ControlLiquidezOperacionesViewModel
    Dim cwControlLiquidezOperacionesView As cwControlLiquidezOperacionesView

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As ControlLiquidezOperaciones
    Private mobjDetallePorDefecto As ControlLiquidezOperacionesDetalle
    Private Const STR_PARAMETROS_EXPORTAR As String = "[COMPANIA]=[[COMPANIA]]|[COMITENTE]=[[COMITENTE]]|[FECHA_PROCESO]=[[FECHA_PROCESO]]|[TIPO_FECHA_PROCESO]=[[TIPO_FECHA_PROCESO]]|[MODULO]=[[MODULO]]|[SUBMODULO]=[[SUBMODULO]]|[MONEDA]=[[MONEDA]]|[USUARIO]=[[USUARIO]]"
    Private Const STR_CARPETA As String = "CONTROL_LIQUIDEZ_OPERACIONES"
    Private Const STR_PROCESO As String = "CONTROL_LIQUIDEZ_OPERACIONES"
    Private strModulosSeleccionados As String = String.Empty
    Private strSubmodulosSeleccionados As String = String.Empty

    Private Enum PARAMETROSEXPORTAR
        COMPANIA
        COMITENTE
        FECHA_PROCESO
        TIPO_FECHA_PROCESO
        MODULO
        SUBMODULO
        MONEDA
        USUARIO
    End Enum

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
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then

                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                ConsultarEncabezadoPorDefectoSync()
                ConsultarDetallePorDefectoSync()

                'Módulos y submodulos
                Await ConsultarModulosSubmodulos()

                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarEncabezado(True, String.Empty)

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    ''' <summary>
    ''' Lista de ControlLiquidezOperaciones que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As EntitySet(Of ControlLiquidezOperaciones)
    Public Property ListaEncabezado() As EntitySet(Of ControlLiquidezOperaciones)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As EntitySet(Of ControlLiquidezOperaciones))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaEncabezado")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de ControlLiquidezOperaciones para navegar sobre el grid con paginación
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
    ''' Elemento de la lista de ControlLiquidezOperaciones que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoSeleccionado As ControlLiquidezOperaciones
    Public Property EncabezadoSeleccionado() As ControlLiquidezOperaciones
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As ControlLiquidezOperaciones)

            Dim logIncializarDet As Boolean = False

            _EncabezadoSeleccionado = value

            If _EncabezadoSeleccionado Is Nothing Then
                logIncializarDet = True
            Else
                If _EncabezadoSeleccionado.intIDControlLiquidezOperaciones > 0 Then
                    strAgrupamiento = "CONTROL_LIQUIDEZ" + CStr(EncabezadoSeleccionado.intIDCompania)
                    dblTotalAuxiliar = EncabezadoSeleccionado.dblTotal

                    If Not String.IsNullOrEmpty(EncabezadoSeleccionado.strModulo) Then

                        If Not IsNothing(ListaModulos) Then
                            For Each li In ListaModulos
                                li.logSeleccionado = False
                            Next

                            Dim strModulosConcatenados = Split(EncabezadoSeleccionado.strModulo, "**")

                            For Each obj In strModulosConcatenados
                                ListaModulos.Where(Function(i) i.strRetorno = obj).FirstOrDefault.logSeleccionado = True
                            Next

                        End If

                    End If

                    If Not String.IsNullOrEmpty(EncabezadoSeleccionado.strSubmodulo) Then

                        Dim objListaSubmodulos = From info In ListaModulosSubmodulos
                                                 Join info1 In ListaModulos.Where(Function(i) CType(i.logSeleccionado, Boolean) = True) On info.strIDOwner Equals info1.strRetorno
                                                 Select info
                                                 Where info.strTopico = "SUBMODULOS"


                        If Not IsNothing(objListaSubmodulos) Then

                            If Not IsNothing(ListaSubmodulos) Then
                                For Each li In ListaSubmodulos
                                    li.logSeleccionado = False
                                Next
                            End If


                            Dim strSubmodulosConcatenados = Split(EncabezadoSeleccionado.strSubmodulo, "**")

                            For Each obj In strSubmodulosConcatenados

                                objListaSubmodulos.Where(Function(i) i.strRetorno = obj).FirstOrDefault.logSeleccionado = True

                            Next


                            Dim objListaSubmodulosFinal As New List(Of CFCalculosFinancieros.ModulosSubmodulos)

                            For Each li In objListaSubmodulos
                                If objListaSubmodulosFinal.Where(Function(i) i.strDescripcion = li.strDescripcion).Count = 0 Then
                                    objListaSubmodulosFinal.Add(li)
                                End If
                            Next

                            ListaSubmodulos = objListaSubmodulosFinal

                        End If

                    End If

                    ConsultarDetalle(_EncabezadoSeleccionado.intIDControlLiquidezOperaciones)
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
        Try
            If Not IsNothing(EncabezadoSeleccionado) Then
                'If e.PropertyName = "intIDCompania" Then
                '    strAgrupamiento = "CONTROL_LIQUIDEZ" + CStr(EncabezadoSeleccionado.intIDCompania)
                'End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar los botones del detalle.", Me.ToString(), "_EncabezadoSeleccionado_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Lista de detalles de la entidad en este caso detalle de codificacion de activos
    ''' </summary>
    Private WithEvents _ListaDetalle As List(Of ControlLiquidezOperacionesDetalle)
    Public Property ListaDetalle() As List(Of ControlLiquidezOperacionesDetalle)
        Get
            Return _ListaDetalle
        End Get
        Set(ByVal value As List(Of ControlLiquidezOperacionesDetalle))
            _ListaDetalle = value
            MyBase.CambioItem("ListaDetalle")
            MyBase.CambioItem("ListaDetallePaginada")
        End Set
    End Property

    Private _ListaDatos As List(Of ControlLiquidezOperacionesDatos)
    Public Property ListaDatos() As List(Of ControlLiquidezOperacionesDatos)
        Get
            Return _ListaDatos
        End Get
        Set(ByVal value As List(Of ControlLiquidezOperacionesDatos))
            _ListaDatos = value
        End Set
    End Property

    Private Sub _ListaDetalle_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles Me.PropertyChanged
        Try
            If Not IsNothing(ListaDetalle) Then
                If e.PropertyName = "ListaDetalle" And Editando Then

                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar los botones del detalle.", Me.ToString(), "_EncabezadoSeleccionado_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

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
    Private WithEvents _DetalleSeleccionado As ControlLiquidezOperacionesDetalle
    Public Property DetalleSeleccionado() As ControlLiquidezOperacionesDetalle
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As ControlLiquidezOperacionesDetalle)
            _DetalleSeleccionado = value
            MyBase.CambioItem("DetalleSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaControlLiquidezOperaciones
    Public Property cb() As CamposBusquedaControlLiquidezOperaciones
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaControlLiquidezOperaciones)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

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

    Private _HabilitarBorrarDetalle As Boolean = False
    Public Property HabilitarBorrarDetalle() As Boolean
        Get
            Return _HabilitarBorrarDetalle
        End Get
        Set(ByVal value As Boolean)
            _HabilitarBorrarDetalle = value
            MyBase.CambioItem("HabilitarBorrarDetalle")
        End Set
    End Property

    Private _BorrarEspecie As Boolean = False
    Public Property BorrarEspecie() As Boolean
        Get
            Return _BorrarEspecie
        End Get
        Set(ByVal value As Boolean)
            _BorrarEspecie = value
            MyBase.CambioItem("BorrarEspecie")
        End Set
    End Property

    Private _strNombreCompania As String
    Public Property strNombreCompania() As String
        Get
            Return _strNombreCompania
        End Get
        Set(ByVal value As String)
            _strNombreCompania = value
            MyBase.CambioItem("strNombreCompania")
        End Set
    End Property

    Private _dblTotalAuxiliar As System.Nullable(Of System.Double)
    Public Property dblTotalAuxiliar() As System.Nullable(Of System.Double)
        Get
            Return _dblTotalAuxiliar
        End Get
        Set(ByVal value As System.Nullable(Of System.Double))
            _dblTotalAuxiliar = value
            MyBase.CambioItem("dblTotalAuxiliar")
        End Set
    End Property

    Private _strAgrupamiento As String
    Public Property strAgrupamiento() As String
        Get
            Return _strAgrupamiento
        End Get
        Set(ByVal value As String)
            _strAgrupamiento = value
            MyBase.CambioItem("strAgrupamiento")
        End Set
    End Property

    Private _ListaModulosSubmodulos As List(Of ModulosSubmodulos)
    Public Property ListaModulosSubmodulos() As List(Of ModulosSubmodulos)
        Get
            Return _ListaModulosSubmodulos
        End Get
        Set(ByVal value As List(Of ModulosSubmodulos))
            _ListaModulosSubmodulos = value
            MyBase.CambioItem("ListaModulosSubmodulos")
        End Set
    End Property

    Private _ListaModulos As List(Of ModulosSubmodulos)
    Public Property ListaModulos() As List(Of ModulosSubmodulos)
        Get
            Return _ListaModulos
        End Get
        Set(ByVal value As List(Of ModulosSubmodulos))
            _ListaModulos = value
            MyBase.CambioItem("ListaModulos")
        End Set
    End Property

    Private _SubmoduloSeleccionado As ModulosSubmodulos
    Public Property SubmoduloSeleccionado() As ModulosSubmodulos
        Get
            Return _SubmoduloSeleccionado
        End Get
        Set(ByVal value As ModulosSubmodulos)
            _SubmoduloSeleccionado = value
            MyBase.CambioItem("SubmoduloSeleccionado")
        End Set
    End Property

    Private _ListaSubmodulos As List(Of ModulosSubmodulos)
    Public Property ListaSubmodulos() As List(Of ModulosSubmodulos)
        Get
            Return _ListaSubmodulos
        End Get
        Set(ByVal value As List(Of ModulosSubmodulos))
            _ListaSubmodulos = value
            MyBase.CambioItem("ListaSubmodulos")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    ''' <summary>
    ''' Se ejecuta cuando se da clic en el botón Nuevo de la barra de herramientas.
    ''' Inicializa un nuevo objeto que contiene los datos por defecto para ingresar un nuevo encabezado y lo coloca como el objeto seleccionado en el encabezado
    ''' </summary>
    Public Overrides Sub NuevoRegistro()

        Dim objNvoEncabezado As New ControlLiquidezOperaciones

        Try
            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not mobjEncabezadoPorDefecto Is Nothing Then
                Program.CopiarObjeto(Of ControlLiquidezOperaciones)(mobjEncabezadoPorDefecto, objNvoEncabezado)
            Else
                objNvoEncabezado.intIDControlLiquidezOperaciones = -1
                objNvoEncabezado.intIDCompania = 0
                objNvoEncabezado.lngIDComitente = String.Empty
                objNvoEncabezado.dtmFechaProceso = Nothing
                objNvoEncabezado.strModulo = String.Empty
                objNvoEncabezado.lngIDMoneda = 0
                objNvoEncabezado.dblSaldo = 0
                objNvoEncabezado.dblCompras = 0
                objNvoEncabezado.dblVentas = 0
                objNvoEncabezado.dblTotal = 0
                dblTotalAuxiliar = 0
            End If

            objNvoEncabezado.strUsuario = Program.Usuario

            mobjEncabezadoAnterior = ObtenerRegistroAnterior()

            Editando = True
            MyBase.CambioItem("Editando")

            EncabezadoSeleccionado = objNvoEncabezado

            HabilitarEncabezado = True
            strAgrupamiento = String.Empty
            dblTotalAuxiliar = 0

            If Not IsNothing(ListaModulos) Then
                For Each li In ListaModulos
                    li.logSeleccionado = False
                Next
            End If

            If Not IsNothing(ListaSubmodulos) Then
                For Each li In ListaModulosSubmodulos
                    li.logSeleccionado = False
                Next
            End If

            If Not IsNothing(ListaSubmodulos) Then
                ListaSubmodulos = Nothing
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando se da clic en la opción Buscar de la barra de herramientas.
    ''' Ejecuta una búsqueda sobre los datos que contengan en los campos definidos internamente en el procedimiento de búsqueda (filtrado) el texto ingresado en el campo Filtro de la barra de herramientas
    ''' </summary>
    Public Overrides Async Sub Filtrar()
        Try
            Await ConsultarEncabezado(True, FiltroVM)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inicializar la ejecución del filtro.", Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

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
    Public Overrides Async Sub ConfirmarBuscar()
        Try
            If Not IsNothing(cb.intIDCompania) Or Not IsNothing(cb.lngIDComitente) Or Not IsNothing(cb.dtmFechaProceso) Or Not IsNothing(cb.strTipoFechaProceso) Or Not IsNothing(cb.strModulo) Or Not IsNothing(cb.lngIDMoneda) Then 'Validar que ingresó algo en los campos de búsqueda
                Await ConsultarEncabezado(False, String.Empty, cb.intIDCompania, cb.lngIDComitente, cb.dtmFechaProceso, cb.strTipoFechaProceso, cb.strModulo, CInt(cb.lngIDMoneda))
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro.", Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón guardar de la barra de herramientas. 
    ''' Ejecuta el proceso que ingresa o actualiza la base de datos con los cambios realizados 
    ''' </summary>
    Public Overrides Async Sub ActualizarRegistro()
        Dim strAccion As String = ValoresUserState.Actualizar.ToString()

        Try
            Dim xmlCompleto As String
            Dim xmlDetalle As String
            ErrorForma = String.Empty

            If ValidarRegistro() Then

                If ValidarDetalle() Then

                    Await BuscarOperaciones()

                    xmlCompleto = "<ControlLiquidezOperaciones>"

                    If Not IsNothing(ListaDetalle) Then

                        For Each objeto In (From c In ListaDetalle)

                            xmlDetalle = "<Detalle intIDControlLiquidezOperacionesDetalle=""" & objeto.intIDControlLiquidezOperacionesDetalle &
                                        """ strDescripcion=""" & objeto.strDescripcion & """ strSigno=""" & objeto.strSigno &
                                        """ dblValor=""" & objeto.dblValor & """></Detalle>"

                            xmlCompleto = xmlCompleto & xmlDetalle
                        Next

                    End If

                    xmlCompleto = xmlCompleto & "</ControlLiquidezOperaciones>"



                    IsBusy = True

                    Dim strMsg As String = String.Empty
                    Dim objRet As InvokeOperation(Of String)

                    ModulosSeleccionados()

                    SubmodulosSeleccionados()

                    objRet = Await mdcProxy.ControlLiquidezOperaciones_ActualizarSync(EncabezadoSeleccionado.intIDControlLiquidezOperaciones, EncabezadoSeleccionado.intIDCompania, _
                                                                                       EncabezadoSeleccionado.lngIDComitente, EncabezadoSeleccionado.dtmFechaProceso,
                                                                                       EncabezadoSeleccionado.strTipoFechaProceso, strModulosSeleccionados, strSubmodulosSeleccionados, _
                                                                                       EncabezadoSeleccionado.lngIDMoneda, EncabezadoSeleccionado.dblSaldo, _
                                                                                       EncabezadoSeleccionado.dblCompras, EncabezadoSeleccionado.dblVentas, _
                                                                                       EncabezadoSeleccionado.dblTotal, xmlCompleto, Program.Usuario, Program.HashConexion).AsTask()

                    If Not String.IsNullOrEmpty(objRet.Value.ToString()) Then
                        strMsg = String.Format("{0}{1} + {2}", strMsg, vbCrLf, objRet.Value.ToString())

                        If Not String.IsNullOrEmpty(strMsg) Then
                            A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias: " & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    Else
                        Editando = False
                        HabilitarEncabezado = False
                        HabilitarEdicionDetalle = False
                        HabilitarBorrarDetalle = False
                        ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                        Await ConsultarEncabezado(True, String.Empty)
                    End If
                Else
                    HabilitarEncabezado = True
                    HabilitarEdicionDetalle = True
                    HabilitarBorrarDetalle = True
                End If

            Else
                HabilitarEncabezado = True
                HabilitarEdicionDetalle = True
                HabilitarBorrarDetalle = True
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

                Editando = True
                MyBase.CambioItem("Editando")

                HabilitarEncabezado = True
                HabilitarEdicionDetalle = True
                HabilitarBorrarDetalle = True

                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al editar el registro seleccionado.", Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
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

                EncabezadoSeleccionado = mobjEncabezadoAnterior
                HabilitarEncabezado = False
                HabilitarEdicionDetalle = False
                HabilitarBorrarDetalle = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro.", Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        MyBase.CancelarBuscar()
    End Sub

    ''' <summary>
    ''' Se ejcuta cuando el usuario da clic en el botón Borrar de la barra de herramientas e incia el proceso de eliminación del encabezado activo
    ''' </summary>
    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If

                A2Utilidades.Mensajes.mostrarMensajePregunta("Está opción borra el registro seleccionado. ¿Confirma el borrado de este registro?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf BorrarRegistroConfirmado)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro.", Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejcutá cuando el usuario confirma la eliminación del encabezado activo.
    ''' Ejecuta el proceso de eliminación en la base de datos
    ''' </summary>
    Private Sub BorrarRegistroConfirmado(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                If Not IsNothing(_EncabezadoSeleccionado) Then
                    IsBusy = True

                    mobjEncabezadoAnterior = ObtenerRegistroAnterior()

                    If mdcProxy.ControlLiquidezOperaciones.Where(Function(i) i.intIDControlLiquidezOperaciones = EncabezadoSeleccionado.intIDControlLiquidezOperaciones).Count > 0 Then
                        mdcProxy.ControlLiquidezOperaciones.Remove(mdcProxy.ControlLiquidezOperaciones.Where(Function(i) i.intIDControlLiquidezOperaciones = EncabezadoSeleccionado.intIDControlLiquidezOperaciones).First)
                    End If

                    If _ListaEncabezado.Count > 0 Then
                        EncabezadoSeleccionado = _ListaEncabezado.LastOrDefault
                    Else
                        EncabezadoSeleccionado = Nothing
                        For Each li In ListaModulos
                            li.logSeleccionado = False
                        Next
                        ListaSubmodulos = Nothing
                    End If

                    Program.VerificarCambiosProxyServidor(mdcProxy)
                    mdcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, ValoresUserState.Borrar.ToString)

                    'Await ConsultarEncabezado(True, String.Empty)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro.", Me.ToString(), "BorrarRegistroConfirmado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ActualizarDetalle()
        Try
            If Not IsNothing(DetalleSeleccionado) Then
                cwControlLiquidezOperacionesView = New cwControlLiquidezOperacionesView(Me, DetalleSeleccionado, mobjDetallePorDefecto, HabilitarEncabezado)
                Program.Modal_OwnerMainWindowsPrincipal(cwControlLiquidezOperacionesView)
                cwControlLiquidezOperacionesView.ShowDialog()
            Else
                A2Utilidades.Mensajes.mostrarMensaje("No es posible editar este detalle.", "ActualizarDetalle", wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle.", Me.ToString(), "ActualizarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Nuevo del detalle 
    ''' Solamente se ingresa un nuevo elemento en la lista del detalle para que el usuario ingrese el nuevo detalle
    ''' </summary>
    Public Sub IngresarDetalle()
        Try

            cwControlLiquidezOperacionesView = New cwControlLiquidezOperacionesView(Me, Nothing, mobjDetallePorDefecto, HabilitarEncabezado)
            Program.Modal_OwnerMainWindowsPrincipal(cwControlLiquidezOperacionesView)
            cwControlLiquidezOperacionesView.ShowDialog()

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el detalle.", Me.ToString(), "IngresarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Borrar del detalle 
    ''' Solamente se elimina el registro de la lista del detalle pero no se afecta base de datos. Esto se hace al guardar el encabezado.
    ''' </summary>
    Public Sub BorrarDetalle()
        Try

            If Not IsNothing(DetalleSeleccionado) Then

                If _ListaDetalle.Where(Function(i) i.intIDControlLiquidezOperacionesDetalle = _DetalleSeleccionado.intIDControlLiquidezOperacionesDetalle).Count > 0 Then
                    _ListaDetalle.Remove(_ListaDetalle.Where(Function(i) i.intIDControlLiquidezOperacionesDetalle = _DetalleSeleccionado.intIDControlLiquidezOperacionesDetalle).First)
                End If

                Dim objNuevaListaDetalle As New List(Of ControlLiquidezOperacionesDetalle)

                For Each li In _ListaDetalle
                    objNuevaListaDetalle.Add(li)
                Next

                If objNuevaListaDetalle.Where(Function(i) i.intIDControlLiquidezOperacionesDetalle = _DetalleSeleccionado.intIDControlLiquidezOperacionesDetalle).Count > 0 Then
                    objNuevaListaDetalle.Remove(objNuevaListaDetalle.Where(Function(i) i.intIDControlLiquidezOperacionesDetalle = _DetalleSeleccionado.intIDControlLiquidezOperacionesDetalle).First)
                End If

                ListaDetalle = objNuevaListaDetalle

                If Not IsNothing(_ListaDetalle) Then
                    If _ListaDetalle.Count > 0 Then
                        DetalleSeleccionado = _ListaDetalle.First
                    End If
                End If
                CalcularTotal()
            Else
                A2Utilidades.Mensajes.mostrarMensaje("No es posible borrar este detalle o no existe.", "BorrarDetalle", wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar el registro seleccionado.", Me.ToString(), "BorrarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consulta el nombre y la fecha de portafolio de un cliente.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Async Function ConsultarDatosPortafolio(ByVal plngIDComitente As String) As Task
        Try
            Dim objRet As LoadOperation(Of DatosPortafolios)
            Dim dcProxy As CalculosFinancierosDomainContext

            dcProxy = inicializarProxyCalculosFinancieros()

            objRet = Await dcProxy.Load(dcProxy.ConsultarDatosPortafolioSyncQuery(plngIDComitente, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta para el método ConsultarDatosPortafolio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        EncabezadoSeleccionado.strDescripcionComitente = objRet.Entities.First.strNombre
                    Else
                        EncabezadoSeleccionado.lngIDComitente = Nothing
                        EncabezadoSeleccionado.strDescripcionComitente = Nothing
                    End If
                End If
            Else
                EncabezadoSeleccionado.lngIDComitente = Nothing
                EncabezadoSeleccionado.strDescripcionComitente = Nothing
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la consulta para el método ConsultarDatosPortafolio.", Me.ToString(), "ConsultarDatosPortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    Public Async Function BuscarOperaciones() As Task
        Dim mdcProxy As CalculosFinancierosDomainContext = inicializarProxyCalculosFinancieros()
        Dim objRet As LoadOperation(Of ControlLiquidezOperacionesDatos)

        Try
            If ValidarRegistro() Then

                IsBusy = True

                ErrorForma = String.Empty

                If Not mdcProxy.ArbitrajesDetalles Is Nothing Then
                    mdcProxy.ArbitrajesDetalles.Clear()
                End If

                mdcProxy = inicializarProxyCalculosFinancieros()

                ModulosSeleccionados()

                SubmodulosSeleccionados()

                objRet = Await mdcProxy.Load(mdcProxy.ControlLiquidezOperaciones_BuscarSyncQuery(EncabezadoSeleccionado.intIDCompania, EncabezadoSeleccionado.lngIDComitente, EncabezadoSeleccionado.dtmFechaProceso, EncabezadoSeleccionado.strTipoFechaProceso, strModulosSeleccionados, strSubmodulosSeleccionados, EncabezadoSeleccionado.lngIDMoneda, Program.Usuario, Program.HashConexion)).AsTask()

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        If objRet.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de las operaciones", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de las operaciones.", Me.ToString(), "BuscarOperaciones", Program.TituloSistema, Program.Maquina, objRet.Error)
                        End If

                        objRet.MarkErrorAsHandled()
                    Else
                        ListaDatos = objRet.Entities.ToList

                        EncabezadoSeleccionado.dblSaldo = ListaDatos.First.dblSaldo
                        EncabezadoSeleccionado.dblCompras = ListaDatos.First.dblCompras
                        EncabezadoSeleccionado.dblVentas = ListaDatos.First.dblVentas

                        CalcularTotal()
                    End If
                Else
                    ListaDetalle = Nothing
                End If

                IsBusy = False
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la consulta de las operaciones.", Me.ToString(), "BuscarOperaciones", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function

    Public Async Function ConsultarModulosSubmodulos() As Task
        Dim mdcProxy As CalculosFinancierosDomainContext = inicializarProxyCalculosFinancieros()
        Dim objRet As LoadOperation(Of ModulosSubmodulos)

        Try

            IsBusy = True

            ErrorForma = String.Empty

            If Not mdcProxy.ArbitrajesDetalles Is Nothing Then
                mdcProxy.ArbitrajesDetalles.Clear()
            End If

            mdcProxy = inicializarProxyCalculosFinancieros()

            objRet = Await mdcProxy.Load(mdcProxy.ControlLiquidezOperaciones_ModulosSubmodulosSyncQuery(Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de las operaciones", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de las operaciones.", Me.ToString(), "BuscarOperaciones", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    ListaModulosSubmodulos = objRet.Entities.ToList

                    ListaModulos = ListaModulosSubmodulos.Where(Function(i) i.strTopico = "MODULOS").ToList
                End If
            Else
                ListaDetalle = Nothing
            End If

            IsBusy = False


        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la consulta de las operaciones.", Me.ToString(), "BuscarOperaciones", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"

    ''' <summary>
    ''' Inicializa el objeto en el cual se capturan los filtros seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaControlLiquidezOperaciones
            objCB.intIDCompania = 0
            objCB.lngIDComitente = String.Empty
            objCB.dtmFechaProceso = Nothing
            objCB.strTipoFechaProceso = String.Empty
            objCB.strModulo = String.Empty
            objCB.lngIDMoneda = 0

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
    Private Function ObtenerRegistroAnterior() As ControlLiquidezOperaciones
        Dim objEncabezado As ControlLiquidezOperaciones = New ControlLiquidezOperaciones

        Try
            If Not IsNothing(_EncabezadoSeleccionado) Then
                Program.CopiarObjeto(Of ControlLiquidezOperaciones)(_EncabezadoSeleccionado, objEncabezado)
                objEncabezado.intIDControlLiquidezOperaciones = _EncabezadoSeleccionado.intIDControlLiquidezOperaciones
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para guardar los datos del registro activo antes de su modificación.", Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (objEncabezado)
    End Function

    Private Function ValidarRegistro() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(_EncabezadoSeleccionado) Then

                If Not IsNothing(ListaModulos) Then
                    If ListaModulos.Where(Function(i) CType(i.logSeleccionado, Boolean) = True).Count = 0 Then
                        strMsg = String.Format("{0}{1} + El módulo es un campo requerido.", strMsg, vbCrLf)
                    End If
                End If

                If Not IsNothing(ListaSubmodulos) Then
                    If ListaSubmodulos.Count > 0 Then
                        If ListaSubmodulos.Where(Function(i) CType(i.logSeleccionado, Boolean) = True).Count = 0 Then
                            strMsg = String.Format("{0}{1} + El submódulo es un campo requerido.", strMsg, vbCrLf)
                        End If
                    End If
                End If

                'Valida la compañía
                If IsNothing(_EncabezadoSeleccionado.intIDCompania) Or _EncabezadoSeleccionado.intIDCompania = 0 Then
                    strMsg = String.Format("{0}{1} + La compañía es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida la fecha
                If IsNothing(_EncabezadoSeleccionado.dtmFechaProceso) Then
                    strMsg = String.Format("{0}{1} + La fecha es un campo requerido.", strMsg, vbCrLf)
                End If

                'Valida el tipo de fecha
                If String.IsNullOrEmpty(_EncabezadoSeleccionado.strTipoFechaProceso) Then
                    strMsg = String.Format("{0}{1} + El tipo de fecha es un campo requerido.", strMsg, vbCrLf)
                End If

                If IsNothing(_EncabezadoSeleccionado.lngIDMoneda) Then
                    strMsg = String.Format("{0}{1} + La moneda es un campo requerido.", strMsg, vbCrLf)
                End If

            Else
                strMsg = String.Format("{0}{1} Debe de seleccionar un registro", strMsg, vbCrLf)
            End If

            If strMsg.Equals(String.Empty) Then
                '------------------------------------------------------------------------------------------------------------------------
                '-- VALIDAR DATOS DEL DETALLE
                '-------------------------------------------------------------------------------------------------------------------------
                logResultado = True
            Else
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias antes de guardar: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "ValidarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

    Public Sub CalcularTotal()
        Try
            Dim dblSumatoriaValorAdicion As Double
            Dim dblSumatoriaValorSustraccion As Double

            If Not IsNothing(ListaDetalle) Then
                dblSumatoriaValorAdicion = CDbl(ListaDetalle.Where(Function(i) i.strSigno = "A").Sum(Function(i) i.dblValor))
                dblSumatoriaValorSustraccion = CDbl(ListaDetalle.Where(Function(i) i.strSigno = "S").Sum(Function(i) i.dblValor)) * -1
            Else
                dblSumatoriaValorAdicion = 0
                dblSumatoriaValorSustraccion = 0
            End If

            EncabezadoSeleccionado.dblTotal = EncabezadoSeleccionado.dblSaldo - EncabezadoSeleccionado.dblCompras + EncabezadoSeleccionado.dblVentas + _
                                              dblSumatoriaValorAdicion + dblSumatoriaValorSustraccion


            dblTotalAuxiliar = EncabezadoSeleccionado.dblTotal
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al calcular el total.", Me.ToString(), "CalcularTotal", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Sub ExportarInformacion()
        Try
            Dim objRet As LoadOperation(Of GenerarArchivosPlanos)
            Dim dcProxyUtil As UtilidadesDomainContext
            Dim strParametros As String = STR_PARAMETROS_EXPORTAR
            Dim dia As String = String.Empty
            Dim mes As String = String.Empty
            Dim ano As String = String.Empty
            Dim strFechaProceso As String = String.Empty

            If ValidarRegistro() Then

                IsBusy = True

                dcProxyUtil = inicializarProxyUtilidadesOYD()

                If (EncabezadoSeleccionado.dtmFechaProceso.Value.Day < 10) Then
                    dia = "0" + EncabezadoSeleccionado.dtmFechaProceso.Value.Day.ToString
                Else
                    dia = EncabezadoSeleccionado.dtmFechaProceso.Value.Day.ToString
                End If

                If (EncabezadoSeleccionado.dtmFechaProceso.Value.Month < 10) Then
                    mes = "0" + EncabezadoSeleccionado.dtmFechaProceso.Value.Month.ToString
                Else
                    mes = EncabezadoSeleccionado.dtmFechaProceso.Value.Month.ToString
                End If

                ano = EncabezadoSeleccionado.dtmFechaProceso.Value.Year.ToString

                strFechaProceso = ano + mes + dia

                strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.COMPANIA), CType(EncabezadoSeleccionado.intIDCompania, String))
                strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.COMITENTE), EncabezadoSeleccionado.lngIDComitente)
                strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.FECHA_PROCESO), strFechaProceso)
                strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.TIPO_FECHA_PROCESO), EncabezadoSeleccionado.strTipoFechaProceso)
                strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.MODULO), strModulosSeleccionados)
                strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.SUBMODULO), strSubmodulosSeleccionados)
                strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.MONEDA), CType(EncabezadoSeleccionado.lngIDMoneda, String))
                strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.USUARIO), Program.Usuario)

                objRet = Await dcProxyUtil.Load(dcProxyUtil.GenerarArchivoPlanoSyncQuery(STR_CARPETA, STR_PROCESO, strParametros, "RPT_CONTROL_LIQUIDEZ_OPERACIONES", "TAB", "EXCEL", Program.Maquina, Program.Usuario, Program.HashConexion)).AsTask

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        If objRet.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la generación del archivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la generación del archivo.", Me.ToString(), "ExportarInformeValoracion", Program.TituloSistema, Program.Maquina, objRet.Error)
                        End If
                        'IsBusy = False
                        objRet.MarkErrorAsHandled()
                    Else
                        If objRet.Entities.Count > 0 Then
                            Dim objResultado = objRet.Entities.First

                            If objResultado.Exitoso Then
                                Program.VisorArchivosWeb_DescargarURL(objResultado.RutaArchivoPlano)
                            Else
                                A2Utilidades.Mensajes.mostrarMensaje(objResultado.Mensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                            End If
                        End If
                        'IsBusy = False
                    End If
                End If
            End If
            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso ExportarInformacion", _
                                                             Me.ToString(), "ExportarInformacion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Function ValorAReemplazar(ByVal pintTipoCampo As PARAMETROSEXPORTAR) As String
        Return String.Format("[[{0}]]", pintTipoCampo.ToString)
    End Function

    Public Sub ModulosSeleccionados()
        Try
            strModulosSeleccionados = String.Empty

            If Not IsNothing(ListaModulos) Then
                For Each li In ListaModulos.Where(Function(i) CType(i.logSeleccionado, Boolean) = True)
                    If strModulosSeleccionados = String.Empty Then
                        strModulosSeleccionados = strModulosSeleccionados + li.strRetorno
                    Else
                        strModulosSeleccionados = strModulosSeleccionados + "**" + li.strRetorno
                    End If
                Next
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al concatenar los módulos.", Me.ToString(), "ModulosSeleccionados", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub SubmodulosSeleccionados()
        Try
            strSubmodulosSeleccionados = String.Empty

            If Not IsNothing(ListaSubmodulos) Then
                For Each li In ListaSubmodulos.Where(Function(i) CType(i.logSeleccionado, Boolean) = True)
                    If strSubmodulosSeleccionados = String.Empty Then
                        strSubmodulosSeleccionados = strSubmodulosSeleccionados + li.strRetorno
                    Else
                        strSubmodulosSeleccionados = strSubmodulosSeleccionados + "**" + li.strRetorno
                    End If
                Next
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al concatenar los submódulos.", Me.ToString(), "SubmodulosSeleccionados", Application.Current.ToString(), Program.Maquina, ex)
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
                HabilitarEncabezado = False
                HabilitarEdicionDetalle = False
                HabilitarBorrarDetalle = False

                MyBase.TerminoSubmitChanges(So)
                ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                Await ConsultarEncabezado(True, String.Empty)
            End If
        Catch ex As Exception
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
        Dim objRet As LoadOperation(Of ControlLiquidezOperaciones)
        Dim dcProxy As CalculosFinancierosDomainContext

        Try
            dcProxy = inicializarProxyCalculosFinancieros()

            objRet = Await dcProxy.Load(dcProxy.ControlLiquidezOperaciones_ConsultarPorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de los valores por defecto.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto.", Me.ToString(), "ConsultarEncabezadoPorDefectoSync", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    mobjEncabezadoPorDefecto = objRet.Entities.FirstOrDefault
                End If
            Else
                mobjEncabezadoPorDefecto = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto.", Me.ToString(), "ConsultarEncabezadoPorDefectoSync", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Consultar de forma sincrónica los datos de ControlLiquidezOperaciones
    ''' </summary>
    ''' <param name="plogFiltrar">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    ''' <param name="pstrFiltro">Texto que se utiliza para filtrar los datos solicitados</param>    
    Private Async Function ConsultarEncabezado(ByVal plogFiltrar As Boolean,
                                               ByVal pstrFiltro As String,
                                               Optional ByVal pintIDCompania As Integer = 0,
                                               Optional ByVal plngIDComitente As String = "",
                                               Optional ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime) = Nothing,
                                               Optional ByVal pstrTipoFechaProceso As String = "",
                                               Optional ByVal pstrModulo As String = "",
                                               Optional ByVal plngIDMoneda As Integer = 0) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim objRet As LoadOperation(Of ControlLiquidezOperaciones)

        Try
            IsBusy = True

            ErrorForma = String.Empty

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyCalculosFinancieros()
            End If

            mdcProxy.ControlLiquidezOperaciones.Clear()

            If plogFiltrar Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRet = Await mdcProxy.Load(mdcProxy.ControlLiquidezOperaciones_FiltrarSyncQuery(pstrFiltro:=pstrFiltro, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion)).AsTask()
            Else
                objRet = Await mdcProxy.Load(mdcProxy.ControlLiquidezOperaciones_ConsultarSyncQuery(pintIDCompania:=pintIDCompania,
                                                                                                    plngIDComitente:=plngIDComitente,
                                                                                                    pdtmFechaProceso:=pdtmFechaProceso,
                                                                                                    pstrTipoFechaProceso:=pstrTipoFechaProceso,
                                                                                                    pstrModulo:=pstrModulo,
                                                                                                    plngIDMoneda:=plngIDMoneda,
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
                    ListaEncabezado = mdcProxy.ControlLiquidezOperaciones

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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción de la lista de ControlLiquidezOperaciones ", Me.ToString(), "ConsultarEncabezado", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

#End Region

#Region "Métodos sincrónicos del detalle - REQUERIDO (si hay detalle)"

    ''' <summary>
    ''' Consulta los datos por defecto para un nuevo detalle
    ''' </summary>
    Private Async Sub ConsultarDetallePorDefectoSync()

        Dim dcProxy As CalculosFinancierosDomainContext

        Try
            dcProxy = inicializarProxyCalculosFinancieros()

            Dim objRet As LoadOperation(Of ControlLiquidezOperacionesDetalle)

            objRet = Await dcProxy.Load(dcProxy.ControlLiquidezOperacionesDetalle_ConsultarPorDefectoSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

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
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los valores por defecto del detalle.", Me.ToString(), "consultarDetallePorDefectoSync", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Ejecutar de forma sincrónica una consulta de datos del detalle relacionado con el encabezado seleccionado
    ''' </summary>
    ''' <param name="pintIdEncabezado">Id del encabezado</param>
    ''' <remarks>NOTA: En este método no se puede utilizar la propiedad IsBusy porque hace que sean necesarios dos clic para seleccionar un detalle</remarks>
    Public Async Function ConsultarDetalle(ByVal pintIdEncabezado As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False
        Dim mdcProxy As CalculosFinancierosDomainContext = inicializarProxyCalculosFinancieros()
        Dim objRet As LoadOperation(Of ControlLiquidezOperacionesDetalle)

        Try
            ErrorForma = String.Empty

            If Not mdcProxy.ControlLiquidezOperacionesDetalles Is Nothing Then
                mdcProxy.ControlLiquidezOperacionesDetalles.Clear()
            End If

            objRet = Await mdcProxy.Load(mdcProxy.ControlLiquidezOperacionesDetalle_ConsultarSyncQuery(pintIdEncabezado, Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el detalle pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el detalle del registro seleccionado.", Me.ToString(), "ConsultarDetalle", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                Else
                    ListaDetalle = objRet.Entities.ToList
                End If
            End If

            logResultado = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la recepción del detalle del registro seleccionado.", Me.ToString(), "ConsultarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

#End Region

#Region "Métodos privados del detalle - REQUERIDO (si hay detalle)"

    ''' <summary>
    ''' Procedimiento que se ejecuta cuando se va guardar un nuevo encabezado o actualizar el activo. 
    ''' Se debe llamar desde el procedimiento ValidarRegistro
    ''' </summary>
    Private Function ValidarDetalle() As Boolean
        Dim logResultado As Boolean = True
        Dim strMsg As String = String.Empty

        Try
            '------------------------------------------------------------------------------------------------------------------------------------------------
            '-- Valida que por lo menos exista un detalle para poder crear todo un registro
            '------------------------------------------------------------------------------------------------------------------------------------------------
            'If IsNothing(_ListaDetalle) Then
            '    strMsg = String.Format("{0}{1} + Debe ingresar por lo menos un detalle.", strMsg, vbCrLf)
            'ElseIf _ListaDetalle.Count = 0 Then
            '    strMsg = String.Format("{0}{1} + Debe ingresar por lo menos un detalle.", strMsg, vbCrLf)
            'End If

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

#End Region

End Class

''' <summary>
''' REQUERIDO
''' 
''' Clase base para forma de búsquedas 
''' Esta clase se instancia para crear un objeto que guarda los valores seleccionados por el usuario en la forma de búsqueda
''' Sus atributos dependen de los datos del encabezado relevantes en una búsqueda
''' </summary>
Public Class CamposBusquedaControlLiquidezOperaciones
    Implements INotifyPropertyChanged

    Private _intIDCompania As Integer
    Public Property intIDCompania() As Integer
        Get
            Return _intIDCompania
        End Get
        Set(ByVal value As Integer)
            _intIDCompania = value
            'strAgrupamiento = "CONTROL_LIQUIDEZ" + CStr(intIDCompania)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intIDCompania"))
        End Set
    End Property

    Private _strDescripcionCompania As String
    Public Property strDescripcionCompania() As String
        Get
            Return _strDescripcionCompania
        End Get
        Set(ByVal value As String)
            _strDescripcionCompania = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDescripcionCompania"))
        End Set
    End Property

    Private _lngIDComitente As String
    Public Property lngIDComitente() As String
        Get
            Return _lngIDComitente
        End Get
        Set(ByVal value As String)
            _lngIDComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDComitente"))
        End Set
    End Property

    Private _strDescripcionComitente As String
    Public Property strDescripcionComitente() As String
        Get
            Return _strDescripcionComitente
        End Get
        Set(ByVal value As String)
            _strDescripcionComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDescripcionComitente"))
        End Set
    End Property

    Private _dtmFechaProceso As System.Nullable(Of System.DateTime)
    Public Property dtmFechaProceso() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaProceso
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaProceso = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmFechaProceso"))
        End Set
    End Property

    Private _strTipoFechaProceso As String
    Public Property strTipoFechaProceso() As String
        Get
            Return _strTipoFechaProceso
        End Get
        Set(ByVal value As String)
            _strTipoFechaProceso = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoFechaProceso"))
        End Set
    End Property

    Private _strModulo As String
    Public Property strModulo() As String
        Get
            Return _strModulo
        End Get
        Set(ByVal value As String)
            _strModulo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strModulo"))
        End Set
    End Property

    Private _lngIDMoneda As System.Nullable(Of Integer)
    Public Property lngIDMoneda() As System.Nullable(Of Integer)
        Get
            Return _lngIDMoneda
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _lngIDMoneda = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDMoneda"))
        End Set
    End Property

    Private _strAgrupamiento As String
    Public Property strAgrupamiento() As String
        Get
            Return _strAgrupamiento
        End Get
        Set(ByVal value As String)
            _strAgrupamiento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strAgrupamiento"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class

