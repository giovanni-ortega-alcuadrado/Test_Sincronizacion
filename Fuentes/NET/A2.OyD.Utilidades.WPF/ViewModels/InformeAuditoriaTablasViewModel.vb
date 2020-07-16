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
Imports A2.OyD.OYDServer.RIA.Web.OyDTitulos
Imports A2ComunesImportaciones
Imports System.Windows
Imports System.Collections
Imports System.Text
Imports System.IO
Imports System.Reflection
Imports System.Runtime.InteropServices.Automation
Imports System.Object
Imports System.Globalization

Public Class InformeAuditoriaTablasViewModel
    Inherits A2ControlMenu.A2ViewModel
    Dim dcProxy As UtilidadesDomainContext
    Dim view As InformeAuditoriaTablas
    Private Shared SEPARATOR_FORMAT_CVS As String = System.Globalization.CultureInfo.CurrentCulture.Parent.TextInfo.ListSeparator
    Public Const CSTR_NOMBREPROCESO_AUDITORIA_TABLAS As String = "AUDITORIATABLAS"
    Public Const CSTR_NOMBREPROCESO_CSVREPORTE = "GeneracionCSVReportes"
    Dim objServicios As A2VisorReportes.A2.Visor.Servicios.GeneralesClient


    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New UtilidadesDomainContext
            Else
                dcProxy = New UtilidadesDomainContext(New System.Uri((Program.RutaServicioUtilidadesOYD)))
            End If
            DirectCast(dcProxy.DomainClient, WebDomainClient(Of A2.OyD.OYDServer.RIA.Web.UtilidadesDomainContext.IUtilidadesDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 0, 600)

            'SLB20140528 Manejo del TimeOut a traves de un parametro de la aplicación.
            Dim TimeOutMinutos As Byte = 15
            If Not String.IsNullOrEmpty(Program.Consultar_TIME_OUT_REPORTES_EN_MINUTOS) Then
                TimeOutMinutos = CInt(Program.Consultar_TIME_OUT_REPORTES_EN_MINUTOS)
            End If

            clsEstablecerComunicacionVisorReportes.ServicioGenerales(objServicios, Application.Current.Resources("A2VServicioParam").ToString())
            'AddHandler objServicios.GenerarArchivoCompleted, AddressOf TerminoGenerarArchivo

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "Public Class InformeAuditoriaTablasViewModel.New ", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub


#Region "Resultados Asincrónicos"

    Private Sub TerminoTraerTitulos(ByVal lo As InvokeOperation(Of System.Nullable(Of Boolean)))

        Try
            If Not lo.HasError Then
                'Dim grid As New DataGrid
                'grid.AutoGenerateColumns = False
                'grid.HeadersVisibility = DataGridHeadersVisibility.All
                'ListaTitulos = dcProxy.ReporteExcelTitulos
                'CrearColumnasTitulos(grid)
                'Dim strMensaje As String = lo.UserState
                'exportDataGrid(grid, strMensaje.ToUpper())

                TerminoCrearArchivo()

            Else
                A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al consultar los datos para el reporte titulos", _
                                 Me.ToString(), "TerminoTraerTitulos", Application.Current.ToString(), Program.Maquina, lo.Error)
                IsBusy = False
            End If
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al consultar los datos para el reporte titulos", _
                                 Me.ToString(), "TerminoTraerTitulos", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    Private Sub TerminoCrearArchivo()
        Try
            Dim cwCar As New ListarArchivosDirectorioView(CSTR_NOMBREPROCESO_CSVREPORTE) '(CSTR_NOMBREPROCESO_AUDITORIA_TABLAS)
            Program.Modal_OwnerMainWindowsPrincipal(cwCar)
            cwCar.ShowDialog()
            IsBusy = False
        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al levantar la ventana de visualización de los archivos", _
                                 Me.ToString(), "TerminoCrearArchivo", Application.Current.ToString(), Program.Maquina, ex)

        End Try
    End Sub

    'Tablas padres

#End Region

