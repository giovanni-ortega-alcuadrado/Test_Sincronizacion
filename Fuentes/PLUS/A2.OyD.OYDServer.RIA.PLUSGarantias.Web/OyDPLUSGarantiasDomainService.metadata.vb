
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

Namespace OyDPLUSGarantias

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
        '    <RegularExpression("[A-Z][A-Za-z0-9]*")>
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

End Namespace

#Region "Visor de Garantias"
<MetadataTypeAttribute(GetType(VisorDeGarantias.VisorDeGarantiasMetadata))> _
Partial Public Class VisorDeGarantias
    Friend NotInheritable Class VisorDeGarantiasMetadata

        Public Property ID() As Integer

        Property IDCliente() As String

        Public Property NroDocumento() As String

        Public Property NombreCliente() As String

        Public Property TipoProducto() As String

        Public Property IDLiquidacion() As Integer

        Public Property Parcial() As Integer

        Public Property Tipo() As String

        Public Property ClaseOrden() As String

        Public Property FechaLiquidacion() As System.Nullable(Of Date)

        Public Property IDEspecie() As String


        Public Property ValorBloquear() As System.Nullable(Of Decimal)

        Public Property Prioridad() As System.Nullable(Of Integer)

        Public Property PrioridadGrupo() As System.Nullable(Of Integer)

        Public Property PuedeBloquear() As System.Nullable(Of Boolean)

        Public Property PuedeDesBloquear() As System.Nullable(Of Boolean)

        Public Property DescripcionTipoOferta() As String


    End Class
End Class

#End Region

