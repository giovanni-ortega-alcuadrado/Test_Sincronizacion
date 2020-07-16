Imports Telerik.Windows.Controls
'Codigo Desarrollado por: Santiago Alexander Vergara Orrego
'Archivo: Public Class LiquidacionesDividendosTTVViewModel.vb
'Mayo 29/2013
'Propiedad de Alcuadrado S.A.
Imports System.ComponentModel
Imports System.Linq
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports System.Windows.Data
Imports System.Web
Imports System.Text
Imports A2ControlMenu


Imports System.Xml.Serialization
Imports System.IO
Imports System.Xml
Imports System.Xml.Linq
Imports System.Core
Imports A2.OYD.OYDServer.RIA.Web


Public Class LiquidacionesDividendosTTVViewModel
    Inherits A2ControlMenu.A2ViewModel
    Dim dcProxy As TesoreriaDomainContext

    Public Sub New()
        If System.Diagnostics.Debugger.IsAttached And Program.ejecutarAppSegunAmbiente() Then
            dcProxy = New TesoreriaDomainContext()

        Else
            dcProxy = New TesoreriaDomainContext(New System.Uri(Program.RutaServicioNegocio))

        End If
        Try
            If Not Program.IsDesignMode() Then

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la creación de los objetos", _
                                 Me.ToString(), "LiquidacionesDividendosTTVViewModel.New", Application.Current.ToString(), Program.Maquina, ex)
        End Try
        logSelectTodos = False
        dtmFecha = Date.Now
    End Sub

#Region "Metodos"

    ''' <summary>
    ''' Se arma el xml contodos los registros seleccionados en la pantalla y se envian a la base de datos para ser procesados,
    ''' es decir generar la nota contable y las facturas de banca inversión
    ''' </summary>
    ''' <remarks>SV20130605</remarks>
    Public Sub ProcesarDatos()
        Try
            If Not IsNothing(_ListaLiquidacionesDividendos) Then
                IsBusy = True
                _ListaLiquidacionesDividendosSeleccionadas = New List(Of OyDTesoreria.LiquidacionTTV)

                For Each objliquidacion In _ListaLiquidacionesDividendos
                    If objliquidacion.Marcar = True Then
                        _ListaLiquidacionesDividendosSeleccionadas.Add(objliquidacion)
                    End If
                Next

                If _ListaLiquidacionesDividendosSeleccionadas.Count > 0 Then

                    Dim strXMLCompleto As String

                    strXMLCompleto = "<Liquidaciones>  "

                    For Each obj In _ListaLiquidacionesDividendosSeleccionadas

                        Dim strXML = <Liquidacion lngIdLiquidacion=<%= obj.Id %>
                                         CodigoComprador=<%= obj.CodigoCompradorM %>
                                         NombreComprador=<%= obj.NombreCompradorM %>
                                         NroDocumentoComprador=<%= obj.NroDocumentoCompradorM %>
                                         CodigoVendedor=<%= obj.CodigoVendedorM %>
                                         NombreVendedor=<%= obj.NombreVendedorM %>
                                         NroDocumentoVendedor=<%= obj.NroDocumentoVendedorM %>
                                         CodigoFirmaContraparte=<%= obj.CodigoFirmaContraparteM %>
                                         CantidadAcciones=<%= obj.CantidadAccionesM %>
                                         ValorDividendos=<%= obj.ValorDividendosM %>
                                         valorRetencion=<%= obj.valorRetencionM %>
                                         NombreFirmaContraparteC=<%= obj.NombreFirmaContraparteCM %>
                                         NombreFirmaContraparteV=<%= obj.NombreFirmaContraparteVM %>
                                         OperacionCompra=<%= obj.OperacionCompraM %>
                                         OperacionVenta=<%= obj.OperacionVentaM %>
                                         EspecieCompra=<%= obj.EspecieCompraM %>
                                         OrdenSalida=<%= obj.OrdenSalidaM %>
                                         OrdenRegreso=<%= obj.OrdenRegresoM %>
                                         OrdeSalidaVenta=<%= obj.OrdeSalidaVentaM %>
                                         OrderSalidaRegreso=<%= obj.OrderSalidaRegresoM %>
                                         bitMarcado=<%= obj.Marcar %>>
                                     </Liquidacion>

                        strXMLCompleto = strXMLCompleto & strXML.ToString

                    Next
                    strXMLCompleto = strXMLCompleto & " </Liquidaciones>"
                    Dim strxmlseguro As String = System.Web.HttpUtility.HtmlEncode(strXMLCompleto)
                    If Not IsNothing(dcProxy.tblRespuestaValidacionesTesorerias) Then
                        dcProxy.tblRespuestaValidacionesTesorerias.Clear()
                    End If
                    dcProxy.Load(dcProxy.LiquidacionTTV_ProcesarQuery(strxmlseguro, Program.Usuario, Program.HashConexion), AddressOf TerminoProcesarLiquidaciones, "")

                Else
                    IsBusy = False
                    A2Utilidades.Mensajes.mostrarMensaje("No se han seleccionado registros para procesar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
                End If
            Else
                A2Utilidades.Mensajes.mostrarMensaje("No se han seleccionado registros para procesar.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia, "")
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al procesar las liquidaciones", _
                                 Me.ToString(), "ProcesarDatos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Proceso para consultar las liquidaciones de dividendos TTV dependiendo de la fecha que se seleccione en la pantalla 
    ''' </summary>
    ''' <remarks>SV20130516</remarks>
    Public Sub ConsultarLiquidacionesDividendos()
        Try
            IsBusy = True
            dcProxy.LiquidacionTTVs.Clear()
            dcProxy.Load(dcProxy.LiquidacionTTV_ConsultarQuery(_dtmfecha,Program.Usuario, Program.HashConexion), AddressOf TerminoTraerLiquidacionesDividendos, "Consulta")

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la consulta de liquidaciones dividendos TTV", _
                                 Me.ToString(), "ConsultarLiquidacionesDividendos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Public Function MarcarDesmarcarTodos(ByVal logMarcar As Boolean) As Boolean
        Try
            If Not IsNothing(ListaLiquidacionesDividendos) Then
                If logMarcar = True Then
                    For Each li In ListaLiquidacionesDividendos
                        li.Marcar = True
                    Next
                Else
                    For Each li In ListaLiquidacionesDividendos
                        li.Marcar = False
                    Next
                End If
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al marcar o desmarcar todos", _
                                 Me.ToString(), "MarcarDesmarcarTodos", Application.Current.ToString(), Program.Maquina, ex)
            Return False
        End Try
    End Function

#End Region

#Region "ResultadosAsincronicos"


    ''' <summary>
    ''' Proceso donde se recibe el resultado de procesar las liquidaciones de dividentos TTV
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SV20130605</remarks>
    Private Sub TerminoProcesarLiquidaciones(ByVal lo As LoadOperation(Of OyDTesoreria.tblRespuestaValidacionesTesoreria))

        If Not lo.HasError Then

            For Each obj In dcProxy.tblRespuestaValidacionesTesorerias.ToList
                If obj.Exitoso = False Then
                    A2Utilidades.Mensajes.mostrarMensaje(obj.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Errores, "")
                Else
                    A2Utilidades.Mensajes.mostrarMensaje(obj.Mensaje, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito, "")
                End If
            Next

            ConsultarLiquidacionesDividendos()
            logSelectTodos = False

        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al procesar las liquidaciones", _
                     Me.ToString(), "TerminoProcesarLiquidaciones", Application.Current.ToString(), Program.Maquina, lo.Error)

        End If
        IsBusy = False
    End Sub

    ''' <summary>
    ''' Proceso donde se recibe el resultado de la consulta de liquidaciones dividendos TTV
    ''' </summary>
    ''' <param name="lo"></param>
    ''' <remarks>SV20130530</remarks>
    Private Sub TerminoTraerLiquidacionesDividendos(ByVal lo As LoadOperation(Of OyDTesoreria.LiquidacionTTV))
        IsBusy = False
        If Not lo.HasError Then
            If dcProxy.LiquidacionTTVs.Count > 0 Then
                ListaLiquidacionesDividendos = dcProxy.LiquidacionTTVs
            Else
                A2Utilidades.Mensajes.mostrarMensaje("No se encontraron registros para la fecha seleccionada", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
            End If
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema en la obtención de liquidaciones dividendos TTV", _
                     Me.ToString(), "TerminoTraerLiquidacionesDividendos", Application.Current.ToString(), Program.Maquina, lo.Error)
        End If
    End Sub


