Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: Bancos_BancosNacionalesRelacionadosViewModel.vb
'Generado el : 04/11/2012 14:00:13
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

Public Class Bancos_BancosNacionalesRelacionadosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaBancos_BancosNacionalesRelacionado
    Private Bancos_BancosNacionalesRelacionadoPorDefecto As Bancos_BancosNacionalesRelacionado
    Private Bancos_BancosNacionalesRelacionadoAnterior As Bancos_BancosNacionalesRelacionado
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim changes As Boolean
    Dim count As Integer
    Dim IdBanco As Integer
    Dim IdBancobuscar As Integer
    Dim nombrebanco As String

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
                dcProxy.Load(dcProxy.Bancos_BancosNacionalesRelacionadosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancos_BancosNacionalesRelacionados, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerBancos_BancosNacionalesRelacionadoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancos_BancosNacionalesRelacionadosPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  Bancos_BancosNacionalesRelacionadosViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "Bancos_BancosNacionalesRelacionadosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"
    Private Sub TerminoTraerbancosrelacionadosasociados(ByVal lo As LoadOperation(Of ListaBancosRelacionadosAsociados))
        If Not lo.HasError Then
            DisponiblesBancos.Clear()
            For Each ll In dcProxy.ListaBancosRelacionadosAsociados
                DisponiblesBancos.Add(New ItemRelacionesBancos With {.IDBanco = ll.lngIdBancoNacional, .NombreBanco = ll.NombreBanco, .Chequear = ll.Chequear, .CheckedOriginal = ll.Chequear})
            Next
            'For Each co In dcProxy.Bancos_BancosNacionalesRelacionados
            '    If co.IDBanco = Bancos_BancosNacionalesRelacionadoSelected.IDBanco Then
            '        DisponiblesBancos.Add(New ItemRelacionesBancos With {.IDBanco = co.IdBancoNacional, .NombreBanco = co.NombreBanco, .Chequear = True, .CheckedOriginal = True})
            '    End If
            'Next
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Bancosrelacionados",
                                       Me.ToString(), "TerminoTraerbancosrelacionadosasociados", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If

    End Sub
    Private Sub TerminoTraerBancos_BancosNacionalesRelacionadosPorDefecto_Completed(ByVal lo As LoadOperation(Of Bancos_BancosNacionalesRelacionado))
        If Not lo.HasError Then
            Bancos_BancosNacionalesRelacionadoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Bancos_BancosNacionalesRelacionado por defecto",
                                             Me.ToString(), "TerminoTraerBancos_BancosNacionalesRelacionadoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerBancos_BancosNacionalesRelacionados(ByVal lo As LoadOperation(Of Bancos_BancosNacionalesRelacionado))
        If Not lo.HasError Then
            ListaBancos_BancosNacionalesRelacionados = dcProxy.Bancos_BancosNacionalesRelacionados
            If dcProxy.Bancos_BancosNacionalesRelacionados.Count > 0 Then
                If lo.UserState = "insert" Then
                    Bancos_BancosNacionalesRelacionadoSelected = ListaBancos_BancosNacionalesRelacionados.First
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    'If ListaBancos_BancosNacionalesRelacionados.Count = 0 Then
                    '    Bancos_BancosNacionalesRelacionadoSelected = New Bancos_BancosNacionalesRelacionado
                    'Else
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    If DisponiblesBancos.Count > 0 Then
                        DisponiblesBancos.Clear()
                    End If
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                    'End If
                End If
            End If
        Else
            A2ComunesControl.FuncionesCompartidas.obtenerMensajeValidacionErrorPersonalizado("Se presentó un problema en la obtención de la lista de Bancos_BancosNacionalesRelacionados", Me.ToString, "TerminoTraerBancos_BancosNacionalesRelacionado", lo.Error)
            'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Bancos_BancosNacionalesRelacionados", _
            '                                 Me.ToString(), "TerminoTraerBancos_BancosNacionalesRelacionado", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    'Tablas padres



#End Region

#Region "Propiedades"

    Private _ListaBancos_BancosNacionalesRelacionados As EntitySet(Of Bancos_BancosNacionalesRelacionado)
    Public Property ListaBancos_BancosNacionalesRelacionados() As EntitySet(Of Bancos_BancosNacionalesRelacionado)
        Get
            Return _ListaBancos_BancosNacionalesRelacionados
        End Get
        Set(ByVal value As EntitySet(Of Bancos_BancosNacionalesRelacionado))
            _ListaBancos_BancosNacionalesRelacionados = value

            MyBase.CambioItem("ListaBancos_BancosNacionalesRelacionados")
            MyBase.CambioItem("ListaBancos_BancosNacionalesRelacionadosPaged")
            If Not IsNothing(value) Then
                If IsNothing(Bancos_BancosNacionalesRelacionadoAnterior) Then
                    Bancos_BancosNacionalesRelacionadoSelected = _ListaBancos_BancosNacionalesRelacionados.FirstOrDefault
                Else
                    Bancos_BancosNacionalesRelacionadoSelected = Bancos_BancosNacionalesRelacionadoAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaBancos_BancosNacionalesRelacionadosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaBancos_BancosNacionalesRelacionados) Then
                Dim view = New PagedCollectionView(_ListaBancos_BancosNacionalesRelacionados)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _Bancos_BancosNacionalesRelacionadoSelected As Bancos_BancosNacionalesRelacionado
    Public Property Bancos_BancosNacionalesRelacionadoSelected() As Bancos_BancosNacionalesRelacionado
        Get
            Return _Bancos_BancosNacionalesRelacionadoSelected
        End Get
        Set(ByVal value As Bancos_BancosNacionalesRelacionado)
            _Bancos_BancosNacionalesRelacionadoSelected = value
            If Not value Is Nothing Then
                If ListaBancos_BancosNacionalesRelacionados.Count = 0 Then
                    _Bancos_BancosNacionalesRelacionadoSelected.IDBanco = IdBancobuscar
                    _Bancos_BancosNacionalesRelacionadoSelected.NombreBanco = nombrebanco
                End If
                dcProxy.ListaBancosRelacionadosAsociados.Clear()
                DisponiblesBancos.Clear()
                dcProxy.Load(dcProxy.llenarBancosrelacionadosasociadosQuery(Bancos_BancosNacionalesRelacionadoSelected.IDBanco, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerbancosrelacionadosasociados, Nothing)
                'dcProxy.Load(dcProxy.llenarBancosrelacionadosdisponiblesQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerbancosrelacionadosdisponibles, Nothing)
            End If
            MyBase.CambioItem("Bancos_BancosNacionalesRelacionadoSelected")
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
    Private _DisponiblesBancos As ObservableCollection(Of ItemRelacionesBancos) = New ObservableCollection(Of ItemRelacionesBancos)
    Public Property DisponiblesBancos() As ObservableCollection(Of ItemRelacionesBancos)
        Get
            Return _DisponiblesBancos
        End Get
        Set(ByVal value As ObservableCollection(Of ItemRelacionesBancos))
            _DisponiblesBancos = value
            Bancoseleccionado = value.FirstOrDefault

            MyBase.CambioItem("DisponiblesBancos")
            MyBase.CambioItem("DisponiblesBancosPaged")
        End Set

    End Property
    Public ReadOnly Property DisponiblesBancosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_DisponiblesBancos) Then
                Dim view = New PagedCollectionView(_DisponiblesBancos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
    Private _Bancoseleccionado As ItemRelacionesBancos
    Public Property Bancoseleccionado() As ItemRelacionesBancos
        Get
            Return _Bancoseleccionado
        End Get
        Set(ByVal value As ItemRelacionesBancos)
            _Bancoseleccionado = value
            If Not value Is Nothing Then
                MyBase.CambioItem("Bancoseleccionado")
            End If
        End Set
    End Property
    Public Sub limpiar()
        MyBase.CambioItem("DisponiblesBancos")
        MyBase.CambioItem("Bancoseleccionado")
    End Sub
#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        '      Try
        '          Dim NewBancos_BancosNacionalesRelacionado As New Bancos_BancosNacionalesRelacionado
        '	'TODO: Verificar cuales son los campos que deben inicializarse
        'NewBancos_BancosNacionalesRelacionado.IDComisionista = Bancos_BancosNacionalesRelacionadoPorDefecto.IDComisionista
        'NewBancos_BancosNacionalesRelacionado.IDSucComisionista = Bancos_BancosNacionalesRelacionadoPorDefecto.IDSucComisionista
        'NewBancos_BancosNacionalesRelacionado.IDBanco = Bancos_BancosNacionalesRelacionadoPorDefecto.IDBanco
        'NewBancos_BancosNacionalesRelacionado.IdBancoNacional = Bancos_BancosNacionalesRelacionadoPorDefecto.IdBancoNacional
        'NewBancos_BancosNacionalesRelacionado.Actualizacion = Bancos_BancosNacionalesRelacionadoPorDefecto.Actualizacion
        'NewBancos_BancosNacionalesRelacionado.Usuario = Program.Usuario
        'NewBancos_BancosNacionalesRelacionado.IDBancosNacionalesR = Bancos_BancosNacionalesRelacionadoPorDefecto.IDBancosNacionalesR
        '      Bancos_BancosNacionalesRelacionadoAnterior = Bancos_BancosNacionalesRelacionadoSelected
        '      Bancos_BancosNacionalesRelacionadoSelected = NewBancos_BancosNacionalesRelacionado
        '      MyBase.CambioItem("Bancos_BancosNacionalesRelacionados")
        '      Editando = True
        '      MyBase.CambioItem("Editando")
        '  Catch ex As Exception
        'IsBusy = False
        '      A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
        '                                                   Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        '  End Try
    End Sub
    Public Sub NuevosRegistro()
        Try
            Dim NewBancos_BancosNacionalesRelacionado As New Bancos_BancosNacionalesRelacionado
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewBancos_BancosNacionalesRelacionado.IDComisionista = Bancos_BancosNacionalesRelacionadoPorDefecto.IDComisionista
            NewBancos_BancosNacionalesRelacionado.IDSucComisionista = Bancos_BancosNacionalesRelacionadoPorDefecto.IDSucComisionista
            NewBancos_BancosNacionalesRelacionado.IDBanco = Bancos_BancosNacionalesRelacionadoSelected.IDBanco
            NewBancos_BancosNacionalesRelacionado.IdBancoNacional = Bancos_BancosNacionalesRelacionadoPorDefecto.IdBancoNacional
            NewBancos_BancosNacionalesRelacionado.Actualizacion = Bancos_BancosNacionalesRelacionadoPorDefecto.Actualizacion
            NewBancos_BancosNacionalesRelacionado.Usuario = Program.Usuario
            NewBancos_BancosNacionalesRelacionado.NombreBanco = Bancos_BancosNacionalesRelacionadoSelected.NombreBanco
            NewBancos_BancosNacionalesRelacionado.IDBancosNacionalesR = Bancos_BancosNacionalesRelacionadoPorDefecto.IDBancosNacionalesR
            Bancos_BancosNacionalesRelacionadoAnterior = Bancos_BancosNacionalesRelacionadoSelected
            Bancos_BancosNacionalesRelacionadoSelected = NewBancos_BancosNacionalesRelacionado
            MyBase.CambioItem("Bancos_BancosNacionalesRelacionados")
            Editando = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try


    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.Bancos_BancosNacionalesRelacionados.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.Bancos_BancosNacionalesRelacionadosFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancos_BancosNacionalesRelacionados, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.Bancos_BancosNacionalesRelacionadosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancos_BancosNacionalesRelacionados, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro",
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.IDBanco <> 0 Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Bancos_BancosNacionalesRelacionados.Clear()
                Bancos_BancosNacionalesRelacionadoAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " IDBanco = " &  cb.IDBanco.ToString()    'Dic202011 quitar
                dcProxy.Load(dcProxy.Bancos_BancosNacionalesRelacionadosConsultarQuery(cb.IDBanco, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancos_BancosNacionalesRelacionados, "Busqueda")
                IdBancobuscar = cb.IDBanco
                nombrebanco = cb.NombreBanco
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaBancos_BancosNacionalesRelacionado
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro",
                Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            'Dim origen = "update"
            '      ErrorForma = ""
            '      Bancos_BancosNacionalesRelacionadoAnterior = Bancos_BancosNacionalesRelacionadoSelected
            '      If Not ListaBancos_BancosNacionalesRelacionados.Contains(Bancos_BancosNacionalesRelacionadoSelected) Then
            '    origen = "insert"
            '          ListaBancos_BancosNacionalesRelacionados.Add(Bancos_BancosNacionalesRelacionadoSelected)
            '      End If
            '      IsBusy = True
            Dim origen = "update"
            ErrorForma = ""

            Dim cambiaron As List(Of ItemRelacionesBancos) = DisponiblesBancos.Where(Function(ic) ic.Chequear <> ic.CheckedOriginal).ToList
            Dim cambiaronfalse As List(Of ItemRelacionesBancos) = cambiaron.Where(Function(ai) ai.Chequear = False).ToList
            Dim cambiarontrue As List(Of ItemRelacionesBancos) = cambiaron.Where(Function(ab) ab.Chequear = True).ToList
            Dim resultadofinal As List(Of ItemRelacionesBancos) = DisponiblesBancos.Where(Function(o) o.Chequear = True).ToList
            Dim a As ItemRelacionesBancos

            While cambiarontrue.Any
                a = cambiarontrue.FirstOrDefault
                If a.Chequear = True Then
                    NuevosRegistro()
                    Bancos_BancosNacionalesRelacionadoAnterior = Bancos_BancosNacionalesRelacionadoSelected
                    Bancoseleccionado = a
                    Bancos_BancosNacionalesRelacionadoSelected.IdBancoNacional = Bancoseleccionado.IDBanco
                    If Not ListaBancos_BancosNacionalesRelacionados.Contains(Bancos_BancosNacionalesRelacionadoSelected) Then
                        origen = "insert"
                        ListaBancos_BancosNacionalesRelacionados.Add(Bancos_BancosNacionalesRelacionadoSelected)

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
                        IdBanco = Bancos_BancosNacionalesRelacionadoSelected.IDBanco
                    End If
                    Bancoseleccionado = a
                    For Each e In ListaBancos_BancosNacionalesRelacionados
                        If (Bancoseleccionado.IDBanco.Equals(e.IdBancoNacional) And IdBanco.Equals(e.IDBanco)) Then
                            Bancos_BancosNacionalesRelacionadoSelected = e
                        End If
                    Next
                    If Not IsNothing(Bancos_BancosNacionalesRelacionadoSelected) Then
                        ListaBancos_BancosNacionalesRelacionados.Remove(Bancos_BancosNacionalesRelacionadoSelected)
                        Bancos_BancosNacionalesRelacionadoSelected = _ListaBancos_BancosNacionalesRelacionados.LastOrDefault
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
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub TerminoSubmitChanges(ByVal So As OpenRiaServices.DomainServices.Client.SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                If changes = True Then
                    dcProxy.RejectChanges()
                    changes = False

                End If
                So.MarkErrorAsHandled()
                Exit Try
            End If
            If ((So.UserState = "insert") Or (So.UserState = "update") Or (So.UserState = "BorrarRegistro")) Then
                MyBase.QuitarFiltroDespuesGuardar()
                Bancos_BancosNacionalesRelacionadoAnterior = Nothing
                dcProxy.Bancos_BancosNacionalesRelacionados.Clear()
                dcProxy.Load(dcProxy.Bancos_BancosNacionalesRelacionadosFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerBancos_BancosNacionalesRelacionados, "insert") ' Recarga la lista para que carguen los include

            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_Bancos_BancosNacionalesRelacionadoSelected) Then
            Editando = True
            EditaReg = False
            'consulta = True
            NuevosRegistro()
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_Bancos_BancosNacionalesRelacionadoSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                EditaReg = False
                If _Bancos_BancosNacionalesRelacionadoSelected.EntityState = EntityState.Detached Then
                    Bancos_BancosNacionalesRelacionadoSelected = Bancos_BancosNacionalesRelacionadoAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no esta habilitada para este maestro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        '  Try
        '      If Not IsNothing(_Bancos_BancosNacionalesRelacionadoSelected) Then
        '          dcProxy.Bancos_BancosNacionalesRelacionados.Remove(_Bancos_BancosNacionalesRelacionadoSelected)
        '         Bancos_BancosNacionalesRelacionadoSelected = _ListaBancos_BancosNacionalesRelacionados.LastOrDefault  'Dic202011  nueva
        '          IsBusy = True
        '          dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")   'Dic202011 Nothing -> "BorrarRegistro"
        '      End If
        '  Catch ex As Exception
        'IsBusy = False
        '      A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
        '       Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        '  End Try
    End Sub

    Public Overrides Sub Buscar()
        cb.IDBanco = Nothing
        cb.NombreBanco = String.Empty
        MyBase.Buscar()
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaBancos_BancosNacionalesRelacionado
    Implements INotifyPropertyChanged

    Private _IDBanco As Integer
    <Display(Name:="Banco")>
    Public Property IDBanco As Integer
        Get
            Return _IDBanco
        End Get
        Set(ByVal value As Integer)
            _IDBanco = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDBanco"))

        End Set
    End Property

    Private _NombreBanco As String
    <Display(Name:="Descripcion")>
    Public Property NombreBanco As String
        Get
            Return _NombreBanco
        End Get
        Set(ByVal value As String)
            _NombreBanco = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreBanco"))

        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

Public Class ItemRelacionesBancos
    Implements INotifyPropertyChanged
    Private _Chequear As Boolean
    '<StringLength(15, ErrorMessage:="La longitud máxima es de 15")> _
    <Display(Name:="Checked")>
    Public Property Chequear As Boolean
        Get
            Return _Chequear
        End Get
        Set(ByVal value As Boolean)
            _Chequear = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Chequear"))
        End Set
    End Property
    Private _NombreBanco As String
    <Display(Name:="Descripcion")>
    Public Property NombreBanco As String
        Get
            Return _NombreBanco
        End Get
        Set(ByVal value As String)
            _NombreBanco = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreBanco"))

        End Set
    End Property

    Private _IDBanco As Integer
    <Display(Name:="Banco")>
    Public Property IDBanco As Integer
        Get
            Return _IDBanco
        End Get
        Set(ByVal value As Integer)
            _IDBanco = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDBanco"))

        End Set
    End Property

    <Display(Name:="CheckedOriginal")>
    Public Property CheckedOriginal As Boolean
    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class




