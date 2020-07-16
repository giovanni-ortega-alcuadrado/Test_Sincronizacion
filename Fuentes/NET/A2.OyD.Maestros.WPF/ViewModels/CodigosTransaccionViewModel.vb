Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ConsecutivosViewModel.vb
'Generado el : 02/18/2011 09:56:35
'Propiedad de Alcuadrado S.A. 2010

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web

Public Class CodigosTransaccionViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaCodigosTransaccion
    Private CuentasBancariasPorConceptoPorDefecto As CodigosTransaccion
    Private CodigosTransaccionAnterior As CodigosTransaccion
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosDomainContext()
            dcProxy1 = New MaestrosDomainContext()
        Else
            dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxy1 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.FiltrarCodigosTransaccionQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigosTransaccion, "FiltroInicial")

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "CodigosTransaccionViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerCodigosTransaccion(ByVal lo As LoadOperation(Of CodigosTransaccion))
        If Not lo.HasError Then
            ListaCodigosTransaccion = dcProxy.CodigosTransaccions
            If dcProxy.CodigosTransaccions.Count > 0 Then
                If lo.UserState = "insert" Then
                    CodigosTransaccionSelected = ListaCodigosTransaccion.First   ' JFSB 20160827 Se pone el primer registro que tiene la lista
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de códigos de transacción",
                                             Me.ToString(), "TerminoTraerCodigosTransaccion", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub




#End Region

#Region "Propiedades"

    Private _ListaCodigosTransaccion As EntitySet(Of CodigosTransaccion)
    Public Property ListaCodigosTransaccion() As EntitySet(Of CodigosTransaccion)
        Get
            Return _ListaCodigosTransaccion
        End Get
        Set(ByVal value As EntitySet(Of CodigosTransaccion))
            _ListaCodigosTransaccion = value

            MyBase.CambioItem("ListaCodigosTransaccion")
            MyBase.CambioItem("ListaCodigosTransaccionPaged")
            If Not IsNothing(value) Then
                If IsNothing(CodigosTransaccionAnterior) Then
                    CodigosTransaccionSelected = _ListaCodigosTransaccion.FirstOrDefault
                Else
                    CodigosTransaccionSelected = CodigosTransaccionAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaCodigosTransaccionPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCodigosTransaccion) Then
                Dim view = New PagedCollectionView(_ListaCodigosTransaccion)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _CodigosTransaccionSelected As CodigosTransaccion
    Public Property CodigosTransaccionSelected() As CodigosTransaccion
        Get
            Return _CodigosTransaccionSelected
        End Get
        Set(ByVal value As CodigosTransaccion)
            _CodigosTransaccionSelected = value
            If Not value Is Nothing Then
            End If
            MyBase.CambioItem("CodigosTransaccionSelected")
        End Set
    End Property

    Private _Editareg As Boolean
    Public Property Editareg As Boolean
        Get
            Return _Editareg
        End Get
        Set(ByVal value As Boolean)
            _Editareg = value
            MyBase.CambioItem("Editareg")
        End Set
    End Property

    Private _TabSeleccionadaFinanciero As Integer = 0
    Public Property TabSeleccionadaFinanciero
        Get
            Return _TabSeleccionadaFinanciero
        End Get
        Set(ByVal value)
            _TabSeleccionadaFinanciero = value
            MyBase.CambioItem("TabSeleccionadaFinanciero")
        End Set
    End Property

    Private _HabilitarCodigo As Boolean
    Public Property HabilitarCodigo() As Boolean
        Get
            Return _HabilitarCodigo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCodigo = value
            MyBase.CambioItem("HabilitarCodigo")
        End Set
    End Property
