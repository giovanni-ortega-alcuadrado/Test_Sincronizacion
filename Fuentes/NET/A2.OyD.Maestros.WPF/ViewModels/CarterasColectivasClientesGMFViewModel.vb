Imports Telerik.Windows.Controls
'Archivo: CarterasColectivasClientesGMFViewModel.vb
'Generado el : 21/10/2014 
'Jorge Andrés Bedoya

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web

Public Class CarterasColectivasClientesGMFViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaCarterasColectivasClientesGMF
    Private CarterasColectivasClientesGMFPorDefecto As CarterasColectivasClientesGMF
    Private CarterasColectivasClientesGMFAnterior As CarterasColectivasClientesGMF

    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext

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
                dcProxy.Load(dcProxy.CarterasColectivasClientesGMFFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCarterasColectivasClientesGMF, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerCarterasColectivasClientesGMFPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCarterasColectivasClientesGMFPorDefecto_Completed, "Default")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "CarterasColectivasClientesGMFViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

#Region "Propiedades"

    Private _ListaCarterasColectivasClientesGMF As EntitySet(Of CarterasColectivasClientesGMF)
    Public Property ListaCarterasColectivasClientesGMF() As EntitySet(Of CarterasColectivasClientesGMF)
        Get
            Return _ListaCarterasColectivasClientesGMF
        End Get
        Set(ByVal value As EntitySet(Of CarterasColectivasClientesGMF))
            _ListaCarterasColectivasClientesGMF = value

            MyBase.CambioItem("ListaCarterasColectivasClientesGMF")
            MyBase.CambioItem("ListaCarterasColectivasClientesGMFPaged")
            If Not IsNothing(value) Then
                If IsNothing(ListaCarterasColectivasClientesGMF) Then
                    CarterasColectivasClientesGMFSelected = _ListaCarterasColectivasClientesGMF.FirstOrDefault
                Else
                    CarterasColectivasClientesGMFSelected = CarterasColectivasClientesGMFAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaCarterasColectivasClientesGMFPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCarterasColectivasClientesGMF) Then
                Dim view = New PagedCollectionView(_ListaCarterasColectivasClientesGMF)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _CarterasColectivasClientesGMFSelected As CarterasColectivasClientesGMF
    Public Property CarterasColectivasClientesGMFSelected() As CarterasColectivasClientesGMF
        Get
            Return _CarterasColectivasClientesGMFSelected
        End Get
        Set(ByVal value As CarterasColectivasClientesGMF)
            _CarterasColectivasClientesGMFSelected = value
            MyBase.CambioItem("CarterasColectivasClientesGMFSelected")
        End Set
    End Property

    Private _EditarDocumento As Boolean
    Public Property EditarDocumento() As Boolean
        Get
            Return _EditarDocumento
        End Get
        Set(ByVal value As Boolean)
            _EditarDocumento = value
            MyBase.CambioItem("EditarDocumento")
        End Set
    End Property


#End Region


