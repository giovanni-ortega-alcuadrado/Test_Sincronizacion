Imports System.Configuration
Imports A2.OyD.Infraestructura

''' <summary>
''' Esta clase fue creada para poder extender los metodos new() de la clase oyddatacontext y poder descifrar la cadena de conexión.
''' Cuando se realizan cambios en el diseñador de consultas relacional (.dbml) los metodos new() vuelven a ser automáticamente generados y se presenta un error de que 
''' existen múltiples definiciones con firmas idénticas (‘Public Sub New()’ has multiple definitions with identical signatures).
''' LA SOLUCION es eliminar de la clase oyddatacontext todos los métodos New() y dejar los que se encuentran en esta clase.
''' </summary>
''' <remarks></remarks>
Partial Public Class OyDPLUSMaestrosDataContext
    Public Sub New()
        MyBase.New(ObtenerCadenaConexion(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString), mappingSource)
        OnCreated()
    End Sub
    Public Overrides Sub SubmitChanges(failureMode As Data.Linq.ConflictMode)
        Try
            MyBase.SubmitChanges(failureMode)
        Catch ex As Exception
            ManejarError(ex, "OyDPLUSMaestrosDataContext", "SubmitChanges")
        End Try
    End Sub

    Private Sub InsertPreciosTick(ByVal obj As OyDPLUSMaestros.PreciosTick)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDPreciosTick
        Dim p2 As String = obj.strMsgValidacion
        Me.uspOyDNet_Maestros_PreciosTick_Actualizar(p1, CType(obj.dblPrecioInicial, System.Nullable(Of Double)), CType(obj.dblPrecioFinal, System.Nullable(Of Double)), CType(obj.dblMultiplos, System.Nullable(Of Double)), obj.strUsuario, CType(Nothing, String), CType(Nothing, System.Nullable(Of Byte)), p2)
        obj.intIDPreciosTick = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub UpdatePreciosTick(ByVal obj As OyDPLUSMaestros.PreciosTick)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDPreciosTick
        Dim p2 As String = obj.strMsgValidacion
        Me.uspOyDNet_Maestros_PreciosTick_Actualizar(p1, CType(obj.dblPrecioInicial, System.Nullable(Of Double)), CType(obj.dblPrecioFinal, System.Nullable(Of Double)), CType(obj.dblMultiplos, System.Nullable(Of Double)), obj.strUsuario, CType(Nothing, String), CType(Nothing, System.Nullable(Of Byte)), p2)
        obj.intIDPreciosTick = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

End Class