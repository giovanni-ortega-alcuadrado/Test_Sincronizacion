Imports Telerik.Windows.Controls

'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ClasificacionRiesgoViewModel.vb
'Generado el : 24/01/2014 
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

Public Class ClasificacionRiesgoViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaClasificacionRiesgo
    Private ClasificacionRiesgoPorDefecto As ClasificacionRiesgo
    Private ClasificacionRiesgoAnterior As ClasificacionRiesgo

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
                dcProxy.Load(dcProxy.ClasificacionRiesgoFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificacionriesgo, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerClasificacionRiesgoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificacionRiesgoPorDefecto_Completed, "Default")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "CiudadesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

#Region "Propiedades"

    Private _ListaClasificacionRiesgo As EntitySet(Of ClasificacionRiesgo)
    Public Property ListaClasificacionRiesgo() As EntitySet(Of ClasificacionRiesgo)
        Get
            Return _ListaClasificacionRiesgo
        End Get
        Set(ByVal value As EntitySet(Of ClasificacionRiesgo))
            _ListaClasificacionRiesgo = value

            MyBase.CambioItem("ListaClasificacionRiesgo")
            MyBase.CambioItem("ListaClasificacionRiesgoPaged")
            If Not IsNothing(value) Then
                If IsNothing(ClasificacionRiesgoAnterior) Then
                    ClasificacionRiesgoSelected = _ListaClasificacionRiesgo.FirstOrDefault
                Else
                    ClasificacionRiesgoSelected = ClasificacionRiesgoAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaClasificacionRiesgoPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaClasificacionRiesgo) Then
                Dim view = New PagedCollectionView(_ListaClasificacionRiesgo)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ClasificacionRiesgoSelected As ClasificacionRiesgo
    Public Property ClasificacionRiesgoSelected() As ClasificacionRiesgo
        Get
            Return _ClasificacionRiesgoSelected
        End Get
        Set(ByVal value As ClasificacionRiesgo)
            _ClasificacionRiesgoSelected = value
            MyBase.CambioItem("ClasificacionRiesgoSelected")
        End Set
    End Property

    Private _Editareg As Boolean
    Public Property Editareg() As Boolean
        Get
            Return _Editareg
        End Get
        Set(ByVal value As Boolean)
            _Editareg = value
            MyBase.CambioItem("Editareg")
        End Set
    End Property

    Private _Limpiar As Integer
    Public Property Limpiar As Integer
        Get
            Return _Limpiar
        End Get
        Set(ByVal value As Integer)
            _Limpiar = value
            MyBase.CambioItem("Limpiar")
        End Set
    End Property

#End Region


