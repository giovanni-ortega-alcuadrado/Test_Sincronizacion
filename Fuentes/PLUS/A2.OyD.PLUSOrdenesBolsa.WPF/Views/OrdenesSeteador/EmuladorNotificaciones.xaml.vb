Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports A2ComunesControl
Imports A2.OyD.OYDServer.RIA.Web.OyDPLUSOrdenesBolsa

Partial Public Class EmuladorNotificaciones
    Inherits UserControl

    Private _mobjVM As OrdenSeteadorViewModel


    Public Sub New()
        InitializeComponent()
        _mobjVM = Application.Current.Resources("VM")
        Me.DataContext = _mobjVM
    End Sub

End Class
