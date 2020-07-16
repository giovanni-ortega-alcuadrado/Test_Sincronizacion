
'Codigo generado
'Plantilla: RIAServiceModelMetadataTemplate2010
'Archivo: ViewModel.vb
'Generado el : 06/17/2011 11:58:30
'Propiedad de Alcuadrado S.A. 2010
Option Compare Binary
Option Infer On
Option Strict On
Option Explicit On

Imports A2.CF.RIA.Web
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Data.Linq
Imports System.Linq
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server

Namespace CFPortafolio
    <MetadataTypeAttribute(GetType(Custodia.CustodiaMetadata))> _
    Partial Public Class Custodia
        Friend NotInheritable Class CustodiaMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="IdComisionista", Description:="IdComisionista")> _
            Public Property IdComisionista As Integer

            <Display(Name:="IdSucComisionista", Description:="IdSucComisionista")> _
            Public Property IdSucComisionista As Integer

            '<Required(ErrorMessage:="Este campo es requerido. (IdRecibo)")> _
            <Display(Name:="Recibo No")> _
            Public Property IdRecibo As Integer

            '<Required(ErrorMessage:="Este campo es requerido. (Comitente)")> _
            '<StringLength(17, ErrorMessage:="El campo {0} permite una longitud máxima de 17.")> _
            <Display(Name:="Cliente")> _
            Public Property Comitente As String

            '<Required(ErrorMessage:="Este campo es requerido. (Tipo Identificación)")> _
            <Display(Name:="Tipo ID")> _
            Public Property TipoIdentificacion As String

            '<Required(ErrorMessage:="Este campo es requerido. (NroDocumento)")> _
            <Display(Name:="Número")> _
            Public Property NroDocumento As Nullable(Of Decimal)

            '<StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50.")> _
            '<Required(ErrorMessage:="Este campo es requerido. (Nombre)")> _
            <Display(Name:="Nombre")> _
            Public Property Nombre As String

            '<StringLength(25, ErrorMessage:="El campo {0} permite una longitud máxima de 25.")> _
            <Display(Name:="Teléfono")> _
            Public Property Telefono1 As String

            '<StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50.")> _
            <Display(Name:="Dirección")> _
            Public Property Direccion As String

            '<Required(ErrorMessage:="Este campo es requerido. (Recibo)")> _
            <Display(Name:="Fecha Recibo")> _
            Public Property Recibo As DateTime

            '<Required(ErrorMessage:="Este campo es requerido. (Estado)")> _
            <Display(Name:="Estado")> _
            Public Property Estado As String

            '<Required(ErrorMessage:="Este campo es requerido. (Fecha Estado)")> _
            <Display(Name:="Fecha")> _
            Public Property Fecha_Estado As DateTime

            '<StringLength(25, ErrorMessage:="El campo {0} permite una longitud máxima de 25.")> _
            <Display(Name:="Causa")> _
            Public Property ConceptoAnulacion As String

            '<StringLength(255, ErrorMessage:="El campo {0} permite una longitud máxima de 255.")> _
            <Display(Name:="Notas", Description:="Notas")> _
            Public Property Notas As String

            '<Required(ErrorMessage:="Este campo es requerido. (NroLote)")> _
            <Display(Name:="NroLote", Description:="NroLote")> _
            Public Property NroLote As Integer

            <Display(Name:="Elaboración")> _
            Public Property Elaboracion As Nullable(Of DateTime)

            <Display(Name:="Actualización", Description:="Actualización")> _
            Public Property Actualizacion As DateTime

            '<Required(ErrorMessage:="Este campo es requerido. (Usuario)")> _
            <Display(Name:="Usuario", Description:="Usuario")> _
            Public Property Usuario As String

            <Display(Name:="Custodia", Description:="IDCustodia")> _
            Public Property IDCustodia As Integer

            <Display(Name:=" ")> _
            Public Property Por_Aprobar As String

            <Display(Name:=" ")> _
            Public Property DescripcionEstado As String

        End Class
    End Class

    <MetadataTypeAttribute(GetType(DetalleCustodia.DetalleCustodiaMetadata))> _
    Partial Public Class DetalleCustodia
        Friend NotInheritable Class DetalleCustodiaMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="IdComisionista", Description:="IdComisionista")> _
            Public Property IdComisionista As Integer

            <Display(Name:="IdSucComisionista", Description:="IdSucComisionista")> _
            Public Property IdSucComisionista As Integer

            '<Required(ErrorMessage:="Este campo es requerido. (IdRecibo)")> _
            <Display(Name:="IdRecibo", Description:="IdRecibo")> _
            Public Property IdRecibo As Integer

            '<Required(ErrorMessage:="Este campo es requerido. (Secuencia)")> _
            <Display(Name:="Secuencia", Description:="Secuencia")> _
            Public Property Secuencia As Integer

            '<Required(ErrorMessage:="Este campo es requerido. (Comitente)")> _
            <Display(Name:="Comitente", Description:="Comitente")> _
            Public Property Comitente As String

            '<Required(ErrorMessage:="Este campo es requerido. (IdEspecie)")> _
            <Display(Name:="Especie", Description:="IdEspecie")> _
            Public Property IdEspecie As String

            '<Required(ErrorMessage:="Este campo es requerido. (NroTitulo)")> _
            ' <StringLength(25, ErrorMessage:="El campo {0} permite una longitud máxima de 25.")> _
            <Display(Name:="NroTitulo", Description:="NroTitulo")> _
            Public Property NroTitulo As String

            '<Required(ErrorMessage:="Este campo es requerido. (RentaVariable)")> _
            <Display(Name:="RentaVariable", Description:="RentaVariable")> _
            Public Property RentaVariable As Boolean

            <Display(Name:="Indicador Economico", Description:="IndicadorEconomico")> _
            Public Property IndicadorEconomico As String

            <Display(Name:="Puntos Indicador", Description:="PuntosIndicador")> _
            Public Property PuntosIndicador As Nullable(Of Double)

            <Display(Name:="Dias Vencimiento", Description:="DiasVencimiento")> _
            Public Property DiasVencimiento As Nullable(Of Integer)

            <Display(Name:="Modalidad", Description:="Modalidad")> _
            Public Property Modalidad As String

            <Display(Name:="Emisión", Description:="Emision")> _
            Public Property Emision As Nullable(Of DateTime)

            <Display(Name:="Vencimiento", Description:="Vencimiento")> _
            Public Property Vencimiento As Nullable(Of DateTime)

            <Display(Name:="Cantidad", Description:="Cantidad")> _
            Public Property Cantidad As Nullable(Of Double)

            <Display(Name:="Fondo", Description:="Fondo")> _
            Public Property Fondo As String

            <Display(Name:="Tasa Interés", Description:="TasaInteres")> _
            Public Property TasaInteres As Nullable(Of Double)

            '<StringLength(25, ErrorMessage:="El campo {0} permite una longitud máxima de 25.")> _
            <Display(Name:="NroRefFondo", Description:="NroRefFondo")> _
            Public Property NroRefFondo As String

            <Display(Name:="Retención", Description:="Retencion")> _
            Public Property Retencion As Nullable(Of DateTime)

            <Display(Name:="Tasa Retención", Description:="TasaRetencion")> _
            Public Property TasaRetencion As Nullable(Of Double)

            <Display(Name:="Valor Retención", Description:="ValorRetencion")> _
            Public Property ValorRetencion As Nullable(Of Double)

            <Display(Name:="PorcRendimiento", Description:="PorcRendimiento")> _
            Public Property PorcRendimiento As Nullable(Of Double)

            <Display(Name:="AgenteRetenedor", Description:="IdAgenteRetenedor")> _
            Public Property IdAgenteRetenedor As Nullable(Of Integer)

            <Display(Name:="EstadoActual", Description:="EstadoActual")> _
            Public Property EstadoActual As String

            '<Required(ErrorMessage:="Este campo es requerido. (ObjVenta)")> _
            <Display(Name:="ObjVenta", Description:="ObjVenta")> _
            Public Property ObjVenta As Boolean

            '<Required(ErrorMessage:="Este campo es requerido. (ObjRenovReinv)")> _
            <Display(Name:="ObjRenovReinv", Description:="ObjRenovReinv")> _
            Public Property ObjRenovReinv As Boolean

            '<Required(ErrorMessage:="Este campo es requerido. (ObjCobroIntDiv)")> _
            <Display(Name:="ObjCobroIntDiv", Description:="ObjCobroIntDiv")> _
            Public Property ObjCobroIntDiv As Boolean

            '<Required(ErrorMessage:="Este campo es requerido. (ObjSuscripcion)")> _
            <Display(Name:="ObjSuscripcion", Description:="ObjSuscripcion")> _
            Public Property ObjSuscripcion As Boolean

            '<Required(ErrorMessage:="Este campo es requerido. (ObjCancelacion)")> _
            <Display(Name:="ObjCancelacion", Description:="ObjCancelacion")> _
            Public Property ObjCancelacion As Boolean

            <Display(Name:="Notas", Description:="Notas")> _
            Public Property Notas As String

            <Display(Name:="Sellado", Description:="Sellado")> _
            Public Property Sellado As Nullable(Of DateTime)

            <Display(Name:="CuentaDeceval", Description:="IdCuentaDeceval")> _
            Public Property IdCuentaDeceval As Nullable(Of Integer)

            <Display(Name:="ISIN", Description:="ISIN")> _
            Public Property ISIN As String

            <Display(Name:="Fungible", Description:="Fungible")> _
            Public Property Fungible As Nullable(Of Integer)

            '<StringLength(1, ErrorMessage:="El campo {0} permite una longitud máxima de 1.")> _
            <Display(Name:="TipoValor", Description:="TipoValor")> _
            Public Property TipoValor As String

            <Display(Name:="FechasPagoRendimientos", Description:="FechasPagoRendimientos")> _
            Public Property FechasPagoRendimientos As String

            <Display(Name:="DepositoExtranjero", Description:="IDDepositoExtranjero")> _
            Public Property IDDepositoExtranjero As Nullable(Of Integer)

            <Display(Name:="Custodio", Description:="IDCustodio")> _
            Public Property IDCustodio As Nullable(Of Integer)

            <Display(Name:="TitularCustodio", Description:="TitularCustodio")> _
            Public Property TitularCustodio As String

            '<StringLength(30, ErrorMessage:="El campo {0} permite una longitud máxima de 30.")> _
            <Display(Name:="Reinversion", Description:="Reinversion")> _
            Public Property Reinversion As String

            <Display(Name:="Liquidacion", Description:="IDLiquidacion")> _
            Public Property IDLiquidacion As Nullable(Of Integer)

            <Display(Name:="Parcial", Description:="Parcial")> _
            Public Property Parcial As Nullable(Of Integer)

            '<StringLength(1, ErrorMessage:="El campo {0} permite una longitud máxima de 1.")> _
            <Display(Name:="TipoLiquidacion", Description:="TipoLiquidacion")> _
            Public Property TipoLiquidacion As String

            '<StringLength(1, ErrorMessage:="El campo {0} permite una longitud máxima de 1.")> _
            <Display(Name:="ClaseLiquidacion", Description:="ClaseLiquidacion")> _
            Public Property ClaseLiquidacion As String

            <Display(Name:="Liquidación", Description:="Liquidacion")> _
            Public Property Liquidacion As Nullable(Of DateTime)

            <Display(Name:="TotalLiq", Description:="TotalLiq")> _
            Public Property TotalLiq As Nullable(Of Double)

            <Display(Name:="TasaCompraVende", Description:="TasaCompraVende")> _
            Public Property TasaCompraVende As Nullable(Of Double)

            <Display(Name:="CumplimientoTitulo", Description:="CumplimientoTitulo")> _
            Public Property CumplimientoTitulo As Nullable(Of DateTime)

            <Display(Name:="TasaDescuento", Description:="TasaDescuento")> _
            Public Property TasaDescuento As Nullable(Of Double)

            '<StringLength(1, ErrorMessage:="El campo {0} permite una longitud máxima de 1.")> _
            <Display(Name:="MotivoBloqueo", Description:="MotivoBloqueo")> _
            Public Property MotivoBloqueo As String

            '<StringLength(300, ErrorMessage:="El campo {0} permite una longitud máxima de 300.")> _
            <Display(Name:="NotasBloqueo", Description:="NotasBloqueo")> _
            Public Property NotasBloqueo As String

            <Display(Name:="EstadoEntrada", Description:="EstadoEntrada")> _
            Public Property EstadoEntrada As String

            <Display(Name:="EstadoSalida", Description:="EstadoSalida")> _
            Public Property EstadoSalida As String

            <Display(Name:="CargadoArchivo", Description:="CargadoArchivo")> _
            Public Property CargadoArchivo As Boolean

            <Display(Name:="Actualizacion", Description:="Actualizacion")> _
            Public Property Actualizacion As DateTime

            <Required(ErrorMessage:="Este campo es requerido. (Usuario)")> _
              <Display(Name:="Usuario", Description:="Usuario")> _
            Public Property Usuario As String

            <Display(Name:="DetalleCustodias", Description:="IDDetalleCustodias")> _
            Public Property IDDetalleCustodias As Integer

            <Display(Name:="Precio", Description:="Precio")> _
            Public Property Precio As Nullable(Of Double)

            '<Required(ErrorMessage:="Este campo es requerido. (Nombre)")> _
            <Display(Name:="Nombre", Description:="Nombre")> _
            Public Property Nombre As String

        End Class
    End Class

    ''' <summary>
    ''' Clase para definicion de los campos del tipo BeneficiariosCustodia
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' Creado por       : Juan Carlos Soto.
    ''' Descripción      : Creado.
    ''' Fecha            : Febrero 26/2013
    ''' Pruebas Negocio  : No se le han hecho pruebas de caja blanca. 
    ''' 
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se comentan los campos Secuencia y Conector. Se agregan los campos Cuenta Depósito, Depósito y Nro Título.
    ''' Fecha            : Marzo 27/2013
    ''' Pruebas Negocio  : Pruebas CB OK - Marzo 27/2013. 
    ''' </history>
    <MetadataTypeAttribute(GetType(BeneficiariosCustodia.BeneficiariosCustodiaMetadata))> _
    Partial Public Class BeneficiariosCustodia
        Friend NotInheritable Class BeneficiariosCustodiaMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="Cuenta Depósito", Description:="Cuenta Depósito")> _
            Public Property CuentaDeposito As Integer

            <Display(Name:="Depósito", Description:="Depósito")> _
            Public Property Deposito As String

            <Display(Name:="Tipo Identificación", Description:="Tipo Identificación")> _
            Public Property TipoIdentificacion As String

            <Display(Name:="No. Documento", Description:="No. Documento")> _
            Public Property NroDocumento As Decimal

            <Display(Name:="Nombre", Description:="Nombre")> _
            Public Property Nombre As String

            '<Display(Name:="Secuencia", Description:="Secuencia")> _
            'Public Property Secuencia As Integer

            '<Display(Name:="Conector", Description:="Conector")> _
            'Public Property Conector As String

            <Display(Name:="No. Título", Description:="No. Título")> _
            Public Property NroTitulo As String

        End Class
    End Class

    <MetadataTypeAttribute(GetType(ListadoCustodiasEntrega.ListadoCustodiasEntregaMetadata))> _
    Partial Public Class ListadoCustodiasEntrega
        Friend NotInheritable Class ListadoCustodiasEntregaMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="cantidad", Description:="cantidad")> _
            Public Property cantidad As Nullable(Of Double)

            <Display(Name:="ClaseLiquidacion", Description:="ClaseLiquidacion")> _
            Public Property ClaseLiquidacion As String

            <Display(Name:="CumplimientoTitulo", Description:="CumplimientoTitulo")> _
            Public Property CumplimientoTitulo As Nullable(Of DateTime)

            <Display(Name:="curTotalLiq", Description:="curTotalLiq")> _
            Public Property curTotalLiq As Nullable(Of Double)

            <Display(Name:="DescEstado", Description:="DescEstado")> _
            Public Property DescEstado As String

            <Display(Name:="DiasVencimiento", Description:="DiasVencimiento")> _
            Public Property DiasVencimiento As Nullable(Of Integer)

            <Display(Name:="Direccion", Description:="Direccion")> _
            Public Property Direccion As String

            <Display(Name:="Emision", Description:="Emision")> _
            Public Property Emision As Nullable(Of DateTime)

            <Display(Name:="EstadoActual", Description:="EstadoActual")> _
            Public Property EstadoActual As String

            <Display(Name:="FechasPagoRendimientos", Description:="FechasPagoRendimientos")> _
            Public Property FechasPagoRendimientos As String

            <Display(Name:="Fondo", Description:="Fondo")> _
            Public Property Fondo As String


            <Display(Name:="Fungible", Description:="Fungible")> _
            Public Property Fungible As Nullable(Of Integer)

            <Display(Name:="IdAgenteRetenedor", Description:="IdAgenteRetenedor")> _
            Public Property IdAgenteRetenedor As Nullable(Of Integer)

            <Display(Name:="IdCuentaDeceval", Description:="IdCuentaDeceval")> _
            Public Property IdCuentaDeceval As Nullable(Of Integer)

            <Display(Name:="IDCustodio", Description:="IDCustodio")> _
            Public Property IDCustodio As Nullable(Of Integer)

            <Display(Name:="IDDepositoExtranjero", Description:="IDDepositoExtranjero")> _
            Public Property IDDepositoExtranjero As Nullable(Of Integer)

            <Display(Name:="IDEspecie", Description:="IDEspecie")> _
            Public Property IDEspecie As String

            <Display(Name:="IDLiquidacion", Description:="IDLiquidacion")> _
            Public Property IDLiquidacion As Nullable(Of Integer)

            <Display(Name:="idRecibo", Description:="idRecibo")> _
            Public Property idRecibo As Integer

            <Display(Name:="IndicadorEconomico", Description:="IndicadorEconomico")> _
            Public Property IndicadorEconomico As String

            <Display(Name:="ISIN", Description:="ISIN")> _
            Public Property ISIN As String

            <Display(Name:="Liquidacion", Description:="Liquidacion")> _
            Public Property Liquidacion As Nullable(Of DateTime)

            <Display(Name:="Modalidad", Description:="Modalidad")> _
            Public Property Modalidad As String

            <Display(Name:="Nombre", Description:="Nombre")> _
            Public Property Nombre As String

            <Display(Name:="NroDocumento", Description:="NroDocumento")> _
            Public Property NroDocumento As Decimal

            <Display(Name:="NroRefFondo", Description:="NroRefFondo")> _
            Public Property NroRefFondo As String

            <Display(Name:="Nrotitulo", Description:="Nrotitulo")> _
            Public Property Nrotitulo As String

            <Display(Name:="ObjCancelacion", Description:="ObjCancelacion")> _
            Public Property ObjCancelacion As Boolean

            <Display(Name:="ObjCobroIntDiv", Description:="ObjCobroIntDiv")> _
            Public Property ObjCobroIntDiv As Boolean

            <Display(Name:="ObjRenovReinv", Description:="ObjRenovReinv")> _
            Public Property ObjRenovReinv As Boolean

            <Display(Name:="ObjSuscripcion", Description:="ObjSuscripcion")> _
            Public Property ObjSuscripcion As Boolean

            <Display(Name:="ObjVenta", Description:="ObjVenta")> _
            Public Property ObjVenta As Boolean

            <Display(Name:="Parcial", Description:="Parcial")> _
            Public Property Parcial As Nullable(Of Integer)

            <Display(Name:="PorcRendimiento", Description:="PorcRendimiento")> _
            Public Property PorcRendimiento As Nullable(Of Double)

            <Display(Name:="PuntosIndicador", Description:="PuntosIndicador")> _
            Public Property PuntosIndicador As Nullable(Of Double)

            <Display(Name:="ReciboTR", Description:="ReciboTR")> _
            Public Property ReciboTR As Nullable(Of Integer)

            <Display(Name:="Reinversion", Description:="Reinversion")> _
            Public Property Reinversion As String

            <Display(Name:="RentaVariable", Description:="RentaVariable")> _
            Public Property RentaVariable As Boolean

            <Display(Name:="Retencion", Description:="Retencion")> _
            Public Property Retencion As Nullable(Of DateTime)

            <Display(Name:="Secuencia", Description:="Secuencia")> _
            Public Property Secuencia As Integer

            <Display(Name:="Sellado", Description:="Sellado")> _
            Public Property Sellado As Nullable(Of DateTime)

            <Display(Name:="Sellado1", Description:="Sellado1")> _
            Public Property Sellado1 As Nullable(Of DateTime)

            <Display(Name:="TasaCompraVende", Description:="TasaCompraVende")> _
            Public Property TasaCompraVende As Nullable(Of Double)

            <Display(Name:="TasaDescuento", Description:="TasaDescuento")> _
            Public Property TasaDescuento As Nullable(Of Double)

            <Display(Name:="TasaInteres", Description:="TasaInteres")> _
            Public Property TasaInteres As Nullable(Of Double)

            <Display(Name:="TasaRetencion", Description:="TasaRetencion")> _
            Public Property TasaRetencion As Nullable(Of Double)

            <Display(Name:="telefono1", Description:="telefono1")> _
            Public Property telefono1 As String

            <Display(Name:="TipoIdentificacion", Description:="TipoIdentificacion")> _
            Public Property TipoIdentificacion As Char

            <Display(Name:="TipoLiquidacion", Description:="TipoLiquidacion")> _
            Public Property TipoLiquidacion As String

            <Display(Name:="TipoValor", Description:="TipoValor")> _
            Public Property TipoValor As Nullable(Of Char)

            <Display(Name:="TitularCustodio", Description:="TitularCustodio")> _
            Public Property TitularCustodio As String

            <Display(Name:="ValorRetencion", Description:="ValorRetencion")> _
            Public Property ValorRetencion As Nullable(Of Double)

            <Display(Name:="Vencimiento", Description:="Vencimiento")> _
            Public Property Vencimiento As Nullable(Of DateTime)

            <Display(Name:="ObjParaEntregarAlCliente", Description:="ObjParaEntregarAlCliente")> _
            Public Property ObjParaEntregarAlCliente As Nullable(Of Boolean)

            <Display(Name:="CantidadDevolver", Description:="CantidadDevolver")> _
            Public Property CantidadDevolver As Nullable(Of Double)

        End Class
    End Class

    <MetadataTypeAttribute(GetType(CaracteristicasTitulos.CaracteristicasTitulosMetadata))> _
    Partial Public Class CaracteristicasTitulos
        Friend NotInheritable Class CaracteristicasTitulosMetadata
            Private Sub New()
                MyBase.New()
            End Sub


            <Required(ErrorMessage:="Este campo es requerido. (Recibo Nro)")> _
              <Display(Name:="Recibo Nro")> _
            Public Property IdRecibo As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Secuencia)")> _
              <Display(Name:="Secuencia")> _
            Public Property Secuencia As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Titulo Nro)")> _
            <Display(Name:="Titulo Nro")> _
            Public Property NroTitulo As String

            <Required(ErrorMessage:="Este campo es requerido. (Cliente)")> _
            <Display(Name:="Cliente")> _
            Public Property Comitente As String

            <Required(ErrorMessage:="Este campo es requerido. (Especie)")> _
              <Display(Name:="Especie")> _
            Public Property IdEspecie As String

            <Display(Name:="Cantidad")> _
            Public Property Cantidad As Nullable(Of Double)

            <Display(Name:="Liquidación")> _
            Public Property IDLiquidacion As Nullable(Of Integer)

            <Display(Name:="Fecha de Liquidación")> _
            Public Property Liquidacion As Nullable(Of DateTime)

            <Display(Name:="Total Liquidación")> _
            Public Property TotalLiq As Nullable(Of Double)

            <Display(Name:="Parcial")> _
            Public Property Parcial As Nullable(Of Integer)

            <Display(Name:="Fecha Cumplimiento")> _
            Public Property CumplimientoTitulo As Nullable(Of DateTime)

            <Display(Name:="Clase")> _
            Public Property ClaseLiquidacion As String

            <Display(Name:="Tasa Efectiva Real")> _
            Public Property TasaDescuento As Nullable(Of Double)

            <Display(Name:="Indicador")> _
            Public Property IndicadorEconomico As String

            <Display(Name:="Tipo")> _
            Public Property TipoLiquidacion As String

            <Display(Name:="Tasa Facial")> _
            Public Property TasaCompraVende As Nullable(Of Double)

            <Display(Name:="Puntos Indicador")> _
            Public Property PuntosIndicador As Nullable(Of Double)

            <Range(0, 999999999, ErrorMessage:="El campo {0} permite una longitud máxima de 9.")> _
            <Display(Name:="Fungible")> _
            Public Property Fungible As Nullable(Of Integer)


            <Display(Name:="Cuenta Fondo")> _
            Public Property IdCuentaDeceval As Integer

            <Display(Name:="Fondo")> _
            Public Property Fondo As String

            <Display(Name:="ISIN")> _
            Public Property ISIN As String

            <Required(ErrorMessage:="Este campo es requerido. (Usuario)")> _
              <Display(Name:="Usuario")> _
            Public Property Usuario As String

            <Display(Name:="IDDetalleCustodias")> _
            Public Property IDDetalleCustodias As Integer

            <Display(Name:="Modalidad")> _
            Public Property Modalidad As String

        End Class
    End Class


    <MetadataTypeAttribute(GetType(ReporteExcelTitulo.ReporteExcelTituloMetadata))> _
    Partial Public Class ReporteExcelTitulo
        Friend NotInheritable Class ReporteExcelTituloMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID")> _
            Public Property ID As Integer

            <Display(Name:="Fecha de Corte")> _
            Public Property FechaCorte As DateTime

            <Display(Name:="Codigo Cliente")> _
            Public Property CodigoCliente As Integer

            <Display(Name:="Nombre Cliente")> _
            Public Property NombreCliente As String

            <Display(Name:="Nemotecnico Especie")> _
            Public Property NemotecnicoEspecie As String

            <Display(Name:="Nombre Especie")> _
            Public Property NombreEspecie As Integer

            <Display(Name:="Valor Nominal Titulo")> _
            Public Property ValorNomina As Double

            <Display(Name:="Nro Recibo Custodia")> _
            Public Property NroRecibo As Integer

            <Display(Name:="Nro Fila Custodia")> _
            Public Property NroFila As Integer

            <Display(Name:="Codigo Deposito Valores")> _
            Public Property CodigoDeposito As String

            <Display(Name:="Nombre Deposito de Valores")> _
            Public Property NombreDeposito As String

            <Display(Name:="ISIN")> _
            Public Property ISIN As String

            <Display(Name:="Cuenta Deposito")> _
            Public Property CuentaDeposito As Integer

            <Display(Name:="Concepto Bloqueo")> _
            Public Property ConceptoBloqueo As String

            <Display(Name:="Codigo Sucursal Receptor")> _
            Public Property CodigoSucursal As Integer

            <Display(Name:="Nombre Sucursal Receptor")> _
            Public Property NombreSucursal As String

            <Display(Name:="Codigo Receptor Lider Cliente")> _
            Public Property CodigoReceptor As String

            <Display(Name:="Nombre Receptor Lider Cliente")> _
            Public Property NombreReceptor As String

            <Display(Name:="Nro Liquidacion")> _
            Public Property NroLiquidacion As Integer

            <Display(Name:="Nro Parcial")> _
            Public Property NroParcial As Integer

            <Display(Name:="Fecha Elaboracion Liquidacion")> _
            Public Property FechaElaboracion As DateTime

            <Display(Name:="Tipo Liquidacion")> _
            Public Property TipoLiquidacion As String

            <Display(Name:="Clase Liquidacion")> _
            Public Property ClaseLiquidacion As String

            <Display(Name:="Periodicidad")> _
            Public Property Periodicidad As String

            <Display(Name:="Tasa Emision")> _
            Public Property TasaEmision As Double

            <Display(Name:="Indicador Economico")> _
            Public Property IndicadorEconomico As String

            <Display(Name:="Puntos indicador")> _
            Public Property Puntos As Double

            <Display(Name:="Fecha Emision")> _
            Public Property Emision As DateTime

            <Display(Name:="Fecha Vencimiento")> _
            Public Property Vencimiento As DateTime

            <Display(Name:="VPN_Mercado_Alianza")> _
            Public Property VPNMercado As Double

            '<Display(Name:="Total (VPN  X  V/r nominal)")> _
            'Public Property VPNMercadoTotal As Double

            <Display(Name:="NroTitulo")> _
            Public Property NroTitulo As String

            <Display(Name:="TIRVPN")> _
            Public Property TIRVPN As Double

            <Display(Name:="VlrLineal")> _
            Public Property VlrLineal As Double

            <Display(Name:="TIROriginal")> _
            Public Property TIROriginal As Double

            <Display(Name:="TIRActual")> _
            Public Property TIRActual As Double

            <Display(Name:="Spread")> _
            Public Property Spread As Double

            <Display(Name:="TIRSpread")> _
            Public Property TIRSpread As Double

            <Display(Name:="Valor Valoración OyD")> _
            Public Property ValorValoracionOyD As Double

            <Display(Name:="Fecha Recibo")> _
            Public Property Recibo As DateTime

            <Display(Name:="Fecha Elaboracion")> _
            Public Property Elaboracion As DateTime

            <Display(Name:="Nro Documento")> _
            Public Property NroDocumento As String

            <Display(Name:="Tasa Compra")> _
            Public Property TasaCompra As Double

            <Display(Name:="TasaEfectiva")> _
            Public Property TasaEfectiva As Double

            <Display(Name:="Precio")> _
            Public Property Precio As Decimal

            <Display(Name:="Liquidación")> _
            Public Property Liquidacion As DateTime

            <Display(Name:="Transacción")> _
            Public Property Transaccion As Decimal

            <Display(Name:="Tipo de Oferta")> _
            Public Property tipoDeOferta As String

            <Display(Name:="Descripción Tipo de Oferta")> _
            Public Property DescripcionTipoDeOferta As String

            <Display(Name:="Nombre Indicador")> _
            Public Property NombreIndicador As String

        End Class
    End Class

#Region "SaldarPagosDECEVAL"
    <MetadataTypeAttribute(GetType(SaldarPagosDECEVA.SaldarPagosDECEVAMetadata))> _
    Partial Public Class SaldarPagosDECEVA
        Friend NotInheritable Class SaldarPagosDECEVAMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Required(ErrorMessage:="Este campo es requerido. (Tesoreria)")> _
              <Display(Name:="Tesoreria", Description:="Tesoreria")> _
            Public Property Tesoreria As Nullable(Of Boolean)

            <Required(ErrorMessage:="Este campo es requerido. (FechaUno)")> _
              <Display(Name:="FechaUno", Description:="FechaUno")> _
            Public Property FechaUno As Nullable(Of DateTime)

            <Required(ErrorMessage:="Este campo es requerido. (FechaDos)")> _
              <Display(Name:="FechaDos", Description:="FechaDos")> _
            Public Property FechaDos As Nullable(Of DateTime)

            <Required(ErrorMessage:="Este campo es requerido. (ConsecutivoUno)")> _
              <Display(Name:="ConsecutivoUno", Description:="ConsecutivoUno")> _
            Public Property ConsecutivoUno As String

            <Required(ErrorMessage:="Este campo es requerido. (Numero)")> _
              <Display(Name:="Numero", Description:="Numero")> _
            Public Property Numero As Nullable(Of Integer)

            <Required(ErrorMessage:="Este campo es requerido. (haElaboracion)")> _
              <Display(Name:="haElaboracion", Description:="haElaboracion")> _
            Public Property haElaboracion As Nullable(Of DateTime)

            <Required(ErrorMessage:="Este campo es requerido. (ConsecutivoDos)")> _
              <Display(Name:="ConsecutivoDos", Description:="ConsecutivoDos")> _
            Public Property ConsecutivoDos As String

            <Required(ErrorMessage:="Este campo es requerido. (Banco)")> _
              <Display(Name:="Banco", Description:="Banco")> _
            Public Property Banco As Nullable(Of Integer)

            <Required(ErrorMessage:="Este campo es requerido. (CuentaContable)")> _
              <Display(Name:="CuentaContable", Description:="CuentaContable")> _
            Public Property CuentaContable As Nullable(Of Integer)

            <Required(ErrorMessage:="Este campo es requerido. (IDSaldarPagosDECEVAL)")> _
              <Display(Name:="IDSaldarPagosDECEVAL", Description:="IDSaldarPagosDECEVAL")> _
            Public Property IDSaldarPagosDECEVAL As Integer

        End Class
    End Class
#End Region
End Namespace



