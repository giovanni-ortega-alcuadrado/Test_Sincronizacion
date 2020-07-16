Partial Public Class ucPopupAyuda
    Inherits Window

    Private _strURL As String = ""

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(pstrURL As String)
        MyBase.New()
        InitializeComponent()
        _strURL = pstrURL
        'htmlbr.Visibility = Visibility.Collapsed
        'htmlbr.SourceUri = New Uri(_strURL, UriKind.Absolute)
        BI.BusyContent = "Cargando..."
        BI.IsBusy = True

    End Sub

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs) Handles OKButton.Click
        BI.BusyContent = "Cerrando..."
        BI.IsBusy = True
        'htmlbr.Visibility = Visibility.Collapsed
        Me.DialogResult = True
    End Sub

    'Private Sub htmlbr_UriLoaded(sender As Object, e As System.EventArgs) Handles htmlbr.UriLoaded
    '    BI.IsBusy = False
    '    htmlbr.Visibility = Visibility.Visible
    'End Sub

    Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
        'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    End Sub

End Class
