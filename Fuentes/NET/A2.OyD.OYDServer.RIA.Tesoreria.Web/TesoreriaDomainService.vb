Option Compare Binary
Option Infer On
Option Strict On
Option Explicit On

Imports A2.OyD.OYDServer.RIA.Web.OyDTesoreria
Imports System.Web
Imports System.Data.Linq
Imports System.Web.UI.WebControls
Imports System.Text
Imports System.Threading.Tasks
Imports System.Transactions
Imports System.Data.SqlClient
Imports System.IO
Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.LinqToSql
Imports OpenRiaServices.DomainServices.Server
Imports A2.OyD.Infraestructura

<EnableClientAccess()>
Partial Public Class TesoreriaDomainService
    Inherits LinqToSqlDomainService(Of OyDTesoreriaDatacontext)
    Public pruebadiccionario As New Dictionary(Of String, String)
    Dim contadorprueba As Integer
    Private L2SDC As New OyDTesoreriaDatacontext
    Public Const CSTR_NOMBREPROCESO_ADMONFACTURASFIRMAS = "TESORERIA_ADMONFACTURASFIRMAS"
    Private Shared SEPARATOR_FORMAT_CVS As String = System.Globalization.CultureInfo.CurrentCulture.Parent.TextInfo.ListSeparator
    Dim RETORNO As Boolean
    Dim Usuario As String = ""

    Public Sub New()
        Me.DataContext.CommandTimeout = 0
    End Sub

    Public Overrides Function Submit(ByVal changeSet As OpenRiaServices.DomainServices.Server.ChangeSet) As Boolean
        Dim result As Boolean

        Using tx = New TransactionScope(
                TransactionScopeOption.Required,
                New TransactionOptions With {.IsolationLevel = IsolationLevel.ReadCommitted})

            result = MyBase.Submit(changeSet)
            If (Not Me.ChangeSet.HasError) Then
                tx.Complete()
            End If
        End Using
        Return result
    End Function

    Private _ListaTesoreria As List(Of ReporteExcelTesoreria)
    Public Property ListaTesoreria() As List(Of ReporteExcelTesoreria)
        Get
            Return _ListaTesoreria
        End Get
        Set(ByVal value As List(Of ReporteExcelTesoreria))
            _ListaTesoreria = value
        End Set
    End Property

