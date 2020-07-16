Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ExcepcionesRDIPViewModel.vb
'Generado el : 08/09/2011 13:22:06
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
Imports A2.OyD.OYDServer.RIA.Web.OyDCitiBank

Public Class ExcepcionesRDIPViewModel
    Inherits A2ControlMenu.A2ViewModel
    Property cb As New CamposBusquedaExcepcionesRDI
    Private ExcepcionesRDIPorDefecto As ExcepcionesRDI
    Private ExcepcionesRDIAnterior As ExcepcionesRDI
    Dim dcProxy As CitiBankDomainContext
    Dim dcProxy1 As CitiBankDomainContext
    Dim DicCamposTab As New Dictionary(Of String, Integer)

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New CitiBankDomainContext()
                dcProxy1 = New CitiBankDomainContext()
            Else
                dcProxy = New CitiBankDomainContext(New System.Uri((Program.RutaServicioNegocio)))
                dcProxy1 = New CitiBankDomainContext(New System.Uri((Program.RutaServicioNegocio)))
            End If
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.ExcepcionesRDIPFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerExcepcionesRDIP, "")
                dcProxy1.Load(dcProxy1.TraerExcepcionesRDIPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerExcepcionesRDIPPorDefecto_Completed, "Default")
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container    
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  ExcepcionesRDIPViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ExcepcionesRDIPViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerExcepcionesRDIPPorDefecto_Completed(ByVal lo As LoadOperation(Of ExcepcionesRDI))
        Try
            If Not lo.HasError Then
                ExcepcionesRDIPorDefecto = lo.Entities.FirstOrDefault
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ExcepcionesRDI por defecto", _
                                                 Me.ToString(), "TerminoTraerExcepcionesRDIPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ExcepcionesRDI por defecto", _
                                             Me.ToString(), "TerminoTraerExcepcionesRDIPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerExcepcionesRDIP(ByVal lo As LoadOperation(Of ExcepcionesRDI))
        Try
            If Not lo.HasError Then
                ListaExcepcionesRDIP = dcProxy.ExcepcionesRDIs
                If dcProxy.ExcepcionesRDIs.Count > 0 Then
                    If lo.UserState = "insert" Then
                        ExcepcionesRDISelected = ListaExcepcionesRDIP.Last
                    End If
                Else
                    If lo.UserState = "Busqueda" Then
                        A2Utilidades.Mensajes.mostrarMensaje("No se encontro ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        'MessageBox.Show("No se encontro ningún registro")
                        'MyBase.Buscar()
                        'MyBase.CancelarBuscar()
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ExcepcionesRDIP", _
                                                 Me.ToString(), "TerminoTraerExcepcionesRDI", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
            IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ExcepcionesRDIP", _
                                             Me.ToString(), "TerminoTraerExcepcionesRDI", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'Tablas padres

#End Region

#Region "Propiedades"

    Private _ListaExcepcionesRDIP As EntitySet(Of ExcepcionesRDI)
    Public Property ListaExcepcionesRDIP() As EntitySet(Of ExcepcionesRDI)
        Get
            Return _ListaExcepcionesRDIP
        End Get
        Set(ByVal value As EntitySet(Of ExcepcionesRDI))
            _ListaExcepcionesRDIP = value

            MyBase.CambioItem("ListaExcepcionesRDIP")
            MyBase.CambioItem("ListaExcepcionesRDIPPaged")
            If Not IsNothing(value) Then
                If IsNothing(ExcepcionesRDIAnterior) Then
                    ExcepcionesRDISelected = _ListaExcepcionesRDIP.FirstOrDefault
                Else
                    ExcepcionesRDISelected = ExcepcionesRDIAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaExcepcionesRDIPPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaExcepcionesRDIP) Then
                Dim view = New PagedCollectionView(_ListaExcepcionesRDIP)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ExcepcionesRDISelected As ExcepcionesRDI
    Public Property ExcepcionesRDISelected() As ExcepcionesRDI
        Get
            Return _ExcepcionesRDISelected
        End Get
        Set(ByVal value As ExcepcionesRDI)
            _ExcepcionesRDISelected = value
            MyBase.CambioItem("ExcepcionesRDISelected")
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
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no esta habilitada para el formulario.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.ExcepcionesRDIs.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ExcepcionesRDIPFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerExcepcionesRDIP, Nothing)
            Else
                dcProxy.Load(dcProxy.ExcepcionesRDIPFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerExcepcionesRDIP, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try

            If Not IsNothing(ExcepcionesRDISelected.NuevoComentario) Or ExcepcionesRDISelected.NuevoComentario <> String.Empty Then
                Dim origen = "update"
                ErrorForma = ""
                ExcepcionesRDIAnterior = ExcepcionesRDISelected
                ExcepcionesRDISelected.Comentario = ExcepcionesRDISelected.Comentario & _
                                                    vbCrLf & _
                                                    ExcepcionesRDISelected.NuevoComentario
                ExcepcionesRDISelected.UsuarioComentario = Program.Usuario
                ExcepcionesRDISelected.NuevoComentario = Nothing
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
            Else
                A2Utilidades.Mensajes.mostrarMensaje("No se agrego ningun comentario.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
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
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                               Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
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
        If Not IsNothing(_ExcepcionesRDISelected) Then
            Editando = True
            ExcepcionesRDISelected.FechaComentario = Format(Now.Date, "dd/MM/yyyy")
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ExcepcionesRDISelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _ExcepcionesRDISelected.EntityState = EntityState.Detached Then
                    ExcepcionesRDISelected = ExcepcionesRDIAnterior
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
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no esta habilitada para el formulario.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            'If cb.Tipo <> 0 Or cb.NombreConsecutivo <> 0 Or cb.IDDocumento <> 0 Or cb.TipoIdentificacion <> 0 Or cb.NroDocumento <> 0 Or cb.Nombre <> 0 Or cb.IDBanco <> 0 Or cb.NumCheque <> 0 Or cb.Valor <> 0 Or cb.Detalle <> 0 Or Not IsNothing(cb.Documento) Or cb.Estado <> 0 Or cb.Estado <> 0 Or cb.Impresiones <> 0 Or cb.FormaPagoCE <> 0 Or cb.NroLote <> 0 Or cb.Contabilidad <> 0 Or Not IsNothing(cb.Actualizacion) Or cb.Usuario <> 0 Or cb.ParametroContable <> 0 Or cb.ImpresionFisica <> 0 Or cb.MultiCliente <> 0 Or cb.CuentaCliente <> 0 Or cb.CodComitente <> 0 Or cb.ArchivoTransferencia <> 0 Or cb.IdNumInst <> 0 Or cb.DVP <> 0 Or cb.Instruccion <> 0 Or cb.IdNroOrden <> 0 Or cb.DetalleInstruccion <> 0 Or cb.EstadoNovedadContabilidad <> 0 Or cb.eroComprobante_Contabilidad <> 0 Or cb.hadecontabilizacion_Contabilidad <> 0 Or Not IsNothing(cb.haProceso_Contabilidad) Or cb.EstadoNovedadContabilidad_Anulada <> 0 Or cb.EstadoAutomatico <> 0 Or cb.eroLote_Contabilidad <> 0 Or cb.MontoEscrito <> 0 Or cb.TipoIntermedia <> 0 Or cb.Concepto <> 0 Or cb.RecaudoDirecto <> 0 Or Not IsNothing(cb.ContabilidadEncuenta) Or cb.Sobregiro <> 0 Or cb.IdentificacionAutorizadoCheque <> 0 Or cb.NombreAutorizadoCheque <> 0 Or cb.IDTesoreria <> 0 Or Not IsNothing(cb.ContabilidadENC) Or cb.NroLoteAntENC <> 0 Or Not IsNothing(cb.ContabilidadAntENC) Or cb.IdSucursalBancaria <> 0 Or Not IsNothing(cb.Creacion) <> 0 Then 'Validar que ingresó algo en los campos de búsqueda

            ErrorForma = ""
            dcProxy.ExcepcionesRDIs.Clear()
            ExcepcionesRDIAnterior = Nothing
            IsBusy = True
            dcProxy.Load(dcProxy.ExcepcionesRDIPConsultarQuery(cb.IDOrden, cb.Clase, cb.UsuarioAdvertencia, cb.IDEspecie, IIf(cb.FechaRegistro.Year < 1900, "1799/01/01", cb.FechaRegistro), cb.IDComitente, cb.ClasificacionRiesgo, cb.PerfilInversionista, IIf(cb.FechaComentario.Year < 1900, "1799/01/01", cb.FechaComentario), Program.Usuario, Program.HashConexion), AddressOf TerminoTraerExcepcionesRDIP, "Busqueda")
            MyBase.ConfirmarBuscar()
            cb = New CamposBusquedaExcepcionesRDI
            CambioItem("cb")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        Try
            cb.FechaRegistro = Nothing
            cb.FechaComentario = Nothing
            cb.DisplayDate = Date.Now
            cb.DisplayDate2 = Date.Now
            cb.Clase = String.Empty
            cb.ClasificacionRiesgo = Nothing
            cb.IDComitente = Nothing
            cb.IDEspecie = Nothing
            cb.IDOrden = Nothing
            cb.Nombre = Nothing
            cb.UsuarioAdvertencia = Nothing
            cb.UsuarioComentario = Nothing
            cb.PerfilInversionista = Nothing
            MyBase.Buscar()
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Buscar los registros", _
                                 Me.ToString(), "Buscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub
    Public Sub llenarDiccionario()
        DicCamposTab.Add("FechaComentario", 1)
    End Sub
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaExcepcionesRDI
    Implements INotifyPropertyChanged

    Private _IDComitente As String
    <Display(Name:="Comitente")> _
    Public Property IDComitente As String
        Get
            Return _IDComitente
        End Get
        Set(ByVal value As String)
            _IDComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDComitente"))
        End Set
    End Property

    Private _Nombre As String
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

    Private _IDEspecie As String
    <Display(Name:="Especie")> _
    Public Property IDEspecie As String
        Get
            Return _IDEspecie
        End Get
        Set(ByVal value As String)
            _IDEspecie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDEspecie"))
        End Set
    End Property

    Private _IDOrden As String
    <Display(Name:="Orden")> _
    Public Property IDOrden As String
        Get
            Return _IDOrden
        End Get
        Set(ByVal value As String)
            _IDOrden = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDOrden"))
        End Set
    End Property

    Private _Clase As String
    <Display(Name:="Clase")> _
    Public Property Clase As String
        Get
            Return _Clase
        End Get
        Set(ByVal value As String)
            _Clase = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Clase"))
        End Set
    End Property

    Private _UsuarioAdvertencia As String
    <Display(Name:="Usuario Advertencia")> _
    Public Property UsuarioAdvertencia As String
        Get
            Return _UsuarioAdvertencia
        End Get
        Set(ByVal value As String)
            _UsuarioAdvertencia = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("UsuarioAdvertencia"))
        End Set
    End Property

    Private _FechaRegistro As DateTime
    <Display(Name:="Fecha Registro")> _
    <DisplayFormat(ApplyFOrmatInEditMode:=True, DataFormatString:="{0:d}", NullDisplayText:="La Fecha no es Correcta")> _
    Public Property FechaRegistro As DateTime
        Get
            Return _FechaRegistro
        End Get
        Set(ByVal value As DateTime)
            _FechaRegistro = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaRegistro"))
        End Set
    End Property

    Private _ClasificacionRiesgo As String
    <Display(Name:="Clasificación Riesgo")> _
    Public Property ClasificacionRiesgo As String
        Get
            Return _ClasificacionRiesgo
        End Get
        Set(ByVal value As String)
            _ClasificacionRiesgo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ClasificacionRiesgo"))
        End Set
    End Property

    Private _PerfilInversionista As String
    <Display(Name:="Perfil Inversionista")> _
    Public Property PerfilInversionista As String
        Get
            Return _PerfilInversionista
        End Get
        Set(ByVal value As String)
            _PerfilInversionista = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("PerfilInversionista"))
        End Set
    End Property

    Private _UsuarioComentario As String
    <Display(Name:="Usuario Comentario")> _
    Public Property UsuarioComentario As String
        Get
            Return _UsuarioComentario
        End Get
        Set(ByVal value As String)
            _UsuarioComentario = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("UsuarioComentario"))
        End Set
    End Property

    Private _FechaComentario As DateTime
    <Display(Name:="Fecha Comentario")> _
        <DisplayFormat(ApplyFOrmatInEditMode:=True, DataFormatString:="{0:d}", NullDisplayText:="La Fecha no es Correcta")> _
    Public Property FechaComentario As DateTime
        Get
            Return _FechaComentario
        End Get
        Set(ByVal value As DateTime)
            _FechaComentario = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaComentario"))
        End Set
    End Property

    <Display(Name:="Fecha Registro")> _
    Public Property DisplayDate As DateTime

    <Display(Name:="Fecha Comentario")> _
    Public Property DisplayDate2 As DateTime

    


    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class





