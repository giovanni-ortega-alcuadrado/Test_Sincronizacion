Imports System.Threading.Tasks
Imports A2.OyD.OYDServer.RIA.Web.CFEspecies
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

<EnableClientAccess()>
Partial Public Class EspeciesCFDomainService
    Inherits LinqToSqlDomainService(Of CF_EspeciesDataContext)

    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

#Region "Especies"

    Public Function EspeciesFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Especi)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Especies_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "EspeciesFiltrar"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EspeciesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function EspeciesConsultar(ByVal pId As String, ByVal pNombre As String, ByVal plngIdEmisor As Integer, ByVal plngIDClase As Integer, ByVal plogActivo As Boolean, ByVal pdtmEmision As System.Nullable(Of Date), ByVal pdtmVencimiento As System.Nullable(Of Date), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Especi)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Especies_Consultar(pId, pNombre, plngIdEmisor, plngIDClase, plogActivo, pdtmEmision, pdtmVencimiento, DemeInfoSesion(pstrUsuario, "BuscarEspecies"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarEspecies")
            Return Nothing
        End Try
    End Function

    Public Function TraerEspeciePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Especi
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Especi
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.Id = 
            'e.Nombre = 
            e.EsAccion = True
            e.IDClase = 1
            'e.IDTarifa = 
            e.IDGrupo = 5
            e.IDSubGrupo = 6
            'e.VlrNominal = 
            'e.Notas = 
            'e.IdEmisor = 
            'e.IDAdmonEmision = 
            'e.LeyCirculacion = 
            'e.Emision = 
            'e.Vencimiento = 
            'e.Modalidad = 
            'e.TasaInicial = 
            'e.TasaNominal = 
            'e.PeriodoPago = 
            'e.DiaDesde = 
            'e.DiaHasta = 
            'e.Mercado = 
            e.DeclaraDividendos = True
            e.TituloMaterializado = True
            'e.Sector = 
            e.Activo = True
            'e.Emisora = 
            'e.TipoTasaFija = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.BusIntegracion = 
            'e.Suscripcion = 
            'e.CaracteristicasRF = 
            'e.Bursatilidad = 
            'e.ClaseInversion = 
            'e.Corresponde = 
            'e.BaseCalculoInteres = 
            'e.RefTasaVble = 
            'e.Amortiza = 
            'e.ClaseAcciones = 
            e.Liquidez = False
            e.Negociable = True
            e.IDBolsa = 4
            'e.NroAcciones
            'e.IDEspecies = 

            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerEspeciePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertEspecie(ByVal Especie As Especi)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Especie.pstrUsuarioConexion, Especie.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Especie.InfoSesion = DemeInfoSesion(Especie.pstrUsuarioConexion, "InsertEspecie")
            Me.DataContext.Especi.InsertOnSubmit(Especie)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertEspecie")
        End Try
    End Sub

    Public Sub UpdateEspecie(ByVal currentEspecie As Especi)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentEspecie.pstrUsuarioConexion, currentEspecie.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentEspecie.InfoSesion = DemeInfoSesion(currentEspecie.pstrUsuarioConexion, "UpdateEspecie")
            Me.DataContext.Especi.Attach(currentEspecie, Me.ChangeSet.GetOriginal(currentEspecie))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateEspecie")
        End Try
    End Sub

    Public Sub DeleteEspecie(ByVal Especi As Especi)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Especi.pstrUsuarioConexion, Especi.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Especies_Eliminar( pId,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteEspecie"),0).ToList
            Especi.InfoSesion = DemeInfoSesion(Especi.pstrUsuarioConexion, "DeleteEspecie")
            Me.DataContext.Especi.Attach(Especi)
            Me.DataContext.Especi.DeleteOnSubmit(Especi)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteEspecie")
        End Try
    End Sub

    ''' <summary>
    ''' Se verifica si el nemotécnico ya existe.
    ''' </summary>
    ''' <param name="strNemo"></param>
    ''' <param name="plngIdBolsa"></param>
    ''' <returns></returns>
    ''' <remarks>SLB20121109</remarks>
    Public Function ExisteNemoIngresado(ByVal strNemo As String, ByVal plngIdBolsa As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Dim gstrsql = "SELECT strNombre FROM tblEspecies WHERE strID = '" & strNemo & "'"
            'Dim ret = Me.DataContext.ExecuteQuery(Of String)(gstrsql)
            'Dim resultado = ret(0)
            'If Not IsNothing(resultado) Then
            '    Existe = True
            'End If
            Dim RSExiste = Me.DataContext.uspOyDNet_Maestros_Especies_ExisteNemo(strNemo, plngIdBolsa, DemeInfoSesion(pstrUsuario, "ExisteNemoIngresado"), Constantes.ERROR_PERSONALIZADO_SQLSERVER)
            Return strNemo
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ExisteNemoIngresado")
            Return "True"
        End Try

    End Function

    ''' <summary>
    ''' Inactivar o Activar la Especie Seleccionada.
    ''' </summary>
    ''' <param name="pstrId"></param>
    ''' <param name="plogActivo"></param>
    ''' <returns></returns>
    ''' <remarks>SLB20121109</remarks>
    Public Function InactivarEspecies(ByVal pstrId As String, ByVal plogActivo As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Especies_Eliminar(pstrId, plogActivo, DemeInfoSesion(pstrUsuario, "ExisteNemoIngresado"), Constantes.ERROR_PERSONALIZADO_SQLSERVER, pstrUsuario)
            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InactivarEspecies")
            Return False
        End Try
    End Function

    Public Function ConsultarValoresDefectoBursatilidad(ByVal pstrBursatilidad As String, ByVal pintEmisor As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrValoresPorDefecto As String = ""
            Me.DataContext.uspCalculosFinancieros_ConsultarValoresDefectoBursatilidad(pstrBursatilidad, pintEmisor, pstrValoresPorDefecto)
            Return pstrValoresPorDefecto
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarValoresDefectoBursatilidadAcciones")
            Return Nothing
        End Try
    End Function

    Private Function ConsultarValoresDefectoBursatilidadAsync(ByVal pstrBursatilidad As String, ByVal pintEmisor As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(ConsultarValoresDefectoBursatilidad(pstrBursatilidad, pintEmisor, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarValoresDefectoBursatilidadSync(ByVal pstrBursatilidad As String, ByVal pintEmisor As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.ConsultarValoresDefectoBursatilidadAsync(pstrBursatilidad, pintEmisor, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

#End Region

#Region "EspeciesISIN"

    Public Function EspeciesISINFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesISIN)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ISIN_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "EspeciesISINFiltrar"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EspeciesISINFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function EspeciesISINConsultar(ByVal pIdEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesISIN)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ISIN_Consultar(pIdEspecie, DemeInfoSesion(pstrUsuario, "BuscarEspeciesISIN"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarEspeciesISIN")
            Return Nothing
        End Try
    End Function

    Public Function TraerEspeciesISINPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As EspeciesISIN
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New EspeciesISIN
            'e.strISIN =
            'e.strDescripcion =
            e.IDEspecie = " "
            e.Usuario = HttpContext.Current.User.Identity.Name
            e.IDEspeciesISIN = -1
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerEspeciesISINPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertEspeciesISIN(ByVal EspeciesISIN As EspeciesISIN)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,EspeciesISIN.pstrUsuarioConexion, EspeciesISIN.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            EspeciesISIN.InfoSesion = DemeInfoSesion(EspeciesISIN.pstrUsuarioConexion, "InsertEspeciesISIN")
            Me.DataContext.EspeciesISIN.InsertOnSubmit(EspeciesISIN)

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertEspeciesISIN")
        End Try
    End Sub

    Public Sub UpdateEspeciesISIN(ByVal currentEspeciesISIN As EspeciesISIN)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentEspeciesISIN.pstrUsuarioConexion, currentEspeciesISIN.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentEspeciesISIN.InfoSesion = DemeInfoSesion(currentEspeciesISIN.pstrUsuarioConexion, "UpdateEspeciesISIN")
            Me.DataContext.EspeciesISIN.Attach(currentEspeciesISIN, Me.ChangeSet.GetOriginal(currentEspeciesISIN))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateEspeciesISIN")
        End Try
    End Sub

    Public Sub DeleteEspeciesISIN(ByVal EspeciesISIN As EspeciesISIN)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,EspeciesISIN.pstrUsuarioConexion, EspeciesISIN.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ISIN_Eliminar( pIdEspecies, DemeInfoSesion(pstrUsuario, "DeleteEspeciesISIN"),0).ToList
            EspeciesISIN.InfoSesion = DemeInfoSesion(EspeciesISIN.pstrUsuarioConexion, "DeleteEspeciesISIN")
            Me.DataContext.EspeciesISIN.Attach(EspeciesISIN)
            Me.DataContext.EspeciesISIN.DeleteOnSubmit(EspeciesISIN)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteEspeciesISIN")
        End Try
    End Sub
#End Region

#Region "EspeciesISINFungible"

    Public Function EspeciesISINFungibleFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesISINFungible)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ISINFungible_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "EspeciesISINFungibleFiltrar"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EspeciesISINFungibleFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function EspeciesISINFungibleConsultar(ByVal pIdEspecie As String, ByVal pstrISIN As String, ByVal pstrtipotasa As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesISINFungible)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ISINFungible_Consultar(pIdEspecie, pstrISIN, pstrtipotasa, DemeInfoSesion(pstrUsuario, "BuscarEspeciesISINFungible"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarEspeciesISINFungible")
            Return Nothing
        End Try
    End Function

    Public Function TraerEspeciesISINFungiblePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As EspeciesISINFungible
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New EspeciesISINFungible
            e.IDEspecie = " "
            'e.ISIN = 
            'e.IDFungible = 
            'e.Emision = 
            'e.Usuario = HttpContext.Current.User.Identity.Name
            e.IDIsinFungible = 0
            e.Amortizada = False
            e.logFlujosIrregulares = False
            e.logPoseeRetencion = False
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerEspeciesISINFungiblePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertEspeciesISINFungible(ByVal EspeciesISINFungible As EspeciesISINFungible)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,EspeciesISINFungible.pstrUsuarioConexion, EspeciesISINFungible.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            EspeciesISINFungible.InfoSesion = DemeInfoSesion(EspeciesISINFungible.pstrUsuarioConexion, "InsertEspeciesISINFungible")
            Me.DataContext.EspeciesISINFungible.InsertOnSubmit(EspeciesISINFungible)

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertEspeciesISINFungible")
        End Try
    End Sub

    Public Sub UpdateEspeciesISINFungible(ByVal currentEspeciesISINFungible As EspeciesISINFungible)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentEspeciesISINFungible.pstrUsuarioConexion, currentEspeciesISINFungible.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentEspeciesISINFungible.InfoSesion = DemeInfoSesion(currentEspeciesISINFungible.pstrUsuarioConexion, "UpdateEspeciesISINFungible")
            Me.DataContext.EspeciesISINFungible.Attach(currentEspeciesISINFungible, Me.ChangeSet.GetOriginal(currentEspeciesISINFungible))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateEspeciesISINFungible")
        End Try
    End Sub

    Public Sub DeleteEspeciesISINFungible(ByVal EspeciesISINFungible As EspeciesISINFungible)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,EspeciesISINFungible.pstrUsuarioConexion, EspeciesISINFungible.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ISINFungible_Eliminar( pIdEspecies, DemeInfoSesion(pstrUsuario, "DeleteISINFungible"),0).ToList
            EspeciesISINFungible.InfoSesion = DemeInfoSesion(EspeciesISINFungible.pstrUsuarioConexion, "DeleteEspeciesISINFungible")
            Me.DataContext.EspeciesISINFungible.Attach(EspeciesISINFungible)
            Me.DataContext.EspeciesISINFungible.DeleteOnSubmit(EspeciesISINFungible)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteEspeciesISINFungible")
        End Try
    End Sub

    Public Function EliminarISINFungible(ByVal pintIDISINFungible As Integer, ByVal pstrEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strRetornoValidacion As String = String.Empty
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ISINFungible_Eliminar(pintIDISINFungible, pstrEspecie, DemeInfoSesion(pstrUsuario, "EliminarISINFungible"), 0, strRetornoValidacion)
            Return strRetornoValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarISINFungible")
            Return Nothing
        End Try
    End Function

#Region "Métodos asincrónicos"
    Public Function ISINFungible_Calificacion_Validar(ByVal pdtmEmision As System.Nullable(Of Date), ByVal pdtmVencimiento As System.Nullable(Of Date), ByVal pintIDCalificacionInversion As System.Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Me.DataContext.uspOyDNet_Maestros_ISINFungible_Calificacion_Validar(pdtmEmision, pdtmVencimiento, pintIDCalificacionInversion, pstrMsgValidacion)
            Return pstrMsgValidacion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ISINFungible_Calificacion_Validar")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarPorcentajeRetencion(ByVal intIDConceptoRetencion As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Double
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim dblPorcentajeRetencion As Double
            Me.DataContext.uspCalculosFinanciero_ConceptoRetencion_Consultar(intIDConceptoRetencion, dblPorcentajeRetencion, DemeInfoSesion(pstrUsuario, "ConsultarPorcentajeRetencion"), 0)
            Return dblPorcentajeRetencion
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarPorcentajeRetencion")
            Return Nothing
        End Try
    End Function
#End Region

#Region " Metodos Sincronicos"
    Public Function ConsultarPorcentajeRetencionSync(ByVal pintIDConceptoRetencion As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Double
        Dim objTask As Task(Of Double) = Me.ConsultarPorcentajeRetencionAsync(pintIDConceptoRetencion, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarPorcentajeRetencionAsync(ByVal pintIDConceptoRetencion As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of Double)
        Dim objTaskComplete As TaskCompletionSource(Of Double) = New TaskCompletionSource(Of Double)()
        objTaskComplete.TrySetResult(ConsultarPorcentajeRetencion(pintIDConceptoRetencion, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function
#End Region

#Region "Amortizaciones"

    ' EOMC -- Metodos para manejo de amortizaciones en maestro de ISIN Fungibles -- 08/08/2013

    Public Function AmortizacionesISINConsultar(pintIDIsinFungible As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of AmortizacionesEspeci)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_EspeciesISIN_Amortizaciones_Consultar(pintIDIsinFungible, DemeInfoSesion(pstrUsuario, "AmortizacionesISINConsultar"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AmortizacionesISINConsultar")
            Return Nothing
        End Try
    End Function

    '<Insert()> _
    Public Sub InsertAmortizacionesEspeci(ByVal Amortizacion As AmortizacionesEspeci)
        Try
            ' No hace nada, pero se adiciona el método para que el viewmodel pueda acceder al método add, sin generar error

            'Amortizacion.InfoSesion = DemeInfoSesion(pstrUsuario, "InsertAmortizacionesEspeci")
            'Me.DataContext.AmortizacionesEspecis.InsertOnSubmit(Amortizacion)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertAmortizacionesEspeci")
        End Try
    End Sub

    '<Update()> _
    Public Sub UpdateAmortizacionesEspeci(ByVal Amortizacion As AmortizacionesEspeci)
        Try
            ' No hace nada, pero se adiciona el método para que el viewmodel pueda acceder al método add, sin generar error

            'Amortizacion.InfoSesion = DemeInfoSesion(pstrUsuario, "UpdateAmortizacionesEspeci")
            'Me.DataContext.AmortizacionesEspecis.InsertOnSubmit(Amortizacion)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateAmortizacionesEspeci")
        End Try
    End Sub

    '<Delete()> _
    Public Sub DeleteAmortizacionesEspeci(ByVal Amortizacion As AmortizacionesEspeci)
        Try
            ' No hace nada, pero se adiciona el método para que el viewmodel pueda acceder al método remove, sin generar error

            'Amortizacion.InfoSesion = DemeInfoSesion(pstrUsuario, "DeleteAmortizacionesEspeci")
            'Me.DataContext.AmortizacionesEspecis.Attach(Amortizacion)
            'Me.DataContext.AmortizacionesEspecis.DeleteOnSubmit(Amortizacion)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteAmortizacionesEspeci")
        End Try
    End Sub

#End Region

#Region "Flujos Diarios"

    ' EOMC -- Metodo para consulta de flujos diarios en maestro de ISIN Fungibles -- 08/08/2013

    Public Function EspeciesISINFungibleFlujosDiariosConsultar(ByVal pintIDIsnFungible As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of FlujosDiariosValoracion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_EspeciesISIN_Flujos_Diarios_Consultar(pintIDIsnFungible, DemeInfoSesion(pstrUsuario, "EspeciesISINFungibleFlujosDiariosConsultar"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EspeciesISINFungibleFlujosDiariosConsultar")
            Return Nothing
        End Try
    End Function
#End Region

#End Region

#Region "Emisores"

    Public Function EmisoresFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Emisore)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Emisores_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "EmisoresFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EmisoresFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function EmisoresConsultar(ByVal pID As Integer, ByVal pNombre As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Emisore)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Emisores_Consultar(pID, pNombre, DemeInfoSesion(pstrUsuario, "BuscarEmisores"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarEmisores")
            Return Nothing
        End Try
    End Function

    Public Function TraerEmisorePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Emisore
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Emisore
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            e.ID = -1
            'e.Nombre = 
            'e.NIT = 
            'e.Telefono1 = 
            'e.Telefono2 = 
            'e.Fax1 = 
            'e.Fax2 = 
            'e.Direccion = 
            'e.Internet = 
            'e.EMail = 
            e.IDGrupo = 5
            e.IDSubGrupo = 6
            'e.IDPoblacion = 
            'e.IDDepartamento = 
            'e.IDPais = 
            'e.Actualizacion = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.Contacto = 
            'e.Mostrar = 
            'e.Total = 
            'e.Responde = 
            e.TipoComision = "0"
            'e.IdEmisor = 
            e.logActivo = False
            e.VigiladoSuper = False
            e.logEsPatrimonioAutonomo = False
            e.FuenteExtranjero = False
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerEmisorePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertEmisore(ByVal Emisore As Emisore)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Emisore.pstrUsuarioConexion, Emisore.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Emisore.InfoSesion = DemeInfoSesion(Emisore.pstrUsuarioConexion, "InsertEmisore")
            Me.DataContext.Emisores.InsertOnSubmit(Emisore)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertEmisore")
        End Try
    End Sub

    Public Sub UpdateEmisore(ByVal currentEmisore As Emisore)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentEmisore.pstrUsuarioConexion, currentEmisore.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentEmisore.InfoSesion = DemeInfoSesion(currentEmisore.pstrUsuarioConexion, "UpdateEmisore")
            Me.DataContext.Emisores.Attach(currentEmisore, Me.ChangeSet.GetOriginal(currentEmisore))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateEmisore")
        End Try
    End Sub

    Public Sub DeleteEmisore(ByVal Emisore As Emisore)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Emisore.pstrUsuarioConexion, Emisore.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Emisores_Eliminar( pID,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteEmisore"),0).ToList
            Emisore.InfoSesion = DemeInfoSesion(Emisore.pstrUsuarioConexion, "DeleteEmisore")
            Me.DataContext.Emisores.Attach(Emisore)
            Me.DataContext.Emisores.DeleteOnSubmit(Emisore)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteEmisore")
        End Try
    End Sub

    Public Function Traer_Especies_Emisore(ByVal pID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Especie)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pID) Then
                Dim ret = Me.DataContext.uspOyDNet_Maestros_EspeciesEmisore_Consultar(pID, DemeInfoSesion(pstrUsuario, "Traer_Especies_Emisore"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_Especies_Emisore")
            Return Nothing
        End Try

    End Function
#End Region

#Region "ClasesEspecies"

    Public Function ClasesEspeciesFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ClasesEspecie)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ClasesEspecies_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ClasesEspeciesFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ClasesEspeciesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ClasesEspeciesConsultar(ByVal pIDClaseEspecie As Integer, ByVal pNombre As String, pstrCodInversionSuper As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ClasesEspecie)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ClasesEspecies_Consultar(pIDClaseEspecie, pNombre, DemeInfoSesion(pstrUsuario, "BuscarClasesEspecies"), 0, pstrCodInversionSuper).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarClasesEspecies")
            Return Nothing
        End Try
    End Function

    Public Function TraerClasesEspeciePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ClasesEspecie
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ClasesEspecie
            'e.IDClasesEspecies = -1
            e.IDClaseEspecie = -1
            'e.IDComisionista = -1
            'e.IDSucComisionista = -1
            'e.IDClaseEspecie = 
            e.AplicaAccion = True
            'e.Nombre = 
            'e.Actualizacion = 
            'e.Usuario = HttpContext.Current.User.Identity.Name
            'e.IdProducto = 
            e.TituloCarteraColectiva = False
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerClasesEspeciePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertClasesEspecie(ByVal ClasesEspecie As ClasesEspecie)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ClasesEspecie.pstrUsuarioConexion, ClasesEspecie.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ClasesEspecie.InfoSesion = DemeInfoSesion(ClasesEspecie.pstrUsuarioConexion, "InsertClasesEspecie")
            Me.DataContext.ClasesEspecies.InsertOnSubmit(ClasesEspecie)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertClasesEspecie")
        End Try
    End Sub

    Public Sub UpdateClasesEspecie(ByVal currentClasesEspecie As ClasesEspecie)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentClasesEspecie.pstrUsuarioConexion, currentClasesEspecie.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentClasesEspecie.InfoSesion = DemeInfoSesion(currentClasesEspecie.pstrUsuarioConexion, "UpdateClasesEspecie")
            Me.DataContext.ClasesEspecies.Attach(currentClasesEspecie, Me.ChangeSet.GetOriginal(currentClasesEspecie))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateClasesEspecie")
        End Try
    End Sub

    Public Sub DeleteClasesEspecie(ByVal ClasesEspecie As ClasesEspecie)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ClasesEspecie.pstrUsuarioConexion, ClasesEspecie.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ClasesEspecies_Eliminar( pIDClaseEspecie,  pNombre, DemeInfoSesion(pstrUsuario, "DeleteClasesEspecie"),0).ToList
            ClasesEspecie.InfoSesion = DemeInfoSesion(ClasesEspecie.pstrUsuarioConexion, "DeleteClasesEspecie")
            Me.DataContext.ClasesEspecies.Attach(ClasesEspecie)
            Me.DataContext.ClasesEspecies.DeleteOnSubmit(ClasesEspecie)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteClasesEspecie")
        End Try
    End Sub

    Public Function EliminarClasesEspecie(ByVal pID As Integer, ByVal pstrUsuario As String, ByVal strMsg As String, ByVal pstrUsuarioLlamado As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuarioLlamado, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_ClasesEspecies_Eliminar(pID, pstrUsuario, strMsg, DemeInfoSesion(pstrUsuario, "EliminarClasesEspecie"), 0)
            Return strMsg
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EliminarClasesEspecie")
            Return Nothing
        End Try
    End Function
#End Region

#Region "EspeciesDividendos"

    Public Function EspeciesDividendosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesDividendos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Dividendos_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "EspeciesDividendosFiltrar"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EspeciesDividendosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function EspeciesDividendosConsultar(ByVal pIdEspecies As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesDividendos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Dividendos_Consultar(pIdEspecies, DemeInfoSesion(pstrUsuario, "BuscarEspeciesDividendos"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarEspeciesDividendos")
            Return Nothing
        End Try
    End Function

    Public Function TraerEspeciesDividendoPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As EspeciesDividendos
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New EspeciesDividendos

            e.InicioVigencia = Now.Date
            e.FinVigencia = Now.Date
            'e.Causacion = 
            e.InicioPago = Now.Date
            e.FinPago = Now.Date
            e.CantidadAcciones = 0
            e.Causacion = 0
            e.CantidadPesos = 0
            e.IDCtrlDividendo = "NO"
            e.Usuario = HttpContext.Current.User.Identity.Name
            e.IDEspecie = " "
            e.IDDividendos = -1
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerEspeciesDividendoPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertEspeciesDividendos(ByVal EspeciesDividendos As EspeciesDividendos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,EspeciesDividendos.pstrUsuarioConexion, EspeciesDividendos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            EspeciesDividendos.InfoSesion = DemeInfoSesion(EspeciesDividendos.pstrUsuarioConexion, "InsertEspeciesDividendos")
            Me.DataContext.EspeciesDividendos.InsertOnSubmit(EspeciesDividendos)

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertEspeciesDividendos")
        End Try
    End Sub

    Public Sub UpdateEspeciesDividendos(ByVal currentEspeciesDividendos As EspeciesDividendos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentEspeciesDividendos.pstrUsuarioConexion, currentEspeciesDividendos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentEspeciesDividendos.InfoSesion = DemeInfoSesion(currentEspeciesDividendos.pstrUsuarioConexion, "UpdateEspeciesDividendos")
            Me.DataContext.EspeciesDividendos.Attach(currentEspeciesDividendos, Me.ChangeSet.GetOriginal(currentEspeciesDividendos))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateEspeciesDividendos")
        End Try
    End Sub

    Public Sub DeleteEspeciesDividendos(ByVal pIdEspecies As EspeciesDividendos)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pIdEspecies.pstrUsuarioConexion, pIdEspecies.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Dividendos_Eliminar(pIdEspecies, DemeInfoSesion(pstrUsuario, "DeleteDividendo"), 0).ToList()
            pIdEspecies.InfoSesion = DemeInfoSesion(pIdEspecies.pstrUsuarioConexion, "DeleteEspeciesDividendos")
            Me.DataContext.EspeciesDividendos.Attach(pIdEspecies)
            Me.DataContext.EspeciesDividendos.DeleteOnSubmit(pIdEspecies)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteEspeciesDividendos")
        End Try
    End Sub
#End Region

#Region "EspeciesPrecios"

    Public Function EspeciesPreciosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesPrecios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Precios_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "EspeciesPreciosFiltrar"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EspeciesPreciosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function EspeciesPreciosConsultar(ByVal pIdEspecies As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesPrecios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Precios_Consultar(pIdEspecies, DemeInfoSesion(pstrUsuario, "BuscarEspeciesPrecios"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarEspeciesPrecios")
            Return Nothing
        End Try
    End Function

    Public Function TraerEspeciesPreciosPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As EspeciesPrecios
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New EspeciesPrecios
            'e.IdComisionista = 
            'e.IdSucComisionista = 
            e.IdBolsa = 4
            e.Cierre = Now.Date
            'e.curPrecio =
            e.IDPreciosEspecie = -1
            e.IdEspecie = " "
            e.Usuario = HttpContext.Current.User.Identity.Name
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerEspeciesPreciosPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertEspeciesPrecios(ByVal EspeciesPrecios As EspeciesPrecios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,EspeciesPrecios.pstrUsuarioConexion, EspeciesPrecios.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            EspeciesPrecios.InfoSesion = DemeInfoSesion(EspeciesPrecios.pstrUsuarioConexion, "InsertEspeciesPrecios")
            Me.DataContext.EspeciesPrecios.InsertOnSubmit(EspeciesPrecios)

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertEspeciesPrecios")
        End Try
    End Sub

    Public Sub UpdateEspeciesPrecios(ByVal currentEspeciesPrecios As EspeciesPrecios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentEspeciesPrecios.pstrUsuarioConexion, currentEspeciesPrecios.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentEspeciesPrecios.InfoSesion = DemeInfoSesion(currentEspeciesPrecios.pstrUsuarioConexion, "UpdateEspeciesPrecios")
            Me.DataContext.EspeciesPrecios.Attach(currentEspeciesPrecios, Me.ChangeSet.GetOriginal(currentEspeciesPrecios))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateEspeciesPrecios")
        End Try
    End Sub

    Public Sub DeleteEspeciesPrecios(ByVal EspeciesPrecios As EspeciesPrecios)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,EspeciesPrecios.pstrUsuarioConexion, EspeciesPrecios.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Precios_Eliminar( pIdEspecies, DemeInfoSesion(pstrUsuario, "DeletePrecios"),0).ToList
            EspeciesPrecios.InfoSesion = DemeInfoSesion(EspeciesPrecios.pstrUsuarioConexion, "DeleteEspeciesPrecios")
            Me.DataContext.EspeciesPrecios.Attach(EspeciesPrecios)
            Me.DataContext.EspeciesPrecios.DeleteOnSubmit(EspeciesPrecios)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteEspeciesPrecios")
        End Try
    End Sub
#End Region

#Region "TiposEmisores"
    Public Function TiposEmisoresFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TiposEmisores)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_TiposEmisores_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "TiposEmisoresFiltrar"), 0).ToList
            Return ret

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TiposEmisoresFiltrar")
            Return Nothing
        End Try
    End Function
    Public Function TraerTiposEmisoresPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As TiposEmisores
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New TiposEmisores
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.Usuario_Consecutivo = 
            'e.Nombre_Consecutivo = 
            'e.Actualizacion = 
            e.strUsuario = HttpContext.Current.User.Identity.Name
            'e.IDConsecutivosUsuarios = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerTiposEmisoresPorDefecto")
            Return Nothing
        End Try
    End Function
    Public Function TiposEmisoresConsultar(pintIdTipoEmisor As Integer,
                                                       pintCodTipoEmisor As Integer,
                                                       pintIdEmisor As Integer,
                                                       pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TiposEmisores)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_TiposEmisores_Consultar(pintIdTipoEmisor, pintCodTipoEmisor, pintIdEmisor, pstrUsuario, DemeInfoSesion(pstrUsuario, "TiposEmisoresConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TiposEmisoresConsultar")
            Return Nothing
        End Try
    End Function

    Public Sub InsertTiposEmisores(ByVal Obj As TiposEmisores)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Obj.pstrUsuarioConexion, Obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Obj.InfoSesion = DemeInfoSesion(Obj.pstrUsuarioConexion, "InsertTiposEmisores")
            Me.DataContext.TiposEmisores.InsertOnSubmit(Obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTiposEmisores")
        End Try
    End Sub
    Public Sub UpdateTiposEmisores(ByVal obj As TiposEmisores)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            obj.InfoSesion = DemeInfoSesion(obj.pstrUsuarioConexion, "UpdateTiposEmisores")
            Me.DataContext.TiposEmisores.Attach(obj, Me.ChangeSet.GetOriginal(obj))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTiposEmisores")
        End Try
    End Sub
    Public Sub DeleteTiposEmisores(ByVal obj As TiposEmisores)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_ConsecutivosUsuarios_Eliminar( pUsuario, DemeInfoSesion(pstrUsuario, "DeleteConsecutivosUsuario"),0).ToList
            obj.InfoSesion = DemeInfoSesion(obj.pstrUsuarioConexion, "DeleteTiposEmisores")
            Me.DataContext.TiposEmisores.Attach(obj)
            Me.DataContext.TiposEmisores.DeleteOnSubmit(obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTiposEmisores")
        End Try
    End Sub
#End Region

#Region "EspeciesTotales"

    Public Function EspeciesTotalesConsultar(ByVal pIdEspecies As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesTotales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Totales_Consultar(pIdEspecies, DemeInfoSesion(pstrUsuario, "BuscarEspeciesTotales"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarEspeciesTotales")
            Return Nothing
        End Try
    End Function

#End Region

#Region "EspeciesNemotécnicos"

    Public Function EspeciesNemotecnicosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesNemotecnicos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Nemotecnicos_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "EspeciesNemotecnicosFiltrar"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EspeciesNemotecnicosFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function EspeciesNemotecnicosConsultar(ByVal pIdEspecies As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesNemotecnicos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Nemotecnicos_Consultar(pIdEspecies, DemeInfoSesion(pstrUsuario, "BuscarEspeciesNemotecnicos"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarEspeciesNemotecnicos")
            Return Nothing
        End Try
    End Function

    Public Function TraerEspeciesNemotecnicosPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As EspeciesNemotecnicos
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New EspeciesNemotecnicos
            'e.IdComisionista = 
            'e.IdSucComisionista = 
            e.IDBolsa = 4
            e.IDEspecie = " "
            'e.Nemotecnico = 
            e.Usuario = HttpContext.Current.User.Identity.Name
            e.IDEspeciesBolsa = -1
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerEspeciesNemotecnicosPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertEspeciesNemotecnicos(ByVal EspeciesNemotecnicos As EspeciesNemotecnicos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,EspeciesNemotecnicos.pstrUsuarioConexion, EspeciesNemotecnicos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            EspeciesNemotecnicos.InfoSesion = DemeInfoSesion(EspeciesNemotecnicos.pstrUsuarioConexion, "InsertEspeciesNemotecnicos")
            Me.DataContext.EspeciesNemotecnicos.InsertOnSubmit(EspeciesNemotecnicos)

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertEspeciesNemotecnicos")
        End Try
    End Sub

    Public Sub UpdateEspeciesNemotecnicos(ByVal currentEspeciesNemotecnicos As EspeciesNemotecnicos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentEspeciesNemotecnicos.pstrUsuarioConexion, currentEspeciesNemotecnicos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentEspeciesNemotecnicos.InfoSesion = DemeInfoSesion(currentEspeciesNemotecnicos.pstrUsuarioConexion, "UpdateEspeciesNemotecnicos")
            Me.DataContext.EspeciesNemotecnicos.Attach(currentEspeciesNemotecnicos, Me.ChangeSet.GetOriginal(currentEspeciesNemotecnicos))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateEspeciesNemotecnicos")
        End Try
    End Sub

    Public Sub DeleteEspeciesNemotecnicos(ByVal EspeciesNemotecnicos As EspeciesNemotecnicos)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,EspeciesNemotecnicos.pstrUsuarioConexion, EspeciesNemotecnicos.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Nemotecnicos_Eliminar( pIdEspecies, DemeInfoSesion(pstrUsuario, "DeleteNemotécnico"),0).ToList
            EspeciesNemotecnicos.InfoSesion = DemeInfoSesion(EspeciesNemotecnicos.pstrUsuarioConexion, "DeleteEspeciesNemotecnicos")
            Me.DataContext.EspeciesNemotecnicos.Attach(EspeciesNemotecnicos)
            Me.DataContext.EspeciesNemotecnicos.DeleteOnSubmit(EspeciesNemotecnicos)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteEspeciesNemotecnicos")
        End Try
    End Sub
#End Region

#Region "Unificar especies"

    Public Function UnificarEspecie(ByVal IdRetira As String, ByVal pstrAccion As Char, ByVal IdUnifica As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ConsultaNombreEspecie
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.uspOyDNet_Maestros_Especies_Unificar(IdRetira, pstrAccion, IdUnifica, DemeInfoSesion(pstrUsuario, "ConsultaNombreEspecie"), 0)
            Return Nothing
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UnificarEspecie")
            Return Nothing
        End Try

    End Function
    Public Function TraerUnificarEspecie(ByVal IdRetira As String, ByVal pstrAccion As Char, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConsultaNombreEspecie)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e = Me.DataContext.uspOyDNet_Maestros_Especies_Consultar_Unificar(IdRetira, pstrAccion, DemeInfoSesion(pstrUsuario, "ConsultaNombre"), 0).ToList
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UnificarEspecie")
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' Función para verificar el ISIN si existe, tambien esta función permite Unificar. 
    ''' </summary>
    ''' <param name="pstrIdIsinRetira"></param>
    ''' <param name="pstrAccion"></param>
    ''' <param name="pstrIdisinUnifica"></param>
    ''' <param name="pstrIdEspecie"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UnificarEspecieISIN(ByVal pstrIdIsinRetira As String, ByVal pstrAccion As Char, ByVal pstrIdisinUnifica As String, ByVal pstrIdEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrDescIsin As String = String.Empty
            Me.DataContext.uspOyDNet_Maestros_EspeciesISIN_Unificar(pstrIdIsinRetira, pstrAccion, pstrIdisinUnifica, pstrIdEspecie, pstrDescIsin, DemeInfoSesion(pstrUsuario, "UnificarEspecieISIN"), 0)
            Return pstrDescIsin
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UnificarEspecieISIN")
            Return Nothing
        End Try

    End Function

#End Region

#Region "EspeciesClaseInversion"
#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertEspeciesClaseInversion(ByVal objEspeciesClaseInversion As EspeciesClaseInversion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objEspeciesClaseInversion.pstrUsuarioConexion, objEspeciesClaseInversion.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objEspeciesClaseInversion.InfoSesion = DemeInfoSesion(objEspeciesClaseInversion.pstrUsuarioConexion, "InsertEspeciesClaseInversion")
            Me.DataContext.EspeciesClaseInversion.InsertOnSubmit(objEspeciesClaseInversion)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertEspeciesClaseInversion")
        End Try
    End Sub
    Public Sub UpdateEspeciesClaseInversion(ByVal currentEspeciesClaseInversion As EspeciesClaseInversion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentEspeciesClaseInversion.pstrUsuarioConexion, currentEspeciesClaseInversion.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentEspeciesClaseInversion.InfoSesion = DemeInfoSesion(currentEspeciesClaseInversion.pstrUsuarioConexion, "UpdateEspeciesClaseInversion")
            Me.DataContext.EspeciesClaseInversion.Attach(currentEspeciesClaseInversion, Me.ChangeSet.GetOriginal(currentEspeciesClaseInversion))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateEspeciesClaseInversion")
        End Try
    End Sub
    Public Sub DeleteEspeciesClaseInversion(ByVal objEspeciesClaseInversion As EspeciesClaseInversion)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objEspeciesClaseInversion.pstrUsuarioConexion, objEspeciesClaseInversion.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objEspeciesClaseInversion.InfoSesion = DemeInfoSesion(objEspeciesClaseInversion.pstrUsuarioConexion, "DeleteEspeciesClaseInversion")
            Me.DataContext.EspeciesClaseInversion.Attach(objEspeciesClaseInversion)
            Me.DataContext.EspeciesClaseInversion.DeleteOnSubmit(objEspeciesClaseInversion)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteEspeciesClaseInversion")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"
    Public Function FiltrarEspeciesClaseInversion(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesClaseInversion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_EspeciesClaseInversion_Filtrar(pstrFiltro, pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarEspeciesClaseInversion"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarEspeciesClaseInversion")
            Return Nothing
        End Try
    End Function
    Public Function ConsultarEspeciesClaseInversion(pintIdEspeciesClaseInversion As Integer, pstrDescripcion As String, pstrCodigo As String, pstrMetodoValoracion As String, ByVal pstrUsuario As String, pintTipoTitulo As Integer, pstrFactorRiesgo As String, ByVal pstrInfoConexion As String) As List(Of EspeciesClaseInversion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_EspeciesClaseInversion_Consultar(String.Empty, pintIdEspeciesClaseInversion, pstrDescripcion, pstrCodigo, pstrMetodoValoracion, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarEspeciesClaseInversion"), 0, pintTipoTitulo, pstrFactorRiesgo).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarEspeciesClaseInversion")
            Return Nothing
        End Try
    End Function
    Public Function ConsultarEspeciesClaseInversionPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As EspeciesClaseInversion
        Dim objEspeciesClaseInversion As EspeciesClaseInversion = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_EspeciesClaseInversion_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, -1, String.Empty, String.Empty, String.Empty, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarEspeciesClaseInversionPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER, 0, String.Empty).ToList
            If ret.Count > 0 Then
                objEspeciesClaseInversion = ret.FirstOrDefault
            End If
            Return objEspeciesClaseInversion
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarEspeciesClaseInversionPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function FiltrarEspeciesClaseInversionSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesClaseInversion)
        Dim objTask As Task(Of List(Of EspeciesClaseInversion)) = Me.FiltrarEspeciesClaseInversionAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function FiltrarEspeciesClaseInversionAsync(ByVal pstrFiltro As String, pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of EspeciesClaseInversion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of EspeciesClaseInversion)) = New TaskCompletionSource(Of List(Of EspeciesClaseInversion))()
        objTaskComplete.TrySetResult(FiltrarEspeciesClaseInversion(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarEspeciesClaseInversionSync(ByVal pintIdEspeciesClaseInversion As Integer, ByVal pstrDescripcion As String, ByVal pstrCodigo As String, ByVal pstrMetodoValoracion As String, ByVal pstrUsuario As String, pintTipoTitulo As Integer, pstrFactorRiesgo As String, ByVal pstrInfoConexion As String) As List(Of EspeciesClaseInversion)
        Dim objTask As Task(Of List(Of EspeciesClaseInversion)) = Me.ConsultarEspeciesClaseInversionAsync(pintIdEspeciesClaseInversion, pstrDescripcion, pstrCodigo, pstrMetodoValoracion, pstrUsuario, pintTipoTitulo, pstrFactorRiesgo, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarEspeciesClaseInversionAsync(ByVal pintIdEspeciesClaseInversion As Integer, ByVal pstrDescripcion As String, ByVal pstrCodigo As String, ByVal pstrMetodoValoracion As String, ByVal pstrUsuario As String, pintTipoTitulo As Integer, pstrFactorRiesgo As String, ByVal pstrInfoConexion As String) As Task(Of List(Of EspeciesClaseInversion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of EspeciesClaseInversion)) = New TaskCompletionSource(Of List(Of EspeciesClaseInversion))()
        objTaskComplete.TrySetResult(ConsultarEspeciesClaseInversion(pintIdEspeciesClaseInversion, pstrDescripcion, pstrCodigo, pstrMetodoValoracion, pstrUsuario, pintTipoTitulo, pstrFactorRiesgo, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarEspeciesClaseInversionPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As EspeciesClaseInversion
        Dim objTask As Task(Of EspeciesClaseInversion) = Me.ConsultarEspeciesClaseInversionPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarEspeciesClaseInversionPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of EspeciesClaseInversion)
        Dim objTaskComplete As TaskCompletionSource(Of EspeciesClaseInversion) = New TaskCompletionSource(Of EspeciesClaseInversion)()
        objTaskComplete.TrySetResult(ConsultarEspeciesClaseInversionPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "PreEspecies"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertPreEspecies(ByVal newPreEspecies As PreEspecies)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,newPreEspecies.pstrUsuarioConexion, newPreEspecies.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            newPreEspecies.strInfoSesion = DemeInfoSesion(newPreEspecies.pstrUsuarioConexion, "InsertPreEspecies")
            Me.DataContext.PreEspecies.InsertOnSubmit(newPreEspecies)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertPreEspecies")
        End Try
    End Sub

    Public Sub UpdatePreEspecies(ByVal currentPreEspecies As PreEspecies)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentPreEspecies.pstrUsuarioConexion, currentPreEspecies.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentPreEspecies.strInfoSesion = DemeInfoSesion(currentPreEspecies.pstrUsuarioConexion, "UpdatePreEspecies")
            Me.DataContext.PreEspecies.Attach(currentPreEspecies, Me.ChangeSet.GetOriginal(currentPreEspecies))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdatePreEspecies")
        End Try
    End Sub

    Public Sub DeletePreEspecies(ByVal deletePreEspecies As PreEspecies)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,deletePreEspecies.pstrUsuarioConexion, deletePreEspecies.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            deletePreEspecies.strInfoSesion = DemeInfoSesion(deletePreEspecies.pstrUsuarioConexion, "DeletePreEspecies")
            Me.DataContext.PreEspecies.Attach(deletePreEspecies)
            Me.DataContext.PreEspecies.DeleteOnSubmit(deletePreEspecies)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeletePreEspecies")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"
    Public Function FiltrarPreEspecies(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PreEspecies)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_PreEspecies_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarPreEspecies"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarPreEspecies")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarPreEspecies(ByVal pstrIDEspecie As String, ByVal plogEsAccion As Boolean, ByVal plngIDClase As Integer, ByVal plngIdEmisor As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PreEspecies)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_PreEspecies_Consultar(String.Empty, pstrIDEspecie, plogEsAccion, plngIDClase, plngIdEmisor, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarPreEspecies"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarPreEspecies")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarPreEspeciesPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As PreEspecies
        Dim objPreEspecies As PreEspecies = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_PreEspecies_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, String.Empty, False, 0, 0, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarPreEspeciesPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objPreEspecies = ret.FirstOrDefault
            End If
            Return objPreEspecies
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarPreEspeciesPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function FiltrarPreEspeciesSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PreEspecies)
        Dim objTask As Task(Of List(Of PreEspecies)) = Me.FiltrarPreEspeciesAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function FiltrarPreEspeciesAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of PreEspecies))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of PreEspecies)) = New TaskCompletionSource(Of List(Of PreEspecies))()
        objTaskComplete.TrySetResult(FiltrarPreEspecies(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarPreEspeciesSync(ByVal pstrIDEspecie As String, ByVal plogEsAccion As Boolean, ByVal plngIDClase As Integer, ByVal plngIdEmisor As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PreEspecies)
        Dim objTask As Task(Of List(Of PreEspecies)) = Me.ConsultarPreEspeciesAsync(pstrIDEspecie, plogEsAccion, plngIDClase, plngIdEmisor, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarPreEspeciesAsync(ByVal pstrIDEspecie As String, ByVal plogEsAccion As Boolean, ByVal plngIDClase As Integer, ByVal plngIdEmisor As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of PreEspecies))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of PreEspecies)) = New TaskCompletionSource(Of List(Of PreEspecies))()
        objTaskComplete.TrySetResult(ConsultarPreEspecies(pstrIDEspecie, plogEsAccion, plngIDClase, plngIdEmisor, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarPreEspeciesPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As PreEspecies
        Dim objTask As Task(Of PreEspecies) = Me.ConsultarPreEspeciesPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarPreEspeciesPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of PreEspecies)
        Dim objTaskComplete As TaskCompletionSource(Of PreEspecies) = New TaskCompletionSource(Of PreEspecies)()
        objTaskComplete.TrySetResult(ConsultarPreEspeciesPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "EspeciesDeposito"

#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertEspeciesDeposito(ByVal newEspeciesDeposito As EspeciesDeposito)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,newEspeciesDeposito.pstrUsuarioConexion, newEspeciesDeposito.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            newEspeciesDeposito.strInfoSesion = DemeInfoSesion(newEspeciesDeposito.pstrUsuarioConexion, "InsertEspeciesDeposito")
            Me.DataContext.EspeciesDeposito.InsertOnSubmit(newEspeciesDeposito)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertEspeciesDeposito")
        End Try
    End Sub

    Public Sub UpdateEspeciesDeposito(ByVal currentEspeciesDeposito As EspeciesDeposito)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentEspeciesDeposito.pstrUsuarioConexion, currentEspeciesDeposito.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentEspeciesDeposito.strInfoSesion = DemeInfoSesion(currentEspeciesDeposito.pstrUsuarioConexion, "UpdateEspeciesDeposito")
            Me.DataContext.EspeciesDeposito.Attach(currentEspeciesDeposito, Me.ChangeSet.GetOriginal(currentEspeciesDeposito))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateEspeciesDeposito")
        End Try
    End Sub

    Public Sub DeleteEspeciesDeposito(ByVal deleteEspeciesDeposito As EspeciesDeposito)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,deleteEspeciesDeposito.pstrUsuarioConexion, deleteEspeciesDeposito.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            deleteEspeciesDeposito.strInfoSesion = DemeInfoSesion(deleteEspeciesDeposito.pstrUsuarioConexion, "DeleteEspeciesDeposito")
            Me.DataContext.EspeciesDeposito.Attach(deleteEspeciesDeposito)
            Me.DataContext.EspeciesDeposito.DeleteOnSubmit(deleteEspeciesDeposito)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteEspeciesDeposito")
        End Try
    End Sub
#End Region

#Region "Métodos asincrónicos"

    Public Function ConsultarEspeciesDeposito(ByVal pstrIDEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesDeposito)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Depositos_Consultar(pstrIDEspecie, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarEspeciesDeposito"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarEspeciesDeposito")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarEspeciesDepositoPorDefecto(ByVal pstrIDEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of EspeciesDeposito)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Depositos_Consultar(String.Empty, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarEspeciesDepositoPorDefecto"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarEspeciesDepositoPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#End Region

#Region "Metodos asincronicos"
    <Query(HasSideEffects:=True)>
    Public Function Operaciones_CalculosTasaFacial_Calcular(ByVal pstrTipoCalculo As String,
                                                     ByVal pdblValorNominal As Nullable(Of Double),
                                                     ByVal pdblValorEfectiva As Nullable(Of Double),
                                                     ByVal pstrModalidad As String,
                                                     ByVal pdtmFechaEmision As Nullable(Of Date),
                                                     ByVal pdtmFechaVencimiento As Nullable(Of Date),
                                                     ByVal pintBaseCupon As Nullable(Of Integer),
                                                     ByVal pstrUsuario As String,
                                                     ByVal pstrUsuarioWindows As String,
                                                     ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of Operaciones_CalculosTasaFacial_Calcular)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_CalculosTasaFacial_Calcular(pstrTipoCalculo, pdblValorNominal, pdblValorEfectiva, pstrModalidad,
                                                                                               pdtmFechaEmision, pdtmFechaVencimiento, pintBaseCupon,
                                                                                               pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "Operaciones_CalculosTasaFacial_Calcular"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Operaciones_CalculosTasaFacial_Calcular")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Configuracion Bursatilidad"
#Region "Métodos modelo para activar funcionalidad RIA"
    Public Sub InsertConfiguracionBursatilidad(ByVal objCongifuracionBursatilidad As ConfiguracionBursatilidad)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objCongifuracionBursatilidad.pstrUsuarioConexion, objCongifuracionBursatilidad.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objCongifuracionBursatilidad.strInfoSesion = DemeInfoSesion(objCongifuracionBursatilidad.pstrUsuarioConexion, "InsertConfiguracionBursatilidad")
            Me.DataContext.ConfiguracionBursatilidad.InsertOnSubmit(objCongifuracionBursatilidad)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertConfiguracionBursatilidad")
        End Try
    End Sub

    Public Sub UpdateConfiguracionBursatilidad(ByVal currentConfiguracionBursatilidad As ConfiguracionBursatilidad)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentConfiguracionBursatilidad.pstrUsuarioConexion, currentConfiguracionBursatilidad.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentConfiguracionBursatilidad.strInfoSesion = DemeInfoSesion(currentConfiguracionBursatilidad.pstrUsuarioConexion, "UpdateConfiguracionBursatilidad")
            Me.DataContext.ConfiguracionBursatilidad.Attach(currentConfiguracionBursatilidad, Me.ChangeSet.GetOriginal(currentConfiguracionBursatilidad))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateConfiguracionBursatilidad")
        End Try
    End Sub

    Public Sub DeleteConfiguracionBursatilidad(ByVal objCongifuracionBursatilidad As ConfiguracionBursatilidad)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,objCongifuracionBursatilidad.pstrUsuarioConexion, objCongifuracionBursatilidad.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objCongifuracionBursatilidad.strInfoSesion = DemeInfoSesion(objCongifuracionBursatilidad.pstrUsuarioConexion, "DeleteConfigracionBursatilidad")
            Me.DataContext.ConfiguracionBursatilidad.Attach(objCongifuracionBursatilidad)
            Me.DataContext.ConfiguracionBursatilidad.DeleteOnSubmit(objCongifuracionBursatilidad)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteConfigracionBursatilidad")
        End Try
    End Sub
#End Region
#Region "Métodos asincrónicos"
    Public Function FiltrarConfiguracionBursatilidad(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionBursatilidad)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ConfiguracionBursatilidad_Filtrar(RetornarValorDescodificado(pstrFiltro), pstrUsuario, DemeInfoSesion(pstrUsuario, "FiltrarConfiguracionBursatilidad"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "FiltrarConfiguracionBursatilidad")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarConsultarConfiguracionBursatilidad(pstrBursatilidad As String, plogEntidadVigilada As Boolean?, pstrClaseInversion As String, pstrClaseContable As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionBursatilidad)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ConfiguracionBursatilidad_Consultar(String.Empty, pstrBursatilidad, plogEntidadVigilada, pstrClaseInversion, pstrClaseContable, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConfiguracionBursatilidad"), 0, Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarIndicadores")
            Return Nothing
        End Try
    End Function

    Public Function ConsultarConsultarConfiguracionBursatilidadPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ConfiguracionBursatilidad
        Dim objConfiguracionBursatilidad As ConfiguracionBursatilidad = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_ConfiguracionBursatilidad_Consultar(Constantes.CONSULTAR_DATOS_POR_DEFECTO, String.Empty, False, Nothing, 0, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarConfigracionBursatilidadPorDefecto"), 0, Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            If ret.Count > 0 Then
                objConfiguracionBursatilidad = ret.FirstOrDefault
            End If
            Return objConfiguracionBursatilidad
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarConfiguracionBursatilidadPorDefecto")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"
    Public Function FiltrarConfiguracionBursatilidadSync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionBursatilidad)
        Dim objTask As Task(Of List(Of ConfiguracionBursatilidad)) = Me.FiltrarConfiguracionBursatilidadAsync(pstrFiltro, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function FiltrarConfiguracionBursatilidadAsync(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ConfiguracionBursatilidad))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ConfiguracionBursatilidad)) = New TaskCompletionSource(Of List(Of ConfiguracionBursatilidad))()
        objTaskComplete.TrySetResult(FiltrarConfiguracionBursatilidad(pstrFiltro, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarConfiguracionBursatilidadSync(pstrBursatilidad As String, plogEntidadVigilada As Boolean?, pstrClaseInversion As String, pstrClaseContable As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionBursatilidad)
        Dim objTask As Task(Of List(Of ConfiguracionBursatilidad)) = Me.ConsultarConfiguaracionBursatilidadAsync(pstrBursatilidad, plogEntidadVigilada, pstrClaseInversion, pstrClaseContable, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarConfiguaracionBursatilidadAsync(pstrBursatilidad As String, plogEntidadVigilada As Boolean?, pstrClaseInversion As String, pstrClaseContable As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ConfiguracionBursatilidad))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ConfiguracionBursatilidad)) = New TaskCompletionSource(Of List(Of ConfiguracionBursatilidad))()
        objTaskComplete.TrySetResult(ConsultarConsultarConfiguracionBursatilidad(pstrBursatilidad, plogEntidadVigilada, pstrClaseInversion, pstrClaseContable, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function ConsultarIndicadoresPorDefectoSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ConfiguracionBursatilidad
        Dim objTask As Task(Of ConfiguracionBursatilidad) = Me.ConsultarConfiguracionBursatilidadaPorDefectoAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarConfiguracionBursatilidadaPorDefectoAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of ConfiguracionBursatilidad)
        Dim objTaskComplete As TaskCompletionSource(Of ConfiguracionBursatilidad) = New TaskCompletionSource(Of ConfiguracionBursatilidad)()
        objTaskComplete.TrySetResult(ConsultarConsultarConfiguracionBursatilidadPorDefecto(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region
#End Region

#Region "ConceptoRetencion"

#Region "Métodos asincrónicos"

    Public Function Especies_ConceptoRetencion_Consultar(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConceptoRetencion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Especies_ConceptoRetencion_Consultar(pstrUsuario, DemeInfoSesion(pstrUsuario, "Especies_ConceptoRetencion_Consultar"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Especies_ConceptoRetencion_Consultar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function Especies_ConceptoRetencion_ConsultarSync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConceptoRetencion)
        Dim objTask As Task(Of List(Of ConceptoRetencion)) = Me.Especies_ConceptoRetencion_ConsultarAsync(pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function Especies_ConceptoRetencion_ConsultarAsync(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ConceptoRetencion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ConceptoRetencion)) = New TaskCompletionSource(Of List(Of ConceptoRetencion))()
        objTaskComplete.TrySetResult(Especies_ConceptoRetencion_Consultar(pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "ArbitrajeADR"

    'Métodos para el manejo de la pestaña "Arbitraje ADR" en el maestro "Especies"

    Public Function EspecieArbitrajeADR_Consultar(ByVal pstrEspecie As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.CFEspecies.ArbitrajeADR)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_Especies_ADR_Consultar(pstrEspecie, DemeInfoSesion(pstrUsuario, "EspecieArbitrajeADR_Consultar"), Constantes.ERROR_PERSONALIZADO_SQLSERVER).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "EspecieArbitrajeADR_Consultar")
            Return Nothing
        End Try
    End Function

    Public Sub InsertArbitrajeADR(ByVal objArbitrajeADR As ArbitrajeADR)

    End Sub

    Public Sub UpdateArbitrajeADR(ByVal currentArbitrajeADR As ArbitrajeADR)

    End Sub

    Public Sub DeleteArbitrajeADR(ByVal objArbitrajeADR As ArbitrajeADR)

    End Sub

#End Region

End Class
