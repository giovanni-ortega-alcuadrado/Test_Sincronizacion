Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: MonedasViewModel.vb
'Generado el : 04/19/2011 11:12:06
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

Public Class MonedasViewModel
	Inherits A2ControlMenu.A2ViewModel
	Public Property cb As New CamposBusquedaMoneda
	Private MonedaPorDefecto As Moneda
    Private MonedaAnterior As Moneda
    Private MonedavalorPorDefecto As MonedaValo
    Private strCodigoInternacional As String = String.Empty
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim x As Integer
    Dim fecha As DateTime
    Dim i As Integer
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
                dcProxy.Load(dcProxy.MonedasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMonedas, "")
                dcProxy1.Load(dcProxy1.TraerMonedaPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMonedasPorDefecto_Completed, "Default")
                dcProxy1.Load(dcProxy1.TraerMonedaValoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMonedaValor_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  MonedasViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "MonedasViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerMonedasPorDefecto_Completed(ByVal lo As LoadOperation(Of Moneda))
        If Not lo.HasError Then
            MonedaPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Moneda por defecto", _
                                             Me.ToString(), "TerminoTraerMonedaPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerMonedas(ByVal lo As LoadOperation(Of Moneda))
        If Not lo.HasError Then
            ListaMonedas= dcProxy.Monedas
           	If dcProxy.Monedas.Count > 0 Then
				If lo.UserState = "insert" Then
                    MonedaSelected = ListaMonedas.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    If ListaMonedaValor.Count > 0 Then
                        ListaMonedaValor.Clear()
                    End If
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2ComunesControl.FuncionesCompartidas.obtenerMensajeValidacionErrorPersonalizado("Se presentó un problema en la obtención de la lista de Monedas", Me.ToString, "TerminoTraerEspecie", lo.Error)
            lo.MarkErrorAsHandled()
            'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Monedas", _
            '                                 Me.ToString(), "TerminoTraerMoneda", Application.Current.ToString(), Program.Maquina, lo.Error)
            'lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

'Tablas padres

    Private Sub TerminoTraerMonedaValor(ByVal lo As LoadOperation(Of MonedaValo))
        If Not lo.HasError Then
            ListaMonedaValor = dcProxy.MonedaValos
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de MonedaValor", _
                                             Me.ToString(), "TerminoTraerMonedaValor", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminoTraerMonedaValor_Completed(ByVal lo As LoadOperation(Of MonedaValo))
        If Not lo.HasError Then
            MonedavalorPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Tarifa por defecto", _
                                             Me.ToString(), "TerminoTraerTarifaPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
	

#End Region

#Region "Propiedades"

    Private _ListaMonedas As EntitySet(Of Moneda)
    Public Property ListaMonedas() As EntitySet(Of Moneda)
        Get
            Return _ListaMonedas
        End Get
        Set(ByVal value As EntitySet(Of Moneda))
            _ListaMonedas = value

            MyBase.CambioItem("ListaMonedas")
            MyBase.CambioItem("ListaMonedasPaged")
            If Not IsNothing(value) Then
                If IsNothing(MonedaAnterior) Then
                    MonedaSelected = _ListaMonedas.FirstOrDefault
                Else
                    MonedaSelected = MonedaAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaMonedasPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaMonedas) Then
                Dim view = New PagedCollectionView(_ListaMonedas)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _MonedaSelected As Moneda
    Public Property MonedaSelected() As Moneda
        Get
            Return _MonedaSelected
        End Get
        Set(ByVal value As Moneda)
            _MonedaSelected = value
			If Not value Is Nothing Then
                dcProxy.MonedaValos.Clear()
                dcProxy.Load(dcProxy.Traer_MonedaValor_MonedaQuery(MonedaSelected.Codigo,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMonedaValor, Nothing)
			End If
			MyBase.CambioItem("MonedaSelected")
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
            Dim NewMoneda As New Moneda
			'TODO: Verificar cuales son los campos que deben inicializarse
		NewMoneda.IDComisionista = MonedaPorDefecto.IDComisionista
		NewMoneda.IDSucComisionista = MonedaPorDefecto.IDSucComisionista
		NewMoneda.Codigo = MonedaPorDefecto.Codigo
		NewMoneda.Codigo_internacional = MonedaPorDefecto.Codigo_internacional
		NewMoneda.Descripcion = MonedaPorDefecto.Descripcion
		NewMoneda.Convercion_Dolar = MonedaPorDefecto.Convercion_Dolar
		NewMoneda.Nro_Decimales = MonedaPorDefecto.Nro_Decimales
		NewMoneda.Dias_Cumplimiento = MonedaPorDefecto.Dias_Cumplimiento
		NewMoneda.ValorBase_IVA = MonedaPorDefecto.ValorBase_IVA
		NewMoneda.Usuario = Program.Usuario
		NewMoneda.Actualizacion = MonedaPorDefecto.Actualizacion
		NewMoneda.Mercado_Integrado = MonedaPorDefecto.Mercado_Integrado
        MonedaAnterior = MonedaSelected
            MonedaSelected = NewMoneda
            strCodigoInternacional = String.Empty
        MyBase.CambioItem("Monedas")
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
        dcProxy.Monedas.Clear()
        IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.MonedasFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMonedas, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.MonedasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMonedas, "Filtrar")
            End If
    Catch ex As Exception
		IsBusy = False
        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                         Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
    End Try
