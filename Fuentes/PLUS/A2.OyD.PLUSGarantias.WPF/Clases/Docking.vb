Imports C1.WPF
Imports C1.WPF.Docking

Public Class A2DockTabControl
    Inherits C1DockTabControl

    Public Sub New()
        'CanUserFloat = False
        'CanUserHide = False
        TabStripPlacement = Dock.Bottom
        TabItemShape = C1TabItemShape.Sloped
        BorderThickness = New Thickness(0)
        'TabItemClose = C1.Silverlight.C1TabItemCloseOptions.None
    End Sub

End Class

Public Class A2DockControl
    Inherits C1DockControl

    Public Property Dock As Dock = Dock.Bottom

    Protected Overrides Function CreateDockTabControlOverride() As C1DockTabControl
        Dim tab As New A2DockTabControl()
        tab.Dock = Dock
        tab.DockWidth = 250
        Return tab
    End Function
End Class



Public Class A2DockTabItem
    Inherits C1DockTabItem

End Class
