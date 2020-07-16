Imports Telerik.Windows.Controls
Imports System.ComponentModel

Partial Public Class PruebaVentanaEmergenteTesoreria
    Inherits UserControl

    Public Sub New()
        InitializeComponent()

        Dim objLista As New List(Of ObjetoTipoTesoreria)
        objLista.Add(New ObjetoTipoTesoreria With {.TipoTesoreria = "CE", .Descripcion = "Comprobantes de egreso"})
        objLista.Add(New ObjetoTipoTesoreria With {.TipoTesoreria = "RC", .Descripcion = "Recibo de caja"})
        objLista.Add(New ObjetoTipoTesoreria With {.TipoTesoreria = "N", .Descripcion = "Notas contables"})

        cboTipoTesoreria.ItemsSource = objLista
        cboTipoTesoreria.SelectedValue = "CE"
    End Sub

    Private Sub btnProcesar_Click(sender As Object, e As RoutedEventArgs)
        Dim objVentanaEmergente As New TesoreriaVentanaEmergenteView(cboTipoTesoreria.SelectedValue, txtIDDocumento.Text, txtNombreConsecutivo.Text)
        AddHandler objVentanaEmergente.Closed, AddressOf TerminoGenerarDocumento
        Program.Modal_OwnerMainWindowsPrincipal(objVentanaEmergente)
        objVentanaEmergente.ShowDialog()
    End Sub

    Private Sub TerminoGenerarDocumento(ByVal sender As Object, ByVal e As EventArgs)
        Dim objVentanaEmergente As TesoreriaVentanaEmergenteView = CType(sender, TesoreriaVentanaEmergenteView)
        If Not IsNothing(objVentanaEmergente) Then
            Dim strDocumentoCreado As String = String.Empty

            If objVentanaEmergente.DocumentoCreado Then
                strDocumentoCreado = String.Format("Documento creado, IDDocumento:{0} - Fecha documento:{1:yyyy-MM-dd} - Nombre Consecutivo:{2} - Tipo documento:{3}",
                                                   objVentanaEmergente.IDDocumentoActualizado,
                                                   objVentanaEmergente.FechaDocumentoActualizado,
                                                   objVentanaEmergente.NombreConsecutivoActualizado,
                                                   objVentanaEmergente.TipoRegistroActualizado)
                A2Utilidades.Mensajes.mostrarMensaje(strDocumentoCreado, Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Exito)
            Else
                A2Utilidades.Mensajes.mostrarMensaje("Se cancelo la creación del documento.", Program.TituloSistema, A2Utilidades.wppMensajes.TiposMensaje.Advertencia)
            End If
        End If
    End Sub

End Class

Public Class ObjetoTipoTesoreria
    Implements INotifyPropertyChanged

    Private _TipoTesoreria As String
    Public Property TipoTesoreria() As String
        Get
            Return _TipoTesoreria
        End Get
        Set(ByVal value As String)
            _TipoTesoreria = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("TipoTesoreria"))
        End Set
    End Property

    Private _Descripcion As String
    Public Property Descripcion() As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Descripcion"))
        End Set
    End Property

    Public Event PropertyChanged(sender As Object, e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged
End Class
