
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

<MetadataTypeAttribute(GetType(tblPreOrdenes.tblPreOrdenesMetadata))>
Partial Public Class tblPreOrdenes
    Friend NotInheritable Class tblPreOrdenesMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(AllowEmptyStrings:=True)>
        Public Property strTipoPreOrden As String

        <Required(AllowEmptyStrings:=True)>
        Public Property strTipoInversion As String

        <Required(AllowEmptyStrings:=True)>
        Public Property strInstrumento As String

        <Required(AllowEmptyStrings:=True)>
        Public Property strIntencion As String

        <Required(AllowEmptyStrings:=True)>
        Public Property strInstrucciones As String

    End Class
End Class

<MetadataTypeAttribute(GetType(tblPreOrdenes_Portafolio.tblPreOrdenes_PortafolioMetadata))>
Partial Public Class tblPreOrdenes_Portafolio
    Friend NotInheritable Class tblPreOrdenes_PortafolioMetadata
        Private Sub New()
            MyBase.New()
        End Sub

        <Required(AllowEmptyStrings:=True)>
        Public Property strNroTitulo As String

        <Required(AllowEmptyStrings:=True)>
        Public Property strInstrumento As String

    End Class
End Class
