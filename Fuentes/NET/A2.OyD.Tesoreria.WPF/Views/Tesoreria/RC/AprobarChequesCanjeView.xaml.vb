Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes


Partial Public Class AprobarChequesCanjeView
    Inherits UserControl

#Region "Variables"

    Private mlogInicializado As Boolean = False
    Dim objA2VM As A2UtilsViewModel
    Private strClaseCombos As String = ""
    Private mstrDicCombosEspecificos As String = String.Empty

#End Region

#Region "Inicializaciones"
    Public Sub New()
        Dim objA2VM As A2UtilsViewModel
        Dim strModuloTesoreria As String = ""

        Try
            'Si el recurso no esta vacio, se remueve
            If Not String.IsNullOrEmpty(Application.Current.Resources("moduloTesoreria")) Then
                Application.Current.Resources.Remove("moduloTesoreria")
            End If

            strClaseCombos = "Tesoreria_Notas"
            mstrDicCombosEspecificos = String.Format("Combos_{0}", Me.ToString)
            Application.Current.Resources.Add("moduloTesoreria", TesoreriaViewModel.ClasesTesoreria.CE)


            objA2VM = New A2UtilsViewModel(strClaseCombos, mstrDicCombosEspecificos)
            Me.Resources.Add("A2VM", objA2VM)

            mlogInicializado = True
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "New (Overload)", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

        Me.DataContext = New AprobarChequesCanjeViewModel
InitializeComponent()
    End Sub
#End Region

#Region "Eventos"

    Private Sub btnBuscarCheques_Click(sender As Object, e As RoutedEventArgs)
        'CType(Me.DataContext, AprobarChequesCanjeViewModel).BuscarChequesPorAprobar()
        CType(Me.DataContext, AprobarChequesCanjeViewModel).VerificarParametroCanje("BUSCAR")
    End Sub

    Private Sub btnAprobar_Click(sender As Object, e As RoutedEventArgs)
        'CType(Me.DataContext, AprobarChequesCanjeViewModel).AprobarCheques()
        CType(Me.DataContext, AprobarChequesCanjeViewModel).VerificarParametroCanje("APROBAR")
    End Sub

    Private Sub btnDesaprobar_Click(sender As Object, e As RoutedEventArgs)
        'CType(Me.DataContext, AprobarChequesCanjeViewModel).DesaprobarCheques()
        CType(Me.DataContext, AprobarChequesCanjeViewModel).VerificarParametroCanje("DESAPROBAR")
    End Sub

    Private Sub SeleccionarTodos_Checked(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, AprobarChequesCanjeViewModel).Check = True
    End Sub

    Private Sub NoSeleccinarTodos_Unchecked(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, AprobarChequesCanjeViewModel).Check = False
    End Sub

    Private Sub C1NumericBox_LostFocus(sender As Object, e As RoutedEventArgs)
        Try
            Dim IDDigitado As Nullable(Of Integer) = CType(sender, A2Utilidades.A2NumericBox).Value

            If IDDigitado > 0 Then
                CType(Me.DataContext, AprobarChequesCanjeViewModel).buscarCompania(IDDigitado, "buscarCompaniaEncabezado")
            Else
                CType(Me.DataContext, AprobarChequesCanjeViewModel).NombreCompania = String.Empty
            End If

        Catch ex As Exception
            A2Utilidades.Mensajes.mostrarErrorAplicacion(Program.Usuario, "Se presentó un problema al recibir los datos de la compañia del banco", Me.ToString(), "C1NumericBox_LostFocus", Program.TituloSistema, Program.Maquina, ex)
        End Try
    End Sub

    Private Sub BuscadorGenerico_finalizoBusqueda(pstrClaseControl As String, pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            If pstrClaseControl = "compania" Then
                CType(Me.DataContext, AprobarChequesCanjeViewModel).IDCompania = pobjItem.IdItem
                CType(Me.DataContext, AprobarChequesCanjeViewModel).NombreCompania = pobjItem.Nombre
            End If
        End If
    End Sub

#End Region

End Class
