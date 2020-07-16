Imports System.Configuration
Imports A2.OyD.OYDServer.RIA.Web.CFEspecies
Imports A2.OyD.Infraestructura
''' <summary>
''' Esta clase fue creada para poder extender los metodos new() de la clase oyddatacontext y poder descifrar la cadena de conexión.
''' Cuando se realizan cambios en el diseñador de consultas relacional (.dbml) los metodos new() vuelven a ser automáticamente generados y se presenta un error de que 
''' existen múltiples definiciones con firmas idénticas (‘Public Sub New()’ has multiple definitions with identical signatures).
''' LA SOLUCION es eliminar de la clase oyddatacontext todos los métodos New() y dejar los que se encuentran en esta clase.
''' </summary>
''' <remarks></remarks>
''' 

Partial Public Class CF_EspeciesDataContext
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
            ManejarError(ex, "CF_EspeciesDataContext", "SubmitChanges")
        End Try
    End Sub

    Private Sub InsertEspeci(ByVal obj As Especi)
        Dim p1 As Integer = obj.IDEspecies
        Me.uspOyDNet_Maestros_Especies_Actualizar(obj.Id, obj.Nombre, CType(obj.EsAccion, System.Nullable(Of Boolean)), obj.IDClase, obj.IDTarifa, obj.IDGrupo, obj.IDSubGrupo, CType(obj.VlrNominal, System.Nullable(Of Decimal)), obj.Notas, obj.IdEmisor, obj.IDAdmonEmision, obj.LeyCirculacion, CType(obj.Emision, System.Nullable(Of Date)), CType(obj.Vencimiento, System.Nullable(Of Date)), obj.Modalidad, CType(obj.TasaInicial, System.Nullable(Of Double)), CType(obj.TasaNominal, System.Nullable(Of Double)), obj.PeriodoPago, obj.DiaDesde, obj.DiaHasta, obj.Mercado, CType(obj.DeclaraDividendos, System.Nullable(Of Boolean)), CType(obj.TituloMaterializado, System.Nullable(Of Boolean)), obj.Sector, CType(obj.Activo, System.Nullable(Of Boolean)), obj.Emisora, obj.TipoTasaFija, obj.Usuario, CType(obj.BusIntegracion, System.Nullable(Of Boolean)), CType(obj.Suscripcion, System.Nullable(Of Date)), CType(obj.CaracteristicasRF, System.Nullable(Of Boolean)), obj.Bursatilidad, obj.ClaseInversion, obj.Corresponde, obj.BaseCalculoInteres, obj.RefTasaVble, obj.Amortiza, obj.ClaseAcciones, obj.ClasificacionRiesgo, obj.Liquidez, CType(obj.Negociable, System.Nullable(Of Boolean)), obj.IDBolsa, obj.NroAcciones, p1, obj.IDMoneda, obj.CurvaEspecie, CType(obj.CodigoAvalado, System.Nullable(Of Boolean)), obj.NitAvalista, obj.NitAdministradorAutonomo, obj.IDCalificacionInversion, obj.ISINesInsertar, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)), obj.AdmisionGarantia, obj.TipoEspecie, obj.FechaBursatilidad, obj.PrefijoFactorRiesgo, obj.FactorRiesgo, obj.TipoCalculo, obj.ClaseContableTitulo, obj.MacroActivo, obj.Indicador, obj.ConceptoRetencion, obj.TituloParticipativo, obj.logDemocratizacion, obj.xmlDetalleADR)
        obj.IDEspecies = p1
    End Sub

    Private Sub UpdateEspeci(ByVal obj As Especi)
        Dim p1 As Integer = obj.IDEspecies
        Me.uspOyDNet_Maestros_Especies_Actualizar(obj.Id, obj.Nombre, CType(obj.EsAccion, System.Nullable(Of Boolean)), obj.IDClase, obj.IDTarifa, obj.IDGrupo, obj.IDSubGrupo, CType(obj.VlrNominal, System.Nullable(Of Decimal)), obj.Notas, obj.IdEmisor, obj.IDAdmonEmision, obj.LeyCirculacion, CType(obj.Emision, System.Nullable(Of Date)), CType(obj.Vencimiento, System.Nullable(Of Date)), obj.Modalidad, CType(obj.TasaInicial, System.Nullable(Of Double)), CType(obj.TasaNominal, System.Nullable(Of Double)), obj.PeriodoPago, obj.DiaDesde, obj.DiaHasta, obj.Mercado, CType(obj.DeclaraDividendos, System.Nullable(Of Boolean)), CType(obj.TituloMaterializado, System.Nullable(Of Boolean)), obj.Sector, CType(obj.Activo, System.Nullable(Of Boolean)), obj.Emisora, obj.TipoTasaFija, obj.Usuario, CType(obj.BusIntegracion, System.Nullable(Of Boolean)), CType(obj.Suscripcion, System.Nullable(Of Date)), CType(obj.CaracteristicasRF, System.Nullable(Of Boolean)), obj.Bursatilidad, obj.ClaseInversion, obj.Corresponde, obj.BaseCalculoInteres, obj.RefTasaVble, obj.Amortiza, obj.ClaseAcciones, obj.ClasificacionRiesgo, obj.Liquidez, CType(obj.Negociable, System.Nullable(Of Boolean)), obj.IDBolsa, obj.NroAcciones, p1, obj.IDMoneda, obj.CurvaEspecie, CType(obj.CodigoAvalado, System.Nullable(Of Boolean)), obj.NitAvalista, obj.NitAdministradorAutonomo, obj.IDCalificacionInversion, obj.ISINesInsertar, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)), obj.AdmisionGarantia, obj.TipoEspecie, obj.FechaBursatilidad, obj.PrefijoFactorRiesgo, obj.FactorRiesgo, obj.TipoCalculo, obj.ClaseContableTitulo, obj.MacroActivo, obj.Indicador, obj.ConceptoRetencion, obj.TituloParticipativo, obj.logDemocratizacion, obj.xmlDetalleADR)
        obj.IDEspecies = p1
    End Sub

    Private Sub InsertEspeciesISINFungible(ByVal obj As EspeciesISINFungible)
        Dim p1 As System.Nullable(Of Integer) = obj.IDIsinFungible
        Dim p2 As String = obj.MsjAmortizaciones
        Me.uspOyDNet_Maestros_ISINFungible_Actualizar(obj.ISIN, obj.Descripcion, obj.IDEspecie, obj.IDFungible, CType(obj.Emision, System.Nullable(Of Integer)), CType(obj.IDConsecutivo, System.Nullable(Of Integer)), CType(obj.Fecha_Emision, System.Nullable(Of Date)), CType(obj.Fecha_Vencimiento, System.Nullable(Of Date)), CType(obj.Tasa_Facial, System.Nullable(Of Decimal)), obj.Modalidad, obj.intIndicador, CType(obj.TasaBase, System.Nullable(Of Integer)), CType(obj.Puntos_Indicador, System.Nullable(Of Decimal)), p1, obj.Amortizada, obj.logPoseeRetencion, obj.dblPorcentajeRetencion, obj.logFlujosIrregulares, obj.logActivo, obj.logSectorFinanciero, obj.ConEspecie, obj.Amortizaciones, p2, obj.Usuario, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)), CType(obj.intIDCalificacionInversion, System.Nullable(Of Integer)), CType(obj.dblTasaEfectiva, System.Nullable(Of Double)), CType(obj.Minimo, System.Nullable(Of Double)), CType(obj.Multiplo, System.Nullable(Of Double)), CType(obj.Fecha_Irregular, System.Nullable(Of Date)), CType(obj.intIDConceptoRetencion, System.Nullable(Of Integer)), obj.strTipoEspecie, obj.logEsAccion)
        obj.IDIsinFungible = p1.GetValueOrDefault
        obj.MsjAmortizaciones = p2
    End Sub

    Private Sub UpdateEspeciesISINFungible(ByVal obj As EspeciesISINFungible)
        Dim p1 As System.Nullable(Of Integer) = obj.IDIsinFungible
        Dim p2 As String = obj.MsjAmortizaciones
        Me.uspOyDNet_Maestros_ISINFungible_Actualizar(obj.ISIN, obj.Descripcion, obj.IDEspecie, obj.IDFungible, CType(obj.Emision, System.Nullable(Of Integer)), CType(obj.IDConsecutivo, System.Nullable(Of Integer)), CType(obj.Fecha_Emision, System.Nullable(Of Date)), CType(obj.Fecha_Vencimiento, System.Nullable(Of Date)), CType(obj.Tasa_Facial, System.Nullable(Of Decimal)), obj.Modalidad, obj.intIndicador, CType(obj.TasaBase, System.Nullable(Of Integer)), CType(obj.Puntos_Indicador, System.Nullable(Of Decimal)), p1, obj.Amortizada, obj.logPoseeRetencion, obj.dblPorcentajeRetencion, obj.logFlujosIrregulares, obj.logActivo, obj.logSectorFinanciero, obj.ConEspecie, obj.Amortizaciones, p2, obj.Usuario, obj.InfoSesion, CType(Nothing, System.Nullable(Of Byte)), CType(obj.intIDCalificacionInversion, System.Nullable(Of Integer)), CType(obj.dblTasaEfectiva, System.Nullable(Of Double)), CType(obj.Minimo, System.Nullable(Of Double)), CType(obj.Multiplo, System.Nullable(Of Double)), CType(obj.Fecha_Irregular, System.Nullable(Of Date)), CType(obj.intIDConceptoRetencion, System.Nullable(Of Integer)), obj.strTipoEspecie, obj.logEsAccion)
        obj.IDIsinFungible = p1.GetValueOrDefault
        obj.MsjAmortizaciones = p2
    End Sub

End Class
