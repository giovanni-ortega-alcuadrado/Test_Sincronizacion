Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: MensajesReglasViewModel.vb
'Generado el : 11/04/2012 17:12:49
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

Public Class MensajesReglasViewModel
    Inherits A2ControlMenu.A2ViewModel
    Private MensajesReglaPorDefecto As MensajesRegla
    Private MensajesReglaAnterior As MensajesRegla
    Dim dcProxy As OyDPLUSMaestrosDomainContext
    Dim dcProxy1 As OyDPLUSMaestrosDomainContext
    Dim objProxy As OyDPLUSutilidadesDomainContext

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OyDPLUSMaestrosDomainContext()
                dcProxy1 = New OyDPLUSMaestrosDomainContext()
                objProxy = New OyDPLUSutilidadesDomainContext()
            Else
                dcProxy = New OyDPLUSMaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy1 = New OyDPLUSMaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
                objProxy = New OyDPLUSutilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYDPLUS))
            End If

            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.ReglasFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReglas, "")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "MensajesReglasViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerMensajesReglasPorDefecto_Completed(ByVal lo As LoadOperation(Of MensajesRegla))
        If Not lo.HasError Then
            MensajesReglaPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la MensajesRegla por defecto", _
                                             Me.ToString(), "TerminoTraerMensajesReglaPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerMensajesReglas(ByVal lo As LoadOperation(Of MensajesRegla))
        If Not lo.HasError Then
            ListaMensajesReglas = dcProxy.MensajesReglas
            If dcProxy.MensajesReglas.Count > 0 Then
                If lo.UserState = "insert" Then
                    _MensajesReglaSelected = ListaMensajesReglas.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de MensajesReglas", _
                                             Me.ToString(), "TerminoTraerMensajesRegla", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private Sub TerminoTraerReglas(ByVal lo As LoadOperation(Of Regla))
        If Not lo.HasError Then
            ListaReglas = lo.Entities.ToList

            dcProxy.Load(dcProxy.MensajesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMensajes, "")
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención las reglas.", _
                                             Me.ToString(), "TerminoTraerReglas", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerMensajes(ByVal lo As LoadOperation(Of Mensaje))
        If Not lo.HasError Then
            ListaMensajes = lo.Entities.ToList

            dcProxy.Load(dcProxy.MensajesReglasFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMensajesReglas, "")
            dcProxy1.Load(dcProxy1.TraerMensajesReglaPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMensajesReglasPorDefecto_Completed, "Default")
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención los mensajes.", _
                                             Me.ToString(), "TerminoTraerMensajes", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoValidarRegistro(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.ValidacionEliminarRegistro))
        Try
            If Not lo.HasError Then
                If lo.Entities.ToList.Count > 0 Then
                    If lo.Entities.ToList.First.PermitirRealizarAccion Then
                        Dim origen = "update"
                        If Not ListaMensajesReglas.Contains(_MensajesReglaSelected) Then
                            origen = "insert"
                            ListaMensajesReglas.Add(_MensajesReglaSelected)
                        End If
                        Program.VerificarCambiosProxyServidor(dcProxy)
                        dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
                    Else
                        IsBusy = False
                        mostrarMensaje(lo.Entities.ToList.First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If

                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la validación de eliminación.", _
                                                 Me.ToString(), "TerminoValidarEliminarRegistro", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la validación de eliminación.", _
                                                 Me.ToString(), "TerminoValidarEliminarRegistro", Application.Current.ToString(), Program.Maquina, ex)
            lo.MarkErrorAsHandled()   '????
            IsBusy = False
        End Try
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaMensajesReglas As EntitySet(Of MensajesRegla)
    Public Property ListaMensajesReglas() As EntitySet(Of MensajesRegla)
        Get
            Return _ListaMensajesReglas
        End Get
        Set(ByVal value As EntitySet(Of MensajesRegla))
            _ListaMensajesReglas = value

            MyBase.CambioItem("ListaMensajesReglas")
            MyBase.CambioItem("ListaMensajesReglasPaged")
            If Not IsNothing(_ListaMensajesReglas) Then
                If _ListaMensajesReglas.Count > 0 Then
                    _MensajesReglaSelected = _ListaMensajesReglas.FirstOrDefault
                Else
                    _MensajesReglaSelected = Nothing
                End If
            Else
                _MensajesReglaSelected = Nothing
            End If
        End Set
    End Property

    Public ReadOnly Property ListaMensajesReglasPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaMensajesReglas) Then
                Dim view = New PagedCollectionView(_ListaMensajesReglas)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _MensajesReglaSelected As MensajesRegla
    Public Property MensajesReglaSelected() As MensajesRegla
        Get
            Return _MensajesReglaSelected
        End Get
        Set(ByVal value As MensajesRegla)
            _MensajesReglaSelected = value
            MyBase.CambioItem("MensajesReglaSelected")
        End Set
    End Property

    Private _ListaReglas As List(Of Regla)
    Public Property ListaReglas() As List(Of Regla)
        Get
            Return _ListaReglas
        End Get
        Set(ByVal value As List(Of Regla))
            _ListaReglas = value
            MyBase.CambioItem("ListaReglas")
        End Set
    End Property

    Private _ListaMensajes As List(Of Mensaje)
    Public Property ListaMensajes() As List(Of Mensaje)
        Get
            Return _ListaMensajes
        End Get
        Set(ByVal value As List(Of Mensaje))
            _ListaMensajes = value
            MyBase.CambioItem("ListaMensajes")
        End Set
    End Property

    Private _cb As CamposBusquedaMensajesRegla
    Public Property cb() As CamposBusquedaMensajesRegla
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaMensajesRegla)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property



