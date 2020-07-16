Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewModelTemplate2010
'Archivo: EspeciesViewModel.vb
'Generado el : 06/30/2011 10:47:25
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
Imports A2.OyD.OYDServer.RIA.Web.CFEspecies
Imports A2.OyD.OYDServer.RIA.Web.CFMaestros
Imports A2Utilidades.Mensajes
Imports GalaSoft.MvvmLight.Messaging

Public Class EspeciesViewModel
    Inherits A2ControlMenu.A2ViewModel

    Dim blnMostrarBarraBoton As Boolean

    Public Property cb As New CamposBusquedaEspecie
    Private EspeciePorDefecto As Especi
    Private EspeciesDividendoPorDefecto As EspeciesDividendos
    Private EspeciesPreciosPorDefecto As EspeciesPrecios
    Private EspeciesISINPorDefecto As EspeciesISIN
    Private EspeciesISINFungiblePorDefecto As EspeciesISINFungible
    Private EspeciesNemotecnicosPorDefecto As EspeciesNemotecnicos
    Private EspeciesDepositoPorDefecto As List(Of EspeciesDeposito)

    Private EspeciesTotalesPorDefecto As EspeciesTotales
    Private EspecieAnterior As Especi
    Private EspecieAnteriorPreEspecie As Especi
    Dim dcProxy As EspeciesCFDomainContext
    Dim dcProxy1 As EspeciesCFDomainContext
    Dim dcProxy2 As EspeciesCFDomainContext
    Dim dcProxyMaestros As MaestrosCFDomainContext
    Dim fecha As DateTime
    Dim i, x As Integer
    Private mdcProxyUtilidad01 As UtilidadesDomainContext
    Private mdcProxyUtilidad02 As UtilidadesDomainContext
    Private mdcProxyUtilidad03 As UtilidadesDomainContext
    Private mdcProxyUtilidad04 As UtilidadesDomainContext
    Private origen As String = String.Empty
    Dim DicCamposTab As New Dictionary(Of String, Integer)
    Dim strdescripcionclase As String
    Dim contador As Boolean = True
    Dim variables As Boolean = True
    Private _mlogExisteNemotblEspeciesBolsa As Boolean = False
    Private _mlogExisteNemotblEspecies As Boolean = False
    Private _ClaseNuevo As clase = New clase
    Private _ListaComboGrupo As OYDUtilidades.ItemCombo
    Private _ListaComboSubGrupo As OYDUtilidades.ItemCombo
    Dim _ListaCombos As OYDUtilidades.ItemCombo
    Dim mintSubGrupoPorDefecto As Integer
    Dim mlogNuevoRegistro As Boolean = True

    Private Const PARAM_STR_MOSTRAR_BARRA_BOTONES As String = "MOSTRAR_BARRA_BOTONES"
    'Descripción:   Se agrega el campo "Democratización" (Aplica solo para acciones).
    'Responsable:   Jorge Peña (Alcuadrado S.A.)
    'Fecha:         5 de Mayo/2016
    Private VISUALIZAR_CAMPO_DEMOCRATIZACION As String = "NO"
    Private Const OPCION_DUPLICAR As String = "DUPLICAR"
    'Public Property DicVisibilidadColumnas As New Dictionary(Of String, String)

    Private objResultado As ISINesView
    Private strEspeciePosicionar As String
    Private logRecargarDetalles As Boolean = True
    Dim logDuplicar As Boolean = False
    'Public logLimpiarTituloParticipativo = True
    ''' <history>
    ''' ID caso de prueba:  Id_10
    ''' Descripción:        Desarrollo PreEspecies.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              9 de Junio/2014
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 9 de Junio/2014
    ''' </history>
    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New EspeciesCFDomainContext()
                dcProxy1 = New EspeciesCFDomainContext()
                dcProxy2 = New EspeciesCFDomainContext()
                mdcProxyUtilidad01 = New A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext()
                mdcProxyUtilidad02 = New A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext()
                mdcProxyUtilidad03 = New A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext()
                mdcProxyUtilidad04 = New A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext()
                dcProxyMaestros = New MaestrosCFDomainContext
            Else
                dcProxy = New EspeciesCFDomainContext(New System.Uri(Program.RutaServicioEspecies))
                dcProxy1 = New EspeciesCFDomainContext(New System.Uri(Program.RutaServicioEspecies))
                dcProxy2 = New EspeciesCFDomainContext(New System.Uri(Program.RutaServicioEspecies))
                mdcProxyUtilidad01 = New A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                mdcProxyUtilidad02 = New A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                mdcProxyUtilidad03 = New A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                mdcProxyUtilidad04 = New A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext(New System.Uri(Program.RutaServicioUtilidadesOYD))
                dcProxyMaestros = New MaestrosCFDomainContext(New System.Uri(Program.RutaServicioMaestros))
            End If
        Catch ex As Exception

        End Try

        Try

            If Not Program.IsDesignMode() Then
                IsBusy = True
                mdcProxyUtilidad01.Verificaparametro(PARAM_STR_MOSTRAR_BARRA_BOTONES, Program.Usuario, Program.HashConexion, AddressOf TerminotraerparametroMenu, Nothing)

                'Descripción:   Se agrega el campo "Democratización" (Aplica solo para acciones).
                'Responsable:   Jorge Peña (Alcuadrado S.A.)
                'Fecha:         5 de Mayo/2016
                mdcProxyUtilidad01.Verificaparametro("VISUALIZAR_CAMPO_DEMOCRATIZACION", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "VISUALIZAR_CAMPO_DEMOCRATIZACION")
                ' EOMC -- Lanza cargas de listas para combos nuevos por cálculos financieros -- 08/08/2013
                mdcProxyUtilidad01.Load(mdcProxyUtilidad01.cargarCombosEspeciesCalculosFinancierosQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesCombosCalculosFinancieros_Completed, "")
                dcProxy.Load(dcProxy.EspeciesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspecies, "FiltroInicial")
                dcProxy1.Load(dcProxy1.TraerEspeciePorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesPorDefecto_Completed, "Default")
                dcProxy1.Load(dcProxy1.TraerEspeciesDividendoPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesDividendoPorDefecto_Completed, "Default")
                dcProxy1.Load(dcProxy1.TraerEspeciesPreciosPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesPreciosPorDefecto_Completed, "Default")
                dcProxy1.Load(dcProxy1.TraerEspeciesISINPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesISINPorDefecto_Completed, "Default")
                dcProxy1.Load(dcProxy1.TraerEspeciesISINFungiblePorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesISINFungiblePorDefecto_Completed, "Default")
                dcProxy1.Load(dcProxy1.TraerEspeciesNemotecnicosPorDefectoQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesNemotecnicosPorDefecto_Completed, "Default")
                dcProxy1.Load(dcProxy1.ConsultarEspeciesDepositoPorDefectoQuery(String.Empty, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarEspeciesDepositoPorDefecto_Completed, "Default")
                dcProxyMaestros.Load(dcProxyMaestros.ClasificacionesConsultarQuery(0, Nothing, "E", False, False, 0, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerClasificaciones, Nothing)
                dcProxy2.Load(dcProxy2.Especies_ConceptoRetencion_ConsultarQuery(Program.Usuario, Program.HashConexion), AddressOf TerminoEspecies_ConceptoRetencion_Consultar, "Default")
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "EspeciesViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Resultados Asincrónicos"


    Private Sub TerminoTraerEspeciesPorDefecto_Completed(ByVal lo As LoadOperation(Of Especi))
        Try
            If Not lo.HasError Then
                EspeciePorDefecto = lo.Entities.FirstOrDefault
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Especie por defecto", _
                                                 Me.ToString(), "TerminoTraerEspeciePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la Especie por defecto", _
                                                 Me.ToString(), "TerminoTraerEspeciePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End Try
    End Sub

    Private Sub TerminoTraerEspeciesDividendoPorDefecto_Completed(ByVal lo As LoadOperation(Of EspeciesDividendos))
        Try
            If Not lo.HasError Then
                EspeciesDividendoPorDefecto = lo.Entities.FirstOrDefault
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la EspeciesDividendo por defecto", _
                                                 Me.ToString(), "TerminoTraerEspeciesDividendoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la EspeciesDividendo por defecto", _
                                                 Me.ToString(), "TerminoTraerEspeciesDividendoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End Try
    End Sub

    Private Sub TerminoTraerEspeciesISINPorDefecto_Completed(ByVal lo As LoadOperation(Of EspeciesISIN))
        Try
            If Not lo.HasError Then
                EspeciesISINPorDefecto = lo.Entities.FirstOrDefault
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la EspeciesISIN por defecto", _
                                                 Me.ToString(), "TerminoTraerEspeciesISINPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la EspeciesISIN por defecto", _
                                                Me.ToString(), "TerminoTraerEspeciesISINPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End Try
    End Sub

    Private Sub TerminoTraerEspeciesPreciosPorDefecto_Completed(ByVal lo As LoadOperation(Of EspeciesPrecios))
        Try
            If Not lo.HasError Then
                EspeciesPreciosPorDefecto = lo.Entities.FirstOrDefault
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la EspeciesPrecios por defecto", _
                                                 Me.ToString(), "TerminoTraerEspeciesPreciosPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la EspeciesPrecios por defecto", _
                                                Me.ToString(), "TerminoTraerEspeciesPreciosPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End Try
    End Sub

    Private Sub TerminoTraerEspeciesISINFungiblePorDefecto_Completed(ByVal lo As LoadOperation(Of EspeciesISINFungible))
        Try
            If Not lo.HasError Then
                EspeciesISINFungiblePorDefecto = lo.Entities.FirstOrDefault
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la EspeciesISINFungible por defecto", _
                                                 Me.ToString(), "TerminoTraerEspeciesISINFungiblePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la EspeciesISINFungible por defecto", _
                                    Me.ToString(), "TerminoTraerEspeciesISINFungiblePorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End Try
    End Sub

    Private Sub TerminoTraerEspeciesNemotecnicosPorDefecto_Completed(ByVal lo As LoadOperation(Of EspeciesNemotecnicos))
        Try
            If Not lo.HasError Then
                EspeciesNemotecnicosPorDefecto = lo.Entities.FirstOrDefault
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la EspeciesNemotécnico por defecto", _
                                                 Me.ToString(), "TerminoTraerEspeciesNemotecnicosPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la EspeciesNemotécnico por defecto", _
                                                Me.ToString(), "TerminoTraerEspeciesNemotecnicosPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End Try
    End Sub

    Private Sub TerminoConsultarEspeciesDepositoPorDefecto_Completed(ByVal lo As LoadOperation(Of EspeciesDeposito))
        Try
            If Not lo.HasError Then
                EspeciesDepositoPorDefecto = lo.Entities.ToList
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de especies depósito por defecto", _
                                                 Me.ToString(), "TerminoConsultarEspeciesDepositoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de especies depósito por defecto", _
                                                Me.ToString(), "TerminoConsultarEspeciesDepositoPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End Try
    End Sub

    Private Sub TerminoTraerEspeciesTotalesPorDefecto_Completed(ByVal lo As LoadOperation(Of EspeciesTotales))
        If Not lo.HasError Then
            EspeciesTotalesPorDefecto = lo.Entities.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la EspeciesTotales por defecto", _
                                             Me.ToString(), "TerminoTraerEspeciesTotalesPorDefecto_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    ''' <history>
    ''' Descripción:        Desarrollo PreEspecies. Se agregó la opción "TerminoGuardar" en el lo.UserState
    '''                     para reacargar la lista cuando sea una preespecie.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              9 de Junio/2014
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 9 de Junio/2014
    ''' </history>
    Private Sub TerminoTraerEspecies(ByVal lo As LoadOperation(Of Especi))
        Try

            If Not lo.HasError Then

                If lo.UserState = "TerminoGuardar" Then
                    logRecargarDetalles = False
                Else
                    logRecargarDetalles = True
                End If

                ListaEspecies = dcProxy.Especis
                If dcProxy.Especis.Count > 0 Then
                    If lo.UserState = "insert" Then
                        EspecieSelected = ListaEspecies.Last
                    ElseIf lo.UserState = "TerminoGuardar" Then
                        logRecargarDetalles = True
                        If Not String.IsNullOrEmpty(strEspeciePosicionar) Then
                            If ListaEspecies.Where(Function(i) i.Id = strEspeciePosicionar).Count > 0 Then
                                EspecieSelected = ListaEspecies.Where(Function(i) i.Id = strEspeciePosicionar).First
                            End If
                        End If
                    End If
                Else
                    If lo.UserState = "Busqueda" Or lo.UserState = "FiltroVM" Or lo.UserState = "FiltroInicial" Then
                        A2Utilidades.Mensajes.mostrarMensaje("No se encontró ningún registro", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        variables = True
                        'MyBase.Buscar()
                        'MyBase.CancelarBuscar()
                    End If
                End If


            Else

                A2ComunesControl.FuncionesCompartidas.obtenerMensajeValidacionErrorPersonalizado("Se presentó un problema en la obtención de la lista de Especies", Me.ToString, "TerminoTraerEspecie", lo.Error)
                lo.MarkErrorAsHandled()

                'If lo.Error.ToString.Contains("ErrorPersonalizado") Then
                '    IsBusy = False
                '    Dim intPosIni As Integer = lo.Error.ToString.IndexOf("ErrorPersonalizado,") + 20
                '    Dim intPosFin As Integer = lo.Error.ToString.IndexOf("|")
                '    strMsg = lo.Error.ToString.Substring(intPosIni, intPosFin - intPosIni)
                '    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                '    lo.MarkErrorAsHandled()
                '    Exit Sub
                'Else
                '    IsBusy = False
                '    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Especies", _
                '                                     Me.ToString(), "TerminoTraerEspecie", Application.Current.ToString(), Program.Maquina, lo.Error)
                '    lo.MarkErrorAsHandled()   '????
                'End If
            End If
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar la operación", Me.ToString(), "TerminoModificacionValidar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerEspeciesDividendos(ByVal lo As LoadOperation(Of EspeciesDividendos))
        If Not lo.HasError Then
            ListaEspeciesDividendos = dcProxy.EspeciesDividendos
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de EspeciesDividendos", _
                                             Me.ToString(), "TerminoTraerEspeciesDividendos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub
    Private Sub TerminoTraerArbitrajeADR(ByVal lo As LoadOperation(Of ArbitrajeADR))
        If Not lo.HasError Then
            If Not IsNothing(ListaArbitrajeADR) Then ListaArbitrajeADR.Clear() Else ListaArbitrajeADR = New ObservableCollection(Of ArbitrajeADR)
            If Not IsNothing(ListaArbitrajeADREliminado) Then ListaArbitrajeADREliminado.Clear() Else ListaArbitrajeADREliminado = New ObservableCollection(Of ArbitrajeADR)
            For Each item In dcProxy.ArbitrajeADRs
                ListaArbitrajeADR.Add(item)
            Next
            ListaArbitrajeADR = New ObservableCollection(Of ArbitrajeADR)(ListaArbitrajeADR.OrderByDescending(Function(x) x.dtmVigencia))
            ListaArbitrajeADRSinFiltro = New ObservableCollection(Of ArbitrajeADR)(ListaArbitrajeADR.OrderByDescending(Function(x) x.dtmVigencia))
            ArbitrajeADRSelected = ListaArbitrajeADR.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de ArbitrajeADR",
                                             Me.ToString(), "TerminoTraerArbitrajeADR", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerEspeciesPrecios(ByVal lo As LoadOperation(Of EspeciesPrecios))
        If Not lo.HasError Then
            ListaEspeciesPrecios = dcProxy.EspeciesPrecios
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de EspeciesPrecios", _
                                             Me.ToString(), "TerminoTraerEspeciesPrecios", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerEspeciesISIN(ByVal lo As LoadOperation(Of EspeciesISIN))
        If Not lo.HasError Then
            ListaEspeciesISIN = dcProxy.EspeciesISINs
            'IsinFungible = False
            'If ListaEspeciesISIN.Count = 0 Then
            '    IsinFungible = False
            'Else
            '    IsinFungible = True
            'End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de EspeciesISIN", _
                                             Me.ToString(), "TerminoTraerEspeciesISIN", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerEspeciesISINFungible(ByVal lo As LoadOperation(Of EspeciesISINFungible))
        If Not lo.HasError Then
            If Not IsNothing(ListaEspeciesISINFungible) Then ListaEspeciesISINFungible.Clear() Else ListaEspeciesISINFungible = New ObservableCollection(Of EspeciesISINFungible)
            For Each it In dcProxy.EspeciesISINFungibles
                ListaEspeciesISINFungible.Add(it)
            Next
            ListaEspeciesISINFungible = New ObservableCollection(Of EspeciesISINFungible)(ListaEspeciesISINFungible.OrderByDescending(Function(x) x.Fecha_Emision))
            ListaEspeciesISINFungibleSinFiltro = ListaEspeciesISINFungible
            EspeciesISINFungibleSelected = ListaEspeciesISINFungible.FirstOrDefault
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de EspeciesISINFungibles", _
                                             Me.ToString(), "TerminoTraerEspeciesISINFungibles", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerEspeciesTotales(ByVal lo As LoadOperation(Of EspeciesTotales))
        If Not lo.HasError Then
            ListaEspeciesTotales = dcProxy.EspeciesTotales
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de EspeciesTotales", _
                                             Me.ToString(), "TerminoTraerEspeciesTotales", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoTraerEspeciesNemotecnicos(ByVal lo As LoadOperation(Of EspeciesNemotecnicos))
        If Not lo.HasError Then
            ListaEspeciesNemotecnicos = dcProxy.EspeciesNemotecnicos
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de EspeciesNemotécnicos", _
                                             Me.ToString(), "TerminoTraerEspeciesNemotecnicos", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    Private Sub TerminoConsultarEspeciesDeposito(ByVal lo As LoadOperation(Of EspeciesDeposito))
        If Not lo.HasError Then
            ListaEspeciesDeposito = dcProxy.EspeciesDepositos.ToList
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de EspeciesTotales", _
                                             Me.ToString(), "TerminoTraerEspeciesTotales", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
    End Sub

    'Tablas padres

    Private Sub TerminoTraerEspeciesCombosCalculosFinancieros_Completed(lo As LoadOperation(Of A2.OyD.OYDServer.RIA.Web.OYDUtilidades.ItemCombo))
        Dim ListaCombos As List(Of OYDUtilidades.ItemCombo)

        Try
            If Not lo.HasError Then
                ListaCombos = lo.Entities.ToList
                ListaComboMoneda = ListaCombos.Where(Function(i) i.Categoria = "MONEDA").ToList
                ListaComboCurvaCC = ListaCombos.Where(Function(i) i.Categoria = "CURVACC").ToList
                ListaComboClaseInversion = ListaCombos.Where(Function(i) i.Categoria = "CLASEINVERSION").ToList
                ListaComboClassificacionInversion = ListaCombos.Where(Function(i) i.Categoria = "CALIFICACIONINVERSION").ToList
                MyBase.CambioItem("ListaComboMoneda")
                MyBase.CambioItem("ListaComboCurvaCC")
                MyBase.CambioItem("ListaComboClaseInversion")
                MyBase.CambioItem("ListaComboClassificacionInversion")
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los combos para cálculos financieros", _
                                                 Me.ToString(), "TerminoTraerEspeciesCombosCalculosFinancieros_Completed", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de los combos para cálculos financieros", _
                                                Me.ToString(), "TerminoTraerEspeciesCombosCalculosFinancieros_Completed", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoTraerClasificaciones(ByVal lo As LoadOperation(Of Clasificacion))
        Try
            If Not lo.HasError Then
                ' listaclasificacion = dcProxyMaestros.Clasificaciones
                listaclasificacion = lo.Entities.ToList
                ConsultarListaSubgrupo()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de Clasificaciones", _
                                                 Me.ToString(), "TerminoTraerClasificacion", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al traer las clasificaciones", _
             Me.ToString(), "TerminoTraerClasificaciones", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    Public Sub TerminoEspecies_ConceptoRetencion_Consultar(ByVal lo As LoadOperation(Of ConceptoRetencion))
        Try
            If Not lo.HasError Then
                ListaConceptoRetencion = dcProxy2.ConceptoRetencions

                Dim objListaGravados = New List(Of ConceptoRetencion)
                Dim objListaNoGravados = New List(Of ConceptoRetencion)

                For Each li In ListaConceptoRetencion.Where(Function(i) i.logGravado = True)
                    objListaGravados.Add(li)
                Next

                ListaConceptoRetencionGravados = objListaGravados.ToList()



                For Each li In ListaConceptoRetencion.Where(Function(i) i.logGravado = False)
                    objListaNoGravados.Add(li)
                Next

                ListaConceptoRetencionNoGravados = objListaNoGravados.ToList()

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de conceptos retención",
                                                 Me.ToString(), "TerminoEspecies_ConceptoRetencion_Consultar", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de conceptos retención",
                                                 Me.ToString(), "TerminoEspecies_ConceptoRetencion_Consultar", Application.Current.ToString(), Program.Maquina, lo.Error)
            'lo.MarkErrorAsHandled()   '????
        End Try
    End Sub

#End Region

#Region "Propiedades"

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

    Private _ListaEspecies As EntitySet(Of Especi)
    Public Property ListaEspecies() As EntitySet(Of Especi)
        Get
            Return _ListaEspecies
        End Get
        Set(ByVal value As EntitySet(Of Especi))
            _ListaEspecies = value

            MyBase.CambioItem("ListaEspecies")
            MyBase.CambioItem("ListaEspeciesPaged")
            If Not IsNothing(value) Then
                EspecieSelected = _ListaEspecies.FirstOrDefault
                'If IsNothing(EspecieAnterior) Then
                '    EspecieSelected = _ListaEspecies.FirstOrDefault
                'Else
                '    EspecieSelected = EspecieAnterior
                'End If
            End If
        End Set
    End Property

    Public ReadOnly Property ListaEspeciesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEspecies) Then
                Dim view = New PagedCollectionView(_ListaEspecies)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <history>
    ''' Descripción:        Desarrollo PreEspecies. Se agregó la variable "logRecargarDetalles" para indicar 
    '''                     si se debe recargar la lista cuando es una preespecie.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              9 de Junio/2014
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 9 de Junio/2014
    ''' </history>
    ''' <history>
    ''' Descripción:        Se eliminó la instrucción "EspecieSelected.ValoraDescuentoFactoring = False" porque ya no se utilizan en la pantalla de Especies. Cambio autorizado por Jorge Arango.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              29 de Octubre/2014
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 29 de Octubre/2014
    ''' </history>
    Private WithEvents _EspecieSelected As Especi
    Public Property EspecieSelected() As Especi
        Get
            Return _EspecieSelected
        End Get
        Set(ByVal value As Especi)
            'If Not IsNothing(_EspecieSelected) AndAlso _EspecieSelected.Equals(value) Then
            '    Exit Property
            'End If
            _EspecieSelected = value

            'Descripción:   Se agrega el campo "Democratización" (Aplica solo para acciones).
            'Responsable:   Jorge Peña (Alcuadrado S.A.)
            'Fecha:         5 de Mayo/2016
            mdcProxyUtilidad01.Verificaparametro("VISUALIZAR_CAMPO_DEMOCRATIZACION", Program.Usuario, Program.HashConexion, AddressOf TerminoConsultarParametros, "VISUALIZAR_CAMPO_DEMOCRATIZACION")

            If Not value Is Nothing And logRecargarDetalles Then

                If blnMostrarBarraBoton Then habilitarLinkISIN = Editando Else habilitarLinkISIN = True

                If EspecieSelected.TipoEspecie = "A" Then
                    HabilitarTabArbitrajeADR = "Visible"
                    TabSeleccionado = EspecieTabs.ArbitrajeADR
                Else
                    HabilitarTabArbitrajeADR = "Hidden"
                    If TabSeleccionado = EspecieTabs.ArbitrajeADR Then TabSeleccionado = EspecieTabs.CF
                End If

                ' EOMC -- Se cambia case por if (está verificando un boolean) y se modifican valores para propiedades que inhabilitan controles dependiende de 'EsAccion' -- 08/08/2013
                If EspecieSelected.EsAccion Then

                    VisualizarAcciones = Visibility.Visible
                    VisualizarRentaFija = Visibility.Collapsed

                    'Descripción:   Se agrega el campo "Democratización" (Aplica solo para acciones).
                    'Responsable:   Jorge Peña (Alcuadrado S.A.)
                    'Fecha:         5 de Mayo/2016
                    If VISUALIZAR_CAMPO_DEMOCRATIZACION = "SI" Then
                        VisualizarDemocratizacion = Visibility.Visible
                    Else
                        VisualizarDemocratizacion = Visibility.Collapsed
                    End If

                    Habilitar = False
                    HabilitarIndicador = False
                    If Editando Then
                        Inhabilitar = True
                        InhabilitarBursatilidad = True
                    Else
                        Inhabilitar = False
                        InhabilitarBursatilidad = False
                    End If
                    Clase = "1"
                    HabilitarTabs = True
                    HabilitarTabPrecio = True


                    Visible = Visibility.Collapsed
                    VisibleV = Visibility.Collapsed
                    VisibleF = Visibility.Collapsed

                    EspecieSelected.TipoTasaFija = Nothing
                    EspecieSelected.ConceptoRetencion = -1
                Else

                    VisualizarRentaFija = Visibility.Visible
                    VisualizarAcciones = Visibility.Collapsed

                    'Descripción:   Se agrega el campo "Democratización" (Aplica solo para acciones).
                    'Responsable:   Jorge Peña (Alcuadrado S.A.)
                    'Fecha:         5 de Mayo/2016
                    VisualizarDemocratizacion = Visibility.Collapsed

                    If Editando Then
                        Habilitar = True
                        HabilitarIndicador = True
                    Else
                        Habilitar = False
                        HabilitarIndicador = False
                    End If

                    Inhabilitar = False
                    InhabilitarBursatilidad = False
                    Clase = "0"
                    HabilitarTabs = False
                    HabilitarTabPrecio = False

                    Select Case EspecieSelected.TipoTasaFija
                        Case String.Empty
                            Visible = Visibility.Collapsed
                            VisibleV = Visibility.Collapsed
                            VisibleF = Visibility.Collapsed
                        Case "F"
                            VisibleF = Visibility.Visible
                            VisibleV = Visibility.Collapsed
                            Visible = Visibility.Visible
                        Case "V"
                            VisibleF = Visibility.Collapsed
                            VisibleV = Visibility.Visible
                            Visible = Visibility.Visible
                    End Select
                End If

                buscarItem("emisor")
                buscarItem("clase")
                buscarItem("admonemision")
                buscarItem("claseinversion")
                If Not value Is Nothing Then
                    If variables = False Then
                        If contador = False Then
                            dcProxy.EspeciesDividendos.Clear()
                            dcProxy.Load(dcProxy.EspeciesDividendosConsultarQuery(EspecieSelected.Id, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesDividendos, Nothing)
                            dcProxy.EspeciesPrecios.Clear()
                            dcProxy.Load(dcProxy.EspeciesPreciosConsultarQuery(EspecieSelected.Id, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesPrecios, Nothing)
                            dcProxy.EspeciesISINs.Clear()
                            dcProxy.Load(dcProxy.EspeciesISINConsultarQuery(EspecieSelected.Id, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesISIN, Nothing)
                            dcProxy.EspeciesISINFungibles.Clear()
                            If Not String.IsNullOrEmpty(EspecieSelected.Id) Then
                                dcProxy.Load(dcProxy.EspeciesISINFungibleConsultarQuery(EspecieSelected.Id, String.Empty, EspecieSelected.TipoTasaFija, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesISINFungible, Nothing)
                            End If
                            dcProxy.EspeciesNemotecnicos.Clear()
                            dcProxy.Load(dcProxy.EspeciesNemotecnicosConsultarQuery(EspecieSelected.Id, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesNemotecnicos, Nothing)
                            dcProxy.EspeciesTotales.Clear()
                            dcProxy.Load(dcProxy.EspeciesTotalesConsultarQuery(EspecieSelected.Id, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesTotales, Nothing)
                            dcProxy.ArbitrajeADRs.Clear()
                            dcProxy.Load(dcProxy.EspecieArbitrajeADR_ConsultarQuery(EspecieSelected.Id, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArbitrajeADR, Nothing)
                            contador = True
                            variables = True
                        Else
                            contador = False
                            Exit Property
                        End If
                    Else
                        dcProxy.EspeciesDividendos.Clear()
                        dcProxy.Load(dcProxy.EspeciesDividendosConsultarQuery(EspecieSelected.Id, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesDividendos, Nothing)
                        dcProxy.EspeciesPrecios.Clear()
                        dcProxy.Load(dcProxy.EspeciesPreciosConsultarQuery(EspecieSelected.Id, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesPrecios, Nothing)
                        dcProxy.EspeciesISINs.Clear()
                        dcProxy.Load(dcProxy.EspeciesISINConsultarQuery(EspecieSelected.Id, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesISIN, Nothing)
                        dcProxy.EspeciesISINFungibles.Clear()
                        If Not String.IsNullOrEmpty(EspecieSelected.Id) Then
                            dcProxy.Load(dcProxy.EspeciesISINFungibleConsultarQuery(EspecieSelected.Id, String.Empty, EspecieSelected.TipoTasaFija, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesISINFungible, Nothing)
                        End If
                        dcProxy.EspeciesNemotecnicos.Clear()
                        dcProxy.Load(dcProxy.EspeciesNemotecnicosConsultarQuery(EspecieSelected.Id, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesNemotecnicos, Nothing)
                        dcProxy.EspeciesTotales.Clear()
                        dcProxy.Load(dcProxy.EspeciesTotalesConsultarQuery(EspecieSelected.Id, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesTotales, Nothing)
                        dcProxy.ArbitrajeADRs.Clear()
                        dcProxy.Load(dcProxy.EspecieArbitrajeADR_ConsultarQuery(EspecieSelected.Id, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArbitrajeADR, Nothing)
                    End If

                    Me.EmisoresClaseSelected.NombreAdmonEmision = EspecieSelected.NombreAdmonEmision
                End If


                If Not logDuplicar Then
                    dcProxy.EspeciesDepositos.Clear()
                    dcProxy.Load(dcProxy.ConsultarEspeciesDepositoQuery(EspecieSelected.Id, Program.Usuario, Program.HashConexion), AddressOf TerminoConsultarEspeciesDeposito, Nothing)
                End If

                'If Not IsNothing(EspecieSelected.Vencimiento) And Not IsNothing(EspecieSelected.Emision) Then
                '    DiferenciaPlazo()
                'End If
                'MyBase.CambioItem("EspecieSelected")
            End If
            MyBase.CambioItem("EspecieSelected")
            MyBase.CambioItem("Visible")
            MyBase.CambioItem("VisibleV")
            MyBase.CambioItem("VisibleF")

        End Set
    End Property

    Private _EmisorClaseSelected As Emisores = New Emisores
    Public Property EmisorClaseSelected As Emisores
        Get
            Return _EmisorClaseSelected
        End Get
        Set(ByVal value As Emisores)
            _EmisorClaseSelected = value
            MyBase.CambioItem("EmisorClaseSelected")
        End Set
    End Property

    Private _ClaseClaseSelected As clase = New clase
    Public Property ClaseClaseSelected As clase
        Get
            Return _ClaseClaseSelected
        End Get
        Set(ByVal value As clase)
            _ClaseClaseSelected = value
            MyBase.CambioItem("ClaseClaseSelected")
        End Set
    End Property

    Private _EmisoresClaseSelected As OtrosEmisores = New OtrosEmisores
    Public Property EmisoresClaseSelected As OtrosEmisores
        Get
            Return _EmisoresClaseSelected
        End Get
        Set(ByVal value As OtrosEmisores)
            _EmisoresClaseSelected = value
            MyBase.CambioItem("EmisoresClaseSelected")
        End Set
    End Property

    Private _HabilitarIndicador As Boolean
    Public Property HabilitarIndicador As Boolean
        Get
            Return _HabilitarIndicador
        End Get
        Set(ByVal value As Boolean)
            _HabilitarIndicador = value
            MyBase.CambioItem("HabilitarIndicador")
        End Set
    End Property

    Private _Habilitar As Boolean
    Public Property Habilitar As Boolean
        Get
            Return _Habilitar
        End Get
        Set(ByVal value As Boolean)
            _Habilitar = value
            MyBase.CambioItem("Habilitar")
        End Set
    End Property


    Private _HabilitarTabs As Boolean = True
    Public Property HabilitarTabs As Boolean
        Get
            Return _HabilitarTabs
        End Get
        Set(ByVal value As Boolean)
            _HabilitarTabs = value
            MyBase.CambioItem("HabilitarTabs")
        End Set
    End Property

    Private _HabilitarTabPrecio As Boolean = True
    Public Property HabilitarTabPrecio As Boolean
        Get
            Return _HabilitarTabPrecio
        End Get
        Set(ByVal value As Boolean)
            _HabilitarTabPrecio = value
            MyBase.CambioItem("HabilitarTabPrecio")
        End Set
    End Property

    Private _HabilitarTabISIN As Boolean = True
    Public Property HabilitarTabISIN As Boolean
        Get
            Return _HabilitarTabISIN
        End Get
        Set(ByVal value As Boolean)
            _HabilitarTabISIN = value
            MyBase.CambioItem("HabilitarTabISIN")
        End Set
    End Property

    Private _EditandoPrecio As Boolean = True
    Public Property EditandoPrecio As Boolean
        Get
            Return _EditandoPrecio
        End Get
        Set(ByVal value As Boolean)
            _EditandoPrecio = value
            MyBase.CambioItem("EditandoPrecio")
        End Set
    End Property

    Private _Inhabilitar As Boolean
    Public Property Inhabilitar As Boolean
        Get
            Return _Inhabilitar
        End Get
        Set(ByVal value As Boolean)
            _Inhabilitar = value
            MyBase.CambioItem("Inhabilitar")
        End Set
    End Property

    Private _InhabilitarBursatilidad As Boolean
    Public Property InhabilitarBursatilidad As Boolean
        Get
            Return _InhabilitarBursatilidad
        End Get
        Set(ByVal value As Boolean)
            _InhabilitarBursatilidad = value
            MyBase.CambioItem("InhabilitarBursatilidad")
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

    Private _IsinFungibleBusquedad As Boolean = False
    Public Property IsinFungibleBusquedad As Boolean
        Get
            Return _IsinFungibleBusquedad
        End Get
        Set(ByVal value As Boolean)
            _IsinFungibleBusquedad = value
            MyBase.CambioItem("IsinFungibleBusquedad")
        End Set
    End Property

    Private _Visible As Visibility = Visibility.Visible
    Public Property Visible() As Visibility
        Get
            Return _Visible
        End Get
        Set(ByVal value As Visibility)
            _Visible = value
            MyBase.CambioItem("Visible")
        End Set
    End Property

    Private _VisibleV As Visibility = Visibility.Visible
    Public Property VisibleV() As Visibility
        Get
            Return _VisibleV
        End Get
        Set(ByVal value As Visibility)
            _VisibleV = value
            MyBase.CambioItem("VisibleV")
        End Set
    End Property

    Private _VisibleF As Visibility = Visibility.Visible
    Public Property VisibleF() As Visibility
        Get
            Return _VisibleF
        End Get
        Set(ByVal value As Visibility)
            _VisibleF = value
            MyBase.CambioItem("VisibleF")
        End Set
    End Property

    Private _HabilitarCodigo As Boolean = False
    Public Property HabilitarCodigo() As Boolean
        Get
            Return _HabilitarCodigo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCodigo = value
            MyBase.CambioItem("HabilitarCodigo")
        End Set
    End Property

    Private _HabilitarCodigo2 As Boolean = False
    Public Property HabilitarCodigo2() As Boolean
        Get
            Return _HabilitarCodigo2
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCodigo2 = value
            MyBase.CambioItem("HabilitarCodigo2")
        End Set
    End Property

    Private _HabilitarNemotecnico As Boolean = False
    Public Property HabilitarNemotecnico() As Boolean
        Get
            Return _HabilitarNemotecnico
        End Get
        Set(ByVal value As Boolean)
            _HabilitarNemotecnico = value
            MyBase.CambioItem("HabilitarNemotecnico")
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

    Private _InhabilitarDetallePrecio As Boolean = True
    Public Property InhabilitarDetallePrecio() As Boolean
        Get
            Return _InhabilitarDetallePrecio
        End Get
        Set(ByVal value As Boolean)
            _InhabilitarDetallePrecio = value
            MyBase.CambioItem("InhabilitarDetallePrecio")
        End Set
    End Property

    Private _HabilitarTituloMaterializado As Boolean = False
    Public Property HabilitarTituloMaterializado() As Boolean
        Get
            Return _HabilitarTituloMaterializado
        End Get
        Set(ByVal value As Boolean)
            _HabilitarTituloMaterializado = value
            MyBase.CambioItem("HabilitarTituloMaterializado")
        End Set
    End Property

    'Private _HabilitarCamposTituloParticipativo As Boolean = False
    'Public Property HabilitarCamposTituloParticipativo() As Boolean
    '    Get
    '        Return _HabilitarCamposTituloParticipativo
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _HabilitarCamposTituloParticipativo = value
    '        MyBase.CambioItem("HabilitarCamposTituloParticipativo")
    '    End Set
    'End Property

    'Private _HabilitarTipo As Boolean = False
    'Public Property HabilitarTipo() As Boolean
    '    Get
    '        Return _HabilitarTipo
    '    End Get
    '    Set(ByVal value As Boolean)
    '        _HabilitarTipo = value
    '        MyBase.CambioItem("HabilitarTipo")
    '    End Set
    'End Property

    Private _HabilitarCamposTabCFTituloParticipativo As Boolean = False
    Public Property HabilitarCamposTabCFTituloParticipativo() As Boolean
        Get
            Return _HabilitarCamposTabCFTituloParticipativo
        End Get
        Set(ByVal value As Boolean)
            _HabilitarCamposTabCFTituloParticipativo = value
            MyBase.CambioItem("HabilitarCamposTabCFTituloParticipativo")
        End Set
    End Property

    Private _TabSeleccionadaFinanciero As Integer = 0
    Public Property TabSeleccionadaFinanciero As Integer
        Get
            Return _TabSeleccionadaFinanciero
        End Get
        Set(ByVal value As Integer)
            _TabSeleccionadaFinanciero = value
            MyBase.CambioItem("TabSeleccionadaFinanciero")
        End Set
    End Property

    Public ReadOnly Property FechaActual As DateTime
        Get
            Return Now.Date
        End Get
    End Property

    ''' <summary>
    ''' Propiedad que determina si estan visibles campos de Acciones
    ''' </summary>
    ''' <remarks></remarks>
    Private _VisualizarAcciones As Visibility = Visibility.Visible
    Public Property VisualizarAcciones() As Visibility
        Get
            Return _VisualizarAcciones
        End Get
        Set(ByVal value As Visibility)
            _VisualizarAcciones = value
            MyBase.CambioItem("VisualizarAcciones")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad que determina si esta visible el campos Democratizacion (Aplica solo para acciones)
    ''' Descripción:   Se agrega el campo "Democratización" (Aplica solo para acciones).
    ''' Responsable:   Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:         5 de Mayo/2016
    ''' </summary>
    ''' <remarks></remarks>
    Private _VisualizarDemocratizacion As Visibility = Visibility.Collapsed
    Public Property VisualizarDemocratizacion() As Visibility
        Get
            Return _VisualizarDemocratizacion
        End Get
        Set(ByVal value As Visibility)
            _VisualizarDemocratizacion = value
            MyBase.CambioItem("VisualizarDemocratizacion")
        End Set
    End Property

    ''' <summary>
    ''' Propiedad que determina si estan visibles campos de renta fija
    ''' </summary>
    ''' <remarks></remarks>
    Private _VisualizarRentaFija As Visibility = Visibility.Visible
    Public Property VisualizarRentaFija() As Visibility
        Get
            Return _VisualizarRentaFija
        End Get
        Set(ByVal value As Visibility)
            _VisualizarRentaFija = value
            MyBase.CambioItem("VisualizarRentaFija")
        End Set
    End Property

    Private _HabilitarDuplicarRegistro As Boolean = True
    Public Property HabilitarDuplicarRegistro() As Boolean
        Get
            Return _HabilitarDuplicarRegistro
        End Get
        Set(ByVal value As Boolean)
            _HabilitarDuplicarRegistro = value
            MyBase.CambioItem("HabilitarDuplicarRegistro")
        End Set
    End Property

    Private Property _AltoBotonDuplicar As Integer = 19
    Public Property AltoBotonDuplicar() As Integer
        Get
            Return _AltoBotonDuplicar
        End Get
        Set(ByVal value As Integer)
            _AltoBotonDuplicar = value
            MyBase.CambioItem("AltoBotonDuplicar")
        End Set
    End Property

    ' ''' <summary>
    ' ''' Este metodo es para llenar el control Plazo el Cual resulta de la diferencia de la fecha de Emisión y la fecha de Vencimiento
    ' ''' </summary>
    ' ''' <remarks>
    ' ''' Jeison Ramírez Pino
    ' ''' </remarks>
    'Public Sub DiferenciaPlazo()
    '    Try
    '        If IsDate(EspecieSelected.Emision.Value.Date) And IsDate(EspecieSelected.Vencimiento.Value.Date) Then
    '            EspecieSelected.Plazo = (CStr(DateDiff("d", EspecieSelected.Emision.Value.Date, EspecieSelected.Vencimiento.Value.Date)) & " dias")
    '        Else
    '            EspecieSelected.Plazo.Equals("")
    '        End If
    '    Catch ex As Exception
    '        IsBusy = False
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
    '                                                     Me.ToString(), "DiferenciaPlazo", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub

    Private _Clase As String = "1"
    Public Property Clase As String
        Get
            Return _Clase
        End Get
        Set(ByVal value As String)
            _Clase = value
            MyBase.CambioItem("Clase")
        End Set
    End Property

    Private _claseinversionSelected As claseinversion = New claseinversion
    Public Property claseinversionSelected As claseinversion
        Get
            Return _claseinversionSelected
        End Get
        Set(ByVal value As claseinversion)
            _claseinversionSelected = value
            MyBase.CambioItem("claseinversionSelected")
        End Set
    End Property

    Private _HabilitarDeclaraDividendos As Boolean = True
    Public Property HabilitarDeclaraDividendos As Boolean
        Get
            Return _HabilitarDeclaraDividendos
        End Get
        Set(ByVal value As Boolean)
            _HabilitarDeclaraDividendos = value
            MyBase.CambioItem("HabilitarDeclaraDividendos")
        End Set
    End Property

    Private _ListaComboMoneda As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaComboMoneda As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaComboMoneda
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaComboMoneda = value
            MyBase.CambioItem("ListaComboMoneda")
        End Set
    End Property

    Private _ListaComboCurvaCC As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaComboCurvaCC As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaComboCurvaCC
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaComboCurvaCC = value
            MyBase.CambioItem("ListaComboCurvaCC")
        End Set
    End Property

    Private _ListaComboClaseInversion As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaComboClaseInversion As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaComboClaseInversion
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaComboClaseInversion = value
            MyBase.CambioItem("ListaComboClaseInversion")
        End Set
    End Property

    Private _ListaComboClassificacionInversion As List(Of OYDUtilidades.ItemCombo)
    Public Property ListaComboClassificacionInversion As List(Of OYDUtilidades.ItemCombo)
        Get
            Return _ListaComboClassificacionInversion
        End Get
        Set(ByVal value As List(Of OYDUtilidades.ItemCombo))
            _ListaComboClassificacionInversion = value
            MyBase.CambioItem("ListaComboClassificacionInversion")
        End Set
    End Property

    '#Region "Commands"
    '    Private WithEvents _validarEspecie As RelayCommand
    '    Public ReadOnly Property ValidarEspecie() As RelayCommand
    '        Get
    '            If _validarEspecie Is Nothing Then
    '                _validarEspecie = New RelayCommand(AddressOf ValidarEspecieLanzarISIN)
    '            End If
    '            Return _validarEspecie
    '        End Get
    '    End Property
    '#End Region

    Private _viewEspecie As EspeciesView
    Public Property viewEspecie() As EspeciesView
        Get
            Return _viewEspecie
        End Get
        Set(ByVal value As EspeciesView)
            _viewEspecie = value
        End Set
    End Property

    Private _habilitarLinkISIN As Boolean
    Public Property habilitarLinkISIN() As Boolean
        Get
            Return _habilitarLinkISIN
        End Get
        Set(ByVal value As Boolean)
            _habilitarLinkISIN = value
            MyBase.CambioItem("habilitarLinkISIN")
        End Set
    End Property

    Private _ListaEspeciesDeposito As List(Of EspeciesDeposito)
    Public Property ListaEspeciesDeposito As List(Of EspeciesDeposito)
        Get
            Return _ListaEspeciesDeposito
        End Get
        Set(ByVal value As List(Of EspeciesDeposito))
            _ListaEspeciesDeposito = value
            MyBase.CambioItem("ListaEspeciesDeposito")
        End Set
    End Property

    Private _listaclasificacion As List(Of CFMaestros.Clasificacion)
    Public Property listaclasificacion As List(Of CFMaestros.Clasificacion)
        Get
            Return _listaclasificacion
        End Get
        Set(ByVal value As List(Of CFMaestros.Clasificacion))
            _listaclasificacion = value
            MyBase.CambioItem("listaclasificacion")
        End Set
    End Property

    Private _listasubGrupo As List(Of CFMaestros.Clasificacion)
    Public Property listasubGrupo As List(Of CFMaestros.Clasificacion)
        Get
            Return _listasubGrupo
        End Get
        Set(ByVal value As List(Of CFMaestros.Clasificacion))
            _listasubGrupo = value
            MyBase.CambioItem("listasubGrupo")
        End Set
    End Property

    Private _DescripcionClaseInversion As String
    Public Property DescripcionClaseInversion() As String
        Get
            Return _DescripcionClaseInversion
        End Get
        Set(ByVal value As String)
            _DescripcionClaseInversion = value
            MyBase.CambioItem("DescripcionClaseInversion")
        End Set
    End Property

#End Region

#Region "Métodos"

    Public Sub DetalleClaseInversion()

        Dim Datos() As String

        If (claseinversionSelected.strDescripcion <> String.Empty) Then
            Datos = DescripcionClaseInversion.Split(",")

            Dim cwDetalleModalResultadoGenericoView = New cwDetalleModalResultadoGenericoView("Información adicional", Datos(0), claseinversionSelected.strDescripcion)
            Program.Modal_OwnerMainWindowsPrincipal(cwDetalleModalResultadoGenericoView)
            cwDetalleModalResultadoGenericoView.ShowDialog()
        End If

    End Sub



    Public Overrides Sub NuevoRegistro()
        Try
            Dim NewEspeci As New Especi
            IsinFungible = True
            If EspeciePorDefecto.EsAccion Then
                IsinFungibleBusquedad = False
            Else
                IsinFungibleBusquedad = True
            End If
            'TODO: Verificar cuales son los campos que deben inicializarse
            NewEspeci.IDComisionista = EspeciePorDefecto.IDComisionista
            NewEspeci.IDSucComisionista = EspeciePorDefecto.IDSucComisionista
            NewEspeci.Id = EspeciePorDefecto.Id
            NewEspeci.Nombre = EspeciePorDefecto.Nombre
            NewEspeci.EsAccion = EspeciePorDefecto.EsAccion
            NewEspeci.IDClase = Nothing 'EspeciePorDefecto.IDClase
            NewEspeci.IDTarifa = EspeciePorDefecto.IDTarifa
            NewEspeci.strAccion = "Acción"

            'SLB 20130403 Cargar el primero por defecto
            If IsNothing(_ListaComboGrupo) And IsNothing(_ListaComboSubGrupo) Then
                If Not IsNothing(CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))) Then
                    _ListaComboGrupo = Nothing 'CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("gruposE").FirstOrDefault
                    _ListaComboSubGrupo = Nothing 'CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).Item("subgruposE").FirstOrDefault
                End If
            End If

            If Not IsNothing(_ListaComboGrupo) Then
                NewEspeci.IDGrupo = _ListaComboGrupo.ID
            Else
                NewEspeci.IDGrupo = EspeciePorDefecto.IDGrupo
            End If
            If Not IsNothing(_ListaComboSubGrupo) Then
                NewEspeci.IDSubGrupo = _ListaComboSubGrupo.ID
            Else
                NewEspeci.IDSubGrupo = EspeciePorDefecto.IDSubGrupo
            End If

            NewEspeci.VlrNominal = EspeciePorDefecto.VlrNominal
            NewEspeci.Notas = EspeciePorDefecto.Notas
            NewEspeci.IdEmisor = EspeciePorDefecto.IdEmisor
            NewEspeci.IDAdmonEmision = EspeciePorDefecto.IDAdmonEmision
            NewEspeci.NombreAdmonEmision = EspeciePorDefecto.NombreAdmonEmision

            If Not IsNothing(ListaComboCurvaCC) Then
                NewEspeci.CurvaEspecie = 1
            End If

            If Not IsNothing(CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))) Then
                If CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey("CIRCULACION") Then
                    If CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))("CIRCULACION").Where(Function(I) I.ID = "N").Count > 0 Then
                        NewEspeci.LeyCirculacion = "N"
                    End If
                End If
            End If

            NewEspeci.Emision = EspeciePorDefecto.Emision
            NewEspeci.Vencimiento = EspeciePorDefecto.Vencimiento
            NewEspeci.Modalidad = EspeciePorDefecto.Modalidad
            NewEspeci.TasaInicial = EspeciePorDefecto.TasaInicial
            NewEspeci.TasaNominal = EspeciePorDefecto.TasaNominal
            NewEspeci.PeriodoPago = EspeciePorDefecto.PeriodoPago
            NewEspeci.DiaDesde = EspeciePorDefecto.DiaDesde
            NewEspeci.DiaHasta = EspeciePorDefecto.DiaHasta
            NewEspeci.Mercado = EspeciePorDefecto.Mercado
            NewEspeci.DeclaraDividendos = EspeciePorDefecto.DeclaraDividendos
            NewEspeci.TituloMaterializado = 0
            NewEspeci.Sector = EspeciePorDefecto.Sector
            NewEspeci.Activo = EspeciePorDefecto.Activo
            NewEspeci.Emisora = EspeciePorDefecto.Emisora
            NewEspeci.TipoTasaFija = EspeciePorDefecto.TipoTasaFija
            NewEspeci.Actualizacion = EspeciePorDefecto.Actualizacion
            NewEspeci.Usuario = Program.Usuario
            NewEspeci.BusIntegracion = EspeciePorDefecto.BusIntegracion
            NewEspeci.Suscripcion = EspeciePorDefecto.Suscripcion
            NewEspeci.CaracteristicasRF = EspeciePorDefecto.CaracteristicasRF
            NewEspeci.Bursatilidad = EspeciePorDefecto.Bursatilidad
            NewEspeci.ClaseInversion = EspeciePorDefecto.ClaseInversion
            NewEspeci.Corresponde = EspeciePorDefecto.Corresponde
            NewEspeci.BaseCalculoInteres = EspeciePorDefecto.BaseCalculoInteres
            NewEspeci.RefTasaVble = EspeciePorDefecto.RefTasaVble
            NewEspeci.Amortiza = EspeciePorDefecto.Amortiza
            NewEspeci.ClaseAcciones = EspeciePorDefecto.ClaseAcciones
            NewEspeci.Liquidez = EspeciePorDefecto.Liquidez
            'NewEspeci.Liquidez = Nothing
            NewEspeci.Negociable = EspeciePorDefecto.Negociable
            NewEspeci.IDBolsa = EspeciePorDefecto.IDBolsa
            'NewEspeci.IDEspecies = ListaEspecies.Last.IDEspecies + 1
            NewEspeci.IDEspecies = -1
            'NewEspeci.Valor = EspecieSelected.Valor
            NewEspeci.Valor = EspeciePorDefecto.Valor
            NewEspeci.Usuario = Program.Usuario
            NewEspeci.AdmisionGarantia = EspeciePorDefecto.AdmisionGarantia
            NewEspeci.Indicador = EspeciePorDefecto.Indicador
            NewEspeci.ConceptoRetencion = -1
            _mlogExisteNemotblEspecies = False
            _mlogExisteNemotblEspeciesBolsa = False
            EspecieAnterior = EspecieSelected
            EspecieSelected = NewEspeci
            MyBase.CambioItem("Especies")
            Editando = True
            habilitarLinkISIN = Editando
            'HabilitarTipo = True
            'HabilitarCamposTituloParticipativo = False  'JAEZ 20161128
            HabilitarCamposTabCFTituloParticipativo = True
            NewEspeci.TituloParticipativo = False
            NewEspeci.logDemocratizacion = False
            HabilitarTituloMaterializado = False

            Inhabilitar = True ' EOMC -- Se habilitan controles para acciones -- 08/08/2013
            InhabilitarBursatilidad = False

            HabilitarCodigo = True
            HabilitarCodigo2 = True
            HabilitarNemotecnico = True
            HabilitarDuplicarRegistro = False
            AltoBotonDuplicar = 23
            _InhabilitarDetalles = False
            logDuplicar = False
            MyBase.CambioItem("Editando")

            EmisorClaseSelected.IdEmisor = String.Empty
            EmisorClaseSelected.Emisor = String.Empty
            ClaseClaseSelected.IdClase = String.Empty
            ClaseClaseSelected.Clase = String.Empty
            EmisoresClaseSelected.IDAdmonEmision = String.Empty
            EmisoresClaseSelected.NombreAdmonEmision = String.Empty
            EmisoresClaseSelected.Emisores = String.Empty
            claseinversionSelected.strcodigo = String.Empty
            claseinversionSelected.strDescripcion = String.Empty

            ListaEspeciesISINFungible = Nothing
            EspeciesISINFungibleSelected = Nothing
            ListaEspeciesDeposito = EspeciesDepositoPorDefecto
            If Not IsNothing(EspecieSelected.IDGrupo) Then
                ConsultarListaSubgrupo()
            End If

            NombreColeccionDetalle = "cmEspeciesNemotecnicos"
            DescripcionClaseInversion = "Clase Inversión"
            NuevoRegistroDetalle()

            If EspecieSelected.TipoEspecie = "A" Then
                HabilitarTabArbitrajeADR = "Visible"
            Else
                HabilitarTabArbitrajeADR = "Hidden"
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de un nuevo registro", _
                                                         Me.ToString(), "NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


    ''' <history>
    ''' ID caso de prueba:  Id_12
    ''' Descripción:        Desarrollo PreEspecies. Debe de buscar tanto la especie como la preespecie.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              9 de Junio/2014
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 9 de Junio/2014
    ''' </history>
    Public Overrides Sub Filtrar()
        Try
            dcProxy.Especis.Clear()
            IsBusy = True
            If FiltroVM.Length > 0 Then
                Dim TextoFiltroSeguro = System.Web.HttpUtility.UrlEncode(FiltroVM)
                dcProxy.Load(dcProxy.EspeciesFiltrarQuery(TextoFiltroSeguro, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspecies, "FiltroVM")
                variables = False
            Else
                dcProxy.Load(dcProxy.EspeciesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspecies, "FiltroVM")
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la implementación del filtro", _
                                             Me.ToString(), "Filtrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub Buscar()
        MyBase.Buscar()
        cb.logActivo = 1
        cb.Nombre = ""
        cb.Id = ""
        cb.plngIdEmisor = 0
        cb.plngIDClase = 0
        cb.Clase = String.Empty
        cb.Emisor = String.Empty
        HabilitarDuplicarRegistro = False
        AltoBotonDuplicar = 21
    End Sub


    ''' <history>
    ''' ID caso de prueba:  Id_13
    ''' Descripción:        Desarrollo PreEspecies. Debe de buscar tanto la especie como la preespecie.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              9 de Junio/2014
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 9 de Junio/2014
    ''' </history> 
    ''' <history>
    ''' Descripción:        Se añade a la búsqueda avanzada la fecha de emisión y de vencimiento
    ''' Modificado:         Jhonatan Arley Acevedo Martínez (Alcuadrado S.A.)
    ''' Fecha:              14 de Septiembre/2015
    ''' </history>
    Public Overrides Sub ConfirmarBuscar()
        Try

            If cb.Id <> String.Empty Or cb.Nombre <> String.Empty Or cb.plngIdEmisor <> 0 Or cb.plngIDClase <> 0 Or cb.logActivo = True Or cb.logActivo = False Or cb.pdtmEmision <> Nothing Or cb.pdtmVencimiento <> Nothing Then
                ErrorForma = ""
                dcProxy.Especis.Clear()
                EspecieAnterior = Nothing
                IsBusy = True
                dcProxy.Load(dcProxy.EspeciesConsultarQuery(cb.Id, cb.Nombre,
                                                            cb.plngIdEmisor,
                                                            cb.plngIDClase,
                                                            cb.logActivo,
                                                            cb.pdtmEmision,
                                                            cb.pdtmVencimiento, Program.Usuario, Program.HashConexion),
                                                            AddressOf TerminoTraerEspecies,
                                                            "Busqueda")
                MyBase.ConfirmarBuscar()
                cb = New CamposBusquedaEspecie
                CambioItem("cb")

                EmisorClaseSelected.IdEmisor = String.Empty
                EmisorClaseSelected.Emisor = String.Empty
                ClaseClaseSelected.IdClase = String.Empty
                ClaseClaseSelected.Clase = String.Empty
                EmisoresClaseSelected.IDAdmonEmision = String.Empty
                EmisoresClaseSelected.NombreAdmonEmision = String.Empty
                EmisoresClaseSelected.Emisores = String.Empty
                claseinversionSelected.strcodigo = String.Empty
                claseinversionSelected.strDescripcion = String.Empty
                HabilitarDuplicarRegistro = True
                AltoBotonDuplicar = 19
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la busqueda del registro", _
             Me.ToString(), "ConfirmarBuscar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <history>
    ''' ID caso de prueba:  Id_14, Id_15, Id_17
    ''' Descripción:        Desarrollo PreEspecies. Prueba realizada para modificar e ingresar registros.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              9 de Junio/2014
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 9 de Junio/2014
    ''' </history>
    ''' <history>
    ''' ID caso de prueba:  CP0005, CP0006
    ''' Descripción:        Se eliminó la validación "Debe seleccionar la clase de inversión" porque ya no se utilizan en la pantalla de Especies. Cambio autorizado por Jorge Arango.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              29 de Octubre/2014
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 29 de Octubre/2014
    ''' </history>
    ''' <history>
    ''' ID caso de prueba:  CP0011, CP0012, CP0014, CP0015
    ''' Descripción:        Se comenta la validación "Debe seleccionar la calificación de inversión" porque se ocultó del maestro de especies para colocarla en el maestro de isines.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              5 de Marzo/2015
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 5 de Marzo/2015
    ''' </history>
    '''  ''' <history>
    ''' Id Cambio:          CAT20150901
    ''' Descripción:        Ajuste. Se añade validación para el usuario cuando, se debe de seleccionar tipo = estandarizada 
    ''' Responsable:        Javier Pardo (Alcuadrado S.A.)
    ''' Fecha:              21 de Julio/2015
    ''' Pruebas CB:         Javier Pardo (Alcuadrado S.A.) - 22 de Julio/2015
    ''' </history> 
    ''' <history>
    ''' Id Cambio:          JERF20181019
    ''' Descripción:        Se elimina la validación de la fecha de emisión cuando es Acción y titulo participativo
    ''' Responsable:        Juan Esteban Restrepo Franco (Alcuadrado S.A.)
    ''' Fecha:              19 de Octubre/2018
    ''' Pruebas CB:         Juan Esteban Restrepo Franco (Alcuadrado S.A.) - 19 de Octubre/2018
    ''' </history> 
    Public Overrides Sub ActualizarRegistro()
        Try

            IsBusy = True

            IsinFungible = True
            If EspecieSelected.EsAccion Then
                IsinFungibleBusquedad = False
            Else
                IsinFungibleBusquedad = True
            End If

            _mlogExisteNemotblEspecies = False
            If String.IsNullOrEmpty(EspecieSelected.Nombre) Then
                IsinFungible = True
                If EspecieSelected.EsAccion Then
                    IsinFungibleBusquedad = False
                Else
                    IsinFungibleBusquedad = True
                End If
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el nombre.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If
            If EspecieSelected.IdEmisor.Equals(0) Then
                IsinFungible = True
                If EspecieSelected.EsAccion Then
                    IsinFungibleBusquedad = False
                Else
                    IsinFungibleBusquedad = True
                End If
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el código del emisor.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If
            If EspecieSelected.IDClase.Equals(0) Then
                IsinFungible = True
                If EspecieSelected.EsAccion Then
                    IsinFungibleBusquedad = False
                Else
                    IsinFungibleBusquedad = True
                End If
                A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar el código de la clase.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            If EspecieSelected.TipoEspecie = "F" And EspecieSelected.EsAccion = False Then
                IsinFungible = True
                If EspecieSelected.EsAccion Then
                    IsinFungibleBusquedad = False
                Else
                    IsinFungibleBusquedad = True
                End If
                A2Utilidades.Mensajes.mostrarMensaje("Si la especie es Renta Fija, el tipo de la especie no puede ser Full Listing.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            If String.IsNullOrEmpty(EspecieSelected.TipoEspecie) Then
                IsinFungible = True
                If EspecieSelected.EsAccion Then
                    IsinFungibleBusquedad = False
                Else
                    IsinFungibleBusquedad = True
                End If
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el  tipo de especie .", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            If String.IsNullOrEmpty(EspecieSelected.LeyCirculacion) Then
                IsinFungible = True
                If EspecieSelected.EsAccion Then
                    IsinFungibleBusquedad = False
                Else
                    IsinFungibleBusquedad = True
                End If
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el campo ley circulación.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If
            If String.IsNullOrEmpty(EspecieSelected.Sector) Then
                IsinFungible = True
                If EspecieSelected.EsAccion Then
                    IsinFungibleBusquedad = False
                Else
                    IsinFungibleBusquedad = True
                End If
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el campo sector.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If
            If String.IsNullOrEmpty(EspecieSelected.Mercado) Then
                IsinFungible = True
                If EspecieSelected.EsAccion Then
                    IsinFungibleBusquedad = False
                Else
                    IsinFungibleBusquedad = True
                End If
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el campo mercado.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If
            'If ClaseClaseSelected.Clase = String.Empty Then
            '    A2Utilidades.Mensajes.mostrarMensaje("El Campo Clase no  puede quedar vacio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Exit Sub
            'End If
            'If EmisorClaseSelected.Emisor = String.Empty Then
            '    A2Utilidades.Mensajes.mostrarMensaje("El campo Emisor no puede quedar vacio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '    Exit Sub
            'End If

            If EmisoresClaseSelected.NombreAdmonEmision = String.Empty Then
                'A2Utilidades.Mensajes.mostrarMensaje("El campo Admón Emisión no puede quedar vacio.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                'JEPM20150722 Mostrar mensaje relacionado a código que no existe en lugar que está vacío el campo.
                IsinFungible = True
                If EspecieSelected.EsAccion Then
                    IsinFungibleBusquedad = False
                Else
                    IsinFungibleBusquedad = True
                End If
                A2Utilidades.Mensajes.mostrarMensaje("La Administración de la Emisión con el código " & EspecieSelected.IDAdmonEmision.ToString & " no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            If EspecieSelected.EsAccion Then

                If Not IsNothing(EspecieSelected.FechaBursatilidad) Then
                    If EspecieSelected.FechaBursatilidad.Value.Date > Now.Date Then
                        IsinFungible = True
                        If EspecieSelected.EsAccion Then
                            IsinFungibleBusquedad = False
                        Else
                            IsinFungibleBusquedad = True
                        End If
                        A2Utilidades.Mensajes.mostrarMensaje("La Act. Bursatil no puede ser mayor a la fecha actual.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                End If

            Else
                'Es Renta fija

                'SLB20130221 Cuando la especie es una accion el campo liquidez debe guardarse con Null
                _EspecieSelected.Liquidez = Nothing

                If IsNothing(EspecieSelected.TipoTasaFija) Then
                    IsinFungible = True
                    If EspecieSelected.EsAccion Then
                        IsinFungibleBusquedad = False
                    Else
                        IsinFungibleBusquedad = True
                    End If
                    A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el tipo de tasa.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsBusy = False
                    Exit Sub
                End If
            End If

            If EspecieSelected.TituloParticipativo Then
                If Not EspeciesISINFungibleSelected.logEsAccion Then
                    If IsNothing(EspeciesISINFungibleSelected.Fecha_Emision) Or IsNothing(EspeciesISINFungibleSelected.Fecha_Vencimiento) Then
                        A2Utilidades.Mensajes.mostrarMensaje("La especie es título participativo, debe ingresar las fechas de emisión y vencimiento para el ISIN.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = False
                        Exit Sub
                    End If
                End If
            End If

            If _mlogExisteNemotblEspecies Then
                IsinFungible = True
                If EspecieSelected.EsAccion Then
                    IsinFungibleBusquedad = False
                Else
                    IsinFungibleBusquedad = True
                End If
                A2Utilidades.Mensajes.mostrarMensaje("La especie con el código '" + EspecieSelected.Id + "' ya existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            If _mlogExisteNemotblEspeciesBolsa Then
                IsinFungible = True
                If EspecieSelected.EsAccion Then
                    IsinFungibleBusquedad = False
                Else
                    IsinFungibleBusquedad = True
                End If
                A2Utilidades.Mensajes.mostrarMensaje("En el detalle de Nemotécnico hay nemos que ya existen para otra Especies en las Bolsas Seleccionadas ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionado = EspecieTabs.Nemotecnicos
                IsBusy = False
                Exit Sub
            End If

            'SLB 20121113
            If ListaEspeciesNemotecnicos.Count > 0 Then
                For Each obj In ListaEspeciesNemotecnicos
                    If IsNothing(obj.Nemotecnico) Then
                        IsinFungible = True
                        If EspecieSelected.EsAccion Then
                            IsinFungibleBusquedad = False
                        Else
                            IsinFungibleBusquedad = True
                        End If
                        A2Utilidades.Mensajes.mostrarMensaje("Debe ingresar un nemotécnico", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        TabSeleccionado = EspecieTabs.Nemotecnicos
                        IsBusy = False
                        Exit Sub
                    End If

                    If IsNothing(obj.IDEspecie) Then
                        obj.IDEspecie = EspecieSelected.Id
                    End If
                Next
            End If


            For Each li In ListaEspeciesDeposito
                If IsNothing(li.strIDEspecie) Then
                    li.strIDEspecie = EspecieSelected.Id
                    li.strUsuario = Program.Usuario
                End If
            Next


            If ListaEspeciesDividendos.Count > 0 Then
                For Each led In ListaEspeciesDividendos
                    'If led.FinPago.Date < led.InicioPago.Date Then
                    '    A2Utilidades.Mensajes.mostrarMensaje("El Fin Pago no Debe ser Menor que el Inicio Pago.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    '    TabSeleccionado = EspecieTabs.Dividendos
                    '    Exit Sub
                    'End If
                    'If led.FinVigencia.Date < led.InicioVigencia.Date Then
                    '    A2Utilidades.Mensajes.mostrarMensaje("El Fin Vigencia no Debe ser Menor que el Inicio Vigencia.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    '    TabSeleccionado = EspecieTabs.Dividendos
                    '    Exit Sub
                    'End If

                    If (IsNothing(led.CantidadAcciones) Or led.CantidadAcciones = 0) _
                        And (IsNothing(led.Causacion) Or led.Causacion = 0) _
                        And (IsNothing(led.CantidadPesos) Or led.CantidadPesos = 0) Then
                        IsinFungible = True
                        If EspecieSelected.EsAccion Then
                            IsinFungibleBusquedad = False
                        Else
                            IsinFungibleBusquedad = True
                        End If
                        A2Utilidades.Mensajes.mostrarMensaje("Debe de ingresar al menos un valor para Valor Acciones Pesos, Factor Conversión o Precio Liberación en el detalle de Dividendos.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        TabSeleccionado = EspecieTabs.Dividendos
                        IsBusy = False
                        Exit Sub
                    End If

                    ' Se valida que la fecha de inicio pago del registro especifico no este repetida en otro de los registros
                    Dim fechaInicioPago = led.InicioPago.Date
                    Dim fechaInicioRepetida = From ld In ListaEspeciesDividendos Where ld.InicioPago.Date = fechaInicioPago
                                              Select ld

                    If fechaInicioRepetida.Count > 1 Then
                        IsinFungible = True
                        If EspecieSelected.EsAccion Then
                            IsinFungibleBusquedad = False
                        Else
                            IsinFungibleBusquedad = True
                        End If
                        A2Utilidades.Mensajes.mostrarMensaje("Fecha Pago Duplicada en el detalle de Dividendos", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        TabSeleccionado = EspecieTabs.Dividendos
                        IsBusy = False
                        Exit Sub
                    End If

                    led.IDCtrlDividendo = "NO"
                Next
            End If

            ' eomc -- 07/05/2013 -- Inicio
            'Campos requeridos por fuera del tab de cálculos financieros
            If EspecieSelected.ClaseInversion = Nothing Then
                IsinFungible = True
                If EspecieSelected.EsAccion Then
                    IsinFungibleBusquedad = False
                Else
                    IsinFungibleBusquedad = True
                End If
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar Clase Inversión", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If

            If Not EspecieSelected.EsAccion Then
                If IsNothing(EspecieSelected.BaseCalculoInteres) Or EspecieSelected.BaseCalculoInteres = 0 Then
                    IsinFungible = True
                    If EspecieSelected.EsAccion Then
                        IsinFungibleBusquedad = False
                    Else
                        IsinFungibleBusquedad = True
                    End If
                    A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la base de cálculo de interés", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    TabSeleccionado = EspecieTabs.CF
                    IsBusy = False
                    Exit Sub
                End If
            End If

            If (Not EspecieSelected.EsAccion) And EspecieSelected.TipoTasaFija = "V" Then
                If EspecieSelected.RefTasaVble <> "1" And EspecieSelected.RefTasaVble <> "2" Then
                    IsinFungible = True
                    If EspecieSelected.EsAccion Then
                        IsinFungibleBusquedad = False
                    Else
                        IsinFungibleBusquedad = True
                    End If
                    A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar 'Previo' o  'Vigente' para la referencia de la tasa variable", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    TabSeleccionado = EspecieTabs.CF
                    IsBusy = False
                    Exit Sub
                End If
            End If

            If Not EspecieSelected.EsAccion And EspecieSelected.Corresponde = Nothing Then
                IsinFungible = True
                If EspecieSelected.EsAccion Then
                    IsinFungibleBusquedad = False
                Else
                    IsinFungibleBusquedad = True
                End If
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar Corresponde a", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsBusy = False
                Exit Sub
            End If
            'Finalizan campos fuera del tab de cálculos financieros

            'Campos dentro del tab de cálculos financieros
            If IsNothing(EspecieSelected.IDMoneda) Or EspecieSelected.IDMoneda = 0 Then
                IsinFungible = True
                If EspecieSelected.EsAccion Then
                    IsinFungibleBusquedad = False
                Else
                    IsinFungibleBusquedad = True
                End If
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar una moneda", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionado = EspecieTabs.CF
                IsBusy = False
                Exit Sub
            End If

            If EspecieSelected.TipoCalculo = Nothing Then
                IsinFungible = True
                If EspecieSelected.EsAccion Then
                    IsinFungibleBusquedad = False
                Else
                    IsinFungibleBusquedad = True
                End If
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar Tipo de Cálculo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionado = EspecieTabs.CF
                IsBusy = False
                Exit Sub
            End If

            If EspecieSelected.EsAccion Then
                If IsNothing(EspecieSelected.IDCalificacionInversion) Or EspecieSelected.IDCalificacionInversion = 0 Then
                    IsinFungible = True
                    If EspecieSelected.EsAccion Then
                        IsinFungibleBusquedad = False
                    Else
                        IsinFungibleBusquedad = True
                    End If
                    A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la calificación de inversión", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    TabSeleccionado = EspecieTabs.CF
                    IsBusy = False
                    Exit Sub
                End If
            End If

            If IsNothing(_ListaEspeciesDeposito) Then
                IsinFungible = True
                If EspecieSelected.EsAccion Then
                    IsinFungibleBusquedad = False
                Else
                    IsinFungibleBusquedad = True
                End If
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un depósito", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionado = EspecieTabs.CF
                IsBusy = False
                Exit Sub
            End If

            If _ListaEspeciesDeposito.Where(Function(i) i.logSeleccionado).Count = 0 Then
                IsinFungible = True
                If EspecieSelected.EsAccion Then
                    IsinFungibleBusquedad = False
                Else
                    IsinFungibleBusquedad = True
                End If
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar un depósito", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionado = EspecieTabs.CF
                IsBusy = False
                Exit Sub
            End If

            'If Not EspecieSelected.EsAccion Then
            '    If IsNothing(EspecieSelected.CurvaEspecie) Or EspecieSelected.CurvaEspecie = 0 Then
            '        A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar la curva de la especie", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            '        TabSeleccionado = EspecieTabs.CF
            '        Exit Sub
            '    End If
            'End If

            If EspecieSelected.MacroActivo = Nothing Then
                IsinFungible = True
                If EspecieSelected.EsAccion Then
                    IsinFungibleBusquedad = False
                Else
                    IsinFungibleBusquedad = True
                End If
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar Macroactivo", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionado = EspecieTabs.CF
                IsBusy = False
                Exit Sub
            End If

            If String.IsNullOrEmpty(EspecieSelected.NitAvalista) Then
                EspecieSelected.CodigoAvalado = False
            Else
                EspecieSelected.CodigoAvalado = True
            End If

            If EspecieSelected.ClaseContableTitulo = Nothing Then
                IsinFungible = True
                If EspecieSelected.EsAccion Then
                    IsinFungibleBusquedad = False
                Else
                    IsinFungibleBusquedad = True
                End If
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar Clase Contable Título", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                TabSeleccionado = EspecieTabs.CF
                IsBusy = False
                Exit Sub
            End If
            ' Finaliza tab  cálculos financieros

            ' eomc -- 07/05/2013 -- Fin


            If (EspecieSelected.TipoEspecie = "E") Then

                If IsNothing(ListaEspeciesISINFungible) Then
                    A2Utilidades.Mensajes.mostrarMensaje("No se han ingresado ISIN fungibles.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    TabSeleccionado = EspecieTabs.IsinFungible
                    IsBusy = False
                    Exit Sub
                Else
                    If ListaEspeciesISINFungible.Count = 0 Then
                        A2Utilidades.Mensajes.mostrarMensaje("No se han ingresado ISIN fungibles.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        TabSeleccionado = EspecieTabs.IsinFungible
                        IsBusy = False
                        Exit Sub
                    Else
                        ' EOMC -- concatena los id's de los isines generados -- 08/08/2013 
                        Dim s As String = (ListaEspeciesISINFungible.Select(Function(x) x.IDIsinFungible.ToString())).Aggregate(Function(a, b) (a + "," + b))
                        EspecieSelected.ISINesInsertar = s
                    End If
                End If

            End If

            'Asignar la lista de ISINes si el usuario ingresó detalles
            If Not IsNothing(ListaEspeciesISINFungible) Then
                If ListaEspeciesISINFungible.Count > 0 Then
                    Dim s As String = (ListaEspeciesISINFungible.Select(Function(x) x.IDIsinFungible.ToString())).Aggregate(Function(a, b) (a + "," + b))
                    EspecieSelected.ISINesInsertar = s
                End If
            End If

            Dim xmlCompleto As String = Nothing
            Dim xmlDetalle As String

            If Not IsNothing(ListaArbitrajeADR) Then
                For Each item In ListaArbitrajeADR
                    If item.strAccion = ValoresUserState.Ingresar.ToString Or item.HasChanges Then
                        Dim Fecha As String
                        If IsNothing(item.dtmVigencia) Then
                            Fecha = ""
                        Else
                            Fecha = item.dtmVigencia.Value.ToString("yyyy-MM-dd")
                        End If
                        xmlDetalle = "<Detalle strAccion=""" & item.strAccion &
                                     """ intIDArbitrajeADR=""" & item.intIDArbitrajeADR &
                                     """ dtmVigencia = """ & Fecha &
                                     """ strIDEspecie=""" & item.strIDEspecie &
                                     """ dblValor=""" & item.dblValor &
                                     """ />"
                        xmlCompleto = xmlCompleto & xmlDetalle
                    End If
                Next
            End If

            If Not IsNothing(ListaArbitrajeADREliminado) Then
                For Each item In ListaArbitrajeADREliminado.Where(Function(i) i.intIDArbitrajeADR <> 0)
                    xmlDetalle = "<Detalle strAccion=""" & ValoresUserState.Borrar.ToString() &
                                 """ intIDArbitrajeADR=""" & item.intIDArbitrajeADR &
                                 """ dtmVigencia = """ & item.dtmVigencia.Value.ToString("yyyy-MM-dd") &
                                 """ strIDEspecie=""" & item.strIDEspecie &
                                 """ dblValor=""" & item.dblValor &
                                 """ />"
                    xmlCompleto = xmlCompleto & xmlDetalle
                Next
            End If

            If xmlCompleto <> Nothing Then
                EspecieSelected.xmlDetalleADR = "<ArbitrajesDetalle>" & xmlCompleto & "</ArbitrajesDetalle>"
            End If

            If Not ListaEspecies.Contains(EspecieSelected) Then
                origen = "insert"
                mostrarMensajePregunta("¿Desea guardar la especie " + EspecieSelected.Id + "?",
                                         Program.TituloSistema,
                                         "GUARDAR",
                                         AddressOf TerminaPregunta, False)
                'C1.Silverlight.C1MessageBox.Show("¿Desea guardar la especie " + EspecieSelected.Id + "?", Program.TituloSistema, C1.Silverlight.C1MessageBoxButton.YesNo, C1.Silverlight.C1MessageBoxIcon.Question, AddressOf TerminaPregunta)
                IsinFungibleBusquedad = False
            Else
                origen = "update"
                ErrorForma = ""
                EspecieAnterior = EspecieSelected
                Program.VerificarCambiosProxyServidor(dcProxy)
                dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
                IsBusy = True
            End If

            HabilitarTituloMaterializado = False
            InhabilitarDetallePrecio = False
            EditandoPrecio = False
            IsinFungible = False
            IsinFungibleBusquedad = False

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la actualización del registro", _
                                 Me.ToString(), "ActualizarRegistro", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    Private Sub TerminaPregunta(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
            If Not IsNothing(objResultado) Then
                If Not IsNothing(objResultado.CodigoLlamado) Then
                    Select Case objResultado.CodigoLlamado.ToUpper
                        Case "GUARDAR"
                            If objResultado.DialogResult Then
                                If Not IsNothing(EspecieSelected) Then
                                    ListaEspecies.Add(EspecieSelected)
                                    Program.VerificarCambiosProxyServidor(dcProxy)
                                    dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, origen)
                                Else
                                    IsinFungible = False
                                End If
                            Else
                                IsinFungible = True
                                IsBusy = False  'JAEZ 20161128 
                            End If
                        Case "DUPLICARREGISTRO"
                            If objResultado.DialogResult Then
                                DuplicarRegistro()
                                Messenger.Default.Send(Of Especi)(Nothing) ' Necesario para poner el focus en el textbox del Nemotécnico
                            Else
                                IsinFungible = True
                            End If
                    End Select
                End If
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en TerminaPregunta", _
             Me.ToString(), "TeminaPregunta", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub DuplicarRegistro()
        Try
            logDuplicar = True
            'logLimpiarTituloParticipativo = False
            EspecieAnterior = Nothing

            'Crea la nueva especie para duplicar.
            Dim objNewRegistroDuplicarEspecie As New CFEspecies.Especi

            ObtenerValoresRegistroAnterior(_EspecieSelected, objNewRegistroDuplicarEspecie)

            objNewRegistroDuplicarEspecie.Nombre = ""
            objNewRegistroDuplicarEspecie.Id = Nothing
            objNewRegistroDuplicarEspecie.IDEspecies = -1
            objNewRegistroDuplicarEspecie.Actualizacion = Now
            objNewRegistroDuplicarEspecie.Usuario = Program.Usuario

            ObtenerValoresRegistroAnterior(_EspecieSelected, EspecieAnterior)

            EspecieSelected = objNewRegistroDuplicarEspecie

            ListaEspeciesISINFungible.Clear()
            DetalleNemotecnico()
            EspeciesISINSelected = Nothing

            Editando = True

            EditandoPrecio = False
            habilitarLinkISIN = Editando
            HabilitarCodigo = False
            HabilitarNemotecnico = True
            IsinFungible = True
            InhabilitarDetalles = False
            HabilitarDuplicarRegistro = False
            AltoBotonDuplicar = 24
            If (EspecieSelected.TipoEspecie = "N") Then
                HabilitarTituloMaterializado = True
            Else
                HabilitarTituloMaterializado = False
            End If

            If (EspecieSelected.TipoTasaFija = "F" Or EspecieSelected.TipoTasaFija = Nothing) Then
                HabilitarIndicador = False
            Else
                HabilitarIndicador = True
            End If

            If _EspecieSelected.EsAccion Then
                Habilitar = False
                HabilitarIndicador = False
                Inhabilitar = True
                InhabilitarBursatilidad = False
                IsinFungibleBusquedad = False
                HabilitarCodigo2 = False
                EspecieSelected.TituloMaterializado = False
                HabilitarTituloMaterializado = False
                'HabilitarTipo = True
                'HabilitarCamposTituloParticipativo = False  'JAEZ 20161128
            Else
                Habilitar = True
                Inhabilitar = False
                InhabilitarBursatilidad = True
                IsinFungibleBusquedad = True
                HabilitarTituloMaterializado = True
                'ConsultarValoresDefectoTituloParticipativo()
            End If

            HabilitarPropiedadTabCFTituloParticipativo()

            'HabilitarCamposTabCFTituloParticipativo = True

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la inicialización del registro duplicado", Me.ToString(), "DuplicarRegistro", Program.TituloSistema, Program.Maquina, ex)
            Editando = False

            ObtenerValoresRegistroAnterior(_EspecieSelected, Nothing)
        End Try
    End Sub

    Public Sub ObtenerValoresRegistroAnterior(ByVal pobjRegistro As CFEspecies.Especi, ByRef pobjRegistroSalvarDatos As CFEspecies.Especi)
        Try
            If Not IsNothing(pobjRegistro) Then
                Dim objNewRegistro As New CFEspecies.Especi

                objNewRegistro.Actualizacion = pobjRegistro.Actualizacion
                objNewRegistro.AdmisionGarantia = pobjRegistro.AdmisionGarantia
                objNewRegistro.Amortiza = pobjRegistro.Amortiza
                objNewRegistro.BaseCalculoInteres = pobjRegistro.BaseCalculoInteres
                objNewRegistro.Bursatilidad = pobjRegistro.Bursatilidad
                objNewRegistro.BusIntegracion = pobjRegistro.BusIntegracion
                objNewRegistro.CaracteristicasRF = pobjRegistro.CaracteristicasRF
                objNewRegistro.ClaseAcciones = pobjRegistro.ClaseAcciones
                objNewRegistro.ClaseContableTitulo = pobjRegistro.ClaseContableTitulo
                objNewRegistro.ClaseInversion = pobjRegistro.ClaseInversion
                objNewRegistro.ClasificacionRiesgo = pobjRegistro.ClasificacionRiesgo
                objNewRegistro.CodigoAvalado = pobjRegistro.CodigoAvalado
                objNewRegistro.Corresponde = pobjRegistro.Corresponde
                objNewRegistro.CurvaEspecie = pobjRegistro.CurvaEspecie
                objNewRegistro.DeclaraDividendos = pobjRegistro.DeclaraDividendos
                objNewRegistro.DiaDesde = pobjRegistro.DiaDesde
                objNewRegistro.DiaHasta = pobjRegistro.DiaHasta
                objNewRegistro.Emision = pobjRegistro.Emision
                objNewRegistro.Emisora = pobjRegistro.Emisora
                objNewRegistro.EsAccion = pobjRegistro.EsAccion
                objNewRegistro.FactorRiesgo = pobjRegistro.FactorRiesgo
                objNewRegistro.FechaBursatilidad = pobjRegistro.FechaBursatilidad
                objNewRegistro.Id = pobjRegistro.Id
                objNewRegistro.IDAdmonEmision = pobjRegistro.IDAdmonEmision
                objNewRegistro.IDBolsa = pobjRegistro.IDBolsa
                objNewRegistro.IDCalificacionInversion = pobjRegistro.IDCalificacionInversion
                objNewRegistro.IDClase = pobjRegistro.IDClase
                objNewRegistro.IDComisionista = pobjRegistro.IDComisionista
                objNewRegistro.IdEmisor = pobjRegistro.IdEmisor
                objNewRegistro.IDEspecies = pobjRegistro.IDEspecies
                objNewRegistro.IDGrupo = pobjRegistro.IDGrupo
                objNewRegistro.IDMoneda = pobjRegistro.IDMoneda
                objNewRegistro.IDSubGrupo = pobjRegistro.IDSubGrupo
                objNewRegistro.IDSucComisionista = pobjRegistro.IDSucComisionista
                objNewRegistro.IDTarifa = pobjRegistro.IDTarifa
                objNewRegistro.Indicador = pobjRegistro.Indicador
                objNewRegistro.InfoSesion = pobjRegistro.InfoSesion
                objNewRegistro.ISINesInsertar = pobjRegistro.ISINesInsertar
                objNewRegistro.LeyCirculacion = pobjRegistro.LeyCirculacion
                objNewRegistro.Liquidez = pobjRegistro.Liquidez
                objNewRegistro.MacroActivo = pobjRegistro.MacroActivo
                objNewRegistro.Mercado = pobjRegistro.Mercado
                objNewRegistro.Modalidad = pobjRegistro.Modalidad
                objNewRegistro.Negociable = pobjRegistro.Negociable
                objNewRegistro.NitAdministradorAutonomo = pobjRegistro.NitAdministradorAutonomo
                objNewRegistro.NitAvalista = pobjRegistro.NitAvalista
                objNewRegistro.Nombre = pobjRegistro.Nombre
                objNewRegistro.NombreAdmonEmision = pobjRegistro.NombreAdmonEmision
                objNewRegistro.Notas = pobjRegistro.Notas
                objNewRegistro.NroAcciones = pobjRegistro.NroAcciones
                objNewRegistro.PeriodoPago = pobjRegistro.PeriodoPago
                objNewRegistro.Plazo = pobjRegistro.Plazo
                objNewRegistro.PrefijoFactorRiesgo = pobjRegistro.PrefijoFactorRiesgo
                objNewRegistro.RefTasaVble = pobjRegistro.RefTasaVble
                objNewRegistro.Sector = pobjRegistro.Sector
                objNewRegistro.strAccion = pobjRegistro.strAccion
                objNewRegistro.strClase = pobjRegistro.strClase
                objNewRegistro.strEmisor = pobjRegistro.strEmisor
                objNewRegistro.strPrecreada = pobjRegistro.strPrecreada
                objNewRegistro.Suscripcion = pobjRegistro.Suscripcion
                objNewRegistro.TasaInicial = pobjRegistro.TasaInicial
                objNewRegistro.TasaNominal = pobjRegistro.TasaNominal
                objNewRegistro.TipoCalculo = pobjRegistro.TipoCalculo
                objNewRegistro.TipoEspecie = pobjRegistro.TipoEspecie
                objNewRegistro.TipoTasaFija = pobjRegistro.TipoTasaFija
                objNewRegistro.TituloMaterializado = pobjRegistro.TituloMaterializado
                objNewRegistro.Usuario = pobjRegistro.Usuario
                objNewRegistro.Valor = pobjRegistro.Valor
                objNewRegistro.Vencimiento = pobjRegistro.Vencimiento
                objNewRegistro.VlrNominal = pobjRegistro.VlrNominal
                objNewRegistro.Activo = pobjRegistro.Activo
                objNewRegistro.ConceptoRetencion = pobjRegistro.ConceptoRetencion
                objNewRegistro.TituloParticipativo = pobjRegistro.TituloParticipativo
                objNewRegistro.logDemocratizacion = pobjRegistro.logDemocratizacion

                pobjRegistroSalvarDatos = objNewRegistro
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al obtener los datos de la especie anterior.", Me.ToString(), "ObtenerValoresRegistroAnterior", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    'Public Sub ConsultarValoresDefectoTituloParticipativo()
    '    Try
    '        If Not IsNothing(EspecieSelected) Then
    '            If Not EspecieSelected.EsAccion Then ' And EspecieSelected.TituloParticipativo Then
    '                If Not IsNothing(CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS),  _
    '                             Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))) Then
    '                    If CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS),  _
    '                        Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey("TIPOESPECIE") Then
    '                        If CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS),  _
    '                            Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))("TIPOESPECIE").Where(Function(I) I.ID = "N").Count > 0 Then
    '                            EspecieSelected.TipoEspecie = "N" ' Indica No estandarizada por defecto
    '                        End If
    '                    End If

    '                    If CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS),  _
    '                        Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey("TIPOTASARF") Then
    '                        If CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS),  _
    '                            Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))("TIPOTASARF").Where(Function(I) I.ID = "F").Count > 0 Then
    '                            EspecieSelected.TipoTasaFija = "F" ' Indica Tasa Fija por defecto
    '                        End If
    '                    End If

    '                    If CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS),  _
    '                        Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey("REF_TASAVBLE_ESPECIE") Then
    '                        If CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS),  _
    '                            Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))("REF_TASAVBLE_ESPECIE").Where(Function(I) I.ID = "3").Count > 0 Then
    '                            EspecieSelected.RefTasaVble = "3" ' Indica N por defecto
    '                        End If
    '                    End If

    '                    If CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS),  _
    '                        Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey("BASECALINTERESES_ESP") Then
    '                        If CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS),  _
    '                            Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))("BASECALINTERESES_ESP").Where(Function(I) I.ID = "2").Count > 0 Then
    '                            EspecieSelected.BaseCalculoInteres = "2" ' Indica 365 por defecto
    '                        End If
    '                    End If

    '                    If CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS),  _
    '                        Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey("ESPECIE_CORRESPONDE") Then
    '                        If CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS),  _
    '                            Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))("ESPECIE_CORRESPONDE").Where(Function(I) I.ID = "0").Count > 0 Then
    '                            EspecieSelected.Corresponde = "0" ' Indica No aplica por defecto
    '                        End If
    '                    End If

    '                    If CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS),  _
    '                        Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey("AMORTIZACION_ESPECIE") Then
    '                        If CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS),  _
    '                            Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))("AMORTIZACION_ESPECIE").Where(Function(I) I.ID = "1").Count > 0 Then
    '                            EspecieSelected.Amortiza = "1" ' Indica No tiene amortizaciones de capital por defecto
    '                        End If
    '                    End If

    '                    If CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS),  _
    '                        Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey("CURVACC") Then
    '                        If CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS),  _
    '                            Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))("CURVACC").Where(Function(I) I.ID = "1").Count > 0 Then
    '                            EspecieSelected.CurvaEspecie = 1  ' Indica No, por defecto
    '                        End If
    '                    End If

    '                    For Each li In ListaEspeciesDeposito
    '                        li.logSeleccionado = False
    '                    Next

    '                    For Each li In ListaEspeciesDeposito.Where(Function(i) i.strDeposito = "D")
    '                        li.logSeleccionado = True
    '                    Next

    '                End If
    '            ElseIf Not EspecieSelected.EsAccion And logLimpiarTituloParticipativo Then
    '                EspecieSelected.TipoEspecie = String.Empty
    '                EspecieSelected.TipoTasaFija = String.Empty
    '                EspecieSelected.RefTasaVble = String.Empty
    '                EspecieSelected.BaseCalculoInteres = Nothing
    '                EspecieSelected.Corresponde = String.Empty
    '                EspecieSelected.Amortiza = String.Empty
    '                For Each li In ListaEspeciesDeposito
    '                    li.logSeleccionado = False
    '                Next
    '                logDuplicar = False
    '            End If
    '        End If
    '    Catch ex As Exception
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al tratar de obtener los valores por defecto para las especies seleccionadas como título participativo.", _
    '                                                     Me.ToString(), "ConsultarValoresDefectoTituloParticipativo", Program.TituloSistema, _
    '                                                     Program.Maquina, ex, Program.RutaServicioLog)
    '    End Try
    'End Sub


    Public Overrides Sub TerminoSubmitChanges(ByVal So As SubmitOperation)
        Try
            IsBusy = False
            If So.HasError Then

                If (So.Error.Message.Contains("ErrorPersonalizado,") = True) Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorPersonalizado,") '.Split(So.Error.Message, vbCr)
                    Dim Mensaje = Split(Mensaje1(1), vbCr)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0).Substring(Mensaje(0).IndexOf("failed.") + 7, Mensaje(0).Length), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                    Exit Try
                    'If So.UserState = "insert" Then
                    '    ListaDiasNoHabiles.Remove(DiasNoHabileSelected)
                    'End If
                    'Else
                    '    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                    '                      Me.ToString(), "TerminoSubmitChanges" & So.UserState.ToString(), Application.Current.ToString(), Program.Maquina, So.Error)
                ElseIf (So.Error.Message.Contains("Descripcion:") = True) Then
                    A2Utilidades.Mensajes.mostrarMensaje(So.Error.ToString.Substring(So.Error.ToString.IndexOf("failed.") + 20, (So.Error.ToString.Length - (So.Error.ToString.IndexOf("failed.") + 20))), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                    Exit Try
                ElseIf (So.Error.Message.Contains("Full Listing,") = True) Then
                    Dim Mensaje1 = Split(So.Error.Message, "Full Listing,")
                    Dim Mensaje = Split(Mensaje1(1), "|")
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje(0), Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsinFungible = True
                    IsinFungibleBusquedad = True
                    So.MarkErrorAsHandled()
                    Exit Try

                ElseIf So.Error.ToString.Contains("Ya existe el isin") Then
                    Dim intPosIni As Integer = So.Error.ToString.IndexOf("Ya existe el isin")
                    Dim intPosFin As Integer = So.Error.ToString.IndexOf("|")
                    Dim Mensaje = So.Error.ToString.Substring(intPosIni, intPosFin - intPosIni)
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    IsinFungible = True
                    IsinFungibleBusquedad = True
                    So.MarkErrorAsHandled()
                    Exit Try
                ElseIf So.Error.ToString.Contains("ErrorGenerico,") Then
                    Dim Mensaje1 = Split(So.Error.Message, "ErrorGenerico,")
                    Dim Mensaje = Replace(Mensaje1(1), "|", vbNewLine)
                    TabSeleccionado = EspecieTabs.ArbitrajeADR
                    A2Utilidades.Mensajes.mostrarMensaje(Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                    Exit Try
                End If


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
                    A2Utilidades.Mensajes.mostrarMensaje(So.Error.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If
                If Not strMsg.Equals(String.Empty) Then
                    A2Utilidades.Mensajes.mostrarMensaje(strMsg, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    So.MarkErrorAsHandled()
                End If

            Else
                Inhabilitar = False
                InhabilitarBursatilidad = False
                Habilitar = False
                HabilitarIndicador = False
                'HabilitarTipo = False
                'HabilitarCamposTituloParticipativo = False
                HabilitarCamposTabCFTituloParticipativo = False
                'Santiago Vergara - Marzo 12/2014 - Se deshabilita el códifgo si guarda correctamente
                HabilitarCodigo = False
                HabilitarNemotecnico = False
                logDuplicar = False
                HabilitarDuplicarRegistro = True
                AltoBotonDuplicar = 19
                If blnMostrarBarraBoton Then habilitarLinkISIN = Habilitar Else habilitarLinkISIN = True
            End If

            If Not String.IsNullOrEmpty(EspecieSelected.Id) Then
                dcProxy.Load(dcProxy.EspeciesISINFungibleConsultarQuery(EspecieSelected.Id, String.Empty, EspecieSelected.TipoTasaFija, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesISINFungible, Nothing)
                dcProxy.Load(dcProxy.EspecieArbitrajeADR_ConsultarQuery(EspecieSelected.Id, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArbitrajeADR, Nothing)
            End If



            MyBase.TerminoSubmitChanges(So)
            'If ListaEspeciesISIN.Count = 0 Then
            '    IsinFungible = False
            'Else
            '    IsinFungible = True
            'End If

            If Not IsNothing(EspecieSelected) Then
                strEspeciePosicionar = EspecieSelected.Id
            End If

            ConsultarListaSubgrupo()

            If Not IsNothing(dcProxy.Especis) Then
                dcProxy.Especis.Clear()
            End If
            dcProxy.Load(dcProxy.EspeciesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspecies, "TerminoGuardar")

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al reflejar en el servidor los cambios realizados", _
                                             Me.ToString(), "TerminoSubmitChanges", Application.Current.ToString(), Program.Maquina, ex)
        End Try

        ErrorForma = String.Empty
    End Sub

    ''' <history>
    ''' ID caso de prueba:  Id_15
    ''' Descripción:        Desarrollo PreEspecies. Se agregó la lógica cuando la especie a modificar es una preespecie.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              9 de Junio/2014
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 9 de Junio/2014
    ''' </history>
    ''' <history>
    ''' ID caso de prueba:  CP0012, CP0015
    ''' Descripción:        Se agrega instrucción para que asigne información en las propiedades IDEspecie y Nemotecnico de la lista ListaEspeciesNemotecnicos cuando 
    '''                     la especie es una PreEspecie. Lo anterior porque estaba generando la validación "Debe ingresar un nemotécnico" al actualizar una PreEspecie.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              5 de Marzo/2015
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 5 de Marzo/2015
    ''' </history>
    Public Overrides Sub EditarRegistro()
        If Not IsNothing(_EspecieSelected) Then
            'SLB - Si la especie se encuentra inactiva no se puede modificar.
            IsinFungible = True
            If EspecieSelected.EsAccion Then
                IsinFungibleBusquedad = False
            Else
                IsinFungibleBusquedad = True
            End If

            If Not EspecieSelected.Activo Then
                MyBase.RetornarValorEdicionNavegacion()
                A2Utilidades.Mensajes.mostrarMensaje("La especie está inactiva, no se puede modificar", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            EspecieSelected.Usuario = Program.Usuario
            EspecieSelected.Actualizacion = Now.Date.ToString

            If EspecieSelected.strPrecreada = "SI" Then
                EspecieAnteriorPreEspecie = EspecieSelected
                NuevoRegistro()

                EspecieSelected.IDComisionista = EspecieAnteriorPreEspecie.IDComisionista
                EspecieSelected.IDSucComisionista = EspecieAnteriorPreEspecie.IDSucComisionista
                EspecieSelected.EsAccion = EspecieAnteriorPreEspecie.EsAccion
                EspecieSelected.Id = EspecieAnteriorPreEspecie.Id
                EspecieSelected.IdEmisor = EspecieAnteriorPreEspecie.IdEmisor
                EmisorClaseSelected.Emisor = EspecieAnteriorPreEspecie.strEmisor


                EspecieSelected.IDClase = EspecieAnteriorPreEspecie.IDClase
                ClaseClaseSelected.Clase = EspecieAnteriorPreEspecie.strClase

                EspecieSelected.strPrecreada = EspecieAnteriorPreEspecie.strPrecreada

                _mlogExisteNemotblEspecies = False
                If ListaEspeciesNemotecnicos.Count > 0 Then
                    ListaEspeciesNemotecnicos.First.IDEspecie = EspecieSelected.Id
                    ListaEspeciesNemotecnicos.First.Nemotecnico = EspecieSelected.Id
                End If
            End If

            ' EOMC -- Setea propiedad isenabled dependiendo de si es acción -- 08/08/2013 
            If _EspecieSelected.EsAccion Then
                Habilitar = False
                HabilitarIndicador = False
                Inhabilitar = True
                InhabilitarBursatilidad = False
                'HabilitarTipo = True
                'HabilitarCamposTituloParticipativo = False 'JAEZ 20161128
                ' HabilitarCamposTabCFTituloParticipativo = True
            Else
                Habilitar = True
                Inhabilitar = False
                InhabilitarBursatilidad = True
            End If

            HabilitarPropiedadTabCFTituloParticipativo()

            Editando = True
            If (EspecieSelected.TipoEspecie = "N") Then
                HabilitarTituloMaterializado = True
            Else
                HabilitarTituloMaterializado = False
            End If
            EditandoPrecio = False
            habilitarLinkISIN = Editando
            HabilitarNemotecnico = False
            HabilitarCodigo = False
            HabilitarDuplicarRegistro = False
            AltoBotonDuplicar = 23
            If (EspecieSelected.TipoTasaFija = "F" Or EspecieSelected.TipoTasaFija = Nothing) Then
                HabilitarIndicador = False
            Else
                HabilitarIndicador = True
            End If
            InhabilitarDetalles = False
            'InhabilitarDetallePrecio = False
            If Not IsNothing(ListaEspeciesISIN) Then
                If ListaEspeciesISIN.Count = 0 Then
                    HabilitarCodigo2 = True
                Else
                    HabilitarCodigo2 = False
                End If

            End If
            If EspecieSelected.EsAccion Then
                HabilitarCodigo2 = False
            End If

            If Not ListaComboCurvaCC Is Nothing And EspecieSelected.CurvaEspecie = 0 Then
                EspecieSelected.CurvaEspecie = 1
            End If

            If Not IsNothing(CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))) _
                And IsNothing(EspecieSelected.LeyCirculacion) Or EspecieSelected.LeyCirculacion = "" Then
                If CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo))).ContainsKey("CIRCULACION") Then
                    If CType(Application.Current.Resources(Program.NOMBRE_LISTA_COMBOS), Dictionary(Of String, List(Of OYDUtilidades.ItemCombo)))("CIRCULACION").Where(Function(I) I.ID = "N").Count > 0 Then
                        EspecieSelected.LeyCirculacion = "N"
                    End If
                End If
            End If
            logDuplicar = False
            ConsultarListaSubgrupo(True)

        End If
        'If Not IsNothing(EspecieSelected) Then
        '    If Not IsNothing(EspecieSelected.Vencimiento) And Not IsNothing(EspecieSelected.Emision) Then
        '        DiferenciaPlazo()
        '    End If
        'End If

    End Sub

    Public Overrides Sub CancelarEditarRegistro()
        Try
            ErrorForma = ""
            If Not IsNothing(_EspecieSelected) Then
                dcProxy.RejectChanges()
                Editando = False
                HabilitarTituloMaterializado = False
                habilitarLinkISIN = Editando
                Habilitar = False
                HabilitarIndicador = False
                Inhabilitar = False
                InhabilitarBursatilidad = False
                logDuplicar = False
                'Santiago Vergara - Marzo 12/2014 - Se deshabilita el códifgo si se cancela la edición
                HabilitarCodigo = False
                HabilitarNemotecnico = False
                'HabilitarCamposTituloParticipativo = False
                HabilitarCamposTabCFTituloParticipativo = False
                'HabilitarTipo = False
                HabilitarDuplicarRegistro = True
                AltoBotonDuplicar = 19
                If IsNothing(Me.claseinversionSelected) Then
                    Me.claseinversionSelected = New claseinversion() With {.strDescripcion = strdescripcionclase}
                Else
                    Me.claseinversionSelected.strDescripcion = strdescripcionclase
                End If

                Dim intSubGrupo As Integer = 0

                If Not IsNothing(EspecieSelected.IDSubGrupo) Then
                    intSubGrupo = EspecieSelected.IDSubGrupo
                End If

                ConsultarListaSubgrupo()
                If _EspecieSelected.EntityState = EntityState.Detached Then
                    EspecieSelected = EspecieAnterior
                End If

                If intSubGrupo <> 0 Then
                    If Not IsNothing(EspecieSelected) Then
                        EspecieSelected.IDSubGrupo = Nothing
                        EspecieSelected.IDSubGrupo = intSubGrupo
                    End If

                End If

                buscarItem("claseinversion")

            End If

            TextoFiltroISIN = String.Empty
            EmisionFiltroISIN = Nothing
            VencimientoFiltroISIN = Nothing
            IsinFungible = False
            IsinFungibleBusquedad = False
            dcProxy.EspeciesISINFungibles.Clear()

            If Not IsNothing(EspecieSelected) Then
                If Not String.IsNullOrEmpty(EspecieSelected.Id) Then
                    dcProxy.Load(dcProxy.EspeciesISINFungibleConsultarQuery(EspecieSelected.Id, String.Empty, EspecieSelected.TipoTasaFija, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesISINFungible, Nothing)
                    dcProxy.ArbitrajeADRs.Clear()
                    dcProxy.Load(dcProxy.EspecieArbitrajeADR_ConsultarQuery(EspecieSelected.Id, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerArbitrajeADR, Nothing)
                End If
            End If

            InhabilitarDetalles = True
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al cancelar la edición del registro", _
                     Me.ToString(), "CancelarEditarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <history>
    ''' ID caso de prueba:  Id_18
    ''' Descripción:        Desarrollo PreEspecies. Prueba realizada para borrar un registro.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              9 de Junio/2014
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 9 de Junio/2014
    ''' </history>
    Public Overrides Sub BorrarRegistro()
        Try
            If Not IsNothing(EspecieSelected) Then
                Dim strMsg As String = String.Empty

                If EspecieSelected.strPrecreada = "SI" Then
                    A2Utilidades.Mensajes.mostrarMensaje("Una preespecie solamente puede ser eliminada desde el maestro de preespecies.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Exit Sub
                Else
                    If _EspecieSelected.Activo Then
                        strMsg = "Realmente desea Inactivar la especie"
                    Else
                        strMsg = "Realmente desea Activar la especie"
                    End If
                End If

                A2Utilidades.Mensajes.mostrarMensajePregunta(strMsg, Program.TituloSistema, String.Empty, AddressOf TerminoPreguntaInactivarEspecie)
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registro", _
             Me.ToString(), "BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <history>
    ''' Descripción:  Se agrega la instrucción "Program.Usuario" para llevarlo a la base de datos.
    ''' Responsable:  Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:        21 de Agosto/2015
    ''' </history>
    ''' <remarks></remarks>
    Private Sub TerminoPreguntaInactivarEspecie(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)

            If objResultado.DialogResult Then
                If Not IsNothing(_EspecieSelected) Then
                    IsBusy = True
                    dcProxy.InactivarEspecies(EspecieSelected.Id, EspecieSelected.Activo, Program.Usuario, Program.HashConexion, AddressOf TerminoInactivarEspecie, "inactivar")
                    'dcProxy.Especis.Remove(_EspecieSelected)
                    'IsBusy = True
                    'dcProxy.SubmitChanges(AddressOf TerminoSubmitChanges, Nothing)
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al tratar de Inactivar la Especie", _
             Me.ToString(), "TerminoPreguntaInactivarEspecie", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <history>
    ''' Descripción:  Se realiza el llamado EspeciesFiltrarQuery para consultar el estado, usuario y fecha de actualización.
    ''' Responsable:  Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:        21 de Agosto/2015
    ''' </history>
    ''' <remarks></remarks>
    Private Sub TerminoInactivarEspecie(ByVal lo As InvokeOperation(Of Boolean))
        If Not lo.HasError Then

            If Not IsNothing(dcProxy.Especis) Then
                dcProxy.Especis.Clear()
            End If

            dcProxy.Load(dcProxy.EspeciesFiltrarQuery("", Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspecies, "TerminoGuardar")

        Else

            If lo.Error.ToString.Contains("No se puede Activar") Then
                Dim intPosIni As Integer = lo.Error.ToString.IndexOf("No se puede Activar")
                Dim intPosFin As Integer = lo.Error.ToString.IndexOf("|")
                Dim Mensaje = lo.Error.ToString.Substring(intPosIni, intPosFin - intPosIni)
                A2Utilidades.Mensajes.mostrarMensaje(Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                IsinFungible = True
                IsinFungibleBusquedad = True
                lo.MarkErrorAsHandled()
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al anular la Especie", _
                                             Me.ToString(), "TerminoInactivarEspecie", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If

        End If
        IsBusy = False
    End Sub


    'Public Overrides Sub CambiarAForma()
    '    Try
    '        Select Case EspecieSelected.TipoTasaFija
    '            Case String.Empty
    '                Visible = Visibility.Collapsed
    '                VisibleV = Visibility.Collapsed
    '                VisibleF = Visibility.Collapsed
    '            Case "V"
    '                VisibleV = Visibility.Collapsed
    '                Visible = Visibility.Visible
    '                VisibleF = Visibility.Visible
    '            Case "F"
    '                VisibleF = Visibility.Collapsed
    '                VisibleV = Visibility.Visible
    '                Visible = Visibility.Visible
    '        End Select
    '        MyBase.CambiarAForma()
    '    Catch ex As Exception
    '        IsBusy = False
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Cambiar a Forma", _
    '         Me.ToString(), "CambiarAForma", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub

    Public Sub seleccionarCampoTab(ByVal pstrNombreCampo As String)
        If DicCamposTab.ContainsKey(pstrNombreCampo) Then
            Dim miTab = DicCamposTab(pstrNombreCampo)
            TabSeleccionadaFinanciero = miTab
        End If
    End Sub

    Public Sub llenarDiccionario()
        Try
            DicCamposTab.Add("Nombre", 1)
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al llenar Diccionario", _
          Me.ToString(), "llenarDiccionario", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Sub ValidarEspecieLanzarISIN(obj As Object)
        Dim isnf As EspeciesISINFungible = CType(obj, EspeciesISINFungible)

        If Not IsNothing(EspecieSelected) Then
            If Not EspecieSelected.EsAccion And EspecieSelected.Amortiza = Nothing Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe definir si la especie amortiza o no.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                LanzarMaestroISIN(isnf, False)
            End If
        End If

    End Sub

    Public Sub FiltrarListaISIN()
        Try
            'TextoFiltroISIN = UCase(TextoFiltroISIN)

            If TextoFiltroISIN = "" And IsNothing(EmisionFiltroISIN) And IsNothing(VencimientoFiltroISIN) Then
                A2Utilidades.Mensajes.mostrarMensaje("Seleccione algun campo para realizar el filtro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                dcProxy.EspeciesISINFungibles.Clear()
                If Not String.IsNullOrEmpty(EspecieSelected.Id) Then
                    dcProxy.Load(dcProxy.EspeciesISINFungibleConsultarQuery(EspecieSelected.Id, String.Empty, EspecieSelected.TipoTasaFija, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesISINFungible, Nothing)
                End If
                Exit Sub
            End If

            Dim EsFechaVencimiento As Boolean = IsDate(VencimientoFiltroISIN)
            Dim EsFechaEmision As Boolean = IsDate(EmisionFiltroISIN)
            Dim FechaPorDefecto As DateTime 'Se crea esta variable para poder hacer la comparacion con la fecha de emision y vencimiento si en la lista estos vienen nothing
            Dim SinResultado As String = "" 'Se crea esta variable para que en caso de algunos ISIN de acciones que no tienen fecha de vencimiento ni de emisión 
            'al menos cargue el grid en blanco y no quede como si no hubiera hecho nada (básicamente así funciona el filtro)

            If Not IsNothing(ListaEspeciesISINFungibleSinFiltro) Then
                If EsFechaVencimiento And EsFechaEmision And (TextoFiltroISIN <> Nothing) Then
                    ListaEspeciesISINFungible = New ObservableCollection(Of EspeciesISINFungible)(ListaEspeciesISINFungibleSinFiltro.Where(Function(i) (IIf(IsNothing(i.Fecha_Emision), FechaPorDefecto, i.Fecha_Emision) = EmisionFiltroISIN) And ((IIf(IsNothing(i.Fecha_Vencimiento), FechaPorDefecto, i.Fecha_Vencimiento)) = VencimientoFiltroISIN) And (i.ISIN.ToUpper.Contains(TextoFiltroISIN.ToUpper))))
                ElseIf (TextoFiltroISIN <> Nothing And EsFechaVencimiento) Then
                    ListaEspeciesISINFungible = New ObservableCollection(Of EspeciesISINFungible)(ListaEspeciesISINFungibleSinFiltro.Where(Function(i) ((IIf(IsNothing(i.Fecha_Vencimiento), FechaPorDefecto, i.Fecha_Vencimiento)) = VencimientoFiltroISIN) And (i.ISIN.ToUpper.Contains(TextoFiltroISIN.ToUpper))))
                ElseIf (TextoFiltroISIN <> Nothing And EsFechaEmision) Then
                    ListaEspeciesISINFungible = New ObservableCollection(Of EspeciesISINFungible)(ListaEspeciesISINFungibleSinFiltro.Where(Function(i) (IIf(IsNothing(i.Fecha_Emision), FechaPorDefecto, i.Fecha_Emision) = EmisionFiltroISIN) And (i.ISIN.ToUpper.Contains(TextoFiltroISIN.ToUpper))))
                ElseIf (EsFechaEmision And EsFechaVencimiento) Then
                    ListaEspeciesISINFungible = New ObservableCollection(Of EspeciesISINFungible)(ListaEspeciesISINFungibleSinFiltro.Where(Function(i) (IIf(IsNothing(i.Fecha_Emision), FechaPorDefecto, i.Fecha_Emision) = EmisionFiltroISIN) And ((IIf(IsNothing(i.Fecha_Vencimiento), FechaPorDefecto, i.Fecha_Vencimiento)) = VencimientoFiltroISIN)))
                ElseIf (TextoFiltroISIN <> Nothing) Then
                    ListaEspeciesISINFungible = New ObservableCollection(Of EspeciesISINFungible)(ListaEspeciesISINFungibleSinFiltro.Where(Function(i) i.ISIN.ToUpper.Contains(TextoFiltroISIN.ToUpper)))
                ElseIf EsFechaEmision And ListaEspeciesISINFungibleSinFiltro.Where(Function(i) i.Fecha_Emision.HasValue).Count >= 1 Then
                    ListaEspeciesISINFungible = New ObservableCollection(Of EspeciesISINFungible)(ListaEspeciesISINFungibleSinFiltro.Where(Function(i) (IIf(IsNothing(i.Fecha_Emision), FechaPorDefecto, i.Fecha_Emision) = EmisionFiltroISIN)))
                ElseIf EsFechaVencimiento And ListaEspeciesISINFungibleSinFiltro.Where(Function(i) i.Fecha_Vencimiento.HasValue).Count >= 1 Then
                    ListaEspeciesISINFungible = New ObservableCollection(Of EspeciesISINFungible)(ListaEspeciesISINFungibleSinFiltro.Where(Function(i) ((IIf(IsNothing(i.Fecha_Vencimiento), FechaPorDefecto, i.Fecha_Vencimiento)) = Convert.ToDateTime(VencimientoFiltroISIN).Date)))
                Else
                    ListaEspeciesISINFungible = New ObservableCollection(Of EspeciesISINFungible)(ListaEspeciesISINFungibleSinFiltro.Where(Function(i) i.ISIN = SinResultado))
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar el filtro por ISIN", _
                                                        Me.ToString(), "FiltrarListaISIN", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub ConsultarListaSubgrupo(Optional ByVal VolverALlevarValorSubGrupo As Boolean = False)
        Try
            Dim objlista As New List(Of CFMaestros.Clasificacion)
            Dim idSubGrupo As Integer = 0

            If Editando = False Then
                If Not IsNothing(listaclasificacion) Then
                    For Each li In listaclasificacion
                        objlista.Add(li)
                    Next
                End If

                listasubGrupo = objlista
            Else
                If VolverALlevarValorSubGrupo Then
                    If Not IsNothing(_EspecieSelected) Then
                        If Not IsNothing(_EspecieSelected.IDSubGrupo) Then
                            idSubGrupo = _EspecieSelected.IDSubGrupo
                        End If
                    End If
                End If

                If Not IsNothing(listaclasificacion) Then
                    For Each li In listaclasificacion
                        If li.EsGrupo = False And li.IDPerteneceA = IIf(IsNothing(_EspecieSelected.IDGrupo), 0, _EspecieSelected.IDGrupo) And li.AplicaA.Equals("E") Then
                            objlista.Add(li)
                        End If
                    Next
                End If

                listasubGrupo = objlista

                If idSubGrupo <> 0 Then
                    _EspecieSelected.IDSubGrupo = Nothing
                    _EspecieSelected.IDSubGrupo = idSubGrupo
                    MyBase.CambioItem("EspecieSelected.IDSubGrupo")
                    MyBase.CambioItem("EspecieSelected")
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al ConsultarListaSubgrupo", _
                                                         Me.ToString(), "ConsultarListaSubgrupo", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Sub HabilitarPropiedadTabCFTituloParticipativo()
        Try
            If Not IsNothing(EspecieSelected) Then
                If EspecieSelected.TituloParticipativo Then
                    'HabilitarTipo = False
                    'HabilitarCamposTituloParticipativo = False
                    HabilitarCamposTabCFTituloParticipativo = False
                Else
                    'HabilitarTipo = True
                    'HabilitarCamposTituloParticipativo = True
                    HabilitarCamposTabCFTituloParticipativo = True
                    If Not IsNothing(ListaEspeciesISINFungible) Then
                        If EspecieSelected.EsAccion Then
                            For Each ISIN In ListaEspeciesISINFungible
                                ISIN.Fecha_Emision = Nothing
                                ISIN.Fecha_Vencimiento = Nothing
                                ISIN.ConEspecie = IIf(EspecieSelected.IDEspecies > 0, True, False)
                            Next
                        End If
                    End If
                End If

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al tratar de habilitar los campos que no corresponden para título participativo.",
                                                         Me.ToString(), "HabilidarPropiedadTabCFTituloParticipativo", Program.TituloSistema,
                                                         Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private _ListaConceptoRetencion As EntitySet(Of ConceptoRetencion)
    Public Property ListaConceptoRetencion() As EntitySet(Of ConceptoRetencion)
        Get
            Return _ListaConceptoRetencion
        End Get
        Set(ByVal value As EntitySet(Of ConceptoRetencion))
            _ListaConceptoRetencion = value

            MyBase.CambioItem("ListaConceptoRetencion")
        End Set
    End Property

    Private _ListaConceptoRetencionGravados As List(Of ConceptoRetencion)
    Public Property ListaConceptoRetencionGravados() As List(Of ConceptoRetencion)
        Get
            Return _ListaConceptoRetencionGravados
        End Get
        Set(ByVal value As List(Of ConceptoRetencion))
            _ListaConceptoRetencionGravados = value

            MyBase.CambioItem("ListaConceptoRetencionGravados")
        End Set
    End Property

    Private _ListaConceptoRetencionNoGravados As List(Of ConceptoRetencion)
    Public Property ListaConceptoRetencionNoGravados() As List(Of ConceptoRetencion)
        Get
            Return _ListaConceptoRetencionNoGravados
        End Get
        Set(ByVal value As List(Of ConceptoRetencion))
            _ListaConceptoRetencionNoGravados = value

            MyBase.CambioItem("ListaConceptoRetencionNoGravados")
        End Set
    End Property

    Private _intIDConceptoRetencion As List(Of ConceptoRetencion)
    Public Property intIDConceptoRetencion() As List(Of ConceptoRetencion)
        Get
            Return _intIDConceptoRetencion
        End Get
        Set(ByVal value As List(Of ConceptoRetencion))
            _intIDConceptoRetencion = value

            MyBase.CambioItem("intIDConceptoRetencion")
        End Set
    End Property

#End Region

#Region "PropertyChanged"
    ''' <history>
    ''' Descripción:        Se eliminó la instrucción "EspecieSelected.ValoraDescuentoFactoring = False" porque ya no se utilizan en la pantalla de Especies. Cambio autorizado por Jorge Arango.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              29 de Octubre/2014
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 29 de Octubre/2014
    ''' </history>
    ''' <history>
    ''' Descripción:        Se limpian los datos "Bursatilidad","ClaseContableTitulo","claseinversionSelected" cuando cambia entre accion y renta fija para evitar inconsistencias en los datos.
    ''' Responsable:        Germán Arbey González Osorio (Alcuadrado S.A.)
    ''' Fecha:              Diciembre 09/2014
    ''' Pruebas CB:         Germán Arbey González Osorio (Alcuadrado S.A.) - Diciembre 09/2014
    ''' </history>
    Private Sub _EspecieSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EspecieSelected.PropertyChanged
        Try
            Select Case e.PropertyName
                Case "EsAccion"

                    ' EOMC -- Se cambia case por if (está verificando un boolean) y se modifican valores para propiedades que inhabilitan controles dependiende de 'EsAccion' -- 08/08/2013
                    If EspecieSelected.EsAccion Then
                        IsinFungibleBusquedad = False
                        EspecieSelected.strAccion = "Acción"
                        VisualizarAcciones = Visibility.Visible
                        VisualizarRentaFija = Visibility.Collapsed

                        'Descripción:   Se agrega el campo "Democratización" (Aplica solo para acciones).
                        'Responsable:   Jorge Peña (Alcuadrado S.A.)
                        'Fecha:         5 de Mayo/2016
                        If VISUALIZAR_CAMPO_DEMOCRATIZACION = "SI" Then
                            VisualizarDemocratizacion = Visibility.Visible
                        Else
                            VisualizarDemocratizacion = Visibility.Collapsed
                        End If

                        EspecieSelected.DeclaraDividendos = True
                        HabilitarDeclaraDividendos = True
                        Inhabilitar = True
                        InhabilitarBursatilidad = False
                        Habilitar = False
                        'HabilitarTipo = True
                        HabilitarIndicador = False
                        Clase = "1"
                        Me.ClaseClaseSelected.IdClase = Nothing ' _ClaseNuevo.IdClase
                        Me.ClaseClaseSelected.Clase = String.Empty '  _ClaseNuevo.Clase
                        EspecieSelected.IDClase = Nothing ' CInt(_ClaseNuevo.IdClase)
                        _EspecieSelected.Liquidez = False
                        HabilitarTabs = True
                        HabilitarTabPrecio = True
                        'EspecieSelected.TituloParticipativo = False
                        ' eomc -- 12/11/2013 -- Inicio
                        ' Se borran datos de renta fija para evitar guardar basura en DB
                        EspecieSelected.TipoTasaFija = Nothing
                        EspecieSelected.ClaseInversion = Nothing
                        claseinversionSelected = Nothing
                        EspecieSelected.ClaseContableTitulo = Nothing
                        EspecieSelected.RefTasaVble = Nothing
                        EspecieSelected.Corresponde = Nothing
                        EspecieSelected.BaseCalculoInteres = Nothing
                        EspecieSelected.Amortiza = Nothing
                        EspecieSelected.IDTarifa = Nothing
                        EspecieSelected.TasaInicial = 0
                        EspecieSelected.TipoTasaFija = Nothing
                        EspecieSelected.TasaNominal = 0
                        EspecieSelected.TipoEspecie = String.Empty
                        ' eomc -- 12/11/2013 -- Fin
                        Visible = Visibility.Collapsed
                        VisibleV = Visibility.Collapsed
                        VisibleF = Visibility.Collapsed
                        EspecieSelected.ConceptoRetencion = -1
                        HabilitarCamposTabCFTituloParticipativo = True
                        'HabilitarCamposTituloParticipativo = False  'JAEZ 20161128
                    Else
                        EspecieSelected.strAccion = "Renta Fija"
                        VisualizarRentaFija = Visibility.Visible
                        VisualizarAcciones = Visibility.Collapsed

                        'Descripción:   Se agrega el campo "Democratización" (Aplica solo para acciones).
                        'Responsable:   Jorge Peña (Alcuadrado S.A.)
                        'Fecha:         5 de Mayo/2016
                        VisualizarDemocratizacion = Visibility.Collapsed

                        Habilitar = True
                        'HabilitarCamposTituloParticipativo = True 'JAEZ 20161128
                        If EspecieSelected.TipoTasaFija = Nothing Then
                            HabilitarIndicador = False
                            EspecieSelected.Indicador = Nothing
                        Else
                            HabilitarIndicador = True
                        End If
                        Inhabilitar = False
                        InhabilitarBursatilidad = True
                        Clase = "0"
                        HabilitarDeclaraDividendos = False
                        EspecieSelected.DeclaraDividendos = False
                        Me.ClaseClaseSelected.IdClase = String.Empty
                        Me.ClaseClaseSelected.Clase = String.Empty
                        EspecieSelected.IDClase = 0
                        _EspecieSelected.Liquidez = Nothing
                        HabilitarTabs = False
                        HabilitarTabPrecio = False
                        IsinFungibleBusquedad = True

                        EspecieSelected.Bursatilidad = ""
                        EspecieSelected.ClaseContableTitulo = Nothing
                        If Not claseinversionSelected Is Nothing Then
                            claseinversionSelected.strcodigo = ""
                            claseinversionSelected.strDescripcion = ""
                        End If

                        Select Case EspecieSelected.TipoTasaFija
                            Case String.Empty
                                Visible = Visibility.Collapsed
                                VisibleV = Visibility.Collapsed
                                VisibleF = Visibility.Collapsed
                            Case "F"
                                VisibleF = Visibility.Visible
                                VisibleV = Visibility.Collapsed
                                Visible = Visibility.Visible
                            Case "V"
                                VisibleF = Visibility.Collapsed
                                VisibleV = Visibility.Visible
                                Visible = Visibility.Visible
                        End Select
                    End If

                Case "DeclaraDividendos"
                    Select Case EspecieSelected.DeclaraDividendos
                        Case True
                            HabilitarDeclaraDividendos = True
                        Case Else
                            HabilitarDeclaraDividendos = False
                    End Select
                    'Case "Vencimiento"
                    '    If IsDate(EspecieSelected.Vencimiento) And IsDate(EspecieSelected.Emision) Then
                    '        EspecieSelected.Plazo = CStr((DateDiff("d", EspecieSelected.Emision, EspecieSelected.Vencimiento))) & " dias"
                    '    Else
                    '        EspecieSelected.Plazo = ""
                    '    End If
                Case "Id"
                    If EspecieSelected.Id <> String.Empty Then
                        IsBusy = True
                        dcProxy.ExisteNemoIngresado(EspecieSelected.Id, -1, Program.Usuario, Program.HashConexion, AddressOf TerminoExisteNemoIngresado, "ExisteNemotblEspecies")
                    End If
                    'For Each led In ListaEspecies
                    '    If Not IsNothing(EspecieSelected.Id) Then
                    '        If led.Id.ToUpper = EspecieSelected.Id.ToUpper Then
                    '            A2Utilidades.Mensajes.mostrarMensaje("La especie con el código " + EspecieSelected.Id + " ya existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    '            Exit Sub
                    '        End If
                    '    End If
                    'Next

                    ' EOMC -- Se modifican valores para propiedades que ocultan filas del grid de ISIN fungibles -- 08/08/2013
                Case "TipoTasaFija"
                    If EspecieSelected.EsAccion Then
                        Visible = Visibility.Collapsed
                        VisibleV = Visibility.Collapsed
                        VisibleF = Visibility.Collapsed
                    Else
                        Select Case EspecieSelected.TipoTasaFija
                            Case String.Empty
                                Visible = Visibility.Collapsed
                                VisibleV = Visibility.Collapsed
                                VisibleF = Visibility.Collapsed
                            Case "F"
                                VisibleF = Visibility.Visible
                                VisibleV = Visibility.Collapsed
                                Visible = Visibility.Visible
                                HabilitarIndicador = False
                                EspecieSelected.Indicador = Nothing

                            Case "V"
                                VisibleF = Visibility.Collapsed
                                VisibleV = Visibility.Visible
                                Visible = Visibility.Visible
                                HabilitarIndicador = True
                        End Select
                    End If
                Case "IDGrupo"
                    If Not IsNothing(EspecieSelected.IDGrupo) Then
                        ConsultarListaSubgrupo()
                    End If
                Case "TipoEspecie"
                    If EspecieSelected.TipoEspecie = "N" Then
                        HabilitarTituloMaterializado = True
                        HabilitarTabArbitrajeADR = "Hidden"
                        If TabSeleccionado = EspecieTabs.ArbitrajeADR Then TabSeleccionado = EspecieTabs.CF
                    ElseIf EspecieSelected.TipoEspecie = "A" Then
                        HabilitarTabArbitrajeADR = "Visible"
                        TabSeleccionado = EspecieTabs.ArbitrajeADR
                    Else
                        HabilitarTituloMaterializado = False
                        EspecieSelected.TituloMaterializado = False
                        HabilitarTabArbitrajeADR = "Hidden"
                        If TabSeleccionado = EspecieTabs.ArbitrajeADR Then TabSeleccionado = EspecieTabs.CF
                    End If
                Case "TituloParticipativo"
                    HabilitarPropiedadTabCFTituloParticipativo()
            End Select
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Cambiar de Propiedad", _
             Me.ToString(), "EspecieSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub _EspeciesNemotecnicosSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EspeciesNemotecnicosSelected.PropertyChanged
        Try
            If e.PropertyName.Equals("Nemotecnico") Or e.PropertyName.Equals("IDBolsa") Then
                'ExisteNemo(EspeciesNemotecnicosSelected.Nemotecnico, EspeciesNemotecnicosSelected.IDBolsa)
                If EspeciesNemotecnicosSelected.Nemotecnico <> String.Empty Then
                    IsBusy = True
                    dcProxy.ExisteNemoIngresado(EspeciesNemotecnicosSelected.Nemotecnico, EspeciesNemotecnicosSelected.IDBolsa, Program.Usuario, Program.HashConexion, AddressOf TerminoExisteNemoIngresado, "ExisteNemotblEspeciesBolsa")
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Cambiar de Propiedad", _
             Me.ToString(), "EspeciesNemotecnicosSelected_PropertyChanged", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#Region "Cambios"

#Region "Constantes"

    Private Enum EspecieTabs As Byte
        Totales
        Dividendos
        Precios
        Nemotecnicos
        IsinFungible
        CF
        ArbitrajeADR
    End Enum

#End Region

    'Public Sub ExisteNemo(ByVal strNemotecnico As String, ByVal lngIDBolsa As Integer)
    '    Try
    '        If strNemotecnico <> String.Empty Then
    '            IsBusy = True
    '            dcProxy.ExisteNemoIngresado(strNemotecnico, lngIDBolsa, AddressOf TerminoExisteNemoIngresado, "ExisteNemotblEspeciesBolsa")
    '        End If
    '    Catch ex As Exception
    '        IsBusy = False
    '        A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al Cambiar de Propiedad", _
    '         Me.ToString(), "ExisteNemo", Application.Current.ToString(), Program.Maquina, ex)
    '    End Try
    'End Sub

    ''' <history>
    ''' Descripción:        Desarrollo PreEspecies. Se agregó lógica para indicar que la especie no sea una preespecie.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              9 de Junio/2014
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 9 de Junio/2014
    ''' </history>
    Private Sub TerminoExisteNemoIngresado(ByVal lo As InvokeOperation(Of String))
        If Not lo.HasError Then
            If Not lo.Value.ToString = "False" Then
                Select Case lo.UserState.ToString
                    Case "ExisteNemotblEspecies"
                        If EspecieSelected.strPrecreada <> "SI" Then
                            A2Utilidades.Mensajes.mostrarMensaje("La especie con el código '" + EspecieSelected.Id + "' ya existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            _mlogExisteNemotblEspecies = True
                            'EspecieSelected.Id = String.Empty
                        End If
                    Case "ExisteNemotblEspeciesBolsa"
                        If lo.Value <> EspecieSelected.Id Then
                            A2Utilidades.Mensajes.mostrarMensaje("El nemotécnico '" + EspeciesNemotecnicosSelected.Nemotecnico + "' ya existe para otra Especie en la Bolsa seleccionada", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            _mlogExisteNemotblEspeciesBolsa = True
                            TabSeleccionado = EspecieTabs.Nemotecnicos
                            'EspeciesNemotecnicosSelected.Nemotecnico = String.Empty
                        Else
                            _mlogExisteNemotblEspeciesBolsa = False
                        End If
                End Select
            Else
                Select Case lo.UserState.ToString
                    Case "ExisteNemotblEspecies"
                        _mlogExisteNemotblEspecies = False
                        If ListaEspeciesNemotecnicos.Count > 0 Then
                            ListaEspeciesNemotecnicos.First.IDEspecie = EspecieSelected.Id
                            ListaEspeciesNemotecnicos.First.Nemotecnico = EspecieSelected.Id
                        End If
                    Case "ExisteNemotblEspeciesBolsa"
                        _mlogExisteNemotblEspeciesBolsa = False
                End Select
            End If
            Messenger.Default.Send(Of Especi)(Nothing)
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la verificación de la Nemotécnico ingreado", _
                                             Me.ToString(), "TerminoExisteNemoIngresado", Application.Current.ToString(), Program.Maquina, lo.Error)
            lo.MarkErrorAsHandled()   '????
        End If
        IsBusy = False
    End Sub

#End Region

#Region "Tablas hijas"

#Region "EspeciesDividendos"

    '******************************************************** EspeciesDividendos 
    Private _ListaEspeciesDividendos As EntitySet(Of EspeciesDividendos)
    Public Property ListaEspeciesDividendos() As EntitySet(Of EspeciesDividendos)
        Get
            Return _ListaEspeciesDividendos
        End Get
        Set(ByVal value As EntitySet(Of EspeciesDividendos))
            _ListaEspeciesDividendos = value
            MyBase.CambioItem("ListaEspeciesDividendos")
            MyBase.CambioItem("ListaEspeciesDividendosPaged")
        End Set
    End Property

    Public ReadOnly Property ListaEspeciesDividendosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEspeciesDividendos) Then
                Dim view = New PagedCollectionView(_ListaEspeciesDividendos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
    Private _ListaEspeciesDividendosAnterior As List(Of EspeciesDividendos)
    Public Property ListaEspeciesDividendosAnterior() As List(Of EspeciesDividendos)
        Get
            Return _ListaEspeciesDividendosAnterior
        End Get
        Set(ByVal value As List(Of EspeciesDividendos))
            _ListaEspeciesDividendosAnterior = value
            MyBase.CambioItem("ListaEspeciesDividendosAnterior")
        End Set
    End Property

    Private WithEvents _EspeciesDividendosSelected As EspeciesDividendos
    Public Property EspeciesDividendosSelected() As EspeciesDividendos
        Get
            Return _EspeciesDividendosSelected
        End Get
        Set(ByVal value As EspeciesDividendos)

            If Not value Is Nothing Then
                _EspeciesDividendosSelected = value
                MyBase.CambioItem("EspeciesDividendosSelected")
            End If
        End Set
    End Property

    Private Sub _EspeciesDividendosSelected_PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Handles _EspeciesDividendosSelected.PropertyChanged
        Select Case e.PropertyName
            Case "intIDConceptoRetencionGravados"
                For Each li In ListaConceptoRetencion.Where(Function(i) i.intIDConceptoRetencion = EspeciesDividendosSelected.intIDConceptoRetencionGravados)
                    EspeciesDividendosSelected.dblPorcentajeRetencionGravados = li.dblPorcentajeRetencion
                Next
            Case "intIDConceptoRetencionNoGravados"
                For Each li In ListaConceptoRetencion.Where(Function(i) i.intIDConceptoRetencion = EspeciesDividendosSelected.intIDConceptoRetencionNoGravados)
                    EspeciesDividendosSelected.dblPorcentajeRetencionNoGravados = li.dblPorcentajeRetencion
                Next
        End Select

    End Sub

#End Region

    ''' <summary>
    ''' Este es el Metodo de Nuevo de todos los detalles de Especies 
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub NuevoRegistroDetalle()

        If Not IsNothing(EspecieSelected) Then

            EspecieSelected.Usuario = Program.Usuario
            EspecieSelected.Actualizacion = Now.Date.ToString

        End If
        logDuplicar = False
        Select Case NombreColeccionDetalle
            Case "cmEspeciesDividendos"
                Dim NewEspeciesDividendos As New EspeciesDividendos
                NewEspeciesDividendos.IDEspecie = EspecieSelected.Id
                NewEspeciesDividendos.InicioVigencia = EspeciesDividendoPorDefecto.InicioVigencia
                NewEspeciesDividendos.FinVigencia = EspeciesDividendoPorDefecto.FinVigencia
                NewEspeciesDividendos.Causacion = EspeciesDividendoPorDefecto.Causacion
                NewEspeciesDividendos.InicioPago = EspeciesDividendoPorDefecto.InicioPago
                NewEspeciesDividendos.FinPago = EspeciesDividendoPorDefecto.FinPago
                NewEspeciesDividendos.CantidadAcciones = EspeciesDividendoPorDefecto.CantidadAcciones
                NewEspeciesDividendos.CantidadPesos = EspeciesDividendoPorDefecto.CantidadPesos
                NewEspeciesDividendos.IDCtrlDividendo = EspeciesDividendoPorDefecto.IDCtrlDividendo
                If Not IsNothing(EspeciesDividendosSelected) Then
                    If ListaEspeciesDividendos.Count < 1 Then
                        NewEspeciesDividendos.IDDividendos = EspeciesDividendoPorDefecto.IDDividendos
                    Else
                        NewEspeciesDividendos.IDDividendos = ListaEspeciesDividendos.Last.IDDividendos + 1
                    End If
                End If
                NewEspeciesDividendos.Usuario = Program.Usuario

                ListaEspeciesDividendos.Add(NewEspeciesDividendos)
                EspeciesDividendosSelected = NewEspeciesDividendos
                InhabilitarDetalles = False
                MyBase.CambioItem("EspeciesDividendosSelected")
                MyBase.CambioItem("ListaEspeciesDividendos")
                MyBase.CambioItem("Editando")
            Case "cmEspeciesNemotecnicos"
                DetalleNemotecnico()

            Case "cmEspeciesPrecios"
                Dim NewEspeciesPrecios As New EspeciesPrecios
                NewEspeciesPrecios.IdEspecie = EspecieSelected.Id
                NewEspeciesPrecios.IdBolsa = EspeciesPreciosPorDefecto.IdBolsa
                NewEspeciesPrecios.Cierre = EspeciesPreciosPorDefecto.Cierre
                NewEspeciesPrecios.Precio = EspeciesPreciosPorDefecto.Precio
                If Not IsNothing(EspeciesPreciosSelected) Then
                    If ListaEspeciesPrecios.Count < 1 Then
                        NewEspeciesPrecios.IDPreciosEspecie = EspeciesPreciosPorDefecto.IDPreciosEspecie
                    Else
                        NewEspeciesPrecios.IDPreciosEspecie = ListaEspeciesPrecios.Last.IDPreciosEspecie + 1
                    End If
                End If
                NewEspeciesPrecios.Usuario = Program.Usuario
                ListaEspeciesPrecios.Add(NewEspeciesPrecios)
                EspeciesPreciosSelected = NewEspeciesPrecios
                InhabilitarDetalles = False
                MyBase.CambioItem("EspeciesPreciosSelected")
                MyBase.CambioItem("EspeciesPrecios")

            Case "cmEspeciesISIN"
                Dim NewEspeciesISIN As New EspeciesISIN
                NewEspeciesISIN.IDEspecie = EspecieSelected.Id
                NewEspeciesISIN.ISIN = EspeciesISINPorDefecto.ISIN
                NewEspeciesISIN.Descripcion = EspeciesISINPorDefecto.Descripcion
                If Not IsNothing(EspeciesISINSelected) Then
                    If ListaEspeciesISIN.Count < 1 Then
                        NewEspeciesISIN.IDEspeciesISIN = EspeciesISINPorDefecto.IDEspeciesISIN
                    Else
                        NewEspeciesISIN.IDEspeciesISIN = ListaEspeciesISIN.Last.IDEspeciesISIN + 1
                    End If
                End If
                NewEspeciesISIN.Usuario = Program.Usuario
                ListaEspeciesISIN.Add(NewEspeciesISIN)
                EspeciesISINSelected = NewEspeciesISIN
                InhabilitarDetalles = False
                MyBase.CambioItem("EspeciesISINSelected")
                MyBase.CambioItem("ListaEspeciesISIN")

                'Case "cmEspeciesISINFungible"

                ' validar la existencia de la especie
                'ActualizarRegistro() 'EOMC -- 07/08/2013 -- Inicio

                'lanzar maestro ISIN en detalle en modo de ingreso


                'If EspecieSelected.IDEspecies <> -1 Then 'EOMC -- 07/08/2013 -- Inicio

                'Dim NewEspeciesISINFungible As New EspeciesISINFungible
                'NewEspeciesISINFungible.IDEspecie = EspecieSelected.Id
                'NewEspeciesISINFungible.ISIN = EspeciesISINFungiblePorDefecto.ISIN
                'NewEspeciesISINFungible.IDFungible = EspeciesISINFungiblePorDefecto.IDFungible
                'NewEspeciesISINFungible.Emision = EspeciesISINFungiblePorDefecto.Emision
                'NewEspeciesISINFungible.IDConsecutivo = ListaEspeciesISINFungible.Count + 1
                'If Not IsNothing(EspeciesISINFungibleSelected) Then
                '    If ListaEspeciesISINFungible.Count < 1 Then
                '        NewEspeciesISINFungible.IDIsinFungible = EspeciesISINFungiblePorDefecto.IDIsinFungible
                '    Else
                '        NewEspeciesISINFungible.IDIsinFungible = ListaEspeciesISINFungible.Last.IDIsinFungible + 1
                '    End If
                'End If
                'NewEspeciesISINFungible.Usuario = Program.Usuario
                'ListaEspeciesISINFungible.Add(NewEspeciesISINFungible)
                'EspeciesISINFungibleSelected = NewEspeciesISINFungible
                'InhabilitarDetalles = False
                'MyBase.CambioItem("EspeciesISINFungibleSelected")
                'MyBase.CambioItem("ListaEspeciesISINFungible")


                ''EOMC -- 07/08/2013 -- Inicio
                'Dim NewEspeciesISIN As New EspeciesISIN
                'NewEspeciesISIN.IDEspecie = EspecieSelected.Id
                'NewEspeciesISIN.ISIN = EspeciesISINPorDefecto.ISIN
                'NewEspeciesISIN.Descripcion = EspeciesISINPorDefecto.Descripcion
                'If Not IsNothing(EspeciesISINSelected) Then
                '    If ListaEspeciesISIN.Count < 1 Then
                '        NewEspeciesISIN.IDEspeciesISIN = EspeciesISINPorDefecto.IDEspeciesISIN
                '    Else
                '        NewEspeciesISIN.IDEspeciesISIN = ListaEspeciesISIN.Last.IDEspeciesISIN + 1
                '    End If
                'End If
                'NewEspeciesISIN.Usuario = Program.Usuario
                ''ListaEspeciesISIN.Add(NewEspeciesISIN)
                'EspeciesISINSelected = NewEspeciesISIN
                'InhabilitarDetalles = False
                'MyBase.CambioItem("EspeciesISINSelected")
                'MyBase.CambioItem("ListaEspeciesISIN")

                'EOMC -- 07/08/2013 -- Fin

                'End If
        End Select
    End Sub

    Public Sub DetalleNemotecnico()
        Try
            Dim NewEspeciesNemotecnicos As New EspeciesNemotecnicos
            NewEspeciesNemotecnicos.IDEspecie = EspecieSelected.Id
            NewEspeciesNemotecnicos.IDBolsa = EspeciesNemotecnicosPorDefecto.IDBolsa
            NewEspeciesNemotecnicos.Nemotecnico = EspeciesNemotecnicosPorDefecto.Nemotecnico
            'NewEspeciesNemotecnicos.Nemotecnico = String.Empty
            If Not IsNothing(EspeciesNemotecnicosSelected) Then
                If ListaEspeciesNemotecnicos.Count < 1 Then
                    NewEspeciesNemotecnicos.IDEspeciesBolsa = EspeciesNemotecnicosPorDefecto.IDEspeciesBolsa
                Else
                    NewEspeciesNemotecnicos.IDEspeciesBolsa = ListaEspeciesNemotecnicos.Last.IDEspeciesBolsa + 1
                End If
            End If
            NewEspeciesNemotecnicos.Usuario = Program.Usuario
            If dcProxy.EspeciesNemotecnicos.Count = 0 Then
                ListaEspeciesNemotecnicos = dcProxy.EspeciesNemotecnicos
            End If
            ListaEspeciesNemotecnicos.Add(NewEspeciesNemotecnicos)
            EspeciesNemotecnicosSelected = NewEspeciesNemotecnicos
            InhabilitarDetalles = False
            MyBase.CambioItem("EspeciesNemotecnicosSelected")
            MyBase.CambioItem("ListaEspeciesNemotecnicos")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al crear el detalle del nemotécnico", _
             Me.ToString(), "DetalleNemotecnico", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Public Sub NuevoISINFungible()
        If String.IsNullOrEmpty(EspecieSelected.Id) Then
            A2Utilidades.Mensajes.mostrarMensaje("No se ha asignado una especie", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        ElseIf String.IsNullOrEmpty(EspecieSelected.TipoEspecie) Then
            A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el  tipo de especie.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Else
            LanzarMaestroISIN(EspecieSelected.Id, mlogNuevoRegistro)
        End If
    End Sub

    Public Sub DetalleIsinFungible_NuevoRegistro()
        Try
            If String.IsNullOrEmpty(EspecieSelected.Id) Then
                A2Utilidades.Mensajes.mostrarMensaje("No se ha asignado una especie", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            ElseIf Not EspecieSelected.EsAccion And EspecieSelected.Amortiza = Nothing Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe definir si la especie amortiza o no.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            ElseIf String.IsNullOrEmpty(EspecieSelected.TipoEspecie) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el  tipo de especie.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                LanzarMaestroISIN(EspecieSelected.Id, True)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al crear un nuevo registo", _
             Me.ToString(), "ConfirmarNuevo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub DetallleIsinFungible_BorrarRegistro()
        Try
            If Not IsNothing(ListaEspeciesISINFungible) Then
                If Not IsNothing(EspeciesISINFungibleSelected) Then
                    IsBusy = True
                    dcProxy.EliminarISINFungible(EspeciesISINFungibleSelected.IDIsinFungible, EspecieSelected.Id, Program.Usuario, Program.HashConexion, AddressOf terminoBorrarISINFungible, "borrar")
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un nuevo registo", _
             Me.ToString(), "ConfirmarBorrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub DetallleIsinFungible_DuplicarRegistro()
        Try
            If Not IsNothing(ListaEspeciesISINFungible) Then
                If ListaEspeciesISINFungible.Count > 0 Then
                    If String.IsNullOrEmpty(EspecieSelected.Id) Then
                        A2Utilidades.Mensajes.mostrarMensaje("No se ha asignado una especie", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    ElseIf Not EspecieSelected.EsAccion And EspecieSelected.Amortiza = Nothing Then
                        A2Utilidades.Mensajes.mostrarMensaje("Debe definir si la especie amortiza o no.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        Dim isnf As New EspeciesISINFungible()
                        isnf.ISIN = EspeciesISINFungibleSelected.ISIN
                        isnf.Descripcion = EspeciesISINFungibleSelected.Descripcion
                        isnf.IDFungible = EspeciesISINFungibleSelected.IDFungible
                        isnf.Emision = EspeciesISINFungibleSelected.Emision
                        isnf.Fecha_Emision = EspeciesISINFungibleSelected.Fecha_Emision
                        isnf.Fecha_Vencimiento = EspeciesISINFungibleSelected.Fecha_Vencimiento
                        isnf.Tasa_Facial = EspeciesISINFungibleSelected.Tasa_Facial
                        isnf.Modalidad = EspeciesISINFungibleSelected.Modalidad
                        isnf.intIndicador = EspeciesISINFungibleSelected.intIndicador
                        isnf.Indicador = EspeciesISINFungibleSelected.Indicador
                        isnf.Puntos_Indicador = EspeciesISINFungibleSelected.Puntos_Indicador
                        isnf.IDConsecutivo = EspeciesISINFungibleSelected.IDConsecutivo
                        isnf.IDConsecutivo = EspeciesISINFungibleSelected.IDConsecutivo
                        isnf.TasaBase = EspeciesISINFungibleSelected.TasaBase
                        'isnf.IDIsinFungible = EspeciesISINFungibleSelected.IDIsinFungible
                        isnf.Usuario = Program.Usuario
                        isnf.IDEspecie = EspeciesISINFungibleSelected.IDEspecie
                        isnf.Amortizada = EspeciesISINFungibleSelected.Amortizada
                        isnf.logPoseeRetencion = EspeciesISINFungibleSelected.logPoseeRetencion
                        isnf.dblPorcentajeRetencion = EspeciesISINFungibleSelected.dblPorcentajeRetencion
                        isnf.logFlujosIrregulares = EspeciesISINFungibleSelected.logFlujosIrregulares
                        isnf.logActivo = EspeciesISINFungibleSelected.logActivo
                        isnf.logSectorFinanciero = EspeciesISINFungibleSelected.logSectorFinanciero
                        isnf.ConEspecie = EspeciesISINFungibleSelected.ConEspecie
                        isnf.Amortizaciones = EspeciesISINFungibleSelected.Amortizaciones
                        isnf.logEsAccion = EspeciesISINFungibleSelected.logEsAccion
                        isnf.strTipoTasaFija = EspeciesISINFungibleSelected.strTipoTasaFija
                        isnf.intIDCalificacionInversion = EspeciesISINFungibleSelected.intIDCalificacionInversion
                        isnf.dblTasaEfectiva = EspeciesISINFungibleSelected.dblTasaEfectiva
                        isnf.Minimo = EspeciesISINFungibleSelected.Minimo
                        isnf.Multiplo = EspeciesISINFungibleSelected.Multiplo
                        isnf.Fecha_Irregular = EspeciesISINFungibleSelected.Fecha_Irregular
                        isnf.intIDConceptoRetencion = EspeciesISINFungibleSelected.intIDConceptoRetencion
                        isnf.logTituloCarteraColectiva = EspeciesISINFungibleSelected.logTituloCarteraColectiva
                        LanzarMaestroISIN(isnf, False)
                    End If
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un nuevo registo", _
             Me.ToString(), "ConfirmarBorrar", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Overrides Sub BorrarRegistroDetalle()
        Select Case NombreColeccionDetalle
            Case "cmEspeciesPrecios"
                If Not IsNothing(ListaEspeciesPrecios) Then
                    If Not IsNothing(EspeciesPreciosSelected) Then
                        Dim intRegistroPosicionar As Integer = Program.BuscarPosicionarItemLista(EspeciesPreciosSelected, ListaEspeciesPrecios.ToList)
                        ListaEspeciesPrecios.Remove(EspeciesPreciosSelected)
                        EspeciesPreciosSelected = ListaEspeciesPrecios.LastOrDefault
                        If ListaEspeciesPrecios.Count > 0 Then
                            Program.PosicionarItemLista(EspeciesPreciosSelected, ListaEspeciesPrecios.ToList, intRegistroPosicionar)
                        End If
                        MyBase.CambioItem("ListaEspeciesPreciosSelected")
                        MyBase.CambioItem("ListaEspeciesPrecios")
                    End If
                End If
            Case "cmEspeciesDividendos"
                If Not IsNothing(ListaEspeciesDividendos) Then
                    If Not IsNothing(EspeciesDividendosSelected) Then
                        ListaEspeciesDividendos.Remove(EspeciesDividendosSelected)
                        EspeciesDividendosSelected = ListaEspeciesDividendos.LastOrDefault
                        If ListaEspeciesDividendos.Count > 0 Then
                            EspeciesDividendosSelected = ListaEspeciesDividendos.FirstOrDefault
                        End If
                        MyBase.CambioItem("ListaEspeciesDividendosSelected")
                        MyBase.CambioItem("ListaEspeciesDividendos")
                    End If
                End If
            Case "cmEspeciesNemotecnicos"
                If Not IsNothing(ListaEspeciesNemotecnicos) Then
                    If Not IsNothing(EspeciesNemotecnicosSelected) Then
                        ListaEspeciesNemotecnicos.Remove(EspeciesNemotecnicosSelected)
                        EspeciesNemotecnicosSelected = ListaEspeciesNemotecnicos.LastOrDefault
                        If ListaEspeciesNemotecnicos.Count > 0 Then
                            EspeciesNemotecnicosSelected = ListaEspeciesNemotecnicos.FirstOrDefault
                        End If
                        MyBase.CambioItem("ListaEspeciesNemotecnicosSelected")
                        MyBase.CambioItem("ListaEspeciesNemotecnicos")
                    End If
                End If
            Case "cmEspeciesISIN"
                If Not IsNothing(ListaEspeciesISIN) Then
                    If Not IsNothing(EspeciesISINSelected) Then
                        ListaEspeciesISIN.Remove(EspeciesISINSelected)
                        EspeciesISINSelected = ListaEspeciesISIN.LastOrDefault
                        If ListaEspeciesISIN.Count > 0 Then
                            EspeciesISINSelected = ListaEspeciesISIN.FirstOrDefault
                        End If
                        MyBase.CambioItem("ListaEspeciesISINSelected")
                        MyBase.CambioItem("ListaEspeciesISIN")
                    End If
                End If
                '    Case "cmEspeciesISINFungible"
                '        If Not IsNothing(ListaEspeciesISINFungible) Then
                '            If Not IsNothing(EspeciesISINFungibleSelected) Then
                '                'If Not IsNothing(objResultado) Then objResultado._vm.dcProxy.EspeciesISINFungibles.Remove(EspeciesISINFungibleSelected)
                '                'ListaEspeciesISINFungible.Remove(EspeciesISINFungibleSelected)
                '                'If ListaEspeciesISINFungible.Count > 0 Then
                '                '    EspeciesISINFungibleSelected = ListaEspeciesISINFungible.FirstOrDefault
                '                'End If
                '                IsBusy = True
                '                dcProxy.EliminarISINFungible(EspeciesISINFungibleSelected.IDIsinFungible, EspecieSelected.Id, AddressOf terminoBorrarISINFungible, "borrar")
                '                MyBase.CambioItem("ListaEspeciesISINFungible")
                '                MyBase.CambioItem("EspeciesISINFungibleSelected")
                '            End If
                '        End If
        End Select
    End Sub


#Region "EspeciesNemotecnicos"
    '******************************************************** EspeciesNemotecnicos 

    Private _ListaEspeciesNemotecnicos As EntitySet(Of EspeciesNemotecnicos)
    Public Property ListaEspeciesNemotecnicos() As EntitySet(Of EspeciesNemotecnicos)
        Get
            Return _ListaEspeciesNemotecnicos
        End Get
        Set(ByVal value As EntitySet(Of EspeciesNemotecnicos))
            _ListaEspeciesNemotecnicos = value
            EspeciesNemotecnicosSelected = ListaEspeciesNemotecnicos.FirstOrDefault
            MyBase.CambioItem("ListaEspeciesNemotecnicos")
            MyBase.CambioItem("ListaEspeciesNemotecnicosPaged")
        End Set
    End Property

    Public ReadOnly Property ListaEspeciesNemotecnicosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEspeciesNemotecnicos) Then
                Dim view = New PagedCollectionView(_ListaEspeciesNemotecnicos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

    Private _ListaEspeciesNemotecnicosAnterior As List(Of EspeciesNemotecnicos)
    Public Property ListaEspeciesNemotecnicosAnterior() As List(Of EspeciesNemotecnicos)
        Get
            Return _ListaEspeciesNemotecnicosAnterior
        End Get
        Set(ByVal value As List(Of EspeciesNemotecnicos))
            _ListaEspeciesNemotecnicosAnterior = value
            MyBase.CambioItem("ListaEspeciesNemotecnicosAnterior")
        End Set
    End Property

    Private WithEvents _EspeciesNemotecnicosSelected As EspeciesNemotecnicos
    Public Property EspeciesNemotecnicosSelected() As EspeciesNemotecnicos
        Get
            Return _EspeciesNemotecnicosSelected
        End Get
        Set(ByVal value As EspeciesNemotecnicos)

            If Not value Is Nothing Then
                _EspeciesNemotecnicosSelected = value
                MyBase.CambioItem("EspeciesNemotecnicosSelected")
            End If
        End Set
    End Property

#End Region

#Region "EspeciesPrecios"
    '******************************************************** EspeciesPrecios 
    Private _ListaEspeciesPrecios As EntitySet(Of EspeciesPrecios)
    Public Property ListaEspeciesPrecios() As EntitySet(Of EspeciesPrecios)
        Get
            Return _ListaEspeciesPrecios
        End Get
        Set(ByVal value As EntitySet(Of EspeciesPrecios))
            _ListaEspeciesPrecios = value
            MyBase.CambioItem("ListaEspeciesPrecios")
            MyBase.CambioItem("ListaEspeciesPreciosPaged")
        End Set
    End Property

    Public ReadOnly Property ListaEspeciesPreciosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEspeciesPrecios) Then
                Dim view = New PagedCollectionView(_ListaEspeciesPrecios)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
    Private _ListaEspeciesPreciosAnterior As List(Of EspeciesPrecios)
    Public Property ListaEspeciesPreciosAnterior() As List(Of EspeciesPrecios)
        Get
            Return _ListaEspeciesPreciosAnterior
        End Get
        Set(ByVal value As List(Of EspeciesPrecios))
            _ListaEspeciesPreciosAnterior = value
            MyBase.CambioItem("ListaEspeciesPreciosAnterior")
        End Set
    End Property

    Private _EspeciesPreciosSelected As EspeciesPrecios
    Public Property EspeciesPreciosSelected() As EspeciesPrecios
        Get
            Return _EspeciesPreciosSelected
        End Get
        Set(ByVal value As EspeciesPrecios)

            If Not value Is Nothing Then
                _EspeciesPreciosSelected = value
                MyBase.CambioItem("EspeciesPreciosSelected")
            End If
        End Set
    End Property

#End Region

#Region "EspeciesISINFungible"

    Private _ListaEspeciesISINFungible As New ObservableCollection(Of EspeciesISINFungible)
    Public Property ListaEspeciesISINFungible() As ObservableCollection(Of EspeciesISINFungible)
        Get
            Return _ListaEspeciesISINFungible
        End Get
        Set(ByVal value As ObservableCollection(Of EspeciesISINFungible))
            _ListaEspeciesISINFungible = value
            MyBase.CambioItem("ListaEspeciesISINFungible")
            MyBase.CambioItem("ListaEspeciesISINFungiblePaged")
        End Set
    End Property

    Private _ListaEspeciesISINFungibleSinFiltro As New ObservableCollection(Of EspeciesISINFungible)
    Public Property ListaEspeciesISINFungibleSinFiltro() As ObservableCollection(Of EspeciesISINFungible)
        Get
            Return _ListaEspeciesISINFungibleSinFiltro
        End Get
        Set(ByVal value As ObservableCollection(Of EspeciesISINFungible))
            _ListaEspeciesISINFungibleSinFiltro = value
        End Set
    End Property

    Private _ListaAmortizaciones As EntitySet(Of AmortizacionesEspeci)
    Public Property ListaAmortizaciones As EntitySet(Of AmortizacionesEspeci)

        Get
            Return _ListaAmortizaciones
        End Get
        Set(ByVal value As EntitySet(Of AmortizacionesEspeci))
            _ListaAmortizaciones = value
            MyBase.CambioItem("ListaAmortizaciones")
        End Set
    End Property

    Public ReadOnly Property ListaEspeciesISINFungiblePaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEspeciesISINFungible) Then
                Dim view = New PagedCollectionView(_ListaEspeciesISINFungible)
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

    Private _EspeciesISINFungibleSelected As EspeciesISINFungible
    Public Property EspeciesISINFungibleSelected() As EspeciesISINFungible
        Get
            Return _EspeciesISINFungibleSelected
        End Get
        Set(ByVal value As EspeciesISINFungible)

            If Not value Is Nothing Then
                _EspeciesISINFungibleSelected = value
                MyBase.CambioItem("EspeciesISINFungibleSelected")
            End If
        End Set
    End Property

#End Region

#Region "EspeciesISIN"
    '******************************************************** EspeciesISIN 
    Private _ListaEspeciesISIN As EntitySet(Of EspeciesISIN)
    Public Property ListaEspeciesISIN() As EntitySet(Of EspeciesISIN)
        Get
            Return _ListaEspeciesISIN
        End Get
        Set(ByVal value As EntitySet(Of EspeciesISIN))
            _ListaEspeciesISIN = value
            MyBase.CambioItem("ListaEspeciesISIN")
            MyBase.CambioItem("ListaEspeciesISINPaged")
        End Set
    End Property

    Public ReadOnly Property ListaEspeciesISINPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEspeciesISIN) Then
                Dim view = New PagedCollectionView(_ListaEspeciesISIN)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
    Private _ListaEspeciesISINAnterior As List(Of EspeciesISIN)
    Public Property ListaEspeciesISINAnterior() As List(Of EspeciesISIN)
        Get
            Return _ListaEspeciesISINAnterior
        End Get
        Set(ByVal value As List(Of EspeciesISIN))
            _ListaEspeciesISINAnterior = value
            MyBase.CambioItem("ListaEspeciesISINAnterior")
        End Set
    End Property

    Private _EspeciesISINSelected As EspeciesISIN
    Public Property EspeciesISINSelected() As EspeciesISIN
        Get
            Return _EspeciesISINSelected
        End Get
        Set(ByVal value As EspeciesISIN)
            If Not value Is Nothing Then
                _EspeciesISINSelected = value
                MyBase.CambioItem("EspeciesISINSelected")
            End If
        End Set
    End Property

    Private _TextoFiltroISIN As String
    Public Property TextoFiltroISIN() As String
        Get
            Return _TextoFiltroISIN
        End Get
        Set(ByVal value As String)
            _TextoFiltroISIN = value
            MyBase.CambioItem("TextoFiltroISIN")
        End Set
    End Property

    Private _EmisionFiltroISIN As Date?
    Public Property EmisionFiltroISIN() As Date?
        Get
            Return _EmisionFiltroISIN
        End Get
        Set(ByVal value As Date?)
            _EmisionFiltroISIN = value
            MyBase.CambioItem("EmisionFiltroISIN")
        End Set
    End Property

    Private _VencimientoFiltroISIN As Date?
    Public Property VencimientoFiltroISIN() As Date?
        Get
            Return _VencimientoFiltroISIN
        End Get
        Set(ByVal value As Date?)
            _VencimientoFiltroISIN = value
            MyBase.CambioItem("VencimientoFiltroISIN")
        End Set
    End Property



#End Region

#Region "EspeciesTotales"

    '******************************************************** EspeciesTotales 
    Private _ListaEspeciesTotales As EntitySet(Of EspeciesTotales)
    Public Property ListaEspeciesTotales() As EntitySet(Of EspeciesTotales)
        Get
            Return _ListaEspeciesTotales
        End Get
        Set(ByVal value As EntitySet(Of EspeciesTotales))
            _ListaEspeciesTotales = value
            MyBase.CambioItem("ListaEspeciesTotales")
            MyBase.CambioItem("ListaEspeciesTotalesPaged")
        End Set
    End Property

    Public ReadOnly Property ListaEspeciesTotalesPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaEspeciesTotales) Then
                Dim view = New PagedCollectionView(_ListaEspeciesTotales)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property
    Private _ListaEspeciesTotalesAnterior As List(Of EspeciesTotales)
    Public Property ListaEspeciesTotalesAnterior() As List(Of EspeciesTotales)
        Get
            Return _ListaEspeciesTotalesAnterior
        End Get
        Set(ByVal value As List(Of EspeciesTotales))
            _ListaEspeciesTotalesAnterior = value
            MyBase.CambioItem("ListaEspeciesTotalesAnterior")
        End Set
    End Property

    Private _EspeciesTotalesSelected As EspeciesTotales
    Public Property EspeciesTotalesSelected() As EspeciesTotales
        Get
            Return _EspeciesTotalesSelected
        End Get
        Set(ByVal value As EspeciesTotales)

            If Not value Is Nothing Then
                _EspeciesTotalesSelected = value
                MyBase.CambioItem("EspeciesTotalesSelected")
            End If
        End Set
    End Property
#End Region

#Region "ArbitrajeADR"

    Private _HabilitarTabArbitrajeADR As String
    Public Property HabilitarTabArbitrajeADR As String
        Get
            Return _HabilitarTabArbitrajeADR
        End Get
        Set(ByVal value As String)
            _HabilitarTabArbitrajeADR = value
            MyBase.CambioItem("HabilitarTabArbitrajeADR")
        End Set
    End Property

    Private _ADRFiltroEspecie As String
    Public Property ADRFiltroEspecie() As String
        Get
            Return _ADRFiltroEspecie
        End Get
        Set(ByVal value As String)
            _ADRFiltroEspecie = value
            MyBase.CambioItem("ADRFiltroEspecie")
        End Set
    End Property

    Private _ADRFiltroFecha As Date?
    Public Property ADRFiltroFecha() As Date?
        Get
            Return _ADRFiltroFecha
        End Get
        Set(ByVal value As Date?)
            _ADRFiltroFecha = value
            MyBase.CambioItem("ADRFiltroFecha")
        End Set
    End Property

    Private _ListaArbitrajeADR As New ObservableCollection(Of ArbitrajeADR)
    Public Property ListaArbitrajeADR() As ObservableCollection(Of ArbitrajeADR)
        Get
            Return _ListaArbitrajeADR
        End Get
        Set(ByVal value As ObservableCollection(Of ArbitrajeADR))
            _ListaArbitrajeADR = value
            MyBase.CambioItem("ListaArbitrajeADR")
        End Set
    End Property

    Private _ListaArbitrajeADREliminado As New ObservableCollection(Of ArbitrajeADR)
    Public Property ListaArbitrajeADREliminado() As ObservableCollection(Of ArbitrajeADR)
        Get
            Return _ListaArbitrajeADREliminado
        End Get
        Set(ByVal value As ObservableCollection(Of ArbitrajeADR))
            _ListaArbitrajeADREliminado = value
            MyBase.CambioItem("ListaArbitrajeADREliminado")
        End Set
    End Property

    Private _ListaArbitrajeADRSinFiltro As New ObservableCollection(Of ArbitrajeADR)
    Public Property ListaArbitrajeADRSinFiltro() As ObservableCollection(Of ArbitrajeADR)
        Get
            Return _ListaArbitrajeADRSinFiltro
        End Get
        Set(ByVal value As ObservableCollection(Of ArbitrajeADR))
            _ListaArbitrajeADRSinFiltro = value
        End Set
    End Property

    Private WithEvents _ArbitrajeADRSelected As ArbitrajeADR
    Public Property ArbitrajeADRSelected() As ArbitrajeADR
        Get
            Return _ArbitrajeADRSelected
        End Get
        Set(ByVal value As ArbitrajeADR)

            If Not value Is Nothing Then
                _ArbitrajeADRSelected = value
                MyBase.CambioItem("ArbitrajeADRSelected")
            End If
        End Set
    End Property

    Public Sub DetalleADR_FiltrarLista()

        Try
            Dim EsFechaVigencia As Boolean = IsDate(ADRFiltroFecha)
            Dim SinResultado As String = "" 'Se crea esta variable para que en caso de algunos ISIN de acciones que no tienen fecha de vencimiento ni de emisión 
            'al menos cargue el grid en blanco y no quede como si no hubiera hecho nada (básicamente así funciona el filtro)

            If ADRFiltroEspecie = "" And IsNothing(ADRFiltroFecha) Then
                A2Utilidades.Mensajes.mostrarMensaje("Seleccione algun campo para realizar el filtro.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            If Not IsNothing(ListaArbitrajeADRSinFiltro) Then
                If EsFechaVigencia And (ADRFiltroEspecie <> Nothing) Then
                    ListaArbitrajeADR = New ObservableCollection(Of ArbitrajeADR)(ListaArbitrajeADRSinFiltro.Where(Function(i) i.dtmVigencia = ADRFiltroFecha And (i.strIDEspecie.ToUpper.Contains(ADRFiltroEspecie.ToUpper))))
                ElseIf (ADRFiltroEspecie <> Nothing) Then
                    ListaArbitrajeADR = New ObservableCollection(Of ArbitrajeADR)(ListaArbitrajeADRSinFiltro.Where(Function(i) i.strIDEspecie.ToUpper.Contains(ADRFiltroEspecie.ToUpper)))
                ElseIf EsFechaVigencia Then
                    ListaArbitrajeADR = New ObservableCollection(Of ArbitrajeADR)(ListaArbitrajeADRSinFiltro.Where(Function(i) i.dtmVigencia = ADRFiltroFecha))
                Else
                    ListaArbitrajeADR = New ObservableCollection(Of ArbitrajeADR)(ListaArbitrajeADRSinFiltro.Where(Function(i) i.strIDEspecie = SinResultado))
                End If
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al realizar el filtro.",
                                                         Me.ToString(), "DetalleADR_FiltrarLista", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Sub DetalleADR_LimpiarFiltro()

        Try
            ADRFiltroEspecie = Nothing
            ADRFiltroFecha = Nothing
            ListaArbitrajeADR.Clear()
            If Not IsNothing(ListaArbitrajeADRSinFiltro) Then
                ListaArbitrajeADR = New ObservableCollection(Of ArbitrajeADR)(ListaArbitrajeADRSinFiltro.OrderByDescending(Function(x) x.dtmVigencia))
            End If
            MyBase.CambioItem("ListaArbitrajeADRSinFiltro")
            MyBase.CambioItem("ListaArbitrajeADR")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al limpiar el filtro de ADR.",
                                                         Me.ToString(), "DetalleADR_LimpiarFiltro", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Sub DetalleADR_NuevoRegistro()
        Try
            Dim NewArbitrajeADR As New ArbitrajeADR
            NewArbitrajeADR.intIDEntidad = -1
            NewArbitrajeADR.strAccion = ValoresUserState.Ingresar.ToString()
            NewArbitrajeADR.strIDEspecieADR = EspecieSelected.Id
            NewArbitrajeADR.intIDArbitrajeADR = 0
            NewArbitrajeADR.dtmVigencia = Date.Now
            NewArbitrajeADR.strIDEspecie = String.Empty
            NewArbitrajeADR.dblValor = 0

            If Not IsNothing(ArbitrajeADRSelected) Then
                If ListaArbitrajeADR.Count < 1 Then
                    NewArbitrajeADR.intIDEntidad = -1
                Else
                    NewArbitrajeADR.intIDEntidad = ListaArbitrajeADR.Last.intIDEntidad + 1
                End If
            End If

            ListaArbitrajeADR.Add(NewArbitrajeADR)
            ListaArbitrajeADRSinFiltro.Add(NewArbitrajeADR)
            ArbitrajeADRSelected = NewArbitrajeADR
            InhabilitarDetalles = False
            MyBase.CambioItem("ArbitrajeADRSelected")
            MyBase.CambioItem("ListaArbitrajeADR")

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al crear un nuevo registo",
             Me.ToString(), "DetalleADR_NuevoRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub DetalleADR_BorrarRegistro()
        Try
            If Not IsNothing(ListaArbitrajeADR) Then
                If Not IsNothing(ArbitrajeADRSelected) Then

                    ListaArbitrajeADR.Remove(ArbitrajeADRSelected)
                    ListaArbitrajeADRSinFiltro.Remove(ArbitrajeADRSelected)
                    ListaArbitrajeADREliminado.Add(ArbitrajeADRSelected)
                    ArbitrajeADRSelected = ListaArbitrajeADR.LastOrDefault

                    If ListaArbitrajeADR.Count > 0 Then
                        ArbitrajeADRSelected = ListaArbitrajeADR.FirstOrDefault
                    End If

                    MyBase.CambioItem("ArbitrajeADRSelected")
                    MyBase.CambioItem("ListaArbitrajeADR")
                    MyBase.CambioItem("ListaArbitrajeADREliminado")
                End If
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al borrar un registo.",
            Me.ToString(), "DetalleADR_BorrarRegistro", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

#End Region

    Friend Sub buscarItem(ByVal pstrTipoItem As String, Optional ByVal pstrIdItem As String = "")
        Dim strIdItem As String = String.Empty
        Dim logConsultar As Boolean = False

        Try
            If Not Me.EspecieSelected Is Nothing Then
                Select Case pstrTipoItem.ToLower()
                    Case "emisor"
                        pstrIdItem = pstrIdItem.Trim()
                        If pstrIdItem.Equals(String.Empty) Then
                            strIdItem = Me.EspecieSelected.IdEmisor
                        Else
                            strIdItem = pstrIdItem
                        End If
                        If Not strIdItem.Equals(String.Empty) Then
                            logConsultar = True
                        End If
                        If logConsultar Then
                            mdcProxyUtilidad01.BuscadorGenericos.Clear()
                            mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarItemEspecificoQuery(pstrTipoItem, strIdItem, Program.Usuario, Program.HashConexion), AddressOf buscarGenericoCompleted, pstrTipoItem)
                        End If
                    Case "emisor,property"
                        If Editando Then
                            If EspecieSelected.IdEmisor = 0 Or EspecieSelected.IdEmisor = Nothing Then
                                EmisorClaseSelected.Emisor = String.Empty
                            End If
                            If Not _EspecieSelected.IdEmisor.Equals(0) Then
                                mdcProxyUtilidad01.BuscadorGenericos.Clear()
                                mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarItemEspecificoQuery("emisor", EspecieSelected.IdEmisor, Program.Usuario, Program.HashConexion), AddressOf buscarGenericoCompleted, "emisor,property")
                            End If
                        End If

                    Case "clase"
                        If Not IsNothing(_EspecieSelected) Then
                            ClaseClaseSelected.IdClase = _EspecieSelected.IDClase
                            ClaseClaseSelected.Clase = _EspecieSelected.strClase
                        End If
                        'pstrIdItem = pstrIdItem.Trim()
                        'If pstrIdItem.Equals(String.Empty) Then
                        '    strIdItem = Me.EspecieSelected.IDClase
                        'Else
                        '    strIdItem = pstrIdItem
                        'End If
                        'If Not strIdItem.Equals(String.Empty) Then
                        '    logConsultar = True
                        'End If
                        'If logConsultar Then
                        '    mdcProxyUtilidad02.BuscadorGenericos.Clear()
                        '    mdcProxyUtilidad02.Load(mdcProxyUtilidad02.buscarItemEspecificoQuery(pstrTipoItem, strIdItem, Program.Usuario, Program.HashConexion), AddressOf buscarGenericoCompleted, pstrTipoItem)
                        'End If
                    Case "clase,property"
                        If Editando Then
                            If EspecieSelected.IDClase = 0 Or EspecieSelected.IDClase = Nothing Then
                                ClaseClaseSelected.Clase = String.Empty
                                'HabilitarCamposTituloParticipativo = True
                                HabilitarCamposTabCFTituloParticipativo = True
                                'HabilitarTipo = True
                                'EspecieSelected.TituloParticipativo = False
                                'ConsultarValoresDefectoTituloParticipativo()
                                'logLimpiarTituloParticipativo = True
                            End If
                            If Not _EspecieSelected.IDClase.Equals(0) Then
                                mdcProxyUtilidad01.BuscadorGenericos.Clear()
                                mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarItemsQuery(EspecieSelected.IDClase, "Clase", "A", Clase, "", "", Program.Usuario, Program.HashConexion), AddressOf buscarGenericoCompleted, "clase,property")
                            End If
                        End If

                    Case "admonemision,property"
                        If Editando Then
                            If EspecieSelected.IDAdmonEmision = 0 Or EspecieSelected.IDAdmonEmision = Nothing Then
                                EmisoresClaseSelected.NombreAdmonEmision = String.Empty
                            End If
                            If Not _EspecieSelected.IDAdmonEmision.Equals(0) Then
                                mdcProxyUtilidad01.BuscadorGenericos.Clear()
                                mdcProxyUtilidad01.Load(mdcProxyUtilidad01.buscarItemEspecificoQuery("admonemision", EspecieSelected.IDAdmonEmision, Program.Usuario, Program.HashConexion), AddressOf buscarGenericoCompleted, "admonemision,property")

                            End If
                        End If

                    Case "admonemision"
                        pstrIdItem = pstrIdItem.Trim()
                        If pstrIdItem.Equals(String.Empty) Then
                            strIdItem = Me.EspecieSelected.IDAdmonEmision
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


                    Case "claseinversion"
                        pstrIdItem = pstrIdItem.Trim()
                        If pstrIdItem.Equals(String.Empty) Then
                            strIdItem = Me.EspecieSelected.ClaseInversion
                        Else
                            strIdItem = pstrIdItem
                        End If
                        If Not IsNothing(strIdItem) Then
                            If Not strIdItem.Equals(String.Empty) Then
                                logConsultar = True
                            End If
                        End If

                        If logConsultar Then
                            mdcProxyUtilidad04.BuscadorGenericos.Clear()
                            mdcProxyUtilidad04.Load(mdcProxyUtilidad04.buscarItemEspecificoQuery(pstrTipoItem, strIdItem, Program.Usuario, Program.HashConexion), AddressOf buscarGenericoCompleted, pstrTipoItem)
                        End If


                    Case Else
                        logConsultar = False
                End Select


            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los datos del cliente de la orden", Me.ToString(), "buscarComitente", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <history>
    ''' Descripción:        Desarrollo PreEspecies. Se agregó lógica para recargar la especie cuando es una preespecie.
    ''' Responsable:        Jorge Peña (Alcuadrado S.A.)
    ''' Fecha:              9 de Junio/2014
    ''' Pruebas CB:         Jorge Peña (Alcuadrado S.A.) - 9 de Junio/2014
    ''' </history>
    ''' <history>
    ''' Id Cambio:          JEPM20150721
    ''' Descripción:        Ajuste. Se añade validación para el usuario cuando el código de la "Admón Emisión" no existe.
    ''' Responsable:        Javier Pardo (Alcuadrado S.A.)
    ''' Fecha:              21 de Julio/2015
    ''' Pruebas CB:         Javier Pardo (Alcuadrado S.A.) - 22 de Julio/2015
    ''' </history> 
    Private Sub buscarGenericoCompleted(ByVal lo As LoadOperation(Of A2.OyD.OYDServer.RIA.Web.OYDUtilidades.BuscadorGenerico))
        Dim strTipoItem As String
        Try
            If lo.UserState Is Nothing Then
                strTipoItem = ""
            Else
                strTipoItem = lo.UserState
            End If

            If lo.Entities.ToList.Count > 0 Then
                TextoFiltroISIN = String.Empty
                VencimientoFiltroISIN = Nothing
                EmisionFiltroISIN = Nothing
                'If Not logDuplicar And Not mlogNuevoRegistro And Not Editando Then
                '    HabilitarDuplicarRegistro = Visibility.Visible
                'End If
                Select Case strTipoItem.ToLower()
                    Case "emisor"
                        If Not IsNothing(Me.EmisorClaseSelected) Then
                            Me.EmisorClaseSelected.IdEmisor = lo.Entities.ToList.Item(0).Id
                            Me.EmisorClaseSelected.Emisor = lo.Entities.ToList.Item(0).Nombre
                        End If


                    Case "emisor,property"
                        If Not IsNothing(Me.EmisorClaseSelected) Then
                            Me.EmisorClaseSelected.IdEmisor = lo.Entities.ToList.Item(0).Id
                            Me.EmisorClaseSelected.Emisor = lo.Entities.ToList.Item(0).Nombre
                        End If
                    Case "clase"
                        If Not IsNothing(Me.ClaseClaseSelected) Then
                            Me.ClaseClaseSelected.IdClase = lo.Entities.ToList.Item(0).Id
                            Me.ClaseClaseSelected.Clase = lo.Entities.ToList.Item(0).Nombre
                        End If

                        If Not IsNothing(EspecieAnteriorPreEspecie) And EspecieSelected.strPrecreada = "SI" Then
                            ClaseClaseSelected.Clase = EspecieAnteriorPreEspecie.strClase
                        End If

                        strdescripcionclase = lo.Entities.ToList.Item(0).Nombre
                        _ClaseNuevo.IdClase = lo.Entities.ToList.Item(0).Id
                        _ClaseNuevo.Clase = lo.Entities.ToList.Item(0).Nombre
                    Case "clase,property"
                        Dim ClaseLinq = (From obj In lo.Entities.ToList Where obj.IdItem = CStr(EspecieSelected.IDClase) Select obj).ToList
                        If ClaseLinq.Count > 0 Then
                            'Dim ClaseLinq = (From obj In lo.Entities.ToList Where obj.IdItem = CStr(EspecieSelected.IDClase) Select obj).ToList.ElementAt(0)
                            If Not IsNothing(Me.ClaseClaseSelected) Then
                                Me.ClaseClaseSelected.IdClase = ClaseLinq.ElementAt(0).Id
                                Me.ClaseClaseSelected.Clase = ClaseLinq.ElementAt(0).Nombre
                                Me.EspecieSelected.ClaseContableTitulo = ClaseLinq.ElementAt(0).InfoAdicional01  'JAEZ 20161215 
                                strdescripcionclase = ClaseLinq.ElementAt(0).Nombre

                                Me.EspecieSelected.TituloParticipativo = ClaseLinq.ElementAt(0).InfoAdicional02
                                If Me.EspecieSelected.TituloParticipativo Then
                                    ' Me.HabilitarCamposTituloParticipativo = False
                                    Me.HabilitarCamposTabCFTituloParticipativo = False
                                    'Me.HabilitarTipo = False
                                Else
                                    'Me.HabilitarCamposTituloParticipativo = True
                                    Me.HabilitarCamposTabCFTituloParticipativo = True
                                    'Me.HabilitarTipo = True
                                    'logLimpiarTituloParticipativo = True
                                End If
                                'ConsultarValoresDefectoTituloParticipativo()
                            End If
                        Else
                            A2Utilidades.Mensajes.mostrarMensaje("La clase con el código " & EspecieSelected.IDClase.ToString & " no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                            If Not IsNothing(ClaseClaseSelected) Then
                                Me.ClaseClaseSelected.IdClase = 0
                                Me.ClaseClaseSelected.Clase = String.Empty
                                strdescripcionclase = String.Empty
                                Me.EspecieSelected.IDClase = 0
                                'Me.EspecieSelected.TituloParticipativo = False
                            End If
                            'Me.HabilitarCamposTituloParticipativo = True
                            Me.HabilitarCamposTabCFTituloParticipativo = True
                            'Me.HabilitarTipo = True
                            'logLimpiarTituloParticipativo = True
                            'ConsultarValoresDefectoTituloParticipativo()
                        End If

                    Case "admonemision,property"
                        If Not IsNothing(Me.EmisoresClaseSelected) Then
                            Me.EmisoresClaseSelected.IDAdmonEmision = lo.Entities.ToList.Item(0).Id
                            Me.EmisoresClaseSelected.Emisores = lo.Entities.ToList.Item(0).Nombre
                            Me.EmisoresClaseSelected.NombreAdmonEmision = lo.Entities.ToList.Item(0).Nombre
                        End If



                    Case "claseinversion"
                        If Not IsNothing(Me.claseinversionSelected) Then
                            Me.claseinversionSelected.strDescripcion = lo.Entities.ToList.Item(0).Descripcion


                            DescripcionClaseInversion = lo.Entities.ToList.Item(0).IdItem & ", " & lo.Entities.ToList.Item(0).Nombre & _
                                Environment.NewLine & Me.claseinversionSelected.strDescripcion
                        End If
                End Select
            Else
                Select Case strTipoItem.ToLower()
                    Case "emisor,property"
                        A2Utilidades.Mensajes.mostrarMensaje("El emisor con el código " & EspecieSelected.IdEmisor.ToString & " no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        If Not IsNothing(Me.EmisorClaseSelected) Then
                            Me.EmisorClaseSelected.IdEmisor = 0
                            Me.EmisorClaseSelected.Emisor = String.Empty
                            Me.EspecieSelected.IdEmisor = 0
                        End If
                    Case "clase,property"
                        A2Utilidades.Mensajes.mostrarMensaje("La clase con el código " & EspecieSelected.IDClase.ToString & " no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        If Not IsNothing(Me.ClaseClaseSelected) Then
                            Me.ClaseClaseSelected.IdClase = 0
                            Me.ClaseClaseSelected.Clase = String.Empty
                            strdescripcionclase = String.Empty
                            Me.EspecieSelected.IDClase = 0
                            'Me.EspecieSelected.TituloParticipativo = False
                        End If
                        'Me.HabilitarCamposTituloParticipativo = True
                        Me.HabilitarCamposTabCFTituloParticipativo = True
                        'Me.HabilitarTipo = True
                        'logLimpiarTituloParticipativo = True
                        'ConsultarValoresDefectoTituloParticipativo()
                    Case "admonemision" 'JEPM20150721
                        'JEPM20150722 No mostrar mensaje en vista lista, sino sólo al editar. Pero sí dejar la propiedad con el valor por defecto.
                        'A2Utilidades.Mensajes.mostrarMensaje("La Administración de la Emisión con el código " & EspecieSelected.IDAdmonEmision.ToString & " no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        If Not IsNothing(Me.EmisoresClaseSelected) Then
                            Me.EmisoresClaseSelected.IDAdmonEmision = String.Empty
                            Me.EmisoresClaseSelected.Emisores = String.Empty
                            Me.EmisoresClaseSelected.NombreAdmonEmision = String.Empty
                        End If
                    Case "admonemision,property"
                        A2Utilidades.Mensajes.mostrarMensaje("La Administración de la Emisión con el código " & EspecieSelected.IDAdmonEmision.ToString & " no existe", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        If Not IsNothing(Me.EmisoresClaseSelected) Then
                            Me.EmisoresClaseSelected.IDAdmonEmision = String.Empty
                            Me.EmisoresClaseSelected.Emisores = String.Empty
                            Me.EmisoresClaseSelected.NombreAdmonEmision = String.Empty
                        End If
                End Select
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la consulta de items ("""")", Me.ToString(), "buscarGenericoCompleted", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Método para establecer los valores por defecto de los campos Clase Inversión y Clase Contable Título cuando la especie es una acción
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Descripción:        Valida que si el objeto "claseinversionSelected" esta nulo se realiza la instancia correspondiente
    ''' Responsable:        Germán Arbey González Osorio (Alcuadrado S.A.)
    ''' Fecha:              Diciembre 09/2014
    ''' Pruebas CB:         Germán Arbey González Osorio (Alcuadrado S.A.) - Diciembre 09/2014
    ''' </history>
    Public Async Sub ConsultarValoresDefectoBursatilidad()
        Try
            If Not IsNothing(EspecieSelected) And Editando = True Then

                Dim strRetorno As String = String.Empty
                Dim strValoresPorDefecto As String()
                Dim strClaseContable As String = String.Empty
                Dim strClaseInversion As String = String.Empty
                Dim strClaseInversionDesc As String = String.Empty

                Dim objRet As InvokeOperation(Of String)

                objRet = Await dcProxy.ConsultarValoresDefectoBursatilidadSync(pstrBursatilidad:=EspecieSelected.Bursatilidad,
                                                                               pintEmisor:=EspecieSelected.IdEmisor, pstrUsuario:=Program.Usuario, pstrInfoConexion:=Program.HashConexion).AsTask()

                If Not objRet Is Nothing Then

                    If objRet.HasError Then
                        If objRet.Error Is Nothing Then
                            A2Utilidades.Mensajes.mostrarMensaje("Se generó un problema al ejecutar la consulta de valoracion por defecto en bursatilidad.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores)
                        End If
                        objRet.MarkErrorAsHandled()
                    Else

                        If IsNothing(Me.claseinversionSelected) Then
                            Me.claseinversionSelected = New claseinversion() With {.strDescripcion = strdescripcionclase}
                        End If

                        strRetorno = objRet.Value

                        If strRetorno = "" Then
                            EspecieSelected.ClaseInversion = ""
                            EspecieSelected.ClaseContableTitulo = ""
                            claseinversionSelected.strcodigo = ""
                            claseinversionSelected.strDescripcion = ""
                        Else
                            strValoresPorDefecto = strRetorno.Split(",")
                            EspecieSelected.ClaseContableTitulo = strValoresPorDefecto.ElementAt(0)
                            EspecieSelected.ClaseInversion = strValoresPorDefecto.ElementAt(1)
                            claseinversionSelected.strcodigo = strValoresPorDefecto.ElementAt(1)
                            claseinversionSelected.strDescripcion = strValoresPorDefecto.ElementAt(2).Replace("|", ",")
                            DescripcionClaseInversion = claseinversionSelected.strcodigo & ", " & claseinversionSelected.strcodigo & _
                                                        Environment.NewLine & claseinversionSelected.strDescripcion
                        End If

                        MyBase.CambioItem("claseinversionSelected")
                        MyBase.CambioItem("EspecieSelected")

                    End If

                End If

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al consultar los valores por defecto en la bursatilidad.", _
                                                             Me.ToString(), "ConsultarListaCalificaiones", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub PreguntarDuplicarRegistro()
        Try
            viewEspecie = Nothing
            'viewEspecie.GridEdicion.Children.Add(viewEspecies)

            If Not IsNothing(_EspecieSelected) Then
                If EspecieSelected.Activo Then
                    mostrarMensajePregunta("¿Esta seguro que desea duplicar el registro?", _
                                              Program.TituloSistema, _
                                              "DUPLICARREGISTRO", _
                                              AddressOf TerminaPregunta, False)
                Else
                    A2Utilidades.Mensajes.mostrarMensaje("La especie se encuentra inactiva, no es posible duplicarla.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

            Else
                A2Utilidades.Mensajes.mostrarMensaje("Debe de seleccionar un registro para duplicar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al preguntar sí se deseaba duplicar el registro", Me.ToString(), "PreguntarDuplicarRegistro", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Lanza maestro de ISIN fungibles indicando si es en modo de edición o nuevo -- 08/08/2013
    ''' </summary>
    ''' <param name="pisinISINSelected"></param>
    ''' <remarks></remarks>
    ''' <history>
    ''' Descripción:        Se envía la variable "Editando" a la vista ISINesView para habilitar o deshabilitar los botones de grabar y cancelar
    ''' Responsable:        Germán Arbey González Osorio (Alcuadrado S.A.)
    ''' Fecha:              Diciembre 15/2014
    ''' Pruebas CB:         Germán Arbey González Osorio (Alcuadrado S.A.) - Diciembre 15/2014
    ''' </history>
    Private Sub LanzarMaestroISIN(pisinISINSelected As Object, pmlogNuevoRegistro As Object)
        Try
            If (Not EspecieSelected.EsAccion) And String.IsNullOrEmpty(EspecieSelected.TipoTasaFija) Then
                A2Utilidades.Mensajes.mostrarMensaje("Debe seleccionar el tipo de tasa fija.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            Else
                Dim objISIN As ISINesView
                Dim strEspecie As String
                Dim strTipoTasaFija As String

                If Not EspecieSelected.EsAccion Then
                    If EspecieSelected.TipoTasaFija = "V" And IsNothing(EspecieSelected.Indicador) Then
                        A2Utilidades.Mensajes.mostrarMensaje("El indicador es un campo requerido.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        Exit Sub
                    End If
                End If

                strTipoTasaFija = IIf(String.IsNullOrEmpty(EspecieSelected.TipoTasaFija), String.Empty, EspecieSelected.TipoTasaFija)

                If TypeOf pisinISINSelected Is String Then
                    strEspecie = pisinISINSelected
                    objISIN = New ISINesView(pisinISINSelected.ToString, EspecieSelected.EsAccion, strTipoTasaFija, EspecieSelected.IDEspecies <> -1, EspecieSelected.Amortiza, mlogNuevoRegistro, EspecieSelected.BaseCalculoInteres, EspecieSelected.Nombre, EspecieSelected.Indicador, Editando, EspecieSelected.ConceptoRetencion, EspecieSelected.TituloParticipativo, EspecieSelected.TipoEspecie)
                Else
                    strEspecie = CType(pisinISINSelected, EspeciesISINFungible).IDEspecie
                    objISIN = New ISINesView(CType(pisinISINSelected, EspeciesISINFungible), EspecieSelected.EsAccion, EspecieSelected.TipoTasaFija, EspecieSelected.IDEspecies <> -1, EspecieSelected.Amortiza, EspecieSelected.BaseCalculoInteres, EspecieSelected.Indicador, Editando, EspecieSelected.ConceptoRetencion, EspecieSelected.TituloParticipativo, EspecieSelected.TipoEspecie)
                End If

                AddHandler objISIN.Closed, AddressOf TerminoEdicionISINes
                Program.Modal_OwnerMainWindowsPrincipal(objISIN)
                objISIN.ShowDialog()
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarMensaje("No se encontraron ISINes para la especie en edición.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        End Try
    End Sub

    ''' <summary>
    ''' Recibe respuesta del popup con maestro de ISIN Fungible -- 08/08/2013
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub TerminoEdicionISINes(sender As Object, e As EventArgs)
        Try
            Dim indice As Integer
            Dim isin As EspeciesISINFungible
            Dim cwISINes As Window
            Dim lstIsin As New List(Of EspeciesISINFungible)
            cwISINes = CType(sender, Window)
            objResultado = CType(cwISINes, ISINesView)
            Dim isinItem As EspeciesISINFungible = objResultado.isinIsinFungible
            If CBool(objResultado.blnRespuesta) Then

                ' Especie nueva, aún no se ha generado
                If EspecieSelected.IDEspecies = -1 Then

                    ' si no hay registros de ISIN Fungibles, crea la lista y agrega registros
                    If IsNothing(ListaEspeciesISINFungible) Then

                        lstIsin = objResultado._vm.dcProxy.EspeciesISINFungibles.ToList
                        ListaEspeciesISINFungible = New ObservableCollection(Of EspeciesISINFungible)

                        For Each it In lstIsin
                            ListaEspeciesISINFungible.Add(it)
                        Next

                        EspeciesISINFungibleSelected = ListaEspeciesISINFungible.LastOrDefault

                    Else
                        ' verifica la existencia del registro en la lista
                        isin = ListaEspeciesISINFungible.Where(Function(i) i.IDIsinFungible = isinItem.IDIsinFungible).FirstOrDefault

                        If IsNothing(isin) Then
                            ListaEspeciesISINFungible.Add(isinItem)
                        Else
                            indice = ListaEspeciesISINFungible.IndexOf(isin)
                            ListaEspeciesISINFungible(indice) = isinItem
                        End If

                        EspeciesISINFungibleSelected = isinItem

                    End If
                Else
                    dcProxy.EspeciesISINFungibles.Clear()
                    dcProxy.Load(dcProxy.EspeciesISINFungibleConsultarQuery(EspecieSelected.Id, String.Empty, EspecieSelected.TipoTasaFija, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesISINFungible, Nothing)
                End If

            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al asignar liquidaciones probables", Me.ToString(), "TerminoEdicionISINes", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Carga el valor del parámetro MOSTRAR_BARRA_BOTONES -- 08/08/2013
    ''' </summary>
    ''' <param name="obj"></param>
    ''' <remarks></remarks>
    Private Sub TerminotraerparametroMenu(ByVal obj As InvokeOperation(Of String))
        If obj.HasError Then
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "No se recuperó la validacion", Me.ToString(), "TerminotraerparametroMenu", Program.TituloSistema, Program.Maquina, obj.Error, Program.RutaServicioLog)

        Else
            If obj.Value = "NO" Then
                blnMostrarBarraBoton = False
                habilitarLinkISIN = True
            Else
                blnMostrarBarraBoton = True
                habilitarLinkISIN = Editando
            End If
        End If
    End Sub

    'Descripción:   Se agrega el campo "Democratización" (Aplica solo para acciones).
    'Responsable:   Jorge Peña (Alcuadrado S.A.)
    'Fecha:         5 de Mayo/2016
    Private Sub TerminoConsultarParametros(ByVal lo As InvokeOperation(Of String))
        Try
            If Not lo.HasError Then
                Select Case lo.UserState.ToString
                    Case "VISUALIZAR_CAMPO_DEMOCRATIZACION"
                        VISUALIZAR_CAMPO_DEMOCRATIZACION = lo.Value.ToString
                        If String.IsNullOrEmpty(VISUALIZAR_CAMPO_DEMOCRATIZACION) Then
                            VISUALIZAR_CAMPO_DEMOCRATIZACION = "NO"
                        End If
                End Select
            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de paramétros", _
                                                     Me.ToString(), "TerminoConsultarParametros", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de paramétros", _
                                     Me.ToString(), "TerminoConsultarParametros", Application.Current.ToString(), Program.Maquina, lo.Error)
        End Try
    End Sub

    Private Sub terminoBorrarISINFungible(ByVal So As InvokeOperation(Of String))
        If So.HasError Then
            A2Utilidades.Mensajes.mostrarMensaje(So.Error.Message, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
        Else
            If Not (So.Value) = String.Empty Then
                A2Utilidades.Mensajes.mostrarMensaje(So.Value.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

                If So.UserState = "borrar" Then
                    If (EspecieSelected.IDEspecies <= 0) Then

                        If ListaEspeciesISINFungible.Where(Function(i) i.IDIsinFungible = EspeciesISINFungibleSelected.IDIsinFungible).Count > 0 Then
                            ListaEspeciesISINFungible.Remove(ListaEspeciesISINFungible.Where(Function(i) i.IDIsinFungible = EspeciesISINFungibleSelected.IDIsinFungible).First)
                        End If

                        If ListaEspeciesISINFungible.Count > 0 Then
                            EspeciesISINFungibleSelected = ListaEspeciesISINFungible.FirstOrDefault
                        Else
                            EspeciesISINFungibleSelected = Nothing
                        End If
                        MyBase.CambioItem("ListaEspeciesISINFungible")
                        MyBase.CambioItem("EspeciesISINFungibleSelected")
                    Else
                        dcProxy.EspeciesISINFungibles.Clear()
                        dcProxy.Load(dcProxy.EspeciesISINFungibleConsultarQuery(EspecieSelected.Id, String.Empty, EspecieSelected.TipoTasaFija, Program.Usuario, Program.HashConexion), AddressOf TerminoTraerEspeciesISINFungible, Nothing)
                    End If

                End If
            End If
        End If
        IsBusy = False
    End Sub

End Class

'Clase base para forma de búsquedas
Public Class CamposBusquedaEspecie

    Implements INotifyPropertyChanged

    '<StringLength(15, ErrorMessage:="La longitud máxima es de 15")> _
    ' <Display(Name:="Id")> _
    'Public Property Id As String

    Private _Id As String
    <Display(Name:="Nemotécnico")> _
    Public Property Id As String
        Get
            Return _Id
        End Get
        Set(ByVal value As String)
            _Id = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Id"))
        End Set
    End Property

    '<StringLength(50, ErrorMessage:="La longitud máxima es de 50")> _
    ' <Display(Name:="Nombre")> _
    'Public Property Nombre As String

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

    '<Display(Name:="IdEmisor")> _
    'Public Property plngIdEmisor As System.Nullable(Of Integer)

    Private _plngIdEmisor As Integer?
    <Display(Name:="Emisor")>
    Public Property plngIdEmisor As Integer?
        Get
            Return _plngIdEmisor
        End Get
        Set(ByVal value As Integer?)
            _plngIdEmisor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("plngIdEmisor"))
        End Set
    End Property

    'Private _IDAdmonEmision As Integer?
    '<Display(Name:="Admón Emisión")>
    'Public Property IDAdmonEmision As Integer?
    '    Get
    '        Return _IDAdmonEmision
    '    End Get
    '    Set(ByVal value As Integer?)
    '        _IDAdmonEmision = value
    '        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDAdmonEmision"))
    '    End Set
    'End Property

    Private _Emisor As String
    <Display(Name:=" ")>
    Public Property Emisor As String
        Get
            Return _Emisor
        End Get
        Set(ByVal value As String)
            _Emisor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Emisor"))
        End Set
    End Property


    '<Display(Name:="Clase")> _
    'Public Property plngIDClase As System.Nullable(Of Integer)

    Private _plngIDClase As Integer?
    <Display(Name:="Clase")>
    Public Property plngIDClase As Integer?
        Get
            Return _plngIDClase
        End Get
        Set(ByVal value As Integer?)
            _plngIDClase = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("plngIDClase"))
        End Set
    End Property

    Private _Clase As String
    <Display(Name:=" ")>
    Public Property Clase As String
        Get
            Return _Clase
        End Get
        Set(ByVal value As String)
            _Clase = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Clase"))
        End Set
    End Property

    '<Display(Name:="Activo")> _
    'Public Property logActivo As System.Nullable(Of Boolean)

    Private _logActivo As Boolean
    <Display(Name:="Estado")> _
    Public Property logActivo As Boolean
        Get
            Return _logActivo
        End Get
        Set(ByVal value As Boolean)
            _logActivo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("logActivo"))
        End Set
    End Property


    Private _pdtmEmision As Nullable(Of DateTime)
    <Display(Name:="pdtmEmision")> _
    Public Property pdtmEmision As Nullable(Of DateTime)
        Get
            Return _pdtmEmision
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _pdtmEmision = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("pdtmEmision"))
        End Set
    End Property

    Private _pdtmVencimiento As Nullable(Of DateTime)
    <Display(Name:="pdtmVencimiento")> _
    Public Property pdtmVencimiento() As Nullable(Of DateTime)
        Get
            Return _pdtmVencimiento
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            _pdtmVencimiento = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("pdtmVencimiento"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class


Public Class Emisores
    Implements INotifyPropertyChanged

    Private _IdEmisor As String
    <Display(Name:="IdEmisor")> _
    Public Property IdEmisor As String
        Get
            Return _IdEmisor
        End Get
        Set(ByVal value As String)
            _IdEmisor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IdEmisor"))
        End Set
    End Property

    Private _Emisor As String
    <Display(Name:=" ")> _
    Public Property Emisor As String
        Get
            Return _Emisor
        End Get
        Set(ByVal value As String)
            _Emisor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Emisor"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class


Public Class clase
    Implements INotifyPropertyChanged

    Private _IdClase As String
    <Display(Name:="IdClase")> _
    Public Property IdClase As String
        Get
            Return _IdClase
        End Get
        Set(ByVal value As String)
            _IdClase = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IdClase"))
        End Set
    End Property

    Private _Clase As String
    <Display(Name:=" ")> _
    Public Property Clase As String
        Get
            Return _Clase
        End Get
        Set(ByVal value As String)
            _Clase = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Clase"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class


Public Class OtrosEmisores
    Implements INotifyPropertyChanged

    Private _IDAdmonEmision As String
    <Display(Name:="IDAdmonEmision")> _
    Public Property IDAdmonEmision As String
        Get
            Return _IDAdmonEmision
        End Get
        Set(ByVal value As String)
            _IDAdmonEmision = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDAdmonEmision"))
        End Set
    End Property

    Private _NombreAdmonEmision As String
    <Display(Name:="NombreAdmonEmision")> _
    Public Property NombreAdmonEmision As String
        Get
            Return _NombreAdmonEmision
        End Get
        Set(ByVal value As String)
            _NombreAdmonEmision = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("NombreAdmonEmision"))
        End Set
    End Property

    Private _Emisores As String
    <Display(Name:="Emisores")> _
    Public Property Emisores As String
        Get
            Return _Emisores
        End Get
        Set(ByVal value As String)
            _Emisores = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Emisores"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class


Public Class claseinversion
    Implements INotifyPropertyChanged

    Private _strcodigo As String
    <Display(Name:="codigo")> _
    Public Property strcodigo As String
        Get
            Return _strcodigo
        End Get
        Set(ByVal value As String)
            _strcodigo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strcodigo"))
        End Set
    End Property

    Private _strDescripcion As String
    <Display(Name:="Clase Inversión")> _
    Public Property strDescripcion As String
        Get
            Return _strDescripcion
        End Get
        Set(ByVal value As String)
            _strDescripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("strDescripcion"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class