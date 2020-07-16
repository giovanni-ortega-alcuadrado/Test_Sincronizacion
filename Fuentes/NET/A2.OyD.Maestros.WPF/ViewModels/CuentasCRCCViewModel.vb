Imports Telerik.Windows.Controls


Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OYD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes

Public Class CuentasCRCCViewModel
    Inherits A2ControlMenu.A2ViewModel
    Private CuentasCRCCDefecto As CuentasCRCC
    Private CuentasCRCCAnterior As CuentasCRCC
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim objProxy As UtilidadesDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)
    Public _mlogBuscarCliente As Boolean = True

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosDomainContext()
            dcProxy1 = New MaestrosDomainContext()
            objProxy = New UtilidadesDomainContext()

        Else
            dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxy1 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
        End If
        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.CuentasCRCC_FiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasCRCC, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerCuentasCRCCPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasCRCCPorDefecto_Completed, "Default")

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "CuentasDecevalPorAgrupadorViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerCuentasCRCCPorDefecto_Completed(ByVal lo As LoadOperation(Of CuentasCRCC))
        If Not lo.HasError Then
            CuentasCRCCDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la TerminoTraerCuentasCRCCPorDefecto por defecto", _
                                             Me.ToString(), "TerminoTraerCuentasCRCCPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerCuentasCRCC(ByVal lo As LoadOperation(Of CuentasCRCC))
        If Not lo.HasError Then
            ListaCuentasCRCC = dcProxy.CuentasCRCCs
            If dcProxy.CuentasCRCCs.Count > 0 Then
                If lo.UserState = "insert" Then
                    CuentasCRCCSelected = ListaCuentasCRCC.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de CuentasCRCC", _
                                             Me.ToString(), "TerminoTraerCuentasCRCC", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub
  
   
#End Region

