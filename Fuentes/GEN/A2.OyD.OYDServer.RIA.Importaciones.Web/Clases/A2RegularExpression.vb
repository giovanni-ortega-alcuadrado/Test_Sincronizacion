Imports System.ComponentModel.DataAnnotations

Public Class A2RegularExpression
    Inherits RegularExpressionAttribute
    Public Sub New(pattern As String)
        MyBase.New(pattern)
    End Sub

    Private Shadows m_MatchTimeoutInMilliseconds As Integer
    Protected Shadows Property MatchTimeoutInMilliseconds() As Integer
        Get
            Return m_MatchTimeoutInMilliseconds
        End Get
        Set
            m_MatchTimeoutInMilliseconds = Value
        End Set
    End Property

End Class
