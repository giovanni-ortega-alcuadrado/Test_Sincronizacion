Imports Telerik.Windows.Controls

'Codigo generado
'Plantilla: ViewCodeBehindTemplate.tt
'Archivo: BancosView.xaml.vb
'Generado el : 08/08/2011 12:11:50
'Propiedad de Alcuadrado S.A. 2010
Imports A2.OyD.OYDServer.RIA.Web
Imports System.IO
Imports A2Utilidades.Mensajes




Partial Public Class AjustesBancariosView
    Inherits UserControl
    Private WithEvents mobjBuscadorLst As A2ComunesControl.BuscadorGenericoLista
    Private mobjVM As TesoreriaViewModel
    Dim objA2VM As A2UtilsViewModel
    Private strClaseCombos As String = ""
    Private mstrDicCombosEspecificos As String = String.Empty

    Public Sub New()
        'Dim objA2VM As A2UtilsViewModel
        Dim strModuloTesoreria As String = ""

        Try
            'Si el recurso no esta vacio, se remueve
            If Not String.IsNullOrEmpty(Application.Current.Resources("moduloTesoreria")) Then
                Application.Current.Resources.Remove("moduloTesoreria")
            End If

            strClaseCombos = "Tesoreria_Ajustesbancarios"
            mstrDicCombosEspecificos = String.Format("Combos_{0}", Me.ToString)
            ' Application.Current.Resources.Add("moduloTesoreria", TesoreriaViewModel.ClasesTesoreria.N)

            objA2VM = New A2UtilsViewModel(strClaseCombos, mstrDicCombosEspecificos)
            Me.Resources.Add("A2VM", objA2VM)

            ' mlogInicializado = True
        Catch ex As Exception
            mostrarErrorAplicacion(Program.Usuario, "Se presentó un error al inicializar el view model A2", Me.Name, "New", "New (Overload)", Program.Maquina, ex, Program.RutaServicioLog)
        End Try

        Me.DataContext = New AjustesBancariosViewModel
InitializeComponent()

    End Sub

    Private Sub AjustesBancarios_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        cm.GridTransicion = grdGridForma
cm.GridViewRegistros = datapager1
        cm.DF = df
        CType(Me.DataContext, AjustesBancariosViewModel).NombreView = Me.ToString
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
    Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
        If Not IsNothing(pobjItem) Then
            Select Case pstrClaseControl
                Case "IDBanco"
                    CType(Me.DataContext, AjustesBancariosViewModel).AjustesBancarioSelected.IDBanco = pobjItem.IdItem
                    CType(Me.DataContext, AjustesBancariosViewModel).AjustesBancarioSelected.Nombre = pobjItem.Nombre
                    'CType(Me.DataContext, AjustesBancariosViewModel).AjustesBancarioSelected.ChequeraAutomatica = pobjItem.Estado
                Case "IDBancoajuste"
                    CType(Me.DataContext, AjustesBancariosViewModel).cb.IDBanco = pobjItem.IdItem
                    CType(Me.DataContext, AjustesBancariosViewModel).cb.NombreBanco = pobjItem.Nombre
            End Select
        End If
    End Sub
    'Private Sub ValidationSummary_FocusingInvalidControl(ByVal sender As System.Object, ByVal e As System.Windows.Controls.FocusingInvalidControlEventArgs)
    '    CType(Me.DataContext, BancosViewModel).seleccionarCampoTab(e.Target.PropertyName)
    'End Sub
    ''Private Sub df_LostFocus(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles df.LostFocus
    ''    df.ValidateItem()
    ''End Sub
    'Private Sub BuscadorGenerico_finalizoBusqueda(ByVal pstrClaseControl As System.String, ByVal pobjItem As OYDUtilidades.BuscadorGenerico)
    '    If Not IsNothing(pobjItem) Then
    '        Select Case pstrClaseControl.ToLower
    '            Case "ciudades"
    '                CType(Me.DataContext, BancosViewModel).BancoSelected.IDPoblacion = CType(pobjItem.IdItem, Integer)
    '                CType(Me.DataContext, BancosViewModel).CiudadesClaseSelected.Ciudad = pobjItem.Nombre

    '                CType(Me.DataContext, BancosViewModel).BancoSelected.IDDepartamento = CType(pobjItem.IdItem, Integer)
    '                CType(Me.DataContext, BancosViewModel).CiudadesClaseSelected.Departamento = pobjItem.CodigoAuxiliar

    '                CType(Me.DataContext, BancosViewModel).BancoSelected.IDPais = CType(pobjItem.IdItem, Integer)
    '                CType(Me.DataContext, BancosViewModel).CiudadesClaseSelected.Pais = pobjItem.InfoAdicional02
    '        End Select
    '    End If
    'End Sub

End Class


