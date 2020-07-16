Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: MensajesViewModel.vb
'Generado el : 11/04/2012 17:00:41
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

Public Class MensajesViewModel
    Inherits A2ControlMenu.A2ViewModel
    Private MensajePorDefecto As Mensaje
    Private MensajeAnterior As Mensaje
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
                dcProxy.Load(dcProxy.MensajesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMensajes, "")
                dcProxy1.Load(dcProxy1.TraerMensajePorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMensajesPorDefecto_Completed, "Default")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "MensajesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerMensajesPorDefecto_Completed(ByVal lo As LoadOperation(Of Mensaje))
        If Not lo.HasError Then
            MensajePorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Mensaje por defecto", _
                                             Me.ToString(), "TerminoTraerMensajePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerMensajes(ByVal lo As LoadOperation(Of Mensaje))
        If Not lo.HasError Then
            ListaMensajes = dcProxy.Mensajes
            If dcProxy.Mensajes.Count > 0 Then
                If lo.UserState = "insert" Then
                    _MensajeSelected = ListaMensajes.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Mensajes", _
                                             Me.ToString(), "TerminoTraerMensaje", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private Sub TerminoValidarRegistro(ByVal lo As LoadOperation(Of OYDPLUSUtilidades.ValidacionEliminarRegistro))
        Try
            If Not lo.HasError Then
                If lo.Entities.ToList.Count > 0 Then
                    If lo.UserState = "ELIMINAR" Then
                        If lo.Entities.ToList.First.PermitirRealizarAccion Then
                            dcProxy.Mensajes.Remove(_MensajeSelected)
                            Program.VerificarCambiosProxyServidor(dcProxy)
                            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, Nothing)
                        Else
                            IsBusy = False
                            mostrarMensaje(lo.Entities.ToList.First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    Else
                        If lo.Entities.ToList.First.PermitirRealizarAccion Then
                            Dim origen = "update"
                            If Not ListaMensajes.Contains(_MensajeSelected) Then
                                origen = "insert"
                                ListaMensajes.Add(_MensajeSelected)
                            End If
                            Program.VerificarCambiosProxyServidor(dcProxy)
                            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
                        Else
                            IsBusy = False
                            mostrarMensaje(lo.Entities.ToList.First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
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

    Private _ListaMensajes As EntitySet(Of Mensaje)
    Public Property ListaMensajes() As EntitySet(Of Mensaje)
        Get
            Return _ListaMensajes
        End Get
        Set(ByVal value As EntitySet(Of Mensaje))
            _ListaMensajes = value

            MyBase.CambioItem("ListaMensajes")
            MyBase.CambioItem("ListaMensajesPaged")
            If Not IsNothing(_ListaMensajes) Then
                If _ListaMensajes.Count > 0 Then
                    _MensajeSelected = _ListaMensajes.FirstOrDefault
                Else
                    _MensajeSelected = Nothing
                End If
            Else
                _MensajeSelected = Nothing
            End If
        End Set
    End Property

    Public ReadOnly Property ListaMensajesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaMensajes) Then
                Dim view = New PagedCollectionView(_ListaMensajes)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _MensajeSelected As Mensaje
    Public Property MensajeSelected() As Mensaje
        Get
            Return _MensajeSelected
        End Get
        Set(ByVal value As Mensaje)
            _MensajeSelected = value
            MyBase.CambioItem("MensajeSelected")
        End Set
    End Property

    Private _cb As CamposBusquedaMensaje
    Public Property cb() As CamposBusquedaMensaje
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaMensaje)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property


