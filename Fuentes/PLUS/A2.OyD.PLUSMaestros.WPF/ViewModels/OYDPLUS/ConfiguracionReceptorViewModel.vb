Imports Telerik.Windows.Controls

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSMaestros
Imports OpenRiaServices.DomainServices.Client

Public Class ConfiguracionReceptorViewModel
    Inherits A2ControlMenu.A2ViewModel
    Private CertificacionesXReceptoPorDefecto As CertificacionesXRecepto
    Private ConceptosXReceptoPorDefecto As ConceptosXRecepto
    Private ConfiguracionesAdicionalesReceptoPorDefecto As ConfiguracionesAdicionalesRecepto
    Private ParametrosReceptoPorDefecto As ParametrosRecepto
    Private ReceptoresClientesAutorizadoPorDefecto As ReceptoresClientesAutorizado
    Private TipoNegocioXReceptoPorDefecto As TipoNegocioXRecepto
    Private TipoProductoXReceptoPorDefecto As TipoProductoXRecepto
    Private ConfiguracionesAdicionalesReceptoAnterior As ConfiguracionesAdicionalesRecepto

    Dim dcProxy As OyDPLUSMaestrosDomainContext
    Dim dcProxy1 As OyDPLUSMaestrosDomainContext
    Dim dcProxyUtils As UtilidadesDomainContext

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New OyDPLUSMaestrosDomainContext()
                dcProxy1 = New OyDPLUSMaestrosDomainContext()
                dcProxyUtils = New UtilidadesDomainContext()
            Else
                dcProxy = New OyDPLUSMaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxy1 = New OyDPLUSMaestrosDomainContext(New System.Uri(Program.RutaServicioNegocio))
                dcProxyUtils = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            End If

            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.ConfiguracionesAdicionalesReceptorFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConfiguracionesAdicionalesReceptor, "")

                dcProxy1.Load(dcProxy1.TipoNegocioFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoNegocio, "")
                dcProxy1.Load(dcProxy1.ConceptosTesoreriaFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConceptosTesoreria, "")
                dcProxy1.Load(dcProxy1.TraerCertificacionesXReceptoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCertificacionesXReceptorPorDefecto_Completed, "Default")
                dcProxy1.Load(dcProxy1.TraerConceptosXReceptoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConceptosXReceptorPorDefecto_Completed, "Default")
                dcProxy1.Load(dcProxy1.TraerParametrosReceptoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerParametrosReceptorPorDefecto_Completed, "Default")
                dcProxy1.Load(dcProxy1.TraerReceptoresClientesAutorizadoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptoresClientesAutorizadosPorDefecto_Completed, "Default")
                dcProxy1.Load(dcProxy1.TraerTipoNegocioXReceptoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoNegocioXReceptorPorDefecto_Completed, "Default")
                dcProxy1.Load(dcProxy1.TraerTipoProductoXReceptoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoProductoXReceptorPorDefecto_Completed, "Default")

                dcProxyUtils.Load(dcProxyUtils.cargarCombosEspecificosQuery("PARAMETROSRECEPTOR", Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombosEspecificos, String.Empty)

                'ListaTipoOrden.Add(New ClaseTipoOrden With {.Codigo = "A", .Descripcion = "Ambas"})
                'ListaTipoOrden.Add(New ClaseTipoOrden With {.Codigo = "D", .Descripcion = "Directa"})
                'ListaTipoOrden.Add(New ClaseTipoOrden With {.Codigo = "I", .Descripcion = "Indirecta"})

                'ListaTipoOrdenDefecto.Add(New ClaseTipoOrden With {.Codigo = "I", .Descripcion = "Indirecta"})
                'ListaTipoOrdenDefecto.Add(New ClaseTipoOrden With {.Codigo = "D", .Descripcion = "Directa"})
                'ListaTipoOrdenDefecto.Add(New ClaseTipoOrden With {.Codigo = "", .Descripcion = "Ninguna"})
                dcProxy1.Load(dcProxy1.TraerConfiguracionesAdicionalesReceptoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConfiguracionesAdicionalesReceptorPorDefecto_Completed, "Default")

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ConfiguracionReceptorViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerConfiguracionesAdicionalesReceptorPorDefecto_Completed(ByVal lo As LoadOperation(Of ConfiguracionesAdicionalesRecepto))
        Try
            If Not lo.HasError Then
                ConfiguracionesAdicionalesReceptoPorDefecto = lo.Entities.FirstOrDefault
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ConfiguracionesAdicionalesRecepto por defecto",
                                                 Me.ToString(), "TerminoTraerConfiguracionesAdicionalesReceptoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ConfiguracionesAdicionalesRecepto por defecto",
                                                 Me.ToString(), "TerminoTraerConfiguracionesAdicionalesReceptoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, ex)
            lo.MarkErrorAsHandled()   '????
        End Try
    End Sub

    Private Sub TerminoTraerConfiguracionesAdicionalesReceptor(ByVal lo As LoadOperation(Of ConfiguracionesAdicionalesRecepto))
        Try
            If Not lo.HasError Then
                ListaConfiguracionesAdicionalesReceptor = dcProxy.ConfiguracionesAdicionalesReceptos
                If ListaConfiguracionesAdicionalesReceptor.Count = 0 Then
                    If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                        A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ConfiguracionesAdicionalesReceptor",
                                                 Me.ToString(), "TerminoTraerConfiguracionesAdicionalesRecepto", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ConfiguracionesAdicionalesReceptor",
                                                 Me.ToString(), "TerminoTraerConfiguracionesAdicionalesRecepto", Application.Current.ToString(), Program.Maquina, ex)
            lo.MarkErrorAsHandled()   '????
        End Try
        IsBusy = False
    End Sub

    Private Sub TerminoTraerTipoNegocio(ByVal lo As LoadOperation(Of TipoNegocio))
        Try
            If Not lo.HasError Then
                ListaTipoNegocioCompleta = dcProxy1.TipoNegocios.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de TipoNegocio",
                                                 Me.ToString(), "TerminoTraerTipoNegoci", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de TipoNegocio",
                                                 Me.ToString(), "TerminoTraerTipoNegoci", Application.Current.ToString(), Program.Maquina, ex)
            lo.MarkErrorAsHandled()   '????
        End Try
    End Sub

    Private Sub TerminoTraerConceptosTesoreria(ByVal lo As LoadOperation(Of ConceptosTesoreri))
        Try
            If Not lo.HasError Then
                ListaConceptos = dcProxy1.ConceptosTesoreris.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de conceptos",
                                                 Me.ToString(), "TerminoTraerConceptosTesoreria", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de conceptos",
                                                 Me.ToString(), "TerminoTraerConceptosTesoreria", Application.Current.ToString(), Program.Maquina, ex)
            lo.MarkErrorAsHandled()   '????
        End Try
    End Sub

    Private Sub TerminoConsultarCombosEspecificos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            If lo.HasError = False Then
                Dim objListaTopicos As New List(Of ClaseTopicos)
                Dim objListaRetornosCompleta As New List(Of ClaseRetornos)

                If Not IsNothing(ListaTopicosParametros) Then
                    ListaTopicosParametros.Clear()
                End If

                If Not IsNothing(ListaRetornosCompleta) Then
                    ListaRetornosCompleta.Clear()
                End If

                For Each li In lo.Entities.ToList
                    If li.Categoria = "TOPICOS" Then
                        objListaTopicos.Add(New ClaseTopicos With {.Codigo = li.ID, .Descripcion = li.Descripcion})
                    Else
                        objListaRetornosCompleta.Add(New ClaseRetornos With {.Codigo = li.ID, .Descripcion = li.Descripcion, .Topico = li.Categoria, .Prioridad = 0, .Seleccionado = False})
                    End If
                Next

                ListaTopicosParametros = objListaTopicos
                ListaRetornosCompleta = objListaRetornosCompleta
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los combos especificos.",
                                             Me.ToString(), "TerminoConsultarCombosEspecificos", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los combos especificos.",
                                             Me.ToString(), "TerminoConsultarCombosEspecificos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


#End Region

