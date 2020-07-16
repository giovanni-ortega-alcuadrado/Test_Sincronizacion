Imports System.Configuration
Imports A2.OyD.OYDServer.RIA.Web.CFMaestros
Imports A2.OyD.Infraestructura
''' <summary>
''' Esta clase fue creada para poder extender los metodos new() de la clase oyddatacontext y poder descifrar la cadena de conexión.
''' Cuando se realizan cambios en el diseñador de consultas relacional (.dbml) los metodos new() vuelven a ser automáticamente generados y se presenta un error de que 
''' existen múltiples definiciones con firmas idénticas (‘Public Sub New()’ has multiple definitions with identical signatures).
''' LA SOLUCION es eliminar de la clase oyddatacontext todos los métodos New() y dejar los que se encuentran en esta clase.
''' </summary>
''' <remarks></remarks>
''' 


Partial Public Class CF_MaestrosDataContext
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
            ManejarError(ex, "CF_MaestrosDataContext", "SubmitChanges")
        End Try
    End Sub

#Region "Calificadoras"

    Private Sub InsertCalificadoras(ByVal obj As Calificadoras)
        Dim p1 As Integer = obj.intIdCalificadora
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_Calificadoras_Actualizar(
            p1,
            CType(obj.intCodCalificadora, System.Nullable(Of Integer)),
            obj.strNomCalificadora,
            obj.strUsuario,
            obj.InfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIdCalificadora = p1
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub UpdateCalificadoras(ByVal obj As Calificadoras)
        Dim p1 As System.Nullable(Of Integer) = obj.intIdCalificadora
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_Calificadoras_Actualizar(
            p1,
            CType(obj.intCodCalificadora, System.Nullable(Of Integer)),
            obj.strNomCalificadora,
            obj.strUsuario,
            obj.InfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIdCalificadora = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub DeleteCalificadoras(ByVal obj As Calificadoras)
        Dim p1 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_Calificadoras_Eliminar(
            CType(obj.intIdCalificadora, System.Nullable(Of Integer)),
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

#Region "CalificacionesInversiones"
    Private Sub InsertCalificacionesInversiones(ByVal obj As CalificacionesInversiones)
        Dim p1 As System.Nullable(Of Integer) = obj.intIdCalificacionInversion
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_CalificacionesInversiones_Actualizar(
            p1,
            obj.strCalificacion,
            obj.strTipoCalificacion,
            obj.strCalificacionInversion,
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIdCalificacionInversion = p1
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub UpdateCalificacionesInversiones(ByVal obj As CalificacionesInversiones)
        Dim p1 As System.Nullable(Of Integer) = obj.intIdCalificacionInversion
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_CalificacionesInversiones_Actualizar(
            p1,
            obj.strCalificacion,
            obj.strTipoCalificacion,
            obj.strCalificacionInversion,
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIdCalificacionInversion = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub DeleteCalificacionesInversiones(ByVal obj As CalificacionesInversiones)
        Dim p1 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_CalificacionesInversiones_Eliminar(
            CType(obj.intIdCalificacionInversion, System.Nullable(Of Integer)),
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

#Region "CalificacionesCalificadora"
    Private Sub InsertCalificacionesCalificadora(ByVal obj As CalificacionesCalificadora)
        Dim p1 As Integer = obj.intIdCalificaCalificadora
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_CalificacionesCalificadora_Actualizar(
            p1,
            CType(obj.intCodCalificadora, System.Nullable(Of Integer)),
            CType(obj.intIDCalificacion, System.Nullable(Of Integer)),
            CType(obj.intCodigoSuper, System.Nullable(Of Integer)),
            obj.strUsuario,
            obj.InfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIdCalificaCalificadora = p1
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub UpdateCalificacionesCalificadora(ByVal obj As CalificacionesCalificadora)
        Dim p1 As System.Nullable(Of Integer) = obj.intIdCalificaCalificadora
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_CalificacionesCalificadora_Actualizar(
            p1,
            CType(obj.intCodCalificadora, System.Nullable(Of Integer)),
            CType(obj.intIDCalificacion, System.Nullable(Of Integer)),
            CType(obj.intCodigoSuper, System.Nullable(Of Integer)),
            obj.strUsuario,
            obj.InfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIdCalificaCalificadora = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub DeleteCalificacionesCalificadora(ByVal obj As CalificacionesCalificadora)
        Dim p1 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_CalificacionesCalificadora_Eliminar(
            CType(obj.intIdCalificaCalificadora, System.Nullable(Of Integer)),
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

#Region "ChoquesTasasInteres"
    Private Sub InsertChoquesTasasInteres(ByVal obj As ChoquesTasasInteres)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDChoquesTasasInteres
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_ChoquesTasasInteres_Actualizar(
            p1,
            CType(obj.intZona, System.Nullable(Of Integer)),
            CType(obj.intBanda, System.Nullable(Of Integer)),
            CType(obj.dblLimiteInferior, System.Nullable(Of Double)),
            obj.dblLimiteSuperior,
            obj.strTipoMoneda,
            CType(obj.intPBS, System.Nullable(Of Integer)),
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIDChoquesTasasInteres = p1
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub UpdateChoquesTasasInteres(ByVal obj As ChoquesTasasInteres)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDChoquesTasasInteres
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_ChoquesTasasInteres_Actualizar(
            p1,
            CType(obj.intZona, System.Nullable(Of Integer)),
            CType(obj.intBanda, System.Nullable(Of Integer)),
            CType(obj.dblLimiteInferior, System.Nullable(Of Double)),
            obj.dblLimiteSuperior, obj.strTipoMoneda,
            CType(obj.intPBS, System.Nullable(Of Integer)),
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIDChoquesTasasInteres = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub DeleteChoquesTasasInteres(ByVal obj As ChoquesTasasInteres)
        Dim p1 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_ChoquesTasasInteres_Eliminar(
            CType(obj.intIDChoquesTasasInteres, System.Nullable(Of Integer)),
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

#Region "Entidades"
    Private Sub InsertEntidades(ByVal obj As Entidades)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDEntidad
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_Entidades_Actualizar(
            p1,
            obj.strTipoIdentificacion,
            obj.strNroDocumento,
            obj.strNombre,
            CType(obj.intIDTipoEntidad, System.Nullable(Of Integer)),
            CType(obj.intIDCodigoCIIU, System.Nullable(Of Integer)),
            obj.strTelefono,
            obj.strDireccion,
            CType(obj.intIDCalificacionInversionLarga, System.Nullable(Of Integer)),
            CType(obj.intIDCalificacionInversionCorta, System.Nullable(Of Integer)),
            CType(obj.logActivo, System.Nullable(Of Boolean)),
            CType(obj.intIDCalificadora, System.Nullable(Of Integer)),
            CType(obj.dblValorCupo, System.Nullable(Of Double)),
            CType(obj.dblValorCupoConsumido, System.Nullable(Of Double)),
            CType(obj.lngIDPoblacion, System.Nullable(Of Integer)),
            CType(obj.lngIDDepartamento, System.Nullable(Of Integer)),
            CType(obj.lngIDPais, System.Nullable(Of Integer)),
            obj.logReplicarAEmisores,
            obj.logEsComisionista,
            obj.strEMail,
            obj.logAplicaEnrutamiento,
            obj.strCodigoEmisor,
            obj.logVigiladoSuper,
            obj.lngIDGrupo,
            obj.lngIDSubGrupo,
            obj.logCarteraColectiva,
            obj.intCodigoEntidadAdmin,
            obj.strClaseInversion,
            CType(obj.intDigitoVerificacion, System.Nullable(Of Integer)),
            CType(obj.intIDComisionista, System.Nullable(Of Integer)),
            CType(obj.intCodigoNaturaleza, System.Nullable(Of Integer)),
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2,
            obj.logAdecuadoGobierno,
            obj.strTipoFondo,
            obj.strCalidadTributaria,
            obj.strClasificacionAfiliado)
        obj.intIDEntidad = p1
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub UpdateEntidades(ByVal obj As Entidades)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDEntidad
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_Entidades_Actualizar(
            p1,
            obj.strTipoIdentificacion,
            obj.strNroDocumento,
            obj.strNombre,
            CType(obj.intIDTipoEntidad, System.Nullable(Of Integer)),
            CType(obj.intIDCodigoCIIU, System.Nullable(Of Integer)),
            obj.strTelefono,
            obj.strDireccion,
            CType(obj.intIDCalificacionInversionLarga, System.Nullable(Of Integer)),
            CType(obj.intIDCalificacionInversionCorta, System.Nullable(Of Integer)),
            CType(obj.logActivo, System.Nullable(Of Boolean)),
            CType(obj.intIDCalificadora, System.Nullable(Of Integer)),
            CType(obj.dblValorCupo, System.Nullable(Of Double)),
            CType(obj.dblValorCupoConsumido, System.Nullable(Of Double)),
            CType(obj.lngIDPoblacion, System.Nullable(Of Integer)),
            CType(obj.lngIDDepartamento, System.Nullable(Of Integer)),
            CType(obj.lngIDPais, System.Nullable(Of Integer)),
            obj.logReplicarAEmisores,
            obj.logEsComisionista,
            obj.strEMail,
            obj.logAplicaEnrutamiento,
            obj.strCodigoEmisor,
            obj.logVigiladoSuper,
            obj.lngIDGrupo,
            obj.lngIDSubGrupo,
            obj.logCarteraColectiva,
            obj.intCodigoEntidadAdmin,
            obj.strClaseInversion,
            CType(obj.intDigitoVerificacion, System.Nullable(Of Integer)),
            CType(obj.intIDComisionista, System.Nullable(Of Integer)),
            CType(obj.intCodigoNaturaleza, System.Nullable(Of Integer)),
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2,
            obj.logAdecuadoGobierno,
            obj.strTipoFondo, obj.strCalidadTributaria, obj.strClasificacionAfiliado)
        obj.intIDEntidad = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub DeleteEntidades(ByVal obj As Entidades)
        Dim p1 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_Entidades_Eliminar(
            CType(obj.intIDEntidad, System.Nullable(Of Integer)),
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

#Region "EstadosConceptoTitulos"

    Private Sub InsertEstadosConceptoTitulos(ByVal obj As CFMaestros.EstadosConceptoTitulos)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDEstadosConceptoTitulos
        Dim p2 As String = obj.strMsgValidacion
        Me.uspOyDNet_EstadosConceptoTitulos_Actualizar(
            p1,
            obj.strDescripcionConceptoTitulo,
            obj.strEstadoEntrada,
            obj.strEstadoSalida,
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIDEstadosConceptoTitulos = p1
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub UpdateEstadosConceptoTitulos(ByVal obj As CFMaestros.EstadosConceptoTitulos)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDEstadosConceptoTitulos
        Dim p2 As String = obj.strMsgValidacion
        Me.uspOyDNet_EstadosConceptoTitulos_Actualizar(
            p1,
            obj.strDescripcionConceptoTitulo,
            obj.strEstadoEntrada,
            obj.strEstadoSalida,
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIDEstadosConceptoTitulos = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub DeleteEstadosConceptoTitulos(ByVal obj As CFMaestros.EstadosConceptoTitulos)
        Dim p1 As String = obj.strMsgValidacion
        Me.uspOyDNet_EstadosConceptoTitulos_Eliminar(
            CType(obj.intIDEstadosConceptoTitulos, System.Nullable(Of Integer)),
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

#Region "EntidadesCuentasDeposito"

    Private Sub InsertEntidadesCuentasDeposito(ByVal obj As EntidadesCuentasDeposito)
        Dim p1 As System.Nullable(Of Integer) = obj.intID
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_Entidades_CuentasDeposito_Actualizar(p1,
                                                                       CType(obj.intIDEntidad, System.Nullable(Of Integer)),
                                                                       obj.strDeposito,
                                                                       obj.strCuentaDeposito,
                                                                       obj.strUsuario,
                                                                       obj.strUsuarioWindows,
                                                                       obj.strMaquina,
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

    Private Sub UpdateEntidadesCuentasDeposito(ByVal obj As EntidadesCuentasDeposito)
        Dim p1 As System.Nullable(Of Integer) = obj.intID
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_Entidades_CuentasDeposito_Actualizar(p1,
                                                                       CType(obj.intIDEntidad, System.Nullable(Of Integer)),
                                                                       obj.strDeposito,
                                                                       obj.strCuentaDeposito,
                                                                       obj.strUsuario,
                                                                       obj.strUsuarioWindows,
                                                                       obj.strMaquina,
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

    Private Sub DeleteEntidadesCuentasDeposito(ByVal obj As CFMaestros.EntidadesCuentasDeposito)
        Dim p1 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_Entidades_CuentasDeposito_Eliminar(
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

#Region "EstadosBloqueoTitulos"
    Private Sub InsertEstadosBloqueoTitulos(ByVal obj As EstadosBloqueoTitulos)
        Dim p1 As System.Nullable(Of Integer) = obj.intID
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_EstadosBloqueoTitulos_Actualizar(
            p1,
            obj.strEstadoBloqueo,
            obj.strEstadoDesBloqueo,
            obj.strMecanismo,
            obj.strMotivoBloqueo,
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2,
            obj.logEstado)
        obj.intID = p1
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub UpdateEstadosBloqueoTitulos(ByVal obj As EstadosBloqueoTitulos)
        Dim p1 As System.Nullable(Of Integer) = obj.intID
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_EstadosBloqueoTitulos_Actualizar(
            p1,
            obj.strEstadoBloqueo,
            obj.strEstadoDesBloqueo,
            obj.strMecanismo,
            obj.strMotivoBloqueo,
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2,
            obj.logEstado)
        obj.intID = p1
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub DeleteEstadosBloqueoTitulos(ByVal obj As EstadosBloqueoTitulos)
        Dim p1 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_EstadosBloqueoTitulos_Eliminar(
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

#Region "clase contable titulo"
    Private Sub InserttblClaseContableTitulo(ByVal obj As tblClaseContableTitulo)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDClaseContableTitulo
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_ClaseContableTitulo_Actualizar(
            obj.intIDClaseContableTitulo,
            obj.strTipoTitulo,
            obj.strReferencia,
            obj.logGravado,
            obj.logNCRNGO,
            obj.strUsuario,
            obj.logMultimoneda,
            obj.xmlDetalleGrid,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIDClaseContableTitulo = p1
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub
    Private Sub UpdatetblClaseContableTitulo(ByVal obj As tblClaseContableTitulo)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDClaseContableTitulo
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_ClaseContableTitulo_Actualizar(
            obj.intIDClaseContableTitulo,
            obj.strTipoTitulo,
            obj.strReferencia,
            obj.logGravado,
            obj.logNCRNGO,
            obj.strUsuario,
            obj.logMultimoneda,
            obj.xmlDetalleGrid,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIDClaseContableTitulo = p1
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

#End Region

#Region "CuentasFondos"

    Private Sub InsertCuentasFondos(ByVal obj As CuentasFondos)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDCuentasDeceval
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_CuentasFondos_Actualizar(
            p1,
            CType(obj.lngidCuentaDeceval, System.Nullable(Of Integer)),
            obj.strDeposito,
            obj.lngidComitenteLider,
            obj.strTipoIdComitente,
            obj.strNroDocumento,
            obj.strNombre,
            obj.strPrefijo,
            CType(Nothing, String),
            obj.strConector1,
            obj.strTipoIdBenef1,
            obj.lngNroDocBenef1,
            obj.strConector2,
            obj.strTipoIdBenef2,
            obj.lngNroDocBenef2,
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2,
            obj.strCuentaPrincipalDCV)
        obj.intIDCuentasDeceval = p1
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub UpdateCuentasFondos(ByVal obj As CuentasFondos)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDCuentasDeceval
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_CuentasFondos_Actualizar(
            p1,
            CType(obj.lngidCuentaDeceval, System.Nullable(Of Integer)),
            obj.strDeposito,
            obj.lngidComitenteLider,
            obj.strTipoIdComitente,
            obj.strNroDocumento,
            obj.strNombre,
            obj.strPrefijo,
            CType(Nothing, String),
            obj.strConector1,
            obj.strTipoIdBenef1,
            obj.lngNroDocBenef1,
            obj.strConector2,
            obj.strTipoIdBenef2,
            obj.lngNroDocBenef2,
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2,
            obj.strCuentaPrincipalDCV)
        obj.intIDCuentasDeceval = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub DeleteCuentasFondos(ByVal obj As CuentasFondos)
        Dim p1 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_CuentasFondos_Eliminar(
            CType(obj.intIDCuentasDeceval, System.Nullable(Of Integer)),
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

#Region "EstadosEntradaSalidaTitulos"

    Private Sub InsertEstadosEntradaSalidaTitulos(ByVal obj As EstadosEntradaSalidaTitulos)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDEstadosEntradaSalidaTitulos
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_EstadosEntradaSalidaTitulos_Actualizar(
            p1,
            obj.strRetorno,
            obj.strDescripcion,
            obj.strTopico,
            obj.strMecanismo,
            obj.strMotivoBloqueo,
            obj.strEstadoActual,
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIDEstadosEntradaSalidaTitulos = p1
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub UpdateEstadosEntradaSalidaTitulos(ByVal obj As EstadosEntradaSalidaTitulos)
        Dim p1 As System.Nullable(Of Integer) = obj.intIDEstadosEntradaSalidaTitulos
        Dim p2 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_EstadosEntradaSalidaTitulos_Actualizar(
            p1,
            obj.strRetorno,
            obj.strDescripcion,
            obj.strTopico,
            obj.strMecanismo,
            obj.strMotivoBloqueo,
            obj.strEstadoActual,
            obj.strUsuario,
            obj.strInfoSesion,
            Constantes.ERROR_PERSONALIZADO_SQLSERVER,
            p2)
        obj.intIDEstadosEntradaSalidaTitulos = p1.GetValueOrDefault
        obj.strMsgValidacion = p2
        If Not String.IsNullOrEmpty(p2) Then
            If Not p2.Contains(Constantes.IDENTIFICADOR_EXITOSO_MENSAJE) Then
                Throw New Exception(p2, New Exception(Constantes.IDENTIFICADOR_VALIDACION_A2))
            End If
        End If
    End Sub

    Private Sub DeleteEstadosEntradaSalidaTitulos(ByVal obj As EstadosEntradaSalidaTitulos)
        Dim p1 As String = obj.strMsgValidacion
        Me.uspCalculosFinancieros_EstadosEntradaSalidaTitulos_Eliminar(
            CType(obj.intIDEstadosEntradaSalidaTitulos, System.Nullable(Of Integer)),
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
