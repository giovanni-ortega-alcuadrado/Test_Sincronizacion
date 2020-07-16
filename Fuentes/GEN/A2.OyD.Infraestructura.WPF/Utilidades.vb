Imports System.Data

Public Module Utilidades

    Public Function GenerarExcel(ByVal pobjDatos As DataTable, ByVal pstrRuta As String) As String
        Return GenerarExcel_Local(pobjDatos, pstrRuta)
    End Function

    Public Function GenerarExcel(ByVal pobjDatos As DataTable, ByVal pstrRuta As String, ByVal pstrNombreArchivo As String) As String
        Return GenerarExcel_Local(pobjDatos, pstrRuta, pstrNombreArchivo)
    End Function

    Public Function GenerarExcel(ByVal pobjDatos As DataTable, ByVal pstrRuta As String, ByVal pstrNombreArchivo As String, ByVal pstrNombreHoja As String) As String
        Return GenerarExcel_Local(pobjDatos, pstrRuta, pstrNombreArchivo, pstrNombreHoja)
    End Function

    Public Function GenerarExcel(ByVal pobjDatos As DataTable, ByVal pstrRuta As String, ByVal pstrNombreArchivo As String, ByVal pstrNombreHoja As String, ByVal plogNombreColumnas As Boolean) As String
        Return GenerarExcel_Local(pobjDatos, pstrRuta, pstrNombreArchivo, pstrNombreHoja, plogNombreColumnas)
    End Function

    Private Function GenerarExcel_Local(ByVal pobjDatos As DataTable, ByVal pstrRuta As String, Optional ByVal pstrNombreArchivo As String = "", Optional ByVal pstrNombreHoja As String = "", Optional ByVal plogNombreColumnas As Boolean = True) As String
        Try

            Dim strExtensionNombreArchivo As String = ".xlsx"
            Dim objWorkbook As SpreadsheetGear.IWorkbook = SpreadsheetGear.Factory.GetWorkbook
            Dim objWorkSheet As SpreadsheetGear.IWorksheet = objWorkbook.Worksheets(0)
            objWorkSheet.Name = "Sheet1"
            If Not String.IsNullOrEmpty(pstrNombreHoja) Then
                objWorkSheet.Name = pstrNombreHoja
            End If
            Dim objRange As SpreadsheetGear.IRange = objWorkSheet.Cells(0, 0)

            Dim strAgrupadorMiles As String = objWorkbook.WorkbookSet.Culture.NumberFormat.NumberGroupSeparator
            Dim strSeparadorDecimal As String = objWorkbook.WorkbookSet.Culture.NumberFormat.NumberDecimalSeparator
            Dim strFormatoNumerosExcel As String = "#" & strAgrupadorMiles & "##0" & strSeparadorDecimal & "00"
            Dim strFormatoFechasExcel As String = "dd/mm/yyyy"
            Dim strBackSlash As String = "\"

            Dim strNombreArchivo As String
            Dim strRutaCompletaArchivo As String

            If Not String.IsNullOrEmpty(pstrNombreArchivo) Then
                If pstrRuta.Substring(pstrRuta.Length - 2, 1) <> strBackSlash Then
                    pstrRuta = pstrRuta & strBackSlash
                End If

                strNombreArchivo = pstrNombreArchivo & strExtensionNombreArchivo
                strRutaCompletaArchivo = pstrRuta & strNombreArchivo
            Else
                strNombreArchivo = pstrRuta
                strRutaCompletaArchivo = pstrRuta
            End If

            If plogNombreColumnas Then
                objRange.CopyFromDataTable(pobjDatos, SpreadsheetGear.Data.SetDataFlags.None)

                For I As Integer = 0 To pobjDatos.Columns.Count - 1

                    Select Case pobjDatos.Columns(I).DataType
                        Case GetType(Decimal), GetType(Double)
                            objWorkSheet.Cells(0, I).EntireColumn.NumberFormat = strFormatoNumerosExcel
                        Case GetType(Date)
                            objWorkSheet.Cells(0, I).EntireColumn.NumberFormat = strFormatoFechasExcel
                    End Select

                Next

                objWorkSheet.UsedRange.Columns.AutoFit()

                objWorkSheet.UsedRange.Columns.AutoFilter()

                'objWorkSheet.Cells(1, 0).EntireRow.Insert(SpreadsheetGear.InsertShiftDirection.Down)

                objWorkSheet.Cells(0, 0).EntireRow.Interior.Color = SpreadsheetGear.Color.FromArgb(51, 51, 153)

                objWorkSheet.Cells(0, 0).EntireRow.Font.Bold = True

                objWorkSheet.Cells(0, 0).EntireRow.Font.Color = SpreadsheetGear.Color.FromArgb(255, 255, 255)
            Else
                objRange.CopyFromDataTable(pobjDatos, SpreadsheetGear.Data.SetDataFlags.NoColumnHeaders)
            End If

            objWorkbook.SaveAs(strRutaCompletaArchivo, SpreadsheetGear.FileFormat.OpenXMLWorkbook)

            Return strNombreArchivo
        Catch ex As Exception
            Throw
            Return String.Empty
        End Try

    End Function

End Module
