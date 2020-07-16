Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones
Imports System.ComponentModel
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data

Partial Public Class cwMovimientosDecevalView
    Inherits Window
    Implements INotifyPropertyChanged

#Region "Variables"

    Dim cwDetalleMovimientosDecevalView As cwDetalleMovimientosDecevalView
    Dim intClientesActivosNoBloqueados As Integer = 0
    Private mdcProxy As ImportacionesDomainContext
    Dim intCargas As Integer = 0
    Dim intDescargas As Integer = 0
    Dim intBloqDes As Integer = 0
    Dim intLeidos As Integer = 0
    Public ViewImportarArchivo As New cwCargaArchivos
    Dim objListaMensajes As New List(Of String)

#End Region

#Region "Propiedades"

    Private _IsBusy As Boolean = False
    Public Property IsBusy As Boolean
        Get
            Return _IsBusy
        End Get
        Set(ByVal value As Boolean)
            _IsBusy = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsBusy"))
        End Set
    End Property

    ''' <summary>
    ''' Objeto que captura los valores seleccionados por el usuario en la forma de búsqueda
    ''' </summary>
    Private WithEvents _ListaTitulosMovimientos As List(Of TitulosMovimientos)
    Public Property ListaTitulosMovimientos() As List(Of TitulosMovimientos)
        Get
            Return _ListaTitulosMovimientos
        End Get
        Set(ByVal value As List(Of TitulosMovimientos))
            _TitulosMovimientosPaginada = Nothing
            _ListaTitulosMovimientos = value

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaTitulosMovimientos"))
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TitulosMovimientosPaginada"))
        End Set
    End Property

    ''' <summary>
    ''' Colección que pagina la lista de MovimientosParticipacionesFondos para navegar sobre el grid con paginación
    ''' </summary>
    Private _TitulosMovimientosPaginada As PagedCollectionView = Nothing
    Public ReadOnly Property TitulosMovimientosPaginada() As PagedCollectionView
        Get
            If Not IsNothing(_ListaTitulosMovimientos) Then
                If IsNothing(_TitulosMovimientosPaginada) Then
                    Dim view = New PagedCollectionView(_ListaTitulosMovimientos)
                    _TitulosMovimientosPaginada = view
                    Return view
                Else
                    Return (_TitulosMovimientosPaginada)
                End If
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Lista de movimientos deceval que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaClientesActivosNoBloqueados As List(Of OyDImportaciones.ClientesActivosNoBloqueados)
    Public Property ListaClientesActivosNoBloqueados() As List(Of OyDImportaciones.ClientesActivosNoBloqueados)
        Get
            Return _ListaClientesActivosNoBloqueados
        End Get
        Set(ByVal value As List(Of OyDImportaciones.ClientesActivosNoBloqueados))
            _ListaClientesActivosNoBloqueados = value

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaClientesActivosNoBloqueados"))
        End Set
    End Property

    ''' <summary>
    ''' Indica cuál de los detalles está seleccionado
    ''' </summary>
    Private WithEvents _DetalleSeleccionado As TitulosMovimientos
    Public Property DetalleSeleccionado() As TitulosMovimientos
        Get
            Return _DetalleSeleccionado
        End Get
        Set(ByVal value As TitulosMovimientos)
            _DetalleSeleccionado = value

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DetalleSeleccionado"))
        End Set
    End Property

    ''' <summary>
    ''' Lista de detalles de la entidad en este caso detalle de codificacion de activos
    ''' </summary>
    Private _ListaDetalle As List(Of TitulosMovimientos)
    Public Property ListaDetalle() As List(Of TitulosMovimientos)
        Get
            Return _ListaDetalle
        End Get
        Set(ByVal value As List(Of TitulosMovimientos))
            _ListaDetalle = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaDetalle"))
        End Set
    End Property

    Private _HabilitarEdicionDetalle As Boolean = False
    Public Property HabilitarEdicionDetalle As Boolean
        Get
            Return _HabilitarEdicionDetalle
        End Get
        Set(ByVal value As Boolean)
            _HabilitarEdicionDetalle = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitarEdicionDetalle"))
        End Set
    End Property

    Private _ListaResultados As List(Of String) = New List(Of String)
    Public Property ListaResultados() As List(Of String)
        Get
            Return _ListaResultados
        End Get
        Set(ByVal value As List(Of String))
            _ListaResultados = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaResultados"))
        End Set
    End Property

