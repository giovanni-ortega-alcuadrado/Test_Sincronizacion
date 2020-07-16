
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

Namespace CFEspecies

    <MetadataTypeAttribute(GetType(Especi.EspeciMetadata))> _
    Partial Public Class Especi
        Friend NotInheritable Class EspeciMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="IDComisionista")> _
            Public Property IDComisionista As Integer

            <Display(Name:="IDSucComisionista")> _
            Public Property IDSucComisionista As Integer

            <StringLength(15, ErrorMessage:="El campo {0} permite una longitud máxima de 15.")> _
            <Required(ErrorMessage:="Este campo es requerido. (Nemotécnico)")> _
            <Display(Name:="Nemotécnico")> _
            Public Property Id As String


            <Display(Name:="Clase")> _
            Public Property EsAccion As Boolean

            <A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} Tiene un Formato incorrecto")>
            <Display(Name:="Clase")>
            Public Property IDClase As Integer

            <Display(Name:="Denominación")>
            Public Property IDTarifa As Nullable(Of Integer)

            <Required(ErrorMessage:="Este campo es requerido. (Grupo)")>
            <Display(Name:="Grupo")>
            Public Property IDGrupo As Integer

            <Required(ErrorMessage:="Este campo es requerido. (SubGrupo)")>
            <Display(Name:="SubGrupo")>
            Public Property IDSubGrupo As Integer

            <Display(Name:="Riesgo")>
            Public Property ClasificacionRiesgo As String

            <Display(Name:="Plazo")>
            Public Property Plazo As String

            <Display(Name:="Valor Nominal")>
            Public Property VlrNominal As Nullable(Of Decimal)

            <StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50.")>
            <Display(Name:="Notas")>
            Public Property Notas As String

            <Required(ErrorMessage:="Este campo es requerido. (Emisor)")>
            <Display(Name:="Emisor")>
            <A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} Tiene un Formato incorrecto")>
            Public Property IdEmisor As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Admon Emisión)")>
            <Display(Name:="Admon Emisión")>
            Public Property IDAdmonEmision As Integer


            <Display(Name:="Emisión")>
            Public Property Emision As Nullable(Of DateTime)

            <Display(Name:="Vencimiento")>
            Public Property Vencimiento As Nullable(Of DateTime)

            <StringLength(2, ErrorMessage:="El campo {0} permite una longitud máxima de 2.")>
            <Display(Name:="Modalidad")>
            Public Property Modalidad As String

            <Display(Name:="Tasa Inicial")>
            Public Property TasaInicial As Nullable(Of Double)

            <Display(Name:="Tasa Nominal")>
            Public Property TasaNominal As Nullable(Of Double)

            <A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} Tiene un Formato incorrecto")>
            <Required(ErrorMessage:="Este campo es requerido. (PeriodoPago)")>
            <Display(Name:="Periodo Pago Dividendo")>
            Public Property PeriodoPago As Nullable(Of Integer)

            <A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} Tiene un Formato incorrecto")>
            <Required(ErrorMessage:="Este campo es requerido. (DiaDesde)")>
            <Display(Name:="Dia Desde")>
            Public Property DiaDesde As Nullable(Of Integer)

            <A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} Tiene un Formato incorrecto")>
            <Required(ErrorMessage:="Este campo es requerido. (DiaHasta)")>
            <Display(Name:="Dia Hasta")>
            Public Property DiaHasta As Nullable(Of Integer)



            <Required(ErrorMessage:="Este campo es requerido. (DeclaraDividendos)")>
            <Display(Name:="Declara Dividendos")>
            Public Property DeclaraDividendos As Boolean

            <Required(ErrorMessage:="Este campo es requerido. (TituloMaterializado)")>
            <Display(Name:="Titulo Materializado")>
            Public Property TituloMaterializado As Boolean



            <Required(ErrorMessage:="Este campo es requerido. (Activo)")>
            <Display(Name:="Estado")>
            Public Property Activo As Boolean

            <Display(Name:="Emisora")>
            Public Property Emisora As String

            <Display(Name:="Tipo Tasa Fija")>
            Public Property TipoTasaFija As String

            <Display(Name:="Actualización")>
            Public Property Actualizacion As DateTime

            <Display(Name:="Usuario")>
            Public Property Usuario As String

            <Display(Name:="BusIntegracion")>
            Public Property BusIntegracion As Boolean

            <Display(Name:="Suscripcion")>
            Public Property Suscripcion As Nullable(Of DateTime)

            <Display(Name:="CaracteristicasRF")>
            Public Property CaracteristicasRF As Boolean

            <Display(Name:="Bursatilidad")>
            Public Property Bursatilidad As String

            <StringLength(600, ErrorMessage:="El campo {0} permite una longitud máxima de 600")>
            <Display(Name:="Clase Inversión")>
            Public Property ClaseInversion As String

            <Display(Name:="Corresponde a...")>
            Public Property Corresponde As String

            <Display(Name:="Base Calculo Interés")>
            Public Property BaseCalculoInteres As String

            <Display(Name:="Ref. Tasa Variable")>
            Public Property RefTasaVble As String

            <Display(Name:="Amortización de Capital")>
            Public Property Amortiza As String

            <Display(Name:="Clase Acciones")>
            Public Property ClaseAcciones As String

            <Display(Name:=" ")>
            Public Property Liquidez As Nullable(Of Boolean)

            <Display(Name:=" ")>
            Public Property Negociable As Nullable(Of Boolean)

            <Display(Name:="Bolsa")>
            Public Property IDBolsa As Integer

            <Display(Name:="Acciones en Circulación")>
            Public Property NroAcciones As Int64

            <Required(ErrorMessage:="Este campo es requerido. (Especies)")>
            <Display(Name:="Especies")>
            Public Property IDEspecies As Integer

            <Display(Name:="Admisible garantía")>
            Public Property AdmisionGarantia As System.Nullable(Of Decimal)

            <Display(Name:="clase contable título")>
            Public Property ClaseContableTitulo As String

        End Class
    End Class

    <MetadataTypeAttribute(GetType(EspeciesISIN.EspeciesISINMetadata))>
    Partial Public Class EspeciesISIN
        Friend NotInheritable Class EspeciesISINMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <StringLength(12, ErrorMessage:="El campo {0} permite una longitud máxima de 12.")>
            <Required(ErrorMessage:="Este campo es requerido. (ISIN)")>
            <Display(Name:="ISIN")>
            Public Property ISIN As String


            '<Required(ErrorMessage:="Este campo es requerido. (Descripción)")> _
            <StringLength(50, ErrorMessage:="El campo {0} permite una longitud máxima de 50.")>
            <Display(Name:="Descripcion")>
            Public Property Descripcion As String

        End Class
    End Class

    <MetadataTypeAttribute(GetType(ISINFungible.ISINFungibleMetadata))>
    Partial Public Class ISINFungible
        Friend NotInheritable Class ISINFungibleMetadata
            Private Sub New()
                MyBase.New()
            End Sub


            <StringLength(12, ErrorMessage:="El campo {0} permite una longitud máxima de 12.")>
            <Required(ErrorMessage:="Este campo es requerido. (ISIN)")>
            <Display(Name:="ISIN")>
            Public Property ISIN As String

            <A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} Tiene un Formato incorrecto")>
            <Required(ErrorMessage:="Este campo es requerido. (Nro. Emision)")>
            <Display(Name:="Emision")>
            Public Property Emision As Nullable(Of Integer)
        End Class
    End Class

    <MetadataTypeAttribute(GetType(EspeciesClaseInversionOld.EspeciesClaseInversionOldMetadata))>
    Partial Public Class EspeciesClaseInversionOld
        Friend NotInheritable Class EspeciesClaseInversionOldMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="TipoEntidadI")>
            Public Property IdTipoEspecieClaseInversion As Int16

            '<Required(ErrorMessage:="Este campo es requerido. (Código)")> _
            <Display(Name:="Código", Description:="Código")>
            Public Property Codigo As String

            <Required(ErrorMessage:="Este campo es requerido. (Descripción)")>
            <Display(Name:="Descripción", Description:="Descripción")>
            Public Property Descripcion As String

            <Required(ErrorMessage:="Este campo es requerido. (Método Valoración)")>
            <Display(Name:="Método Valoración", Description:="Método Valoración")>
            Public Property MetodoValoracion As String

            <Required(ErrorMessage:="Este campo es requerido. (Método Valoración)")>
            <Display(Name:="Factor Riesgo", Description:="FactorRiesgo")>
            Public Property FactorRiesgo As String

        End Class
    End Class

    <MetadataTypeAttribute(GetType(ClasesEspecie.ClasesEspecieMetadata))>
    Partial Public Class ClasesEspecie
        Friend NotInheritable Class ClasesEspecieMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ClasesEspecies")>
            Public Property IDClasesEspecies As Integer

            <Display(Name:="IDComisionista", Description:="IDComisionista")>
            Public Property IDComisionista As Integer

            <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")>
            Public Property IDSucComisionista As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Código)")>
            <Display(Name:="Código")>
            Public Property IDClaseEspecie As Integer

            <Display(Name:="Acción")>
            Public Property AplicaAccion As Boolean

            <Required(ErrorMessage:="Este campo es requerido. (Nombre)")>
            <StringLength(50, ErrorMessage:="El campo Nombre permite una longitud máxima de 50 caracteres.")>
            <Display(Name:="Nombre")>
            Public Property Nombre As String

            <Display(Name:="Actualizacion", Description:="Actualizacion")>
            Public Property Actualizacion As DateTime

            <Display(Name:="Usuario", Description:="Usuario")>
            Public Property Usuario As String

            <StringLength(15, ErrorMessage:="El campo Código inversión super permite una longitud máxima de 15 caracteres.")>
            <Display(Name:="Código inversión super")>
            Public Property strCodInversionSuper As String

            <Display(Name:="Producto")>
            Public Property IdProducto As Nullable(Of Integer)

            <Display(Name:="Título participativo")>
            Public Property TituloCarteraColectiva As Boolean

            <Display(Name:="Clase contable título")>
            Public Property ClaseContableTitulo As Boolean

            <StringLength(2, ErrorMessage:="El campo Código deceval permite una longitud máxima de 2 caracteres.")>
            <Display(Name:="Código deceval")>
            Public Property CodigoClaseDeceval As String

        End Class
    End Class

    <MetadataTypeAttribute(GetType(EspeciesDividendos.EspeciesDividendosMetadata))>
    Partial Public Class EspeciesDividendos
        Friend NotInheritable Class EspeciesDividendosMetadata
            Private Sub New()
                MyBase.New()
            End Sub


            <Required(ErrorMessage:="Este campo es requerido. (Inicio Vigencia)")>
            <Display(Name:="Inicio Vigencia")>
            Public Property InicioVigencia As DateTime

            <Required(ErrorMessage:="Este campo es requerido. (Fin Vigencia)")>
            <Display(Name:="Fin Vigencia")>
            Public Property FinVigencia As DateTime

            '<Required(ErrorMessage:="Este campo es requerido. (Causacion)")> _
            '<A2RegularExpression("[0-9]+", ErrorMessage:="El campo {0} Tiene un Formato incorrecto")> _
            '<Display(Name:="Causacion")> _
            'Public Property Causacion As Double

            <Required(ErrorMessage:="Este campo es requerido. (Inicio Pago)")> _
            <Display(Name:="Inicio Pago")> _
            Public Property InicioPago As DateTime

            <Required(ErrorMessage:="Este campo es requerido. (Fin Pago)")> _
            <Display(Name:="Fin Pago")> _
            Public Property FinPago As DateTime

            <Required(ErrorMessage:="Este campo es requerido. (Cantidad Acciones)")> _
            <Display(Name:="Cantidad Acciones")> _
            Public Property CantidadAcciones As Nullable(Of Double)


            <Display(Name:="Cantidad Pesos")> _
            Public Property CantidadPesos As Nullable(Of Decimal)

            <Display(Name:="Porcentaje Rte Fte Declarante")> _
            Public Property PorcentajeRteFteDeclarante As Nullable(Of Double)

            <Display(Name:="Porcentaje Rte Fte NO Declarante")> _
            Public Property PorcentajeRteFteNODeclarante As Nullable(Of Double)

            <Display(Name:="Gravado")> _
            Public Property Gravado As Nullable(Of Double)

        End Class
    End Class

    <MetadataTypeAttribute(GetType(EspeciesTotales.EspeciesTotalesMetadata))> _
    Partial Public Class EspeciesTotales
        Friend NotInheritable Class EspeciesTotalesMetadata
            Private Sub New()
                MyBase.New()
            End Sub





            '<Display(Name:="dtmLiquidacion", Description:="dtmLiquidacion")> _
            'Public Property dtmLiquidacion As DateTime

            '<Display(Name:="strTipo", Description:="strTipo")> _
            'Public Property strTipo As String

            '<Display(Name:="dblCantidad", Description:="dblCantidad")> _
            'Public Property dblCantidad As Double


            '<Display(Name:="curTotalLiq", Description:="curTotalLiq")> _
            'Public Property curTotalLiq As Nullable(Of Decimal)

            '<Display(Name:="curComision", Description:="curComision")> _
            'Public Property curComision As Nullable(Of Decimal)

        End Class
    End Class

End Namespace
