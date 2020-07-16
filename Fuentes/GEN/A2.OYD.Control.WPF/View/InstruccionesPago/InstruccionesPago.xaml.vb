Partial Public Class InstruccionesPago
    Inherits UserControl

    Public Sub New 
        InitializeComponent()
    End Sub

    Private Sub InstruccionesPago_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Dim colInstrucciones As List(Of Instruccion) = New List(Of Instruccion)

        colInstrucciones.Add(New Instruccion("Transferencia ACH", True, 500000000, ""))
        colInstrucciones.Add(New Instruccion("Consignación bancaria", False, 250000000, ""))
        colInstrucciones.Add(New Instruccion("Cliente recoge cheque", False, 250000000, "Entregar solamente al cliente"))

        Me.dgrdInstrucciones.ItemsSource = colInstrucciones

    End Sub

#Region "Administrar instrucciones"

    Friend Sub adicionarInstruccion(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim objInst As wcInstruccionesEditar = New wcInstruccionesEditar
        Program.Modal_OwnerMainWindowsPrincipal(objInst)
        objInst.ShowDialog()
    End Sub

    Friend Sub editarInstruccion(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Dim objInst As wcInstruccionesEditar = New wcInstruccionesEditar
        Program.Modal_OwnerMainWindowsPrincipal(objInst)
        objInst.ShowDialog()
        MessageBox.Show("Editar")
    End Sub

    Friend Sub borrarInstruccion(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        MessageBox.Show("Borrar")
    End Sub
#End Region

End Class

Public Class Instruccion
    Public Property Instruccion As String
    Public Property PorDefecto As Boolean
    Public Property Valor As Double
    Public Property Observaciones As String

    Public Sub New(ByVal pstrInstruccion As String, ByVal plogPorDefecto As Boolean, ByVal pdbValor As Double, ByVal pstrObservaciones As String)
        Instruccion = pstrInstruccion
        PorDefecto = plogPorDefecto
        Valor = pdbValor
        Observaciones = pstrObservaciones
    End Sub
End Class