#End Region

#Region "Métodos"

    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewMensajesRegla As New MensajesRegla
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewMensajesRegla.IDMensajeRegla = MensajesReglaPorDefecto.IDMensajeRegla
            NewMensajesRegla.IDRegla = MensajesReglaPorDefecto.IDRegla
            NewMensajesRegla.IDMensaje = MensajesReglaPorDefecto.IDMensaje
            NewMensajesRegla.IDTipoMensaje = MensajesReglaPorDefecto.IDTipoMensaje
            NewMensajesRegla.Usuario = Program.Usuario

            ObtenerRegistroAnterior()
            MensajesReglaAnterior = _MensajesReglaSelected
            _MensajesReglaSelected = NewMensajesRegla
            MyBase.CambioItem("MensajesReglaSelected")
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
            If Not IsNothing(dcProxy.MensajesReglas) Then
                dcProxy.MensajesReglas.Clear()
            End If
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.MensajesReglasFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMensajesReglas, Nothing)
            Else
                dcProxy.Load(dcProxy.MensajesReglasFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMensajesReglas, Nothing)
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
            If Not ListaMensajesReglas.Contains(_MensajesReglaSelected) Then
                origen = "insert"
            End If
            IsBusy = True
            If Not IsNothing(objProxy.ValidacionEliminarRegistros) Then
                objProxy.ValidacionEliminarRegistros.Clear()
            End If

            If origen = "insert" Then
                objProxy.Load(objProxy.ValidarDuplicidadRegistroQuery("OYDPLUS.tblMensajesReglas", "'intIDRegla'|'intIDMensaje'", String.Format("'{0}'|'{1}'", _MensajesReglaSelected.IDRegla, _MensajesReglaSelected.IDMensaje), Program.Usuario, Program.HashConexion), AddressOf TerminoValidarRegistro, "ACTUALIZARREGISTRO")
            Else
                If MensajesReglaAnterior.IDRegla <> _MensajesReglaSelected.IDRegla Or MensajesReglaAnterior.IDMensaje <> _MensajesReglaSelected.IDMensaje Then
                    objProxy.Load(objProxy.ValidarDuplicidadRegistroQuery("OYDPLUS.tblMensajesReglas", "'intIDRegla'|'intIDMensaje'", String.Format("'{0}'|'{1}'", _MensajesReglaSelected.IDRegla, _MensajesReglaSelected.IDMensaje), Program.Usuario, Program.HashConexion), AddressOf TerminoValidarRegistro, "ACTUALIZARREGISTRO")
                Else
                    Program.VerificarCambiosProxyServidor(dcProxy)
                    dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
                End If
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
            Else
                If So.UserState = "borrar" Then
                    MyBase.QuitarFiltroDespuesGuardar()
                    Filtrar()
                End If
            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_MensajesReglaSelected) Then
            ObtenerRegistroAnterior()
            Editando = True
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_MensajesReglaSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                _MensajesReglaSelected = MensajesReglaAnterior
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_MensajesReglaSelected) Then
                dcProxy.MensajesReglas.Remove(_MensajesReglaSelected)
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "borrar")
            End If
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
            If (Not IsNothing(cb.IDMensaje) And cb.IDMensaje <> 0) Or (Not IsNothing(cb.IDRegla) And cb.IDRegla <> 0) Then
                ErrorForma = ""
                If Not IsNothing(dcProxy.MensajesReglas) Then
                    dcProxy.MensajesReglas.Clear()
                End If
                MensajesReglaAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " IdBolsa = " &  cb.IdBolsa.ToString() & " Nombre = " &  cb.Nombre.ToString() 
                dcProxy.Load(dcProxy.MensajesReglasConsultarQuery(cb.IDRegla, cb.IDMensaje, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMensajesReglas, "Busqueda")
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
            Dim objCB As New CamposBusquedaMensajesRegla
            objCB.IDMensaje = Nothing
            objCB.IDRegla = Nothing

            cb = objCB
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar la consulta", _
             Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ObtenerRegistroAnterior()
        Try
            Dim objMensajesRegla As New MensajesRegla
            If Not IsNothing(_MensajesReglaSelected) Then
                objMensajesRegla.IDMensajeRegla = _MensajesReglaSelected.IDMensajeRegla
                objMensajesRegla.IDMensaje = _MensajesReglaSelected.IDMensaje
                objMensajesRegla.IDRegla = _MensajesReglaSelected.IDRegla
                objMensajesRegla.Usuario = _MensajesReglaSelected.Usuario
            End If

            MensajesReglaAnterior = Nothing
            MensajesReglaAnterior = objMensajesRegla
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.", _
             Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaMensajesRegla
    Implements INotifyPropertyChanged

    Private _IDRegla As System.Nullable(Of Integer)
    <Display(Name:="Regla")> _
    Public Property IDRegla() As System.Nullable(Of Integer)
        Get
            Return _IDRegla
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _IDRegla = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDRegla"))
        End Set
    End Property

    Private _IDMensaje As System.Nullable(Of Integer)
    <Display(Name:="Mensaje")> _
    Public Property IDMensaje() As System.Nullable(Of Integer)
        Get
            Return _IDMensaje
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _IDMensaje = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDMensaje"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class





