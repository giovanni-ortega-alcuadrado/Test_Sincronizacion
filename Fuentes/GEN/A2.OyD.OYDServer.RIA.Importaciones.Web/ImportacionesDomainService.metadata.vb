
Option Compare Binary
Option Infer On
Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Linq

Namespace OyDImportaciones


    'The MetadataTypeAttribute identifies ArchivoMetadata as the class
    ' that carries additional metadata for the Archivo class.
    <MetadataTypeAttribute(GetType(Archivo.ArchivoMetadata))> _
    Partial Public Class Archivo

        'This class allows you to attach custom attributes to properties
        ' of the Archivo class.
        '
        'For example, the following marks the Xyz property as a
        ' required property and specifies the format for valid values:
        '    <Required()>
        '    <RegularExpression("[A-Z][A-Za-z0-9]*")>
        '    <StringLength(32)>
        '    Public Property Xyz As String
        Friend NotInheritable Class ArchivoMetadata

            'Metadata classes are not meant to be instantiated.
            Private Sub New()
                MyBase.New()
            End Sub

            Public Property Extension As String

            Public Property FechaHora As Nullable(Of DateTime)

            Public Property KBytes As Nullable(Of Integer)

            Public Property Nombre As String

            Public Property Ruta As String

            Public Property RutaWeb As String
        End Class
    End Class
    <MetadataTypeAttribute(GetType(TblpagosDCV.TblpagosDCVMetadata))> _
    Partial Public Class TblpagosDCV
        Friend NotInheritable Class TblpagosDCVMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Editable(True)> _
                <Display(Name:="Seleccionado")> _
            Public Property Seleccionado As Boolean
        End Class
    End Class

#Region "OYD PLUS"
    '************************IMPORTACION OYD PLUS ****************************************************
    <MetadataTypeAttribute(GetType(clsTransferenciaOyDPlus.clsTransferenciaOyDPlusMetadata))> _
    Partial Public Class clsTransferenciaOyDPlus

        Friend NotInheritable Class clsTransferenciaOyDPlusMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            Public Property TipoDocTitular As String
            Public Property NroDocTitular As String
            Public Property NombreTitular As String
            Public Property TipoCuenta As String
            Public Property CodigoBanco As String
            Public Property NroCuenta As String
            Public Property ValorGiro As String
            Public Property Concepto As String
        End Class
    End Class

    <MetadataTypeAttribute(GetType(clsChequesOyDPlus.clsChequesOyDPlusMetadata))> _
    Partial Public Class clsChequesOyDPlus

        Friend NotInheritable Class clsChequesOyDPlusMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            Public Property TipoDocBeneficiario As String
            Public Property NroDocBeneficiario As String
            Public Property NombreBeneficiario As String
            Public Property ValorGiro As String
            Public Property Concepto As String
            Public Property TipoCruce As String
        End Class
    End Class
    '************************IMPORTACION OYD PLUS ****************************************************
