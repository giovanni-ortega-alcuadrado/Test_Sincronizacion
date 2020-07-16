Imports A2Utilidades
Imports System.Reflection
Imports System.ComponentModel.DataAnnotations

Public Class FuncionesCompartidas

#Region "Constantes Aplicación"
    ' Nombre claves de parámetros que están en la lista de parámetros de la aplicación
    Public Const CLAVE_PARAM_APP_RUTA_SERV_UTIL As String = "URLServicioUtilidadesOYD"
    Public Const CLAVE_PARAM_EJECUTAR_SEGUN_AMB As String = "EJECUTAR_APP_SEGUN_AMBIENTE"

    ' Nombre listas de combos
    Public Const NOMBRE_LISTA_COMBOS_APP As String = "CalculosFinancierosLstCombosGen"
    Public Const NOMBRE_LISTA_COMBOSESPECIFICOS_APP As String = "CalculosFinancierosLstCombosEsp"

    Public Const STR_TIMEUP_PROXY As String = "TIMEUP_PROXY"

#End Region

#Region "Métodos públicos que retornan valor"

    ''' <summary>
    ''' Separador de las inconsistencias reportadas desde el servidor.
    ''' </summary>
    ''' 
    Public Shared Function SeparadorInconsistencias() As String
        Return ("|")
    End Function

    ''' <summary>
    ''' Verificar si se debe o no mostrar un mensaje (popup) que permite hacer seguimiento al ejecutar la aplicación.
    ''' </summary>
    ''' 
    Public Shared Function mostrarMensaje() As Boolean
        Dim logMostrarMensaje As Boolean
        Try
            If Program.MostrarMensajeLog.Trim.Equals("1") Then
                logMostrarMensaje = True
            Else
                logMostrarMensaje = False
            End If
        Catch ex As Exception
            logMostrarMensaje = False
        End Try
        Return (logMostrarMensaje)
    End Function

    ''' <summary>
    ''' Ruta del servicio web RIA que expone la funcionalidad de negocio para un módulo específico del sistema
    ''' </summary>
    ''' 
    Public Shared Function RutaServicioNegocio(ByVal pstrTipoServicio As String) As String
        If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
            If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(pstrTipoServicio) Then
                Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(pstrTipoServicio).ToString()
            Else
                Return (clsRecursos.SimularURL_ServiciosRIA(pstrTipoServicio))
            End If
        Else
            Return (clsRecursos.SimularURL_ServiciosRIA(pstrTipoServicio))
        End If
    End Function

    ''' <summary>
    ''' Ruta del servicio web RIA que expone la funcionalidad de utilidades generales del aplicativo
    ''' </summary>
    ''' 
    Public Shared Function RutaServicioUtilidades() As String
        If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
            If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(CLAVE_PARAM_APP_RUTA_SERV_UTIL) Then
                Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(CLAVE_PARAM_APP_RUTA_SERV_UTIL).ToString()
            Else
                Return (clsRecursos.SimularURL_ServiciosRIA(CLAVE_PARAM_APP_RUTA_SERV_UTIL))
            End If
        Else
            Return (clsRecursos.SimularURL_ServiciosRIA(CLAVE_PARAM_APP_RUTA_SERV_UTIL))
        End If
    End Function

    ''' <summary>
    ''' Indica si al iniciar los controles en ambiente de desarrollo se ejecuta el proxy con el URL por defecto de los servicios web RIA, lo cual permite
    ''' hacer debug hasta el Domain service o si forza el sistema para que aunque se esté en debug se tome el URL explicito del servicio web.
    ''' </summary>
    ''' 
    Public Shared Function ejecutarAppSegunAmbiente() As Boolean
        If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
            If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(CLAVE_PARAM_EJECUTAR_SEGUN_AMB) Then
                'Return CType(CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(CLAVE_PARAM_EJECUTAR_SEGUN_AMB), Boolean)
                Return False
            Else
                Return (False)
            End If
        Else
            Return (False)
        End If
    End Function

    ''' <summary>
    ''' Valdar si un string enviado en el primer parámetro es nothing o vacio y retornar el valor enviado en el segundo parámetro
    ''' </summary>
    ''' 
    Public Shared Function validarValorString(ByVal pstrValor As String, ByVal pstrRetornoNothingOVacio As String) As String
        Dim strRetorno As String = String.Empty

        If String.IsNullOrEmpty(pstrValor) Then
            strRetorno = pstrRetornoNothingOVacio
        Else
            strRetorno = pstrValor
        End If

        Return strRetorno
    End Function

    Public Shared Function MensajeEsperaOperacion() As String
        Return ("El sistema está finalizando un proceso necesarios para iniciar la edición de los datos. Por favor espere un momento y vuelva a dar clic en el botón para ejecutar nuevamente.")
    End Function

    ''' <summary>
    ''' Retornar un texto que contiene los mensajes de error o validación generados de acuerdo con la estructura definida:
    ''' Msg1|Msg2|Msg3 ... y retorna [Encabezado]Msg1[Salto línea]Msg2 ...
    ''' </summary>
    ''' 
    Public Shared Function obtenerMensajeValidacion(ByVal pstrMsg As String, ByVal pstrAccion As String, ByVal plogError As Boolean) As String
        Const IDENTIFICADOR_VALIDACION_A2 As String = "validaciona2"

        Dim strMsg As String = String.Empty
        Dim strValidarTipo As String = String.Empty

        If Not IsNothing(pstrMsg) AndAlso Not pstrMsg.Equals(String.Empty) Then

            strMsg = "Se presentaron las siguientes inconsistencias al "

            If plogError Then
                strValidarTipo = Right(pstrMsg, IDENTIFICADOR_VALIDACION_A2.Length()).ToLower
                If strValidarTipo.Equals(IDENTIFICADOR_VALIDACION_A2) Then
                    If pstrMsg.ToLower.IndexOf("innerexception message") > 0 Then
                        pstrMsg = pstrMsg.Substring(0, pstrMsg.Length - ("InnerException message: " & IDENTIFICADOR_VALIDACION_A2).Length)
                    Else
                        pstrMsg = pstrMsg.Substring(0, pstrMsg.Length - ("Mensaje de InnerException: " & IDENTIFICADOR_VALIDACION_A2).Length)
                    End If
                    pstrMsg = pstrMsg.Substring("Submit operation failed. ".Length)
                Else
                    strMsg = "Se presentó el siguiente problema al "
                End If
            End If

            strMsg &= pstrAccion.ToLower() &
                    " los datos:" &
                    vbNewLine & vbNewLine & "* "

            strMsg &= pstrMsg.Replace(SeparadorInconsistencias(), vbNewLine & "* ")
        End If

        Return (strMsg)
    End Function

    Public Shared Function obtenerMensajeValidacionErrorPersonalizado(ByVal pstrMensajeError As String, ByVal pstrModulo As String, ByVal pstrProcedimiento As String, ByVal pobjEX As Exception) As Boolean
        If (pobjEX.Message.Contains("ErrorPersonalizado,") = True) Then
            Dim MensajesRetornoProcedimiento = Split(pobjEX.Message, "ErrorPersonalizado,")
            Dim MensajesPersonalizados = Split(MensajesRetornoProcedimiento(1), String.Format("| -"))
            Dim MensajesValidacion = Split(MensajesPersonalizados(0), "|")
            Dim MensajeMostrar As String = String.Empty

            For Each objMensaje In MensajesValidacion
                If String.IsNullOrEmpty(MensajeMostrar) Then
                    MensajeMostrar = objMensaje
                Else
                    MensajeMostrar = String.Format("{0}{1}{2}", MensajeMostrar, vbCrLf, objMensaje)
                End If
            Next

            A2Utilidades.Mensajes.mostrarMensaje(MensajeMostrar, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)

            Return True
        Else
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, pstrMensajeError, pstrModulo, pstrProcedimiento, Application.Current.ToString(), Program.Maquina, pobjEX)
            Return False
        End If
    End Function

    Public Shared Function TimeUpProxy() As Integer
        If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
            If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(STR_TIMEUP_PROXY) Then
                Dim objRetorno = CInt(CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(STR_TIMEUP_PROXY))
                If objRetorno <= 0 Then objRetorno = 180
                Return objRetorno
            Else
                Return (180)
            End If
        Else
            Return (180)
        End If
    End Function

