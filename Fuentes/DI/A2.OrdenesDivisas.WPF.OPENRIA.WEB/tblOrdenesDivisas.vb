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

Partial Public Class tblOrdenesDivisas
    Public Property intID As Integer
    Public Property intIDOrden As Integer
    Public Property intIDMoneda As Integer
    Public Property strTipoReferencia As String
    Public Property strCompensacion As String
    Public Property intIDFormulario As Nullable(Of Integer)
    Public Property intIDNumeral As Nullable(Of Integer)
    Public Property strMesa As String
    Public Property strObservaciones As String
    Public Property dblCantidad As Double
    Public Property dblPrecio As Double
    Public Property intIDMonedaIntermedia As Integer
    Public Property dblPrecioIntermedio As Double
    Public Property dblSpreadComision As Double
    Public Property dblValorBruto As Double
    Public Property dblValorNeto As Double
    Public Property dblValorTRM As Double
    Public Property dblPrecioMonedaNegociada As Double
    Public Property dblValorRteFuente As Nullable(Of Double)
    Public Property dblValorGMF As Nullable(Of Double)
    Public Property logProcesoContableNexDay As Nullable(Of Boolean)
    Public Property strFolio As String
    Public Property strSistemaOperacionNegociada As String
    Public Property intIdPais As Nullable(Of Integer)
    Public Property intIdciudad As Nullable(Of Integer)
    Public Property IntCODSWIFT As Nullable(Of Integer)
    Public Property dblValorGiroUIAF As Nullable(Of Double)
    Public Property dblBaseIVA As Nullable(Of Double)
    Public Property dblValorIVA As Nullable(Of Double)
    Public Property strFolioSETFX As String
    Public Property strUsuario As String
    Public Property dtmActualizacion As Date
    Public Property intDestinoInversion As Nullable(Of Double)
    Public Property strInstrucciones As String
    Public Property logEnviado As Nullable(Of Boolean)
    Public Property strSistemaOrigen As String
    Public Property dblPrecioParcial As Nullable(Of Double)
    Public Property dblDevaluacion As Nullable(Of Double)
    Public Property dblValorTasaForward As Nullable(Of Double)
    Public Property strTipoCumplimiento As String
    Public Property dblSpot As Nullable(Of Double)

    Public Overridable Property tblOrdenes As tblOrdenes

End Class