End Sub


Public Overrides Sub ConfirmarBuscar()
	Try
            If cb.Codigo <> 0 Or cb.Codigo_internacional <> String.Empty Or cb.Descripcion <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Monedas.Clear()
                MonedaAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " Codigo = " &  cb.Codigo.ToString() 
                dcProxy.Load(dcProxy.MonedasConsultarQuery(cb.Codigo, cb.Codigo_internacional, cb.Descripcion,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMonedas, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaMoneda
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
            If Not IsNothing(MonedaSelected) Then
                If Not IsNothing(MonedaSelected.Dias_Cumplimiento) Then
                    If (MonedaSelected.Dias_Cumplimiento) > 9 Or (MonedaSelected.Dias_Cumplimiento) < 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje("Los días de cumplimiento debe de ser 9 o menos", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If
                If Not IsNothing(MonedaSelected.Nro_Decimales) Then
                    If (MonedaSelected.Nro_Decimales) > 4 Or (MonedaSelected.Nro_Decimales) < 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje("El Nro. de Decimales debe de ser 4 o menos", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If
            End If

            'JAEZ 20161216  se quita la validación para que no necesité un detalle
            'If (ListaMonedaValor.Count) = 0 Then
            '    A2Utilidades.Mensajes.mostrarMensaje("El Detalle es requerido. Digite al menos un valor", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Exit Sub
            'End If

            Dim numeroErrores = (From lr In ListaMonedaValor Where lr.HasValidationErrors = True).Count
            If numeroErrores <> 0 Then
                MessageBox.Show("Por favor revise que todos los campos requeridos en los registros de detalle hayan sido correctamente diligenciados.", "Alerta", MessageBoxButton.OK)
                Exit Sub
            End If


            'Dim CodigoRepetido = From ld In ListaMonedas Where ld.Codigo_internacional.ToUpper = MonedaSelected.Codigo_internacional.ToUpper
            '                              Select ld
            'If CodigoRepetido.Count > 0 Then
            '    A2Utilidades.Mensajes.mostrarMensaje("El Código Internacional " + MonedaSelected.Codigo_internacional + " Ya existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Exit Sub
            'End If
            'If strCodigoInternacional <> MonedaSelected.Codigo_internacional Then


            '    For Each LM In ListaMonedas
            '        If LM.Codigo_internacional.ToUpper = MonedaSelected.Codigo_internacional.ToUpper Then
            '            A2Utilidades.Mensajes.mostrarMensaje("El Código Internacional " + MonedaSelected.Codigo_internacional + " Ya existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '            Exit Sub
            '        End If
            '    Next
            'End If

            Dim obj As List(Of Moneda) = ListaMonedas.Where(Function(i) i.Codigo <> MonedaSelected.Codigo And i.Codigo_internacional.ToUpper = MonedaSelected.Codigo_internacional.ToUpper).ToList

            If obj.Count > 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("El Código Internacional " + MonedaSelected.Codigo_internacional + " Ya existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                'Dim strMsg As String = dcProxy.ValidarMoneda(MonedaSelected.Codigo, MonedaSelected.Codigo_internacional, Program.Usuario, Program.HashConexion).ToString

                'A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.Aplicacion, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            For Each a In ListaMonedaValor
                '   a.FechaValor = Format(a.FechaValor, "yyyy/mm/dd")
                If a.FechaValor.Date = fecha.Date Then
                    i = 1
                    Exit For
                End If
                fecha = a.FechaValor
                If a.Valor_Moneda_Local = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Existe un registro en el campo valor vacio y es requerido. Por favor verifiquelo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    i = 0
                    Exit Sub
                End If
            Next

            If i = 1 Then
                'MessageBox.Show("Fecha Duplicada:", "OyDNet", MessageBoxButton.OK)
                A2Utilidades.Mensajes.mostrarMensaje("Fecha Duplicada:" + fecha.ToShortDateString + "", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                i = 0
            Else
                IsBusy = True
                dcProxy.ValidarMoneda(MonedaSelected.Codigo, MonedaSelected.Codigo_internacional, Program.Usuario, Program.HashConexion, AddressOf TerminoValidarMoneda, String.Empty)
            End If
            fecha = Nothing
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoValidarMoneda(ByVal lo As InvokeOperation(Of String))
        Try
            If IsNothing(lo.Error) Then
                If String.IsNullOrEmpty(lo.Value) Then
                    Dim origen = "update"
                    ErrorForma = ""
                    MonedaAnterior = MonedaSelected

                    If Not ListaMonedas.Contains(MonedaSelected) Then
                        origen = "insert"
                        ListaMonedas.Add(MonedaSelected)
                    End If
                    IsBusy = True
                    Program.VerificarCambiosProxyServidor(dcProxy)
                    dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
                Else
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarMensaje(lo.Value, Program.Aplicacion, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                Me.ToString(), "TerminoValidarMoneda", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "TerminoValidarMoneda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                If So.UserState = "BorrarRegistro" Or So.UserState = "update" Then
                    dcProxy.RejectChanges()
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
        If Not IsNothing(_MonedaSelected) Then
            Editando = True
            strCodigoInternacional = MonedaSelected.Codigo_internacional
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_MonedaSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _MonedaSelected.EntityState = EntityState.Detached Then
                    MonedaSelected = MonedaAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_MonedaSelected) Then
                'dcProxy.Monedas.Remove(_MonedaSelected)
                'MonedaSelected = _ListaMonedas.LastOrDefault
                IsBusy = True
                'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                dcProxy.EliminarMonedas(MonedaSelected.Codigo, String.Empty,Program.Usuario, Program.HashConexion, AddressOf terminoeliminar, "borrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub terminoeliminar(ByVal So As InvokeOperation(Of String))
        If So.HasError Then
            A2Utilidades.Mensajes.mostrarMensaje(So.Error.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Else
            If Not (So.Value) = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje(So.Value.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                If So.UserState = "borrar" Then
                    dcProxy.Monedas.Clear()
                    dcProxy.Load(dcProxy.MonedasFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerMonedas, "insert") ' Recarga la lista para que carguen los include
                End If
            End If
        End If
        IsBusy = False
    End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub
    Public Sub llenarDiccionario()
        DicCamposTab.Add("ValorBase_IVA", 1)
        DicCamposTab.Add("Codigo_internacional", 1)
        DicCamposTab.Add("Descripcion", 1)
    End Sub

    Public Overrides Sub Buscar()
        cb.Codigo = 0
        cb.Codigo_internacional = String.Empty
        cb.Descripcion = String.Empty
        MyBase.Buscar()
    End Sub

#End Region

#Region "Tablas hijas"


    '******************************************************** MonedaValor 
    Private _ListaMonedaValor As EntitySet(Of MonedaValo)
    Public Property ListaMonedaValor() As EntitySet(Of MonedaValo)
        Get
            Return _ListaMonedaValor
        End Get
        Set(ByVal value As EntitySet(Of MonedaValo))
            _ListaMonedaValor = value
            MyBase.CambioItem("ListaMonedaValor")
            MyBase.CambioItem("ListaMonedaValorPaged")
        End Set
    End Property

    Public ReadOnly Property MonedaValorPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaMonedaValor) Then
                Dim view = New PagedCollectionView(_ListaMonedaValor)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
    Private _ListamonedaAnterior As List(Of MonedaValo)
    Public Property ListamonedaAnterior() As List(Of MonedaValo)
        Get
            Return _ListamonedaAnterior
        End Get
        Set(ByVal value As List(Of MonedaValo))
            _ListamonedaAnterior = value
            MyBase.CambioItem("ListamonedaAnterior")
        End Set
    End Property

    Private _MonedaValoSelected As MonedaValo
    Public Property MonedaValoSelected() As MonedaValo
        Get
            Return _MonedaValoSelected
        End Get
        Set(ByVal value As MonedaValo)
            _MonedaValoSelected = value
            If Not value Is Nothing Then
                MyBase.CambioItem("MonedaValoSelected")
            End If
        End Set
    End Property

    Public Overrides Sub NuevoRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmMonedaValo"
                Dim NewMonedaValo As New MonedaValo


                ListamonedaAnterior = ListaMonedaValor.ToList
                ListaMonedaValor.Clear()
                x = 0

                While ListamonedaAnterior.Any
                    MonedaValoSelected = ListamonedaAnterior.First
                    MonedaValoSelected.NroRegistro = x
                    ListaMonedaValor.Add(MonedaValoSelected)
                    If ListamonedaAnterior.Contains(MonedaValoSelected) Then
                        ListamonedaAnterior.Remove(MonedaValoSelected)
                    End If
                    x = x + 1
                End While
                NewMonedaValo.Codigo = MonedaSelected.Codigo
                NewMonedaValo.Usuario = Program.Usuario
                NewMonedaValo.Base_IVA_Diario = 0
                NewMonedaValo.FechaValor = Now
                NewMonedaValo.Actualizacion = Now
                NewMonedaValo.NroRegistro = x
                ListaMonedaValor.Add(NewMonedaValo)
                MonedaValoSelected = NewMonedaValo
                MyBase.CambioItem("MonedaValoSelected")
                MyBase.CambioItem("ListaMonedaValo")

        End Select
    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        'MessageBox.Show("Esta Funcionalidad No Esta Habilitada", "Funcionalidad", MessageBoxButton.OK)
        ' A2Utilidades.Mensajes.mostrarMensaje("Esta Funcionalidad No Esta Habilitada", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Select Case NombreColeccionDetalle
            Case "cmMonedaValo"
                If Not IsNothing(ListaMonedaValor) Then
                    If Not IsNothing(ListaMonedaValor) Then
                        Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(MonedaValoSelected, ListaMonedaValor.ToList)

                        ListaMonedaValor.Remove(_MonedaValoSelected)
                        MonedaValoSelected = _ListaMonedaValor.LastOrDefault
                        If ListaMonedaValor.Count > 0 Then
                            Program.PosicionarItemLista(MonedaValoSelected, ListaMonedaValor.ToList, intRegistroPosicionar)
                        Else
                            MonedaValoSelected = Nothing
                        End If
                        MyBase.CambioItem("MonedaValoSelected")
                        MyBase.CambioItem("ListaMonedaValor")
                    End If
                End If

        End Select
    End Sub
#End Region
End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaMoneda
    Implements INotifyPropertyChanged

    'Public Property Codigo As Integer

    Private _Codigo As Integer = 0
    <Display(Name:="Código")> _
    Public Property Codigo As Integer
        Get
            Return _Codigo
        End Get
        Set(ByVal value As Integer)
            _Codigo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Codigo"))
        End Set
    End Property

    '<Display(Name:="Código Internacional")> _
    'Public Property Codigo_internacional As String

    Private _Codigo_internacional As String = String.Empty
    <Display(Name:="Código Internacional")> _
    Public Property Codigo_internacional As String
        Get
            Return _Codigo_internacional
        End Get
        Set(ByVal value As String)
            _Codigo_internacional = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Codigo_internacional"))
        End Set
    End Property

    '<Display(Name:="Descripción")> _
    'Public Property Descripcion As String

    Private _Descripcion As String = String.Empty
    <Display(Name:="Descripción")> _
    Public Property Descripcion As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Descripcion"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class




