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

Namespace OYDPLUSUtilidades

    <MetadataTypeAttribute(GetType(AuditoriaTabla.AuditoriaTablaMetadata))> _
    Partial Public Class AuditoriaTabla
        Friend NotInheritable Class AuditoriaTablaMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="IdAuditoria", Description:="IdAuditoria")> _
            Public Property IdAuditoria As Integer

            <Display(Name:="Tabla para Auditar:", Description:="Nombre Tabla")> _
            Public Property NombreTabla As String

            <Display(Name:="Log Ingreso:", Description:="Ingreso")> _
            Public Property Ingreso As Boolean

            <Display(Name:="Log Modificación:", Description:="Modificación")> _
            Public Property Modificacion As Boolean

            <Display(Name:="Log Eliminación:", Description:="Eliminación")> _
            Public Property Eliminacion As Boolean

            <Display(Name:="Usuario", Description:="Usuario")> _
            Public Property Usuario As String

        End Class
    End Class

#Region "OYDPLUS"

    '**************************************************************************************************
    <MetadataTypeAttribute(GetType(tblSaldosCliente.tblSaldosClienteMetadata))> _
    Partial Public Class tblSaldosCliente
        Friend NotInheritable Class tblSaldosClienteMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="ID", Description:="ID")> _
            Public Property ID As Integer

            <Display(Name:="Código OYD", Description:="Código OYD")> _
            Public Property CodigoOYD As String

            <Display(Name:="Descripción", Description:="Descripción")> _
            Public Property Descripcion As String

            <Display(Name:="Valor", Description:="Valor")> _
            Public Property Valor As Double

            <Display(Name:="Activo", Description:="Activo")> _
            Public Property Usado As Boolean

        End Class
    End Class

#End Region

End Namespace