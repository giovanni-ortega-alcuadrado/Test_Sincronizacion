Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Data
Imports System.Diagnostics
Imports C1.WPF.FlexGrid
Imports C1.WPF.Excel
Imports System.Globalization
Imports System.Windows

Public Class ExcelRow
        Inherits GroupRow

        Private _cellStyles As Dictionary(Of Column, CellStyle)

        Private Const DEFAULT_FORMAT As String = "#,##0.######"

        Public Sub New(ByVal styleRow As ExcelRow)
            IsReadOnly = False
            If styleRow IsNot Nothing AndAlso styleRow.Grid IsNot Nothing Then
                For Each c In styleRow.Grid.Columns
                Dim cs As Object = styleRow.GetCellStyle(c)
                If cs IsNot Nothing Then
                        Me.SetCellStyle(c, cs.Clone())
                    End If
                Next
            End If
        End Sub

        Public Sub New()
        End Sub

        Public Overrides Function GetDataFormatted(ByVal col As Column) As String
        Dim data As Object = GetDataRaw(col)
        Dim ifmt As Object = TryCast(data, IFormattable)
        If ifmt IsNot Nothing Then
            Dim s As Object = TryCast(GetCellStyle(col), ExcelCellStyle)
            Dim fmt As Object = If(s IsNot Nothing AndAlso (Not String.IsNullOrEmpty(s.Format)), s.Format, DEFAULT_FORMAT)
            data = ifmt.ToString(fmt)
        End If

            Return If(data IsNot Nothing, data.ToString(), String.Empty)
        End Function

        Public Sub SetCellStyle(ByVal col As Column, ByVal style As CellStyle)
            If Not Object.ReferenceEquals(style, GetCellStyle(col)) Then
                If _cellStyles Is Nothing Then
                    _cellStyles = New Dictionary(Of Column, CellStyle)()
                End If

                _cellStyles(col) = style
                If Grid IsNot Nothing Then
                    Grid.Invalidate(New CellRange(Me.Index, col.Index))
                End If
            End If
        End Sub

        Public Function GetCellStyle(ByVal col As Column) As CellStyle
            Dim s As CellStyle = Nothing
            If _cellStyles IsNot Nothing Then
                _cellStyles.TryGetValue(col, s)
            End If

            Return s
        End Function
    End Class

