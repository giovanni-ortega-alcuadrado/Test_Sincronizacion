Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ClasificacionesViewModel.vb
'Generado el : 02/24/2011 13:27:32
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

Public Class ClasificacionesViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaClasificacion
    Private ClasificacionPorDefecto As Clasificacion
    Private ClasificacionAnterior As Clasificacion
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim idClasificacion As String = String.Empty
    Dim DicCamposTab As New Dictionary(Of String, Integer)

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
                dcProxy.Load(dcProxy.ClasificacionesFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificaciones, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerClasificacionPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificacionesPorDefecto_Completed, "Default")

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ClasificacionesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerClasificacionesPorDefecto_Completed(ByVal lo As LoadOperation(Of Clasificacion))
        If Not lo.HasError Then
            ClasificacionPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Clasificacion por defecto", _
                                             Me.ToString(), "TerminoTraerClasificacionPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerClasificaciones(ByVal lo As LoadOperation(Of Clasificacion))
        If Not lo.HasError Then
            If lo.UserState = "insert" Or lo.UserState = "FiltroInicial" Then
                TodasListaClasificaciones = dcProxy.Clasificacions.ToList
                ListaClasificacionesPerteneceA = Nothing
                ListaClasificacionesPerteneceA = TodasListaClasificaciones.Where(Function(l) (l.EsGrupo = True Or l.EsSector = True)).ToList
                'cb.ListaClasificaciones = dcProxy.Clasificacions
            End If

            ListaClasificaciones = dcProxy.Clasificacions
            'cb.ListaClasificaciones = ListaClasificaciones
            cmbAplicaAVisible = Visibility.Visible
            rdbGrupoVisible = Visibility.Visible
            rdbSubgrupoVisible = Visibility.Visible
            rdbSectorVisible = Visibility.Visible
            rdbSubsectorVisible = Visibility.Visible
            cmbPerteneceAVisible = Visibility.Visible
            txtNemoVisible = Visibility.Visible

            If dcProxy.Clasificacions.Count > 0 Then
                If lo.UserState = "insert" Or lo.UserState = "FiltroInicial" Then
                    ClasificacionSelected = ListaClasificaciones.FirstOrDefault
                End If

                'Se organizan las clasificaciones para el form de edicion
                'Dim clasific
                'For Each clasific In ListaClasificaciones
                '    If clasific.IDPerteneceA <> clasific.Código Then
                '        Dim esGrupo = From cl In TodasListaClasificaciones Where cl.Código = clasific.IDPerteneceA And cl.EsGrupo = True
                '        Dim esSector = From cl In TodasListaClasificaciones Where cl.Código = clasific.IDPerteneceA And cl.EsSector = True
                '        'Dim esGrupo = From cl In ListaClasificaciones Where cl.Código = clasific.IDPerteneceA And cl.EsGrupo = True
                '        'Dim esSector = From cl In ListaClasificaciones Where cl.Código = clasific.IDPerteneceA And cl.EsSector = True
                '        If esGrupo.Count() <> 0 Then
                '            clasific.EsSubgrupo = True
                '        Else
                '            clasific.EsSubgrupo = False
                '        End If
                '        If esSector.Count() <> 0 Then
                '            clasific.EsSubsector = True
                '        Else
                '            clasific.EsSubsector = False
                '        End If
                '    End If
                'Next

                'Se llena la lista ListaClasificacionesPerteneceA
                'ListaClasificacionesPerteneceA = ListaClasificaciones.Where(Function(T) T.Código = ClasificacionSelected.Código).ToList
                'Where lc.Código = ClasificacionSelected.Código
                '                  Select lc
                MyBase.CambioItem("ListaClasificacionesPerteneceA")
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2ComunesControl.FuncionesCompartidas.obtenerMensajeValidacionErrorPersonalizado("Se presentó un problema en la obtención de la lista de Clasificaciones", Me.ToString, "TerminoTraerClasificacion", lo.Error)
            'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Clasificaciones", _
            '                                 Me.ToString(), "TerminoTraerClasificacion", Application.Current.ToString(), Program.Maquina, lo.Error)
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
                If So.UserState.ToString.ToLower = "borrar" Then
                    dcProxy.Clasificacions.Clear()
                    dcProxy.Load(dcProxy.ClasificacionesFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificaciones, "insert") ' Recarga la lista para que carguen los include
                End If
            End If
        End If
        IsBusy = False
    End Sub

