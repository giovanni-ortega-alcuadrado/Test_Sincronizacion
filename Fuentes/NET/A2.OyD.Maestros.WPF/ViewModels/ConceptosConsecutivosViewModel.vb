Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ConceptosConsecutivosViewModel.vb
'Generado el : 03/31/2011 13:12:36
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

Public Class ConceptosConsecutivosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Private ConceptosConsecutivoPorDefecto As ConceptosConsecutivo
    Private ConceptosConsecutivoAnterior As ConceptosConsecutivo
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim consulta As Boolean = True
    Dim changes As Boolean
    Dim nom_consecutvio As String
    Dim consecutivo As String
    Dim count As Integer




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

                dcProxy.Load(dcProxy.ConceptosConsecutivosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConceptosConsecutivos, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerConceptosConsecutivoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConceptosConsecutivosPorDefecto_Completed, "Default")

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ConceptosConsecutivosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        MyBase.CambioItem("visible")


    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraertabla(ByVal lo As LoadOperation(Of LlenarlistaConcepto))
        If Not lo.HasError Then
            ListaTabladisponibles.Clear()
            For Each ll In dcProxy.LlenarlistaConceptos

                ListaTabladisponibles.Add(New ItemConsecutivo With {.Categoria = ll.Categoria, .Chequear = ll.Chequear, .CheckedOriginal = ll.Chequear, .Concepto = ll.Concepto})

            Next

            For Each co In ListaConceptosConsecutivos
                If co.Consecutivo.Equals(ConceptosConsecutivoSelected.Consecutivo) Then
                    ListaTabladisponibles.Add(New ItemConsecutivo With {.Categoria = co.Descripcion_concepto, .Chequear = True, .CheckedOriginal = True})
                End If
            Next

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ConceptosConsecutivos", _
                                       Me.ToString(), "TerminoTraerConceptosConsecutivo", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If


    End Sub



    Private Sub TerminoTraerConceptosConsecutivosPorDefecto_Completed(ByVal lo As LoadOperation(Of ConceptosConsecutivo))
        If Not lo.HasError Then

            ConceptosConsecutivoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ConceptosConsecutivo por defecto", _
                                             Me.ToString(), "TerminoTraerConceptosConsecutivoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerConceptosConsecutivos(ByVal lo As LoadOperation(Of ConceptosConsecutivo))
        If Not lo.HasError Then
            ListaConceptosConsecutivos = dcProxy.ConceptosConsecutivos
            'dcProxy.Load(dcProxy.llenarlistconsecutivodisponiblesQuery(ConceptosConsecutivoSelected.Consecutivo,Program.Usuario, Program.HashConexion), AddressOf TerminoTraertabla, " ")
            If dcProxy.ConceptosConsecutivos.Count > 0 Then
                If lo.UserState = "insert" Then
                    ConceptosConsecutivoSelected = ListaConceptosConsecutivos.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    If ListaConceptosConsecutivos.Count = 0 Then
                        ConceptosConsecutivoSelected = New ConceptosConsecutivo
                    Else


                        A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        'MyBase.Buscar()
                        'MyBase.CancelarBuscar()
                    End If
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ConceptosConsecutivos", _
                                             Me.ToString(), "TerminoTraerConceptosConsecutivo", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    'Tablas padres



#End Region

