﻿
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
''' <remarks>Jhon Alexis Echavarria (Alcuadrado S.A.) - 01 de Marzo 2018</remarks>
''' <history>
'''
'''</history>
Public Class InstrumentosViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoPorDefecto As tblProductos
    Private mdcProxy As DER_MaestrosDomainServices ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Para manejar la forma principal 
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mobjEncabezadoAnterior As tblProductos
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
    ''' Fecha            : Marzo 01/2018
    ''' Pruebas CB       :  Jhon Alexis Echavarria (Alcuadrado S.A.) -Marzo 01/2018 - Resultado Ok 
    ''' </history>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            mdcProxy = inicializarProxy()

            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                ' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                consultarEncabezadoPorDefecto()

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

#Region "Propiedades del Encabezado - REQUERIDO"

    ''' <summary>
    ''' Lista de Registros que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezado As List(Of CPX_tblProductos)
    Public Property ListaEncabezado() As List(Of CPX_tblProductos)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As List(Of CPX_tblProductos))
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
    Private _EncabezadoSeleccionado As CPX_tblProductos
    Public Property EncabezadoSeleccionado() As CPX_tblProductos
        Get
            Return _EncabezadoSeleccionado
        End Get
        Set(ByVal value As CPX_tblProductos)
            _EncabezadoSeleccionado = value
            'Llamado de metodo para realizar la consulta del encabezado editable
            ConsultarEncabezadoEdicion()
            MyBase.CambioItem("EncabezadoSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Elemento de la lista de Registros que se encuentra seleccionado
    ''' </summary>
    Private WithEvents _EncabezadoEdicionSeleccionado As tblProductos
    Public Property EncabezadoEdicionSeleccionado() As tblProductos
        Get
            Return _EncabezadoEdicionSeleccionado
        End Get
        Set(ByVal value As tblProductos)
            _EncabezadoEdicionSeleccionado = value
            If Not IsNothing(_EncabezadoEdicionSeleccionado) Then
                If _EncabezadoEdicionSeleccionado.intIdProducto > 0 Then
                    HabilitarEdicionDetalle = True
                Else
                    HabilitarEdicionDetalle = False
                End If
            Else
                HabilitarEdicionDetalle = False
            End If
            MyBase.CambioItem("EncabezadoEdicionSeleccionado")
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private _cb As CamposBusquedaProductos
    Public Property cb() As CamposBusquedaProductos
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaProductos)
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

    Private _ListaDetalle As List(Of tblProductosVencimientos)
    Public Property ListaDetalle() As List(Of tblProductosVencimientos)
        Get
            Return _ListaDetalle
        End Get
        Set(ByVal value As List(Of tblProductosVencimientos))
            _ListaDetalle = value
            MyBase.CambioItem("ListaDetalle")
        End Set
    End Property

    'Valor del tipo Subyacente por defecto
    Private _intIdTipoSubyacenteDefecto As Integer
    Public Property intIdTipoSubyacenteDefecto() As Integer
        Get
            Return _intIdTipoSubyacenteDefecto
        End Get
        Set(ByVal value As Integer)
            _intIdTipoSubyacenteDefecto = value
            MyBase.CambioItem("intIdTipoSubyacenteDefecto")
        End Set
    End Property


    'Para habilitar o inhabilitar el combo Activo subyacente
    Private _HabilitarActivoSubyacente As Boolean = False
    Public Property HabilitarActivoSubyacente() As Boolean
        Get
            Return _HabilitarActivoSubyacente
        End Get
        Set(ByVal value As Boolean)
            _HabilitarActivoSubyacente = value
            MyBase.CambioItem("HabilitarActivoSubyacente")
        End Set
    End Property

    'Para habilitar o inhabilitar el combo de producto subyacente
    Private _HabilitarProductoSubyacente As Boolean = False
    Public Property HabilitarProductoSubyacente() As Boolean
        Get
            Return _HabilitarProductoSubyacente
        End Get
        Set(ByVal value As Boolean)
            _HabilitarProductoSubyacente = value
            MyBase.CambioItem("HabilitarProductoSubyacente")
        End Set
    End Property

    'Lista para tipos de ejercicio
    Private _ListaTipoEjercicio As List(Of ProductoCombos)
    Public Property ListaTipoEjercicio() As List(Of ProductoCombos)
        Get
            Return _ListaTipoEjercicio
        End Get
        Set(ByVal value As List(Of ProductoCombos))
            _ListaTipoEjercicio = value
            MyBase.CambioItem("ListaTipoEjercicio")
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
    Public Overrides Sub NuevoRegistro()
        Try
            Dim objNvoEncabezado As New tblProductos
            If mdcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            'Obtiene el registro por defecto
            ObtenerRegistroAnterior(mobjEncabezadoPorDefecto, objNvoEncabezado)
            'Salva el registro anterior
            ObtenerRegistroAnterior(_EncabezadoEdicionSeleccionado, mobjEncabezadoAnterior)

            Editando = True
            Habilitar_Inhabilitar_PorTipoSubyacente()
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

    Public Overrides Sub QuitarFiltro()
        MyBase.QuitarFiltro()
        strTipoFiltroBusqueda = String.Empty
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
    ''' <history>
    ''' </history>
    Public Overrides Async Sub ConfirmarBuscar()
        Try
            If Not String.IsNullOrEmpty(cb.strNombreProducto) Or cb.intIdTipoProducto > 0 Or cb.intIdTipoCumplimiento > 0 Or cb.intIdActivoSubyacente > 0 Or cb.intEstado <> -1 Then
                Await ConsultarEncabezado("BUSQUEDA", String.Empty, 0, cb.strNombreProducto, cb.intIdTipoProducto, cb.intIdTipoCumplimiento, cb.intIdActivoSubyacente, cb.intEstado)
                strTipoFiltroBusqueda = "BUSQUEDA"
            End If
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
    ''' 

    Public Overrides Async Sub ActualizarRegistro()
        Dim strAccion As String = ValoresUserState.Actualizar.ToString()

        Try
            MyBase.ActualizarRegistro()

            ErrorForma = String.Empty
            IsBusy = True

            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesGenerales)) = Await mdcProxy.Productos_ActualizarAsync(_EncabezadoEdicionSeleccionado.intIdProducto,
                                                                                                                                  _EncabezadoEdicionSeleccionado.strNombreProducto,
                                                                                                                                  _EncabezadoEdicionSeleccionado.strDescripcion,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdTipoProducto,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdActivoSubyacente,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdTipoCumplimiento,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdTipoEjercicio,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intUltimoDiaNegociacion,
                                                                                                                                  _EncabezadoEdicionSeleccionado.numValorNominalContrato,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdTipoNegociacionPrima,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdEstado,
                                                                                                                                  _EncabezadoEdicionSeleccionado.strPrefijoProducto,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intNroDecimalesPrecio,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intNroDecimalesPrima,
                                                                                                                                  _EncabezadoEdicionSeleccionado.numTickPrecio,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intMaximoNegociacion,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intMinimoRegistro,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intMinimoNegociacionPolitica,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intTipoContrato,
                                                                                                                                  _EncabezadoEdicionSeleccionado.numPorcenAutoretencion,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdTipoLiquidacion,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdTipoDerivado,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdTipoCurva,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdTipoSubyacente,
                                                                                                                                  _EncabezadoEdicionSeleccionado.bitPrecioPorcentual,
                                                                                                                                  _EncabezadoEdicionSeleccionado.intIdProductoSubyacente,
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

                        MyBase.FinalizoGuardadoRegistros()

                        ' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                        If strTipoFiltroBusqueda = "FILTRAR" Then
                            Await ConsultarEncabezado("FILTRAR", FiltroVM, intIDRegistroActualizado)
                        ElseIf strTipoFiltroBusqueda = "BUSQUEDA" And Not IsNothing(cb) Then
                            Await ConsultarEncabezado("BUSQUEDA", String.Empty, Nothing, cb.strNombreProducto)
                        Else
                            Await ConsultarEncabezado("FILTRAR", String.Empty, intIDRegistroActualizado)
                        End If
                    Else
                        MyBase.ActualizarListaInconsistencias(objListaMensajes, Program.TituloSistema)
                        IsBusy = False
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
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Editar de la barra de herramientas.
    ''' Activa la edición del encabezado y del detalle (si aplica) del encabezado activo
    ''' </summary>
    Public Overrides Sub EditarRegistro()
        Try
            If Not IsNothing(_EncabezadoEdicionSeleccionado) Then

                If mdcProxy.IsLoading Then
                    A2Utilidades.Mensajes.mostrarMensaje(Program.MensajeEsperaOperacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
                IsBusy = True

                ObtenerRegistroAnterior(_EncabezadoEdicionSeleccionado, mobjEncabezadoAnterior)

                Editando = True
                Habilitar_Inhabilitar_PorTipoSubyacente()
                MyBase.CambioItem("Editando")
                MyBase.EditarRegistro()

                IsBusy = False
            End If
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
            Habilitar_Inhabilitar_PorTipoSubyacente()
            MyBase.CambioItem("Editando")
            MyBase.CancelarEditarRegistro()

            EncabezadoEdicionSeleccionado = mobjEncabezadoAnterior
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

            If Not IsNothing(_EncabezadoSeleccionado) Then
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
            Dim strAccion As String = ValoresUserState.Actualizar.ToString()

            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                If Not IsNothing(_EncabezadoSeleccionado) Then
                    IsBusy = True

                    Dim objRespuesta As InvokeResult(Of List(Of CPX_tblValidacionesGenerales)) = Await mdcProxy.Productos_EliminarAsync(_EncabezadoSeleccionado.intID, Program.Usuario, Program.HashConexion)

                    If Not IsNothing(objRespuesta) Then
                        If Not IsNothing(objRespuesta.Value) Then
                            Dim objListaResultado As List(Of CPX_tblValidacionesGenerales) = CType(objRespuesta.Value, List(Of CPX_tblValidacionesGenerales))
                            Dim objListaMensajes As New List(Of ProductoValidaciones)

                            For Each li In objListaResultado
                                objListaMensajes.Add(New ProductoValidaciones With {
                                             .Campo = li.strCampo,
                                             .TituloCampo = MyBase.ObtenerTituloCampo(li.strCampo),
                                             .Mensaje = li.strMensaje
                                             })
                            Next
                            If (objListaResultado.Where(Function(i) i.logInconsitencia = True).Count) = 0 Then
                                A2Utilidades.Mensajes.mostrarMensaje(objListaResultado.First.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Exito)

                                EncabezadoSeleccionado = Nothing

                                Await ConsultarEncabezado(True, String.Empty)
                            Else
                                MyBase.ActualizarListaInconsistencias(objListaMensajes, Program.TituloSistema)
                                IsBusy = False
                            End If
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al mostrar la lista de mensajes.", Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                            IsBusy = False
                        End If
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al mostrar la lista de mensajes.", Program.TituloSistema, wppMensajes.TiposMensaje.Errores)
                        IsBusy = False
                    End If
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
            Dim objCB As New CamposBusquedaProductos
            objCB.strNombreProducto = String.Empty
            objCB.intIdTipoProducto = 0
            objCB.intIdTipoCumplimiento = 0
            objCB.intIdActivoSubyacente = 0
            objCB.intEstado = 1
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
    Private Sub ObtenerRegistroAnterior(ByVal pobjRegistro As tblProductos, ByRef pobjRegistroSalvar As tblProductos)
        Dim objEncabezado As tblProductos = New tblProductos

        Try
            If Not IsNothing(pobjRegistro) Then

                objEncabezado.intIdProducto = pobjRegistro.intIdProducto
                objEncabezado.strNombreProducto = pobjRegistro.strNombreProducto
                objEncabezado.strDescripcion = pobjRegistro.strDescripcion
                objEncabezado.intIdTipoProducto = pobjRegistro.intIdTipoProducto
                objEncabezado.intIdActivoSubyacente = pobjRegistro.intIdActivoSubyacente
                objEncabezado.intIdTipoCumplimiento = pobjRegistro.intIdTipoCumplimiento
                objEncabezado.intIdTipoEjercicio = pobjRegistro.intIdTipoEjercicio
                objEncabezado.intUltimoDiaNegociacion = pobjRegistro.intUltimoDiaNegociacion
                objEncabezado.numValorNominalContrato = pobjRegistro.numValorNominalContrato
                objEncabezado.intIdTipoNegociacionPrima = pobjRegistro.intIdTipoNegociacionPrima
                objEncabezado.intIdEstado = pobjRegistro.intIdEstado
                objEncabezado.strPrefijoProducto = pobjRegistro.strPrefijoProducto
                objEncabezado.intNroDecimalesPrecio = pobjRegistro.intNroDecimalesPrecio
                objEncabezado.intNroDecimalesPrima = pobjRegistro.intNroDecimalesPrima
                objEncabezado.numTickPrecio = pobjRegistro.numTickPrecio
                objEncabezado.intMaximoNegociacion = pobjRegistro.intMaximoNegociacion
                objEncabezado.intMinimoRegistro = pobjRegistro.intMinimoRegistro
                objEncabezado.intMinimoNegociacionPolitica = pobjRegistro.intMinimoNegociacionPolitica
                objEncabezado.intTipoContrato = pobjRegistro.intTipoContrato
                objEncabezado.numPorcenAutoretencion = pobjRegistro.numPorcenAutoretencion
                objEncabezado.intIdTipoLiquidacion = pobjRegistro.intIdTipoLiquidacion
                objEncabezado.intIdTipoDerivado = pobjRegistro.intIdTipoDerivado
                objEncabezado.intIdTipoCurva = pobjRegistro.intIdTipoCurva
                objEncabezado.intIdTipoSubyacente = pobjRegistro.intIdTipoSubyacente
                objEncabezado.bitPrecioPorcentual = pobjRegistro.bitPrecioPorcentual
                objEncabezado.intIdProductoSubyacente = pobjRegistro.intIdProductoSubyacente
                objEncabezado.strUsuario = pobjRegistro.strUsuario

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
    Private Sub _EncabezadoEdicionSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EncabezadoEdicionSeleccionado.PropertyChanged
        Try
            If Editando Then
                If e.PropertyName = "intIdTipoProducto" Then

                    ListaTipoEjercicio = DiccionarioCombosPantalla("TIPOEJERCICIO").Where(Function(i) i.IDDependencia1 = EncabezadoEdicionSeleccionado.intIdTipoProducto).ToList

                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, DiccionarioMensajesPantalla("GENERICO_ERRORREGISTRO"), Me.ToString(), "_EncabezadoEdicionSeleccionado_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    'Método para habilitar o inhabilitar los combos de activo subyacente  o el producto subyacente de acuerdo al tipo subyacente seleccionado
    Public Sub Habilitar_Inhabilitar_PorTipoSubyacente()
        Try

            If Not IsNothing(EncabezadoEdicionSeleccionado) Then
                If EncabezadoEdicionSeleccionado.intIdTipoSubyacente = intIdTipoSubyacenteDefecto Then
                    If Editando Then
                        HabilitarActivoSubyacente = True
                        HabilitarProductoSubyacente = False
                        EncabezadoEdicionSeleccionado.intIdProductoSubyacente = -1
                    Else
                        HabilitarActivoSubyacente = False
                        HabilitarProductoSubyacente = False
                    End If
                Else
                    If Editando Then
                        HabilitarProductoSubyacente = True
                        HabilitarActivoSubyacente = False
                        EncabezadoEdicionSeleccionado.intIdActivoSubyacente = -1
                    Else
                        HabilitarActivoSubyacente = False
                        HabilitarProductoSubyacente = False
                    End If
                End If
            End If
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
            Dim objRespuesta As InvokeResult(Of tblProductos)

            objRespuesta = Await mdcProxy.Productos_DefectoAsync(Program.Usuario, Program.HashConexion)

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    mobjEncabezadoPorDefecto = objRespuesta.Value

                    'Obtener el valor del tipo subyacente por defecto
                    intIdTipoSubyacenteDefecto = mobjEncabezadoPorDefecto.intIdTipoSubyacente

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

    ''' <summary>
    ''' Consultar de forma sincrónica los datos de Registro
    ''' </summary>
    ''' <param name="pstrOpcion">Indica si la consulta se hace por la funcionalidad de filtrar (si es verdadero) o de consultar (si es falso)</param>
    Private Async Function ConsultarEncabezado(ByVal pstrOpcion As String,
                                               Optional ByVal pstrFiltro As String = "",
                                               Optional ByVal pintIDRegistro As Integer = 0,
                                               Optional ByVal pstrNombreProducto As String = "",
                                               Optional ByVal pintTipoProducto As Integer = 0,
                                               Optional ByVal pintTipoCumplimiento As Integer = 0,
                                               Optional ByVal pintActivoSubyacente As Integer = 0,
                                               Optional ByVal pintEstado As Integer = 1) As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            IsBusy = True
            Dim objRespuesta As InvokeResult(Of List(Of CPX_tblProductos)) = Nothing

            ErrorForma = String.Empty

            If pstrOpcion = "FILTRAR" Then
                pstrFiltro = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrFiltro, String.Empty)) ' Transformar caracteres especiales para evitar errores en su interpretación
                objRespuesta = Await mdcProxy.Productos_FiltrarAsync(pstrFiltro, Program.Usuario, Program.HashConexion)

            Else
                pstrNombreProducto = System.Web.HttpUtility.UrlEncode(Program.validarValorString(pstrNombreProducto, String.Empty))

                objRespuesta = Await mdcProxy.Productos_ConsultarAsync(pstrNombreProducto, pintTipoProducto, pintTipoCumplimiento, pintActivoSubyacente, pintEstado, Program.Usuario, Program.HashConexion)
            End If

            If Not IsNothing(objRespuesta) Then
                If Not IsNothing(objRespuesta.Value) Then
                    ListaEncabezado = objRespuesta.Value

                    If pintIDRegistro > 0 Then
                        If ListaEncabezado.Where(Function(i) i.intID = pintIDRegistro).Count > 0 Then
                            EncabezadoSeleccionado = ListaEncabezado.Where(Function(i) i.intID = pintIDRegistro).First
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
        Finally
            IsBusy = False
        End Try

        Return (logResultado)
    End Function

    Public Async Sub ConsultarEncabezadoEdicion()
        Try
            EncabezadoEdicionSeleccionado = Nothing

            If Not IsNothing(_EncabezadoSeleccionado) Then
                If _EncabezadoSeleccionado.intID > 0 Then
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
    ''' <param name="pintID">Indica el ID de la entidad a consultar</param>
    Private Async Function ConsultarEncabezadoEdicion(ByVal pintID As Integer) As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            IsBusy = True
            Dim objRespuesta As InvokeResult(Of tblProductos) = Nothing

            ErrorForma = String.Empty

            objRespuesta = Await mdcProxy.Productos_IDAsync(pintID, Program.Usuario, Program.HashConexion)

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
Public Class CamposBusquedaProductos
    Implements INotifyPropertyChanged

    Private _strNombreProducto As String
    Public Property strNombreProducto() As String
        Get
            Return _strNombreProducto
        End Get
        Set(ByVal value As String)
            _strNombreProducto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strNombreProducto"))
        End Set
    End Property

    Private _intIdTipoProducto As Short
    Public Property intIdTipoProducto() As Short
        Get
            Return _intIdTipoProducto
        End Get
        Set(ByVal value As Short)
            _intIdTipoProducto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intIdTipoProducto"))
        End Set
    End Property

    Private _intIdTipoCumplimiento As Integer
    Public Property intIdTipoCumplimiento() As Integer
        Get
            Return _intIdTipoCumplimiento
        End Get
        Set(ByVal value As Integer)
            _intIdTipoCumplimiento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intIdTipoCumplimiento"))
        End Set
    End Property

    Private _intIdActivoSubyacente As Integer
    Public Property intIdActivoSubyacente() As Integer
        Get
            Return _intIdActivoSubyacente
        End Get
        Set(ByVal value As Integer)
            _intIdActivoSubyacente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intIdActivoSubyacente"))
        End Set
    End Property

    Private _intEstado As Short
    Public Property intEstado() As Short
        Get
            Return _intEstado
        End Get
        Set(ByVal value As Short)
            _intEstado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intEstado"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class