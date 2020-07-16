Option Compare Binary
Option Infer On
Option Strict On
Option Explicit On

Imports System.ComponentModel.DataAnnotations

Namespace OyDTesoreria

    <MetadataTypeAttribute(GetType(Cheque.ChequeMetadata))> _
    Partial Public Class Cheque
        Friend NotInheritable Class ChequeMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            '       <Required(ErrorMessage:="Este campo es requerido. (IDComisionista)")> _
            '<Display(Name:="IDComisionista", Description:="IDComisionista")> _
            '       Public Property IDComisionista As Integer

            '<Required(ErrorMessage:="Este campo es requerido. (IDSucComisionista)")> _
            '  <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
            'Public Property IDSucComisionista As Integer

            '<Required(ErrorMessage:="Este campo es requerido. (NombreConsecutivo)")> _
            '  <Display(Name:="NombreConsecutivo", Description:="NombreConsecutivo")> _
            'Public Property NombreConsecutivo As String

            '<Required(ErrorMessage:="Este campo es requerido. (IDDocumento)")> _
            '  <Display(Name:="IDDocumento", Description:="IDDocumento")> _
            'Public Property IDDocumento As Integer

            '<Required(ErrorMessage:="Este campo es requerido. (Secuencia)")> _
            '  <Display(Name:="Secuencia", Description:="Secuencia")> _
            'Public Property Secuencia As Integer

            '<Required(ErrorMessage:="Este campo es requerido. (Efectivo)")> _
            '  <Display(Name:="Efectivo", Description:="Efectivo")> _
            'Public Property Efectivo As Boolean

            <Display(Name:="Banco Girador", Description:="BancoGirador")> _
            Public Property BancoGirador As String

            <Display(Name:="Número Cheque", Description:="NumCheque")> _
            Public Property NumCheque As Nullable(Of Double)

            <Required(ErrorMessage:="Este campo es requerido. (Valor)")> _
              <Display(Name:="Valor", Description:="Valor")> _
            Public Property Valor As Decimal

            <Display(Name:="Banco Consignacion", Description:="BancoConsignacion")> _
            Public Property BancoConsignacion As Nullable(Of Integer)

            <Display(Name:="Consignación", Description:="Consignacion")> _
            Public Property Consignacion As Nullable(Of DateTime)

            <Display(Name:="Forma Pago", Description:="FormaPagoRC")> _
            Public Property FormaPagoRC As String

            <Display(Name:="Actualización", Description:="Actualizacion")> _
            Public Property Actualizacion As DateTime

            '<Display(Name:="Usuario", Description:="Usuario")> _
            'Public Property Usuario As String

            <Display(Name:="Comentario", Description:="Comentario")> _
            Public Property Comentario As String

            '<Display(Name:="IdProducto", Description:="IdProducto")> _
            'Public Property IdProducto As Nullable(Of Integer)

            '<Display(Name:="SucursalesBancolombia", Description:="SucursalesBancolombia")> _
            'Public Property SucursalesBancolombia As String

            '<Display(Name:="IDCheques", Description:="IDCheques")> _
            'Public Property IDCheques As Integer

            '<Required(ErrorMessage:="Este campo es requerido. (ChequeHizoCanje)")> _
            '  <Display(Name:="ChequeHizoCanje", Description:="ChequeHizoCanje")> _
            'Public Property ChequeHizoCanje As Boolean

        End Class
    End Class

    <MetadataTypeAttribute(GetType(TesoreriaAdicionale.TesoreriaAdicionaleMetadata))> _
    Partial Public Class TesoreriaAdicionale
        Friend NotInheritable Class TesoreriaAdicionaleMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            '      <Required(ErrorMessage:="Este campo es requerido. (IDComisionista)")> _
            '<Display(Name:="IDComisionista", Description:="IDComisionista")> _
            '      Public Property IDComisionista As Integer

            '      <Required(ErrorMessage:="Este campo es requerido. (IDSucComisionista)")> _
            '        <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
            '      Public Property IDSucComisionista As Integer

            '      <Display(Name:="Tipo", Description:="Tipo")> _
            '      Public Property Tipo As String

            '      <Required(ErrorMessage:="Este campo es requerido. (NombreConsecutivo)")> _
            '        <Display(Name:="NombreConsecutivo", Description:="NombreConsecutivo")> _
            '      Public Property NombreConsecutivo As String

            '      <Required(ErrorMessage:="Este campo es requerido. (IDDocumento)")> _
            '        <Display(Name:="IDDocumento", Description:="IDDocumento")> _
            '      Public Property IDDocumento As Integer

            <Display(Name:="Observación")> _
            Public Property Observacion As String

            '<Required(ErrorMessage:="Este campo es requerido. (Actualizacion)")> _
            '  <Display(Name:="Actualizacion", Description:="Actualizacion")> _
            'Public Property Actualizacion As DateTime

            '<Display(Name:="Usuario", Description:="Usuario")> _
            'Public Property Usuario As String

            '<Display(Name:="IDTesoreriaAdicionales", Description:="IDTesoreriaAdicionales")> _
            'Public Property IDTesoreriaAdicionales As Integer

        End Class
    End Class

    <MetadataTypeAttribute(GetType(DetalleTesoreri.DetalleTesoreriMetadata))> _
    Partial Public Class DetalleTesoreri
        Friend NotInheritable Class DetalleTesoreriMetadata
            Private Sub New()
                MyBase.New()
            End Sub

