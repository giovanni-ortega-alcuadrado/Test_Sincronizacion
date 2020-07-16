Imports Telerik.Windows.Controls
Public Class Accion

    Private _accion As String

    Private Sub New(nombreAccion As String)
        _accion = nombreAccion
    End Sub

    Public ReadOnly Property nombreAccion As String
        Get
            Return _accion
        End Get
    End Property

    Public Shared ReadOnly Rechazada As Accion = New Accion("La orden {0} ha sido rechazada")
    Public Shared ReadOnly MarcarLanzada As Accion = New Accion("Se marcó la orden como lanzada")
    Public Shared ReadOnly LanzadaSAE As Accion = New Accion("La orden {0} fue enrutada por el sistema SAE")
    Public Shared ReadOnly Ninguna As Accion = New Accion(Nothing)
End Class