#Region "Propiedades"


    Private _ListaConceptosConsecutivos As EntitySet(Of ConceptosConsecutivo)
    Public Property ListaConceptosConsecutivos() As EntitySet(Of ConceptosConsecutivo)
        Get
            Return _ListaConceptosConsecutivos
        End Get
        Set(ByVal value As EntitySet(Of ConceptosConsecutivo))
            _ListaConceptosConsecutivos = value

            MyBase.CambioItem("ListaConceptosConsecutivos")
            MyBase.CambioItem("ListaConceptosConsecutivosPaged")
            If Not IsNothing(value) Then
                If IsNothing(ConceptosConsecutivoAnterior) Then
                    ConceptosConsecutivoSelected = _ListaConceptosConsecutivos.FirstOrDefault
                Else
                    ConceptosConsecutivoSelected = ConceptosConsecutivoAnterior
                End If
            End If
        End Set
    End Property
    Public ReadOnly Property ListaConceptosConsecutivosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaConceptosConsecutivos) Then
                Dim view = New PagedCollectionView(_ListaConceptosConsecutivos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ConceptosConsecutivoSelected As ConceptosConsecutivo
    Public Property ConceptosConsecutivoSelected() As ConceptosConsecutivo
        Get
            Return _ConceptosConsecutivoSelected
        End Get
        Set(ByVal value As ConceptosConsecutivo)
            _ConceptosConsecutivoSelected = value
            If Not value Is Nothing Then
                If consulta = True Then
                    If ListaConceptosConsecutivos.Count = 0 Then
                        ConceptosConsecutivoSelected.Consecutivo = consecutivo
                    End If
                    dcProxy.LlenarlistaConceptos.Clear()
                    ListaTabladisponibles.Clear()
                    dcProxy.Load(dcProxy.llenarlistconsecutivodisponiblesQuery(ConceptosConsecutivoSelected.Consecutivo,Program.Usuario, Program.HashConexion), AddressOf TerminoTraertabla, Nothing)
                End If
            End If
            MyBase.CambioItem("ConceptosConsecutivoSelected")
            consulta = True

        End Set
    End Property
    Private _ListaTabladisponibles As ObservableCollection(Of ItemConsecutivo) = New ObservableCollection(Of ItemConsecutivo)
    Public Property ListaTabladisponibles() As ObservableCollection(Of ItemConsecutivo)
        Get
            Return _ListaTabladisponibles
        End Get
        Set(ByVal value As ObservableCollection(Of ItemConsecutivo))
            _ListaTabladisponibles = value
            tablaSelected = value.FirstOrDefault

            MyBase.CambioItem("ListaTabladisponibles")
            MyBase.CambioItem("TabladisponiblesPaged")

        End Set

    End Property
    Public ReadOnly Property TabladisponiblesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaTabladisponibles) Then
                Dim view = New PagedCollectionView(_ListaTabladisponibles)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
    Private _tablaSelected As ItemConsecutivo = New ItemConsecutivo
    Public Property tablaSelected() As ItemConsecutivo
        Get
            Return _tablaSelected
        End Get
        Set(ByVal value As ItemConsecutivo)
            _tablaSelected = value
            If Not value Is Nothing Then
                MyBase.CambioItem("tablaSelected")


            End If
        End Set
    End Property

    Private _EditaReg As Boolean
    Public Property EditaReg() As Boolean
        Get
            Return _EditaReg
        End Get
        Set(ByVal value As Boolean)

            _EditaReg = value
            MyBase.CambioItem("EditaReg")
        End Set
    End Property

    'Private _Enabled As Boolean = True
    'Public ReadOnly Property Enabled() As Boolean
    '    Get
    '        Return Not Checked
    '    End Get
    'End Property

    'Private _Checked As Boolean = False
    'Public Property Checked() As Boolean
    '    Get
    '        Return _Checked
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _Checked = value
    '        MyBase.CambioItem("Checked")
    '        MyBase.CambioItem("Enabled")
    '    End Set
    'End Property
    Dim _cb As New CamposBusquedaConceptosConsecutivo
    Public Property cb() As CamposBusquedaConceptosConsecutivo
        Get
            'Checked = False
            CambioItem("Checked")
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaConceptosConsecutivo)
            _cb = value
            CambioItem("cb")
        End Set
    End Property

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no esta habilitada para este maestro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)

        '    Try
        '        Dim NewConceptosConsecutivo As New ConceptosConsecutivo
        '        'TODO: Verificar cuales son los campos que deben inicializarse
        '        NewConceptosConsecutivo.IDComisionista = ConceptosConsecutivoPorDefecto.IDComisionista
        '        NewConceptosConsecutivo.IdSucComisionista = ConceptosConsecutivoPorDefecto.IdSucComisionista
        '        NewConceptosConsecutivo.Consecutivo = ConceptosConsecutivoSelected.Consecutivo
        '        NewConceptosConsecutivo.Concepto = ConceptosConsecutivoSelected.Concepto
        '        NewConceptosConsecutivo.Actualizacion = ConceptosConsecutivoPorDefecto.Actualizacion
        '        NewConceptosConsecutivo.Usuario = Program.Usuario
        '        NewConceptosConsecutivo.IDConceptosconsecutivos = ConceptosConsecutivoPorDefecto.IDConceptosconsecutivos
        '        ConceptosConsecutivoAnterior = ConceptosConsecutivoSelected
        '        ConceptosConsecutivoSelected = NewConceptosConsecutivo
        '        MyBase.CambioItem("ConceptosConsecutivos")
        '        Editando = True
        '        EditaReg = True

        '        MyBase.CambioItem("Editando")

        '    Catch ex As Exception
        '        IsBusy = False
        '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
        '                                                     Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        '    End Try
    End Sub
    Public Sub NuevosRegistro()
        Try

            Dim NewConceptosConsecutivo As New ConceptosConsecutivo
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewConceptosConsecutivo.IDComisionista = ConceptosConsecutivoPorDefecto.IDComisionista
            NewConceptosConsecutivo.IdSucComisionista = ConceptosConsecutivoPorDefecto.IdSucComisionista
            NewConceptosConsecutivo.Consecutivo = ConceptosConsecutivoSelected.Consecutivo
            NewConceptosConsecutivo.Concepto = ConceptosConsecutivoSelected.Concepto
            NewConceptosConsecutivo.Actualizacion = ConceptosConsecutivoPorDefecto.Actualizacion
            NewConceptosConsecutivo.Usuario = Program.Usuario
            NewConceptosConsecutivo.IDConceptosconsecutivos = ConceptosConsecutivoPorDefecto.IDConceptosconsecutivos
            ConceptosConsecutivoAnterior = ConceptosConsecutivoSelected
            ConceptosConsecutivoSelected = NewConceptosConsecutivo


            MyBase.CambioItem("ConceptosConsecutivos")

            Editando = True
            EditaReg = True
            MyBase.CambioItem("Editando")

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.ConceptosConsecutivos.Clear()
            dcProxy.LlenarlistaConceptos.Clear()
            IsBusy = True

            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ConceptosConsecutivosFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConceptosConsecutivos, "FiltroVM")

            Else
                dcProxy.Load(dcProxy.ConceptosConsecutivosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConceptosConsecutivos, "Filtrar")

            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        MyBase.Buscar()
        'Checked = False

    End Sub
  




    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.Consecutivo <> String.Empty Or cb.Concepto <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""

                dcProxy.ConceptosConsecutivos.Clear()
                dcProxy.LlenarlistaConceptos.Clear()
                ConceptosConsecutivoAnterior = Nothing
                IsBusy = True

                dcProxy.Load(dcProxy.ConceptosConsecutivosConsultarQuery(cb.Consecutivo, cb.Concepto,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConceptosConsecutivos, "Busqueda")

                consecutivo = cb.Consecutivo

                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaConceptosConsecutivo
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

            Dim cambiaron As List(Of ItemConsecutivo) = ListaTabladisponibles.Where(Function(ic) ic.Chequear <> ic.CheckedOriginal).ToList
            Dim cambiaronfalse As List(Of ItemConsecutivo) = cambiaron.Where(Function(ai) ai.Chequear = False).ToList
            Dim cambiarontrue As List(Of ItemConsecutivo) = cambiaron.Where(Function(ab) ab.Chequear = True).ToList
            Dim resultadofinal As List(Of ItemConsecutivo) = ListaTabladisponibles.Where(Function(o) o.Chequear = True).ToList
            Dim a As ItemConsecutivo


            While cambiarontrue.Any
                a = cambiarontrue.FirstOrDefault
                If a.Chequear = True Then
                    NuevosRegistro()
                    ConceptosConsecutivoAnterior = ConceptosConsecutivoSelected
                    tablaSelected = a
                    ConceptosConsecutivoSelected.Concepto = tablaSelected.Concepto
                    ConceptosConsecutivoSelected.Descripcion_concepto = tablaSelected.Categoria
                    If Not ListaConceptosConsecutivos.Contains(ConceptosConsecutivoSelected) Then
                        origen = "insert"
                        ListaConceptosConsecutivos.Add(ConceptosConsecutivoSelected)

                    End If
                End If
                If cambiarontrue.Contains(a) Then
                    cambiarontrue.Remove(a)
                End If
            End While

            While cambiaronfalse.Any
                a = cambiaronfalse.FirstOrDefault
                If a.Chequear = False Then
                    If count = 0 Then
                        nom_consecutvio = ConceptosConsecutivoSelected.Consecutivo
                    End If
                    tablaSelected = a
                    For Each e In ListaConceptosConsecutivos
                        If (tablaSelected.Categoria.Equals(e.Descripcion_concepto) And nom_consecutvio.Equals(e.Consecutivo)) Then
                            ConceptosConsecutivoSelected = e
                        End If
                    Next
                    If Not IsNothing(ConceptosConsecutivoSelected) Then
                        ListaConceptosConsecutivos.Remove(ConceptosConsecutivoSelected)
                        ConceptosConsecutivoSelected = _ListaConceptosConsecutivos.LastOrDefault
                    End If
                End If
                If cambiaronfalse.Contains(a) Then
                    cambiaronfalse.Remove(a)
                End If
                changes = True
                count = count + 1
            End While
            count = 0
            IsBusy = True
            If resultadofinal.Count = 0 Then
                MessageBox.Show("Debe de registrar minimo un Concepto")
                dcProxy.RejectChanges()
                Editando = False
                EditaReg = False
                ConceptosConsecutivoSelected = _ListaConceptosConsecutivos.FirstOrDefault

                IsBusy = False
            Else
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
            ConceptosConsecutivoSelected = ConceptosConsecutivoSelected
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)

                If changes = True Then
                    dcProxy.RejectChanges()
                    changes = False

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

        If Not IsNothing(_ConceptosConsecutivoSelected) Then
            Editando = True
            EditaReg = False
            'consulta = False

            NuevosRegistro()
            'ConceptosConsecutivoSelected.Usuario = Program.Usuario
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub


    Public Overrides Sub CancelarEditarRegistro()
        Try

            ErrorForma = ""
            If Not IsNothing(_ConceptosConsecutivoSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                EditaReg = False


                cb = New CamposBusquedaConceptosConsecutivo
                CambioItem("cb")
                If _ConceptosConsecutivoSelected.EntityState = EntityState.Detached Then
                    ConceptosConsecutivoSelected = ConceptosConsecutivoAnterior
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no esta habilitada para este maestro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
        'Try
        '    If Not IsNothing(ConsecutivosConceptoSelected) Then
        '        dcProxy.ConceptosConsecutivos.Remove(_ConsecutivosConceptoSelected)
        '        ' dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
        '        ConsecutivosConceptoSelected = _ListaConsecutivosConceptos.FirstOrDefault

        '    End If
        'Catch ex As Exception
        '    IsBusy = False
        '    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
        '     Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        'End Try
    End Sub
    Public Overrides Sub QuitarFiltro()
        cb = New CamposBusquedaConceptosConsecutivo
        CambioItem("cb")
        MyBase.QuitarFiltro()
    End Sub


#End Region


End Class


'Clase base para para forma de búsquedas
Public Class CamposBusquedaConceptosConsecutivo

    <StringLength(15, ErrorMessage:="La longitud máxima es de 15")> _
     <Display(Name:="Nombre Consecutivo")> _
    Public Property Consecutivo As String

    <Display(Name:="Detalle Concepto")> _
    Public Property Concepto As String
End Class


'Clase base para llenar listbox
Public Class ItemConsecutivo
    Implements INotifyPropertyChanged
    Private _Chequear As Boolean
    '<StringLength(15, ErrorMessage:="La longitud máxima es de 15")> _
    <Display(Name:="Checked", Description:="Chequear")> _
 Public Property Chequear As Boolean
        Get
            Return _Chequear
        End Get
        Set(ByVal value As Boolean)
            _Chequear = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Chequear"))
        End Set
    End Property

    Private _Categoria As String
    <Display(Name:="Descripcion")> _
    Public Property Categoria As String
        Get
            Return _Categoria
        End Get
        Set(ByVal value As String)
            _Categoria = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Categoria"))
        End Set
    End Property

    <Display(Name:="CheckedOriginal", Description:="CheckedOriginal")> _
    Public Property CheckedOriginal As Boolean


    <Display(Name:="Concepto")> _
    Public Property Concepto As Integer

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class




