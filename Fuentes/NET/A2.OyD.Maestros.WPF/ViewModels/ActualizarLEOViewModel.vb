Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ActualizarLEOViewModel.vb
'Generado el : 03/07/2011 12:15:57
'Propiedad de Alcuadrado S.A. 2010

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu
Imports A2Utilidades

Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades
Imports Telerik.Windows.Data

Public Class ActualizarLEOViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaBancosNacionale
    'Private BancosNacionalePorDefecto As BancosNacionale
    'Private BancosNacionaleAnterior As BancosNacionale
    'Private RelacionesCodBancoPorDefecto As RelacionesCodBanco

    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim dcProxy2 As MaestrosDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)
    Dim pstrTipo, pstrClase, pstrDatoReceptor, pstrLEO As String
    Dim plngID As Integer
    Dim objProxy As UtilidadesDomainContext
    Dim ValorParametro As String
    Public ViewActualizarLeo As ActualizarLEOView
    Dim EstadosColasLEO As EstadosColasLEO
    Dim dg As ActualizarLEOView
    Public logRealizarConsultaOrdenes As Boolean = True

    Public Sub New()

        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New MaestrosDomainContext()
                dcProxy1 = New MaestrosDomainContext()
                dcProxy2 = New MaestrosDomainContext()
                objProxy = New UtilidadesDomainContext()
            Else
                dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
                dcProxy1 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
                dcProxy2 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
                objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
            End If
            If Not Program.IsDesignMode() Then
                'IsBusy = True
                dcProxy.OrdenesLEO_TipoOrden(Program.Usuario, Program.HashConexion, AddressOf TraerConsultaTipoOrdenCompleted, "")
                objProxy.Verificaparametro("TIEMPO_ACTUALIZACION_LEO", Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametrolista, "TIEMPO_ACTUALIZACION_LEO")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ActualizarLEOViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TraerLeoCompleted(lo As LoadOperation(Of ListadoActualizarLEO))
        Try

            Dim actual As New List(Of ListadoActualizarLEO)
            Dim anterior As New List(Of ListadoActualizarLEO)
            Dim Itemanterior As Integer, Itemactual As Integer
            Dim a As ListadoActualizarLEO
            'Dim diccionariocoloractual As List(Of diccionariocoloractual)
            Dim diccionariocoloranterior As List(Of diccionariocoloranterior)

            If Not lo.HasError Then
                ObjMarcar = False
                Dim objListaEstadosLEO = dcProxy.ListadoActualizarLEOs.ToList()

                If Not IsNothing(objListaEstadosLEO) Then

                    If objListaEstadosLEO.Count > 0 Then
                        diccionariocoloractual = New List(Of diccionariocoloractual)
                        For Each li In objListaEstadosLEO 'actual
                            If Not IsNothing(li.strEstadoMaximaVersion) Then
                                Select Case li.strEstadoMaximaVersion.FirstOrDefault
                                    Case "M"
                                        Itemactual = li.lngVersionOriginal
                                        If anterior.Count = 0 Then
                                            diccionariocoloractual.Add(New diccionariocoloractual With {.Key = li.lngIDOrden, .Clase = li.strClase, .Color = "Yellow"})
                                        Else
                                            For Each li1 In anterior
                                                If li1.lngIDOrden = li.lngIDOrden And li1.strClase = li.strClase Then
                                                    a = li
                                                    Exit For
                                                Else
                                                    Itemanterior = -1
                                                End If
                                            Next
                                            Itemanterior = a.lngVersionOriginal
                                            If Not (Itemactual.Equals(Itemanterior)) Then
                                                If diccionariocoloranterior.Count < 0 Then
                                                    For Each li2 In diccionariocoloranterior
                                                        If li2.Key = li.lngIDOrden And li2.Clase = li.strClase And li2.Color = "Yellow" Then
                                                            diccionariocoloractual.Add(New diccionariocoloractual With {.Key = li.lngIDOrden, .Clase = li.strClase, .Color = "Orange"})
                                                            Exit For
                                                        ElseIf li2.Key = li.lngIDOrden And li2.Color = "Orange" And li2.Clase = li.strClase Then
                                                            diccionariocoloractual.Add(New diccionariocoloractual With {.Key = li.lngIDOrden, .Color = "Yellow", .Clase = li.strClase})
                                                            Exit For
                                                        End If
                                                    Next
                                                End If
                                            Else
                                                For Each li3 In diccionariocoloranterior
                                                    If li3.Key = li.lngIDOrden And li3.Clase = li.strClase Then
                                                        'datarow.BackColor = li3.Color
                                                        diccionariocoloractual.Add(New diccionariocoloractual With {.Key = li3.Key, .Color = li3.Color, .Clase = li3.Clase})
                                                        Exit For

                                                    End If
                                                Next
                                                'If Not datarow.BackColor = Color.Yellow And Not datarow.BackColor = Color.Orange Then
                                                '    datarow.BackColor = Color.Yellow
                                                '    diccionariocoloractual.Add(New listacolor With {.Key = mResult.lngIDOrden, .value = Color.Yellow, .clase = mResult.strClase})
                                                'End If

                                            End If

                                        End If
                                    Case "A"
                                        diccionariocoloractual.Add(New diccionariocoloractual With {.Key = li.lngIDOrden, .Color = "Rojo", .Clase = li.strClase})
                                    Case "C"
                                        diccionariocoloractual.Add(New diccionariocoloractual With {.Key = li.lngIDOrden, .Color = "Rojo", .Clase = li.strClase})
                                    Case Else
                                        diccionariocoloractual.Add(New diccionariocoloractual With {.Key = li.lngIDOrden, .Color = "Transparente", .Clase = li.strClase})
                                        'ViewActualizarLeo.dg.ColumnBackground = New BrushConverter().ConvertFromString("#FFFFFF") ' datarow.BackColor = Color.Transparent
                                End Select
                            End If
                        Next
                    End If
                End If

                ListaEstadosLEO = dcProxy.ListadoActualizarLEOs.ToList()

                MyBase.CambioItem("ListaEstadosLEO")
                MyBase.CambioItem("ListaActLeoPaged")
                AgrupacionDefectogrid()
                ViewActualizarLeo.dg.AutoExpandGroups = True
                ReiniciaTimer2()

            Else
                'JCS Marzo 13/2013 Se agrega manejo de error.
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del método",
                                 Me.ToString(), "TraerLeoCompleted", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
                'FIN JCS Marzo 13/2013 Se agrega manejo de error.
                IsBusy = False
            End If

            IsBusy = False
            IsBusyTimer = False
        Catch ex As Exception
            IsBusyTimer = False
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del método",
                                 Me.ToString(), "TraerLeoCompleted", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub


    Private Sub TraerConsultaOrdenCompleted(ByVal lo As InvokeOperation(Of String))
        Try
            If Not lo.HasError Then
                If Not IsNothing(lo.Value) Then
                    rchDetalleOrden = lo.Value.ToString()
                End If
            Else
                'JCS Marzo 13/2013 Se agrega manejo de error.
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del método",
                                     Me.ToString(), "TraerLeoCompleted", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
                'FIN JCS Marzo 13/2013 Se agrega manejo de error.
                IsBusy = False
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del método",
                                 Me.ToString(), "TraerConsultaOrdenCompleted", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private Sub TraerConsultaTipoOrdenCompleted(ByVal lo As InvokeOperation(Of String))
        Try
            If Not lo.HasError Then
                If Not IsNothing(lo.Value) Then
                    TipoOrdenGrid = lo.Value.ToString()
                End If
            Else
                'JCS Marzo 13/2013 Se agrega manejo de error.
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del método",
                                     Me.ToString(), "TraerLeoCompleted", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
                'FIN JCS Marzo 13/2013 Se agrega manejo de error.
                IsBusy = False
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del método",
                                 Me.ToString(), "TraerConsultaOrdenCompleted", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private Sub Terminotraerparametrolista(ByVal obj As InvokeOperation(Of String))
        If obj.HasError Then
            'JFSB 20171008 Se agrega propiedad para habilitar los botones de menú
            obj.MarkErrorAsHandled()
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la lista de parametros", Me.ToString(), "Terminotraerparametrolista", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            ValorParametro = obj.Value
        End If
    End Sub

    Private Sub AgrupacionDefectogrid()
        Dim descriptor As New GroupDescriptor()
        descriptor.Member = "strNombreTipo"
        descriptor.SortDirection = ListSortDirection.Ascending
        descriptor.DisplayContent = "Estado"
        If ViewActualizarLeo.dg.GroupDescriptors.Count = 0 Then
            ViewActualizarLeo.dg.GroupDescriptors.Add(descriptor)
        End If
        'You can create and add more descriptors here
    End Sub

    Private Sub ExpandirColapsarAgrupacion(ByVal plogAgrupar As Boolean)
        Try
            Dim objLista As New List(Of ListadoActualizarLEO)

            If Not IsNothing(ListaEstadosLEO) Then
                For Each li In ListaEstadosLEO
                    objLista.Add(li)
                Next
            End If

            If plogAgrupar = True Then
                ViewActualizarLeo.dg.AutoExpandGroups = False
            Else
                ViewActualizarLeo.dg.AutoExpandGroups = True
            End If


            ListaEstadosLEO = Nothing
            ListaEstadosLEO = objLista

            AgrupacionDefectogrid()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del método",
                                Me.ToString(), "ExpandirColapsarAgrupacion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoActualizarEstado(ByVal lo As InvokeOperation(Of Integer))
        Try
            IsBusy = False
            If lo.HasError Then
                logRealizarConsultaOrdenes = True

                Dim strMsg As String = String.Empty
                If (lo.Error.Message.Contains("ErrorPersonalizado,") = True) Then
                    Dim Mensaje1 = Split(lo.Error.Message, "ErrorPersonalizado,")
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    lo.MarkErrorAsHandled()
                    Exit Sub
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(lo.Error.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    lo.MarkErrorAsHandled()
                End If

                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    lo.MarkErrorAsHandled()
                End If
            Else
                IsBusy = True
                IsBusyTimer = True
                dcProxy.ListadoActualizarLEOs.Clear()
                dcProxy.Load(dcProxy.OrdenesLEO_ConsultarQuery(_strLogin, _strValoresCamposDescLEO, _dtmFecha, _strIDReceptor, Program.Usuario, Program.HashConexion), AddressOf TraerLeoCompleted, "")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al término de la llamada al proceso de actualización de LEOS.", Me.ToString(), "TerminoActualizarEstado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaEstadosLEO As List(Of ListadoActualizarLEO)
    Public Property ListaEstadosLEO() As List(Of ListadoActualizarLEO)
        Get
            Return _ListaEstadosLEO
        End Get
        Set(ByVal value As List(Of ListadoActualizarLEO))
            _ListaEstadosLEO = value
            MyBase.CambioItem("ListaEstadosLEO")
            MyBase.CambioItem("ListaActLeoPaged")
        End Set
    End Property


    Private _ListaEstadosLEOSeleccionada As ListadoActualizarLEO
    Public Property ListaEstadosLEOSeleccionada() As ListadoActualizarLEO
        Get
            Return _ListaEstadosLEOSeleccionada
        End Get
        Set(ByVal value As ListadoActualizarLEO)
            _ListaEstadosLEOSeleccionada = value
            If Not IsNothing(_ListaEstadosLEOSeleccionada) Then
                dcProxy1.OrdenesLEO_DatosOrden(ActualizarLeoSelected.strTipo, ActualizarLeoSelected.strClase, ActualizarLeoSelected.lngIDOrden, ActualizarLeoSelected.strIDReceptor, ActualizarLeoSelected.strLEO, Program.Usuario, Program.HashConexion, AddressOf TraerConsultaOrdenCompleted, "")
            End If
            ' dcProxy1.Load(dcProxy.OrdenesLEO_DatosOrdenQuery(_ListaEstadosLEOSeleccionada.,), AddressOf TraerLeoCompleted, "")
            MyBase.CambioItem("ListaEstadosLEOSeleccionada")
            MyBase.CambioItem("ListaActLeoPaged")
        End Set
    End Property

    Private _ListaActLeoPaged As PagedCollectionView
    Public Property ListaActLeoPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEstadosLEO) Then
                Dim _ListaActLeoPaged = New PagedCollectionView(_ListaEstadosLEO)
                Return _ListaActLeoPaged
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As PagedCollectionView)
            _ListaActLeoPaged = value
            MyBase.CambioItem("ListaActLeoPaged")
        End Set
    End Property


    Private WithEvents _ActualizarLeoSelected As ListadoActualizarLEO
    Public Property ActualizarLeoSelected() As ListadoActualizarLEO
        Get
            Return _ActualizarLeoSelected
        End Get
        Set(ByVal value As ListadoActualizarLEO)
            _ActualizarLeoSelected = value
            If Not IsNothing(_ActualizarLeoSelected) Then
                dcProxy1.OrdenesLEO_DatosOrden(ActualizarLeoSelected.strTipo, ActualizarLeoSelected.strClase, ActualizarLeoSelected.lngIDOrden, ActualizarLeoSelected.strIDReceptor, ActualizarLeoSelected.strLEO, Program.Usuario, Program.HashConexion, AddressOf TraerConsultaOrdenCompleted, "")
            End If
            MyBase.CambioItem("ActualizarLeoSelected")
        End Set
    End Property

    Private _strValoresCamposDescLEO As String
    Public Property ValoresCamposDescLEO() As String
        Get
            Return _strValoresCamposDescLEO
        End Get
        Set(ByVal value As String)
            _strValoresCamposDescLEO = value
            MyBase.CambioItem("ValoresCamposDescLEO")
        End Set
    End Property

    Private _strLogin As String
    Public Property Login() As String
        Get
            Return _strLogin
        End Get
        Set(ByVal value As String)
            _strLogin = value
            MyBase.CambioItem("Login")
        End Set
    End Property

    Private _strIDReceptor As String
    Public Property IDReceptor() As String
        Get
            Return _strIDReceptor
        End Get
        Set(ByVal value As String)
            _strIDReceptor = value
            MyBase.CambioItem("IDReceptor")
        End Set
    End Property


    Private _lblUsuario As String = Program.Usuario
    Public Property lblUsuario() As String
        Get
            Return _lblUsuario
        End Get
        Set(ByVal value As String)
            _lblUsuario = value
            MyBase.CambioItem("lblUsuario")
        End Set
    End Property


    Private _dtmFecha As Date
    Public Property Fecha() As Date
        Get
            Return _dtmFecha
        End Get
        Set(ByVal value As Date)
            _dtmFecha = Date.Now
            MyBase.CambioItem("Fecha")
        End Set
    End Property

    Private _lblFecha As DateTime
    Public Property lblFecha() As DateTime
        Get
            Return _lblFecha
        End Get
        Set(ByVal value As DateTime)
            _lblFecha = value
            MyBase.CambioItem("lblFecha")
        End Set
    End Property


    Private _lblHora As DateTime
    Public Property lblHora() As DateTime
        Get
            Return _lblHora
        End Get
        Set(ByVal value As DateTime)
            _lblHora = value
            MyBase.CambioItem("lblHora")
        End Set
    End Property

    Private _rchDetalleOrden As String
    Public Property rchDetalleOrden() As String
        Get
            Return _rchDetalleOrden
        End Get
        Set(ByVal value As String)
            _rchDetalleOrden = value
            MyBase.CambioItem("rchDetalleOrden")
        End Set
    End Property

    Private _TipoOrdenGrid As String
    Public Property TipoOrdenGrid() As String
        Get
            Return _TipoOrdenGrid
        End Get
        Set(ByVal value As String)
            _TipoOrdenGrid = value
            MyBase.CambioItem("TipoOrdenGrid")
        End Set
    End Property

    Private _lblInfoAuto As String
    Public Property lblInfoAuto() As String
        Get
            Return _lblInfoAuto
        End Get
        Set(ByVal value As String)
            _lblInfoAuto = value
            MyBase.CambioItem("lblInfoAuto")
        End Set
    End Property

    Private _ExpandirGrid As Boolean = True
    Public Property ExpandirGrid As Boolean
        Get
            Return _ExpandirGrid
        End Get
        Set(ByVal value As Boolean)
            _ExpandirGrid = value
            MyBase.CambioItem("ExpandirGrid")
        End Set
    End Property

    Private _chkOcultar As Boolean = False
    Public Property chkOcultar As Boolean
        Get
            Return _chkOcultar
        End Get
        Set(ByVal value As Boolean)
            _chkOcultar = value
            MyBase.CambioItem("chkOcultar")
            ExpandirColapsarAgrupacion(_chkOcultar)
        End Set
    End Property

    Private _MostrarCambiarEstado As Boolean = False
    Public Property MostrarCambiarEstado As Boolean
        Get
            Return _MostrarCambiarEstado
        End Get
        Set(ByVal value As Boolean)
            _MostrarCambiarEstado = value
            MyBase.CambioItem("MostrarCambiarEstado")
        End Set
    End Property

    Private _ObjMarcar As Boolean = False
    Public Property ObjMarcar As Boolean
        Get
            Return _ObjMarcar
        End Get
        Set(ByVal value As Boolean)
            _ObjMarcar = value
            If _ObjMarcar = True Then
                MostrarCambiarEstado = True
            Else
                MostrarCambiarEstado = False
            End If
            MyBase.CambioItem("ObjMarcar")
        End Set
    End Property

    Private _chkUndo As Boolean = False
    Public Property chkUndo As Boolean
        Get
            Return _chkUndo
        End Get
        Set(ByVal value As Boolean)
            _chkUndo = value

            MyBase.CambioItem("chkUndo")
        End Set
    End Property


    Private WithEvents _diccionariocoloractual As List(Of diccionariocoloractual)
    Public Property diccionariocoloractual() As List(Of diccionariocoloractual)
        Get
            Return _diccionariocoloractual
        End Get
        Set(ByVal value As List(Of diccionariocoloractual))
            _diccionariocoloractual = value
            ' RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("_diccionariocoloractual"))
        End Set
    End Property

    Private _IsBusyTimer As Boolean = False
    Public Property IsBusyTimer() As Boolean
        Get
            Return _IsBusyTimer
        End Get
        Set(ByVal value As Boolean)
            _IsBusyTimer = value
            MyBase.CambioItem("IsBusyTimer")
        End Set
    End Property

