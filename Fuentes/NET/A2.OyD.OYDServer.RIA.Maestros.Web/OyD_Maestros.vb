Imports System.Configuration
Imports A2.OyD.Infraestructura

''' <summary>
''' Esta clase fue creada para poder extender los metodos new() de la clase oyddatacontext y poder descifrar la cadena de conexión.
''' Cuando se realizan cambios en el diseñador de consultas relacional (.dbml) los metodos new() vuelven a ser automáticamente generados y se presenta un error de que 
''' existen múltiples definiciones con firmas idénticas (‘Public Sub New()’ has multiple definitions with identical signatures).
''' LA SOLUCION es eliminar de la clase oyddatacontext todos los métodos New() y dejar los que se encuentran en esta clase.
''' </summary>
''' <remarks></remarks>
Partial Public Class OyDDataContext
    Dim idEmpleado As Integer = 0
    Dim idEmpleadoDetalle As Integer = 0

    Public Sub New()
        MyBase.New(ObtenerCadenaConexion(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString), mappingSource)
        OnCreated()
    End Sub
    Public Overrides Sub SubmitChanges(failureMode As Data.Linq.ConflictMode)
        Try
            MyBase.SubmitChanges(failureMode)
        Catch ex As Exception
            ManejarError(ex, "OyDDataContext", "SubmitChanges")
        End Try
    End Sub

    Private Sub InsertEmpleado(ByVal obj As Empleado)
        Dim p1 As Integer = obj.IDEmpleado
        Me.uspOyDNet_Maestros_Empleados_Actualizar(p1, obj.Nombre, obj.NroDocumento, obj.IDReceptor, obj.Login, obj.IDCargo, obj.AccesoOperadorBolsa, obj.Usuario, CType(obj.Activo, System.Nullable(Of Boolean)), obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)), obj.strEmail, obj.Maquinas, obj.TipoIdentificacion)
        obj.IDEmpleado = p1
        idEmpleado = p1
    End Sub

    Private Sub InsertEmpleadoSistema(ByVal obj As EmpleadoSistema)
        Dim p1 As Integer = obj.ID
        If idEmpleado = 0 Then
            idEmpleadoDetalle = obj.IDEmpleado
        Else
            idEmpleadoDetalle = idEmpleado
        End If
        Me.uspOyDNet_EmpleadosSistemas_Actualizar(p1, idEmpleadoDetalle, obj.Sistema, obj.CodigoMapeo, obj.Valor, obj.Usuario, CType(Nothing, String), CType(Nothing, System.Nullable(Of Byte)))
        obj.ID = p1
    End Sub

    Private Sub InsertTiposCuentasRecaudadoras(ByVal obj As TiposCuentasRecaudadoras)
        Dim p1 As Integer = obj.IDTiposCuentasRecaudadoras
        Dim p2 As String = obj.strMsgValidacion
        Me.uspOyDNet_Maestros_TiposCuentasRecaudadoras_Actualizar(p1, obj.TipoCuenta, obj.TipoReciboCaja, obj.RegistrarCheques, obj.ManejoComisiones, obj.ManejoTraslado, CType(obj.DetalleFecha, System.Nullable(Of Boolean)), CType(obj.DetalleReferencia, System.Nullable(Of Boolean)), CType(obj.DetalleNombreTransaccion, System.Nullable(Of Boolean)), obj.DetallePersonalizado, CType(obj.CuentaCentralizadora, System.Nullable(Of Boolean)), obj.Usuario, obj.InfoSession, CType(Nothing, System.Nullable(Of Byte)), CType(Nothing, String))
        obj.IDTiposCuentasRecaudadoras = p1
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains("EXITOSO") Then
                Throw New Exception(p2, New Exception("validacionA2"))
            End If
        End If
    End Sub

    Private Sub UpdateTiposCuentasRecaudadoras(ByVal obj As TiposCuentasRecaudadoras)
        Dim p1 As Integer = obj.IDTiposCuentasRecaudadoras
        Dim p2 As String = obj.strMsgValidacion
        Me.uspOyDNet_Maestros_TiposCuentasRecaudadoras_Actualizar(p1, obj.TipoCuenta, obj.TipoReciboCaja, obj.RegistrarCheques, obj.ManejoComisiones, obj.ManejoTraslado, CType(obj.DetalleFecha, System.Nullable(Of Boolean)), CType(obj.DetalleReferencia, System.Nullable(Of Boolean)), CType(obj.DetalleNombreTransaccion, System.Nullable(Of Boolean)), obj.DetallePersonalizado, CType(obj.CuentaCentralizadora, System.Nullable(Of Boolean)), obj.Usuario, obj.InfoSession, CType(Nothing, System.Nullable(Of Byte)), CType(Nothing, String))
        obj.IDTiposCuentasRecaudadoras = p1
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains("EXITOSO") Then
                Throw New Exception(p2, New Exception("validacionA2"))
            End If
        End If
    End Sub

    Private Sub DeleteTiposCuentasRecaudadoras(ByVal obj As TiposCuentasRecaudadoras)
        Dim p1 As System.Nullable(Of Integer) = obj.IDTiposCuentasRecaudadoras
        Dim p2 As String = obj.strMsgValidacion
        Me.uspOyDNet_Maestros_TiposCuentasRecaudadoras_Eliminar(p1, obj.Usuario, obj.InfoSession, CType(Nothing, System.Nullable(Of Byte)), p2)
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains("EXITOSO") Then
                Throw New Exception(p2, New Exception("validacionA2"))
            End If
        End If
    End Sub

    Private Sub InsertCuentasBancariasPorConcepto(ByVal obj As CuentasBancariasPorConcepto)
        Dim p1 As Integer = obj.ID
        Dim p2 As String = obj.strMsgValidacion
        Me.usp_CuentasbancariasPorConcepto_Actualizar(p1, obj.IdCuentaBancaria, obj.IDCodigoBanco, obj.IdConcepto, obj.IdConceptoAnterior, obj.CuentaContable, obj.Accion, obj.Usuario, obj.InfoSession, CType(Nothing, System.Nullable(Of Byte)), CType(Nothing, String))
        obj.ID = p1
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains("EXITOSO") Then
                Throw New Exception(p2, New Exception("validacionA2"))
            End If
        End If
    End Sub

    Private Sub UpdateCuentasBancariasPorConcepto(ByVal obj As CuentasBancariasPorConcepto)
        Dim p1 As Integer = obj.ID
        Dim p2 As String = obj.strMsgValidacion
        Me.usp_CuentasbancariasPorConcepto_Actualizar(p1, obj.IdCuentaBancaria, obj.IDCodigoBanco, obj.IdConcepto, obj.IdConceptoAnterior, obj.CuentaContable, obj.Accion, obj.Usuario, obj.InfoSession, CType(Nothing, System.Nullable(Of Byte)), CType(Nothing, String))
        obj.ID = p1
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains("EXITOSO") Then
                Throw New Exception(p2, New Exception("validacionA2"))
            End If
        End If
    End Sub

    Private Sub DeleteCuentasBancariasPorConcepto(ByVal obj As CuentasBancariasPorConcepto)
        Dim p1 As System.Nullable(Of Integer) = obj.ID

        Me.usp_CuentasbancariasPorConcepto_RETIRAR(p1, obj.IdCuentaBancaria, obj.IDCodigoBanco, obj.IdConcepto, obj.InfoSession, CType(Nothing, System.Nullable(Of Byte)))

    End Sub

    Private Sub InsertCodigosAjustes(ByVal obj As CodigosAjustes)
        Dim p1 As Integer = obj.ID
        Me.usp_CodigosAjustes_Actualizar(p1, obj.COD_TRANSACCION, obj.DESCRIPCION, obj.IdConcepto, obj.IdConceptoAnterior, obj.Owner, obj.Accion, obj.Usuario, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.ID = p1
    End Sub

    Private Sub UpdateCodigosAjustes(ByVal obj As CodigosAjustes)
        Dim p1 As Integer = obj.ID
        Me.usp_CodigosAjustes_Actualizar(p1, obj.COD_TRANSACCION, obj.DESCRIPCION, obj.IdConcepto, obj.IdConceptoAnterior, obj.Owner, obj.Accion, obj.Usuario, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.ID = p1
    End Sub

    Private Sub DeleteCodigosAjustes(ByVal obj As CodigosAjustes)
        Dim p1 As System.Nullable(Of Integer) = obj.ID

        Me.usp_CodigosAjustes_Eliminar(p1, obj.COD_TRANSACCION, obj.IdConcepto, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))

    End Sub

    Private Sub InsertCodigosTransaccion(ByVal obj As CodigosTransaccion)
        Dim p1 As System.Nullable(Of Integer) = obj.ID

        Me.uspOyDNet_CodigosTransaccion_Actualizar(p1, obj.Accion, obj.Codigo, obj.Transaccion, obj.DetalleRC, obj.TipoTransaccion, obj.Usuario, obj.infosesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.ID = p1
    End Sub

    Private Sub UpdateCodigosTransaccion(ByVal obj As CodigosTransaccion)
        Dim p1 As Integer = obj.ID
        Me.uspOyDNet_CodigosTransaccion_Actualizar(p1, obj.Accion, obj.Codigo, obj.Transaccion, obj.DetalleRC, obj.TipoTransaccion, obj.Usuario, obj.infosesion, CType(Nothing, System.Nullable(Of Byte)))
        obj.ID = p1
    End Sub

    Private Sub DeleteCodigosTransaccion(ByVal obj As CodigosTransaccion)
        Dim p1 As Integer = obj.ID
        Me.uspOyDNet_CodigosTransaccion_Eliminar(p1, obj.Codigo, obj.Usuario, CType(Nothing, String), CType(Nothing, System.Nullable(Of Byte)))
        obj.ID = p1
    End Sub

End Class