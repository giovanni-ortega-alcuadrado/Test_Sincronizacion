'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated from a template.
'
'     Manual changes to this file may cause unexpected behavior in your application.
'     Manual changes to this file will be overwritten if the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Collections.Generic

Partial Public Class tblOrdenesDV
    Public Property intIdOrden As Integer
    Public Property intIdordenEnc As Integer
    Public Property intNroOrden As Integer
    Public Property intIdTipoOperacion As Integer
    Public Property bitCompra As Boolean
    Public Property intIdCuentaCRCC As Integer
    Public Property intIdSubcuentaCRCC As Nullable(Of Integer)
    Public Property intIdProductoVencimiento As Integer
    Public Property intIdProductoEspecial As Nullable(Of Integer)
    Public Property intIdTipoOpcionProducto As Nullable(Of Short)
    Public Property intIdComercial As Integer
    Public Property bitRegistradaEnBolsa As Boolean
    Public Property intIdTipoRegistro As Short
    Public Property intIdContraparte As Nullable(Of Integer)
    Public Property numCantidad As Decimal
    Public Property numPrecio As Decimal
    Public Property numPrima As Nullable(Of Decimal)
    Public Property numComision As Decimal
    Public Property bitComisionPorPorcentaje As Boolean
    Public Property numPorcentajeComision As Nullable(Of Decimal)
    Public Property bitFacturaComisionInicio As Boolean
    Public Property numCantidadPendiente As Decimal
    Public Property numSaldoPendiente As Decimal
    Public Property bitEsGiveOut As Boolean
    Public Property intIdFirmaGiveOut As Nullable(Of Integer)
    Public Property strReferenciaGiveOut As String
    Public Property intDiasAvisoCumplimiento As Byte
    Public Property intIdTipoOrdenDuracion As Short
    Public Property intIdTipoOrdenEjecucion As Short
    Public Property intIdTipoOrdenNaturaleza As Short
    Public Property numPrecioStop As Nullable(Of Decimal)
    Public Property numCantidadVisible As Nullable(Of Decimal)
    Public Property numCantidadMinima As Nullable(Of Decimal)
    Public Property intIdInstrumentoSeguimiento As Nullable(Of Integer)
    Public Property intIdComponentePrecio As Nullable(Of Integer)
    Public Property intIdSesion As Nullable(Of Integer)
    Public Property strObservaciones As String
    Public Property strUsuarioCreacion As String
    Public Property dtmActualizacion As Date
    Public Property strUsuario As String
    Public Property strInstruccionesPago As String
    Public Property bitPermiteConsultar As Boolean
    Public Property numCantidadCancelada As Nullable(Of Decimal)
    Public Property intIdComercialToma As Nullable(Of Integer)
    Public Property dtmFechaHoraToma As Nullable(Of Date)
    Public Property intIdCanal As Nullable(Of Short)
    Public Property intIdMedioVerificable As Nullable(Of Integer)
    Public Property strDetalleMV As String
    Public Property numPrecioSpot As Nullable(Of Decimal)
    Public Property intIdTipoPrima As Nullable(Of Integer)
    Public Property numPrecioPrima As Nullable(Of Decimal)
    Public Property strReceptorCliente As String
    Public Property intAplicarComision As Nullable(Of Integer)
    Public Property strPrimaPor As String

End Class
