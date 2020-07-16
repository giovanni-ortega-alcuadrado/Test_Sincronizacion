Imports Telerik.Windows.Controls
Imports A2Utilidades
Imports System.Reflection
Imports System.ComponentModel.DataAnnotations

Public Class FuncionesCompartidas

#Region "Constantes Aplicación"
    ' Nombre claves de parámetros que están en la lista de parámetros de la aplicación
    Public Const CLAVE_PARAM_EJECUTAR_SEGUN_AMB As String = "EJECUTAR_APP_SEGUN_AMBIENTE"
#End Region


#Region "Métodos públicos que retornan valor"
    Public Shared Function ObtenerColumnasDelRango(ubicacionRango As String) As String
        Dim columnas As String = String.Empty
        columnas = (ubicacionRango.Split(":")(0)).Split("$")(1) + "," + (ubicacionRango.Split(":")(1)).Split("$")(1)
        Return columnas
    End Function


    ''' <summary>
    ''' Valdar si un string enviado en el primer parámetro es nothing o vacio y retornar el valor enviado en el segundo parámetro
    ''' </summary>
    Public Shared Function validarValorString(ByVal pstrValor As String, ByVal pstrRetornoNothingOVacio As String) As String
        Dim strRetorno As String = String.Empty

        If String.IsNullOrEmpty(pstrValor) Then
            strRetorno = pstrRetornoNothingOVacio
        Else
            strRetorno = pstrValor
        End If

        Return strRetorno
    End Function

    ''' <summary>
    ''' Separador de las inconsistencias reportadas desde el servidor.
    ''' </summary>
    ''' 
    Public Shared Function SeparadorInconsistencias() As String
        Return ("|")
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

        If (pstrMsg) IsNot Nothing AndAlso Not pstrMsg.Equals(String.Empty) Then

            strMsg = "Se presentaron las siguientes inconsistencias al "

            If plogError Then
                strValidarTipo = pstrMsg.Substring(pstrMsg.Length - IDENTIFICADOR_VALIDACION_A2.Length()).ToLower
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
                    Environment.NewLine & Environment.NewLine & "* "

            strMsg &= pstrMsg.Replace(SeparadorInconsistencias(), Environment.NewLine & "* ")
        End If

        Return (strMsg)
    End Function

    ''' <summary>
    ''' Ruta del servicio web RIA que expone la funcionalidad de negocio para un módulo específico del sistema
    ''' </summary> 
    Public Shared Function RutaServicioNegocio(ByVal pstrTipoServicio As String) As String
        If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
            If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(pstrTipoServicio) Then
                Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(pstrTipoServicio).ToString()
            Else
                Return ("")
            End If
        Else
            Return ("")
        End If
    End Function

    ''' <summary>
    ''' Indica si al iniciar los controles en ambiente de desarrollo se ejecuta el proxy con el URL por defecto de los servicios web RIA, lo cual permite
    ''' hacer debug hasta el Domain service o si forza el sistema para que aunque se esté en debug se tome el URL explicito del servicio web.
    ''' </summary>
    Public Shared Function ejecutarAppSegunAmbiente() As Boolean
        If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
            If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(CLAVE_PARAM_EJECUTAR_SEGUN_AMB) Then
                Return CType(CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(CLAVE_PARAM_EJECUTAR_SEGUN_AMB), Boolean)
            Else
                Return (True)
            End If
        Else
            Return (True)
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

            If tmpInformation IsNot Nothing Then
                If (tmpInformation.OfType(Of EditableAttribute)()) IsNot Nothing Then
                    If tmpInformation.OfType(Of EditableAttribute).Count > 0 Then
                        If tmpInformation.OfType(Of EditableAttribute).First.AllowEdit = False Then
                            Continue For
                        End If
                    End If
                End If
                If (tmpInformation.OfType(Of KeyAttribute)()) IsNot Nothing Then
                    If tmpInformation.OfType(Of KeyAttribute).Count > 0 Then
                        Continue For
                    End If
                End If
                If (tmpInformation.OfType(Of AssociationAttribute)()) IsNot Nothing Then
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
