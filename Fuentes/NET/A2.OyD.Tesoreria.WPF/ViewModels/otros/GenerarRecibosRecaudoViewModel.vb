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
Imports System.Text
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones

Public Class GenerarRecibosRecaudoViewModel
    Inherits A2ControlMenu.A2ViewModel

    Implements INotifyPropertyChanged

    ''' <summary>
    ''' ViewModel para la pantalla Generar Recibos Recaudo perteneciente al proyecto de Tesorería.
    ''' </summary>
    ''' <history>
    ''' Creado por       : Jeison Ramírez Pino (IoSoft S.A.)
    ''' Descripción      : Creacion. 
    ''' Fecha            : 02 de Julio/2016
    ''' Pruebas CB       : Jeison Ramírez Pino - 02 de Julio/2016 - Resultado Ok 
    ''' </history>

#Region "Constantes"
    Private Const RUTAARCHIVO As String = "RUTA_GENERAR_RECIBOS_CAJA_RECAUDO"

    Public Enum TipoOpcionCargar
        GENERAR
        TRASLADOS
        RESULTADO
    End Enum
#End Region

#Region "Variables"
    Public ViewGenerarRecibosRecaudos As GenerarRecibosRecaudoView = Nothing
    Private mdcProxy As TesoreriaDomainContext
    Public view As GenerarRecibosRecaudoView
    Private dcProxyConsulta As TesoreriaDomainContext
    Private dcProxyImportaciones As ImportacionesDomainContext
    Private mdcProxyUtilidad As UtilidadesDomainContext
#End Region

#Region "Inicialización"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                mdcProxy = New TesoreriaDomainContext()
                dcProxyConsulta = New TesoreriaDomainContext()
                dcProxyImportaciones = New ImportacionesDomainContext()
                mdcProxyUtilidad = New UtilidadesDomainContext()
            Else
                mdcProxy = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                dcProxyConsulta = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                dcProxyImportaciones = New ImportacionesDomainContext(New System.Uri((Program.RutaServicioImportaciones)))
                mdcProxyUtilidad = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            End If

            DirectCast(mdcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 15, 0)
            DirectCast(dcProxyConsulta.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 15, 0)
            DirectCast(dcProxyImportaciones.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 15, 0)
            DirectCast(mdcProxyUtilidad.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 15, 0)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "GenerarRecibosRecaudoViewModel", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        'IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando
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
    ''' Lista de Indicadores que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private WithEvents _ListaDetalle As List(Of TrasladoenLinea)
    Public Property ListaDetalle() As List(Of TrasladoenLinea)
        Get
            Return _ListaDetalle
        End Get
        Set(ByVal value As List(Of TrasladoenLinea))
            _ListaDetallePaginada = Nothing
            _ListaDetalle = value

            MyBase.CambioItem("ListaDetalle")
            MyBase.CambioItem("ListaDetallePaginada")
        End Set
    End Property

    Private WithEvents _TrasladoenLineaSelected As New OyDTesoreria.TrasladoenLinea
    Public Property TrasladoenLineaSelected() As OyDTesoreria.TrasladoenLinea
        Get
            Return _TrasladoenLineaSelected
        End Get
        Set(ByVal value As OyDTesoreria.TrasladoenLinea)
            _TrasladoenLineaSelected = value
            MyBase.CambioItem("TrasladoenLineaSelected")

            Try
                If Not IsNothing(_TrasladoenLineaSelected) Then
                    'MyBase.CambioItem("DiccionarioCombos")
                End If
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al realizar las consultas de los traslados en linea.", Me.ToString(), "TrasladoenLineaSelected", Application.Current.ToString(), Program.Maquina, ex)
                IsBusy = False
            End Try
        End Set
    End Property

    Private _ListaDetallePaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de Indicadores para navegar sobre el grid con paginación
    ''' </summary>
    Public ReadOnly Property ListaDetallePaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDetalle) Then
                If IsNothing(_ListaDetallePaginada) Then
                    Dim view = New PagedCollectionView(_ListaDetalle)
                    _ListaDetallePaginada = view
                    Return view
                Else
                    Return (_ListaDetallePaginada)
                End If
            Else
                Return Nothing
            End If
        End Get
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
            For Each Lista In ListaDetalle
                Lista.logSeleccionado = True
            Next
            MyBase.CambioItem("logSeleccionartodos")
        End Set
    End Property

    Private _strResultados As New StringBuilder
    Public Property Resultados() As StringBuilder
        Get
            Return _strResultados
        End Get
        Set(ByVal value As StringBuilder)
            _strResultados = value
            MyBase.CambioItem("Resultados")
        End Set
    End Property

    Private _listaCargasArchivos As List(Of CargasArchivo)
    Public Property ListaCargasArchivos() As List(Of CargasArchivo)
        Get
            Return _listaCargasArchivos
        End Get
        Set(ByVal value As List(Of CargasArchivo))
            _listaCargasArchivos = value
            MyBase.CambioItem("ListaCargasArchivos")
            CargasArchivoSeleccionado = _listaCargasArchivos.FirstOrDefault
        End Set
    End Property

    Private _cargasArchivoSeleccionado As CargasArchivo
    Public Property CargasArchivoSeleccionado() As CargasArchivo
        Get
            Return _cargasArchivoSeleccionado
        End Get
        Set(ByVal value As CargasArchivo)
            If Not IsNothing(value) Then
                _cargasArchivoSeleccionado = value
                MyBase.CambioItem("CargasArchivoSeleccionado")
            End If
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

    Private _strExportarRC As String = String.Empty
    Public Property strExportarRC() As String
        Get
            Return _strExportarRC
        End Get
        Set(ByVal value As String)
            _strExportarRC = value
            MyBase.CambioItem("strExportarRC")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    Public Sub Ayuda()
        Try
            A2Utilidades.Mensajes.mostrarMensaje("Formato de archivo (Cuenta,Fecha,Valor,Codigo_Transaccion,Referencia)", Program.TituloSistema, wppMensajes.TiposMensaje.Exito)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al presionar el botón Generar", Me.ToString(), "Generar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub Procesar()
        Try
            Dim objNewTrasladosenLineaView As New TrasladosenLineaView(Me)
            Program.Modal_OwnerMainWindowsPrincipal(objNewTrasladosenLineaView)
            objNewTrasladosenLineaView.ShowDialog()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al presionar el botón de Procesar Traslado en linea.", Me.ToString(), "Procesar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarTraslados()
        Try
            IsBusy = True
            If Not IsNothing(dcProxyConsulta.TrasladoenLineas) Then
                dcProxyConsulta.TrasladoenLineas.Clear()
            End If

            dcProxyConsulta.Load(dcProxyConsulta.RCTrasladosEnLinea_ConsultarQuery(Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoConsultarTraslados, "")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al presionar el botón de Procesar Traslado en linea.", Me.ToString(), "ConsultarTraslados", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Async Sub Exportar()
        Try
            IsBusy = True

            Dim strRutaArchivo As String = String.Empty
            strRutaArchivo = Await ConsultarParametro(RUTAARCHIVO)

            If Await VerificarRutaServidor(strRutaArchivo) = False Then
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensaje("La ruta del parámetro " & RUTAARCHIVO & " no existe o no se tiene acceso. Por favor configure el parámetro por una ruta válida para realizar la generación de los archivos.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            dcProxyImportaciones.RespuestaArchivoImportacions.Clear()
            dcProxyImportaciones.Load(dcProxyImportaciones.RecibosRecaudo_ExportarQuery("ImpRecibosRecaudo", Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminaConsultarResultados, Nothing)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al presionar el botón de Exportar", Me.ToString(), "Exportar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

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

    Public Sub Aceptar()
        Try
            IsBusy = True

            If String.IsNullOrEmpty(NombreArchivo) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe de seleccionar un archivo para procesar.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            If Not IsNothing(dcProxyConsulta.RespuestaProcesosGenericosConfirmacions) Then
                dcProxyConsulta.RespuestaProcesosGenericosConfirmacions.Clear()
            End If

            dcProxyConsulta.Load(dcProxyConsulta.RCRecibosRecaudos_ValidarQuery(Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoValidarRecibosRecaudo, String.Empty)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al presionar el botón de Aceptar", Me.ToString(), "Aceptar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConfirmarGenerar(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If Not CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                Exit Sub
            Else
                IsBusy = True

                If Not IsNothing(dcProxyImportaciones.RespuestaArchivoImportacions) Then
                    dcProxyImportaciones.RespuestaArchivoImportacions.Clear()
                End If

                dcProxyImportaciones.Load(dcProxyImportaciones.RecibosRecaudo_ProcesarQuery(NombreArchivo, "ImpRecibosRecaudo", Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoGenerarDocumentos, String.Empty)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al preguntar si está seguro de generar los Recibos de Caja", _
                                                             Me.ToString(), "ConfirmarGenerar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Metodo AceptarTraslado: Se encarga de Ingresar la información de la pantalla de traslados en linea a la BD.
    ''' </summary>
    ''' <remarks>IOSoft 2016</remarks>
    Public Sub AceptarTraslado()
        Try
            Dim strMsg As String = String.Empty
            Dim pxml_RC As String = String.Empty

            If IsNothing(ListaDetalle) Then
                Mensajes.mostrarMensaje(String.Format("{0}{1}  No existe un registro seleccionado en la pantalla para trasladar.", strMsg, vbCrLf), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If ListaDetalle.Where(Function(i) i.logSeleccionado = True).Count = 0 Then
                Mensajes.mostrarMensaje(String.Format("{0}{1} No existe un registro seleccionado en la pantalla para trasladar.", strMsg, vbCrLf), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True

            If Not IsNothing(ListaDetalle) Then
                For Each Lista In ListaDetalle
                    If Lista.logSeleccionado Then
                        If String.IsNullOrEmpty(pxml_RC) Then
                            pxml_RC = Lista.intIdentity & "|"
                        Else
                            pxml_RC = "=" & Lista.intIdentity & "|"
                        End If

                        'pxml_RC = pxml_RC & pxml_RC
                    End If
                Next
            End If

            If Not IsNothing(dcProxyConsulta.RespuestaProcesosGenericosConfirmacions) Then
                dcProxyConsulta.RespuestaProcesosGenericosConfirmacions.Clear()
            End If

            dcProxyConsulta.Load(dcProxyConsulta.RCTrasladosEnLinea_IngresarQuery(pxml_RC, Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoIngresarTraslados, "")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al presionar el botón de Aceptar de la pantalla de Traslado en Linea", Me.ToString(), "AceptarTraslado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    ''' <summary>
    ''' Metodo EliminarTraslado: Se encarga de eliminar la información de la pantalla de traslados en linea.
    ''' </summary>
    ''' <remarks>IOSoft 2016</remarks>
    Public Sub EliminarTraslado()
        Try
            Dim pxml_RC As String = String.Empty
            Dim strMsg As String = String.Empty

            If IsNothing(ListaDetalle) Then
                Mensajes.mostrarMensaje(String.Format("{0}{1} No existe un registro seleccionado en la pantalla para eliminar.", strMsg, vbCrLf), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If ListaDetalle.Where(Function(i) i.logSeleccionado = True).Count = 0 Then
                Mensajes.mostrarMensaje(String.Format("{0}{1} No existe un registro seleccionado en la pantalla para eliminar.", strMsg, vbCrLf), Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True

            If Not IsNothing(ListaDetalle) Then
                For Each Lista In ListaDetalle
                    If Lista.logSeleccionado Then
                        If String.IsNullOrEmpty(pxml_RC) Then
                            pxml_RC = Lista.intIdentity & "|"
                        Else
                            pxml_RC = "=" & Lista.intIdentity & "|"
                        End If

                        'pxml_RC = pxml_RC & pxml_RC
                    End If
                Next

            End If

            If Not IsNothing(dcProxyConsulta.RespuestaProcesosGenericosConfirmacions) Then
                dcProxyConsulta.RespuestaProcesosGenericosConfirmacions.Clear()
            End If

            dcProxyConsulta.Load(dcProxyConsulta.RCTrasladosEnLinea_EliminarQuery(pxml_RC, Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoEliminarTraslados, "")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al presionar el botón de Eliminar de la pantalla de Traslado en Linea", Me.ToString(), "EliminarTraslado", Application.Current.ToString(), Program.Maquina, ex)
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

    Private Sub TerminaConsultarResultados(lo As LoadOperation(Of RespuestaArchivoImportacion))
        Try
            If lo.HasError = False Then
                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacion)
                Dim objListaMensajes As New List(Of String)
                Dim objViewImportarArchivo As New A2ComunesControl.ResultadoGenericoImportaciones()

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    For Each li In objListaRespuesta
                        objListaMensajes.Add(li.Mensaje)
                    Next

                    objViewImportarArchivo.ListaMensajes = objListaMensajes
                Else
                    objViewImportarArchivo.ListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")
                End If

                objViewImportarArchivo.Title = "Recibos recaudo"
                Program.Modal_OwnerMainWindowsPrincipal(objViewImportarArchivo)
                objViewImportarArchivo.ShowDialog()
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ejecutar la acción", _
                                 Me.ToString(), "ImportacionDecevalViewModel.TerminaCargaDatosDeceval", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoEliminarTraslados(ByVal lo As LoadOperation(Of OyDTesoreria.RespuestaProcesosGenericosConfirmacion))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    If lo.Entities.First.logExitoso Then
                        IsBusy = True
                        If Not IsNothing(dcProxyConsulta.TrasladoenLineas) Then
                            dcProxyConsulta.TrasladoenLineas.Clear()
                        End If

                        dcProxyConsulta.Load(dcProxyConsulta.RCTrasladosEnLinea_ConsultarQuery(Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoConsultarTraslados, "")
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje(lo.Entities.First.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("El sistema no retorno información por favor verifique con el administrador.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al eliminar los traslados.", Me.ToString(), "TerminoEliminarTraslados", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
            IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al eliminar los traslados.", Me.ToString(), "TerminoEliminarTraslados", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoConsultarTraslados(ByVal lo As LoadOperation(Of OyDTesoreria.TrasladoenLinea))
        Try
            If lo.HasError = False Then
                ListaDetalle = lo.Entities.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar los traslados.", Me.ToString(), "TerminoConsultarTraslados", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
            IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar los traslados.", Me.ToString(), "TerminoConsultarTraslados", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoIngresarTraslados(ByVal lo As LoadOperation(Of OyDTesoreria.RespuestaProcesosGenericosConfirmacion))
        Try
            If lo.HasError = False Then
                If lo.Entities.First.logExitoso Then
                    IsBusy = True
                    If Not IsNothing(dcProxyConsulta.TrasladoenLineas) Then
                        dcProxyConsulta.TrasladoenLineas.Clear()
                    End If

                    dcProxyConsulta.Load(dcProxyConsulta.RCTrasladosEnLinea_ConsultarQuery(Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoConsultarTraslados, "")
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(lo.Entities.First.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al ingresar los traslados.", Me.ToString(), "TerminoIngresarTraslados", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            End If
            IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al ingresar los traslados.", Me.ToString(), "TerminoIngresarTraslados", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Sub TerminoValidarRecibosRecaudo(ByVal lo As LoadOperation(Of OyDTesoreria.RespuestaProcesosGenericosConfirmacion))
        Try
            If Not lo.HasError Then
                Dim objListaRespuesta As List(Of OyDTesoreria.RespuestaProcesosGenericosConfirmacion)

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    If objListaRespuesta.Where(Function(i) i.logDetieneIngreso).Count > 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje(objListaRespuesta.First.strMensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                    ElseIf objListaRespuesta.Where(Function(i) i.logConfirmacion).Count > 0 Then
                        A2Utilidades.Mensajes.mostrarMensajePregunta(objListaRespuesta.First.strMensaje, Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf ConfirmarGenerar, False)
                        IsBusy = False
                    Else
                        IsBusy = True
                        If Not IsNothing(dcProxyImportaciones.RespuestaArchivoImportacions) Then
                            dcProxyImportaciones.RespuestaArchivoImportacions.Clear()
                        End If

                        dcProxyImportaciones.Load(dcProxyImportaciones.RecibosRecaudo_ProcesarQuery(NombreArchivo, "ImpRecibosRecaudo", Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoGenerarDocumentos, String.Empty)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("No se puede continuar con el proceso ya que no se retorno ninguna información.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
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

    Public Sub TerminoGenerarDocumentos(ByVal lo As LoadOperation(Of OyDImportaciones.RespuestaArchivoImportacion))
        Try
            If Not lo.HasError Then
                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacion)
                Dim objListaMensajes As New List(Of String)
                Dim objViewImportarArchivo As New A2ComunesControl.ResultadoGenericoImportaciones()

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    If objListaRespuesta.Where(Function(i) i.Exitoso = False).Count > 0 Then

                        objListaMensajes.Add("El sistema generó algunas inconsistencias al intentar generar los documentos:")

                        For Each li In objListaRespuesta.Where(Function(i) i.Exitoso = False)
                            objListaMensajes.Add(li.Mensaje)
                        Next

                        objViewImportarArchivo.ListaMensajes = objListaMensajes
                    Else
                        objListaMensajes.Add("Se generaron los registros exitosamente:")

                        For Each li In objListaRespuesta.Where(Function(i) i.Exitoso)
                            objListaMensajes.Add(li.Mensaje)
                        Next

                        objViewImportarArchivo.ListaMensajes = objListaMensajes
                    End If
                Else
                    objViewImportarArchivo.ListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")
                End If

                objViewImportarArchivo.Title = "Generar recibos recaudo"
                Program.Modal_OwnerMainWindowsPrincipal(objViewImportarArchivo)
                objViewImportarArchivo.ShowDialog()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar los ajustes automaticos.", Me.ToString(), "TerminoGenerarDocumentos", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar los ajustes automaticos.", Me.ToString(), "TerminoGenerarDocumentos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

#End Region

#Region "Métodos privados del encabezado"
    Private Function ValidarDatos() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            'Valida la fecha de valoración
            If Not IsNothing(ListaDetalle) Then
                If ListaDetalle.Count = 0 Then
                    strMsg = String.Format("{0}{1} + No nay Comprobantes de egresos para generar.", strMsg, vbCrLf)
                End If
            End If

            If Not IsNothing(ListaDetalle) Then
                If ListaDetalle.Count = 0 Then
                    strMsg = String.Format("{0}{1} + No nay Comprobantes de egresos para generar.", strMsg, vbCrLf)
                End If
            End If

            If Not IsNothing(ListaDetalle) Then
                If ListaDetalle.Count = 0 Then
                    strMsg = String.Format("{0}{1} + No nay Comprobantes de egresos para generar.", strMsg, vbCrLf)
                End If
            End If

            'If IsNothing(intSucursal) Or intSucursal = 0 Then
            '    strMsg = String.Format("{0}{1} + La sucursal es un campo requerido.", strMsg, vbCrLf)
            'End If

            If IsNothing(ListaDetalle) Then
                strMsg = String.Format("{0}{1} + Debe consultar primero los documentos para poder generar.", strMsg, vbCrLf)
            ElseIf ListaDetalle.Count = 0 Then
                strMsg = String.Format("{0}{1} + Debe consultar primero los documentos para poder generar.", strMsg, vbCrLf)
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

End Class