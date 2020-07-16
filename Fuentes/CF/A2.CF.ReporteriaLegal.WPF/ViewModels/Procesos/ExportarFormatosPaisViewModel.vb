Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades
Imports A2.OyD.OYDServer.RIA.Web.CFReporteriaLegal
Imports A2Utilidades.Mensajes
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Web
Imports OpenRiaServices.DomainServices.Client
Imports A2.OyD.OYDServer.RIA.Web.CFUtilidades
Imports System.Collections.ObjectModel

Public Class ExportarFormatosPaisViewModel
    Inherits A2ControlMenu.A2ViewModel

#Region "Constantes"

    Private Const STR_PARAMETROS_EXPORTAR As String = "[FECHAPROCESO]=[[FECHAPROCESO]]"
    Private Const STR_CARPETA As String = "FORMATOSGENERADOS"

    Private Enum PARAMETROSEXPORTAR
        FECHAPROCESO
    End Enum

#End Region

#Region "Variables - REQUERIDO"
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con los domain services
    '---------------------------------------------------------------------------------------------------------------------------------------------------

    Private dcProxy As ReporteriaLegalDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private dcProxyUtilidades As UtilidadesDomainContext ' Comunicación con el web service del data context de utilidades. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private logConsultarEntidadesControl As Boolean = True
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
    ''' <history>
    ''' Creado por       : Juan Carlos Soto Cruz.
    ''' Descripción      : Creacion.
    ''' Fecha            : Marzo 31/2014
    ''' Pruebas CB       : Juan Carlos Soto Cruz - Marzo 31/2014 - Resultado Ok 
    ''' </history>
    Public Async Function inicializar() As Task(Of Boolean)

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then

                '' Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                'consultarEncabezadoPorDefectoSync()

                '' Consultar los registros del encabezado incluyendo el Await para que el llamado sea sincrónico
                'Await ConsultarEncabezado(True, String.Empty)
                Await ConsultarValoresDefecto()
                Await ConsultarCombosPantalla()
                CargaListas() 'JEPM20160405

                Await ConsultarConfiguracionReportes_EntidadesControl(CodigoPais, False)
                Await ConsultarConfiguracionReportes()


            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            IsBusy = False
        End Try

        Return (logResultado)

    End Function

#End Region

