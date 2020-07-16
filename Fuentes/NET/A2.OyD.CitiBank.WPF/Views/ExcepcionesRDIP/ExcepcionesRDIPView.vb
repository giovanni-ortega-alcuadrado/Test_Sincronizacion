Imports Telerik.Windows.Controls
Imports A2.OyD.OYDServer.RIA.Web
Imports A2Utilidades.Mensajes
Imports Microsoft.VisualBasic.CompilerServices

'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: ExcepcionesRDIPView.xaml.vb
'Generado el : 08/09/2011 13:22:05
'Propiedad de Alcuadrado S.A. 2010


Partial Public Class ExcepcionesRDIPView
    Inherits UserControl
    Private strClaseCombos As String = ""
    Private mstrDicCombosEspecificos As String = String.Empty

    Public Sub New()
        Dim objA2VM As A2UtilsViewModel
        Dim strModuloTesoreria As String = ""

        strClaseCombos = "CitiBank_ExcepcionesRDIP"
        mstrDicCombosEspecificos = String.Format("Combos_{0}", Me.ToString)

        Try
            objA2VM = New A2UtilsViewModel(strClaseCombos, mstrDicCombosEspecificos)
            Me.Resources.Add("A2VM", objA2VM)
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "New (Overload)", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

        Me.DataContext = New ExcepcionesRDIPViewModel
InitializeComponent()
    End Sub

    Private Sub ExcepcionesRDIP_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        CType(Me.DataContext, ExcepcionesRDIPViewModel).NombreView = Me.ToString
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

    'Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    '    df.ValidateItem()
    'End Sub

#Region "Buscadores ExcepcionesRDIP"
    Private Sub Buscar_finalizoBusquedaComitente(ByVal pstrClaseControl As System.String, ByVal pobjComitente As OYDUtilidades.BuscadorClientes)
        If Not IsNothing(pobjComitente) Then
            CType(Me.DataContext, ExcepcionesRDIPViewModel).cb.IDComitente = pobjComitente.IdComitente
            CType(Me.DataContext, ExcepcionesRDIPViewModel).cb.Nombre = pobjComitente.Nombre
        End If
    End Sub

    Private Sub Buscar_finalizoBusquedaEspecie(ByVal pstrClaseControl As System.String, ByVal pobjEspecie As OYDUtilidades.BuscadorEspecies)
        If Not IsNothing(pobjEspecie) Then
            CType(Me.DataContext, ExcepcionesRDIPViewModel).cb.IDEspecie = pobjEspecie.Especie
        End If
    End Sub

    
#End Region

End Class


