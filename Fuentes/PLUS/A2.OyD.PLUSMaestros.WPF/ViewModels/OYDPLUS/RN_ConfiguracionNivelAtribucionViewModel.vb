Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: RN_ConfiguracionNivelAtribucionViewModel.vb
'Generado el : 11/13/2012 12:49:04
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

Public Class RN_ConfiguracionNivelAtribucionViewModel
    Inherits A2ControlMenu.A2ViewModel
    Private RN_ConfiguracionNivelAtribucioPorDefecto As RN_ConfiguracionNivelAtribucio
    Private RN_ConfiguracionNivelAtribucioAnterior As RN_ConfiguracionNivelAtribucio
    Dim IdItemActualizar As Integer

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
                dcProxy.Load(dcProxy.RN_ConfiguracionNivelAtribucionFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerRN_ConfiguracionNivelAtribucion, "")
                dcProxy1.Load(dcProxy1.TraerRN_ConfiguracionNivelAtribucioPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerRN_ConfiguracionNivelAtribucionPorDefecto_Completed, "Default")

                dcProxy.Load(dcProxy.RN_TiposDocumentoConsultarQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTiposDocumento, "")
                dcProxy.Load(dcProxy.RN_NivelesAtribucionConsultarQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerNivelesAtribucion, "")
                dcProxy.Load(dcProxy.ReglasFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReglas, "")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "RN_ConfiguracionNivelAtribucionViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerRN_ConfiguracionNivelAtribucionPorDefecto_Completed(ByVal lo As LoadOperation(Of RN_ConfiguracionNivelAtribucio))
        If Not lo.HasError Then
            RN_ConfiguracionNivelAtribucioPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la RN_ConfiguracionNivelAtribucio por defecto", _
                                             Me.ToString(), "TerminoTraerRN_ConfiguracionNivelAtribucioPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerRN_ConfiguracionNivelAtribucion(ByVal lo As LoadOperation(Of RN_ConfiguracionNivelAtribucio))
        If Not lo.HasError Then
            ListaRN_ConfiguracionNivelAtribucion = dcProxy.RN_ConfiguracionNivelAtribucios
            If dcProxy.RN_ConfiguracionNivelAtribucios.Count > 0 Then
                If lo.UserState = "insert" Then
                    RN_ConfiguracionNivelAtribucioSelected = _ListaRN_ConfiguracionNivelAtribucion.OrderBy(Function(i) i.ID).Last
                ElseIf lo.UserState = "update" Then
                    If _ListaRN_ConfiguracionNivelAtribucion.Where(Function(i) i.ID = IdItemActualizar).Count > 0 Then
                        RN_ConfiguracionNivelAtribucioSelected = _ListaRN_ConfiguracionNivelAtribucion.Where(Function(i) i.ID = IdItemActualizar).FirstOrDefault
                    End If
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de RN_ConfiguracionNivelAtribucion", _
                                             Me.ToString(), "TerminoTraerRN_ConfiguracionNivelAtribucio", Application.Current.ToString(), Program.Maquina, lo.Error)
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
                            dcProxy.RN_ConfiguracionNivelAtribucios.Remove(_RN_ConfiguracionNivelAtribucioSelected)
                            Program.VerificarCambiosProxyServidor(dcProxy)
                            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, Nothing)
                        Else
                            IsBusy = False
                            mostrarMensaje(lo.Entities.ToList.First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                    Else
                        If lo.Entities.ToList.First.PermitirRealizarAccion Then
                            Dim origen = "update"
                            If ListaRN_ConfiguracionNivelAtribucion.Where(Function(i) i.ID = _RN_ConfiguracionNivelAtribucioSelected.ID).Count = 0 Then
                                origen = "insert"
                                ListaRN_ConfiguracionNivelAtribucion.Add(RN_ConfiguracionNivelAtribucioSelected)
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
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la validación de eliminación.",
                                                 Me.ToString(), "TerminoValidarRegistro", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la validación de eliminación.",
                                                 Me.ToString(), "TerminoValidarRegistro", Application.Current.ToString(), Program.Maquina, ex)
            lo.MarkErrorAsHandled()   '????
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoTraerTiposDocumento(ByVal lo As LoadOperation(Of RN_TiposDocumento))
        Try
            If lo.HasError = False Then
                ListaTiposDocumento = dcProxy.RN_TiposDocumentos.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los tipos de documento.", _
                                                 Me.ToString(), "TerminoTraerTiposDocumento", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los tipos de documento.", _
                                                 Me.ToString(), "TerminoTraerTiposDocumento", Application.Current.ToString(), Program.Maquina, ex)
            lo.MarkErrorAsHandled()   '????
        End Try
    End Sub

    Private Sub TerminoTraerReglas(ByVal lo As LoadOperation(Of Regla))
        Try
            If lo.HasError = False Then
                ListaReglas = dcProxy.Reglas.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de las reglas.", _
                                                 Me.ToString(), "TerminoTraerReglas", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de las reglas.", _
                                                 Me.ToString(), "TerminoTraerReglas", Application.Current.ToString(), Program.Maquina, ex)
            lo.MarkErrorAsHandled()   '????
        End Try
    End Sub

    Private Sub TerminoTraerNivelesAtribucion(ByVal lo As LoadOperation(Of RN_NivelesAtribucion))
        Try
            If lo.HasError = False Then
                ListaNivelesAtribucionCompleta = dcProxy.RN_NivelesAtribucions.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los niveles atribucion.", _
                                                 Me.ToString(), "TerminoTraerNivelesAtribucion", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los niveles atribucion.", _
                                                 Me.ToString(), "TerminoTraerNivelesAtribucion", Application.Current.ToString(), Program.Maquina, ex)
            lo.MarkErrorAsHandled()   '????
        End Try
    End Sub

