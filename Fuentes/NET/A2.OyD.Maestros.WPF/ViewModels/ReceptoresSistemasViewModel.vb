Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ReceptoresSistemasViewModel.vb
'Generado el : 04/20/2011 11:55:31
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

Public Class ReceptoresSistemasViewModel
	Inherits A2ControlMenu.A2ViewModel
	Public Property cb As New CamposBusquedaReceptoresSistema
	Private ReceptoresSistemaPorDefecto As ReceptoresSistema
	Private ReceptoresSistemaAnterior As ReceptoresSistema
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim dcProxyUtilidades As UtilidadesDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)
    Dim strCodigoSAE As String = "SAE"

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosDomainContext()
            dcProxy1 = New MaestrosDomainContext()
            dcProxyUtilidades = New UtilidadesDomainContext()
        Else
            dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxy1 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxyUtilidades = New UtilidadesDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_UTIL_OYD).ToString()))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                ConsultarReceptores("TODOSLOSRECEPTORES")
                dcProxy.Load(dcProxy.ReceptoresSistemasFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptoresSistemas, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerReceptoresSistemaPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptoresSistemasPorDefecto_Completed, "Default")
                dcProxyUtilidades.Verificaparametro("VALIDAR_MONTOLIMITE_OAENRUTADAS", Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "VALIDAR_MONTOLIMITE_OAENRUTADAS")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ReceptoresSistemasViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerReceptoresSistemasPorDefecto_Completed(ByVal lo As LoadOperation(Of ReceptoresSistema))
        If Not lo.HasError Then
            ReceptoresSistemaPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ReceptoresSistema por defecto", _
                                             Me.ToString(), "TerminoTraerReceptoresSistemaPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerReceptoresSistemas(ByVal lo As LoadOperation(Of ReceptoresSistema))
        If Not lo.HasError Then
            VisEditar = Visibility.Visible
            VisNuevo = Visibility.Collapsed
            ListaReceptoresSistemas = dcProxy.ReceptoresSistemas
            If dcProxy.ReceptoresSistemas.Count > 0 Then
                If lo.UserState = "insert" Then
                    ReceptoresSistemaSelected = ListaReceptoresSistemas.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    'MessageBox.Show("No se encontró ningún registro")
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ReceptoresSistemas", _
                                             Me.ToString(), "TerminoTraerReceptoresSistema", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private Sub Terminotraerparametro(ByVal lo As InvokeOperation(Of String))
        Try
            If IsNothing(lo.Error) Then
                Dim strNombreParametro As String = lo.UserState.ToString.ToUpper

                Select Case strNombreParametro
                    Case "VALIDAR_MONTOLIMITE_OAENRUTADAS"
                        If lo.Value.ToUpper.Equals("SI") Then
                            HabilitarMontoLimite = True
                        Else
                            HabilitarMontoLimite = False
                        End If
                End Select

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los parametros.", _
                                            Me.ToString(), "Terminotraerparametro", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los parametros.", _
                                             Me.ToString(), "Terminotraerparametro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


#End Region

