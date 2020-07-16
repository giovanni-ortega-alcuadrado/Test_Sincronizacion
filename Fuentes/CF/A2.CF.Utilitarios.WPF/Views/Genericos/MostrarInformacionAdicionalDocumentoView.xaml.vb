Imports Telerik.Windows.Controls
Imports A2Utilidades.Mensajes
Imports System.Xml.Linq

Partial Public Class MostrarInformacionAdicionalDocumentoView
    Inherits Window

    Public Sub New(ByVal pstrInformacionAdicional As String)
        Try
            InitializeComponent()
            ArmarInformacionMostrar(pstrInformacionAdicional)
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Falló la inicialización de mostrar información adicional.", Me.Name, "MostrarInformacionAdicionalDocumentoView", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub ArmarInformacionMostrar(ByVal pstrInformacionAdicional As String)
        Try
            Dim xmlDetalle As XDocument = XDocument.Parse(pstrInformacionAdicional)

            If Not IsNothing(xmlDetalle) Then
                Dim objInfoAdicional = From info In xmlDetalle.Descendants
                                       Select info

                If Not IsNothing(objInfoAdicional) Then
                    For Each objElementosInfoAdicional As XElement In objInfoAdicional
                        For Each objInfo In objElementosInfoAdicional.Elements
                            Dim objTreeViewItemEncabezado As New TreeViewItem
                            Dim objTreeViewItemValor As New TreeViewItem

                            objTreeViewItemEncabezado.Header = objInfo.Name
                            objTreeViewItemEncabezado.IsExpanded = True
                            objTreeViewItemValor.Header = objInfo.Value
                            objTreeViewItemValor.IsExpanded = True

                            objTreeViewItemEncabezado.Items.Add(objTreeViewItemValor)

                            treeViewInformacion.Items.Add(objTreeViewItemEncabezado)
                        Next
                    Next
                End If
            End If

            txtInformacionAdicional.Visibility = Visibility.Collapsed
        Catch ex As Exception
            txtInformacionAdicional.Text = "El xml de la información adicional no tiene un formato valido:" & vbCrLf & pstrInformacionAdicional
            treeViewInformacion.Visibility = Visibility.Collapsed
        End Try
    End Sub

End Class