#Region "Campos Comprovantes"

#End Region

#Region "Campos Recibos de Caja"

            <Display(Name:="NIT", Description:="NIT")> _
            Public Property NIT As String

            <Display(Name:="Centro de Costo", Description:="CentroCosto")> _
            Public Property CentroCosto As String
#End Region

#Region "Campos Notas"
            <Display(Name:="Nombre Consecutivo", Description:="NombreConsecutivo")> _
            Public Property NombreConsecutivo As String
#End Region

#Region "Campos Comunes"
            <Display(Name:="ID Comitente", Description:="IDComitente")> _
            Public Property IDComitente As String

            <Display(Name:="Nombre", Description:="Nombre")> _
            Public Property Nombre As String

            <Display(Name:="Detalle", Description:="Detalle")> _           
            Public Property Detalle As String

            <Display(Name:="Cuenta Contable", Description:="IDCuentaContable")> _
            Public Property IDCuentaContable As String

            <Display(Name:="Credito", Description:="Credito")> _
            Public Property Credito As Decimal

            <Display(Name:="Debito", Description:="Debito")> _
            Public Property Debito As Decimal
#End Region
            '<Display(Name:="IDComisionista", Description:="IDComisionista")> _
            'Public Property IDComisionista As Integer

            '<Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
            'Public Property IDSucComisionista As Integer

            '<Display(Name:="Tipo", Description:="Tipo")> _
            'Public Property Tipo As String

            '<Display(Name:="IDDocumento", Description:="IDDocumento")> _
            'Public Property IDDocumento As Integer

            '<Display(Name:="Secuencia", Description:="Secuencia")> _
            'Public Property Secuencia As Integer

            '<Display(Name:="Valor", Description:="Valor")> _
            'Public Property Valor As Decimal

            '<Display(Name:="IDBanco", Description:="IDBanco")> _
            'Public Property IDBanco As Nullable(Of Integer)

            '<Display(Name:="Actualizacion", Description:="Actualizacion")> _
            'Public Property Actualizacion As DateTime

            '<Display(Name:="Usuario", Description:="Usuario")> _
            'Public Property Usuario As String

            '<Display(Name:="EstadoTransferencia", Description:="EstadoTransferencia")> _
            'Public Property EstadoTransferencia As String

            '<Display(Name:="BancoDestino", Description:="BancoDestino")> _
            'Public Property BancoDestino As Nullable(Of Integer)

            '<Display(Name:="CuentaDestino", Description:="CuentaDestino")> _
            'Public Property CuentaDestino As String

            '<Display(Name:="TipoCuenta", Description:="TipoCuenta")> _
            'Public Property TipoCuenta As String

            '<Display(Name:="IdentificacionTitular", Description:="IdentificacionTitular")> _
            'Public Property IdentificacionTitular As String

            '<Display(Name:="Titular", Description:="Titular")> _
            'Public Property Titular As String

            '<Display(Name:="TipoIdTitular", Description:="TipoIdTitular")> _
            'Public Property TipoIdTitular As String

            '<Display(Name:="FormaEntrega", Description:="FormaEntrega")> _
            'Public Property FormaEntrega As String

            '<Display(Name:="Beneficiario", Description:="Beneficiario")> _
            'Public Property Beneficiario As String

            '<Display(Name:="TipoIdentBeneficiario", Description:="TipoIdentBeneficiario")> _
            'Public Property TipoIdentBeneficiario As String

            '<Display(Name:="IdentificacionBenficiciario", Description:="IdentificacionBenficiciario")> _
            'Public Property IdentificacionBenficiciario As String

            '<Display(Name:="NombrePersonaRecoge", Description:="NombrePersonaRecoge")> _
            'Public Property NombrePersonaRecoge As String

            '<Display(Name:="IdentificacionPerRecoge", Description:="IdentificacionPerRecoge")> _
            'Public Property IdentificacionPerRecoge As String

            '<Display(Name:="OficinaEntrega", Description:="OficinaEntrega")> _
            'Public Property OficinaEntrega As String

            '<Display(Name:="IDDetalleTesoreria", Description:="IDDetalleTesoreria")> _
            'Public Property IDDetalleTesoreria As Integer

            '<Display(Name:="NombreConsecutivoNotaGMF", Description:="NombreConsecutivoNotaGMF")> _
            'Public Property NombreConsecutivoNotaGMF As String

            '<Display(Name:="IDDocumentoNotaGMF", Description:="IDDocumentoNotaGMF")> _
            'Public Property IDDocumentoNotaGMF As Nullable(Of Integer)

            '<Display(Name:="Exportados", Description:="Exportados")> _
            'Public Property Exportados As Nullable(Of Integer)

        End Class
    End Class

    <MetadataTypeAttribute(GetType(Tesoreri.TesoreriMetadata))> _
    Partial Public Class Tesoreri
        Friend NotInheritable Class TesoreriMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            '<Display(Name:="IDComisionista", Description:="IDComisionista")> _
            'Public Property IDComisionista As Integer

            '<Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
            'Public Property IDSucComisionista As Integer

            '<Display(Name:="Tipo", Description:="Tipo")> _
            'Public Property Tipo As String

            <Display(Name:="Tipo")> _
            <Required(ErrorMessage:="El campo {0}, es requerido.", AllowEmptyStrings:=True)> _
            Public Property NombreConsecutivo As String

            <Display(Name:="Número")> _
            Public Property IDDocumento As Integer

            <StringLength(1, ErrorMessage:="El campo {0} permite una longitud máxima de 1.")> _
            <Display(Name:="Documento")> _
            Public Property TipoIdentificacion As String


            <Display(Name:="Número")> _
            Public Property NroDocumento As String

            <Display(Name:="Banco")> _
            Public Property IDBanco As Nullable(Of Integer)

            <Display(Name:=" ")> _
            Public Property NombreBco As String

            <Display(Name:="Cheque")> _
            Public Property NumCheque As Nullable(Of Double)

            <StringLength(17, ErrorMessage:="El campo {0} permite una longitud máxima de 17.")> _
            <Display(Name:="Comitente")> _
            Public Property IdComitente As String

            <StringLength(80, ErrorMessage:="El campo {0} permite una longitud máxima de 80.")> _
            <Display(Name:="A favor de:")> _
            Public Property Nombre As String

            <Display(Name:=" ")> _
            Public Property EstadoMC As String
            '<Display(Name:="Valor", Description:="Valor")> _
            'Public Property Valor As Nullable(Of Decimal)

            '<Display(Name:="Detalle", Description:="Detalle")> _
            'Public Property Detalle As String

            <Display(Name:="Fecha")> _
            Public Property Documento As DateTime

            <Display(Name:="Estado")> _
            Public Property Estado As String

            <Display(Name:="Fecha")> _
            Public Property FecEstado As DateTime

            <Display(Name:=" ")> _
            Public Property Impresiones As Integer

            <Display(Name:="Forma Pago")> _
            Public Property FormaPagoCE As String

            '<Display(Name:="NroLote", Description:="NroLote")> _
            'Public Property NroLote As Integer

            <Display(Name:="Enviar a Contabilidad")> _
            Public Property Contabilidad As Boolean

            '<Display(Name:="Actualizacion", Description:="Actualizacion")> _
            'Public Property Actualizacion As DateTime

            '<Display(Name:="Usuario", Description:="Usuario")> _
            'Public Property Usuario As String

            '<Display(Name:="ParametroContable", Description:="ParametroContable")> _
            'Public Property ParametroContable As String

            '<Display(Name:="ImpresionFisica", Description:="ImpresionFisica")> _
            'Public Property ImpresionFisica As Boolean

            '<Display(Name:="MultiCliente", Description:="MultiCliente")> _
            'Public Property MultiCliente As Boolean

            <Display(Name:="Cuenta Bancaria")> _
            Public Property CuentaCliente As String

            '<Display(Name:="CodComitente", Description:="CodComitente")> _
            'Public Property CodComitente As String

            '<Display(Name:="ArchivoTransferencia", Description:="ArchivoTransferencia")> _
            'Public Property ArchivoTransferencia As String

            '<Display(Name:="IdNumInst", Description:="IdNumInst")> _
            'Public Property IdNumInst As Nullable(Of Integer)

            '<Display(Name:="DVP", Description:="DVP")> _
            'Public Property DVP As String

            '<Display(Name:="Instruccion", Description:="Instruccion")> _
            'Public Property Instruccion As String

            '<Display(Name:="IdNroOrden", Description:="IdNroOrden")> _
            'Public Property IdNroOrden As Nullable(Of Decimal)

            <Display(Name:="Detalle Instruccion")> _
            Public Property DetalleInstruccion As String

            '<Display(Name:="EstadoNovedadContabilidad", Description:="EstadoNovedadContabilidad")> _
            'Public Property EstadoNovedadContabilidad As String

            '<Display(Name:="eroComprobante_Contabilidad", Description:="eroComprobante_Contabilidad")> _
            'Public Property eroComprobante_Contabilidad As String

            '<Display(Name:="hadecontabilizacion_Contabilidad", Description:="hadecontabilizacion_Contabilidad")> _
            'Public Property hadecontabilizacion_Contabilidad As String

            '<Display(Name:="haProceso_Contabilidad", Description:="haProceso_Contabilidad")> _
            'Public Property haProceso_Contabilidad As Nullable(Of DateTime)

            '<Display(Name:="EstadoNovedadContabilidad_Anulada", Description:="EstadoNovedadContabilidad_Anulada")> _
            'Public Property EstadoNovedadContabilidad_Anulada As String

            '<Display(Name:="EstadoAutomatico", Description:="EstadoAutomatico")> _
            'Public Property EstadoAutomatico As String

            '<Display(Name:="eroLote_Contabilidad", Description:="eroLote_Contabilidad")> _
            'Public Property eroLote_Contabilidad As String

            '<Display(Name:="MontoEscrito", Description:="MontoEscrito")> _
            'Public Property MontoEscrito As String

            '<Display(Name:="TipoIntermedia", Description:="TipoIntermedia")> _
            'Public Property TipoIntermedia As String

            '<Display(Name:="Concepto", Description:="Concepto")> _
            'Public Property Concepto As String

            '<Display(Name:="RecaudoDirecto", Description:="RecaudoDirecto")> _
            'Public Property RecaudoDirecto As Nullable(Of Boolean)

            '<Display(Name:="ContabilidadEncuenta", Description:="ContabilidadEncuenta")> _
            'Public Property ContabilidadEncuenta As Nullable(Of DateTime)

            '<Display(Name:="Sobregiro", Description:="Sobregiro")> _
            'Public Property Sobregiro As Nullable(Of Boolean)

            '<Display(Name:="IdentificacionAutorizadoCheque", Description:="IdentificacionAutorizadoCheque")> _
            'Public Property IdentificacionAutorizadoCheque As String

            '<Display(Name:="NombreAutorizadoCheque", Description:="NombreAutorizadoCheque")> _
            'Public Property NombreAutorizadoCheque As String

            '<Display(Name:="IDTesoreria", Description:="IDTesoreria")> _
            'Public Property IDTesoreria As Integer

            '<Display(Name:="ContabilidadENC", Description:="ContabilidadENC")> _
            'Public Property ContabilidadENC As Nullable(Of DateTime)

            '<Display(Name:="NroLoteAntENC", Description:="NroLoteAntENC")> _
            'Public Property NroLoteAntENC As Nullable(Of Integer)

            '<Display(Name:="ContabilidadAntENC", Description:="ContabilidadAntENC")> _
            'Public Property ContabilidadAntENC As Nullable(Of DateTime)

            <Display(Name:="Sucursales")> _
            Public Property IdSucursalBancaria As Nullable(Of Integer)

            <Display(Name:=" ")> _
            Public Property SucursalBancaria As String

            <Display(Name:="Creación", Description:="Creacion")> _
            Public Property Creacion As Nullable(Of DateTime)

            <Display(Name:="TipoCuenta", Description:="TipoCuenta")> _
            Public Property TipoCuenta As String

        End Class
    End Class

