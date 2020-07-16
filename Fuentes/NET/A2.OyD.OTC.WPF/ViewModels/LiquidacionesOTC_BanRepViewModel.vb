Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data


Public Class LiquidacionesOTC_BanRepViewModel
    Inherits A2ControlMenu.A2ViewModel

    Private Const MSTR_MC_ACCION_ACTUALIZAR As String = "U"

    Private dcProxyImportaciones As ImportacionesDomainContext
    Private _nroRegistrosCargados As Integer = 0
    Private _NroProceso As Integer
    Private _maximoId As Integer

    Private _DispatcherTimerLiquidacionesRepos As System.Windows.Threading.DispatcherTimer

#Region "Propiedades"


    Private _ViewImportarArchivo As ImportarArchivosOTC_BanRep
    Public Property ViewImportarArchivo() As ImportarArchivosOTC_BanRep
        Get
            Return _ViewImportarArchivo
        End Get
        Set(ByVal value As ImportarArchivosOTC_BanRep)
            _ViewImportarArchivo = value
        End Set
    End Property

    Private _strProgresoArchivo As String
    Public Property strProgresoArchivo() As String
        Get
            Return _strProgresoArchivo
        End Get
        Set(ByVal value As String)
            _strProgresoArchivo = value
            MyBase.CambioItem("strProgresoArchivo")
        End Set
    End Property

    Private _ListaReposBanRep As List(Of ArchivoReposBanRepVista)
    Public Property ListaReposBanRep() As List(Of ArchivoReposBanRepVista)
        Get
            Return _ListaReposBanRep
        End Get
        Set(ByVal value As List(Of ArchivoReposBanRepVista))
            _ListaReposBanRep = value
            MyBase.CambioItem("ListaReposBanRep")
            MyBase.CambioItem("ListaReposBanRepPaged")
        End Set
    End Property

    Public ReadOnly Property ListaReposBanRepPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaReposBanRep) Then
                Dim view = New PagedCollectionView(_ListaReposBanRep)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _ListaReposBanRepSelected As ArchivoReposBanRepVista
    Public Property ListaReposBanRepSelected() As ArchivoReposBanRepVista
        Get
            Return _ListaReposBanRepSelected
        End Get
        Set(ByVal value As ArchivoReposBanRepVista)
            If Not IsNothing(value) Then
                _ListaReposBanRepSelected = value
            End If
            MyBase.CambioItem("ListaReposBanRepSelected")
        End Set
    End Property

    Private _LiquidacionesGeneradas As Boolean = False
    Public Property LiquidacionesGeneradas() As Boolean
        Get
            Return _LiquidacionesGeneradas
        End Get
        Set(ByVal value As Boolean)
            _LiquidacionesGeneradas = value
            MyBase.CambioItem("LiquidacionesGeneradas")
        End Set
    End Property

    Private _mensajeCarga As String = "Cargando..."
    Public Property mensajeCarga() As String
        Get
            Return _mensajeCarga
        End Get
        Set(ByVal value As String)
            _mensajeCarga = value
            MyBase.CambioItem("mensajeCarga")
        End Set
    End Property

    Private _ProgresoArchivo As List(Of String)
    Public Property ProgresoArchivo() As List(Of String)
        Get
            Return _ProgresoArchivo
        End Get
        Set(ByVal value As List(Of String))
            _ProgresoArchivo = value
            MyBase.CambioItem("ProgresoArchivo")
        End Set
    End Property
#End Region

