Imports A2Utilidades

Public Class clsEnvioCorreo
    ''' <summary>
    ''' Proceso para el envío de correo genérico de la aplicación
    ''' </summary>
    ''' <param name="strAsunto"></param>
    ''' <param name="strCorreos"></param>
    ''' <param name="strMensajeCorreo"></param>
    Public Sub EnviarCorreo(strAsunto As String, strCorreos As String, strMensajeCorreo As String)

        If Application.Current.Resources("EMAIL_ENVIO_ACTIVO") = "SI" And Not String.IsNullOrEmpty(strCorreos) Then

            Dim smtp As System.Net.Mail.SmtpClient = New System.Net.Mail.SmtpClient()
            Dim correo As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()

            If Not String.IsNullOrEmpty(Application.Current.Resources("EMAIL_HOST_SMTP").ToString()) Then
                smtp.Host = Application.Current.Resources("EMAIL_HOST_SMTP")
            Else
                MessageBox.Show("Error en envío de correo. No se obtuvo el valor del parámetro EMAIL_HOST_SMTP")
                Exit Sub
            End If

            If Not String.IsNullOrEmpty(Application.Current.Resources("EMAIL_USUARIO_REMITENTE").ToString()) Then
                Dim strClave As String = String.Empty

                If Not String.IsNullOrEmpty(Application.Current.Resources("EMAIL_CLAVE_REMITENTE").ToString()) Then
                    strClave = Cifrar.descifrar(Application.Current.Resources("EMAIL_CLAVE_REMITENTE").ToString())
                End If

                smtp.Credentials = New System.Net.NetworkCredential(Application.Current.Resources("EMAIL_USUARIO_REMITENTE").ToString, strClave)
                correo.From = New System.Net.Mail.MailAddress(Application.Current.Resources("EMAIL_USUARIO_REMITENTE").ToString)
            Else
                MessageBox.Show("Error en envío de correo. No se obtuvo el valor del parámetro EMAIL_USUARIO_REMITENTE")
                Exit Sub
            End If

            If Not IsNothing(Application.Current.Resources("EMAIL_CONEXION_SSL")) AndAlso Application.Current.Resources("EMAIL_CONEXION_SSL") = "SI" Then
                smtp.EnableSsl = True
            Else
                smtp.EnableSsl = False
            End If

            If Not String.IsNullOrEmpty(Application.Current.Resources("EMAIL_PUERTO_SMTP").ToString()) Then
                smtp.Port = Application.Current.Resources("EMAIL_PUERTO_SMTP")
            Else
                MessageBox.Show("Error en envío de correo. No se obtuvo el valor del parámetro EMAIL_PUERTO_SMTP")
                Exit Sub
            End If

            Dim lstCorreos As String() = strCorreos.Split(";")

            correo.Subject = strAsunto
            correo.IsBodyHtml = True

            For Each strCorreo In lstCorreos
                correo.[To].Add(strCorreo)
            Next

            correo.Body = strMensajeCorreo
            smtp.Send(correo)

        End If

    End Sub

    ''' <summary>
    ''' JAPC20200211 C-20190368 Proceso para preparar el correo desabrobacion de la orden
    ''' </summary>
    ''' <param name="strAsunto"></param>
    ''' <param name="strCorreos"></param>
    ''' <param name="strNombreAsesor"></param>
    ''' <param name="strNumeroOrden"></param>
    ''' <param name="strTipo"></param>
    ''' <param name="strNombreUsuario"></param>
    ''' <param name="strNombreUsuarioDevolucion"></param>
    ''' <param name="strObservacion"></param>
    ''' <param name="strCliente"></param>
    ''' <param name="strCantidad"></param>
    ''' <param name="strPrecio"></param>
    ''' <param name="dtmFecha"></param>
    ''' <param name="dtmFechaHoraCreacion"></param>
    Public Sub PrepararCorreoEvento(strAsunto As String,
                                  strCorreos As String,
                                  strNombreAsesor As String,
                                  strNumeroOrden As String,
                                  strTipo As String,
                                  strNombreUsuario As String,
                                  strNombreUsuarioDevolucion As String,
                                  strObservacion As String,
                                  strCliente As String,
                                  strCantidad As String,
                                  strPrecio As String,
                                  dtmFecha As DateTime?,
                                  dtmFechaHoraCreacion As DateTime?)

        Dim strMensajePlantilla As String

        If Not String.IsNullOrEmpty(strCorreos) Then

            'If Not String.IsNullOrEmpty(Application.Current.Resources("EMAIL_MENSAJE_EVENTO")) Then
            '    strMensajePlantilla = Application.Current.Resources("EMAIL_MENSAJE_EVENTO")
            'Else
            '    MessageBox.Show("Error al preparar el correo del evento. No se obtuvo el valor del parámetro EMAIL_MENSAJE_EVENTO")
            '    Exit Sub
            'End If

            If Not String.IsNullOrEmpty(My.Settings.EMAIL_MENSAJE_EVENTO) Then
                strMensajePlantilla = My.Settings.EMAIL_MENSAJE_EVENTO
            Else
                MessageBox.Show("Error al preparar el correo del evento. No se obtuvo el valor del parámetro EMAIL_MENSAJE_EVENTO")
                Exit Sub
            End If

            strMensajePlantilla = Replace(strMensajePlantilla, "[NOMBREASESOR]", strNombreAsesor)
            strMensajePlantilla = Replace(strMensajePlantilla, "[NUMEROORDEN]", strNumeroOrden)
            strMensajePlantilla = Replace(strMensajePlantilla, "[TIPO]", strTipo)
            strMensajePlantilla = Replace(strMensajePlantilla, "[NOMBREUSUARIO]", strNombreUsuario)
            strMensajePlantilla = Replace(strMensajePlantilla, "[NOMBREUSUARIODEVOLUCION]", strNombreUsuarioDevolucion)
            strMensajePlantilla = Replace(strMensajePlantilla, "[OBSERVACION]", strObservacion)
            strMensajePlantilla = Replace(strMensajePlantilla, "[CLIENTE]", strCliente)
            strMensajePlantilla = Replace(strMensajePlantilla, "[CANTIDAD]", strCantidad)
            strMensajePlantilla = Replace(strMensajePlantilla, "[PRECIO]", strPrecio)
            strMensajePlantilla = Replace(strMensajePlantilla, "[FECHA]", dtmFecha)
            strMensajePlantilla = Replace(strMensajePlantilla, "[HORACREACION]", dtmFechaHoraCreacion)

            EnviarCorreo(strAsunto, strCorreos, strMensajePlantilla)
        End If

    End Sub

    ''' <summary>
    ''' Proceso para preparar el correo de eliminación de un evento en el calendario
    ''' </summary>
    ''' <param name="strAsunto"></param>
    ''' <param name="strCorreos"></param>
    ''' <param name="strTema"></param>
    ''' <param name="strDescripcion"></param>
    Public Sub PrepararCorreoEventoEliminado(strAsunto As String,
                                  strCorreos As String,
                                  strTema As String,
                                  strDescripcion As String)

        Dim strMensajePlantilla As String

        If Not String.IsNullOrEmpty(strCorreos) Then

            If Not String.IsNullOrEmpty(Application.Current.Resources("EMAIL_MENSAJE_EVENTO_ELIMINADO")) Then
                strMensajePlantilla = Application.Current.Resources("EMAIL_MENSAJE_EVENTO_ELIMINADO")
            Else
                MessageBox.Show("Error al preparar el correo del evento eliminado. No se obtuvo el valor del parámetro EMAIL_MENSAJE_EVENTO_ELIMINADO")
                Exit Sub
            End If

            strMensajePlantilla = Replace(strMensajePlantilla, "[TEMA]", strTema)
            strMensajePlantilla = Replace(strMensajePlantilla, "[DESCRIPCION]", strDescripcion)

            EnviarCorreo(strAsunto, strCorreos, strMensajePlantilla)
        End If

    End Sub
End Class