#Region "OyD Plus"

    <MetadataTypeAttribute(GetType(TesoreriaOrdenesEncabezado.TesoreriaOrdenesEncabezadoMetadata))> _
    Partial Public Class TesoreriaOrdenesEncabezado
        Friend NotInheritable Class TesoreriaOrdenesEncabezadoMetadata
            Private Sub New()
                MyBase.New()
            End Sub
            <Display(Name:="ID Orden de Tesoreria", Description:="ID Orden de Tesoreria")> _
            Property lngID As Integer

            <Display(Name:="Tipo Producto", Description:="Tipo Producto")> _
            Property strTipoProducto As String

            <Display(Name:="Código Receptor", Description:="Código Receptor")> _
            Property strCodigoReceptor As String

            ', Description:="Nro Documento")> _
            <Display(Name:="Nro Documento")> _
            Property strNroDocumento As String

            <Display(Name:="Nro Documento", Description:="Nro Documento")> _
            Property lngNroDocumento As String

            <Display(Name:="Tipo Identificación", Description:="Tipo Identificación")> _
            Property strTipoIdentificacion As String

            ', Description:="Nombre"
            <Display(Name:="Nombre")> _
            Property strNombre As String

            <Display(Name:="Valor", Description:="Valor")> _
            Property curValor As Double

            ', Description:="ID Comitente"
            <Display(Name:="ID Comitente")> _
            Property strIDComitente As String

            <Display(Name:="Fecha Documento", Description:="FEcha Documento")> _
            Property dtmDocumento As Date

            <Display(Name:="Estado Orden Tesoreria", Description:="Estado Orden Tesoreria")> _
            Property strEstado As String

            <Display(Name:="Usuario", Description:="Usuario")> _
            Property strUsuario As String

        End Class
    End Class

    <MetadataTypeAttribute(GetType(TesoreriaOrdenesDetalleChequesOyDPlus.TesoreriaOrdenesDetalleChequesOyDPlusMetadata))> _
    Partial Public Class TesoreriaOrdenesDetalleChequesOyDPlus
        Friend NotInheritable Class TesoreriaOrdenesDetalleChequesOyDPlusMetadata
            Private Sub New()
                MyBase.New()
            End Sub
            <Display(Name:="ID Tesoreria Detalle", Description:="ID Tesoreria Detalle")> _
            Property lngID As Integer

            <Display(Name:="ID Tesoreria Encabezado", Description:="ID Tesoreria Encabezado")> _
            Property lngIDTesoreriaOrdenes As Integer

            <Display(Name:="Tipo", Description:="Tipo")> _
            Property strTipo As String

            <Display(Name:="Forma Pago", Description:="Forma Pago")> _
            Property strFormaPago As String

            <Display(Name:="ID Concepto", Description:="ID Concepto")> _
            Property lngIDConcepto As String

            <Display(Name:="Concepto", Description:="Concepto")> _
            Property strDetalleConcepto As String

            <Display(Name:="Nro Documento", Description:="Nro Documento")> _
            Property strNroDocumento As String

            <Display(Name:="Tipo Documento", Description:="Tipo Documento")> _
            Property strTipoDocumento As String

            <Display(Name:="Beneficiario", Description:="Beneficiario")> _
            Property strNombre As String

            <Display(Name:="Es Tercero", Description:="Es Tercero")> _
            Property strEsTercero As String

            <Display(Name:="Valor", Description:="Valor")> _
            Property curValor As Double

            <Display(Name:="Tipo Cobro GMF", Description:="Tipo Cobro GMF")> _
            Property strTipoCobroGMF As String

            <Display(Name:="Tipo Cheque", Description:="Tipo Cheque")> _
            Property strTipoCheque As String

            <Display(Name:="Nro Cheque", Description:="Nro Cheque")> _
            Property lngNroCheque As Integer

            <Display(Name:="Es Cuenta Inscrita", Description:="Es Cuenta Inscrita")> _
            Property strEsCuentaRegistrada As String

            <Display(Name:="Cuenta", Description:="Cuenta")> _
            Property strCuenta As String

            <Display(Name:="Tipo Cuenta", Description:="Tipo Cuenta")> _
            Property strTipoCuenta As String

            <Display(Name:="Banco", Description:="Banco")> _
            Property lngIdBanco As Integer

            <Display(Name:="Nombre Titular", Description:="Nombre Titular")> _
            Property strNombreTitular As String

            <Display(Name:="Tipo Documento Titular", Description:="Tipo Documento Titular")> _
            Property strTipoDocumentoTitular As String

            <Display(Name:="Nro Documento Titular", Description:="Nro Documento Titular")> _
            Property strNroDocumentoTitular As String

            <Display(Name:="Valor GMF", Description:="Valor GMF")> _
            Property curValorGMF As Double

            <Display(Name:="Fecha Actualización", Description:="Fecha Actualización")> _
            Property dtmFechaActualizacion As DateTime

            <Display(Name:="InfoSesion", Description:="InfoSesion")> _
            Property InfoSesion As String
        End Class
    End Class
