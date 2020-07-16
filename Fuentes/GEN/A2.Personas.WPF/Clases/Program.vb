Imports A2Ctl = A2ComunesControl
Imports A2Utilidades
Imports System.Reflection
Imports System.ComponentModel.DataAnnotations

'''----------------------------------------------------------------------------------------------------------------------------
''' Objeto global para almacenar la información común al sistema
''' 
Friend Class Program
    Inherits A2Utilidades.Program

#Region "Constantes públicas específicas"
    '''----------------------------------------------------------------------------------------------------------------------------
    ''' Declaración de constantes globales
    ''' 
    Friend Const NOMBRE_DICCIONARIO_ACTUALIZARCOMBOSESPECIFICOS As String = "ActualizarDiccionarioCombosEspecificos"
    Friend Const NOMBRE_DICCIONARIO_COMBOSESPECIFICOS_ITEMS As String = "Items_DiccionarioCombosEspecificos"

    Friend Enum TipoServicio
		URLServicioPersonas
	End Enum

#End Region

#Region "Métodos públicos genéricos"

    Public Overloads Shared ReadOnly Property Usuario As String
        Get
            If Program.UsaUsuarioSinDomino Then
                Return Program.UsuarioSinDomino
            Else
                Return A2Utilidades.Program.Usuario
            End If
        End Get
    End Property

    ''' <summary>
    ''' Ruta del servicio web RIA que expone la funcionalidad de negocio para un módulo específico del sistema
    ''' </summary>
    ''' 
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
    ''' Valdar si un string enviado en el primer parámetro es nothing o vacio y retornar el valor enviado en el segundo parámetro
    ''' </summary>
    ''' 
    Friend Shared Function validarValorString(ByVal pstrValor As String, ByVal pstrRetornoNothingOVacio As String) As String
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

    Friend Shared ReadOnly Property obtenerMensajeValidacion(ByVal pstrMsg As String, ByVal pstrAccion As String, ByVal plogError As Boolean) As String
        Get
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

                strMsg &= pstrMsg.Replace("|", vbNewLine & "* ")
            End If

            Return (strMsg)
        End Get
    End Property

#End Region

#Region "Funciones para retornar clave"

    Private Shared Function RetornarClave(ByVal pstrConstante As String) As String
        If Application.Current.Resources.Contains(Recursos.RecursosApp.A2Consola_AppActiva.ToString()) Then
            If CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.ContainsKey(pstrConstante) Then
                Return CType(Application.Current.Resources(Recursos.RecursosApp.A2Consola_AppActiva.ToString()), Aplicacion).Parametros.Item(pstrConstante).ToString()
            Else
                Return ("")
            End If
        Else
            Return ("")
        End If
    End Function
#End Region

End Class
