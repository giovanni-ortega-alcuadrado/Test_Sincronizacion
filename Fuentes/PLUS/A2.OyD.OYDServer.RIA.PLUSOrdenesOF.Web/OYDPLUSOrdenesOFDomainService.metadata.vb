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
    <MetadataTypeAttribute(GetType(OrdenOYDPLUSOF.OrdenOYDPLUSOFMetadata))> _
    Partial Public Class OrdenOYDPLUSOF
        Friend NotInheritable Class OrdenOYDPLUSOFMetadata
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

            <Display(Name:="Valor futuro mercado")> _
            Public Property ValorFuturoRepo As System.Nullable(Of Double)

            <Display(Name:="Valor futuro cliente")> _
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

#End Region

End Namespace