#End Region






    'The MetadataTypeAttribute identifies LineaComentarioMetadata as the class
    ' that carries additional metadata for the LineaComentario class.
    <MetadataTypeAttribute(GetType(LineaComentario.LineaComentarioMetadata))> _
    Partial Public Class LineaComentario

        'This class allows you to attach custom attributes to properties
        ' of the LineaComentario class.
        '
        'For example, the following marks the Xyz property as a
        ' required property and specifies the format for valid values:
        '    <Required()>
        '    <RegularExpression("[A-Z][A-Za-z0-9]*")>
        '    <StringLength(32)>
        '    Public Property Xyz As String
        Friend NotInheritable Class LineaComentarioMetadata

            'Metadata classes are not meant to be instantiated.
            Private Sub New()
                MyBase.New()
            End Sub

            Public Property FechaHora As Nullable(Of DateTime)

            Public Property Texto As String
        End Class
    End Class


    'The MetadataTypeAttribute identifies LiquidacionEditarMetadata as the class
    ' that carries additional metadata for the LiquidacionEditar class.
    <MetadataTypeAttribute(GetType(LiquidacionEditar.LiquidacionEditarMetadata))> _
    Partial Public Class LiquidacionEditar

        'This class allows you to attach custom attributes to properties
        ' of the LiquidacionEditar class.
        '
        'For example, the following marks the Xyz property as a
        ' required property and specifies the format for valid values:
        '    <Required()>
        '    <RegularExpression("[A-Z][A-Za-z0-9]*")>
        '    <StringLength(32)>
        '    Public Property Xyz As String
        Friend NotInheritable Class LiquidacionEditarMetadata

            'Metadata classes are not meant to be instantiated.
            Private Sub New()
                MyBase.New()
            End Sub

            Public Property IDLiq As Nullable(Of Integer)

            '<Required()> _
            '<RegularExpression("[0-9]+", ErrorMessage:="El campo {0} sólo permite números")> _
            Public Property IDOrden As String
        End Class
    End Class


    <MetadataTypeAttribute(GetType(ImportacionLi.ImportacionLiMetadata))> _
    Partial Public Class ImportacionLi

        Friend NotInheritable Class ImportacionLiMetadata
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

            <Display(Name:="Comitente")> _
            Public Property IDComitente As String

            <Display(Name:="Ordenante")> _
            Public Property IDOrdenante As String

            <Display(Name:="Bolsa", Description:="IDBolsa")> _
            Public Property IDBolsa As Nullable(Of Integer)

            <Display(Name:="Rueda", Description:="IDRueda")> _
            Public Property IDRueda As Nullable(Of Int16)

            <Display(Name:="Va lBolsa")> _
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

            <Display(Name:="Plaza")> _
            Public Property Plaza As String

            <Display(Name:="ComisionistaLocal", Description:="IDComisionistaLocal")> _
            Public Property IDComisionistaLocal As Nullable(Of Integer)

            <Display(Name:="ComisionistaOtraPlaza", Description:="IDComisionistaOtraPlaza")> _
            Public Property IDComisionistaOtraPlaza As Nullable(Of Integer)

            <Display(Name:="CiudadOtraPlaza", Description:="IDCiudadOtraPlaza")> _
            Public Property IDCiudadOtraPlaza As Nullable(Of Integer)

            <Display(Name:="Tasa Efectiva")> _
            Public Property TasaEfectiva As Nullable(Of Double)

            <Display(Name:="Cantidad")> _
            Public Property Cantidad As Nullable(Of Double)

            <Display(Name:="Precio")> _
            Public Property Precio As Nullable(Of Double)

            <Display(Name:="Valor Trans.")> _
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

            <Display(Name:="Dias Intereses", Description:="DiasIntereses")> _
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

            <Display(Name:="Transaccion")> _
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

            <Display(Name:="PorcIvaComision")> _
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

            <Display(Name:="Nombre")> _
            Public Property Nombre As String

            <Display(Name:="Clase")> _
            Public Property Clase As String

            <Display(Name:="Tipo Orden")> _
            Public Property TipoOrden As String

        End Class
    End Class

    <MetadataTypeAttribute(GetType(TblTitulosValorizado.TblTitulosValorizadoMetadata))> _
    Partial Public Class TblTitulosValorizado
        Friend NotInheritable Class TblTitulosValorizadoMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="dblValor", Description:="dblValor")> _
            Public Property dblValor As Nullable(Of Decimal)

            <Display(Name:="dtmActualizacion", Description:="dtmActualizacion")> _
            Public Property dtmActualizacion As Nullable(Of DateTime)

            <Display(Name:="dtmFechaValorizacion", Description:="dtmFechaValorizacion")> _
            Public Property dtmFechaValorizacion As Nullable(Of DateTime)

            <Display(Name:="dtmFechaVencimiento", Description:="dtmFechaVencimiento")> _
            Public Property dtmFechaVencimiento As Nullable(Of DateTime)

            <Display(Name:="lngidComisionista", Description:="lngidComisionista")> _
            Public Property lngidComisionista As Nullable(Of Integer)

            <Display(Name:="lngidSecuencia", Description:="lngidSecuencia")> _
            Public Property lngidSecuencia As Nullable(Of Integer)

            <Display(Name:="lngidSucComisionista", Description:="lngidSucComisionista")> _
            Public Property lngidSucComisionista As Nullable(Of Integer)

            <Display(Name:="logAprobado", Description:="logAprobado")> _
            Public Property logAprobado As Nullable(Of Boolean)

            <Display(Name:="strCadena", Description:="strCadena")> _
            Public Property strCadena As String

            <Display(Name:="strIDEspecie", Description:="strIDEspecie")> _
            Public Property strIDEspecie As String

            <Display(Name:="strIsinANNA", Description:="strIsinANNA")> _
            Public Property strIsinANNA As String

            <Display(Name:="strUsuario", Description:="strUsuario")> _
            Public Property strUsuario As String

        End Class
    End Class


    <MetadataTypeAttribute(GetType(ListaTitulosValorizados.ListaTitulosValorizadosMetadata))> _
    Partial Public Class ListaTitulosValorizados
        Friend NotInheritable Class ListaTitulosValorizadosMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="Aprobado", Description:="Aprobado")> _
            Public Property Aprobado As Nullable(Of Boolean)

            <Display(Name:="Fungible", Description:="Fungible")> _
            Public Property Fungible As String

            <Display(Name:="Fvalorizacion", Description:="Fvalorizacion")> _
            Public Property Fvalorizacion As String

            <Display(Name:="IdEspecie", Description:="IdEspecie")> _
            Public Property IdEspecie As String

            <Display(Name:="IsinAnna", Description:="IsinAnna")> _
            Public Property IsinAnna As String

            <Display(Name:="Registro_Completo", Description:="Registro_Completo")> _
            Public Property Registro_Completo As String

            <Display(Name:="Secuencia", Description:="Secuencia")> _
            Public Property Secuencia As String

            <Display(Name:="ValorEspecie", Description:="ValorEspecie")> _
            Public Property ValorEspecie As Nullable(Of Double)

            <Display(Name:="FechaVencimiento", Description:="FechaVencimiento")> _
            Public Property FechaVencimiento As String

        End Class

    End Class

    <MetadataTypeAttribute(GetType(Reporterecibodecaja.ReporterecibodecajaMetadata))> _
    Partial Public Class Reporterecibodecaja
        Friend NotInheritable Class ReporterecibodecajaMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="Especie")> _
            Public Property Especie As String

            <Display(Name:="Valor A Pagar")> _
            Public Property VlrAPagar As Integer

            '<Display(Name:="Codigo Cliente")> _
            'Public Property CodigoCliente As Integer

            '<Display(Name:="Nombre Cliente")> _
            'Public Property NombreCliente As String

            '<Display(Name:="Nemotecnico Especie")> _
            'Public Property NemotecnicoEspecie As String

            '<Display(Name:="Nombre Especie")> _
            'Public Property NombreEspecie As Integer

            '<Display(Name:="Valor Nominal Titulo")> _
            'Public Property ValorNomina As Double

            '<Display(Name:="Nro Recibo Custodia")> _
            'Public Property NroRecibo As Integer

            '<Display(Name:="Nro Fila Custodia")> _
            'Public Property NroFila As Integer

            '<Display(Name:="Codigo Deposito Valores")> _
            'Public Property CodigoDeposito As String

            '<Display(Name:="Nombre Deposito de Valores")> _
            'Public Property NombreDeposito As String

            '<Display(Name:="ISIN")> _
            'Public Property ISIN As String

            '<Display(Name:="Cuenta Deposito")> _
            'Public Property CuentaDeposito As Integer

            '<Display(Name:="Concepto Bloqueo")> _
            'Public Property ConceptoBloqueo As String

            '<Display(Name:="Codigo Sucursal Receptor")> _
            'Public Property CodigoSucursal As Integer

            '<Display(Name:="Nombre Sucursal Receptor")> _
            'Public Property NombreSucursal As String

            '<Display(Name:="Codigo Receptor Lider Cliente")> _
            'Public Property CodigoReceptor As String

            '<Display(Name:="Nombre Receptor Lider Cliente")> _
            'Public Property NombreReceptor As String

            '<Display(Name:="Nro Liquidacion")> _
            'Public Property NroLiquidacion As Integer

            '<Display(Name:="Nro Parcial")> _
            'Public Property NroParcial As Integer

            '<Display(Name:="Fecha Elaboracion Liquidacion")> _
            'Public Property FechaElaboracion As DateTime

            '<Display(Name:="Tipo Liquidacion")> _
            'Public Property TipoLiquidacion As String

            '<Display(Name:="Clase Liquidacion")> _
            'Public Property ClaseLiquidacion As String

            '<Display(Name:="Periodicidad")> _
            'Public Property Periodicidad As String

            '<Display(Name:="Tasa Emision")> _
            'Public Property TasaEmision As Double

            '<Display(Name:="Indicador Economico")> _
            'Public Property IndicadorEconomico As String

            '<Display(Name:="Puntos indicador")> _
            'Public Property Puntos As Double

            '<Display(Name:="Fecha Emision")> _
            'Public Property Emision As DateTime

            '<Display(Name:="Fecha Vencimiento")> _
            'Public Property Vencimiento As DateTime

            '<Display(Name:="VPN_Mercado_Alianza")> _
            'Public Property VPNMercado As Double

            '<Display(Name:="Total (VPN  X  V/r nominal)")> _
            'Public Property VPNMercadoTotal As Double

            '<Display(Name:="NroTitulo")> _
            'Public Property NroTitulo As String

            '<Display(Name:="TIRVPN")> _
            'Public Property TIRVPN As Double

            '<Display(Name:="VlrLineal")> _
            'Public Property VlrLineal As Double

            '<Display(Name:="TIROriginal")> _
            'Public Property TIROriginal As Double

            '<Display(Name:="TIRActual")> _
            'Public Property TIRActual As Double

            '<Display(Name:="Spread")> _
            'Public Property Spread As Double

            '<Display(Name:="TIRSpread")> _
            'Public Property TIRSpread As Double

            '<Display(Name:="Valor Valoración OyD")> _
            'Public Property ValorValoracionOyD As Double

            '<Display(Name:="Fecha Recibo")> _
            'Public Property Recibo As DateTime

            '<Display(Name:="Fecha Elaboracion")> _
            'Public Property Elaboracion As DateTime

            '<Display(Name:="Nro Documento")> _
            'Public Property NroDocumento As String

            '<Display(Name:="Tasa Compra")> _
            'Public Property TasaCompra As Double

            '<Display(Name:="TasaEfectiva")> _
            'Public Property TasaEfectiva As Double

            '<Display(Name:="Precio")> _
            'Public Property Precio As Decimal

            '<Display(Name:="Liquidación")> _
            'Public Property Liquidacion As DateTime

            '<Display(Name:="Transacción")> _
            'Public Property Transaccion As Decimal

            '<Display(Name:="Tipo de Oferta")> _
            'Public Property tipoDeOferta As String

            '<Display(Name:="Descripción Tipo de Oferta")> _
            'Public Property DescripcionTipoDeOferta As String

            '<Display(Name:="Nombre Indicador")> _
            'Public Property NombreIndicador As String

        End Class
    End Class
End Namespace

