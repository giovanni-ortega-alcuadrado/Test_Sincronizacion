Imports System.Timers
Imports System.Net
Imports System.Configuration
Imports System.Runtime.Serialization
Imports System.Text
Imports A2.OyD.Infraestructura

Public Class clsProcesosArchivo

    Public Property gstrUser As String

    Public Enum ColumnasArchivoCheque
        TipoDctoBeneficiario
        NroBeneficiario
        NombreBeneficiario
        ValorGiro
        Concepto
    End Enum

    Public Enum ColumnasArchivoTransferencias
        TipoDocTitular
        NroDocTitular
        NombreTitular
        TipoCuenta
        CodigoBanco
        NroCuenta
        ValorGiro
        Concepto
    End Enum

#Region "Enumeradores de Columnas y Hojas Recibos"

    Public Enum HojasRecibos
        CargarPagosA
        Cheque
        Transferencia
        Consignacion
    End Enum

    Private Const NroColumnasCargarPagos As Integer = 1
    Private Const NroColumnasCheques As Integer = 7
    Private Const NroColumnasTransferencia As Integer = 7
    Private Const NroColumnasConsignacion As Integer = 7
    Public Const NroColumnasFondos As Integer = 9
    Public Const NroColumnasFondosClientes As Integer = 6

#End Region

    Sub New()

    End Sub


    Public Function RecorrerArchivoExcelRecibos(ByVal pstrRutaLocal As String, ByVal pstrNombreArchivo As String) As Dictionary(Of String, Object)
        Try
            Dim objDiccionario As New Dictionary(Of String, Object)
            Dim intFila As Integer = 1
            Dim intColumna As Integer = 0
            Dim intTotalColumna As Integer = 0
            Dim intHoja As Integer = 0
            Dim logHayFila As Boolean = True
            Dim objStringFila As String = String.Empty

            Dim objStringPagos As String = String.Empty
            Dim objStringCheques As String = String.Empty
            Dim objStringTransferencia As String = String.Empty
            Dim objStringConsignacion As String = String.Empty
            Dim objValorString As String = String.Empty
            Dim objValor As String = String.Empty

            Dim workbook As SpreadsheetGear.IWorkbook = SpreadsheetGear.Factory.GetWorkbook(pstrRutaLocal & pstrNombreArchivo)

            'Comienza a recorrer las hojas
            While intHoja <= 3

                'abro el libro de excel
                workbook.Sheets(intHoja).Select()

                'Define el número de columnas de la hoja
                intFila = 1
                logHayFila = True

                While logHayFila

                    objStringFila = String.Empty

                    Select Case intHoja
                        Case HojasRecibos.CargarPagosA
                            intTotalColumna = NroColumnasCargarPagos
                            objValorString = String.Empty
                            'Recorre las columnas para construir la información.
                            For intColumna = 0 To intTotalColumna

                                'Obtiene el valor de la colomna
                                Dim rangoCelda As SpreadsheetGear.IRange = workbook.ActiveWorksheet.Cells(intFila, intColumna)
                                objValor = LTrim(RTrim(rangoCelda.Text))
                                'Busco el nemo que corresponde a la columna 2 de la hoja dentro de la lista de nemos de la BVC si no existe terminó el ciclo para que continue con la fila siguiente
                                objStringFila = String.Format("{0}{1}", objStringFila, objValor)
                                objValorString = IIf(String.IsNullOrEmpty(objValorString), String.Format("'{0}'", objValor), String.Format("{0}**'{1}'", objValorString, objValor))

                            Next

                            If Not String.IsNullOrEmpty(objStringFila) Then
                                objStringPagos = IIf(String.IsNullOrEmpty(objStringPagos), objValorString, String.Format("{0}|{1}", objStringPagos, objValorString))
                            End If
                        Case HojasRecibos.Cheque
                            intTotalColumna = NroColumnasCheques
                            objValorString = String.Empty

                            'Recorre las columnas para construir la información.
                            For intColumna = 0 To intTotalColumna

                                'Obtiene el valor de la colomna
                                Dim rangoCelda As SpreadsheetGear.IRange = workbook.ActiveWorksheet.Cells(intFila, intColumna)
                                objValor = LTrim(RTrim(rangoCelda.Text))
                                'Busco el nemo que corresponde a la columna 2 de la hoja dentro de la lista de nemos de la BVC si no existe terminó el ciclo para que continue con la fila siguiente
                                objStringFila = String.Format("{0}{1}", objStringFila, objValor)
                                objValorString = IIf(String.IsNullOrEmpty(objValorString), String.Format("'{0}'", objValor), String.Format("{0}**'{1}'", objValorString, objValor))

                                'If Not String.IsNullOrEmpty(objValor) Then

                                'End If
                            Next

                            If Not String.IsNullOrEmpty(objStringFila) Then
                                objStringCheques = IIf(String.IsNullOrEmpty(objStringCheques), objValorString, String.Format("{0}|{1}", objStringCheques, objValorString))
                            End If
                        Case HojasRecibos.Transferencia
                            intTotalColumna = NroColumnasTransferencia
                            objValorString = String.Empty

                            'Recorre las columnas para construir la información.
                            For intColumna = 0 To intTotalColumna

                                'Obtiene el valor de la colomna
                                Dim rangoCelda As SpreadsheetGear.IRange = workbook.ActiveWorksheet.Cells(intFila, intColumna)
                                objValor = LTrim(RTrim(rangoCelda.Text))
                                'Busco el nemo que corresponde a la columna 2 de la hoja dentro de la lista de nemos de la BVC si no existe terminó el ciclo para que continue con la fila siguiente
                                objStringFila = String.Format("{0}{1}", objStringFila, objValor)
                                objValorString = IIf(String.IsNullOrEmpty(objValorString), String.Format("'{0}'", objValor), String.Format("{0}**'{1}'", objValorString, objValor))

                            Next

                            If Not String.IsNullOrEmpty(objStringFila) Then
                                objStringTransferencia = IIf(String.IsNullOrEmpty(objStringTransferencia), objValorString, String.Format("{0}|{1}", objStringTransferencia, objValorString))
                            End If
                        Case HojasRecibos.Consignacion
                            intTotalColumna = NroColumnasConsignacion
                            objValorString = String.Empty

                            'Recorre las columnas para construir la información.
                            For intColumna = 0 To intTotalColumna

                                'Obtiene el valor de la colomna
                                Dim rangoCelda As SpreadsheetGear.IRange = workbook.ActiveWorksheet.Cells(intFila, intColumna)
                                objValor = LTrim(RTrim(rangoCelda.Text))
                                'Busco el nemo que corresponde a la columna 2 de la hoja dentro de la lista de nemos de la BVC si no existe terminó el ciclo para que continue con la fila siguiente
                                objStringFila = String.Format("{0}{1}", objStringFila, objValor)
                                objValorString = IIf(String.IsNullOrEmpty(objValorString), String.Format("'{0}'", objValor), String.Format("{0}**'{1}'", objValorString, objValor))
                            Next

                            If Not String.IsNullOrEmpty(objStringFila) Then
                                objStringConsignacion = IIf(String.IsNullOrEmpty(objStringConsignacion), objValorString, String.Format("{0}|{1}", objStringConsignacion, objValorString))
                            End If
                    End Select

                    If String.IsNullOrEmpty(objStringFila) Then
                        logHayFila = False
                    Else
                        intFila += 1
                    End If
                End While

                intHoja += 1
            End While

            objDiccionario.Add("STRINGPAGOS", objStringPagos)
            objDiccionario.Add("STRINGCHEQUES", objStringCheques)
            objDiccionario.Add("STRINGTRANSFERENCIA", objStringTransferencia)
            objDiccionario.Add("STRINGCONSIGNACION", objStringConsignacion)

            Return objDiccionario
        Catch ex As Exception
            'ManejarError(ex, Me.ToString(), "RecorrerArchivoExcelRecibos")
            Return Nothing
        End Try
    End Function

    Public Function RecorrerArchivoExcel(ByVal pstrRutaLocal As String, ByVal pstrNombreArchivo As String, ByVal pintNroColumnas As Integer) As String
        Try
            Dim intFila As Integer = 1
            Dim intColumna As Integer = 0
            Dim logHayFila As Boolean = True
            Dim objStringFila As String = String.Empty
            Dim objValor As String = String.Empty
            Dim objStringRetorno As String = String.Empty

            Dim workbook As SpreadsheetGear.IWorkbook = SpreadsheetGear.Factory.GetWorkbook(pstrRutaLocal & pstrNombreArchivo)

            'abro el libro de excel
            workbook.Sheets(0).Select()

            'Define el número de columnas de la hoja
            logHayFila = True

            While logHayFila

                objStringFila = String.Empty

                'Recorre las columnas para construir la información.
                For intColumna = 0 To pintNroColumnas
                    'Obtiene el valor de la colomna
                    Dim rangoCelda As SpreadsheetGear.IRange = workbook.ActiveWorksheet.Cells(intFila, intColumna)
                    objValor = LTrim(RTrim(rangoCelda.Text))
                    'Busco el nemo que corresponde a la columna 2 de la hoja dentro de la lista de nemos de la BVC si no existe terminó el ciclo para que continue con la fila siguiente
                    If String.IsNullOrEmpty(objStringFila) Then
                        objStringFila = objValor
                    Else
                        objStringFila = String.Format("{0}**{1}", objStringFila, objValor)
                    End If
                Next

                If String.IsNullOrEmpty(objStringFila) Then
                    logHayFila = False
                Else
                    If String.IsNullOrEmpty(objStringRetorno) Then
                        objStringRetorno = objStringFila
                    Else
                        objStringRetorno = String.Format("{0}|{1}", objStringRetorno, objStringFila)
                    End If

                    intFila += 1
                End If
            End While

            Return objStringRetorno
        Catch ex As Exception
            ManejarError(ex, Me.ToString(), "RecorrerArchivoExcel")
            Return Nothing
        End Try
    End Function

    Function RecorrerArchivoExcelLEO(pstrRutaLocal As String, pstrNombreArchivo As String) As StringBuilder
        Dim objDiccionario As New Dictionary(Of String, Object)
        Dim intFila As Integer = 1
        Dim intColumna As Integer = 0
        Dim intTotalColumna As Integer = 0
        Dim logHayFila As Boolean = True
        'Dim logFilaValida As Boolean
        Dim objStringFila As String = String.Empty

        Dim objStringOrden As StringBuilder = New StringBuilder
        Dim objValorString As String = String.Empty
        Dim objValor As String = String.Empty

        Dim strClaseOrden As String = String.Empty

        'Dim workbook As SpreadsheetGear.IWorkbook = SpreadsheetGear.Factory.GetWorkbook(pstrRutaLocal & pstrNombreArchivo)

        ''abro el libro de excel
        'workbook.Sheets(0).Select()

        ''Define el número de columnas de la hoja
        'intFila = 1
        'logHayFila = True

        'While logHayFila

        '    objStringFila = String.Empty
        '    intTotalColumna = 15
        '    objStringFila = String.Empty

        '    logFilaValida = False

        '    'Recorre las columnas para construir la información.
        '    For intColumna = 0 To intTotalColumna

        '        'Obtiene el valor de la colomna
        '        Dim rangoCelda As SpreadsheetGear.IRange = workbook.ActiveWorksheet.Cells(intFila, intColumna)
        '        objValor = LTrim(RTrim(rangoCelda.Text))

        '        If intColumna = 0 And intFila = 1 Then strClaseOrden = objValor

        '        Select Case intColumna
        '            Case 0
        '                objStringFila += "<c>" + objValor + "</c>"
        '            Case 1
        '                objStringFila += "<cc>" + objValor + "</cc>"
        '            Case 2
        '                objStringFila += "<tc>" + objValor + "</tc>"
        '            Case 3
        '                objStringFila += "<u>" + objValor + "</u>"
        '            Case 4
        '                objStringFila += "<q>" + objValor + "</q>"
        '            Case 5
        '                objStringFila += "<d>" + objValor + "</d>"
        '            Case 6
        '                objStringFila += "<cd>" + objValor + "</cd>"
        '            Case 7
        '                objStringFila += "<tcl>" + objValor + "</tcl>"
        '            Case 8
        '                objStringFila += "<ocl>" + objValor + "</ocl>"
        '            Case 9
        '                objStringFila += "<e>" + objValor + "</e>"
        '            Case 10
        '                objStringFila += "<fe>" + objValor + "</fe>"
        '            Case 11
        '                objStringFila += "<fv>" + objValor + "</fv>"
        '            Case 12
        '                objStringFila += "<m>" + objValor + "</m>"
        '            Case 13
        '                objStringFila += "<tf>" + objValor + "</tf>"
        '            Case 14
        '                objStringFila += "<cm>" + objValor + "</cm>"
        '        End Select
        '        'TasaFacial	Comisión If intColumna = 0 Then

        '        logFilaValida = logFilaValida Or objValor.Length

        '    Next

        '    If logFilaValida Then
        '        ' agregar la fila con las etiquetas de inicio y fin
        '        objStringOrden += "<f>" + objStringFila + "</f>"
        '        intFila += 1
        '    Else
        '        logHayFila = False
        '    End If
        'End While


        Dim objReader As IO.StreamReader
        Dim strLinea As String()

        intFila = 0

        objStringOrden.AppendLine("<r>")

        objReader = New System.IO.StreamReader(pstrRutaLocal & pstrNombreArchivo)
        Do While objReader.Peek() <> -1
            Dim primeraLinea = objReader.ReadLine()
            strLinea = primeraLinea.Split(CChar(","))

            If intFila = 0 Then
                If strLinea.Count <> 15 Then
                    objStringOrden.Clear()
                    objStringOrden.AppendLine("El archivo no tiene el formato correcto.")
                    Return objStringOrden
                End If
            Else

                objStringFila = "<c>" + strLinea(0) + "</c><cc>" + strLinea(1) + "</cc><tc>" + strLinea(2) + "</tc><u>" + strLinea(3) + _
                    "</u><q>" + strLinea(4) + "</q><d>" + strLinea(5) + "</d><cd>" + strLinea(6) + "</cd><tcl>" + strLinea(7) + _
                    "</tcl><ocl>" + strLinea(8) + "</ocl><e>" + strLinea(9) + "</e><fe>" + strLinea(10) + "</fe><fv>" + strLinea(11) + _
                    "</fv><m>" + strLinea(12) + "</m><tf>" + strLinea(13) + "</tf><cm>" + strLinea(14) + "</cm>"

                objStringOrden.AppendLine("<f>" + objStringFila + "</f>")

            End If
            intFila += 1

        Loop

        'Else
        ''Archivo no tiene contenido
        'Return "El archivo está vacio."
        'End If

        objReader.Close()

        objStringOrden.AppendLine("</r>")

        A2Utilidades.Utilidades.registrarLog("Termino Recorrer", "Ordenes LEO", "Recorrer", "fuente", True, True, True)

        Return objStringOrden

    End Function

    ''' <summary>
    ''' Verifica el correcto formato del archivo de importación de amortizaciones
    ''' </summary>
    ''' <param name="pstrRutaLocal"></param>
    ''' <param name="pstrNombreArchivo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function RecorrerCSVAmortizaciones(pstrRutaLocal As String, pstrNombreArchivo As String) As StringBuilder

        Dim objStringAmortizacion As StringBuilder = New StringBuilder
        Dim objReader As IO.StreamReader
        Dim strLinea As String()
        Dim intFila As Integer = 0
        Dim objStringFila As String = String.Empty

        objStringAmortizacion.AppendLine("<r>")

        objReader = New System.IO.StreamReader(pstrRutaLocal & pstrNombreArchivo)


        Do While objReader.Peek() <> -1
            Dim primeraLinea = objReader.ReadLine()
            strLinea = primeraLinea.Split(CChar(","))

            If intFila = 0 Then
                If strLinea.Count <> 3 Then
                    objStringAmortizacion.Clear()
                    objStringAmortizacion.AppendLine("El archivo no tiene el formato correcto.")
                    Return objStringAmortizacion
                End If
            Else

                objStringFila = "<fc>" + strLinea(1) + "</fc><p>" + strLinea(2) + "</p>"

                objStringAmortizacion.AppendLine("<f>" + objStringFila + "</f>")

            End If
            intFila += 1

        Loop

        objReader.Close()

        objStringAmortizacion.AppendLine("</r>")

        A2Utilidades.Utilidades.registrarLog("Termino Recorrer", "Amoprtizaciones ISIN", "Recorrer", "fuente", True, True, True)

        Return objStringAmortizacion
    End Function

    ''' <summary>
    ''' Elimina lineas del archivo indicado
    ''' </summary>
    ''' <param name="pstrRutaLocal"></param>
    ''' <param name="pstrNombreArchivo"></param>
    ''' <param name="pintLineasARemover"></param>
    ''' <remarks></remarks>
    Public Sub EliminarFilas(pstrRutaLocal As String, pstrNombreArchivo As String, pintLineasARemover As Integer)
        Try

            Dim lines As New List(Of String)(IO.File.ReadAllLines(pstrRutaLocal & "\" & pstrNombreArchivo))
            lines.RemoveRange(0, pintLineasARemover)
            IO.File.WriteAllLines(pstrRutaLocal & "\" & pstrNombreArchivo, lines.ToArray())

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

End Class