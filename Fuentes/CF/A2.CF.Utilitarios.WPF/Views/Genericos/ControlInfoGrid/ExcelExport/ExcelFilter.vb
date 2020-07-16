Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Data
Imports System.Diagnostics
Imports C1.WPF.Excel
Imports C1.WPF.FlexGrid
Imports C1.WPF
Imports System.Net
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Ink
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Shapes


Friend NotInheritable Class ExcelFilter

    Private Shared _lastBook As C1XLBook

    Private Shared _cellStyles As Dictionary(Of XLStyle, ExcelCellStyle) = New Dictionary(Of XLStyle, ExcelCellStyle)()

    Private Shared _excelStyles As Dictionary(Of ExcelCellStyle, XLStyle) = New Dictionary(Of ExcelCellStyle, XLStyle)()

    Public Shared Sub Save(ByVal flex As C1FlexGrid, ByVal sheet As XLSheet)
        If Not Object.ReferenceEquals(sheet.Book, _lastBook) Then
            _cellStyles.Clear()
            _excelStyles.Clear()
            _lastBook = sheet.Book
        End If

        sheet.DefaultRowHeight = PixelsToTwips(flex.Rows.DefaultSize)
        sheet.DefaultColumnWidth = PixelsToTwips(flex.Columns.DefaultSize)
        sheet.Locked = flex.IsReadOnly
        sheet.ShowGridLines = flex.GridLinesVisibility <> GridLinesVisibility.None
        sheet.ShowHeaders = flex.HeadersVisibility <> HeadersVisibility.None
        sheet.OutlinesBelow = flex.GroupRowPosition = GroupRowPosition.BelowData
        sheet.Columns.Clear()

        Dim objColumnas As New List(Of clsColumnasFlexGrid)
        Dim intContadorColumna As Integer = 0
        Dim intContadorColumnaReal As Integer = 0

        For Each col As Column In flex.Columns
            If col.Visible Then
                objColumnas.Add(New clsColumnasFlexGrid With {.intIDColumna = intContadorColumna, .intIDColumnaReal = intContadorColumnaReal, .objColumna = col})
                intContadorColumnaReal += 1
            End If
            intContadorColumna += 1
        Next

        If Not IsNothing(objColumnas) Then
            For Each objCol As clsColumnasFlexGrid In objColumnas
                Dim col = objCol.objColumna
                Dim c As Object = sheet.Columns.Add()
                If Not col.Width.IsAuto Then
                    c.Width = PixelsToTwips(col.ActualWidth)
                End If

                c.Visible = col.Visible
                If TypeOf col.CellStyle Is ExcelCellStyle Then
                    c.Style = GetXLStyle(flex, sheet, CType(col.CellStyle, ExcelCellStyle))
                End If
            Next
        End If
        'For Each col As Column In flex.Columns
        '    Dim c As Object = sheet.Columns.Add()
        '    If Not col.Width.IsAuto Then
        '        c.Width = PixelsToTwips(col.ActualWidth)
        '    End If

        '    c.Visible = col.Visible
        '    If TypeOf col.CellStyle Is ExcelCellStyle Then
        '        c.Style = GetXLStyle(flex, sheet, CType(col.CellStyle, ExcelCellStyle))
        '    End If
        'Next

        sheet.Rows.Clear()
        Dim headerStyle As XLStyle = Nothing
        headerStyle = New XLStyle(sheet.Book)
        headerStyle.Font = New XLFont("Arial", 10, True, False)

        For Each row As Row In flex.ColumnHeaders.Rows
            Dim r1 As Object = sheet.Rows.Add()
            If row.Height > -1 Then
                r1.Height = PixelsToTwips(row.Height)
            End If

            If TypeOf row.CellStyle Is ExcelCellStyle Then
                r1.Style = GetXLStyle(flex, sheet, CType(row.CellStyle, ExcelCellStyle))
            End If

            If TypeOf row Is ExcelRow Then
                r1.OutlineLevel = (CType(row, ExcelRow)).Level
            End If

            If Not IsNothing(objColumnas) Then
                For Each li In objColumnas
                    Dim cell As Object = sheet(row.Index, li.intIDColumnaReal)
                    Dim colHeader As String = If(flex.ColumnHeaders(row.Index, li.intIDColumna) IsNot Nothing, flex.ColumnHeaders(row.Index, li.intIDColumna).ToString(), flex.Columns(li.intIDColumna).ColumnName)
                    cell.Value = colHeader
                    cell.Style = headerStyle
                Next
            End If
            'For c As Integer = 0 To flex.ColumnHeaders.Columns.Count - 1
            '    Dim cell As Object = sheet(row.Index, c)
            '    Dim colHeader As String = If(flex.ColumnHeaders(row.Index, c) IsNot Nothing, flex.ColumnHeaders(row.Index, c).ToString(), flex.Columns(c).ColumnName)
            '    cell.Value = colHeader
            '    cell.Style = headerStyle
            'Next

            r1.Visible = row.Visible
        Next

        For Each row As Row In flex.Rows
            Dim r2 As Object = sheet.Rows.Add()
            If row.Height > -1 Then
                r2.Height = PixelsToTwips(row.Height)
            End If

            If TypeOf row.CellStyle Is ExcelCellStyle Then
                r2.Style = GetXLStyle(flex, sheet, CType(row.CellStyle, ExcelCellStyle))
            End If

            If TypeOf row Is ExcelRow Then
                r2.OutlineLevel = (CType(row, ExcelRow)).Level
            End If

            r2.Visible = row.Visible
        Next

        Dim rowStart As Integer = flex.ColumnHeaders.Rows.Count
        Dim r = 0
        While r <= flex.Rows.Count - 1
            If Not IsNothing(objColumnas) Then
                For Each li In objColumnas
                    Dim cell As Object = sheet(rowStart, li.intIDColumnaReal)
                    Dim obj As Object = flex(r, li.intIDColumna)
                    cell.Value = If(TypeOf obj Is FrameworkElement, 0, obj)
                    Dim row As Object = TryCast(flex.Rows(r), ExcelRow)
                    If row IsNot Nothing Then
                        Dim col As Object = flex.Columns(li.intIDColumna)
                        Dim cs As Object = TryCast(row.GetCellStyle(col), ExcelCellStyle)
                        If cs IsNot Nothing Then
                            cell.Style = GetXLStyle(flex, sheet, cs)
                        End If
                    End If
                Next
            End If
            'For c As Integer = 0 To flex.Columns.Count - 1
            '    Dim cell As Object = sheet(rowStart, c)
            '    Dim obj As Object = flex(r, c)
            '    cell.Value = If(TypeOf obj Is FrameworkElement, 0, obj)
            '    Dim row As Object = TryCast(flex.Rows(r), ExcelRow)
            '    If row IsNot Nothing Then
            '        Dim col As Object = flex.Columns(c)
            '        Dim cs As Object = TryCast(row.GetCellStyle(col), ExcelCellStyle)
            '        If cs IsNot Nothing Then
            '            cell.Style = GetXLStyle(flex, sheet, cs)
            '        End If
            '    End If
            'Next

            r += 1
            rowStart += 1
        End While

        Dim sel As CellRange = flex.Selection
        If sel.IsValid Then
            Dim xlSel = New XLCellRange(sheet, sel.Row, sel.Row2, sel.Column, sel.Column2)
            sheet.SelectedCells.Clear()
            sheet.SelectedCells.Add(xlSel)
        End If
    End Sub

    Private Shared Function TwipsToPixels(ByVal twips As Double) As Double
        Return Convert.ToInt32(twips / 1440 * 96 * 1.2 + 0.5)
    End Function

    Private Shared Function PixelsToTwips(ByVal pixels As Double) As Integer
        Return Convert.ToInt32(pixels * 1440 / 96 / 1.2 + 0.5)
    End Function

    Private Shared Function PointsToPixels(ByVal points As Double) As Double
        Return points / 72 * 96 * 1.2
    End Function

    Private Shared Function PixelsToPoints(ByVal pixels As Double) As Double
        Return pixels * 72 / 96 / 1.2
    End Function

    Private Shared Function GetCellStyle(ByVal x As XLStyle) As ExcelCellStyle
        Dim s As ExcelCellStyle = Nothing
        If _cellStyles.TryGetValue(x, s) Then
            Return s
        End If

        s = New ExcelCellStyle()
        Select Case x.AlignHorz
            Case XLAlignHorzEnum.Left
                s.HorizontalAlignment = HorizontalAlignment.Left
            Case XLAlignHorzEnum.Center
                s.HorizontalAlignment = HorizontalAlignment.Center
            Case XLAlignHorzEnum.Right
                s.HorizontalAlignment = HorizontalAlignment.Right
        End Select

        Select Case x.AlignVert
            Case XLAlignVertEnum.Top
                s.VerticalAlignment = VerticalAlignment.Top
            Case XLAlignVertEnum.Center
                s.VerticalAlignment = VerticalAlignment.Center
            Case XLAlignVertEnum.Bottom
                s.VerticalAlignment = VerticalAlignment.Bottom
        End Select

        s.TextWrapping = x.WordWrap
        If x.BackPattern = XLPatternEnum.Solid AndAlso IsColorValid(x.BackColor) Then
            s.Background = New SolidColorBrush(x.BackColor)
        End If

        If IsColorValid(x.ForeColor) Then
            s.Foreground = New SolidColorBrush(x.ForeColor)
        End If

        Dim font As Object = x.Font
        If font IsNot Nothing Then
            s.FontFamily = New FontFamily(font.FontName)
            s.FontSize = PointsToPixels(font.FontSize)
            If font.Bold Then
                s.FontWeight = FontWeights.Bold
            End If

            If font.Italic Then
                s.FontStyle = FontStyles.Italic
            End If

            If font.Underline <> XLUnderlineStyle.None Then
                s.TextDecorations = TextDecorations.Underline
            End If
        End If

        If Not String.IsNullOrEmpty(x.Format) Then
            s.Format = XLStyle.FormatXLToDotNet(x.Format)
        End If

        s.CellBorderThickness = New Thickness(GetBorderThickness(x.BorderLeft), GetBorderThickness(x.BorderTop), GetBorderThickness(x.BorderRight), GetBorderThickness(x.BorderBottom))
        s.CellBorderBrushLeft = GetBorderBrush(x.BorderColorLeft)
        s.CellBorderBrushTop = GetBorderBrush(x.BorderColorTop)
        s.CellBorderBrushRight = GetBorderBrush(x.BorderColorRight)
        s.CellBorderBrushBottom = GetBorderBrush(x.BorderColorBottom)
        _cellStyles(x) = s
        Return s
    End Function

    Private Shared Function GetXLStyle(ByVal flex As C1FlexGrid, ByVal sheet As XLSheet, ByVal s As ExcelCellStyle) As XLStyle
        Dim x As XLStyle = Nothing
        If _excelStyles.TryGetValue(s, x) Then
            Return x
        End If

        x = New XLStyle(sheet.Book)
        If s.HorizontalAlignment.HasValue Then
            Select Case s.HorizontalAlignment.Value
                Case HorizontalAlignment.Left
                    x.AlignHorz = XLAlignHorzEnum.Left
                Case HorizontalAlignment.Center
                    x.AlignHorz = XLAlignHorzEnum.Center
                Case HorizontalAlignment.Right
                    x.AlignHorz = XLAlignHorzEnum.Right
            End Select
        End If

        If s.VerticalAlignment.HasValue Then
            Select Case s.VerticalAlignment.Value
                Case VerticalAlignment.Top
                    x.AlignVert = XLAlignVertEnum.Top
                Case VerticalAlignment.Center
                    x.AlignVert = XLAlignVertEnum.Center
                Case VerticalAlignment.Bottom
                    x.AlignVert = XLAlignVertEnum.Bottom
            End Select
        End If

        If s.TextWrapping.HasValue Then
            x.WordWrap = s.TextWrapping.Value
        End If

        If TypeOf s.Background Is SolidColorBrush Then
            x.BackColor = (CType(s.Background, SolidColorBrush)).Color
            x.BackPattern = XLPatternEnum.Solid
        End If

        If TypeOf s.Foreground Is SolidColorBrush Then
            x.ForeColor = (CType(s.Foreground, SolidColorBrush)).Color
        End If

        Dim fontName As Object = flex.FontFamily.Source
        Dim fontSize As Object = flex.FontSize
        Dim bold As Object = False
        Dim italic As Object = False
        Dim underline As Boolean = False
        Dim hasFont As Boolean = False
        If s.FontFamily IsNot Nothing Then
            fontName = s.FontFamily.Source
            hasFont = True
        End If

        If s.FontSize.HasValue Then
            fontSize = s.FontSize.Value
            hasFont = True
        End If

        If s.FontWeight.HasValue Then
            bold = s.FontWeight.Value = FontWeights.Bold OrElse s.FontWeight.Value = FontWeights.ExtraBold OrElse s.FontWeight.Value = FontWeights.SemiBold
            hasFont = True
        End If

        If s.FontStyle.HasValue Then
            italic = s.FontStyle.Value = FontStyles.Italic
            hasFont = True
        End If

        If s.TextDecorations IsNot Nothing Then
            underline = True
            hasFont = True
        End If

        If hasFont Then
            fontSize = PixelsToPoints(fontSize)
            If underline Then
                Dim color As Object = Colors.Black
                If TypeOf flex.Foreground Is SolidColorBrush Then
                    color = (CType(flex.Foreground, SolidColorBrush)).Color
                End If

                If TypeOf s.Foreground Is SolidColorBrush Then
                    color = (CType(s.Foreground, SolidColorBrush)).Color
                End If

                x.Font = New XLFont(fontName, Convert.ToSingle(fontSize), bold, italic, False, XLFontScript.None, XLUnderlineStyle.Single, color)
            Else
                x.Font = New XLFont(fontName, Convert.ToSingle(fontSize), bold, italic)
            End If
        End If

        If Not String.IsNullOrEmpty(s.Format) Then
            x.Format = XLStyle.FormatDotNetToXL(s.Format)
        End If

        If s.CellBorderThickness.Left > 0 AndAlso TypeOf s.CellBorderBrushLeft Is SolidColorBrush Then
            x.BorderLeft = GetBorderLineStyle(s.CellBorderThickness.Left)
            x.BorderColorLeft = (CType(s.CellBorderBrushLeft, SolidColorBrush)).Color
        End If

        If s.CellBorderThickness.Top > 0 AndAlso TypeOf s.CellBorderBrushTop Is SolidColorBrush Then
            x.BorderTop = GetBorderLineStyle(s.CellBorderThickness.Top)
            x.BorderColorTop = (CType(s.CellBorderBrushTop, SolidColorBrush)).Color
        End If

        If s.CellBorderThickness.Right > 0 AndAlso TypeOf s.CellBorderBrushRight Is SolidColorBrush Then
            x.BorderRight = GetBorderLineStyle(s.CellBorderThickness.Right)
            x.BorderColorRight = (CType(s.CellBorderBrushRight, SolidColorBrush)).Color
        End If

        If s.CellBorderThickness.Bottom > 0 AndAlso TypeOf s.CellBorderBrushBottom Is SolidColorBrush Then
            x.BorderBottom = GetBorderLineStyle(s.CellBorderThickness.Bottom)
            x.BorderColorBottom = (CType(s.CellBorderBrushBottom, SolidColorBrush)).Color
        End If

        _excelStyles(s) = x
        Return x
    End Function

    Private Shared Function GetBorderThickness(ByVal ls As XLLineStyleEnum) As Double
        Select Case ls
            Case XLLineStyleEnum.None
                Return 0
            Case XLLineStyleEnum.Hair
                Return 0.5
            Case XLLineStyleEnum.Thin, XLLineStyleEnum.ThinDashDotDotted, XLLineStyleEnum.ThinDashDotted, XLLineStyleEnum.Dashed, XLLineStyleEnum.Dotted
                Return 1
            Case XLLineStyleEnum.Medium, XLLineStyleEnum.MediumDashDotDotted, XLLineStyleEnum.MediumDashDotted, XLLineStyleEnum.MediumDashed, XLLineStyleEnum.SlantedMediumDashDotted
                Return 2
            Case XLLineStyleEnum.Double, XLLineStyleEnum.Thick
                Return 3
        End Select

        Return 0
    End Function

    Private Shared Function GetBorderLineStyle(ByVal t As Double) As XLLineStyleEnum
        If t = 0 Then
            Return XLLineStyleEnum.None
        End If

        If t < 1 Then
            Return XLLineStyleEnum.Hair
        End If

        If t < 2 Then
            Return XLLineStyleEnum.Thin
        End If

        If t < 3 Then
            Return XLLineStyleEnum.Medium
        End If

        Return XLLineStyleEnum.Thick
    End Function

    Private Shared Function GetBorderBrush(ByVal color As Color) As Brush
        Return If(IsColorValid(color), New SolidColorBrush(color), Nothing)
    End Function

    Private Shared Function IsColorValid(ByVal color As Color) As Boolean
        Return color.A > 0
    End Function
End Class

Public Class clsColumnasFlexGrid
    Public Property intIDColumna As Integer
    Public Property intIDColumnaReal As Integer
    Public Property objColumna As Column
End Class
