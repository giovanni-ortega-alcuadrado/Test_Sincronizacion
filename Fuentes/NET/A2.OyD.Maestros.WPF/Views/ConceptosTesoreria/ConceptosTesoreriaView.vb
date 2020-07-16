Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: ConceptosTesoreriaView.xaml.vb
'Generado el : 02/15/2011 13:33:49
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class  ConceptosTesoreriaView
    Inherits UserControl
    Private mlogInicializar As Boolean = True
    Private mobjVM As ConceptosTesoreriaViewModel
    Private mobjVM1 As ConceptosTesoreriaViewModel

    Public Sub New()
        Me.DataContext = New ConceptosTesoreriaViewModel
InitializeComponent()
    End Sub

    Private Sub ConceptosTesoreria_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        If mlogInicializar Then
            mlogInicializar = False
            inicializar()
        End If
    End Sub

    Private Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            mobjVM = CType(Me.DataContext, ConceptosTesoreriaViewModel)
            mobjVM.NombreView = Me.ToString

            mobjVM.inicializar()
        End If
    End Sub

    Private Sub txtCuenta_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        Try
            If Not (e.Key > 47 And e.Key < 58) And Not e.Key = 9 And Not (e.Key > 95 And e.Key < 106) Then
                e.Handled = True
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al validar el digito.", _
                                Me.ToString(), "txtCuenta_KeyDown", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub



    Private Sub CancelarEditarRegistro_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        df.CancelEdit()
        If Not IsNothing(df.ValidationSummary) Then
            df.ValidationSummary.DataContext = Nothing
        End If
    End Sub

    Private Sub cm_EventoRefrescarCombos(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoRefrescarCombos
        Try
            If Me.Resources.Contains("A2VM") Then
                mobjVM.IsBusy = True
                CType(Me.Resources("A2VM"), A2UtilsViewModel).EjecutaActualizacionCombos(Me.ToString)
                mobjVM.IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar los combos.",
                                 Me.ToString(), "cm_EventoRefrescarCombos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub cm_EventoConfirmarGrabacion(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoConfirmarGrabacion
        df.ValidateItem()
        If df.ValidationSummary.HasErrors Then
            df.CancelEdit()
        Else
            'Dim concepto As CheckBox
            'concepto = df.FindName("chkConcepto")
            'If concepto.IsChecked Then
            '    'Cambiar el valor de "AplicaA" --> "Egresos" para enviarlo a la BD
            '    Dim pru As String = "aaa"
            'End If
            df.CommitEdit()
        End If
    End Sub
    
    'Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    '    df.ValidateItem()
    'End Sub
    Private Sub ctlBuscadorConcepto_finalizoBusqueda(pstrClaseControl As String, pobjItem AS OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                CType(Me.DataContext, ConceptosTesoreriaViewModel).ConceptosTesoreriSelected.Retencion = pobjItem.CodItem
                CType(Me.DataContext, ConceptosTesoreriaViewModel).ConceptosTesoreriSelected.DescripcionRetencion = String.Format("{0} - {1}", pobjItem.CodItem, pobjItem.Nombre)
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar la retención.", _
                                                Me.ToString(), "ctlBuscadorConcepto_finalizoBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try

    End Sub

    Private Sub btnLimpiarRetencion_Click(sender As Object, e As RoutedEventArgs)
        Try
            CType(Me.DataContext, ConceptosTesoreriaViewModel).ConceptosTesoreriSelected.Retencion = Nothing
            CType(Me.DataContext, ConceptosTesoreriaViewModel).ConceptosTesoreriSelected.DescripcionRetencion = Nothing
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al seleccionar la retención.", _
                                                Me.ToString(), "btnLimpiarRetencion_Click", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

    Private Sub TextBox_KeyDown_1(sender As Object, e As KeyEventArgs)
        'JBT valida que solo deje entrar numeros,guiones y espacios en blanco ya que son numeros de cuentas JBT20140304
        If Not (e.Key > 47 And e.Key < 58) And Not e.Key = 9 And Not (e.Key > 95 And e.Key < 106) And Not e.Key = 189 And Not e.Key = 32 And Not e.Key = 109 Then
            e.Handled = True
        End If
    End Sub

    'DEMC20170914
    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem AS OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl.ToLower
                Case "idcuentacontable"
                    CType(Me.DataContext, ConceptosTesoreriaViewModel).ConceptosTesoreriSelected.CuentaContable = pobjItem.IdItem
                Case "idcuentacontabledb"
                    CType(Me.DataContext, ConceptosTesoreriaViewModel).ConceptosTesoreriSelected.CuentaContable = pobjItem.IdItem
                Case "idcuentacontablecr"
                    CType(Me.DataContext, ConceptosTesoreriaViewModel).ConceptosTesoreriSelected.CuentaContableAux = pobjItem.IdItem
                Case "idcuentacontablecrdife"
                    CType(Me.DataContext, ConceptosTesoreriaViewModel).ConceptosTesoreriSelected.CuentaContableCRDiferido = pobjItem.IdItem
                Case "idcuentacontabledbdife"
                    CType(Me.DataContext, ConceptosTesoreriaViewModel).ConceptosTesoreriSelected.CuentaContableDBDiferido = pobjItem.IdItem

            End Select
        End If
    End Sub
    'DEMC20170914

    'JCM20180712
    Private Sub BuscadorGenericoListaButon_GotFocus(sender As Object, e As RoutedEventArgs)
        mobjVM1 = CType(Me.DataContext, ConceptosTesoreriaViewModel)
        CType(sender, A2ComunesControl.BuscadorGenericoListaButon).TipoItem = mobjVM1.strTipoItem
    End Sub

    Private Sub BuscadorGenericoListaButon_GotFocus1(sender As Object, e As RoutedEventArgs)
        mobjVM1 = CType(Me.DataContext, ConceptosTesoreriaViewModel)
        CType(sender, A2ComunesControl.BuscadorGenericoListaButon).TipoItem = mobjVM1.strTipoItem
    End Sub


    Private Sub BuscadorGenericoListaButon_GotFocus2(sender As Object, e As RoutedEventArgs)
        mobjVM1 = CType(Me.DataContext, ConceptosTesoreriaViewModel)
        CType(sender, A2ComunesControl.BuscadorGenericoListaButon).TipoItem = mobjVM1.strTipoItem
    End Sub
    'JCM20180712
End Class


