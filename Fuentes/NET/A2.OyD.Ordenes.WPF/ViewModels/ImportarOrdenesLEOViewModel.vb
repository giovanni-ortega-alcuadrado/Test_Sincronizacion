Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data

Public Class ImportarOrdenesLEOViewModel
    Inherits A2ControlMenu.A2ViewModel

    Private Const MSTR_MC_ACCION_ACTUALIZAR As String = "U"

    Private dcProxyImportaciones As ImportacionesDomainContext
    Private _NroProceso As System.Nullable(Of Double)
    Private _maximoId As Integer
    Private _nroRegistrosCargados As Integer = 0

    Private _DispatcherTimerOrdenesLEO As System.Windows.Threading.DispatcherTimer
    Private lstMensajeEstadoGeneracionOrdenLEO As New List(Of String)

    Dim intDiasVigenciaDuracionInmediata As Integer

#Region "Properties"

    Private _ViewImportarArchivo As ImportarArchivosLeo
    Public Property ViewImportarArchivo() As ImportarArchivosLeo
        Get
            Return _ViewImportarArchivo
        End Get
        Set(ByVal value As ImportarArchivosLeo)
            _ViewImportarArchivo = value
        End Set
    End Property

    Private WithEvents _OrdenesLEOSelected As ArchivoOrdenesLeoVista
    Public Property OrdenesLEOSelected() As ArchivoOrdenesLeoVista
        Get
            Return _OrdenesLEOSelected
        End Get
        Set(ByVal value As ArchivoOrdenesLeoVista)
            If Not IsNothing(value) Then
                _OrdenesLEOSelected = value
            End If
            MyBase.CambioItem("OrdenesLEOSelected")
        End Set
    End Property

    Private _ListaOrdenesLEO As List(Of ArchivoOrdenesLeoVista)
    Public Property ListaOrdenesLEO() As List(Of ArchivoOrdenesLeoVista)
        Get
            Return _ListaOrdenesLEO
        End Get
        Set(ByVal value As List(Of ArchivoOrdenesLeoVista))
            _ListaOrdenesLEO = value
            MyBase.CambioItem("ListaOrdenesLEO")
            MyBase.CambioItem("ListaOrdenesLEOPaged")
        End Set
    End Property

    Public ReadOnly Property ListaOrdenesLEOPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaOrdenesLEO) Then
                Dim view = New PagedCollectionView(_ListaOrdenesLEO)
                Return view
            Else
                Return Nothing
            End If
        End Get
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

    Private _OrdenesGeneradas As Boolean = False
    Public Property OrdenesGeneradas() As Boolean
        Get
            Return _OrdenesGeneradas
        End Get
        Set(ByVal value As Boolean)
            _OrdenesGeneradas = value
            MyBase.CambioItem("OrdenesGeneradas")
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


    Private _lstTipoClasificacion As List(Of TipoClasificacion)
    Public Property lstTipoClasificacion() As List(Of TipoClasificacion)
        Get
            Return _lstTipoClasificacion
        End Get
        Set(ByVal value As List(Of TipoClasificacion))
            _lstTipoClasificacion = value
            MyBase.CambioItem("lstTipoClasificacion")
        End Set
    End Property

    Private _lstObjetosClasificacion As List(Of OyDImportaciones.ObjetoClasificacion)

    Public Property lstObjetosClasificacion() As List(Of OyDImportaciones.ObjetoClasificacion)
        Get
            Return _lstObjetosClasificacion
        End Get
        Set(ByVal value As List(Of OyDImportaciones.ObjetoClasificacion))
            _lstObjetosClasificacion = value
        End Set
    End Property

    Private _lstComboClasificacion As List(Of OyDImportaciones.ObjetoClasificacion)
    Public Property lstComboClasificacion() As List(Of OyDImportaciones.ObjetoClasificacion)
        Get
            Return _lstComboClasificacion
        End Get
        Set(ByVal value As List(Of OyDImportaciones.ObjetoClasificacion))
            _lstComboClasificacion = value
            MyBase.CambioItem("lstComboClasificacion")
        End Set
    End Property

