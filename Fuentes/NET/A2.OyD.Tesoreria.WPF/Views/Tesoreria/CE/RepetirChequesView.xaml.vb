Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes

'Codigo Desarrollado por: Santiago Alexander Vergara Orrego
'Archivo: Public Class RepetirChequesView.vb
'Propiedad de Alcuadrado S.A. 
'Junio 28/2013

Partial Public Class RepetirChequesView
    Inherits UserControl

#Region "Variables"

    Private mlogInicializado As Boolean = False
    Dim objA2VM As A2UtilsViewModel
    Private strClaseCombos As String = ""
    Private mstrDicCombosEspecificos As String = String.Empty

#End Region
    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl.ToLower
                Case "compania"
                    CType(Me.DataContext, RepetirChequesViewModel).logConsultarCompania = False
                    CType(Me.DataContext, RepetirChequesViewModel).IDCompania = pobjItem.IdItem
                    CType(Me.DataContext, RepetirChequesViewModel).NombreCompania = pobjItem.Nombre
                    CType(Me.DataContext, RepetirChequesViewModel).logConsultarCompania = True
                    CType(Me.DataContext, RepetirChequesViewModel).ConsultarConsecutivosCompania()
            End Select
        End If
    End Sub

    Private Sub C1NumericBox_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            Dim IDDigitado As Nullable(Of Integer) = CType(sender, A2Utilidades.A2NumericBox).Value

            If IDDigitado > 0 Then
                CType(Me.DataContext, RepetirChequesViewModel).buscarCompania(IDDigitado, "buscarCompaniaEncabezado")
            Else
                CType(Me.DataContext, RepetirChequesViewModel).NombreCompania = String.Empty
                CType(Me.DataContext, RepetirChequesViewModel).LimpiarConsecutivoCompania()
            End If
        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la compañia del banco", Me.ToString(), "C1NumericBox_LostFocus", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Public Sub New()
        Dim objA2VM As A2UtilsViewModel
        Dim strModuloTesoreria As String = ""

        Try
            'Si el recurso no esta vacio, se remueve
            If Not String.IsNullOrEmpty(Application.Current.Resources("moduloTesoreria")) Then
                Application.Current.Resources.Remove("moduloTesoreria")
            End If

            strClaseCombos = "Tesoreria_ComprobantesEgreso"
            mstrDicCombosEspecificos = String.Format("Combos_{0}", Me.ToString)
            Application.Current.Resources.Add("moduloTesoreria", TesoreriaViewModel.ClasesTesoreria.CE)


            objA2VM = New A2UtilsViewModel(strClaseCombos, mstrDicCombosEspecificos)
            Me.Resources.Add("A2VM", objA2VM)

            mlogInicializado = True
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "New (Overload)", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

        Me.DataContext = New RepetirChequesViewModel
InitializeComponent()
    End Sub

#Region "Eventos"

    Private Sub btnConsultarEgreso_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, RepetirChequesViewModel).BuscarChequeRepetir()
    End Sub

    Private Sub btnImprimir_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, RepetirChequesViewModel).ImprimirCheques()
    End Sub

    Private Sub A2NumericBox_KeyDown(sender As Object, e As KeyEventArgs)
        Try
            If e.Key = Key.Enter Then
                CType(Me.DataContext, RepetirChequesViewModel).BuscarChequeRepetir()
            End If
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "A2NumericBox_KeyDown", Program.TituloSistema, Program.Maquina, ex, Program.RutaServicioLog)
        End Try
    End Sub

#End Region

End Class
