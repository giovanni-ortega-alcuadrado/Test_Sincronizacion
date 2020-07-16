Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports System.Collections.ObjectModel

'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: CodificacionContableView.xaml.vb
'Generado el : 09/01/2011 11:00:17
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class CodificacionContableView
    Inherits UserControl
    Private strClaseCombos As String = ""
    Private mstrDicCombosEspecificos As String = String.Empty

    Public Sub New()
        Dim objA2VM As A2UtilsViewModel
        
        Dim strModuloTesoreria As String = ""

        strClaseCombos = "CitiBank_CodificacionContable"
        mstrDicCombosEspecificos = String.Format("Combos_{0}", Me.ToString)

        Try

            objA2VM = New A2UtilsViewModel(strClaseCombos, mstrDicCombosEspecificos)
            Me.Resources.Add("A2VM", objA2VM)

        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "New (Overload)", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

        Me.DataContext = New CodificacionContableViewModel
InitializeComponent()
    End Sub

    Private Sub CodificacionContable_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        CType(Me.DataContext, CodificacionContableViewModel).NombreView = Me.ToString
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

    

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        CType(Me.DataContext, CodificacionContableViewModel).SeleccionarRegistro()
    End Sub

Private Sub txtComitenteBusqueda_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs)
        If Not (e.Key > 47 And e.Key < 58) And Not (e.Key > 95 And e.Key < 106) And Not (e.Key = 9) And Not (e.Key = 13) Then
            e.Handled = True
        End If
    End Sub

End Class


