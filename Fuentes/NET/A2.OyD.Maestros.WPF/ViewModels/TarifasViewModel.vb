Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: TarifasViewModel.vb
'Generado el : 04/15/2011 16:56:04
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

Public Class TarifasViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaTarifa
    Private TarifaPorDefecto As Tarifa
    Private TarifaAnterior As Tarifa
    Private DetalleTarifaPorDefecto As DetalleTarifa
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim dcProxy3 As MaestrosDomainContext
    Dim mensaje As String
    Dim MakeAndCheck As Integer = 0
    Dim Make As Boolean
    Dim esVersion As Boolean = False
    Dim codigo As Integer
    Dim DicCamposTab As New Dictionary(Of String, Integer)
    Dim Busquedavacia As Boolean




    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosDomainContext()
            dcProxy1 = New MaestrosDomainContext()
            dcProxy3 = New MaestrosDomainContext()
        Else
            dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxy1 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxy3 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
        End If
        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.TarifasFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTarifas, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerTarifaPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTarifasPorDefecto_Completed, "Default")
                dcProxy.Load(dcProxy.TraerDetalleTarifaPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDetalleTarifas_Completed, "Default")
                Make = True
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  TarifasViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "TarifasViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerTarifasPorDefecto_Completed(ByVal lo As LoadOperation(Of Tarifa))
        If Not lo.HasError Then

            TarifaPorDefecto = lo.Entities.FirstOrDefault

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Tarifa por defecto", _
                                             Me.ToString(), "TerminoTraerTarifaPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerTarifas(ByVal lo As LoadOperation(Of Tarifa))
        If Not lo.HasError Then

            ListaTarifas = dcProxy.Tarifas

            If MakeAndCheck = 1 And lo.UserState = Nothing Then
                'TarifaSelected = (From sl In ListaTarifas Where sl.ID = codigo Select sl).First
                TarifaSelected = ListaTarifas.Where(Function(ic) ic.ID = codigo).First
            End If
            If Not IsNothing(TarifaSelected.MakeAndCheck) Then
                MakeAndCheck = TarifaSelected.MakeAndCheck

            End If
            If dcProxy.Tarifas.Count > 0 Then
                If lo.UserState = "insert" Then
                    TarifaSelected = ListaTarifas.First
                End If
                If MakeAndCheck = 1 Then
                    If TarifaSelected.Por_Aprobar <> Nothing Then
                        If TarifaSelected.Estado.Equals("Ingreso") Then
                            visible = False
                        Else
                            visible = True
                        End If
                        visibleap = True
                        content = "por aprobar"
                    Else
                        content = "aprobada"
                        If TarifaSelected.Por_Aprobar = Nothing And lo.UserState <> Nothing Then
                            visible = False
                            visibleap = False
                        End If
                    End If
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    Busquedavacia = True
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    If ListaDetalleTarifas.Count > 0 Then
                        ListaDetalleTarifas.Clear()
                    End If
                    TarifaSelected = Nothing
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Tarifas", _
                                             Me.ToString(), "TerminoTraerTarifa", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    'Tablas padres

    Private Sub TerminoTraerDetalleTarifas(ByVal lo As LoadOperation(Of DetalleTarifa))
        If Not lo.HasError Then

            ListaDetalleTarifas = dcProxy.DetalleTarifas
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de DetalleTarifas", _
                                             Me.ToString(), "TerminoTraerDetalleTarifas", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerDetalleTarifas_Completed(ByVal lo As LoadOperation(Of DetalleTarifa))
        If Not lo.HasError Then
            DetalleTarifaPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Tarifa por defecto", _
                                             Me.ToString(), "TerminoTraerTarifaPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminoValidarEdicion(ByVal lo As LoadOperation(Of Tarifa))
        Try

            If Not lo.HasError Then
                If dcProxy3.Tarifas.Count > 0 Then
                    MyBase.RetornarValorEdicionNavegacion()
                    A2Utilidades.Mensajes.mostrarMensaje("La tarifas no se puede editar porque tiene una versión pendiente por aprobar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    Editar()
                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo obtener los clientes por aprobar.", _
                                     Me.ToString(), "TerminoValidarEdicion", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo obtener las tarifas por aprobar.", _
                                     Me.ToString(), "TerminoValidarEdicion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub TerminoValidarborrado(ByVal lo As LoadOperation(Of Tarifa))
        Try

            If Not lo.HasError Then
                If dcProxy3.Tarifas.Count > 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Las tarifas no se pueden borrar porque tiene una versión pendiente por aprobar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    borrar()
                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo obtener las tarifas por aprobar.", _
                                     Me.ToString(), "TerminoValidarEdicion", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo obtener las tarifas por aprobar.", _
                                     Me.ToString(), "TerminoValidarEdicion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


#End Region

#Region "Propiedades"

    Private _ListaTarifas As EntitySet(Of Tarifa)
    Public Property ListaTarifas() As EntitySet(Of Tarifa)
        Get
            Return _ListaTarifas
        End Get
        Set(ByVal value As EntitySet(Of Tarifa))
            _ListaTarifas = value

            MyBase.CambioItem("ListaTarifas")
            MyBase.CambioItem("ListaTarifasPaged")
            If Not IsNothing(value) Then
                'If IsNothing(TarifaAnterior) Then

                '    TarifaSelected = _ListaTarifas.FirstOrDefault
                'Else

                '    TarifaSelected = TarifaAnterior
                'End If
                If IsNothing(TarifaAnterior) Then
                    TarifaSelected = _ListaTarifas.FirstOrDefault
                Else
                    If Not TarifaAnterior.ID.Equals(_ListaTarifas.FirstOrDefault.ID) Then
                        TarifaSelected = _ListaTarifas.FirstOrDefault
                    ElseIf _ListaTarifas.FirstOrDefault.Por_Aprobar <> Nothing Then
                        TarifaSelected = _ListaTarifas.FirstOrDefault
                    Else
                        TarifaSelected = TarifaAnterior
                    End If

                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaTarifasPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaTarifas) Then
                Dim view = New PagedCollectionView(_ListaTarifas)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _TarifaSelected As Tarifa
    Public Property TarifaSelected() As Tarifa
        Get
            Return _TarifaSelected
        End Get
        Set(ByVal value As Tarifa)
            If Not value Is Nothing Then
                _TarifaSelected = value
                If esVersion = False Then
                    dcProxy.DetalleTarifas.Clear()

                    If _TarifaSelected.Por_Aprobar Is Nothing Then
                        dcProxy.Load(dcProxy.Traer_DetalleTarifas_TarifaQuery(0, TarifaSelected.ID, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDetalleTarifas, Nothing)
                    Else
                        dcProxy.Load(dcProxy.Traer_DetalleTarifas_TarifaQuery(1, TarifaSelected.ID, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDetalleTarifas, Nothing)
                        If TarifaSelected.Estado.Equals("Ingreso") Then
                            visible = False
                        End If
                    End If
                End If
                esVersion = False
            End If
            If Busquedavacia Then
                _TarifaSelected = Nothing
                Busquedavacia = False
            End If

            MyBase.CambioItem("TarifaSelected")
        End Set
    End Property

    Private _visible As Boolean
    Public Property visible As Boolean
        Get
            Return _visible
        End Get
        Set(ByVal value As Boolean)
            _visible = value
            MyBase.CambioItem("visible")
        End Set
    End Property
    Private _visibilidad As Visibility = Visibility.Collapsed
    Public Property visibilidad As Visibility
        Get
            Return _visibilidad
        End Get
        Set(ByVal value As Visibility)
            _visibilidad = value
            MyBase.CambioItem("visibilidad")
        End Set
    End Property
    Private _visibleap As Boolean
    Public Property visibleap As Boolean
        Get
            Return _visibleap
        End Get
        Set(ByVal value As Boolean)
            _visibleap = value
            MyBase.CambioItem("visibleap")
        End Set
    End Property

    Private _content As String
    Public Property content As String
        Get
            Return _content
        End Get
        Set(ByVal value As String)
            _content = value
            MyBase.CambioItem("content")
        End Set
    End Property
    Private _Editareg As Boolean = False
    Public Property Editareg As Boolean
        Get
            Return _Editareg
        End Get
        Set(ByVal value As Boolean)
            _Editareg = value
            MyBase.CambioItem("Editareg")
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
#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewTarifa As New Tarifa
            'If ListaTarifas.Count > 0 Then
            '    NewTarifa.ID = ListaTarifas.Count + 1
            'End If
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewTarifa.IDComisionista = TarifaPorDefecto.IDComisionista
            NewTarifa.IDSucComisionista = TarifaPorDefecto.IDSucComisionista
            NewTarifa.ID = TarifaPorDefecto.ID
            NewTarifa.Nombre = TarifaPorDefecto.Nombre
            'NewTarifa.Usuario = Program.Usuario
            NewTarifa.Usuario = Program.Usuario      'eomc -- 21/16/2013 -- se usa program.usuario en veez de HttpContext.Current.User.Identity.Name
            NewTarifa.Actualizacion = TarifaPorDefecto.Actualizacion
            NewTarifa.Valor = TarifaPorDefecto.Valor
            NewTarifa.Simbolo = TarifaPorDefecto.Simbolo
            NewTarifa.IDTarifas = TarifaPorDefecto.IDTarifas
            NewTarifa.Aprobacion = 0
            TarifaAnterior = TarifaSelected
            TarifaSelected = NewTarifa
            MyBase.CambioItem("Tarifas")
            Editando = True
            Editareg = True
            MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub Filtrar()
        Try
            dcProxy.Tarifas.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.TarifasFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTarifas, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.TarifasFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTarifas, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.ID <> 0 Or cb.Nombre <> String.Empty Or cb.Filtro = 1 Or cb.Filtro = 0 Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Tarifas.Clear()
                TarifaAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " ID = " &  cb.ID.ToString() & " Nombre = " &  cb.Nombre.ToString() 
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(cb.Nombre)
                dcProxy.Load(dcProxy.TarifasConsultarQuery(cb.Filtro, cb.ID, TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTarifas, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaTarifa
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

            If Not ListaDetalleTarifas.Count = 0 Then
                If Not IsNothing(DetalleTarifaSelected) Then
                    If Not IsNothing(DetalleTarifaSelected.Valor) Then
                        For Each led In ListaDetalleTarifas
                            ' Se valida que la fecha de inicio pago del registro especifico no este repetida en otro de los registros
                            Dim fechaInicioPago = led.FechaValor.Date
                            Dim fechaInicioRepetida = From ld In ListaDetalleTarifas Where ld.FechaValor.Date = fechaInicioPago
                                                      Select ld

                            If fechaInicioRepetida.Count > 1 Then
                                A2Utilidades.Mensajes.mostrarMensaje("Elemento duplicado es " + led.FechaValor + " .", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                        Next
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Por favor verifica que el campo valor no este vacio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If
            Else
            End If





            Dim numeroErrores = (From lr In ListaDetalleTarifas Where lr.HasValidationErrors = True).Count
            If numeroErrores <> 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor revise que todos los campos requeridos en los registros de detalle hayan sido correctamente diligenciados.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            Dim origen = "update"
            ErrorForma = ""
            TarifaAnterior = TarifaSelected
            If Not ListaTarifas.Contains(TarifaSelected) Then
                origen = "insert"
                For Each a In ListaTarifas
                    If TarifaSelected.Nombre.Equals(a.Nombre) Then
                        mensaje = a.Nombre
                    End If
                Next
                If TarifaSelected.Nombre.Equals(mensaje) Then
                    A2Utilidades.Mensajes.mostrarMensaje("el nombre de la tarifa ya existe.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    CancelarEditarRegistro()
                Else
                    If ListaDetalleTarifas.Count = 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar minimo un Detalle.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    Else
                        ListaTarifas.Add(TarifaSelected)
                    End If

                End If

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
                'TODO: Pendiente garantizar que Userstate no venga vacío
                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And ((So.UserState = "insert")) Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,")
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    If So.UserState = "insert" Then
                        ListaTarifas.Remove(TarifaSelected)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                                   Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                End If

                If So.UserState = "BorrarRegistro" Or So.UserState = "update" Then
                    dcProxy.RejectChanges()
                End If
                So.MarkErrorAsHandled()
                Exit Try
            End If
            'If Not IsNothing(MakeAndCheck) Then
            If MakeAndCheck = 1 Then
                If ((So.UserState = "insert") Or (So.UserState = "update") Or (So.UserState = "BorrarRegistro")) Then
                    MyBase.QuitarFiltroDespuesGuardar()
                    TarifaAnterior = Nothing
                    dcProxy.Tarifas.Clear()
                    dcProxy.Load(dcProxy.TarifasFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTarifas, "insert") ' Recarga la lista para que carguen los include solo cuando use MakeAndCheck

                End If
            End If
            If _TarifaSelected.Por_Aprobar Is Nothing Then

                dcProxy.DetalleTarifas.Clear()
                dcProxy.Load(dcProxy.Traer_DetalleTarifas_TarifaQuery(0, TarifaSelected.ID, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDetalleTarifas, Nothing)
            Else
                dcProxy.DetalleTarifas.Clear()
                dcProxy.Load(dcProxy.Traer_DetalleTarifas_TarifaQuery(1, TarifaSelected.ID, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDetalleTarifas, Nothing)
                If TarifaSelected.Estado.Equals("Ingreso") Then
                    visible = False
                End If
            End If
            'End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub EditarRegistro()
        If MakeAndCheck = 1 Then
            If Not IsNothing(_TarifaSelected) Then
                ValidarEdicion()
            Else
                MyBase.RetornarValorEdicionNavegacion()
            End If
        Else
            If Not IsNothing(_TarifaSelected) Then
                TarifaSelected.Usuario = Program.Usuario
                TarifaSelected.Actualizacion = Now
                Editando = True
                Editareg = False
            Else
                MyBase.RetornarValorEdicionNavegacion()
            End If
        End If
    End Sub
    Public Sub Editar()
        If Not IsNothing(_TarifaSelected) Then
            TarifaSelected.Usuario = Program.Usuario
            TarifaSelected.Actualizacion = Now
            Editando = True
            Editareg = False
        End If

    End Sub
    Public Overrides Sub CambiarAForma()
        If MakeAndCheck = 1 Then
            'If CustodiSelected.Por_Aprobar Is Nothing Then
            '    content = APROBADA
            'Else
            '    content = POR_APROBAR
            '    visible = True
            '    Dim o = visNavegando
            'End If
            If Not IsNothing(TarifaSelected.Estado) Then
                If TarifaSelected.Estado.Equals("Ingreso") Then
                    visible = False
                Else
                    visible = True
                End If
            End If
            MyBase.CambiarAForma()
        Else
            MyBase.CambiarAForma()
        End If


    End Sub
    Public Overrides Sub AprobarRegistro()
        Try
            esVersion = True
            Dim numeroErrores = (From lr In ListaDetalleTarifas Where lr.HasValidationErrors = True).Count
            If numeroErrores <> 0 Then

                A2Utilidades.Mensajes.mostrarMensaje("Por favor revise que todos los campos requeridos en los registros de detalle hayan sido correctamente diligenciados.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            Dim origen = "update"
            ErrorForma = ""
            TarifaAnterior = TarifaSelected
            If (TarifaSelected.Estado.Equals("Modificacion") Or TarifaSelected.Estado.Equals("Ingreso") Or TarifaSelected.Estado.Equals("Retiro")) Then
                TarifaSelected.Aprobacion = 2
                DetalleTarifaSelected = ListaDetalleTarifas.FirstOrDefault
                If Not IsNothing(DetalleTarifaSelected) Then
                    For Each li In ListaDetalleTarifas
                        DetalleTarifaSelected = li
                        DetalleTarifaSelected.Aprobacion = 2
                    Next

                End If
                'ElseIf (TarifaSelected.Estado.Equals("RetiroDetalle")) Then
                '    origen = "BorrarRegistro"

                '    DetalleTarifaSelected = ListaDetalleTarifas.FirstOrDefault
                '    If Not IsNothing(DetalleTarifaSelected) Then
                '        For Each li In ListaDetalleTarifas
                '            DetalleTarifaSelected = li
                '            DetalleTarifaSelected.Aprobacion = 2
                '            '  ListaDetalleTarifas.Remove(_DetalleTarifaSelected)
                '        Next

                '    End If

            ElseIf (TarifaSelected.Estado.Equals("Retiro")) Then
                origen = "BorrarRegistro"
                TarifaSelected.Aprobacion = 2
            End If

            IsBusy = True
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        MyBase.AprobarRegistro()
    End Sub
    Public Overrides Sub RechazarRegistro()
        Try
            esVersion = True
            Dim numeroErrores = (From lr In ListaDetalleTarifas Where lr.HasValidationErrors = True).Count
            If numeroErrores <> 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor revise que todos los campos requeridos en los registros de detalle hayan sido correctamente diligenciados.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            Dim origen = "update"
            ErrorForma = ""
            TarifaAnterior = TarifaSelected
            If (TarifaSelected.Estado.Equals("Modificacion") Or TarifaSelected.Estado.Equals("Ingreso") Or TarifaSelected.Estado.Equals("Retiro")) Then
                TarifaSelected.Aprobacion = 1
                DetalleTarifaSelected = ListaDetalleTarifas.FirstOrDefault
                If Not IsNothing(DetalleTarifaSelected) Then
                    For Each li In ListaDetalleTarifas
                        DetalleTarifaSelected = li
                        DetalleTarifaSelected.Aprobacion = 1
                    Next

                End If
                'ElseIf (TarifaSelected.Estado.Equals("RetiroDetalle")) Then
                '    origen = "BorrarRegistro"

                '    DetalleTarifaSelected = ListaDetalleTarifas.FirstOrDefault
                '    If Not IsNothing(DetalleTarifaSelected) Then
                '        For Each li In ListaDetalleTarifas
                '            DetalleTarifaSelected = li
                '            DetalleTarifaSelected.Aprobacion = 1
                '            '  ListaDetalleTarifas.Remove(_DetalleTarifaSelected)
                '        Next

                '    End If

            ElseIf (TarifaSelected.Estado.Equals("Retiro")) Then
                origen = "BorrarRegistro"
                TarifaSelected.Aprobacion = 1
            End If

            IsBusy = True
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        MyBase.RechazarRegistro()
    End Sub
    Public Overrides Sub VersionRegistro()
        Try
            esVersion = True
            codigo = TarifaSelected.ID
            If TarifaSelected.Por_Aprobar Is Nothing Then
                dcProxy.Tarifas.Clear()
                '  TarifaAnterior = Nothing
                IsBusy = True
                dcProxy.Load(dcProxy.TarifasConsultarQuery(0, cb.ID, cb.Nombre, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTarifas, Nothing)
            Else

                dcProxy.Tarifas.Clear()
                'TarifaAnterior = Nothing
                IsBusy = True
                dcProxy.Load(dcProxy.TarifasConsultarQuery(1, cb.ID, cb.Nombre, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTarifas, Nothing)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de la versión del registro", _
             Me.ToString(), "VersionRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        MyBase.VersionRegistro()
    End Sub
    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_TarifaSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                Editareg = False
                If _TarifaSelected.EntityState = EntityState.Detached Then
                    TarifaSelected = TarifaAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub BorrarRegistro()
        If MakeAndCheck = 1 Then
            If Not IsNothing(_TarifaSelected) Then
                Validarborrado()
            End If
        Else

            Try
                If Not IsNothing(_TarifaSelected) Then

                    dcProxy.Tarifas.Remove(_TarifaSelected)
                    TarifaSelected = _ListaTarifas.LastOrDefault
                    IsBusy = True
                    Program.VerificarCambiosProxyServidor(dcProxy)
                    dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                End If
            Catch ex As Exception
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
                 Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
            End Try
        End If
    End Sub
    Public Sub borrar()
        Try
            If Not IsNothing(_TarifaSelected) Then

                'dcProxy.Tarifas.Remove(_TarifaSelected)
                'TarifaSelected = _ListaTarifas.LastOrDefault
                IsBusy = True
                'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                dcProxy.EliminarTarifas(TarifaSelected.Aprobacion, TarifaSelected.Nombre, TarifaSelected.Usuario, TarifaSelected.ID, String.Empty, TarifaSelected.IDTarifas, Program.Usuario, Program.HashConexion, AddressOf Terminoeliminar, "borrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub Terminoeliminar(ByVal So As InvokeOperation(Of String))
        If So.HasError Then
            A2Utilidades.Mensajes.mostrarMensaje(So.Error.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Else
            If Not (So.Value) = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje(So.Value.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                If So.UserState = "borrar" Then
                    dcProxy.Tarifas.Clear()
                    dcProxy.Load(dcProxy.TarifasFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTarifas, "insert") ' Recarga la lista para que carguen los include
                End If
            End If
        End If
        IsBusy = False
    End Sub
    Public Overrides Sub Buscar()
        If MakeAndCheck = 1 Then
            visibilidad = Visibility.Visible
        End If
        cb.Filtro = 1
        cb.ID = Nothing
        cb.Nombre = String.Empty
        MyBase.Buscar()
    End Sub
    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub
    Public Sub llenarDiccionario()
        DicCamposTab.Add("Nombre", 1)
    End Sub
    Public Sub ValidarEdicion()
        Try

            If Not IsNothing(dcProxy3.Tarifas) Then
                dcProxy3.Tarifas.Clear()
            End If

            dcProxy3.Load(dcProxy3.TarifasConsultarQuery(0, TarifaSelected.ID, TarifaSelected.Nombre, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarEdicion, "ValidarEdicion")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar validar la edición", _
                                 Me.ToString(), "ValidarEdicion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Sub Validarborrado()
        Try

            If Not IsNothing(dcProxy3.Tarifas) Then
                dcProxy3.Tarifas.Clear()
            End If

            dcProxy3.Load(dcProxy3.TarifasConsultarQuery(0, TarifaSelected.ID, TarifaSelected.Nombre, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarborrado, "ValidarEdicion")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar validar la edición", _
                                 Me.ToString(), "ValidarEdicion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Tablas hijas"


    '******************************************************** DetalleTarifas 
    Private _ListaDetalleTarifas As EntitySet(Of DetalleTarifa)
    Public Property ListaDetalleTarifas() As EntitySet(Of DetalleTarifa)
        Get
            Return _ListaDetalleTarifas
        End Get
        Set(ByVal value As EntitySet(Of DetalleTarifa))
            _ListaDetalleTarifas = value
            MyBase.CambioItem("ListaDetalleTarifas")
            MyBase.CambioItem("ListaDetalleTarifasPaged")
        End Set
    End Property

    Public ReadOnly Property DetalleTarifasPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDetalleTarifas) Then
                Dim view = New PagedCollectionView(_ListaDetalleTarifas)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _DetalleTarifaSelected As DetalleTarifa
    Public Property DetalleTarifaSelected() As DetalleTarifa
        Get
            Return _DetalleTarifaSelected
        End Get
        Set(ByVal value As DetalleTarifa)
            _DetalleTarifaSelected = value
            If Not value Is Nothing Then
                MyBase.CambioItem("DetalleTarifaSelected")
            End If

        End Set
    End Property

    Public Overrides Sub NuevoRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmDetalleTarifa"
                Dim NewDetalleTarifa As New DetalleTarifa
                'If ListaDetalleTarifas.Count > 0 Then
                '    NewDetalleTarifa.IDDetalleTarifas = ListaDetalleTarifas.Count + 1
                'End If
                NewDetalleTarifa.IDDetalleTarifas = DetalleTarifaPorDefecto.IDDetalleTarifas
                NewDetalleTarifa.Codigo = TarifaSelected.ID
                NewDetalleTarifa.FechaValor = Now
                'NewDetalleTarifa.Usuario = Program.Usuario
                NewDetalleTarifa.Usuario = Program.Usuario      'eomc -- 21/16/2013 -- se usa program.usuario en veez de HttpContext.Current.User.Identity.Name
                NewDetalleTarifa.Actualizacion = DetalleTarifaPorDefecto.Actualizacion
                ListaDetalleTarifas.Add(NewDetalleTarifa)
                DetalleTarifaSelected = NewDetalleTarifa
                Editareg = True
                MyBase.CambioItem("DetalleTarifaSelected")
                MyBase.CambioItem("ListaDetalleTarifa")

        End Select
    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmDetalleTarifa"
                If Not IsNothing(ListaDetalleTarifas) Then
                    If Not IsNothing(ListaDetalleTarifas) Then
                        Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(DetalleTarifaSelected, ListaDetalleTarifas.ToList)

                        ListaDetalleTarifas.Remove(_DetalleTarifaSelected)
                        If ListaDetalleTarifas.Count > 0 Then
                            Program.PosicionarItemLista(DetalleTarifaSelected, ListaDetalleTarifas.ToList, intRegistroPosicionar)
                        Else
                            DetalleTarifaSelected = Nothing
                        End If
                        MyBase.CambioItem("DetalleTarifaSelected")
                        MyBase.CambioItem("ListaDetalleTarifas")
                    End If
                End If

        End Select
    End Sub
#End Region
End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaTarifa
    Implements INotifyPropertyChanged
    Private _ID As Integer
    <Display(Name:="Código")> _
    Public Property ID As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ID"))
        End Set
    End Property
    Private _Nombre As String
    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")> _
     <Display(Name:="Nombre")> _
    Public Property Nombre As String
        Get
            Return _Nombre
        End Get
        Set(ByVal value As String)
            _Nombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nombre"))
        End Set
    End Property
    Private _Filtro As Byte
    <Display(Name:="Filtro")> _
    Public Property Filtro As Byte
        Get
            Return _Filtro
        End Get
        Set(ByVal value As Byte)
            _Filtro = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Filtro"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class




