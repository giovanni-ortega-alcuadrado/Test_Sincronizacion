Imports System.Threading.Tasks
Imports A2.OyD.OYDServer.RIA.Web.CFOperaciones
Imports System.Web
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura
''' <summary>
''' DomainServices para las pantallas correspondientes a la migración de Titulos2008 a .NET
''' </summary>
''' Creado por       : Germán Arbey González Osorio (Alcuadrado S.A.)
''' Descripción      : Creacion.
''' Fecha            : Diciembre 15/2014
''' Pruebas CB       : Germán Arbey González Osorio - Diciembre 15/2014 - Resultado Ok
''' <remarks></remarks>

<EnableClientAccess()> _
Partial Public Class OperacionesCFDomainService
    Inherits LinqToSqlDomainService(Of CF_OperacionesDataContext)

    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertOperaciones_Combos(ByVal currentRegistro As Operaciones_Combos)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertOperaciones_Combos")
        End Try
    End Sub

    Public Sub UpdateOperaciones_Combos(ByVal currentRegistro As Operaciones_Combos)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateOperaciones_Combos")
        End Try
    End Sub

    Public Sub DeleteOperaciones_Combos(ByVal currentRegistro As Operaciones_Combos)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteOperaciones_Combos")
        End Try
    End Sub

    Public Sub InsertOperaciones_RespuestaValidacion(ByVal currentRegistro As Operaciones_RespuestaValidacion)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertOperaciones_RespuestaValidacion")
        End Try
    End Sub

    Public Sub UpdateOperaciones_RespuestaValidacion(ByVal currentRegistro As Operaciones_RespuestaValidacion)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateOperaciones_RespuestaValidacion")
        End Try
    End Sub

    Public Sub DeleteOperaciones_RespuestaValidacion(ByVal currentRegistro As Operaciones_RespuestaValidacion)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteOperaciones_RespuestaValidacion")
        End Try
    End Sub

    Public Sub InsertOperaciones_TiposNegocio(ByVal currentRegistro As Operaciones_TiposNegocio)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertOperaciones_TiposNegocio")
        End Try
    End Sub

    Public Sub UpdateOperaciones_TiposNegocio(ByVal currentRegistro As Operaciones_TiposNegocio)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateOperaciones_TiposNegocio")
        End Try
    End Sub

    Public Sub DeleteOperaciones_TiposNegocio(ByVal currentRegistro As Operaciones_TiposNegocio)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteOperaciones_TiposNegocio")
        End Try
    End Sub

    Public Sub InsertOperaciones_OtrosNegocios(ByVal currentRegistro As Operaciones_OtrosNegocios)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertOperaciones_OtrosNegocios")
        End Try
    End Sub

    Public Sub UpdateOperaciones_OtrosNegocios(ByVal currentRegistro As Operaciones_OtrosNegocios)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateOperaciones_OtrosNegocios")
        End Try
    End Sub

    Public Sub DeleteOperaciones_OtrosNegocios(ByVal currentRegistro As Operaciones_OtrosNegocios)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteOperaciones_OtrosNegocios")
        End Try
    End Sub

    Public Sub InsertOperaciones_ReceptoresOtrosNegocios(ByVal currentRegistro As Operaciones_ReceptoresOtrosNegocios)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertOperaciones_ReceptoresOtrosNegocios")
        End Try
    End Sub

    Public Sub UpdateOperaciones_ReceptoresOtrosNegocios(ByVal currentRegistro As Operaciones_ReceptoresOtrosNegocios)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateOperaciones_ReceptoresOtrosNegocios")
        End Try
    End Sub

    Public Sub DeleteOperaciones_ReceptoresOtrosNegocios(ByVal currentRegistro As Operaciones_ReceptoresOtrosNegocios)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteOperaciones_ReceptoresOtrosNegocios")
        End Try
    End Sub

    Public Sub InsertOperaciones_EntidadDefecto(ByVal currentRegistro As Operaciones_EntidadDefecto)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertOperaciones_EntidadDefecto")
        End Try
    End Sub

    Public Sub UpdateOperaciones_EntidadDefecto(ByVal currentRegistro As Operaciones_EntidadDefecto)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateOperaciones_EntidadDefecto")
        End Try
    End Sub

    Public Sub DeleteOperaciones_EntidadDefecto(ByVal currentRegistro As Operaciones_EntidadDefecto)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteOperaciones_EntidadDefecto")
        End Try
    End Sub

    Public Sub InsertOperaciones_OtrosNegociosCalculos(ByVal currentRegistro As Operaciones_OtrosNegociosCalculos)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertOperaciones_OtrosNegociosCalculos")
        End Try
    End Sub

    Public Sub UpdateOperaciones_OtrosNegociosCalculos(ByVal currentRegistro As Operaciones_OtrosNegociosCalculos)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateOperaciones_OtrosNegociosCalculos")
        End Try
    End Sub

    Public Sub DeleteOperaciones_OtrosNegociosCalculos(ByVal currentRegistro As Operaciones_OtrosNegociosCalculos)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteOperaciones_OtrosNegociosCalculos")
        End Try
    End Sub

    Public Sub InsertOperaciones_EntidadesCuentasDeposito(ByVal currentRegistro As Operaciones_EntidadesCuentasDeposito)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertOperaciones_EntidadesCuentasDeposito")
        End Try
    End Sub

    Public Sub UpdateOperaciones_EntidadesCuentasDeposito(ByVal currentRegistro As Operaciones_EntidadesCuentasDeposito)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateOperaciones_EntidadesCuentasDeposito")
        End Try
    End Sub

    Public Sub DeleteOperaciones_EntidadesCuentasDeposito(ByVal currentRegistro As Operaciones_EntidadesCuentasDeposito)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteOperaciones_EntidadesCuentasDeposito")
        End Try
    End Sub

    Public Sub InsertHomologacionTipoOrigen(ByVal currentRegistro As HomologacionTipoOrigen)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentRegistro.pstrUsuarioConexion, currentRegistro.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentRegistro.InfoSesion = DemeInfoSesion(currentRegistro.pstrUsuarioConexion, "InsertHomologacionTipoOrigen")
            Me.DataContext.HomologacionTipoOrigen.InsertOnSubmit(currentRegistro)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertHomologacionTipoOrigen")
        End Try
    End Sub

    Public Sub UpdateHomologacionTipoOrigen(ByVal currentRegistro As HomologacionTipoOrigen)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentRegistro.pstrUsuarioConexion, currentRegistro.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentRegistro.InfoSesion = DemeInfoSesion(currentRegistro.pstrUsuarioConexion, "UpdateHomologacionTipoOrigen")
            Me.DataContext.HomologacionTipoOrigen.Attach(currentRegistro, Me.ChangeSet.GetOriginal(currentRegistro))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateHomologacionTipoOrigen")
        End Try
    End Sub

    Public Sub DeleteHomologacionTipoOrigen(ByVal currentRegistro As HomologacionTipoOrigen)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentRegistro.pstrUsuarioConexion, currentRegistro.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentRegistro.InfoSesion = DemeInfoSesion(currentRegistro.pstrUsuarioConexion, "DeleteHomologacionTipoOrigen")
            Me.DataContext.HomologacionTipoOrigen.Attach(currentRegistro)
            Me.DataContext.HomologacionTipoOrigen.DeleteOnSubmit(currentRegistro)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteHomologacionTipoOrigen")
        End Try
    End Sub

    Public Sub InsertInmobiliarios(ByVal currentRegistro As Inmobiliarios)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertInmobiliarios")
        End Try
    End Sub

    Public Sub UpdateInmobiliarios(ByVal currentRegistro As Inmobiliarios)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateInmobiliarios")
        End Try
    End Sub

    Public Sub DeleteInmobiliarios(ByVal currentRegistro As Inmobiliarios)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteInmobiliarios")
        End Try
    End Sub

    Public Sub InsertInmobiliarios_Detalles(ByVal currentRegistro As Inmobiliarios_Detalles)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertInmobiliarios_Detalles")
        End Try
    End Sub

    Public Sub UpdateInmobiliarios_Detalles(ByVal currentRegistro As Inmobiliarios_Detalles)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateInmobiliarios_Detalles")
        End Try
    End Sub

    Public Sub DeleteInmobiliarios_Detalles(ByVal currentRegistro As Inmobiliarios_Detalles)
        Try

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteInmobiliarios_Detalles")
        End Try
    End Sub

#End Region

