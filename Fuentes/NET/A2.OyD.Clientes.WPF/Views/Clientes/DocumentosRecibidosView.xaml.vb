Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports System.Collections.ObjectModel


Partial Public Class DocumentosRecibidosView
    Inherits UserControl

    Public Sub New()
        Me.DataContext = New DocumentosRecibidosViewModel
InitializeComponent()
    End Sub

    Private Sub DocumentosRecibidosViewModel_Loaded(sender As Object, e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        CType(Me.DataContext, DocumentosRecibidosViewModel).NombreView = Me.ToString
    End Sub

    Private Sub BuscadorClienteListaButon_finalizoBusqueda(pstrClaseControl As String, pobjComitente AS OYDUtilidades.BuscadorClientes)

        If Not IsNothing(pobjComitente) Then
            CType(Me.DataContext, DocumentosRecibidosViewModel).lngIdComitente = pobjComitente.IdComitente
            CType(Me.DataContext, DocumentosRecibidosViewModel).strTipoIdentificacion = pobjComitente.CodTipoIdentificacion
            CType(Me.DataContext, DocumentosRecibidosViewModel).strNroDocumento = pobjComitente.NroDocumento
            CType(Me.DataContext, DocumentosRecibidosViewModel).strNombre = pobjComitente.Nombre
            CType(Me.DataContext, DocumentosRecibidosViewModel).ConsultarDocumentos()
        End If
    End Sub

    Private Sub btnCargar_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, DocumentosRecibidosViewModel).LlamarServicio()
    End Sub

Private Sub txtNroIdentificacion_LostFocus(sender As Object, e As RoutedEventArgs) Handles txtCliente.LostFocus
        CType(Me.DataContext, DocumentosRecibidosViewModel).ConsultarDatosCliente()
    End Sub
End Class


