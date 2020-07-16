Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: InstalacionViewModel.vb
'Generado el : 04/28/2011 11:33:03
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

Public Enum tabIndexs
    General
    Clientes
    Parametros
    Reportes1
    Reportes2
    Bolsa
    OPCF
    Divisas
    Yankees
    OTC
    Tesoreria
End Enum

Public Class InstalacionViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Property cb As New CamposBusquedaInstalacio
    Private InstalacioPorDefecto As Instalacio
    Private InstalacioAnterior As Instalacio
    Dim dcProxy As MaestrosDomainContext
    Dim dcProxy1 As MaestrosDomainContext
    Dim objProxy As UtilidadesDomainContext
    Dim valor As String
    Dim nitcomisionistaanterior As String

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New MaestrosDomainContext()
            dcProxy1 = New MaestrosDomainContext()
            objProxy = New UtilidadesDomainContext()
        Else
            dcProxy = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            dcProxy1 = New MaestrosDomainContext(New System.Uri(Application.Current.Resources(Program.NOMBRE_PARAM_RUTA_SERV_MAESTROS).ToString()))
            objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
        End If

        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.InstalacionFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerInstalacion, "FiltroInicial")
                'dcProxy.Load(dcProxy.ParametrosFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerParametros, "FiltroInicial")
                objProxy.Verificaparametro("UTILIZARCUENTASTRASLADOGMF",Program.Usuario, Program.HashConexion, AddressOf TerminoTraerParametros, "UTILIZARCUENTASTRASLADOGMF")
                dcProxy1.Load(dcProxy1.TraerInstalacioPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerInstalacionPorDefecto_Completed, "Default")
                'se reutiliza funcion ubicada en tesoreria para consultar un parametro JBT
                dcProxy.VerificaParametros("NOTAS_COBRO_GMF",Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, Nothing)
                ''Obtengo la instancia del EventAggregator
                'Me._eventAggregator = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IEventAggregator)()
                ''Obtengo la instancia del Container
                'Dim container As IUnityContainer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of IUnityContainer)()
                ''Registro la instancia actual para poder obtener luego la información
                'container.RegisterInstance(Of  InstalacionViewModel)(Me)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "InstalacionViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"
    Private Sub terminotraercombocontraparte(ByVal lo As LoadOperation(Of ItemCombo))
        If Not lo.HasError Then
            Contraparteotc = dcProxy.ItemCombos.ToList
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Instalacio por defecto", _
                                          Me.ToString(), "terminotraercombocontraparte", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub

    Private Sub TerminoTraerInstalacionPorDefecto_Completed(ByVal lo As LoadOperation(Of Instalacio))
        If Not lo.HasError Then
            InstalacioPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Instalacio por defecto", _
                                             Me.ToString(), "TerminoTraerInstalacioPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerInstalacion(ByVal lo As LoadOperation(Of Instalacio))
        If Not lo.HasError Then
            ListaInstalacion = dcProxy.Instalacios
            If dcProxy.Instalacios.Count > 0 Then
                If lo.UserState = "insert" Then
                    InstalacioSelected = ListaInstalacion.Last
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    'MessageBox.Show("No se encontró ningún registro")
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    'MyBase.Buscar()
                    'MyBase.CancelarBuscar()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Instalacion", _
                                             Me.ToString(), "TerminoTraerInstalacio", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    Private Sub TerminoTraerParametros(ByVal obj As InvokeOperation(Of String))
        If Not obj.HasError Then
            Select Case obj.UserState
                Case "UTILIZARCUENTASTRASLADOGMF"
                    If obj.Value.Equals("SI") Then
                        visible = Visibility.Visible
                    Else
                        visible = Visibility.Collapsed
                    End If
            End Select

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de Parametros", _
                                             Me.ToString(), "TerminoTraerParametro", Application.Current.ToString(), Program.Maquina, obj.Error)
            obj.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

    'Tablas padres

    Private Sub Terminotraerparametro(ByVal obj As InvokeOperation(Of String))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la validacion", Me.ToString(), "Terminotraerparametro", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)

        Else
            If obj.Value = "SI" Then
                visiblecxc = Visibility.Visible
            Else
                visiblecxc = Visibility.Collapsed
            End If
        End If
    End Sub

#End Region

