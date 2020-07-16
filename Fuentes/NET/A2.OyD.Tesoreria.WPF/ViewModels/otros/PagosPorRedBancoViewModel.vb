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

Public Class PagosPorRedBancoViewModel
    Inherits A2ControlMenu.A2ViewModel

    Implements INotifyPropertyChanged

    ''' <summary>
    ''' ViewModel para la pantalla Pagos por red banco.
    ''' </summary>
    ''' <history>
    ''' Creado por       : Catalina Dávila (IoSoft S.A.)
    ''' Descripción      : Creacion. 
    ''' Fecha            : 9 de Agosto/2016
    ''' </history>

#Region "Constantes"

    Private Const RUTAARCHIVO As String = "PAGOSPORREDBANCO_RUTA"
    Private Const RUTAARCHIVO_BACKUP As String = "BACKUP_ARCHIVOSPAGOS_PAB"

#End Region

#Region "Variables - REQUERIDO"

    Public ViewPagosPorRedBanco As PagosPorRedBancoView = Nothing

    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As TesoreriaDomainContext ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private mdcProxyUtilidad As UtilidadesDomainContext
    Private dcProxyImportaciones As ImportacionesDomainContext
    Private STR_PAGOSPORREDBANCO_RUTA As String

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
                dcProxyImportaciones = New ImportacionesDomainContext
            Else
                mdcProxy = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                mdcProxyUtilidad = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                dcProxyImportaciones = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
            End If

            DirectCast(mdcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.TesoreriaDomainContext.ITesoreriaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 15, 0)
            DirectCast(mdcProxyUtilidad.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 15, 0)
            DirectCast(dcProxyImportaciones.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 15, 0)

            dtmFechaProceso = Date.Now.ToShortDateString
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

    ''' <summary>
    ''' Lista de detalles de la entidad en este caso detalle de codificacion de activos
    ''' </summary>
    Private _ListaDetalle As List(Of PagosPorRedBanco)
    Public Property ListaDetalle() As List(Of PagosPorRedBanco)
        Get
            Return _ListaDetalle
        End Get
        Set(ByVal value As List(Of PagosPorRedBanco))
            _ListaDetalle = value
            MyBase.CambioItem("ListaDetalle")
            MyBase.CambioItem("ListaDetallePaginada")
        End Set
    End Property

    Private _ListaDetallePaginada As PagedCollectionView = Nothing
    ''' <summary>
    ''' Colección que pagina la lista de Indicadores para navegar sobre el grid con paginación
    ''' </summary>
    Public ReadOnly Property ListaDetallePaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDetalle) Then
                 Dim view = New PagedCollectionView(_ListaDetalle)
                _ListaDetallePaginada = view
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _strNroCuenta As String
    Public Property strNroCuenta() As String
        Get
            Return _strNroCuenta
        End Get
        Set(ByVal value As String)
            _strNroCuenta = value
            MyBase.CambioItem("strNroCuenta")
        End Set
    End Property

    Private Async Sub _PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles Me.PropertyChanged

        If e.PropertyName = "strNroCuenta" And Not String.IsNullOrEmpty(strNroCuenta) Then
            Await ConsultarDetalle()
        ElseIf e.PropertyName = "dtmFechaProceso" Then
            strNroCuenta = String.Empty
            strNombreCuenta = String.Empty
            dblCantidad = 0
            dblTotal = 0
            ListaDetalle = Nothing
        End If

    End Sub

    Private _strNombreCuenta As String
    Public Property strNombreCuenta() As String
        Get
            Return _strNombreCuenta
        End Get
        Set(ByVal value As String)
            _strNombreCuenta = value
            MyBase.CambioItem("strNombreCuenta")
        End Set
    End Property

    ''' <summary>
    ''' Indica cuál de los detalles está seleccionado
    ''' </summary>
    Private WithEvents _DetalleSeleccionado As PagosPorRedBanco
    Public Property DetalleSeleccionado() As PagosPorRedBanco
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As PagosPorRedBanco)
            _DetalleSeleccionado = value

            MyBase.CambioItem("DetalleSeleccionado")
        End Set
    End Property

    Private _dtmFechaProceso As Nullable(Of System.DateTime)
    Public Property dtmFechaProceso() As Nullable(Of System.DateTime)
        Get
            Return _dtmFechaProceso
        End Get
        Set(ByVal value As Nullable(Of System.DateTime))
            _dtmFechaProceso = value
            MyBase.CambioItem("dtmFechaProceso")
        End Set
    End Property

    Private _dblCantidad As String
    Public Property dblCantidad() As String
        Get
            Return _dblCantidad
        End Get
        Set(ByVal value As String)
            _dblCantidad = value
            MyBase.CambioItem("dblCantidad")
        End Set
    End Property

    Private _dblTotal As String
    Public Property dblTotal() As String
        Get
            Return _dblTotal
        End Get
        Set(ByVal value As String)
            _dblTotal = value
            MyBase.CambioItem("dblTotal")
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

    ''' <summary>
    ''' Ejecuta el proceso de generar documentos si el usuario presiona el botón Generar RC.
    ''' </summary>
    Public Async Sub Aceptar()
        Try
            If ValidarDetalle() Then

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
                A2Utilidades.Mensajes.mostrarMensajePregunta("¿ Está seguro de realizar las transferencias por pagos por red banco ?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf ConfirmarAceptar, False)
            Else
                Exit Sub
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al presionar el botón Aceptar", Me.ToString(), "Aceptar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para confirmar si continua el proceso si hay fechas sin valoración inferiores a la fecha de valoración.
    ''' </summary>
    Public Sub ConfirmarAceptar(ByVal sender As Object, ByVal e As EventArgs)
        Try

            If Not CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                IsBusy = False
                Exit Sub
            Else
                IsBusy = True
                dcProxyImportaciones.RespuestaArchivoImportacions.Clear()
                dcProxyImportaciones.Load(dcProxyImportaciones.PagosPorRedBanco_ExportarQuery(strNroCuenta, dtmFechaProceso, "PagosPorRedBanco_Exportar", Program.Usuario, Program.Maquina, Program.HashConexion), AddressOf TerminoGenerarArchivoPlano, String.Empty)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al preguntar si está seguro de realizar las tranferencias por pagos por red banco", _
                                                             Me.ToString(), "ConfirmarAceptar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoGenerarArchivoPlano(lo As LoadOperation(Of RespuestaArchivoImportacion))
        Try
            If Not lo.HasError Then
                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacion)
                Dim objListaMensajes As New List(Of String)
                Dim objViewImportarArchivo As New A2ComunesControl.ResultadoGenericoImportaciones()
                Dim logAgregarMensaje As Boolean = True

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    For Each li In objListaRespuesta

                        If logAgregarMensaje Then
                            objListaMensajes.Add("El archivo se generó exitosamente.")
                            objListaMensajes.Add("Se creó la transferencia de pagos por red banco con " & ListaDetalle.First.dblCantidad & " registro(s) por un valor de " & FormatCurrency(ListaDetalle.First.dblTotal))
                            logAgregarMensaje = False
                        End If

                        objListaMensajes.Add(li.Mensaje)
                    Next

                    objViewImportarArchivo.ListaMensajes = objListaMensajes
                Else
                    objViewImportarArchivo.ListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")
                End If

                objViewImportarArchivo.Title = "Generar transferencias pagos por red banco"
                Program.Modal_OwnerMainWindowsPrincipal(objViewImportarArchivo)
                objViewImportarArchivo.ShowDialog()

                strNroCuenta = String.Empty
                strNombreCuenta = String.Empty
                dblCantidad = 0
                dblTotal = 0
                ListaDetalle = Nothing

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar el archivo plano", Me.ToString(), "TerminoGenerarArchivoPlano", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al generar el archivo plano.", Me.ToString(), "TerminoGenerarArchivoPlano", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        IsBusy = False
    End Sub

    Public Async Function ConsultarDetalle() As Task
        Try
            Dim objRet As LoadOperation(Of PagosPorRedBanco)

            IsBusy = True

            If mdcProxy Is Nothing Then
                mdcProxy = New TesoreriaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
            End If

            mdcProxy.PagosPorRedBancos.Clear()

            objRet = Await mdcProxy.Load(mdcProxy.PagosPorRedBanco_ConsultarSyncQuery(strNroCuenta, dtmFechaProceso, Program.Usuario, Program.Maquina, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al seleccionar y consultar los registros.", Me.ToString(), "ConsultarDetalle", Program.TituloSistema, Program.Maquina, objRet.Error)
                End If
            End If

            ListaDetalle = mdcProxy.PagosPorRedBancos.ToList

            dblCantidad = ListaDetalle.First.dblCantidad
            dblTotal = ListaDetalle.First.dblTotal

            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al presionar el buscador de cuentas.", Me.ToString(), "ConsultarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

#End Region

#Region "Métodos privados del encabezado - REQUERIDOS"

    Private Function ValidarDatos() As Boolean
        Dim logResultado As Boolean = False
        Dim strMsg As String = String.Empty

        Try
            '-------------------------------------------------------------------------------------------------------------------------
            '-- VALIDAR DATOS DEL ENCABEZADO
            '-------------------------------------------------------------------------------------------------------------------------
            'If IsNothing(strNroCuenta) Or strNroCuenta = 0 Then
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

    Private Function ValidarDetalle() As Boolean
        Dim logResultado As Boolean = True
        Dim strMsg As String = String.Empty

        Try
            '------------------------------------------------------------------------------------------------------------------------------------------------
            '-- Valida que por lo menos exista un detalle para poder crear todo un registro
            '------------------------------------------------------------------------------------------------------------------------------------------------
            If IsNothing(_ListaDetalle) Then
                strMsg = String.Format("{0}{1} + Debe haber por lo menos un detalle.", strMsg, vbCrLf)
            ElseIf _ListaDetalle.Count = 0 Then
                strMsg = String.Format("{0}{1} + Debe haber por lo menos un detalle.", strMsg, vbCrLf)
            End If

            If Not strMsg.Equals(String.Empty) Then
                logResultado = False
                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias en el detalle: " & vbNewLine & vbNewLine & strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            logResultado = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos del detalle.", Me.ToString(), "ValidarDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        Return (logResultado)
    End Function

#End Region

    'Public Event PropertyChanged1(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

End Class