#End Region

#Region "Propiedades"

    Private TodasListaClasificaciones As List(Of Clasificacion)

    Private _ListaClasificaciones As EntitySet(Of Clasificacion)
    Public Property ListaClasificaciones() As EntitySet(Of Clasificacion)
        Get
            Return _ListaClasificaciones
        End Get
        Set(ByVal value As EntitySet(Of Clasificacion))
            _ListaClasificaciones = value

            MyBase.CambioItem("ListaClasificaciones")
            MyBase.CambioItem("ListaClasificacionesPaged")
            If Not IsNothing(value) Then
                If IsNothing(ClasificacionAnterior) Then
                    ClasificacionSelected = _ListaClasificaciones.FirstOrDefault
                Else
                    ClasificacionSelected = ClasificacionAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaClasificacionesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaClasificaciones) Then
                Dim view = New PagedCollectionView(_ListaClasificaciones)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _ClasificacionSelected As Clasificacion
    Public Property ClasificacionSelected() As Clasificacion
        Get
            Return _ClasificacionSelected
        End Get
        Set(ByVal value As Clasificacion)
            _ClasificacionSelected = value
            MyBase.CambioItem("ClasificacionSelected")
        End Set
    End Property

    Private _ListaClasificacionesPerteneceA As List(Of Clasificacion)
    Public Property ListaClasificacionesPerteneceA() As List(Of Clasificacion)
        Get
            Return _ListaClasificacionesPerteneceA
        End Get
        Set(ByVal value As List(Of Clasificacion))
            _ListaClasificacionesPerteneceA = value
            MyBase.CambioItem("ListaClasificacionesPerteneceA")
        End Set
    End Property

    Private _rdbSectorVisible As Visibility
    Public Property rdbSectorVisible() As Visibility
        Get
            Return _rdbSectorVisible
        End Get
        Set(ByVal value As Visibility)
            _rdbSectorVisible = value
            MyBase.CambioItem("rdbSectorVisible")
        End Set
    End Property

    Private _rdbSubsectorVisible As Visibility
    Public Property rdbSubsectorVisible() As Visibility
        Get
            Return _rdbSubsectorVisible
        End Get
        Set(ByVal value As Visibility)
            _rdbSubsectorVisible = value
            MyBase.CambioItem("rdbSubsectorVisible")
        End Set
    End Property

    Private _cmbPerteneceAVisible As Visibility
    Public Property cmbPerteneceAVisible() As Visibility
        Get
            Return _cmbPerteneceAVisible
        End Get
        Set(ByVal value As Visibility)
            _cmbPerteneceAVisible = value
            MyBase.CambioItem("cmbPerteneceAVisible")
        End Set
    End Property

    Private _txtNemoVisible As Visibility
    Public Property txtNemoVisible() As Visibility
        Get
            Return _txtNemoVisible
        End Get
        Set(ByVal value As Visibility)
            _txtNemoVisible = value
            MyBase.CambioItem("txtNemoVisible")
        End Set
    End Property

    Private _cmbHabilitado As Boolean
    Public Property cmbHabilitado() As Boolean
        Get
            Return _cmbHabilitado
        End Get
        Set(ByVal value As Boolean)
            _cmbHabilitado = value
            MyBase.CambioItem("cmbHabilitado")
        End Set
    End Property

    Private _cmbAplicaAHabilitado As Boolean
    Public Property cmbAplicaAHabilitado() As Boolean
        Get
            Return _cmbAplicaAHabilitado
        End Get
        Set(ByVal value As Boolean)
            _cmbAplicaAHabilitado = value
            MyBase.CambioItem("cmbAplicaAHabilitado")
        End Set
    End Property

    Private _cmbAplicaAVisible As Visibility
    Public Property cmbAplicaAVisible() As Visibility
        Get
            Return _cmbAplicaAVisible
        End Get
        Set(ByVal value As Visibility)
            _cmbAplicaAVisible = value
            MyBase.CambioItem("cmbAplicaAVisible")
        End Set
    End Property

    Private _rdbGrupoVisible As Visibility
    Public Property rdbGrupoVisible() As Visibility
        Get
            Return _rdbGrupoVisible
        End Get
        Set(ByVal value As Visibility)
            _rdbGrupoVisible = value
            MyBase.CambioItem("rdbGrupoVisible")
        End Set
    End Property

    Private _rdbSubgrupoVisible As Visibility
    Public Property rdbSubgrupoVisible() As Visibility
        Get
            Return _rdbSubgrupoVisible
        End Get
        Set(ByVal value As Visibility)
            _rdbSubgrupoVisible = value
            MyBase.CambioItem("rdbSubgrupoVisible")
        End Set
    End Property

    Private _Insertando As Boolean
    Public Property Insertando() As Boolean
        Get
            Return _Insertando
        End Get
        Set(ByVal value As Boolean)
            _Insertando = value
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

    'Private _radioGrupoChequeado As Boolean
    'Public Property radioGrupoChequeado() As Boolean
    '    Get
    '        Return _radioGrupoChequeado
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _radioGrupoChequeado = value
    '        MyBase.CambioItem("radioGrupoChequeado")
    '    End Set
    'End Property