#Region "Propiedades"

    Private _ListaConfiguracionesAdicionalesReceptor As EntitySet(Of ConfiguracionesAdicionalesRecepto)
    Public Property ListaConfiguracionesAdicionalesReceptor() As EntitySet(Of ConfiguracionesAdicionalesRecepto)
        Get
            Return _ListaConfiguracionesAdicionalesReceptor
        End Get
        Set(ByVal value As EntitySet(Of ConfiguracionesAdicionalesRecepto))
            _ListaConfiguracionesAdicionalesReceptor = value
            MyBase.CambioItem("ListaConfiguracionesAdicionalesReceptor")
            MyBase.CambioItem("ListaConfiguracionesAdicionalesReceptorPaged")

            If Not IsNothing(_ListaConfiguracionesAdicionalesReceptor) Then
                If _ListaConfiguracionesAdicionalesReceptor.Count > 0 Then
                    If Not IsNothing(_ConfiguracionesAdicionalesReceptoSelected) Then
                        If _ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor <> _ListaConfiguracionesAdicionalesReceptor.FirstOrDefault.CodigoReceptor Then
                            ConfiguracionesAdicionalesReceptoSelected = _ListaConfiguracionesAdicionalesReceptor.FirstOrDefault
                        End If
                    End If
                Else
                    ConfiguracionesAdicionalesReceptoSelected = Nothing
                End If
            Else
                ConfiguracionesAdicionalesReceptoSelected = Nothing
            End If
        End Set
    End Property

    Public ReadOnly Property ListaConfiguracionesAdicionalesReceptorPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaConfiguracionesAdicionalesReceptor) Then
                Dim view = New PagedCollectionView(_ListaConfiguracionesAdicionalesReceptor)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private Sub _ConfiguracionesAdicionalesReceptoSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ConfiguracionesAdicionalesReceptoSelected.PropertyChanged
        Try
            If Editando Then
                If e.PropertyName.Equals("TipoOrden") Then

                    If _ConfiguracionesAdicionalesReceptoSelected.TipoOrden = "I" Then
                        _ConfiguracionesAdicionalesReceptoSelected.TipoOrdenDefecto = "I"
                        HabilitarComboConfiguracion = False
                    ElseIf _ConfiguracionesAdicionalesReceptoSelected.TipoOrden = "D" Then

                        _ConfiguracionesAdicionalesReceptoSelected.TipoOrdenDefecto = "D"
                        HabilitarComboConfiguracion = False
                    Else

                        HabilitarComboConfiguracion = True




                    End If

                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar el cambio de propiedad.",
             Me.ToString(), "_RetornoSeleccionado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub






    Private WithEvents _ConfiguracionesAdicionalesReceptoSelected As ConfiguracionesAdicionalesRecepto
    Public Property ConfiguracionesAdicionalesReceptoSelected() As ConfiguracionesAdicionalesRecepto
        Get
            Return _ConfiguracionesAdicionalesReceptoSelected
        End Get
        Set(ByVal value As ConfiguracionesAdicionalesRecepto)
            _ConfiguracionesAdicionalesReceptoSelected = value

            If Not IsNothing(_ConfiguracionesAdicionalesReceptoSelected) Then
                'Consulta los parametros del receptor
                TopicoSeleccionado = Nothing 'JBT20140327 
                ConsultarParametros_Receptor(_ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor)

                'Consulta las certificaciones del receptor
                ConsultarCertificaciones_Receptor(_ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor)

                'Consulta los conceptos del receptor
                ConsultarConceptos_Receptor(_ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor)

                'Consultar los clientes autorizados receptor
                ConsultarClientesAutorizados_Receptor(_ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor)

                'Consultar los Tipos de negocio receptor
                ConsultarTipoNegocio_Receptor(_ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor)

                'Consultar los tipos de producto receptor
                ConsultarTipoProducto_Receptor(_ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor)

                TabSeleccionado = 1

            End If

            MyBase.CambioItem("ConfiguracionesAdicionalesReceptoSelected")
        End Set
    End Property

    Private _cb As CamposBusquedaConfiguracionReceptor
    Public Property cb() As CamposBusquedaConfiguracionReceptor
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposBusquedaConfiguracionReceptor)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property

    Private _ListaTipoNegocioCompleta As List(Of TipoNegocio)
    Public Property ListaTipoNegocioCompleta() As List(Of TipoNegocio)
        Get
            Return _ListaTipoNegocioCompleta
        End Get
        Set(ByVal value As List(Of TipoNegocio))
            _ListaTipoNegocioCompleta = value
            MyBase.CambioItem("ListaTipoNegocioCompleta")
        End Set
    End Property




    Private _HabilitarComboConfiguracion As Boolean = False
    Public Property HabilitarComboConfiguracion() As Boolean
        Get
            Return _HabilitarComboConfiguracion
        End Get
        Set(ByVal value As Boolean)
            _HabilitarComboConfiguracion = value
            MyBase.CambioItem("HabilitarComboConfiguracion")
        End Set
    End Property


    'Private _ListaTipoOrden As List(Of ClaseTipoOrden) = New List(Of ClaseTipoOrden)
    'Public Property ListaTipoOrden() As List(Of ClaseTipoOrden)
    '    Get
    '        Return _ListaTipoOrden
    '    End Get
    '    Set(ByVal value As List(Of ClaseTipoOrden))
    '        _ListaTipoOrden = value
    '        MyBase.CambioItem("ListaTipoOrden")
    '    End Set
    'End Property

    'Private _ListaTipoOrdenDefecto As List(Of ClaseTipoOrden) = New List(Of ClaseTipoOrden)
    'Public Property ListaTipoOrdenDefecto() As List(Of ClaseTipoOrden)
    '    Get
    '        Return _ListaTipoOrdenDefecto
    '    End Get
    '    Set(ByVal value As List(Of ClaseTipoOrden))
    '        _ListaTipoOrdenDefecto = value
    '        MyBase.CambioItem("ListaTipoOrdenDefecto")
    '    End Set
    'End Property

    Private _ListaConceptos As List(Of ConceptosTesoreri)
    Public Property ListaConceptos() As List(Of ConceptosTesoreri)
        Get
            Return _ListaConceptos
        End Get
        Set(ByVal value As List(Of ConceptosTesoreri))
            _ListaConceptos = value
            MyBase.CambioItem("ListaConceptos")
        End Set
    End Property

    Private _TabSeleccionado As Integer = 0
    Public Property TabSeleccionado() As Integer
        Get
            Return _TabSeleccionado
        End Get
        Set(ByVal value As Integer)
            _TabSeleccionado = value
            MyBase.CambioItem("TabSeleccionado")
        End Set
    End Property

    Private _BorrarCliente As Boolean = False
    Public Property BorrarCliente() As Boolean
        Get
            Return _BorrarCliente
        End Get
        Set(ByVal value As Boolean)
            _BorrarCliente = value
            MyBase.CambioItem("BorrarCliente")
        End Set
    End Property

    Private _BorrarEspecie As Boolean = False
    Public Property BorrarEspecie() As Boolean
        Get
            Return _BorrarEspecie
        End Get
        Set(ByVal value As Boolean)
            _BorrarEspecie = value
            MyBase.CambioItem("BorrarEspecie")
        End Set
    End Property

    Private _BorrarReceptor As Boolean = False
    Public Property BorrarReceptor() As Boolean
        Get
            Return _BorrarReceptor
        End Get
        Set(ByVal value As Boolean)
            _BorrarReceptor = value
            MyBase.CambioItem("BorrarReceptor")
        End Set
    End Property

    Private _FechaActual As DateTime = Now
    Public Property FechaActual() As DateTime
        Get
            Return _FechaActual
        End Get
        Set(ByVal value As DateTime)
            _FechaActual = value
            MyBase.CambioItem("FechaActual")
        End Set
    End Property

    Private _EditandoDetalle As Boolean = True
    Public Property EditandoDetalle() As Boolean
        Get
            Return _EditandoDetalle
        End Get
        Set(ByVal value As Boolean)
            _EditandoDetalle = value
            MyBase.CambioItem("EditandoDetalle")
        End Set
    End Property

#End Region

