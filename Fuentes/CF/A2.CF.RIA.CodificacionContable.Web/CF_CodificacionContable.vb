Imports System.Data.Linq
Imports System.Reflection
Imports A2.OyD.OYDServer.RIA.Web.CFCodificacionContable
Imports A2.OyD.Infraestructura

Partial Public Class CodificacionContableDBML
    Public Sub New()
        MyBase.New(ObtenerCadenaConexion(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString), mappingSource)
        OnCreated()
    End Sub
    Public Overrides Sub SubmitChanges(failureMode As Data.Linq.ConflictMode)
        Try
            MyBase.SubmitChanges(failureMode)
        Catch ex As Exception
            ManejarError(ex, "CodificacionContableDBML", "SubmitChanges")
        End Try
    End Sub

#Region "ConfiguracionContableConcepto"
    Private Sub InsertConfiguracionContableConcepto(ByVal obj As ConfiguracionContableConcepto)
        Dim p1 As Integer = obj.intID
        Dim p2 As String = obj.strMsgValidacion
        Me.uspOyDNet_Maestros_CodificacionContableConcepto_Actualizar(
            p1,
            obj.strNormaContable,
            obj.strConcepto,
            obj.strCuentaContableDBPositiva,
            obj.strCuentaContableDBNegativa,
            obj.strCuentaContableCRPositiva,
            obj.strCuentaContableCRNegativa,
            obj.strTipoTitulos,
            CType(obj.intTipoProducto, System.Nullable(Of Integer)),
            obj.strTipoInversion,
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intID = p1
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub UpdateConfiguracionContableConcepto(ByVal obj As ConfiguracionContableConcepto)
        Dim p1 As System.Nullable(Of Integer) = obj.intID
        Dim p2 As String = obj.strMsgValidacion
        Me.uspOyDNet_Maestros_CodificacionContableConcepto_Actualizar(
            p1,
            obj.strNormaContable,
            obj.strConcepto,
            obj.strCuentaContableDBPositiva,
            obj.strCuentaContableDBNegativa,
            obj.strCuentaContableCRPositiva,
            obj.strCuentaContableCRNegativa,
            obj.strTipoTitulos,
            CType(obj.intTipoProducto, System.Nullable(Of Integer)),
            obj.strTipoInversion,
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intID = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub DeleteConfiguracionContableConcepto(ByVal obj As ConfiguracionContableConcepto)
        Dim p1 As String = obj.strMsgValidacion
        Me.uspOyDNet_Maestros_CodificacionContableConcepto_Eliminar(
            CType(obj.intID, System.Nullable(Of Integer)),
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p1)
        obj.strMsgValidacion = p1
        If Not String.IsNullOrEmpty(p1) Then
            Throw New Exception(p1, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
        End If
    End Sub
#End Region

#Region "CodificacionContable"

    <Global.System.Data.Linq.Mapping.FunctionAttribute()> _
    Public Function uspOyDNet_Maestros_CodificacionContable_Actualizar( _
                <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Int NOT NULL")> ByRef plngID As System.Nullable(Of Integer), _
                <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Varchar(2) NOT NULL")> ByVal pstrNormaContable As String, _
                <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Int NOT NULL")> ByVal plngModulo As System.Nullable(Of Integer), _
                <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Int NOT NULL")> ByVal plngNegocio As System.Nullable(Of Integer), _
                <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Int NOT NULL")> ByVal plngOperacion As System.Nullable(Of Integer), _
                <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Int NOT NULL")> ByVal plngDuracion As System.Nullable(Of Integer), _
                <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Int NOT NULL")> ByVal plngTipoFecha As System.Nullable(Of Integer), _
                <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="INT NOT NULL")> ByVal pintTipoProducto As System.Nullable(Of Integer), _
                <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Varchar(80) NOT NULL")> ByVal pstrTipoInversion As String, _
                <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Int NOT NULL")> ByVal plngIdMoneda As System.Nullable(Of Integer), _
                <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Datetime NOT NULL")> ByVal pdtmFechaInicio As System.Nullable(Of Date), _
                <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Int NOT NULL")> ByVal plogActivo As System.Nullable(Of Integer), _
                <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="Text NOT NULL")> ByVal pxmlDetalleGrid As String, _
                <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(60)")> ByVal pstrUsuario As String, _
                <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(1000)")> ByVal pstrInfoSesion As String, _
                <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="TinyInt")> ByVal pintErrorPersonalizado As System.Nullable(Of Byte), _
                <Global.System.Data.Linq.Mapping.ParameterAttribute(DbType:="VarChar(8000)")> ByRef pstrMsgValidacion As String) As Integer

        Dim result As IExecuteResult = Me.ExecuteMethodCall(Me, CType(MethodInfo.GetCurrentMethod, MethodInfo), plngID, pstrNormaContable, plngModulo, plngNegocio, plngOperacion, plngDuracion, plngTipoFecha, pintTipoProducto, pstrTipoInversion, plngIdMoneda, pdtmFechaInicio, plogActivo, pxmlDetalleGrid, pstrUsuario, pstrInfoSesion, pintErrorPersonalizado, pstrMsgValidacion)
        plngID = CType(result.GetParameterValue(0), System.Nullable(Of Integer))
        pstrMsgValidacion = CType(result.GetParameterValue(16), String)

        'If Not String.IsNullOrEmpty(pstrMsgValidacion) Then
        '    If Not pstrMsgValidacion.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
        '        Throw New Exception(pstrMsgValidacion, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
        '    End If
        'End If

        Return CType(result.ReturnValue, Integer)

    End Function

    Private Sub DeleteCodificacionContable(ByVal obj As CodificacionContable)
        Dim p1 As String = obj.strMsgValidacion
        Me.uspOyDNet_Maestros_CodificacionContable_Eliminar(
            CType(obj.intID, System.Nullable(Of Integer)),
            obj.strUsuario,
            obj.InfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p1)
        obj.strMsgValidacion = p1
        If Not String.IsNullOrEmpty(p1) Then
            Throw New Exception(p1, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
        End If
    End Sub

#End Region

End Class