#Region "Propiedades"

    Private _tabIndex As Integer = 0
    Public Property tabIndex() As Integer
        Get
            Return _tabIndex
        End Get
        Set(ByVal value As Integer)
            _tabIndex = value
            'If Not IsNothing(value) Then
            '    'If value = 0 Then
            '    '    visible = Visibility.Collapsed    
            '    'End If
            'End If
            MyBase.CambioItem("tabIndex")

        End Set
    End Property


    Private _ListaInstalacion As EntitySet(Of Instalacio)
    Public Property ListaInstalacion() As EntitySet(Of Instalacio)
        Get
            Return _ListaInstalacion
        End Get
        Set(ByVal value As EntitySet(Of Instalacio))
            _ListaInstalacion = value

            MyBase.CambioItem("ListaInstalacion")
            MyBase.CambioItem("ListaInstalacionPaged")
            If Not IsNothing(value) Then
                If IsNothing(InstalacioAnterior) Then
                    InstalacioSelected = _ListaInstalacion.FirstOrDefault
                Else
                    InstalacioSelected = InstalacioAnterior
                End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaInstalacionPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaInstalacion) Then
                Dim view = New PagedCollectionView(_ListaInstalacion)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _InstalacioSelected As Instalacio
    Public Property InstalacioSelected() As Instalacio
        Get
            Return _InstalacioSelected
        End Get
        Set(ByVal value As Instalacio)
            _InstalacioSelected = value
            If Not IsNothing(value) Then
                dcProxy.ItemCombos.Clear()
                Contraparteotc.Clear()
                dcProxy.Load(dcProxy.LLenarcontraparteotcQuery(value.NitComisionista,Program.Usuario, Program.HashConexion), AddressOf terminotraercombocontraparte, Nothing)
            End If
            MyBase.CambioItem("InstalacioSelected")
        End Set
    End Property

    Private _visible As Visibility = Visibility.Collapsed
    Public Property visible As Visibility
        Get
            Return _visible
        End Get
        Set(ByVal value As Visibility)
            _visible = value
            MyBase.CambioItem("visible")
        End Set
    End Property

    Private _Contraparteotc As New List(Of ItemCombo)
    Public Property Contraparteotc As List(Of ItemCombo)
        Get
            Return _Contraparteotc
        End Get
        Set(value As List(Of ItemCombo))
            _Contraparteotc = value
            MyBase.CambioItem("Contraparteotc")
        End Set
    End Property

    Private _visiblecxc As Visibility = Visibility.Collapsed
    Public Property visiblecxc As Visibility
        Get
            Return _visiblecxc
        End Get
        Set(ByVal value As Visibility)
            _visiblecxc = value
            MyBase.CambioItem("visiblecxc")
        End Set
    End Property

#End Region

