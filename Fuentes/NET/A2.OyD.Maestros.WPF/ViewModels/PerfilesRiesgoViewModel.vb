Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: PerfilesRiesgoViewModel.vb
'Generado el : 07/22/2014 09:37:28
'Propiedad de Alcuadrado S.A. 2014

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web

Public Class PerfilesRiesgoViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaPerfilesRiesgo
    Private PerfilesriesgoPorDefecto As PerfilesRiesgo
    Private PerfilesRiesgoAnterior As PerfilesRiesgo
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim IDRegistro As Integer = 0

    Public Enum OpcionTipoPerfil
        Ninguno
        Departamento
        Mesa
        Sector
        TipoProducto
    End Enum

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New MaestrosDomainContext()
                dcProxy1 = New MaestrosDomainContext()
            Else
                dcProxy = New MaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy1 = New MaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
            End If
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.PerfilesRiesgoFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPerfilesRiesgo, "")
                dcProxy1.Load(dcProxy1.TraerPerfilesRiesgoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPerfilesRiesgoPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  PerfilesRiesgoViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "PerfilesRiesgoViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerPerfilesRiesgoPorDefecto_Completed(ByVal lo As LoadOperation(Of PerfilesRiesgo))
        If Not lo.HasError Then
            PerfilesriesgoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Perfiles Riesgo por defecto", _
                                             Me.ToString(), "TerminoTraerPerfilesRiesgoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerPerfilesRiesgo(ByVal lo As LoadOperation(Of PerfilesRiesgo))
        If Not lo.HasError Then
            ListaPerfilesRiesgo = dcProxy.PerfilesRiesgos
            If dcProxy.PerfilesRiesgos.Count > 0 Then
                If lo.UserState = "insert" Or lo.UserState = "update" Then
                    If IDRegistro <> 0 Then
                        If _ListaPerfilesRiesgo.Where(Function(i) i.PerfilRiesgo = IDRegistro).Count > 0 Then
                            PerfilesRiesgoSelected = _ListaPerfilesRiesgo.Where(Function(i) i.PerfilRiesgo = IDRegistro).First
                        End If
                        IDRegistro = 0
                    Else
                        PerfilesRiesgoSelected = ListaPerfilesRiesgo.Last
                    End If
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    MessageBox.Show("No se encontró ningún registro")
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista del Perfil de Riesgo", _
                                             Me.ToString(), "TerminoTraerPerfilesRiesgo", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub




#End Region

#Region "Propiedades"

    Private _ListaPerfilesRiesgo As EntitySet(Of PerfilesRiesgo)
    Public Property ListaPerfilesRiesgo() As EntitySet(Of PerfilesRiesgo)
        Get
            Return _ListaPerfilesRiesgo
        End Get
        Set(ByVal value As EntitySet(Of PerfilesRiesgo))
            _ListaPerfilesRiesgo = value

            MyBase.CambioItem("ListaPerfilesRiesgo")
            MyBase.CambioItem("ListaPerfilesRiesgoPaged")
            If Not IsNothing(value) Then
                If IsNothing(PerfilesRiesgoAnterior) Then
                    PerfilesRiesgoSelected = _ListaPerfilesRiesgo.FirstOrDefault
                Else
                    PerfilesRiesgoSelected = PerfilesRiesgoAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaPerfilesRiesgoPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaPerfilesRiesgo) Then
                Dim view = New PagedCollectionView(_ListaPerfilesRiesgo)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _PerfilesRiesgoSelected As PerfilesRiesgo
    Public Property PerfilesRiesgoSelected() As PerfilesRiesgo
        Get
            Return _PerfilesRiesgoSelected
        End Get
        Set(ByVal value As PerfilesRiesgo)
            _PerfilesRiesgoSelected = value
            If Not IsNothing(_PerfilesRiesgoSelected) Then
                HabilitarComboPerfil()
            End If
            MyBase.CambioItem("PerfilesRiesgoSelected")
        End Set
    End Property

    Private _MostrarComboDepartamentos As Visibility = Visibility.Collapsed
    Public Property MostrarComboDepartamentos() As Visibility
        Get
            Return _MostrarComboDepartamentos
        End Get
        Set(ByVal value As Visibility)
            _MostrarComboDepartamentos = value
            MyBase.CambioItem("MostrarComboDepartamentos")
        End Set
    End Property

    Private _MostrarComboMesas As Visibility = Visibility.Collapsed
    Public Property MostrarComboMesas() As Visibility
        Get
            Return _MostrarComboMesas
        End Get
        Set(ByVal value As Visibility)
            _MostrarComboMesas = value
            MyBase.CambioItem("MostrarComboMesas")
        End Set
    End Property

    Private _MostrarComboSectores As Visibility = Visibility.Collapsed
    Public Property MostrarComboSectores() As Visibility
        Get
            Return _MostrarComboSectores
        End Get
        Set(ByVal value As Visibility)
            _MostrarComboSectores = value
            MyBase.CambioItem("MostrarComboSectores")
        End Set
    End Property

    Private _MostrarComboTipoProductos As Visibility = Visibility.Collapsed
    Public Property MostrarComboTipoProductos() As Visibility
        Get
            Return _MostrarComboTipoProductos
        End Get
        Set(ByVal value As Visibility)
            _MostrarComboTipoProductos = value
            MyBase.CambioItem("MostrarComboTipoProductos")
        End Set
    End Property


    Private _EditandoTipoPerfil As Boolean
    Public Property EditandoTipoPerfil() As Boolean
        Get
            Return _EditandoTipoPerfil
        End Get
        Set(ByVal value As Boolean)
            _EditandoTipoPerfil = value
            MyBase.CambioItem("EditandoTipoPerfil")
        End Set
    End Property

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewPerfilRiesgo As New PerfilesRiesgo
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewPerfilRiesgo.PerfilRiesgo = PerfilesriesgoPorDefecto.PerfilRiesgo
            NewPerfilRiesgo.TipoPerfil = PerfilesriesgoPorDefecto.TipoPerfil
            NewPerfilRiesgo.IDDescPerfil = PerfilesriesgoPorDefecto.IDDescPerfil
            NewPerfilRiesgo.CalificacionPerfil = PerfilesriesgoPorDefecto.CalificacionPerfil
            PerfilesRiesgoAnterior = PerfilesRiesgoSelected
            PerfilesRiesgoSelected = NewPerfilRiesgo
            MyBase.CambioItem("PerfilesRiesgo")
            Editando = True
            MyBase.CambioItem("Editando")
            EditandoTipoPerfil = True
            HabilitarComboPerfil()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.PerfilesRiesgos.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.PerfilesRiesgoFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPerfilesRiesgo, Nothing)
            Else
                dcProxy.Load(dcProxy.PerfilesRiesgoFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPerfilesRiesgo, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.PerfilRiesgo <> 0 Or cb.TipoPerfil <> 0 Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.PerfilesRiesgos.Clear()
                PerfilesRiesgoAnterior = Nothing
                IsBusy = True
                Dim DescripcionFiltroVM = " ID = " & cb.PerfilRiesgo.ToString() & " TipoPerfil = " & cb.TipoPerfil.ToString()
                dcProxy.Load(dcProxy.PerfilesRiesgoConsultarQuery(cb.PerfilRiesgo, cb.TipoPerfil,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPerfilesRiesgo, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaPerfilesRiesgo
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

            Dim intCantidadRegistros As Integer
            intCantidadRegistros = (From li In ListaPerfilesRiesgo Where li.TipoPerfil = PerfilesRiesgoSelected.TipoPerfil And li.IDDescPerfil = PerfilesRiesgoSelected.IDDescPerfil Select li).Count

            Dim origen = "update"
            ErrorForma = ""
            PerfilesRiesgoAnterior = PerfilesRiesgoSelected
            If Not ListaPerfilesRiesgo.Contains(PerfilesRiesgoSelected) Then
                origen = "insert"
                ListaPerfilesRiesgo.Add(PerfilesRiesgoSelected)
            End If

            If (intCantidadRegistros = 1 And origen = "insert") Or (intCantidadRegistros > 1 And origen = "update") Then
                A2Utilidades.Mensajes.mostrarMensaje("Ya se encuentra configurado el perfil de riesgo.", Application.Current.ToString(), A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
            End If
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
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                So.MarkErrorAsHandled()
                Exit Try
            End If
            MyBase.TerminoSubmitChanges(So)

            If Not IsNothing(So.UserState) Then
                If So.UserState = "update" Or So.UserState = "insert" Then
                    IDRegistro = _PerfilesRiesgoSelected.PerfilRiesgo
                    dcProxy.PerfilesRiesgos.Clear()
                    IsBusy = True

                    dcProxy.Load(dcProxy.PerfilesRiesgoFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPerfilesRiesgo, So.UserState)
                Else
                    IDRegistro = 0
                End If
            Else
                IDRegistro = 0
            End If


        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_PerfilesRiesgoSelected) Then
            Editando = True
            EditandoTipoPerfil = False
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_PerfilesRiesgoSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                EditandoTipoPerfil = False
                If _PerfilesRiesgoSelected.EntityState = EntityState.Detached Then
                    PerfilesRiesgoSelected = PerfilesRiesgoAnterior
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
            If Not IsNothing(_PerfilesRiesgoSelected) Then
                dcProxy.PerfilesRiesgos.Remove(_PerfilesRiesgoSelected)
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub HabilitarComboPerfil()
        Try
            If Not IsNothing(_PerfilesRiesgoSelected) Then
                If _PerfilesRiesgoSelected.TipoPerfil = OpcionTipoPerfil.Departamento Then
                    MostrarComboDepartamentos = Visibility.Visible
                    MostrarComboMesas = Visibility.Collapsed
                    MostrarComboSectores = Visibility.Collapsed
                    MostrarComboTipoProductos = Visibility.Collapsed
                ElseIf _PerfilesRiesgoSelected.TipoPerfil = OpcionTipoPerfil.Mesa Then
                    MostrarComboDepartamentos = Visibility.Collapsed
                    MostrarComboMesas = Visibility.Visible
                    MostrarComboSectores = Visibility.Collapsed
                    MostrarComboTipoProductos = Visibility.Collapsed
                ElseIf _PerfilesRiesgoSelected.TipoPerfil = OpcionTipoPerfil.Sector Then
                    MostrarComboDepartamentos = Visibility.Collapsed
                    MostrarComboMesas = Visibility.Collapsed
                    MostrarComboSectores = Visibility.Visible
                    MostrarComboTipoProductos = Visibility.Collapsed
                ElseIf _PerfilesRiesgoSelected.TipoPerfil = OpcionTipoPerfil.TipoProducto Then
                    MostrarComboDepartamentos = Visibility.Collapsed
                    MostrarComboMesas = Visibility.Collapsed
                    MostrarComboSectores = Visibility.Collapsed
                    MostrarComboTipoProductos = Visibility.Visible
                Else
                    MostrarComboDepartamentos = Visibility.Collapsed
                    MostrarComboMesas = Visibility.Collapsed
                    MostrarComboSectores = Visibility.Collapsed
                    MostrarComboTipoProductos = Visibility.Collapsed
                End If
            Else
                MostrarComboDepartamentos = Visibility.Collapsed
                MostrarComboMesas = Visibility.Collapsed
                MostrarComboSectores = Visibility.Collapsed
                MostrarComboTipoProductos = Visibility.Collapsed
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al mostrar el combo", _
             Me.ToString(), "HabilitarComboPerfil", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _PerfilesRiesgoSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _PerfilesRiesgoSelected.PropertyChanged
        If Editando Then
            If e.PropertyName.Equals("TipoPerfil") Then
                HabilitarComboPerfil()
            End If
        End If
    End Sub
#End Region

End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaPerfilesRiesgo

    <Display(Name:="ID", Description:="ID")> _
    Public Property PerfilRiesgo As Integer

    <Display(Name:="Tipo perfil", Description:="Tipo perfil")> _
    Public Property TipoPerfil As Integer
End Class
