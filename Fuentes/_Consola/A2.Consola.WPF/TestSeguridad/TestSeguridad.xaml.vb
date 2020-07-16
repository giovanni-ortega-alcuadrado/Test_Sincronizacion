Imports A2Utilidades

Partial Public Class TestSeguridad
    Inherits Window

    Public Sub New(ByVal pstrUsuario As String, ByVal pstrIPCliente As String)
        InitializeComponent()
        txtUsuarioCliente.Text = pstrUsuario
        txtIPCliente.Text = pstrIPCliente
    End Sub

    Private Async Sub btnValidarSeguridad_Click(sender As Object, e As RoutedEventArgs)
        Try
            ctlBusy.IsBusy = True
            txtRespuestaSeguridad.Text = String.Empty
            Dim objCtl As A2ComunesControl.ParametrosConsola
            objCtl = New A2ComunesControl.ParametrosConsola
            txtRespuestaSeguridad.Text = Await objCtl.ValidarSeguridadCliente()
            ctlBusy.IsBusy = False
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, String.Format("Se presento un error al validar la información.", Program.Aplicacion, Program.VersionAplicacion), Me.Name, "btnValidarSeguridad_Click", Program.ConsolaTitulo, Program.Maquina, ex)
            ctlBusy.IsBusy = False
        End Try
    End Sub

    Private Async Sub btnValidarIP_Click(sender As Object, e As RoutedEventArgs)
        Try
            ctlBusy.IsBusy = True
            txtRespuestaIP.Text = String.Empty
            Dim objCtl As A2ComunesControl.ParametrosConsola
            objCtl = New A2ComunesControl.ParametrosConsola
            txtRespuestaIP.Text = Await objCtl.ValidarIPCliente()
            ctlBusy.IsBusy = False
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, String.Format("Se presento un error al validar la información.", Program.Aplicacion, Program.VersionAplicacion), Me.Name, "btnValidarIP_Click", Program.ConsolaTitulo, Program.Maquina, ex)
            ctlBusy.IsBusy = False
        End Try
    End Sub

    Private Async Sub btnValidarUsuario_Click(sender As Object, e As RoutedEventArgs)
        Try
            ctlBusy.IsBusy = True
            txtRespuestaUsuario.Text = String.Empty
            Dim objCtl As A2ComunesControl.ParametrosConsola
            objCtl = New A2ComunesControl.ParametrosConsola
            txtRespuestaUsuario.Text = Await objCtl.ValidarUsuarioCliente()
            ctlBusy.IsBusy = False
        Catch ex As Exception
            Mensajes.mostrarErrorAplicacion(Program.Usuario, String.Format("Se presento un error al validar la información.", Program.Aplicacion, Program.VersionAplicacion), Me.Name, "btnValidarUsuario_Click", Program.ConsolaTitulo, Program.Maquina, ex)
            ctlBusy.IsBusy = False
        End Try
    End Sub
End Class