#End Region

#Region "Métodos"

    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewClasificacion As New Clasificacion
            'TODO: Verificar cuales son los campos que deben inicializarse
            'NewClasificacion.IDComisionista = ClasificacionPorDefecto.IDComisionista
            'NewClasificacion.IDSucComisionista = ClasificacionPorDefecto.IDSucComisionista
            NewClasificacion.Código = ClasificacionPorDefecto.Código
            NewClasificacion.Nombre = ClasificacionPorDefecto.Nombre
            'NewClasificacion.EsGrupo = ClasificacionPorDefecto.EsGrupo
            NewClasificacion.EsGrupo = True
            NewClasificacion.EsSector = ClasificacionPorDefecto.EsSector
            'NewClasificacion.IDPerteneceA = ClasificacionPorDefecto.IDPerteneceA
            'NewClasificacion.IDPerteneceA = 0
            NewClasificacion.AplicaA = ClasificacionPorDefecto.AplicaA
            NewClasificacion.Nemo = ClasificacionPorDefecto.Nemo
            NewClasificacion.Actualizacion = ClasificacionPorDefecto.Actualizacion
            NewClasificacion.Usuario = Program.Usuario
            NewClasificacion.IDClasificacion = ClasificacionPorDefecto.IDClasificacion
            NewClasificacion.IDPerteneceA = 0
            ClasificacionAnterior = ClasificacionSelected
            ClasificacionSelected = NewClasificacion

            'Manipulacion de controles
            rdbSectorVisible = Visibility.Collapsed
            rdbSubsectorVisible = Visibility.Collapsed
            cmbPerteneceAVisible = Visibility.Collapsed
            txtNemoVisible = Visibility.Collapsed
            cmbHabilitado = False

            MyBase.CambioItem("Clasificaciones")
            Editando = True
            Insertando = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.Clasificacions.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ClasificacionesFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificaciones, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.ClasificacionesFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificaciones, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.Código <> 0 Or cb.Nombre <> String.Empty Or cb.AplicaA <> String.Empty Or (cb.EsSector = True) Or _
                (cb.EsGrupo = True) Or cb.IDPerteneceA <> "0" Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Clasificacions.Clear()
                ClasificacionAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " Código = " &  cb.Código.ToString() & " Nombre = " &  cb.Nombre.ToString() 
                dcProxy.Load(dcProxy.ClasificacionesConsultarQuery(cb.Código, cb.Nombre, cb.AplicaA, cb.EsGrupo, cb.EsSector, CInt(cb.IDPerteneceA),Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificaciones, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaClasificacion
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        cb.IDPerteneceA = "0"
        cb.AplicaA = String.Empty
        cb.ListaClasificaciones = TodasListaClasificaciones
        cb.rdbSectorVisible = Visibility.Collapsed
        cb.rdbSubsectorVisible = Visibility.Collapsed
        cb.cmbPerteneceAVisible = Visibility.Collapsed
        cb.txtNemoVisible = Visibility.Collapsed
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try

            If ClasificacionSelected.EsSubsector Or ClasificacionSelected.EsSubgrupo Then
                If ClasificacionSelected.IDPerteneceA = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe indicar el Grupo al que pertenece.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            End If

            Dim origen = "update"
            ErrorForma = ""
            'If Not ClasificacionSelected.strIDPerteneceA = "0" Then
            '    ClasificacionSelected.IDPerteneceA = CInt(ClasificacionSelected.strIDPerteneceA)
            'End If
            ClasificacionAnterior = ClasificacionSelected
            If Not ListaClasificaciones.Contains(ClasificacionSelected) Then
                origen = "insert"
                ListaClasificaciones.Add(ClasificacionSelected)
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
                'Dim sm As String
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)

                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If

                'If So.EntitiesInError.Count > 0 Then
                '    For a As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                '        sm &= So.EntitiesInError(0).ValidationErrors(a).ErrorMessage & vbNewLine
                '    Next
                '    A2Utilidades.Mensajes.mostrarMensaje(sm, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                'End If

                So.MarkErrorAsHandled()
                Exit Try
            End If
            If So.UserState = "insert" Then
                MyBase.QuitarFiltroDespuesGuardar()
                dcProxy.Clasificacions.Clear()
                dcProxy.Load(dcProxy.ClasificacionesFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificaciones, "insert") ' Recarga la lista para que carguen los include
            End If
            cmbAplicaAVisible = Visibility.Visible
            rdbGrupoVisible = Visibility.Visible
            rdbSubgrupoVisible = Visibility.Visible
            rdbSectorVisible = Visibility.Visible
            rdbSubsectorVisible = Visibility.Visible
            cmbPerteneceAVisible = Visibility.Visible
            txtNemoVisible = Visibility.Visible

            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_ClasificacionSelected) Then
            Editando = True
            Insertando = False
            cmbAplicaAVisible = Visibility.Collapsed
            rdbGrupoVisible = Visibility.Collapsed
            rdbSubgrupoVisible = Visibility.Collapsed
            rdbSectorVisible = Visibility.Collapsed
            rdbSubsectorVisible = Visibility.Collapsed
            cmbPerteneceAVisible = Visibility.Collapsed
            txtNemoVisible = Visibility.Collapsed
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ClasificacionSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _ClasificacionSelected.EntityState = EntityState.Detached Then
                    ClasificacionSelected = ClasificacionAnterior
                End If
                cmbAplicaAVisible = Visibility.Visible
                rdbGrupoVisible = Visibility.Visible
                rdbSubgrupoVisible = Visibility.Visible
                rdbSectorVisible = Visibility.Visible
                rdbSubsectorVisible = Visibility.Visible
                cmbPerteneceAVisible = Visibility.Visible
                txtNemoVisible = Visibility.Visible
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_ClasificacionSelected) Then
                IsBusy = True
                'dcProxy.Clasificacions.Remove(_ClasificacionSelected)
                'ClasificacionSelected = _ListaClasificaciones.LastOrDefault
                'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                dcProxy.EliminarClasificacion(ClasificacionSelected.IDClasificacion, String.Empty,Program.Usuario, Program.HashConexion, AddressOf Terminoeliminar, "Borrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _ClasificacionSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ClasificacionSelected.PropertyChanged
        If Insertando Then
            If (ClasificacionSelected.AplicaA <> String.Empty) Then

                If (ClasificacionSelected.AplicaA = "C") Then
                    rdbSectorVisible = Visibility.Visible
                    rdbSubsectorVisible = Visibility.Visible
                Else
                    rdbSectorVisible = Visibility.Collapsed
                    rdbSubsectorVisible = Visibility.Collapsed
                    cmbPerteneceAVisible = Visibility.Collapsed
                    txtNemoVisible = Visibility.Collapsed
                End If

                If (ClasificacionSelected.EsSubgrupo = True Or ClasificacionSelected.EsSubsector = True) Then
                    cmbPerteneceAVisible = Visibility.Visible
                    If Not e.PropertyName.Equals("IDPerteneceA") Then
                        If e.PropertyName.Equals("EsSubgrupo") Or (e.PropertyName.Equals("AplicaA") And ClasificacionSelected.EsSubgrupo) Then
                            ListaClasificacionesPerteneceA = TodasListaClasificaciones.Where(Function(l) l.AplicaA = ClasificacionSelected.AplicaA And (l.EsGrupo = True)).ToList()
                            ClasificacionSelected.IDPerteneceA = 0
                        End If
                        If e.PropertyName.Equals("EsSubsector") Or (e.PropertyName.Equals("AplicaA") And ClasificacionSelected.EsSubsector) Then
                            txtNemoVisible = Visibility.Visible
                            ListaClasificacionesPerteneceA = TodasListaClasificaciones.Where(Function(l) l.AplicaA = ClasificacionSelected.AplicaA And (l.EsSector = True)).ToList()
                            ClasificacionSelected.IDPerteneceA = 0
                        End If

                        'If ClasificacionSelected.EsSubgrupo Then
                        '    'ListaClasificacionesPerteneceA = ListaClasificaciones.Where(Function(l) l.AplicaA = ClasificacionSelected.AplicaA And (l.EsGrupo = True)).ToList()
                        '    ListaClasificacionesPerteneceA = TodasListaClasificaciones.Where(Function(l) l.AplicaA = ClasificacionSelected.AplicaA And (l.EsGrupo = True)).ToList()
                        'Else
                        '    txtNemoVisible = Visibility.Visible
                        '    'ListaClasificacionesPerteneceA = From lc In ListaClasificaciones
                        '    '                                 Where lc.AplicaA = ClasificacionSelected.AplicaA And lc.EsSubsector = True
                        '    '                                 Select lc
                        '    'ListaClasificacionesPerteneceA = ListaClasificaciones.Where(Function(l) l.AplicaA = ClasificacionSelected.AplicaA And (l.EsSector = True)).ToList()
                        '    ListaClasificacionesPerteneceA = TodasListaClasificaciones.Where(Function(l) l.AplicaA = ClasificacionSelected.AplicaA And (l.EsSector = True)).ToList()
                        'End If
                    End If
                Else
                    txtNemoVisible = Visibility.Collapsed
                    cmbPerteneceAVisible = Visibility.Collapsed
                End If
            End If
        End If
    End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub

    Public Sub llenarDiccionario()
        DicCamposTab.Add("Nombre", 1)
        DicCamposTab.Add("AplicaA", 1)
        DicCamposTab.Add("IDPerteneceA", 1)
    End Sub

    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaClasificacion
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la Búsqueda", _
                     Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region


