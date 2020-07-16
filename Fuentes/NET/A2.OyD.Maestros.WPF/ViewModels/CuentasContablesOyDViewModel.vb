Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: CuentasContablesOyDViewModel.vb
'Generado el : 04/06/2011 13:31:25
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
Imports Microsoft.VisualBasic.CompilerServices

Public Class CuentasContablesOyDViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaCuentasContablesOy
    Private CuentasContablesOyPorDefecto As CuentasContablesOy
    Private CuentasContablesOyAnterior As CuentasContablesOy
    Private TipoCuenta As String
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
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
                dcProxy.Load(dcProxy.CuentasContablesOyDFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasContablesOyD, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerCuentasContablesOyPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasContablesOyDPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  CuentasContablesOyDViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "CuentasContablesOyDViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerCuentasContablesOyDPorDefecto_Completed(ByVal lo As LoadOperation(Of CuentasContablesOy))
        If Not lo.HasError Then
            CuentasContablesOyPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la CuentasContablesOy por defecto", _
                                             Me.ToString(), "TerminoTraerCuentasContablesOyPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerCuentasContablesOyD(ByVal lo As LoadOperation(Of CuentasContablesOy))
        Try
            If Not lo.HasError Then
                ListaCuentasContablesOyD = dcProxy.CuentasContablesOys
                Dim ctacble As CuentasContablesOy = ListaCuentasContablesOyD.Where(Function(i) i.IDCuentaContable = -1).ToList.FirstOrDefault
                If dcProxy.CuentasContablesOys.Count > 0 Then
                    TipoCuenta = dcProxy.CuentasContablesOys.Last.Msg
                End If
                'ListaCuentasContablesOyD.Remove(ListaCuentasContablesOyD.Last)
                If Not IsNothing(ctacble) Then ListaCuentasContablesOyD.Remove(ctacble)
                'For i = 0 To dcProxy.CuentasContablesOys.Count - 2
                '    ListaCuentasContablesOyD.Add(dcProxy.CuentasContablesOys(i))
                'Next
                'ListaCuentasContablesOyD.Remove(ListaCuentasContablesOyD.Last)
                If dcProxy.CuentasContablesOys.Count > 0 Then
                    If lo.UserState = "insert" Then
                        CuentasContablesOySelected = ListaCuentasContablesOyD.Last
                    End If
                Else
                    If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                        A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        'MyBase.Buscar()
                        'MyBase.CancelarBuscar()
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de CuentasContablesOyD", _
                                                 Me.ToString(), "TerminoTraerCuentasContablesOy", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al retornar el resultado de la consulta", _
                                 Me.ToString(), "TerminoTraerCuentasContablesOyD", Application.Current.ToString(), Program.Maquina, ex)
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
                    dcProxy.CuentasContablesOys.Clear()
                    dcProxy.Load(dcProxy.CuentasContablesOyDFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasContablesOyD, "insert") ' Recarga la lista para que carguen los include
                End If
            End If
        End If
        IsBusy = False
    End Sub


#End Region

