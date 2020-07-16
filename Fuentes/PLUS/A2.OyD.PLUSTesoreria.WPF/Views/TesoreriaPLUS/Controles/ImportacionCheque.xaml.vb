Imports Telerik.Windows.Controls
Imports System.IO


'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: ImportacionLiqView.xaml.vb
'Generado el : 07/19/2011 09:26:12
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class ImportacionCheque
    Inherits Window
    Dim objVM_OT_OyDPlus As TesoreriaViewModel_OYDPLUS
    Dim vm As ImportacionChequeViewModel
    Public logEntro As Boolean = False
   
    Public Enum TipoArchivoTransaccion
        Cheque
        Transferencia
    End Enum

    Public Sub New(pstrFormaPago As String, pobjVM_OT_OyDPlus As TesoreriaViewModel_OYDPLUS, pstrNombreArchivo As String)
        strFormapagoImportacion = pstrFormaPago
        strNombreArchivoCarga = pstrNombreArchivo
        objVM_OT_OyDPlus = pobjVM_OT_OyDPlus
        InitializeComponent()


    End Sub

    Private Sub ImportacionDecevalView_Loaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        'vm = CType(Me.DataContext, ImportacionChequeViewModel)
        vm = CType(Me.Resources("VMImportacionChequeViewModel"), ImportacionChequeViewModel)
        Me.DataContext = vm
        CType(Me.DataContext, ImportacionChequeViewModel).NombreView = Me.ToString
        If Not IsNothing(objVM_OT_OyDPlus) Then
            vm.objVM_TesoreriaOYDPLUS = objVM_OT_OyDPlus
        End If
        vm.NombreArchivoSeleccionado = strNombreArchivoCarga
    End Sub

    Private Sub cbArchivosASubir_SelectionChanged(sender As Object, e As System.Windows.Controls.SelectionChangedEventArgs)
        vm.LimpiarResultados()
    End Sub

    Private Sub Button_Click(sender As System.Object, e As System.Windows.RoutedEventArgs)
        vm.CargarArchivo(strFormapagoImportacion, strNombreArchivoCarga)
    End Sub

        'Private Sub Window_Closed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Closed
    '    'Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, True)
    'End Sub
   
End Class


