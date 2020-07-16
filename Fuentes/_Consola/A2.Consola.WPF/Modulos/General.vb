Friend Module General
    Friend Const MINT_MAX_VENTANAS As Byte = 20
    Friend Const GSTR_PREF_NOM_VENTANA As String = "WINDOC"
    Friend Const GSTR_RECURSO_MONOAPLICACION As String = "A2Consola_App_Monoaplicacion"

    Friend glogSugerirLogin As Boolean = False
    Friend glogUsaUsuarioSinDominio As Boolean = False
    Friend glogMostrarDetalleErrorUsuario As Boolean = False

    ' -- CCM20131025 - Tipos de opción de menú
    ''' <summary>
    ''' B: Menu
    ''' G: Opción con parámetros
    ''' H: Hipervinculo
    ''' I: Fuera del browser
    ''' </summary>
    ''' 
    Friend Enum TipoOpcionMenu
        B 'Dentro del browser exclusivamente
        G 'Opción con parámetros
        H 'Hipervinculo
        I 'Fuera del browser
    End Enum

    Friend Function eliminarDominio(ByVal pstrLogin As String) As String
        Dim intPos As Integer = 0
        Try
            intPos = pstrLogin.LastIndexOf("/")
            If intPos < 0 Then
                intPos = pstrLogin.LastIndexOf("\")
            End If
            If intPos >= 0 Then
                pstrLogin = pstrLogin.Substring(intPos + 1)
            End If
        Catch ex As Exception
            pstrLogin = ""
        End Try

        Return (pstrLogin)
    End Function

End Module