#Region "Métodos"

    Sub CargarArchivoOTC_BanRep(pstrNombreArchivo As String)
        Try
            ViewImportarArchivo.IsBusy = True
            If Not IsNothing(dcProxyImportaciones.RespuestaArchivoImportacionOTCs) Then
                dcProxyImportaciones.RespuestaArchivoImportacionOTCs.Clear()
            End If

            _nroRegistrosCargados = 0

            dcProxyImportaciones.Load(dcProxyImportaciones.CargarArchivoImportacionBanRepQuery(pstrNombreArchivo, "BanRep", Program.Usuario, Program.RutafisicaArchivo, Program.HashConexion), AddressOf TerminoCargarArchivoBanRep, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar el archivo recibo.", _
                               Me.ToString(), "CargarArchivoRecibos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub seguirConsultando(maximoId As Integer)
        If Not IsNothing(dcProxyImportaciones.ArchivoReposBanReps) Then
            dcProxyImportaciones.ArchivoReposBanReps.Clear()
        End If
        dcProxyImportaciones.Load(dcProxyImportaciones.ObtenerValoresArchivoReposBanRepQuery(_NroProceso, Program.Usuario, maximoId, Program.HashConexion), AddressOf TerminoObtenerArchivoRecibos, String.Empty)
    End Sub
#End Region

#Region "Resultados Asíncronos"

    Private Sub TerminoCargarArchivoBanRep(ByVal lo As LoadOperation(Of OyDImportaciones.RespuestaArchivoImportacionOTC))

        Try
            If lo.HasError = False Then

                'MessageBox.Show("Termina bulk insert sin error")


                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacionOTC)
                Dim objListaMensajes As New List(Of String)

                objListaRespuesta = lo.Entities.ToList

                strProgresoArchivo = String.Empty

                If objListaRespuesta.Count > 0 Then
                    If objListaRespuesta.Where(Function(i) i.Exitoso = False).Count > 0 Then
                        objListaMensajes.Add("El archivo genero algunas inconsistencias al intentar subirlo:")

                        For Each li In objListaRespuesta.Where(Function(i) i.Exitoso = False).OrderBy(Function(o) o.Tipo)
                            If li.Tipo = "100+" Then
                                objListaMensajes.Add(li.Mensaje)
                            Else
                                objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: {2}", li.Fila, li.Columna, li.Mensaje))
                            End If
                        Next

                        objListaMensajes.Add("No se importaron registros debido a que se presentaron inconsistencias")

                        ViewImportarArchivo.ListaMensajes = objListaMensajes
                        ViewImportarArchivo.IsBusy = False
                        If Not IsNothing(ListaReposBanRep) Then
                            ListaReposBanRep.Clear()
                            dcProxyImportaciones.ArchivoReposBanReps.Clear()
                            MyBase.CambioItem("ListaReposBanRep")
                            MyBase.CambioItem("ListaReposBanRepPaged")
                        End If
                    Else
                        Dim objProceso As Integer = 0

                        If Not IsNothing(ListaReposBanRep) Then
                            ListaReposBanRep.Clear()
                            dcProxyImportaciones.ArchivoReposBanReps.Clear()
                            MyBase.CambioItem("ListaReposBanRep")
                            MyBase.CambioItem("ListaReposBanRepPaged")
                        End If

                        For Each li In objListaRespuesta.Where(Function(i) i.Exitoso)
                            objListaMensajes.Add(li.Mensaje)
                            objProceso = li.NroProceso
                        Next

                        If objProceso <> 0 Then

                            ViewImportarArchivo.ListaMensajes = objListaMensajes

                            If Not IsNothing(dcProxyImportaciones.InformacionArchivoRecibos) Then
                                dcProxyImportaciones.InformacionArchivoRecibos.Clear()
                            End If
                            'MessageBox.Show("Lanzar consuta de datos importados")

                            dcProxyImportaciones.Load(dcProxyImportaciones.ObtenerValoresArchivoReposBanRepQuery(objProceso, Program.Usuario, 0, Program.HashConexion), AddressOf TerminoObtenerArchivoRecibos, String.Empty)

                        Else
                            ViewImportarArchivo.ListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")
                            ViewImportarArchivo.IsBusy = False
                        End If

                    End If
                Else
                    ViewImportarArchivo.ListaMensajes.Add("No se obtuvieron registros al procesar el archivo.")
                    ViewImportarArchivo.IsBusy = False
                End If
            Else

                'MessageBox.Show("Termina bulk insert con error")

                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el archivo de repos banco república.", Me.ToString(), "TerminoCargarArchivoLEO", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                lo.MarkErrorAsHandled()
                ViewImportarArchivo.IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el archivo de repos banco república.", Me.ToString(), "TerminoCargarArchivoLEO", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            ViewImportarArchivo.IsBusy = False
        End Try
    End Sub

    Private Sub TerminoObtenerArchivoRecibos(ByVal lo As LoadOperation(Of OyDImportaciones.ArchivoReposBanRep))

        Try
            ViewImportarArchivo.IsBusy = False

            If lo.HasError = False Then

                If lo.Entities.Count > 0 Then

                    Alista(dcProxyImportaciones.ArchivoReposBanReps)
                    'MessageBox.Show("Cantidad de registros= " + lo.Entities.Count.ToString)
                    ListaReposBanRepSelected = ListaReposBanRep.ToList().FirstOrDefault()
                    _NroProceso = ListaReposBanRepSelected.idProceso

                    _maximoId = ListaReposBanRep.Max(Function(i) i.intID)
                    'MessageBox.Show("_maximoId= " + _maximoId.ToString)

                    If lo.Entities.Count = 1000 Then
                        seguirConsultando(_maximoId)
                    End If
                    SeleccionarTodasLEO(False) 'Para mostrar resultado directo no uno a uno GOD 20200117

                End If
            Else
                'MessageBox.Show("Con errores")

                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la información el archivo de carga de órdenes.", Me.ToString(), "TerminoObtenerArchivoRecibos", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                ViewImportarArchivo.IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la información el archivo de carga de órdenes.", Me.ToString(), "TerminoObtenerArchivoRecibos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            ViewImportarArchivo.IsBusy = False
        End Try
    End Sub

    Private Sub TerminoSubmitChangesRepos(ByVal So As SubmitOperation)
        Dim strMsg As String = String.Empty
        Try
            IsBusy = False
            If So.HasError Then
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next

                    If Not strMsg.Equals(String.Empty) Then
                        A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        So.MarkErrorAsHandled()
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                                   Me.ToString(), "TerminoSubmitChangesRepos" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                    So.MarkErrorAsHandled()
                End If

                Exit Try
            Else
                ViewImportarArchivo.IsBusy = True
                If Not IsNothing(dcProxyImportaciones.RespuestaArchivoImportacionOTCs) Then
                    dcProxyImportaciones.RespuestaArchivoImportacionOTCs.Clear()
                End If
            End If

            MyBase.TerminoSubmitChanges(So)

            dcProxyImportaciones.GenerarLiquidacionesRepos(_NroProceso, Program.Usuario, Program.HashConexion, AddressOf TerminoCargarLlamadaJobLiquidaciones, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", Me.ToString(), "TerminoSubmitChangesRepos", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoCargarLlamadaJobLiquidaciones(ByVal lo As InvokeOperation(Of String))
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar liquidaciones de repos", _
                Me.ToString(), "AddressOf TerminoCargarLlamadaJobLiquidaciones", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            Else
                ConsultaEstado()
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al término de la llamada al proceso de generación de liquidaciones repos.", Me.ToString(), "TerminoCargarLlamadaJobLiquidaciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoVerifiCargaGeneracionLiquidacionesRepos(ByVal lo As LoadOperation(Of OyDImportaciones.RespuestaArchivoImportacionOTC))
        Try
            Dim strMensaje As String
            If lo.HasError = False Then

                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacionOTC)

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    If objListaRespuesta.Where(Function(i) i.Exitoso = False).Count > 0 Then

                        Dim objTipo As String = String.Empty
                        strMensaje = "Estado de la generación de liquidaciones banco república: "
                        For Each li In objListaRespuesta.Where(Function(i) i.Exitoso = False).OrderBy(Function(o) o.Tipo)
                            strMensaje = String.Format("Se han generado {0} liquidaciones de un total de {1}. Mensajes: {2}", li.Fila, li.Columna, li.Mensaje)
                            mensajeCarga = strMensaje
                        Next

                        strProgresoArchivo = strMensaje
                    Else
                        If objListaRespuesta.Count > 0 Then
                            strMensaje = String.Empty
                            For Each li In objListaRespuesta.Where(Function(i) i.Exitoso)
                                strMensaje = String.Format("Se generaron {0} liquidaciones de un total de {1}." + vbNewLine + "{2}", li.Fila, li.Columna, li.Mensaje)
                                mensajeCarga = strMensaje
                            Next
                        Else
                            strMensaje = "Fin de archivo"
                        End If

                        strProgresoArchivo = strProgresoArchivo + vbNewLine + strMensaje

                        ListaReposBanRep.Clear()
                        dcProxyImportaciones.ArchivoReposBanReps.Clear()
                        MyBase.CambioItem("ListaReposBanRep")
                        MyBase.CambioItem("ListaReposBanRepPaged")

                        PararVerificacion()

                    End If
                Else
                    ProgresoArchivo.Add("No se obtuvieron registros al procesar el archivo.")
                    MyBase.CambioItem("ProgresoArchivo")
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al verificar el estado de generación de liquidaciones repos.", Me.ToString(), "TerminoVerifiCargaGeneracionLiquidacionesRepos", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                ViewImportarArchivo.IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al verificar el estado de generación de liquidaciones repos.", Me.ToString(), "TerminoVerifiCargaGeneracionLiquidacionesRepos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            ViewImportarArchivo.IsBusy = False
        End Try
    End Sub

