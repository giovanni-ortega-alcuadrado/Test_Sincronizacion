Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: ClientesViewModel.vb
'Generado el : 07/08/2011 09:34:53
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
Imports A2.OyD.OYDServer.RIA.Web.OYDUtilidades
Imports Microsoft.VisualBasic.CompilerServices
Imports A2Utilidades.Mensajes
Imports System.Threading.Tasks
Imports System.Text.RegularExpressions
Imports System.Collections
Imports A2Utilidades
Imports Telerik.Windows.Data

Public Class ClientesViewModel
    Inherits A2ControlMenu.A2ViewModel
    Public Sub New()
    End Sub

    ''' <history>
    ''' ID caso de prueba:   CP0001
    ''' Descripción:         Desarrollo BRS - Template Calificación Clientes y Contrapartes - SARiC
    ''' Responsable:         Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:               29 de Septiembre/2014
    ''' Pruebas CB:          Jorge Peña (Alcuadrado S.A.) - 29 de Septiembre/2014
    ''' </history>
    Public Sub inicializar()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New ClientesDomainContext()
            'dcProxy1 = New ClientesDomainContext()
            dcProxy2 = New MaestrosDomainContext()
            dcProxy3 = New ClientesDomainContext()
            dcProxy4 = New ClientesDomainContext()
            dcProxy5 = New ClientesDomainContext()
            dcProxy6 = New ClientesDomainContext()
            dcProxy7 = New ClientesDomainContext()
            mdcProxyUtilidad01 = New UtilidadesDomainContext()
            mdcProxyUtilidad02 = New UtilidadesDomainContext()
            mdcProxyUtilidad03 = New UtilidadesDomainContext()
            mdcProxyUtilidad04 = New UtilidadesDomainContext()
            mdcProxyUtilidad05 = New UtilidadesDomainContext()
            mdcProxyUtilidad06 = New UtilidadesDomainContext()
            mdcProxyUtilidad07 = New UtilidadesDomainContext()
            mdcProxyUtilidad08 = New UtilidadesDomainContext()

            objProxy = New UtilidadesDomainContext()

        Else
            dcProxy = New ClientesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            'dcProxy1 = New ClientesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy2 = New MaestrosDomainContext(New System.Uri(Program.RutaServicioMaestros))
            dcProxy3 = New ClientesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy4 = New ClientesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy5 = New ClientesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy6 = New ClientesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            dcProxy7 = New ClientesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            mdcProxyUtilidad02 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            mdcProxyUtilidad03 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            mdcProxyUtilidad04 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            mdcProxyUtilidad05 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            mdcProxyUtilidad06 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            mdcProxyUtilidad07 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            mdcProxyUtilidad08 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
            objProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))

        End If
        DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.ClientesDomainContext.IClientesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 300)
        Try
            HabilitarMenu = False
            If Not Program.IsDesignMode() Then
                IsBusy = True
                dcProxy.Load(dcProxy.ClientesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientes, "FiltroInicial")
                'dcProxy1.Load(dcProxy1.TraerClientePorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesPorDefecto_Completed, "Default")
                objProxy.consultarFechaCierre("O", Program.Usuario, Program.HashConexion, AddressOf consultarFechaCierreCompleted, "")
                CargarCombosViewModel()

                'dcProxy2.Load(dcProxy2.InstalacionFiltrarQuery("",Program.Usuario, Program.HashConexion), AddressOf TerminoTraerInstalacion, "FiltroInicial")
                'objProxy.Verificaparametro("CamposUtilizadosCity",Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "CamposUtilizadosCity")
                'objProxy.Verificaparametro("MANEJOCLIENTESPARACONFIRMAR",Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "MANEJOCLIENTESPARACONFIRMAR")
                'objProxy.Verificaparametro("CALCULADATOSFINANCIEROS",Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "CALCULADATOSFINANCIEROS")
                'objProxy.Verificaparametro("OBLIGATORIOS_DOC_REQUERIDOS",Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "OBLIGATORIOS_DOC_REQUERIDOS")
                'objProxy.Verificaparametro("MANEJAR_PRECLIENTES",Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "MANEJAR_PRECLIENTES")
                'objProxy.Verificaparametro("ACTIVARRETEICA",Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "ACTIVARRETEICA")
                'objProxy.Verificaparametro("VALOR_PARTICIPACION_ACCIONISTAS",Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "VALOR_PARTICIPACION_ACCIONISTAS")
                'objProxy.Verificaparametro("REPLICARCLIENTESAGORA",Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "REPLICARCLIENTESAGORA")
                'objProxy.Verificaparametro("FormaFacturacionComision",Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "FormaFacturacionComision")
                'objProxy.Verificaparametro("APLICADISTRIBRECEPTORES",Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "APLICADISTRIBRECEPTORES")
                'objProxy.Verificaparametro("VALIDARDIGITONIT",Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "VALIDARDIGITONIT")
                'objProxy.Verificaparametro("OBLIGA_SUCURSAL_CLIENTE",Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "OBLIGA_SUCURSAL_CLIENTE")
                ''Santiago Vergara - Octubre 22/2013 - se consulta el valor del parámetro VALIDA_DOC_TIPOPRODUCTO
                ''objProxy.Verificaparametro("VALIDA_DOC_TIPOPRODUCTO",Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "VALIDA_DOC_TIPOPRODUCTO")
                'objProxy.Verificaparametro("TIPOPRODUCTOCLIENTEDEFECTO",Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "TIPOPRODUCTOCLIENTEDEFECTO")
                'objProxy.Verificaparametro("TOTALDIGITOSCELULAR",Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "TOTALDIGITOSCELULAR")
                'objProxy.Verificaparametro("INFOLABORALREQUERIDO",Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "INFOLABORALREQUERIDO")
                'objProxy.Verificaparametro("VALIDACONTRASEÑA_CLIENTE",Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "VALIDACONTRASEÑA_CLIENTE")
                'objProxy.Verificaparametro("PERFILCLIENTEDEFECTO",Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "PERFILCLIENTEDEFECTO")
                'objProxy.Verificaparametro("VALIDAOCUPACIONCONPEP_CLIENTE",Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "VALIDAOCUPACIONCONPEP_CLIENTE")
                objProxy.Load(objProxy.listaVerificaparametroQuery("", "Clientes", Program.Usuario, Program.HashConexion), AddressOf Terminotraerparametrolista, Nothing)
                mdcProxyUtilidad07.ConsultarConsecutivoExistente("MODIFICARECEPTO", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarConsecutivoExistente, Nothing)

                mdcProxyUtilidad08.Load(mdcProxyUtilidad08.PantallasParametrizacion_ConsultarQuery("CLIENTES", Program.Usuario, Program.HashConexion), AddressOf TerminoPantallasParametrizacion_Consultar, Nothing)

                'llenarDiccionario()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "ClientesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Sub Autorefresh()
        Try
            If Not Program.IsDesignMode() Then
                If Not IsNothing(_ClienteSelected) Then
                    ClienteAnterior = Nothing
                    ' Santiago Vergara - Octubre 08/2014 - se ajusta lógica para que haga la consulta dependiendo de si el cliente esta por aprobar o esta aprobado
                    Dim strIDComitente As String = _ClienteSelected.IDComitente

                    If _ClienteSelected.Por_Aprobar Is Nothing Then
                        dcProxy.Clientes.Clear()
                        IsBusy = True
                        dcProxy.Load(dcProxy.ClientesConsultarQuery(1, strIDComitente, Nothing, Nothing, Nothing, Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientes, "BusquedaAutorefrescar")
                    Else
                        dcProxy.Clientes.Clear()
                        IsBusy = True
                        dcProxy.Load(dcProxy.ClientesConsultarQuery(0, strIDComitente, Nothing, Nothing, Nothing, Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientes, "BusquedaAutorefrescar")
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos",
                                 Me.ToString(), "Autorefresh", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#Region "Declaraciones"
    Public Property cb As New CamposBusquedaCliente(Me)
    'Private ClientePorDefecto As OyDClientes.Cliente
    'Private ClienteAnterior As OyDClientes.Cliente
    Dim dcProxy As ClientesDomainContext
    'Dim dcProxy1 As ClientesDomainContext
    Dim dcProxy2 As MaestrosDomainContext
    Dim dcProxy3 As ClientesDomainContext
    Dim dcProxy4 As ClientesDomainContext
    Dim dcProxy5 As ClientesDomainContext
    Dim dcProxy6 As ClientesDomainContext
    Dim dcProxy7 As ClientesDomainContext
    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Private mdcProxyUtilidad02 As UtilidadesDomainContext
    Private mdcProxyUtilidad03 As UtilidadesDomainContext
    Private mdcProxyUtilidad04 As UtilidadesDomainContext
    Private mdcProxyUtilidad05 As UtilidadesDomainContext
    Private mdcProxyUtilidad06 As UtilidadesDomainContext
    Private mdcProxyUtilidad07 As UtilidadesDomainContext
    Private mdcProxyUtilidad08 As UtilidadesDomainContext
    Dim objProxy As UtilidadesDomainContext
    Dim MakeAndCheck As Integer = 0
    Dim fechaCierre As DateTime
    Dim esVersion As Boolean = False, cambioconocimiento As Boolean, plogActivacion = False
    Dim codigo
    Dim fecha As Date
    Dim strciudaddoc, strdepartamentodoc, strpaisdoc, strciudad, strdepartamento, strDocumRepetido,
     strpais, strcodigociiu, strcodigociirepre, strCiudadNacimiento, strProfesion, strObligatoriosDoc, strCodigoReceptorLider, strObligasucursalcliente As String
    Dim logpagocomisiones, LogdistribucionReceptor, logvalidacion, logcambiocuenta, logEnviamail, logValidacionOperaProductos, logfiltroMaker As Boolean
    Dim DicCamposTab As New Dictionary(Of String, Integer)
    Dim ListaDisparosAsync As New Dictionary(Of Integer, String)
    Dim esversionportafolio As Boolean
    Const TAB_PRINCIPAL_GENERAL = 0
    Const TAB_PRINCIPAL_CUSTODIAS = 6
    Const TAB_PRINCIPAL_TESORERIA = 8
    Const TAB_PRINCIPAL_FACTURAS = 9
    Const TAB_PRINCIPAL_SALDOS = 10
    Const TAB_PRINCIPAL_TOTALES = 11
    Const TAB_PRINCIPAL_ENCARGOS = 12
    Const TAB_PRINCIPAL_LIQXCUMPLIR = 13
    Const TAB_PRINCIPAL_REPO = 14
    Const TAB_PRINCIPAL_FONDOS = 15
    Const TAB_PRINCIPAL_DIVISAS = 16
    Const TAB_PRINCIPAL_DEPOSITO = 2
    Const TAB_PRINCIPAL_ORDENES = 3
    Const TAB_PRINCIPAL_ACCIONES = 4
    Const TAB_PRINCIPAL_PORTAFOLIO = 1
    Const TAB_PRINCIPAL_RENTAFIJA = 5
    Const TAB_PRINCIPAL_VENCIMIENTO = 7
    Const TAB_PRINCIPAL_GENERAL_GENERAL = 0
    Const TAB_PRINCIPAL_GENERAL_FINANCIERO = 1
    Const TAB_PRINCIPAL_GENERAL_RECEPTORES = 2
    Const TAB_PRINCIPAL_GENERAL_ORDENANTES = 3
    Const TAB_PRINCIPAL_GENERAL_BENEFICIARIOS = 4
    Const TAB_PRINCIPAL_GENERAL_FICHA = 5
    Const TAB_PRINCIPAL_GENERAL_UBICACION = 6
    Const TAB_PRINCIPAL_GENERAL_MERCADEO = 7
    Const TAB_PRINCIPAL_GENERAL_INFADICIONAL = 8
    Const TAB_PRINCIPAL_GENERAL_DOCUMRECIBIDOS = 9
    Const TAB_PRINCIPAL_GENERAL_IR = 12
    Const TAB_PRINCIPAL_GENERAL_PERSONASPORCONFIRMAR = 13
    Const TAB_PRINCIPAL_GENERAL_FINANCIERO_GENERAL = 0
    Const TAB_PRINCIPAL_GENERAL_FINANCIERO_TIPOSENTIDAD = 1
    Const TAB_PRINCIPAL_GENERAL_FINANCIERO_CTASBANCARIAS = 2
    Const TAB_PRINCIPAL_GENERAL_FINANCIERO_OPTEXTRANJERA = 3
    Const TAB_PRINCIPAL_GENERAL_FINANCIERO_CLASIFICACIONES = 4
    Const TAB_PRINCIPAL_GENERAL_FINANCIERO_FATCA = 5
    Const TAB_PRINCIPAL_GENERAL_FINANCIERO_FATCAREPRESENTANTE = 6
    Const CONST_TIPODEPERSONA_JURIDICA = 2
    Const CONST_TIPODEIDENTIFICACION_NIT = "N"
    Dim Contadorconocimientos, mintGrupoPorDefecto, mintSubGrupoPorDefecto, intDocummenor, intClientesActivos, intparticipacionaccionistas, intCuenta, SiguenteSuc, intClienteRelacionados As Integer
    Dim ConfiguraFirma As String = String.Empty
    Dim strMensajeinactivacion As String = String.Empty
    Dim NombreAnteriorAuto As String = String.Empty
    Dim ClientesAccionistasNombre1 As String = String.Empty
    Dim ClientesAccionistasNombre2 As String = String.Empty
    Dim ClientesAccionistasApellido1 As String = String.Empty
    Dim ClientesAccionistasApellido2 As String = String.Empty

    Dim ClientesBeneficiarioNombre1 As String = String.Empty
    Dim ClientesBeneficiarioNombre2 As String = String.Empty
    Dim ClientesBeneficiarioApellido1 As String = String.Empty
    Dim ClientesBeneficiarioApellido2 As String = String.Empty

    Dim ClientesDepEconomicaNombre1 As String = String.Empty
    Dim ClientesDepEconomicaNombre2 As String = String.Empty
    Dim ClientesDepEconomicaApellido1 As String = String.Empty
    Dim ClientesDepEconomicaApellido2 As String = String.Empty

    Dim ClientesPersonaNombre1 As String = String.Empty
    Dim ClientesPersonaNombre2 As String = String.Empty
    Dim ClientesPersonaApellido1 As String = String.Empty
    Dim ClientesPersonaApellido2 As String = String.Empty


    Dim Nombre1AccAnterior As String = String.Empty
    Dim Nombre2AccAnterior As String = String.Empty
    Dim configuraloghir, configuraconocimiento, logvalorparametro, mlogGrabarListaClinton, logPerteneceConsecutivo As Boolean
    Dim curcomisioncompras, curcomisionventas, curoperacionventas, curoperacioncompras, curtotaloperacion, curtotalcomision, CursaldoTotal, Cursaldomesanterior, mcdblSalariominimo As Double
    Dim validafiltro, Busquedavacia, TerminoConsultainhabil, logReplicaAgora, logvalidoreplicacuenta, logcambiotitularcuenta, logvalidoreplicadirecc, logvalidoreplicabene As Boolean
    Dim Preclientesf As Preclientes
    Private DatosListaClinton As New List(Of ClienteInhabilitado)
    Private DatosListaClintonselected As New ClienteInhabilitado
    Dim Consultadirecciones As ConsultaDescripcionDirecciones
    Dim Cuentasanterior As New CuentasClienteclase
    Dim Direccionesanterior As New ClientesDireccioneclase
    Dim Beneficiarioanterior As New ClientesBeneficiariosclase
    Dim logValidarDigitoVerificacion As Boolean = False
    Dim logPasoValidacionDigito As Boolean = True
    Dim strDigitoPermitido As String = String.Empty
    'Santiago Vergara - Octubre 22/2013 - Variable para almacenar si se valida o no el tipo de producto
    'Dim logValidarTipoProducto As Boolean = False
    'Dim objParametros As List(Of OYDUtilidades.ItemCombo) = Nothing
    Dim TIPOPRODUCTOCLIENTEDEFECTO As String = ""
    Dim TOTALDIGITOSCELULAR As String = ""
    Dim INFOLABORALREQUERIDO As String = ""
    Dim VALIDACONTRASEÑA_CLIENTE As String = ""
    Dim PERFILCLIENTEDEFECTO As String = ""
    Dim VALIDAOCUPACIONCONPEP_CLIENTE As String = ""
    Dim VALIDA_VALORACTIVO As String = ""
    Dim strdescripcionperfil As String = ""
    Dim SEPARADORMAIL As String = ","
    Dim PORCENTAJE_CERCANIA_SEGUNDO_MENSAJE As Integer = 0
    Dim MOSTRARMENSAJEADICIONALCLIENTES As String
    'SLB20131125
    Dim logTerminoCargarClasificaciones As Boolean = False
    Dim ListaCombosEspecificos As List(Of OYDUtilidades.ItemCombo)
    'JBT
    Dim expresionemail As String = Program.ExpresionRegularEmail
    'Santiago Vergara - Junio 20/2014 - Varialbles para almacenar valores de los parámetros de repricación
    Private REPLICAR_CLIENTES As Boolean = False
    Private REPLICAR_FONDOS As Boolean = False
    Private REPLICAR_MERCAMSOFT As Boolean = False
    Private REPLICAR_PORTAFOLIOS As Boolean = False
    Private VALIDA_TIPOIDENTIFICACIONNACIMIENTO As Boolean = True

    'NAOC20141118 - Fatca
    '**********************************************************************
    Private FATCA_CAMPOSADICIONALES As Boolean = False
    '**********************************************************************
    'Santiago Mazo Padierna - Septiembre 07/2016 - Variable para desarrollo Listas restrictivas
    Dim PORCENTAJE_CERCANIA_MAYOR As Integer = 0
    Dim PORCENTAJE_CERCANIA_MENOR As Integer = 0
    Dim ClienteAnuladoInhabil As Boolean = False
    Dim ACTIVAR_AUTORIZACION_INHABIL As String = ""
    '**********************************************************************
    'Santiago Mazo Padierna - Septiembre 28/2016 - Variable almacenar el  código del tipo de intermediario asignado a “Cliente extranjero”
    Dim CODIGO_INTERCLIENTEXTRANJERO As Integer = 0
    '**********************************************************************
    'Santiago Mazo Padierna - Diciembre 19/2017 - Variable almacenar el parametro para activar la creación automática de el relacionamiento de cuentas 	
    Dim PATRIMONIOAUTONOMO_ACTIVAR As String = ""
    '**********************************************************************
    Dim MostrarClienteFondos As Visibility = Visibility.Visible
    Dim logResumido As Boolean = True
    Dim logPasarAForma As Boolean = True
    Public viewClientes As ClientesView
    Dim logEsNuevoRegistro As Boolean = False
    Dim logValidarDocumentosIDSucursal As Boolean = True


#End Region
#Region "Resultados Asincrónicos"
#Region "Tablas Hijas"
    Private Sub TerminoTraerCuentasclientes(ByVal lo As LoadOperation(Of OyDClientes.CuentasCliente))
        If Not lo.HasError Then
            ListaCuentasClientes = dcProxy.CuentasClientes
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Cuentasclientes",
                                             Me.ToString(), "TerminoTraerCuentasclientes", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerClientesReceptores(ByVal lo As LoadOperation(Of OyDClientes.ClientesReceptore))
        If Not lo.HasError Then
            ListaClientesReceptore = dcProxy.ClientesReceptores
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesReceptores",
                                             Me.ToString(), "TerminoTraerClientesReceptores", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerClientesOrdenantes(ByVal lo As LoadOperation(Of OyDClientes.ClientesOrdenante))
        If Not lo.HasError Then
            ListaClientesOrdenantes = dcProxy.ClientesOrdenantes
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesOrdenantes",
                                             Me.ToString(), "TerminoTraerClientesOrdenantes", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerClientesBeneficiarios(ByVal lo As LoadOperation(Of OyDClientes.ClientesBeneficiarios))
        If Not lo.HasError Then
            ListaClientesBeneficiarios = dcProxy.ClientesBeneficiarios
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesBeneficiarios",
                                             Me.ToString(), "TerminoTraerClientesBeneficiarios", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerClientesAficiones(ByVal lo As LoadOperation(Of OyDClientes.ClientesAficiones))
        If Not lo.HasError Then
            ListaClientesAficiones = dcProxy.ClientesAficiones
            'For Each ll In dcProxy.ClientesAficiones

            '    Listaclientesaficionesclase.Add(New ListaoriginalAficiones With {.IDComitente = ll.IDComitente, .IDSucCliente = ll.IDSucCliente, .Descripcion = ll.Descripcion, .Retorno = ll.Retorno, .Usuario = ll.Usuario, .Seleccion = ll.Seleccion,
            '                                                                     .IDClientesAficiones = ll.IDClientesAficiones, .InfoSesion = ll.InfoSesion})
            'Next
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesAficiones",
                                             Me.ToString(), "TerminoTraerClientesAficiones", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerClientesDeportes(ByVal lo As LoadOperation(Of OyDClientes.ClientesDeportes))
        If Not lo.HasError Then
            ListaClientesDeportes = dcProxy.ClientesDeportes
            'For Each ll In dcProxy.ClientesDeportes

            '    Listaclientesdeportesclase.Add(New ListaOriginalDeportes With {.IDComitente = ll.IDComitente, .IDSucCliente = ll.IDSucCliente, .Descripcion = ll.Descripcion, .Retorno = ll.Retorno, .Usuario = ll.Usuario, .Seleccion = ll.Seleccion,
            '                                                                     .IDClientesDeportes = ll.IDClientesDeportes, .InfoSesion = ll.InfoSesion})
            'Next
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesDeportes",
                                             Me.ToString(), "TerminoTraerClientesDeportes", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminoTraerClientesDirecciones(ByVal lo As LoadOperation(Of OyDClientes.ClientesDireccione))
        If Not lo.HasError Then
            ListaClientesDirecciones = dcProxy.ClientesDirecciones
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesDirecciones",
                                             Me.ToString(), "TerminoTraerClientesDirecciones", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminoTraerClientesDocumentosRequeridos(ByVal lo As LoadOperation(Of OyDClientes.ClientesDocumentosRequeridos))
        If Not lo.HasError Then
            ListaClientesDocumentosRequeridos = dcProxy.ClientesDocumentosRequeridos
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesDocumentosRequeridos",
                                             Me.ToString(), "TerminoTraerClientesDocumentosRequeridos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerClientesLOGHistoriaI(ByVal lo As LoadOperation(Of OyDClientes.ClientesLOGHistoriaIR))
        If Not lo.HasError Then
            ListaClientesLOGHistoriaI = dcProxy.ClientesLOGHistoriaIRs
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesLOGHistoriaI",
                                             Me.ToString(), "TerminoTraerClientesLOGHistoriaI", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerClientesTipoCliente(ByVal lo As LoadOperation(Of OyDClientes.TipoClient))
        If Not lo.HasError Then
            ListaClientesTipoCliente = dcProxy.TipoClients
            If ListaClientesTipoCliente.Count = 0 Then
                ClientesSeVisibility.VisivilidadMenu = Visibility.Visible
                ClientesTipoClienteSelected = ListaClientesTipoCliente.FirstOrDefault
            Else
                ClientesSeVisibility.VisivilidadMenu = Visibility.Collapsed
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesTipoCliente",
                                             Me.ToString(), "TerminoTraerClientesTipoCliente", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    ''' <history>
    ''' Descripción:    Método que alimenta la lista ListaClientesProductos.
    ''' Responsable:    Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:          29 de Septiembre/2014
    ''' Pruebas CB:     Jorge Peña (Alcuadrado S.A.) - 29 de Septiembre/2014
    ''' </history>
    Private Sub TerminoConsultarClientesProductos(ByVal lo As LoadOperation(Of OyDClientes.ClientesProductos))
        If Not lo.HasError Then
            ListaClientesProductos = dcProxy.ClientesProductos
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesProductos",
                                             Me.ToString(), "TerminoConsultarClientesProductos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerClientesConocimientoEspecifico(ByVal lo As LoadOperation(Of OyDClientes.ClientesConocimientoEspecifico))
        If Not lo.HasError Then
            Contadorconocimientos = dcProxy.ClientesConocimientoEspecificos.Count + Contadorconocimientos
            If Contadorconocimientos = dcProxy.ClientesConocimientoEspecificos.Count Then
                ListaClientesConocimientoEspecifico = dcProxy.ClientesConocimientoEspecificos
                For Each ll In dcProxy.ClientesConocimientoEspecificos

                    ListaClientesConocimientoEspecificoclase.Add(New ListaOriginalClientesConocimientoEspecifico With {.ID = ll.ID, .CodigoConocimiento = ll.CodigoConocimiento, .Usuario = ll.Usuario, .DescripcionConocimiento = ll.DescripcionConocimiento, .Conocimiento = ll.Conocimiento, .InfoSesion = ll.InfoSesion,
                                                                                     .IDClientesConocimiento = ll.IDClientesConocimiento, .ConocimientoOriginal = ll.Conocimiento})
                Next
            End If

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesConocimientoEspecifico",
                                             Me.ToString(), "TerminoTraerClientesConocimientoEspecifico", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub


    Private Sub TerminoTraerPortafolio(ByVal lo As LoadOperation(Of OyDClientes.Portafolio))
        ClientesSeBoolean.IsbusyPortafolio = False
        If Not lo.HasError Then
            If Not IsNothing(dcProxy.Portafolios.ToList.Item(5).IDComitente) Then
                If dcProxy.Portafolios.ToList.Item(5).IDComitente = ClienteSelected.IDComitente Then
                    If dcProxy.Portafolios.ToList.Item(5).EspeciesNoValorizadas = String.Empty Then
                        ClientesSeVisibility.labelvisible = Visibility.Collapsed
                        'Listaportafolio.Add(dcProxy.Portafolios.ToList.Item(0))
                        'Listaportafolio.Add(dcProxy.Portafolios.ToList.Item(1))
                        'Listaportafolio.Add(dcProxy.Portafolios.ToList.Item(2))
                        'Listaportafolio.Add(dcProxy.Portafolios.ToList.Item(3))
                        'Listaportafolio.Add(dcProxy.Portafolios.ToList.Item(4))
                        Listaportafolio = dcProxy.Portafolios.ToList
                        Listaportafolio.Remove(Listaportafolio.Item(5))
                    Else
                        textoPortafolio = "Las especies:" & vbCrLf & dcProxy.Portafolios.ToList.Item(5).EspeciesNoValorizadas & vbCrLf &
                            "A la Fecha de corte:" & dcProxy.Portafolios.ToList.Item(5).FechaCorteHabil & ", No están valorizadas"
                        ClientesSeVisibility.labelvisible = Visibility.Visible
                        Listaportafolio.Clear()
                        MyBase.CambioItem("Listaportafolio")
                    End If
                Else
                    dcProxy.Portafolios.Clear()
                    Exit Sub
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Portafolio",
                                             Me.ToString(), "TerminoTraerPortafolio", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminotraerClientestesoreria(ByVal lo As LoadOperation(Of OyDClientes.ClientesTesoreria))
        If Not lo.HasError Then
            ListaTesoreria = dcProxy.ClientesTesorerias.ToList
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Tesoreria",
                                             Me.ToString(), "TerminotraerClientestesoreria", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminotraerClientesCustodias(ByVal lo As LoadOperation(Of OyDClientes.ClientesCustodias))
        IsBusyCustodias = False
        If Not lo.HasError Then
            ListaCustodias = dcProxy.ClientesCustodias.ToList
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de custodias",
                                             Me.ToString(), "TerminotraerClientesCustodias", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminotraerClientesVencimientos(ByVal lo As LoadOperation(Of OyDClientes.ClientesVencimientos))
        If Not lo.HasError Then
            ListaVencimientos = dcProxy.ClientesVencimientos.ToList
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de vencimientos",
                                             Me.ToString(), "TerminotraerClientesVencimientos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminotraerClientesLiqxCumplir(ByVal lo As LoadOperation(Of OyDClientes.ClientesLiqxCumplir))
        If Not lo.HasError Then
            ListaLiqxCumplir = dcProxy.ClientesLiqxCumplirs.ToList
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de liqxcumplir",
                                             Me.ToString(), "TerminotraerClientesLiqxCumplir", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminotraerClientesRepos(ByVal lo As LoadOperation(Of OyDClientes.ClientesRepo))
        If Not lo.HasError Then
            ListaRepos = dcProxy.ClientesRepos.ToList
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Repos",
                                             Me.ToString(), "TerminotraerClientesRepos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminotraerClientesFondos(ByVal lo As LoadOperation(Of OyDClientes.ClientesFondos))
        If Not lo.HasError Then
            ListaFondos = lo.Entities.ToList
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Fondos",
                                             Me.ToString(), "TerminotraerClientesFondos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusyFondos = False
    End Sub
    Private Sub TerminotraerClientesFondosTotales(ByVal lo As LoadOperation(Of OyDClientes.ClientesFondosTotales))
        If Not lo.HasError Then
            ListaFondosTotales = lo.Entities.ToList

            dcProxy.ClientesFondos.Clear()
            dcProxy.Load(dcProxy.TraerSaldoClientesFondosDetalladoQuery(ClienteSelected.IDComitente, FechaCorteFondos, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesFondos, Nothing)
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Fondos Totales", Me.ToString(), "TerminotraerClientesFondosTotales", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
            IsBusyFondos = False
        End If
    End Sub
    Private Sub TerminotraerClientesDeposito(ByVal lo As LoadOperation(Of OyDClientes.CuentasDeposito))
        If Not lo.HasError Then
            ListaCuentadeposito = dcProxy.CuentasDepositos.ToList
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Depositos",
                                             Me.ToString(), "TerminotraerClientesDeposito", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminotraerClientesAcciones(ByVal lo As LoadOperation(Of OyDClientes.ClientesAcciones))
        If Not lo.HasError Then
            ListaClientesAcciones = dcProxy.ClientesAcciones.ToList
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Acciones",
                                             Me.ToString(), "TerminotraerAcciones", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminotraerClientesRentafija(ByVal lo As LoadOperation(Of OyDClientes.ClientesRentaFija))
        If Not lo.HasError Then
            ListaClientesRentaFija = dcProxy.ClientesRentaFijas.ToList
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Acciones",
                                             Me.ToString(), "TerminotraerClientesRentafija", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminotraerClientesOrdenes(ByVal lo As LoadOperation(Of OyDClientes.ClientesOrdenes))
        If Not lo.HasError Then
            ListaClientesOrdenes = dcProxy.ClientesOrdenes.ToList
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Ordenes",
                                             Me.ToString(), "TerminotraerClientesOrdenes", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminotraerClientesFactura(ByVal lo As LoadOperation(Of OyDClientes.FacturasCli))
        If Not lo.HasError Then
            ListaClientesFactura = dcProxy.FacturasClis.ToList
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de facturas",
                                             Me.ToString(), "TerminotraerClientesFactura", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminotraerClientesDivisas(ByVal lo As LoadOperation(Of OyDClientes.Divisas))
        If Not lo.HasError Then
            ListaClientesDivisas = dcProxy.Divisas.ToList
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Divisas",
                                             Me.ToString(), "TerminotraerClientesDivisas", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminotraerClientesTotales(ByVal lo As LoadOperation(Of OyDClientes.ClientesTotales))
        If Not lo.HasError Then
            Try
                Dim intAnno As Integer
                Dim intMes As Integer
                Dim li As Object
                Dim listaopcom = dcProxy.ClientesTotales.ToList
                Dim lista = dcProxy.ClientesTotales.ToList
                While lista.Any
                    li = lista.FirstOrDefault
                    curcomisioncompras = 0
                    curoperacioncompras = 0
                    curcomisionventas = 0
                    curoperacionventas = 0
                    curtotalcomision = 0
                    curtotaloperacion = 0
                    If li.Y = Now.Year Then
                        intAnno = li.Y
                        intMes = li.M
                        For Each ll In listaopcom
                            If ll.M = intMes And ll.Y = intAnno Then
                                Select Case (ll.Tipo)
                                    Case "R", "C"
                                        curcomisioncompras = curcomisioncompras + ll.Comisiones
                                        curoperacioncompras = curoperacioncompras + ll.Operaciones
                                    Case "V", "S"
                                        curcomisionventas = curcomisionventas + ll.Comisiones
                                        curoperacionventas = curoperacionventas + ll.Operaciones
                                End Select
                                curtotalcomision = curcomisioncompras + curcomisionventas
                                curtotaloperacion = curoperacioncompras + curoperacionventas

                                lista.Remove(ll)

                            End If

                        Next

                        ListaClseTotales.Add(New ClientesTotalesgroup With {.Periodo = Mes(intMes) + "-" + intAnno.ToString, .Compras = curoperacioncompras, .Ventas = curoperacionventas, .Totales = curtotaloperacion, .Categoria = "Operaciones"})
                        ListaClseTotales.Add(New ClientesTotalesgroup With {.Periodo = Mes(intMes) + "-" + intAnno.ToString, .Compras = curcomisioncompras, .Ventas = curcomisionventas, .Totales = curtotalcomision, .Categoria = "Comisiones"})
                    Else
                        intAnno = li.Y
                        For Each ll In listaopcom
                            If ll.Y = intAnno Then
                                Select Case (ll.Tipo)
                                    Case "R", "C"
                                        curcomisioncompras = curcomisioncompras + ll.Comisiones
                                        curoperacioncompras = curoperacioncompras + ll.Operaciones
                                    Case "V", "S"
                                        curcomisionventas = curcomisionventas + ll.Comisiones
                                        curoperacionventas = curoperacionventas + ll.Operaciones
                                End Select
                                curtotalcomision = curcomisioncompras + curcomisionventas
                                curtotaloperacion = curoperacioncompras + curoperacionventas

                                lista.Remove(ll)

                            End If
                        Next
                        ListaClseTotales.Add(New ClientesTotalesgroup With {.Periodo = intAnno.ToString, .Compras = curoperacioncompras, .Ventas = curoperacionventas, .Totales = curtotaloperacion, .Categoria = "Operaciones"})
                        ListaClseTotales.Add(New ClientesTotalesgroup With {.Periodo = intAnno.ToString, .Compras = curcomisioncompras, .Ventas = curcomisionventas, .Totales = curtotalcomision, .Categoria = "Comisiones"})
                    End If
                End While

                ClientesTotalespaged = New PagedCollectionView(ListaClseTotales)
                ClientesTotalespaged.GroupDescriptions.Clear()
                ClientesTotalespaged.GroupDescriptions.Add(New PropertyGroupDescription("Categoria"))
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de los totales",
                                          Me.ToString(), "TerminotraerClientesTotales", Application.Current.ToString(), Program.Maquina, ex)
                lo.MarkErrorAsHandled()
            End Try

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de los totales",
                                             Me.ToString(), "TerminotraerClientesTotales", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Function Mes(ByVal intmes As Integer) As String
        Mes = ""
        Select Case intmes
            Case 1
                Mes = "Enero"
            Case 2
                Mes = "Febrero"
            Case 3
                Mes = "Marzo"
            Case 4
                Mes = "Abril"
            Case 5
                Mes = "Mayo"
            Case 6
                Mes = "Junio"
            Case 7
                Mes = "Julio"
            Case 8
                Mes = "Agosto"
            Case 9
                Mes = "Septiembre"
            Case 10
                Mes = "Octubre"
            Case 11
                Mes = "Noviembre"
            Case 12
                Mes = "Diciembre"

        End Select
        Return Mes
    End Function
    Private Sub TerminotraerClientesSaldos(ByVal lo As LoadOperation(Of OyDClientes.ClientesSaldos))
        If Not lo.HasError Then
            Try
                'CFMA20181008 s - 38214 Agregamos código para que al momento de cambiar de comitente se recargue correctamente la consulta en la pantalla
                If Not IsNothing(_ClienteSelected) Then
                    If lo.UserState = _ClienteSelected.IDComitente Then
                        ListaSaldos.Clear()
                        'CFMA20181008
                        Dim intAnno As Integer
                        Dim intMes As Integer
                        Dim li As Object
                        Dim valor As String
                        Dim listaopcom = dcProxy.ClientesSaldos.ToList
                        Dim lista = dcProxy.ClientesSaldos.ToList
                        While lista.Any
                            li = lista.FirstOrDefault

                            intAnno = li.lngAno
                            intMes = li.lngMes
                            For Each ll In listaopcom
                                If ll.lngMes = intMes And ll.lngAno = intAnno Then
                                    CursaldoTotal = ll.saldo
                                    lista.Remove(ll)
                                End If
                            Next
                            If intMes = Now.Month And intAnno = Now.Year Then
                                valor = "Actual"
                            Else
                                valor = Mes(intMes) + "-" + intAnno.ToString
                            End If
                            If CursaldoTotal < 0 Then
                                ListaSaldos.Add(New ClientesSaldosG With {.Periodo = valor, .Acargo = 0, .Afavor = CursaldoTotal * -1})

                            Else
                                ListaSaldos.Add(New ClientesSaldosG With {.Periodo = valor, .Acargo = CursaldoTotal, .Afavor = 0})

                            End If


                        End While

                        If ListaSaldos.Count > 0 Then
                            If ListaSaldos.Last.Periodo.Contains("-") Then
                                Cursaldomesanterior = ListaSaldos.Last.Acargo
                                valor = "Actual"
                                ListaSaldos.Add(New ClientesSaldosG With {.Periodo = valor, .Acargo = Cursaldomesanterior, .Afavor = 0})

                            End If
                        Else
                            valor = "Actual"
                            ListaSaldos.Add(New ClientesSaldosG With {.Periodo = valor, .Acargo = 0, .Afavor = 0})

                        End If
                    End If 'CFMA20181008
                End If   'CFMA20181008
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de los saldos",
                                          Me.ToString(), "TerminotraerClientesSaldos", Application.Current.ToString(), Program.Maquina, ex)
                lo.MarkErrorAsHandled()
            End Try

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de los saldos",
                                             Me.ToString(), "TerminotraerClientesSaldos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminotraerClientesEncargos(ByVal lo As LoadOperation(Of OyDClientes.ClientesEncargos))
        If Not lo.HasError Then
            ListaEncargos = dcProxy.ClientesEncargos.ToList
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de encargos.",
                                             Me.ToString(), "TerminotraerClientesEncargos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminoTraerClientesAccionistas(ByVal lo As LoadOperation(Of OyDClientes.ClientesAccionistas))
        If Not lo.HasError Then
            ListaClientesAccionistas = dcProxy.ClientesAccionistas
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesAccionistas",
                                             Me.ToString(), "TerminoTraerClientesAccionistas", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminoTraerClientesFicha(ByVal lo As LoadOperation(Of OyDClientes.ClientesFicha))
        If Not lo.HasError Then
            ListaClientesFicha = dcProxy.ClientesFichas
            'If cargandoaccionistas = True And cargandonuevo = True Then
            ' ListaClientesFicha.Clear() 
            'cargandoaccionistas = False  
            'End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesFichas",
                                             Me.ToString(), "TerminoTraerClientesFicha", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminoTraerClientesPersonas(ByVal lo As LoadOperation(Of OyDClientes.ClientesPersonasParaConfirmar))
        If Not lo.HasError Then
            ListaClientesPersonas = dcProxy.ClientesPersonasParaConfirmars
            'If cargandobeneficiario = True And cargandonuevo = True Then
            '    ListaClientesBeneficiarios.Clear()
            '    cargandobeneficiario = False
            'End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesPersonas",
                                             Me.ToString(), "TerminoTraerClientesPersonas", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminoTraerClientesDepEconomica(ByVal lo As LoadOperation(Of OyDClientes.ClientesPersonasDepEconomica))
        If Not lo.HasError Then
            ListaClientesDepEconomica = dcProxy.ClientesPersonasDepEconomicas
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesDepEconomica",
                                             Me.ToString(), "TerminoTraerClientesDepEconomica", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminotraerCodigoOtrossistemas(ByVal lo As LoadOperation(Of CodigosOtrosSistema))
        If Not lo.HasError Then
            IsBusy = False
            intCuenta = dcProxy2.CodigosOtrosSistemas.Count
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesDepEconomica",
                                             Me.ToString(), "TerminoTraerClientesDepEconomica", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminoTraerClientesPaisesFatca(ByVal lo As LoadOperation(Of OyDClientes.ClientesPaisesFATCA))
        If Not lo.HasError Then
            ListaClientesPaisesFATCA = dcProxy.ClientesPaisesFATCAs
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ClientesPaisesFatca",
                                             Me.ToString(), "TerminoTraerClientesPaisesFatca", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
#End Region
    'Private Sub TerminoTraerClientesPorDefecto_Completed(ByVal lo As LoadOperation(Of OyDClientes.Cliente))
    '    If Not lo.HasError Then
    '        ClientePorDefecto = lo.Entities.FirstOrDefault
    '    Else
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Cliente por defecto", _
    '                                         Me.ToString(), "TerminoTraerClientePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
    '        lo.MarkErrorAsHandled()   '????
    '    End If
    'End Sub
    Private Sub TerminoTraerClientes(ByVal lo As LoadOperation(Of OyDClientes.Cliente))
        If Not lo.HasError Then
            ListaClientes = dcProxy.Clientes
            If dcProxy.Clientes.Count > 0 Then
                If MakeAndCheck = 1 And lo.UserState = Nothing Then
                    'ClienteSelected = (From sl In ListaClientes Where sl.IDComitente.Equals(codigo) Select sl).First
                    If logfiltroMaker = True Then
                        ClienteSelected = ListaClientes.Where(Function(ic) ic.IDComitente.Equals(codigo) And ic.MakeAndCheck Is Nothing).First
                    Else
                        ClienteSelected = ListaClientes.Where(Function(ic) ic.IDComitente.Equals(codigo)).First
                    End If
                End If
                If Not IsNothing(ClienteSelected.MakeAndCheck) Then
                    MakeAndCheck = ClienteSelected.MakeAndCheck
                End If
                If lo.UserState = "insert" Then
                    ClienteSelected = ListaClientes.First
                End If
                If MakeAndCheck = 1 Then
                    If ClienteSelected.Por_Aprobar <> Nothing Then
                        If ClienteSelected.Estado.Equals("Ingreso") Then
                            ClientesSeBoolean.visible = False
                        Else
                            ClientesSeBoolean.visible = True
                        End If
                        ClientesSeBoolean.visibleap = True
                        content = "por aprobar"
                    Else
                        content = "aprobada"
                        If ClienteSelected.Por_Aprobar = Nothing And lo.UserState <> Nothing Then
                            ClientesSeBoolean.visible = False
                            ClientesSeBoolean.visibleap = False
                        End If
                    End If
                End If

                If logPasarAForma Then
                    MyBase.CambiarFormulario_Forma_Manual()
                    logPasarAForma = False
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "BusquedaAutorefrescar" Or lo.UserState = "FiltroVM" Then
                    'MessageBox.Show("No se encontro ningún registro")
                    If lo.UserState <> "BusquedaAutorefrescar" Then
                        A2Utilidades.Mensajes.mostrarMensaje("No se encontro ningún registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If

                Busquedavacia = True
                ClienteSelected = Nothing
            End If
            'JFSB 20171008 Se agrega propiedad para habilitar los botones de menú
            HabilitarMenu = True
        Else
            'JFSB 20171008 Se agrega propiedad para habilitar los botones de menú
            HabilitarMenu = True
            A2ComunesControl.FuncionesCompartidas.obtenerMensajeValidacionErrorPersonalizado("Se presentó un problema en la obtención de la lista de Clientes", Me.ToString, "TerminoTraerEspecie", lo.Error)
            lo.MarkErrorAsHandled()
            'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Clientes", _
            '                                 Me.ToString(), "TerminoTraerCliente", Application.Current.ToString(), Program.Maquina, lo.Error)
            'lo.MarkErrorAsHandled()   '????
            IsBusy = False
        End If
        If lo.UserState <> "FiltroInicial" Then
            IsBusy = False
        End If

        IsBusy = False
    End Sub
    Private Sub TerminoValidarEdicion(ByVal lo As LoadOperation(Of OyDClientes.Cliente))
        Try
            If Not lo.HasError Then
                If dcProxy3.Clientes.Count > 0 Then
                    MyBase.RetornarValorEdicionNavegacion()
                    A2Utilidades.Mensajes.mostrarMensaje("El cliente no se puede editar porque tiene una versión pendiente por aprobar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    Editar()
                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo obtener los clientes por aprobar.",
                                     Me.ToString(), "TerminoValidarEdicion", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo obtener los clientes por aprobar.",
                                     Me.ToString(), "TerminoValidarEdicion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub TerminoValidarborrado(ByVal lo As LoadOperation(Of OyDClientes.Cliente))
        Try

            If Not lo.HasError Then
                If dcProxy3.Clientes.Count > 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("El cliente no se puede inactivar porque tiene una versión pendiente por aprobar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Else
                    borrar()
                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo obtener los clientes por aprobar.",
                                     Me.ToString(), "TerminoValidarEdicion", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se pudo obtener los clientes por aprobar.",
                                     Me.ToString(), "TerminoValidarEdicion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    'Private Sub TerminoTraerInstalacion(ByVal lo As LoadOperation(Of Instalacio))
    '    If Not lo.HasError Then
    '        ListaInstalacion = dcProxy2.Instalacios
    '        If dcProxy2.Instalacios.Count > 0 Then
    '            InstalacioSelected = ListaInstalacion.Last
    '            If Not String.IsNullOrEmpty(InstalacioSelected.ServidorBus) And Not String.IsNullOrEmpty(InstalacioSelected.BaseDatosBus) And Not String.IsNullOrEmpty(InstalacioSelected.OwnerBus) Then
    '                ClientesSeVisibility.TienebusVisible = Visibility.Visible
    '            End If
    '        End If
    '    Else
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Instalacion", _
    '                                         Me.ToString(), "TerminoTraerInstalacion", Application.Current.ToString(), Program.Maquina, lo.Error)
    '        lo.MarkErrorAsHandled()   '????
    '        IsBusy = False
    '    End If
    '    'IsBusy = False
    'End Sub
    Private Sub Terminoinactivo(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            'Consulta si el cliente es ordenante los clientes activos de este 
            If ClienteSelected.TipoVinculacion <> "C" Then
                intClientesActivos = 0
                dcProxy.ConsultaClientesActivos(ClienteSelected.IDComitente, intClientesActivos, Program.Usuario, Program.HashConexion, AddressOf TerminoTraerClientesActivos, Nothing)
            End If
            'C1.Silverlight.C1MessageBox.Show("Debes ingresar la causa de la inactivacion en la pestaña general  y Grabar ", Program.TituloSistema)
            TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_GENERAL
            ClienteSelected.InactivaItem = True
            ClienteSelected.Concepto = Nothing
            ClienteSelected.IDConcepto = Nothing
            ClienteSelected.EstadoCliente = "I"
            ClienteSelected.Activo = False
            ClienteSelected.Usuario = Program.Usuario
            Fechadesplegada = Date.Now
            Editando = True
            ClientesSeBoolean.Editarcampos = False
            ClientesSeBoolean.EditaInactividad = True
            'SV20141007AJUSTEINCATIVIDAD 
            ClientesSeVisibility.VisibleConceptoActividad = Visibility.Collapsed
            ClientesSeVisibility.VisibleConceptoInactividad = Visibility.Visible

            If visNavegando = "Collapsed" Then
                MyBase.CambiarFormulario_Forma_Manual()
            End If
        Else
            ClienteSelected.InactivaItem = False
        End If
    End Sub
    Private Sub Terminoactivo(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            Try
                'plogActivacion = True
                'Consulta si el cliente es ordenante los clientes activos de este 
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_GENERAL
                ClienteSelected.InactivaItem = True
                ClienteSelected.Concepto = Nothing
                ClienteSelected.IDConcepto = Nothing
                ClienteSelected.EstadoCliente = "A"
                ClienteSelected.Activo = True
                ClienteSelected.Usuario = Program.Usuario
                Fechadesplegada = Date.Now
                Editando = True
                ClientesSeBoolean.Editarcampos = False
                ClientesSeBoolean.EditaInactividad = True
                'SV20141007AJUSTEINCATIVIDAD
                ClientesSeVisibility.VisibleConceptoActividad = Visibility.Visible
                ClientesSeVisibility.VisibleConceptoInactividad = Visibility.Collapsed

                If visNavegando = "Collapsed" Then
                    MyBase.CambiarFormulario_Forma_Manual()
                End If
            Catch ex As Exception
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro",
                 Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
            End Try

        Else
            ClienteSelected.InactivaItem = False
        End If
    End Sub
    Private Sub TerminoinactivoBloqueo(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If objResultado.DialogResult Then
            'Consulta si el cliente es ordenante los clientes activos de este 
            ClienteSelected.EstadoCliente = "B"
            ClienteSelected.Usuario = Program.Usuario
            ActualizarRegistro()
        End If
    End Sub
    Private Sub TerminoTraerClientesActivos(ByVal obj As InvokeOperation(Of Integer))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó si tiene clientes activos", Me.ToString(), "consultarFechaCierreCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            intClientesActivos = obj.Value
        End If
    End Sub
    Private Sub TerminoTraerClientesrelacionados(ByVal obj As InvokeOperation(Of Integer))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó si tiene clientes relacionados", Me.ToString(), "TerminoTraerClientesrelacionados", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            intClienteRelacionados = obj.Value
        End If
    End Sub
    Private Sub TerminoConsultarConsecutivoExistente(ByVal obj As InvokeOperation(Of Nullable(Of Boolean)))
        If obj.HasError Then
            'JFSB 20171008 Se agrega propiedad para habilitar los botones de menú
            HabilitarMenu = True
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó el consecutivo", Me.ToString(), "TerminoConsultarConsecutivoExistenteCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            logPerteneceConsecutivo = obj.Value
        End If
    End Sub
    ''' <summary>
    ''' metodo asincronico que valida si se debe guardar un registro dejando los documentos pendientes o no
    ''' JBT20130213
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub TerminovalidarGenerales(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If Not IsNothing(objResultado) Then
            If Not IsNothing(objResultado.CodigoLlamado) Then
                Select Case objResultado.CodigoLlamado.ToUpper
                    Case "PREGUNTARAPROBACION"
                        Select Case objResultado.DialogResult
                            'Case False
                            '    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                            '    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_DOCUMRECIBIDOS
                            Case True
                                Try
                                    If ListaRespuestaValidacionesCientes.Count > 0 Then
                                        SelectedValidacionesCientes = Nothing

                                        For Each li In ListaRespuestaValidacionesCientes
                                            SelectedValidacionesCientes = li
                                            If li.RequiereConfirmacion Then
                                                If li.Tab = -1 Then
                                                    logEnviamail = True
                                                End If
                                                'se quita el codigo porque con el sincronismo de wpf se generan mensajes repetidos
                                                Exit For
                                            ElseIf li.DetieneIngreso Then
                                                'resumen mensajes
                                                A2Utilidades.Mensajes.mostrarMensaje(li.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                                Exit Sub
                                            End If
                                        Next

                                        If Not IsNothing(SelectedValidacionesCientes) Then
                                            ListaRespuestaValidacionesCientes.Remove(SelectedValidacionesCientes)
                                            mostrarMensajePregunta(SelectedValidacionesCientes.Mensaje,
                                               Program.TituloSistema,
                                               "PREGUNTARAPROBACION",
                                               AddressOf TerminovalidarGenerales, True, SelectedValidacionesCientes.Confirmacion)
                                        End If
                                    Else
                                        IsBusy = True
                                        validacionesinhabil()
                                    End If
                                Catch ex As Exception
                                    IsBusy = False
                                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                                         Me.ToString(), "TerminovalidarGenerales", Application.Current.ToString(), Program.Maquina, ex)
                                    ClientesSeBoolean.Editarcampos = True
                                End Try
                        End Select
                    Case "CIUDADANORESIDENTE"
                        If objResultado.DialogResult Then
                            If Not IsNothing(_ListaClientesPaisesFATCA) Then
                                Dim objListaID As New List(Of Integer)
                                For Each li In _ListaClientesPaisesFATCA
                                    objListaID.Add(li.IDClientesPaises)
                                Next

                                For Each li In objListaID
                                    If _ListaClientesPaisesFATCA.Where(Function(i) i.IDClientesPaises = li).Count > 0 Then
                                        _ListaClientesPaisesFATCA.Remove(_ListaClientesPaisesFATCA.Where(Function(i) i.IDClientesPaises = li).First)
                                    End If
                                Next
                            End If
                            _ListaClientesPaisesFATCA = Nothing
                            ClientesPaisesFATCAselected = Nothing
                        Else
                            IsBusy = False
                        End If
                    Case "CIUDADANORESIDENTEGUARDAR"
                        If objResultado.DialogResult Then
                            Dim objListaID As New List(Of Integer)
                            For Each li In _ListaClientesPaisesFATCA
                                objListaID.Add(li.IDClientesPaises)
                            Next

                            For Each li In objListaID
                                If _ListaClientesPaisesFATCA.Where(Function(i) i.IDClientesPaises = li).Count > 0 Then
                                    _ListaClientesPaisesFATCA.Remove(_ListaClientesPaisesFATCA.Where(Function(i) i.IDClientesPaises = li).First)
                                End If
                            Next
                            ClientesPaisesFATCAselected = Nothing


                            validaciones()
                            guardarregistro()
                        Else
                            IsBusy = False
                        End If
                End Select
            End If
        End If
    End Sub
    Private Sub TerminoTraerRespuestaMail(ByVal obj As InvokeOperation(Of String))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la respuesta del mail", Me.ToString(), "TerminoTraerRespuestaMail", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            If Not obj.Value = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje(obj.Value.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        End If
    End Sub

    Private Sub TerminoTraerClientesAutorizaciones(ByVal lo As LoadOperation(Of OyDClientes.ClientesAutorizaciones))
        If lo.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un error en el proceso de autorizaciones", Me.ToString(), "TerminoTraerClientesAutorizaciones", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
        End If
    End Sub

    'Tablas padres
    Private Sub consultarFechaCierreCompleted(ByVal obj As InvokeOperation(Of System.Nullable(Of Date)))
        If obj.HasError Then
            'JFSB 20171008 Se agrega propiedad para habilitar los botones de menú
            HabilitarMenu = True
            obj.MarkErrorAsHandled()
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la fecha de cierre del sistema", Me.ToString(), "consultarFechaCierreCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            fechaCierre = obj.Value
        End If
    End Sub
    Private Sub TerminoTraerRespuestaDocum(ByVal lo As LoadOperation(Of OyDClientes.ClientesSucursales))
        Dim strRepetido As String
        strRepetido = String.Empty
        If lo.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó el numero de documento", Me.ToString(), "TerminoTraerRespuestaDocum", Program.TituloSistema, Program.Maquina, lo.Error, Program.RutaServicioLog)
            If lo.UserState = "ACTUALIZARREGISTRO" Then
                IsBusy = False
            End If
        Else
            For Each li In lo.Entities.ToList
                strRepetido = li.Mensaje
            Next

            If Not strRepetido = String.Empty Then
                strDocumRepetido = strRepetido
            Else
                strDocumRepetido = String.Empty
            End If

            If lo.UserState = "ACTUALIZARREGISTRO" Then
                IsBusy = False
                ContinuarGuardadoCliente()
            End If
        End If
    End Sub

    ''' <history>
    ''' Descripción:    Se agregó el parámetro HABILITAR_CLASIFICACIONES_EN_CLIENTES que indica si la pestaña clasificaciones está activa.
    ''' Responsable:    Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:          29 de Septiembre/2014
    ''' Pruebas CB:     Jorge Peña (Alcuadrado S.A.) - 29 de Septiembre/2014
    ''' </history>
    Private Sub Terminotraerparametrolista(ByVal obj As LoadOperation(Of valoresparametro))
        If obj.HasError Then
            'JFSB 20171008 Se agrega propiedad para habilitar los botones de menú
            HabilitarMenu = True
            obj.MarkErrorAsHandled()
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la lista de parametros", Me.ToString(), "Terminotraerparametrolista", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            'Dim lista = obj.Entities.ToList
            For Each li In obj.Entities.ToList
                Select Case li.Parametro
                    Case "CamposUtilizadosCity"
                        ConfiguraFirma = li.Valor
                        validarconfiguracionfirma()
                    Case "MANEJOCLIENTESPARACONFIRMAR"
                        If li.Valor = "SI" Then
                            ClientesSeVisibility.PersonaporConfigvi = Visibility.Visible
                            'objProxy.Verificaparametro("SMMLV",Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "SMMLV")
                        Else
                            'escondo el tab de clientespara confirmar
                            ClientesSeVisibility.PersonaporConfigvi = Visibility.Collapsed
                        End If
                    Case "CALCULADATOSFINANCIEROS"
                        If li.Valor = "SI" Then
                            logvalorparametro = True
                        End If
                    Case "OBLIGATORIOS_DOC_REQUERIDOS"
                        strObligatoriosDoc = li.Valor
                    Case "MANEJAR_PRECLIENTES"
                        If li.Valor = "SI" Then
                            ClientesSeVisibility.Preclientevisible = Visibility.Visible
                        End If
                    Case "ACTIVARRETEICA"
                        If li.Valor = "SI" Then
                            ClientesSeVisibility.RETEICAvisible = Visibility.Visible
                        End If
                    Case "SMMLV"
                        CamposSeGenerales.SMMLV = li.Valor
                    Case "GRUPOPORDEFECTO"
                        mintGrupoPorDefecto = -1
                        mintGrupoPorDefecto = IIf(Versioned.IsNumeric(li.Valor), li.Valor, 0)
                        'ClienteSelected.IDGrupo = mintGrupoPorDefecto
                    Case "SUBGRUPOPORDEFECTO"
                        mintSubGrupoPorDefecto = -1
                        mintSubGrupoPorDefecto = IIf(Versioned.IsNumeric(li.Valor), li.Valor, 0)
                    Case "VALOR_PARTICIPACION_ACCIONISTAS"
                        intparticipacionaccionistas = li.Valor
                    Case "REPLICARCLIENTESAGORA"
                        If li.Valor = "SI" Then
                            ClientesSeVisibility.AGORAvisible = Visibility.Visible
                            logReplicaAgora = True
                        End If
                    Case "FormaFacturacionComision"
                        If li.Valor = "C" Then
                            logpagocomisiones = True
                            ClientesSeVisibility.Pagocomisiones = Visibility.Visible
                        End If
                    Case "APLICADISTRIBRECEPTORES"
                        If li.Valor = "SI" Then
                            LogdistribucionReceptor = True
                            ClientesSeVisibility.VisiblePorcentaje = Visibility.Visible
                        Else
                            LogdistribucionReceptor = False
                            ClientesSeVisibility.VisiblePorcentaje = Visibility.Collapsed
                        End If
                    Case "VALIDARDIGITONIT"
                        If li.Valor = "SI" Then
                            logValidarDigitoVerificacion = True
                        Else
                            logValidarDigitoVerificacion = False
                        End If
                    Case "OBLIGA_SUCURSAL_CLIENTE"
                        strObligasucursalcliente = li.Valor
                    Case "TIPOPRODUCTOCLIENTEDEFECTO"
                        TIPOPRODUCTOCLIENTEDEFECTO = li.Valor
                    Case "TOTALDIGITOSCELULAR"
                        TOTALDIGITOSCELULAR = li.Valor
                    Case "INFOLABORALREQUERIDO"
                        INFOLABORALREQUERIDO = li.Valor
                        If INFOLABORALREQUERIDO = "SI" Then
                            ClientesSeVisibility.visibletexocupa = Visibility.Collapsed
                            ClientesSeVisibility.visiblecomocupa = Visibility.Visible
                        End If
                    Case "VALIDACONTRASEÑA_CLIENTE"
                        VALIDACONTRASEÑA_CLIENTE = li.Valor
                    Case "PERFILCLIENTEDEFECTO"
                        PERFILCLIENTEDEFECTO = li.Valor
                    Case "VALIDAOCUPACIONCONPEP_CLIENTE"
                        VALIDAOCUPACIONCONPEP_CLIENTE = li.Valor
                    Case "VALIDA_VALORACTIVO"
                        VALIDA_VALORACTIVO = li.Valor
                    Case "SEPARADOREMAILCLIENTES"
                        SEPARADORMAIL = li.Valor
                    Case "PORCENTAJE_CERCANIA_SEGUNDO_MENSAJE"
                        PORCENTAJE_CERCANIA_SEGUNDO_MENSAJE = IIf(Versioned.IsNumeric(li.Valor), li.Valor, 0)
                    Case "PORCENTAJE_CERCANIA_MAYOR"
                        PORCENTAJE_CERCANIA_MAYOR = IIf(Versioned.IsNumeric(li.Valor), li.Valor, 0)
                    Case "PORCENTAJE_CERCANIA_MENOR"
                        PORCENTAJE_CERCANIA_MENOR = IIf(Versioned.IsNumeric(li.Valor), li.Valor, 0)
                    Case "ACTIVAR_AUTORIZACION_INHABIL"
                        ACTIVAR_AUTORIZACION_INHABIL = li.Valor
                    Case "MOSTRARMENSAJEADICIONALCLIENTES"
                        MOSTRARMENSAJEADICIONALCLIENTES = li.Valor
                        'Santiago Vergara - Junio 20/2014 - Obtener los valores de los parámetros de replicación
                    Case "REPLICAR_CLIENTES"
                        If li.Valor = "SI" Then
                            REPLICAR_CLIENTES = True
                        End If
                    Case "REPLICAR_FONDOS"
                        If li.Valor = "SI" Then
                            REPLICAR_FONDOS = True
                        End If
                    Case "REPLICAR_MERCAMSOFT"
                        If li.Valor = "SI" Then
                            REPLICAR_MERCAMSOFT = True
                        End If
                    Case "REPLICAR_PORTAFOLIOS"
                        If li.Valor = "SI" Then
                            REPLICAR_PORTAFOLIOS = True
                        End If
                    Case "VALIDARTIPOIDENTIFICACION_NACIMIENTO"
                        If li.Valor = "SI" Then
                            VALIDA_TIPOIDENTIFICACIONNACIMIENTO = True
                        Else
                            VALIDA_TIPOIDENTIFICACIONNACIMIENTO = False
                        End If
                    Case "HABILITAR_CLASIFICACIONES_EN_CLIENTES"
                        If li.Valor = "SI" Then
                            HABILITAR_CLASIFICACIONES_EN_CLIENTES = Visibility.Visible
                        End If
                        'NAOC20141118 - Fatca
                        '**********************************************************************
                    Case "CLIENTES_CAMPOS_ADICIONALES_FATCA"
                        If li.Valor = "SI" Then
                            FATCA_CAMPOSADICIONALES = True
                        Else
                            FATCA_CAMPOSADICIONALES = False
                        End If
                        '**********************************************************************
                    Case "CODIGO_INTERCLIENTEXTRANJERO"
                        CODIGO_INTERCLIENTEXTRANJERO = IIf(Versioned.IsNumeric(li.Valor), li.Valor, 0)
                         '**********************************************************************
                    Case "PatrimonioAutonomo_Activar"
                        PATRIMONIOAUTONOMO_ACTIVAR = li.Valor
                End Select
            Next
            'NAOC20141118 - Fatca
            '**********************************************************************
            ValidarHabilitarFatca()
            '**********************************************************************

        End If
    End Sub
    Private Sub Terminotraerparametro(ByVal obj As InvokeOperation(Of String))
        If obj.HasError Then
            obj.MarkErrorAsHandled()
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó el parametro", Me.ToString(), "Terminotraerparametro", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            Select Case obj.UserState
                Case "FONDOS"
                    If obj.Value = "SI" Then
                        MostrarClienteFondos = Visibility.Visible
                        objProxy.Verificaparametro("CF_UTILIZAPASIVA_TESORERIA_A2", Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "CF_UTILIZAPASIVA_TESORERIA_A2")
                    Else
                        'escondo el tab de Fondos
                        MostrarClienteFondos = Visibility.Collapsed
                    End If
            End Select
        End If
    End Sub
    ''' <summary>
    ''' Si el beneficiario tiene alguna semejanza con un cliente Inhabilitado, si es True la respuesta
    ''' se debe grabar el Log en tblListaClinton con el parámetro mlogGrabarListaClinton
    ''' </summary>
    ''' <param name="lo">JBT20130204</param>
    ''' <remarks></remarks>

    Private Sub TerminoConsultarClienteInhabilitado(ByVal lo As LoadOperation(Of ClienteInhabilitado))
        Try  'Se agrega Try - Catch a la funcion de consultarClienteInhabilitado CAFT20151028
            If Not lo.HasError Then
                If TerminoConsultainhabil Then
                    Exit Sub
                End If
                Dim a = lo.UserState.split("‡")
                Dim strNroDocumento = a(1)
                Dim strnombre = a(2)
                'mlogterminolistaclinton = a(4)
                Select Case a(0)
                    Case "CIstrNroDocumento"
                        If lo.Entities.Count > 0 Then
                            Select Case a(3)
                                Case "C"
                                    If plogActivacion Then
                                        A2Utilidades.Mensajes.mostrarMensaje("Imposible Activar Cliente por estar  Inhabilitado. Pertenece a:" & lo.Entities.First.Nombre & " el motivo es: " & lo.Entities.First.Motivo & vbCr &
                                    "Fecha: " & lo.Entities.First.Ingreso,
                                    Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    Else
                                        If ACTIVAR_AUTORIZACION_INHABIL = "NO" Then
                                            A2Utilidades.Mensajes.mostrarMensaje("El documento " & strNroDocumento.ToString & " ya existe como Inhabilitado." & vbCr &
                                                      "Pertenece a: " & lo.Entities.First.Nombre & " el motivo es: " & lo.Entities.First.Motivo & vbCr &
                                                      "Fecha: " & lo.Entities.First.Ingreso,
                                                      Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                            ListaDisparosAsync.Clear()
                                            'logvalidacion = True
                                            IsBusy = False
                                            Exit Sub
                                        End If
                                    End If
                                Case "A"
                                    If ACTIVAR_AUTORIZACION_INHABIL = "NO" Then
                                        A2Utilidades.Mensajes.mostrarMensaje("El documento del Accionista " & strNroDocumento.ToString & " ya existe como Inhabilitado." & vbCr &
                                                     "Pertenece a: " & lo.Entities.First.Nombre & " el motivo es: " & lo.Entities.First.Motivo & vbCr &
                                                     "Fecha: " & lo.Entities.First.Ingreso,
                                                     Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                        ListaDisparosAsync.Clear()
                                        'logvalidacion = True
                                        IsBusy = False
                                        Exit Sub
                                    End If

                                Case "B"
                                    If ACTIVAR_AUTORIZACION_INHABIL = "NO" Then
                                        A2Utilidades.Mensajes.mostrarMensaje("El documento del Beneficiario " & strNroDocumento.ToString & " ya existe como Inhabilitado." & vbCr &
                                                     "Pertenece a: " & lo.Entities.First.Nombre & " el motivo es: " & lo.Entities.First.Motivo & vbCr &
                                                     "Fecha: " & lo.Entities.First.Ingreso,
                                                     Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                        ListaDisparosAsync.Clear()
                                        'logvalidacion = True
                                        IsBusy = False
                                        Exit Sub
                                    End If

                                Case "R"
                                    If ACTIVAR_AUTORIZACION_INHABIL = "NO" Then
                                        A2Utilidades.Mensajes.mostrarMensaje("El documento del Representante Legal " & strNroDocumento.ToString & " ya existe como Inhabilitado." & vbCr &
                                                 "Pertenece a: " & lo.Entities.First.Nombre & " el motivo es: " & lo.Entities.First.Motivo & vbCr &
                                                 "Fecha: " & lo.Entities.First.Ingreso,
                                                 Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                        ListaDisparosAsync.Clear()
                                        'logvalidacion = True
                                        IsBusy = False
                                        Exit Sub
                                    End If

                                Case "CB"
                                    If ACTIVAR_AUTORIZACION_INHABIL = "NO" Then
                                        A2Utilidades.Mensajes.mostrarMensaje("El documento del titular de la cuenta bancaria " & strNroDocumento.ToString & " ya existe como Inhabilitado." & vbCr &
                                                      "Pertenece a: " & lo.Entities.First.Nombre & " el motivo es: " & lo.Entities.First.Motivo & vbCr &
                                                      "Fecha: " & lo.Entities.First.Ingreso,
                                                      Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                        ListaDisparosAsync.Clear()
                                        'logvalidacion = True
                                        IsBusy = False
                                        Exit Sub
                                    End If

                                Case "D"
                                    If ACTIVAR_AUTORIZACION_INHABIL = "NO" Then
                                        A2Utilidades.Mensajes.mostrarMensaje("El documento matriculado en dependencia económica " & strNroDocumento.ToString & " ya existe como Inhabilitado." & vbCr &
                                                  "Pertenece a: " & lo.Entities.First.Nombre & " el motivo es: " & lo.Entities.First.Motivo & vbCr &
                                                  "Fecha: " & lo.Entities.First.Ingreso,
                                                  Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                        ListaDisparosAsync.Clear()
                                        'logvalidacion = True
                                        IsBusy = False
                                        Exit Sub
                                    End If

                                Case "P"
                                    If ACTIVAR_AUTORIZACION_INHABIL = "NO" Then
                                        A2Utilidades.Mensajes.mostrarMensaje("El documento matriculado en personas para confirmar " & strNroDocumento.ToString & " ya existe como Inhabilitado." & vbCr &
                                                 "Pertenece a: " & lo.Entities.First.Nombre & " el motivo es: " & lo.Entities.First.Motivo & vbCr &
                                                 "Fecha: " & lo.Entities.First.Ingreso,
                                                 Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                        ListaDisparosAsync.Clear()
                                        'logvalidacion = True
                                        IsBusy = False
                                        Exit Sub
                                    End If

                            End Select
                        End If

                    Case "CINombre"
                        If lo.Entities.Count > 0 Then
                            Select Case a(3)

                                Case "C"
                                    If ACTIVAR_AUTORIZACION_INHABIL = "SI" Then
                                        If (NombreAnteriorAuto <> ClienteSelected.Nombre) Then
                                            If (lo.Entities.First.porcentaje >= PORCENTAJE_CERCANIA_MAYOR) Then
                                                A2Utilidades.Mensajes.mostrarMensaje("El Cliente: " & ClienteSelected.Nombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                             & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "se creara como cliente inactivo y debe ser aprobado por el oficial de cumplimiento. ",
                                                                             Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                                DatosListaClintonselected.Nombre = strnombre
                                                DatosListaClintonselected.NroDocumento = strNroDocumento
                                                DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                DatosListaClintonselected.Forma = "frmClientes"
                                                DatosListaClinton.Add(DatosListaClintonselected)
                                                mlogGrabarListaClinton = True
                                                ClienteAnuladoInhabil = True
                                                IsBusy = False

                                            ElseIf ((lo.Entities.First.porcentaje > PORCENTAJE_CERCANIA_MENOR) And (lo.Entities.First.porcentaje < PORCENTAJE_CERCANIA_MAYOR)) Then
                                                Dim o = MessageBox.Show("El Cliente: " & ClienteSelected.Nombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                             & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                             Program.TituloSistema, MessageBoxButton.OKCancel)
                                                If o = MessageBoxResult.OK Then
                                                    DatosListaClintonselected.Nombre = strnombre
                                                    DatosListaClintonselected.NroDocumento = strNroDocumento
                                                    DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                    DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                    DatosListaClintonselected.Forma = "frmClientes"
                                                    DatosListaClinton.Add(DatosListaClintonselected)
                                                    mlogGrabarListaClinton = True
                                                    ClienteAnuladoInhabil = False
                                                Else
                                                    ListaDisparosAsync.Clear()
                                                    TerminoConsultainhabil = True
                                                    'logvalidacion = True
                                                    IsBusy = False
                                                End If
                                            End If
                                        End If
                                    Else
                                        Dim o = MessageBox.Show("El Cliente: " & ClienteSelected.Nombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                         & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                         Program.TituloSistema, MessageBoxButton.OKCancel)
                                        If o = MessageBoxResult.OK Then
                                            DatosListaClintonselected.Nombre = strnombre
                                            DatosListaClintonselected.NroDocumento = strNroDocumento
                                            DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                            DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                            DatosListaClintonselected.Forma = "frmClientes"
                                            DatosListaClinton.Add(DatosListaClintonselected)
                                            mlogGrabarListaClinton = True
                                            ClienteAnuladoInhabil = False
                                        Else
                                            ListaDisparosAsync.Clear()
                                            TerminoConsultainhabil = True
                                            'logvalidacion = True
                                            IsBusy = False
                                        End If
                                    End If

                                Case "A"
                                    If ACTIVAR_AUTORIZACION_INHABIL = "SI" Then
                                        If Not IsNothing(ClientesAccionistasSelected) Then
                                            If ((ClientesAccionistasNombre1 <> ClientesAccionistasSelected.Nombre1) Or (ClientesAccionistasNombre2 <> ClientesAccionistasSelected.Nombre2) Or (ClientesAccionistasApellido1 <> ClientesAccionistasSelected.Apellido1) Or (ClientesAccionistasApellido2 <> ClientesAccionistasSelected.Apellido2)) Then
                                                If (lo.Entities.First.porcentaje >= PORCENTAJE_CERCANIA_MAYOR) Then
                                                    A2Utilidades.Mensajes.mostrarMensaje("El Accionista: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                                 & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "se creara como cliente inactivo y debe ser aprobado por el oficial de cumplimiento.. ",
                                                                                 Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                                    DatosListaClintonselected.Nombre = strnombre
                                                    DatosListaClintonselected.NroDocumento = strNroDocumento
                                                    DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                    DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                    DatosListaClintonselected.Forma = "frmClientesAccionistas"
                                                    DatosListaClinton.Add(DatosListaClintonselected)
                                                    mlogGrabarListaClinton = True
                                                    ClienteAnuladoInhabil = True
                                                    IsBusy = False

                                                ElseIf ((lo.Entities.First.porcentaje > PORCENTAJE_CERCANIA_MENOR) And (lo.Entities.First.porcentaje < PORCENTAJE_CERCANIA_MAYOR)) Then
                                                    Dim o = MessageBox.Show("El Accionista: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                               & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                               Program.TituloSistema, MessageBoxButton.OKCancel)
                                                    If o = MessageBoxResult.OK Then
                                                        DatosListaClintonselected.Nombre = strnombre
                                                        DatosListaClintonselected.NroDocumento = strNroDocumento
                                                        DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                        DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                        DatosListaClintonselected.Forma = "frmClientesAccionistas"
                                                        DatosListaClinton.Add(DatosListaClintonselected)
                                                        mlogGrabarListaClinton = True
                                                        ClienteAnuladoInhabil = False
                                                    Else
                                                        ListaDisparosAsync.Clear()
                                                        TerminoConsultainhabil = True
                                                        'logvalidacion = True
                                                        IsBusy = False
                                                    End If
                                                End If
                                            End If
                                        End If
                                    Else
                                        Dim o = MessageBox.Show("El Accionista: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                            & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                Program.TituloSistema, MessageBoxButton.OKCancel)
                                        If o = MessageBoxResult.OK Then
                                            DatosListaClintonselected.Nombre = strnombre
                                            DatosListaClintonselected.NroDocumento = strNroDocumento
                                            DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                            DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                            DatosListaClintonselected.Forma = "frmClientesAccionistas"
                                            DatosListaClinton.Add(DatosListaClintonselected)
                                            mlogGrabarListaClinton = True
                                            ClienteAnuladoInhabil = False
                                        Else
                                            ListaDisparosAsync.Clear()
                                            TerminoConsultainhabil = True
                                            'logvalidacion = True
                                            IsBusy = False
                                        End If
                                    End If

                                Case "B"
                                    If ACTIVAR_AUTORIZACION_INHABIL = "SI" Then
                                        If Not IsNothing(ClientesBeneficiarioSelected) Then
                                            If ((ClientesBeneficiarioNombre1 <> ClientesBeneficiarioSelected.Nombre1) Or (ClientesBeneficiarioNombre2 <> ClientesBeneficiarioSelected.Nombre2) Or (ClientesBeneficiarioApellido1 <> ClientesBeneficiarioSelected.Apellido1) Or (ClientesBeneficiarioApellido2 <> ClientesBeneficiarioSelected.Apellido2)) Then
                                                If (lo.Entities.First.porcentaje >= PORCENTAJE_CERCANIA_MAYOR) Then
                                                    A2Utilidades.Mensajes.mostrarMensaje("El Beneficiario: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                                & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "se creara como cliente inactivo y debe ser aprobado por el oficial de cumplimiento. ",
                                                                                Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                                    DatosListaClintonselected.Nombre = strnombre
                                                    DatosListaClintonselected.NroDocumento = strNroDocumento
                                                    DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                    DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                    DatosListaClintonselected.Forma = "frmClientesBeneficiarios"
                                                    DatosListaClinton.Add(DatosListaClintonselected)
                                                    mlogGrabarListaClinton = True
                                                    ClienteAnuladoInhabil = True
                                                    IsBusy = False

                                                ElseIf ((lo.Entities.First.porcentaje > PORCENTAJE_CERCANIA_MENOR) And (lo.Entities.First.porcentaje < PORCENTAJE_CERCANIA_MAYOR)) Then
                                                    Dim o = MessageBox.Show("El Beneficiario: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                                 & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                                 Program.TituloSistema, MessageBoxButton.OKCancel)
                                                    If o = MessageBoxResult.OK Then
                                                        DatosListaClintonselected.Nombre = strnombre
                                                        DatosListaClintonselected.NroDocumento = strNroDocumento
                                                        DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                        DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                        DatosListaClintonselected.Forma = "frmClientesBeneficiarios"
                                                        DatosListaClinton.Add(DatosListaClintonselected)
                                                        mlogGrabarListaClinton = True
                                                        ClienteAnuladoInhabil = False
                                                        IsBusy = False
                                                    Else
                                                        ListaDisparosAsync.Clear()
                                                        TerminoConsultainhabil = True
                                                        'logvalidacion = True
                                                        IsBusy = False
                                                    End If
                                                End If
                                            End If
                                        End If

                                    Else
                                        Dim o = MessageBox.Show("El Beneficiario: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                            & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                Program.TituloSistema, MessageBoxButton.OKCancel)
                                        If o = MessageBoxResult.OK Then
                                            DatosListaClintonselected.Nombre = strnombre
                                            DatosListaClintonselected.NroDocumento = strNroDocumento
                                            DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                            DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                            DatosListaClintonselected.Forma = "frmClientesBeneficiarios"
                                            DatosListaClinton.Add(DatosListaClintonselected)
                                            mlogGrabarListaClinton = True
                                            ClienteAnuladoInhabil = False
                                        Else
                                            ListaDisparosAsync.Clear()
                                            TerminoConsultainhabil = True
                                            'logvalidacion = True
                                            IsBusy = False
                                        End If
                                    End If


                                Case "R"
                                    If ACTIVAR_AUTORIZACION_INHABIL = "SI" Then
                                        If (NombreAnteriorAuto <> ClienteSelected.RepresentanteLegal) Then
                                            If (lo.Entities.First.porcentaje >= PORCENTAJE_CERCANIA_MAYOR) Then
                                                A2Utilidades.Mensajes.mostrarMensaje("El Representante Legal: " & ClienteSelected.RepresentanteLegal & " Tiene semejanza con el cliente Inhabilitado " _
                                                                           & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "se creara como cliente inactivo y debe ser aprobado por el oficial de cumplimiento. ",
                                                                           Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                                DatosListaClintonselected.Nombre = strnombre
                                                DatosListaClintonselected.NroDocumento = strNroDocumento
                                                DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                DatosListaClintonselected.Forma = "frmClienteRepLegal"
                                                DatosListaClinton.Add(DatosListaClintonselected)
                                                mlogGrabarListaClinton = True
                                                ClienteAnuladoInhabil = True
                                                IsBusy = False

                                            ElseIf ((lo.Entities.First.porcentaje > PORCENTAJE_CERCANIA_MENOR) And (lo.Entities.First.porcentaje < PORCENTAJE_CERCANIA_MAYOR)) Then
                                                Dim o = MessageBox.Show("El Representante Legal: " & ClienteSelected.RepresentanteLegal & " Tiene semejanza con el cliente Inhabilitado " _
                                                                             & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                             Program.TituloSistema, MessageBoxButton.OKCancel)
                                                If o = MessageBoxResult.OK Then
                                                    DatosListaClintonselected.Nombre = strnombre
                                                    DatosListaClintonselected.NroDocumento = strNroDocumento
                                                    DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                    DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                    DatosListaClintonselected.Forma = "frmClienteRepLegal"
                                                    DatosListaClinton.Add(DatosListaClintonselected)
                                                    mlogGrabarListaClinton = True
                                                    ClienteAnuladoInhabil = False
                                                Else
                                                    ListaDisparosAsync.Clear()
                                                    TerminoConsultainhabil = True
                                                    'logvalidacion = True
                                                    IsBusy = False
                                                End If
                                            End If
                                        End If
                                    Else
                                        Dim o = MessageBox.Show("El Representante Legal: " & ClienteSelected.RepresentanteLegal & " Tiene semejanza con el cliente Inhabilitado " _
                                                                 & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                 Program.TituloSistema, MessageBoxButton.OKCancel)
                                        If o = MessageBoxResult.OK Then
                                            DatosListaClintonselected.Nombre = strnombre
                                            DatosListaClintonselected.NroDocumento = strNroDocumento
                                            DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                            DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                            DatosListaClintonselected.Forma = "frmClienteRepLegal"
                                            DatosListaClinton.Add(DatosListaClintonselected)
                                            mlogGrabarListaClinton = True
                                            ClienteAnuladoInhabil = False
                                        Else
                                            ListaDisparosAsync.Clear()
                                            TerminoConsultainhabil = True
                                            'logvalidacion = True
                                            IsBusy = False
                                        End If
                                    End If


                                Case "CB"
                                    If ACTIVAR_AUTORIZACION_INHABIL = "SI" Then
                                        If (NombreAnteriorAuto <> ClienteSelected.Nombre) Then
                                            If (lo.Entities.First.porcentaje >= PORCENTAJE_CERCANIA_MAYOR) Then
                                                A2Utilidades.Mensajes.mostrarMensaje("El titular de la cuenta: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                          & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "se creara como cliente inactivo y debe ser aprobado por el oficial de cumplimiento. ",
                                                                          Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                                DatosListaClintonselected.Nombre = strnombre
                                                DatosListaClintonselected.NroDocumento = strNroDocumento
                                                DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                DatosListaClintonselected.Forma = "frmClientescuentas"
                                                DatosListaClinton.Add(DatosListaClintonselected)
                                                mlogGrabarListaClinton = True
                                                ClienteAnuladoInhabil = True
                                                IsBusy = False

                                            ElseIf ((lo.Entities.First.porcentaje > PORCENTAJE_CERCANIA_MENOR) And (lo.Entities.First.porcentaje < PORCENTAJE_CERCANIA_MAYOR)) Then
                                                Dim o = MessageBox.Show("El titular de la cuenta: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                            & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                            Program.TituloSistema, MessageBoxButton.OKCancel)
                                                If o = MessageBoxResult.OK Then
                                                    DatosListaClintonselected.Nombre = strnombre
                                                    DatosListaClintonselected.NroDocumento = strNroDocumento
                                                    DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                    DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                    DatosListaClintonselected.Forma = "frmClientescuentas"
                                                    DatosListaClinton.Add(DatosListaClintonselected)
                                                    mlogGrabarListaClinton = True
                                                    ClienteAnuladoInhabil = False
                                                Else
                                                    ListaDisparosAsync.Clear()
                                                    TerminoConsultainhabil = True
                                                    'logvalidacion = True
                                                    IsBusy = False
                                                End If
                                            End If
                                        End If
                                    Else
                                        Dim o = MessageBox.Show("El titular de la cuenta: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                 & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                 Program.TituloSistema, MessageBoxButton.OKCancel)
                                        If o = MessageBoxResult.OK Then
                                            DatosListaClintonselected.Nombre = strnombre
                                            DatosListaClintonselected.NroDocumento = strNroDocumento
                                            DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                            DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                            DatosListaClintonselected.Forma = "frmClientescuentas"
                                            DatosListaClinton.Add(DatosListaClintonselected)
                                            mlogGrabarListaClinton = True
                                            ClienteAnuladoInhabil = False
                                        Else
                                            ListaDisparosAsync.Clear()
                                            TerminoConsultainhabil = True
                                            'logvalidacion = True
                                            IsBusy = False
                                        End If
                                    End If

                                Case "D"
                                    If ACTIVAR_AUTORIZACION_INHABIL = "SI" Then
                                        If Not IsNothing(ClientesDepEconomicaselected) Then
                                            If ((ClientesDepEconomicaNombre1 <> ClientesDepEconomicaselected.Nombre1) Or (ClientesDepEconomicaNombre2 <> ClientesDepEconomicaselected.Nombre2) Or (ClientesDepEconomicaApellido1 <> ClientesDepEconomicaselected.Apellido1) Or (ClientesDepEconomicaApellido2 <> ClientesDepEconomicaselected.Apellido2)) Then
                                                If (lo.Entities.First.porcentaje >= PORCENTAJE_CERCANIA_MAYOR) Then
                                                    A2Utilidades.Mensajes.mostrarMensaje("El nombre matriculado en dependencia económica: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                              & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "se creara como cliente inactivo y debe ser aprobado por el oficial de cumplimiento. ",
                                                                              Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                                    DatosListaClintonselected.Nombre = strnombre
                                                    DatosListaClintonselected.NroDocumento = strNroDocumento
                                                    DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                    DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                    DatosListaClintonselected.Forma = "frmClienteRepLegal"
                                                    DatosListaClinton.Add(DatosListaClintonselected)
                                                    mlogGrabarListaClinton = True
                                                    ClienteAnuladoInhabil = True
                                                    IsBusy = False

                                                ElseIf ((lo.Entities.First.porcentaje > PORCENTAJE_CERCANIA_MENOR) And (lo.Entities.First.porcentaje < PORCENTAJE_CERCANIA_MAYOR)) Then
                                                    Dim o = MessageBox.Show("El nombre matriculado en dependencia económica: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                                & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                                Program.TituloSistema, MessageBoxButton.OKCancel)
                                                    If o = MessageBoxResult.OK Then
                                                        DatosListaClintonselected.Nombre = strnombre
                                                        DatosListaClintonselected.NroDocumento = strNroDocumento
                                                        DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                        DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                        DatosListaClintonselected.Forma = "frmClienteRepLegal"
                                                        DatosListaClinton.Add(DatosListaClintonselected)
                                                        mlogGrabarListaClinton = True
                                                        ClienteAnuladoInhabil = False
                                                    Else
                                                        ListaDisparosAsync.Clear()
                                                        TerminoConsultainhabil = True
                                                        'logvalidacion = True
                                                        IsBusy = False
                                                    End If
                                                End If
                                            End If
                                        End If
                                    Else
                                        Dim o = MessageBox.Show("El nombre matriculado en dependencia económica: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                            & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                Program.TituloSistema, MessageBoxButton.OKCancel)
                                        If o = MessageBoxResult.OK Then
                                            DatosListaClintonselected.Nombre = strnombre
                                            DatosListaClintonselected.NroDocumento = strNroDocumento
                                            DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                            DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                            DatosListaClintonselected.Forma = "frmClienteRepLegal"
                                            DatosListaClinton.Add(DatosListaClintonselected)
                                            mlogGrabarListaClinton = True
                                            ClienteAnuladoInhabil = False
                                        Else
                                            ListaDisparosAsync.Clear()
                                            TerminoConsultainhabil = True
                                            'logvalidacion = True
                                            IsBusy = False
                                        End If
                                    End If



                                Case "P"
                                    If ACTIVAR_AUTORIZACION_INHABIL = "SI" Then
                                        If Not IsNothing(ClientesPersonaselected) Then
                                            If ((ClientesPersonaNombre1 <> ClientesPersonaselected.Nombre1) Or (ClientesPersonaNombre2 <> ClientesPersonaselected.Nombre2) Or (ClientesPersonaApellido1 <> ClientesPersonaselected.Apellido1) Or (ClientesPersonaApellido2 <> ClientesPersonaselected.Apellido2)) Then
                                                If (lo.Entities.First.porcentaje >= PORCENTAJE_CERCANIA_MAYOR) Then
                                                    A2Utilidades.Mensajes.mostrarMensaje("El nombre matriculado en personas para confirmar: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                              & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "se creara como cliente inactivo y debe ser aprobado por el oficial de cumplimiento. ",
                                                                              Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                                    DatosListaClintonselected.Nombre = strnombre
                                                    DatosListaClintonselected.NroDocumento = strNroDocumento
                                                    DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                    DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                    DatosListaClintonselected.Forma = "frmClienteRepLegal"
                                                    DatosListaClinton.Add(DatosListaClintonselected)
                                                    mlogGrabarListaClinton = True
                                                    ClienteAnuladoInhabil = True
                                                    IsBusy = False

                                                ElseIf ((lo.Entities.First.porcentaje > PORCENTAJE_CERCANIA_MENOR) And (lo.Entities.First.porcentaje < PORCENTAJE_CERCANIA_MAYOR)) Then
                                                    Dim o = MessageBox.Show("El nombre matriculado en personas para confirmar: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                                & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                                Program.TituloSistema, MessageBoxButton.OKCancel)
                                                    If o = MessageBoxResult.OK Then
                                                        DatosListaClintonselected.Nombre = strnombre
                                                        DatosListaClintonselected.NroDocumento = strNroDocumento
                                                        DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                        DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                        DatosListaClintonselected.Forma = "frmClienteRepLegal"
                                                        DatosListaClinton.Add(DatosListaClintonselected)
                                                        mlogGrabarListaClinton = True
                                                        ClienteAnuladoInhabil = False
                                                        IsBusy = False
                                                    Else
                                                        ListaDisparosAsync.Clear()
                                                        TerminoConsultainhabil = True
                                                        'logvalidacion = True
                                                        IsBusy = False
                                                    End If
                                                End If
                                            End If
                                        End If
                                    Else
                                        Dim o = MessageBox.Show("El nombre matriculado en personas para confirmar: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                            & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                Program.TituloSistema, MessageBoxButton.OKCancel)
                                        If o = MessageBoxResult.OK Then
                                            DatosListaClintonselected.Nombre = strnombre
                                            DatosListaClintonselected.NroDocumento = strNroDocumento
                                            DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                            DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                            DatosListaClintonselected.Forma = "frmClienteRepLegal"
                                            DatosListaClinton.Add(DatosListaClintonselected)
                                            mlogGrabarListaClinton = True
                                            ClienteAnuladoInhabil = False
                                        Else
                                            ListaDisparosAsync.Clear()
                                            TerminoConsultainhabil = True
                                            'logvalidacion = True
                                            IsBusy = False
                                        End If
                                    End If



                                Case "CSM"
                                    If ACTIVAR_AUTORIZACION_INHABIL = "SI" Then
                                        If (NombreAnteriorAuto <> ClienteSelected.Nombre) Then
                                            If (lo.Entities.First.porcentaje >= PORCENTAJE_CERCANIA_MAYOR) Then
                                                A2Utilidades.Mensajes.mostrarMensaje("El Cliente: " & ClienteSelected.Nombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                          & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "se creara como cliente inactivo y debe ser aprobado por el oficial de cumplimiento. ",
                                                                          Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                                DatosListaClintonselected.Nombre = strnombre
                                                DatosListaClintonselected.NroDocumento = strNroDocumento
                                                DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                DatosListaClintonselected.Forma = "frmClientes-SegMsg"
                                                DatosListaClinton.Add(DatosListaClintonselected)
                                                mlogGrabarListaClinton = True
                                                ClienteAnuladoInhabil = True
                                                IsBusy = False

                                            ElseIf ((lo.Entities.First.porcentaje > PORCENTAJE_CERCANIA_MENOR) And (lo.Entities.First.porcentaje < PORCENTAJE_CERCANIA_MAYOR)) Then
                                                Dim o = MessageBox.Show("El Cliente: " & ClienteSelected.Nombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                            & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                            Program.TituloSistema, MessageBoxButton.OKCancel)
                                                If o = MessageBoxResult.OK Then
                                                    DatosListaClintonselected.Nombre = strnombre
                                                    DatosListaClintonselected.NroDocumento = strNroDocumento
                                                    DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                    DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                    DatosListaClintonselected.Forma = "frmClientes-SegMsg"
                                                    DatosListaClinton.Add(DatosListaClintonselected)
                                                    mlogGrabarListaClinton = True
                                                    ClienteAnuladoInhabil = False
                                                    IsBusy = False
                                                Else
                                                    ListaDisparosAsync.Clear()
                                                    TerminoConsultainhabil = True
                                                    'logvalidacion = True
                                                    IsBusy = False
                                                End If
                                            End If
                                        End If

                                    Else
                                        Dim o = MessageBox.Show("El Cliente: " & ClienteSelected.Nombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                 & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                 Program.TituloSistema, MessageBoxButton.OKCancel)
                                        If o = MessageBoxResult.OK Then
                                            DatosListaClintonselected.Nombre = strnombre
                                            DatosListaClintonselected.NroDocumento = strNroDocumento
                                            DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                            DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                            DatosListaClintonselected.Forma = "frmClientes-SegMsg"
                                            DatosListaClinton.Add(DatosListaClintonselected)
                                            mlogGrabarListaClinton = True
                                            ClienteAnuladoInhabil = False
                                        Else
                                            ListaDisparosAsync.Clear()
                                            TerminoConsultainhabil = True
                                            'logvalidacion = True
                                            IsBusy = False
                                        End If
                                    End If

                                Case "ASM"
                                    If ACTIVAR_AUTORIZACION_INHABIL = "SI" Then
                                        If (NombreAnteriorAuto <> ClienteSelected.Nombre) Then
                                            If (lo.Entities.First.porcentaje >= PORCENTAJE_CERCANIA_MAYOR) Then
                                                A2Utilidades.Mensajes.mostrarMensaje("El Accionista: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                         & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "se creara como cliente inactivo y debe ser aprobado por el oficial de cumplimiento. ",
                                                                         Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                                DatosListaClintonselected.Nombre = strnombre
                                                DatosListaClintonselected.NroDocumento = strNroDocumento
                                                DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                DatosListaClintonselected.Forma = "frmClientesAccionistas-SegMsg"
                                                DatosListaClinton.Add(DatosListaClintonselected)
                                                mlogGrabarListaClinton = True
                                                ClienteAnuladoInhabil = True
                                                IsBusy = False

                                            ElseIf ((lo.Entities.First.porcentaje > PORCENTAJE_CERCANIA_MENOR) And (lo.Entities.First.porcentaje < PORCENTAJE_CERCANIA_MAYOR)) Then
                                                Dim o = MessageBox.Show("El Accionista: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                            & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                            Program.TituloSistema, MessageBoxButton.OKCancel)
                                                If o = MessageBoxResult.OK Then
                                                    DatosListaClintonselected.Nombre = strnombre
                                                    DatosListaClintonselected.NroDocumento = strNroDocumento
                                                    DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                    DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                    DatosListaClintonselected.Forma = "frmClientesAccionistas-SegMsg"
                                                    DatosListaClinton.Add(DatosListaClintonselected)
                                                    mlogGrabarListaClinton = True
                                                    ClienteAnuladoInhabil = False
                                                    IsBusy = False
                                                Else
                                                    ListaDisparosAsync.Clear()
                                                    TerminoConsultainhabil = True
                                                    'logvalidacion = True
                                                    IsBusy = False
                                                End If
                                            End If
                                        End If
                                    Else
                                        Dim o = MessageBox.Show("El Accionista: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                Program.TituloSistema, MessageBoxButton.OKCancel)
                                        If o = MessageBoxResult.OK Then
                                            DatosListaClintonselected.Nombre = strnombre
                                            DatosListaClintonselected.NroDocumento = strNroDocumento
                                            DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                            DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                            DatosListaClintonselected.Forma = "frmClientesAccionistas-SegMsg"
                                            DatosListaClinton.Add(DatosListaClintonselected)
                                            mlogGrabarListaClinton = True
                                            ClienteAnuladoInhabil = False
                                        Else
                                            ListaDisparosAsync.Clear()
                                            TerminoConsultainhabil = True
                                            'logvalidacion = True
                                            IsBusy = False
                                        End If
                                    End If

                                Case "BSM"
                                    If ACTIVAR_AUTORIZACION_INHABIL = "SI" Then
                                        If (NombreAnteriorAuto <> ClienteSelected.Nombre) Then
                                            If (lo.Entities.First.porcentaje >= PORCENTAJE_CERCANIA_MAYOR) Then
                                                A2Utilidades.Mensajes.mostrarMensaje("El Beneficiario: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                        & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "se creara como cliente inactivo y debe ser aprobado por el oficial de cumplimiento. ",
                                                                        Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                                DatosListaClintonselected.Nombre = strnombre
                                                DatosListaClintonselected.NroDocumento = strNroDocumento
                                                DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                DatosListaClintonselected.Forma = "frmClientesBeneficiarios-SegMsg"
                                                DatosListaClinton.Add(DatosListaClintonselected)
                                                mlogGrabarListaClinton = True
                                                ClienteAnuladoInhabil = True
                                                IsBusy = False

                                            ElseIf ((lo.Entities.First.porcentaje > PORCENTAJE_CERCANIA_MENOR) And (lo.Entities.First.porcentaje < PORCENTAJE_CERCANIA_MAYOR)) Then
                                                Dim o = MessageBox.Show("El Beneficiario: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                             & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                             Program.TituloSistema, MessageBoxButton.OKCancel)
                                                If o = MessageBoxResult.OK Then
                                                    DatosListaClintonselected.Nombre = strnombre
                                                    DatosListaClintonselected.NroDocumento = strNroDocumento
                                                    DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                    DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                    DatosListaClintonselected.Forma = "frmClientesBeneficiarios-SegMsg"
                                                    DatosListaClinton.Add(DatosListaClintonselected)
                                                    mlogGrabarListaClinton = True
                                                    ClienteAnuladoInhabil = False
                                                    IsBusy = False
                                                Else
                                                    ListaDisparosAsync.Clear()
                                                    TerminoConsultainhabil = True
                                                    'logvalidacion = True
                                                    IsBusy = False
                                                End If
                                            End If
                                        End If
                                    Else
                                        Dim o = MessageBox.Show("El Beneficiario: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                 & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                 Program.TituloSistema, MessageBoxButton.OKCancel)
                                        If o = MessageBoxResult.OK Then
                                            DatosListaClintonselected.Nombre = strnombre
                                            DatosListaClintonselected.NroDocumento = strNroDocumento
                                            DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                            DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                            DatosListaClintonselected.Forma = "frmClientesBeneficiarios-SegMsg"
                                            DatosListaClinton.Add(DatosListaClintonselected)
                                            mlogGrabarListaClinton = True
                                            ClienteAnuladoInhabil = False
                                        Else
                                            ListaDisparosAsync.Clear()
                                            TerminoConsultainhabil = True
                                            'logvalidacion = True
                                            IsBusy = False
                                        End If
                                    End If

                                Case "RSM"
                                    If ACTIVAR_AUTORIZACION_INHABIL = "SI" Then
                                        If (NombreAnteriorAuto <> ClienteSelected.Nombre) Then
                                            If (lo.Entities.First.porcentaje >= PORCENTAJE_CERCANIA_MAYOR) Then
                                                A2Utilidades.Mensajes.mostrarMensaje("El Representante Legal: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                       & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "se creara como cliente inactivo y debe ser aprobado por el oficial de cumplimiento. ",
                                                                       Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                                DatosListaClintonselected.Nombre = strnombre
                                                DatosListaClintonselected.NroDocumento = strNroDocumento
                                                DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                DatosListaClintonselected.Forma = "frmClienteRepLegal-SegMsg"
                                                DatosListaClinton.Add(DatosListaClintonselected)
                                                mlogGrabarListaClinton = True
                                                ClienteAnuladoInhabil = True
                                                IsBusy = False


                                            ElseIf ((lo.Entities.First.porcentaje > PORCENTAJE_CERCANIA_MENOR) And (lo.Entities.First.porcentaje < PORCENTAJE_CERCANIA_MAYOR)) Then
                                                Dim o = MessageBox.Show("El Representante Legal: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                            & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                            Program.TituloSistema, MessageBoxButton.OKCancel)
                                                If o = MessageBoxResult.OK Then
                                                    DatosListaClintonselected.Nombre = strnombre
                                                    DatosListaClintonselected.NroDocumento = strNroDocumento
                                                    DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                    DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                    DatosListaClintonselected.Forma = "frmClienteRepLegal-SegMsg"
                                                    DatosListaClinton.Add(DatosListaClintonselected)
                                                    mlogGrabarListaClinton = True
                                                    ClienteAnuladoInhabil = False
                                                    IsBusy = False
                                                Else
                                                    ListaDisparosAsync.Clear()
                                                    TerminoConsultainhabil = True
                                                    'logvalidacion = True
                                                    IsBusy = False
                                                End If
                                            End If
                                        End If
                                    Else
                                        Dim o = MessageBox.Show("El Representante Legal: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                                                         & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                                                         Program.TituloSistema, MessageBoxButton.OKCancel)
                                        If o = MessageBoxResult.OK Then
                                            DatosListaClintonselected.Nombre = strnombre
                                            DatosListaClintonselected.NroDocumento = strNroDocumento
                                            DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                            DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                            DatosListaClintonselected.Forma = "frmClienteRepLegal-SegMsg"
                                            DatosListaClinton.Add(DatosListaClintonselected)
                                            mlogGrabarListaClinton = True
                                            ClienteAnuladoInhabil = False
                                        Else
                                            ListaDisparosAsync.Clear()
                                            TerminoConsultainhabil = True
                                            'logvalidacion = True
                                            IsBusy = False
                                        End If
                                    End If


                                Case "CBSM"
                                    If ACTIVAR_AUTORIZACION_INHABIL = "SI" Then
                                        If (NombreAnteriorAuto <> ClienteSelected.Nombre) Then
                                            If (lo.Entities.First.porcentaje >= PORCENTAJE_CERCANIA_MAYOR) Then
                                                A2Utilidades.Mensajes.mostrarMensaje("El titular de la cuenta: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                       & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "se creara como cliente inactivo y debe ser aprobado por el oficial de cumplimiento. ",
                                                                       Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                                DatosListaClintonselected.Nombre = strnombre
                                                DatosListaClintonselected.NroDocumento = strNroDocumento
                                                DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                DatosListaClintonselected.Forma = "frmClientescuentas-SegMsg"
                                                DatosListaClinton.Add(DatosListaClintonselected)
                                                mlogGrabarListaClinton = True
                                                ClienteAnuladoInhabil = True
                                                IsBusy = False

                                            ElseIf ((lo.Entities.First.porcentaje > PORCENTAJE_CERCANIA_MENOR) And (lo.Entities.First.porcentaje < PORCENTAJE_CERCANIA_MAYOR)) Then
                                                Dim o = MessageBox.Show("El titular de la cuenta: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                             & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                             Program.TituloSistema, MessageBoxButton.OKCancel)
                                                If o = MessageBoxResult.OK Then
                                                    DatosListaClintonselected.Nombre = strnombre
                                                    DatosListaClintonselected.NroDocumento = strNroDocumento
                                                    DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                    DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                    DatosListaClintonselected.Forma = "frmClientescuentas-SegMsg"
                                                    DatosListaClinton.Add(DatosListaClintonselected)
                                                    mlogGrabarListaClinton = True
                                                    ClienteAnuladoInhabil = False
                                                    IsBusy = False
                                                Else
                                                    ListaDisparosAsync.Clear()
                                                    TerminoConsultainhabil = True
                                                    'logvalidacion = True
                                                    IsBusy = False
                                                End If
                                            End If
                                        End If
                                    Else
                                        Dim o = MessageBox.Show("El titular de la cuenta: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                Program.TituloSistema, MessageBoxButton.OKCancel)
                                        If o = MessageBoxResult.OK Then
                                            DatosListaClintonselected.Nombre = strnombre
                                            DatosListaClintonselected.NroDocumento = strNroDocumento
                                            DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                            DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                            DatosListaClintonselected.Forma = "frmClientescuentas-SegMsg"
                                            DatosListaClinton.Add(DatosListaClintonselected)
                                            mlogGrabarListaClinton = True
                                            ClienteAnuladoInhabil = False
                                        Else
                                            ListaDisparosAsync.Clear()
                                            TerminoConsultainhabil = True
                                            'logvalidacion = True
                                            IsBusy = False
                                        End If
                                    End If
                                Case "DSM"
                                    If ACTIVAR_AUTORIZACION_INHABIL = "SI" Then
                                        If (NombreAnteriorAuto <> ClienteSelected.Nombre) Then
                                            If (lo.Entities.First.porcentaje >= PORCENTAJE_CERCANIA_MAYOR) Then
                                                A2Utilidades.Mensajes.mostrarMensaje("El nombre matriculado en dependencia económica: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                        & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "se creara como cliente inactivo y debe ser aprobado por el oficial de cumplimiento. ",
                                                                        Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                                DatosListaClintonselected.Nombre = strnombre
                                                DatosListaClintonselected.NroDocumento = strNroDocumento
                                                DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                DatosListaClintonselected.Forma = "frmClienteRepLegal-SegMsg"
                                                DatosListaClinton.Add(DatosListaClintonselected)
                                                mlogGrabarListaClinton = True
                                                ClienteAnuladoInhabil = True
                                                IsBusy = False

                                            ElseIf ((lo.Entities.First.porcentaje > PORCENTAJE_CERCANIA_MENOR) And (lo.Entities.First.porcentaje < PORCENTAJE_CERCANIA_MAYOR)) Then
                                                Dim o = MessageBox.Show("El nombre matriculado en dependencia económica: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                            & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                            Program.TituloSistema, MessageBoxButton.OKCancel)
                                                If o = MessageBoxResult.OK Then
                                                    DatosListaClintonselected.Nombre = strnombre
                                                    DatosListaClintonselected.NroDocumento = strNroDocumento
                                                    DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                    DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                    DatosListaClintonselected.Forma = "frmClienteRepLegal-SegMsg"
                                                    DatosListaClinton.Add(DatosListaClintonselected)
                                                    mlogGrabarListaClinton = True
                                                    ClienteAnuladoInhabil = False
                                                    IsBusy = False
                                                Else
                                                    ListaDisparosAsync.Clear()
                                                    TerminoConsultainhabil = True
                                                    'logvalidacion = True
                                                    IsBusy = False
                                                End If
                                            End If
                                        End If
                                    Else
                                        Dim o = MessageBox.Show("El nombre matriculado en dependencia económica: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                Program.TituloSistema, MessageBoxButton.OKCancel)
                                        If o = MessageBoxResult.OK Then
                                            DatosListaClintonselected.Nombre = strnombre
                                            DatosListaClintonselected.NroDocumento = strNroDocumento
                                            DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                            DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                            DatosListaClintonselected.Forma = "frmClienteRepLegal-SegMsg"
                                            DatosListaClinton.Add(DatosListaClintonselected)
                                            mlogGrabarListaClinton = True
                                            ClienteAnuladoInhabil = False
                                        Else
                                            ListaDisparosAsync.Clear()
                                            TerminoConsultainhabil = True
                                            'logvalidacion = True
                                            IsBusy = False
                                        End If
                                    End If

                                Case "PSM"
                                    If ACTIVAR_AUTORIZACION_INHABIL = "SI" Then
                                        If (NombreAnteriorAuto <> ClienteSelected.Nombre) Then
                                            If (lo.Entities.First.porcentaje >= PORCENTAJE_CERCANIA_MAYOR) Then
                                                A2Utilidades.Mensajes.mostrarMensaje("El nombre matriculado en personas para confirmar: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                       & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "se creara como cliente inactivo y debe ser aprobado por el oficial de cumplimiento. ",
                                                                       Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                                DatosListaClintonselected.Nombre = strnombre
                                                DatosListaClintonselected.NroDocumento = strNroDocumento
                                                DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                DatosListaClintonselected.Forma = "frmClienteRepLegal-SegMsg"
                                                DatosListaClinton.Add(DatosListaClintonselected)
                                                mlogGrabarListaClinton = True
                                                ClienteAnuladoInhabil = True
                                                IsBusy = False

                                            ElseIf ((lo.Entities.First.porcentaje > PORCENTAJE_CERCANIA_MENOR) And (lo.Entities.First.porcentaje < PORCENTAJE_CERCANIA_MAYOR)) Then
                                                Dim o = MessageBox.Show("El nombre matriculado en personas para confirmar: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                            & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                            Program.TituloSistema, MessageBoxButton.OKCancel)
                                                If o = MessageBoxResult.OK Then
                                                    DatosListaClintonselected.Nombre = strnombre
                                                    DatosListaClintonselected.NroDocumento = strNroDocumento
                                                    DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                                    DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                                    DatosListaClintonselected.Forma = "frmClienteRepLegal-SegMsg"
                                                    DatosListaClinton.Add(DatosListaClintonselected)
                                                    mlogGrabarListaClinton = True
                                                    ClienteAnuladoInhabil = False
                                                    IsBusy = False
                                                Else
                                                    ListaDisparosAsync.Clear()
                                                    TerminoConsultainhabil = True
                                                    'logvalidacion = True
                                                    IsBusy = False
                                                End If
                                            End If
                                        End If
                                    Else
                                        Dim o = MessageBox.Show("El nombre matriculado en personas para confirmar: " & strnombre & " Tiene semejanza con el cliente Inhabilitado " _
                                                                & lo.Entities.First.Nombre & " en un " & lo.Entities.First.porcentaje & "%" & vbCr & "Desea continuar ?",
                                                                Program.TituloSistema, MessageBoxButton.OKCancel)
                                        If o = MessageBoxResult.OK Then
                                            DatosListaClintonselected.Nombre = strnombre
                                            DatosListaClintonselected.NroDocumento = strNroDocumento
                                            DatosListaClintonselected.porcentaje = lo.Entities.First.porcentaje
                                            DatosListaClintonselected.Clienteinhabilitado = lo.Entities.First.Nombre
                                            DatosListaClintonselected.Forma = "frmClienteRepLegal-SegMsg"
                                            DatosListaClinton.Add(DatosListaClintonselected)
                                            mlogGrabarListaClinton = True
                                            ClienteAnuladoInhabil = False
                                        Else
                                            ListaDisparosAsync.Clear()
                                            TerminoConsultainhabil = True
                                            'logvalidacion = True
                                            IsBusy = False
                                        End If
                                    End If

                            End Select

                        End If
                End Select
                If ListaDisparosAsync.Count = 0 Then
                    Exit Sub
                Else
                    ListaDisparosAsync.Remove(a(4))
                    If ListaDisparosAsync.Count = 0 Then
                        guardarregistro()
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de los paramétros",
                                                     Me.ToString(), "TerminoConsultarClienteInhabilitado", Application.Current.ToString(), Program.Maquina, lo.Error)
                ListaDisparosAsync.Clear()
                lo.MarkErrorAsHandled()   '????
                IsBusy = False
            End If

        Catch ex As Exception    'CAFT20151028
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al termino de la consulta de Cliente Inhabilitado.",
                                                 Me.ToString(), "TerminoConsultarClienteInhabilitado", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try

    End Sub
    Private Sub TerminotraerparametroDocumMenor(ByVal obj As InvokeOperation(Of Integer))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó si el tipo de documento es para un menor de edad", Me.ToString(), "consultarFechaCierreCompleted", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
        Else
            intDocummenor = obj.Value
        End If
    End Sub
    Private Sub TerminoCalcularDigitoVerificacion(ByVal lo As InvokeOperation(Of Integer))
        Try
            If IsNothing(lo.Error) Then
                If Not String.IsNullOrEmpty(lo.UserState) Then
                    Dim digitoValidar As String = String.Empty
                    Dim strNitValidar As String = String.Empty

                    strNitValidar = lo.UserState.Substring(0, Len(lo.UserState) - 1)
                    digitoValidar = Right(lo.UserState, 1)
                    strDigitoPermitido = lo.Value.ToString

                    If strDigitoPermitido <> digitoValidar Then
                        logPasoValidacionDigito = False
                        A2Utilidades.Mensajes.mostrarMensaje(String.Format("El digito de verificación {0} para el NIT {1} no es valido. Permitido {2}", digitoValidar, strNitValidar, strDigitoPermitido), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        logPasoValidacionDigito = True
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la validar el digito de verificación.",
                                                 Me.ToString(), "TerminoCalcularDigitoVerificacion", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la validar el digito de verificación.",
                                                 Me.ToString(), "TerminoCalcularDigitoVerificacion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub TerminoValidacionesClientes(ByVal lo As LoadOperation(Of OyDClientes.tblRespuestaValidacionesCientes))
        Try
            If IsNothing(lo.Error) Then
                If ListaRespuestaValidacionesCientes.Count = 0 Then
                    ListaRespuestaValidacionesCientes = lo.Entities.ToList
                Else
                    For Each li In lo.Entities.ToList
                        ListaRespuestaValidacionesCientes.Add(li)
                    Next
                End If
                'IsBusy = True
                If ListaRespuestaValidacionesCientes.Count > 0 Then
                    SelectedValidacionesCientes = Nothing
                    For Each li In ListaRespuestaValidacionesCientes
                        SelectedValidacionesCientes = li
                        If li.RequiereConfirmacion Then
                            If li.Tab = -1 Then
                                logEnviamail = True
                            End If
                            'se quita el codigo porque con el sincronismo de wpf se generan mensajes repetidos
                            Exit For
                        ElseIf li.DetieneIngreso Then
                            'resumen mensajes
                            A2Utilidades.Mensajes.mostrarMensaje(li.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If
                    Next
                    If Not IsNothing(SelectedValidacionesCientes) Then
                        ListaRespuestaValidacionesCientes.Remove(SelectedValidacionesCientes)
                        mostrarMensajePregunta(SelectedValidacionesCientes.Mensaje,
                           Program.TituloSistema,
                           "PREGUNTARAPROBACION",
                           AddressOf TerminovalidarGenerales, True, SelectedValidacionesCientes.Confirmacion)
                    End If

                Else
                    IsBusy = True
                    validacionesinhabil()
                End If
            Else
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la validación de los clientes.",
                                                 Me.ToString(), "TerminoValidacionesClientes", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la validación de los clientes.",
                                                 Me.ToString(), "TerminoValidacionesClientes", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoPantallasParametrizacion_Consultar(lo As LoadOperation(Of PantallasParametrizacion))
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso terminó consultar la parametrización de la pantalla. ",
                                                 Me.ToString(), "TerminoPantallasParametrizacion_Consultar", Application.Current.ToString(), Program.Maquina, lo.Error)
            Else
                ListaPantallasParametrizacion = mdcProxyUtilidad08.PantallasParametrizacions.ToList
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso terminó procesar valoracion.",
                                                             Me.ToString(), "TerminoValidarFechaCierrePortafolio", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region
#Region "Propiedades"
    Private _listasubSector As List(Of Clasificacion)
    Public Property listasubSector As List(Of Clasificacion)
        Get
            Return _listasubSector
        End Get
        Set(ByVal value As List(Of Clasificacion))
            _listasubSector = value
            MyBase.CambioItem("listasubSector")
        End Set
    End Property
    Private _listasubGrupo As List(Of Clasificacion)
    Public Property listasubGrupo As List(Of Clasificacion)
        Get
            Return _listasubGrupo
        End Get
        Set(ByVal value As List(Of Clasificacion))
            _listasubGrupo = value
            MyBase.CambioItem("listasubGrupo")
        End Set
    End Property
    Private _listaclasificacion As EntitySet(Of Clasificacion)
    Public Property listaclasificacion As EntitySet(Of Clasificacion)
        Get
            Return _listaclasificacion
        End Get
        Set(ByVal value As EntitySet(Of Clasificacion))
            _listaclasificacion = value
            MyBase.CambioItem("listaclasificacion")
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
        End Set
    End Property
    Private _InstalacioSelected As New Instalacio
    Public Property InstalacioSelected() As Instalacio
        Get
            Return _InstalacioSelected
        End Get
        Set(ByVal value As Instalacio)
            _InstalacioSelected = value
            MyBase.CambioItem("InstalacioSelected")
        End Set
    End Property

    Private _ListaClientes As EntitySet(Of OyDClientes.Cliente)
    Public Property ListaClientes() As EntitySet(Of OyDClientes.Cliente)
        Get
            Return _ListaClientes
        End Get
        Set(ByVal value As EntitySet(Of OyDClientes.Cliente))
            _ListaClientes = value
            MyBase.CambioItem("ListaClientes")
            MyBase.CambioItem("ListaClientesPaged")
            If Not IsNothing(value) Then
                If IsNothing(ClienteAnterior) Then
                    ClienteSelected = _ListaClientes.FirstOrDefault
                Else
                    'SV20150427 Se añade validación ya que generaba error cuando la consulta no traia datos
                    If Not IsNothing(_ListaClientes) AndAlso _ListaClientes.Count > 0 Then
                        If Not ClienteAnterior.IDComitente.Equals(_ListaClientes.FirstOrDefault.IDComitente) Then
                            ClienteSelected = _ListaClientes.FirstOrDefault
                        ElseIf _ListaClientes.FirstOrDefault.Por_Aprobar <> Nothing Then
                            ClienteSelected = _ListaClientes.FirstOrDefault
                        Else
                            ClienteSelected = ClienteAnterior
                        End If

                    Else
                        ClienteSelected = Nothing
                    End If
                End If
            End If
        End Set

    End Property
    Public ReadOnly Property ListaClientesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaClientes) Then
                Dim view = New PagedCollectionView(_ListaClientes)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <history>
    ''' Descripción:    Se agregó el llamado al proxi con la entidad ClientesProductos.
    ''' Responsable:    Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:          29 de Septiembre/2014
    ''' Pruebas CB:     Jorge Peña (Alcuadrado S.A.) - 29 de Septiembre/2014
    ''' </history>
    Private WithEvents _ClienteSelected As OyDClientes.Cliente
    Public Property ClienteSelected() As OyDClientes.Cliente
        Get
            Return _ClienteSelected
        End Get
        Set(ByVal value As OyDClientes.Cliente)

            If Not value Is Nothing Then
                _ClienteSelected = value

                If Not IsNothing(_ClienteSelected) Then
                    If _ClienteSelected.TipoPersona = 2 Then
                        If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                            objTipoId = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOIDC2")
                        End If
                    Else
                        If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                            objTipoId = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOIDC1")
                        End If
                    End If
                End If

                TipoIdentificacionEdicion = _ClienteSelected.TipoIdentificacion

                'SV20141007AJUSTEINCATIVIDAD
                If Not IsNothing(_ClienteSelected) AndAlso Not IsNothing(_ClienteSelected.EstadoCliente) Then
                    If _ClienteSelected.EstadoCliente = "A" Then
                        ClientesSeVisibility.VisibleConceptoActividad = Visibility.Visible
                        ClientesSeVisibility.VisibleConceptoInactividad = Visibility.Collapsed
                    Else
                        ClientesSeVisibility.VisibleConceptoActividad = Visibility.Collapsed
                        ClientesSeVisibility.VisibleConceptoInactividad = Visibility.Visible
                    End If
                End If

                'NAOC20141118 - Fatca
                '**********************************************************************
                ValidarHabilitarFatca()
                '**********************************************************************


                ' If esVersion = False Then
                If Not IsNothing(listaclasificacion) Then
                    If Not IsNothing(listasubSector) Then
                        listasubSector.Clear()
                    End If
                    listasubSector = listaclasificacion.Where(Function(di) di.EsSector = False And di.IDPerteneceA = IIf(IsNothing(_ClienteSelected.IDSector), 0, _ClienteSelected.IDSector) And di.AplicaA.Equals("C")).ToList
                    _ClienteSelected.IDSubSector = value.IDSubSector
                    MyBase.CambioItem("listasubSector")
                    If Not IsNothing(listasubGrupo) Then
                        listasubGrupo.Clear()
                    End If
                    listasubGrupo = listaclasificacion.Where(Function(di) di.EsGrupo = False And di.IDPerteneceA = IIf(IsNothing(_ClienteSelected.IDGrupo), 0, _ClienteSelected.IDGrupo) And di.AplicaA.Equals("C")).ToList
                    _ClienteSelected.IDSubGrupo = value.IDSubGrupo
                    MyBase.CambioItem("listasubGrupo")
                End If
                buscarItem("ciudadesdoc")
                buscarItem("ciudades")
                buscarItem("Codigos_Ciiu")
                buscarItem("Ciudadrep")
                buscarItem("Ciudadna")
                buscarItem("CodigoProfesion")

                If Not IsNothing(_ClienteSelected.TipoPersona) Then
                    If _ClienteSelected.TipoPersona = 2 Then
                        ClientesSeVisibility.TipoPersonaJuridica = Visibility.Visible
                        ClientesSeVisibility.TipoPersonaNatural = Visibility.Collapsed
                        If Not ConfiguraFirma.Equals("SI") Then
                            'Confirguracion para ue city no vea el tab de accionistas por el momento 
                            ClientesSeVisibility.ConfiguravisibleAccionistas = Visibility.Visible
                        End If
                    Else
                        ClientesSeVisibility.TipoPersonaNatural = Visibility.Visible
                        ClientesSeVisibility.TipoPersonaJuridica = Visibility.Collapsed
                        ClientesSeVisibility.ConfiguravisibleAccionistas = Visibility.Collapsed
                    End If
                End If
                If Not IsNothing(_ClienteSelected.TipoReferencia) Then
                    If _ClienteSelected.TipoReferencia.Equals("O") Then
                        ClientesSeVisibility.ReferenciaOtros = Visibility.Visible
                        ClientesSeVisibility.ReferenciaPor = Visibility.Collapsed
                    ElseIf _ClienteSelected.TipoReferencia.Equals("R") Then
                        ClientesSeVisibility.ReferenciaOtros = Visibility.Collapsed
                        ClientesSeVisibility.ReferenciaPor = Visibility.Visible
                    Else
                        ClientesSeVisibility.ReferenciaOtros = Visibility.Collapsed
                        ClientesSeVisibility.ReferenciaPor = Visibility.Collapsed
                    End If
                End If
                If _ClienteSelected.FormaPago = "C" Then
                    ClientesSeVisibility.VisibleTipoCheque = Visibility.Visible
                Else
                    ClientesSeVisibility.VisibleTipoCheque = Visibility.Collapsed
                End If

                If Editando = False Then
                    If _ClienteSelected.Por_Aprobar Is Nothing Then
                        dcProxy.ClientesReceptores.Clear()
                        dcProxy.Load(dcProxy.Traer_ClientesReceptoreQuery(0, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesReceptores, Nothing)
                        Select Case TabSeleccionadaFinanciero

                            Case TAB_PRINCIPAL_GENERAL_FINANCIERO
                                dcProxy.CuentasClientes.Clear()
                                dcProxy.Load(dcProxy.Traer_CuentasClientesQuery(0, _ClienteSelected.IDComitente, False, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasclientes, Nothing)
                                dcProxy.TipoClients.Clear()
                                dcProxy.Load(dcProxy.TipoClienteFiltrarQuery(0, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesTipoCliente, Nothing)
                                'Case TAB_PRINCIPAL_GENERAL_RECEPTORES
                                '    dcProxy.ClientesReceptores.Clear()
                                '    dcProxy.Load(dcProxy.Traer_ClientesReceptoreQuery(0, _ClienteSelected.IDComitente,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesReceptores, Nothing)
                                dcProxy.ClientesProductos.Clear()
                                dcProxy.Load(dcProxy.ConsultarClientesProductosQuery(ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarClientesProductos, Nothing)
                                'NAOC20141118 - Fatca
                                '**********************************************************************
                                dcProxy.ClientesPaisesFATCAs.Clear()
                                dcProxy.Load(dcProxy.ConsultarClientesPaisesFATCAQuery(0, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesPaisesFatca, Nothing)
                                    '**********************************************************************
                            Case TAB_PRINCIPAL_GENERAL_ORDENANTES
                                dcProxy.ClientesOrdenantes.Clear()
                                dcProxy.Load(dcProxy.Traer_ClientesOrdenanteQuery(0, _ClienteSelected.IDComitente, Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesOrdenantes, Nothing)
                            Case TAB_PRINCIPAL_GENERAL_BENEFICIARIOS
                                dcProxy.ClientesBeneficiarios.Clear()
                                dcProxy.Load(dcProxy.Traer_ClientesBeneficiariosQuery(0, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesBeneficiarios, Nothing)
                            Case TAB_PRINCIPAL_GENERAL_MERCADEO
                                dcProxy.ClientesAficiones.Clear()
                                dcProxy.Load(dcProxy.Traer_ClientesAficionesQuery(0, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesAficiones, Nothing)
                                dcProxy.ClientesDeportes.Clear()
                                dcProxy.Load(dcProxy.Traer_ClientesDeportesQuery(0, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDeportes, Nothing)
                                dcProxy.ClientesAccionistas.Clear()
                                dcProxy.Load(dcProxy.Traer_ClientesAccionistasQuery(0, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesAccionistas, Nothing)
                                dcProxy.ClientesFichas.Clear()
                                dcProxy.Load(dcProxy.Traer_ClientesFichaClienteQuery(0, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesFicha, Nothing)
                                dcProxy.ClientesPersonasDepEconomicas.Clear()
                                dcProxy.Load(dcProxy.Traer_ClientesDepEconomicaQuery(0, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDepEconomica, Nothing)
                            Case TAB_PRINCIPAL_GENERAL_UBICACION
                                dcProxy.ClientesDirecciones.Clear()
                                dcProxy.Load(dcProxy.Traer_ClientesDireccionesQuery(0, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDirecciones, Nothing)
                            Case TAB_PRINCIPAL_GENERAL_DOCUMRECIBIDOS
                                dcProxy.ClientesDocumentosRequeridos.Clear()
                                dcProxy.Load(dcProxy.Traer_ClientesDocumentosRequeridosQuery(_ClienteSelected.IDComitente, _ClienteSelected.TipoPersona, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDocumentosRequeridos, Nothing)
                            Case TAB_PRINCIPAL_GENERAL_IR
                                If configuraloghir Then
                                    dcProxy.ClientesLOGHistoriaIRs.Clear()
                                    dcProxy.Load(dcProxy.Traer_ClientesLOGHistoriaIQuery(_ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesLOGHistoriaI, Nothing)
                                End If
                                If configuraconocimiento Then
                                    dcProxy.ClientesConocimientoEspecificos.Clear()
                                    ListaClientesConocimientoEspecificoclase.Clear()
                                    Contadorconocimientos = 0
                                    dcProxy.Load(dcProxy.Traer_ClientesConocimientoEspecificoQuery(0, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesConocimientoEspecifico, Nothing)
                                End If
                            Case TAB_PRINCIPAL_GENERAL_PERSONASPORCONFIRMAR
                                dcProxy.ClientesPersonasParaConfirmars.Clear()
                                dcProxy.Load(dcProxy.Traer_ClientesPersonasQuery(0, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesPersonas, Nothing)
                        End Select

                    Else
                        dcProxy.ClientesReceptores.Clear()
                        dcProxy.Load(dcProxy.Traer_ClientesReceptoreQuery(1, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesReceptores, Nothing)
                        dcProxy.CuentasClientes.Clear()
                        dcProxy.Load(dcProxy.Traer_CuentasClientesQuery(1, _ClienteSelected.IDComitente, False, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasclientes, Nothing)
                        dcProxy.TipoClients.Clear()
                        dcProxy.Load(dcProxy.TipoClienteFiltrarQuery(1, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesTipoCliente, Nothing)
                        dcProxy.ClientesProductos.Clear()
                        dcProxy.Load(dcProxy.ConsultarClientesProductosQuery(ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarClientesProductos, Nothing)
                        dcProxy.ClientesOrdenantes.Clear()
                        dcProxy.Load(dcProxy.Traer_ClientesOrdenanteQuery(1, _ClienteSelected.IDComitente, Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesOrdenantes, Nothing)
                        dcProxy.ClientesBeneficiarios.Clear()
                        dcProxy.Load(dcProxy.Traer_ClientesBeneficiariosQuery(1, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesBeneficiarios, Nothing)
                        dcProxy.ClientesAccionistas.Clear()
                        dcProxy.Load(dcProxy.Traer_ClientesAccionistasQuery(1, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesAccionistas, Nothing)
                        dcProxy.ClientesFichas.Clear()
                        dcProxy.Load(dcProxy.Traer_ClientesFichaClienteQuery(1, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesFicha, Nothing)
                        dcProxy.ClientesPersonasDepEconomicas.Clear()
                        dcProxy.Load(dcProxy.Traer_ClientesDepEconomicaQuery(1, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDepEconomica, Nothing)
                        dcProxy.ClientesDirecciones.Clear()
                        dcProxy.Load(dcProxy.Traer_ClientesDireccionesQuery(1, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDirecciones, Nothing)
                        If configuraloghir Then
                            dcProxy.ClientesLOGHistoriaIRs.Clear()
                            dcProxy.Load(dcProxy.Traer_ClientesLOGHistoriaIQuery(_ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesLOGHistoriaI, Nothing)
                        End If
                        If configuraconocimiento Then
                            dcProxy.ClientesConocimientoEspecificos.Clear()
                            ListaClientesConocimientoEspecificoclase.Clear()
                            Contadorconocimientos = 0
                            dcProxy.Load(dcProxy.Traer_ClientesConocimientoEspecificoQuery(1, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesConocimientoEspecifico, Nothing)
                        End If
                        dcProxy.ClientesPersonasParaConfirmars.Clear()
                        dcProxy.Load(dcProxy.Traer_ClientesPersonasQuery(1, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesPersonas, Nothing)
                        dcProxy.ClientesAficiones.Clear()
                        dcProxy.Load(dcProxy.Traer_ClientesAficionesQuery(1, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesAficiones, Nothing)
                        dcProxy.ClientesDeportes.Clear()
                        dcProxy.Load(dcProxy.Traer_ClientesDeportesQuery(1, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDeportes, Nothing)
                        'NAOC20141118 - Fatca
                        '**********************************************************************
                        dcProxy.ClientesPaisesFATCAs.Clear()
                        dcProxy.Load(dcProxy.ConsultarClientesPaisesFATCAQuery(1, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesPaisesFatca, Nothing)
                        '**********************************************************************
                        Select Case TabSeleccionadaFinanciero
                                'Case TAB_PRINCIPAL_GENERAL_FINANCIERO
                                '    dcProxy.CuentasClientes.Clear()
                                '    dcProxy.Load(dcProxy.Traer_CuentasClientesQuery(1, _ClienteSelected.IDComitente,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasclientes, Nothing)
                                '    dcProxy.TipoClients.Clear()
                                '    dcProxy.Load(dcProxy.TipoClienteFiltrarQuery(1, _ClienteSelected.IDComitente,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesTipoCliente, Nothing)
                                'Case TAB_PRINCIPAL_GENERAL_RECEPTORES
                                '    dcProxy.ClientesReceptores.Clear()
                                '    dcProxy.Load(dcProxy.Traer_ClientesReceptoreQuery(1, _ClienteSelected.IDComitente,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesReceptores, Nothing)
                                'Case TAB_PRINCIPAL_GENERAL_ORDENANTES
                                '    dcProxy.ClientesOrdenantes.Clear()
                                '    dcProxy.Load(dcProxy.Traer_ClientesOrdenanteQuery(1, _ClienteSelected.IDComitente,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesOrdenantes, Nothing)
                                'Case TAB_PRINCIPAL_GENERAL_BENEFICIARIOS
                                '    dcProxy.ClientesBeneficiarios.Clear()
                                '    dcProxy.Load(dcProxy.Traer_ClientesBeneficiariosQuery(1, _ClienteSelected.IDComitente,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesBeneficiarios, Nothing)
                                'Case TAB_PRINCIPAL_GENERAL_MERCADEO
                                '    dcProxy.ClientesAficiones.Clear()
                                '    dcProxy.Load(dcProxy.Traer_ClientesAficionesQuery(1, _ClienteSelected.IDComitente,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesAficiones, Nothing)
                                '    dcProxy.ClientesDeportes.Clear()
                                '    dcProxy.Load(dcProxy.Traer_ClientesDeportesQuery(1, _ClienteSelected.IDComitente,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDeportes, Nothing)
                                '    dcProxy.ClientesAccionistas.Clear()
                                '    dcProxy.Load(dcProxy.Traer_ClientesAccionistasQuery(1, _ClienteSelected.IDComitente,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesAccionistas, Nothing)
                                '    dcProxy.ClientesFichas.Clear()
                                '    dcProxy.Load(dcProxy.Traer_ClientesFichaClienteQuery(1, _ClienteSelected.IDComitente,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesFicha, Nothing)
                                '    dcProxy.ClientesPersonasDepEconomicas.Clear()
                                '    dcProxy.Load(dcProxy.Traer_ClientesDepEconomicaQuery(1, _ClienteSelected.IDComitente,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDepEconomica, Nothing)
                                'Case TAB_PRINCIPAL_GENERAL_UBICACION
                                '    dcProxy.ClientesDirecciones.Clear()
                                '    dcProxy.Load(dcProxy.Traer_ClientesDireccionesQuery(1, _ClienteSelected.IDComitente,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDirecciones, Nothing)
                            Case TAB_PRINCIPAL_GENERAL_DOCUMRECIBIDOS
                                dcProxy.ClientesDocumentosRequeridos.Clear()
                                dcProxy.Load(dcProxy.Traer_ClientesDocumentosRequeridosQuery(_ClienteSelected.IDComitente, _ClienteSelected.TipoPersona, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDocumentosRequeridos, Nothing)
                                'Case TAB_PRINCIPAL_GENERAL_IR
                                '    If configuraloghir Then
                                '        dcProxy.ClientesLOGHistoriaIRs.Clear()
                                '        dcProxy.Load(dcProxy.Traer_ClientesLOGHistoriaIQuery(_ClienteSelected.IDComitente,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesLOGHistoriaI, Nothing)
                                '    End If
                                '    If configuraconocimiento Then
                                '        dcProxy.ClientesConocimientoEspecificos.Clear()
                                '        ListaClientesConocimientoEspecificoclase.Clear()
                                '        Contadorconocimientos = 0
                                '        dcProxy.Load(dcProxy.Traer_ClientesConocimientoEspecificoQuery(1, _ClienteSelected.IDComitente,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesConocimientoEspecifico, Nothing)
                                '    End If
                                'Case TAB_PRINCIPAL_GENERAL_PERSONASPORCONFIRMAR
                                '    dcProxy.ClientesPersonasParaConfirmars.Clear()
                                '    dcProxy.Load(dcProxy.Traer_ClientesPersonasQuery(1, _ClienteSelected.IDComitente,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesPersonas, Nothing)
                        End Select

                        If _ClienteSelected.Estado.Equals("Ingreso") Then
                            ClientesSeBoolean.visible = False
                        End If
                    End If
                End If
                If esversionportafolio = False Then
                    Select Case TabSeleccionadaPrincipal
                        Case TAB_PRINCIPAL_ORDENES
                            ListaClientesOrdenes.Clear()
                            dcProxy.ClientesOrdenes.Clear()
                            dcProxy.Load(dcProxy.TraerClientesOrdenesQuery(_ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesOrdenes, Nothing)
                        Case TAB_PRINCIPAL_RENTAFIJA
                            ListaClientesRentaFija.Clear()
                            dcProxy.ClientesRentaFijas.Clear()
                            dcProxy.Load(dcProxy.TraerClientesRentaFijaQuery(_ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesRentafija, Nothing)
                        Case TAB_PRINCIPAL_PORTAFOLIO
                            ClientesSeBoolean.IsbusyPortafolio = True
                            textoPortafolio = String.Empty
                            Listaportafolio.Clear()
                            dcProxy.Portafolios.Clear()
                            dcProxy.Load(dcProxy.TraerPortafolioQuery(_ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPortafolio, Nothing)
                        Case TAB_PRINCIPAL_ACCIONES
                            ListaClientesAcciones.Clear()
                            dcProxy.ClientesAcciones.Clear()
                            dcProxy.Load(dcProxy.TraerClientesAccionesQuery(_ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesAcciones, Nothing)
                        Case TAB_PRINCIPAL_DEPOSITO
                            ListaCuentadeposito.Clear()
                            dcProxy.CuentasDepositos.Clear()
                            dcProxy.Load(dcProxy.TraerClientesDepositoQuery(_ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesDeposito, Nothing)
                        Case TAB_PRINCIPAL_TESORERIA
                            ListaTesoreria.Clear()
                            dcProxy.ClientesTesorerias.Clear()
                            dcProxy.Load(dcProxy.TraerClientesTesoreriaQuery(_ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientestesoreria, Nothing)
                        Case TAB_PRINCIPAL_CUSTODIAS
                            'SLB20140423 Consultar las costudias 
                            IsBusyCustodias = True
                            ListaCustodias.Clear()
                            dcProxy.ClientesCustodias.Clear()
                            dcProxy.Load(dcProxy.TraerClientesCustodiasQuery(_ClienteSelected.IDComitente, FechaCorte, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesCustodias, Nothing)
                        Case TAB_PRINCIPAL_FACTURAS
                            ListaClientesFactura.Clear()
                            dcProxy.FacturasClis.Clear()
                            dcProxy.Load(dcProxy.TraerClientesFacturasQuery(_ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesFactura, Nothing)
                        Case TAB_PRINCIPAL_FONDOS
                            ConsultarFondos()
                        Case TAB_PRINCIPAL_DIVISAS
                            ListaClientesDivisas.Clear()
                            dcProxy.Divisas.Clear()
                            dcProxy.Load(dcProxy.TraerClientesDivisasQuery(_ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesDivisas, Nothing)
                        Case TAB_PRINCIPAL_VENCIMIENTO
                            ListaVencimientos.Clear()
                        Case TAB_PRINCIPAL_LIQXCUMPLIR
                            ListaLiqxCumplir.Clear()
                        Case TAB_PRINCIPAL_REPO
                            ListaRepos.Clear()
                        Case TAB_PRINCIPAL_TOTALES
                            ListaClseTotales.Clear()
                            dcProxy.ClientesTotales.Clear()
                            dcProxy.Load(dcProxy.TraerClientesTotalesQuery(_ClienteSelected.IDComitente, Now, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesTotales, Nothing)
                        Case TAB_PRINCIPAL_SALDOS
                            ListaSaldos.Clear()
                            dcProxy.ClientesSaldos.Clear()
                            'JFSB 20160926 Se agrega el usuario sin dominio
                            'CFMA20181008
                            'dcProxy.Load(dcProxy.TraerClientesSaldosQuery(_ClienteSelected.IDComitente, Now, Program.UsuarioSinDomino, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesSaldos, Nothing)
                            dcProxy.Load(dcProxy.TraerClientesSaldosQuery(_ClienteSelected.IDComitente, Now, Program.UsuarioSinDomino, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesSaldos, _ClienteSelected.IDComitente)
                            'CFMA20181008
                        Case TAB_PRINCIPAL_ENCARGOS
                            ListaEncargos.Clear()
                            dcProxy.ClientesEncargos.Clear()
                            dcProxy.Load(dcProxy.TraerClientesEncargosQuery(_ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesEncargos, Nothing)

                    End Select
                End If
                esversionportafolio = False
                Historialir.ClasificacionInversionista = _ClienteSelected.ClasificacionInversionista
                Historialir.ConocimientoExperiencia = _ClienteSelected.ConocimientoExperiencia
                Historialir.EdadCliente = _ClienteSelected.EdadCliente
                Historialir.HorizonteInversion = _ClienteSelected.HorizonteInversion
                Historialir.ObjetivoInversion = _ClienteSelected.ObjetivoInversion
                Historialir.Suitability = _ClienteSelected.Suitability
                calculosmmlv()
                'End If
                esVersion = False
            Else
                _ClienteSelected = Nothing
                If Not IsNothing(CamposSeGenerales) Then
                    CamposSeGenerales.SMMLV = String.Empty
                    CamposSeGenerales.strcodigociu = String.Empty
                    CamposSeGenerales.strProfesion = String.Empty
                    CamposSeGenerales.valorenpesos = String.Empty
                End If

                If Not IsNothing(CiudadesClientes) Then
                    CiudadesClientes.strciudad = String.Empty
                    CiudadesClientes.strciudadNacimiento = String.Empty
                    CiudadesClientes.strdepartamento = String.Empty
                    CiudadesClientes.strdepartamentoDoc = String.Empty
                    CiudadesClientes.strPais = String.Empty
                    CiudadesClientes.strPaisDoc = String.Empty
                    CiudadesClientes.strPoblacion = String.Empty
                    CiudadesClientes.strPoblaciondoc = String.Empty
                End If

                dcProxy.ClientesReceptores.Clear()
                dcProxy.CuentasClientes.Clear()
                dcProxy.TipoClients.Clear()
                dcProxy.ClientesProductos.Clear()
                dcProxy.ClientesOrdenantes.Clear()
                dcProxy.ClientesBeneficiarios.Clear()
                dcProxy.ClientesAccionistas.Clear()
                dcProxy.ClientesFichas.Clear()
                dcProxy.ClientesPersonasDepEconomicas.Clear()
                dcProxy.ClientesDirecciones.Clear()
                dcProxy.ClientesLOGHistoriaIRs.Clear()
                dcProxy.ClientesConocimientoEspecificos.Clear()
                If Not IsNothing(ListaClientesConocimientoEspecificoclase) Then
                    ListaClientesConocimientoEspecificoclase.Clear()
                End If
                dcProxy.ClientesPersonasParaConfirmars.Clear()
                dcProxy.ClientesAficiones.Clear()
                dcProxy.ClientesDeportes.Clear()
                dcProxy.ClientesPaisesFATCAs.Clear()
                dcProxy.ClientesDocumentosRequeridos.Clear()

            End If
            If Busquedavacia Then
                _ClienteSelected = Nothing
                Busquedavacia = False
            End If

            MyBase.CambioItem("ClienteSelected")
        End Set
    End Property


    Private _ClienteAnterior As OyDClientes.Cliente
    Public Property ClienteAnterior() As OyDClientes.Cliente
        Get
            Return _ClienteAnterior
        End Get
        Set(ByVal value As OyDClientes.Cliente)
            If Not IsNothing(value) AndAlso value.IDComitente = "-1" Then
                _ClienteAnterior = Nothing
            Else
                _ClienteAnterior = value
            End If
        End Set
    End Property


    'Private _ClienteSelectedcl As New OyDClientes.Cliente
    'Public Property ClienteSelectedcl() As OyDClientes.Cliente
    '    Get
    '        Return _ClienteSelectedcl
    '    End Get
    '    Set(ByVal value As OyDClientes.Cliente)
    '        _ClienteSelectedcl = value
    '        MyBase.CambioItem("ClienteSelectedcl")
    '    End Set
    'End Property
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
    Private _TabSeleccionado As Integer = 0
    Public Property TabSeleccionado
        Get
            Return _TabSeleccionado
        End Get
        Set(ByVal value)
            _TabSeleccionado = value
            MyBase.CambioItem("TabSeleccionado")

        End Set
    End Property

    ''' <history>
    ''' Descripción:    Se agregó el llamado al proxi con la entidad ClientesProductos.
    ''' Responsable:    Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:          29 de Septiembre/2014
    ''' Pruebas CB:     Jorge Peña (Alcuadrado S.A.) - 29 de Septiembre/2014
    ''' </history>
    Private _TabSeleccionadaFinanciero As Integer = 0
    Public Property TabSeleccionadaFinanciero
        Get
            Return _TabSeleccionadaFinanciero
        End Get
        Set(ByVal value)
            If Editando = False Then
                If Not IsNothing(_ClienteSelected) Then
                    If _ClienteSelected.Por_Aprobar Is Nothing Then
                        Select Case value
                            Case TAB_PRINCIPAL_GENERAL_FINANCIERO
                                If Not dcProxy.CuentasClientes.HasChanges And Not dcProxy.TipoClients.HasChanges Then
                                    If Editando = True And dcProxy.CuentasClientes.Count > 0 Then
                                        Exit Property
                                    End If
                                    dcProxy.CuentasClientes.Clear()
                                    dcProxy.Load(dcProxy.Traer_CuentasClientesQuery(0, ClienteSelected.IDComitente, False, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasclientes, Nothing)
                                    dcProxy.TipoClients.Clear()
                                    dcProxy.Load(dcProxy.TipoClienteFiltrarQuery(0, ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesTipoCliente, Nothing)
                                    dcProxy.ClientesProductos.Clear()
                                    dcProxy.Load(dcProxy.ConsultarClientesProductosQuery(ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarClientesProductos, Nothing)
                                End If
                                'NAOC20141118 - Fatca
                                '**********************************************************************
                                If Not dcProxy.ClientesPaisesFATCAs.HasChanges Then
                                    dcProxy.ClientesPaisesFATCAs.Clear()
                                    dcProxy.Load(dcProxy.ConsultarClientesPaisesFATCAQuery(0, ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesPaisesFatca, Nothing)
                                End If
                            '**********************************************************************
                            Case TAB_PRINCIPAL_GENERAL_RECEPTORES
                                If Not dcProxy.ClientesReceptores.HasChanges Then
                                    dcProxy.ClientesReceptores.Clear()
                                    dcProxy.Load(dcProxy.Traer_ClientesReceptoreQuery(0, ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesReceptores, Nothing)
                                End If
                            Case TAB_PRINCIPAL_GENERAL_ORDENANTES
                                If Not dcProxy.ClientesOrdenantes.HasChanges Then
                                    dcProxy.ClientesOrdenantes.Clear()
                                    dcProxy.Load(dcProxy.Traer_ClientesOrdenanteQuery(0, ClienteSelected.IDComitente, Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesOrdenantes, Nothing)
                                End If
                            Case TAB_PRINCIPAL_GENERAL_BENEFICIARIOS
                                If Not dcProxy.ClientesBeneficiarios.HasChanges Then
                                    dcProxy.ClientesBeneficiarios.Clear()
                                    dcProxy.Load(dcProxy.Traer_ClientesBeneficiariosQuery(0, ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesBeneficiarios, Nothing)
                                End If
                            Case TAB_PRINCIPAL_GENERAL_MERCADEO
                                If Not dcProxy.ClientesAficiones.HasChanges And Not dcProxy.ClientesDeportes.HasChanges And Not dcProxy.ClientesAccionistas.HasChanges And Not dcProxy.ClientesFichas.HasChanges Then
                                    dcProxy.ClientesAficiones.Clear()
                                    dcProxy.Load(dcProxy.Traer_ClientesAficionesQuery(0, ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesAficiones, Nothing)
                                    dcProxy.ClientesDeportes.Clear()
                                    dcProxy.Load(dcProxy.Traer_ClientesDeportesQuery(0, ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDeportes, Nothing)
                                    dcProxy.ClientesAccionistas.Clear()
                                    dcProxy.Load(dcProxy.Traer_ClientesAccionistasQuery(0, ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesAccionistas, Nothing)
                                    dcProxy.ClientesFichas.Clear()
                                    dcProxy.Load(dcProxy.Traer_ClientesFichaClienteQuery(0, ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesFicha, Nothing)
                                    dcProxy.ClientesPersonasDepEconomicas.Clear()
                                    dcProxy.Load(dcProxy.Traer_ClientesDepEconomicaQuery(0, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDepEconomica, Nothing)
                                End If
                            Case TAB_PRINCIPAL_GENERAL_UBICACION
                                If Not dcProxy.ClientesDirecciones.HasChanges Then
                                    dcProxy.ClientesDirecciones.Clear()
                                    dcProxy.Load(dcProxy.Traer_ClientesDireccionesQuery(0, ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDirecciones, Nothing)
                                End If
                            Case TAB_PRINCIPAL_GENERAL_DOCUMRECIBIDOS
                                If Not dcProxy.ClientesDocumentosRequeridos.HasChanges Then
                                    dcProxy.ClientesDocumentosRequeridos.Clear()
                                    dcProxy.Load(dcProxy.Traer_ClientesDocumentosRequeridosQuery(ClienteSelected.IDComitente, ClienteSelected.TipoPersona, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDocumentosRequeridos, Nothing)
                                End If
                            Case TAB_PRINCIPAL_GENERAL_IR
                                If Not dcProxy.ClientesLOGHistoriaIRs.HasChanges And Not dcProxy.ClientesConocimientoEspecificos.HasChanges Then
                                    If configuraloghir Then
                                        dcProxy.ClientesLOGHistoriaIRs.Clear()
                                        dcProxy.Load(dcProxy.Traer_ClientesLOGHistoriaIQuery(ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesLOGHistoriaI, Nothing)
                                    End If
                                    If configuraconocimiento Then
                                        dcProxy.ClientesConocimientoEspecificos.Clear()
                                        ListaClientesConocimientoEspecificoclase.Clear()
                                        Contadorconocimientos = 0
                                        dcProxy.Load(dcProxy.Traer_ClientesConocimientoEspecificoQuery(0, ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesConocimientoEspecifico, Nothing)
                                    End If
                                End If
                            Case TAB_PRINCIPAL_GENERAL_PERSONASPORCONFIRMAR
                                If Not dcProxy.ClientesPersonasParaConfirmars.HasChanges Then
                                    dcProxy.ClientesPersonasParaConfirmars.Clear()
                                    dcProxy.Load(dcProxy.Traer_ClientesPersonasQuery(0, ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesPersonas, Nothing)
                                End If
                        End Select

                    Else
                        Select Case value

                            Case TAB_PRINCIPAL_GENERAL_FINANCIERO
                                If Not dcProxy.CuentasClientes.HasChanges And Not dcProxy.TipoClients.HasChanges Then
                                    dcProxy.CuentasClientes.Clear()
                                    dcProxy.Load(dcProxy.Traer_CuentasClientesQuery(1, ClienteSelected.IDComitente, False, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasclientes, Nothing)
                                    dcProxy.TipoClients.Clear()
                                    dcProxy.Load(dcProxy.TipoClienteFiltrarQuery(1, ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesTipoCliente, Nothing)
                                    dcProxy.ClientesProductos.Clear()
                                    dcProxy.Load(dcProxy.ConsultarClientesProductosQuery(ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarClientesProductos, Nothing)
                                End If
                                'NAOC20141118 - Fatca
                                '**********************************************************************
                                If Not dcProxy.ClientesPaisesFATCAs.HasChanges Then
                                    dcProxy.ClientesPaisesFATCAs.Clear()
                                    dcProxy.Load(dcProxy.ConsultarClientesPaisesFATCAQuery(1, ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesPaisesFatca, Nothing)
                                End If
                            '**********************************************************************
                            Case TAB_PRINCIPAL_GENERAL_RECEPTORES
                                If Not dcProxy.ClientesReceptores.HasChanges Then
                                    dcProxy.ClientesReceptores.Clear()
                                    dcProxy.Load(dcProxy.Traer_ClientesReceptoreQuery(1, ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesReceptores, Nothing)
                                End If
                            Case TAB_PRINCIPAL_GENERAL_ORDENANTES
                                If Not dcProxy.ClientesOrdenantes.HasChanges Then
                                    dcProxy.ClientesOrdenantes.Clear()
                                    dcProxy.Load(dcProxy.Traer_ClientesOrdenanteQuery(1, ClienteSelected.IDComitente, Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesOrdenantes, Nothing)
                                End If
                            Case TAB_PRINCIPAL_GENERAL_BENEFICIARIOS
                                If Not dcProxy.ClientesBeneficiarios.HasChanges Then
                                    dcProxy.ClientesBeneficiarios.Clear()
                                    dcProxy.Load(dcProxy.Traer_ClientesBeneficiariosQuery(1, ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesBeneficiarios, Nothing)
                                End If
                            Case TAB_PRINCIPAL_GENERAL_MERCADEO
                                If Not dcProxy.ClientesAficiones.HasChanges And Not dcProxy.ClientesDeportes.HasChanges And Not dcProxy.ClientesAccionistas.HasChanges And Not dcProxy.ClientesFichas.HasChanges Then
                                    dcProxy.ClientesAficiones.Clear()
                                    dcProxy.Load(dcProxy.Traer_ClientesAficionesQuery(1, ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesAficiones, Nothing)
                                    dcProxy.ClientesDeportes.Clear()
                                    dcProxy.Load(dcProxy.Traer_ClientesDeportesQuery(1, ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDeportes, Nothing)
                                    dcProxy.ClientesAccionistas.Clear()
                                    dcProxy.Load(dcProxy.Traer_ClientesAccionistasQuery(1, ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesAccionistas, Nothing)
                                    dcProxy.ClientesFichas.Clear()
                                    dcProxy.Load(dcProxy.Traer_ClientesFichaClienteQuery(1, ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesFicha, Nothing)
                                    dcProxy.ClientesPersonasDepEconomicas.Clear()
                                    dcProxy.Load(dcProxy.Traer_ClientesDepEconomicaQuery(1, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDepEconomica, Nothing)
                                End If
                            Case TAB_PRINCIPAL_GENERAL_UBICACION
                                If Not dcProxy.ClientesDirecciones.HasChanges Then
                                    dcProxy.ClientesDirecciones.Clear()
                                    dcProxy.Load(dcProxy.Traer_ClientesDireccionesQuery(1, ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDirecciones, Nothing)
                                End If
                            Case TAB_PRINCIPAL_GENERAL_DOCUMRECIBIDOS
                                If Not dcProxy.ClientesDocumentosRequeridos.HasChanges Then
                                    dcProxy.ClientesDocumentosRequeridos.Clear()
                                    dcProxy.Load(dcProxy.Traer_ClientesDocumentosRequeridosQuery(ClienteSelected.IDComitente, ClienteSelected.TipoPersona, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDocumentosRequeridos, Nothing)
                                End If
                            Case TAB_PRINCIPAL_GENERAL_IR
                                If Not dcProxy.ClientesLOGHistoriaIRs.HasChanges And Not dcProxy.ClientesConocimientoEspecificos.HasChanges Then
                                    If configuraloghir Then
                                        dcProxy.ClientesLOGHistoriaIRs.Clear()
                                        dcProxy.Load(dcProxy.Traer_ClientesLOGHistoriaIQuery(ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesLOGHistoriaI, Nothing)
                                    End If
                                    If configuraconocimiento Then
                                        dcProxy.ClientesConocimientoEspecificos.Clear()
                                        ListaClientesConocimientoEspecificoclase.Clear()
                                        Contadorconocimientos = 0
                                        dcProxy.Load(dcProxy.Traer_ClientesConocimientoEspecificoQuery(1, ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesConocimientoEspecifico, Nothing)
                                    End If
                                End If
                            Case TAB_PRINCIPAL_GENERAL_PERSONASPORCONFIRMAR
                                If Not dcProxy.ClientesPersonasParaConfirmars.HasChanges Then
                                    dcProxy.ClientesPersonasParaConfirmars.Clear()
                                    dcProxy.Load(dcProxy.Traer_ClientesPersonasQuery(1, ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesPersonas, Nothing)
                                End If
                        End Select
                    End If
                End If
            Else
                Select Case value
                    Case TAB_PRINCIPAL_GENERAL_MERCADEO
                        Dim strReceptorEntrevista As String = String.Empty
                        If Not IsNothing(_ClienteSelected) Then
                            strReceptorEntrevista = _ClienteSelected.ReceptorEntrevista
                        End If
                        ListaClientesReceptoreEntrevista = Nothing
                        If Not IsNothing(_ListaClientesReceptore) Then
                            ListaClientesReceptoreEntrevista = _ListaClientesReceptore.ToList
                        End If
                        If Not String.IsNullOrEmpty(strReceptorEntrevista) Then
                            _ClienteSelected.ReceptorEntrevista = strReceptorEntrevista
                        End If
                End Select
            End If

            _TabSeleccionadaFinanciero = value
            MyBase.CambioItem("TabSeleccionadaFinanciero")

        End Set
    End Property
    Private _TabSeleccionadaPrincipal As Integer = 0
    Public Property TabSeleccionadaPrincipal
        Get
            Return _TabSeleccionadaPrincipal
        End Get
        Set(ByVal value)
            Select Case value

                Case TAB_PRINCIPAL_ORDENES
                    ListaClientesOrdenes.Clear()
                    dcProxy.ClientesOrdenes.Clear()
                    dcProxy.Load(dcProxy.TraerClientesOrdenesQuery(ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesOrdenes, Nothing)
                Case TAB_PRINCIPAL_TESORERIA
                    ListaTesoreria.Clear()
                    dcProxy.ClientesTesorerias.Clear()
                    dcProxy.Load(dcProxy.TraerClientesTesoreriaQuery(ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientestesoreria, Nothing)
                Case TAB_PRINCIPAL_DEPOSITO
                    ListaCuentadeposito.Clear()
                    dcProxy.CuentasDepositos.Clear()
                    dcProxy.Load(dcProxy.TraerClientesDepositoQuery(ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesDeposito, Nothing)
                Case TAB_PRINCIPAL_PORTAFOLIO
                    If ClientesSeBoolean.IsbusyPortafolio = False Then
                        If Listaportafolio.Count = 0 Then
                            textoPortafolio = String.Empty
                            ClientesSeBoolean.IsbusyPortafolio = True
                            Listaportafolio.Clear()
                            dcProxy.Portafolios.Clear()
                            dcProxy.Load(dcProxy.TraerPortafolioQuery(ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPortafolio, Nothing)
                        Else
                            If Listaportafolio.First.IDComitente <> ClienteSelected.IDComitente Then
                                textoPortafolio = String.Empty
                                ClientesSeBoolean.IsbusyPortafolio = True
                                Listaportafolio.Clear()
                                dcProxy.Portafolios.Clear()
                                dcProxy.Load(dcProxy.TraerPortafolioQuery(ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerPortafolio, Nothing)
                            End If
                        End If
                    End If
                Case TAB_PRINCIPAL_ACCIONES
                    ListaClientesAcciones.Clear()
                    dcProxy.ClientesAcciones.Clear()
                    dcProxy.Load(dcProxy.TraerClientesAccionesQuery(ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesAcciones, Nothing)
                Case TAB_PRINCIPAL_RENTAFIJA
                    ListaClientesRentaFija.Clear()
                    dcProxy.ClientesRentaFijas.Clear()
                    dcProxy.Load(dcProxy.TraerClientesRentaFijaQuery(ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesRentafija, Nothing)
                Case TAB_PRINCIPAL_CUSTODIAS
                    IsBusyCustodias = True
                    ListaCustodias.Clear()
                    dcProxy.ClientesCustodias.Clear()
                    dcProxy.Load(dcProxy.TraerClientesCustodiasQuery(_ClienteSelected.IDComitente, FechaCorte, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesCustodias, Nothing)
                    MyBase.CambioItem("ListaCustodias")
                Case TAB_PRINCIPAL_FACTURAS
                    ListaClientesFactura.Clear()
                    dcProxy.FacturasClis.Clear()
                    dcProxy.Load(dcProxy.TraerClientesFacturasQuery(ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesFactura, Nothing)
                Case TAB_PRINCIPAL_FONDOS
                    ConsultarFondos()
                Case TAB_PRINCIPAL_DIVISAS
                    ListaClientesDivisas.Clear()
                    dcProxy.Divisas.Clear()
                    dcProxy.Load(dcProxy.TraerClientesDivisasQuery(_ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesDivisas, Nothing)
                Case TAB_PRINCIPAL_VENCIMIENTO
                    ListaVencimientos.Clear()
                    MyBase.CambioItem("ListaVencimientos")
                Case TAB_PRINCIPAL_LIQXCUMPLIR
                    ListaLiqxCumplir.Clear()
                    MyBase.CambioItem("ListaLiqxCumplir")
                Case TAB_PRINCIPAL_REPO
                    ListaRepos.Clear()
                    MyBase.CambioItem("ListaRepos")
                Case TAB_PRINCIPAL_FONDOS
                    ListaFondos.Clear()
                    ListaFondosTotales.Clear()
                    MyBase.CambioItem("ListaFondos")
                Case TAB_PRINCIPAL_TOTALES
                    ListaClseTotales.Clear()
                    dcProxy.ClientesTotales.Clear()
                    dcProxy.Load(dcProxy.TraerClientesTotalesQuery(ClienteSelected.IDComitente, Now, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesTotales, Nothing)
                Case TAB_PRINCIPAL_SALDOS
                    ListaSaldos.Clear()
                    dcProxy.ClientesSaldos.Clear()
                    'JFSB 20160926 Se agrega el usuario sin dominio
                    'CFMA20181008
                    'dcProxy.Load(dcProxy.TraerClientesSaldosQuery(ClienteSelected.IDComitente, Now, Program.UsuarioSinDomino, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesSaldos, Nothing)
                    dcProxy.Load(dcProxy.TraerClientesSaldosQuery(ClienteSelected.IDComitente, Now, Program.UsuarioSinDomino, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesSaldos, _ClienteSelected.IDComitente)
                    'CFMA20181008

                Case TAB_PRINCIPAL_ENCARGOS
                    ListaEncargos.Clear()
                    dcProxy.ClientesEncargos.Clear()
                    dcProxy.Load(dcProxy.TraerClientesEncargosQuery(ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesEncargos, Nothing)
            End Select



            _TabSeleccionadaPrincipal = value
            MyBase.CambioItem("TabSeleccionadaPrincipal")

        End Set
    End Property
    Private _objTipoId As List(Of OYDUtilidades.ItemCombo) = New List(Of OYDUtilidades.ItemCombo)
    Public Property objTipoId() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _objTipoId
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _objTipoId = value
            MyBase.CambioItem("objTipoId")
        End Set
    End Property
    Private _TipoIdentificacionEdicion As String
    Public Property TipoIdentificacionEdicion() As String
        Get
            Return _TipoIdentificacionEdicion
        End Get
        Set(ByVal value As String)
            _TipoIdentificacionEdicion = value
            If Editando And logEsNuevoRegistro Then
                _ClienteSelected.TipoIdentificacion = TipoIdentificacionEdicion
            End If
            MyBase.CambioItem("TipoIdentificacionEdicion")
        End Set
    End Property
    Private _objTipoIdBuscar As List(Of OYDUtilidades.ItemCombo) = New List(Of OYDUtilidades.ItemCombo)
    Public Property objTipoIdBuscar() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _objTipoIdBuscar
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _objTipoIdBuscar = value
            MyBase.CambioItem("objTipoIdBuscar")
        End Set
    End Property
    Private _parametroins As List(Of OYDUtilidades.ItemCombo) = New List(Of OYDUtilidades.ItemCombo)
    Public Property parametroins() As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _parametroins
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _parametroins = value
            MyBase.CambioItem("parametroins")
        End Set
    End Property

    Private _Historialir As HistoriaIR = New HistoriaIR
    Public Property Historialir() As HistoriaIR
        Get
            Return _Historialir
        End Get
        Set(ByVal value As HistoriaIR)
            _Historialir = value
            MyBase.CambioItem("Historialir")
        End Set
    End Property
    Private _Listaportafolio As List(Of OyDClientes.Portafolio) = New List(Of OyDClientes.Portafolio)
    Public Property Listaportafolio As List(Of OyDClientes.Portafolio)
        Get
            Return _Listaportafolio
        End Get
        Set(ByVal value As List(Of OyDClientes.Portafolio))
            _Listaportafolio = value
            MyBase.CambioItem("Listaportafolio")
        End Set
    End Property
    Private _ListaTesoreria As List(Of OyDClientes.ClientesTesoreria) = New List(Of OyDClientes.ClientesTesoreria)
    Public Property ListaTesoreria As List(Of OyDClientes.ClientesTesoreria)
        Get
            Return _ListaTesoreria
        End Get
        Set(ByVal value As List(Of OyDClientes.ClientesTesoreria))
            _ListaTesoreria = value
            MyBase.CambioItem("ListaTesoreria")
        End Set
    End Property
    Private _ListaCustodias As List(Of OyDClientes.ClientesCustodias) = New List(Of OyDClientes.ClientesCustodias)
    Public Property ListaCustodias As List(Of OyDClientes.ClientesCustodias)
        Get
            Return _ListaCustodias
        End Get
        Set(ByVal value As List(Of OyDClientes.ClientesCustodias))
            _ListaCustodias = value
            MyBase.CambioItem("ListaCustodias")
        End Set
    End Property
    Private _ListaVencimientos As List(Of OyDClientes.ClientesVencimientos) = New List(Of OyDClientes.ClientesVencimientos)
    Public Property ListaVencimientos As List(Of OyDClientes.ClientesVencimientos)
        Get
            Return _ListaVencimientos
        End Get
        Set(ByVal value As List(Of OyDClientes.ClientesVencimientos))
            _ListaVencimientos = value
            MyBase.CambioItem("ListaVencimientos")
        End Set
    End Property
    Private _ListaCuentadeposito As List(Of OyDClientes.CuentasDeposito) = New List(Of OyDClientes.CuentasDeposito)
    Public Property ListaCuentadeposito As List(Of OyDClientes.CuentasDeposito)
        Get
            Return _ListaCuentadeposito
        End Get
        Set(ByVal value As List(Of OyDClientes.CuentasDeposito))
            _ListaCuentadeposito = value
            MyBase.CambioItem("ListaCuentadeposito")
        End Set
    End Property
    Private _ListaClientesAcciones As List(Of OyDClientes.ClientesAcciones) = New List(Of OyDClientes.ClientesAcciones)
    Public Property ListaClientesAcciones As List(Of OyDClientes.ClientesAcciones)
        Get
            Return _ListaClientesAcciones
        End Get
        Set(ByVal value As List(Of OyDClientes.ClientesAcciones))
            _ListaClientesAcciones = value
            MyBase.CambioItem("ListaClientesAcciones")
        End Set
    End Property
    Private _ListaClientesRentaFija As List(Of OyDClientes.ClientesRentaFija) = New List(Of OyDClientes.ClientesRentaFija)
    Public Property ListaClientesRentaFija As List(Of OyDClientes.ClientesRentaFija)
        Get
            Return _ListaClientesRentaFija
        End Get
        Set(ByVal value As List(Of OyDClientes.ClientesRentaFija))
            _ListaClientesRentaFija = value
            MyBase.CambioItem("ListaClientesRentaFija")
        End Set
    End Property
    Private _ListaClientesOrdenes As List(Of OyDClientes.ClientesOrdenes) = New List(Of OyDClientes.ClientesOrdenes)
    Public Property ListaClientesOrdenes As List(Of OyDClientes.ClientesOrdenes)
        Get
            Return _ListaClientesOrdenes
        End Get
        Set(ByVal value As List(Of OyDClientes.ClientesOrdenes))
            _ListaClientesOrdenes = value
            MyBase.CambioItem("ListaClientesOrdenes")
        End Set
    End Property
    Private _textoPortafolio As String
    Public Property textoPortafolio As String
        Get
            Return _textoPortafolio
        End Get
        Set(ByVal value As String)
            _textoPortafolio = value
            MyBase.CambioItem("textoPortafolio")
        End Set
    End Property
    Private _FechaCorte As DateTime = Now.Date
    Public Property FechaCorte As DateTime
        Get
            Return _FechaCorte
        End Get
        Set(ByVal value As DateTime)
            _FechaCorte = value
            MyBase.CambioItem("FechaCorte")
        End Set
    End Property
    Private _ListaClientesFactura As List(Of OyDClientes.FacturasCli) = New List(Of OyDClientes.FacturasCli)
    Public Property ListaClientesFactura As List(Of OyDClientes.FacturasCli)
        Get
            Return _ListaClientesFactura
        End Get
        Set(ByVal value As List(Of OyDClientes.FacturasCli))
            _ListaClientesFactura = value
            MyBase.CambioItem("ListaClientesFactura")
        End Set
    End Property
    Private _ListaClientesDivisas As List(Of OyDClientes.Divisas) = New List(Of OyDClientes.Divisas)
    Public Property ListaClientesDivisas As List(Of OyDClientes.Divisas)
        Get
            Return _ListaClientesDivisas
        End Get
        Set(value As List(Of OyDClientes.Divisas))
            _ListaClientesDivisas = value
            MyBase.CambioItem("ListaClientesDivisas")
        End Set
    End Property
    Private _ListaLiqxCumplir As List(Of OyDClientes.ClientesLiqxCumplir) = New List(Of OyDClientes.ClientesLiqxCumplir)
    Public Property ListaLiqxCumplir As List(Of OyDClientes.ClientesLiqxCumplir)
        Get
            Return _ListaLiqxCumplir
        End Get
        Set(ByVal value As List(Of OyDClientes.ClientesLiqxCumplir))
            _ListaLiqxCumplir = value
            MyBase.CambioItem("ListaLiqxCumplir")
        End Set
    End Property
    Private _ListaRepos As List(Of OyDClientes.ClientesRepo) = New List(Of OyDClientes.ClientesRepo)
    Public Property ListaRepos As List(Of OyDClientes.ClientesRepo)
        Get
            Return _ListaRepos
        End Get
        Set(ByVal value As List(Of OyDClientes.ClientesRepo))
            _ListaRepos = value
            MyBase.CambioItem("ListaRepos")
        End Set
    End Property

    Private _IsBusyFondos As Boolean
    Public Property IsBusyFondos() As Boolean
        Get
            Return _IsBusyFondos
        End Get
        Set(ByVal value As Boolean)
            _IsBusyFondos = value
            MyBase.CambioItem("IsBusyFondos")
        End Set
    End Property
    Private Property _ListaFondos As List(Of OyDClientes.ClientesFondos) = New List(Of OyDClientes.ClientesFondos)
    Public Property ListaFondos As List(Of OyDClientes.ClientesFondos)
        Get
            Return _ListaFondos
        End Get
        Set(ByVal value As List(Of OyDClientes.ClientesFondos))
            _ListaFondos = value
            MyBase.CambioItem("ListaFondos")
        End Set
    End Property
    Private Property _ListaFondosTotales As List(Of OyDClientes.ClientesFondosTotales) = New List(Of OyDClientes.ClientesFondosTotales)
    Public Property ListaFondosTotales As List(Of OyDClientes.ClientesFondosTotales)
        Get
            Return _ListaFondosTotales
        End Get
        Set(ByVal value As List(Of OyDClientes.ClientesFondosTotales))
            _ListaFondosTotales = value
            MyBase.CambioItem("ListaFondosTotales")
        End Set
    End Property
    Private _FechaCorteFondos As DateTime = Now.Date
    Public Property FechaCorteFondos As DateTime
        Get
            Return _FechaCorteFondos
        End Get
        Set(ByVal value As DateTime)
            _FechaCorteFondos = value
            MyBase.CambioItem("FechaCorteFondos")
        End Set
    End Property


    Private _ClientesTotalespaged As PagedCollectionView
    Public Property ClientesTotalespaged() As PagedCollectionView
        Get
            Return _ClientesTotalespaged
        End Get
        Set(value As PagedCollectionView)
            _ClientesTotalespaged = value
            MyBase.CambioItem("ClientesTotalespaged")
        End Set
    End Property
    Private _ListaClseTotales As ObservableCollection(Of ClientesTotalesgroup) = New ObservableCollection(Of ClientesTotalesgroup)
    Public Property ListaClseTotales As ObservableCollection(Of ClientesTotalesgroup)
        Get
            Return _ListaClseTotales
        End Get
        Set(ByVal value As ObservableCollection(Of ClientesTotalesgroup))
            _ListaClseTotales = value
            MyBase.CambioItem("ListaClseTotales")
        End Set
    End Property
    Private _ListaSaldos As ObservableCollection(Of ClientesSaldosG) = New ObservableCollection(Of ClientesSaldosG)
    Public Property ListaSaldos As ObservableCollection(Of ClientesSaldosG)
        Get
            Return _ListaSaldos
        End Get
        Set(ByVal value As ObservableCollection(Of ClientesSaldosG))
            _ListaSaldos = value
            MyBase.CambioItem("ListaSaldos")
        End Set
    End Property
    Private _ListaEncargos As List(Of OyDClientes.ClientesEncargos) = New List(Of OyDClientes.ClientesEncargos)
    Public Property ListaEncargos As List(Of OyDClientes.ClientesEncargos)
        Get
            Return _ListaEncargos
        End Get
        Set(ByVal value As List(Of OyDClientes.ClientesEncargos))
            _ListaEncargos = value
            MyBase.CambioItem("ListaEncargos")
        End Set
    End Property
    Private _FechaInicial As DateTime = Now.Date
    Public Property FechaInicial As DateTime
        Get
            Return _FechaInicial
        End Get
        Set(ByVal value As DateTime)
            _FechaInicial = value
            MyBase.CambioItem("FechaInicial")
        End Set
    End Property
    Private _FechaFinal As DateTime = Now.Date
    Public Property FechaFinal As DateTime
        Get
            Return _FechaFinal
        End Get
        Set(ByVal value As DateTime)
            _FechaFinal = value
            MyBase.CambioItem("FechaFinal")
        End Set
    End Property
    Private _TabSeleccionadaMercadeo As Integer = 0
    Public Property TabSeleccionadaMercadeo As Integer
        Get
            Return _TabSeleccionadaMercadeo
        End Get
        Set(value As Integer)
            _TabSeleccionadaMercadeo = value
            MyBase.CambioItem("TabSeleccionadaMercadeo")
        End Set
    End Property
    Private _ClientesSeVisibility As New ClientesVisibility
    Public Property ClientesSeVisibility As ClientesVisibility
        Get
            Return _ClientesSeVisibility
        End Get
        Set(value As ClientesVisibility)
            _ClientesSeVisibility = value
            MyBase.CambioItem("ClientesSeVisibility")
        End Set
    End Property
    Private _ClientesSeBoolean As New ClientesBoolean
    Public Property ClientesSeBoolean As ClientesBoolean
        Get
            Return _ClientesSeBoolean
        End Get
        Set(value As ClientesBoolean)
            _ClientesSeBoolean = value
            MyBase.CambioItem("ClientesSeBoolean")
        End Set
    End Property
    Private _CiudadesClientes As CiudadesGenerales = New CiudadesGenerales
    Public Property CiudadesClientes As CiudadesGenerales
        Get
            Return _CiudadesClientes
        End Get
        Set(ByVal value As CiudadesGenerales)
            _CiudadesClientes = value
            MyBase.CambioItem("CiudadesClientes")
        End Set
    End Property
    Private _LimiteFecha As DateTime = Now.Date
    Public Property LimiteFecha As DateTime
        Get
            Return _LimiteFecha
        End Get
        Set(ByVal value As DateTime)
            _LimiteFecha = value
            MyBase.CambioItem("LimiteFecha")
        End Set
    End Property
    Private _CamposSeGenerales As CamposGenerales = New CamposGenerales
    Public Property CamposSeGenerales As CamposGenerales
        Get
            Return _CamposSeGenerales
        End Get
        Set(value As CamposGenerales)
            _CamposSeGenerales = value
            MyBase.CambioItem("CamposSeGenerales")
        End Set
    End Property
    Private _Fechadesplegada As DateTime
    Public Property Fechadesplegada As DateTime
        Get
            Return _Fechadesplegada
        End Get
        Set(ByVal value As DateTime)
            _Fechadesplegada = value
            MyBase.CambioItem("Fechadesplegada")
        End Set
    End Property

    ' eomc -- 12/12/2013 -- Inicio
    Private _labelCodigoCIIU As String = "Actividad Econo"
    Public Property labelCodigoCIIU As String
        Get
            Return _labelCodigoCIIU
        End Get
        Set(ByVal value As String)
            _labelCodigoCIIU = value
            MyBase.CambioItem("labelCodigoCIIU")
        End Set
    End Property
    Private _labelActividadEconomica As String = "Actividad eco.(anterior)"
    Public Property labelActividadEconomica As String
        Get
            Return _labelActividadEconomica
        End Get
        Set(ByVal value As String)
            _labelActividadEconomica = value
            MyBase.CambioItem("labelActividadEconomica")
        End Set
    End Property
    ' eomc -- 12/12/2013 -- Fin

    Private _IsBusyCustodias As Boolean = False
    Public Property IsBusyCustodias As Boolean
        Get
            Return _IsBusyCustodias
        End Get
        Set(ByVal value As Boolean)
            _IsBusyCustodias = value
            MyBase.CambioItem("IsBusyCustodias")
        End Set
    End Property

    Private _ListaRespuestaValidacionesCientes As List(Of OyDClientes.tblRespuestaValidacionesCientes) = New List(Of OyDClientes.tblRespuestaValidacionesCientes)
    Public Property ListaRespuestaValidacionesCientes As List(Of OyDClientes.tblRespuestaValidacionesCientes)
        Get
            Return _ListaRespuestaValidacionesCientes
        End Get
        Set(ByVal value As List(Of OyDClientes.tblRespuestaValidacionesCientes))
            _ListaRespuestaValidacionesCientes = value
            MyBase.CambioItem("ListaRespuestaValidacionesCientes")
        End Set
    End Property
    Private _SelectedValidacionesCientes As New OyDClientes.tblRespuestaValidacionesCientes
    Public Property SelectedValidacionesCientes As OyDClientes.tblRespuestaValidacionesCientes
        Get
            Return _SelectedValidacionesCientes
        End Get
        Set(ByVal value As OyDClientes.tblRespuestaValidacionesCientes)
            _SelectedValidacionesCientes = value
            MyBase.CambioItem("SelectedValidacionesCientes")
        End Set
    End Property

    ''' <history>
    ''' Descripción:    Propiedad que indica si la pestaña clasificaciones está activa o inactiva.
    ''' Responsable:    Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:          29 de Septiembre/2014
    ''' Pruebas CB:     Jorge Peña (Alcuadrado S.A.) - 29 de Septiembre/2014
    ''' </history>
    Private _HABILITAR_CLASIFICACIONES_EN_CLIENTES As Visibility = Visibility.Collapsed
    Public Property HABILITAR_CLASIFICACIONES_EN_CLIENTES() As Visibility
        Get
            Return _HABILITAR_CLASIFICACIONES_EN_CLIENTES
        End Get
        Set(ByVal value As Visibility)
            _HABILITAR_CLASIFICACIONES_EN_CLIENTES = value
            MyBase.CambioItem("HABILITAR_CLASIFICACIONES_EN_CLIENTES")
        End Set
    End Property

    Private _TextoAccionnistaContribuyente As String = String.Format("Dentro de la persona juridica que represento, al menos{0}uno de los accionistas que tienen un porcentaje igual{0}o superior al 10% de la participación, es contribuyente{0}del gobierno Estadounidense", vbCrLf)
    Public Property TextoAccionnistaContribuyente() As String
        Get
            Return _TextoAccionnistaContribuyente
        End Get
        Set(ByVal value As String)
            _TextoAccionnistaContribuyente = value
            MyBase.CambioItem("TextoAccionnistaContribuyente")
        End Set
    End Property

    ''' <summary>
    ''' Lista de ProcesarPortafolio que se encuentran cargadas en el grid del formulario
    ''' </summary>
    Private _ListaPantallasParametrizacion As List(Of PantallasParametrizacion)
    Public Property ListaPantallasParametrizacion() As List(Of PantallasParametrizacion)
        Get
            Return _ListaPantallasParametrizacion
        End Get
        Set(ByVal value As List(Of PantallasParametrizacion))
            _ListaPantallasParametrizacion = value

            MyBase.CambioItem("ListaPantallasParametrizacion")
        End Set
    End Property
#End Region
#Region "Métodos"

    Private Function RetornarValorDiccionario(ByVal pstrTopico As String) As String
        Dim strRetornoValorID As String = String.Empty
        If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
            If CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey(pstrTopico) Then
                If CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item(pstrTopico).Count > 0 Then
                    strRetornoValorID = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item(pstrTopico).FirstOrDefault.ID '"N"
                End If
            End If
        End If
        Return strRetornoValorID
    End Function

    ''' <history>
    ''' ID caso de prueba:   CP0005, CP0006
    ''' Descripción:         Método que alimenta la lista ListaClientesProductos.
    ''' Responsable:         Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:               29 de Septiembre/2014
    ''' Pruebas CB:          Jorge Peña (Alcuadrado S.A.) - 29 de Septiembre/2014
    ''' </history>
    Public Overrides Sub NuevoRegistro()
        Try
            'IsBusy = True
            If dcProxy.IsLoading Or Not logTerminoCargarClasificaciones Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            Dim NewCliente As New OyDClientes.Cliente
            'TODO: Verificar cuales son los campos que deben inicializarse
            EditandoReceptores = True
            NewCliente.IDComitente = "-1"
            NewCliente.TipoPersona = 1
            NewCliente.strNroDocumento = Nothing
            NewCliente.IDPoblacionDoc = InstalacioSelected.IdPoblacion
            NewCliente.TipoVinculacion = "C"
            NewCliente.Ingreso = Now.Date
            NewCliente.IDPoblacion = InstalacioSelected.IdPoblacion
            NewCliente.IdCiudadNacimiento = InstalacioSelected.IdPoblacion
            NewCliente.RetFuente = Nothing
            NewCliente.Activo = True
            NewCliente.SuperValores = Now.Date
            'NewCliente.Admonvalores = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("ADMONVALORES").FirstOrDefault.ID '"N"
            NewCliente.Admonvalores = RetornarValorDiccionario("ADMONVALORES")
            NewCliente.Contribuyente = Nothing
            NewCliente.Nacimiento = Nothing
            NewCliente.ActualizacionFicha = Now.Date
            NewCliente.NoLlamarAlCliente = "1"
            NewCliente.NacimientoRepresentanteLegal = Nothing
            NewCliente.Entrevista = Nothing
            NewCliente.Actualizacion = Now.Date
            NewCliente.Usuario = Program.Usuario
            NewCliente.EstadoCliente = "A"
            NewCliente.UltimoMov = Now.Date
            NewCliente.FondoExtranjero = False
            'Santiago Vergara - Junio 20/2014 - Se adiciona asignación de valores por defecto 
            NewCliente.ReplicarSafyrFondos = REPLICAR_FONDOS
            NewCliente.ReplicarSafyrPortafolios = REPLICAR_PORTAFOLIOS
            NewCliente.ReplicarSafyrClientes = REPLICAR_CLIENTES
            NewCliente.ReplicarMercansoft = REPLICAR_MERCAMSOFT

            'SV20160127
            If Not IsNothing(NewCliente.ReplicarSafyrFondos) AndAlso NewCliente.ReplicarSafyrFondos = True Then
                ClientesSeBoolean.Fondos = True
            Else
                ClientesSeBoolean.Fondos = False
            End If

            If Not IsNothing(NewCliente.ReplicarSafyrPortafolios) AndAlso NewCliente.ReplicarSafyrPortafolios = True Then
                ClientesSeBoolean.Portafolios = True
            Else
                ClientesSeBoolean.Portafolios = False
            End If

            If Not IsNothing(NewCliente.ReplicarSafyrClientes) AndAlso NewCliente.ReplicarSafyrClientes = True Then
                ClientesSeBoolean.Clientes = True
            Else
                ClientesSeBoolean.Clientes = False
            End If

            If Not IsNothing(NewCliente.ReplicarMercansoft) AndAlso NewCliente.ReplicarMercansoft = True Then
                ClientesSeBoolean.Mercasonft = True
            Else
                ClientesSeBoolean.Mercasonft = False
            End If

            NewCliente.Declarante = Nothing
            NewCliente.AutoRetenedor = Nothing
            NewCliente.ExentoGMF = Nothing
            NewCliente.RETEICA = Nothing
            NewCliente.CREE = Nothing
            NewCliente.FechaExpedicionDoc = Now.Date
            NewCliente.AceptaCruces = True
            NewCliente.IDClientes = -1
            NewCliente.Embajada = False
            NewCliente.IdProfesion = 0
            NewCliente.IDCiudadReprLegal = 0
            NewCliente.IDSucCliente = String.Empty
            NewCliente.OrigenFondo = "O"
            NewCliente.IDConcepto = 0
            NewCliente.FactorComisionCliente = 0
            NewCliente.IngresoMensual = Nothing
            NewCliente.Activos = Nothing
            NewCliente.Patrimonio = Nothing
            NewCliente.Egresos = Nothing
            NewCliente.Pasivos = Nothing
            NewCliente.Utilidades = Nothing
            NewCliente.IdCiudadNacimiento = Nothing
            NewCliente.AdmonInvExterior = False
            NewCliente.AutorizaTratamiento = Nothing
            NewCliente.AdministradoBanco = False

            If TIPOPRODUCTOCLIENTEDEFECTO <> "-1" Then
                Dim lista = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOPRODUCTO").Where(Function(id) id.ID = TIPOPRODUCTOCLIENTEDEFECTO)
                If lista.Count = 0 Then
                    'NewCliente.TipoProducto = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOPRODUCTO").FirstOrDefault.ID
                    NewCliente.Admonvalores = RetornarValorDiccionario("TIPOPRODUCTO")
                Else
                    NewCliente.TipoProducto = TIPOPRODUCTOCLIENTEDEFECTO
                End If
            Else
                NewCliente.TipoProducto = TIPOPRODUCTOCLIENTEDEFECTO
            End If
            If PERFILCLIENTEDEFECTO <> "-1" Then
                'Dim listaperfil = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("PERFIL").Where(Function(id) id.ID = PERFILCLIENTEDEFECTO)
                Dim listaperfil = RetornarValorDiccionario("PERFIL")
                If listaperfil.Count = 0 Then
                    NewCliente.Perfil = "5"
                Else
                    NewCliente.Perfil = PERFILCLIENTEDEFECTO
                End If
            Else
                NewCliente.Perfil = PERFILCLIENTEDEFECTO
            End If
            'NewCliente.TipoCliente = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPODECLIENTE").FirstOrDefault.ID
            NewCliente.TipoCliente = RetornarValorDiccionario("TIPODECLIENTE")
            'NewCliente.FormaPago = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("FORMAPAGO").FirstOrDefault.ID
            NewCliente.FormaPago = RetornarValorDiccionario("FORMAPAGO")
            'NewCliente.CategoriaCliente = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("CATEGORIACLI").FirstOrDefault.ID
            NewCliente.CategoriaCliente = RetornarValorDiccionario("CATEGORIACLI")
            'NewCliente.PerfilRiesgo = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("PERFILRIESGO").FirstOrDefault.ID
            NewCliente.PerfilRiesgo = RetornarValorDiccionario("PERFILRIESGO")
            'NAOC20141118 - Fatca
            '**********************************************************************
            If FATCA_CAMPOSADICIONALES Then
                NewCliente.CiudadanoResidenteDomicilio = False
                NewCliente.AplicaFatca = False
                NewCliente.TransfiereACuentasEEUU = False
                NewCliente.TitularCuentaEEUU = String.Empty
                NewCliente.EntidadTransferencia = String.Empty
                NewCliente.EmpresaListadaBolsa = False
                NewCliente.SubsidiariaDeEntidad = False
                NewCliente.AccionistaContribuyenteEEUU = False
                NewCliente.SinAnimoDeLucroFatca = False
                NewCliente.InstitucionFinanciera = False
                NewCliente.Regulador = String.Empty
                NewCliente.MercadoNegociaAcciones = String.Empty
                NewCliente.EmpresaMatriz = String.Empty
                NewCliente.GIIN = String.Empty
            End If

            NewCliente.strInstruccionCRCC = PantallasParametrizacionTipoString("strInstruccionCRCC", "strValorPorDefecto")

            '**********************************************************************

            'objProxy.Verificaparametro("GRUPOPORDEFECTO",Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "GRUPOPORDEFECTO")
            'objProxy.Verificaparametro("SUBGRUPOPORDEFECTO",Program.Usuario, Program.HashConexion, AddressOf Terminotraerparametro, "SUBGRUPOPORDEFECTO")
            ClienteAnterior = ClienteSelected
            ClienteSelected = NewCliente
            dcProxy.RejectChanges()
            ClienteSelected.IDGrupo = mintGrupoPorDefecto
            dcProxy.ClientesDocumentosRequeridos.Clear()
            dcProxy.Load(dcProxy.Traer_ClientesDocumentosRequeridosQuery(ClienteSelected.IDComitente, ClienteSelected.TipoPersona, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDocumentosRequeridos, Nothing)
            dcProxy.ClientesProductos.Clear()
            dcProxy.Load(dcProxy.ConsultarClientesProductosQuery(ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarClientesProductos, Nothing)
            CiudadesClientes.strdepartamento = ""
            CiudadesClientes.strPais = ""
            CiudadesClientes.strPoblacion = ""
            CiudadesClientes.strdepartamentoDoc = ""
            CiudadesClientes.strPaisDoc = ""
            CiudadesClientes.strPoblaciondoc = ""
            CamposSeGenerales.strcodigociu = ""
            CiudadesClientes.strciudad = ""
            CiudadesClientes.strciudadNacimiento = ""
            CamposSeGenerales.strProfesion = ""
            TipoIdentificacionEdicion = String.Empty
            If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                objTipoId = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOIDC1")
            End If
            Editando = True
            ClientesSeBoolean.Editarcampos = True
            ClientesSeBoolean.Editareg = True
            ClientesSeBoolean.Read = False
            ClientesSeBoolean.HabilitaBus = False

            If ConfiguraFirma.Equals("NO") Then
                ClientesSeBoolean.HabilitaAE = True
            End If
            'ClientesSeBoolean.Editanrdcto = Not InstalacioSelected.ClientesCedula
            'JFSB 20171009
            ClientesSeBoolean.Editanrdcto = InstalacioSelected.ClientesCedula
            If InstalacioSelected.ClientesAutomatico = 0 Then
                ClientesSeBoolean.Comitente = True
            Else
                ClientesSeBoolean.Comitente = False
            End If
            ClientesSeBoolean.HabilitaDepEconomica = False
            If ClienteSelected.TipoPersona = 2 Then
                ClientesSeBoolean.HabilitaCampoPNatural = False
                ClientesSeBoolean.HabilitaCampoPJuridica = True
            Else
                ClientesSeBoolean.HabilitaCampoPNatural = True
                ClientesSeBoolean.HabilitaCampoPJuridica = False
            End If
            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_GENERAL
            TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
            ClientesSeVisibility.TipoPersonaNatural = Visibility.Visible
            ClientesSeVisibility.TipoPersonaJuridica = Visibility.Collapsed
            ClientesSeVisibility.VisivilidadMenu = Visibility.Visible

            ClienteSelected.TipoCheque = "N"
            If ClienteSelected.FormaPago = "C" Then
                ClientesSeVisibility.VisibleTipoCheque = Visibility.Visible
            Else
                ClientesSeVisibility.VisibleTipoCheque = Visibility.Collapsed
            End If

            MyBase.CambioItem("TipoPersonaJuridica")
            MyBase.CambioItem("TipoPersonaNatural")
            ClientesSeBoolean.EditarPreclientes = True

            'NAOC20141118 - Fatca
            '**********************************************************************
            If FATCA_CAMPOSADICIONALES Then
                If ClienteSelected.CiudadanoResidenteDomicilio Then
                    ClientesSeBoolean.logTransfiereACuentasEEUU = True
                Else
                    ClientesSeBoolean.logTransfiereACuentasEEUU = False
                End If

                If ClienteSelected.TransfiereACuentasEEUU Then
                    ClientesSeBoolean.logtranfierefondos = True
                Else
                    ClientesSeBoolean.logtranfierefondos = False
                End If

                If ClienteSelected.EmpresaListadaBolsa Then
                    ClientesSeBoolean.logEmpresaListadaEnBolsa = True
                Else
                    ClientesSeBoolean.logEmpresaListadaEnBolsa = False
                End If

                If ClienteSelected.SubsidiariaDeEntidad Then
                    ClientesSeBoolean.logSubsidiariaEntidadPublica = True
                Else
                    ClientesSeBoolean.logSubsidiariaEntidadPublica = False
                End If

                If ClienteSelected.InstitucionFinanciera Then
                    ClientesSeBoolean.logInstitucionFinanciera = True
                Else
                    ClientesSeBoolean.logInstitucionFinanciera = False
                End If

                If Not IsNothing(dcProxy.ClientesPaisesFATCAs) Then
                    dcProxy.ClientesPaisesFATCAs.Clear()
                End If
                ListaClientesPaisesFATCA = Nothing

            End If
            '**********************************************************************
            'RBP20160513

            NewCliente.PoderParaFirmar = False
            NewCliente.EnvioFisico = False
            NewCliente.logSuperIntendencia = False
            NewCliente.Apt = False
            NewCliente.AdministradoBanco = False


            ConsultaDetalle()

            logPasoValidacionDigito = True
            strDigitoPermitido = String.Empty
            'MyBase.CambioItem("Editando")

            If visNavegando = "Collapsed" Then
                MyBase.CambiarFormulario_Forma_Manual()
            End If

            logEsNuevoRegistro = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro ",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub
    Public Sub validarconfiguracionfirma()
        'se encarga de validar la configuracion de las firmas para saber que campos y que funcionalidades se habilitan
        If ConfiguraFirma.Equals("SI") Then
            configuraconocimiento = True
            configuraloghir = True
            'ClientesSeBoolean.HabilitaAE = False
            ClientesSeVisibility.ConfiguraVisibleCity = Visibility.Visible
            ' eomc -- 12/122013 -- Inicio
            labelCodigoCIIU = "Código CIIU"
            labelActividadEconomica = "Actividad económica"
            ' eomc -- 12/122013 -- Fin
        End If
    End Sub

    ''' <history>
    ''' ID caso de prueba:   CP0003
    ''' Descripción:         Desarrollo BRS - Template Calificación Clientes y Contrapartes - SARiC
    ''' Responsable:         Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:               29 de Septiembre/2014
    ''' Pruebas CB:          Jorge Peña (Alcuadrado S.A.) - 29 de Septiembre/2014
    ''' </history>
    Public Overrides Sub Filtrar()
        Try
            If Not validafiltro Then 'valida que no entre al filtro cuando se le da cancelar en las busquedas
                dcProxy.Clientes.Clear()
                IsBusy = True
                ClienteAnterior = Nothing
                If FiltroVM.Length > 0 Then
                    Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                    dcProxy.Load(dcProxy.ClientesFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientes, "FiltroVM")
                Else

                    dcProxy.Load(dcProxy.ClientesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientes, "Filtrar")

                End If
            End If
            validafiltro = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro",
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub ConfirmarBuscar()
        Try
            If cb.IDComitente <> String.Empty Or cb.Nombre <> String.Empty Or cb.Filtro = 1 Or cb.Filtro = 0 Or cb.strNroDocumento <> String.Empty Or cb.TipoIdentificacion <> String.Empty Or cb.Clasificacion <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Clientes.Clear()
                ClienteAnterior = Nothing
                IsBusy = True
                Dim TextoIDComitente = System.Web.HttpUtility.UrlEncode(cb.IDComitente)
                Dim TextoNombre = System.Web.HttpUtility.UrlEncode(cb.Nombre)
                Dim TextostrNroDocumento = System.Web.HttpUtility.UrlEncode(cb.strNroDocumento)
                ' DescripcionFiltroVM = " IDComitente = " & cb.IDComitente.ToString() & " Nombre = " & cb.Nombre.ToString()
                dcProxy.Load(dcProxy.ClientesConsultarQuery(cb.Filtro, TextoIDComitente, TextoNombre, TextostrNroDocumento, cb.TipoIdentificacion, cb.Clasificacion, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientes, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaCliente(Me)
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro",
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <history>
    ''' ID caso de prueba:   CP0007, CP0008
    ''' Descripción:         Desarrollo BRS - Template Calificación Clientes y Contrapartes - SARiC
    ''' Responsable:         Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:               29 de Septiembre/2014
    ''' Pruebas CB:          Jorge Peña (Alcuadrado S.A.) - 29 de Septiembre/2014
    ''' </history>
    Public Overrides Async Sub ActualizarRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(_ListaClientesDirecciones) Then
                Dim strDireccionOficina As String = String.Empty
                Dim strDireccionResidencia As String = String.Empty
                For Each li In _ListaClientesDirecciones
                    If li.Tipo = "C" Then
                        strDireccionResidencia = li.Direccion
                    ElseIf li.Tipo = "O" Then
                        strDireccionOficina = li.Direccion
                    End If

                Next
                If ClienteSelected.TipoPersona = 1 Then
                    ClienteSelected.Direccion = strDireccionResidencia
                Else
                    ClienteSelected.Direccion = strDireccionOficina
                End If

                ClienteSelected.DireccionOficina = strDireccionOficina
            End If

            Dim logConsultoSucursalCliente As Boolean = False

            If (ClienteSelected.IDSucCliente = "" Or ClienteSelected.IDSucCliente Is Nothing) Then
                IsBusy = True
                logConsultoSucursalCliente = Await TraerSiguienteSuc(False)
                IsBusy = False
            End If

            If logConsultoSucursalCliente Then
                If Not IsNothing(ClienteSelected.IDSucCliente) Then
                    If ClienteSelected.IDSucCliente <> String.Empty Then
                        IsBusy = True
                        validarDocumento("ACTUALIZARREGISTRO")
                    Else
                        ContinuarGuardadoCliente()
                    End If
                Else
                    ContinuarGuardadoCliente()
                End If
            Else
                ContinuarGuardadoCliente()
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)
            ClientesSeBoolean.Editarcampos = True
        End Try
    End Sub

    Private Async Sub ContinuarGuardadoCliente()
        Try
            'Realiza validación del digito de verificación.
            If logValidarDigitoVerificacion And logPasoValidacionDigito = False And _ClienteSelected.TipoPersona = CONST_TIPODEPERSONA_JURIDICA And _ClienteSelected.TipoIdentificacion = CONST_TIPODEIDENTIFICACION_NIT Then
                Dim strNitValidar = _ClienteSelected.strNroDocumento.Substring(0, Len(_ClienteSelected.strNroDocumento) - 1)
                Dim digitoValidar = Right(_ClienteSelected.strNroDocumento, 1)
                A2Utilidades.Mensajes.mostrarMensaje(String.Format("El digito de verificación {0} para el NIT {1} no es valido. Permitido {2}", digitoValidar, strNitValidar, strDigitoPermitido), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If ClienteSelected.InactivaItem = False Or IsNothing(ClienteSelected.InactivaItem) Then
                If FATCA_CAMPOSADICIONALES Then
                    Dim logTieneDetallesFatca As Boolean = False

                    If _ClienteSelected.CiudadanoResidenteDomicilio = False Then
                        If Not IsNothing(_ListaClientesPaisesFATCA) Then
                            If _ListaClientesPaisesFATCA.Count > 0 Then
                                mostrarMensajePregunta("Cuando Es ciudadano, residente o tiene domicilio fiscal en otro pais esta en No, se deben de borrar los detalles de relación Fatca. ¿Desea que los detalles se borren automáticamente?",
                                                       Program.TituloSistema,
                                                       "CIUDADANORESIDENTEGUARDAR",
                                                       AddressOf TerminovalidarGenerales, False)

                                logTieneDetallesFatca = True
                            End If
                        End If
                    End If

                    If logTieneDetallesFatca = False Then
                        'validapnatural()
                        'validapjuridica()
                        If (_ClienteSelected.IDComitente = -1) Then
                            Await TraerComitente()
                        End If

                        validaciones()
                        If logvalidacion = True Then
                            logvalidacion = False
                            Exit Sub
                        End If
                    Else
                        Exit Sub
                    End If
                Else
                    'validapnatural()
                    'validapjuridica()
                    If (_ClienteSelected.IDComitente = -1) Then
                        Await TraerComitente()
                    End If

                    validaciones()
                    If logvalidacion = True Then
                        logvalidacion = False
                        Exit Sub
                    End If
                End If

            Else
                Dim estado = IIf(ClienteSelected.EstadoCliente = "A", "activación", "inactivación")
                If validainactivar(estado) Then
                    Exit Sub
                End If
            End If

            Dim origen = "update"
            ErrorForma = ""
            ClienteAnterior = ClienteSelected
            'If Not ListaClientes.Contains(ClienteSelected) Then
            If ListaClientes.Where(Function(li) li.IDClientes = ClienteSelected.IDClientes).Count = 0 Then
                origen = "insert"
                dcProxy.RejectChanges()
                ListaClientes.Add(ClienteSelected)
            End If
            If configuraloghir Then
                InsertaLogHistoriaIR()
            End If
            ClientesSeBoolean.Editarcampos = False
            ClientesSeBoolean.Editareg = False
            'ClientesSeBoolean.Editanrdcto = False
            'JFSB 20171009
            ClientesSeBoolean.Editanrdcto = True
            ClientesSeBoolean.HabilitaBus = True
            ClientesSeBoolean.logAplicafatcaRepresentante = False
            ClientesSeBoolean.logAplicafatcaCliente = False
            ClientesSeBoolean.HabilitaAE = False
            IsBusy = True
            esVersion = True
            terminarediciones()
            intClienteRelacionados = 0
            'dcProxy7.Load(dcProxy7.ConsultarClientesRelacionadosQuery(ClienteSelected.IDComitente, intClienteRelacionados, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesAutorizaciones, "")
            If dcProxy.IsLoading = False Then
                VerificarCambiosEntidadSubmit(origen)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "ContinuarGuardadoCliente", Application.Current.ToString(), Program.Maquina, ex)
            ClientesSeBoolean.Editarcampos = True
        End Try
    End Sub

    Private Sub VerificarCambiosEntidadSubmit(ByVal origen As String)
        Try
            If Not IsNothing(ListaClientesOrdenantes) Then
                For Each li In ListaClientesOrdenantes
                    If li.HasChanges Then
                        li.Usuario = Program.Usuario
                    End If
                Next
            End If

            If Not IsNothing(ListaClientesAccionistas) Then
                For Each li In ListaClientesAccionistas
                    If li.HasChanges Then
                        li.Usuario = Program.Usuario
                    End If
                Next
            End If

            If Not IsNothing(ListaClientesAficiones) Then
                For Each li In ListaClientesAficiones
                    If li.HasChanges Then
                        li.Usuario = Program.Usuario
                    End If
                Next
            End If

            If Not IsNothing(ListaClientesBeneficiarios) Then
                For Each li In ListaClientesBeneficiarios
                    If li.HasChanges Then
                        li.Usuario = Program.Usuario
                    End If
                Next
            End If

            If Not IsNothing(ListaClientesConocimientoEspecifico) Then
                For Each li In ListaClientesConocimientoEspecifico
                    If li.HasChanges Then
                        li.Usuario = Program.Usuario
                    End If
                Next
            End If

            If Not IsNothing(ListaClientesDepEconomica) Then
                For Each li In ListaClientesDepEconomica
                    If li.HasChanges Then
                        li.Usuario = Program.Usuario
                    End If
                Next
            End If

            If Not IsNothing(ListaClientesDeportes) Then
                For Each li In ListaClientesDeportes
                    If li.HasChanges Then
                        li.Usuario = Program.Usuario
                    End If
                Next
            End If

            If Not IsNothing(ListaClientesDirecciones) Then
                For Each li In ListaClientesDirecciones
                    If li.HasChanges Then
                        li.Usuario = Program.Usuario
                    End If
                Next
            End If

            If Not IsNothing(ListaClientesDocumentosRequeridos) Then
                For Each li In ListaClientesDocumentosRequeridos
                    If li.HasChanges Then
                        li.Usuario = Program.Usuario
                    End If
                Next
            End If

            If Not IsNothing(ListaClientesFicha) Then
                For Each li In ListaClientesFicha
                    If li.HasChanges Then
                        li.Usuario = Program.Usuario
                    End If
                Next
            End If

            If Not IsNothing(ListaClientesLOGHistoriaI) Then
                For Each li In ListaClientesLOGHistoriaI
                    If li.HasChanges Then
                        li.Usuario = Program.Usuario
                    End If
                Next
            End If

            If Not IsNothing(ListaClientesPaisesFATCA) Then
                For Each li In ListaClientesPaisesFATCA
                    If li.HasChanges Then
                        li.Usuario = Program.Usuario
                    End If
                Next
            End If

            If Not IsNothing(ListaClientesPersonas) Then
                For Each li In ListaClientesPersonas
                    If li.HasChanges Then
                        li.Usuario = Program.Usuario
                    End If
                Next
            End If

            If Not IsNothing(ListaClientesReceptore) Then
                For Each li In ListaClientesReceptore
                    If li.HasChanges Then
                        li.Usuario = Program.Usuario
                    End If
                Next
            End If

            If Not IsNothing(ListaClientesTipoCliente) Then
                For Each li In ListaClientesTipoCliente
                    If li.HasChanges Then
                        li.Usuario = Program.Usuario
                    End If
                Next
            End If
            'CFMA20180305  se agrega código para actualizar usuario al momento de modificar una cuenta bancaria, (auditoria)
            If Not IsNothing(ListaCuentasClientes) Then
                For Each li In ListaCuentasClientes
                    If li.HasChanges Then
                        li.Usuario = Program.Usuario
                    End If
                Next
            End If

            dcProxy7.ConsultarClientesRelacionados(ClienteSelected.IDComitente, intClienteRelacionados, Program.Usuario, Program.HashConexion, AddressOf TerminoTraerClientesrelacionados, Nothing)

            Program.VerificarCambiosProxyServidor(dcProxy)
            dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del usuario para los registros",
                                 Me.ToString(), "VerificarCambiosEntidadSubmit", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub guardarregistro()
        Try
            If logvalidacion = True Then
                logvalidacion = False
                Exit Sub
            End If
            Dim origen = "update"
            ErrorForma = ""
            ClienteAnterior = ClienteSelected

            'If Not ListaClientes.Contains(ClienteSelected) Then
            If ListaClientes.Where(Function(li) li.IDClientes = ClienteSelected.IDClientes).Count = 0 Then
                origen = "insert"
                'If (ClienteSelected.IDSucCliente = "" Or ClienteSelected.IDSucCliente Is Nothing) Then
                '    ClienteSelected.IDSucCliente = 0
                'End If
                If ACTIVAR_AUTORIZACION_INHABIL = "SI" Then
                    If (ClienteAnuladoInhabil) Then
                        ClienteSelected.Activo = False
                        ClienteSelected.EstadoCliente = "I"
                        ClienteSelected.Concepto = Date.Now
                        ClienteSelected.ClienteAutorizacion = True
                    End If
                End If

                ListaClientes.Add(ClienteSelected)
            End If
            If ACTIVAR_AUTORIZACION_INHABIL = "SI" And origen = "update" Then
                If (ClienteAnuladoInhabil) Then
                    ClienteSelected.Activo = False
                    ClienteSelected.EstadoCliente = "I"
                    ClienteSelected.ClienteAutorizacion = True
                End If
            End If
            If configuraloghir Then
                InsertaLogHistoriaIR()
            End If
            ClientesSeBoolean.Editarcampos = False
            ClientesSeBoolean.Editareg = False
            'ClientesSeBoolean.Editanrdcto = False
            ClientesSeBoolean.Editanrdcto = True
            ClientesSeBoolean.HabilitaBus = True
            ClientesSeBoolean.logAplicafatcaRepresentante = False
            ClientesSeBoolean.logAplicafatcaCliente = False
            ClientesSeBoolean.HabilitaAE = False
            IsBusy = True
            esVersion = True
            validareplicaciones()
            terminarediciones()
            If dcProxy.IsLoading = False Then
                VerificarCambiosEntidadSubmit(origen)
            End If
            strDocumRepetido = String.Empty

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "guardarregistro", Application.Current.ToString(), Program.Maquina, ex)
            ClientesSeBoolean.Editarcampos = True
        End Try

    End Sub

    ''' <history>
    ''' Descripción:    Se agrega la instrucción "(So.UserState = "insert" And HABILITAR_CLASIFICACIONES_EN_CLIENTES = Visibility.Visible)" para que recargue la lista de clientes con los campos del agrupador "Calificación final del riesgo".
    ''' Responsable:    Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:          29 de Septiembre/2014
    ''' Pruebas CB:     Jorge Peña (Alcuadrado S.A.) - 29 de Septiembre/2014
    ''' </history>
    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then
                Dim strMsg As String = String.Empty
                'TODO: Pendiente garantizar que Userstate no venga vacío
                'A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                '                       Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                Else
                    If (So.Error.Message.Contains("ErrorPersonalizado,") = True) And ((So.UserState = "insert") Or (So.UserState = "update")) Then
                        Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,")
                        Dim Mensaje = Split(Mensaje1(1), vbCr)
                        movimientosclientes(Mensaje(0))
                        'A2Utilidades.Mensajes.mostrarMensaje(ValidaMensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        So.MarkErrorAsHandled()
                        ClientesSeBoolean.Editarcampos = True
                        Exit Sub
                    ElseIf So.Error.Message.Contains("duplicate") Then
                        A2Utilidades.Mensajes.mostrarMensaje("Existe un registro duplicado en la base de datos por favor verifique las pestañas donde se agrego registros nuevos", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        So.MarkErrorAsHandled()
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje(So.Error.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        So.MarkErrorAsHandled()
                    End If
                End If
                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                So.MarkErrorAsHandled()
                If So.UserState = "BorrarRegistro" Then
                    dcProxy.RejectChanges()
                End If
                'So.MarkErrorAsHandled()
                ClientesSeBoolean.Editarcampos = True
                Exit Try
            End If

            'JABG 20160428
            If logEnviamail Then
                dcProxy.ClientesEnvioMail(ClienteSelected.IDComitente, ClienteSelected.IDNacionalidad, ListaClientesDirecciones.Where(Function(li) li.DireccionEnvio = True).First.Ciudad, "", Program.Usuario, Program.HashConexion, AddressOf TerminoTraerRespuestaMail, Nothing)
            End If

            If ACTIVAR_AUTORIZACION_INHABIL = "SI" And (So.UserState = "insert") Or (So.UserState = "update") Then
                If (ClienteAnuladoInhabil) Then
                    dcProxy5.Load(dcProxy5.ClientesAutorizacionesRegistrarQuery(ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesAutorizaciones, "")
                End If
            End If

            If PATRIMONIOAUTONOMO_ACTIVAR = "SI" Then

                If So.UserState = "insert" Then
                    If ClienteSelected.TipoProducto = "PA" Then
                        If Not IsNothing(ClienteSelected.ReplicarSafyrFondos) Then
                            If ClienteSelected.ReplicarSafyrFondos = True Then
                                A2Utilidades.Mensajes.mostrarMensaje("El cliente está configurado como patrimonio autónomo y habilitado para el sistema Finonset V3, el mismo no será replicado hasta tanto no se complete la información en el maestro de relaciones entre cuentas", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Else
                                A2Utilidades.Mensajes.mostrarMensaje("El cliente está configurado como patrimonio autónomo es necesario relacionar el código parametrizado en el maestro de relaciones entre cuentas.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            End If
                        End If
                    End If
                End If

                If So.UserState = "update" Then
                    If ClienteSelected.TipoProducto = "PA" Then
                        'DEMC20190523
                        If Not IsNothing(ClienteSelected.ReplicarSafyrFondos) Then
                            'If ClienteSelected.ReplicarSafyrFondos = True Then
                            '    If intClienteRelacionados <> 0 Then
                            '        A2Utilidades.Mensajes.mostrarMensaje("Los cambios realizados serán replicados al sistema Finonset V3", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                            '    Else
                            '        A2Utilidades.Mensajes.mostrarMensaje("El cliente está configurado como Patrimonio Autónomo, es necesario relacionar el código parametrizado en el maestro de relaciones entre cuentas.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            '    End IfJu
                            'End If

                            If ClienteSelected.ReplicarSafyrFondos = True Then

                                A2Utilidades.Mensajes.mostrarMensaje("Los cambios realizados serán replicados al sistema Finonset V3", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                            End If

                            If intClienteRelacionados = 0 Then
                                A2Utilidades.Mensajes.mostrarMensaje("El cliente está configurado como Patrimonio Autónomo, es necesario relacionar el código parametrizado en el maestro de relaciones entre cuentas.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            End If
                            'DEMC20190523
                        End If
                    End If
                End If

            End If

            If Not IsNothing(MakeAndCheck) Then
                If MakeAndCheck = 1 Or (So.UserState = "insert" And HABILITAR_CLASIFICACIONES_EN_CLIENTES = Visibility.Visible) Then
                    ClienteSelected = Nothing
                    dcProxy.Clientes.Clear()
                    dcProxy.Load(dcProxy.ClientesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientes, "insert") ' Recarga la lista para que carguen los include
                Else
                    If So.UserState = "BorrarRegistro" Or So.UserState = "update" Then
                        MyBase.QuitarFiltroDespuesGuardar()
                        ClienteSelected = Nothing
                        dcProxy.Clientes.Clear()
                        dcProxy.Load(dcProxy.ClientesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientes, "insert") ' Recarga la lista para que carguen los include
                    ElseIf So.UserState = "insert" And MOSTRARMENSAJEADICIONALCLIENTES = "SI" Then
                        A2Utilidades.Mensajes.mostrarMensaje("Recuerde diligenciar la pantalla de datos adicionales clientes", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
                    End If
                End If
            End If

            inhabilitacampos()

            If DatosListaClinton.Count > 0 Then
                For Each li In DatosListaClinton
                    mdcProxyUtilidad06.GrabarListaClinton(li.Forma, li.Comitente, li.NroDocumento,
                                           li.Nombre, li.Clienteinhabilitado, li.porcentaje,
                                      Program.Usuario, Program.HashConexion, AddressOf TerminoGrabarListaClinton, "grabar")
                Next
            End If

            If Not IsNothing(Preclientesf) Then
                If Preclientesf.logPrecliente Then
                    Preclientesf.modificarestado(False)
                End If
            End If


            ClienteAnuladoInhabil = False
            EditandoReceptores = False

            MyBase.TerminoSubmitChanges(So)

            If Editando = False And ClientesSeBoolean.Editarcampos Then
                ClientesSeBoolean.Editarcampos = False
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
            ClientesSeBoolean.Editarcampos = True
        End Try


    End Sub
    Public Sub inhabilitacampos()
        ClientesSeBoolean.Read = True
        ClientesSeBoolean.Fondos = False
        ClientesSeBoolean.Portafolios = False
        ClientesSeBoolean.Clientes = False
        ClientesSeBoolean.Mercasonft = False
        ClientesSeBoolean.HabilitaDepEconomica = False
        ClientesSeBoolean.EsOperacionExt = False
        ClientesSeBoolean.HabilitaCampoPNatural = False
        ClientesSeBoolean.HabilitaCampoPJuridica = False
        ClientesSeBoolean.EditaInactividad = False
        ClientesSeBoolean.Comitente = False
        ClientesSeBoolean.EditarPreclientes = False
        'NAOC20141118 - Fatca
        '**********************************************************************
        ClientesSeBoolean.logTransfiereACuentasEEUU = False
        ClientesSeBoolean.logtranfierefondos = False
        ClientesSeBoolean.logEmpresaListadaEnBolsa = False
        ClientesSeBoolean.logSubsidiariaEntidadPublica = False
        ClientesSeBoolean.logInstitucionFinanciera = False
        '**********************************************************************
    End Sub
    Public Sub buscaconenter(ByVal IDComitente As String, ByVal Nombre As String, ByVal Filtro As Byte, ByVal strNroDocumento As String, ByVal TipoIdentificacion As String, ByVal Clasificacion As String)
        Try
            If IDComitente <> String.Empty Or Nombre <> String.Empty Or Filtro = 1 Or Filtro = 0 Or strNroDocumento <> String.Empty Or TipoIdentificacion <> String.Empty Or Clasificacion <> String.Empty Then 'Validar que ingresó algo en los campos de búsqueda
                ErrorForma = ""
                dcProxy.Clientes.Clear()
                ClienteAnterior = Nothing
                IsBusy = True
                Dim TextoIDComitente = System.Web.HttpUtility.UrlEncode(IIf(IDComitente = "", Nothing, IDComitente))
                Dim TextoNombre = System.Web.HttpUtility.UrlEncode(IIf(Nombre = "", Nothing, Nombre))
                Dim TextostrNroDocumento = System.Web.HttpUtility.UrlEncode(IIf(strNroDocumento = "", Nothing, strNroDocumento))
                ' DescripcionFiltroVM = " IDComitente = " & cb.IDComitente.ToString() & " Nombre = " & cb.Nombre.ToString()
                dcProxy.Load(dcProxy.ClientesConsultarQuery(Filtro, TextoIDComitente, TextoNombre, TextostrNroDocumento, TipoIdentificacion, Clasificacion, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientes, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaCliente(Me)
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro",
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    ''' <summary>
    ''' Se recibe la respuesta la insercción en Lista Clinton
    ''' </summary>
    ''' <param name="lo">JBT20130208</param>
    ''' <remarks></remarks>
    Private Sub TerminoGrabarListaClinton(ByVal lo As InvokeOperation(Of Boolean))
        If lo.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al grabar el Log de la Lista Clinton",
                                               Me.ToString(), "TerminoGrabarListaClinton" & lo.UserState.ToString(), Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()
        End If
    End Sub
    Public Overrides Sub EditarRegistro()

        If dcProxy.IsLoading Then
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If
        If IsNothing(_ClienteSelected) Then
            Exit Sub
        Else
            logEsNuevoRegistro = False

            NombreAnteriorAuto = ClienteSelected.Nombre
            If Not IsNothing(ClientesAccionistasSelected) Then
                ClientesAccionistasNombre1 = ClientesAccionistasSelected.Nombre1
                ClientesAccionistasNombre2 = ClientesAccionistasSelected.Nombre2
                ClientesAccionistasApellido1 = ClientesAccionistasSelected.Apellido1
                ClientesAccionistasApellido2 = ClientesAccionistasSelected.Apellido1
            End If
            If Not IsNothing(ClientesBeneficiarioSelected) Then
                ClientesBeneficiarioNombre1 = ClientesBeneficiarioSelected.Nombre1
                ClientesBeneficiarioNombre2 = ClientesBeneficiarioSelected.Nombre2
                ClientesBeneficiarioApellido1 = ClientesBeneficiarioSelected.Apellido1
                ClientesBeneficiarioApellido2 = ClientesBeneficiarioSelected.Apellido2
            End If
            If Not IsNothing(ClientesDepEconomicaselected) Then
                ClientesDepEconomicaNombre1 = ClientesDepEconomicaselected.Nombre1
                ClientesDepEconomicaNombre2 = ClientesDepEconomicaselected.Nombre2
                ClientesDepEconomicaApellido1 = ClientesDepEconomicaselected.Apellido1
                ClientesDepEconomicaApellido2 = ClientesDepEconomicaselected.Apellido2
            End If
            If Not IsNothing(ClientesPersonaselected) Then
                ClientesPersonaNombre1 = ClientesPersonaselected.Nombre1
                ClientesPersonaNombre2 = ClientesPersonaselected.Nombre2
                ClientesPersonaApellido1 = ClientesPersonaselected.Apellido1
                ClientesPersonaApellido2 = ClientesPersonaselected.Apellido2
            End If
        End If
        If ClienteSelected.Activo = 0 Then
            MyBase.RetornarValorEdicionNavegacion()
            A2Utilidades.Mensajes.mostrarMensaje("El cliente esta inactivo no se puede modificar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If
        If MakeAndCheck = 1 Then
            ValidarEdicion()
        Else
            If Not IsNothing(_ClienteSelected) Then
                ConsultaDetalle(True)
                Editando = True
                If logPerteneceConsecutivo = True Then
                    EditandoReceptores = True
                Else
                    EditandoReceptores = False
                End If

                If ClienteSelected.DepEconomica Then
                    ClientesSeBoolean.HabilitaDepEconomica = True
                Else
                    ClientesSeBoolean.HabilitaDepEconomica = False
                End If
                ClientesSeBoolean.Editarcampos = True
                ClientesSeBoolean.Editareg = False
                'ClientesSeBoolean.Editanrdcto = False
                'JFSB 20171009
                ClientesSeBoolean.Editanrdcto = True
                ClientesSeBoolean.Read = False
                ClientesSeBoolean.HabilitaBus = False
                ClientesSeBoolean.Comitente = False

                'SV20160127
                If Not IsNothing(ClienteSelected.ReplicarSafyrFondos) AndAlso ClienteSelected.ReplicarSafyrFondos = True Then
                    ClientesSeBoolean.Fondos = True
                Else
                    ClientesSeBoolean.Fondos = False
                End If

                If Not IsNothing(ClienteSelected.ReplicarSafyrPortafolios) AndAlso ClienteSelected.ReplicarSafyrPortafolios = True Then
                    ClientesSeBoolean.Portafolios = True
                Else
                    ClientesSeBoolean.Portafolios = False
                End If

                If Not IsNothing(ClienteSelected.ReplicarSafyrClientes) AndAlso ClienteSelected.ReplicarSafyrClientes = True Then
                    ClientesSeBoolean.Clientes = True
                Else
                    ClientesSeBoolean.Clientes = False
                End If

                If Not IsNothing(ClienteSelected.ReplicarMercansoft) AndAlso ClienteSelected.ReplicarMercansoft = True Then
                    ClientesSeBoolean.Mercasonft = True
                Else
                    ClientesSeBoolean.Mercasonft = False
                End If

                If ConfiguraFirma.Equals("NO") Then
                    ClientesSeBoolean.HabilitaAE = True
                End If
                ClienteSelected.Actualizacion = Now.Date
                ClienteSelected.Usuario = Program.Usuario
                If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                    Dim Nacionalidadlista = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("Paises").Where(Function(li) li.ID = ClienteSelected.IDNacionalidad.ToString).ToList
                    If Nacionalidadlista.Count > 0 Then
                        Dim Nacionalidad = Nacionalidadlista.First
                        If DescripcionValida(Program.FatcaDescripcionPais, "|", Nacionalidad.Descripcion) And ClienteSelected.TipoPersona = 1 Then
                            ClientesSeBoolean.logAplicafatcaCliente = True
                        Else
                            ClientesSeBoolean.logAplicafatcaCliente = False
                        End If
                    End If
                End If
                If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                    Dim Nacionalidadlista = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("Paises").Where(Function(li) li.ID = ClienteSelected.IDNacionalidadReprLegal.ToString).ToList
                    If Nacionalidadlista.Count > 0 Then
                        Dim Nacionalidad = Nacionalidadlista.First
                        If DescripcionValida(Program.FatcaDescripcionPais, "|", Nacionalidad.Descripcion) And ClienteSelected.TipoPersona = 2 Then
                            ClientesSeBoolean.logAplicafatcaRepresentante = True
                        Else
                            ClientesSeBoolean.logAplicafatcaRepresentante = False
                        End If
                    End If
                End If
                If ClienteSelected.TipoPersona = 2 Then
                    ClientesSeBoolean.HabilitaCampoPNatural = False
                    ClientesSeBoolean.HabilitaCampoPJuridica = True
                Else
                    ClientesSeBoolean.HabilitaCampoPNatural = True
                    ClientesSeBoolean.HabilitaCampoPJuridica = False
                End If
                If ClienteSelected.OpMonedaExtranjera Then
                    ClientesSeBoolean.EsOperacionExt = True
                End If
                'NAOC20141118 - Fatca
                '**********************************************************************
                If FATCA_CAMPOSADICIONALES Then
                    If IsNothing(ClienteSelected.CiudadanoResidenteDomicilio) Then
                        ClienteSelected.CiudadanoResidenteDomicilio = False
                    End If
                    If IsNothing(ClienteSelected.TransfiereACuentasEEUU) Then
                        ClienteSelected.TransfiereACuentasEEUU = False
                    End If
                    If IsNothing(ClienteSelected.TitularCuentaEEUU) Then
                        ClienteSelected.TitularCuentaEEUU = String.Empty
                    End If
                    If IsNothing(ClienteSelected.EntidadTransferencia) Then
                        ClienteSelected.EntidadTransferencia = String.Empty
                    End If
                    If IsNothing(ClienteSelected.EmpresaListadaBolsa) Then
                        ClienteSelected.EmpresaListadaBolsa = False
                    End If
                    If IsNothing(ClienteSelected.SubsidiariaDeEntidad) Then
                        ClienteSelected.SubsidiariaDeEntidad = False
                    End If
                    If IsNothing(ClienteSelected.AccionistaContribuyenteEEUU) Then
                        ClienteSelected.AccionistaContribuyenteEEUU = False
                    End If
                    If IsNothing(ClienteSelected.SinAnimoDeLucroFatca) Then
                        ClienteSelected.SinAnimoDeLucroFatca = False
                    End If
                    If IsNothing(ClienteSelected.InstitucionFinanciera) Then
                        ClienteSelected.InstitucionFinanciera = False
                    End If
                    If IsNothing(ClienteSelected.Regulador) Then
                        ClienteSelected.Regulador = String.Empty
                    End If
                    If IsNothing(ClienteSelected.MercadoNegociaAcciones) Then
                        ClienteSelected.MercadoNegociaAcciones = String.Empty
                    End If
                    If IsNothing(ClienteSelected.EmpresaMatriz) Then
                        ClienteSelected.EmpresaMatriz = String.Empty
                    End If
                    If IsNothing(ClienteSelected.GIIN) Then
                        ClienteSelected.GIIN = String.Empty
                    End If

                    If ClienteSelected.CiudadanoResidenteDomicilio Then
                        ClientesSeBoolean.logTransfiereACuentasEEUU = True
                    Else
                        ClientesSeBoolean.logTransfiereACuentasEEUU = False
                    End If

                    If ClienteSelected.TransfiereACuentasEEUU Then
                        ClientesSeBoolean.logtranfierefondos = True
                    Else
                        ClientesSeBoolean.logtranfierefondos = False
                    End If

                    If ClienteSelected.EmpresaListadaBolsa Then
                        ClientesSeBoolean.logEmpresaListadaEnBolsa = True
                    Else
                        ClientesSeBoolean.logEmpresaListadaEnBolsa = False
                    End If

                    If ClienteSelected.SubsidiariaDeEntidad Then
                        ClientesSeBoolean.logSubsidiariaEntidadPublica = True
                    Else
                        ClientesSeBoolean.logSubsidiariaEntidadPublica = False
                    End If

                    If ClienteSelected.InstitucionFinanciera Then
                        ClientesSeBoolean.logInstitucionFinanciera = True
                    Else
                        ClientesSeBoolean.logInstitucionFinanciera = False
                    End If
                End If
                '**********************************************************************



                'If ClienteSelected.MenorEdad Then
                dcProxy.ConsultaDocumMenor(ClienteSelected.TipoIdentificacion, Convert.ToInt32(ClienteSelected.MenorEdad), Program.Usuario, Program.HashConexion, AddressOf TerminotraerparametroDocumMenor, Nothing)
                'End If


                logPasoValidacionDigito = True
                strDigitoPermitido = String.Empty

                If Not IsNothing(ClienteSelected.AdmonInvExterior) AndAlso ClienteSelected.AdmonInvExterior = True Then
                    ClienteSelected.AdmonInvExterior = True
                Else
                    ClienteSelected.AdmonInvExterior = False
                End If
                If Not IsNothing(ClienteSelected.AdministradoBanco) AndAlso ClienteSelected.AdministradoBanco = True Then
                    ClienteSelected.AdministradoBanco = True
                Else
                    ClienteSelected.AdministradoBanco = False
                End If
            End If
        End If
    End Sub
    Public Sub Editar()
        If Not IsNothing(_ClienteSelected) Then
            ConsultaDetalle(True)
            Editando = True
            If logPerteneceConsecutivo = True Then
                EditandoReceptores = True
            Else
                EditandoReceptores = False
            End If

            ClientesSeBoolean.HabilitaDepEconomica = True
            ClientesSeBoolean.Editarcampos = True
            ClientesSeBoolean.Editareg = False
            'ClientesSeBoolean.Editanrdcto = False
            'JFSB 20171009
            ClientesSeBoolean.Editanrdcto = True
            ClientesSeBoolean.Read = False
            ClientesSeBoolean.HabilitaBus = False
            ClientesSeBoolean.Comitente = False

            'SV20160127
            If Not IsNothing(ClienteSelected.ReplicarSafyrFondos) AndAlso ClienteSelected.ReplicarSafyrFondos = True Then
                ClientesSeBoolean.Fondos = True
            Else
                ClientesSeBoolean.Fondos = False
            End If

            If Not IsNothing(ClienteSelected.ReplicarSafyrPortafolios) AndAlso ClienteSelected.ReplicarSafyrPortafolios = True Then
                ClientesSeBoolean.Portafolios = True
            Else
                ClientesSeBoolean.Portafolios = False
            End If

            If Not IsNothing(ClienteSelected.ReplicarSafyrClientes) AndAlso ClienteSelected.ReplicarSafyrClientes = True Then
                ClientesSeBoolean.Clientes = True
            Else
                ClientesSeBoolean.Clientes = False
            End If

            If Not IsNothing(ClienteSelected.ReplicarMercansoft) AndAlso ClienteSelected.ReplicarMercansoft = True Then
                ClientesSeBoolean.Mercasonft = True
            Else
                ClientesSeBoolean.Mercasonft = False
            End If

            If ConfiguraFirma.Equals("NO") Then
                ClientesSeBoolean.HabilitaAE = True
            End If
            ClienteSelected.Actualizacion = Now.Date
            ClienteSelected.Usuario = Program.Usuario
            If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                Dim Nacionalidadlista = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("Paises").Where(Function(li) li.ID = ClienteSelected.IDNacionalidad.ToString).ToList
                If Nacionalidadlista.Count > 0 Then
                    Dim Nacionalidad = Nacionalidadlista.First
                    If DescripcionValida(Program.FatcaDescripcionPais, "|", Nacionalidad.Descripcion) And ClienteSelected.TipoPersona = 1 Then
                        ClientesSeBoolean.logAplicafatcaCliente = True
                    Else
                        ClientesSeBoolean.logAplicafatcaCliente = False
                    End If
                End If
            End If
            If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                Dim Nacionalidadlista = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("Paises").Where(Function(li) li.ID = ClienteSelected.IDNacionalidadReprLegal.ToString).ToList
                If Nacionalidadlista.Count > 0 Then
                    Dim Nacionalidad = Nacionalidadlista.First
                    If DescripcionValida(Program.FatcaDescripcionPais, "|", Nacionalidad.Descripcion) And ClienteSelected.TipoPersona = 2 Then
                        ClientesSeBoolean.logAplicafatcaRepresentante = True
                    Else
                        ClientesSeBoolean.logAplicafatcaRepresentante = False
                    End If
                End If
            End If
            If ClienteSelected.TipoPersona = 2 Then
                ClientesSeBoolean.HabilitaCampoPNatural = False
                ClientesSeBoolean.HabilitaCampoPJuridica = True
            Else
                ClientesSeBoolean.HabilitaCampoPNatural = True
                ClientesSeBoolean.HabilitaCampoPJuridica = False
            End If
            If ClienteSelected.OpMonedaExtranjera Then
                ClientesSeBoolean.EsOperacionExt = True
            End If
            'NAOC20141118 - Fatca
            '**********************************************************************
            If FATCA_CAMPOSADICIONALES Then
                If IsNothing(ClienteSelected.CiudadanoResidenteDomicilio) Then
                    ClienteSelected.CiudadanoResidenteDomicilio = False
                End If
                If IsNothing(ClienteSelected.TransfiereACuentasEEUU) Then
                    ClienteSelected.TransfiereACuentasEEUU = False
                End If
                If IsNothing(ClienteSelected.TitularCuentaEEUU) Then
                    ClienteSelected.TitularCuentaEEUU = String.Empty
                End If
                If IsNothing(ClienteSelected.EntidadTransferencia) Then
                    ClienteSelected.EntidadTransferencia = String.Empty
                End If
                If IsNothing(ClienteSelected.EmpresaListadaBolsa) Then
                    ClienteSelected.EmpresaListadaBolsa = False
                End If
                If IsNothing(ClienteSelected.SubsidiariaDeEntidad) Then
                    ClienteSelected.SubsidiariaDeEntidad = False
                End If
                If IsNothing(ClienteSelected.AccionistaContribuyenteEEUU) Then
                    ClienteSelected.AccionistaContribuyenteEEUU = False
                End If
                If IsNothing(ClienteSelected.SinAnimoDeLucroFatca) Then
                    ClienteSelected.SinAnimoDeLucroFatca = False
                End If
                If IsNothing(ClienteSelected.InstitucionFinanciera) Then
                    ClienteSelected.InstitucionFinanciera = False
                End If
                If IsNothing(ClienteSelected.Regulador) Then
                    ClienteSelected.Regulador = String.Empty
                End If
                If IsNothing(ClienteSelected.MercadoNegociaAcciones) Then
                    ClienteSelected.MercadoNegociaAcciones = String.Empty
                End If
                If IsNothing(ClienteSelected.EmpresaMatriz) Then
                    ClienteSelected.EmpresaMatriz = String.Empty
                End If
                If IsNothing(ClienteSelected.GIIN) Then
                    ClienteSelected.GIIN = String.Empty
                End If

                If ClienteSelected.CiudadanoResidenteDomicilio Then
                    ClientesSeBoolean.logTransfiereACuentasEEUU = True
                Else
                    ClientesSeBoolean.logTransfiereACuentasEEUU = False
                End If

                If ClienteSelected.TransfiereACuentasEEUU Then
                    ClientesSeBoolean.logtranfierefondos = True
                Else
                    ClientesSeBoolean.logtranfierefondos = False
                End If

                If ClienteSelected.EmpresaListadaBolsa Then
                    ClientesSeBoolean.logEmpresaListadaEnBolsa = True
                Else
                    ClientesSeBoolean.logEmpresaListadaEnBolsa = False
                End If

                If ClienteSelected.SubsidiariaDeEntidad Then
                    ClientesSeBoolean.logSubsidiariaEntidadPublica = True
                Else
                    ClientesSeBoolean.logSubsidiariaEntidadPublica = False
                End If

                If ClienteSelected.InstitucionFinanciera Then
                    ClientesSeBoolean.logInstitucionFinanciera = True
                Else
                    ClientesSeBoolean.logInstitucionFinanciera = False
                End If
            End If
            '**********************************************************************
            'If ClienteSelected.MenorEdad Then
            dcProxy.ConsultaDocumMenor(ClienteSelected.TipoIdentificacion, Convert.ToInt32(ClienteSelected.MenorEdad), Program.Usuario, Program.HashConexion, AddressOf TerminotraerparametroDocumMenor, Nothing)
            'End If
            'final = Now
            'MessageBox.Show(desde.ToString("dd,mm,yyyy hh:mm:ss.fff") + "   " + hasta.ToString("dd,mm,yyyy hh:mm:ss.fff") + "  " + (hasta - desde).Milliseconds.ToString + "   " + final.ToString("dd,mm,yyyy hh:mm:ss.fff") + "  " + (final - hasta).Milliseconds.ToString + " " + (final - desde).Milliseconds.ToString)
        End If

    End Sub
    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            logEsNuevoRegistro = False
            EditandoReceptores = False
            CiudadesClientes.strPoblaciondoc = strciudaddoc
            CiudadesClientes.strdepartamentoDoc = strdepartamentodoc
            CiudadesClientes.strPaisDoc = strpaisdoc
            CamposSeGenerales.strcodigociu = strcodigociiu
            CiudadesClientes.strciudad = strcodigociirepre
            CiudadesClientes.strciudadNacimiento = strCiudadNacimiento
            CamposSeGenerales.strProfesion = strProfesion
            Editando = False
            ClientesSeBoolean.HabilitaDepEconomica = False
            ClientesSeBoolean.Editarcampos = False
            ClientesSeBoolean.Editareg = False
            'ClientesSeBoolean.Editanrdcto = False
            'JFSB 20171009
            ClientesSeBoolean.Editanrdcto = True
            ClientesSeBoolean.Read = True
            ClientesSeBoolean.HabilitaBus = True
            ClientesSeBoolean.Comitente = False
            ClientesSeBoolean.EditarPreclientes = False
            ClientesSeBoolean.EditaInactividad = False
            ClientesSeBoolean.EsOperacionExt = False
            ClientesSeBoolean.HabilitaCampoPNatural = False
            ClientesSeBoolean.HabilitaCampoPJuridica = False
            ClientesSeBoolean.logAplicafatcaRepresentante = False
            ClientesSeBoolean.logAplicafatcaCliente = False
            ClientesSeBoolean.HabilitaAE = False

            'SV20160127
            ClientesSeBoolean.Fondos = False
            ClientesSeBoolean.Portafolios = False
            ClientesSeBoolean.Clientes = False
            ClientesSeBoolean.Mercasonft = False

            'NAOC20141118 - Fatca
            '**********************************************************************
            ClientesSeBoolean.logTransfiereACuentasEEUU = False
            ClientesSeBoolean.logtranfierefondos = False
            ClientesSeBoolean.logEmpresaListadaEnBolsa = False
            ClientesSeBoolean.logSubsidiariaEntidadPublica = False
            ClientesSeBoolean.logInstitucionFinanciera = False
            '**********************************************************************
            logPasoValidacionDigito = True
            strDigitoPermitido = String.Empty
            Preclientesf = Nothing
            strDocumRepetido = String.Empty

            If Not IsNothing(ClienteAnterior) Then
                If ClienteAnterior.TipoPersona = 2 Then
                    If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                        Dim strTipoIdentificacionEdicion As String = TipoIdentificacionEdicion
                        objTipoId = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOIDC2")
                        TipoIdentificacionEdicion = strTipoIdentificacionEdicion
                    End If
                Else
                    If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                        Dim strTipoIdentificacionEdicion As String = TipoIdentificacionEdicion
                        objTipoId = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOIDC1")
                        TipoIdentificacionEdicion = strTipoIdentificacionEdicion
                    End If
                End If
            End If

            dcProxy.RejectChanges()
            If _ClienteSelected.EntityState = EntityState.Detached Then
                ClienteSelected = ClienteAnterior
            End If
            If Not IsNothing(_ClienteSelected) Then
                If _ClienteSelected.Por_Aprobar Is Nothing Then
                    dcProxy.CuentasClientes.Clear()
                    dcProxy.Load(dcProxy.Traer_CuentasClientesQuery(0, _ClienteSelected.IDComitente, False, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasclientes, Nothing)
                Else
                    dcProxy.CuentasClientes.Clear()
                    dcProxy.Load(dcProxy.Traer_CuentasClientesQuery(1, _ClienteSelected.IDComitente, False, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasclientes, Nothing)
                End If
            End If

            dcProxy.Load(dcProxy.ClientesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientes, "insert")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro",
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <history>
    ''' ID caso de prueba:   CP0009
    ''' Descripción:         Desarrollo BRS - Template Calificación Clientes y Contrapartes - SARiC
    ''' Responsable:         Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:               29 de Septiembre/2014
    ''' Pruebas CB:          Jorge Peña (Alcuadrado S.A.) - 29 de Septiembre/2014
    ''' </history>
    Public Overrides Sub BorrarRegistro()
        If dcProxy.IsLoading Then
            A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If
        If IsNothing(_ClienteSelected) Then
            Exit Sub
        End If
        If ClienteSelected.ClienteAutorizacion = True Then
            A2Utilidades.Mensajes.mostrarMensaje("El cliente debe de ser activado o rechazado por el sistema de autorizaciones.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If
        If MakeAndCheck = 1 Then

            Validarborrado()
        Else

            If ClienteSelected.Activo = True Then
                Try
                    If ClienteSelected.EstadoCliente = "A" Then
                        'C1.Silverlight.C1MessageBox.Show("Esta seguro de inactivar este Cliente", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf Terminoinactivo)
                        mostrarMensajePregunta("¿Esta seguro de inactivar este Cliente?",
                                               Program.TituloSistema,
                                               "INACTIVARCLIENTE",
                                               AddressOf Terminoinactivo, False)
                    ElseIf ClienteSelected.EstadoCliente = "B" Then
                        mostrarMensajePregunta("¿Esta seguro de activar este Cliente?",
                                       Program.TituloSistema,
                                       "ACTIVARCLIENTE",
                                       AddressOf Terminoactivo, False)
                    End If
                Catch ex As Exception
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inactivar un registro",
                     Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
                End Try
            Else
                Try
                    'plogActivacion = True
                    'Consulta si el cliente es ordenante los clientes activos de este 
                    mostrarMensajePregunta("¿Esta seguro de activar este Cliente?",
                                       Program.TituloSistema,
                                       "ACTIVARCLIENTE",
                                       AddressOf Terminoactivo, False)
                Catch ex As Exception
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al activar un registro",
                     Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
                End Try
            End If
        End If
    End Sub
    Public Sub borrar()
        'If ClienteSelected.Activo = True Then
        '    'C1.Silverlight.C1MessageBox.Show("Esta seguro de inactivar este Cliente", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf Terminoinactivo)
        '    mostrarMensajePregunta("¿Esta seguro de inactivar este Cliente?", _
        '                           Program.TituloSistema, _
        '                           "INACTIVARCLIENTE", _
        '                           AddressOf Terminoinactivo, False)
        'Else
        '    Try
        '        TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
        '        TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_GENERAL
        '        ClienteSelected.InactivaItem = True
        '        ClienteSelected.Concepto = Nothing
        '        ClienteSelected.IDConcepto = Nothing
        '        Fechadesplegada = Date.Now
        '        Editando = True
        '        ClientesSeBoolean.Editarcampos = False
        '        ClientesSeBoolean.EditaInactividad = True
        '    Catch ex As Exception
        '        IsBusy = False
        '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
        '         Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        '    End Try
        'End If
        If ClienteSelected.Activo = True Then
            Try
                If ClienteSelected.EstadoCliente = "A" Then
                    'C1.Silverlight.C1MessageBox.Show("Esta seguro de inactivar este Cliente", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf Terminoinactivo)
                    mostrarMensajePregunta("¿Esta seguro de inactivar este Cliente?",
                                           Program.TituloSistema,
                                           "INACTIVARCLIENTE",
                                           AddressOf Terminoinactivo, False)
                ElseIf ClienteSelected.EstadoCliente = "B" Then
                    mostrarMensajePregunta("¿Esta seguro de activar este Cliente?",
                                   Program.TituloSistema,
                                   "ACTIVARCLIENTE",
                                   AddressOf Terminoactivo, False)
                End If
            Catch ex As Exception
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al inactivar un registro",
                 Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
            End Try
        Else
            Try
                'plogActivacion = True
                'Consulta si el cliente es ordenante los clientes activos de este 
                mostrarMensajePregunta("¿Esta seguro de activar este Cliente?",
                                   Program.TituloSistema,
                                   "ACTIVARCLIENTE",
                                   AddressOf Terminoactivo, False)
            Catch ex As Exception
                IsBusy = False
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al activar un registro",
                 Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
            End Try
        End If
    End Sub

    ''' <history>
    ''' ID caso de prueba:   CP0002
    ''' Descripción:         Desarrollo BRS - Template Calificación Clientes y Contrapartes - SARiC
    ''' Responsable:         Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:               29 de Septiembre/2014
    ''' Pruebas CB:          Jorge Peña (Alcuadrado S.A.) - 29 de Septiembre/2014
    ''' </history>
    Public Overrides Sub CambiarAForma()
        If MakeAndCheck = 1 Then
            If Not IsNothing(ClienteSelected.Estado) Then
                If ClienteSelected.Estado.Equals("Ingreso") Then
                    ClientesSeBoolean.visible = False
                Else
                    ClientesSeBoolean.visible = True
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

            Dim origen = "update"
            ErrorForma = ""
            ClienteAnterior = ClienteSelected
            If (ClienteSelected.Estado.Equals("Modificacion") Or ClienteSelected.Estado.Equals("Ingreso")) Then
                ClienteSelected.Aprobacion = 2
                ClienteSelected.UsuarioAprobador = Program.Usuario
                'CLIENTESRECEPTORES
                ClientesReceptoreSelected = _ListaClientesReceptore.FirstOrDefault
                If Not IsNothing(ClientesReceptoreSelected) Then
                    If Not IsNothing(ClientesReceptoreSelected.Estado) Then
                        If (ClientesReceptoreSelected.Estado.Equals("Modificacion") Or ClientesReceptoreSelected.Estado.Equals("Ingreso") Or ClientesReceptoreSelected.Estado.Equals("Retiro")) Then
                            For Each li In ListaClientesReceptore
                                'ClientesReceptoreSelected = li
                                'ClientesReceptoreSelected.Aprobacion = 2
                                li.Aprobacion = 2
                                'li.Usuario = Program.Usuario
                            Next
                        End If
                    End If
                End If
                'CUENTASCLIENTES
                CuentasClientesSelected = _ListaCuentasClientes.FirstOrDefault
                If Not IsNothing(CuentasClientesSelected) Then
                    If Not IsNothing(CuentasClientesSelected.Estado) Then
                        If (CuentasClientesSelected.Estado.Equals("Modificacion") Or CuentasClientesSelected.Estado.Equals("Ingreso") Or CuentasClientesSelected.Estado.Equals("Retiro")) Then
                            For Each li In ListaCuentasClientes
                                'CuentasClientesSelected = li
                                'CuentasClientesSelected.Aprobacion = 2
                                li.Aprobacion = 2
                                'li.Usuario = Program.Usuario
                            Next
                        End If
                    End If
                End If
                'CLIENTESORDENANTES
                ClientesOrdenantesSelected = _ListaClientesOrdenantes.FirstOrDefault
                If Not IsNothing(ClientesOrdenantesSelected) Then
                    If Not IsNothing(ClientesOrdenantesSelected.Estado) Then
                        If (ClientesOrdenantesSelected.Estado.Equals("Modificacion") Or ClientesOrdenantesSelected.Estado.Equals("Ingreso") Or ClientesOrdenantesSelected.Estado.Equals("Retiro")) Then
                            For Each li In ListaClientesOrdenantes
                                'ClientesOrdenantesSelected = li
                                'ClientesOrdenantesSelected.Aprobacion = 2
                                li.Aprobacion = 2
                                'li.Usuario = Program.Usuario
                            Next
                        End If
                    End If
                End If
                'CLIENTESBENEFICIARIOS
                ClientesBeneficiarioSelected = _ListaClientesBeneficiarios.FirstOrDefault
                If Not IsNothing(ClientesBeneficiarioSelected) Then
                    If Not IsNothing(ClientesBeneficiarioSelected.Estado) Then
                        If (ClientesBeneficiarioSelected.Estado.Equals("Modificacion") Or ClientesBeneficiarioSelected.Estado.Equals("Ingreso") Or ClientesBeneficiarioSelected.Estado.Equals("Retiro")) Then
                            For Each li In ListaClientesBeneficiarios
                                'ClientesBeneficiarioSelected = li
                                'ClientesBeneficiarioSelected.Aprobacion = 2
                                li.Aprobacion = 2
                                'li.Usuario = Program.Usuario
                            Next
                        End If
                    End If
                End If
                'CLIENTESTIPOCLIENTE
                ClientesTipoClienteSelected = _ListaClientesTipoCliente.FirstOrDefault
                If Not IsNothing(ClientesTipoClienteSelected) Then
                    If Not IsNothing(ClientesTipoClienteSelected.Estado) Then
                        If (ClientesTipoClienteSelected.Estado.Equals("Modificacion") Or ClientesTipoClienteSelected.Estado.Equals("Ingreso") Or ClientesTipoClienteSelected.Estado.Equals("Retiro")) Then
                            For Each li In ListaClientesTipoCliente
                                'ClientesTipoClienteSelected = li
                                'ClientesTipoClienteSelected.Aprobacion = 2
                                li.Aprobacion = 2
                                'li.Usuario = Program.Usuario
                            Next
                        End If
                    End If
                End If

                'CLIENTESDIRECCIONES
                ClientesDireccionesSelected = _ListaClientesDirecciones.FirstOrDefault
                If Not IsNothing(ClientesDireccionesSelected) Then
                    If Not IsNothing(ClientesDireccionesSelected.Estado) Then
                        If (ClientesDireccionesSelected.Estado.Equals("Modificacion") Or ClientesDireccionesSelected.Estado.Equals("Ingreso") Or ClientesDireccionesSelected.Estado.Equals("Retiro")) Then
                            For Each li In ListaClientesDirecciones
                                'ClientesDireccionesSelected = li
                                'ClientesDireccionesSelected.Aprobacion = 2
                                li.Aprobacion = 2
                                'li.Usuario = Program.Usuario
                            Next
                        End If
                    End If
                End If
                'CLIENTESAFICIONES
                ClientesAficionesSelected = _ListaClientesAficiones.FirstOrDefault
                If Not IsNothing(ClientesAficionesSelected) Then
                    If Not IsNothing(ClientesAficionesSelected.Estado) Then
                        If (ClientesAficionesSelected.Estado.Equals("Modificacion") Or ClientesAficionesSelected.Estado.Equals("Ingreso") Or ClientesAficionesSelected.Estado.Equals("Retiro")) Then
                            For Each li In ListaClientesAficiones
                                'ClientesDireccionesSelected = li
                                'ClientesDireccionesSelected.Aprobacion = 2
                                li.Aprobacion = 2
                                'li.Usuario = Program.Usuario
                            Next
                        End If
                    End If
                End If
                'CLIENTESDEPORTES
                ClientesDeportesSelected = _ListaClientesDeportes.FirstOrDefault
                If Not IsNothing(ClientesDeportesSelected) Then
                    If Not IsNothing(ClientesDeportesSelected.Estado) Then
                        If (ClientesDeportesSelected.Estado.Equals("Modificacion") Or ClientesDeportesSelected.Estado.Equals("Ingreso") Or ClientesDeportesSelected.Estado.Equals("Retiro")) Then
                            For Each li In ListaClientesDeportes
                                'ClientesDireccionesSelected = li
                                'ClientesDireccionesSelected.Aprobacion = 2
                                li.Aprobacion = 2
                                'li.Usuario = Program.Usuario
                            Next
                        End If
                    End If
                End If
                'CLIENTESFICHA
                ClientesFichaSelected = _ListaClientesFicha.FirstOrDefault
                If Not IsNothing(ClientesFichaSelected) Then
                    If Not IsNothing(ClientesFichaSelected.Estado) Then
                        If (ClientesFichaSelected.Estado.Equals("Modificacion") Or ClientesFichaSelected.Estado.Equals("Ingreso") Or ClientesFichaSelected.Estado.Equals("Retiro")) Then
                            For Each li In ListaClientesFicha
                                li.Aprobacion = 2
                            Next
                        End If
                    End If
                End If
                'CLIENTESDEPECONOMICA
                ClientesDepEconomicaselected = _ListaClientesDepEconomica.FirstOrDefault
                If Not IsNothing(ClientesDepEconomicaselected) Then
                    If Not IsNothing(ClientesDepEconomicaselected.Estado) Then
                        If (ClientesDepEconomicaselected.Estado.Equals("Modificacion") Or ClientesDepEconomicaselected.Estado.Equals("Ingreso") Or ClientesDepEconomicaselected.Estado.Equals("Retiro")) Then
                            For Each li In ListaClientesDepEconomica
                                li.Aprobacion = 2
                            Next
                        End If
                    End If
                End If
                'CONOCIMIENTOESPECIFICO
                If configuraconocimiento Then
                    If Not IsNothing(_ListaClientesConocimientoEspecifico) Then
                        ClientesConocimientoEspecificoSelected = _ListaClientesConocimientoEspecifico.FirstOrDefault
                    End If
                    If Not IsNothing(ClientesConocimientoEspecificoSelected) Then
                        If Not IsNothing(ClientesConocimientoEspecificoSelected.Estado) Then
                            If (ClientesConocimientoEspecificoSelected.Estado.Equals("Modificacion") Or ClientesConocimientoEspecificoSelected.Estado.Equals("Ingreso") Or ClientesConocimientoEspecificoSelected.Estado.Equals("Retiro")) Then
                                For Each li In ListaClientesConocimientoEspecifico
                                    'ClientesConocimientoEspecificoSelected = li
                                    If li.IDClientesConocimiento <> 0 Then
                                        'ClientesConocimientoEspecificoSelected.Aprobacion = 2
                                        li.Aprobacion = 2
                                        'li.Usuario = Program.Usuario
                                    End If
                                Next
                            End If
                        End If
                    End If
                End If
                'NAOC20141118 - Fatca
                '**********************************************************************
                If FATCA_CAMPOSADICIONALES Then
                    If Not IsNothing(_ListaClientesPaisesFATCA) Then
                        ClientesPaisesFATCAselected = _ListaClientesPaisesFATCA.FirstOrDefault
                        If Not IsNothing(ClientesPaisesFATCAselected) Then
                            If Not IsNothing(ClientesPaisesFATCAselected.Estado) Then
                                If (ClientesPaisesFATCAselected.Estado.Equals("Modificacion") Or ClientesPaisesFATCAselected.Estado.Equals("Ingreso") Or ClientesPaisesFATCAselected.Estado.Equals("Retiro")) Then
                                    For Each li In ListaClientesPaisesFATCA
                                        li.Aprobacion = 2
                                    Next
                                End If
                            End If
                        End If
                    End If
                End If
                '**********************************************************************
            ElseIf (ClienteSelected.Estado.Equals("Retiro")) Then
                origen = "BorrarRegistro"
                ClienteSelected.Aprobacion = 2
                ClienteSelected.UsuarioAprobador = Program.Usuario
            End If
            ClientesSeBoolean.Editarcampos = False
            ClientesSeBoolean.logAplicafatcaRepresentante = False
            ClientesSeBoolean.logAplicafatcaCliente = False
            ClientesSeBoolean.HabilitaAE = False
            IsBusy = True
            If dcProxy.IsLoading = False Then
                VerificarCambiosEntidadSubmit(origen)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "AprobarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        MyBase.AprobarRegistro()
    End Sub
    Public Overrides Sub RechazarRegistro()
        Try
            esVersion = True

            Dim origen = "update"
            ErrorForma = ""
            ClienteAnterior = ClienteSelected
            If (ClienteSelected.Estado.Equals("Modificacion") Or ClienteSelected.Estado.Equals("Ingreso")) Then
                ClienteSelected.Aprobacion = 1
                ClienteSelected.UsuarioAprobador = Program.Usuario
                'CLIENTESRECEPTORES
                ClientesReceptoreSelected = _ListaClientesReceptore.FirstOrDefault
                If Not IsNothing(ClientesReceptoreSelected) Then
                    If Not IsNothing(ClientesReceptoreSelected.Estado) Then
                        If (ClientesReceptoreSelected.Estado.Equals("Modificacion") Or ClientesReceptoreSelected.Estado.Equals("Ingreso") Or ClientesReceptoreSelected.Estado.Equals("Retiro")) Then
                            For Each li In ListaClientesReceptore
                                'ClientesReceptoreSelected = li
                                'ClientesReceptoreSelected.Aprobacion = 1
                                li.Aprobacion = 1
                                'li.Usuario = Program.Usuario
                            Next
                        End If
                    End If
                End If
                'CUENTASCLIENTES
                CuentasClientesSelected = _ListaCuentasClientes.FirstOrDefault
                If Not IsNothing(CuentasClientesSelected) Then
                    If Not IsNothing(CuentasClientesSelected.Estado) Then
                        If (CuentasClientesSelected.Estado.Equals("Modificacion") Or CuentasClientesSelected.Estado.Equals("Ingreso") Or CuentasClientesSelected.Estado.Equals("Retiro")) Then
                            For Each li In ListaCuentasClientes
                                'CuentasClientesSelected = li
                                'CuentasClientesSelected.Aprobacion = 1
                                li.Aprobacion = 1
                                'li.Usuario = Program.Usuario
                            Next
                        End If
                    End If
                End If
                'CLIENTESORDENANTES
                ClientesOrdenantesSelected = _ListaClientesOrdenantes.FirstOrDefault
                If Not IsNothing(ClientesOrdenantesSelected) Then
                    If Not IsNothing(ClientesOrdenantesSelected.Estado) Then
                        If (ClientesOrdenantesSelected.Estado.Equals("Modificacion") Or ClientesOrdenantesSelected.Estado.Equals("Ingreso") Or ClientesOrdenantesSelected.Estado.Equals("Retiro")) Then
                            For Each li In ListaClientesOrdenantes
                                'ClientesOrdenantesSelected = li
                                'ClientesOrdenantesSelected.Aprobacion = 1
                                li.Aprobacion = 1
                                'li.Usuario = Program.Usuario
                            Next
                        End If
                    End If
                End If
                'CLIENTESBENEFICIARIOS
                ClientesBeneficiarioSelected = _ListaClientesBeneficiarios.FirstOrDefault
                If Not IsNothing(ClientesBeneficiarioSelected) Then
                    If Not IsNothing(ClientesBeneficiarioSelected.Estado) Then
                        If (ClientesBeneficiarioSelected.Estado.Equals("Modificacion") Or ClientesBeneficiarioSelected.Estado.Equals("Ingreso") Or ClientesBeneficiarioSelected.Estado.Equals("Retiro")) Then
                            For Each li In ListaClientesBeneficiarios
                                'ClientesBeneficiarioSelected = li
                                'ClientesBeneficiarioSelected.Aprobacion = 1
                                li.Aprobacion = 1
                                'li.Usuario = Program.Usuario
                            Next
                        End If
                    End If
                End If
                'CLIENTESTIPOCLIENTE
                ClientesTipoClienteSelected = _ListaClientesTipoCliente.FirstOrDefault
                If Not IsNothing(ClientesTipoClienteSelected) Then
                    If Not IsNothing(ClientesTipoClienteSelected.Estado) Then
                        If (ClientesTipoClienteSelected.Estado.Equals("Modificacion") Or ClientesTipoClienteSelected.Estado.Equals("Ingreso") Or ClientesTipoClienteSelected.Estado.Equals("Retiro")) Then
                            For Each li In ListaClientesTipoCliente
                                'ClientesTipoClienteSelected = li
                                'ClientesTipoClienteSelected.Aprobacion = 1
                                li.Aprobacion = 1
                                'li.Usuario = Program.Usuario
                            Next
                        End If
                    End If
                End If
                'CLIENTESDIRECCIONES
                ClientesDireccionesSelected = _ListaClientesDirecciones.FirstOrDefault
                If Not IsNothing(ClientesDireccionesSelected) Then
                    If Not IsNothing(ClientesDireccionesSelected.Estado) Then
                        If (ClientesDireccionesSelected.Estado.Equals("Modificacion") Or ClientesDireccionesSelected.Estado.Equals("Ingreso") Or ClientesDireccionesSelected.Estado.Equals("Retiro")) Then
                            For Each li In ListaClientesDirecciones
                                'ClientesDireccionesSelected = li
                                'ClientesDireccionesSelected.Aprobacion = 1
                                li.Aprobacion = 1
                                'li.Usuario = Program.Usuario
                            Next
                        End If
                    End If
                End If
                'CLIENTESAFICIONES
                ClientesAficionesSelected = _ListaClientesAficiones.FirstOrDefault
                If Not IsNothing(ClientesAficionesSelected) Then
                    If Not IsNothing(ClientesAficionesSelected.Estado) Then
                        If (ClientesAficionesSelected.Estado.Equals("Modificacion") Or ClientesAficionesSelected.Estado.Equals("Ingreso") Or ClientesAficionesSelected.Estado.Equals("Retiro")) Then
                            For Each li In ListaClientesAficiones
                                'ClientesDireccionesSelected = li
                                'ClientesDireccionesSelected.Aprobacion = 2
                                li.Aprobacion = 1
                                'li.Usuario = Program.Usuario
                            Next
                        End If
                    End If
                End If
                'CLIENTESDEPORTES
                ClientesDeportesSelected = _ListaClientesDeportes.FirstOrDefault
                If Not IsNothing(ClientesDeportesSelected) Then
                    If Not IsNothing(ClientesDeportesSelected.Estado) Then
                        If (ClientesDeportesSelected.Estado.Equals("Modificacion") Or ClientesDeportesSelected.Estado.Equals("Ingreso") Or ClientesDeportesSelected.Estado.Equals("Retiro")) Then
                            For Each li In ListaClientesDeportes
                                'ClientesDireccionesSelected = li
                                'ClientesDireccionesSelected.Aprobacion = 2
                                li.Aprobacion = 1
                                'li.Usuario = Program.Usuario
                            Next
                        End If
                    End If
                End If
                'CLIENTESFICHA
                ClientesFichaSelected = _ListaClientesFicha.FirstOrDefault
                If Not IsNothing(ClientesFichaSelected) Then
                    If Not IsNothing(ClientesFichaSelected.Estado) Then
                        If (ClientesFichaSelected.Estado.Equals("Modificacion") Or ClientesFichaSelected.Estado.Equals("Ingreso") Or ClientesFichaSelected.Estado.Equals("Retiro")) Then
                            For Each li In ListaClientesFicha
                                li.Aprobacion = 1
                            Next
                        End If
                    End If
                End If
                'CLIENTESDEPECONOMICA
                ClientesDepEconomicaselected = _ListaClientesDepEconomica.FirstOrDefault
                If Not IsNothing(ClientesDepEconomicaselected) Then
                    If Not IsNothing(ClientesDepEconomicaselected.Estado) Then
                        If (ClientesDepEconomicaselected.Estado.Equals("Modificacion") Or ClientesDepEconomicaselected.Estado.Equals("Ingreso") Or ClientesDepEconomicaselected.Estado.Equals("Retiro")) Then
                            For Each li In ListaClientesDepEconomica
                                li.Aprobacion = 1
                            Next
                        End If
                    End If
                End If
                'CONOCIMIENTOESPECIFICO
                If configuraconocimiento Then
                    ClientesConocimientoEspecificoSelected = _ListaClientesConocimientoEspecifico.FirstOrDefault
                    If Not IsNothing(ClientesConocimientoEspecificoSelected) Then
                        If Not IsNothing(ClientesConocimientoEspecificoSelected.Estado) Then
                            If (ClientesConocimientoEspecificoSelected.Estado.Equals("Modificacion") Or ClientesConocimientoEspecificoSelected.Estado.Equals("Ingreso") Or ClientesConocimientoEspecificoSelected.Estado.Equals("Retiro")) Then
                                For Each li In ListaClientesConocimientoEspecifico
                                    'ClientesConocimientoEspecificoSelected = li
                                    If li.IDClientesConocimiento <> 0 Then
                                        'ClientesConocimientoEspecificoSelected.Aprobacion = 1
                                        li.Aprobacion = 1
                                        'li.Usuario = Program.Usuario
                                    End If
                                Next
                            End If
                        End If
                    End If
                End If
                'NAOC20141118 - Fatca
                '**********************************************************************
                If FATCA_CAMPOSADICIONALES Then
                    If Not IsNothing(_ListaClientesPaisesFATCA) Then
                        ClientesPaisesFATCAselected = _ListaClientesPaisesFATCA.FirstOrDefault
                        If Not IsNothing(ClientesPaisesFATCAselected) Then
                            If Not IsNothing(ClientesPaisesFATCAselected.Estado) Then
                                If (ClientesPaisesFATCAselected.Estado.Equals("Modificacion") Or ClientesPaisesFATCAselected.Estado.Equals("Ingreso") Or ClientesPaisesFATCAselected.Estado.Equals("Retiro")) Then
                                    For Each li In ListaClientesPaisesFATCA
                                        li.Aprobacion = 1
                                    Next
                                End If
                            End If
                        End If
                    End If
                End If
                '**********************************************************************

            ElseIf (ClienteSelected.Estado.Equals("Retiro")) Then
                origen = "BorrarRegistro"
                ClienteSelected.Aprobacion = 1
                ClienteSelected.UsuarioAprobador = Program.Usuario
            End If
            ClientesSeBoolean.Editarcampos = False
            ClientesSeBoolean.logAplicafatcaRepresentante = False
            ClientesSeBoolean.logAplicafatcaCliente = False
            ClientesSeBoolean.HabilitaAE = False
            IsBusy = True
            If dcProxy.IsLoading = False Then
                VerificarCambiosEntidadSubmit(origen)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro",
                                 Me.ToString(), "RechazarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        MyBase.RechazarRegistro()
    End Sub
    Public Overrides Sub VersionRegistro()
        Try
            esVersion = True
            esversionportafolio = True
            codigo = ClienteSelected.IDComitente
            If ClienteSelected.Por_Aprobar Is Nothing Then
                'dcProxy.Clientes.Clear()
                IsBusy = True
                logfiltroMaker = False
                dcProxy.Load(dcProxy.ClientesConsultarVersionQuery(0, ClienteSelected.IDComitente, ClienteSelected.Nombre, ClienteSelected.strNroDocumento, ClienteSelected.TipoIdentificacion, ClienteSelected.Clasificacion, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientes, Nothing)
            Else
                'dcProxy.Clientes.Clear()
                IsBusy = True
                logfiltroMaker = True
                dcProxy.Load(dcProxy.ClientesConsultarVersionQuery(1, ClienteSelected.IDComitente, ClienteSelected.Nombre, ClienteSelected.strNroDocumento, ClienteSelected.TipoIdentificacion, ClienteSelected.Clasificacion, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientes, Nothing)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro",
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        MyBase.VersionRegistro()
    End Sub

    ''' <history>
    ''' ID caso de prueba:   CP0004
    ''' Descripción:         Desarrollo BRS - Template Calificación Clientes y Contrapartes - SARiC
    ''' Responsable:         Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:               29 de Septiembre/2014
    ''' Pruebas CB:          Jorge Peña (Alcuadrado S.A.) - 29 de Septiembre/2014
    ''' </history>
    Public Overrides Sub Buscar()

        If dcProxy.IsLoading Then

            Dim objView As New ClientesView

            A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema no ha terminado de cargar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub

        End If

        If MakeAndCheck = 1 Then
            ClientesSeVisibility.visibilidad = Visibility.Visible
        End If
        cb.Filtro = 1
        If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
            objTipoIdBuscar = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOID")
        End If
        MyBase.Buscar()
    End Sub
    Private Sub _ClientesSelected_PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) Handles _ClienteSelected.PropertyChanged
        Select Case e.PropertyName
            Case "IDSector"
                If Not IsNothing(listasubSector) Then
                    listasubSector.Clear()
                End If
                listasubSector = listaclasificacion.Where(Function(di) di.EsSector = False And di.IDPerteneceA = IIf(IsNothing(ClienteSelected.IDSector), 0, ClienteSelected.IDSector) And di.AplicaA.Equals("C")).ToList
                MyBase.CambioItem("listasubSector")
                ClienteSelected.IDSubSector = Nothing
            Case "IDGrupo"
                If Not IsNothing(listasubGrupo) Then
                    listasubGrupo.Clear()
                End If
                listasubGrupo = listaclasificacion.Where(Function(di) di.EsGrupo = False And di.IDPerteneceA = IIf(IsNothing(ClienteSelected.IDGrupo), 0, ClienteSelected.IDGrupo) And di.AplicaA.Equals("C")).ToList
                MyBase.CambioItem("listasubGrupo")
                If Not ListaClientes.Contains(ClienteSelected) Then
                    ClienteSelected.IDSubGrupo = mintSubGrupoPorDefecto
                Else
                    ClienteSelected.IDSubGrupo = Nothing
                End If

            Case "TipoPersona"
                If Not IsNothing(ClienteSelected.TipoPersona) Then
                    If ClienteSelected.TipoPersona = 2 Then
                        ClientesSeBoolean.HabilitaCampoPNatural = False
                        ClientesSeBoolean.HabilitaCampoPJuridica = True
                        ClienteSelected.Nombre = ""
                        ClientesSeVisibility.TipoPersonaJuridica = Visibility.Visible
                        ClientesSeVisibility.TipoPersonaNatural = Visibility.Collapsed
                        If Not ConfiguraFirma.Equals("SI") Then
                            'Confirguracion para ue city no vea el tab de accionistas por el momento 
                            ClientesSeVisibility.ConfiguravisibleAccionistas = Visibility.Visible
                        End If
                        MyBase.CambioItem("TipoPersonaJuridica")
                        MyBase.CambioItem("TipoPersonaNatural")

                        If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                            TipoIdentificacionEdicion = String.Empty
                            objTipoId = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOIDC2")

                        End If
                        dcProxy.ClientesDocumentosRequeridos.Clear()
                        dcProxy.Load(dcProxy.Traer_ClientesDocumentosRequeridosQuery(ClienteSelected.IDComitente, ClienteSelected.TipoPersona, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDocumentosRequeridos, Nothing)
                    Else
                        ClientesSeBoolean.HabilitaCampoPNatural = True
                        ClientesSeBoolean.HabilitaCampoPJuridica = False
                        If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                            TipoIdentificacionEdicion = String.Empty
                            objTipoId = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOIDC1")
                        End If
                        ClienteSelected.Apellido1 = ""
                        ClienteSelected.Apellido2 = ""
                        ClienteSelected.Nombre1 = ""
                        ClienteSelected.Nombre2 = ""
                        ClientesSeVisibility.TipoPersonaNatural = Visibility.Visible
                        ClientesSeVisibility.TipoPersonaJuridica = Visibility.Collapsed
                        ClientesSeVisibility.ConfiguravisibleAccionistas = Visibility.Collapsed
                        MyBase.CambioItem("TipoPersonaJuridica")
                        MyBase.CambioItem("TipoPersonaNatural")
                        dcProxy.ClientesDocumentosRequeridos.Clear()
                        dcProxy.Load(dcProxy.Traer_ClientesDocumentosRequeridosQuery(ClienteSelected.IDComitente, ClienteSelected.TipoPersona, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDocumentosRequeridos, Nothing)
                    End If

                    'NAOC20141118 - Fatca
                    '**********************************************************************
                    ValidarHabilitarFatca()
                    '**********************************************************************
                End If
            Case "TipoReferencia"
                If Not IsNothing(ClienteSelected.TipoReferencia) Then
                    If ClienteSelected.TipoReferencia.Equals("O") Then
                        ClientesSeVisibility.ReferenciaOtros = Visibility.Visible
                        ClientesSeVisibility.ReferenciaPor = Visibility.Collapsed
                        MyBase.CambioItem("ReferenciaOtros")
                        MyBase.CambioItem("ReferenciaPor")
                    ElseIf ClienteSelected.TipoReferencia.Equals("R") Then
                        ClientesSeVisibility.ReferenciaOtros = Visibility.Collapsed
                        ClientesSeVisibility.ReferenciaPor = Visibility.Visible
                        MyBase.CambioItem("ReferenciaOtros")
                        MyBase.CambioItem("ReferenciaPor")
                    Else
                        ClientesSeVisibility.ReferenciaOtros = Visibility.Collapsed
                        ClientesSeVisibility.ReferenciaPor = Visibility.Collapsed
                        MyBase.CambioItem("ReferenciaOtros")
                        MyBase.CambioItem("ReferenciaPor")
                    End If
                End If
            Case "OpMonedaExtranjera"
                If Not IsNothing(ClienteSelected.OpMonedaExtranjera) Then
                    If ClienteSelected.OpMonedaExtranjera = False Then
                        ClientesSeBoolean.EsOperacionExt = False
                    Else
                        ClientesSeBoolean.EsOperacionExt = True
                    End If
                End If
            Case "Apellido1"
                ClienteSelected.Nombre = ClienteSelected.Apellido1 + " " + ClienteSelected.Apellido2 + " " + ClienteSelected.Nombre1 + " " + ClienteSelected.Nombre2
            Case "Apellido2"
                ClienteSelected.Nombre = ClienteSelected.Apellido1 + " " + ClienteSelected.Apellido2 + " " + ClienteSelected.Nombre1 + " " + ClienteSelected.Nombre2
            Case "Nombre1"
                ClienteSelected.Nombre = ClienteSelected.Apellido1 + " " + ClienteSelected.Apellido2 + " " + ClienteSelected.Nombre1 + " " + ClienteSelected.Nombre2
            Case "Nombre2"
                ClienteSelected.Nombre = ClienteSelected.Apellido1 + " " + ClienteSelected.Apellido2 + " " + ClienteSelected.Nombre1 + " " + ClienteSelected.Nombre2
            Case "Nacimiento"
                fecha = Now.Date
                Dim p = DateDiff("yyyy", ClienteSelected.Nacimiento, fecha)
                If p < 18 Then
                    If ClienteSelected.TipoPersona = 1 Then
                        ClienteSelected.MenorEdad = True
                        dcProxy.ConsultaDocumMenor(ClienteSelected.TipoIdentificacion, Convert.ToInt32(ClienteSelected.MenorEdad), Program.Usuario, Program.HashConexion, AddressOf TerminotraerparametroDocumMenor, Nothing)

                    End If
                    If Not IsNothing(ClienteSelected.TipoVinculacion) Then
                        If ClienteSelected.TipoVinculacion.Equals("A") Or ClienteSelected.TipoVinculacion.Equals("O") Then
                            If ClienteSelected.MenorEdad = True Then
                                A2Utilidades.Mensajes.mostrarMensaje("Un ordenante no puede ser menor de edad.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                'ClienteSelected.MenorEdad = False
                                intDocummenor = 0
                            End If
                        End If
                    End If
                Else
                    ClienteSelected.MenorEdad = False
                    'intDocummenor = 0
                    dcProxy.ConsultaDocumMenor(ClienteSelected.TipoIdentificacion, Convert.ToInt32(ClienteSelected.MenorEdad), Program.Usuario, Program.HashConexion, AddressOf TerminotraerparametroDocumMenor, Nothing)
                End If
                'Case "IDTipoEntidad"
                '    ClientesTipoClienteSelected.IDTipoEntidad = ClienteSelected.IDTipoEntidad
            Case "TipoVinculacion"
                If ClienteSelected.TipoVinculacion.Equals("A") Or ClienteSelected.TipoVinculacion.Equals("O") Then
                    If ClienteSelected.MenorEdad = True Then
                        A2Utilidades.Mensajes.mostrarMensaje("Un ordenante no puede ser menor de edad.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        ClienteSelected.MenorEdad = False
                    End If
                    If Not ClienteSelected.IDComitente.Equals("-1") Then
                        For Each Lista In ListaClientesOrdenantes
                            Dim idordenante = ClienteSelected.IDComitente
                            Dim ordenante = ListaClientesOrdenantes.Where(Function(li) IIf(IsNothing(li.IDOrdenante), String.Empty, li.IDOrdenante).Equals(idordenante)).ToList

                            If ordenante.Count = 1 Then
                                Exit Sub
                            End If
                        Next
                        Dim NewClientesOrdenantesSelected As New OyDClientes.ClientesOrdenante
                        NewClientesOrdenantesSelected.IDComitente = ClienteSelected.IDComitente
                        NewClientesOrdenantesSelected.IDOrdenante = ClienteSelected.IDComitente
                        NewClientesOrdenantesSelected.Nombre = ClienteSelected.Nombre
                        NewClientesOrdenantesSelected.Usuario = Program.Usuario
                        NewClientesOrdenantesSelected.Asociados = 0
                        NewClientesOrdenantesSelected.lider = False
                        NewClientesOrdenantesSelected.Relacionado = False
                        ListaClientesOrdenantes.Add(NewClientesOrdenantesSelected)
                        ClientesOrdenantesSelected = NewClientesOrdenantesSelected
                    End If
                Else
                    CType(dcProxy.ClientesOrdenantes, IRevertibleChangeTracking).RejectChanges()
                End If
            Case "strNroDocumento"
                If Not IsNothing(ClienteSelected.strNroDocumento) Then
                    If ClienteSelected.strNroDocumento.Length <= 15 Then
                        If ClienteSelected.strNroDocumento <> String.Empty And ClienteSelected.strNroDocumento <> "0" Then
                            validarDocumento()
                            If Versioned.IsNumeric(ClienteSelected.strNroDocumento) Then
                                ClienteSelected.NroDocumento = CType(ClienteSelected.strNroDocumento, Decimal)
                            Else
                                ClienteSelected.NroDocumento = 0
                            End If
                        End If
                    End If
                End If
            Case "ReplicarSafyrFondos"
                If Not IsNothing(ClienteSelected.ReplicarSafyrFondos) Then
                    If ClienteSelected.ReplicarSafyrFondos = False Then
                        ClientesSeBoolean.Fondos = False
                        ClienteSelected.CodReceptorSafyrFondos = Nothing
                    Else
                        ClientesSeBoolean.Fondos = True
                    End If
                End If
            Case "ReplicarSafyrPortafolios"
                If Not IsNothing(ClienteSelected.ReplicarSafyrPortafolios) Then
                    If ClienteSelected.ReplicarSafyrPortafolios = False Then
                        ClientesSeBoolean.Portafolios = False
                        ClienteSelected.CodReceptorSafyrPortafolio = Nothing
                    Else
                        ClientesSeBoolean.Portafolios = True
                    End If
                End If
            Case "ReplicarSafyrClientes"
                If Not IsNothing(ClienteSelected.ReplicarSafyrClientes) Then
                    If ClienteSelected.ReplicarSafyrClientes = False Then
                        ClientesSeBoolean.Clientes = False
                        ClienteSelected.CodReceptorSafyrClientes = Nothing
                    Else
                        ClientesSeBoolean.Clientes = True
                    End If
                End If
            Case "ReplicarMercansoft"
                If Not IsNothing(ClienteSelected.ReplicarMercansoft) Then
                    If ClienteSelected.ReplicarMercansoft = False Then
                        ClientesSeBoolean.Mercasonft = False
                        ClienteSelected.CodReceptorMercansoft = Nothing
                    Else
                        ClientesSeBoolean.Mercasonft = True
                    End If
                End If

            Case "Clasificacion"
                If Not IsNothing(ClienteSelected.Clasificacion) Then
                    ClienteSelected.CategoriaCliente = ClienteSelected.Clasificacion
                End If
            Case "TipoIdentificacion"
                If ClienteSelected.strNroDocumento <> String.Empty And Not IsNothing(ClienteSelected.strNroDocumento) Then
                    validarDocumento()
                End If
                dcProxy.ConsultaDocumMenor(ClienteSelected.TipoIdentificacion, Convert.ToInt32(ClienteSelected.MenorEdad), Program.Usuario, Program.HashConexion, AddressOf TerminotraerparametroDocumMenor, Nothing)
            Case "Excluido"
                If ClienteSelected.Excluido Then
                    ClienteSelected.Activos = Nothing
                    ClienteSelected.IngresoMensual = Nothing
                    ClienteSelected.Patrimonio = Nothing
                    ClienteSelected.Pasivos = Nothing
                    ClienteSelected.Utilidades = Nothing
                    ClienteSelected.Egresos = Nothing
                End If
            Case "DepEconomica"
                If ClienteSelected.DepEconomica Then
                    ClientesSeBoolean.HabilitaDepEconomica = True
                Else
                    ClientesSeBoolean.HabilitaDepEconomica = False
                    If Not IsNothing(ListaClientesDepEconomica) Then
                        If ListaClientesDepEconomica.Count > 0 Then
                            Dim a As OyDClientes.ClientesPersonasDepEconomica
                            While ListaClientesDepEconomica.Any
                                a = ListaClientesDepEconomica.FirstOrDefault
                                ListaClientesDepEconomica.Remove(a)
                            End While
                        End If
                    End If
                End If
            Case "IDSucCliente"
                If logValidarDocumentosIDSucursal Then
                    If Not IsNothing(ClienteSelected.IDSucCliente) Then
                        If ClienteSelected.IDSucCliente <> String.Empty Then
                            validarDocumento()
                        End If
                    End If
                End If
            Case "IDComitente"
                If InstalacioSelected.ClientesCedula Then
                    If Not IsNothing(ClienteSelected.IDComitente) Then
                        If Not ClienteSelected.IDComitente.Trim = ClienteSelected.strNroDocumento Then
                            ClienteSelected.strNroDocumento = ClienteSelected.IDComitente
                        End If
                    Else
                        ClienteSelected.strNroDocumento = ClienteSelected.IDComitente
                    End If
                End If
            Case "IDNacionalidad"
                If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                    Dim Nacionalidadlista = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("Paises").Where(Function(li) li.ID = ClienteSelected.IDNacionalidad.ToString).ToList
                    If Nacionalidadlista.Count > 0 Then
                        Dim Nacionalidad = Nacionalidadlista.First
                        If DescripcionValida(Program.FatcaDescripcionPais, "|", Nacionalidad.Descripcion) And ClienteSelected.TipoPersona = 1 Then
                            ClientesSeBoolean.logAplicafatcaCliente = True
                        Else
                            ClientesSeBoolean.logAplicafatcaCliente = False
                            ClienteSelected.TipoCiudadano = String.Empty
                            ClienteSelected.AplicaFatca = False
                        End If
                    End If
                End If
            Case "IDNacionalidadReprLegal"
                If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                    Dim Nacionalidadlista = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("Paises").Where(Function(li) li.ID = ClienteSelected.IDNacionalidadReprLegal.ToString).ToList
                    If Nacionalidadlista.Count > 0 Then
                        Dim Nacionalidad = Nacionalidadlista.First
                        If DescripcionValida(Program.FatcaDescripcionPais, "|", Nacionalidad.Descripcion) And ClienteSelected.TipoPersona = 2 Then
                            ClientesSeBoolean.logAplicafatcaRepresentante = True
                        Else
                            ClientesSeBoolean.logAplicafatcaRepresentante = False
                            ClienteSelected.TipoCiudadanoReprLegal = String.Empty
                            ClienteSelected.AplicaFatcaReprLegal = False
                        End If
                    End If
                End If
            Case "CiudadanoResidenteDomicilio" 'NAOC20141118 - Fatca
                If _ClienteSelected.CiudadanoResidenteDomicilio Then
                    ClientesSeBoolean.logTransfiereACuentasEEUU = True
                Else
                    ClientesSeBoolean.logTransfiereACuentasEEUU = False

                    If Not IsNothing(_ListaClientesPaisesFATCA) Then
                        If _ListaClientesPaisesFATCA.Count > 0 Then
                            mostrarMensajePregunta("Al marcar la opción en No, se deben de borrar los detalles de relación Fatca. ¿Desea que los detalles se borren automáticamente?",
                                                   Program.TituloSistema,
                                                   "CIUDADANORESIDENTE",
                                                   AddressOf TerminovalidarGenerales, False)
                        End If
                    End If
                End If
            Case "TransfiereACuentasEEUU" 'NAOC20141118 - Fatca
                If _ClienteSelected.TransfiereACuentasEEUU Then
                    ClientesSeBoolean.logtranfierefondos = True
                Else
                    ClientesSeBoolean.logtranfierefondos = False
                End If
            Case "EmpresaListadaBolsa" 'NAOC20141118 - Fatca
                If ClienteSelected.EmpresaListadaBolsa Then
                    ClientesSeBoolean.logEmpresaListadaEnBolsa = True
                Else
                    ClientesSeBoolean.logEmpresaListadaEnBolsa = False
                End If
            Case "SubsidiariaDeEntidad" 'NAOC20141118 - Fatca
                If ClienteSelected.SubsidiariaDeEntidad Then
                    ClientesSeBoolean.logSubsidiariaEntidadPublica = True
                Else
                    ClientesSeBoolean.logSubsidiariaEntidadPublica = False
                End If
            Case "InstitucionFinanciera" 'NAOC20141118 - Fatca
                If ClienteSelected.InstitucionFinanciera Then
                    ClientesSeBoolean.logInstitucionFinanciera = True
                Else
                    ClientesSeBoolean.logInstitucionFinanciera = False
                End If
            Case "TipoIntermediario"
                _ClienteSelected.codEmpresaExtranjera = Nothing
                '_ClienteSelected.IDNacionalidadReprLegal.Value.ToString.FirstOrDefault()
                _ClienteSelected.AdmonInvExterior = False

                'Case "Activos" 'NAOC20141118 - Fatca
                '    If logvalorparametro Then
                '        If ClienteSelected.Activos > 0 And ClienteSelected.Pasivos > 0 Then
                '                ClienteSelected.Pasivos = ClienteSelected.Activos - ClienteSelected.Patrimonio
                '            End If
                '        End If

                'Case "Pasivos" 'NAOC20141118 - Fatca
                '    If logvalorparametro Then
                '        If ClienteSelected.Activos > 0 And ClienteSelected.Pasivos > 0 Then
                '            ClienteSelected.Patrimonio = ClienteSelected.Activos - ClienteSelected.Pasivos
                '        End If
                '    End If
                'Case "Patrimonio" 'NAOC20141118 - Fatca
                '    If logvalorparametro Then
                '        If ClienteSelected.Activos > 0 And ClienteSelected.Pasivos > 0 Then
                '            ClienteSelected.Pasivos = ClienteSelected.Activos - ClienteSelected.Patrimonio
                '        End If
                'End If

                'Case "ValidationErrors"
                '        Dim obj = ClienteSelected.ValidationErrors
                'listades.Add(New listprueba With {.descripcion = ClienteSelected.ValidationErrors.FirstOrDefault.ErrorMessage})
                'MyBase.CambioItem("listades")
                'TabSeleccionadaFinanciero = 2
            Case "FormaPago"
                _ClienteSelected.TipoCheque = "N"
                If _ClienteSelected.FormaPago = "C" Then
                    ClientesSeVisibility.VisibleTipoCheque = Visibility.Visible
                Else
                    ClientesSeVisibility.VisibleTipoCheque = Visibility.Collapsed
                End If

        End Select

    End Sub
    Public Function validainactivar(ByVal estado As String) As Boolean
        If ClienteSelected.IDConcepto = 0 Or IsNothing(ClienteSelected.IDConcepto) Then
            A2Utilidades.Mensajes.mostrarMensaje("El motivo de " + estado + " es requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return True
            Exit Function
        End If
        If IsNothing(ClienteSelected.Concepto) Then
            A2Utilidades.Mensajes.mostrarMensaje("La fecha de  " + estado + "  es requerida", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return True
            Exit Function
        End If
        If ClienteSelected.TipoVinculacion <> "C" And intClientesActivos > 0 Then
            A2Utilidades.Mensajes.mostrarMensaje("El cliente a inactivar es ordenante de cuentas activas", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return True
            Exit Function
        End If
        Return False
    End Function

    ''' <history>
    ''' Descripción:    Se agregan las validaciones para las propiedades strTipoClienteClasificacion, strPerteneceGrupoEconomico, strCalificacionRiesgoSuperior y PerfilRiesgo siempre y cuando esté activa la pestaña Clasificaciones.
    ''' Responsable:    Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:          29 de Septiembre/2014
    ''' Pruebas CB:     Jorge Peña (Alcuadrado S.A.) - 29 de Septiembre/2014
    ''' </history>
    Public Sub validaciones()
        Try
            ListaRespuestaValidacionesCientes.Clear()
            logEnviamail = False
            logValidacionOperaProductos = False
            If logvalidacion Then
                Exit Sub
            End If

            If Not IsNothing(ClienteSelected.TipoPersona) Then
                ' Validaciones para una persona natural
                ' JBT20130213
                If ClienteSelected.TipoPersona = 1 Then
                    If ClienteSelected.Apellido1 = String.Empty Or ClienteSelected.Nombre1 = String.Empty Then
                        A2Utilidades.Mensajes.mostrarMensaje("El primer apellido y el primer nombre son requeridos para personas naturales.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logvalidacion = True
                        Exit Sub
                    End If
                End If
                ' Validaciones para una persona juridica
                ' JBT20130213
                If ClienteSelected.TipoPersona = 2 Then
                    If Not (ClienteSelected.TipoVinculacion.Equals("C")) Then
                        A2Utilidades.Mensajes.mostrarMensaje("Una persona jurídica solo puede ser comitente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logvalidacion = True
                        Exit Sub
                    End If
                End If
            End If
            'validaciones encabezado
            If ClienteSelected.Actualizacion.ToShortDateString <= fechaCierre Then
                A2Utilidades.Mensajes.mostrarMensaje("La fecha no puede ser menor o igual a la fecha de cierre (" & fechaCierre.ToShortDateString() & ").", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logvalidacion = True
                Exit Sub
            End If

            If (ClienteSelected.IDComitente.Equals("-1") Or ClienteSelected.IDComitente.Equals("0") Or ClienteSelected.IDComitente = String.Empty) And (InstalacioSelected.ClientesAutomatico = 0) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar un número de comitente valido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logvalidacion = True
                Exit Sub
            End If
            If String.IsNullOrEmpty(ClienteSelected.Nombre) Then
                A2Utilidades.Mensajes.mostrarMensaje("El Nombre del cliente es un campo requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logvalidacion = True
                Exit Sub
            End If
            If String.IsNullOrEmpty(ClienteSelected.TipoIdentificacion) Then
                A2Utilidades.Mensajes.mostrarMensaje("El tipo de identificación del cliente es requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logvalidacion = True
                Exit Sub
            End If
            If IsNothing(ClienteSelected.IDPoblacionDoc) Or ClienteSelected.IDPoblacionDoc = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("La ciudad del documento es requerida", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logvalidacion = True
                Exit Sub
            End If


            If InstalacioSelected.ClientesAgrupados Then
                If String.IsNullOrEmpty(ClienteSelected.IDSucCliente) Then
                    ClienteSelected.IDSucCliente = ""
                End If
            Else
                If Not InstalacioSelected.ClientesCedula Then
                    If ClienteSelected.TipoPersona = 2 Then
                        If String.IsNullOrEmpty(ClienteSelected.IDSucCliente) Then
                            A2Utilidades.Mensajes.mostrarMensaje("La sucursal del cliente es un campo requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logvalidacion = True
                            Exit Sub
                        End If
                    End If
                End If



                If Not IsNothing(ClienteSelected.IDSucCliente) Then
                    If ClienteSelected.IDSucCliente <> String.Empty Then
                        validarDocumento()
                    End If
                End If

            End If
            If strDocumRepetido <> String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje(strDocumRepetido, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logvalidacion = True
                'strDocumRepetido = String.Empty
                Exit Sub
            End If
            'Dim respuesta = HtmlPage.Window.Confirm("Respuesta")
            'If respuesta Then
            '    MessageBox.Show("dd")
            'End If
            'HtmlPage.Window.Prompt("whatis name of site?")
            If IsNothing(ClienteSelected.FechaExpedicionDoc) Then
                A2Utilidades.Mensajes.mostrarMensaje("La fecha de expedición del documento es un dato requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_GENERAL
                logvalidacion = True
                Exit Sub
            End If
            If ClienteSelected.FechaExpedicionDoc = Now.Date Then
                'Dim o = MessageBox.Show("La fecha de expedición del documento es la fecha actual,¿Desea continuar?.", Program.TituloSistema, MessageBoxButton.OKCancel)
                'If o = MessageBoxResult.Cancel Then
                '    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                '    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_GENERAL
                '    logvalidacion = True
                '    Exit Sub
                'End If
                ListaRespuestaValidacionesCientes.Add(New OyDClientes.tblRespuestaValidacionesCientes With {.ID = -2, .Mensaje = "La fecha de expedición del documento es la fecha actual.",
                              .RequiereConfirmacion = True, .DetieneIngreso = False, .Confirmacion = "¿Desea continuar?", .Tab = TAB_PRINCIPAL_GENERAL_GENERAL})
                logvalidacion = True
            End If
            If IsNothing(ClienteSelected.IDPaisDoc) Or ClienteSelected.IDPaisDoc = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("El pais del documento es un dato requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_GENERAL
                logvalidacion = True
                Exit Sub
            End If
            If IsNothing(ClienteSelected.IDPoblacionDoc) Or ClienteSelected.IDPoblacionDoc = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("La ciudad del documento es un dato requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_GENERAL
                logvalidacion = True
                Exit Sub
            End If
            If IsNothing(ClienteSelected.IDDepartamentoDoc) Or ClienteSelected.IDDepartamentoDoc = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("El departamento del documento es un dato requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_GENERAL
                logvalidacion = True
                Exit Sub
            End If

            If String.IsNullOrEmpty(ClienteSelected.strNroDocumento) Or ClienteSelected.strNroDocumento = "0" Then
                A2Utilidades.Mensajes.mostrarMensaje("El número de documento del cliente es un campo requerido .", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logvalidacion = True
                Exit Sub
            Else 'valida que el nro de documento no tenga caracteres especiales
                Dim objValidacion = clsExpresiones.ValidarCaracteresEnCadena(ClienteSelected.strNroDocumento, clsExpresiones.TipoExpresion.LetrasNumerosUnicamente)
                If Not IsNothing(objValidacion) Then
                    If objValidacion.TextoValido = False Then
                        A2Utilidades.Mensajes.mostrarMensaje("El número de documento solo puede contener números y letras.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logvalidacion = True
                        Exit Sub
                    End If
                End If
            End If
            If ClienteSelected.TipoVinculacion.Equals("A") Or ClienteSelected.TipoVinculacion.Equals("O") Then
                If ClienteSelected.TipoPersona = 2 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Un ordenante no puede ser persona jurídica .", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logvalidacion = True
                    Exit Sub
                End If
            End If
            'validaciones tab general
            If IsNothing(ClienteSelected.IDSector) Or ClienteSelected.IDSector = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el sector del cliente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_GENERAL
                logvalidacion = True
                Exit Sub
            End If
            If IsNothing(ClienteSelected.IDSubSector) Or ClienteSelected.IDSubSector = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el subsector del cliente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_GENERAL
                logvalidacion = True
                Exit Sub
            End If
            If IsNothing(ClienteSelected.IDGrupo) Or ClienteSelected.IDGrupo = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el grupo del cliente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_GENERAL
                logvalidacion = True
                Exit Sub
            End If
            If IsNothing(ClienteSelected.IDSubGrupo) Or ClienteSelected.IDSubGrupo = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el subgrupo del cliente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_GENERAL
                logvalidacion = True
                Exit Sub
            End If
            If Not String.IsNullOrEmpty(ClienteSelected.EMail) Then
                Dim emails = ClienteSelected.EMail.Split(SEPARADORMAIL.Trim)
                For Each li In emails
                    If Not IsValidmail(li) Then
                        A2Utilidades.Mensajes.mostrarMensaje("El e-mail " & li & " no es valido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                        TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_GENERAL
                        logvalidacion = True
                        Exit Sub
                    End If
                Next
            End If
            If Not String.IsNullOrEmpty(ClienteSelected.EMailReciboInstruccion) Then
                Dim emails = ClienteSelected.EMailReciboInstruccion.Split(SEPARADORMAIL.Trim)
                For Each li In emails
                    If Not IsValidmail(li) Then
                        A2Utilidades.Mensajes.mostrarMensaje("El e-mail Instrucción " & li & " no es valido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                        TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_GENERAL
                        logvalidacion = True
                        Exit Sub
                    End If
                Next
            End If
            If IsNothing(ClienteSelected.TipoProducto) Or ClienteSelected.TipoProducto = "-1" Then
                A2Utilidades.Mensajes.mostrarMensaje("El campo tipo producto es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_GENERAL
                logvalidacion = True
                Exit Sub
            End If



            'Santiago Vergara - Octubre 22/2013 - Se verifica si se valida el tipo de producto
            'If logValidarTipoProducto Then
            '    If Not String.IsNullOrEmpty(ClienteSelected.TipoProducto) Then
            '        If Await ValidarClienteTipoProducto() Then
            '            TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
            '            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_GENERAL
            '            logvalidacion = True
            '            Exit Sub
            '        End If
            '    End If
            'End If
            'validaciones tab financiero
            If Not IsNothing(ClienteSelected.TipoPersona) Then
                ' Validaciones para una persona natural
                ' JBT20130213
                If ClienteSelected.TipoPersona = 1 Then
                    If String.IsNullOrEmpty(ClienteSelected.codigoCiiu) Then
                        A2Utilidades.Mensajes.mostrarMensaje("La actividad económica es requerida para personas naturales.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                        TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                        TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_GENERAL
                        logvalidacion = True
                        Exit Sub
                    End If
                End If
                ' Validaciones para una persona juridica
                ' JBT20130213
                If ClienteSelected.TipoPersona = 2 Then
                    If String.IsNullOrEmpty(ClienteSelected.Superintendencia) Then
                        A2Utilidades.Mensajes.mostrarMensaje("Por favor seleccione la entidad que vigila el cliente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logvalidacion = True
                        TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                        TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                        TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_GENERAL
                        Exit Sub
                    End If
                End If
            End If
            If IsNothing(ClienteSelected.Contribuyente) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe definir si el cliente es contribuyente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_GENERAL
                logvalidacion = True
                Exit Sub
            End If
            If IsNothing(ClienteSelected.Declarante) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe definir si el cliente es declarante.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_GENERAL
                logvalidacion = True
                Exit Sub
            End If
            If IsNothing(ClienteSelected.RetFuente) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe definir si el cliente está sujeto a retención.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_GENERAL
                logvalidacion = True
                Exit Sub
            End If


            If IsNothing(ClienteSelected.ExentoGMF) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe definir si el cliente es exento de GMF o no.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_GENERAL
                logvalidacion = True
                Exit Sub
            End If
            If IsNothing(ClienteSelected.AutoRetenedor) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe definir si el cliente es autoretenedor.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_GENERAL
                logvalidacion = True
                Exit Sub
            End If

            'Santiago Mazo -Febrero 08/2016 - Se agrega la validación del campo Reteica
            If ClientesSeVisibility.RETEICAvisible = Visibility.Visible And IsNothing(ClienteSelected.RETEICA) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un valor para el campo reteica.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_GENERAL
                logvalidacion = True
                Exit Sub
            End If

            If IsNothing(ClienteSelected.CREE) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe definir si al cliente se le aplica retención CREE .", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_GENERAL
                logvalidacion = True
                Exit Sub
            End If
            If logpagocomisiones Then
                'CFMA If ClienteSelected.Comision = 0 And ClienteSelected.PorcentajeComision = 0 Then
                If (IsNothing(ClienteSelected.Comision)) And (IsNothing(ClienteSelected.PorcentajeComision)) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar un valor para el porcentaje pago comisión o un valor para comisión fija .", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                    TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_GENERAL
                    logvalidacion = True
                    Exit Sub
                End If
            End If

            If (IsNothing(ClienteSelected.IngresoMensual)) And ClienteSelected.Excluido <> True Then
                A2Utilidades.Mensajes.mostrarMensaje("Ingreso mensual es un campo requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_GENERAL
                logvalidacion = True
                Exit Sub
            End If

            If (IsNothing(ClienteSelected.Activos)) And ClienteSelected.Excluido <> True Then
                A2Utilidades.Mensajes.mostrarMensaje("Los activos es un campo requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_GENERAL
                logvalidacion = True
                Exit Sub
            End If

            If (IsNothing(ClienteSelected.Patrimonio)) And ClienteSelected.Excluido <> True Then
                A2Utilidades.Mensajes.mostrarMensaje("El patrimonio es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_GENERAL
                logvalidacion = True
                Exit Sub
            End If
            If (IsNothing(ClienteSelected.Egresos)) And ClienteSelected.Excluido <> True Then
                A2Utilidades.Mensajes.mostrarMensaje("Los egresos son requeridos .", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_GENERAL
                logvalidacion = True
                Exit Sub
            End If
            If ClienteSelected.Patrimonio > ClienteSelected.Activos Then
                A2Utilidades.Mensajes.mostrarMensaje("El valor del patrimonio no puede ser mayor que los activos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_GENERAL
                logvalidacion = True
                Exit Sub
            End If
            If (VALIDA_VALORACTIVO = "SI") And ((IIf(ClienteSelected.Patrimonio Is Nothing, 0, ClienteSelected.Patrimonio) + IIf(ClienteSelected.Pasivos Is Nothing, 0, ClienteSelected.Pasivos)) <> IIf(ClienteSelected.Activos Is Nothing, 0, ClienteSelected.Activos)) Then
                A2Utilidades.Mensajes.mostrarMensaje("El valor registrado en los activos " + CStr(IIf(ClienteSelected.Activos Is Nothing, " ", ClienteSelected.Activos)) + ", no coinciden con el valor del pasivo " + CStr(IIf(ClienteSelected.Pasivos Is Nothing, " ", ClienteSelected.Pasivos)) + "+ Patrimonio " + CStr(IIf(ClienteSelected.Patrimonio Is Nothing, " ", ClienteSelected.Patrimonio)) _
                , Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_GENERAL
                logvalidacion = True
                Exit Sub
            End If

            If ClienteSelected.Utilidades > ClienteSelected.IngresoMensual Then
                A2Utilidades.Mensajes.mostrarMensaje("El valor de las utilidades no puede ser mayor que los ingresos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_GENERAL
                logvalidacion = True
                Exit Sub
            End If

            If HABILITAR_CLASIFICACIONES_EN_CLIENTES = Visibility.Visible Then
                If IsNothing(ClienteSelected.Pasivos) And ClienteSelected.Excluido <> True Then
                    A2Utilidades.Mensajes.mostrarMensaje("El pasivo es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                    TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_GENERAL
                    logvalidacion = True
                    Exit Sub
                End If
            End If

            If (IsNothing(ClienteSelected.OrigenIngresos) Or ClienteSelected.OrigenIngresos = String.Empty) And ClienteSelected.Excluido <> True Then
                A2Utilidades.Mensajes.mostrarMensaje("El origen de ingresos del cliente son requeridos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_GENERAL
                logvalidacion = True
                Exit Sub
            End If

            If Not IsNothing(ListaClientesTipoCliente) Then
                If ListaClientesTipoCliente.Count = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe registrar al menos un tipo de entidad.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logvalidacion = True
                    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                    TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_TIPOSENTIDAD
                    Exit Sub
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe registrar al menos un tipo de entidad.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logvalidacion = True
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_TIPOSENTIDAD
                Exit Sub
            End If

            If HABILITAR_CLASIFICACIONES_EN_CLIENTES = Visibility.Visible Then
                If IsNothing(ClienteSelected.strTipoClienteClasificacion) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el tipo de cliente / contraparte.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                    TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_CLASIFICACIONES
                    logvalidacion = True
                    Exit Sub
                End If

                If IsNothing(ClienteSelected.strPerteneceGrupoEconomico) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar que pertenece a un grupo económico reconocido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                    TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_CLASIFICACIONES
                    logvalidacion = True
                    Exit Sub
                End If

                If IsNothing(ClienteSelected.strCalificacionRiesgoSuperior) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la calificación de riesgo superior a AA+ .", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                    TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_CLASIFICACIONES
                    logvalidacion = True
                    Exit Sub
                End If

                If Not IsNothing(ListaClientesProductos) Then
                    For Each li In ListaClientesProductos
                        If (li.logOperaProducto) Then
                            logValidacionOperaProductos = True
                        End If
                    Next
                    If Not logValidacionOperaProductos Then
                        A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar al menos un producto en los que el cliente opera o va a operar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                        TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                        TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_CLASIFICACIONES
                        logvalidacion = True
                        Exit Sub
                    End If
                End If

            End If

            If Not IsNothing(ListaCuentasClientes) Then
                If InstalacioSelected.CuentasBancarias Then
                    If IsNothing(ListaCuentasClientes) Then
                        A2Utilidades.Mensajes.mostrarMensaje("Las cuentas bancarias son datos requeridos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logvalidacion = True
                        TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                        TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                        TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_CTASBANCARIAS
                        Exit Sub
                    ElseIf ListaCuentasClientes.Count = 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje("Las cuentas bancarias son datos requeridos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logvalidacion = True
                        TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                        TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                        TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_CTASBANCARIAS
                        Exit Sub
                    Else
                        For Each Lista In ListaCuentasClientes
                            If IsNothing(Lista.IDBanco) Then
                                logvalidacion = True
                                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_CTASBANCARIAS
                                A2Utilidades.Mensajes.mostrarMensaje("El banco en la cuenta bancaria es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                            If String.IsNullOrEmpty(Lista.NombreSucursal) Then
                                logvalidacion = True
                                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_CTASBANCARIAS
                                A2Utilidades.Mensajes.mostrarMensaje("La sucursal en la cuenta bancaria es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                            If String.IsNullOrEmpty(Lista.Cuenta) Then
                                logvalidacion = True
                                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_CTASBANCARIAS
                                A2Utilidades.Mensajes.mostrarMensaje("El nro. de cuenta en la cuenta bancaria es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                            If String.IsNullOrEmpty(Lista.TipoCuenta) Then
                                logvalidacion = True
                                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_CTASBANCARIAS
                                A2Utilidades.Mensajes.mostrarMensaje("El tipo de cuenta en la cuenta bancaria es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                            If String.IsNullOrEmpty(Lista.Titular) Then
                                logvalidacion = True
                                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_CTASBANCARIAS
                                A2Utilidades.Mensajes.mostrarMensaje("El titular en la cuenta bancaria es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                            If String.IsNullOrEmpty(Lista.TipoID) Then
                                logvalidacion = True
                                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_CTASBANCARIAS
                                A2Utilidades.Mensajes.mostrarMensaje("El tipo de identificación del titular en la cuenta bancaria es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                            If String.IsNullOrEmpty(Lista.NumeroID) Then
                                logvalidacion = True
                                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_CTASBANCARIAS
                                A2Utilidades.Mensajes.mostrarMensaje("El número ID del titular en la cuenta bancaria es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                        Next
                        'Dim esTransferir = ListaCuentasClientes.Where(Function(li) li.TransferirA = True)
                        'If esTransferir.Count > 1 Then
                        '    logvalidacion = True
                        '    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                        '    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                        '    TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_CTASBANCARIAS
                        '    A2Utilidades.Mensajes.mostrarMensaje("Sólo una cuenta puede ser marcada para  transferencias.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        '    Exit Sub
                        'End If
                        Dim esDividendo = ListaCuentasClientes.Where(Function(li) li.Dividendos = True)
                        If esDividendo.Count > 1 Then
                            logvalidacion = True
                            TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                            TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_CTASBANCARIAS
                            A2Utilidades.Mensajes.mostrarMensaje("Sólo una cuenta puede ser marcada como dividendos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If
                        For Each Lista In ListaCuentasClientes
                            Dim idbanco = Lista.IDBanco
                            Dim NombreSucursal = Lista.NombreSucursal
                            Dim Cuenta = Lista.Cuenta
                            Dim TipoCuenta = Lista.TipoCuenta
                            Dim NumeroID = Lista.NumeroID
                            Dim cuentasbancarias = ListaCuentasClientes.Where(Function(li) IIf(IsNothing(li.IDBanco), 0, li.IDBanco) = idbanco And IIf(IsNothing(li.NombreSucursal),
                                String.Empty, li.NombreSucursal).Trim.Equals(NombreSucursal) And IIf(IsNothing(li.Cuenta), String.Empty, li.Cuenta).Trim.Equals(Cuenta) And IIf(IsNothing(li.TipoCuenta), String.Empty, li.TipoCuenta).Trim.Equals(TipoCuenta) And IIf(IsNothing(li.NumeroID), String.Empty, li.NumeroID).Trim.Equals(NumeroID)).ToList

                            If cuentasbancarias.Count = 2 Then
                                logvalidacion = True
                                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_CTASBANCARIAS
                                A2Utilidades.Mensajes.mostrarMensaje("No puede existir dos cuentas bancarias iguales.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If

                        Next
                    End If
                Else
                    If ListaCuentasClientes.Count > 0 Then
                        'Dim esTransferir = ListaCuentasClientes.Where(Function(li) li.TransferirA = True)
                        'If esTransferir.Count > 1 Then
                        '    logvalidacion = True
                        '    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                        '    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                        '    TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_CTASBANCARIAS
                        '    A2Utilidades.Mensajes.mostrarMensaje("Solo una cuenta puede ser marcada para  transferencias.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        '    Exit Sub
                        'End If
                        Dim esDividendo = ListaCuentasClientes.Where(Function(li) li.Dividendos = True)
                        If esDividendo.Count > 1 Then
                            logvalidacion = True
                            TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                            TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_CTASBANCARIAS
                            A2Utilidades.Mensajes.mostrarMensaje("Solo una cuenta puede ser marcada como dividendos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If

                        Dim esOperaciones = ListaCuentasClientes.Where(Function(li) li.Operaciones = True)
                        If esOperaciones.Count > 1 Then
                            logvalidacion = True
                            TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                            TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_CTASBANCARIAS
                            A2Utilidades.Mensajes.mostrarMensaje("Solo una cuenta puede ser marcada como operaciones.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If

                        For Each Lista In ListaCuentasClientes
                            Dim idbanco = Lista.IDBanco
                            Dim NombreSucursal = Lista.NombreSucursal
                            Dim Cuenta = Lista.Cuenta
                            Dim TipoCuenta = Lista.TipoCuenta
                            Dim NumeroID = Lista.NumeroID
                            Dim cuentasbancarias = ListaCuentasClientes.Where(Function(li) IIf(IsNothing(li.IDBanco), 0, li.IDBanco) = idbanco And IIf(IsNothing(li.NombreSucursal),
                                String.Empty, li.NombreSucursal).Trim.Equals(NombreSucursal) And IIf(IsNothing(li.Cuenta), String.Empty, li.Cuenta).Trim.Equals(Cuenta) And IIf(IsNothing(li.TipoCuenta), String.Empty, li.TipoCuenta).Trim.Equals(TipoCuenta) And IIf(IsNothing(li.NumeroID), String.Empty, li.NumeroID).Trim.Equals(NumeroID)).ToList

                            If cuentasbancarias.Count = 2 Then
                                logvalidacion = True
                                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_CTASBANCARIAS
                                A2Utilidades.Mensajes.mostrarMensaje("No puede existir dos cuentas bancarias iguales.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If

                        Next
                    End If
                End If
            End If


            If ClienteSelected.OpMonedaExtranjera Then
                If IsNothing(ClienteSelected.TipoOperacion) Then
                    A2Utilidades.Mensajes.mostrarMensaje("El tipo de operación es requerido .", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logvalidacion = True
                    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                    TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_OPTEXTRANJERA
                    Exit Sub
                End If
                If String.IsNullOrEmpty(ClienteSelected.BancoExt) Then
                    A2Utilidades.Mensajes.mostrarMensaje("El Nombre del banco extranjero es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logvalidacion = True
                    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                    TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_OPTEXTRANJERA
                    Exit Sub
                End If
                If String.IsNullOrEmpty(ClienteSelected.Moneda) Then
                    A2Utilidades.Mensajes.mostrarMensaje("La moneda es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logvalidacion = True
                    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                    TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_OPTEXTRANJERA
                    Exit Sub
                End If
                If String.IsNullOrEmpty(ClienteSelected.NroCuentaExt) Then
                    A2Utilidades.Mensajes.mostrarMensaje("El nro extranjero es requerido .", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logvalidacion = True
                    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                    TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_OPTEXTRANJERA
                    Exit Sub
                End If
                If String.IsNullOrEmpty(ClienteSelected.CiudadExt) Then
                    A2Utilidades.Mensajes.mostrarMensaje("La ciudad extranjera es requerida .", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logvalidacion = True
                    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                    TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_OPTEXTRANJERA
                    Exit Sub
                End If
                If String.IsNullOrEmpty(ClienteSelected.PaisExt) Then
                    A2Utilidades.Mensajes.mostrarMensaje("El pais es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logvalidacion = True
                    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                    TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_OPTEXTRANJERA
                    Exit Sub
                End If
                If (Not IsNothing(ClienteSelected.CualOtroTipoOper) And ClienteSelected.CualOtroTipoOper <> String.Empty) And (ClienteSelected.TipoOperacion <> 0) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Si elige otro tipo de operación debe elegir en la lista la opción otro tipo.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logvalidacion = True
                    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                    TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_OPTEXTRANJERA
                    Exit Sub
                End If
                If (String.IsNullOrEmpty(ClienteSelected.CualOtroTipoOper)) And (ClienteSelected.TipoOperacion = 0) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Si elige en la lista la opción otro tipo,debe colocar cual.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logvalidacion = True
                    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                    TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_OPTEXTRANJERA
                    Exit Sub
                End If
            End If

            'validaciones tab receptores
            If Not IsNothing(ListaClientesReceptore) Then
                If ListaClientesReceptore.Count = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe registrar al menos un receptor.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logvalidacion = True
                    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_RECEPTORES
                    Exit Sub
                Else
                    strCodigoReceptorLider = String.Empty
                    Dim obj = ListaClientesReceptore.Where(Function(li) li.Lider = True).ToList
                    If obj.Count = 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje("Debe haber al menos un receptor líder.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logvalidacion = True
                        TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                        TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_RECEPTORES
                        Exit Sub
                    ElseIf obj.Count > 1 Then
                        logvalidacion = True
                        TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                        TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_RECEPTORES
                        A2Utilidades.Mensajes.mostrarMensaje("No puede existir dos receptores líderes.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                    strCodigoReceptorLider = obj.FirstOrDefault.IDReceptor
                    If LogdistribucionReceptor Then
                        Dim dblporcentaje As Double
                        For Each LI In ListaClientesReceptore
                            If Not IsNothing(LI.Porcentaje) Then
                                dblporcentaje = dblporcentaje + LI.Porcentaje
                            End If
                        Next
                        If dblporcentaje <> 100 Then
                            logvalidacion = True
                            TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_RECEPTORES
                            A2Utilidades.Mensajes.mostrarMensaje("La suma de los porcentajes debe ser igual a 100", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe registrar al menos un receptor.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logvalidacion = True
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_RECEPTORES
                Exit Sub
            End If
            'validaciones tab ordenantes
            If Not IsNothing(ListaClientesOrdenantes) Then
                If ListaClientesOrdenantes.Count = 0 Then
                    If ClienteSelected.TipoVinculacion.Equals("A") Or ClienteSelected.TipoVinculacion.Equals("O") Then
                        Dim NewClientesOrdenantesSelected As New OyDClientes.ClientesOrdenante
                        NewClientesOrdenantesSelected.IDComitente = ClienteSelected.IDComitente
                        NewClientesOrdenantesSelected.IDOrdenante = ClienteSelected.IDComitente
                        NewClientesOrdenantesSelected.Nombre = ClienteSelected.Nombre
                        NewClientesOrdenantesSelected.Usuario = Program.Usuario
                        NewClientesOrdenantesSelected.lider = True
                        NewClientesOrdenantesSelected.Asociados = 0
                        NewClientesOrdenantesSelected.Relacionado = False
                        ListaClientesOrdenantes.Add(NewClientesOrdenantesSelected)
                        ClientesOrdenantesSelected = NewClientesOrdenantesSelected
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Debe registrar al menos un ordenante.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                        TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_ORDENANTES
                        logvalidacion = True
                        Exit Sub
                    End If
                Else
                    Dim objOrd = ListaClientesOrdenantes.Where(Function(li) String.IsNullOrEmpty(li.IDOrdenante)).ToList
                    If Not IsNothing(objOrd) Then
                        If objOrd.Count > 0 Then
                            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el ordenante.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_ORDENANTES
                            logvalidacion = True
                            Exit Sub
                        End If
                    End If

                    Dim obj = ListaClientesOrdenantes.Where(Function(li) li.lider = True).ToList
                    If Not IsNothing(obj) Then
                        If obj.Count = 0 Then
                            A2Utilidades.Mensajes.mostrarMensaje("Debe haber al menos un ordenante líder.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_ORDENANTES
                            logvalidacion = True
                            Exit Sub
                        ElseIf obj.Count > 1 Then
                            logvalidacion = True
                            TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_ORDENANTES
                            A2Utilidades.Mensajes.mostrarMensaje("No pueden existir dos ordenantes líderes.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            Exit Sub
                        End If
                    End If





                    Dim re = ListaClientesOrdenantes.Where(Function(li) li.Asociados >= 5 And (IIf(IsNothing(li.Relacionado), False, li.Relacionado) = False)).ToList

                    If Not IsNothing(re) Then
                        If re.Count > 0 Then
                            A2Utilidades.Mensajes.mostrarMensaje("El ordenante " & re(0).IDOrdenante.Trim.ToString & " ya tiene " & re(0).Asociados & " comitentes registrados", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_ORDENANTES
                            logvalidacion = True
                            Exit Sub
                        End If
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe registrar al menos un ordenante.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_ORDENANTES
                logvalidacion = True
                Exit Sub
            End If
            'validaciones tab beneficiarios
            If Not IsNothing(ListaClientesBeneficiarios) Then
                If ListaClientesBeneficiarios.Count > 0 Then
                    For Each Lista In ListaClientesBeneficiarios
                        Dim TipoID = Lista.TipoID
                        Dim nrodocumento = Lista.NroDocumento
                        Dim beneficiario = ListaClientesBeneficiarios.Where(Function(li) IIf(IsNothing(li.TipoID), String.Empty, li.TipoID).Equals(TipoID) And IIf(IsNothing(li.NroDocumento), String.Empty, li.NroDocumento).Equals(nrodocumento)).ToList
                        If beneficiario.Count = 2 Then
                            A2Utilidades.Mensajes.mostrarMensaje("No puede existir dos beneficiarios con igual tipo y número de documento.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logvalidacion = True
                            TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_BENEFICIARIOS
                            Exit Sub
                        End If
                        If Lista.NroDocumento = 0 Or IsNothing(Lista.NroDocumento) Then
                            A2Utilidades.Mensajes.mostrarMensaje("El número del documento del beneficiario es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logvalidacion = True
                            TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_BENEFICIARIOS
                            Exit Sub
                        End If

                    Next
                End If
            End If

            'validaciones tab ficha
            If Not IsNothing(ClienteSelected.TipoPersona) Then
                ' Validaciones para una persona natural
                ' JBT20130213
                If ClienteSelected.TipoPersona = 1 Then
                    If IsNothing(ClienteSelected.EstadoCivil) Or ClienteSelected.EstadoCivil = String.Empty Then
                        A2Utilidades.Mensajes.mostrarMensaje("El estado civil es requerido para personas naturales.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                        TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FICHA
                        logvalidacion = True
                        Exit Sub
                    End If
                    If IsNothing(ClienteSelected.Sexo) Or ClienteSelected.Sexo = String.Empty Then
                        A2Utilidades.Mensajes.mostrarMensaje("El sexo es requerido para personas naturales.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                        TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FICHA
                        logvalidacion = True
                        Exit Sub
                    End If
                    If IsNothing(ClienteSelected.Nacimiento) Then
                        A2Utilidades.Mensajes.mostrarMensaje("La fecha de nacimiento es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                        TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FICHA
                        logvalidacion = True
                        Exit Sub
                    End If
                    If INFOLABORALREQUERIDO = "SI" Then
                        If IsNothing(ClienteSelected.IdProfesion) Or ClienteSelected.IdProfesion = 0 Then
                            A2Utilidades.Mensajes.mostrarMensaje("La profesión del cliente es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FICHA
                            logvalidacion = True
                            Exit Sub
                        End If
                        If IsNothing(ClienteSelected.Ocupacion) Then
                            A2Utilidades.Mensajes.mostrarMensaje("La ocupación del cliente es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FICHA
                            logvalidacion = True
                            Exit Sub
                        End If
                    End If
                End If
                ' Validaciones para una persona juridica
                ' JBT20130213
                If ClienteSelected.TipoPersona = 2 Then
                    If InstalacioSelected.RepresentanteLegal Then

                        If LTrim(RTrim(ClienteSelected.TipoReprLegal)) = String.Empty Then
                            A2Utilidades.Mensajes.mostrarMensaje("El Tipo de identificación del representante es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logvalidacion = True
                            TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FICHA
                            Exit Sub
                        End If
                        'CFMA20180531 valida correctamente que exista un valor
                        'If ClienteSelected.IDReprLegal = 0 Or IsNothing(ClienteSelected.IDReprLegal) Then
                        If String.IsNullOrEmpty(ClienteSelected.IDReprLegal) Then
                            A2Utilidades.Mensajes.mostrarMensaje("El número de identificación del representante es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logvalidacion = True
                            TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FICHA
                            Exit Sub
                        End If
                        If String.IsNullOrEmpty(ClienteSelected.RepresentanteLegal) Then
                            A2Utilidades.Mensajes.mostrarMensaje("El nombre y el apellido del representante son requeridos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logvalidacion = True
                            TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FICHA
                            Exit Sub
                        End If
                        If String.IsNullOrEmpty(ClienteSelected.CargoReprLegal) Then
                            A2Utilidades.Mensajes.mostrarMensaje("El cargo del representante es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logvalidacion = True
                            TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FICHA
                            Exit Sub
                        End If
                        If ClienteSelected.TelefonoReprLegal = String.Empty Then
                            A2Utilidades.Mensajes.mostrarMensaje("El teléfono del representante es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logvalidacion = True
                            TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FICHA
                            Exit Sub
                        End If
                        If ClienteSelected.IDCiudadReprLegal = 0 Or IsNothing(ClienteSelected.IDCiudadReprLegal) Or _ClienteSelected.IDCiudadReprLegal = 999999 Then
                            A2Utilidades.Mensajes.mostrarMensaje("La ciudad de residencia del representante es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logvalidacion = True
                            TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FICHA
                            Exit Sub
                        End If
                        If ClienteSelected.DireccionReprLegal = String.Empty Then
                            A2Utilidades.Mensajes.mostrarMensaje("La dirección del representante es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logvalidacion = True
                            TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FICHA
                            Exit Sub
                        End If

                    End If
                    If IsNothing(ClienteSelected.Nacimiento) Then
                        A2Utilidades.Mensajes.mostrarMensaje("La fecha de constitución de la empresa es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                        TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FICHA
                        logvalidacion = True
                        Exit Sub
                    End If
                End If

            End If
            If ClienteSelected.TipoVinculacion.Equals("A") Or ClienteSelected.TipoVinculacion.Equals("O") Then
                If DateAdd("yyyy", -18, Now) < ClienteSelected.Nacimiento And ClienteSelected.TipoPersona = 1 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Un ordenante no puede ser menor de edad.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    ClienteSelected.MenorEdad = True
                    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FICHA
                    logvalidacion = True
                    Exit Sub
                End If
            End If
            If IsNothing(ClienteSelected.IDNacionalidad) Or ClienteSelected.IDNacionalidad = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("La nacionalidad es un dato requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FICHA
                logvalidacion = True
                Exit Sub
            End If
            If strObligasucursalcliente = "SI" And IsNothing(ClienteSelected.IDSucursal) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe elegir la sucursal del cliente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FICHA
                logvalidacion = True
                Exit Sub
            End If

            'Modifica por Juan David Correa.
            'Se actualiza el valor de la variable MenorEdad
            'y se condiciona la validación dependiendo de un parametro.
            If DateAdd("yyyy", -18, Now) < ClienteSelected.Nacimiento And ClienteSelected.TipoPersona = 1 Then
                ClienteSelected.MenorEdad = True
            Else
                ClienteSelected.MenorEdad = False
            End If

            If VALIDA_TIPOIDENTIFICACIONNACIMIENTO Then
                If ClienteSelected.MenorEdad Then
                    If intDocummenor = 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje("Un menor de edad no puede tener este tipo de documento ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                        TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FICHA
                        logvalidacion = True
                        Exit Sub
                    End If
                Else
                    If intDocummenor = 1 Then
                        A2Utilidades.Mensajes.mostrarMensaje("Un mayor de edad no puede tener este tipo de documento ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                        TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FICHA
                        logvalidacion = True
                        Exit Sub
                    End If
                End If
            End If

            If ClienteSelected.TipoPersona = 1 Then
                If (IsNothing(ClienteSelected.IdCiudadNacimiento) Or ClienteSelected.IdCiudadNacimiento = 0) And (ClienteSelected.ReplicarSafyrFondos) Then
                    A2Utilidades.Mensajes.mostrarMensaje("La ciudad de nacimiento del cliente es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logvalidacion = True
                    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FICHA
                    Exit Sub
                End If
            End If


            'validaciones tab ubicacion
            If Not IsNothing(ListaClientesDirecciones) Then
                If ListaClientesDirecciones.Count = 0 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe registrar al menos una ubicación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_UBICACION
                    logvalidacion = True
                    Exit Sub
                Else
                    Dim obj = ListaClientesDirecciones.Where(Function(li) li.DireccionEnvio.Value = True And li.Activo.Value = True).ToList
                    If obj.Count = 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje("Debe al menos ingresar una dirección de envío activa.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                        TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_UBICACION
                        logvalidacion = True
                        Exit Sub
                    End If
                    For Each li In ListaClientesDirecciones
                        If (li.Direccion Is Nothing Or li.Direccion = String.Empty) And (li.Tipo = "O" Or li.Tipo = "C") Then
                            A2Utilidades.Mensajes.mostrarMensaje("La dirección de la " + CStr(IIf(li.Tipo = "O", "Oficina", "Residencia")) + " es requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_UBICACION
                            logvalidacion = True
                            Exit Sub
                        End If
                        If li.Telefono Is Nothing And (li.Tipo = "O" Or li.Tipo = "C") Then
                            A2Utilidades.Mensajes.mostrarMensaje("El teléfono de la " + CStr(IIf(li.Tipo = "O", "Oficina", "Residencia")) + " es requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_UBICACION
                            logvalidacion = True
                            Exit Sub
                        End If
                        If (li.Ciudad Is Nothing Or li.Ciudad = 0) Then
                            A2Utilidades.Mensajes.mostrarMensaje("La ciudad es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_UBICACION
                            logvalidacion = True
                            Exit Sub
                        End If
                    Next

                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe registrar al menos una ubicación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_UBICACION
                logvalidacion = True
                Exit Sub
            End If

            If Not IsNothing(ListaClientesDirecciones) Then
                Dim esdireccionenvio = ListaClientesDirecciones.Where(Function(li) li.DireccionEnvio = True)
                If esdireccionenvio.Count >= 2 Then
                    A2Utilidades.Mensajes.mostrarMensaje("Sólo puede elegir una Dirección de envio para el cliente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_UBICACION
                    logvalidacion = True
                    Exit Sub
                End If
            End If

            If String.IsNullOrEmpty(ClienteSelected.Telefono1) And String.IsNullOrEmpty(ClienteSelected.Fax2) Then
                A2Utilidades.Mensajes.mostrarMensaje("El teléfono principal o el celular es requerido", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_GENERAL
                logvalidacion = True
                Exit Sub
            End If
            If Not String.IsNullOrEmpty(ClienteSelected.Fax2) And Not String.IsNullOrEmpty(TOTALDIGITOSCELULAR) Then
                If (ClienteSelected.Fax2.Length > 0) And (ClienteSelected.Fax2.Length <> CInt(TOTALDIGITOSCELULAR) And (CInt(TOTALDIGITOSCELULAR) > 0 And CInt(TOTALDIGITOSCELULAR) <= 25)) Then
                    A2Utilidades.Mensajes.mostrarMensaje("El número de celular no es correcto. El total de digitos requerido es " + TOTALDIGITOSCELULAR, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_GENERAL
                    logvalidacion = True
                    Exit Sub
                End If
            End If


            'validaciones tab mercadeo
            If Not IsNothing(ClienteSelected.Entrevista) Or Not String.IsNullOrEmpty(ClienteSelected.ReceptorEntrevista) Or Not String.IsNullOrEmpty(ClienteSelected.LugarEntrevista) Or Not String.IsNullOrEmpty(ClienteSelected.ObservacionEnt) Then
                If IsNothing(ClienteSelected.Entrevista) Or String.IsNullOrEmpty(ClienteSelected.ReceptorEntrevista) Or String.IsNullOrEmpty(ClienteSelected.LugarEntrevista) Or String.IsNullOrEmpty(ClienteSelected.ObservacionEnt) Then
                    logvalidacion = True
                    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_MERCADEO
                    TabSeleccionadaMercadeo = 0
                    A2Utilidades.Mensajes.mostrarMensaje("Los datos de la entrevista no estan completos", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                End If
            End If

            If Not IsNothing(ListaClientesFicha) Then
                For Each li In ListaClientesFicha
                    If String.IsNullOrEmpty(li.Descripcion) Then
                        logvalidacion = True
                        TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                        TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_MERCADEO
                        TabSeleccionadaMercadeo = 4
                        A2Utilidades.Mensajes.mostrarMensaje("Debe elegir la descripción del tipo de correspondencia", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                Next
            End If

            If Not IsNothing(ClienteSelected.TipoPersona) Then
                ' Validaciones para una persona juridica
                ' JBT20130213
                If ClienteSelected.TipoPersona = 2 Then
                    If Not IsNothing(ListaClientesAccionistas) Then
                        Dim ValorTotalPaticipacion As Decimal = 0
                        Dim intparticipacionaccionistasReal As Decimal
                        Dim Contadordet As Integer = 0

                        If intparticipacionaccionistas > 100 Then
                            intparticipacionaccionistasReal = 100
                        Else
                            intparticipacionaccionistasReal = intparticipacionaccionistas
                        End If

                        For Each li In ListaClientesAccionistas
                            Contadordet += 1
                            If Not IsNothing(li.Participacion) Then
                                ValorTotalPaticipacion += li.Participacion
                            End If
                            If String.IsNullOrEmpty(li.TipoIdentificacion) Then
                                logvalidacion = True
                                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_MERCADEO
                                TabSeleccionadaMercadeo = 3
                                A2Utilidades.Mensajes.mostrarMensaje("Debe elegir el tipo de identificación del accionista", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                            If String.IsNullOrEmpty(li.Nombre1) Then
                                logvalidacion = True
                                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_MERCADEO
                                TabSeleccionadaMercadeo = 3
                                A2Utilidades.Mensajes.mostrarMensaje("Debe digitar el nombre del accionista", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                            If String.IsNullOrEmpty(li.NroDocumento) Then
                                logvalidacion = True
                                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_MERCADEO
                                TabSeleccionadaMercadeo = 3
                                A2Utilidades.Mensajes.mostrarMensaje("Debe digitar el número de documento del accionista", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                            If li.Participacion > 100 Or li.Participacion < 0 Or IsNothing(li.Participacion) Then
                                logvalidacion = True
                                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_MERCADEO
                                TabSeleccionadaMercadeo = 3
                                A2Utilidades.Mensajes.mostrarMensaje("Ingrese un valor mayor o igual a 0 y menor o igual a 100", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Exit Sub
                            End If
                        Next
                        If Contadordet <> 0 Then
                            If ValorTotalPaticipacion < intparticipacionaccionistasReal Or ValorTotalPaticipacion > 100 Then
                                logvalidacion = True
                                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_MERCADEO
                                TabSeleccionadaMercadeo = 3
                                If intparticipacionaccionistasReal = 100 Then
                                    A2Utilidades.Mensajes.mostrarMensaje("La suma de los porcentajes de participación debe ser igual a 100", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                Else
                                    A2Utilidades.Mensajes.mostrarMensaje("La suma de los porcentajes de participación debe ser mayor o igual que " + intparticipacionaccionistasReal.ToString() + " y menor o igual que 100", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                End If
                                Exit Sub
                            End If
                        End If
                    End If
                End If
            End If

            'validaciones tab infadicional
            If HABILITAR_CLASIFICACIONES_EN_CLIENTES = Visibility.Visible Then
                If ClienteSelected.PerfilRiesgo = "-1" Then
                    A2Utilidades.Mensajes.mostrarMensaje("El perfil riesgo del cliente es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logvalidacion = True
                    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_INFADICIONAL
                    Exit Sub
                End If
            End If

            If (String.IsNullOrEmpty(ClienteSelected.IndicativoTelefono)) And (ClienteSelected.ReplicarMercansoft) Then
                A2Utilidades.Mensajes.mostrarMensaje("El indicativo telefónico del cliente es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logvalidacion = True
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_INFADICIONAL
                Exit Sub
            End If
            'se vuelve a habilitar la validación caso 5467
            If (IsNothing(ClienteSelected.TipoIntermediario) Or ClienteSelected.TipoIntermediario = 0) And (ClienteSelected.ReplicarMercansoft) Then
                A2Utilidades.Mensajes.mostrarMensaje("El tipo de intermediario cambiario es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logvalidacion = True
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_INFADICIONAL
                Exit Sub
            End If
            If ((ClienteSelected.TipoIntermediario = CODIGO_INTERCLIENTEXTRANJERO) And (ClienteSelected.codEmpresaExtranjera = Nothing Or IsNothing(ClienteSelected.IDPaisExtranjera))) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar los campos Cod. super emp extranjera  y país origen emp extranjera.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logvalidacion = True
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_INFADICIONAL
                Exit Sub
            End If
            If VALIDACONTRASEÑA_CLIENTE = "SI" And ClienteSelected.EnviarInformeEconomico = "1" And String.IsNullOrEmpty(ClienteSelected.ClaveInternet) Then
                A2Utilidades.Mensajes.mostrarMensaje("La contraseña es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logvalidacion = True
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_INFADICIONAL
                Exit Sub
            End If


            If HABILITAR_CLASIFICACIONES_EN_CLIENTES = Visibility.Visible Then
                If ClienteSelected.CategoriaCliente = "-1" Then
                    A2Utilidades.Mensajes.mostrarMensaje("La categoría del cliente es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logvalidacion = True
                    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_INFADICIONAL
                    Exit Sub
                End If
            End If

            If ClienteSelected.Perfil = "-1" Then
                A2Utilidades.Mensajes.mostrarMensaje("El perfil del cliente es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logvalidacion = True
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_INFADICIONAL
                Exit Sub
            End If

            If String.IsNullOrEmpty(ClienteSelected.strInstruccionCRCC) And PantallasParametrizacionTipoBoolean("strInstruccionCRCC", "logEsObligatorio") Then
                A2Utilidades.Mensajes.mostrarMensaje("La instrucción CRCC del cliente es requerida.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logvalidacion = True
                TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_INFADICIONAL
                Exit Sub
            End If

            'Esta validación se quita, ya que por las pruebas hechas para BTG, no aplica, ya que este campo siempre se valida para persona natural, sin importar si es PEP.
            'RBP20160215_
            'If VALIDAOCUPACIONCONPEP_CLIENTE = "SI" And String.IsNullOrEmpty(ClienteSelected.Ocupacion) And strdescripcionperfil.Contains("PEP") Then
            '    A2Utilidades.Mensajes.mostrarMensaje("La ocupación del cliente es requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    logvalidacion = True
            '    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
            '    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FICHA
            '    Exit Sub
            'End If

            'validaciones tab documentos recibidos
            If Not IsNothing(ListaClientesDocumentosRequeridos) Then
                For Each li In ListaClientesDocumentosRequeridos
                    If li.Entregado Then
                        If Not IsNothing(li.Entrega) Then
                            If li.Entrega.Value.Date > Now.Date Then
                                A2Utilidades.Mensajes.mostrarMensaje("La fecha no puede ser mayor a la fecha actual docto " + li.NombreDocumento + " .", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                logvalidacion = True
                                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_DOCUMRECIBIDOS
                                Exit Sub
                            End If
                        End If
                        If li.FechaIniVigenciaDocto Then
                            If li.IniVigencia Is Nothing Then
                                A2Utilidades.Mensajes.mostrarMensaje("Por favor ingrese la fecha de inicio de vigencia al documento,docto " + li.NombreDocumento + " .", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                logvalidacion = True
                                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_DOCUMRECIBIDOS
                                Exit Sub
                            End If
                        End If
                        If li.FechaFinVigenciaDocto Then
                            If li.FinVigencia Is Nothing Then
                                A2Utilidades.Mensajes.mostrarMensaje("Por favor ingrese la fecha de fin de vigencia al documento,docto " + li.NombreDocumento + " .", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                logvalidacion = True
                                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_DOCUMRECIBIDOS
                                Exit Sub
                            End If
                        End If
                        If Not IsNothing(li.IniVigencia) And Not IsNothing(li.FinVigencia) Then
                            If li.FinVigencia.Value.Date < li.IniVigencia.Value.Date Then
                                A2Utilidades.Mensajes.mostrarMensaje("La fecha de vigencia hasta  no puede ser menor a la fecha de vigencia desde, docto " + li.NombreDocumento + " .", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                logvalidacion = True
                                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_DOCUMRECIBIDOS
                                Exit Sub
                            End If
                        End If
                    End If
                Next

                For Each li In ListaClientesDocumentosRequeridos
                    If li.Entregado = False Then
                        If li.Requerido Then
                            If strObligatoriosDoc = "NO" Then
                                'C1.Silverlight.C1MessageBox.Show("Existen documentos requeridos que no han sido entregados, ¿desea dejarlos pendientes?", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminovalidarGenerales)
                                'mostrarMensajePregunta("Existen documentos requeridos que no han sido entregados.", _
                                '                       Program.TituloSistema, _
                                '                       "documentosrequeridos", _
                                '                       AddressOf TerminovalidarGenerales, True, "¿desea dejarlos pendientes?")
                                ListaRespuestaValidacionesCientes.Add(New OyDClientes.tblRespuestaValidacionesCientes With {.ID = -1, .Mensaje = "Existen documentos requeridos que no han sido entregados.",
                                .RequiereConfirmacion = True, .DetieneIngreso = False, .Confirmacion = "¿desea dejarlos pendientes?", .Tab = TAB_PRINCIPAL_GENERAL_DOCUMRECIBIDOS})
                                logvalidacion = True
                                'Exit Sub
                                Exit For
                            Else
                                A2Utilidades.Mensajes.mostrarMensaje("Existen documentos requeridos que no han sido entregados por tanto no es posible guardar el registro. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                logvalidacion = True
                                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_DOCUMRECIBIDOS
                                Exit Sub
                            End If
                        End If
                    End If
                Next
            End If

            'NAOC20141118 - Fatca
            '********************************************************************************************
            If FATCA_CAMPOSADICIONALES Then
                'Validaciones de pestaña de Fatca
                If IsNothing(_ClienteSelected.AplicaFatca) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Aplica FATCA es un campo requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logvalidacion = True
                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                    TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_FATCA
                    Exit Sub
                End If
                If IsNothing(_ClienteSelected.CiudadanoResidenteDomicilio) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Es ciudadano, residente o tiene domicilio fiscal en otro país es un campo requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    logvalidacion = True
                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                    TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_FATCA
                    Exit Sub
                ElseIf _ClienteSelected.CiudadanoResidenteDomicilio Then
                    If IsNothing(_ClienteSelected.TransfiereACuentasEEUU) Then
                        A2Utilidades.Mensajes.mostrarMensaje("Transfiere fondos a cuentas en los Estados Unidos es un campo requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logvalidacion = True
                        TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                        TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_FATCA
                        Exit Sub
                    ElseIf _ClienteSelected.TransfiereACuentasEEUU Then
                        If String.IsNullOrEmpty(_ClienteSelected.TitularCuentaEEUU) Then
                            A2Utilidades.Mensajes.mostrarMensaje("Nombre del titular de la cuenta donde se transfiere fondos es un campo requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logvalidacion = True
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                            TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_FATCA
                            Exit Sub
                        End If

                        If String.IsNullOrEmpty(_ClienteSelected.EntidadTransferencia) Then
                            A2Utilidades.Mensajes.mostrarMensaje("Nombre de la entidad donde se transfiere fondos es un campo requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logvalidacion = True
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                            TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_FATCA
                            Exit Sub
                        End If
                    End If

                    'validaciones tab documentos recibidos
                    If Not IsNothing(ListaClientesPaisesFATCA) Then
                        If ListaClientesPaisesFATCA.Count > 0 Then
                            For Each li In ListaClientesPaisesFATCA
                                If IsNothing(li.IDPais) Or li.IDPais = 0 Then
                                    A2Utilidades.Mensajes.mostrarMensaje("Por favor ingrese el pais en el detalle relación Fatca.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    logvalidacion = True
                                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                                    TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_FATCA
                                    Exit Sub
                                End If
                                If String.IsNullOrEmpty(li.NIF) Then
                                    A2Utilidades.Mensajes.mostrarMensaje("Por favor ingrese el Nro. Identificación tributario NIF en el detalle relación fatca.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    logvalidacion = True
                                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                                    TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_FATCA
                                    Exit Sub
                                End If
                                If String.IsNullOrEmpty(li.TipoCiudadano) Then
                                    A2Utilidades.Mensajes.mostrarMensaje("Por favor ingrese la relación con el país en el detalle relación fatca.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    logvalidacion = True
                                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                                    TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_FATCA
                                    Exit Sub
                                End If
                            Next

                            'validar los duplicados
                            For Each li In ListaClientesPaisesFATCA
                                If ListaClientesPaisesFATCA.Where(Function(i) i.IDPais = li.IDPais And i.NIF = li.NIF And i.TipoCiudadano = li.TipoCiudadano).Count > 1 Then
                                    A2Utilidades.Mensajes.mostrarMensaje("Hay registros duplicados en el detalle relación fatca.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    logvalidacion = True
                                    TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                                    TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_FATCA
                                    Exit Sub
                                End If
                            Next
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("Debe de existir al menos un detalle de relación en fatca.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logvalidacion = True
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                            TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_FATCA
                            Exit Sub
                        End If
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Debe de existir al menos un detalle de relación en fatca.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        logvalidacion = True
                        TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                        TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_FATCA
                        Exit Sub
                    End If

                    'Validaciones de pestaña de Fatca representante legal
                    If _ClienteSelected.TipoPersona = 2 Then
                        If IsNothing(_ClienteSelected.EmpresaListadaBolsa) Then
                            A2Utilidades.Mensajes.mostrarMensaje("Es una empresa listada en bolsa es un campo requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logvalidacion = True
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                            TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_FATCAREPRESENTANTE
                            Exit Sub

                        ElseIf _ClienteSelected.EmpresaListadaBolsa Then
                            If String.IsNullOrEmpty(_ClienteSelected.MercadoNegociaAcciones) Then
                                A2Utilidades.Mensajes.mostrarMensaje("Mercados donde negocia acciones es un campo requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                logvalidacion = True
                                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_FATCAREPRESENTANTE
                                Exit Sub
                            End If
                        End If

                        If IsNothing(_ClienteSelected.SubsidiariaDeEntidad) Then
                            A2Utilidades.Mensajes.mostrarMensaje("Subsidiaria de una entidad pública o regulada es un campo requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            logvalidacion = True
                            TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                            TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_FATCAREPRESENTANTE
                            Exit Sub

                        ElseIf _ClienteSelected.SubsidiariaDeEntidad Then
                            If String.IsNullOrEmpty(_ClienteSelected.EmpresaMatriz) Then
                                A2Utilidades.Mensajes.mostrarMensaje("Nombre de la empresa matriz es un campo requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                logvalidacion = True
                                TabSeleccionadaFinanciero = TAB_PRINCIPAL_GENERAL_FINANCIERO
                                TabSeleccionado = TAB_PRINCIPAL_GENERAL_FINANCIERO_FATCAREPRESENTANTE
                                Exit Sub
                            End If
                        End If
                    End If

                End If

            End If

            'CFMA20170404
            'If IsNothing(ClienteSelected.AutorizaTratamiento) Then
            '    A2Utilidades.Mensajes.mostrarMensaje("Debe definir si el cliente autoriza tratamiento de datos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
            '    TabSeleccionado = TAB_PRINCIPAL_GENERAL_INFADICIONAL
            '    logvalidacion = True
            '    Exit Sub
            'End If

            If IsNothing(ClienteSelected.AutorizaTratamiento) Then
                If ListaClientes.Where(Function(li) li.IDClientes = ClienteSelected.IDClientes).Count <> 0 Then 'Editando Then
                    ListaRespuestaValidacionesCientes.Add(New OyDClientes.tblRespuestaValidacionesCientes With {.ID = -1, .Mensaje = "Aún no ha definido si el cliente Autoriza el tratamiento de datos.", .RequiereConfirmacion = True, .DetieneIngreso = False, .Confirmacion = "¿Desea dejarlo pendiente?", .Tab = TAB_PRINCIPAL_GENERAL_INFADICIONAL})
                    logvalidacion = True
                    'Exit Sub
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("Debe definir si el cliente autoriza tratamiento de datos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    TabSeleccionadaPrincipal = TAB_PRINCIPAL_GENERAL
                    TabSeleccionado = TAB_PRINCIPAL_GENERAL_INFADICIONAL
                    logvalidacion = True
                    Exit Sub
                End If
            End If
            'CFMA20170404
            '********************************************************************************************

            'validaciones en sql
            dcProxy.tblRespuestaValidacionesCientes.Clear()
            dcProxy.Load(dcProxy.ClientesValidacionesQuery(ListaClientesDirecciones.Where(Function(li) li.DireccionEnvio = True).First.Ciudad, ClienteSelected.IDNacionalidad, Program.Usuario, Program.HashConexion, ClienteSelected.IDComitente, ClienteSelected.strNroDocumento, ClienteSelected.strInstruccionCRCC), AddressOf TerminoValidacionesClientes, Nothing)
            logvalidacion = True
            Exit Sub
        Catch ex As Exception
            logvalidacion = True
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los Clientes",
                                 Me.ToString(), "Validaciones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarOrdenatesAsociados(ByVal pstrOrdenante As String)
        Try
            dcProxy.RespuestaOrdenantesAsociados.Clear()
            dcProxy.Load(dcProxy.ConsultarAsociacionOrdenantesQuery(pstrOrdenante, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarOrdenantesAsociados, "")
            Exit Sub
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los ordenantes asociados",
                                 Me.ToString(), "OrdenantesAsociados", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private Sub TerminoConsultarOrdenantesAsociados(ByVal lo As LoadOperation(Of OyDClientes.RespuestaOrdenantesAsociados))
        Try

            If Not lo.HasError Then
                If lo.Entities.Count > 0 Then
                    If lo.Entities.First.Exitoso = False Then
                        mostrarMensaje(lo.Entities.First.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If

                    If Not IsNothing(_ClientesOrdenantesSelected) Then
                        ClientesOrdenantesSelected.Asociados = lo.Entities.First.NroAsociados
                    End If

                End If
                IsBusy = False
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los ordenantes asociados",
                                                 Me.ToString(), "TerminoConsultarOrdenantesAsociados", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                IsBusy = False
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los ordenantes asociados",
                                 Me.ToString(), "TerminoConsultarOrdenantesAsociados", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Friend Sub buscarItem(ByVal pstrTipoItem As String, Optional ByVal pstrIdItem As String = "")
        Dim strIdItem As String = String.Empty
        Dim logConsultar As Boolean = False

        Try
            If Not Me.ClienteSelected Is Nothing Then
                Select Case pstrTipoItem
                    Case "ciudadesdoc"

                        If Not IsNothing(ClienteSelected.IDPoblacionDoc) Then
                            pstrIdItem = pstrIdItem.Trim()
                            If pstrIdItem.Equals(String.Empty) Then
                                strIdItem = Me.ClienteSelected.IDPoblacionDoc
                            Else
                                strIdItem = pstrIdItem
                            End If
                            If Not strIdItem.Equals(String.Empty) Then
                                logConsultar = True
                            End If
                            If logConsultar Then
                                mdcProxyUtilidad01.BuscadorGenericos.Clear()
                                mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarItemEspecificoQuery("ciudades", strIdItem, Program.Usuario, Program.HashConexion), AddressOf buscarGenericoCompleted, pstrTipoItem)
                            End If
                        End If
                    Case "ciudades"

                        If Not IsNothing(ClienteSelected.IDPoblacion) Then
                            pstrIdItem = pstrIdItem.Trim()
                            If pstrIdItem.Equals(String.Empty) Then
                                strIdItem = Me.ClienteSelected.IDPoblacion
                            Else
                                strIdItem = pstrIdItem
                            End If
                            If Not strIdItem.Equals(String.Empty) Then
                                logConsultar = True
                            End If
                            If logConsultar Then
                                mdcProxyUtilidad02.BuscadorGenericos.Clear()
                                mdcProxyUtilidad02.Load(mdcProxyUtilidad02.buscarItemEspecificoQuery(pstrTipoItem, strIdItem, Program.Usuario, Program.HashConexion), AddressOf buscarGenericoCompleted, pstrTipoItem)
                            End If
                        End If
                    Case "Codigos_Ciiu"

                        If Not IsNothing(ClienteSelected.codigoCiiu) Then
                            pstrIdItem = pstrIdItem.Trim()
                            If pstrIdItem.Equals(String.Empty) Then
                                strIdItem = Me.ClienteSelected.codigoCiiu
                            Else
                                strIdItem = pstrIdItem
                            End If
                            If Not strIdItem.Equals(String.Empty) Then
                                logConsultar = True
                            End If
                            If logConsultar Then
                                mdcProxyUtilidad03.BuscadorGenericos.Clear()
                                mdcProxyUtilidad03.Load(mdcProxyUtilidad03.buscarItemEspecificoQuery(pstrTipoItem, strIdItem, Program.Usuario, Program.HashConexion), AddressOf buscarGenericoCompleted, pstrTipoItem)
                            End If
                        End If
                    Case "Ciudadrep"

                        If Not IsNothing(ClienteSelected.IDCiudadReprLegal) Then
                            pstrIdItem = pstrIdItem.Trim()
                            If pstrIdItem.Equals(String.Empty) Then
                                strIdItem = Me.ClienteSelected.IDCiudadReprLegal
                            Else
                                strIdItem = pstrIdItem
                            End If
                            If Not strIdItem.Equals(String.Empty) Then
                                logConsultar = True
                            End If
                            If logConsultar Then
                                mdcProxyUtilidad04.BuscadorGenericos.Clear()
                                mdcProxyUtilidad04.Load(mdcProxyUtilidad04.buscarItemEspecificoQuery("ciudades", strIdItem, Program.Usuario, Program.HashConexion), AddressOf buscarGenericoCompleted, pstrTipoItem)
                            End If
                        End If
                    Case "Ciudadna"

                        If Not IsNothing(ClienteSelected.IdCiudadNacimiento) Then
                            pstrIdItem = pstrIdItem.Trim()
                            If pstrIdItem.Equals(String.Empty) Then
                                strIdItem = Me.ClienteSelected.IdCiudadNacimiento
                            Else
                                strIdItem = pstrIdItem
                            End If
                            If Not strIdItem.Equals(String.Empty) Then
                                logConsultar = True
                            End If
                            If logConsultar Then
                                mdcProxyUtilidad05.BuscadorGenericos.Clear()
                                mdcProxyUtilidad05.Load(mdcProxyUtilidad05.buscarItemEspecificoQuery("ciudades", strIdItem, Program.Usuario, Program.HashConexion), AddressOf buscarGenericoCompleted, pstrTipoItem)
                            End If
                        End If
                    Case "CodigoProfesion"

                        If Not IsNothing(ClienteSelected.IdProfesion) Then
                            pstrIdItem = pstrIdItem.Trim()
                            If pstrIdItem.Equals(String.Empty) Then
                                strIdItem = Me.ClienteSelected.IdProfesion
                            Else
                                strIdItem = pstrIdItem
                            End If
                            If Not strIdItem.Equals(String.Empty) Then
                                logConsultar = True
                            End If
                            If logConsultar Then
                                mdcProxyUtilidad06.BuscadorGenericos.Clear()
                                mdcProxyUtilidad06.Load(mdcProxyUtilidad06.buscarItemEspecificoQuery(pstrTipoItem, strIdItem, Program.Usuario, Program.HashConexion), AddressOf buscarGenericoCompleted, pstrTipoItem)
                            End If
                        End If

                    Case Else
                        logConsultar = False
                End Select


            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos de la ciudad", Me.ToString(), "Buscar ciudad", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    Private Sub buscarGenericoCompleted(ByVal lo As LoadOperation(Of OYDUtilidades.BuscadorGenerico))
        Dim strTipoItem As String
        Try
            If lo.UserState Is Nothing Then
                strTipoItem = ""
            Else
                strTipoItem = lo.UserState
            End If

            If lo.Entities.ToList.Count > 0 Then
                Select Case strTipoItem
                    Case "ciudadesdoc"

                        Me.CiudadesClientes.strPoblaciondoc = lo.Entities.ToList.Item(0).Nombre
                        Me.CiudadesClientes.strdepartamentoDoc = lo.Entities.ToList.Item(0).CodigoAuxiliar
                        Me.CiudadesClientes.strPaisDoc = lo.Entities.ToList.Item(0).InfoAdicional02
                        strciudaddoc = lo.Entities.ToList.Item(0).Nombre
                        strdepartamentodoc = lo.Entities.ToList.Item(0).CodigoAuxiliar
                        strpaisdoc = lo.Entities.ToList.Item(0).InfoAdicional02
                        If Not ListaClientes.Contains(ClienteSelected) Then
                            _ClienteSelected.IDDepartamentoDoc = lo.Entities.ToList.Item(0).InfoAdicional01
                            _ClienteSelected.IDPaisDoc = lo.Entities.ToList.Item(0).EtiquetaCodItem
                        End If
                    Case "ciudades"

                        Me.CiudadesClientes.strPoblacion = lo.Entities.ToList.Item(0).Nombre
                        Me.CiudadesClientes.strdepartamento = lo.Entities.ToList.Item(0).CodigoAuxiliar
                        Me.CiudadesClientes.strPais = lo.Entities.ToList.Item(0).InfoAdicional02
                        If Not ListaClientes.Contains(ClienteSelected) Then
                            _ClienteSelected.IDDepartamento = lo.Entities.ToList.Item(0).InfoAdicional01
                            _ClienteSelected.IdPais = lo.Entities.ToList.Item(0).EtiquetaCodItem
                        End If
                    Case "Codigos_Ciiu"

                        Me.CamposSeGenerales.strcodigociu = lo.Entities.ToList.Item(0).Descripcion
                        strcodigociiu = lo.Entities.ToList.Item(0).Descripcion
                    Case "Ciudadrep"

                        Me.CiudadesClientes.strciudad = lo.Entities.ToList.Item(0).Descripcion
                        strcodigociirepre = lo.Entities.ToList.Item(0).Descripcion

                    Case "Ciudadna"

                        Me.CiudadesClientes.strciudadNacimiento = lo.Entities.ToList.Item(0).Descripcion
                        strCiudadNacimiento = lo.Entities.ToList.Item(0).Descripcion
                    Case "CodigoProfesion"
                        Me.CamposSeGenerales.strProfesion = lo.Entities.ToList.Item(0).Descripcion
                        strProfesion = lo.Entities.ToList.Item(0).Descripcion
                End Select

            Else
                Select Case strTipoItem
                    Case "ciudadesdoc"
                        Me.CiudadesClientes.strPoblaciondoc = String.Empty
                        Me.CiudadesClientes.strdepartamentoDoc = String.Empty
                        Me.CiudadesClientes.strPaisDoc = String.Empty
                        strciudaddoc = String.Empty
                        strdepartamentodoc = String.Empty
                        strpaisdoc = String.Empty
                    Case "ciudades"
                        Me.CiudadesClientes.strPoblacion = String.Empty
                        Me.CiudadesClientes.strdepartamento = String.Empty
                        Me.CiudadesClientes.strPais = String.Empty
                    Case "Codigos_Ciiu"
                        Me.CamposSeGenerales.strcodigociu = String.Empty
                        strcodigociiu = String.Empty
                    Case "Ciudadrep"
                        Me.CiudadesClientes.strciudad = String.Empty
                        strcodigociirepre = String.Empty
                    Case "Ciudadna"
                        Me.CiudadesClientes.strciudadNacimiento = String.Empty
                        strCiudadNacimiento = String.Empty
                    Case "CodigoProfesion"
                        Me.CamposSeGenerales.strProfesion = String.Empty
                        strProfesion = String.Empty
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la consulta de items ("""")", Me.ToString(), "buscarGenericoCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub
    ''' <summary>
    ''' validaciones generales del modulo de clientes 
    ''' JBT20140113
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub validacionesinhabil()
        Try
            TerminoConsultainhabil = False
            If Not IsNothing(ListaClientesAccionistas) Then
                If ListaClientesAccionistas.Count > 0 Then
                    For Each li In ListaClientesAccionistas
                        ValidaClienteInhabil(li.NroDocumento, li.Nombre1 + " " + li.Nombre2 + " " + li.Apellido1 + " " + li.Apellido2, "A")
                    Next

                End If
            End If
            If Not IsNothing(ListaClientesBeneficiarios) Then
                If ListaClientesBeneficiarios.Count > 0 Then
                    For Each li In ListaClientesBeneficiarios
                        ValidaClienteInhabil(li.NroDocumento, li.Nombre1 + " " + li.Nombre2 + " " + li.Apellido1 + " " + li.Apellido2, "B")
                    Next
                End If
            End If
            If Not IsNothing(ClienteSelected.TipoPersona) Then
                If ClienteSelected.TipoPersona = 2 Then
                    If Not IsNothing(ClienteSelected.IDReprLegal) And Not IsNothing(ClienteSelected.RepresentanteLegal) Then
                        ValidaClienteInhabil(ClienteSelected.IDReprLegal, ClienteSelected.RepresentanteLegal, "R")
                    End If
                End If
            End If
            If Not IsNothing(ListaCuentasClientes) Then
                If ListaCuentasClientes.Count > 0 Then
                    For Each li In ListaCuentasClientes
                        'Santiago Vergara - Julio 08/2014 - Se añade la validación de datos nothing
                        If Not IsNothing(li.NumeroID) Then
                            ValidaClienteInhabil(li.NumeroID, li.Titular, "CB")
                        End If
                    Next
                End If
            End If
            If Not IsNothing(ListaClientesDepEconomica) Then
                If ListaClientesDepEconomica.Count > 0 Then
                    For Each li In ListaClientesDepEconomica
                        ValidaClienteInhabil(li.NroDocumento, li.Nombre1 + " " + li.Nombre2 + " " + li.Apellido1 + " " + li.Apellido2, "D")
                    Next
                End If
            End If
            If Not IsNothing(ListaClientesPersonas) Then
                If ListaClientesPersonas.Count > 0 Then
                    For Each li In ListaClientesPersonas
                        ValidaClienteInhabil(li.NroDocumento, li.Nombre1 + " " + li.Nombre2 + " " + li.Apellido1 + " " + li.Apellido2, "P")
                    Next
                End If
            End If
            ValidaClienteInhabil(ClienteSelected.strNroDocumento, ClienteSelected.Nombre, "C")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en los Clientes Inhabilitados",
                                 Me.ToString(), "validacionesinhabil", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    ''' <summary>
    ''' validaciones generales del modulo de clientes 
    ''' JBT20130213
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ValidaClienteInhabil(ByVal strnrodocumento As String, ByVal strnombre As String, ByVal _strmodulos As String)
        Try
            'Santiago Vergara - Julio 08/2014 - Se añade la validación de datos nothing
            If (IsNothing(strnrodocumento) OrElse strnrodocumento.Equals("")) And (IsNothing(strnombre) OrElse strnombre.Equals("")) Then Exit Sub
            If strnrodocumento <> "" Then
                mdcProxyUtilidad01.ClienteInhabilitados.Clear()
                mdcProxyUtilidad01.Load(mdcProxyUtilidad01.ValidarClienteInhabilitadoQuery(strnrodocumento, "", Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarClienteInhabilitado, "CIstrNroDocumento" + "‡" + strnrodocumento + "‡" + strnombre + "‡" + _strmodulos + "‡" + (ListaDisparosAsync.Count + 1).ToString)
                If Not ListaDisparosAsync.ContainsKey(ListaDisparosAsync.Count + 1) Then
                    ListaDisparosAsync.Add(ListaDisparosAsync.Count + 1, strnrodocumento)
                Else
                    ListaDisparosAsync(ListaDisparosAsync.Count + 1) = strnrodocumento
                End If
            End If
            If strnombre <> "" Then
                mdcProxyUtilidad02.ClienteInhabilitados.Clear()
                mdcProxyUtilidad02.Load(mdcProxyUtilidad02.ValidarClienteInhabilitadoNombreQuery("", strnombre, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarClienteInhabilitado, "CINombre" + "‡" + strnrodocumento + "‡" + strnombre + "‡" + _strmodulos + "‡" + (ListaDisparosAsync.Count + 1).ToString)
                If Not ListaDisparosAsync.ContainsKey(ListaDisparosAsync.Count + 1) Then
                    ListaDisparosAsync.Add(ListaDisparosAsync.Count + 1, strnombre)
                Else
                    ListaDisparosAsync(ListaDisparosAsync.Count + 1) = strnombre
                End If
                If PORCENTAJE_CERCANIA_SEGUNDO_MENSAJE > 0 Then
                    _strmodulos = _strmodulos + "SM"
                    mdcProxyUtilidad03.ClienteInhabilitados.Clear()
                    mdcProxyUtilidad03.Load(mdcProxyUtilidad03.ValidarClienteInhabilitadoSegundoMensajeQuery("", strnombre, True, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarClienteInhabilitado, "CINombre" + "‡" + strnrodocumento + "‡" + strnombre + "‡" + _strmodulos + "‡" + (ListaDisparosAsync.Count + 1).ToString)
                    ListaDisparosAsync.Add(ListaDisparosAsync.Count + 1, strnombre)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            ListaDisparosAsync.Clear()
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los Clientes Inhabilitados",
                                 Me.ToString(), "ValidaClienteInhabil", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ValidarEdicion()
        Try

            If Not IsNothing(dcProxy3.Clientes) Then
                dcProxy3.Clientes.Clear()
            End If

            dcProxy3.Load(dcProxy3.ClientesConsultarPorAprobarQuery(ClienteSelected.IDComitente, ClienteSelected.Nombre, ClienteSelected.strNroDocumento, ClienteSelected.TipoIdentificacion, ClienteSelected.Clasificacion, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarEdicion, "ValidarEdicion")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar validar la edición",
                                 Me.ToString(), "ValidarEdicion", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Sub Validarborrado()
        Try

            If Not IsNothing(dcProxy3.Clientes) Then
                dcProxy3.Clientes.Clear()
            End If
            If Not IsNothing(ClienteSelected) Then
                dcProxy3.Load(dcProxy3.ClientesConsultarPorAprobarQuery(ClienteSelected.IDComitente, ClienteSelected.Nombre, ClienteSelected.strNroDocumento, ClienteSelected.TipoIdentificacion, ClienteSelected.Clasificacion, Program.Usuario, Program.HashConexion), AddressOf TerminoValidarborrado, "Validarborrado")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cambiar validar la edición",
                                 Me.ToString(), "Validarborrado", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Sub InsertaLogHistoriaIR()
        If ClienteSelected.Suitability <> Historialir.Suitability Then
            Dim NewClientesLogHistoriaIR As New OyDClientes.ClientesLOGHistoriaIR
            NewClientesLogHistoriaIR.ID = ClienteSelected.IDComitente
            NewClientesLogHistoriaIR.Comentario = "Modificacion Del valor del campo Suitability (" & Historialir.Suitability & ") se cambio por (" & ClienteSelected.Suitability & ")"
            NewClientesLogHistoriaIR.Usuario = Program.Usuario
            ListaClientesLOGHistoriaI.Add(NewClientesLogHistoriaIR)
            ClientesLOGHistoriaISelected = NewClientesLogHistoriaIR
        End If

        If ClienteSelected.ObjetivoInversion <> Historialir.ObjetivoInversion Then
            Dim NewClientesLogHistoriaIR As New OyDClientes.ClientesLOGHistoriaIR
            NewClientesLogHistoriaIR.ID = ClienteSelected.IDComitente
            NewClientesLogHistoriaIR.Comentario = "Modificacion Del valor del campo ObjetivoInversion (" & Historialir.ObjetivoInversion & ") se cambio por (" & ClienteSelected.ObjetivoInversion & ")"
            NewClientesLogHistoriaIR.Usuario = Program.Usuario
            ListaClientesLOGHistoriaI.Add(NewClientesLogHistoriaIR)
            ClientesLOGHistoriaISelected = NewClientesLogHistoriaIR
        End If

        If ClienteSelected.HorizonteInversion <> Historialir.HorizonteInversion Then
            Dim NewClientesLogHistoriaIR As New OyDClientes.ClientesLOGHistoriaIR
            NewClientesLogHistoriaIR.ID = ClienteSelected.IDComitente
            NewClientesLogHistoriaIR.Comentario = "Modificacion Del valor del campo HorizonteInversion (" & Historialir.HorizonteInversion & ") se cambio por (" & ClienteSelected.HorizonteInversion & ")"
            NewClientesLogHistoriaIR.Usuario = Program.Usuario
            ListaClientesLOGHistoriaI.Add(NewClientesLogHistoriaIR)
            ClientesLOGHistoriaISelected = NewClientesLogHistoriaIR
        End If
        If ClienteSelected.EdadCliente <> Historialir.EdadCliente Then
            Dim NewClientesLogHistoriaIR As New OyDClientes.ClientesLOGHistoriaIR
            NewClientesLogHistoriaIR.ID = ClienteSelected.IDComitente
            NewClientesLogHistoriaIR.Comentario = "Modificacion Del valor del campo EdadCliente (" & Historialir.EdadCliente & ") se cambio por (" & ClienteSelected.EdadCliente & ")"
            NewClientesLogHistoriaIR.Usuario = Program.Usuario
            ListaClientesLOGHistoriaI.Add(NewClientesLogHistoriaIR)
            ClientesLOGHistoriaISelected = NewClientesLogHistoriaIR
        End If
        If ClienteSelected.ConocimientoExperiencia <> Historialir.ConocimientoExperiencia Then
            Dim NewClientesLogHistoriaIR As New OyDClientes.ClientesLOGHistoriaIR
            NewClientesLogHistoriaIR.ID = ClienteSelected.IDComitente
            NewClientesLogHistoriaIR.Comentario = "Modificacion Del valor del campo ConocimientoExperiencia (" & Historialir.ConocimientoExperiencia & ") se cambio por (" & ClienteSelected.ConocimientoExperiencia & ")"
            NewClientesLogHistoriaIR.Usuario = Program.Usuario
            ListaClientesLOGHistoriaI.Add(NewClientesLogHistoriaIR)
            ClientesLOGHistoriaISelected = NewClientesLogHistoriaIR
        End If
        If ClienteSelected.ClasificacionInversionista <> Historialir.ClasificacionInversionista Then
            Dim NewClientesLogHistoriaIR As New OyDClientes.ClientesLOGHistoriaIR
            NewClientesLogHistoriaIR.ID = ClienteSelected.IDComitente
            NewClientesLogHistoriaIR.Comentario = "Modificacion Del valor del campo ClasificacionInversionista (" & Historialir.ClasificacionInversionista & ") se cambio por (" & ClienteSelected.ClasificacionInversionista & ")"
            NewClientesLogHistoriaIR.Usuario = Program.Usuario
            ListaClientesLOGHistoriaI.Add(NewClientesLogHistoriaIR)
            ClientesLOGHistoriaISelected = NewClientesLogHistoriaIR
        End If
        If cambioconocimiento = True Then
            Dim desmarcados As String = String.Empty, marcados As String = String.Empty
            Dim lista = ListaClientesConocimientoEspecificoclase.Where(Function(i) i.Conocimiento <> i.ConocimientoOriginal).ToList
            For Each li In lista
                If li.Conocimiento = False Then
                    desmarcados = desmarcados + li.DescripcionConocimiento + ","
                Else
                    marcados = marcados + li.DescripcionConocimiento + ","
                End If
            Next
            Dim NewClientesLogHistoriaIR As New OyDClientes.ClientesLOGHistoriaIR
            NewClientesLogHistoriaIR.ID = ClienteSelected.IDComitente
            NewClientesLogHistoriaIR.Comentario = "Modificacion De los campos que habian marcados (" & desmarcados & ") y se cambio por los que quedaron marcados (" & marcados & ")"
            NewClientesLogHistoriaIR.Usuario = Program.Usuario
            ListaClientesLOGHistoriaI.Add(NewClientesLogHistoriaIR)
            ClientesLOGHistoriaISelected = NewClientesLogHistoriaIR


            cambioconocimiento = False
        End If

    End Sub
    Sub EnviarBus()
        If IsNothing(_ClienteSelected) Then
            Exit Sub
        End If
        If ClienteSelected.TipoVinculacion.Equals("O") Then
            A2Utilidades.Mensajes.mostrarMensaje("El cliente no puede ser ordenante,no puede ser creado en BVC.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If
        If ClienteSelected.Estado <> Nothing Or ClientesSeBoolean.visible = True Then
            A2Utilidades.Mensajes.mostrarMensaje("El cliente no puede ser creado en BVC,esta pendiente de aprobacion.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Exit Sub
        End If
        Dim objValidacion = clsExpresiones.ValidarCaracteresEnCadena(ClienteSelected.strNroDocumento, clsExpresiones.TipoExpresion.LetrasNumerosUnicamente)
        If Not IsNothing(objValidacion) Then
            If objValidacion.TextoValido = False Then
                A2Utilidades.Mensajes.mostrarMensaje("El número de documento solo puede contener números y letras.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                logvalidacion = True
                Exit Sub
            End If
        End If
        dcProxy.ClientesEnviarBus(ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion, AddressOf TerminoSubmit, "insert")


    End Sub
    Sub Preclientes()
        Try
            Preclientesf = New Preclientes()
            AddHandler Preclientesf.Closed, AddressOf CerroVentana
            Program.Modal_OwnerMainWindowsPrincipal(Preclientesf)
            Preclientesf.ShowDialog()
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en preclientes",
                                  Me.ToString(), "Preclientes", Application.Current.ToString(), Program.Maquina, ex.InnerException)
        End Try
    End Sub
    Sub ConsultaDescripcionD()
        If ClientesFichaSelected.TipoReferencia = "DE" Or ClientesFichaSelected.TipoReferencia = "T" Or ClientesFichaSelected.TipoReferencia = "A" Or ClientesFichaSelected.TipoReferencia = "E" Or ClientesFichaSelected.TipoReferencia = "O" Or ClientesFichaSelected.TipoReferencia = "R" Or ClientesFichaSelected.TipoReferencia = "C" Or ClientesFichaSelected.TipoReferencia = "P" Then
            Consultadirecciones = New ConsultaDescripcionDirecciones(ListaClientesDirecciones.ToList, ClientesFichaSelected.TipoReferencia)
            AddHandler Consultadirecciones.Closed, AddressOf CerroVentanaDirec
            Program.Modal_OwnerMainWindowsPrincipal(Consultadirecciones)
            Consultadirecciones.ShowDialog()
        End If
    End Sub
    Private Sub CerroVentana()
        Try
            If Preclientesf.DialogResult = True Then
                dcProxy.Load(dcProxy.PasarPreclientesQuery(Preclientesf.PreClientesSelected.IDPreCliente, Program.Usuario, Program.HashConexion), AddressOf TerminoPasarpreclientes, Nothing)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema cerrando la ventana",
                                      Me.ToString(), "CerroVentana", Application.Current.ToString(), Program.Maquina, ex.InnerException)
        End Try
    End Sub
    Private Sub CerroVentanaDirec()
        If Consultadirecciones.DialogResult = True Then
            Try
                If Not IsNothing(Consultadirecciones.DescripcionDic.SelectedItem) Then
                    ClientesFichaSelected.Descripcion = Consultadirecciones.DescripcionDic.SelectedItem.Direccion
                    ClientesFichaSelected.IDPoblacion = Consultadirecciones.DescripcionDic.SelectedItem.Ciudad
                    ClientesFichaSelected.NombreCiudad = Consultadirecciones.DescripcionDic.SelectedItem.Nombre
                End If
            Catch ex As Exception
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al terminar de cerrar la ventana",
                                 Me.ToString(), "CerroVentanaDirec", Application.Current.ToString(), Program.Maquina, ex)
            End Try
        End If
    End Sub
    Private Sub TerminoPasarpreclientes(ByVal lo As LoadOperation(Of OyDClientes.PreClientes))
        If Not lo.HasError Then

            IsBusy = False
            If dcProxy.PreClientes.ToList.Count = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("No se encontro ningún registro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                If UCase(lo.Entities.First.strTipoCliente) = "N" Then
                    ClienteSelected.TipoPersona = 1
                    ClienteSelected.TipoVinculacion = "A"
                    ClienteSelected.Sexo = lo.Entities.First.Sexo
                    ClienteSelected.Estrato = lo.Entities.First.strEstrato
                Else
                    ClienteSelected.TipoPersona = 2
                    ClienteSelected.TipoVinculacion = lo.Entities.First.strTipoVinculacion
                    ClienteSelected.Sexo = Nothing
                End If

                If lo.Entities.First.strEstado = "B" Then
                    ClienteSelected.EstadoCliente = lo.Entities.First.strEstado
                Else
                    ClienteSelected.EstadoCliente = "A"
                End If

                ClienteSelected.strNroRim = lo.Entities.First.IDPreCliente
                TipoIdentificacionEdicion = lo.Entities.First.TipoIdentificacion
                ClienteSelected.Contribuyente = lo.Entities.First.logContribuye
                ClienteSelected.Egresos = lo.Entities.First.curEgreso
                ClienteSelected.RetFuente = lo.Entities.First.logReteFuente
                If Not IsNothing(lo.Entities.First.dtmNacimiento) Then
                    If lo.Entities.First.dtmNacimiento.Value.Year > Now.Date.Year Then
                        ClienteSelected.Nacimiento = Nothing
                    Else
                        ClienteSelected.Nacimiento = lo.Entities.First.dtmNacimiento
                    End If
                Else
                    ClienteSelected.Nacimiento = lo.Entities.First.dtmNacimiento
                End If

                ClienteSelected.Nombre = lo.Entities.First.Nombres
                ClienteSelected.Nombre1 = lo.Entities.First.PrimerNombre
                ClienteSelected.Nombre2 = lo.Entities.First.SegundoNombre
                ClienteSelected.Apellido1 = lo.Entities.First.PrimerApellido
                ClienteSelected.Apellido2 = lo.Entities.First.SegundoApellido
                Select Case lo.Entities.First.strTipoDireccionEnvio
                    Case "R" 'residencia
                        ClienteSelected.DireccionEnvio = lo.Entities.First.strDireccionResidencia
                    Case "O" 'oficina
                        ClienteSelected.DireccionEnvio = lo.Entities.First.strDireccionEmpresa
                    Case "U" 'otra
                        ClienteSelected.DireccionEnvio = lo.Entities.First.strOtraDireccion
                End Select
                ClienteSelected.EMail = lo.Entities.First.strEmail
                ClienteSelected.EstadoCivil = lo.Entities.First.strEstadoCivil
                If ClienteSelected.TipoPersona = 2 Then
                    ClienteSelected.TipoReprLegal = IIf(lo.Entities.First.strTipoRepresentanteLegal Is Nothing, Nothing, lo.Entities.First.strTipoRepresentanteLegal)
                    ClienteSelected.RepresentanteLegal = lo.Entities.First.strRepresentanteLegal
                    ClienteSelected.CargoReprLegal = lo.Entities.First.strCargoRepresentanteLegal
                    ClienteSelected.TelefonoReprLegal = lo.Entities.First.strTelefonoRepresentanteLegal
                    ClienteSelected.DireccionReprLegal = lo.Entities.First.strDireccionRepresentanteLegal
                    ClienteSelected.IDCiudadReprLegal = IIf(lo.Entities.First.lngIDCiudadRepresentanteLegal Is Nothing, Nothing, lo.Entities.First.lngIDCiudadRepresentanteLegal)
                End If
                ClienteSelected.strNroDocumento = lo.Entities.First.NroDocumento
                If DateAdd("yyyy", -18, Now) < ClienteSelected.Nacimiento And ClienteSelected.TipoPersona = 1 Then
                    ClienteSelected.MenorEdad = True
                End If
                ClienteSelected.IngresoMensual = lo.Entities.First.curIngresoMensual
                ClienteSelected.Activos = lo.Entities.First.curActivos
                ClienteSelected.Patrimonio = lo.Entities.First.curPatrimonio
                ClienteSelected.IDNacionalidad = lo.Entities.First.IDNacionalidad
                'ClienteSelected.DetalleOIngresos = lo.Entities.First.curOtrosIngresos
                ClienteSelected.codigoCiiu = lo.Entities.First.strCodigoCiiu
                ClienteSelected.Declarante = lo.Entities.First.logDeclarante
                ClienteSelected.AutoRetenedor = lo.Entities.First.logAutoRetenedor
                ClienteSelected.ExentoGMF = lo.Entities.First.logExentoGMF
                ClienteSelected.IdCiudadNacimiento = lo.Entities.First.lngIDCiudadNacimiento
                ClienteSelected.FechaExpedicionDoc = lo.Entities.First.dtmFechaExpedicionDoc
                ClienteSelected.Direccion = lo.Entities.First.strDireccionResidencia
                ClienteSelected.IDPoblacion = lo.Entities.First.lngIDPoblacion
                ClienteSelected.IDDepartamento = lo.Entities.First.lngIDDeptoGeneral
                ClienteSelected.IdPais = IIf(lo.Entities.First.lngIDPaisGeneral Is Nothing, Nothing, lo.Entities.First.lngIDPaisGeneral)
                ClienteSelected.IDPoblacionDoc = lo.Entities.First.lngIDPoblacionExpedicionDoc
                ClienteSelected.IDDepartamentoDoc = lo.Entities.First.lngIDDeptoExpedicion
                ClienteSelected.IDPaisDoc = lo.Entities.First.lngIDPaisExpedicion
                If ClienteSelected.IDPaisDoc Is Nothing Or ClienteSelected.IDPaisDoc = 0 Then
                    Me.CiudadesClientes.strPoblaciondoc = String.Empty
                    Me.CiudadesClientes.strdepartamentoDoc = String.Empty
                    Me.CiudadesClientes.strPaisDoc = String.Empty
                End If
                ClienteSelected.Telefono1 = IIf(lo.Entities.First.intTelefonoResidencia Is Nothing, Nothing, lo.Entities.First.intTelefonoResidencia)
                ClienteSelected.Telefono2 = lo.Entities.First.strTelefono2
                ClienteSelected.Fax2 = lo.Entities.First.strCelular
                'nuevo detalle direccion
                If (Not IsNothing(lo.Entities.First.strDireccionResidencia) Or lo.Entities.First.strDireccionResidencia <> String.Empty) Or (Not IsNothing(lo.Entities.First.intTelefonoResidencia) Or lo.Entities.First.intTelefonoResidencia <> 0) Or (Not IsNothing(lo.Entities.First.PoblacionGeneral) Or lo.Entities.First.PoblacionGeneral <> String.Empty) Or (Not IsNothing(lo.Entities.First.strFaxResidencia) Or lo.Entities.First.strFaxResidencia <> String.Empty) Then

                    Dim NewClientesDireccdioneSelected As New OyDClientes.ClientesDireccione

                    NewClientesDireccdioneSelected.IDComitente = ClienteSelected.IDComitente
                    NewClientesDireccdioneSelected.Usuario = Program.Usuario
                    NewClientesDireccdioneSelected.Activo = True
                    If lo.Entities.First.strTipoDireccionEnvio = "R" Then
                        NewClientesDireccdioneSelected.DireccionEnvio = True
                    Else
                        NewClientesDireccdioneSelected.DireccionEnvio = False
                    End If

                    NewClientesDireccdioneSelected.intClave_PorAprobar = -1
                    NewClientesDireccdioneSelected.Tipo = "C"
                    NewClientesDireccdioneSelected.Direccion = lo.Entities.First.strDireccionResidencia
                    NewClientesDireccdioneSelected.Telefono = lo.Entities.First.intTelefonoResidencia
                    NewClientesDireccdioneSelected.Ciudad = lo.Entities.First.lngIDPoblacion
                    NewClientesDireccdioneSelected.Nombre = lo.Entities.First.PoblacionGeneral
                    NewClientesDireccdioneSelected.Fax = lo.Entities.First.strFaxResidencia
                    ListaClientesDirecciones = dcProxy.ClientesDirecciones
                    ListaClientesDirecciones.Add(NewClientesDireccdioneSelected)
                    ClientesDireccionesSelected = NewClientesDireccdioneSelected
                    MyBase.CambioItem("ClientesDireccionesSelected")
                    MyBase.CambioItem("ListaClientesDirecciones")
                    ClientesSeBoolean.Read = False
                End If
                If (Not IsNothing(lo.Entities.First.strDireccionEmpresa) Or lo.Entities.First.strDireccionEmpresa <> String.Empty) Or (Not IsNothing(lo.Entities.First.intTelefonoEmpresa) Or lo.Entities.First.intTelefonoEmpresa <> 0) Or (Not IsNothing(lo.Entities.First.lngIDCiudadEmpresa) Or lo.Entities.First.lngIDCiudadEmpresa <> 0) Or (Not IsNothing(lo.Entities.First.NombreCiudadEmpresa) Or lo.Entities.First.NombreCiudadEmpresa <> String.Empty) Then

                    Dim NewClientesDireccdioneSelected As New OyDClientes.ClientesDireccione

                    NewClientesDireccdioneSelected.IDComitente = ClienteSelected.IDComitente
                    NewClientesDireccdioneSelected.Usuario = Program.Usuario
                    NewClientesDireccdioneSelected.Activo = True
                    If lo.Entities.First.strTipoDireccionEnvio = "O" Then
                        NewClientesDireccdioneSelected.DireccionEnvio = True
                    Else
                        NewClientesDireccdioneSelected.DireccionEnvio = False
                    End If

                    NewClientesDireccdioneSelected.intClave_PorAprobar = -1
                    NewClientesDireccdioneSelected.Tipo = "O"
                    NewClientesDireccdioneSelected.Direccion = lo.Entities.First.strDireccionEmpresa
                    NewClientesDireccdioneSelected.Telefono = lo.Entities.First.intTelefonoEmpresa
                    NewClientesDireccdioneSelected.Ciudad = lo.Entities.First.lngIDCiudadEmpresa
                    NewClientesDireccdioneSelected.Nombre = lo.Entities.First.NombreCiudadEmpresa
                    ListaClientesDirecciones = dcProxy.ClientesDirecciones
                    ListaClientesDirecciones.Add(NewClientesDireccdioneSelected)
                    ClientesDireccionesSelected = NewClientesDireccdioneSelected
                    MyBase.CambioItem("ClientesDireccionesSelected")
                    MyBase.CambioItem("ListaClientesDirecciones")
                    ClientesSeBoolean.Read = False
                End If
                If (Not IsNothing(lo.Entities.First.strOtraDireccion) Or lo.Entities.First.strOtraDireccion <> String.Empty) Or (Not IsNothing(lo.Entities.First.intOtroTelefono) Or lo.Entities.First.intOtroTelefono <> 0) Or (Not IsNothing(lo.Entities.First.lngIDCiudadOtraDireccion) Or lo.Entities.First.lngIDCiudadOtraDireccion <> 0) Or (Not IsNothing(lo.Entities.First.NombreCiudadOtraDireccion) Or lo.Entities.First.NombreCiudadOtraDireccion <> String.Empty) Then

                    Dim NewClientesDireccdioneSelected As New OyDClientes.ClientesDireccione

                    NewClientesDireccdioneSelected.IDComitente = ClienteSelected.IDComitente
                    NewClientesDireccdioneSelected.Usuario = Program.Usuario
                    NewClientesDireccdioneSelected.Activo = True
                    If lo.Entities.First.strTipoDireccionEnvio = "U" Then
                        NewClientesDireccdioneSelected.DireccionEnvio = True
                    Else
                        NewClientesDireccdioneSelected.DireccionEnvio = False
                    End If

                    NewClientesDireccdioneSelected.intClave_PorAprobar = -1
                    NewClientesDireccdioneSelected.Tipo = "T"
                    NewClientesDireccdioneSelected.Direccion = lo.Entities.First.strOtraDireccion
                    NewClientesDireccdioneSelected.Telefono = lo.Entities.First.intOtroTelefono
                    NewClientesDireccdioneSelected.Ciudad = lo.Entities.First.lngIDCiudadOtraDireccion
                    NewClientesDireccdioneSelected.Nombre = lo.Entities.First.NombreCiudadOtraDireccion
                    ListaClientesDirecciones = dcProxy.ClientesDirecciones
                    ListaClientesDirecciones.Add(NewClientesDireccdioneSelected)
                    ClientesDireccionesSelected = NewClientesDireccdioneSelected
                    MyBase.CambioItem("ClientesDireccionesSelected")
                    MyBase.CambioItem("ListaClientesDirecciones")
                    ClientesSeBoolean.Read = False
                End If
                'nueva detalle cuenta bancaria
                If Not IsNothing(lo.Entities.First.lngIDBanco1) And lo.Entities.First.lngIDBanco1 = 0 Then

                    Dim NewCuentasClientesSelected As New OyDClientes.CuentasCliente

                    NewCuentasClientesSelected.IDComitente = ClienteSelected.IDComitente
                    NewCuentasClientesSelected.Usuario = Program.Usuario
                    NewCuentasClientesSelected.Activo = False
                    NewCuentasClientesSelected.logTitular = False
                    NewCuentasClientesSelected.Habilita = True
                    NewCuentasClientesSelected.logexcluirInteresDividendo = False
                    NewCuentasClientesSelected.IDBanco = lo.Entities.First.lngIDBanco1
                    NewCuentasClientesSelected.NombreBanco = lo.Entities.First.NombreBanco1
                    NewCuentasClientesSelected.NombreSucursal = lo.Entities.First.strNombreSucursal1
                    NewCuentasClientesSelected.Cuenta = lo.Entities.First.strCuenta1
                    NewCuentasClientesSelected.TipoCuenta = IIf(lo.Entities.First.strTipoCuenta1 = "A", "Ahorro", "Cuenta Corriente")
                    ListaCuentasClientes = dcProxy.CuentasClientes
                    ListaCuentasClientes.Add(NewCuentasClientesSelected)
                    CuentasClientesSelected = NewCuentasClientesSelected
                    MyBase.CambioItem("CuentasClientesSelected")
                    MyBase.CambioItem("ListaCuentasClientes")
                End If
                If Not IsNothing(lo.Entities.First.lngIDBanco2) And lo.Entities.First.lngIDBanco2 = 0 Then

                    Dim NewCuentasClientesSelected As New OyDClientes.CuentasCliente

                    NewCuentasClientesSelected.IDComitente = ClienteSelected.IDComitente
                    NewCuentasClientesSelected.Usuario = Program.Usuario
                    NewCuentasClientesSelected.Activo = False
                    NewCuentasClientesSelected.logTitular = False
                    NewCuentasClientesSelected.Habilita = True
                    NewCuentasClientesSelected.logexcluirInteresDividendo = False
                    NewCuentasClientesSelected.IDBanco = lo.Entities.First.lngIDBanco2
                    NewCuentasClientesSelected.NombreBanco = lo.Entities.First.NombreBanco2
                    NewCuentasClientesSelected.NombreSucursal = lo.Entities.First.strNombreSucursal2
                    NewCuentasClientesSelected.Cuenta = lo.Entities.First.strCuenta2
                    NewCuentasClientesSelected.TipoCuenta = IIf(lo.Entities.First.strTipoCuenta2 = "A", "Ahorro", "Cuenta Corriente")
                    ListaCuentasClientes = dcProxy.CuentasClientes
                    ListaCuentasClientes.Add(NewCuentasClientesSelected)
                    CuentasClientesSelected = NewCuentasClientesSelected
                    MyBase.CambioItem("CuentasClientesSelected")
                    MyBase.CambioItem("ListaCuentasClientes")
                End If
                'nuevo detalle beneficiarios
                If Not IsNothing(lo.Entities.First.strCotitularTipoIdentificacion1) Then

                    Dim NewClientesBeneficiarioSelected As New OyDClientes.ClientesBeneficiarios

                    NewClientesBeneficiarioSelected.IDComitente = ClienteSelected.IDComitente
                    NewClientesBeneficiarioSelected.Usuario = Program.Usuario
                    NewClientesBeneficiarioSelected.Activo = False
                    NewClientesBeneficiarioSelected.Habilita = True
                    NewClientesBeneficiarioSelected.TipoID = lo.Entities.First.strCotitularTipoIdentificacion1
                    NewClientesBeneficiarioSelected.NroDocumento = lo.Entities.First.lngCotitularNroDocumento1
                    NewClientesBeneficiarioSelected.Nombre1 = lo.Entities.First.strCotitularPrimerNombre1
                    NewClientesBeneficiarioSelected.Nombre2 = lo.Entities.First.strCotitularSegundoNombre1
                    NewClientesBeneficiarioSelected.Apellido1 = lo.Entities.First.strCotitularPrimerApellido1
                    NewClientesBeneficiarioSelected.Apellido2 = lo.Entities.First.strCotitularSegundoApellido1
                    ListaClientesBeneficiarios = dcProxy.ClientesBeneficiarios
                    ListaClientesBeneficiarios.Add(NewClientesBeneficiarioSelected)
                    ClientesBeneficiarioSelected = NewClientesBeneficiarioSelected
                    MyBase.CambioItem("ClientesBeneficiarioSelected")
                    MyBase.CambioItem("ListaClientesBeneficiarios")
                End If

                If Not IsNothing(lo.Entities.First.strCotitularTipoIdentificacion2) Then

                    Dim NewClientesBeneficiarioSelected As New OyDClientes.ClientesBeneficiarios

                    NewClientesBeneficiarioSelected.IDComitente = ClienteSelected.IDComitente
                    NewClientesBeneficiarioSelected.Usuario = Program.Usuario
                    NewClientesBeneficiarioSelected.Activo = False
                    NewClientesBeneficiarioSelected.Habilita = True
                    NewClientesBeneficiarioSelected.TipoID = lo.Entities.First.strCotitularTipoIdentificacion2
                    NewClientesBeneficiarioSelected.NroDocumento = lo.Entities.First.lngCotitularNroDocumento2
                    NewClientesBeneficiarioSelected.Nombre1 = lo.Entities.First.strCotitularPrimerNombre2
                    NewClientesBeneficiarioSelected.Nombre2 = lo.Entities.First.strCotitularSegundoNombre2
                    NewClientesBeneficiarioSelected.Apellido1 = lo.Entities.First.strCotitularPrimerApellido2
                    NewClientesBeneficiarioSelected.Apellido2 = lo.Entities.First.strCotitularSegundoApellido2
                    ListaClientesBeneficiarios = dcProxy.ClientesBeneficiarios
                    ListaClientesBeneficiarios.Add(NewClientesBeneficiarioSelected)
                    ClientesBeneficiarioSelected = NewClientesBeneficiarioSelected
                    MyBase.CambioItem("ClientesBeneficiarioSelected")
                    MyBase.CambioItem("ListaClientesBeneficiarios")
                End If
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Clientes",
                                             Me.ToString(), "TerminoPasarpreclientes", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
            IsBusy = False
        End If


    End Sub
    Private Sub TerminoSubmit(ByVal lo As InvokeOperation)
        Try
            IsBusy = False
            If lo.HasError Then
                'TODO: Pendiente garantizar que Userstate no venga vacío
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                               Me.ToString(), "TerminoSubmit" & lo.UserState.ToString(), Application.Current.ToString(), Program.Maquina, lo.Error)
                'A2Utilidades.Mensajes.mostrarMensaje("No se pude enviar el cliente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                If lo.UserState = "insert" Then
                    dcProxy.RejectChanges()
                End If
                lo.MarkErrorAsHandled()
                Exit Try
            End If
            If lo.UserState = "insert" Then
                'cancelar()
                A2Utilidades.Mensajes.mostrarMensaje("El cliente fue enviado con éxito.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                             Me.ToString(), "TerminoSubmit", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            If miTab = 1 Then
                TabSeleccionadaFinanciero = miTab
                TabSeleccionado = 0
            Else
                TabSeleccionadaFinanciero = miTab
            End If

        End If
    End Sub
    Public Sub llenarDiccionario()
        DicCamposTab.Add("Contribuyente", 1)
        DicCamposTab.Add("IngresoMensual", 1)
        DicCamposTab.Add("Nombre", 0)
        DicCamposTab.Add("IDSucCliente", 0)
        DicCamposTab.Add("RetFuente", 1)
        DicCamposTab.Add("IDSector", 0)
        DicCamposTab.Add("TipoIdentificacion", 0)
        DicCamposTab.Add("IDSubSector", 0)
        DicCamposTab.Add("IDGrupo", 0)
        DicCamposTab.Add("IDSubGrupo", 0)
        DicCamposTab.Add("OrigenIngresos", 1)
    End Sub
    Public Sub ConsultarCustodia()
        IsBusyCustodias = True
        ListaCustodias.Clear()
        dcProxy.ClientesCustodias.Clear()
        dcProxy.Load(dcProxy.TraerClientesCustodiasQuery(_ClienteSelected.IDComitente, FechaCorte, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesCustodias, Nothing)
    End Sub
    Public Sub ConsultarVencimientos()
        ListaVencimientos.Clear()
        dcProxy.ClientesVencimientos.Clear()
        dcProxy.Load(dcProxy.TraerClientesVencimientosQuery(ClienteSelected.IDComitente, FechaInicial, FechaFinal, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesVencimientos, Nothing)
    End Sub
    Public Sub ConsultarLiqxCumplir()
        ListaLiqxCumplir.Clear()
        dcProxy.ClientesLiqxCumplirs.Clear()
        dcProxy.Load(dcProxy.TraerClientesLiqxCumplirQuery(ClienteSelected.IDComitente, FechaCorte, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesLiqxCumplir, Nothing)
    End Sub
    Public Sub ConsultarRepos()
        ListaRepos.Clear()
        dcProxy.ClientesRepos.Clear()
        dcProxy.Load(dcProxy.TraerClientesReposQuery(ClienteSelected.IDComitente, FechaCorte, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesRepos, Nothing)
    End Sub
    Public Sub ConsultarFondos()

        IsBusyFondos = True
        dcProxy.ClientesFondosTotales.Clear()
        dcProxy.Load(dcProxy.TraerSaldoClientesFondosResumidoQuery(ClienteSelected.IDComitente, FechaCorteFondos, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesFondosTotales, Nothing)
    End Sub
    Public Sub validarDocumento(Optional ByVal pstrUserState As String = "")
        dcProxy.ClientesSucursales.Clear()
        dcProxy.Load(dcProxy.TraerClientesDocumentoQuery(ClienteSelected.strNroDocumento, ClienteSelected.TipoIdentificacion, ClienteSelected.IDSucCliente, ClienteSelected.IDClientes, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerRespuestaDocum, pstrUserState)
    End Sub
    Public Async Function TraerSiguienteSuc(Optional ByVal plogEjecutarPropertyChange As Boolean = True) As Task(Of Boolean)
        Dim logRetorno As Boolean = False

        Try
            Dim objRet As LoadOperation(Of OyDClientes.Cliente)
            dcProxy6 = New ClientesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            objRet = Await dcProxy6.Load(dcProxy6.ConsultaClientesSucursalSyncQuery(ClienteSelected.strNroDocumento, Program.Usuario, Program.HashConexion)).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de los sucursales.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los sucursales.", Me.ToString(), "TraerSiguienteSuc", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If plogEjecutarPropertyChange = False Then
                        logValidarDocumentosIDSucursal = False
                    End If

                    ClienteSelected.IDSucCliente = objRet.Entities.ToList.Item(0).IDSucCliente

                    If plogEjecutarPropertyChange = False Then
                        logValidarDocumentosIDSucursal = True
                    End If

                    logRetorno = True
                End If
            Else
                SiguenteSuc = Nothing
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los sucursales.", Me.ToString(), "TraerSiguienteSuc", Program.TituloSistema, Program.Maquina, ex)
        End Try

        Return logRetorno
    End Function

    Public Async Function TraerComitente() As Task
        Dim objRet As LoadOperation(Of OyDClientes.ClientesOrdenante)
        Try
            Dim ordenante As String = String.Empty
            If Not IsNothing(ClientesOrdenantesSelected) Then
                If Not IsNothing(ListaClientesOrdenantes.Where(Function(o) IIf(IsNothing(o.IDOrdenante), String.Empty, o.IDOrdenante) = ClientesOrdenantesSelected.IDOrdenante).FirstOrDefault()) Then
                    ordenante = ListaClientesOrdenantes.Where(Function(o) IIf(IsNothing(o.IDOrdenante), String.Empty, o.IDOrdenante) = ClientesOrdenantesSelected.IDOrdenante).FirstOrDefault().IDOrdenante
                End If
            End If

            dcProxy7 = New ClientesDomainContext(New System.Uri(Program.RutaServicioNegocio))
            objRet = Await dcProxy7.Load(dcProxy7.Traer_ClientesOrdenanteSyncQuery(0, Nothing, ordenante, Program.Usuario, Program.HashConexion)).AsTask

            If objRet.AllEntities.Count = 0 Then
                Exit Function
            End If

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de los ordenantes.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los ordenantes.", Me.ToString(), "TraerComitente", Program.TituloSistema, Program.Maquina, objRet.Error)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    For Each Item In ListaClientesOrdenantes
                        '(From item In _ListaClienteRelacionados Select item.IDComitente_Relacionado, item.Lider Where Lider = True And IDComitente_Relacionado <> _ClienteEncabezadoSelected.IdEncabezado).Count

                        If dcProxy7.ClientesOrdenantes.Where(Function(i) LTrim(i.IDOrdenante) = Item.IDOrdenante).Count > 0 Then 'DEMC20190321
                            Item.Asociados = dcProxy7.ClientesOrdenantes.Where(Function(i) LTrim(i.IDOrdenante) = Item.IDOrdenante).FirstOrDefault.Asociados
                        End If
                    Next
                End If
            Else
                ListaClientesOrdenantes.Clear()
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un problema al ejecutar la consulta de los ordenantes.", Me.ToString(), "TraerComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Function



    Public Sub validareplicaciones()
        If ClienteSelected.ReplicarSafyrFondos And String.IsNullOrEmpty(ClienteSelected.CodReceptorSafyrFondos) Then
            ClienteSelected.CodReceptorSafyrFondos = strCodigoReceptorLider
        End If
        If ClienteSelected.ReplicarSafyrPortafolios And String.IsNullOrEmpty(ClienteSelected.CodReceptorSafyrPortafolio) Then
            ClienteSelected.CodReceptorSafyrPortafolio = strCodigoReceptorLider
        End If
        If ClienteSelected.ReplicarSafyrClientes And String.IsNullOrEmpty(ClienteSelected.CodReceptorSafyrClientes) Then
            ClienteSelected.CodReceptorSafyrClientes = strCodigoReceptorLider
        End If
        If ClienteSelected.ReplicarMercansoft And String.IsNullOrEmpty(ClienteSelected.CodReceptorMercansoft) Then
            ClienteSelected.CodReceptorMercansoft = strCodigoReceptorLider
        End If
        If intCuenta = 0 And _ClienteSelected.ReplicarMercansoft Then
            A2Utilidades.Mensajes.mostrarMensaje("El cliente será replicado a mercansoft pero recuerde matricular el código DATATEC, Ir a códigos otros sistemas.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        End If
        If logReplicaAgora And ClienteSelected.ReplicarSantanderAgora Then
            A2Utilidades.Mensajes.mostrarMensaje("El cliente será replicado a Agora.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        End If
    End Sub
    Public Sub terminarediciones()
        If dcProxy.ClientesBeneficiarios.HasChanges Then
            Dim editableObject As IEditableObject = TryCast(ClientesBeneficiarioSelected, IEditableObject)
            If Not IsNothing(editableObject) Then
                editableObject.EndEdit()
            End If
        End If
        If dcProxy.ClientesDirecciones.HasChanges Then
            Dim editableObject As IEditableObject = TryCast(ClientesDireccionesSelected, IEditableObject)
            If Not IsNothing(editableObject) Then
                editableObject.EndEdit()
            End If
        End If
        If dcProxy.ClientesReceptores.HasChanges Then
            Dim editableObject As IEditableObject = TryCast(ClientesReceptoreSelected, IEditableObject)
            If Not IsNothing(editableObject) Then
                editableObject.EndEdit()
            End If
        End If
        If dcProxy.CuentasClientes.HasChanges Then
            Dim editableObject As IEditableObject = TryCast(CuentasClientesSelected, IEditableObject)
            If Not IsNothing(editableObject) Then
                editableObject.EndEdit()
            End If
        End If
        If dcProxy.ClientesOrdenantes.HasChanges Then
            Dim editableObject As IEditableObject = TryCast(ClientesOrdenantesSelected, IEditableObject)
            If Not IsNothing(editableObject) Then
                editableObject.EndEdit()
            End If
        End If
    End Sub
    Public Overrides Sub CancelarBuscar()
        validafiltro = True
        cb.IDComitente = ""
        cb.Nombre = ""
        cb.strNroDocumento = ""
        cb.TipoIdentificacion = ""
        cb.Clasificacion = ""
        MyBase.CancelarBuscar()
    End Sub


    ''' <summary>
    ''' Este metodo permite calcular los datos financieros dependiendo de que la firma los tenga habilitados mediante un parametro
    ''' JBT20130213
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Calculadatos()

        Try

            If logvalorparametro Then
                '    If ClienteSelected.Activos > 0 And ClienteSelected.Patrimonio > 0 Then
                '        ClienteSelected.Pasivos = ClienteSelected.Activos - ClienteSelected.Patrimonio
                '    End If

                '    If ClienteSelected.Activos > 0 And ClienteSelected.Pasivos > 0 Then
                '        ClienteSelected.Patrimonio = ClienteSelected.Activos - ClienteSelected.Pasivos
                '    End If

                If ClienteSelected.IngresoMensual > 0 And ClienteSelected.Egresos > 0 Then
                    ClienteSelected.Utilidades = ClienteSelected.IngresoMensual - ClienteSelected.Egresos
                End If

                If ClienteSelected.IngresoMensual > 0 And ClienteSelected.Utilidades > 0 Then
                    ClienteSelected.Egresos = ClienteSelected.IngresoMensual - ClienteSelected.Utilidades
                End If


            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los calculos",
                                         Me.ToString(), "Calculadatos", Application.Current.ToString(), Program.Maquina, ex.InnerException)
            '????
        End Try

    End Sub

    Public Sub CalcularFormula(ByVal Tipo As String)

        Try

            If logvalorparametro Then
                If Tipo = "Activos" Then
                    If Not (ClienteSelected.Activos Is Nothing) And Not (ClienteSelected.Pasivos Is Nothing) And (ClienteSelected.Patrimonio Is Nothing) Then
                        ClienteSelected.Patrimonio = ClienteSelected.Activos - ClienteSelected.Pasivos
                    ElseIf Not (ClienteSelected.Activos Is Nothing) And (ClienteSelected.Pasivos Is Nothing) And Not (ClienteSelected.Patrimonio Is Nothing) Then
                        ClienteSelected.Pasivos = ClienteSelected.Activos - ClienteSelected.Patrimonio
                    Else

                        ClienteSelected.Patrimonio = ClienteSelected.Activos - ClienteSelected.Pasivos

                    End If
                ElseIf Tipo = "Pasivos" Then
                    If Not (ClienteSelected.Pasivos Is Nothing) And Not (ClienteSelected.Activos Is Nothing) And (ClienteSelected.Patrimonio Is Nothing) Then
                        ClienteSelected.Patrimonio = ClienteSelected.Activos - ClienteSelected.Pasivos
                    ElseIf Not (ClienteSelected.Pasivos Is Nothing) And (ClienteSelected.Activos Is Nothing) And Not (ClienteSelected.Patrimonio Is Nothing) Then
                        ClienteSelected.Activos = ClienteSelected.Pasivos + ClienteSelected.Patrimonio
                    Else
                        If Not (ClienteSelected.Pasivos Is Nothing) Then
                            ClienteSelected.Patrimonio = ClienteSelected.Activos - ClienteSelected.Pasivos
                        End If
                    End If

                ElseIf Tipo = "Patrimonio" Then
                    If Not (ClienteSelected.Pasivos Is Nothing) And Not (ClienteSelected.Patrimonio Is Nothing) And (ClienteSelected.Activos Is Nothing) Then
                        ClienteSelected.Activos = ClienteSelected.Pasivos + ClienteSelected.Patrimonio
                    ElseIf Not (ClienteSelected.Patrimonio Is Nothing) And (ClienteSelected.Pasivos Is Nothing) And Not (ClienteSelected.Activos Is Nothing) Then
                        ClienteSelected.Pasivos = ClienteSelected.Activos - ClienteSelected.Patrimonio
                    Else
                        If Not (ClienteSelected.Patrimonio Is Nothing) Then
                            ClienteSelected.Pasivos = ClienteSelected.Activos - ClienteSelected.Patrimonio
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los calculos",
                                         Me.ToString(), "CalculadatosActivos", Application.Current.ToString(), Program.Maquina, ex.InnerException)
            '????
        End Try

    End Sub

    'Public Sub CalculadatosActivos()

    '    Try

    '        If logvalorparametro Then
    '            If ClienteSelected.Pasivos > 0 And ClienteSelected.Patrimonio > 0 Then
    '                ClienteSelected.Activo = ClienteSelected.Pasivos + ClienteSelected.Patrimonio
    '            End If
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los calculos", _
    '                                     Me.ToString(), "CalculadatosActivos", Application.Current.ToString(), Program.Maquina, ex.InnerException)
    '        '????
    '    End Try

    'End Sub

    'Public Sub CalculadatosPasivos()

    '    Try

    '        If logvalorparametro Then
    '            If ClienteSelected.Activos > 0 And ClienteSelected.Patrimonio > 0 Then
    '                ClienteSelected.Pasivos = ClienteSelected.Activos - ClienteSelected.Patrimonio
    '            End If
    '        End If

    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los calculos", _
    '                                     Me.ToString(), "CalculadatosPasivos", Application.Current.ToString(), Program.Maquina, ex.InnerException)
    '        '????
    '    End Try

    'End Sub

    'Public Sub CalculadatosPatrimonio()

    '    Try
    '        If logvalorparametro Then
    '            If ClienteSelected.Pasivos > 0 And ClienteSelected.Activo > 0 And ClienteSelected.Patrimonio Is Nothing Then
    '                ClienteSelected.Patrimonio = ClienteSelected.Activos - ClienteSelected.Pasivos
    '            End If
    '        End If


    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los calculos", _
    '                                     Me.ToString(), "CalculadatosPatrimonio", Application.Current.ToString(), Program.Maquina, ex.InnerException)
    '        '????
    '    End Try

    'End Sub
    ''' <summary>
    ''' Este metodo permite calcular el valor en pesos mediante el salario minimo y el numero de salarios
    ''' JBT20130213
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub calculosmmlv()
        Try
            If CamposSeGenerales.SMMLV <> String.Empty And ClienteSelected.NroSalarios <> 0 Then
                CamposSeGenerales.valorenpesos = CDbl(CamposSeGenerales.SMMLV) * CDbl(ClienteSelected.NroSalarios)
            Else
                CamposSeGenerales.valorenpesos = String.Empty
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los calculos",
                                         Me.ToString(), "Calculadatos", Application.Current.ToString(), Program.Maquina, ex.InnerException)
            '????
        End Try
    End Sub

    ''' <summary>
    ''' Este metodo Consulta los detalles que son editables
    ''' JBT20130213
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Descripción:    Se agregó el llamado al proxi con la entidad ClientesProductos.
    ''' Responsable:    Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:          29 de Septiembre/2014
    ''' Pruebas CB:     Jorge Peña (Alcuadrado S.A.) - 29 de Septiembre/2014
    ''' </history>
    Public Sub ConsultaDetalle(Optional ByVal plogEditandoRegistro As Boolean = False)
        Try
            IsBusy = True
            If _ClienteSelected.Por_Aprobar Is Nothing Then
                dcProxy.CuentasClientes.Clear()
                dcProxy.Load(dcProxy.Traer_CuentasClientesQuery(0, _ClienteSelected.IDComitente, plogEditandoRegistro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasclientes, Nothing)
                dcProxy.TipoClients.Clear()
                dcProxy.Load(dcProxy.TipoClienteFiltrarQuery(0, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesTipoCliente, Nothing)
                dcProxy.ClientesReceptores.Clear()
                dcProxy.Load(dcProxy.Traer_ClientesReceptoreQuery(0, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesReceptores, Nothing)
                dcProxy.ClientesOrdenantes.Clear()
                dcProxy.Load(dcProxy.Traer_ClientesOrdenanteQuery(0, _ClienteSelected.IDComitente, Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesOrdenantes, Nothing)
                dcProxy.ClientesBeneficiarios.Clear()
                dcProxy.Load(dcProxy.Traer_ClientesBeneficiariosQuery(0, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesBeneficiarios, Nothing)
                dcProxy.ClientesAficiones.Clear()
                dcProxy.Load(dcProxy.Traer_ClientesAficionesQuery(0, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesAficiones, Nothing)
                dcProxy.ClientesDeportes.Clear()
                dcProxy.Load(dcProxy.Traer_ClientesDeportesQuery(0, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDeportes, Nothing)
                dcProxy.ClientesAccionistas.Clear()
                dcProxy.Load(dcProxy.Traer_ClientesAccionistasQuery(0, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesAccionistas, Nothing)
                dcProxy.ClientesFichas.Clear()
                dcProxy.Load(dcProxy.Traer_ClientesFichaClienteQuery(0, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesFicha, Nothing)
                dcProxy.ClientesPersonasDepEconomicas.Clear()
                dcProxy.Load(dcProxy.Traer_ClientesDepEconomicaQuery(0, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDepEconomica, Nothing)
                dcProxy.ClientesDirecciones.Clear()
                dcProxy.Load(dcProxy.Traer_ClientesDireccionesQuery(0, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDirecciones, Nothing)
                dcProxy.ClientesDocumentosRequeridos.Clear()
                dcProxy.Load(dcProxy.Traer_ClientesDocumentosRequeridosQuery(_ClienteSelected.IDComitente, _ClienteSelected.TipoPersona, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDocumentosRequeridos, Nothing)
                If configuraloghir Then
                    dcProxy.ClientesLOGHistoriaIRs.Clear()
                    dcProxy.Load(dcProxy.Traer_ClientesLOGHistoriaIQuery(_ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesLOGHistoriaI, Nothing)
                End If
                If configuraconocimiento Then
                    dcProxy.ClientesConocimientoEspecificos.Clear()
                    ListaClientesConocimientoEspecificoclase.Clear()
                    Contadorconocimientos = 0
                    dcProxy.Load(dcProxy.Traer_ClientesConocimientoEspecificoQuery(0, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesConocimientoEspecifico, Nothing)
                End If
                dcProxy.ClientesPersonasParaConfirmars.Clear()
                dcProxy.Load(dcProxy.Traer_ClientesPersonasQuery(0, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesPersonas, Nothing)
                dcProxy.ClientesProductos.Clear()
                dcProxy.Load(dcProxy.ConsultarClientesProductosQuery(ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarClientesProductos, Nothing)
                'NAOC20141118 - Fatca
                '**********************************************************************
                dcProxy.ClientesPaisesFATCAs.Clear()
                dcProxy.Load(dcProxy.ConsultarClientesPaisesFATCAQuery(0, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesPaisesFatca, Nothing)
                '**********************************************************************
            Else
                dcProxy.CuentasClientes.Clear()
                dcProxy.Load(dcProxy.Traer_CuentasClientesQuery(1, _ClienteSelected.IDComitente, plogEditandoRegistro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerCuentasclientes, Nothing)
                dcProxy.TipoClients.Clear()
                dcProxy.Load(dcProxy.TipoClienteFiltrarQuery(1, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesTipoCliente, Nothing)
                dcProxy.ClientesReceptores.Clear()
                dcProxy.Load(dcProxy.Traer_ClientesReceptoreQuery(1, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesReceptores, Nothing)
                dcProxy.ClientesOrdenantes.Clear()
                dcProxy.Load(dcProxy.Traer_ClientesOrdenanteQuery(1, _ClienteSelected.IDComitente, Nothing, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesOrdenantes, Nothing)
                dcProxy.ClientesBeneficiarios.Clear()
                dcProxy.Load(dcProxy.Traer_ClientesBeneficiariosQuery(1, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesBeneficiarios, Nothing)
                dcProxy.ClientesAficiones.Clear()
                dcProxy.Load(dcProxy.Traer_ClientesAficionesQuery(1, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesAficiones, Nothing)
                dcProxy.ClientesDeportes.Clear()
                dcProxy.Load(dcProxy.Traer_ClientesDeportesQuery(1, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDeportes, Nothing)
                dcProxy.ClientesAccionistas.Clear()
                dcProxy.Load(dcProxy.Traer_ClientesAccionistasQuery(1, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesAccionistas, Nothing)
                dcProxy.ClientesFichas.Clear()
                dcProxy.Load(dcProxy.Traer_ClientesFichaClienteQuery(1, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesFicha, Nothing)
                dcProxy.ClientesPersonasDepEconomicas.Clear()
                dcProxy.Load(dcProxy.Traer_ClientesDepEconomicaQuery(1, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDepEconomica, Nothing)
                dcProxy.ClientesDirecciones.Clear()
                dcProxy.Load(dcProxy.Traer_ClientesDireccionesQuery(1, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDirecciones, Nothing)
                dcProxy.ClientesDocumentosRequeridos.Clear()
                dcProxy.Load(dcProxy.Traer_ClientesDocumentosRequeridosQuery(_ClienteSelected.IDComitente, _ClienteSelected.TipoPersona, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesDocumentosRequeridos, Nothing)
                If configuraloghir Then
                    dcProxy.ClientesLOGHistoriaIRs.Clear()
                    dcProxy.Load(dcProxy.Traer_ClientesLOGHistoriaIQuery(_ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesLOGHistoriaI, Nothing)
                End If
                If configuraconocimiento Then
                    dcProxy.ClientesConocimientoEspecificos.Clear()
                    ListaClientesConocimientoEspecificoclase.Clear()
                    Contadorconocimientos = 0
                    dcProxy.Load(dcProxy.Traer_ClientesConocimientoEspecificoQuery(1, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesConocimientoEspecifico, Nothing)
                End If
                dcProxy.ClientesPersonasParaConfirmars.Clear()
                dcProxy.Load(dcProxy.Traer_ClientesPersonasQuery(1, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesPersonas, Nothing)
                dcProxy.ClientesProductos.Clear()
                dcProxy.Load(dcProxy.ConsultarClientesProductosQuery(ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarClientesProductos, Nothing)
                'NAOC20141118 - Fatca
                '**********************************************************************
                dcProxy.ClientesPaisesFATCAs.Clear()
                dcProxy.Load(dcProxy.ConsultarClientesPaisesFATCAQuery(1, _ClienteSelected.IDComitente, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClientesPaisesFatca, Nothing)
                '**********************************************************************
            End If
            dcProxy2.CodigosOtrosSistemas.Clear()
            dcProxy2.Load(dcProxy2.CodigosOtrosSistemasConsultarQuery(_ClienteSelected.IDComitente, "VALIDARCLIENTEDAT", Program.Usuario, Program.HashConexion), AddressOf TerminotraerCodigoOtrossistemas, Nothing)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los Registros",
                             Me.ToString(), "Calculadatos", Application.Current.ToString(), Program.Maquina, ex.InnerException)
        End Try
    End Sub
    Public Sub ValidarDigitoVerificacion(ByVal pstrNit As String)
        Try
            If Not IsNothing(_ClienteSelected) Then
                'Solo aplica para cuando el tipo de persona sea juridica y el tipo de identifiacación sea NIT
                If Not String.IsNullOrEmpty(pstrNit) And _ClienteSelected.TipoPersona = CONST_TIPODEPERSONA_JURIDICA And _ClienteSelected.TipoIdentificacion = CONST_TIPODEIDENTIFICACION_NIT Then
                    If logValidarDigitoVerificacion Then
                        Dim strNitValidar As String = String.Empty

                        strNitValidar = pstrNit.Substring(0, Len(pstrNit) - 1)

                        dcProxy.CalcularDigitoVerificacion(strNitValidar, Program.Usuario, Program.HashConexion, AddressOf TerminoCalcularDigitoVerificacion, pstrNit)
                    Else
                        logPasoValidacionDigito = True
                        strDigitoPermitido = String.Empty
                    End If
                Else
                    logPasoValidacionDigito = True
                    strDigitoPermitido = String.Empty
                End If
            Else
                logPasoValidacionDigito = True
                strDigitoPermitido = String.Empty
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el digito de verficicación",
                             Me.ToString(), "ValidarDigitoVerificacion", Application.Current.ToString(), Program.Maquina, ex.InnerException)
        End Try
    End Sub
    Public Sub movimientosclientes(ByVal codigoerror As String)
        strMensajeinactivacion = ""
        Select Case codigoerror
            Case "1"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene Saldo en OyD" + vbCrLf + "¿Desea bloquearlo?"
            Case "2"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene Custodias activas en OyD" + vbCrLf + "¿Desea bloquearlo?"
            Case "3"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene Saldo y Custodias en OyD" + vbCrLf + "¿Desea bloquearlo?"
            Case "4"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene Encargos en Safyr" + vbCrLf + "¿Desea bloquearlo?"
            Case "5"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene Saldo en OyD y Encargos en Safyr" + vbCrLf + "¿Desea bloquearlo?"
            Case "6"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene Custodias activas en OyD y Encargos en Safyr Saldo en OyD" + vbCrLf + "¿Desea bloquearlo?"
            Case "7"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene Saldo y Custodias activas en OyD, y Encargos en Safyr" + vbCrLf + "¿Desea bloquearlo?"
            Case "8"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene Divisas activas en OyD" + vbCrLf + "¿Desea bloquearlo?"
            Case "9"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene Saldo y Divisas activas en OyD" + vbCrLf + "¿Desea bloquearlo?"
            Case "10"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene Custodias y Divisas activas en OyD" + vbCrLf + "¿Desea bloquearlo?"
            Case "11"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene Saldo,Custodias y Divisas activas en OyD" + vbCrLf + "¿Desea bloquearlo?"
            Case "12"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene Divisas activas en OyD, Y Encargos en Safyr" + vbCrLf + "¿Desea bloquearlo?"
            Case "13"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene Saldo y Divisas activas en OyD, Y Encargos en Safyr" + vbCrLf + "¿Desea bloquearlo?"
            Case "14"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene Divisas y Custodias activas en OyD, Y Encargos en Safyr" + vbCrLf + "¿Desea bloquearlo?"
            Case "15"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene Saldo,Custodias y Divisas activas en OyD, Y Encargos en Safyr" + vbCrLf + "¿Desea bloquearlo?"
            'CAMBIO SM20180529 validaciones de fondos de inversión
            Case "16"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene saldo en fondos de inversión " + vbCrLf + "¿Desea bloquearlo?"
            Case "17"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene saldo y Saldo en fondos de inversión " + vbCrLf + "¿Desea bloquearlo?"
            Case "18"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene custodias en OyD y saldo en fondos de inversión " + vbCrLf + "¿Desea bloquearlo?"
            Case "19"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene saldo, custodias y saldo en fondos de inversión " + vbCrLf + "¿Desea bloquearlo?"
            Case "20"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene encargos en Safyr y saldo en fondos de inversión " + vbCrLf + "¿Desea bloquearlo?"
            Case "21"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene saldo, encargos en Safyr y saldo en fondos de inversión " + vbCrLf + "¿Desea bloquearlo?"
            Case "22"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene custodias en OyD, encargos en Safyr y saldo en fondos de inversión " + vbCrLf + "¿Desea bloquearlo?"
            Case "23"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene saldo, custodias en OyD, encargos en Safyr y saldo en fondos de inversión " + vbCrLf + "¿Desea bloquearlo?"
            Case "24"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene divisas activas en OyD y saldo en fondos de inversión " + vbCrLf + "¿Desea bloquearlo?"
            Case "25"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene saldo, divisas activas en OyD y saldo en fondos de inversión " + vbCrLf + "¿Desea bloquearlo?"
            Case "26"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene custodias en OyD, divisas activas en OyD y saldo en fondos de inversión " + vbCrLf + "¿Desea bloquearlo?"
            Case "28"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene encargos en Safyr, divisas activas en OyD y saldo en fondos de inversión " + vbCrLf + "¿Desea bloquearlo?"
            Case "30"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene custodias en OyD, encargos en Safyr, divisas activas en OyD y saldo en fondos de inversión " + vbCrLf + "¿Desea bloquearlo?"
            Case "31"
                strMensajeinactivacion = "El cliente no puede inactivarse por que tiene saldos, custodias en OyD, encargos en Safyr, divisas activas en OyD y saldo en fondos de inversión " + vbCrLf + "¿Desea bloquearlo?"
            Case Else
                A2Utilidades.Mensajes.mostrarMensaje(codigoerror, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
        End Select
        'C1.Silverlight.C1MessageBox.Show(strMensajeinactivacion, Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question,Program.Usuario, Program.HashConexion, AddressOf TerminoinactivoBloqueo)
        mostrarMensajePregunta(strMensajeinactivacion,
                               Program.TituloSistema,
                               "BLOQUEARRECURSO",
                               AddressOf TerminoinactivoBloqueo, False)
    End Sub
    Public Sub recuperadescripcionperfil(ByVal strdescripcion As String)
        strdescripcionperfil = strdescripcion
    End Sub

#Region "CargarCombos"

    ''' <summary>
    ''' Método para consultar los combos especificos 
    ''' </summary>
    ''' <remarks>SLB20131125</remarks>
    Private Sub CargarCombosViewModel()
        Try
            objProxy.Load(objProxy.cargarCombosEspecificosQuery("clientes", Program.Usuario, Program.HashConexion), AddressOf TerminoCargarCombos, "")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los combos especificos en ViewModel de Tesorería",
                                 Me.ToString(), "CargarCombosViewModel", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método encargado de recibir la lista de las Combos Especificos de PagosDeceval.
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SLB20131125</remarks>
    Private Sub TerminoCargarCombos(ByVal lo As LoadOperation(Of OYDUtilidades.ItemCombo))
        Try
            IsBusy = False
            If Not lo.HasError Then
                ListaCombosEspecificos = objProxy.ItemCombos.ToList()

                dcProxy2.Load(dcProxy2.ClasificacionesConsultarQuery(0, Nothing, "C", False, False, 0, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificaciones, Nothing)
            Else
                'JFSB 20171008 Se agrega propiedad para habilitar los botones de menú
                HabilitarMenu = True
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de combos",
                                                 Me.ToString(), "TerminoCargarCombos", Application.Current.ToString(), Program.Maquina, lo.Error)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de combos",
                                             Me.ToString(), "TerminoCargarCombos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerClasificaciones(ByVal lo As LoadOperation(Of Clasificacion))
        Try
            If Not lo.HasError Then
                listaclasificacion = dcProxy2.Clasificacions

                'SLB20131125 Manejo de parametros.
                'If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                '    objTipoId = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOID")
                '    parametroins = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("PARAMETROSINST")
                If Not IsNothing(ListaCombosEspecificos) Then
                    objTipoId = ListaCombosEspecificos.Where(Function(obj) obj.Categoria = "TIPOID").ToList
                    parametroins = ListaCombosEspecificos.Where(Function(obj) obj.Categoria = "PARAMETROSINST").ToList
                    For Each li In parametroins
                        Select Case li.ID
                            Case "SB"
                                InstalacioSelected.ServidorBus = li.Descripcion
                            Case "BB"
                                InstalacioSelected.BaseDatosBus = li.Descripcion
                            Case "OB"
                                InstalacioSelected.OwnerBus = li.Descripcion
                            Case "PO"
                                InstalacioSelected.IdPoblacion = li.Descripcion
                            Case "CCE"
                                InstalacioSelected.ClientesCedula = li.Descripcion
                            Case "CAU"
                                InstalacioSelected.ClientesAutomatico = li.Descripcion
                            Case "CAG"
                                InstalacioSelected.ClientesAgrupados = li.Descripcion
                            Case "CB"
                                InstalacioSelected.CuentasBancarias = li.Descripcion
                            Case "RL"
                                InstalacioSelected.RepresentanteLegal = li.Descripcion
                        End Select
                    Next
                    If Not String.IsNullOrEmpty(InstalacioSelected.ServidorBus) And Not String.IsNullOrEmpty(InstalacioSelected.BaseDatosBus) And Not String.IsNullOrEmpty(InstalacioSelected.OwnerBus) Then
                        ClientesSeVisibility.TienebusVisible = Visibility.Visible
                    End If
                End If
                If Not IsNothing(_ClienteSelected) Then
                    If IsNothing(listasubSector) Then
                        listasubSector = listaclasificacion.Where(Function(di) di.EsSector = False And di.IDPerteneceA = IIf(IsNothing(_ClienteSelected.IDSector), 0, _ClienteSelected.IDSector) And di.AplicaA.Equals("C")).ToList
                        MyBase.CambioItem("listasubSector")
                    End If
                    If IsNothing(listasubGrupo) Then
                        listasubGrupo = listaclasificacion.Where(Function(di) di.EsGrupo = False And di.IDPerteneceA = IIf(IsNothing(_ClienteSelected.IDGrupo), 0, _ClienteSelected.IDGrupo) And di.AplicaA.Equals("C")).ToList
                        MyBase.CambioItem("listasubGrupo")
                    End If
                End If
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Clasificaciones",
                                                 Me.ToString(), "TerminoTraerClasificacion", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                IsBusy = False
                'JFSB 20171008 Se agrega propiedad para habilitar los botones de menú
                HabilitarMenu = True
            End If
        Catch ex As Exception
            IsBusy = False
            'JFSB 20171008 Se agrega propiedad para habilitar los botones de menú
            HabilitarMenu = True
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer las clasificaciones",
             Me.ToString(), "TerminoTraerClasificaciones", Application.Current.ToString(), Program.Maquina, ex)
        Finally
            logTerminoCargarClasificaciones = True
        End Try
    End Sub

#End Region
    Sub asignarCodigoCIIU(pobjItem As BuscadorGenerico)
        ClienteSelected.codigoCiiu = pobjItem.IdItem
        ClienteSelected.ActividadEconomica = pobjItem.Descripcion
        If configuraconocimiento Then CamposSeGenerales.strcodigociu = pobjItem.IdItem Else CamposSeGenerales.strcodigociu = pobjItem.Descripcion
    End Sub
    ''' <summary>
    ''' Funcion para verificar email correcto
    ''' JBT20140201
    ''' </summary>
    ''' <remarks></remarks>
    Public Function IsValidmail(emailaddress As String) As Boolean
        Try
            'Dim match As Match = regex.Match(emailaddress, "^[A-Z0-9._%+-]+@(?:[A-Z0-9-]+.)+[A-Z]{2,4}$")
            'If match.Success Then
            '    Return True
            'Else
            '    Return False
            'End If
            'Return Regex.IsMatch(emailaddress, "^[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z_+])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9}$")
            Return Regex.IsMatch(emailaddress, expresionemail)
        Catch generatedExceptionName As FormatException
            Return False
        End Try
    End Function
    ''' <summary>
    ''' Funcion para verificar si un string existe en un vector
    ''' JBT20140304
    ''' </summary>
    ''' <remarks></remarks>
    Public Function DescripcionValida(ByVal vector As String, ByVal separador As String, ByVal parametro As String) As Boolean
        Try
            Dim objeto = vector.Split(separador)
            For i = 0 To objeto.Length - 1
                If parametro = objeto(i) Then
                    Return True
                    Exit Function
                End If
            Next
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Sub ValidarHabilitarFatca()
        Try
            If FATCA_CAMPOSADICIONALES Then
                ClientesSeVisibility.FatcaCliente = Visibility.Collapsed
                ClientesSeVisibility.TabFatca = Visibility.Visible

                If Not IsNothing(_ClienteSelected) Then
                    If _ClienteSelected.TipoPersona = 2 Then
                        ClientesSeVisibility.TabFatcaRepresentanteLegal = Visibility.Visible
                    Else
                        ClientesSeVisibility.TabFatcaRepresentanteLegal = Visibility.Collapsed
                    End If
                Else
                    ClientesSeVisibility.TabFatcaRepresentanteLegal = Visibility.Collapsed
                End If
            Else
                ClientesSeVisibility.FatcaCliente = Visibility.Visible
                ClientesSeVisibility.TabFatca = Visibility.Collapsed
                ClientesSeVisibility.TabFatcaRepresentanteLegal = Visibility.Collapsed
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los Registros",
                             Me.ToString(), "Calculadatos", Application.Current.ToString(), Program.Maquina, ex.InnerException)
        End Try
    End Sub

    Private Function PantallasParametrizacionTipoString(ByVal pstrCampo As String, ByVal pstrColumna As String) As String
        Dim strDato As String = String.Empty

        Try
            If pstrColumna = "strValorPorDefecto" Then
                If Not IsNothing(ListaPantallasParametrizacion) Then
                    If ListaPantallasParametrizacion.Where(Function(Li) Li.strPantalla = "CLIENTES" And Li.strCampo = pstrCampo).Count() > 0 Then
                        strDato = ListaPantallasParametrizacion.Where(Function(i) i.strPantalla = "CLIENTES" And i.strCampo = pstrCampo).FirstOrDefault.strValorPorDefecto
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "ValidarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (strDato)
    End Function

    Private Function PantallasParametrizacionTipoBoolean(ByVal pstrCampo As String, ByVal pstrColumna As String) As Boolean
        Dim logDato As Boolean = False

        Try
            If pstrColumna = "logEsObligatorio" Then
                If Not IsNothing(ListaPantallasParametrizacion) Then
                    If ListaPantallasParametrizacion.Where(Function(Li) Li.strPantalla = "CLIENTES" And Li.strCampo = pstrCampo).Count() > 0 Then
                        logDato = ListaPantallasParametrizacion.Where(Function(i) i.strPantalla = "CLIENTES" And i.strCampo = pstrCampo).FirstOrDefault.logEsObligatorio
                    End If
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar los datos ingresados.", Me.ToString(), "ValidarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return (logDato)
    End Function

#End Region
#Region "Metodos Sincrónicos"

    'Santiago Vergara - Octubre 22/2013 - Metodo que llama el serrvicio para validar que no se repita el tipo de producto para el mismo numero de documento
    'Private Async Function ValidarClienteTipoProducto() As Task(Of Boolean)
    '    Dim logValidacionTipoProducto As Boolean
    '    Try
    '        logValidacionTipoProducto = False
    '        Dim objRet As InvokeOperation(Of Integer)

    '        objRet = Await dcProxy.ValidarClienteTipoProductoSync(ClienteSelected.strNroDocumento, ClienteSelected.TipoProducto, ClienteSelected.IDComitente, Program.Usuario).AsTask

    '        If objRet.HasError = False Then
    '            If objRet.Value > 0 Then
    '                'Obtiene la descripción del tipo de producto
    '                If IsNothing(objParametros) Then
    '                    If CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey("TIPOPRODUCTO") Then
    '                        objParametros = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOPRODUCTO")
    '                    End If
    '                End If

    '                Dim strDescripcionTipoProducto As String = String.Empty

    '                If objParametros.Where(Function(i) i.ID = ClienteSelected.TipoProducto).Count > 0 Then
    '                    strDescripcionTipoProducto = objParametros.Where(Function(i) i.ID = ClienteSelected.TipoProducto).First.Descripcion
    '                End If

    '                mostrarMensaje("Ya existe " & objRet.Value.ToString & " con el mismo tipo de producto, para este cliente " & " Nro. Documento " & ClienteSelected.strNroDocumento & " Tipo producto: " & strDescripcionTipoProducto, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    '                logValidacionTipoProducto = True
    '                IsBusy = False
    '            End If
    '        Else
    '            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el tipo de producto.", _
    '                             Me.ToString(), "TerminoValidarTipoProducto", Application.Current.ToString(), Program.Maquina, objRet.Error)
    '            IsBusy = False
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el tipo de producto.", _
    '                             Me.ToString(), "TerminoValidarTipoProducto", Application.Current.ToString(), Program.Maquina, ex)
    '        IsBusy = False
    '    End Try

    '    Return logValidacionTipoProducto

    'End Function

#End Region
#Region "Tablas Hijas"
#Region "CuentasClientes"

    Private _EditandoReceptores As Boolean = False
    Public Property EditandoReceptores As Boolean
        Get
            Return _EditandoReceptores
        End Get
        Set(ByVal value As Boolean)
            _EditandoReceptores = value
            MyBase.CambioItem("EditandoReceptores")
        End Set
    End Property

    'JFSB 20171008 Se agrega propiedad para habilitar los botones de menú
    Private _HabilitarMenu As Boolean = False
    Public Property HabilitarMenu() As Boolean
        Get
            Return _HabilitarMenu
        End Get
        Set(ByVal value As Boolean)
            _HabilitarMenu = value
            MyBase.CambioItem("HabilitarMenu")
        End Set
    End Property

    Private _ListaCuentasClientes As EntitySet(Of OyDClientes.CuentasCliente)
    Public Property ListaCuentasClientes() As EntitySet(Of OyDClientes.CuentasCliente)
        Get
            Return _ListaCuentasClientes
        End Get
        Set(ByVal value As EntitySet(Of OyDClientes.CuentasCliente))
            _ListaCuentasClientes = value
            MyBase.CambioItem("ListaCuentasClientes")
        End Set
    End Property
    Private WithEvents _CuentasClientesSelected As OyDClientes.CuentasCliente
    Public Property CuentasClientesSelected() As OyDClientes.CuentasCliente
        Get
            Return _CuentasClientesSelected
        End Get
        Set(ByVal value As OyDClientes.CuentasCliente)

            If Not value Is Nothing Then
                _CuentasClientesSelected = value
                Cuentasanterior.logTitular = value.logTitular
                Cuentasanterior.nombreSucursal = value.NombreSucursal
                Cuentasanterior.nombreBanco = value.NombreBanco
                Cuentasanterior.cuenta = value.Cuenta
                Cuentasanterior.observacion = value.Observacion
                Cuentasanterior.numeroID = value.NumeroID
                Cuentasanterior.idCuentasclientes = value.IDCuentasclientes
                Cuentasanterior.tipoID = CuentasClientesSelected.TipoID
                Cuentasanterior.operaciones = CuentasClientesSelected.Operaciones
                Cuentasanterior.tipoCuenta = CuentasClientesSelected.TipoCuenta
                Cuentasanterior.tipoDocumento = CuentasClientesSelected.tipoDocumento
                Cuentasanterior.titular = CuentasClientesSelected.Titular
                logvalidoreplicacuenta = False
                logcambiotitularcuenta = False
                MyBase.CambioItem("CuentasClientesSelected")
            End If
        End Set
    End Property
    Private Sub _CuentasClientesSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _CuentasClientesSelected.PropertyChanged
        If e.PropertyName.Equals("HasChanges") Or e.PropertyName.Equals("EntityState") Or e.PropertyName.Equals("IsReadOnly") Or e.PropertyName.Equals("InfoSesion") Then
            Exit Sub
        End If
        If Editando = False Then
            Exit Sub
        End If
        If (e.PropertyName.Equals("TipoID")) Then
            If _ClienteSelected.NroDocumento.ToString = CuentasClientesSelected.NumeroID And _ClienteSelected.TipoIdentificacion = CuentasClientesSelected.TipoID Then
                CuentasClientesSelected.HabilitarUnicoTitular = True
            Else
                CuentasClientesSelected.HabilitarUnicoTitular = False
                CuentasClientesSelected.UnicoTitular = False
            End If
        End If
        If logvalidoreplicacuenta Then
            If (e.PropertyName.Equals("Titular") Or e.PropertyName.Equals("TipoID") Or e.PropertyName.Equals("NumeroID")) And logcambiotitularcuenta Then
                Exit Sub
            End If
            logvalidoreplicacuenta = False
            Exit Sub
        End If
        If e.PropertyName.Equals("logTitular") Then
            If CuentasClientesSelected.logTitular Then
                logvalidoreplicacuenta = True
                logcambiotitularcuenta = True
                CuentasClientesSelected.Titular = ClienteSelected.Nombre
                CuentasClientesSelected.TipoID = ClienteSelected.TipoIdentificacion
                CuentasClientesSelected.NumeroID = ClienteSelected.strNroDocumento
            End If
        End If
        If Not IsNothing(CuentasClientesSelected.IdConsecutivoSafyr) Or Not IsNothing(CuentasClientesSelected.IdConsecutivoClientes) Or Not IsNothing(CuentasClientesSelected.IdConsecutivoPortafolios) Then
            mostrarMensajePregunta("Esta cuenta se encuentra replicada. Un cambio en OYD impacta la integridad de la información con otros sistemas" & vbCrLf _
                                   & "¿Continúa?" & vbCrLf & "(<Si> acepta el cambio y continúa el proceso / <No> descarta el cambio y continúa el proceso.)",
                       Program.TituloSistema,
                       e.PropertyName,
                       AddressOf Terminovalidarcuentaotrossistemas, False)
        End If

    End Sub
    Private Sub Terminovalidarcuentaotrossistemas(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If Not objResultado.DialogResult Then
            logvalidoreplicacuenta = True
            Select Case objResultado.CodigoLlamado
                Case "logTitular"
                    CuentasClientesSelected.logTitular = Cuentasanterior.logTitular
                Case "NombreSucursal"
                    CuentasClientesSelected.NombreSucursal = Cuentasanterior.nombreSucursal
                Case "NombreBanco"
                    CuentasClientesSelected.NombreBanco = Cuentasanterior.nombreBanco
                Case "Cuenta"
                    CuentasClientesSelected.Cuenta = Cuentasanterior.cuenta
                Case "Observacion"
                    CuentasClientesSelected.Observacion = Cuentasanterior.observacion
                Case "NumeroID"
                    logcambiotitularcuenta = False
                    CuentasClientesSelected.NumeroID = Cuentasanterior.numeroID
                Case "IDCuentasclientes"
                    CuentasClientesSelected.IDCuentasclientes = Cuentasanterior.idCuentasclientes
                Case "TipoID"
                    logcambiotitularcuenta = False
                    CuentasClientesSelected.TipoID = Cuentasanterior.tipoID
                Case "Operaciones"
                    CuentasClientesSelected.Operaciones = Cuentasanterior.operaciones
                Case "TipoCuenta"
                    CuentasClientesSelected.TipoCuenta = Cuentasanterior.tipoCuenta
                Case "tipoDocumento"
                    CuentasClientesSelected.tipoDocumento = Cuentasanterior.tipoDocumento
                Case "Titular"
                    logcambiotitularcuenta = False
                    CuentasClientesSelected.Titular = Cuentasanterior.titular
            End Select
        Else
            logvalidoreplicacuenta = False
            Select Case objResultado.CodigoLlamado
                Case "logTitular"
                    Cuentasanterior.logTitular = CuentasClientesSelected.logTitular
                Case "NombreSucursal"
                    Cuentasanterior.nombreSucursal = CuentasClientesSelected.NombreSucursal
                Case "NombreBanco"
                    Cuentasanterior.nombreBanco = CuentasClientesSelected.NombreBanco
                Case "Cuenta"
                    Cuentasanterior.cuenta = CuentasClientesSelected.Cuenta
                Case "Observacion"
                    Cuentasanterior.observacion = CuentasClientesSelected.Observacion
                Case "NumeroID"
                    Cuentasanterior.numeroID = CuentasClientesSelected.NumeroID
                Case "IDCuentasclientes"
                    Cuentasanterior.idCuentasclientes = CuentasClientesSelected.IDCuentasclientes
                Case "TipoID"
                    Cuentasanterior.tipoID = CuentasClientesSelected.TipoID
                Case "Operaciones"
                    Cuentasanterior.operaciones = CuentasClientesSelected.Operaciones
                Case "TipoCuenta"
                    Cuentasanterior.tipoCuenta = CuentasClientesSelected.TipoCuenta
                Case "tipoDocumento"
                    Cuentasanterior.tipoDocumento = CuentasClientesSelected.tipoDocumento
                Case "Titular"
                    Cuentasanterior.titular = CuentasClientesSelected.Titular
            End Select
        End If
    End Sub
#End Region
#Region "Clientes receptores"
    Private _ListaClientesReceptore As EntitySet(Of OyDClientes.ClientesReceptore)
    Public Property ListaClientesReceptore() As EntitySet(Of OyDClientes.ClientesReceptore)
        Get
            Return _ListaClientesReceptore
        End Get
        Set(ByVal value As EntitySet(Of OyDClientes.ClientesReceptore))
            _ListaClientesReceptore = value
            MyBase.CambioItem("ListaClientesReceptore")
            Dim strReceptorEntrevista As String = String.Empty
            If Not IsNothing(_ClienteSelected) Then
                strReceptorEntrevista = _ClienteSelected.ReceptorEntrevista
            End If
            ListaClientesReceptoreEntrevista = Nothing
            If Not IsNothing(_ListaClientesReceptore) Then
                ListaClientesReceptoreEntrevista = _ListaClientesReceptore.ToList
            End If
            If Not String.IsNullOrEmpty(strReceptorEntrevista) Then
                _ClienteSelected.ReceptorEntrevista = strReceptorEntrevista
            End If
        End Set
    End Property
    Private _ListaClientesReceptoreEntrevista As List(Of OyDClientes.ClientesReceptore)
    Public Property ListaClientesReceptoreEntrevista() As List(Of OyDClientes.ClientesReceptore)
        Get
            Return _ListaClientesReceptoreEntrevista
        End Get
        Set(ByVal value As List(Of OyDClientes.ClientesReceptore))
            _ListaClientesReceptoreEntrevista = value
            MyBase.CambioItem("ListaClientesReceptoreEntrevista")
        End Set
    End Property
    Private WithEvents _ClientesReceptoreSelected As OyDClientes.ClientesReceptore
    Public Property ClientesReceptoreSelected() As OyDClientes.ClientesReceptore
        Get
            Return _ClientesReceptoreSelected
        End Get
        Set(ByVal value As OyDClientes.ClientesReceptore)
            If Not value Is Nothing Then
                _ClientesReceptoreSelected = value
                MyBase.CambioItem("ClientesReceptoreSelected")
            End If
        End Set
    End Property
    'Private Sub __ClientesReceptoreSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ClientesReceptoreSelected.PropertyChanged
    '    If ClientesReceptoreSelected.Lider = False Then
    '        Exit Sub
    '    Else
    '        If e.PropertyName.Equals("Lider") Then
    '            For Each Lista In ListaClientesReceptore
    '                Dim esliderReceptor = ListaClientesReceptore.Where(Function(li) li.Lider = True)
    '                If esliderReceptor.Count = 2 Then
    '                    A2Utilidades.Mensajes.mostrarMensaje("No puede existir dos receptores lideres.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    '                    ClientesReceptoreSelected.Lider = False
    '                    Exit Sub
    '                End If
    '            Next
    '        End If
    '    End If
    'End Sub
    Sub validarreceptor()
        For Each Lista In ListaClientesReceptore
            Dim idreceptor = Lista.IDReceptor
            Dim receptores = ListaClientesReceptore.Where(Function(li) IIf(IsNothing(li.IDReceptor), String.Empty, li.IDReceptor).Equals(idreceptor)).ToList
            If receptores.Count = 2 Then
                A2Utilidades.Mensajes.mostrarMensaje("No puede existir dos receptores iguales.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                ClientesReceptoreSelected.IDReceptor = Nothing
                ClientesReceptoreSelected.Nombre = Nothing
                Exit Sub
            End If
        Next

    End Sub
#End Region
#Region "Clientes ordenantes"
    Private _ListaClientesOrdenantes As EntitySet(Of OyDClientes.ClientesOrdenante)
    Public Property ListaClientesOrdenantes() As EntitySet(Of OyDClientes.ClientesOrdenante)
        Get
            Return _ListaClientesOrdenantes
        End Get
        Set(ByVal value As EntitySet(Of OyDClientes.ClientesOrdenante))
            _ListaClientesOrdenantes = value
            MyBase.CambioItem("ListaClientesOrdenantes")
        End Set
    End Property

    Private WithEvents _ClientesOrdenantesSelected As OyDClientes.ClientesOrdenante
    Public Property ClientesOrdenantesSelected() As OyDClientes.ClientesOrdenante
        Get
            Return _ClientesOrdenantesSelected
        End Get
        Set(ByVal value As OyDClientes.ClientesOrdenante)
            If Not value Is Nothing Then
                _ClientesOrdenantesSelected = value
                MyBase.CambioItem("ClientesOrdenantesSelected")
            End If
        End Set
    End Property
    'Private Sub __ClientesOrdenantesSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ClientesOrdenantesSelected.PropertyChanged
    '    'If ClientesOrdenantesSelected.lider = False Then
    '    '    Exit Sub
    '    'Else
    '    'If e.PropertyName.Equals("lider") Then
    '    'For Each Lista In ListaClientesOrdenantes
    '    '    Dim esliderOrdenante = ListaClientesOrdenantes.Where(Function(li) li.lider = True)
    '    '    If esliderOrdenante.Count = 2 Then
    '    '        A2Utilidades.Mensajes.mostrarMensaje("No puede existir dos ordenantes lideres.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    '    '        ClientesOrdenantesSelected.lider = False
    '    '        Exit Sub
    '    '    End If
    '    'Next
    '    If ClientesOrdenantesSelected.Asociados >= 5 And ClientesOrdenantesSelected.Relacionado = False Then
    '        A2Utilidades.Mensajes.mostrarMensaje("El ordenante " & ClientesOrdenantesSelected.IDOrdenante.Trim.ToString & " ya tiene " & ClientesOrdenantesSelected.Asociados & " comitentes registrados", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    '        ClientesOrdenantesSelected.IDOrdenante = Nothing
    '        Exit Sub
    '    End If
    '    'End If
    '    'If e.PropertyName.Equals("Relacionado") Then
    '    '    If ClientesOrdenantesSelected.Asociados >= 5 And ClientesOrdenantesSelected.Relacionado = False Then
    '    '        A2Utilidades.Mensajes.mostrarMensaje("El ordenante " & ClientesOrdenantesSelected.IDOrdenante.Trim.ToString & " ya tiene " & ClientesOrdenantesSelected.Asociados & " comitentes registrados", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
    '    '        ClientesOrdenantesSelected.IDOrdenante = Nothing
    '    '        Exit Sub
    '    '    End If
    '    'End If

    '    'End If
    'End Sub
    Sub validaordenantes()
        Dim logTuvoValidacion As Boolean = False

        For Each Lista In ListaClientesOrdenantes
            Dim idordenante = Lista.IDOrdenante
            Dim ordenante = ListaClientesOrdenantes.Where(Function(li) IIf(IsNothing(li.IDOrdenante), String.Empty, li.IDOrdenante).Trim.Equals(idordenante)).ToList

            If ordenante.
                Count = 2 Then
                A2Utilidades.Mensajes.mostrarMensaje("No puede existir dos ordenantes iguales.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                ClientesOrdenantesSelected.IDOrdenante = Nothing
                ClientesOrdenantesSelected.Nombre = Nothing
                logTuvoValidacion = True
                Exit For
            End If

        Next

        If logTuvoValidacion = False Then
            ConsultarOrdenatesAsociados(ClientesOrdenantesSelected.IDOrdenante)
        End If
    End Sub
#End Region
#Region "Clientes beneficiarios"
    Private _ListaClientesBeneficiarios As EntitySet(Of OyDClientes.ClientesBeneficiarios)
    Public Property ListaClientesBeneficiarios() As EntitySet(Of OyDClientes.ClientesBeneficiarios)
        Get
            Return _ListaClientesBeneficiarios
        End Get
        Set(ByVal value As EntitySet(Of OyDClientes.ClientesBeneficiarios))
            _ListaClientesBeneficiarios = value
            MyBase.CambioItem("ListaClientesBeneficiarios")
        End Set
    End Property
    Private WithEvents _ClientesBeneficiarioSelected As OyDClientes.ClientesBeneficiarios
    Public Property ClientesBeneficiarioSelected() As OyDClientes.ClientesBeneficiarios
        Get
            Return _ClientesBeneficiarioSelected
        End Get
        Set(ByVal value As OyDClientes.ClientesBeneficiarios)

            If Not value Is Nothing Then
                _ClientesBeneficiarioSelected = value
                Beneficiarioanterior.activo = value.Activo
                Beneficiarioanterior.apellido1 = value.Apellido1
                Beneficiarioanterior.apellido2 = value.Apellido2
                Beneficiarioanterior.beneficiarioCuentasDeposito = value.BeneficiarioCuentasDeposito
                Beneficiarioanterior.direccion = value.Direccion
                Beneficiarioanterior.idCiudadDoc = value.IDCiudadDoc
                Beneficiarioanterior.idCiudadDomicilio = value.IdCiudadDomicilio
                Beneficiarioanterior.nombre = value.Nombre
                Beneficiarioanterior.nombre1 = value.Nombre1
                Beneficiarioanterior.nombre2 = value.Nombre2
                Beneficiarioanterior.nombreCiudadDoc = value.NombreCiudadDoc
                Beneficiarioanterior.nombreCiudadDomicilio = value.NombreCiudadDomicilio
                Beneficiarioanterior.nroDocumento = value.NroDocumento
                Beneficiarioanterior.parentesco = value.Parentesco
                Beneficiarioanterior.telefono = value.Telefono
                Beneficiarioanterior.tipoBeneficiario = value.TipoBeneficiario
                Beneficiarioanterior.tipoID = value.TipoID
                MyBase.CambioItem("ClientesBeneficiarioSelected")
            End If
        End Set
    End Property
    Private Sub _ClientesBeneficiarioSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ClientesBeneficiarioSelected.PropertyChanged
        If e.PropertyName.Equals("HasChanges") Or e.PropertyName.Equals("EntityState") Or e.PropertyName.Equals("IsReadOnly") Or e.PropertyName.Equals("InfoSesion") Or e.PropertyName.Equals("Nombre") Then
            Exit Sub
        End If
        If Editando = False Then
            Exit Sub
        End If
        If logvalidoreplicabene Then
            logvalidoreplicabene = False
            Exit Sub
        End If
        If Not IsNothing(ClientesBeneficiarioSelected.IdConsecutivoSafyr) Or Not IsNothing(ClientesBeneficiarioSelected.IdConsecutivoClientes) Or Not IsNothing(ClientesBeneficiarioSelected.IdConsecutivoPortafolios) Then
            mostrarMensajePregunta("Este beneficiario se encuentra repliacada. Un cambio en OYD impacta la integridad de la información con otros sistemas" & vbCrLf _
                                   & "¿Continúa?" & vbCrLf & "(<Si> acepta el cambio y continúa el proceso / <No> descarta el cambio y continúa el proceso.)",
                       Program.TituloSistema,
                       e.PropertyName,
                       AddressOf Terminovalidarbeneotrossistemas, False)
        End If
        If e.PropertyName.Equals("Apellido1") Or e.PropertyName.Equals("Apellido2") Or e.PropertyName.Equals("Nombre1") Or e.PropertyName.Equals("Nombre2") Then
            ClientesBeneficiarioSelected.Nombre = ClientesBeneficiarioSelected.Apellido1 + " " + ClientesBeneficiarioSelected.Apellido2 + " " + ClientesBeneficiarioSelected.Nombre1 + " " + ClientesBeneficiarioSelected.Nombre2
        End If


    End Sub
    Private Sub Terminovalidarbeneotrossistemas(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If Not objResultado.DialogResult Then
            logvalidoreplicabene = True
            Select Case objResultado.CodigoLlamado
                Case "Activo"
                    ClientesBeneficiarioSelected.Activo = Beneficiarioanterior.activo
                Case "Apellido1"
                    ClientesBeneficiarioSelected.Apellido1 = Beneficiarioanterior.apellido1
                Case "Apellido2"
                    ClientesBeneficiarioSelected.Apellido2 = Beneficiarioanterior.apellido2
                Case "BeneficiarioCuentasDeposito"
                    ClientesBeneficiarioSelected.BeneficiarioCuentasDeposito = Beneficiarioanterior.beneficiarioCuentasDeposito
                Case "Direccion"
                    ClientesBeneficiarioSelected.Direccion = Beneficiarioanterior.direccion
                Case "IDCiudadDoc"
                    ClientesBeneficiarioSelected.IDCiudadDoc = Beneficiarioanterior.idCiudadDoc
                Case "IdCiudadDomicilio"
                    ClientesBeneficiarioSelected.IdCiudadDomicilio = Beneficiarioanterior.idCiudadDomicilio
                Case "Nombre1"
                    ClientesBeneficiarioSelected.Nombre1 = Beneficiarioanterior.nombre1
                Case "Nombre2"
                    ClientesBeneficiarioSelected.Nombre2 = Beneficiarioanterior.nombre2
                Case "NombreCiudadDoc"
                    ClientesBeneficiarioSelected.NombreCiudadDoc = Beneficiarioanterior.nombreCiudadDoc
                Case "NombreCiudadDomicilio"
                    ClientesBeneficiarioSelected.NombreCiudadDomicilio = Beneficiarioanterior.nombreCiudadDomicilio
                Case "NroDocumento"
                    ClientesBeneficiarioSelected.NroDocumento = Beneficiarioanterior.nroDocumento
                Case "Parentesco"
                    ClientesBeneficiarioSelected.Parentesco = Beneficiarioanterior.parentesco
                Case "Telefono"
                    ClientesBeneficiarioSelected.Telefono = Beneficiarioanterior.telefono
                Case "TipoBeneficiario"
                    ClientesBeneficiarioSelected.TipoBeneficiario = Beneficiarioanterior.tipoBeneficiario
                Case "TipoID"
                    ClientesBeneficiarioSelected.TipoID = Beneficiarioanterior.tipoID
            End Select
        Else
            logvalidoreplicabene = False
            Select Case objResultado.CodigoLlamado
                Case "Activo"
                    Beneficiarioanterior.activo = ClientesBeneficiarioSelected.Activo
                Case "Apellido1"
                    Beneficiarioanterior.apellido1 = ClientesBeneficiarioSelected.Apellido1
                Case "Apellido2"
                    Beneficiarioanterior.apellido2 = ClientesBeneficiarioSelected.Apellido2
                Case "BeneficiarioCuentasDeposito"
                    Beneficiarioanterior.beneficiarioCuentasDeposito = ClientesBeneficiarioSelected.BeneficiarioCuentasDeposito
                Case "Direccion"
                    Beneficiarioanterior.direccion = ClientesBeneficiarioSelected.Direccion
                Case "IDCiudadDoc"
                    Beneficiarioanterior.idCiudadDoc = ClientesBeneficiarioSelected.IDCiudadDoc
                Case "IdCiudadDomicilio"
                    Beneficiarioanterior.idCiudadDomicilio = ClientesBeneficiarioSelected.IdCiudadDomicilio
                Case "Nombre1"
                    Beneficiarioanterior.nombre1 = ClientesBeneficiarioSelected.Nombre1
                Case "Nombre2"
                    Beneficiarioanterior.nombre2 = ClientesBeneficiarioSelected.Nombre2
                Case "NombreCiudadDoc"
                    Beneficiarioanterior.nombreCiudadDoc = ClientesBeneficiarioSelected.NombreCiudadDoc
                Case "NombreCiudadDomicilio"
                    Beneficiarioanterior.nombreCiudadDomicilio = ClientesBeneficiarioSelected.NombreCiudadDomicilio
                Case "NroDocumento"
                    Beneficiarioanterior.nroDocumento = ClientesBeneficiarioSelected.NroDocumento
                Case "Parentesco"
                    Beneficiarioanterior.parentesco = ClientesBeneficiarioSelected.Parentesco
                Case "Telefono"
                    Beneficiarioanterior.telefono = ClientesBeneficiarioSelected.Telefono
                Case "TipoBeneficiario"
                    Beneficiarioanterior.tipoBeneficiario = ClientesBeneficiarioSelected.TipoBeneficiario
                Case "TipoID"
                    Beneficiarioanterior.tipoID = ClientesBeneficiarioSelected.TipoID
            End Select
        End If
    End Sub
#End Region
#Region "Clientes Aficiones"
    Private _ListaClientesAficiones As EntitySet(Of OyDClientes.ClientesAficiones)
    Public Property ListaClientesAficiones() As EntitySet(Of OyDClientes.ClientesAficiones)
        Get
            Return _ListaClientesAficiones
        End Get
        Set(ByVal value As EntitySet(Of OyDClientes.ClientesAficiones))
            _ListaClientesAficiones = value
            MyBase.CambioItem("ListaClientesAficiones")
        End Set
    End Property
    Private WithEvents _ClientesAficionesSelected As OyDClientes.ClientesAficiones
    Public Property ClientesAficionesSelected() As OyDClientes.ClientesAficiones
        Get
            Return _ClientesAficionesSelected
        End Get
        Set(ByVal value As OyDClientes.ClientesAficiones)

            If Not value Is Nothing Then
                _ClientesAficionesSelected = value
                MyBase.CambioItem("ClientesAficionesSelected")
            End If
        End Set
    End Property
    'Private _Listaclientesaficionesclase As ObservableCollection(Of ListaoriginalAficiones) = New ObservableCollection(Of ListaoriginalAficiones)
    'Public Property Listaclientesaficionesclase() As ObservableCollection(Of ListaoriginalAficiones)
    '    Get
    '        Return _Listaclientesaficionesclase
    '    End Get
    '    Set(ByVal value As ObservableCollection(Of ListaoriginalAficiones))
    '        _Listaclientesaficionesclase = value
    '        ClientesAficionesSelectedClase = value.FirstOrDefault

    '        MyBase.CambioItem("Listaclientesaficionesclase")
    '        MyBase.CambioItem("ClientesAficionesPaged")

    '    End Set

    'End Property
    'Private WithEvents _ClientesAficionesSelectedClase As ListaoriginalAficiones = New ListaoriginalAficiones
    'Public Property ClientesAficionesSelectedClase() As ListaoriginalAficiones
    '    Get
    '        Return _ClientesAficionesSelectedClase
    '    End Get
    '    Set(ByVal value As ListaoriginalAficiones)

    '        If Not value Is Nothing Then
    '            _ClientesAficionesSelectedClase = value
    '            MyBase.CambioItem("ClientesAficionesSelectedClase")
    '        End If
    '    End Set
    'End Property
    'Private Sub _ClientesAficionesSelectedClase_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ClientesAficionesSelectedClase.PropertyChanged
    '    If e.PropertyName.Equals("Seleccion") Then
    '        If ClientesAficionesSelectedClase.Seleccion = True Then
    '            nuevaAficion()
    '        Else
    '            For Each a In ListaClientesAficiones
    '                If (ClientesAficionesSelectedClase.Retorno.Equals(a.Retorno)) Then
    '                    ClientesAficionesSelected = a
    '                End If
    '            Next
    '            ListaClientesAficiones.Remove(ClientesAficionesSelected)
    '            ClientesAficionesSelected = _ListaClientesAficiones.LastOrDefault
    '        End If

    '    End If

    'End Sub
    Private Sub _ClientesAficionesSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ClientesAficionesSelected.PropertyChanged

        If e.PropertyName.Equals("Seleccion") Then
            If ClientesAficionesSelected.Seleccion = True Then
                'For Each a In ListaClientesAficiones
                '    If (ClientesAficionesSelectedClase.Retorno.Equals(a.Retorno)) Then
                '        ClientesAficionesSelected = a
                '    End If
                'Next
                nuevaAficion()
                'Else
                '    For Each a In ListaClientesAficiones
                '        If (ClientesAficionesSelectedClase.Retorno.Equals(a.Retorno)) Then
                '            ClientesAficionesSelected = a
                '        End If
                '    Next
                '    ListaClientesAficiones.Remove(ClientesAficionesSelected)
                '    ClientesAficionesSelected = _ListaClientesAficiones.LastOrDefault
            End If
        End If

    End Sub
    Sub nuevaAficion()
        Try
            ClientesAficionesSelected.Usuario = Program.Usuario
            ClientesAficionesSelected.IDSucCliente = ClienteSelected.IDSucCliente
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region
#Region "Clientes Deportes"
    Private _ListaClientesDeportes As EntitySet(Of OyDClientes.ClientesDeportes)
    Public Property ListaClientesDeportes() As EntitySet(Of OyDClientes.ClientesDeportes)
        Get
            Return _ListaClientesDeportes
        End Get
        Set(ByVal value As EntitySet(Of OyDClientes.ClientesDeportes))
            _ListaClientesDeportes = value
            MyBase.CambioItem("ListaClientesDeportes")
        End Set
    End Property
    Private WithEvents _ClientesDeportesSelected As OyDClientes.ClientesDeportes
    Public Property ClientesDeportesSelected() As OyDClientes.ClientesDeportes
        Get
            Return _ClientesDeportesSelected
        End Get
        Set(ByVal value As OyDClientes.ClientesDeportes)

            If Not value Is Nothing Then
                _ClientesDeportesSelected = value
                MyBase.CambioItem("ClientesDeportesSelected")
            End If
        End Set
    End Property
    Private Sub _ClientesDeportesSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ClientesDeportesSelected.PropertyChanged

        If e.PropertyName.Equals("Seleccion") Then
            If ClientesDeportesSelected.Seleccion = True Then
                'For Each a In ListaClientesDeportes
                '    If (ClientesDeportesSelectedClase.Retorno.Equals(a.Retorno)) Then
                '        ClientesDeportesSelected = a
                '    End If
                'Next
                nuevodeporte()
                'Else
                '    For Each a In ListaClientesDeportes
                '        If (ClientesDeportesSelectedClase.Retorno.Equals(a.Retorno)) Then
                '            ClientesDeportesSelected = a
                '        End If
                '    Next
                '    ListaClientesDeportes.Remove(ClientesDeportesSelected)
                '    ClientesDeportesSelected = _ListaClientesDeportes.LastOrDefault
            End If
        End If

    End Sub
    Sub nuevodeporte()
        Try
            ClientesDeportesSelected.Usuario = Program.Usuario
            ClientesDeportesSelected.IDSucCliente = ClienteSelected.IDSucCliente
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region
#Region "Clientes Direcciones"
    Private _ListaClientesDirecciones As EntitySet(Of OyDClientes.ClientesDireccione)
    Public Property ListaClientesDirecciones() As EntitySet(Of OyDClientes.ClientesDireccione)
        Get
            Return _ListaClientesDirecciones
        End Get
        Set(ByVal value As EntitySet(Of OyDClientes.ClientesDireccione))
            _ListaClientesDirecciones = value
            MyBase.CambioItem("ListaClientesDirecciones")
        End Set
    End Property
    Private WithEvents _ClientesDireccionesSelected As OyDClientes.ClientesDireccione
    Public Property ClientesDireccionesSelected() As OyDClientes.ClientesDireccione
        Get
            Return _ClientesDireccionesSelected
        End Get
        Set(ByVal value As OyDClientes.ClientesDireccione)

            If Not value Is Nothing Then
                _ClientesDireccionesSelected = value
                Direccionesanterior.activo = value.Activo
                Direccionesanterior.ciudad = value.Ciudad
                Direccionesanterior.direccion = value.Direccion
                Direccionesanterior.direccionEnvio = value.DireccionEnvio
                Direccionesanterior.entregaA = value.EntregaA
                Direccionesanterior.extension = value.Extension
                Direccionesanterior.fax = value.Fax
                Direccionesanterior.nombre = value.Nombre
                Direccionesanterior.telefono = value.Telefono
                Direccionesanterior.tipo = value.Tipo
                logvalidoreplicadirecc = False
                MyBase.CambioItem("ClientesDireccionesSelected")
            End If
        End Set
    End Property
    Private Sub _ClientesDireccionesSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ClientesDireccionesSelected.PropertyChanged
        If e.PropertyName.Equals("HasChanges") Or e.PropertyName.Equals("EntityState") Or e.PropertyName.Equals("IsReadOnly") Or e.PropertyName.Equals("InfoSesion") Then
            Exit Sub
        End If
        If Editando = False Then
            Exit Sub
        End If
        If logvalidoreplicadirecc Then
            logvalidoreplicadirecc = False
            Exit Sub
        End If
        If e.PropertyName.Equals("DireccionEnvio") Then
            If ClientesDireccionesSelected.DireccionEnvio = False Then
                Exit Sub
            End If
            Dim esdireccionenvio = ListaClientesDirecciones.Where(Function(li) li.DireccionEnvio = True)
            If esdireccionenvio.Count >= 2 Then
                A2Utilidades.Mensajes.mostrarMensaje("Sólo puede elegir una Dirección de envio para el cliente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                ClientesDireccionesSelected.DireccionEnvio = False
                Exit Sub
            End If
            If ClientesDireccionesSelected.DireccionEnvio = True Then
                ClienteSelected.DireccionEnvio = ClientesDireccionesSelected.Direccion
                ClienteSelected.IDPoblacion = ClientesDireccionesSelected.Ciudad
                If Not IsNothing(ClientesDireccionesSelected.IdDepartamento) And ClientesDireccionesSelected.IdDepartamento > 0 Then
                    ClienteSelected.IDDepartamento = ClientesDireccionesSelected.IdDepartamento
                End If
                If Not IsNothing(ClientesDireccionesSelected.IdPais) And ClientesDireccionesSelected.IdPais > 0 Then
                    ClienteSelected.IdPais = ClientesDireccionesSelected.IdPais
                End If
                ClienteSelected.EntregarA = ClientesDireccionesSelected.EntregaA
                ClienteSelected.Fax1 = ClientesDireccionesSelected.Fax
                If IsNothing(ClientesDireccionesSelected.Telefono) Then
                    ClienteSelected.Telefono1 = ""
                Else
                    ClienteSelected.Telefono1 = ClientesDireccionesSelected.Telefono
                End If
            End If
        Else
            If ClientesDireccionesSelected.Tipo = "C" And ClienteSelected.TipoPersona = 1 Then
                ClienteSelected.Direccion = ClientesDireccionesSelected.Direccion
            ElseIf ClientesDireccionesSelected.Tipo = "O" Then
                If ClienteSelected.TipoPersona = 2 Then
                    ClienteSelected.Direccion = ClientesDireccionesSelected.Direccion
                End If
                ClienteSelected.DireccionOficina = ClientesDireccionesSelected.Direccion
            End If
            If ClientesDireccionesSelected.DireccionEnvio = True Then
                ClienteSelected.DireccionEnvio = ClientesDireccionesSelected.Direccion
                ClienteSelected.IDPoblacion = ClientesDireccionesSelected.Ciudad
                If Not IsNothing(ClientesDireccionesSelected.IdDepartamento) And ClientesDireccionesSelected.IdDepartamento > 0 Then
                    ClienteSelected.IDDepartamento = ClientesDireccionesSelected.IdDepartamento
                End If
                If Not IsNothing(ClientesDireccionesSelected.IdPais) And ClientesDireccionesSelected.IdPais > 0 Then
                    ClienteSelected.IdPais = ClientesDireccionesSelected.IdPais
                End If
                ClienteSelected.EntregarA = ClientesDireccionesSelected.EntregaA
                ClienteSelected.Fax1 = ClientesDireccionesSelected.Fax
                If IsNothing(ClientesDireccionesSelected.Telefono) Then
                    ClienteSelected.Telefono1 = ""
                Else
                    ClienteSelected.Telefono1 = ClientesDireccionesSelected.Telefono
                End If
            End If
        End If

        If e.PropertyName.Equals("Tipo") Or e.PropertyName.Equals("Ciudad") Or e.PropertyName.Equals("Activo") Or e.PropertyName.Equals("DireccionEnvio") Then
            validardireccionSafyr(e.PropertyName)
        End If

    End Sub
    Public Sub validardireccionSafyr(ByVal pstrcampo As String)
        If Not IsNothing(ClientesDireccionesSelected.IdConsecutivoSafyr) Or Not IsNothing(ClientesDireccionesSelected.IdConsecutivoClientes) Or Not IsNothing(ClientesDireccionesSelected.IdConsecutivoPortafolios) Then
            mostrarMensajePregunta("Esta cuenta se encuentra repliacada. Un cambio en OYD impacta la integridad de la información con otros sistemas" & vbCrLf _
                                       & "¿Continúa?" & vbCrLf & "(<Si> acepta el cambio y continúa el proceso / <No> descarta el cambio y continúa el proceso.)",
                           Program.TituloSistema,
                       pstrcampo,
                           AddressOf Terminovalidardirecotrossistemas, False)
        End If
    End Sub
    Private Sub Terminovalidardirecotrossistemas(ByVal sender As Object, ByVal e As EventArgs)
        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        If Not objResultado.DialogResult Then
            logvalidoreplicadirecc = True
            Select Case objResultado.CodigoLlamado
                Case "Activo"
                    ClientesDireccionesSelected.Activo = Direccionesanterior.activo
                Case "Ciudad"
                    ClientesDireccionesSelected.Ciudad = Direccionesanterior.ciudad
                Case "Direccion"
                    ClientesDireccionesSelected.Direccion = Direccionesanterior.direccion
                Case "DireccionEnvio"
                    ClientesDireccionesSelected.DireccionEnvio = Direccionesanterior.direccionEnvio
                Case "EntregaA"
                    ClientesDireccionesSelected.EntregaA = Direccionesanterior.entregaA
                Case "Extension"
                    ClientesDireccionesSelected.Extension = Direccionesanterior.extension
                Case "Fax"
                    ClientesDireccionesSelected.Fax = Direccionesanterior.fax
                Case "Nombre"
                    ClientesDireccionesSelected.Nombre = Direccionesanterior.nombre
                Case "Telefono"
                    ClientesDireccionesSelected.Telefono = Direccionesanterior.telefono
                Case "Tipo"
                    ClientesDireccionesSelected.Tipo = Direccionesanterior.tipo
            End Select
        Else
            logvalidoreplicadirecc = False
            Select Case objResultado.CodigoLlamado
                Case "Activo"
                    Direccionesanterior.activo = ClientesDireccionesSelected.Activo
                Case "Ciudad"
                    Direccionesanterior.ciudad = ClientesDireccionesSelected.Ciudad
                Case "Direccion"
                    Direccionesanterior.direccion = ClientesDireccionesSelected.Direccion
                Case "DireccionEnvio"
                    Direccionesanterior.direccionEnvio = ClientesDireccionesSelected.DireccionEnvio
                Case "EntregaA"
                    Direccionesanterior.entregaA = ClientesDireccionesSelected.EntregaA
                Case "Extension"
                    Direccionesanterior.extension = ClientesDireccionesSelected.Extension
                Case "Fax"
                    Direccionesanterior.fax = ClientesDireccionesSelected.Fax
                Case "Nombre"
                    Direccionesanterior.nombre = ClientesDireccionesSelected.Nombre
                Case "Telefono"
                    Direccionesanterior.telefono = ClientesDireccionesSelected.Telefono
                Case "Tipo"
                    Direccionesanterior.tipo = ClientesDireccionesSelected.Tipo
            End Select
        End If
    End Sub
#End Region
#Region "Clientes ClientesDocumentosRequeridos"
    Private _ListaClientesDocumentosRequeridos As EntitySet(Of OyDClientes.ClientesDocumentosRequeridos)
    Public Property ListaClientesDocumentosRequeridos() As EntitySet(Of OyDClientes.ClientesDocumentosRequeridos)
        Get
            Return _ListaClientesDocumentosRequeridos
        End Get
        Set(ByVal value As EntitySet(Of OyDClientes.ClientesDocumentosRequeridos))
            _ListaClientesDocumentosRequeridos = value
            MyBase.CambioItem("ListaClientesDocumentosRequeridos")
        End Set
    End Property

    ''' <history>
    ''' Descripción:    Se crea la propiedad ListaClientesProductos para llenar el grid de la pestaña clasificaciones.
    ''' Responsable:    Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:          29 de Septiembre/2014
    ''' Pruebas CB:     Jorge Peña (Alcuadrado S.A.) - 29 de Septiembre/2014
    ''' </history>
    Private _ListaClientesProductos As EntitySet(Of OyDClientes.ClientesProductos)
    Public Property ListaClientesProductos() As EntitySet(Of OyDClientes.ClientesProductos)
        Get
            Return _ListaClientesProductos
        End Get
        Set(ByVal value As EntitySet(Of OyDClientes.ClientesProductos))
            _ListaClientesProductos = value
            MyBase.CambioItem("ListaClientesProductos")
        End Set
    End Property

    Private WithEvents _ClientesDocumentosRequeridosSelected As OyDClientes.ClientesDocumentosRequeridos
    Public Property ClientesDocumentosRequeridosSelected() As OyDClientes.ClientesDocumentosRequeridos
        Get
            Return _ClientesDocumentosRequeridosSelected
        End Get
        Set(ByVal value As OyDClientes.ClientesDocumentosRequeridos)

            If Not value Is Nothing Then
                _ClientesDocumentosRequeridosSelected = value
                MyBase.CambioItem("ClientesDocumentosRequeridosSelected")
            End If
        End Set
    End Property

    Private Sub _ClientesDocumentosRequeridosSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ClientesDocumentosRequeridosSelected.PropertyChanged

        If e.PropertyName.Equals("Entregado") Then
            If ClientesDocumentosRequeridosSelected.Entregado = True Then
                'For Each a In ListaClientesDeportes
                '    If (ClientesDeportesSelectedClase.Retorno.Equals(a.Retorno)) Then
                '        ClientesDeportesSelected = a
                '    End If
                'Next
                nuevoDocumentosRequerido()
                'Else
                '    'For Each a In ListaClientesDeportes
                '    '    If (ClientesDeportesSelectedClase.Retorno.Equals(a.Retorno)) Then
                '    '        ClientesDeportesSelected = a
                '    '    End If
                '    'Next
                '    dcProxy.ClientesDocumentosRequeridos.Remove(ClientesDocumentosRequeridosSelected)
                '    'ListaClientesDocumentosRequeridos.Remove(ClientesDocumentosRequeridosSelected)
                '    ClientesDocumentosRequeridosSelected = _ListaClientesDocumentosRequeridos.LastOrDefault
            Else
                ClientesDocumentosRequeridosSelected.Entrega = Nothing
            End If
        End If
    End Sub

    Sub nuevoDocumentosRequerido()
        Try
            ClientesDocumentosRequeridosSelected.Usuario = Program.Usuario
            ClientesDocumentosRequeridosSelected.Entrega = Now.Date
            'ClientesDocumentosRequeridosSelected.IDSucCliente = "1"
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "nuevoDocumentosRequerido", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    Private WithEvents _ClientesClientesProductosSelected As OyDClientes.ClientesProductos
    Public Property ClientesClientesProductosSelected() As OyDClientes.ClientesProductos
        Get
            Return _ClientesClientesProductosSelected
        End Get
        Set(ByVal value As OyDClientes.ClientesProductos)

            If Not value Is Nothing Then
                _ClientesClientesProductosSelected = value
                MyBase.CambioItem("ClientesClientesProductosSelected")
            End If
        End Set
    End Property

    Private Sub _ClientesClientesProductosSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ClientesClientesProductosSelected.PropertyChanged
        If e.PropertyName.Equals("logOperaProducto") Then
            If ClientesClientesProductosSelected.logOperaProducto = True Then
                ClientesClientesProductosSelected.strUsuario = Program.Usuario
            End If
        End If
    End Sub

#End Region
#Region "Clientes ClientesLOGHistoriaI"
    Private _ListaClientesLOGHistoriaI As EntitySet(Of OyDClientes.ClientesLOGHistoriaIR)
    Public Property ListaClientesLOGHistoriaI() As EntitySet(Of OyDClientes.ClientesLOGHistoriaIR)
        Get
            Return _ListaClientesLOGHistoriaI
        End Get
        Set(ByVal value As EntitySet(Of OyDClientes.ClientesLOGHistoriaIR))
            _ListaClientesLOGHistoriaI = value
            MyBase.CambioItem("ListaClientesLOGHistoriaI")
        End Set
    End Property
    Private _ClientesLOGHistoriaISelected As OyDClientes.ClientesLOGHistoriaIR
    Public Property ClientesLOGHistoriaISelected() As OyDClientes.ClientesLOGHistoriaIR
        Get
            Return _ClientesLOGHistoriaISelected
        End Get
        Set(ByVal value As OyDClientes.ClientesLOGHistoriaIR)

            If Not value Is Nothing Then
                _ClientesLOGHistoriaISelected = value
                MyBase.CambioItem("ClientesLOGHistoriaISelected")
            End If
        End Set
    End Property
#End Region
#Region "Clientes ClientesConocimientoEspecifico"
    Private _ListaClientesConocimientoEspecifico As EntitySet(Of OyDClientes.ClientesConocimientoEspecifico)
    Public Property ListaClientesConocimientoEspecifico() As EntitySet(Of OyDClientes.ClientesConocimientoEspecifico)
        Get
            Return _ListaClientesConocimientoEspecifico
        End Get
        Set(ByVal value As EntitySet(Of OyDClientes.ClientesConocimientoEspecifico))
            _ListaClientesConocimientoEspecifico = value
            MyBase.CambioItem("ListaClientesConocimientoEspecifico")
        End Set
    End Property
    Private _ClientesConocimientoEspecificoSelected As OyDClientes.ClientesConocimientoEspecifico
    Public Property ClientesConocimientoEspecificoSelected() As OyDClientes.ClientesConocimientoEspecifico
        Get
            Return _ClientesConocimientoEspecificoSelected
        End Get
        Set(ByVal value As OyDClientes.ClientesConocimientoEspecifico)

            If Not value Is Nothing Then
                _ClientesConocimientoEspecificoSelected = value
                MyBase.CambioItem("ClientesConocimientoEspecificoSelected")
            End If
        End Set
    End Property
    Private _ListaClientesConocimientoEspecificoclase As ObservableCollection(Of ListaOriginalClientesConocimientoEspecifico) = New ObservableCollection(Of ListaOriginalClientesConocimientoEspecifico)
    Public Property ListaClientesConocimientoEspecificoclase() As ObservableCollection(Of ListaOriginalClientesConocimientoEspecifico)
        Get
            Return _ListaClientesConocimientoEspecificoclase
        End Get
        Set(ByVal value As ObservableCollection(Of ListaOriginalClientesConocimientoEspecifico))
            _ListaClientesConocimientoEspecificoclase = value
            ClientesConocimientoEspecificoSelectedClase = value.FirstOrDefault

            MyBase.CambioItem("ListaClientesConocimientoEspecificoclase")
            MyBase.CambioItem("ClientesConocimientoEspecificoPaged")
        End Set
    End Property
    Private WithEvents _ClientesConocimientoEspecificoSelectedClase As ListaOriginalClientesConocimientoEspecifico = New ListaOriginalClientesConocimientoEspecifico
    Public Property ClientesConocimientoEspecificoSelectedClase() As ListaOriginalClientesConocimientoEspecifico
        Get
            Return _ClientesConocimientoEspecificoSelectedClase
        End Get
        Set(ByVal value As ListaOriginalClientesConocimientoEspecifico)

            If Not value Is Nothing Then
                _ClientesConocimientoEspecificoSelectedClase = value
                MyBase.CambioItem("ClientesConocimientoEspecificoSelectedClase")
            End If
        End Set
    End Property
    Private Sub _ClientesConocimientoEspecificoSelectedClase_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ClientesConocimientoEspecificoSelectedClase.PropertyChanged

        If e.PropertyName.Equals("Conocimiento") Then
            If ClientesConocimientoEspecificoSelectedClase.Conocimiento = True Then
                For Each a In ListaClientesConocimientoEspecifico
                    If (ClientesConocimientoEspecificoSelectedClase.CodigoConocimiento = a.CodigoConocimiento) Then
                        ClientesConocimientoEspecificoSelected = a
                    End If
                Next
                nuevoconocimiento()
            Else
                For Each a In ListaClientesConocimientoEspecifico
                    If (ClientesConocimientoEspecificoSelectedClase.CodigoConocimiento = a.CodigoConocimiento) Then
                        ClientesConocimientoEspecificoSelected = a
                    End If
                Next
                ListaClientesConocimientoEspecifico.Remove(ClientesConocimientoEspecificoSelected)
                ClientesConocimientoEspecificoSelected = _ListaClientesConocimientoEspecifico.LastOrDefault
            End If
            cambioconocimiento = True
        End If

    End Sub
    Sub nuevoconocimiento()
        Try
            'Dim NewClientesConocimiento As New OyDClientes.ClientesConocimientoEspecifico
            ''TODO: Verificar cuales son los campos que deben inicializarse
            'NewClientesConocimiento.ID = ClienteSelected.IDComitente
            'NewClientesConocimiento.IDClientesConocimiento = -1
            'NewClientesConocimiento.CodigoConocimiento = ClientesConocimientoEspecificoSelected.CodigoConocimiento
            'NewClientesConocimiento.DescripcionConocimiento = "p"
            'NewClientesConocimiento.Usuario = Program.Usuario
            'ListaClientesConocimientoEspecifico.Add(NewClientesConocimiento)
            'ClientesConocimientoEspecificoSelected = NewClientesConocimiento
            ClientesConocimientoEspecificoSelected.Usuario = Program.Usuario

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro",
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region
#Region "Clientes TipoCliente"
    Private _ListaClientesTipoCliente As EntitySet(Of OyDClientes.TipoClient)
    Public Property ListaClientesTipoCliente() As EntitySet(Of OyDClientes.TipoClient)
        Get
            Return _ListaClientesTipoCliente
        End Get
        Set(ByVal value As EntitySet(Of OyDClientes.TipoClient))
            _ListaClientesTipoCliente = value
            MyBase.CambioItem("ListaClientesTipoCliente")
        End Set
    End Property
    Private _ClientesTipoClienteSelected As OyDClientes.TipoClient
    Public Property ClientesTipoClienteSelected() As OyDClientes.TipoClient
        Get
            Return _ClientesTipoClienteSelected
        End Get
        Set(ByVal value As OyDClientes.TipoClient)

            If Not value Is Nothing Then
                _ClientesTipoClienteSelected = value
                MyBase.CambioItem("ClientesTipoClienteSelected")
            End If
        End Set
    End Property
#End Region
#Region "Clientes Accionistas"
    Private _ListaClientesAccionistas As EntitySet(Of OyDClientes.ClientesAccionistas)
    Public Property ListaClientesAccionistas() As EntitySet(Of OyDClientes.ClientesAccionistas)
        Get
            Return _ListaClientesAccionistas
        End Get
        Set(ByVal value As EntitySet(Of OyDClientes.ClientesAccionistas))
            _ListaClientesAccionistas = value
            MyBase.CambioItem("ListaClientesAccionistas")
        End Set
    End Property
    Private WithEvents _ClientesAccionistasSelected As OyDClientes.ClientesAccionistas
    Public Property ClientesAccionistasSelected() As OyDClientes.ClientesAccionistas
        Get
            Return _ClientesAccionistasSelected
        End Get
        Set(ByVal value As OyDClientes.ClientesAccionistas)

            If Not value Is Nothing Then
                _ClientesAccionistasSelected = value
                MyBase.CambioItem("ClientesAccionistasSelected")
            End If
        End Set
    End Property
    Private Sub _ClientesAccionistasSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ClientesAccionistasSelected.PropertyChanged
        If e.PropertyName.Equals("IDNacionalidad") Then
            If Not DescripcionValida(Program.FatcaDescripcionPais, "|", ClientesAccionistasSelected.IDNacionalidad) Then
                ClientesAccionistasSelected.AplicaFatca = False
                ClientesAccionistasSelected.TipoCiudadano = String.Empty
            End If
        End If
    End Sub
#End Region
#Region "Clientes ClienteFicha"
    Private _ListaClientesFicha As EntitySet(Of OyDClientes.ClientesFicha)
    Public Property ListaClientesFicha() As EntitySet(Of OyDClientes.ClientesFicha)
        Get
            Return _ListaClientesFicha
        End Get
        Set(ByVal value As EntitySet(Of OyDClientes.ClientesFicha))
            _ListaClientesFicha = value
            MyBase.CambioItem("ListaClientesFicha")
        End Set
    End Property
    Private WithEvents _ClientesFichaSelected As OyDClientes.ClientesFicha
    Public Property ClientesFichaSelected() As OyDClientes.ClientesFicha
        Get
            Return _ClientesFichaSelected
        End Get
        Set(ByVal value As OyDClientes.ClientesFicha)

            If Not value Is Nothing Then
                _ClientesFichaSelected = value
                MyBase.CambioItem("ClientesFichaSelected")
            End If
        End Set
    End Property
    Private Sub _ClientesFichaSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ClientesFichaSelected.PropertyChanged
        If e.PropertyName.Equals("TipoReferencia") Then
            If ClientesFichaSelected.TipoReferencia = "EM" Then
                ClientesFichaSelected.Descripcion = ClienteSelected.EMail
                ClientesFichaSelected.IDPoblacion = Nothing
                ClientesFichaSelected.NombreCiudad = ""
            Else
                ClientesFichaSelected.Descripcion = ""
                ClientesFichaSelected.IDPoblacion = Nothing
                ClientesFichaSelected.NombreCiudad = ""
            End If
        End If

    End Sub
#End Region
#Region "Clientes Personas"
    Private _ListaClientesPersonas As EntitySet(Of OyDClientes.ClientesPersonasParaConfirmar)
    Public Property ListaClientesPersonas() As EntitySet(Of OyDClientes.ClientesPersonasParaConfirmar)
        Get
            Return _ListaClientesPersonas
        End Get
        Set(ByVal value As EntitySet(Of OyDClientes.ClientesPersonasParaConfirmar))
            _ListaClientesPersonas = value
            MyBase.CambioItem("ListaClientesPersonas")
        End Set
    End Property
    Private WithEvents _ClientesPersonaselected As OyDClientes.ClientesPersonasParaConfirmar
    Public Property ClientesPersonaselected() As OyDClientes.ClientesPersonasParaConfirmar
        Get
            Return _ClientesPersonaselected
        End Get
        Set(ByVal value As OyDClientes.ClientesPersonasParaConfirmar)

            If Not value Is Nothing Then
                _ClientesPersonaselected = value
                MyBase.CambioItem("ClientesPersonaselected")
            End If
        End Set
    End Property
    'Private Sub _ClientesBeneficiarioSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ClientesBeneficiarioSelected.PropertyChanged
    '    If e.PropertyName.Equals("Apellido1") Or e.PropertyName.Equals("Apellido2") Or e.PropertyName.Equals("Nombre1") Or e.PropertyName.Equals("Nombre2") Then
    '        ClientesBeneficiarioSelected.Nombre = ClientesBeneficiarioSelected.Apellido1 + " " + ClientesBeneficiarioSelected.Apellido2 + " " + ClientesBeneficiarioSelected.Nombre1 + " " + ClientesBeneficiarioSelected.Nombre2
    '    End If

    'End Sub
#End Region
#Region "Clientes DepEconomica"
    Private _ListaClientesDepEconomica As EntitySet(Of OyDClientes.ClientesPersonasDepEconomica)
    Public Property ListaClientesDepEconomica() As EntitySet(Of OyDClientes.ClientesPersonasDepEconomica)
        Get
            Return _ListaClientesDepEconomica
        End Get
        Set(ByVal value As EntitySet(Of OyDClientes.ClientesPersonasDepEconomica))
            _ListaClientesDepEconomica = value
            MyBase.CambioItem("ListaClientesDepEconomica")
        End Set
    End Property
    Private WithEvents _ClientesDepEconomicaselected As OyDClientes.ClientesPersonasDepEconomica
    Public Property ClientesDepEconomicaselected() As OyDClientes.ClientesPersonasDepEconomica
        Get
            Return _ClientesDepEconomicaselected
        End Get
        Set(ByVal value As OyDClientes.ClientesPersonasDepEconomica)

            If Not value Is Nothing Then
                _ClientesDepEconomicaselected = value
                MyBase.CambioItem("ClientesDepEconomicaselected")
            End If
        End Set
    End Property
    'Private Sub _ClientesBeneficiarioSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _ClientesBeneficiarioSelected.PropertyChanged
    '    If e.PropertyName.Equals("Apellido1") Or e.PropertyName.Equals("Apellido2") Or e.PropertyName.Equals("Nombre1") Or e.PropertyName.Equals("Nombre2") Then
    '        ClientesBeneficiarioSelected.Nombre = ClientesBeneficiarioSelected.Apellido1 + " " + ClientesBeneficiarioSelected.Apellido2 + " " + ClientesBeneficiarioSelected.Nombre1 + " " + ClientesBeneficiarioSelected.Nombre2
    '    End If

    'End Sub
#End Region
#Region "Clientes ClientesPaisesFATCA"
    Private _ListaClientesPaisesFATCA As EntitySet(Of OyDClientes.ClientesPaisesFATCA)
    Public Property ListaClientesPaisesFATCA() As EntitySet(Of OyDClientes.ClientesPaisesFATCA)
        Get
            Return _ListaClientesPaisesFATCA
        End Get
        Set(ByVal value As EntitySet(Of OyDClientes.ClientesPaisesFATCA))
            _ListaClientesPaisesFATCA = value
            MyBase.CambioItem("ListaClientesPaisesFATCA")
        End Set
    End Property
    Private WithEvents _ClientesPaisesFATCAselected As OyDClientes.ClientesPaisesFATCA
    Public Property ClientesPaisesFATCAselected() As OyDClientes.ClientesPaisesFATCA
        Get
            Return _ClientesPaisesFATCAselected
        End Get
        Set(ByVal value As OyDClientes.ClientesPaisesFATCA)

            If Not value Is Nothing Then
                _ClientesPaisesFATCAselected = value
                MyBase.CambioItem("ClientesPaisesFATCAselected")
            End If
        End Set
    End Property

#End Region
    Public Overrides Sub NuevoRegistroDetalle()
        Try
            Select Case NombreColeccionDetalle
                Case "cmReceptores"
                    Dim NewClientesReceptoreSelected As New OyDClientes.ClientesReceptore
                    NewClientesReceptoreSelected.IDComitente = ClienteSelected.IDComitente
                    NewClientesReceptoreSelected.Usuario = Program.Usuario
                    NewClientesReceptoreSelected.Porcentaje = 0
                    NewClientesReceptoreSelected.Habilita = True
                    If IsNothing(ListaClientesReceptore) Then
                        ListaClientesReceptore = dcProxy.ClientesReceptores
                    End If
                    ListaClientesReceptore.Add(NewClientesReceptoreSelected)
                    ClientesReceptoreSelected = NewClientesReceptoreSelected
                    MyBase.CambioItem("ClientesReceptoreSelected")
                    MyBase.CambioItem("ListaClientesReceptore")

                    'CFMA20170130 se corrige la logica ya que el sistema presentaba inconsistencias al momento de guardar un nuevo registro 
                Case "cmCtasBancarias"
                    Dim NewCuentasClientesSelected As New OyDClientes.CuentasCliente
                    If Not IsNothing(ListaCuentasClientes) Then
                        NewCuentasClientesSelected.IDCuentasclientes = -(_ListaCuentasClientes.Count() + 1)
                    Else
                        NewCuentasClientesSelected.IDCuentasclientes = -1
                    End If
                    'CFMA20170130 

                    NewCuentasClientesSelected.IDComitente = ClienteSelected.IDComitente
                    NewCuentasClientesSelected.NumeroID = ""
                    NewCuentasClientesSelected.Titular = ""
                    NewCuentasClientesSelected.Usuario = Program.Usuario
                    NewCuentasClientesSelected.Activo = True
                    NewCuentasClientesSelected.logTitular = False
                    NewCuentasClientesSelected.Habilita = True
                    NewCuentasClientesSelected.logexcluirInteresDividendo = False
                    If IsNothing(ListaCuentasClientes) Then
                        ListaCuentasClientes = dcProxy.CuentasClientes
                    End If
                    ListaCuentasClientes.Add(NewCuentasClientesSelected)
                    CuentasClientesSelected = NewCuentasClientesSelected
                    MyBase.CambioItem("CuentasClientesSelected")
                    MyBase.CambioItem("ListaCuentasClientes")
                Case "cmOrdenantes"
                    Dim NewClientesOrdenantesSelected As New OyDClientes.ClientesOrdenante
                    If Not IsNothing(ListaClientesOrdenantes) Then
                        If ListaClientesOrdenantes.Count = 0 Then
                            If ClienteSelected.TipoVinculacion.Equals("A") Or ClienteSelected.TipoVinculacion.Equals("O") Then
                                NewClientesOrdenantesSelected.IDComitente = ClienteSelected.IDComitente
                                NewClientesOrdenantesSelected.IDOrdenante = ClienteSelected.IDComitente
                                NewClientesOrdenantesSelected.Nombre = ClienteSelected.Nombre
                                NewClientesOrdenantesSelected.Usuario = Program.Usuario
                                NewClientesOrdenantesSelected.lider = True
                                NewClientesOrdenantesSelected.Asociados = 0
                                NewClientesOrdenantesSelected.Relacionado = False
                                ListaClientesOrdenantes.Add(NewClientesOrdenantesSelected)
                                ClientesOrdenantesSelected = NewClientesOrdenantesSelected
                            Else
                                NewClientesOrdenantesSelected.IDComitente = ClienteSelected.IDComitente
                                NewClientesOrdenantesSelected.Usuario = Program.Usuario
                                NewClientesOrdenantesSelected.Asociados = 0
                                NewClientesOrdenantesSelected.lider = False
                                NewClientesOrdenantesSelected.Relacionado = False
                                NewClientesOrdenantesSelected.Habilita = True
                                If IsNothing(ListaClientesOrdenantes) Then
                                    ListaClientesOrdenantes = dcProxy.ClientesOrdenantes
                                End If
                                ListaClientesOrdenantes.Add(NewClientesOrdenantesSelected)
                                ClientesOrdenantesSelected = NewClientesOrdenantesSelected
                            End If
                        Else
                            NewClientesOrdenantesSelected.IDComitente = ClienteSelected.IDComitente
                            NewClientesOrdenantesSelected.Usuario = Program.Usuario
                            NewClientesOrdenantesSelected.Asociados = 0
                            NewClientesOrdenantesSelected.lider = False
                            NewClientesOrdenantesSelected.Relacionado = False
                            NewClientesOrdenantesSelected.Habilita = True
                            If IsNothing(ListaClientesOrdenantes) Then
                                ListaClientesOrdenantes = dcProxy.ClientesOrdenantes
                            End If
                            ListaClientesOrdenantes.Add(NewClientesOrdenantesSelected)
                            ClientesOrdenantesSelected = NewClientesOrdenantesSelected
                        End If
                        MyBase.CambioItem("ClientesOrdenantesSelected")
                        MyBase.CambioItem("ListaClientesOrdenantes")
                    End If
                Case "cmBeneficiarios"
                    Dim NewClientesBeneficiarioSelected As New OyDClientes.ClientesBeneficiarios
                    NewClientesBeneficiarioSelected.IDComitente = ClienteSelected.IDComitente
                    NewClientesBeneficiarioSelected.Usuario = Program.Usuario
                    NewClientesBeneficiarioSelected.Activo = True
                    NewClientesBeneficiarioSelected.Habilita = True
                    If IsNothing(ListaClientesBeneficiarios) Then
                        ListaClientesBeneficiarios = dcProxy.ClientesBeneficiarios
                    End If
                    ListaClientesBeneficiarios.Add(NewClientesBeneficiarioSelected)
                    ClientesBeneficiarioSelected = NewClientesBeneficiarioSelected
                    MyBase.CambioItem("ClientesBeneficiarioSelected")
                    MyBase.CambioItem("ListaClientesBeneficiarios")
                Case "cmTiposdeEntidad"
                    If IsNothing(ListaClientesTipoCliente) Then
                        ListaClientesTipoCliente = dcProxy.TipoClients
                    End If
                    If ListaClientesTipoCliente.Count = 0 Then
                        Dim NewClientesTipoClienteSelected As New OyDClientes.TipoClient
                        NewClientesTipoClienteSelected.IDComitente = ClienteSelected.IDComitente
                        NewClientesTipoClienteSelected.Usuario = Program.Usuario
                        ListaClientesTipoCliente.Add(NewClientesTipoClienteSelected)
                        ClientesTipoClienteSelected = NewClientesTipoClienteSelected
                        MyBase.CambioItem("ClientesTipoClienteSelected")
                        MyBase.CambioItem("ListaClientesBeneficiarios")
                    Else
                        A2Utilidades.Mensajes.mostrarMensaje("Sólo puedes agregar un detalle.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                Case "cmUbicacion"
                    Dim NewClientesDireccdioneSelected As New OyDClientes.ClientesDireccione
                    NewClientesDireccdioneSelected.IDComitente = ClienteSelected.IDComitente
                    NewClientesDireccdioneSelected.Usuario = Program.Usuario
                    NewClientesDireccdioneSelected.Activo = True
                    NewClientesDireccdioneSelected.DireccionEnvio = False
                    NewClientesDireccdioneSelected.intClave_PorAprobar = -1
                    If IsNothing(ListaClientesDirecciones) Then
                        ListaClientesDirecciones = dcProxy.ClientesDirecciones
                    End If
                    ListaClientesDirecciones.Add(NewClientesDireccdioneSelected)
                    ClientesDireccionesSelected = NewClientesDireccdioneSelected
                    MyBase.CambioItem("ClientesDireccionesSelected")
                    MyBase.CambioItem("ListaClientesDirecciones")
                    ClientesSeBoolean.Read = False

                    'CFMA20170130 se corrige la logica ya que el sistema presentaba inconsistencias al momento de guardar un nuevo registro 
                Case "cmAccionistas"
                    Dim NewClientesAccionistasSelected As New OyDClientes.ClientesAccionistas
                    If Not IsNothing(ListaClientesAccionistas) Then
                        NewClientesAccionistasSelected.IDClientesAccionistas = -(_ListaClientesAccionistas.Count() + 1)
                    Else
                        NewClientesAccionistasSelected.IDClientesAccionistas = -1
                    End If
                    'CFMA20170130

                    'CFMA20160614 Se corrige código ya que al ingresar un nuevo detalle de accionista no se asignaba ningún valor para el campo participación y llegaba null a la condición
                    NewClientesAccionistasSelected.IDComitente = ClienteSelected.IDComitente
                    NewClientesAccionistasSelected.Usuario = Program.Usuario
                    NewClientesAccionistasSelected.AplicaFatca = False
                    NewClientesAccionistasSelected.Habilita = True
                    NewClientesAccionistasSelected.Participacion = 0
                    If IsNothing(ListaClientesAccionistas) Then
                        ListaClientesAccionistas = dcProxy.ClientesAccionistas
                    End If
                    ListaClientesAccionistas.Add(NewClientesAccionistasSelected)
                    ClientesAccionistasSelected = NewClientesAccionistasSelected
                    MyBase.CambioItem("ClientesAccionistasSelected")
                    MyBase.CambioItem("ListaClientesAccionistas")
                Case "cmFichacliente"
                    Dim NewClientesFichaSelected As New OyDClientes.ClientesFicha
                    NewClientesFichaSelected.IDComitente = ClienteSelected.IDComitente
                    NewClientesFichaSelected.Descripcion = ""
                    NewClientesFichaSelected.CodDocumento = ""
                    NewClientesFichaSelected.Usuario = Program.Usuario
                    NewClientesFichaSelected.Habilita = True
                    If IsNothing(ListaClientesFicha) Then
                        ListaClientesFicha = dcProxy.ClientesFichas
                    End If
                    ListaClientesFicha.Add(NewClientesFichaSelected)
                    ClientesFichaSelected = NewClientesFichaSelected
                    MyBase.CambioItem("ClientesFichaSelected")
                    MyBase.CambioItem("ListaClientesFicha")
                Case "cmPersona"
                    Dim NewClientesPersonaSelected As New OyDClientes.ClientesPersonasParaConfirmar
                    NewClientesPersonaSelected.IDComitente = ClienteSelected.IDComitente
                    NewClientesPersonaSelected.Nombre1 = ""
                    NewClientesPersonaSelected.Usuario = Program.Usuario
                    NewClientesPersonaSelected.Habilita = True
                    If IsNothing(ListaClientesPersonas) Then
                        ListaClientesPersonas = dcProxy.ClientesPersonasParaConfirmars
                    End If
                    ListaClientesPersonas.Add(NewClientesPersonaSelected)
                    ClientesPersonaselected = NewClientesPersonaSelected
                    MyBase.CambioItem("ClientesPersonaselected")
                    MyBase.CambioItem("ListaClientesPersonas")
                Case "cmDepEconomica"
                    Dim NewClientesDepEconomicaSelected As New OyDClientes.ClientesPersonasDepEconomica
                    NewClientesDepEconomicaSelected.IDComitente = ClienteSelected.IDComitente
                    NewClientesDepEconomicaSelected.NroDocumento = ""
                    NewClientesDepEconomicaSelected.Nombre1 = ""
                    NewClientesDepEconomicaSelected.Usuario = Program.Usuario
                    NewClientesDepEconomicaSelected.Habilita = True
                    NewClientesDepEconomicaSelected.Activo = False
                    If IsNothing(ListaClientesDepEconomica) Then
                        ListaClientesDepEconomica = dcProxy.ClientesPersonasDepEconomicas
                    End If
                    ListaClientesDepEconomica.Add(NewClientesDepEconomicaSelected)
                    ClientesDepEconomicaselected = NewClientesDepEconomicaSelected
                    MyBase.CambioItem("ClientesPersonaselected")
                    MyBase.CambioItem("ListaClientesPersonas")
                Case "cmDetalleFATCA"
                    Dim newClienteFatca As New OyDClientes.ClientesPaisesFATCA
                    newClienteFatca.IDComitente = ClienteSelected.IDComitente
                    newClienteFatca.Actualizacion = Now
                    newClienteFatca.Usuario = Program.Usuario
                    newClienteFatca.Habilita = True
                    If IsNothing(ListaClientesPaisesFATCA) Then
                        ListaClientesPaisesFATCA = dcProxy.ClientesPaisesFATCAs
                    End If
                    newClienteFatca.IDClientesPaises = -ListaClientesPaisesFATCA.Count

                    ListaClientesPaisesFATCA.Add(newClienteFatca)
                    ClientesPaisesFATCAselected = newClienteFatca
                    MyBase.CambioItem("ClientesPaisesFATCAselected")
                    MyBase.CambioItem("ListaClientesPaisesFATCA")
            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creacion del detalle",
                                   Me.ToString(), "NuevoRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Overrides Sub BorrarRegistroDetalle()
        Try
            'A2Utilidades.Mensajes.mostrarMensaje("Esta Funcionalidad No Esta Habilitada.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
            Select Case NombreColeccionDetalle
                Case "cmReceptores"
                    If Not IsNothing(ListaClientesReceptore) Then
                        If ListaClientesReceptore.Count > 0 Then
                            If Not IsNothing(_ClientesReceptoreSelected) Then
                                Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ClientesReceptoreSelected, ListaClientesReceptore.ToList)
                                'cambio realizado en esta linea debido a un error interno de component one
                                ListaClientesReceptore.Remove(_ListaClientesReceptore.Where(Function(i) i.IDReceptor = _ClientesReceptoreSelected.IDReceptor).First)
                                If ListaClientesReceptore.Count > 0 Then
                                    Program.PosicionarItemLista(ClientesReceptoreSelected, ListaClientesReceptore.ToList, intRegistroPosicionar)
                                Else
                                    ClientesReceptoreSelected = Nothing
                                End If
                                MyBase.CambioItem("ClientesReceptoreSelected")
                                MyBase.CambioItem("ListaClientesReceptore")
                            Else
                                'A2Utilidades.Mensajes.mostrarMensaje("Debes elegir un Receptor para eliminar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                ListaClientesReceptore.Remove(ListaClientesReceptore.FirstOrDefault)

                                If ListaClientesReceptore.Count > 0 Then
                                    ClientesReceptoreSelected = ListaClientesReceptore.FirstOrDefault
                                Else
                                    ClientesReceptoreSelected = Nothing
                                End If
                            End If
                        End If
                    End If
                Case "cmCtasBancarias"
                    If Not IsNothing(ListaCuentasClientes) Then
                        If ListaCuentasClientes.Count > 0 Then
                            If Not IsNothing(_CuentasClientesSelected) Then
                                Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(CuentasClientesSelected, ListaCuentasClientes.ToList)
                                'ListaCuentasClientes.Remove(_CuentasClientesSelected)
                                ListaCuentasClientes.Remove(_ListaCuentasClientes.Where(Function(i) i.IDCuentasclientes = _CuentasClientesSelected.IDCuentasclientes).First)

                                If ListaCuentasClientes.Count > 0 Then
                                    Program.PosicionarItemLista(CuentasClientesSelected, ListaCuentasClientes.ToList, intRegistroPosicionar)
                                Else
                                    CuentasClientesSelected = Nothing
                                End If
                                MyBase.CambioItem("CuentasClientesSelected")
                                MyBase.CambioItem("ListaCuentasClientes")
                            Else
                                'A2Utilidades.Mensajes.mostrarMensaje("Debes elegir una cuenta bancaria para eliminar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                ListaCuentasClientes.Remove(ListaCuentasClientes.FirstOrDefault)

                                If ListaCuentasClientes.Count > 0 Then
                                    CuentasClientesSelected = ListaCuentasClientes.FirstOrDefault
                                Else
                                    CuentasClientesSelected = Nothing
                                End If
                            End If
                        End If
                    End If
                Case "cmOrdenantes"
                    If Not IsNothing(ListaClientesOrdenantes) Then
                        If ListaClientesOrdenantes.Count > 0 Then
                            If Not IsNothing(_ClientesOrdenantesSelected) Then
                                Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ClientesOrdenantesSelected, ListaClientesOrdenantes.ToList)
                                'ListaClientesOrdenantes.Remove(_ClientesOrdenantesSelected)
                                If _ListaClientesOrdenantes.Where(Function(i) i.IDClientesOrdenantes = _ClientesOrdenantesSelected.IDClientesOrdenantes).Count > 0 Then
                                    ListaClientesOrdenantes.Remove(_ListaClientesOrdenantes.Where(Function(i) i.IDOrdenante = _ClientesOrdenantesSelected.IDOrdenante).First)
                                End If

                                If ListaClientesOrdenantes.Count > 0 Then
                                    Program.PosicionarItemLista(CuentasClientesSelected, ListaCuentasClientes.ToList, intRegistroPosicionar)
                                Else
                                    ClientesOrdenantesSelected = Nothing
                                End If
                                MyBase.CambioItem("ClientesOrdenantesSelected")
                                MyBase.CambioItem("ListaClientesOrdenantes")
                            Else
                                'A2Utilidades.Mensajes.mostrarMensaje("Debes elegir un ordenante  para eliminar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                ListaClientesOrdenantes.Remove(ListaClientesOrdenantes.FirstOrDefault)

                                If ListaClientesOrdenantes.Count > 0 Then
                                    ClientesOrdenantesSelected = ListaClientesOrdenantes.FirstOrDefault
                                Else
                                    ClientesOrdenantesSelected = Nothing
                                End If
                            End If
                        End If
                    End If
                Case "cmBeneficiarios"
                    If Not IsNothing(ListaClientesBeneficiarios) Then
                        If ListaClientesBeneficiarios.Count > 0 Then
                            If Not IsNothing(_ClientesBeneficiarioSelected) Then
                                Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ClientesBeneficiarioSelected, ListaClientesBeneficiarios.ToList)
                                'ListaClientesBeneficiarios.Remove(_ClientesBeneficiarioSelected)
                                ListaClientesBeneficiarios.Remove(_ListaClientesBeneficiarios.Where(Function(i) i.IDBeneficiarios = _ClientesBeneficiarioSelected.IDBeneficiarios).First)

                                If ListaClientesBeneficiarios.Count > 0 Then
                                    Program.PosicionarItemLista(ClientesBeneficiarioSelected, ListaClientesBeneficiarios.ToList, intRegistroPosicionar)
                                Else
                                    ClientesBeneficiarioSelected = Nothing
                                End If
                                MyBase.CambioItem("ClientesBeneficiarioSelected")
                                MyBase.CambioItem("ListaClientesBeneficiarios")
                            Else
                                'A2Utilidades.Mensajes.mostrarMensaje("Debes elegir un beneficiario  para eliminar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                ListaClientesBeneficiarios.Remove(ListaClientesBeneficiarios.FirstOrDefault)

                                If ListaClientesBeneficiarios.Count > 0 Then
                                    ClientesBeneficiarioSelected = ListaClientesBeneficiarios.FirstOrDefault
                                Else
                                    ClientesBeneficiarioSelected = Nothing
                                End If
                            End If
                        End If
                    End If
                Case "cmTiposdeEntidad"
                    If Not IsNothing(ListaClientesTipoCliente) Then
                        If ListaClientesTipoCliente.Count > 0 Then
                            If Not IsNothing(_ClientesTipoClienteSelected) Then
                                Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ClientesTipoClienteSelected, ListaClientesTipoCliente.ToList)
                                'ListaClientesTipoCliente.Remove(_ClientesTipoClienteSelected)
                                ListaClientesTipoCliente.Remove(_ListaClientesTipoCliente.Where(Function(i) i.IDTipoCliente = _ClientesTipoClienteSelected.IDTipoCliente).First)

                                If ListaClientesTipoCliente.Count > 0 Then
                                    Program.PosicionarItemLista(ClientesTipoClienteSelected, ListaClientesTipoCliente.ToList, intRegistroPosicionar)
                                Else
                                    ClientesTipoClienteSelected = Nothing
                                End If
                                MyBase.CambioItem("ClientesTipoClienteSelected")
                                MyBase.CambioItem("ListaClientesTipoCliente")
                            Else
                                ' A2Utilidades.Mensajes.mostrarMensaje("Debes elegir un tipo de entidad  para eliminar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                ListaClientesTipoCliente.Remove(ListaClientesTipoCliente.FirstOrDefault)

                                If ListaClientesTipoCliente.Count > 0 Then
                                    ClientesTipoClienteSelected = ListaClientesTipoCliente.FirstOrDefault
                                Else
                                    ClientesTipoClienteSelected = Nothing
                                End If
                            End If
                        End If
                    End If
                Case "cmUbicacion"
                    If Not IsNothing(ListaClientesDirecciones) Then
                        If ListaClientesDirecciones.Count > 0 Then
                            If Not IsNothing(_ClientesDireccionesSelected) Then
                                Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ClientesDireccionesSelected, ListaClientesDirecciones.ToList)
                                'ListaClientesDirecciones.Remove(_ClientesDireccionesSelected)
                                If ClientesDireccionesSelected.Tipo = "C" And ClienteSelected.TipoPersona = 1 Then
                                    ClienteSelected.Direccion = String.Empty
                                ElseIf ClientesDireccionesSelected.Tipo = "O" Then
                                    If ClienteSelected.TipoPersona = 2 Then
                                        ClienteSelected.Direccion = String.Empty
                                    End If
                                    ClienteSelected.DireccionOficina = String.Empty
                                End If
                                If _ListaClientesDirecciones.Where(Function(i) i.Consecutivo = _ClientesDireccionesSelected.Consecutivo).Count > 0 Then
                                    ListaClientesDirecciones.Remove(_ListaClientesDirecciones.Where(Function(i) i.Consecutivo = _ClientesDireccionesSelected.Consecutivo).First)
                                End If

                                'ClientesDireccionesSelected = ListaClientesDirecciones.LastOrDefault
                                If ListaClientesDirecciones.Count > 0 Then
                                    Program.PosicionarItemLista(ClientesDireccionesSelected, ListaClientesDirecciones.ToList, intRegistroPosicionar)
                                Else
                                    ClientesDireccionesSelected = Nothing
                                End If
                                MyBase.CambioItem("ClientesDireccionesSelected")
                                MyBase.CambioItem("ListaClientesDirecciones")
                            Else
                                'A2Utilidades.Mensajes.mostrarMensaje("Debes elegir una direccion  para eliminar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                If ListaClientesDirecciones.Count > 0 Then
                                    ListaClientesDirecciones.Remove(ListaClientesDirecciones.FirstOrDefault)

                                    If ListaClientesDirecciones.Count > 0 Then
                                        ClientesDireccionesSelected = ListaClientesDirecciones.FirstOrDefault
                                    Else
                                        ClientesDireccionesSelected = Nothing
                                    End If
                                End If
                            End If
                        End If
                    End If
                Case "cmAccionistas"
                    If Not IsNothing(ListaClientesAccionistas) Then
                        If ListaClientesAccionistas.Count > 0 Then
                            If Not IsNothing(_ClientesAccionistasSelected) Then
                                Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ClientesAccionistasSelected, ListaClientesAccionistas.ToList)
                                'ListaClientesAccionistas.Remove(_ClientesAccionistasSelected)
                                ListaClientesAccionistas.Remove(_ListaClientesAccionistas.Where(Function(i) i.IDClientesAccionistas = _ClientesAccionistasSelected.IDClientesAccionistas).First)

                                If ListaClientesAccionistas.Count > 0 Then
                                    Program.PosicionarItemLista(ClientesAccionistasSelected, ListaClientesAccionistas.ToList, intRegistroPosicionar)
                                Else
                                    ClientesAccionistasSelected = Nothing
                                End If
                                MyBase.CambioItem("ClientesAccionistasSelected")
                                MyBase.CambioItem("ListaClientesAccionistas")
                            Else
                                'A2Utilidades.Mensajes.mostrarMensaje("Debes elegir un beneficiario  para eliminar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                ListaClientesAccionistas.Remove(ListaClientesAccionistas.FirstOrDefault)

                                If ListaClientesAccionistas.Count > 0 Then
                                    ClientesAccionistasSelected = ListaClientesAccionistas.FirstOrDefault
                                Else
                                    ClientesAccionistasSelected = Nothing
                                End If
                            End If
                        End If
                    End If
                Case "cmFichacliente"
                    If Not IsNothing(ListaClientesFicha) Then
                        If ListaClientesFicha.Count > 0 Then
                            If Not IsNothing(_ClientesFichaSelected) Then
                                Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ClientesFichaSelected, ListaClientesFicha.ToList)
                                'ListaClientesFicha.Remove(_ClientesFichaSelected)
                                ListaClientesFicha.Remove(_ListaClientesFicha.Where(Function(i) i.IDClientesFicha = _ClientesFichaSelected.IDClientesFicha).First)

                                If ListaClientesFicha.Count > 0 Then
                                    Program.PosicionarItemLista(ClientesFichaSelected, ListaClientesFicha.ToList, intRegistroPosicionar)
                                Else
                                    ClientesFichaSelected = Nothing
                                End If
                                MyBase.CambioItem("ClientesFichaSelected")
                                MyBase.CambioItem("ListaClientesFicha")
                            Else
                                'A2Utilidades.Mensajes.mostrarMensaje("Debes elegir un beneficiario  para eliminar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                ListaClientesFicha.Remove(ListaClientesFicha.FirstOrDefault)

                                If ListaClientesFicha.Count > 0 Then
                                    ClientesFichaSelected = ListaClientesFicha.FirstOrDefault
                                Else
                                    ClientesFichaSelected = Nothing
                                End If
                            End If
                        End If
                    End If
                Case "cmPersona"
                    If Not IsNothing(ListaClientesPersonas) Then
                        If ListaClientesPersonas.Count > 0 Then
                            If Not IsNothing(_ClientesPersonaselected) Then
                                Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ClientesPersonaselected, ListaClientesPersonas.ToList)
                                'ListaClientesPersonas.Remove(_ClientesPersonaselected)
                                ListaClientesPersonas.Remove(_ListaClientesPersonas.Where(Function(i) i.IDClientesPersonas = _ClientesPersonaselected.IDClientesPersonas).First)

                                If ListaClientesPersonas.Count > 0 Then
                                    Program.PosicionarItemLista(ClientesPersonaselected, ListaClientesPersonas.ToList, intRegistroPosicionar)
                                Else
                                    ClientesPersonaselected = Nothing
                                End If
                                MyBase.CambioItem("ClientesPersonaselected")
                                MyBase.CambioItem("ListaClientesPersonas")
                            Else
                                'A2Utilidades.Mensajes.mostrarMensaje("Debes elegir un beneficiario  para eliminar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                ListaClientesPersonas.Remove(ListaClientesPersonas.FirstOrDefault)

                                If ListaClientesPersonas.Count > 0 Then
                                    ClientesPersonaselected = ListaClientesPersonas.FirstOrDefault
                                Else
                                    ClientesPersonaselected = Nothing
                                End If
                            End If
                        End If
                    End If
                Case "cmDepEconomica"
                    If Not IsNothing(ListaClientesDepEconomica) Then
                        If ListaClientesDepEconomica.Count > 0 Then
                            If Not IsNothing(_ClientesDepEconomicaselected) Then
                                Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ClientesDepEconomicaselected, ListaClientesDepEconomica.ToList)
                                'ListaClientesDepEconomica.Remove(_ClientesDepEconomicaselected)
                                ListaClientesDepEconomica.Remove(_ListaClientesDepEconomica.Where(Function(i) i.IDClientesPersDepEco = _ClientesDepEconomicaselected.IDClientesPersDepEco).First)

                                If ListaClientesDepEconomica.Count > 0 Then
                                    Program.PosicionarItemLista(ClientesDepEconomicaselected, ListaClientesDepEconomica.ToList, intRegistroPosicionar)
                                Else
                                    ClientesDepEconomicaselected = Nothing
                                End If
                                MyBase.CambioItem("ClientesDepEconomicaselected")
                                MyBase.CambioItem("ListaClientesDepEconomica")
                            Else
                                'A2Utilidades.Mensajes.mostrarMensaje("Debes elegir un beneficiario  para eliminar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                ListaClientesDepEconomica.Remove(ListaClientesDepEconomica.FirstOrDefault)

                                If ListaClientesDepEconomica.Count > 0 Then
                                    ClientesDepEconomicaselected = ListaClientesDepEconomica.FirstOrDefault
                                Else
                                    ClientesDepEconomicaselected = Nothing
                                End If
                            End If
                        End If
                    End If
                Case "cmDetalleFATCA"
                    If Not IsNothing(ListaClientesPaisesFATCA) Then
                        If ListaClientesPaisesFATCA.Count > 0 Then
                            If Not IsNothing(_ClientesPaisesFATCAselected) Then
                                Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(ClientesPaisesFATCAselected, ListaClientesPaisesFATCA.ToList)
                                'ListaClientesDepEconomica.Remove(_ClientesDepEconomicaselected)
                                ListaClientesPaisesFATCA.Remove(_ListaClientesPaisesFATCA.Where(Function(i) i.IDClientesPaises = _ClientesPaisesFATCAselected.IDClientesPaises).First)

                                If ListaClientesPaisesFATCA.Count > 0 Then
                                    Program.PosicionarItemLista(ClientesPaisesFATCAselected, ListaClientesPaisesFATCA.ToList, intRegistroPosicionar)
                                Else
                                    ClientesPaisesFATCAselected = Nothing
                                End If
                                MyBase.CambioItem("ClientesPaisesFATCAselected")
                                MyBase.CambioItem("ListaClientesPaisesFATCA")
                            Else
                                'A2Utilidades.Mensajes.mostrarMensaje("Debes elegir un beneficiario  para eliminar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                ListaClientesPaisesFATCA.Remove(ListaClientesPaisesFATCA.FirstOrDefault)

                                If ListaClientesPaisesFATCA.Count > 0 Then
                                    ClientesPaisesFATCAselected = ListaClientesPaisesFATCA.FirstOrDefault
                                Else
                                    ClientesPaisesFATCAselected = Nothing
                                End If
                            End If
                        End If
                    End If
            End Select
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el borrado del detalle",
                                   Me.ToString(), "BorrarRegistroDetalle", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
#End Region
End Class
'Clase base para forma de búsquedas
Public Class CamposBusquedaCliente
    Implements INotifyPropertyChanged
    Dim mobjVM As ClientesViewModel
    Public Sub New(ByRef pobjVMPadre As ClientesViewModel)
        mobjVM = pobjVMPadre
    End Sub
    <StringLength(17, ErrorMessage:="La longitud máxima es de 17")>
    <Display(Name:="Código")>
    Private _IDComitente As String
    Public Property IDComitente As String
        Get
            Return _IDComitente
        End Get
        Set(value As String)
            _IDComitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDComitente"))
        End Set
    End Property

    <StringLength(50, ErrorMessage:="La longitud máxima es de 50")>
    <Display(Name:="Nombre")>
    Private _Nombre As String
    Public Property Nombre As String
        Get
            Return _Nombre
        End Get
        Set(value As String)
            _Nombre = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Nombre"))
        End Set
    End Property

    <StringLength(50, ErrorMessage:="La longitud máxima es de 15")>
    <Display(Name:="Nro Documento")>
    Private _strNroDocumento As String
    Public Property strNroDocumento As String
        Get
            Return _strNroDocumento
        End Get
        Set(value As String)
            _strNroDocumento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strNroDocumento"))
        End Set
    End Property

    <Display(Name:="Tipo Identificación")>
    Private _TipoIdentificacion As String
    Public Property TipoIdentificacion As String
        Get
            Return _TipoIdentificacion
        End Get
        Set(value As String)
            _TipoIdentificacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoIdentificacion"))
        End Set
    End Property

    <Display(Name:="Clasificación")>
    Private _Clasificacion As String
    Public Property Clasificacion As String
        Get
            Return _Clasificacion
        End Get
        Set(value As String)
            _Clasificacion = value
            If value = "1" Then
                If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                    mobjVM.objTipoIdBuscar = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOIDC1")
                End If
            ElseIf value = "2" Then
                If Application.Current.Resources.Contains(Program.NOMBRE_LISTA_COMBOS) Then
                    mobjVM.objTipoIdBuscar = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("TIPOIDC2")

                End If
            End If
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Clasificacion"))
        End Set
    End Property

    <Display(Name:="Filtro")>
    Public Property Filtro As Byte

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class
Public Class ListaOriginalClientesConocimientoEspecifico
    Implements INotifyPropertyChanged

    '<StringLength(15, ErrorMessage:="La longitud máxima es de 15")> _
    <Display(Name:="ID", Description:="ID")>
    Public Property ID As Char

    <Display(Name:="CodigoConocimiento", Description:="CodigoConocimiento")>
    Public Property CodigoConocimiento As Integer

    <Display(Name:="Usuario", Description:="Usuario")>
    Public Property Usuario As String

    <Display(Name:="DescripcionConocimiento", Description:="DescripcionConocimiento")>
    Public Property DescripcionConocimiento As String

    Private _Conocimiento As Boolean
    <Display(Name:="Conocimiento", Description:="Conocimiento")>
    Public Property Conocimiento As Boolean
        Get
            Return _Conocimiento
        End Get
        Set(ByVal value As Boolean)
            _Conocimiento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Conocimiento"))
        End Set
    End Property
    <Display(Name:="InfoSesion", Description:="InfoSesion")>
    Public Property InfoSesion As String

    <Display(Name:="IDClientesConocimiento", Description:="IDClientesConocimiento")>
    Public Property IDClientesConocimiento As Integer

    Private _ConocimientoOriginal As Boolean
    <Display(Name:="ConocimientoOriginal")>
    Public Property ConocimientoOriginal As Boolean
        Get
            Return _ConocimientoOriginal
        End Get
        Set(ByVal value As Boolean)
            _ConocimientoOriginal = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ConocimientoOriginal"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
Public Class HistoriaIR
    <Display(Name:="ObjetivoInversion")>
    Public Property ObjetivoInversion As String

    <Display(Name:="HorizonteInversion")>
    Public Property HorizonteInversion As String

    <Display(Name:="EdadCliente")>
    Public Property EdadCliente As String

    <Display(Name:="ConocimientoExperiencia")>
    Public Property ConocimientoExperiencia As String

    <Display(Name:="ClasificacionInversionista")>
    Public Property ClasificacionInversionista As String

    <Display(Name:="Suitability")>
    Public Property Suitability As System.Nullable(Of Integer)
End Class
Public Class CiudadesGenerales
    Implements INotifyPropertyChanged
    Private _strPoblaciondoc As String
    <Display(Name:="Ciudad")>
    Public Property strPoblaciondoc As String
        Get
            Return _strPoblaciondoc
        End Get
        Set(ByVal value As String)
            _strPoblaciondoc = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strPoblaciondoc"))
        End Set
    End Property
    Private _strdepartamentoDoc As String
    <Display(Name:="Departamento")>
    Public Property strdepartamentoDoc As String
        Get
            Return _strdepartamentoDoc
        End Get
        Set(ByVal value As String)
            _strdepartamentoDoc = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strdepartamentoDoc"))
        End Set
    End Property
    Private _strPaisDoc As String
    <Display(Name:="Pais")>
    Public Property strPaisDoc As String
        Get
            Return _strPaisDoc
        End Get
        Set(ByVal value As String)
            _strPaisDoc = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strPaisDoc"))
        End Set
    End Property
    Private _strPoblacion As String
    <Display(Name:="Ciudad")>
    Public Property strPoblacion As String
        Get
            Return _strPoblacion
        End Get
        Set(ByVal value As String)
            _strPoblacion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strPoblacion"))
        End Set
    End Property
    Private _strdepartamento As String
    <Display(Name:="Departamento")>
    Public Property strdepartamento As String
        Get
            Return _strdepartamento
        End Get
        Set(ByVal value As String)
            _strdepartamento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strdepartamento"))
        End Set
    End Property
    Private _strPais As String
    <Display(Name:="Pais")>
    Public Property strPais As String
        Get
            Return _strPais
        End Get
        Set(ByVal value As String)
            _strPais = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strPais"))
        End Set
    End Property
    Private _strciudad As String
    <Display(Name:="Ciudad")>
    Public Property strciudad As String
        Get
            Return _strciudad
        End Get
        Set(ByVal value As String)
            _strciudad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strciudad"))
        End Set
    End Property
    Private _strciudadNacimiento As String
    <Display(Name:="Ciudad Nacimiento")>
    Public Property strciudadNacimiento As String
        Get
            Return _strciudadNacimiento
        End Get
        Set(ByVal value As String)
            _strciudadNacimiento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strciudadNacimiento"))
        End Set
    End Property
    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class
Public Class ClientesTotalesgroup

    <Display(Name:="Periodo")>
    Public Property Periodo As String

    <Display(Name:="Compras")>
    Public Property Compras As Double

    <Display(Name:="Ventas")>
    Public Property Ventas As Double

    <Display(Name:="Totales")>
    Public Property Totales As Double

    <Display(Name:="Categoría")>
    Public Property Categoria As String

End Class
Public Class ClientesSaldosG
    Implements INotifyPropertyChanged
    Private _Periodo As String
    <Display(Name:="Periodo")>
    Public Property Periodo As String
        Get
            Return _Periodo
        End Get
        Set(ByVal value As String)
            _Periodo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Periodo"))
        End Set
    End Property
    Private _Acargo As Double
    <Display(Name:="A Cargo")>
    Public Property Acargo As Double
        Get
            Return _Acargo
        End Get
        Set(ByVal value As Double)
            _Acargo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Acargo"))
        End Set
    End Property
    Private _Afavor As Double
    <Display(Name:="A Favor")>
    Public Property Afavor As Double
        Get
            Return _Afavor
        End Get
        Set(ByVal value As Double)
            _Afavor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Afavor"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
Public Class ClientesVisibility
    Implements INotifyPropertyChanged
    Private _TipoPersonaJuridica As Visibility = Visibility.Collapsed
    Public Property TipoPersonaJuridica As Visibility
        Get
            Return _TipoPersonaJuridica
        End Get
        Set(ByVal value As Visibility)
            _TipoPersonaJuridica = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoPersonaJuridica"))
        End Set
    End Property
    Private _TipoPersonaNatural As Visibility = Visibility.Collapsed
    Public Property TipoPersonaNatural As Visibility
        Get
            Return _TipoPersonaNatural
        End Get
        Set(ByVal value As Visibility)
            _TipoPersonaNatural = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoPersonaNatural"))
        End Set
    End Property
    Private _ReferenciaOtros As Visibility = Visibility.Collapsed
    Public Property ReferenciaOtros As Visibility
        Get
            Return _ReferenciaOtros
        End Get
        Set(ByVal value As Visibility)
            _ReferenciaOtros = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ReferenciaOtros"))
        End Set
    End Property
    Private _ReferenciaPor As Visibility = Visibility.Collapsed
    Public Property ReferenciaPor As Visibility
        Get
            Return _ReferenciaPor
        End Get
        Set(ByVal value As Visibility)
            _ReferenciaPor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ReferenciaPor"))
        End Set
    End Property
    Private _visibilidad As Visibility = Visibility.Collapsed
    Public Property visibilidad As Visibility
        Get
            Return _visibilidad
        End Get
        Set(ByVal value As Visibility)
            _visibilidad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("visibilidad"))
        End Set
    End Property
    Private _VisivilidadMenu As Visibility = Visibility.Collapsed
    Public Property VisivilidadMenu() As Visibility
        Get
            Return _VisivilidadMenu
        End Get
        Set(ByVal value As Visibility)
            _VisivilidadMenu = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("VisivilidadMenu"))
        End Set
    End Property
    Private _labelvisible As Visibility = Visibility.Collapsed
    Public Property labelvisible As Visibility
        Get
            Return _labelvisible
        End Get
        Set(ByVal value As Visibility)
            _labelvisible = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("labelvisible"))
        End Set
    End Property
    Private _ConfiguravisibleAccionistas As Visibility = Visibility.Collapsed
    Public Property ConfiguravisibleAccionistas As Visibility
        Get
            Return _ConfiguravisibleAccionistas
        End Get
        Set(ByVal value As Visibility)
            _ConfiguravisibleAccionistas = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ConfiguravisibleAccionistas"))
        End Set
    End Property
    Private _PersonaporConfigvi As Visibility = Visibility.Collapsed
    Public Property PersonaporConfigvi As Visibility
        Get
            Return _PersonaporConfigvi
        End Get
        Set(value As Visibility)
            _PersonaporConfigvi = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("PersonaporConfigvi"))
        End Set
    End Property
    Private _ConfiguraVisibleCity As Visibility = Visibility.Collapsed
    Public Property ConfiguraVisibleCity As Visibility
        Get
            Return _ConfiguraVisibleCity
        End Get
        Set(value As Visibility)
            _ConfiguraVisibleCity = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ConfiguraVisibleCity"))
        End Set
    End Property
    Private _Preclientevisible As Visibility = Visibility.Collapsed
    Public Property Preclientevisible As Visibility
        Get
            Return _Preclientevisible
        End Get
        Set(value As Visibility)
            _Preclientevisible = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Preclientevisible"))
        End Set
    End Property
    Private _RETEICAvisible As Visibility = Visibility.Collapsed
    Public Property RETEICAvisible As Visibility
        Get
            Return _RETEICAvisible
        End Get
        Set(value As Visibility)
            _RETEICAvisible = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("RETEICAvisible"))
        End Set
    End Property
    Private _AGORAvisible As Visibility = Visibility.Collapsed
    Public Property AGORAvisible As Visibility
        Get
            Return _AGORAvisible
        End Get
        Set(value As Visibility)
            _AGORAvisible = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("AGORAvisible"))
        End Set
    End Property
    Private _Pagocomisiones As Visibility = Visibility.Collapsed
    Public Property Pagocomisiones As Visibility
        Get
            Return _Pagocomisiones
        End Get
        Set(value As Visibility)
            _Pagocomisiones = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Pagocomisiones"))
        End Set
    End Property
    Private _VisiblePorcentaje As Visibility = Visibility.Collapsed
    Public Property VisiblePorcentaje As Visibility
        Get
            Return _VisiblePorcentaje
        End Get
        Set(value As Visibility)
            _VisiblePorcentaje = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("VisiblePorcentaje"))
        End Set
    End Property
    Private _TienebusVisible As Visibility = Visibility.Collapsed
    Public Property TienebusVisible As Visibility
        Get
            Return _TienebusVisible
        End Get
        Set(value As Visibility)
            _TienebusVisible = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TienebusVisible"))
        End Set
    End Property

    Private _visibletexocupa As Visibility = Visibility.Visible
    Public Property visibletexocupa As Visibility
        Get
            Return _visibletexocupa
        End Get
        Set(value As Visibility)
            _visibletexocupa = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("visibletexocupa"))
        End Set
    End Property
    Private _visiblecomocupa As Visibility = Visibility.Collapsed
    Public Property visiblecomocupa As Visibility
        Get
            Return _visiblecomocupa
        End Get
        Set(value As Visibility)
            _visiblecomocupa = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("visiblecomocupa"))
        End Set
    End Property

    Private _VisibleConceptoActividad As Visibility = Visibility.Collapsed
    Public Property VisibleConceptoActividad() As Visibility
        Get
            Return _VisibleConceptoActividad
        End Get
        Set(ByVal value As Visibility)
            _VisibleConceptoActividad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("VisibleConceptoActividad"))
        End Set
    End Property

    Private _VisibleConceptoInactividad As Visibility = Visibility.Collapsed
    Public Property VisibleConceptoInactividad() As Visibility
        Get
            Return _VisibleConceptoInactividad
        End Get
        Set(ByVal value As Visibility)
            _VisibleConceptoInactividad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("VisibleConceptoInactividad"))
        End Set
    End Property

    'NAOC20141118 - Fatca
    '**********************************************************************
    Private _FatcaCliente As Visibility = Visibility.Visible
    Public Property FatcaCliente() As Visibility
        Get
            Return _FatcaCliente
        End Get
        Set(ByVal value As Visibility)
            _FatcaCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FatcaCliente"))
        End Set
    End Property

    Private _TabFatca As Visibility = Visibility.Collapsed
    Public Property TabFatca() As Visibility
        Get
            Return _TabFatca
        End Get
        Set(ByVal value As Visibility)
            _TabFatca = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TabFatca"))
        End Set
    End Property

    'RBP20160411 Tab de Divisas Bancolombia
    Private _TabDivisasClientes As Visibility = Visibility.Collapsed
    Public Property TabDivisasClientes() As Visibility
        Get
            Return _TabDivisasClientes
        End Get
        Set(value As Visibility)
            _TabDivisasClientes = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TabDivisasClientes"))
        End Set
    End Property



    Private _TabFatcaRepresentanteLegal As Visibility = Visibility.Collapsed
    Public Property TabFatcaRepresentanteLegal() As Visibility
        Get
            Return _TabFatcaRepresentanteLegal
        End Get
        Set(ByVal value As Visibility)
            _TabFatcaRepresentanteLegal = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TabFatcaRepresentanteLegal"))
        End Set
    End Property
    '**********************************************************************
    Private _VisibleTipoCheque As Visibility = Visibility.Collapsed
    Public Property VisibleTipoCheque() As Visibility
        Get
            Return _VisibleTipoCheque
        End Get
        Set(ByVal value As Visibility)
            _VisibleTipoCheque = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("VisibleTipoCheque"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
Public Class ClientesBoolean
    Implements INotifyPropertyChanged
    Private _visible As Boolean
    Public Property visible As Boolean
        Get
            Return _visible
        End Get
        Set(ByVal value As Boolean)
            _visible = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("visible"))
        End Set
    End Property
    Private _visibleap As Boolean
    Public Property visibleap As Boolean
        Get
            Return _visibleap
        End Get
        Set(ByVal value As Boolean)
            _visibleap = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("visibleap"))
        End Set
    End Property
    Private _EsOperacionExt As Boolean = False
    Public Property EsOperacionExt As Boolean
        Get
            Return _EsOperacionExt
        End Get
        Set(ByVal value As Boolean)
            _EsOperacionExt = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("EsOperacionExt"))
        End Set
    End Property
    Private _HabilitaBus As Boolean = True
    Public Property HabilitaBus As Boolean
        Get
            Return _HabilitaBus
        End Get
        Set(ByVal value As Boolean)
            _HabilitaBus = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitaBus"))
        End Set
    End Property
    Private _Editarcampos As Boolean = False
    Public Property Editarcampos As Boolean
        Get
            Return _Editarcampos
        End Get
        Set(ByVal value As Boolean)
            _Editarcampos = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Editarcampos"))
        End Set
    End Property
    Private _Editareg As Boolean = False
    Public Property Editareg As Boolean
        Get
            Return _Editareg
        End Get
        Set(ByVal value As Boolean)
            _Editareg = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Editareg"))
        End Set
    End Property
    Private _Read As Boolean = True
    Public Property Read As Boolean
        Get
            Return _Read
        End Get
        Set(ByVal value As Boolean)
            _Read = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Read"))
        End Set
    End Property
    Private _Fondos As Boolean = False
    Public Property Fondos As Boolean
        Get
            Return _Fondos
        End Get
        Set(ByVal value As Boolean)
            _Fondos = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Fondos"))
        End Set
    End Property
    Private _Portafolios As Boolean = False
    Public Property Portafolios As Boolean
        Get
            Return _Portafolios
        End Get
        Set(ByVal value As Boolean)
            _Portafolios = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Portafolios"))
        End Set
    End Property
    Private _Clientes As Boolean = False
    Public Property Clientes As Boolean
        Get
            Return _Clientes
        End Get
        Set(ByVal value As Boolean)
            _Clientes = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Clientes"))
        End Set
    End Property
    Private _Mercasonft As Boolean = False
    Public Property Mercasonft As Boolean
        Get
            Return _Mercasonft
        End Get
        Set(ByVal value As Boolean)
            _Mercasonft = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Mercasonft"))
        End Set
    End Property
    Private _Comitente As Boolean = False
    Public Property Comitente As Boolean
        Get
            Return _Comitente
        End Get
        Set(ByVal value As Boolean)
            _Comitente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Comitente"))
        End Set
    End Property
    Private _IsbusyPortafolio As Boolean
    Public Property IsbusyPortafolio As Boolean
        Get
            Return _IsbusyPortafolio
        End Get
        Set(ByVal value As Boolean)
            _IsbusyPortafolio = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IsbusyPortafolio"))
        End Set
    End Property
    Private _EditarPreclientes As Boolean = False
    Public Property EditarPreclientes As Boolean
        Get
            Return _EditarPreclientes
        End Get
        Set(ByVal value As Boolean)
            _EditarPreclientes = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("EditarPreclientes"))
        End Set
    End Property

    Private _EditaInactividad As Boolean = False
    Public Property EditaInactividad As Boolean
        Get
            Return _EditaInactividad
        End Get
        Set(ByVal value As Boolean)
            _EditaInactividad = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("EditaInactividad"))
        End Set
    End Property

    Private _HabilitaDepEconomica As Boolean = False
    Public Property HabilitaDepEconomica As Boolean
        Get
            Return _HabilitaDepEconomica
        End Get
        Set(ByVal value As Boolean)
            _HabilitaDepEconomica = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitaDepEconomica"))
        End Set
    End Property
    Private _HabilitaCampoPNatural As Boolean = False
    Public Property HabilitaCampoPNatural As Boolean
        Get
            Return _HabilitaCampoPNatural
        End Get
        Set(ByVal value As Boolean)
            _HabilitaCampoPNatural = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitaCampoPNatural"))
        End Set
    End Property
    Private _HabilitaCampoPJuridica As Boolean = False
    Public Property HabilitaCampoPJuridica As Boolean
        Get
            Return _HabilitaCampoPJuridica
        End Get
        Set(ByVal value As Boolean)
            _HabilitaCampoPJuridica = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitaCampoPJuridica"))
        End Set
    End Property
    Private _Editanrdcto As Boolean = True
    Public Property Editanrdcto As Boolean
        Get
            Return _Editanrdcto
        End Get
        Set(ByVal value As Boolean)
            _Editanrdcto = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Editanrdcto"))
        End Set
    End Property
    Private _HabilitaAE As Boolean = False
    Public Property HabilitaAE As Boolean
        Get
            Return _HabilitaAE
        End Get
        Set(ByVal value As Boolean)
            _HabilitaAE = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("HabilitaAE"))
        End Set
    End Property
    'NAOC20141118 - Fatca
    '**********************************************************************
    Private _logAplicafatcaCliente As Boolean = False
    Public Property logAplicafatcaCliente As Boolean
        Get
            Return _logAplicafatcaCliente
        End Get
        Set(ByVal value As Boolean)
            _logAplicafatcaCliente = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logAplicafatcaCliente"))
        End Set
    End Property
    Private _logAplicafatcaRepresentante As Boolean = False
    Public Property logAplicafatcaRepresentante As Boolean
        Get
            Return _logAplicafatcaRepresentante
        End Get
        Set(ByVal value As Boolean)
            _logAplicafatcaRepresentante = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logAplicafatcaRepresentante"))
        End Set
    End Property
    'NAOC20141118 - Fatca
    '**********************************************************************
    Private _logTransfiereACuentasEEUU As Boolean = False
    Public Property logTransfiereACuentasEEUU() As Boolean
        Get
            Return _logTransfiereACuentasEEUU
        End Get
        Set(ByVal value As Boolean)
            _logTransfiereACuentasEEUU = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logTransfiereACuentasEEUU"))
        End Set
    End Property

    Private _logtranfierefondos As Boolean = False
    Public Property logtranfierefondos() As Boolean
        Get
            Return _logtranfierefondos
        End Get
        Set(ByVal value As Boolean)
            _logtranfierefondos = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logtranfierefondos"))
        End Set
    End Property

    Private _logEmpresaListadaEnBolsa As Boolean = False
    Public Property logEmpresaListadaEnBolsa() As Boolean
        Get
            Return _logEmpresaListadaEnBolsa
        End Get
        Set(ByVal value As Boolean)
            _logEmpresaListadaEnBolsa = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logEmpresaListadaEnBolsa"))
        End Set
    End Property

    Private _logSubsidiariaEntidadPublica As Boolean = False
    Public Property logSubsidiariaEntidadPublica() As Boolean
        Get
            Return _logSubsidiariaEntidadPublica
        End Get
        Set(ByVal value As Boolean)
            _logSubsidiariaEntidadPublica = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logSubsidiariaEntidadPublica"))
        End Set
    End Property

    Private _logInstitucionFinanciera As Boolean = False
    Public Property logInstitucionFinanciera() As Boolean
        Get
            Return _logInstitucionFinanciera
        End Get
        Set(ByVal value As Boolean)
            _logInstitucionFinanciera = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logInstitucionFinanciera"))
        End Set
    End Property

    'RBP20160125_Propiedad para habilitar campo descripción cuenta
    Private _logDescripcionCuenta As Boolean = False
    Public Property logDescripcionCuenta() As Boolean
        Get
            Return _logDescripcionCuenta
        End Get
        Set(ByVal value As Boolean)
            _logDescripcionCuenta = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logDescripcionCuenta"))
        End Set
    End Property
    '**********************************************************************
    'RBP20160513
    Private _logPoderParaFirmar As Boolean = False
    Public Property logPoderParaFirmar() As Boolean
        Get
            Return _logPoderParaFirmar
        End Get
        Set(ByVal value As Boolean)
            _logPoderParaFirmar = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logPoderParaFirmar"))
        End Set
    End Property


    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

End Class
Public Class CamposGenerales
    Implements INotifyPropertyChanged
    Private _SMMLV As String
    <Display(Name:="SMMLV")>
    Public Property SMMLV As String
        Get
            Return _SMMLV
        End Get
        Set(ByVal value As String)
            _SMMLV = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("SMMLV"))
        End Set
    End Property
    Private _valorenpesos As String
    <Display(Name:="valor en pesos SMMLV")>
    Public Property valorenpesos As String
        Get
            Return _valorenpesos
        End Get
        Set(ByVal value As String)
            _valorenpesos = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("valorenpesos"))
        End Set
    End Property
    Private _strProfesion As String
    <Display(Name:="Profesión")>
    Public Property strProfesion As String
        Get
            Return _strProfesion
        End Get
        Set(ByVal value As String)
            _strProfesion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strProfesion"))
        End Set
    End Property

    Private _strcodigociu As String
    <Display(Name:="Actividad Econo")>
    Public Property strcodigociu As String
        Get
            Return _strcodigociu
        End Get
        Set(ByVal value As String)
            _strcodigociu = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strcodigociu"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
Public Class CuentasClienteclase
    Public activo As Nullable(Of Boolean)

    Public codigoACH As String

    Public cuenta As String

    Public cuentaPpal As Boolean

    Public dividendos As Boolean

    Public idBanco As Nullable(Of Integer)

    Public idComisionista As Integer

    Public idCuentasclientes As Integer

    Public idSucComisionista As Integer

    Public intermedia As Nullable(Of Integer)

    Public logexcluirInteresDividendo As Nullable(Of Boolean)

    Public logTitular As Nullable(Of Boolean)

    Public nombreBanco As String

    Public nombreSucursal As String

    Public numeroID As String

    Public observacion As String

    Public operaciones As Boolean

    Public propietario As String

    Public referencia As Boolean

    Public tipoCuenta As String

    Public tipoDocumento As String

    Public tipoID As String

    Public titular As String

    Public transferirA As Boolean
End Class
Public Class ClientesDireccioneclase

    Public activo As Nullable(Of Boolean)

    Public ciudad As Nullable(Of Integer)

    Public direccion As String

    Public direccionEnvio As Nullable(Of Boolean)

    Public entregaA As String

    Public extension As String

    Public fax As String

    Public nombre As String

    Public telefono As Nullable(Of Long)

    Public tipo As String
End Class
Public Class ClientesBeneficiariosclase
    Public activo As Nullable(Of Boolean)

    Public apellido1 As String

    Public apellido2 As String

    Public beneficiarioCuentasDeposito As String

    Public direccion As String

    Public idCiudadDoc As Nullable(Of Integer)

    Public idCiudadDomicilio As Nullable(Of Integer)

    Public nombre As String

    Public nombre1 As String

    Public nombre2 As String

    Public nombreCiudadDoc As String

    Public nombreCiudadDomicilio As String

    Public nroDocumento As Decimal

    Public parentesco As String

    Public telefono As String

    Public tipoBeneficiario As String

    Public tipoID As String
End Class