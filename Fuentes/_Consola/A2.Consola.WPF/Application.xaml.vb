Imports System.Configuration
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Security.Principal
Imports System.Web
Imports System.Windows.Threading
Imports A2Utilidades
Imports A2Utilidades.Recursos

Class Application

    Private mlogErrorDocking As Boolean = False
    Public Shared mobjParametrosStartup As StartupEventArgs

    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.

    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.
    Protected Overrides Sub OnStartup(e As StartupEventArgs)
        Try
            MyBase.OnStartup(e)
            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal)
            mobjParametrosStartup = e

            'CODIGO PARA EVITAR QUE LA APLICACIÓN SE CIERRE SI SE GENERA UN ERROR
            AddHandler Application.Current.DispatcherUnhandledException, AddressOf AppDispatcherUnhandledException
        Catch ex As Exception
            MessageBox.Show("Error inicializando la aplicación" & vbNewLine & vbNewLine & ex.Message)
        End Try
    End Sub

    'CODIGO PARA EVITAR QUE LA APLICACIÓN SE CIERRE SI SE GENERA UN ERROR
    Private Sub AppDispatcherUnhandledException(ByVal sender As Object, ByVal e As DispatcherUnhandledExceptionEventArgs)
        Try
            e.Handled = True
            If Application.Current.Resources.Contains(Recursos.RecursosMensajes.A2Consola_UltimoMensaje_Error.ToString) Then
                Dim objMensajeGenerado As Exception = CType(Application.Current.Resources(Recursos.RecursosMensajes.A2Consola_UltimoMensaje_Error.ToString), Exception)

                Application.Current.Resources.Remove(Recursos.RecursosMensajes.A2Consola_UltimoMensaje_Error.ToString)

                If Not IsNothing(objMensajeGenerado) Then
                    If Not IsNothing(e.Exception) Then
                        If objMensajeGenerado.Message <> e.Exception.Message Then
                            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un error dentro de la aplicación.", Me.ToString(), "AppDispatcherUnhandledException", "", Program.Maquina, e.Exception, Program.RutaServicioLog)
                        End If
                    End If
                End If
            Else
                If Not IsNothing(e.Exception) Then
                    A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un error dentro de la aplicación.", Me.ToString(), "AppDispatcherUnhandledException", "", Program.Maquina, e.Exception, Program.RutaServicioLog)
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se generó un error al mostrar el error no controlado.", Me.ToString(), "AppDispatcherUnhandledException", "", Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

End Class