#End Region

#Region "Inicializacion"

    ''' <summary>
    ''' Inicializa la ventana para cobro de utilidades (estilos y consulta de los registros)
    ''' </summary>
    Public Sub New(ByVal pstrMovimientosDeceval As String)
        Try
            CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.EncuentaContabilidad)
        Catch ex As Exception
            MessageBox.Show("ATENCIÓN: No fue posible aplicar los estilos propios de la aplicación." & vbNewLine & vbNewLine & "Se generó el error ''" & ex.Message & "''" & vbNewLine & vbNewLine & "Procedimiento: ", "Error de aplicación", MessageBoxButton.OK)
        End Try

        InitializeComponent()
        Me.DataContext = Me

        Convertir_A_Filas(pstrMovimientosDeceval)

    End Sub

#End Region

#Region "Métodos para control de eventos"

    Private Sub btnAceptar_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(ListaTitulosMovimientos) Then
                If ListaTitulosMovimientos.Count > 0 Then
                    A2Utilidades.Mensajes.mostrarMensajePregunta("¿ Desea realizar los movimientos ?", Program.TituloSistema, ValoresUserState.Borrar.ToString(), AddressOf ConfirmaRealizarMovimientos)
                End If
            End If
            'Me.DialogResult = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón aceptar.", Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As RoutedEventArgs)
        Try
            'Me.Close()
            Me.DialogResult = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el evento click del botón cerrar.", Me.ToString(), "btnAceptar_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Métodos privados del detalle - REQUERIDO (si hay detalle)"

    Private Async Sub Convertir_A_Filas(ByVal pstrMovimientosDeceval As String)
        Try
            Dim strLinea As String()
            Dim strFila As String()
            Dim intContador As Integer = 1

            strLinea = pstrMovimientosDeceval.Split(vbNewLine)
            Dim objLista As New List(Of TitulosMovimientos)

            IsBusy = True
            For Each mFila In strLinea
                strFila = mFila.Split(";")

                If strFila.Length > 1 Then

                    Dim objClaseTitulosMovimientos As New TitulosMovimientos

                    objClaseTitulosMovimientos.intIDTitulosMovimientos = intContador
                    objClaseTitulosMovimientos.logLineaSeleccionada = False
                    objClaseTitulosMovimientos.intLineaArchivo = strFila(0).ToString
                    objClaseTitulosMovimientos.strIDEspecie = strFila(1).ToString
                    objClaseTitulosMovimientos.strISIN = strFila(2).ToString
                    objClaseTitulosMovimientos.lngIDFungible = strFila(3).ToString
                    objClaseTitulosMovimientos.dblNroCuenta = strFila(4).ToString
                    objClaseTitulosMovimientos.lngIDComitente = strFila(5).ToString
                    objClaseTitulosMovimientos.dtmMovimiento = strFila(6).ToString
                    objClaseTitulosMovimientos.dblCantidad = strFila(7).ToString
                    objClaseTitulosMovimientos.strNombre = strFila(8).ToString
                    objClaseTitulosMovimientos.strNroDocumento = strFila(9).ToString
                    objClaseTitulosMovimientos.dblVlrValorizado = strFila(10).ToString
                    objClaseTitulosMovimientos.strEstadoEntrada = strFila(11).ToString
                    objClaseTitulosMovimientos.strEstadoSalida = strFila(12).ToString
                    objClaseTitulosMovimientos.strTipo = strFila(16).ToString
                    objClaseTitulosMovimientos.strDescripcionMovimiento = strFila(17).ToString

                    Await ClientesActivosNoBloqueados(objClaseTitulosMovimientos.dblNroCuenta)

                    'Comparar si solo tiene un cuenta o varias, si es solo una, debera marcar la primera columna
                    If intClientesActivosNoBloqueados = 1 Then objClaseTitulosMovimientos.logLineaSeleccionada = True

                    objLista.Add(objClaseTitulosMovimientos)

                    intContador = intContador + 1

                End If
            Next

            ListaTitulosMovimientos = objLista

            Await RealizarMovimiento()

            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el método consultar clientes activos no bloqueados. ", Me.ToString(), "ClientesActivosNoBloqueados", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Async Sub ConfirmaRealizarMovimientos(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyImportaciones()
            End If

            If CType(sender, A2Utilidades.wppMensajePregunta).DialogResult Then

                Await RealizarMovimiento()

            End If
            IsBusy = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al confirmar para realizar los movimientos.", Me.ToString(), "ConfirmaRealizarMovimientos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario da clic en el botón Editar del detalle 
    ''' </summary>
    Private Async Sub ActualizarDetalle_Click(sender As Object, e As RoutedEventArgs)
        Try
            If Not IsNothing(DetalleSeleccionado) Then

                Await ClientesActivosNoBloqueados(DetalleSeleccionado.dblNroCuenta)

                cwDetalleMovimientosDecevalView = New cwDetalleMovimientosDecevalView(Me, DetalleSeleccionado, HabilitarEdicionDetalle, ListaClientesActivosNoBloqueados)
                Program.Modal_OwnerMainWindowsPrincipal(cwDetalleMovimientosDecevalView)
                cwDetalleMovimientosDecevalView.ShowDialog()

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el movimiento.", _
                               Me.ToString(), "ActualizarDetalle_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private Sub chkSeleccionar_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim chk As CheckBox = CType(sender, CheckBox)
            Dim check As Boolean = chk.IsChecked.Value

            If ListaTitulosMovimientos IsNot Nothing Then
                For Each it In ListaTitulosMovimientos
                    it.logLineaSeleccionada = check
                Next
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaTitulosMovimientos"))
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la anulación del proceso.", Me.Name, "chkAnular_Click", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

