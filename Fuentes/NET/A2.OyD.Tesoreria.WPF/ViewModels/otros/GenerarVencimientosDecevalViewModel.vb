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
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones


Public Class GenerarVencimientosDecevalViewModel
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

    Public ViewGenerarVencimientosDeceval As GenerarVencimientosDecevalView = Nothing

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As TesoreriaDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private mdcProxyUtilidad As UtilidadesDomainContext
    Private mdcProxyImportaciones As ImportacionesDomainContext

#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        Try
            IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando

            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                mdcProxy = New TesoreriaDomainContext()
                mdcProxyUtilidad = New UtilidadesDomainContext()
                mdcProxyImportaciones = New ImportacionesDomainContext()
            Else
                mdcProxy = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                mdcProxyUtilidad = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                mdcProxyImportaciones = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
            End If

            DirectCast(mdcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 300, 0)
            DirectCast(mdcProxyUtilidad.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 300, 0)
            DirectCast(mdcProxyImportaciones.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 300, 0)

            IsBusy = False
        Catch ex As Exception
            IsBusy = False
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

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del objeto de negocio.", Me.ToString(), "inicializar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logResultado)

    End Function

#End Region

#Region "Propiedades del Encabezado - REQUERIDO"

    Private _strRuta As String
    Public Property strRuta() As String
        Get
            Return _strRuta
        End Get
        Set(ByVal value As String)
            _strRuta = value
            MyBase.CambioItem("strRuta")
        End Set
    End Property

    Private _strExtensionesPermitidas As String = "Archivo de Texto|*.txt"
    Public Property strExtensionesPermitidas() As String
        Get
            Return _strExtensionesPermitidas
        End Get
        Set(ByVal value As String)
            _strExtensionesPermitidas = value
            MyBase.CambioItem("strExtensionesPermitidas")
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

#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    Public Sub CargarArchivo(pstrModulo As String, pstrNombreCompletoArchivo As String)
        Try
            IsBusy = True

            strRuta = pstrNombreCompletoArchivo

            If Not IsNothing(mdcProxyImportaciones.RespuestaArchivoImportacions) Then
                mdcProxyImportaciones.RespuestaArchivoImportacions.Clear()
            End If

            mdcProxyImportaciones.Load(mdcProxyImportaciones.GenerarVencimientosDeceval_ImportarQuery(pstrNombreCompletoArchivo, "GenerarVencimientosDeceval", Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoCargarArchivo, String.Empty)

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al importar el archivo.", _
                               Me.ToString(), "CargarArchivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoCargarArchivo(ByVal lo As LoadOperation(Of OyDImportaciones.RespuestaArchivoImportacion))
        Try
            If Not lo.HasError Then
                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacion)
                Dim objListaMensajes As New List(Of String)

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    If objListaRespuesta.Where(Function(i) CType(i.Exitoso, Boolean) = False).Count > 0 Then

                        'objListaMensajes.Add("El archivo generó algunas inconsistencias al intentar subirlo:")

                        For Each li In objListaRespuesta.Where(Function(i) CType(i.Exitoso, Boolean) = False).OrderBy(Function(o) o.Tipo)
                            If li.Tipo = "RESULTADOS" Then
                                objListaMensajes.Add(li.Mensaje)
                            Else
                                objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: {2}", li.Fila, li.Columna, li.Mensaje))
                            End If
                        Next

                        'objListaMensajes.Add("No se importaron registros debido a que se presentaron inconsistencias")

                        ViewImportarArchivo.ListaMensajes = objListaMensajes
                    End If
                Else
                    ViewImportarArchivo.ListaMensajes = objListaMensajes
                End If

                IsBusy = False

            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al terminar de generar el archivo. ", Me.ToString(), "TerminoCargarArchivo", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar de generar el archivo. ", Me.ToString(), "TerminoCargarArchivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ExportarDatos()
        Try
            IsBusy = True

            mdcProxyImportaciones.RespuestaArchivoImportacions.Clear()
            mdcProxyImportaciones.Load(mdcProxyImportaciones.GenerarVencimientosDeceval_ExportarQuery("GenerarVencimientosDeceval_Exportar", Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoGenerarExcel, String.Empty)

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al preguntar si está seguro de realizar las tranferencias por pagos por red banco", _
                                                             Me.ToString(), "ConfirmarAceptar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoGenerarExcel(lo As LoadOperation(Of RespuestaArchivoImportacion))
        Try
            If Not lo.HasError Then
                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacion)
                Dim strUrlArchivo As String = String.Empty

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    For Each li In objListaRespuesta
                        If String.IsNullOrEmpty(strUrlArchivo) Then
                            strUrlArchivo = li.URLArchivo
                        End If
                    Next
                End If

                If Not String.IsNullOrEmpty(strUrlArchivo) Then
                    Dim strNroVentana As String = Date.Now.Hour.ToString & Date.Now.Minute.ToString & Date.Now.Second.ToString & Date.Now.Millisecond.ToString

                    Program.VisorArchivosWeb_DescargarURL(strUrlArchivo)
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar el excel.", Me.ToString(), "TerminoGenerarExcel", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar el excel.", Me.ToString(), "TerminoGenerarExcel", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
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
            'If IsNothing(intSucursal) Or intSucursal = 0 Then
            '    strMsg = String.Format("{0}{1} + La sucursal es un campo requerido.", strMsg, vbCrLf)
            'End If

            'If IsNothing(ListaDetalle) Then
            '    strMsg = String.Format("{0}{1} + Debe consultar primero los documentos para poder generar.", strMsg, vbCrLf)
            'ElseIf ListaDetalle.Count = 0 Then
            '    strMsg = String.Format("{0}{1} + Debe consultar primero los documentos para poder generar.", strMsg, vbCrLf)
            'ElseIf ListaDetalle.Where(Function(i) i.logGenerar).Count = 0 Then
            '    strMsg = String.Format("{0}{1} + No nay Recibos de caja marcados para generar.", strMsg, vbCrLf)
            'End If

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

