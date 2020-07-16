Imports Telerik.Windows.Controls
Imports System.Threading.Tasks
Imports A2Utilidades
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFTitulosNet
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades
Imports System.Web
Imports A2ComunesControl
Imports A2ComunesImportaciones

Public Class CruceOperacionesManualViewModel
    Inherits A2ControlMenu.A2ViewModel

    Implements INotifyPropertyChanged

    ''' <summary>
    ''' ViewModel para la pantalla "Títulos" perteneciente al proyecto OyD Server.
    ''' </summary>
    ''' <history>
    ''' Creado por       : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Creacion. 
    ''' Fecha            : Febrero 21/2014
    ''' Pruebas CB       : Jorge Peña - Febrero 21/2014 - Resultado Ok 
    ''' </history>

#Region "Constantes"
    Private Const STR_PARAMETROS_EXPORTAR As String = "[DTMFECHALIMITE]=[[DTMFECHALIMITE]]|[LNGCOMITENTE]=[[LNGCOMITENTE]]|[STRIDESPECIE]=[[STRIDESPECIE]]|[STRCLASE]=[[STRCLASE]]|[USUARIO]=[[USUARIO]]"
    Private Const STR_CARPETA As String = "EXPORTARPORTAFOLIO"
    Private Const STR_PROCESO As String = "TitulosExportarPortafolio"

    Private Enum PARAMETROSEXPORTAR
        DTMFECHALIMITE
        LNGCOMITENTE
        STRIDESPECIE
        STRCLASE
        USUARIO
    End Enum

    Const TAB_TITULOS_VENDIDOS = 1
    Const TAB_TITULOS_CRUCE = 2
#End Region

#Region "Variables - REQUERIDO"

    Public ViewTitulos As CruceOperacionesManualView = Nothing
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    ' Poxies para comunicación con el domain service
    '---------------------------------------------------------------------------------------------------------------------------------------------------
    Private mdcProxy As TitulosNetDomainContext  ' Comunicación con el web service del data context. Se maneja a nivel global para garantizar que el domain service identifique los cambios
    Private dtmExportarPortafolio As System.Nullable(Of System.DateTime)
    Private intTotalRegistrosVentas As Integer = 0
    Public STRARCH_TITULOS_MVTOS As String = "NO"
#End Region