#End Region


#Region "Timer Refrescar pantalla"

    Private _myDispatcherTimerOrdenes As System.Windows.Threading.DispatcherTimer '= New System.Windows.Threading.DispatcherTimer

    Public Sub ReiniciaTimer()
        Try
            If _myDispatcherTimerOrdenes Is Nothing Then
                _myDispatcherTimerOrdenes = New System.Windows.Threading.DispatcherTimer
                _myDispatcherTimerOrdenes.Interval = New TimeSpan(0, 0, 1)
                AddHandler _myDispatcherTimerOrdenes.Tick, AddressOf Me.Each_Tick
            End If
            _myDispatcherTimerOrdenes.Start()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar reiniciar el timer.", Me.ToString, "ReiniciaTimer", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub

    Private _myDispatcherTimerOrdenes2 As System.Windows.Threading.DispatcherTimer '= New System.Windows.Threading.DispatcherTimer
    Public Sub ReiniciaTimer2()
        Try
            If ValorParametro <> "0" And Not String.IsNullOrEmpty(ValorParametro) Then
                logRealizarConsultaOrdenes = True

                If _myDispatcherTimerOrdenes2 Is Nothing Then
                    _myDispatcherTimerOrdenes2 = New System.Windows.Threading.DispatcherTimer
                    _myDispatcherTimerOrdenes2.Interval = New TimeSpan(0, 0, Convert.ToInt32(ValorParametro))
                    lblInfoAuto = "Actualización pantalla cada " & ValorParametro & " Segundos"
                    AddHandler _myDispatcherTimerOrdenes2.Tick, AddressOf Me.Each_Tick2
                End If
                _myDispatcherTimerOrdenes2.Start()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar reiniciar el timer.", Me.ToString, "ReiniciaTimer2", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try

    End Sub
    Private Sub Each_Tick2(sender As Object, e As EventArgs)
        Try
            If logRealizarConsultaOrdenes Then
                If Not IsNothing(ListaEstadosLEO) Then
                    If ListaEstadosLEO.Where(Function(i) i.ObjMarcar).Count > 0 Then
                        Exit Sub
                    End If
                End If

                pararTemporizador2()
                IsBusyTimer = True
                logRealizarConsultaOrdenes = False
                dcProxy.ListadoActualizarLEOs.Clear()
                dcProxy.Load(dcProxy.OrdenesLEO_ConsultarQuery(_strLogin, _strValoresCamposDescLEO, _dtmFecha, _strIDReceptor, Program.Usuario, Program.HashConexion), AddressOf TraerLeoCompleted, "")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar reiniciar el timer.", Me.ToString, "Each_Tick2", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Para hilo del temporizador
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub pararTemporizador()
        Try
            If Not IsNothing(_myDispatcherTimerOrdenes) Then
                _myDispatcherTimerOrdenes.Stop()
                RemoveHandler _myDispatcherTimerOrdenes.Tick, AddressOf Me.Each_Tick
                _myDispatcherTimerOrdenes = Nothing
            End If

            If Not IsNothing(_myDispatcherTimerOrdenes2) Then
                _myDispatcherTimerOrdenes2.Stop()
                RemoveHandler _myDispatcherTimerOrdenes2.Tick, AddressOf Me.Each_Tick2
                _myDispatcherTimerOrdenes2 = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar parar el timer.", Me.ToString, "pararTemporizador", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Para hilo del temporizador
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub pararTemporizador2()
        Try
            If Not IsNothing(_myDispatcherTimerOrdenes2) Then
                _myDispatcherTimerOrdenes2.Stop()
                RemoveHandler _myDispatcherTimerOrdenes2.Tick, AddressOf Me.Each_Tick2
                _myDispatcherTimerOrdenes2 = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al intentar parar el timer.", Me.ToString, "pararTemporizador", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub Each_Tick(sender As Object, e As EventArgs)
        lblFecha = DateTime.Now
    End Sub

