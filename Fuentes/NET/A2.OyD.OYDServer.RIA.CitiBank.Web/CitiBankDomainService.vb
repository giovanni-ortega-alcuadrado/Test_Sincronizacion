Option Compare Binary

Option Infer On
Option Strict On
Option Explicit On

Imports A2.OyD.OYDServer.RIA.Web.OyDCitiBank
Imports System.Web
Imports System.Data.Linq
Imports System.Text
Imports System.IO
Imports System.Collections
Imports System.Dynamic
Imports System.Object
Imports System.Web.UI.WebControls
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura

<EnableClientAccess()>
Public Class CitiBankDomainService
    Inherits LinqToSqlDomainService(Of OyDCitiBankDatacontext)

    ''' <summary>
    ''' Asignar la variable time out
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

#Region "ExcepcionesRDIP"

    Public Function ExcepcionesRDIPFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ExcepcionesRDI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ExcepcionesRDIP_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "ExcepcionesRDIPFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ExcepcionesRDIPFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function ExcepcionesRDIPConsultar(ByVal plngidOrden As Integer, ByVal pstrClaseOrden As Char,
                                             ByVal pstrUsuarioAdvertencia As String, ByVal pstrIdEspecie As String,
                                             ByVal pdtmRegistro As DateTime, ByVal plngIDComitente As String,
                                             ByVal pstrClasificacionRiesgoEspecie As Integer, ByVal pstrPerfilInversionistaIR As Integer,
                                             ByVal pdtmFechaComentario As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ExcepcionesRDI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_ExcepcionesRDIP_Consultar_OyDNet(plngidOrden, pstrClaseOrden, pstrUsuarioAdvertencia,
                                                                          pstrIdEspecie, pdtmRegistro, plngIDComitente, pstrClasificacionRiesgoEspecie,
                                                                          pstrPerfilInversionistaIR, pdtmFechaComentario, DemeInfoSesion(pstrUsuario, "ExcepcionesRDIPConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ExcepcionesRDIPFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function TraerExcepcionesRDIPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As ExcepcionesRDI
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New ExcepcionesRDI
            e.idRegistro = 0
            e.idOrden = 2010000079
            e.ClaseOrden = "A"
            e.UsuarioAdvertencia = "AG83077"
            e.IdEspecie = "ECOPETROL"
            e.Registro = Now
            e.IDComitente = "               24"
            e.ClasificacionRiesgoEspecie = 6
            e.PerfilInversionistaIR = 1
            e.Comentario = String.Empty
            e.UsuarioComentario = "AG83077"
            e.FechaComentario = Now
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerExcepcionesRDIPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertExcepcionesRDI(ByVal ExcepcionesRDI As ExcepcionesRDI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ExcepcionesRDI.pstrUsuarioConexion, ExcepcionesRDI.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            ExcepcionesRDI.InfoSesion = DemeInfoSesion(ExcepcionesRDI.pstrUsuarioConexion, "InsertExcepcionesRDI")
            Me.DataContext.ExcepcionesRDIP.InsertOnSubmit(ExcepcionesRDI)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertExcepcionesRDI")
        End Try
    End Sub

    Public Sub UpdateExcepcionesRDI(ByVal currentExcepcionesRDI As ExcepcionesRDI)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentExcepcionesRDI.pstrUsuarioConexion, currentExcepcionesRDI.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentExcepcionesRDI.InfoSesion = DemeInfoSesion(currentExcepcionesRDI.pstrUsuarioConexion, "UpdateExcepcionesRDI")
            Me.DataContext.ExcepcionesRDIP.Attach(currentExcepcionesRDI, Me.ChangeSet.GetOriginal(currentExcepcionesRDI))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateExcepcionesRDI")
        End Try
    End Sub

    Public Sub DeleteExcepcionesRDI(ByVal ExcepcionesRDI As ExcepcionesRDI)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,ExcepcionesRDI.pstrUsuarioConexion, ExcepcionesRDI.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_CitiBank_ExcepcionesRDIP_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteExcepcionesRDI"),0).ToList
            ExcepcionesRDI.InfoSesion = DemeInfoSesion(ExcepcionesRDI.pstrUsuarioConexion, "DeleteExcepcionesRDI")
            Me.DataContext.ExcepcionesRDIP.Attach(ExcepcionesRDI)
            Me.DataContext.ExcepcionesRDIP.DeleteOnSubmit(ExcepcionesRDI)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteExcepcionesRDI")
        End Try
    End Sub
#End Region

#Region "COMUNES"

    Public Function GetAuditorias(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As IQueryable(Of OyDCitiBank.Auditoria)
        Dim objResultado As IQueryable(Of OyDCitiBank.Auditoria) = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objResultado = Me.DataContext.Auditorias
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GetAuditorias")
        End Try
        Return objResultado
    End Function

    Public Function CargarCombos(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OyDCitiBank.ItemCombo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = From c In Me.DataContext.spA2utils_CargarCombos("")
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargarCombose")
            Return Nothing
        End Try
    End Function
#End Region

#Region "CodificacionContable"

    Public Function CodificacionContableFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CodificacionContabl)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CitiBank_CodificacionContable_Filtrar(RetornarValorDescodificado(pstrFiltro), DemeInfoSesion(pstrUsuario, "CodificacionContableFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CodificacionContableFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function CodificacionContableConsultar(ByVal plngIDCodificacion As Integer, ByVal pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CodificacionContabl)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CitiBank_CodificacionContable_Consultar(plngIDCodificacion, pstrModulo, DemeInfoSesion(pstrUsuario, "CodificacionContableConsultar"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ExcepcionesRDIPFiltrar")
            Return Nothing
        End Try
    End Function


    Public Function TraerCodificacionContablPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As CodificacionContabl
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strValorDefectoCombos As String = "-1"

            Dim e As New CodificacionContabl
            'e.IDCodificacion = Nothing
            'e.IDComisionista = Nothing
            'e.IDSucComisionista = Nothing
            'e.Modulo = strValorDefectoCombos
            'e.TipoOperacion = strValorDefectoCombos
            'e.UsarFecha = strValorDefectoCombos
            'e.TipoCliente = strValorDefectoCombos
            'e.Branch = Nothing
            'e.CuentaCosmos = Nothing
            'e.CodigoTransaccion = Nothing
            'e.IndicadorMvto = strValorDefectoCombos
            'e.NroLote = Nothing
            'e.DetalleAdicional = strValorDefectoCombos
            'e.TextoDetalle = ""
            'e.NroReferencia = strValorDefectoCombos
            'e.PorOperacion = Nothing
            'e.VlrAReportar = strValorDefectoCombos
            'e.Producto = ""
            'e.NroBase = Nothing
            e.SucursalContable = False
            'e.ConsecutivoTesoreria = strValorDefectoCombos
            e.Actualizacion = Now.Date
            e.Usuario = HttpContext.Current.User.ToString()
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerCodificacionContablPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertCodificacionContabl(ByVal CodificacionContabl As CodificacionContabl)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,CodificacionContabl.pstrUsuarioConexion, CodificacionContabl.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            CodificacionContabl.InfoSesion = DemeInfoSesion(CodificacionContabl.pstrUsuarioConexion, "InsertCodificacionContabl")

            Me.DataContext.CodificacionContable.InsertOnSubmit(CodificacionContabl)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCodificacionContabl")
        End Try
    End Sub

    Public Sub UpdateCodificacionContabl(ByVal currentCodificacionContabl As CodificacionContabl)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentCodificacionContabl.pstrUsuarioConexion, currentCodificacionContabl.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentCodificacionContabl.InfoSesion = DemeInfoSesion(currentCodificacionContabl.pstrUsuarioConexion, "UpdateCodificacionContabl")
            Me.DataContext.CodificacionContable.Attach(currentCodificacionContabl, Me.ChangeSet.GetOriginal(currentCodificacionContabl))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCodificacionContabl")
        End Try
    End Sub

    Public Sub DeleteCodificacionContabl(ByVal CodificacionContabl As CodificacionContabl)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,CodificacionContabl.pstrUsuarioConexion, CodificacionContabl.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_CitiBank_CodificacionContable_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteCodificacionContabl"),0).ToList
            CodificacionContabl.InfoSesion = DemeInfoSesion(CodificacionContabl.pstrUsuarioConexion, "DeleteCodificacionContabl")
            Me.DataContext.CodificacionContable.Attach(CodificacionContabl)
            Me.DataContext.CodificacionContable.DeleteOnSubmit(CodificacionContabl)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCodificacionContabl")
        End Try
    End Sub


    Public Function ListarConsecutivosDocumentos(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of A2.OyD.OYDServer.RIA.Web.OyDCitiBank.ConsecutivosDocumento)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return Me.DataContext.uspOyDNet_CitiBank_ListaConsecutivos(pstrUsuario).ToList()
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ListarConsecutivosDocumentos")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Traslado de saldo"

    ''' <summary>
    ''' Operacion encargada de consultar el saldo de un Comitente.
    ''' </summary>
    ''' <param name="SaldoDisponibleChequeado">Boolean</param>
    ''' <param name="CodigoCliente">String</param>
    ''' <param name="Fecha">Date</param>
    ''' <param name="Usuario">String</param>
    ''' <returns>Decimal</returns>
    ''' <remarks>
    ''' Nombre	        :	SaldoConsultar
    ''' Creado por	    :	Juan Carlos Soto Cruz.
    ''' Fecha			:	Agosto 20/2011
    ''' Pruebas CB		:	Juan Carlos Soto Cruz - Agosto 23/2011 - Resultado Ok
    ''' </remarks>
    Public Function SaldoConsultar(ByVal SaldoDisponibleChequeado As Boolean, ByVal CodigoCliente As String, ByVal Fecha As Date, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Decimal
        'Dim SaldoCorte As Decimal
        Dim SaldoCorte As System.Nullable(Of Decimal) = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.uspOyDNet_Clientes_SaldoDisponible_ConsultarSaldo(SaldoDisponibleChequeado, CodigoCliente, Fecha, SaldoCorte, DemeInfoSesion(pstrUsuario, "SaldoConsultar"), ClsConstantes.GINT_ErrorPersonalizado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "SaldoConsultar")
        End Try
        Return CDec(SaldoCorte)
    End Function

    ''' <summary>
    ''' Operacion engargada de consultar si un comitente posee saldo pendiente por aprobar.
    ''' </summary>
    ''' <param name="IDComitente">Codigo del comitente para el cual se va a realizar la consulta.</param>
    ''' <param name="Usuario">Se debe enviar Program.usuario</param>
    ''' <returns>Retorna el saldo pendiente por aprobar si lo posee de lo contrario retorna nulo.</returns>
    ''' <remarks>
    ''' Nombre	        :	PendientePorAprobarConsultar
    ''' Creado por	    :	Juan Carlos Soto Cruz.
    ''' Fecha			:	Agosto 20/2011
    ''' Pruebas CB		:	Juan Carlos Soto Cruz - Agosto 23/2011 - Resultado Ok
    ''' </remarks>
    Public Function PendientePorAprobarConsultar(ByVal IDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Decimal)
        Dim ValorPendientePA As System.Nullable(Of Decimal) = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.uspOyDNet_TrasladarSaldo_ConsultarPendientePorAprobar(IDComitente, ValorPendientePA, DemeInfoSesion(pstrUsuario, "PendientePorAprobarConsultar"), ClsConstantes.GINT_ErrorPersonalizado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PendientePorAprobarConsultar")
        End Try
        Return ValorPendientePA
    End Function

    ''' <summary>
    ''' Operacion encargada de ejecutar el proceso de traslado de saldo.
    ''' </summary>
    ''' <param name="TipoNota">ND o NC</param>
    ''' <param name="NombreConsecutivo">Nombre del consecutivo dependiendo del tipo de Nota.</param>
    ''' <param name="IDComitente">Codigo del comitente para el cual se va a realizar el traslado de saldo.</param>
    ''' <param name="ValorATrasladar">Valor a trasladar.</param>
    ''' <param name="IdCuentaContable">Codigo de la cuenta contable.</param>
    ''' <param name="Documento">Fecha del documento</param>
    ''' <param name="ModuloDestino">Indica cual radio button fue chequeado en pantalla para el modulo de destino del traslado.</param>
    ''' <param name="NombreCliente">El nombre del comitente para el cual se va a realizar el traslado de saldo.</param>
    ''' <param name="Usuario">Se debe enviar Program.usuario</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' Nombre	        :	GrabarNota
    ''' Creado por	    :	Juan Carlos Soto Cruz.
    ''' Fecha			:	Agosto 20/2011
    ''' Pruebas CB		:	Juan Carlos Soto Cruz - Agosto 23/2011 - Resultado Ok
    ''' </remarks>
    Public Function GrabarNota(ByVal TipoNota As String, ByVal NombreConsecutivo As String, ByVal IDComitente As String, ByVal ValorATrasladar As Decimal,
                               ByVal IdCuentaContable As String, ByVal Documento As DateTime, ByVal ModuloDestino As Integer, ByVal NombreCliente As String,
                               ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Integer)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.uspOyDNet_TrasladarSaldo_GrabarNota(TipoNota, NombreConsecutivo, IDComitente, ValorATrasladar, IdCuentaContable, Documento,
                                                               ModuloDestino, NombreCliente, pstrUsuario, DemeInfoSesion(pstrUsuario, "GrabarNota"),
                                                               ClsConstantes.GINT_ErrorPersonalizado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GrabarNota")
        End Try
    End Function

#End Region



End Class