#Region "Inicialización - REQUERIDO"
    ''' <summary>
    ''' Constructor de la clase
    ''' </summary>
    Public Sub New()
        IndexClase = 0  '(Todos)
        IsBusy = True ' Activar el control que bloquea la pantalla mientras se está procesando

    End Sub

    ''' <summary>
    ''' Inicalización de acceso a datos y carga inicial de datos
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba: CP0001
    ''' Responsable      : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Enero 5/2015
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Enero 5/2015 - Resultado Ok 
    ''' </history>
    Public Function inicializar() As Boolean

        Dim logResultado As Boolean = False

        Try
            ' Validación para cuando se carga en modo de diseño que no intente conectar al web service
            If Not Program.IsDesignMode() Then

                'Inicializar consultas de datos por defecto para los nuevos registros. A estos métodos no se les antepone el Await para permitir que su llamado sea asincrónico
                dtmFechaCorte = Date.Now()
                ConsultarParametros()

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

    Private _dtmFechaCorte As System.Nullable(Of System.DateTime)
    Public Property dtmFechaCorte() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmFechaCorte
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmFechaCorte = value
            MyBase.CambioItem("dtmFechaCorte")
        End Set
    End Property

    Private _strIdEspecie As String = String.Empty
    Public Property strIdEspecie() As String
        Get
            Return _strIdEspecie
        End Get
        Set(ByVal value As String)
            _strIdEspecie = value
            MyBase.CambioItem("strIdEspecie")
        End Set
    End Property

    Private _strClase As String = String.Empty
    Public Property strClase() As String
        Get
            Return _strClase
        End Get
        Set(ByVal value As String)
            _strClase = value
            MyBase.CambioItem("strClase")
        End Set
    End Property

    Private _lngIDComitente As String = String.Empty
    Public Property lngIDComitente() As String
        Get
            Return _lngIDComitente
        End Get
        Set(ByVal value As String)
            _lngIDComitente = LTrim(RTrim(value))
            MyBase.CambioItem("lngIDComitente")
        End Set
    End Property

    Private _BorrarCliente As Boolean = False
    Public Property BorrarCliente() As Boolean
        Get
            Return _BorrarCliente
        End Get
        Set(ByVal value As Boolean)
            _BorrarCliente = value
            MyBase.CambioItem("BorrarCliente")
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

    Private _strTipoProceso As String = String.Empty
    Public Property strTipoProceso() As String
        Get
            Return _strTipoProceso
        End Get
        Set(ByVal value As String)
            _strTipoProceso = value
            MyBase.CambioItem("strTipoProceso")
        End Set
    End Property

    Private _IndexClase As Integer
    Public Property IndexClase() As Integer
        Get
            Return _IndexClase
        End Get
        Set(ByVal value As Integer)
            _IndexClase = value
            MyBase.CambioItem("IndexClase")
        End Set
    End Property

    Private _HabilitarbtnExportarExcel As Boolean = False
    Public Property HabilitarbtnExportarExcel() As Boolean
        Get
            Return _HabilitarbtnExportarExcel
        End Get
        Set(ByVal value As Boolean)
            _HabilitarbtnExportarExcel = value
            MyBase.CambioItem("HabilitarbtnExportarExcel")
        End Set
    End Property

    ''' <summary>
    ''' Lista de Titulos que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezadoVentas As EntitySet(Of TitulosVentas)
    Public Property ListaEncabezadoVentas() As EntitySet(Of TitulosVentas)
        Get
            Return _ListaEncabezadoVentas
        End Get
        Set(ByVal value As EntitySet(Of TitulosVentas))
            _ListaEncabezadoPaginadaVentas = Nothing
            _ListaEncabezadoVentas = value
            MyBase.CambioItem("ListaEncabezadoVentas")
            MyBase.CambioItem("ListaEncabezadoPaginadaVentas")
        End Set
    End Property

    Private _ListaEncabezadoVentasFiltro As List(Of TitulosVentas)
    Public Property ListaEncabezadoVentasFiltro() As List(Of TitulosVentas)
        Get
            Return _ListaEncabezadoVentasFiltro
        End Get
        Set(ByVal value As List(Of TitulosVentas))
            _ListaEncabezadoPaginadaVentas = Nothing
            _ListaEncabezadoVentasFiltro = value

            MyBase.CambioItem("ListaEncabezadoVentasFiltro")
            MyBase.CambioItem("ListaEncabezadoPaginadaVentas")
        End Set
    End Property

    ''' <summary>
    ''' Colección que pagina la lista de Titulos para navegar sobre el grid con paginación
    ''' </summary>
    Private _ListaEncabezadoPaginadaVentas As PagedCollectionView = Nothing
    Public ReadOnly Property ListaEncabezadoPaginadaVentas() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEncabezadoVentas) Then
                If IsNothing(_ListaEncabezadoPaginadaVentas) Then
                    Dim view = New PagedCollectionView(_ListaEncabezadoVentas)
                    _ListaEncabezadoPaginadaVentas = view
                    Return view
                Else
                    Return (_ListaEncabezadoPaginadaVentas)
                End If
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Lista de Titulos que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaEncabezadoPortafolio As EntitySet(Of TitulosPortafolio)
    Public Property ListaEncabezadoPortafolio() As EntitySet(Of TitulosPortafolio)
        Get
            Return _ListaEncabezadoPortafolio
        End Get
        Set(ByVal value As EntitySet(Of TitulosPortafolio))
            _ListaEncabezadoPaginadaPortafolio = Nothing
            _ListaEncabezadoPortafolio = value
            
            MyBase.CambioItem("ListaEncabezadoPortafolio")
            MyBase.CambioItem("ListaEncabezadoPaginadaPortafolio")
        End Set
    End Property

    ''' <summary>
    ''' Colección que pagina la lista de Titulos para navegar sobre el grid con paginación
    ''' </summary>
    Private _ListaEncabezadoPaginadaPortafolio As PagedCollectionView = Nothing
    Public ReadOnly Property ListaEncabezadoPaginadaPortafolio() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEncabezadoPortafolio) Then
                If IsNothing(_ListaEncabezadoPaginadaPortafolio) Then
                    Dim view = New PagedCollectionView(_ListaEncabezadoPortafolio)
                    _ListaEncabezadoPaginadaPortafolio = view
                    Return view
                Else
                    Return (_ListaEncabezadoPaginadaPortafolio)
                End If
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _TabSeleccionado As Integer = 0
    Public Property TabSeleccionado As Integer
        Get
            Return _TabSeleccionado
        End Get
        Set(value As Integer)
            _TabSeleccionado = value
            Select Case value
                Case TAB_TITULOS_VENDIDOS
                    VisibilidadContenedorPrincipalPortafolio = Visibility.Visible
                Case TAB_TITULOS_CRUCE
                    VisibilidadContenedorPrincipalPortafolio = Visibility.Collapsed
            End Select
            MyBase.CambioItem("TabSeleccionado")
        End Set
    End Property

    Private _VisibilidadContenedorPrincipalPortafolio As Visibility = Visibility.Visible
    Public Property VisibilidadContenedorPrincipalPortafolio() As Visibility
        Get
            Return _VisibilidadContenedorPrincipalPortafolio
        End Get
        Set(ByVal value As Visibility)
            _VisibilidadContenedorPrincipalPortafolio = value
            MyBase.CambioItem("VisibilidadContenedorPrincipalPortafolio")
        End Set
    End Property

    ''' <summary>
    ''' Indica cuál de los detalles está seleccionado
    ''' </summary>
    Private WithEvents _DetalleSeleccionado As TitulosVentas
    Public Property DetalleSeleccionado() As TitulosVentas
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As TitulosVentas)
            _DetalleSeleccionado = value
            If Not IsNothing(DetalleSeleccionado) Then
                ConsultarPortafolio()
            End If

            MyBase.CambioItem("DetalleSeleccionado")
        End Set
    End Property

    Private _IsBusyPortafolio As Boolean
    Public Property IsBusyPortafolio() As Boolean
        Get
            Return _IsBusyPortafolio
        End Get
        Set(ByVal value As Boolean)
            _IsBusyPortafolio = value
            MyBase.CambioItem("IsBusyPortafolio")
        End Set
    End Property

    Private _strCantidadRegistrosVentas As String = String.Empty
    Public Property strCantidadRegistrosVentas() As String
        Get
            Return _strCantidadRegistrosVentas
        End Get
        Set(ByVal value As String)
            _strCantidadRegistrosVentas = value
            MyBase.CambioItem("strCantidadRegistrosVentas")
        End Set
    End Property

    Private _strFiltroVentas As String = String.Empty
    Public Property strFiltroVentas() As String
        Get
            Return _strFiltroVentas
        End Get
        Set(ByVal value As String)
            _strFiltroVentas = value
            MyBase.CambioItem("strFiltroVentas")
        End Set
    End Property