#Region "Propiedades"


    Private _AbrirArchivo As Stream
    Public Property AbrirArchivo() As Stream
        Get
            Return _AbrirArchivo
        End Get
        Set(ByVal value As Stream)
            _AbrirArchivo = value
        End Set
    End Property


    'Private _ListaTitulos As EntitySet(Of ReporteExcelTitulo)
    'Public Property ListaTitulos() As EntitySet(Of ReporteExcelTitulo)
    '    Get
    '        Return _ListaTitulos
    '    End Get
    '    Set(ByVal value As EntitySet(Of ReporteExcelTitulo))
    '        _ListaTitulos = value
    '    End Set
    'End Property

    Private _TipoBloqueoVisible As Visibility = Visibility.Collapsed
    Public Property TipoBloqueoVisible() As Visibility
        Get
            Return _TipoBloqueoVisible
        End Get
        Set(ByVal value As Visibility)
            _TipoBloqueoVisible = value
        End Set
    End Property


    Private _cb As CamposInformeAuditoria = New CamposInformeAuditoria
    Public Property cb() As CamposInformeAuditoria
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposInformeAuditoria)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property


#End Region

#Region "Métodos"

    Public Sub Limpiar()
        Try
            cb.Tabla = "(Todos)"
            cb.FechaDesde = Date.Now.Date
            cb.FechaHasta = Date.Now.Date
            cb.FiltroConsulta = String.Empty
            cb.FiltroDatos = String.Empty

            MyBase.CambioItem("cb")

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al restaurar los valores por defecto", _
                                 Me.ToString(), "InformeAuditoriaTablasViewModel.Limpiar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Async Sub EjecutarConsulta()
        Try
            'IsBusy = True
            'dcProxy.DatosInforme(cb.FechaDesde, cb.FechaHasta, cb.Tabla, cb.FiltroConsulta, cb.FiltroDatos, Program.Usuario, Program.HashConexion, AddressOf TerminoTraerTitulos, "csv")

            'SLB20131108 Se genera el informe desde el Visor de Reportes
            Dim strParametros As String

            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
            System.Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo("en-US")

            'CORREC_CITI_SV_2014
            Dim dtmfechaDesde As Date = Date.Parse(cb.FechaDesde, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces)
            Dim strfechaDesde As String = dtmfechaDesde.ToString("MM/dd/yyyy HH:mm:ss")

            Dim dtmfechaHasta As Date = Date.Parse(cb.FechaHasta, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces)
            Dim strfechaHasta As String = dtmfechaHasta.ToString("MM/dd/yyyy HH:mm:ss")

            strParametros = String.Format("'@pdtmFechainicial'**'{0}'**'DATETIME'|", strfechaDesde) 'CORREC_CITI_SV_2014
            strParametros = strParametros & String.Format("'@pdtmFechafin'**'{0}'**'DATETIME'|", strfechaHasta) 'CORREC_CITI_SV_2014
            strParametros = strParametros & String.Format("'@pstrTabla'**'{0}'**'STRING'|", cb.Tabla)
            strParametros = strParametros & String.Format("'@pstrFiltroClave'**'{0}'**'STRING'|", cb.FiltroConsulta)
            strParametros = strParametros & String.Format("'@pstrFiltroContenido'**'{0}'**'STRING'|", cb.FiltroDatos)
            strParametros = strParametros & String.Format("'@pstrUsuario'**'{0}'**'STRING'", Program.Usuario)

            IsBusy = True
            Dim objRespuestaServicio = Await objServicios.GenerarArchivoAsync("Informe Auditoría", Program.Usuario, Application.Current.Resources("A2RutaFisicaGeneracion").ToString, strParametros, "Usp_AuditoriaInforme_OyDNet", Program.Maquina, A2VisorReportes.A2.Visor.Servicios.TipoArchivo.EXCEL.EXCEL, "Informe Auditoría", Program.HashConexion("Informe Auditoría"))
            TerminoGenerarArchivo(objRespuestaServicio)

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al ejecutar la consulta.",
                                 Me.ToString(), "InformeAuditoriaTablasViewModel.EjecutarConsulta", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TerminoGenerarArchivo(ByVal e As A2VisorReportes.A2.Visor.Servicios.RespuestaArchivo)
        Try
            IsBusy = False
            If e.EjecucionExitosa Then
                TerminoCrearArchivo()
            Else
                Dim ex1 As New Exception(e.Mensaje)
                A2Utilidades.Mensajes.mostrarMensaje("Se presentó un error en el momento de generar el archivo plano." & ex1.ToString, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error en el momento de generar el archivo plano.",
                                 Me.ToString(), "TerminoGenerarArchivoPlano", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Sub HabilitarTipoBloqueo()
        'Try
        '    If cb.Estado = "B" Then
        '        TipoBloqueoVisible = Visibility.Visible
        '    Else
        '        TipoBloqueoVisible = Visibility.Collapsed
        '        cb.TipoBloqueo = "T"
        '    End If
        '    MyBase.CambioItem("cb")
        '    MyBase.CambioItem("TipoBloqueo")
        '    MyBase.CambioItem("TipoBloqueoVisible")
        'Catch ex As Exception
        '    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al habilitar el Campo Tipo Bloqueo.", _
        '                         Me.ToString(), "InformeAuditoriaTablasViewModel.HabilitarTipoBloqueo", Application.Current.ToString(), Program.Maquina, ex)
        'End Try
    End Sub

    'Private Sub CrearColumnasTitulos(ByVal pgrid As DataGrid)
    '    Dim FechaCorte As New DataGridTextColumn
    '    FechaCorte.Header = "Fecha de Corte"
    '    pgrid.Columns.Add(FechaCorte)

    '    Dim CodigoCliente As New DataGridTextColumn
    '    CodigoCliente.Header = "Codigo Cliente"
    '    pgrid.Columns.Add(CodigoCliente)

    '    Dim NombreCliente As New DataGridTextColumn
    '    NombreCliente.Header = "Nombre Cliente"
    '    pgrid.Columns.Add(NombreCliente)

    '    Dim NemotecnicoEspecie As New DataGridTextColumn
    '    NemotecnicoEspecie.Header = "Nemotecnico Especie"
    '    pgrid.Columns.Add(NemotecnicoEspecie)

    '    Dim NombreEspecie As New DataGridTextColumn
    '    NombreEspecie.Header = "Nombre Especie"
    '    pgrid.Columns.Add(NombreEspecie)

    '    Dim ValorNomina As New DataGridTextColumn
    '    ValorNomina.Header = "Valor Nominal Titulo"
    '    pgrid.Columns.Add(ValorNomina)

    '    Dim NroRecibo As New DataGridTextColumn
    '    NroRecibo.Header = "Nro Recibo Custodia"
    '    pgrid.Columns.Add(NroRecibo)

    '    Dim NroFila As New DataGridTextColumn
    '    NroFila.Header = "Nro Fila Custodia"
    '    pgrid.Columns.Add(NroFila)

    '    Dim CodigoDeposito As New DataGridTextColumn
    '    CodigoDeposito.Header = "Codigo Deposito Valores"
    '    pgrid.Columns.Add(CodigoDeposito)

    '    Dim NombreDeposito As New DataGridTextColumn
    '    NombreDeposito.Header = "Nombre Deposito de Valores"
    '    pgrid.Columns.Add(NombreDeposito)

    '    Dim ISIN As New DataGridTextColumn
    '    ISIN.Header = "ISIN"
    '    pgrid.Columns.Add(ISIN)

    '    Dim CuentaDeposito As New DataGridTextColumn
    '    CuentaDeposito.Header = "Cuenta Deposito"
    '    pgrid.Columns.Add(CuentaDeposito)

    '    Dim ConceptoBloqueo As New DataGridTextColumn
    '    ConceptoBloqueo.Header = "Concepto Bloqueo"
    '    pgrid.Columns.Add(ConceptoBloqueo)

    '    Dim CodigoSucursal As New DataGridTextColumn
    '    CodigoSucursal.Header = "Codigo Sucursal Receptor"
    '    pgrid.Columns.Add(CodigoSucursal)

    '    Dim NombreSucursal As New DataGridTextColumn
    '    NombreSucursal.Header = "Nombre Sucursal Receptor"
    '    pgrid.Columns.Add(NombreSucursal)

    '    Dim CodigoReceptor As New DataGridTextColumn
    '    CodigoReceptor.Header = "Codigo Receptor Lider Cliente"
    '    pgrid.Columns.Add(CodigoReceptor)

    '    Dim NombreReceptor As New DataGridTextColumn
    '    NombreReceptor.Header = "Nombre Receptor Lider Cliente"
    '    pgrid.Columns.Add(NombreReceptor)

    '    Dim NroLiquidacion As New DataGridTextColumn
    '    NroLiquidacion.Header = "Nro Liquidacion"
    '    pgrid.Columns.Add(NroLiquidacion)

    '    Dim NroParcial As New DataGridTextColumn
    '    NroParcial.Header = "Nro Parcial"
    '    pgrid.Columns.Add(NroParcial)

    '    Dim FechaElaboracion As New DataGridTextColumn
    '    FechaElaboracion.Header = "Fecha Elaboracion Liquidacion"
    '    pgrid.Columns.Add(FechaElaboracion)

    '    Dim TipoLiquidacion As New DataGridTextColumn
    '    TipoLiquidacion.Header = "Tipo Liquidacion"
    '    pgrid.Columns.Add(TipoLiquidacion)

    '    Dim ClaseLiquidacion As New DataGridTextColumn
    '    ClaseLiquidacion.Header = "Clase Liquidacion"
    '    pgrid.Columns.Add(ClaseLiquidacion)

    '    Dim Periodicidad As New DataGridTextColumn
    '    Periodicidad.Header = "Periodicidad"
    '    pgrid.Columns.Add(Periodicidad)

    '    Dim TasaEmision As New DataGridTextColumn
    '    TasaEmision.Header = "Tasa Emision"
    '    pgrid.Columns.Add(TasaEmision)

    '    Dim IndicadorEconomico As New DataGridTextColumn
    '    IndicadorEconomico.Header = "Indicador Economico"
    '    pgrid.Columns.Add(IndicadorEconomico)

    '    Dim Puntos As New DataGridTextColumn
    '    Puntos.Header = "Puntos indicador"
    '    pgrid.Columns.Add(Puntos)

    '    Dim Emision As New DataGridTextColumn
    '    Emision.Header = "Fecha Emision"
    '    pgrid.Columns.Add(Emision)

    '    Dim Vencimiento As New DataGridTextColumn
    '    Vencimiento.Header = "Fecha Vencimiento"
    '    pgrid.Columns.Add(Vencimiento)

    '    Dim VPNMercado As New DataGridTextColumn
    '    VPNMercado.Header = "VPN_Mercado_Alianz"
    '    pgrid.Columns.Add(VPNMercado)

    '    Dim VPNMercadoTotal As New DataGridTextColumn
    '    VPNMercadoTotal.Header = "Total (VPN  X  V/r nominal)"
    '    pgrid.Columns.Add(VPNMercadoTotal)

    '    Dim NroTitulo As New DataGridTextColumn
    '    NroTitulo.Header = "NroTitulo"
    '    pgrid.Columns.Add(NroTitulo)

    '    Dim TIRVPN As New DataGridTextColumn
    '    TIRVPN.Header = "TIRVPN"
    '    pgrid.Columns.Add(TIRVPN)

    '    Dim VlrLineal As New DataGridTextColumn
    '    VlrLineal.Header = "VlrLineal"
    '    pgrid.Columns.Add(VlrLineal)

    '    Dim TIROriginal As New DataGridTextColumn
    '    TIROriginal.Header = "TIROriginal"
    '    pgrid.Columns.Add(TIROriginal)

    '    Dim TIRActual As New DataGridTextColumn
    '    TIRActual.Header = "TIRActual"
    '    pgrid.Columns.Add(TIRActual)

    '    Dim Spread As New DataGridTextColumn
    '    Spread.Header = "Spread"
    '    pgrid.Columns.Add(Spread)

    '    Dim TIRSpread As New DataGridTextColumn
    '    TIRSpread.Header = "TIRSpread"
    '    pgrid.Columns.Add(TIRSpread)

    '    Dim ValorValoracionOyD As New DataGridTextColumn
    '    ValorValoracionOyD.Header = "Valor Valoracion OyD"
    '    pgrid.Columns.Add(ValorValoracionOyD)

    '    Dim Recibo As New DataGridTextColumn
    '    Recibo.Header = "Fecha Recibo"
    '    pgrid.Columns.Add(Recibo)

    '    Dim Elaboracion As New DataGridTextColumn
    '    Elaboracion.Header = "Fecha Elaboracion"
    '    pgrid.Columns.Add(Elaboracion)

    '    Dim NroDocumento As New DataGridTextColumn
    '    NroDocumento.Header = "Nro Documento"
    '    pgrid.Columns.Add(NroDocumento)

    '    Dim TasaCompra As New DataGridTextColumn
    '    TasaCompra.Header = "Tasa Compra"
    '    pgrid.Columns.Add(TasaCompra)

    '    Dim TasaEfectiva As New DataGridTextColumn
    '    TasaEfectiva.Header = "TasaEfectiva"
    '    pgrid.Columns.Add(TasaEfectiva)

    '    Dim Precio As New DataGridTextColumn
    '    Precio.Header = "Precio"
    '    pgrid.Columns.Add(Precio)

    '    Dim Liquidacion As New DataGridTextColumn
    '    Liquidacion.Header = "Liquidacion"
    '    pgrid.Columns.Add(Liquidacion)

    '    Dim Transaccion As New DataGridTextColumn
    '    Transaccion.Header = "Transaccion"
    '    pgrid.Columns.Add(Transaccion)

    '    Dim tipoDeOferta As New DataGridTextColumn
    '    tipoDeOferta.Header = "Tipo de Oferta"
    '    pgrid.Columns.Add(tipoDeOferta)

    '    Dim DescripcionTipoDeOferta As New DataGridTextColumn
    '    DescripcionTipoDeOferta.Header = "Descripcion Tipo de Oferta"
    '    pgrid.Columns.Add(DescripcionTipoDeOferta)

    '    Dim NombreIndicador As New DataGridTextColumn
    '    NombreIndicador.Header = "Nombre Indicador"
    '    pgrid.Columns.Add(NombreIndicador)


    'End Sub

#Region "[Export Methods, source = http://www.codeproject.com/KB/silverlight/SilverlightDataGridExport.aspx?msg=3476945]        Public Shared Sub exportDataGrid(dGrid As DataGrid)"

    'Private Sub exportDataGrid(ByVal dGrid As DataGrid, ByVal strFormat As String)

    '    Dim strBuilder As New StringBuilder()
    '    Dim strLineas As New List(Of String)

    '    If IsNothing(dGrid) Then Return
    '    Dim lstFields As List(Of String) = New List(Of String)()
    '    If dGrid.HeadersVisibility = DataGridHeadersVisibility.Column OrElse dGrid.HeadersVisibility = DataGridHeadersVisibility.All Then
    '        For Each dgcol As DataGridColumn In dGrid.Columns
    '            lstFields.Add(formatField(dgcol.Header.ToString(), strFormat, True))
    '        Next
    '        buildStringOfRow(strBuilder, lstFields, strFormat)
    '        strLineas.Add(strBuilder.ToString())
    '    End If
    '    dGrid.ItemsSource = ListaTitulos

    '    For a = 0 To ListaTitulos.Count - 1
    '        lstFields.Clear()
    '        strBuilder.Clear()
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).FechaCorte), String.Empty, ListaTitulos(a).FechaCorte), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).CodigoCliente), String.Empty, ListaTitulos(a).CodigoCliente), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).NombreCliente), String.Empty, ListaTitulos(a).NombreCliente), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).NemotecnicoEspecie), String.Empty, ListaTitulos(a).NemotecnicoEspecie), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).NombreEspecie), String.Empty, ListaTitulos(a).NombreEspecie), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).ValorNomina), String.Empty, ListaTitulos(a).ValorNomina), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).NroRecibo), String.Empty, ListaTitulos(a).NroRecibo), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).NroFila), String.Empty, ListaTitulos(a).NroFila), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).CodigoDeposito), String.Empty, ListaTitulos(a).CodigoDeposito), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).NombreDeposito), String.Empty, ListaTitulos(a).NombreDeposito), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).ISIN), String.Empty, ListaTitulos(a).ISIN), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).CuentaDeposito), String.Empty, ListaTitulos(a).CuentaDeposito), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).ConceptoBloqueo), String.Empty, ListaTitulos(a).ConceptoBloqueo), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).CodigoSucursal), String.Empty, ListaTitulos(a).CodigoSucursal), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).NombreSucursal), String.Empty, ListaTitulos(a).NombreSucursal), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).CodigoReceptor), String.Empty, ListaTitulos(a).CodigoReceptor), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).CodigoReceptor), String.Empty, ListaTitulos(a).CodigoReceptor), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).NombreReceptor), String.Empty, ListaTitulos(a).NombreReceptor), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).NroLiquidacion), String.Empty, ListaTitulos(a).NroLiquidacion), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).NroParcial), String.Empty, ListaTitulos(a).NroParcial), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).FechaElaboracion), String.Empty, ListaTitulos(a).FechaElaboracion), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).TipoLiquidacion), String.Empty, ListaTitulos(a).TipoLiquidacion), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).ClaseLiquidacion), String.Empty, ListaTitulos(a).ClaseLiquidacion), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).Periodicidad), String.Empty, ListaTitulos(a).Periodicidad), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).TasaEmision), String.Empty, ListaTitulos(a).TasaEmision), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).IndicadorEconomico), String.Empty, ListaTitulos(a).IndicadorEconomico), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).Puntos), String.Empty, ListaTitulos(a).Puntos), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).Emision), String.Empty, ListaTitulos(a).Emision), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).Vencimiento), String.Empty, ListaTitulos(a).Vencimiento), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).VPNMercado), String.Empty, ListaTitulos(a).VPNMercado), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).VPNMercadoTotal), String.Empty, ListaTitulos(a).VPNMercadoTotal), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).NroTitulo), String.Empty, ListaTitulos(a).NroTitulo), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).TIRVPN), String.Empty, ListaTitulos(a).TIRVPN), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).VlrLineal), String.Empty, ListaTitulos(a).VlrLineal), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).TIROriginal), String.Empty, ListaTitulos(a).TIROriginal), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).TIRActual), String.Empty, ListaTitulos(a).TIRActual), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).Spread), String.Empty, ListaTitulos(a).Spread), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).TIRSpread), String.Empty, ListaTitulos(a).TIRSpread), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).ValorValoracionOyD), String.Empty, ListaTitulos(a).ValorValoracionOyD), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).Recibo), String.Empty, ListaTitulos(a).Recibo), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).Elaboracion), String.Empty, ListaTitulos(a).Elaboracion), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).NroDocumento), String.Empty, ListaTitulos(a).NroDocumento), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).TasaCompra), String.Empty, ListaTitulos(a).TasaCompra), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).TasaEfectiva), String.Empty, ListaTitulos(a).TasaEfectiva), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).Precio), String.Empty, ListaTitulos(a).Precio), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).Liquidacion), String.Empty, ListaTitulos(a).Liquidacion), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).Transaccion), String.Empty, ListaTitulos(a).Transaccion), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).tipoDeOferta), String.Empty, ListaTitulos(a).tipoDeOferta), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).DescripcionTipoDeOferta), String.Empty, ListaTitulos(a).DescripcionTipoDeOferta), strFormat, False))
    '        lstFields.Add(formatField(IIf(IsNothing(ListaTitulos(a).NombreIndicador), String.Empty, ListaTitulos(a).NombreIndicador), strFormat, False))


    '        buildStringOfRow(strBuilder, lstFields, strFormat)
    '        strLineas.Add(strBuilder.ToString())
    '    Next

    '    dcProxy.Guardar_ArchivoServidor(CSTR_NOMBREPROCESO_TITULOSACTIVOS, Program.Usuario, String.Format("ReporteTitulosActivos{0}.csv", Now.ToString("yyyy-mm-dd")), strLineas,Program.Usuario, Program.HashConexion, AddressOf TerminoCrearArchivo, True)

    'End Sub

    'Private Shared Function formatField(ByVal data As String, ByVal format As String, ByVal encabezado As Boolean) As String
    '    Select Case format
    '        Case "XML"
    '            Return String.Format("<Cell><Data ss:Type=""String" & """>{0}</Data></Cell>", data)
    '        Case "CSV"
    '            If encabezado = True Then
    '                Return String.Format("""   {0}   """, data.Replace("""", """""""").Replace("" & vbLf & "", "").Replace("" & vbCr & "", ""))
    '            Else
    '                Return String.Format("""{0}""", data.Replace("""", """""""").Replace("" & vbLf & "", "").Replace("" & vbCr & "", ""))
    '            End If
    '    End Select
    '    Return data

    'End Function

    'Private Shared Sub buildStringOfRow(ByVal strBuilder As StringBuilder, ByVal lstFields As List(Of String), ByVal strFormat As String)
    '    Select Case strFormat
    '        Case "XML"
    '            strBuilder.AppendLine("<Row>")
    '            strBuilder.AppendLine(String.Join("" & vbCrLf & "", lstFields.ToArray()))
    '            strBuilder.AppendLine("</Row>")
    '            ' break;
    '        Case "CSV"
    '            strBuilder.AppendLine(String.Join(SEPARATOR_FORMAT_CVS, lstFields.ToArray()))
    '            ' break;

    '    End Select

    'End Sub

        'Public Sub FormatoExcel()
    '    Const xlToRight As Integer = -4161
    '    Const xlCenter As Integer = -4108

    '    Using oApp As Object = AutomationFactory.CreateObject("Excel.application")

    '        oApplication.Visible = True
    '        oApplication.Workbooks.Open(FileName:="D:\Compartida\PruebasTitulos.xls")
    '        oApplication.Range("A1").Select()
    '        oApplication.cells.Select()
    '        oApplication.Selection.AutoFilter()
    '        oApplication.ActiveWindow.SplitRow = 1
    '        oApplication.ActiveWindow.FreezePanes = True
    '        oApplication.Range("A1").Select()
    '        oApplication.Range(oApplication.Selection, oApplication.Selection.End(xlToRight)).Select()
    '        oApplication.Selection.Interior.ColorIndex = 55 '14
    '        oApplication.Selection.Font.ColorIndex = 2
    '        oApplication.Selection.Font.Bold = True
    '        oApplication.Range("A2").Select()

    '        With oApplication.Selection
    '            .HorizontalAlignment = xlCenter
    '        End With

    '        oApplication.Range("A1").Select()
    '        oApplication.cells.Select()
    '        oApplication.cells.EntireColumn.AutoFit()
    '        oApplication.Range("A2").Select()
    '    End Using
    'End Sub