#End Region

#Region "Métodos"

    Private Sub Alista(entitySet As EntitySet(Of OyDImportaciones.ArchivoReposBanRep))


        If IsNothing(ListaReposBanRep) Then ListaReposBanRep = New List(Of ArchivoReposBanRepVista)

        Try

            _nroRegistrosCargados += entitySet.Count

            For Each it As OyDImportaciones.ArchivoReposBanRep In entitySet

                ListaReposBanRep.Add(New ArchivoReposBanRepVista() With {.intID = it.intID,
                    .id = it.id,
                    .idProceso = it.idProceso,
                    .CodTitulo = it.CodTitulo,
                    .FechaLiquidacion = it.FechaLiquidacion,
                    .FechaRestitucion = it.FechaRestitucion,
                    .Tipo = it.Tipo,
                    .Nemotecnico = it.Nemotecnico,
                    .ISIN = it.ISIN,
                    .NroEmision = it.NroEmision,
                    .NroOferta = it.NroOferta,
                    .Precio = Convert.ToDecimal(FormatearNumero(it.Precio)),
                    .TasaEfectiva = Convert.ToDecimal(FormatearNumero(it.TasaEfectiva)),
                    .ValorNominal = Convert.ToDecimal(FormatearNumero(it.ValorNominal)),
                    .ValorOperacion = Convert.ToDecimal(FormatearNumero(it.ValorOperacion)),
                    .ValorRestitucion = Convert.ToDecimal(FormatearNumero(it.ValorRestitucion)),
                    .strUsuarioArchivo = it.strUsuarioArchivo,
                    .bitGenerarOrden = it.bitGenerarOrden
                })
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Function FormatearNumero(ByVal strValor As String) As String
        Dim strSimboloDecimal As String
        Dim strResultado As String
        If Not IsNothing(strValor) Then
            If strValor.Length > 3 Then
                strSimboloDecimal = strValor.Substring(strValor.Length - 3, 1)

                If strSimboloDecimal = "," Then
                    strResultado = strValor.Replace(".", "")
                    strResultado = strResultado.Replace(",", ".")
                Else
                    strResultado = strValor.Replace(",", "")
                End If
            Else
                strResultado = strValor
            End If
            Return strResultado
        Else
            Return ""
        End If
    End Function

    Public Sub SeleccionarTodasLEO(blnIsChequed As Boolean)
        If ListaReposBanRep IsNot Nothing Then
            For Each it In ListaReposBanRep
                it.bitGenerarOrden = blnIsChequed
            Next
            MyBase.CambioItem("ListaReposBanRep")
            MyBase.CambioItem("ListaReposBanRepPaged")
        End If
    End Sub

    Private Sub GenerarLiquidacionesRepos()
        Dim strAccion As String = MSTR_MC_ACCION_ACTUALIZAR

        Try
            If IsNothing(ListaReposBanRep) Then
                A2Utilidades.Mensajes.mostrarMensaje("No hay archivo cargado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                If ListaReposBanRep.Count > 0 Then
                    If ListaReposBanRep.Where(Function(i) i.bitGenerarOrden = True).Count > 0 Then
                        IsBusy = True
                        datosEntidad()
                        Program.VerificarCambiosProxyServidorImportaciones(dcProxyImportaciones)
                        dcProxyImportaciones.SubmitChanges(AddressOf TerminoSubmitChangesRepos, strAccion)
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("No hay registros seleccionados para la generación de liquidaciones.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("No hay archivo cargado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar el proceso de generación de liquidaciones de repos", Me.ToString(), "GenerarLiquidacionesRepos", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub datosEntidad()
        Dim ID As Integer
        For Each it As OyDImportaciones.ArchivoReposBanRep In dcProxyImportaciones.ArchivoReposBanReps
            ID = it.intID
            it.bitGenerarOrden = ListaReposBanRep.Where(Function(i) i.intID = ID).FirstOrDefault().bitGenerarOrden
        Next
    End Sub

    Public Sub New()
        Try
            '---------------------------------------------------------------------------------------------------------------------
            '-- Inicializar servicio de acceso a datos
            '---------------------------------------------------------------------------------------------------------------------
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxyImportaciones = New ImportacionesDomainContext()
            Else
                dcProxyImportaciones = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
            End If

            DirectCast(dcProxyImportaciones.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.ImportacionesDomainContext.IImportacionesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 5, 0)

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "LiquidacionesOTC_BanRepViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ConsultaEstado()
        Try
            If _DispatcherTimerLiquidacionesRepos Is Nothing Then
                _DispatcherTimerLiquidacionesRepos = New System.Windows.Threading.DispatcherTimer
                _DispatcherTimerLiquidacionesRepos.Interval = New TimeSpan(0, 0, 0, 0, 5000) 'Program.Par_Consultar_LEO_Cada)
                AddHandler _DispatcherTimerLiquidacionesRepos.Tick, AddressOf Me.Each_Tick
            End If
            _DispatcherTimerLiquidacionesRepos.Start()
            LiquidacionesGeneradas = True
            VerficarGeneracion()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al iniciar la verificación de liquidaciones de repos.", _
                                       Me.ToString(), "ConsultaEstado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub Each_Tick(sender As Object, e As EventArgs)
        VerficarGeneracion()
    End Sub

    Private Sub VerficarGeneracion()
        Try

            If Not IsNothing(dcProxyImportaciones.RespuestaArchivoImportacionOTCs) Then
                dcProxyImportaciones.RespuestaArchivoImportacionOTCs.Clear()
            End If

            dcProxyImportaciones.Load(dcProxyImportaciones.VerificarGeneracionLiquidacionesReposQuery(_NroProceso, Program.Usuario, Program.HashConexion), AddressOf TerminoVerifiCargaGeneracionLiquidacionesRepos, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar el archivo recibo.", _
                               Me.ToString(), "CargarArchivoRecibos", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    Private Sub PararVerificacion()
        Try
            If Not _DispatcherTimerLiquidacionesRepos Is Nothing Then
                _DispatcherTimerLiquidacionesRepos.Stop()
                RemoveHandler _DispatcherTimerLiquidacionesRepos.Tick, AddressOf Me.Each_Tick
                _DispatcherTimerLiquidacionesRepos = Nothing
                LiquidacionesGeneradas = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al detener la verificación de creación de órdenes LEO.", _
                                       Me.ToString(), "PararVerificacion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

#Region "Commands"

    Private WithEvents _GenerarLiquidaciones As RelayCommand
    Public ReadOnly Property GenerarLiquidaciones() As RelayCommand
        Get
            If _GenerarLiquidaciones Is Nothing Then
                _GenerarLiquidaciones = New RelayCommand(AddressOf GenerarLiquidacionesRepos)
            End If
            Return _GenerarLiquidaciones
        End Get
    End Property

#End Region
End Class
