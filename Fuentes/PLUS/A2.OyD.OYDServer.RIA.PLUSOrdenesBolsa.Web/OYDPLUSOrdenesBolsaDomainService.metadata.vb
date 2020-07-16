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

Namespace OyDPLUSOrdenesBolsa

#Region "OYDPLUS"

    '**************************************************************************************************
    <MetadataTypeAttribute(GetType(OrdenOYDPLUS.OrdenOYDPLUSMetadata))> _
    Partial Public Class OrdenOYDPLUS
        Friend NotInheritable Class OrdenOYDPLUSMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID nro orden: ")> _
            Public Property IDNroOrden As Integer

            <Display(Name:="Nro orden: ")> _
            Public Property NroOrden As Integer

            <Display(Name:="Versión: ")> _
            Public Property Version As Integer

            <Display(Name:="Fecha ingreso: ")> _
            Public Property FechaOrden As DateTime

            <Display(Name:="Fecha vigencia")> _
            Public Property FechaVigencia As System.Nullable(Of DateTime)

            <Display(Name:="Hora vigencia")> _
            Public Property HoraVigencia As String

            <Display(Name:="Clase orden")> _
            Public Property Clase As String

            <Display(Name:="Tipo operación")> _
            Public Property TipoOperacion As String

            <Display(Name:="Estado")> _
            Public Property EstadoOrden As String

            <Display(Name:="Estado: ")> _
            Public Property NombreEstadoOrden As String

            <Display(Name:="Tipo negocio")> _
            Public Property TipoNegocio As String

            <Display(Name:="Tipo producto")> _
            Public Property TipoProducto As String

            <Display(Name:="Tipo orden")> _
            Public Property TipoOrden As String

            <Display(Name:="Receptor")> _
            Public Property Receptor As String

            <Display(Name:="Usuario: ")> _
            Public Property Usuario As String

            <Display(Name:="Plantilla")> _
            Public Property Plantilla As String

            <Display(Name:="Bolsa: ")> _
            Public Property Bolsa As String

            <Display(Name:="Bolsa: ")> _
            Public Property NombreBolsa As String

            <Display(Name:="Días")> _
            Public Property Dias As System.Nullable(Of Integer)

            <Display(Name:="Clasificación")> _
            Public Property Clasificacion As String

            <Display(Name:="Tipo límite")> _
            Public Property TipoLimite As String

            <Display(Name:="Duración")> _
            Public Property Duracion As String

            <Display(Name:="Condiciones negociación")> _
            Public Property CondicionesNegociacion As String

            <Display(Name:="Forma pago")> _
            Public Property FormaPago As String

            <Display(Name:="Tipo inversión")> _
            Public Property TipoInversion As String

            <Display(Name:="Ejecución")> _
            Public Property Ejecucion As String

            <Display(Name:="Mercado")> _
            Public Property Mercado As String

            <Display(Name:="Código OyD")> _
            Public Property IDComitente As String

            <Display(Name:="Ordenante")> _
            Public Property IDOrdenante As String

            <Display(Name:="Nombre cliente")> _
            Public Property NombreCliente As String

            <Display(Name:="Tipo")> _
            Public Property CategoriaCliente As String

            <Display(Name:="Identificación")> _
            Public Property NroDocumento As String

            <Display(Name:="Nombre ordenante")> _
            Public Property NombreOrdenante As String

            <Display(Name:="Cuenta depósito")> _
            Public Property CuentaDeposito As System.Nullable(Of Integer)

            <Display(Name:="Ubicación título")> _
            Public Property UBICACIONTITULO As String

            <Display(Name:="Cuenta depósito")> _
            Public Property DescripcionCta As String

            <Display(Name:="Estado LEO: ")> _
            Public Property EstadoLEO As String

            <Display(Name:="Estado LEO: ")> _
            Public Property NombreEstadoLEO As String

            <Display(Name:="Usuario operador")> _
            Public Property UsuarioOperador As String

            <Display(Name:="Canal recepción")> _
            Public Property CanalRecepcion As String

            <Display(Name:="Medio verificable")> _
            Public Property MedioVerificable As String

            <Display(Name:="Fecha recepción")> _
            Public Property FechaRecepcion As System.Nullable(Of DateTime)

            <Display(Name:="Extensión teléfono")> _
            Public Property NroExtensionToma As String

            <Display(Name:="Estado SAE")> _
            Public Property EstadoSAE As String

            <Display(Name:="Estado SAE")> _
            Public Property NombreEstadoSAE As String

            <Display(Name:="Nro orden SAE")> _
            Public Property NroOrdenSAE As System.Nullable(Of Integer)

            <Display(Name:="Especie")> _
            Public Property Especie As String

            <Display(Name:="ISIN")> _
            Public Property ISIN As String

            <Display(Name:="Fecha emisión")> _
            Public Property FechaEmision As System.Nullable(Of DateTime)

            <Display(Name:="Fecha vencimiento")> _
            Public Property FechaVencimiento As System.Nullable(Of DateTime)

            <Display(Name:="Fecha cumplimiento")> _
            Public Property FechaCumplimiento As System.Nullable(Of DateTime)

            <Display(Name:="Tasa facial")> _
            Public Property TasaFacial As System.Nullable(Of Double)

            <Display(Name:="Modalidad")> _
            Public Property Modalidad As String

            <Display(Name:="Indicador")> _
            Public Property Indicador As System.Nullable(Of Integer)

            <Display(Name:="Puntos indicador")> _
            Public Property PuntosIndicador As System.Nullable(Of Double)

            <Display(Name:="Estandarizada")> _
            Public Property Estandarizada As System.Nullable(Of Boolean)

            <Display(Name:="En pesos")> _
            Public Property EnPesos As System.Nullable(Of Boolean)

            <Display(Name:="Cantidad")> _
            Public Property Cantidad As System.Nullable(Of Double)

            <Display(Name:="Precio")> _
            Public Property Precio As System.Nullable(Of Double)

            <Display(Name:="Precio")> _
            Public Property PrecioMaximoMinimo As System.Nullable(Of Double)

            <Display(Name:="Valor captación giro")> _
            Public Property ValorCaptacionGiro As System.Nullable(Of Double)

            <Display(Name:="Valor futuro")> _
            Public Property ValorFuturoRepo As System.Nullable(Of Double)

            <Display(Name:="Valor futuro")> _
            Public Property ValorFuturoCliente As System.Nullable(Of Double)

            <Display(Name:="Valor acción")> _
            Public Property ValorAccion As System.Nullable(Of Double)

            <Display(Name:="Tasa mercado")> _
            Public Property TasaRegistro As System.Nullable(Of Double)

            <Display(Name:="Tasa cliente")> _
            Public Property TasaCliente As System.Nullable(Of Double)

            <Display(Name:="Tasa nominal")> _
            Public Property TasaNominal As System.Nullable(Of Double)

            <Display(Name:="% Comisión")> _
            Public Property Comision As System.Nullable(Of Double)

            <Display(Name:="Valor comisión")> _
            Public Property ValorComision As System.Nullable(Of Double)

            <Display(Name:="Iva comisión")> _
            Public Property IvaComision As System.Nullable(Of Double)

            <Display(Name:="% Castigo")> _
            Public Property Castigo As System.Nullable(Of Double)

            <Display(Name:="Valor")> _
            Public Property ValorOrden As System.Nullable(Of Double)

            <Display(Name:="Días repo")> _
            Public Property DiasRepo As Integer

            <Display(Name:="Producto valores")> _
            Public Property ProductoValores As System.Nullable(Of Integer)

            <Display(Name:="Costos adicionales orden")> _
            Public Property CostosAdicionales As Double

            <Display(Name:="Instrucciones")> _
            Public Property Instrucciones As String

            <Display(Name:="Notas")> _
            Public Property Notas As String

            <Display(Name:="Orden cruzada")> _
            Public Property OrdenCruzada As Boolean

            <Display(Name:="Orden cruzada cliente")> _
            Public Property OrdenCruzadaCliente As Boolean

            <Display(Name:="Orden cruzada receptor")> _
            Public Property OrdenCruzadaReceptor As Boolean

            <Display(Name:="Orden original")> _
            Public Property IDOrdenOriginal As System.Nullable(Of Integer)

            <Display(Name:="Orden original")> _
            Public Property IDNroOrdenOriginal As System.Nullable(Of Integer)

            <Display(Name:="Modificable")> _
            Public Property Modificable As Boolean

        End Class
    End Class

    '**************************************************************************************************
    <MetadataTypeAttribute(GetType(tblConfiguracionesAdicionalesReceptor.tblConfiguracionesAdicionalesReceptorMetadata))> _
    Partial Public Class tblConfiguracionesAdicionalesReceptor
        Friend NotInheritable Class tblConfiguracionesAdicionalesReceptorMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")> _
            Public Property ID As Integer

            <Display(Name:="Receptor", Description:="Receptor")> _
            Public Property CodigoReceptor As String

            <Display(Name:="Tipo Orden", Description:="Tipo Orden")> _
            Public Property TipoOrden As String

            <Display(Name:="Extensión por Defecto", Description:="Extensión por Defecto")> _
            Public Property ExtensionDefecto As String

            <Display(Name:="Tipo Orden Defecto", Description:="Tipo Orden Defecto")> _
            Public Property TipoOrdenDefecto As String

        End Class
    End Class

    '**************************************************************************************************
    <MetadataTypeAttribute(GetType(tblParametrosReceptor.tblParametrosReceptorMetadata))> _
    Partial Public Class tblParametrosReceptor
        Friend NotInheritable Class tblParametrosReceptorMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")> _
            Public Property ID As Integer

            <Display(Name:="Receptor", Description:="Receptor")> _
            Public Property CodigoReceptor As String

            <Display(Name:="Topico", Description:="Topico")> _
            Public Property Topico As String

            <Display(Name:="Valor", Description:="Valor")> _
            Public Property Valor As String

            <Display(Name:="Descripción", Description:="Descripción")> _
            Public Property Descripcion As String

            <Display(Name:="Prioridad", Description:="Prioridad")> _
            Public Property Prioridad As Integer

        End Class
    End Class

    '**************************************************************************************************
    <MetadataTypeAttribute(GetType(tblMensajes.tblMensajesMetadata))> _
    Partial Public Class tblMensajes
        Friend NotInheritable Class tblMensajesMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")> _
            Public Property ID As Integer

            <Display(Name:="Titulo", Description:="Titulo")> _
            Public Property Titulo As String

            <Display(Name:="Valor", Description:="Valor")> _
            Public Property Valor As String

            <Display(Name:="Color", Description:="Color")> _
            Public Property Color As String

            <Display(Name:="Descripción", Description:="Descripción")> _
            Public Property Descripcion As String

        End Class
    End Class

    '**************************************************************************************************
    <MetadataTypeAttribute(GetType(tblReceptoresUsuario.tblReceptoresUsuarioMetadata))> _
    Partial Public Class tblReceptoresUsuario
        Friend NotInheritable Class tblReceptoresUsuarioMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")> _
            Public Property ID As Integer

            <Display(Name:="Codigo", Description:="Codigo")> _
            Public Property CodigoReceptor As String

            <Display(Name:="Nombre", Description:="Nombre")> _
            Public Property Nombre As String

            <Display(Name:="Prioridad", Description:="Prioridad")> _
            Public Property Prioridad As Integer

            <Display(Name:="Usuario", Description:="Usuario")> _
            Public Property Usuario As String

        End Class
    End Class

    '**************************************************************************************************
    <MetadataTypeAttribute(GetType(CombosReceptor.CombosReceptorMetadata))> _
    Partial Public Class CombosReceptor
        Friend NotInheritable Class CombosReceptorMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")> _
            Public Property ID As Integer

            <Display(Name:="Categoria", Description:="Categoria")> _
            Public Property Categoria As String

            <Display(Name:="Retorno", Description:="Retorno")> _
            Public Property Retorno As String

            <Display(Name:="Descripción", Description:="Descripción")> _
            Public Property Descripcion As String

        End Class
    End Class

    '**************************************************************************************************
    <MetadataTypeAttribute(GetType(tblEspeciesXTipoNegocio.tblEspeciesXTipoNegocioMetadata))> _
    Partial Public Class tblEspeciesXTipoNegocio
        Friend NotInheritable Class tblEspeciesXTipoNegocioMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")> _
            Public Property ID As Integer

            <Display(Name:="Tipo Negocio", Description:="Tipo Negocio")> _
            Public Property TipoNegocio As String

            <Display(Name:="Especie", Description:="Especie")> _
            Public Property IDEspecie As String

            <Display(Name:="Maneja ISIN", Description:="Maneja ISIN")> _
            Public Property ManejaISIN As Boolean

            <Display(Name:="Maxima cantidad negocio", Description:="Maxima cantidad negocio")> _
            Public Property MaxCantidad As Double

        End Class
    End Class

    '**************************************************************************************************
    <MetadataTypeAttribute(GetType(tblRespuestaValidaciones.tblRespuestaValidacionesMetadata))> _
    Partial Public Class tblRespuestaValidaciones
        Friend NotInheritable Class tblRespuestaValidacionesMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")> _
            Public Property ID As Integer

            <Display(Name:="Exitoso", Description:="Exitoso")> _
            Public Property Exitoso As Boolean

            <Display(Name:="Requiere Confirmación", Description:="Requiere Confirmación")> _
            Public Property RequiereConfirmacion As Double

            <Display(Name:="Mensaje", Description:="Mensaje")> _
            Public Property Mensaje As Integer

            <Display(Name:="Confirmación", Description:="Confirmación")> _
            Public Property Confirmacion As Double

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
    <MetadataTypeAttribute(GetType(tblReceptoresOrdenesPorCruzar.tblReceptoresOrdenesPorCruzarMetadata))> _
    Partial Public Class tblReceptoresOrdenesPorCruzar
        Friend NotInheritable Class tblReceptoresOrdenesPorCruzarMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")> _
            Public Property ID As Integer

            <Required(ErrorMessage:="El id de la orden es un campo requerido")> _
            <Display(Name:="Orden", Description:="Orden")> _
            Public Property IDOrdenOriginal As Integer

            <Required(ErrorMessage:="El receptor es un campo requerido")> _
            <Display(Name:="Receptor", Description:="Receptor")> _
            Public Property IDReceptor As String

        End Class
    End Class

    '**************************************************************************************************
    <MetadataTypeAttribute(GetType(tblOrdenesCruzadas.tblOrdenesCruzadasMetadata))> _
    Partial Public Class tblOrdenesCruzadas
        Friend NotInheritable Class tblOrdenesCruzadasMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")> _
            Public Property ID As Integer

            <Required(ErrorMessage:="El id de la orden es un campo requerido")> _
            <Display(Name:="Orden", Description:="Orden")> _
            Public Property IDOrdenOriginal As Integer

            <Required(ErrorMessage:="El id de la orden es un campo requerido")> _
            <Display(Name:="Orden", Description:="Orden")> _
            Public Property IDOrdenCruzada As Integer

        End Class
    End Class

