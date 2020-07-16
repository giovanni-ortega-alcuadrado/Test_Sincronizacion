Option Compare Binary
Option Infer On
Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Data.Linq
Imports System.Linq

Namespace OyDOrdenes

    '**************************************************************************************************
    <MetadataTypeAttribute(GetType(Orden.OrdenMetadata))> _
    Partial Public Class Orden
        Friend NotInheritable Class OrdenMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="Tipo")> _
            Public Property Tipo As String

            <Display(Name:="Clase")> _
            Public Property Clase As String

            <Display(Name:="Número")> _
            Public Property NroOrden As Integer

            '<[ReadOnly](True)> _
            <Display(Name:="Versión")> _
            Public Property Version As Integer

            <Display(Name:="Ordinaria")> _
            Public Property Ordinaria As Boolean

            <Display(Name:="Clasificación")> _
            Public Property Objeto As String

            <Display(Name:="Estado")> _
            Public Property Estado As String

            <Display(Name:="Cambio de estado")> _
            Public Property FechaEstado As DateTime

            <Display(Name:="Es repo")> _
            Public Property Repo As Boolean

            <Display(Name:="ID Comitente")> _
            Public Property IDComitente As String

            <Display(Name:="ID Ordenante")> _
            Public Property IDOrdenante As String

            <Display(Name:="Comisión pactada")> _
            Public Property ComisionPactada As Double

            <Display(Name:="Condiciones negociación")> _
            Public Property CondicionesNegociacion As String

            <Display(Name:="Tipo límite - Naturaleza")> _
            Public Property TipoLimite As String

            <Display(Name:="Forma pago")> _
            Public Property FormaPago As String

            <Description("Fecha de elaboración de la orden")> _
            <Display(Name:="Elaboración")> _
            Public Property FechaOrden As DateTime

            <Description("Fecha de elaboración de la orden")> _
            <Display(Name:="Vigencia cliente")> _
            Public Property VigenciaHasta As Nullable(Of DateTime)

            <Display(Name:="Instrucciones")> _
            Public Property Instrucciones As String

            <Display(Name:="Notas")> _
            Public Property Notas As String

            <Display(Name:="Sistema")> _
            Public Property FechaSistema As DateTime

            <Display(Name:="Ubicacion título")> _
            Public Property UBICACIONTITULO As String

            <Display(Name:="Cuenta depósito")> _
            Public Property CuentaDeposito As Nullable(Of Integer)

            '<Required(ErrorMessage:="El tipo de inversión para la orden es requerida")> _
            <Display(Name:="Tipo de inversión")> _
            Public Property TipoInversion As String

            <Display(Name:="Producto financiero")> _
            Public Property IDProducto As Integer

            <Display(Name:="Estado LEO")> _
            Public Property EstadoLEO As String

            <Display(Name:="Usuario operador")> _
            Public Property UsuarioOperador As String

            <Display(Name:="Canal recepción")> _
            Public Property CanalRecepcion As String

            <Display(Name:="Medio verificable")> _
            Public Property MedioVerificable As String

            <Display(Name:="Fecha de recepción")> _
            Public Property FechaRecepcion As Nullable(Of DateTime)

            <Display(Name:="Receptor toma")> _
            Public Property IdReceptorToma As String

            <Display(Name:="Nro. extensión")> _
            Public Property NroExtensionToma As String

            '<Required(ErrorMessage:="El tipo de transacción es requerido")> _
            <Display(Name:="Tipo transacción")> _
            Public Property TipoTransaccion As String

            <Display(Name:="Ejecución")> _
            Public Property Ejecucion As String

            <Display(Name:="Duración")> _
            Public Property Duracion As String

            <Display(Name:="Cantidad mínima")> _
            Public Property CantidadMinima As Nullable(Of Decimal)

            <Display(Name:="Cantidad visible")> _
            Public Property CantidadVisible As Nullable(Of Decimal)

            <Display(Name:="Precio stop")> _
            Public Property PrecioStop As Nullable(Of Decimal)

            <Display(Name:="Hora vigencia")> _
            Public Property HoraVigencia As String

            <Display(Name:="EstadoOrdenBus")> _
            Public Property EstadoOrdenBus As String

            <Display(Name:="Costos adicionales orden")> _
            Public Property CostoAdicionalesOrden As Nullable(Of Double)

            <Required(ErrorMessage:="La bolsa en la cual se negocia la orden es requerida (Bolsa)")> _
            <Display(Name:="Bolsa")> _
            Public Property IdBolsa As Integer

            <Display(Name:="Usuario ingreso")> _
            Public Property UsuarioIngreso As String

            <Display(Name:="Negocio especial")> _
            Public Property NegocioEspecial As String

            <Display(Name:="Referenciada")> _
            Public Property Eca As Boolean

            <Display(Name:="Nro Swap")> _
            Public Property ConsecutivoSwap As Nullable(Of Integer)

            <Display(Name:="Sitio ingreso")> _
            Public Property SitioIngreso As String

            <Display(Name:="Nemotécnico")> _
            Public Property Nemotecnico As String

            <Display(Name:="Prioridad")> _
            Public Property Prioridad As Byte

            <Display(Name:="Emisión")> _
            Public Property FechaEmision As Nullable(Of DateTime)

            <Display(Name:="Vencimiento")> _
            Public Property FechaVencimiento As Nullable(Of DateTime)

            <Display(Name:="Cumplimiento")> _
            Public Property FechaCumplimiento As Nullable(Of DateTime)

            <Display(Name:="Tasa Compra-Venta")> _
            Public Property TasaCompraVenta As Nullable(Of Double)

            <Display(Name:="Cantidad")> _
            Public Property Cantidad As Double

            <Display(Name:="Modalidad")> _
            Public Property Modalidad As String

            <Display(Name:="Mercado")> _
            Public Property Mercado As String

            <Display(Name:="TasaNominal")> _
            Public Property TasaNominal As Nullable(Of Double)

            <Display(Name:="Indicador económico")> _
            Public Property IndicadorEconomico As String

            <Display(Name:="Puntos indicador")> _
            Public Property PuntosIndicador As Nullable(Of Double)

            <Display(Name:="Precio")> _
            Public Property Precio As Nullable(Of Double)

            <Display(Name:="Precio compra/venta")> _
            Public Property PrecioCompraVenta As Nullable(Of Double)

            <Display(Name:="Cantidad en Pesos")> _
            Public Property EnPesos As Boolean

            <Display(Name:="TasaInicial")> _
            Public Property TasaInicial As Nullable(Of Double)

            <Display(Name:="Dividendo")> _
            Public Property DividendoCompra As Boolean

            <Display(Name:="Tasa efectiva inferior")> _
            Public Property EfectivaInferior As Nullable(Of Double)

            <Display(Name:="Tasa efectiva superior")> _
            Public Property EfectivaSuperior As Nullable(Of Double)

            <Display(Name:="Días vencimiento inferior")> _
            Public Property DiasVencimientoInferior As Nullable(Of Integer)

            <Display(Name:="Días vencimiento superior")> _
            Public Property DiasVencimientoSuperior As Nullable(Of Integer)

            <Display(Name:="Ciudad Seteo")> _
            Public Property IdCiudadSeteo As Nullable(Of Integer)

            <Display(Name:="Precio registro")> _
            Public Property PrecioRegistro As Nullable(Of Double)

            <Display(Name:="Valor Liq")> _
            Public Property ValorLiq As Nullable(Of Double)

            <Display(Name:="Emisión DCV")> _
            Public Property EmisionDCV As String

            <Display(Name:="Valor futuro repo")> _
            Public Property ValorFuturoRepo As Nullable(Of Double)

            'Propiedades de Ordenes de OYDPLUS
            <Display(Name:="Orden OYDPLUS")> _
            Public Property OrdenOyDPlus As Boolean

            <Display(Name:="Tipo Negocio")> _
            Public Property TipoNegocio As String

            <Display(Name:="Tipo Orden")> _
            Public Property TipoOrden As String

            <Display(Name:="Tipo Producto")> _
            Public Property TipoProducto As String

            <Display(Name:="Dias repo")> _
            Public Property DiasRepo As Nullable(Of Integer)

            <Display(Name:="BROKEN TRADE")> _
            Public Property BrokenTrader As String

        End Class
    End Class

    '**************************************************************************************************
    <MetadataTypeAttribute(GetType(LiqAsociadasOrden.LiqAsociadasOrdenMetadata))> _
    Partial Public Class LiqAsociadasOrden
        Friend NotInheritable Class LiqAsociadasOrdenMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="Nro. liquidacion", Description:="Nro. de la liquidacion")> _
            Public Property NroLiquidacion As Integer

            <Display(Name:="Parcial", Description:="Parcial")> _
            Public Property Parcial As Nullable(Of Integer)

            <Required(ErrorMessage:="La Fecha de Liquidación probable es requerida")> _
            <Display(Name:="Fecha liquidación", Description:="Fecha de la iquidación")> _
            Public Property FechaLiquidacion As Nullable(Of DateTime)

        End Class
    End Class

    '**************************************************************************************************
    <MetadataTypeAttribute(GetType(ReceptoresOrden.ReceptoresOrdenMetadata))> _
    Partial Public Class ReceptoresOrden
        Friend NotInheritable Class ReceptoresOrdenMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="Receptor", Description:="Código del receptor que participa en la orden")> _
            Public Property IDReceptor As String

            <Display(Name:="Líder", Description:="Indica si el recptor es el líder del negocio")> _
            Public Property Lider As Boolean

            <Display(Name:="Porcentaje", Description:="Porcentaje de participación del receptor en la orden")> _
            Public Property Porcentaje As Double

        End Class
    End Class

    <MetadataTypeAttribute(GetType(ReceptorOrdenesOF.ReceptorOrdenesOFMetadata))> _
    Partial Public Class ReceptorOrdenesOF
        Friend NotInheritable Class ReceptorOrdenesOFMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="Receptor", Description:="Código del receptor que participa en la orden")> _
            Public Property IDReceptor As String

            <Display(Name:="Líder", Description:="Indica si el recptor es el líder del negocio")> _
            Public Property Lider As Boolean

            <Display(Name:="Porcentaje", Description:="Porcentaje de participación del receptor en la orden")> _
            Public Property Porcentaje As Double

        End Class
    End Class
    '**************************************************************************************************
    '<MetadataTypeAttribute(GetType(InstruccionesOrden.InstruccionesOrdenMetadata))> _
    'Partial Public Class InstruccionesOrden
    '    Friend NotInheritable Class InstruccionesOrdenMetadata
    '        Private Sub New()
    '            MyBase.New()
    '        End Sub

    '        <Display(Name:="IdBolsa", Description:="IdBolsa")> _
    '        Public Property IdBolsa As Integer

    '        <Display(Name:="Retorno", Description:="Código de la instrucción")> _
    '        Public Property IdInstruccion As String

    '        <Display(Name:="Instrucción", Description:="Instruccion")> _
    '        Public Property Instruccion As String

    '        <Display(Name:="Cuenta", Description:="Cuenta")> _
    '        Public Property Cuenta As String

    '        <Display(Name:="Valor", Description:="Valor")> _
    '        Public Property Valor As Nullable(Of Double)

    '        <Display(Name:="Seleccionado", Description:="Seleccionado")> _
    '        Public Property Seleccionado As Boolean

    '        <Display(Name:="Cumplido", Description:="Cumplido")> _
    '        Public Property Cumplido As Integer

    '        <Display(Name:="Aprobado", Description:="Aprobado")> _
    '        Public Property Aprobado As Boolean

    '    End Class
    'End Class

    '**************************************************************************************************
    <MetadataTypeAttribute(GetType(InstruccionesOrdene.InstruccionesOrdeneMetadata))> _
    Partial Public Class InstruccionesOrdene
        Friend NotInheritable Class InstruccionesOrdeneMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="IDComisionista", Description:="IDComisionista")> _
            Public Property IDComisionista As Integer

            <Display(Name:="IdSucComisionista", Description:="IdSucComisionista")> _
            Public Property IdSucComisionista As Integer

            '<Required(ErrorMessage:="Este campo es requerido. (Tipo)")> _
            <Display(Name:="Tipo", Description:="Tipo")> _
            Public Property Tipo As String

            '<Required(ErrorMessage:="Este campo es requerido. (Clase)")> _
            <Display(Name:="Clase", Description:="Clase")> _
            Public Property Clase As String

            '<Required(ErrorMessage:="Este campo es requerido. (ID)")> _
            <Display(Name:="ID", Description:="ID")> _
            Public Property ID As Integer

            '<Required(ErrorMessage:="Este campo es requerido. (Version)")> _
            <Display(Name:="Version", Description:="Versión")> _
            Public Property Version As Integer

            '<Required(ErrorMessage:="Este campo es requerido. (IdBolsa)")> _
            <Display(Name:="IdBolsa", Description:="IdBolsa")> _
            Public Property IdBolsa As Integer

            '<Required(ErrorMessage:="Este campo es requerido. (Retorno)")> _
            <Display(Name:="Retorno", Description:="Retorno")> _
            Public Property Retorno As String

            '<Required(ErrorMessage:="Este campo es requerido. (Instruccion)")> _
            <Display(Name:="Instruccion", Description:="Instruccion")> _
            Public Property Instruccion As String

            '<Required(ErrorMessage:="Este campo es requerido. (Cuenta)")> _
            <Display(Name:="Cuenta", Description:="Cuenta")> _
            Public Property Cuenta As String

            '<Required(ErrorMessage:="Este campo es requerido. (Valor)")> _
            <Display(Name:="Valor", Description:="Valor")> _
            Public Property Valor As Nullable(Of Double)

            '<Required(ErrorMessage:="Este campo es requerido. (Seleccionado)")> _
            <Display(Name:="Seleccionado", Description:="Seleccionado")> _
            Public Property Seleccionado As Boolean

            <Display(Name:="Cumplido", Description:="Cumplido")> _
            Public Property Cumplido As Integer

            <Display(Name:="Aprobado", Description:="Aprobado")> _
            Public Property Aprobado As Boolean

            '<Required(ErrorMessage:="Este campo es requerido. (Actualizacion)")> _
            <Display(Name:="Actualizacion", Description:="Actualizacion")> _
            Public Property Actualizacion As DateTime

            '<Required(ErrorMessage:="Este campo es requerido. (Usuario)")> _
            <Display(Name:="Usuario", Description:="Usuario")> _
            Public Property Usuario As String

            <Display(Name:="IDInstruccionesOrdenes", Description:="IDInstruccionesOrdenes")> _
            Public Property IDInstruccionesOrdenes As Integer

        End Class
    End Class

    '**************************************************************************************************
    <MetadataTypeAttribute(GetType(OrdenesPago.OrdenesPagoMetadata))> _
    Partial Public Class OrdenesPago
        Friend NotInheritable Class OrdenesPagoMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Required(ErrorMessage:="Este campo es requerido. (IDComisionista)")> _
              <Display(Name:="IDComisionista", Description:="IDComisionista")> _
            Public Property IDComisionista As Integer

            <Required(ErrorMessage:="Este campo es requerido. (IDSucComisionista)")> _
              <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
            Public Property IDSucComisionista As Integer

            <Required(ErrorMessage:="Este campo es requerido. (TipoOrden)")> _
              <Display(Name:="TipoOrden", Description:="TipoOrden")> _
            Public Property TipoOrden As String

            <Required(ErrorMessage:="Este campo es requerido. (ClaseOrden)")> _
              <Display(Name:="ClaseOrden", Description:="ClaseOrden")> _
            Public Property ClaseOrden As String

            <Required(ErrorMessage:="Este campo es requerido. (IDOrden)")> _
              <Display(Name:="IDOrden", Description:="IDOrden")> _
            Public Property IDOrden As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Version)")> _
              <Display(Name:="Version", Description:="Version")> _
            Public Property Version As Integer

            <Required(ErrorMessage:="Este campo es requerido. (IdBolsa)")> _
              <Display(Name:="IdBolsa", Description:="IdBolsa")> _
            Public Property IdBolsa As Integer

            '<Required(ErrorMessage:="Este campo es requerido. (FormaPago)")> _
            <Display(Name:="Forma Cumplimiento")> _
            Public Property FormaPago As String

            <Required(ErrorMessage:="Este campo es requerido. (CumpPesos)")> _
              <Display(Name:="CumpPesos", Description:="CumpPesos")> _
            Public Property logCumpPesos As Boolean

            <Required(ErrorMessage:="Este campo es requerido. (CumpTitulo)")> _
              <Display(Name:="CumpTitulo", Description:="CumpTitulo")> _
            Public Property logCumpTitulo As Boolean

            <Display(Name:="Tipo Cumplimiento")> _
            Public Property CumpPesos As String

            <Display(Name:="CumpTitulo", Description:="CumpTitulo")> _
            Public Property CumpTitulo As String

            <Display(Name:="Deposito")> _
            Public Property DeposPesos As String

            <Display(Name:="Cuenta Sebra")> _
            Public Property CtaSebraPesos As String

            <Display(Name:="DeposTitulo", Description:="DeposTitulo")> _
            Public Property DeposTitulo As String

            <Display(Name:="Cuenta Custodio")> _
            Public Property CtaTitulo As String

            <Required(ErrorMessage:="Este campo es requerido. (Actualizacion)")> _
              <Display(Name:="Actualizacion", Description:="Actualizacion")> _
            Public Property Actualizacion As DateTime

            <Required(ErrorMessage:="Este campo es requerido. (Usuario)")> _
              <Display(Name:="Usuario", Description:="Usuario")> _
            Public Property Usuario As String

            <Display(Name:="IDOrdenesPagos", Description:="IDOrdenesPagos")> _
            Public Property IDOrdenesPagos As Integer

        End Class
    End Class

    '**************************************************************************************************
    <MetadataTypeAttribute(GetType(AdicionalesOrdene.AdicionalesOrdeneMetadata))> _
    Partial Public Class AdicionalesOrdene
        Friend NotInheritable Class AdicionalesOrdeneMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="IDComisionista", Description:="IDComisionista")> _
            Public Property IDComisionista As Integer

            <Display(Name:="IdSucComisionista", Description:="IdSucComisionista")> _
            Public Property IdSucComisionista As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Tipo)")> _
              <Display(Name:="Tipo", Description:="Tipo")> _
            Public Property Tipo As String

            <Display(Name:="Clase", Description:="Clase")> _
            Public Property Clase As String

            <Display(Name:="ID", Description:="ID")> _
            Public Property ID As Integer

            <Display(Name:="Version", Description:="Version")> _
            Public Property Version As Integer

            <Display(Name:="IdBolsa", Description:="IdBolsa")> _
            Public Property IdBolsa As Integer

            <Required(ErrorMessage:="Este campo es requerido. (IdComision)")> _
              <Display(Name:="IdComision", Description:="IdComision")> _
            Public Property IdComision As Integer

            <Required(ErrorMessage:="Este campo es requerido. (PorcCompra)")> _
              <Display(Name:="PorcCompra", Description:="PorcCompra")> _
            Public Property PorcCompra As Double

            <Required(ErrorMessage:="Este campo es requerido. (PorcVenta)")> _
              <Display(Name:="PorcVenta", Description:="PorcVenta")> _
            Public Property PorcVenta As Double

            <Required(ErrorMessage:="Este campo es requerido. (PorcOtro)")> _
              <Display(Name:="PorcOtro", Description:="PorcOtro")> _
            Public Property PorcOtro As Double

            <Required(ErrorMessage:="Este campo es requerido. (IdOperacion)")> _
              <Display(Name:="IdOperacion", Description:="IdOperacion")> _
            Public Property IdOperacion As Integer

            <Required(ErrorMessage:="Este campo es requerido. (ComisionSugerida)")> _
              <Display(Name:="ComisionSugerida", Description:="ComisionSugerida")> _
            Public Property ComisionSugerida As Double

            <Display(Name:="Actualizacion", Description:="Actualizacion")> _
            Public Property Actualizacion As DateTime

            <Display(Name:="Usuario", Description:="Usuario")> _
            Public Property Usuario As String

            <Display(Name:="IDAdicionalesOrdenes", Description:="IDAdicionalesOrdenes")> _
            Public Property IDAdicionalesOrdenes As Integer

        End Class
    End Class

    '**************************************************************************************************

