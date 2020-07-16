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

Namespace OyDPLUSOrdenesDivisas

#Region "Orden Divisas"

    '**************************************************************************************************
    <MetadataTypeAttribute(GetType(OrdenDivisas.OrdenDivisasMetadata))> _
    Partial Public Class OrdenDivisas
        Friend NotInheritable Class OrdenDivisasMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID nro orden: ")> _
            Public Property ID As Integer

            <Display(Name:="Nro orden: ")> _
            Public Property NroOrden As Integer

            <Display(Name:="Fecha ingreso: ")> _
            Public Property FechaOrden As DateTime

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

            <Display(Name:="Receptor")> _
            Public Property Receptor As String

            <Display(Name:="Usuario: ")> _
            Public Property Usuario As String

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

            <Display(Name:="Fecha cumplimiento")> _
            Public Property Cumplimiento As System.Nullable(Of Integer)

            <Display(Name:="Concepto giro")> _
            Public Property ConceptoGiro As String

            <Display(Name:="Moneda")> _
            Public Property Moneda As String

            <Display(Name:="Monto")> _
            Public Property Monto As Double

            <Display(Name:="Tasa de Cesión mesa")> _
            Public Property TasaDeCesionMesa As Double

            <Display(Name:="Tasa Cliente")> _
            Public Property TasaCliente As Double

            <Display(Name:="Comisión comercial VIA Spread")> _
            Public Property ComisionComercialVIASpread As Double
           
            <Display(Name:="Comisión comercial VIA Papeleta")> _
            Public Property ComisionComercialVIAPapeleta As Double

            <Display(Name:="Tasa Bruta")> _
            Public Property TasaBruta As Double

            <Display(Name:="Cantidad Bruta")> _
            Public Property CantidadBruta As Double


            <Display(Name:="Cantidad Neta")> _
            Public Property CantidadNeta As Double

            <Display(Name:="Tasa Dolar")> _
            Public Property TasaDolar As Double

            <Display(Name:="Cantidad USD")> _
            Public Property CantidadUSD As Double

            <Display(Name:="Iva Otros Costos")> _
            Public Property IvaOtrosCostos As Double
        End Class
    End Class

#End Region

End Namespace