#Region "ObservacionesRecibosdeCajaTesoreria"

    Public Function TesoreriaAdicionalesFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TesoreriaAdicionale)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ObservacionesTesoreria_Filtrar(pstrFiltro, DemeInfoSesion(pstrUsuario, "TesoreriaAdicionalesFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TesoreriaAdicionalesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function TraerTesoreriaAdicionalePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As TesoreriaAdicionale
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New TesoreriaAdicionale
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.Tipo = 
            'e.NombreConsecutivo = 
            'e.IDDocumento = 
            'e.Observacion = 
            'e.Actualizacion = 
            'e.Usuario = 
            'e.IDTesoreriaAdicionales = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerTesoreriaAdicionalePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertTesoreriaAdicionale(ByVal TesoreriaAdicionale As TesoreriaAdicionale)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,TesoreriaAdicionale.pstrUsuarioConexion, TesoreriaAdicionale.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            TesoreriaAdicionale.InfoSesion = DemeInfoSesion(TesoreriaAdicionale.pstrUsuarioConexion, "InsertTesoreriaAdicionale")
            Me.DataContext.TesoreriaAdicionales.InsertOnSubmit(TesoreriaAdicionale)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTesoreriaAdicionale")
        End Try
    End Sub

    Public Sub UpdateTesoreriaAdicionale(ByVal currentTesoreriaAdicionale As TesoreriaAdicionale)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentTesoreriaAdicionale.pstrUsuarioConexion, currentTesoreriaAdicionale.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentTesoreriaAdicionale.InfoSesion = DemeInfoSesion(currentTesoreriaAdicionale.pstrUsuarioConexion, "UpdateTesoreriaAdicionale")
            Me.DataContext.TesoreriaAdicionales.Attach(currentTesoreriaAdicionale, Me.ChangeSet.GetOriginal(currentTesoreriaAdicionale))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTesoreriaAdicionale")
        End Try
    End Sub

    Public Sub DeleteTesoreriaAdicionale(ByVal TesoreriaAdicionale As TesoreriaAdicionale)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,TesoreriaAdicionale.pstrUsuarioConexion, TesoreriaAdicionale.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Tesoreria_TesoreriaAdicionales_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteTesoreriaAdicionale"),0).ToList
            TesoreriaAdicionale.InfoSesion = DemeInfoSesion(TesoreriaAdicionale.pstrUsuarioConexion, "DeleteTesoreriaAdicionale")
            Me.DataContext.TesoreriaAdicionales.Attach(TesoreriaAdicionale)
            Me.DataContext.TesoreriaAdicionales.DeleteOnSubmit(TesoreriaAdicionale)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTesoreriaAdicionale")
        End Try
    End Sub
#End Region

#Region "ChequesRecibosdeCajaTesoreria"

    Public Function ChequesFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Cheque)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ChequesTesoreria_Filtrar(pstrFiltro, DemeInfoSesion(pstrUsuario, "ChequesFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ChequesFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function TraerChequePorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Cheque
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Cheque
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.NombreConsecutivo = 
            'e.IDDocumento = 
            'e.Secuencia = 
            'e.Efectivo = 
            'e.BancoGirador = 
            'e.NumCheque = 
            'e.Valor = 
            'e.BancoConsignacion = 
            'e.Consignacion = 
            'e.FormaPagoRC = 
            'e.Actualizacion = 
            'e.Usuario = 
            'e.Comentario = 
            'e.IdProducto = 
            'e.SucursalesBancolombia = 
            'e.IDCheques = 
            'e.ChequeHizoCanje = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerChequePorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertCheque(ByVal Cheque As Cheque)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Cheque.pstrUsuarioConexion, Cheque.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Cheque.InfoSesion = DemeInfoSesion(Cheque.pstrUsuarioConexion, "InsertCheque")
            Me.DataContext.Cheques.InsertOnSubmit(Cheque)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertCheque")
        End Try
    End Sub

    Public Sub UpdateCheque(ByVal currentCheque As Cheque)
        'esta funcionalidad se implemento para make and cheked debido a que cuando se va a eliminar no se pueden enviar parametros desde el viewmodel hasta el domaninservice 
        'ya que cuando llega al metodo de eliminacion el no toma los parametros enviados desde el viewmodel si no los originales
        'Se modifico el la condición del if Retiro por RetiroDetalle
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentCheque.pstrUsuarioConexion, currentCheque.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(currentCheque.Estado) Then
                If currentCheque.Estado.Equals("Retiro") Then
                    currentCheque.InfoSesion = DemeInfoSesion(currentCheque.pstrUsuarioConexion, "UpdateCheque")
                    Me.DataContext.Cheques.Attach(currentCheque)
                    Me.DataContext.Cheques.DeleteOnSubmit(currentCheque)
                Else
                    currentCheque.InfoSesion = DemeInfoSesion(currentCheque.pstrUsuarioConexion, "UpdateCheque")
                    Me.DataContext.Cheques.Attach(currentCheque, Me.ChangeSet.GetOriginal(currentCheque))
                End If
            Else
                currentCheque.InfoSesion = DemeInfoSesion(currentCheque.pstrUsuarioConexion, "UpdateCheque")
                Me.DataContext.Cheques.Attach(currentCheque, Me.ChangeSet.GetOriginal(currentCheque))

            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCheque")
        End Try
    End Sub

    Public Sub DeleteCheque(ByVal Cheque As Cheque)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Cheque.pstrUsuarioConexion, Cheque.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Tesoreria_Cheques_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteCheque"),0).ToList
            Cheque.InfoSesion = DemeInfoSesion(Cheque.pstrUsuarioConexion, "DeleteCheque")
            Me.DataContext.Cheques.Attach(Cheque)
            Me.DataContext.Cheques.DeleteOnSubmit(Cheque)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteCheque")
        End Try
    End Sub

    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Este método sirve para controlar que no se realicen modificaciones con el summitchanges.
    ''' Fecha            : Mayo 24/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Mayo 24/2013 - Resultado Ok 
    <OpenRiaServices.DomainServices.Server.Update()>
    Public Sub UpdateChequesPorAprobar(ByVal currentChequesxAprobar As OyDTesoreria.ChequesPorAprobar)
        Try
            Throw New NotImplementedException("No se puede modificar")
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateChequesPorAprobar")
        End Try
    End Sub
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Se consultan los cheques pendientes por aprobar/desaprobar
    ''' Fecha            : Mayo 24/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Mayo 24/2013 - Resultado Ok 
    Public Function ConsultarChequesPorAprobar(ByVal pdtmFechaCorte As DateTime, ByVal pstrUsuario As String, ByVal pintIDCompania As System.Nullable(Of System.Int32), ByVal pstrInfoConexion As String) As List(Of ChequesPorAprobar)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_TesoreriaChequesPorAprobar_Consultar(pdtmFechaCorte, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarChequesPorAprobar"), 0, pintIDCompania).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarCheques")
            Return Nothing
        End Try
    End Function
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Este método es para cambiar el estado al cheque, para que quede aprobado.
    ''' Fecha            : Mayo 24/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Mayo 24/2013 - Resultado Ok 
    Public Function Cheques_Aprobar(ByVal pstrNombreConsecutivo As String,
                                    ByVal plngIDDocumento As Integer,
                                    ByVal plngSecuencia As Integer,
                                    ByVal pdtmConsignacion As Date,
                                    ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_TesoreriaChequesAprobar_Insertar(pstrNombreConsecutivo,
                                                                                plngIDDocumento,
                                                                                plngSecuencia,
                                                                                pdtmConsignacion,
                                                                                pstrUsuario,
                                                                                DemeInfoSesion(pstrUsuario, "AprobarCheques"),
                                                                                0)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AprobarCheques")
            Return Nothing
        End Try
    End Function
    ''' Agregado por     : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Este método es para desaprobar cheques.
    ''' Fecha            : Mayo 24/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Mayo 24/2013 - Resultado Ok 
    Public Function Cheques_Desaprobar(ByVal pstrTipo As String,
                                       ByVal plngIDDocumento As Double,
                                       ByVal plngSecuencia As Integer,
                                       ByVal pstrNombreConsecutivo As String,
                                       ByVal plngIDComitente As String,
                                       ByVal pstrConsecutivoNota As String,
                                       ByVal pdtmFechaCorte As Date,
                                       ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_TesoreriaChequesDesaprobar_Insertar(pstrTipo,
                                                                                   plngIDDocumento,
                                                                                   plngSecuencia,
                                                                                   pstrNombreConsecutivo,
                                                                                   plngIDComitente,
                                                                                   pstrConsecutivoNota,
                                                                                   pdtmFechaCorte,
                                                                                   pstrUsuario,
                                                                                   DemeInfoSesion(pstrUsuario, "DesaprobarCheques"),
                                                                                   0)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DesaprobarCheques")
            Return Nothing
        End Try
    End Function
#End Region

#Region "DetalleTesoreria"

    Public Function DetalleTesoreriaFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DetalleTesoreri)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Maestros_DetalleTesoreria_Filtrar(pstrFiltro, DemeInfoSesion(pstrUsuario, "DetalleTesoreriaFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DetalleTesoreriaFiltrar")
            Return Nothing
        End Try
    End Function



    Public Function TraerDetalleTesoreriPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As DetalleTesoreri
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New DetalleTesoreri
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.Tipo = 
            'e.NombreConsecutivo =
            'e.IDDocumento = 
            'e.Secuencia = 
            'e.IDComitente = 
            'e.Valor = 
            'e.IDCuentaContable = 
            'e.Detalle = 
            'e.IDBanco = 
            'e.NIT = 
            'e.CentroCosto = 
            'e.Actualizacion = 
            'e.Usuario = 
            'e.EstadoTransferencia = 
            'e.BancoDestino = 
            'e.CuentaDestino = 
            'e.TipoCuenta = 
            'e.IdentificacionTitular = 
            'e.Titular = 
            'e.TipoIdTitular = 
            'e.FormaEntrega = 
            'e.Beneficiario = 
            'e.TipoIdentBeneficiario = 
            'e.IdentificacionBenficiciario = 
            'e.NombrePersonaRecoge = 
            'e.IdentificacionPerRecoge = 
            'e.OficinaEntrega = 
            'e.IDDetalleTesoreria = 
            'e.NombreConsecutivoNotaGMF = 
            'e.IDDocumentoNotaGMF = 
            'e.Exportados = 
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerDetalleTesoreriPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertDetalleTesoreri(ByVal DetalleTesoreri As DetalleTesoreri)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,DetalleTesoreri.pstrUsuarioConexion, DetalleTesoreri.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            DetalleTesoreri.InfoSesion = DemeInfoSesion(DetalleTesoreri.pstrUsuarioConexion, "InsertDetalleTesoreri")
            Me.DataContext.DetalleTesoreria.InsertOnSubmit(DetalleTesoreri)
            If DetalleTesoreri.ConsecutivoConsignacionOPT <> "" Then
                Me.DataContext.uspOyDNet_Tesoreria_OrdenesTesoreriaPendientes_CambiarEstado(DetalleTesoreri.ConsecutivoConsignacionOPT, DetalleTesoreri.Nombre, DetalleTesoreri.Valor, DemeInfoSesion(DetalleTesoreri.pstrUsuarioConexion, "CambiarEstadoOPT"), 0)
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertDetalleTesoreri")
        End Try
    End Sub

    Public Sub UpdateDetalleTesoreri(ByVal currentDetalleTesoreri As DetalleTesoreri)
        'esta funcionalidad se implemento para make and cheked debido a que cuando se va a eliminar no se pueden enviar parametros desde el viewmodel hasta el domaninservice 
        'ya que cuando llega al metodo de eliminacion el no toma los parametros enviados desde el viewmodel si no los originales
        'Se modifico el la condición del if Retiro por RetiroDetalle
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentDetalleTesoreri.pstrUsuarioConexion, currentDetalleTesoreri.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(currentDetalleTesoreri.EstadoMC) Then
                If currentDetalleTesoreri.EstadoMC.Equals("Retiro") Then
                    currentDetalleTesoreri.InfoSesion = DemeInfoSesion(currentDetalleTesoreri.pstrUsuarioConexion, "UpdateDetalleTesoreri")
                    Me.DataContext.DetalleTesoreria.Attach(currentDetalleTesoreri)
                    Me.DataContext.DetalleTesoreria.DeleteOnSubmit(currentDetalleTesoreri)
                Else
                    currentDetalleTesoreri.InfoSesion = DemeInfoSesion(currentDetalleTesoreri.pstrUsuarioConexion, "UpdateDetalleTesoreri")
                    Me.DataContext.DetalleTesoreria.Attach(currentDetalleTesoreri, Me.ChangeSet.GetOriginal(currentDetalleTesoreri))
                End If
            Else
                currentDetalleTesoreri.InfoSesion = DemeInfoSesion(currentDetalleTesoreri.pstrUsuarioConexion, "UpdateDetalleTesoreri")
                Me.DataContext.DetalleTesoreria.Attach(currentDetalleTesoreri, Me.ChangeSet.GetOriginal(currentDetalleTesoreri))

            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateDetalleTesoreri")
        End Try
    End Sub

    Public Sub DeleteDetalleTesoreri(ByVal DetalleTesoreri As DetalleTesoreri)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,DetalleTesoreri.pstrUsuarioConexion, DetalleTesoreri.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_DetalleTesoreria_Eliminar( DemeInfoSesion(pstrUsuario, "DeleteDetalleTesoreri"),0).ToList
            DetalleTesoreri.InfoSesion = DemeInfoSesion(DetalleTesoreri.pstrUsuarioConexion, "DeleteDetalleTesoreri")
            Me.DataContext.DetalleTesoreria.Attach(DetalleTesoreri)
            Me.DataContext.DetalleTesoreria.DeleteOnSubmit(DetalleTesoreri)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteDetalleTesoreri")
        End Try
    End Sub

    Public Function CargaMasivaDetalleTesoreria_Consultar(ByVal pstrTipodocumento As String, ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of DetalleTesoreri)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CargaMasivaDetalleTesoreria_Consultar(pstrTipodocumento, pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "detalletesoreria"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DetalleTesoreriaFiltrar")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Tesoreria"
    '---------------------------------------------------TERMINA OYD PLUS--------------------------------------------------------

    Public Function TesoreriaFiltrar(ByVal pstrFiltro As String, ByVal pstrModulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Tesoreri)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_Filtrar(pstrFiltro, pstrModulo, DemeInfoSesion(pstrUsuario, "TesoreriaFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TesoreriaFiltrar")
            Return Nothing
        End Try
    End Function

    Public Function TesoreriaConsultar(ByVal filtro As Byte, ByVal pTipo As String, ByVal pNombreConsecutivo As String, ByVal pIDDocumento As Integer, ByVal pDocumento As System.Nullable(Of DateTime), ByVal pEstadoMC As String, ByVal idbanco As Integer, ByVal pIDCompania As Nullable(Of Integer), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Tesoreri)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_Consultar(filtro, pTipo, pNombreConsecutivo, pIDDocumento, pDocumento, pEstadoMC, idbanco, DemeInfoSesion(pstrUsuario, "BuscarTesoreria"), 0, pIDCompania).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarTesoreria")
            Return Nothing
        End Try
    End Function

    Public Function TraerTesoreriPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Tesoreri
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New Tesoreri
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            e.Tipo = "N"
            'e.NombreConsecutivo = "NC"
            e.IDDocumento = -1
            e.TipoIdentificacion = "C"
            'e.NroDocumento = 
            'e.Nombre = 
            'e.IDBanco = 
            'e.NumCheque = 
            'e.Valor = 
            'e.Detalle = 
            e.Documento = DateTime.Now
            e.Estado = "P"
            'e.Estado = 
            'e.Impresiones = 
            'e.FormaPagoCE = 
            'e.NroLote = 
            'e.Contabilidad = 
            e.Actualizacion = DateTime.Now
            e.Usuario = HttpContext.Current.User.Identity.Name
            'e.ParametroContable = 
            'e.ImpresionFisica = 
            'e.MultiCliente = 
            'e.CuentaCliente = 
            'e.CodComitente = 
            'e.ArchivoTransferencia = 
            'e.IdNumInst = 
            'e.DVP = 
            'e.Instruccion = 
            'e.IdNroOrden = 
            'e.DetalleInstruccion = 
            e.EstadoNovedadContabilidad = "N"
            'e.eroComprobante_Contabilidad = 
            'e.hadecontabilizacion_Contabilidad = 
            'e.haProceso_Contabilidad = 
            e.EstadoNovedadContabilidad_Anulada = "N"
            e.EstadoAutomatico = "N"
            'e.eroLote_Contabilidad = 
            'e.MontoEscrito = 
            'e.TipoIntermedia = 
            'e.Concepto = 
            'e.RecaudoDirecto = 
            'e.ContabilidadEncuenta = 
            'e.Sobregiro = 
            'e.IdentificacionAutorizadoCheque = 
            'e.NombreAutorizadoCheque = 
            'e.IDTesoreria = 
            'e.ContabilidadENC = 
            'e.NroLoteAntENC = 
            'e.ContabilidadAntENC = 
            'e.IdSucursalBancaria = 
            'e.Creacion = 
            e.FecEstado = DateTime.Now
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerTesoreriPorDefecto")
            Return Nothing
        End Try
    End Function

    Public Sub InsertTesoreri(ByVal Tesoreri As Tesoreri)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Tesoreri.pstrUsuarioConexion, Tesoreri.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Tesoreri.XMLDetalle = HttpUtility.HtmlDecode(Tesoreri.XMLDetalle)
            Tesoreri.XMLDetalleCheques = HttpUtility.HtmlDecode(Tesoreri.XMLDetalleCheques)

            Tesoreri.InfoSesion = DemeInfoSesion(Tesoreri.pstrUsuarioConexion, "InsertTesoreri")
            Me.DataContext.Tesoreria.InsertOnSubmit(Tesoreri)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertTesoreri")
        End Try
    End Sub

    Public Sub UpdateTesoreri(ByVal currentTesoreri As Tesoreri)
        'esta funcionalidad se implemento para make and cheked debido a que cuando se va a eliminar no se pueden enviar parametros desde el viewmodel hasta el domaninservice 
        'ya que cuando llega al metodo de eliminacion el no toma los parametros enviados desde el viewmodel si no los originales
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentTesoreri.pstrUsuarioConexion, currentTesoreri.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(currentTesoreri.Estado) Then
                If currentTesoreri.Estado.Equals("Retiro") Then

                    currentTesoreri.XMLDetalle = HttpUtility.HtmlDecode(currentTesoreri.XMLDetalle)
                    currentTesoreri.XMLDetalleCheques = HttpUtility.HtmlDecode(currentTesoreri.XMLDetalleCheques)

                    currentTesoreri.InfoSesion = DemeInfoSesion(currentTesoreri.pstrUsuarioConexion, "UpdateCliente")
                    Me.DataContext.Tesoreria.Attach(currentTesoreri)
                    Me.DataContext.Tesoreria.DeleteOnSubmit(currentTesoreri)
                Else
                    currentTesoreri.XMLDetalle = HttpUtility.HtmlDecode(currentTesoreri.XMLDetalle)
                    currentTesoreri.XMLDetalleCheques = HttpUtility.HtmlDecode(currentTesoreri.XMLDetalleCheques)

                    currentTesoreri.InfoSesion = DemeInfoSesion(currentTesoreri.pstrUsuarioConexion, "UpdateCliente")
                    Me.DataContext.Tesoreria.Attach(currentTesoreri, Me.ChangeSet.GetOriginal(currentTesoreri))
                End If
            Else
                currentTesoreri.XMLDetalle = HttpUtility.HtmlDecode(currentTesoreri.XMLDetalle)
                currentTesoreri.XMLDetalleCheques = HttpUtility.HtmlDecode(currentTesoreri.XMLDetalleCheques)

                currentTesoreri.InfoSesion = DemeInfoSesion(currentTesoreri.pstrUsuarioConexion, "UpdateCliente")
                Me.DataContext.Tesoreria.Attach(currentTesoreri, Me.ChangeSet.GetOriginal(currentTesoreri))

            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateCliente")
        End Try
    End Sub

    Public Sub DeleteTesoreri(ByVal Tesoreri As Tesoreri)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,Tesoreri.pstrUsuarioConexion, Tesoreri.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            'Me.DataContext.uspOyDNet_Maestros_Tesoreria_Eliminar( pTipo,  pNombreConsecutivo,  pIDDocumento,  pTipoIdentificacion,  pNroDocumento,  pNombre,  pIDBanco,  pNumCheque,  pValor,  pDetalle,  pDocumento,  pEstado,  pEstado,  pImpresiones,  pFormaPagoCE,  pNroLote,  pContabilidad,  pActualizacion,  pUsuario,  pParametroContable,  pImpresionFisica,  pMultiCliente,  pCuentaCliente,  pCodComitente,  pArchivoTransferencia,  pIdNumInst,  pDVP,  pInstruccion,  pIdNroOrden,  pDetalleInstruccion,  pEstadoNovedadContabilidad,  peroComprobante_Contabilidad,  phadecontabilizacion_Contabilidad,  phaProceso_Contabilidad,  pEstadoNovedadContabilidad_Anulada,  pEstadoAutomatico,  peroLote_Contabilidad,  pMontoEscrito,  pTipoIntermedia,  pConcepto,  pRecaudoDirecto,  pContabilidadEncuenta,  pSobregiro,  pIdentificacionAutorizadoCheque,  pNombreAutorizadoCheque,  pIDTesoreria,  pContabilidadENC,  pNroLoteAntENC,  pContabilidadAntENC,  pIdSucursalBancaria,  pCreacion, DemeInfoSesion(pstrUsuario, "DeleteTesoreri"),0).ToList
            Tesoreri.InfoSesion = DemeInfoSesion(Tesoreri.pstrUsuarioConexion, "DeleteTesoreri")
            Me.DataContext.Tesoreria.Attach(Tesoreri)
            Me.DataContext.Tesoreria.DeleteOnSubmit(Tesoreri)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTesoreri")
        End Try
    End Sub

    ''' <summary>
    ''' Función encargada se mover los datos de las tablas temporales a las tablas de Tesoreria
    ''' </summary>
    ''' <param name="pintIDTesoreria">Identity de Tesoreria</param>
    ''' <param name="pstrUsuario">Usuario</param>
    ''' <returns>SLB20130204</returns>
    ''' <remarks>Retorna el Identity de la tabla TblTesoreria</remarks>
    ''' JFSB 20180102 Se agrega el parámetro de la compañía
    Public Function TrasladarTesoreria_TMP_TBL(ByVal pintIDTesoreria As Integer, ByVal pstrUsuario As String, ByVal pintIDCompania As Integer, ByVal pstrUsuarioLlamado As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuarioLlamado, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_Transaccion_TMPTBL_Actualizar(pintIDTesoreria, pstrUsuario, DemeInfoSesion(pstrUsuario, "TrasladarTesoreria_TMP_TBL"), pintIDCompania, 0)
            Return pintIDTesoreria
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TrasladarTesoreria_TMP_TBL")
            Return Nothing
        End Try
    End Function
    'JFSB 20180208 Se agrega el nuevo método para invocar desde el metodo de Guardar en las pantallas de tesoreria
    <Query(HasSideEffects:=True)>
    Public Function TesoreriaActualizar(ByVal Aprobacion As Byte, ByVal pstrTipo As String, ByVal pstrNombreConsecutivo As String, ByVal plngIDDocumento As Integer,
                                        ByVal pstrTipoIdentificacion As String, ByVal plngNroDocumento As String, ByVal pstrNombre As String, ByVal plngIDBanco As Nullable(Of Integer),
                                        ByVal plngNumCheque As Nullable(Of Double), ByVal pcurValor As Nullable(Of Decimal), ByVal pstrDetalle As String, ByVal pdtmDocumento As Nullable(Of DateTime),
                                        ByVal pstrEstado As String, ByVal pdtmEstado As Nullable(Of DateTime), ByVal plngImpresiones As Integer, ByVal pstrFormaPagoCE As String,
                                        ByVal pintNroLote As Integer, ByVal plogContabilidad As Nullable(Of Boolean), ByVal pdtmActualizacion As Nullable(Of DateTime), ByVal pstrUsuario As String,
                                        ByVal pstrParametroContable As String, ByVal plogImpresionFisica As Nullable(Of Boolean), ByVal plogMultiCliente As Nullable(Of Boolean), ByVal pstrCuentaCliente As String,
                                        ByVal plngCodComitente As String, ByVal pstrArchivoTransferencia As String, ByVal plngIdNumInst As Nullable(Of Integer), ByVal pstrDVP As String,
                                        ByVal pstrInstruccion As String, ByVal plngIdNroOrden As Nullable(Of Decimal), ByVal pstrDetalleInstruccion As String, ByVal pstrEstadoNovedadContabilidad As String,
                                        ByVal pNumeroComprobante_Contabilidad As String, ByVal pFechadecontabilizacion_Contabilidad As String, ByVal pFechaProceso_Contabilidad As Nullable(Of DateTime),
                                        ByVal pstrEstadoNovedadContabilidad_Anulada As String, ByVal pstrEstadoAutomatico As String, ByVal pNumeroLote_Contabilidad As String,
                                        ByVal pstrMontoEscrito As String, ByVal pstrTipoIntermedia As String, ByVal pstrConcepto As String, ByVal plogRecaudoDirecto As Nullable(Of Boolean),
                                        ByVal pdtmContabilidadEncuenta As Nullable(Of DateTime), ByVal plogSobregiro As Nullable(Of Boolean), ByVal pstrIdentificacionAutorizadoCheque As String, ByVal pstrNombreAutorizadoCheque As String,
                                        ByVal pintIDTesoreria As Integer, ByVal pdtmContabilidadENC As Nullable(Of DateTime), ByVal pintNroLoteAntENC As Nullable(Of Integer),
                                        ByVal pdtmContabilidadAntENC As Nullable(Of DateTime), ByVal plngIdSucursalBancaria As Nullable(Of Integer), ByVal pdtmCreacion As Nullable(Of DateTime), ByVal pintCobroGMF As Nullable(Of Integer),
                                        ByVal plogTrasladoEntreBancos As Nullable(Of Boolean), ByVal plngBancoConsignacion As Nullable(Of Integer), ByVal plogClienteGMF As Nullable(Of Boolean), ByVal plogBancoGMF As Nullable(Of Boolean),
                                        ByVal pstrTipocheque As String, ByVal pstrTipoCuenta As String, ByVal pstrDetalles As String, ByVal pstrDetalleCheques As String, ByVal plogMostrarMensaje As Nullable(Of Boolean), ByVal pintIDCompania As Nullable(Of Integer)) As List(Of RespuestaDatosTesoreria)
        Try
            Dim strDetalles As String
            Dim strDetalleCheques As String

            strDetalles = HttpUtility.HtmlDecode(pstrDetalles)
            strDetalleCheques = HttpUtility.HtmlDecode(pstrDetalleCheques)

            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_Actualizar_Directo(Aprobacion, pstrTipo, pstrNombreConsecutivo, plngIDDocumento, pstrTipoIdentificacion, plngNroDocumento,
                                                                    pstrNombre, plngIDBanco, plngNumCheque, pcurValor, pstrDetalle, pdtmDocumento, pstrEstado, pdtmEstado, plngImpresiones,
                                                                    pstrFormaPagoCE, pintNroLote, plogContabilidad, pdtmActualizacion, pstrUsuario, pstrParametroContable,
                                                                    plogImpresionFisica, plogMultiCliente, pstrCuentaCliente, plngCodComitente, pstrArchivoTransferencia, plngIdNumInst, pstrDVP,
                                                                    pstrInstruccion, plngIdNroOrden, pstrDetalleInstruccion, pstrEstadoNovedadContabilidad, pNumeroComprobante_Contabilidad, pFechadecontabilizacion_Contabilidad,
                                                                    pFechaProceso_Contabilidad, pstrEstadoNovedadContabilidad_Anulada, pstrEstadoAutomatico, pNumeroLote_Contabilidad, pstrMontoEscrito,
                                                                    pstrTipoIntermedia, pstrConcepto, plogRecaudoDirecto, pdtmContabilidadEncuenta, plogSobregiro, pstrIdentificacionAutorizadoCheque,
                                                                    pstrNombreAutorizadoCheque, pintIDTesoreria, pdtmContabilidadENC, pintNroLoteAntENC, pdtmContabilidadAntENC, plngIdSucursalBancaria,
                                                                    pdtmCreacion, pintCobroGMF, plogTrasladoEntreBancos, plngBancoConsignacion, plogClienteGMF, plogBancoGMF, pstrTipocheque, pstrTipoCuenta, strDetalles,
                                                                    strDetalleCheques, plogMostrarMensaje, pintIDCompania, DemeInfoSesion(pstrUsuario, "Procesos_Consultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TesoreriaActualizar")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function TesoreriaActualizarSync(ByVal Aprobacion As Byte, ByVal pstrTipo As String, ByVal pstrNombreConsecutivo As String, ByVal plngIDDocumento As Integer,
                                        ByVal pstrTipoIdentificacion As String, ByVal plngNroDocumento As String, ByVal pstrNombre As String, ByVal plngIDBanco As Nullable(Of Integer),
                                        ByVal plngNumCheque As Nullable(Of Double), ByVal pcurValor As Nullable(Of Decimal), ByVal pstrDetalle As String, ByVal pdtmDocumento As Nullable(Of DateTime),
                                        ByVal pstrEstado As String, ByVal pdtmEstado As Nullable(Of DateTime), ByVal plngImpresiones As Integer, ByVal pstrFormaPagoCE As String,
                                        ByVal pintNroLote As Integer, ByVal plogContabilidad As Nullable(Of Boolean), ByVal pdtmActualizacion As Nullable(Of DateTime), ByVal pstrUsuario As String,
                                        ByVal pstrParametroContable As String, ByVal plogImpresionFisica As Nullable(Of Boolean), ByVal plogMultiCliente As Nullable(Of Boolean), ByVal pstrCuentaCliente As String,
                                        ByVal plngCodComitente As String, ByVal pstrArchivoTransferencia As String, ByVal plngIdNumInst As Nullable(Of Integer), ByVal pstrDVP As String,
                                        ByVal pstrInstruccion As String, ByVal plngIdNroOrden As Nullable(Of Decimal), ByVal pstrDetalleInstruccion As String, ByVal pstrEstadoNovedadContabilidad As String,
                                        ByVal pNumeroComprobante_Contabilidad As String, ByVal pFechadecontabilizacion_Contabilidad As String, ByVal pFechaProceso_Contabilidad As Nullable(Of DateTime),
                                        ByVal pstrEstadoNovedadContabilidad_Anulada As String, ByVal pstrEstadoAutomatico As String, ByVal pNumeroLote_Contabilidad As String,
                                        ByVal pstrMontoEscrito As String, ByVal pstrTipoIntermedia As String, ByVal pstrConcepto As String, ByVal plogRecaudoDirecto As Nullable(Of Boolean),
                                        ByVal pdtmContabilidadEncuenta As Nullable(Of DateTime), ByVal plogSobregiro As Nullable(Of Boolean), ByVal pstrIdentificacionAutorizadoCheque As String, ByVal pstrNombreAutorizadoCheque As String,
                                        ByVal pintIDTesoreria As Integer, ByVal pdtmContabilidadENC As Nullable(Of DateTime), ByVal pintNroLoteAntENC As Nullable(Of Integer),
                                        ByVal pdtmContabilidadAntENC As Nullable(Of DateTime), ByVal plngIdSucursalBancaria As Nullable(Of Integer), ByVal pdtmCreacion As Nullable(Of DateTime), ByVal pintCobroGMF As Nullable(Of Integer),
                                        ByVal plogTrasladoEntreBancos As Nullable(Of Boolean), ByVal plngBancoConsignacion As Nullable(Of Integer), ByVal plogClienteGMF As Nullable(Of Boolean), ByVal plogBancoGMF As Nullable(Of Boolean),
                                        ByVal pstrTipocheque As String, ByVal pstrTipoCuenta As String, ByVal pstrDetalles As String, ByVal pstrDetalleCheques As String, ByVal plogMostrarMensaje As Nullable(Of Boolean), ByVal pintIDCompania As Nullable(Of Integer)) As List(Of RespuestaDatosTesoreria)

        Dim objTask As Task(Of List(Of RespuestaDatosTesoreria)) = Me.TesoreriaActualizarAsync(Aprobacion, pstrTipo, pstrNombreConsecutivo, plngIDDocumento, pstrTipoIdentificacion, plngNroDocumento,
                                                                    pstrNombre, plngIDBanco, plngNumCheque, pcurValor, pstrDetalle, pdtmDocumento, pstrEstado, pdtmEstado, plngImpresiones,
                                                                    pstrFormaPagoCE, pintNroLote, plogContabilidad, pdtmActualizacion, pstrUsuario, pstrParametroContable,
                                                                    plogImpresionFisica, plogMultiCliente, pstrCuentaCliente, plngCodComitente, pstrArchivoTransferencia, plngIdNumInst, pstrDVP,
                                                                    pstrInstruccion, plngIdNroOrden, pstrDetalleInstruccion, pstrEstadoNovedadContabilidad, pNumeroComprobante_Contabilidad, pFechadecontabilizacion_Contabilidad,
                                                                    pFechaProceso_Contabilidad, pstrEstadoNovedadContabilidad_Anulada, pstrEstadoAutomatico, pNumeroLote_Contabilidad, pstrMontoEscrito,
                                                                    pstrTipoIntermedia, pstrConcepto, plogRecaudoDirecto, pdtmContabilidadEncuenta, plogSobregiro, pstrIdentificacionAutorizadoCheque,
                                                                    pstrNombreAutorizadoCheque, pintIDTesoreria, pdtmContabilidadENC, pintNroLoteAntENC, pdtmContabilidadAntENC, plngIdSucursalBancaria,
                                                                    pdtmCreacion, pintCobroGMF, plogTrasladoEntreBancos, plngBancoConsignacion, plogClienteGMF, plogBancoGMF, pstrTipocheque, pstrTipoCuenta, pstrDetalles,
                                                                    pstrDetalleCheques, plogMostrarMensaje, pintIDCompania)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function TesoreriaActualizarAsync(ByVal Aprobacion As Byte, ByVal pstrTipo As String, ByVal pstrNombreConsecutivo As String, ByVal plngIDDocumento As Integer,
                                        ByVal pstrTipoIdentificacion As String, ByVal plngNroDocumento As String, ByVal pstrNombre As String, ByVal plngIDBanco As Nullable(Of Integer),
                                        ByVal plngNumCheque As Nullable(Of Double), ByVal pcurValor As Nullable(Of Decimal), ByVal pstrDetalle As String, ByVal pdtmDocumento As Nullable(Of DateTime),
                                        ByVal pstrEstado As String, ByVal pdtmEstado As Nullable(Of DateTime), ByVal plngImpresiones As Integer, ByVal pstrFormaPagoCE As String,
                                        ByVal pintNroLote As Integer, ByVal plogContabilidad As Nullable(Of Boolean), ByVal pdtmActualizacion As Nullable(Of DateTime), ByVal pstrUsuario As String,
                                        ByVal pstrParametroContable As String, ByVal plogImpresionFisica As Nullable(Of Boolean), ByVal plogMultiCliente As Nullable(Of Boolean), ByVal pstrCuentaCliente As String,
                                        ByVal plngCodComitente As String, ByVal pstrArchivoTransferencia As String, ByVal plngIdNumInst As Nullable(Of Integer), ByVal pstrDVP As String,
                                        ByVal pstrInstruccion As String, ByVal plngIdNroOrden As Nullable(Of Decimal), ByVal pstrDetalleInstruccion As String, ByVal pstrEstadoNovedadContabilidad As String,
                                        ByVal pNumeroComprobante_Contabilidad As String, ByVal pFechadecontabilizacion_Contabilidad As String, ByVal pFechaProceso_Contabilidad As Nullable(Of DateTime),
                                        ByVal pstrEstadoNovedadContabilidad_Anulada As String, ByVal pstrEstadoAutomatico As String, ByVal pNumeroLote_Contabilidad As String,
                                        ByVal pstrMontoEscrito As String, ByVal pstrTipoIntermedia As String, ByVal pstrConcepto As String, ByVal plogRecaudoDirecto As Nullable(Of Boolean),
                                        ByVal pdtmContabilidadEncuenta As Nullable(Of DateTime), ByVal plogSobregiro As Nullable(Of Boolean), ByVal pstrIdentificacionAutorizadoCheque As String, ByVal pstrNombreAutorizadoCheque As String,
                                        ByVal pintIDTesoreria As Integer, ByVal pdtmContabilidadENC As Nullable(Of DateTime), ByVal pintNroLoteAntENC As Nullable(Of Integer),
                                        ByVal pdtmContabilidadAntENC As Nullable(Of DateTime), ByVal plngIdSucursalBancaria As Nullable(Of Integer), ByVal pdtmCreacion As Nullable(Of DateTime), ByVal pintCobroGMF As Nullable(Of Integer),
                                        ByVal plogTrasladoEntreBancos As Nullable(Of Boolean), ByVal plngBancoConsignacion As Nullable(Of Integer), ByVal plogClienteGMF As Nullable(Of Boolean), ByVal plogBancoGMF As Nullable(Of Boolean),
                                        ByVal pstrTipocheque As String, ByVal pstrTipoCuenta As String, ByVal pstrDetalles As String, ByVal pstrDetalleCheques As String, ByVal plogMostrarMensaje As Nullable(Of Boolean), ByVal pintIDCompania As Nullable(Of Integer)) As Task(Of List(Of RespuestaDatosTesoreria))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of RespuestaDatosTesoreria)) = New TaskCompletionSource(Of List(Of RespuestaDatosTesoreria))()

        objTaskComplete.TrySetResult(TesoreriaActualizar(Aprobacion, pstrTipo, pstrNombreConsecutivo, plngIDDocumento, pstrTipoIdentificacion, plngNroDocumento,
                                                                    pstrNombre, plngIDBanco, plngNumCheque, pcurValor, pstrDetalle, pdtmDocumento, pstrEstado, pdtmEstado, plngImpresiones,
                                                                    pstrFormaPagoCE, pintNroLote, plogContabilidad, pdtmActualizacion, pstrUsuario, pstrParametroContable,
                                                                    plogImpresionFisica, plogMultiCliente, pstrCuentaCliente, plngCodComitente, pstrArchivoTransferencia, plngIdNumInst, pstrDVP,
                                                                    pstrInstruccion, plngIdNroOrden, pstrDetalleInstruccion, pstrEstadoNovedadContabilidad, pNumeroComprobante_Contabilidad, pFechadecontabilizacion_Contabilidad,
                                                                    pFechaProceso_Contabilidad, pstrEstadoNovedadContabilidad_Anulada, pstrEstadoAutomatico, pNumeroLote_Contabilidad, pstrMontoEscrito,
                                                                    pstrTipoIntermedia, pstrConcepto, plogRecaudoDirecto, pdtmContabilidadEncuenta, plogSobregiro, pstrIdentificacionAutorizadoCheque,
                                                                    pstrNombreAutorizadoCheque, pintIDTesoreria, pdtmContabilidadENC, pintNroLoteAntENC, pdtmContabilidadAntENC, plngIdSucursalBancaria,
                                                                    pdtmCreacion, pintCobroGMF, plogTrasladoEntreBancos, plngBancoConsignacion, plogClienteGMF, plogBancoGMF, pstrTipocheque, pstrTipoCuenta, pstrDetalles,
                                                                    pstrDetalleCheques, plogMostrarMensaje, pintIDCompania))
        Return (objTaskComplete.Task)
    End Function

    ''' <summary>
    ''' Función de borrar de los datos de la tabla temporales en caso de error en el traslado 
    ''' </summary>
    ''' <param name="pintIDTesoreria">Identity de Tesoreria</param>
    ''' <param name="pstrUsuario">Usuario</param>
    ''' <returns>SLB20130204</returns>
    ''' <remarks>Retorna el Identity de la tabla TblTesoreria</remarks>
    Public Function BorrarTesoreria_TMP_TBL(ByVal pintIDTesoreria As Integer, ByVal pstrUsuario As String, ByVal pstrUsuarioLlamado As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuarioLlamado, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_Transaccion_TMPTBL_Borrar(pintIDTesoreria, pstrUsuario, DemeInfoSesion(pstrUsuario, "TrasladarTesoreria_TMP_TBL"), 0)
            Return pintIDTesoreria
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BorrarTesoreria_TMP_TBL")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada se mover los datos de las tablas temporales a las tablas de Tesoreria
    ''' </summary>
    ''' <param name="pintIDTesoreria">Identity de Tesoreria</param>
    ''' <param name="pstrUsuario">Usuario</param>
    ''' <returns>SLB20130204</returns>
    ''' <remarks>Retorna el Identity de la tabla TblTesoreria</remarks>
    Public Function AprobarTesoreria(ByVal pintIDTesoreria As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_Aprobar(pintIDTesoreria, pstrUsuario, DemeInfoSesion(pstrUsuario, "AprobarTesoreria"), 0)
            Return pintIDTesoreria
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AprobarTesoreria")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada se mover los datos de las tablas temporales a las tablas de Tesoreria
    ''' </summary>
    ''' <param name="pintIDTesoreria">Identity de Tesoreria</param>
    ''' <param name="pstrUsuario">Usuario</param>
    ''' <returns>SLB20130429</returns>
    ''' <remarks>Retorna el Identity de la tabla TblTesoreria</remarks>
    Public Function RechazarTesoreria(ByVal pintIDTesoreria As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_Rechazar(pintIDTesoreria, pstrUsuario, DemeInfoSesion(pstrUsuario, "RechazarTesoreria"), 0)
            Return pintIDTesoreria
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "RechazarTesoreria")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función para anular un documento de Tesoreria
    ''' </summary>
    ''' <param name="pintIDTesoreria">Identity de Tesoreria</param>
    ''' <param name="pstrCausalInactividad">Causal</param>
    ''' <param name="pstrUsuario">Usuario</param>
    ''' <returns></returns>
    ''' <remarks>SLB20130301</remarks>
    Public Function AnularTesoreria(ByVal pintIDTesoreria As Integer, ByVal pstrCausalInactividad As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_Anulacion(pintIDTesoreria, pstrCausalInactividad, pstrUsuario, DemeInfoSesion(pstrUsuario, "AnularTesoreria"), 0)
            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AnularTesoreria")
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Función para verificar si el un documento de Tesoreria cambio de esta P a I
    ''' </summary>
    ''' <param name="pintIDTesoreria">Identity de Tesoreria</param>
    ''' <param name="pstrUsuario">Usuario</param>
    ''' <returns></returns>
    ''' <remarks>SLB20130312</remarks>
    Public Function Verificar_EstadoImpresion(ByVal pintIDTesoreria As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_VerificarEstadoImpresion(pintIDTesoreria, pstrUsuario, DemeInfoSesion(pstrUsuario, "Verificar_EstadoImpresion"), 0)
            Return CBool(ret)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Verificar_EstadoImpresion")
            Return False
        End Try
    End Function

    Public Function Traer_DetalleTesoreria_Tesoreri(ByVal Filtro As Integer, ByVal pstrModulo As String, ByVal pstrNombreConsecutivo As String, ByVal pIDDocumento As Integer, ByVal pstrEstado As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DetalleTesoreri)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pIDDocumento) Then
                Dim ret = Me.DataContext.uspOyDNet_DetalleTesoreria_Consultar(Filtro, pstrModulo, pstrNombreConsecutivo, pIDDocumento, pstrEstado, DemeInfoSesion(pstrUsuario, "Traer_DetalleTesoreria_Tesoreri"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_DetalleTesoreria_Tesoreri")
            Return Nothing
        End Try
    End Function

    Public Function Traer_Cheques_Tesoreri(ByVal Filtro As Integer, ByVal pIDDocumento As Integer, ByVal pNombreConsecutivo As String, ByVal pEstado As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Cheque)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pIDDocumento) Then
                Dim ret = Me.DataContext.uspOyDNet_ChequesTesoreria_Consultar(Filtro, pIDDocumento, pNombreConsecutivo, pEstado, DemeInfoSesion(pstrUsuario, "Traer_Cheques_Tesoreri"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_DetalleTesoreria_Tesoreri")
            Return Nothing
        End Try
    End Function

    Public Function Traer_Observaciones_Tesoreri(ByVal Filtro As Integer, ByVal pIDDocumento As Integer, ByVal pNombreConsecutivo As String, ByVal pTipo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TesoreriaAdicionale)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not IsNothing(pIDDocumento) Then
                Dim ret = Me.DataContext.uspOyDNet_ObservacionesTesoreria_Consultar(Filtro, pIDDocumento, pNombreConsecutivo, pTipo, DemeInfoSesion(pstrUsuario, "Traer_Observaciones_Tesoreri"), 0).ToList
                Return ret
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_DetalleTesoreria_Tesoreri")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Operación encargada de consultar variables para validar que el cliente tenga saldo disponible en comprobantes de egreso.
    ''' </summary>
    ''' <returns>List(Of TraerBolsa)</returns>
    ''' <remarks>
    ''' Creado por       : Juan Carlos Soto Cruz.    
    ''' Fecha            : Octubre 21/2011
    ''' </remarks>
    Public Function TraerBolsa(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TraerBolsa)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_TraerBolsa(DemeInfoSesion(pstrUsuario, "BuscarTipoDocumento"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerBolsa")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' 'Operacion encargada de consultar el saldo de un cliente a la fecha dada
    ''' </summary>
    ''' <param name="pLngIdComisionista">Codigo identificador de la Comisionista.</param>
    ''' <param name="pLngIdSucComisionista">Codigo identificador de la Sucursal de la Comisionista.</param>
    ''' <param name="plngIDComitente">Codigo identificador del cliente.</param>
    ''' <param name="pdtmDocumento">Fecha.</param>
    ''' <returns>Double - Saldo del cliente a la fecha.</returns>
    ''' <remarks></remarks>
    Public Function ValidarSaldoClientesTesoreria(ByVal pLngIdComisionista As Integer, ByVal pLngIdSucComisionista As Integer, ByVal plngIDComitente As String,
                                                  ByVal pdtmDocumento As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Double
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pDblSAldo As System.Nullable(Of Double) = 0
            Dim ret = Me.DataContext.uspValidarSaldoClientesTesoreria(pLngIdComisionista, pLngIdSucComisionista, plngIDComitente, pdtmDocumento, pDblSAldo)
            Return CDbl(pDblSAldo)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarSaldoClientesTesoreria")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' 'Operacion encargada de consultar el saldo de un cliente a la fecha dada
    ''' </summary>
    ''' <param name="pLngIdComisionista">Codigo identificador de la Comisionista.</param>
    ''' <param name="pLngIdSucComisionista">Codigo identificador de la Sucursal de la Comisionista.</param>
    ''' <param name="plngIDComitente">Codigo identificador del cliente.</param>
    ''' <param name="pdtmDocumento">Fecha.</param>
    ''' <returns>Double - Saldo del cliente a la fecha.</returns>
    ''' <remarks></remarks>
    Public Function ValidarSaldoClientesTesoreria_Generico(ByVal pLngIdComisionista As Integer, ByVal pLngIdSucComisionista As Integer, ByVal plngIDComitente As String,
                                                  ByVal pdtmDocumento As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Double
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pDblSAldo As System.Nullable(Of Double) = 0
            Dim ret = Me.DataContext.uspValidarSaldoClientesTesoreria_OyDNet(pLngIdComisionista, pLngIdSucComisionista, plngIDComitente, pdtmDocumento).ToList
            pDblSAldo = ret.Item(0).SaldoCliente
            Return CDbl(pDblSAldo)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarSaldoClientesTesoreria")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function ValidarSaldoClientesTesoreria_Masivo_Generico(ByVal pLngIdComisionista As Integer, ByVal pLngIdSucComisionista As Integer, ByVal plngIDComitente As String, ByVal pstrNombreConsecutivo As String,
                                                  ByVal pdtmDocumento As Nullable(Of DateTime), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of SaldosClientesMasivo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not String.IsNullOrEmpty(plngIDComitente) Then
                plngIDComitente = HttpUtility.HtmlDecode(plngIDComitente)
            End If
            Dim ret = Me.DataContext.uspValidarSaldoClientesTesoreriaMasivo_OyDNet(pLngIdComisionista, pLngIdSucComisionista, plngIDComitente, pstrNombreConsecutivo, pdtmDocumento, pstrUsuario).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarSaldoClientesTesoreria_Masivo_Generico")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function GeneracionAutomaticaNotasConsultar(ByVal pstrOperaciones As String, ByVal pstrClase As String, ByVal pstrNaturaleza As String, ByVal pstrTipoOperacion As String,
                                                       ByVal pdtmFecha As Nullable(Of DateTime),
                                                       ByVal pstrFechaDe As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ResultadoGeneAutoNotas)
        Try

            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspGeneracionAutomaticaNotasConsultar_OyDNet(pstrOperaciones, pstrClase, pstrNaturaleza, pstrTipoOperacion, pdtmFecha, pstrFechaDe, pstrUsuario, DemeInfoSesion(pstrUsuario, "GeneracionAutomaticaNotasConsultar"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GeneracionAutomaticaNotasConsultar")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function ResultadoGeneracionAutomaticaNotas(ByVal pxml_strIdRegistros As String,
                                                       ByVal pstrNombreConsecutivo As String, ByVal pdtmFechaDocumento As Nullable(Of DateTime),
                                                       ByVal pstrRegistroCliente As String, ByVal pstrContraParteCliente As String, ByVal pstrRegistroBanco As String,
                                                       ByVal pstrContraParteBanco As String, ByVal pstrRegistroCuentaContable As String, ByVal pstrContraparteCuentaContable As String,
                                                       ByVal pstrRegistroCCostos As String, ByVal pstrContraparteCCostos As String,
                                                       ByVal plogRegistroNinguno As Boolean, ByVal plogContraParteNinguno As Boolean, ByVal plogRegistroBanco As Boolean,
                                                       ByVal plogContraparteBanco As Boolean, ByVal plogRegistroCliente As Boolean, ByVal plogContraParteCliente As Boolean,
                                                       ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RespuestaProcesosGenericosConfirmacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not String.IsNullOrEmpty(pxml_strIdRegistros) Then
                pxml_strIdRegistros = HttpUtility.HtmlDecode(pxml_strIdRegistros)
            End If
            Dim ret = Me.DataContext.uspGeneracionAutomaticaNotasGenerar_OyDNet(pxml_strIdRegistros, pstrNombreConsecutivo,
                                                                                pdtmFechaDocumento, pstrRegistroCliente, pstrContraParteCliente, pstrRegistroBanco, pstrContraParteBanco,
                                                                                pstrRegistroCuentaContable, pstrContraparteCuentaContable, pstrRegistroCCostos, pstrContraparteCCostos,
                                                                                plogRegistroNinguno, plogContraParteNinguno, plogRegistroBanco, plogContraparteBanco,
                                                                                plogRegistroCliente, plogContraParteCliente, pstrUsuario, DemeInfoSesion(pstrUsuario, "GeneracionAutomaticaNotasConsultar"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ResultadoGeneracionAutomaticaNotas")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Valida si el CE tiene una Nota GMF Relacionado
    ''' </summary>
    ''' <param name="pstrNombreConsecutivo">Nombre del Consecutivo del CE</param>
    ''' <param name="plngIDDocumento">Numero del CE</param>
    ''' <param name="pbitNotaGMF"></param>
    ''' <returns>Retorna una Lista de Tipo ValidarDocumentoGMF</returns>
    ''' <remarks>SLB20130204</remarks>
    Public Function ValidarRelacion_GMF(ByVal pstrNombreConsecutivo As String, ByVal plngIDDocumento As Integer, ByVal pbitNotaGMF As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ValidarDocumentoGMF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_ValidarDocumentoGMF_OyDNet(pstrNombreConsecutivo, plngIDDocumento, pbitNotaGMF, DemeInfoSesion(pstrUsuario, "ValidarRelacion_GMF"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarRelacionGMF")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Valida si el RC tiene un CE Relacionado
    ''' </summary>
    ''' <param name="pstrNombreConsecutivo">Nombre del Consecutivo del RC</param>
    ''' <param name="plngIDDocumento">Numero del RC</param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>Retorna una Lista de Tipo ValidarDocumentoGMF</returns>
    ''' <remarks>SLB20130204</remarks>
    Public Function ValidarComprobanteAsociado(ByVal pstrNombreConsecutivo As String, ByVal plngIDDocumento As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ValidarDocumentoGMF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_RecibodeCajaConsultarComprobanteAsociado_OyDNet(pstrNombreConsecutivo, plngIDDocumento, pstrUsuario, DemeInfoSesion(pstrUsuario, "ValidarComprobanteAsociado"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarComprobanteAsociado")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encarga de Verificar si la Moneda del Consecutivo coincide con la Moneda del Banco
    ''' </summary>
    ''' <param name="pintIDBanco">Código del Banco</param>
    ''' <param name="pstrNombreConsecutivo">Nombre del Consecutivo</param>
    ''' <returns>True(Coinciden) - False(No Coinciden)</returns>
    ''' <remarks>SLB20130202</remarks>
    <Query(HasSideEffects:=True)>
    Public Function MonedaConsecutivo_Corresponde_MonedaBanco(ByVal pintIDBanco As Nullable(Of Integer), ByVal pstrNombreConsecutivo As String, ByVal plogValidarInformacion As Boolean, ByVal pstrRegistrosXML As String, ByVal pstrTipo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DatosMonedaBancoConsecutivo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not String.IsNullOrEmpty(pstrRegistrosXML) Then
                pstrRegistrosXML = HttpUtility.HtmlDecode(pstrRegistrosXML)
            End If
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_CoincideMoneda_Banco(pintIDBanco, pstrNombreConsecutivo, DemeInfoSesion(pstrUsuario, "MonedaConsecutivo_Corresponde_MonedaBanco"), 0, plogValidarInformacion, pstrRegistrosXML, pstrTipo).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "MonedaConsecutivo_Corresponde_MonedaBanco")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Valida si el usuario tiene permiso para duplicar un documento en Tesorería
    ''' </summary>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    ''' <remarks>SLB20130321</remarks>
    Public Function ValidarUsuario_DuplicarTesoreria(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strUsuarioConsecutivo As String = String.Empty

            Me.DataContext.uspOyDNet_Tesoreria_ValidarDuplicidadUsuario(pstrUsuario, DemeInfoSesion(pstrUsuario, "ValidarUsuario_DuplicarTesoreria"), 0, strUsuarioConsecutivo)

            Return strUsuarioConsecutivo
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidarUsuario_DuplicarTesoreria")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Valida si el documento de Tesoreria fue modificado antes de poder editarlo.
    ''' </summary>
    ''' <param name="strTipo"></param>
    ''' <param name="strNombreConsecutivo"></param>
    ''' <param name="lngIDDocumento"></param>
    ''' <param name="strUsuario"></param>
    ''' <returns>Retorna la lista TesoreriaModificable</returns>
    ''' <remarks>SLB20130801</remarks>
    Public Function Validar_EstadoDocumentoTesoreria(ByVal strTipo As String, ByVal strNombreConsecutivo As String, ByVal lngIDDocumento As Integer,
                                                     ByVal strUsuario As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TesoreriaModificable)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_VerificarEstado_Consultar(strTipo, strNombreConsecutivo, lngIDDocumento, strUsuario, DemeInfoSesion(pstrUsuario, "Validar_EstadoDocumentoTesoreria"), ClsConstantes.GINT_ErrorPersonalizado).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Validar_EstadoDocumentoTesoreria")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consulta las cuentas clientes del comitente seleccionado.
    ''' </summary>
    ''' <param name="plngIDComitente"></param>
    ''' <param name="pstrCuenta"></param>
    ''' <returns></returns>
    ''' <remarks>SLB20130611</remarks>
    Public Function CuentasClientesTesoreria_Consultar(ByVal plngIDComitente As String, ByVal pstrCuenta As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CuentasClientes_Tesoreria)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_ConsultarCuentasClientesBeneficiarios_OyDNet(plngIDComitente, pstrCuenta, DemeInfoSesion(pstrUsuario, "CuentasClientesTesoreria_Consultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CuentasClientesTesoreria_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Consulta la configuración de los consecutivos de Tesorería.
    ''' </summary>
    ''' <param name="pstrModuloTesoreria"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    ''' <remarks>SLB20140620</remarks>
    Public Function ConfiguracionConsecutivos_Consultar(ByVal pstrModuloTesoreria As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConfiguracionConsecutivos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_ConfiguracionConsecutivos(pstrModuloTesoreria, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConfiguracionConsecutivos_Consultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConfiguracionConsecutivos_Consultar")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Operacion encargada de buscar el retorno o la descripción de un tópico y una descripción o retorno dados respectivamente.
    ''' </summary>
    ''' <param name="pstrTopico">Topico</param>
    ''' <param name="pbolCodigo">TRUE - Indica consultar la descripcion, FALSE - Indica consultar el retorno.</param>
    ''' <param name="pstrValor">Valor del retorno o de la descripcion.</param>
    ''' <returns>String</returns>
    ''' <remarks>
    ''' Creado por       : Juan Carlos Soto Cruz.    
    ''' Fecha            : Octubre 24/2011
    ''' </remarks>
    Public Function BuscarTipoDocumento(ByVal pstrTopico As String, ByVal pbolCodigo As Boolean, ByVal pstrValor As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrTipoDocumento As String = String.Empty
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_BuscarTipoDocumento(pstrTopico, pbolCodigo, pstrValor, DemeInfoSesion(pstrUsuario, "BuscarTipoDocumento"), 0, pstrTipoDocumento)
            Return pstrTipoDocumento
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BuscarTipoDocumento")
            Return Nothing
        End Try

    End Function


    ''' <summary>
    ''' Operación que verifica si una transferencia de un cliente cobra o no GMF.
    ''' </summary>
    ''' <param name="plngIDComitente">Codigo identificador del cliente.</param>
    ''' <returns>Verdadero si una transferencia de un cliente cobra GMF o falso en caso contrario.</returns>
    ''' <remarks>
    ''' Creado por       : Jairo Andres Gonzalez Franco    
    ''' Fecha            : 20141021
    ''' </remarks>
    Public Function Validar_GMF_Transferencias(ByVal pstrFormaPago As String, ByVal plngIDComitente As String, ByVal pstrCuentaCliente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pbitCobroGMF? As Boolean = False
            Dim ret = Me.DataContext.uspOyDNet_Validar_GMF_Transferencias(pstrFormaPago, plngIDComitente, pstrCuentaCliente, pbitCobroGMF, pstrUsuario, DemeInfoSesion(pstrUsuario, "Validar_GMF_Transferencias"), 0)
            Return CBool(pbitCobroGMF.Value)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Validar_GMF_Transferencias")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Operación que verifica que el cliente especificado tenga una cuenta con el indicativo “logTransferirA”
    ''' </summary>
    ''' <param name="plngIDComitente">Codigo identificador del cliente.</param>
    ''' <returns>Verdadero si existe una cuenta de transferencia o falso en caso contrario.</returns>
    ''' <remarks>
    ''' Creado por       : Juan Carlos Soto Cruz.    
    ''' Fecha            : Octubre 25/2011
    ''' </remarks>
    Public Function ConsutarCuentaTransferencia(ByVal plngIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pbolTransferenciaDisponible As Boolean = False
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_ConsutarCuentaTransferencia(plngIDComitente, DemeInfoSesion(pstrUsuario, "BuscarTipoDocumento"), 0, pbolTransferenciaDisponible)
            Return pbolTransferenciaDisponible
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsutarCuentaTransferencia")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Operación encargada de consultar el consecutivo de un banco en la tabla tblConsecutivos
    ''' </summary>
    ''' <param name="lngIdBanco">Codigo del banco.</param>
    ''' <returns>List(Of ConsecutivoBanco)</returns>
    ''' <remarks>
    ''' Creado por       : Juan Carlos Soto Cruz.    
    ''' Fecha            : Octubre 26/2011
    ''' </remarks>
    Public Function ConsutarConsecutivoBanco(ByVal lngIdBanco As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ConsecutivoBanco)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_ConsutarConsecutivoBanco(lngIdBanco, DemeInfoSesion(pstrUsuario, "ConsutarConsecutivoBanco"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsutarConsecutivoBanco")
            Return Nothing
        End Try
    End Function

    Public Function VerificarGMF(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Instalacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_VerificarGMF_OyDNet().ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "VerificarGMF")
            Return Nothing
        End Try
    End Function

    Public Function PoseeCobrogravamen(ByVal idbanco As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of Bancogravamen)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_PoseeCobrogravamen_OyDNet(idbanco).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PoseeCobrogravamen")
            Return Nothing
        End Try
    End Function

    Public Function Verifica4pormil(ByVal parametro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim retorno As String = String.Empty
            Dim ret = Me.DataContext.usp_Valida4porlmil_OyDNet(parametro, retorno)
            Return retorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Verifica4pormil")
            Return Nothing
        End Try
    End Function
    ''' JFSB 20180102 Se agrega el parámetro de la compañía
    <Query(HasSideEffects:=True)>
    Public Function ValidaDatosencuenta(ByVal strCuentaContable As String, ByVal strNit As String, ByVal strCentroCosto As String, ByVal lngResultado As Integer,
                                        ByVal lngOpcion As Integer, ByVal pstrRegistrosXML As String, ByVal plogValidarInformacion As Boolean, ByVal plngIdCompania As Integer, ByVal pstrUsuario As String,
                                        ByVal pstrInfoConexion As String) As List(Of DatosEnCuenta)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If Not String.IsNullOrEmpty(pstrRegistrosXML) Then
                pstrRegistrosXML = HttpUtility.HtmlDecode(pstrRegistrosXML)
            End If

            Dim ret = Me.DataContext.spValidarDatosEnCuenta_OyDNet(strCuentaContable, strNit, strCentroCosto, lngResultado, lngOpcion, pstrRegistrosXML, plngIdCompania, plogValidarInformacion).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ValidaDatosencuenta")
            Return Nothing
        End Try
    End Function
    ''' JFSB 20180102 Se agrega el parámetro de la compañía
    Public Function Tesoreriacuentabancoencuenta(ByVal idBanco As Integer, ByVal plngIdCompania As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of CuentaBancoEnCuenta)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_Tesoreria_CE_CuentaBancoEnCuenta_VALIDAR_OyDNet(idBanco, plngIdCompania).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Tesoreriacuentabancoencuenta")
            Return Nothing
        End Try
    End Function

    'JFSB 20171210 Se agrega funciones para el proceso de reasignación de cuenta por medio de las fuentes
    <Query(HasSideEffects:=True)>
    Public Function Tesoreria_ReasignarCuentaContable(ByVal pxmlDatosCuentas As String,
                                                      ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RetornoReasignarCuentas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strxmlDetTesoreria = HttpUtility.HtmlDecode(pxmlDatosCuentas)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_Reasignar_CuentaContable(strxmlDetTesoreria,
                                                                                  pstrUsuario,
                                                                                  DemeInfoSesion(pstrUsuario, "Tesoreria_ReasignarCuentaContable"),
                                                                                  0).ToList
            Return ret

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Tesoreria_ReasignarCuentaContable")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function Tesoreria_ReasignarCuentaContableSync(ByVal pxmlDatosCuentas As String,
                                                      ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RetornoReasignarCuentas)
        Dim objTask As Task(Of List(Of RetornoReasignarCuentas)) = Me.Tesoreria_ReasignarCuentaContableSyncAsync(pxmlDatosCuentas, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function Tesoreria_ReasignarCuentaContableSyncAsync(ByVal pxmlDatosCuentas As String,
                                                      ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of RetornoReasignarCuentas))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of RetornoReasignarCuentas)) = New TaskCompletionSource(Of List(Of RetornoReasignarCuentas))()
        objTaskComplete.TrySetResult(Tesoreria_ReasignarCuentaContable(pxmlDatosCuentas, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    <Query(HasSideEffects:=True)>
    Public Function Tesoreria_ConsultarNotaGFM(ByVal plngIDDocumento As Integer, ByVal pstrNombreConsecutivo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RespuetaConsultarNotaGMF)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ConsultarNotaGMF(plngIDDocumento, pstrNombreConsecutivo, pstrUsuario, DemeInfoSesion(pstrUsuario, "Tesoreria_ConsultarNotaGFM"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Tesoreria_ConsultarNotaGFM")
            Return Nothing
        End Try

    End Function

    Public Function AprobarCompEgreso(ByVal Consecutivo As String, ByVal documento As Integer, ByVal accion As String, ByVal logsobregiro As Boolean, ByVal documentonota As Integer, ByVal actualizadetalle As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspAprobarCompEgreso_OyDNet(Consecutivo, documento, accion, logsobregiro, documentonota, actualizadetalle)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AprobarCompEgreso")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function Tesoreria_Validaciones(ByVal pstrTipo As String,
                                            ByVal pstrNombreConsecutivo As String,
                                            ByVal plngIDDocumento As Nullable(Of System.Int32),
                                            ByVal pstrTipoIdentificacion As String,
                                            ByVal plngNroDocumento As String,
                                            ByVal pstrNombre As String,
                                            ByVal plngIDBanco As Nullable(Of System.Int32),
                                            ByVal pxmlDetalleTesoreria As String,
                                            ByVal pxmlDetalleCheques As String,
                                            ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidacionesTesoreria)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)



            Dim strxmlDetTesoreria = HttpUtility.HtmlDecode(pxmlDetalleTesoreria)
            Dim strxmlDetCheques = HttpUtility.HtmlDecode(pxmlDetalleCheques)

            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_Validaciones(pstrTipo,
                                             pstrNombreConsecutivo,
                                             plngIDDocumento,
                                             pstrTipoIdentificacion,
                                             plngNroDocumento,
                                             pstrNombre,
                                             plngIDBanco,
                                             strxmlDetTesoreria,
                                             strxmlDetCheques,
                                             pstrUsuario,
                                            DemeInfoSesion(pstrUsuario, "Tesoreria_Validaciones"),
                                            0).ToList
            Return ret

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Tesoreria_Validaciones")
            Return Nothing
        End Try
    End Function

    Public Function Tesoreria_ValidarEstadoDocumento(ByVal pstrAccion As String,
                                                     ByVal plngIDTesoreria As Nullable(Of System.Int32),
                                                     ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RespuestaValidacionEstadoDocumento)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_ValidarEstadoDocumento(pstrAccion,
                                                                                plngIDTesoreria,
                                                                                pstrUsuario,
                                                                                DemeInfoSesion(pstrUsuario, "Tesoreria_ValidarEstadoDocumento"),
                                                                                0).ToList
            Return ret

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Tesoreria_ValidarEstadoDocumento")
            Return Nothing
        End Try
    End Function

    Public Function Tesoreria_ValidarEstadoDocumentoSync(ByVal pstrAccion As String,
                                                     ByVal plngIDTesoreria As Nullable(Of System.Int32),
                                                     ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RespuestaValidacionEstadoDocumento)
        Dim objTask As Task(Of List(Of RespuestaValidacionEstadoDocumento)) = Me.Tesoreria_ValidarEstadoDocumentoSyncAsync(pstrAccion, plngIDTesoreria, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function Tesoreria_ValidarEstadoDocumentoSyncAsync(ByVal pstrAccion As String,
                                                     ByVal plngIDTesoreria As Nullable(Of System.Int32),
                                                     ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of RespuestaValidacionEstadoDocumento))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of RespuestaValidacionEstadoDocumento)) = New TaskCompletionSource(Of List(Of RespuestaValidacionEstadoDocumento))()
        objTaskComplete.TrySetResult(Tesoreria_ValidarEstadoDocumento(pstrAccion, plngIDTesoreria, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Tesoreria_GenerarNotaContableFondos(ByVal pintIDCompania As Integer,
                                                        ByVal pstrNombreConsecutivo As String,
                                                        ByVal pintIDConcepto As Integer,
                                                        ByVal pstrDetalle As String,
                                                        ByVal pstrTipoMovimiento As String,
                                                        ByVal pstrRetencion As String,
                                                        ByVal pstrCuentaContableCR As String,
                                                        ByVal pstrCuentaContableDB As String,
                                                        ByVal pintIDBancoCR As Nullable(Of Integer),
                                                        ByVal pintIDBancoDB As Nullable(Of Integer),
                                                        ByVal pstrNit As String,
                                                        ByVal pstrCentroCostos As String,
                                                        ByVal pdtmFechaInicial As Nullable(Of DateTime),
                                                        ByVal pdtmFechaFinal As Nullable(Of DateTime),
                                                        ByVal plogManejaDiferido As Boolean,
                                                        ByVal pstrTipoPagoDiferido As String,
                                                        ByVal pstrTipoMovimientoDiferido As String,
                                                        ByVal pstrCuentaContableCRDiferido As String,
                                                        ByVal pstrCuentaContableDBDiferido As String,
                                                        ByVal pcurValor As Decimal,
                                                        ByVal pstrCodigoOYD As String,
                                                        ByVal pintIDEncargo As Nullable(Of Integer),
                                                        ByVal pdtmFechaRetiro As Nullable(Of DateTime),
                                                        ByVal pintIDDetalleRetiro As Nullable(Of Integer),
                                                        ByVal plogManejaNotaDBCR As Nullable(Of Boolean),
                                                        ByVal pstrTipoMovimientoCRDB As String,
                                                        ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RespuestaValidacionEstadoDocumento)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_GenerarNotaContableFondos(pintIDCompania,
                                                                                   pstrNombreConsecutivo,
                                                                                   pintIDConcepto,
                                                                                   pstrDetalle,
                                                                                   pstrTipoMovimiento,
                                                                                   pstrRetencion,
                                                                                   pstrCuentaContableCR,
                                                                                   pstrCuentaContableDB,
                                                                                   pintIDBancoCR,
                                                                                   pintIDBancoDB,
                                                                                   pstrNit,
                                                                                   pstrCentroCostos,
                                                                                   pdtmFechaInicial,
                                                                                   pdtmFechaFinal,
                                                                                   plogManejaDiferido,
                                                                                   pstrTipoPagoDiferido,
                                                                                   pstrTipoMovimientoDiferido,
                                                                                   pstrCuentaContableCRDiferido,
                                                                                   pstrCuentaContableDBDiferido,
                                                                                   pcurValor,
                                                                                   pstrCodigoOYD,
                                                                                   pintIDEncargo,
                                                                                   pdtmFechaRetiro,
                                                                                   pintIDDetalleRetiro,
                                                                                   plogManejaNotaDBCR,
                                                                                   pstrTipoMovimientoCRDB,
                                                                                   pstrUsuario,
                                                                                   DemeInfoSesion(pstrUsuario, "Tesoreria_GenerarNotaContableFondos"),
                                                                                   0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Tesoreria_GenerarNotaContableFondos")
            Return Nothing
        End Try
    End Function

    Public Function Tesoreria_GenerarNotaContableFondosSync(ByVal pintIDCompania As Integer,
                                                        ByVal pstrNombreConsecutivo As String,
                                                        ByVal pintIDConcepto As Integer,
                                                        ByVal pstrDetalle As String,
                                                        ByVal pstrTipoMovimiento As String,
                                                        ByVal pstrRetencion As String,
                                                        ByVal pstrCuentaContableCR As String,
                                                        ByVal pstrCuentaContableDB As String,
                                                        ByVal pintIDBancoCR As Nullable(Of Integer),
                                                        ByVal pintIDBancoDB As Nullable(Of Integer),
                                                        ByVal pstrNit As String,
                                                        ByVal pstrCentroCostos As String,
                                                        ByVal pdtmFechaInicial As Nullable(Of DateTime),
                                                        ByVal pdtmFechaFinal As Nullable(Of DateTime),
                                                        ByVal plogManejaDiferido As Boolean,
                                                        ByVal pstrTipoPagoDiferido As String,
                                                        ByVal pstrTipoMovimientoDiferido As String,
                                                        ByVal pstrCuentaContableCRDiferido As String,
                                                        ByVal pstrCuentaContableDBDiferido As String,
                                                        ByVal pcurValor As Decimal,
                                                        ByVal pstrCodigoOYD As String,
                                                        ByVal pintIDEncargo As Nullable(Of Integer),
                                                        ByVal pdtmFechaRetiro As Nullable(Of DateTime),
                                                        ByVal pintIDDetalleRetiro As Nullable(Of Integer),
                                                        ByVal plogManejaNotaDBCR As Nullable(Of Boolean),
                                                        ByVal pstrTipoMovimientoCRDB As String,
                                                        ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RespuestaValidacionEstadoDocumento)
        Dim objTask As Task(Of List(Of RespuestaValidacionEstadoDocumento)) = Me.Tesoreria_GenerarNotaContableFondosSyncAsync(pintIDCompania,
                                                                                   pstrNombreConsecutivo,
                                                                                   pintIDConcepto,
                                                                                   pstrDetalle,
                                                                                   pstrTipoMovimiento,
                                                                                   pstrRetencion,
                                                                                   pstrCuentaContableCR,
                                                                                   pstrCuentaContableDB,
                                                                                   pintIDBancoCR,
                                                                                   pintIDBancoDB,
                                                                                   pstrNit,
                                                                                   pstrCentroCostos,
                                                                                   pdtmFechaInicial,
                                                                                   pdtmFechaFinal,
                                                                                   plogManejaDiferido,
                                                                                   pstrTipoPagoDiferido,
                                                                                   pstrTipoMovimientoDiferido,
                                                                                   pstrCuentaContableCRDiferido,
                                                                                   pstrCuentaContableDBDiferido,
                                                                                   pcurValor,
                                                                                   pstrCodigoOYD,
                                                                                   pintIDEncargo,
                                                                                   pdtmFechaRetiro,
                                                                                   pintIDDetalleRetiro,
                                                                                   plogManejaNotaDBCR,
                                                                                   pstrTipoMovimientoCRDB,
                                                                                   pstrUsuario,
                                                                                   pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function


    Private Function Tesoreria_GenerarNotaContableFondosSyncAsync(ByVal pintIDCompania As Integer,
                                                        ByVal pstrNombreConsecutivo As String,
                                                        ByVal pintIDConcepto As Integer,
                                                        ByVal pstrDetalle As String,
                                                        ByVal pstrTipoMovimiento As String,
                                                        ByVal pstrRetencion As String,
                                                        ByVal pstrCuentaContableCR As String,
                                                        ByVal pstrCuentaContableDB As String,
                                                        ByVal pintIDBancoCR As Nullable(Of Integer),
                                                        ByVal pintIDBancoDB As Nullable(Of Integer),
                                                        ByVal pstrNit As String,
                                                        ByVal pstrCentroCostos As String,
                                                        ByVal pdtmFechaInicial As Nullable(Of DateTime),
                                                        ByVal pdtmFechaFinal As Nullable(Of DateTime),
                                                        ByVal plogManejaDiferido As Boolean,
                                                        ByVal pstrTipoPagoDiferido As String,
                                                        ByVal pstrTipoMovimientoDiferido As String,
                                                        ByVal pstrCuentaContableCRDiferido As String,
                                                        ByVal pstrCuentaContableDBDiferido As String,
                                                        ByVal pcurValor As Decimal,
                                                        ByVal pstrCodigoOYD As String,
                                                        ByVal pintIDEncargo As Nullable(Of Integer),
                                                        ByVal pdtmFechaRetiro As Nullable(Of DateTime),
                                                        ByVal pintIDDetalleRetiro As Nullable(Of Integer),
                                                        ByVal plogManejaNotaDBCR As Nullable(Of Boolean),
                                                        ByVal pstrTipoMovimientoCRDB As String,
                                                        ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of RespuestaValidacionEstadoDocumento))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of RespuestaValidacionEstadoDocumento)) = New TaskCompletionSource(Of List(Of RespuestaValidacionEstadoDocumento))()
        objTaskComplete.TrySetResult(Tesoreria_GenerarNotaContableFondos(pintIDCompania,
                                                                                   pstrNombreConsecutivo,
                                                                                   pintIDConcepto,
                                                                                   pstrDetalle,
                                                                                   pstrTipoMovimiento,
                                                                                   pstrRetencion,
                                                                                   pstrCuentaContableCR,
                                                                                   pstrCuentaContableDB,
                                                                                   pintIDBancoCR,
                                                                                   pintIDBancoDB,
                                                                                   pstrNit,
                                                                                   pstrCentroCostos,
                                                                                   pdtmFechaInicial,
                                                                                   pdtmFechaFinal,
                                                                                   plogManejaDiferido,
                                                                                   pstrTipoPagoDiferido,
                                                                                   pstrTipoMovimientoDiferido,
                                                                                   pstrCuentaContableCRDiferido,
                                                                                   pstrCuentaContableDBDiferido,
                                                                                   pcurValor,
                                                                                   pstrCodigoOYD,
                                                                                   pintIDEncargo,
                                                                                   pdtmFechaRetiro,
                                                                                   pintIDDetalleRetiro,
                                                                                   plogManejaNotaDBCR,
                                                                                   pstrTipoMovimientoCRDB,
                                                                                   pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function


    Public Function consultarCompaniaTesoreriaValidarAnular(ByVal pintIDCompania As Integer, ByVal pdtmFecha As DateTime, ByVal pstrNombreConsecutivo As String, ByVal plngIDdocumento As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim strResultado As String = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.uspOyDNet_Companias_TesoreriaValidarAnular(pintIDCompania, pdtmFecha, pstrNombreConsecutivo, plngIDdocumento, strResultado, pstrUsuario, DemeInfoSesion(pstrUsuario, "consultarCompaniaTesoreriaValidarAnular"), 0)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "consultarCompaniaTesoreriaValidarAnular")
        End Try
        Return strResultado
    End Function

    Public Function consultarCompaniaTesoreriaValidarAnularSync(ByVal pintIDCompania As Integer, ByVal pdtmFecha As DateTime, ByVal pstrNombreConsecutivo As String, ByVal plngIDdocumento As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Dim objTask As Task(Of String) = Me.consultarCompaniaTesoreriaValidarAnularAsync(pintIDCompania, pdtmFecha, pstrNombreConsecutivo, plngIDdocumento, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function consultarCompaniaTesoreriaValidarAnularAsync(ByVal pintIDCompania As Integer, ByVal pdtmFecha As DateTime, ByVal pstrNombreConsecutivo As String, ByVal plngIDdocumento As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of String)
        Dim objTaskComplete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        objTaskComplete.TrySetResult(consultarCompaniaTesoreriaValidarAnular(pintIDCompania, pdtmFecha, pstrNombreConsecutivo, plngIDdocumento, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Sub InsertGenerarNotasFondosCancelacion(ByVal currentGenerarNotasFondosCancelacion As GenerarNotasFondosCancelacion)

    End Sub

    Public Sub UpdateGenerarNotasFondosCancelacion(ByVal currentGenerarNotasFondosCancelacion As GenerarNotasFondosCancelacion)

    End Sub

    Public Function Tesoreria_GenerarNotaContableFondos_ConsultarPendientes(ByVal pstrTipoConsulta As String, ByVal pintIDCompania As Integer, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GenerarNotasFondosCancelacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_GenerarNotaContableFondos_ConsultarPendientes(pstrTipoConsulta, pintIDCompania, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "Tesoreria_GenerarNotaContableFondos_ConsultarPendientes"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Tesoreria_GenerarNotaContableFondos_ConsultarPendientes")
            Return Nothing
        End Try
    End Function

    Public Function Tesoreria_GenerarNotaContableFondos_ConsultarPendientesSync(ByVal pstrTipoConsulta As String, ByVal pintIDCompania As Integer, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GenerarNotasFondosCancelacion)
        Dim objTask As Task(Of List(Of GenerarNotasFondosCancelacion)) = Me.Tesoreria_GenerarNotaContableFondosSyncAsync(pstrTipoConsulta, pintIDCompania, pstrUsuario, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function Tesoreria_GenerarNotaContableFondosSyncAsync(ByVal pstrTipoConsulta As String, ByVal pintIDCompania As Integer, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of GenerarNotasFondosCancelacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of GenerarNotasFondosCancelacion)) = New TaskCompletionSource(Of List(Of GenerarNotasFondosCancelacion))()
        objTaskComplete.TrySetResult(Tesoreria_GenerarNotaContableFondos_ConsultarPendientes(pstrTipoConsulta, pintIDCompania, pstrUsuario, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Tesoreria_GenerarNotaContableFondos_CancelarPendientes(ByVal pstrTipoConsulta As String, ByVal pstrRegistros As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaValidacionEstadoDocumento)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_GenerarNotaContableFondos_CancelarPendientes(pstrTipoConsulta, pstrRegistros, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "Tesoreria_GenerarNotaContableFondos_ConsultarPendientes"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Tesoreria_GenerarNotaContableFondos_ConsultarPendientes")
            Return Nothing
        End Try
    End Function

    Public Function Tesoreria_GenerarNotaContableFondos_CancelarPendientesSync(ByVal pstrTipoConsulta As String, ByVal pstrRegistros As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaValidacionEstadoDocumento)
        Dim objTask As Task(Of List(Of RespuestaValidacionEstadoDocumento)) = Me.Tesoreria_GenerarNotaContableFondos_CancelarPendientesSyncAsync(pstrTipoConsulta, pstrRegistros, pstrUsuario, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function Tesoreria_GenerarNotaContableFondos_CancelarPendientesSyncAsync(ByVal pstrTipoConsulta As String, ByVal pstrRegistros As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of RespuestaValidacionEstadoDocumento))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of RespuestaValidacionEstadoDocumento)) = New TaskCompletionSource(Of List(Of RespuestaValidacionEstadoDocumento))()
        objTaskComplete.TrySetResult(Tesoreria_GenerarNotaContableFondos_CancelarPendientes(pstrTipoConsulta, pstrRegistros, pstrUsuario, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Sub InsertGenerarNotasFondos_Retiros(ByVal currentGenerarNotasFondos_Retiros As GenerarNotasFondos_Retiros)

    End Sub

    Public Sub UpdateGenerarNotasFondos_Retiros(ByVal currentGenerarNotasFondos_Retiros As GenerarNotasFondos_Retiros)

    End Sub

    Public Function Tesoreria_GenerarNotaContableFondos_Retiros(ByVal pintIDCompania As Integer, ByVal pintIDEncargogistros As Integer, ByVal pstrCodigoOYD As String, ByVal pstrNombreConsecutivo As String, ByVal pdtmFechaInicialMovimiento As DateTime, ByVal pdtmFechaFinalMovimiento As DateTime, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GenerarNotasFondos_Retiros)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_GenerarNotaContableFondos_ConsultarRetiros(pintIDCompania, pintIDEncargogistros, pstrCodigoOYD, pstrNombreConsecutivo, pdtmFechaInicialMovimiento, pdtmFechaFinalMovimiento, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "Tesoreria_GenerarNotaContableFondos_ConsultarPendientes"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Tesoreria_GenerarNotaContableFondos_Retiros")
            Return Nothing
        End Try
    End Function

    Public Function Tesoreria_GenerarNotaContableFondos_RetirosSync(ByVal pintIDCompania As Integer, ByVal pintIDEncargogistros As Integer, ByVal pstrCodigoOYD As String, ByVal pstrNombreConsecutivo As String, ByVal pdtmFechaInicialMovimiento As DateTime, ByVal pdtmFechaFinalMovimiento As DateTime, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GenerarNotasFondos_Retiros)
        Dim objTask As Task(Of List(Of GenerarNotasFondos_Retiros)) = Me.Tesoreria_GenerarNotaContableFondos_RetirosSyncAsync(pintIDCompania, pintIDEncargogistros, pstrCodigoOYD, pstrNombreConsecutivo, pdtmFechaInicialMovimiento, pdtmFechaFinalMovimiento, pstrUsuario, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function Tesoreria_GenerarNotaContableFondos_RetirosSyncAsync(ByVal pintIDCompania As Integer, ByVal pintIDEncargogistros As Integer, ByVal pstrCodigoOYD As String, ByVal pstrNombreConsecutivo As String, ByVal pdtmFechaInicialMovimiento As DateTime, ByVal pdtmFechaFinalMovimiento As DateTime, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of GenerarNotasFondos_Retiros))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of GenerarNotasFondos_Retiros)) = New TaskCompletionSource(Of List(Of GenerarNotasFondos_Retiros))()
        objTaskComplete.TrySetResult(Tesoreria_GenerarNotaContableFondos_Retiros(pintIDCompania, pintIDEncargogistros, pstrCodigoOYD, pstrNombreConsecutivo, pdtmFechaInicialMovimiento, pdtmFechaFinalMovimiento, pstrUsuario, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#Region "Otros"
    Public Function CalcularDiaHabil(ByVal pdtmFechaOrden As DateTime, pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblFechasHabiles)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ValidarDiaHabil_OrdenesTesoreria(pdtmFechaOrden, pstrUsuario, DemeInfoSesion(pstrUsuario, "CalcularDiaHabil"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CalcularDiaHabil")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Generación Automatica de CE y RC"

    ''' <summary>
    ''' Función que permite consultar los ultimos valores sugeridos y tambien para guardarlos.
    ''' </summary>
    ''' <param name="pstrOperacion"></param>
    ''' <param name="pstrTipo"></param>
    ''' <param name="pcmbNombreConsecutivos"></param>
    ''' <param name="pCmbCuentaContable"></param>
    ''' <param name="pcmbfFechaCorte"></param>
    ''' <param name="pTxtRecibi"></param>
    ''' <param name="pcmbvTipoId"></param>
    ''' <param name="ptxtNumeroId"></param>
    ''' <param name="ptxtIdBanco"></param>
    ''' <param name="ptxtBanco"></param>
    ''' <param name="ptxtCCosto"></param>
    ''' <param name="pcmbFormaPago1"></param>
    ''' <param name="pTxtNroCheque"></param>
    ''' <param name="pTxtBancoGirador"></param>
    ''' <param name="pcmbfFechaConsignacion"></param>
    ''' <param name="pchkClienteRegistrar"></param>
    ''' <param name="pTxtObservaciones"></param>
    ''' <param name="strFormaPagoCE"></param>
    ''' <param name="strTipoIdentificacion"></param>
    ''' <param name="strNombreConsecutivo"></param>
    ''' <param name="lngNroDocumento"></param>
    ''' <param name="strIDCuentaContable"></param>
    ''' <param name="lngNumCheque"></param>
    ''' <param name="dtmDocumento"></param>
    ''' <returns>Retorna una lista de tipo UltimosValores</returns>
    ''' <remarks>SLB20130419</remarks>
    Public Function ConsultarUltimosValores(ByVal pstrOperacion As String, ByVal pstrTipo As String, ByVal pcmbNombreConsecutivos? As Integer,
                                            ByVal pCmbCuentaContable? As Integer, ByVal pcmbfFechaCorte As String, ByVal pTxtRecibi As String,
                                            ByVal pcmbvTipoId? As Integer, ByVal ptxtNumeroId As String, ByVal ptxtIdBanco? As Integer,
                                            ByVal ptxtBanco As String, ByVal ptxtCCosto As String, ByVal pcmbFormaPago1? As Integer,
                                            ByVal pTxtNroCheque As String, ByVal pTxtBancoGirador As String, ByVal pcmbfFechaConsignacion As String,
                                            ByVal pchkClienteRegistrar As Boolean, ByVal pTxtObservaciones As String, ByVal strFormaPagoCE As String,
                                            ByVal strTipoIdentificacion As String, ByVal strNombreConsecutivo As String, ByVal lngNroDocumento As String,
                                            ByVal strIDCuentaContable As String, ByVal lngNumCheque? As Double, ByVal dtmDocumento? As Date,
                                            ByVal dtmConsignacion? As Date, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of UltimosValores)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_Ultimos_Valores_OyDNet(pstrOperacion, pstrTipo, pcmbNombreConsecutivos, pCmbCuentaContable, pcmbfFechaCorte,
                                                                pTxtRecibi, pcmbvTipoId, ptxtNumeroId, ptxtIdBanco, ptxtBanco, ptxtCCosto, pcmbFormaPago1,
                                                                pTxtNroCheque, pTxtBancoGirador, pcmbfFechaConsignacion, pchkClienteRegistrar, pTxtObservaciones,
                                                                strFormaPagoCE, strTipoIdentificacion, strNombreConsecutivo, lngNroDocumento, strIDCuentaContable,
                                                                lngNumCheque, dtmDocumento, dtmConsignacion, DemeInfoSesion(pstrUsuario, "ConsultarUltimosValores"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarUltimosValores")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función que permite consultar las liquidaciones pendientes de Tesorería.
    ''' </summary>
    ''' <param name="pdtmCumplimientoEfectivo"></param>
    ''' <param name="pstrFormaPago"></param>
    ''' <param name="pstrReceptor"></param>
    ''' <param name="pstrModulo"></param>
    ''' <param name="pstrTipoCliente"></param>
    ''' <param name="pstrSistema"></param>
    ''' <param name="pstrTipoOperacion"></param>
    ''' <param name="pstrClaseAccion"></param>
    ''' <param name="pstrTipo"></param>
    ''' <returns>Retorna una lista de tipo TesoreriaGA</returns>
    ''' <remarks>SLB20130419</remarks>
    Public Function ConsultarPendientesTesoreria(ByVal pdtmCumplimientoEfectivo As Date, ByVal pstrFormaPago As String, ByVal pstrReceptor As String,
                                        ByVal pstrModulo As String, ByVal pstrTipoCliente As String, ByVal pstrSistema As String, ByVal pstrTipoOperacion As String,
                                        ByVal pstrClaseAccion As String, ByVal pstrTipo As String, ByVal Modulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TesoreriaGA)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Select Case Modulo
                Case "CE"
                    Dim ret = Me.DataContext.usp_CEAutomatico_Generar_OyDNet(pdtmCumplimientoEfectivo, pstrFormaPago, pstrReceptor, pstrModulo, pstrTipoCliente,
                                                                     pstrSistema, pstrTipoOperacion, pstrClaseAccion, pstrTipo, DemeInfoSesion(pstrUsuario, "ConsultarPendientesTesoreria"), 0).ToList
                    Return ret
                Case "RC"
                    Dim ret = Me.DataContext.usp_RCAutomatico_Generar_OyDNet(pdtmCumplimientoEfectivo, pstrFormaPago, pstrReceptor, pstrModulo, pstrTipoCliente,
                                                                     pstrSistema, pstrTipoOperacion, pstrClaseAccion, pstrTipo, DemeInfoSesion(pstrUsuario, "ConsultarPendientesTesoreria"), 0).ToList
                    Return ret
            End Select
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarPendientesTesoreria")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función que permite consultar las liquidaciones pendientes de operaciones convenidas de Tesorería.
    ''' </summary>
    ''' <param name="pdtmCumplimientoEfectivo"></param>
    ''' <param name="pstrFormaPago"></param>
    ''' <param name="pstrReceptor"></param>
    ''' <param name="Modulo"></param>
    ''' <returns>Retorna una lista de tipo TesoreriaGAConvenida</returns>
    ''' <remarks>SLB20130515</remarks>
    Public Function ConsultarPendientesConvenidasTesoreria(ByVal pdtmCumplimientoEfectivo As Date, ByVal pstrFormaPago As String, ByVal pstrReceptor As String,
                                                           ByVal Modulo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TesoreriaGAConvenida)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Select Case Modulo
                Case "CE"
                    Dim ret = Me.DataContext.usp_CEAutomatico_OpCompraConvenidas_Consultar_OyDNet(pdtmCumplimientoEfectivo, pstrFormaPago, pstrReceptor, DemeInfoSesion(pstrUsuario, "ConsultarPendientesConvenidasTesoreria"), 0).ToList
                    Return ret
                Case "RC"
                    Dim ret = Me.DataContext.usp_RCAutomatico_OpVentaConvenidas_Consultar_OyDNet(pdtmCumplimientoEfectivo, pstrFormaPago, pstrReceptor, DemeInfoSesion(pstrUsuario, "ConsultarPendientesConvenidasTesoreria"), 0).ToList
                    Return ret
            End Select
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarPendientesConvenidasTesoreria")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de insertar el encabezado de un CE.
    ''' </summary>
    ''' <param name="pstrNombreConsecutivo"></param>
    ''' <param name="pstrTipoIdentificacion"></param>
    ''' <param name="plngNroDocumento"></param>
    ''' <param name="pstrNombre"></param>
    ''' <param name="plngIdBanco"></param>
    ''' <param name="plngNumCheque"></param>
    ''' <param name="pdtmDocumento"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <param name="pstrFormaPagoCE"></param>
    ''' <param name="pstrCuentaCliente"></param>
    ''' <returns>Retorna el consecutivo del CE insetado.</returns>
    ''' <remarks>SLB20130422</remarks>
    Public Function InsertarCE(ByVal pstrNombreConsecutivo As String, ByVal pstrTipoIdentificacion As String, ByVal plngNroDocumento As String,
                                    ByVal pstrNombre As String, ByVal plngIdBanco As Integer, ByVal plngNumCheque As Integer?, ByVal pdtmDocumento As Date,
                                    ByVal pstrUsuario As String, ByVal pstrFormaPagoCE As String, ByVal pstrCuentaCliente As String,
                                    ByVal pstrTipocheque As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spInsCompEgreso_OyDNet(pstrNombreConsecutivo, pstrTipoIdentificacion, plngNroDocumento, pstrNombre, plngIdBanco,
                                                            plngNumCheque, pdtmDocumento, pstrUsuario, pstrFormaPagoCE, pstrCuentaCliente,
                                                            DemeInfoSesion(pstrUsuario, "InsertarCE"), 0, pstrTipocheque)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertarCE")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de insertar los detalles de un CE.
    ''' </summary>
    ''' <param name="pstrNombreConsecutivo"></param>
    ''' <param name="plngIdDocumento"></param>
    ''' <param name="plngSecuencia"></param>
    ''' <param name="plngComitente"></param>
    ''' <param name="pcurValor"></param>
    ''' <param name="pstrIdCuentaContable"></param>
    ''' <param name="pstrDetalle"></param>
    ''' <param name="pstrNIT"></param>
    ''' <param name="pstrCCosto"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <param name="pstrNombreConsecutivoNotaGMF"></param>
    ''' <param name="plngIDDocumentoNotaGMF"></param>
    ''' <param name="pintExportados"></param>
    ''' <returns>Rertorna el resultado de la inserción del detalle del CE.</returns>
    ''' <remarks>SLB20130422</remarks>
    Public Function InsertarCEDetalle(ByVal pstrNombreConsecutivo As String, ByVal plngIdDocumento As Integer, ByVal plngSecuencia As Integer,
                                ByVal plngComitente As String, ByVal pcurValor As Decimal, ByVal pstrIdCuentaContable As String, ByVal pstrDetalle As String,
                                ByVal pstrNIT As String, ByVal pstrCCosto As String, ByVal pstrUsuario As String, ByVal pstrNombreConsecutivoNotaGMF As String,
                                ByVal plngIDDocumentoNotaGMF? As Integer, ByVal pintExportados? As Integer, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spInsCompEgresoDetalle_OyDNet(pstrNombreConsecutivo, plngIdDocumento, plngSecuencia, plngComitente, pcurValor, pstrIdCuentaContable,
                                                                   pstrDetalle, pstrNIT, pstrCCosto, pstrUsuario, pstrNombreConsecutivoNotaGMF, plngIDDocumentoNotaGMF,
                                                                   pintExportados, DemeInfoSesion(pstrUsuario, "InsertarCEDetalle"), 0)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertarCEDetalle")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de insertar el encabezado de un RC.
    ''' </summary>
    ''' <param name="pstrNombreConsecutivo"></param>
    ''' <param name="pstrTipoIdentificacion"></param>
    ''' <param name="plngNroDocumento"></param>
    ''' <param name="pstrNombre"></param>
    ''' <param name="pdtmDocumento"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>Retorna el consecutivo del RC insetado.</returns>
    ''' <remarks>SLB20130509</remarks>
    Public Function InsertarRC(ByVal pstrNombreConsecutivo As String, ByVal pstrTipoIdentificacion As String, ByVal plngNroDocumento As String,
                                    ByVal pstrNombre As String, ByVal pdtmDocumento As Date, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spInsReciboCaja_OyDNet(pstrNombreConsecutivo, pstrTipoIdentificacion, plngNroDocumento, pstrNombre, pdtmDocumento,
                                                            pstrUsuario, DemeInfoSesion(pstrUsuario, "InsertarRC"), 0)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertarRC")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de insertar los detalles de un RC.
    ''' </summary>
    ''' <param name="pstrNombreConsecutivo"></param>
    ''' <param name="plngIdDocumento"></param>
    ''' <param name="plngSecuencia"></param>
    ''' <param name="plngComitente"></param>
    ''' <param name="pcurValor"></param>
    ''' <param name="pstrIdCuentaContable"></param>
    ''' <param name="pstrDetalle"></param>
    ''' <param name="pstrNIT"></param>
    ''' <param name="pstrCCosto"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>Rertorna el resultado de la inserción del detalle del RC.</returns>
    ''' <remarks>SLB20130509</remarks>
    Public Function InsertarRCDetalle(ByVal pstrNombreConsecutivo As String, ByVal plngIdDocumento As Integer, ByVal plngSecuencia As Integer,
                                ByVal plngComitente As String, ByVal pcurValor As Decimal, ByVal pstrIdCuentaContable As String, ByVal pstrDetalle As String,
                                ByVal pstrNIT As String, ByVal pstrCCosto As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spInsReciboCajaDetalle_OyDNet(pstrNombreConsecutivo, plngIdDocumento, plngSecuencia, plngComitente, pcurValor, pstrIdCuentaContable,
                                                                   pstrDetalle, pstrNIT, pstrCCosto, pstrUsuario, DemeInfoSesion(pstrUsuario, "InsertarRCDetalle"), 0)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertarRCDetalle")
            Return Nothing
        End Try
    End Function

    Public Function InsertarCheque(ByVal pstrNombreConsecutivo As String, ByVal plngIdDocumento As Integer, ByVal plngSecuencia As Integer,
                                ByVal plogEfectivo As Boolean, ByVal pstrBancoGirador As String, ByVal plngNumCheque? As Decimal, ByVal pcurValor As Decimal,
                                ByVal plngBancoConsignacion As Integer, ByVal pdtmConsignacion As Date, ByVal pstrUsuario As String, ByVal pstrComentario As String,
                                ByVal pstrFormaPagoRC As String, ByVal plngIDTipoProducto? As Integer, ByVal pstrSucursalesBancolombia As String,
                                ByVal plogChequeHizoCanje? As Boolean, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.spInsReciboCajaCheque_OyDNet(pstrNombreConsecutivo, plngIdDocumento, plngSecuencia, plogEfectivo, pstrBancoGirador,
                                                                  plngNumCheque, pcurValor, plngBancoConsignacion, pdtmConsignacion, pstrUsuario, pstrComentario,
                                                                  pstrFormaPagoRC, plngIDTipoProducto, pstrSucursalesBancolombia, plogChequeHizoCanje, DemeInfoSesion(pstrUsuario, "InsertarCheque"), 0)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertarCheque")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de insertar de las liquidaciones de tesorería.
    ''' </summary>
    ''' <param name="plngID"></param>
    ''' <param name="plngIDSecuencia"></param>
    ''' <param name="plngIDDocumento"></param>
    ''' <param name="pstrNombreConsecutivo"></param>
    ''' <param name="pstrTipo"></param>
    ''' <returns>Rertorna el resultado de la inserción de la liquidación de tesorería.</returns>
    ''' <remarks>SLB20130422</remarks>
    Public Function InsertarLiquidacionesTesoreria(ByVal plngID As Integer, ByVal plngIDSecuencia As Integer, ByVal plngIDDocumento As Integer,
                            ByVal pstrNombreConsecutivo As String, ByVal pstrTipo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_Actualizar_tblLiquidacionesTesoreria_OyDNet(plngID, plngIDSecuencia, plngIDDocumento, pstrNombreConsecutivo, pstrTipo,
                                                                                     DemeInfoSesion(pstrUsuario, "InsertarLiquidacionesTesoreria"), 0)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertarLiquidacionesTesoreria")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de borrar los documentos de tesorería en caso de haberse producido un error.
    ''' </summary>
    ''' <param name="plngIDDocumento"></param>
    ''' <param name="pstrNombreConsecutivo"></param>
    ''' <param name="pstrTipo"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns></returns>
    ''' <remarks>SLB20130422</remarks>
    Public Function BorrarDocumentoTesoreria(ByVal plngIDDocumento As Integer, ByVal pstrNombreConsecutivo As String, ByVal pstrTipo As String,
                                            ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_Borrar_GA(plngIDDocumento, pstrNombreConsecutivo, pstrTipo, pstrUsuario,
                                                                                     DemeInfoSesion(pstrUsuario, "BorrarDocumentoTesoreria"), 0)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BorrarDocumentoTesoreria")
            Return Nothing
        End Try
    End Function


    Public Function InsertarLiquidacionesBolsa_Tesoreria(ByVal plngIDLiquidacion As Integer, ByVal plngParcial As Integer, ByVal pdtmLiquidacion As Date,
                                                         ByVal pstrTipoLiquidacion As String, ByVal pstrClaseLiquidacion As String, ByVal plngIDBolsaLiquidacion As Integer,
                                                         ByVal plngIDDctoTesoreria As Integer, ByVal plngSecuenciaDctoTesoreria As Integer, ByVal pstrNombreConsecutivo As String,
                                                         ByVal pstrTipoDctoTesoreria As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_LiquidacionesBolsa_Tesoreria_Relacion_Ingresar_OyDNet(plngIDLiquidacion, plngParcial, pdtmLiquidacion, pstrTipoLiquidacion,
                                                                                               pstrClaseLiquidacion, plngIDBolsaLiquidacion, plngIDDctoTesoreria, plngSecuenciaDctoTesoreria,
                                                                                               pstrNombreConsecutivo, pstrTipoDctoTesoreria, DemeInfoSesion(pstrUsuario, "InsertarLiquidacionesBolsa_Tesoreria"), 0)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertarLiquidacionesBolsa_Tesoreria")
            Return Nothing
        End Try
    End Function

    Public Sub UpdateTesoreriaGA(ByVal currentTesoreriaGAs As TesoreriaGA)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentTesoreriaGAs.pstrUsuarioConexion, currentTesoreriaGAs.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.TesoreriaGA.InsertOnSubmit(currentTesoreriaGAs)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTesoreriaGA")
        End Try
    End Sub

    Public Sub DeleteTesoreriaGA(ByVal currentTesoreriaGAs As TesoreriaGA)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentTesoreriaGAs.pstrUsuarioConexion, currentTesoreriaGAs.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.TesoreriaGA.Attach(currentTesoreriaGAs)
            Me.DataContext.TesoreriaGA.DeleteOnSubmit(currentTesoreriaGAs)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTesoreriaGA")
        End Try
    End Sub

    Public Sub UpdateUltimosValores(ByVal currentUltimosValores As UltimosValores)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentUltimosValores.pstrUsuarioConexion, currentUltimosValores.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.UltimosValores.InsertOnSubmit(currentUltimosValores)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateUltimosValores")
        End Try
    End Sub

    Public Sub UpdateTesoreriaGAConvenida(ByVal currentTesoreriaGAConvenidas As TesoreriaGAConvenida)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentTesoreriaGAConvenidas.pstrUsuarioConexion, currentTesoreriaGAConvenidas.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.TesoreriaGAConvenida.InsertOnSubmit(currentTesoreriaGAConvenidas)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTesoreriaGAConvenida")
        End Try
    End Sub

    Public Sub DeleteTesoreriaGAConvenida(ByVal currentTesoreriaGAConvenidas As TesoreriaGAConvenida)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentTesoreriaGAConvenidas.pstrUsuarioConexion, currentTesoreriaGAConvenidas.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.TesoreriaGAConvenida.Attach(currentTesoreriaGAConvenidas)
            Me.DataContext.TesoreriaGAConvenida.DeleteOnSubmit(currentTesoreriaGAConvenidas)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTesoreriaGA")
        End Try
    End Sub

    ''' <history>
    ''' Modificado por   : Yeiny Adenis Marín Zapata.
    ''' Descripción      : Con este método se obtienen los cheques pendientes por imprimir.
    ''' Fecha            : Mayo 28/2013
    ''' Pruebas CB       : Yeiny Adenis Marín Zapata - Mayo 28/2013 - Resultado Ok 
    ''' </history>
    Public Function ConsultarChequesxImprimir(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ImprimirCheques)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_TesoreriaChequesxImprimir_Consultar(pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarChequesxImprimir"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarChequesxImprimir")
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' Se consultan los cheques para repetir impresión
    ''' </summary>
    ''' <param name="pstrConsecutivo"></param>
    ''' <param name="pintIdDocumento"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <returns>SV20130628</returns>
    Public Function ConsultarRepetirCheques(ByVal pstrConsecutivo As String, ByVal pintIdDocumento As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ImprimirCheques)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_TesoreriaRepetirCheques_Consultar(pstrConsecutivo, pintIdDocumento, pstrUsuario, DemeInfoSesion(pstrUsuario, "ConsultarRepetirCheques"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarRepetirCheques")
            Return Nothing
        End Try
    End Function


#End Region

#Region "Generar Archivo Plano Operaciones"

    ''' <summary>
    ''' Metodo encargado de generar el archivo plano de operaciones en efectivo.
    ''' </summary>
    ''' <param name="pdtmDesde"></param>
    ''' <param name="pdtmHasta"></param>
    ''' <param name="NombreArchivo"></param>
    ''' <param name="NombreProceso"></param>
    ''' <param name="Usuario"></param>
    ''' <returns>Retorna True si el archivo plano se genero correctamente.</returns>
    ''' <remarks>SLB20130607</remarks>
    Public Function GenerarArchivoPlanoOperaciones(ByVal pdtmDesde As Date, ByVal pdtmHasta As Date, ByVal NombreArchivo As String,
                                       ByVal NombreProceso As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strValor As String = String.Empty
            Dim strFila As String = String.Empty
            Dim lineasArchivo As New List(Of String)

            Dim res = Me.DataContext.uspOperacionesEnEfectivoRealizadasForFile_OyDNet(pdtmDesde, pdtmHasta, DemeInfoSesion(pstrUsuario, "GenerarArchivoPlanoACH"), ClsConstantes.GINT_ErrorPersonalizado).ToList

            If res.Count > 0 Then
                For Each objImp In res
                    lineasArchivo.Add(objImp.strImportar)
                Next
            End If

            NombreArchivo = DateTime.Now.ToString() & NombreArchivo
            NombreArchivo = NombreArchivo.Replace(CType("/", Char), CType("_", Char))
            NombreArchivo = NombreArchivo.Replace(CType(" ", Char), CType("_", Char))
            NombreArchivo = NombreArchivo.Replace(CType(":", Char), CType("_", Char))

            Return GuardarArchivo(NombreProceso, pstrUsuario, NombreArchivo, lineasArchivo, True)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarArchivoPlanoOperaciones")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Generar Archivo Plano ACH"

    ''' <summary>
    ''' Función encargada de consultar los CE con los cuales se puede generar Archivos Planos para ACH.
    ''' </summary>
    ''' <param name="pdtmFechaElaboracionCEDesde"></param>
    ''' <param name="pdtmFechaElaboracionCEHasta"></param>
    ''' <param name="pdtmConsecutivoCE"></param>
    ''' <param name="intNroCE"></param>
    ''' <param name="intBanco"></param>
    ''' <param name="strFormato"></param>
    ''' <returns>Retorna una lista de tipo TesoreriaACHPendiente</returns>
    ''' <remarks>SLB20130531</remarks>
    Public Function ConsultarPendientesACH(ByVal pdtmFechaElaboracionCEDesde As Date?, ByVal pdtmFechaElaboracionCEHasta As Date?, ByVal pdtmConsecutivoCE As String,
                                    ByVal intNroCE As Integer?, ByVal intBanco As Integer?, ByVal strFormato As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of TesoreriaACHPendiente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim ret = Me.DataContext.usp_OyD_DetalleComprobanteEgreso_Transferencia_Consultar_OyDNet(pdtmFechaElaboracionCEDesde, pdtmFechaElaboracionCEHasta, pdtmConsecutivoCE,
                                                                                                     intNroCE, intBanco, strFormato, DemeInfoSesion(pstrUsuario, "ConsultarPendientesACH"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarPendientesACH")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función que se encarga de insertar los CE exportados ACH.
    ''' </summary>
    ''' <param name="pstrTipo"></param>
    ''' <param name="pstrNombreConsecutivo"></param>
    ''' <param name="pdtmConsecutivoCE"></param>
    ''' <param name="pintNroCE"></param>
    ''' <returns></returns>
    ''' <remarks>SLB20130531</remarks>
    Public Function InsertaRCExportados(ByVal pstrTipo As String, ByVal pstrNombreConsecutivo As String, ByVal pdtmConsecutivoCE As Date,
                                        ByVal pintNroCE As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim ret = Me.DataContext.usp_OyD_IngresarRCExportados_OyDNet(pstrTipo, pstrNombreConsecutivo, pdtmConsecutivoCE, pintNroCE, DemeInfoSesion(pstrUsuario, "InsertaRCExportados"), 0)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertaRCExportados")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de insertar los datos a exportar para generar el plano ACH.
    ''' </summary>
    ''' <param name="intCodigoFormatoExportacion"></param>
    ''' <param name="xml_text"></param>
    ''' <param name="strFormato"></param>
    ''' <returns></returns>
    ''' <remarks>SLB20130531</remarks>
    Public Function InsertarDatosPlanoACH(ByVal intCodigoFormatoExportacion As Integer, ByVal xml_text As String, ByVal strFormato As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim ret = Me.DataContext.usp_OyD_PlanoPagosATerceros_Generar_Bancos_OyDNet(intCodigoFormatoExportacion, xml_text, strFormato, DemeInfoSesion(pstrUsuario, "InsertarDatosPlanoACH"), 0)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertarDatosPlanoACH")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de generar el archivo plano ACH, a partir de los datos insertados previamente.
    ''' </summary>
    ''' <param name="lngFormato"></param>
    ''' <param name="lngIDBanco"></param>
    ''' <param name="strFormato"></param>
    ''' <param name="NombreArchivo"></param>
    ''' <param name="NombreProceso"></param>
    ''' <param name="Usuario"></param>
    ''' <returns></returns>
    ''' <remarks>SLB20130604</remarks>
    Public Function GenerarArchivoPlanoACH(ByVal lngFormato As Integer, ByVal lngIDBanco As Integer, ByVal strFormato As String, ByVal NombreArchivo As String,
                                           ByVal NombreProceso As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strValor As String = String.Empty
            Dim strFila As String = String.Empty
            Dim ListaResultado As New List(Of FormatosExportacionDetalle)
            Dim intNumeroRegistrosTipo2 As Integer
            Dim intContador, NroFilas, intCont As Integer
            Dim strContador As String
            Dim bolNuevaFila As Boolean
            Dim lineasArchivo As New List(Of String)

            'Esta consulta es para tipo 1
            ListaResultado = Me.DataContext.uspOyDNet_FormatosExportacionDetalle_Consultar(lngFormato, 1, DemeInfoSesion(pstrUsuario, "GenerarArchivoPlanoACH"), ClsConstantes.GINT_ErrorPersonalizado).ToList()

            If ListaResultado.Count > 0 Then

                NroFilas = ListaResultado.Count

                For intCont = 0 To NroFilas - 1

                    If intCont = 0 Then
                        If lngFormato = 9 And ListaResultado(intCont).OrdenCampo = 0 Then
                            strValor = CStr(L2SDC.ExecuteQuery(Of Object)(ListaResultado(intCont).strSQL).FirstOrDefault)
                            lineasArchivo.Add(strValor)
                        End If
                    End If

                    If ListaResultado(intCont).IDTipoRegistro = 1 Then

                        If Not String.IsNullOrEmpty(ListaResultado(intCont).ValorPOrDefecto) Then
                            strValor = ListaResultado(intCont).ValorPOrDefecto
                        Else
                            If Not String.IsNullOrEmpty(ListaResultado(intCont).strSQL) Then
                                If Right(Trim(ListaResultado(intCont).strSQL), 1) = "?" Then
                                    strValor = fnEjecutarQuerySQL(Replace(ListaResultado(intCont).strSQL, "?", "@p0"), lngIDBanco)
                                Else
                                    strValor = fnEjecutarQuerySQL(ListaResultado(intCont).strSQL)
                                End If
                            End If
                        End If

                    End If

                    strFila = strFila & fnFormatearCampos(strValor, CStr(IIf(String.IsNullOrEmpty(ListaResultado(intCont).ValorPOrDefecto), "", ListaResultado(intCont).ValorPOrDefecto)), ListaResultado(intCont).PosicionInicial, ListaResultado(intCont).PosicionFinal,
                                                          ListaResultado(intCont).JustificadoIzquierda, ListaResultado(intCont).JustificadoDerecha, ListaResultado(intCont).CaracterJustificacion, pstrUsuario, pstrInfoConexion)

                    If ListaResultado(intCont).logSeparadorCampo Then
                        strFila = strFila & ListaResultado(intCont).strSeparadorCampo
                    End If
                Next

                lineasArchivo.Add(strFila)
            End If

            'Esta consulta es para tipo 2.
            strFila = ""
            strValor = ""

            If lngFormato = 9 Then
                intNumeroRegistrosTipo2 = L2SDC.ExecuteQuery(Of Integer)("Select count(*) as strcampo From TmpDatosPagoXRedBancoGentiExt").FirstOrDefault
            Else
                intNumeroRegistrosTipo2 = L2SDC.ExecuteQuery(Of Integer)("Select count(*) as strcampo From tmpDatos" & strFormato).FirstOrDefault
            End If

            ListaResultado.Clear()
            ListaResultado = Me.DataContext.uspOyDNet_FormatosExportacionDetalle_Consultar(lngFormato, 2, DemeInfoSesion(pstrUsuario, "GenerarArchivoPlanoACH"), ClsConstantes.GINT_ErrorPersonalizado).ToList()

            If ListaResultado.Count > 0 Then

                NroFilas = ListaResultado.Count
                intContador = 1
                strContador = CStr(intContador)
                intCont = 0

                For intCont = 0 To NroFilas - 1

                    If bolNuevaFila Then
                        bolNuevaFila = False
                        intCont = 0
                    End If

                    If Not String.IsNullOrEmpty(ListaResultado(intCont).ValorPOrDefecto) Then
                        strValor = ListaResultado(intCont).ValorPOrDefecto
                    Else
                        If Not String.IsNullOrEmpty(ListaResultado(intCont).strSQL) Then
                            If Right(Trim(ListaResultado(intCont).strSQL), 1) = "?" Then
                                strValor = fnEjecutarQuerySQL(Replace(ListaResultado(intCont).strSQL, "?", "@p0"), strContador)
                            Else
                                If Right(Trim(ListaResultado(intCont).strSQL), 3) = "txt" Then
                                    strValor = fnEjecutarQuerySQL(Replace(ListaResultado(intCont).strSQL, "txt", "@p0"), lngIDBanco)
                                Else
                                    strValor = fnEjecutarQuerySQL(ListaResultado(intCont).strSQL)
                                End If
                            End If
                        End If
                    End If

                    If intCont = NroFilas - 1 Then

                        If intNumeroRegistrosTipo2 >= intContador Then
                            intContador = intContador + 1
                            strContador = CStr(intContador)
                            bolNuevaFila = True
                        End If

                        If intNumeroRegistrosTipo2 = 1 Then
                            strFila = strFila & fnFormatearCampos(strValor, CStr(IIf(String.IsNullOrEmpty(ListaResultado(intCont).ValorPOrDefecto), "", ListaResultado(intCont).ValorPOrDefecto)), ListaResultado(intCont).PosicionInicial, ListaResultado(intCont).PosicionFinal,
                                                          ListaResultado(intCont).JustificadoIzquierda, ListaResultado(intCont).JustificadoDerecha, ListaResultado(intCont).CaracterJustificacion, pstrUsuario, pstrInfoConexion)
                            lineasArchivo.Add(strFila)
                            strFila = ""
                            bolNuevaFila = False
                        End If

                    End If

                    strFila = strFila & fnFormatearCampos(strValor, CStr(IIf(String.IsNullOrEmpty(ListaResultado(intCont).ValorPOrDefecto), "", ListaResultado(intCont).ValorPOrDefecto)), ListaResultado(intCont).PosicionInicial, ListaResultado(intCont).PosicionFinal,
                                                          ListaResultado(intCont).JustificadoIzquierda, ListaResultado(intCont).JustificadoDerecha, ListaResultado(intCont).CaracterJustificacion, pstrUsuario, pstrInfoConexion)

                    If ListaResultado(intCont).logSeparadorCampo Then
                        If Not bolNuevaFila Then
                            strFila = strFila & ListaResultado(intCont).strSeparadorCampo
                        End If
                    End If

                    If bolNuevaFila Then
                        lineasArchivo.Add(strFila)
                        strFila = ""
                        intCont = 0
                    End If
                Next

            End If

            'Esta consulta es para tipo 3(Registro de totales).
            strFila = ""
            strValor = ""

            ListaResultado.Clear()
            ListaResultado = Me.DataContext.uspOyDNet_FormatosExportacionDetalle_Consultar(lngFormato, 3, DemeInfoSesion(pstrUsuario, "GenerarArchivoPlanoACH"), ClsConstantes.GINT_ErrorPersonalizado).ToList()

            If ListaResultado.Count > 0 Then

                NroFilas = ListaResultado.Count
                intContador = 1
                strContador = CStr(intContador)
                intCont = 0

                For intCont = 0 To NroFilas - 1

                    If Not String.IsNullOrEmpty(ListaResultado(intCont).ValorPOrDefecto) Then
                        strValor = ListaResultado(intCont).ValorPOrDefecto
                    Else
                        If Not String.IsNullOrEmpty(ListaResultado(intCont).strSQL) Then
                            If Right(Trim(ListaResultado(intCont).strSQL), 1) = "?" Then
                                strValor = fnEjecutarQuerySQL(Replace(ListaResultado(intCont).strSQL, "?", "@p0"), lngIDBanco)
                            Else
                                strValor = fnEjecutarQuerySQL(ListaResultado(intCont).strSQL)
                            End If
                        End If
                    End If

                    strFila = strFila & fnFormatearCampos(strValor, CStr(IIf(String.IsNullOrEmpty(ListaResultado(intCont).ValorPOrDefecto), "", ListaResultado(intCont).ValorPOrDefecto)), ListaResultado(intCont).PosicionInicial, ListaResultado(intCont).PosicionFinal,
                                                              ListaResultado(intCont).JustificadoIzquierda, ListaResultado(intCont).JustificadoDerecha, ListaResultado(intCont).CaracterJustificacion, pstrUsuario, pstrInfoConexion)
                Next
                lineasArchivo.Add(strFila)
            End If

            NombreArchivo = DateTime.Now.ToString() & NombreArchivo
            NombreArchivo = NombreArchivo.Replace(CType("/", Char), CType("_", Char))
            NombreArchivo = NombreArchivo.Replace(CType(" ", Char), CType("_", Char))
            NombreArchivo = NombreArchivo.Replace(CType(":", Char), CType("_", Char))

            'JFSB 20180321 - Se agrega el parametro pstrUsuario para que genere el archivo como es
            Return GuardarArchivo(NombreProceso, pstrUsuario, NombreArchivo, lineasArchivo, True)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarArchivoPlanoACH")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función que permite ejecutar Querys en SQL Server con o sin parametros.
    ''' </summary>
    ''' <param name="strSQL"></param>
    ''' <param name="strParametro"></param>
    ''' <returns></returns>
    ''' <remarks>SLB20130604</remarks>
    Private Function fnEjecutarQuerySQL(ByVal strSQL As String, Optional strParametro As Object = Nothing) As String
        Dim result As String = String.Empty

        If IsNothing(strParametro) Then
            Dim res = L2SDC.ExecuteQuery(Of Resultado)(strSQL).FirstOrDefault
            If Not IsNothing(res) Then
                result = CStr(res.strCampo)
            End If
        Else
            Dim res = L2SDC.ExecuteQuery(Of Resultado)(strSQL, strParametro).FirstOrDefault
            If Not IsNothing(res) Then
                result = CStr(res.strCampo)
            End If
        End If

        Return result
    End Function

    Public Class Resultado
        Public strCampo As Object
    End Class

    ''' <summary>
    ''' Función que permite formatear los campos del archivo plano.
    ''' </summary>
    ''' <param name="strValor"></param>
    ''' <param name="strValorPOrDefecto"></param>
    ''' <param name="IntPosicionInicial"></param>
    ''' <param name="IntPosicionFinal"></param>
    ''' <param name="logJustificadoIzquierda"></param>
    ''' <param name="logJustificadoDerecha"></param>
    ''' <param name="strCaracterJustificacion"></param>
    ''' <returns>Retorna un String</returns>
    ''' <remarks>SLB20130604</remarks>
    Private Function fnFormatearCampos(strValor As String, strValorPOrDefecto As String, IntPosicionInicial As Integer, IntPosicionFinal As Integer,
                                       logJustificadoIzquierda As Boolean, logJustificadoDerecha As Boolean, strCaracterJustificacion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        '/******************************************************************************************
        '/* fnFormatearCampos()
        '/* Alcance       : Privado
        '/* Descripción   : El objetivo de esta función es formatear los
        '/* Parametros    : lngIDFormato  'Formato del archivo que desea realizar(Bancolombia,Occidente).
        '/* Realizado por : Ricardo Peña (Al Cuadrado S.A).
        '/* Fecha         : 14-02-2008.
        '/******************************************************************************************

        Dim intloguitud As Integer
        Dim strdatos As String

        intloguitud = IntPosicionFinal - IntPosicionInicial + 1

        strdatos = strValor

        If logJustificadoIzquierda Then
            strdatos = Left(strValor & StrDup(intloguitud, strCaracterJustificacion), intloguitud)
        End If

        If logJustificadoDerecha Then
            strdatos = Right(StrDup(intloguitud, strCaracterJustificacion) & strValor, intloguitud)
        End If

        fnFormatearCampos = strdatos

    End Function

    ''' <summary>
    ''' Función para borrar los CE exportados en caso de que ocurra un error.
    ''' </summary>
    ''' <param name="strBorrar"></param>
    ''' <returns></returns>
    ''' <remarks>SLB20130605</remarks>
    Public Function BorrarRCExportados(ByVal strBorrar As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strSQL As String = String.Empty
            Dim parametros() As String
            Dim CExportadosBorrar = Split(strBorrar, ";")
            For Each objBorrar In CExportadosBorrar
                parametros = Split(objBorrar, "*")
                If parametros.Length = 2 Then
                    strSQL = "DELETE FROM tblIngresarRCExportados WHERE strNombreConsecutivo = " & ChrW(39) & parametros(0) & ChrW(39) & " AND lngIDDocumento = " & parametros(1)
                    L2SDC.ExecuteQuery(Of Object)(strSQL)
                End If
            Next
            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BorrarRCExportados")
            Return Nothing
        End Try

    End Function

    Public Sub UpdateTesoreriaACHPendiente(ByVal currentTesoreriaACHPendientes As TesoreriaACHPendiente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentTesoreriaACHPendientes.pstrUsuarioConexion, currentTesoreriaACHPendientes.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.TesoreriaACHPendiente.InsertOnSubmit(currentTesoreriaACHPendientes)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateTesoreriaACHPendiente")
        End Try
    End Sub

    Public Sub DeleteTesoreriaACHPendiente(ByVal currentTesoreriaACHPendientes As TesoreriaACHPendiente)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentTesoreriaACHPendientes.pstrUsuarioConexion, currentTesoreriaACHPendientes.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.TesoreriaACHPendiente.Attach(currentTesoreriaACHPendientes)
            Me.DataContext.TesoreriaACHPendiente.DeleteOnSubmit(currentTesoreriaACHPendientes)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteTesoreriaACHPendiente")
        End Try
    End Sub
    <Query(HasSideEffects:=True)>
    Public Function GenerarArchivoCarteraLiquidez_ACH(ByVal pstrRegistros As String, ByVal pstrTipoCartera As String, ByVal pstrNombreProceso As String, ByVal pstrNombreArchivo As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GeneracionCENCDeceval)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objListaRespuesta As New List(Of GeneracionCENCDeceval)
            Dim strConexion As String = A2Utilidades.Cifrar.descifrar(My.MySettings.Default.dbOYDConnectionString)
            Dim strProceso As String = "[dbo].[uspOyDNet_PlanoACHBancolombiaLiquidez_Consultar]"
            Dim intTipoSeparador As TipoSeparador = Nothing
            Dim strTipoSeparador As String = String.Empty
            Dim strRutaArchivoGeneracion As String = String.Empty
            Dim strRutaBackupGeneracion As String = String.Empty
            Dim strArchivoRutaGeneracion As String = String.Empty
            Dim strArchivoRutaBackup As String = String.Empty
            Dim strRespuestaGeneracionArchivos As String = String.Empty

            Dim objListaParametros As New List(Of SqlParameter)
            pstrRegistros = HttpUtility.UrlDecode(pstrRegistros)
            objListaParametros.Add(CrearSQLParameter("pstrNombreArchivo", pstrNombreArchivo, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrRegistrosGenerar", pstrRegistros, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrTipoCartera", pstrTipoCartera, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrUsuario", pstrUsuario, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrMaquina", pstrMaquina, SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pstrInfoSesion", DemeInfoSesion(pstrUsuario, "GenerarArchivoCarteraLiquidez_ACH"), SqlDbType.VarChar))
            objListaParametros.Add(CrearSQLParameter("pintErrorPersonalizado", 0, SqlDbType.Int))

            Dim objDatosConsulta As DataSet = RetornarDataSet(strConexion, strProceso, objListaParametros)

            For Each objRow As DataRow In objDatosConsulta.Tables(0).Rows
                If Not IsNothing(objRow("NombreArchivo")) And Not IsDBNull(objRow("NombreArchivo")) Then
                    pstrNombreArchivo = CStr(objRow("NombreArchivo"))
                End If
                If Not IsNothing(objRow("RutaArchivo")) And Not IsDBNull(objRow("RutaArchivo")) Then
                    strRutaArchivoGeneracion = CStr(objRow("RutaArchivo"))
                End If
                If Not IsNothing(objRow("RutaArchivoBackup")) And Not IsDBNull(objRow("RutaArchivoBackup")) Then
                    strRutaBackupGeneracion = CStr(objRow("RutaArchivoBackup"))
                End If
            Next

            If Not String.IsNullOrEmpty(strRutaArchivoGeneracion) Then
                If Not Directory.Exists(strRutaArchivoGeneracion) Then
                    Directory.CreateDirectory(strRutaArchivoGeneracion)
                End If

                If pstrTipoCartera = "1" Then
                    strArchivoRutaGeneracion = GenerarExcel(objDatosConsulta.Tables(1), strRutaArchivoGeneracion, pstrNombreArchivo)
                Else
                    strArchivoRutaGeneracion = GenerarTextoPlano(objDatosConsulta.Tables(1), strRutaArchivoGeneracion, pstrNombreArchivo, "csv", String.Empty, True, False, False)
                End If
            End If

            If Not String.IsNullOrEmpty(strRutaBackupGeneracion) Then
                If Not Directory.Exists(strRutaBackupGeneracion) Then
                    Directory.CreateDirectory(strRutaBackupGeneracion)
                End If

                If pstrTipoCartera = "1" Then
                    strArchivoRutaBackup = GenerarExcel(objDatosConsulta.Tables(1), strRutaBackupGeneracion, pstrNombreArchivo)
                Else
                    strArchivoRutaBackup = GenerarTextoPlano(objDatosConsulta.Tables(1), strRutaBackupGeneracion, pstrNombreArchivo, "csv", String.Empty, True, False, False)
                End If
            End If

            strRespuestaGeneracionArchivos = "Se generó el archivo en la ruta " & strRutaArchivoGeneracion & strArchivoRutaGeneracion
            strRespuestaGeneracionArchivos = strRespuestaGeneracionArchivos & vbCrLf & "Se generó el archivo backup en la ruta " & strRutaBackupGeneracion & strArchivoRutaBackup

            objListaRespuesta.Add(New GeneracionCENCDeceval With {.intID = 1,
                                                                  .strMensaje = strRespuestaGeneracionArchivos})

            Return objListaRespuesta
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarArchivoCarteraLiquidez_ACH")
            Return Nothing
        End Try
    End Function
    <Query(HasSideEffects:=True)>
    Public Function GenerarArchivoCarteraLiquidez_ACHSync(ByVal pstrRegistros As String, ByVal pstrTipoCartera As String, ByVal pstrNombreProceso As String, ByVal pstrNombreArchivo As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GeneracionCENCDeceval)
        Dim objTask As Task(Of List(Of GeneracionCENCDeceval)) = Me.GenerarArchivoCarteraLiquidez_ACHAsync(pstrRegistros, pstrTipoCartera, pstrNombreProceso, pstrNombreArchivo, pstrUsuario, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function GenerarArchivoCarteraLiquidez_ACHAsync(ByVal pstrRegistros As String, ByVal pstrTipoCartera As String, ByVal pstrNombreProceso As String, ByVal pstrNombreArchivo As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of GeneracionCENCDeceval))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of GeneracionCENCDeceval)) = New TaskCompletionSource(Of List(Of GeneracionCENCDeceval))()
        objTaskComplete.TrySetResult(GenerarArchivoCarteraLiquidez_ACH(pstrRegistros, pstrTipoCartera, pstrNombreProceso, pstrNombreArchivo, pstrUsuario, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#Region "Generación de Recibos de caja con datos Deceval"


    'Declaración de Variables para Parametros de Salida.
    Private obTesoreriaEncabezado As New Tesoreri

    Public Function GetListaDetalleTesorerias(ByVal Pagina As Integer, ByVal RegistroPorPagina As Integer, ByVal plogDemocratizados As Nullable(Of Boolean), ByVal pstrIDEspecie As String, ByVal pintIDSucursal As Nullable(Of Integer), ByVal pstrTipo As String, ByVal plogVersionNueva As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ListaDetalleTesoreria)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            If plogVersionNueva Then
                Dim e = Me.DataContext.SpConsultarPagosDecevalOyDNet(Pagina, RegistroPorPagina, plogDemocratizados, pstrIDEspecie, pintIDSucursal, pstrTipo).ToList()
                Return e
            Else
                Dim e = Me.DataContext.SpConsultarPagosDeceval_611OyDNet(Pagina, RegistroPorPagina).ToList()
                Return e
            End If
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GetListaDetalleTesorerias")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function PagosDeceval_TotalesRegistros(ByVal plogDemocratizados As Boolean, ByVal pstrIDEspecie As String, ByVal pintSucursal As Integer, ByVal pstrTipo As String, ByVal pstrSeleccionado As String, ByVal pstrNoSeleccionados As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PagosDECEVALTotales)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e = Me.DataContext.uspOyDNet_PagosDeceval_CantidadTotal(plogDemocratizados, pstrIDEspecie, pintSucursal, pstrTipo, pstrSeleccionado, pstrNoSeleccionados, pstrUsuario, DemeInfoSesion(pstrUsuario, "PagosDeceval_TotalesRegistros"), 0).ToList()
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PagosDeceval_TotalesRegistros")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Método para generar Recibos de Caja a partir de Deceval
    ''' </summary>
    ''' <param name="pstrID"></param>
    ''' <param name="plngNroDetalles"></param>
    ''' <param name="pstrNombreConsecutivo"></param>
    ''' <param name="plngNroDocumento"></param>
    ''' <param name="pstrNombreRecibi"></param>
    ''' <param name="pdtmDocumento"></param>
    ''' <param name="pstrUsuario"></param>
    ''' <param name="pstrIdCuentaContable"></param>
    ''' <param name="pstrCCosto"></param>
    ''' <param name="pstrBancoGirador"></param>
    ''' <param name="plngNumCheque"></param>
    ''' <param name="plngBancoConsignacion"></param>
    ''' <param name="pdtmConsignacion"></param>
    ''' <param name="pstrObservaciones"></param>
    ''' <param name="pstrFormaPago"></param>
    ''' <returns>Retorna el resulta de la generación</returns>
    ''' <remarks>SLB20131125</remarks>
    <Query(HasSideEffects:=True)>
    Public Function Generar_RC_Deceval(ByVal pstrID As String, ByVal plngNroDetalles As Integer, ByVal pstrNombreConsecutivo As String, ByVal pstrTipoIdentificacion As String, ByVal plngNroDocumento As String,
                               ByVal pstrNombreRecibi As String, ByVal pdtmDocumento As DateTime, ByVal pstrUsuario As String, ByVal pstrIdCuentaContable As String,
                               ByVal pstrCCosto As String, ByVal pstrBancoGirador As String, ByVal plngNumCheque As Double?, ByVal plngBancoConsignacion As Integer?,
                               ByVal pdtmConsignacion As Date, ByVal pstrObservaciones As String, ByVal pstrFormaPago As String, ByVal plogDemocratizados As Boolean, ByVal pstrIDEspecie As String,
                               ByVal pintSucursal As Nullable(Of Integer), ByVal pstrTipo As String, ByVal pstrRegistrosEditados As String, ByVal pstrUsuarioLlamado As String, ByVal pstrInfoConexion As String) As List(Of RespuestaProcesosGenericos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuarioLlamado, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrID = HttpUtility.HtmlDecode(pstrID)
            pstrRegistrosEditados = HttpUtility.HtmlDecode(pstrRegistrosEditados)
            Dim ret = Me.DataContext.uspOyDNet_Generar_RecibosCaja_Deceval(pstrID, plngNroDetalles, pstrNombreConsecutivo, pstrTipoIdentificacion, plngNroDocumento, pstrNombreRecibi, pdtmDocumento,
                                                                          pstrUsuario, pstrIdCuentaContable, pstrCCosto, pstrBancoGirador, plngNumCheque, plngBancoConsignacion,
                                                                          pdtmConsignacion, pstrObservaciones, pstrFormaPago, String.Empty, DemeInfoSesion(pstrUsuario, "Generar_RC_Deceval"), 0,
                                                                          plogDemocratizados, pstrIDEspecie, pintSucursal, pstrTipo, pstrRegistrosEditados).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Generar_RC_Deceval")
            Return Nothing
        End Try
    End Function
    <Query(HasSideEffects:=True)>
    Public Function Generar_RC_DecevalSync(ByVal pstrID As String, ByVal plngNroDetalles As Integer, ByVal pstrNombreConsecutivo As String, ByVal pstrTipoIdentificacion As String, ByVal plngNroDocumento As String,
                               ByVal pstrNombreRecibi As String, ByVal pdtmDocumento As DateTime, ByVal pstrUsuario As String, ByVal pstrIdCuentaContable As String,
                               ByVal pstrCCosto As String, ByVal pstrBancoGirador As String, ByVal plngNumCheque As Double?, ByVal plngBancoConsignacion As Integer?,
                               ByVal pdtmConsignacion As Date, ByVal pstrObservaciones As String, ByVal pstrFormaPago As String, ByVal plogDemocratizados As Boolean, ByVal pstrIDEspecie As String,
                               ByVal pintSucursal As Nullable(Of Integer), ByVal pstrTipo As String, ByVal pstrRegistrosEditados As String, ByVal pstrUsuarioLlamado As String, ByVal pstrInfoConexion As String) As List(Of RespuestaProcesosGenericos)
        Dim objTask As Task(Of List(Of RespuestaProcesosGenericos)) = Me.Generar_RC_DecevalAsync(pstrID, plngNroDetalles, pstrNombreConsecutivo, pstrTipoIdentificacion, plngNroDocumento, pstrNombreRecibi, pdtmDocumento,
                                                                          pstrUsuario, pstrIdCuentaContable, pstrCCosto, pstrBancoGirador, plngNumCheque, plngBancoConsignacion,
                                                                          pdtmConsignacion, pstrObservaciones, pstrFormaPago, plogDemocratizados, pstrIDEspecie, pintSucursal, pstrTipo, pstrRegistrosEditados, pstrUsuarioLlamado, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    <Query(HasSideEffects:=True)>
    Private Function Generar_RC_DecevalAsync(ByVal pstrID As String, ByVal plngNroDetalles As Integer, ByVal pstrNombreConsecutivo As String, ByVal pstrTipoIdentificacion As String, ByVal plngNroDocumento As String,
                               ByVal pstrNombreRecibi As String, ByVal pdtmDocumento As DateTime, ByVal pstrUsuario As String, ByVal pstrIdCuentaContable As String,
                               ByVal pstrCCosto As String, ByVal pstrBancoGirador As String, ByVal plngNumCheque As Double?, ByVal plngBancoConsignacion As Integer?,
                               ByVal pdtmConsignacion As Date, ByVal pstrObservaciones As String, ByVal pstrFormaPago As String, ByVal plogDemocratizados As Boolean, ByVal pstrIDEspecie As String,
                               ByVal pintSucursal As Nullable(Of Integer), ByVal pstrTipo As String, ByVal pstrRegistrosEditados As String, ByVal pstrUsuarioLlamado As String, ByVal pstrInfoConexion As String) As Task(Of List(Of RespuestaProcesosGenericos))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of RespuestaProcesosGenericos)) = New TaskCompletionSource(Of List(Of RespuestaProcesosGenericos))()
        objTaskComplete.TrySetResult(Generar_RC_Deceval(pstrID, plngNroDetalles, pstrNombreConsecutivo, pstrTipoIdentificacion, plngNroDocumento, pstrNombreRecibi, pdtmDocumento,
                                                                          pstrUsuario, pstrIdCuentaContable, pstrCCosto, pstrBancoGirador, plngNumCheque, plngBancoConsignacion,
                                                                          pdtmConsignacion, pstrObservaciones, pstrFormaPago, plogDemocratizados, pstrIDEspecie, pintSucursal, pstrTipo, pstrRegistrosEditados, pstrUsuarioLlamado, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function Generar_RC_Deceval611(ByVal pstrID As String, ByVal plngNroDetalles As Integer, ByVal pstrNombreConsecutivo As String, ByVal pstrTipoIdentificacion As String, ByVal plngNroDocumento As String,
                               ByVal pstrNombreRecibi As String, ByVal pdtmDocumento As DateTime, ByVal pstrUsuario As String, ByVal pstrIdCuentaContable As String,
                               ByVal pstrCCosto As String, ByVal pstrBancoGirador As String, ByVal plngNumCheque As Double?, ByVal plngBancoConsignacion As Integer?,
                               ByVal pdtmConsignacion As Date, ByVal pstrObservaciones As String, ByVal pstrFormaPago As String, ByVal pstrUsuarioLlamado As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuarioLlamado, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strResultado As String = String.Empty
            Dim ret = Me.DataContext.uspOyDNet_Generar_RecibosCaja_Deceval_611(pstrID, plngNroDetalles, pstrNombreConsecutivo, pstrTipoIdentificacion, plngNroDocumento, pstrNombreRecibi, pdtmDocumento,
                                                                          pstrUsuario, pstrIdCuentaContable, pstrCCosto, pstrBancoGirador, plngNumCheque, plngBancoConsignacion,
                                                                          pdtmConsignacion, pstrObservaciones, pstrFormaPago, strResultado, DemeInfoSesion(pstrUsuario, "Generar_RC_Deceval_611"), 0)
            Return strResultado
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Generar_RC_Deceval_611")
            Return Nothing
        End Try
    End Function

    '<ServiceModel.DomainServices.Server.Invoke()> _
    'Public Function pruebaobjeto( ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS Dictionary(Of String, String)

    '    Dim e = Me.DataContext.SpConsultarPagosDecevalOyDNet().ToList()

    '    For Each a In e
    '        contadorprueba = contadorprueba + 1
    '        pruebadiccionario.Add(contadorprueba.ToString, a.Detalle)

    '        If contadorprueba = 20000 Then
    '            Return pruebadiccionario
    '        End If
    '    Next
    '    Return pruebadiccionario
    'End Function

    Public Function GetParamEncabezado(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As EntitySet(Of OyDTesoreria.Tesoreri)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return New EntitySet(Of OyDTesoreria.Tesoreri)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GetParamEncabezado")
            Return Nothing
        End Try
    End Function

    Public Function GetPara(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OyDTesoreria.TesoreriaActualizarParam)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return New List(Of OyDTesoreria.TesoreriaActualizarParam)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GetPara")
            Return Nothing
        End Try
    End Function

    Public Function GrabarReciboCajaEncabezado(ByVal plngIdSecuencia As Integer,
                                              ByVal pstrNombreConsecutivo As String,
                                            ByVal pstrTipoIdentificacion As String,
                                            ByVal plngNroDocumento As String,
                                            ByVal pstrNombre As String,
                                            ByVal plngIDBanco As System.Nullable(Of System.Int32),
                                            ByVal plngNumCheque As System.Nullable(Of Double),
                                            ByVal pcurValor As System.Nullable(Of Decimal),
                                            ByVal pstrDetalle As String,
                                            ByVal pdtmDocumento As System.Nullable(Of Date),
                                            ByVal pstrEstado As String,
                                            ByVal pdtmEstado As System.Nullable(Of Date),
                                            ByVal plngImpresiones As Integer,
                                            ByVal pstrFormaPagoCE As String,
                                            ByVal pintNroLote As Integer,
                                            ByVal plogContabilidad As System.Nullable(Of Boolean),
                                            ByVal pdtmActualizacion As System.Nullable(Of Date),
                                            ByVal pstrUsuario As String,
                                            ByVal pstrParametroContable As String,
                                            ByVal plogImpresionFisica As System.Nullable(Of Boolean),
                                            ByVal plogMultiCliente As System.Nullable(Of Boolean),
                                            ByVal pstrCuentaCliente As String,
                                            ByVal plngCodComitente As String,
                                            ByVal pstrArchivoTransferencia As String,
                                            ByVal plngIdNumInst As System.Nullable(Of System.Int32),
                                            ByVal pstrDVP As String,
                                            ByVal pstrInstruccion As String,
                                            ByVal plngIdNroOrden As System.Nullable(Of Decimal),
                                            ByVal pstrDetalleInstruccion As String,
                                            ByVal pstrEstadoNovedadContabilidad As String,
                                            ByVal pNumeroComprobante_Contabilidad As String,
                                            ByVal pFechadecontabilizacion_Contabilidad As String,
                                            ByVal pFechaProceso_Contabilidad As System.Nullable(Of Date),
                                            ByVal pstrEstadoNovedadContabilidad_Anulada As String,
                                            ByVal pstrEstadoAutomatico As String,
                                            ByVal pNumeroLote_Contabilidad As String,
                                            ByVal pstrMontoEscrito As String,
                                            ByVal pstrTipoIntermedia As String,
                                            ByVal pstrConcepto As String,
                                            ByVal plogRecaudoDirecto As System.Nullable(Of Boolean),
                                            ByVal pdtmContabilidadEncuenta As System.Nullable(Of Date),
                                            ByVal plogSobregiro As System.Nullable(Of Boolean),
                                            ByVal pstrIdentificacionAutorizadoCheque As String,
                                            ByVal pstrNombreAutorizadoCheque As String,
                                            ByVal pdtmContabilidadENC As System.Nullable(Of Date),
                                            ByVal pintNroLoteAntENC As System.Nullable(Of System.Int32),
                                            ByVal pdtmContabilidadAntENC As System.Nullable(Of Date),
                                            ByVal plngIdSucursalBancaria As System.Nullable(Of System.Int32),
                                            ByVal pdtmCreacion As System.Nullable(Of Date),
                                            ByVal plngIDDocumento As Integer,
                                            ByVal pcurValorDetalle As System.Nullable(Of Decimal),
                                            ByVal pstrIDCuentaContable As String,
                                            ByVal pstrDetalleObs As String,
                                            ByVal pstrCentroCosto As String,
                                            ByVal pstrISIN As String,
                                            ByVal plngFungible As System.Nullable(Of Integer),
                                            ByVal plngCuentaInversionista As System.Nullable(Of Integer),
                                            ByVal pXMLDetalle As String,
                                            ByVal pXMLDetalleCheques As String,
                                            ByVal pintIDCompania As Integer,
                                            ByVal pstrTipoCuenta As String, ByVal pstrUsuarioLlamado As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuarioLlamado, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRetorno As New List(Of OyDTesoreria.Tesoreri)
            Dim p1 As Integer = 0
            Dim p2 As Integer = 0
            Dim strTipo As String = ClsConstantes.GINT_TESORERIA_RECIBO_CAJA
            Dim pintIdentTesoreria As Integer
            'Dim documento As Integer = 0

            'If plngIdSecuencia = 0 Then

            Dim e = Me.DataContext.uspOyDNet_Tesoreria_Actualizar(ClsConstantes.GINT_MAKER_AND_CHEKER_POR_APROBAR,
                                                            strTipo,
                                                            pstrNombreConsecutivo,
                                                            p1,
                                                            pstrTipoIdentificacion,
                                                            plngNroDocumento,
                                                            pstrNombre,
                                                            plngIDBanco,
                                                            plngNumCheque,
                                                            pcurValor,
                                                            pstrDetalle,
                                                            pdtmDocumento,
                                                            pstrEstado,
                                                            pdtmEstado,
                                                            plngImpresiones,
                                                            pstrFormaPagoCE,
                                                            pintNroLote,
                                                            plogContabilidad,
                                                            pdtmActualizacion,
                                                            pstrUsuario,
                                                            pstrParametroContable,
                                                            plogImpresionFisica,
                                                            plogMultiCliente,
                                                            pstrCuentaCliente,
                                                            plngCodComitente,
                                                            pstrArchivoTransferencia,
                                                            plngIdNumInst,
                                                            pstrDVP,
                                                            pstrInstruccion,
                                                            plngIdNroOrden,
                                                            pstrDetalleInstruccion,
                                                            pstrEstadoNovedadContabilidad,
                                                            pNumeroComprobante_Contabilidad,
                                                            pFechadecontabilizacion_Contabilidad,
                                                            pFechaProceso_Contabilidad,
                                                            pstrEstadoNovedadContabilidad_Anulada,
                                                            pstrEstadoAutomatico,
                                                            pNumeroLote_Contabilidad,
                                                            pstrMontoEscrito,
                                                            pstrTipoIntermedia,
                                                            pstrConcepto,
                                                            plogRecaudoDirecto,
                                                            pdtmContabilidadEncuenta,
                                                            plogSobregiro,
                                                            pstrIdentificacionAutorizadoCheque,
                                                            pstrNombreAutorizadoCheque,
                                                            p2,
                                                            pdtmContabilidadENC,
                                                            pintNroLoteAntENC,
                                                            pdtmContabilidadAntENC,
                                                            plngIdSucursalBancaria,
                                                            pdtmCreacion,
                                                            0,
                                                            False,
                                                            vbNull,
                                                            False,
                                                            False, "",
                                                            pstrTipoCuenta,
                                                            pXMLDetalle,
                                                            pXMLDetalleCheques,
                                                            pintIDCompania,
                                                            DemeInfoSesion(pstrUsuario, "GrabarReciboCajaEncabezado"),
                                                            0)
            plngIDDocumento = p1
            pintIdentTesoreria = p2

            'End If

            'GrabarReciboCajaDetalle(pstrNombreConsecutivo, _
            '                                  plngIDDocumento, _
            '                                 plngIdSecuencia, _
            '                                 "", _
            '                                 pcurValorDetalle, _
            '                                 pstrIDCuentaContable, _
            '                                 pstrDetalleObs, _
            '                                 plngIDBanco, _
            '                                 CStr(plngNroDocumento), _
            '                                 pstrCentroCosto, _
            '                                 Now.Date, _
            '                                 pstrUsuario, _
            '                                 Nothing, _
            '                                 Nothing, _
            '                                 Nothing, _
            '                                 Nothing, _
            '                                 Nothing, _
            '                                 Nothing, _
            '                                 Nothing, _
            '                                 Nothing, _
            '                                 Nothing, _
            '                                 Nothing, _
            '                                 Nothing, _
            '                                 Nothing, _
            '                                 Nothing, _
            '                                 Nothing, _
            '                                 Nothing, _
            '                                 Nothing, _
            '                                 Nothing,
            '                                 pstrISIN, _
            '                                 plngFungible, _
            '                                 plngCuentaInversionista)


            'objRetorno.Add(New OyDTesoreria.Tesoreri With {.IDDocumento = plngIDDocumento, .NroLote = plngIdSecuencia})


            'Return objRetorno

            Return plngIDDocumento.ToString + "," + pintIdentTesoreria.ToString

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GrabarReciboCajaEncabezado")
            Return Nothing
        End Try


    End Function

    Public Function GrabarReciboCajaDetalle(ByVal pstrNombreConsecutivo As String,
                                        ByVal plngIDDocumento As Integer,
                                        ByVal plngSecuencia As Integer,
                                        ByVal plngIDComitente As String,
                                        ByVal pcurValor As System.Nullable(Of Decimal),
                                        ByVal pstrIDCuentaContable As String,
                                        ByVal pstrDetalle As String,
                                        ByVal plngIDBanco As System.Nullable(Of System.Int32),
                                        ByVal pstrNIT As String,
                                        ByVal pstrCentroCosto As String,
                                        ByVal pdtmActualizacion As System.Nullable(Of Date),
                                        ByVal pstrUsuario As String,
                                        ByVal pstrEstadoTransferencia As String,
                                        ByVal plngBancoDestino As System.Nullable(Of System.Int32),
                                        ByVal pstrCuentaDestino As String,
                                        ByVal pstrTipoCuenta As String,
                                        ByVal pstrIdentificacionTitular As String,
                                        ByVal pstrTitular As String,
                                        ByVal pstrTipoIdTitular As String,
                                        ByVal pstrFormaEntrega As String,
                                        ByVal pstrBeneficiario As String,
                                        ByVal pstrTipoIdentBeneficiario As String,
                                        ByVal pstrIdentificacionBenficiciario As String,
                                        ByVal pstrNombrePersonaRecoge As String,
                                        ByVal pstrIdentificacionPerRecoge As String,
                                        ByVal pstrOficinaEntrega As String,
                                        ByVal pstrNombreConsecutivoNotaGMF As String,
                                        ByVal plngIDDocumentoNotaGMF As System.Nullable(Of System.Int32),
                                        ByVal pintExportados As System.Nullable(Of System.Int32),
                                        ByVal pstrISIN As String,
                                        ByVal plngFungible As System.Nullable(Of Integer),
                                        ByVal plngCuentaInversionista As System.Nullable(Of Integer),
                                        ByVal pstrOpcion As String,
                                        ByVal pintNroRegistro As Integer,
                                        ByVal intidentity As Integer,
                                        ByVal pStrOficinaCuentaInversion As String,
                                        ByVal pStrNombreCarteraColectiva As String,
                                        ByVal pStrNombreAsesor As String,
                                        ByVal pstrCodigoCartera As String,
                                        ByVal pstrUsuarioLlamado As String,
                                        ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuarioLlamado, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strTipo As String = ClsConstantes.GINT_TESORERIA_RECIBO_CAJA
            Dim p1 As Integer = 0
            Dim e = Me.DataContext.uspOyDNet_DetalleTesoreria_Actualizar(ClsConstantes.GINT_MAKER_AND_CHEKER_POR_APROBAR,
                                                                    strTipo,
                                                                    pstrNombreConsecutivo,
                                                                    plngIDDocumento,
                                                                    plngSecuencia,
                                                                    plngIDComitente,
                                                                    pcurValor,
                                                                    pstrIDCuentaContable,
                                                                    pstrDetalle,
                                                                    plngIDBanco,
                                                                    pstrNIT,
                                                                    pstrCentroCosto,
                                                                    pdtmActualizacion,
                                                                    pstrUsuario,
                                                                    pstrEstadoTransferencia,
                                                                    plngBancoDestino,
                                                                    pstrCuentaDestino,
                                                                    pstrTipoCuenta,
                                                                    pstrIdentificacionTitular,
                                                                    pstrTitular,
                                                                    pstrTipoIdTitular,
                                                                    pstrFormaEntrega,
                                                                    pstrBeneficiario,
                                                                    pstrTipoIdentBeneficiario,
                                                                    pstrIdentificacionBenficiciario,
                                                                    pstrNombrePersonaRecoge,
                                                                    pstrIdentificacionPerRecoge,
                                                                    pstrOficinaEntrega,
                                                                    p1,
                                                                    pstrNombreConsecutivoNotaGMF,
                                                                    plngIDDocumentoNotaGMF,
                                                                    Nothing,
                                                                    -1,
                                                                    pintExportados,
                                                                    intidentity,
                                                                    pStrOficinaCuentaInversion,
                                                                    pStrNombreCarteraColectiva,
                                                                    pStrNombreAsesor,
                                                                    pstrCodigoCartera,
                                                                    DemeInfoSesion(pstrUsuario, "GrabarReciboCajaDetalle"),
                                                                    ClsConstantes.GINT_ErrorPersonalizado)
            If pstrOpcion = String.Empty Then
                Call BorrarPagoDeceval(pstrISIN, plngFungible, plngCuentaInversionista, pstrNombreConsecutivo, plngIDDocumento, plngSecuencia, pstrUsuario)
            Else
                Call BorrarPagoDCV(pintNroRegistro, pstrNombreConsecutivo, plngIDDocumento, plngSecuencia, pstrUsuario)
            End If

            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GrabarReciboCajaDetalle")
            Return Nothing
        End Try
    End Function

    Public Function GrabarChequePagoDeceval(ByVal pstrNombreConsecutivo As String,
                                            ByVal plngIDDocumento As Integer,
                                            ByVal plngSecuencia As Integer,
                                            ByVal plogEfectivo As Boolean,
                                            ByVal pstrBancoGirador As String,
                                            ByVal plngNumCheque As System.Nullable(Of Double),
                                            ByVal pcurValor As System.Nullable(Of Decimal),
                                            ByVal plngBancoConsignacion As System.Nullable(Of System.Int32),
                                            ByVal pdtmConsignacion As System.Nullable(Of Date),
                                            ByVal pstrUsuario As String,
                                            ByVal pstrObservaciones As String,
                                            ByVal pstrFormaPago As String,
                                            ByVal intIdidentity As Integer,
                                            ByVal pstrUsuarioLlamado As String,
                                            ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuarioLlamado, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objCheque As New Cheque
            Dim Id As Integer = 0

            objCheque.Aprobacion = ClsConstantes.GINT_MAKER_AND_CHEKER_POR_APROBAR

            objCheque.NombreConsecutivo = pstrNombreConsecutivo
            objCheque.IDDocumento = plngIDDocumento
            objCheque.Secuencia = plngSecuencia
            objCheque.Efectivo = plogEfectivo
            objCheque.BancoGirador = pstrBancoGirador
            objCheque.NumCheque = plngNumCheque
            objCheque.Valor = CDec(pcurValor)
            objCheque.BancoConsignacion = plngBancoConsignacion
            objCheque.Consignacion = pdtmConsignacion
            objCheque.FormaPagoRC = pstrFormaPago ' ClsConstantes.FORMA_PAGO_RECIBO_CAJA_PAGO_DECEVAL
            objCheque.Actualizacion = Now.Date
            objCheque.Usuario = pstrUsuario
            objCheque.Comentario = pstrObservaciones
            objCheque.Estado = ClsConstantes.DESC_ESTADO_1_INGRESADO_PENDIENTE_POR_APROBACION

            Dim e = Me.DataContext.uspOyDNet_ChequesTesoreria_Actualizar(objCheque.Aprobacion,
                                                                    objCheque.NombreConsecutivo,
                                                                    objCheque.IDDocumento,
                                                                    objCheque.Secuencia,
                                                                    objCheque.Efectivo,
                                                                    objCheque.BancoGirador,
                                                                    objCheque.NumCheque,
                                                                    objCheque.Valor,
                                                                    objCheque.BancoConsignacion,
                                                                    objCheque.Consignacion,
                                                                    objCheque.FormaPagoRC,
                                                                    objCheque.Actualizacion,
                                                                    objCheque.Usuario,
                                                                    objCheque.Comentario,
                                                                    objCheque.IdProducto,
                                                                    objCheque.SucursalesBancolombia,
                                                                    Id,
                                                                    objCheque.ChequeHizoCanje,
                                                                    intIdidentity,
                                                                    DemeInfoSesion(pstrUsuario, "GrabarChequePagoDeceval"),
                                                                    ClsConstantes.GINT_ErrorPersonalizado)
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GrabarChequePagoDeceval")
            Return Nothing
        End Try

    End Function


    Public Sub BorrarPagoDeceval(ByVal pstrISIN As String,
                                    ByVal plngFungible As System.Nullable(Of Integer),
                                    ByVal plngCuentaInversionista As System.Nullable(Of Integer),
                                    ByVal strNombreConsecutivo As String,
                                    ByVal lngIDDocumento As System.Nullable(Of Integer),
                                    ByVal lngSecuencia As System.Nullable(Of Integer),
                                    ByVal pstrUsuario As String)

        Try
            Me.DataContext.uspOyDNet_Tesoreria_BorrarPagosDeceval(pstrISIN, plngFungible, plngCuentaInversionista, strNombreConsecutivo, lngIDDocumento, lngSecuencia, pstrUsuario, DemeInfoSesion(pstrUsuario, "BorrarPagoDeceval"), ClsConstantes.GINT_ErrorPersonalizado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BorrarPagoDeceval")
        End Try

    End Sub
    Public Sub BorrarPagoDCV(ByVal pIntIdNroRegistro As System.Nullable(Of Integer),
                                   ByVal pstrNombreConsecutivo As String,
                                   ByVal lngIDDocumento As System.Nullable(Of Integer),
                                   ByVal lngSecuencia As System.Nullable(Of Integer),
                                   ByVal pstrUsuario As String)

        Try
            Me.DataContext.usp_tblPagosDCV_BorrarMvto_CrearHistorico_OyDNet(pIntIdNroRegistro, pstrNombreConsecutivo, lngIDDocumento, lngSecuencia, pstrUsuario, DemeInfoSesion(pstrUsuario, "BorrarPagoDeceval"), ClsConstantes.GINT_ErrorPersonalizado)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "BorrarPagoDeceval")
        End Try

    End Sub


#End Region

#Region "Plano Comprobantes de Egreso"

    Public Function GetListaComprobantesEgreso(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ComEgresosSeleccionar)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return Me.DataContext.uspOyDNet_Tesoreria_ComprobantesEgreso_Filtrar(DemeInfoSesion(pstrUsuario, "GetListaComprobantesEgreso"), ClsConstantes.GINT_ErrorPersonalizado).ToList()
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GetListaComprobantesEgreso")
            Return Nothing
        End Try
    End Function

    Public Sub UpdateComEgresosSeleccionar(ByVal comEgresoSeleccionar As ComEgresosSeleccionar)
    End Sub

    Public Function GenerarComprobantesDeEgreso(ByVal pComprobantes As String, ByVal pConsecutivos As String, ByVal pSeparador As String, ByVal NombreArchivo As String, ByVal NombreProceso As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean

        ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
        Dim ListaResultado As List(Of PlanoComprobantesEgreso) = Me.DataContext.uspOyDNet_Tesoreria_PlanoComprobantes_Generar(DemeInfoSesion(pstrUsuario, "GenerarComprobantesDeEgreso"), ClsConstantes.GINT_ErrorPersonalizado, pComprobantes, pConsecutivos, pSeparador).ToList()

        Dim lineas As IEnumerable(Of String)
        Dim lineasArchivo As New List(Of String)
        Dim strlineacontrol As String = String.Empty
        For Each r As PlanoComprobantesEgreso In ListaResultado
            lineas = r.Linea.Split(CType("*", Char))
            'el for recorre hasta la longitud -3 por que se debe eliminar la ultimA LINEA QUE ES  la del TRL mas la que genera el split al final
            For o = 0 To lineas.Count - 3
                If Not String.IsNullOrEmpty(lineas(o)) Then
                    lineasArchivo.Add(lineas(o))
                End If
            Next
            strlineacontrol = lineas(lineas.Count - 2)
        Next
        lineasArchivo.Add(strlineacontrol)
        NombreArchivo = DateTime.Now.ToString() & NombreArchivo
        NombreArchivo = NombreArchivo.Replace(CType("/", Char), CType("_", Char))
        NombreArchivo = NombreArchivo.Replace(CType(" ", Char), CType("_", Char))
        NombreArchivo = NombreArchivo.Replace(CType(":", Char), CType("_", Char))

        Return GuardarArchivo(NombreProceso, pstrUsuario, NombreArchivo, lineasArchivo, True)
    End Function

    Public Function ValidarPermisos(ByVal usuarios As String, ByVal modulo As String, ByVal tienepermiso As System.Nullable(Of Boolean), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Boolean
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e = Me.DataContext.uspConsultarUsuariosAutorizados_OyDNet(usuarios, modulo, tienepermiso, (DemeInfoSesion(pstrUsuario, "ValidarPermisos")), 0)
            Return CBool(tienepermiso)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsutarConsecutivoBanco")
            Return Nothing
        End Try


    End Function

    Public Function LogAutorizacion(ByVal nombreconsecutivo As String, ByVal documento As Decimal, ByVal comitenete As String, ByVal detalle As String, ByVal dtmdocumento As DateTime, ByVal usuarioautoriza As String, ByVal usuarios As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e = Me.DataContext.uspInsLogAutorizaciones_OyDNet(nombreconsecutivo, documento, comitenete, detalle, dtmdocumento, usuarioautoriza, usuarios, (DemeInfoSesion(pstrUsuario, "LogAutorizacion")), 0)
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "LogAutorizacion")
            Return Nothing
        End Try


    End Function
#End Region

#Region "Generar Notas de Tesoreria (Cancelacion saldos)."
    Public Function GetListaCancelaSaldos(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ListaArchivoCancelaSaldo)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return Me.DataContext.ListaArchivoCancelaSaldos.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GetListaCancelaSaldos")
            Return Nothing
        End Try
    End Function

    Public Function GetNotaContable_HeadDetail(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of NotaContable_HeadDetail)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Return Me.DataContext.NotaContable_HeadDetail.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GetNotaContable_HeadDetail")
            Return Nothing
        End Try
    End Function

    Public Function CargarArchivoCancelaSaldo(ByVal pstrNombreCompletoArchivo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ListaArchivoCancelaSaldo)

        Dim objReader As System.IO.StreamReader = Nothing
        Dim onjListaRetorno As New List(Of ListaArchivoCancelaSaldo)


        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim strVariable As String = String.Empty
            Dim curDebito As Decimal = 0
            Dim curCredito As Decimal = 0
            Dim curVlrPorCumplir As Decimal = 0

            FileOpen(1, pstrNombreCompletoArchivo, OpenMode.Input)

            While CInt(EOF(1)) = 0

                Dim objDato As New ListaArchivoCancelaSaldo

                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, objDato.CodigoCliente)
                Input(1, objDato.Cliente)
                Input(1, curDebito)
                Input(1, curCredito)
                Input(1, curVlrPorCumplir)
                Input(1, objDato.Naturaleza)
                Input(1, objDato.Receptor)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)
                Input(1, strVariable)

                If curDebito <> 0 Then objDato.Debito = curDebito
                If curCredito <> 0 Then objDato.Credito = curCredito
                If curVlrPorCumplir <> 0 Then objDato.ValorPorCumplir = curVlrPorCumplir

                onjListaRetorno.Add(objDato)

            End While

            FileClose(1)

            Return onjListaRetorno

        Catch ex As Exception
            FileClose(1)
            If Not objReader Is Nothing Then objReader.Close()
            ManejarError(ex, Me.ToString(), "CargarArchivoCancelaSaldo")
            Return Nothing
        End Try
    End Function

    Public Function SalvarDatosNotaContable(ByVal pstrTipo As String,
                                            ByVal pstrNombreConsecutivo As String,
                                            ByVal lngIdDocumento As System.Nullable(Of Integer),
                                            ByVal plngSecuencia As System.Nullable(Of Integer),
                                            ByVal plngComitente As String,
                                            ByVal pcurValor As System.Nullable(Of Decimal),
                                            ByVal pstrIdCuentaContable As String,
                                            ByVal pstrDetalle As String,
                                            ByVal plngIDBanco As System.Nullable(Of Integer),
                                            ByVal pstrNIT As String,
                                            ByVal pstrCCosto As String,
                                            ByVal pintNroDetalle As System.Nullable(Of Integer),
                                            ByVal pdtmDocumento As System.Nullable(Of Date),
                                            ByVal plogContabilidad As System.Nullable(Of Boolean),
                                            ByVal plngConsec As System.Nullable(Of Integer),
                                            ByVal pstrEstadoAprobacion As String,
                                            ByVal pstrEstado As String,
                                            ByVal pdtmCreacion As System.Nullable(Of Date),
                                            ByVal pstrUsuario As String, ByVal pstrUsuarioLlamado As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Integer)
        Dim intReturn As Integer = 0
        Dim lngConsecutivo As System.Nullable(Of Integer) = 0

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString, pstrUsuarioLlamado, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim strCodigo As String = String.Empty
            Dim strTipoIdentificacion As String = String.Empty

            strTipoIdentificacion = CampoTabla(plngComitente, "strTipoIdentificacion", "tblClientes", "lngID")

            If strTipoIdentificacion <> "N" Then
                pstrNIT = plngComitente
            Else
                pstrNIT = Mid(plngComitente, 1, plngComitente.Length - 1)
            End If

            pstrEstadoAprobacion = ClsConstantes.DESC_ESTADO_1_INGRESADO_PENDIENTE_POR_APROBACION


            intReturn = Me.DataContext.uspOyDNet_Tesoreria_InsNotaContableYDetalle(pstrTipo,
                                                                                    pstrNombreConsecutivo,
                                                                                    lngIdDocumento,
                                                                                    plngSecuencia,
                                                                                    plngComitente,
                                                                                    pcurValor,
                                                                                    pstrIdCuentaContable,
                                                                                    pstrDetalle,
                                                                                    plngIDBanco,
                                                                                    pstrNIT,
                                                                                    pstrCCosto,
                                                                                    pintNroDetalle,
                                                                                    pdtmDocumento,
                                                                                    plogContabilidad,
                                                                                    plngConsec,
                                                                                    pstrEstadoAprobacion,
                                                                                    pstrEstado,
                                                                                    pdtmCreacion,
                                                                                    pstrUsuario,
                                                                                    DemeInfoSesion(pstrUsuario, "SalvarDatosNotaContable"),
                                                                                    ClsConstantes.GINT_ErrorPersonalizado,
                                                                                    lngConsecutivo)
            Return lngConsecutivo
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "SalvarDatosNotaContable")
            Return Nothing
        End Try
    End Function

#End Region

#Region "COMUNES"
    Public Function GetAuditorias(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As IQueryable(Of OyDTesoreria.Auditoria)
        Dim objResultado As IQueryable(Of OyDTesoreria.Auditoria) = Nothing
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            objResultado = Me.DataContext.Auditorias
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GetAuditorias")
        End Try
        Return objResultado
    End Function

    Public Function CargarCombos(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OyDTesoreria.ItemCombo)
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

#Region "OrdenesTesoreria"

    Public Function OrdenesTesoreriaEditar(ByVal IDOrdenTesoreria As Integer,
                                           ByVal CodCliente As String,
                                           ByVal NombreCliente As String,
                                           ByVal Tipo As String,
                                           ByVal ConsecutivoConsignacion As String,
                                           ByVal Detalle As String,
                                           ByVal ValorSaldo As Decimal,
                                           ByVal Beneficiario As String,
                                           ByVal IDBancoGirador As Integer,
                                           ByVal NroCheque As Double,
                                           ByVal FechaConsignacion As DateTime,
                                           ByVal IDBancoRec As Integer,
                                           ByVal FormaPago As String,
                                           ByVal CtaContable As String,
                                           ByVal CtaContableContraP As String,
                                           ByVal EstadoImpresion As Integer,
                                           ByVal Procesado As Integer,
                                           ByVal Notas As String,
                                           ByVal TipoSello As String,
                                           ByVal CreacionOrden As DateTime,
                                           ByVal Login As String,
                                           ByVal Actualizacion As DateTime, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_OrdenesTesoreria_Actualizar(IDOrdenTesoreria,
                                                                                     CodCliente,
                                                                                     NombreCliente,
                                                                                     Tipo,
                                                                                     ConsecutivoConsignacion,
                                                                                     Detalle,
                                                                                     ValorSaldo,
                                                                                     Beneficiario,
                                                                                     IDBancoGirador,
                                                                                     NroCheque,
                                                                                     Now.Date,
                                                                                     IDBancoRec,
                                                                                     FormaPago,
                                                                                     CtaContable,
                                                                                     CtaContableContraP,
                                                                                     EstadoImpresion,
                                                                                     Procesado,
                                                                                     Notas,
                                                                                     TipoSello,
                                                                                     CreacionOrden,
                                                                                     Login,
                                                                                     Now.Date, DemeInfoSesion(pstrUsuario, ""), 0)







            'Dim ret = Me.DataContext.uspOyDNet_Maestros_Tarifas_Eliminar(Aprobacion, pNombre, pUsuario, pID, mensaje, pIDTarifas, DemeInfoSesion(pstrUsuario, "EliminarPaises"), 0)
            Return CStr(IDOrdenTesoreria)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OrdenesTesoreriaEditar")
            Return Nothing
        End Try
    End Function

    'Public Function OrdenesTesoreriaFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS List(Of OrdenesTesoreri)
    '    Try
    '        Dim ret = Me.DataContext.uspOyDNet_Tesoreria_OrdenesTesoreria_Filtrar(pstrFiltro, DemeInfoSesion(pstrUsuario, "OrdenesTesoreriaFiltrar"), 0).ToList
    '        Return ret
    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "OrdenesTesoreriaFiltrar")
    '        Return Nothing
    '    End Try
    'End Function

    'Public Function OrdenesTesoreriaConsultar(ByVal pIDOrdenTesoreria As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS List(Of OrdenesTesoreri)
    '    Try
    '        Dim ret = Me.DataContext.uspOyDNet_Tesoreria_OrdenesTesoreria_Consultar(pIDOrdenTesoreria, DemeInfoSesion(pstrUsuario, "BuscarOrdenesTesoreria"), 0).ToList
    '        Return ret
    '        Exit Function
    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "BuscarOrdenesTesoreria")
    '        Return Nothing
    '    End Try
    'End Function

    'Public Function TraerOrdenesTesoreriPorDefecto( ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) AS OrdenesTesoreri
    '    Try
    '        Dim e As New OrdenesTesoreri
    '        'e.IdComisionista = 
    '        'e.IdSucComisionista = 
    '        'e.IDOrdenTesoreria = 
    '        'e.CodCliente = 
    '        'e.NombreCliente = 
    '        'e.Tipo = 
    '        'e.ConsecutivoConsignacion = 
    '        'e.Detalle = 
    '        'e.ValorSaldo = 
    '        'e.Beneficiario = 
    '        'e.IDBancoGirador = 
    '        'e.NroCheque = 
    '        'e.FechaConsignacion = 
    '        'e.IDBancoRec = 
    '        'e.FormaPago = 
    '        'e.CtaContable = 
    '        'e.CtaContableContraP = 
    '        'e.EstadoImpresion = 
    '        'e.Procesado = 
    '        'e.Notas = 
    '        'e.TipoSello = 
    '        'e.CreacionOrden = 
    '        'e.Login = 
    '        'e.FechaActualizacion = 
    '        Return e
    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "TraerOrdenesTesoreriPorDefecto")
    '        Return Nothing
    '    End Try
    'End Function

    'Public Sub InsertOrdenesTesoreri(ByVal OrdenesTesoreri As OrdenesTesoreri)
    '    Try
    '        OrdenesTesoreri.InfoSesion = DemeInfoSesion(pstrUsuario, "InsertOrdenesTesoreri")
    '        Me.DataContext.OrdenesTesoreria.InsertOnSubmit(OrdenesTesoreri)
    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "InsertOrdenesTesoreri")
    '    End Try
    'End Sub

    'Public Sub UpdateOrdenesTesoreri(ByVal currentOrdenesTesoreri As OrdenesTesoreri)
    '    Try
    '        currentOrdenesTesoreri.InfoSesion = DemeInfoSesion(pstrUsuario, "UpdateOrdenesTesoreri")
    '        Me.DataContext.OrdenesTesoreria.Attach(currentOrdenesTesoreri, Me.ChangeSet.GetOriginal(currentOrdenesTesoreri))
    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "UpdateOrdenesTesoreri")
    '    End Try
    'End Sub

    'Public Sub DeleteOrdenesTesoreri(ByVal OrdenesTesoreri As OrdenesTesoreri)
    '    Try
    '        'Me.DataContext.uspOyDNet_Tesoreria_OrdenesTesoreria_Eliminar( pIDOrdenTesoreria, DemeInfoSesion(pstrUsuario, "DeleteOrdenesTesoreri"),0).ToList
    '        OrdenesTesoreri.InfoSesion = DemeInfoSesion(pstrUsuario, "DeleteOrdenesTesoreri")
    '        Me.DataContext.OrdenesTesoreria.Attach(OrdenesTesoreri)
    '        Me.DataContext.OrdenesTesoreria.DeleteOnSubmit(OrdenesTesoreri)
    '    Catch ex As Exception
    '        ManejarError(ex, Me.ToString(), "DeleteOrdenesTesoreri")
    '    End Try
    'End Sub
#End Region

#Region "Pendientes Tesorería"
    Public Sub InsertPendientesTesoreria(ByVal PendientesTesoreri As PendientesTesoreria)
    End Sub

    Public Sub UpdatePendientesTesoreria(ByVal currentPendientesTesoreri As PendientesTesoreria)
    End Sub

    Public Function PendientesTesoreriaFiltrar(ByVal pstrNombreConsecutivo As String, ByVal pstrFormaPago As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of PendientesTesoreria)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_PendientesTesoreria_Filtrar(pstrNombreConsecutivo, pstrFormaPago, DemeInfoSesion(pstrUsuario, "PendientesTesoreriaFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PendientesTesoreriaFiltrar")
            Return Nothing
        End Try
    End Function
    <Query(HasSideEffects:=True)>
    Public Function AprobarDesaprobarDocumentosTesoreria(ByVal pstrRegistros As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaProcesosGenericosConfirmacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_AprobarDesAprobarDocumentos(pstrRegistros, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "AprobarDesaprobarDocumentosTesoreria"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AprobarDesaprobarDocumentosTesoreria")
            Return Nothing
        End Try
    End Function
    Public Function InsertarComprobantes(ByVal pstrConsecutivo As String,
                                         ByVal plngIDDocumento As Integer,
                                         ByVal pstrAccion As String,
                                         ByVal plogSobregiro As Boolean, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspAprobarCompEgreso_OyDNet(pstrConsecutivo,
                                                                 plngIDDocumento,
                                                                 pstrAccion,
                                                                 plogSobregiro, 0, False)
            Return CStr(plngIDDocumento)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertarComprobantes")
            Return Nothing
        End Try
    End Function

    Public Function InsertarNotas(ByVal plngIDDocumento As Integer,
                                  ByVal pstrNombreConsecutivo As String,
                                  ByVal pstrAccion As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_AprobarNotaGMF_OyDNet(plngIDDocumento,
                                                        pstrNombreConsecutivo,
                                                        pstrAccion)
            Return CStr(plngIDDocumento)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertarNotas")
            Return Nothing
        End Try
    End Function

    Public Function OrdenesPendientesTesoreria_Consultar(ByVal pstrTipo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of OrdenesTesoreri)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_OrdenesTesoreriaPendientes_Consultar(pstrTipo, DemeInfoSesion(pstrUsuario, "PendientesTesoreriaFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "OrdenesPendientesTesoreria_Consultar")
            Return Nothing
        End Try
    End Function


#End Region

#Region "AjustesBancarios"
    Public Function AjustesBancariosFiltrar(ByVal pstrFiltro As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of AjustesBancario)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_AjustesBancarios_Filtrar(pstrFiltro, pstrUsuario, DemeInfoSesion(pstrUsuario, "AjustesBancariosFiltrar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AjustesBancariosFiltrar")
            Return Nothing
        End Try
    End Function
    Public Function AjustesBancariosConsultar(ByVal pstrTipo As String, ByVal pstrNombreConsecutivo As String, ByVal plngIDDocumento As Integer, ByVal pdtmDocumento As DateTime, ByVal plngIDBanco As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of AjustesBancario)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_AjustesBancarios_Consultar(pstrTipo, pstrNombreConsecutivo, plngIDDocumento, pdtmDocumento, plngIDBanco, DemeInfoSesion(pstrUsuario, "AjustesBancariosConsultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AjustesBancariosConsultar")
            Return Nothing
        End Try
    End Function
    Public Function Traer_AjustesBancariosPorDefecto(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As AjustesBancario
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim e As New AjustesBancario
            'e.IDComisionista = 
            'e.IDSucComisionista = 
            'e.IDBanco = 
            'e.IdBancoNacional = 
            'e.Actualizacion = 
            'e.Usuario = 
            e.IDTesoreria = -1
            Return e
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_AjustesBancariosPorDefecto")
            Return Nothing
        End Try
    End Function
    Public Sub Insert_AjustesBancarios(ByVal AjustesBancarios As AjustesBancario)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,AjustesBancarios.pstrUsuarioConexion, AjustesBancarios.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            AjustesBancarios.InfoSesion = DemeInfoSesion(AjustesBancarios.pstrUsuarioConexion, "Insert_AjustesBancarios")
            Me.DataContext.AjustesBancario.InsertOnSubmit(AjustesBancarios)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Insert_AjustesBancarios")
        End Try
    End Sub

    Public Sub Update_AjustesBancarios(ByVal currentAjustesBancarios As AjustesBancario)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentAjustesBancarios.pstrUsuarioConexion, currentAjustesBancarios.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentAjustesBancarios.InfoSesion = DemeInfoSesion(currentAjustesBancarios.pstrUsuarioConexion, "Update_AjustesBancarios")
            Me.DataContext.AjustesBancario.Attach(currentAjustesBancarios, Me.ChangeSet.GetOriginal(currentAjustesBancarios))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Update_AjustesBancarios")
        End Try
    End Sub

#End Region

#Region "Entrega de Cheques"

    Public Function ComprobanteEgreso_ControlCheques_Consultar(ByVal pdtmFechaInicio As DateTime?, ByVal pdtmFechaFin As DateTime?,
                                                               ByVal pstrNombreConsecutivo As String, ByVal pintEntregado As System.Nullable(Of Byte), ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of ChequesSinEntrega)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_OyD_ComprobanteEgreso_ControlCheques_Consultar_OyDNet(pdtmFechaInicio, pdtmFechaFin, pstrNombreConsecutivo _
                                                                                               , pintEntregado, DemeInfoSesion(pstrUsuario, "ComprobanteEgreso_ControlCheques_Consultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ComprobanteEgreso_ControlCheques_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function ComprobanteEgreso_ControlCheques_Ingresar(plngIDDocumento As Integer, pstrNombreConsecutivo As String, pstrTipo As String _
                                                                     , plngNumCheque As Double, plogChequeEntregado As Boolean, pdtmFechaEntrega As DateTime _
                                                                     , pstrRecibidoPor As String, pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = (Me.DataContext.usp_OyD_ComprobanteEgreso_ControlCheques_Ingresar_OyDNet(plngIDDocumento, pstrNombreConsecutivo, pstrTipo _
                                                                                              , plngNumCheque, plogChequeEntregado, pdtmFechaEntrega _
                                                                                              , pstrRecibidoPor, pstrUsuario, Date.Now, DemeInfoSesion(pstrUsuario, "ComprobanteEgreso_ControlCheques_Ingresar"), 0))
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ComprobanteEgreso_ControlCheques_Ingresar")
            Return Nothing
        End Try
    End Function

    Public Sub Update_ChequesSinEntrega(ByVal currentChequesSinEntrega As ChequesSinEntrega)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentChequesSinEntrega.pstrUsuarioConexion, currentChequesSinEntrega.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.ChequesSinEntregar.Attach(currentChequesSinEntrega, Me.ChangeSet.GetOriginal(currentChequesSinEntrega))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Update_ChequesSinEntrega")
        End Try
    End Sub


#End Region

#Region "Liquidaciones Dividendos TTV"
    <Query(HasSideEffects:=True)>
    Public Function LiquidacionTTV_Procesar(ByVal pstrXML As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidacionesTesoreria)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strxmldecodificado = HttpUtility.HtmlDecode(pstrXML)
            Dim ret = Me.DataContext.usp_LiquidacionesTTV_Procesar_OyDNet(strxmldecodificado, pstrUsuario, DemeInfoSesion(pstrUsuario, "LiquidacionTTV_Procesar"), 0).ToList

            Return ret

        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "LiquidacionTTV_Procesar")
            Return Nothing
        End Try
    End Function


    Public Function LiquidacionTTV_Consultar(ByVal pdtmFecha As DateTime?, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of LiquidacionTTV)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_ConsultarLiquidacionesTTV_OyDNet(pdtmFecha, DemeInfoSesion(pstrUsuario, "LiquidacionTTV_Consultar"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "LiquidacionTTV_Consultar")
            Return Nothing
        End Try
    End Function

    Public Sub Update_LiquidacionTTV(ByVal currentLiquidacionTTV As LiquidacionTTV)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentLiquidacionTTV.pstrUsuarioConexion, currentLiquidacionTTV.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            currentLiquidacionTTV.InfoSesion = DemeInfoSesion(currentLiquidacionTTV.pstrUsuarioConexion, "Update_LiquidacionTTV")
            Me.DataContext.LiquidacionTTVs.Attach(currentLiquidacionTTV, Me.ChangeSet.GetOriginal(currentLiquidacionTTV))
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Update_LiquidacionTTV")
        End Try
    End Sub

    Public Sub InsertLiquidacionTTV(ByVal LiquidacionTTV As LiquidacionTTV)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,LiquidacionTTV.pstrUsuarioConexion, LiquidacionTTV.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            LiquidacionTTV.InfoSesion = DemeInfoSesion(LiquidacionTTV.pstrUsuarioConexion, "InsertLiquidacionTTV")
            Me.DataContext.LiquidacionTTVs.InsertOnSubmit(LiquidacionTTV)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertLiquidacionTTV")
        End Try
    End Sub


    Public Sub DeleteLiquidacionTTV(ByVal LiquidacionTTV As LiquidacionTTV)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,LiquidacionTTV.pstrUsuarioConexion, LiquidacionTTV.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            LiquidacionTTV.InfoSesion = DemeInfoSesion(LiquidacionTTV.pstrUsuarioConexion, "DeleteLiquidacionTTV")
            Me.DataContext.LiquidacionTTVs.Attach(LiquidacionTTV)
            Me.DataContext.LiquidacionTTVs.DeleteOnSubmit(LiquidacionTTV)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeleteLiquidacionTTV")

        End Try
    End Sub


#End Region

#Region "Admon Facturas a Firmas Contraparte"
    ''' <summary>
    ''' Funcion para  listar Encabezado pantalla
    ''' </summary>
    ''' <remarks>TraerAdmonFacturasFirmasContraparte()
    ''' Se encarga de    : realizar la consulta y traer los registros
    ''' Creado por   : Juan David Osorio Legarda.
    ''' Fecha            : Junio 2013
    ''' Pruebas CB       : Juan David Osorio Legarda.-  Junio 2013- Resultado Ok    
    ''' </remarks>
    Public Function TraerAdmonFacturasFirmasContraparte(plngID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of AdmonFacturasFirmasContraparte)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_ConsultarFacturasClienteCompradorTTV_OyDNet(plngID, pstrUsuario, DemeInfoSesion(pstrUsuario, "TraerAdmonFacturasFirmasContraparte"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerAdmonFacturasFirmasContraparte")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Funcion para  listar Detalle pantalla
    ''' </summary>
    ''' <remarks>TraerAdmonFacturasFirmasContraparteDETALLE()
    ''' Se encarga de    : realizar la consulta y traer los registros
    ''' Creado por   : Juan David Osorio Legarda.
    ''' Fecha            : Junio 2013
    ''' Pruebas CB       : Juan David Osorio Legarda.-  Junio 2013- Resultado Ok    
    ''' </remarks>
    Public Function TraerAdmonFacturasFirmasContraparteDETALLE(plngID As Integer, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of AdmonFacturasFirmasContraparteDetalle)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_ConsultarDetalleFactura_TTV_OyDNet(plngID, Nothing, Nothing, pstrUsuario, DemeInfoSesion(pstrUsuario, "TraerAdmonFacturasFirmasContraparteDETALLE"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TraerAdmonFacturasFirmasContraparteDETALLE")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Funcion para Actualizar y generar el recibo de caja y la nota contable
    ''' </summary>
    ''' <remarks>ActualizarAdmonFacturasFirmasContraparte()
    ''' Se encarga de    : realizar la actualizacion generando registros de tesoreria
    ''' Creado por   : Juan David Osorio Legarda.
    ''' Fecha            : Junio 2013
    ''' Pruebas CB       : Juan David Osorio Legarda.-  Junio 2013- Resultado Ok    
    ''' </remarks>
    Public Function ActualizarAdmonFacturasFirmasContraparte(plngIdRegistro As Integer,
                                                             pdtmFechaElaboracion As DateTime,
                                                             pstrIDComitente As String,
                                                             pstrNombreConsecutivo As String,
                                                             pstrNombre As String,
                                                             pstrTipoIdentificacion As String,
                                                             pstrNroDocumento As String,
                                                             pcurValorAbonar As Double,
                                                             pstrFormaPago As String,
                                                             plngIDBancoConsignacion As Integer,
                                                             pstrBancoGirador As String,
                                                             pdtmFechaConsignacion As DateTime,
                                                             pstrDetalle As String,
                                                             plngNroCheque As Integer,
                                                             pstrObservaciones As String,
                                                             pstrPrefijo As String,
                                                             ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of RetornoAdmonFacturasFirmas)

        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_Actualizar_AdmonFacturasFirmasContraparteTTV_OyDNet(
                                                             plngIdRegistro,
                                                            pdtmFechaElaboracion,
                                                            pstrIDComitente,
                                                            pstrNombreConsecutivo,
                                                             pstrNombre,
                                                             pstrTipoIdentificacion,
                                                             pstrNroDocumento,
                                                             pcurValorAbonar,
                                                             pstrFormaPago,
                                                             plngIDBancoConsignacion,
                                                             pstrBancoGirador,
                                                             pdtmFechaConsignacion,
                                                             pstrDetalle,
                                                             plngNroCheque,
                                                             pstrObservaciones,
                                                             pstrPrefijo,
                                                             pstrUsuario, DemeInfoSesion(pstrUsuario, "ActualizarAdmonFacturasFirmasContraparte"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizarAdmonFacturasFirmasContraparte")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' metodo para actulizar entidad en linq
    ''' </summary>
    ''' <remarks>UpdateAdmonFacturasFirmasContraparteDetalle()
    ''' Creado por   : Juan David Osorio Legarda.
    ''' Fecha            : Junio 2013
    ''' Pruebas CB       : Juan David Osorio Legarda.-  Junio 2013- Resultado Ok    
    ''' </remarks>
    Public Sub UpdateAdmonFacturasFirmasContraparteDetalle(ByVal obj As OyDTesoreria.AdmonFacturasFirmasContraparteDetalle)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,obj.pstrUsuarioConexion, obj.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.AdmonFacturasFirmasContraparteDetalle.Attach(obj, Me.ChangeSet.GetOriginal(obj))
            Me.DataContext.AdmonFacturasFirmasContraparteDetalle.DeleteOnSubmit(obj)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdateAdmonFacturasFirmasContraparteDetalle")
        End Try

    End Sub
#Region "Procesar exportar Excel"
    ''' <summary>
    ''' Funciones para Procesar y Exportar a Excel
    ''' </summary>
    ''' <remarks>
    ''' Creado por   : Juan David Osorio Legarda.
    ''' Fecha            : Junio 2013
    ''' Pruebas CB       : Juan David Osorio Legarda.-  Junio 2013- Resultado Ok    
    ''' </remarks>
    Public Function Traer_ReporteExcelAdmonFacturasFirmas(ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As System.Nullable(Of Boolean)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_ConsultarFacturasPendientesXCancelar().ToList
            RETORNO = False
            Usuario = pstrUsuario
            ListaTesoreria = ret.ToList
            Dim grid As New DataGrid
            grid.AutoGenerateColumns = False
            CrearColumnasTitulos(grid)
            Dim strMensaje As String = "csv"
            exportDataGrid(grid, strMensaje.ToUpper())
            If RETORNO = True Then
                Return True
            Else
                Return Nothing
            End If


        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Traer_ReporteExcelAdmonFacturasFirmas")
            Return Nothing
        End Try
    End Function
    Private Sub CrearColumnasTitulos(ByVal pgrid As DataGrid)
        Dim lngID As New BoundColumn
        lngID.HeaderText = "lngID"
        pgrid.Columns.Add(lngID)

        Dim strPrefijo As New BoundColumn
        strPrefijo.HeaderText = "strPrefijo"
        pgrid.Columns.Add(strPrefijo)

        Dim dtmDocumento As New BoundColumn
        dtmDocumento.HeaderText = "dtmDocumento"
        pgrid.Columns.Add(dtmDocumento)

        Dim strNombre As New BoundColumn
        strNombre.HeaderText = "strNombre"
        pgrid.Columns.Add(strNombre)

        Dim DescPrefijo As New BoundColumn
        DescPrefijo.HeaderText = "DescPrefijo"
        pgrid.Columns.Add(DescPrefijo)

        Dim strTipoIdentificacion As New BoundColumn
        strTipoIdentificacion.HeaderText = "strTipoIdentificacion"
        pgrid.Columns.Add(strTipoIdentificacion)

        Dim lngNroIdentificacion As New BoundColumn
        lngNroIdentificacion.HeaderText = "lngNroIdentificacion"
        pgrid.Columns.Add(lngNroIdentificacion)

        Dim strDireccion As New BoundColumn
        strDireccion.HeaderText = "strDireccion"
        pgrid.Columns.Add(strDireccion)

        Dim strTelefono As New BoundColumn
        strTelefono.HeaderText = "strTelefono"
        pgrid.Columns.Add(strTelefono)

        Dim VlrFactura As New BoundColumn
        VlrFactura.HeaderText = "VlrFactura"
        pgrid.Columns.Add(VlrFactura)

        Dim SaldoXPagar As New BoundColumn
        SaldoXPagar.HeaderText = "SaldoXPagar"
        pgrid.Columns.Add(SaldoXPagar)

        Dim Operacion As New BoundColumn
        Operacion.HeaderText = "Operacion"
        pgrid.Columns.Add(Operacion)

        Dim ClienteOperacion As New BoundColumn
        ClienteOperacion.HeaderText = "ClienteOperacion"
        pgrid.Columns.Add(ClienteOperacion)

    End Sub
    Private Sub exportDataGrid(ByVal dGrid As DataGrid, ByVal strFormat As String)

        Dim strBuilder As New StringBuilder()
        Dim strLineas As New List(Of String)

        If IsNothing(dGrid) Then Return
        Dim lstFields As List(Of String) = New List(Of String)()
        'If dGrid.HeaderStyle= Then  'es para saber si es la columna de el header y q le de el formato
        For Each dgcol As DataGridColumn In dGrid.Columns
            lstFields.Add(formatField(dgcol.HeaderText.ToString(), strFormat, True))
        Next
        buildStringOfRow(strBuilder, lstFields, strFormat)
        strLineas.Add(strBuilder.ToString())
        'End If
        dGrid.DataSource = ListaTesoreria

        For a = 0 To ListaTesoreria.Count - 1
            lstFields.Clear()
            strBuilder.Clear()
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTesoreria(a).lngID), String.Empty, ListaTesoreria(a).lngID)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTesoreria(a).strPrefijo), String.Empty, ListaTesoreria(a).strPrefijo)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTesoreria(a).dtmDocumento), String.Empty, ListaTesoreria(a).dtmDocumento)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTesoreria(a).strNombre), String.Empty, ListaTesoreria(a).strNombre)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTesoreria(a).DescPrefijo), String.Empty, ListaTesoreria(a).DescPrefijo)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTesoreria(a).strTipoIdentificacion), String.Empty, ListaTesoreria(a).strTipoIdentificacion)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTesoreria(a).lngNroIdentificacion), String.Empty, ListaTesoreria(a).lngNroIdentificacion)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTesoreria(a).strDireccion), String.Empty, ListaTesoreria(a).strDireccion)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTesoreria(a).strTelefono), String.Empty, ListaTesoreria(a).strTelefono)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTesoreria(a).VlrFactura), String.Empty, ListaTesoreria(a).VlrFactura)), strFormat, False))
            lstFields.Add(formatField(CStr(IIf(IsNothing(ListaTesoreria(a).SaldoXPagar), String.Empty, ListaTesoreria(a).SaldoXPagar)), strFormat, False))
            buildStringOfRow(strBuilder, lstFields, strFormat)
            strLineas.Add(strBuilder.ToString())
        Next

        RETORNO = Guardar_ArchivoServidor(CSTR_NOMBREPROCESO_ADMONFACTURASFIRMAS, Usuario, String.Format("ReporteAdmonFacturasFirmasContraparte{0}.csv", Now.ToString("yyyy-mm-dd")), strLineas)

    End Sub
    Private Shared Function formatField(ByVal data As String, ByVal format As String, ByVal encabezado As Boolean) As String
        Select Case format
            Case "XML"
                Return String.Format("<Cell><Data ss:Type=""String" & """>{0}</Data></Cell>", data)
            Case "CSV"
                If encabezado = True Then
                    Return String.Format("""   {0}   """, data.Replace("""", """""""").Replace("" & vbLf & "", "").Replace("" & vbCr & "", ""))
                Else
                    Return String.Format("""{0}""", data.Replace("""", """""""").Replace("" & vbLf & "", "").Replace("" & vbCr & "", ""))
                End If
        End Select
        Return data

    End Function
    Private Shared Sub buildStringOfRow(ByVal strBuilder As StringBuilder, ByVal lstFields As List(Of String), ByVal strFormat As String)
        Select Case strFormat
            Case "XML"
                strBuilder.AppendLine("<Row>")
                strBuilder.AppendLine(String.Join("" & vbCrLf & "", lstFields.ToArray()))
                strBuilder.AppendLine("</Row>")
                ' break;
            Case "CSV"
                strBuilder.AppendLine(String.Join(SEPARATOR_FORMAT_CVS, lstFields.ToArray()))
                ' break;

        End Select

    End Sub
    Public Function Guardar_ArchivoServidor(ByVal NombreProceso As String, ByVal pstrUsuario As String, ByVal NombreArchivo As String, ByVal Lista As List(Of String)) As Boolean
        Try
            GuardarArchivo(NombreProceso, pstrUsuario, NombreArchivo, Lista, False)
            Return True
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Guardar_ArchivoServidor")
            Return False
        End Try
    End Function
#End Region
#End Region

#Region "Pagos Saldos Deceval"

    Public Function ConsultarPagosDeceval(ByVal pdtmFechaElaboracionRCDesde As Date, ByVal pdtmFechaElaboracionRCHasta As Date, ByVal ConsecutivoRC As String,
                                          ByVal intNroRC As Integer?, ByVal plogDividendos As Boolean?, ByVal pstrFormaPago As String,
                                          ByVal pstrUsuario As String, ByVal plogDemocratizados As Boolean?, ByVal pstrIDEspecie As String, ByVal pstrTipoCartera As String,
                                          ByVal pstrTipoBanco As String, ByVal pstrTipoCheque As String, ByVal pstrTipoEspecie As String, plogExento As Nullable(Of Boolean), ByVal pstrInfoConexion As String) As List(Of PagosDECEVAL)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_OyD_DetalleReciboCaja_PagosDECEVAL_Consultar_OyDNet(pdtmFechaElaboracionRCDesde, pdtmFechaElaboracionRCHasta, ConsecutivoRC,
                                                                                             intNroRC, plogDividendos, pstrFormaPago, pstrUsuario,
                                                                                             DemeInfoSesion(pstrUsuario, "ConsultarPagosDeceval"), 0, plogDemocratizados, pstrIDEspecie, pstrTipoCartera,
                                                                                             pstrTipoBanco, pstrTipoCheque, pstrTipoEspecie, plogExento).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ConsultarPagosDeceval")
            Return Nothing
        End Try
    End Function

    Public Sub UpdatePagosDECEVAL(ByVal currentPagosDECEVAL As PagosDECEVAL)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentPagosDECEVAL.pstrUsuarioConexion, currentPagosDECEVAL.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.PagosDECEVAL.InsertOnSubmit(currentPagosDECEVAL)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "UpdatePagosDECEVAL")
        End Try
    End Sub

    Public Sub DeletePagosDECEVAL(ByVal currentPagosDECEVAL As PagosDECEVAL)
        Try
            'ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,currentPagosDECEVAL.pstrUsuarioConexion, currentPagosDECEVAL.pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Me.DataContext.PagosDECEVAL.Attach(currentPagosDECEVAL)
            Me.DataContext.PagosDECEVAL.DeleteOnSubmit(currentPagosDECEVAL)
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DeletePagosDECEVAL")
        End Try
    End Sub

    Public Function CuentasClientes_Verificar(ByVal plngIDComitente As String, ByVal pstrProcedencia As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tmpCuentasCliente)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_CuentasClientes_Verificar(plngIDComitente, pstrProcedencia, pstrUsuario, DemeInfoSesion(pstrUsuario, "CuentasClientes_Verificar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CuentasClientes_Verificar")
            Return Nothing
        End Try
    End Function

    Public Function RelacionRCDecevalCENotas(ByVal plngIDDocumentoInicial As Integer, ByVal plngSecuenciaInicial As Integer, ByVal pstrNombreConsecutivoInicial As String,
                                             ByVal pstrTipoInicial As String, ByVal plngIDDocumentoFinal As Integer, ByVal pstrNombreConsecutivoFinal As String,
                                             ByVal pstrTipoFinal As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_OyD_DetalleReciboCaja_RelacionCENotas_Ingresar_OyDNet(plngIDDocumentoInicial, plngSecuenciaInicial, pstrNombreConsecutivoInicial,
                                                                                               pstrTipoInicial, plngIDDocumentoFinal, pstrNombreConsecutivoFinal,
                                                                                               pstrTipoFinal, pstrUsuario, DemeInfoSesion(pstrUsuario, "RelacionRCDecevalCENotas "), 0)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "RelacionRCDecevalCENotas ")
            Return Nothing
        End Try
    End Function

    Public Function InsertarNOTA(ByVal pstrNombreConsecutivo As String, ByVal pdtmDocumento As Date, ByVal plogContabilidad As Boolean,
                                 ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim Clientes_DC As New OyDTesoreriaDatacontext
            Dim ret = Clientes_DC.spInsNotaContable_OyDNet(pstrNombreConsecutivo, pdtmDocumento, plogContabilidad, pstrUsuario)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertarNOTA")
            Return Nothing
        End Try
    End Function

    Public Function InsertarDetalleNOTA(ByVal pstrTipo As String, ByVal pstrNombreConsecutivo As String, ByVal plngIDDocumento As Integer,
                                        ByVal plngSecuencia As Integer, ByVal plngIDComitente As String, ByVal pcurValor As Decimal,
                                        ByVal pstrIDCuentaContable As String, ByVal pstrDetalle As String, ByVal plngIDBanco As Integer?,
                                        ByVal pstrNIT As String, ByVal pstrCCosto As String, ByVal pstrUsuario As String,
                                        ByVal pstrNombreConsecutivoNotaGMF As String, ByVal plngIDDocumentoNotaGMF As Integer?, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim Clientes_DC As New OyDTesoreriaDatacontext
            Dim ret = Clientes_DC.spInsNotaContableDetalle_OyDNet(pstrTipo, pstrNombreConsecutivo, plngIDDocumento, plngSecuencia, plngIDComitente,
                                                                  pcurValor, pstrIDCuentaContable, pstrDetalle, plngIDBanco, pstrNIT, pstrCCosto,
                                                                  pstrUsuario, pstrNombreConsecutivoNotaGMF, plngIDDocumentoNotaGMF)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "InsertarDetalleNOTA")
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Función encargada de insertar los datos a exportar para generar el plano ACH dependiendo del formato seleccionado.
    ''' </summary>
    ''' <param name="intCodigoFormatoExportacion"></param>
    ''' <param name="strFormato"></param>
    ''' <returns></returns>
    ''' <remarks>SLB20130903</remarks>
    Public Function PlanosACHCadena_Generar(ByVal intCodigoFormatoExportacion As Integer, ByVal strFormato As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try

            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_OyD_PlanoPagosATerceros_Generar_OyDNet(intCodigoFormatoExportacion, strFormato, DemeInfoSesion(pstrUsuario, "PlanosACHCadena_Generar"), 0)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PlanosACHCadena_Generar")
            Return Nothing
        End Try
    End Function

    Public Function PlanosACHCadena_Insertar(ByVal pstrCadena1 As String, ByVal pstrCadena2 As String, ByVal pstrCadena3 As String, ByVal pstrCadena4 As String,
                                             ByVal pstrCadena5 As String, ByVal pstrCadena6 As String, ByVal pstrCadena7 As String, ByVal pstrCadena8 As String,
                                             ByVal pstrCadena9 As String, ByVal pstrCadena10 As String, ByVal pstrCadena11 As String, ByVal pstrCadena12 As String,
                                             ByVal pstrCadena13 As String, ByVal pstrCadena14 As String, ByVal pstrCadena15 As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)

            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_PlanosACHCadena_Insertar(pstrCadena1, pstrCadena2, pstrCadena3, pstrCadena4, pstrCadena5, pstrCadena6,
                                                                                  pstrCadena7, pstrCadena8, pstrCadena9, pstrCadena10, pstrCadena11, pstrCadena12,
                                                                                  pstrCadena13, pstrCadena14, pstrCadena15, pstrUsuario, DemeInfoSesion(pstrUsuario, "PlanosACHCadena_Insertar"), 0)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PlanosACHCadena_Insertar")
            Return Nothing
        End Try
    End Function

    Public Function RelacionarGMF(ByVal plngNroNotaGMF As Integer, ByVal pstrNombreConsecutivoNota As String, ByVal plngNroDocumento As Integer,
                                  ByVal pstrNombreConsecutivo As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_Tesoreria_RelacionarGMF(plngNroNotaGMF, pstrNombreConsecutivoNota, plngNroDocumento, pstrNombreConsecutivo,
                                                                       pstrUsuario, DemeInfoSesion(pstrUsuario, "RelacionarGMF"), 0)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "RelacionarGMF")
            Return Nothing
        End Try
    End Function

    '<Query(HasSideEffects:=True)> _
    Public Function Borrar_GeneracionPagosDeceval(ByVal strBorrar As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret As Integer = 1
            Dim parametros() As String
            Dim CExportadosBorrar = Split(strBorrar, ";")
            For Each objBorrar In CExportadosBorrar
                parametros = Split(objBorrar, "*")
                If parametros.Length = 3 Then
                    ret = Me.DataContext.uspOyDNet_Tesoreria_PagosSaldos_Borrar(CInt(parametros(0)), parametros(1), parametros(2), Nothing,
                                                                                    Nothing, Nothing, pstrUsuario, DemeInfoSesion(pstrUsuario, "Borrar_GeneracionPagosDeceval"), 0)
                End If
                If parametros.Length = 6 Then
                    ret = Me.DataContext.uspOyDNet_Tesoreria_PagosSaldos_Borrar(CInt(parametros(0)), parametros(1), parametros(2), CInt(parametros(3)),
                                                                                CInt(parametros(4)), parametros(5), pstrUsuario, DemeInfoSesion(pstrUsuario, "Borrar_GeneracionPagosDeceval"), 0)
                End If
            Next

            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "Borrar_GeneracionPagosDeceval")
            Return Nothing
        End Try
    End Function

    Public Function ActualizarEstadoRC_PagoDeceval(ByVal pstrTipoDocumento As String, ByVal pintIDDocumento As Integer, ByVal pstrNombreConsecutivo As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Integer
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_ActualizarEstadoRC_PagoDeceval(pstrTipoDocumento, pintIDDocumento, pstrNombreConsecutivo, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "ActualizarEstadoRC_PagoDeceval"), 0)
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "ActualizarEstadoRC_PagoDeceval")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function GenerarComprobanteONotaDebitoDeceval(ByVal pstrRegistrosGenerar As String,
                                                         ByVal plogMostrarCamposACH As Boolean,
                                                         ByVal pstrFormaPagoCE As String,
                                                         ByVal plogTipoTesoreria As Boolean,
                                                         ByVal pstrNombreConsecutivo As String,
                                                         ByVal pintNroBanco As Nullable(Of Integer),
                                                         ByVal pdtmFechaElaboracion As Nullable(Of DateTime),
                                                         ByVal pstrTipoCheque As String,
                                                         ByVal plogCobroGMF As Boolean,
                                                         ByVal pstrCuentaContable As String,
                                                         ByVal plogExento As Nullable(Of Boolean),
                                                         ByVal pstrUsuario As String,
                                                         ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GeneracionCENCDeceval)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            pstrRegistrosGenerar = HttpUtility.UrlDecode(pstrRegistrosGenerar)
            Dim ret = Me.DataContext.uspOyDNet_ComprobantesONotasDebito_Generar(pstrRegistrosGenerar, plogMostrarCamposACH, pstrFormaPagoCE,
                                                                                plogTipoTesoreria, pstrNombreConsecutivo, pintNroBanco, pdtmFechaElaboracion,
                                                                                pstrTipoCheque, plogCobroGMF, pstrCuentaContable, plogExento, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "GenerarComprobanteONotaDebitoDeceval"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarComprobanteONotaDebitoDeceval")
            Return Nothing
        End Try
    End Function
    <Query(HasSideEffects:=True)>
    Public Function GenerarComprobanteONotaDebitoDecevalSync(ByVal pstrRegistrosGenerar As String,
                                                         ByVal plogMostrarCamposACH As Boolean,
                                                         ByVal pstrFormaPagoCE As String,
                                                         ByVal plogTipoTesoreria As Boolean,
                                                         ByVal pstrNombreConsecutivo As String,
                                                         ByVal pintNroBanco As Nullable(Of Integer),
                                                         ByVal pdtmFechaElaboracion As Nullable(Of DateTime),
                                                         ByVal pstrTipoCheque As String,
                                                         ByVal plogCobroGMF As Boolean,
                                                         ByVal pstrCuentaContable As String,
                                                         ByVal plogExento As Nullable(Of Boolean),
                                                         ByVal pstrUsuario As String,
                                                         ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GeneracionCENCDeceval)
        Dim objTask As Task(Of List(Of GeneracionCENCDeceval)) = Me.GenerarComprobanteONotaDebitoDecevalAsync(pstrRegistrosGenerar, plogMostrarCamposACH, pstrFormaPagoCE,
                                                                                plogTipoTesoreria, pstrNombreConsecutivo, pintNroBanco, pdtmFechaElaboracion,
                                                                                pstrTipoCheque, plogCobroGMF, pstrCuentaContable, plogExento, pstrUsuario, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function GenerarComprobanteONotaDebitoDecevalAsync(ByVal pstrRegistrosGenerar As String,
                                                         ByVal plogMostrarCamposACH As Boolean,
                                                         ByVal pstrFormaPagoCE As String,
                                                         ByVal plogTipoTesoreria As Boolean,
                                                         ByVal pstrNombreConsecutivo As String,
                                                         ByVal pintNroBanco As Nullable(Of Integer),
                                                         ByVal pdtmFechaElaboracion As Nullable(Of DateTime),
                                                         ByVal pstrTipoCheque As String,
                                                         ByVal plogCobroGMF As Boolean,
                                                         ByVal pstrCuentaContable As String,
                                                         ByVal plogExento As Nullable(Of Boolean),
                                                         ByVal pstrUsuario As String,
                                                         ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of GeneracionCENCDeceval))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of GeneracionCENCDeceval)) = New TaskCompletionSource(Of List(Of GeneracionCENCDeceval))()
        objTaskComplete.TrySetResult(GenerarComprobanteONotaDebitoDeceval(pstrRegistrosGenerar, plogMostrarCamposACH, pstrFormaPagoCE,
                                                                                plogTipoTesoreria, pstrNombreConsecutivo, pintNroBanco, pdtmFechaElaboracion,
                                                                                pstrTipoCheque, plogCobroGMF, pstrCuentaContable, plogExento, pstrUsuario, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#Region "Carga Masiva Tesoreria"
    Public Function CargaMasivaValidarTesoreriaManual(ByVal pstrTipoDocumento As String, ByVal pstrNombreConsecutivo As String, ByVal pdtmDocumento As System.Nullable(Of DateTime), ByVal plngIDBanco As System.Nullable(Of Integer), ByVal pstrTipoIdentificacion As String, ByVal plngNroDocumento As String,
                                                      ByVal pstrNombre As String, ByVal pstrFormaPago As String, ByVal plogContabilidad As Boolean, ByVal pstrTipocheque As String, ByVal pstrCuentaCliente As String,
                                                      ByVal plngIDComitente As String, ByVal plngIDConcepto As System.Nullable(Of Integer), ByVal pstrDetalle As String, ByVal pstrIDCuentaContable As String,
                                                      ByVal pstrNIT As String, ByVal pstrCentroCosto As String, ByVal pcurValor As Nullable(Of Decimal), ByVal pcurValorCR As Nullable(Of Decimal), ByVal pcurValorDB As Nullable(Of Decimal), ByVal pstrBancoGiradorRC As String,
                                                      ByVal lngNumCheque As Nullable(Of Double), ByVal pcurValorChequeRC As Nullable(Of Decimal), ByVal pdtmConsignacionRC As System.Nullable(Of DateTime), ByVal pstrFormaPagoRC As String,
                                                      ByVal pstrObservacionesRC As String, ByVal plngIDTipoProducto As System.Nullable(Of Integer), ByVal plngBancoDestino As System.Nullable(Of Integer), ByVal pstrNombreBancoDestino As String,
                                                      ByVal pstrCuentaDestino As String, ByVal pstrTipoCuenta As String, ByVal pstrDescripcionTipoCuenta As String, ByVal pstrIdentificacionTitular As String, ByVal pstrTitular As String,
                                                      ByVal pstrTipoIdTitular As String, ByVal pstrDescripcionTipoIdTitular As String, ByVal pstrFormaEntrega As String, ByVal pstrDescripcionFormaEntrega As String, ByVal pstrBeneficiario As String,
                                                      ByVal pstrTipoIdentBeneficiario As String, ByVal pstrDescripcionTipoIdentBeneficiario As String, ByVal pstrIdentificacionBenficiciario As String, ByVal pstrNombrePersonaRecoge As String,
                                                      ByVal pstrIdentificacionPerRecoge As String, ByVal pstrOficinaEntrega As String, ByVal pstrOficinaCuentaInversion As String, ByVal pstrNombreCarteraColectiva As String, ByVal pstrNombreAsesor As String,
                                                      ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidacionesCargaMasiva)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CargaMasivaTesoreria_ValidarDatos(pstrTipoDocumento, pstrNombreConsecutivo, pdtmDocumento, plngIDBanco, pstrTipoIdentificacion, plngNroDocumento,
                                                                                 pstrNombre, pstrFormaPago, plogContabilidad, pstrTipocheque, pstrCuentaCliente, plngIDComitente, plngIDConcepto,
                                                                                 pstrDetalle, pstrIDCuentaContable, pstrNIT, pstrCentroCosto, pcurValor, pcurValorCR, pcurValorDB, pstrBancoGiradorRC,
                                                                                 lngNumCheque, pcurValorChequeRC, pdtmConsignacionRC, pstrFormaPagoRC, pstrObservacionesRC, plngIDTipoProducto,
                                                                                 plngBancoDestino, pstrNombreBancoDestino, pstrCuentaDestino, pstrTipoCuenta, pstrDescripcionTipoCuenta, pstrIdentificacionTitular, pstrTitular,
                                                                                 pstrTipoIdTitular, pstrDescripcionTipoIdTitular, pstrFormaEntrega, pstrDescripcionFormaEntrega, pstrBeneficiario, pstrTipoIdentBeneficiario,
                                                                                 pstrDescripcionTipoIdentBeneficiario, pstrIdentificacionBenficiciario, pstrNombrePersonaRecoge, pstrIdentificacionPerRecoge,
                                                                                 pstrOficinaEntrega, pstrOficinaCuentaInversion, pstrNombreCarteraColectiva, pstrNombreAsesor,
                                                                                 pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "CargaMasivaValidarTesoreriaManual"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargaMasivaValidarTesoreriaManual")
            Return Nothing
        End Try
    End Function

    Public Function CargaMasivaTesoreriaConsultarResultados(ByVal pstrTipoDocumento As String, ByVal pstrNombreConsecutivo As String, ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of tblRespuestaValidacionesCargaMasiva)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CargaMasivaTesoreria_ConsultarResultados(pstrTipoDocumento, pstrNombreConsecutivo,
                                                                                 pstrUsuario, pstrUsuarioWindows, pstrMaquina,
                                                                                 DemeInfoSesion(pstrUsuario, "CargaMasivaTesoreriaConsultarResultados"), 0)
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargaMasivaTesoreriaConsultarResultados")
            Return Nothing
        End Try
    End Function

    Public Function CargaMasivaTesoreria_ValidarHabilitarCampos(ByVal pstrTipoDocumento As String, ByVal pstrNombreConsecutivo As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of CamposEditablesCargaTesoreria)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CargaMasivaTesoreria_ValidarHabilitarCampos(pstrTipoDocumento,
                                                                                           pstrNombreConsecutivo,
                                                                                      pstrUsuario,
                                                                                      pstrMaquina,
                                                                                      DemeInfoSesion(pstrUsuario, "CargaMasivaTesoreria_ValidarHabilitarCampos"),
                                                                                      0
                                                                                    ).ToList
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargaMasivaTesoreria_ValidarHabilitarCampos")
            Return Nothing
        End Try
    End Function

    Public Function CargaMasivaTesoreria_ConsultarCantidadProcesados(ByVal pstrTipoDocumento As String, ByVal pstrNombreConsecutivo As String, ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of CargaMasivaCantidadProcesadas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CargaMasivaTesoreria_ConsultarCantidadProcesados(pstrTipoDocumento,
                                                                                                pstrNombreConsecutivo,
                                                                                                pstrUsuario,
                                                                                                pstrUsuarioWindows,
                                                                                                pstrMaquina,
                                                                                                DemeInfoSesion(pstrUsuario, "CargaMasivaTesoreria_ConsultarCantidadProcesados"), 0
                                                                                    ).ToList
            Return ret.ToList
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargaMasivaTesoreria_ConsultarCantidadProcesados")
            Return Nothing
        End Try
    End Function

#End Region

#Region "GenerarCEIntermedia"

    Public Sub InsertGenerarCEIntermedia(ByVal newGenerarCEIntermedia As OyDTesoreria.GenerarCEIntermedia)

    End Sub

    Public Sub UpdateGenerarCEIntermedia(ByVal currentGenerarCEIntermedia As OyDTesoreria.GenerarCEIntermedia)

    End Sub

    Public Function GenerarCEIntermediaConsultar(ByVal pintSucursal As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GenerarCEIntermedia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_GenerarCE_Intermedia_Consultar(pintSucursal, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "GenerarCEIntermediaConsultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarCEIntermediaConsultar")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function GenerarCEIntermediaGenerar(ByVal pintSucursal As String, ByVal pstrRegistros As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GenerarCEIntermedia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_GenerarCE_Intermedia_Generar(pintSucursal, pstrRegistros, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "GenerarCEIntermediaGenerar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarCEIntermediaGenerar")
            Return Nothing
        End Try
    End Function

    Public Function GenerarCE_Intermedia_Seleccionar(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GenerarCEIntermedia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_GenerarCE_Intermedia_Seleccionar(pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "GenerarCE_Intermedia_Seleccionar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarCE_Intermedia_Seleccionar")
            Return Nothing
        End Try
    End Function

    Public Function GenerarCE_Intermedia_Resultados(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of ResultadosGenerarRCIntermedia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_GenerarCE_Intermedia_Resultados(pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "GenerarCE_Intermedia_Resultados"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarCE_Intermedia_Resultados")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function GenerarCEIntermediaValidarConfirmaciones(ByVal pintSucursal As String, ByVal pstrRegistros As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of ResultadosGenerarRCIntermedia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_GenerarCE_Intermedia_VerificarConfirmaciones(pintSucursal, pstrRegistros, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "GenerarCEIntermediaValidarConfirmaciones"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarCEIntermediaValidarConfirmaciones")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Métodos sincrónicos"

    Public Function GenerarCE_Intermedia_ConsultarSync(ByVal pintSucursal As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GenerarCEIntermedia)
        Dim objTask As Task(Of List(Of GenerarCEIntermedia)) = Me.GenerarCE_Intermedia_ConsultarAsync(pintSucursal, pstrUsuario, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function GenerarCE_Intermedia_ConsultarAsync(ByVal pintSucursal As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of GenerarCEIntermedia))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of GenerarCEIntermedia)) = New TaskCompletionSource(Of List(Of GenerarCEIntermedia))()
        objTaskComplete.TrySetResult(GenerarCEIntermediaConsultar(pintSucursal, pstrUsuario, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function CargaCECarteraColectivas_ConsultarSync(ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GenerarCECarterasColectivas)
        Dim objTask As Task(Of List(Of GenerarCECarterasColectivas)) = Me.CargaCECarteraColectivas_ConsultarAsync(pstrUsuario, pstrUsuarioWindows, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function CargaCECarteraColectivas_ConsultarAsync(ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of GenerarCECarterasColectivas))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of GenerarCECarterasColectivas)) = New TaskCompletionSource(Of List(Of GenerarCECarterasColectivas))()
        objTaskComplete.TrySetResult(CargaCECarteraColectivas_Consultar(pstrUsuario, pstrUsuarioWindows, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function




    <Query(HasSideEffects:=True)>
    Public Function GenerarCE_Intermedia_GenerarSync(ByVal pintSucursal As String, ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GenerarCEIntermedia)
        Dim objTask As Task(Of List(Of GenerarCEIntermedia)) = Me.GenerarCE_Intermedia_GenerarAsync(pintSucursal, pxmlDetalleGrid, pstrUsuario, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function GenerarCE_Intermedia_GenerarAsync(ByVal pintSucursal As String, ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of GenerarCEIntermedia))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of GenerarCEIntermedia)) = New TaskCompletionSource(Of List(Of GenerarCEIntermedia))()
        objTaskComplete.TrySetResult(GenerarCEIntermediaGenerar(pintSucursal, pxmlDetalleGrid, pstrUsuario, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function GenerarCE_Intermedia_SeleccionarSync(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GenerarCEIntermedia)
        Dim objTask As Task(Of List(Of GenerarCEIntermedia)) = Me.GenerarCE_Intermedia_SeleccionarAsync(pstrUsuario, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function GenerarCE_Intermedia_SeleccionarAsync(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of GenerarCEIntermedia))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of GenerarCEIntermedia)) = New TaskCompletionSource(Of List(Of GenerarCEIntermedia))()
        objTaskComplete.TrySetResult(GenerarCE_Intermedia_Seleccionar(pstrUsuario, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function GenerarCE_Intermedia_ResultadosSync(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of ResultadosGenerarRCIntermedia)
        Dim objTask As Task(Of List(Of ResultadosGenerarRCIntermedia)) = Me.GenerarCE_Intermedia_ResultadosAsync(pstrUsuario, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function GenerarCE_Intermedia_ResultadosAsync(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ResultadosGenerarRCIntermedia))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ResultadosGenerarRCIntermedia)) = New TaskCompletionSource(Of List(Of ResultadosGenerarRCIntermedia))()
        objTaskComplete.TrySetResult(GenerarCE_Intermedia_Resultados(pstrUsuario, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    <Query(HasSideEffects:=True)>
    Public Function GenerarCEIntermediaValidarConfirmacionesSync(ByVal pintSucursal As String, ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of ResultadosGenerarRCIntermedia)
        Dim objTask As Task(Of List(Of ResultadosGenerarRCIntermedia)) = Me.GenerarCEIntermediaValidarConfirmacionesAsync(pintSucursal, pxmlDetalleGrid, pstrUsuario, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function GenerarCEIntermediaValidarConfirmacionesAsync(ByVal pintSucursal As String, ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ResultadosGenerarRCIntermedia))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ResultadosGenerarRCIntermedia)) = New TaskCompletionSource(Of List(Of ResultadosGenerarRCIntermedia))()
        objTaskComplete.TrySetResult(GenerarCEIntermediaValidarConfirmaciones(pintSucursal, pxmlDetalleGrid, pstrUsuario, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    <Query(HasSideEffects:=True)>
    Public Function GenerarCECarteraColectivasValidarConfirmacionesSync(ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of ResultadosGenerarRCIntermedia)
        Dim objTask As Task(Of List(Of ResultadosGenerarRCIntermedia)) = Me.GenerarCECarteraColectivasValidarConfirmacionesAsync(pxmlDetalleGrid, pstrUsuario, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function GenerarCECarteraColectivasValidarConfirmacionesAsync(ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ResultadosGenerarRCIntermedia))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ResultadosGenerarRCIntermedia)) = New TaskCompletionSource(Of List(Of ResultadosGenerarRCIntermedia))()
        objTaskComplete.TrySetResult(GenerarCECarteraColectivasValidarConfirmaciones(pxmlDetalleGrid, pstrUsuario, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)

    End Function

    <Query(HasSideEffects:=True)>
    Public Function GenerarCE_CarteraColectivas_GenerarSync(ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GenerarCECarterasColectivas)
        Dim objTask As Task(Of List(Of GenerarCECarterasColectivas)) = Me.GenerarCE_CarteraColectivas_GenerarAsync(pxmlDetalleGrid, pstrUsuario, pstrUsuarioWindows, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function GenerarCE_CarteraColectivas_GenerarAsync(ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of GenerarCECarterasColectivas))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of GenerarCECarterasColectivas)) = New TaskCompletionSource(Of List(Of GenerarCECarterasColectivas))()
        objTaskComplete.TrySetResult(GenerarCECarteraColectivasGenerar(pxmlDetalleGrid, pstrUsuario, pstrUsuarioWindows, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function GenerarCE_CarteraColectivas_ResultadosSync(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of ResultadosGenerarRCIntermedia)
        Dim objTask As Task(Of List(Of ResultadosGenerarRCIntermedia)) = Me.GenerarCE_CarteraColectivas_ResultadosAsync(pstrUsuario, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function GenerarCE_CarteraColectivas_ResultadosAsync(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ResultadosGenerarRCIntermedia))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ResultadosGenerarRCIntermedia)) = New TaskCompletionSource(Of List(Of ResultadosGenerarRCIntermedia))()
        objTaskComplete.TrySetResult(GenerarCE_CarteraColectivas_Resultados(pstrUsuario, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function GenerarCE_CarteraColectivas_SeleccionarSync(ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GenerarCECarterasColectivas)
        Dim objTask As Task(Of List(Of GenerarCECarterasColectivas)) = Me.GenerarCE_CarteraColectivas_SeleccionaAsync(pstrUsuario, pstrUsuarioWindows, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function GenerarCE_CarteraColectivas_SeleccionaAsync(ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of GenerarCECarterasColectivas))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of GenerarCECarterasColectivas)) = New TaskCompletionSource(Of List(Of GenerarCECarterasColectivas))()
        objTaskComplete.TrySetResult(GenerarCE_CarteraColectiva_Seleccionar(pstrUsuario, pstrUsuarioWindows, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#Region "Metodos Sincronos"
    Public Function CuentasClientes_VerificarSync(ByVal plngIDComitente As String, ByVal pstrProcedencia As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of tmpCuentasCliente)
        Dim objTask As Task(Of List(Of tmpCuentasCliente)) = Me.CuentasClientes_VerificarSyncAsync(plngIDComitente, pstrProcedencia, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function

    Private Function CuentasClientes_VerificarSyncAsync(ByVal plngIDComitente As String, ByVal pstrProcedencia As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of tmpCuentasCliente))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of tmpCuentasCliente)) = New TaskCompletionSource(Of List(Of tmpCuentasCliente))()
        objTaskComplete.TrySetResult(CuentasClientes_Verificar(plngIDComitente, pstrProcedencia, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function


#End Region

#Region "Métodos asincrónicos"

#End Region

#Region "GenerarRCIntermediaConsultar"

#Region "Métodos modelo para activar funcionalidad RIA"


    Public Sub InsertGenerarRCIntermedia(ByVal newGenerarRCIntermedia As OyDTesoreria.GenerarRCIntermedia)

    End Sub

    Public Sub UpdateGenerarRCIntermedia(ByVal currentGenerarRCIntermedia As OyDTesoreria.GenerarRCIntermedia)

    End Sub

    Public Sub InsertGenerarCECarteraColectivas(ByVal newGenerarCECarteraColectivas As OyDTesoreria.GenerarCECarterasColectivas)

    End Sub

    Public Sub UpdateGenerarCECarteraColectivas(ByVal currentGenerarCECarteraColectivasa As OyDTesoreria.GenerarCECarterasColectivas)

    End Sub


    Public Sub InsertResultadoGeneAutoNotas(ByVal newResultadosGenerarRCIntermedia As OyDTesoreria.ResultadoGeneAutoNotas)

    End Sub

    Public Sub UpdateResultadoGeneAutoNotas(ByVal currentResultadosGenerarRCIntermedia As OyDTesoreria.ResultadoGeneAutoNotas)

    End Sub


    Public Sub InsertResultadosGenerarRCIntermedia(ByVal newResultadosGenerarRCIntermedia As OyDTesoreria.ResultadosGenerarRCIntermedia)

    End Sub

    Public Sub UpdateResultadosGenerarRCIntermedia(ByVal currentResultadosGenerarRCIntermedia As OyDTesoreria.ResultadosGenerarRCIntermedia)

    End Sub

#End Region

#Region "Métodos asincrónicos"

    Public Function GenerarRC_Intermedia_Consultar(ByVal pintSucursal As System.Nullable(Of Integer), ByVal pdtmFechaInicial As System.Nullable(Of System.DateTime), ByVal pdtmFechaFinal As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GenerarRCIntermedia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_GenerarRC_Intermedia_Consultar(pintSucursal, pdtmFechaInicial, pdtmFechaFinal, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "GenerarRC_Intermedia_Consultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarRC_Intermedia_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function GenerarRC_Intermedia_Generar(ByVal pintSucursal As System.Nullable(Of Integer), ByVal pdtmFechaInicial As System.Nullable(Of System.DateTime), ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GenerarRCIntermedia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_GenerarRC_Intermedia_Generar(pintSucursal, pdtmFechaInicial, pxmlDetalleGrid, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "GenerarRC_Intermedia_Generar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarRC_Intermedia_Generar")
            Return Nothing
        End Try
    End Function

    Public Function GenerarRC_Intermedia_Seleccionar(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GenerarRCIntermedia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_GenerarRC_Intermedia_Seleccionar(pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "GenerarRC_Intermedia_Seleccionar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarRC_Intermedia_Seleccionar")
            Return Nothing
        End Try
    End Function

    Public Function GenerarRC_Intermedia_Resultados(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of ResultadosGenerarRCIntermedia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_GenerarRC_Intermedia_Resultados(pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "GenerarRC_Intermedia_Resultados"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarRC_Intermedia_Resultados")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function GenerarRC_Intermedia_ConsultarSync(ByVal pintSucursal As System.Nullable(Of Integer), ByVal pdtmFechaInicial As System.Nullable(Of System.DateTime), ByVal pdtmFechaFinal As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GenerarRCIntermedia)
        Dim objTask As Task(Of List(Of GenerarRCIntermedia)) = Me.GenerarRC_Intermedia_ConsultarAsync(pintSucursal, pdtmFechaInicial, pdtmFechaFinal, pstrUsuario, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function GenerarRC_Intermedia_ConsultarAsync(ByVal pintSucursal As System.Nullable(Of Integer), ByVal pdtmFechaInicial As System.Nullable(Of System.DateTime), ByVal pdtmFechaFinal As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of GenerarRCIntermedia))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of GenerarRCIntermedia)) = New TaskCompletionSource(Of List(Of GenerarRCIntermedia))()
        objTaskComplete.TrySetResult(GenerarRC_Intermedia_Consultar(pintSucursal, pdtmFechaInicial, pdtmFechaFinal, pstrUsuario, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function GenerarRC_Intermedia_GenerarSync(ByVal pintSucursal As System.Nullable(Of Integer), ByVal pdtmFechaInicial As System.Nullable(Of System.DateTime), ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GenerarRCIntermedia)
        Dim objTask As Task(Of List(Of GenerarRCIntermedia)) = Me.GenerarRC_Intermedia_GenerarAsync(pintSucursal, pdtmFechaInicial, pxmlDetalleGrid, pstrUsuario, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function GenerarRC_Intermedia_GenerarAsync(ByVal pintSucursal As System.Nullable(Of Integer), ByVal pdtmFechaInicial As System.Nullable(Of System.DateTime), ByVal pxmlDetalleGrid As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of GenerarRCIntermedia))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of GenerarRCIntermedia)) = New TaskCompletionSource(Of List(Of GenerarRCIntermedia))()
        objTaskComplete.TrySetResult(GenerarRC_Intermedia_Generar(pintSucursal, pdtmFechaInicial, pxmlDetalleGrid, pstrUsuario, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function GenerarRC_Intermedia_SeleccionarSync(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GenerarRCIntermedia)
        Dim objTask As Task(Of List(Of GenerarRCIntermedia)) = Me.GenerarRC_Intermedia_SeleccionarAsync(pstrUsuario, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function GenerarRC_Intermedia_SeleccionarAsync(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of GenerarRCIntermedia))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of GenerarRCIntermedia)) = New TaskCompletionSource(Of List(Of GenerarRCIntermedia))()
        objTaskComplete.TrySetResult(GenerarRC_Intermedia_Seleccionar(pstrUsuario, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function GenerarRC_Intermedia_ResultadosSync(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of ResultadosGenerarRCIntermedia)
        Dim objTask As Task(Of List(Of ResultadosGenerarRCIntermedia)) = Me.GenerarRC_Intermedia_ResultadosAsync(pstrUsuario, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function GenerarRC_Intermedia_ResultadosAsync(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ResultadosGenerarRCIntermedia))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ResultadosGenerarRCIntermedia)) = New TaskCompletionSource(Of List(Of ResultadosGenerarRCIntermedia))()
        objTaskComplete.TrySetResult(GenerarRC_Intermedia_Resultados(pstrUsuario, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "Recibos Recaudos"

#Region "TrasladosenLinea"
    Public Sub InsertGenerarRecibosRecaudos(ByVal newGenerarRecibosRecaudos As OyDTesoreria.TrasladoenLinea)

    End Sub

    Public Sub UpdateGenerarRecibosRecaudos(ByVal currentGenerarRecibosRecaudos As OyDTesoreria.TrasladoenLinea)

    End Sub
    Public Function RCTrasladosEnLinea_Consultar(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of TrasladoenLinea)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_RCTrasladosEnLinea_Consultar_OYDNET(pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "RCTrasladosEnLinea_Consultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "RCTrasladosEnLinea_Consultar")
            Return Nothing
        End Try
    End Function
    Public Function RCTrasladosEnLinea_Eliminar(ByVal pxml_RC As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaProcesosGenericosConfirmacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_RCTrasladosEnLinea_Eliminar_OYDNET(pxml_RC, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "RCTrasladosEnLinea_Eliminar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "RCTrasladosEnLinea_Eliminar")
            Return Nothing
        End Try
    End Function
    Public Function RCTrasladosEnLinea_Ingresar(ByVal pxml_RC As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaProcesosGenericosConfirmacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.usp_RCTrasladosEnLinea_Ingresar_OYDNET(pxml_RC, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "RCTrasladosEnLinea_Ingresar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "RCTrasladosEnLinea_Ingresar")
            Return Nothing
        End Try
    End Function
    Public Function RCRecibosRecaudos_Validar(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaProcesosGenericosConfirmacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_RecibosRecaudo_Validar(pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "RCRecibosRecaudos_Validar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "RCRecibosRecaudos_Validar")
            Return Nothing
        End Try
    End Function
#End Region

#End Region

#Region "Generar ajustes automaticos"

    Public Sub InsertGenerarAjustesAutomaticos(ByVal newGenerarAjustesAutomaticos As OyDTesoreria.GenerarAjustesAutomaticos)

    End Sub

    Public Sub UpdateGenerarAjustesAutomaticos(ByVal currentGenerarAjustesAutomaticos As OyDTesoreria.GenerarAjustesAutomaticos)

    End Sub

    Public Function AjustesAutomaticos_Consultar(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GenerarAjustesAutomaticos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_GenerarAjustesAutomaticos_Consultar(pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "AjustesAutomaticos_Consultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AjustesAutomaticos_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function AjustesAutomaticos_Generar(ByVal pstrConsecutivo As String, ByVal pstrTipoAjuste As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaProcesosGenericos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_GenerarAjustesAutomaticos_Generar(pstrConsecutivo, pstrTipoAjuste, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "AjustesAutomaticos_Generar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "AjustesAutomaticos_Generar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Carteras colectivas confianza"

    Public Function CarterasColectivasConfianza_RutaArchivo(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As String
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim strRutaArchivoDefecto As String = String.Empty
            Me.DataContext.uspOyDNet_CuentasActivasConfianza_RutaArchivo(strRutaArchivoDefecto, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "CarterasColectivasConfianza_RutaArchivo"), 0)
            Return strRutaArchivoDefecto
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CarterasColectivasConfianza_RutaArchivo")
            Return Nothing
        End Try
    End Function

#End Region

#Region "CE Carteras colectivas"
    Public Function CargaCECarteraColectivas_Consultar(ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GenerarCECarterasColectivas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CargaCECarteraColectivas_Consultar(pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "CargaCECarteraColectivas_Consultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "CargaCECarteraColectivas_Consultar")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function GenerarCECarteraColectivasValidarConfirmaciones(ByVal pstrRegistros As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of ResultadosGenerarRCIntermedia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CargaCECarteraColectivas_VerificarConfirmaciones(pstrRegistros, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "GenerarCECarteraColectivasValidarConfirmaciones"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarCECarteraColectivasValidarConfirmaciones")
            Return Nothing
        End Try
    End Function

    Public Function GenerarCE_CarteraColectivas_Resultados(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of ResultadosGenerarRCIntermedia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_GenerarCE_CarteraColectivas_Resultados(pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "GenerarCE_CarteraColectivas_Resultados"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarCE_CarteraColectivas_Resultados")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function GenerarCECarteraColectivasGenerar(ByVal pstrRegistros As String, ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GenerarCECarterasColectivas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_CargaCECarteraColectivas_Generar(pstrRegistros, pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "GenerarCEIntermediaGenerar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarCECarteraColectivasGenerar")
            Return Nothing
        End Try
    End Function

    Public Function GenerarCE_CarteraColectiva_Seleccionar(ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of GenerarCECarterasColectivas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_GenerarCE_CarteraColectivas_Seleccionar(pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "GenerarCE_CarteraColectiva_Seleccionar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarCE_CarteraColectiva_Seleccionar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "RecuadosXReferencia"

    Public Function RecaudosXReferencia_Consultar(ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of ImportarRecaudosXReferencia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_RecaudosXReferencia_Consultar(pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "RecaudosXReferencia_Consultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "RecaudosXReferencia_Consultar")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function RecaudosXReferencia_Generar(ByVal pdtmFechaRegistro As Nullable(Of DateTime), ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of ImportarRecaudosXReferencia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim pstrMsgValidacion As String = ""
            Dim ret = Me.DataContext.uspOyDNet_RecaudosXReferencia_Generar(pdtmFechaRegistro, pstrUsuario, pstrUsuarioWindows, pstrMaquina, DemeInfoSesion(pstrUsuario, "RecaudosXReferencia_Generar"), 0).ToList()
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "RecaudosXReferencia_Generar")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function RecaudosXReferencia_GenerarSync(ByVal pdtmFechaRegistro As Nullable(Of DateTime), ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of ImportarRecaudosXReferencia)
        Dim objTask As Task(Of List(Of ImportarRecaudosXReferencia)) = Me.RecaudosXReferencia_GenerarAsync(pdtmFechaRegistro, pstrUsuario, pstrUsuarioWindows, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function RecaudosXReferencia_GenerarAsync(ByVal pdtmFechaRegistro As Nullable(Of DateTime), ByVal pstrUsuario As String, ByVal pstrUsuarioWindows As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ImportarRecaudosXReferencia))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ImportarRecaudosXReferencia)) = New TaskCompletionSource(Of List(Of ImportarRecaudosXReferencia))()
        objTaskComplete.TrySetResult(RecaudosXReferencia_Generar(pdtmFechaRegistro, pstrUsuario, pstrUsuarioWindows, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function GenerarRecaudosXReferencia_ResultadosSync(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of ResultadosGenerarRCIntermedia)
        Dim objTask As Task(Of List(Of ResultadosGenerarRCIntermedia)) = Me.GenerarRecaudosXReferencia_ResultadosAsync(pstrUsuario, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function GenerarRecaudosXReferencia_ResultadosAsync(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of ResultadosGenerarRCIntermedia))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of ResultadosGenerarRCIntermedia)) = New TaskCompletionSource(Of List(Of ResultadosGenerarRCIntermedia))()
        objTaskComplete.TrySetResult(GenerarRecaudosXReferencia_Resultados(pstrUsuario, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    Public Function GenerarRecaudosXReferencia_Resultados(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of ResultadosGenerarRCIntermedia)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_GenerarRecaudosXReferencia_Resultados(pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "GenerarCE_CarteraColectivas_Resultados"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "GenerarCE_CarteraColectivas_Resultados")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Transferencia pagos masivos"
    Public Function TransferenciaPagosMasivos_Procesar(ByVal pstrNombreConsecutivo As String, ByVal pdtmFechaDocumento As DateTime,
                                                       ByVal plngIDBanco As Integer, ByVal plngIDComitente As String,
                                                       ByVal pstrNombre As String, ByVal pstrTipoDocumento As String,
                                                       ByVal pstrNroDocumento As String, ByVal pstrConfirmaciones As String,
                                                       ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaProcesosGenericosConfirmacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_TransferenciaPagosMasivos_Procesar(pstrNombreConsecutivo, pdtmFechaDocumento, plngIDBanco, plngIDComitente, pstrNombre, pstrTipoDocumento, pstrNroDocumento, pstrConfirmaciones, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "TransferenciaPagosMasivos_Procesar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TransferenciaPagosMasivos_Procesar")
            Return Nothing
        End Try
    End Function

    Public Function TransferenciaPagosMasivos_Consulta(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of TransferenciaPagosMasivos)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_TransferenciaPagosMasivos_Consultar(pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "TransferenciaPagosMasivos_Consulta"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TransferenciaPagosMasivos_Consulta")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Transferencia pagos masivos"
    Public Function TransaccionesCarterasColectivas_Consultar(ByVal pdtmFechaProceso As DateTime, ByVal pstrNombreConsecutivo As String,
                                                              ByVal pstrCuentaBancaria As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of TransaccionesCarterasColectivas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_TransaccionesCarterasColectivas_Consultar(pdtmFechaProceso, pstrNombreConsecutivo, pstrCuentaBancaria, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "TransaccionesCarterasColectivas_Consultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TransaccionesCarterasColectivas_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function TransaccionesCarterasColectivas_Validar(ByVal pdtmFechaProceso As DateTime, ByVal pstrNombreConsecutivo As String,
                                                              ByVal pstrCuentaBancaria As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaProcesosGenericosConfirmacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_TransaccionesCarterasColectivas_Validar(pdtmFechaProceso, pstrNombreConsecutivo, pstrCuentaBancaria, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "TransaccionesCarterasColectivas_Validar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TransaccionesCarterasColectivas_Validar")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Transferencias Carteras Colectivas"
    Public Function TransferenciasCarterasColectivas_Consultar(ByVal pdtmFechaProceso As DateTime, ByVal pstrNombreConsecutivo As String,
                                                              ByVal pstrCuentaBancaria As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of TransferenciasCarterasColectivas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_TransferenciasCarterasColectivas_Consultar(pdtmFechaProceso, pstrNombreConsecutivo, pstrCuentaBancaria, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "TransferenciasonesCarterasColectivas_Consultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TransferenciasCarterasColectivas_Consultar")
            Return Nothing
        End Try
    End Function

    Public Function TransferenciasCarterasColectivas_Validar(ByVal pdtmFechaProceso As DateTime, ByVal pstrNombreConsecutivo As String,
                                                              ByVal pstrCuentaBancaria As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaProcesosGenericosConfirmacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_TransferenciasCarterasColectivas_Validar(pdtmFechaProceso, pstrNombreConsecutivo, pstrCuentaBancaria, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "TransferenciasCarterasColectivas_Validar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TransferenciasCarterasColectivas_Validar")
            Return Nothing
        End Try
    End Function
#End Region

#Region "Transferencias a cuentas no inscritas"
    Public Sub InsertTransferenciaCuentasNoInscritas(ByVal newTransferenciaCuentasNoInscritas As OyDTesoreria.TransferenciaCuentasNoInscritas)

    End Sub

    Public Sub UpdateTransferenciaCuentasNoInscritas(ByVal currentTransferenciaCuentasNoInscritas As OyDTesoreria.TransferenciaCuentasNoInscritas)

    End Sub

    Public Function TransferenciasACuentasNoInscritas_Consultar(ByVal pstrTipoCuenta As String, ByVal pdtmFechaProceso As DateTime, ByVal pstrTipoBanco As String,
                                                                ByVal pstrCuentaBancaria As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of TransferenciaCuentasNoInscritas)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_TransferenciaCuentasNoInscritas_Consultar(pstrTipoCuenta, pdtmFechaProceso, pstrTipoBanco, pstrCuentaBancaria, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "TransferenciasACuentasNoInscritas_Consultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TransferenciasACuentasNoInscritas_Consultar")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function TransferenciasCarterasColectivas_Anular(ByVal pstrRegistros As String, ByVal pdtmFechaProceso As DateTime, ByVal pstrTipoBanco As String,
                                                              ByVal pstrCuentaBancaria As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaProcesosGenericosConfirmacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_TransferenciaCuentasNoInscritas_Anular(pstrRegistros, pdtmFechaProceso, pstrTipoBanco, pstrCuentaBancaria, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "TransferenciasCarterasColectivas_Anular"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TransferenciasCarterasColectivas_Anular")
            Return Nothing
        End Try
    End Function
#End Region

#Region "PagosPorRedBanco"

#Region "Métodos modelo para activar funcionalidad RIA"

#End Region

#Region "Métodos asincrónicos"

    Public Function PagosPorRedBanco_Consultar(ByVal pstrCuentaBancaria As String, ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of PagosPorRedBanco)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_PagosPorRedBanco_Consultar(pstrCuentaBancaria, pdtmFechaProceso, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "PagosPorRedBanco_Consultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "PagosPorRedBanco_Consultar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function PagosPorRedBanco_ConsultarSync(ByVal pstrCuentaBancaria As String, ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of PagosPorRedBanco)
        Dim objTask As Task(Of List(Of PagosPorRedBanco)) = Me.PagosPorRedBanco_ConsultarAsync(pstrCuentaBancaria, pdtmFechaProceso, pstrUsuario, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function PagosPorRedBanco_ConsultarAsync(ByVal pstrCuentaBancaria As String, ByVal pdtmFechaProceso As System.Nullable(Of System.DateTime), ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of PagosPorRedBanco))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of PagosPorRedBanco)) = New TaskCompletionSource(Of List(Of PagosPorRedBanco))()
        objTaskComplete.TrySetResult(PagosPorRedBanco_Consultar(pstrCuentaBancaria, pdtmFechaProceso, pstrUsuario, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "TrasladosTesoreria_Importacion"

#Region "Métodos modelo para activar funcionalidad RIA"

    Public Sub InsertTrasladosTesoreria_Importacion(ByVal newTrasladosTesoreria_Importacion As OyDTesoreria.TrasladosTesoreria_Importacion)

    End Sub

    Public Sub UpdateTrasladosTesoreria_Importacion(ByVal currentTrasladosTesoreria_Importacion As OyDTesoreria.TrasladosTesoreria_Importacion)

    End Sub

#End Region

#Region "Métodos asincrónicos"

    Public Function TrasladosTesoreria_Consultar(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of TrasladosTesoreria_Importacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspOyDNet_TrasladosTesoreria_Consultar(pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "TrasladosTesoreria_Consultar"), 0).ToList
            Return ret
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TrasladosTesoreria_Consultar")
            Return Nothing
        End Try
    End Function

    <Query(HasSideEffects:=True)>
    Public Function TrasladosTesoreria_Procesar(ByVal pdtmFechaElaboracion As System.Nullable(Of System.DateTime), ByVal pstrFormaPago As String, ByVal pstrConsecutivoCE As String, ByVal pstrConsecutivoRC As String, ByVal pintIDBancoCE As Integer, ByVal pintIDBancoRC As Integer, ByVal pstrCuentaContableCE As String, ByVal pstrCuentaContableRC As String, ByVal pstrRegistrosModificados As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaProcesosGenericosConfirmacion)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim objRet = Me.DataContext.uspOyDNet_TrasladosTesoreria_Procesar(pdtmFechaElaboracion, pstrFormaPago, pstrConsecutivoCE, pstrConsecutivoRC, pintIDBancoCE, pintIDBancoRC, pstrCuentaContableCE, pstrCuentaContableRC, pstrRegistrosModificados, pstrUsuario, pstrMaquina, DemeInfoSesion(pstrUsuario, "TrasladosTesoreria_Procesar"), 0, String.Empty).ToList
            Return objRet
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "TrasladosTesoreria_Procesar")
            Return Nothing
        End Try
    End Function

#End Region

#Region "Métodos sincrónicos"

    Public Function TrasladosTesoreria_ConsultarSync(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of TrasladosTesoreria_Importacion)
        Dim objTask As Task(Of List(Of TrasladosTesoreria_Importacion)) = Me.TrasladosTesoreria_ConsultarAsync(pstrUsuario, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function TrasladosTesoreria_ConsultarAsync(ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of TrasladosTesoreria_Importacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of TrasladosTesoreria_Importacion)) = New TaskCompletionSource(Of List(Of TrasladosTesoreria_Importacion))()
        objTaskComplete.TrySetResult(TrasladosTesoreria_Consultar(pstrUsuario, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

    <Query(HasSideEffects:=True)>
    Public Function TrasladosTesoreria_ProcesarSync(ByVal pdtmFechaElaboracion As System.Nullable(Of System.DateTime), ByVal pstrFormaPago As String, ByVal pstrConsecutivoCE As String, ByVal pstrConsecutivoRC As String, ByVal pintIDBancoCE As Integer, ByVal pintIDBancoRC As Integer, ByVal pstrCuentaContableCE As String, ByVal pstrCuentaContableRC As String, ByVal pstrRegistrosModificados As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As List(Of RespuestaProcesosGenericosConfirmacion)
        Dim objTask As Task(Of List(Of RespuestaProcesosGenericosConfirmacion)) = Me.TrasladosTesoreria_ProcesarAsync(pdtmFechaElaboracion, pstrFormaPago, pstrConsecutivoCE, pstrConsecutivoRC, pintIDBancoCE, pintIDBancoRC, pstrCuentaContableCE, pstrCuentaContableRC, pstrRegistrosModificados, pstrUsuario, pstrMaquina, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function TrasladosTesoreria_ProcesarAsync(ByVal pdtmFechaElaboracion As System.Nullable(Of System.DateTime), ByVal pstrFormaPago As String, ByVal pstrConsecutivoCE As String, ByVal pstrConsecutivoRC As String, ByVal pintIDBancoCE As Integer, ByVal pintIDBancoRC As Integer, ByVal pstrCuentaContableCE As String, ByVal pstrCuentaContableRC As String, ByVal pstrRegistrosModificados As String, ByVal pstrUsuario As String, ByVal pstrMaquina As String, ByVal pstrInfoConexion As String) As Task(Of List(Of RespuestaProcesosGenericosConfirmacion))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of RespuestaProcesosGenericosConfirmacion)) = New TaskCompletionSource(Of List(Of RespuestaProcesosGenericosConfirmacion))()
        objTaskComplete.TrySetResult(TrasladosTesoreria_Procesar(pdtmFechaElaboracion, pstrFormaPago, pstrConsecutivoCE, pstrConsecutivoRC, pintIDBancoCE, pintIDBancoRC, pstrCuentaContableCE, pstrCuentaContableRC, pstrRegistrosModificados, pstrUsuario, pstrMaquina, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

#End Region

#Region "Portafolios"

#Region "Métodos asincrónicos"
    Public Function ConsultarDatosPortafolio(ByVal plngIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DatosPortafolios)
        Try
            ValidacionSeguridadUsuario(Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_NoValidar, Me.ToString, System.Reflection.MethodInfo.GetCurrentMethod().Name, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.dbOYDConnectionString,pstrUsuario, pstrInfoConexion, Global.A2.OyD.OYDServer.RIA.Web.My.MySettings.Default.Seguridad_TiempoLlamado)
            Dim ret = Me.DataContext.uspCalculosFinancieros_DatosPortafolio_Consultar(plngIDComitente, pstrUsuario, DemeInfoSesion(pstrUsuario, "DatosPortafolio"), 0).ToList
            Return ret
            Exit Function
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "DatosPortafolio")
            Return Nothing
        End Try
    End Function
#End Region

    Public Function ConsultarDatosPortafolioSync(ByVal plngIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As List(Of DatosPortafolios)
        Dim objTask As Task(Of List(Of DatosPortafolios)) = Me.ConsultarDatosPortafoliosAsync(plngIDComitente, pstrUsuario, pstrInfoConexion)
        objTask.Wait()
        Return objTask.Result
    End Function
    Private Function ConsultarDatosPortafoliosAsync(ByVal plngIDComitente As String, ByVal pstrUsuario As String, ByVal pstrInfoConexion As String) As Task(Of List(Of DatosPortafolios))
        Dim objTaskComplete As TaskCompletionSource(Of List(Of DatosPortafolios)) = New TaskCompletionSource(Of List(Of DatosPortafolios))()
        objTaskComplete.TrySetResult(ConsultarDatosPortafolio(plngIDComitente, pstrUsuario, pstrInfoConexion))
        Return (objTaskComplete.Task)
    End Function

#End Region

End Class