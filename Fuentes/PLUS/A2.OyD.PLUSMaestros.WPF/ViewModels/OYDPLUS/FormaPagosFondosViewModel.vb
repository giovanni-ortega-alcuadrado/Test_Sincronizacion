Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: FormaPagosViewModel.vb
'Generado el : 11/15/2012 07:29:09
'Propiedad de Alcuadrado S.A. 2010

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSMaestros
Imports OpenRiaServices.DomainServices.Client

Public Class FormaPagosFondosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Private FormaPagoPorDefecto As tblFondosFormasPagos
    Private FormaPagoAnterior As tblFondosFormasPagos
    Dim dcProxy As OyDPLUSMaestrosDomainContext
    Dim dcProxy1 As OyDPLUSMaestrosDomainContext
    Dim objProxy As OyDPLUSutilidadesDomainContext
    Dim IdItemActualizar As Integer = 0
    Private _MODULOORDNES As String = String.Empty
    Private _MODULOORDNESTESORERIA As String = String.Empty

    Public Sub New()
        Try

            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OyDPLUSMaestrosDomainContext()
                dcProxy1 = New OyDPLUSMaestrosDomainContext()
                objProxy = New OyDPLUSutilidadesDomainContext()
                _HoraInicioPermitida = "07:00"
                _HoraFinPermitida = "18:00"
                _MODULOORDNES = "O"
                _MODULOORDNESTESORERIA = "OT"
            Else
                dcProxy = New OyDPLUSMaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy1 = New OyDPLUSMaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
                objProxy = New OyDPLUSutilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYDPLUS))
                _HoraInicioPermitida = Program.RetornarValorProgram(Program.HoraInicio_Horario, "07:00")
                _HoraFinPermitida = Program.RetornarValorProgram(Program.HoraFin_Horario, "18:00")

                _MODULOORDNES = Program.RetornarValorProgram(Program.Modulo_Ordenes, "O")
                _MODULOORDNESTESORERIA = Program.RetornarValorProgram(Program.Modulo_OrdenesTesoreria, "OT")
            End If

            If _HoraInicioPermitida = "24:00" Then
                _HoraInicioPermitida = "00:00"
            ElseIf _HoraInicioPermitida = "00:00" Then
                _HoraInicioPermitida = "00:01"
            End If

            If _HoraFinPermitida = "24:00" Or _HoraFinPermitida = "23:59" Then
                _HoraFinPermitida = "23:58"
            End If

            Dim _HoraInicio As TimeSpan = TimeSpan.Parse(_HoraInicioPermitida)
            Dim _HoraFin As TimeSpan = TimeSpan.Parse(_HoraFinPermitida)

            _HoraInicio = _HoraInicio.Add(TimeSpan.FromMinutes(-1))
            _HoraFin = _HoraFin.Add(TimeSpan.FromMinutes(1))

            _HoraInicioPermitida = _HoraInicio.ToString().Substring(0, 5)
            _HoraFinPermitida = _HoraFin.ToString().Substring(0, 5)

            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.FormasPagosFondosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerFormaPago, "")
                'dcProxy1.Load(dcProxy1.TraerFormaPagoFondosPorDefectoQuery, AddressOf TerminoTraerFormaPagosPorDefecto_Completed, "Default")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "FormaPagosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Function RetornarValorProgram(ByVal strProgram As String, ByVal strRetornoOpcional As String)
        Dim objRetorno As String = String.Empty

        If Not String.IsNullOrEmpty(strProgram) Then
            objRetorno = strProgram
        Else
            objRetorno = strRetornoOpcional
        End If

        Return objRetorno
    End Function

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerFormaPagosPorDefecto_Completed(ByVal lo As LoadOperation(Of tblFondosFormasPagos))
        If Not lo.HasError Then
            FormaPagoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la FormaPago por defecto",
                                             Me.ToString(), "TerminoTraerFormaPagoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerFormaPago(ByVal lo As LoadOperation(Of tblFondosFormasPagos))
        If Not lo.HasError Then
            ListaFormaPago = lo.Entities.ToList

            If ListaFormaPago.Count = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de FormaPagos", _
                                             Me.ToString(), "TerminoTraerFormaPago", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaFormaPago As List(Of tblFondosFormasPagos)
    Public Property ListaFormaPago() As List(Of tblFondosFormasPagos)
        Get
            Return _ListaFormaPago
        End Get
        Set(ByVal value As List(Of tblFondosFormasPagos))
            _ListaFormaPago = value

            MyBase.CambioItem("ListaFormaPago")
            MyBase.CambioItem("ListaFormaPagoPaged")
            If Not IsNothing(ListaFormaPago) Then
                If ListaFormaPago.Count > 0 Then
                    FormaPagoSelected = ListaFormaPago.FirstOrDefault
                Else
                    FormaPagoSelected = Nothing
                End If
            Else
                FormaPagoSelected = Nothing
            End If
        End Set
    End Property

    Public ReadOnly Property ListaFormaPagoPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaFormaPago) Then
                Dim view = New PagedCollectionView(_ListaFormaPago)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _FormaPagoSelected As tblFondosFormasPagos
    Public Property FormaPagoSelected() As tblFondosFormasPagos
        Get
            Return _FormaPagoSelected
        End Get
        Set(ByVal value As tblFondosFormasPagos)
            _FormaPagoSelected = value
            MyBase.CambioItem("FormaPagoSelected")
        End Set
    End Property

    Private _HoraInicioPermitida As String
    Public Property HoraInicioPermitida() As String
        Get
            Return _HoraInicioPermitida
        End Get
        Set(ByVal value As String)
            _HoraInicioPermitida = value
            MyBase.CambioItem("HoraInicioPermitida")
        End Set
    End Property

    Private _HoraFinPermitida As String
    Public Property HoraFinPermitida() As String
        Get
            Return _HoraFinPermitida
        End Get
        Set(ByVal value As String)
            _HoraFinPermitida = value
            MyBase.CambioItem("HoraFinPermitida")
        End Set
    End Property

    Private _HabilitarOrdenes As Boolean
    Public Property HabilitarOrdenes() As Boolean
        Get
            Return _HabilitarOrdenes
        End Get
        Set(ByVal value As Boolean)
            _HabilitarOrdenes = value
            MyBase.CambioItem("HabilitarOrdenes")
        End Set
    End Property

    Private _HabilitarTesoreria As Boolean
    Public Property HabilitarTesoreria() As Boolean
        Get
            Return _HabilitarTesoreria
        End Get
        Set(ByVal value As Boolean)
            _HabilitarTesoreria = value
            MyBase.CambioItem("HabilitarTesoreria")
        End Set
    End Property

    Private _DiccionarioCombosEspecificos As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
    Public Property DiccionarioCombosEspecificos() As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))
        Get
            Return _DiccionarioCombosEspecificos
        End Get
        Set(ByVal value As Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))
            _DiccionarioCombosEspecificos = value
        End Set
    End Property

    Private _NombreVistaControl As String
    Public Property NombreVistaControl() As String
        Get
            Return _NombreVistaControl
        End Get
        Set(ByVal value As String)
            _NombreVistaControl = value
        End Set
    End Property

    Private _NombreDiccionarioCombos As String
    Public Property NombreDiccionarioCombos() As String
        Get
            Return _NombreDiccionarioCombos
        End Get
        Set(ByVal value As String)
            _NombreDiccionarioCombos = value
        End Set
    End Property

    Private WithEvents _cb As CamposBusquedaFormaPagoFondos = New CamposBusquedaFormaPagoFondos
    Public Property cb() As CamposBusquedaFormaPagoFondos
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaFormaPagoFondos)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property


