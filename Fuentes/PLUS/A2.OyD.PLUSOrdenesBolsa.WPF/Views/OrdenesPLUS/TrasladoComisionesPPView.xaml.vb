Imports A2.OyD.OYDServer.RIA.Web

Imports Telerik.Windows.Controls
'ViewModel para el registro traslado de comisiones
'Mayo 19 de 2014
'Santiago Vergara
'-------------------------------------------------------------------------


Partial Public Class TrasladoComisionesPPView
    Inherits UserControl

#Region "Constantes y definición de variables"

    Dim objVMA2Utils As A2UtilsViewModel
    Dim objVMTraslado As TrasladoComisionesPPViewModel

#End Region

    Public Sub New()

        'Carga los Estilos de la aplicación de OYDPLUS
        'CambiarEstilosAplicacion.ObtenerEstilos(Me, CambiarEstilosAplicacion.OpcionEstilo.OYDPLUS)

        Dim strClaseCombos As String = "Ord_OyDPLUS"

        objVMA2Utils = New A2UtilsViewModel(strClaseCombos, strClaseCombos)
        Me.Resources.Add("A2VM", objVMA2Utils)

        Me.DataContext = New TrasladoComisionesPPViewModel
InitializeComponent()

    End Sub

    Private Sub Mensajes_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        objVMTraslado = CType(Me.DataContext, TrasladoComisionesPPViewModel)

        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        CType(Me.DataContext, TrasladoComisionesPPViewModel).NombreView = Me.ToString
    End Sub

    Private Sub CancelarEditarRegistro_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        df.CancelEdit()
        If Not IsNothing(df.ValidationSummary) Then
            df.ValidationSummary.DataContext = Nothing
        End If
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

    Private Sub SeleccionarFocoControl(sender As System.Object, e As System.Windows.RoutedEventArgs)
        MyBase.OnGotFocus(e)
        If TypeOf sender Is TextBox Then
            CType(sender, TextBox).Select(0, CType(sender, TextBox).Text.Length + 1)
        ElseIf TypeOf sender Is A2Utilidades.A2NumericBox Then
            'CType(sender, A2Utilidades.A2NumericBox).Select(0, CType(sender, A2Utilidades.A2NumericBox).Value.ToString.Length + 10)
        End If

    End Sub

    Private Sub cm_EventoRefrescarCombos(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles cm.EventoRefrescarCombos
        Try
            If Me.Resources.Contains("A2VM") Then
                CType(Me.DataContext, TrasladoComisionesPPViewModel).IsBusy = True
                CType(Me.Resources("A2VM"), A2UtilsViewModel).EjecutaActualizacionCombos(Me.ToString)
                CType(Me.DataContext, TrasladoComisionesPPViewModel).IsBusy = False
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al refrescar los combos.",
                                 Me.ToString(), "cm_EventoRefrescarCombos", Application.Current.ToString(), Program.Maquina, ex)
        End Try
    End Sub
    Private Sub BuscadorGenericoListaButon_finalizoBusqueda(pstrClaseControl As String, pobjItem AS OYDUtilidades.BuscadorGenerico)
        Try
            If Not IsNothing(pobjItem) Then
                If Not IsNothing(objVMTraslado) Then
                    Select Case pstrClaseControl
                        Case "ReceptorA"
                            objVMTraslado.OperacionesPorReceptorSelected.ReceptorA = pobjItem.IdItem
                            objVMTraslado.OperacionesPorReceptorSelected.NombreReceptorA = pobjItem.IdItem & " - " & pobjItem.Nombre
                        Case "ReceptorB"
                            objVMTraslado.OperacionesPorReceptorSelected.ReceptorB = pobjItem.IdItem
                            objVMTraslado.OperacionesPorReceptorSelected.NombreReceptorB = pobjItem.IdItem & " - " & pobjItem.Nombre
                        Case "ReceptorB_Busqueda"
                            objVMTraslado.cb.ReceptorB = pobjItem.IdItem
                            objVMTraslado.cb.NombreReceptorB = pobjItem.IdItem & " - " & pobjItem.Nombre
                        Case "ReceptorA_Busqueda"
                            objVMTraslado.cb.ReceptorA = pobjItem.IdItem
                            objVMTraslado.cb.NombreReceptorA = pobjItem.IdItem & " - " & pobjItem.Nombre
                    End Select
                End If
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al asignar el elemento seleccionado", Me.Name, "BuscadorGenericoListaButon_finalizoBusqueda", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

End Class