#End Region

#Region "Generación Recibo de Caja con Datos de Deceval"

    <MetadataTypeAttribute(GetType(TesoreriaActualizarParam.TesoreriaActualizarParamMetadata))> _
    Partial Public Class TesoreriaActualizarParam
        Friend NotInheritable Class TesoreriaActualizarParamMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            Public Property Aprobacion As Byte

            Public Property infosesion As String

            Public Property pcurValor As Decimal

            Public Property pdtmActualizacion As DateTime

            Public Property pdtmContabilidadAntENC As DateTime

            Public Property pdtmContabilidadENC As DateTime

            Public Property pdtmContabilidadEncuenta As DateTime

            Public Property pdtmCreacion As DateTime

            Public Property pdtmDocumento As DateTime

            Public Property pdtmEstado As DateTime

            Public Property pFechadecontabilizacion_Contabilidad As String

            Public Property pFechaProceso_Contabilidad As DateTime

            Public Property pintErrorPersonalizado As Byte

            Public Property pintNroLote As Integer

            Public Property pintNroLoteAntENC As Integer

            Public Property plngCodComitente As String

            Public Property plngIDBanco As Integer

            Public Property plngIdNroOrden As Decimal

            Public Property plngIdNumInst As Integer

            Public Property plngIdSucursalBancaria As Integer

            Public Property plngImpresiones As Integer

            Public Property plngNroDocumento As String

            Public Property plngNumCheque As Double

            Public Property plogContabilidad As Boolean

            Public Property plogImpresionFisica As Boolean

            Public Property plogMultiCliente As Boolean

            Public Property plogRecaudoDirecto As Boolean

            Public Property plogSobregiro As Boolean

            Public Property pNumeroComprobante_Contabilidad As String

            Public Property pNumeroLote_Contabilidad As String

            Public Property pstrArchivoTransferencia As String

            Public Property pstrConcepto As String

            Public Property pstrCuentaCliente As String

            Public Property pstrDetalle As String

            Public Property pstrDetalleInstruccion As String

            Public Property pstrDVP As String

            Public Property pstrEstado As String

            Public Property pstrEstadoAutomatico As String

            Public Property pstrEstadoNovedadContabilidad As String

            Public Property pstrEstadoNovedadContabilidad_Anulada As String

            Public Property pstrFormaPagoCE As String

            Public Property pstrIdentificacionAutorizadoCheque As String

            Public Property pstrInstruccion As String

            Public Property pstrMontoEscrito As String

            Public Property pstrNombre As String

            Public Property pstrNombreAutorizadoCheque As String

            Public Property pstrNombreConsecutivo As String

            Public Property pstrParametroContable As String

            Public Property pstrTipo As String

            Public Property pstrTipoIdentificacion As String

            Public Property pstrTipoIntermedia As String

            Public Property pstrUsuario As String
        End Class
    End Class

