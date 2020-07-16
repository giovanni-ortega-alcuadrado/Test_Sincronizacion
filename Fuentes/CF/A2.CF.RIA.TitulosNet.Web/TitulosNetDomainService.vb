Imports System.Threading.Tasks
Imports A2.OyD.OYDServer.RIA.Web.CFTitulosNet
Imports System.Text
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

<EnableClientAccess()>
Partial Public Class TitulosNetDomainService
    Inherits LinqToSqlDomainService(Of CF_TitulosNetDataContext)

    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

#Region "Titulos"

#Region "Métodos asincrónicos"
    Public Function ConsultarConsecutivo(pstrNombreConsecutivo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pintConsecutivo As System.Nullable(Of Integer)
            Me.DataContext.uspOyDNet_Titulos_Consecutivo(pstrNombreConsecutivo, pintConsecutivo, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConsecutivo"), 0)
            Return pintConsecutivo
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConsecutivo")
            Return Nothing
        End Try
    End Function

    Public Function GuardarUsuarioProcesoTitulos(ByVal pstrTipoProceso As String, ByVal pintConsecutivo As Integer, ByVal pstrMsgValidacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.uspOyDNet_Titulos_ControlUsuarios(pstrTipoProceso, pintConsecutivo, pstrMsgValidacion, pstrUsuario, DemeInfoSesion(pstrUsuario, "GuardarUsuarioProcesoTitulos"), 0)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GuardarUsuarioProcesoTitulos")
            Return Nothing
        End Try
    End Function

    Public Function Titulos_CompradosSinCargar_Consultar(pstrFiltro As String, plogFiltrarTodo As System.Nullable(Of System.Boolean), pdtmFechaLimite As System.Nullable(Of System.DateTime), plngComitente As String, pstrIDEspecie As String, pdtmFechaCorte As System.Nullable(Of System.DateTime), pstrClase As String, pintConsecutivo As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TitulosCompras)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Titulos_CompradosSinCargar_Consultar(pstrFiltro, plogFiltrarTodo, pdtmFechaLimite, plngComitente, pstrIDEspecie, pdtmFechaCorte, pstrClase, pintConsecutivo, pstrUsuario, DemeInfoSesion(pstrUsuario, "Titulos_CompradosSinCargar_Consultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Titulos_CompradosSinCargar_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function Titulos_VendidosSinDescargar_Consultar(pstrFiltro As String, plogFiltrarTodo As System.Nullable(Of System.Boolean), pdtmFechaLimite As System.Nullable(Of System.DateTime), plngComitente As String, pstrIDEspecie As String, pdtmFechaCorte As System.Nullable(Of System.DateTime), pstrClase As String, pintConsecutivo As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TitulosVentas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Titulos_VendidosSinDescargar_Consultar(pstrFiltro, plogFiltrarTodo, pdtmFechaLimite, plngComitente, pstrIDEspecie, pdtmFechaCorte, pstrClase, pintConsecutivo, pstrUsuario, DemeInfoSesion(pstrUsuario, "Titulos_VendidosSinDescargar_Consultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Titulos_VendidosSinDescargar_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function Titulos_ComprasXCargar_Consultar(plngComitente As String, pstrIDEspecie As String, pstrClase As String, pintConsecutivo As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal plogFiltradoDesdePantalla As System.Nullable(Of System.Boolean), ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Titulos_ComprasXCargar_Consultar(plngComitente, pstrIDEspecie, pstrClase, pintConsecutivo, pstrUsuario, DemeInfoSesion(pstrUsuario, "Titulos_ComprasXCargar_Consultar"), 0, plogFiltradoDesdePantalla)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Titulos_ComprasXCargar_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function Titulos_CruceTitulosPortafolio_Consultar(pstrIDEspecie As String, plngComitente As String, pdtmFechaEmision As System.Nullable(Of System.DateTime), pdtmFechaLimite As System.Nullable(Of System.DateTime), pdtmFechaVencimiento As System.Nullable(Of System.DateTime), pdblTasa As System.Nullable(Of Double), pstrIndicador As String, pdblPuntos As System.Nullable(Of Double), pstrModalidad As String, pdblCantidad As System.Nullable(Of Double), pstrClase As String, plngCustodia As System.Nullable(Of Integer), plngSecuencia As System.Nullable(Of Integer), pstrTipoDeOferta As String, plngIdLiqVenta As System.Nullable(Of Double), plngIdParcialVenta As System.Nullable(Of Double), pintConsecutivo As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal plogFiltradoDesdePantalla As System.Nullable(Of System.Boolean), ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Titulos_CruceTitulosPortafolio_Consultar(pstrIDEspecie, plngComitente, pdtmFechaEmision, pdtmFechaLimite, pdtmFechaVencimiento, pdblTasa, pstrIndicador, pdblPuntos, pstrModalidad, pdblCantidad, pstrClase, plngCustodia, plngSecuencia, pstrTipoDeOferta, plngIdLiqVenta, plngIdParcialVenta, pintConsecutivo, pstrUsuario, DemeInfoSesion(pstrUsuario, "Titulos_CruceTitulosPortafolio_Consultar"), 0, plogFiltradoDesdePantalla)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Titulos_CruceTitulosPortafolio_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarCruce(pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TitulosCruce)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Titulos_ConsultarCruce(pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarCruce"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCruce")
            Return Nothing
        End Try
    End Function

    Public Function PortafolioCliente_Consultar(pdtmFechaLimite As System.Nullable(Of System.DateTime), plngComitente As String, pstrIDEspecie As String, pstrClase As String, pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TitulosPortafolio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Titulos_PortafolioCliente_Consultar(pdtmFechaLimite, plngComitente, pstrIDEspecie, pstrClase, pstrUsuario, DemeInfoSesion(pstrUsuario, "PortafolioCliente_Consultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PortafolioCliente_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function Titulos_MovimientosDeceval_Importar(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrModulo As String, ByVal pstrNombreCompletoArchivo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objDatosRutas = clsClaseUploads.FnRutasImportaciones(pstrModulo, pstrUsuario)
            Dim directorioUsuario = objDatosRutas.RutaArchivosLocal
            Dim objStringordenes As StringBuilder = Nothing

            Dim objImportar As New clsProcesosArchivo With {.gstrUser = My.User.Name}

            Dim ret = Me.DataContext.uspOyDNet_Titulos_MovimientosDeceval_Importar(pdtmFechaProceso, pstrUsuario.Replace(".", "_") & objDatosRutas.NombreProceso & "\" & pstrNombreCompletoArchivo, pstrUsuario).ToList

            Return ret

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DerechosPatrimoniales_Deceval_Importar")
            Return Nothing
        End Try
    End Function

    Public Function Titulos_MovimientosDeceval_ImportarSeleccionArchivo(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrRutaCompletaArchivo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Titulos_MovimientosDeceval_Importar(pdtmFechaProceso, pstrRutaCompletaArchivo, pstrUsuario).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Titulos_MovimientosDeceval_ImportarSeleccionArchivo")
            Return Nothing
        End Try
    End Function

    Public Function SolicitarArchivoMovimientosDeceval(ByVal pstrArchivo As String, ByVal pintTipoArchivo As Nullable(Of Integer), ByVal pstrCodigoISIN As String, ByVal pintCuentaInversionista As Nullable(Of Integer), ByVal pstrCodigoDeposito As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblResultadoEnvioArchivo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_DecevalArchivos_Solicitar(pstrArchivo, pintTipoArchivo, pstrUsuario, DemeInfoSesion(pstrUsuario, "SolicitarArchivoMovimientosDeceval"), 0, pstrCodigoISIN, pintCuentaInversionista, pstrCodigoDeposito)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "SolicitarArchivoMovimientosDeceval")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function ConsultarConsecutivoSync(ByVal pstrNombreConsecutivo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Dim objTask As Task(Of Integer) = Me.ConsultarConsecutivoAsync(pstrNombreConsecutivo, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarConsecutivoAsync(ByVal pstrNombreConsecutivo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Integer)
        Dim objTaskComplete As TaskCompletionSource(Of Integer) = New TaskCompletionSource(Of Integer)()
        objTaskComplete.TrySetResult(ConsultarConsecutivo(pstrNombreConsecutivo, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function GuardarUsuarioProcesoTitulosSync(ByVal pstrTipoProceso As String, ByVal pintConsecutivo As Integer, ByVal pstrMsgValidacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.GuardarUsuarioProcesoTitulosAsync(pstrTipoProceso, pintConsecutivo, pstrMsgValidacion, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function GuardarUsuarioProcesoTitulosAsync(ByVal pstrTipoProceso As String, ByVal pintConsecutivo As Integer, ByVal pstrMsgValidacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(GuardarUsuarioProcesoTitulos(pstrTipoProceso, pintConsecutivo, pstrMsgValidacion, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Titulos_CompradosSinCargar_ConsultarSync(ByVal pstrFiltro As String, ByVal plogFiltrarTodo As System.Nullable(Of System.Boolean), ByVal pdtmFechaLimite As System.Nullable(Of System.DateTime), ByVal plngComitente As String, ByVal pstrIDEspecie As String, ByVal pdtmFechaCorte As System.Nullable(Of System.DateTime), ByVal pstrClase As String, ByVal pintConsecutivo As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TitulosCompras)
        Dim objTask As Task(Of List(Of TitulosCompras)) = Me.Titulos_CompradosSinCargar_ConsultarAsync(pstrFiltro, plogFiltrarTodo, pdtmFechaLimite, plngComitente, pstrIDEspecie, pdtmFechaCorte, pstrClase, pintConsecutivo, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Titulos_CompradosSinCargar_ConsultarAsync(ByVal pstrFiltro As String, ByVal plogFiltrarTodo As System.Nullable(Of System.Boolean), ByVal pdtmFechaLimite As System.Nullable(Of System.DateTime), ByVal plngComitente As String, ByVal pstrIDEspecie As String, ByVal pdtmFechaCorte As System.Nullable(Of System.DateTime), ByVal pstrClase As String, ByVal pintConsecutivo As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of TitulosCompras))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of TitulosCompras)) = New TaskCompletionSource(Of List(Of TitulosCompras))()
        objTaskComplete.TrySetResult(Titulos_CompradosSinCargar_Consultar(pstrFiltro, plogFiltrarTodo, pdtmFechaLimite, plngComitente, pstrIDEspecie, pdtmFechaCorte, pstrClase, pintConsecutivo, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Titulos_VendidosSinDescargar_ConsultarSync(ByVal pstrFiltro As String, ByVal plogFiltrarTodo As System.Nullable(Of System.Boolean), ByVal pdtmFechaLimite As System.Nullable(Of System.DateTime), ByVal plngComitente As String, ByVal pstrIDEspecie As String, ByVal pdtmFechaCorte As System.Nullable(Of System.DateTime), ByVal pstrClase As String, ByVal pintConsecutivo As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TitulosVentas)
        Dim objTask As Task(Of List(Of TitulosVentas)) = Me.Titulos_VendidosSinDescargar_ConsultarAsync(pstrFiltro, plogFiltrarTodo, pdtmFechaLimite, plngComitente, pstrIDEspecie, pdtmFechaCorte, pstrClase, pintConsecutivo, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Titulos_VendidosSinDescargar_ConsultarAsync(ByVal pstrFiltro As String, ByVal plogFiltrarTodo As System.Nullable(Of System.Boolean), ByVal pdtmFechaLimite As System.Nullable(Of System.DateTime), ByVal plngComitente As String, ByVal pstrIDEspecie As String, ByVal pdtmFechaCorte As System.Nullable(Of System.DateTime), ByVal pstrClase As String, ByVal pintConsecutivo As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of TitulosVentas))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of TitulosVentas)) = New TaskCompletionSource(Of List(Of TitulosVentas))()
        objTaskComplete.TrySetResult(Titulos_VendidosSinDescargar_Consultar(pstrFiltro, plogFiltrarTodo, pdtmFechaLimite, plngComitente, pstrIDEspecie, pdtmFechaCorte, pstrClase, pintConsecutivo, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Titulos_ComprasXCargar_ConsultarSync(ByVal plngComitente As String, ByVal pstrIDEspecie As String, ByVal pstrClase As String, ByVal pintConsecutivo As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal plogFiltradoDesdePantalla As System.Nullable(Of System.Boolean), ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.Titulos_ComprasXCargar_ConsultarAsync(plngComitente, pstrIDEspecie, pstrClase, pintConsecutivo, pstrUsuario, plogFiltradoDesdePantalla, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Titulos_ComprasXCargar_ConsultarAsync(ByVal plngComitente As String, ByVal pstrIDEspecie As String, ByVal pstrClase As String, ByVal pintConsecutivo As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal plogFiltradoDesdePantalla As System.Nullable(Of System.Boolean), ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(Titulos_ComprasXCargar_Consultar(plngComitente, pstrIDEspecie, pstrClase, pintConsecutivo, pstrUsuario, plogFiltradoDesdePantalla, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Titulos_CruceTitulosPortafolio_ConsultarSync(ByVal pstrIDEspecie As String, ByVal plngComitente As String, ByVal pdtmFechaEmision As System.Nullable(Of System.DateTime), ByVal pdtmFechaLimite As System.Nullable(Of System.DateTime), ByVal pdtmFechaVencimiento As System.Nullable(Of System.DateTime), ByVal pdblTasa As System.Nullable(Of Double), ByVal pstrIndicador As String, ByVal pdblPuntos As System.Nullable(Of Double), ByVal pstrModalidad As String, ByVal pdblCantidad As System.Nullable(Of Double), ByVal pstrClase As String, ByVal plngCustodia As System.Nullable(Of Integer), ByVal plngSecuencia As System.Nullable(Of Integer), ByVal pstrTipoDeOferta As String, ByVal plngIdLiqVenta As System.Nullable(Of Double), ByVal plngIdParcialVenta As System.Nullable(Of Double), ByVal pintConsecutivo As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal plogFiltradoDesdePantalla As System.Nullable(Of System.Boolean), ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.Titulos_CruceTitulosPortafolio_ConsultarAsync(pstrIDEspecie, plngComitente, pdtmFechaEmision, pdtmFechaLimite, pdtmFechaVencimiento, pdblTasa, pstrIndicador, pdblPuntos, pstrModalidad, pdblCantidad, pstrClase, plngCustodia, plngSecuencia, pstrTipoDeOferta, plngIdLiqVenta, plngIdParcialVenta, pintConsecutivo, pstrUsuario, plogFiltradoDesdePantalla, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Titulos_CruceTitulosPortafolio_ConsultarAsync(ByVal pstrIDEspecie As String, ByVal plngComitente As String, ByVal pdtmFechaEmision As System.Nullable(Of System.DateTime), ByVal pdtmFechaLimite As System.Nullable(Of System.DateTime), ByVal pdtmFechaVencimiento As System.Nullable(Of System.DateTime), ByVal pdblTasa As System.Nullable(Of Double), ByVal pstrIndicador As String, ByVal pdblPuntos As System.Nullable(Of Double), ByVal pstrModalidad As String, ByVal pdblCantidad As System.Nullable(Of Double), ByVal pstrClase As String, ByVal plngCustodia As System.Nullable(Of Integer), ByVal plngSecuencia As System.Nullable(Of Integer), ByVal pstrTipoDeOferta As String, ByVal plngIdLiqVenta As System.Nullable(Of Double), ByVal plngIdParcialVenta As System.Nullable(Of Double), ByVal pintConsecutivo As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal plogFiltradoDesdePantalla As System.Nullable(Of System.Boolean), ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(Titulos_CruceTitulosPortafolio_Consultar(pstrIDEspecie, plngComitente, pdtmFechaEmision, pdtmFechaLimite, pdtmFechaVencimiento, pdblTasa, pstrIndicador, pdblPuntos, pstrModalidad, pdblCantidad, pstrClase, plngCustodia, plngSecuencia, pstrTipoDeOferta, plngIdLiqVenta, plngIdParcialVenta, pintConsecutivo, pstrUsuario, plogFiltradoDesdePantalla, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarCruceSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TitulosCruce)
        Dim objTask As Task(Of List(Of TitulosCruce)) = Me.ConsultarCruceAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarCruceAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of TitulosCruce))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of TitulosCruce)) = New TaskCompletionSource(Of List(Of TitulosCruce))()
        objTaskComplete.TrySetResult(ConsultarCruce(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function PortafolioCliente_ConsultarSync(ByVal pdtmFechaLimite As System.Nullable(Of System.DateTime), ByVal plngComitente As String, ByVal pstrIDEspecie As String, ByVal pstrClase As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TitulosPortafolio)
        Dim objTask As Task(Of List(Of TitulosPortafolio)) = Me.PortafolioCliente_ConsultarAsync(pdtmFechaLimite, plngComitente, pstrIDEspecie, pstrClase, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function PortafolioCliente_ConsultarAsync(ByVal pdtmFechaLimite As System.Nullable(Of System.DateTime), ByVal plngComitente As String, ByVal pstrIDEspecie As String, ByVal pstrClase As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of TitulosPortafolio))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of TitulosPortafolio)) = New TaskCompletionSource(Of List(Of TitulosPortafolio))()
        objTaskComplete.TrySetResult(PortafolioCliente_Consultar(pdtmFechaLimite, plngComitente, pstrIDEspecie, pstrClase, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Titulos_MovimientosDeceval_ImportarSync(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrModulo As String, ByVal pstrNombreCompletoArchivo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Dim objTask As Task(Of List(Of RespuestaArchivoImportacion)) = Me.Titulos_MovimientosDeceval_ImportarAsync(pdtmFechaProceso, pstrModulo, pstrNombreCompletoArchivo, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Titulos_MovimientosDeceval_ImportarAsync(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrModulo As String, ByVal pstrNombreCompletoArchivo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of RespuestaArchivoImportacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of RespuestaArchivoImportacion)) = New TaskCompletionSource(Of List(Of RespuestaArchivoImportacion))()
        objTaskComplete.TrySetResult(Titulos_MovimientosDeceval_Importar(pdtmFechaProceso, pstrModulo, pstrNombreCompletoArchivo, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Titulos_MovimientosDeceval_ImportarSeleccionArchivoSync(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrRutaCompletaArchivo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RespuestaArchivoImportacion)
        Dim objTask As Task(Of List(Of RespuestaArchivoImportacion)) = Me.Titulos_MovimientosDeceval_ImportarSeleccionArchivoAsync(pdtmFechaProceso, pstrRutaCompletaArchivo, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Titulos_MovimientosDeceval_ImportarSeleccionArchivoAsync(ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrRutaCompletaArchivo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of RespuestaArchivoImportacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of RespuestaArchivoImportacion)) = New TaskCompletionSource(Of List(Of RespuestaArchivoImportacion))()
        objTaskComplete.TrySetResult(Titulos_MovimientosDeceval_ImportarSeleccionArchivo(pdtmFechaProceso, pstrRutaCompletaArchivo, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "CruceOperacionesManual"

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertTitulosVentas(ByVal obj As CFTitulosNet.TitulosVentas)

    End Sub

    Public Sub UpdateTitulosVentas(ByVal obj As CFTitulosNet.TitulosVentas)

    End Sub

    Public Sub UpdateTitulosPortafolio(ByVal obj As CFTitulosNet.TitulosPortafolio)

    End Sub

#End Region

#Region "Métodos asincrónicos"

    Public Function CruceOperacionesManual_VendidosSinDescargar_Consultar(pstrFiltro As String, plogFiltrarTodo As System.Nullable(Of System.Boolean), pdtmFechaLimite As System.Nullable(Of System.DateTime), plngComitente As String, pstrIDEspecie As String, pdtmFechaCorte As System.Nullable(Of System.DateTime), pstrClase As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TitulosVentas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CruceOperacionesManual_VendidosSinDescargar_Consultar(pstrFiltro, plogFiltrarTodo, pdtmFechaLimite, plngComitente, pstrIDEspecie, pdtmFechaCorte, pstrClase, pstrUsuario, DemeInfoSesion(pstrUsuario, "CruceOperacionesManual_VendidosSinDescargar_Consultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CruceOperacionesManual_VendidosSinDescargar_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function CruceOperacionesManual_PortafolioCliente_Consultar(ByVal pstrFiltro As String, ByVal plogFiltrarTodo As System.Nullable(Of System.Boolean), pdtmFechaLimite As System.Nullable(Of System.DateTime), plngComitente As String, pstrIDEspecie As String, pstrClase As String, pintIDLiquidaciones As System.Nullable(Of Integer), pstrOrigen As String, pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TitulosPortafolio)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CruceOperacionesManual_PortafolioCliente_Consultar(pstrFiltro, plogFiltrarTodo, pdtmFechaLimite, plngComitente, pstrIDEspecie, pstrClase, pintIDLiquidaciones, pstrOrigen, pstrUsuario, DemeInfoSesion(pstrUsuario, "CruceOperacionesManual_PortafolioCliente_Consultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CruceOperacionesManual_PortafolioCliente_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function CruceOperacionesManual_Actualizar(ByVal pintIDLiquidaciones As System.Nullable(Of Integer), ByVal pxmlDetalleGrid As String, ByVal pstrOrigen As String, ByVal pstrMsgValidacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.uspOyDNet_CruceOperacionesManual_Actualizar(pintIDLiquidaciones, pxmlDetalleGrid, pstrOrigen, pstrMsgValidacion, pstrUsuario, DemeInfoSesion(pstrUsuario, "CruceTitulosDescargadosManualmente_Actualizar"), 0)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CruceTitulosDescargadosManualmente_Actualizar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function CruceOperacionesManual_VendidosSinDescargar_ConsultarSync(ByVal pstrFiltro As String, ByVal plogFiltrarTodo As System.Nullable(Of System.Boolean), ByVal pdtmFechaLimite As System.Nullable(Of System.DateTime), ByVal plngComitente As String, ByVal pstrIDEspecie As String, ByVal pdtmFechaCorte As System.Nullable(Of System.DateTime), ByVal pstrClase As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TitulosVentas)
        Dim objTask As Task(Of List(Of TitulosVentas)) = Me.CruceOperacionesManual_VendidosSinDescargar_ConsultarAsync(pstrFiltro, plogFiltrarTodo, pdtmFechaLimite, plngComitente, pstrIDEspecie, pdtmFechaCorte, pstrClase, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function CruceOperacionesManual_VendidosSinDescargar_ConsultarAsync(ByVal pstrFiltro As String, ByVal plogFiltrarTodo As System.Nullable(Of System.Boolean), ByVal pdtmFechaLimite As System.Nullable(Of System.DateTime), ByVal plngComitente As String, ByVal pstrIDEspecie As String, ByVal pdtmFechaCorte As System.Nullable(Of System.DateTime), ByVal pstrClase As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of TitulosVentas))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of TitulosVentas)) = New TaskCompletionSource(Of List(Of TitulosVentas))()
        objTaskComplete.TrySetResult(CruceOperacionesManual_VendidosSinDescargar_Consultar(pstrFiltro, plogFiltrarTodo, pdtmFechaLimite, plngComitente, pstrIDEspecie, pdtmFechaCorte, pstrClase, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function CruceOperacionesManual_PortafolioCliente_ConsultarSync(ByVal pstrFiltro As String, ByVal plogFiltrarTodo As System.Nullable(Of System.Boolean), ByVal pdtmFechaLimite As System.Nullable(Of System.DateTime), ByVal plngComitente As String, ByVal pstrIDEspecie As String, ByVal pstrClase As String, pintIDLiquidaciones As System.Nullable(Of Integer), pstrOrigen As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TitulosPortafolio)
        Dim objTask As Task(Of List(Of TitulosPortafolio)) = Me.CruceOperacionesManual_PortafolioCliente_ConsultarAsync(pstrFiltro, plogFiltrarTodo, pdtmFechaLimite, plngComitente, pstrIDEspecie, pstrClase, pintIDLiquidaciones, pstrOrigen, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function CruceOperacionesManual_PortafolioCliente_ConsultarAsync(ByVal pstrFiltro As String, ByVal plogFiltrarTodo As System.Nullable(Of System.Boolean), ByVal pdtmFechaLimite As System.Nullable(Of System.DateTime), ByVal plngComitente As String, ByVal pstrIDEspecie As String, ByVal pstrClase As String, pintIDLiquidaciones As System.Nullable(Of Integer), pstrOrigen As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of TitulosPortafolio))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of TitulosPortafolio)) = New TaskCompletionSource(Of List(Of TitulosPortafolio))()
        objTaskComplete.TrySetResult(CruceOperacionesManual_PortafolioCliente_Consultar(pstrFiltro, plogFiltrarTodo, pdtmFechaLimite, plngComitente, pstrIDEspecie, pstrClase, pintIDLiquidaciones, pstrOrigen, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function CruceOperacionesManual_ActualizarSync(ByVal pintIDLiquidaciones As System.Nullable(Of Integer), ByVal pxmlDetalleGrid As String, ByVal pstrOrigen As String, ByVal pstrMsgValidacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.CruceOperacionesManual_ActualizarAsync(pintIDLiquidaciones, pxmlDetalleGrid, pstrOrigen, pstrMsgValidacion, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function CruceOperacionesManual_ActualizarAsync(ByVal pintIDLiquidaciones As System.Nullable(Of Integer), ByVal pxmlDetalleGrid As String, ByVal pstrOrigen As String, ByVal pstrMsgValidacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(CruceOperacionesManual_Actualizar(pintIDLiquidaciones, pxmlDetalleGrid, pstrOrigen, pstrMsgValidacion, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "ConceptoRetencion"

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertConceptoRetencion(ByVal objNewConceptoRetencion As ConceptoRetencion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objNewConceptoRetencion.pstrUsuarioConexion, objNewConceptoRetencion.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objNewConceptoRetencion.strInfoSesion = DemeInfoSesion(objNewConceptoRetencion.pstrUsuarioConexion, "InsertConceptoRetencion")
            Me.DataContext.ConceptoRetencion.InsertOnSubmit(objNewConceptoRetencion)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertConceptoRetencion")
        End Try
    End Sub

    Public Sub UpdateConceptoRetencion(ByVal objCurrentConceptoRetencion As ConceptoRetencion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objCurrentConceptoRetencion.pstrUsuarioConexion, objCurrentConceptoRetencion.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objCurrentConceptoRetencion.strInfoSesion = DemeInfoSesion(objCurrentConceptoRetencion.pstrUsuarioConexion, "UpdateConceptoRetencion")
            Me.DataContext.ConceptoRetencion.Attach(objCurrentConceptoRetencion, Me.ChangeSet.GetOriginal(objCurrentConceptoRetencion))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateConceptoRetencion")
        End Try
    End Sub

    Public Sub DeleteConceptoRetencion(ByVal objDeleteConceptoRetencion As ConceptoRetencion)
        Try
            objDeleteConceptoRetencion.strInfoSesion = DemeInfoSesion(objDeleteConceptoRetencion.pstrUsuarioConexion, "DeleteConceptoRetencion")
            Me.DataContext.ConceptoRetencion.Attach(objDeleteConceptoRetencion)
            Me.DataContext.ConceptoRetencion.DeleteOnSubmit(objDeleteConceptoRetencion)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteConceptoRetencion")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"
    Public Function ConceptosRetencion_Filtrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConceptoRetencion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ConceptosRetencion_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "ConceptosRetencion_Filtrar"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConceptosRetencion_Filtrar")
            Return Nothing
        End Try
    End Function

    Public Function ConceptosRetencion_Consultar(ByVal pintIDConceptoRetencion As System.Nullable(Of Integer), ByVal pstrCodigo As String, ByVal pstrDescripcion As String, ByVal plogGravado As System.Nullable(Of System.Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConceptoRetencion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ConceptosRetencion_Consultar(String.Empty, pintIDConceptoRetencion, pstrCodigo, pstrDescripcion, plogGravado, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConceptosRetencion_Consultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConceptosRetencion_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function ConceptosRetencion_ConsultarPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ConceptoRetencion
        Dim objConceptoRetencion As ConceptoRetencion = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ConceptosRetencion_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, 0, String.Empty, String.Empty, 0, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConceptosRetencion_ConsultarPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objConceptoRetencion = ret.FirstOrDefault
            End If
            Return objConceptoRetencion
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConceptosRetencion_ConsultarPorDefecto")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de insertar o modificar el encabezado
    ''' </summary>
    Public Function ConceptosRetencion_Actualizar(ByVal pintIDConceptoRetencion As System.Nullable(Of Integer), ByVal pstrCodigo As String, ByVal pstrDescripcion As String, ByVal pdblPorcentajeRetencion As System.Nullable(Of Double), ByVal plogGravado As System.Nullable(Of System.Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Dim ret = Me.DataContext.uspCalculosFinancieros_ConceptosRetencion_Actualizar(pintIDConceptoRetencion, pstrCodigo, pstrDescripcion, pdblPorcentajeRetencion, plogGravado, pstrUsuario, DemeInfoSesion(pstrUsuario, "CuentasFondos_Actualizar"), 0, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CuentasFondos_Actualizar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function ConceptosRetencion_FiltrarSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConceptoRetencion)
        Dim objTask As Task(Of List(Of ConceptoRetencion)) = Me.ConceptosRetencion_FiltrarAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConceptosRetencion_FiltrarAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ConceptoRetencion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ConceptoRetencion)) = New TaskCompletionSource(Of List(Of ConceptoRetencion))()
        objTaskComplete.TrySetResult(ConceptosRetencion_Filtrar(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConceptosRetencion_ConsultarSync(ByVal pintIDConceptoRetencion As System.Nullable(Of Integer), ByVal pstrCodigo As String, ByVal pstrDescripcion As String, ByVal plogGravado As System.Nullable(Of System.Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConceptoRetencion)
        Dim objTask As Task(Of List(Of ConceptoRetencion)) = Me.ConceptosRetencion_ConsultarAsync(pintIDConceptoRetencion, pstrCodigo, pstrDescripcion, plogGravado, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConceptosRetencion_ConsultarAsync(ByVal pintIDConceptoRetencion As System.Nullable(Of Integer), ByVal pstrCodigo As String, ByVal pstrDescripcion As String, ByVal plogGravado As System.Nullable(Of System.Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ConceptoRetencion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ConceptoRetencion)) = New TaskCompletionSource(Of List(Of ConceptoRetencion))()
        objTaskComplete.TrySetResult(ConceptosRetencion_Consultar(pintIDConceptoRetencion, pstrCodigo, pstrDescripcion, plogGravado, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConceptosRetencion_ConsultarPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ConceptoRetencion
        Dim objTask As Task(Of ConceptoRetencion) = Me.ConceptosRetencion_ConsultarPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConceptosRetencion_ConsultarPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of ConceptoRetencion)
        Dim objTaskComplete As TaskCompletionSource(Of ConceptoRetencion) = New TaskCompletionSource(Of ConceptoRetencion)()
        objTaskComplete.TrySetResult(ConceptosRetencion_ConsultarPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConceptosRetencion_ActualizarSync(ByVal pintIDConceptoRetencion As System.Nullable(Of Integer), ByVal pstrCodigo As String, ByVal pstrDescripcion As String, ByVal pdblPorcentajeRetencion As System.Nullable(Of Double), ByVal plogGravado As System.Nullable(Of System.Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.ConceptosRetencion_ActualizarAsync(pintIDConceptoRetencion, pstrCodigo, pstrDescripcion, pdblPorcentajeRetencion, plogGravado, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConceptosRetencion_ActualizarAsync(ByVal pintIDConceptoRetencion As System.Nullable(Of Integer), ByVal pstrCodigo As String, ByVal pstrDescripcion As String, ByVal pdblPorcentajeRetencion As System.Nullable(Of Double), ByVal plogGravado As System.Nullable(Of System.Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(ConceptosRetencion_Actualizar(pintIDConceptoRetencion, pstrCodigo, pstrDescripcion, pdblPorcentajeRetencion, plogGravado, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

End Class

Public Class RutasArchivos
    Public Property NombreProceso As String
    Public Property RutaWeb As String
    Public Property RutaArchivosLocal As String
    Public Property MensajeDebbug As String
    Public Property RutaArchivosUpload As String
End Class