#Region "Propiedades"

    Private _ListaCuentasCRCC As EntitySet(Of CuentasCRCC)
    Public Property ListaCuentasCRCC() As EntitySet(Of CuentasCRCC)
        Get
            Return _ListaCuentasCRCC
        End Get
        Set(ByVal value As EntitySet(Of CuentasCRCC))
            _ListaCuentasCRCC = value

            MyBase.CambioItem("ListaCuentasCRCC")
            MyBase.CambioItem("ListaCuentasCRCCPaged")
            If Not IsNothing(value) Then
                If IsNothing(CuentasCRCCAnterior) Then
                    CuentasCRCCSelected = _ListaCuentasCRCC.FirstOrDefault
                Else
                    CuentasCRCCSelected = CuentasCRCCAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaCuentasCRCCPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCuentasCRCC) Then
                Dim view = New PagedCollectionView(_ListaCuentasCRCC)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _CuentasCRCCSelected As CuentasCRCC
    Public Property CuentasCRCCSelected() As CuentasCRCC
        Get
            Return _CuentasCRCCSelected
        End Get
        Set(ByVal value As CuentasCRCC)

            _CuentasCRCCSelected = value
            MyBase.CambioItem("CuentasCRCCSelected")
        End Set
    End Property

    Private _EditaRegistro As Boolean = False
    Public Property EditaRegistro As Boolean
        Get
            Return _EditaRegistro
        End Get
        Set(ByVal value As Boolean)
            _EditaRegistro = value
            MyBase.CambioItem("EditaRegistro")
        End Set
    End Property
    Private _Checked As Boolean = False
    Public ReadOnly Property Checked() As Boolean
        Get
            Return Not Enabled
        End Get
    End Property

    Private _Enabled As Boolean = True
    Public Property Enabled() As Boolean
        Get
            Return _Enabled
        End Get
        Set(ByVal value As Boolean)
            _Enabled = value
            MyBase.CambioItem("Checked")
            MyBase.CambioItem("Enabled")
        End Set
    End Property
   

    Private _cb As CamposBusquedaCuentasCRCC = New CamposBusquedaCuentasCRCC
    Public Property cb() As CamposBusquedaCuentasCRCC
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaCuentasCRCC)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewCuentasCRCC As New CuentasCRCC
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewCuentasCRCC.IDtblCuentasCRCC = CuentasCRCCDefecto.IDtblCuentasCRCC
            NewCuentasCRCC.IDComitente = CuentasCRCCDefecto.IDComitente
            NewCuentasCRCC.CuentaCRCC = CuentasCRCCDefecto.CuentaCRCC
            NewCuentasCRCC.TipoDeOferta = CuentasCRCCDefecto.TipoDeOferta
            NewCuentasCRCC.Actualizacion = CuentasCRCCDefecto.Actualizacion
            NewCuentasCRCC.Usuario = Program.Usuario
            CuentasCRCCSelected = NewCuentasCRCC
            MyBase.CambioItem("CuentasCRCC")
            Editando = True
            EditaRegistro = True
            Enabled = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.CuentasCRCCs.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.CuentasCRCC_FiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasCRCC, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.CuentasCRCC_FiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasCRCC, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub Buscar()
        cb.IDComitente = String.Empty
        cb.CuentaCRCC = String.Empty
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.IDComitente <> String.Empty Or cb.CuentaCRCC <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.CuentasCRCCs.Clear()
                CuentasCRCCAnterior = Nothing
                IsBusy = True
                ' DescripcionFiltroVM = " Comitente = " & cb.Comitente.ToString()
                dcProxy.Load(dcProxy.CuentasCRCC_ConsultarQuery(cb.CuentaCRCC, cb.IDComitente,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasCRCC, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaCuentasCRCC
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try

            If String.IsNullOrEmpty(_CuentasCRCCSelected.IDComitente) Then
                A2Utilidades.Mensajes.mostrarMensaje("El comitente es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If IsNothing(_CuentasCRCCSelected.CuentaCRCC) Or _CuentasCRCCSelected.CuentaCRCC = "" Then
                A2Utilidades.Mensajes.mostrarMensaje("La cuenta CRCC es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            'If String.IsNullOrEmpty(_CuentasCRCCSelected.TipoDeOferta) Then
            '    A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un tipo de oferta.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Exit Sub
            'End If


            Dim origen = "update"
            ErrorForma = ""

            If Not ListaCuentasCRCC.Contains(CuentasCRCCSelected) Then
                origen = "insert"
                ListaCuentasCRCC.Add(CuentasCRCCSelected)
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
                If So.UserState = "insert" Or So.UserState = "update" Then
                    If So.Error.ToString.Contains("ErrorPersonalizado") Then
                        Dim intPosIni As Integer = So.Error.ToString.IndexOf("ErrorPersonalizado,") + 19
                        Dim intPosFin As Integer = So.Error.ToString.IndexOf("|")
                        strMsg = So.Error.ToString.Substring(intPosIni, intPosFin - intPosIni)
                        A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        So.MarkErrorAsHandled()
                        Exit Sub
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la validación de operaciones", _
                                                    Me.ToString(), "TerminoModificacionValidar", Application.Current.ToString(), Program.Maquina, So.Error)
                        So.MarkErrorAsHandled()
                    End If
                End If
              

                If So.UserState = "BorrarRegistro" Or So.UserState = "update" Then
                    dcProxy.RejectChanges()
                End If
                So.MarkErrorAsHandled()
                'EditaReg = True
                Exit Try
            End If
            If So.UserState = "insert" Then
                dcProxy.CuentasCRCCs.Clear()
                dcProxy.Load(dcProxy.CuentasCRCC_FiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasCRCC, "insert")
                '    dcProxy.Load(dcProxy.CuentasDecevalPorAgrupadorFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasDecevalPorAgrupador, "insert") ' Recarga la lista para que carguen los include
            End If
            EditaRegistro = False
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_CuentasCRCCSelected) Then
            Editando = True
            EditaRegistro = True
            Enabled = False
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_CuentasCRCCSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                EditaRegistro = False
                If _CuentasCRCCSelected.EntityState = EntityState.Detached Then
                    _CuentasCRCCSelected = CuentasCRCCAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_CuentasCRCCSelected) Then
                    IsBusy = True
                    'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                dcProxy.EliminarCuentasCRCC(CuentasCRCCSelected.IDtblCuentasCRCC, String.Empty,Program.Usuario, Program.HashConexion, AddressOf terminoeliminar, "borrar")
                End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub terminoeliminar(ByVal So As InvokeOperation(Of String))
        If So.HasError Then
            A2Utilidades.Mensajes.mostrarMensaje(So.Error.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Else
            If So.UserState = "borrar" Then
                dcProxy.CuentasCRCCs.Clear()
                dcProxy.Load(dcProxy.CuentasCRCC_FiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasCRCC, "insert") ' Recarga la lista para que carguen los include
            End If
        End If
        IsBusy = False
    End Sub
   
    Private Sub _CuentasDecevalPorAgrupadoSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _CuentasCRCCSelected.PropertyChanged
        Select Case e.PropertyName
            Case "IDComitente"
                'SLB20130930 Se adiciona la busqueda del comitente desde el control 
                If _mlogBuscarCliente Then
                    If Not String.IsNullOrEmpty(_CuentasCRCCSelected.IDComitente) Then
                        buscarComitente(_CuentasCRCCSelected.IDComitente, "encabezado")
                    End If
                End If
        End Select

    End Sub

   
    Public Sub llenarDiccionario()
        DicCamposTab.Add("Comitente", 1)
        DicCamposTab.Add("NroDocumento", 1)
        DicCamposTab.Add("Deposito", 1)
        DicCamposTab.Add("TipoIdComitente", 1)
    End Sub
#End Region

#Region "Busqueda de Comitente desde el control de la vista"

    ''' <summary>
    ''' Buscar los datos del comitente que tiene asignada la Tesoreria.
    ''' </summary>
    ''' <param name="pstrIdComitente">Comitente que se debe buscar. Es opcional y normalmente se toma de la orden activa</param>
    ''' <remarks>SLB20130122</remarks>
    Friend Sub buscarComitente(Optional ByVal pstrIdComitente As String = "", Optional ByVal pstrBusqueda As String = "")

        Dim strIdComitente As String = String.Empty

        Try
            If Not Me._CuentasCRCCSelected Is Nothing Then
                If Not strIdComitente.Equals(Me._CuentasCRCCSelected.IDComitente) Then
                    If pstrIdComitente.Trim.Equals(String.Empty) Then
                        strIdComitente = Me._CuentasCRCCSelected.IDComitente
                    Else
                        strIdComitente = pstrIdComitente
                    End If

                    If Not strIdComitente Is Nothing AndAlso Not strIdComitente.Trim.Equals(String.Empty) Then
                        objProxy.BuscadorClientes.Clear()
                        objProxy.Load(objProxy.buscarClienteEspecificoQuery(strIdComitente, Program.Usuario, "IdComitente", Program.HashConexion), AddressOf buscarComitenteCompleted, pstrBusqueda)
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se obtiene el resultado de buscar el cliente cuando se digita desde el control
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20130122</remarks>
    Private Sub buscarComitenteCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorClientes))
        Try
            If lo.Entities.ToList.Count > 0 Then
                If lo.Entities.ToList.Item(0).Estado.ToLower = "inactivo" Or lo.Entities.ToList.Item(0).Estado.ToLower = "bloqueado" Then
                    A2Utilidades.Mensajes.mostrarMensaje("El comitente ingresado se encuentra " & lo.Entities.ToList.Item(0).Estado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    _CuentasCRCCSelected.IDComitente = String.Empty
                    _CuentasCRCCSelected.NomCliente = String.Empty
                    _CuentasCRCCSelected.NroDocumento = String.Empty
                    _CuentasCRCCSelected.TipoIdComitente = String.Empty
                Else
                    _CuentasCRCCSelected.IDComitente = lo.Entities.First.IdComitente
                    _CuentasCRCCSelected.NomCliente = lo.Entities.First.Nombre
                    _CuentasCRCCSelected.NroDocumento = lo.Entities.First.NroDocumento
                    _CuentasCRCCSelected.TipoIdComitente = lo.Entities.First.CodTipoIdentificacion
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("El comitente ingresado no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                _CuentasCRCCSelected.IDComitente = String.Empty
                _CuentasCRCCSelected.NomCliente = String.Empty
                _CuentasCRCCSelected.NroDocumento = String.Empty
                _CuentasCRCCSelected.TipoIdComitente = String.Empty
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos del comitente de la orden", Me.ToString(), "buscarComitenteCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

#End Region


End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaCuentasCRCC
    Implements INotifyPropertyChanged

    Private _IDComitente As String
    <StringLength(17, ErrorMessage:="La longitud máxima es de 17")> _
     <Display(Name:="Comitente")> _
    Public Property IDComitente() As String
        Get
            Return _IDComitente
        End Get
        Set(ByVal value As String)
            _IDComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDComitente"))
        End Set
    End Property

    Private _CuentaCRCC As String
    <Display(Name:="CuentaCRCC")> _
    Public Property CuentaCRCC() As String
        Get
            Return _CuentaCRCC
        End Get
        Set(ByVal value As String)
            _CuentaCRCC = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CuentaCRCC"))
        End Set
    End Property


    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class








