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

Public Class GenerarAuditoriasdeRecaudoViewModel
    Inherits A2ControlMenu.A2ViewModel

    Implements INotifyPropertyChanged

    ''' <summary>
    ''' ViewModel para la pantalla Generar Auditorias de Recaudo perteneciente al proyecto de Tesorería.
    ''' </summary>
    ''' <history>
    ''' Creado por       : Natalia Andrea Otalvaro Castrillon (IoSoft S.A.)
    ''' Descripción      : Creacion. 
    ''' Fecha            : 03 de Agosto/2016
    ''' Pruebas CB       : Natalia Andrea Otalvaro - 03 de Agosto/2016 - Resultado Ok 
    ''' </history>

#Region "Constantes"

#End Region

#Region "Variables"
    Public ViewGenerarAuditoriasdeRecaudo As GenerarAuditoriasdeRecaudoView = Nothing
    Private mdcProxy As TesoreriaDomainContext
    Public view As GenerarAuditoriasdeRecaudoView
    Private mdcProxyUtilidad As UtilidadesDomainContext
    Private dcProxyImportaciones As ImportacionesDomainContext

    Dim strRutaArchivo_AuditoriasRecaudo As String = String.Empty

#End Region

#Region "Inicialización"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        Try
            IsBusy = True

            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                mdcProxy = New TesoreriaDomainContext()
                dcProxyImportaciones = New ImportacionesDomainContext()
                mdcProxyUtilidad = New UtilidadesDomainContext()
            Else
                mdcProxy = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                dcProxyImportaciones = New ImportacionesDomainContext(New System.Uri((Program.RutaServicioImportaciones)))
                mdcProxyUtilidad = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            End If

            DirectCast(mdcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 15, 0)
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
                mdcProxyUtilidad.Load(mdcProxyUtilidad.cargarCombosEspecificosQuery("GenerarAuditoriasRecaudo", Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombosEspecificos, "")
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

                    If dicListaCombos.ContainsKey("RUTA_GENERAR_AUDITORIAS_RECAUDO") Then
                        strRutaArchivo_AuditoriasRecaudo = dicListaCombos("RUTA_GENERAR_AUDITORIAS_RECAUDO").First.Descripcion
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

    Private _NombreArchivoTotales As String
    Public Property NombreArchivoTotales() As String
        Get
            Return _NombreArchivoTotales
        End Get
        Set(ByVal value As String)
            _NombreArchivoTotales = value
            MyBase.CambioItem("NombreArchivoTotales")
        End Set
    End Property

    Private _NombreArchivoSaldos As String
    Public Property NombreArchivoSaldos() As String
        Get
            Return _NombreArchivoSaldos
        End Get
        Set(ByVal value As String)
            _NombreArchivoSaldos = value
            MyBase.CambioItem("NombreArchivoSaldos")
        End Set
    End Property

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    Public Sub ExportarTotales()
        Try
            If String.IsNullOrEmpty(NombreArchivoTotales) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe de seleccionar el archivo de RC y CE Totales recaudo.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True
            dcProxyImportaciones.RespuestaArchivoImportacions.Clear()
            dcProxyImportaciones.Load(dcProxyImportaciones.GenerarAuditoriaRecaudos_GenerarTotalesQuery(NombreArchivoTotales, strRutaArchivo_AuditoriasRecaudo, "tmpRCyCETotalRecaudo", "ImpRecibosRecaudoTotales", Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminaGenerarArchivoTotales, Nothing)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al presionar el botón de Exportar", Me.ToString(), "ExportarTotales", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub Aceptar()
        Try
            If String.IsNullOrEmpty(NombreArchivoSaldos) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe de seleccionar el archivo de saldos de bancos.", Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True
            mdcProxy.TrasladoenLineas.Clear()
            mdcProxy.Load(mdcProxy.RCTrasladosEnLinea_ConsultarQuery(Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoConsultarTrasladosEnLinea, String.Empty)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al presionar el botón de Aceptar", Me.ToString(), "Aceptar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoConsultarTrasladosEnLinea(ByVal lo As LoadOperation(Of TrasladoenLinea))
        Try
            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    A2Utilidades.Mensajes.mostrarMensajePregunta("Existen movimientos pendientes de traslados en línea sin generarles recibos de caja .  ¿Desea continuar?", Program.TituloSistema, "EXISTESTRASLADOS", AddressOf ConfirmarGenerar, False)
                    IsBusy = False
                Else
                    IsBusy = True
                    dcProxyImportaciones.RespuestaArchivoImportacions.Clear()
                    dcProxyImportaciones.Load(dcProxyImportaciones.GenerarAuditoriaRecaudos_GenerarSaldosBancosQuery(NombreArchivoSaldos, "ImpRecibosRecaudoSaldos", Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminaGenerarArchivoSaldos, Nothing)
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los traslados.", Me.ToString(), "TerminoConsultarTrasladosEnLinea", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los traslados.", Me.ToString(), "TerminoConsultarTrasladosEnLinea", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConfirmarGenerar(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If Not CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                IsBusy = False
                Exit Sub
            Else
                IsBusy = True
                dcProxyImportaciones.RespuestaArchivoImportacions.Clear()
                dcProxyImportaciones.Load(dcProxyImportaciones.GenerarAuditoriaRecaudos_GenerarSaldosBancosQuery(NombreArchivoSaldos, "ImpRecibosRecaudoSaldos", Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminaGenerarArchivoSaldos, Nothing)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al preguntar si está seguro de generar los Recibos de Caja", _
                                                             Me.ToString(), "ConfirmarGenerar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminaGenerarArchivoTotales(lo As LoadOperation(Of RespuestaArchivoImportacion))
        Try
            If Not lo.HasError Then
                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacion)
                Dim objListaMensajes As New List(Of String)
                Dim objViewImportarArchivo As New A2ComunesControl.ResultadoGenericoImportaciones()
                Dim strURLArchivo As String = String.Empty

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    For Each li In objListaRespuesta
                        objListaMensajes.Add(li.Mensaje)
                        If li.Tipo = "ARCHIVO" Then
                            strURLArchivo = li.URLArchivo
                        End If
                    Next

                    If Not String.IsNullOrEmpty(strURLArchivo) Then
                        Dim strNroVentana As String = Date.Now.Hour.ToString & Date.Now.Minute.ToString & Date.Now.Second.ToString & Date.Now.Millisecond.ToString
                        Program.VisorArchivosWeb_DescargarURL(strURLArchivo)
                    End If
                    objViewImportarArchivo.ListaMensajes = objListaMensajes
                Else
                    objViewImportarArchivo.ListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")
                End If

                objViewImportarArchivo.Title = "Generar recibos recaudo"
                Program.Modal_OwnerMainWindowsPrincipal(objViewImportarArchivo)
                objViewImportarArchivo.ShowDialog()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar la auditoría ajustes automaticos.", Me.ToString(), "TerminaGenerarArchivoTotales", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar la auditoría ajustes automaticos.", Me.ToString(), "TerminaGenerarArchivoTotales", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

    Private Sub TerminaGenerarArchivoSaldos(lo As LoadOperation(Of RespuestaArchivoImportacion))
        Try
            If Not lo.HasError Then
                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacion)
                Dim objListaMensajes As New List(Of String)
                Dim objViewImportarArchivo As New A2ComunesControl.ResultadoGenericoImportaciones()
                Dim strURLArchivo As String = String.Empty

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    For Each li In objListaRespuesta
                        objListaMensajes.Add(li.Mensaje)
                        If li.Tipo = "ARCHIVO" Then
                            strURLArchivo = li.URLArchivo
                        End If
                    Next

                    If Not String.IsNullOrEmpty(strURLArchivo) Then
                        Dim strNroVentana As String = Date.Now.Hour.ToString & Date.Now.Minute.ToString & Date.Now.Second.ToString & Date.Now.Millisecond.ToString
                        Program.VisorArchivosWeb_DescargarURL(strURLArchivo)
                    End If
                    objViewImportarArchivo.ListaMensajes = objListaMensajes
                Else
                    objViewImportarArchivo.ListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")
                End If

                objViewImportarArchivo.Title = "Generar recibos recaudo"
                Program.Modal_OwnerMainWindowsPrincipal(objViewImportarArchivo)
                objViewImportarArchivo.ShowDialog()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar la auditoría ajustes automaticos.", Me.ToString(), "TerminaGenerarArchivoTotales", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar la auditoría ajustes automaticos.", Me.ToString(), "TerminaGenerarArchivoTotales", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

#End Region

End Class