
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

Namespace OyDPLUSMaestros

#Region "COMUNES"
    'The MetadataTypeAttribute identifies AuditoriaMetadata as the class
    ' that carries additional metadata for the Auditoria class.
    <MetadataTypeAttribute(GetType(Auditoria.AuditoriaMetadata))> _
    Partial Public Class Auditoria

        'This class allows you to attach custom attributes to properties
        ' of the Auditoria class.
        '
        'For example, the following marks the Xyz property as a
        ' required property and specifies the format for valid values:
        '    <Required()>
        '    <A2RegularExpression("[A-Z][A-Za-z0-9]*")>
        '    <StringLength(32)>
        '    Public Property Xyz As String
        Friend NotInheritable Class AuditoriaMetadata

            'Metadata classes are not meant to be instantiated.
            Private Sub New()
                MyBase.New()
            End Sub

            Public Property Browser As String

            Public Property DirIPMaquina As String

            Public Property ErrorPersonalizado As Short

            Public Property Maquina As String

            Public Property Servidor As String

            Public Property Usuario As String
        End Class
    End Class
#End Region

#Region "MAESTROS DE OYDPLUS"

    '**************************************************************************************************
    'Mensajes
    <MetadataTypeAttribute(GetType(Mensaje.MensajeMetadata))>
    Partial Public Class Mensaje
        Friend NotInheritable Class MensajeMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="IDMensaje", Description:="IDMensaje")>
            Public Property IDMensaje As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Código Mensaje)")>
            <Display(Name:="Código Mensaje", Description:="Código Mensaje")>
            Public Property CodigoMensaje As String

            <Required(ErrorMessage:="Este campo es requerido. (Mensaje)")>
            <Display(Name:="Mensaje", Description:="Mensaje para mostrar en la aplicación.")>
            Public Property Mensaje As String

            <Display(Name:="Mensaje Con Reemplazo", Description:="Mensaje para mostrar en la aplicación reemplazando algunos valores.")>
            Public Property MensajeConReempl As String

            <Display(Name:="Usuario", Description:="Usuario")>
            Public Property Usuario As String

        End Class
    End Class
    '**************************************************************************************************

    '**************************************************************************************************
    'Reglas
    <MetadataTypeAttribute(GetType(Regla.ReglaMetadata))>
    Partial Public Class Regla
        Friend NotInheritable Class ReglaMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID Regla", Description:=" ID Regla")>
            Public Property IDRegla As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Código Regla)")>
            <Display(Name:="Código Regla", Description:="Código Regla")>
            Public Property CodigoRegla As String

            <Display(Name:="Descripción Regla", Description:="Descripción corta de lo que hace la Regla.")>
            Public Property DescripcionRegla As String

            <Required(ErrorMessage:="Este campo es requerido. (Modulo)")>
            <Display(Name:="Modulo", Description:="Modulo al que le aplica la regla 'ORDENES' - 'ORDENESTESORERIA'")>
            Public Property Modulo As String

            <Required(ErrorMessage:="Este campo es requerido. (Nombre Corto)")>
            <Display(Name:="Nombre Corto", Description:="Nombre Corto para mostrar en pantalla cuando se incumpla la regla.")>
            Public Property NombreCorto As String

            <Required(ErrorMessage:="Este campo es requerido. (Prioridad)")>
            <Display(Name:="Prioridad", Description:="Prioridad con la cual se validara la regla de negocio.")>
            Public Property Prioridad As Nullable(Of Integer)

            <Required(ErrorMessage:="Este campo es requerido. (Acción Regla)")>
            <Display(Name:="Acción Regla", Description:="Habilitar o deshabilitar la regla de negocio.")>
            Public Property CodigoTipoRegla As String

            <Display(Name:="Causas Justificación", Description:="Causas de la justificación para mostrar al usuario.")>
            Public Property CausasJustificacion As String

            <Display(Name:="Nombre Procedimiento", Description:="Nombre Procedimiento para validar la regla.")>
            Public Property NombreProcedimiento As String

            <Display(Name:="Parametros", Description:="Parametros que recibe el procedimiento.")>
            Public Property Parametros As String

            <Display(Name:="Usuario", Description:="Usuario")>
            Public Property Usuario As String

            <Display(Name:="Motor calculos", Description:="Indica sí la regla se evalua por motor de calculos")>
            Public Property MotorCalculos As Boolean

        End Class
    End Class
    '**************************************************************************************************

    '**************************************************************************************************
    'Mensajes Reglas
    <MetadataTypeAttribute(GetType(MensajesRegla.MensajesReglaMetadata))>
    Partial Public Class MensajesRegla
        Friend NotInheritable Class MensajesReglaMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID Mensaje Regla", Description:="ID Mensaje Regla")>
            Public Property IDMensajeRegla As Integer

            <Required(ErrorMessage:="Este campo es requerido. (ID Regla)")>
            <Display(Name:="Código Regla", Description:="Código de la regla")>
            Public Property IDRegla As Integer

            <Required(ErrorMessage:="Este campo es requerido. (ID Mensaje)")>
            <Display(Name:="Código Mensaje", Description:="Código del mensaje")>
            Public Property IDMensaje As Integer

            <Required(ErrorMessage:="Este campo es requerido. (ID Tipo Mensaje)")>
            <Display(Name:="ID Tipo Mensaje", Description:="ID Tipo Mensaje")>
            Public Property IDTipoMensaje As Integer

            <Display(Name:="Usuario", Description:="Usuario")>
            Public Property Usuario As String

        End Class
    End Class
    '**************************************************************************************************

    '**************************************************************************************************
    'Tipo Negocio
    <MetadataTypeAttribute(GetType(TipoNegocio.TipoNegocioMetadata))>
    Partial Public Class TipoNegocio
        Friend NotInheritable Class TipoNegocioMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")>
            Public Property ID As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Código)")>
            <Display(Name:="Código", Description:="Código")>
            Public Property Codigo As String

            <Required(ErrorMessage:="Este campo es requerido. (Descripción)")>
            <Display(Name:="Descripción", Description:="Descripción")>
            Public Property Descripcion As String

            <Display(Name:="Configuración Menu", Description:="Configuración Menu")>
            Public Property ConfiguracionMenu As String

        End Class
    End Class
    '**************************************************************************************************

    '**************************************************************************************************
    'Certificaciones x Tipo Negogcio
    <MetadataTypeAttribute(GetType(CertificacionXTipoNegocio.CertificacionXTipoNegocioMetadata))>
    Partial Public Class CertificacionXTipoNegocio
        Friend NotInheritable Class CertificacionXTipoNegocioMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")>
            Public Property ID As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Código Certificacion)")>
            <Display(Name:="Código Certificacion", Description:="Código Certificacion")>
            Public Property CodigoCertificacion As String

            <Required(ErrorMessage:="Este campo es requerido. (Código Tipo Negocio)")>
            <Display(Name:="Código Tipo Negocio", Description:="Código Tipo Negocio")>
            Public Property CodigoTipoNegocio As String

        End Class
    End Class
    '**************************************************************************************************

    '**************************************************************************************************
    'Documento x Tipo Negocio
    <MetadataTypeAttribute(GetType(DocumentoXTipoNegocio.DocumentoXTipoNegocioMetadata))>
    Partial Public Class DocumentoXTipoNegocio
        Friend NotInheritable Class DocumentoXTipoNegocioMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")>
            Public Property ID As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Tipo Negocio)")>
            <Display(Name:="Tipo Negocio", Description:="Tipo Negocio")>
            Public Property TipoNegocio As String

            <Required(ErrorMessage:="Este campo es requerido. (Cod Documento)")>
            <Display(Name:="Cod Documento", Description:="Cod Documento")>
            Public Property CodDocumento As Nullable(Of Int32)

        End Class
    End Class
    '**************************************************************************************************

    '**************************************************************************************************
    'Especie x Tipo negocio
    <MetadataTypeAttribute(GetType(TipoNegocioXEspecie.TipoNegocioXEspecieMetadata))>
    Partial Public Class TipoNegocioXEspecie
        Friend NotInheritable Class TipoNegocioXEspecieMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")>
            Public Property ID As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Tipo Negocio)")>
            <Display(Name:="Tipo Negocio", Description:="Tipo Negocio")>
            Public Property TipoNegocio As String

            <Required(ErrorMessage:="Este campo es requerido. (Especie)")>
            <Display(Name:="Especie", Description:="Especie")>
            Public Property IDEspecie As String

            <Display(Name:="Maneja ISIN", Description:="Maneja ISIN")>
            Public Property ManejaISIN As Boolean

            <Required(ErrorMessage:="Este campo es requerido. (Max Valor Cantidad)")>
            <Display(Name:="Max Valor Cantidad", Description:="Max Valor Cantidad")>
            Public Property MaxValorCantidad As Double

        End Class
    End Class
    '**************************************************************************************************

    '**************************************************************************************************
    'Tipo Producto x Tipo Negocio
    <MetadataTypeAttribute(GetType(TipoNegocioXTipoProducto.TipoNegocioXTipoProductoMetadata))>
    Partial Public Class TipoNegocioXTipoProducto
        Friend NotInheritable Class TipoNegocioXTipoProductoMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")>
            Public Property ID As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Tipo Negocio)")>
            <Display(Name:="Tipo Negocio", Description:="Tipo Negocio")>
            Public Property TipoNegocio As String

            <Required(ErrorMessage:="Este campo es requerido. (Tipo Producto)")>
            <Display(Name:="Tipo Producto", Description:="Tipo Producto")>
            Public Property TipoProducto As String

            <Display(Name:="Cliente", Description:="Cliente")>
            Public Property IDComitente As String

            <Display(Name:="Perfil", Description:="Perfil")>
            Public Property Perfil As String

            <Required(ErrorMessage:="Este campo es requerido. (Valor Max Negociación)")>
            <Display(Name:="Valor Max Negociación", Description:="Valor Max Negociación")>
            Public Property ValorMaxNegociacion As Double

            <Required(ErrorMessage:="Este campo es requerido. (Porcentaje de patrimonio técnico)")>
            <Range(0, 100, ErrorMessage:="{0} debe tener un valor entre 0 y 100")>
            <Display(Name:="Porcentaje de patrimonio técnico", Description:="Porcentaje de patrimonio técnico")>
            Public Property PorcentajePatrimonioTecnico As Double

        End Class
    End Class
    '**************************************************************************************************

    '**************************************************************************************************
    'Certificaciones Receptor
    <MetadataTypeAttribute(GetType(CertificacionesXRecepto.CertificacionesXReceptoMetadata))>
    Partial Public Class CertificacionesXRecepto
        Friend NotInheritable Class CertificacionesXReceptoMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")>
            Public Property ID As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Código Receptor)")>
            <Display(Name:="Código Receptor", Description:="Código Receptor")>
            Public Property CodigoReceptor As String

            <Required(ErrorMessage:="Este campo es requerido. (Codigo Certificación)")>
            <Display(Name:="Codigo Certificación", Description:="Codigo Certificación")>
            Public Property CodigoCertificacion As String

            <Required(ErrorMessage:="Este campo es requerido. (Fecha Inicial)")>
            <Display(Name:="Fecha Inicial", Description:="Fecha Inicial")>
            Public Property FechaInicial As DateTime

            <Required(ErrorMessage:="Este campo es requerido. (Fecha Final)")>
            <Display(Name:="Fecha Final", Description:="Fecha Final")>
            Public Property FechaFinal As DateTime

            <Display(Name:="Usuario", Description:="Usuario")>
            Public Property Usuario As String

        End Class
    End Class
    '**************************************************************************************************

    '**************************************************************************************************
    'Conceptos Receptor
    <MetadataTypeAttribute(GetType(ConceptosXRecepto.ConceptosXReceptoMetadata))>
    Partial Public Class ConceptosXRecepto
        Friend NotInheritable Class ConceptosXReceptoMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")>
            Public Property ID As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Código Receptor)")>
            <Display(Name:="Código Receptor", Description:="Código Receptor")>
            Public Property CodigoReceptor As String

            <Required(ErrorMessage:="Este campo es requerido. (Concepto)")>
            <Display(Name:="Concepto", Description:="Concepto")>
            Public Property IdConcepto As Nullable(Of Integer)

            <Required(ErrorMessage:="Este campo es requerido. (Prioridad)")>
            <Display(Name:="Prioridad", Description:="Prioridad")>
            Public Property Prioridad As Integer

            <Display(Name:="Usuario", Description:="Usuario")>
            Public Property Usuario As String

        End Class
    End Class
    '**************************************************************************************************

    '**************************************************************************************************
    'Configuraciones Adicionales Receptor
    <MetadataTypeAttribute(GetType(ConfiguracionesAdicionalesRecepto.ConfiguracionesAdicionalesReceptoMetadata))>
    Partial Public Class ConfiguracionesAdicionalesRecepto
        Friend NotInheritable Class ConfiguracionesAdicionalesReceptoMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Required(ErrorMessage:="Este campo es requerido. (Código Receptor)")>
            <Display(Name:="Código Receptor", Description:="Código Receptor")>
            Public Property CodigoReceptor As String

            <Required(ErrorMessage:="Este campo es requerido. (Tipo Orden)")>
            <Display(Name:="Tipo Orden", Description:="Tipo Orden")>
            Public Property TipoOrden As String

            <Display(Name:="Extensión Defecto", Description:="Extensión Defecto")>
            Public Property ExtensionDefecto As String

            <Display(Name:="Tipo Orden Defecto", Description:="Tipo Orden Defecto")>
            Public Property TipoOrdenDefecto As String

            <Required(ErrorMessage:="Este campo es requerido. (Porcentaje de patrimonio técnico)")>
            <Range(0, 100, ErrorMessage:="{0} debe tener un valor entre 0 y 100")>
            <Display(Name:="Porcentaje de patrimonio técnico", Description:="Porcentaje de patrimonio técnico")>
            Public Property PorcentajePatrimonioTecnico As Double

            <Required(ErrorMessage:="Este campo es requerido. (Enruta Bus)")>
            <Display(Name:="Enruta Bus", Description:="Enruta Bus")>
            Public Property EnrutaBus As Nullable(Of Boolean)

            <Display(Name:="Usuario", Description:="Usuario")>
            Public Property Usuario As String

        End Class
    End Class
    '**************************************************************************************************

    '**************************************************************************************************
    'Parametros Receptor
    <MetadataTypeAttribute(GetType(ParametrosRecepto.ParametrosReceptoMetadata))>
    Partial Public Class ParametrosRecepto
        Friend NotInheritable Class ParametrosReceptoMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")>
            Public Property ID As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Código Receptor)")>
            <Display(Name:="Código Receptor", Description:="Código Receptor")>
            Public Property CodigoReceptor As String

            <Required(ErrorMessage:="Este campo es requerido. (Topico)")>
            <Display(Name:="Topico", Description:="Topico")>
            Public Property Topico As String

            <Required(ErrorMessage:="Este campo es requerido. (Valor)")>
            <Display(Name:="Valor", Description:="Valor")>
            Public Property Valor As String

            <Display(Name:="Prioridad", Description:="Prioridad")>
            Public Property Prioridad As Integer

            <Display(Name:="Usuario", Description:="Usuario")>
            Public Property Usuario As String

        End Class
    End Class

    '**************************************************************************************************
    'Receptores Clientes Autorizado
    <MetadataTypeAttribute(GetType(ReceptoresClientesAutorizado.ReceptoresClientesAutorizadoMetadata))>
    Partial Public Class ReceptoresClientesAutorizado
        Friend NotInheritable Class ReceptoresClientesAutorizadoMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")>
            Public Property ID As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Codigo Receptor Cliente)")>
            <Display(Name:="Codigo Receptor Cliente", Description:="Codigo Receptor Cliente")>
            Public Property CodigoReceptorCliente As String

            <Required(ErrorMessage:="Este campo es requerido. (Codigo Receptor Tercero)")>
            <Display(Name:="Codigo Receptor Tercero", Description:="Codigo Receptor Tercero")>
            Public Property CodigoReceptorTercero As String

            <Display(Name:="Cliente", Description:="Cliente")>
            Public Property IDComitente As String

            <Required(ErrorMessage:="Este campo es requerido. (Fecha Inicial)")>
            <Display(Name:="Fecha Inicial", Description:="Fecha Inicial")>
            Public Property FechaInicial As DateTime

            <Required(ErrorMessage:="Este campo es requerido. (Fecha Final)")>
            <Display(Name:="Fecha Final", Description:="Fecha Final")>
            Public Property FechaFinal As DateTime

            <Display(Name:="Usuario", Description:="Usuario")>
            Public Property Usuario As String

        End Class
    End Class
    '**************************************************************************************************

    '**************************************************************************************************
    'Tipo de negocio receptor
    <MetadataTypeAttribute(GetType(TipoNegocioXRecepto.TipoNegocioXReceptoMetadata))>
    Partial Public Class TipoNegocioXRecepto
        Friend NotInheritable Class TipoNegocioXReceptoMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")>
            Public Property ID As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Código Receptor)")>
            <Display(Name:="Código Receptor", Description:="Código Receptor")>
            Public Property CodigoReceptor As String

            <Required(ErrorMessage:="Este campo es requerido. (Código Tipo Negocio)")>
            <Display(Name:="Código Tipo Negocio", Description:="Código Tipo Negocio")>
            Public Property CodigoTipoNegocio As String

            <Required(ErrorMessage:="Este campo es requerido. (Valor Max Negociación)")>
            <Display(Name:="Valor Max Negociación", Description:="Valor Max Negociación")>
            Public Property ValorMaxNegociacion As Double

            <Required(ErrorMessage:="Este campo es requerido. (Prioridad)")>
            <Display(Name:="Prioridad", Description:="Prioridad")>
            Public Property Prioridad As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Porcentaje Comisión)")>
            <Display(Name:="Porcentaje Comisión", Description:="Porcentaje Comisión")>
            Public Property PorcentajeComision As Nullable(Of Double)

            <Required(ErrorMessage:="Este campo es requerido. (Valor Comisión)")>
            <Display(Name:="Valor Comisión", Description:="Valor Comisión")>
            Public Property ValorComision As Nullable(Of Double)

            <Display(Name:="Usuario", Description:="Usuario")>
            Public Property Usuario As String

        End Class
    End Class
    '**************************************************************************************************

    '**************************************************************************************************
    'Tipo producto Receptor
    <MetadataTypeAttribute(GetType(TipoProductoXRecepto.TipoProductoXReceptoMetadata))>
    Partial Public Class TipoProductoXRecepto
        Friend NotInheritable Class TipoProductoXReceptoMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")>
            Public Property ID As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Código Receptor)")>
            <Display(Name:="Código Receptor", Description:="Código Receptor")>
            Public Property CodigoReceptor As String

            <Required(ErrorMessage:="Este campo es requerido. (Tipo Producto)")>
            <Display(Name:="Tipo Producto", Description:="Tipo Producto")>
            Public Property TipoProducto As String

            <Required(ErrorMessage:="Este campo es requerido. (Prioridad)")>
            <Display(Name:="Prioridad", Description:="Prioridad")>
            Public Property Prioridad As Integer

            <Display(Name:="Usuario", Description:="Usuario")>
            Public Property Usuario As String

        End Class
    End Class
    '**************************************************************************************************

    '**************************************************************************************************
    <MetadataTypeAttribute(GetType(RN_ConfiguracionNivelAtribucio.RN_ConfiguracionNivelAtribucioMetadata))>
    Partial Public Class RN_ConfiguracionNivelAtribucio
        Friend NotInheritable Class RN_ConfiguracionNivelAtribucioMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="Código", Description:="Código")>
            Public Property ID As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Regla)")>
            <Display(Name:="Regla", Description:="Regla de Negocio")>
            Public Property Regla As Nullable(Of Integer)

            <Required(ErrorMessage:="Este campo es requerido. (Tipo Documento)")>
            <Display(Name:="Tipo Documento", Description:="Tipo Documento Autorizaciones")>
            Public Property TipoDocumento As Nullable(Of Integer)

            <Required(ErrorMessage:="Este campo es requerido. (Nivel Atribución)")>
            <Display(Name:="Nivel Atribución", Description:="Nivel Atribución que Aprobara")>
            Public Property NivelAprobacion As Nullable(Of Integer)

            <Display(Name:="Usuario", Description:="Usuario")>
            Public Property Usuario As String

        End Class
    End Class


    <MetadataTypeAttribute(GetType(Costo.CostoMetadata))>
    Partial Public Class Costo
        Friend NotInheritable Class CostoMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="Id Costo", Description:="Id Costo")>
            Public Property Idcosto As Integer

            <Required(ErrorMessage:="Este campo es requerido. (CodigoFormapago)")>
            <Display(Name:="Forma Pago", Description:="Código de Forma de Pago")>
            Public Property CodigoFormapago As String

            <Display(Name:="Tipo Cheque", Description:="Código de Tipo de Cheque")>
            Public Property CodigoTipoCheque As String

            <Display(Name:="Tipo Cruce", Description:="Código Tipo Cruce")>
            Public Property CodigoTipoCruce As String

            <Required(ErrorMessage:="Este campo es requerido. (Valor)")>
            <Display(Name:="Valor", Description:="Valor")>
            Public Property Valor As Decimal

            '<Display(Name:="Usuario", Description:="Usuario")> _
            'Public Property Usuario As String

        End Class
    End Class

    '**************************************************************************************************
    <MetadataTypeAttribute(GetType(TipoProductoXEspeci.TipoProductoXEspeciMetadata))>
    Partial Public Class TipoProductoXEspeci
        Friend NotInheritable Class TipoProductoXEspeciMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")>
            Public Property ID As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Tipo producto)")>
            <Display(Name:="Tipo producto", Description:="Tipo producto")>
            Public Property CodigoTipoProducto As String

            <Display(Name:="Especie", Description:="Especie")>
            Public Property IDEspecie As String

            <Required(ErrorMessage:="Este campo es requerido. (Valor Max Negociación)")>
            <Display(Name:="Valor Max Negociación", Description:="Valor Max Negociación")>
            Public Property ValorMaxNegociacion As Double

            <Display(Name:="Usuario", Description:="Usuario")>
            Public Property Usuario As String

        End Class
    End Class

    '**************************************************************************************************
    <MetadataTypeAttribute(GetType(Plantilla.PlantillaMetadata))>
    Partial Public Class Plantilla
        Friend NotInheritable Class PlantillaMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="Id de plantilla", Description:="Id de plantilla")>
            Public Property intID As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Código de plantilla)")>
            <Display(Name:="Código de plantilla", Description:="Código de plantilla")>
            Public Property strCodigo As String

            <Required(ErrorMessage:="Este campo es requerido. (Cuerpo de la plantilla)")>
            <Display(Name:="Plantilla", Description:="Cuerpo de la plantilla")>
            Public Property strMensaje As String

        End Class
    End Class

    '**************************************************************************************************
    <MetadataTypeAttribute(GetType(ControlHorario.ControlHorarioMetadata))>
    Partial Public Class ControlHorario
        Friend NotInheritable Class ControlHorarioMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")>
            Public Property ID As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Modulo)")>
            <Display(Name:="Modulo", Description:="Modulo")>
            Public Property Modulo As System.Nullable(Of Integer)

            <Display(Name:="Tipo negocio", Description:="Tipo negocio")>
            Public Property TipoNegocio As String

            <Display(Name:="Tipo orden", Description:="Tipo orden")>
            Public Property TipoOrden As System.Nullable(Of Integer)

            <Display(Name:="Tipo producto", Description:="Tipo producto")>
            Public Property TipoProducto As System.Nullable(Of Integer)

            <Display(Name:="Instrucción", Description:="Instrucción")>
            Public Property Instruccion As System.Nullable(Of Integer)

            <Display(Name:="Hora inicio", Description:="Hora inicio")>
            <Required(ErrorMessage:="Este campo es requerido. (Hora inicio)")>
            Public Property HoraInicio As String

            <Display(Name:="Hora fin", Description:="Hora fin")>
            <Required(ErrorMessage:="Este campo es requerido. (Hora fin)")>
            Public Property HoraFin As String

            <Display(Name:="Usuario", Description:="Usuario")>
            Public Property Usuario As String

        End Class
    End Class

    '**************************************************************************************************
    <MetadataTypeAttribute(GetType(PlantillaBanco.PlantillaBancoMetadata))>
    Partial Public Class PlantillaBanco
        Friend NotInheritable Class PlantillaBancoMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="Id de plantilla por banco", Description:="Id de plantilla por banco")>
            Public Property Id As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Id del banco)")>
            <Display(Name:="Id del banco", Description:="Id del banco")>
            Public Property IdBanco As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Id de la plantilla)")>
            <Display(Name:="Id de la plantilla", Description:="Id de la plantilla")>
            Public Property IdPlantilla As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Descripción)")>
            <Display(Name:="Descripción", Description:="Descripción")>
            Public Property strDescripcion As String

            '<A2RegularExpression("[A-Za-zñÑ\w\s]{0,25}", ErrorMessage:="Existen caracteres no permitidos en el campo {0}, solo se permiten letras y números.")> _
            <Required(ErrorMessage:="Este campo es requerido. (Extensión - tipo de archivo)")>
            <A2RegularExpression("^.((doc)|(docx)|(pdf)|(html))$", ErrorMessage:="Por favor ingrese una extensión de archivo válida ejm: (.doc|.docx|.pdf|.html).")>
            <Display(Name:="Extensión - tipo de archivo", Description:="Extensión - tipo de archivo")> _
            Public Property strExtension As String

            <Display(Name:="Nombre del banco", Description:="Nombre del banco")> _
            Public Property strNombre As String

            <Display(Name:="Nombre de la plantilla", Description:="Nombre de la plantilla")> _
            Public Property strCodigo As String

        End Class
    End Class

    <MetadataTypeAttribute(GetType(ClientesRelacionados.ClientesRelacionadosMetadata))> _
    Partial Public Class ClientesRelacionados
        Friend NotInheritable Class ClientesRelacionadosMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")> _
            Public Property ID As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Tipo identificación cliente)")> _
            <Display(Name:="Tipo identificación cliente")> _
            Public Property TipoIdCliente As String

            <Display(Name:="Tipo identificación cliente")> _
            Public Property NombreTipoIdCliente As String

            <Required(ErrorMessage:="Este campo es requerido. (Nro documento cliente)")> _
            <Display(Name:="Nro documento cliente")> _
            Public Property NroDocumentoCliente As String

            <Display(Name:="Nombre cliente")> _
            Public Property NombreCliente As String

            <Required(ErrorMessage:="Este campo es requerido. (Tipo identificación cliente relacionado)")> _
            <Display(Name:="Tipo identificación cliente relacionado")> _
            Public Property TipoIdClienteRelacionado As String

            <Display(Name:="Tipo identificación cliente relacionado")> _
            Public Property NombreTipoIdClienteRelacionado As String

            <Required(ErrorMessage:="Este campo es requerido. (Nro documento cliente relacionado)")> _
            <Display(Name:="Nro documento cliente relacionado")> _
            Public Property NroDocumentoClienteRelacionado As String

            <Display(Name:="Nombre cliente relacionado")> _
            Public Property NombreClienteRelacionado As String

            <Required(ErrorMessage:="Este campo es requerido. (Tipo relación)")> _
            <Display(Name:="Tipo relación")> _
            Public Property TipoRelacion As String

            <Display(Name:="Usuario", Description:="Usuario")> _
            Public Property Usuario As String
        End Class
    End Class

    <MetadataTypeAttribute(GetType(ClientesRelacionadosEncabezado.ClientesRelacionadosEncabezadoMetadata))> _
    Partial Public Class ClientesRelacionadosEncabezado
        Friend NotInheritable Class ClientesRelacionadosEncabezadoMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")> _
            Public Property ID As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Tipo identificación cliente)")> _
            <Display(Name:="Tipo identificación cliente")> _
            Public Property TipoIdCliente As String

            <Display(Name:="Tipo identificación cliente")> _
            Public Property NombreTipoIdCliente As String

            <Required(ErrorMessage:="Este campo es requerido. (Nro documento cliente)")> _
            <Display(Name:="Nro documento cliente")> _
            Public Property NroDocumentoCliente As String

            <Display(Name:="Nombre cliente")> _
            Public Property NombreCliente As String

        End Class
    End Class

    <MetadataTypeAttribute(GetType(PortalioClienteXTipoNegocio.PortalioClienteXTipoNegocioMetadata))> _
    Partial Public Class PortalioClienteXTipoNegocio
        Friend NotInheritable Class PortalioClienteXTipoNegocioMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")> _
            Public Property ID As Integer

            <Display(Name:="Tipo producto")> _
            Public Property TipoProducto As String

            <Display(Name:="Perfil riesgo")> _
            Public Property PerfilRiesgo As String

            <Display(Name:="Código OYD")> _
            Public Property CodigoOYD As String

            <Display(Name:="Nombre")> _
            Public Property NombreCodigoOYD As String

            <Required(ErrorMessage:="Este campo es requerido. (Tipo negocio)")> _
            <Display(Name:="Tipo negocio")> _
            Public Property TipoNegocio As String

            <Required(ErrorMessage:="Este campo es requerido. (Porcentaje)")> _
            <Display(Name:="Porcentaje")> _
            Public Property Porcentaje As Double

            <Required(ErrorMessage:="Este campo es requerido. (Cupo máximo)")> _
            <Display(Name:="Cupo máximo")> _
            Public Property ValorMaximoCupo As Double

        End Class
    End Class


    <MetadataTypeAttribute(GetType(PortalioClienteXTipoNegocioPrincipal.PortalioClienteXTipoNegocioPrincipalMetadata))> _
    Partial Public Class PortalioClienteXTipoNegocioPrincipal
        Friend NotInheritable Class PortalioClienteXTipoNegocioPrincipalMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")> _
            Public Property ID As Integer

            <Display(Name:="Tipo producto")> _
            Public Property TipoProducto As String

            <Display(Name:="Perfil riesgo")> _
            Public Property PerfilRiesgo As String

            <Display(Name:="Código OYD")> _
            Public Property CodigoOYD As String

            <Display(Name:="Nombre")> _
            Public Property NombreCodigoOYD As String

            <Display(Name:="Perfil riesgo")> _
            Public Property NombrePerfilRiesgo As String

            <Display(Name:="Tipo producto")> _
            Public Property NombreTipoProducto As String

        End Class
    End Class

    <MetadataTypeAttribute(GetType(DistribucionComisionXTipoNegocio.DistribucionComisionXTipoNegocioMetadata))> _
    Partial Public Class DistribucionComisionXTipoNegocio
        Friend NotInheritable Class DistribucionComisionXTipoNegocioMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")> _
            Public Property ID As Integer

            <Required(ErrorMessage:="Este campo es requerido. (Tipo negocio)")> _
            <Display(Name:="Tipo negocio")> _
            Public Property TipoNegocio As String

            <Display(Name:="Tipo producto")> _
            Public Property TipoProducto As String

            <Display(Name:="Perfil riesgo")> _
            Public Property PerfilRiesgo As String

            <Display(Name:="Código OYD")> _
            Public Property CodigoOYD As String

            <Display(Name:="Nombre")> _
            Public Property NombreCodigoOYD As String

            <Required(ErrorMessage:="Este campo es requerido. (Límite inferior)")> _
            <Display(Name:="Límite inferior")> _
            Public Property LimiteInferior As Double

            <Required(ErrorMessage:="Este campo es requerido. (Límite inferior)")> _
            <Display(Name:="Límite superior")> _
            Public Property LimiteSuperior As Double

            <Required(ErrorMessage:="Este campo es requerido. (Comisión minima)")> _
            <Display(Name:="Comisión minima")> _
            Public Property ComisionMinima As Double

            <Required(ErrorMessage:="Este campo es requerido. (Comisión máxima)")> _
            <Display(Name:="Comisión máxima")> _
            Public Property ComisionMaxima As Double

            <Required(ErrorMessage:="Este campo es requerido. (Comisión en porcentaje)")> _
            <Display(Name:="Comisión en porcentaje")> _
            Public Property ComisionEnPorcentaje As Boolean


        End Class
    End Class

    <MetadataTypeAttribute(GetType(CupoReceptorXTipoNegocio.CupoReceptorXTipoNegocioMetadata))> _
    Partial Public Class CupoReceptorXTipoNegocio
        Friend NotInheritable Class CupoReceptorXTipoNegocioMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")> _
            Public Property ID As Integer

            <Display(Name:="Sucursal")> _
            Public Property IDSucursal As System.Nullable(Of Integer)

            <Display(Name:="Mesa")> _
            Public Property IDMesa As System.Nullable(Of Integer)

            <Display(Name:="Receptor")> _
            Public Property IDReceptor As String

            <Display(Name:="Nombre")> _
            Public Property NombreIDReceptor As String

            <Required(ErrorMessage:="Este campo es requerido. (Tipo negocio)")> _
            <Display(Name:="Tipo negocio")> _
            Public Property TipoNegocio As String

            <Required(ErrorMessage:="Este campo es requerido. (Porcentaje cupo)")> _
            <Display(Name:="Porcentaje cupo")> _
            Public Property PorcentajeCupo As Double

        End Class
    End Class

    <MetadataTypeAttribute(GetType(CupoReceptorXTipoNegocioPrincipal.CupoReceptorXTipoNegocioPrincipalMetadata))> _
    Partial Public Class CupoReceptorXTipoNegocioPrincipal
        Friend NotInheritable Class CupoReceptorXTipoNegocioPrincipalMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")> _
            Public Property ID As Integer

            <Display(Name:="Sucursal")> _
            Public Property IDSucursal As System.Nullable(Of Integer)

            <Display(Name:="Mesa")> _
            Public Property IDMesa As System.Nullable(Of Integer)

            <Display(Name:="Receptor")> _
            Public Property IDReceptor As String

            <Display(Name:="Nombre")> _
            Public Property NombreReceptor As String

            <Display(Name:="Sucursal")> _
            Public Property NombreSucursal As String

            <Display(Name:="Mesa")> _
            Public Property NombreMesa As String

        End Class
    End Class

    '**************************************************************************************************

    '**************************************************************************************************
    'Alias Especies
    <MetadataTypeAttribute(GetType(AliasEspecie.AliasEspecieMetadata))> _
    Partial Public Class AliasEspecie
        Friend NotInheritable Class AliasEspecieMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="Id especie", Description:=" Id especie")> _
            Public Property IDEspecies As Integer

            <Display(Name:="Nemotécnico", Description:="Nemotécnico.")> _
            Public Property Nemotecnico As String

            <Display(Name:="Nombre", Description:="Nombre especie")> _
            Public Property Nombre As String

            <StringLength(50, ErrorMessage:="La longitud máxima del Alias es de: 50")> _
            <Display(Name:="Alias ", Description:="Alias de la especie.")> _
            Public Property AliasEspecie As String

            <Display(Name:="Usuario", Description:="Usuario")> _
            Public Property Usuario As String

        End Class
    End Class

    ''' <summary>
    ''' Metadata para Bancos nacionales fondos
    ''' </summary>
    ''' <remarks></remarks>
    '''
    <MetadataTypeAttribute(GetType(BancosNacionalesFondos.BancosNacionalesFondosMetaData))> _
    Partial Public Class BancosNacionalesFondos
        Friend NotInheritable Class BancosNacionalesFondosMetaData
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID")> _
            Public Property ID As Integer

            <Required(ErrorMessage:="El banco es un campo requerido")> _
            <Display(Name:="Banco")> _
            Public Property IDBanco As Integer

            <Required(ErrorMessage:="El nro de cuenta es un campo requerido")> _
            <Display(Name:="Nro cuenta")> _
            Public Property NroCuenta As String

            <Required(ErrorMessage:="El tipo cuenta es un campo requerido")> _
            <Display(Name:="Tipo cuenta")> _
            Public Property TipoCuenta As String

            <Required(ErrorMessage:="El Tipo documento titular es un campo requerido")> _
            <Display(Name:="Tipo documento titular")> _
            Public Property TipoDocumentoTitular As String

            <Required(ErrorMessage:="El Nro documento titular es un campo requerido")> _
            <Display(Name:="Nro documento titular")> _
            Public Property NroDocumentoTitular As String

            <Required(ErrorMessage:="El Nombre titular es un campo requerido")> _
            <Display(Name:="Nombre titular")> _
            Public Property NombreTitular As String

            <Display(Name:="Código banco cartera")> _
            Public Property IDCodigoBancoSafyr As Nullable(Of Integer)

            <Required(ErrorMessage:="La cartera colectiva es un campo requerido")> _
            <Display(Name:="Cartera colectiva")> _
            Public Property CarteraColectiva As String

        End Class
    End Class

#End Region

End Namespace
