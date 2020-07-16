Imports System.Configuration
Imports A2.OyD.Infraestructura
Imports A2.OyD.OYDServer.RIA.Web.CFTitulosNet

''' <summary>
''' Esta clase fue creada para poder extender los metodos new() de la clase oyddatacontext y poder descifrar la cadena de conexión.
''' Cuando se realizan cambios en el diseñador de consultas relacional (.dbml) los metodos new() vuelven a ser automáticamente generados y se presenta un error de que 
''' existen múltiples definiciones con firmas idénticas (‘Public Sub New()’ has multiple definitions with identical signatures).
''' LA SOLUCION es eliminar de la clase oyddatacontext todos los métodos New() y dejar los que se encuentran en esta clase.
''' </summary>
''' <remarks></remarks>
''' 

Partial Public Class CF_TitulosNetDataContext
    Dim intIdrecibo As Integer
    Dim IntIdRecibodetalle As Integer
    Public Sub New()
        MyBase.New(ObtenerCadenaConexion(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString), mappingSource)
        OnCreated()
    End Sub
    Public Overrides Sub SubmitChanges(failureMode As Data.Linq.ConflictMode)
        Try
            MyBase.SubmitChanges(failureMode)
        Catch ex As Exception
            ManejarError(ex, "CF_TitulosNetDataContext", "SubmitChanges")
        End Try
    End Sub

#Region "ConceptosRetencion"

    Private Sub InsertConceptoRetencion(ByVal obj As ConceptoRetencion)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDConceptoRetencion
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_ConceptosRetencion_Actualizar(
            p1,
            obj.strCodigo,
            obj.strDescripcion,
            CType(obj.dblPorcentajeRetencion, System.Nullable(Of Double)),
            obj.logGravado,
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIDConceptoRetencion = p1
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub UpdateConceptoRetencion(ByVal obj As ConceptoRetencion)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDConceptoRetencion
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_ConceptosRetencion_Actualizar(
            p1,
            obj.strCodigo,
            obj.strDescripcion,
            CType(obj.dblPorcentajeRetencion, System.Nullable(Of Double)),
            obj.logGravado,
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIDConceptoRetencion = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub DeleteConceptoRetencion(ByVal obj As ConceptoRetencion)
        Dim p1 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_ConceptosRetencion_Eliminar(
            CType(obj.intIDConceptoRetencion, System.Nullable(Of Integer)),
            obj.strCodigo,
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
