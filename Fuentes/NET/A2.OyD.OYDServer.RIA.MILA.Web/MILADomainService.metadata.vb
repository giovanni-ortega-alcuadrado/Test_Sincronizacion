
Option Compare Binary
Option Infer On
Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Linq

Namespace OyDMILA

    <MetadataTypeAttribute(GetType(Liquidacione_MI.LiquidacioneMetadata))> _
    Partial Public Class Liquidacione_MI
        Friend NotInheritable Class LiquidacioneMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="IDComisionista", Description:="IDComisionista")> _
            Public Property IDComisionista As Integer

            <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
            Public Property IDSucComisionista As Integer

            <Required(ErrorMessage:="Este campo es requerido. (ID)")> _
              <Display(Name:="Liquidación")> _
            Public Property ID As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Parcial)")> _
              <Display(Name:="Parcial")> _
            Public Property Parcial As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Tipo)")> _
            <Display(Name:="Tipo")> _
            Public Property Tipo As String

            <Required(ErrorMessage:="Este campo es requerido. (ClaseOrden)")> _
              <Display(Name:="Clase")> _
            Public Property ClaseOrden As String

            <Required(ErrorMessage:="Este campo es requerido. (Manual)")> _
              <Display(Name:="Manual")> _
            Public Property Manual As Boolean

            <Required(ErrorMessage:="Este campo es requerido. (IDOrden)")> _
              <Display(Name:="Orden")> _
            Public Property IDOrden As Integer

            <Display(Name:="Factura")> _
            Public Property IDFactura As Nullable(Of Integer)

            <Display(Name:="Parc.Padre")> _
            Public Property ParcialPadre As Nullable(Of Integer)

            <Display(Name:="Diás Tramo")> _
            Public Property DiasTramo As Nullable(Of Integer)

            <Required(ErrorMessage:="Este campo es requerido. (IDComitente)")> _
              <Display(Name:=" ")> _
            Public Property IDComitente As String

            <Required(ErrorMessage:="Este campo es requerido. (IDOrdenante)")> _
              <Display(Name:=" ")> _
            Public Property IDOrdenante As String

            <Required(ErrorMessage:="Este campo es requerido. (IDEspecie)")> _
             <Display(Name:="Especie")> _
            Public Property IDEspecie As String

            <Required(ErrorMessage:="Este campo es requerido. (Liquidacion)")> _
              <Display(Name:="Fecha Liquidación")> _
            Public Property Liquidacion As DateTime

            <Display(Name:="Vendido", Description:="Vendido")> _
            Public Property Vendido_fecha As Nullable(Of DateTime)

            <Display(Name:="Título")> _
            Public Property CumplimientoTitulo As Nullable(Of DateTime)

            <Required(ErrorMessage:="Este campo es requerido. (Cumplimiento)")> _
              <Display(Name:="Efectivo")> _
            Public Property Cumplimiento As DateTime

            <Required(ErrorMessage:="Este campo es requerido. (IDBolsa)")> _
              <Display(Name:="Bolsa")> _
            Public Property IDBolsa As Integer

            <Display(Name:=" ")> _
            Public Property Plazo As Nullable(Of Integer)

            <Display(Name:=" ")> _
            Public Property DiasVencimiento As Nullable(Of Integer)

            <Required(ErrorMessage:="Este campo es requerido. (TasaEfectiva)")> _
            <Display(Name:=" ")> _
            Public Property TasaEfectiva As Double

            <Display(Name:=" ")> _
            Public Property Modalidad As String

            <Display(Name:=" ")> _
            Public Property Emision As Nullable(Of DateTime)

            <Display(Name:=" ")> _
            Public Property Vencimiento As Nullable(Of DateTime)

            <Display(Name:="Plaza")> _
            Public Property Plaza As String

            <Display(Name:="Ubicación")> _
            Public Property UBICACIONTITULO As String

            <Display(Name:="Comisionista local")> _
            Public Property IDComisionistaLocal As Nullable(Of Integer)

            <Display(Name:="Comisionista Otra Plaza")> _
            Public Property IDComisionistaOtraPlaza As Nullable(Of Integer)

            <Display(Name:="Ciudad Otra Plaza")> _
            Public Property IDCiudadOtraPlaza As Nullable(Of Integer)

            <Display(Name:="Precio")> _
            Public Property Precio As Nullable(Of Double)

            <Display(Name:="Valor")> _
            Public Property Transaccion_cur As Nullable(Of Double)

            <Display(Name:="Comisión")> _
            Public Property Comision As Nullable(Of Double)

            <Display(Name:="Subtotal")> _
            Public Property SubTotalLiq As Nullable(Of Double)

            <Display(Name:="Retención")> _
            Public Property Retencion As Nullable(Of Double)

            <Display(Name:="Intereses")> _
            Public Property Intereses As Nullable(Of Double)

            <Display(Name:="Serv Bolsa")> _
            Public Property ServBolsaFijo As Nullable(Of Double)

            <Display(Name:=" ")> _
            Public Property Traslado As String

            <Display(Name:="Cantidad")> _
            Public Property Cantidad As Nullable(Of Double)

            <Display(Name:="% Comisión")> _
            Public Property FactorComisionPactada As Nullable(Of Double)

            <Display(Name:="IVA Comisión")> _
            Public Property ValorIva As Nullable(Of Double)

            <Display(Name:="Indicador")> _
            Public Property IndicadorEconomico As String

            <Display(Name:="Puntos Indicador")> _
            Public Property PuntosIndicador As Nullable(Of Double)

            <Display(Name:="Traslado")> _
            Public Property ValorTraslado As Nullable(Of Double)

            <Display(Name:="Total")> _
            Public Property TotalLiq As Nullable(Of Double)

            <Display(Name:="Valor Entrega Contrapago")> _
            Public Property ValorEntregaContraPago As Nullable(Of Double)

            <Display(Name:="A quien se envía la retención")> _
            Public Property AquienSeEnviaRetencion As String

            <Display(Name:="Numero Swap")> _
            Public Property NroSwap As Nullable(Of Integer)







            <Editable(True, AllowInitialValue:=False)> _
            <[ReadOnly](False)> _
            <Required(ErrorMessage:="Este campo es requerido. (IDLiquidaciones)")> _
            <Display(Name:="IDLiquidaciones", Description:="IDLiquidaciones")> _
            Public Property IDLiquidaciones As Integer

            <Display(Name:="Prefijo", Description:="Prefijo")> _
            Public Property Prefijo As String

            <Display(Name:="Facturada", Description:="Facturada")> _
            Public Property Facturada As String

            <Display(Name:="ValBolsa", Description:="ValBolsa")> _
            Public Property ValBolsa As Nullable(Of Double)

            <Display(Name:="IDRueda", Description:="IDRueda")> _
            Public Property IDRueda As Nullable(Of Int16)

            <Display(Name:=" ", Description:="TasaDescuento")> _
            Public Property TasaDescuento As Nullable(Of Double)

            <Display(Name:="TasaCompraVende", Description:="TasaCompraVende")> _
            Public Property TasaCompraVende As Nullable(Of Double)

            <Required(ErrorMessage:="Este campo es requerido. (OtraPlaza)")> _
              <Display(Name:="OtraPlaza", Description:="OtraPlaza")> _
            Public Property OtraPlaza As Boolean

            <Display(Name:="DiasIntereses", Description:="DiasIntereses")> _
            Public Property DiasIntereses As Nullable(Of Integer)

            <Display(Name:="Mercado", Description:="Mercado")> _
            Public Property Mercado As String

            <Display(Name:="NroTitulo", Description:="NroTitulo")> _
            Public Property NroTitulo As String

            <Display(Name:="IDCiudadExpTitulo", Description:="IDCiudadExpTitulo")> _
            Public Property IDCiudadExpTitulo As Nullable(Of Integer)

            <Display(Name:="PlazoOriginal", Description:="PlazoOriginal")> _
            Public Property PlazoOriginal As Nullable(Of Integer)

            <Required(ErrorMessage:="Este campo es requerido. (Aplazamiento)")> _
              <Display(Name:="Aplazamiento", Description:="Aplazamiento")> _
            Public Property Aplazamiento As Boolean

            <Display(Name:="VersionPapeleta", Description:="VersionPapeleta")> _
            Public Property VersionPapeleta As Nullable(Of Int16)

            <Display(Name:="EmisionOriginal", Description:="EmisionOriginal")> _
            Public Property EmisionOriginal As Nullable(Of DateTime)

            <Display(Name:="VencimientoOriginal", Description:="VencimientoOriginal")> _
            Public Property VencimientoOriginal As Nullable(Of DateTime)

            <Display(Name:="Impresiones", Description:="Impresiones")> _
            Public Property Impresiones As Nullable(Of Integer)

            <Display(Name:="FormaPago", Description:="FormaPago")> _
            Public Property FormaPago As String

            <Display(Name:="CtrlImpPapeleta", Description:="CtrlImpPapeleta")> _
            Public Property CtrlImpPapeleta As Nullable(Of Integer)

            <Display(Name:="PosicionPropia", Description:="PosicionPropia")> _
            Public Property PosicionPropia As String

            <Display(Name:="Transaccion", Description:="Transaccion")> _
            Public Property Transaccion As String

            <Display(Name:="TipoOperacion", Description:="TipoOperacion")> _
            Public Property TipoOperacion As String

            <Display(Name:="DiasContado", Description:="DiasContado")> _
            Public Property DiasContado As Nullable(Of Integer)

            <Display(Name:="Ordinaria", Description:="Ordinaria")> _
            Public Property Ordinaria As Boolean

            <Display(Name:="ObjetoOrdenExtraordinaria", Description:="ObjetoOrdenExtraordinaria")> _
            Public Property ObjetoOrdenExtraordinaria As String

            <Display(Name:="NumPadre", Description:="NumPadre")> _
            Public Property NumPadre As Nullable(Of Integer)

            <Display(Name:="OperacionPadre", Description:="OperacionPadre")> _
            Public Property OperacionPadre As Nullable(Of DateTime)

            <Required(ErrorMessage:="Este campo es requerido. (Vendido)")> _
              <Display(Name:="Vendido", Description:="Vendido")> _
            Public Property Vendido As Boolean

            <Display(Name:="ValorBrutoCompraVencida", Description:="ValorBrutoCompraVencida")> _
            Public Property ValorBrutoCompraVencida As Nullable(Of Double)


            <Required(ErrorMessage:="Este campo es requerido. (AutoRetenedor)")> _
            <Display(Name:="AutoRetenedor", Description:="AutoRetenedor")> _
            Public Property AutoRetenedor As String

            <Required(ErrorMessage:="Este campo es requerido. (Sujeto)")> _
            <Display(Name:="Sujeto", Description:="Sujeto")> _
            Public Property Sujeto As String

            <Display(Name:="PcRenEfecCompraRet", Description:="PcRenEfecCompraRet")> _
            Public Property PcRenEfecCompraRet As Nullable(Of Double)

            <Display(Name:="PcRenEfecVendeRet", Description:="PcRenEfecVendeRet")> _
            Public Property PcRenEfecVendeRet As Nullable(Of Double)

            <Required(ErrorMessage:="Este campo es requerido. (Reinversion)")> _
              <Display(Name:="Reinversion", Description:="Reinversion")> _
            Public Property Reinversion As String

            <Required(ErrorMessage:="Este campo es requerido. (Swap)")> _
              <Display(Name:="Swap", Description:="Swap")> _
            Public Property Swap As String

            <Required(ErrorMessage:="Este campo es requerido. (Certificacion)")> _
            <Display(Name:="Certificacion", Description:="Certificacion")> _
            Public Property Certificacion As String

            <Display(Name:="DescuentoAcumula", Description:="DescuentoAcumula")> _
            Public Property DescuentoAcumula As Nullable(Of Double)

            <Display(Name:="PctRendimiento", Description:="PctRendimiento")> _
            Public Property PctRendimiento As Nullable(Of Double)

            <Display(Name:="FechaCompraVencido", Description:="FechaCompraVencido")> _
            Public Property FechaCompraVencido As Nullable(Of DateTime)

            <Display(Name:="PrecioCompraVencido", Description:="PrecioCompraVencido")> _
            Public Property PrecioCompraVencido As Nullable(Of Double)

            <Required(ErrorMessage:="Este campo es requerido. (ConstanciaEnajenacion)")> _
            <Display(Name:="ConstanciaEnajenacion", Description:="ConstanciaEnajenacion")> _
            Public Property ConstanciaEnajenacion As String

            <Required(ErrorMessage:="Este campo es requerido. (RepoTitulo)")> _
            <Display(Name:="RepoTitulo", Description:="RepoTitulo")> _
            Public Property RepoTitulo As String

            <Display(Name:="ServBolsaVble", Description:="ServBolsaVble")> _
            Public Property ServBolsaVble As Nullable(Of Double)

            <Display(Name:="HoraGrabacion", Description:="HoraGrabacion")> _
            Public Property HoraGrabacion As String

            <Display(Name:="OrigenOperacion", Description:="OrigenOperacion")> _
            Public Property OrigenOperacion As String

            <Display(Name:="CodigoOperadorCompra", Description:="CodigoOperadorCompra")> _
            Public Property CodigoOperadorCompra As Nullable(Of Integer)

            <Display(Name:="CodigoOperadorVende", Description:="CodigoOperadorVende")> _
            Public Property CodigoOperadorVende As Nullable(Of Integer)

            <Display(Name:="IdentificacionRemate", Description:="IdentificacionRemate")> _
            Public Property IdentificacionRemate As String

            <Display(Name:="ModalidaOperacion", Description:="ModalidaOperacion")> _
            Public Property ModalidaOperacion As String

            <Display(Name:="IndicadorPrecio", Description:="IndicadorPrecio")> _
            Public Property IndicadorPrecio As String

            <Display(Name:="PeriodoExdividendo", Description:="PeriodoExdividendo")> _
            Public Property PeriodoExdividendo As String

            <Display(Name:="PlazoOperacionRepo", Description:="PlazoOperacionRepo")> _
            Public Property PlazoOperacionRepo As Nullable(Of Integer)

            <Display(Name:="ValorCaptacionRepo", Description:="ValorCaptacionRepo")> _
            Public Property ValorCaptacionRepo As Nullable(Of Double)

            <Display(Name:="VolumenCompraRepo", Description:="VolumenCompraRepo")> _
            Public Property VolumenCompraRepo As Nullable(Of Double)

            <Display(Name:="PrecioNetoFraccion", Description:="PrecioNetoFraccion")> _
            Public Property PrecioNetoFraccion As Nullable(Of Double)

            <Display(Name:="VolumenNetoFraccion", Description:="VolumenNetoFraccion")> _
            Public Property VolumenNetoFraccion As Nullable(Of Double)

            <Display(Name:="CodigoContactoComercial", Description:="CodigoContactoComercial")> _
            Public Property CodigoContactoComercial As String

            <Display(Name:="NroFraccionOperacion", Description:="NroFraccionOperacion")> _
            Public Property NroFraccionOperacion As Nullable(Of Integer)

            <Display(Name:="IdentificacionPatrimonio1", Description:="IdentificacionPatrimonio1")> _
            Public Property IdentificacionPatrimonio1 As String

            <Display(Name:="TipoidentificacionCliente2", Description:="TipoidentificacionCliente2")> _
            Public Property TipoidentificacionCliente2 As String

            <Display(Name:="NitCliente2", Description:="NitCliente2")> _
            Public Property NitCliente2 As String

            <Display(Name:="IdentificacionPatrimonio2", Description:="IdentificacionPatrimonio2")> _
            Public Property IdentificacionPatrimonio2 As String

            <Display(Name:="TipoIdentificacionCliente3", Description:="TipoIdentificacionCliente3")> _
            Public Property TipoIdentificacionCliente3 As String

            <Display(Name:="NitCliente3", Description:="NitCliente3")> _
            Public Property NitCliente3 As String

            <Display(Name:="IdentificacionPatrimonio3", Description:="IdentificacionPatrimonio3")> _
            Public Property IdentificacionPatrimonio3 As String

            <Display(Name:="IndicadorOperacion", Description:="IndicadorOperacion")> _
            Public Property IndicadorOperacion As String

            <Display(Name:="BaseRetencion", Description:="BaseRetencion")> _
            Public Property BaseRetencion As Nullable(Of Double)

            <Display(Name:="PorcRetencion", Description:="PorcRetencion")> _
            Public Property PorcRetencion As Nullable(Of Double)

            <Display(Name:="BaseRetencionTranslado", Description:="BaseRetencionTranslado")> _
            Public Property BaseRetencionTranslado As Nullable(Of Double)

            <Display(Name:="PorcRetencionTranslado", Description:="PorcRetencionTranslado")> _
            Public Property PorcRetencionTranslado As Nullable(Of Double)

            <Display(Name:="PorcIvaComision", Description:="PorcIvaComision")> _
            Public Property PorcIvaComision As Nullable(Of Double)

            <Display(Name:="IndicadorAcciones", Description:="IndicadorAcciones")> _
            Public Property IndicadorAcciones As String

            <Display(Name:="OperacionNegociada", Description:="OperacionNegociada")> _
            Public Property OperacionNegociada As String

            <Display(Name:="FechaConstancia", Description:="FechaConstancia")> _
            Public Property FechaConstancia As Nullable(Of DateTime)

            <Display(Name:="ValorConstancia", Description:="ValorConstancia")> _
            Public Property ValorConstancia As Nullable(Of Double)

            <Display(Name:="GeneraConstancia", Description:="GeneraConstancia")> _
            Public Property GeneraConstancia As String

            <Required(ErrorMessage:="Este campo es requerido. (Cargado)")> _
              <Display(Name:="Cargado", Description:="Cargado")> _
            Public Property Cargado As Boolean

            <Display(Name:="Actualizacion", Description:="Actualizacion")> _
            Public Property Actualizacion As DateTime

            <Required(ErrorMessage:="Este campo es requerido. (Usuario)")> _
              <Display(Name:="Usuario", Description:="Usuario")> _
            Public Property Usuario As String

            <Required(ErrorMessage:="Este campo es requerido. (NroLote)")> _
              <Display(Name:="NroLote", Description:="NroLote")> _
            Public Property NroLote As Integer

            <Required(ErrorMessage:="Este campo es requerido. (IDBaseDias)")> _
              <Display(Name:="IDBaseDias", Description:="IDBaseDias")> _
            Public Property IDBaseDias As String

            <Display(Name:="TipoDeOferta", Description:="TipoDeOferta")> _
            Public Property TipoDeOferta As String

            <Display(Name:="NroLoteENC", Description:="NroLoteENC")> _
            Public Property NroLoteENC As Nullable(Of Integer)

            <Display(Name:="ContabilidadENC", Description:="ContabilidadENC")> _
            Public Property ContabilidadENC As Nullable(Of DateTime)

            <Display(Name:="CodigoIntermediario", Description:="CodigoIntermediario")> _
            Public Property CodigoIntermediario As String

            <Display(Name:="Nro. en mercado Extranjero")> _
            Public Property OperacionEnMercadoExtranjero As String


            <Display(Name:="Valor Costos")> _
            Public Property ValorCostos As Double

            <Display(Name:="Valor Neto")> _
            Public Property ValorNeto As Double

            <Display(Name:="Valor Bruto")> _
            Public Property ValorBruto As Double

            <Display(Name:="Valor Comisión")> _
            Public Property ComisionPesos As Double


            <Display(Name:="Iva Comisión")> _
            Public Property ValorIVAComisionPesos As Double


            <Display(Name:="Valor Costos")> _
            Public Property ValorCostosPesos As Double


            <Display(Name:="Valor Neto")> _
            Public Property ValorNetoPesos As Double

            <Display(Name:=" ")> _
            Public Property IDMoneda As Integer


        End Class
    End Class
    <MetadataTypeAttribute(GetType(Factura_MI.FacturaMetadata))> _
    Partial Public Class Factura_MI
        Friend NotInheritable Class FacturaMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="IDComisionista", Description:="IDComisionista")> _
            Public Property IDComisionista As Integer

            <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
            Public Property IDSucComisionista As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Prefijo)")> _
              <Display(Name:="Prefijo", Description:="Prefijo")> _
            Public Property Prefijo As String

            <Required(ErrorMessage:="Este campo es requerido. (Numero)")> _
              <Display(Name:="Número", Description:="Número")> _
            Public Property Numero As Integer

            <Display(Name:="Número", Description:="Número")> _
            Public Property Prefijo_Numero As String

            <Required(ErrorMessage:="Este campo es requerido. (Comitente)")> _
              <Display(Name:="Comitente", Description:="Comitente")> _
            Public Property Comitente As String

            <Required(ErrorMessage:="Este campo es requerido. (Documento)")> _
              <Display(Name:="Fecha Documento", Description:="Fecha Documento")> _
            Public Property Fecha_Documento As DateTime

            <Required(ErrorMessage:="Este campo es requerido. (Estado)")> _
              <Display(Name:="Estado", Description:="Estado")> _
            Public Property Estado As String

            <Required(ErrorMessage:="Este campo es requerido. (Estado)")> _
              <Display(Name:="Fecha Estado", Description:="Fecha Estado")> _
            Public Property Fecha_Estado As DateTime

            <Required(ErrorMessage:="Este campo es requerido. (Impresiones)")> _
              <Display(Name:="Impresiones", Description:="Número de Impresiones")> _
            Public Property Impresiones As Integer

            <Display(Name:="Actualizacion", Description:="Actualizacion")> _
            Public Property Actualizacion As DateTime

            <Required(ErrorMessage:="Este campo es requerido. (Usuario)")> _
              <Display(Name:="Usuario", Description:="Usuario")> _
            Public Property Usuario As String

            <Display(Name:="IDCodigoResolucion", Description:="IDCodigoResolucion")> _
            Public Property IDCodigoResolucion As Nullable(Of Integer)

            <Display(Name:="IDfacturas", Description:="IDfacturas")> _
            Public Property IDfacturas As Integer

        End Class
    End Class
    <MetadataTypeAttribute(GetType(ReceptoresOrdene_MI.ReceptoresOrdeneMetadata))> _
    Partial Public Class ReceptoresOrdene_MI
        Friend NotInheritable Class ReceptoresOrdeneMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="Código", Description:="Código")> _
            <Required(ErrorMessage:="El código de receptor no puede ser vacío.")> _
            Public Property IDReceptor As String

            <Display(Name:=" ")> _
            Public Property Lider As Boolean

            <Display(Name:=" ")> _
            Public Property Porcentaje As Double

            <Display(Name:=" ")> _
            <Required(ErrorMessage:="El nombre del receptor no puede ser vacío, este se establece seleccionando el código del receptor.")> _
            Public Property Nombre As String

            <Display(Name:="IDReceptoresOrdenes", Description:="IDReceptoresOrdenes")> _
            Public Property IDReceptoresOrdenes As Integer

        End Class
    End Class
    <MetadataTypeAttribute(GetType(BeneficiariosOrdene_MI.BeneficiariosOrdeneMetadata))> _
    Partial Public Class BeneficiariosOrdene_MI
        Friend NotInheritable Class BeneficiariosOrdeneMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="Nombre")> _
            Public Property Nombre As String

            <Display(Name:="Documento")> _
            Public Property TipoIdentificacion As Boolean

            <Display(Name:="Número")> _
            Public Property NroDocumento As Double

            <Display(Name:="Parentesco")> _
            Public Property Parentesco As String

        End Class
    End Class
    <MetadataTypeAttribute(GetType(EspeciesLiquidacione_MI.EspeciesLiquidacioneMetadata))> _
    Partial Public Class EspeciesLiquidacione_MI
        Friend NotInheritable Class EspeciesLiquidacioneMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="IDEspecie")> _
            Public Property IDEspecie As String

            <Display(Name:="Nombre")> _
            Public Property Nombre As String

            <Display(Name:="Descripcion")> _
            Public Property Descripcion As String


        End Class
    End Class
    <MetadataTypeAttribute(GetType(AplazamientosLiquidacione_MI.AplazamientosLiquidacioneMetadata))> _
    Partial Public Class AplazamientosLiquidacione_MI
        Friend NotInheritable Class AplazamientosLiquidacioneMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="Aplazamiento")> _
            Public Property Aplazamiento As String

            <Display(Name:="Cumplimiento")> _
            Public Property Cumplimiento As String

            <Display(Name:="Actualización")> _
            Public Property Actualizacion As String

            <Display(Name:="Usuario")> _
            Public Property Usuario As String

        End Class
    End Class
    <MetadataTypeAttribute(GetType(CustodiasLiquidacione_MI.CustodiasLiquidacioneMetadata))> _
    Partial Public Class CustodiasLiquidacione_MI
        Friend NotInheritable Class CustodiasLiquidacioneMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="Custodia")> _
            Public Property IdRecibo As Integer

            <Display(Name:="Secuencia")> _
            Public Property Secuencia As Integer

            <Display(Name:="Estado Custodia")> _
            Public Property Estado As String

            <Display(Name:="Estado Titulo")> _
            Public Property EstadoActual As String

            <Display(Name:="IDCustodia")> _
            Public Property IDCustodia As Integer

        End Class
    End Class