#Region "Ordenes MILA"

    <MetadataTypeAttribute(GetType(Orden_MI.Orden_MIMetadata))> _
    Partial Public Class Orden_MI
        Friend NotInheritable Class Orden_MIMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="Tipo")> _
            Public Property Tipo As String

            <Display(Name:="Clase")> _
            Public Property Clase As String

            <Display(Name:="Número")> _
            Public Property NroOrden As Integer

            '<[ReadOnly](True)> _
            <Display(Name:="Versión")> _
            Public Property Version As Integer

            <Display(Name:="Ordinaria")> _
            Public Property Ordinaria As Boolean

            '<Required(AllowEmptyStrings:=True, ErrorMessage:="La clasificación de la orden es requerida")> _
            <Display(Name:="Clasificación")> _
            Public Property Objeto As String

            <Display(Name:="Estado")> _
            Public Property Estado As String

            <Display(Name:="Cambio de estado")> _
            Public Property FechaEstado As DateTime

            <Display(Name:="Es repo")> _
            Public Property Repo As Boolean

            '<Required(ErrorMessage:="El comitente de la orden es requerido")> _
            <Display(Name:="ID Comitente")> _
            Public Property IDComitente As String

            ' <Required(ErrorMessage:="El ordenante de la orden es requerido")> _
            <Display(Name:="ID Ordenante")> _
            Public Property IDOrdenante As String

            <Display(Name:="Comisión %")> _
            Public Property ComisionPactada As Double

            <Display(Name:="Tasa Conversión Pesos")> _
            Public Property TasaConversion As Double

            <Display(Name:="Tasa Conversión Dólares")> _
            Public Property TasaConversionDolares As Double

            <Display(Name:="Comisión $")> _
            Public Property ComisionPesos As Double

            '<Required(ErrorMessage:="La condición de negociación para la orden es requerida")> _
            <Display(Name:="Condiciones negociación")> _
            Public Property CondicionesNegociacion As String

            '<Required(ErrorMessage:="La naturaleza de la orden es requerida")> _
            <Display(Name:="Tipo límite - Naturaleza")> _
            Public Property TipoLimite As String

            '<Required(ErrorMessage:="La forma de pago para la orden es requerida")> _
            <Display(Name:="Forma pago")> _
            Public Property FormaPago As String

            '<Required(ErrorMessage:="La fecha de elaboración de la orden es requerida")> _
            <Description("Fecha de elaboración de la orden")> _
            <Display(Name:="Elaboración")> _
            Public Property FechaOrden As DateTime

            '<Required(ErrorMessage:="La vigencia de la orden es requerida")> _
            <Description("Fecha de elaboración de la orden")> _
            <Display(Name:="Vigencia Hasta")> _
            Public Property VigenciaHasta As Nullable(Of DateTime)

            <Display(Name:="Instrucciones")> _
            Public Property Instrucciones As String

            <Display(Name:="Notas")> _
            Public Property Notas As String

            <Display(Name:="Sistema")> _
            Public Property FechaSistema As DateTime

            <Display(Name:="Ubicacion título")> _
            Public Property UBICACIONTITULO As String

            <Display(Name:="Cuenta depósito")> _
            Public Property CuentaDeposito As Nullable(Of Integer)

            '<Required(ErrorMessage:="El tipo de inversión para la orden es requerida")> _
            <Display(Name:="Tipo de inversión")> _
            Public Property TipoInversion As String

            <Display(Name:="Producto financiero")> _
            Public Property IDProducto As Integer

            '<Required(ErrorMessage:="El usuario operador es requerido")> _
            <Display(Name:="Estado LEO")> _
            Public Property EstadoLEO As String

            '<Required(ErrorMessage:="El usuario operador es requerido")> _
            <Display(Name:="Usuario operador")> _
            Public Property UsuarioOperador As String

            '<Required(ErrorMessage:="El canal de recepción de la orden es requerido")> _
            <Display(Name:="Canal recepción")> _
            Public Property CanalRecepcion As String

            '<Required(ErrorMessage:="El medio verificable de la orden es requerido")> _
            <Display(Name:="Medio verificable")> _
            Public Property MedioVerificable As String

            '<Required(ErrorMessage:="La fecha de recepción de la orden es requerida")> _
            <Display(Name:="Fecha de recepción")> _
            Public Property FechaRecepcion As Nullable(Of DateTime)

            <Display(Name:="Receptor toma")> _
            Public Property IdReceptorToma As String

            <Display(Name:="Nro. extensión")> _
            Public Property NroExtensionToma As String

            '<Required(ErrorMessage:="El tipo de transacción es requerido")> _
            <Display(Name:="Tipo transacción")> _
            Public Property TipoTransaccion As String

            <Display(Name:="Ejecución")> _
            Public Property Ejecucion As String

            <Display(Name:="Duración")> _
            Public Property Duracion As String

            <Display(Name:="Cantidad mínima")> _
            Public Property CantidadMinima As Nullable(Of Decimal)

            <Display(Name:="Cantidad visible")> _
            Public Property CantidadVisible As Nullable(Of Decimal)

            <Display(Name:="Precio stop")> _
            Public Property PrecioStop As Nullable(Of Decimal)

            <Display(Name:="Hora vigencia")> _
            Public Property HoraVigencia As String

            <Display(Name:="EstadoOrdenBus")> _
            Public Property EstadoOrdenBus As String

            <Display(Name:="Costos adicionales orden")> _
            Public Property CostoAdicionalesOrden As Nullable(Of Double)

            '<Required(ErrorMessage:="La bolsa en la cual se negocia la orden es requerida (Bolsa)")> _
            <Display(Name:="Bolsa Extranjera")> _
            Public Property IdBolsa As Integer

            <Display(Name:="Usuario ingreso")> _
            Public Property UsuarioIngreso As String

            <Display(Name:="Negocio especial")> _
            Public Property NegocioEspecial As String

            <Display(Name:="Referenciada")> _
            Public Property Eca As Boolean

            <Display(Name:="Nro. Swap")> _
            Public Property ConsecutivoSwap As Nullable(Of Integer)

            <Display(Name:="Sitio ingreso")> _
            Public Property SitioIngreso As String

            '<Required(ErrorMessage:="La especie de la orden es requerida")> _
            <Display(Name:="Nemotécnico")> _
            Public Property Nemotecnico As String

            <Display(Name:="Prioridad")> _
            Public Property Prioridad As Byte

            <Display(Name:="Emisión")> _
            Public Property FechaEmision As Nullable(Of DateTime)

            <Display(Name:="Vencimiento")> _
            Public Property FechaVencimiento As Nullable(Of DateTime)

            <Display(Name:="Fecha Cumplimiento")> _
            Public Property FechaCumplimiento As Nullable(Of DateTime)

            <Display(Name:="Tasa Compra-Venta")> _
            Public Property TasaCompraVenta As Nullable(Of Double)

            '<Required(ErrorMessage:="La cantidad o nominal de la orden es requerida")> _
            <Display(Name:="Cantidad")> _
            Public Property Cantidad As Double

            <Display(Name:="Modalidad")> _
            Public Property Modalidad As String

            <Display(Name:="Mercado")> _
            Public Property Mercado As String

            <Display(Name:="Tasa Nominal")> _
            Public Property TasaNominal As Nullable(Of Double)

            <Display(Name:="Indicador económico")> _
            Public Property IndicadorEconomico As String

            <Display(Name:="Puntos indicador")> _
            Public Property PuntosIndicador As Nullable(Of Double)

            <Display(Name:="Precio")> _
            Public Property Precio As Nullable(Of Double)

            <Display(Name:="Precio Inferior")> _
            Public Property PrecioInferior As Nullable(Of Double)

            <Display(Name:="Precio compra/venta")> _
            Public Property PrecioCompraVenta As Nullable(Of Double)

            <Display(Name:="Cantidad en Moneda")> _
            Public Property EnPesos As Boolean

            <Display(Name:="TasaInicial")> _
            Public Property TasaInicial As Nullable(Of Double)

            <Display(Name:="Div.")> _
            Public Property DividendoCompra As Boolean

            <Display(Name:="Tasa efectiva inferior")> _
            Public Property EfectivaInferior As Nullable(Of Double)

            <Display(Name:="Tasa efectiva superior")> _
            Public Property EfectivaSuperior As Nullable(Of Double)

            <Display(Name:="Días vencimiento inferior")> _
            Public Property DiasVencimientoInferior As Nullable(Of Integer)

            <Display(Name:="Días vencimiento superior")> _
            Public Property DiasVencimientoSuperior As Nullable(Of Integer)

            <Display(Name:="Ciudad Seteo")> _
            Public Property IdCiudadSeteo As Nullable(Of Integer)

            <Display(Name:="Precio registro")> _
            Public Property PrecioRegistro As Nullable(Of Double)

            <Display(Name:="Valor Liq")> _
            Public Property ValorLiq As Nullable(Of Double)

            <Display(Name:="Emisión DCV")> _
            Public Property EmisionDCV As String

            <Display(Name:="Valor futuro repo")> _
            Public Property ValorFuturoRepo As Nullable(Of Double)

        End Class
    End Class

