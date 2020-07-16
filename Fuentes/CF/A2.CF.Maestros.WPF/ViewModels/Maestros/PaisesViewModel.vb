Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: PaisesViewModel.vb
'Generado el : 04/12/2011 16:59:12
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
Imports A2.OyD.OYDServer.RIA.Web.CFMaestros

Public Class PaisesViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaPaise
    Private PaisePorDefecto As Paise
    Private PaiseAnterior As Paise
    Dim dcProxy As MaestrosCFDomainContext
    Dim dcProxy1 As MaestrosCFDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)
    Dim objProxy As UtilidadesDomainContext 'JAG 20140129
    Dim mlogVisualizaNivelRiesgo As Boolean 'JAG 20140129

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosCFDomainContext()
            dcProxy1 = New MaestrosCFDomainContext()
            objProxy = New UtilidadesDomainContext()
        Else
            dcProxy = New MaestrosCFDomainContext(New System.Uri(Program.RutaServicioMaestros))
            dcProxy1 = New MaestrosCFDomainContext(New System.Uri(Program.RutaServicioMaestros))
            objProxy = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.PaisesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPaises, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerPaisePorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPaisesPorDefecto_Completed, "Default")
                objProxy.Verificaparametro("VISUALIZA_NIVELRIESGO_PAIS", Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "VISUALIZA_NIVELRIESGO_PAIS") 'JAG 20140129

                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  PaisesViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "PaisesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    'JAG 20140129
    Private Sub Terminotraerparametro(ByVal obj As InvokeOperation(Of String))
        If obj.HasError Then
            obj.MarkErrorAsHandled()
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó el parametro", Me.ToString(), "Terminotraerparametro", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            Select Case obj.UserState
                Case "VISUALIZA_NIVELRIESGO_PAIS"
                    If obj.Value = "SI" Then
                        bitVisualizaNivelRiesgo = Visibility.Visible
                        mlogVisualizaNivelRiesgo = True
                    Else
                        bitVisualizaNivelRiesgo = Visibility.Collapsed
                        mlogVisualizaNivelRiesgo = False
                    End If
            End Select
        End If
    End Sub
    'JAG 20140129

    Private Sub TerminoTraerPaisesPorDefecto_Completed(ByVal lo As LoadOperation(Of Paise))
        If Not lo.HasError Then
            PaisePorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Paise por defecto", _
                                             Me.ToString(), "TerminoTraerPaisePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerPaises(ByVal lo As LoadOperation(Of Paise))
        If Not lo.HasError Then
            ListaPaises = dcProxy.Paises
            If dcProxy.Paises.Count > 0 Then
                If lo.UserState = "insert" Then
                    PaiseSelected = ListaPaises.First
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2ComunesControl.FuncionesCompartidas.obtenerMensajeValidacionErrorPersonalizado("Se presentó un problema en la obtención de la lista de Paises", Me.ToString, "TerminoTraerEspecie", lo.Error)
            lo.MarkErrorAsHandled()
        End If
        IsBusy = False
    End Sub


    Private Sub TerminoTraerDepartamentos(ByVal lo As LoadOperation(Of Departamento))
        If Not lo.HasError Then
            ListaDepartamentos = dcProxy.Departamentos
            If dcProxy.Departamentos.Count > 0 Then
                'If PaiseSelected.ID <> dcProxy.Departamentos.First.IDPais Then
                '    Exit Sub
                'End If
                If lo.UserState = "insert" Then
                    DepartamentoSelected = ListaDepartamentos.FirstOrDefault
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'Se comentan estas dos lineas para que no vuelva a regarcar la lista si no encuentra ningun registro.
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Departamentos", _
                                             Me.ToString(), "TerminoTraerDepartamentos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub


#End Region