#End Region

#Region "Generación Plano Comprobante de Egresos"

    <MetadataTypeAttribute(GetType(ComEgresosSeleccionar.ComEgresosSeleccionarMetadata))> _
    Partial Public Class ComEgresosSeleccionar
        Friend NotInheritable Class ComEgresosSeleccionarMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            Public Property Comprobante As Integer

            Public Property Consecutivo As String

            Public Property Beneficiario As String

            Public Property Banco As String

            Public Property Fecha As DateTime

            Public Property Valor As Double

            Public Property Imprimir As Boolean

        End Class
    End Class

#End Region

#Region "Nota Contables"

    <MetadataTypeAttribute(GetType(NotaContable_HeadDetail.NotaContable_HeadDetailMetadata))> _
    Partial Public Class NotaContable_HeadDetail
        Friend NotInheritable Class NotaContable_HeadDetailMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            Public Property pcurValor As Nullable(Of Decimal)

            Public Property pdtmCreacion As Nullable(Of DateTime)

            Public Property pdtmDocumento As Nullable(Of DateTime)

            Public Property pintNroDetalle As Nullable(Of Integer)

            Public Property plngComitente As String

            Public Property plngConsec As Nullable(Of Integer)

            Public Property plngIDBanco As Nullable(Of Integer)

            Public Property plngIdDocumento As Nullable(Of Integer)

            Public Property plngSecuencia As Nullable(Of Integer)

            Public Property plogContabilidad As Nullable(Of Boolean)

            Public Property pstrCCosto As String

            Public Property pstrDetalle As String

            Public Property pstrEstado As String

            Public Property pstrEstadoAprobacion As String

            Public Property pstrIdCuentaContable As String

            Public Property pstrNIT As String

            Public Property pstrNombreConsecutivo As String

            Public Property pstrTipo As String

            Public Property pstrUsuario As String
        End Class
    End Class

