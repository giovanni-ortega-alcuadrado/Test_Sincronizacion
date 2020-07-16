Imports Telerik.Windows.Controls
Imports System.Windows.Data

Public Class MenuItemCommand
    Public Property id As Int16
    Public Property nombre As String
    Public Property parametro As Object
    Public Property esSeparador As Boolean
    Public Property Command As ICommand
    Public Property Descripcion As String
    Public Enum accionesMenu
        mostrarVisor
        lanzarSAE
        MarcarComoLanzada
        Rechazar
        AsociarLiquidaciones
    End Enum
End Class
Public Class CustomContextMenu
    Inherits ContextMenu

    Protected Overrides Function GetContainerForItemOverride() As DependencyObject

        Dim item As New CustomMenuItem()

        Dim commandBinding As New Binding("Command")
        item.SetBinding(CustomMenuItem.CommandProperty, commandBinding)

        Dim commandParameter As New Binding("CommandParameter")
        item.SetBinding(CustomMenuItem.CommandParameterProperty, commandParameter)

        Return item

    End Function

End Class



Public Class CustomMenuItem
    Inherits MenuItem

    Protected Overrides Function GetContainerForItemOverride() As DependencyObject


        Dim item As New CustomMenuItem()

        Dim commandBinding As New Binding("Command")
        item.SetBinding(CustomMenuItem.CommandProperty, commandBinding)

        Return item

    End Function

End Class