#End Region

#Region "Propiedades"

    Private _ListaRN_ConfiguracionNivelAtribucion As EntitySet(Of RN_ConfiguracionNivelAtribucio)
    Public Property ListaRN_ConfiguracionNivelAtribucion() As EntitySet(Of RN_ConfiguracionNivelAtribucio)
        Get
            Return _ListaRN_ConfiguracionNivelAtribucion
        End Get
        Set(ByVal value As EntitySet(Of RN_ConfiguracionNivelAtribucio))
            _ListaRN_ConfiguracionNivelAtribucion = value

            MyBase.CambioItem("ListaRN_ConfiguracionNivelAtribucion")
            MyBase.CambioItem("ListaRN_ConfiguracionNivelAtribucionPaged")
            If Not IsNothing(_ListaRN_ConfiguracionNivelAtribucion) Then
                If _ListaRN_ConfiguracionNivelAtribucion.Count > 0 Then
                    RN_ConfiguracionNivelAtribucioSelected = _ListaRN_ConfiguracionNivelAtribucion.FirstOrDefault
                Else
                    RN_ConfiguracionNivelAtribucioSelected = Nothing
                End If
            Else
                RN_ConfiguracionNivelAtribucioSelected = Nothing
            End If
        End Set
    End Property

    Public ReadOnly Property ListaRN_ConfiguracionNivelAtribucionPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaRN_ConfiguracionNivelAtribucion) Then
                Dim view = New PagedCollectionView(_ListaRN_ConfiguracionNivelAtribucion)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _RN_ConfiguracionNivelAtribucioSelected As RN_ConfiguracionNivelAtribucio
    Public Property RN_ConfiguracionNivelAtribucioSelected() As RN_ConfiguracionNivelAtribucio
        Get
            Return _RN_ConfiguracionNivelAtribucioSelected
        End Get
        Set(ByVal value As RN_ConfiguracionNivelAtribucio)
            _RN_ConfiguracionNivelAtribucioSelected = value
            MyBase.CambioItem("RN_ConfiguracionNivelAtribucioSelected")
        End Set
    End Property

    Private _cb As CamposBusquedaRN_ConfiguracionNivelAtribucio = New CamposBusquedaRN_ConfiguracionNivelAtribucio
    Public Property cb() As CamposBusquedaRN_ConfiguracionNivelAtribucio
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaRN_ConfiguracionNivelAtribucio)
            _cb = value
            MyBase.CambioItem("cb")
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


    Private _ListaTiposDocumento As List(Of RN_TiposDocumento)
    Public Property ListaTiposDocumento() As List(Of RN_TiposDocumento)
        Get
            Return _ListaTiposDocumento
        End Get
        Set(ByVal value As List(Of RN_TiposDocumento))
            _ListaTiposDocumento = value
            MyBase.CambioItem("ListaTiposDocumento")
        End Set
    End Property

    Private _ListaNivelesAtribucionCompleta As List(Of RN_NivelesAtribucion)
    Public Property ListaNivelesAtribucionCompleta() As List(Of RN_NivelesAtribucion)
        Get
            Return _ListaNivelesAtribucionCompleta
        End Get
        Set(ByVal value As List(Of RN_NivelesAtribucion))
            _ListaNivelesAtribucionCompleta = value
            MyBase.CambioItem("ListaNivelesAtribucionCompleta")
        End Set
    End Property


    Private _ListaNivelesAtribucion As List(Of RN_NivelesAtribucion)
    Public Property ListaNivelesAtribucion() As List(Of RN_NivelesAtribucion)
        Get
            Return _ListaNivelesAtribucion
        End Get
        Set(ByVal value As List(Of RN_NivelesAtribucion))
            _ListaNivelesAtribucion = value
            MyBase.CambioItem("ListaNivelesAtribucion")
        End Set
    End Property

    Private _ListaNodos As New ObservableCollection(Of Nivel)
    Public Property ListaNodos As ObservableCollection(Of Nivel)
        Get
            Return _ListaNodos
        End Get
        Set(ByVal value As ObservableCollection(Of Nivel))
            _ListaNodos = value
            MyBase.CambioItem("ListaNodos")
        End Set
    End Property

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewRN_ConfiguracionNivelAtribucio As New RN_ConfiguracionNivelAtribucio
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewRN_ConfiguracionNivelAtribucio.ID = RN_ConfiguracionNivelAtribucioPorDefecto.ID
            NewRN_ConfiguracionNivelAtribucio.Regla = RN_ConfiguracionNivelAtribucioPorDefecto.Regla
            NewRN_ConfiguracionNivelAtribucio.CodigoRegla = String.Empty
            NewRN_ConfiguracionNivelAtribucio.TipoDocumento = RN_ConfiguracionNivelAtribucioPorDefecto.TipoDocumento
            NewRN_ConfiguracionNivelAtribucio.NombreDocumento = String.Empty
            NewRN_ConfiguracionNivelAtribucio.NivelAprobacion = RN_ConfiguracionNivelAtribucioPorDefecto.NivelAprobacion
            NewRN_ConfiguracionNivelAtribucio.NombreNivel = String.Empty
            NewRN_ConfiguracionNivelAtribucio.Usuario = Program.Usuario

            ObtenerRegistroAnterior()
            RN_ConfiguracionNivelAtribucioSelected = NewRN_ConfiguracionNivelAtribucio
            MyBase.CambioItem("RN_ConfiguracionNivelAtribucioSelected")
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
            If Not IsNothing(dcProxy.RN_ConfiguracionNivelAtribucios) Then
                dcProxy.RN_ConfiguracionNivelAtribucios.Clear()
            End If
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.RN_ConfiguracionNivelAtribucionFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerRN_ConfiguracionNivelAtribucion, Nothing)
            Else
                dcProxy.Load(dcProxy.RN_ConfiguracionNivelAtribucionFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerRN_ConfiguracionNivelAtribucion, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        PrepararNuevaBusqueda()
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If (cb.Regla <> 0 And Not IsNothing(cb.Regla)) Or _
                (cb.TipoDocumento <> 0 And Not IsNothing(cb.TipoDocumento)) Or _
                (cb.NivelAtribucion <> 0 And Not IsNothing(cb.NivelAtribucion)) Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                If Not IsNothing(dcProxy.RN_ConfiguracionNivelAtribucios) Then
                    dcProxy.RN_ConfiguracionNivelAtribucios.Clear()
                End If
                IsBusy = True
                'DescripcionFiltroVM = " Codigo = " &  cb.Codigo.ToString()    'Dic202011 quitar
                dcProxy.Load(dcProxy.RN_ConfiguracionNivelAtribucionConsultarQuery(cb.Regla, cb.TipoDocumento, cb.NivelAtribucion, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerRN_ConfiguracionNivelAtribucion, "Busqueda")
                MyBase.ConfirmarBuscar()
                PrepararNuevaBusqueda()
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
            If ListaRN_ConfiguracionNivelAtribucion.Where(Function(i) i.ID = _RN_ConfiguracionNivelAtribucioSelected.ID).Count = 0 Then
                origen = "insert"
            End If
            IsBusy = True
            If Not IsNothing(objProxy.ValidacionEliminarRegistros) Then
                objProxy.ValidacionEliminarRegistros.Clear()
            End If

            If origen = "insert" Then
                objProxy.Load(objProxy.ValidarDuplicidadRegistroQuery("OYDPLUS.tblRN_ConfiguracionNivelAtribucion", "'intIDRegla'|'intIDTipoDocumento'|'intIDNivelAtribucion'", String.Format("'{0}'|'{1}'|'{2}'", _RN_ConfiguracionNivelAtribucioSelected.Regla, _RN_ConfiguracionNivelAtribucioSelected.TipoDocumento, _RN_ConfiguracionNivelAtribucioSelected.NivelAprobacion), Program.Usuario, Program.HashConexion), AddressOf TerminoValidarRegistro, "ACTUALIZARREGISTRO")
            Else
                If RN_ConfiguracionNivelAtribucioAnterior.Regla <> _RN_ConfiguracionNivelAtribucioSelected.Regla Or _
                   RN_ConfiguracionNivelAtribucioAnterior.TipoDocumento <> _RN_ConfiguracionNivelAtribucioSelected.TipoDocumento Or _
                   RN_ConfiguracionNivelAtribucioAnterior.NivelAprobacion <> _RN_ConfiguracionNivelAtribucioSelected.NivelAprobacion Then
                    objProxy.Load(objProxy.ValidarDuplicidadRegistroQuery("OYDPLUS.tblRN_ConfiguracionNivelAtribucion", "'intIDRegla'|'intIDTipoDocumento'|'intIDNivelAtribucion'", String.Format("'{0}'|'{1}'|'{2}'", _RN_ConfiguracionNivelAtribucioSelected.Regla, _RN_ConfiguracionNivelAtribucioSelected.TipoDocumento, _RN_ConfiguracionNivelAtribucioSelected.NivelAprobacion), Program.Usuario, Program.HashConexion), AddressOf TerminoValidarRegistro, "ACTUALIZARREGISTRO")
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
                    A2Utilidades.Mensajes.mostrarMensaje(So.Error.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                So.MarkErrorAsHandled()
                Exit Try
            End If
            MyBase.TerminoSubmitChanges(So)

            If So.UserState.ToString = "update" Then
                IdItemActualizar = _RN_ConfiguracionNivelAtribucioSelected.ID
            End If
            IsBusy = True

            If Not IsNothing(dcProxy.RN_ConfiguracionNivelAtribucios) Then
                dcProxy.RN_ConfiguracionNivelAtribucios.Clear()
            End If

            MyBase.QuitarFiltroDespuesGuardar()
            dcProxy.Load(dcProxy.RN_ConfiguracionNivelAtribucionFiltrarQuery(FiltroVM, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerRN_ConfiguracionNivelAtribucion, So.UserState)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_RN_ConfiguracionNivelAtribucioSelected) Then
            ObtenerRegistroAnterior()
            Editando = True
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_RN_ConfiguracionNivelAtribucioSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                RN_ConfiguracionNivelAtribucioSelected = RN_ConfiguracionNivelAtribucioAnterior
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_RN_ConfiguracionNivelAtribucioSelected) Then
                dcProxy.RN_ConfiguracionNivelAtribucios.Remove(dcProxy.RN_ConfiguracionNivelAtribucios.Where(Function(n) n.ID = _RN_ConfiguracionNivelAtribucioSelected.ID).First())
                RN_ConfiguracionNivelAtribucioSelected = _ListaRN_ConfiguracionNivelAtribucion.LastOrDefault  'Dic202011  nueva
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")   'Dic202011 Nothing -> "BorrarRegistro"
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaRN_ConfiguracionNivelAtribucio
            objCB.Regla = Nothing
            objCB.TipoDocumento = Nothing
            objCB.NivelAtribucion = Nothing
            objCB.NombreNivel = Nothing

            cb = objCB
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar la consulta", _
             Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ObtenerRegistroAnterior()
        Try
            Dim objRNConfiguracion As New RN_ConfiguracionNivelAtribucio
            If Not IsNothing(_RN_ConfiguracionNivelAtribucioSelected) Then
                objRNConfiguracion.ID = _RN_ConfiguracionNivelAtribucioSelected.ID
                objRNConfiguracion.NivelAprobacion = _RN_ConfiguracionNivelAtribucioSelected.NivelAprobacion
                objRNConfiguracion.NombreNivel = _RN_ConfiguracionNivelAtribucioSelected.NombreNivel
                objRNConfiguracion.Regla = _RN_ConfiguracionNivelAtribucioSelected.Regla
                objRNConfiguracion.CodigoRegla = _RN_ConfiguracionNivelAtribucioSelected.CodigoRegla
                objRNConfiguracion.TipoDocumento = _RN_ConfiguracionNivelAtribucioSelected.TipoDocumento
                objRNConfiguracion.NombreDocumento = _RN_ConfiguracionNivelAtribucioSelected.NombreDocumento
                objRNConfiguracion.Usuario = _RN_ConfiguracionNivelAtribucioSelected.Usuario
            End If
            RN_ConfiguracionNivelAtribucioAnterior = Nothing
            RN_ConfiguracionNivelAtribucioAnterior = objRNConfiguracion
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.", _
             Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub AbrirNivelesAtribucion()
        Try
            If Not IsNothing(_RN_ConfiguracionNivelAtribucioSelected) Then
                If _RN_ConfiguracionNivelAtribucioSelected.TipoDocumento <> 0 And Not IsNothing(_RN_ConfiguracionNivelAtribucioSelected.TipoDocumento) Then
                    LlenarListaNiveles(_RN_ConfiguracionNivelAtribucioSelected.TipoDocumento)

                    If Not IsNothing(ListaNodos) Then
                        Dim VentanaCB As New CW_TreeView()
                        AddHandler VentanaCB.Closed, AddressOf VentanaHijaCBCerrada
                        VentanaCB.ListaNodos = ListaNodos
                        Program.Modal_OwnerMainWindowsPrincipal(VentanaCB)
                        VentanaCB.ShowDialog()
                    End If
                Else
                    mostrarMensaje("Debe seleccionar un tipo de documento.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener abrir el control para ver los niveles de atribución.",
            Me.ToString(), "AbrirNivelesAtribucion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub VentanaHijaCBCerrada(ByVal sender As System.Object, ByVal e As EventArgs)  'Solucion tipo 1
        Dim VentanaCB = CType(sender, CW_TreeView)
        If VentanaCB.DialogResult = True Then
            _RN_ConfiguracionNivelAtribucioSelected.NivelAprobacion = VentanaCB.Selected
            _RN_ConfiguracionNivelAtribucioSelected.NombreNivel = VentanaCB.Descripcion
        End If
    End Sub

    Public Sub AbrirNivelesAtribucionBusqueda()
        Try
            If Not IsNothing(cb) Then
                If cb.TipoDocumento <> 0 And Not IsNothing(cb.TipoDocumento) Then
                    LlenarListaNiveles(cb.TipoDocumento)

                    If Not IsNothing(ListaNodos) Then
                        Dim VentanaCB As New CW_TreeView()
                        AddHandler VentanaCB.Closed, AddressOf VentanaHijaCBCerradaBusqueda
                        VentanaCB.ListaNodos = ListaNodos
                        Program.Modal_OwnerMainWindowsPrincipal(VentanaCB)
                        VentanaCB.ShowDialog()
                    End If
                Else
                    mostrarMensaje("Debe seleccionar un tipo de documento.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener abrir el control para ver los niveles de atribución.", _
            Me.ToString(), "AbrirNivelesAtribucion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub VentanaHijaCBCerradaBusqueda(ByVal sender As System.Object, ByVal e As EventArgs)  'Solucion tipo 1
        Dim VentanaCB = CType(sender, CW_TreeView)
        If VentanaCB.DialogResult = True Then
            cb.NivelAtribucion = VentanaCB.Selected
            cb.NombreNivel = VentanaCB.Descripcion
        End If
    End Sub

    Public Sub LlenarListaNiveles(ByVal pintIDTipoDocumento As Integer)
        Try
            If ListaNivelesAtribucionCompleta.Where(Function(i) i.IDTipoDocumento = pintIDTipoDocumento).Count > 0 Then
                Dim objListaNiveles As New List(Of RN_NivelesAtribucion)

                For Each li In ListaNivelesAtribucionCompleta.Where(Function(i) i.IDTipoDocumento = pintIDTipoDocumento)
                    objListaNiveles.Add(li)
                Next

                ListaNivelesAtribucion = objListaNiveles.OrderBy(Function(i) i.NombreNivel).ToList

                If Not IsNothing(ListaNodos) Then
                    ListaNodos = New ObservableCollection(Of Nivel)
                End If

                ListaNodos.Clear()

                Dim _objNivel As Nivel
                For Each ObjNivelSelect In ListaNivelesAtribucion.Where(Function(a) a.IDNivelPadre = 0 Or IsNothing(a.IDNivelPadre)).ToList
                    _objNivel = New Nivel(ObjNivelSelect.IDNivel, ObjNivelSelect.NombreNivel)
                    AgregarNodos(_objNivel, True)
                Next
            Else
                ListaNivelesAtribucion = Nothing
                ListaNodos = Nothing
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al llenar la lista de niveles dependiendo del nivel.", _
            Me.ToString(), "LlenarListaNiveles", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub AgregarNodos(ByVal ObjNivel As Nivel, ByRef bitEsPadre As Boolean)
        Try
            Dim intIdPadre As Integer = ObjNivel.intId
            Dim objNivelHijo As Nivel

            For Each ObjNivelSelect In ListaNivelesAtribucion.Where(Function(a) a.IDNivelPadre = intIdPadre).ToList
                objNivelHijo = New Nivel(ObjNivelSelect.IDNivel, ObjNivelSelect.NombreNivel)
                ObjNivel.Items.Add(objNivelHijo)
                AgregarNodos(objNivelHijo, False)
            Next
            If bitEsPadre Then
                ListaNodos.Add(ObjNivel)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al crear los nodos del Tree View", _
                                 Me.ToString(), "AgregarNodos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Eventos"

    Private Sub _RN_ConfiguracionNivelAtribucioSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _RN_ConfiguracionNivelAtribucioSelected.PropertyChanged
        Try
            'If e.PropertyName.Equals("TipoDocumento") Then
            '    If Editando Then
            '        If _RN_ConfiguracionNivelAtribucioSelected.TipoDocumento <> 0 And Not IsNothing(_RN_ConfiguracionNivelAtribucioSelected.TipoDocumento) Then
            '            LlenarListaNiveles(_RN_ConfiguracionNivelAtribucioSelected.TipoDocumento)
            '        End If
            '    End If
            'End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar el cambio de propiedad.", _
             Me.ToString(), "_RN_ConfiguracionNivelAtribucioSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region
End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaRN_ConfiguracionNivelAtribucio
    Implements INotifyPropertyChanged

    Private _Regla As System.Nullable(Of Integer)
    <Display(Name:="Regla")> _
    Public Property Regla() As System.Nullable(Of Integer)
        Get
            Return _Regla
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _Regla = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Regla"))
        End Set
    End Property

    Private _TipoDocumento As System.Nullable(Of Integer)
    <Display(Name:="Tipo Documento")> _
    Public Property TipoDocumento() As System.Nullable(Of Integer)
        Get
            Return _TipoDocumento
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _TipoDocumento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoDocumento"))
        End Set
    End Property

    Private _NivelAtribucion As System.Nullable(Of Integer)
    <Display(Name:="Nivel Atribución")> _
    Public Property NivelAtribucion() As System.Nullable(Of Integer)
        Get
            Return _NivelAtribucion
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _NivelAtribucion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NivelAtribucion"))
        End Set
    End Property

    Private _NombreNivel As String
    Public Property NombreNivel() As String
        Get
            Return _NombreNivel
        End Get
        Set(ByVal value As String)
            _NombreNivel = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreNivel"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class




