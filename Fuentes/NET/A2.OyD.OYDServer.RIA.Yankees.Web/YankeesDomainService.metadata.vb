Option Compare Binary
Option Infer On
Option Strict On
Option Explicit On

Imports A2.OyD.OYDServer.RIA.Web
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Data.Linq
Imports System.Linq

Namespace OyDYankees
    <MetadataTypeAttribute(GetType(tblLiquidaciones_YANKEE.tblLiquidaciones_YANKEEMetadata))> _
    Partial Public Class tblLiquidaciones_YANKEE
        Friend NotInheritable Class tblLiquidaciones_YANKEEMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            '<Display(Name:="IDComisionista")> _
            'Public Property IDComisionista As Integer

            '<Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
            'Public Property IDSucComisionista As Integer

            '<Required(ErrorMessage:="Este campo es requerido. (ID)")> _
            <Display(Name:="Liquidación")> _
            Public Property IDY As Integer

            '<Required(ErrorMessage:="Este campo es requerido. (Parcial)")> _
            '  <Display(Name:="Parcial")> _
            'Public Property Parcial As Integer

            '<Required(ErrorMessage:="Este campo es requerido. (Tipo)")> _
            '<Display(Name:="Tipo")> _
            'Public Property Tipo As String

            '<Required(ErrorMessage:="Este campo es requerido. (ClaseOrden)")> _
            '  <Display(Name:="Clase")> _
            'Public Property ClaseOrden As String

            '<Required(ErrorMessage:="Este campo es requerido. (Manual)")> _
            '  <Display(Name:="Manual")> _
            'Public Property Manual As Boolean

            '<Required(ErrorMessage:="Este campo es requerido. (IDOrden)")> _
            '  <Display(Name:="Orden")> _
            'Public Property IDOrden As Integer

            '<Display(Name:="Factura")> _
            'Public Property IDFactura As Nullable(Of Integer)

            '<Display(Name:="Parc.Padre")> _
            'Public Property ParcialPadre As Nullable(Of Integer)

            '<Display(Name:="Días Tramo")> _
            'Public Property DiasTramo As Nullable(Of Integer)

            '<Required(ErrorMessage:="Este campo es requerido. (IDComitente)")> _
            '  <Display(Name:=" ")> _
            'Public Property IDComitente As String

            '<Required(ErrorMessage:="Este campo es requerido. (IDOrdenante)")> _
            '  <Display(Name:=" ")> _
            'Public Property IDOrdenante As String

            '<Required(ErrorMessage:="Este campo es requerido. (IDEspecie)")> _
            '  <Display(Name:=" ")> _
            'Public Property IDEspecie As String

            '<Required(ErrorMessage:="Este campo es requerido. (Liquidacion)")> _
            '  <Display(Name:="Liquidación")> _
            'Public Property Liquidacion As DateTime

            '<Display(Name:="Vendido")> _
            'Public Property Vendido_fecha As Nullable(Of DateTime)

            '<Display(Name:="Título")> _
            'Public Property CumplimientoTitulo As Nullable(Of DateTime)

            '<Required(ErrorMessage:="Este campo es requerido. (Cumplimiento)")> _
            '  <Display(Name:="Efectivo")> _
            'Public Property Cumplimiento As DateTime

            '<Required(ErrorMessage:="Este campo es requerido. (IDBolsa)")> _
            '  <Display(Name:="Bolsa")> _
            'Public Property IDBolsa As Integer

            '<Display(Name:=" ")> _
            'Public Property Plazo As Nullable(Of Integer)

            '<Display(Name:=" ")> _
            'Public Property DiasVencimiento As Nullable(Of Integer)

            '<Required(ErrorMessage:="Este campo es requerido. (TasaEfectiva)")> _
            '<Display(Name:=" ")> _
            'Public Property TasaEfectiva As Double

            '<Display(Name:=" ")> _
            'Public Property Modalidad As String

            '<Display(Name:=" ")> _
            'Public Property Emision As Nullable(Of DateTime)

            '<Display(Name:=" ")> _
            'Public Property Vencimiento As Nullable(Of DateTime)

            '<Display(Name:="Plaza")> _
            'Public Property Plaza As String

            '<Display(Name:="Ubicación")> _
            'Public Property UBICACIONTITULO As String

            '<Display(Name:="Comisionista local")> _
            'Public Property IDComisionistaLocal As Nullable(Of Integer)

            '<Display(Name:="Comisionista Otra Plaza")> _
            'Public Property IDComisionistaOtraPlaza As Nullable(Of Integer)

            '<Display(Name:="Ciudad Otra Plaza")> _
            'Public Property IDCiudadOtraPlaza As Nullable(Of Integer)

            '<Display(Name:="Precio")> _
            'Public Property Precio As Nullable(Of Double)

            '<Display(Name:="Valor")> _
            'Public Property Transaccion_cur As Nullable(Of Double)

            '<Display(Name:="Comisión")> _
            'Public Property Comision As Nullable(Of Double)

            '<Display(Name:="Subtotal")> _
            'Public Property SubTotalLiq As Nullable(Of Double)

            '<Display(Name:="Retención")> _
            'Public Property Retencion As Nullable(Of Double)

            '<Display(Name:="Intereses")> _
            'Public Property Intereses As Nullable(Of Double)

            '<Display(Name:="Serv Bolsa")> _
            'Public Property ServBolsaFijo As Nullable(Of Double)

            '<Display(Name:=" ")> _
            'Public Property Traslado As String

            '<Display(Name:="Cantidad")> _
            'Public Property Cantidad As Nullable(Of Double)

            '<Display(Name:="% Comisión")> _
            'Public Property FactorComisionPactada As Nullable(Of Double)

            '<Display(Name:="IVA Comisión")> _
            'Public Property ValorIva As Nullable(Of Double)

            '<Display(Name:="Indicador")> _
            'Public Property IndicadorEconomico As String

            '<Display(Name:="Puntos Indicador")> _
            'Public Property PuntosIndicador As Nullable(Of Double)

            '<Display(Name:="Traslado")> _
            'Public Property ValorTraslado As Nullable(Of Double)

            '<Display(Name:="Total")> _
            'Public Property TotalLiq As Nullable(Of Double)

            '<Display(Name:="Valor Entrega Contrapago")> _
            'Public Property ValorEntregaContraPago As Nullable(Of Double)

            '<Display(Name:="A quien se envía la retención")> _
            'Public Property AquienSeEnviaRetencion As String

            '<Display(Name:="Numero Swap")> _
            'Public Property NroSwap As Nullable(Of Integer)







            '<Editable(True, AllowInitialValue:=False)> _
            '<[ReadOnly](False)> _
            '<Required(ErrorMessage:="Este campo es requerido. (IDLiquidaciones)")> _
            '<Display(Name:="IDLiquidaciones")> _
            'Public Property IDLiquidaciones As Integer

            '<Display(Name:="Prefijo")> _
            'Public Property Prefijo As String

            '<Display(Name:="Facturada")> _
            'Public Property Facturada As String

            '<Display(Name:="ValBolsa")> _
            'Public Property ValBolsa As Nullable(Of Double)

            '<Display(Name:="IDRueda")> _
            'Public Property IDRueda As Nullable(Of Int16)

            '<Display(Name:=" ")> _
            'Public Property TasaDescuento As Nullable(Of Double)

            '<Display(Name:="TasaCompraVende")> _
            'Public Property TasaCompraVende As Nullable(Of Double)

            '<Required(ErrorMessage:="Este campo es requerido. (OtraPlaza)")> _
            '  <Display(Name:="OtraPlaza")> _
            'Public Property OtraPlaza As Boolean

            '<Display(Name:="DiasIntereses")> _
            'Public Property DiasIntereses As Nullable(Of Integer)

            '<Display(Name:="Mercado", Description:="Mercado")> _
            'Public Property Mercado As String

            '<Display(Name:="NroTitulo", Description:="NroTitulo")> _
            'Public Property NroTitulo As String

            '<Display(Name:="IDCiudadExpTitulo", Description:="IDCiudadExpTitulo")> _
            'Public Property IDCiudadExpTitulo As Nullable(Of Integer)

            '<Display(Name:="PlazoOriginal")> _
            'Public Property PlazoOriginal As Nullable(Of Integer)

            '<Required(ErrorMessage:="Este campo es requerido. (Aplazamiento)")> _
            '  <Display(Name:="Aplazamiento")> _
            'Public Property Aplazamiento As Boolean

            '<Display(Name:="VersionPapeleta")> _
            'Public Property VersionPapeleta As Nullable(Of Int16)

            '<Display(Name:="EmisionOriginal", Description:="EmisionOriginal")> _
            'Public Property EmisionOriginal As Nullable(Of DateTime)

            '<Display(Name:="VencimientoOriginal", Description:="VencimientoOriginal")> _
            'Public Property VencimientoOriginal As Nullable(Of DateTime)

            '<Display(Name:="Impresiones")> _
            'Public Property Impresiones As Nullable(Of Integer)

            '<Display(Name:="FormaPago", Description:="FormaPago")> _
            'Public Property FormaPago As String

            '<Display(Name:="CtrlImpPapeleta", Description:="CtrlImpPapeleta")> _
            'Public Property CtrlImpPapeleta As Nullable(Of Integer)

            '<Display(Name:="PosicionPropia", Description:="PosicionPropia")> _
            'Public Property PosicionPropia As String

            '<Display(Name:="Transaccion")> _
            'Public Property Transaccion As String

            '<Display(Name:="TipoOperacion", Description:="TipoOperacion")> _
            'Public Property TipoOperacion As String

            '<Display(Name:="DiasContado", Description:="DiasContado")> _
            'Public Property DiasContado As Nullable(Of Integer)

            '<Display(Name:="Ordinaria", Description:="Ordinaria")> _
            'Public Property Ordinaria As Boolean

            '<Display(Name:="ObjetoOrdenExtraordinaria", Description:="ObjetoOrdenExtraordinaria")> _
            'Public Property ObjetoOrdenExtraordinaria As String

            '<Display(Name:="NumPadre", Description:="NumPadre")> _
            'Public Property NumPadre As Nullable(Of Integer)

            '<Display(Name:="OperacionPadre", Description:="OperacionPadre")> _
            'Public Property OperacionPadre As Nullable(Of DateTime)

            '<Required(ErrorMessage:="Este campo es requerido. (Vendido)")> _
            '  <Display(Name:="Vendido", Description:="Vendido")> _
            'Public Property Vendido As Boolean

            '<Display(Name:="ValorBrutoCompraVencida", Description:="ValorBrutoCompraVencida")> _
            'Public Property ValorBrutoCompraVencida As Nullable(Of Double)


            '<Required(ErrorMessage:="Este campo es requerido. (AutoRetenedor)")> _
            '<Display(Name:="AutoRetenedor", Description:="AutoRetenedor")> _
            'Public Property AutoRetenedor As String

            '<Required(ErrorMessage:="Este campo es requerido. (Sujeto)")> _
            '<Display(Name:="Sujeto", Description:="Sujeto")> _
            'Public Property Sujeto As String

            '<Display(Name:="PcRenEfecCompraRet", Description:="PcRenEfecCompraRet")> _
            'Public Property PcRenEfecCompraRet As Nullable(Of Double)

            '<Display(Name:="PcRenEfecVendeRet", Description:="PcRenEfecVendeRet")> _
            'Public Property PcRenEfecVendeRet As Nullable(Of Double)

            '<Required(ErrorMessage:="Este campo es requerido. (Reinversion)")> _
            '  <Display(Name:="Reinversion", Description:="Reinversion")> _
            'Public Property Reinversion As String

            '<Required(ErrorMessage:="Este campo es requerido. (Swap)")> _
            '  <Display(Name:="Swap", Description:="Swap")> _
            'Public Property Swap As String

            '<Required(ErrorMessage:="Este campo es requerido. (Certificacion)")> _
            '<Display(Name:="Certificacion", Description:="Certificacion")> _
            'Public Property Certificacion As String

            '<Display(Name:="DescuentoAcumula", Description:="DescuentoAcumula")> _
            'Public Property DescuentoAcumula As Nullable(Of Double)

            '<Display(Name:="PctRendimiento", Description:="PctRendimiento")> _
            'Public Property PctRendimiento As Nullable(Of Double)

            '<Display(Name:="FechaCompraVencido", Description:="FechaCompraVencido")> _
            'Public Property FechaCompraVencido As Nullable(Of DateTime)

            '<Display(Name:="PrecioCompraVencido", Description:="PrecioCompraVencido")> _
            'Public Property PrecioCompraVencido As Nullable(Of Double)

            '<Required(ErrorMessage:="Este campo es requerido. (ConstanciaEnajenacion)")> _
            '<Display(Name:="ConstanciaEnajenacion", Description:="ConstanciaEnajenacion")> _
            'Public Property ConstanciaEnajenacion As String

            '<Required(ErrorMessage:="Este campo es requerido. (RepoTitulo)")> _
            '<Display(Name:="RepoTitulo", Description:="RepoTitulo")> _
            'Public Property RepoTitulo As String

            '<Display(Name:="Serv Bolsa")> _
            'Public Property ServBolsaVble As Nullable(Of Double)

            '<Display(Name:="HoraGrabacion", Description:="HoraGrabacion")> _
            'Public Property HoraGrabacion As String

            '<Display(Name:="OrigenOperacion", Description:="OrigenOperacion")> _
            'Public Property OrigenOperacion As String

            '<Display(Name:="CodigoOperadorCompra", Description:="CodigoOperadorCompra")> _
            'Public Property CodigoOperadorCompra As Nullable(Of Integer)

            '<Display(Name:="CodigoOperadorVende", Description:="CodigoOperadorVende")> _
            'Public Property CodigoOperadorVende As Nullable(Of Integer)

            '<Display(Name:="IdentificacionRemate", Description:="IdentificacionRemate")> _
            'Public Property IdentificacionRemate As String

            '<Display(Name:="ModalidaOperacion", Description:="ModalidaOperacion")> _
            'Public Property ModalidaOperacion As String

            '<Display(Name:="IndicadorPrecio", Description:="IndicadorPrecio")> _
            'Public Property IndicadorPrecio As String

            '<Display(Name:="PeriodoExdividendo", Description:="PeriodoExdividendo")> _
            'Public Property PeriodoExdividendo As String

            '<Display(Name:="PlazoOperacionRepo", Description:="PlazoOperacionRepo")> _
            'Public Property PlazoOperacionRepo As Nullable(Of Integer)

            '<Display(Name:="ValorCaptacionRepo", Description:="ValorCaptacionRepo")> _
            'Public Property ValorCaptacionRepo As Nullable(Of Double)

            '<Display(Name:="VolumenCompraRepo", Description:="VolumenCompraRepo")> _
            'Public Property VolumenCompraRepo As Nullable(Of Double)

            '<Display(Name:="PrecioNetoFraccion", Description:="PrecioNetoFraccion")> _
            'Public Property PrecioNetoFraccion As Nullable(Of Double)

            '<Display(Name:="VolumenNetoFraccion", Description:="VolumenNetoFraccion")> _
            'Public Property VolumenNetoFraccion As Nullable(Of Double)

            '<Display(Name:="CodigoContactoComercial", Description:="CodigoContactoComercial")> _
            'Public Property CodigoContactoComercial As String

            '<Display(Name:="NroFraccionOperacion", Description:="NroFraccionOperacion")> _
            'Public Property NroFraccionOperacion As Nullable(Of Integer)

            '<Display(Name:="IdentificacionPatrimonio1", Description:="IdentificacionPatrimonio1")> _
            'Public Property IdentificacionPatrimonio1 As String

            '<Display(Name:="TipoidentificacionCliente2", Description:="TipoidentificacionCliente2")> _
            'Public Property TipoidentificacionCliente2 As String

            '<Display(Name:="NitCliente2", Description:="NitCliente2")> _
            'Public Property NitCliente2 As String

            '<Display(Name:="IdentificacionPatrimonio2", Description:="IdentificacionPatrimonio2")> _
            'Public Property IdentificacionPatrimonio2 As String

            '<Display(Name:="TipoIdentificacionCliente3", Description:="TipoIdentificacionCliente3")> _
            'Public Property TipoIdentificacionCliente3 As String

            '<Display(Name:="NitCliente3", Description:="NitCliente3")> _
            'Public Property NitCliente3 As String

            '<Display(Name:="IdentificacionPatrimonio3", Description:="IdentificacionPatrimonio3")> _
            'Public Property IdentificacionPatrimonio3 As String

            '<Display(Name:="IndicadorOperacion", Description:="IndicadorOperacion")> _
            'Public Property IndicadorOperacion As String

            '<Display(Name:="BaseRetencion", Description:="BaseRetencion")> _
            'Public Property BaseRetencion As Nullable(Of Double)

            '<Display(Name:="PorcRetencion", Description:="PorcRetencion")> _
            'Public Property PorcRetencion As Nullable(Of Double)

            '<Display(Name:="BaseRetencionTranslado", Description:="BaseRetencionTranslado")> _
            'Public Property BaseRetencionTranslado As Nullable(Of Double)

            '<Display(Name:="PorcRetencionTranslado", Description:="PorcRetencionTranslado")> _
            'Public Property PorcRetencionTranslado As Nullable(Of Double)

            '<Display(Name:="PorcIvaComision", Description:="PorcIvaComision")> _
            'Public Property PorcIvaComision As Nullable(Of Double)

            '<Display(Name:="IndicadorAcciones", Description:="IndicadorAcciones")> _
            'Public Property IndicadorAcciones As String

            '<Display(Name:="OperacionNegociada", Description:="OperacionNegociada")> _
            'Public Property OperacionNegociada As String

            '<Display(Name:="FechaConstancia", Description:="FechaConstancia")> _
            'Public Property FechaConstancia As Nullable(Of DateTime)

            '<Display(Name:="ValorConstancia", Description:="ValorConstancia")> _
            'Public Property ValorConstancia As Nullable(Of Double)

            '<Display(Name:="GeneraConstancia", Description:="GeneraConstancia")> _
            'Public Property GeneraConstancia As String

            '<Required(ErrorMessage:="Este campo es requerido. (Cargado)")> _
            '  <Display(Name:="Cargado", Description:="Cargado")> _
            'Public Property Cargado As Boolean

            '<Display(Name:="Actualizacion", Description:="Actualizacion")> _
            'Public Property Actualizacion As DateTime

            '<Required(ErrorMessage:="Este campo es requerido. (Usuario)")> _
            '  <Display(Name:="Usuario", Description:="Usuario")> _
            'Public Property Usuario As String

            '<Required(ErrorMessage:="Este campo es requerido. (NroLote)")> _
            '  <Display(Name:="NroLote", Description:="NroLote")> _
            'Public Property NroLote As Integer

            '<Required(ErrorMessage:="Este campo es requerido. (IDBaseDias)")> _
            '  <Display(Name:="IDBaseDias", Description:="IDBaseDias")> _
            'Public Property IDBaseDias As String

            '<Display(Name:="TipoDeOferta", Description:="TipoDeOferta")> _
            'Public Property TipoDeOferta As String

            '<Display(Name:="NroLoteENC", Description:="NroLoteENC")> _
            'Public Property NroLoteENC As Nullable(Of Integer)

            '<Display(Name:="ContabilidadENC", Description:="ContabilidadENC")> _
            'Public Property ContabilidadENC As Nullable(Of DateTime)

            '<Display(Name:="CodigoIntermediario", Description:="CodigoIntermediario")> _
            'Public Property CodigoIntermediario As String

        End Class
    End Class
End Namespace