#Region "Orden Seteador"
    <MetadataTypeAttribute(GetType(OrdenSeteador.OrdenSeteadorMetadata))> _
    Partial Public Class OrdenSeteador
        Friend NotInheritable Class OrdenSeteadorMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="IdOrdenes", Description:="Id")> _
            Public Property intIDOrdenes As Integer

            <Display(Name:="Id", Description:="IdOrden")> _
            Public Property lngID As Integer

            <Display(Name:="Versión", Description:="Versión")> _
            Public Property lngVersion As Integer

            <Display(Name:="Id tipo negocio", Description:="Id tipo de negocio")> _
            Public Property strTipoNegocio As String

            <Display(Name:="Tipo negocio", Description:="Tipo de negocio")> _
            Public Property TipoNegocio As String

            <Display(Name:="Opciones menú", Description:="Opciones para menú contextual")> _
            Public Property strConfiguracionMenu As String

            <Display(Name:="Id tipo operación", Description:="Id tipo de operación")> _
            Public Property strTipo As Char

            <Display(Name:="Tipo operación", Description:="Tipo de operación")> _
            Public Property TipoOperacion As String

            <Display(Name:="Id mercado", Description:="Id Mercado")> _
            Public Property strMercado As String

            <Display(Name:="Mercado", Description:="Mercado")> _
            Public Property Mercado As String

            <Display(Name:="Id tipo límite", Description:="Id del tipo de límite")> _
            Public Property strTipoLimite As Char

            <Display(Name:="Tipo límite", Description:="Tipo de límite")> _
            Public Property TipoLimite As String

            <Display(Name:="Especie", Description:="Especie")> _
            Public Property strIDEspecie As String

            <Display(Name:="Fecha orden", Description:="Fecha de ingreso de la orden")> _
            Public Property dtmOrden As String

            <Display(Name:="Fecha vigencia", Description:="Fecha de vigencia de la orden")> _
            Public Property dtmVigenciaHasta As String

            <Display(Name:="Plazo", Description:="Plazo")> _
            Public Property Plazo As System.Nullable(Of Integer)

            <Display(Name:="Cantidad", Description:="Cantidad")> _
            Public Property dblCantidad As Double

            <Display(Name:="Tasa inicial", Description:="Tasa inicial")> _
            Public Property dblTasaInicial As System.Nullable(Of Double)

            <Display(Name:="Tasa compra venta", Description:="Tasa de compra o venta")> _
            Public Property dblTasaCompraVenta As System.Nullable(Of Double)

            <Display(Name:="Precio orden", Description:="Precio de la Orden")> _
            Public Property dblPrecioOrden As System.Nullable(Of Double)

            <Display(Name:="Cliente", Description:="Cliente")> _
            Public Property strNombreCliente As String

            <Display(Name:="Receptor", Description:="Receptor")> _
            Public Property strIdReceptorToma As String

            <Display(Name:="Id estado", Description:="Id Estado")> _
            Public Property strEstado As Char

            <Display(Name:="Estado", Description:="Estado")> _
            Public Property Estado As String

            <Display(Name:="Ruta visor", Description:="Ruta de la página web con los datos para el visor")> _
            Public Property RutaVisor As String

            <Display(Name:="Estado M & C", Description:="Estado M & C")> _
            Public Property strEstadoMakerAndChecker As String

            <Display(Name:="Acción M & C", Description:="Acción M & C")> _
            Public Property strAccionMakerAndCheker As String

            <Display(Name:="Descripción M & C", Description:="Descripción M & C")> _
            Public Property strDescMakerAndCheker As String

            <Display(Name:="Usuario ingreso", Description:="Usuario que ingresó la orden")> _
            Public Property strUsuarioIngreso As String
        End Class

    End Class

    <MetadataTypeAttribute(GetType(Liquidacion.LiquidacionMetadata))> _
    Partial Public Class Liquidacion
        Friend NotInheritable Class LiquidacionMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="Nro orden", Description:="Nro orden")> _
            Public Property lngIdOrden As Integer

            <Display(Name:="Nro folio", Description:="Nro folio")> _
            Public Property intNroFolio As Integer

            <Display(Name:="Cantidad", Description:="Cantidad")> _
            Public Property numCantidad As Double

            <Display(Name:="Fecha cumplimiento", Description:="Fecha cumplimiento")> _
            Public Property dtmFechaCumplimiento As System.Nullable(Of Date)

            <Display(Name:="Monto", Description:="Monto")> _
            Public Property numMonto As Nullable(Of Decimal)

            <Display(Name:="Precio", Description:="Precio")> _
            Public Property numPrecio As Nullable(Of Decimal)

            <Display(Name:="Días cumplimiento", Description:="Días cumplimiento")> _
            Public Property intDiasDeCumplimiento As Integer

            <Display(Name:="Tasa", Description:="Tasa")> _
            Public Property numTasa As Nullable(Of Decimal)

            <Display(Name:="Nemotécnico", Description:="Nemotécnico")> _
            Public Property strNemotecnico As String

            <Display(Name:="Fecha emisión", Description:="Fecha emisión")> _
            Public Property dtmFechaEmision As System.Nullable(Of Date)

            <Display(Name:="Codigo ISIN", Description:="Codigo ISIN")> _
            Public Property strCodigoISIN As String

            <Display(Name:="Trader", Description:="Trader")> _
            Public Property strTrader As String

            <Display(Name:="Rueda", Description:="Rueda")> _
            Public Property strRuedaNegocio As String

            <Display(Name:="Modalidad", Description:="Modalidad")> _
            Public Property strModalidad As String

            <Display(Name:="Fecha vencimiento", Description:="Fecha vencimiento")> _
            Public Property dtmFechaVencimiento As System.Nullable(Of Date)

            <Display(Name:="Mercado", Description:="Mercado")> _
            Public Property strMercado As String

            <Display(Name:="Tipo mercado", Description:="Tipo mercado")> _
            Public Property strTipoMercado As String

        End Class
    End Class