#End Region

#Region "OrdenesOF"
    <MetadataTypeAttribute(GetType(OrdenesOF.OrdenesOFMetadata))> _
    Partial Public Class OrdenesOF
        Friend NotInheritable Class OrdenesOFMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="Tipo")> _
            Public Property Tipo As String

            <Display(Name:="Clase")> _
            Public Property Clase As String

            <Display(Name:="Número")> _
            Public Property NroOrden As Integer

            '<[ReadOnly](True)> _
            <Display(Name:="Versión")> _
            Public Property Version As Integer

            <Display(Name:="Ordinaria")> _
            Public Property Ordinaria As Boolean

            <Display(Name:="Clasificación")> _
            Public Property Objeto As String

            <Display(Name:="Estado")> _
            Public Property Estado As String

            <Display(Name:="Cambio de estado")> _
            Public Property FechaEstado As DateTime

            <Display(Name:="Es repo")> _
            Public Property Repo As Boolean

            <Display(Name:="ID Comitente")> _
            Public Property IDComitente As String

            <Display(Name:="ID Ordenante")> _
            Public Property IDOrdenante As String

            <Display(Name:="Comisión pactada")> _
            Public Property ComisionPactada As Double

            <Display(Name:="Condiciones negociación")> _
            Public Property CondicionesNegociacion As String

            <Display(Name:="Tipo límite - Naturaleza")> _
            Public Property TipoLimite As String

            <Display(Name:="Forma pago")> _
            Public Property FormaPago As String

            <Description("Fecha de elaboración de la orden")> _
            <Display(Name:="Elaboración")> _
            Public Property FechaOrden As DateTime

            <Description("Fecha de elaboración de la orden")> _
            <Display(Name:="Vigencia cliente")> _
            Public Property VigenciaHasta As Nullable(Of DateTime)

            <Display(Name:="Instrucciones")> _
            Public Property Instrucciones As String

            <Display(Name:="Notas")> _
            Public Property Notas As String

            <Display(Name:="Sistema")> _
            Public Property FechaSistema As DateTime

            <Display(Name:="Ubicacion título")> _
            Public Property UBICACIONTITULO As String

            <Display(Name:="Tipo de inversión")> _
            Public Property TipoInversion As String

            <Display(Name:="Precio inferior")> _
            Public Property PrecioInferior As Nullable(Of Double)

            <Display(Name:="Precio")> _
            Public Property Precio As Nullable(Of Double)

            <Required(ErrorMessage:="La especie de la orden es requerida")> _
            <Display(Name:="Nemotécnico")> _
            Public Property Nemotecnico As String

            <Display(Name:="Prioridad")> _
            Public Property Prioridad As Byte

            <Display(Name:="Emisión")> _
            Public Property FechaEmision As Nullable(Of DateTime)

            <Display(Name:="Vencimiento")> _
            Public Property FechaVencimiento As Nullable(Of DateTime)

            <Display(Name:="Cumplimiento")> _
            Public Property FechaCumplimiento As Nullable(Of DateTime)

            <Display(Name:="Cantidad")> _
            Public Property Cantidad As Double

            <Display(Name:="Modalidad")> _
            Public Property Modalidad As String

            <Display(Name:="Mercado")> _
            Public Property Mercado As String

            <Display(Name:="Tasa nominal")> _
            Public Property TasaNominal As Nullable(Of Double)

            <Display(Name:="Cantidad en pesos")> _
            Public Property EnPesos As Boolean

            <Display(Name:="Tasa inicial")> _
            Public Property TasaInicial As Nullable(Of Double)

            <Display(Name:="Dividendo")> _
            Public Property DividendoCompra As Boolean

            <Display(Name:="Tasa efectiva inferior")> _
            Public Property EfectivaInferior As Nullable(Of Double)

            <Display(Name:="Tasa efectiva superior")> _
            Public Property EfectivaSuperior As Nullable(Of Double)

            <Display(Name:="Días vencimiento inferior")> _
            Public Property DiasVencimientoInferior As Nullable(Of Integer)

            <Display(Name:="Días vencimiento superior")> _
            Public Property DiasVencimientoSuperior As Nullable(Of Integer)

        End Class
    End Class

    '**************************************************************************************************
    '<MetadataTypeAttribute(GetType(LiqAsociadasOrden.LiqAsociadasOrdenMetadata))> _
    'Partial Public Class LiqAsociadasOrden
    '    Friend NotInheritable Class LiqAsociadasOrdenMetadata
    '        Private Sub New()
    '            MyBase.New()
    '        End Sub

    '        <Display(Name:="Nro. liquidacion", Description:="Nro. de la liquidacion")> _
    '        Public Property NroLiquidacion As Integer

    '        <Display(Name:="Parcial", Description:="Parcial")> _
    '        Public Property Parcial As Nullable(Of Integer)

    '        <Required(ErrorMessage:="La Fecha de Liquidación probable es requerida")> _
    '        <Display(Name:="Fecha liquidación", Description:="Fecha de la iquidación")> _
    '        Public Property FechaLiquidacion As Nullable(Of DateTime)

    '    End Class
    'End Class

    ''**************************************************************************************************
    '<MetadataTypeAttribute(GetType(ReceptoresOrden.ReceptoresOrdenMetadata))> _
    'Partial Public Class ReceptoresOrden
    '    Friend NotInheritable Class ReceptoresOrdenMetadata
    '        Private Sub New()
    '            MyBase.New()
    '        End Sub

    '        <Display(Name:="Receptor", Description:="Código del receptor que participa en la orden")> _
    '        Public Property IDReceptor As String

    '        <Display(Name:="Líder", Description:="Indica si el recptor es el líder del negocio")> _
    '        Public Property Lider As Boolean

    '        <Display(Name:="Porcentaje", Description:="Porcentaje de participación del receptor en la orden")> _
    '        Public Property Porcentaje As Double

    '    End Class
    'End Class
