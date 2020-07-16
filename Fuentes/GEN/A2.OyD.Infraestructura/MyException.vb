Public Class MyException
    Inherits Exception

    Private objException As Exception

    Public Sub New(ByVal pobjException As Exception)
        MyBase.New(pobjException.Message, pobjException.InnerException)
    End Sub

    Public Overrides ReadOnly Property StackTrace As String
        Get
            Return String.Empty
        End Get
    End Property

End Class
