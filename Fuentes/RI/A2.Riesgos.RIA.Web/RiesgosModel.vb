Imports System.Data.Linq
Imports System.Reflection
Imports System.Data.SqlClient


Public Class RiesgosModelDataContext

#Region "Consultas"

    Private Sub InsertConsultas(ByVal obj As Consultas)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDConsultas
        Dim p2 As String = obj.strMsgValidacion
        Me.usp_MC_Consultas_Actualizar(
            p1,
            obj.strConsulta,
            obj.strProcedimiento,
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIDConsultas = CInt(p1)
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub UpdateConsultas(ByVal obj As Consultas)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDConsultas
        Dim p2 As String = obj.strMsgValidacion
        Me.usp_MC_Consultas_Actualizar(
            p1,
            obj.strConsulta,
            obj.strProcedimiento,
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIDConsultas = CInt(p1)
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub DeleteConsultas(ByVal obj As Consultas)
        Dim p1 As String = obj.strMsgValidacion
        Me.usp_MC_Consultas_Eliminar(
            CType(obj.intIDConsultas, System.Nullable(Of Integer)),
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

End Class