#End Region

#Region "Propiedades"

    Private _logSelectTodos As Boolean
    Public Property logSelectTodos() As Boolean
        Get
            Return _logSelectTodos
        End Get
        Set(ByVal value As Boolean)

            If MarcarDesmarcarTodos(value) Then
                _logSelectTodos = value
                MyBase.CambioItem("logSelectTodos")
            End If

        End Set
    End Property


    Private _dtmfecha As DateTime
    Public Property dtmFecha As DateTime
        Get
            Return _dtmfecha
        End Get
        Set(ByVal value As DateTime)
            _dtmfecha = value
            MyBase.CambioItem("dtmfecha")
        End Set
    End Property


    Private _ListaLiquidacionesDividendos As EntitySet(Of OyDTesoreria.LiquidacionTTV)
    Public Property ListaLiquidacionesDividendos As EntitySet(Of OyDTesoreria.LiquidacionTTV)
        Get
            Return _ListaLiquidacionesDividendos
        End Get
        Set(ByVal value As EntitySet(Of OyDTesoreria.LiquidacionTTV))
            _ListaLiquidacionesDividendos = value
            MyBase.CambioItem("ListaLiquidacionesDividendos")
            MyBase.CambioItem("ListaLiquidacionesDividendosPaged")
        End Set
    End Property

    Private _ListaLiquidacionesDividendosSeleccionadas As List(Of OyDTesoreria.LiquidacionTTV)

    Public ReadOnly Property ListaLiquidacionesDividendosPaged() As PagedCollectionView
        Get
            If Not IsNothing(_ListaLiquidacionesDividendos) Then
                Dim view = New PagedCollectionView(_ListaLiquidacionesDividendos)
                Return view
            Else
                Return Nothing
            End If
        End Get
    End Property

#End Region

End Class

