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
Imports System.Collections.ObjectModel
Imports A2.OyD.OYDServer.RIA.Web.OyDTesoreria
Imports A2ComunesImportaciones

Public Class GenerarComprobantesEgresos_IntermediaViewModel
    Inherits A2ControlMenu.A2ViewModel

    Implements INotifyPropertyChanged

    ''' <summary>
    ''' ViewModel para la pantalla Procesar Portafolio perteneciente al proyecto de Cálculos Financieros.
    ''' </summary>
    ''' <history>
    ''' Creado por       : Catalina Dávila (IoSoft S.A.)
    ''' Descripción      : Creacion. 
    ''' Fecha            : 26 de Junio/2016
    ''' Pruebas CB       : Jorge Peña - 26 de Junio/2016 - Resultado Ok 
    ''' </history>

#Region "Constantes"

#End Region

#Region "Variables - REQUERIDO"

    Public ViewGenerarRecibosCaja_Intermedia As GenerarRecibosCaja_IntermediaView = Nothing

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As TesoreriaDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private mdcProxyUtilidad As UtilidadesDomainContext

#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                mdcProxy = New TesoreriaDomainContext()
                mdcProxyUtilidad = New UtilidadesDomainContext()
            Else
                mdcProxy = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                mdcProxyUtilidad = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            End If

            DirectCast(mdcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 15, 0)
            DirectCast(mdcProxyUtilidad.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 15, 0)

            IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function inicializar() As Boolean
        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then
                mdcProxyUtilidad.Load(mdcProxyUtilidad.cargarCombosEspecificosQuery("GenerarIntermedia", Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombosEspecificos, "")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)

    End Function

    Private Sub TerminoCargarCombosEspecificos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        'se definen de tipo observable los diccionarios y los recursos List
        Dim objListaCombos As List(Of OYDUtilidades.ItemCombo) = Nothing
        Dim objListaNodosCategoria As List(Of OYDUtilidades.ItemCombo) = Nothing
        Dim dicListaCombos As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)) = Nothing
        Dim strNombreCategoria As String = String.Empty

        Try
            If Not lo.HasError Then
                'Obtiene los valores del UserState
                'Convierte los datos recibidos en un diccionario donde el nombre de la categoría es la clave
                objListaCombos = New List(Of OYDUtilidades.ItemCombo)(lo.Entities)
                If objListaCombos.Count > 0 Then
                    Dim listaCategorias = From lc In objListaCombos Select lc.Categoria Distinct 'Lista de categorias incluidas en la consulta retornada

                    ' Guardar los datos recibidos en un diccionario
                    dicListaCombos = New Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))

                    For Each NombreCategoria As String In listaCategorias
                        strNombreCategoria = NombreCategoria
                        objListaNodosCategoria = New List(Of OYDUtilidades.ItemCombo)((From ln In objListaCombos Where ln.Categoria = strNombreCategoria))

                        dicListaCombos.Add(NombreCategoria, objListaNodosCategoria)
                    Next

                    DiccionarioCarga = dicListaCombos

                    Dim strSucursalDefectoUsuario As String = String.Empty

                    If DiccionarioCarga.ContainsKey("SUCURSALDEFECTOUSUARIO") Then
                        strSucursalDefectoUsuario = DiccionarioCarga("SUCURSALDEFECTOUSUARIO").First.ID
                    End If

                    If DiccionarioCarga.ContainsKey("Sucursales") Then
                        If DiccionarioCarga("Sucursales").Count > 0 Then
                            If DiccionarioCarga("Sucursales").Where(Function(i) i.ID = strSucursalDefectoUsuario).Count > 0 Then
                                intSucursal = strSucursalDefectoUsuario
                            End If
                        End If
                    End If

                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos específicos", _
                     Me.ToString(), "TerminoCargarCombosEspecificos", Program.TituloSistema, Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la carga los combos específicos", _
                     Me.ToString(), "TerminoCargarCombosEspecificos", Program.TituloSistema, Program.Maquina, ex)
        End Try

        IsBusy = False
    End Sub

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    Private _DiccionarioCarga As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
    Public Property DiccionarioCarga() As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
        Get
            Return _DiccionarioCarga
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))
            _DiccionarioCarga = value
            MyBase.CambioItem("DiccionarioCarga")
        End Set
    End Property

    ''' <summary>
    ''' Lista de Indicadores que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private WithEvents _ListaEncabezado As List(Of GenerarCEIntermedia)
    Public Property ListaDetalle() As List(Of GenerarCEIntermedia)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As List(Of GenerarCEIntermedia))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value

            MyBase.CambioItem("ListaDetalle")
            MyBase.CambioItem("ListaEncabezadoPaginada")
        End Set
    End Property

    Private _ListaEncabezadoPaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de Indicadores para navegar sobre el grid con paginación
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

    Private _intSucursal As String
    Public Property intSucursal() As String
        Get
            Return _intSucursal
        End Get
        Set(ByVal value As String)
            _intSucursal = value
            ListaDetalle = Nothing
            logSeleccionartodos = False
            logSeleccionartodosConfirmacion = False

            MyBase.CambioItem("intSucursal")
        End Set
    End Property

    ''' <summary>
    ''' Indica cuál de los detalles está seleccionado
    ''' </summary>
    Private WithEvents _DetalleSeleccionado As GenerarCEIntermedia
    Public Property DetalleSeleccionado() As GenerarCEIntermedia
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As GenerarCEIntermedia)
            _DetalleSeleccionado = value

            MyBase.CambioItem("DetalleSeleccionado")
        End Set
    End Property

    Private _ViewImportarArchivo As cwCargaArchivos
    Public Property ViewImportarArchivo() As cwCargaArchivos
        Get
            Return _ViewImportarArchivo
        End Get
        Set(ByVal value As cwCargaArchivos)
            _ViewImportarArchivo = value
        End Set
    End Property

    Private _logSeleccionartodos As Boolean = False
    Public Property logSeleccionartodos() As Boolean
        Get
            Return _logSeleccionartodos
        End Get
        Set(ByVal value As Boolean)
            _logSeleccionartodos = value
            If Not IsNothing(_logSeleccionartodos) Then
                SeleccionarTodos(_logSeleccionartodos)
            End If
            MyBase.CambioItem("logSeleccionartodos")
        End Set
    End Property

    Private _logSeleccionartodosConfirmacion As Boolean = False
    Public Property logSeleccionartodosConfirmacion() As Boolean
        Get
            Return _logSeleccionartodosConfirmacion
        End Get
        Set(ByVal value As Boolean)
            _logSeleccionartodosConfirmacion = value
            If Not IsNothing(_logSeleccionartodosConfirmacion) Then
                SeleccionarTodosConfirmacion(_logSeleccionartodosConfirmacion)
            End If
            MyBase.CambioItem("logSeleccionartodosConfirmacion")
        End Set
    End Property

    Private _ListaConfirmacionUsuario As List(Of ResultadosGenerarRCIntermedia)
    Public Property ListaConfirmacionUsuario() As List(Of ResultadosGenerarRCIntermedia)
        Get
            Return _ListaConfirmacionUsuario
        End Get
        Set(ByVal value As List(Of ResultadosGenerarRCIntermedia))
            _ListaConfirmacionUsuario = value
            MyBase.CambioItem("ListaConfirmacionUsuario")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"


    ''' <summary>
    ''' Ejecuta el proceso de generar documentos si el usuario presiona el botón Generar RC.
    ''' </summary>
    Public Sub GenerarCE()
        Try
            If ValidarDatos() Then
                A2Utilidades.Mensajes.mostrarMensajePregunta("¿ Está seguro de generar los comprobantes de egresos ?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf ConfirmarGenerarCE, False)
            Else
                Exit Sub
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al presionar el botón Generar CE", Me.ToString(), "GenerarCE", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Ejecuta el proceso de generar documentos si el usuario presiona el botón Generar RC.
    ''' </summary>
    Public Sub ConfirmacionGenerarCE()
        Try
            If Not IsNothing(ListaConfirmacionUsuario) And Not IsNothing(ListaDetalle) Then
                For Each li In ListaConfirmacionUsuario
                    If li.Confirmar Then
                        If ListaDetalle.Where(Function(i) i.intIDDatosIntermedia = li.IDDatosIntermedia).Count > 0 Then
                            For Each objDetalle In ListaDetalle.Where(Function(i) i.intIDDatosIntermedia = li.IDDatosIntermedia).ToList
                                If String.IsNullOrEmpty(objDetalle.CodigoConfirmacion) Then
                                    objDetalle.CodigoConfirmacion = li.CodigoConfirmacion
                                Else
                                    If Not objDetalle.CodigoConfirmacion.Contains(li.CodigoConfirmacion) Then
                                        objDetalle.CodigoConfirmacion = objDetalle.CodigoConfirmacion & "*" & li.CodigoConfirmacion
                                    End If
                                End If
                            Next
                        End If
                    End If
                Next
            End If

            GenerarTotalmenteGenerarCE()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al presionar el botón Generar CE", Me.ToString(), "ConfirmacionGenerarCE", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para confirmar si continua el proceso si hay fechas sin valoración inferiores a la fecha de valoración.
    ''' </summary>
    Public Sub ConfirmarGenerarCE(ByVal sender As Object, ByVal e As EventArgs)
        Try

            If Not CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                Exit Sub
            Else
                A2Utilidades.Mensajes.mostrarMensajePregunta("¿ Está totalmente seguro de generar los comprobantes de egresos ?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf ConfirmarTotalmenteGenerarCE, False)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al preguntar si está seguro de generar los Recibos de Caja", _
                                                             Me.ToString(), "ConfirmarGenerarCE", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Sub ConfirmarTotalmenteGenerarCE(ByVal sender As Object, ByVal e As EventArgs)
        Try

            If Not CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                Exit Sub
            End If

            If Not IsNothing(ListaDetalle) Then
                Dim xmlCompleto As String
                Dim xmlDetalle As String

                Dim objRet As LoadOperation(Of ResultadosGenerarRCIntermedia)

                If mdcProxy Is Nothing Then
                    mdcProxy = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                End If

                IsBusy = True

                xmlCompleto = String.Empty

                For Each objeto In (From c In ListaDetalle)
                    If objeto.logGenerar Then
                        If String.IsNullOrEmpty(xmlCompleto) Then
                            xmlDetalle = objeto.intIDDatosIntermedia & "=" & 1 & "=" & IIf(String.IsNullOrEmpty(objeto.CodigoConfirmacion), "NINGUNA", objeto.CodigoConfirmacion)
                        Else
                            xmlDetalle = "|" & objeto.intIDDatosIntermedia & "=" & 1 & "=" & IIf(String.IsNullOrEmpty(objeto.CodigoConfirmacion), "NINGUNA", objeto.CodigoConfirmacion)
                        End If

                        xmlCompleto = xmlCompleto & xmlDetalle
                    End If
                Next

                mdcProxy.ResultadosGenerarRCIntermedias.Clear()

                objRet = Await mdcProxy.Load(mdcProxy.GenerarCEIntermediaValidarConfirmacionesSyncQuery(intSucursal.ToString, xmlCompleto, Program.Usuario, Program.Maquina, Program.HashConexion)).AsTask()

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al seleccionar y consultar los registros.", Me.ToString(), "SeleccionarTodos", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                End If

                If objRet.Entities.ToList.Where(Function(i) i.Tipo = "CONFIRMACION").Count > 0 Then
                    Dim objResultado As New List(Of ResultadosGenerarRCIntermedia)

                    For Each li In objRet.Entities.ToList.Where(Function(i) i.Tipo = "CONFIRMACION")
                        objResultado.Add(li)
                    Next

                    ListaConfirmacionUsuario = objResultado

                    Dim objViewConfirmacionCE As New ConfirmarGeneracionComprobantes_Intermedia(Me)
                    ' JFSB 20160920 Se pone en falso cada vez que se muestre el popup
                    logSeleccionartodosConfirmacion = False
                    Program.Modal_OwnerMainWindowsPrincipal(objViewConfirmacionCE)
                    objViewConfirmacionCE.ShowDialog()
                    IsBusy = False
                Else
                    GenerarTotalmenteGenerarCE()
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar los documentos de tesorería.", _
                                                             Me.ToString(), "ConfirmarTotalmenteGenerarCE", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Sub GenerarTotalmenteGenerarCE()
        Try
            If Not IsNothing(ListaDetalle) Then
                Dim xmlCompleto As String
                Dim xmlDetalle As String

                Dim objRet As LoadOperation(Of GenerarCEIntermedia)

                If mdcProxy Is Nothing Then
                    mdcProxy = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                End If

                IsBusy = True

                xmlCompleto = String.Empty

                For Each objeto In (From c In ListaDetalle)
                    If objeto.logGenerar Then
                        If String.IsNullOrEmpty(xmlCompleto) Then
                            xmlDetalle = objeto.intIDDatosIntermedia & "=" & 1 & "=" & IIf(String.IsNullOrEmpty(objeto.CodigoConfirmacion), "NINGUNA", objeto.CodigoConfirmacion)
                        Else
                            xmlDetalle = "|" & objeto.intIDDatosIntermedia & "=" & 1 & "=" & IIf(String.IsNullOrEmpty(objeto.CodigoConfirmacion), "NINGUNA", objeto.CodigoConfirmacion)
                        End If

                        xmlCompleto = xmlCompleto & xmlDetalle
                    End If

                Next

                mdcProxy.GenerarCEIntermedias.Clear()

                objRet = Await mdcProxy.Load(mdcProxy.GenerarCE_Intermedia_GenerarSyncQuery(intSucursal.ToString, xmlCompleto, Program.Usuario, Program.Maquina, Program.HashConexion)).AsTask()

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al seleccionar y consultar los registros.", Me.ToString(), "SeleccionarTodos", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If
                End If

                ListaDetalle = mdcProxy.GenerarCEIntermedias.ToList

                GenerarCE_Intermedia_Resultados()

                logSeleccionartodos = False
                logSeleccionartodosConfirmacion = False

                IsBusy = False
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar los documentos de tesorería.", _
                                                             Me.ToString(), "GenerarTotalmenteGenerarCE", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub SeleccionarTodos(ByVal plogSelecionarTodos As Boolean)
        Try
            If Not IsNothing(_ListaEncabezado) Then
                For Each li In _ListaEncabezado
                    li.logGenerar = plogSelecionarTodos
                Next
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al seleccionar todos los registros.", _
                                                             Me.ToString(), "SeleccionarTodos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub SeleccionarTodosConfirmacion(ByVal plogSelecionarTodos As Boolean)
        Try
            If Not IsNothing(_ListaConfirmacionUsuario) Then
                Dim objListaConfirmacionUsuario As New List(Of ResultadosGenerarRCIntermedia)
                objListaConfirmacionUsuario = _ListaConfirmacionUsuario

                For Each li In objListaConfirmacionUsuario
                    li.Confirmar = plogSelecionarTodos
                Next

                ListaConfirmacionUsuario = Nothing
                ListaConfirmacionUsuario = objListaConfirmacionUsuario
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al seleccionar todos los registros.", _
                                                             Me.ToString(), "SeleccionarTodos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Function ConsultarDocumentos() As Task
        Try
            If String.IsNullOrEmpty(intSucursal) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe de seleccionar la sucursal.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Function
            End If

            Dim objRet As LoadOperation(Of GenerarCEIntermedia)

            IsBusy = True

            logSeleccionartodos = False

            If mdcProxy Is Nothing Then
                mdcProxy = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
            End If

            mdcProxy.GenerarCEIntermedias.Clear()

            objRet = Await mdcProxy.Load(mdcProxy.GenerarCE_Intermedia_ConsultarSyncQuery(intSucursal, Program.Usuario, Program.Maquina, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al seleccionar y consultar los registros.", Me.ToString(), "ConsultarDocumentos", Program.TituloSistema, Program.Maquina, objRet.Error)
                End If
            End If

            ListaDetalle = mdcProxy.GenerarCEIntermedias.ToList

            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al presionar el botón Consultar Documentos", Me.ToString(), "ConsultarDocumentos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    ''' <summary>
    ''' Método para capturar la respuesta del usuario para eliminar el portafolio para todos los clientes o uno en específico
    ''' </summary>
    Public Async Sub GenerarCE_Intermedia_Resultados()
        Try
            Dim objRet As LoadOperation(Of ResultadosGenerarRCIntermedia)
            Dim viewImportacion As New cwCargaArchivos(CType(Me, GenerarComprobantesEgresos_IntermediaViewModel), String.Empty, String.Empty)

            ViewImportarArchivo.IsBusy = True

            If mdcProxy Is Nothing Then
                mdcProxy = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
            End If

            mdcProxy.ResultadosGenerarRCIntermedias.Clear()

            objRet = Await mdcProxy.Load(mdcProxy.GenerarCE_Intermedia_ResultadosSyncQuery(Program.Usuario, Program.Maquina, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al validar el proceso.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        Dim objListaRespuesta As List(Of ResultadosGenerarRCIntermedia)
                        Dim objListaMensajes As New List(Of String)

                        objListaRespuesta = objRet.Entities.ToList

                        If objListaRespuesta.Count > 0 Then
                            For Each li In objListaRespuesta
                                objListaMensajes.Add(li.Mensaje)
                            Next
                            ViewImportarArchivo.ListaMensajes = objListaMensajes
                        End If
                    End If
                End If
            End If
            ViewImportarArchivo.IsBusy = False
            Program.Modal_OwnerMainWindowsPrincipal(viewImportacion)
            viewImportacion.ShowDialog()



        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el resultado de validaciones.", Me.ToString(), "GenerarCE_Intermedia_Resultados", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Sub CargarArchivo(ByVal pstrModulo As String, ByVal _NombreArchivo As String)
        Try
            'Este método se utiliza vacío para que la pantalla de resultados no falle, ya que valida que este método exista en el ViewModel.
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el método CargarArchivo. ", Me.ToString(), "CargarArchivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"
    Private Function ValidarDatos() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------

            If IsNothing(intSucursal) Or intSucursal = 0 Then
                strMsg = String.Format("{0}{1} + La sucursal es un campo requerido.", strMsg, vbCrLf)
            End If

            If IsNothing(ListaDetalle) Then
                strMsg = String.Format("{0}{1} + Debe consultar primero los documentos para poder generar.", strMsg, vbCrLf)
            ElseIf ListaDetalle.Count = 0 Then
                strMsg = String.Format("{0}{1} + Debe consultar primero los documentos para poder generar.", strMsg, vbCrLf)
            ElseIf ListaDetalle.Where(Function(i) i.logGenerar).Count = 0 Then
                strMsg = String.Format("{0}{1} + No nay Comprobantes de egresos marcados para generar.", strMsg, vbCrLf)
            End If

            If strMsg.Equals(String.Empty) Then
                '------------------------------------------------------------------------------------------------------------------------
                '-- VALIDAR DATOS DEL DETALLE
                '-------------------------------------------------------------------------------------------------------------------------
                logResultado = True
            Else
                IsBusy = False
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias antes de guardar: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "ValidarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)
    End Function

#End Region

    'Public Event PropertyChanged1(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

End Class

