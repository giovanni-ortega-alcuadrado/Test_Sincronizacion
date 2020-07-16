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


Namespace OyDOTC

    <MetadataTypeAttribute(GetType(Liquidaciones_OT.Liquidaciones_OTMetadata))> _
    Partial Public Class Liquidaciones_OT
        Friend NotInheritable Class Liquidaciones_OTMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="Nro. Operación", Description:="Numero consecutivo de la Operación")> _
            Public Property ID As Nullable(Of Integer)

            <Display(Name:="Versión", Description:="Versión")> _
            Public Property VERSION As Nullable(Of Integer)

            <Required(ErrorMessage:="Este campo es requerido. (Sistema)")> _
              <Display(Name:="Sistema", Description:="Sistema")> _
            Public Property NOMBRESISTEMA As String

            <Required(ErrorMessage:="Este campo es requerido. (Operación)")> _
              <Display(Name:="Operación", Description:="Operación")> _
            Public Property NUMEROOPERACION As Nullable(Of Integer)

            <Required(ErrorMessage:="Este campo es requerido. (Fecha Operación)")> _
              <Display(Name:="Fecha Operación", Description:="Fecha Operación")> _
            Public Property OPERACION As DateTime

            <Required(ErrorMessage:="Este campo es requerido. (Tipo Operación)")> _
              <Display(Name:="Tipo Operación", Description:="Tipo de Operación")> _
            Public Property TIPOOPERACION As String

            <Required(ErrorMessage:="Este campo es requerido. (Mercado)")> _
              <Display(Name:="Mercado", Description:="Mercado")> _
            Public Property Mercado As String

            <Required(ErrorMessage:="Este campo es requerido. (Clase Operación)")> _
              <Display(Name:="Clase Operación", Description:="Clase Operación")> _
            Public Property TIPONEGOCIACION As String

            <Required(ErrorMessage:="Este campo es requerido. (Tipo Registro)")> _
              <Display(Name:="Tipo Registro", Description:="Tipo Registro Operación")> _
            Public Property REGISTROOPERACION As String

            <Required(ErrorMessage:="Este campo es requerido. (Pago Operación)")> _
              <Display(Name:="Pago Operación", Description:="Tipo de pago de la Operación")> _
            Public Property TIPOPAGOOPERACION As String

            <Required(ErrorMessage:="Este campo es requerido. (Especie)")> _
              <Display(Name:="Especie", Description:="Especie")> _
            Public Property IDESPECIE As String

            <Required(ErrorMessage:="Este campo es requerido. (Cantidad)")> _
              <Display(Name:="Cantidad", Description:="Cantidad Negociada")> _
            Public Property CANTIDADNEGOCIADA As Double

            <Required(ErrorMessage:="Este campo es requerido. (Fecha Emisión)")> _
              <Display(Name:="Fecha Emisión", Description:="Fecha de Emisión")> _
            Public Property EMISION As Nullable(Of DateTime)

            <Required(ErrorMessage:="Este campo es requerido. (Fecha Cumplimiento)")> _
              <Display(Name:="Fecha Cumplimiento", Description:="Fecha de Cumplimiento")> _
            Public Property CUMPLIMIENTO As DateTime

            <Required(ErrorMessage:="Este campo es requerido. (Fecha Vencimiento)")> _
              <Display(Name:="Fecha Vencimiento", Description:="Fecha de Vencimiento")> _
            Public Property VENCIMIENTO As Nullable(Of DateTime)

            <Required(ErrorMessage:="Este campo es requerido. (Dias Vencieminto)")> _
              <Display(Name:="Dias Vencieminto", Description:="Dias Vencimiento")> _
            Public Property DIASALVENCIMIENTOTITULO As Nullable(Of Integer)

            <Display(Name:="Nominal", Description:="Tasa de Interes Nomimal")> _
            Public Property TASAINTERESNOMINAL As Double

            <Required(ErrorMessage:="Este campo es requerido. (Modalidad)")> _
              <Display(Name:="Modalidad", Description:="Modalida Tasa Nominal")> _
            Public Property MODALIDADTASANOMINAL As String

            <Display(Name:="Efectiva", Description:="Tasa Efectiva Anual")> _
            Public Property TASAEFECTIVAANUAL As Nullable(Of Double)

            <Required(ErrorMessage:="Este campo es requerido. (Precio)")> _
              <Display(Name:="Precio", Description:="Precio")> _
            Public Property PRECIO As Double

            <Required(ErrorMessage:="Este campo es requerido. (Monto)")> _
              <Display(Name:="Monto", Description:="Monto")> _
            Public Property MONTO As Double

            <Required(ErrorMessage:="Este campo es requerido. (Representante)")> _
              <Display(Name:="Representante", Description:="Representante Legal")> _
            Public Property IDREPRESENTANTELEGAL As String

            <Required(ErrorMessage:="Este campo es requerido. (Contraparte )")> _
              <Display(Name:="Contraparte ", Description:="Contraparte o Comitente")> _
            Public Property IDCOMITENTE As String

            <Display(Name:="Título Nro.", Description:="Numero Titulo")> _
            Public Property NROTITULO As String

            <Display(Name:="Indicador", Description:="Indicador")> _
            Public Property INDICADOR As String

            <Display(Name:="Puntos Indicador", Description:="Puntos Indicador")> _
            Public Property PUNTOSINDICADOR As Nullable(Of Single)

            <Required(ErrorMessage:="Este campo es requerido. (Tipo Tasa)")> _
              <Display(Name:="Tipo Tasa", Description:="Tipo Tasa")> _
            Public Property RENTAFIJA As Boolean

            <Display(Name:="Prefijo", Description:="Prefijo")> _
            Public Property PREFIJO As String

            <Display(Name:="Factura Nro.", Description:="Numero Factura")> _
            Public Property IDFACTURA As Nullable(Of Integer)

            <Display(Name:="Nombre Cliente", Description:="Nombre Cliente")> _
            Public Property NOMBRECLIENTE As String

            <Display(Name:="Nombre Representante", Description:="Nombre Representante")> _
            Public Property NOMBREREPRESENTANTE As String

            <Display(Name:="Nombre Especie", Description:="Nombre Especie")> _
            Public Property NOMBREESPECIE As String

            <Display(Name:="Facturada", Description:="Facturada")> _
            Public Property FACTURADA As String

            <Display(Name:="Estado", Description:="Estado de la Operación")> _
            Public Property ESTADO As String

            <Display(Name:="NroLote", Description:="NroLote")> _
            Public Property NroLote As Nullable(Of Integer)

            <Display(Name:="ACTUALIZACION", Description:="ACTUALIZACION")> _
            Public Property ACTUALIZACION As DateTime

            <Display(Name:="USUARIO", Description:="USUARIO")> _
            Public Property USUARIO As String

            <Display(Name:="NroLoteENC", Description:="NroLoteENC")> _
            Public Property NroLoteENC As Nullable(Of Integer)

            <Display(Name:="ContabilidadENC", Description:="ContabilidadENC")> _
            Public Property ContabilidadENC As Nullable(Of DateTime)

            <Display(Name:="Id Liquidación", Description:="Id Liquidación OTC")> _
            Public Property IdLiquidaciones_OTC As Integer

        End Class
    End Class


    <MetadataTypeAttribute(GetType(ReceptoresOT.ReceptoresOTMetadata))> _
    Partial Public Class ReceptoresOT
        Friend NotInheritable Class ReceptoresOTMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Editable(True)> _
                <Display(Name:="IDComisionista", Description:="IDComisionista")> _
            Public Property IDComisionista As Integer

            <Editable(True)> _
                <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
            Public Property IDSucComisionista As Integer

            <Editable(True)> _
                <Display(Name:="Id", Description:="Id")> _
            Public Property Id As Integer

            <Display(Name:="IDReceptor", Description:="IDReceptor")> _
            Public Property IDReceptor As String

            <Display(Name:="Lider", Description:="Lider")> _
            Public Property Lider As Boolean

            <Display(Name:="Porcentaje", Description:="Porcentaje")> _
            Public Property Porcentaje As Double

            <Display(Name:="Actualizacion", Description:="Actualizacion")> _
            Public Property Actualizacion As DateTime

            <Display(Name:="Usuario", Description:="Usuario")> _
            Public Property Usuario As String

        End Class
    End Class


End Namespace

