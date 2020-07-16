
Option Compare Binary
Option Infer On
Option Strict On
Option Explicit On

Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.Server
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Linq
Imports OpenRiaServices.DomainServices.EntityFramework

<MetadataTypeAttribute(GetType(tblParametrizacionTributaria.tblParametrizacionTributariaMetadata))>
Partial Public Class tblParametrizacionTributaria
    Friend NotInheritable Class tblParametrizacionTributariaMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(AllowEmptyStrings:=True)>
        Public Property strNombre As String

        <Required(AllowEmptyStrings:=True)>
        Public Property strModulo As String

        <Required(AllowEmptyStrings:=True)>
        Public Property strFuncionalidad As String

    End Class
End Class

<MetadataTypeAttribute(GetType(tblPais.tblPaisMetadata))>
Partial Public Class tblPais
    Friend NotInheritable Class tblPaisMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(AllowEmptyStrings:=True)>
        Public Property strNombre As String

        <Required(AllowEmptyStrings:=True)>
        Public Property strCodigoISOAlfa2 As String

        <Required(AllowEmptyStrings:=True)>
        Public Property strCodigoISOAlfa3 As String

        <Required(AllowEmptyStrings:=True)>
        Public Property strCodigoISONumerico As String

    End Class
End Class

<MetadataTypeAttribute(GetType(tblPais_Estado.tblPais_EstadoMetadata))>
Partial Public Class tblPais_Estado
    Friend NotInheritable Class tblPais_EstadoMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(AllowEmptyStrings:=True)>
        Public Property strNombre As String

        <Required(AllowEmptyStrings:=True)>
        Public Property strCodigo As String

    End Class
End Class

<MetadataTypeAttribute(GetType(tblPais_Moneda.tblPais_MonedaMetadata))>
Partial Public Class tblPais_Moneda
    Friend NotInheritable Class tblPais_MonedaMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(AllowEmptyStrings:=True)>
        Public Property strCodigoMoneda As String

    End Class
End Class