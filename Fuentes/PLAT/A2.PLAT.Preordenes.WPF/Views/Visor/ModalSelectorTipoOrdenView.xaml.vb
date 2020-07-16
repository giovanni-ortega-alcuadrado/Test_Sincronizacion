Imports A2Utilidades

Public Class ModalSelectorTipoOrdenView

    Public TipoOrdenSeleccionado As String = String.Empty
    Public Sub New(ByVal pstrTitulo As String, ByVal pstrTituloEtiqueta As String, ByVal pstrMensajeRegistro As String, ByVal pobjListaSeleccion As List(Of ProductoCombos), ByVal pstrEtiquetaAceptar As String, ByVal pstrEtiquetaCancelar As String)
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Title = pstrTitulo
        Me.dataFieldSelectorTipoOrden.Label = pstrTituloEtiqueta
        Me.txtMensajeUsuario.Text = pstrMensajeRegistro
        Me.cboSelectorTipoOrden.ItemsSource = pobjListaSeleccion
        Me.btnAceptar.Content = pstrEtiquetaAceptar
        Me.btnCancelar.Content = pstrEtiquetaCancelar
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

    End Sub

    Private Sub BtnAceptar_Click(sender As Object, e As RoutedEventArgs)
        If Not String.IsNullOrEmpty(cboSelectorTipoOrden.SelectedValue) Then

            TipoOrdenSeleccionado = cboSelectorTipoOrden.SelectedValue
            Me.DialogResult = True
        End If
    End Sub

    Private Sub BtnCancelar_Click(sender As Object, e As RoutedEventArgs)
        Me.DialogResult = False
    End Sub
End Class