#End Region

#End Region

#Region "RegistroOperacionesPorReceptor"

    <MetadataTypeAttribute(GetType(RegistroOperacionesPorReceptor.RegistroOperacionesPorReceptorMetaData))> _
    Partial Public Class RegistroOperacionesPorReceptor
        Friend NotInheritable Class RegistroOperacionesPorReceptorMetaData
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="Id operación", Description:="Id operación")> _
            Public Property IdOperacion As Integer

            <Display(Name:="Receptor A", Description:="Receptor A")> _
            Public Property ReceptorA As String

            <Display(Name:="Tipo producto", Description:="Tipo producto")> _
            Public Property TipoProducto As String

            <Display(Name:="Tipo negocio", Description:="Tipo negocio")> _
            Public Property Clase As String

            <Display(Name:="Tipo operación", Description:="Tipo operación")> _
            Public Property TipoOperacion As String

            <Display(Name:="Receptor B", Description:="Receptor B")> _
            Public Property ReceptorB As String

            <Display(Name:="Cliente", Description:="Cliente")> _
            Public Property IdCliente As String

            <Display(Name:="Especie", Description:="Especie")> _
            Public Property IdEspecie As String

            <Display(Name:="Isin", Description:="Isin")> _
            Public Property Isin As String

            <Display(Name:="Emisión", Description:="Emisión")> _
            Public Property Emision As System.Nullable(Of Date)

            <Display(Name:="Vencimiento", Description:="Vencimiento")> _
            Public Property Vencimiento As System.Nullable(Of Date)

            <Display(Name:="Ordenante", Description:="Ordenante")> _
            Public Property IdOrdenante As String

            <Display(Name:="Cuenta Deposito", Description:="Cuenta Deposito")> _
            Public Property CuentaDeposito As System.Nullable(Of System.Int32)

            <Display(Name:="Ubicación Titulo", Description:="strUbicación Titulo")> _
            Public Property UbicacionTitulo As System.String

            <Display(Name:="Tasa facial", Description:="Tasa facial")> _
            Public Property TasaFacial As System.Nullable(Of System.Double)

            <Display(Name:="Modalidad", Description:="Modalidad")> _
            Public Property Modalidad As String

            <Display(Name:="Indicador", Description:="Indicador")> _
            Public Property Indicador As String

            <Display(Name:="Puntos indicador", Description:="Puntos indicador")> _
            Public Property PuntosIndicador As System.Nullable(Of System.Double)

            <Display(Name:="Cantidad", Description:="Cantidad")> _
            Public Property Cantidad As System.Nullable(Of System.Double)

            <Display(Name:="Valor nominal", Description:="Valor nominal")> _
            Public Property ValorNominal As System.Nullable(Of System.Double)

            <Display(Name:="Valor giro", Description:="Valor giro")> _
            Public Property ValorGiro As System.Nullable(Of System.Double)

            <Display(Name:="Valor regreso", Description:="Valor regreso")> _
            Public Property ValorGiro2 As System.Nullable(Of System.Double)

            <Display(Name:="Valor giro contraparte", Description:="Valor giro contraparte")> _
            Public Property ValorGiroContraparte As System.Nullable(Of System.Double)

            <Display(Name:="Comisión venta", Description:="Comisión venta")> _
            Public Property ComisionVenta As System.Nullable(Of System.Double)

            <Display(Name:="Comisión compra", Description:="Comisión compra")> _
            Public Property ComisionCompra As System.Nullable(Of System.Double)

            <Display(Name:="Tasa", Description:="Tasa")> _
            Public Property Tasa As System.Nullable(Of System.Double)

            <Display(Name:="Acum. cupón", Description:="Acum. cupón")> _
            Public Property AcumCupon As System.Nullable(Of System.Double)

            <Display(Name:="Precio", Description:="Precio")> _
            Public Property PrecioSucio As System.Nullable(Of System.Double)

            <StringLength(500, ErrorMessage:="La longitud máxima es de las Observaciones es de: 500")> _
            <Display(Name:="Observaciones", Description:="Observaciones")> _
            Public Property Observaciones As System.String

            <Display(Name:="Fecha ingreso", Description:="Fecha ingreso")> _
            Public Property FechaIngreso As System.Nullable(Of Date)

            <Display(Name:="Fecha cumplimiento", Description:="Fecha cumplimiento")> _
            Public Property FechaCumplimiento As System.Nullable(Of Date)

            <Display(Name:="Fecha liquidación", Description:="Fecha liquidación")> _
            Public Property FechaLiquidacion As System.Nullable(Of Date)

            <Display(Name:="Fecha regreso", Description:="Fecha regreso")> _
            Public Property FechaRegreso As System.Nullable(Of Date)

            <Display(Name:="Plazo", Description:="Plazo")> _
            Public Property Plazo As System.Nullable(Of Integer)

            <Display(Name:="Operación de regreso", Description:="Operación de regreso")> _
            Public Property OperacionRegreso As Boolean

        End Class
    End Class

#End Region

End Namespace