#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            HabilitarCodigo = True
            Dim NewCodigosTransaccion As New CodigosTransaccion

            NewCodigosTransaccion.ID = Nothing
            NewCodigosTransaccion.Codigo = Nothing
            NewCodigosTransaccion.Transaccion = String.Empty
            NewCodigosTransaccion.DetalleRC = String.Empty
            NewCodigosTransaccion.TipoTransaccion = String.Empty
            NewCodigosTransaccion.Actualizacion = Now
            NewCodigosTransaccion.Usuario = Program.Usuario
            NewCodigosTransaccion.Accion = "Ingresar"

            CodigosTransaccionAnterior = CodigosTransaccionSelected
            CodigosTransaccionSelected = NewCodigosTransaccion

            MyBase.CambioItem("CodigosTransaccion")
            Editando = True
            Editareg = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.CodigosTransaccions.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.FiltrarCodigosTransaccionQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigosTransaccion, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.FiltrarCodigosTransaccionQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigosTransaccion, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro",
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        cb.Codigo = Nothing
        cb.Transaccion = String.Empty
        cb.DetalleRC = String.Empty
        cb.TipoTransaccion = String.Empty
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If Not IsNothing(cb.Codigo) Or Not String.IsNullOrEmpty(cb.Transaccion) Or Not String.IsNullOrEmpty(cb.DetalleRC) Or Not String.IsNullOrEmpty(cb.TipoTransaccion) Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.CodigosTransaccions.Clear()
                CodigosTransaccionAnterior = Nothing
                If IsNothing(cb.Codigo) Then
                    cb.Codigo = -1
                End If
                IsBusy = True
                    dcProxy.Load(dcProxy.ConsultarCodigosTransaccionQuery(cb.Codigo, cb.Transaccion, cb.DetalleRC, cb.TipoTransaccion, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigosTransaccion, "Busqueda")
                    MyBase.ConfirmarBuscar()
                    cb = New CamposBusquedaCodigosTransaccion
                    CambioItem("cb")
                End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro",
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaCodigosTransaccion
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la Búsqueda",
                     Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            If IsNothing(CodigosTransaccionSelected.Codigo) Then
                A2Utilidades.Mensajes.mostrarMensaje("El campo código es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If String.IsNullOrEmpty(CodigosTransaccionSelected.Transaccion) Then
                A2Utilidades.Mensajes.mostrarMensaje("El campo transacción es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If String.IsNullOrEmpty(CodigosTransaccionSelected.DetalleRC) Then
                A2Utilidades.Mensajes.mostrarMensaje("El campo detalle es requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If String.IsNullOrEmpty(CodigosTransaccionSelected.TipoTransaccion) Then
                A2Utilidades.Mensajes.mostrarMensaje("El campo Tipo es requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            Dim origen = "Actualizar"
            ErrorForma = ""
            CodigosTransaccionAnterior = CodigosTransaccionSelected

            If Not ListaCodigosTransaccion.Contains(CodigosTransaccionSelected) Then
                origen = "Ingresar"
                ListaCodigosTransaccion.Add(CodigosTransaccionSelected)
            End If
            IsBusy = True
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            Dim strMsg As String = String.Empty
            IsBusy = False
            If So.HasError Then
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And (So.UserState = "Ingresar" Or (So.UserState = "Actualizar")) Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,")
                    Dim Mensaje = Split(Mensaje1(1), vbCr)

                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                      Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                End If

                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If
                So.MarkErrorAsHandled()
                Exit Try
            End If
            If So.UserState = "Ingresar" Or So.UserState = "Actualizar" Then
                IsBusy = True
                dcProxy.Consecutivos.Clear()
                If FiltroVM.Length > 0 Then
                    Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                    dcProxy.Load(dcProxy.FiltrarCodigosTransaccionQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigosTransaccion, "Ingresar")

                Else
                    dcProxy.Load(dcProxy.FiltrarCodigosTransaccionQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigosTransaccion, "Ingresar")
                End If

            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_CodigosTransaccionSelected) Then
            HabilitarCodigo = False
            Editando = True
            Editareg = False
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_CodigosTransaccionSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                Editareg = False
                If _CodigosTransaccionSelected.EntityState = EntityState.Detached Then
                    CodigosTransaccionSelected = CodigosTransaccionAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        
        'MessageBox.Show("Esta funcionalidad no esta habilitada para este maestro", "Funcionalidad ", MessageBoxButton.OK)
        Try
            If Not IsNothing(_CodigosTransaccionSelected) Then
                dcProxy.CodigosTransaccions.Remove(_CodigosTransaccionSelected)
                CodigosTransaccionSelected = _ListaCodigosTransaccion.LastOrDefault
                IsBusy = True
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro",
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub
    Public Sub llenarDiccionario()
        DicCamposTab.Add("NombreConsecutivo", 1)
        DicCamposTab.Add("Descripcion", 1)
        DicCamposTab.Add("IDOwner", 1)
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaCodigosTransaccion
    Implements INotifyPropertyChanged

    Private _Codigo As Nullable(Of Integer) = 0
    Public Property Codigo As Nullable(Of Integer)
        Get
            Return _Codigo
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _Codigo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Codigo"))
        End Set
    End Property

    Private _Transaccion As String
    Public Property Transaccion() As String
        Get
            Return _Transaccion
        End Get
        Set(ByVal value As String)
            _Transaccion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Transaccion"))
        End Set
    End Property

    Private _DetalleRC As String
    Public Property DetalleRC() As String
        Get
            Return _DetalleRC
        End Get
        Set(ByVal value As String)
            _DetalleRC = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DetalleRC"))
        End Set
    End Property

    Private _TipoTransaccion As String = String.Empty
    Public Property TipoTransaccion As String
        Get
            Return _TipoTransaccion
        End Get
        Set(ByVal value As String)
            _TipoTransaccion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoTransaccion"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class