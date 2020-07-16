Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.CFEspecies
Imports System.Windows.Data
Imports OpenRiaServices.DomainServices.Client
Imports System.Text
Imports System.Xml.Linq
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports A2.OyD.OYDServer.RIA.Web.OyDImportaciones
Imports System.Threading.Tasks
Imports A2ComunesImportaciones
Imports System.Globalization

Public Class ISINesViewModel
    Inherits A2ControlMenu.A2ViewModel

    Public Property cb As New CamposBusquedaISIN

    Private Const PARAM_STR_MOSTRAR_BARRA_BOTONES As String = "MOSTRAR_BARRA_BOTONES"
    Private Const MSTR_MC_ACCION_ACTUALIZAR As String = "U"
    Private Const PARAM_STR_EDITABLE_PORC_RETENCIONES = "CF_EDITABLEPORCRETENCIONESP"
    Private Const PARAM_STR_HABILITAR_MINMULTIPLO = "HabilitarMinMultiplo"

    Private dcProxyImportaciones As ImportacionesDomainContext
    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Private _nroRegistrosCargados As Integer = 0
    Private esp As Especi

    Public dcProxy As EspeciesCFDomainContext
    Public dcProxy1 As EspeciesCFDomainContext
    Public mdcProxyCalificacionValidar As EspeciesCFDomainContext
    Private EspeciesISINFungiblePorDefecto As EspeciesISINFungible
    Private EspeciesISINFungibleAnterior As EspeciesISINFungible

    Dim mlogLlamadoDesdeMaestro As Boolean = False
    Private logCambiarCambiarValarRetencion As Boolean = True
    Public strTipoCalculo As String = String.Empty
    Public LogValidarChanges As Boolean = False
    Dim logAmortizaPorDefecto As Boolean = False

