Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ClasificacionRiesgoViewModel.vb
'Generado el : 07/03/2014 
'Propiedad de Alcuadrado S.A. 2010

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OYD.OYDServer.RIA.Web

Public Class ClasificacionRiesgoTipoClienteViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaClasificacionRiesgoTipoCliente
    Private ClasificacionRiesgoTipoClientePorDefecto As ClasificacionRiesgoTipoCliente
    Private ClasificacionRiesgoTipoClienteAnterior As ClasificacionRiesgoTipoCliente

    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    'Dim DicCamposTab As New Dictionary(Of String, Integer)

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosDomainContext()
            dcProxy1 = New MaestrosDomainContext()
        Else
            dcProxy = New MaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy1 = New MaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.ClasificacionRiesgoTipoClienteFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificacionriesgo, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerClasificacionRiesgoTipoClientePorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificacionRiesgoPorDefecto_Completed, "Default")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ClasificacionRiesgoTipoClienteViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

#Region "Propiedades"

    Private _ListaClasificacionRiesgoTipoCliente As EntitySet(Of ClasificacionRiesgoTipoCliente)
    Public Property ListaClasificacionRiesgoTipoCliente() As EntitySet(Of ClasificacionRiesgoTipoCliente)
        Get
            Return _ListaClasificacionRiesgoTipoCliente
        End Get
        Set(ByVal value As EntitySet(Of ClasificacionRiesgoTipoCliente))
            _ListaClasificacionRiesgoTipoCliente = value

            MyBase.CambioItem("ListaClasificacionRiesgoTipoCliente")
            MyBase.CambioItem("ListaClasificacionRiesgoTipoClientePaged")
            If Not IsNothing(value) Then
                If IsNothing(ClasificacionRiesgoTipoClienteAnterior) Then
                    ClasificacionRiesgoTipoClienteSelected = _ListaClasificacionRiesgoTipoCliente.FirstOrDefault
                Else
                    ClasificacionRiesgoTipoClienteSelected = ClasificacionRiesgoTipoClienteAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaClasificacionRiesgoTipoClientePaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaClasificacionRiesgoTipoCliente) Then
                Dim view = New PagedCollectionView(_ListaClasificacionRiesgoTipoCliente)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ClasificacionRiesgoTipoClienteSelected As ClasificacionRiesgoTipoCliente
    Public Property ClasificacionRiesgoTipoClienteSelected() As ClasificacionRiesgoTipoCliente
        Get
            Return _ClasificacionRiesgoTipoClienteSelected
        End Get
        Set(ByVal value As ClasificacionRiesgoTipoCliente)
            _ClasificacionRiesgoTipoClienteSelected = value
            MyBase.CambioItem("ClasificacionRiesgoTipoClienteSelected")
        End Set
    End Property
#End Region


