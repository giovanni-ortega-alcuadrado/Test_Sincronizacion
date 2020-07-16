Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ClasesEspeciesViewModel.vb
'Generado el : 01/20/2011 09:58:14
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

Public Class DestinoInversionViewModel

    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaDestinoInversion
    Private DestinoInversionPorDefecto As DestinoInversion
    Private DestinoInversionAnterior As DestinoInversion
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext

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
                dcProxy.Load(dcProxy.DestinoInversionFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDestinoInversion, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerDestinoInversionPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDestinoInversionPorDefecto_Completed, "Default")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ClasesEspeciesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerDestinoInversionPorDefecto_Completed(ByVal lo As LoadOperation(Of DestinoInversion))
        If Not lo.HasError Then
            DestinoInversionPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ClasesEspecie por defecto", _
                                             Me.ToString(), "TerminoTraerClasesEspeciePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerDestinoInversion(ByVal lo As LoadOperation(Of DestinoInversion))
        If Not lo.HasError Then
            ListaDestinoInversion = dcProxy.DestinoInversions
            If ListaDestinoInversion.Count > 0 Then
                'If lo.UserState = "insert" Then
                '    DestinoInversionSelected = ListaDestinoInversion.Last
                'End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClasesEspecies", _
                                             Me.ToString(), "TerminoTraerClasesEspecie", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private Sub Terminoeliminar(ByVal So As InvokeOperation(Of String))
        If So.HasError Then
            A2Utilidades.Mensajes.mostrarMensaje(So.Error.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Else
            If Not (So.Value) = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje(So.Value.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                If So.UserState = "borrar" Then
                    dcProxy.DestinoInversions.Clear()
                    dcProxy.Load(dcProxy.DestinoInversionFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDestinoInversion, "insert") ' Recarga la lista para que carguen los include
                End If
            End If
        End If
        IsBusy = False
    End Sub


#End Region

#Region "Propiedades"

    Private _ListaDestinoInversion As EntitySet(Of DestinoInversion)
    Public Property ListaDestinoInversion() As EntitySet(Of DestinoInversion)
        Get
            Return _ListaDestinoInversion
        End Get
        Set(ByVal value As EntitySet(Of DestinoInversion))
            _ListaDestinoInversion = value

            MyBase.CambioItem("ListaDestinoInversion")
            MyBase.CambioItem("ListaDestinoInversionPaged")
            If Not IsNothing(value) Then
                If IsNothing(DestinoInversionAnterior) Then
                    DestinoInversionSelected = _ListaDestinoInversion.FirstOrDefault
                Else
                    DestinoInversionSelected = DestinoInversionAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaDestinoInversionPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDestinoInversion) Then
                Dim view = New PagedCollectionView(_ListaDestinoInversion)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _DestinoInversionSelected As DestinoInversion
    Public Property DestinoInversionSelected() As DestinoInversion
        Get
            Return _DestinoInversionSelected
        End Get
        Set(ByVal value As DestinoInversion)
            _DestinoInversionSelected = value
            If Not value Is Nothing Then
            End If
            MyBase.CambioItem("DestinoInversionSelected")
        End Set
    End Property

    Private _HabilitarCodigo As Boolean = False
    Public Property HabilitarCodigo As Boolean
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
            Dim NewDestinoInversion As New DestinoInversion

            NewDestinoInversion.IDDestinoInversion = DestinoInversionPorDefecto.IDDestinoInversion
            NewDestinoInversion.IDComisionista = DestinoInversionPorDefecto.IDComisionista
            NewDestinoInversion.IDSucComisionista = DestinoInversionPorDefecto.IDSucComisionista
            NewDestinoInversion.IDDestino = DestinoInversionPorDefecto.IDDestino
            NewDestinoInversion.NombreDestino = DestinoInversionPorDefecto.NombreDestino
            NewDestinoInversion.Actualizacion = Now
            NewDestinoInversion.Usuario = Program.Usuario

            DestinoInversionAnterior = DestinoInversionSelected
            DestinoInversionSelected = NewDestinoInversion
            MyBase.CambioItem("DestinoInversionSelected")
            Editando = True
            HabilitarCodigo = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            IsBusy = True
            dcProxy.DestinoInversions.Clear()
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.DestinoInversionFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDestinoInversion, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.DestinoInversionFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDestinoInversion, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If Not IsNothing(cb.IDDestino) Or cb.NombreDestino <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.DestinoInversions.Clear()
                DestinoInversionAnterior = Nothing
                IsBusy = True
                'cb.IDClaseEspecie = IIf(IsNothing(cb.IDClaseEspecie), -1, cb.IDClaseEspecie)
                dcProxy.Load(dcProxy.DestinoInversionConsultarQuery(cb.IDDestino, cb.NombreDestino, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDestinoInversion, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaDestinoInversion
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
            'DestinoInversionAnterior = DestinoInversionSelected
            If Not ListaDestinoInversion.Contains(DestinoInversionSelected) Then
                origen = "insert"
                ListaDestinoInversion.Add(DestinoInversionSelected)
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
            IsBusy = False
            If So.HasError Then
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And ((So.UserState = "insert") Or (So.UserState = "update") Or (So.UserState = "BorrarRegistro")) Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,")
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'If So.UserState = "insert" Then
                    '    ListaEmpleados.Remove(EmpleadoSelected)
                    'End If
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                      Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                End If

                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                    DestinoInversionSelected = ListaDestinoInversion.LastOrDefault
                End If

                So.MarkErrorAsHandled()
                Exit Try
            End If

            MyBase.QuitarFiltroDespuesGuardar()
            dcProxy.DestinoInversions.Clear()
            dcProxy.Load(dcProxy.DestinoInversionFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDestinoInversion, "insert") ' Recarga la lista para que carguen los include

            HabilitarCodigo = False
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_DestinoInversionSelected) Then
            Editando = True
            HabilitarCodigo = False
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_DestinoInversionSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                HabilitarCodigo = False
                If _DestinoInversionSelected.EntityState = EntityState.Detached Then
                    DestinoInversionSelected = DestinoInversionAnterior
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
            If Not IsNothing(_DestinoInversionSelected) Then
                IsBusy = True
                dcProxy.DestinoInversions.Remove(_DestinoInversionSelected)
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                'dcProxy.EliminarClasesEspecie(ClasesEspecieSelected.IDClasesEspecies, ClasesEspecieSelected.Usuario, String.Empty,Program.Usuario, Program.HashConexion, AddressOf Terminoeliminar, "borrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaDestinoInversion
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la Búsqueda", _
                     Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        cb.IDDestino = Nothing
        MyBase.Buscar()
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaDestinoInversion

    <Display(Name:="Código")> _
    Public Property IDDestino As Integer?

    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")> _
     <Display(Name:="Nombre")> _
    Public Property NombreDestino As String

End Class