#Region "Métodos"

    Public Overrides Sub NuevoRegistro()
        Try
            'Dim NewConfiguracionesAdicionalesRecepto As New ConfiguracionesAdicionalesRecepto
            ''TODO: Verificar cuales son los campos que deben inicializarse
            'NewConfiguracionesAdicionalesRecepto.ID = ConfiguracionesAdicionalesReceptoPorDefecto.ID
            'NewConfiguracionesAdicionalesRecepto.CodigoReceptor = ConfiguracionesAdicionalesReceptoPorDefecto.CodigoReceptor
            'NewConfiguracionesAdicionalesRecepto.NombreReceptor = ConfiguracionesAdicionalesReceptoPorDefecto.NombreReceptor
            'NewConfiguracionesAdicionalesRecepto.TipoOrden = ConfiguracionesAdicionalesReceptoPorDefecto.TipoOrden
            'NewConfiguracionesAdicionalesRecepto.ExtensionDefecto = ConfiguracionesAdicionalesReceptoPorDefecto.ExtensionDefecto
            'NewConfiguracionesAdicionalesRecepto.TipoOrdenDefecto = ConfiguracionesAdicionalesReceptoPorDefecto.TipoOrdenDefecto
            'NewConfiguracionesAdicionalesRecepto.Usuario = Program.Usuario
            'NewConfiguracionesAdicionalesRecepto.EnrutaBus = ConfiguracionesAdicionalesReceptoPorDefecto.EnrutaBus
            'ConfiguracionesAdicionalesReceptoAnterior = ConfiguracionesAdicionalesReceptoSelected
            'ConfiguracionesAdicionalesReceptoSelected = NewConfiguracionesAdicionalesRecepto
            'MyBase.CambioItem("ConfiguracionesAdicionalesReceptor")
            'Editando = True
            'MyBase.CambioItem("Editando")

            mostrarMensaje("Esta funcionalidad se encuentra deshabilitada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Filtrar()
        Try
            If Not IsNothing(dcProxy.ConfiguracionesAdicionalesReceptos) Then
                dcProxy.ConfiguracionesAdicionalesReceptos.Clear()
            End If
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.ConfiguracionesAdicionalesReceptorFiltrarQuery(FiltroVM, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConfiguracionesAdicionalesReceptor, "FiltroVM")
            Else
                dcProxy.Load(dcProxy.ConfiguracionesAdicionalesReceptorFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConfiguracionesAdicionalesReceptor, "FiltroVM")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro",
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        PrepararNuevaBusqueda()
        MyBase.Buscar()
    End Sub

    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.Codigo <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""

                If Not IsNothing(dcProxy.ConfiguracionesAdicionalesReceptos) Then
                    dcProxy.ConfiguracionesAdicionalesReceptos.Clear()
                End If
                IsBusy = True
                'DescripcionFiltroVM = " Codigo = " &  cb.Codigo.ToString()    'Dic202011 quitar
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(cb.Codigo)
                dcProxy.Load(dcProxy.ConfiguracionesAdicionalesReceptorConsultarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConfiguracionesAdicionalesReceptor, "Busqueda")
                MyBase.ConfirmarBuscar()
                PrepararNuevaBusqueda()
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro",
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub ActualizarRegistro()
        Try
            Dim origen = "update"
            ErrorForma = ""

            If Not IsNothing(ListaRetornos) Then
                If ListaRetornos.Count > 0 Then
                    Dim objTopico As String = String.Empty

                    For Each li In ListaRetornos
                        'If li.Topico <> objTopico Then
                        '    If ListaParametrosReceptor.Where(Function(i) i.Topico = li.Topico).Count > 0 Then
                        '        objTopico = li.Topico
                        '        Dim objListaTopicos As New List(Of ParametrosRecepto)
                        '        objListaTopicos = ListaParametrosReceptor.Where(Function(i) i.Topico = li.Topico).ToList
                        '        For Each lo In objListaTopicos
                        '            ListaParametrosReceptor.Remove(lo)
                        '        Next
                        '    End If
                        'End If

                        If li.Seleccionado Then
                            If ListaParametrosReceptor.Where(Function(I) I.Topico = li.Topico And I.Valor = li.Codigo).Count > 0 Then
                                ListaParametrosReceptor.Where(Function(I) I.Topico = li.Topico And I.Valor = li.Codigo).First.Prioridad = li.Prioridad
                                ListaParametrosReceptor.Where(Function(I) I.Topico = li.Topico And I.Valor = li.Codigo).First.Usuario = Program.Usuario
                            Else
                                Dim objParametro As New ParametrosRecepto
                                objParametro.ID = li.ID
                                objParametro.CodigoReceptor = _ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor
                                objParametro.Prioridad = li.Prioridad
                                objParametro.Topico = li.Topico
                                objParametro.Usuario = Program.Usuario
                                objParametro.Valor = li.Codigo

                                ListaParametrosReceptor.Add(objParametro)
                            End If
                        End If
                    Next
                End If
            End If

            Dim logGuardar As Boolean = True
            Dim logGuardarFecha As Boolean = True

            Dim strMensajeTipoNegocio As String = "No se puede tener tipos de negocio repetidos."
            For Each li In ListaTipoNegocioXReceptor
                If String.IsNullOrEmpty(li.CodigoTipoNegocio) Then
                    strMensajeTipoNegocio = "Debe seleccionar todos los tipos de negocio."
                    logGuardar = False
                    Exit For
                End If
                If ListaTipoNegocioXReceptor.Where(Function(i) i.CodigoTipoNegocio = li.CodigoTipoNegocio).Count > 1 Then
                    logGuardar = False
                    Exit For
                End If
            Next

            If logGuardar = False Then
                mostrarMensaje(strMensajeTipoNegocio, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionado = 0
                Exit Sub
            End If

            Dim strMenasajeTipoProducto = "No se puede tener tipos de producto repetidos."

            For Each li In ListaTipoProductoXReceptor
                If String.IsNullOrEmpty(li.TipoProducto) Then
                    strMenasajeTipoProducto = "Debe de seleccionar todos los tipos de producto."
                    logGuardar = False
                    Exit For
                End If
                If ListaTipoProductoXReceptor.Where(Function(i) i.TipoProducto = li.TipoProducto).Count > 1 Then
                    logGuardar = False
                    Exit For
                End If
            Next

            If logGuardar = False Then
                mostrarMensaje(strMenasajeTipoProducto, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionado = 1
                Exit Sub
            End If

            Dim strMenasajeCertificacion = "No se puede tener certificaciones repetidas."

            For Each li In ListaCertificacionesXReceptor
                If String.IsNullOrEmpty(li.CodigoCertificacion) Then
                    strMenasajeCertificacion = "Debe de seleccionar todas las certificaciones."
                    logGuardar = False
                    Exit For
                End If
                If ListaCertificacionesXReceptor.Where(Function(i) i.CodigoCertificacion = li.CodigoCertificacion).Count > 1 Then
                    logGuardar = False
                    Exit For
                End If

                If li.FechaFinal.Date < li.FechaInicial.Date Then
                    logGuardarFecha = False
                    Exit For
                End If
            Next

            If logGuardar = False Then
                mostrarMensaje(strMenasajeCertificacion, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionado = 3
                Exit Sub
            End If

            If logGuardarFecha = False Then
                mostrarMensaje("No se puede tener certificaciones con fechas finales menores a la fecha inicial.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionado = 3
                Exit Sub
            End If

            Dim strMensajeConcepto As String = "No se puede tener conceptos repetidos."

            For Each li In ListaConceptosXReceptor
                If IsNothing(li.IdConcepto) Or li.IdConcepto = 0 Then
                    logGuardar = False
                    strMensajeConcepto = "Uno o varios conceptos no han sido seleccionados."
                    Exit For
                End If
                If ListaConceptosXReceptor.Where(Function(i) IIf(IsNothing(i.IdConcepto), 0, i.IdConcepto) = IIf(IsNothing(li.IdConcepto), 0, li.IdConcepto)).Count > 1 Then
                    logGuardar = False
                    Exit For
                End If
            Next


            If logGuardar = False Then
                mostrarMensaje(strMensajeConcepto, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionado = 4
                Exit Sub
            End If

            Dim strMensajeRepetidos As String = "No se puede tener parametros repetidos,"

            For Each li In ListaRetornosCompleta

                If (IsNothing(li.Topico) Or String.IsNullOrEmpty(li.Codigo = "") And li.Seleccionado = True) Then
                    logGuardar = False
                    strMensajeConcepto = "Uno o varios parametros no han sido seleccionados."
                    Exit For
                End If
                If ListaRetornosCompleta.Where(Function(i) IIf(IsNothing(i.Topico), "", i.Topico) = IIf(IsNothing(li.Topico), "", li.Topico) And
                                                   IIf(IsNothing(i.Codigo), "", i.Codigo) = IIf(IsNothing(li.Codigo), "", li.Codigo)).Count > 1 Then
                    logGuardar = False
                    strMensajeRepetidos = strMensajeRepetidos & " Tópico: " & ListaTopicosParametros.Where(Function(x) x.Codigo = li.Topico).FirstOrDefault.Descripcion
                    TopicoSeleccionado = li.Topico
                    Exit For
                End If
            Next
            If logGuardar = False Then
                mostrarMensaje(strMensajeRepetidos, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                TabSeleccionado = 2
                Exit Sub
            End If


            Dim strMensajeReceptor As String = "No se puede tener clientes autorizados repetidos."

            For Each li In ListaReceptoresClientesAutorizados
                If String.IsNullOrEmpty(li.CodigoReceptorCliente) Then
                    strMensajeReceptor = "Debe de seleccionar el código del receptor."
                    logGuardar = False
                    Exit For
                End If
                If ListaReceptoresClientesAutorizados.Where(Function(i) i.CodigoReceptorCliente = li.CodigoReceptorCliente And
                                                                        i.IDComitente = li.IDComitente).Count > 1 Then
                    logGuardar = False
                    Exit For
                End If

                If li.FechaFinal.Date < li.FechaInicial.Date Then
                    logGuardarFecha = False
                    Exit For
                End If
            Next

            If logGuardar = False Then
                mostrarMensaje(strMensajeReceptor, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionado = 5
                Exit Sub
            End If

            If logGuardarFecha = False Then
                mostrarMensaje("No se puede tener clientes autorizados con fechas finales menores a la fecha inicial.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionado = 5
                Exit Sub
            End If

            ConfiguracionesAdicionalesReceptoAnterior = _ConfiguracionesAdicionalesReceptoSelected
            'If Not ListaConfiguracionesAdicionalesReceptor.Contains(_ConfiguracionesAdicionalesReceptoSelected) Then
            If ListaConfiguracionesAdicionalesReceptor.Where(Function(li) li.CodigoReceptor = _ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor).Count = 0 Then 'JBT20140327 
                origen = "insert"
                ListaConfiguracionesAdicionalesReceptor.Add(_ConfiguracionesAdicionalesReceptoSelected)
            End If
            IsBusy = True
            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                Dim strMsg As String = String.Empty
                'TODO: Pendiente garantizar que Userstate no venga vacío
                'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                '                       Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                Else
                    mostrarMensaje(So.Error.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                If Not strMsg.Equals(String.Empty) Then
                    mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                So.MarkErrorAsHandled()
                Exit Try
            End If
            MyBase.TerminoSubmitChanges(So)
            BorrarClienteBuscador()
            BorrarEspecieBuscador()
            BorrarReceptorBuscador()

            TopicoSeleccionado = Nothing

            If Not IsNothing(_ConfiguracionesAdicionalesReceptoSelected) Then
                'Consulta los parametros del receptor
                ConsultarParametros_Receptor(_ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor)

                'Consulta las certificaciones del receptor
                ConsultarCertificaciones_Receptor(_ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor)

                'Consulta los conceptos del receptor
                ConsultarConceptos_Receptor(_ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor)

                'Consultar los clientes autorizados receptor
                ConsultarClientesAutorizados_Receptor(_ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor)

                'Consultar los Tipos de negocio receptor
                ConsultarTipoNegocio_Receptor(_ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor)

                'Consultar los tipos de producto receptor
                ConsultarTipoProducto_Receptor(_ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor)

                TabSeleccionado = 1

            End If


        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub EditarRegistro()
        If dcProxy.IsLoading Then
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If



        If Not IsNothing(_ConfiguracionesAdicionalesReceptoSelected) Then
            ObtenerRegistroAnterior()
            _ConfiguracionesAdicionalesReceptoSelected.Usuario = Program.Usuario

            If IsNothing(_ConfiguracionesAdicionalesReceptoSelected.EnrutaBus) Then
                _ConfiguracionesAdicionalesReceptoSelected.EnrutaBus = False
            End If


            EditandoDetalle = False
            ConsultarReceptorActivo(_ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor)


            If IsNothing(ListaTopicosParametros) Then
                dcProxyUtils.Load(dcProxyUtils.cargarCombosEspecificosQuery("PARAMETROSRECEPTOR", Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombosEspecificos, String.Empty)
            End If


            'TopicoSeleccionado = Nothing 'JBT20140327 
        End If
    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_ConfiguracionesAdicionalesReceptoSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                EditandoDetalle = True
                ' _ConfiguracionesAdicionalesReceptoSelected = ConfiguracionesAdicionalesReceptoAnterior
            End If

            BorrarClienteBuscador()
            BorrarEspecieBuscador()
            BorrarReceptorBuscador()




            If IsNothing(ListaTopicosParametros) Then
                dcProxyUtils.Load(dcProxyUtils.cargarCombosEspecificosQuery("PARAMETROSRECEPTOR", Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarCombosEspecificos, String.Empty)
            End If
            TopicoSeleccionado = Nothing
            ConsultarParametros_Receptor(_ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor) 'JBT20140327 
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        HabilitarComboConfiguracion = False

    End Sub

    Public Overrides Sub BorrarRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(_ConfiguracionesAdicionalesReceptoSelected) Then
                'QUITAR TODOS LOS DETALLES DEL RECEPTOR
                Dim objListaParametrosReceptor As New List(Of ParametrosRecepto)
                Dim objListaCertificaciones As New List(Of CertificacionesXRecepto)
                Dim objListaConceptos As New List(Of ConceptosXRecepto)
                Dim objListaReceptoresAutorizados As New List(Of ReceptoresClientesAutorizado)
                Dim objListaTipoNegocio As New List(Of TipoNegocioXRecepto)
                Dim objListaTipoProducto As New List(Of TipoProductoXRecepto)

                objListaParametrosReceptor = ListaParametrosReceptor.ToList
                objListaCertificaciones = ListaCertificacionesXReceptor.ToList
                objListaConceptos = ListaConceptosXReceptor.ToList
                objListaReceptoresAutorizados = ListaReceptoresClientesAutorizados.ToList
                objListaTipoNegocio = ListaTipoNegocioXReceptor.ToList
                objListaTipoProducto = ListaTipoProductoXReceptor.ToList

                For Each li In objListaParametrosReceptor
                    ListaParametrosReceptor.Remove(li)
                Next

                For Each li In objListaCertificaciones
                    ListaCertificacionesXReceptor.Remove(li)
                Next

                For Each li In objListaConceptos
                    ListaConceptosXReceptor.Remove(li)
                Next

                For Each li In objListaReceptoresAutorizados
                    ListaReceptoresClientesAutorizados.Remove(li)
                Next

                For Each li In objListaTipoNegocio
                    ListaTipoNegocioXReceptor.Remove(li)
                Next

                For Each li In objListaTipoProducto
                    ListaTipoProductoXReceptor.Remove(li)
                Next

                dcProxy.ConfiguracionesAdicionalesReceptos.Remove(_ConfiguracionesAdicionalesReceptoSelected)
                _ConfiguracionesAdicionalesReceptoSelected = _ListaConfiguracionesAdicionalesReceptor.LastOrDefault  'Dic202011  nueva
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")   'Dic202011 Nothing -> "BorrarRegistro"
            End If

            'mostrarMensaje("Esta funcionalidad se encuentra deshabilitada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub PrepararNuevaBusqueda()
        Try
            Dim objCB As New CamposBusquedaConfiguracionReceptor
            objCB.Codigo = String.Empty

            cb = objCB
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preparar la consulta", _
             Me.ToString(), "PrepararNuevaBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ObtenerRegistroAnterior()
        Try
            Dim objConfiguracion As New ConfiguracionesAdicionalesRecepto
            If Not IsNothing(_ConfiguracionesAdicionalesReceptoSelected) Then
                objConfiguracion.CodigoReceptor = _ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor
                objConfiguracion.EnrutaBus = _ConfiguracionesAdicionalesReceptoSelected.EnrutaBus
                objConfiguracion.ExtensionDefecto = _ConfiguracionesAdicionalesReceptoSelected.ExtensionDefecto
                objConfiguracion.NombreReceptor = _ConfiguracionesAdicionalesReceptoSelected.NombreReceptor
                objConfiguracion.PorcentajePatrimonioTecnico = _ConfiguracionesAdicionalesReceptoSelected.PorcentajePatrimonioTecnico
                objConfiguracion.NombreTipoOrden = _ConfiguracionesAdicionalesReceptoSelected.NombreTipoOrden
                objConfiguracion.NombreTipoOrdenDefecto = _ConfiguracionesAdicionalesReceptoSelected.NombreTipoOrdenDefecto
                objConfiguracion.TipoOrden = _ConfiguracionesAdicionalesReceptoSelected.TipoOrden
                objConfiguracion.TipoOrdenDefecto = _ConfiguracionesAdicionalesReceptoSelected.TipoOrdenDefecto
                objConfiguracion.Usuario = _ConfiguracionesAdicionalesReceptoSelected.Usuario
            End If

            ConfiguracionesAdicionalesReceptoAnterior = Nothing
            ConfiguracionesAdicionalesReceptoAnterior = objConfiguracion
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la orden anterior.", _
             Me.ToString(), "ObtenerRegistroAnterior", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub BorrarClienteBuscador()
        Try
            If BorrarCliente Then
                BorrarCliente = False
            End If

            BorrarCliente = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al intentar borrar el cliente seleccionado.", _
             Me.ToString(), "BorrarClienteBuscador", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub BorrarEspecieBuscador()
        Try
            If BorrarEspecie Then
                BorrarEspecie = False
            End If

            BorrarEspecie = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al intentar borrar la especie seleccionada.", _
             Me.ToString(), "BorrarEspecieBuscador", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub BorrarReceptorBuscador()
        Try
            If BorrarReceptor Then
                BorrarReceptor = False
            End If

            BorrarReceptor = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al intentar borrar el receptor seleccionado.", _
             Me.ToString(), "BorrarReceptorBuscador", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Parametros Receptor"

    Private _ListaParametrosReceptor As EntitySet(Of ParametrosRecepto)
    Public Property ListaParametrosReceptor() As EntitySet(Of ParametrosRecepto)
        Get
            Return _ListaParametrosReceptor
        End Get
        Set(ByVal value As EntitySet(Of ParametrosRecepto))
            _ListaParametrosReceptor = value
            MyBase.CambioItem("ListaParametrosReceptor")
            MyBase.CambioItem("ListaParametrosReceptorPaged")
        End Set
    End Property

    Public ReadOnly Property ListaParametrosReceptorPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaParametrosReceptor) Then
                Dim view = New PagedCollectionView(_ListaParametrosReceptor)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _ParametrosReceptoSelected As ParametrosRecepto
    Public Property ParametrosReceptoSelected() As ParametrosRecepto
        Get
            Return _ParametrosReceptoSelected
        End Get
        Set(ByVal value As ParametrosRecepto)
            _ParametrosReceptoSelected = value
            MyBase.CambioItem("ParametrosReceptoSelected")
        End Set
    End Property

    Private _ListaTopicosParametros As List(Of ClaseTopicos)
    Public Property ListaTopicosParametros() As List(Of ClaseTopicos)
        Get
            Return _ListaTopicosParametros
        End Get
        Set(ByVal value As List(Of ClaseTopicos))
            _ListaTopicosParametros = value
            MyBase.CambioItem("ListaTopicosParametros")
        End Set
    End Property

    Private _TopicoSeleccionado As String
    <Display(Name:="Topico", Description:="Topico")> _
    Public Property TopicoSeleccionado() As String
        Get
            Return _TopicoSeleccionado
        End Get
        Set(ByVal value As String)
            _TopicoSeleccionado = value
            If Not String.IsNullOrEmpty(TopicoSeleccionado) Then
                ObtenerRetornosTopico()
            Else
                ListaRetornos = Nothing
                MyBase.CambioItem("ListaRetornos")
            End If
            MyBase.CambioItem("TopicoSeleccionado")
        End Set
    End Property

    Private _ListaRetornosCompleta As List(Of ClaseRetornos)
    Public Property ListaRetornosCompleta() As List(Of ClaseRetornos)
        Get
            Return _ListaRetornosCompleta
        End Get
        Set(ByVal value As List(Of ClaseRetornos))
            _ListaRetornosCompleta = value
            MyBase.CambioItem("ListaRetornosCompleta")
        End Set
    End Property

    Private _ListaRetornos As List(Of ClaseRetornos)
    Public Property ListaRetornos() As List(Of ClaseRetornos)
        Get
            Return _ListaRetornos
        End Get
        Set(ByVal value As List(Of ClaseRetornos))
            _ListaRetornos = value
            MyBase.CambioItem("ListaRetornos")
        End Set
    End Property

    Private WithEvents _RetornoSeleccionado As ClaseRetornos
    Public Property RetornoSeleccionado() As ClaseRetornos
        Get
            Return _RetornoSeleccionado
        End Get
        Set(ByVal value As ClaseRetornos)
            _RetornoSeleccionado = value
            MyBase.CambioItem("RetornoSeleccionado")
        End Set
    End Property

    Private Sub ConsultarParametros_Receptor(ByVal pstrCodigoReceptor As String)
        Try
            If Not IsNothing(dcProxy.ParametrosReceptos) Then
                dcProxy.ParametrosReceptos.Clear()
            End If

            dcProxy.Load(dcProxy.ParametrosReceptorConsultarQuery(pstrCodigoReceptor, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerParametrosReceptor, String.Empty)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los parametros del receptor.", _
                                 Me.ToString(), "ConsultarParametros_Receptor", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerParametrosReceptorPorDefecto_Completed(ByVal lo As LoadOperation(Of ParametrosRecepto))
        If Not lo.HasError Then
            ParametrosReceptoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ParametrosRecepto por defecto", _
                                             Me.ToString(), "TerminoTraerParametrosReceptoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerParametrosReceptor(ByVal lo As LoadOperation(Of ParametrosRecepto))
        If Not lo.HasError Then
            ListaParametrosReceptor = dcProxy.ParametrosReceptos
            If ListaParametrosReceptor.Count > 0 Then
                TopicoSeleccionado = ListaParametrosReceptor.First.Topico 'JBT20140327 
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ParametrosReceptor", _
                                             Me.ToString(), "TerminoTraerParametrosRecepto", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Public Sub ObtenerRetornosTopico()
        Try
            Dim objListaRetornos As New List(Of ClaseRetornos)
            Dim objListaRetornosOrganizados As New List(Of ClaseRetornos)

            If Not IsNothing(ListaRetornos) Then
                If ListaRetornos.Count > 0 Then
                    Dim objTopico As String = String.Empty

                    For Each li In ListaRetornos
                        'If li.Topico <> objTopico Then
                        '    If ListaParametrosReceptor.Where(Function(i) i.Topico = li.Topico).Count > 0 Then
                        '        objTopico = li.Topico
                        '        Dim objListaTopicos As New List(Of ParametrosRecepto)
                        '        objListaTopicos = ListaParametrosReceptor.Where(Function(i) i.Topico = li.Topico).ToList
                        '        For Each lo In objListaTopicos
                        '            ListaParametrosReceptor.Remove(lo)
                        '        Next
                        '    End If
                        'End If

                        If li.Seleccionado Then
                            If ListaParametrosReceptor.Where(Function(I) I.Topico = li.Topico And I.Valor = li.Codigo).Count > 0 Then
                                ListaParametrosReceptor.Where(Function(I) I.Topico = li.Topico And I.Valor = li.Codigo).First.Prioridad = li.Prioridad
                                ListaParametrosReceptor.Where(Function(I) I.Topico = li.Topico And I.Valor = li.Codigo).First.Usuario = Program.Usuario
                            Else
                                Dim objParametro As New ParametrosRecepto
                                objParametro.ID = li.ID
                                objParametro.CodigoReceptor = _ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor
                                objParametro.Prioridad = li.Prioridad
                                objParametro.Topico = li.Topico
                                objParametro.Usuario = Program.Usuario
                                objParametro.Valor = li.Codigo

                                ListaParametrosReceptor.Add(objParametro)
                            End If
                        End If
                    Next
                End If
            End If

            If Not IsNothing(_ListaRetornosCompleta) Then
                For Each li In ListaRetornosCompleta
                    If li.Topico = TopicoSeleccionado Then
                        objListaRetornos.Add(New ClaseRetornos With {.Codigo = li.Codigo, .Descripcion = li.Descripcion, .Topico = li.Topico, .Prioridad = 0, .Seleccionado = False})
                    End If
                Next
            End If

            If Not IsNothing(_ListaParametrosReceptor) Then
                If ListaParametrosReceptor.Where(Function(i) i.CodigoReceptor = _ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor And _
                                                         i.Topico = TopicoSeleccionado).Count > 0 Then
                    Dim Descripcion As String = String.Empty

                    For Each li In ListaParametrosReceptor.Where(Function(i) i.CodigoReceptor = _ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor And _
                                                                             i.Topico = TopicoSeleccionado)
                        For Each lo In objListaRetornos.Where(Function(i) i.Topico = TopicoSeleccionado And i.Codigo = li.Valor)
                            lo.ID = li.ID
                            lo.Prioridad = li.Prioridad
                            lo.Seleccionado = True
                        Next
                    Next
                End If
            End If


            If objListaRetornos.Where(Function(i) i.Seleccionado = False).Count > 0 Then
                For Each li In objListaRetornos.Where(Function(i) i.Seleccionado = False)
                    li.ID = 0
                    li.Prioridad = 0
                Next
            End If

            If objListaRetornos.Where(Function(i) i.Seleccionado = True).Count > 0 Then
                For Each li In objListaRetornos.Where(Function(i) i.Seleccionado = True).OrderBy(Function(I) I.Prioridad)
                    objListaRetornosOrganizados.Add(li)
                Next
            End If

            If objListaRetornos.Where(Function(i) i.Seleccionado = False).Count > 0 Then
                For Each li In objListaRetornos.Where(Function(i) i.Seleccionado = False).OrderBy(Function(I) I.Descripcion)
                    objListaRetornosOrganizados.Add(li)
                Next
            End If

            ListaRetornos = objListaRetornosOrganizados.ToList
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtener los valores del topico seleccionado.", _
                                             Me.ToString(), "ObtenerRetornosTopico", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _RetornoSeleccionado_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _RetornoSeleccionado.PropertyChanged
        Try
            If Editando Then
                If e.PropertyName.Equals("Prioridad") Then
                    'Reorganizar las prioridad del tipo de producto.
                    If ListaRetornos.Where(Function(i) i.Prioridad = RetornoSeleccionado.Prioridad And i.Codigo <> RetornoSeleccionado.Codigo And i.Topico = RetornoSeleccionado.Topico And i.Seleccionado = True).Count > 0 Then
                        Dim UltimaPrioridad As Integer = 0

                        For Each li In ListaRetornos.Where(Function(i) i.Prioridad >= RetornoSeleccionado.Prioridad And i.Codigo <> RetornoSeleccionado.Codigo And i.Topico = RetornoSeleccionado.Topico And i.Seleccionado = True).OrderBy(Function(i) i.Prioridad)
                            If li.Prioridad = RetornoSeleccionado.Prioridad Then
                                li.Prioridad = li.Prioridad + 1
                                UltimaPrioridad = li.Prioridad
                            Else
                                If li.Prioridad = UltimaPrioridad Then
                                    li.Prioridad = li.Prioridad + 1
                                    UltimaPrioridad = li.Prioridad
                                End If
                            End If
                        Next
                    End If
                ElseIf e.PropertyName.Equals("Seleccionado") Then
                    If RetornoSeleccionado.Seleccionado = False Then
                        If Not IsNothing(ListaParametrosReceptor) Then
                            If ListaParametrosReceptor.Where(Function(i) i.Topico = RetornoSeleccionado.Topico And i.Valor = RetornoSeleccionado.Codigo).Count > 0 Then
                                Dim objParametro As ParametrosRecepto = ListaParametrosReceptor.Where(Function(i) i.Topico = RetornoSeleccionado.Topico And i.Valor = RetornoSeleccionado.Codigo).First
                                ListaParametrosReceptor.Remove(objParametro)
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar el cambio de propiedad.", _
             Me.ToString(), "_RetornoSeleccionado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Certificaciones Receptor"

    Private _ListaCertificacionesXReceptor As EntitySet(Of CertificacionesXRecepto)
    Public Property ListaCertificacionesXReceptor() As EntitySet(Of CertificacionesXRecepto)
        Get
            Return _ListaCertificacionesXReceptor
        End Get
        Set(ByVal value As EntitySet(Of CertificacionesXRecepto))
            _ListaCertificacionesXReceptor = value
            If Not IsNothing(ListaCertificacionesXReceptor) Then
                If ListaCertificacionesXReceptor.Count > 0 Then
                    _CertificacionesXReceptoSelected = ListaCertificacionesXReceptor.First
                Else
                    _CertificacionesXReceptoSelected = Nothing
                End If
            End If
            MyBase.CambioItem("ListaCertificacionesXReceptor")
            MyBase.CambioItem("ListaCertificacionesXReceptorPaged")
        End Set
    End Property

    Public ReadOnly Property ListaCertificacionesXReceptorPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaCertificacionesXReceptor) Then
                Dim view = New PagedCollectionView(_ListaCertificacionesXReceptor)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _CertificacionesXReceptoSelected As CertificacionesXRecepto
    Public Property CertificacionesXReceptoSelected() As CertificacionesXRecepto
        Get
            Return _CertificacionesXReceptoSelected
        End Get
        Set(ByVal value As CertificacionesXRecepto)
            _CertificacionesXReceptoSelected = value
            MyBase.CambioItem("CertificacionesXReceptoSelected")
        End Set
    End Property

    Public Sub ConsultarCertificaciones_Receptor(ByVal pstrReceptor As String)
        Try
            If Not IsNothing(dcProxy.CertificacionesXReceptos) Then
                dcProxy.CertificacionesXReceptos.Clear()
            End If

            dcProxy.Load(dcProxy.CertificacionesXReceptorConsultarQuery(pstrReceptor, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCertificacionesXReceptor, String.Empty)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar las certificaciones del tipo de negocio.", _
                                             Me.ToString(), "ConsultarCertificaciones_Receptor", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerCertificacionesXReceptorPorDefecto_Completed(ByVal lo As LoadOperation(Of CertificacionesXRecepto))
        If Not lo.HasError Then
            CertificacionesXReceptoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la CertificacionesXRecepto por defecto", _
                                             Me.ToString(), "TerminoTraerCertificacionesXReceptoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerCertificacionesXReceptor(ByVal lo As LoadOperation(Of CertificacionesXRecepto))
        If Not lo.HasError Then
            ListaCertificacionesXReceptor = dcProxy.CertificacionesXReceptos
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de CertificacionesXReceptor", _
                                             Me.ToString(), "TerminoTraerCertificacionesXReceptor", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

#End Region

#Region "Conceptos receptor"

    Private _ListaConceptosXReceptor As EntitySet(Of ConceptosXRecepto)
    Public Property ListaConceptosXReceptor() As EntitySet(Of ConceptosXRecepto)
        Get
            Return _ListaConceptosXReceptor
        End Get
        Set(ByVal value As EntitySet(Of ConceptosXRecepto))
            _ListaConceptosXReceptor = value
            If Not IsNothing(ListaConceptosXReceptor) Then
                If ListaConceptosXReceptor.Count > 0 Then
                    _ConceptosXReceptoSelected = ListaConceptosXReceptor.First
                Else
                    _ConceptosXReceptoSelected = Nothing
                End If
            End If
            MyBase.CambioItem("ListaConceptosXReceptor")
            MyBase.CambioItem("ListaConceptosXReceptorPaged")
        End Set
    End Property

    Public ReadOnly Property ListaConceptosXReceptorPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaConceptosXReceptor) Then
                Dim view = New PagedCollectionView(_ListaConceptosXReceptor)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _ConceptosXReceptoSelected As ConceptosXRecepto
    Public Property ConceptosXReceptoSelected() As ConceptosXRecepto
        Get
            Return _ConceptosXReceptoSelected
        End Get
        Set(ByVal value As ConceptosXRecepto)
            _ConceptosXReceptoSelected = value
            MyBase.CambioItem("ConceptosXReceptoSelected")
        End Set
    End Property

    Public Sub ConsultarConceptos_Receptor(ByVal pstrReceptor As String)
        Try
            If Not IsNothing(dcProxy.ConceptosXReceptos) Then
                dcProxy.ConceptosXReceptos.Clear()
            End If

            dcProxy.Load(dcProxy.ConceptosXReceptorConsultarQuery(pstrReceptor, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerConceptosXReceptor, String.Empty)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los conceptos del receptor.", _
                                             Me.ToString(), "ConsultarConceptos_Receptor", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerConceptosXReceptorPorDefecto_Completed(ByVal lo As LoadOperation(Of ConceptosXRecepto))
        If Not lo.HasError Then
            ConceptosXReceptoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ConceptosXRecepto por defecto", _
                                             Me.ToString(), "TerminoTraerConceptosXReceptoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerConceptosXReceptor(ByVal lo As LoadOperation(Of ConceptosXRecepto))
        If Not lo.HasError Then
            ListaConceptosXReceptor = dcProxy.ConceptosXReceptos
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ConceptosXReceptor", _
                                             Me.ToString(), "TerminoTraerConceptosXReceptor", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub _ConceptosXReceptoSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ConceptosXReceptoSelected.PropertyChanged
        Try
            If Editando Then
                If e.PropertyName.Equals("Prioridad") Then
                    'Reorganizar las prioridad del tipo de producto.
                    If ListaConceptosXReceptor.Where(Function(i) i.Prioridad = _ConceptosXReceptoSelected.Prioridad And i.ID <> _ConceptosXReceptoSelected.ID).Count > 0 Then
                        Dim UltimaPrioridad As Integer = 0

                        For Each li In ListaConceptosXReceptor.Where(Function(i) i.Prioridad >= _ConceptosXReceptoSelected.Prioridad And i.ID <> _ConceptosXReceptoSelected.ID).OrderBy(Function(i) i.Prioridad)
                            If li.Prioridad = _ConceptosXReceptoSelected.Prioridad Then
                                li.Prioridad = li.Prioridad + 1
                                UltimaPrioridad = li.Prioridad
                            Else
                                If li.Prioridad = UltimaPrioridad Then
                                    li.Prioridad = li.Prioridad + 1
                                    UltimaPrioridad = li.Prioridad
                                End If
                            End If
                        Next
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar el cambio de propiedad.", _
             Me.ToString(), "_ConceptosXReceptoSelected", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Clientes Terceros Autorizados"
    Private _ReceptorActivo As ReceptorActivo
    Public Property ReceptorActivo() As ReceptorActivo
        Get
            Return _ReceptorActivo
        End Get
        Set(ByVal value As ReceptorActivo)
            _ReceptorActivo = value
            MyBase.CambioItem("ReceptorActivo")
        End Set
    End Property


    Private _ListaReceptoresClientesAutorizados As EntitySet(Of ReceptoresClientesAutorizado)
    Public Property ListaReceptoresClientesAutorizados() As EntitySet(Of ReceptoresClientesAutorizado)
        Get
            Return _ListaReceptoresClientesAutorizados
        End Get
        Set(ByVal value As EntitySet(Of ReceptoresClientesAutorizado))
            _ListaReceptoresClientesAutorizados = value
            If Not IsNothing(ListaReceptoresClientesAutorizados) Then
                If ListaReceptoresClientesAutorizados.Count > 0 Then
                    _ReceptoresClientesAutorizadoSelected = ListaReceptoresClientesAutorizados.First
                Else
                    _ReceptoresClientesAutorizadoSelected = Nothing
                End If
            End If
            MyBase.CambioItem("ListaReceptoresClientesAutorizados")
            MyBase.CambioItem("ListaReceptoresClientesAutorizadosPaged")
        End Set
    End Property

    Public ReadOnly Property ListaReceptoresClientesAutorizadosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaReceptoresClientesAutorizados) Then
                Dim view = New PagedCollectionView(_ListaReceptoresClientesAutorizados)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ReceptoresClientesAutorizadoSelected As ReceptoresClientesAutorizado
    Public Property ReceptoresClientesAutorizadoSelected() As ReceptoresClientesAutorizado
        Get
            Return _ReceptoresClientesAutorizadoSelected
        End Get
        Set(ByVal value As ReceptoresClientesAutorizado)
            _ReceptoresClientesAutorizadoSelected = value
            MyBase.CambioItem("ReceptoresClientesAutorizadoSelected")
        End Set
    End Property

    Private _CodigoReceptorBuscar As String
    Public Property CodigoReceptorBuscar() As String
        Get
            Return _CodigoReceptorBuscar
        End Get
        Set(ByVal value As String)
            _CodigoReceptorBuscar = value
            MyBase.CambioItem("CodigoReceptorBuscar")
        End Set
    End Property

    Public Sub ConsultarReceptorActivo(ByVal pstrReceptor As String)
        Try
            IsBusy = True
            If Not IsNothing(dcProxy.ReceptorActivos) Then
                dcProxy.ReceptorActivos.Clear()
            End If

            dcProxy.Load(dcProxy.ReceptorActivoConsultarQuery(pstrReceptor, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarReceptorActivo, String.Empty)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los clientes autorizados del receptor.", _
                                             Me.ToString(), "ConsultarClientesAutorizados_Receptor", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarClientesAutorizados_Receptor(ByVal pstrReceptor As String)
        Try
            If Not IsNothing(dcProxy.ReceptoresClientesAutorizados) Then
                dcProxy.ReceptoresClientesAutorizados.Clear()
            End If

            dcProxy.Load(dcProxy.ReceptoresClientesAutorizadosConsultarQuery(pstrReceptor, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerReceptoresClientesAutorizados, String.Empty)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los clientes autorizados del receptor.", _
                                             Me.ToString(), "ConsultarClientesAutorizados_Receptor", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerReceptoresClientesAutorizadosPorDefecto_Completed(ByVal lo As LoadOperation(Of ReceptoresClientesAutorizado))
        If Not lo.HasError Then
            ReceptoresClientesAutorizadoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la ReceptoresClientesAutorizado por defecto", _
                                             Me.ToString(), "TerminoTraerReceptoresClientesAutorizadoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerReceptoresClientesAutorizados(ByVal lo As LoadOperation(Of ReceptoresClientesAutorizado))
        If Not lo.HasError Then
            ListaReceptoresClientesAutorizados = dcProxy.ReceptoresClientesAutorizados
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ReceptoresClientesAutorizados", _
                                             Me.ToString(), "TerminoTraerReceptoresClientesAutorizado", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoConsultarReceptorActivo(ByVal lo As LoadOperation(Of ReceptorActivo))
        IsBusy = False
        If Not lo.HasError Then
            ReceptorActivo = dcProxy.ReceptorActivos.FirstOrDefault
            If IsNothing(ReceptorActivo) Then
                MyBase.RetornarValorEdicionNavegacion()
                mostrarMensaje("¡El registro no se puede editar ya que el receptor esta inactivo!", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Editando = False
            Else
                If ReceptorActivo.logActivo = True Then
                    Editando = True
                Else
                    MyBase.RetornarValorEdicionNavegacion()
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de Receptor Activo", _
                                             Me.ToString(), "TerminoConsultarReceptorActivo", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
#End Region

#Region "Tipo negocio Receptor"

    Private _ListaTipoNegocioXReceptor As EntitySet(Of TipoNegocioXRecepto)
    Public Property ListaTipoNegocioXReceptor() As EntitySet(Of TipoNegocioXRecepto)
        Get
            Return _ListaTipoNegocioXReceptor
        End Get
        Set(ByVal value As EntitySet(Of TipoNegocioXRecepto))
            _ListaTipoNegocioXReceptor = value
            If Not IsNothing(ListaTipoNegocioXReceptor) Then
                If ListaTipoNegocioXReceptor.Count > 0 Then
                    _TipoNegocioXReceptoSelected = ListaTipoNegocioXReceptor.First
                Else
                    _TipoNegocioXReceptoSelected = Nothing
                End If
            End If
            MyBase.CambioItem("ListaTipoNegocioXReceptor")
            MyBase.CambioItem("ListaTipoNegocioXReceptorPaged")
        End Set
    End Property

    Public ReadOnly Property ListaTipoNegocioXReceptorPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaTipoNegocioXReceptor) Then
                Dim view = New PagedCollectionView(_ListaTipoNegocioXReceptor)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _TipoNegocioXReceptoSelected As TipoNegocioXRecepto
    Public Property TipoNegocioXReceptoSelected() As TipoNegocioXRecepto
        Get
            Return _TipoNegocioXReceptoSelected
        End Get
        Set(ByVal value As TipoNegocioXRecepto)
            _TipoNegocioXReceptoSelected = value
            MyBase.CambioItem("TipoNegocioXReceptoSelected")
        End Set
    End Property

    Public Sub ConsultarTipoNegocio_Receptor(ByVal pstrReceptor As String)
        Try
            If Not IsNothing(dcProxy.TipoNegocioXReceptos) Then
                dcProxy.TipoNegocioXReceptos.Clear()
            End If

            dcProxy.Load(dcProxy.TipoNegocioXReceptorConsultarQuery(pstrReceptor, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoNegocioXReceptor, String.Empty)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los tipos de negocio x receptor.", _
                                             Me.ToString(), "ConsultarTipoNegocio_Receptor", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerTipoNegocioXReceptorPorDefecto_Completed(ByVal lo As LoadOperation(Of TipoNegocioXRecepto))
        If Not lo.HasError Then
            TipoNegocioXReceptoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la TipoNegocioXRecepto por defecto", _
                                             Me.ToString(), "TerminoTraerTipoNegocioXReceptoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerTipoNegocioXReceptor(ByVal lo As LoadOperation(Of TipoNegocioXRecepto))
        If Not lo.HasError Then
            ListaTipoNegocioXReceptor = dcProxy.TipoNegocioXReceptos
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de TipoNegocioXReceptor", _
                                             Me.ToString(), "TerminoTraerTipoNegocioXRecepto", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub _TipoNegocioXReceptoSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _TipoNegocioXReceptoSelected.PropertyChanged
        Try
            If Editando Then
                If e.PropertyName.Equals("Prioridad") Then
                    'Reorganizar las prioridad del tipo de producto.
                    If ListaTipoNegocioXReceptor.Where(Function(i) i.Prioridad = _TipoNegocioXReceptoSelected.Prioridad And i.ID <> _TipoNegocioXReceptoSelected.ID).Count > 0 Then
                        Dim UltimaPrioridad As Integer = 0

                        For Each li In ListaTipoNegocioXReceptor.Where(Function(i) i.Prioridad >= _TipoNegocioXReceptoSelected.Prioridad And i.ID <> _TipoNegocioXReceptoSelected.ID).OrderBy(Function(i) i.Prioridad)
                            If li.Prioridad = _TipoNegocioXReceptoSelected.Prioridad Then
                                li.Prioridad = li.Prioridad + 1
                                UltimaPrioridad = li.Prioridad
                            Else
                                If li.Prioridad = UltimaPrioridad Then
                                    li.Prioridad = li.Prioridad + 1
                                    UltimaPrioridad = li.Prioridad
                                End If
                            End If
                        Next
                    End If
                ElseIf e.PropertyName.Equals("ValorComision") Then
                    If Not IsNothing(_TipoNegocioXReceptoSelected) Then
                        If _TipoNegocioXReceptoSelected.ValorComision > 0 Then
                            _TipoNegocioXReceptoSelected.PorcentajeComision = 0
                        End If
                    End If
                ElseIf e.PropertyName.Equals("PorcentajeComision") Then
                    If Not IsNothing(_TipoNegocioXReceptoSelected) Then
                        If _TipoNegocioXReceptoSelected.PorcentajeComision > 0 Then
                            _TipoNegocioXReceptoSelected.ValorComision = 0
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar el cambio de propiedad.", _
             Me.ToString(), "_TipoNegocioXReceptoSelected", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Tipo producto receptor"

    Private _ListaTipoProductoXReceptor As EntitySet(Of TipoProductoXRecepto)
    Public Property ListaTipoProductoXReceptor() As EntitySet(Of TipoProductoXRecepto)
        Get
            Return _ListaTipoProductoXReceptor
        End Get
        Set(ByVal value As EntitySet(Of TipoProductoXRecepto))
            _ListaTipoProductoXReceptor = value
            If Not IsNothing(ListaTipoProductoXReceptor) Then
                If ListaTipoProductoXReceptor.Count > 0 Then
                    _TipoProductoXReceptoSelected = ListaTipoProductoXReceptor.First
                Else
                    _TipoProductoXReceptoSelected = Nothing
                End If
            End If
            MyBase.CambioItem("ListaTipoProductoXReceptor")
            MyBase.CambioItem("ListaTipoProductoXReceptorPaged")
        End Set
    End Property

    Public ReadOnly Property ListaTipoProductoXReceptorPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaTipoProductoXReceptor) Then
                Dim view = New PagedCollectionView(_ListaTipoProductoXReceptor)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private WithEvents _TipoProductoXReceptoSelected As TipoProductoXRecepto
    Public Property TipoProductoXReceptoSelected() As TipoProductoXRecepto
        Get
            Return _TipoProductoXReceptoSelected
        End Get
        Set(ByVal value As TipoProductoXRecepto)
            _TipoProductoXReceptoSelected = value
            MyBase.CambioItem("TipoProductoXReceptoSelected")
        End Set
    End Property

    Public Sub ConsultarTipoProducto_Receptor(ByVal pstrReceptor As String)
        Try
            If Not IsNothing(dcProxy.TipoProductoXReceptos) Then
                dcProxy.TipoProductoXReceptos.Clear()
            End If

            dcProxy.Load(dcProxy.TipoProductoXReceptorConsultarQuery(pstrReceptor, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerTipoProductoXReceptor, String.Empty)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los tipos de producto x receptor.", _
                                             Me.ToString(), "ConsultarTipoProducto_Receptor", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerTipoProductoXReceptorPorDefecto_Completed(ByVal lo As LoadOperation(Of TipoProductoXRecepto))
        If Not lo.HasError Then
            TipoProductoXReceptoPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la TipoProductoXRecepto por defecto", _
                                             Me.ToString(), "TerminoTraerTipoProductoXReceptoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerTipoProductoXReceptor(ByVal lo As LoadOperation(Of TipoProductoXRecepto))
        If Not lo.HasError Then
            ListaTipoProductoXReceptor = dcProxy.TipoProductoXReceptos
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de TipoProductoXReceptor", _
                                             Me.ToString(), "TerminoTraerTipoProductoXRecepto", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub _TipoProductoXReceptoSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _TipoProductoXReceptoSelected.PropertyChanged
        Try
            If Editando Then
                If e.PropertyName.Equals("Prioridad") Then
                    'Reorganizar las prioridad del tipo de producto.
                    If ListaTipoProductoXReceptor.Where(Function(i) i.Prioridad = _TipoProductoXReceptoSelected.Prioridad And i.ID <> _TipoProductoXReceptoSelected.ID).Count > 0 Then
                        Dim UltimaPrioridad As Integer = 0

                        For Each li In ListaTipoProductoXReceptor.Where(Function(i) i.Prioridad >= _TipoProductoXReceptoSelected.Prioridad And i.ID <> _TipoProductoXReceptoSelected.ID).OrderBy(Function(i) i.Prioridad)
                            If li.Prioridad = _TipoProductoXReceptoSelected.Prioridad Then
                                li.Prioridad = li.Prioridad + 1
                                UltimaPrioridad = li.Prioridad
                            Else
                                If li.Prioridad = UltimaPrioridad Then
                                    li.Prioridad = li.Prioridad + 1
                                    UltimaPrioridad = li.Prioridad
                                End If
                            End If
                        Next
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar el cambio de propiedad.", _
             Me.ToString(), "_TipoProductoXReceptoSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Detalles Maestros del Tipo negocio"

    Public Overrides Sub NuevoRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmParametros"
                    'Dim newParametro As New ParametrosRecepto
                    'newParametro.ID = ParametrosReceptoPorDefecto.ID
                    'newParametro.CodigoReceptor = ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor
                    'newParametro.Topico = TopicoSeleccionado
                    'newParametro.Prioridad = ParametrosReceptoPorDefecto.Prioridad
                    'newParametro.Valor = ParametrosReceptoPorDefecto.Valor
                    'newParametro.Usuario = Program.Usuario

                    'ListaParametrosReceptor.Add(newParametro)
                    'ParametrosReceptoSelected = newParametro

                    'MyBase.CambioItem("ParametrosReceptoSelected")
                    'MyBase.CambioItem("ListaParametrosReceptor")
                    mostrarMensaje("Esta funcionalidad se encuentra deshabilitada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    MyBase.CambioItem("Editando")
                Case "cmCertificaciones"
                    Dim newCertificacion As New CertificacionesXRecepto
                    newCertificacion.ID = -New Random().Next()
                    newCertificacion.CodigoReceptor = _ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor
                    newCertificacion.CodigoCertificacion = CertificacionesXReceptoPorDefecto.CodigoCertificacion
                    newCertificacion.FechaInicial = CertificacionesXReceptoPorDefecto.FechaInicial
                    newCertificacion.FechaFinal = CertificacionesXReceptoPorDefecto.FechaFinal
                    newCertificacion.Usuario = Program.Usuario

                    ListaCertificacionesXReceptor.Add(newCertificacion)
                    CertificacionesXReceptoSelected = newCertificacion

                    MyBase.CambioItem("CertificacionesXReceptoSelected")
                    MyBase.CambioItem("ListaCertificacionesXReceptor")
                    MyBase.CambioItem("Editando")
                Case "cmConceptos"
                    Dim newConcepto As New ConceptosXRecepto
                    newConcepto.ID = -New Random().Next()
                    newConcepto.CodigoReceptor = _ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor
                    newConcepto.IdConcepto = ConceptosXReceptoPorDefecto.IdConcepto
                    newConcepto.Prioridad = ConceptosXReceptoPorDefecto.Prioridad
                    newConcepto.Usuario = Program.Usuario

                    ListaConceptosXReceptor.Add(newConcepto)
                    ConceptosXReceptoSelected = newConcepto

                    MyBase.CambioItem("ConceptosXReceptoSelected")
                    MyBase.CambioItem("ListaConceptosXReceptor")
                    MyBase.CambioItem("Editando")
                Case "cmClientesAutorizados"
                    Dim newClienteAutorizado As New ReceptoresClientesAutorizado
                    newClienteAutorizado.ID = -New Random().Next()
                    newClienteAutorizado.CodigoReceptorTercero = _ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor
                    newClienteAutorizado.CodigoReceptorCliente = ReceptoresClientesAutorizadoPorDefecto.CodigoReceptorCliente
                    newClienteAutorizado.FechaInicial = ReceptoresClientesAutorizadoPorDefecto.FechaInicial
                    newClienteAutorizado.FechaFinal = ReceptoresClientesAutorizadoPorDefecto.FechaFinal
                    newClienteAutorizado.IDComitente = ReceptoresClientesAutorizadoPorDefecto.IDComitente
                    newClienteAutorizado.NombreCliente = ReceptoresClientesAutorizadoPorDefecto.NombreCliente
                    newClienteAutorizado.NombreReceptorCliente = ReceptoresClientesAutorizadoPorDefecto.NombreReceptorCliente
                    newClienteAutorizado.NombreReceptorTercero = ReceptoresClientesAutorizadoPorDefecto.NombreReceptorTercero
                    newClienteAutorizado.NombreCliente = ReceptoresClientesAutorizadoPorDefecto.NombreCliente
                    newClienteAutorizado.Usuario = Program.Usuario

                    ListaReceptoresClientesAutorizados.Add(newClienteAutorizado)
                    ReceptoresClientesAutorizadoSelected = newClienteAutorizado

                    MyBase.CambioItem("ReceptoresClientesAutorizadoSelected")
                    MyBase.CambioItem("ListaReceptoresClientesAutorizados")
                    MyBase.CambioItem("Editando")

                    BorrarClienteBuscador()
                    BorrarReceptorBuscador()

                Case "cmTipoNegocio"
                    Dim newTipoNegocio As New TipoNegocioXRecepto
                    newTipoNegocio.ID = -New Random().Next()
                    newTipoNegocio.CodigoReceptor = _ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor
                    newTipoNegocio.CodigoTipoNegocio = TipoNegocioXReceptoPorDefecto.CodigoTipoNegocio
                    newTipoNegocio.Prioridad = TipoNegocioXReceptoPorDefecto.Prioridad
                    newTipoNegocio.ValorMaxNegociacion = TipoNegocioXReceptoPorDefecto.ValorMaxNegociacion
                    newTipoNegocio.PorcentajeComision = TipoNegocioXReceptoPorDefecto.PorcentajeComision
                    newTipoNegocio.ValorComision = TipoNegocioXReceptoPorDefecto.ValorComision
                    newTipoNegocio.Usuario = Program.Usuario

                    ListaTipoNegocioXReceptor.Add(newTipoNegocio)
                    TipoNegocioXReceptoSelected = newTipoNegocio

                    MyBase.CambioItem("TipoNegocioXReceptoSelected")
                    MyBase.CambioItem("ListaTipoNegocioXReceptor")
                    MyBase.CambioItem("Editando")
                Case "cmTipoProducto"
                    Dim newTipoProducto As New TipoProductoXRecepto
                    newTipoProducto.ID = -New Random().Next()
                    newTipoProducto.CodigoReceptor = _ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor
                    newTipoProducto.Prioridad = TipoProductoXReceptoPorDefecto.Prioridad
                    newTipoProducto.TipoProducto = TipoProductoXReceptoPorDefecto.TipoProducto
                    newTipoProducto.Usuario = Program.Usuario

                    ListaTipoProductoXReceptor.Add(newTipoProducto)
                    TipoProductoXReceptoSelected = newTipoProducto

                    MyBase.CambioItem("TipoProductoXReceptoSelected")
                    MyBase.CambioItem("ListaTipoProductoXReceptor")
                    MyBase.CambioItem("Editando")
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al insertar el nuevo detalle.", _
                                                         Me.ToString(), "NuevoRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmParametros"
                    If Not IsNothing(ListaParametrosReceptor) Then
                        'If ListaParametrosReceptor.Where(Function(i) i.CodigoReceptor = ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor And _
                        '                                             i.Topico = RetornoSeleccionado.Topico And _
                        '                                             i.Valor = RetornoSeleccionado.Codigo).Count > 0 Then
                        '    ParametrosReceptoSelected = ListaParametrosReceptor.Where(Function(i) i.CodigoReceptor = ConfiguracionesAdicionalesReceptoSelected.CodigoReceptor And _
                        '                                             i.Topico = RetornoSeleccionado.Topico And _
                        '                                             i.Valor = RetornoSeleccionado.Codigo).First
                        'End If

                        'ListaParametrosReceptor.Remove(ParametrosReceptoSelected)

                        'ListaRetornos.Remove(RetornoSeleccionado)

                        'RetornoSeleccionado = ListaRetornos.LastOrDefault
                        'If ListaRetornos.Count > 0 Then
                        '    RetornoSeleccionado = ListaRetornos.FirstOrDefault
                        'End If

                        'MyBase.CambioItem("ParametrosReceptoSelected")
                        'MyBase.CambioItem("ListaParametrosReceptor")
                        'MyBase.CambioItem("RetornoSeleccionado")
                        'MyBase.CambioItem("ListaRetornos")
                        mostrarMensaje("Esta funcionalidad se encuentra deshabilitada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                        MyBase.CambioItem("Editando")
                    End If
                Case "cmCertificaciones"
                    If Not IsNothing(ListaCertificacionesXReceptor) Then
                        If Not IsNothing(_CertificacionesXReceptoSelected) Then
                            Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(CertificacionesXReceptoSelected, ListaCertificacionesXReceptor.ToList)

                            If ListaCertificacionesXReceptor.Contains(CertificacionesXReceptoSelected) Then
                                ListaCertificacionesXReceptor.Remove(CertificacionesXReceptoSelected)
                            End If
                            If ListaCertificacionesXReceptor.Count > 0 Then
                                Program.PosicionarItemLista(CertificacionesXReceptoSelected, ListaCertificacionesXReceptor.ToList, intRegistroPosicionar)
                            Else
                                CertificacionesXReceptoSelected = Nothing
                            End If
                            'ListaCertificacionesXReceptor.Remove(CertificacionesXReceptoSelected)

                            'CertificacionesXReceptoSelected = ListaCertificacionesXReceptor.LastOrDefault
                            'If ListaCertificacionesXReceptor.Count > 0 Then
                            '    CertificacionesXReceptoSelected = ListaCertificacionesXReceptor.FirstOrDefault
                            'End If
                        End If
                        MyBase.CambioItem("CertificacionXTipoNegociSelected")
                        MyBase.CambioItem("ListaCertificacionesXReceptor")
                    End If
                Case "cmConceptos"
                    If Not IsNothing(ListaConceptosXReceptor) Then
                        If Not IsNothing(_ConceptosXReceptoSelected) Then
                            Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ConceptosXReceptoSelected, ListaConceptosXReceptor.ToList)

                            If ListaConceptosXReceptor.Contains(ConceptosXReceptoSelected) Then
                                ListaConceptosXReceptor.Remove(ConceptosXReceptoSelected)
                            End If
                            If ListaConceptosXReceptor.Count > 0 Then
                                Program.PosicionarItemLista(ConceptosXReceptoSelected, ListaConceptosXReceptor.ToList, intRegistroPosicionar)
                            Else
                                ConceptosXReceptoSelected = ListaConceptosXReceptor.FirstOrDefault
                            End If
                        End If
                        MyBase.CambioItem("ConceptosXReceptoSelected")
                        MyBase.CambioItem("ListaConceptosXReceptor")
                    End If
                Case "cmClientesAutorizados"
                    If Not IsNothing(ListaReceptoresClientesAutorizados) Then
                        If Not IsNothing(_ReceptoresClientesAutorizadoSelected) Then
                            Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ReceptoresClientesAutorizadoSelected, ListaReceptoresClientesAutorizados.ToList)

                            If ListaReceptoresClientesAutorizados.Contains(ReceptoresClientesAutorizadoSelected) Then
                                ListaReceptoresClientesAutorizados.Remove(ReceptoresClientesAutorizadoSelected)
                            End If
                            If ListaReceptoresClientesAutorizados.Count > 0 Then
                                Program.PosicionarItemLista(ReceptoresClientesAutorizadoSelected, ListaReceptoresClientesAutorizados.ToList, intRegistroPosicionar)
                            Else
                                ReceptoresClientesAutorizadoSelected = ListaReceptoresClientesAutorizados.FirstOrDefault
                            End If
                        End If
                        MyBase.CambioItem("ReceptoresClientesAutorizadoSelected")
                        MyBase.CambioItem("ListaReceptoresClientesAutorizados")
                    End If
                Case "cmTipoNegocio"
                    If Not IsNothing(ListaTipoNegocioXReceptor) Then
                        If Not IsNothing(_TipoNegocioXReceptoSelected) Then
                            Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(TipoNegocioXReceptoSelected, ListaTipoNegocioXReceptor.ToList)

                            If ListaTipoNegocioXReceptor.Contains(TipoNegocioXReceptoSelected) Then
                                ListaTipoNegocioXReceptor.Remove(TipoNegocioXReceptoSelected)
                            End If
                            If ListaTipoNegocioXReceptor.Count > 0 Then
                                Program.PosicionarItemLista(TipoNegocioXReceptoSelected, ListaTipoNegocioXReceptor.ToList, intRegistroPosicionar)
                            Else
                                TipoNegocioXReceptoSelected = ListaTipoNegocioXReceptor.FirstOrDefault
                            End If
                        End If
                        MyBase.CambioItem("TipoNegocioXReceptoSelected")
                        MyBase.CambioItem("ListaTipoNegocioXReceptor")
                    End If
                Case "cmTipoProducto"
                    If Not IsNothing(ListaTipoProductoXReceptor) Then
                        If Not IsNothing(_TipoProductoXReceptoSelected) Then
                            Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(TipoProductoXReceptoSelected, ListaTipoProductoXReceptor.ToList)

                            If ListaTipoProductoXReceptor.Contains(TipoProductoXReceptoSelected) Then
                                ListaTipoProductoXReceptor.Remove(TipoProductoXReceptoSelected)
                            End If
                            If ListaTipoProductoXReceptor.Count > 0 Then
                                Program.PosicionarItemLista(TipoProductoXReceptoSelected, ListaTipoProductoXReceptor.ToList, intRegistroPosicionar)
                            Else
                                TipoProductoXReceptoSelected = Nothing
                            End If
                        End If
                        MyBase.CambioItem("TipoProductoXReceptoSelected")
                        MyBase.CambioItem("ListaTipoProductoXReceptor")
                    End If
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar el detalle.", _
                                                         Me.ToString(), "BorrarRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaConfiguracionReceptor
    Implements INotifyPropertyChanged

    Private _Codigo As String
    <Display(Name:="Código Receptor", Description:="Código")> _
    Public Property Codigo() As String
        Get
            Return _Codigo
        End Get
        Set(ByVal value As String)
            _Codigo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Codigo"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class

Public Class ClaseTipoOrden
    Implements INotifyPropertyChanged

    Private _Codigo As String
    Public Property Codigo() As String
        Get
            Return _Codigo
        End Get
        Set(ByVal value As String)
            _Codigo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Codigo"))
        End Set
    End Property

    Private _Descripcion As String
    Public Property Descripcion() As String
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

Public Class ClaseTopicos
    Implements INotifyPropertyChanged

    Private _Codigo As String
    Public Property Codigo() As String
        Get
            Return _Codigo
        End Get
        Set(ByVal value As String)
            _Codigo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Codigo"))
        End Set
    End Property

    Private _Descripcion As String
    Public Property Descripcion() As String
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

Public Class ClaseRetornos
    Implements INotifyPropertyChanged

    Private _Seleccionado As Boolean
    Public Property Seleccionado() As Boolean
        Get
            Return _Seleccionado
        End Get
        Set(ByVal value As Boolean)
            _Seleccionado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Seleccionado"))
        End Set
    End Property

    Private _ID As Integer
    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ID"))
        End Set
    End Property

    Private _Codigo As String
    Public Property Codigo() As String
        Get
            Return _Codigo
        End Get
        Set(ByVal value As String)
            _Codigo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Codigo"))
        End Set
    End Property

    Private _Descripcion As String
    Public Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Descripcion"))
        End Set
    End Property

    Private _Topico As String
    Public Property Topico() As String
        Get
            Return _Topico
        End Get
        Set(ByVal value As String)
            _Topico = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Topico"))
        End Set
    End Property

    Private _Prioridad As Integer
    Public Property Prioridad() As String
        Get
            Return _Prioridad
        End Get
        Set(ByVal value As String)
            _Prioridad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Prioridad"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class