End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaClasificacion

    Implements INotifyPropertyChanged

    <Display(Name:="Código")> _
    Public Property Código As Integer

    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")> _
     <Display(Name:="Nombre")> _
    Public Property Nombre As String

    Private _EsGrupo As Boolean
    <Display(Name:="Grupo")> _
    Public Property EsGrupo As Boolean
        Get
            Return _EsGrupo
        End Get
        Set(ByVal value As Boolean)
            _EsGrupo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("EsGrupo"))
            VisibilidadControles()
        End Set
    End Property

    Private _EsSubgrupo As Boolean
    <Display(Name:="SubGrupo")> _
    Public Property EsSubgrupo As Boolean
        Get
            Return _EsSubgrupo
        End Get
        Set(ByVal value As Boolean)
            _EsSubgrupo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("EsSubgrupo"))
            If EsSubgrupo Then
                CargarCombo()
            End If
            VisibilidadControles()
        End Set
    End Property

    Private _EsSector As Boolean
    <Display(Name:="Sector")> _
    Public Property EsSector As Boolean
        Get
            Return _EsSector
        End Get
        Set(ByVal value As Boolean)
            _EsSector = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("EsSector"))
            VisibilidadControles()
        End Set
    End Property

    Private _EsSubsector As Boolean
    <Display(Name:="SubSector")> _
    Public Property EsSubsector As Boolean
        Get
            Return _EsSubsector
        End Get
        Set(ByVal value As Boolean)
            _EsSubsector = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("EsSubsector"))
            If EsSubsector Then
                txtNemoVisible = Visibility.Visible
                CargarCombo()
            End If
            VisibilidadControles()
        End Set
    End Property


    Private _AplicaA As String
    <Display(Name:="Aplicado A")> _
    Public Property AplicaA As String
        Get
            Return _AplicaA
        End Get
        Set(ByVal value As String)
            _AplicaA = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("AplicaA"))
            If (AplicaA = "C") Then
                rdbSectorVisible = Visibility.Visible
                rdbSubsectorVisible = Visibility.Visible
            Else
                rdbSectorVisible = Visibility.Collapsed
                rdbSubsectorVisible = Visibility.Collapsed
                cmbPerteneceAVisible = Visibility.Collapsed
                txtNemoVisible = Visibility.Collapsed
            End If
            VisibilidadControles()
            CargarCombo()
        End Set
    End Property

    Private _IDPerteneceA As String = "0"
    <Display(Name:="Pertenece A")> _
    Public Property IDPerteneceA As String
        Get
            Return _IDPerteneceA
        End Get
        Set(ByVal value As String)
            _IDPerteneceA = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDPerteneceA"))
        End Set
    End Property

    Private _rdbSectorVisible As Visibility
    Public Property rdbSectorVisible() As Visibility
        Get
            Return _rdbSectorVisible
        End Get
        Set(ByVal value As Visibility)
            _rdbSectorVisible = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("rdbSectorVisible"))
        End Set
    End Property

    Private _rdbSubsectorVisible As Visibility
    Public Property rdbSubsectorVisible() As Visibility
        Get
            Return _rdbSubsectorVisible
        End Get
        Set(ByVal value As Visibility)
            _rdbSubsectorVisible = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("rdbSubsectorVisible"))
        End Set
    End Property

    Private _cmbPerteneceAVisible As Visibility
    Public Property cmbPerteneceAVisible() As Visibility
        Get
            Return _cmbPerteneceAVisible
        End Get
        Set(ByVal value As Visibility)
            _cmbPerteneceAVisible = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("cmbPerteneceAVisible"))
        End Set
    End Property

    Private _txtNemoVisible As Visibility
    Public Property txtNemoVisible() As Visibility
        Get
            Return _txtNemoVisible
        End Get
        Set(ByVal value As Visibility)
            _txtNemoVisible = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("txtNemoVisible"))
        End Set
    End Property

    Private _ListaClasificacionesPerteneceA As IEnumerable(Of Clasificacion)
    Public Property ListaClasificacionesPerteneceA() As IEnumerable(Of Clasificacion)
        Get
            Return _ListaClasificacionesPerteneceA
        End Get
        Set(ByVal value As IEnumerable(Of Clasificacion))
            _ListaClasificacionesPerteneceA = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ListaClasificacionesPerteneceA"))
        End Set
    End Property

    Private _ListaClasificaciones As List(Of Clasificacion)
    Public Property ListaClasificaciones() As List(Of Clasificacion)
        Get
            Return _ListaClasificaciones
        End Get
        Set(ByVal value As List(Of Clasificacion))
            _ListaClasificaciones = value
        End Set
    End Property

    Private Sub CargarCombo()
        If Not AplicaA.Equals(String.Empty) Then

            If EsSubgrupo Then
                ListaClasificacionesPerteneceA = From lc In ListaClasificaciones
                         Where lc.AplicaA = AplicaA And (lc.EsGrupo = True)
                         Select lc
            End If

            If EsSubsector Then
                ListaClasificacionesPerteneceA = From lc In ListaClasificaciones
                         Where lc.AplicaA = AplicaA And lc.EsSector = True
                         Select lc
            End If
        End If
        IDPerteneceA = "0"

    End Sub

    Private Sub VisibilidadControles()
        cmbPerteneceAVisible = Visibility.Collapsed
        If EsSubgrupo Then
            cmbPerteneceAVisible = Visibility.Visible
        End If

        If EsSubsector And AplicaA = "C" Then
            cmbPerteneceAVisible = Visibility.Visible
            txtNemoVisible = Visibility.Visible
        End If
    End Sub

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class




