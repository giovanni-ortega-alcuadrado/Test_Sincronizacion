Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: InhabilitadosViewModel.vb
'Generado el : 03/15/2011 10:24:50
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

Public Class InhabilitadosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaInhabilitado
    Private InhabilitadoPorDefecto As Inhabilitado
    Private InhabilitadoAnterior As Inhabilitado
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
                dcProxy.Load(dcProxy.InhabilitadosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerInhabilitados, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerInhabilitadoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerInhabilitadosPorDefecto_Completed, "Default")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "InhabilitadosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerInhabilitadosPorDefecto_Completed(ByVal lo As LoadOperation(Of Inhabilitado))
        If Not lo.HasError Then
            InhabilitadoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Inhabilitado por defecto",
                                             Me.ToString(), "TerminoTraerInhabilitadoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerInhabilitados(ByVal lo As LoadOperation(Of Inhabilitado))
        If Not lo.HasError Then
            ListaInhabilitados = dcProxy.Inhabilitados
            If dcProxy.Inhabilitados.Count > 0 Then
                If lo.UserState = "insert" Then
                    InhabilitadoSelected = ListaInhabilitados.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Inhabilitados",
                                             Me.ToString(), "TerminoTraerInhabilitado", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaInhabilitados As EntitySet(Of Inhabilitado)
    Public Property ListaInhabilitados() As EntitySet(Of Inhabilitado)
        Get
            Return _ListaInhabilitados
        End Get
        Set(ByVal value As EntitySet(Of Inhabilitado))
            _ListaInhabilitados = value

            MyBase.CambioItem("ListaInhabilitados")
            MyBase.CambioItem("ListaInhabilitadosPaged")
            If Not IsNothing(value) Then
                If IsNothing(InhabilitadoAnterior) Then
                    If _ListaInhabilitados.Count > 0 Then
                        InhabilitadoSelected = _ListaInhabilitados.FirstOrDefault
                    Else
                        InhabilitadoSelected = Nothing
                    End If
                Else
                    If _ListaInhabilitados.Where(Function(i) i.IDInhabilitado = InhabilitadoAnterior.IDInhabilitado).Count > 0 Then
                        InhabilitadoSelected = InhabilitadoAnterior
                    Else
                        If _ListaInhabilitados.Count > 0 Then
                            InhabilitadoSelected = _ListaInhabilitados.FirstOrDefault
                        Else
                            InhabilitadoSelected = Nothing
                        End If
                    End If
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaInhabilitadosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaInhabilitados) Then
                Dim view = New PagedCollectionView(_ListaInhabilitados)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _InhabilitadoSelected As Inhabilitado
    Public Property InhabilitadoSelected() As Inhabilitado
        Get
            Return _InhabilitadoSelected
        End Get
        Set(ByVal value As Inhabilitado)

            _InhabilitadoSelected = value
            If Not IsNothing(value) Then
                If Not IsNothing(value.tipoidentificacion) Then
                    _InhabilitadoSelected.tipoidentificacion = value.tipoidentificacion.Trim
                End If
            End If

            MyBase.CambioItem("InhabilitadoSelected")
        End Set
    End Property

    Private _EditaReg As Boolean
    Public Property EditaReg() As Boolean
        Get
            Return _EditaReg
        End Get
        Set(ByVal value As Boolean)

            _EditaReg = value
            MyBase.CambioItem("EditaReg")
        End Set
    End Property
    Private _Enabled As Boolean = False
    Public Property Enabled() As Boolean
        Get
            Return _Enabled
        End Get
        Set(ByVal value As Boolean)
            If EditaReg = False Then
                value = False
            Else
                value = True
            End If
            _Enabled = value
            MyBase.CambioItem("Enabled")
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


    Private _habilitar As Boolean = False
    Public Property habilitar() As Boolean
        Get
            Return _habilitar
        End Get
        Set(ByVal value As Boolean)
            _habilitar = value
            MyBase.CambioItem("habilitar")
        End Set
    End Property

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewInhabilitado As New Inhabilitado
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewInhabilitado.idcomisionista = InhabilitadoPorDefecto.idcomisionista
            NewInhabilitado.idsuccomisionista = InhabilitadoPorDefecto.idsuccomisionista
            NewInhabilitado.tipoidentificacion = InhabilitadoPorDefecto.tipoidentificacion
            NewInhabilitado.nrodocumento = InhabilitadoPorDefecto.nrodocumento
            NewInhabilitado.nombre = InhabilitadoPorDefecto.nombre
            NewInhabilitado.idconcepto = InhabilitadoPorDefecto.idconcepto
            NewInhabilitado.ingreso = InhabilitadoPorDefecto.ingreso
            NewInhabilitado.actualizacion = InhabilitadoPorDefecto.actualizacion
            NewInhabilitado.usuario = Program.Usuario
            NewInhabilitado.IDInhabilitado = InhabilitadoPorDefecto.IDInhabilitado

            InhabilitadoAnterior = InhabilitadoSelected
            InhabilitadoSelected = NewInhabilitado
            MyBase.CambioItem("Inhabilitados")
            Editando = True
            EditaReg = True
            Enabled = True
            MyBase.CambioItem("InhabilitadoSelected")
            MyBase.CambioItem("Editando")
            habilitar = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            IsBusy = True
            dcProxy.Inhabilitados.Clear()

            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.InhabilitadosFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerInhabilitados, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.InhabilitadosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerInhabilitados, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro",
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.nrodocumento <> String.Empty Or cb.nombre <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Inhabilitados.Clear()
                InhabilitadoAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " nrodocumento = " &  cb.nrodocumento.ToString() & " nombre = " &  cb.nombre.ToString() 
                dcProxy.Load(dcProxy.InhabilitadosConsultarQuery(cb.nrodocumento, cb.nombre, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerInhabilitados, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaInhabilitado
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro",
                Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try

            If InhabilitadoSelected.ingreso <= "1/1/1752" Then
                A2Utilidades.Mensajes.mostrarMensaje("La fecha no puede ser menor a 1/1/1753", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If InhabilitadoSelected.idconcepto = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar una opción en el campo motivo inhabilidad", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
            Else
                Dim origen = "update"
                ErrorForma = ""
                'InhabilitadoAnterior = InhabilitadoSelected
                If Not ListaInhabilitados.Contains(InhabilitadoSelected) Then
                    origen = "insert"
                    ListaInhabilitados.Add(InhabilitadoSelected)
                End If
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                Dim strMsg As String = String.Empty
                'TODO: Pendiente garantizar que Userstate no venga vacío
                'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                '                       Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                Else
                    If So.Error.Message.Contains("Submit operation failed.") Then
                        Dim Mensaje1 = Split(So.Error.Message, "Submit operation failed.") '.Split(So.Error.Message, vbCr)
                        Dim Mensaje = Split(Mensaje1(1), vbCr)
                        A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje(So.Error.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                    So.MarkErrorAsHandled()
                End If
                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                So.MarkErrorAsHandled()

                'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                '                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)

                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If

                If So.UserState = "insert" Then
                    ListaInhabilitados.Remove(InhabilitadoSelected)
                End If

                So.MarkErrorAsHandled()
                Exit Try
            End If
            If So.UserState = "insert" Then
                IsBusy = True
                dcProxy.Inhabilitados.Clear()

                If FiltroVM.Length > 0 Then
                    Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                    dcProxy.Load(dcProxy.InhabilitadosFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerInhabilitados, "insert")
                Else
                    dcProxy.Load(dcProxy.InhabilitadosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerInhabilitados, "insert")
                End If
            End If
            MyBase.TerminoSubmitChanges(So)
            EditaReg = False
            Enabled = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_InhabilitadoSelected) Then
            Editando = True
            EditaReg = False
            Enabled = False
            habilitar = False
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""

            If Not IsNothing(_InhabilitadoSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                EditaReg = False
                Enabled = False

                If _InhabilitadoSelected.EntityState = EntityState.Detached Then
                    InhabilitadoSelected = InhabilitadoAnterior
                End If
            End If
            habilitar = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_InhabilitadoSelected) Then
                dcProxy.Inhabilitados.Remove(_InhabilitadoSelected)
                InhabilitadoSelected = _ListaInhabilitados.LastOrDefault
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
            End If
            habilitar = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro",
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _InhabilitadoSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _InhabilitadoSelected.PropertyChanged

        If e.PropertyName.Equals("nrodocumento") Then
            For Each led In ListaInhabilitados
                If led.nrodocumento = InhabilitadoSelected.nrodocumento Then
                    A2Utilidades.Mensajes.mostrarMensaje("El Documento de Identeidad " + led.nrodocumento + " Ya se encuentra matriculado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            Next
        End If
    End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub
    Public Sub llenarDiccionario()
        DicCamposTab.Add("tipoidentificacion", 1)
        DicCamposTab.Add("nrodocumento", 1)
        DicCamposTab.Add("nombre", 1)
    End Sub
    Public Overrides Sub Buscar()
        cb.nombre = String.Empty
        cb.nrodocumento = String.Empty
        MyBase.Buscar()
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaInhabilitado
    Implements INotifyPropertyChanged
    Private _nrodocumento As String
    <StringLength(15, ErrorMessage:="La longitud máxima es de 15")>
    <Display(Name:="Doc. Identidad")>
    Public Property nrodocumento As String
        Get
            Return _nrodocumento
        End Get
        Set(ByVal value As String)
            _nrodocumento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("nrodocumento"))
        End Set
    End Property
    Private _nombre As String
    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")>
    <Display(Name:="Nombre")>
    Public Property nombre As String
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("nombre"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class




