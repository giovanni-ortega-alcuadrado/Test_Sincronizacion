Imports System.Reflection
Imports System.Runtime.Serialization
Imports System.Data.Linq.Mapping
Imports System.Data.Linq
Imports A2.OyD.Infraestructura

Partial Public Class OyD_OTCDataContext
    Public Sub New()
        MyBase.New(ObtenerCadenaConexion(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString), mappingSource)
        OnCreated()
    End Sub
    Public Overrides Sub SubmitChanges(failureMode As Data.Linq.ConflictMode)
        Try
            MyBase.SubmitChanges(failureMode)
        Catch ex As Exception
            ManejarError(ex, "OyDOrdenesDataContext", "SubmitChanges")
        End Try
    End Sub

#Region "OTC SEN"
    'Private Sub ActualizarOperacionesSEN(ByVal pstrMetodo As String, ByVal obj As tblImportacionLiqSEN)

    '    Me.uspOyDNet_OTC_Liquidaciones_SEN_Importar(pstrMetodo, obj.strUsuario, obj.dtmFechaImportacion, obj.dtmHoraImportacion, _
    '                                                obj.lngIDOperacion, obj.lngIdComitente, obj.strTipo, obj.strEspecie, _
    '                                                obj.dblCantidad, obj.lngDiasVencimiento, obj.curEquivalente, obj.curTotal, _
    '                                                obj.curPrecio, obj.dtmEmision, obj.dtmVencimiento, obj.dtmLiquidacion, _
    '                                                obj.strTipoNegociacion, obj.strISIN, "", obj.intResultado)
    'End Sub
#End Region
End Class