#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewClasificacionRiesgoTipocliente As New ClasificacionRiesgoTipoCliente
            'TODO: Verificar cuales son los campos que deben inicializarse
            'NewClasificacionRiesgoTipocliente.Codigo = Nothing
            NewClasificacionRiesgoTipocliente.ClasificacionTipoCliente = ClasificacionRiesgoTipoClientePorDefecto.ClasificacionTipoCliente
            NewClasificacionRiesgoTipocliente.Usuario = Program.Usuario
            ClasificacionRiesgoTipoClienteAnterior = ClasificacionRiesgoTipoClienteSelected
            ClasificacionRiesgoTipoClienteSelected = NewClasificacionRiesgoTipocliente
            MyBase.CambioItem("ClasificacionRiesgoTipoCliente")
            Editando = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.ClasificacionRiesgoTipoClientes.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ClasificacionRiesgoTipoClienteFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificacionriesgo, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.ClasificacionRiesgoTipoClienteFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificacionriesgo, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.Codigo <> 0 Or cb.ClasificacionTipoCliente <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.ClasificacionRiesgoTipoClientes.Clear()
                ClasificacionRiesgoTipoClienteAnterior = Nothing
                IsBusy = True
                dcProxy.Load(dcProxy.ClasificacionRiesgoTipoClienteConsultarQuery(cb.Codigo, cb.ClasificacionTipoCliente,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificacionriesgo, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaClasificacionRiesgoTipoCliente
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
            'If ClasificacionRiesgoTipoClienteSelected.IdTipoClasificacionRiesgo = 0 Or IsNothing(ClasificacionRiesgoSelected.IdTipoClasificacionRiesgo) Then
            '    A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el tipo de clasificación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Exit Sub
            'End If

            Dim origen = "update"
            ErrorForma = ""
            ClasificacionRiesgoTipoClienteAnterior = ClasificacionRiesgoTipoClienteSelected

            If Not ListaClasificacionRiesgoTipoCliente.Contains(ClasificacionRiesgoTipoClienteSelected) Then
                origen = "insert"
                ListaClasificacionRiesgoTipoCliente.Add(ClasificacionRiesgoTipoClienteSelected)
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
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_ClasificacionRiesgoTipoClienteSelected) Then
            Editando = True
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ClasificacionRiesgoTipoClienteSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _ClasificacionRiesgoTipoClienteSelected.EntityState = EntityState.Detached Then
                    ClasificacionRiesgoTipoClienteSelected = ClasificacionRiesgoTipoClienteAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaClasificacionRiesgoTipoCliente
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la Búsqueda", _
                     Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_ClasificacionRiesgoTipoClienteSelected) Then
                IsBusy = True
                dcProxy.EliminarClasificacionRiesgoTipoCliente(ClasificacionRiesgoTipoClienteSelected.Codigo, String.Empty,Program.Usuario, Program.HashConexion, AddressOf terminoeliminar, "borrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub Buscar()
        'cb.PropiedadTextoCombos = ""
        MyBase.Buscar()
    End Sub
#End Region


#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerClasificacionRiesgoPorDefecto_Completed(ByVal lo As LoadOperation(Of ClasificacionRiesgoTipoCliente))
        If Not lo.HasError Then
            ClasificacionRiesgoTipoClientePorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Ciudade por defecto", _
                                             Me.ToString(), "TerminoTraerCiudadePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerClasificacionriesgo(ByVal lo As LoadOperation(Of ClasificacionRiesgoTipoCliente))
        If Not lo.HasError Then
            ListaClasificacionRiesgoTipoCliente = dcProxy.ClasificacionRiesgoTipoClientes
            If dcProxy.ClasificacionRiesgoTipoClientes.Count > 0 Then
                If lo.UserState = "insert" Then
                    ClasificacionRiesgoTipoClienteSelected = ListaClasificacionRiesgoTipoCliente.FirstOrDefault
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Else
            A2ComunesControl.FuncionesCompartidas.obtenerMensajeValidacionErrorPersonalizado("Se presentó un problema en la obtención de la lista de Clasificacion riesgo", Me.ToString, "TerminoTraerClasificacionriesgo", lo.Error)
            'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Clasificacion riesgo", _
            '                                 Me.ToString(), "TerminoTraerClasificacionriesgo", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private Sub terminoeliminar(ByVal So As InvokeOperation(Of String))
        If So.HasError Then
            A2Utilidades.Mensajes.mostrarMensaje(So.Error.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Else
            If Not (So.Value) = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje(So.Value.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                If So.UserState = "borrar" Then
                    dcProxy.ClasificacionRiesgoTipoClientes.Clear()
                    dcProxy.Load(dcProxy.ClasificacionRiesgoTipoClienteFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificacionriesgo, "insert") ' Recarga la lista para que carguen los include
                End If
            End If
        End If
        IsBusy = False
    End Sub
#End Region

End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaClasificacionRiesgoTipoCliente
    Implements INotifyPropertyChanged

    <Display(Name:="Código")> _
    Public Property Codigo As Integer

    <StringLength(30, ErrorMessage:="La longitud máxima es de 30")> _
    <Display(Name:="ClasificacionTipoCliente")> _
    Public Property ClasificacionTipoCliente As String
    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class