#End Region

#Region "Métodos"

    Public Overrides Sub NuevoRegistro()
        Try
            ObtenerRegistroAnterior()

            Dim NewFormaPago As New tblFondosFormasPagos
            'TODO: Verificar cuales son los campos que deben inicializarse
            If Not IsNothing(FormaPagoPorDefecto) Then
                NewFormaPago.intIdFondoFormaPago = FormaPagoPorDefecto.intIdFondoFormaPago
            End If
            NewFormaPago.Descripcion = ""
            NewFormaPago.TipoMovimiento = ""
            NewFormaPago.Usuario = Program.Usuario
            FormaPagoAnterior = _FormaPagoSelected
            FormaPagoSelected = NewFormaPago
            MyBase.CambioItem("FormaPagoSelected")
            Editando = True
            MyBase.CambioItem("Editando")

            HabilitarOrdenes = False
            HabilitarTesoreria = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.tblFondosFormasPagos.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.FormasPagosFondosFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerFormaPago, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.FormasPagosFondosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerFormaPago, "FiltroVM")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro",
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        cb = New CamposBusquedaFormaPagoFondos()
        MyBase.CambioItem("cb")
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If Not String.IsNullOrEmpty(_cb.strDescripcion) Or Not String.IsNullOrEmpty(_cb.strTipoMovimiento) Or Not String.IsNullOrEmpty(_cb.strTipoTransaccion) Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                If Not IsNothing(dcProxy.tblFondosFormasPagos) Then
                    dcProxy.tblFondosFormasPagos.Clear()
                End If
                FormaPagoAnterior = Nothing
                IsBusy = True


                dcProxy.Load(dcProxy.FormasPagosFondosConsultarQuery(_cb.strDescripcion, _cb.strTipoMovimiento, _cb.strTipoTransaccion, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerFormaPago, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaFormaPagoFondos
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub



    Private Function ObtenerValorPropiedad(ByVal objValor As Object) As String
        If IsNothing(objValor) Then
            Return "NULL"
        Else
            Return objValor
        End If
    End Function

    Private Sub TerminoValidarRegistro(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.ValidacionEliminarRegistro))
        Try
            If Not lo.HasError Then
                If lo.Entities.ToList.Count > 0 Then
                    If lo.UserState = "ELIMINAR" Then

                    Else
                        If lo.Entities.ToList.First.PermitirRealizarAccion Then
                            Dim origen = "update"
                            ErrorForma = ""
                            If Not _ListaFormaPago.Where(Function(i) i.intIdFondoFormaPago = _FormaPagoSelected.intIdFondoFormaPago).Count > 0 Then
                                origen = "insert"
                                ListaFormaPago.Add(FormaPagoSelected)
                            End If
                            IsBusy = True
                            Program.VerificarCambiosProxyServidor(dcProxy)
                            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
                        Else
                            IsBusy = False
                            mostrarMensaje(lo.Entities.ToList.First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la validación de eliminación.",
                                                 Me.ToString(), "TerminoValidarEliminarRegistro", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la validación de eliminación.",
                                                 Me.ToString(), "TerminoValidarEliminarRegistro", Application.Current.ToString(), Program.Maquina, ex)
            lo.MarkErrorAsHandled()   '????
            IsBusy = False
        End Try
    End Sub



    Public Sub ObtenerRegistroAnterior()
        Try
            Dim objFormaPago As New tblFondosFormasPagos
            If Not IsNothing(_FormaPagoSelected) Then
                objFormaPago.intIdFondoFormaPago = _FormaPagoSelected.intIdFondoFormaPago
                objFormaPago.Descripcion = _FormaPagoSelected.Descripcion
                objFormaPago.TipoMovimiento = _FormaPagoSelected.TipoMovimiento
                objFormaPago.TipoTransaccion = _FormaPagoSelected.TipoTransaccion
                objFormaPago.Usuario = _FormaPagoSelected.Usuario
            End If

            FormaPagoAnterior = Nothing
            FormaPagoAnterior = objFormaPago
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.",
             Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub



#End Region

#Region "Eventos"

    Private Sub _FormaPagoSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _FormaPagoSelected.PropertyChanged
        Try

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar el cambio de propiedad.",
             Me.ToString(), "_FormaPagoSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _cb_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _cb.PropertyChanged
        Try

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar el cambio de propiedad.",
             Me.ToString(), "_FormaPagoSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaFormaPagoFondos
    Implements INotifyPropertyChanged

    Private _strDescripcion As String
    <Display(Name:="Descripcion", Description:="Descripcion")> _
    Public Property strDescripcion() As String
        Get
            Return _strDescripcion
        End Get
        Set(ByVal value As String)
            _strDescripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDescripcion"))
        End Set
    End Property
    Private _strTipoMovimiento As String
    <Display(Name:="Tipo Movimiento", Description:="Tipo Movimiento")> _
    Public Property strTipoMovimiento() As String
        Get
            Return _strTipoMovimiento
        End Get
        Set(ByVal value As String)
            _strTipoMovimiento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoMovimiento"))
        End Set
    End Property
    Private _strTipoTransaccion As String
    <Display(Name:="Tipo Transaccion", Description:="Tipo Transaccion")> _
    Public Property strTipoTransaccion() As String
        Get
            Return _strTipoTransaccion
        End Get
        Set(ByVal value As String)
            _strTipoTransaccion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strTipoTransaccion"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class