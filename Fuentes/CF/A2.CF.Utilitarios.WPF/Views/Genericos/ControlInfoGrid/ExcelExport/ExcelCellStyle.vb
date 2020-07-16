
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Data
Imports System.Diagnostics
Imports System.Net
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Ink
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.ComponentModel
Imports C1.WPF.FlexGrid



Public Class ExcelCellStyle
        Inherits CellStyle

        Private _format As String

        Private _bdrThickness As Thickness

        Private _bdrLeft As Brush

        Private _bdrTop As Brush

        Private _bdrRight As Brush

        Private _bdrBottom As Brush

        Private Shared _thicknessEmpty As Thickness = New Thickness(0)

        Public Property Format As String
            Get
                Return _format
            End Get

            Set(ByVal value As String)
                If value <> _format Then
                    _format = value
                    OnPropertyChanged(New PropertyChangedEventArgs("Format"))
                End If
            End Set
        End Property

        Public Property CellBorderThickness As Thickness
            Get
                Return _bdrThickness
            End Get

            Set(ByVal value As Thickness)
                If value <> _bdrThickness Then
                    _bdrThickness = value
                    OnPropertyChanged(New PropertyChangedEventArgs("BorderThickness"))
                End If
            End Set
        End Property

        Public Property CellBorderBrushLeft As Brush
            Get
                Return _bdrLeft
            End Get

            Set(ByVal value As Brush)
                If Not Object.ReferenceEquals(value, _bdrLeft) Then
                    _bdrLeft = value
                    OnPropertyChanged(New PropertyChangedEventArgs("BorderColorLeft"))
                End If
            End Set
        End Property

        Public Property CellBorderBrushTop As Brush
            Get
                Return _bdrTop
            End Get

            Set(ByVal value As Brush)
                If Not Object.ReferenceEquals(value, _bdrTop) Then
                    _bdrTop = value
                    OnPropertyChanged(New PropertyChangedEventArgs("BorderColorTop"))
                End If
            End Set
        End Property

        Public Property CellBorderBrushRight As Brush
            Get
                Return _bdrRight
            End Get

            Set(ByVal value As Brush)
                If Not Object.ReferenceEquals(value, _bdrRight) Then
                    _bdrRight = value
                    OnPropertyChanged(New PropertyChangedEventArgs("BorderColorRight"))
                End If
            End Set
        End Property

        Public Property CellBorderBrushBottom As Brush
            Get
                Return _bdrBottom
            End Get

            Set(ByVal value As Brush)
                If Not Object.ReferenceEquals(value, _bdrBottom) Then
                    _bdrBottom = value
                    OnPropertyChanged(New PropertyChangedEventArgs("BorderColorBottom"))
                End If
            End Set
        End Property

        Public Overrides Sub Apply(ByVal bdr As Border, ByVal selState As SelectedState)
        MyBase.Apply(bdr)
        ApplyBorder(bdr, _bdrLeft, New Thickness(_bdrThickness.Left, 0, 0, 0))
            ApplyBorder(bdr, _bdrTop, New Thickness(0, _bdrThickness.Top, 0, 0))
            ApplyBorder(bdr, _bdrRight, New Thickness(0, 0, _bdrThickness.Right, 0))
            ApplyBorder(bdr, _bdrBottom, New Thickness(0, 0, 0, _bdrThickness.Bottom))
        End Sub

        Private Sub ApplyBorder(ByVal bdr As Border, ByVal br As Brush, ByVal t As Thickness)
            If br IsNot Nothing AndAlso t <> _thicknessEmpty Then
            Dim inner As Object = New Border()
            inner.BorderThickness = t
                inner.BorderBrush = br
            Dim content As Object = bdr.Child
            bdr.Child = inner
                inner.Child = content
                inner.Padding = bdr.Padding
                bdr.Padding = _thicknessEmpty
            End If
        End Sub
    End Class