#Region "Metodos - REQUERIDO"

    ''' <history>
    ''' Modificado por:     Germán Arbey González (Alcuadrado S.A.)
    ''' Descripción:        Se realiza el llamado al metodo ObtenerNombreFormatoSync para cambiar el nombre el archivo a exportar
    ''' Fecha:              Agosto 19/2014
    ''' </history>
    ''' <history>
    ''' Modificado por:     Javier Eduardo Pardo Moreno (Alcuadrado S.A.)
    ''' Descripción:        Se realiza el llamado al metodo GenerarArchivoReporteriaLegalSyncQuery para generar plano, excel o ambos y retornar un dataset
    ''' Fecha:              Marzo 28/2016
    ''' Id del cambio:      JEPM20160328
    ''' </history>
    Public Async Sub ExportarArchivo()
        Try
            IsBusy = True

            If ValidarExportarArchivo() Then
                Dim objRet As LoadOperation(Of GenerarArchivosReporteriaLegal)
                Dim strParametros As String = STR_PARAMETROS_EXPORTAR

                dcProxyUtilidades = inicializarProxyUtilidadesOYD()

                If Not IsNothing(FechaExportacion) Then
                    strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.FECHAPROCESO), FechaExportacion.ToString("yyyyMMdd"))
                Else
                    strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.FECHAPROCESO), String.Empty)
                End If

                If dcProxy Is Nothing Then
                    dcProxy = inicializarProxyReporteriaLegal()
                    DirectCast(dcProxy.DomainClient, WebDomainClient(Of ReporteriaLegalDomainContext.IReporteriaLegalDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 60, 0)
                End If

                Dim strListaArchivosExporacion As String
                Dim objRetName As InvokeOperation(Of String)

                objRetName = Await dcProxy.ObtenerNombreFormatoSync(_NombreCircular, FechaExportacion.ToString("yyyyMMdd"), Program.Usuario, Program.HashConexion).AsTask()

                If objRetName.Value = "" Or objRetName Is Nothing Then
                    strListaArchivosExporacion = _NombreCircular
                Else
                    strListaArchivosExporacion = objRetName.Value
                End If

                Dim strNombreFormatosCadena As String = String.Empty

                For Each objConfiguracionReportes As ConfiguracionReportes In ListaFormatosFiltrada
                    If objConfiguracionReportes.logSeleccionado = True Then
                        strNombreFormatosCadena = strNombreFormatosCadena + objConfiguracionReportes.strNombreFormato + ","
                    End If
                Next

                'Quitar la última coma
                strNombreFormatosCadena = strNombreFormatosCadena.Substring(0, strNombreFormatosCadena.Length() - 1)

                Dim strMsg As String = String.Empty

                'Inicio JEPM20160328
                Dim objProxyUtil As UtilidadesCFDomainContext

                objProxyUtil = inicializarProxyUtilidades()

                objRet = Await objProxyUtil.Load(objProxyUtil.GenerarArchivoReporteriaLegalSyncQuery(STR_CARPETA, strNombreFormatosCadena, FechaExportacion.ToString("yyyyMMdd"), strListaArchivosExporacion, strTipoPortafolio, String.Empty, _ExtensionArchivo, Program.Maquina, Program.Usuario, A2ComunesControl.FuncionesCompartidas.TimeUpProxy, Program.HashConexion, _NombreCircular)).AsTask

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        If objRet.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la exportación del archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la exportación del archivo.", Me.ToString(), "ExportarArchivo", Program.TituloSistema, Program.Maquina, objRet.Error)
                        End If
                        IsBusy = False
                        objRet.MarkErrorAsHandled()
                    Else
                        If objRet.Entities.Count > 0 Then

                            For Each objEntity As GenerarArchivosReporteriaLegal In objRet.Entities
                                If objEntity.Exitoso Then
                                    Program.VisorArchivosWeb_DescargarURL(objEntity.RutaArchivo)
                                Else
                                    mostrarMensaje(objEntity.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                End If
                            Next

                        End If

                        IsBusy = False

                    End If
                End If
                'Fin JEPM20160328

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la exportación del archivo.", "ExportarArchivo", Program.TituloSistema, Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try

    End Sub

    Private Function ValidarExportarArchivo() As Boolean
        Try
            Dim objResultado As Boolean = True
            Dim strMensaje As String = String.Empty

            If String.IsNullOrEmpty(_FechaExportacion.ToString()) Then
                strMensaje = strMensaje & vbCrLf & "  + Debe seleccionar la fecha"
            End If

            If ListaFormatosFiltrada.Where(Function(i) CBool(i.logSeleccionado) = True).Count = 0 Then
                strMensaje = strMensaje & vbCrLf & "  + Debe seleccionar el formato a generar"
            End If

            If String.IsNullOrEmpty(ExtensionArchivo) Then
                strMensaje = strMensaje & vbCrLf & "  + Debe seleccionar la extensión"
            End If

            If Not String.IsNullOrEmpty(strMensaje) Then
                strMensaje = "No es posible realizar el proceso de exportación porque tiene las siguientes inconsistencias: " & vbCrLf & strMensaje
                mostrarMensajeResultadoAsincronico(strMensaje, Program.TituloSistema, AddressOf TerminoMostrarMensajeUsuario, "ValidarExportarArchivo", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                objResultado = False
                IsBusy = False
            End If

            Return objResultado
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el archivo.", "ValidarExportarArchivo", Program.TituloSistema, Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            Return False
        End Try
    End Function

    Private Function ValorAReemplazar(ByVal pintTipoCampo As PARAMETROSEXPORTAR) As String
        Return String.Format("[[{0}]]", pintTipoCampo.ToString)
    End Function

    Public Sub LlenarValoresPorDefecto()
        Try
            ExtensionArchivo = String.Empty
            NombreArchivo = String.Empty
            FechaExportacion = DateTime.Now
            NombreCircular = String.Empty
            strTipoPortafolio = String.Empty
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un al relizar el proceso de asignación de los valores por defecto.", "LlenarValoresPorDefecto", Program.TituloSistema, Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub TerminoMostrarMensajeUsuario(ByVal sender As Object, ByVal e As EventArgs)
        Try

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir la respuesta del mensaje usuario.", Me.ToString(), "TerminoMostrarMensajeUsuario", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Async Function ConsultarCombosPantalla() As Task(Of Boolean)
        Try
            Dim objRestultado As LoadOperation(Of tblCombosReporteria)

            If dcProxy Is Nothing Then
                dcProxy = inicializarProxyReporteriaLegal()
                DirectCast(dcProxy.DomainClient, WebDomainClient(Of ReporteriaLegalDomainContext.IReporteriaLegalDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 60, 0)
            End If

            dcProxy.tblCombosReporterias.Clear()

            objRestultado = Await dcProxy.Load(dcProxy.ConsultarConfiguracionReportes_ConsultarCombosQuery(Program.Usuario, Program.HashConexion)).AsTask

            If Not objRestultado Is Nothing Then
                If objRestultado.HasError Then
                    If objRestultado.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "ConsultarEncabezado", Program.TituloSistema, Program.Maquina, objRestultado.Error)
                    End If

                    objRestultado.MarkErrorAsHandled()
                Else
                    ListaCombosCompleto = dcProxy.tblCombosReporterias.ToList
                End If
            End If

            Return True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la configuración de reportes.", Me.ToString(), "ConsultarConfiguracionReportes", Application.Current.ToString(), Program.Maquina, ex)
            Return False
        End Try
    End Function

    Private Async Function ConsultarValoresDefecto() As Task(Of Boolean)
        Try
            Dim objRestultado As LoadOperation(Of tblValoresDefecto)

            If dcProxy Is Nothing Then
                dcProxy = inicializarProxyReporteriaLegal()
                DirectCast(dcProxy.DomainClient, WebDomainClient(Of ReporteriaLegalDomainContext.IReporteriaLegalDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 60, 0)
            End If

            dcProxy.tblValoresDefectos.Clear()

            objRestultado = Await dcProxy.Load(dcProxy.ConsultarConfiguracionReportes_ValoresDefectoQuery(Program.Usuario, Program.HashConexion)).AsTask

            If Not objRestultado Is Nothing Then
                If objRestultado.HasError Then
                    If objRestultado.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "ConsultarEncabezado", Program.TituloSistema, Program.Maquina, objRestultado.Error)
                    End If

                    objRestultado.MarkErrorAsHandled()
                Else
                    ListaValoresDefecto = dcProxy.tblValoresDefectos.ToList

                    logConsultarEntidadesControl = False
                    If ListaValoresDefecto.Where(Function(i) i.strCodigoDefecto = "PAIS").Count > 0 Then
                        CodigoPais = ListaValoresDefecto.Where(Function(i) i.strCodigoDefecto = "PAIS").First.strValorDefecto
                        DescripcionPais = ListaValoresDefecto.Where(Function(i) i.strCodigoDefecto = "PAIS").First.strDescripcion
                    End If
                    logConsultarEntidadesControl = True
                End If
            End If

            MyBase.CambioItem("ListaConfiguracionReportes")
            Return True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la configuración de reportes.", Me.ToString(), "ConsultarConfiguracionReportes", Application.Current.ToString(), Program.Maquina, ex)
            Return False
        End Try
    End Function

    Private Async Function ConsultarConfiguracionReportes() As Task(Of Boolean)
        Try
            Dim objRestultado As LoadOperation(Of ConfiguracionReportes)

            If dcProxy Is Nothing Then
                dcProxy = inicializarProxyReporteriaLegal()
                DirectCast(dcProxy.DomainClient, WebDomainClient(Of ReporteriaLegalDomainContext.IReporteriaLegalDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 60, 0)
            End If

            dcProxy.ConfiguracionReportes.Clear()

            objRestultado = Await dcProxy.Load(dcProxy.ConsultarConfiguracionReportesSyncQuery(Program.Usuario, Program.HashConexion)).AsTask

            If Not objRestultado Is Nothing Then
                If objRestultado.HasError Then
                    If objRestultado.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "ConsultarEncabezado", Program.TituloSistema, Program.Maquina, objRestultado.Error)
                    End If

                    objRestultado.MarkErrorAsHandled()
                Else
                    ListaConfiguracionReportes = dcProxy.ConfiguracionReportes

                    If Not IsNothing(ListaEntidadesControl) Then
                        If ListaEntidadesControl.Count = 1 Then
                            EntidadControl = ListaEntidadesControl.First.strEntidadControl
                        Else
                            If ListaValoresDefecto.Where(Function(i) i.strCodigoDefecto = "ENTIDADCONTROL").Count > 0 Then
                                EntidadControl = ListaValoresDefecto.Where(Function(i) i.strCodigoDefecto = "ENTIDADCONTROL").First.strValorDefecto
                            Else
                                EntidadControl = "SFC" 'Se quema el valor SuperIntendencia de Colombia, mientras se definen nuevas listas
                            End If
                        End If
                    End If

                    If objRestultado.Entities.Count = 0 Then
                        ' Solamente se presenta el mensaje cuando se ejecuta la opción de filtrar con un filtro específico o la opción de buscar (consultar) 
                        A2Utilidades.Mensajes.mostrarMensaje("No se encontraron datos relacionados a la configuración de reportes.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            Else
                ListaConfiguracionReportes.Clear()
            End If

            MyBase.CambioItem("ListaConfiguracionReportes")
            Return True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la configuración de reportes.", Me.ToString(), "ConsultarConfiguracionReportes", Application.Current.ToString(), Program.Maquina, ex)
            Return False
        End Try
    End Function

    Private Async Sub ConsultarEntidadControl()
        Try
            If Not String.IsNullOrEmpty(CodigoPais) Then
                Await ConsultarConfiguracionReportes_EntidadesControl(CodigoPais)
            Else
                ListaEntidadesControl = Nothing
                EntidadControl = String.Empty
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la configuración de reportes.", Me.ToString(), "ConsultarEntidadControl", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Async Function ConsultarConfiguracionReportes_EntidadesControl(ByVal pstrCodigoPais As String, Optional ByVal plogVerificarDefecto As Boolean = True) As Task(Of Boolean)
        Try

            Dim objRestultado As LoadOperation(Of tblEntidadControl)

            If dcProxy Is Nothing Then
                dcProxy = inicializarProxyReporteriaLegal()
                DirectCast(dcProxy.DomainClient, WebDomainClient(Of ReporteriaLegalDomainContext.IReporteriaLegalDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 60, 0)
            End If

            dcProxy.tblEntidadControls.Clear()

            objRestultado = Await dcProxy.Load(dcProxy.ConsultarConfiguracionReportes_EntidadesControlQuery(pstrCodigoPais, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRestultado Is Nothing Then
                If objRestultado.HasError Then
                    If objRestultado.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados pero no se recibió detalle de la falla generada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta con los criterios de búsqueda indicados.", Me.ToString(), "ConsultarEncabezado", Program.TituloSistema, Program.Maquina, objRestultado.Error)
                    End If

                    objRestultado.MarkErrorAsHandled()
                    ListaEntidadesControl = Nothing
                Else
                    ListaEntidadesControl = dcProxy.tblEntidadControls.ToList

                    If plogVerificarDefecto Then
                        If Not IsNothing(ListaEntidadesControl) Then
                            If ListaEntidadesControl.Count = 1 Then
                                EntidadControl = ListaEntidadesControl.First.strEntidadControl
                            Else
                                If ListaValoresDefecto.Where(Function(i) i.strCodigoDefecto = "ENTIDADCONTROL").Count > 0 Then
                                    EntidadControl = ListaValoresDefecto.Where(Function(i) i.strCodigoDefecto = "ENTIDADCONTROL").First.strValorDefecto
                                Else
                                    EntidadControl = "SFC" 'Se quema el valor SuperIntendencia de Colombia, mientras se definen nuevas listas
                                End If
                            End If
                        End If
                    End If
                End If
            Else
                ListaEntidadesControl = Nothing
            End If

            Return True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar la configuración de las entidades de control.", Me.ToString(), "ConsultarConfiguracionReportes_EntidadesControl", Application.Current.ToString(), Program.Maquina, ex)
            Return False
        End Try
    End Function

    Private Sub CargaListas()
        Try
            If Not IsNothing(ListaCombosCompleto) Then
                Me.ListaCirculares = ListaCombosCompleto.Where(Function(i) i.strTopico = "CIRCULAREXPORTAR").ToList
                Me.ListaTipoCompania = ListaCombosCompleto.Where(Function(i) i.strTopico = "TIPOCOMPANIA").ToList

                If ListaValoresDefecto.Where(Function(i) i.strCodigoDefecto = "TIPOCOMPANIA").Count > 0 Then
                    Me.strTipoPortafolio = ListaValoresDefecto.Where(Function(i) i.strCodigoDefecto = "TIPOCOMPANIA").First.strValorDefecto
                Else
                    Me.strTipoPortafolio = "TODAS"
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las listas.", Me.ToString(), "CargaListas", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

#Region "Propiedades - REQUERIDO"

    Private _ListaConfiguracionReportes As EntitySet(Of ConfiguracionReportes)
    Public Property ListaConfiguracionReportes() As EntitySet(Of ConfiguracionReportes)
        Get
            Return _ListaConfiguracionReportes
        End Get
        Set(ByVal value As EntitySet(Of ConfiguracionReportes))
            _ListaConfiguracionReportes = value

            MyBase.CambioItem("ListaConfiguracionReportes")
        End Set
    End Property

    Private _ListaCirculares As List(Of tblCombosReporteria)
    Public Property ListaCirculares() As List(Of tblCombosReporteria)
        Get
            Return _ListaCirculares
        End Get
        Set(ByVal value As List(Of tblCombosReporteria))
            _ListaCirculares = value
            MyBase.CambioItem("ListaCirculares")
        End Set
    End Property

    Private _ListaCircularesFiltrada As List(Of tblCombosReporteria)
    Public Property ListaCircularesFiltrada() As List(Of tblCombosReporteria)
        Get
            Return _ListaCircularesFiltrada
        End Get
        Set(ByVal value As List(Of tblCombosReporteria))
            _ListaCircularesFiltrada = value
            MyBase.CambioItem("ListaCircularesFiltrada")
        End Set
    End Property

    Private _ListaFormatosFiltrada As List(Of ConfiguracionReportes)
    Public Property ListaFormatosFiltrada() As List(Of ConfiguracionReportes)
        Get
            Return _ListaFormatosFiltrada
        End Get
        Set(ByVal value As List(Of ConfiguracionReportes))
            _ListaFormatosFiltrada = value
            MyBase.CambioItem("ListaFormatosFiltrada")
        End Set
    End Property

    Private _EntidadControl As String = String.Empty
    Public Property EntidadControl() As String
        Get
            Return _EntidadControl
        End Get
        Set(ByVal value As String)
            'JEPM20160420
            If Not IsNothing(ListaConfiguracionReportes) And Not IsNothing(ListaCirculares) Then
                'Filtrar la lista de circulares según la lista de ConfiguracionReportes
                If ListaConfiguracionReportes.Where(Function(i) i.strEntidadControl = value).Count > 0 Then
                    ListaCircularesFiltrada = (From c In ListaCirculares Where (From o In ListaConfiguracionReportes Where o.strEntidadControl = value Select o.strCircularExportar).Contains(c.strCodigo)).ToList

                    If ListaCircularesFiltrada.Count > 0 Then
                        NombreCircular = ListaCircularesFiltrada.FirstOrDefault.strCodigo
                    End If
                Else
                    ListaCircularesFiltrada = Nothing
                    NombreCircular = String.Empty
                End If
            Else
                ListaCircularesFiltrada = Nothing
                NombreCircular = String.Empty
            End If
            _EntidadControl = value
            MyBase.CambioItem("EntidadControl")
        End Set
    End Property

    Private _NombreCircular As String = String.Empty
    Public Property NombreCircular() As String
        Get
            Return _NombreCircular
        End Get
        Set(ByVal value As String)

            If Not IsNothing(ListaConfiguracionReportes) Then
                'Filtrar la lista de ConfiguracionReportes
                ListaFormatosFiltrada = ListaConfiguracionReportes.Where(Function(i) i.strCircularExportar = value).ToList
                ExtensionArchivo = "PLANO" 'JEPM20160331
            End If

            _NombreCircular = value
            MyBase.CambioItem("NombreCircular")
        End Set
    End Property

    Private _NombreArchivo As String = String.Empty
    Public Property NombreArchivo() As String
        Get
            Return _NombreArchivo
        End Get
        Set(ByVal value As String)
            _NombreArchivo = value
            MyBase.CambioItem("NombreArchivo")
        End Set
    End Property

    Private _ExtensionArchivo As String = String.Empty
    Public Property ExtensionArchivo() As String
        Get
            Return _ExtensionArchivo
        End Get
        Set(ByVal value As String)

            'si la extensión es plano se debe bloquear la selección de formatos y marcarlos todos
            If Not IsNothing(ListaFormatosFiltrada) Then

                If value = "PLANO" Then
                    HabilitarFormatos = False
                    For Each objConfiguracionReportes As ConfiguracionReportes In ListaFormatosFiltrada
                        objConfiguracionReportes.logSeleccionado = True
                    Next

                    HabilitarTipoPortafolio = False
                    If ListaValoresDefecto.Where(Function(i) i.strCodigoDefecto = "TIPOCOMPANIA").Count > 0 Then
                        Me.strTipoPortafolio = ListaValoresDefecto.Where(Function(i) i.strCodigoDefecto = "TIPOCOMPANIA").First.strValorDefecto
                    Else
                        Me.strTipoPortafolio = "TODAS"
                    End If
                    logSeleccionarTodos = True
                Else
                    HabilitarFormatos = True
                    HabilitarTipoPortafolio = True
                End If

            End If

            _ExtensionArchivo = value
            MyBase.CambioItem("ExtensionArchivo")
        End Set
    End Property

    Private _FechaExportacion As DateTime = Now
    Public Property FechaExportacion() As DateTime
        Get
            Return _FechaExportacion
        End Get
        Set(ByVal value As DateTime)
            _FechaExportacion = value
            MyBase.CambioItem("FechaExportacion")
        End Set
    End Property

    Private _HabilitarFormatos As Boolean
    Public Property HabilitarFormatos() As Boolean
        Get
            Return _HabilitarFormatos
        End Get
        Set(ByVal value As Boolean)
            _HabilitarFormatos = value
            MyBase.CambioItem("HabilitarFormatos")
        End Set
    End Property

    ''' <summary>
    ''' Creado por:     Javier Eduardo Pardo Moreno
    ''' Descripción:    Propiedad para cargar la lista del tópico TIPOCOMPANIA y agregarle el item 'Todos' sin modificar datos, sino únicamente en esta pantalla
    ''' Fecha:          Abril 05/2016
    ''' ID del cambio:  JEPM20160405
    ''' </summary>
    ''' <remarks></remarks>
    Private _ListaTipoCompania As List(Of tblCombosReporteria)
    Public Property ListaTipoCompania As List(Of tblCombosReporteria)
        Get
            Return _ListaTipoCompania
        End Get
        Set(ByVal value As List(Of tblCombosReporteria))
            _ListaTipoCompania = value
            MyBase.CambioItem("ListaTipoCompania")
        End Set
    End Property

    Private _strTipoPortafolio As String = String.Empty
    Public Property strTipoPortafolio() As String
        Get
            Return _strTipoPortafolio
        End Get
        Set(ByVal value As String)
            _strTipoPortafolio = value
            MyBase.CambioItem("strTipoPortafolio")
        End Set
    End Property

    Private _HabilitarTipoPortafolio As Boolean = True
    Public Property HabilitarTipoPortafolio() As Boolean
        Get
            Return _HabilitarTipoPortafolio
        End Get
        Set(ByVal value As Boolean)
            _HabilitarTipoPortafolio = value
            MyBase.CambioItem("HabilitarTipoPortafolio")
        End Set
    End Property

    Private _logSeleccionarTodos As Boolean = True
    Public Property logSeleccionarTodos() As Boolean
        Get
            Return _logSeleccionarTodos
        End Get
        Set(ByVal value As Boolean)
            _logSeleccionarTodos = value

            For Each objConfiguracionReportes As ConfiguracionReportes In ListaFormatosFiltrada
                objConfiguracionReportes.logSeleccionado = value
            Next

        End Set
    End Property

    Private _ListaEntidadesControl As List(Of tblEntidadControl)
    Public Property ListaEntidadesControl() As List(Of tblEntidadControl)
        Get
            Return _ListaEntidadesControl
        End Get
        Set(ByVal value As List(Of tblEntidadControl))
            _ListaEntidadesControl = value
            MyBase.CambioItem("ListaEntidadesControl")
        End Set
    End Property

    Private _DescripcionPais As String
    Public Property DescripcionPais() As String
        Get
            Return _DescripcionPais
        End Get
        Set(ByVal value As String)
            _DescripcionPais = value
            MyBase.CambioItem("DescripcionPais")
        End Set
    End Property

    Private _CodigoPais As String
    Public Property CodigoPais() As String
        Get
            Return _CodigoPais
        End Get
        Set(ByVal value As String)
            _CodigoPais = value
            If logConsultarEntidadesControl Then
                ConsultarEntidadControl()
            End If
            MyBase.CambioItem("CodigoPais")
        End Set
    End Property

    Private _ListaValoresDefecto As List(Of tblValoresDefecto)
    Public Property ListaValoresDefecto() As List(Of tblValoresDefecto)
        Get
            Return _ListaValoresDefecto
        End Get
        Set(ByVal value As List(Of tblValoresDefecto))
            _ListaValoresDefecto = value
            MyBase.CambioItem("ListaValoresDefecto")
        End Set
    End Property

    Private _ListaCombosCompleto As List(Of tblCombosReporteria)
    Public Property ListaCombosCompleto() As List(Of tblCombosReporteria)
        Get
            Return _ListaCombosCompleto
        End Get
        Set(ByVal value As List(Of tblCombosReporteria))
            _ListaCombosCompleto = value
            MyBase.CambioItem("ListaCombosCompleto")
        End Set
    End Property

#End Region

End Class
