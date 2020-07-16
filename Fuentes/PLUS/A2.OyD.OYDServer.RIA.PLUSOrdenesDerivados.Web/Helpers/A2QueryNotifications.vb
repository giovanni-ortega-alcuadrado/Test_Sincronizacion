Imports System.Threading
Imports System.Threading.Tasks
Imports System.Data.SqlClient
Imports System.Xml
Imports System.Security.Permissions
Imports System.Data.Sql

Public NotInheritable Class A2SBQueryNotifications(Of ResponseSB)

    Private ctsQNTask As CancellationTokenSource

    Private _result As ResponseSB
    Public ReadOnly Property Result() As ResponseSB
        Get
            Return _result
        End Get
    End Property

    Public ReadOnly Property HasErrors() As Boolean
        Get
            Return IIf(Exception IsNot Nothing, True, False)
        End Get
    End Property

    Private _error As Exception
    Public ReadOnly Property Exception() As Exception
        Get
            Return _error
        End Get
    End Property

    Private _IdConversation As Guid
    Public ReadOnly Property IdConversation() As Guid
        Get
            Return _IdConversation
        End Get
    End Property


    Public Sub GetResponse(ByVal IdConversation As Guid)
        _IdConversation = IdConversation
        'creo la tarea
        ctsQNTask = New CancellationTokenSource()
        Dim cancellationToken As CancellationToken = ctsQNTask.Token
        Dim GetSBDataTask As Task = Task.Factory.StartNew(AddressOf GetSBData, cancellationToken)
        Try
            'espero que la notificacion responda
            cancellationToken.WaitHandle.WaitOne()
            GetSBDataTask.Wait()
        Catch ex As Exception
            'timeout o cancel en el hilo que espera la respuesta
            Debug.WriteLine("Ocurrio un Problema con la ejecucion de la Tarea")
            SetError(ex)
        End Try
    End Sub

    Private Function GetSBData(Optional ByVal Suscribe As Boolean = True) As ResponseSB
        Dim ctx As OyDPLUSOrdenesDerivadosDataContext = New OyDPLUSOrdenesDerivadosDataContext(Utilidades.ObtenerCadenaConexionUtilidades())
        Dim query = From u In ctx.MensajesProcesados Where u.IDConversacion = IdConversation Select u
        Dim sqlcmd As SqlCommand = CType(ctx.GetCommand(query), SqlCommand)
        'luego de recibir a llamada no es necesario volver a suscribir la notificacion
        If Suscribe Then
            If UserHaveQNPermission() Then
                If Not StartSqlDependency(sqlcmd) Then
                    Return Nothing
                End If
            Else
                SetError(New InvalidOperationException("El usuario no tiene permisos para la suscripcion a eventos"))
                Return Nothing
            End If
        End If
        Try
            Dim notficationRequest As New SqlNotificationRequest()
            notficationRequest.Options = "Service=[http://www.alcuadrado.com/SSB/ServicioNotificaciones]"
            notficationRequest.UserData = IdConversation.ToString()
            notficationRequest.Timeout = Utilidades.ObtenerTimeUpUtilidades
            sqlcmd.Notification = notficationRequest

            sqlcmd.Connection.Open()
            Dim result As SqlDataReader = sqlcmd.ExecuteReader()
            If result.HasRows Then
                result.Read()
                'convierto el mensaje de XML a Objecto
                _result = IIf(result("Mensaje") IsNot Nothing, result("Mensaje"), String.Empty)
            End If

            ctsQNTask.Cancel()
        Catch ex As Exception
            Debug.WriteLine("Ocurrio un Problema con la ejecucion y/o transformacion de la consulta")
            SetError(ex)
        Finally
            If sqlcmd.Connection.State = ConnectionState.Open Then
                sqlcmd.Connection.Close()
            End If
        End Try
    End Function

    Private Function StartSqlDependency(ByVal sqlcmd As SqlCommand) As Boolean
        Try
            'configuro la notificacion al command
            sqlcmd.Notification = Nothing
            Dim dependency As New System.Data.SqlClient.SqlDependency(sqlcmd, Nothing, Utilidades.ObtenerTimeUpUtilidades)
            AddHandler dependency.OnChange, AddressOf GetSBData_Changed

            'registro la notificacion
            SqlDependency.Stop(Utilidades.ObtenerCadenaConexionUtilidades(), "NotificacionesSSBQueue")
            SqlDependency.Start(Utilidades.ObtenerCadenaConexionUtilidades(), "NotificacionesSSBQueue")
            Return True
        Catch ex As Exception
            Debug.WriteLine("Ocurrio un Problema al registrar la notificacion")
            SetError(ex)
            Return False
        End Try
    End Function

    Private Function UserHaveQNPermission() As Boolean
        Try
            Dim clientPermission As New SqlClientPermission(PermissionState.Unrestricted)
            clientPermission.Demand()
            Return True
        Catch
            Return False
        End Try
    End Function


    Private Sub GetSBData_Changed(ByVal sender As Object, ByVal e As SqlNotificationEventArgs)
        'elimino el delegado
        Dim dependency As SqlDependency = DirectCast(sender, SqlDependency)
        RemoveHandler dependency.OnChange, AddressOf GetSBData_Changed
        If e.Type = SqlNotificationType.Change AndAlso e.Source = SqlNotificationSource.Data _
            AndAlso e.Info = SqlNotificationInfo.Insert Then
            'recibo la notificacion
            GetSBData(False)
        Else
            'log fail
            SetError(New Exception(String.Format("Ocurrio un problema con el Sistema de Notificacion de Eventos - Type: {0}, Source: {1}, Info: {2}", _
                                          e.Type.ToString(), e.Source.ToString(), e.Info.ToString())))
        End If
    End Sub

    Private Sub SetError(ByVal ex As Exception)
        'establezco el error y cancelo la tarea para continuar con la ejecucion
        _error = ex
        ctsQNTask.Cancel()
        Debug.WriteLine(ex.ToString())
    End Sub

End Class