#End Region

#Region "Re-complementación"


    'The MetadataTypeAttribute identifies tblDistribucionMetadata as the class
    ' that carries additional metadata for the tblDistribucion class.
    <MetadataTypeAttribute(GetType(tblDistribucion.tblDistribucionMetadata))> _
    Partial Public Class tblDistribucion

        'This class allows you to attach custom attributes to properties
        ' of the tblDistribucion class.
        '
        'For example, the following marks the Xyz property as a
        ' required property and specifies the format for valid values:
        '    <Required()>
        '    <RegularExpression("[A-Z][A-Za-z0-9]*")>
        '    <StringLength(32)>
        '    Public Property Xyz As String
        Friend NotInheritable Class tblDistribucionMetadata

            'Metadata classes are not meant to be instantiated.
            Private Sub New()
                MyBase.New()
            End Sub

            Public Property dblCantidad As Decimal

            Public Property dblPorcentajeComision As System.Nullable(Of Decimal)

            Public Property dblPorcentajeDistribucion As Decimal

            Public Property intID As Integer

            Public Property intIdOrden As Nullable(Of Integer)

            Public Property intIDResultado As Integer

            Public Property strIdCliente As String

            Public Property strNroDocumento As String

            Public Property tblResultadoCargaRecomplementacion As tblResultadoCargaRecomplementacion
        End Class
    End Class

    'The MetadataTypeAttribute identifies tblDistribucionClienteMetadata as the class
    ' that carries additional metadata for the tblDistribucionCliente class.
    <MetadataTypeAttribute(GetType(tblDistribucionCliente.tblDistribucionClienteMetadata))> _
    Partial Public Class tblDistribucionCliente

        'This class allows you to attach custom attributes to properties
        ' of the tblDistribucionCliente class.
        '
        'For example, the following marks the Xyz property as a
        ' required property and specifies the format for valid values:
        '    <Required()>
        '    <RegularExpression("[A-Z][A-Za-z0-9]*")>
        '    <StringLength(32)>
        '    Public Property Xyz As String
        Friend NotInheritable Class tblDistribucionClienteMetadata

            'Metadata classes are not meant to be instantiated.
            Private Sub New()
                MyBase.New()
            End Sub

            Public Property intID As Integer

            Public Property intIDResultado As Integer

            Public Property strResultado As String

            Public Property tblResultadoCalculoRecomplementacion As tblResultadoCalculoRecomplementacion
        End Class
    End Class

    'The MetadataTypeAttribute identifies tblDistribucionPrecioMetadata as the class
    ' that carries additional metadata for the tblDistribucionPrecio class.
    <MetadataTypeAttribute(GetType(tblDistribucionPrecio.tblDistribucionPrecioMetadata))> _
    Partial Public Class tblDistribucionPrecio

        'This class allows you to attach custom attributes to properties
        ' of the tblDistribucionPrecio class.
        '
        'For example, the following marks the Xyz property as a
        ' required property and specifies the format for valid values:
        '    <Required()>
        '    <RegularExpression("[A-Z][A-Za-z0-9]*")>
        '    <StringLength(32)>
        '    Public Property Xyz As String
        Friend NotInheritable Class tblDistribucionPrecioMetadata

            'Metadata classes are not meant to be instantiated.
            Private Sub New()
                MyBase.New()
            End Sub

            Public Property intID As Integer

            Public Property intIDResultado As Integer

            Public Property strResultado As String

            Public Property tblResultadoCalculoRecomplementacion As tblResultadoCalculoRecomplementacion
        End Class
    End Class

    'The MetadataTypeAttribute identifies tblResultadoCalculoRecomplementacionMetadata as the class
    ' that carries additional metadata for the tblResultadoCalculoRecomplementacion class.
    <MetadataTypeAttribute(GetType(tblResultadoCalculoRecomplementacion.tblResultadoCalculoRecomplementacionMetadata))> _
    Partial Public Class tblResultadoCalculoRecomplementacion

        'This class allows you to attach custom attributes to properties
        ' of the tblResultadoCalculoRecomplementacion class.
        '
        'For example, the following marks the Xyz property as a
        ' required property and specifies the format for valid values:
        '    <Required()>
        '    <RegularExpression("[A-Z][A-Za-z0-9]*")>
        '    <StringLength(32)>
        '    Public Property Xyz As String
        Friend NotInheritable Class tblResultadoCalculoRecomplementacionMetadata

            'Metadata classes are not meant to be instantiated.
            Private Sub New()
                MyBase.New()
            End Sub

            Public Property intID As Integer

            Public Property tblDistribucionClientes As EntitySet(Of tblDistribucionCliente)

            Public Property tblDistribucionPrecios As EntitySet(Of tblDistribucionPrecio)

            Public Property tblValidacionProcesos As EntitySet(Of tblValidacionProceso)
        End Class
    End Class

    'The MetadataTypeAttribute identifies tblResultadoCargaRecomplementacionMetadata as the class
    ' that carries additional metadata for the tblResultadoCargaRecomplementacion class.
    <MetadataTypeAttribute(GetType(tblResultadoCargaRecomplementacion.tblResultadoCargaRecomplementacionMetadata))> _
    Partial Public Class tblResultadoCargaRecomplementacion

        'This class allows you to attach custom attributes to properties
        ' of the tblResultadoCargaRecomplementacion class.
        '
        'For example, the following marks the Xyz property as a
        ' required property and specifies the format for valid values:
        '    <Required()>
        '    <RegularExpression("[A-Z][A-Za-z0-9]*")>
        '    <StringLength(32)>
        '    Public Property Xyz As String
        Friend NotInheritable Class tblResultadoCargaRecomplementacionMetadata

            'Metadata classes are not meant to be instantiated.
            Private Sub New()
                MyBase.New()
            End Sub

            Public Property intID As Integer

            Public Property tblDistribucions As EntitySet(Of tblDistribucion)

            Public Property tblValidacions As EntitySet(Of tblValidacion)
        End Class
    End Class

    'The MetadataTypeAttribute identifies tblValidacionMetadata as the class
    ' that carries additional metadata for the tblValidacion class.
    <MetadataTypeAttribute(GetType(tblValidacion.tblValidacionMetadata))> _
    Partial Public Class tblValidacion

        'This class allows you to attach custom attributes to properties
        ' of the tblValidacion class.
        '
        'For example, the following marks the Xyz property as a
        ' required property and specifies the format for valid values:
        '    <Required()>
        '    <RegularExpression("[A-Z][A-Za-z0-9]*")>
        '    <StringLength(32)>
        '    Public Property Xyz As String
        Friend NotInheritable Class tblValidacionMetadata

            'Metadata classes are not meant to be instantiated.
            Private Sub New()
                MyBase.New()
            End Sub

            Public Property intColumna As Nullable(Of Integer)

            Public Property intFila As Nullable(Of Integer)

            Public Property intID As Integer

            Public Property intIDResultado As Integer

            Public Property logExitoso As Nullable(Of Boolean)

            Public Property strMensaje As String

            Public Property strTipo As String

            Public Property tblResultadoCargaRecomplementacion As tblResultadoCargaRecomplementacion
        End Class
    End Class

    'The MetadataTypeAttribute identifies tblValidacionProcesoMetadata as the class
    ' that carries additional metadata for the tblValidacionProceso class.
    <MetadataTypeAttribute(GetType(tblValidacionProceso.tblValidacionProcesoMetadata))> _
    Partial Public Class tblValidacionProceso

        'This class allows you to attach custom attributes to properties
        ' of the tblValidacionProceso class.
        '
        'For example, the following marks the Xyz property as a
        ' required property and specifies the format for valid values:
        '    <Required()>
        '    <RegularExpression("[A-Z][A-Za-z0-9]*")>
        '    <StringLength(32)>
        '    Public Property Xyz As String
        Friend NotInheritable Class tblValidacionProcesoMetadata

            'Metadata classes are not meant to be instantiated.
            Private Sub New()
                MyBase.New()
            End Sub

            Public Property intColumna As Nullable(Of Integer)

            Public Property intFila As Nullable(Of Integer)

            Public Property intID As Integer

            Public Property intIDResultado As Integer

            Public Property logExitoso As Nullable(Of Boolean)

            Public Property strMensaje As String

            Public Property strTipo As String

            Public Property tblResultadoCalculoRecomplementacion As tblResultadoCalculoRecomplementacion
        End Class
    End Class


#End Region

End Namespace