#Region "Propiedades"

    Property vistaIsines As ISINesView

    Property blnDesdeEspecies As Boolean

    Property blnMinimoMult As Boolean


    Property ViewImportarArchivos As ImportarAmortizaciones

    Private _entEspeciesISINFungible As EntitySet(Of EspeciesISINFungible)
    Public Property EntEspeciesISINFungible() As EntitySet(Of EspeciesISINFungible)
        Get
            Return _entEspeciesISINFungible
        End Get
        Set(ByVal value As EntitySet(Of EspeciesISINFungible))
            _entEspeciesISINFungible = value
            MyBase.CambioItem("EntEspeciesISINFungible")
            MyBase.CambioItem("ListaEspeciesISINFungiblePaged")
        End Set
    End Property

    Private _intIdCalificacionC As Integer
    Public Property intIdCalificacionC() As Integer
        Get
            Return _intIdCalificacionC
        End Get
        Set(ByVal value As Integer)
            _intIdCalificacionC = value
            MyBase.CambioItem("intIdCalificacionC")
        End Set
    End Property

    Private _intIdCalificacionL As Integer
    Public Property intIdCalificacionL() As Integer
        Get
            Return _intIdCalificacionL
        End Get
        Set(ByVal value As Integer)
            _intIdCalificacionL = value
            MyBase.CambioItem("intIdCalificacionL")
        End Set
    End Property

    Public ReadOnly Property ListaEspeciesISINFungiblePaged() As PagedCollectionView
        Get
            If Not IsNothing(_entEspeciesISINFungible) Then
                Dim view = New PagedCollectionView(_entEspeciesISINFungible)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
    Private _ListaEspeciesISINFungibleAnterior As List(Of EspeciesISINFungible)
    Public Property ListaEspeciesISINFungibleAnterior() As List(Of EspeciesISINFungible)
        Get
            Return _ListaEspeciesISINFungibleAnterior
        End Get
        Set(ByVal value As List(Of EspeciesISINFungible))
            _ListaEspeciesISINFungibleAnterior = value
            MyBase.CambioItem("ListaEspeciesISINFungibleAnterior")
        End Set
    End Property

    ''' <history>
    ''' ID caso de prueba:   Id_21
    ''' Descripción:         Se comenta la instrucción "IsBusy = True" porque para evitar que haya que presionar doble click al link en modo lista.
    '''                      Se maneja con la variable mlogLlamadoDesdeMaestro la visibilidad de los objetos visRentaFija, visRentaFijaTF y visRentaFijaTV.
    ''' Responsable:         Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:               5 de Mayo/2014
    ''' Pruebas CB:          Jorge Peña (Alcuadrado S.A.) - 5 de Mayo/2014 - Pruebas OK
    ''' </history>
    ''' <history>
    ''' Descripción:        Se le envía a la consulta la fecha de emiíón y de vencimiento
    ''' Modificado:         Jhonatan Arley Acevedo Martínez (Alcuadrado S.A.)
    ''' Fecha:              14 de Septiembre/2015
    ''' </history>
    Private WithEvents _EspeciesISINFungibleSelected As EspeciesISINFungible
    Public Property EspeciesISINFungibleSelected() As EspeciesISINFungible
        Get
            Return _EspeciesISINFungibleSelected
        End Get
        Set(ByVal value As EspeciesISINFungible)
            If Not value Is Nothing Then
                'IsBusy = True
                _EspeciesISINFungibleSelected = value


                If Not String.IsNullOrEmpty(value.IDEspecie) Then
                    VisTextblock = Visibility.Visible
                    visBuscador = Visibility.Collapsed
                End If

                If mlogLlamadoDesdeMaestro Then
                    If Not EspeciesISINFungibleSelected.logEsAccion Then
                        If EspeciesISINFungibleSelected.strTipoTasaFija = "F" Then
                            visRentaFijaTF = Visibility.Visible
                            visRentaFijaTV = Visibility.Collapsed
                        Else
                            visRentaFijaTV = Visibility.Visible
                            visRentaFijaTF = Visibility.Collapsed
                        End If
                        visRentaFija = Visibility.Visible
                    Else
                        visRentaFija = Visibility.Collapsed
                        visRentaFijaTF = Visibility.Collapsed
                        visRentaFijaTV = Visibility.Collapsed
                    End If
                End If

                If Not IsNothing(EspeciesISINFungibleSelected) Then
                    If EspeciesISINFungibleSelected.logTituloCarteraColectiva Or logTituloCarteraColectiva Then
                        visTituloParticipativo = Visibility.Visible
                    ElseIf EspeciesISINFungibleSelected.logEsAccion Then
                        visTituloParticipativo = Visibility.Collapsed
                        EspeciesISINFungibleSelected.Fecha_Emision = Nothing
                        EspeciesISINFungibleSelected.Fecha_Vencimiento = Nothing
                    End If
                End If

                If _EspeciesISINFungibleSelected.Amortizada Then
                    VisAmortizaciones = Visibility.Visible
                    _EspeciesISINFungibleSelected.ConEspecie = ConEspecie
                    'dcProxy.Load(dcProxy.AmortizacionesISINConsultarQuery(_EspeciesISINFungibleSelected.IDIsinFungible), AddressOf terminoTraerAmortizaciones, Nothing)
                    dcProxy.Load(dcProxy.EspeciesConsultarQuery(_EspeciesISINFungibleSelected.IDEspecie, Nothing,
                                                            Nothing,
                                                            Nothing,
                                                            True,
                                                            Nothing,
                                                            Nothing, Program.Usuario, Program.HashConexion),
                                                            AddressOf terminoConsultarEspecie,
                                                            "Busqueda")
                Else
                    IsBusy = False
                End If
                LogValidarChanges = False
                MyBase.CambioItem("EspeciesISINFungibleSelected")
                LogValidarChanges = True
            End If
        End Set
    End Property

    Private _IsinFungible As Boolean = False
    Public Property IsinFungible As Boolean
        Get
            Return _IsinFungible
        End Get
        Set(ByVal value As Boolean)
            _IsinFungible = value
            MyBase.CambioItem("IsinFungible")
        End Set
    End Property

    Private _conEspecie As Boolean = False
    Public Property ConEspecie As Boolean
        Get
            Return _conEspecie
        End Get
        Set(ByVal value As Boolean)
            _conEspecie = value
        End Set
    End Property

    Private _strEspecie As String
    Public Property strEspecie() As String
        Get
            Return _strEspecie
        End Get
        Set(ByVal value As String)
            _strEspecie = value
            IsBusy = True
            If Not String.IsNullOrEmpty(_strEspecie) Then
                If blnDesdeEspecies Then
                    visMenu = Visibility.Collapsed
                    If IsNothing(EspeciesISINFungibleSelected) Then
                        dcProxy1.Load(dcProxy1.TraerEspeciesISINFungiblePorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesISINFungiblePorDefecto_Completed, "Default")
                    Else
                        If IsNothing(_EspeciesISINFungibleSelected) Then
                            dcProxy.Load(dcProxy.EspeciesISINFungibleConsultarQuery(_strEspecie, String.Empty, String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesISINFungible, Nothing)
                        End If
                    End If
                End If
            Else
                visMenu = Visibility.Visible
                dcProxy.Load(dcProxy.EspeciesISINFungibleFiltrarQuery(strEspecie, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesISINFungible, Nothing)
            End If
            MyBase.CambioItem("strEspecie")
        End Set
    End Property

    Private _visTextblock As Visibility
    Public Property VisTextblock As Visibility
        Get
            Return _visTextblock
        End Get
        Set(value As Visibility)
            _visTextblock = value
            MyBase.CambioItem("VisTextblock")
        End Set
    End Property


    Private _HabCalcularTasa As Boolean = True
    Public Property HabCalcularTasa As Boolean
        Get
            Return _HabCalcularTasa
        End Get
        Set(value As Boolean)
            _HabCalcularTasa = value
            MyBase.CambioItem("HabCalcularTasa")
        End Set
    End Property


    Private _visBuscador As Visibility
    Public Property visBuscador As Visibility
        Get
            Return _visBuscador
        End Get
        Set(value As Visibility)
            _visBuscador = value
            MyBase.CambioItem("visBuscador")
        End Set
    End Property

    Private _InhabilitarDetalles As Boolean = True
    Public Property InhabilitarDetalles() As Boolean
        Get
            Return _InhabilitarDetalles
        End Get
        Set(ByVal value As Boolean)
            _InhabilitarDetalles = value
            MyBase.CambioItem("InhabilitarDetalles")
        End Set
    End Property

    Private _visRentaFija As Visibility = Visibility.Collapsed
    Public Property visRentaFija As Visibility
        Get
            Return _visRentaFija
        End Get
        Set(value As Visibility)
            _visRentaFija = value
            If _visRentaFija = Visibility.Visible Then
                visAcciones = Visibility.Collapsed
            Else
                visAcciones = Visibility.Visible
            End If
            MyBase.CambioItem("visRentaFija")
        End Set
    End Property

    Private _visAcciones As Visibility = Visibility.Collapsed
    Public Property visAcciones As Visibility
        Get
            Return _visAcciones
        End Get
        Set(value As Visibility)
            _visAcciones = value
            MyBase.CambioItem("visAcciones")
        End Set
    End Property

    Private _visRentaFijaTF As Visibility = Visibility.Collapsed
    Public Property visRentaFijaTF As Visibility
        Get
            Return _visRentaFijaTF
        End Get
        Set(value As Visibility)
            _visRentaFijaTF = value
            MyBase.CambioItem("visRentaFijaTF")
        End Set
    End Property

    Private _visRentaFijaTV As Visibility = Visibility.Collapsed
    Public Property visRentaFijaTV As Visibility
        Get
            Return _visRentaFijaTV
        End Get
        Set(value As Visibility)
            _visRentaFijaTV = value
            MyBase.CambioItem("visRentaFijaTV")
        End Set
    End Property

    Private _Amortizaciones As List(Of AmortizacionesEspeci)
    Public Property Amortizaciones() As List(Of AmortizacionesEspeci)
        Get
            Return _Amortizaciones
        End Get
        Set(ByVal value As List(Of AmortizacionesEspeci))
            _Amortizaciones = value
            MyBase.CambioItem("Amortizaciones")
        End Set
    End Property

    Private _amortizacionSelected As AmortizacionesEspeci
    Public Property AmortizacionSelected() As AmortizacionesEspeci
        Get
            Return _amortizacionSelected
        End Get
        Set(ByVal value As AmortizacionesEspeci)
            _amortizacionSelected = value
            MyBase.CambioItem("AmortizacionSelected")
        End Set
    End Property

    Private _puedeImportar As Visibility = Visibility.Collapsed
    Public Property puedeImportar() As Visibility
        Get
            Return _puedeImportar
        End Get
        Set(ByVal value As Visibility)
            _puedeImportar = value
            MyBase.CambioItem("puedeImportar")
        End Set
    End Property

    Private _visAmortizaciones As Visibility = Visibility.Collapsed
    Public Property VisAmortizaciones() As Visibility
        Get
            Return _visAmortizaciones
        End Get
        Set(ByVal value As Visibility)
            _visAmortizaciones = value
            MyBase.CambioItem("VisAmortizaciones")
        End Set
    End Property

    Private _visTituloParticipativo As Visibility = Visibility.Collapsed
    Public Property visTituloParticipativo() As Visibility
        Get
            Return _visTituloParticipativo
        End Get
        Set(ByVal value As Visibility)
            _visTituloParticipativo = value
            MyBase.CambioItem("visTituloParticipativo")
        End Set
    End Property

    Private _visMenu As Visibility
    Public Property visMenu() As Visibility
        Get
            Return _visMenu
        End Get
        Set(ByVal value As Visibility)
            _visMenu = value
            MyBase.CambioItem("visMenu")
        End Set
    End Property

    Private _flujos As List(Of FlujosDiariosValoracion)
    Public Property Flujos() As List(Of FlujosDiariosValoracion)
        Get
            Return _flujos
        End Get
        Set(ByVal value As List(Of FlujosDiariosValoracion))
            _flujos = value
            MyBase.CambioItem("Flujos")
        End Set
    End Property

    Private _editable As Boolean
    Public Property Editable() As Boolean
        Get
            Return _editable
        End Get
        Set(ByVal value As Boolean)
            _editable = value
            MyBase.CambioItem("Editable")
        End Set
    End Property

    Private _visCF As Visibility
    Public Property visCF() As Visibility
        Get
            Return _visCF
        End Get
        Set(ByVal value As Visibility)
            _visCF = value
            MyBase.CambioItem("visCF")
        End Set
    End Property

    Private _visCalificacionL As Visibility = Visibility.Collapsed
    Public Property visCalificacionL() As Visibility
        Get
            Return _visCalificacionL
        End Get
        Set(ByVal value As Visibility)
            _visCalificacionL = value
            MyBase.CambioItem("visCalificacionL")
        End Set
    End Property

    Private _visCalificacionC As Visibility = Visibility.Visible
    Public Property visCalificacionC() As Visibility
        Get
            Return _visCalificacionC
        End Get
        Set(ByVal value As Visibility)
            _visCalificacionC = value
            MyBase.CambioItem("visCalificacionC")
        End Set
    End Property

    Private _blnHabilitarAmortizaciones As Boolean
    Public Property blnHabilitarAmortizaciones() As Boolean
        Get
            Return _blnHabilitarAmortizaciones
        End Get
        Set(ByVal value As Boolean)
            _blnHabilitarAmortizaciones = value
            MyBase.CambioItem("blnHabilitarAmortizaciones")
        End Set
    End Property

    Private _ViewImportarArchivo As cwCargaArchivos
    Public Property ViewImportarArchivo() As cwCargaArchivos
        Get
            Return _ViewImportarArchivo
        End Get
        Set(ByVal value As cwCargaArchivos)
            _ViewImportarArchivo = value
        End Set
    End Property

    Private _blnHabilitarPorcentajeRetencion As Boolean
    Public Property blnHabilitarPorcentajeRetencion() As Boolean
        Get
            Return _blnHabilitarPorcentajeRetencion
        End Get
        Set(ByVal value As Boolean)
            _blnHabilitarPorcentajeRetencion = value
            MyBase.CambioItem("blnHabilitarPorcentajeRetencion")
        End Set
    End Property

    Private _blnNuevoRegistro As Boolean = False
    Public Property blnNuevoRegistro As Boolean
        Get
            Return _blnNuevoRegistro
        End Get
        Set(ByVal value As Boolean)
            _blnNuevoRegistro = value
        End Set
    End Property

    Private _intBaseCalculoInteres As Integer
    Public Property intBaseCalculoInteres As Integer
        Get
            Return _intBaseCalculoInteres
        End Get
        Set(ByVal value As Integer)
            _intBaseCalculoInteres = value
        End Set
    End Property

    Private _strNombreEspecie As String
    Public Property strNombreEspecie As String
        Get
            Return _strNombreEspecie
        End Get
        Set(ByVal value As String)
            _strNombreEspecie = value
        End Set
    End Property

    Private _strIndicador As String
    Public Property strIndicador As String
        Get
            Return _strIndicador
        End Get
        Set(ByVal value As String)
            _strIndicador = value
        End Set
    End Property

    Private _logEditarEspecie As Boolean = Nothing
    Public Property logEditarEspecie As Boolean
        Get
            Return _logEditarEspecie
        End Get
        Set(ByVal value As Boolean)
            _logEditarEspecie = value
        End Set
    End Property

    Private _intIDConceptoRetencion As Integer
    Public Property intIDConceptoRetencion As Integer
        Get
            Return _intIDConceptoRetencion
        End Get
        Set(ByVal value As Integer)
            _intIDConceptoRetencion = value
        End Set
    End Property

    Private _HabilitarParticipativos As Boolean = True
    Public Property HabilitarParticipativos As Boolean
        Get
            Return _HabilitarParticipativos
        End Get
        Set(ByVal value As Boolean)
            _HabilitarParticipativos = value
        End Set
    End Property

    'JAEZ 20161103
    Private _strTipoEspecie As String
    Public Property strTipoEspecie As String
        Get
            Return _strTipoEspecie
        End Get
        Set(ByVal value As String)
            _strTipoEspecie = value
        End Set
    End Property

    'JAEZ 20161103
    Private _logEsAccion As Boolean
    Public Property logEsAccion As Boolean
        Get
            Return _logEsAccion
        End Get
        Set(ByVal value As Boolean)
            _logEsAccion = value
        End Set
    End Property

    Private _logTituloCarteraColectiva As Boolean
    Public Property logTituloCarteraColectiva As Boolean
        Get
            Return _logTituloCarteraColectiva
        End Get
        Set(ByVal value As Boolean)
            _logTituloCarteraColectiva = value
            MyBase.CambioItem("logTituloCarteraColectiva")
        End Set
    End Property

    Private _TabSeleccionado As Integer = 1
    Public Property TabSeleccionado() As Integer
        Get
            Return _TabSeleccionado
        End Get
        Set(ByVal value As Integer)
            _TabSeleccionado = value
            MyBase.CambioItem("TabSeleccionado")
        End Set
    End Property

#End Region




#Region "Resultados asíncronos"

    Private Sub TerminoTraerEspeciesISINFungible(ByVal lo As LoadOperation(Of EspeciesISINFungible))
        If Not lo.HasError Then

            EntEspeciesISINFungible = dcProxy.EspeciesISINFungibles

            If dcProxy.EspeciesISINFungibles.Count > 0 Then
                If lo.UserState = "insert" Then
                    LogValidarChanges = False
                    EspeciesISINFungibleSelected = EntEspeciesISINFungible.Last
                    LogValidarChanges = True
                    EspeciesISINFungibleSelected.ConEspecie = True
                End If
            Else
                If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If

            mdcProxyUtilidad01.Verificaparametro(PARAM_STR_EDITABLE_PORC_RETENCIONES, Program.Usuario, Program.HashConexion, AddressOf TerminotraerparametroCF, PARAM_STR_EDITABLE_PORC_RETENCIONES)

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de EspeciesISINFungibles", _
                                             Me.ToString(), "TerminoTraerEspeciesISINFungibles", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub
    ''' <history>
    '''ID:                   CAT20150901
    ''' Descripción:         Se agrega el valor por defecto 1 para los campos minimo y multiplo
    '''                      
    ''' Responsable:         Carlos Andres Toro (Alcuadrado S.A.)
    ''' Fecha:               1 de Septiembre/2015
    ''' Pruebas CB:          Carlos Andres Toro (Alcuadrado S.A.) - 1 de Septiembre/2015 - Pruebas OK
    ''' </history>
    ''' <history>
    '''ID:                   SM20151009
    ''' Descripción:         Se agrega parametro PARAM_STR_HABILITAR_MINMULTIPLO para cargar los campos Mínimo y Múltiplo 
    '''                      si el parametro esta en SI carga estos valores en 0 si no los debe cargar en 1.
    ''' Responsable:         Santiago Mazo Padierna (Alcuadrado S.A.)
    ''' Fecha:               09 de Octubre/2015
    ''' Pruebas CB:          Santiago Mazo Padierna (Alcuadrado S.A.) -  09 de Octubre/2015 - Pruebas OK
    ''' </history>
    Private Sub TerminoTraerEspeciesISINFungiblePorDefecto_Completed(ByVal lo As LoadOperation(Of EspeciesISINFungible))
        Try
            If Not lo.HasError Then
                If IsNothing(EspeciesISINFungiblePorDefecto) Then

                    If IsNothing(EntEspeciesISINFungible) Then
                        EntEspeciesISINFungible = dcProxy.EspeciesISINFungibles
                    End If
                    EspeciesISINFungiblePorDefecto = lo.Entities.FirstOrDefault
                    EspeciesISINFungiblePorDefecto.IDEspecie = strEspecie
                    EspeciesISINFungiblePorDefecto.Descripcion = strNombreEspecie
                    EspeciesISINFungiblePorDefecto.Usuario = Program.Usuario
                    EspeciesISINFungiblePorDefecto.logActivo = True
                    EspeciesISINFungiblePorDefecto.logSectorFinanciero = True
                    EspeciesISINFungiblePorDefecto.Amortizada = logAmortizaPorDefecto
                    EspeciesISINFungiblePorDefecto.intIndicador = strIndicador

                    EspeciesISINFungiblePorDefecto.intIDCalificacionInversion = Nothing
                    If Not IsNothing(EspeciesISINFungibleSelected) Then
                        EspeciesISINFungibleSelected.intIDConceptoRetencion = intIDConceptoRetencion
                        EspeciesISINFungibleSelected.strTipoEspecie = strTipoEspecie  'JAEZ 20161103
                        EspeciesISINFungibleSelected.logEsAccion = logEsAccion  'JAEZ 20161103

                        LogValidarChanges = False
                        EspeciesISINFungibleSelected = EspeciesISINFungiblePorDefecto
                        LogValidarChanges = True

                        EspeciesISINFungibleSelected.ConEspecie = ConEspecie
                    End If
                Else
                    EspeciesISINFungiblePorDefecto = lo.Entities.FirstOrDefault
                End If

                mdcProxyUtilidad01.Verificaparametro(PARAM_STR_EDITABLE_PORC_RETENCIONES, Program.Usuario, Program.HashConexion, AddressOf TerminotraerparametroCF, PARAM_STR_EDITABLE_PORC_RETENCIONES)

                IsBusy = False
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la EspeciesISINFungible por defecto", _
                                                 Me.ToString(), "TerminoTraerEspeciesISINFungiblePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la EspeciesISINFungible por defecto", _
                                    Me.ToString(), "TerminoTraerEspeciesISINFungiblePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub


    ''' <history>
    ''' ID caso de prueba:   Id_7, Id_8
    ''' Descripción:         Se coloca la instrucción "El ISIN está vinculado a detalles en alguna custodia, no se puede eliminar." 
    '''                      para retornar el mensaje cuando una especie no se puede eliminar.
    ''' Responsable:         Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:               5 de Mayo/2014
    ''' Pruebas CB:          Jorge Peña (Alcuadrado S.A.) - 5 de Mayo/2014 - Pruebas OK
    ''' </history>
    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            If So.HasError Then

                Dim strMsg As String = String.Empty

                If So.EntitiesInError.Count > 0 Then
                    For intI As Integer = 0 To So.EntitiesInError(0).ValidationErrors.Count - 1
                        strMsg &= So.EntitiesInError(0).ValidationErrors(intI).ErrorMessage & vbNewLine
                    Next
                End If
                If strMsg.Equals(String.Empty) Then
                    If So.Error.ToString.Contains("Ya existe el isin") Then
                        Dim intPosIni As Integer = So.Error.ToString.IndexOf("Ya existe el isin")
                        Dim intPosFin As Integer = So.Error.ToString.IndexOf("|")
                        strMsg = So.Error.ToString.Substring(intPosIni, intPosFin - intPosIni)
                        A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    ElseIf So.Error.ToString.Contains("El ISIN está vinculado") Then
                        strMsg = "El ISIN está vinculado a detalles en alguna custodia, no se puede eliminar."
                        A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        'ElseIf So.Error.ToString.Contains(" Debe existir más") Then
                        '    strMsg = "Debe existir más de una amortización."
                        '    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        'Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                                       Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                    Else
                        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados",
                                                       Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                    End If
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
                IsBusy = False
                So.MarkErrorAsHandled()
                Exit Try
            Else
                If blnDesdeEspecies Then
                    Me.vistaIsines.isinIsinFungible = EspeciesISINFungibleSelected
                    Me.vistaIsines.blnRespuesta = True
                    Me.vistaIsines.Close()
                Else
                    IsBusy = False
                    VisTextblock = Visibility.Visible
                    visBuscador = Visibility.Collapsed
                End If
            End If
            MyBase.TerminoSubmitChanges(So)
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    ''' <history>
    ''' Descripción:         Se comenta el case de indicador puesto que arrojaba error al cancelar la creación o edición del isin, se realizan pruebas de editar y grabar
    ''' Responsable:         Jhonatan Arley Acevedo Martínez
    ''' Fecha:               16 de Septiembre/2015
    ''' </history>
    Private Async Sub _EspeciesISINFungibleSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EspeciesISINFungibleSelected.PropertyChanged
        Try
            Select Case e.PropertyName
                Case "Fecha_Emision", "Fecha_Vencimiento"
                    If EspeciesISINFungibleSelected.Fecha_Emision <= EspeciesISINFungibleSelected.Fecha_Vencimiento Then
                        puedeImportar = Visibility.Visible
                    Else
                        puedeImportar = Visibility.Collapsed
                    End If
                    CargarCalificacion()
                Case "Amortizada"
                    If EspeciesISINFungibleSelected.Amortizada Then
                        VisAmortizaciones = Visibility.Visible
                        If EspeciesISINFungibleSelected.Fecha_Emision <= EspeciesISINFungibleSelected.Fecha_Vencimiento Then
                            puedeImportar = Visibility.Visible
                        Else
                            puedeImportar = Visibility.Collapsed
                        End If
                        TabSeleccionado = 0
                    Else
                        VisAmortizaciones = Visibility.Collapsed
                        TabSeleccionado = 1
                    End If
                    'Case "intIndicador"

                    '    Dim objParametros As List(Of OYDUtilidades.ItemCombo) = Nothing
                    '    If CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey("INDICADOR") Then
                    '        objParametros = CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("INDICADOR")
                    '        If Not IsNothing(EspeciesISINFungibleSelected.Indicador) Then
                    '            EspeciesISINFungibleSelected.Indicador = objParametros.Where(Function(i) i.ID = _EspeciesISINFungibleSelected.intIndicador.ToString()).Select(Function(i) i.Descripcion).FirstOrDefault
                    '        End If
                    '    End If
                Case "logFlujosIrregulares"
                    If EspeciesISINFungibleSelected.logFlujosIrregulares = False Then
                        EspeciesISINFungibleSelected.Fecha_Irregular = Nothing
                    End If
                Case "intIDConceptoRetencion"
                    If logCambiarCambiarValarRetencion Then
                        If Not IsNothing(EspeciesISINFungibleSelected.intIDConceptoRetencion) Then
                            EspeciesISINFungibleSelected.dblPorcentajeRetencion = Await ObtenerPorcentajeRetencion(EspeciesISINFungibleSelected.intIDConceptoRetencion)

                        End If
                    End If
                Case "Modalidad"
                    If EspeciesISINFungibleSelected.Modalidad = "NO" Then
                        EspeciesISINFungibleSelected.dblTasaEfectiva = 0
                        EspeciesISINFungibleSelected.Tasa_Facial = 0
                        HabCalcularTasa = False
                    Else

                        HabCalcularTasa = HabilitarParticipativos
                    End If


            End Select
            'IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Cambiar de Propiedad", _
             Me.ToString(), "_EspeciesISINFungibleSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub terminoTraerAmortizaciones(ByVal lo As LoadOperation(Of AmortizacionesEspeci))
        Try
            If Not lo.HasError Then
                Amortizaciones = dcProxy.AmortizacionesEspecis.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la amortizaciones del ISIN", _
                                                 Me.ToString(), "terminoTraerAmortizaciones", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
            IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la amortizaciones del ISIN", _
                                    Me.ToString(), "terminoTraerAmortizaciones", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub terminoConsultarEspecie(ByVal lo As LoadOperation(Of Especi))
        Try
            If Not lo.HasError Then
                If dcProxy.Especis.Count > 0 Then
                    If lo.UserState = "Busqueda" Then
                        esp = dcProxy.Especis.Last
                        _strEspecie = esp.Id
                        _strIndicador = esp.Indicador

                        If esp.EsAccion Then
                            visCF = Visibility.Collapsed
                        Else
                            visCF = Visibility.Visible
                        End If

                        VisTextblock = Visibility.Visible
                        visBuscador = Visibility.Collapsed
                        'EstablecerVisivilidad(esp)
                        If _EspeciesISINFungibleSelected.Amortizada Then
                            VisAmortizaciones = Visibility.Visible
                        Else
                            VisAmortizaciones = Visibility.Collapsed
                        End If
                        If _EspeciesISINFungibleSelected.Fecha_Emision <= _EspeciesISINFungibleSelected.Fecha_Vencimiento Then
                            puedeImportar = Visibility.Visible
                        Else
                            puedeImportar = Visibility.Collapsed
                        End If
                        If _EspeciesISINFungibleSelected.Amortizada Then
                            dcProxy.Load(dcProxy.AmortizacionesISINConsultarQuery(_EspeciesISINFungibleSelected.IDIsinFungible, Program.Usuario, Program.HashConexion), AddressOf terminoTraerAmortizaciones, Nothing)
                        End If
                    End If
                Else
                    If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                        If Not blnDesdeEspecies Then
                            A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        End If
                        'If _EspeciesISINFungibleSelected.Amortizada And blnCalculosFinancieros Then
                        '    AmortizacionesDesdeString()
                        'End If
                        If _EspeciesISINFungibleSelected.Amortizada Then
                            dcProxy.Load(dcProxy.AmortizacionesISINConsultarQuery(_EspeciesISINFungibleSelected.IDIsinFungible, Program.Usuario, Program.HashConexion), AddressOf terminoTraerAmortizaciones, Nothing)
                        End If
                    End If
                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Especies", _
                                                 Me.ToString(), "TerminoTraerEspecie", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
            IsBusy = False
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la especie", _
                                    Me.ToString(), "terminoConsultarEspecie", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try

    End Sub

    Private Sub TerminoTraerFlujosISINFungible(ByVal lo As LoadOperation(Of FlujosDiariosValoracion))
        Try
            If Not lo.HasError Then
                Flujos = dcProxy.FlujosDiariosValoracions.ToList
                IsBusy = False
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los flujos diarios del ISIN", _
                                                 Me.ToString(), "TerminoTraerFlujosISINFungible", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
                IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los flujos diarios del ISIN", _
                                    Me.ToString(), "TerminoTraerFlujosISINFungible", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

    Private Sub TerminoCargarAmortizaciones(ByVal lo As LoadOperation(Of OyDImportaciones.RespuestaArchivoImportacion))
        Try
            Dim MiFormato As NumberFormatInfo = New CultureInfo(CultureInfo.CurrentCulture.ToString()).NumberFormat

            Dim SeparadorMaquina = MiFormato.NumberDecimalSeparator

            Dim SeparadorContrario = IIf(SeparadorMaquina = ",", ".", ",")

            If Not lo Is Nothing Then
                If lo.HasError Then
                    If lo.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al validar el proceso.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    lo.MarkErrorAsHandled()
                Else
                    If lo.Entities.Count > 0 Then
                        Dim objListaRespuesta As List(Of RespuestaArchivoImportacion)
                        Dim objListaMensajes As New List(Of String)

                        objListaRespuesta = lo.Entities.ToList

                        If objListaRespuesta.Count > 0 Then
                            If objListaRespuesta.Where(Function(i) CType(i.Exitoso, Boolean) = False).Count > 0 Then

                                objListaMensajes.Add("El archivo genero algunas inconsistencias al intentar subirlo:")

                                For Each li In objListaRespuesta.Where(Function(i) CType(i.Exitoso, Boolean) = False).OrderBy(Function(o) o.Tipo)
                                    If li.Tipo = "C" Then
                                        objListaMensajes.Add(li.Mensaje)
                                    Else
                                        objListaMensajes.Add(String.Format("Fila: {0} - Columna {1} - Validación: {2}", li.Fila, li.Columna, li.Mensaje))
                                    End If
                                Next

                                objListaMensajes.Add("No se importaron registros debido a que se presentaron inconsistencias")

                                ViewImportarArchivo.ListaMensajes = objListaMensajes
                            Else
                                For Each li In objListaRespuesta.Where(Function(i) CType(i.Exitoso, Boolean) And Not i.Fila = -1)
                                    objListaMensajes.Add(li.Mensaje)
                                Next
                                ViewImportarArchivo.ListaMensajes = objListaMensajes
                            End If

                            Dim objListaAmortizaciones As New List(Of AmortizacionesEspeci)

                            Dim objListaRemoverAmortizaciones As New List(Of AmortizacionesEspeci)



                            If Not IsNothing(Amortizaciones) Then
                                If Amortizaciones.Count > 0 Then
                                    For Each li In Amortizaciones
                                        If li.logRegistroImportado Then
                                            objListaRemoverAmortizaciones.Add(li)
                                        End If
                                    Next
                                    For Each li In objListaRemoverAmortizaciones
                                        If Amortizaciones.Where(Function(i) i.IdAmortizacion = li.IdAmortizacion).Count > 0 Then
                                            Amortizaciones.Remove(Amortizaciones.Where(Function(i) i.IdAmortizacion = li.IdAmortizacion).First)
                                        End If
                                    Next
                                End If

                            End If


                            For Each li In objListaRespuesta.Where(Function(i) i.Exitoso And Not i.Fila = -2)

                                Dim Porcentaje As String = Replace(li.Tipo, SeparadorContrario, SeparadorMaquina)

                                objListaAmortizaciones.Add(New AmortizacionesEspeci With {
                                                        .FechaAmortizacion = CDate(li.Mensaje.Split("|").First()),
                                                        .IdAmortizacion = li.ID,
                                                        .IdIsinFungible = EspeciesISINFungibleSelected.IDIsinFungible,
                                                        .PorcentajeAmortizacion = CDbl(Porcentaje),
                                                        .FechaInicioVigencia = li.Mensaje.Split("|").Last(),
                                                        .logRegistroImportado = True}
                                                    )

                            Next

                            If Not IsNothing(Amortizaciones) Then
                                For Each li In Amortizaciones
                                    objListaAmortizaciones.Add(li)
                                Next
                            End If

                            Amortizaciones = Nothing

                            Amortizaciones = objListaAmortizaciones

                            AmortizacionSelected = Amortizaciones.FirstOrDefault
                            dcProxy.AmortizacionesEspecis.Clear()

                            MyBase.CambioItem("Amortizaciones")

                        Else
                            ViewImportarArchivo.ListaMensajes = objListaMensajes
                        End If
                    End If
                End If
            End If

            ViewImportarArchivo.IsBusy = False
        Catch ex As Exception
            ViewImportarArchivo.IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presento un problema al recibir el archivo de amortizaciones.", Me.ToString(), "TerminoCargarAmortizaciones", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    ''' <summary>
    ''' Carga el valor del parámetro CF_UTILIZACALCULOSFINANCIEROS -- 08/08/2013
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <remarks></remarks>
    Private Sub TerminotraerparametroCF(ByVal obj As InvokeOperation(Of String))
        Try
            If obj.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la validacion", Me.ToString(), "TerminotraerparametroCF", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)
            Else
                If obj.UserState = PARAM_STR_EDITABLE_PORC_RETENCIONES Then
                    If obj.Value = "SI" Then
                        blnHabilitarPorcentajeRetencion = True
                    Else
                        blnHabilitarPorcentajeRetencion = False
                    End If
                    mdcProxyUtilidad01.Verificaparametro(PARAM_STR_HABILITAR_MINMULTIPLO, Program.Usuario, Program.HashConexion, AddressOf TerminotraerparametroCF, PARAM_STR_HABILITAR_MINMULTIPLO)
                    'SM20151009
                ElseIf obj.UserState = PARAM_STR_HABILITAR_MINMULTIPLO Then
                    If _blnNuevoRegistro = True Then
                        If Not IsNothing(EspeciesISINFungiblePorDefecto) Then
                            If obj.Value = "SI" Then
                                EspeciesISINFungiblePorDefecto.Minimo = Nothing
                                EspeciesISINFungiblePorDefecto.Multiplo = Nothing
                            Else
                                EspeciesISINFungiblePorDefecto.Minimo = 1 'SM20151009
                                EspeciesISINFungiblePorDefecto.Multiplo = 1 'SM20151009
                            End If
                        End If

                        NuevoRegistro()
                    End If
                End If
            End If

            If Not IsNothing(esp) Then
                If esp.EsAccion Then
                    visCF = Visibility.Collapsed
                Else
                    visCF = Visibility.Visible
                End If
            End If

            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención del parámetro", _
                                    Me.ToString(), "TerminotraerparametroCF", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub



    ''' <summary>
    ''' Carga el valor del parámetro MOSTRAR_BARRA_BOTONES -- 08/08/2013
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' Descripción:        La propiedad "logEditarEspecie" controla si se muestran los botones del toolbar, si esta en edición se muestran de lo contrario se ocultan
    ''' Responsable:        Germán Arbey González Osorio (Alcuadrado S.A.)
    ''' Fecha:              Diciembre 15/2014
    ''' Pruebas CB:         Germán Arbey González Osorio (Alcuadrado S.A.) - Diciembre 15/2014
    ''' </history>
    Private Sub TerminotraerparametroMenu(ByVal obj As InvokeOperation(Of String))
        Try
            If obj.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la validacion", Me.ToString(), "TerminotraerparametroMenu", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)

            Else
                If obj.Value = "NO" Then
                    visMenu = Visibility.Collapsed
                    Editando = False

                Else

                    If IsNothing(logEditarEspecie) Then
                        visMenu = Visibility.Visible
                    Else
                        If logEditarEspecie Then
                            visMenu = Visibility.Visible
                        Else
                            visMenu = Visibility.Collapsed
                            Editando = False
                        End If
                    End If

                End If
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención del parámetro menú", _
                                    Me.ToString(), "TerminotraerparametroMenu", Application.Current.ToString(), Program.Maquina, ex)
            IsBusy = False
        End Try
    End Sub

#End Region

#Region "Metodos"

    Public Sub New()

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New EspeciesCFDomainContext()
            dcProxy1 = New EspeciesCFDomainContext()
            mdcProxyCalificacionValidar = New EspeciesCFDomainContext()
            dcProxyImportaciones = New ImportacionesDomainContext()
            mdcProxyUtilidad01 = New UtilidadesDomainContext()
        Else
            dcProxy = New EspeciesCFDomainContext(New System.Uri(Program.RutaServicioEspecies))
            dcProxy1 = New EspeciesCFDomainContext(New System.Uri(Program.RutaServicioEspecies))
            mdcProxyCalificacionValidar = New EspeciesCFDomainContext(New System.Uri(Program.RutaServicioEspecies))
            dcProxyImportaciones = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
            mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If
        Try

            mlogLlamadoDesdeMaestro = True
            ConEspecie = True
            If Not Program.IsDesignMode() Then
                IsBusy = True
                strEspecie = String.Empty
                VisTextblock = Visibility.Collapsed
                visBuscador = Visibility.Visible
                mdcProxyUtilidad01.Verificaparametro(PARAM_STR_MOSTRAR_BARRA_BOTONES, Program.Usuario, Program.HashConexion, AddressOf TerminotraerparametroMenu, Nothing)
                mdcProxyUtilidad01.Verificaparametro(PARAM_STR_EDITABLE_PORC_RETENCIONES, Program.Usuario, Program.HashConexion, AddressOf TerminotraerparametroCF, PARAM_STR_EDITABLE_PORC_RETENCIONES)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ISINesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try


    End Sub

    Public Sub New(pstrEspecie As String, pstrNombreEspecie As String, pstrIndicador As String, plogAmortiza As Boolean, pintIDConceptoRetencion As Integer, pstrTipoEspecie As String, plogEsAccion As Boolean)

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New EspeciesCFDomainContext()
            dcProxy1 = New EspeciesCFDomainContext()
            mdcProxyCalificacionValidar = New EspeciesCFDomainContext()
            dcProxyImportaciones = New ImportacionesDomainContext()
            mdcProxyUtilidad01 = New UtilidadesDomainContext()
        Else
            dcProxy = New EspeciesCFDomainContext(New System.Uri(Program.RutaServicioEspecies))
            dcProxy1 = New EspeciesCFDomainContext(New System.Uri(Program.RutaServicioEspecies))
            mdcProxyCalificacionValidar = New EspeciesCFDomainContext(New System.Uri(Program.RutaServicioEspecies))
            dcProxyImportaciones = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
            mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If
        Try
            If Not Program.IsDesignMode() Then
                IsBusy = True
                VisTextblock = Visibility.Visible
                visBuscador = Visibility.Collapsed
                blnDesdeEspecies = True
                logAmortizaPorDefecto = plogAmortiza
                strEspecie = pstrEspecie
                strNombreEspecie = pstrNombreEspecie
                strIndicador = pstrIndicador
                intIDConceptoRetencion = pintIDConceptoRetencion
                strTipoEspecie = pstrTipoEspecie  'JAEZ 20161103
                logEsAccion = plogEsAccion ' JAEZ 20161103
                mdcProxyUtilidad01.Verificaparametro(PARAM_STR_MOSTRAR_BARRA_BOTONES, Program.Usuario, Program.HashConexion, AddressOf TerminotraerparametroMenu, Nothing)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "EspeciesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <history>
    ''' ID caso de prueba:  CP0007
    ''' Descripción:        Se agrega y asigna la propiedad intIDCalificacionInversion.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              5 de Marzo/2015
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 5 de Marzo/2015
    ''' </history>
    ''' <history>
    ''' ID caso de prueba:  CP0016, CP0017
    ''' Descripción:        Se agrega y asigna la propiedad dblTasaEfectiva.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              11 de Mayo/2015
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) -  11 de Mayo/2015
    ''' </history>
    Public Sub New(pisnISINSelected As EspeciesISINFungible, pblnEsAccion As Boolean, pstrIndicador As String)

        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New EspeciesCFDomainContext()
            dcProxy1 = New EspeciesCFDomainContext()
            mdcProxyCalificacionValidar = New EspeciesCFDomainContext()
            dcProxyImportaciones = New ImportacionesDomainContext()
            mdcProxyUtilidad01 = New UtilidadesDomainContext()
        Else
            dcProxy = New EspeciesCFDomainContext(New System.Uri(Program.RutaServicioEspecies))
            dcProxy1 = New EspeciesCFDomainContext(New System.Uri(Program.RutaServicioEspecies))
            mdcProxyCalificacionValidar = New EspeciesCFDomainContext(New System.Uri(Program.RutaServicioEspecies))
            dcProxyImportaciones = New ImportacionesDomainContext(New System.Uri(Program.RutaServicioImportaciones))
            mdcProxyUtilidad01 = New UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
        End If
        Try
            blnHabilitarAmortizaciones = False

            Dim e As New EspeciesISINFungible
            e.ISIN = pisnISINSelected.ISIN
            e.IDFungible = pisnISINSelected.IDFungible
            e.Emision = pisnISINSelected.Emision
            e.Fecha_Emision = pisnISINSelected.Fecha_Emision
            e.Fecha_Vencimiento = pisnISINSelected.Fecha_Vencimiento
            e.Tasa_Facial = pisnISINSelected.Tasa_Facial
            e.Modalidad = pisnISINSelected.Modalidad
            'e.intIndicador = pisnISINSelected.intIndicador
            e.intIndicador = pstrIndicador
            e.Indicador = pisnISINSelected.Indicador
            e.Puntos_Indicador = pisnISINSelected.Puntos_Indicador
            e.IDConsecutivo = pisnISINSelected.IDConsecutivo
            e.TasaBase = pisnISINSelected.TasaBase
            e.IDIsinFungible = pisnISINSelected.IDIsinFungible
            e.Usuario = Program.Usuario
            e.IDEspecie = pisnISINSelected.IDEspecie
            e.Amortizada = pisnISINSelected.Amortizada
            e.logPoseeRetencion = pisnISINSelected.logPoseeRetencion
            logCambiarCambiarValarRetencion = False
            e.dblPorcentajeRetencion = pisnISINSelected.dblPorcentajeRetencion
            logCambiarCambiarValarRetencion = True
            e.logFlujosIrregulares = pisnISINSelected.logFlujosIrregulares
            e.logActivo = pisnISINSelected.logActivo
            e.logSectorFinanciero = pisnISINSelected.logSectorFinanciero
            e.ConEspecie = pisnISINSelected.ConEspecie
            e.Amortizaciones = pisnISINSelected.Amortizaciones
            e.MsjAmortizaciones = pisnISINSelected.MsjAmortizaciones
            e.Descripcion = pisnISINSelected.Descripcion

            If visCalificacionC = Visibility.Visible Then
                e.intIDCalificacionInversion = pisnISINSelected.intIdCalificacionInversionC
            ElseIf visCalificacionL = Visibility.Visible Then
                e.intIDCalificacionInversion = pisnISINSelected.intIdCalificacionInversionL
            End If
            e.dblTasaEfectiva = pisnISINSelected.dblTasaEfectiva
            e.Minimo = pisnISINSelected.Minimo
            e.Multiplo = pisnISINSelected.Multiplo
            e.logEsAccion = pisnISINSelected.logEsAccion
            e.Fecha_Irregular = pisnISINSelected.Fecha_Irregular
            e.intIDConceptoRetencion = pisnISINSelected.intIDConceptoRetencion
            e.strTipoEspecie = pisnISINSelected.strTipoEspecie  'JAEZ 20161102
            e.logEsAccion = pisnISINSelected.logEsAccion ' JAEZ 20161103

            'If Not HabilitarParticipativos Then
            '    pisnISINSelected.logTituloCarteraColectiva = True
            'Else
            '    pisnISINSelected.logTituloCarteraColectiva = False
            'End If
            e.logTituloCarteraColectiva = pisnISINSelected.logTituloCarteraColectiva

            If pisnISINSelected.Amortizada Then
                TabSeleccionado = 0
            Else
                TabSeleccionado = 1
            End If

            'ConsultarValoresDefectoTituloParticipativoISIN(pisnISINSelected.logTituloCarteraColectiva)
            If Not Program.IsDesignMode() Then
                IsBusy = True
                VisTextblock = Visibility.Visible
                visBuscador = Visibility.Collapsed
                If IsNothing(EntEspeciesISINFungible) Then EntEspeciesISINFungible = dcProxy.EspeciesISINFungibles
                EntEspeciesISINFungible.Add(e)
                EspeciesISINFungibleAnterior = e
                LogValidarChanges = False
                EspeciesISINFungibleSelected = e
                LogValidarChanges = True
                If pblnEsAccion Then
                    visCF = Visibility.Collapsed
                    'Else
                    '    dcProxy.Load(dcProxy.EspeciesConsultarQuery(e.IDEspecie, Nothing,
                    '                                            Nothing,
                    '                                            Nothing,
                    '                                            True),
                    '                                            AddressOf terminoConsultarEspecie,
                    '                                            "Busqueda")
                End If
                mdcProxyUtilidad01.Verificaparametro(PARAM_STR_MOSTRAR_BARRA_BOTONES, Program.Usuario, Program.HashConexion, AddressOf TerminotraerparametroMenu, Nothing)
                mdcProxyUtilidad01.Verificaparametro(PARAM_STR_EDITABLE_PORC_RETENCIONES, Program.Usuario, Program.HashConexion, AddressOf TerminotraerparametroCF, PARAM_STR_EDITABLE_PORC_RETENCIONES)
            End If


        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "EspeciesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub NuevoRegistroDetalle()

        Select Case NombreColeccionDetalle
            Case "cmAmortizaciones"

                'System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
                'System.Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo("en-US")

                Dim NewAmortizacion As New AmortizacionesEspeci
                NewAmortizacion.IdIsinFungible = EspeciesISINFungibleSelected.IDIsinFungible
                NewAmortizacion.PorcentajeAmortizacion = 0
                NewAmortizacion.FechaInicioVigencia = Date.Today
                NewAmortizacion.strUsuario = Program.Usuario
                NewAmortizacion.logRegistroImportado = False
                'ListaAmortizaciones.Add(NewAmortizacion)
                Dim objAmortizaciones As New List(Of AmortizacionesEspeci)

                If Not IsNothing(Amortizaciones) Then
                    For Each li In Amortizaciones
                        objAmortizaciones.Add(li)
                    Next
                End If
                objAmortizaciones.Add(NewAmortizacion)
                Amortizaciones = objAmortizaciones
                AmortizacionSelected = NewAmortizacion
                InhabilitarDetalles = False
                MyBase.CambioItem("Amortizaciones")
                MyBase.CambioItem("AmortizacionSelected")
                MyBase.CambioItem("Editando")

        End Select

    End Sub

    Public Overrides Sub EditarRegistro()
        Try
            If dcProxy.IsLoading Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("El sistema está finalizando un proceso necesario para iniciar la edición de los datos. Por favor espere un momento y vuelva a dar clic en el botón para ejecutar nuevamente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(_EspeciesISINFungibleSelected) Then
                _EspeciesISINFungibleSelected.ConEspecie = True
                dcProxy.Load(dcProxy.EspeciesISINFungibleFlujosDiariosConsultarQuery(_EspeciesISINFungibleSelected.IDIsinFungible, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerFlujosISINFungible, Nothing)
                Editando = True
                Editable = False
                logTituloCarteraColectiva = EspeciesISINFungibleSelected.logTituloCarteraColectiva
                'ConsultarValoresDefectoTituloParticipativoISIN(_EspeciesISINFungibleSelected.logTituloCarteraColectiva)

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al editar el registro", _
                     Me.ToString(), "EditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub NuevoRegistro()
        Dim NewEspeciesISINFungible As New EspeciesISINFungible
        Editable = True

        VisTextblock = Visibility.Collapsed
        visBuscador = Visibility.Visible

        If blnDesdeEspecies Then
            NewEspeciesISINFungible.IDEspecie = strEspecie

            If Not IsNothing(EspeciesISINFungiblePorDefecto) Then
                NewEspeciesISINFungible.ISIN = EspeciesISINFungiblePorDefecto.ISIN
                NewEspeciesISINFungible.Descripcion = EspeciesISINFungiblePorDefecto.Descripcion
                NewEspeciesISINFungible.IDFungible = EspeciesISINFungiblePorDefecto.IDFungible
                NewEspeciesISINFungible.Emision = EspeciesISINFungiblePorDefecto.Emision
                NewEspeciesISINFungible.intIDConceptoRetencion = intIDConceptoRetencion
                NewEspeciesISINFungible.strTipoEspecie = strTipoEspecie 'JAEZ 20161103
                NewEspeciesISINFungible.logEsAccion = logEsAccion 'JAEZ 20161103
                If Not IsNothing(EspeciesISINFungibleSelected) Then
                    If EntEspeciesISINFungible.Count < 1 Then
                        NewEspeciesISINFungible.IDIsinFungible = EspeciesISINFungiblePorDefecto.IDIsinFungible
                    Else
                        NewEspeciesISINFungible.IDIsinFungible = EntEspeciesISINFungible.Last.IDIsinFungible + 1
                    End If
                End If

                NewEspeciesISINFungible.Amortizada = EspeciesISINFungiblePorDefecto.Amortizada
                NewEspeciesISINFungible.logFlujosIrregulares = EspeciesISINFungiblePorDefecto.logFlujosIrregulares
                NewEspeciesISINFungible.logPoseeRetencion = EspeciesISINFungiblePorDefecto.logPoseeRetencion
                If Not IsNothing(EspeciesISINFungibleSelected) Then
                    NewEspeciesISINFungible.logTituloCarteraColectiva = EspeciesISINFungibleSelected.logTituloCarteraColectiva
                End If
                'If EspeciesISINFungibleSelected.logTituloCarteraColectiva Then
                '    NewEspeciesISINFungible.Modalidad = EspeciesISINFungiblePorDefecto.Modalidad
                'End If

                NewEspeciesISINFungible.Minimo = EspeciesISINFungiblePorDefecto.Minimo = 1 'SM20151009
                NewEspeciesISINFungible.Multiplo = EspeciesISINFungiblePorDefecto.Multiplo = 1 'SM20151009

                NewEspeciesISINFungible.intIndicador = EspeciesISINFungiblePorDefecto.intIndicador

            End If

            NewEspeciesISINFungible.IDConsecutivo = 0
            NewEspeciesISINFungible.logActivo = True
            NewEspeciesISINFungible.logSectorFinanciero = True
            NewEspeciesISINFungible.ConEspecie = False
        Else
            EspeciesISINFungibleAnterior = EspeciesISINFungibleSelected
            NewEspeciesISINFungible.IDEspecie = String.Empty
            NewEspeciesISINFungible.ISIN = String.Empty
            NewEspeciesISINFungible.IDFungible = 0
            NewEspeciesISINFungible.Emision = 0
            NewEspeciesISINFungible.IDConsecutivo = 0
            NewEspeciesISINFungible.IDIsinFungible = 0
            NewEspeciesISINFungible.logActivo = True
            NewEspeciesISINFungible.logSectorFinanciero = True
            NewEspeciesISINFungible.Amortizada = False
            NewEspeciesISINFungible.logFlujosIrregulares = False
            NewEspeciesISINFungible.logPoseeRetencion = False
            NewEspeciesISINFungible.ConEspecie = True
        End If

        NewEspeciesISINFungible.Usuario = Program.Usuario
        NewEspeciesISINFungible.Amortizaciones = String.Empty
        VisAmortizaciones = Visibility.Collapsed
        puedeImportar = Visibility.Collapsed
        If Not IsNothing(Amortizaciones) Then Amortizaciones = New List(Of AmortizacionesEspeci)()

        If IsNothing(EntEspeciesISINFungible) Then
            EntEspeciesISINFungible = dcProxy.EspeciesISINFungibles
        End If

        EntEspeciesISINFungible.Add(NewEspeciesISINFungible)
        LogValidarChanges = False
        EspeciesISINFungibleSelected = NewEspeciesISINFungible
        LogValidarChanges = True
        InhabilitarDetalles = False
        Editando = True

        MyBase.CambioItem("Editando")
        MyBase.CambioItem("EspeciesISINFungibleSelected")
        MyBase.CambioItem("ListaEspeciesISINFungible")

    End Sub

    ''' <history>
    ''' ID caso de prueba:   Id_22
    ''' Descripción:         Se agregó lógica para que al presionar el botón cancelar recupere el anterior registro seleccionado.
    ''' Responsable:         Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:               5 de Mayo/2014
    ''' Pruebas CB:          Jorge Peña (Alcuadrado S.A.) - 5 de Mayo/2014 - Pruebas OK
    ''' </history>
    Public Overrides Sub CancelarEditarRegistro()

        Try
            ErrorForma = ""
            If Not IsNothing(_EspeciesISINFungibleSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                If _EspeciesISINFungibleSelected.EntityState = EntityState.Detached Then
                    EspeciesISINFungibleSelected = EspeciesISINFungibleAnterior
                End If
            End If

            If mlogLlamadoDesdeMaestro Then
                If Not EspeciesISINFungibleSelected.logEsAccion Then
                    If EspeciesISINFungibleSelected.strTipoTasaFija = "F" Then
                        visRentaFijaTF = Visibility.Visible
                        visRentaFijaTV = Visibility.Collapsed
                    Else
                        visRentaFijaTV = Visibility.Visible
                        visRentaFijaTF = Visibility.Collapsed
                    End If
                    visRentaFija = Visibility.Visible
                Else
                    visRentaFija = Visibility.Collapsed
                    visRentaFijaTF = Visibility.Collapsed
                    visRentaFijaTV = Visibility.Collapsed
                End If
            End If

            InhabilitarDetalles = True

            If blnDesdeEspecies Then
                vistaIsines.blnRespuesta = False
                vistaIsines.Close()
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    ''' <history>
    ''' ID Caso de prueba:   Id_5, Id_6, Id_9, Id_10
    ''' Descripción:         Validar que al ingresar un nuevo registro la pantalla solicite campos obligatorios. Se eliminan las validaciones 
    '''                      "Or EspeciesISINFungibleSelected.Puntos_Indicador = 0" y "Or EspeciesISINFungibleSelected.Tasa_Facial = 0" porque 
    '''                      los puntos indicador si puede ser cero. Este cambio fue autorizado por Jorge Arango.
    ''' Responsable:         Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:               28 de Abril/2014
    ''' Pruebas CB:          Jorge Peña  - 28 de Abril/2014 - Resultados OK
    ''' </history>
    ''' <history>
    ''' ID caso de prueba:  CP0014, CP0015
    ''' Descripción:        Se agrega validación de la calificación de acuerdo a las fechas de emisión y vencimiento.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              5 de Marzo/2015
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 5 de Marzo/2015
    ''' </history>
    '''  <history>
    ''' ID caso de prueba:  
    ''' Descripción:        Se agrega validación de las fechas de las amortizaciones para que estén dentro del rango de las fechas de emisión y vencimiento.
    ''' Responsable:        Santiago Upegui G. (Alcuadrado S.A.)
    ''' Fecha:              10 de Julio/2015
    ''' Pruebas CB:         Santiago Upegui G. (Alcuadrado S.A.) - 10 de Julio/2015
    ''' </history>
    ''' <history>
    ''' ID caso de prueba:  
    ''' Descripción:        Se agrega validación del los valores minimo y multiplo
    ''' Responsable:        Santiago Mazo Padierna (Alcuadrado S.A.)
    ''' Fecha:              9 de Octubre/2015
    ''' Pruebas CB:         Santiago Mazo Padierna (Alcuadrado S.A.) - 9 de Octubre/2015
    ''' 
    ''' ID Cambio: JERF20181011  
    ''' Descripción:        Se elimina validación de cuando la especie es accion y está marcada como titulo participativo para que exija la fecha de emisión
    ''' Responsable:        Juan Esteban Restrepo Franco (Alcuadrado S.A.)
    ''' Fecha:              11 de Octubre/2018
    ''' </history>
    Public Overrides Sub ActualizarRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("Por favor espere, el sistema está cargando.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If
            IsBusy = True
            If String.IsNullOrEmpty(EspeciesISINFungibleSelected.IDEspecie) Then
                'If ConEspecie Then
                '    EspeciesISINFungibleSelected.IDEspecie = strEspecie
                'Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la especie.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If
            'End If

            If String.IsNullOrEmpty(EspeciesISINFungibleSelected.ISIN) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el ISIN.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If


            If EspeciesISINFungibleSelected.Minimo = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar un valor minimo mayor a 0.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            If EspeciesISINFungibleSelected.Multiplo = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar un valor multiplo mayor a 0.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            If visRentaFija = Visibility.Visible Or logTituloCarteraColectiva Or EspeciesISINFungibleSelected.logTituloCarteraColectiva Then 'EspeciesISINFungibleSelected.logTituloCarteraColectiva  cuando ya existe el isin y se edita, la propiedad logTituloCarteraColectiva es cuando se está creando un nuevo isin

                ' Cuando es una especie que ha sido marcada como titulo participativo (logTituloCarteraColectiva=1) y es renta fija,
                ' el isin debe tener fecha de emisión y vencimiento, mientras que si es acción solo es requerida la fecha de emisión
                If Not EspeciesISINFungibleSelected.logEsAccion Then
                    If IsNothing(EspeciesISINFungibleSelected.Fecha_Emision) Or (IsNothing(EspeciesISINFungibleSelected.Fecha_Vencimiento) And Not EspeciesISINFungibleSelected.logEsAccion) Then
                        A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar las fechas de emisión y vencimiento.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                End If


                If Not IsNothing(EspeciesISINFungibleSelected.Fecha_Emision) And Not IsNothing(EspeciesISINFungibleSelected.Fecha_Vencimiento) Then
                    If EspeciesISINFungibleSelected.Fecha_Emision >= EspeciesISINFungibleSelected.Fecha_Vencimiento Then
                        A2Utilidades.Mensajes.mostrarMensaje("La fecha de emisión no puede ser mayor o igual a la fecha de vencimiento.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                End If

                If Not EspeciesISINFungibleSelected.logEsAccion Then
                    If visCalificacionC = Visibility.Visible Then
                        EspeciesISINFungibleSelected.intIDCalificacionInversion = EspeciesISINFungibleSelected.intIdCalificacionInversionC
                    ElseIf visCalificacionL = Visibility.Visible Then
                        EspeciesISINFungibleSelected.intIDCalificacionInversion = EspeciesISINFungibleSelected.intIdCalificacionInversionL
                    End If

                    If EspeciesISINFungibleSelected.logFlujosIrregulares Then
                        If IsNothing(EspeciesISINFungibleSelected.Fecha_Irregular) Then
                            A2Utilidades.Mensajes.mostrarMensaje("La fecha Irregualr es requerida", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            IsBusy = False
                            Exit Sub
                        Else
                            If Not IsNothing(EspeciesISINFungibleSelected.Fecha_Emision) Or Not IsNothing(EspeciesISINFungibleSelected.Fecha_Vencimiento) Then
                                If (EspeciesISINFungibleSelected.Fecha_Emision >= EspeciesISINFungibleSelected.Fecha_Irregular) And (EspeciesISINFungibleSelected.Fecha_Vencimiento <= EspeciesISINFungibleSelected.Fecha_Irregular) Then
                                    A2Utilidades.Mensajes.mostrarMensaje("La fecha Irregular debe  estar dentro del rango de la fecha de emisión y la fecha de vencimiento", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    IsBusy = False
                                    Exit Sub
                                End If
                            End If
                        End If
                    End If

                    If EspeciesISINFungibleSelected.Amortizada Then
                        If Amortizaciones IsNot Nothing Then
                            'If Not IsNothing(EspeciesISINFungibleSelected.Fecha_Emision) Or Not IsNothing(EspeciesISINFungibleSelected.Fecha_Vencimiento) Then

                            '    Dim validarFechaAmortizaciones = (From item In Amortizaciones Where item.FechaAmortizacion < EspeciesISINFungibleSelected.Fecha_Emision Or item.FechaAmortizacion > EspeciesISINFungibleSelected.Fecha_Vencimiento).ToList()

                            '    If validarFechaAmortizaciones IsNot Nothing Then
                            '        If validarFechaAmortizaciones.Count > 0 Then
                            '            A2Utilidades.Mensajes.mostrarMensaje("Las fechas de las amortizaciones deben estar dentro del rango de la fecha de emisión y la fecha de vencimiento", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            '            IsBusy = False
                            '            Exit Sub
                            '        End If
                            '    End If
                            'End If


                            Dim logResultado As Boolean = False
                            Dim strMsg As String = String.Empty

                            Dim validarCantidadAmortizaciones = (From item In Amortizaciones Select item.PorcentajeAmortizacion).Count()

                            Dim ListaPorFechaInicioVigencia = From item In Amortizaciones _
                                                               Group By item.FechaInicioVigencia Into grouping = Group _
                                                               Select FechaInicioVigencia, dblTotalPorcentaje = grouping.Sum(Function(p) p.PorcentajeAmortizacion)

                            If validarCantidadAmortizaciones <= 1 Then
                                A2Utilidades.Mensajes.mostrarMensaje("Debe existir más de una amortización", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                IsBusy = False
                                Exit Sub
                            End If

                            For Each grp In ListaPorFechaInicioVigencia
                                If (grp.dblTotalPorcentaje <> 100 And grp.dblTotalPorcentaje / 100 >= 1) Or _
                                    (grp.dblTotalPorcentaje <> 1 And grp.dblTotalPorcentaje / 100 < 1) Then 'Si el valor del porcentaje de la amortización es base 100 valida que no sea diferente al 100% o si es base 1 el total no debe ser diferente a 1
                                    strMsg = String.Format("{0}{1} + Los registros con fecha de vigencia " & Format(grp.FechaInicioVigencia, "dd/MM/yyyy").ToString & " deben sumar 100 % ", strMsg, vbCrLf)
                                End If
                            Next

                            If strMsg.Equals(String.Empty) Then
                                logResultado = True
                            Else
                                logResultado = False
                                A2Utilidades.Mensajes.mostrarMensaje("Por favor verifique las siguientes inconsistencias antes de guardar: " & vbNewLine & strMsg & vbNewLine, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                IsBusy = False
                                Exit Sub
                            End If

                            Dim FechasRepetidasVigencia = (From item In Amortizaciones Select item.FechaInicioVigencia).Distinct


                            For Each itemFechaVigencia As Date In FechasRepetidasVigencia

                                Dim FechasRepetidasAmortizaciones = (From item In Amortizaciones Where item.FechaInicioVigencia = itemFechaVigencia Select item.FechaAmortizacion).Distinct()

                                Dim cantidadRegistrosAmortizaciones = (From item In Amortizaciones Where item.FechaInicioVigencia = itemFechaVigencia Select item.FechaAmortizacion).Count()
                                Dim cantidadRegistrosRepetidos = FechasRepetidasAmortizaciones.Count()

                                If cantidadRegistrosRepetidos <> cantidadRegistrosAmortizaciones Then
                                    A2Utilidades.Mensajes.mostrarMensaje("Para una misma fecha de vigencia no se deben ingresar varios registros con la misma fecha de amortización. ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                    IsBusy = False
                                    Exit Sub
                                End If
                            Next

                            Dim validarFechaVigenciaVacias = (From item In Amortizaciones Where item.FechaInicioVigencia Is Nothing).ToList()
                            If validarFechaVigenciaVacias.Count <> 0 Then
                                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar las fechas de inicio vigencia", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                                IsBusy = False
                                Exit Sub
                            End If
                        End If
                    End If

                    mdcProxyCalificacionValidar.ISINFungible_Calificacion_Validar(EspeciesISINFungibleSelected.Fecha_Emision, EspeciesISINFungibleSelected.Fecha_Vencimiento, EspeciesISINFungibleSelected.intIDCalificacionInversion, Program.Usuario, Program.HashConexion, AddressOf TerminoISINFungible_Calificacion_Validar, "")

                End If
            End If

            If visRentaFija = Visibility.Collapsed Then
                EspeciesISINFungibleSelected.Amortizaciones = String.Empty
                EspeciesISINFungibleSelected.ConEspecie = ConEspecie
                If Not (IsNothing(EspeciesISINFungibleSelected.Fecha_Emision) And IsNothing(EspeciesISINFungibleSelected.Fecha_Vencimiento)) Then
                    If Not IsNothing(EspeciesISINFungibleAnterior) Then
                        If EspeciesISINFungibleSelected.IDIsinFungible <> -1 And (EspeciesISINFungibleSelected.Fecha_Emision <> EspeciesISINFungibleAnterior.Fecha_Emision Or _
                        EspeciesISINFungibleSelected.Fecha_Vencimiento <> EspeciesISINFungibleAnterior.Fecha_Vencimiento) _
                        And EspeciesISINFungibleSelected.Amortizada Then

                            'recordar cambiar por la nueva forma de llamar mensajes si - no
                            A2Utilidades.Mensajes.mostrarMensajePregunta("Las fecha de emisión y/o vencimiento han sido modificadas, realmente desea actualizar el registro?", Program.TituloSistema, String.Empty, AddressOf TerminaPregunta)
                            IsBusy = False
                            Exit Sub
                        Else
                            If EspeciesISINFungibleSelected.Amortizada Then
                                If Not AsignarAmortizaciones() Then
                                    IsBusy = False
                                    Exit Sub
                                End If
                            End If
                        End If
                    Else
                        If EspeciesISINFungibleSelected.Amortizada Then
                            If Not AsignarAmortizaciones() Then
                                IsBusy = False
                                Exit Sub
                            End If
                        End If
                    End If
                    Program.VerificarCambiosProxyServidor(dcProxy)
                    dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, String.Empty)
                Else
                    Program.VerificarCambiosProxyServidor(dcProxy)
                    dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, String.Empty)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al actualizar el registro", _
                     Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub



    Public Overrides Sub BorrarRegistro()
        Try
            If dcProxy.IsLoading Then
                A2Utilidades.Mensajes.mostrarMensaje("El sistema está finalizando un proceso necesario para iniciar la edición de los datos. Por favor espere un momento y vuelva a dar clic en el botón para ejecutar nuevamente.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(_EspeciesISINFungibleSelected) Then
                dcProxy.EspeciesISINFungibles.Remove(_EspeciesISINFungibleSelected)
                EspeciesISINFungibleSelected = EntEspeciesISINFungible.LastOrDefault
                IsBusy = True
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, "BorrarRegistro")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        If NombreColeccionDetalle = "cmAmortizaciones" Then
            If Not IsNothing(Amortizaciones) Then
                If Not IsNothing(AmortizacionSelected) Then
                    Dim objListaAmortizaciones As New List(Of AmortizacionesEspeci)
                    Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(AmortizacionSelected, Amortizaciones)

                    If Not IsNothing(Amortizaciones) Then
                        For Each li In Amortizaciones
                            objListaAmortizaciones.Add(li)
                        Next
                    End If

                    objListaAmortizaciones.Remove(AmortizacionSelected)
                    Amortizaciones = objListaAmortizaciones

                    If Amortizaciones.Count > 0 Then
                        Program.PosicionarItemLista(AmortizacionSelected, Amortizaciones, intRegistroPosicionar)
                    Else
                        AmortizacionSelected = Nothing
                    End If

                    MyBase.CambioItem("AmortizacionSelected")
                    MyBase.CambioItem("Amortizaciones")
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Función utilizada para importar el archivo Deceval.
    ''' </summary>
    Public Sub CargarArchivo(pstrModulo As String, pstrNombreCompletoArchivo As String)
        Try
            If Not IsNothing(EspeciesISINFungibleSelected) Then

                Dim xmlCompleto As String
                ErrorForma = String.Empty

                xmlCompleto = ""

                ViewImportarArchivo.IsBusy = True

                If Not IsNothing(dcProxyImportaciones.RespuestaArchivoImportacions) Then
                    dcProxyImportaciones.RespuestaArchivoImportacions.Clear()
                End If
                'Editando
                dcProxyImportaciones.Load(dcProxyImportaciones.CargarAmortizacionesISINQuery(EspeciesISINFungibleSelected.IDIsinFungible, EspeciesISINFungibleSelected.Fecha_Emision, EspeciesISINFungibleSelected.Fecha_Vencimiento, xmlCompleto, pstrNombreCompletoArchivo, "Amortizaciones", Program.Usuario, Program.HashConexion), AddressOf TerminoCargarAmortizaciones, String.Empty)

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al subir el archivo.", _
                               Me.ToString(), "CargarArchivo", Application.Current.ToString(), Program.Maquina, ex)
        End Try


    End Sub


    Public Overrides Sub Filtrar()
        Try
            dcProxy.EspeciesISINFungibles.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.EspeciesISINFungibleFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesISINFungible, "FiltroVM")
                'variables = False
            Else
                dcProxy.Load(dcProxy.EspeciesISINFungibleFiltrarQuery(String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesISINFungible, "FiltroVM")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        MyBase.Buscar()
        cb.ISIN = ""
        cb.especie = ""
    End Sub

    ''' <history>
    ''' ID caso de prueba:   Id_3, Id_4
    ''' Descripción:         Validar que el filtro se haga por el Nombre.
    ''' Responsable:         Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:               5 de Mayo/2014
    ''' Pruebas CB:          Jorge Peña (Alcuadrado S.A.) - 5 de Mayo/2014 - Pruebas OK
    ''' </history>
    Public Overrides Sub ConfirmarBuscar()
        Try

            If cb.ISIN <> String.Empty Or cb.especie <> String.Empty Then
                ErrorForma = ""
                dcProxy.EspeciesISINFungibles.Clear()
                EspeciesISINFungibleAnterior = Nothing
                IsBusy = True

                cb.especie = System.Web.HttpUtility.UrlEncode(cb.especie)
                cb.ISIN = System.Web.HttpUtility.UrlEncode(cb.ISIN)

                dcProxy.Load(dcProxy.EspeciesISINFungibleConsultarQuery(cb.especie, cb.ISIN, String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesISINFungible, "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaISIN
                CambioItem("cb")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub EstablecerVisivilidad(pesp As Especi)
        If pesp.EsAccion Then
            visRentaFija = Visibility.Collapsed
            visRentaFijaTF = Visibility.Collapsed
            visRentaFijaTV = Visibility.Collapsed
        Else
            visRentaFija = Visibility.Visible
            If pesp.TipoTasaFija = "F" Then
                visRentaFijaTF = Visibility.Visible
                visRentaFijaTV = Visibility.Collapsed
            Else
                visRentaFijaTV = Visibility.Visible
                visRentaFijaTF = Visibility.Collapsed
            End If
        End If
    End Sub

    Private Sub TerminaPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

            If objResultado.DialogResult Then
                If EspeciesISINFungibleSelected.Amortizada Then
                    AsignarAmortizaciones()
                End If
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, String.Empty)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en TerminaPregunta", _
             Me.ToString(), "TeminaPregunta", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Function AsignarAmortizaciones() As Boolean
        If IsNothing(Amortizaciones) Then
            A2Utilidades.Mensajes.mostrarMensaje("Eligió asignar amortizaciones, debe importar la tabla de amortizaciones o asignarlas manualmente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Return False
        Else
            If Amortizaciones.Count = 0 Then
                A2Utilidades.Mensajes.mostrarMensaje("Eligió asignar amortizaciones, debe importar la tabla de amortizaciones o asignarlas manualmente", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Return False
            Else

                Dim xmlCompleto As String
                Dim xmlDetalle As String

                xmlCompleto = "<Amortizaciones>"

                For Each objeto In (From c In Amortizaciones)

                    xmlDetalle = "<Detalle intIDAmortizacionEspecie=""" & objeto.IdAmortizacion &
                                    """ dtmFechaAmortizacionEspecie=""" & Format(objeto.FechaAmortizacion, "yyyy/MM/dd") &
                                    """ dblPorcentajeAmortizacionEspecie=""" & objeto.PorcentajeAmortizacion &
                                    """ intIdIsinFungible=""" & objeto.IdIsinFungible &
                                    """ dtmInicioVigencia=""" & Format(objeto.FechaInicioVigencia, "yyyy/MM/dd") & """></Detalle>"

                    xmlCompleto = xmlCompleto & xmlDetalle
                Next

                xmlCompleto = xmlCompleto & "</Amortizaciones>"

                EspeciesISINFungibleSelected.Amortizaciones = xmlCompleto

                If blnDesdeEspecies Then vistaIsines.isinIsinFungible = EspeciesISINFungibleSelected
                Return True
            End If
        End If
    End Function

    ''' <history>
    ''' Descripción:        Método que se encarga de validar la diferencia entre la fecha de emisión y fecha de vencimiento del isin teniento en cuenta bisiestos,
    '''                     para determinar que calificación mostrar, visCalificacionC (corresponde a calificación corta tópico combo CFCalificacionesInversionesC) o
    '''                     visCalificacionL (corresponde a calificación larga tópico combo CFCalificacionesInversionesL)
    ''' Responsable:        Jhonatan Arley Acevedo Martínez (Alcuadrado S.A.)
    ''' Fecha:              18 de Abril/2016
    ''' </history> 
    Public Sub CargarCalificacion()
        If Not IsNothing(EspeciesISINFungibleSelected.Fecha_Emision) And Not IsNothing(EspeciesISINFungibleSelected.Fecha_Vencimiento) Then
            Dim Fecha_emision As Date = EspeciesISINFungibleSelected.Fecha_Emision
            Dim Fecha_vencimiento As Date = EspeciesISINFungibleSelected.Fecha_Vencimiento
            Dim Fecha As Date = DateAdd(DateInterval.Day, 1, Fecha_emision)
            Dim Esbisiesto As Integer = 0
            Dim span1 As TimeSpan
            span1 = Fecha_vencimiento - Fecha_emision

            While Fecha <= Fecha_vencimiento

                If Fecha.Day = 29 And Fecha.Month = 2 Then
                    Esbisiesto += 1
                    Fecha = Fecha_vencimiento
                End If
                Fecha = DateAdd(DateInterval.Day, 1, Fecha)
            End While

            If Esbisiesto = 1 And span1.Days <= 366 Then
                visCalificacionC = Visibility.Visible
                visCalificacionL = Visibility.Collapsed
                EspeciesISINFungibleSelected.intIDCalificacionInversion = EspeciesISINFungibleSelected.intIdCalificacionInversionC
            ElseIf span1.Days <= 365 Then
                visCalificacionC = Visibility.Visible
                visCalificacionL = Visibility.Collapsed
                EspeciesISINFungibleSelected.intIDCalificacionInversion = EspeciesISINFungibleSelected.intIdCalificacionInversionC
            Else
                visCalificacionC = Visibility.Collapsed
                visCalificacionL = Visibility.Visible
                EspeciesISINFungibleSelected.intIDCalificacionInversion = EspeciesISINFungibleSelected.intIdCalificacionInversionL
            End If
        End If
    End Sub
    ''' <history>
    ''' ID caso de prueba:  CP0014, CP0015
    ''' Descripción:        Método que devuelve el resultado de la validación de la calificación y continúa con las validaciones que estaban en el método ActualizarRegistro.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              5 de Marzo/2015
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 5 de Marzo/2015
    ''' </history>
    Private Sub TerminoISINFungible_Calificacion_Validar(lo As InvokeOperation(Of String))
        Try
            If lo.HasError Then
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso terminó calificacion validar", _
                                                 Me.ToString(), "TerminoISINFungible_Calificacion_Validar", Application.Current.ToString(), Program.Maquina, lo.Error)
            ElseIf lo.Value <> String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje(lo.Value, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If


            If String.IsNullOrEmpty(EspeciesISINFungibleSelected.Modalidad) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la modalidad.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            'Se comenta esta validación porque el campo "Posee retención" ya no existe en la pantalla. Jorge Peña - 11 de Mayo/2015
            'If EspeciesISINFungibleSelected.logPoseeRetencion Then
            'If IsNothing(EspeciesISINFungibleSelected.dblPorcentajeRetencion) Or EspeciesISINFungibleSelected.dblPorcentajeRetencion <= 0 Then
            '    A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el porcentaje de retención.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Exit Sub
            'End If
            'End If

            If visRentaFijaTF = Visibility.Visible Then
                If IsNothing(EspeciesISINFungibleSelected.Tasa_Facial) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar la tasa facial.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                    Exit Sub
                End If
            End If

            If visRentaFijaTV = Visibility.Visible Then
                If String.IsNullOrEmpty(EspeciesISINFungibleSelected.intIndicador) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el indicador.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                    Exit Sub
                End If

                If IsNothing(EspeciesISINFungibleSelected.Puntos_Indicador) Then
                    A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar los puntos indicador.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                    Exit Sub
                End If

            End If

            EspeciesISINFungibleSelected.Amortizaciones = String.Empty
            EspeciesISINFungibleSelected.ConEspecie = ConEspecie
            If Not (IsNothing(EspeciesISINFungibleSelected.Fecha_Emision) And IsNothing(EspeciesISINFungibleSelected.Fecha_Vencimiento)) Then
                If Not IsNothing(EspeciesISINFungibleAnterior) Then
                    If EspeciesISINFungibleSelected.IDIsinFungible <> -1 And (EspeciesISINFungibleSelected.Fecha_Emision <> EspeciesISINFungibleAnterior.Fecha_Emision Or _
                    EspeciesISINFungibleSelected.Fecha_Vencimiento <> EspeciesISINFungibleAnterior.Fecha_Vencimiento) _
                    And EspeciesISINFungibleSelected.Amortizada Then

                        'recordar cambiar por la nueva forma de llamar mensajes si - no
                        A2Utilidades.Mensajes.mostrarMensajePregunta("Las fecha de emisión y/o vencimiento han sido modificadas, realmente desea actualizar el registro?", Program.TituloSistema, String.Empty, AddressOf TerminaPregunta)
                        IsBusy = False
                        Exit Sub
                    Else
                        If EspeciesISINFungibleSelected.Amortizada Then
                            If Not AsignarAmortizaciones() Then
                                IsBusy = False
                                Exit Sub
                            End If
                        End If
                    End If
                Else
                    If EspeciesISINFungibleSelected.Amortizada Then
                        If Not AsignarAmortizaciones() Then
                            IsBusy = False
                            Exit Sub
                        End If
                    End If
                End If
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, String.Empty)
            Else
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, String.Empty)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en el proceso terminó calificacion validar", _
                                                             Me.ToString(), "TerminoISINFungible_Calificacion_Validar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <history>
    '''
    ''' Descripción:        Método que devuelve el el valor del porcentaje de retencion
    ''' Responsable:        Carlos Andres Toro (Alcuadrado S.A.)
    ''' Fecha:              28 de Septiembre/2015
    ''' Pruebas CB:         Carlos Andres Toro (Alcuadrado S.A.) - 28 de septiembre/2015
    ''' </history>

    Public Async Function ObtenerPorcentajeRetencion(ByVal pintIDConceptoRetencion As Integer) As Task(Of Double)
        Dim dblPorcentajeRetencion As Double

        Try
            Dim objRet As InvokeOperation(Of Double)
            Dim objProxyEsp As EspeciesCFDomainContext

            objProxyEsp = inicializarProxyEspecies()

            objRet = Await objProxyEsp.ConsultarPorcentajeRetencionSync(pintIDConceptoRetencion, Program.Usuario, Program.HashConexion).AsTask

            If Not objRet Is Nothing Then
                If objRet.HasError Then
                    If objRet.Error Is Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta para el método ObtenerPorcentajeRetencion.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                    End If

                    objRet.MarkErrorAsHandled()
                Else
                    If Not IsNothing(objRet.Value) Then
                        dblPorcentajeRetencion = objRet.Value
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar el método ObtenerPorcentajeRetencion. ", Me.ToString(), "ObtenerPorcentajeRetencion", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        Return dblPorcentajeRetencion
    End Function
#End Region

    Private Sub AmortizacionesDesdeString()
        Dim xmlLista As XElement
        Try
            If Not String.IsNullOrEmpty(_EspeciesISINFungibleSelected.Amortizaciones) Then
                xmlLista = XElement.Parse(_EspeciesISINFungibleSelected.Amortizaciones)
                Amortizaciones = New List(Of AmortizacionesEspeci)

                For Each element As XElement In xmlLista.Elements("f")
                    Dim NewAmortizacion As New AmortizacionesEspeci
                    NewAmortizacion.IdIsinFungible = EspeciesISINFungibleSelected.IDIsinFungible
                    NewAmortizacion.PorcentajeAmortizacion = Double.Parse(element.Element("p").Value)
                    NewAmortizacion.FechaAmortizacion = Date.Parse(element.Element("fc").Value)
                    NewAmortizacion.strUsuario = Program.Usuario
                    Amortizaciones.Add(NewAmortizacion)
                Next

                MyBase.CambioItem("Amortizaciones")
                MyBase.CambioItem("Editando")
            End If
        Catch ex As Exception
            Throw New Exception("No se pudo interpretar el xml de amortizaciones", ex)
        End Try
    End Sub

    Public Async Function CalcularValorRegistro() As System.Threading.Tasks.Task(Of Boolean)
        Dim logResultado As Boolean = False

        Try
            If Not IsNothing(_EspeciesISINFungibleSelected) Then

                Dim objRet As LoadOperation(Of CFEspecies.Operaciones_CalculosTasaFacial_Calcular)

                Try
                    IsBusy = True
                    ErrorForma = String.Empty

                    dcProxy.Operaciones_CalculosTasaFacial_Calculars.Clear()

                    objRet = Await dcProxy.Load(dcProxy.Operaciones_CalculosTasaFacial_CalcularQuery(strTipoCalculo,
                                                                                                   _EspeciesISINFungibleSelected.Tasa_Facial,
                                                                                                   _EspeciesISINFungibleSelected.dblTasaEfectiva,
                                                                                                   _EspeciesISINFungibleSelected.Modalidad,
                                                                                                   _EspeciesISINFungibleSelected.Fecha_Emision,
                                                                                                   _EspeciesISINFungibleSelected.Fecha_Vencimiento,
                                                                                                   _intBaseCalculoInteres,
                                                                                                   Program.Usuario,
                                                                                                   Program.UsuarioWindows,
                                                                                                   Program.Maquina, Program.HashConexion)).AsTask()

                    If Not objRet Is Nothing Then
                        If objRet.HasError Then
                            If objRet.Error Is Nothing Then
                                A2Utilidades.Mensajes.mostrarMensaje("Se presentó un problema al calcular los valores.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                            Else
                                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al calcular los valores.", Me.ToString(), "CalcularValorRegistro", Program.TituloSistema, Program.Maquina, objRet.Error)
                            End If

                            objRet.MarkErrorAsHandled()
                        Else
                            If objRet.Entities.Count > 0 Then
                                If objRet.Entities.First.Exitoso Then
                                    Dim objResultadoCalculos = objRet.Entities.First

                                    If strTipoCalculo = "TASAEFECTIVANOMINAL" Then
                                        _EspeciesISINFungibleSelected.Tasa_Facial = objResultadoCalculos.ValorSalidaNominal

                                    Else
                                        _EspeciesISINFungibleSelected.dblTasaEfectiva = objResultadoCalculos.ValorSalidaEfectiva
                                    End If

                                    logResultado = True
                                Else
                                    logResultado = False

                                End If
                            End If
                        End If
                    End If
                Catch ex As Exception
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al calcular los valores.", Me.ToString(), "CalcularValorRegistro", Application.Current.ToString(), Program.Maquina, ex)
                    logResultado = False
                Finally
                End Try



            Else
                logResultado = True
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema para calcular el valor del registro.", Me.ToString(), "CalcularValorRegistro", Program.TituloSistema, Program.Maquina, ex)
            logResultado = False
        End Try

        IsBusy = False
        Return (logResultado)

    End Function

    'Public Sub ConsultarValoresDefectoTituloParticipativoISIN(ByVal plogParticipativos As Boolean)
    '    Try
    '        If plogParticipativos Then
    '            HabilitarParticipativos = False
    '            HabCalcularTasa = False
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los valores por defecto título participativo.", Me.ToString(), "ConsultarValoresDefectoTituloParticipativoISIN", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub

End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaISIN

    Implements INotifyPropertyChanged

    Private _especie As String
    <Display(Name:="Nemotécnico")> _
    Public Property especie As String
        Get
            Return _especie
        End Get
        Set(ByVal value As String)
            _especie = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("especie"))
        End Set
    End Property

    Private _ISIN As String
    <Display(Name:="ISIN")> _
    Public Property ISIN As String
        Get
            Return _ISIN
        End Get
        Set(ByVal value As String)
            _ISIN = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ISIN"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class