#End Region


#Region "Métodos"

    Sub ConsultarLeo()
        Try
            IsBusy = True
            IsBusyTimer = True
            dcProxy.ListadoActualizarLEOs.Clear()
            dcProxy.Load(dcProxy.OrdenesLEO_ConsultarQuery(_strLogin, _strValoresCamposDescLEO, _dtmFecha, _strIDReceptor, Program.Usuario, Program.HashConexion), AddressOf TraerLeoCompleted, "")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del método",
                                 Me.ToString(), "EntregaDeCustodiasViewModel.ConsultarLeo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Sub CambiarEstadoLeo()
        Try
            'Dim objLista As New List(Of ListadoActualizarLEO)

            If Not IsNothing(ListaEstadosLEO) Then
                If ListaEstadosLEO.Count > 0 Then
                    For Each li In ListaEstadosLEO
                        If li.ObjMarcar = True Then
                            MostrarCambiarEstado = True
                            logRealizarConsultaOrdenes = False
                            EstadosColasLEO = New EstadosColasLEO(_ActualizarLeoSelected.lngIDOrden, _ActualizarLeoSelected.strTipo, _ActualizarLeoSelected.strClase, _ActualizarLeoSelected.strEstado, chkUndo, Me)
                            Program.Modal_OwnerMainWindowsPrincipal(EstadosColasLEO)
                            EstadosColasLEO.ShowDialog()
                        End If
                    Next
                End If
                'For Each li In ListaEstadosLEO
                '    'objLista.Add(li)
                'Next
            End If


        Catch ex As Exception
            logRealizarConsultaOrdenes = True
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del método",
                                 Me.ToString(), "EntregaDeCustodiasViewModel.ConsultarLeo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Sub ActualizarEstado(ByVal plngIDOrden As Integer, ByVal pstrTipo As String, ByVal pstrClase As String, ByVal pstrEstadoLEO As String, ByVal pstrUsuarioLEO As String, ByVal pdtmFechaEstadoLEO As Date)
        Try
            logRealizarConsultaOrdenes = False
            IsBusy = True
            dcProxy2.OrdenesEstadoLEO_Actualizar(plngIDOrden, pstrTipo, pstrClase, pstrEstadoLEO, chkUndo, pdtmFechaEstadoLEO, pstrUsuarioLEO, Program.Usuario, Program.HashConexion, AddressOf TerminoActualizarEstado, "")
        Catch ex As Exception
            logRealizarConsultaOrdenes = True
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la ejecución del método",
                                 Me.ToString(), "EntregaDeCustodiasViewModel.ConsultarLeo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub



#End Region

    Private Sub _ActualizarLeoSelected_PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Handles _ActualizarLeoSelected.PropertyChanged
        If Not IsNothing(ListaEstadosLEO) Then
            If ListaEstadosLEO.Count > 0 Then
                For Each li In ListaEstadosLEO
                    If li.ObjMarcar = True Then
                        MostrarCambiarEstado = True
                        'EstadosColasLEO = New EstadosColasLEO()
                        'Program.Modal_OwnerMainWindowsPrincipal(EstadosColasLEO)
                        'EstadosColasLEO.ShowDialog()
                    End If
                Next
            End If
        End If
    End Sub

End Class

Public Class diccionariocoloractual
    Public Property Key As Nullable(Of Integer)
    Public Property Color As String
    Public Property Clase As String
End Class

Public Class diccionariocoloranterior
    Public Property Key As Nullable(Of Integer)
    Public Property Color As String
    Public Property Clase As String
End Class




