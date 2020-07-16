Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: UsuariosFechaCierreViewModel.vb
'Generado el : 04/26/2011 15:28:56
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

Public Class UsuariosFechaCierreViewModel
	Inherits A2ControlMenu.A2ViewModel
	Public Property cb As New CamposBusquedaUsuariosFechaCierr
    Private UsuariosFechaCierrPorDefecto As UsuariosFechaCierr
	Private UsuariosFechaCierrAnterior As UsuariosFechaCierr
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim dcProxyUtilidades As UtilidadesDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)

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
                dcProxyUtilidades.Load(dcProxyUtilidades.cargarCombosCondicionalQuery("MODULOS_USUARIOSCIERRE", String.Empty, 0, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarMetodos, String.Empty)
                dcProxy.Load(dcProxy.UsuariosFechaCierreFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUsuariosFechaCierre, "")
                dcProxy1.Load(dcProxy1.TraerUsuariosFechaCierrPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUsuariosFechaCierrePorDefecto_Completed, "Default")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "UsuariosFechaCierreViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoConsultarMetodos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        If Not lo.HasError Then
            Tabladisponibles = lo.Entities.ToList
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la UsuariosFechaCierr por defecto",
                                             Me.ToString(), "TerminoConsultarMetodos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerUsuariosFechaCierrePorDefecto_Completed(ByVal lo As LoadOperation(Of UsuariosFechaCierr))
        If Not lo.HasError Then
            UsuariosFechaCierrPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la UsuariosFechaCierr por defecto", _
                                             Me.ToString(), "TerminoTraerUsuariosFechaCierrPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerUsuariosFechaCierre(ByVal lo As LoadOperation(Of UsuariosFechaCierr))
        If Not lo.HasError Then
            ListaUsuariosFechaCierre = dcProxy.UsuariosFechaCierrs

            If dcProxy.UsuariosFechaCierrs.Count > 0 Then
                If lo.UserState = "insert" Then
                    UsuariosFechaCierrSelected = ListaUsuariosFechaCierre.Last
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
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de UsuariosFechaCierre", _
                                             Me.ToString(), "TerminoTraerUsuariosFechaCierr", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    'Tablas padres



#End Region

#Region "Propiedades"

    Private _ListaUsuariosFechaCierre As EntitySet(Of UsuariosFechaCierr)
    Public Property ListaUsuariosFechaCierre() As EntitySet(Of UsuariosFechaCierr)
        Get
            Return _ListaUsuariosFechaCierre
        End Get
        Set(ByVal value As EntitySet(Of UsuariosFechaCierr))
            _ListaUsuariosFechaCierre = value

            MyBase.CambioItem("ListaUsuariosFechaCierre")
            MyBase.CambioItem("ListaUsuariosFechaCierrePaged")
            If Not IsNothing(value) Then
                If IsNothing(UsuariosFechaCierrAnterior) Then
                    UsuariosFechaCierrSelected = _ListaUsuariosFechaCierre.FirstOrDefault
                Else
                    UsuariosFechaCierrSelected = UsuariosFechaCierrAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaUsuariosFechaCierrePaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaUsuariosFechaCierre) Then
                Dim view = New PagedCollectionView(_ListaUsuariosFechaCierre)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _UsuariosFechaCierrSelected As UsuariosFechaCierr
    Public Property UsuariosFechaCierrSelected() As UsuariosFechaCierr
        Get
            Return _UsuariosFechaCierrSelected
        End Get
        Set(ByVal value As UsuariosFechaCierr)
            _UsuariosFechaCierrSelected = value
            MyBase.CambioItem("UsuariosFechaCierrSelected")
        End Set
    End Property

    Private _Tabladisponibles As List(Of OYDUtilidades.ItemCombo) = New List(Of OYDUtilidades.ItemCombo)
    Public Property Tabladisponibles() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _Tabladisponibles
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _Tabladisponibles = value
            MyBase.CambioItem("Tabladisponibles")
        End Set
    End Property

    Private _Editareg As Boolean
    Public Property Editareg As Boolean
        Get
            Return _Editareg
        End Get
        Set(ByVal value As Boolean)
            If Enabled = True Then
                value = True
            Else
                value = False
            End If
            _Editareg = value
            MyBase.CambioItem("Editareg")
        End Set
    End Property
    Private _Enabled As Boolean
    Public Property Enabled As Boolean
        Get
            Return _Enabled
        End Get
        Set(ByVal value As Boolean)
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
#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewUsuariosFechaCierr As New UsuariosFechaCierr
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewUsuariosFechaCierr.IDComisionista = UsuariosFechaCierrPorDefecto.IDComisionista
            NewUsuariosFechaCierr.IDSucComisionista = UsuariosFechaCierrPorDefecto.IDSucComisionista
            NewUsuariosFechaCierr.Nombre_Usuario = UsuariosFechaCierrPorDefecto.Nombre_Usuario
            NewUsuariosFechaCierr.Modulo = UsuariosFechaCierrPorDefecto.Modulo
            NewUsuariosFechaCierr.Fecha_Cierre = UsuariosFechaCierrPorDefecto.Fecha_Cierre
            NewUsuariosFechaCierr.Actualizacion = UsuariosFechaCierrPorDefecto.Actualizacion
            NewUsuariosFechaCierr.Usuario = Program.Usuario
            UsuariosFechaCierrAnterior = UsuariosFechaCierrSelected
            UsuariosFechaCierrSelected = NewUsuariosFechaCierr
            MyBase.CambioItem("UsuariosFechaCierre")
            Editando = True
            Enabled = True
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
            dcProxy.UsuariosFechaCierrs.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.UsuariosFechaCierreFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUsuariosFechaCierre, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.UsuariosFechaCierreFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUsuariosFechaCierre, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.Nombre_Usuario <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.UsuariosFechaCierrs.Clear()
                UsuariosFechaCierrAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " Nombre_Usuario = " &  cb.Nombre_Usuario.ToString() 
                dcProxy.Load(dcProxy.UsuariosFechaCierreConsultarQuery(cb.Nombre_Usuario, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUsuariosFechaCierre, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaUsuariosFechaCierr
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

            Dim origen = "update"
            ErrorForma = ""
            'UsuariosFechaCierrAnterior = UsuariosFechaCierrSelected
            If ListaUsuariosFechaCierre.Where(Function(i) i.ID = UsuariosFechaCierrSelected.ID).Count = 0 Then
                For Each a In ListaUsuariosFechaCierre
                    If a.Nombre_Usuario.Equals(UsuariosFechaCierrSelected.Nombre_Usuario) And a.Modulo.Equals(UsuariosFechaCierrSelected.Modulo) Then
                        A2Utilidades.Mensajes.mostrarMensaje("Ya existe una fecha de cierre para el usuario y el módulo seleccionado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        'MessageBox.Show("el usuario ya existe")
                        'UsuariosFechaCierrSelected = UsuariosFechaCierrAnterior
                        'CancelarEditarRegistro()
                        Exit Sub
                    End If
                Next
                origen = "insert"
                ListaUsuariosFechaCierre.Add(UsuariosFechaCierrSelected)
            End If
            UsuariosFechaCierrAnterior = UsuariosFechaCierrSelected
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
            IsBusy = False
            If So.HasError Then
                Dim strMsg As String = String.Empty
                'TODO: Pendiente garantizar que Userstate no venga vacío
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                Else
                    If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And ((So.UserState = "insert") Or (So.UserState = "update")) Then
                        Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,")
                        Dim Mensaje = Split(Mensaje1(1), vbCr)
                        strMsg = Mensaje(0)
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje(So.Error.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

                So.MarkErrorAsHandled()
                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If
                Exit Try
            End If
            'If So.UserState = "insert" Then
            '    dcProxy.UsuariosFechaCierrs.Clear()
            '    dcProxy.Load(dcProxy.UsuariosFechaCierreFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUsuariosFechaCierre, "insert") ' Recarga la lista para que carguen los include
            'End If
            MyBase.TerminoSubmitChanges(So)
            Enabled = False
            Editareg = False

            If So.UserState = "insert" Then
                dcProxy.UsuariosFechaCierrs.Clear()
                IsBusy = True
                If FiltroVM.Length > 0 Then
                    Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                    dcProxy.Load(dcProxy.UsuariosFechaCierreFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUsuariosFechaCierre, "insert")
                Else
                    dcProxy.Load(dcProxy.UsuariosFechaCierreFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerUsuariosFechaCierre, "insert")
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_UsuariosFechaCierrSelected) Then
            Editando = True
            Enabled = False
            Editareg = False
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_UsuariosFechaCierrSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                Enabled = False
                Editareg = False
                If _UsuariosFechaCierrSelected.EntityState = EntityState.Detached Then
                    UsuariosFechaCierrSelected = UsuariosFechaCierrAnterior
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
            If Not IsNothing(_UsuariosFechaCierrSelected) Then
                dcProxy.UsuariosFechaCierrs.Remove(_UsuariosFechaCierrSelected)
                UsuariosFechaCierrSelected = _ListaUsuariosFechaCierre.LastOrDefault
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
        DicCamposTab.Add("Nombre_Usuario", 1)
        DicCamposTab.Add("Modulo", 1)
    End Sub

    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaUsuariosFechaCierr
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la búsqueda", _
             Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaUsuariosFechaCierr
 	
    <StringLength(60, ErrorMessage:="La longitud máxima es de 60")> _
     <Display(Name:="Nombre Usuario")> _
    Public Property Nombre_Usuario As String
End Class
Public Class combomodulo

    '<StringLength(15, ErrorMessage:="La longitud máxima es de 15")> _
    <Display(Name:="Modulo")> _
    Public Property Modulo As String

    <Display(Name:="Descripcion")> _
    Public Property Descripcion As String


End Class




