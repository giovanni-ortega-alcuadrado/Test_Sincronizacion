Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: ConsecutivosDocumentosView.xaml.vb
'Generado el : 04/05/2011 13:47:03
'Propiedad de Alcuadrado S.A. 2010

Imports A2.OyD.OYDServer.RIA.Web


Partial Public Class  ConsecutivosDocumentosView
    Inherits UserControl

#Region "Variables"
    Private mobjVM As ConsecutivosDocumentosViewModel
    Private mlogInicializar As Boolean = True
#End Region

#Region "Inicializacion"

#End Region
    Public Sub New()
        Me.DataContext = New ConsecutivosDocumentosViewModel
InitializeComponent()
    End Sub

    Private Sub View_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        Try
            If mlogInicializar Then
                mlogInicializar = False
                ' Asociar el grid de edición y el data forma a la barra de herramientas que controla la edición
                cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1


                inicializar()

            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema durante la carga del control", Me.Name, "View_Loaded", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

    Private Sub inicializar()
        If Not Me.DataContext Is Nothing Then
            mobjVM = CType(Me.DataContext, ConsecutivosDocumentosViewModel)
            mobjVM.NombreView = Me.ToString
        End If

    End Sub

    Private Sub ConsecutivosDocumentos_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        CType(Me.DataContext, ConsecutivosDocumentosViewModel).NombreColeccionDetalle = Me.ToString
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
        If Not IsNothing(df.ValidationSummary) Then
            If df.ValidationSummary.HasErrors Then
                df.CancelEdit()
            Else
                df.CommitEdit()
            End If
        End If
    End Sub
    

    'Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    '    df.ValidateItem()
    'End Sub

#Region "Eventos detalles"
    Private Sub BuscadorGenerico_finalizoBusqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl
                Case "compania"
                    If Not IsNothing(mobjVM) And Not IsNothing(pobjItem) Then
                        Me.mobjVM.ConsecutivosDocumentoSelected.Compania = pobjItem.IdItem
                        Me.mobjVM.ConsecutivosDocumentoSelected.NombreCompania = pobjItem.CodigoAuxiliar
                        Me.mobjVM.CompaniaConDoc = pobjItem.IdItem
                    End If
                Case "IDCuentaContable"
                    If Not IsNothing(mobjVM) And Not IsNothing(pobjItem) Then
                        Me.mobjVM.ConsecutivosDocumentoSelected.CuentaContable1 = pobjItem.IdItem
                    End If
            End Select
        End If
    End Sub

#End Region

End Class


