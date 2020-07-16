Imports System.Data.Linq
Imports A2.OyD.Infraestructura

Partial Public Class OyD_ImportacionesDataContext
    Public Sub New()
        MyBase.New(ObtenerCadenaConexion(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString), mappingSource)
        OnCreated()
    End Sub
    Public Overrides Sub SubmitChanges(failureMode As Data.Linq.ConflictMode)
        Try
            MyBase.SubmitChanges(failureMode)
        Catch ex As Exception
            ManejarError(ex, "OyD_ImportacionesDataContext", "SubmitChanges")
        End Try
    End Sub

#Region "ChequesImportacionOyD+"
    Private Sub InsertclsChequesOyDPlus(ByVal obj As OyDImportaciones.clsChequesOyDPlus)

    End Sub

    Private Sub UpdateclsChequesOyDPlus(ByVal obj As OyDImportaciones.clsChequesOyDPlus)

    End Sub

    Private Sub DeleteclsChequesOyDPlus(ByVal obj As OyDImportaciones.clsChequesOyDPlus)

    End Sub
#End Region
#Region "TransferenciaImportacionOyD+"
    Private Sub InsertclsTransferenciaOyDPlus(ByVal obj As OyDImportaciones.clsTransferenciaOyDPlus)

    End Sub

    Private Sub UpdateclsTransferenciaOyDPlus(ByVal obj As OyDImportaciones.clsTransferenciaOyDPlus)

    End Sub

    Private Sub DeleteclsTransferenciaOyDPlus(ByVal obj As OyDImportaciones.clsTransferenciaOyDPlus)

    End Sub
#End Region

    Private Sub UpdateArchivoOrdenesLeo(ByVal obj As OyDImportaciones.ArchivoOrdenesLeo)
        Me.usptmpImportarOrdenesLeo(CType(obj.intID, System.Nullable(Of Integer)), CType(obj.id, System.Nullable(Of Integer)), CType(obj.idProceso, System.Nullable(Of Double)), CType(obj.Clase, System.Nullable(Of Char)), obj.Cliente, obj.NombreCliente, obj.Ordenante, CType(obj.Tipo, System.Nullable(Of Char)), obj.Usuario, obj.Cantidad, CType(obj.Deposito, System.Nullable(Of Char)), obj.descDeposito, obj.CuentaDeposito, CType(obj.lngidCuentaDeceval, System.Nullable(Of Integer)), CType(obj.TipoClasificacion, System.Nullable(Of Integer)), obj.DescTipoClasificacion, obj.ObjetoClasificacion, obj.Clasificacion, obj.Especie, obj.NombreEspecie, CType(obj.FechaIngreso, System.Nullable(Of Date)), CType(obj.FechaVigencia, System.Nullable(Of Date)), obj.FechaEmision, obj.FechaVencimiento, obj.Receptor, obj.Modalidad, obj.TasaFacial, obj.TLimite, obj.DescTLimite, obj.CondNegociacion, obj.DescCondNegociacion, obj.FormaPago, obj.DescFormaPago, obj.TipoInversion, obj.DescTipoInversion, obj.Ejecucion, obj.DescEjecucion, obj.Duracion, obj.DescDuracion, obj.ReceptorLeo, obj.CanalLeo, obj.DescCanalLeo, obj.MedioVerificable, obj.DescMedioVerificable, obj.Comision, obj.strUsuarioArchivo, CType(obj.bitGenerarOrden, System.Nullable(Of Boolean)))
    End Sub

End Class



