Imports Telerik.Windows.Controls

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports System.Text
Imports A2Utilidades.Mensajes
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSMaestros
Imports System.Threading.Tasks
Imports OpenRiaServices.DomainServices.Client

Public Class GrupoOperadorViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaGruposOperadores
    Private GrupoOperadorPorDefecto As OyDPLUSMaestros.GrupoOperadores
    Private GrupoOperadorAnterior As GrupoOperadores
    Private DetalleGrupoOperadorPorDefecto As DetalleGrupoOperadores
    Private DetalleGrupoOperadorAnterior As DetalleGrupoOperadores
    Dim dcProxy As OyDPLUSMaestrosDomainContext
    Dim dcProxy1 As OyDPLUSMaestrosDomainContext
    Dim strRegistros As String = ""

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
                dcProxy.Load(dcProxy.GrupoOperadorFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerGruposOperadores, "")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", Me.ToString(), "GrupoOperadorViewModel", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerGruposOperadoresPorDefecto_Completed(ByVal lo As LoadOperation(Of GrupoOperadores))
        If Not lo.HasError Then
            GrupoOperadorSelected = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la GrupoOperadores por defecto", Me.ToString(), "TerminoTraerGruposOperadoresPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerGruposOperadores(ByVal lo As LoadOperation(Of GrupoOperadores))
        If Not lo.HasError Then
            ListaGruposOperadores = dcProxy.GrupoOperadores.ToList
            If dcProxy.GrupoOperadores.Count > 0 Then
                If lo.UserState = "insert" Then
                    GrupoOperadorSelected = ListaGruposOperadores.Last
                ElseIf lo.UserState = "update" Then
                    GrupoOperadorSelected = GrupoOperadorAnterior
                End If
                IsBusy = True
                dcProxy.Load(dcProxy.DetalleGruposOperadoresrConsultarQuery(_GrupoOperadorSelected.IDGrupoOperador, Program.Usuario, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDetalleGruposOperadores, "")
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de GrupoOperadores", Me.ToString(), "TerminoTraerGruposOperadores", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub


#Region "Resultados Asincrónicos Tabla Hija"

    Private Sub TerminoTraerDetalleGruposOperadoressPorDefecto_Completed(ByVal lo As LoadOperation(Of DetalleGrupoOperadores))
        If Not lo.HasError Then
            DetalleGrupoOperadorPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la DetalleGrupoOperadores por defecto", Me.ToString(), "TerminoTraerDetalleGruposOperadoresPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerDetalleGruposOperadores(ByVal lo As LoadOperation(Of DetalleGrupoOperadores))
        If Not lo.HasError Then
            ListaDetalleGruposOperadores = dcProxy.DetalleGrupoOperadores.ToList
            If dcProxy.DetalleGrupoOperadores.Count > 0 Then
                If lo.UserState = "insert" Then
                    DetalleGrupoOperadorSelected = ListaDetalleGruposOperadores.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    MessageBox.Show("No se encontró ningún registro")
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de DetalleGruposOperadores", Me.ToString(), "TerminoTraerDetalleGruposOperadores", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    'Tablas padres

#End Region
    'Tablas padres

#End Region

#Region "Propiedades"

    Private _ListaGruposOperadores As List(Of GrupoOperadores)
    Public Property ListaGruposOperadores() As List(Of GrupoOperadores)
        Get
            Return _ListaGruposOperadores
        End Get
        Set(ByVal value As List(Of GrupoOperadores))
            _ListaGruposOperadores = value

            MyBase.CambioItem("ListaGruposOperadores")
            MyBase.CambioItem("ListaGruposOperadoresPaged")
            If Not IsNothing(value) Then
                If IsNothing(GrupoOperadorAnterior) Then
                    GrupoOperadorSelected = _ListaGruposOperadores.FirstOrDefault
                Else
                    GrupoOperadorSelected = GrupoOperadorAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaGruposOperadoresPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaGruposOperadores) Then
                Dim view = New PagedCollectionView(_ListaGruposOperadores)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Dim loDGE As LoadOperation(Of DetalleGrupoOperadores)

    Private _GrupoOperadorSelected As GrupoOperadores
    Public Property GrupoOperadorSelected() As GrupoOperadores
        Get
            Return _GrupoOperadorSelected
        End Get
        Set(ByVal value As GrupoOperadores)
            _GrupoOperadorSelected = value
            If Not value Is Nothing Then

                If Not IsNothing(loDGE) Then
                    If Not loDGE.IsComplete Then
                        loDGE.Cancel()
                    End If
                End If
                MyBase.CambioItem("GrupoOperadorSelected")

                dcProxy.DetalleGrupoOperadores.Clear()
                loDGE = dcProxy.Load(dcProxy.DetalleGruposOperadoresrConsultarQuery(_GrupoOperadorSelected.IDGrupoOperador, Program.Usuario, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDetalleGruposOperadores, Nothing)

            End If


        End Set
    End Property

#Region "Propiedades de las Tablas Hijas"

    Private _ListaDetalleGruposOperadores As List(Of DetalleGrupoOperadores)
    Public Property ListaDetalleGruposOperadores() As List(Of DetalleGrupoOperadores)
        Get
            Return _ListaDetalleGruposOperadores
        End Get
        Set(ByVal value As List(Of DetalleGrupoOperadores))
            _ListaDetalleGruposOperadores = value
            MyBase.CambioItem("ListaDetalleGruposOperadores")
        End Set
    End Property

    Private WithEvents _DetalleGrupoOperadorSelected As DetalleGrupoOperadores
    Public Property DetalleGrupoOperadorSelected() As DetalleGrupoOperadores
        Get
            Return _DetalleGrupoOperadorSelected
        End Get
        Set(ByVal value As DetalleGrupoOperadores)
            _DetalleGrupoOperadorSelected = value
            MyBase.CambioItem("DetalleGrupoOperadorSelected")
        End Set
    End Property


#End Region

#End Region

#Region "Métodos"
    Public Async Function ConsultarGruposOperadores() As Task
        Try
            IsBusy = True
            Dim objRet As LoadOperation(Of OyDPLUSMaestros.GrupoOperadores)
            objRet = Await dcProxy.Load(dcProxy.GrupoOperadorFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerGruposOperadores, "").AsTask
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de GrupoOperadores", Me.ToString(), "ConsultarGruposOperadores", Application.Current.ToString(), Program.Maquina, ex.InnerException)
        Finally
            IsBusy = False
        End Try
    End Function

    Public Overrides Sub NuevoRegistro()
        Try

            Dim NewGrupoOperador As New GrupoOperadores
            NewGrupoOperador.Usuario = Program.Usuario

            GrupoOperadorAnterior = GrupoOperadorSelected
            GrupoOperadorSelected = NewGrupoOperador
            Editando = True
            MyBase.CambioItem("ListaGruposOperadores")
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.GrupoOperadores.Clear()            
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                IsBusy = True
                dcProxy.Load(dcProxy.GrupoOperadorConsultarQuery("", "", TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerGruposOperadores, "Filtrar")
            Else
                IsBusy = True
                dcProxy.Load(dcProxy.GrupoOperadorConsultarQuery("", "", "", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerGruposOperadores, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro",
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        cb = New CamposBusquedaGruposOperadores
        cb.Operador = Nothing
        cb.NombreGrupo = Nothing
        MyBase.CambioItem("cb")
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.NombreGrupo <> String.Empty Or cb.Operador <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.GrupoOperadores.Clear()
                GrupoOperadorAnterior = Nothing
                IsBusy = True
                dcProxy.Load(dcProxy.GrupoOperadorConsultarQuery(cb.NombreGrupo, cb.Operador, "", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerGruposOperadores, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaGruposOperadores
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro",
                Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Async Sub ActualizarRegistro()
        Try
            Dim origen = "update"
            ErrorForma = ""
            strRegistros = ""

            If IsNothing(GrupoOperadorSelected.NombreGrupo) Or String.IsNullOrEmpty(GrupoOperadorSelected.NombreGrupo) Then
                A2Utilidades.Mensajes.mostrarMensaje("El Nombre del grupo NO debe quedar vacio.", "ActualizarRegistro", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If IsNothing(ListaDetalleGruposOperadores) Or (ListaDetalleGruposOperadores.Count = 0) Then
                A2Utilidades.Mensajes.mostrarMensaje("El Grupo no se puede Guardar, sin un Operador.", "ActualizarRegistro", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If


            GrupoOperadorAnterior = GrupoOperadorSelected


            If Not ListaGruposOperadores.Contains(GrupoOperadorSelected) Then
                origen = "insert"
            End If

            ListaGruposOperadores.Add(GrupoOperadorSelected)

            If ListaDetalleGruposOperadores.Count > 0 Then
                For Each li In ListaDetalleGruposOperadores
                    IsBusy = True
                    strRegistros += String.Format("{0}|{1}*", _GrupoOperadorSelected.IDGrupoOperador, li.IDEmpleado)
                Next
            Else
                strRegistros = ""
            End If

            Await Grabar(origen)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub TerminoActualizar(ByVal So As LoadOperation(Of OyDPLUSMaestros.GrupoOperadores))
        Try
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", Me.ToString(), "TerminoActualizar" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                'So.MarkErrorAsHandled()
                Exit Try
            Else
                If So.UserState <> "insert" And So.UserState <> "update" Then
                    dcProxy.GrupoOperadores.Clear()
                    dcProxy.Load(dcProxy.GrupoOperadorConsultarQuery("", "", "", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerGruposOperadores, So.UserState)
                Else

                    Dim list As List(Of DetalleGrupoOperadores) = ListaDetalleGruposOperadores '.Where(Function(i) i.IDGrupoOperador <> Nothing).ToList
                    ListaDetalleGruposOperadores = list
                End If
            End If
            'MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoActualizar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub TerminoBorrar(ByVal So As LoadOperation(Of OyDPLUSMaestros.GrupoOperadores))
        Try
            IsBusy = False
            If So.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", Me.ToString(), "TerminoActualizar" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                'So.MarkErrorAsHandled()
                Exit Try
            Else
                If So.UserState <> "insert" And So.UserState <> "update" Then
                    dcProxy.GrupoOperadores.Clear()
                    dcProxy.Load(dcProxy.GrupoOperadorConsultarQuery("", "", "", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerGruposOperadores, So.UserState)
                Else

                    Dim list As List(Of DetalleGrupoOperadores) = ListaDetalleGruposOperadores '.Where(Function(i) i.IDGrupoOperador <> Nothing).ToList
                    ListaDetalleGruposOperadores = list
                End If
            End If
            'MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoActualizar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_GrupoOperadorSelected) Then
            Editando = True
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            If Not IsNothing(_GrupoOperadorSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                'JAPC20200131:Validacion para controlador asignacion de vacio en grupo operador seleccionado
                If Not IsNothing(GrupoOperadorAnterior) Then
                    GrupoOperadorSelected = GrupoOperadorAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Async Sub BorrarRegistro()
        Try
            If Not IsNothing(_GrupoOperadorSelected) Then
                IsBusy = True
                Await Remover()                
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro",
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _DetalleGrupoOperadorSelected_PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Handles _DetalleGrupoOperadorSelected.PropertyChanged
        Try
            Select Case e.PropertyName.ToLower()
                Case "clientelider"
            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Ocurrio un error al realizar el cambio en la propiedad.", Me.ToString, "_OrdenesLEOSelected_PropertyChanged", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
            IsBusy = False
        End Try
    End Sub

    Public Async Function Remover() As Task
        Try
            Dim logResultado As Boolean = False
            Dim objRet As LoadOperation(Of OyDPLUSMaestros.GrupoOperadores)

            If Not IsNothing(dcProxy.GrupoOperadores) Then
                dcProxy.GrupoOperadores.Clear()
            End If

            objRet = Await dcProxy.Load(dcProxy.GrupoOperadorEliminarQuery(_GrupoOperadorSelected.IDGrupoOperador, Program.Usuario, Program.HashConexion)).AsTask

            ListaGruposOperadores = objRet.Entities.ToList

            If ListaGruposOperadores.Count > 0 Then
                _GrupoOperadorSelected = ListaGruposOperadores.LastOrDefault
            Else
                _GrupoOperadorSelected = Nothing
            End If

            If Not objRet Is Nothing Then
                If objRet.HasError = False Then
                    dcProxy.Load(dcProxy.GrupoOperadorConsultarQuery("", "", "", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerGruposOperadores, "Busqueda")
                End If
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar el registro", Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    Public Async Function Grabar(ByVal origen As String) As Task
        Try
            Dim logResultado As Boolean = False
            Dim objRet As LoadOperation(Of OyDPLUSMaestros.GrupoOperadores)

            If Not IsNothing(dcProxy.GrupoOperadores) Then
                dcProxy.GrupoOperadores.Clear()
            End If

            IsBusy = True
            objRet = Await dcProxy.Load(dcProxy.GrupoOperadorModificarQuery(GrupoOperadorSelected.IDGrupoOperador, GrupoOperadorSelected.NombreGrupo, strRegistros, Program.Usuario, Program.HashConexion)).AsTask


            ListaGruposOperadores = objRet.Entities.ToList
            _GrupoOperadorSelected = ListaGruposOperadores.Last


            If Not objRet Is Nothing Then
                If objRet.HasError = False Then
                    If objRet.Entities.ToList.Count > 0 Then
                        dcProxy.Load(dcProxy.GrupoOperadorConsultarQuery("", "", "", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerGruposOperadores, origen)
                    Else
                        dcProxy.Load(dcProxy.GrupoOperadorConsultarQuery("", "", "", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerGruposOperadores, origen)
                    End If
                End If
            End If
            Editando = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al grabar el registro", Me.ToString(), "Grabar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    ''' <summary>
    ''' JAPC20200131:Metodo para eliminar detalle grupo operador
    ''' </summary>
    Public Async Function RemoverDetalleConfirmado() As Task
        Try
            If IsNothing(ListaDetalleGruposOperadores) Or (ListaDetalleGruposOperadores.Count <= 1) Then
                A2Utilidades.Mensajes.mostrarMensaje("No se puede dejar el grupo sin un Operador.", "ActualizarRegistro", A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                Dim logResultado As Boolean = False
                Dim objRet As LoadOperation(Of OyDPLUSMaestros.DetalleGrupoOperadores)

                If Not IsNothing(dcProxy.DetalleGrupoOperadores) Then
                    dcProxy.DetalleGrupoOperadores.Clear()
                End If

                If Not IsNothing(_DetalleGrupoOperadorSelected.IDGrupoOperadorDetalle) And (_DetalleGrupoOperadorSelected.IDGrupoOperadorDetalle <> 0) Then
                    objRet = Await dcProxy.Load(dcProxy.DetalleGruposOperadoresrEliminarQuery(_DetalleGrupoOperadorSelected.IDGrupoOperadorDetalle, Program.Usuario, Program.Usuario, Program.HashConexion)).AsTask
                End If

                dcProxy.DetalleGrupoOperadores.Clear()
                loDGE = dcProxy.Load(dcProxy.DetalleGruposOperadoresrConsultarQuery(_GrupoOperadorSelected.IDGrupoOperador, Program.Usuario, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDetalleGruposOperadores, Nothing)

                'ListaDetalleGruposOperadores.Remove(DetalleGrupoOperadorSelected)
                '_DetalleGrupoOperadorSelected = ListaDetalleGruposOperadores.LastOrDefault

            End If

            IsBusy = False
            Editando = True ' GAG20190206 --Al borrar el detalle, el encabezado debe seguir editado
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar el registro", Me.ToString(), "RemoverDetalleConfirmado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Function

    ''' <summary>
    ''' JAPC20200131:Metodo para recibir respuesta modal de confirmacion eliminar detalle grupo operador
    ''' </summary>
    Private Async Sub RespuestaConfirmarBorrar(sender As Object, e As Telerik.Windows.Controls.WindowClosedEventArgs)
        If e.DialogResult = True Then
            Await RemoverDetalleConfirmado()
        End If
    End Sub

    ''' <summary>
    ''' JAPC20200131:Metodo para invocar modal de confirmacion eliminar detalle grupo operador
    ''' </summary>
    Public Sub RemoverDetalle()
        Try
            RadWindow.Confirm(New DialogParameters With {.Header = " ",
                                                                 .Content = "¿Está seguro de borrar el registro seleccionado?",
                                                                 .Closed = AddressOf RespuestaConfirmarBorrar,
                                                                 .Owner = Application.Current.MainWindow,
                                                                 .OkButtonContent = "Aceptar",
                                                                 .CancelButtonContent = "Cancelar"})
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar borrado del registro", Me.ToString(), "RemoverDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region

#Region "Métodos Tablas Hijas"

    Public Overrides Sub NuevoRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmDetalleGrupoOperador"

                Dim NewDetallegrupoOperador As New DetalleGrupoOperadores

                NewDetallegrupoOperador.IDGrupoOperador = Nothing
                NewDetallegrupoOperador.Usuario = String.Empty

                Dim objListaDetalleGruposOperadores As New List(Of DetalleGrupoOperadores)

                If Not IsNothing(ListaDetalleGruposOperadores) Then
                    For Each li In ListaDetalleGruposOperadores
                        objListaDetalleGruposOperadores.Add(li)
                    Next
                End If

                objListaDetalleGruposOperadores.Add(NewDetallegrupoOperador)
                DetalleGrupoOperadorSelected = NewDetallegrupoOperador

                ListaDetalleGruposOperadores = objListaDetalleGruposOperadores

                MyBase.CambioItem("DetalleGrupoOperadorSelected")
                MyBase.CambioItem("ListaDetalleGruposOperadores")
                MyBase.CambioItem("Editando")
        End Select
    End Sub

    Public Overrides Async Sub BorrarRegistroDetalle()
        If NombreColeccionDetalle = "cmDetalleGrupoOperador" Then
            If Not IsNothing(ListaDetalleGruposOperadores) Then
                If Not IsNothing(DetalleGrupoOperadorSelected) Then
                    If DetalleGrupoOperadorSelected.IDGrupoOperador > 0 Then
                        RemoverDetalle()
                        MyBase.CambioItem("DetalleGrupoOperadorSelected")
                        MyBase.CambioItem("ListaDetalleGruposOperadores")
                    Else
                        Dim objListaDetalleGruposOperadores As New List(Of DetalleGrupoOperadores)

                        If Not IsNothing(ListaDetalleGruposOperadores) Then
                            For Each li In ListaDetalleGruposOperadores
                                objListaDetalleGruposOperadores.Add(li)
                            Next
                        End If

                        objListaDetalleGruposOperadores.Remove(DetalleGrupoOperadorSelected)

                        ListaDetalleGruposOperadores = objListaDetalleGruposOperadores

                        If ListaDetalleGruposOperadores.Count > 0 Then
                            DetalleGrupoOperadorSelected = ListaDetalleGruposOperadores.LastOrDefault
                        Else
                            DetalleGrupoOperadorSelected = Nothing
                        End If
                    End If
                End If
            End If
        End If
    End Sub

#End Region
End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaGruposOperadores
    Implements INotifyPropertyChanged

    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")>
    <Display(Name:="Nombre Grupo Operador")>
    Private _NombreGrupo As String
    Public Property NombreGrupo As String
        Get
            Return _NombreGrupo
        End Get
        Set(ByVal value As String)
            _NombreGrupo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreGrupo"))
        End Set
    End Property

    <StringLength(50, ErrorMessage:="La longitud máxima es de 17")>
    <Display(Name:="Operador")>
    Private _Operador As String
    Public Property Operador As String
        Get
            Return _Operador
        End Get
        Set(ByVal value As String)
            _Operador = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Operador"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class




