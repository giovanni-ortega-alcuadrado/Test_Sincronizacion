Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: AliasEspeciesViewModel.vb
'Generado el : 11/04/2012 17:13:35
'Propiedad de Alcuadrado S.A. 2010

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSMaestros
Imports OpenRiaServices.DomainServices.Client

Public Class AliasEspeciesViewModel
    Inherits A2ControlMenu.A2ViewModel
    Private AliasEspeciePorDefecto As AliasEspecie
    Private AliasEspecieAnterior As AliasEspecie
    Dim dcProxy As OyDPLUSMaestrosDomainContext
    Dim dcProxy1 As OyDPLUSMaestrosDomainContext

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OyDPLUSMaestrosDomainContext()
                dcProxy1 = New OyDPLUSMaestrosDomainContext()
            Else
                dcProxy = New OyDPLUSMaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy1 = New OyDPLUSMaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
            End If

            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.AliasEspecieFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAliasEspecies, "")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "AliasEspeciesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerAliasEspecies(ByVal lo As LoadOperation(Of AliasEspecie))
        If Not lo.HasError Then
            ListaAliasEspecies = dcProxy.AliasEspecies
            If dcProxy.AliasEspecies.Count > 0 Then
                If lo.UserState = "insert" Or lo.UserState = "update" Then
                    AliasEspecieSelected = ListaAliasEspecies.FirstOrDefault
                End If
            Else
                If lo.UserState = "Busqueda" Then
                    mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de AliasEspecies", _
                                             Me.ToString(), "TerminoTraerAliasEspecie", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaAliasEspecies As EntitySet(Of AliasEspecie)
    Public Property ListaAliasEspecies() As EntitySet(Of AliasEspecie)
        Get
            Return _ListaAliasEspecies
        End Get
        Set(ByVal value As EntitySet(Of AliasEspecie))
            _ListaAliasEspecies = value

            MyBase.CambioItem("ListaAliasEspecies")
            MyBase.CambioItem("ListaAliasEspeciesPaged")
            If Not IsNothing(_ListaAliasEspecies) Then
                If _ListaAliasEspecies.Count Then
                    AliasEspecieSelected = _ListaAliasEspecies.FirstOrDefault
                Else
                    AliasEspecieSelected = Nothing
                End If
            Else
                AliasEspecieSelected = Nothing
            End If
        End Set
    End Property

    Public ReadOnly Property ListaAliasEspeciesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaAliasEspecies) Then
                Dim view = New PagedCollectionView(_ListaAliasEspecies)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _AliasEspecieSelected As AliasEspecie
    Public Property AliasEspecieSelected() As AliasEspecie
        Get
            Return _AliasEspecieSelected
        End Get
        Set(ByVal value As AliasEspecie)
            _AliasEspecieSelected = value
            MyBase.CambioItem("AliasEspecieSelected")
        End Set
    End Property

    Private _cb As CamposBusquedaAliasEspecie
    Public Property cb() As CamposBusquedaAliasEspecie
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaAliasEspecie)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

#End Region