#End Region

#Region "Ordenes de Tesorería"
    <MetadataTypeAttribute(GetType(OrdenesTesoreri.OrdenesTesoreriMetadata))> _
    Partial Public Class OrdenesTesoreri
        Friend NotInheritable Class OrdenesTesoreriMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="IDOrdenTesoreria", Description:="IDOrdenTesoreria")> _
            Public Property IDOrdenTesoreria As Integer

            <Display(Name:="CodCliente", Description:="CodCliente")> _
            Public Property CodCliente As String

            <Display(Name:="NombreCliente", Description:="NombreCliente")> _
            Public Property NombreCliente As String

            <Display(Name:="Tipo", Description:="Tipo")> _
            Public Property Tipo As String

            <Display(Name:="ConsecutivoConsignacion", Description:="ConsecutivoConsignacion")> _
            Public Property ConsecutivoConsignacion As String

            <Display(Name:="Detalle", Description:="Detalle")> _
            Public Property Detalle As String

            <Display(Name:="ValorSaldo", Description:="ValorSaldo")> _
            Public Property ValorSaldo As Nullable(Of Decimal)

            <Display(Name:="Beneficiario", Description:="Beneficiario")> _
            Public Property Beneficiario As String

            <Display(Name:="IDBancoGirador", Description:="IDBancoGirador")> _
            Public Property IDBancoGirador As Nullable(Of Integer)

            <Display(Name:="NroCheque", Description:="NroCheque")> _
            Public Property NroCheque As Nullable(Of Double)

            <Display(Name:="FechaConsignacion", Description:="FechaConsignacion")> _
            Public Property FechaConsignacion As Nullable(Of DateTime)

            <Display(Name:="IDBancoRec", Description:="IDBancoRec")> _
            Public Property IDBancoRec As Nullable(Of Integer)

            <Display(Name:="FormaPago", Description:="FormaPago")> _
            Public Property FormaPago As String

            <Display(Name:="CtaContable", Description:="CtaContable")> _
            Public Property CtaContable As String

            <Display(Name:="CtaContableContraP", Description:="CtaContableContraP")> _
            Public Property CtaContableContraP As String

            <Display(Name:="EstadoImpresion", Description:="EstadoImpresion")> _
            Public Property EstadoImpresion As Integer

            <Display(Name:="Procesado", Description:="Procesado")> _
            Public Property Procesado As Integer

            <Display(Name:="Notas", Description:="Notas")> _
            Public Property Notas As String

            <Display(Name:="TipoSello", Description:="TipoSello")> _
            Public Property TipoSello As String

            <Display(Name:="CreacionOrden", Description:="CreacionOrden")> _
            Public Property CreacionOrden As Nullable(Of DateTime)

            <Display(Name:="Login", Description:="Login")> _
            Public Property Login As String

            <Display(Name:="FechaActualizacion", Description:="FechaActualizacion")> _
            Public Property FechaActualizacion As DateTime
        End Class
    End Class
