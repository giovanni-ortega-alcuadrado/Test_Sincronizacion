Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OYD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OYD.OYDServer.RIA.Web.OYDUtilidades
Imports System.Web
Imports System.Collections.ObjectModel
Imports A2.OYD.OYDServer.RIA.Web.OyDTesoreria
Imports A2ComunesImportaciones

Public Class TransferenciaPagosMasivosViewModel
    Inherits A2ControlMenu.A2ViewModel

    Implements INotifyPropertyChanged

#Region "Constantes"

    Private Const RUTAARCHIVO As String = "PAGOSMASIVOS_RutaArchivoPago"
    Private Const RUTAARCHIVO_BACKUP As String = "BACKUP_ARCHIVOSPAGOS_PAB"

#End Region

#Region "Variables - REQUERIDO"

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As TesoreriaDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private dcProxyImportaciones As ImportacionesDomainContext
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
                dcProxyImportaciones = New ImportacionesDomainContext
                mdcProxyUtilidad = New UtilidadesDomainContext()
            Else
                mdcProxy = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                dcProxyImportaciones = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
                mdcProxyUtilidad = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            End If

            DirectCast(mdcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 60, 0) 'DEMC20191002 se aumenta el timeout a 60 minutos.
            DirectCast(dcProxyImportaciones.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 60, 0)
            DirectCast(mdcProxyUtilidad.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 60, 0)

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
                IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
                mdcProxyUtilidad.Load(mdcProxyUtilidad.cargarCombosEspecificosQuery("TransferenciaPagosMasivos", Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombosEspecificos, "")
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

                    If DiccionarioCarga.ContainsKey("CONSECUTIVOEGRESOS") Then
                        If DiccionarioCarga("CONSECUTIVOEGRESOS").Count > 0 Then
                            ConsecutivoSeleccionado = DiccionarioCarga("CONSECUTIVOEGRESOS").First.ID
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

    ''' <summary>
    ''' Lista de Indicadores que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private WithEvents _ListaEncabezado As List(Of TransferenciaPagosMasivos)
    Public Property ListaEncabezado() As List(Of TransferenciaPagosMasivos)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As List(Of TransferenciaPagosMasivos))
            _ListaEncabezadoPaginada = Nothing
            _ListaEncabezado = value
            CalcularCantidadRegistros()

            MyBase.CambioItem("ListaEncabezado")
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

    Private _ConsecutivoSeleccionado As String
    Public Property ConsecutivoSeleccionado() As String
        Get
            Return _ConsecutivoSeleccionado
        End Get
        Set(ByVal value As String)
            _ConsecutivoSeleccionado = value
            BancoSeleccionado = Nothing
            NombreBancoSeleccionado = String.Empty

            If Not IsNothing(_ConsecutivoSeleccionado) Then
                HabilitarSeleccionArchivo = True
                If DiccionarioCarga.ContainsKey("CONSECUTIVOEGRESOS") Then
                    For Each li In DiccionarioCarga("CONSECUTIVOEGRESOS")
                        If li.ID = _ConsecutivoSeleccionado Then
                            CompaniaConsecutivoSeleccionado = li.intID
                        End If
                    Next
                End If
            Else
                HabilitarSeleccionArchivo = False
            End If
            MyBase.CambioItem("ConsecutivoSeleccionado")
        End Set
    End Property

    Private _FechaDocumento As DateTime = Now.Date
    Public Property FechaDocumento() As DateTime
        Get
            Return _FechaDocumento
        End Get
        Set(ByVal value As DateTime)
            _FechaDocumento = value
            MyBase.CambioItem("FechaDocumento")
        End Set
    End Property

    Private _CompaniaConsecutivoSeleccionado As Nullable(Of Integer)
    Public Property CompaniaConsecutivoSeleccionado() As Nullable(Of Integer)
        Get
            Return _CompaniaConsecutivoSeleccionado
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _CompaniaConsecutivoSeleccionado = value
            MyBase.CambioItem("CompaniaConsecutivoSeleccionado")
        End Set
    End Property

    Private _BancoSeleccionado As Integer
    Public Property BancoSeleccionado() As Integer
        Get
            Return _BancoSeleccionado
        End Get
        Set(ByVal value As Integer)
            _BancoSeleccionado = value
            MyBase.CambioItem("BancoSeleccionado")
        End Set
    End Property

    Private _NombreBancoSeleccionado As String
    Public Property NombreBancoSeleccionado() As String
        Get
            Return _NombreBancoSeleccionado
        End Get
        Set(ByVal value As String)
            _NombreBancoSeleccionado = value
            MyBase.CambioItem("NombreBancoSeleccionado")
        End Set
    End Property

    Private _IDComitenteSeleccionado As String
    Public Property IDComitenteSeleccionado() As String
        Get
            Return _IDComitenteSeleccionado
        End Get
        Set(ByVal value As String)
            _IDComitenteSeleccionado = value
            MyBase.CambioItem("IDComitenteSeleccionado")
        End Set
    End Property

    Private _NombreComitenteSeleccionado As String
    Public Property NombreComitenteSeleccionado() As String
        Get
            Return _NombreComitenteSeleccionado
        End Get
        Set(ByVal value As String)
            _NombreComitenteSeleccionado = value
            MyBase.CambioItem("NombreComitenteSeleccionado")
        End Set
    End Property

    Private _TipoIDComitenteSeleccionado As String
    Public Property TipoIDComitenteSeleccionado() As String
        Get
            Return _TipoIDComitenteSeleccionado
        End Get
        Set(ByVal value As String)
            _TipoIDComitenteSeleccionado = value
            MyBase.CambioItem("TipoIDComitenteSeleccionado")
        End Set
    End Property

    Private _NroDocumentoComitenteSeleccionado As String
    Public Property NroDocumentoComitenteSeleccionado() As String
        Get
            Return _NroDocumentoComitenteSeleccionado
        End Get
        Set(ByVal value As String)
            _NroDocumentoComitenteSeleccionado = value
            MyBase.CambioItem("NroDocumentoComitenteSeleccionado")
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

    Private _NombreArchivo As String
    Public Property NombreArchivo() As String
        Get
            Return _NombreArchivo
        End Get
        Set(ByVal value As String)
            _NombreArchivo = value
            MyBase.CambioItem("NombreArchivo")
        End Set
    End Property

    Private _NroRegistros As Integer
    Public Property NroRegistros() As Integer
        Get
            Return _NroRegistros
        End Get
        Set(ByVal value As Integer)
            _NroRegistros = value
            MyBase.CambioItem("NroRegistros")
        End Set
    End Property

    Private _ValorTotal As Double
    Public Property ValorTotal() As Double
        Get
            Return _ValorTotal
        End Get
        Set(ByVal value As Double)
            _ValorTotal = value
            MyBase.CambioItem("ValorTotal")
        End Set
    End Property

    Private _Confirmaciones As String
    Public Property Confirmaciones() As String
        Get
            Return _Confirmaciones
        End Get
        Set(ByVal value As String)
            _Confirmaciones = value
            MyBase.CambioItem("Confirmaciones")
        End Set
    End Property

    Private _HabilitarSeleccionArchivo As Boolean = False
    Public Property HabilitarSeleccionArchivo() As Boolean
        Get
            Return _HabilitarSeleccionArchivo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarSeleccionArchivo = value
            MyBase.CambioItem("HabilitarSeleccionArchivo")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    Public Async Function ConsultarParametro(ByVal pstrParametro As String) As Task(Of String)
        Dim strValorParametro As String = String.Empty

        Try
            Dim objRet As InvokeOperation(Of String)

            objRet = Await mdcProxyUtilidad.VerificaparametroSync(pstrParametro, Program.Usuario, Program.HashConexion).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el valor.", Me.ToString(), "ConsultarParametro", Program.TituloSistema, Program.Maquina, objRet.Error)
                Else
                    strValorParametro = objRet.Value
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el valor.", Me.ToString(), "ConsultarParametro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return strValorParametro
    End Function

    Public Async Function VerificarRutaServidor(ByVal pstrRutaFisica As String) As Task(Of Boolean)
        Dim logExisteRutaFisica As Boolean = False

        Try
            Dim objRet As InvokeOperation(Of Boolean)

            objRet = Await dcProxyImportaciones.VerificarRutaFisicaServidorSync(pstrRutaFisica, Program.Usuario, Program.HashConexion).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al seleccionar y consultar los registros.", Me.ToString(), "VerificarRutaServidor", Program.TituloSistema, Program.Maquina, objRet.Error)
                Else
                    logExisteRutaFisica = objRet.Value
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el valor.", Me.ToString(), "VerificarRutaServidor", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return logExisteRutaFisica
    End Function

    Private Sub BorrarInformacion()
        Try
            ListaEncabezado = Nothing
            NombreArchivo = String.Empty
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar la información", Me.ToString(), "BorrarInformacion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub CalcularCantidadRegistros()
        Try
            NroRegistros = 0
            ValorTotal = 0

            If Not IsNothing(ListaEncabezado) Then
                For Each li In ListaEncabezado
                    NroRegistros += 1
                    ValorTotal += li.dblValorConvertido
                Next
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al calcular el valor total", Me.ToString(), "CalcularCantidadRegistros", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Ejecuta el proceso de generar documentos si el usuario presiona el botón Generar RC.
    ''' </summary>
    Public Async Sub Generar()
        Try
            If String.IsNullOrEmpty(ConsecutivoSeleccionado) Then
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensaje("Debe de seleccionar el consecutivo para realizar la generación.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If IsNothing(BancoSeleccionado) Or BancoSeleccionado = 0 Then
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensaje("Debe de seleccionar el banco para realizar la generación.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If String.IsNullOrEmpty(IDComitenteSeleccionado) Then
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensaje("Debe de seleccionar el cliente para realizar la generación.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If IsNothing(NroRegistros) Then
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensaje("No hay registros para generar.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If NroRegistros = 0 Then
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensaje("No hay registros para generar.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True
            Dim strRutaArchivo As String = String.Empty
            Dim strRutaBackup As String = String.Empty

            strRutaArchivo = Await ConsultarParametro(RUTAARCHIVO)
            strRutaBackup = Await ConsultarParametro(RUTAARCHIVO_BACKUP)

            If Await VerificarRutaServidor(strRutaArchivo) = False Then
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensaje("La ruta del parámetro " & RUTAARCHIVO & " no existe o no se tiene acceso. Por favor configure el parámetro por una ruta válida para realizar la generación de los archivos.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not String.IsNullOrEmpty(strRutaBackup) Then
                If Await VerificarRutaServidor(strRutaBackup) = False Then
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarMensaje("La ruta del parámetro " & RUTAARCHIVO_BACKUP & " no existe o no se tiene acceso. Por favor configure el parámetro por una ruta válida para realizar la generación de los archivos.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            End If

            IsBusy = False
            A2Utilidades.Mensajes.mostrarMensajePregunta("¿Está seguro de realizar la operación de Transferencia de Pagos Masiva?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf ConfirmarGenerar, False)

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al presionar el botón Generar", Me.ToString(), "Generar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para confirmar si continua el proceso si hay fechas sin valoración inferiores a la fecha de valoración.
    ''' </summary>
    Public Sub ConfirmarGenerar(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If Not CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                Exit Sub
            Else
                A2Utilidades.Mensajes.mostrarMensajePregunta("¿Está totalmente seguro de realizar la operación de Transferencia de Pagos Masiva?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf ConfirmarTotalmenteGenerar, False)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al preguntar si está seguro de generar los Recibos de Caja", _
                                                             Me.ToString(), "ConfirmarGenerar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConfirmarTotalmenteGenerar(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If Not CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                Exit Sub
            Else
                Confirmaciones = String.Empty

                IsBusy = True
                If Not IsNothing(mdcProxy.RespuestaProcesosGenericosConfirmacions) Then
                    mdcProxy.RespuestaProcesosGenericosConfirmacions.Clear()
                End If

                mdcProxy.Load(mdcProxy.TransferenciaPagosMasivos_ProcesarQuery(ConsecutivoSeleccionado, FechaDocumento, BancoSeleccionado, IDComitenteSeleccionado, NombreComitenteSeleccionado, TipoIDComitenteSeleccionado, NroDocumentoComitenteSeleccionado, Confirmaciones, Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoGenerarDocumentos, String.Empty)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar los documentos de tesorería.", _
                                                             Me.ToString(), "ConfirmarTotalmenteGenerar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub TerminoGenerarDocumentos(ByVal lo As LoadOperation(Of OyDTesoreria.RespuestaProcesosGenericosConfirmacion))
        Try
            If Not lo.HasError Then
                Dim objListaRespuesta As List(Of OyDTesoreria.RespuestaProcesosGenericosConfirmacion)
                Dim objListaMensajes As New List(Of String)
                Dim objViewImportarArchivo As New A2ComunesControl.PoPupConfirmarGeneracionProcesosMasivos()
                Dim logExportarArchivo As Boolean = False
                Dim intIDDocumento As Integer = 0

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    If objListaRespuesta.Where(Function(i) i.logDetieneIngreso).Count > 0 Then
                        objListaMensajes.Add("El sistema generó algunas inconsistencias al intentar generar los documentos:")

                        For Each li In objListaRespuesta.Where(Function(i) i.logDetieneIngreso)
                            objListaMensajes.Add(li.strMensaje)
                        Next

                        objViewImportarArchivo.ListaMensajes = objListaMensajes

                        objViewImportarArchivo.Title = "Transferencias pagos masivos"
                        'Program.Modal_OwnerMainWindowsPrincipal(objViewImportarArchivo)
                        'objViewImportarArchivo.ShowDialog()
                    ElseIf objListaRespuesta.Where(Function(i) i.logConfirmacion).Count > 0 Then
                        Dim strMensajeConfirmacion As String = String.Empty
                        Confirmaciones = String.Empty

                        For Each li In objListaRespuesta.Where(Function(i) i.logConfirmacion)
                            objListaMensajes.Add(li.strMensaje)
                            'If String.IsNullOrEmpty(strMensajeConfirmacion) Then
                            '    strMensajeConfirmacion = li.strMensaje
                            Confirmaciones = li.strConfirmacion
                            'Else
                            '    strMensajeConfirmacion = String.Format("{0}{1}{2}", strMensajeConfirmacion, vbCrLf, li.strMensaje)
                            '    Confirmaciones = String.Format("{0},{1}", Confirmaciones, li.strConfirmacion)
                            'End If
                        Next

                        objViewImportarArchivo.listaMensajes = objListaMensajes
                        AddHandler objViewImportarArchivo.Closed, AddressOf ConfirmarBaseDeDatos
                        Program.Modal_OwnerMainWindowsPrincipal(objViewImportarArchivo)
                        objViewImportarArchivo.ShowDialog()

                        'A2Utilidades.Mensajes.mostrarMensajePregunta(strMensajeConfirmacion, Program.TituloSistema, Confirmaciones,Program.Usuario, Program.HashConexion, AddressOf ConfirmarBaseDeDatos, False)
                    Else
                        objListaMensajes.Add("Se generaron los registros exitosamente:")

                        For Each li In objListaRespuesta.Where(Function(i) i.logExitoso)
                            If li.strConfirmacion = "IDDOCUMENTO" Then
                                intIDDocumento = li.intIDInsertado
                            End If
                            objListaMensajes.Add(li.strMensaje)
                        Next

                        objViewImportarArchivo.listaMensajes = objListaMensajes

                        logExportarArchivo = True

                        objViewImportarArchivo.Title = "Transferencias pagos masivos"
                        Program.Modal_OwnerMainWindowsPrincipal(objViewImportarArchivo)
                        objViewImportarArchivo.ShowDialog()
                    End If
                Else
                    objViewImportarArchivo.listaMensajes.Add("No se obtuvieron registros al procesar el archivo.")

                    objViewImportarArchivo.Title = "Transferencias pagos masivos"
                    Program.Modal_OwnerMainWindowsPrincipal(objViewImportarArchivo)
                    objViewImportarArchivo.ShowDialog()
                End If

                If logExportarArchivo Then
                    IsBusy = True
                    'objViewImportarArchivo.IsBusy = True

                    If Not IsNothing(dcProxyImportaciones.RespuestaArchivoImportacions) Then
                        dcProxyImportaciones.RespuestaArchivoImportacions.Clear()
                    End If

                    dcProxyImportaciones.Load(dcProxyImportaciones.TransferenciaPagosMasivos_ExportarQuery(intIDDocumento, ConsecutivoSeleccionado, "ImpTransPagosMasivos", Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoExportarArchivo, objViewImportarArchivo)
                Else
                    IsBusy = False
                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar los ajustes automaticos.", Me.ToString(), "TerminoGenerarDocumentos", Application.Current.ToString(), Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar los ajustes automaticos.", Me.ToString(), "TerminoGenerarDocumentos", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    ''' <summary>
    ''' Método para confirmar si continua el proceso si hay fechas sin valoración inferiores a la fecha de valoración.
    ''' </summary>
    Public Sub ConfirmarBaseDeDatos(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If Not CType(sender, A2ComunesControl.PoPupConfirmarGeneracionProcesosMasivos).DialogResult Then
                Confirmaciones = String.Empty
            Else
                IsBusy = True
                If Not IsNothing(mdcProxy.RespuestaProcesosGenericosConfirmacions) Then
                    mdcProxy.RespuestaProcesosGenericosConfirmacions.Clear()
                End If

                mdcProxy.Load(mdcProxy.TransferenciaPagosMasivos_ProcesarQuery(ConsecutivoSeleccionado, FechaDocumento, BancoSeleccionado, IDComitenteSeleccionado, NombreComitenteSeleccionado, TipoIDComitenteSeleccionado, NroDocumentoComitenteSeleccionado, Confirmaciones, Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoGenerarDocumentos, String.Empty)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al preguntar si está seguro de generar el comprobante de egreso",
                                                             Me.ToString(), "ConfirmarBaseDeDatos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub Consultar()
        Try
            IsBusy = True

            If Not IsNothing(mdcProxy.TransferenciaPagosMasivos) Then
                mdcProxy.TransferenciaPagosMasivos.Clear()
            End If

            mdcProxy.Load(mdcProxy.TransferenciaPagosMasivos_ConsultaQuery(Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoConsultarTransferenciaPagosMasivos, String.Empty)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al presionar el botón Consultar Documentos", Me.ToString(), "Consultar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoConsultarTransferenciaPagosMasivos(ByVal lo As LoadOperation(Of OyDTesoreria.TransferenciaPagosMasivos))
        Try
            If Not lo.HasError Then
                ListaEncabezado = lo.Entities.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar las transferencias.", Me.ToString(), "TerminoConsultarTransferenciaPagosMasivos", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar las transferencias.", Me.ToString(), "TerminoConsultarTransferenciaPagosMasivos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

    ''' <summary>
    ''' Método para capturar la respuesta del usuario para eliminar el portafolio para todos los clientes o uno en específico
    ''' </summary>
    Public Sub Importar()
        Try
            If String.IsNullOrEmpty(ConsecutivoSeleccionado) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe de seleccionar el consecutivo para realizar la importación.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True

            ViewImportarArchivo = New cwCargaArchivos(Me, NombreArchivo, "ImpTransPagosMasivos")
            ViewImportarArchivo.IsBusy = True
            Program.Modal_OwnerMainWindowsPrincipal(ViewImportarArchivo)
            ViewImportarArchivo.ShowDialog()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el resultado de validaciones.", Me.ToString(), "GenerarRC_Intermedia_Resultados", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub CargarArchivo(ByVal pstrModulo As String, ByVal _NombreArchivo As String)
        Try
            If Not IsNothing(dcProxyImportaciones.RespuestaArchivoImportacions) Then
                dcProxyImportaciones.RespuestaArchivoImportacions.Clear()
            End If
            dcProxyImportaciones.Load(dcProxyImportaciones.TransferenciaPagosMasivos_ImportarQuery(NombreArchivo, "ImpTransPagosMasivos", Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoCargarArchivo, String.Empty)
        Catch ex As Exception
            IsBusy = False
            ViewImportarArchivo.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el método CargarArchivo. ", Me.ToString(), "CargarArchivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoCargarArchivo(ByVal lo As LoadOperation(Of OyDImportaciones.RespuestaArchivoImportacion))
        Try
            If Not lo.HasError Then
                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacion)
                Dim objListaMensajes As New List(Of String)

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    If objListaRespuesta.Where(Function(i) i.Exitoso = False).Count > 0 Then

                        objListaMensajes.Add("El archivo generó algunas inconsistencias al intentar subirlo:")

                        For Each li In objListaRespuesta.Where(Function(i) i.Exitoso = False).OrderBy(Function(o) o.Tipo)
                            If li.Tipo = "C" Then
                                objListaMensajes.Add(li.Mensaje)
                            Else
                                objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: {2}", li.Fila, li.Columna, li.Mensaje))
                            End If
                        Next

                        objListaMensajes.Add("No se importaron registros debido a que se presentaron inconsistencias")

                        ViewImportarArchivo.ListaMensajes = objListaMensajes
                        IsBusy = False
                    Else
                        For Each li In objListaRespuesta.Where(Function(i) i.Exitoso)
                            objListaMensajes.Add(li.Mensaje)
                        Next
                        ViewImportarArchivo.ListaMensajes = objListaMensajes

                        Consultar()
                    End If
                Else
                    ViewImportarArchivo.ListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")
                    IsBusy = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar de generar el archivo. ", Me.ToString(), "TerminoCargarArchivo", Application.Current.ToString(), Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar de generar el archivo. ", Me.ToString(), "TerminoCargarArchivo", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try

        ViewImportarArchivo.IsBusy = False
    End Sub

    Public Sub TerminoExportarArchivo(ByVal lo As LoadOperation(Of OyDImportaciones.RespuestaArchivoImportacion))
        'Dim objViewImportarArchivo As A2ComunesControl.PoPupConfirmarGeneracionProcesosMasivos = CType(lo.UserState, A2ComunesControl.PoPupConfirmarGeneracionProcesosMasivos)
        Dim objViewImportarArchivo As New A2ComunesControl.PoPupConfirmarGeneracionProcesosMasivos() ''JCM20190925
        Try
            If Not lo.HasError Then
                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacion)
                Dim objListaMensajes As New List(Of String)
                objListaRespuesta = lo.Entities.ToList

                If IsNothing(objViewImportarArchivo.listaMensajes) Then
                    objViewImportarArchivo.listaMensajes = New List(Of String)
                Else
                    For Each li In objViewImportarArchivo.listaMensajes
                        objListaMensajes.Add(li)
                    Next
                End If

                If objListaRespuesta.Count > 0 Then
                    If objListaRespuesta.Where(Function(i) i.Exitoso = False).Count > 0 Then
                        objListaMensajes.Add("El sistema generó algunas inconsistencias al intentar exportar el archivo:")

                        For Each li In objListaRespuesta.Where(Function(i) i.Exitoso = False)
                            objListaMensajes.Add(li.Mensaje)
                        Next

                    Else
                        objListaMensajes.Add("Se generaron los registros exitosamente:")

                        For Each li In objListaRespuesta.Where(Function(i) i.Exitoso)
                            objListaMensajes.Add(li.Mensaje)
                        Next
                    End If
                Else
                    objListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")
                End If

                objViewImportarArchivo.listaMensajes = objListaMensajes

                objViewImportarArchivo.Title = "Transferencias pagos masivos"
                Program.Modal_OwnerMainWindowsPrincipal(objViewImportarArchivo)
                ' objViewImportarArchivo.Close()
                objViewImportarArchivo.ShowDialog()



                'objViewImportarArchivo.ShowDialog()

                BorrarInformacion()
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar las transferencia de pagos masivos.", Me.ToString(), "TerminoExportarArchivo", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar las transferencia de pagos masivos.", Me.ToString(), "TerminoExportarArchivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        'objViewImportarArchivo.IsBusy = False
        IsBusy = False
    End Sub

#End Region

End Class