#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            'Dim NewMensaje As New Mensaje
            ''TODO: Verificar cuales son los campos que deben inicializarse
            'NewMensaje.IDMensaje = MensajePorDefecto.IDMensaje
            'NewMensaje.CodigoMensaje = MensajePorDefecto.CodigoMensaje
            'NewMensaje.Mensaje = MensajePorDefecto.Mensaje
            'NewMensaje.MensajeConReempl = MensajePorDefecto.MensajeConReempl
            'NewMensaje.Usuario = Program.Usuario

            'ObtenerRegistroAnterior()
            '_MensajeSelected = NewMensaje
            'MyBase.CambioItem("MensajeSelected")
            'Editando = True
            'MyBase.CambioItem("Editando")

            mostrarMensaje("Esta funcionalidad se encuentra deshabilitada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            If Not IsNothing(dcProxy.Mensajes) Then
                dcProxy.Mensajes.Clear()
            End If
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.MensajesFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMensajes, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.MensajesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMensajes, "FiltroVM")
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
            If Not ListaMensajes.Contains(_MensajeSelected) Then
                origen = "insert"
            End If
            IsBusy = True
            If Not IsNothing(objProxy.ValidacionEliminarRegistros) Then
                objProxy.ValidacionEliminarRegistros.Clear()
            End If

            If origen = "insert" Then
                objProxy.Load(objProxy.ValidarDuplicidadRegistroQuery("OYDPLUS.tblMensajes", "'strCodigoMensaje'", String.Format("'{0}'", _MensajeSelected.CodigoMensaje), Program.Usuario, Program.HashConexion), AddressOf TerminoValidarRegistro, "ACTUALIZARREGISTRO")
            Else
                If MensajeAnterior.CodigoMensaje <> _MensajeSelected.CodigoMensaje Then
                    objProxy.Load(objProxy.ValidarDuplicidadRegistroQuery("OYDPLUS.tblMensajes", "'strCodigoMensaje'", String.Format("'{0}'", _MensajeSelected.CodigoMensaje), Program.Usuario, Program.HashConexion), AddressOf TerminoValidarRegistro, "ACTUALIZARREGISTRO")
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
            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_MensajeSelected) Then
            ObtenerRegistroAnterior()
            Editando = True
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_MensajeSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                _MensajeSelected = MensajeAnterior
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            'If Not IsNothing(_MensajeSelected) Then
            '    IsBusy = True
            '    If Not IsNothing(dcProxy.ValidacionEliminarRegistros) Then
            '        dcProxy.ValidacionEliminarRegistros.Clear()
            '    End If
            '    dcProxy.Load(dcProxy.ValidarEliminarRegistroQuery("'OYDPLUS.tblMensajesReglas'", "'intIDMensaje'", String.Format("'{0}'", _MensajeSelected.IDMensaje), Program.Usuario, Program.HashConexion), AddressOf TerminoValidarRegistro, "ELIMINAR")

            'End If

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
            If Not String.IsNullOrEmpty(cb.CodigoMensaje) Then
                ErrorForma = ""
                If Not IsNothing(dcProxy.Mensajes) Then
                    dcProxy.Mensajes.Clear()
                End If
                MensajeAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " IdBolsa = " &  cb.IdBolsa.ToString() & " Nombre = " &  cb.Nombre.ToString() 
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(cb.CodigoMensaje)
                dcProxy.Load(dcProxy.MensajesConsultarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMensajes, "Busqueda")
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
            Dim objCB As New CamposBusquedaMensaje
            objCB.CodigoMensaje = String.Empty

            cb = objCB
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar la consulta", _
             Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ObtenerRegistroAnterior()
        Try
            Dim objMensaje As New Mensaje
            If Not IsNothing(_MensajeSelected) Then
                objMensaje.CodigoMensaje = _MensajeSelected.CodigoMensaje
                objMensaje.IDMensaje = _MensajeSelected.IDMensaje
                objMensaje.Mensaje = _MensajeSelected.Mensaje
                objMensaje.MensajeConReempl = _MensajeSelected.MensajeConReempl
                objMensaje.Usuario = _MensajeSelected.Usuario
            End If
            MensajeAnterior = Nothing
            MensajeAnterior = objMensaje
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.", _
             Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaMensaje
    Implements INotifyPropertyChanged

    Private _CodigoMensaje As String
    <Display(Name:="Código mensaje")> _
    Public Property CodigoMensaje() As String
        Get
            Return _CodigoMensaje
        End Get
        Set(ByVal value As String)
            _CodigoMensaje = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("CodigoMensaje"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class




