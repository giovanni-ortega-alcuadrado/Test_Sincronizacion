Imports A2Utilidades.Utilidades
Module Generales
    Public intIdTesoreriaOrdenesEncabezado As Nullable(Of Integer)
    Public Const STR_TODOS As String = "(Todos)"
    
    Public Enum TIPOS_MERCADO
        ''' <summary>
        ''' Acciones 'Renta Variable'
        ''' </summary>
        ''' <remarks></remarks>
        A
        ''' <summary>
        ''' Renta Fija 'Crediticio'
        ''' </summary>
        ''' <remarks></remarks>
        C
        ''' <summary>
        ''' No determinado
        ''' </summary>
        ''' <remarks></remarks>
        T '???
    End Enum



    Public Function ReemplazarcaracterEnCadena(ByVal pstrcadena As Object) As String
        '/******************************************************************************************
        '/* INICIO DOCUMENTO
        '/* Function BuscarEnCadena(ByVal pstrcadena As Variant, ByVal pstrBuscar As String, ByVal pstrReemplazar As String) As String
        '/* Alcance     :   Public
        '/* Descripción :   Remplaza un caracter por otro en una cadena de caracteres utilizado en divisas.
        '/* Parámetros  :
        '/* Por             Nombre              Tipo            Descripción
        '/* Valor           pstrcadena          Variant         Cadena de caracteres donde las ocurrencias
        '/*                                                     de otro caracter
        '/* Valor           pstrBuscar          string          Caracteres a buscar en pstrCadena
        '/* Valor           pstrReemplazar      string          Caracteres que remplazaran a pstrBuscar
        '/*                                                     en pstrCadena.
        '/* Valores de retorno:
        '/* RemplazarEnCadena : Retorna la cadena de caracteres remplazada con el caracter
        '/*                     especificado si se encuentra la ocurrencia de otro caracter,
        '/*                     En caso de no tener ocurrencia este ultimo, saldra la misma cadena.
        '/* FIN DOCUMENTO
        '/******************************************************************************************
        Dim ByTPosicion As Integer
        Dim strCaracter As Object
        Dim BytPosicionAnt As Integer
        Dim strCadenaTemporal As String = ""

        ReemplazarcaracterEnCadena = IIf(IsNothing(pstrcadena), "", pstrcadena)
        If pstrcadena <> "" And Not IsNothing(pstrcadena) Then
            BytPosicionAnt = 1
            ByTPosicion = Len(pstrcadena)
            Do While BytPosicionAnt <= ByTPosicion

                strCaracter = Mid(pstrcadena, BytPosicionAnt, 1)
                If Not IsNumeric(strCaracter) Then
                    If strCaracter = "." Or strCaracter = "/" Or strCaracter = "-" Or strCaracter = "_" Or strCaracter = " " Or strCaracter = "#" Or strCaracter = "P" _
                        Or strCaracter = "P" Or strCaracter = "B" Or strCaracter = "X" Then
                        strCadenaTemporal = strCadenaTemporal + ""
                    Else
                        ReemplazarcaracterEnCadena = strCadenaTemporal
                        Exit Function
                    End If
                Else
                    strCadenaTemporal = strCadenaTemporal + strCaracter
                End If
                BytPosicionAnt = BytPosicionAnt + 1
            Loop
        End If

        ReemplazarcaracterEnCadena = strCadenaTemporal
    End Function



    Public Function ComparaFechas(ByVal vdtInicial As String, ByVal vdtFinal As String, ByVal vstrTipo As String) As Boolean
        '/******************************************************************************************
        '/* INICIO DOCUMENTO
        '/* Function ComparaFechas(ByVal vdtInicial As String, ByVal vdtFinal As String, ByVal vstrTipo As String) As Boolean
        '/* Alcance     :   Public
        '/* Descripción :   Compara el valor de dos fechas.
        '/* Parámetros  :
        '/* Por             Nombre              Tipo            Descripción
        '/* Valor           vdtInicial          string          Primera fecha a comparar
        '/* Valor           vdtFinal            string          Segunda fecha a comparar
        '/* Valor           vstrTipo            string          operador booleano que indicara
        '/*                                                     el tipo de comparación.
        '/* Valores de retorno:
        '/* ComparaFechas:  Devuelve TRUE si el operador suministrado es cierto para las dos fechas
        '/*                 Devuelve FALSE si el operador suministrado NO es cierto.
        '/* FIN DOCUMENTO
        '/******************************************************************************************
        Dim vdtFinal_net As DateTime = vdtFinal
        Dim vdtInicial_net As DateTime = vdtInicial

        ComparaFechas = True
        Select Case vstrTipo
            Case "="
                If vdtFinal_net = vdtInicial_net Then
                    Exit Function
                End If
                'If Day(vdtInicial_net) = vdtFinal_net.Day And Month(vdtInicial_net) = Month(vdtFinal_net) And Year(vdtInicial) = Year(vdtFinal_net) Then
                '    Exit Function
                'End If
            Case "<"
                If vdtInicial_net < vdtFinal_net Then
                    Exit Function
                End If
                'If Year(vdtInicial_net) < Year(vdtFinal) Then
                '    Exit Function
                'ElseIf Month(vdtInicial_net) < Month(vdtFinal) And Year(vdtInicial_net) = Year(vdtFinal) Then
                '    Exit Function
                'ElseIf Day(vdtInicial_net) < Day(vdtFinal_net) And Month(vdtInicial_net) = Month(vdtFinal) And Year(vdtInicial_net) = Year(vdtFinal) Then
                '    Exit Function
                'End If
            Case ">"
                If vdtInicial_net > vdtFinal_net Then
                    Exit Function
                End If
                'If Year(vdtInicial_net) > Year(vdtFinal) Then
                '    Exit Function
                'ElseIf Month(vdtInicial_net) > Month(vdtFinal) And Year(vdtInicial_net) = Year(vdtFinal_net) Then
                '    Exit Function
                'ElseIf Day(vdtInicial_net) > Day(vdtFinal_net) And Month(vdtInicial_net) = Month(vdtFinal_net) And Year(vdtInicial_net) = Year(vdtFinal_net) Then
                '    Exit Function
                'End If
        End Select

        ComparaFechas = False
    End Function

  
    Public Sub dblTotalValorFuturoRepo(ByVal pstrCliente As String, _
                                   ByVal pdtmOrden As Date, _
                                   ByVal pdblOtrosValores As Double, _
                                   ByRef pdblTotalRecompras As Double, _
                                   ByRef pdblTotalFuturoOrdenesRepos As Double, _
                                   ByRef pdblTotalRecomprasImportadas As Double, _
                                   ByRef pdblPatrimonioTecnicoFirma As Double, _
                                   ByRef pdblSuperaPatrimonioTecnico As Double)
        'On Error GoTo Error

        '        Dim mpstConsultarFuturoRepo As rdoPreparedStatement
        '        Dim rst As rdoResultset
        '        Dim strSQL As String
        '        ' Prepared statement que consulta: Total de recompras, Total de órdenes, Patrimonio técnico de la Firma
        '10:
        '        strSQL = "{CALL uspValorFuturoRepoConsultar (?, ?, ?)}"
        '        mpstConsultarFuturoRepo = gdb.CreatePreparedStatement("", strSQL)

        '20:     mpstConsultarFuturoRepo.rdoParameters(0).Value = pstrCliente
        '30:     mpstConsultarFuturoRepo.rdoParameters(1).Value = pdtmOrden
        '35:     mpstConsultarFuturoRepo.rdoParameters(2).Value = pdblOtrosValores
        '40:     rst = mpstConsultarFuturoRepo.OpenResultset(rdOpenStatic)
        '50:     If Not (rst.EOF And rst.BOF) Then
        '60:         rst.MoveFirst()
        '70:         pdblTotalRecompras = rst.rdoColumns("TotalRecompras").Value
        '90:         pdblTotalFuturoOrdenesRepos = rst.rdoColumns("TotalFuturoRepoOrdenes").Value
        '95:         pdblTotalRecomprasImportadas = rst.rdoColumns("TotalRecomprasImportadas").Value
        '96:         pdblPatrimonioTecnicoFirma = rst.rdoColumns("PatrimonioTecnico").Value
        '97:         pdblSuperaPatrimonioTecnico = rst.rdoColumns("SuperaPatrimonioTecnico").Value

        '100:        rst.Cancel()
        '110:        rst.Close()
        '120:        rst = Nothing

        '            ''130      fn_dblTotalValorFuturoRepo = pdblTotalRecompras + pdblTotalFuturoOrdenesRepos
        '        End If

        '140:
        '        mpstConsultarFuturoRepo.Close()
        '        mpstConsultarFuturoRepo = Nothing
        '        Exit Sub
        'Error:
        '        LogErrores("mdlGeneral.dblTotalValorFuturoRepo", Err.Number, Err.Description & " (Línea " & CStr(Erl) & ")")
    End Sub

    Public Function fmtCodigo(ByVal pvstrCodigo As String) As String
        'Justifica un número a la derecha
        fmtCodigo = Format$(pvstrCodigo, "@@@@@@@@@@@@@@@@@")
    End Function
    ''' <summary>
    ''' Funcion para encontrar caracteres especiales en un string JBT20130418
    ''' </summary>
    ''' <remarks></remarks>
    Public Function validarcaracteres(ByVal dato As String) As Boolean
        If ((dato.IndexOf("!") > 0) Or
              (dato.IndexOf("#") > 0) Or
              (dato.IndexOf("$") > 0) Or
              (dato.IndexOf("%") > 0) Or
              (dato.IndexOf("&") > 0) Or
              (dato.IndexOf("(") > 0) Or
              (dato.IndexOf(")") > 0) Or
              (dato.IndexOf("*") > 0) Or
              (dato.IndexOf("+") > 0) Or
              (dato.IndexOf(",") > 0) Or
              (dato.IndexOf("-") > 0) Or
              (dato.IndexOf("/") > 0) Or
              (dato.IndexOf("/") > 0) Or
              (dato.IndexOf("/") > 0) Or
              (dato.IndexOf(":") > 0) Or
              (dato.IndexOf(";") > 0) Or
              (dato.IndexOf("<") > 0) Or
              (dato.IndexOf("=") > 0) Or
              (dato.IndexOf(">") > 0) Or
              (dato.IndexOf("?") > 0) Or
              (dato.IndexOf("@") > 0) Or
              (dato.IndexOf("[") > 0) Or
              (dato.IndexOf("]") > 0) Or
              (dato.IndexOf("^") > 0) Or
              (dato.IndexOf("_") > 0) Or
              (dato.IndexOf("{") > 0) Or
              (dato.IndexOf("|") > 0) Or
              (dato.IndexOf("}") > 0) Or
              (dato.IndexOf("~") > 0)) Then
            Return True
        End If
        Return False
    End Function


End Module
