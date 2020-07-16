Public Class DesaprobacionModalView
    Inherits UserControl

#Region "Inicializacion"
    Public Sub New()

        Dim vm = New CorreosViewModel

        'Añadir recurso para utilizar contexto de datos del viewmodel
        Me.Resources.Add("uvm", vm)

        InitializeComponent()
    End Sub

#End Region

End Class