#Region "Commands"

    Private WithEvents _GenerarOrdenes As RelayCommand
    Public ReadOnly Property GenerarOrdenes() As RelayCommand
        Get
            If _GenerarOrdenes Is Nothing Then
                _GenerarOrdenes = New RelayCommand(AddressOf GenerarOrdenesLEO)
            End If
            Return _GenerarOrdenes
        End Get
    End Property

#End Region

#End Region

#Region "Metodos"
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
                                 Me.ToString(), "ImportarOrdenesLEOViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Sub CargarArchivoLEO(pstrNombreArchivo As String)
        Try
            ViewImportarArchivo.IsBusy = True
            If Not IsNothing(dcProxyImportaciones.RespuestaArchivoImportacions) Then
                dcProxyImportaciones.RespuestaArchivoImportacions.Clear()
            End If

            _nroRegistrosCargados = 0

            dcProxyImportaciones.Load(dcProxyImportaciones.CargarArchivoImportacionLEOQuery(pstrNombreArchivo, "OrdenesLEO", Program.Usuario, Program.RutafisicaArchivo, Program.HashConexion), AddressOf TerminoCargarArchivoLEO, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar el archivo recibo.", _
                               Me.ToString(), "CargarArchivoRecibos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub CargarObjetoClasificacion()

        Dim lstTipoCla = New List(Of TipoClasificacion)()
        lstTipoCla.Add(New TipoClasificacion() With {.id = 0, .Nombre = "0 - Ordinaria"})
        lstTipoCla.Add(New TipoClasificacion() With {.id = 1, .Nombre = "1 - Extraordinaria"})
        lstTipoClasificacion = lstTipoCla

        Try
            dcProxyImportaciones.Load(dcProxyImportaciones.cargarObjetosClasificacionQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoCargarListaObjetoClasificacion, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la información el archivo de carga de recibos.", Me.ToString(), "TerminoObtenerArchivoRecibos", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            ViewImportarArchivo.IsBusy = False

        End Try
    End Sub

    Private Sub GenerarOrdenesLEO()
        Dim strAccion As String = MSTR_MC_ACCION_ACTUALIZAR

        Try
            If IsNothing(ListaOrdenesLEO) Then
                A2Utilidades.Mensajes.mostrarMensaje("No hay archivo cargado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                If ListaOrdenesLEO.Count > 0 Then
                    If ListaOrdenesLEO.Where(Function(i) i.bitGenerarOrden = True).Count > 0 Then
                        IsBusy = True
                        datosEntidad()
                        Program.VerificarCambiosProxyServidorImportaciones(dcProxyImportaciones)
                        dcProxyImportaciones.SubmitChanges(AddressOf TerminoSubmitChangesLEO, strAccion)
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("No hay registros seleccionados para la generación de órdenes.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("No hay archivo cargado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al iniciar el proceso de generación de órdenes", Me.ToString(), "GenerarOrdenesLEO", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub SeleccionarTodasLEO(blnIsChequed As Boolean)
        For Each it In ListaOrdenesLEO
            it.bitGenerarOrden = blnIsChequed
        Next
        MyBase.CambioItem("ListaOrdenesLEO")
        MyBase.CambioItem("ListaOrdenesLEOPaged")
    End Sub

    Private Sub ConsultaEstado()
        Try
            If _DispatcherTimerOrdenesLEO Is Nothing Then
                _DispatcherTimerOrdenesLEO = New System.Windows.Threading.DispatcherTimer
                _DispatcherTimerOrdenesLEO.Interval = New TimeSpan(0, 0, 0, 0, Program.Par_Consultar_LEO_Cada)
                AddHandler _DispatcherTimerOrdenesLEO.Tick, AddressOf Me.Each_Tick
            End If
            _DispatcherTimerOrdenesLEO.Start()
            OrdenesGeneradas = True
            VerficarGeneracion()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al iniciar la verificación de generación de órdenes LEO.", _
                                       Me.ToString(), "ConsultaEstado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub Each_Tick(sender As Object, e As EventArgs)
        VerficarGeneracion()
    End Sub

    Private Sub VerficarGeneracion()
        Try

            If Not IsNothing(dcProxyImportaciones.RespuestaArchivoImportacions) Then
                dcProxyImportaciones.RespuestaArchivoImportacions.Clear()
            End If

            dcProxyImportaciones.Load(dcProxyImportaciones.VerificarGeneracionOrdenesLEOQuery(_NroProceso, Program.Usuario, Program.HashConexion), AddressOf TerminoVerificargeneracionOrdenLEO, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cargar el archivo recibo.", _
                               Me.ToString(), "CargarArchivoRecibos", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    Private Sub PararVerificacion()
        Try
            If Not _DispatcherTimerOrdenesLEO Is Nothing Then
                _DispatcherTimerOrdenesLEO.Stop()
                RemoveHandler _DispatcherTimerOrdenesLEO.Tick, AddressOf Me.Each_Tick
                _DispatcherTimerOrdenesLEO = Nothing
                OrdenesGeneradas = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al detener la verificación de creación de órdenes LEO.", _
                                       Me.ToString(), "PararVerificacion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub Alista(entitySet As EntitySet(Of OyDImportaciones.ArchivoOrdenesLeo))
        Dim lstOjCla, listItemCla As List(Of OyDImportaciones.ObjetoClasificacion)
        Dim strClase As String = entitySet.ToList().FirstOrDefault().Clase
        If strClase = "A" Then
            lstOjCla = lstObjetosClasificacion.Where(Function(i) i.Categoria = "ORDENACCIONESEXTRA").ToList
        Else
            lstOjCla = lstObjetosClasificacion.Where(Function(i) i.Categoria = "ORDENCREDITICIOEXTRA").ToList
        End If

        If IsNothing(ListaOrdenesLEO) Then ListaOrdenesLEO = New List(Of ArchivoOrdenesLeoVista)

        Try

            _nroRegistrosCargados += entitySet.Count

            For Each it As OyDImportaciones.ArchivoOrdenesLeo In entitySet

                If it.TipoClasificacion = 0 Then
                    listItemCla = Nothing
                Else
                    listItemCla = lstOjCla
                End If

                ListaOrdenesLEO.Add(New ArchivoOrdenesLeoVista() With {.intID = it.intID,
                    .id = it.id,
                    .idProceso = it.idProceso,
                    .Clase = it.Clase,
                    .Cliente = it.Cliente,
                    .NombreCliente = it.NombreCliente,
                    .Ordenante = it.Ordenante,
                    .Tipo = it.Tipo,
                    .Usuario = it.Usuario,
                    .Cantidad = it.Cantidad,
                    .Deposito = it.Deposito,
                    .descDeposito = it.descDeposito,
                    .CuentaDeposito = it.CuentaDeposito,
                    .lngidCuentaDeceval = it.lngidCuentaDeceval,
                    .TipoClasificacion = it.TipoClasificacion,
                    .DescTipoClasificacion = it.DescTipoClasificacion,
                    .ObjetoClasificacion = it.ObjetoClasificacion,
                    .Clasificacion = it.Clasificacion,
                    .Especie = it.Especie,
                    .NombreEspecie = it.NombreEspecie,
                    .FechaIngreso = it.FechaIngreso,
                    .FechaVigencia = it.FechaVigencia,
                    .FechaEmision = it.FechaEmision,
                    .FechaVencimiento = it.FechaVencimiento,
                    .Receptor = it.Receptor,
                    .Modalidad = it.Modalidad,
                    .TasaFacial = it.TasaFacial,
                    .TLimite = it.TLimite,
                    .DescTLimite = it.DescTLimite,
                    .CondNegociacion = it.CondNegociacion,
                    .DescCondNegociacion = it.DescCondNegociacion,
                    .FormaPago = it.FormaPago,
                    .DescFormaPago = it.DescFormaPago,
                    .TipoInversion = it.TipoInversion,
                    .DescTipoInversion = it.DescTipoInversion,
                    .Ejecucion = it.Ejecucion,
                    .DescEjecucion = it.DescEjecucion,
                    .Duracion = it.Duracion,
                    .DescDuracion = it.DescDuracion,
                    .ReceptorLeo = it.ReceptorLeo,
                    .CanalLeo = it.CanalLeo,
                    .DescCanalLeo = it.DescCanalLeo,
                    .MedioVerificable = it.MedioVerificable,
                    .DescMedioVerificable = it.DescMedioVerificable,
                    .Comision = it.Comision,
                    .strUsuarioArchivo = it.strUsuarioArchivo,
                    .bitGenerarOrden = it.bitGenerarOrden,
                    .lstComboClasificacion = listItemCla
                })
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub datosEntidad()
        Dim ID As Integer
        For Each it As OyDImportaciones.ArchivoOrdenesLeo In dcProxyImportaciones.ArchivoOrdenesLeos
            ID = it.intID
            it.Clasificacion = ListaOrdenesLEO.Where(Function(i) i.intID = ID).FirstOrDefault().Clasificacion
            it.ObjetoClasificacion = ListaOrdenesLEO.Where(Function(i) i.intID = ID).FirstOrDefault().ObjetoClasificacion
            it.TipoClasificacion = ListaOrdenesLEO.Where(Function(i) i.intID = ID).FirstOrDefault().TipoClasificacion
            it.DescTipoClasificacion = ListaOrdenesLEO.Where(Function(i) i.intID = ID).FirstOrDefault().DescTipoClasificacion
            it.bitGenerarOrden = ListaOrdenesLEO.Where(Function(i) i.intID = ID).FirstOrDefault().bitGenerarOrden
        Next
    End Sub

    Private Sub _OrdenesLEOSelected_PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Handles _OrdenesLEOSelected.PropertyChanged
        Try
            Select Case e.PropertyName.ToLower()
                Case "tipoclasificacion"
                    If Not IsNothing(_OrdenesLEOSelected.TipoClasificacion) Then
                        _OrdenesLEOSelected.DescTipoClasificacion = lstTipoClasificacion.Where(Function(i) i.id = _OrdenesLEOSelected.TipoClasificacion).FirstOrDefault().Nombre
                        If _OrdenesLEOSelected.TipoClasificacion = 1 Then
                            If _OrdenesLEOSelected.Clase = "A" Then _OrdenesLEOSelected.lstComboClasificacion = lstObjetosClasificacion.Where(Function(i) i.Categoria = "ORDENACCIONESEXTRA").ToList Else _OrdenesLEOSelected.lstComboClasificacion = lstObjetosClasificacion.Where(Function(i) i.Categoria = "ORDENCREDITICIOEXTRA").ToList
                        Else
                            _OrdenesLEOSelected.lstComboClasificacion = Nothing
                            _OrdenesLEOSelected.Clasificacion = Nothing
                        End If
                    End If
                Case "objetoclasificacion"
                    If IsNothing(_OrdenesLEOSelected.ObjetoClasificacion) Then
                        _OrdenesLEOSelected.Clasificacion = Nothing
                    Else
                        _OrdenesLEOSelected.Clasificacion = _OrdenesLEOSelected.lstComboClasificacion.Where(Function(i) i.ID = _OrdenesLEOSelected.ObjetoClasificacion).FirstOrDefault().Descripcion
                    End If
            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar el cambio en la propiedad.", Me.ToString, "_OrdenesLEOSelected_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

#End Region

#Region "Resultados asíncronos"

    Private Sub TerminoCargarArchivoLEO(ByVal lo As LoadOperation(Of OyDImportaciones.RespuestaArchivoImportacion))


        Try
            If lo.HasError = False Then

                'MessageBox.Show("Termina bulk insert sin error")


                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacion)
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
                        If Not IsNothing(ListaOrdenesLEO) Then
                            ListaOrdenesLEO.Clear()
                            dcProxyImportaciones.ArchivoOrdenesLeos.Clear()
                            MyBase.CambioItem("ListaOrdenesLEO")
                            MyBase.CambioItem("ListaOrdenesLEOPaged")
                        End If
                    Else
                        Dim objProceso As Double = 0

                        If Not IsNothing(ListaOrdenesLEO) Then
                            ListaOrdenesLEO.Clear()
                            dcProxyImportaciones.ArchivoOrdenesLeos.Clear()
                            MyBase.CambioItem("ListaOrdenesLEO")
                            MyBase.CambioItem("ListaOrdenesLEOPaged")
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
                            dcProxyImportaciones.Load(dcProxyImportaciones.ObtenerValoresArchivoLEOQuery(objProceso, Program.Usuario, 0, Program.HashConexion), AddressOf TerminoObtenerArchivoRecibos, String.Empty)
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

                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el archivo de Ordenes LEO.", Me.ToString(), "TerminoCargarArchivoLEO", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                lo.MarkErrorAsHandled()
                ViewImportarArchivo.IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el archivo de Ordenes LEO.", Me.ToString(), "TerminoCargarArchivoLEO", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            ViewImportarArchivo.IsBusy = False
        End Try
    End Sub

    Private Sub TerminoObtenerArchivoRecibos(ByVal lo As LoadOperation(Of OyDImportaciones.ArchivoOrdenesLeo))

        'MessageBox.Show("TerminoObtenerArchivoRecibos")

        Dim strClase As String
        Try
            ViewImportarArchivo.IsBusy = False

            If lo.HasError = False Then
                'MessageBox.Show("sin errores")

                If lo.Entities.Count > 0 Then
                    'MessageBox.Show("dentro del count")

                    Alista(dcProxyImportaciones.ArchivoOrdenesLeos)
                    'MessageBox.Show("Cantidad de registros= " + lo.Entities.Count.ToString)
                    OrdenesLEOSelected = ListaOrdenesLEO.ToList().FirstOrDefault()
                    _NroProceso = OrdenesLEOSelected.idProceso
                    strClase = OrdenesLEOSelected.Clase

                    _maximoId = ListaOrdenesLEO.Max(Function(i) i.intID)
                    'MessageBox.Show("_maximoId= " + _maximoId.ToString)

                    If strClase = "A" Then lstComboClasificacion = lstObjetosClasificacion.Where(Function(i) i.Categoria = "ORDENACCIONESEXTRA").ToList Else lstComboClasificacion = lstObjetosClasificacion.Where(Function(i) i.Categoria = "ORDENCREDITICIOEXTRA").ToList

                    If lo.Entities.Count = 1000 Then
                        seguirConsultando(_maximoId)
                    End If

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

    Private Sub TerminoSubmitChangesLEO(ByVal So As SubmitOperation)
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
                                                   Me.ToString(), "TerminoSubmitChangesLEO" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                    So.MarkErrorAsHandled()
                End If

                Exit Try
            Else
                ViewImportarArchivo.IsBusy = True
                If Not IsNothing(dcProxyImportaciones.RespuestaArchivoImportacions) Then
                    dcProxyImportaciones.RespuestaArchivoImportacions.Clear()
                End If
            End If

            MyBase.TerminoSubmitChanges(So)

            dcProxyImportaciones.GenerarOrdenesLeo(_NroProceso, Program.Usuario, Program.HashConexion, AddressOf TerminoCargarLlamadaJobLEO, String.Empty)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", Me.ToString(), "TerminoSubmitChangesLEO", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoCargarLlamadaJobLEO(ByVal lo As InvokeOperation(Of String))
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al generar órdenes", _
                Me.ToString(), "AddressOf TerminoCargarLlamadaJobLEO", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            Else
                ConsultaEstado()
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al término de la llamada al proceso de generación de órdenes.", Me.ToString(), "TerminoCargarLlamadaJobLEO", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoVerificargeneracionOrdenLEO(ByVal lo As LoadOperation(Of OyDImportaciones.RespuestaArchivoImportacion))
        Try
            Dim strMensaje As String
            If lo.HasError = False Then

                Dim objListaRespuesta As List(Of OyDImportaciones.RespuestaArchivoImportacion)

                objListaRespuesta = lo.Entities.ToList

                If objListaRespuesta.Count > 0 Then
                    If objListaRespuesta.Where(Function(i) i.Exitoso = False).Count > 0 Then

                        Dim objTipo As String = String.Empty
                        strMensaje = "Estado de la generación de órdenes LEO: "
                        For Each li In objListaRespuesta.Where(Function(i) i.Exitoso = False).OrderBy(Function(o) o.Tipo)
                            strMensaje = String.Format("Se han generado {0} órdenes de un total de {1}. Mensajes: {2}", li.Fila, li.Columna, li.Mensaje)
                            mensajeCarga = strMensaje
                        Next

                        strProgresoArchivo = strMensaje
                    Else
                        If objListaRespuesta.Count > 0 Then
                            strMensaje = String.Empty
                            For Each li In objListaRespuesta.Where(Function(i) i.Exitoso)
                                strMensaje = String.Format("Se generaron {0} órdenes de un total de {1}." + vbNewLine + "{2}", li.Fila, li.Columna, li.Mensaje)
                                mensajeCarga = strMensaje
                            Next
                        Else
                            strMensaje = "Fin de archivo"
                        End If

                        strProgresoArchivo = strProgresoArchivo + vbNewLine + strMensaje

                        ListaOrdenesLEO.Clear()
                        dcProxyImportaciones.ArchivoOrdenesLeos.Clear()
                        MyBase.CambioItem("ListaOrdenesLEO")
                        MyBase.CambioItem("ListaOrdenesLEOPaged")

                        PararVerificacion()

                    End If
                Else
                    ProgresoArchivo.Add("No se obtuvieron registros al procesar el archivo.")
                    MyBase.CambioItem("ProgresoArchivo")
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al verificar el estado de generación de Ordenes LEO.", Me.ToString(), "TerminoVerificargeneracionOrdenLEO", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                ViewImportarArchivo.IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al verificar el estado de generación de Ordenes LEO.", Me.ToString(), "TerminoVerificargeneracionOrdenLEO", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            ViewImportarArchivo.IsBusy = False
        End Try
    End Sub

    Private Sub TerminoCargarListaObjetoClasificacion(ByVal lo As LoadOperation(Of OyDImportaciones.ObjetoClasificacion))
        Try

            If lo.HasError = False Then
                If lo.Entities.Count > 0 Then
                    lstObjetosClasificacion = dcProxyImportaciones.ObjetoClasificacions.ToList
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la lista de objetos clasificación.", Me.ToString(), "TerminoCargarListaObjetoClasificacion", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
                ViewImportarArchivo.IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir la lista de objetos clasificación.", Me.ToString(), "TerminoCargarListaObjetoClasificacion", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

    Private Sub seguirConsultando(maximoId As Integer)
        If Not IsNothing(dcProxyImportaciones.ArchivoOrdenesLeos) Then
            dcProxyImportaciones.ArchivoOrdenesLeos.Clear()
        End If
        dcProxyImportaciones.Load(dcProxyImportaciones.ObtenerValoresArchivoLEOQuery(_NroProceso, Program.Usuario, maximoId, Program.HashConexion), AddressOf TerminoObtenerArchivoRecibos, String.Empty)
    End Sub

End Class
