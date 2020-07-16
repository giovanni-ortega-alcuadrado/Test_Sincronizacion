
Option Compare Binary
Option Infer On
Option Strict On
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Linq
Imports System.ServiceModel.DomainServices.Hosting
Imports System.ServiceModel.DomainServices.Server


'The MetadataTypeAttribute identifies ConsultasConfiguracionMetadata as the class
' that carries additional metadata for the ConsultasConfiguracion class.
<MetadataTypeAttribute(GetType(ConsultasConfiguracion.ConsultasConfiguracionMetadata))>  _
Partial Public Class ConsultasConfiguracion
    
    'This class allows you to attach custom attributes to properties
    ' of the ConsultasConfiguracion class.
    '
    'For example, the following marks the Xyz property as a
    ' required property and specifies the format for valid values:
    '    <Required()>
    '    <RegularExpression("[A-Z][A-Za-z0-9]*")>
    '    <StringLength(32)>
    '    Public Property Xyz As String
    Friend NotInheritable Class ConsultasConfiguracionMetadata
        
        'Metadata classes are not meant to be instantiated.
        Private Sub New()
            MyBase.New
        End Sub
        
        Public Property ColumnasResultado As String
        
        Public Property Fecha As DateTime
        
        Public Property ID As Integer
        
        Public Property Metadata As String
        
        Public Property Metodo As String
        
        Public Property Periodicidad As Byte
        
        Public Property Procedimiento As String
        
        Public Property Usuario As String
    End Class
End Class


'The MetadataTypeAttribute identifies ConsultasConfiguracionMetadata as the class
' that carries additional metadata for the ConsultasConfiguracion class.
<MetadataTypeAttribute(GetType(AlertasConfiguracion.AlertasConfiguracionMetadata))> _
Partial Public Class AlertasConfiguracion

    'This class allows you to attach custom attributes to properties
    ' of the ConsultasConfiguracion class.
    '
    'For example, the following marks the Xyz property as a
    ' required property and specifies the format for valid values:
    '    <Required()>
    '    <RegularExpression("[A-Z][A-Za-z0-9]*")>
    '    <StringLength(32)>
    '    Public Property Xyz As String
    Friend NotInheritable Class AlertasConfiguracionMetadata

        'Metadata classes are not meant to be instantiated.
        Private Sub New()
            MyBase.New()
        End Sub

        Public Property IDAlertas As Integer

        Public Property Metodo As String

        Public Property Consulta As String

        Public Property ValorAnterior As String

        Public Property ValorNuevo As String

        Public Property TipoAlerta As String

        Public Property Destinatarios As String

        Public Property Alerta As String

        Public Property Fecha As DateTime

        Public Property Usuario As String
    End Class
End Class


'The MetadataTypeAttribute identifies BotonesXToolbarConfiguracionMetadata as the class
' that carries additional metadata for the BotonesXToolbarConfiguracion class.
<MetadataTypeAttribute(GetType(ToolbarsPorAplicacionConfiguracion.ToolbarsPorAplicacionConfiguracionMetadata))> _
Partial Public Class ToolbarsPorAplicacionConfiguracion

    'This class allows you to attach custom attributes to properties
    ' of the ConsultasConfiguracion class.
    '
    'For example, the following marks the Xyz property as a
    ' required property and specifies the format for valid values:
    '    <Required()>
    '    <RegularExpression("[A-Z][A-Za-z0-9]*")>
    '    <StringLength(32)>
    '    Public Property Xyz As String
    Friend NotInheritable Class ToolbarsPorAplicacionConfiguracionMetadata

        'Metadata classes are not meant to be instantiated.
        Private Sub New()
            MyBase.New()
        End Sub

        Public Property lngId As Integer = 0

        Public Property strNombreBoton As String

        Public Property strToolTip As String

    End Class
End Class