#End Region
#End Region

End Class


'Clase base para forma de búsquedas
Public Class CamposInformeAuditoria
    Implements INotifyPropertyChanged

    Private _FechaDesde As DateTime
    <Display(Name:="De la fecha:")> _
    Public Property FechaDesde As DateTime
        Get
            Return _FechaDesde
        End Get
        Set(ByVal value As DateTime)
            _FechaDesde = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaDesde"))
        End Set
    End Property

    Private _FechaHasta As DateTime
    <Display(Name:="A la fecha:")> _
    Public Property FechaHasta As DateTime
        Get
            Return _FechaHasta
        End Get
        Set(ByVal value As DateTime)
            _FechaHasta = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FechaHasta"))
        End Set
    End Property

    Private _Tabla As String
    <Display(Name:="Tabla:")> _
    Public Property Tabla As String
        Get
            Return _Tabla
        End Get
        Set(ByVal value As String)
            _Tabla = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Tabla"))
        End Set
    End Property

    Private _FiltroConsulta As String
    <Display(Name:="Filtro de la consulta:")> _
    Public Property FiltroConsulta As String
        Get
            Return _FiltroConsulta
        End Get
        Set(ByVal value As String)
            _FiltroConsulta = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FiltroConsulta"))
        End Set
    End Property

    Private _FiltroDatos As String
    <Display(Name:="Filtro de los datos devueltos:")> _
    Public Property FiltroDatos As String
        Get
            Return _FiltroDatos
        End Get
        Set(ByVal value As String)
            _FiltroDatos = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("FiltroDatos"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class





