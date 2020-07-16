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

Public Class CodigosAjustesViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaCodigosAjustes
    Private CodigosAjustesPorDefecto As CodigosAjustes
    Private CodigosAjustesAnterior As CodigosAjustes
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
                dcProxy.Load(dcProxy.FiltrarCodigosAjustesQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigosAjustes, "FiltroInicial")

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ConsecutivosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerCodigosAjustesPorDefecto_Completed(ByVal lo As LoadOperation(Of CodigosAjustes))
        If Not lo.HasError Then
            CodigosAjustesPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención del registro por defecto", _
                                             Me.ToString(), "TerminoTraerCuentasBancariasPorConceptoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerCodigosAjustes(ByVal lo As LoadOperation(Of CodigosAjustes))
        If Not lo.HasError Then
            ListaCodigosAjustes = dcProxy.CodigosAjustes
            If dcProxy.CodigosAjustes.Count > 0 Then
                If lo.UserState = "insert" Then
                    CodigosAjustesSelected = ListaCodigosAjustes.First   ' JFSB 20160827 Se pone el primer registro que tiene la lista
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Consecutivos", _
                                             Me.ToString(), "TerminoTraerConsecutivo", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaCodigosAjustes As EntitySet(Of CodigosAjustes)
    Public Property ListaCodigosAjustes() As EntitySet(Of CodigosAjustes)
        Get
            Return _ListaCodigosAjustes
        End Get
        Set(ByVal value As EntitySet(Of CodigosAjustes))
            _ListaCodigosAjustes = value

            MyBase.CambioItem("ListaCodigosAjustes")
            MyBase.CambioItem("ListaCodigosAjustesPaged")
            If Not IsNothing(value) Then
                If IsNothing(CodigosAjustesAnterior) Then
                    CodigosAjustesSelected = _ListaCodigosAjustes.FirstOrDefault
                Else
                    CodigosAjustesSelected = CodigosAjustesAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaCodigosAjustesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCodigosAjustes) Then
                Dim view = New PagedCollectionView(_ListaCodigosAjustes)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _CodigosAjustesSelected As CodigosAjustes
    Public Property CodigosAjustesSelected() As CodigosAjustes
        Get
            Return _CodigosAjustesSelected
        End Get
        Set(ByVal value As CodigosAjustes)
            _CodigosAjustesSelected = value
            If Not value Is Nothing Then
            End If
            MyBase.CambioItem("CodigosAjustesSelected")
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

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewCodigosAjustes As New CodigosAjustes

            NewCodigosAjustes.COD_TRANSACCION = Nothing
            NewCodigosAjustes.DESCRIPCION = String.Empty
            NewCodigosAjustes.IdConcepto = Nothing
            NewCodigosAjustes.DescripcionConcepto = String.Empty
            NewCodigosAjustes.IdConceptoAnterior = Nothing
            NewCodigosAjustes.Usuario = Program.Usuario
            NewCodigosAjustes.Accion = "Ingresar"
            NewCodigosAjustes.TIPO_PARAMETRO = String.Empty
            NewCodigosAjustes.Owner = String.Empty
            NewCodigosAjustes.strDescripcionOwner = String.Empty

            CodigosAjustesAnterior = CodigosAjustesSelected
            CodigosAjustesSelected = NewCodigosAjustes
            MyBase.CambioItem("CodigosAjustes")
            Editando = True
            Editareg = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.CodigosAjustes.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.FiltrarCodigosAjustesQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigosAjustes, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.FiltrarCodigosAjustesQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigosAjustes, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        cb.CodTransaccion = Nothing
        cb.Descripcion = String.Empty
        cb.Owner = String.Empty
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If IsNothing(cb.CodTransaccion) Or String.IsNullOrEmpty(cb.Descripcion) Or String.IsNullOrEmpty(cb.Owner) Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.CodigosAjustes.Clear()
                CodigosAjustesAnterior = Nothing
                IsBusy = True
                dcProxy.Load(dcProxy.ConsultarCodigosAjustesQuery(cb.CodTransaccion, cb.Descripcion, cb.Owner, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigosAjustes, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaCodigosAjustes
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaCodigosAjustes
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la Búsqueda", _
                     Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            If IsNothing(CodigosAjustesSelected.COD_TRANSACCION) Then
                A2Utilidades.Mensajes.mostrarMensaje("el campo código transacción es un campo requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If String.IsNullOrEmpty(CodigosAjustesSelected.DESCRIPCION) Then
                A2Utilidades.Mensajes.mostrarMensaje("el campo descripción es un campo requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If IsNothing(CodigosAjustesSelected.IdConcepto) Then
                A2Utilidades.Mensajes.mostrarMensaje("El campo concepto es un campo requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If String.IsNullOrEmpty(CodigosAjustesSelected.Owner) Then
                A2Utilidades.Mensajes.mostrarMensaje("El campo owner es un campo requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If IsNothing(CodigosAjustesSelected.IdConceptoAnterior) Then
                CodigosAjustesSelected.IdConceptoAnterior = CodigosAjustesSelected.IdConcepto
            End If
            Dim origen = "Actualizar"
            ErrorForma = ""
            CodigosAjustesAnterior = CodigosAjustesSelected
            If Not ListaCodigosAjustes.Contains(CodigosAjustesSelected) Then
                origen = "Ingresar"
                ListaCodigosAjustes.Add(CodigosAjustesSelected)
            End If
            IsBusy = True
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
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
                    'If So.UserState = "insert" Then
                    '    ListaCuentasBancariasPorConcepto.Remove(CuentasBancariasPorConceptoSelected)
                    'End If
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
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
                dcProxy.CodigosAjustes.Clear()
                If FiltroVM.Length > 0 Then
                    Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                    dcProxy.Load(dcProxy.FiltrarCodigosAjustesQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigosAjustes, "Ingresar")

                Else
                    dcProxy.Load(dcProxy.FiltrarCodigosAjustesQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCodigosAjustes, "Ingresar")
                End If


            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_CodigosAjustesSelected) Then
            CodigosAjustesSelected.IdConceptoAnterior = CodigosAjustesSelected.IdConcepto
            Editando = True
            Editareg = False
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_CodigosAjustesSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                Editareg = False
                If _CodigosAjustesSelected.EntityState = EntityState.Detached Then
                    CodigosAjustesSelected = CodigosAjustesAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        
        'MessageBox.Show("Esta funcionalidad no esta habilitada para este maestro", "Funcionalidad ", MessageBoxButton.OK)
        Try
            If Not IsNothing(_CodigosAjustesSelected) Then
                dcProxy.CodigosAjustes.Remove(_CodigosAjustesSelected)
                CodigosAjustesSelected = _ListaCodigosAjustes.LastOrDefault
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
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
Public Class CamposBusquedaCodigosAjustes
    Implements INotifyPropertyChanged

    Private _CodTransaccion As Nullable(Of Double) = 0
    Public Property CodTransaccion As Nullable(Of Double)
        Get
            Return _CodTransaccion
        End Get
        Set(ByVal value As Nullable(Of Double))
            _CodTransaccion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CodTransaccion"))
        End Set
    End Property

    Private _Descripcion As String
    Public Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Descripcion"))
        End Set
    End Property

    Private _Owner As String
    Public Property Owner() As String
        Get
            Return _Owner
        End Get
        Set(ByVal value As String)
            _Owner = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Owner"))
        End Set
    End Property


    Private _DescripcionOwner As String = String.Empty
    Public Property DescripcionOwner As String
        Get
            Return _DescripcionOwner
        End Get
        Set(ByVal value As String)
            _DescripcionOwner = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("DescripcionOwner"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class




