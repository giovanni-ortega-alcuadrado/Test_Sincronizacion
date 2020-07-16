Imports Telerik.Windows.Controls
Public Class Menu

    Public Sub New()
        'do nothing
    End Sub

    Public Sub New(ByVal nombre As String)
        Me.Nombre = nombre
    End Sub

    Public Sub New(ByVal codigo As String, ByVal nombre As String)
        Me.Codigo = codigo
        Me.Nombre = nombre
    End Sub

    Public Property Codigo As String

    Public Property Nombre As String

    Public Property SubMenus() As List(Of Menu)

End Class

