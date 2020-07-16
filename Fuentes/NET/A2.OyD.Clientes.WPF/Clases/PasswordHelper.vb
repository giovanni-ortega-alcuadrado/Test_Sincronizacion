Partial Public Class PasswordHelper
    Public Shared ReadOnly PasswordProperty As DependencyProperty = DependencyProperty.RegisterAttached("Password", GetType(String), GetType(PasswordHelper), New FrameworkPropertyMetadata(String.Empty, AddressOf OnPasswordPropertyChanged))
    Public Shared ReadOnly AttachProperty As DependencyProperty = DependencyProperty.RegisterAttached("Attach", GetType(Boolean), GetType(PasswordHelper), New PropertyMetadata(False, AddressOf Attach))
    Private Shared ReadOnly IsUpdatingProperty As DependencyProperty = DependencyProperty.RegisterAttached("IsUpdating", GetType(Boolean), GetType(PasswordHelper))

    Shared Sub SetAttach(ByVal dp As DependencyObject, ByVal value As Boolean)
        dp.SetValue(AttachProperty, value)
    End Sub

    Shared Function GetAttach(ByVal dp As DependencyObject) As Boolean
        Return CBool(dp.GetValue(AttachProperty))
    End Function

    Shared Function GetPassword(ByVal dp As DependencyObject) As String
        Return CStr(dp.GetValue(PasswordProperty))
    End Function

    Shared Sub SetPassword(ByVal dp As DependencyObject, ByVal value As String)
        dp.SetValue(PasswordProperty, value)
    End Sub

    Private Shared Function GetIsUpdating(ByVal dp As DependencyObject) As Boolean
        Return CBool(dp.GetValue(IsUpdatingProperty))
    End Function

    Private Shared Sub SetIsUpdating(ByVal dp As DependencyObject, ByVal value As Boolean)
        dp.SetValue(IsUpdatingProperty, value)
    End Sub

    Private Shared Sub OnPasswordPropertyChanged(ByVal sender As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim passwordBox As PasswordBox = TryCast(sender, PasswordBox)
        RemoveHandler passwordBox.PasswordChanged, AddressOf PasswordChanged

        If Not CBool(GetIsUpdating(passwordBox)) Then
            passwordBox.Password = CStr(e.NewValue)
        End If

        AddHandler passwordBox.PasswordChanged, AddressOf PasswordChanged
    End Sub

    Private Shared Sub Attach(ByVal sender As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim passwordBox As PasswordBox = TryCast(sender, PasswordBox)
        If passwordBox Is Nothing Then Return

        If CBool(e.OldValue) Then
            RemoveHandler passwordBox.PasswordChanged, AddressOf PasswordChanged
        End If

        If CBool(e.NewValue) Then
            AddHandler passwordBox.PasswordChanged, AddressOf PasswordChanged
        End If
    End Sub

    Private Shared Sub PasswordChanged(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim passwordBox As PasswordBox = TryCast(sender, PasswordBox)
        SetIsUpdating(passwordBox, True)
        SetPassword(passwordBox, passwordBox.Password)
        SetIsUpdating(passwordBox, False)
    End Sub
End Class
