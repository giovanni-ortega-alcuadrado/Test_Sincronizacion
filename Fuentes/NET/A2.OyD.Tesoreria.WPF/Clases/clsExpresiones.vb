Imports Telerik.Windows.Controls
Imports System.Text.RegularExpressions

Public Class clsExpresiones
    Private Const STR_EXPRESION_SOLONUMEROS As String = "^[0-9]*$"
    Private Const STR_EXPRESIONREEMPLAZAR_SOLONUMEROS As String = "!*[0-9]"
    Private Const STR_EXPRESION_SOLOLETRASNUMEROS As String = "^[a-zA-Z0-9_-]*$"
    Private Const STR_EXPRESIONREEMPLAZAR_SOLOLETRASNUMEROS As String = "!*[a-zA-Z0-9_-]"
    Private Const STR_EXPRESION_SOLOLETRASNUMEROSUNICAMENTE As String = "^[a-zA-Z0-9]*$"
    Private Const STR_EXPRESIONREEMPLAZAR_SOLOLETRASNUMEROSUNICAMENTE As String = "!*[a-zA-Z0-9]"
    Private Const STR_EXPRESION_SOLOLETRAS As String = "^[a-zA-Z ]*$"
    Private Const STR_EXPRESIONREEMPLAZAR_SOLOLETRAS As String = "!*[a-zA-Z ]"
    Private Const STR_EXPRESION_CADENACARACTERES As String = "^[a-z A-ZÑ 0-9 á-ú ¿?!¡._-]*$"
    Private Const STR_EXPRESIONREEMPLAZAR_CADENACARACTERES As String = "!*[a-z A-ZÑ 0-9 á-ú ¿?!¡._-]"

    Public Enum TipoExpresion
        Numeros
        Doble
        Caracteres
        LetrasNumeros
        LetrasNumerosUnicamente
        Letras
    End Enum

    Public Shared Function ValidarConversionNumero(ByVal pstrValor As String) As Boolean
        Dim logPasoValidacion As Boolean = True

        Try
            Dim intIDValor As Integer = CInt(pstrValor)
        Catch ex As Exception
            logPasoValidacion = False
        End Try
        Return logPasoValidacion
    End Function

    Public Shared Function ValidarConversionDoble(ByVal pstrValor As String) As Boolean
        Dim logPasoValidacion As Boolean = True

        Try
            Dim intIDValor As Double = CDbl(pstrValor)
        Catch ex As Exception
            logPasoValidacion = False
        End Try
        Return logPasoValidacion
    End Function

    Public Shared Function ValidarCaracteresEnCadena(ByVal pstrValor As String, ByVal pobjExpresion As TipoExpresion) As TextoFormateado
        Dim objRetorno As New TextoFormateado
        Dim strExpresion As String = String.Empty
        Dim strExpresionReemplazar As String = String.Empty

        If pobjExpresion = TipoExpresion.Numeros Then
            strExpresion = STR_EXPRESION_SOLONUMEROS
            strExpresionReemplazar = STR_EXPRESIONREEMPLAZAR_SOLONUMEROS
        ElseIf pobjExpresion = TipoExpresion.Caracteres Then
            strExpresion = STR_EXPRESION_CADENACARACTERES
            strExpresionReemplazar = STR_EXPRESIONREEMPLAZAR_CADENACARACTERES
        ElseIf pobjExpresion = TipoExpresion.LetrasNumeros Then
            strExpresion = STR_EXPRESION_SOLOLETRASNUMEROS
            strExpresionReemplazar = STR_EXPRESIONREEMPLAZAR_SOLOLETRASNUMEROS
        ElseIf pobjExpresion = TipoExpresion.LetrasNumerosUnicamente Then
            strExpresion = STR_EXPRESION_SOLOLETRASNUMEROSUNICAMENTE
            strExpresionReemplazar = STR_EXPRESIONREEMPLAZAR_SOLOLETRASNUMEROSUNICAMENTE
        ElseIf pobjExpresion = TipoExpresion.Letras Then
            strExpresion = STR_EXPRESION_SOLOLETRAS
            strExpresionReemplazar = STR_EXPRESIONREEMPLAZAR_SOLOLETRAS
        Else
            strExpresion = STR_EXPRESION_CADENACARACTERES
            strExpresionReemplazar = STR_EXPRESIONREEMPLAZAR_CADENACARACTERES
        End If

        If Regex.IsMatch(pstrValor, strExpresion) Then
            objRetorno.TextoValido = True
            objRetorno.PosicionPrimerInvalido = 0
            objRetorno.CadenaAnterior = pstrValor
            objRetorno.CadenaNueva = pstrValor
        Else
            objRetorno.TextoValido = False
            objRetorno.CadenaAnterior = pstrValor
            objRetorno.CadenaNueva = pstrValor
            objRetorno.PosicionPrimerInvalido = 0

            Dim strCadenaInvalida As String() = Regex.Split(pstrValor, strExpresionReemplazar)
            For Each caracter In strCadenaInvalida
                If Not String.IsNullOrEmpty(caracter) Then
                    If objRetorno.PosicionPrimerInvalido = 0 Then
                        objRetorno.PosicionPrimerInvalido = pstrValor.IndexOf(caracter)
                    End If
                    objRetorno.CadenaNueva = objRetorno.CadenaNueva.Replace(caracter, "")
                End If
            Next
        End If

        Return objRetorno
    End Function
End Class


Public Class TextoFormateado
    Public Property TextoValido As Boolean
    Public Property PosicionPrimerInvalido As Integer
    Public Property CadenaAnterior As String
    Public Property CadenaNueva As String
End Class