#End Region

#Region "Métodos sin retorno"
    ''' <summary>
    ''' Copiar un objeto a otro propiedad por propiedad
    ''' </summary>
    ''' 
    Public Shared Sub CopyObject(Of T)(from As T, [to] As T)
        Dim tto As Type = GetType(T)
        Dim pFrom As PropertyInfo() = tto.GetProperties()

        For Each p As PropertyInfo In pFrom
            Dim tmp As PropertyInfo = tto.GetProperty(p.Name)
            Dim tmpInformation = p.GetCustomAttributes(True)
            'Dim tmpInformation As Object() = p.GetCustomAttributes(GetType(AssociationAttribute), False)

            If tmp Is Nothing Or tmpInformation.IsReadOnly Then
                Continue For
            End If

            If tmp.Name = "ValidationErrors" Or tmp.Name = "HasValidationErrors" Or tmp.Name = "EntityState" Or tmp.Name = "HasChanges" Or tmp.Name = "IsReadOnly" Or tmp.Name = "EntityActions" Or tmp.Name = "EntityConflict" Then
                Continue For
            End If

            If Not IsNothing(tmpInformation) Then
                If Not IsNothing(tmpInformation.OfType(Of EditableAttribute)) Then
                    If tmpInformation.OfType(Of EditableAttribute).Count > 0 Then
                        If tmpInformation.OfType(Of EditableAttribute).First.AllowEdit = False Then
                            Continue For
                        End If
                    End If
                End If
                If Not IsNothing(tmpInformation.OfType(Of KeyAttribute)) Then
                    If tmpInformation.OfType(Of KeyAttribute).Count > 0 Then
                        Continue For
                    End If
                End If
                If Not IsNothing(tmpInformation.OfType(Of AssociationAttribute)) Then
                    If tmpInformation.OfType(Of AssociationAttribute).Count > 0 Then
                        Continue For
                    End If
                End If
            End If

            Try
                tmp.SetValue([to], p.GetValue(from, Nothing), Nothing)
            Catch ex As Exception
                Throw New System.Exception("Error al copiar el objeto: " & ex.Message)
            End Try
        Next
    End Sub
#End Region

End Class