#Region "Métodos públicos del detalle - REQUERIDO (si hay detalle)"

    Public Async Function ClientesActivosNoBloqueados(ByVal plngIDCuenta As Integer) As Task
        Try
            Dim objRet As LoadOperation(Of ClientesActivosNoBloqueados)
            Dim mdcProxy As ImportacionesDomainContext

            mdcProxy = inicializarProxyImportaciones()

            mdcProxy.ClientesActivosNoBloqueados.Clear()

            objRet = Await mdcProxy.Load(mdcProxy.ClientesActivosNoBloqueados_ConsultarSyncQuery(plngIDCuenta, "D", Program.Usuario, Program.HashConexion)).AsTask()

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de códigos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If objRet.Entities.Count > 0 Then
                        ListaClientesActivosNoBloqueados = mdcProxy.ClientesActivosNoBloqueados.ToList
                        intClientesActivosNoBloqueados = ListaClientesActivosNoBloqueados.Count
                    End If
                End If
            Else
                Dim objtest As String = String.Empty
                objtest = "test"
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el método consultar clientes activos no bloqueados. ", Me.ToString(), "ClientesActivosNoBloqueados", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Function

    ''' <summary>
    ''' Función utilizada para importar el archivo Deceval.
    ''' </summary>
    ''' <history>
    ''' ID caso de prueba:  CP0010
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              8 de Septiembre/2015
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 8 de Septiembre/2015
    ''' </history> 
    Public Sub CargarArchivo(pstrModulo As String, pstrNombreCompletoArchivo As String)
        Try
            ViewImportarArchivo.IsBusy = True

            mdcProxy.RespuestaArchivoImportacions.Clear()
            ListaResultados.Clear()

            For Each li In objListaMensajes
                ListaResultados.Add(li)
            Next

            ListaResultados.Add(vbNewLine & "Leídos: " & intLeidos.ToString)
            ListaResultados.Add("Cargados: " & intCargas.ToString)
            ListaResultados.Add("Descargados: " & intDescargas.ToString)
            ListaResultados.Add("Bloqueados / Desbloqueados: " & intBloqDes.ToString)

            ViewImportarArchivo.ListaMensajes = ListaResultados

            ViewImportarArchivo.IsBusy = False
            intLeidos = 0
            intCargas = 0
            intDescargas = 0
            intBloqDes = 0

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al visualizar el resultado del movimiento.", _
                               Me.ToString(), "CargarArchivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Async Function RealizarMovimiento() As Task
        Try

            Dim objRealizarMovimientos As LoadOperation(Of RespuestaArchivoImportacion)

            IsBusy = True

            Dim NuevaListaTitulosMovimientos As New List(Of TitulosMovimientos)

            Dim intContador As Integer = 1

            If mdcProxy Is Nothing Then
                mdcProxy = inicializarProxyImportaciones()
            End If

            objListaMensajes.Clear()

            intLeidos = ListaTitulosMovimientos.Count

            For Each Lista In ListaTitulosMovimientos
                If Lista.logLineaSeleccionada Then

                    mdcProxy.RespuestaArchivoImportacions.Clear()

                    objRealizarMovimientos = Await mdcProxy.Load(mdcProxy.MovimientosArchivoCambio_ProcesarSyncQuery(Lista.intLineaArchivo, Lista.lngIDComitente, Lista.strEstadoEntrada,
                                                                                                  Lista.strEstadoSalida, Lista.strTipo, Nothing, Nothing, Program.Usuario, Program.HashConexion)).AsTask


                    If Not objRealizarMovimientos Is Nothing Then
                        If objRealizarMovimientos.HasError Then
                            If objRealizarMovimientos.Error Is Nothing Then
                                A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema en la línea: " + Lista.intLineaArchivo, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                            End If

                            objRealizarMovimientos.MarkErrorAsHandled()
                        ElseIf objRealizarMovimientos.Entities.Count = 0 Then

                            If Lista.strTipo = "C" Then
                                intCargas = intCargas + 1
                            ElseIf Lista.strTipo = "V" Then
                                intDescargas = intDescargas + 1
                            ElseIf Lista.strTipo = "B" Then
                                intBloqDes = intBloqDes + 1
                            End If

                        ElseIf objRealizarMovimientos.Entities.Count > 0 Then
                            Dim objListaRespuesta As List(Of RespuestaArchivoImportacion)

                            objListaRespuesta = objRealizarMovimientos.Entities.ToList

                            For Each li In objListaRespuesta '.Where(Function(i) CType(i.Exitoso, Boolean))

                                objListaMensajes.Add(li.Mensaje)
                            Next

                            ViewImportarArchivo.ListaMensajes = objListaMensajes
                            
                            NuevaListaTitulosMovimientos.Add(Lista)
                        End If

                    End If

                Else
                    NuevaListaTitulosMovimientos.Add(Lista)
                End If
            Next

            ListaTitulosMovimientos = NuevaListaTitulosMovimientos

            Dim viewImportacion As New cwCargaArchivos(CType(Me.DataContext, cwMovimientosDecevalView), String.Empty, String.Empty)
            Program.Modal_OwnerMainWindowsPrincipal(viewImportacion)
            viewImportacion.ShowDialog()

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al confirmar para realizar los movimientos.", Me.ToString(), "RealizarMovimiento", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