#Region "Propiedades"

    'JAG 20140129
    Private _bitVisualizaNivelRiesgo As Visibility
    Public Property bitVisualizaNivelRiesgo() As Visibility
        Get
            Return _bitVisualizaNivelRiesgo
        End Get
        Set(ByVal value As Visibility)
            _bitVisualizaNivelRiesgo = value
            MyBase.CambioItem("bitVisualizaNivelRiesgo")
        End Set
    End Property
    'JAG 20140129

    ''' <history>
    ''' Cambio por:     Javier Ed
    ''' Descripción:    Se comenta la linea que trae el país anterior debido a que al FILTRAR la lista de 
    '''                 departamentos no se limpia, sino que se acumula.
    ''' ID del cambio:  JEPM20150914
    ''' </history>
    Private _ListaPaises As EntitySet(Of Paise)
    Public Property ListaPaises() As EntitySet(Of Paise)
        Get
            Return _ListaPaises
        End Get
        Set(ByVal value As EntitySet(Of Paise))
            _ListaPaises = value

            MyBase.CambioItem("ListaPaises")
            MyBase.CambioItem("ListaPaisesPaged")
            If Not IsNothing(value) Then
                If IsNothing(PaiseAnterior) Then
                    PaiseSelected = _ListaPaises.FirstOrDefault
                Else
                    'JEPM20150914
                    'PaiseSelected = PaiseAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaPaisesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaPaises) Then
                Dim view = New PagedCollectionView(_ListaPaises)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _PaiseSelected As Paise
    Public Property PaiseSelected() As Paise
        Get
            Return _PaiseSelected
        End Get
        Set(ByVal value As Paise)
            _PaiseSelected = value
            'dcProxy.Departamentos.Clear()
            If Not IsNothing(value) Then
                dcProxy.Departamentos.Clear()
                dcProxy.Load(dcProxy.Traer_Departamentos_PaiseQuery(PaiseSelected.ID, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerDepartamentos, Nothing)
            End If
            MyBase.CambioItem("PaiseSelected")
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
            Dim NewPaise As New Paise
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewPaise.IDComisionista = PaisePorDefecto.IDComisionista
            NewPaise.IDSucComisionista = PaisePorDefecto.IDSucComisionista
            NewPaise.ID = PaisePorDefecto.ID
            NewPaise.Nombre = PaisePorDefecto.Nombre
            NewPaise.Codigo_ISO = PaisePorDefecto.Codigo_ISO
            NewPaise.Actualizacion = PaisePorDefecto.Actualizacion
            NewPaise.Usuario = Program.Usuario
            NewPaise.IDPais = PaisePorDefecto.IDPais
            NewPaise.CodigoDane = PaisePorDefecto.CodigoDane
            NewPaise.NivelRiesgo = PaisePorDefecto.NivelRiesgo 'JAG 20140128
            NewPaise.ZonaEconomica = PaisePorDefecto.ZonaEconomica 'JEPM20150901
            PaiseAnterior = PaiseSelected
            PaiseSelected = NewPaise
            MyBase.CambioItem("Paises")
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
            dcProxy.Paises.Clear()
            dcProxy.Departamentos.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.PaisesFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPaises, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.PaisesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPaises, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.ID <> 0 Or cb.Nombre <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Paises.Clear()
                PaiseAnterior = Nothing
                IsBusy = True
                'Dim DescripcionFiltroVM = " ID = " & cb.ID.ToString() & " Nombre = " & cb.Nombre.ToString()
                dcProxy.Load(dcProxy.PaisesConsultarQuery(cb.ID, cb.Nombre, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPaises, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaPaise
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()

        Dim numeroErrores = (From lr In ListaDepartamentos Where lr.HasValidationErrors = True).Count
        If numeroErrores <> 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("Por favor revise que todos los campos requeridos en los registros de detalle que hayan sido correctamente diligenciados.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            'MessageBox.Show("Por favor revise que todos los campos requeridos en los registros de detalle hayan sido correctamente diligenciados.", "Alerta", MessageBoxButton.OK)
            Exit Sub
        End If
        'JAG 20140129
        If (PaiseSelected.NivelRiesgo = 0) And mlogVisualizaNivelRiesgo = True Then
            A2Utilidades.Mensajes.mostrarMensaje("El nivel de riesgo del pais es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If
        'JAG 20140129
        Try
            Dim origen = "update"
            ErrorForma = ""
            PaiseAnterior = PaiseSelected
            If Not ListaPaises.Contains(PaiseSelected) Then
                origen = "insert"
                ListaPaises.Add(PaiseSelected)
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

                Dim numeroErrores = (From lr In ListaDepartamentos Where lr.HasValidationErrors = True).Count
                If numeroErrores <> 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Por favor revise que todos los campos requeridos en los registros de detalle que hayan sido correctamente diligenciados.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                    Exit Sub
                End If

                If (So.Error.Message.Contains("ErrorPersonalizadoBorrarHija,") = True) And ((So.UserState = "insert") Or (So.UserState = "update") Or (So.UserState = "BorrarRegistro")) Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizadoBorrarHija,") '.Split(So.Error.Message, vbCr)
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'If So.UserState = "insert" Then
                    '    ListaDepartamentos.Remove(DepartamentoSelected)
                    'End If
                    If So.Error.Message.Contains("ErrorPersonalizadoBorrarHija,") = True Then
                        dcProxy.RejectChanges()
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(So.Error.Message.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                    '                  Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                End If

                'A2Utilidades.Mensajes.mostrarMensaje(So.Error.Message.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If

                So.MarkErrorAsHandled()
                Exit Try
            End If
            'If So.UserState = "insert" Then
            '    dcProxy.Paises.Clear()
            '    dcProxy.Load(dcProxy.PaisesFiltrarQuery(""), AddressOf TerminoTraerPaises, "insert") ' Recarga la lista para que carguen los include
            'End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_PaiseSelected) Then
            Editando = True
            PaiseSelected.Usuario = Program.Usuario
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_PaiseSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _PaiseSelected.EntityState = EntityState.Detached Then
                    PaiseSelected = PaiseAnterior
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub CancelarBuscar()
        Try
            cb = New CamposBusquedaPaise
            CambioItem("cb")
            MyBase.CancelarBuscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la Búsqueda", _
                     Me.ToString(), "CancelarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(_PaiseSelected) Then
                'dcProxy.Paises.Remove(_PaiseSelected)
                'PaiseSelected = _ListaPaises.LastOrDefault
                IsBusy = True
                'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                dcProxy.EliminarPaises(PaiseSelected.IDPais, String.Empty, Program.Usuario, Program.HashConexion, AddressOf terminoeliminar, "borrar")
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
                    dcProxy.Paises.Clear()
                    dcProxy.Load(dcProxy.PaisesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPaises, "insert") ' Recarga la lista para que carguen los include
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
        DicCamposTab.Add("Nombre", 1)
    End Sub
#End Region

#Region "Tablas hijas"

    '******************************************************** Departamentos 
    Private _ListaDepartamentos As EntitySet(Of Departamento)
    Public Property ListaDepartamentos() As EntitySet(Of Departamento)
        Get
            Return _ListaDepartamentos
        End Get
        Set(ByVal value As EntitySet(Of Departamento))
            _ListaDepartamentos = value
            'If IsNothing(value) Then
            '    DepartamentoSelected = _ListaDepartamentos.FirstOrDefault
            'End If
            DepartamentoSelected = _ListaDepartamentos.FirstOrDefault
            MyBase.CambioItem("ListaDepartamentos")
            MyBase.CambioItem("ListaDepartamentosPaged")
        End Set
    End Property

    Public ReadOnly Property DepartamentosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaDepartamentos) Then
                Dim view = New PagedCollectionView(_ListaDepartamentos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _DepartamentoSelected As Departamento
    Public Property DepartamentoSelected() As Departamento
        Get
            Return _DepartamentoSelected
        End Get
        Set(ByVal value As Departamento)
            _DepartamentoSelected = value
            'If Not value Is Nothing Then
            '    MyBase.CambioItem("DepartamentoSelected")
            'End If
            MyBase.CambioItem("DepartamentoSelected")
        End Set
    End Property

    Public Overrides Sub NuevoRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmDepartamento"
                    Dim NewDepartamento As New Departamento
                    If ListaDepartamentos.Count > 0 Then
                        NewDepartamento.IDDepartamento = ListaDepartamentos.Count + 1
                    End If
                    NewDepartamento.IDPais = PaiseSelected.ID
                    NewDepartamento.Usuario = "0"
                    ListaDepartamentos.Add(NewDepartamento)
                    DepartamentoSelected = NewDepartamento
                    MyBase.CambioItem("DepartamentoSelected")
                    MyBase.CambioItem("ListaDepartamento")

            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al añadir un registro", _
             Me.ToString(), "NuevoRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmDepartamento"
                If Not IsNothing(ListaDepartamentos) Then
                    If Not IsNothing(_DepartamentoSelected) Then
                        Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(DepartamentoSelected, ListaDepartamentos.ToList)
                        ListaDepartamentos.Remove(_DepartamentoSelected)
                        If ListaDepartamentos.Count > 0 Then
                            Program.PosicionarItemLista(DepartamentoSelected, ListaDepartamentos.ToList, intRegistroPosicionar)
                        End If
                        MyBase.CambioItem("DepartamentoSelected")
                        MyBase.CambioItem("ListaDepartamentos")
                    End If
                End If

        End Select
    End Sub

    'Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
    '    If DicCamposTab.ContainsKey(pstrNombreCampo) Then
    '        Dim miTab = DicCamposTab(pstrNombreCampo)
    '        TabSeleccionadaFinanciero = miTab
    '    End If
    'End Sub
    'Public Sub llenarDiccionario()
    '    DicCamposTab.Add("Nombre", 1)
    'End Sub
#End Region
End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaPaise

    <Display(Name:="ID")> _
    Public Property ID As Integer

    <StringLength(40, ErrorMessage:="La longitud máxima es de 40")> _
     <Display(Name:="Nombre")> _
    Public Property Nombre As String
End Class