#Region "Métodos"

    Public Overrides Sub NuevoRegistro()
        Try
            mostrarMensaje("Esta funcionalidad se encuentra deshabilitada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            If Not IsNothing(dcProxy.AliasEspecies) Then
                dcProxy.AliasEspecies.Clear()
            End If
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.AliasEspecieFiltrarQuery(FiltroVM, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAliasEspecies, Nothing)
            Else
                dcProxy.Load(dcProxy.AliasEspecieFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAliasEspecies, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            Dim origen = "update"
            ErrorForma = ""
            AliasEspecieSelected.Usuario = Program.Usuario
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
                    mostrarMensaje(So.Error.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                If Not strMsg.Equals(String.Empty) Then
                    mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                So.MarkErrorAsHandled()
                Exit Try
            End If
            MyBase.TerminoSubmitChanges(So)


            If So.UserState = "update" Or So.UserState = "insert" Then
                MyBase.QuitarFiltroDespuesGuardar()
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_AliasEspecieSelected) Then
            ObtenerRegistroAnterior()
            Editando = True
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_AliasEspecieSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                AliasEspecieSelected = AliasEspecieAnterior
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            mostrarMensaje("Esta funcionalidad se encuentra deshabilitada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        PrepararNuevaBusqueda()
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.IDEspecies <> 0 Or Not String.IsNullOrEmpty(cb.Nemotecnico) Or Not String.IsNullOrEmpty(cb.Nombre) Or Not String.IsNullOrEmpty(cb.AliasEspecie) Then
                ErrorForma = ""
                If Not IsNothing(dcProxy.AliasEspecies) Then
                    dcProxy.AliasEspecies.Clear()
                End If

                AliasEspecieAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " IdBolsa = " &  cb.IdBolsa.ToString() & " Nombre = " &  cb.Nombre.ToString() 
                If Not IsNothing(dcProxy.AliasEspecies) Then
                    dcProxy.AliasEspecies.Clear()
                End If
                Dim TextoFiltroSeguroNemo = System.Web.HttpUtility.UrlEncode(cb.Nemotecnico)
                Dim TextoFiltroSeguroNombre = System.Web.HttpUtility.UrlEncode(cb.Nombre)
                Dim TextoFiltroSeguroAlias = System.Web.HttpUtility.UrlEncode(cb.AliasEspecie)

                dcProxy.Load(dcProxy.AliasEspecieConsultarQuery(cb.IDEspecies, TextoFiltroSeguroNemo, TextoFiltroSeguroNombre, TextoFiltroSeguroAlias, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerAliasEspecies, "Busqueda")
                MyBase.ConfirmarBuscar()
                PrepararNuevaBusqueda()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaAliasEspecie
            objCB.IDEspecies = Nothing
            objCB.Nemotecnico = String.Empty
            objCB.Nombre = String.Empty
            objCB.AliasEspecie = String.Empty

            cb = objCB
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar la consulta", _
             Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ObtenerRegistroAnterior()
        Try
            Dim objAliasEspecie As New AliasEspecie
            If Not IsNothing(_AliasEspecieSelected) Then
                objAliasEspecie.IDEspecies = _AliasEspecieSelected.IDEspecies
                objAliasEspecie.Nombre = _AliasEspecieSelected.Nombre
                objAliasEspecie.Nemotecnico = _AliasEspecieSelected.Nemotecnico
                objAliasEspecie.AliasEspecie = _AliasEspecieSelected.AliasEspecie
                objAliasEspecie.Usuario = _AliasEspecieSelected.Usuario
            End If
            AliasEspecieAnterior = Nothing
            AliasEspecieAnterior = objAliasEspecie
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.", _
             Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaAliasEspecie
    Implements INotifyPropertyChanged

    Private _IDEspecies As Integer
    <Display(Name:="Id especie")> _
    Public Property IDEspecies() As Integer
        Get
            Return _IDEspecies
        End Get
        Set(ByVal value As Integer)
            _IDEspecies = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDEspecies"))
        End Set
    End Property

    Private _Nemotecnico As String
    <Display(Name:="Nemotécnico")> _
    Public Property Nemotecnico() As String
        Get
            Return _Nemotecnico
        End Get
        Set(ByVal value As String)
            _Nemotecnico = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nemotecnico"))
        End Set
    End Property

    Private _Nombre As String
    <Display(Name:="Nombre")> _
    Public Property Nombre() As String
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            _Nombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nombre"))
        End Set
    End Property

    Private _AliasEspecie As String
    <Display(Name:="Alias")> _
    Public Property AliasEspecie() As String
        Get
            Return _AliasEspecie
        End Get
        Set(ByVal value As String)
            _AliasEspecie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("AliasEspecie"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
