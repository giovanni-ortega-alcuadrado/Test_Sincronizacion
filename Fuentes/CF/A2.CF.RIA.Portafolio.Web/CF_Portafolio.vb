Imports System.Configuration
Imports A2.OyD.Infraestructura
''' <summary>
''' Esta clase fue creada para poder extender los metodos new() de la clase oyddatacontext y poder descifrar la cadena de conexión.
''' Cuando se realizan cambios en el diseñador de consultas relacional (.dbml) los metodos new() vuelven a ser automáticamente generados y se presenta un error de que 
''' existen múltiples definiciones con firmas idénticas (‘Public Sub New()’ has multiple definitions with identical signatures).
''' LA SOLUCION es eliminar de la clase oyddatacontext todos los métodos New() y dejar los que se encuentran en esta clase.
''' </summary>
''' <remarks></remarks>
''' 


Partial Public Class CFPortafolioDatacontext
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
            ManejarError(ex, "CFPortafolioDatacontext", "SubmitChanges")
        End Try
    End Sub


    Private Sub InsertCustodia(ByVal obj As CFPortafolio.Custodia)

        ' Esta funcion fue creada para poder extender los metodos insercion de custodia y detalle custodia de la clase oyddatacontext .
        ' Cuando se realizan cambios en el diseñador de consultas relacional (.dbml) las funciones InsertCustodia y InsertDetalleCustodia vuelven a ser automáticamente generados y se presenta un error de que 
        ' existen múltiples definiciones de la funcion 'has multiple definitions with identical signatures').
        ' LA SOLUCION es eliminar de la clase oyddatacontext estas dos funciones y dejar los que se encuentran en esta clase.

        Dim p1 As Integer = obj.IdRecibo
        Dim p2 As Integer = obj.IDCustodia
        Me.uspOyDNet_Bolsa_Custodia_Actualizar(obj.Aprobacion, p1, obj.Comitente, obj.TipoIdentificacion, CType(obj.NroDocumento, System.Nullable(Of Decimal)), obj.Nombre, obj.Telefono1, obj.Direccion, CType(obj.Recibo, System.Nullable(Of Date)), obj.Estado, CType(obj.Fecha_Estado, System.Nullable(Of Date)), obj.ConceptoAnulacion, obj.Notas, obj.NroLote, CType(obj.Elaboracion, System.Nullable(Of Date)), obj.Usuario, p2, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)), obj.UsuarioAprobador)
        obj.IdRecibo = p1
        obj.IDCustodia = p2
        intIdrecibo = p1
    End Sub

    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se agrega parámetro Precio 
    ''' Fecha            : Abril 09/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Abril 09/2013 - Resultado Ok 
    ''' 
    ''' Modificado por   : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Se agrega parámetro strTipoInversion 
    ''' Fecha            : Abril 10/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Abril 10/2014 - Resultado Ok 
    Private Sub InsertDetalleCustodia(ByVal obj As CFPortafolio.DetalleCustodia)
        Dim p1 As Integer = obj.IDDetalleCustodias
        Dim p2 As String = obj.strMsgValidacion
        If intIdrecibo = 0 Then
            IntIdRecibodetalle = obj.IdRecibo
        Else
            IntIdRecibodetalle = intIdrecibo
        End If
        Me.uspOyDNet_Bolsa_DetalleCustodias_Actualizar(obj.Aprobacion, IntIdRecibodetalle, obj.Secuencia, obj.Comitente, obj.IdEspecie, obj.NroTitulo, CType(obj.RentaVariable, System.Nullable(Of Boolean)), obj.IndicadorEconomico, CType(obj.PuntosIndicador, System.Nullable(Of Double)), obj.DiasVencimiento, obj.Modalidad, CType(obj.Emision, System.Nullable(Of Date)), CType(obj.Vencimiento, System.Nullable(Of Date)), CType(obj.Cantidad, System.Nullable(Of Double)), obj.Fondo, CType(obj.TasaInteres, System.Nullable(Of Double)), obj.NroRefFondo, CType(obj.Retencion, System.Nullable(Of Date)), CType(obj.TasaRetencion, System.Nullable(Of Double)), CType(obj.ValorRetencion, System.Nullable(Of Double)), CType(obj.PorcRendimiento, System.Nullable(Of Double)), obj.IdAgenteRetenedor, obj.EstadoActual, CType(obj.ObjVenta, System.Nullable(Of Boolean)), CType(obj.ObjRenovReinv, System.Nullable(Of Boolean)), CType(obj.ObjCobroIntDiv, System.Nullable(Of Boolean)), CType(obj.ObjSuscripcion, System.Nullable(Of Boolean)), CType(obj.ObjCancelacion, System.Nullable(Of Boolean)), obj.Notas, CType(obj.Sellado, System.Nullable(Of Date)), obj.IdCuentaDeceval, obj.ISIN, obj.Fungible, obj.TipoValor, obj.FechasPagoRendimientos, obj.IDDepositoExtranjero, obj.IDCustodio, obj.TitularCustodio, obj.Reinversion, obj.IDLiquidacion, obj.Parcial, obj.TipoLiquidacion, obj.ClaseLiquidacion, CType(obj.Liquidacion, System.Nullable(Of Date)), CType(obj.TotalLiq, System.Nullable(Of Double)), CType(obj.TasaCompraVende, System.Nullable(Of Double)), CType(obj.CumplimientoTitulo, System.Nullable(Of Date)), CType(obj.TasaDescuento, System.Nullable(Of Double)), obj.MotivoBloqueo, obj.NotasBloqueo, obj.EstadoEntrada, obj.EstadoSalida, CType(obj.CargadoArchivo, System.Nullable(Of Boolean)), obj.Usuario, p1, obj.InfoSesion, CType(obj.Precio, System.Nullable(Of Double)), obj.strTipoInversion, CType(Nothing, System.Nullable(Of Byte)), p2, obj.strOrigen, obj.IDMoneda, obj.FechaReclasificacionInversion, obj.strIdReceptor)
        obj.IDDetalleCustodias = p1
        obj.strMsgValidacion = p2
    End Sub

    ''' Modificado por   : Jorge Peña (Alcuadrado S.A.)
    ''' Descripción      : Se agrega este método para poder recupear el mensaje de validación cuando la custodia no se puede modificar.
    ''' Fecha            : Abril 16/2014
    ''' Pruebas CB       : Jorge Peña (Alcuadrado S.A.) - Abril 16/2014 - Resultado Ok 
    Private Sub UpdateDetalleCustodia(ByVal obj As CFPortafolio.DetalleCustodia)
        Dim p1 As Integer = obj.IDDetalleCustodias
        Dim p2 As String = obj.strMsgValidacion
        Me.uspOyDNet_Bolsa_DetalleCustodias_Actualizar(obj.Aprobacion, obj.IdRecibo, obj.Secuencia, obj.Comitente, obj.IdEspecie, obj.NroTitulo, CType(obj.RentaVariable, System.Nullable(Of Boolean)), obj.IndicadorEconomico, CType(obj.PuntosIndicador, System.Nullable(Of Double)), obj.DiasVencimiento, obj.Modalidad, CType(obj.Emision, System.Nullable(Of Date)), CType(obj.Vencimiento, System.Nullable(Of Date)), CType(obj.Cantidad, System.Nullable(Of Double)), obj.Fondo, CType(obj.TasaInteres, System.Nullable(Of Double)), obj.NroRefFondo, CType(obj.Retencion, System.Nullable(Of Date)), CType(obj.TasaRetencion, System.Nullable(Of Double)), CType(obj.ValorRetencion, System.Nullable(Of Double)), CType(obj.PorcRendimiento, System.Nullable(Of Double)), obj.IdAgenteRetenedor, obj.EstadoActual, CType(obj.ObjVenta, System.Nullable(Of Boolean)), CType(obj.ObjRenovReinv, System.Nullable(Of Boolean)), CType(obj.ObjCobroIntDiv, System.Nullable(Of Boolean)), CType(obj.ObjSuscripcion, System.Nullable(Of Boolean)), CType(obj.ObjCancelacion, System.Nullable(Of Boolean)), obj.Notas, CType(obj.Sellado, System.Nullable(Of Date)), obj.IdCuentaDeceval, obj.ISIN, obj.Fungible, obj.TipoValor, obj.FechasPagoRendimientos, obj.IDDepositoExtranjero, obj.IDCustodio, obj.TitularCustodio, obj.Reinversion, obj.IDLiquidacion, obj.Parcial, obj.TipoLiquidacion, obj.ClaseLiquidacion, CType(obj.Liquidacion, System.Nullable(Of Date)), CType(obj.TotalLiq, System.Nullable(Of Double)), CType(obj.TasaCompraVende, System.Nullable(Of Double)), CType(obj.CumplimientoTitulo, System.Nullable(Of Date)), CType(obj.TasaDescuento, System.Nullable(Of Double)), obj.MotivoBloqueo, obj.NotasBloqueo, obj.EstadoEntrada, obj.EstadoSalida, CType(obj.CargadoArchivo, System.Nullable(Of Boolean)), obj.Usuario, p1, obj.InfoSesion, CType(obj.Precio, System.Nullable(Of Double)), obj.strTipoInversion, CType(Nothing, System.Nullable(Of Byte)), p2, obj.strOrigen, obj.IDMoneda, obj.FechaReclasificacionInversion, obj.strIdReceptor)
        obj.IDDetalleCustodias = p1
        obj.strMsgValidacion = p2
    End Sub

    Private Sub UpdateCustodiasObtenerTitulos(ByVal obj As CFPortafolio.CustodiasObtenerTitulos)
        Me.uspOyDNet_Custodias_ActualizarEstado(obj.IdRecibo, obj.Secuencia, obj.Estado, obj.NombreConsecutivo, obj.TipoDcto, obj.Posicion, obj.Usuario, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)))
    End Sub

End Class