#Region "Propiedades"

    Private _ListaReceptoresSistemas As EntitySet(Of ReceptoresSistema)
    Public Property ListaReceptoresSistemas() As EntitySet(Of ReceptoresSistema)
        Get
            Return _ListaReceptoresSistemas
        End Get
        Set(ByVal value As EntitySet(Of ReceptoresSistema))
            _ListaReceptoresSistemas = value

            MyBase.CambioItem("ListaReceptoresSistemas")
            MyBase.CambioItem("ListaReceptoresSistemasPaged")
            If Not IsNothing(value) Then
                If IsNothing(ReceptoresSistemaAnterior) Then
                    ReceptoresSistemaSelected = _ListaReceptoresSistemas.FirstOrDefault
                Else
                    ReceptoresSistemaSelected = ReceptoresSistemaAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaReceptoresSistemasPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaReceptoresSistemas) Then
                Dim view = New PagedCollectionView(_ListaReceptoresSistemas)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _ReceptoresSistemaSelected As ReceptoresSistema
    Public Property ReceptoresSistemaSelected() As ReceptoresSistema
        Get
            Return _ReceptoresSistemaSelected
        End Get
        Set(ByVal value As ReceptoresSistema)
            _ReceptoresSistemaSelected = value
            HabilitarDeshabilitarLimite()
            MyBase.CambioItem("ReceptoresSistemaSelected")
        End Set
    End Property

    Private _Editareg As Boolean
    Public Property Editareg As Boolean
        Get
            Return _Editareg
        End Get
        Set(ByVal value As Boolean)
            _Editareg = value
            MyBase.CambioItem("EditaReg")
        End Set
    End Property
    Private _Enabled As Boolean
    Public Property Enabled As Boolean
        Get
            Return _Enabled
        End Get
        Set(ByVal value As Boolean)
            If Editareg = False Then
                value = False
            ElseIf Editareg = True Then
                value = True
            End If
            _Enabled = value
            MyBase.CambioItem("Enabled")
        End Set
    End Property

    Private _VisEditar As Visibility
    Public Property VisEditar() As Visibility
        Get
            Return _VisEditar
        End Get
        Set(ByVal value As Visibility)
            _VisEditar = value
            MyBase.CambioItem("VisEditar")
        End Set
    End Property

    Private _VisNuevo As Visibility
    Public Property VisNuevo() As Visibility
        Get
            Return _VisNuevo
        End Get
        Set(ByVal value As Visibility)
            _VisNuevo = value
            MyBase.CambioItem("VisNuevo")
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

    Private _HabilitarMontoLimite As Boolean = False
    Public Property HabilitarMontoLimite() As Boolean
        Get
            Return _HabilitarMontoLimite
        End Get
        Set(ByVal value As Boolean)
            _HabilitarMontoLimite = value
            If _HabilitarMontoLimite Then
                MostrarMontoLimite = Visibility.Visible
            Else
                MostrarMontoLimite = Visibility.Collapsed
            End If
            MyBase.CambioItem("MostrarMontoLimite")
        End Set
    End Property

    Private _ListaReceptoresCompleta As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaReceptoresCompleta() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaReceptoresCompleta
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaReceptoresCompleta = value
            MyBase.CambioItem("ListaReceptoresCompleta")
        End Set
    End Property

    Private _ListaReceptoresActivos As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaReceptoresActivos() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaReceptoresActivos
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaReceptoresActivos = value
            MyBase.CambioItem("ListaReceptoresActivos")
        End Set
    End Property

    Private _MostrarMontoLimite As Visibility = Visibility.Collapsed
    Public Property MostrarMontoLimite() As Visibility
        Get
            Return _MostrarMontoLimite
        End Get
        Set(ByVal value As Visibility)
            _MostrarMontoLimite = value
            MyBase.CambioItem("MostrarMontoLimite")
        End Set
    End Property

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            ConsultarReceptores("RECEPTORESACTIVOS")

            Dim NewReceptoresSistema As New ReceptoresSistema
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewReceptoresSistema.IDComisionista = ReceptoresSistemaPorDefecto.IDComisionista
            NewReceptoresSistema.IDSucComisionista = ReceptoresSistemaPorDefecto.IDSucComisionista
            NewReceptoresSistema.Codigo = ReceptoresSistemaPorDefecto.Codigo
            NewReceptoresSistema.Codigo_Sistema = ReceptoresSistemaPorDefecto.Codigo_Sistema
            NewReceptoresSistema.Valor_Sistema = ReceptoresSistemaPorDefecto.Valor_Sistema
            NewReceptoresSistema.Actualizacion = ReceptoresSistemaPorDefecto.Actualizacion
            NewReceptoresSistema.Usuario = Program.Usuario
            NewReceptoresSistema.IDReceptoresSistemas = ReceptoresSistemaPorDefecto.IDReceptoresSistemas
            NewReceptoresSistema.MontoLimite = ReceptoresSistemaPorDefecto.MontoLimite
            ReceptoresSistemaAnterior = ReceptoresSistemaSelected
            ReceptoresSistemaSelected = NewReceptoresSistema
            PropiedadTextoCombos = String.Empty
            MyBase.CambioItem("ReceptoresSistemas")
            Editando = True
            Editareg = True
            Enabled = True
            VisEditar = Visibility.Collapsed
            VisNuevo = Visibility.Visible
            PropiedadTextoCombos = String.Empty
            MyBase.CambioItem("PropiedadTextoCombos")
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.ReceptoresSistemas.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ReceptoresSistemasFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptoresSistemas, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.ReceptoresSistemasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptoresSistemas, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.Codigo <> String.Empty Or cb.Codigo_Sistema <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.ReceptoresSistemas.Clear()
                ReceptoresSistemaAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " Codigo = " &  cb.Codigo.ToString() & " Codigo_Sistema = " &  cb.Codigo_Sistema.ToString() 
                dcProxy.Load(dcProxy.ReceptoresSistemasConsultarQuery(cb.Codigo, cb.Codigo_Sistema,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptoresSistemas, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaReceptoresSistema
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
            If _ReceptoresSistemaSelected.Codigo_Sistema = strCodigoSAE And _ReceptoresSistemaSelected.MontoLimite = 0 And _HabilitarMontoLimite Then
                A2Utilidades.Mensajes.mostrarMensaje("El valor del monto limite debe ser mayor que cero.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            ConsultarReceptores("RECEPTORESACTIVOS", _ReceptoresSistemaSelected.Codigo, "VALIDARRECEPTORACTIVOGUARDADO")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ContinuarGuardadoRegistro()
        Try
            Dim origen = "update"
            ErrorForma = ""
            ReceptoresSistemaAnterior = ReceptoresSistemaSelected
            If Not ListaReceptoresSistemas.Contains(ReceptoresSistemaSelected) Then
                origen = "insert"
                ListaReceptoresSistemas.Add(ReceptoresSistemaSelected)
            End If
            IsBusy = True
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ContinuarGuardadoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And ((So.UserState = "insert") Or (So.UserState = "update")) Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,") '.Split(So.Error.Message, vbCr)
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    If So.UserState = "insert" Then
                        ListaReceptoresSistemas.Remove(ReceptoresSistemaSelected)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                      Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                End If
                If So.UserState = "BorrarRegistro" Or So.UserState = "update" Then
                    dcProxy.RejectChanges()
                End If
                So.MarkErrorAsHandled()
                Exit Try
            Else
                VisEditar = Visibility.Visible
                VisNuevo = Visibility.Collapsed
            End If
            'If So.UserState = "insert" Then
            '    dcProxy.ReceptoresSistemas.Clear()
            '    dcProxy.Load(dcProxy.ReceptoresSistemasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptoresSistemas, "insert") ' Recarga la lista para que carguen los include
            'End If
            MyBase.TerminoSubmitChanges(So)
            Editareg = False
            Enabled = False
            VisNuevo = Visibility.Collapsed
            VisEditar = Visibility.Visible
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_ReceptoresSistemaSelected) Then
            ConsultarReceptores("RECEPTORESACTIVOS", _ReceptoresSistemaSelected.Codigo, "VALIDARRECEPTORACTIVO")
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Sub ContinuarEdicionRegistro()
        If Not IsNothing(_ReceptoresSistemaSelected) Then
            Editando = True
            Editareg = False
            Enabled = False
            VisNuevo = Visibility.Collapsed
            VisEditar = Visibility.Visible

        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ReceptoresSistemaSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                Editareg = False
                Enabled = False
                VisNuevo = Visibility.Collapsed
                VisEditar = Visibility.Visible
                If _ReceptoresSistemaSelected.EntityState = EntityState.Detached Then
                    ReceptoresSistemaSelected = ReceptoresSistemaAnterior
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
            If Not IsNothing(_ReceptoresSistemaSelected) Then
                dcProxy.ReceptoresSistemas.Remove(_ReceptoresSistemaSelected)
                ReceptoresSistemaSelected = _ListaReceptoresSistemas.LastOrDefault
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

    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaReceptoresSistema
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la Búsqueda", _
                     Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub llenarDiccionario()
        DicCamposTab.Add("Codigo", 1)
        DicCamposTab.Add("Codigo_Sistema", 1)
        DicCamposTab.Add("Valor_Sistema", 1)
    End Sub

    Private Sub _ReceptoresSistemaSelectedChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ReceptoresSistemaSelected.PropertyChanged
        Try
            If e.PropertyName = "Codigo_Sistema" Then
                HabilitarDeshabilitarLimite()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar la propiedad.",
                     Me.ToString(), "_ReceptoresSistemaSelectedChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub ConsultarReceptores(ByVal pstrConsulta As String, Optional ByVal pstrFiltroTexto As String = "", Optional ByVal pstrUserState As String = "")
        Try
            If pstrConsulta <> "TODOSLOSRECEPTORES" Then
                IsBusy = True
            End If

            If String.IsNullOrEmpty(pstrUserState) Then
                pstrUserState = pstrConsulta
            End If
            dcProxyUtilidades.ItemCombos.Clear()
            dcProxyUtilidades.Load(dcProxyUtilidades.cargarCombosCondicionalQuery(pstrConsulta, pstrFiltroTexto, 0, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarReceptores, pstrUserState)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar los receptores.",
                     Me.ToString(), "ConsultarReceptores", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoConsultarReceptores(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If IsNothing(lo.Error) Then
                If lo.UserState = "TODOSLOSRECEPTORES" Then
                    ListaReceptoresCompleta = lo.Entities.ToList
                ElseIf lo.UserState = "VALIDARRECEPTORACTIVO" Then
                    If lo.Entities.Count > 0 Then
                        ContinuarEdicionRegistro()
                        IsBusy = False
                    Else
                        MyBase.RetornarValorEdicionNavegacion()
                        A2Utilidades.Mensajes.mostrarMensaje("El receptor se encuentra inactivo, por lo tanto no se puede editar el registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                    End If
                ElseIf lo.UserState = "VALIDARRECEPTORACTIVOGUARDADO" Then
                    If lo.Entities.Count > 0 Then
                        ContinuarGuardadoRegistro()
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("El receptor se encuentra inactivo, por lo tanto no se puede guardar el registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                    End If
                Else
                    ListaReceptoresActivos = lo.Entities.ToList
                    IsBusy = False
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar los receptores.",
                     Me.ToString(), "TerminoConsultarReceptores", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al consultar los receptores.",
                     Me.ToString(), "TerminoConsultarReceptores", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub HabilitarDeshabilitarLimite()
        Try
            If Not IsNothing(_ReceptoresSistemaSelected) Then
                If _HabilitarMontoLimite Then
                    If _ReceptoresSistemaSelected.Codigo_Sistema = strCodigoSAE Then
                        MostrarMontoLimite = Visibility.Visible
                    Else
                        MostrarMontoLimite = Visibility.Collapsed
                    End If
                Else
                    MostrarMontoLimite = Visibility.Collapsed
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al habilitar el el monto limite.", _
                     Me.ToString(), "HabilitarDeshabilitarLimite", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaReceptoresSistema
 	
    <StringLength(4, ErrorMessage:="La longitud máxima es de 4")> _
     <Display(Name:="Receptor")> _
    Public Property Codigo As String
 	
    <StringLength(20, ErrorMessage:="La longitud máxima es de 20")> _
     <Display(Name:="Sistema")> _
    Public Property Codigo_Sistema As String
End Class




