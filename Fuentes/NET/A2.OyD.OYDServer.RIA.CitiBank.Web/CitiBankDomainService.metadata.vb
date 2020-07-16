
Option Compare Binary
Option Infer On
Option Strict On
Option Explicit On

Imports System.ComponentModel.DataAnnotations

Namespace OyDCitiBank


    '**************************************************************************************************
    <MetadataTypeAttribute(GetType(ExcepcionesRDI.ExcepcionesRDIMetadata))> _
    Partial Public Class ExcepcionesRDI
        Friend NotInheritable Class ExcepcionesRDIMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID registro")> _
            Public Property idRegistro As Integer

            <Display(Name:="Orden")> _
            Public Property idOrden As Integer

            <Display(Name:="Clase")> _
            Public Property ClaseOrden As String

            <Display(Name:="Usuario advertencia")> _
            Public Property UsuarioAdvertencia As String

            <Display(Name:="Especie")> _
            Public Property IdEspecie As String


            '<RegularExpression("(0?[1-9]|[12][0-9]|3[01])/(0?[1-9]|1[012])/((19|20)\\d\\d)", ErrorMessage:="La fecha no es correcta")> _

            <Display(Name:="Fecha registro")> _
            Public Property Registro As DateTime

            <Display(Name:="Comitente")> _
            Public Property IDComitente As String

            <Display(Name:="Nombre")> _
            Public Property Nombre As String

            <Display(Name:="Clasificación riesgo")> _
            Public Property ClasificacionRiesgoEspecie As Integer

            <Display(Name:="Perfil inversionista")> _
            Public Property PerfilInversionistaIR As Integer

            <Display(Name:="Comentario actual (Sólo Lectura)")> _
            Public Property Comentario As String

            <Display(Name:="Comentario nuevo")> _
            Public Property NuevoComentario As String

            <Display(Name:="Usuario comentario")> _
            Public Property UsuarioComentario As String

            <Display(Name:="Fecha comentario")> _
            Public Property FechaComentario As Nullable(Of DateTime)

        End Class
    End Class


#Region "Codificación Contable"
    <MetadataTypeAttribute(GetType(CodificacionContabl.CodificacionContablMetadata))> _
    Partial Public Class CodificacionContabl
        Friend NotInheritable Class CodificacionContablMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="Indicador de Codificación")> _
            Public Property IDCodificacion As Integer

            <Display(Name:="IDComisionista", Description:="IDComisionista")> _
            Public Property IDComisionista As Integer

            <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
            Public Property IDSucComisionista As Integer

            <Display(Name:="Módulos")> _
            Public Property Modulo As String

            <Display(Name:="Tipo Operación")> _
            Public Property TipoOperacion As String

            <Display(Name:="Usar Fecha")> _
            Public Property UsarFecha As String

            <Display(Name:="Tipo Cliente")> _
            Public Property TipoCliente As String

            <Display(Name:="Branch")> _
            Public Property Branch As Nullable(Of Integer)

            <Display(Name:="Cuenta Cosmos")> _
            Public Property CuentaCosmos As Nullable(Of Decimal)

            <Display(Name:="Cod. Transacción")> _
            Public Property CodigoTransaccion As Nullable(Of Integer)

            <Display(Name:="Indicador Mvto.")> _
            Public Property IndicadorMvto As String

            <Display(Name:="Dpto. y Nro. Lote")> _
            Public Property NroLote As Nullable(Of Integer)

            <Display(Name:="Detalle adicional")> _
            Public Property DetalleAdicional As String

            <Display(Name:="Texto de detalle")> _
            Public Property TextoDetalle As String

            <Display(Name:="Nro. de referencia")> _
            Public Property NroReferencia As String

            <Display(Name:="Valor a registrar por:")> _
            Public Property PorOperacion As Nullable(Of Boolean)

            <Display(Name:="Valor a reportar")> _
            Public Property VlrAReportar As String

            <StringLength(5, ErrorMessage:="El campo {0} permite una longitud maxima de 5.")> _
            <Display(Name:="Cod. Producto")> _
            Public Property Producto As String

            <Display(Name:="Nro. Base")> _
            Public Property NroBase As String

            <Display(Name:="Con sucursal")> _
            Public Property SucursalContable As Nullable(Of Boolean)

            <Display(Name:="Consecutivo")> _
            Public Property ConsecutivoTesoreria As String

            <Display(Name:="Actualizacion", Description:="Actualizacion")> _
            Public Property Actualizacion As DateTime

            <Display(Name:="Usuario", Description:="Usuario")> _
            Public Property Usuario As String

        End Class
    End Class
#End Region

End Namespace