#End Region

#Region "Propiedades de la Especie"
    Private _NemotecnicoSeleccionado As OYDUtilidades.BuscadorEspecies
    Public Property NemotecnicoSeleccionado As OYDUtilidades.BuscadorEspecies
        Get
            Return (_NemotecnicoSeleccionado)
        End Get
        Set(ByVal value As OYDUtilidades.BuscadorEspecies)
            _NemotecnicoSeleccionado = value
            strIdEspecie = String.Empty
            MyBase.CambioItem("strIdEspecie")
        End Set
    End Property
#End Region

#Region "Métodos públicos del encabezado - REQUERIDO"

    Public Async Function ConsultarLiquidacionesVenta() As Task
        Try
            Dim objTitulosVentas As LoadOperation(Of TitulosVentas)

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyTitulosNet()
            End If

            mdcProxy.TitulosVentas.Clear()

            objTitulosVentas = Await mdcProxy.Load(mdcProxy.CruceOperacionesManual_VendidosSinDescargar_ConsultarSyncQuery(Nothing, False, Nothing, lngIDComitente, strIdEspecie, dtmFechaCorte, strClase, Program.Usuario, Program.HashConexion)).AsTask

            If Not objTitulosVentas Is Nothing Then
                If objTitulosVentas.HasError Then
                    If objTitulosVentas.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar liquidaciones de venta.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar liquidaciones de venta.", Me.ToString(), "ConsultarLiquidacionesVenta", Program.TituloSistema, Program.Maquina, objTitulosVentas.Error)
                    End If

                    objTitulosVentas.MarkErrorAsHandled()
                Else
                    ListaEncabezadoVentas = mdcProxy.TitulosVentas
                    If ListaEncabezadoVentas.Count > 0 Then
                        intTotalRegistrosVentas = CInt(mdcProxy.TitulosVentas.FirstOrDefault.intCantidadRegistros)
                        strCantidadRegistrosVentas = CStr(ListaEncabezadoVentas.Count) +
                                                      " registro(s) de " + CStr(mdcProxy.TitulosVentas.FirstOrDefault.intCantidadRegistros) +
                                                      " existente(s)"
                    Else
                        strCantidadRegistrosVentas = "0 registro(s) de 0 existente(s)"
                    End If
                End If
            Else
                ListaEncabezadoVentas = Nothing
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar liquidaciones de venta", Me.ToString, "ConsultarLiquidacionesVenta", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Function

    ''' <history>
    ''' ID caso de prueba: CP0007
    ''' Responsable      : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Enero 5/2015
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Enero 5/2015 - Resultado Ok 
    ''' </history>
    Public Async Function ConsultarPortafolio() As Task
        Try
            Dim objTitulosPortafolio As LoadOperation(Of TitulosPortafolio)

            IsBusyPortafolio = True

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyTitulosNet()
            End If

            mdcProxy.TitulosPortafolios.Clear()

            'For Each li In ListaEncabezadoVentas
            '    If li.intIDVentasXDescargar <> DetalleSeleccionado.intIDVentasXDescargar Then
            '        li.logCruzarLiquidacionVenta = False
            '    End If
            'Next

            objTitulosPortafolio = Await mdcProxy.Load(mdcProxy.CruceOperacionesManual_PortafolioCliente_ConsultarSyncQuery(Nothing, False, dtmFechaCorte, DetalleSeleccionado.lngIDComitente, DetalleSeleccionado.strIDEspecie, DetalleSeleccionado.strClase_Descripcion, DetalleSeleccionado.intIDLiquidaciones, DetalleSeleccionado.strOrigen, Program.Usuario, Program.HashConexion)).AsTask

            dtmExportarPortafolio = dtmFechaCorte

            If Not objTitulosPortafolio Is Nothing Then
                If objTitulosPortafolio.HasError Then
                    If objTitulosPortafolio.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el portafolio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el portafolio.", Me.ToString(), "ConsultarPortafolio", Program.TituloSistema, Program.Maquina, objTitulosPortafolio.Error)
                    End If

                    objTitulosPortafolio.MarkErrorAsHandled()
                Else
                    ListaEncabezadoPortafolio = mdcProxy.TitulosPortafolios
                    If ListaEncabezadoPortafolio.Count > 0 Then
                        HabilitarbtnExportarExcel = True
                    Else
                        HabilitarbtnExportarExcel = False
                    End If
                End If
            Else
                ListaEncabezadoPortafolio = Nothing
            End If

            IsBusyPortafolio = False

        Catch ex As Exception
            IsBusyPortafolio = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al intentar consultar el portafolio", Me.ToString, "ConsultarPortafolio", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
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
            'Valida la fecha de valoración
            If IsNothing(dtmFechaCorte) Then
                strMsg = String.Format("{0}{1} + La fecha de corte es un campo requerido.", strMsg, vbCrLf)
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

    Private Async Sub ConfirmarConsultarLiquidaciones(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyTitulosNet()
            End If

            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then

                IsBusy = True

                Await ConsultarLiquidacionesVenta()

                lngIDComitente = String.Empty
                strIdEspecie = String.Empty
                strClase = "T"

                MyBase.CambioItem("ListaEncabezadoVentas")
            End If
            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al confirmar consultar liquidaciones.", Me.ToString(), "ConfirmarConsultarLiquidaciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Async Sub ConfirmarExportarPortafolio(ByVal sender As Object, ByVal e As EventArgs)
        Try
            IsBusy = True

            Dim objRet As LoadOperation(Of GenerarArchivosPlanos)
            Dim dcProxyUtil As UtilidadesDomainContext
            Dim strParametros As String = STR_PARAMETROS_EXPORTAR
            Dim dia As String = String.Empty
            Dim mes As String = String.Empty
            Dim ano As String = String.Empty
            Dim strFechaLimite As String = String.Empty

            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then
                If Not ValidarDatos() Then Exit Sub

                dcProxyUtil = inicializarProxyUtilidadesOYD()

                If (dtmExportarPortafolio.Value.Day < 10) Then
                    dia = "0" + dtmExportarPortafolio.Value.Day.ToString
                Else
                    dia = dtmExportarPortafolio.Value.Day.ToString
                End If

                If (dtmExportarPortafolio.Value.Month < 10) Then
                    mes = "0" + dtmExportarPortafolio.Value.Month.ToString
                Else
                    mes = dtmExportarPortafolio.Value.Month.ToString
                End If

                ano = dtmExportarPortafolio.Value.Year.ToString

                strFechaLimite = ano + mes + dia

                strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.DTMFECHALIMITE), strFechaLimite)
                strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.LNGCOMITENTE), DetalleSeleccionado.lngIDComitente)
                strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.STRIDESPECIE), DetalleSeleccionado.strIDEspecie)
                strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.STRCLASE), DetalleSeleccionado.strClase_Descripcion)
                strParametros = strParametros.Replace(ValorAReemplazar(PARAMETROSEXPORTAR.USUARIO), Program.Usuario)

                objRet = Await dcProxyUtil.Load(dcProxyUtil.GenerarArchivoPlanoSyncQuery(STR_CARPETA, STR_PROCESO, strParametros, "InformePortafolio", "TAB", "EXCEL", Program.Maquina, Program.Usuario, Program.HashConexion)).AsTask

                If Not objRet Is Nothing Then
                    If objRet.HasError Then
                        If objRet.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la generación del archivo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la generación del archivo.", Me.ToString(), "ExportarPortafolioExcel", Program.TituloSistema, Program.Maquina, objRet.Error)
                        End If
                        objRet.MarkErrorAsHandled()
                    Else
                        If objRet.Entities.Count > 0 Then
                            Dim objResultado = objRet.Entities.First

                            If objResultado.Exitoso Then
                                If (Application.Current.IsRunningOutOfBrowser) Then
                                    Dim button As New MyHyperlinkButton
                                    button.NavigateUri = New Uri(objResultado.RutaArchivoPlano & "?date=" & strFechaLimite & DateTime.Now.ToString("HH:mm:ss"))
                                    button.TargetName = "_blank"
                                    button.ClickMe()
                                Else
                                    HtmlPage.Window.Navigate(New Uri(objResultado.RutaArchivoPlano & "?date=" & strFechaLimite & DateTime.Now.ToString("HH:mm:ss")), "_blank")
                                End If
                            Else
                                A2Utilidades.Mensajes.mostrarMensaje(objResultado.Mensaje, Program.TituloSistema, wppMensajes.TiposMensaje.Advertencia)
                            End If
                        End If
                    End If
                End If

            End If
            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al confirmar la exportación del portafolio en excel.", Me.ToString(), "ConfirmarExportarPortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Function FiltrarVentas(ByVal pstrFiltro As String) As Task
        Try
            Dim objTitulosVentas As LoadOperation(Of TitulosVentas)

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyTitulosNet()
            End If

            mdcProxy.TitulosVentas.Clear()

            IsBusy = True

            objTitulosVentas = Await mdcProxy.Load(mdcProxy.CruceOperacionesManual_VendidosSinDescargar_ConsultarSyncQuery(pstrFiltro, CType(IIf(pstrFiltro = "", True, False), Boolean?), Nothing, lngIDComitente, strIdEspecie, dtmFechaCorte, strClase, Program.Usuario, Program.HashConexion)).AsTask

            If Not objTitulosVentas Is Nothing Then
                If objTitulosVentas.HasError Then
                    If objTitulosVentas.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar liquidaciones de venta.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar liquidaciones de venta.", Me.ToString(), "ConsultarLiquidacionesVenta", Program.TituloSistema, Program.Maquina, objTitulosVentas.Error)
                    End If

                    objTitulosVentas.MarkErrorAsHandled()
                Else
                    ListaEncabezadoVentas = mdcProxy.TitulosVentas
                    If ListaEncabezadoVentas.Count > 0 Then
                        strCantidadRegistrosVentas = CStr(ListaEncabezadoVentas.Count) +
                                                      " registro(s) de " + CStr(mdcProxy.TitulosVentas.FirstOrDefault.intCantidadRegistros) +
                                                      " existente(s)"
                    Else
                        strCantidadRegistrosVentas = "0 registro(s) de " +
                                                      CStr(intTotalRegistrosVentas) + " existente(s)"
                    End If
                End If
            Else
                ListaEncabezadoVentas = Nothing
            End If
            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al filtrar la información de las ventas.", Me.ToString(), "FiltrarVentas", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function

    ''' <summary>
    ''' Método para seleccionar o retirar la selección de todas las casillas tipo check en la columna de cobro
    ''' </summary>
    ''' <param name="blnIsChequed">Parámetro tipo Boolean</param>
    Public Sub SeleccionarTodoPortafolio(blnIsChequed As Boolean)
        Try
            If ListaEncabezadoPortafolio IsNot Nothing Then
                For Each it In ListaEncabezadoPortafolio
                    it.logCruzarPortafolio = blnIsChequed
                Next
                MyBase.CambioItem("SeleccionarTodoPortafolio")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar todos los portafolios.", Me.ToString(), "SeleccionarTodoPortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Function FiltrarPortafolio(ByVal pstrFiltro As String) As Task
        Try
            Dim objTitulosPortafolios As LoadOperation(Of TitulosPortafolio)

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyTitulosNet()
            End If

            mdcProxy.TitulosPortafolios.Clear()

            IsBusy = True

            If Not IsNothing(DetalleSeleccionado) Then
                objTitulosPortafolios = Await mdcProxy.Load(mdcProxy.CruceOperacionesManual_PortafolioCliente_ConsultarSyncQuery(pstrFiltro, CType(IIf(pstrFiltro = "", True, False), Boolean?), dtmFechaCorte, DetalleSeleccionado.lngIDComitente, DetalleSeleccionado.strIDEspecie, DetalleSeleccionado.strClase_Descripcion, DetalleSeleccionado.intIDLiquidaciones, DetalleSeleccionado.strOrigen, Program.Usuario, Program.HashConexion)).AsTask

                If Not objTitulosPortafolios Is Nothing Then
                    If objTitulosPortafolios.HasError Then
                        If objTitulosPortafolios.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al consultar el portafolio del cliente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        Else
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al consultar el portafolio del cliente.", Me.ToString(), "FiltrarPortafolio", Program.TituloSistema, Program.Maquina, objTitulosPortafolios.Error)
                        End If

                        objTitulosPortafolios.MarkErrorAsHandled()
                    Else
                        ListaEncabezadoPortafolio = mdcProxy.TitulosPortafolios
                    End If
                Else
                    ListaEncabezadoPortafolio = Nothing
                End If
            End If

            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al filtrar la información del portafolio.", Me.ToString(), "FiltrarPortafolio", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function