#End Region

#Region "Pendientes Tesorería"
    <MetadataTypeAttribute(GetType(PendientesTesoreri.PendientesTesoreriMetadata))> _
    Partial Public Class PendientesTesoreri
        Friend NotInheritable Class PendientesTesoreriMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Required(ErrorMessage:="Este campo es requerido. (ID)")> _
              <Display(Name:="ID", Description:="ID")> _
            Public Property ID As Integer

            <Required(ErrorMessage:="Este campo es requerido. (NombreConsecutivo)")> _
              <Display(Name:="NombreConsecutivo", Description:="NombreConsecutivo")> _
            Public Property NombreConsecutivo As String

            <Required(ErrorMessage:="Este campo es requerido. (IDDocumento)")> _
              <Display(Name:="IDDocumento", Description:="IDDocumento")> _
            Public Property IDDocumento As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Nombre)")> _
              <Display(Name:="Nombre", Description:="Nombre")> _
            Public Property Nombre As String

            <Required(ErrorMessage:="Este campo es requerido. (Documento)")> _
              <Display(Name:="Documento", Description:="Documento")> _
            Public Property Documento As DateTime

            <Required(ErrorMessage:="Este campo es requerido. (Sobregiro)")> _
              <Display(Name:="Sobregiro", Description:="Sobregiro")> _
            Public Property Sobregiro As Nullable(Of Boolean)

        End Class
    End Class
