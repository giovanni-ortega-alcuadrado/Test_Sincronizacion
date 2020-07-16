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

Public Class TransferenciasCarterasColectivasViewModel
    Inherits A2ControlMenu.A2ViewModel

    Implements INotifyPropertyChanged

#Region "Constantes"
    Private Const RUTAARCHIVO As String = "RUTA_CONSECUTIVO_TRANSFERENCIAS_CC"

#End Region

#Region "Variables - REQUERIDO"

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As TesoreriaDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private dcProxyImportaciones As ImportacionesDomainContext
    Private mdcProxyUtilidad As UtilidadesDomainContext

    Private strRutaArchivoExporta As String
    Private strNombreArchivoExportar As String

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

            DirectCast(mdcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 15, 0)
            DirectCast(dcProxyImportaciones.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 15, 0)
            DirectCast(mdcProxyUtilidad.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 15, 0)

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
                mdcProxyUtilidad.Load(mdcProxyUtilidad.cargarCombosEspecificosQuery("TransferenciaCarterasColectivas", Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombosEspecificos, "")
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

                    If DiccionarioCarga.ContainsKey("TIPOCONSECUTIVO") Then
                        If DiccionarioCarga("TIPOCONSECUTIVO").Count > 0 Then
                            ConsecutivoSeleccionado = DiccionarioCarga("TIPOCONSECUTIVO").First.ID
                        End If
                    End If

                    FechaProceso = Now.Date

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
    Private WithEvents _ListaEncabezado As List(Of TransferenciasCarterasColectivas)
    Public Property ListaEncabezado() As List(Of TransferenciasCarterasColectivas)
        Get
            Return _ListaEncabezado
        End Get
        Set(ByVal value As List(Of TransferenciasCarterasColectivas))
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
            BorrarInformacion()
            Consultar()
            MyBase.CambioItem("ConsecutivoSeleccionado")
        End Set
    End Property

    Private _FechaProceso As DateTime
    Public Property FechaProceso() As DateTime
        Get
            Return _FechaProceso
        End Get
        Set(ByVal value As DateTime)
            _FechaProceso = value
            If Not IsNothing(_FechaProceso) Then
                strFechaProceso = _FechaProceso.ToString("yyyy-MM-dd")
                Consultar()
            End If
            MyBase.CambioItem("FechaProceso")
        End Set
    End Property

    Private _strFechaProceso As String
    Public Property strFechaProceso() As String
        Get
            Return _strFechaProceso
        End Get
        Set(ByVal value As String)
            _strFechaProceso = value
            MyBase.CambioItem("strFechaProceso")
        End Set
    End Property

    Private _CuentaBancaria As String
    Public Property CuentaBancaria() As String
        Get
            Return _CuentaBancaria
        End Get
        Set(ByVal value As String)
            _CuentaBancaria = value
            Consultar()
            MyBase.CambioItem("CuentaBancaria")
        End Set
    End Property

    Private _NombreCuentaBancaria As String
    Public Property NombreCuentaBancaria() As String
        Get
            Return _NombreCuentaBancaria
        End Get
        Set(ByVal value As String)
            _NombreCuentaBancaria = value
            MyBase.CambioItem("NombreCuentaBancaria")
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
            CuentaBancaria = String.Empty
            NombreCuentaBancaria = String.Empty
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
                    ValorTotal += li.ValorAdicion
                Next
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al calcular el valor total", Me.ToString(), "CalcularCantidadRegistros", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub Consultar()
        Try
            If Not String.IsNullOrEmpty(ConsecutivoSeleccionado) And Not String.IsNullOrEmpty(CuentaBancaria) Then
                IsBusy = True

                If Not IsNothing(mdcProxy.TransferenciasCarterasColectivas) Then
                    mdcProxy.TransferenciasCarterasColectivas.Clear()
                End If

                mdcProxy.Load(mdcProxy.TransferenciasCarterasColectivas_ConsultarQuery(FechaProceso, ConsecutivoSeleccionado, CuentaBancaria, Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoConsultarTransferenciasCarterasColectivas, String.Empty)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al presionar el botón Consultar Documentos", Me.ToString(), "Consultar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoConsultarTransferenciasCarterasColectivas(ByVal lo As LoadOperation(Of OyDTesoreria.TransferenciasCarterasColectivas))
        Try
            If Not lo.HasError Then
                ListaEncabezado = lo.Entities.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar las transferencias.", Me.ToString(), "TerminoConsultarTransferenciasCarterasColectivas", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar las transferencias.", Me.ToString(), "TerminoConsultarTransferenciasCarterasColectivas", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

    ''' <summary>
    ''' Ejecuta el proceso de generar documentos si el usuario presiona el botón Generar RC.
    ''' </summary>
    Public Async Sub Generar()
        Try
            If String.IsNullOrEmpty(ConsecutivoSeleccionado) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe de seleccionar el consecutivo para realizar la generación.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If String.IsNullOrEmpty(CuentaBancaria) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe de seleccionar la cuenta bancaria.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If IsNothing(NroRegistros) Then
                A2Utilidades.Mensajes.mostrarMensaje("No hay registros para generar.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If NroRegistros = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("No hay registros para generar.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True

            strRutaArchivoExporta = Await ConsultarParametro(RUTAARCHIVO)

            If Await VerificarRutaServidor(strRutaArchivoExporta) = False Then
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensaje("La ruta del parámetro " & RUTAARCHIVO & " no existe o no se tiene acceso. Por favor configure el parámetro por una ruta válida para realizar la generación de los archivos.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = False
            A2Utilidades.Mensajes.mostrarMensajePregunta("¿Está seguro de realizar la generación del archivo plano?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf ConfirmarGenerar, False)

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
                A2Utilidades.Mensajes.mostrarMensajePregunta("¿Está totalmente seguro de realizar la generación del archivo plano?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf ConfirmarTotalmenteGenerar, False)
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

                mdcProxy.Load(mdcProxy.TransferenciasCarterasColectivas_ValidarQuery(FechaProceso, ConsecutivoSeleccionado, CuentaBancaria, Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoValidarDocumento, String.Empty)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar los documentos de tesorería.", _
                                                             Me.ToString(), "ConfirmarTotalmenteGenerar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub TerminoValidarDocumento(ByVal lo As LoadOperation(Of OyDTesoreria.RespuestaProcesosGenericosConfirmacion))
        Try
            If Not lo.HasError Then
                Dim objListaRespuesta As List(Of OyDTesoreria.RespuestaProcesosGenericosConfirmacion)
                Dim objListaMensajes As New List(Of String)
                Dim objViewImportarArchivo As New A2ComunesControl.ResultadoGenericoImportaciones()

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    If objListaRespuesta.Where(Function(i) i.logDetieneIngreso).Count > 0 Then
                        objListaMensajes.Add("El sistema generó algunas inconsistencias al intentar generar los documentos:")

                        For Each li In objListaRespuesta.Where(Function(i) i.logDetieneIngreso)
                            objListaMensajes.Add(li.strMensaje)
                        Next

                        objViewImportarArchivo.ListaMensajes = objListaMensajes

                        objViewImportarArchivo.Title = "Transferencias pagos masivos"
                        Program.Modal_OwnerMainWindowsPrincipal(objViewImportarArchivo)
                        objViewImportarArchivo.ShowDialog()
                        IsBusy = False
                    Else
                        Dim intConsecutivo As Integer = 0
                        Dim RetornoOperacion As String = String.Empty

                        For Each li In objListaRespuesta
                            If li.strConfirmacion = "CONSECUTIVO" Then
                                intConsecutivo = li.strMensaje
                            End If
                            If li.strConfirmacion = "OPERACION" Then
                                RetornoOperacion = li.strMensaje
                            End If
                        Next

                        strNombreArchivoExportar = RetornoOperacion & intConsecutivo.ToString & FechaProceso.ToString("yymmdd")

                        If Not IsNothing(dcProxyImportaciones.RespuestaArchivoImportacions) Then
                            dcProxyImportaciones.RespuestaArchivoImportacions.Clear()
                        End If
                        dcProxyImportaciones.Load(dcProxyImportaciones.TransferenciasCarterasColectivas_ExportarQuery(strRutaArchivoExporta, strNombreArchivoExportar, FechaProceso, ConsecutivoSeleccionado, CuentaBancaria, "TransferenciaCC", Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoExportarArchivo, String.Empty)
                    End If
                Else
                    objViewImportarArchivo.ListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")

                    objViewImportarArchivo.Title = "Transferencias carteras colectivas"
                    Program.Modal_OwnerMainWindowsPrincipal(objViewImportarArchivo)
                    objViewImportarArchivo.ShowDialog()
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

    Public Sub TerminoExportarArchivo(ByVal lo As LoadOperation(Of OyDImportaciones.RespuestaArchivoImportacion))
        Try
            If Not lo.HasError Then
                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacion)
                Dim objListaMensajes As New List(Of String)
                objListaRespuesta = lo.Entities.ToList

                Dim objViewImportarArchivo As New A2ComunesControl.ResultadoGenericoImportaciones

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

                objViewImportarArchivo.ListaMensajes = objListaMensajes

                objViewImportarArchivo.Title = "Transferencias carteras colectivas"
                Program.Modal_OwnerMainWindowsPrincipal(objViewImportarArchivo)
                objViewImportarArchivo.ShowDialog()

                BorrarInformacion()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar las transferencia de pagos masivos.", Me.ToString(), "TerminoExportarArchivo", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar las transferencia de pagos masivos.", Me.ToString(), "TerminoExportarArchivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

#End Region

End Class