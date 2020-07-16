Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports Microsoft.VisualBasic.CompilerServices

'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: FacturasBancaInvView.xaml.vb
'Generado el : 02/24/2012 07:45:33
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class FacturasBancaInvView
    Inherits UserControl
    Private WithEvents buscadorClientes As A2ComunesControl.BuscadorClienteLista


    Public Sub New()
        Me.DataContext = New FacturasBancaInvViewModel
InitializeComponent()
    End Sub
    Private Sub FacturasBancaInv_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        CType(Me.DataContext, FacturasBancaInvViewModel).NombreView = Me.ToString
        Me.dtpDocumento.DisplayDateEnd = CType(Me.DataContext, FacturasBancaInvViewModel).FechaActual
    End Sub

#Region "Eventos Buscadores"

    Private Sub Buscar_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            'CType(Me.DataContext, FacturasBancaInvViewModel).FacturasBancaInSelected.IDComitente = pobjComitente.IdComitente
            CType(Me.DataContext, FacturasBancaInvViewModel).FacturasBancaInSelected.IDComitente = pobjComitente.CodigoOYD
            CType(Me.DataContext, FacturasBancaInvViewModel).FacturasBancaInSelected.Nombre = pobjComitente.Nombre
            CType(Me.DataContext, FacturasBancaInvViewModel).FacturasBancaInSelected.TipoIdentificacion = pobjComitente.CodTipoIdentificacion

            'CType(Me.DataContext, FacturasBancaInvViewModel).FacturasBancaInSelected.Direccion = pobjComitente.
            CType(Me.DataContext, FacturasBancaInvViewModel).FacturasBancaInSelected.Direccion = pobjComitente.DireccionEnvio
            CType(Me.DataContext, FacturasBancaInvViewModel).FacturasBancaInSelected.Telefono = pobjComitente.Telefono
            'SLB20130625 Número de documento alfanúmerico.
            CType(Me.DataContext, FacturasBancaInvViewModel).FacturasBancaInSelected.NroIdentificacion = pobjComitente.NroDocumento
            'If Versioned.IsNumeric(pobjComitente.NroDocumento) Then
            '    CType(Me.DataContext, FacturasBancaInvViewModel).FacturasBancaInSelected.NroIdentificacion = pobjComitente.NroDocumento
            'Else
            '    CType(Me.DataContext, FacturasBancaInvViewModel).FacturasBancaInSelected.NroIdentificacion = 0
            'End If
        End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusquedaComitente(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl.ToLower
                Case "idcomitente"
                    CType(Me.DataContext, FacturasBancaInvViewModel).cb.Comitente = pobjItem.IdComitente
                    CType(Me.DataContext, FacturasBancaInvViewModel).cb.NombreComitente = pobjItem.Nombre
            End Select
        End If
    End Sub

#End Region

#Region "Eventos Controles"

    Private Sub txtComitenteBusqueda_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If CType(Me.DataContext, FacturasBancaInvViewModel).Editando Then
            If Not DirectCast(sender, System.Windows.Controls.TextBox).Text = "" Or Not IsNothing(DirectCast(sender, System.Windows.Controls.TextBox).Text) Then
                CType(Me.DataContext, FacturasBancaInvViewModel).FacturasBancaInSelected.IDComitente = DirectCast(sender, System.Windows.Controls.TextBox).Text
                CType(Me.DataContext, FacturasBancaInvViewModel).buscarComitente(Right(Space(17) & CType(Me.DataContext, FacturasBancaInvViewModel).FacturasBancaInSelected.IDComitente, 17))
            End If
        End If
    End Sub

    Private Sub txtComitenteBusqueda_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If Not (e.Key > 47 And e.Key < 58) And Not e.Key = 9 And Not (e.Key > 95 And e.Key < 106) Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtIVA_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If DirectCast(sender, System.Windows.Controls.TextBox).Text = "" Or IsNothing(DirectCast(sender, System.Windows.Controls.TextBox).Text) Then
            CType(Me.DataContext, FacturasBancaInvViewModel).FacturasBancaInSelected.IVA = Nothing
        Else
            CType(Me.DataContext, FacturasBancaInvViewModel).FacturasBancaInSelected.IVA = CDec(Replace(DirectCast(sender, System.Windows.Controls.TextBox).Text, ",", ""))
        End If
        CType(Me.DataContext, FacturasBancaInvViewModel).TotalDetalleFactura(False)
    End Sub

    Private Sub txtValorDetalle_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        'If DirectCast(sender, System.Windows.Controls.TextBox).Text = "" Or IsNothing(DirectCast(sender, System.Windows.Controls.TextBox).Text) Then
        '    CType(Me.DataContext, FacturasBancaInvViewModel).FacturasBancaInSelected.IVA = Nothing
        'Else
        '    CType(Me.DataContext, FacturasBancaInvViewModel).FacturasBancaInSelected.IVA = CDec(Replace(DirectCast(sender, System.Windows.Controls.TextBox).Text, ",", ""))
        'End If
        CType(Me.DataContext, FacturasBancaInvViewModel).TotalDetalleFactura(True)
    End Sub

    Private Sub txtRetencion_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        If DirectCast(sender, System.Windows.Controls.TextBox).Text = "" Or IsNothing(DirectCast(sender, System.Windows.Controls.TextBox).Text) Then
            CType(Me.DataContext, FacturasBancaInvViewModel).FacturasBancaInSelected.Retencion = Nothing
        Else
            CType(Me.DataContext, FacturasBancaInvViewModel).FacturasBancaInSelected.Retencion = CDec(Replace(DirectCast(sender, System.Windows.Controls.TextBox).Text, ",", ""))
        End If
        CType(Me.DataContext, FacturasBancaInvViewModel).TotalDetalleFactura(False)
    End Sub

#End Region

End Class