#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewCarterasColectivasClientesGMF As New CarterasColectivasClientesGMF
            NewCarterasColectivasClientesGMF.IDCarterasColectivasClientesGMF = CarterasColectivasClientesGMFPorDefecto.IDCarterasColectivasClientesGMF
            NewCarterasColectivasClientesGMF.IDComitente = CarterasColectivasClientesGMFPorDefecto.IDComitente
            NewCarterasColectivasClientesGMF.TipoIdentificacion = CarterasColectivasClientesGMFPorDefecto.TipoIdentificacion
            NewCarterasColectivasClientesGMF.NroDocumento = CarterasColectivasClientesGMFPorDefecto.NroDocumento
            NewCarterasColectivasClientesGMF.Nombre = CarterasColectivasClientesGMFPorDefecto.Nombre
            NewCarterasColectivasClientesGMF.Usuario = Program.Usuario
            NewCarterasColectivasClientesGMF.Actualizacion = Now
            CarterasColectivasClientesGMFAnterior = CarterasColectivasClientesGMFSelected
            CarterasColectivasClientesGMFSelected = NewCarterasColectivasClientesGMF

            MyBase.CambioItem("CarterasColectivasClientesGMF")
            Editando = True

            EditarDocumento = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.CarterasColectivasClientesGMFs.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.CarterasColectivasClientesGMFFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCarterasColectivasClientesGMF, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.CarterasColectivasClientesGMFFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCarterasColectivasClientesGMF, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.tipoidentificacion <> String.Empty Or cb.Nrodocumento <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.CarterasColectivasClientesGMFs.Clear()
                CarterasColectivasClientesGMFAnterior = Nothing
                IsBusy = True
                dcProxy.Load(dcProxy.CarterasColectivasClientesGMFConsultarQuery(cb.tipoidentificacion, cb.Nrodocumento, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCarterasColectivasClientesGMF, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaCarterasColectivasClientesGMF
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

            If IsNothing(CarterasColectivasClientesGMFSelected.TipoIdentificacion) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el tipo de documento", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If IsNothing(CarterasColectivasClientesGMFSelected.NroDocumento) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el número de documento", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            Dim origen = "update"
            ErrorForma = ""

            If Not ListaCarterasColectivasClientesGMF.Contains(CarterasColectivasClientesGMFSelected) Then
                origen = "insert"
                ListaCarterasColectivasClientesGMF.Add(CarterasColectivasClientesGMFSelected)
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

            EditarDocumento = False

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_CarterasColectivasClientesGMFSelected) Then
            Editando = True
            _CarterasColectivasClientesGMFSelected.Usuario = Program.Usuario
            _CarterasColectivasClientesGMFSelected.Actualizacion = Now
            EditarDocumento = False
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub


    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_CarterasColectivasClientesGMFSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _CarterasColectivasClientesGMFSelected.EntityState = EntityState.Detached Then
                    CarterasColectivasClientesGMFSelected = CarterasColectivasClientesGMFAnterior
                End If
                EditarDocumento = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaCarterasColectivasClientesGMF
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
            If Not IsNothing(_CarterasColectivasClientesGMFSelected) Then
                dcProxy.CarterasColectivasClientesGMFs.Remove(_CarterasColectivasClientesGMFSelected)
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                CarterasColectivasClientesGMFSelected = ListaCarterasColectivasClientesGMF.LastOrDefault
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        MyBase.Buscar()
    End Sub
#End Region


#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerCarterasColectivasClientesGMFPorDefecto_Completed(ByVal lo As LoadOperation(Of CarterasColectivasClientesGMF))
        If Not lo.HasError Then
            CarterasColectivasClientesGMFPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la cartera colectiva cliente GMF por defecto", _
                                             Me.ToString(), "TerminoTraerCarterasColectivasClientesGMFPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub

    Private Sub TerminoTraerCarterasColectivasClientesGMF(ByVal lo As LoadOperation(Of CarterasColectivasClientesGMF))
        If Not lo.HasError Then
            ListaCarterasColectivasClientesGMF = dcProxy.CarterasColectivasClientesGMFs
            If dcProxy.CarterasColectivasClientesGMFs.Count > 0 Then
                If lo.UserState = "insert" Then
                    CarterasColectivasClientesGMFSelected = ListaCarterasColectivasClientesGMF.FirstOrDefault
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de la cartera colectiva clientes GMF", _
                                             Me.ToString(), "TerminoTraerCarterasColectivasClientesGMF", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
        IsBusy = False
    End Sub
    
#End Region

End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaCarterasColectivasClientesGMF
    Implements INotifyPropertyChanged

    <Display(Name:="Tipo documento")> _
    Public Property tipoidentificacion As String

    <StringLength(20, ErrorMessage:="La longitud máxima es de 15")> _
    <Display(Name:="Número documento")> _
    Public Property Nrodocumento As String

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class