#Region "ImportacionLiq_MI"

    <MetadataTypeAttribute(GetType(ImportacionLiq_MI.ImportacionLiq_MIMetadata))> _
    Partial Public Class ImportacionLiq_MI

        Friend NotInheritable Class ImportacionLiq_MIMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="IDComisionista", Description:="IDComisionista")> _
            Public Property IDComisionista As Nullable(Of Integer)

            <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
            Public Property IDSucComisionista As Nullable(Of Integer)

            <Display(Name:="Liquidación")> _
            Public Property ID As Nullable(Of Integer)

            <Display(Name:="Parcial")> _
            Public Property Parcial As Nullable(Of Integer)

            <Display(Name:="Tipo")> _
            Public Property Tipo As String

            <Display(Name:="Clase Orden")> _
            Public Property ClaseOrden As String

            <Display(Name:="Especie")> _
            Public Property IDEspecie As String

            <Display(Name:="Orden")>
            <A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} sólo permite números")>
            Public Property IDOrden As Nullable(Of Integer)

            <Display(Name:="Comitente", Description:="IDComitente")> _
            Public Property IDComitente As String

            <Display(Name:="Ordenante", Description:="IDOrdenante")> _
            Public Property IDOrdenante As String

            <Display(Name:="Bolsa", Description:="IDBolsa")> _
            Public Property IDBolsa As Nullable(Of Integer)

            <Display(Name:="Rueda", Description:="IDRueda")> _
            Public Property IDRueda As Nullable(Of Int16)

            <Display(Name:="Va lBolsa", Description:="ValBolsa")> _
            Public Property ValBolsa As Nullable(Of Double)

            <Display(Name:="Tasa Descuento", Description:="TasaDescuento")> _
            Public Property TasaDescuento As Nullable(Of Double)

            <Display(Name:="TasaCompraVende", Description:="TasaCompraVende")> _
            Public Property TasaCompraVende As Nullable(Of Double)

            <Display(Name:="Modalidad", Description:="Modalidad")> _
            Public Property Modalidad As String

            <Display(Name:="IndicadorEconomico", Description:="IndicadorEconomico")> _
            Public Property IndicadorEconomico As String

            <Display(Name:="Puntos Indicador", Description:="PuntosIndicador")> _
            Public Property PuntosIndicador As Nullable(Of Double)

            <Display(Name:="Plazo", Description:="Plazo")> _
            Public Property Plazo As Nullable(Of Integer)

            <Display(Name:="Fecha Liquidación")> _
            Public Property Liquidacion As Nullable(Of DateTime)

            <Display(Name:="Fecha Cumplimiento")> _
            Public Property Cumplimiento As Nullable(Of DateTime)

            <Display(Name:="Emision", Description:="Emision")> _
            Public Property Emision As Nullable(Of DateTime)

            <Display(Name:="Vencimiento", Description:="Vencimiento")> _
            Public Property Vencimiento As Nullable(Of DateTime)

            <Display(Name:="Otra Plaza", Description:="OtraPlaza")> _
            Public Property OtraPlaza As Boolean

            <Display(Name:="Plaza", Description:="Plaza")> _
            Public Property Plaza As String

            <Display(Name:="ComisionistaLocal", Description:="IDComisionistaLocal")> _
            Public Property IDComisionistaLocal As Nullable(Of Integer)

            <Display(Name:="ComisionistaOtraPlaza", Description:="IDComisionistaOtraPlaza")> _
            Public Property IDComisionistaOtraPlaza As Nullable(Of Integer)

            <Display(Name:="CiudadOtraPlaza", Description:="IDCiudadOtraPlaza")> _
            Public Property IDCiudadOtraPlaza As Nullable(Of Integer)

            <Display(Name:="Tasa Efectiva", Description:="TasaEfectiva")> _
            Public Property TasaEfectiva As Nullable(Of Double)

            <Display(Name:="Cantidad")> _
            Public Property Cantidad As Nullable(Of Double)

            <Display(Name:="Precio")> _
            Public Property Precio As Nullable(Of Double)

            <Display(Name:="Valor")> _
            Public Property Transaccion_cur As Nullable(Of Double)

            <Display(Name:="Sub Total")> _
            Public Property SubTotalLiq As Nullable(Of Double)

            <Display(Name:="Total")> _
            Public Property TotalLiq As Nullable(Of Double)

            <Display(Name:="Comisión")> _
            Public Property Comision As Nullable(Of Double)

            <Display(Name:="Retención")> _
            Public Property Retencion As Nullable(Of Double)


            <Display(Name:="Intereses")> _
            Public Property Intereses As Nullable(Of Double)

            <Display(Name:="Valor IVA")> _
            Public Property ValorIva As Nullable(Of Double)

            <Display(Name:="Dias Intereses")> _
            Public Property DiasIntereses As Nullable(Of Integer)

            <Display(Name:="Factor Comision Pactada", Description:="FactorComisionPactada")> _
            Public Property FactorComisionPactada As Nullable(Of Double)

            <Display(Name:="Mercado", Description:="Mercado")> _
            Public Property Mercado As String

            <Display(Name:="Nro Titulo", Description:="NroTitulo")> _
            Public Property NroTitulo As String

            <Display(Name:="CiudadExpTitulo", Description:="IDCiudadExpTitulo")> _
            Public Property IDCiudadExpTitulo As Nullable(Of Integer)

            <Display(Name:="Plazo Original", Description:="PlazoOriginal")> _
            Public Property PlazoOriginal As Nullable(Of Integer)

            <Display(Name:="Aplazamiento", Description:="Aplazamiento")> _
            Public Property Aplazamiento As Boolean

            <Display(Name:="Version Papeleta", Description:="VersionPapeleta")> _
            Public Property VersionPapeleta As Nullable(Of Int16)

            <Display(Name:="Emision Original", Description:="EmisionOriginal")> _
            Public Property EmisionOriginal As Nullable(Of DateTime)

            <Display(Name:="Vencimiento Original", Description:="VencimientoOriginal")> _
            Public Property VencimientoOriginal As Nullable(Of DateTime)

            <Display(Name:="Impresiones", Description:="Impresiones")> _
            Public Property Impresiones As Nullable(Of Integer)

            <Display(Name:="FormaPago", Description:="FormaPago")> _
            Public Property FormaPago As String

            <Display(Name:="CtrlImpPapeleta", Description:="CtrlImpPapeleta")> _
            Public Property CtrlImpPapeleta As Nullable(Of Integer)

            <Display(Name:="DiasVencimiento", Description:="DiasVencimiento")> _
            Public Property DiasVencimiento As Nullable(Of Integer)

            <Display(Name:="PosicionPropia", Description:="PosicionPropia")> _
            Public Property PosicionPropia As String

            <Display(Name:="Transaccion", Description:="Transaccion")> _
            Public Property Transaccion_str As String

            <Display(Name:="Tipo Operacion", Description:="TipoOperacion")> _
            Public Property TipoOperacion As String

            <Display(Name:="DiasContado", Description:="DiasContado")> _
            Public Property DiasContado As Nullable(Of Integer)

            <Display(Name:="Ordinaria", Description:="Ordinaria")> _
            Public Property Ordinaria As Boolean

            <Display(Name:="ObjetoOrdenExtraordinaria", Description:="ObjetoOrdenExtraordinaria")> _
            Public Property ObjetoOrdenExtraordinaria As String

            <Display(Name:="NumPadre", Description:="NumPadre")> _
            Public Property NumPadre As Nullable(Of Integer)

            <Display(Name:="ParcialPadre", Description:="ParcialPadre")> _
            Public Property ParcialPadre As Nullable(Of Integer)

            <Display(Name:="OperacionPadre", Description:="OperacionPadre")> _
            Public Property OperacionPadre As Nullable(Of DateTime)

            <Display(Name:="DiasTramo", Description:="DiasTramo")> _
            Public Property DiasTramo As Nullable(Of Integer)

            <Display(Name:="Vendido", Description:="Vendido")> _
            Public Property Vendido_log As Boolean

            <Display(Name:="Vendido", Description:="Vendido")> _
            Public Property Vendido_dtm As Nullable(Of DateTime)

            <Display(Name:="ValorTraslado", Description:="ValorTraslado")> _
            Public Property ValorTraslado As Nullable(Of Double)

            <Display(Name:="ValorBrutoCompraVencida", Description:="ValorBrutoCompraVencida")> _
            Public Property ValorBrutoCompraVencida As Nullable(Of Double)

            '<Required(ErrorMessage:="Este campo es requerido. (AutoRetenedor)")>
            '<Display(Name:="AutoRetenedor", Description:="AutoRetenedor")> _
            'Public Property AutoRetenedor As String

            '<Display(Name:="Sujeto", Description:="Sujeto")> _
            'Public Property Sujeto As String

            <Display(Name:="PcRenEfecCompraRet", Description:="PcRenEfecCompraRet")> _
            Public Property PcRenEfecCompraRet As Nullable(Of Double)

            <Display(Name:="PcRenEfecVendeRet", Description:="PcRenEfecVendeRet")> _
            Public Property PcRenEfecVendeRet As Nullable(Of Double)

            <Display(Name:="Reinversion", Description:="Reinversion")> _
            Public Property Reinversion As String

            <Display(Name:="Swap", Description:="Swap")> _
            Public Property Swap As String

            <Display(Name:="NroSwap", Description:="NroSwap")> _
            Public Property NroSwap As Nullable(Of Integer)

            '<Display(Name:="Certificacion", Description:="Certificacion")> _
            'Public Property Certificacion As String

            <Display(Name:="DescuentoAcumula", Description:="DescuentoAcumula")> _
            Public Property DescuentoAcumula As Nullable(Of Double)

            <Display(Name:="PctRendimiento", Description:="PctRendimiento")> _
            Public Property PctRendimiento As Nullable(Of Double)

            <Display(Name:="FechaCompraVencido", Description:="FechaCompraVencido")> _
            Public Property FechaCompraVencido As Nullable(Of DateTime)

            <Display(Name:="PrecioCompraVencido", Description:="PrecioCompraVencido")> _
            Public Property PrecioCompraVencido As Nullable(Of Double)

            '<Display(Name:="ConstanciaEnajenacion", Description:="ConstanciaEnajenacion")> _
            'Public Property ConstanciaEnajenacion As String

            '<Display(Name:="RepoTitulo", Description:="RepoTitulo")> _
            'Public Property RepoTitulo As String

            <Display(Name:="ServBolsaVble", Description:="ServBolsaVble")> _
            Public Property ServBolsaVble As Nullable(Of Double)

            <Display(Name:="Serv. Bolsa")> _
            Public Property ServBolsaFijo As Nullable(Of Double)

            <Display(Name:="Traslado", Description:="Traslado")> _
            Public Property Traslado As String

            <Display(Name:="UBICACIONTITULO", Description:="UBICACIONTITULO")> _
            Public Property UBICACIONTITULO As String

            <Display(Name:="Tipo Identificacion", Description:="TipoIdentificacion")> _
            Public Property TipoIdentificacion As String

            <Display(Name:="Nro Documento", Description:="NroDocumento")> _
            Public Property NroDocumento As String

            <Display(Name:="Valor Entrega ContraPago", Description:="ValorEntregaContraPago")> _
            Public Property ValorEntregaContraPago As Nullable(Of Double)

            <Display(Name:="AquienSeEnviaRetencion", Description:="AquienSeEnviaRetencion")> _
            Public Property AquienSeEnviaRetencion As String

            <Display(Name:="IDBaseDias", Description:="IDBaseDias")> _
            Public Property IDBaseDias As String

            <Display(Name:="Tipo De Oferta", Description:="TipoDeOferta")> _
            Public Property TipoDeOferta As String

            <Display(Name:="Hora Grabacion", Description:="HoraGrabacion")> _
            Public Property HoraGrabacion As String

            <Display(Name:="Origen Operacion", Description:="OrigenOperacion")> _
            Public Property OrigenOperacion As String

            <Display(Name:="CodigoOperadorCompra", Description:="CodigoOperadorCompra")> _
            Public Property CodigoOperadorCompra As Nullable(Of Integer)

            <Display(Name:="CodigoOperadorVende", Description:="CodigoOperadorVende")> _
            Public Property CodigoOperadorVende As Nullable(Of Integer)

            <Display(Name:="IdentificacionRemate", Description:="IdentificacionRemate")> _
            Public Property IdentificacionRemate As String

            <Display(Name:="ModalidaOperacion", Description:="ModalidaOperacion")> _
            Public Property ModalidaOperacion As String

            <Display(Name:="IndicadorPrecio", Description:="IndicadorPrecio")> _
            Public Property IndicadorPrecio As String

            <Display(Name:="PeriodoExdividendo", Description:="PeriodoExdividendo")> _
            Public Property PeriodoExdividendo As String

            <Display(Name:="PlazoOperacionRepo", Description:="PlazoOperacionRepo")> _
            Public Property PlazoOperacionRepo As Nullable(Of Integer)

            <Display(Name:="ValorCaptacionRepo", Description:="ValorCaptacionRepo")> _
            Public Property ValorCaptacionRepo As Nullable(Of Double)

            <Display(Name:="VolumenCompraRepo", Description:="VolumenCompraRepo")> _
            Public Property VolumenCompraRepo As Nullable(Of Double)

            <Display(Name:="PrecioNetoFraccion", Description:="PrecioNetoFraccion")> _
            Public Property PrecioNetoFraccion As Nullable(Of Double)

            <Display(Name:="VolumenNetoFraccion", Description:="VolumenNetoFraccion")> _
            Public Property VolumenNetoFraccion As Nullable(Of Double)

            <Display(Name:="CodigoContactoComercial", Description:="CodigoContactoComercial")> _
            Public Property CodigoContactoComercial As String

            <Display(Name:="NroFraccionOperacion", Description:="NroFraccionOperacion")> _
            Public Property NroFraccionOperacion As Nullable(Of Integer)

            <Display(Name:="IdentificacionPatrimonio1", Description:="IdentificacionPatrimonio1")> _
            Public Property IdentificacionPatrimonio1 As String

            <Display(Name:="TipoidentificacionCliente2", Description:="TipoidentificacionCliente2")> _
            Public Property TipoidentificacionCliente2 As String

            <Display(Name:="NitCliente2", Description:="NitCliente2")> _
            Public Property NitCliente2 As String

            <Display(Name:="IdentificacionPatrimonio2", Description:="IdentificacionPatrimonio2")> _
            Public Property IdentificacionPatrimonio2 As String

            <Display(Name:="TipoIdentificacionCliente3", Description:="TipoIdentificacionCliente3")> _
            Public Property TipoIdentificacionCliente3 As String

            <Display(Name:="NitCliente3", Description:="NitCliente3")> _
            Public Property NitCliente3 As String

            <Display(Name:="IdentificacionPatrimonio3", Description:="IdentificacionPatrimonio3")> _
            Public Property IdentificacionPatrimonio3 As String

            <Display(Name:="IndicadorOperacion", Description:="IndicadorOperacion")> _
            Public Property IndicadorOperacion As String

            <Display(Name:="BaseRetencion", Description:="BaseRetencion")> _
            Public Property BaseRetencion As Nullable(Of Double)

            <Display(Name:="PorcRetencion", Description:="PorcRetencion")> _
            Public Property PorcRetencion As Nullable(Of Double)

            <Display(Name:="BaseRetencionTranslado", Description:="BaseRetencionTranslado")> _
            Public Property BaseRetencionTranslado As Nullable(Of Double)

            <Display(Name:="PorcRetencionTranslado", Description:="PorcRetencionTranslado")> _
            Public Property PorcRetencionTranslado As Nullable(Of Double)

            <Display(Name:="PorcIvaComision", Description:="PorcIvaComision")> _
            Public Property PorcIvaComision As Nullable(Of Double)

            <Display(Name:="IndicadorAcciones", Description:="IndicadorAcciones")> _
            Public Property IndicadorAcciones As String

            <Display(Name:="OperacionNegociada", Description:="OperacionNegociada")> _
            Public Property OperacionNegociada As String

            <Display(Name:="FechaConstancia", Description:="FechaConstancia")> _
            Public Property FechaConstancia As Nullable(Of DateTime)

            <Display(Name:="ValorConstancia", Description:="ValorConstancia")> _
            Public Property ValorConstancia As Nullable(Of Double)

            <Display(Name:="GeneraConstancia", Description:="GeneraConstancia")> _
            Public Property GeneraConstancia As String

            <Display(Name:="Actualizacion", Description:="Actualizacion")> _
            Public Property Actualizacion As Nullable(Of DateTime)

            <Display(Name:="Usuario", Description:="Usuario")> _
            Public Property Usuario As String

            <Display(Name:="CodigoIntermediario", Description:="CodigoIntermediario")> _
            Public Property CodigoIntermediario As String

            <Display(Name:="Nombre", Description:="Nombre")> _
            Public Property Nombre As String

            <Display(Name:="Clase")> _
            Public Property Clase As String

            <Display(Name:="Tipo Orden")> _
            Public Property TipoOrden As String

        End Class
    End Class

#End Region


End Namespace

