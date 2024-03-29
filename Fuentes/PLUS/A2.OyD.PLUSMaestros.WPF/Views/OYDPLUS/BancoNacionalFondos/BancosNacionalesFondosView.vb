﻿Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls
'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: CostosView.xaml.vb
'Generado el : 11/15/2012 07:29:09
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class BancosNacionalesFondosView
    Inherits UserControl

    Dim objVM As BancosNacionalesFondosViewModel

    Public Sub New()
        'Carga los Estilos de la aplicación de OYDPLUS
        'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

        Me.DataContext = New BancosNacionalesFondosViewModel
InitializeComponent()

    End Sub

    Private Sub BancoNacionalFondos_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        objVM = CType(Me.DataContext, BancosNacionalesFondosViewModel)
        objVM.NombreView = Me.ToString
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
                objVM.IsBusy = True
                CType(Me.Resources("A2VM"), A2UtilsViewModel).EjecutaActualizacionCombos(Me.ToString)
                objVM.IsBusy = False
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

    Private Sub SeleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        MyBase.OnGotFocus(e)
        If TypeOf sender Is TextBox Then
            CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
            'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
        End If
    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(pstrClaseControl As System.String, pobjItem As OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If pstrClaseControl = "bancosnacionales" Then
                    objVM.BancoNacionalFondoSelected.IDBanco = pobjItem.IdItem
                    objVM.BancoNacionalFondoSelected.NombreBanco = pobjItem.Nombre
                Else
                    objVM.cb.IDBanco = pobjItem.IdItem
                    objVM.cb.Banco = pobjItem.Nombre
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al finalizar la busqueda de bancos.", _
             Me.ToString(), "BuscadorGenerico_finalizoBusqueda", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub

End Class