#End Region

#Region "Resultados asincrónicos del encabezado - REQUERIDO"
    ''' <history>
    ''' ID caso de prueba: CP0002, CP0003
    ''' Responsable      : Jorge Peña (Alcuadrado S.A.)
    ''' Fecha            : Enero 5/2015
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Enero 5/2015 - Resultado Ok 
    ''' </history>
    Public Async Function ConsultarLiquidaciones() As Task
        Try
            IsBusy = True

            If ValidarDatos() Then
                If Not IsNothing(dtmFechaCorte) Then
                    If dtmFechaCorte > Date.Now() Then
                        A2Utilidades.Mensajes.mostrarMensaje("La fecha de corte debe ser igual o menor a la actual.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        dtmFechaCorte = Date.Now()
                        Exit Function
                    End If
                End If
            Else
                IsBusy = False
                dtmFechaCorte = Date.Now()
                Exit Function
            End If

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyTitulosNet()
            End If

            mdcProxy.TitulosCruces.Clear()

            strFiltroVentas = String.Empty

            If (strClase = "" Or strClase = "T") And lngIDComitente = "" And strIdEspecie = "" Then
                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensajePregunta("Esta operación puede tardar varios minutos, ¿Desea continuar?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf ConfirmarConsultarLiquidaciones)
            Else
                Await ConsultarLiquidacionesVenta()

                MyBase.CambioItem("ListaEncabezadoVentas")

                IsBusy = False
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar liquidaciones", Me.ToString(), "ConsultarLiquidaciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos del encabezado - REQUERIDO"

    Private Function ValorAReemplazar(ByVal pintTipoCampo As PARAMETROSEXPORTAR) As String
        Return String.Format("[[{0}]]", pintTipoCampo.ToString)
    End Function

    Public Sub ExportarPortafolioExcel()
        Try
            If HabilitarbtnExportarExcel Then
                A2Utilidades.Mensajes.mostrarMensajePregunta("¿Exportar portafolio a Excel?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf ConfirmarExportarPortafolio)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso método exportar portafolio excel", _
                                                             Me.ToString(), "ExportarPortafolioExcel", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Async Function Grabar() As Task
        Try
            Dim xmlCompleto As String
            Dim xmlDetalle As String

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyTitulosNet()
            End If

            IsBusy = True

            If Not IsNothing(ListaEncabezadoPortafolio) Then
                xmlCompleto = "<TitulosDescargadosManualmente>"

                For Each objeto In (From c In ListaEncabezadoPortafolio.Where(Function(i) CType(i.logCruzarPortafolio, Boolean) = True))

                    xmlDetalle = "<Detalle intIDDetalleCustodias=""" & objeto.intIDDetalleCustodias & """></Detalle>"

                    xmlCompleto = xmlCompleto & xmlDetalle
                Next

                xmlCompleto = xmlCompleto & "</TitulosDescargadosManualmente>"

                Await mdcProxy.CruceOperacionesManual_ActualizarSync(DetalleSeleccionado.intIDLiquidaciones, xmlCompleto, DetalleSeleccionado.strOrigen, "", Program.Usuario, Program.HashConexion).AsTask()

            End If

            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al registrar las custodias para realizar el cruce", _
                                                             Me.ToString(), "Grabar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

#End Region

#Region "Consultar parametros"

    ''' <summary>
    ''' Método para consultar el parámetro que indica si activa popup para los movimientos Deceval.
    ''' </summary>
    Public Sub ConsultarParametros()
        Try
            Dim dcProxyUtil As UtilidadesDomainContext

            dcProxyUtil = inicializarProxyUtilidadesOYD()

            dcProxyUtil.Verificaparametro("ARCH_TITULOS_MVTOS", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "ARCH_TITULOS_MVTOS")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el método consultar parámetros", _
                                                             Me.ToString(), "ConsultarParametros", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado de asignar el resultado de los parámetros consultados en la tabla de parámetros.
    ''' </summary>
    ''' <param name="lo">Valor del parámetro</param>
    Private Sub TerminoConsultarParametros(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            Select Case lo.UserState.ToString
                Case "ARCH_TITULOS_MVTOS"
                    STRARCH_TITULOS_MVTOS = lo.Value.ToString
            End Select
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de paramétros", _
                                                 Me.ToString(), "TerminoConsultarParametros", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub

#End Region

End Class






