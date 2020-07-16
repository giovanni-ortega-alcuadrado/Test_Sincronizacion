Imports Telerik.Windows.Controls

Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports A2ControlMenu


Imports A2.OyD.OYDServer.RIA.Web
Imports A2.OyD.OYDServer.RIA.Web.OyDBolsa
Imports A2OYDBolsa
Imports System.Windows
Imports System.Collections
Imports System.Text
Imports System.IO
Imports System.Reflection
Imports System.Runtime.InteropServices.Automation
Imports System.Object
Imports System.Globalization

Imports A2Utilidades.Mensajes

Public Class FacturarLiquidacionesVBViewModel
    Inherits A2ControlMenu.A2ViewModel
    Dim dcProxy As BolsaDomainContext

    'Dim view As FacturarLiquidacionesVBView
    Public Const CSTR_NOMBREPROCESO_CSVREPORTE = "GeneracionCSVReportes"
    Private Shared SEPARATOR_FORMAT_CVS As String = System.Globalization.CultureInfo.CurrentCulture.Parent.TextInfo.ListSeparator
    'Dim objServicios As A2VisorReportes.A2.Visor.Servicios.GeneralesClient

    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New BolsaDomainContext()
            Else
                dcProxy = New BolsaDomainContext(New System.Uri((Program.RutaServicioNegocio)))
            End If
            DirectCast(dcProxy.DomainClient, WebDomainClient(Of BolsaDomainContext.IBolsaDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 50, 0)

            If IsNothing(Application.Current.Resources("A2VServicioParam")) Then
                A2Utilidades.Mensajes.mostrarMensaje("No existe el parámetro 'A2VServicioParam', por favor verfique el archivo XML de configuración.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IndexClientesEnvioCadena = "0"
            IndexClientesTipoPersona = "2"

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ReporteExcelTitulosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#Region "Variables"

#End Region

#Region "Propiedades"

    Private _IndexClientesEnvioCadena As String = "0"
    Public Property IndexClientesEnvioCadena As String
        Get
            Return _IndexClientesEnvioCadena
        End Get
        Set(ByVal value As String)
            _IndexClientesEnvioCadena = value
            MyBase.CambioItem("IndexClientesEnvioCadena")
        End Set
    End Property
    Private _IndexClientesTipoPersona As String = "2"
    Public Property IndexClientesTipoPersona As String
        Get
            Return _IndexClientesTipoPersona
        End Get
        Set(ByVal value As String)
            _IndexClientesTipoPersona = value
            MyBase.CambioItem("IndexClientesTipoPersona")
        End Set
    End Property


    Private _VisibilidadTipoDocumentoAP As Visibility = Visibility.Collapsed
    Public Property VisibilidadTipoDocumentoAP As Visibility
        Get
            Return _VisibilidadTipoDocumentoAP
        End Get
        Set(ByVal value As Visibility)
            _VisibilidadTipoDocumentoAP = value
            MyBase.CambioItem("VisibilidadTipoDocumentoAP")
        End Set
    End Property

    Private _VisibilidadBotonEnviarCadena As Visibility = Visibility.Collapsed
    Public Property VisibilidadBotonEnviarCadena As Visibility
        Get
            Return _VisibilidadBotonEnviarCadena
        End Get
        Set(ByVal value As Visibility)
            _VisibilidadBotonEnviarCadena = value
            MyBase.CambioItem("VisibilidadBotonEnviarCadena")
        End Set
    End Property


    Private _ClientesEnvioCadena As String
    <Display(Name:="ClientesEnvioCadena")> _
    Public Property ClientesEnvioCadena As String
        Get
            Return _ClientesEnvioCadena
        End Get
        Set(ByVal value As String)
            _ClientesEnvioCadena = value

            If _ClientesEnvioCadena = "1" Then
                VisibilidadTipoDocumentoAP = Visibility.Collapsed
                VisibilidadBotonEnviarCadena = Visibility.Visible
            Else
                VisibilidadTipoDocumentoAP = Visibility.Visible
                VisibilidadBotonEnviarCadena = Visibility.Collapsed
            End If


            MyBase.CambioItem("ClientesEnvioCadena")
        End Set
    End Property

    Private _IDComitenteDesde As String = "                0"
    <Display(Name:="Comitente Desde")> _
    Public Property IDComitenteDesde As String
        Get
            Return _IDComitenteDesde
        End Get
        Set(ByVal value As String)
            _IDComitenteDesde = value
            MyBase.CambioItem("IDComitenteDesde")
        End Set
    End Property

    Private _IDComitenteHasta As String = "    9999999999999"
    <Display(Name:="Comitente Hasta")> _
    Public Property IDComitenteHasta As String
        Get
            Return _IDComitenteHasta
        End Get
        Set(ByVal value As String)
            _IDComitenteHasta = value
            MyBase.CambioItem("IDComitenteHasta")

        End Set
    End Property

    Private _TIPODOCUMENTOAPT As String
    <Display(Name:="Tipo Documento APT")> _
    Public Property TIPODOCUMENTOAPT As String
        Get
            Return _TIPODOCUMENTOAPT
        End Get
        Set(ByVal value As String)
            _TIPODOCUMENTOAPT = value
            MyBase.CambioItem("TIPODOCUMENTOAPT")

        End Set
    End Property

    Private _SucursalesTodos As String
    <Display(Name:="SucursalesTodos")> _
    Public Property SucursalesTodos As String
        Get
            Return _SucursalesTodos
        End Get
        Set(ByVal value As String)
            _SucursalesTodos = value
            MyBase.CambioItem("SucursalesTodos")

        End Set
    End Property

    Private _SeleccionarTodos As Boolean = False
    Public Property SeleccionarTodos As Boolean
        Get
            Return _SeleccionarTodos
        End Get
        Set(ByVal value As Boolean)
            _SeleccionarTodos = value
            If Not IsNothing(ListaClientesAPT) Then
                For Each li In ListaClientesAPT
                    li.Seleccionado = _SeleccionarTodos
                Next
            End If
            MyBase.CambioItem("SeleccionarTodos")
        End Set
    End Property


    Private _ListaClientesAPT As List(Of OyDBolsa.tblPlanoPapeletasBolsa) = New List(Of OyDBolsa.tblPlanoPapeletasBolsa)
    Public Property ListaClientesAPT As List(Of OyDBolsa.tblPlanoPapeletasBolsa)
        Get
            Return _ListaClientesAPT
        End Get
        Set(ByVal value As List(Of OyDBolsa.tblPlanoPapeletasBolsa))
            _ListaClientesAPT = value
            MyBase.CambioItem("ListaClientesAPT")
        End Set
    End Property

    Private _chkVIP As Boolean = True
    Public Property chkVIP As Boolean
        Get
            Return _chkVIP
        End Get
        Set(ByVal value As Boolean)
            _chkVIP = value
            MyBase.CambioItem("chkVIP")
        End Set
    End Property

    Private _chkCompleta As Boolean = True
    Public Property chkCompleta As Boolean
        Get
            Return _chkCompleta
        End Get
        Set(ByVal value As Boolean)
            _chkCompleta = value
            MyBase.CambioItem("chkCompleta")
        End Set
    End Property

    Private _chkParcial As Boolean = True
    Public Property chkParcial As Boolean
        Get
            Return _chkParcial
        End Get
        Set(ByVal value As Boolean)
            _chkParcial = value
            MyBase.CambioItem("chkParcial")
        End Set
    End Property

    Private _chkNinguna As Boolean = True
    Public Property chkNinguna As Boolean
        Get
            Return _chkNinguna
        End Get
        Set(ByVal value As Boolean)
            _chkNinguna = value
            MyBase.CambioItem("chkNinguna")
        End Set
    End Property

#End Region

#Region "Métodos"

    Public Sub LimpiarPantalla()

    End Sub

    Public Sub ConsultaClientes_APT()
        IsBusy = True
        dcProxy.tblPlanoPapeletasBolsas.Clear()
        dcProxy.Load(dcProxy.ConsultaClientes_APT_PapeletasQuery(IDComitenteDesde, IDComitenteHasta, TIPODOCUMENTOAPT, Program.Usuario, Program.HashConexion), AddressOf TerminotraerClientesPapeletasAPT, Nothing)
    End Sub

    Public Sub ConsultarReportefacturas()
        Dim strParametros As String = String.Empty
        Dim strTipoLiquidacion As String = "T"
        Dim ClaseLiquidacion As String = "T"
        Dim strMercado As String = "T"
        Dim strSegmentacion As String = String.Empty
        Try

            If SucursalesTodos < -1 Or IsNothing(SucursalesTodos) Then

                IsBusy = False
                A2Utilidades.Mensajes.mostrarMensaje("La sucursal es un dato requerido ", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub

            End If

            strSegmentacion = IIf(chkParcial, "1", "0")
            strSegmentacion = strSegmentacion & IIf(chkCompleta, "1", "0")
            strSegmentacion = strSegmentacion & "1"
            strSegmentacion = strSegmentacion & IIf(chkVIP, "V", "0")

            strParametros = "&plngIDComisionista=" & 10 & "&plngIDSucComisionista=" & 1 & "&pTipoLiquidacion=" & strTipoLiquidacion & "&pClaseLiquidacion=" & ClaseLiquidacion & _
                            "&pstrUsuario=" & Program.Usuario & "&pstrClienteInicio=" & IDComitenteDesde & "&pstrClienteFinal=" & IDComitenteHasta & _
                            "&pstrMercado=" & strMercado & "&plngSucursal=" & SucursalesTodos & "&pstrSegmentacion=" & strSegmentacion


            General.MostrarReporte(strParametros, "Facturas", "rptFacturacionSeleccionado")
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en ejecutar el reporte de facturas", _
                                             Me.ToString(), "Ejecutar_Reporte", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub EnviarCadena()
        Try
            If IndexClientesEnvioCadena = "0" Then

                mostrarMensajePregunta("Se va exportar las operaciones de clientes a Cadena", _
                                       Program.TituloSistema, _
                                       "ENVIARCADENA", _
                                       AddressOf TerminoMensajePregunta, False)
            Else

                mostrarMensajePregunta("Se va exportar las operaciones de clientes a Cadena APT", _
                           Program.TituloSistema, _
                           "ENVIARCADENAPAPELETAAPT", _
                           AddressOf TerminoMensajePreguntaPapeletaAPT, False)

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema enviando a cadena", _
                                             Me.ToString(), "Envió_Cadena", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    'Cuando se exporta las papeletas de APT a cadena
    Private Sub TerminoMensajePreguntaPapeletaAPT(ByVal sender As Object, ByVal e As EventArgs)

        Dim strLiquidacionesAPT As String = "" 'cuando se selecciona una peración del grid
        Dim strClientesTodosAPT As String = "" 'Cuando se selecciona todos los clienes del Grid

        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        Try

            If objResultado.DialogResult Then
                If objResultado.CodigoLlamado = "ENVIARCADENAPAPELETAAPT" Then
                    IsBusy = True
                    dcProxy.TmpFacturas_EnvioCadenas.Clear()

                    If SeleccionarTodos = True Then
                        For Each li In ListaClientesAPT
                            If li.Seleccionado Then
                                If strClientesTodosAPT = "" Then
                                    strClientesTodosAPT = li.Cliente & "," & li.NroLiq
                                Else
                                    strClientesTodosAPT = strClientesTodosAPT & "|" & li.Cliente & "," & li.NroLiq
                                End If
                            End If
                        Next
                        If Not IsNothing(dcProxy.tbltmpPlanoPapeletas) Then
                            dcProxy.tbltmpPlanoPapeletas.Clear()
                        End If
                        dcProxy.Load(dcProxy.generarCadenaPapeletasQuery("", "", 1, strClientesTodosAPT, Program.Usuario, Program.HashConexion), AddressOf TerminotraerCadenaPapeletas, Nothing)

                    Else
                        For Each li In ListaClientesAPT
                            If li.Seleccionado Then
                                If strLiquidacionesAPT = "" Then
                                    strLiquidacionesAPT = li.Cliente & "," & li.NroLiq
                                Else
                                    strLiquidacionesAPT = strLiquidacionesAPT & "|" & li.Cliente & "," & li.NroLiq
                                End If
                            End If
                        Next
                        If Not IsNothing(dcProxy.tbltmpPlanoPapeletas) Then
                            dcProxy.tbltmpPlanoPapeletas.Clear()
                        End If
                        dcProxy.Load(dcProxy.generarCadenaPapeletasQuery("", "", 1, strLiquidacionesAPT, Program.Usuario, Program.HashConexion), AddressOf TerminotraerCadenaPapeletas, Nothing)
                    End If
                End If
            Else
                If objResultado.CodigoLlamado = "ENVIARCADENAPAPELETAAPT" Then
                    mostrarMensaje("se canceló el envío a cadena APT Papeletas", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema enviando a cadena", _
                                             Me.ToString(), "TerminoMensajePreguntaPapeletaAPT", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoMensajePregunta(ByVal sender As Object, ByVal e As EventArgs)

        Dim objResultado As A2Utilidades.wppMensajePregunta = CType(sender, A2Utilidades.wppMensajePregunta)
        Try
            If objResultado.DialogResult Then
                If objResultado.CodigoLlamado = "ENVIARCADENA" Then
                    IsBusy = True
                    dcProxy.TmpFacturas_EnvioCadenas.Clear()
                    'dcProxy.Load(dcProxy.generarCadenaFacturasQuery(0, 0, IDComitenteDesde, IDComitenteHasta, "T", -1, Program.Usuario, Program.HashConexion), AddressOf TerminotraerCadenaFacturas, Nothing)
                    dcProxy.Load(dcProxy.generarCadenaPapeletasQuery(IDComitenteDesde, IDComitenteHasta, 0, "", Program.Usuario, Program.HashConexion), AddressOf TerminotraerCadenaPapeletas, Nothing)
                End If
            Else
                If objResultado.CodigoLlamado = "ENVIARCADENA" Then
                    mostrarMensaje("se canceló el envío a cadena", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema enviando a cadena", _
                                             Me.ToString(), "TerminoMensajePregunta", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminotraerClientesPapeletasAPT(ByVal lo As LoadOperation(Of OyDBolsa.tblPlanoPapeletasBolsa))
        Dim PapeletasAPT As String = ""

        IsBusy = False
        Try
            If Not lo.HasError Then
                ListaClientesAPT = dcProxy.tblPlanoPapeletasBolsas.ToList
                VisibilidadBotonEnviarCadena = Visibility.Visible

                If ListaClientesAPT.Count = 0 Then
                    mostrarMensaje("No hay datos de clientes APT", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de la lista de clientes papeletas APT", _
                                                 Me.ToString(), "TerminotraerClientesPapeletasAPT", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema enviando a cadena APT", _
                                             Me.ToString(), "TerminotraerClientesPapeletasAPT", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminotraerCadenaFacturas(ByVal lo As LoadOperation(Of OyDBolsa.TmpFacturas_EnvioCadena))
        IsBusy = False
        Try
            If Not lo.HasError Then
                If dcProxy.TmpFacturas_EnvioCadenas.Count > 0 Then
                    If dcProxy.TmpFacturas_EnvioCadenas.First.logExitoso Then
                        IsBusy = True
                        mostrarMensaje(dcProxy.TmpFacturas_EnvioCadenas.First.strMensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        If Not IsNothing(dcProxy.tbltmpPlanoPapeletas) Then
                            dcProxy.tbltmpPlanoPapeletas.Clear()
                        End If
                        dcProxy.Load(dcProxy.generarCadenaPapeletasQuery(IDComitenteDesde, IDComitenteHasta, 0, "", Program.Usuario, Program.HashConexion), AddressOf TerminotraerCadenaPapeletas, Nothing)
                    Else
                        mostrarMensaje(dcProxy.TmpFacturas_EnvioCadenas.First.strMensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                        IsBusy = True
                        If Not IsNothing(dcProxy.tbltmpPlanoPapeletas) Then
                            dcProxy.tbltmpPlanoPapeletas.Clear()
                        End If
                        dcProxy.Load(dcProxy.generarCadenaPapeletasQuery(IDComitenteDesde, IDComitenteHasta, 0, "", Program.Usuario, Program.HashConexion), AddressOf TerminotraerCadenaPapeletas, Nothing)
                    End If
                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la generación de envia de cadena", _
                                                 Me.ToString(), "TerminotraerCadenaFacturas", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema enviando a cadena Facturas", _
                                             Me.ToString(), "TerminotraerCadenaFacturas", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminotraerCadenaPapeletas(ByVal lo As LoadOperation(Of OyDBolsa.tbltmpPlanoPapeletas))
        Try
            IsBusy = False
            If Not lo.HasError Then

                If dcProxy.tbltmpPlanoPapeletas.Count > 0 Then
                    If dcProxy.tbltmpPlanoPapeletas.First.logExitoso Then
                        ' IsBusy = True
                        mostrarMensaje(dcProxy.tbltmpPlanoPapeletas.First.strMensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    Else
                        mostrarMensaje(dcProxy.tbltmpPlanoPapeletas.First.strMensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                    End If
                End If

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la generación de envia de cadena", _
                                                 Me.ToString(), "TerminotraerClientesPapeletasAPT", Application.Current.ToString(), Program.Maquina, lo.Error)
                lo.MarkErrorAsHandled()   '????
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema enviando a cadena papeletas", _
                                             Me.ToString(), "TerminotraerCadenaPapeletas", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

#End Region

End Class