#Region "Métodos asincrónicos"

    Public Function Operaciones_ConsultarCombos(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_Combos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_LiquidacionesOtrosNegocios_ConsultarCombos(pstrUsuario, DemeInfoSesion(pstrUsuario, "Operaciones_ConsultarCombos"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Operaciones_ConsultarCombos")
            Return Nothing
        End Try
    End Function

    Public Function Operaciones_ConsultarTiposNegocio(ByVal pstrAplicacion As String, ByVal pstrVersion As String, ByVal pstrUsuarioUtilidades As String, ByVal pstrClave As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_TiposNegocio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, A2Utilidades.CifrarSL.descifrar(pstrUsuario), pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not String.IsNullOrEmpty(pstrAplicacion) Then
                pstrAplicacion = A2Utilidades.Cifrar.cifrar(A2Utilidades.CifrarSL.descifrar(pstrAplicacion))
            End If
            If Not String.IsNullOrEmpty(pstrVersion) Then
                pstrVersion = A2Utilidades.Cifrar.cifrar(A2Utilidades.CifrarSL.descifrar(pstrVersion))
            End If
            If Not String.IsNullOrEmpty(pstrUsuarioUtilidades) Then
                pstrUsuarioUtilidades = A2Utilidades.Cifrar.cifrar(A2Utilidades.CifrarSL.descifrar(pstrUsuarioUtilidades))
            End If
            If Not String.IsNullOrEmpty(pstrClave) Then
                pstrClave = A2Utilidades.Cifrar.cifrar(A2Utilidades.CifrarSL.descifrar(pstrClave))
            End If
            If Not String.IsNullOrEmpty(pstrUsuario) Then
                pstrUsuario = A2Utilidades.Cifrar.cifrar(A2Utilidades.CifrarSL.descifrar(pstrUsuario))
            End If
            If Not String.IsNullOrEmpty(pstrMaquina) Then
                pstrMaquina = A2Utilidades.Cifrar.cifrar(A2Utilidades.CifrarSL.descifrar(pstrMaquina))
            End If

            Dim ret = Me.DataContext.uspCalculosFinancieros_LiquidacionesOtrosNegocios_ConsultarTiposNegocio(pstrAplicacion, pstrVersion, pstrUsuarioUtilidades, pstrClave, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "Operaciones_ConsultarTiposNegocio"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Operaciones_ConsultarTiposNegocio")
            Return Nothing
        End Try
    End Function

    Public Function Operaciones_OtrosNegociosFiltrar(ByVal pstrEstado As String, ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_OtrosNegocios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_LiquidacionesOtrosNegocios_Filtrar(pstrEstado, pstrFiltro, pstrUsuario, DemeInfoSesion(pstrUsuario, "Operaciones_OtrosNegociosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Operaciones_OtrosNegociosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function Operaciones_OtrosNegociosConsultar(ByVal pintID As Nullable(Of Integer), ByVal pstrReferencia As String, ByVal pstrTipoRegistro As String, ByVal pstrEstado As String, ByVal pstrTipoNegocio As String, ByVal pstrTipoOrigen As String, ByVal pstrTipoOperacion As String, ByVal pstrCliente As String, ByVal pstrNemotecnico As String, ByVal pdtmFechaOperacion As Nullable(Of DateTime), ByVal pdtmFechaCumplimiento As Nullable(Of DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_OtrosNegocios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_LiquidacionesOtrosNegocios_Consultar(pintID, pstrReferencia, pstrTipoRegistro, pstrEstado, pstrTipoNegocio, pstrTipoOrigen, pstrTipoOperacion, pstrCliente, pstrNemotecnico, pdtmFechaOperacion, pdtmFechaCumplimiento, pstrUsuario, DemeInfoSesion(pstrUsuario, "Operaciones_OtrosNegociosConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Operaciones_OtrosNegociosConsultar")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function Operaciones_OtrosNegociosValidar(ByVal pintID As Integer,
                                                     ByVal pstrReferencia As String,
                                                     ByVal pstrEstado As String,
                                                     ByVal pstrTipoNegocio As String,
                                                     ByVal pstrTipoOrigen As String,
                                                     ByVal plngIDComitente As String,
                                                     ByVal plngIDOrdenante As String,
                                                     ByVal pstrDeposito As String,
                                                     ByVal plngIDCuentaDeposito As Nullable(Of Integer),
                                                     ByVal pintIDContraparte As Nullable(Of Integer),
                                                     ByVal pintIDCuentaDepositoContraparte As Nullable(Of Integer),
                                                     ByVal pstrClasificacionInversion As String,
                                                     ByVal pintIDPagador As Nullable(Of Integer),
                                                     ByVal pstrTipoOperacion As String,
                                                     ByVal pdtmFechaOperacion As Nullable(Of DateTime),
                                                     ByVal pdtmFechaCumplimiento As Nullable(Of DateTime),
                                                     ByVal pdtmFechaVencimientoOperacion As Nullable(Of DateTime),
                                                     ByVal pstrNemotecnico As String,
                                                     ByVal pstrISIN As String,
                                                     ByVal pdtmFechaEmision As Nullable(Of DateTime),
                                                     ByVal pdtmFechaVencimiento As Nullable(Of DateTime),
                                                     ByVal pstrModalidad As String,
                                                     ByVal pdblTasaFacial As Nullable(Of Double),
                                                     ByVal pstrIndicadorEconomico As String,
                                                     ByVal pdblPuntosIndicador As Nullable(Of Double),
                                                     ByVal pstrTipoCumplimiento As String,
                                                     ByVal pstrMercado As String,
                                                     ByVal pdblNominal As Nullable(Of Double),
                                                     ByVal pintIDMoneda As Nullable(Of Integer),
                                                     ByVal pdblTasaCambioConversion As Nullable(Of Double),
                                                     ByVal pdblPrecioSucio As Nullable(Of Double),
                                                     ByVal pdblTasaNegociacionYield As Nullable(Of Double),
                                                     ByVal pdblValorGiroBruto As Nullable(Of Double),
                                                     ByVal pdblTasaPactadaRepo As Nullable(Of Double),
                                                     ByVal pdblHaircut As Nullable(Of Double),
                                                     ByVal pdblPorcentajeComision As Nullable(Of Double),
                                                     ByVal pdblValorComision As Nullable(Of Double),
                                                     ByVal pdblIVA As Nullable(Of Double),
                                                     ByVal pstrTipoReteFuente As String,
                                                     ByVal pdblRetencion As Nullable(Of Double),
                                                     ByVal pdblValorNeto As Nullable(Of Double),
                                                     ByVal pdblValorRegresoRepo As Nullable(Of Double),
                                                     ByVal pstrObservaciones As String,
                                                     ByVal pintPaisNegociacion As Nullable(Of Integer),
                                                     ByVal pstrTipoRepo As String,
                                                     ByVal pstrTipoRegistro As String,
                                                     ByVal pstrXMLReceptores As String,
                                                     ByVal pstrUsuario As String,
                                                     ByVal pstrUsuarioWindows As String,
                                                     ByVal pstrMaquina As String,
                                                     ByVal pdblValorNetoCop As Nullable(Of Double),
                                                     ByVal plogTipoBanRep As Nullable(Of Boolean),
                                                     ByVal pdblValorFlujoIntermedio As Nullable(Of Double),
                                                     ByVal plngIDOrdenOrigen As Nullable(Of Integer),
                                                     ByVal plngIDComitenteOtro As String,
                                                     ByVal pstrTipo As String,
                                                     ByVal pstrDepositoContraparte As String,
                                                     ByVal plngIDCuentaDepositoContraparte As Nullable(Of Integer),
                                                     ByVal pdblTasaNeta As Nullable(Of Double),
                                                     ByVal pdblValorGiroBrutoCOP As Nullable(Of Double),
                                                     ByVal pdblValorComisionCOP As Nullable(Of Double),
                                                     ByVal pdblValorRegresoRepoMoneda As Nullable(Of Double),
                                                     ByVal pstrInfoConexion As String,
                                                     ByVal plogOperacionAplazada As Nullable(Of Boolean),
                                                     ByVal pdblCantidadAplazada As System.Nullable(Of System.Double),
                                                     ByVal pdtmFechaCumplimientoAplazada As System.Nullable(Of System.DateTime)) As List(Of Operaciones_RespuestaValidacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_LiquidacionesOtrosNegocios_Validar(pintID, pstrReferencia, pstrEstado, pstrTipoNegocio, pstrTipoOrigen, plngIDComitente,
                                                                                               plngIDOrdenante, pstrDeposito, plngIDCuentaDeposito, pintIDContraparte, pintIDCuentaDepositoContraparte, pstrClasificacionInversion,
                                                                                               pintIDPagador, pstrTipoOperacion, pdtmFechaOperacion, pdtmFechaCumplimiento, pdtmFechaVencimientoOperacion,
                                                                                               pstrNemotecnico, pstrISIN, pdtmFechaEmision, pdtmFechaVencimiento, pstrModalidad, pdblTasaFacial, pstrIndicadorEconomico, pdblPuntosIndicador,
                                                                                               pstrTipoCumplimiento, pstrMercado, pdblNominal, pintIDMoneda, pdblTasaCambioConversion, pdblPrecioSucio, pdblTasaNegociacionYield, pdblValorGiroBruto,
                                                                                               pdblTasaPactadaRepo, pdblHaircut, pdblPorcentajeComision, pdblValorComision, pdblIVA, pstrTipoReteFuente,
                                                                                               pdblRetencion, pdblValorNeto, pdblValorRegresoRepo, pstrObservaciones, pintPaisNegociacion,
                                                                                               pstrTipoRepo, pstrUsuario, pstrTipoRegistro, pstrXMLReceptores, pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "Operaciones_OtrosNegociosValidar"), 0,
                                                                                               pdblValorNetoCop, plogTipoBanRep, pdblValorFlujoIntermedio, plngIDOrdenOrigen, plngIDComitenteOtro, pstrTipo,
                                                                                               pstrDepositoContraparte, plngIDCuentaDepositoContraparte, pdblTasaNeta, pdblValorGiroBrutoCOP,
                                                                                               pdblValorComisionCOP, pdblValorRegresoRepoMoneda, plogOperacionAplazada, pdblCantidadAplazada,
                                                                                               pdtmFechaCumplimientoAplazada).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Operaciones_OtrosNegociosValidar")
            Return Nothing
        End Try
    End Function

    Public Function Operaciones_OtrosNegociosAnular(ByVal pintID As Integer,
                                                     ByVal pstrObservaciones As String,
                                                     ByVal pstrUsuario As String,
                                                     ByVal pstrUsuarioWindows As String,
                                                     ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_RespuestaValidacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_LiquidacionesOtrosNegocios_Anular(pintID, pstrObservaciones, pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "Operaciones_OtrosNegociosAnular"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Operaciones_OtrosNegociosAnular")
            Return Nothing
        End Try
    End Function

    Public Function Operaciones_OtrosNegociosLiqxDife(ByVal pintID As Integer,
                                                     ByVal pstrObservaciones As String,
                                                     ByVal pstrUsuario As String,
                                                     ByVal pstrUsuarioWindows As String,
                                                     ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_RespuestaValidacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_LiquidacionesOtrosNegocios_LiqxDife(pintID, pstrObservaciones, pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "Operaciones_OtrosNegociosLiqxDife"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Operaciones_OtrosNegociosLiqxDife")
            Return Nothing
        End Try
    End Function

    Public Function Operaciones_OtrosNegociosReceptores(ByVal pintIDLiquidacion As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_ReceptoresOtrosNegocios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_LiquidacionesOtrosNegocios_ConsultarReceptores(pintIDLiquidacion, pstrUsuario, DemeInfoSesion(pstrUsuario, "Operaciones_OtrosNegociosReceptores"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Operaciones_OtrosNegociosReceptores")
            Return Nothing
        End Try
    End Function

    Public Function Operaciones_EntidadDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_EntidadDefecto)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_LiquidacionesOtrosNegocios_EntidadDefecto(pstrUsuario, DemeInfoSesion(pstrUsuario, "Operaciones_EntidadDefecto"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Operaciones_EntidadDefecto")
            Return Nothing
        End Try
    End Function

    Public Function Operaciones_OtrosNegociosReceptoresCliente(ByVal pstrCliente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_ReceptoresOtrosNegocios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_LiquidacionesOtrosNegocios_ConsultarReceptoresCliente(pstrCliente, pstrUsuario, DemeInfoSesion(pstrUsuario, "Operaciones_OtrosNegociosReceptoresCliente"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Operaciones_OtrosNegociosReceptoresCliente")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)> _
    Public Function Operaciones_OtrosNegociosCalcular(ByVal pstrTipoCalculo As String,
                                                     ByVal pstrTipoNegocio As String,
                                                     ByVal pstrTipoOrigen As String,
                                                     ByVal plngIDComitente As String,
                                                     ByVal pstrTipoOperacion As String,
                                                     ByVal pdtmFechaOperacion As Nullable(Of DateTime),
                                                     ByVal pdtmFechaCumplimiento As Nullable(Of DateTime),
                                                     ByVal pdtmFechaVencimientoOperacion As Nullable(Of DateTime),
                                                     ByVal pstrNemotecnico As String,
                                                     ByVal pstrISIN As String,
                                                     ByVal pdtmFechaEmision As Nullable(Of DateTime),
                                                     ByVal pdtmFechaVencimiento As Nullable(Of DateTime),
                                                     ByVal pstrModalidad As String,
                                                     ByVal pdblTasaFacial As Nullable(Of Double),
                                                     ByVal pstrIndicadorEconomico As String,
                                                     ByVal pdblPuntosIndicador As Nullable(Of Double),
                                                     ByVal pdblNominal As Nullable(Of Double),
                                                     ByVal pintIDMoneda As Nullable(Of Integer),
                                                     ByVal pdblTasaCambioConversion As Nullable(Of Double),
                                                     ByVal pdblPrecioSucio As Nullable(Of Double),
                                                     ByVal pdblTasaNegociacionYield As Nullable(Of Double),
                                                     ByVal pdblValorGiroBruto As Nullable(Of Double),
                                                     ByVal pdblTasaPactadaRepo As Nullable(Of Double),
                                                     ByVal pdblHaircut As Nullable(Of Double),
                                                     ByVal pdblPorcentajeComision As Nullable(Of Double),
                                                     ByVal pdblValorComision As Nullable(Of Double),
                                                     ByVal pdblIVA As Nullable(Of Double),
                                                     ByVal pstrTipoReteFuente As String,
                                                     ByVal pdblRetencion As Nullable(Of Double),
                                                     ByVal pdblValorNeto As Nullable(Of Double),
                                                     ByVal pdblValorRegresoRepo As Nullable(Of Double),
                                                     ByVal pintPaisNegociacion As Nullable(Of Integer),
                                                     ByVal pstrUsuario As String,
                                                     ByVal pstrUsuarioWindows As String,
                                                     ByVal pstrMaquina As String,
                                                     ByVal pdblValorNetoCop As Nullable(Of Double),
                                                     ByVal plogTipoBanRep As Nullable(Of Boolean),
                                                     ByVal pdblValorFlujoIntermedio As Nullable(Of Double),
                                                     ByVal pstrTipo As String,
                                                     ByVal pstrTipoRepo As String,
                                                     ByVal pdblTasaNeta As Nullable(Of Double),
                                                     ByVal pdblValorGiroBrutoCOP As Nullable(Of Double),
                                                     ByVal pdblValorComisionCOP As Nullable(Of Double),
                                                     ByVal pdblValorRegresoRepoMoneda As Nullable(Of Double), ByVal pstrInfoConexion As String) As List(Of Operaciones_OtrosNegociosCalculos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_LiquidacionesOtrosNegocios_Calcular(pstrTipoCalculo, pstrTipoNegocio, pstrTipoOrigen, plngIDComitente,
                                                                                               pstrTipoOperacion, pdtmFechaOperacion, pdtmFechaCumplimiento, pdtmFechaVencimientoOperacion,
                                                                                               pstrNemotecnico, pstrISIN, pdtmFechaEmision, pdtmFechaVencimiento, pstrModalidad, pdblTasaFacial, pstrIndicadorEconomico, pdblPuntosIndicador,
                                                                                               pdblNominal, pintIDMoneda, pdblTasaCambioConversion, pdblPrecioSucio, pdblTasaNegociacionYield, pdblValorGiroBruto,
                                                                                               pdblTasaPactadaRepo, pdblHaircut, pdblPorcentajeComision, pdblValorComision, pdblIVA, pstrTipoReteFuente,
                                                                                               pdblRetencion, pdblValorNeto, pdblValorRegresoRepo, pintPaisNegociacion,
                                                                                               pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "Operaciones_OtrosNegociosCalcular"), 0,
                                                                                               pdblValorNetoCop, plogTipoBanRep, pdblValorFlujoIntermedio, pstrTipo,
                                                                                               pstrTipoRepo, pdblTasaNeta, pdblValorGiroBrutoCOP, pdblValorComisionCOP,
                                                                                               pdblValorRegresoRepoMoneda).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Operaciones_OtrosNegociosCalcular")
            Return Nothing
        End Try
    End Function

    Public Function Operaciones_OtrosNegociosConsultarTRMMoneda(ByVal pdtmFechaOperacion As DateTime, ByVal pintIDMoneda As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Double
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim dblValorRetorno As Double = 0
            Me.DataContext.uspCalculosFinancieros_LiquidacionesOtrosNegocios_ConsultarTRM(pdtmFechaOperacion, pintIDMoneda, dblValorRetorno, pstrUsuario, DemeInfoSesion(pstrUsuario, "Operaciones_OtrosNegociosConsultarTRMMoneda"), 0)
            Return dblValorRetorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Operaciones_OtrosNegociosConsultarTRMMoneda")
            Return Nothing
        End Try
    End Function

    Public Function Operaciones_EntidadesCuentasDeposito(ByVal plogConsultarTodos As Boolean, ByVal pintIDEntidad As Integer, ByVal plogConsultarPorNroDocumento As Boolean, ByVal pstrNroDocumento As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_EntidadesCuentasDeposito)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Entidades_CuentasDeposito_Consultar(plogConsultarTodos, pintIDEntidad, plogConsultarPorNroDocumento, pstrNroDocumento, pstrUsuario, DemeInfoSesion(pstrUsuario, "Operaciones_EntidadesCuentasDeposito"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Operaciones_EntidadesCuentasDeposito")
            Return Nothing
        End Try
    End Function

    Public Function Operaciones_OtrosNegociosValidarEstado(ByVal pintID As Integer,
                                                           ByVal pstrAccion As String,
                                                           ByVal pstrUsuario As String,
                                                           ByVal pstrUsuarioWindows As String,
                                                           ByVal pstrMaquina As String, ByVal pstrInfoConexion As String,
                                                           ByVal plogOperacionAplazada As System.Nullable(Of System.Boolean)) As List(Of Operaciones_RespuestaValidacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_LiquidacionesOtrosNegocios_ValidarEstado(pintID, pstrAccion, pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "Operaciones_OtrosNegociosValidarEstado"), 0, plogOperacionAplazada).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Operaciones_OtrosNegociosValidarEstado")
            Return Nothing
        End Try
    End Function

    Public Function Operaciones_OtrosNegociosConsultarPrecioMercado(ByVal pstrEspecie As String, ByVal pstrISIN As String, ByVal pdtmFechaOperacion As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Double
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim dblValorRetorno As Double = 0
            Me.DataContext.uspCalculosFinancieros_LiquidacionesOtrosNegocios_ConsultarPRECIO(pstrEspecie, pstrISIN, pdtmFechaOperacion, dblValorRetorno, pstrUsuario, DemeInfoSesion(pstrUsuario, "Operaciones_OtrosNegociosConsultarPrecioMercado"), 0)
            Return dblValorRetorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Operaciones_OtrosNegociosConsultarPrecioMercado")
            Return Nothing
        End Try
    End Function

    Public Function HomologacionTipoOrigen_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of HomologacionTipoOrigen)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_HomologacionTipoOrigen_Filtrar(pstrFiltro, pstrUsuario, DemeInfoSesion(pstrUsuario, "HomologacionTipoOrigen_Filtrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "HomologacionTipoOrigen_Filtrar")
            Return Nothing
        End Try
    End Function

    Public Function HomologacionTipoOrigen_Consultar(ByVal pstrTipoOrigenPrincipal As String, ByVal pstrTipoOrigenSecundario As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of HomologacionTipoOrigen)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_HomologacionTipoOrigen_Consultar(pstrTipoOrigenPrincipal, pstrTipoOrigenSecundario, pstrUsuario, DemeInfoSesion(pstrUsuario, "HomologacionTipoOrigen_Consultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "HomologacionTipoOrigen_Consultar")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)> _
    Public Function InmobiliariosValidar(ByVal pintID As Integer,
                                                     ByVal pstrCodigo As String,
                                                     ByVal pstrEstado As String,
                                                     ByVal pstrTipo As String,
                                                     ByVal pstrArea As String,
                                                     ByVal pstrCodigoCatastro As String,
                                                     ByVal pstrDireccion As String,
                                                     ByVal plngIDComitente As String,
                                                     ByVal pdtmFechaCompra As Nullable(Of DateTime),
                                                     ByVal pdblValorCompra As Nullable(Of Double),
                                                     ByVal pintMonedaNegociacion As Nullable(Of Integer),
                                                     ByVal pintMonedaLocal As Nullable(Of Integer),
                                                     ByVal pdblTasaCambio As Nullable(Of Double),
                                                     ByVal pstrDescripcion As String,
                                                     ByVal pdtmFechaCumplimiento As Nullable(Of DateTime),
                                                     ByVal pdtmFechaUltimaValoracion As Nullable(Of DateTime),
                                                     ByVal pdblValorCompraActualizado As Nullable(Of Double),
                                                     ByVal pdblValorOperacion As Nullable(Of Double),
                                                     ByVal pstrUsuario As String,
                                                     ByVal pstrUsuarioWindows As String,
                                                     ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_RespuestaValidacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrCodigo = HttpUtility.UrlDecode(pstrCodigo)
            pstrArea = HttpUtility.UrlDecode(pstrArea)
            pstrCodigoCatastro = HttpUtility.UrlDecode(pstrCodigoCatastro)
            pstrDireccion = HttpUtility.UrlDecode(pstrDireccion)
            pstrDescripcion = HttpUtility.UrlDecode(pstrDescripcion)

            Dim ret = Me.DataContext.uspCalculosFinancieros_Inmobiliarios_Validar(pintID, pstrCodigo, pstrEstado, pstrTipo, pstrArea, pstrCodigoCatastro, pstrDireccion, plngIDComitente,
                                                                                  pdtmFechaCompra, pdblValorCompra, pintMonedaNegociacion, pintMonedaLocal, pdblTasaCambio, pstrDescripcion,
                                                                                  pdtmFechaCumplimiento, pdtmFechaUltimaValoracion, pdblValorCompraActualizado, pdblValorOperacion,
                                                                                  pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "InmobiliariosValidar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InmobiliariosValidar")
            Return Nothing
        End Try
    End Function

    Public Function InmobiliariosAnular(ByVal pintID As Integer,
                                                     ByVal pstrObservaciones As String,
                                                     ByVal pstrUsuario As String,
                                                     ByVal pstrUsuarioWindows As String,
                                                     ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_RespuestaValidacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Inmobiliarios_Anular(pintID, pstrObservaciones, pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "InmobiliariosAnular"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InmobiliariosAnular")
            Return Nothing
        End Try
    End Function

    Public Function InmobiliariosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Inmobiliarios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Inmobiliarios_Filtrar(pstrFiltro, pstrUsuario, DemeInfoSesion(pstrUsuario, "InmobiliariosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InmobiliariosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function InmobiliariosConsultar(ByVal pintID As Nullable(Of Integer), ByVal pstrCodigo As String, ByVal pdtmFechaCompra As Nullable(Of DateTime), ByVal plngIDComitente As String, ByVal pstrTipo As String, ByVal pstrEstado As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Inmobiliarios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Inmobiliarios_Consultar(pintID, pstrCodigo, pdtmFechaCompra, plngIDComitente, pstrTipo, pstrEstado, pstrUsuario, DemeInfoSesion(pstrUsuario, "InmobiliariosConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InmobiliariosConsultar")
            Return Nothing
        End Try
    End Function

    Public Function InmobiliariosCombos(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_Combos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_Inmobiliarios_ConsultarCombos(pstrUsuario, DemeInfoSesion(pstrUsuario, "InmobiliariosCombos"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InmobiliariosCombos")
            Return Nothing
        End Try
    End Function

    Public Function InmobiliariosDetalles_Consultar(ByVal pintIDInmobiliario As Integer, ByVal pdtmFechaInicial As Nullable(Of DateTime), ByVal pdtmFechaFinal As Nullable(Of DateTime), ByVal pintIDConcepto As Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Inmobiliarios_Detalles)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_InmobiliariosDetalles_Consultar(pintIDInmobiliario, pdtmFechaInicial, pdtmFechaFinal, pintIDConcepto, pstrUsuario, DemeInfoSesion(pstrUsuario, "InmobiliariosDetalles_Consultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InmobiliariosDetalles_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function InmobiliariosDetalles_Actualizar(ByVal pintID As Integer,
                                                     ByVal pintIDInmobiliario As Integer,
                                                     ByVal pdtmFechaMovimiento As DateTime,
                                                     ByVal pdblValor As Double,
                                                     ByVal pdblBaseIva As Double,
                                                     ByVal pstrTipoMovimiento As String,
                                                     ByVal plngIDDocumento As Nullable(Of Integer),
                                                     ByVal pstrTipoTesoreria As String,
                                                     ByVal pstrNombreConsecutivo As String,
                                                     ByVal pstrUsuario As String,
                                                     ByVal pstrUsuarioWindows As String,
                                                     ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_RespuestaValidacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_InmobiliariosDetalles_Actualizar(pintID,
                                                                                             pintIDInmobiliario,
                                                                                             pdtmFechaMovimiento,
                                                                                             pdblValor,
                                                                                             pdblBaseIva,
                                                                                             pstrTipoMovimiento,
                                                                                             plngIDDocumento,
                                                                                             pstrTipoTesoreria,
                                                                                             pstrNombreConsecutivo,
                                                                                             pstrUsuario,
                                                                                             pstrUsuarioWindows,
                                                                                             pstrMaquina,
                                                                                             DemeInfoSesion(pstrUsuario, "InmobiliariosDetalles_Actualizar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InmobiliariosDetalles_Actualizar")
            Return Nothing
        End Try
    End Function

    Public Function InmobiliariosDetalles_Eliminar(ByVal pintID As Integer,
                                                 ByVal pintIDInmobiliario As Integer,
                                                 ByVal pstrUsuario As String,
                                                 ByVal pstrUsuarioWindows As String,
                                                 ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_RespuestaValidacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_InmobiliariosDetalles_Eliminar(pintID,
                                                                                         pintIDInmobiliario,
                                                                                         pstrUsuario,
                                                                                         pstrUsuarioWindows,
                                                                                         pstrMaquina,
                                                                                         DemeInfoSesion(pstrUsuario, "InmobiliariosDetalles_Anular"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InmobiliariosDetalles_Anular")
            Return Nothing
        End Try
    End Function

    Public Function InmobiliariosDetalles_ValidarEstado(ByVal pintID As Integer,
                                                        ByVal pintIDInmobiliario As Integer,
                                                        ByVal pstrAccion As String,
                                                        ByVal pstrUsuario As String,
                                                        ByVal pstrUsuarioWindows As String,
                                                        ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_RespuestaValidacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_InmobiliariosDetalles_ValidarEstado(pintID,
                                                                                         pintIDInmobiliario,
                                                                                         pstrAccion,
                                                                                         pstrUsuario,
                                                                                         pstrUsuarioWindows,
                                                                                         pstrMaquina,
                                                                                         DemeInfoSesion(pstrUsuario, "InmobiliariosDetalles_ValidarEstado"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InmobiliariosDetalles_ValidarEstado")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function Operaciones_ConsultarCombosSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_Combos)
        Dim objTask As Task(Of List(Of Operaciones_Combos)) = Me.Operaciones_ConsultarCombosAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Operaciones_ConsultarCombosAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Operaciones_Combos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Operaciones_Combos)) = New TaskCompletionSource(Of List(Of Operaciones_Combos))()
        objTaskComplete.TrySetResult(Operaciones_ConsultarCombos(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Operaciones_ConsultarTiposNegocioSync(ByVal pstrAplicacion As String, ByVal pstrVersion As String, ByVal pstrUsuarioUtilidades As String, ByVal pstrClave As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_TiposNegocio)
        Dim objTask As Task(Of List(Of Operaciones_TiposNegocio)) = Me.Operaciones_ConsultarTiposNegocioAsync(pstrAplicacion, pstrVersion, pstrUsuarioUtilidades, pstrClave, pstrUsuario, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Operaciones_ConsultarTiposNegocioAsync(ByVal pstrAplicacion As String, ByVal pstrVersion As String, ByVal pstrUsuarioUtilidades As String, ByVal pstrClave As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Operaciones_TiposNegocio))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Operaciones_TiposNegocio)) = New TaskCompletionSource(Of List(Of Operaciones_TiposNegocio))()
        objTaskComplete.TrySetResult(Operaciones_ConsultarTiposNegocio(pstrAplicacion, pstrVersion, pstrUsuarioUtilidades, pstrClave, pstrUsuario, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Operaciones_OtrosNegociosFiltrarSync(ByVal pstrEstado As String, ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_OtrosNegocios)
        Dim objTask As Task(Of List(Of Operaciones_OtrosNegocios)) = Me.Operaciones_OtrosNegociosFiltrarAsync(pstrEstado, pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Operaciones_OtrosNegociosFiltrarAsync(ByVal pstrEstado As String, ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Operaciones_OtrosNegocios))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Operaciones_OtrosNegocios)) = New TaskCompletionSource(Of List(Of Operaciones_OtrosNegocios))()
        objTaskComplete.TrySetResult(Operaciones_OtrosNegociosFiltrar(pstrEstado, pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Operaciones_OtrosNegociosConsultarSync(ByVal pintID As Nullable(Of Integer), ByVal pstrReferencia As String, ByVal pstrTipoRegistro As String, ByVal pstrEstado As String, ByVal pstrTipoNegocio As String, ByVal pstrTipoOrigen As String, ByVal pstrTipoOperacion As String, ByVal pstrCliente As String, ByVal pstrNemotecnico As String, ByVal pdtmFechaOperacion As Nullable(Of DateTime), ByVal pdtmFechaCumplimiento As Nullable(Of DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_OtrosNegocios)
        Dim objTask As Task(Of List(Of Operaciones_OtrosNegocios)) = Me.Operaciones_OtrosNegociosConsultarAsync(pintID, pstrReferencia, pstrTipoRegistro, pstrEstado, pstrTipoNegocio, pstrTipoOrigen, pstrTipoOperacion, pstrCliente, pstrNemotecnico, pdtmFechaOperacion, pdtmFechaCumplimiento, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Operaciones_OtrosNegociosConsultarAsync(ByVal pintID As Nullable(Of Integer), ByVal pstrReferencia As String, ByVal pstrTipoRegistro As String, ByVal pstrEstado As String, ByVal pstrTipoNegocio As String, ByVal pstrTipoOrigen As String, ByVal pstrTipoOperacion As String, ByVal pstrCliente As String, ByVal pstrNemotecnico As String, ByVal pdtmFechaOperacion As Nullable(Of DateTime), ByVal pdtmFechaCumplimiento As Nullable(Of DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Operaciones_OtrosNegocios))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Operaciones_OtrosNegocios)) = New TaskCompletionSource(Of List(Of Operaciones_OtrosNegocios))()
        objTaskComplete.TrySetResult(Operaciones_OtrosNegociosConsultar(pintID, pstrReferencia, pstrTipoRegistro, pstrEstado, pstrTipoNegocio, pstrTipoOrigen, pstrTipoOperacion, pstrCliente, pstrNemotecnico, pdtmFechaOperacion, pdtmFechaCumplimiento, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    <Query(HasSideEffects:=True)>
    Public Function Operaciones_OtrosNegociosValidarSync(ByVal pintID As Integer,
                                                     ByVal pstrReferencia As String,
                                                     ByVal pstrEstado As String,
                                                     ByVal pstrTipoNegocio As String,
                                                     ByVal pstrTipoOrigen As String,
                                                     ByVal plngIDComitente As String,
                                                     ByVal plngIDOrdenante As String,
                                                     ByVal pstrDeposito As String,
                                                     ByVal plngIDCuentaDeposito As Nullable(Of Integer),
                                                     ByVal pintIDContraparte As Nullable(Of Integer),
                                                     ByVal pintIDCuentaDepositoContraparte As Nullable(Of Integer),
                                                     ByVal pstrClasificacionInversion As String,
                                                     ByVal pintIDPagador As Nullable(Of Integer),
                                                     ByVal pstrTipoOperacion As String,
                                                     ByVal pdtmFechaOperacion As Nullable(Of DateTime),
                                                     ByVal pdtmFechaCumplimiento As Nullable(Of DateTime),
                                                     ByVal pdtmFechaVencimientoOperacion As Nullable(Of DateTime),
                                                     ByVal pstrNemotecnico As String,
                                                     ByVal pstrISIN As String,
                                                     ByVal pdtmFechaEmision As Nullable(Of DateTime),
                                                     ByVal pdtmFechaVencimiento As Nullable(Of DateTime),
                                                     ByVal pstrModalidad As String,
                                                     ByVal pdblTasaFacial As Nullable(Of Double),
                                                     ByVal pstrIndicadorEconomico As String,
                                                     ByVal pdblPuntosIndicador As Nullable(Of Double),
                                                     ByVal pstrTipoCumplimiento As String,
                                                     ByVal pstrMercado As String,
                                                     ByVal pdblNominal As Nullable(Of Double),
                                                     ByVal pintIDMoneda As Nullable(Of Integer),
                                                     ByVal pdblTasaCambioConversion As Nullable(Of Double),
                                                     ByVal pdblPrecioSucio As Nullable(Of Double),
                                                     ByVal pdblTasaNegociacionYield As Nullable(Of Double),
                                                     ByVal pdblValorGiroBruto As Nullable(Of Double),
                                                     ByVal pdblTasaPactadaRepo As Nullable(Of Double),
                                                     ByVal pdblHaircut As Nullable(Of Double),
                                                     ByVal pdblPorcentajeComision As Nullable(Of Double),
                                                     ByVal pdblValorComision As Nullable(Of Double),
                                                     ByVal pdblIVA As Nullable(Of Double),
                                                     ByVal pstrTipoReteFuente As String,
                                                     ByVal pdblRetencion As Nullable(Of Double),
                                                     ByVal pdblValorNeto As Nullable(Of Double),
                                                     ByVal pdblValorRegresoRepo As Nullable(Of Double),
                                                     ByVal pstrObservaciones As String,
                                                     ByVal pintPaisNegociacion As Nullable(Of Integer),
                                                     ByVal pstrTipoRepo As String,
                                                     ByVal pstrTipoRegistro As String,
                                                     ByVal pstrXMLReceptores As String,
                                                     ByVal pstrUsuario As String,
                                                     ByVal pstrUsuarioWindows As String,
                                                     ByVal pstrMaquina As String,
                                                     ByVal pdblValorNetoCop As Nullable(Of Double),
                                                     ByVal plogTipoBanRep As Nullable(Of Boolean),
                                                     ByVal pdblValorFlujoIntermedio As Nullable(Of Double),
                                                     ByVal plngIDOrdenOrigen As Nullable(Of Integer),
                                                     ByVal plngIDComitenteOtro As String,
                                                     ByVal pstrTipo As String,
                                                     ByVal pstrDepositoContraparte As String,
                                                     ByVal plngIDCuentaDepositoContraparte As Nullable(Of Integer),
                                                     ByVal pdblTasaNeta As Nullable(Of Double),
                                                     ByVal pdblValorGiroBrutoCOP As Nullable(Of Double),
                                                     ByVal pdblValorComisionCOP As Nullable(Of Double),
                                                     ByVal pdblValorRegresoRepoMoneda As Nullable(Of Double),
                                                     ByVal pstrInfoConexion As String,
                                                     ByVal plogOperacionAplazada As Nullable(Of Boolean),
                                                     ByVal pdblCantidadAplazada As System.Nullable(Of System.Double),
                                                     ByVal pdtmFechaCumplimientoAplazada As System.Nullable(Of System.DateTime)) As List(Of Operaciones_RespuestaValidacion)
        Dim strReceptoresXML = HttpUtility.UrlDecode(pstrXMLReceptores)
        Dim strObservaciones = HttpUtility.UrlDecode(pstrObservaciones)

        Dim objTask As Task(Of List(Of Operaciones_RespuestaValidacion)) = Me.Operaciones_OtrosNegociosValidarAsync(pintID, pstrReferencia, pstrEstado, pstrTipoNegocio, pstrTipoOrigen, plngIDComitente,
                                                                                               plngIDOrdenante, pstrDeposito, plngIDCuentaDeposito, pintIDContraparte, pintIDCuentaDepositoContraparte, pstrClasificacionInversion,
                                                                                               pintIDPagador, pstrTipoOperacion, pdtmFechaOperacion, pdtmFechaCumplimiento, pdtmFechaVencimientoOperacion,
                                                                                               pstrNemotecnico, pstrISIN, pdtmFechaEmision, pdtmFechaVencimiento, pstrModalidad, pdblTasaFacial, pstrIndicadorEconomico, pdblPuntosIndicador,
                                                                                               pstrTipoCumplimiento, pstrMercado, pdblNominal, pintIDMoneda, pdblTasaCambioConversion, pdblPrecioSucio, pdblTasaNegociacionYield, pdblValorGiroBruto,
                                                                                               pdblTasaPactadaRepo, pdblHaircut, pdblPorcentajeComision, pdblValorComision, pdblIVA, pstrTipoReteFuente,
                                                                                               pdblRetencion, pdblValorNeto, pdblValorRegresoRepo, strObservaciones, pintPaisNegociacion,
                                                                                               pstrTipoRepo, pstrTipoRegistro, strReceptoresXML, pstrUsuario, pstrUsuarioWindows, pstrMaquina,
                                                                                               pdblValorNetoCop, plogTipoBanRep, pdblValorFlujoIntermedio, plngIDOrdenOrigen, plngIDComitenteOtro, pstrTipo,
                                                                                               pstrDepositoContraparte, plngIDCuentaDepositoContraparte, pdblTasaNeta, pdblValorGiroBrutoCOP,
                                                                                               pdblValorComisionCOP, pdblValorRegresoRepoMoneda, pstrInfoConexion, plogOperacionAplazada, pdblCantidadAplazada,
                                                                                                                    pdtmFechaCumplimientoAplazada)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Operaciones_OtrosNegociosValidarAsync(ByVal pintID As Integer,
                                                     ByVal pstrReferencia As String,
                                                     ByVal pstrEstado As String,
                                                     ByVal pstrTipoNegocio As String,
                                                     ByVal pstrTipoOrigen As String,
                                                     ByVal plngIDComitente As String,
                                                     ByVal plngIDOrdenante As String,
                                                     ByVal pstrDeposito As String,
                                                     ByVal plngIDCuentaDeposito As Nullable(Of Integer),
                                                     ByVal pintIDContraparte As Nullable(Of Integer),
                                                     ByVal pintIDCuentaDepositoContraparte As Nullable(Of Integer),
                                                     ByVal pstrClasificacionInversion As String,
                                                     ByVal pintIDPagador As Nullable(Of Integer),
                                                     ByVal pstrTipoOperacion As String,
                                                     ByVal pdtmFechaOperacion As Nullable(Of DateTime),
                                                     ByVal pdtmFechaCumplimiento As Nullable(Of DateTime),
                                                     ByVal pdtmFechaVencimientoOperacion As Nullable(Of DateTime),
                                                     ByVal pstrNemotecnico As String,
                                                     ByVal pstrISIN As String,
                                                     ByVal pdtmFechaEmision As Nullable(Of DateTime),
                                                     ByVal pdtmFechaVencimiento As Nullable(Of DateTime),
                                                     ByVal pstrModalidad As String,
                                                     ByVal pdblTasaFacial As Nullable(Of Double),
                                                     ByVal pstrIndicadorEconomico As String,
                                                     ByVal pdblPuntosIndicador As Nullable(Of Double),
                                                     ByVal pstrTipoCumplimiento As String,
                                                     ByVal pstrMercado As String,
                                                     ByVal pdblNominal As Nullable(Of Double),
                                                     ByVal pintIDMoneda As Nullable(Of Integer),
                                                     ByVal pdblTasaCambioConversion As Nullable(Of Double),
                                                     ByVal pdblPrecioSucio As Nullable(Of Double),
                                                     ByVal pdblTasaNegociacionYield As Nullable(Of Double),
                                                     ByVal pdblValorGiroBruto As Nullable(Of Double),
                                                     ByVal pdblTasaPactadaRepo As Nullable(Of Double),
                                                     ByVal pdblHaircut As Nullable(Of Double),
                                                     ByVal pdblPorcentajeComision As Nullable(Of Double),
                                                     ByVal pdblValorComision As Nullable(Of Double),
                                                     ByVal pdblIVA As Nullable(Of Double),
                                                     ByVal pstrTipoReteFuente As String,
                                                     ByVal pdblRetencion As Nullable(Of Double),
                                                     ByVal pdblValorNeto As Nullable(Of Double),
                                                     ByVal pdblValorRegresoRepo As Nullable(Of Double),
                                                     ByVal pstrObservaciones As String,
                                                     ByVal pintPaisNegociacion As Nullable(Of Integer),
                                                     ByVal pstrTipoRepo As String,
                                                     ByVal pstrTipoRegistro As String,
                                                     ByVal pstrXMLReceptores As String,
                                                     ByVal pstrUsuario As String,
                                                     ByVal pstrUsuarioWindows As String,
                                                     ByVal pstrMaquina As String,
                                                     ByVal pdblValorNetoCop As Nullable(Of Double),
                                                     ByVal plogTipoBanRep As Nullable(Of Boolean),
                                                     ByVal pdblValorFlujoIntermedio As Nullable(Of Double),
                                                     ByVal plngIDOrdenOrigen As Nullable(Of Integer),
                                                     ByVal plngIDComitenteOtro As String,
                                                     ByVal pstrTipo As String,
                                                     ByVal pstrDepositoContraparte As String,
                                                     ByVal plngIDCuentaDepositoContraparte As Nullable(Of Integer),
                                                     ByVal pdblTasaNeta As Nullable(Of Double),
                                                     ByVal pdblValorGiroBrutoCOP As Nullable(Of Double),
                                                     ByVal pdblValorComisionCOP As Nullable(Of Double),
                                                     ByVal pdblValorRegresoRepoMoneda As Nullable(Of Double),
                                                     ByVal pstrInfoConexion As String,
                                                     ByVal plogOperacionAplazada As Nullable(Of Boolean),
                                                     ByVal pdblCantidadAplazada As System.Nullable(Of System.Double),
                                                     ByVal pdtmFechaCumplimientoAplazada As System.Nullable(Of System.DateTime)) As Task(Of List(Of Operaciones_RespuestaValidacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Operaciones_RespuestaValidacion)) = New TaskCompletionSource(Of List(Of Operaciones_RespuestaValidacion))()
        objTaskComplete.TrySetResult(Operaciones_OtrosNegociosValidar(pintID, pstrReferencia, pstrEstado, pstrTipoNegocio, pstrTipoOrigen, plngIDComitente,
                                                                                               plngIDOrdenante, pstrDeposito, plngIDCuentaDeposito, pintIDContraparte, pintIDCuentaDepositoContraparte, pstrClasificacionInversion,
                                                                                               pintIDPagador, pstrTipoOperacion, pdtmFechaOperacion, pdtmFechaCumplimiento, pdtmFechaVencimientoOperacion,
                                                                                               pstrNemotecnico, pstrISIN, pdtmFechaEmision, pdtmFechaVencimiento, pstrModalidad, pdblTasaFacial, pstrIndicadorEconomico, pdblPuntosIndicador,
                                                                                               pstrTipoCumplimiento, pstrMercado, pdblNominal, pintIDMoneda, pdblTasaCambioConversion, pdblPrecioSucio, pdblTasaNegociacionYield, pdblValorGiroBruto,
                                                                                               pdblTasaPactadaRepo, pdblHaircut, pdblPorcentajeComision, pdblValorComision, pdblIVA, pstrTipoReteFuente,
                                                                                               pdblRetencion, pdblValorNeto, pdblValorRegresoRepo, pstrObservaciones, pintPaisNegociacion,
                                                                                               pstrTipoRepo, pstrTipoRegistro, pstrXMLReceptores, pstrUsuario, pstrUsuarioWindows, pstrMaquina,
                                                                                               pdblValorNetoCop, plogTipoBanRep, pdblValorFlujoIntermedio, plngIDOrdenOrigen, plngIDComitenteOtro, pstrTipo,
                                                                                               pstrDepositoContraparte, plngIDCuentaDepositoContraparte, pdblTasaNeta, pdblValorGiroBrutoCOP,
                                                                                               pdblValorComisionCOP, pdblValorRegresoRepoMoneda, pstrInfoConexion, plogOperacionAplazada, pdblCantidadAplazada,
                                                                                                pdtmFechaCumplimientoAplazada))
        Return (objTaskComplete.Task)
    End Function

    Public Function Operaciones_OtrosNegociosAnularSync(ByVal pintID As Integer,
                                                     ByVal pstrObservaciones As String,
                                                     ByVal pstrUsuario As String,
                                                     ByVal pstrUsuarioWindows As String,
                                                     ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_RespuestaValidacion)
        Dim objTask As Task(Of List(Of Operaciones_RespuestaValidacion)) = Me.Operaciones_OtrosNegociosAnularAsync(pintID, pstrObservaciones, pstrUsuario, pstrUsuarioWindows, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Operaciones_OtrosNegociosAnularAsync(ByVal pintID As Integer,
                                                     ByVal pstrObservaciones As String,
                                                     ByVal pstrUsuario As String,
                                                     ByVal pstrUsuarioWindows As String,
                                                     ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Operaciones_RespuestaValidacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Operaciones_RespuestaValidacion)) = New TaskCompletionSource(Of List(Of Operaciones_RespuestaValidacion))()
        objTaskComplete.TrySetResult(Operaciones_OtrosNegociosAnular(pintID, pstrObservaciones, pstrUsuario, pstrUsuarioWindows, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Operaciones_OtrosNegociosLiqxDifeSync(ByVal pintID As Integer,
                                                     ByVal pstrObservaciones As String,
                                                     ByVal pstrUsuario As String,
                                                     ByVal pstrUsuarioWindows As String,
                                                     ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_RespuestaValidacion)
        Dim objTask As Task(Of List(Of Operaciones_RespuestaValidacion)) = Me.Operaciones_OtrosNegociosLiqxDifeAsync(pintID, pstrObservaciones, pstrUsuario, pstrUsuarioWindows, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Operaciones_OtrosNegociosLiqxDifeAsync(ByVal pintID As Integer,
                                                     ByVal pstrObservaciones As String,
                                                     ByVal pstrUsuario As String,
                                                     ByVal pstrUsuarioWindows As String,
                                                     ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Operaciones_RespuestaValidacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Operaciones_RespuestaValidacion)) = New TaskCompletionSource(Of List(Of Operaciones_RespuestaValidacion))()
        objTaskComplete.TrySetResult(Operaciones_OtrosNegociosLiqxDife(pintID, pstrObservaciones, pstrUsuario, pstrUsuarioWindows, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Operaciones_OtrosNegociosReceptoresSync(ByVal pintIDLiquidacion As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_ReceptoresOtrosNegocios)
        Dim objTask As Task(Of List(Of Operaciones_ReceptoresOtrosNegocios)) = Me.Operaciones_OtrosNegociosReceptoresAsync(pintIDLiquidacion, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Operaciones_OtrosNegociosReceptoresAsync(ByVal pintIDLiquidacion As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Operaciones_ReceptoresOtrosNegocios))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Operaciones_ReceptoresOtrosNegocios)) = New TaskCompletionSource(Of List(Of Operaciones_ReceptoresOtrosNegocios))()
        objTaskComplete.TrySetResult(Operaciones_OtrosNegociosReceptores(pintIDLiquidacion, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Operaciones_EntidadDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_EntidadDefecto)
        Dim objTask As Task(Of List(Of Operaciones_EntidadDefecto)) = Me.Operaciones_EntidadDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Operaciones_EntidadDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Operaciones_EntidadDefecto))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Operaciones_EntidadDefecto)) = New TaskCompletionSource(Of List(Of Operaciones_EntidadDefecto))()
        objTaskComplete.TrySetResult(Operaciones_EntidadDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Operaciones_OtrosNegociosReceptoresClienteSync(ByVal pstrCliente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_ReceptoresOtrosNegocios)
        Dim objTask As Task(Of List(Of Operaciones_ReceptoresOtrosNegocios)) = Me.Operaciones_OtrosNegociosReceptoresClienteAsync(pstrCliente, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Operaciones_OtrosNegociosReceptoresClienteAsync(ByVal pstrCliente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Operaciones_ReceptoresOtrosNegocios))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Operaciones_ReceptoresOtrosNegocios)) = New TaskCompletionSource(Of List(Of Operaciones_ReceptoresOtrosNegocios))()
        objTaskComplete.TrySetResult(Operaciones_OtrosNegociosReceptoresCliente(pstrCliente, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    <Query(HasSideEffects:=True)> _
    Public Function Operaciones_OtrosNegociosCalcularSync(ByVal pstrTipoCalculo As String,
                                                     ByVal pstrTipoNegocio As String,
                                                     ByVal pstrTipoOrigen As String,
                                                     ByVal plngIDComitente As String,
                                                     ByVal pstrTipoOperacion As String,
                                                     ByVal pdtmFechaOperacion As Nullable(Of DateTime),
                                                     ByVal pdtmFechaCumplimiento As Nullable(Of DateTime),
                                                     ByVal pdtmFechaVencimientoOperacion As Nullable(Of DateTime),
                                                     ByVal pstrNemotecnico As String,
                                                     ByVal pstrISIN As String,
                                                     ByVal pdtmFechaEmision As Nullable(Of DateTime),
                                                     ByVal pdtmFechaVencimiento As Nullable(Of DateTime),
                                                     ByVal pstrModalidad As String,
                                                     ByVal pdblTasaFacial As Nullable(Of Double),
                                                     ByVal pstrIndicadorEconomico As String,
                                                     ByVal pdblPuntosIndicador As Nullable(Of Double),
                                                     ByVal pdblNominal As Nullable(Of Double),
                                                     ByVal pintIDMoneda As Nullable(Of Integer),
                                                     ByVal pdblTasaCambioConversion As Nullable(Of Double),
                                                     ByVal pdblPrecioSucio As Nullable(Of Double),
                                                     ByVal pdblTasaNegociacionYield As Nullable(Of Double),
                                                     ByVal pdblValorGiroBruto As Nullable(Of Double),
                                                     ByVal pdblTasaPactadaRepo As Nullable(Of Double),
                                                     ByVal pdblHaircut As Nullable(Of Double),
                                                     ByVal pdblPorcentajeComision As Nullable(Of Double),
                                                     ByVal pdblValorComision As Nullable(Of Double),
                                                     ByVal pdblIVA As Nullable(Of Double),
                                                     ByVal pstrTipoReteFuente As String,
                                                     ByVal pdblRetencion As Nullable(Of Double),
                                                     ByVal pdblValorNeto As Nullable(Of Double),
                                                     ByVal pdblValorRegresoRepo As Nullable(Of Double),
                                                     ByVal pintPaisNegociacion As Nullable(Of Integer),
                                                     ByVal pstrUsuario As String,
                                                     ByVal pstrUsuarioWindows As String,
                                                     ByVal pstrMaquina As String,
                                                     ByVal pdblValorNetoCop As Nullable(Of Double),
                                                     ByVal plogTipoBanRep As Nullable(Of Boolean),
                                                     ByVal pdblValorFlujoIntermedio As Nullable(Of Double),
                                                     ByVal pstrTipo As String,
                                                     ByVal pstrTipoRepo As String,
                                                     ByVal pdblTasaNeta As Nullable(Of Double),
                                                     ByVal pdblValorGiroBrutoCOP As Nullable(Of Double),
                                                     ByVal pdblValorComisionCOP As Nullable(Of Double),
                                                     ByVal pdblValorRegresoRepoMoneda As Nullable(Of Double), ByVal pstrInfoConexion As String) As List(Of Operaciones_OtrosNegociosCalculos)
        Dim objTask As Task(Of List(Of Operaciones_OtrosNegociosCalculos)) = Me.Operaciones_OtrosNegociosCalcularAsync(pstrTipoCalculo, pstrTipoNegocio, pstrTipoOrigen, plngIDComitente,
                                                                        pstrTipoOperacion, pdtmFechaOperacion, pdtmFechaCumplimiento, pdtmFechaVencimientoOperacion,
                                                                        pstrNemotecnico, pstrISIN, pdtmFechaEmision, pdtmFechaVencimiento, pstrModalidad, pdblTasaFacial, pstrIndicadorEconomico, pdblPuntosIndicador,
                                                                        pdblNominal, pintIDMoneda, pdblTasaCambioConversion, pdblPrecioSucio, pdblTasaNegociacionYield, pdblValorGiroBruto,
                                                                        pdblTasaPactadaRepo, pdblHaircut, pdblPorcentajeComision, pdblValorComision, pdblIVA, pstrTipoReteFuente,
                                                                        pdblRetencion, pdblValorNeto, pdblValorRegresoRepo, pintPaisNegociacion,
                                                                        pstrUsuario, pstrUsuarioWindows, pstrMaquina, pdblValorNetoCop,
                                                                        plogTipoBanRep, pdblValorFlujoIntermedio, pstrTipo, pstrTipoRepo, pdblTasaNeta, pdblValorGiroBrutoCOP, pdblValorComisionCOP,
                                                                        pdblValorRegresoRepoMoneda, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Operaciones_OtrosNegociosCalcularAsync(ByVal pstrTipoCalculo As String,
                                                     ByVal pstrTipoNegocio As String,
                                                     ByVal pstrTipoOrigen As String,
                                                     ByVal plngIDComitente As String,
                                                     ByVal pstrTipoOperacion As String,
                                                     ByVal pdtmFechaOperacion As Nullable(Of DateTime),
                                                     ByVal pdtmFechaCumplimiento As Nullable(Of DateTime),
                                                     ByVal pdtmFechaVencimientoOperacion As Nullable(Of DateTime),
                                                     ByVal pstrNemotecnico As String,
                                                     ByVal pstrISIN As String,
                                                     ByVal pdtmFechaEmision As Nullable(Of DateTime),
                                                     ByVal pdtmFechaVencimiento As Nullable(Of DateTime),
                                                     ByVal pstrModalidad As String,
                                                     ByVal pdblTasaFacial As Nullable(Of Double),
                                                     ByVal pstrIndicadorEconomico As String,
                                                     ByVal pdblPuntosIndicador As Nullable(Of Double),
                                                     ByVal pdblNominal As Nullable(Of Double),
                                                     ByVal pintIDMoneda As Nullable(Of Integer),
                                                     ByVal pdblTasaCambioConversion As Nullable(Of Double),
                                                     ByVal pdblPrecioSucio As Nullable(Of Double),
                                                     ByVal pdblTasaNegociacionYield As Nullable(Of Double),
                                                     ByVal pdblValorGiroBruto As Nullable(Of Double),
                                                     ByVal pdblTasaPactadaRepo As Nullable(Of Double),
                                                     ByVal pdblHaircut As Nullable(Of Double),
                                                     ByVal pdblPorcentajeComision As Nullable(Of Double),
                                                     ByVal pdblValorComision As Nullable(Of Double),
                                                     ByVal pdblIVA As Nullable(Of Double),
                                                     ByVal pstrTipoReteFuente As String,
                                                     ByVal pdblRetencion As Nullable(Of Double),
                                                     ByVal pdblValorNeto As Nullable(Of Double),
                                                     ByVal pdblValorRegresoRepo As Nullable(Of Double),
                                                     ByVal pintPaisNegociacion As Nullable(Of Integer),
                                                     ByVal pstrUsuario As String,
                                                     ByVal pstrUsuarioWindows As String,
                                                     ByVal pstrMaquina As String,
                                                     ByVal pdblValorNetoCop As Nullable(Of Double),
                                                     ByVal plogTipoBanRep As Nullable(Of Boolean),
                                                     ByVal pdblValorFlujoIntermedio As Nullable(Of Double),
                                                     ByVal pstrTipo As String,
                                                     ByVal pstrTipoRepo As String,
                                                     ByVal pdblTasaNeta As Nullable(Of Double),
                                                     ByVal pdblValorGiroBrutoCOP As Nullable(Of Double),
                                                     ByVal pdblValorComisionCOP As Nullable(Of Double),
                                                     ByVal pdblValorRegresoRepoMoneda As Nullable(Of Double), ByVal pstrInfoConexion As String) As Task(Of List(Of Operaciones_OtrosNegociosCalculos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Operaciones_OtrosNegociosCalculos)) = New TaskCompletionSource(Of List(Of Operaciones_OtrosNegociosCalculos))()
        objTaskComplete.TrySetResult(Operaciones_OtrosNegociosCalcular(pstrTipoCalculo, pstrTipoNegocio, pstrTipoOrigen, plngIDComitente,
                                                                        pstrTipoOperacion, pdtmFechaOperacion, pdtmFechaCumplimiento, pdtmFechaVencimientoOperacion,
                                                                        pstrNemotecnico, pstrISIN, pdtmFechaEmision, pdtmFechaVencimiento, pstrModalidad, pdblTasaFacial, pstrIndicadorEconomico, pdblPuntosIndicador,
                                                                        pdblNominal, pintIDMoneda, pdblTasaCambioConversion, pdblPrecioSucio, pdblTasaNegociacionYield, pdblValorGiroBruto,
                                                                        pdblTasaPactadaRepo, pdblHaircut, pdblPorcentajeComision, pdblValorComision, pdblIVA, pstrTipoReteFuente,
                                                                        pdblRetencion, pdblValorNeto, pdblValorRegresoRepo, pintPaisNegociacion,
                                                                        pstrUsuario, pstrUsuarioWindows, pstrMaquina, pdblValorNetoCop,
                                                                        plogTipoBanRep, pdblValorFlujoIntermedio, pstrTipo, pstrTipoRepo, pdblTasaNeta, pdblValorGiroBrutoCOP, pdblValorComisionCOP,
                                                                        pdblValorRegresoRepoMoneda, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Operaciones_OtrosNegociosConsultarTRMMonedaSync(ByVal pdtmFechaOperacion As DateTime, ByVal pintIDMoneda As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Double
        Dim objTask As Task(Of Double) = Me.Operaciones_OtrosNegociosConsultarTRMMonedaAsync(pdtmFechaOperacion, pintIDMoneda, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Operaciones_OtrosNegociosConsultarTRMMonedaAsync(ByVal pdtmFechaOperacion As DateTime, ByVal pintIDMoneda As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Double)
        Dim objTaskComplete As TaskCompletionSource(Of Double) = New TaskCompletionSource(Of Double)()
        objTaskComplete.TrySetResult(Operaciones_OtrosNegociosConsultarTRMMoneda(pdtmFechaOperacion, pintIDMoneda, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Operaciones_EntidadesCuentasDepositoSync(ByVal plogConsultarTodos As Boolean, ByVal pintIDEntidad As Integer, ByVal plogConsultarPorNroDocumento As Boolean, ByVal pstrNroDocumento As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_EntidadesCuentasDeposito)
        Dim objTask As Task(Of List(Of Operaciones_EntidadesCuentasDeposito)) = Me.Operaciones_EntidadesCuentasDepositoAsync(plogConsultarTodos, pintIDEntidad, plogConsultarPorNroDocumento, pstrNroDocumento, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Operaciones_EntidadesCuentasDepositoAsync(ByVal plogConsultarTodos As Boolean, ByVal pintIDEntidad As Integer, ByVal plogConsultarPorNroDocumento As Boolean, ByVal pstrNroDocumento As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Operaciones_EntidadesCuentasDeposito))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Operaciones_EntidadesCuentasDeposito)) = New TaskCompletionSource(Of List(Of Operaciones_EntidadesCuentasDeposito))()
        objTaskComplete.TrySetResult(Operaciones_EntidadesCuentasDeposito(plogConsultarTodos, pintIDEntidad, plogConsultarPorNroDocumento, pstrNroDocumento, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Operaciones_OtrosNegociosValidarEstadoSync(ByVal pintID As Integer,
                                                               ByVal pstrAccion As String,
                                                               ByVal pstrUsuario As String,
                                                               ByVal pstrUsuarioWindows As String,
                                                               ByVal pstrMaquina As String, ByVal pstrInfoConexion As String,
                                                               ByVal plogOperacionAplazada As System.Nullable(Of System.Boolean)) As List(Of Operaciones_RespuestaValidacion)
        Dim objTask As Task(Of List(Of Operaciones_RespuestaValidacion)) = Me.Operaciones_OtrosNegociosValidarEstadoAsync(pintID, pstrAccion, pstrUsuario, pstrUsuarioWindows, pstrMaquina, pstrInfoConexion, plogOperacionAplazada)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Operaciones_OtrosNegociosValidarEstadoAsync(ByVal pintID As Integer,
                                                                 ByVal pstrAccion As String,
                                                                 ByVal pstrUsuario As String,
                                                                 ByVal pstrUsuarioWindows As String,
                                                                 ByVal pstrMaquina As String,
                                                                 ByVal pstrInfoConexion As String,
                                                                 ByVal plogOperacionAplazada As System.Nullable(Of System.Boolean)) As Task(Of List(Of Operaciones_RespuestaValidacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Operaciones_RespuestaValidacion)) = New TaskCompletionSource(Of List(Of Operaciones_RespuestaValidacion))()
        objTaskComplete.TrySetResult(Operaciones_OtrosNegociosValidarEstado(pintID, pstrAccion, pstrUsuario, pstrUsuarioWindows, pstrMaquina, pstrInfoConexion, plogOperacionAplazada))
        Return (objTaskComplete.Task)
    End Function

    Public Function Operaciones_OtrosNegociosConsultarPrecioMercadoSync(ByVal pstrEspecie As String, ByVal pstrISIN As String, ByVal pdtmFechaOperacion As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Double
        Dim objTask As Task(Of Double) = Me.Operaciones_OtrosNegociosConsultarPrecioMercadoAsync(pstrEspecie, pstrISIN, pdtmFechaOperacion, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Operaciones_OtrosNegociosConsultarPrecioMercadoAsync(ByVal pstrEspecie As String, ByVal pstrISIN As String, ByVal pdtmFechaOperacion As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Double)
        Dim objTaskComplete As TaskCompletionSource(Of Double) = New TaskCompletionSource(Of Double)()
        objTaskComplete.TrySetResult(Operaciones_OtrosNegociosConsultarPrecioMercado(pstrEspecie, pstrISIN, pdtmFechaOperacion, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function HomologacionTipoOrigen_FiltrarSync(pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of HomologacionTipoOrigen)
        Dim objTask As Task(Of List(Of HomologacionTipoOrigen)) = Me.HomologacionTipoOrigen_FiltrarAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function HomologacionTipoOrigen_FiltrarAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of HomologacionTipoOrigen))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of HomologacionTipoOrigen)) = New TaskCompletionSource(Of List(Of HomologacionTipoOrigen))()
        objTaskComplete.TrySetResult(HomologacionTipoOrigen_Filtrar(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function HomologacionTipoOrigen_ConsultarSync(ByVal pstrTipoOrigenPrincipal As String, ByVal pstrTipoOrigenSecundario As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of HomologacionTipoOrigen)
        Dim objTask As Task(Of List(Of HomologacionTipoOrigen)) = Me.HomologacionTipoOrigen_ConsultarAsync(pstrTipoOrigenPrincipal, pstrTipoOrigenSecundario, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function HomologacionTipoOrigen_ConsultarAsync(ByVal pstrTipoOrigenPrincipal As String, ByVal pstrTipoOrigenSecundario As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of HomologacionTipoOrigen))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of HomologacionTipoOrigen)) = New TaskCompletionSource(Of List(Of HomologacionTipoOrigen))()
        objTaskComplete.TrySetResult(HomologacionTipoOrigen_Consultar(pstrTipoOrigenPrincipal, pstrTipoOrigenSecundario, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    <Query(HasSideEffects:=True)> _
    Public Function InmobiliariosValidarSync(ByVal pintID As Integer,
                                                     ByVal pstrCodigo As String,
                                                     ByVal pstrEstado As String,
                                                     ByVal pstrTipo As String,
                                                     ByVal pstrArea As String,
                                                     ByVal pstrCodigoCatastro As String,
                                                     ByVal pstrDireccion As String,
                                                     ByVal plngIDComitente As String,
                                                     ByVal pdtmFechaCompra As Nullable(Of DateTime),
                                                     ByVal pdblValorCompra As Nullable(Of Double),
                                                     ByVal pintMonedaNegociacion As Nullable(Of Integer),
                                                     ByVal pintMonedaLocal As Nullable(Of Integer),
                                                     ByVal pdblTasaCambio As Nullable(Of Double),
                                                     ByVal pstrDescripcion As String,
                                                     ByVal pdtmFechaCumplimiento As Nullable(Of DateTime),
                                                     ByVal pdtmFechaUltimaValoracion As Nullable(Of DateTime),
                                                     ByVal pdblValorCompraActualizado As Nullable(Of Double),
                                                     ByVal pdblValorOperacion As Nullable(Of Double),
                                                     ByVal pstrUsuario As String,
                                                     ByVal pstrUsuarioWindows As String,
                                                     ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_RespuestaValidacion)
        Dim objTask As Task(Of List(Of Operaciones_RespuestaValidacion)) = InmobiliariosValidarAsync(pintID, pstrCodigo, pstrEstado, pstrTipo, pstrArea, pstrCodigoCatastro, pstrDireccion, plngIDComitente,
                                                                                  pdtmFechaCompra, pdblValorCompra, pintMonedaNegociacion, pintMonedaLocal, pdblTasaCambio, pstrDescripcion,
                                                                                  pdtmFechaCumplimiento, pdtmFechaUltimaValoracion, pdblValorCompraActualizado, pdblValorOperacion,
                                                                                  pstrUsuario, pstrUsuarioWindows, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function InmobiliariosValidarAsync(ByVal pintID As Integer,
                                                     ByVal pstrCodigo As String,
                                                     ByVal pstrEstado As String,
                                                     ByVal pstrTipo As String,
                                                     ByVal pstrArea As String,
                                                     ByVal pstrCodigoCatastro As String,
                                                     ByVal pstrDireccion As String,
                                                     ByVal plngIDComitente As String,
                                                     ByVal pdtmFechaCompra As Nullable(Of DateTime),
                                                     ByVal pdblValorCompra As Nullable(Of Double),
                                                     ByVal pintMonedaNegociacion As Nullable(Of Integer),
                                                     ByVal pintMonedaLocal As Nullable(Of Integer),
                                                     ByVal pdblTasaCambio As Nullable(Of Double),
                                                     ByVal pstrDescripcion As String,
                                                     ByVal pdtmFechaCumplimiento As Nullable(Of DateTime),
                                                     ByVal pdtmFechaUltimaValoracion As Nullable(Of DateTime),
                                                     ByVal pdblValorCompraActualizado As Nullable(Of Double),
                                                     ByVal pdblValorOperacion As Nullable(Of Double),
                                                     ByVal pstrUsuario As String,
                                                     ByVal pstrUsuarioWindows As String,
                                                     ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Operaciones_RespuestaValidacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Operaciones_RespuestaValidacion)) = New TaskCompletionSource(Of List(Of Operaciones_RespuestaValidacion))()
        objTaskComplete.TrySetResult(InmobiliariosValidar(pintID, pstrCodigo, pstrEstado, pstrTipo, pstrArea, pstrCodigoCatastro, pstrDireccion, plngIDComitente,
                                                                                  pdtmFechaCompra, pdblValorCompra, pintMonedaNegociacion, pintMonedaLocal, pdblTasaCambio, pstrDescripcion,
                                                                                  pdtmFechaCumplimiento, pdtmFechaUltimaValoracion, pdblValorCompraActualizado, pdblValorOperacion,
                                                                                  pstrUsuario, pstrUsuarioWindows, pstrMaquina, pstrInfoConexion))

        Return (objTaskComplete.Task)
    End Function

    Public Function InmobiliariosAnularSync(ByVal pintID As Integer,
                                                   ByVal pstrObservaciones As String,
                                                   ByVal pstrUsuario As String,
                                                   ByVal pstrUsuarioWindows As String,
                                                   ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_RespuestaValidacion)
        Dim objTask As Task(Of List(Of Operaciones_RespuestaValidacion)) = Me.InmobiliariosAnularAsync(pintID, pstrObservaciones, pstrUsuario, pstrUsuarioWindows, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function InmobiliariosAnularAsync(ByVal pintID As Integer,
                                                     ByVal pstrObservaciones As String,
                                                     ByVal pstrUsuario As String,
                                                     ByVal pstrUsuarioWindows As String,
                                                     ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Operaciones_RespuestaValidacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Operaciones_RespuestaValidacion)) = New TaskCompletionSource(Of List(Of Operaciones_RespuestaValidacion))()
        objTaskComplete.TrySetResult(InmobiliariosAnular(pintID, pstrObservaciones, pstrUsuario, pstrUsuarioWindows, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function InmobiliariosConsultarSync(ByVal pintID As Nullable(Of Integer), ByVal pstrCodigo As String, ByVal pdtmFechaCompra As Nullable(Of DateTime), ByVal plngIDComitente As String, ByVal pstrTipo As String, ByVal pstrEstado As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Inmobiliarios)
        Dim objTask As Task(Of List(Of Inmobiliarios)) = Me.InmobiliariosConsultarAsync(pintID, pstrCodigo, pdtmFechaCompra, plngIDComitente, pstrTipo, pstrEstado, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function InmobiliariosConsultarAsync(ByVal pintID As Nullable(Of Integer), ByVal pstrCodigo As String, ByVal pdtmFechaCompra As Nullable(Of DateTime), ByVal plngIDComitente As String, ByVal pstrTipo As String, ByVal pstrEstado As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Inmobiliarios))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Inmobiliarios)) = New TaskCompletionSource(Of List(Of Inmobiliarios))()
        objTaskComplete.TrySetResult(InmobiliariosConsultar(pintID, pstrCodigo, pdtmFechaCompra, plngIDComitente, pstrTipo, pstrEstado, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function InmobiliariosFiltrarSync(pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Inmobiliarios)
        Dim objTask As Task(Of List(Of Inmobiliarios)) = Me.InmobiliariosFiltrarAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function InmobiliariosFiltrarAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Inmobiliarios))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Inmobiliarios)) = New TaskCompletionSource(Of List(Of Inmobiliarios))()
        objTaskComplete.TrySetResult(InmobiliariosFiltrar(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function InmobiliariosCombosSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_Combos)
        Dim objTask As Task(Of List(Of Operaciones_Combos)) = Me.InmobiliariosCombosAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function InmobiliariosCombosAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Operaciones_Combos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Operaciones_Combos)) = New TaskCompletionSource(Of List(Of Operaciones_Combos))()
        objTaskComplete.TrySetResult(InmobiliariosCombos(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function InmobiliariosDetalles_ConsultarSync(ByVal pintIDInmobiliario As Integer, ByVal pdtmFechaInicial As Nullable(Of DateTime), ByVal pdtmFechaFinal As Nullable(Of DateTime), ByVal pintIDConcepto As Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Inmobiliarios_Detalles)
        Dim objTask As Task(Of List(Of Inmobiliarios_Detalles)) = Me.InmobiliariosDetalles_ConsultarAsync(pintIDInmobiliario, pdtmFechaInicial, pdtmFechaFinal, pintIDConcepto, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function InmobiliariosDetalles_ConsultarAsync(ByVal pintIDInmobiliario As Integer, ByVal pdtmFechaInicial As Nullable(Of DateTime), ByVal pdtmFechaFinal As Nullable(Of DateTime), ByVal pintIDConcepto As Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Inmobiliarios_Detalles))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Inmobiliarios_Detalles)) = New TaskCompletionSource(Of List(Of Inmobiliarios_Detalles))()
        objTaskComplete.TrySetResult(InmobiliariosDetalles_Consultar(pintIDInmobiliario, pdtmFechaInicial, pdtmFechaFinal, pintIDConcepto, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function InmobiliariosDetalles_ActualizarSync(ByVal pintID As Integer,
                                                     ByVal pintIDInmobiliario As Integer,
                                                     ByVal pdtmFechaMovimiento As DateTime,
                                                     ByVal pdblValor As Double,
                                                     ByVal pdblBaseIva As Double,
                                                     ByVal pstrTipoMovimiento As String,
                                                     ByVal plngIDDocumento As Nullable(Of Integer),
                                                     ByVal pstrTipoTesoreria As String,
                                                     ByVal pstrNombreConsecutivo As String,
                                                     ByVal pstrUsuario As String,
                                                     ByVal pstrUsuarioWindows As String,
                                                     ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_RespuestaValidacion)
        Dim objTask As Task(Of List(Of Operaciones_RespuestaValidacion)) = Me.InmobiliariosDetalles_ActualizarAsync(pintID,
                                                                                             pintIDInmobiliario,
                                                                                             pdtmFechaMovimiento,
                                                                                             pdblValor,
                                                                                             pdblBaseIva,
                                                                                             pstrTipoMovimiento,
                                                                                             plngIDDocumento,
                                                                                             pstrTipoTesoreria,
                                                                                             pstrNombreConsecutivo,
                                                                                             pstrUsuario,
                                                                                             pstrUsuarioWindows,
                                                                                             pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function InmobiliariosDetalles_ActualizarAsync(ByVal pintID As Integer,
                                                     ByVal pintIDInmobiliario As Integer,
                                                     ByVal pdtmFechaMovimiento As DateTime,
                                                     ByVal pdblValor As Double,
                                                     ByVal pdblBaseIva As Double,
                                                     ByVal pstrTipoMovimiento As String,
                                                     ByVal plngIDDocumento As Nullable(Of Integer),
                                                     ByVal pstrTipoTesoreria As String,
                                                     ByVal pstrNombreConsecutivo As String,
                                                     ByVal pstrUsuario As String,
                                                     ByVal pstrUsuarioWindows As String,
                                                     ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Operaciones_RespuestaValidacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Operaciones_RespuestaValidacion)) = New TaskCompletionSource(Of List(Of Operaciones_RespuestaValidacion))()
        objTaskComplete.TrySetResult(InmobiliariosDetalles_Actualizar(pintID,
                                                                    pintIDInmobiliario,
                                                                    pdtmFechaMovimiento,
                                                                    pdblValor,
                                                                    pdblBaseIva,
                                                                    pstrTipoMovimiento,
                                                                    plngIDDocumento,
                                                                    pstrTipoTesoreria,
                                                                    pstrNombreConsecutivo,
                                                                    pstrUsuario,
                                                                    pstrUsuarioWindows,
                                                                    pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function InmobiliariosDetalles_EliminarSync(ByVal pintID As Integer,
                                                       ByVal pintIDInmobiliario As Integer,
                                                       ByVal pstrUsuario As String,
                                                       ByVal pstrUsuarioWindows As String,
                                                       ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_RespuestaValidacion)
        Dim objTask As Task(Of List(Of Operaciones_RespuestaValidacion)) = Me.InmobiliariosDetalles_EliminarAsync(pintID,
                                                                                                                  pintIDInmobiliario,
                                                                                                                  pstrUsuario,
                                                                                                                  pstrUsuarioWindows,
                                                                                                                  pstrMaquina,
                                                                                                                  pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function InmobiliariosDetalles_EliminarAsync(ByVal pintID As Integer,
                                                         ByVal pintIDInmobiliario As Integer,
                                                         ByVal pstrUsuario As String,
                                                         ByVal pstrUsuarioWindows As String,
                                                         ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Operaciones_RespuestaValidacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Operaciones_RespuestaValidacion)) = New TaskCompletionSource(Of List(Of Operaciones_RespuestaValidacion))()
        objTaskComplete.TrySetResult(InmobiliariosDetalles_Eliminar(pintID,
                                                                    pintIDInmobiliario,
                                                                    pstrUsuario,
                                                                    pstrUsuarioWindows,
                                                                    pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function InmobiliariosDetalles_ValidarEstadoSync(ByVal pintID As Integer,
                                                       ByVal pintIDInmobiliario As Integer,
                                                       ByVal pstrAccion As String,
                                                       ByVal pstrUsuario As String,
                                                       ByVal pstrUsuarioWindows As String,
                                                       ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_RespuestaValidacion)
        Dim objTask As Task(Of List(Of Operaciones_RespuestaValidacion)) = Me.InmobiliariosDetalles_ValidarEstadoAsync(pintID,
                                                                                                                  pintIDInmobiliario,
                                                                                                                  pstrAccion,
                                                                                                                  pstrUsuario,
                                                                                                                  pstrUsuarioWindows,
                                                                                                                  pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function InmobiliariosDetalles_ValidarEstadoAsync(ByVal pintID As Integer,
                                                         ByVal pintIDInmobiliario As Integer,
                                                         ByVal pstrAccion As String,
                                                         ByVal pstrUsuario As String,
                                                         ByVal pstrUsuarioWindows As String,
                                                         ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of Operaciones_RespuestaValidacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of Operaciones_RespuestaValidacion)) = New TaskCompletionSource(Of List(Of Operaciones_RespuestaValidacion))()
        objTaskComplete.TrySetResult(InmobiliariosDetalles_ValidarEstado(pintID,
                                                                    pintIDInmobiliario,
                                                                    pstrAccion,
                                                                    pstrUsuario,
                                                                    pstrUsuarioWindows,
                                                                    pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

End Class