#End Region
    <MetadataTypeAttribute(GetType(AjustesBancario.AjustesBancarioMetadata))> _
    Partial Public Class AjustesBancario
        Friend NotInheritable Class AjustesBancarioMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            '<Display(Name:="IDComisionista", Description:="IDComisionista")> _
            'Public Property IDComisionista As Integer

            '<Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
            'Public Property IDSucComisionista As Integer

            '<Display(Name:=" ")> _
            'Public Property Tipo As String

            <Display(Name:="Tipo")> _
            <Required(ErrorMessage:="El campo {0}, es requerido.")> _
            Public Property NombreConsecutivo As String

            <Display(Name:="Número")> _
            Public Property IDDocumento As Integer

            <Display(Name:="Banco")> _
            Public Property IDBanco As Nullable(Of Integer)

            <Display(Name:="Fecha")> _
            Public Property Documento As DateTime

            <Display(Name:="Estado")> _
            Public Property Estado As String

            <Display(Name:="Fecha Estado")> _
            Public Property FecEstado As DateTime

            <Display(Name:=" ")> _
            Public Property Impresiones As Integer

            <Display(Name:=" ")> _
            Public Property Nombre As String

            <Display(Name:="Detalle")> _
            <Required(ErrorMessage:="Debe ingresar un Detalle")> _
            Public Property Detalle As String

        End Class
    End Class

End Namespace