#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewClasificacionRiesgo As New ClasificacionRiesgo
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewClasificacionRiesgo.IdClasificacionRiesgo = ClasificacionRiesgoPorDefecto.IdClasificacionRiesgo
            NewClasificacionRiesgo.IdTipoClasificacionRiesgo = ClasificacionRiesgoPorDefecto.IdTipoClasificacionRiesgo
            NewClasificacionRiesgo.Prefijo = ClasificacionRiesgoPorDefecto.Prefijo
            NewClasificacionRiesgo.Detalle = ClasificacionRiesgoPorDefecto.Detalle
            NewClasificacionRiesgo.Usuario = Program.Usuario
            NewClasificacionRiesgo.GenerarAlerta = ClasificacionRiesgoPorDefecto.GenerarAlerta
            ClasificacionRiesgoAnterior = ClasificacionRiesgoSelected
            ClasificacionRiesgoSelected = NewClasificacionRiesgo
            PropiedadTextoCombos = ""
            Limpiar = 0
            MyBase.CambioItem("ClasificacionRiesgo")
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
            dcProxy.ClasificacionRiesgos.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ClasificacionRiesgoFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificacionriesgo, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.ClasificacionRiesgoFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificacionriesgo, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.IdClasificacionRiesgo <> 0 Or cb.IdTipoClasificacionRiesgo <> 0 Or cb.Prefijo <> String.Empty Or cb.Detalle <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.ClasificacionRiesgos.Clear()
                ClasificacionRiesgoAnterior = Nothing
                IsBusy = True
                dcProxy.Load(dcProxy.ClasificacionRiesgoConsultarQuery(cb.IdClasificacionRiesgo, cb.IdTipoClasificacionRiesgo, cb.Prefijo, cb.Detalle,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificacionriesgo, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaClasificacionRiesgo
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
            If ClasificacionRiesgoSelected.IdTipoClasificacionRiesgo = 0 Or IsNothing(ClasificacionRiesgoSelected.IdTipoClasificacionRiesgo) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el tipo de clasificación", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            Dim origen = "update"
            ErrorForma = ""
            ClasificacionRiesgoAnterior = ClasificacionRiesgoSelected

            If Not ListaClasificacionRiesgo.Contains(ClasificacionRiesgoSelected) Then
                origen = "insert"
                ListaClasificacionRiesgo.Add(ClasificacionRiesgoSelected)
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
        If Not IsNothing(_ClasificacionRiesgoSelected) Then
            Editando = True
            Editareg = False
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ClasificacionRiesgoSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                Editareg = False
                If _ClasificacionRiesgoSelected.EntityState = EntityState.Detached Then
                    ClasificacionRiesgoSelected = ClasificacionRiesgoAnterior
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
            cb = New CamposBusquedaClasificacionRiesgo
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
            If Not IsNothing(_ClasificacionRiesgoSelected) Then
                IsBusy = True
                dcProxy.EliminarClasificacionRiesgo(ClasificacionRiesgoSelected.IdClasificacionRiesgo, String.Empty,Program.Usuario, Program.HashConexion, AddressOf terminoeliminar, "borrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
    '    If DicCamposTab.ContainsKey(pstrNombreCampo) Then
    '        Dim miTab = DicCamposTab(pstrNombreCampo)
    '        TabSeleccionadaFinanciero = miTab
    '    End If
    'End Sub
    'Public Sub llenarDiccionario()
    '    DicCamposTab.Add("Nombre", 1)
    'End Sub

    Public Overrides Sub Buscar()
        cb.PropiedadTextoCombos = ""
        MyBase.Buscar()
    End Sub
#End Region


#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerClasificacionRiesgoPorDefecto_Completed(ByVal lo As LoadOperation(Of ClasificacionRiesgo))
        If Not lo.HasError Then
            ClasificacionRiesgoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Ciudade por defecto", _
                                             Me.ToString(), "TerminoTraerCiudadePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerClasificacionriesgo(ByVal lo As LoadOperation(Of ClasificacionRiesgo))
        If Not lo.HasError Then
            ListaClasificacionRiesgo = dcProxy.ClasificacionRiesgos
            If dcProxy.ClasificacionRiesgos.Count > 0 Then
                If lo.UserState = "insert" Then
                    ClasificacionRiesgoSelected = ListaClasificacionRiesgo.FirstOrDefault
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
                    dcProxy.ClasificacionRiesgos.Clear()
                    dcProxy.Load(dcProxy.ClasificacionRiesgoFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificacionriesgo, "insert") ' Recarga la lista para que carguen los include
                End If
            End If
        End If
        IsBusy = False
    End Sub
#End Region

End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaClasificacionRiesgo
    Implements INotifyPropertyChanged

    <Display(Name:="Código")> _
    Public Property IdClasificacionRiesgo As Integer

    <Display(Name:="Tipo Clasificacion Riesgo")> _
    Public Property IdTipoClasificacionRiesgo As Integer

    <StringLength(20, ErrorMessage:="La longitud máxima es de 20")> _
    <Display(Name:="Prefijo")> _
    Public Property Prefijo As String

    <StringLength(100, ErrorMessage:="La longitud máxima es de 100")> _
    <Display(Name:="Detalle")> _
    Public Property Detalle As String

    Private _PropiedadTextoCombos As String
    Public Property PropiedadTextoCombos() As String
        Get
            Return _PropiedadTextoCombos
        End Get
        Set(ByVal value As String)
            _PropiedadTextoCombos = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("PropiedadTextoCombos"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class