#Region "Propiedades"

    Private _ListaCuentasContablesOyD As EntitySet(Of CuentasContablesOy)
    Public Property ListaCuentasContablesOyD() As EntitySet(Of CuentasContablesOy)
        Get
            Return _ListaCuentasContablesOyD
        End Get
        Set(ByVal value As EntitySet(Of CuentasContablesOy))
            _ListaCuentasContablesOyD = value

            MyBase.CambioItem("ListaCuentasContablesOyD")
            MyBase.CambioItem("ListaCuentasContablesOyDPaged")
            If Not IsNothing(value) Then
                If IsNothing(CuentasContablesOyAnterior) Then
                    CuentasContablesOySelected = _ListaCuentasContablesOyD.FirstOrDefault
                Else
                    CuentasContablesOySelected = CuentasContablesOyAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaCuentasContablesOyDPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCuentasContablesOyD) Then
                Dim view = New PagedCollectionView(_ListaCuentasContablesOyD)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _CuentasContablesOySelected As CuentasContablesOy
    Public Property CuentasContablesOySelected() As CuentasContablesOy
        Get
            Return _CuentasContablesOySelected
        End Get
        Set(ByVal value As CuentasContablesOy)
            _CuentasContablesOySelected = value
            MyBase.CambioItem("CuentasContablesOySelected")
        End Set
    End Property

    Private _txtIdHabilitado As Boolean
    Public Property txtIdHabilitado() As Boolean
        Get
            Return _txtIdHabilitado
        End Get
        Set(ByVal value As Boolean)
            _txtIdHabilitado = value
            MyBase.CambioItem("txtIdHabilitado")
        End Set
    End Property

    Private _cmbNaturalezaHabilitado As Boolean
    Public Property cmbNaturalezaHabilitado() As Boolean
        Get
            Return _cmbNaturalezaHabilitado
        End Get
        Set(ByVal value As Boolean)
            _cmbNaturalezaHabilitado = value
            MyBase.CambioItem("cmbNaturalezaHabilitado")
        End Set
    End Property

    Private _cmbDctoAsociadoHabilitado As Boolean
    Public Property cmbDctoAsociadoHabilitado() As Boolean
        Get
            Return _cmbDctoAsociadoHabilitado
        End Get
        Set(ByVal value As Boolean)
            _cmbDctoAsociadoHabilitado = value
            MyBase.CambioItem("cmbDctoAsociadoHabilitado")
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

    Private _CuentaTerceros As Boolean = False
    Public Property CuentaTerceros() As Boolean
        Get
            Return _CuentaTerceros
        End Get
        Set(ByVal value As Boolean)
            _CuentaTerceros = value
            MyBase.CambioItem("CuentaTerceros")
        End Set
    End Property

    Private _CCostos As Boolean = False
    Public Property CCostos() As Boolean
        Get
            Return _CCostos
        End Get
        Set(ByVal value As Boolean)
            _CCostos = value
            MyBase.CambioItem("CCostos")
        End Set
    End Property

    Private _chkHabilitado As Boolean
    Public Property chkHabilitado() As Boolean
        Get
            Return _chkHabilitado
        End Get
        Set(ByVal value As Boolean)
            _chkHabilitado = value
            MyBase.CambioItem("chkHabilitado")
        End Set
    End Property

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            If TipoCuenta = "V" Then
                A2Utilidades.Mensajes.mostrarMensaje("Esta opción solo se habilitará en caso de no tener el aplicativo de EnCuenta", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                'A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no esta habilitada en su version de OyD, hagalo desde en EnCuenta", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            txtIdHabilitado = True
            cmbNaturalezaHabilitado = True
            cmbDctoAsociadoHabilitado = True
            chkHabilitado = True

            Dim NewCuentasContablesOy As New CuentasContablesOy
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewCuentasContablesOy.IDComisionista = CuentasContablesOyPorDefecto.IDComisionista
            NewCuentasContablesOy.IDSucComisionista = CuentasContablesOyPorDefecto.IDSucComisionista
            NewCuentasContablesOy.ID = CuentasContablesOyPorDefecto.ID
            NewCuentasContablesOy.Nombre = CuentasContablesOyPorDefecto.Nombre
            NewCuentasContablesOy.Naturaleza = CuentasContablesOyPorDefecto.Naturaleza
            NewCuentasContablesOy.DctoAsociado = CuentasContablesOyPorDefecto.DctoAsociado
            NewCuentasContablesOy.actualizacion = CuentasContablesOyPorDefecto.actualizacion
            NewCuentasContablesOy.Usuario = Program.Usuario
            NewCuentasContablesOy.IDCuentaContable = CuentasContablesOyPorDefecto.IDCuentaContable
            NewCuentasContablesOy.CuentaTerceros = CuentasContablesOyPorDefecto.CuentaTerceros
            NewCuentasContablesOy.CCostos = CuentasContablesOyPorDefecto.CCostos
            CuentasContablesOyAnterior = CuentasContablesOySelected
            CuentasContablesOySelected = NewCuentasContablesOy
            PropiedadTextoCombos = ""
            MyBase.CambioItem("CuentasContablesOyD")
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
            dcProxy.CuentasContablesOys.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.CuentasContablesOyDFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasContablesOyD, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.CuentasContablesOyDFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasContablesOyD, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.ID <> String.Empty Or cb.Nombre <> String.Empty Or cb.Naturaleza <> String.Empty Or cb.DctoAsociado <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.CuentasContablesOys.Clear()
                CuentasContablesOyAnterior = Nothing
                IsBusy = True
                'DescripcionFiltroVM = " ID = " &  cb.ID.ToString() & " Nombre = " &  cb.Nombre.ToString() & " Naturaleza = " &  cb.Naturaleza.ToString() & " DctoAsociado = " &  cb.DctoAsociado.ToString() 
                dcProxy.Load(dcProxy.CuentasContablesOyDConsultarQuery(cb.ID, cb.Nombre, cb.Naturaleza, cb.DctoAsociado,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasContablesOyD, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaCuentasContablesOy
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
            CuentasContablesOyAnterior = CuentasContablesOySelected
            If Not ListaCuentasContablesOyD.Contains(CuentasContablesOySelected) Then
                Dim duplicado = (From listaCtasContables In ListaCuentasContablesOyD Where
                                 listaCtasContables.DctoAsociado = CuentasContablesOySelected.DctoAsociado And
                                 listaCtasContables.ID = CuentasContablesOySelected.ID And listaCtasContables.Naturaleza = CuentasContablesOySelected.Naturaleza).Count
                If Not duplicado > 0 Then
                    origen = "insert"
                    ListaCuentasContablesOyD.Add(CuentasContablesOySelected)
                    txtIdHabilitado = False
                    cmbNaturalezaHabilitado = False
                    cmbDctoAsociadoHabilitado = False
                    chkHabilitado = False
                Else
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarMensaje("La cuenta contable ya existe con esa Naturaleza y Documento Asociado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MessageBox.Show("La cuenta contable ya existe con esa Naturaleza y Documento Asociado.", "Registro duplicado", MessageBoxButton.OK)
                    Exit Sub
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
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)

                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                    ListaCuentasContablesOyD.Remove(ListaCuentasContablesOyD.Last)
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
        If TipoCuenta = "V" Then
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta opción solo se habilitará en caso de no tener el aplicativo de EnCuenta", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            'A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no esta habilitada en su version de OyD, hagalo desde en EnCuenta", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If
        If Not IsNothing(_CuentasContablesOySelected) Then
            Editando = True
            chkHabilitado = True
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_CuentasContablesOySelected) Then
                dcProxy.RejectChanges()
                'ListaCuentasContablesOyD.Remove(ListaCuentasContablesOyD.Last)
                Editando = False
                txtIdHabilitado = False
                cmbNaturalezaHabilitado = False
                cmbDctoAsociadoHabilitado = False
                chkHabilitado = False

                If _CuentasContablesOySelected.EntityState = EntityState.Detached Then
                    CuentasContablesOySelected = CuentasContablesOyAnterior
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
            If TipoCuenta = "V" Then
                A2Utilidades.Mensajes.mostrarMensaje("Esta opción solo se habilitará en caso de no tener el aplicativo de EnCuenta", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                'A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no esta habilitada en su version de OyD, hagalo desde en EnCuenta", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            If Not IsNothing(_CuentasContablesOySelected) Then
                'dcProxy.CuentasContablesOys.Remove(_CuentasContablesOySelected)
                'CuentasContablesOySelected = _ListaCuentasContablesOyD.LastOrDefault
                IsBusy = True
                'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
                dcProxy.EliminarCuentasContables(CuentasContablesOySelected.IDCuentaContable, String.Empty,Program.Usuario, Program.HashConexion, AddressOf Terminoeliminar, "borrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub
    Public Sub llenarDiccionario()
        DicCamposTab.Add("ID", 1)
        DicCamposTab.Add("Nombre", 1)
        DicCamposTab.Add("Naturaleza", 1)
        DicCamposTab.Add("DctoAsociado", 1)
    End Sub

    Public Overrides Sub Buscar()
        If TipoCuenta = "V" Then
            A2Utilidades.Mensajes.mostrarMensaje("Esta opción solo se habilitará en caso de no tener el aplicativo de EnCuenta", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            'A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no esta habilitada en su version de OyD, hagalo desde en EnCuenta", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If
        MyBase.Buscar()
    End Sub

    'Private Sub _CuentasContablesOySelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _CuentasContablesOySelected.PropertyChanged
    '    'If CambioNroDoc Then
    '    '    CambioNroDoc = False
    '    '    Exit Sub
    '    'End If
    '    If e.PropertyName.Equals("ID") Then
    '        If Not Versioned.IsNumeric(_CuentasContablesOySelected.ID) Then
    '            If Not (_CuentasContablesOySelected.ID = String.Empty) Then
    '                A2Utilidades.Mensajes.mostrarMensaje("El Número Documento Actual debe ser un valor numérico", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    '                'CambioNroDoc = True
    '                _CuentasContablesOySelected.ID = String.Empty
    '            End If
    '        End If
    '    End If
    '    'If e.PropertyName.Equals("TipoIdentificacionIdActual") Then

    '    'End If
    'End Sub

#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaCuentasContablesOy

    <StringLength(15, ErrorMessage:="La longitud máxima es de 15")> _
    <Display(Name:="Código")> _
    Public Property ID As String

    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")> _
     <Display(Name:="Nombre")> _
    Public Property Nombre As String

    <StringLength(1, ErrorMessage:="La longitud máxima es de 1")> _
    <Display(Name:="Naturaleza")> _
    Public Property Naturaleza As String

    <StringLength(2, ErrorMessage:="La longitud máxima es de 2")> _
     <Display(Name:="Documento Asociado ")> _
    Public Property DctoAsociado As String
End Class




