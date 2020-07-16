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

Namespace OYDUtilidades
    <MetadataTypeAttribute(GetType(DT.DTMetadata))> _
    Partial Public Class DT
        Friend NotInheritable Class DTMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            <Display(Name:="IDComisionista", Description:="IDComisionista")> _
            Public Property IDComisionista As Integer

            <Display(Name:="IDSucComisionista", Description:="IDSucComisionista")> _
            Public Property IDSucComisionista As Integer

            <Display(Name:="DTS", Description:="IDDTS")> _
            Public Property DTS As Integer

            <Display(Name:="Descripcion", Description:="Descripcion")> _
            Public Property Descripcion As String

            <Display(Name:="NomSP", Description:="NomSP")> _
            Public Property NomSP As String

            <Display(Name:="Actualizacion", Description:="Actualizacion")> _
            Public Property Actualizacion As DateTime

            <Display(Name:="Usuario", Description:="Usuario")> _
            Public Property Usuario As String

            <Display(Name:="IDDTS", Description:="IDDTS")> _
            Public Property IDDTS As Integer

        End Class
    End Class

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


#Region "Control de Programaciones"
  
    <MetadataTypeAttribute(GetType(tbl_Programacione.tbl_ProgramacioneMetadata))> _
    Partial Public Class tbl_Programacione
        Friend NotInheritable Class tbl_ProgramacioneMetadata

            'No se van a crear instancias de las clases de metadatos.
            Private Sub New()
                MyBase.New()
            End Sub

            Public Property dtmFechaInicio As DateTime

            Public Property dtmHoraInicio As Binary

            Public Property dtmIntervalo_FechaFinalizacion As Nullable(Of DateTime)

            Public Property dtmIntervalo_FechaInicio As Nullable(Of DateTime)

            Public Property intDia As Nullable(Of Integer)

            Public Property intIdProgramacion As Integer

            Public Property intIntervalo_TotalRepeticiones As Nullable(Of Integer)

            Public Property intOrdinal As Nullable(Of Integer)

            Public Property intRecurrencia As Nullable(Of Integer)

            Public Property intRepeticion As Integer

            Public Property intTipoProgramacion As Integer

            Public Property logEjecutaDia As Nullable(Of Boolean)

            Public Property logIntervalo_ConFechaFinalizacion As Nullable(Of Boolean)

            Public Property logIntervalo_ConRepeticiones As Nullable(Of Boolean)

            Public Property logIntervalo_SinFechaFinalizacion As Nullable(Of Boolean)

            Public Property logOrdinal As Nullable(Of Boolean)

            Public Property strDiasEjecucion As String

            Public Property tbl_ProgramacionesDetalles As EntitySet(Of tbl_ProgramacionesDetalle)
        End Class
    End Class

    <MetadataTypeAttribute(GetType(tbl_ProgramacionesDetalle.tbl_ProgramacionesDetalleMetadata))> _
     Partial Public Class tbl_ProgramacionesDetalle
        Friend NotInheritable Class tbl_ProgramacionesDetalleMetadata
            Private Sub New()
                MyBase.New()
            End Sub

            Public Property dtmFechaProgramacion As DateTime

            Public Property intIdDetalleProgramacion As Integer

            Public Property intIdProgramacion As Integer

            Public Property tbl_Programacione As tbl_Programacione
        End Class
    End Class
#End Region

End Namespace