#End Region

#Region "Manejadores error"

    Private Sub dgGrid_BindingValidationError(ByVal sender As Object, ByVal e As System.Windows.Controls.ValidationErrorEventArgs)
        Try
            ' Control de error del bindding del grid
            If Not e.Error.Exception Is Nothing Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema para presentar los datos del detalle.", Me.Name, "dgGrid_BindingValidationError", Program.TituloSistema, Program.Maquina, e.Error.Exception)
            End If
            e.Handled = True
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga de los datos", Me.Name, "dgGrid_BindingValidationError", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
        'App.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    End Sub

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

#End Region

End Class

Public Class TitulosMovimientos
    Implements INotifyPropertyChanged

    Private _intIDTitulosMovimientos As System.Nullable(Of Integer)
    Public Property intIDTitulosMovimientos() As System.Nullable(Of Integer)
        Get
            Return _intIDTitulosMovimientos
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _intIDTitulosMovimientos = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intIDTitulosMovimientos"))
        End Set
    End Property

    Private _logLineaSeleccionada As System.Nullable(Of System.Boolean)
    Public Property logLineaSeleccionada() As System.Nullable(Of System.Boolean)
        Get
            Return _logLineaSeleccionada
        End Get
        Set(ByVal value As System.Nullable(Of System.Boolean))
            _logLineaSeleccionada = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logLineaSeleccionada"))
        End Set
    End Property

    Private _intLineaArchivo As System.Nullable(Of Integer)
    Public Property intLineaArchivo() As System.Nullable(Of Integer)
        Get
            Return _intLineaArchivo
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _intLineaArchivo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("intLineaArchivo"))
        End Set
    End Property

    Private _strIDEspecie As String
    Public Property strIDEspecie() As String
        Get
            Return _strIDEspecie
        End Get
        Set(ByVal value As String)
            _strIDEspecie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strIDEspecie"))
        End Set
    End Property

    Private _strISIN As String
    Public Property strISIN() As String
        Get
            Return _strISIN
        End Get
        Set(ByVal value As String)
            _strISIN = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strISIN"))
        End Set
    End Property

    Private _lngIDFungible As System.Nullable(Of Integer)
    Public Property lngIDFungible() As System.Nullable(Of Integer)
        Get
            Return _lngIDFungible
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _lngIDFungible = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDFungible"))
        End Set
    End Property

    Private _dblNroCuenta As System.Nullable(Of Double)
    Public Property dblNroCuenta() As System.Nullable(Of Double)
        Get
            Return _dblNroCuenta
        End Get
        Set(ByVal value As System.Nullable(Of Double))
            _dblNroCuenta = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblNroCuenta"))
        End Set
    End Property

    Private _lngIDComitente As String
    Public Property lngIDComitente() As String
        Get
            Return _lngIDComitente
        End Get
        Set(ByVal value As String)
            _lngIDComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("lngIDComitente"))
        End Set
    End Property

    Private _strNroDocumento As String
    Public Property strNroDocumento() As String
        Get
            Return _strNroDocumento
        End Get
        Set(ByVal value As String)
            _strNroDocumento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strNroDocumento"))
        End Set
    End Property

    Private _strNombre As String
    Public Property strNombre() As String
        Get
            Return _strNombre
        End Get
        Set(ByVal value As String)
            _strNombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strNombre"))
        End Set
    End Property

    Private _dtmMovimiento As System.Nullable(Of System.DateTime)
    Public Property dtmMovimiento() As System.Nullable(Of System.DateTime)
        Get
            Return _dtmMovimiento
        End Get
        Set(ByVal value As System.Nullable(Of System.DateTime))
            _dtmMovimiento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dtmMovimiento"))
        End Set
    End Property

    Private _dblCantidad As System.Nullable(Of Double)
    Public Property dblCantidad() As System.Nullable(Of Double)
        Get
            Return _dblCantidad
        End Get
        Set(ByVal value As System.Nullable(Of Double))
            _dblCantidad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblCantidad"))
        End Set
    End Property

    Private _dblVlrValorizado As System.Nullable(Of Double)
    Public Property dblVlrValorizado() As System.Nullable(Of Double)
        Get
            Return _dblVlrValorizado
        End Get
        Set(ByVal value As System.Nullable(Of Double))
            _dblVlrValorizado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("dblVlrValorizado"))
        End Set
    End Property

    Private _strEstadoEntrada As String
    Public Property strEstadoEntrada() As String
        Get
            Return _strEstadoEntrada
        End Get
        Set(ByVal value As String)
            _strEstadoEntrada = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strEstadoEntrada"))
        End Set
    End Property

    Private _strEstadoSalida As String
    Public Property strEstadoSalida() As String
        Get
            Return _strEstadoSalida
        End Get
        Set(ByVal value As String)
            _strEstadoSalida = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strEstadoSalida"))
        End Set
    End Property

    Private _strTipo As String
    Public Property strTipo() As String
        Get
            Return _strTipo
        End Get
        Set(ByVal value As String)
            _strTipo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipo"))
        End Set
    End Property

    Private _strDescripcionMovimiento As String
    Public Property strDescripcionMovimiento() As String
        Get
            Return _strDescripcionMovimiento
        End Get
        Set(ByVal value As String)
            _strDescripcionMovimiento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDescripcionMovimiento"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class