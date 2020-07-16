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
Imports A2.OyD.OYDServer.RIA.Web.CFPortafolio
Imports A2ComunesImportaciones
Imports System.Windows
Imports System.Collections
Imports System.Text
Imports System.IO
Imports System.Reflection
Imports System.Runtime.InteropServices.Automation
Imports System.Object
Imports System.Globalization

Public Class ReporteExcelTitulosViewModel
    Inherits A2ControlMenu.A2ViewModel
    Dim dcProxy As PortafolioDomainContext
    Dim view As ReporteExcelTitulosActivos
    'Public Const CSTR_NOMBREPROCESO_TITULOSACTIVOS = "TITULOSACTIVOS"
    Public Const CSTR_NOMBREPROCESO_CSVREPORTE = "GeneracionCSVReportes"
    Private Shared SEPARATOR_FORMAT_CVS As String = System.Globalization.CultureInfo.CurrentCulture.Parent.TextInfo.ListSeparator
    Dim objServicios As A2VisorReportes.A2.Visor.Servicios.GeneralesClient


    Public Sub New()
        Try
            If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
                dcProxy = New PortafolioDomainContext()
            Else
                dcProxy = New PortafolioDomainContext(New System.Uri((Program.RutaServicioPortafolio)))
            End If
            DirectCast(dcProxy.DomainClient, WebDomainClient(Of PortafolioDomainContext.IPortafolioDomainServiceContract)).ChannelFactory.Endpoint.Binding.SendTimeout = New TimeSpan(0, 10, 0)

            'SLB20140528 Manejo del TimeOut a traves de un parametro de la aplicación.
            Dim TimeOutMinutos As Byte = 15
            If Not String.IsNullOrEmpty(Program.Consultar_TIME_OUT_REPORTES_EN_MINUTOS) Then
                TimeOutMinutos = CInt(Program.Consultar_TIME_OUT_REPORTES_EN_MINUTOS)
            End If

            If IsNothing(Application.Current.Resources("A2VServicioParam")) Then
                A2Utilidades.Mensajes.mostrarMensaje("No existe el parámetro 'A2VServicioParam', por favor verfique el archivo XML de configuración.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            clsEstablecerComunicacionVisorReportes.ServicioGenerales(objServicios, Application.Current.Resources("A2VServicioParam").ToString())
            'objServicios.Endpoint.Address = New System.ServiceModel.EndpointAddress(Application.Current.Resources("A2VServicioParam").ToString())
            'objServicios.InnerChannel.OperationTimeout = New TimeSpan(0, TimeOutMinutos, 0)
            'AddHandler objServicios.GenerarArchivoCompleted, AddressOf TerminoGenerarArchivo
            Limpiar() 'SLB20140512 Se pone aqui para quitarlo del Loaded de la vista.

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "ReporteExcelTitulosViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
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
            Dim cwCar As New ListarArchivosDirectorioView(CSTR_NOMBREPROCESO_CSVREPORTE) 'CSTR_NOMBREPROCESO_TITULOSACTIVOS)
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


    Private _cb As CamposReporteExcelTitulos = New CamposReporteExcelTitulos
    Public Property cb() As CamposReporteExcelTitulos
        Get
            Return _cb
        End Get
        Set(ByVal value As CamposReporteExcelTitulos)
            _cb = value
            MyBase.CambioItem("cb")
        End Set
    End Property


#End Region

#Region "Métodos"


    Public Sub Limpiar()
        Try
            cb.ConceptoBloqueo = "T"
            cb.Estado = "T"
            cb.TipoBloqueo = "T"
            cb.Fecha = Now
            cb.Deposito = "T"
            cb.IDComitenteDesde = "                0"
            cb.IDComitenteHasta = "99999999999999999"
            cb.IDEspecieDesde = "0"
            cb.IDEspecieHasta = "ZZZZ"
            cb.Receptor = "T"
            cb.Sucursal = "-1"

            MyBase.CambioItem("cb")

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al restaurar los valores por defecto", _
                                 Me.ToString(), "ReporteExcelTitulosViewModel.Limpiar", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Public Async Sub EjecutarConsulta()
        Try
            'IsBusy = True
            'dcProxy.Traer_ReporteExcelTitulos(cb.IDComitenteDesde, cb.IDComitenteHasta, cb.IDEspecieDesde, cb.IDEspecieHasta, _
            'IIf(cb.Fecha.Year < 1900, Now, cb.Fecha), cb.Estado, cb.ConceptoBloqueo, CInt(cb.Sucursal), cb.Receptor, cb.Deposito, Program.Usuario, Program.HashConexion, AddressOf TerminoTraerTitulos, "csv")

            'SLB20131108 Se genera el informe desde el Visor de Reportes
            Dim strParametros As String

            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
            System.Threading.Thread.CurrentThread.CurrentUICulture = New System.Globalization.CultureInfo("en-US")
            'CORREC_CITI_SV_2014
            Dim dtmfecha As Date = Date.Parse(cb.Fecha, CultureInfo.CurrentCulture, DateTimeStyles.AllowWhiteSpaces)
            Dim strfecha As String = dtmfecha.ToString("MM/dd/yyyy HH:mm:ss")

            strParametros = String.Format("'@pstrCodigoClienteInicio'**'{0}'**'STRING'|", cb.IDComitenteDesde)
            strParametros = strParametros & String.Format("'@pstrCodigoClienteFin'**'{0}'**'STRING'|", cb.IDComitenteHasta)
            strParametros = strParametros & String.Format("'@pstrCodigoEspecieInicio'**'{0}'**'STRING'|", cb.IDEspecieDesde)
            strParametros = strParametros & String.Format("'@pstrCodigoEspecieFin'**'{0}'**'STRING'|", cb.IDEspecieHasta)
            strParametros = strParametros & String.Format("'@pdtmFechaCorte'**'{0}'**'DATETIME'|", strfecha) ''CORREC_CITI_SV_2014
            strParametros = strParametros & String.Format("'@pstrEstadoTitulo'**'{0}'**'STRING'|", cb.Estado)
            strParametros = strParametros & String.Format("'@pstrConcepto'**'{0}'**'STRING'|", cb.ConceptoBloqueo)
            strParametros = strParametros & String.Format("'@plngSucursalReceptor'**'{0}'**'INTEGER'|", CInt(cb.Sucursal))
            strParametros = strParametros & String.Format("'@pstrIDReceptor'**'{0}'**'STRING'|", cb.Receptor)
            strParametros = strParametros & String.Format("'@pstrDeposito'**'{0}'**'STRING'|", cb.Deposito)
            strParametros = strParametros & String.Format("'@pstrUsuario'**'{0}'**'STRING'", Program.Usuario)

            If IsNothing(Application.Current.Resources("A2RutaFisicaGeneracion")) Then
                A2Utilidades.Mensajes.mostrarMensaje("No existe el parámetro 'A2RutaFisicaGeneracion', por favor verfique el archivo XML de configuración.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
                Exit Sub
            End If

            IsBusy = True
            Dim objRespuestaServicio = Await objServicios.GenerarArchivoAsync("Informe General Ampliado", Program.Usuario, Application.Current.Resources("A2RutaFisicaGeneracion").ToString, strParametros, "sp_rptCustodiasGeneral_TitulosActivos_OyDNet", Program.Maquina, A2VisorReportes.A2.Visor.Servicios.TipoArchivo.EXCEL.EXCEL, "Informe General Ampliado", Program.HashConexion("Informe General Ampliado"))
            TerminoGenerarArchivo(objRespuestaServicio)

        Catch ex As Exception
            IsBusy = False
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al ejecutar la consulta.",
                                 Me.ToString(), "ReporteExcelTitulosViewModel.EjecutarConsulta", Application.Current.ToString(), Program.Maquina, ex)
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
        Try
            If cb.Estado = "B" Then
                TipoBloqueoVisible = Visibility.Visible
            Else
                TipoBloqueoVisible = Visibility.Collapsed
                cb.TipoBloqueo = "T"
            End If
            MyBase.CambioItem("cb")
            MyBase.CambioItem("TipoBloqueo")
            MyBase.CambioItem("TipoBloqueoVisible")
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó al habilitar el Campo Tipo Bloqueo.", _
                                 Me.ToString(), "ReporteExcelTitulosViewModel.HabilitarTipoBloqueo", Application.Current.ToString(), Program.Maquina, ex)
        End Try
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

    '    dcProxy.Guardar_ArchivoServidor(CSTR_NOMBREPROCESO_TITULOSACTIVOS, Program.Usuario, String.Format("ReporteTitulosActivos{0}.csv", Now.ToString("yyyy-mm-dd")), strLineas, AddressOf TerminoCrearArchivo, True)

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
Public Class CamposReporteExcelTitulos
    Implements INotifyPropertyChanged

    Private _Deposito As String
    <Display(Name:="Fondo")> _
    Public Property Deposito As String
        Get
            Return _Deposito
        End Get
        Set(ByVal value As String)
            _Deposito = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Deposito"))
        End Set
    End Property

    Private _Sucursal As String
    <Display(Name:="Sucursal")> _
    Public Property Sucursal As String
        Get
            Return _Sucursal
        End Get
        Set(ByVal value As String)
            _Sucursal = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Sucursal"))
        End Set
    End Property

    Private _Receptor As String
    <Display(Name:="Receptor")> _
    Public Property Receptor As String
        Get
            Return _Receptor
        End Get
        Set(ByVal value As String)
            _Receptor = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Receptor"))
        End Set
    End Property

    Private _Estado As String
    <Display(Name:="Estado")> _
    Public Property Estado As String
        Get
            Return _Estado
        End Get
        Set(ByVal value As String)
            _Estado = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Estado"))
        End Set
    End Property

    Private _TipoBloqueo As String
    <Display(Name:="Tipo Bloqueo")> _
    Public Property TipoBloqueo As String
        Get
            Return _TipoBloqueo
        End Get
        Set(ByVal value As String)
            _TipoBloqueo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Estado"))
        End Set
    End Property

    Private _ConceptoBloqueo As String
    <Display(Name:="Concepto Bloqueo")> _
    Public Property ConceptoBloqueo As String
        Get
            Return _ConceptoBloqueo
        End Get
        Set(ByVal value As String)
            _ConceptoBloqueo = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("ConceptoBloqueo"))
        End Set
    End Property

    Private _IDComitenteDesde As String
    <Display(Name:="Comitente Desde")> _
    Public Property IDComitenteDesde As String
        Get
            Return _IDComitenteDesde
        End Get
        Set(ByVal value As String)
            _IDComitenteDesde = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDComitenteDesde"))
        End Set
    End Property

    Private _IDComitenteHasta As String
    <Display(Name:="Comitente Hasta")> _
    Public Property IDComitenteHasta As String
        Get
            Return _IDComitenteHasta
        End Get
        Set(ByVal value As String)
            _IDComitenteHasta = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDComitenteHasta"))
        End Set
    End Property

    Private _IDEspecieDesde As String
    <Display(Name:="Especie Desde")> _
    Public Property IDEspecieDesde As String
        Get
            Return _IDEspecieDesde
        End Get
        Set(ByVal value As String)
            _IDEspecieDesde = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDEspecieDesde"))
        End Set
    End Property

    Private _IDEspecieHasta As String
    <Display(Name:="Especie Hasta")> _
    Public Property IDEspecieHasta As String
        Get
            Return _IDEspecieHasta
        End Get
        Set(ByVal value As String)
            _IDEspecieHasta = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("IDEspecieHasta"))
        End Set
    End Property

    Private _Fecha As DateTime
    <Display(Name:="Fecha")> _
    Public Property Fecha As DateTime
        Get
            Return _Fecha
        End Get
        Set(ByVal value As DateTime)
            _Fecha = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Fecha"))
        End Set
    End Property

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
End Class