#Region "Métodos"
    Public Overrides Sub NuevoRegistro()
        Try
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            'MessageBox.Show("Esta funcionalidad no está disponible para este maestro.", "Funcionalidad Inhabilitada", MessageBoxButton.OK)
            '          Dim NewInstalacio As New Instalacio
            '	'TODO: Verificar cuales son los campos que deben inicializarse
            'NewInstalacio.IDComisionista = InstalacioPorDefecto.IDComisionista
            'NewInstalacio.IDSucComisionista = InstalacioPorDefecto.IDSucComisionista
            'NewInstalacio.IDBolsa = InstalacioPorDefecto.IDBolsa
            'NewInstalacio.IdPoblacion = InstalacioPorDefecto.IdPoblacion
            'NewInstalacio.Linea1 = InstalacioPorDefecto.Linea1
            'NewInstalacio.Enca11 = InstalacioPorDefecto.Enca11
            'NewInstalacio.Enca21 = InstalacioPorDefecto.Enca21
            'NewInstalacio.Enca31 = InstalacioPorDefecto.Enca31
            'NewInstalacio.Enca41 = InstalacioPorDefecto.Enca41
            'NewInstalacio.Enca12 = InstalacioPorDefecto.Enca12
            'NewInstalacio.Enca22 = InstalacioPorDefecto.Enca22
            'NewInstalacio.Enca32 = InstalacioPorDefecto.Enca32
            'NewInstalacio.Enca42 = InstalacioPorDefecto.Enca42
            'NewInstalacio.Observacion1 = InstalacioPorDefecto.Observacion1
            'NewInstalacio.Observacion2 = InstalacioPorDefecto.Observacion2
            'NewInstalacio.ClientesAutomatico = InstalacioPorDefecto.ClientesAutomatico
            'NewInstalacio.ClientesCedula = InstalacioPorDefecto.ClientesCedula
            'NewInstalacio.EncaFac = InstalacioPorDefecto.EncaFac
            'NewInstalacio.EncaFacBca = InstalacioPorDefecto.EncaFacBca
            'NewInstalacio.EncaTit = InstalacioPorDefecto.EncaTit
            'NewInstalacio.EncaEgr = InstalacioPorDefecto.EncaEgr
            'NewInstalacio.EncaCaj = InstalacioPorDefecto.EncaCaj
            'NewInstalacio.EncaNot = InstalacioPorDefecto.EncaNot
            'NewInstalacio.EncaExt = InstalacioPorDefecto.EncaExt
            'NewInstalacio.RCLineas = InstalacioPorDefecto.RCLineas
            'NewInstalacio.CELineas = InstalacioPorDefecto.CELineas
            'NewInstalacio.NCLineas = InstalacioPorDefecto.NCLineas
            'NewInstalacio.TITLineas = InstalacioPorDefecto.TITLineas
            'NewInstalacio.FacLineas = InstalacioPorDefecto.FacLineas
            'NewInstalacio.FacBcaLineas = InstalacioPorDefecto.FacBcaLineas
            'NewInstalacio.EXTLineas = InstalacioPorDefecto.EXTLineas
            'NewInstalacio.Receptores = InstalacioPorDefecto.Receptores
            'NewInstalacio.FechaOrden = InstalacioPorDefecto.FechaOrden
            'NewInstalacio.Usuario = Program.Usuario
            'NewInstalacio.ValSobregiroCE = InstalacioPorDefecto.ValSobregiroCE
            'NewInstalacio.Resolucion = InstalacioPorDefecto.Resolucion
            'NewInstalacio.IvaComision = InstalacioPorDefecto.IvaComision
            'NewInstalacio.NombreCuenta = InstalacioPorDefecto.NombreCuenta
            'NewInstalacio.SerBolsaFijo = InstalacioPorDefecto.SerBolsaFijo
            'NewInstalacio.SerBolsaFijoAcciones = InstalacioPorDefecto.SerBolsaFijoAcciones
            'NewInstalacio.TopeSerBolsaFijoAcciones = InstalacioPorDefecto.TopeSerBolsaFijoAcciones
            'NewInstalacio.EncaCus = InstalacioPorDefecto.EncaCus
            'NewInstalacio.CusLineas = InstalacioPorDefecto.CusLineas
            'NewInstalacio.UsuarioEntregas = InstalacioPorDefecto.UsuarioEntregas
            'NewInstalacio.UsuarioRecibido = InstalacioPorDefecto.UsuarioRecibido
            'NewInstalacio.UsuarioCustodia = InstalacioPorDefecto.UsuarioCustodia
            'NewInstalacio.UsuarioSobrantes = InstalacioPorDefecto.UsuarioSobrantes
            'NewInstalacio.IVA = InstalacioPorDefecto.IVA
            'NewInstalacio.RteFuente = InstalacioPorDefecto.RteFuente
            'NewInstalacio.NitComisionista = InstalacioPorDefecto.NitComisionista
            'NewInstalacio.Servidor = InstalacioPorDefecto.Servidor
            'NewInstalacio.BaseDatos = InstalacioPorDefecto.BaseDatos
            'NewInstalacio.Owner = InstalacioPorDefecto.Owner
            'NewInstalacio.ServidorBus = InstalacioPorDefecto.ServidorBus
            'NewInstalacio.BaseDatosBus = InstalacioPorDefecto.BaseDatosBus
            'NewInstalacio.OwnerBus = InstalacioPorDefecto.OwnerBus
            'NewInstalacio.Compania = InstalacioPorDefecto.Compania
            'NewInstalacio.DepositoExtranjero = InstalacioPorDefecto.DepositoExtranjero
            'NewInstalacio.CustodioLocal = InstalacioPorDefecto.CustodioLocal
            'NewInstalacio.IdContraparteOTC = InstalacioPorDefecto.IdContraparteOTC
            'NewInstalacio.ValorContrato = InstalacioPorDefecto.ValorContrato
            'NewInstalacio.CodigoIMC = InstalacioPorDefecto.CodigoIMC
            'NewInstalacio.ReteIva = InstalacioPorDefecto.ReteIva
            'NewInstalacio.ValorInicial = InstalacioPorDefecto.ValorInicial
            'NewInstalacio.GMS = InstalacioPorDefecto.GMS
            'NewInstalacio.CargarReceptorCliente = InstalacioPorDefecto.CargarReceptorCliente
            'NewInstalacio.Cierre = InstalacioPorDefecto.Cierre
            'NewInstalacio.UltimaVersion = InstalacioPorDefecto.UltimaVersion
            'NewInstalacio.TasaInicial = InstalacioPorDefecto.TasaInicial
            'NewInstalacio.AplazarOTC = InstalacioPorDefecto.AplazarOTC
            'NewInstalacio.CuentasBancarias = InstalacioPorDefecto.CuentasBancarias
            'NewInstalacio.RepresentanteLegal = InstalacioPorDefecto.RepresentanteLegal
            'NewInstalacio.FechaLimite = InstalacioPorDefecto.FechaLimite
            'NewInstalacio.Actualizacion = InstalacioPorDefecto.Actualizacion
            'NewInstalacio.Usuario = Program.Usuario
            'NewInstalacio.DiaSemana = InstalacioPorDefecto.DiaSemana
            'NewInstalacio.PorcentajeGarantia = InstalacioPorDefecto.PorcentajeGarantia
            'NewInstalacio.TarifaRteFuente = InstalacioPorDefecto.TarifaRteFuente
            'NewInstalacio.ImpDocTesoreria = InstalacioPorDefecto.ImpDocTesoreria
            'NewInstalacio.TipoEntidad = InstalacioPorDefecto.TipoEntidad
            'NewInstalacio.CodigoEntidad = InstalacioPorDefecto.CodigoEntidad
            'NewInstalacio.RteComision = InstalacioPorDefecto.RteComision
            'NewInstalacio.RteICA = InstalacioPorDefecto.RteICA
            'NewInstalacio.CodigoEntidadUIAF = InstalacioPorDefecto.CodigoEntidadUIAF
            'NewInstalacio.TipoEntidadUIAF = InstalacioPorDefecto.TipoEntidadUIAF
            'NewInstalacio.ValidaCuentaSuperVal = InstalacioPorDefecto.ValidaCuentaSuperVal
            'NewInstalacio.ValSobregiroNC = InstalacioPorDefecto.ValSobregiroNC
            'NewInstalacio.Tipo = InstalacioPorDefecto.Tipo
            'NewInstalacio.CtaContable = InstalacioPorDefecto.CtaContable
            'NewInstalacio.CCosto = InstalacioPorDefecto.CCosto
            'NewInstalacio.CtaContableContraparte = InstalacioPorDefecto.CtaContableContraparte
            'NewInstalacio.CCostoContraparte = InstalacioPorDefecto.CCostoContraparte
            'NewInstalacio.ReporteExtractoClientePedirRangos = InstalacioPorDefecto.ReporteExtractoClientePedirRangos
            'NewInstalacio.CtaContableClientes = InstalacioPorDefecto.CtaContableClientes
            'NewInstalacio.URL = InstalacioPorDefecto.URL
            'NewInstalacio.Path = InstalacioPorDefecto.Path
            'NewInstalacio.Ordenantes = InstalacioPorDefecto.Ordenantes
            'NewInstalacio.ReceptorSuc = InstalacioPorDefecto.ReceptorSuc
            'NewInstalacio.ClientesAgrupados = InstalacioPorDefecto.ClientesAgrupados
            'NewInstalacio.PathActualiza = InstalacioPorDefecto.PathActualiza
            'NewInstalacio.DatosFinancieros = InstalacioPorDefecto.DatosFinancieros
            'NewInstalacio.ConceptoDetalleTesoreriaManual = InstalacioPorDefecto.ConceptoDetalleTesoreriaManual
            'NewInstalacio.NroUsu = InstalacioPorDefecto.NroUsu
            'NewInstalacio.ServidorNacional = InstalacioPorDefecto.ServidorNacional
            'NewInstalacio.Compania = InstalacioPorDefecto.Compania
            'NewInstalacio.CompaniaM = InstalacioPorDefecto.CompaniaM
            'NewInstalacio.Multicuenta = InstalacioPorDefecto.Multicuenta
            'NewInstalacio.MaximoValor = InstalacioPorDefecto.MaximoValor
            'NewInstalacio.DefensorCliente = InstalacioPorDefecto.DefensorCliente
            'NewInstalacio.UrlReportesBus = InstalacioPorDefecto.UrlReportesBus
            'NewInstalacio.RutaReportesBus = InstalacioPorDefecto.RutaReportesBus
            'NewInstalacio.GMFInferior = InstalacioPorDefecto.GMFInferior
            'NewInstalacio.CtaContableContraparteNotasCxC = InstalacioPorDefecto.CtaContableContraparteNotasCxC
            'NewInstalacio.tipoNotasCxC = InstalacioPorDefecto.tipoNotasCxC
            'NewInstalacio.IDInstalacion = InstalacioPorDefecto.IDInstalacion
            '      InstalacioAnterior = InstalacioSelected
            '      InstalacioSelected = NewInstalacio
            '      MyBase.CambioItem("Instalacion")
            '      Editando = True
            '      MyBase.CambioItem("Editando")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            dcProxy.Instalacios.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.InstalacionFiltrarQuery(TextoFiltroSeguro,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerInstalacion, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.InstalacionFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerInstalacion, "Filtrar")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        'MessageBox.Show("Esta funcionalidad no está disponible para este maestro.", "Funcionalidad Inhabilitada", MessageBoxButton.OK)
        'MyBase.Buscar()
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            Dim origen = "update"
            ErrorForma = ""
            InstalacioAnterior = InstalacioSelected
            If Not ListaInstalacion.Contains(InstalacioSelected) Then
                origen = "insert"
                ListaInstalacion.Add(InstalacioSelected)
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
        If Not IsNothing(_InstalacioSelected) Then
            Editando = True
            nitcomisionistaanterior = InstalacioSelected.NitComisionista
            'JFSB 20171214 Se envía el usuario cuando se edita el regitro
            InstalacioSelected.strUsuario = Program.Usuario
        Else
            MyBase.RetornarValorEdicionNavegacion()
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_InstalacioSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _InstalacioSelected.EntityState = EntityState.Detached Then
                    InstalacioSelected = InstalacioAnterior
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
            'MessageBox.Show("Esta funcionalidad no está disponible para este maestro.", "Funcionalidad Inhabilitada", MessageBoxButton.OK)
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Esta funcionalidad no está disponible para este maestro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            'If Not IsNothing(_InstalacioSelected) Then
            '        dcProxy.Instalacios.Remove(_InstalacioSelected)
            '    IsBusy = True
            '    dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, Nothing)
            'End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub llenarcontraparteotc()
        If Not IsNothing(InstalacioSelected.NitComisionista) And InstalacioSelected.NitComisionista <> 0 And InstalacioSelected.NitComisionista <> nitcomisionistaanterior Then
            dcProxy.ItemCombos.Clear()
            Contraparteotc.Clear()
            dcProxy.Load(dcProxy.LLenarcontraparteotcQuery(InstalacioSelected.NitComisionista,Program.Usuario, Program.HashConexion), AddressOf terminotraercombocontraparte, Nothing)
            nitcomisionistaanterior = InstalacioSelected.NitComisionista
        End If

    End Sub

    Private Sub _InstalacioSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _InstalacioSelected.PropertyChanged
        'If e.PropertyName.Equals("NitComisionista") Then
        '    If Not IsNothing(InstalacioSelected.NitComisionista) And InstalacioSelected.NitComisionista <> 0 Then
        '        dcProxy.ItemCombos.Clear()
        '        Contraparteotc.Clear()
        '        dcProxy.Load(dcProxy.LLenarcontraparteotcQuery(InstalacioSelected.NitComisionista,Program.Usuario, Program.HashConexion), AddressOf terminotraercombocontraparte, Nothing)
        '    End If
        'End If
        If Editando Then
            If e.PropertyName.Equals("ClientesAutomatico") Then
                If _InstalacioSelected.ClientesAutomatico Then
                    _InstalacioSelected.ClientesCedula = False
                End If
            ElseIf e.PropertyName.Equals("ClientesCedula") Then
                If _InstalacioSelected.ClientesCedula Then
                    _InstalacioSelected.ClientesAutomatico = False
                End If
            End If
        End If

    End Sub

#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposBusquedaInstalacio
End Class




